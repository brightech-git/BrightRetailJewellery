Imports System.Data.OleDb
Public Class frmAutoBrsMast
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim flagSave As Boolean = False
    Dim TACCODE As String
    'Dim HideAccLink As Boolean = IIf(GetAdmindbSoftValue("HIDE_ACHARI_ACCLINK", "Y") = "Y", True, False)
    Dim _FourCMaintain As Boolean = IIf(GetAdmindbSoftValue("4CMAINTAIN", "Y") = "Y", True, False)
    Dim CENTRATE_DESIGN As Boolean = IIf(GetAdmindbSoftValue("CENTRATE_DES", "Y") = "Y", True, False)
    Dim selectedcostid As String
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Function funcCallGrid() As Integer
        strSql = " SELECT AB.ACCODE,A.ACNAME"
        strSql += " ,CASE WHEN ISNULL(FILETYPE,'')='CS' THEN 'CSV' ELSE 'EXCEL' END FILETYPE"
        strSql += " ,FILEPATH,FILENAME"
        strSql += " ,CASE WHEN ISNULL(AB.ACTIVE,'')='N' THEN 'NO' ELSE 'YES' END ACTIVE"
        strSql += " ,CHEQUEDATE,CHEQUENO,DEPOSIT,WITHDRAWAL"
        strSql += " FROM " & cnAdminDb & "..AUTOBRSMAST AS AB"
        strSql += " LEFT JOIN " & cnAdminDb & "..ACHEAD A ON A.ACCODE = AB.ACCODE"
        strSql += " ORDER BY  ACNAME"
        funcOpenGrid(strSql, gridView)
        With gridView
            .Columns("ACCODE").Visible = False
            .Columns("ACNAME").Width = 200
        End With
    End Function

    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcSave() As Integer
        If flagSave = False Then
            funcAdd()
        Else
            funcUpdate()
        End If
    End Function
    Function funcAdd() As Integer
        Dim ds As New Data.DataSet
        ds.Clear()
        Dim Accode As String = ""
        strSql = " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE"
        strSql += " ACNAME = '" & cmbAcname_MAN.Text & "'"
        Accode = objGPack.GetSqlValue(strSql, , , tran)
        If funcCheckUnique(Accode) = True Then MsgBox("This acname already exists.", MsgBoxStyle.Information) : Exit Function
        strSql = " INSERT INTO " & cnAdminDb & "..AUTOBRSMAST"
        strSql += " (ACCODE,FILETYPE,FILEPATH,FILENAME,ACTIVE,CHEQUEDATE,CHEQUENO,DEPOSIT,WITHDRAWAL"
        strSql += " )VALUES("
        strSql += " '" & Accode & "'" 'ACCODE
        strSql += " ,'" & Mid(cmbFileType_MAN.Text, 1, 2) & "'" 'FILETYPE
        strSql += " ,'" & txtFilepath.Text & "'" 'FILEPATH
        strSql += " ,'" & txtFilename.Text & "'" 'FILENAME
        strSql += " ,'" & Mid(cmbAcname_MAN.Text, 1, 1) & "'" 'ACTIVE
        strSql += " ,'" & txtCheuedate.Text & "'" 'CHEQUEDATE
        strSql += " ,'" & txtChequeNo.Text & "'" 'CHEQUENO
        strSql += " ,'" & txtDepositAmt.Text & "'" 'DEPOSIT
        strSql += " ,'" & txtWithdraAmt.Text & "'" 'WITHDRAWAL
        strSql += " )"
        Try
            ExecQuery(SyncMode.Master, strSql, cn, )
            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcUpdate() As Integer
        Dim Accode As String = ""

        strSql = " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE"
        strSql += " ACNAME = '" & cmbAcname_MAN.Text & "'"
        Accode = objGPack.GetSqlValue(strSql, , , tran)

        strSql = " UPDATE " & cnAdminDb & "..AUTOBRSMAST SET"
        strSql += " FILETYPE='" & Mid(cmbFileType_MAN.Text, 1, 2) & "'"
        strSql += " ,FILEPATH='" & txtFilepath.Text & "'"
        strSql += " ,FILENAME='" & txtFilename.Text & "'"
        strSql += " ,ACTIVE='" & Mid(cmbAcname_MAN.Text, 1, 1) & "'"
        strSql += " ,CHEQUEDATE='" & txtCheuedate.Text & "'"
        strSql += " ,CHEQUENO='" & txtChequeNo.Text & "'"
        strSql += " ,DEPOSIT='" & txtDepositAmt.Text & "'"
        strSql += " ,WITHDRAWAL='" & txtWithdraAmt.Text & "'"
        strSql += " WHERE ACCODE = '" & Accode & "'"
        Try
            ExecQuery(SyncMode.Master, strSql, cn, )
            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcGetDetails(ByVal temp As String) As Integer
        strSql = " SELECT AB.ACCODE,A.ACNAME"
        strSql += " ,CASE WHEN ISNULL(FILETYPE,'')='CS' THEN 'CSV' ELSE 'EXCEL' END FILETYPE"
        strSql += " ,FILEPATH,FILENAME"
        strSql += " ,CASE WHEN ISNULL(AB.ACTIVE,'')='N' THEN 'NO' ELSE 'YES' END ACTIVE"
        strSql += " ,CHEQUEDATE,CHEQUENO,DEPOSIT,WITHDRAWAL"
        strSql += " FROM " & cnAdminDb & "..AUTOBRSMAST AS AB"
        strSql += " LEFT JOIN " & cnAdminDb & "..ACHEAD AS A ON AB.ACCODE=A.ACCODE"
        strSql += " WHERE AB.ACCODE = '" & temp & "'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        With dt.Rows(0)
            cmbAcname_MAN.Text = .Item("ACNAME").ToString
            cmbFileType_MAN.Text = .Item("FILETYPE").ToString
            txtFilepath.Text = .Item("FILEPATH").ToString
            txtFilename.Text = .Item("FILENAME").ToString
            cmbActive.Text = .Item("ACTIVE").ToString
            txtCheuedate.Text = .Item("CHEQUEDATE").ToString
            txtChequeNo.Text = .Item("CHEQUENO").ToString
            txtDepositAmt.Text = .Item("DEPOSIT").ToString
            txtWithdraAmt.Text = .Item("WITHDRAWAL").ToString
        End With
        flagSave = True
        TACCODE = temp

    End Function

    Private Sub frmAutoBrsMast_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            cmbAcname_MAN.Focus()
        End If
    End Sub

    Private Sub frmAutoBrsMast_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmAutoBrsMast_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ISNULL(ACTIVE,'')<>'N' ORDER BY ACNAME"
        objGPack.FillCombo(strSql, cmbAcname_MAN, True)

        cmbFileType_MAN.Items.Clear()
        cmbFileType_MAN.Items.Add("EXCEL")
        cmbFileType_MAN.Items.Add("CSV")

        cmbActive.Items.Clear()
        cmbActive.Items.Add("YES")
        cmbActive.Items.Add("NO")

        funcGridStyle(gridView)
        funcNew()
    End Sub
    Function funcNew()
        objGPack.TextClear(Me)
        cmbAcname_MAN.Text = ""
        cmbFileType_MAN.Text = ""
        flagSave = False
        funcCallGrid()
    End Function
    Function funcCheckUnique(ByVal Accode As String) As Boolean
        Dim str As String = Nothing
        Dim dt As New DataTable
        dt.Clear()
        str = " DECLARE @ACCODE AS VARCHAR "
        str += " SET @ACCODE = '" & Accode & "'"
        str += " SELECT 1 FROM " & cnAdminDb & "..AUTOBRSMAST"
        str += " WHERE ACCODE='" & Accode & "'"
        da = New OleDbDataAdapter(str, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Return True ''Already Exist
        End If
        Return False
    End Function
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
    End Sub
    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click, OpenToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        funcCallGrid()
        gridView.Focus()
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        funcSave()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        funcNew()
    End Sub
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        funcExit()
    End Sub
    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Escape Then
            cmbAcname_MAN.Focus()
        ElseIf e.KeyCode = Keys.Enter Then
            e.Handled = True
            gridView.CurrentCell = gridView.CurrentCell
            If gridView.RowCount > 0 Then
                funcGetDetails(gridView.Item(0, gridView.CurrentRow.Index).Value)
                cmbAcname_MAN.Focus()
            End If
        End If
    End Sub
    Private Sub btnopenFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnopenFile.Click
        Dim OpenDialog As New OpenFileDialog
        Dim Str As String
        Str = "(*.xls) | *.xls"
        Str += "|(*.xlsx) | *.xlsx"
        Str += "|(*.csv) | *.csv"
        OpenDialog.Filter = Str
        If OpenDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim path As String = OpenDialog.FileName
            If path <> "" Then
                Dim Filename As String = ""
                Dim Filepath As String = ""
                Dim splitval() As String = path.Split("\")
                If splitval.Length > 1 Then
                    Filename = splitval(splitval.Length - 1)
                    Filepath = path.Replace(Filename, "")
                End If
                txtFilepath.Text = Filepath
                txtFilename.Text = Filename
                txtFilename.Focus()
            End If
        End If
    End Sub
End Class