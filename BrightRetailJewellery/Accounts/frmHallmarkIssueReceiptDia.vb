Imports System.Data.OleDb
Public Class frmHallmarkIssueReceiptDia
    Dim StrSql As String
    Dim Da As OleDbDataAdapter
    Dim SubItemId As Integer
    Dim OrgPcs As Integer = 0
    Dim OrgGrsWt As Double = 0
    Dim OrgNetWt As Double = 0

    Public Sub New(ByVal itemid As Integer, ByVal subItemid As Integer, ByVal pcs As Integer, ByVal grswt As Decimal, ByVal netwt As Decimal)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)

        StrSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACTIVE = 'Y' ORDER BY ACNAME"
        objGPack.FillCombo(StrSql, cmbDesigner_MAN)
        cmbDesigner_MAN.Enabled = True

        'StrSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=" & itemid
        txtItemCode_Num_Man.Text = itemid
        txtPieces_Num_Man.Text = pcs
        txtGrossWt_Wet.Text = grswt
        txtNetWt_Wet.Text = netwt
        OrgPcs = pcs : OrgGrsWt = grswt : OrgNetWt = netwt
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
        StrSql = "SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID =" & subItemid
        cmbSubItemName_Man.Items.Clear()
        'cmbSubItemName_Man.Items.Add("ALL")
        objGPack.FillCombo(StrSql, cmbSubItemName_Man, False)
        Me.BackColor = Color.Silver
        pnlItemCode.BackColor = Color.Silver
    End Sub
    Private Sub frmHallmarkIssueReceiptDia_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If objGPack.Validator_Check(Me) Then Exit Sub
        If Val(txtPieces_Num_Man.Text) > OrgPcs Then MsgBox("Pcs should Exceeds Actual Receipt Pcs", MsgBoxStyle.Information) : txtPieces_Num_Man.Text = OrgPcs : txtPieces_Num_Man.Focus() : Exit Sub
        If Val(txtGrossWt_Wet.Text) > OrgGrsWt Then MsgBox("GrsWt should Exceeds Actual Receipt GrsWt", MsgBoxStyle.Information) : txtGrossWt_Wet.Text = OrgGrsWt : txtGrossWt_Wet.Focus() : Exit Sub
        If Val(txtNetWt_Wet.Text) > OrgNetWt Then MsgBox("NetWt should Exceeds Actual Receipt NetWt", MsgBoxStyle.Information) : txtNetWt_Wet.Text = OrgNetWt : txtNetWt_Wet.Focus() : Exit Sub
        If txtItemName.Text = "" Then
            MessageBox.Show("Please Select ItemId...")
            txtItemCode_Num_Man.Focus()
            Exit Sub
        End If
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub frmHallmarkIssueReceiptDia_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
End Class