Imports System.Data.OleDb
Public Class frmBviewadv
    Public Amount As Double = Nothing
    Public DtTdsCategory As New DataTable
    Public Accode As String
    Dim Da As OleDb.OleDbDataAdapter
    Dim StrSql As String
    Public EditFlag As Boolean = False
    Public sno As String = "-1"


    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        objGPack.Validator_Object(Me)
        ' Add any initialization after the InitializeComponent() call.
        StrSql = " SELECT NAME"
        StrSql += " FROM " & cnAdminDb & "..SYSCOLUMNS WHERE ID = (SELECT ID FROM " & cnAdminDb & "..SYSOBJECTS WHERE NAME = 'PERSONALINFO')"
        StrSql += " AND NAME IN ('ACCODE','ADDRESS1','ADDRESS2','ADDRESS3','APPVER','AREA','CITY','COSTID','COUNTRY','DOORNO','EMAIL','FAX','MOBILE','PHONERES','PINCODE','PNAME','PREVILEGEID','STATE')"
        StrSql += " ORDER BY NAME"
        objGPack.FillCombo(StrSql, cmbSearchKey, , False)
        cmbSearchKey.Text = "P.PNAME"
        sno = "-1"
    End Sub
    Private Sub frmBviewadv_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        ElseIf e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    
    Private Sub txtSearchString_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSearchString.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtSearchString.Text <> "" Then
                StrSql = " SELECT DISTINCT ISNULL(A.TRANNO,99999) TRANNO,P.TRANDATE,P.PREVILEGEID,P.ACCODE,P.PNAME,P.DOORNO,P.ADDRESS1,P.ADDRESS2,P.ADDRESS3,P.AREA,P.PINCODE,P.CITY,P.COUNTRY,P.PHONERES,P.MOBILE,P.SNO SNO"
                StrSql += " FROM " & cnAdminDb & "..PERSONALINFO P LEFT JOIN " & cnAdminDb & "..CUSTOMERINFO C ON P.SNO = C.PSNO LEFT JOIN " & cnStockDb & "..ACCTRAN A ON C.BATCHNO = A.BATCHNO WHERE 1=1"
                If rbtExact.Checked Then
                    StrSql += " AND P." & cmbSearchKey.Text & " = '" & txtSearchString.Text & "'"
                Else
                    StrSql += " AND P." & cmbSearchKey.Text & " LIKE '%" & txtSearchString.Text & "%'"
                End If
                StrSql += " AND A.TRANDATE BETWEEN '" & dtpBillDatef.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpBillDatet.Value.ToString("yyyy-MM-dd") & "'"
                sno = BrighttechPack.SearchDialog.Show("Select Customer", StrSql, cn, , 15)
                If sno <> "" Then
                    Me.DialogResult = Windows.Forms.DialogResult.OK
                    Me.Close()
                End If
            End If
        End If
    End Sub


    Private Sub frmAccTds_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        cmbOGrsNet.Items.Add("GRS")
        cmbOGrsNet.Items.Add("NET")
        cmbOGrsNet.Text = "GRS"
        Me.dtpBillDatef.Select()
    End Sub


    Private Sub txtWt_To_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtWt_To.KeyPress
        'If e.KeyChar = Chr(Keys.Enter) Then Me.DialogResult = Windows.Forms.DialogResult.OK : Me.Close()
    End Sub

    

    Private Sub chkDate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDate.CheckedChanged
        dtpBillDatef.Enabled = chkDate.Checked
        dtpBillDatet.Enabled = chkDate.Checked
    End Sub
End Class