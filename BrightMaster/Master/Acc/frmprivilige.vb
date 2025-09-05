Imports System.Data.OleDb

Public Class frmprivilige
    Dim StrchkCmbMetal As String
    Dim StrchkCmbitem As String
    Dim StrchkCmbSubitem As String
    Dim StrchkCostName As String
    Dim StrchkCompany As String
    Dim Strchkscheme As String
    Dim strSql As String = Nothing
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim tagCondStr As String = Nothing
    Dim itemCondStr As String = Nothing
    Dim emptyCondStr As String = Nothing
    Dim emptyCondStr_NONTAG As String = Nothing
    Dim dsResult As New DataSet("MainResult")
    Dim RW As Integer = Nothing
    Dim SelectedCompany As String

    Dim dtMetal As New DataTable
    Dim dtCounter As New DataTable
    Dim dtItemType As New DataTable
    Dim dtCompany As New DataTable
    Dim dtScheme As New DataTable

    Dim dtCostCentre As New DataTable
    Dim dtItem As New DataTable
    Dim dtSubItem As New DataTable
    Dim dtDesigner As New DataTable
    Dim _Cmd As OleDbCommand
    Dim _Da As OleDbDataAdapter
    Dim _DtTemp As DataTable
    Dim _DiscId As Integer = Nothing
    Dim _flagUpdate As Boolean
    Dim StCompany As New DataTable
    Dim StCostCentre As New DataTable
    Dim StItem As New DataTable
    Dim Stscheme As New DataTable


    Private Sub frmprivilige_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If tabMain.SelectedTab.Name = tabView.Name Then btnBack_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub frmprivilige_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Function funcLoadItemName() As Integer
        strSql = " SELECT 'ALL' ITEMNAME,'ALL' ITEMID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ITEMNAME,CONVERT(VARCHAR,ITEMID),2 RESULT FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ACTIVE = 'Y'"
        If chkCmbMetal.Text <> "ALL" And chkCmbMetal.Text <> "" Then
            strSql += " AND METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(chkCmbMetal.Text) & "))"
        End If
        strSql += " ORDER BY RESULT,ITEMNAME"
        dtItem = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtItem)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbItem, dtItem, "ITEMNAME", , "ALL")
        chkCmbItem.Text = "ALL"
    End Function
    Function funcLoadSubItemName() As Integer
        ChkcmbSubItem.Items.Clear()
        If chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
            strSql = " SELECT 'ALL' SUBITEMNAME,'ALL' SUBITEMID,1 RESULT"
            strSql += " UNION ALL"
            strSql += " SELECT SUBITEMNAME,CONVERT(VARCHAR,SUBITEMID)SUBITEMID,2 RESULT FROM " & cnAdminDb & "..SUBITEMMAST"
            strSql += " WHERE ACTIVE = 'Y'"
            strSql += " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
            strSql += " ORDER BY RESULT,SUBITEMNAME"
            dtSubItem = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtSubItem)
            BrighttechPack.GlobalMethods.FillCombo(ChkcmbSubItem, dtSubItem, "SUBITEMNAME", , "ALL")
            ChkcmbSubItem.Text = "ALL"
        End If
    End Function

    Private Sub frmprivilige_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        txtFrom1_AMT.Visible = False
        txtTo1_AMT.Visible = False

        cmbType.Items.Add("SALES")
        If GetAdmindbSoftValue("CHITDB", "N").ToUpper = "Y" Then
            cmbType.Items.Add("CHIT")
            txtCusttype.Items.Add("CUSTOMER")
            txtCusttype.Items.Add("INTRODUCER")
            txtCusttype.Items.Add("EMPLOYEE")
            txtCusttype.Items.Add("OTHER")
        Else
            txtCusttype.Items.Add("CUSTOMER")
        End If
        cmbActive.Items.Clear()
        cmbActive.Items.Add("YES")
        cmbActive.Items.Add("NO")

        CmbValType_MAN.Items.Clear()
        CmbValType_MAN.Items.Add("AMOUNT")
        CmbValType_MAN.Items.Add("WEIGHT")

        strSql = " SELECT 'ALL' METALNAME,'ALL' METALID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT METALNAME,METALID,2 RESULT FROM " & cnAdminDb & "..METALMAST "
        strSql += " ORDER BY RESULT,METALNAME"
        dtMetal = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtMetal)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbMetal, dtMetal, "METALNAME", , "ALL")

        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY "
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)
        chkCmbCompany.Text = "ALL"

        If GetAdmindbSoftValue("COSTCENTRE").ToUpper = "Y" Then
            strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
            strSql += " UNION ALL"
            strSql += " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
            strSql += " ORDER BY RESULT,COSTNAME"
            dtCostCentre = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCostCentre)
            BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , "ALL")
            chkCmbCostCentre.Text = "ALL"
        Else
            'chkLstCostcentre.Enabled = False
        End If
        If GetAdmindbSoftValue("CHITDB", "N").ToUpper = "Y" Then
            funcLoadScheme()
        End If
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub SetCheckedList(ByVal chklst As CheckedListBox, ByVal bool As Boolean)
        For cnt As Integer = 0 To chklst.Items.Count - 1
            chklst.SetItemChecked(cnt, bool)
        Next
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        objGPack.TextClear(Me)
        tabMain.SelectedTab = tabGeneral
        txtFrom.Text = ""
        txtTo.Text = ""
        _DiscId = Val(objGPack.GetSqlValue("SELECT ISNULL(MAX(DISCID),0)+1 AS DISCID FROM " & cnAdminDb & "..DISCMASTER"))
        _flagUpdate = False
        'chkLstItem.Items.Clear()
        'cmbState.Text = "ALL"
        'SetCheckedList(chkLstCostcentre, False)
        'SetCheckedList(chkLstItem, False)
        cmbType.Text = "SALES"
        cmbActive.Text = "YES"
        txtCusttype.Text = "CUSTOMER"
        txtCusttype.Enabled = False
        chkfrange.Enabled = True
        cmbType.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
    Private Sub CallGRid()
        gridView.DataSource = Nothing
        If cmbType.Text = "SALES" Then
            strSql = " SELECT COMPANYID,COSTID,(CASE WHEN SALESCHIT='S' THEN 'SALES' ELSE 'SCHEME' END) AS SALESCHIT,"
            strSql += " (CASE WHEN WEIGHTORAMOUNT ='A' THEN 'AMOUNT' ELSE 'WEIGHT' END) AS WEIGHTORAMOUNT,"
            strSql += " (SELECT TOP 1 METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID=P.METALID)  AS METAL"
            strSql += " , (SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=P.ITEMID) AS ITEM "
            strSql += " , (SELECT TOP 1 SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID=P.ITEMID AND SUBITEMID=P.SUBITEMID) AS SUBITEM "
            strSql += " ,FROMRANGE ,TORANGE ,POINTS ,VALUE , (CASE WHEN ACTIVE  ='Y' THEN 'YES' ELSE 'NO' END) AS ACTIVE,FIXEDRANGE"
            strSql += " ,(CASE WHEN VALUEINWTRAMT ='W' THEN 'WEIGHT' ELSE 'AMOUNT' END) AS VALUEINWTRAMT,ISNULL(POINTMUL,'') POINTMUL"
            strSql += " FROM " & cnAdminDb & "..PRIVILIGE P where SALESCHIT ='S'  ORDER BY COMPANYID,COSTID,METAL,ITEM"
        Else
            strSql = " SELECT COMPANYID,COSTID,"
            strSql += " (CASE WHEN SALESCHIT='S' THEN 'SALES' ELSE 'SCHEME' END) AS SALESCHIT, "
            strSql += " (CASE WHEN WEIGHTORAMOUNT ='A' THEN 'AMOUNT' ELSE 'WEIGHT' END) AS WEIGHTORAMOUNT, "
            strSql += " (SELECT TOP 1 SCHEMENAME   FROM " & cnChitCompanyid & "SAVINGS..SCHEME  WHERE SCHEMEID = P.SCHEMEID  ) AS SCHEME,FROMRANGE ,TORANGE ,POINTS ,VALUE , "
            strSql += " (CASE WHEN ACTIVE  ='Y' THEN 'YES' ELSE 'NO' END) AS ACTIVE,FIXEDRANGE "
            strSql += " ,(CASE WHEN VALUEINWTRAMT ='W' THEN 'WEIGHT' ELSE 'AMOUNT' END) AS VALUEINWTRAMT,ISNULL(POINTMUL,'') POINTMUL"
            strSql += " FROM " & cnAdminDb & "..PRIVILIGE p where SALESCHIT ='C' ORDER BY COMPANYID,COSTID,SCHEME,WEIGHTORAMOUNT"
        End If

        Dim dtGrid As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        gridView.DataSource = dtGrid
        'gridView.Columns("COSTNAME").Visible = chkLstCostcentre.Enabled
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        gridView.Columns("FIXEDRANGE").Visible = False
        gridView.Columns("POINTMUL").Visible = False
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        CallGRid()
        If Not gridView.RowCount > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        tabMain.SelectedTab = tabView
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub


    Private Function GetCheckedItem(ByVal chklst As CheckedListBox) As String
        Dim _RetStr As String = Nothing
        If chklst.Items.Count > 0 Then
            If chklst.CheckedItems.Count > 0 Then
                For cnt As Integer = 0 To chklst.CheckedItems.Count - 1
                    _RetStr += "'" & chklst.CheckedItems.Item(cnt).ToString & "'"
                    If cnt <> chklst.CheckedItems.Count - 1 Then
                        _RetStr += ","
                    End If
                Next
            End If
        End If
        Return _RetStr
    End Function


    Private Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGeneral
        cmbType.Focus()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        '        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        Dim chkQry As String = "SELECT 1 FROM " & cnAdminDb & "..PRIVILIGE WHERE 1<>1"
        strSql = "DELETE FROM " & cnAdminDb & "..PRIVILIGE WHERE "
        strSql = strSql & " companyid = '" & gridView.Rows(gridView.CurrentRow.Index).Cells("COMPANYID").Value.ToString & "'"
        strSql = strSql & " AND COSTID ='" & gridView.Rows(gridView.CurrentRow.Index).Cells("COSTID").Value.ToString & "'"
        If gridView.Rows(gridView.CurrentRow.Index).Cells("SALESCHIT").Value.ToString = "SALES" Then
            strSql = strSql & " AND ITEMID ='" & objGPack.GetSqlValue("SELECT ITEMID  FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & gridView.Rows(gridView.CurrentRow.Index).Cells("ITEM").Value.ToString & "'") & "'"
        Else
            strSql = strSql & " AND SCHEMEID ='" & objGPack.GetSqlValue("SELECT SCHEMEID  FROM " & cnChitCompanyid & "Savings..Scheme WHERE schemeName = '" & gridView.Rows(gridView.CurrentRow.Index).Cells("SCHEME").Value.ToString & "'") & "'"
        End If
        strSql = strSql & "     AND FROMRANGE =" & Val(gridView.Rows(gridView.CurrentRow.Index).Cells("FROMRANGE").Value.ToString)
        strSql = strSql & "     AND TORANGE =" & Val(gridView.Rows(gridView.CurrentRow.Index).Cells("TORANGE").Value.ToString) & ""

        DeleteItem(SyncMode.Master, chkQry, strSql)

        CallGRid()
        If gridView.RowCount > 0 Then gridView.Focus() Else btnBack_Click(Me, New EventArgs)
    End Sub

    Private Sub chkLstCostcentre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        'If _flagUpdate Then Me.SelectNextControl(chkLstCostcentre, True, True, True, True)
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then e.Handled = True
    End Sub


    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            e.Handled = True
            If Not gridView.RowCount > 0 Then Exit Sub
            If gridView.Rows(gridView.CurrentRow.Index).Cells("SALESCHIT").Value.ToString = "SALES" Then
                With gridView.CurrentRow
                    '    cmbType.Text = .Cells("TYPE").Value.ToString
                    chkCmbCompany.Text = objGPack.GetSqlValue("SELECT COMPANYNAME  FROM " & cnAdminDb & "..company WHERE COMPANYID = '" & gridView.Rows(gridView.CurrentRow.Index).Cells("COMPANYID").Value.ToString & "'")
                    chkCmbCostCentre.Text = objGPack.GetSqlValue("SELECT COSTNAME  FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & gridView.Rows(gridView.CurrentRow.Index).Cells("COSTID").Value.ToString & "'")
                    chkCmbMetal.Text = gridView.Rows(gridView.CurrentRow.Index).Cells("METAL").Value.ToString
                    chkCmbItem.Text = gridView.Rows(gridView.CurrentRow.Index).Cells("ITEM").Value.ToString
                    ChkcmbSubItem.Text = gridView.Rows(gridView.CurrentRow.Index).Cells("SUBITEM").Value.ToString
                    txtFrom.Text = Format(Val(gridView.Rows(gridView.CurrentRow.Index).Cells("FROMRANGE").Value.ToString), "##.0000")
                    txtTo.Text = Format(Val(gridView.Rows(gridView.CurrentRow.Index).Cells("TORANGE").Value.ToString), "##.0000")
                    txtFrom1_AMT.Text = Format(Val(gridView.Rows(gridView.CurrentRow.Index).Cells("FROMRANGE").Value.ToString), "##.00")
                    txtTo1_AMT.Text = Format(Val(gridView.Rows(gridView.CurrentRow.Index).Cells("TORANGE").Value.ToString), "##.00")
                    txtpoints.Text = gridView.Rows(gridView.CurrentRow.Index).Cells("POINTS").Value.ToString
                    txtvalue_AMT.Text = gridView.Rows(gridView.CurrentRow.Index).Cells("VALUE").Value.ToString
                    cmbActive.Text = gridView.Rows(gridView.CurrentRow.Index).Cells("ACTIVE").Value.ToString
                    CmbValType_MAN.Text = gridView.Rows(gridView.CurrentRow.Index).Cells("VALUEINWTRAMT").Value.ToString
                    ChkPointMultiple.Checked = IIf(gridView.Rows(gridView.CurrentRow.Index).Cells("POINTMUL").Value.ToString = "Y", True, False)
                    If gridView.Rows(gridView.CurrentRow.Index).Cells("FIXEDRANGE").Value.ToString = "Y" Then
                        chkfrange.Checked = True
                    Else
                        chkfrange.Checked = False
                    End If
                    privelegetype()
                    chkfrange.Enabled = False
                    If gridView.Rows(gridView.CurrentRow.Index).Cells("WEIGHTORAMOUNT").Value.ToString() = "WEIGHT" Then
                        rbtWeight.Checked = True
                    Else
                        rbtValue.Checked = True
                    End If
                    chkCmbCompany.Enabled = False
                    chkCmbMetal.Enabled = False
                    chkCmbItem.Enabled = False
                    ChkcmbSubItem.Enabled = False
                    chkCmbCostCentre.Enabled = False
                End With
                tabMain.SelectedTab = tabGeneral
                If rbtWeight.Checked Then txtFrom.Focus() Else txtFrom1_AMT.Focus()
                _flagUpdate = True
            ElseIf gridView.Rows(gridView.CurrentRow.Index).Cells("SALESCHIT").Value.ToString = "SCHEME" Then
                With gridView.CurrentRow
                    '    cmbType.Text = .Cells("TYPE").Value.ToString
                    chkCmbCompany.Text = objGPack.GetSqlValue("SELECT COMPANYNAME  FROM " & cnAdminDb & "..company WHERE COMPANYID = '" & gridView.Rows(gridView.CurrentRow.Index).Cells("COMPANYID").Value.ToString & "'")
                    chkCmbCostCentre.Text = objGPack.GetSqlValue("SELECT COSTNAME  FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & gridView.Rows(gridView.CurrentRow.Index).Cells("COSTID").Value.ToString & "'")
                    chkCmbdtScheme.Text = gridView.Rows(gridView.CurrentRow.Index).Cells("SCHEME").Value.ToString
                    txtFrom.Text = Format(Val(gridView.Rows(gridView.CurrentRow.Index).Cells("FROMRANGE").Value.ToString), "##.0000")
                    txtTo.Text = Format(Val(gridView.Rows(gridView.CurrentRow.Index).Cells("TORANGE").Value.ToString), "##.0000")
                    txtFrom1_AMT.Text = Format(Val(gridView.Rows(gridView.CurrentRow.Index).Cells("FROMRANGE").Value.ToString), "##,00")
                    txtTo1_AMT.Text = Format(Val(gridView.Rows(gridView.CurrentRow.Index).Cells("TORANGE").Value.ToString), "##,00")
                    txtpoints.Text = gridView.Rows(gridView.CurrentRow.Index).Cells("POINTS").Value.ToString
                    txtvalue_AMT.Text = gridView.Rows(gridView.CurrentRow.Index).Cells("VALUE").Value.ToString
                    cmbActive.Text = gridView.Rows(gridView.CurrentRow.Index).Cells("ACTIVE").Value.ToString
                    CmbValType_MAN.Text = gridView.Rows(gridView.CurrentRow.Index).Cells("VALUEINWTRAMT").Value.ToString
                    ChkPointMultiple.Checked = IIf(gridView.Rows(gridView.CurrentRow.Index).Cells("POINTMUL").Value.ToString = "Y", True, False)
                    If gridView.Rows(gridView.CurrentRow.Index).Cells("FIXEDRANGE").Value.ToString = "Y" Then
                        chkfrange.Checked = True
                    Else
                        chkfrange.Checked = False
                    End If
                    privelegetype()
                    chkfrange.Enabled = False
                    If gridView.Rows(gridView.CurrentRow.Index).Cells("WEIGHTORAMOUNT").Value.ToString() = "WEIGHT" Then
                        rbtWeight.Checked = True
                    Else
                        rbtValue.Checked = True
                    End If
                End With
                tabMain.SelectedTab = tabGeneral
                txtFrom.Focus()
                _flagUpdate = True
            Else
                MsgBox("Please select Valid  Row")
            End If

        End If
    End Sub

    Private Sub itemload()

    End Sub
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Function funcLoadScheme()
        strSql = " Select 'ALL' as SchemeName union select  SchemeName from " & cnChitCompanyid & "Savings..Scheme "
        strSql += " order by SchemeName"
        dtScheme = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtScheme)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbdtScheme, dtScheme, "SchemeName", , "ALL")
    End Function

    Private Sub cmbCompany_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        funcLoadScheme()
    End Sub

    Private Sub privelegetype()
        If chkfrange.Checked = True Then
            txtTo.Visible = False
            txtTo1_AMT.Visible = False
            Label7.Visible = False
            lblFromValue.Text = "Range"
            ChkPointMultiple.Checked = False
            ChkPointMultiple.Enabled = False
        Else
            If rbtValue.Checked = True Then
                txtTo1_AMT.Visible = True
            Else
                txtTo.Visible = True
            End If
            Label7.Visible = True
            ChkPointMultiple.Enabled = True
            lblFromValue.Text = "From"
        End If
    End Sub

    Private Sub tabGeneral_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabGeneral.Click

    End Sub

    Private Sub Label19_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub btnExit_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub chkCmbMetal_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCmbMetal.LostFocus
        funcLoadItemName()
    End Sub
    Private Sub btnSave_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If objGPack.Validator_Check(Me) Then Exit Sub
        If Val(txtFrom.Text) < 0 Then
            MsgBox("Range values should not Zero", MsgBoxStyle.Information)
            txtFrom.Focus()
            Exit Sub
        End If
        If chkfrange.Checked = True Then
            txtTo.Text = txtFrom.Text
        End If
        If Val(txtTo.Text) < 0 And chkfrange.Checked = False Then
            MsgBox("Range values should not Zero", MsgBoxStyle.Information)
            txtTo.Focus()
            Exit Sub
        End If
        If Val(txtpoints.Text) < 0 Then
            MsgBox("set Points", MsgBoxStyle.Information)
            txtpoints.Focus()
            Exit Sub
        End If
        If _flagUpdate = True Then
            UPDREC(IIf(rbtWeight.Checked = True, "W", "A"), Val(txtFrom.Text), Val(txtTo.Text), Val(txtpoints.Text), Val(txtvalue_AMT.Text))
            Exit Sub
        End If
        Dim lc_comid As String, lc_itemid As String, lc_subitemid As String, lc_metal As String, lc_ccode As String, lc_schemeid As String
        If chkCmbCompany.CheckedItems.Count = chkCmbCompany.Items.Count Then
            StrchkCompany = "'ALL'"
        Else
            StrchkCompany = GetQryString(chkCmbCompany.Text)
        End If

        If chkCmbCostCentre.CheckedItems.Count = chkCmbCostCentre.Items.Count Then
            StrchkCostName = "'ALL'"
        Else
            StrchkCostName = GetQryString(chkCmbCostCentre.Text)
        End If

        If chkCmbItem.CheckedItems.Count = chkCmbItem.Items.Count Then
            StrchkCmbitem = "'ALL'"
        Else
            StrchkCmbitem = GetQryString(chkCmbItem.Text)
        End If
        If ChkcmbSubItem.CheckedItems.Count = ChkcmbSubItem.Items.Count Then
            StrchkCmbSubitem = "'ALL'"
        Else
            StrchkCmbSubitem = GetQryString(ChkcmbSubItem.Text)
        End If

        If chkCmbMetal.CheckedItems.Count = chkCmbMetal.Items.Count Then
            StrchkCmbMetal = "'ALL'"
        Else
            StrchkCmbMetal = GetQryString(chkCmbMetal.Text)
        End If

        If chkCmbdtScheme.CheckedItems.Count = chkCmbdtScheme.Items.Count Then
            Strchkscheme = "'ALL'"
        Else
            Strchkscheme = GetQryString(chkCmbdtScheme.Text)
        End If

        If StrchkCostName = "''" Then StrchkCostName = "'ALL'"
        If StrchkCompany = "''" Then StrchkCompany = "'ALL'"
        If StrchkCmbitem = "''" Then StrchkCmbitem = "'ALL'"
        If StrchkCmbSubitem = "''" Then StrchkCmbSubitem = "'ALL'"
        If StrchkCmbMetal = "''" Then StrchkCmbMetal = "'ALL'"
        If Strchkscheme = "''" Then Strchkscheme = "'ALL'"


        StCompany = New DataTable
        If StrchkCompany = "'ALL'" Then
            strSql = "SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY "
        Else
            strSql = "SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME in (" & StrchkCompany & ")"
        End If
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(StCompany)

        StCostCentre = New DataTable
        If StrchkCostName = "'ALL'" Then
            strSql = "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE "
        Else
            strSql = "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME in (" & StrchkCostName & ")"
        End If

        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(StCostCentre)
        lc_ccode = ""
        lc_schemeid = ""
        If cmbType.Text = "SALES" Then
            StItem = New DataTable
            If StrchkCmbitem = "'ALL'" Then
                strSql = "SELECT I.METALID,I.ITEMID,ISNULL(S.SUBITEMID,0)SUBITEMID FROM " & cnAdminDb & "..ITEMMAST I "
                strSql = strSql & "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST S ON I.ITEMID=S.ITEMID "
                If StrchkCmbMetal <> "'ALL'" Then
                    strSql = strSql & " WHERE I.METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME in (" & StrchkCmbMetal & "))"
                End If
            Else
                If StrchkCmbSubitem = "'ALL'" Then
                    strSql = "SELECT I.METALID,I.ITEMID,ISNULL(S.SUBITEMID,0)SUBITEMID FROM " & cnAdminDb & "..ITEMMAST I "
                    strSql = strSql & "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST S ON I.ITEMID=S.ITEMID "
                    strSql = strSql & "  WHERE I.ITEMNAME in (" & StrchkCmbitem & ")"
                    If StrchkCmbMetal <> "'ALL'" Then
                        strSql = strSql & " AND  I.METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME in (" & StrchkCmbMetal & "))"
                    End If
                Else
                    strSql = "SELECT I.METALID,I.ITEMID,ISNULL(S.SUBITEMID,0)SUBITEMID FROM " & cnAdminDb & "..ITEMMAST I "
                    strSql = strSql & "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST S ON I.ITEMID=S.ITEMID "
                    strSql = strSql & "  WHERE I.ITEMNAME in (" & StrchkCmbitem & ")"
                    strSql = strSql & "  AND S.SUBITEMNAME in (" & StrchkCmbSubitem & ")"
                    If StrchkCmbMetal <> "'ALL'" Then
                        strSql = strSql & " AND  I.METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME in (" & StrchkCmbMetal & "))"
                    End If
                End If
            End If
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(StItem)
            If GetAdmindbSoftValue("COSTCENTRE").ToUpper = "Y" Then
                For Each Row As DataRow In StCompany.Rows
                    lc_comid = Row("companyid").ToString
                    For Each RowC As DataRow In StCostCentre.Rows
                        lc_ccode = RowC("CoSTID").ToString
                        For Each Row1 As DataRow In StItem.Rows
                            lc_itemid = Row1("itemid").ToString
                            lc_subitemid = Row1("subitemid").ToString
                            lc_metal = Row1("metalid").ToString
                            strSql = " SELECT COUNT(*)  FROM " & cnAdminDb & "..PRIVILIGE WHERE companyid = '" & lc_comid & "' and COSTID='" & lc_ccode & "' AND SALESCHIT ='S'  "
                            strSql += vbCrLf & " AND METALID = '" & lc_metal & "' AND ITEMID ='" & lc_itemid & "' AND SUBITEMID ='" & lc_subitemid & "' AND SCHEMEID = '" & lc_schemeid & "' "
                            strSql += vbCrLf & " AND(" & Val(txtFrom.Text) & " BETWEEN FROMRANGE AND TORANGE OR " & Val(txtTo.Text) & " BETWEEN FROMRANGE AND TORANGE"
                            strSql += vbCrLf & " 	OR FROMRANGE BETWEEN " & Val(txtFrom.Text) & " AND " & Val(txtTo.Text) & "	OR TORANGE BETWEEN " & Val(txtFrom.Text) & " AND " & Val(txtTo.Text) & " )"
                            strSql += vbCrLf & " and CUSTOMERTYPE='" & Mid(txtCusttype.Text, 1, 1) & "'"
                            If Val(objGPack.GetSqlValue(strSql)) > 0 Then
                                MsgBox("Same Data Already Exist ")
                                Exit Sub
                            End If
                            INSREC(lc_comid, lc_ccode, "S", IIf(rbtWeight.Checked = True, "W", "A"), lc_metal, lc_itemid, lc_subitemid, lc_schemeid, Val(txtFrom.Text), Val(txtTo.Text), Val(txtpoints.Text), Val(txtvalue_AMT.Text), IIf(chkfrange.Checked = True, "Y", "N"))
                        Next
                    Next
                Next
            Else
                For Each Row As DataRow In StCompany.Rows
                    lc_comid = Row("companyid").ToString
                    For Each Row1 As DataRow In StItem.Rows
                        lc_itemid = Row1("itemid").ToString
                        lc_subitemid = Row1("subitemid").ToString
                        lc_metal = Row1("metalid").ToString
                        strSql = " SELECT COUNT(*)  FROM " & cnAdminDb & "..PRIVILIGE WHERE companyid = '" & lc_comid & "' and COSTID='" & lc_ccode & "' AND SALESCHIT ='S'  "
                        strSql += vbCrLf & " AND METALID = '" & lc_metal & "' AND ITEMID ='" & lc_itemid & "' AND SUBITEMID ='" & lc_subitemid & "' AND SCHEMEID = '" & lc_schemeid & "' "
                        strSql += vbCrLf & " AND(" & Val(txtFrom.Text) & " BETWEEN FROMRANGE AND TORANGE OR " & Val(txtTo.Text) & " BETWEEN FROMRANGE AND TORANGE"
                        strSql += vbCrLf & " 	OR FROMRANGE BETWEEN " & Val(txtFrom.Text) & " AND " & Val(txtTo.Text) & "	OR TORANGE BETWEEN " & Val(txtFrom.Text) & " AND " & Val(txtTo.Text) & " )"
                        strSql += vbCrLf & " and CUSTOMERTYPE='" & Mid(txtCusttype.Text, 1, 1) & "'"
                        If Val(objGPack.GetSqlValue(strSql)) > 0 Then
                            MsgBox("Same Data Already Exist ")
                            Exit Sub
                        End If
                        INSREC(lc_comid, lc_ccode, "S", IIf(rbtWeight.Checked = True, "W", "A"), lc_metal, lc_itemid, lc_subitemid, lc_schemeid, Val(txtFrom.Text), Val(txtTo.Text), Val(txtpoints.Text), Val(txtvalue_AMT.Text), IIf(chkfrange.Checked = True, "Y", "N"))
                    Next
                Next
            End If
        Else 'SCHEME

            Stscheme = New DataTable
            If Strchkscheme = "'ALL'" Then
                strSql = "SELECT SchemeId FROM " & cnChitCompanyid & "Savings..Scheme"
            Else
                strSql = "SELECT SchemeId FROM " & cnChitCompanyid & "Savings..Scheme  WHERE SchemeName in (" & Strchkscheme & ")"
            End If
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(Stscheme)

            If GetAdmindbSoftValue("COSTCENTRE").ToUpper = "Y" Then
                For Each Row As DataRow In StCompany.Rows
                    lc_comid = Row("companyid").ToString
                    For Each RowC As DataRow In StCostCentre.Rows
                        lc_ccode = RowC("CoSTID").ToString
                        lc_itemid = ""
                        lc_metal = ""
                        lc_subitemid = ""
                        For Each sRow As DataRow In Stscheme.Rows
                            lc_schemeid = sRow("SchemeId").ToString
                            strSql = " SELECT COUNT(*)  FROM " & cnAdminDb & "..PRIVILIGE WHERE companyid = '" & lc_comid & "' and COSTID='" & lc_ccode & "' AND SALESCHIT ='S'  "
                            strSql += vbCrLf & " AND METALID = '" & lc_metal & "' AND ITEMID ='" & lc_itemid & "' AND SUBITEMID ='" & lc_subitemid & "' AND SCHEMEID = '" & lc_schemeid & "' "
                            strSql += vbCrLf & " AND(" & Val(txtFrom.Text) & " BETWEEN FROMRANGE AND TORANGE OR " & Val(txtTo.Text) & " BETWEEN FROMRANGE AND TORANGE"
                            strSql += vbCrLf & " 	OR FROMRANGE BETWEEN " & Val(txtFrom.Text) & " AND " & Val(txtTo.Text) & "	OR TORANGE BETWEEN " & Val(txtFrom.Text) & " AND " & Val(txtTo.Text) & " )"
                            strSql += vbCrLf & " and CUSTOMERTYPE='" & Mid(txtCusttype.Text, 1, 1) & "'"
                            If Val(objGPack.GetSqlValue(strSql)) > 0 Then
                                MsgBox("Same Data Already Exist ")
                                Exit Sub
                            End If
                            INSREC(lc_comid, lc_ccode, "C", IIf(rbtWeight.Checked = True, "W", "A"), lc_metal, lc_itemid, lc_subitemid, lc_schemeid, Val(txtFrom.Text), Val(txtTo.Text), Val(txtpoints.Text), Val(txtvalue_AMT.Text), IIf(chkfrange.Checked = True, "Y", "N"))
                        Next

                    Next
                Next
            Else
                For Each Row As DataRow In StCompany.Rows
                    lc_comid = Row("companyid").ToString
                    lc_itemid = ""
                    lc_subitemid = ""
                    lc_metal = ""
                    lc_ccode = ""
                    For Each sRow As DataRow In Stscheme.Rows
                        lc_schemeid = sRow("SchemeId").ToString
                        strSql = " SELECT COUNT(*)  FROM " & cnAdminDb & "..PRIVILIGE WHERE companyid = '" & lc_comid & "' and COSTID='" & lc_ccode & "' AND SALESCHIT ='S'  "
                        strSql += vbCrLf & " AND METALID = '" & lc_metal & "' AND ITEMID ='" & lc_itemid & "' AND SUBITEMID ='" & lc_subitemid & "' AND SCHEMEID = '" & lc_schemeid & "' "
                        strSql += vbCrLf & " AND(" & Val(txtFrom.Text) & " BETWEEN FROMRANGE AND TORANGE OR " & Val(txtTo.Text) & " BETWEEN FROMRANGE AND TORANGE"
                        strSql += vbCrLf & " 	OR FROMRANGE BETWEEN " & Val(txtFrom.Text) & " AND " & Val(txtTo.Text) & "	OR TORANGE BETWEEN " & Val(txtFrom.Text) & " AND " & Val(txtTo.Text) & " )"
                        strSql += vbCrLf & " and CUSTOMERTYPE='" & Mid(txtCusttype.Text, 1, 1) & "'"
                        If Val(objGPack.GetSqlValue(strSql)) > 0 Then
                            MsgBox("Same Data Already Exist ")
                            Exit Sub
                        End If
                        INSREC(lc_comid, lc_ccode, "C", IIf(rbtWeight.Checked = True, "W", "A"), lc_metal, lc_itemid, lc_subitemid, lc_schemeid, Val(txtFrom.Text), Val(txtTo.Text), Val(txtpoints.Text), Val(txtvalue_AMT.Text), IIf(chkfrange.Checked = True, "Y", "N"))
                    Next
                Next
            End If
        End If
        'MsgBox(" Updated ")
        btnNew_Click(Me, New EventArgs)
        'INSREC()
    End Sub
    Private Sub UPDREC(ByVal weightoramount As String, ByVal FRMRAG As Decimal, ByVal TORAG As Decimal, ByVal POINTS As Decimal, ByVal AMOUNT As Decimal)
        Dim mItemid As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID  FROM " & cnAdminDb & "..ITEMMAST WHERE  ITEMNAME   = '" & chkCmbItem.Text & "'"))
        Dim msubItemid As Integer = Val(objGPack.GetSqlValue("SELECT SUBITEMID  FROM " & cnAdminDb & "..SUBITEMMAST WHERE  SUBITEMNAME = '" & ChkcmbSubItem.Text & "' AND ITEMID='" & mItemid & "'"))
        strSql = " UPDATE  " & cnAdminDb & "..PRIVILIGE"
        strSql += " SET "
        strSql += " WEIGHTORAMOUNT ='" & weightoramount & "'" 'WEIGHT OR AMOUNT W/A
        strSql += " ,FROMRANGE=" & Val(FRMRAG) & "" 'FROMRANGE
        strSql += " ,TORANGE=" & Val(TORAG) & "" 'TORANGE
        strSql += " ,POINTS=" & Val(POINTS) & "" 'POINTS
        strSql += " ,VALUE=" & Val(AMOUNT) & "" 'VALUES
        strSql += " ,ACTIVE='" & Mid(cmbActive.Text, 1, 1) & "'" 'ACTIVE
        strSql += " ,USERID='" & userId & "'" 'USERID
        strSql += " ,UPDATED='" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,VALUEINWTRAMT='" & Mid(CmbValType_MAN.Text, 1, 1) & "'" 'VALUEINWTRAMT
        strSql += " ,POINTMUL='" & IIf(ChkPointMultiple.Checked, "Y", "N") & "'" 'POINTMUL
        strSql += " WHERE COMPANYID ='" & objGPack.GetSqlValue("SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE  COMPANYNAME   = '" & chkCmbCompany.Text & "'") & "'"
        If GetAdmindbSoftValue("COSTCENTRE").ToUpper = "Y" Then
            strSql += " AND COSTID ='" & objGPack.GetSqlValue("SELECT COSTID  FROM " & cnAdminDb & "..COSTCENTRE WHERE  COSTNAME   = '" & chkCmbCostCentre.Text & "'") & "'"
        End If
        If cmbType.Text = "SALES" Then
            strSql += " AND ITEMID ='" & mItemid & "'"
            strSql += " AND SUBITEMID ='" & msubItemid & "'"
        Else
            strSql += " AND SCHEMEID ='" & objGPack.GetSqlValue("SELECT SchemeId  FROM " & cnChitCompanyid & "Savings..Scheme WHERE  schemeName   = '" & chkCmbdtScheme.Text & "'") & "'"
        End If
        ExecQuery(SyncMode.Master, strSql, cn, tran)
        _flagUpdate = False
        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub INSREC(ByVal companyid As String, ByVal costcode As String, ByVal saleschit As String, ByVal weightoramount As String, ByVal METALID As String, ByVal ITEMID As String, ByVal SUBITEMID As String, ByVal SCHEMEID As String, ByVal FRMRAG As Decimal, ByVal TORAG As Decimal, ByVal POINTS As Decimal, ByVal AMOUNT As Decimal, ByVal FIXEDRANGE As String)
        strSql = " INSERT INTO " & cnAdminDb & "..PRIVILIGE"
        strSql += " ("
        strSql += " COMPANYID,COSTID,SALESCHIT,WEIGHTORAMOUNT,METALID,ITEMID,SUBITEMID,"
        strSql += " SCHEMEID,FROMRANGE,TORANGE,POINTS,VALUE,FIXEDRANGE,ACTIVE,USERID,UPDATED,CUSTOMERTYPE,VALUEINWTRAMT,POINTMUL)VALUES ("
        strSql += " '" & companyid & "'" 'COMPANYID
        strSql += " ,'" & costcode & "'" 'COSTID
        strSql += " ,'" & saleschit & "'" 'SALES OR SCHEME S/C
        strSql += " ,'" & weightoramount & "'" 'WEIGHT OR AMOUNT W/A
        strSql += " ,'" & METALID & "'" 'METALID
        strSql += " ,'" & ITEMID & "'" 'ITEMID
        strSql += " ,'" & SUBITEMID & "'" 'ITEMID
        strSql += " ,'" & SCHEMEID & "'" 'SCHEMEID
        strSql += " ," & Val(FRMRAG) & "" 'FROMRANGE
        strSql += " ," & Val(TORAG) & "" 'TORANGE
        strSql += " ," & Val(POINTS) & "" 'POINTS
        strSql += " ," & Val(AMOUNT) & "" 'VALUES
        strSql += " ,'" & FIXEDRANGE & "'" 'FIXED RANGE
        strSql += " ,'" & Mid(cmbActive.Text, 1, 1) & "'" 'ACTIVE
        strSql += " ,'" & userId & "'" 'USERID
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Mid(txtCusttype.Text, 1, 1) & "'" 'CUSTTYPE
        strSql += " ,'" & Mid(CmbValType_MAN.Text, 1, 1) & "'" 'VALUEINWTRAMT
        strSql += " ,'" & IIf(ChkPointMultiple.Checked, "Y", "N") & "'" 'POINTMUL
        strSql += " )"
        ExecQuery(SyncMode.Master, strSql, cn, tran)
    End Sub

    Private Sub btnNew_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        txtCusttype.Text = "CUSTOMER"
        cmbType.Text = "SALES"
        cmbActive.Text = "YES"
        CmbValType_MAN.Text = "AMOUNT"
        If cmbType.Text = "SALES" Then
            chkCmbItem.Enabled = True
            ChkcmbSubItem.Enabled = True
            chkCmbMetal.Enabled = True
            chkCmbdtScheme.Enabled = False
        Else
            chkCmbItem.Enabled = False
            ChkcmbSubItem.Enabled = False
            chkCmbMetal.Enabled = False
            chkCmbdtScheme.Enabled = True
        End If
        privelegetype()
    End Sub

    Private Sub cmbType_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbType.LostFocus
        If cmbType.Text = "SALES" Then
            txtCusttype.Text = "CUSTOMER"
            txtCusttype.Enabled = False
        Else
            txtCusttype.Enabled = True
            txtCusttype.Focus()
        End If
    End Sub

    Private Sub cmbType_SelectedIndexChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbType.SelectedIndexChanged
        If cmbType.Text = "SALES" Then
            chkCmbItem.Enabled = True
            ChkcmbSubItem.Enabled = True
            chkCmbMetal.Enabled = True
            chkCmbdtScheme.Enabled = False
        Else
            chkCmbItem.Enabled = False
            ChkcmbSubItem.Enabled = False
            chkCmbMetal.Enabled = False
            chkCmbdtScheme.Enabled = True
        End If
        privelegetype()
    End Sub

    Private Sub chkCmbItem_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCmbItem.LostFocus
        funcLoadSubItemName()
    End Sub

    Private Sub chkCmbItem_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCmbItem.SelectedIndexChanged
        'funcLoadSubItemName()
    End Sub

    Private Sub btnOpen_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        CallGRid()
        If Not gridView.RowCount > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        tabMain.SelectedTab = tabView
    End Sub

    Private Sub gridView_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellContentClick

    End Sub

    Private Sub rbtValue_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtValue.CheckedChanged
        If rbtValue.Checked = True Then
            txtFrom1_AMT.Visible = True
            txtTo1_AMT.Visible = True
            txtFrom.Visible = False
            txtTo.Visible = False
        Else
            txtFrom1_AMT.Visible = False
            txtTo1_AMT.Visible = False
            txtFrom.Visible = True
            txtTo.Visible = True
        End If
        privelegetype()
    End Sub

    Private Sub txtFrom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtFrom.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then Exit Sub
        Dim preStr As String = Nothing

        If e.KeyChar = "." And sender.Text.Contains(".") Then
            e.Handled = True
            Exit Sub
        End If
        Select Case e.KeyChar
            Case "+", "-", "0" To "9", ".", _
            ChrW(Keys.Enter), ChrW(Keys.Escape), ChrW(Keys.Space)
            Case ChrW(Keys.Back)
                Exit Sub
            Case Else
                e.Handled = True
                MsgBox(preStr + "Digits only Allowed 0 to 9", MsgBoxStyle.Information)
                sender.Focus()
        End Select
        If sender.Text.Contains(".") Then
            Dim dotPos As Integer = InStr(sender.Text, ".", CompareMethod.Text)
            Dim sp() As String = sender.Text.Split(".")
            Dim curPos As Integer = sender.SelectionStart
            If sp.Length >= 2 Then
                If curPos >= dotPos Then
                    If sp(1).Length > 3 Then
                        e.Handled = True
                    End If
                End If
            End If
        End If

    End Sub

    Private Sub txtFrom_WET_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFrom.TextChanged
        If txtFrom1_AMT.Visible = False Then txtFrom1_AMT.Text = Format(Val(txtFrom.Text), "##.00")
    End Sub

    Private Sub txtFrom1_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFrom1_AMT.TextChanged
        If txtFrom.Visible = False Then txtFrom.Text = Format(Val(txtFrom1_AMT.Text), "##.0000")
    End Sub

    Private Sub txtTo1_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTo1_AMT.TextChanged
        If txtTo.Visible = False Then txtTo.Text = Format(Val(txtTo1_AMT.Text), "##.0000")
    End Sub

    Private Sub txtTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then Exit Sub
        Dim preStr As String = Nothing

        If e.KeyChar = "." And sender.Text.Contains(".") Then
            e.Handled = True
            Exit Sub
        End If
        Select Case e.KeyChar
            Case "+", "-", "0" To "9", ".", _
            ChrW(Keys.Enter), ChrW(Keys.Escape), ChrW(Keys.Space)
            Case ChrW(Keys.Back)
                Exit Sub
            Case Else
                e.Handled = True
                MsgBox(preStr + "Digits only Allowed 0 to 9", MsgBoxStyle.Information)
                sender.Focus()
        End Select
        If sender.Text.Contains(".") Then
            Dim dotPos As Integer = InStr(sender.Text, ".", CompareMethod.Text)
            Dim sp() As String = sender.Text.Split(".")
            Dim curPos As Integer = sender.SelectionStart
            If sp.Length >= 2 Then
                If curPos >= dotPos Then
                    If sp(1).Length > 3 Then
                        e.Handled = True
                    End If
                End If
            End If
        End If

    End Sub
    Private Sub txtTo_WET_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTo.TextChanged
        If txtTo1_AMT.Visible = False Then txtTo1_AMT.Text = Format(Val(txtTo.Text), "##.000")

    End Sub

    Private Sub chkfrange_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkfrange.CheckedChanged
        privelegetype()
    End Sub

    Private Sub rbtWeight_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtWeight.CheckedChanged
        If rbtValue.Checked = True Then
            txtFrom1_AMT.Visible = True
            txtTo1_AMT.Visible = True
            txtFrom.Visible = False
            txtTo.Visible = False
        Else
            txtFrom1_AMT.Visible = False
            txtTo1_AMT.Visible = False
            txtFrom.Visible = True
            txtTo.Visible = True
        End If
        privelegetype()
    End Sub
End Class