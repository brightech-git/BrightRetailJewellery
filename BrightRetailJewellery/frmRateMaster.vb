Imports System.Data.OleDb
Public Class frmRateMaster
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim objGridShower As frmGridDispDia
    Dim RATECARD_MANUAL As String = GetAdmindbSoftValue("RATECARD_MANUAL", "N")
    Dim RATEPURITY_SEP As Boolean = IIf(GetAdmindbSoftValue("RATEPURITY_SEP", "N") = "Y", True, False)
    Dim SYNC_RATE As Boolean = IIf(GetAdmindbSoftValue("SYNC_RATE", "N") = "Y", True, False)
    Dim CENTR_DB_BR As Boolean = IIf(GetAdmindbSoftValue("CENTR_DB_ALLCOSTID", "N") = "Y", True, False)
    Public Rateviewonly As Boolean = False
    Dim SMS_RATE_UPDATE As String = objGPack.GetSqlValue("SELECT ISNULL(TEMPLATE_DESC,'') AS TEMPLATE_DESC FROM " & cnAdminDb & "..SMSTEMPLATE WHERE TEMPLATE_NAME='SMS_RATE_UPDATE' AND ISNULL(ACTIVE,'Y')<>'N'", "TEMPLATE_DESC").ToString
    Dim dtCostCentre As New DataTable
    Dim TAG_DUMP As String = GetAdmindbSoftValue("TAG_DUMP", "")
    Dim Authorize As Boolean = False
    Dim _Edit As Boolean = False
    Dim RateCalc_Shortname As Boolean = IIf(GetAdmindbSoftValue("RATECALC_SHORTNAME", "N") = "Y", True, False)
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Public Sub New(ByVal Edit As Boolean)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        dtpDate.Value = GetServerDate(tran)
        funcFillGrid()
        btnUpdate.Visible = False
        btnView.Visible = False
        lblBullGold.Visible = RATEPURITY_SEP : txtBullGRate_AMT.Visible = RATEPURITY_SEP
        lblBullSil.Visible = RATEPURITY_SEP : txtBullSRate_AMT.Visible = RATEPURITY_SEP
        lblBullPlat.Visible = RATEPURITY_SEP : txtBullPRate_AMT.Visible = RATEPURITY_SEP
        gridView.EditMode = DataGridViewEditMode.EditProgrammatically
    End Sub

    Function funcNew() As Integer
        dtpDate.Value = GetServerDate(tran)
        funcFillGrid()
        dtpDate.Focus()
    End Function
    Function funcExit() As Integer
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Function
    Function funcFillGrid() As Integer
        gridView.DataSource = Nothing
        Dim dt As New DataTable
        dt.Clear()

        strSql = " SELECT RATEPER,CONVERT(VARCHAR,COSTID)AS COSTID,2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " WHERE ISNULL(ACTIVE,'Y')<>'N'"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)

        strSql = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPRATE')>0"
        strSql += vbCrLf + "      DROP TABLE TEMPTABLEDB..TEMPRATE"
        strSql += vbCrLf + "  SELECT METALID"
        strSql += vbCrLf + "  ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = R.METALID)AS METALNAME"
        strSql += vbCrLf + "  ,(SELECT TOP 1 PURITYNAME FROM " & cnAdminDb & "..PURITYMAST WHERE METALID = R.METALID AND PURITY = R.PURITY AND PURITYID NOT IN (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATGROUP = 'L'))AS PURITYNAME"
        strSql += vbCrLf + "  ,(SELECT TOP 1 SHORTNAME FROM " & cnAdminDb & "..PURITYMAST WHERE METALID = R.METALID AND PURITY = R.PURITY AND PURITYID NOT IN (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATGROUP = 'L'))AS SHORTNAME"
        strSql += vbCrLf + "  ,PURITY"
        strSql += vbCrLf + "  ,SRATE AS RATE"
        strSql += vbCrLf + "  ,(SELECT TOP 1 RATEPURITY FROM " & cnAdminDb & "..PURITYMAST WHERE METALID = R.METALID AND PURITY = R.PURITY AND PURITYID NOT IN (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATGROUP = 'L'))AS RATEPURITY"
        strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMPRATE"
        strSql += vbCrLf + "  FROM " & cnAdminDb & "..RATEMAST AS R"
        strSql += vbCrLf + "  WHERE RDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + "  AND RATEGROUP IN (SELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = R.RDATE)"
        If RATE_BRANCHWISE Then
            strSql += " AND COSTID='" & cnCostId & "'"
        End If
        strSql += vbCrLf + "  ORDER BY METALID,PURITY DESC"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " SELECT METALID"
        strSql += vbCrLf + "  ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = P.METALID)AS METALNAME"
        strSql += vbCrLf + "  ,(SELECT TOP 1 PURITYNAME FROM " & cnAdminDb & "..PURITYMAST WHERE METALID = P.METALID AND PURITY = P.PURITY  AND PURITYID NOT IN (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATGROUP = 'L'))AS PURITYNAME"
        strSql += vbCrLf + "  ,(SELECT TOP 1 SHORTNAME FROM " & cnAdminDb & "..PURITYMAST WHERE METALID = P.METALID AND PURITY = P.PURITY  AND PURITYID NOT IN (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATGROUP = 'L'))AS SHORTNAME"
        strSql += vbCrLf + "  ,PURITY"
        If Authorize Then
            strSql += vbCrLf + " ,NULL OTHER_EXCH,NULL OTHER_CASH,NULL OWN_EXCH,NULL OWN_CASH"
        End If
        strSql += vbCrLf + "  ,ISNULL((SELECT TOP 1 ISNULL(RATE,0) FROM TEMPTABLEDB..TEMPRATE WHERE METALID = P.METALID AND PURITY = P.PURITY),0) AS RATE"
        strSql += vbCrLf + "  ,(SELECT TOP 1 RATEPURITY FROM " & cnAdminDb & "..PURITYMAST WHERE METALID = P.METALID AND PURITY = P.PURITY  AND PURITYID NOT IN (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATGROUP = 'L'))AS RATEPURITY"
        strSql += vbCrLf + "  FROM"
        strSql += vbCrLf + "  ("
        strSql += vbCrLf + "  SELECT DISTINCT METALID"
        strSql += vbCrLf + "  ,PURITY FROM "
        strSql += vbCrLf + "  " & cnAdminDb & "..PURITYMAST"
        strSql += vbCrLf + "  WHERE METALID NOT IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE ISNULL(ACTIVE,'Y') ='N')"
        'strsql += vbcrlf + "  WHERE METALID NOT IN ('T')"
        strSql += vbCrLf + "  )P"
        strSql += vbCrLf + "  ORDER BY METALID,PURITY DESC"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)

        strSql = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPRATE')>0"
        strSql += vbCrLf + "      DROP TABLE TEMPTABLEDB..TEMPRATE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        For cnt As Integer = 1 To dt.Rows.Count - 1
            If dt.Rows(cnt - 1).Item("METALID").ToString = dt.Rows(cnt).Item("METALID").ToString Then
                dt.Rows(cnt).Item("METALNAME") = ""
            End If
        Next
        gridView.DataSource = dt
        FuncGridHeaderNew()
        With gridView.Columns("MetalId")
            .ReadOnly = True
            .Visible = False
            .HeaderText = "METAL"
            .Resizable = DataGridViewTriState.False
        End With
        With gridView.Columns("MetalName")
            .ReadOnly = True
            .HeaderText = "METAL"
            .Resizable = DataGridViewTriState.False
        End With
        With gridView.Columns("PurityName")
            .ReadOnly = True
            If Authorize Then
                .Width = 200
                .MinimumWidth = 200
            Else
                .Width = 200
                .MinimumWidth = 265
            End If
            .HeaderText = "PURITY NAME"
            .Resizable = DataGridViewTriState.False
        End With
        gridView.Columns("RATEPurity").Visible = False
        If gridView.Columns.Contains("SHORTNAME") Then gridView.Columns("SHORTNAME").Visible = False
        With gridView.Columns("Purity")
            .ReadOnly = True
            .MinimumWidth = 60
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Width = 60
            .HeaderText = "PURITY"
            .Resizable = DataGridViewTriState.False
        End With
        If Authorize Then
            With gridView.Columns("OTHER_EXCH")
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "0.00"
                .MinimumWidth = 60
                .Width = 60
                .HeaderText = "EXCH"
                .Resizable = DataGridViewTriState.False
            End With
            With gridView.Columns("OTHER_CASH")
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "0.00"
                .MinimumWidth = 60
                .Width = 60
                .HeaderText = "CASH"
                .Resizable = DataGridViewTriState.False
            End With
            With gridView.Columns("OWN_EXCH")
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "0.00"
                .MinimumWidth = 60
                .Width = 60
                .HeaderText = "EXCH"
                .Resizable = DataGridViewTriState.False
            End With
            With gridView.Columns("OWN_CASH")
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "0.00"
                .MinimumWidth = 60
                .Width = 60
                .HeaderText = "CASH"
                .Resizable = DataGridViewTriState.False
            End With
        End If
        With gridView.Columns("Rate")
            If Not RATEPURITY_SEP = True Then
                If _Edit = True Then
                    .ReadOnly = False
                Else
                    .ReadOnly = True
                End If
            Else
                .ReadOnly = True
            End If
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .DefaultCellStyle.Format = "0.00"
            .MinimumWidth = 60
            .Width = 60
            .HeaderText = "RATE"
            .Resizable = DataGridViewTriState.False
        End With
        'gridView.SelectionMode = DataGridViewSelectionMode.CellSelect
        If RATEPURITY_SEP Then txtBullGRate_AMT.Focus()
    End Function
    Function funcSave() As Integer
        If RATE_BRANCHWISE And CENTR_DB_BR And cnCostId <> cnHOCostId Then MsgBox("Master Entry Cannot allowed at Location...", MsgBoxStyle.Information) : Exit Function
        Dim RateGroup As Integer
        RateGroup = objGPack.GetMax("RateGroup", "RateMast", cnAdminDb)
        Dim tran As OleDbTransaction = Nothing
        Try
            tran = cn.BeginTransaction
            If RATE_BRANCHWISE And CENTR_DB_BR Then
                For I As Integer = 0 To dtCostCentre.Rows.Count - 1
                    Dim CostId As String = dtCostCentre.Rows(I).Item("COSTID").ToString
                    Dim Rateper As Decimal = Val(dtCostCentre.Rows(I).Item("RATEPER").ToString)
                    For cnt As Integer = 0 To gridView.Rows.Count - 1
                        Dim sRate As Decimal = gridView.Item("RATE", cnt).Value.ToString
                        If Rateper <> 0 Then
                            sRate = sRate + (sRate * Rateper) / 100
                            sRate = CalcRoundoffAmt(sRate, "F")
                        End If
                        strSql = " Insert into " & cnAdminDb & "..RateMast"
                        strSql += " ("
                        strSql += " MetalId"
                        strSql += " ,Rdate"
                        strSql += " ,Rategroup"
                        strSql += " ,Purity"
                        strSql += " ,Srate"
                        strSql += " ,Prate"
                        strSql += " ,UserId"
                        strSql += " ,Updated"
                        strSql += " ,UpTime"
                        strSql += " ,CostId"
                        strSql += " )values("
                        strSql += " '" & gridView.Item("MetalId", cnt).Value.ToString & "'" 'MetalId
                        strSql += " ,'" & Format(dtpDate.Value.Date, "yyyy-MM-dd") & "'" 'Rdate
                        strSql += " ," & RateGroup & "" 'Rategroup
                        strSql += " ,'" & gridView.Item("Purity", cnt).Value.ToString & "'" 'Purity
                        strSql += " ,'" & sRate & "'" 'Srate
                        strSql += " ,'" & sRate & "'" 'Prate
                        strSql += " ," & userId & "" 'UserId
                        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Updated
                        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UpTime
                        strSql += " ,'" & CostId & "'" 'CostId
                        strSql += " )"
                        cmd = New OleDbCommand(strSql, cn, tran)
                        cmd.ExecuteNonQuery()
                    Next
                Next
            Else
                Dim TAG_DUMP_DBID As String
                If TAG_DUMP <> "" Then
                    Dim TAG() As String
                    TAG = TAG_DUMP.Split(",")
                    If TAG.Length = 7 Then
                        TAG_DUMP_DBID = TAG(0).ToString
                    End If
                End If
                For cnt As Integer = 0 To gridView.Rows.Count - 1
                    strSql = " Insert into " & cnAdminDb & "..RateMast"
                    strSql += " ("
                    strSql += " MetalId"
                    strSql += " ,Rdate"
                    strSql += " ,Rategroup"
                    strSql += " ,Purity"
                    strSql += " ,Srate"
                    strSql += " ,Prate"
                    strSql += " ,UserId"
                    strSql += " ,Updated"
                    strSql += " ,UpTime"
                    If SYNC_RATE Then strSql += " ,CostId"
                    strSql += " )values("
                    strSql += " '" & gridView.Item("MetalId", cnt).Value.ToString & "'" 'MetalId
                    strSql += " ,'" & Format(dtpDate.Value.Date, "yyyy-MM-dd") & "'" 'Rdate
                    strSql += " ," & RateGroup & "" 'Rategroup
                    strSql += " ,'" & gridView.Item("Purity", cnt).Value.ToString & "'" 'Purity
                    strSql += " ,'" & gridView.Item("Rate", cnt).Value.ToString & "'" 'Srate
                    strSql += " ,'" & gridView.Item("Rate", cnt).Value.ToString & "'" 'Prate
                    strSql += " ," & userId & "" 'UserId
                    strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Updated
                    strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UpTime
                    If SYNC_RATE Then strSql += " ,'" & cnCostId & "'" 'CostId
                    strSql += " )"
                    If SYNC_RATE Then
                        'ExecQuery(SyncMode.Master, strSql, cn, tran, cnCostId)
                        ExecQuery(SyncMode.Master, strSql, cn, tran, cnCostId)
                    Else
                        cmd = New OleDbCommand(strSql, cn, tran)
                        cmd.ExecuteNonQuery()
                        If TAG_DUMP_DBID <> "" Then
                            cmd = New OleDbCommand(Replace(strSql, cnAdminDb & "..RateMast", TAG_DUMP_DBID & "ADMINDB..RateMast"), cn, tran)
                            cmd.ExecuteNonQuery()
                        End If
                    End If
                Next
            End If
            tran.Commit()
            funcFillGrid()
            MsgBox("Rate Updated..")
            funcSendAlertSms(RateGroup)
        Catch ex As Exception
            If Not (tran Is Nothing) Then
                tran.Rollback()
            End If
            MsgBox("RollBacked..")
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function

    Private Sub funcSendAlertSms(ByVal _rategrp As Integer)
        If ALERTBASE_MENU Then
            strSql = "SELECT TOP 1 1 FROM " & cnAdminDb & "..ALERTTRAN WHERE MENUID IN("
            strSql += vbCrLf + " SELECT MENUID FROM " & cnAdminDb & "..PRJMENUS WHERE ACCESSID LIKE'" & Me.Name & "%')"
            strSql += vbCrLf + " AND _EDIT='Y'"
        Else
            strSql = "SELECT TOP 1 1 FROM " & cnAdminDb & "..ALERTTRAN WHERE TABLENAME='RATEMAST'"
            strSql += vbCrLf + " AND _EDIT='Y'"
        End If
        If objGPack.GetSqlValue(strSql, , "-1") = "-1" Then Exit Sub
        Dim GoldRate As String = Format(Val(GetRate(GetEntryDate(GetServerDate), objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE PURITYID IN (SELECT TOP 1 PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE PURITY BETWEEN 91.6 AND 92 AND METALID = 'G' AND METALTYPE = 'O' ORDER BY PURITY DESC)"))), "0.00")
        Dim SilverRate As String = Format(Val(GetRate(GetEntryDate(GetServerDate), objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE PURITYID IN (SELECT TOP 1 PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE PURITY BETWEEN 91.6 AND 92 AND METALID = 'S' AND METALTYPE = 'O' ORDER BY PURITY DESC)"))), "0.00")
        Dim TempMsg As String = ""
        TempMsg = SMS_RATE_UPDATE
        TempMsg = Replace(SMS_RATE_UPDATE, vbCrLf, "")
        TempMsg = Replace(TempMsg, "<GOLDRATE>", GoldRate)
        TempMsg = Replace(TempMsg, "<SILVERRATE>", SilverRate)
        TempMsg = Replace(TempMsg, "<GOLD>", GoldRate)
        TempMsg = Replace(TempMsg, "<SILVER>", SilverRate)
        TempMsg = Replace(TempMsg, "<USERNAME>", cnUserName)
        TempMsg = Replace(TempMsg, "<DATE>", dtpDate.Value.ToString("dd-MM-yyyy"))

        strSql = vbCrLf + " SELECT "
        strSql += vbCrLf + " M.METALNAME METAL"
        strSql += vbCrLf + " ,(SELECT TOP 1 PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE METALID = R.METALID AND PURITY = R.PURITY  AND PURITYID NOT IN (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATGROUP = 'L'))AS PURITY"
        strSql += vbCrLf + " ,R.SRATE AS RATE"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..RATEMAST AS R"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..METALMAST AS M ON M.METALID = R.METALID"
        strSql += vbCrLf + " WHERE R.RDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " AND RATEGROUP='" & _rategrp & "'"
        strSql += vbCrLf + " ORDER BY RATEGROUP,METAL,R.PURITY DESC"
        Dim dtRateView As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtRateView)
        If dtRateView.Rows.Count > 0 Then
            For Each Raterow As DataRow In dtRateView.Rows
                TempMsg = Replace(TempMsg, "<" & Raterow("PURITY").ToString & ">", Raterow("RATE").ToString)
            Next
        End If

        strSql = " SELECT  DISTINCT B.GROUPMOBILES FROM " & cnAdminDb & "..ALERTTRAN A "
        strSql += " LEFT JOIN " & cnAdminDb & "..ALERTGROUP B ON A.GROUPID=B.GROUPID "
        Dim dtAlert As New DataTable
        cmd = New OleDbCommand(strSql, cn, tran)
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtAlert)
        If dtAlert.Rows.Count > 0 Then
            For I As Integer = 0 To dtAlert.Rows.Count - 1
                Dim Mobiles() As String = dtAlert.Rows(I).Item("GROUPMOBILES").ToString.Split(",")
                If Not Mobiles Is Nothing Then
                    For J As Integer = 0 To Mobiles.Length - 1
                        If SmsSend(TempMsg, Mobiles(J).ToString) = False Then Exit For
                    Next
                End If
            Next
        End If
    End Sub

    Private Sub frmRateMaster_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
        End If
    End Sub

    Private Sub frmRateMaster_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView.Focused Then
                Exit Sub
            End If
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmRateMaster_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If RATEPURITY_SEP = True Then
            Authorize = BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Authorize, False)
            _Edit = BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit, False)
        Else
            Authorize = True
            '_Edit = True
            _Edit = BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit, False)
        End If

        gridView.BorderStyle = BorderStyle.None
        gridView.SelectionMode = DataGridViewSelectionMode.CellSelect
        gridView.BackgroundColor = Color.WhiteSmoke
        gridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        gridView.ColumnHeadersHeight = 25
        gridView.RowTemplate.Height = 25
        gridView.RowTemplate.Resizable = DataGridViewTriState.False
        gridView.DefaultCellStyle.SelectionBackColor = Color.White
        gridView.DefaultCellStyle.SelectionForeColor = Color.Black
        dtpDate.Value = GetEntryDate(GetServerDate)
        lblBullGold.Visible = RATEPURITY_SEP And _Edit : txtBullGRate_AMT.Visible = RATEPURITY_SEP And _Edit
        lblBullSil.Visible = RATEPURITY_SEP And _Edit : txtBullSRate_AMT.Visible = RATEPURITY_SEP And _Edit
        lblBullPlat.Visible = RATEPURITY_SEP And _Edit : txtBullPRate_AMT.Visible = RATEPURITY_SEP And _Edit
        gridViewHead.Visible = RATEPURITY_SEP
        gridviewhead1.Visible = RATEPURITY_SEP
        If RATEPURITY_SEP = False Then
            Authorize = False
            Me.Width = 510
            Me.Height = 405
        Else
            Me.Width = 685
            Me.Height = 530 '405
        End If
        If RATEPURITY_SEP = True And Authorize = False Then
            gridViewHead.Visible = False
            gridviewhead1.Visible = False
        End If

        funcFillGrid()
        dtpDate.Select()
    End Sub
    Private Sub gridView_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellEndEdit
        If e.RowIndex >= 0 Then
            If e.RowIndex < gridView.RowCount - 1 And RATECARD_MANUAL = "N" Then
                If e.ColumnIndex = gridView.Columns("RATE").Index _
                    And e.RowIndex > 0 Then
                    If gridView.Item("MetalId", e.RowIndex).Value.ToString = gridView.Item("MetalId", e.RowIndex - 1).Value.ToString _
                    And Val(gridView.Item("Rate", e.RowIndex - 1).Value.ToString) > 0 _
                    And Val(gridView.Item("Rate", e.RowIndex).Value.ToString) > Val(gridView.Item("Rate", e.RowIndex - 1).Value.ToString) Then
                        gridView.Item(e.ColumnIndex, e.RowIndex).Value = ""
                        Exit Sub
                    End If
                End If
            End If
        End If
        funcGenAutoRate(e.RowIndex)
    End Sub
    Function funcGenAutoRate(ByVal curRow As Integer) As Integer
        If Not curRow <> gridView.Rows.Count - 1 Then
            Exit Function
        End If
        If Val(gridView.Item("Purity", curRow).Value.ToString) = 100 Then
            Dim _trate As Double = 0
            For cnt As Integer = curRow + 1 To gridView.Rows.Count - 1
                If gridView.Item("MetalId", cnt).Value.ToString = gridView.Item("MetalId", curRow).Value.ToString Then
                    If RateCalc_Shortname Then
                        If gridView.Item("SHORTNAME", cnt).Value.ToString.Contains("%") Then
                            _trate = Math.Round((Val(gridView.Item("Rate", curRow).Value.ToString) * Val(gridView.Item("SHORTNAME", cnt).Value.ToString)) / 100, 0)
                            gridView.Item("Rate", cnt).Value = Math.Round(Val(_trate), 0)
                        Else
                            If gridView.Item("SHORTNAME", cnt).Value.ToString.Contains("*") And Val(_trate) > 0 Then
                                Dim _tempshortname() As String
                                _tempshortname = gridView.Item("SHORTNAME", cnt).Value.ToString.Split("*")
                                If _tempshortname.Length > 1 Then
                                    gridView.Item("Rate", cnt).Value = Math.Round((Val(_trate) / Val(_tempshortname(0).ToString)) * Val(_tempshortname(1).ToString), 0)
                                Else
                                    gridView.Item("Rate", cnt).Value = (Val(gridView.Item("Rate", curRow).Value.ToString) * Val(gridView.Item("Purity", cnt).Value.ToString)) / 100
                                End If

                            Else
                                gridView.Item("Rate", cnt).Value = (Val(gridView.Item("Rate", curRow).Value.ToString) * Val(gridView.Item("Purity", cnt).Value.ToString)) / 100
                            End If
                        End If
                    Else
                        gridView.Item("Rate", cnt).Value = (Val(gridView.Item("Rate", curRow).Value.ToString) * Val(gridView.Item("Purity", cnt).Value.ToString)) / 100
                    End If
                End If
            Next
        End If
    End Function

    Private Sub gridView_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles gridView.DataError
        MsgBox("Incorrect Rate", MsgBoxStyle.Information)
    End Sub
    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        For Each dgvRow As DataGridViewRow In gridView.Rows
            Dim metal As String = objGPack.GetSqlValue("SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = '" & dgvRow.Cells("METALID").Value.ToString & "'")
            If Val(dgvRow.Cells("PURITY").Value.ToString) = 91.6 And dgvRow.Cells("METALID").Value.ToString = "G" Then
                If Val(dgvRow.Cells("RATE").Value.ToString) = 0 Then
                    MsgBox("91.6 " & metal & " RATE SHOULD NOT EMPTY", MsgBoxStyle.Information)
                    Exit Sub
                End If
            ElseIf Val(dgvRow.Cells("PURITY").Value.ToString) = 91.6 And dgvRow.Cells("METALID").Value.ToString = "S" Then
                If Val(dgvRow.Cells("RATE").Value.ToString) = 0 Then
                    MsgBox("91.6 " & metal & " RATE SHOULD NOT EMPTY", MsgBoxStyle.Information)
                    Exit Sub
                End If
            End If
        Next
        funcSave()
        dtpDate.Focus()
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        funcExit()
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        btnUpdate_Click(Me, New EventArgs)
    End Sub

    Private Sub gridView_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles gridView.EditingControlShowing
        'This event is fired when the user tries to edit the content of a cell: 
        If Val(gridView.CurrentRow.Cells("RATE").Value) <> 0 Then
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit) Then gridView.CurrentRow.Cells("RATE").ReadOnly = True : Exit Sub
        Else
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then gridView.CurrentRow.Cells("RATE").ReadOnly = True : Exit Sub
        End If
        If Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("RATE").Index And Not e.Control Is Nothing Then
            Dim tb As TextBox = CType(e.Control, TextBox)
            '---add an event handler to the TextBox control---
            AddHandler tb.KeyPress, AddressOf TextBox_KeyPress
        End If
        'If Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("ProducedQty").Index And Not e.Control Is Nothing Then
        '    Dim tb As TextBox = CType(e.Control, TextBox)
        '    '---add an event handler to the TextBox control---
        '    AddHandler tb.KeyPress, AddressOf TextBox_KeyPress
        'End If
    End Sub

    Private Sub TextBox_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If e.KeyChar = "." And CType(sender, TextBox).Text.Contains(".") Then
            e.Handled = True
            Return
            Exit Sub
        End If
        Select Case e.KeyChar
            Case "+", "-", "0" To "9", ChrW(Keys.Back), ".",
            ChrW(Keys.Enter), ChrW(Keys.Escape)
            Case Else
                e.Handled = True
                MsgBox("Digits only Allowed 0 to 9", MsgBoxStyle.Information)
                CType(sender, TextBox).Focus()
                Return
        End Select
        If CType(sender, TextBox).Text.Contains(".") Then
            Dim dotPos As Integer = InStr(CType(sender, TextBox).Text, ".", CompareMethod.Text)
            Dim sp() As String = CType(sender, TextBox).Text.Split(".")
            Dim curPos As Integer = CType(sender, TextBox).SelectionStart
            If sp.Length >= 2 Then
                If curPos >= dotPos Then
                    If sp(1).Length > 1 Then
                        e.Handled = True
                        Return
                    End If
                End If
            End If
        End If

        ''---if textbox is empty and user pressed a decimal char---
        'If CType(sender, TextBox).Text = String.Empty And e.KeyChar = Chr(46) Then
        '    e.Handled = True
        '    Return
        'End If
        ''---if textbox already has a decimal point---
        'If CType(sender, TextBox).Text.Contains(Chr(46)) And e.KeyChar = Chr(46) Then
        '    e.Handled = True
        '    Return
        'End If
        ''---if the key pressed is not a valid decimal number---
        'If (Not (Char.IsDigit(e.KeyChar) Or Char.IsControl(e.KeyChar) Or (e.KeyChar = Chr(46)))) Then
        '    e.Handled = True
        'End If
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        gridView.DefaultCellStyle.SelectionBackColor = Color.LightGoldenrodYellow
        gridView.DefaultCellStyle.SelectionForeColor = Color.Black
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Val(gridView.CurrentRow.Cells("Rate").Value.ToString) <> 0 Then
                If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit) Then gridView.CurrentRow.Cells("Rate").ReadOnly = True : Exit Sub
            Else
                If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then gridView.CurrentRow.Cells("Rate").ReadOnly = True : Exit Sub
            End If
            e.Handled = True
            If gridView.RowCount > 0 Then
                gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells("RATE")
            End If
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Me.SelectNextControl(gridView, True, True, True, True)
        End If
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        gridView.DefaultCellStyle.SelectionBackColor = Color.White
        gridView.DefaultCellStyle.SelectionForeColor = Color.Black
    End Sub

    Private Sub gridView_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.SelectionChanged
        Try
            If Not gridView.RowCount > 0 Then Exit Sub
            gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells("RATE")
        Catch ex As Exception

        End Try
    End Sub

    Private Sub dtpDate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpDate.LostFocus
        funcFillGrid()
    End Sub

    Private Sub DataGridView_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
        If e.KeyCode = Keys.Delete Then
            Dim dgv As DataGridView = CType(sender, DataGridView)
            If Not dgv.RowCount > 0 Then Exit Sub
            If dgv.CurrentRow Is Nothing Then Exit Sub
            Dim RateGroup As String = dgv.CurrentRow.Cells("RATEGROUP").Value.ToString
            Dim dv As DataView
            dv = CType(dgv.DataSource, DataTable).DefaultView
            dv.RowFilter = "RATEGROUP <> '" & RateGroup & "'"
            dgv.DataSource = dv.ToTable
            DgvFormat(dgv)
            strSql = " DELETE FROM " & cnAdminDb & "..RATEMAST"
            strSql += " WHERE RDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "'"
            strSql += " AND RATEGROUP = " & Val(RateGroup) & ""
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click

        strSql = vbCrLf + " SELECT "
        strSql += vbCrLf + " M.METALNAME METAL"
        strSql += vbCrLf + " ,(SELECT TOP 1 PURITYNAME FROM " & cnAdminDb & "..PURITYMAST WHERE METALID = R.METALID AND PURITY = R.PURITY  AND PURITYID NOT IN (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATGROUP = 'L'))AS PURITY"
        strSql += vbCrLf + " ,R.SRATE AS RATE"
        strSql += vbCrLf + " ,RATEGROUP"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..RATEMAST AS R"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..METALMAST AS M ON M.METALID = R.METALID"
        strSql += vbCrLf + " WHERE R.RDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ORDER BY RATEGROUP,METAL,R.PURITY DESC"
        Dim dtRateView As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtRateView)
        If Not dtRateView.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "RATE VIEW"
        Dim tit As String = " RATE VIEW ON " & dtpDate.Text
        AddHandler objGridShower.gridView.KeyDown, AddressOf DataGridView_KeyDown
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtRateView)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = True
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        objGridShower.gridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        'DataGridView_SummaryFormatting(objGridShower.gridView)
        objGridShower.formuser = userId
        objGridShower.Show()
        With objGridShower.gridView
            .Columns("METAL").Width = 100
            .Columns("PURITY").Width = 250
            .Columns("RATE").Width = 120
            .Columns("RATEGROUP").Visible = False

            .Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End With
        DgvFormat(objGridShower.gridView)

        objGridShower.FormReLocation = True
        objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("METAL")))
    End Sub

    Private Sub DgvFormat(ByVal Dgv As DataGridView)
        With Dgv
            Dim RateGroup As String = ""
            Dim Metal As String = ""
            Dim RowColor As Color = Color.White
            For cnt As Integer = 0 To .RowCount - 1
                If RateGroup <> .Rows(cnt).Cells("RATEGROUP").Value.ToString Then
                    RowColor = IIf(RowColor = Color.White, Color.AliceBlue, Color.White)
                    Metal = ""
                    RateGroup = .Rows(cnt).Cells("RATEGROUP").Value.ToString
                End If
                If Metal <> .Rows(cnt).Cells("METAL").Value.ToString Then
                    Metal = .Rows(cnt).Cells("METAL").Value.ToString
                Else
                    .Rows(cnt).Cells("METAL").Value = DBNull.Value
                End If
                .Rows(cnt).DefaultCellStyle.BackColor = RowColor
            Next
        End With
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        btnView_Click(Me, New EventArgs)
    End Sub
    Private Sub txtBullRate_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtBullGRate_AMT.KeyPress
        'If e.KeyChar = Chr(Keys.Enter) Then txtBullSRate_AMT.Focus()

    End Sub

    Private Sub calcratePure()
        If Authorize = False Then Exit Sub
        For cnt As Integer = 0 To gridView.Rows.Count - 1
            Dim Temprate As Double = 0
            Dim temppurityname As String = ""
            Temprate = gridView.Item("RATE", cnt).Value
            strSql = " SELECT PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYNAME= '" & gridView.Item("PURITYNAME", cnt).Value & "'"
            temppurityname = GetSqlValue(cn, strSql)
            strSql = " select PURITYID,OWNPURLESSPER,OWNPURLESSAMT,OTHPURLESSPER,OTHPURLESSAMT,OWNEXLESSPER,OWNEXLESSAMT,OTHEXLESSPER,OTHEXLESSAMT,ISNULL(RATEGET,'Y') AS RATEGET"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTYPE  WHERE PURITYID = '" & temppurityname & "'" ' AND RATEGET = 'Y'"
            Dim dtItemType As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtItemType)
            If dtItemType.Rows.Count > 0 And Val(Temprate) <> 0 Then
                If Val(dtItemType.Rows(0).Item("OTHEXLESSPER").ToString) <> 0 Then
                    gridView.Item("OTHER_EXCH", cnt).Value = Math.Round((Temprate * Val(dtItemType.Rows(0).Item("OTHEXLESSPER").ToString)) / 100, 2)
                ElseIf Val(dtItemType.Rows(0).Item("OTHEXLESSAMT").ToString) <> 0 Then
                    gridView.Item("OTHER_EXCH", cnt).Value = Math.Round((Temprate - Val(dtItemType.Rows(0).Item("OTHEXLESSAMT").ToString)), 2)
                Else
                    gridView.Item("OTHER_EXCH", cnt).Value = Temprate
                End If

                If Val(dtItemType.Rows(0).Item("OTHPURLESSPER").ToString) <> 0 Then
                    gridView.Item("OTHER_CASH", cnt).Value = Math.Round((Temprate * Val(dtItemType.Rows(0).Item("OTHPURLESSPER").ToString)) / 100, 2)
                ElseIf Val(dtItemType.Rows(0).Item("OTHPURLESSAMT").ToString) <> 0 Then
                    gridView.Item("OTHER_CASH", cnt).Value = Math.Round((Temprate - Val(dtItemType.Rows(0).Item("OTHPURLESSAMT").ToString)), 2)
                Else
                    gridView.Item("OTHER_CASH", cnt).Value = Temprate
                End If

                If Val(dtItemType.Rows(0).Item("OWNEXLESSPER").ToString) <> 0 Then
                    gridView.Item("OWN_EXCH", cnt).Value = Math.Round((Temprate * Val(dtItemType.Rows(0).Item("OWNEXLESSPER").ToString)) / 100, 2)
                ElseIf Val(dtItemType.Rows(0).Item("OWNEXLESSAMT").ToString) <> 0 Then
                    gridView.Item("OWN_EXCH", cnt).Value = Math.Round((Temprate - Val(dtItemType.Rows(0).Item("OWNEXLESSAMT").ToString)), 2)
                Else
                    gridView.Item("OWN_EXCH", cnt).Value = Temprate
                End If

                If Val(dtItemType.Rows(0).Item("OWNPURLESSPER").ToString) <> 0 Then
                    gridView.Item("OWN_CASH", cnt).Value = Math.Round((Temprate * Val(dtItemType.Rows(0).Item("OWNPURLESSPER").ToString)) / 100, 2)
                ElseIf Val(dtItemType.Rows(0).Item("OWNPURLESSAMT").ToString) <> 0 Then
                    gridView.Item("OWN_CASH", cnt).Value = Math.Round((Temprate - Val(dtItemType.Rows(0).Item("OWNPURLESSAMT").ToString)), 2)
                Else
                    gridView.Item("OWN_CASH", cnt).Value = Temprate
                End If
            End If

        Next
    End Sub
    Private Sub calcratepurity()
        If Rateviewonly Then Exit Sub
        If Not _Edit Then Exit Sub
        For cnt As Integer = 0 To gridView.Rows.Count - 1
            If gridView.Item("MetalId", cnt).Value.ToString = "G" Then
                gridView.Item("Rate", cnt).Value = Math.Round((Val(txtBullGRate_AMT.Text) * Val(gridView.Item("RatePurity", cnt).Value.ToString)) / 100)
                gridView.Item("Rate", cnt).ReadOnly = True
                If Authorize = True Then
                    gridView.Item("OTHER_EXCH", cnt).ReadOnly = True
                    gridView.Item("OTHER_CASH", cnt).ReadOnly = True
                    gridView.Item("OWN_EXCH", cnt).ReadOnly = True
                    gridView.Item("OWN_CASH", cnt).ReadOnly = True
                End If
            ElseIf gridView.Item("MetalId", cnt).Value.ToString = "S" Then
                gridView.Item("Rate", cnt).Value = Math.Round((Val(txtBullSRate_AMT.Text) * Val(gridView.Item("RatePurity", cnt).Value.ToString)) / 100, 2)
                gridView.Item("Rate", cnt).ReadOnly = True
                If Authorize = True Then
                    gridView.Item("OTHER_EXCH", cnt).ReadOnly = True
                    gridView.Item("OTHER_CASH", cnt).ReadOnly = True
                    gridView.Item("OWN_EXCH", cnt).ReadOnly = True
                    gridView.Item("OWN_CASH", cnt).ReadOnly = True
                End If
            ElseIf gridView.Item("MetalId", cnt).Value.ToString = "P" Then
                gridView.Item("Rate", cnt).Value = Math.Round((Val(txtBullPRate_AMT.Text) * Val(gridView.Item("RatePurity", cnt).Value.ToString)) / 100, 2)
                gridView.Item("Rate", cnt).ReadOnly = True
                If Authorize = True Then
                    gridView.Item("OTHER_EXCH", cnt).ReadOnly = True
                    gridView.Item("OTHER_CASH", cnt).ReadOnly = True
                    gridView.Item("OWN_EXCH", cnt).ReadOnly = True
                    gridView.Item("OWN_CASH", cnt).ReadOnly = True
                End If
            End If
        Next
        calcratePure()
    End Sub

    Private Sub txtBullSRate_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtBullSRate_AMT.KeyPress
        ' If e.KeyChar = Chr(Keys.Enter) Then calcratepurity()
    End Sub

    Private Sub txtBullPRate_AMT_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtBullPRate_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then calcratepurity()
    End Sub
    Public Function FuncGridHeaderNew() As Integer
        If Authorize = False Then Exit Function
        calcratePure()
        Try
            Dim dtMergeHeader1 As New DataTable("~MERGEHEADER")
            With dtMergeHeader1
                .Columns.Add("METALNAME~PURITYNAME~PURITY", GetType(String))
                .Columns.Add("OTHER_EXCH~OTHER_CASH", GetType(String))
                .Columns.Add("OWN_EXCH~OWN_CASH", GetType(String))
                .Columns.Add("RATE", GetType(String))
                .Columns.Add("SCROLL", GetType(String))

                .Columns("METALNAME~PURITYNAME~PURITY").Caption = ""
                .Columns("OTHER_EXCH~OTHER_CASH").Caption = "OTHER"
                .Columns("OWN_EXCH~OWN_CASH").Caption = "OWN"
                .Columns("RATE").Caption = ""
                .Columns("Scroll").Caption = ""
            End With
            gridviewhead1.DataSource = dtMergeHeader1
            Dim dtMergeHeader As New DataTable("~MERGEHEADER")
            With dtMergeHeader
                .Columns.Add("METALNAME~PURITYNAME~PURITY", GetType(String))
                .Columns.Add("OTHER_EXCH~OTHER_CASH~OWN_EXCH~OWN_CASH", GetType(String))
                .Columns.Add("RATE", GetType(String))
                .Columns.Add("SCROLL", GetType(String))

                .Columns("METALNAME~PURITYNAME~PURITY").Caption = ""
                .Columns("OTHER_EXCH~OTHER_CASH~OWN_EXCH~OWN_CASH").Caption = "BUYING"
                .Columns("RATE").Caption = "SELLING"
                .Columns("Scroll").Caption = ""
            End With
            gridViewHead.DataSource = dtMergeHeader
            funcGridHeaderStyle()
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Function
    Function funcGridHeaderStyle() As Integer
        If Authorize = False Then Exit Function
        If gridviewhead1.Columns.Count = 0 Then Exit Function
        If gridViewHead.Columns.Count = 0 Then Exit Function
        With gridviewhead1
            .Columns("SCROLL").HeaderText = ""
            With .Columns("METALNAME~PURITYNAME~PURITY")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Width = gridView.Columns("METALNAME").Width + gridView.Columns("PURITYNAME").Width + gridView.Columns("PURITY").Width
                .HeaderText = ""
            End With
            With .Columns("OTHER_EXCH~OTHER_CASH")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Width = gridView.Columns("OTHER_EXCH").Width + gridView.Columns("OTHER_CASH").Width
                .HeaderText = "OTHER"
            End With
            With .Columns("OWN_EXCH~OWN_CASH")
                .HeaderText = "OWN"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                If RATEPURITY_SEP = True Then
                    gridView.Columns("OTHER_EXCH").DefaultCellStyle.BackColor = Color.LightGreen
                    gridView.Columns("OTHER_CASH").DefaultCellStyle.BackColor = Color.LightGreen
                    gridView.Columns("OWN_EXCH").DefaultCellStyle.BackColor = Color.LightGreen
                    gridView.Columns("OWN_CASH").DefaultCellStyle.BackColor = Color.LightGreen
                End If
                .Width = gridView.Columns("OWN_EXCH").Width + gridView.Columns("OWN_CASH").Width
            End With
            With .Columns("RATE")
                .HeaderText = ""
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                If RATEPURITY_SEP = True Then
                    gridView.Columns("RATE").DefaultCellStyle.BackColor = Color.LightSalmon
                End If
                .Width = gridView.Columns("RATE").Width
            End With
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        End With

        With gridViewHead
            .Columns("SCROLL").HeaderText = ""
            With .Columns("METALNAME~PURITYNAME~PURITY")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Width = gridView.Columns("METALNAME").Width + gridView.Columns("PURITYNAME").Width + gridView.Columns("PURITY").Width
                .HeaderText = ""
            End With
            With .Columns("OTHER_EXCH~OTHER_CASH~OWN_EXCH~OWN_CASH")
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Width = gridView.Columns("OTHER_EXCH").Width + gridView.Columns("OTHER_CASH").Width + gridView.Columns("OWN_EXCH").Width + gridView.Columns("OWN_CASH").Width
                .HeaderText = "BUYING"
            End With
            With .Columns("RATE")
                .HeaderText = "SELLING"
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Width = gridView.Columns("RATE").Width
            End With
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        End With

        Dim colWid As Integer = 0
        For cnt As Integer = 0 To gridView.ColumnCount - 1
            If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
        Next
        With gridviewhead1
            If colWid >= gridView.Width Then
                .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            Else
                .Columns("SCROLL").Visible = False
            End If
        End With
        With gridViewHead
            If colWid >= gridView.Width Then
                .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            Else
                .Columns("SCROLL").Visible = False
            End If
        End With

    End Function

    Private Sub gridView_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles gridView.ColumnWidthChanged
        If Authorize = False Then Exit Sub
        If gridviewhead1.Columns.Count = 0 Then Exit Sub
        If gridViewHead.Columns.Count = 0 Then Exit Sub
        gridviewhead1.Columns("METALNAME~PURITYNAME~PURITY").Width = gridView.Columns("METALNAME").Width + gridView.Columns("PURITYNAME").Width + gridView.Columns("PURITY").Width
        gridviewhead1.Columns("OTHER_EXCH~OTHER_CASH").Width = gridView.Columns("OTHER_EXCH").Width + gridView.Columns("OTHER_CASH").Width
        gridviewhead1.Columns("OWN_EXCH~OWN_CASH").Width = gridView.Columns("OWN_EXCH").Width + gridView.Columns("OWN_CASH").Width
        gridviewhead1.Columns("RATE").Width = gridView.Columns("RATE").Width

        gridViewHead.Columns("METALNAME~PURITYNAME~PURITY").Width = gridView.Columns("METALNAME").Width + gridView.Columns("PURITYNAME").Width + gridView.Columns("PURITY").Width
        gridViewHead.Columns("OTHER_EXCH~OTHER_CASH~OWN_EXCH~OWN_CASH").Width = gridView.Columns("OTHER_EXCH").Width + gridView.Columns("OTHER_CASH").Width + gridView.Columns("OWN_EXCH").Width + gridView.Columns("OWN_CASH").Width
        gridViewHead.Columns("RATE").Width = gridView.Columns("RATE").Width

        Dim colWid As Integer = 0
        For cnt As Integer = 0 To gridView.ColumnCount - 1
            If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
        Next
        With gridviewhead1
            If colWid >= gridView.Width Then
                .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            Else
                .Columns("SCROLL").Visible = False
            End If
        End With
        With gridViewHead
            If colWid >= gridView.Width Then
                .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            Else
                .Columns("SCROLL").Visible = False
            End If
        End With
    End Sub

    Private Sub gridView_Scroll(sender As Object, e As ScrollEventArgs) Handles gridView.Scroll
        If Authorize = False Then Exit Sub
        If gridviewhead1.Columns.Count = 0 Then Exit Sub
        If gridViewHead.Columns.Count = 0 Then Exit Sub
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridviewhead1.HorizontalScrollingOffset = e.NewValue
                gridviewhead1.Columns("SCROLL").Visible = CType(gridView.Controls(1), VScrollBar).Visible
                gridviewhead1.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            End If
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridViewHead.HorizontalScrollingOffset = e.NewValue
                gridViewHead.Columns("SCROLL").Visible = CType(gridView.Controls(1), VScrollBar).Visible
                gridViewHead.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub

End Class