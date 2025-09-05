Imports System.Data.OleDb
Imports System.Text.RegularExpressions
Imports System.IO

Public Class frmAcheadAttachments
#Region "Declaration"
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim dt As New DataTable
    Dim strSql As String
    Dim flagSave As Boolean = False
    Dim defalutDestination As String = Nothing
    Dim defalutSourcePath As String = Nothing
    Dim picExtension As String = Nothing
    Dim picPath As String = Nothing
#End Region

#Region "Functions and Methods"

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Function funcCatchPic(ByVal btn As System.Windows.Forms.Button)
        If Not IO.Directory.Exists(defalutDestination) Then
            MsgBox(defalutDestination & vbCrLf & " Destination Path not found" & vbCrLf & "Please make the correct path", MsgBoxStyle.Information)
            Exit Function
        End If

        'If flagDeviceMode = True Then
        ' piccap()
        ' btnSave.Focus()
        ' Exit Sub
        ' End If
        picCapture.Visible = True
        Try
            Dim openDia As New OpenFileDialog
            Dim str As String
            If IO.File.Exists(defalutSourcePath) Then openDia.InitialDirectory = defalutSourcePath
            str = "JPEG(*.jpg)|*.jpg"
            str += "|Bitmap(*.bmp)|*.bmp"
            str += "|GIF(*.gif)|*.gif"
            str += "|All Files(*.*)|*.*"
            openDia.Filter = str
            If openDia.ShowDialog = Windows.Forms.DialogResult.OK Then

                Dim Finfo As FileInfo
                Finfo = New FileInfo(openDia.FileName)
                picPath = ""
                AutoImageSizer(openDia.FileName, picCapture)
                picPath = openDia.FileName
                picExtension = Finfo.Extension
                Me.SelectNextControl(btn, True, True, True, True)
            Else
                Me.SelectNextControl(btn, True, True, True, True)
            End If
            Return picPath
        Catch ex As Exception
            MsgBox(E0016, MsgBoxStyle.Exclamation)
            Return ""
        End Try
    End Function

    Private Function funcValidation() As Boolean
        If txtAccode.Text = "" Then
            MsgBox("Invalid A/c code.", MsgBoxStyle.Information)
            Exit Function
        End If
        If txtAcName.Text = "" Then
            MsgBox("Invalid A/c Name.", MsgBoxStyle.Information)
            Exit Function
        End If
        If cmbTypeName.Text.Trim = "" Then
            MsgBox("Invalid type.", MsgBoxStyle.Information)
            Exit Function
        End If
        If lblPanCard.Text.ToString = "" Then
            MsgBox("Invalid Image Path.", MsgBoxStyle.Information)
            Exit Function
        End If
        strSql = " SELECT ACNAME"
        strSql += " FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & txtAccode.Text & "'"
        If txtAcName.Text.ToString <> objGPack.GetSqlValue(strSql) Then
            MsgBox("A/c code not Match.", MsgBoxStyle.Information)
            Exit Function
        End If
        Return True
    End Function

    Private Function funcSave(ByVal Accode As String, ByVal type As String, ByVal Path As String)
        strSql = " DELETE FROM " & cnAdminDb & "..ACHEADDOC WHERE ACCODE = '" & Accode & "' AND TYPE='" & type & "'"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        strSql = " INSERT INTO " & cnAdminDb & "..ACHEADDOC(ACCODE,TYPE,FILEPATH,USERID,UPDATED,UPTIME) "
        strSql += vbCrLf + " SELECT '" & Accode & "' ACCODE"
        strSql += vbCrLf + ",'" & type & "' TYPE"
        strSql += vbCrLf + ",'" & Path & "' PATH "
        strSql += vbCrLf + " ," & userId & " USERID"
        strSql += vbCrLf + " ,'" & Today.Date.ToString("yyyy-MM-dd") & "' UPDATED"
        strSql += vbCrLf + " ,'" & Date.Now.ToLongTimeString & "' UPTIME"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
    End Function

#End Region

