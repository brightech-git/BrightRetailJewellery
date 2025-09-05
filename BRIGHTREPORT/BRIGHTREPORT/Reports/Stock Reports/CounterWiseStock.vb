Imports System.Data.OleDb
Imports System.Xml
Imports System.IO
Public Class CounterWiseStock
    'calno 600 -VASANTHAN, Client-DAR 
    Dim strSql As String = Nothing
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim tagCondStr As String = Nothing
    Dim itemCondStr As String = Nothing
    Dim emptyCondStr As String = Nothing
    Dim emptyCondStr_NONTAG As String = Nothing
    Dim dsResult As New DataSet("MainResult")
    Dim RW As Integer = Nothing
    Private CheckOnly As Boolean = False
    Dim RPT_CHKDIS_ROLEEDIT As Boolean = IIf(GetAdmindbSoftValue("RPT_CHKDIS_ROLEEDIT", "N") = "Y", True, False)
    Dim celWasEndEdit As DataGridViewCell = Nothing
    Dim CounterWiseStockP As String = GetAdmindbSoftValue("RPT_CTRSTOCK_PROC", "SP_RPT_COUNTERWISESTOCK")
    Dim CTRSTK_PCSCHK As Boolean = IIf(GetAdmindbSoftValue("CTRSTK_PCSCHK", "N") = "Y", True, False)
    Dim Save As Boolean = False
    Dim Authorize As Boolean = False

    Public Sub New(ByVal CheckOnly As Boolean)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        'Me.WindowState = FormWindowState.Maximized
        Me.CheckOnly = CheckOnly
        tabMain.SelectedTab = tabGen
    End Sub

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        'Me.WindowState = FormWindowState.Maximized
        tabMain.SelectedTab = tabGen
    End Sub

    Function funcExit() As Integer
        Me.Close()
    End Function

    Function funcLoadCostCentre() As Integer
        strSql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE'"
        If objGPack.GetSqlValue(strSql, , "N") = "Y" Then
            strSql = " Select CostName from " & cnAdminDb & "..CostCentre order by CostName"
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
            chkLstCostCentre.Items.Clear()
            chkLstCostCentre.Enabled = False
        End If
    End Function

    Function funcLoadItemType() As Integer
        strSql = " Select Name from " & cnAdminDb & "..itemType order by Name"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        chkLstItemType.Items.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            chkLstItemType.Items.Add(dt.Rows(i).Item("NAME").ToString)
        Next
    End Function
    Function funcLoadsubitemgroup() As Integer
        cmbSubItemGroup.Items.Clear()
        strSql = vbCrLf + " SELECT DISTINCT SGROUPNAME FROM " & cnAdminDb & "..SUBITEMGROUP WHERE ACTIVE='Y'"
        strSql += vbCrLf + " ORDER BY SGROUPNAME"
        Dim dtsubitemgrp As DataTable
        dtsubitemgrp = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtsubitemgrp)
        cmbSubItemGroup.Items.Add("ALL", True)
        If dtsubitemgrp.Rows.Count > 0 Then
            BrighttechPack.GlobalMethods.FillCombo(cmbSubItemGroup, dtsubitemgrp, "SGROUPNAME", False, "ALL")
        End If
        cmbSubItemGroup.Text = "ALL"
    End Function

    Function funcLoadCounter() As Integer
        strSql = " select itemCtrName from " & cnAdminDb & "..itemCounter WHERE ISNULL(ACTIVE,'')<>'N' order by ItemCtrName"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        chkLstCounter.Items.Clear()
        cmbCounter.Items.Clear()
        For i As Integer = 0 To dt.Rows.Count - 1
            chkLstCounter.Items.Add(dt.Rows(i).Item("ITEMCTRNAME").ToString)
            cmbCounter.Items.Add(dt.Rows(i).Item("ITEMCTRNAME").ToString)
        Next
    End Function

    Function funcLoadDesigner() As Integer
        cmbDesigner.Items.Clear()
        cmbDesigner.Items.Add("ALL")
        strSql = " Select DesignerName from " & cnAdminDb & "..Designer order by DesignerName"
        objGPack.FillCombo(strSql, cmbDesigner, False)
        cmbDesigner.Text = "ALL"
    End Function

    Function funcLoadCategory() As Integer

        cmbCategory.Items.Clear()
        cmbCategory.Items.Add("ALL")
        strSql = " select CatName from " & cnAdminDb & "..Category "
        If cmbMetal.Text <> "ALL" Then
            strSql += " where metalid = (select metalid from " & cnAdminDb & "..metalmast where metalname = '" & cmbMetal.Text & "')"
        End If
        strSql += "  order by CatName"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        For i As Integer = 0 To dt.Rows.Count - 1
            chkCmbCategory.Items.Add(dt.Rows(i).Item("CATNAME").ToString)
            cmbCategory.Items.Add(dt.Rows(i).Item("CATNAME").ToString)
        Next


        'objGPack.FillCombo(strSql, cmbCategory, False)
        cmbCategory.Text = "ALL"
    End Function

    Function funcLoadItemName() As Integer
        cmbItemName.Items.Clear()
        cmbItemName.Items.Add("ALL")
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ACTIVE,'') = 'Y'"
        strSql += " AND ISNULL(STOCKREPORT,'') = 'Y'"
        If cmbMetal.Text <> "ALL" Then
            strSql += " AND  METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "')"
        End If
        strSql += "  ORDER BY ITEMID"
        objGPack.FillCombo(strSql, cmbItemName, False)
        cmbItemName.Text = "ALL"
    End Function

    Function funcLoadMetal() As Integer
        cmbMetal.Items.Clear()
        cmbMetal.Items.Add("ALL")
        strSql = " select MetalName from " & cnAdminDb & "..metalMast order by MetalName"
        objGPack.FillCombo(strSql, cmbMetal, False, False)
        cmbMetal.Text = "ALL"
    End Function
    Private Sub GridStyle()
        ''
        FillGridGroupStyle_KeyNoWise(gridView)
        FormatGridColumns(gridView, False, , , False)
        gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        With gridView
            .Columns("KEYNO").Visible = False
            If .Columns.Contains("COSTID") Then .Columns("COSTID").Visible = False
            .Columns("PARTICULAR").Width = 200
            .Columns("OPCS").Width = 70
            .Columns("OGRSWT").Width = 100
            .Columns("ONETWT").Width = 100
            .Columns("ODIAPCS").Width = 70
            .Columns("ODIAWT").Width = 100
            .Columns("OSTNPCS").Width = 70
            .Columns("OSTNWT").Width = 100

            If chkSepApprovalColumn.Checked = True Or chkOnlyApproval.Checked = True Then
                .Columns("APPOPCS").Width = 70
                .Columns("APPOGRSWT").Width = 100
                .Columns("APPONETWT").Width = 100
            End If

            .Columns("TRPCS").Width = 70
            .Columns("TRGRSWT").Width = 100
            .Columns("TRNETWT").Width = 100
            .Columns("TRDIAPCS").Width = 70
            .Columns("TRDIAWT").Width = 100
            .Columns("TRSTNPCS").Width = 70
            .Columns("TRSTNWT").Width = 100

            .Columns("RPCS").Width = 70
            .Columns("RGRSWT").Width = 100
            .Columns("RNETWT").Width = 100
            .Columns("RDIAPCS").Width = 70
            .Columns("RDIAWT").Width = 100
            .Columns("RSTNPCS").Width = 70
            .Columns("RSTNWT").Width = 100
            If chkSepApprovalColumn.Checked = True Or chkOnlyApproval.Checked = True Then
                .Columns("APPRPCS").Width = 70
                .Columns("APPRGRSWT").Width = 100
                .Columns("APPRNETWT").Width = 100
            End If

            .Columns("TIPCS").Width = 70
            .Columns("TIGRSWT").Width = 100
            .Columns("TINETWT").Width = 100
            .Columns("TIDIAPCS").Width = 70
            .Columns("TIDIAWT").Width = 100
            .Columns("TISTNPCS").Width = 70
            .Columns("TISTNWT").Width = 100

            .Columns("IPCS").Width = 70
            .Columns("IGRSWT").Width = 100
            .Columns("INETWT").Width = 100
            .Columns("IDIAPCS").Width = 70
            .Columns("IDIAWT").Width = 100
            .Columns("ISTNPCS").Width = 70
            .Columns("ISTNWT").Width = 100
            If chkSepApprovalColumn.Checked = True Or chkOnlyApproval.Checked = True Then
                .Columns("APPIPCS").Width = 70
                .Columns("APPIGRSWT").Width = 100
                .Columns("APPINETWT").Width = 100
            End If

            .Columns("MIPCS").Width = 70
            .Columns("MIGRSWT").Width = 100
            .Columns("MINETWT").Width = 100

            .Columns("CPCS").Width = 70
            .Columns("CGRSWT").Width = 100
            .Columns("CNETWT").Width = 100
            .Columns("CDIAPCS").Width = 70
            .Columns("CDIAWT").Width = 100
            .Columns("CSTNPCS").Width = 70
            .Columns("CSTNWT").Width = 100
            If chkSepApprovalColumn.Checked = True Or chkOnlyApproval.Checked = True Then
                .Columns("APPCPCS").Width = 70
                .Columns("APPCGRSWT").Width = 100
                .Columns("APPCNETWT").Width = 100
            End If
            .Columns("RATE").Width = 80
            ''HEADER TEXT
            If Not chkOnlyTag.Checked Then .Columns("OPCS").HeaderText = "O.PCS" Else .Columns("OPCS").HeaderText = "TAG"
            .Columns("OGRSWT").HeaderText = "O.GRSWT"
            .Columns("ONETWT").HeaderText = "O.NETWT"
            .Columns("OSALVALUE").HeaderText = "O.SALVALUE"
            .Columns("OPURVALUE").HeaderText = "O.PURVALUE"
            .Columns("ODIAPCS").HeaderText = "O.DIAPCS"
            .Columns("ODIAWT").HeaderText = "O.DIAWT"
            .Columns("OSTNPCS").HeaderText = "O.STNPCS"
            .Columns("OSTNWT").HeaderText = "O.STNWT"


            If Not chkOnlyTag.Checked Then .Columns("TRPCS").HeaderText = "T.PCS" Else .Columns("TRPCS").HeaderText = "TAG"
            .Columns("TRGRSWT").HeaderText = "T.GRSWT"
            .Columns("TRNETWT").HeaderText = "T.NETWT"
            .Columns("TRDIAPCS").HeaderText = "T.DIAPCS"
            .Columns("TRDIAWT").HeaderText = "T.DIAWT"
            .Columns("TRSTNPCS").HeaderText = "T.STNPCS"
            .Columns("TRSTNWT").HeaderText = "T.STNWT"

            If Not chkOnlyTag.Checked Then .Columns("RPCS").HeaderText = "R.PCS" Else .Columns("RPCS").HeaderText = "TAG"
            .Columns("RGRSWT").HeaderText = "R.GRSWT"
            .Columns("RNETWT").HeaderText = "R.NETWT"
            .Columns("RSALVALUE").HeaderText = "R.SALVALUE"
            .Columns("RPURVALUE").HeaderText = "R.PURVALUE"
            .Columns("RDIAPCS").HeaderText = "R.DIAPCS"
            .Columns("RDIAWT").HeaderText = "R.DIAWT"
            .Columns("RSTNPCS").HeaderText = "R.STNPCS"
            .Columns("RSTNWT").HeaderText = "R.STNWT"

            If Not chkOnlyTag.Checked Then .Columns("TIPCS").HeaderText = "T.PCS" Else .Columns("TIPCS").HeaderText = "TAG"
            .Columns("TIGRSWT").HeaderText = "T.GRSWT"
            .Columns("TINETWT").HeaderText = "T.NETWT"
            .Columns("TIDIAPCS").HeaderText = "T.DIAPCS"
            .Columns("TIDIAWT").HeaderText = "T.DIAWT"
            .Columns("TISTNPCS").HeaderText = "T.STNPCS"
            .Columns("TISTNWT").HeaderText = "T.STNWT"


            If Not chkOnlyTag.Checked Then .Columns("IPCS").HeaderText = "I.PCS" Else .Columns("IPCS").HeaderText = "TAG"
            .Columns("IGRSWT").HeaderText = "I.GRSWT"
            .Columns("INETWT").HeaderText = "I.NETWT"
            .Columns("ISALVALUE").HeaderText = "I.SALVALUE"
            .Columns("IPURVALUE").HeaderText = "I.PURVALUE"
            .Columns("IDIAPCS").HeaderText = "I.DIAPCS"
            .Columns("IDIAWT").HeaderText = "I.DIAWT"
            .Columns("ISTNPCS").HeaderText = "I.STNPCS"
            .Columns("ISTNWT").HeaderText = "I.STNWT"


            If Not chkOnlyTag.Checked Then .Columns("MIPCS").HeaderText = "M.PCS" Else .Columns("MIPCS").HeaderText = "TAG"
            .Columns("MIGRSWT").HeaderText = "M.GRSWT"
            .Columns("MINETWT").HeaderText = "M.NETWT"

            If .Columns.Contains("PSPCS") Then
                If Not chkOnlyTag.Checked Then .Columns("PSPCS").HeaderText = "P.PCS" Else .Columns("PSPCS").HeaderText = "TAG"
            End If
            If .Columns.Contains("PSGRSWT") Then .Columns("PSGRSWT").HeaderText = "P.GRSWT"
            If .Columns.Contains("PSNETWT") Then .Columns("PSNETWT").HeaderText = "P.NETWT"


            If Not chkOnlyTag.Checked Then .Columns("CPCS").HeaderText = "C.PCS" Else .Columns("CPCS").HeaderText = "TAG"
            .Columns("CGRSWT").HeaderText = "C.GRSWT"
            .Columns("CNETWT").HeaderText = "C.NETWT"
            .Columns("CSALVALUE").HeaderText = "C.SALVALUE"
            .Columns("CPURVALUE").HeaderText = "C.PURVALUE"
            .Columns("CDIAPCS").HeaderText = "C.DIAPCS"
            .Columns("CDIAWT").HeaderText = "C.DIAWT"
            .Columns("CSTNPCS").HeaderText = "C.STNPCS"
            .Columns("CSTNWT").HeaderText = "C.STNWT"


            ''BACKCOLOR
            .Columns("OPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OGRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ONETWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ODIAPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ODIAWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OSTNPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("OSTNWT").DefaultCellStyle.BackColor = Color.AliceBlue

            .Columns("TIPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("TIGRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("TINETWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("TIDIAPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("TIDIAWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("TISTNPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("TISTNWT").DefaultCellStyle.BackColor = Color.AliceBlue

            .Columns("IPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("IGRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("INETWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("MIPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("MIGRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("MINETWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("IDIAPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("IDIAWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ISTNPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ISTNWT").DefaultCellStyle.BackColor = Color.AliceBlue

            .Columns("APPOPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("APPOGRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("APPONETWT").DefaultCellStyle.BackColor = Color.AliceBlue

            .Columns("APPIPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("APPIGRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("APPINETWT").DefaultCellStyle.BackColor = Color.AliceBlue

            ''VISIBLE
            If CheckOnly Then
                .Columns("CHPCS").Visible = True
                .Columns("CHGRSWT").Visible = chkGrsWt.Checked
                .Columns("CHNETWT").Visible = chkNetWt.Checked
                .Columns("CHPCS").ReadOnly = False
                .Columns("CHGRSWT").ReadOnly = False
                .Columns("CHNETWT").ReadOnly = False
                .Columns("CHPCS").HeaderText = "PCS"
                .Columns("CHGRSWT").HeaderText = "GRSWT"
                .Columns("CHNETWT").HeaderText = "NETWT"
            End If
            .Columns("OPCS").Visible = Not CheckOnly And (rbtWithBoth.Checked Or rbtWithOpening.Checked)
            .Columns("IPCS").Visible = Not CheckOnly And rbtWithBoth.Checked
            .Columns("RPCS").Visible = Not CheckOnly And rbtWithBoth.Checked
            .Columns("CPCS").Visible = Not CheckOnly And (rbtWithBoth.Checked Or rbtWithClosing.Checked)

            .Columns("ODIAPCS").Visible = ChkWithDia.Checked And rbtDiaStnByColumn.Checked
            .Columns("ODIAWT").Visible = ChkWithDia.Checked And rbtDiaStnByColumn.Checked And (chkGrsWt.Checked Or chkNetWt.Checked)
            .Columns("OSTNPCS").Visible = ChkWithStone.Checked And rbtDiaStnByColumn.Checked
            .Columns("OSTNWT").Visible = ChkWithStone.Checked And rbtDiaStnByColumn.Checked And (chkGrsWt.Checked Or chkNetWt.Checked)

            .Columns("OGRSWT").Visible = chkGrsWt.Checked And Not CheckOnly And (rbtWithBoth.Checked Or rbtWithOpening.Checked)
            .Columns("TRGRSWT").Visible = chkGrsWt.Checked And chkSeperateTransferCol.Checked And Not CheckOnly And rbtWithBoth.Checked
            .Columns("RGRSWT").Visible = chkGrsWt.Checked And Not CheckOnly And rbtWithBoth.Checked
            .Columns("TIGRSWT").Visible = chkGrsWt.Checked And chkSeperateTransferCol.Checked And Not CheckOnly And rbtWithBoth.Checked
            .Columns("IGRSWT").Visible = chkGrsWt.Checked And Not CheckOnly And rbtWithBoth.Checked
            .Columns("MIGRSWT").Visible = chkGrsWt.Checked And ChkMiscIssue.Checked And Not CheckOnly And rbtWithBoth.Checked
            .Columns("CGRSWT").Visible = chkGrsWt.Checked And Not CheckOnly And (rbtWithBoth.Checked Or rbtWithClosing.Checked)
            If .Columns.Contains("PSGRSWT") Then
                .Columns("PSGRSWT").Visible = chkGrsWt.Checked And ChkPartSale.Checked And Not CheckOnly And rbtWithBoth.Checked
            End If
            .Columns("TRDIAPCS").Visible = ChkWithDia.Checked And rbtDiaStnByColumn.Checked
            .Columns("TRDIAWT").Visible = ChkWithDia.Checked And rbtDiaStnByColumn.Checked And (chkGrsWt.Checked Or chkNetWt.Checked)
            .Columns("TRSTNPCS").Visible = ChkWithStone.Checked And rbtDiaStnByColumn.Checked
            .Columns("TRSTNWT").Visible = ChkWithStone.Checked And rbtDiaStnByColumn.Checked And (chkGrsWt.Checked Or chkNetWt.Checked)
            .Columns("RDIAPCS").Visible = ChkWithDia.Checked And rbtDiaStnByColumn.Checked
            .Columns("RDIAWT").Visible = ChkWithDia.Checked And rbtDiaStnByColumn.Checked And (chkGrsWt.Checked Or chkNetWt.Checked)
            .Columns("RSTNPCS").Visible = ChkWithStone.Checked And rbtDiaStnByColumn.Checked
            .Columns("RSTNWT").Visible = ChkWithStone.Checked And rbtDiaStnByColumn.Checked And (chkGrsWt.Checked Or chkNetWt.Checked)

            .Columns("ONETWT").Visible = chkNetWt.Checked And Not CheckOnly And (rbtWithBoth.Checked Or rbtWithOpening.Checked)
            .Columns("TRNETWT").Visible = chkNetWt.Checked And chkSeperateTransferCol.Checked And Not CheckOnly And rbtWithBoth.Checked
            .Columns("RNETWT").Visible = chkNetWt.Checked And Not CheckOnly And rbtWithBoth.Checked
            .Columns("TINETWT").Visible = chkNetWt.Checked And chkSeperateTransferCol.Checked And Not CheckOnly And rbtWithBoth.Checked
            .Columns("INETWT").Visible = chkNetWt.Checked And Not CheckOnly And rbtWithBoth.Checked
            .Columns("MINETWT").Visible = chkNetWt.Checked And ChkMiscIssue.Checked And Not CheckOnly And rbtWithBoth.Checked
            If .Columns.Contains("PSNETWT") Then .Columns("PSNETWT").Visible = chkNetWt.Checked And ChkPartSale.Checked And Not CheckOnly And rbtWithBoth.Checked
            .Columns("CNETWT").Visible = chkNetWt.Checked And Not CheckOnly And (rbtWithBoth.Checked Or rbtWithClosing.Checked)

            .Columns("TIDIAPCS").Visible = ChkWithDia.Checked And rbtDiaStnByColumn.Checked
            .Columns("TIDIAWT").Visible = ChkWithDia.Checked And rbtDiaStnByColumn.Checked And (chkGrsWt.Checked Or chkNetWt.Checked)
            .Columns("TISTNPCS").Visible = ChkWithStone.Checked And rbtDiaStnByColumn.Checked
            .Columns("TISTNWT").Visible = ChkWithStone.Checked And rbtDiaStnByColumn.Checked And (chkGrsWt.Checked Or chkNetWt.Checked)
            .Columns("IDIAPCS").Visible = ChkWithDia.Checked And rbtDiaStnByColumn.Checked
            .Columns("IDIAWT").Visible = ChkWithDia.Checked And rbtDiaStnByColumn.Checked And (chkGrsWt.Checked Or chkNetWt.Checked)
            .Columns("ISTNPCS").Visible = ChkWithStone.Checked And rbtDiaStnByColumn.Checked
            .Columns("ISTNWT").Visible = ChkWithStone.Checked And rbtDiaStnByColumn.Checked And (chkGrsWt.Checked Or chkNetWt.Checked)


            .Columns("TRPCS").Visible = chkSeperateTransferCol.Checked And Not CheckOnly And rbtWithBoth.Checked
            .Columns("TIPCS").Visible = chkSeperateTransferCol.Checked And Not CheckOnly And rbtWithBoth.Checked
            .Columns("MIPCS").Visible = ChkMiscIssue.Checked And Not CheckOnly And rbtWithBoth.Checked
            If .Columns.Contains("PSPCS") Then .Columns("PSPCS").Visible = ChkPartSale.Checked And Not CheckOnly And rbtWithBoth.Checked

            .Columns("CDIAPCS").Visible = ChkWithDia.Checked And rbtDiaStnByColumn.Checked
            .Columns("CDIAWT").Visible = ChkWithDia.Checked And rbtDiaStnByColumn.Checked And (chkGrsWt.Checked Or chkNetWt.Checked)
            .Columns("CSTNPCS").Visible = ChkWithStone.Checked And rbtDiaStnByColumn.Checked
            .Columns("CSTNWT").Visible = ChkWithStone.Checked And rbtDiaStnByColumn.Checked And (chkGrsWt.Checked Or chkNetWt.Checked)


            If chkSepApprovalColumn.Checked = True Then
                .Columns("APPOPCS").Visible = Not CheckOnly And (rbtWithBoth.Checked Or rbtWithOpening.Checked)
                .Columns("APPRPCS").Visible = Not CheckOnly And rbtWithBoth.Checked
                .Columns("APPIPCS").Visible = Not CheckOnly And rbtWithBoth.Checked
                .Columns("APPCPCS").Visible = Not CheckOnly And (rbtWithBoth.Checked Or rbtWithOpening.Checked)

                .Columns("APPOGRSWT").Visible = chkGrsWt.Checked And Not CheckOnly And (rbtWithBoth.Checked Or rbtWithOpening.Checked)
                .Columns("APPRGRSWT").Visible = chkGrsWt.Checked And Not CheckOnly And rbtWithBoth.Checked
                .Columns("APPIGRSWT").Visible = chkGrsWt.Checked And Not CheckOnly And rbtWithBoth.Checked
                .Columns("APPCGRSWT").Visible = chkGrsWt.Checked And Not CheckOnly And (rbtWithBoth.Checked Or rbtWithClosing.Checked)

                .Columns("APPONETWT").Visible = chkNetWt.Checked And Not CheckOnly And (rbtWithBoth.Checked Or rbtWithOpening.Checked)
                .Columns("APPRNETWT").Visible = chkNetWt.Checked And Not CheckOnly And rbtWithBoth.Checked
                .Columns("APPINETWT").Visible = chkNetWt.Checked And Not CheckOnly And rbtWithBoth.Checked
                .Columns("APPCNETWT").Visible = chkNetWt.Checked And Not CheckOnly And (rbtWithBoth.Checked Or rbtWithClosing.Checked)

            ElseIf chkOnlyApproval.Checked = True And chkSepApprovalColumn.Checked = False Then
                .Columns("APPOPCS").Visible = Not CheckOnly And (rbtWithBoth.Checked Or rbtWithOpening.Checked)
                .Columns("APPRPCS").Visible = Not CheckOnly And rbtWithBoth.Checked
                .Columns("APPIPCS").Visible = Not CheckOnly And rbtWithBoth.Checked
                .Columns("APPCPCS").Visible = Not CheckOnly And (rbtWithBoth.Checked Or rbtWithOpening.Checked)

                .Columns("APPOGRSWT").Visible = chkGrsWt.Checked And Not CheckOnly And (rbtWithBoth.Checked Or rbtWithOpening.Checked)
                .Columns("APPRGRSWT").Visible = chkGrsWt.Checked And Not CheckOnly And rbtWithBoth.Checked
                .Columns("APPIGRSWT").Visible = chkGrsWt.Checked And Not CheckOnly And rbtWithBoth.Checked
                .Columns("APPCGRSWT").Visible = chkGrsWt.Checked And Not CheckOnly And (rbtWithBoth.Checked Or rbtWithClosing.Checked)

                .Columns("APPONETWT").Visible = chkNetWt.Checked And Not CheckOnly And (rbtWithBoth.Checked Or rbtWithOpening.Checked)
                .Columns("APPRNETWT").Visible = chkNetWt.Checked And Not CheckOnly And rbtWithBoth.Checked
                .Columns("APPINETWT").Visible = chkNetWt.Checked And Not CheckOnly And rbtWithBoth.Checked
                .Columns("APPCNETWT").Visible = chkNetWt.Checked And Not CheckOnly And (rbtWithBoth.Checked Or rbtWithClosing.Checked)

                .Columns("OPCS").Visible = False
                .Columns("IPCS").Visible = False
                .Columns("RPCS").Visible = False
                .Columns("CPCS").Visible = False

                .Columns("OGRSWT").Visible = False
                .Columns("TRGRSWT").Visible = False
                .Columns("RGRSWT").Visible = False
                .Columns("TIGRSWT").Visible = False
                .Columns("IGRSWT").Visible = False
                .Columns("MIGRSWT").Visible = False
                .Columns("CGRSWT").Visible = False

                .Columns("ONETWT").Visible = False
                .Columns("TRNETWT").Visible = False
                .Columns("RNETWT").Visible = False
                .Columns("TINETWT").Visible = False
                .Columns("INETWT").Visible = False
                .Columns("MINETWT").Visible = False
                .Columns("CNETWT").Visible = False

                .Columns("TRPCS").Visible = False
                .Columns("TIPCS").Visible = False
                .Columns("MIPCS").Visible = False

            Else
                .Columns("APPOPCS").Visible = False
                .Columns("APPOGRSWT").Visible = False
                .Columns("APPONETWT").Visible = False
                .Columns("APPRPCS").Visible = False
                .Columns("APPRGRSWT").Visible = False
                .Columns("APPRNETWT").Visible = False
                .Columns("APPIPCS").Visible = False
                .Columns("APPIGRSWT").Visible = False
                .Columns("APPINETWT").Visible = False
                .Columns("APPCPCS").Visible = False
                .Columns("APPCGRSWT").Visible = False
                .Columns("APPCNETWT").Visible = False
            End If

            .Columns("NOOFTAGS").Visible = ChkWithNoofTags.Checked

            .Columns("OSALVALUE").Visible = ChkSalvalue.Checked
            .Columns("RSALVALUE").Visible = ChkSalvalue.Checked
            .Columns("ISALVALUE").Visible = ChkSalvalue.Checked
            .Columns("CSALVALUE").Visible = ChkSalvalue.Checked

            .Columns("OPURVALUE").Visible = ChkPurchase.Checked
            .Columns("RPURVALUE").Visible = ChkPurchase.Checked
            .Columns("IPURVALUE").Visible = ChkPurchase.Checked
            .Columns("CPURVALUE").Visible = ChkPurchase.Checked

            .Columns("RATE").Visible = ChkRate.Checked


            .Columns("ITEMCTRNAME").Visible = False
            .Columns("ITEMNAME").Visible = False
            .Columns("SUBITEMNAME").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("STONE").Visible = False
            .Columns("SGROUPID").Visible = False

        End With
    End Sub
    Public Function GetSelectedMetalid(ByVal chkLst As ComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Text <> "ALL" Then
            If WithQuotes Then retStr += "'"
            retStr = objGPack.GetSqlValue("SELECT Metalid FROM " & cnAdminDb & "..MetalMast WHERE MetalName= '" & chkLst.Text.ToString & "'")
            If WithQuotes Then retStr += "'"
        Else
            retStr = "ALL"
        End If
        Return retStr
    End Function

    Public Function GetSelecteditemids(ByVal chkLst As ComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Text <> "ALL" Then
            If WithQuotes Then retStr += "'"
            retStr = objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME= '" & chkLst.Text.ToString & "'")
            If WithQuotes Then retStr += "'"
        Else
            retStr = "ALL"
        End If
        Return retStr
    End Function
    Public Function GetSelectedDesignerid(ByVal chkLst As ComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Text <> "ALL" Then
            If WithQuotes Then retStr += "'"
            retStr = objGPack.GetSqlValue("SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME= '" & chkLst.Text.ToString & "'")
            If WithQuotes Then retStr += "'"
        Else
            retStr = "ALL"
        End If
        Return retStr
    End Function
    Public Function GetSelectedCatCode(ByVal chkLst As ComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Text <> "ALL" Then
            If WithQuotes Then retStr += "'"
            retStr = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME= '" & chkLst.Text.ToString & "'")
            If WithQuotes Then retStr += "'"
        Else
            retStr = "ALL"
        End If
        Return retStr
    End Function

    Public Function GetSelecteditemtypeid(ByVal chkLst As CheckedListBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME= '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "ALL"
        End If
        Return retStr
    End Function

    Public Function GetSelectedCostId(ByVal chkLst As CheckedListBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME= '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "ALL"
        End If
        Return retStr
    End Function

    Public Function GetSelectedCounderid(ByVal chkLst As CheckedListBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME= '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "ALL"
        End If
        Return retStr
    End Function

    Public Function GetCounderid(ByVal countername As String, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If countername <> "" Then
            If WithQuotes Then retStr += "'"
            retStr += objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME= '" & countername.ToString & "'")
            If WithQuotes Then retStr += "'"
        Else
            retStr = "ALL"
        End If
        Return retStr
    End Function
    Public Function GetSelectedsubitemgrpid(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT SGROUPID FROM " & cnAdminDb & "..SUBITEMGROUP WHERE SGROUPNAME= '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "''"
        End If
        Return retStr
    End Function
    Private Sub Report()
        Dim RecDate As String = Nothing
        gridView.DataSource = Nothing
        If Not chkLstCompany.CheckedItems.Count > 0 Then chkCompanySelectAll.Checked = True
        Dim SelectedCompany As String = GetSelectedCompanyId(chkLstCompany, False)
        Dim SelectedCounter As String
        'SelectedCounter = GetChecked_CheckedList(chkLstCounter, False)
        'SelectedCounter = GetChecked_CheckedListid(chkLstCounter, "ITEMCTRID", "ITEMCTRNAME", cnAdminDb & "..ITEMCOUNTER", False)
        SelectedCounter = GetSelectedCounderid(chkLstCounter, False)
        Dim SelectedItemType As String = GetChecked_CheckedList(chkLstItemType, False)
        Dim SelectedCategory As String
        If chkMultiCat.Checked Then SelectedCategory = GetChecked_CheckedListid(chkCmbCategory, "Catcode", "Catname", cnAdminDb & "..CATEGORY", False) Else SelectedCategory = GetSelectedCatCode(cmbCategory, False)

        Dim SelectedCostCentre As String = GetChecked_CheckedList(chkLstCostCentre, False)
        'Dim SelectedCostCentre As String = 
        Dim TranCol As String = "P"
        If chkGrsWt.Checked Then TranCol += "G"
        If chkNetWt.Checked Then TranCol += "N"
        Dim DiaStnRow As String = ""
        'If SelectedCounter.Split(",").Length > 20 And chkLstCounter.CheckedItems.Count <> chkLstCounter.Items.Count Then MsgBox("Selected counters in List is more" & vbCrLf & "May be the process is break", MsgBoxStyle.MsgBoxHelp)
        If ChkWithDia.Checked = True And ChkWithStone.Checked = True And rbtDiaStnByRow.Checked = True Then
            DiaStnRow = "D,S"
        ElseIf ChkWithDia.Checked = True And ChkWithStone.Checked = False And rbtDiaStnByRow.Checked = True Then
            DiaStnRow = "D"
        ElseIf ChkWithDia.Checked = False And ChkWithStone.Checked = True And rbtDiaStnByRow.Checked = True Then
            DiaStnRow = "S"
        Else
            DiaStnRow = "N"
        End If
        Dim Groupby As String = ""
        If rbtCostcentre.Checked Then
            Groupby = "CO"
        ElseIf rbtCounterGrp.Checked Then
            Groupby = "CG"
        ElseIf rbtSubItemGroup.Checked Then
            Groupby = "SG"
        Else
            Groupby = "CO"
        End If
        '600
        If CounterWiseStockP = "" Then CounterWiseStockP = "SP_RPT_COUNTERWISESTOCK"

        strSql = " EXEC " & cnAdminDb & ".." & CounterWiseStockP
        strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
        strSql += vbCrLf + " ,@ASONDATE = '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@METAL = '" & GetSelectedMetalid(cmbMetal, False) & "'"
        strSql += vbCrLf + " ,@CATCODE = '" & SelectedCategory & "'"
        strSql += vbCrLf + " ,@ITEMIDS = '" & GetSelecteditemids(cmbItemName, False) & "'"
        strSql += vbCrLf + " ,@DESIGNERID = '" & GetSelectedDesignerid(cmbDesigner, False) & "'"
        strSql += vbCrLf + " ,@SUMMARY = '" & IIf(rbtSummary.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@WITHSUBITEM = '" & IIf(chkWithSubItem.Checked, "Y", "N") & "'"
        If rbtNonTag.Checked Then
            strSql += vbCrLf + ",@STOCKTYPE = 'N'"
        ElseIf rbtTag.Checked Then
            strSql += vbCrLf + ",@STOCKTYPE = 'T'"
        Else
            strSql += vbCrLf + ",@STOCKTYPE = 'B'"
        End If
        strSql += vbCrLf + " ,@COSTIDS = '" & IIf(chkAllCostCentre.Checked = True, "ALL", GetSelectedCostId(chkLstCostCentre, False)) & "'"
        strSql += vbCrLf + " ,@ITEMTYPEIDS = '" & IIf(chkAllItemType.Checked = True, "ALL", GetSelecteditemtypeid(chkLstItemType, False)) & "'"
        If ChkMultiCounter.Checked Then
            'SelectedCounter = GetChecked_CheckedList(chkLstCounter, False)
            If chkLstCounter.CheckedItems.Count = chkLstCounter.Items.Count Then SelectedCounter = "ALL"
            'strSql += vbCrLf + " ,@COUNTERID = '" & GetSelectedCounderid(chkLstCounter, False) & "'"
            strSql += vbCrLf + " ,@COUNTERID = '" & SelectedCounter & "'"
        Else
            strSql += vbCrLf + " ,@COUNTERID = '" & GetCounderid(IIf(cmbCounter.Text.Trim <> "" And cmbCounter.Text.Trim <> "ALL", cmbCounter.Text, ""), False) & "'"
        End If
        strSql += vbCrLf + " ,@COMPANYID = '" & IIf(chkCompanySelectAll.Checked = True, "ALL", SelectedCompany) & "'"
        strSql += vbCrLf + " ,@ORDER = '" & IIf(rbtOrder.Checked, "O", "") & "'"
        If chkOnlyApproval.Checked = True And ChkWithApproval.Checked = False Then
            strSql += vbCrLf + " ,@APPROVAL = 'O'"
        ElseIf ChkWithApproval.Checked = True And chkOnlyApproval.Checked = False Then
            strSql += vbCrLf + " ,@APPROVAL = 'W'"
        Else
            strSql += vbCrLf + " ,@APPROVAL = ''"
        End If
        strSql += vbCrLf + " ,@TRANSFER_DETAIL = '" & IIf(chkSeperateTransferCol.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@TRANSACTIONONLY = '" & IIf(chkTransactionOnly.Checked, TranCol, "") & "'"
        strSql += vbCrLf + " ,@ORDERBYID = '" & IIf(chkOrderbyId.Checked, "Y", "N") & "'"
        ' Dim GUID As New System.Guid
        ' LSet(Mid(str3, 1, InStr(str3, vbCr) - 1), 45)
        Dim SYSTEMID As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
        strSql += vbCrLf + " ,@SYSTEMID = '" & SYSTEMID & "'"
        strSql += vbCrLf + " ,@SEPMISC = '" & IIf(ChkMiscIssue.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@SEPAPP = '" & IIf(chkSepApprovalColumn.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@DIASTNBYROW='" & DiaStnRow & "'"
        strSql += vbCrLf + " ,@SEPPARTSALE = '" & IIf(ChkPartSale.Checked, "Y", "N") & "'"
        If CounterWiseStockP = "SP_RPT_COUNTERWISESTOCK" Or CounterWiseStockP = "SP_RPT_COUNTERWISESTOCK_F1" Then
            strSql += vbCrLf + " ,@GROUPBY='" & Groupby & "'" 'IIf(rbtCostcentre.Checked, "CO", "CG")
        End If
        strSql += vbCrLf + " ,@SUBITEMGROUP = '" & GetSelectedsubitemgrpid(cmbSubItemGroup, False) & "'"
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        If rbtSummary.Checked And ChkSubtotal.Checked = False Then
            strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & SYSTEMID & "STOCKREPORT WHERE RESULT <> 2  " 'AND COLHEAD<>'S' "
            strSql += "ORDER BY "
            If CounterWiseStockP = "SP_RPT_COUNTERWISESTOCK" Or CounterWiseStockP = "SP_RPT_COUNTERWISESTOCK_F1" Then
                'strSql += "" & IIf(rbtCostcentre.Checked, "COSTID,", "GROUPNAME,") & ""
                If rbtCostcentre.Checked Then
                    strSql += "COSTID,"
                ElseIf rbtCounterGrp.Checked Then
                    strSql += "GROUPNAME,"
                ElseIf rbtSubItemGroup.Checked Then
                    strSql += "SGROUPNAME,"
                Else
                    strSql += "COSTID,"
                End If
            End If
            If rbtSummary.Checked = True And rbtSubItemGroup.Checked = True Then
            Else
                strSql += "ITEMCTRNAME,"
            End If
            strSql += "RESULT,ITEMNAME"
        Else
            strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & SYSTEMID & "STOCKREPORT "
            strSql += " ORDER BY "
            If CounterWiseStockP = "SP_RPT_COUNTERWISESTOCK" Or CounterWiseStockP = "SP_RPT_COUNTERWISESTOCK_F1" Then
                'strSql += "" & IIf(rbtCostcentre.Checked, "COSTID,", "GROUPNAME,") & ""
                If rbtCostcentre.Checked Then
                    strSql += "COSTID,"
                ElseIf rbtCounterGrp.Checked Then
                    strSql += "GROUPNAME,"
                ElseIf rbtSubItemGroup.Checked Then
                    strSql += "SGROUPNAME,"
                Else
                    strSql += "COSTID,"
                End If
            End If
            If rbtSummary.Checked = True And rbtSubItemGroup.Checked = True Then
            Else
                strSql += "ITEMCTRNAME,"
            End If
            strSql += "RESULT,ITEMNAME"
        End If
        Dim dt As New DataTable
        dt.Columns.Add("KEYNO", GetType(Integer))

        dt.Columns("KEYNO").AutoIncrement = True
        dt.Columns("KEYNO").AutoIncrementSeed = 0
        dt.Columns("KEYNO").AutoIncrementStep = 1
        'If Is_Oldformat = "Y" Then
        '    dt.Columns.Add("APCS", GetType(Integer))
        '    dt.Columns.Add("ANETWT", GetType(Integer))
        '    dt.Columns.Add("AGRSWT", GetType(Integer))
        'End If
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        Prop_Sets()
        If Not dt.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            dtpAsOnDate.Focus()
            Exit Sub
        End If
        dt.Columns("KEYNO").SetOrdinal(dt.Columns.Count - 1)
        If CheckOnly Then
            gridViewHead.Visible = False
            dt.Columns.Add("CHPCS", GetType(String))
            dt.Columns.Add("CHGRSWT", GetType(String))
            dt.Columns.Add("CHNETWT", GetType(String))
            dt.Columns.Add("STATUS", GetType(String))
        End If
        tabView.Show()
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        gridView.DataSource = dt
        'FOR PENDING TRANSFER
        If chkPendTr.Checked Then
            Dim psds As New DataSet
            Dim psdt As New DataTable
            strSql = " EXEC " & cnAdminDb & "..SP_RPT_PENDINGSTK"
            strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@METALID = '" & GetSelectedMetalid(cmbMetal, False) & "'"
            strSql += vbCrLf + " ,@ITEMIDS = '" & GetSelecteditemids(cmbItemName, False) & "'"
            strSql += vbCrLf + " ,@COMPANYID = '" & IIf(chkCompanySelectAll.Checked = True, "ALL", SelectedCompany) & "'"
            strSql += vbCrLf + " ,@COSTIDS = '" & IIf(chkAllCostCentre.Checked = True, "ALL", GetSelectedCostId(chkLstCostCentre, False)) & "'"
            cmd = New OleDbCommand(strSql, cn)
            da = New OleDbDataAdapter(cmd)
            da.Fill(psds)
            psdt = psds.Tables(0)
            If psdt.Rows.Count > 0 Then
                CType(gridView.DataSource, DataTable).Rows.Add()
                CType(gridView.DataSource, DataTable).Rows.Add()
                gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "PENDING TRANSFER"
                gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle

                For ii As Integer = 0 To psdt.Rows.Count - 1
                    CType(gridView.DataSource, DataTable).Rows.Add()
                    With psdt.Rows(ii)
                        Dim mitemname As String = .Item("ITEMNAME").ToString
                        If mitemname = "" Then mitemname = .Item("CATNAME").ToString Else mitemname = "**" & mitemname
                        gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = mitemname
                        gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = .Item("PCS")
                        gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = .Item("GRSWT")
                        gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = .Item("NETWT")
                    End With
                Next
                CType(gridView.DataSource, DataTable).Rows.Add()
                gridView.Rows(gridView.RowCount - 1).Cells("PARTICULAR").Value = "TOTAL"
                gridView.Rows(gridView.RowCount - 1).Cells("COLHEAD").Value = "G"
                gridView.Rows(gridView.RowCount - 1).Cells("OPCS").Value = psdt.Compute("SUM(PCS)", "")
                gridView.Rows(gridView.RowCount - 1).Cells("OGRSWT").Value = psdt.Compute("SUM(GRSWT)", "")
                gridView.Rows(gridView.RowCount - 1).Cells("ONETWT").Value = psdt.Compute("SUM(NETWT)", "")
                gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
            End If
        End If

        lblTitle.Text = "COUNTER WISE STOCK CHECK"
        If cmbMetal.Text <> "" And cmbMetal.Text <> "ALL" Then
            lblTitle.Text += " FOR [" & cmbMetal.Text & "]"
        End If
        lblTitle.Text += " AS ON " & dtpAsOnDate.Value.ToString("dd-MM-yyyy")
        GridViewHeaderStyle()
        GridStyle()
        gridView.Columns("COLHEAD").Visible = False
        If gridView.Columns.Contains("GROUPNAME") Then gridView.Columns("GROUPNAME").Visible = False
        If gridView.Columns.Contains("SGROUPNAME") Then gridView.Columns("SGROUPNAME").Visible = False
        'If Is_Oldformat = "Y" Then
        '    gridView.Columns("APCS").Visible = False
        '    gridView.Columns("ANETWT").Visible = False
        '    gridView.Columns("AGRSWT").Visible = False
        'End If
        GridViewHeaderStyle()
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        tabMain.SelectedTab = tabView
    End Sub

    Private Sub GridViewHeaderStyle()
        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        Dim strOapproval As String = ""
        Dim strIapproval As String = ""
        Dim strRapproval As String = ""
        Dim strCapproval As String = ""
        Dim strSepPartlySale As String = ""
        If chkSepApprovalColumn.Checked = True Or chkOnlyApproval.Checked = True Then strOapproval = "~APPOPCS~APPOGRSWT~APPONETWT~OTAGPCS"
        If chkSepApprovalColumn.Checked = True Or chkOnlyApproval.Checked = True Then strRapproval = "~APPRPCS~APPRGRSWT~APPRNETWT~RTAGPCS"
        If chkSepApprovalColumn.Checked = True Or chkOnlyApproval.Checked = True Then strIapproval = "~APPIPCS~APPIGRSWT~APPINETWT~ITAGPCS"
        If chkSepApprovalColumn.Checked = True Or chkOnlyApproval.Checked = True Then strCapproval = "~APPCPCS~APPCGRSWT~APPCNETWT~CTAGPCS"
        If ChkPartSale.Checked = True Then
            If gridView.Columns.Contains("PSPCS") Then If gridView.Columns("PSPCS").Visible Then strSepPartlySale = strSepPartlySale + "~PSPCS"
            If gridView.Columns.Contains("PSGRSWT") Then If gridView.Columns("PSGRSWT").Visible Then strSepPartlySale = strSepPartlySale + "~PSGRSWT"
            If gridView.Columns.Contains("PSNETWT") Then If gridView.Columns("PSNETWT").Visible Then strSepPartlySale = strSepPartlySale + "~PSNETWT"
        End If
        With dtMergeHeader
            .Columns.Add("PARTICULAR", GetType(String))
            .Columns.Add("OPCS~OGRSWT~ONETWT~ODIAPCS~ODIAWT~OSTNPCS~OSTNWT~OTAGPCS~OSALVALUE~OPURVALUE" & strOapproval, GetType(String))
            .Columns.Add("TRPCS~TRGRSWT~TRNETWT~TRDIAPCS~TRDIAWT~TRSTNPCS~TRSTNWT~TRTAGPCS~RPCS~RGRSWT~RNETWT~RDIAPCS~RDIAWT~RSTNPCS~RSTNWT~RTAGPCS~RSALVALUE~RPURVALUE" & strRapproval, GetType(String))
            .Columns.Add("TIPCS~TIGRSWT~TINETWT~TIDIAPCS~TIDIAWT~TISTNPCS~TISTNWT~TITAGPCS~IPCS~IGRSWT~INETWT~IDIAPCS~IDIAWT~ISTNPCS~ISTNWT~ITAGPCS~MIPCS~MIGRSWT~MINETWT~MITAGPCS~APCS~AGRSWT~ANETWT~ATAGPCS~ISALVALUE~IPURVALUE" & strIapproval & strSepPartlySale, GetType(String))
            .Columns.Add("CPCS~CGRSWT~CNETWT~CDIAPCS~CDIAWT~CSTNPCS~CSTNWT~CTAGPCS~CSALVALUE~CPURVALUE" & strCapproval, GetType(String))
            .Columns.Add("RATE", GetType(String))
            .Columns.Add("NOOFTAGS", GetType(String))
            .Columns.Add("SCROLL", GetType(String))
            .Columns("PARTICULAR").Caption = ""
            .Columns("OPCS~OGRSWT~ONETWT~ODIAPCS~ODIAWT~OSTNPCS~OSTNWT~OTAGPCS~OSALVALUE~OPURVALUE" & strOapproval).Caption = "OPENING"
            .Columns("TRPCS~TRGRSWT~TRNETWT~TRDIAPCS~TRDIAWT~TRSTNPCS~TRSTNWT~TRTAGPCS~RPCS~RGRSWT~RNETWT~RDIAPCS~RDIAWT~RSTNPCS~RSTNWT~RTAGPCS~RSALVALUE~RPURVALUE" & strRapproval).Caption = "RECEIPT"
            .Columns("TIPCS~TIGRSWT~TINETWT~TIDIAPCS~TIDIAWT~TISTNPCS~TISTNWT~TITAGPCS~IPCS~IGRSWT~INETWT~IDIAPCS~IDIAWT~ISTNPCS~ISTNWT~ITAGPCS~MIPCS~MIGRSWT~MINETWT~MITAGPCS~APCS~AGRSWT~ANETWT~ATAGPCS~ISALVALUE~IPURVALUE" & strIapproval & strSepPartlySale).Caption = "ISSUE"
            .Columns("CPCS~CGRSWT~CNETWT~CDIAPCS~CDIAWT~CSTNPCS~CSTNWT~CTAGPCS~CSALVALUE~CPURVALUE" & strCapproval).Caption = "CLOSING"
            .Columns("RATE").Caption = "RATE"
            .Columns("NOOFTAGS").Caption = "NOOFTAGS"
            .Columns("SCROLL").Caption = ""
        End With

        With gridViewHead
            .DataSource = dtMergeHeader
            .Columns("PARTICULAR").HeaderText = ""
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            funcColWidth()
            gridView.Focus()
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To gridView.ColumnCount - 1
                If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
            Next
            If colWid >= gridView.Width Then
                .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            Else
                .Columns("SCROLL").Visible = False
            End If
        End With

    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If chkOnlyApproval.Checked = True And rbtTag.Checked = False Then
            MsgBox("Approval is allowed for Taged Items only", MsgBoxStyle.Information)
            rbtTag.Checked = True
            Exit Sub
        End If

        If chkOnlyTag.Checked = True And rbtTag.Checked = False Then
            MsgBox("Filtering allowed for Taged Items only", MsgBoxStyle.Information)
            rbtTag.Checked = True
            Exit Sub
        End If
        If Not chkLstCostCentre.CheckedItems.Count > 0 Then
            chkAllCostCentre.Checked = True
        End If
        If Not chkLstCounter.CheckedItems.Count > 0 Then
            chkAllCounter.Checked = True
        End If
        If Not chkLstItemType.CheckedItems.Count > 0 Then
            chkAllItemType.Checked = True
        End If
        If Not CheckBckDays(userId, Me.Name, dtpAsOnDate.Value) Then dtpAsOnDate.Focus() : Exit Sub
        Me.Refresh()
        Report()
    End Sub

    Private Sub frmItemWiseStock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Escape) And tabMain.SelectedTab.Name = tabView.Name Then
            btnBack_Click(Me, New EventArgs)
        ElseIf e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub gridView_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles gridView.CellBeginEdit
        If gridView.Rows(e.RowIndex).Cells("COLHEAD").Value.ToString = "T" Then
            e.Cancel = True
        End If
    End Sub

    Private Sub gridView_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellEndEdit
        celWasEndEdit = gridView(e.ColumnIndex, e.RowIndex)
        Dim pcs As Integer = 0
        Dim grsWt As Decimal = 0
        Dim netWt As Decimal = 0

        If IsNumeric(gridView.Rows(e.RowIndex).Cells("CHPCS").Value.ToString) = False Then
            gridView.Rows(e.RowIndex).Cells("CHPCS").Value = ""
        End If
        If IsNumeric(gridView.Rows(e.RowIndex).Cells("CHGRSWT").Value.ToString) = False Then
            gridView.Rows(e.RowIndex).Cells("CHGRSWT").Value = ""
        End If
        If IsNumeric(gridView.Rows(e.RowIndex).Cells("CHNETWT").Value.ToString) = False Then
            gridView.Rows(e.RowIndex).Cells("CHNETWT").Value = ""
        End If

        With gridView.Rows(e.RowIndex)
            pcs = Val(.Cells("CPCS").Value.ToString) - Val(.Cells("CHPCS").Value.ToString)
            If chkGrsWt.Checked Then
                grsWt = Val(.Cells("CGRSWT").Value.ToString) - Val(.Cells("CHGRSWT").Value.ToString)
            End If
            If chkNetWt.Checked Then
                netWt = Val(.Cells("CNETWT").Value.ToString) - Val(.Cells("CHNETWT").Value.ToString)
            End If
            If CTRSTK_PCSCHK Then
                If pcs = 0 And grsWt = 0 And netWt = 0 Then
                    .Cells("STATUS").Value = "Ok"
                Else
                    .Cells("STATUS").Value = "DIFFER"
                End If
            Else
                strSql = "SELECT STOCKTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & .Cells("ITEMNAME").Value.ToString & "'"
                Dim stkType As String = objGPack.GetSqlValue(strSql, "STOCKTYPE", "").ToString
                If stkType <> "N" And .Cells("PARTICULAR").Value.ToString <> "SUBTOTAL" And .Cells("PARTICULAR").Value.ToString <> "GRANDTOTAL" Then
                    If pcs = 0 And grsWt = 0 And netWt = 0 Then
                        .Cells("STATUS").Value = "Ok"
                    Else
                        .Cells("STATUS").Value = "DIFFER"
                    End If
                Else
                    If grsWt = 0 And netWt = 0 Then
                        .Cells("STATUS").Value = "Ok"
                    Else
                        .Cells("STATUS").Value = "DIFFER"
                    End If
                End If
            End If
        End With
    End Sub

    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView.ColumnWidthChanged
        If gridViewHead.ColumnCount > 0 Then
            funcColWidth()
        End If
    End Sub

    Function funcColWidth() As Integer
        With gridViewHead
            Dim strOapproval As String = ""
            Dim strIapproval As String = ""
            Dim strRapproval As String = ""
            Dim strCapproval As String = ""
            Dim strSepPartlySale As String = ""
            Dim psColWidth As Double = 0
            If chkSepApprovalColumn.Checked = True Or chkOnlyApproval.Checked = True Then strOapproval = "~APPOPCS~APPOGRSWT~APPONETWT~OTAGPCS"
            If chkSepApprovalColumn.Checked = True Or chkOnlyApproval.Checked = True Then strRapproval = "~APPRPCS~APPRGRSWT~APPRNETWT~RTAGPCS"
            If chkSepApprovalColumn.Checked = True Or chkOnlyApproval.Checked = True Then strIapproval = "~APPIPCS~APPIGRSWT~APPINETWT~ITAGPCS"
            If chkSepApprovalColumn.Checked = True Or chkOnlyApproval.Checked = True Then strCapproval = "~APPCPCS~APPCGRSWT~APPCNETWT~CTAGPCS"
            .Columns("SCROLL").HeaderText = ""
            .Columns("PARTICULAR").Width = gridView.Columns("PARTICULAR").Width
            If ChkPartSale.Checked = True Then
                If gridView.Columns.Contains("PSPCS") Then If gridView.Columns("PSPCS").Visible Then strSepPartlySale = strSepPartlySale + "~PSPCS"
                If gridView.Columns.Contains("PSPCS") Then If gridView.Columns("PSPCS").Visible Then psColWidth = gridView.Columns("PSPCS").Width
                If gridView.Columns.Contains("PSGRSWT") Then If gridView.Columns("PSGRSWT").Visible Then strSepPartlySale = strSepPartlySale + "~PSGRSWT"
                If gridView.Columns.Contains("PSGRSWT") Then If gridView.Columns("PSGRSWT").Visible Then psColWidth = gridView.Columns("PSGRSWT").Width
                If gridView.Columns.Contains("PSNETWT") Then If gridView.Columns("PSNETWT").Visible Then strSepPartlySale = strSepPartlySale + "~PSNETWT"
                If gridView.Columns.Contains("PSNETWT") Then If gridView.Columns("PSNETWT").Visible Then psColWidth = gridView.Columns("PSNETWT").Width
            End If

            .Columns("OPCS~OGRSWT~ONETWT~ODIAPCS~ODIAWT~OSTNPCS~OSTNWT~OTAGPCS~OSALVALUE~OPURVALUE" & strOapproval).Visible = gridView.Columns("OPCS").Visible Or chkOnlyApproval.Checked = True
            .Columns("OPCS~OGRSWT~ONETWT~ODIAPCS~ODIAWT~OSTNPCS~OSTNWT~OTAGPCS~OSALVALUE~OPURVALUE" & strOapproval).Width =
            IIf(gridView.Columns("OPCS").Visible, gridView.Columns("OPCS").Width, 0) _
            + IIf(gridView.Columns("OGRSWT").Visible, gridView.Columns("OGRSWT").Width, 0) _
            + IIf(gridView.Columns("ONETWT").Visible, gridView.Columns("ONETWT").Width, 0) _
            + IIf(gridView.Columns("ODIAPCS").Visible, gridView.Columns("ODIAPCS").Width, 0) _
            + IIf(gridView.Columns("ODIAWT").Visible, gridView.Columns("ODIAWT").Width, 0) _
            + IIf(gridView.Columns("OSTNPCS").Visible, gridView.Columns("OSTNPCS").Width, 0) _
            + IIf(gridView.Columns("OSTNWT").Visible, gridView.Columns("OSTNWT").Width, 0) _
            + IIf(gridView.Columns("APPOPCS").Visible, gridView.Columns("APPOPCS").Width, 0) _
            + IIf(gridView.Columns("APPOGRSWT").Visible, gridView.Columns("APPOGRSWT").Width, 0) _
            + IIf(gridView.Columns("APPONETWT").Visible, gridView.Columns("APPONETWT").Width, 0) _
            + IIf(gridView.Columns("OSALVALUE").Visible, gridView.Columns("OSALVALUE").Width, 0) _
            + IIf(gridView.Columns("OPURVALUE").Visible, gridView.Columns("OPURVALUE").Width, 0)
            .Columns("OPCS~OGRSWT~ONETWT~ODIAPCS~ODIAWT~OSTNPCS~OSTNWT~OTAGPCS~OSALVALUE~OPURVALUE" & strOapproval).HeaderText = "OPENING"

            .Columns("TRPCS~TRGRSWT~TRNETWT~TRDIAPCS~TRDIAWT~TRSTNPCS~TRSTNWT~TRTAGPCS~RPCS~RGRSWT~RNETWT~RDIAPCS~RDIAWT~RSTNPCS~RSTNWT~RTAGPCS~RSALVALUE~RPURVALUE" & strRapproval).Visible = gridView.Columns("RPCS").Visible Or chkOnlyApproval.Checked = True
            .Columns("TRPCS~TRGRSWT~TRNETWT~TRDIAPCS~TRDIAWT~TRSTNPCS~TRSTNWT~TRTAGPCS~RPCS~RGRSWT~RNETWT~RDIAPCS~RDIAWT~RSTNPCS~RSTNWT~RTAGPCS~RSALVALUE~RPURVALUE" & strRapproval).Width =
            IIf(gridView.Columns("TRPCS").Visible, gridView.Columns("TRPCS").Width, 0) _
            + IIf(gridView.Columns("TRGRSWT").Visible, gridView.Columns("TRGRSWT").Width, 0) _
            + IIf(gridView.Columns("TRNETWT").Visible, gridView.Columns("TRNETWT").Width, 0) _
            + IIf(gridView.Columns("TRDIAPCS").Visible, gridView.Columns("TRDIAPCS").Width, 0) _
            + IIf(gridView.Columns("TRDIAWT").Visible, gridView.Columns("TRDIAWT").Width, 0) _
            + IIf(gridView.Columns("TRSTNPCS").Visible, gridView.Columns("TRSTNPCS").Width, 0) _
            + IIf(gridView.Columns("TRSTNWT").Visible, gridView.Columns("TRSTNWT").Width, 0) _
            + IIf(gridView.Columns("RPCS").Visible, gridView.Columns("RPCS").Width, 0) _
            + IIf(gridView.Columns("RGRSWT").Visible, gridView.Columns("RGRSWT").Width, 0) _
            + IIf(gridView.Columns("RNETWT").Visible, gridView.Columns("RNETWT").Width, 0) _
            + IIf(gridView.Columns("RDIAPCS").Visible, gridView.Columns("RDIAPCS").Width, 0) _
            + IIf(gridView.Columns("RDIAWT").Visible, gridView.Columns("RDIAWT").Width, 0) _
            + IIf(gridView.Columns("RSTNPCS").Visible, gridView.Columns("RSTNPCS").Width, 0) _
            + IIf(gridView.Columns("RSTNWT").Visible, gridView.Columns("RSTNWT").Width, 0) _
            + IIf(gridView.Columns("APPRPCS").Visible, gridView.Columns("APPRPCS").Width, 0) _
            + IIf(gridView.Columns("APPRGRSWT").Visible, gridView.Columns("APPRGRSWT").Width, 0) _
            + IIf(gridView.Columns("APPRNETWT").Visible, gridView.Columns("APPRNETWT").Width, 0) _
            + IIf(gridView.Columns("RSALVALUE").Visible, gridView.Columns("RSALVALUE").Width, 0) _
            + IIf(gridView.Columns("RPURVALUE").Visible, gridView.Columns("RPURVALUE").Width, 0)
            .Columns("TRPCS~TRGRSWT~TRNETWT~TRDIAPCS~TRDIAWT~TRSTNPCS~TRSTNWT~TRTAGPCS~RPCS~RGRSWT~RNETWT~RDIAPCS~RDIAWT~RSTNPCS~RSTNWT~RTAGPCS~RSALVALUE~RPURVALUE" & strRapproval).HeaderText = "RECEIPT"

            .Columns("TIPCS~TIGRSWT~TINETWT~TIDIAPCS~TIDIAWT~TISTNPCS~TISTNWT~TITAGPCS~IPCS~IGRSWT~INETWT~IDIAPCS~IDIAWT~ISTNPCS~ISTNWT~ITAGPCS~MIPCS~MIGRSWT~MINETWT~MITAGPCS~APCS~AGRSWT~ANETWT~ATAGPCS~ISALVALUE~IPURVALUE" & strIapproval & strSepPartlySale).Visible = gridView.Columns("IPCS").Visible Or chkOnlyApproval.Checked = True
            .Columns("TIPCS~TIGRSWT~TINETWT~TIDIAPCS~TIDIAWT~TISTNPCS~TISTNWT~TITAGPCS~IPCS~IGRSWT~INETWT~IDIAPCS~IDIAWT~ISTNPCS~ISTNWT~ITAGPCS~MIPCS~MIGRSWT~MINETWT~MITAGPCS~APCS~AGRSWT~ANETWT~ATAGPCS~ISALVALUE~IPURVALUE" & strIapproval & strSepPartlySale).Width =
            IIf(gridView.Columns("TIPCS").Visible, gridView.Columns("TIPCS").Width, 0) _
            + IIf(gridView.Columns("IPCS").Visible, gridView.Columns("IPCS").Width, 0) _
            + IIf(gridView.Columns("MIPCS").Visible, gridView.Columns("MIPCS").Width, 0) _
            + IIf(gridView.Columns("TIGRSWT").Visible, gridView.Columns("TIGRSWT").Width, 0) _
            + IIf(gridView.Columns("IGRSWT").Visible, gridView.Columns("IGRSWT").Width, 0) _
            + IIf(gridView.Columns("MIGRSWT").Visible, gridView.Columns("MIGRSWT").Width, 0) _
            + IIf(gridView.Columns("TINETWT").Visible, gridView.Columns("TINETWT").Width, 0) _
            + IIf(gridView.Columns("INETWT").Visible, gridView.Columns("INETWT").Width, 0) _
            + IIf(gridView.Columns("MINETWT").Visible, gridView.Columns("MINETWT").Width, 0) _
            + IIf(gridView.Columns("TIDIAPCS").Visible, gridView.Columns("TIDIAPCS").Width, 0) _
            + IIf(gridView.Columns("TIDIAWT").Visible, gridView.Columns("TIDIAWT").Width, 0) _
            + IIf(gridView.Columns("TISTNPCS").Visible, gridView.Columns("TISTNPCS").Width, 0) _
            + IIf(gridView.Columns("TISTNWT").Visible, gridView.Columns("TISTNWT").Width, 0) _
            + IIf(gridView.Columns("IDIAPCS").Visible, gridView.Columns("IDIAPCS").Width, 0) _
            + IIf(gridView.Columns("IDIAWT").Visible, gridView.Columns("IDIAWT").Width, 0) _
            + IIf(gridView.Columns("ISTNPCS").Visible, gridView.Columns("ISTNPCS").Width, 0) _
            + IIf(gridView.Columns("ISTNWT").Visible, gridView.Columns("ISTNWT").Width, 0) _
            + IIf(gridView.Columns("APPIPCS").Visible, gridView.Columns("APPIPCS").Width, 0) _
            + IIf(gridView.Columns("APPIGRSWT").Visible, gridView.Columns("APPIGRSWT").Width, 0) _
            + IIf(gridView.Columns("APPINETWT").Visible, gridView.Columns("APPINETWT").Width, 0) _
            + IIf(gridView.Columns("ISALVALUE").Visible, gridView.Columns("ISALVALUE").Width, 0) _
            + psColWidth _
            + IIf(gridView.Columns("IPURVALUE").Visible, gridView.Columns("IPURVALUE").Width, 0)
            .Columns("TIPCS~TIGRSWT~TINETWT~TIDIAPCS~TIDIAWT~TISTNPCS~TISTNWT~TITAGPCS~IPCS~IGRSWT~INETWT~IDIAPCS~IDIAWT~ISTNPCS~ISTNWT~ITAGPCS~MIPCS~MIGRSWT~MINETWT~MITAGPCS~APCS~AGRSWT~ANETWT~ATAGPCS~ISALVALUE~IPURVALUE" & strIapproval & strSepPartlySale).HeaderText = "ISSUE"

            .Columns("CPCS~CGRSWT~CNETWT~CDIAPCS~CDIAWT~CSTNPCS~CSTNWT~CTAGPCS~CSALVALUE~CPURVALUE" & strCapproval).Visible = gridView.Columns("CPCS").Visible Or chkOnlyApproval.Checked = True
            .Columns("CPCS~CGRSWT~CNETWT~CDIAPCS~CDIAWT~CSTNPCS~CSTNWT~CTAGPCS~CSALVALUE~CPURVALUE" & strCapproval).Width = IIf(gridView.Columns("CPCS").Visible, gridView.Columns("CPCS").Width, 0)
            If chkGrsWt.Checked Then .Columns("CPCS~CGRSWT~CNETWT~CDIAPCS~CDIAWT~CSTNPCS~CSTNWT~CTAGPCS~CSALVALUE~CPURVALUE" & strCapproval).Width += IIf(gridView.Columns("CGRSWT").Visible, gridView.Columns("CGRSWT").Width, 0)
            If chkNetWt.Checked Then .Columns("CPCS~CGRSWT~CNETWT~CDIAPCS~CDIAWT~CSTNPCS~CSTNWT~CTAGPCS~CSALVALUE~CPURVALUE" & strCapproval).Width += IIf(gridView.Columns("CNETWT").Visible, gridView.Columns("CNETWT").Width, 0)
            If ChkSalvalue.Checked Then .Columns("CPCS~CGRSWT~CNETWT~CDIAPCS~CDIAWT~CSTNPCS~CSTNWT~CTAGPCS~CSALVALUE~CPURVALUE" & strCapproval).Width += IIf(gridView.Columns("CSALVALUE").Visible, gridView.Columns("CSALVALUE").Width, 0)
            If ChkPurchase.Checked Then .Columns("CPCS~CGRSWT~CNETWT~CDIAPCS~CDIAWT~CSTNPCS~CSTNWT~CTAGPCS~CSALVALUE~CPURVALUE" & strCapproval).Width += IIf(gridView.Columns("CPURVALUE").Visible, gridView.Columns("CPURVALUE").Width, 0)

            If ChkWithDia.Checked And rbtDiaStnByColumn.Checked Then .Columns("CPCS~CGRSWT~CNETWT~CDIAPCS~CDIAWT~CSTNPCS~CSTNWT~CTAGPCS~CSALVALUE~CPURVALUE" & strCapproval).Width += IIf(gridView.Columns("CDIAPCS").Visible, gridView.Columns("CDIAPCS").Width, 0)
            If ChkWithDia.Checked And rbtDiaStnByColumn.Checked Then .Columns("CPCS~CGRSWT~CNETWT~CDIAPCS~CDIAWT~CSTNPCS~CSTNWT~CTAGPCS~CSALVALUE~CPURVALUE" & strCapproval).Width += IIf(gridView.Columns("CDIAWT").Visible, gridView.Columns("CDIAWT").Width, 0)
            If ChkWithStone.Checked And rbtDiaStnByColumn.Checked Then .Columns("CPCS~CGRSWT~CNETWT~CDIAPCS~CDIAWT~CSTNPCS~CSTNWT~CTAGPCS~CSALVALUE~CPURVALUE" & strCapproval).Width += IIf(gridView.Columns("CSTNPCS").Visible, gridView.Columns("CSTNPCS").Width, 0)
            If ChkWithStone.Checked And rbtDiaStnByColumn.Checked Then .Columns("CPCS~CGRSWT~CNETWT~CDIAPCS~CDIAWT~CSTNPCS~CSTNWT~CTAGPCS~CSALVALUE~CPURVALUE" & strCapproval).Width += IIf(gridView.Columns("CSTNWT").Visible, gridView.Columns("CSTNWT").Width, 0)


            If chkSepApprovalColumn.Checked = True Or chkOnlyApproval.Checked = True Then .Columns("CPCS~CGRSWT~CNETWT~CDIAPCS~CDIAWT~CSTNPCS~CSTNWT~CTAGPCS~CSALVALUE~CPURVALUE" & strCapproval).Width += IIf(gridView.Columns("APPCPCS").Visible, gridView.Columns("APPCPCS").Width, 0)
            If chkSepApprovalColumn.Checked = True Or chkOnlyApproval.Checked = True And chkGrsWt.Checked Then .Columns("CPCS~CGRSWT~CNETWT~CDIAPCS~CDIAWT~CSTNPCS~CSTNWT~CTAGPCS~CSALVALUE~CPURVALUE" & strCapproval).Width += IIf(gridView.Columns("APPCGRSWT").Visible, gridView.Columns("APPCGRSWT").Width, 0)
            If chkSepApprovalColumn.Checked = True Or chkOnlyApproval.Checked = True And chkNetWt.Checked Then .Columns("CPCS~CGRSWT~CNETWT~CDIAPCS~CDIAWT~CSTNPCS~CSTNWT~CTAGPCS~CSALVALUE~CPURVALUE" & strCapproval).Width += IIf(gridView.Columns("APPCNETWT").Visible, gridView.Columns("APPCNETWT").Width, 0)

            'If chkOnlyTag.Checked Then .Columns("CPCS~CGRSWT~CNETWT~CTAGPCS").Width = gridView.Columns("CTAGPCS").Width
            .Columns("CPCS~CGRSWT~CNETWT~CDIAPCS~CDIAWT~CSTNPCS~CSTNWT~CTAGPCS~CSALVALUE~CPURVALUE" & strCapproval).HeaderText = "CLOSING"

            .Columns("RATE").Visible = gridView.Columns("RATE").Visible Or ChkRate.Checked = True
            .Columns("RATE").Width = IIf(gridView.Columns("RATE").Visible, gridView.Columns("RATE").Width, 0)
            .Columns("RATE").HeaderText = "RATE"

            .Columns("NOOFTAGS").Visible = gridView.Columns("NOOFTAGS").Visible Or ChkWithNoofTags.Checked = True
            .Columns("NOOFTAGS").Width = IIf(gridView.Columns("NOOFTAGS").Visible, gridView.Columns("NOOFTAGS").Width, 0)
            .Columns("NOOFTAGS").HeaderText = "NOOFTAGS"

            .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
            .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
        End With
    End Function

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            Me.btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        funcExit()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub

    Private Sub chkLstCounter_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstCounter.GotFocus
        If chkLstCounter.Items.Count > 0 Then
            chkLstCounter.SelectedIndex = 0
        End If
    End Sub

    Private Sub chkLstCounter_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstCounter.LostFocus
        chkLstCounter.ClearSelected()
    End Sub

    Private Sub chkLstItemType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstItemType.GotFocus
        If chkLstItemType.Items.Count > 0 Then
            chkLstItemType.SelectedIndex = 0
        End If
    End Sub

    Private Sub chkLstItemType_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstItemType.LostFocus
        chkLstItemType.ClearSelected()
    End Sub

    Private Sub chkLstCostCentre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstCostCentre.GotFocus
        If chkLstCostCentre.Items.Count > 0 Then
            chkLstCostCentre.SelectedIndex = 0
        End If
    End Sub

    Private Sub chkLstCostCentre_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstCostCentre.LostFocus
        chkLstCostCentre.ClearSelected()
    End Sub

    Private Sub gridView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView.Scroll
        If gridViewHead Is Nothing Then Exit Sub
        If Not gridViewHead.Columns.Count > 0 Then Exit Sub
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridViewHead.HorizontalScrollingOffset = e.NewValue
        End If
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridViewHead.HorizontalScrollingOffset = e.NewValue
                gridViewHead.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                gridViewHead.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub

    Private Sub cmbMetal_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMetal.SelectedIndexChanged
        funcLoadCategory()
        funcLoadItemName()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcLoadMetal()
        funcLoadCategory()
        funcLoadItemName()
        funcLoadDesigner()
        funcLoadCounter()
        funcLoadItemType()
        funcLoadsubitemgroup()
        chkLstCostCentre.Items.Clear()
        If chkLstCostCentre.Enabled = True Then
            funcLoadCostCentre()
        End If
        Prop_Gets()
        If chkLstCompany.Items.Count = 1 Then
            chkCompanySelectAll.Checked = False
            chkCompanySelectAll.Enabled = False
            chkLstCompany.SetItemChecked(0, True)
        End If
        If CheckOnly Then
            chkSeperateTransferCol.Checked = False
            chkSeperateTransferCol.Visible = False
            rbtWithBoth.Visible = False
            rbtWithClosing.Visible = False
            rbtWithOpening.Visible = False
        End If

        'rbtDetailed.Checked = True
        'chkAllCostCentre.Checked = False
        'chkAllItemType.Checked = False
        'chkAllCounter.Checked = False
        'chkSeperateTransferCol.Checked = True
        'chkTransactionOnly.Checked = False

        'gridView.DataSource = Nothing
        If RPT_CHKDIS_ROLEEDIT = True Then If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit, False) Then Panel1.Enabled = False
        dtpAsOnDate.Value = GetServerDate()
        cmbMetal.Select()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name & IIf(Me.CheckOnly, "CHECK", ""), BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, gridViewHead)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name & IIf(Me.CheckOnly, "CHECK", ""), BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        Dim IS40COLCLSSTKPRINT As Boolean = IIf(GetAdmindbSoftValue("40COLCLSSTKPRINT", "N") = "Y", True, False)
        If IS40COLCLSSTKPRINT Then
            If MsgBox("Do you want to print on 40 Col. Print ?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then IS40COLCLSSTKPRINT = False
        End If
        If IS40COLCLSSTKPRINT Then
            Call PRINT40COLSUMMARY()
        Else
            If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
                BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, gridViewHead)
            End If
        End If
    End Sub
    Private Sub PRINT40COLSUMMARY()
        If IO.File.Exists(Application.StartupPath & "\SUMMARYPRINT.TXT") Then
            IO.File.Delete(Application.StartupPath & "\SUMMARYPRINT.TXT")
        End If
        Dim write As StreamWriter
        write = IO.File.CreateText(Application.StartupPath & "\SUMMARYPRINT.TXT")
        Dim lineprn As String = Space(40)
        write.WriteLine("--------------------------------------")
        write.WriteLine("COUNTERWISE STOCK ASON " + Mid(dtpAsOnDate.Text, 1, 6) + Mid(dtpAsOnDate.Text, 9, 10) + " " + Format(Now, "hh:mmtt"))
        write.WriteLine("--------------------------------------")
        If CheckOnly Then
            write.WriteLine("DESCRIPTION".PadRight(17, "") & Space(1) & "PCS".PadRight(5, "") & Space(1) & IIf(chkGrsWt.Checked, "GRS WT", "NET WT").PadRight(8, "") & Space(1) & "Status".PadRight(7, ""))
        Else
            write.WriteLine("DESCRIPTION".PadRight(17, "") & Space(1) & "PCS".PadRight(5, "") & Space(1) & IIf(chkGrsWt.Checked, "GRS WT", "NET WT").PadRight(8, "") & Space(1) & "Rate".PadRight(7, ""))
        End If
        write.WriteLine("--------------------------------------")
        'gridView.DataSource

        For i As Integer = 0 To gridView.Rows.Count - 1
            With gridView.Rows(i)
                Dim prndesc As String = Mid(.Cells("PARTICULAR").Value.ToString, 1, 17)
                Dim prnpcs As String = ""
                Dim prngwt As String = ""
                If gridView.Columns.Contains("CHPcs") Then
                    prnpcs = .Cells("CHPcs").Value.ToString()
                    prngwt = IIf(chkGrsWt.Checked, .Cells("CHGRSWT").Value.ToString, .Cells("CHNETWT").Value.ToString)
                Else
                    prnpcs = .Cells("CPcs").Value.ToString()
                    prngwt = IIf(chkGrsWt.Checked, .Cells("CGRSWT").Value.ToString, .Cells("CNETWT").Value.ToString)
                End If
                Dim prnrate As String
                If CheckOnly Then
                    If Not IsDBNull(.Cells("Status").Value.ToString) Then prnrate = IIf(.Cells("STATUS").Value.ToString <> "", .Cells("STATUS").Value.ToString, "")
                Else
                    If Not IsDBNull(.Cells("Rate").Value.ToString) Then prnrate = IIf(Val(.Cells("Rate").Value.ToString) <> 0, Format(Val(.Cells("Rate").Value.ToString), "0.00"), "")
                End If
                prndesc = LSet(prndesc, 14)
                prnpcs = RSet(prnpcs, 5)
                prngwt = RSet(prngwt, 9)
                prnrate = RSet(prnrate, 9)
                If prndesc.Contains("TOTAL") Then write.WriteLine("--------------------------------------")
                write.WriteLine(prndesc & Space(1) & prnpcs & Space(1) & prngwt & Space(1) & prnrate)
                'write.WriteLine(prndesc.PadRight(17, "") & Space(1) & prnpcs.PadLeft(5, "") & Space(1) & prngwt.PadLeft(8, "") & Space(1) & prnrate.PadLeft(7, ""))
                If prndesc.Contains("TOTAL") Then write.WriteLine("--------------------------------------")
            End With
        Next
        For i As Integer = 0 To 4
            write.WriteLine()
        Next
        write.Close()
        If IO.File.Exists(Application.StartupPath & "\SUMMARYPRINT.BAT") Then
            IO.File.Delete(Application.StartupPath & "\SUMMARYPRINT.BAT")
        End If
        Dim writebat As StreamWriter

        Dim PrnName As String = ""
        Dim CondId As String = ""
        Try
            CondId = "'AGOLDREPORT40COLUMN" & Environ("NODE-ID").ToString & "'"
        Catch ex As Exception
            MsgBox("Set Node-Id", MsgBoxStyle.Information)
            Exit Sub
        End Try
        writebat = IO.File.CreateText(Application.StartupPath & "\SUMMARYPRINT.BAT")
        strSql = "SELECT * FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID=" & CondId & ""
        Dim dt As New DataTable
        dt = GetSqlTable(strSql, cn)
        If dt.Rows.Count <> 0 Then
            PrnName = dt.Rows(0).Item("CTLTEXT").ToString
        Else
            PrnName = "PRN"
        End If
        writebat.WriteLine("TYPE " & Application.StartupPath & "\SUMMARYPRINT.TXT>" & PrnName)
        writebat.Flush()
        writebat.Close()
        Shell(Application.StartupPath & "\SUMMARYPRINT.BAT")
    End Sub
    Private Sub frmItemWiseStock_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        pnlGroupFilter.Location = New Point((ScreenWid - pnlGroupFilter.Width) / 2, ((ScreenHit - 128) - pnlGroupFilter.Height) / 2)
        LoadCompany(chkLstCompany)
        transitCheck()
        ChkMultiCounter.Checked = True
        Authorize = BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Authorize, False)
        If Authorize = False And userId <> 999 Then
            Panel1.Enabled = False
            btnSave_OWN.Enabled = False
        End If
        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Function transitCheck()
        If objGPack.GetSqlValue(" SELECT COUNT(*) FROM " & cnAdminDb & "..TITEMTAG", , 0, ) > 0 Or objGPack.GetSqlValue(" SELECT COUNT(*) FROM " & cnAdminDb & "..TITEMNONTAG", , 0, ) > 0 Then
            MsgBox("Stock available in In-Transit " & vbCrLf & vbCrLf & "Please download it", MsgBoxStyle.Information)
        End If
    End Function
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGen
        cmbMetal.Focus()
    End Sub

    Private Sub ChkWithApproval_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If ChkWithApproval.Checked = True Then
            'chkSepApprovalColumn.Enabled = True
            chkOnlyApproval.Checked = False
        ElseIf ChkWithApproval.Checked = False Then
            'chkSepApprovalColumn.Checked = False
            'chkSepApprovalColumn.Enabled = False
        End If
    End Sub

    Private Sub chkSepApprovalColumn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If chkSepApprovalColumn.Checked = True Then
            chkOnlyApproval.Checked = False
            'ChkWithApproval.Checked = True
        End If
    End Sub

    Private Sub chkOnlyApproval_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If chkOnlyApproval.Checked = True Then
            chkSepApprovalColumn.Checked = False
            ChkWithApproval.Checked = False
        End If
    End Sub

    Private Sub chkOnlyTag_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkOnlyTag.Click
        If chkOnlyTag.Checked = True Then
            chkNetWt.Checked = False
            chkGrsWt.Checked = False
            chkGrsWt.Enabled = False
            chkNetWt.Enabled = False
        Else
            chkGrsWt.Enabled = True
            chkNetWt.Enabled = True
        End If
    End Sub

    Private Sub chkAllCostCentre_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAllCostCentre.CheckedChanged
        SetChecked_CheckedList(chkLstCostCentre, chkAllCostCentre.Checked)
    End Sub

    Private Sub chkAllCounter_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAllCounter.CheckedChanged
        SetChecked_CheckedList(chkLstCounter, chkAllCounter.Checked)
    End Sub

    Private Sub chkAllItemType_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAllItemType.CheckedChanged
        SetChecked_CheckedList(chkLstItemType, chkAllItemType.Checked)
    End Sub
    Private Sub chkCompanySelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCompanySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
    End Sub


    Private Sub gridView_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.SelectionChanged
        ' if selection is changed after Cell Editing
        If CheckOnly Then
            If celWasEndEdit IsNot Nothing AndAlso
               gridView.CurrentCell IsNot Nothing Then
                ' if we are currently in the next line of last edit cell
                If (gridView.CurrentCell.RowIndex = celWasEndEdit.RowIndex + 1 AndAlso
                   gridView.CurrentCell.ColumnIndex = celWasEndEdit.ColumnIndex) Then
                    Dim iColNew As Integer
                    Dim iRowNew As Integer
                    ' if we at the last column
                    If celWasEndEdit.ColumnIndex >= gridView.ColumnCount - 1 Then
                        iColNew = 0                         ' move to first column
                        iRowNew = gridView.CurrentCell.RowIndex   ' and move to next row
                    Else ' else it means we are NOT at the last column
                        ' move to next column
                        iColNew = celWasEndEdit.ColumnIndex + 1
                        ' but row should remain same
                        iRowNew = celWasEndEdit.RowIndex
                    End If
                    celWasEndEdit = gridView(iColNew, iRowNew)   ' ok set the current column
                    If Not celWasEndEdit.Visible Or celWasEndEdit.ReadOnly Then
                        For rIndex As Integer = iRowNew To gridView.RowCount - 1
                            'If gridView.Rows(rIndex).Cells("COLHEAD").Value.ToString <> "" Then Continue For
                            For cIndex As Integer = iColNew To gridView.Columns.Count - 1
                                celWasEndEdit = gridView.Rows(rIndex).Cells(cIndex)
                                If celWasEndEdit.Visible And Not celWasEndEdit.ReadOnly Then Exit For
                            Next
                            If celWasEndEdit.Visible And Not celWasEndEdit.ReadOnly Then Exit For
                            iColNew = 0
                        Next
                    End If
                    gridView.CurrentCell = celWasEndEdit
                End If
            End If
            celWasEndEdit = Nothing                      ' reset the cell end edit
        End If

    End Sub
    Private Sub ResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResizeToolStripMenuItem.Click
        If gridView.RowCount > 0 Then
            If ResizeToolStripMenuItem.Checked Then
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridView.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
            'gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End If
    End Sub

    Private Sub GroupBox1_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupBox1.Enter

    End Sub

    Private Sub rbtSummary_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtSummary.CheckedChanged
        If rbtSummary.Checked = True Then
            ChkSubtotal.Enabled = True
            ChkSubtotal.Checked = True
        Else
            ChkSubtotal.Checked = False
            ChkSubtotal.Enabled = False
        End If
    End Sub

    Private Sub ChkWithNoofTags_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If ChkWithNoofTags.Checked = True Then
            If rbtTag.Checked = False Then
                MsgBox("Tag only select", MsgBoxStyle.Information)
                rbtTag.Checked = True
            End If
        End If
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New CounterWiseStock_Properties
        obj.p_cmbMetal = cmbMetal.Text
        obj.p_cmbCategory = cmbCategory.Text
        obj.p_cmbItemName = cmbItemName.Text
        obj.p_cmbDesigner = cmbDesigner.Text
        obj.p_chkCompanySelectAll = chkCompanySelectAll.Checked
        GetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany)
        obj.p_chkAllCounter = chkAllCounter.Checked
        GetChecked_CheckedList(chkLstCounter, obj.p_chkLstCounter)
        obj.p_chkAllItemType = chkAllItemType.Checked
        GetChecked_CheckedList(chkLstItemType, obj.p_chkLstItemType)
        'obj.p_chkAllCostCentre = chkAllCostCentre.Checked
        'GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        obj.p_rbtDetailed = rbtDetailed.Checked
        obj.p_rbtSummary = rbtSummary.Checked
        obj.p_rbtBoth = rbtBoth.Checked
        obj.p_rbtTag = rbtTag.Checked
        obj.p_rbtNonTag = rbtNonTag.Checked
        obj.p_rbtWithBoth = rbtWithBoth.Checked
        obj.p_rbtWithOpening = rbtWithOpening.Checked
        obj.p_rbtWithClosing = rbtWithClosing.Checked
        obj.p_chkGrsWt = chkGrsWt.Checked
        obj.p_chkNetWt = chkNetWt.Checked
        obj.p_chkWithSubItem = chkWithSubItem.Checked
        obj.p_chkSeperateTransferCol = chkSeperateTransferCol.Checked
        obj.p_chkWithApproval = chkSepApprovalColumn.Checked
        obj.p_chkOnlyApproval = chkOnlyApproval.Checked
        obj.p_chkTransactionOnly = chkTransactionOnly.Checked
        obj.p_chkOrderbyId = chkOrderbyId.Checked
        obj.p_chkSalvalue = ChkSalvalue.Checked
        obj.p_chkPurchasevalue = ChkPurchase.Checked
        obj.p_chkWithDia = ChkWithDia.Checked
        obj.p_chkWithStone = ChkWithStone.Checked
        obj.p_rbtDiaStnByRow = rbtDiaStnByRow.Checked
        obj.p_rbtDiaStnByColumn = rbtDiaStnByColumn.Checked
        obj.p_chkMultiCounter = ChkMultiCounter.Checked
        obj.p_chkPendTr = chkPendTr.Checked
        obj.p_chkCostCentreWise = rbtCostcentre.Checked
        obj.p_ChkPartSale = ChkPartSale.Checked
        SetSettingsObj(obj, Me.Name & IIf(CheckOnly, "_CHECK", ""), GetType(CounterWiseStock_Properties), Save)
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New CounterWiseStock_Properties
        GetSettingsObj(obj, Me.Name & IIf(CheckOnly, "_CHECK", ""), GetType(CounterWiseStock_Properties), IIf(Authorize = False, True, False))
        cmbMetal.Text = obj.p_cmbMetal
        cmbCategory.Text = obj.p_cmbCategory
        cmbItemName.Text = obj.p_cmbItemName
        cmbDesigner.Text = obj.p_cmbDesigner
        chkCompanySelectAll.Checked = obj.p_chkCompanySelectAll
        SetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany, strCompanyName)
        chkAllCounter.Checked = obj.p_chkAllCounter
        SetChecked_CheckedList(chkLstCounter, obj.p_chkLstCounter, Nothing)
        chkAllItemType.Checked = obj.p_chkAllItemType
        SetChecked_CheckedList(chkLstItemType, obj.p_chkLstItemType, Nothing)
        'chkAllCostCentre.Checked = obj.p_chkAllCostCentre
        'SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, cnCostName)
        rbtDetailed.Checked = obj.p_rbtDetailed
        rbtSummary.Checked = obj.p_rbtSummary
        rbtBoth.Checked = obj.p_rbtBoth
        rbtTag.Checked = obj.p_rbtTag
        rbtNonTag.Checked = obj.p_rbtNonTag
        rbtWithBoth.Checked = obj.p_rbtWithBoth
        rbtWithOpening.Checked = obj.p_rbtWithOpening
        rbtWithClosing.Checked = obj.p_rbtWithClosing
        chkGrsWt.Checked = obj.p_chkGrsWt
        chkNetWt.Checked = obj.p_chkNetWt
        chkWithSubItem.Checked = obj.p_chkWithSubItem
        chkSeperateTransferCol.Checked = obj.p_chkSeperateTransferCol
        chkSepApprovalColumn.Checked = obj.p_chkWithApproval
        chkOnlyApproval.Checked = obj.p_chkOnlyApproval
        chkTransactionOnly.Checked = obj.p_chkTransactionOnly
        chkOrderbyId.Checked = obj.p_chkOrderbyId
        ChkSalvalue.Checked = obj.p_chkSalvalue
        ChkPurchase.Checked = obj.p_chkPurchasevalue
        ChkWithDia.Checked = obj.p_chkWithDia
        ChkWithStone.Checked = obj.p_chkWithStone
        rbtDiaStnByRow.Checked = obj.p_rbtDiaStnByRow
        rbtDiaStnByColumn.Checked = obj.p_rbtDiaStnByColumn
        ChkMultiCounter.Checked = obj.p_chkMultiCounter
        chkPendTr.Checked = obj.p_chkPendTr
        rbtCostcentre.Checked = obj.p_chkCostCentreWise
        ChkPartSale.Checked = obj.p_chkpartsale
    End Sub

    Private Sub ChkWithDia_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkWithDia.CheckedChanged
        If ChkWithDia.Checked Or ChkWithStone.Checked Then
            pnlDisStnResult.Enabled = True
        Else
            pnlDisStnResult.Enabled = False
        End If
    End Sub

    Private Sub ChkWithStone_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkWithStone.CheckedChanged
        If ChkWithDia.Checked Or ChkWithStone.Checked Then
            pnlDisStnResult.Enabled = True
        Else
            pnlDisStnResult.Enabled = False
        End If
    End Sub

    Private Sub ChkMultiCounter_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkMultiCounter.CheckStateChanged
        If ChkMultiCounter.Checked Then
            chkLstCounter.Visible = True
            cmbCounter.Visible = False
            chkAllCounter.Enabled = True
        Else
            chkLstCounter.Visible = False
            cmbCounter.Visible = True
            chkAllCounter.Enabled = False
        End If
    End Sub

    Private Sub ChkMultiCounter_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkMultiCounter.CheckedChanged

    End Sub

    Private Sub chkMultiCat_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkMultiCat.CheckStateChanged
        chkCmbCategory.Visible = chkMultiCat.Checked
        cmbCategory.Visible = Not chkMultiCat.Checked

    End Sub

    Private Sub chkAllCategory_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'SetChecked_CheckedList(chkCmbCategory, chkAllCategory.Checked)
    End Sub

    Private Sub chkMultiCat_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMultiCat.CheckedChanged

    End Sub

    Private Sub btnSave_OWN_Click(sender As Object, e As EventArgs) Handles btnSave_OWN.Click
        Save = True
        Prop_Sets()
        Save = False
    End Sub

    Private Sub cmbSubItemGroup_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSubItemGroup.SelectedIndexChanged

    End Sub

    Private Sub Label9_Click(sender As Object, e As EventArgs) Handles Label9.Click

    End Sub
End Class


Public Class CounterWiseStock_Properties
    Private chkCompanySelectAll As Boolean = False
    Public Property p_chkCompanySelectAll() As Boolean
        Get
            Return chkCompanySelectAll
        End Get
        Set(ByVal value As Boolean)
            chkCompanySelectAll = value
        End Set
    End Property

    Private chkAllCounter As Boolean = False
    Public Property p_chkAllCounter() As Boolean
        Get
            Return chkAllCounter
        End Get
        Set(ByVal value As Boolean)
            chkAllCounter = value
        End Set
    End Property

    Private chkAllItemType As Boolean = False
    Public Property p_chkAllItemType() As Boolean
        Get
            Return chkAllItemType
        End Get
        Set(ByVal value As Boolean)
            chkAllItemType = value
        End Set
    End Property

    Private chkAllCostCentre As Boolean = False
    Public Property p_chkAllCostCentre() As Boolean
        Get
            Return chkAllCostCentre
        End Get
        Set(ByVal value As Boolean)
            chkAllCostCentre = value
        End Set
    End Property

    Private chkLstCompany As New List(Of String)
    Public Property p_chkLstCompany() As List(Of String)
        Get
            If Not chkLstCompany.Count > 0 Then
                chkLstCompany.Add(strCompanyName)
            End If
            Return chkLstCompany
        End Get
        Set(ByVal value As List(Of String))
            chkLstCompany = value
        End Set
    End Property

    Private chkLstCounter As New List(Of String)
    Public Property p_chkLstCounter() As List(Of String)
        Get
            Return chkLstCounter
        End Get
        Set(ByVal value As List(Of String))
            chkLstCounter = value
        End Set
    End Property

    Private chkLstItemType As New List(Of String)
    Public Property p_chkLstItemType() As List(Of String)
        Get
            Return chkLstItemType
        End Get
        Set(ByVal value As List(Of String))
            chkLstItemType = value
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

    Private cmbMetal As String = "ALL"
    Public Property p_cmbMetal() As String
        Get
            Return cmbMetal
        End Get
        Set(ByVal value As String)
            cmbMetal = value
        End Set
    End Property

    Private cmbCategory As String = "ALL"
    Public Property p_cmbCategory() As String
        Get
            Return cmbCategory
        End Get
        Set(ByVal value As String)
            cmbCategory = value
        End Set
    End Property

    Private cmbItemName As String = "ALL"
    Public Property p_cmbItemName() As String
        Get
            Return cmbItemName
        End Get
        Set(ByVal value As String)
            cmbItemName = value
        End Set
    End Property

    Private cmbDesigner As String = "ALL"
    Public Property p_cmbDesigner() As String
        Get
            Return cmbDesigner
        End Get
        Set(ByVal value As String)
            cmbDesigner = value
        End Set
    End Property

    Private chkGrsWt As Boolean = True
    Public Property p_chkGrsWt() As Boolean
        Get
            Return chkGrsWt
        End Get
        Set(ByVal value As Boolean)
            chkGrsWt = value
        End Set
    End Property

    Private chkNetWt As Boolean = False
    Public Property p_chkNetWt() As Boolean
        Get
            Return chkNetWt
        End Get
        Set(ByVal value As Boolean)
            chkNetWt = value
        End Set
    End Property

    Private chkWithSubItem As Boolean = False
    Public Property p_chkWithSubItem() As Boolean
        Get
            Return chkWithSubItem
        End Get
        Set(ByVal value As Boolean)
            chkWithSubItem = value
        End Set
    End Property

    Private chkSeperateTransferCol As Boolean = True
    Public Property p_chkSeperateTransferCol() As Boolean
        Get
            Return chkSeperateTransferCol
        End Get
        Set(ByVal value As Boolean)
            chkSeperateTransferCol = value
        End Set
    End Property

    Private chkWithApproval As Boolean = True
    Public Property p_chkWithApproval() As Boolean
        Get
            Return chkWithApproval
        End Get
        Set(ByVal value As Boolean)
            chkWithApproval = value
        End Set
    End Property

    Private chkOnlyApproval As Boolean = False
    Public Property p_chkOnlyApproval() As Boolean
        Get
            Return chkOnlyApproval
        End Get
        Set(ByVal value As Boolean)
            chkOnlyApproval = value
        End Set
    End Property

    Private chkTransactionOnly As Boolean = False
    Public Property p_chkTransactionOnly() As Boolean
        Get
            Return chkTransactionOnly
        End Get
        Set(ByVal value As Boolean)
            chkTransactionOnly = value
        End Set
    End Property

    Private rbtDetailed As Boolean = True
    Public Property p_rbtDetailed() As Boolean
        Get
            Return rbtDetailed
        End Get
        Set(ByVal value As Boolean)
            rbtDetailed = value
        End Set
    End Property

    Private rbtSummary As Boolean = False
    Public Property p_rbtSummary() As Boolean
        Get
            Return rbtSummary
        End Get
        Set(ByVal value As Boolean)
            rbtSummary = value
        End Set
    End Property
    Private rbtBoth As Boolean = True
    Public Property p_rbtBoth() As Boolean
        Get
            Return rbtBoth
        End Get
        Set(ByVal value As Boolean)
            rbtBoth = value
        End Set
    End Property
    Private rbtTag As Boolean = False
    Public Property p_rbtTag() As Boolean
        Get
            Return rbtTag
        End Get
        Set(ByVal value As Boolean)
            rbtTag = value
        End Set
    End Property
    Private rbtNonTag As Boolean = False
    Public Property p_rbtNonTag() As Boolean
        Get
            Return rbtNonTag
        End Get
        Set(ByVal value As Boolean)
            rbtNonTag = value
        End Set
    End Property

    Private rbtWithBoth As Boolean = True
    Public Property p_rbtWithBoth() As Boolean
        Get
            Return rbtWithBoth
        End Get
        Set(ByVal value As Boolean)
            rbtWithBoth = value
        End Set
    End Property
    Private rbtWithOpening As Boolean = False
    Public Property p_rbtWithOpening() As Boolean
        Get
            Return rbtWithOpening
        End Get
        Set(ByVal value As Boolean)
            rbtWithOpening = value
        End Set
    End Property
    Private rbtWithClosing As Boolean = False
    Public Property p_rbtWithClosing() As Boolean
        Get
            Return rbtWithClosing
        End Get
        Set(ByVal value As Boolean)
            rbtWithClosing = value
        End Set
    End Property
    Private chkOrderbyId As Boolean = False
    Public Property p_chkOrderbyId() As Boolean
        Get
            Return chkOrderbyId
        End Get
        Set(ByVal value As Boolean)
            chkOrderbyId = value
        End Set
    End Property
    Private chkSalvalue As Boolean = False
    Public Property p_chkSalvalue() As Boolean
        Get
            Return chkSalvalue
        End Get
        Set(ByVal value As Boolean)
            chkSalvalue = value
        End Set
    End Property
    Private chkPurchasevalue As Boolean = False
    Public Property p_chkPurchasevalue() As Boolean
        Get
            Return chkPurchasevalue
        End Get
        Set(ByVal value As Boolean)
            chkPurchasevalue = value
        End Set
    End Property

    Private chkWithDia As Boolean = False
    Public Property p_chkWithDia() As Boolean
        Get
            Return chkWithDia
        End Get
        Set(ByVal value As Boolean)
            chkWithDia = value
        End Set
    End Property

    Private chkWithStone As Boolean = False
    Public Property p_chkWithStone() As Boolean
        Get
            Return chkWithStone
        End Get
        Set(ByVal value As Boolean)
            chkWithStone = value
        End Set
    End Property
    Private rbtDiaStnByRow As Boolean = True
    Public Property p_rbtDiaStnByRow() As Boolean
        Get
            Return rbtDiaStnByRow
        End Get
        Set(ByVal value As Boolean)
            rbtDiaStnByRow = value
        End Set
    End Property

    Private rbtDiaStnByColumn As Boolean = False
    Public Property p_rbtDiaStnByColumn() As Boolean
        Get
            Return rbtDiaStnByColumn
        End Get
        Set(ByVal value As Boolean)
            rbtDiaStnByColumn = value
        End Set
    End Property
    Private chkMultiCounter As Boolean = True
    Public Property p_chkMultiCounter() As Boolean
        Get
            Return chkMultiCounter
        End Get
        Set(ByVal value As Boolean)
            chkMultiCounter = value
        End Set
    End Property
    Private chkPendTr As Boolean = True
    Public Property p_chkPendTr() As Boolean
        Get
            Return chkPendTr
        End Get
        Set(ByVal value As Boolean)
            chkPendTr = value
        End Set
    End Property
    Private chkCostCentreWise As Boolean = True
    Public Property p_chkCostCentreWise() As Boolean
        Get
            Return chkCostCentreWise
        End Get
        Set(ByVal value As Boolean)
            chkCostCentreWise = value
        End Set
    End Property
    Private chkpartsale As Boolean = True
    Public Property p_chkpartsale() As Boolean
        Get
            Return chkpartsale
        End Get
        Set(ByVal value As Boolean)
            chkpartsale = value
        End Set
    End Property
End Class