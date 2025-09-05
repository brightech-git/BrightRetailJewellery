Imports System.Data.OleDb
Public Class W_LotBulkIssueDia
    Dim StrSql As String
    Dim Da As OleDbDataAdapter
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ObjGPack.Validator_Object(Me)
    End Sub

    Private Sub LotBulkIssueDia_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub LotBulkIssueDia_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        StrSql = " SELECT DESIGNERNAME FROM " & CNADMINDB & "..DESIGNER WHERE ACTIVE = 'Y' ORDER BY DESIGNERNAME"
        ObjGPack.FillCombo(StrSql, cmbDesigner_MAN)
        StrSql = " SELECT CTLTEXT FROM " & CNADMINDB & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE' AND CTLTEXT = 'Y'"
        If ObjGPack.GetSqlValue(StrSql, , "-1") = "-1" Then
            cmbCostCentre_Man.Enabled = False
        End If
        StrSql = " SELECT 1 FROM " & CNADMINDB & "..SOFTCONTROL WHERE CTLID = 'ITEMCOUNTER' AND CTLTEXT = 'Y'"
        If objGPack.GetSqlValue(StrSql, , "-1") = "-1" Then
            cmbItemCounter_MAN.Enabled = False
        End If
        cmbItemCounter_MAN.Text = ""
        If cmbCostCentre_Man.Enabled = True Then
            cmbCostCentre_Man.Items.Clear()
            StrSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            objGPack.FillCombo(StrSql, cmbCostCentre_Man, False)
            cmbCostCentre_Man.Text = ""
        End If
        If cmbItemCounter_MAN.Enabled = True Then
            StrSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ACTIVE = 'Y' ORDER BY DISPLAYORDER,ITEMCTRNAME"
            objGPack.FillCombo(StrSql, cmbItemCounter_MAN)
            cmbItemCounter_MAN.Enabled = True
            cmbItemCounter_MAN.Text = ""
        End If
        cmbEntryType.Text = "REGULAR"
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If objGPack.Validator_Check(Me) Then Exit Sub
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
End Class