#Region "Events"

    Private Sub frmAcheadAttachments_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmAcheadAttachments_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmbTypeName.Items.Clear()
        strSql = " SELECT NAME FROM " & cnAdminDb & "..IDPROOF ORDER BY NAME"
        objGPack.FillCombo(strSql, cmbTypeName, False, False)
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICPATH'"
        defalutDestination = UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", , tran))
        If Not defalutDestination.EndsWith("\") And defalutDestination <> "" Then defalutDestination += "\"
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICSOURCEPATH'"
        defalutSourcePath = UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", , tran))
        If txtAccode.Text = "" Then
            btnNew_Click(Me, e)
        Else
            btnOpen_Click(Me, e)
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        lblPanCard.Text = ""
        gridView.DataSource = Nothing
        AutoImageSizer(My.Resources.no_photo, picCapture, PictureBoxSizeMode.StretchImage)
        txtAccode.Focus()
        txtAccode.SelectAll()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        gridView.DataSource = Nothing
        AutoImageSizer(My.Resources.no_photo, picCapture, PictureBoxSizeMode.StretchImage)
        strSql = " SELECT ACNAME"
        strSql += " FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & txtAccode.Text & "'"
        strSql += GetAcNameQryFilteration()
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            txtAcName.Text = dt.Rows(0)("ACNAME").ToString
            lblPanCard.Text = ""
            strSql = " SELECT "
            strSql += " (SELECT NAME FROM " & cnAdminDb & "..IDPROOF WHERE CONVERT(VARCHAR,ID)=A.TYPE )TYPE,FILEPATH "
            strSql += " FROM " & cnAdminDb & "..ACHEADDOC AS A WHERE ACCODE='" & txtAccode.Text & "' ORDER BY TYPE"
            Dim dtPath As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtPath)
            If dtPath.Rows.Count > 0 Then
                gridView.DataSource = dtPath
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            End If
        Else
            txtAcName.Text = ""
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not funcValidation() Then Exit Sub
        Try
            Dim IDIMAGEFILE As String = ""
            Dim TYPE As String = objGPack.GetSqlValue("SELECT ID FROM " & cnAdminDb & "..IDPROOF WHERE NAME='" & cmbTypeName.Text.ToString & "'", , "").ToString
            IDIMAGEFILE = TYPE + "_" + txtAccode.Text.ToString()
            If File.Exists(lblPanCard.Text.ToString) = True Then
                Dim serverPath As String = Nothing
                Dim fileDestPath As String = (defalutDestination + IDIMAGEFILE + IIf(picExtension.ToString.StartsWith("."), picExtension.ToString, "." + picExtension.ToString))
                Dim Finfo As FileInfo
                Finfo = New FileInfo(fileDestPath)
                If IO.Directory.Exists(Finfo.Directory.FullName) = False Then
                    MsgBox(Finfo.Directory.FullName & " Does not exist. Please make a corresponding path", MsgBoxStyle.Information)
                    Exit Sub
                End If
                If UCase(lblPanCard.Text.ToString) <> fileDestPath.ToUpper Then
                    Dim cFile As New FileInfo(lblPanCard.Text.ToString)
                    cFile.CopyTo(fileDestPath, True)
                End If
                funcSave(txtAccode.Text.ToString, TYPE, fileDestPath)
            End If
            MsgBox("Saved...", MsgBoxStyle.Information)
            cmbTypeName.Text = ""
            lblPanCard.Text = ""
            cmbTypeName.Focus()
            AutoImageSizer(My.Resources.no_photo, picCapture, PictureBoxSizeMode.StretchImage)
            'btnNew_Click(Me, e)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtAccode_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtAccode.KeyDown
        If e.KeyCode = Keys.Insert Then
            strSql = "SELECT ''SNO,ACCODE,PREVILEGEID,TITLE,INITIAL,ACNAME PNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,PINCODE,STATE,COUNTRY,"
            strSql += " PHONENO PHONERES,MOBILE,EMAILID EMAIL,FAX,PAN,''IDTYPE,'' IDNO,ISNULL(RELIGION,'')RELIGION"
            strSql += " FROM " & cnAdminDb & "..ACHEAD WHERE ISNULL(ACTIVE,'')<>'N' ORDER BY ACNAME"
            txtAccode.Text = BrighttechPack.SearchDialog.Show("Select Customer", strSql, cn, 5, 1)
        End If
    End Sub

    Private Sub txtAccode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAccode.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            gridView.DataSource = Nothing
            AutoImageSizer(My.Resources.no_photo, picCapture, PictureBoxSizeMode.StretchImage)
            strSql = " SELECT ACNAME"
            strSql += " FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & txtAccode.Text & "'"
            strSql += GetAcNameQryFilteration()
            Dim dt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                txtAcName.Text = dt.Rows(0)("ACNAME").ToString
                lblPanCard.Text = ""
                strSql = " SELECT "
                strSql += " (SELECT NAME FROM " & cnAdminDb & "..IDPROOF WHERE CONVERT(VARCHAR,ID)=A.TYPE )TYPE,FILEPATH "
                strSql += " FROM " & cnAdminDb & "..ACHEADDOC AS A WHERE ACCODE='" & txtAccode.Text & "'  ORDER BY TYPE"
                Dim dtPath As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtPath)
                If dtPath.Rows.Count > 0 Then
                    gridView.DataSource = dtPath
                    gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
                    'For Each dr As DataRow In dtPath.Rows
                    '    If dr.Item("TYPE").ToString = "A" Then
                    '        lblAadhar.Text = dr.Item("FILEPATH").ToString
                    '    End If
                    '    If dr.Item("TYPE").ToString = "P" Then
                    '        lblPanCard.Text = dr.Item("FILEPATH").ToString
                    '    End If
                    '    If dr.Item("TYPE").ToString = "O" Then
                    '        lblPassport.Text = dr.Item("FILEPATH").ToString
                    '    End If
                    '    If dr.Item("TYPE").ToString = "D" Then
                    '        lblDrivingLicense.Text = dr.Item("FILEPATH").ToString
                    '    End If
                    '    If dr.Item("TYPE").ToString = "G" Then
                    '        lblGST.Text = dr.Item("FILEPATH").ToString
                    '    End If
                    'Next
                End If
            Else
                txtAcName.Text = ""
            End If
        End If
    End Sub

    Private Sub btnBrwPanCard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrwPanCard.Click
        If cmbTypeName.Text = "" Then
            MsgBox("Invalid type.", MsgBoxStyle.Information)
            cmbTypeName.Focus()
            Exit Sub
        End If
        lblPanCard.Text = funcCatchPic(btnBrwPanCard)
    End Sub

    Private Sub lblPanCard_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblPanCard.LinkClicked
        If lblPanCard.Text <> "" Then
            AutoImageSizer(lblPanCard.Text.ToString, picCapture)
        End If
    End Sub

    Private Sub gridView_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gridView.SelectionChanged
        If gridView.Rows.Count > 0 Then
            If gridView.CurrentRow.Cells("FILEPATH").Value.ToString <> "" Then
                AutoImageSizer(gridView.CurrentRow.Cells("FILEPATH").Value.ToString, picCapture)
                cmbTypeName.Text = gridView.CurrentRow.Cells("TYPE").Value.ToString
            End If
        End If
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        btnSave_Click(Me, e)
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        btnOpen_Click(Me, e)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, e)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, e)
    End Sub

#End Region

End Class