Imports System.Data.OleDb
Public Class CashTranUpdate
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Public batchNo As String
    Public Remark As String
    Public Trantype As String
    Public Acname As String
    Public Accode As String

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        strSql = " SELECT ISNULL(PRONAME,' ') FROM " & cnAdminDb & "..PROCESSTYPE  WHERE PROTYPE IN ('B','I') AND PROMODULE = 'P'"
        objGPack.FillCombo(strSql, cmbTranType_Man, True, False)

        'strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE in ('D','G')"
        'objGPack.FillCombo(strSql, cmbPartyName, True, False)

    End Sub



    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub



    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim NAccode As String = ""
        Dim NRemark As String = ""
        Dim NTrantype As String = ""
        NRemark = txtRemark.Text
        NTrantype = cmbTranType_Man.Text

        Me.Refresh()
        Try

            tran = Nothing
            tran = cn.BeginTransaction()
            If NRemark <> Remark And batchNo <> "" Then
                strSql = "UPDATE " & cnStockDb & "..ACCTRAN SET REMARK1='" & NRemark & "' WHERE BATCHNO='" & batchNo & "' AND PAYMODE<>'CA'"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
                strSql = "UPDATE " & cnAdminDb & "..OUTSTANDING SET REMARK1='" & NRemark & "' WHERE BATCHNO='" & batchNo & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
            End If
            If NTrantype <> Trantype And batchNo <> "" Then
                Dim CTRANCODE As Integer = Val(objGPack.GetSqlValue("SELECT PROID FROM " & cnAdminDb & "..PROCESSTYPE WHERE PRONAME='" & NTrantype & "'", "", "", tran))
                strSql = "UPDATE " & cnAdminDb & "..OUTSTANDING SET CTRANCODE='" & CTRANCODE & "' WHERE BATCHNO='" & batchNo & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
            End If

            tran.Commit()
            tran = Nothing
            Me.Close()
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            If Not tran Is Nothing Then tran.Dispose()
        End Try
    End Sub


    Private Sub btnExit_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub CashTranUpdate_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub CashTranUpdate_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmbTranType_Man.Text = Trantype
        txtRemark.Text = Remark
        cmbTranType_Man.Focus()
    End Sub

End Class