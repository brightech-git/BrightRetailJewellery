Imports System.Data.OleDb
Public Class CounterTransferInfo
    Dim objGridShower As frmGridDispDia
    Dim StrSql As String
    Dim Cmd As OleDbCommand
    Dim dtCounter, dtmetal As DataTable

    Private Sub CounterTransferInfo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub CounterTransferInfo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GrpContainer.Location = New Point((ScreenWid - GrpContainer.Width) / 2, ((ScreenHit - 128) - GrpContainer.Height) / 2)
        StrSql = " SELECT 'ALL' COUNTER,1 RESULT"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT ITEMCTRNAME AS COUNTER,2 RESULT FROM " & cnAdminDb & "..ITEMCOUNTER"
        StrSql += vbCrLf + " ORDER BY RESULT,COUNTER"
        dtCounter = New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtCounter)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbOldCounter, dtCounter, "COUNTER", , "ALL")
        BrighttechPack.GlobalMethods.FillCombo(chkCmbNewCounter, dtCounter, "COUNTER", , "ALL")

        StrSql = " SELECT 'ALL' METALNAME,'ALL' METALID,1 RESULT"
        StrSql += " UNION ALL"
        StrSql += " SELECT METALNAME,METALID,2 RESULT FROM " & cnAdminDb & "..METALMAST "
        StrSql += " ORDER BY RESULT,METALNAME"
        dtmetal = New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtmetal)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbmetal, dtmetal, "METALNAME", , "ALL")
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        txtRefno.Text = ""
        Prop_Gets()
        dtpFrom.Select()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Prop_Sets()
        Dim Refno As String = ""
        'If txtRefno.Text <> "" Then Refno = cnCostId & txtRefno.Text
        If txtRefno.Text <> "" Then Refno = txtRefno.Text
        StrSql = vbCrLf + "  IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPCTRANSFER')>0 DROP TABLE TEMPTABLEDB..TEMPCTRANSFER"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        StrSql = vbCrLf + "  IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPCTRANSFER1')>0 DROP TABLE TEMPTABLEDB..TEMPCTRANSFER1"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        StrSql = vbCrLf + "  DECLARE @FRMDATE SMALLDATETIME"
        StrSql += vbCrLf + "  DECLARE @TODATE SMALLDATETIME"
        StrSql += vbCrLf + "  SELECT @FRMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "',@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + "  SELECT * "
        StrSql += vbCrLf + "  ,IDENTITY(INT,1,1) AS KEYNO"
        StrSql += vbCrLf + "  INTO TEMPTABLEDB..TEMPCTRANSFER"
        StrSql += vbCrLf + "  FROM ("
        StrSql += vbCrLf + "  SELECT C.TAGSNO,C.ITEMID,C.TAGNO,COU.ITEMCTRNAME AS COUNTER,C.RECDATE,C.ISSDATE,C.REASON"
        StrSql += vbCrLf + "  ,U.USERNAME"
        StrSql += vbCrLf + "  ,C.UPTIME"
        StrSql += vbCrLf + "  ,A.USERNAME AS AUTHORIZE_USER"
        StrSql += vbCrLf + "  ,C.AU_UPTIME AS AUTHORIZE_TIME"
        StrSql += vbCrLf + "  FROM " & cnAdminDb & "..CTRANSFER AS C"
        StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMTAG AS T ON T.SNO = C.TAGSNO"
        StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..USERMASTER AS U ON U.USERID = C.USERID"
        StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..USERMASTER AS A ON A.USERID = C.AU_USERID"
        StrSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..ITEMCOUNTER AS COU ON COU.ITEMCTRID = C.ITEMCTRID"
        StrSql += vbCrLf + "  WHERE ( C.RECDATE BETWEEN @FRMDATE And @TODATE Or C.ISSDATE BETWEEN @FRMDATE And @TODATE )"
        'StrSql += vbCrLf + "  And ISNULL(C.USERID,0) <> 0"
        If Refno <> "" Then StrSql += vbCrLf + "  And C.REFNO = '" & Refno & "'"
        StrSql += vbCrLf + "  UNION ALL"
        StrSql += vbCrLf + "  SELECT C.TAGSNO,C.ITEMID,C.TAGNO,COU.ITEMCTRNAME AS COUNTER,C.RECDATE,C.ISSDATE,C.REASON"
        StrSql += vbCrLf + "  ,U.USERNAME"
        StrSql += vbCrLf + "  ,C.UPTIME"
        StrSql += vbCrLf + "  ,A.USERNAME AS AUTHORIZE_USER"
        StrSql += vbCrLf + "  ,C.AU_UPTIME AS AUTHORIZE_TIME"
        StrSql += vbCrLf + "  FROM " & cnAdminDb & "..CTRANSFER AS C"
        StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMTAG AS T ON T.SNO = C.TAGSNO"
        StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..USERMASTER AS U ON U.USERID = C.USERID"
        StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..USERMASTER AS A ON A.USERID = C.AU_USERID"
        StrSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..ITEMCOUNTER AS COU ON COU.ITEMCTRID = C.ITEMCTRID"
        StrSql += vbCrLf + "  WHERE ( C.RECDATE BETWEEN @FRMDATE AND @TODATE OR C.ISSDATE BETWEEN @FRMDATE AND @TODATE )"
        If Refno <> "" Then StrSql += vbCrLf + "  AND C.TAGSNO IN (SELECT TAGSNO FROM " & cnAdminDb & "..CTRANSFER WHERE REFNO='" & Refno & "')"
        If Refno <> "" Then StrSql += vbCrLf + "  AND C.ENTORDER = (SELECT TOP 1 (ENTORDER - 1) FROM " & cnAdminDb & "..CTRANSFER WHERE REFNO='" & Refno & "' ORDER BY ENTORDER DESC )"
        If Refno <> "" Then StrSql += vbCrLf + "  AND (SELECT COUNT(*)CNT FROM " & cnAdminDb & "..CTRANSFER WHERE TAGSNO = C.TAGSNO AND ITEMID =C.ITEMID AND TAGNO=C.TAGNO AND REFNO= '" & Refno & "') <2"
        StrSql += vbCrLf + "  )C "
        StrSql += vbCrLf + "  ORDER BY C.TAGSNO,C.ITEMID,C.TAGNO,C.RECDATE,C.ISSDATE DESC"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        If chkSummary.Checked Then
            StrSql = vbCrLf + "  SELECT I.ITEMNAME,X.TAGNO,T.PCS,T.GRSWT,T.NETWT"
            StrSql += vbCrLf + " ,T.RATE,T.SALVALUE"
            StrSql += vbCrLf + "  ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
            StrSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
            StrSql += vbCrLf + " ,CASE WHEN X.ISSDATE IS NOT NULL THEN X.ISSDATE ELSE X.RECDATE END AS ISSDATE,X.COUNTER AS OLDCOUNTER,X.NEWCOUNTER,X.REASON,X.TRANSUSER"
            'StrSql += vbCrLf + " ,LTRIM(SUBSTRING(CONVERT(vARCHAR,X.UPTIME,100),12,20))TRANSTIME"
            StrSql += vbCrLf + " ,LTRIM(SUBSTRING(CONVERT(vARCHAR,"
            StrSql += vbCrLf + " (SELECT TOP 1 UPTIME FROM TEMPTABLEDB..TEMPCTRANSFER WHERE TAGSNO = X.TAGSNO AND ITEMID = X.ITEMID AND TAGNO = X.TAGNO AND KEYNO = ISNULL(X.KEYNO,0)+1)"
            StrSql += vbCrLf + " ,100),12,20))TRANSTIME"
            StrSql += vbCrLf + " ,X.AUTHORIZE_USER,LTRIM(SUBSTRING(CONVERT(vARCHAR,X.AUTHORIZE_TIME,100),12,20))AUTHORIZE_TIME"
            StrSql += vbCrLf + "  INTO TEMPTABLEDB..TEMPCTRANSFER1 FROM"
            StrSql += vbCrLf + "  ("
            StrSql += vbCrLf + "  SELECT * "
            StrSql += vbCrLf + "  ,(SELECT TOP 1 COUNTER FROM TEMPTABLEDB..TEMPCTRANSFER WHERE TAGSNO = T.TAGSNO AND ITEMID = T.ITEMID AND TAGNO = T.TAGNO"
            StrSql += vbCrLf + "   AND KEYNO > T.KEYNO)AS NEWCOUNTER"
            StrSql += vbCrLf + "  ,(SELECT TOP 1 username FROM TEMPTABLEDB..TEMPCTRANSFER WHERE TAGSNO = T.TAGSNO AND ITEMID = T.ITEMID AND TAGNO = T.TAGNO"
            StrSql += vbCrLf + "   AND KEYNO > T.KEYNO)AS TRANSUSER"
            StrSql += vbCrLf + "  FROM TEMPTABLEDB..TEMPCTRANSFER AS T"
            StrSql += vbCrLf + "  )X"
            StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMTAG AS T ON T.SNO = X.TAGSNO"
            StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS I ON I.ITEMID = T.ITEMID"
            StrSql += vbCrLf + "  WHERE ISNULL(X.NEWCOUNTER,'') <> '' AND X.NEWCOUNTER <> X.COUNTER "
            If chkCmbOldCounter.Text <> "ALL" And chkCmbOldCounter.Text <> "" Then
                StrSql += vbCrLf + "  AND ISNULL(X.COUNTER,'') IN (" & GetQryString(chkCmbOldCounter.Text) & ")"
            End If
            If chkCmbNewCounter.Text <> "ALL" And chkCmbNewCounter.Text <> "" Then
                StrSql += vbCrLf + "  AND ISNULL(X.NEWCOUNTER,'') IN (" & GetQryString(chkCmbNewCounter.Text) & ")"
            End If
            If chkcmbmetal.Text <> "ALL" And chkcmbmetal.Text <> "" Then
                StrSql += vbCrLf + "  AND ISNULL(I.METALID,'') IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN   (" & GetQryString(chkcmbmetal.Text) & "))"
            End If
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

            StrSql = vbCrLf + " SELECT "
            StrSql += vbCrLf + " ITEMNAME,ISSDATE"
            StrSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(DIAPCS)DIAPCS,SUM(DIAWT)DIAWT"
            StrSql += vbCrLf + " ,OLDCOUNTER,NEWCOUNTER,REASON,TRANSUSER,1 AS RESULT"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMPCTRANSFER1 AS T"
            StrSql += vbCrLf + " GROUP BY ITEMNAME,ISSDATE,OLDCOUNTER,NEWCOUNTER,REASON,TRANSUSER"


            'StrSql += vbCrLf + " /*UNION ALL"
            'StrSql += vbCrLf + " SELECT "
            'StrSql += vbCrLf + " OLDCOUNTER ITEMNAME"
            'StrSql += vbCrLf + " ,NULL PCS,NULL GRSWT,NULL NETWT,NULL DIAPCS,NULL DIAWT"
            'StrSql += vbCrLf + " ,NULL ISSDATE,ISNULL(OLDCOUNTER,'') OLDCOUNTER,NULL NEWCOUNTER,NULL REASON,NULL TRANSUSER,0 AS RESULT"
            'StrSql += vbCrLf + " FROM TEMPCTRANSFER1 AS T"
            'StrSql += vbCrLf + " GROUP BY OLDCOUNTER"
            'StrSql += vbCrLf + " UNION ALL"
            'StrSql += vbCrLf + " SELECT "
            'StrSql += vbCrLf + " ISNULL(OLDCOUNTER,'') + ' TOTAL' ITEMNAME"
            'StrSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(DIAPCS)DIAPCS,SUM(DIAWT)DIAWT"
            'StrSql += vbCrLf + " ,NULL ISSDATE,ISNULL(OLDCOUNTER,'') OLDCOUNTER,NULL NEWCOUNTER,NULL REASON,NULL TRANSUSER,2 AS RESULT"
            'StrSql += vbCrLf + " FROM TEMPCTRANSFER1 AS T"
            'StrSql += vbCrLf + " GROUP BY OLDCOUNTER"
            'StrSql += vbCrLf + " UNION ALL"
            'StrSql += vbCrLf + " SELECT "
            'StrSql += vbCrLf + " 'GRAND TOTAL' ITEMNAME"
            'StrSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(DIAPCS)DIAPCS,SUM(DIAWT)DIAWT"
            'StrSql += vbCrLf + " ,NULL ISSDATE,'ZZZZZZZZZ' OLDCOUNTER,NULL NEWCOUNTER,NULL REASON,NULL TRANSUSER,3 AS RESULT"
            'StrSql += vbCrLf + " FROM TEMPCTRANSFER1 AS T"
            'StrSql += vbCrLf + " ORDER BY OLDCOUNTER,RESULT,NEWCOUNTER,ISSDATE,ITEMNAME,REASON,TRANSUSER*/"

        Else
            StrSql = vbCrLf + "  SELECT I.ITEMNAME,X.TAGNO,T.PCS,T.GRSWT,T.NETWT"
            StrSql += vbCrLf + " ,T.RATE,T.SALVALUE"
            StrSql += vbCrLf + "  ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
            StrSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
            StrSql += vbCrLf + " ,CASE WHEN X.ISSDATE IS NOT NULL THEN X.ISSDATE ELSE X.RECDATE END AS ISSDATE,X.COUNTER AS OLDCOUNTER,X.NEWCOUNTER,X.REASON,X.TRANSUSER"
            'StrSql += vbCrLf + " ,LTRIM(SUBSTRING(CONVERT(vARCHAR,X.UPTIME,100),12,20))TRANSTIME"
            StrSql += vbCrLf + " ,LTRIM(SUBSTRING(CONVERT(vARCHAR,"
            StrSql += vbCrLf + " (SELECT TOP 1 UPTIME FROM TEMPTABLEDB..TEMPCTRANSFER WHERE TAGSNO = X.TAGSNO AND ITEMID = X.ITEMID AND TAGNO = X.TAGNO AND KEYNO = ISNULL(X.KEYNO,0)+1)"
            StrSql += vbCrLf + " ,100),12,20))TRANSTIME"
            StrSql += vbCrLf + " ,X.AUTHORIZE_USER,LTRIM(SUBSTRING(CONVERT(vARCHAR,X.AUTHORIZE_TIME,100),12,20))AUTHORIZE_TIME"
            StrSql += vbCrLf + "  FROM"
            StrSql += vbCrLf + "  ("
            StrSql += vbCrLf + "  SELECT * "
            StrSql += vbCrLf + "  ,(SELECT TOP 1 COUNTER FROM TEMPTABLEDB..TEMPCTRANSFER WHERE TAGSNO = T.TAGSNO AND ITEMID = T.ITEMID AND TAGNO = T.TAGNO"
            StrSql += vbCrLf + "   AND KEYNO > T.KEYNO)AS NEWCOUNTER"
            StrSql += vbCrLf + "  ,(SELECT TOP 1 username FROM TEMPTABLEDB..TEMPCTRANSFER WHERE TAGSNO = T.TAGSNO AND ITEMID = T.ITEMID AND TAGNO = T.TAGNO"
            StrSql += vbCrLf + "   AND KEYNO > T.KEYNO)AS TRANSUSER"
            StrSql += vbCrLf + "  FROM TEMPTABLEDB..TEMPCTRANSFER AS T"
            StrSql += vbCrLf + "  )X"
            StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMTAG AS T ON T.SNO = X.TAGSNO"
            StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS I ON I.ITEMID = T.ITEMID"
            StrSql += vbCrLf + "  WHERE ISNULL(X.NEWCOUNTER,'') <> '' AND X.NEWCOUNTER <> X.COUNTER "
            If chkCmbOldCounter.Text <> "ALL" And chkCmbOldCounter.Text <> "" Then
                StrSql += vbCrLf + "  AND ISNULL(X.COUNTER,'') IN (" & GetQryString(chkCmbOldCounter.Text) & ")"
            End If
            If chkCmbNewCounter.Text <> "ALL" And chkCmbNewCounter.Text <> "" Then
                StrSql += vbCrLf + "  AND ISNULL(X.NEWCOUNTER,'') IN (" & GetQryString(chkCmbNewCounter.Text) & ")"
            End If
            If chkcmbmetal.Text <> "ALL" And chkcmbmetal.Text <> "" Then
                StrSql += vbCrLf + "  AND ISNULL(I.METALID,'') IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN   (" & GetQryString(chkcmbmetal.Text) & "))"
            End If
        End If

        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtGrid)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If

        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name

        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "COUNTER TRANSFER INFO"
        Dim tit As String = "COUNTER TRANSFER INFO REPORT DATE FROM " & dtpFrom.Text & " TO " & dtpTo.Text
        If txtRefno.Text <> "" Then tit = tit + " REF. NO : " & txtRefno.Text
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        'objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.Show()

        Dim ObjGrouper As New BrighttechPack.DataGridViewGrouper(objGridShower.gridView, dtGrid)
        If chkGrpByCounter.Checked Then ObjGrouper.pColumns_Group.Add("OLDCOUNTER")
        If dtGrid.Columns.Contains("PCS") Then ObjGrouper.pColumns_Sum.Add("PCS")
        If dtGrid.Columns.Contains("GRSWT") Then ObjGrouper.pColumns_Sum.Add("GRSWT")
        If dtGrid.Columns.Contains("NETWT") Then ObjGrouper.pColumns_Sum.Add("NETWT")
        If dtGrid.Columns.Contains("DIAPCS") Then ObjGrouper.pColumns_Sum.Add("DIAPCS")
        If dtGrid.Columns.Contains("DIAWT") Then ObjGrouper.pColumns_Sum.Add("DIAWT")
        If dtGrid.Columns.Contains("SALVALUE") Then ObjGrouper.pColumns_Sum.Add("SALVALUE")
        ObjGrouper.pColName_Particular = ""
        ObjGrouper.pColName_Particular = "ITEMNAME"
        ObjGrouper.pColName_ReplaceWithParticular = ""
        ObjGrouper.pColName_ReplaceWithParticular = "ITEMNAME"
        ObjGrouper.pColumns_Sort = "ITEMNAME"
        'ObjGrouper.pColumns_Sum_FilterString = "STONE <> '1'"

        If chkSummary.Checked Then
            ObjGrouper.pColumns_Group.Add("OLDCOUNTER")
        End If

        ObjGrouper.GroupDgv()

        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        'DataGridView_SummaryFormatting(objGridShower.gridView)

        FormatGridColumns(objGridShower.gridView, False, False, , False)
        With objGridShower.gridView
            If dtGrid.Columns.Contains("ITEMNAME") Then .Columns("ITEMNAME").Width = 150
            If dtGrid.Columns.Contains("TAGNO") Then .Columns("TAGNO").Width = 70
            If dtGrid.Columns.Contains("PCS") Then .Columns("PCS").Width = 60
            If dtGrid.Columns.Contains("GRSWT") Then .Columns("GRSWT").Width = 80
            If dtGrid.Columns.Contains("NETWT") Then .Columns("NETWT").Width = 80
            If dtGrid.Columns.Contains("DIAPCS") Then .Columns("DIAPCS").Width = 60
            If dtGrid.Columns.Contains("DIAWT") Then .Columns("DIAWT").Width = 80
            If dtGrid.Columns.Contains("DIAWT") Then .Columns("DIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            If dtGrid.Columns.Contains("SALVALUE") Then .Columns("SALVALUE").Width = 80

            If dtGrid.Columns.Contains("ISSDATE") Then .Columns("ISSDATE").Width = 80
            If dtGrid.Columns.Contains("ISSDATE") Then .Columns("ISSDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            If dtGrid.Columns.Contains("ISSDATE") Then .Columns("ISSDATE").HeaderText = "TRANSFER DATE"
            If dtGrid.Columns.Contains("OLDCOUNTER") Then .Columns("OLDCOUNTER").Width = 120
            If dtGrid.Columns.Contains("NEWCOUNTER") Then .Columns("NEWCOUNTER").Width = 120
            If dtGrid.Columns.Contains("TRANSUSER") Then .Columns("TRANSUSER").Width = 120
            If dtGrid.Columns.Contains("TRANSUSER") Then .Columns("TRANSUSER").HeaderText = "TRANSFERED BY"

            If dtGrid.Columns.Contains("TRANSTIME") Then .Columns("TRANSTIME").Width = 80
            If dtGrid.Columns.Contains("TRANSTIME") Then .Columns("TRANSTIME").HeaderText = "TRANS TIME"

            If dtGrid.Columns.Contains("AUTHORIZE_USER") Then .Columns("AUTHORIZE_USER").Width = 120
            If dtGrid.Columns.Contains("AUTHORIZE_USER") Then .Columns("AUTHORIZE_USER").HeaderText = "AUTHORIZED BY"

            If dtGrid.Columns.Contains("AUTHORIZE_TIME") Then .Columns("AUTHORIZE_TIME").Width = 80
            If dtGrid.Columns.Contains("AUTHORIZE_TIME") Then .Columns("AUTHORIZE_TIME").HeaderText = "AUTHORIZED TIME"
            If dtGrid.Columns.Contains("KEYNO") Then .Columns("KEYNO").Visible = False
            If dtGrid.Columns.Contains("RESULT") Then .Columns("RESULT").Visible = False
            'If chkGrpByCounter.Checked = False Then
            If dtGrid.Columns.Contains("OLDCOUNTER") Then .Columns("OLDCOUNTER").Visible = IIf(chkGrpByCounter.Checked, False, True)
            'End If
            If dtGrid.Columns.Contains("ITEMNAME") Then .Columns("ITEMNAME").Visible = True
        End With

        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        objGridShower.gridViewHeader.Visible = False
        'FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("ITEMNAME")))
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New CounterTransferInfo_Properties
        GetChecked_CheckedList(chkCmbOldCounter, obj.P_chkCmbOldCounter)
        GetChecked_CheckedList(chkCmbNewCounter, obj.P_chkCmbNewCounter)
        chkGrpByCounter.Checked = obj.P_chkGrpByCounter
        SetSettingsObj(obj, Me.Name, GetType(CounterTransferInfo_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New CounterTransferInfo_Properties
        GetSettingsObj(obj, Me.Name, GetType(CounterTransferInfo_Properties))
        SetChecked_CheckedList(chkCmbOldCounter, obj.P_chkCmbOldCounter, "ALL")
        SetChecked_CheckedList(chkCmbNewCounter, obj.P_chkCmbNewCounter, "ALL")
        obj.P_chkGrpByCounter = chkGrpByCounter.Checked
    End Sub

End Class
Public Class CounterTransferInfo_Properties
    Private chkCmbOldCounter As New List(Of String)
    Public Property P_chkCmbOldCounter() As List(Of String)
        Get
            Return chkCmbOldCounter
        End Get
        Set(ByVal value As List(Of String))
            chkCmbOldCounter = value
        End Set
    End Property

    Private chkCmbNewCounter As New List(Of String)
    Public Property P_chkCmbNewCounter() As List(Of String)
        Get
            Return chkCmbNewCounter
        End Get
        Set(ByVal value As List(Of String))
            chkCmbNewCounter = value
        End Set
    End Property

    Private chkGrpByCounter As New Boolean
    Public Property P_chkGrpByCounter() As Boolean
        Get
            Return chkGrpByCounter
        End Get
        Set(ByVal value As Boolean)
            chkGrpByCounter = value
        End Set
    End Property
End Class
