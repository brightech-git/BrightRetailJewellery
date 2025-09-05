Imports System.Data.OleDb
Public Class LotBulkIssueDia
    Dim StrSql As String
    Dim Da As OleDbDataAdapter
    Dim HideAccLink As Boolean = IIf(GetAdmindbSoftValue("HIDE_ACHARI_ACCLINK", "Y") = "Y", True, False)

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        If HideAccLink Then
            StrSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACTIVE = 'Y' ORDER BY DESIGNERNAME"
            objGPack.FillCombo(StrSql, cmbDesigner_MAN)
            cmbDesigner_MAN.Enabled = True
        Else
            cmbDesigner_MAN.Enabled = False
            cmbDesigner_MAN.Text = ""
        End If
        StrSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE' AND CTLTEXT = 'Y'"
        If objGPack.GetSqlValue(StrSql, , "-1") = "-1" Then
            cmbCostCentre_Man.Enabled = False
        End If
        StrSql = " SELECT 1 FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'ITEMCOUNTER' AND CTLTEXT = 'Y'"
        If objGPack.GetSqlValue(StrSql, , "-1") = "-1" Then
            cmbItemCounter_MAN.Enabled = False
        End If
        cmbItemCounter_MAN.Text = ""
        If cmbCostCentre_Man.Enabled = True Then
            cmbCostCentre_Man.Items.Clear()
            StrSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            objGPack.FillCombo(StrSql, cmbCostCentre_Man, False)
            StrSql = " SELECT TOP 1 COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & cnCostId & "' "
            cmbCostCentre_Man.Text = objGPack.GetSqlValue(StrSql, , "")
        End If
        If cmbItemCounter_MAN.Enabled = True Then
            StrSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ACTIVE = 'Y' ORDER BY DISPLAYORDER,ITEMCTRNAME"
            objGPack.FillCombo(StrSql, cmbItemCounter_MAN)
            cmbItemCounter_MAN.Enabled = True
            cmbItemCounter_MAN.Text = ""
        End If
    End Sub

    Private Sub LotBulkIssueDia_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub LotBulkIssueDia_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        cmbEntryType.Text = "REGULAR"
        pnlItemCode.Enabled = chkIssToAssort.Checked
        If pnlItemCode.Enabled = False Then
            txtItemCode_Num_Man.Text = ""
            cmbItemCounter_MAN.Text = ""
            cmbSubItemName_Man.Text = ""
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If objGPack.Validator_Check(Me) Then Exit Sub
        If chkIssToAssort.Checked = True And txtItemName.Text = "" Then
            MessageBox.Show("Please select itemcode...")
            txtItemCode_Num_Man.Focus()
            Exit Sub
        End If
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub txtItemCode_Num_Man_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtItemCode_Num_Man.KeyDown
        If e.KeyCode = Keys.Insert Then  'Insert Key
            StrSql = " SELECT ITEMID,ITEMNAME,"
            StrSql += " CASE WHEN STOCKTYPE = 'T' THEN 'TAGED' "
            StrSql += " WHEN STOCKTYPE = 'N' THEN 'NON TAGED' ELSE 'PACKET BASED' END AS STOCKTYPE,"
            StrSql += " CASE WHEN SUBITEM = 'Y' THEN 'YES' ELSE 'NO' END AS SUBITEM, "
            StrSql += " CASE WHEN CALTYPE = 'W' THEN 'WEIGHT'"
            StrSql += " WHEN CALTYPE = 'R' THEN 'RATE'"
            StrSql += " WHEN CALTYPE = 'F' THEN 'FIXED'"
            StrSql += " WHEN CALTYPE = 'B' THEN 'BOTH'"
            StrSql += " WHEN CALTYPE = 'M' THEN 'METAL RATE' END AS CALTYPE"

            StrSql += " FROM " & cnAdminDb & "..ITEMMAST"
            StrSql += " WHERE ITEMID LIKE '" & txtItemCode_Num_Man.Text & "%'"
            StrSql += " AND ACTIVE = 'Y'"
            StrSql += " AND STUDDED <> 'S'"
            StrSql += GetItemQryFilteration("S")
            StrSql += " ORDER BY ITEMNAME"
            txtItemCode_Num_Man.Text = BrighttechPack.SearchDialog.Show("Search ItemId", StrSql, cn, 1)
        ElseIf e.KeyCode = Keys.Enter Then
            If txtItemCode_Num_Man.Text = "" Then
                Exit Sub
            End If
            Dim itemName As String = objGPack.GetSqlValue(" select ItemName from " & cnAdminDb & "..ItemMast where ItemId = '" & txtItemCode_Num_Man.Text & "' AND ACTIVE = 'Y' AND STUDDED <> 'S'" & GetItemQryFilteration("S"), "itemname")
            txtItemName.Text = itemName
            If txtItemName.Text = "" Then
                MsgBox(E0004 + Me.GetNextControl(txtItemCode_Num_Man, False).Text, MsgBoxStyle.Information)
                txtItemCode_Num_Man.Select()
                Exit Sub
            End If
            ''SUBITEMSETTING
            StrSql = GetSubItemQry(New String() {"SUBITEMNAME"}, Val(txtItemCode_Num_Man.Text))
            cmbSubItemName_Man.Items.Clear()
            cmbSubItemName_Man.Items.Add("ALL")
            objGPack.FillCombo(StrSql, cmbSubItemName_Man, False)
            cmbSubItemName_Man.Text = "ALL"
            If Not cmbSubItemName_Man.Items.Count > 1 Then
                cmbSubItemName_Man.Enabled = False
            Else
                cmbSubItemName_Man.Enabled = True
            End If
            StrSql = "SELECT (SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID=DEFAULTCOUNTER)CTR "
            StrSql += " FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = '" & txtItemCode_Num_Man.Text & "' AND ACTIVE = 'Y'"
            Dim CtrName As String = objGPack.GetSqlValue(StrSql, "CTR", "")
            cmbItemCounter_MAN.Text = CtrName
        End If
    End Sub

    Private Sub chkIssToAssort_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIssToAssort.CheckedChanged
        pnlItemCode.Enabled = chkIssToAssort.Checked
        If pnlItemCode.Enabled = False Then
            txtItemCode_Num_Man.Text = ""
            cmbItemCounter_MAN.Text = ""
            cmbSubItemName_Man.Text = ""
        End If
    End Sub
End Class