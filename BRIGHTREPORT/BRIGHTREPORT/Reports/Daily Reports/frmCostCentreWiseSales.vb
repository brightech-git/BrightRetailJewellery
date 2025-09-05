Imports System.Data.OleDb
Public Class frmCostCentreWiseSales
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim objGridShower As frmGridDispDia
    Dim SelectedCompany As String
    Dim DAYBOOKDRCR As Boolean = IIf(GetAdmindbSoftValue("DAYBOOKDRCR", "Y").ToString() = "Y", True, False)

    Private Sub DayBookfrm_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Public Sub LoadCompany(ByRef chkLstBox As CheckedListBox)
        chkLstBox.Items.Clear()
        strSql = " SELECT COMPANYNAME FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N' ORDER BY DISPLAYORDER,COMPANYNAME"
        Dim dtCompany As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        For Each ro As DataRow In dtCompany.Rows
            chkLstBox.Items.Add(ro("COMPANYNAME").ToString)
            If ro("COMPANYNAME").ToString = strCompanyName Then chkLstBox.SetItemChecked(chkLstBox.Items.Count - 1, True)
        Next
    End Sub
    Private Sub DayBookfrm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)
        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            chkLstCostCentre.Enabled = True
            chkLstCostCentre.Items.Clear()
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            'FillCheckedListBox(strSql, chkLstCostCentre)

            Dim dt As New DataTable

            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            For i As Integer = 0 To dt.Rows.Count - 1
                If cnCostName = dt.Rows(i).Item("COSTNAME").ToString And cnDefaultCostId = False Then
                    chkLstCostCentre.Items.Add(dt.Rows(i).Item("COSTNAME").ToString, True)
                Else
                    chkLstCostCentre.Items.Add(dt.Rows(i).Item("COSTNAME").ToString, False)
                End If
            Next
            If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkLstCostCentre.Enabled = False
        Else
            chkLstCostCentre.Enabled = False
            chkLstCostCentre.Items.Clear()
        End If
        LoadCompany(chkLstCompany)
        'chkLstUserWise.Enabled = True
        'chkLstUserWise.Items.Clear()
        'strSql = " SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER ORDER BY USERNAME"
        'Dim dtUsr As New DataTable
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dtUsr)
        'For i As Integer = 0 To dtUsr.Rows.Count - 1
        '    chkLstUserWise.Items.Add(dtUsr.Rows(i).Item("USERNAME").ToString, False)
        'Next
        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub chkCostCentreSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCostCentreSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCostCentre, chkCostCentreSelectAll.Checked)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        'rbtAcName.Checked = True
        Prop_Gets()
        dtpFrom.Select()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
     
        Dim chkCostName As String = GetChecked_CheckedList(chkLstCostCentre)
        SelectedCompany = GetSelectedCompanyId(chkLstCompany, False)
        If SelectedCompany = "" Then
            MsgBox("Please Select any Company..!", MsgBoxStyle.Information)
            Exit Sub
        End If
        Dim costId As String = ""
        If chkCostName <> "" Then
            strSql = " SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & ")"
            Dim dtTemp As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTemp)
            For cnt As Integer = 0 To dtTemp.Rows.Count - 1
                costId += "" & dtTemp.Rows(cnt).Item("COSTID").ToString & ""
                If cnt <> dtTemp.Rows.Count - 1 Then
                    costId += ","
                End If
            Next
        End If

        
        strSql = " EXEC " & cnStockDb & "..SP_RPT_COSTCENTREWISESALES"
        strSql += vbCrLf + " @TRANDB ='" & cnStockDb & "'"
        strSql += vbCrLf + " ,@ADMINDB='" & cnAdminDb & "'"
        strSql += vbCrLf + " ,@FRMDATE ='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TODATE ='" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@COMPANYID ='" & SelectedCompany & "'      "
        strSql += vbCrLf + " ,@COSTID ='" & costId & "' "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1

        strSql = "SELECT * FROM TEMPTABLEDB..TEMPCOSALES ORDER BY RESULT,CATNAME"
        Prop_Sets()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)

        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = " COST CENTRE WISE SALES REPORT "
        Dim tit As String = " FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        If chkCostName <> "" Then tit += " COSTCENTRE : " & Replace(chkCostName, "'", "")
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = True
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        objGridShower.gridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        objGridShower.formuser = userId
        objGridShower.gridViewHeader.Visible = True
        objGridShower.Show()
        DataGridView_SummaryFormatting(objGridShower.gridView)
        GridViewHeaderCreator(objGridShower.gridViewHeader)
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)

        dtpFrom.Select()
        objGridShower.FormReLocation = True
        'objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("CATNAME")))
        strSql = " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPCOSALES')>0 DROP TABLE TEMPTABLEDB..TEMPCOSALES  "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Sub
    Private Sub GridViewHeaderCreator(ByVal gridviewHead As DataGridView)

        Dim dtHead As New DataTable
        Dim dtopwt As String, dtoprs As String
        Dim dttrwt As String, dttrrs As String
        Dim dtclwt As String, dtclrs As String

        strSql = " DECLARE @COL AS VARCHAR(500)DECLARE @CT AS VARCHAR(100)SET @COL='' "
        strSql += vbCrLf + " DECLARE CUR1 CURSOR FOR SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE "
        strSql += vbCrLf + " OPEN CUR1 WHILE (1 = 1) BEGIN FETCH NEXT FROM CUR1 INTO @CT "
        strSql += vbCrLf + " IF @@FETCH_STATUS =-1 BREAK  SET @COL+='OP_'+@CT+'_WT~' END CLOSE CUR1 DEALLOCATE CUR1"
        strSql += vbCrLf + " SET @COL+='OP_TOTAL_WT' SELECT  @COL "
        dtopwt = objGPack.GetSqlValue(strSql, , "", )

        strSql = " DECLARE @COL AS VARCHAR(500)DECLARE @CT AS VARCHAR(100)SET @COL='' "
        strSql += vbCrLf + " DECLARE CUR1 CURSOR FOR SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE "
        strSql += vbCrLf + " OPEN CUR1 WHILE (1 = 1) BEGIN FETCH NEXT FROM CUR1 INTO @CT "
        strSql += vbCrLf + " IF @@FETCH_STATUS =-1 BREAK  SET @COL+='OP_'+@CT+'_RS~' END CLOSE CUR1 DEALLOCATE CUR1"
        strSql += vbCrLf + " SET @COL+='OP_TOTAL_RS' SELECT  @COL "
        dtoprs = objGPack.GetSqlValue(strSql, , "", )

        strSql = " DECLARE @COL AS VARCHAR(500)DECLARE @CT AS VARCHAR(100)SET @COL='' "
        strSql += vbCrLf + " DECLARE CUR1 CURSOR FOR SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE "
        strSql += vbCrLf + " OPEN CUR1 WHILE (1 = 1) BEGIN FETCH NEXT FROM CUR1 INTO @CT "
        strSql += vbCrLf + " IF @@FETCH_STATUS =-1 BREAK  SET @COL+='TR_'+@CT+'_WT~' END CLOSE CUR1 DEALLOCATE CUR1"
        strSql += vbCrLf + " SET @COL+='TR_TOTAL_WT' SELECT  @COL "
        dttrwt = objGPack.GetSqlValue(strSql, , "", )

        strSql = " DECLARE @COL AS VARCHAR(500)DECLARE @CT AS VARCHAR(100)SET @COL='' "
        strSql += vbCrLf + " DECLARE CUR1 CURSOR FOR SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE "
        strSql += vbCrLf + " OPEN CUR1 WHILE (1 = 1) BEGIN FETCH NEXT FROM CUR1 INTO @CT "
        strSql += vbCrLf + " IF @@FETCH_STATUS =-1 BREAK  SET @COL+='TR_'+@CT+'_RS~' END CLOSE CUR1 DEALLOCATE CUR1"
        strSql += vbCrLf + " SET @COL+='TR_TOTAL_RS' SELECT  @COL "
        dttrrs = objGPack.GetSqlValue(strSql, , "", )

        strSql = " DECLARE @COL AS VARCHAR(500)DECLARE @CT AS VARCHAR(100)SET @COL='' "
        strSql += vbCrLf + " DECLARE CUR1 CURSOR FOR SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE "
        strSql += vbCrLf + " OPEN CUR1 WHILE (1 = 1) BEGIN FETCH NEXT FROM CUR1 INTO @CT "
        strSql += vbCrLf + " IF @@FETCH_STATUS =-1 BREAK  SET @COL+='CL_'+@CT+'_WT~' END CLOSE CUR1 DEALLOCATE CUR1"
        strSql += vbCrLf + " SET @COL+='CL_TOTAL_WT' SELECT  @COL "
        dtclwt = objGPack.GetSqlValue(strSql, , "", )

        strSql = " DECLARE @COL AS VARCHAR(500)DECLARE @CT AS VARCHAR(100)SET @COL='' "
        strSql += vbCrLf + " DECLARE CUR1 CURSOR FOR SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE "
        strSql += vbCrLf + " OPEN CUR1 WHILE (1 = 1) BEGIN FETCH NEXT FROM CUR1 INTO @CT "
        strSql += vbCrLf + " IF @@FETCH_STATUS =-1 BREAK  SET @COL+='CL_'+@CT+'_RS~' END CLOSE CUR1 DEALLOCATE CUR1"
        strSql += vbCrLf + " SET @COL+='CL_TOTAL_RS' SELECT  @COL "
        dtclrs = objGPack.GetSqlValue(strSql, , "", )

        strSql = "SELECT ''[CATNAME~UNIT]"
        strSql += vbCrLf + " ,''[" & dtopwt & "] "
        strSql += vbCrLf + " ,''[" & dtoprs & "] "
        strSql += vbCrLf + " ,''[" & dttrwt & "] "
        strSql += vbCrLf + " ,''[" & dttrrs & "] "
        strSql += vbCrLf + " ,''[" & dtclwt & "] "
        strSql += vbCrLf + " ,''[" & dtclrs & "] "
        strSql += vbCrLf + " ,''SCROLL"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtHead)

        gridviewHead.DataSource = dtHead
        gridviewHead.Columns("CATNAME~UNIT").HeaderText = ""
        gridviewHead.Columns("" + dtopwt + "").HeaderText = "UP TO PREVIOUS DAY"
        gridviewHead.Columns("" + dtoprs + "").HeaderText = "UP TO PREVIOUS DAY"
        gridviewHead.Columns("" + dttrwt + "").HeaderText = "FOR THE DAY"
        gridviewHead.Columns("" + dttrrs + "").HeaderText = "FOR THE DAY"
        gridviewHead.Columns("" + dtclwt + "").HeaderText = "OVERALL BRANCH TOTAL SALES VALUE "
        gridviewHead.Columns("" + dtclrs + "").HeaderText = "OVERALL BRANCH TOTAL SALES VALUE "
        gridviewHead.Columns("SCROLL").HeaderText = ""
        gridviewHead.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        gridviewHead.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        SetGridHeadColWid(gridviewHead, dtopwt, dtoprs, dttrwt, dttrrs, dtclwt, dtclrs)
    End Sub
    Private Sub SetGridHeadColWid(ByVal gridViewHeader As DataGridView, ByVal opwt As String, ByVal oprs As String, ByVal trwt As String, ByVal trrs As String, ByVal clwt As String, ByVal clrs As String)

        Dim dtopwt() As String = opwt.Split("~")
        Dim dtoprs() As String = oprs.Split("~")
        Dim dttrwt() As String = trwt.Split("~")
        Dim dttrrs() As String = trrs.Split("~")
        Dim dtclwt() As String = clwt.Split("~")
        Dim dtclrs() As String = clrs.Split("~")

        Dim dtopwtwith As String = "0", dtoprswith As String = "0"
        Dim dttrwtwith As String = "0", dttrrswith As String = "0"
        Dim dtclwtwith As String = "0", dtclrswith As String = "0"

     
        Dim f As frmGridDispDia
        f = objGPack.GetParentControl(gridViewHeader)
        If Not f.gridViewHeader.Visible Then Exit Sub
        If f.gridViewHeader Is Nothing Then Exit Sub
        If Not f.gridView.ColumnCount > 0 Then Exit Sub
        If Not f.gridViewHeader.ColumnCount > 0 Then Exit Sub
        With f.gridViewHeader

            If dtopwt.Length > 0 Then
                For i As Integer = 0 To dtopwt.Length - 1
                    dtopwtwith = Val(dtopwtwith) + f.gridView.Columns(dtopwt(i)).Width
                Next
            End If

            If dtoprs.Length > 0 Then
                For i As Integer = 0 To dtoprs.Length - 1
                    dtoprswith = Val(dtoprswith) + f.gridView.Columns(dtoprs(i)).Width
                Next
            End If

            If dttrwt.Length > 0 Then
                For i As Integer = 0 To dttrwt.Length - 1
                    dttrwtwith = Val(dttrwtwith) + f.gridView.Columns(dttrwt(i)).Width
                Next
            End If

            If dttrrs.Length > 0 Then
                For i As Integer = 0 To dttrrs.Length - 1
                    dttrrswith = Val(dttrrswith) + f.gridView.Columns(dttrrs(i)).Width
                Next
            End If

            If dtclwt.Length > 0 Then
                For i As Integer = 0 To dtclwt.Length - 1
                    dtclwtwith = Val(dtclwtwith) + f.gridView.Columns(dtclwt(i)).Width
                Next
            End If

            If dtclrs.Length > 0 Then
                For i As Integer = 0 To dtclrs.Length - 1
                    dtclrswith = Val(dtclrswith) + f.gridView.Columns(dtclrs(i)).Width
                Next
            End If

            .Columns("CATNAME~UNIT").Width = f.gridView.Columns("CATNAME").Width + f.gridView.Columns("UNIT").Width
            .Columns("" + opwt + "").Width = dtopwtwith
            .Columns("" + oprs + "").Width = dtoprswith
            .Columns("" + trwt + "").Width = dttrwtwith
            .Columns("" + trrs + "").Width = dttrrswith
            .Columns("" + clwt + "").Width = dtclwtwith
            .Columns("" + clrs + "").Width = dtclrswith

            Dim colWid As Integer = 0
            For cnt As Integer = 0 To f.gridView.ColumnCount - 1
                If f.gridView.Columns(cnt).Visible Then colWid += f.gridView.Columns(cnt).Width
            Next
            If colWid >= f.gridView.Width Then
                f.gridViewHeader.Columns("SCROLL").Visible = CType(f.gridView.Controls(1), VScrollBar).Visible
                f.gridViewHeader.Columns("SCROLL").Width = CType(f.gridView.Controls(1), VScrollBar).Width
                f.gridViewHeader.Columns("SCROLL").HeaderText = ""
            Else
                f.gridViewHeader.Columns("SCROLL").Visible = False
            End If
        End With
    End Sub
    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)

        For i As Integer = 0 To dgv.Columns.Count - 1
            If dgv.Columns(i).HeaderText <> "CATNAME" And dgv.Columns(i).HeaderText <> "UNIT" Then
                dgv.Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            Else
                dgv.Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            End If
        Next
        With dgv
            .Columns("CATNAME").Width = 250
            .Columns("RESULT").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("KEYNO").Visible = False

            'BrighttechPack.GlobalMethods.FormatGridColumns(dgv)
            '.Columns("TDATE").Width = 90
            '.Columns("TRANNO").Width = 80
            '.Columns("PARTICULAR").Width = 350
            '.Columns("DEBIT").Width = 150
            '.Columns("CREDIT").Width = 150

            '.Columns("TDATE").HeaderText = "TRANDATE"

            '.Columns("DEBIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            '.Columns("CREDIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            'For CNT As Integer = 6 To .ColumnCount - 1
            '    .Columns(CNT).Visible = False
            'Next

            '.Columns("CHQCARDNO").Visible = chkChequeNo.Checked
            '.Columns("CHQDATE").Visible = chkChequeDate.Checked
            '.Columns("REFNO").Visible = chkRefNo.Checked
            '.Columns("REFDATE").Visible = chkRefDate.Checked

            '.Columns("CHQCARDNO").Width = 80
            '.Columns("CHQDATE").Width = 80
            '.Columns("REFNO").Width = 80
            '.Columns("REFDATE").Width = 80

            '.Columns("CHQCARDNO").Visible = IIf(chkMore.Checked = True, chkChequeNo.Checked, chkMore.Checked)
            '.Columns("CHQDATE").Visible = IIf(chkMore.Checked = True, chkChequeDate.Checked, chkMore.Checked)
            '.Columns("REFNO").Visible = IIf(chkMore.Checked = True, chkRefNo.Checked, chkMore.Checked)
            '.Columns("REFDATE").Visible = IIf(chkMore.Checked = True, chkRefDate.Checked, chkMore.Checked)
            'FormatGridColumns(dgv, False, False, , False)
            'FillGridGroupStyle_KeyNoWise(dgv)
        End With

      
    End Sub

    Private Sub chkLstCostCentre_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstCostCentre.Leave
        If Not chkLstCostCentre.CheckedItems.Count > 0 Then
            chkCostCentreSelectAll.Checked = True
        End If
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New frmCostCentreWiseSales_Properties
        obj.p_chkCostCentreSelectAll = chkCostCentreSelectAll.Checked
        GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        obj.p_chkCompanySelectAll = chkCompanySelectAll.Checked
        GetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany)
        SetSettingsObj(obj, Me.Name, GetType(frmCostCentreWiseSales_Properties))
    End Sub

    Private Sub Prop_Gets()

        Dim obj As New frmCostCentreWiseSales_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmCostCentreWiseSales_Properties))
        chkCostCentreSelectAll.Checked = obj.p_chkCostCentreSelectAll
        SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, cnCostName)
        chkCompanySelectAll.Checked = obj.p_chkCompanySelectAll
        SetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany, cnCostName)
        
    End Sub

    Private Sub chkMore_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMore.CheckedChanged
        grpMore.Enabled = chkMore.Checked
        If chkMore.Checked = True Then ChkSummary.Checked = False
    End Sub

    Private Sub ChkSummary_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkSummary.CheckedChanged
        If ChkSummary.Enabled = True Then
            chkMore.Checked = False
            chkDetailed.Checked = False
        End If
    End Sub

    Private Sub chkDetailed_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDetailed.CheckedChanged
        If chkDetailed.Checked = True Then ChkSummary.Checked = False
    End Sub

    Private Sub chkUserWiseSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUserWiseSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstUserWise, chkUserWiseSelectAll.Checked)
    End Sub

    Private Sub chkCompanySelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCompanySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
    End Sub
End Class

Public Class frmCostCentreWiseSales_Properties

    Private chkCostCentreSelectAll As Boolean = False

    Public Property p_chkCostCentreSelectAll() As Boolean
        Get
            Return chkCostCentreSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkCostCentreSelectAll = value
        End Set
    End Property

    Private chkCompanySelectAll As Boolean = False

    Public Property p_chkCompanySelectAll() As Boolean
        Get
            Return chkCompanySelectAll
        End Get
        Set(ByVal value As Boolean)
            chkCompanySelectAll = value
        End Set
    End Property

  
    Private chkLstCostCentre As New List(Of String)
    Public Property p_chkLstCostCentre() As List(Of String)
        Get
            Return chkLstCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkLstCostCentre = value
        End Set
    End Property

    Private chkLstCompany As New List(Of String)
    Public Property p_chkLstCompany() As List(Of String)
        Get
            Return chkLstCompany
        End Get
        Set(ByVal value As List(Of String))
            chkLstCompany = value
        End Set
    End Property

   
End Class