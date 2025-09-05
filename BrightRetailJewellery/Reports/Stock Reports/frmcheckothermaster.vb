Imports System.Data.OleDb
Public Class frmcheckothermaster
    Dim strSql As String
    Dim dt As DataTable
    Public Sub New()

        InitializeComponent()
       
    End Sub

    Private Sub frmAccLedgerMore_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        strSql = " SELECT MISCNAME,MISCID,1 RESULT FROM " & cnAdminDb & "..OTHERMASTERENTRY  WHERE ACTIVE='Y'"
        strSql += " ORDER BY MISCID"
        Dim dtMetal = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtMetal)
        BrighttechPack.GlobalMethods.FillCombo(chkmiscname, dtMetal, "MISCNAME", , "ALL")
        chkothermisc.Enabled = False
        chkothermisc.Text = ""
    End Sub

    Private Sub frmcheckothermaster_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
        If e.KeyChar = Chr(Keys.Escape) Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub chkothermisc_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chkothermisc.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub chkmiscname_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chkmiscname.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            chkothermisc.Enabled = True
            strSql = " SELECT NAME,ID,1 RESULT FROM " & cnAdminDb & "..OTHERMASTER WHERE ACTIVE='Y' "
            If chkmiscname.Text <> "ALL" And chkmiscname.Text <> "" Then
                strSql += "AND MISCID IN (SELECT MISCID FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE MISCNAME IN (" & GetQryString(chkmiscname.Text) & "))"
            End If
            strSql += " ORDER BY ID"
            Dim dtMetal = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtMetal)
            BrighttechPack.GlobalMethods.FillCombo(chkothermisc, dtMetal, "NAME", , "ALL")
        End If
    End Sub
End Class