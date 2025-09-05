Imports System.Data.OleDb
Imports System.IO
Imports System.Runtime.InteropServices
Public Class SendManual
    Dim StrSql As String
    Dim Da As OleDbDataAdapter
    Dim Cmd As OleDbCommand
    Dim Syncdb As String = ""
    Dim frmCostId As String

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub SendManual_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub SendManual_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        frmcostid = objGPack.GetSqlValue(" SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTID'").ToUpper
        'Dim SenderId As String = objGPack.GetSqlValue(" SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE MAIN = 'Y'").ToUpper
        Dim SenderId As String = objGPack.GetSqlValue(" SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE COSTID = '" & frmCostId & "'").ToUpper
        StrSql = " SELECT COSTID "
        StrSql += " FROM " & cnAdminDb & "..SYNCCOSTCENTRE S"
        StrSql += " where costid <> '" & SenderId & "'"
        objGPack.FillCombo(StrSql, cmbCostId)
        If cmbCostId.Items.Count > 0 Then
            cmbCostId.SelectedIndex = 0
        End If
        Syncdb = cnAdminDb
        Dim uprefix As String = Mid(cnAdminDb, 1, 3)
        If GetAdmindbSoftValue("SYNC-SEPDATADB", "N", tran) = "Y" Then
            Dim usuffix As String = "UTILDB"
            If DbChecker(uprefix + usuffix, True, tran) <> 0 Then Syncdb = uprefix + usuffix

        End If

    End Sub

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        Dim fDia As New FolderBrowserDialog
        If fDia.ShowDialog = Windows.Forms.DialogResult.OK Then
            txtFilePath.Text = fDia.SelectedPath
        End If
    End Sub

    Private Sub btnGenerate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        If cmbCostId.Text = "" Then
            MsgBox("To Costid should not empty", MsgBoxStyle.Information)
            cmbCostId.Select()
            Exit Sub
        End If
        Dim fInfo As IO.FileInfo = Nothing
        Dim objZip As BrighttechPack.Zipper

        Dim TblName As String = "SYNC-" & frmCostId & "-" & cmbCostId.Text & "-" & Today.Date.ToString("ddMMyy") & "-" & Date.Now.ToString("HHssmm")
        StrSql = "SELECT * FROM " & Syncdb & "..SENDSYNC"
        StrSql += " WHERE FROMID = '" & frmCostId & "' AND STATUS = 'N'"
        StrSql += " AND TOID = '" & cmbCostId.Text & "'"
        StrSql += " ORDER BY UID"
        Dim ds As New DataSet
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(ds, TblName)
        If Not ds.Tables(0).Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        If ds.Tables(0).Rows.Count > 0 Then
            ''Encrypting Qry
            For Each row As DataRow In ds.Tables(0).Rows
                row("SQLTEXT") = BrighttechPack.Methods.EncryptXml(row("SQLTEXT").ToString)
            Next
            ds.Tables(0).AcceptChanges()

            ''Creating xml File
            Dim xmlFile As String = System.IO.Path.GetTempPath & "\" & TblName & ".xml"
            Dim fs As New IO.StreamWriter(xmlFile)
            ds.WriteXml(fs, XmlWriteMode.WriteSchema)
            fs.Close()

            ''Zipping
            Dim zipFile As String = txtFilePath.Text & "\" & TblName & ".zip"
            objZip = New BrighttechPack.Zipper
            objZip.Zip(xmlFile, zipFile)
            If IO.File.Exists(xmlFile) Then IO.File.Delete(xmlFile)

            ''Updating sended records
            fInfo = New FileInfo(zipFile)
            StrSql = " UPDATE " & Syncdb & "..SENDSYNC SET STATUS = 'M',UPDFILE = '" & fInfo.Name & "'"
            StrSql += " WHERE FROMID = '" & frmCostId & "' AND STATUS = 'N'"
            StrSql += " AND TOID = '" & cmbCostId.Text & "'"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            MsgBox("Successfully File Generated", MsgBoxStyle.Information)
            Me.Close()
        End If
    End Sub

    Private Sub txtFilePath_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFilePath.GotFocus
        SendKeys.Send("{TAB}")
    End Sub
End Class