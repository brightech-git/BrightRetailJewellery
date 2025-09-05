Imports System.Data.OleDb
Imports System.IO
Public Class frmCardDetails
    'calno130913 Alter by vasanth for kanagalakshmi diamonds
    Dim strSql As String = ""
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim dt As DataTable
    Public TagNoEnb As Boolean = True
    Public cardImagePath As String
    Dim defaultPic As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICPATH'")

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Try
            If Not defaultPic.EndsWith("\") Then defaultPic += "\"
            funcNew()
        Catch ex As Exception
            MsgBox("Message :" + ex.Message + "StackTrace   :" + ex.StackTrace)
        End Try
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Try
            funcNew()
        Catch ex As Exception
            MsgBox("Message :" + ex.Message + "StackTrace   :" + ex.StackTrace)
        End Try
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        'Dim sqlCn As SqlClient.SqlConnection
        'If ConInfo.lDbLoginType = "S" Then
        '    sqlCn = New SqlClient.SqlConnection("Persist Security Info=False; Initial Catalog=;Data Source=" & cnDataSource & ";uid=sa;pwd=;")
        'Else
        '    sqlCn = New SqlClient.SqlConnection("Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=;Data Source=" & ConInfo.lServerName & "")
        'End If
        'Dim sqlCmd As SqlClient.SqlCommand = Nothing
        Try
            strSql = " DELETE FROM " & cnAdminDb & "..GURANTECARD WHERE TAGNO = '" & cmbTagNo.Text & "'"
            ExecQuery(SyncMode.Stock, strSql, cn)
            Dim strWt As String = ""
            For cnt As Integer = 0 To chkLstWt.Items.Count - 1
                If chkLstWt.GetItemCheckState(cnt) = CheckState.Checked Then
                    strWt = strWt & chkLstWt.Items.Item(cnt).ToString
                    If cnt <> chkLstWt.Items.Count - 1 Then
                        strWt = strWt & ","
                    End If
                End If
            Next
            If Strings.Right(strWt, 1) = "," Then
                strWt = Strings.Left(strWt, strWt.Length - 1)
            End If

            strSql = "    INSERT INTO " & cnAdminDb & "..GURANTECARD"
            strSql += "    (TAGNO,DESCART,SHAPE,ESTWTEACH,ESTTWT,ESTMEASUREMENT,COLORGRADE,CLARITYGRADE,TYPEOFFINISH,PICFILENAME,ISTAG)"
            strSql += "    VALUES('" & cmbTagNo.Text & "','" & txtDescription.Text & "','" & cmbShape_Own.Text & "'"
            strSql += "     ,'" & strWt & "','" & txtTotWt.Text & "' ,'" & txtSize.Text & "','" & cmbColorGrade_Own.Text & "','" & cmbClarityGrade_Own.Text & "'"
            strSql += "     ,'" & cmbTypeOfFinish_Own.Text & "','" & cardImagePath & "','" & IIf(cmbTagNo.Text = "", "N", "") & "')"
            ExecQuery(SyncMode.Stock, strSql, cn)

            strSql = "IF (SELECT 1 FROM SYSOBJECTS WHERE NAME ='TEMPGURCARDPRINT')>0"
            strSql += " DROP TABLE TEMPGURCARDPRINT"
            strSql += " CREATE TABLE TEMPGURCARDPRINT(TAGNO VARCHAR(20),DESCART VARCHAR(300),SHAPE VARCHAR(100),ESTTWT VARCHAR(100)"
            strSql += " ,COLORGRADE VARCHAR(100),CLARITYGRADE VARCHAR(100),TYPEOFFINISH VARCHAR(100),PICFILENAME VARCHAR(100)"
            Dim cntUpdate As Integer = 0
            Dim estwt As String = ""
            Dim estmm As String = ""
            For cnt As Integer = 0 To chkLstWt.Items.Count - 1
                If chkLstWt.GetItemCheckState(cnt) = CheckState.Checked Then
                    cntUpdate += 1
                    estwt += ",ESTWT" & cntUpdate & " VARCHAR(25)"
                    estmm += ",ESTMM" & cntUpdate & " VARCHAR(25)"
                End If
            Next
            strSql += estwt
            strSql += estmm
            strSql += ")"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()


            strSql = "INSERT INTO TEMPGURCARDPRINT(TAGNO,DESCART,SHAPE,ESTTWT,COLORGRADE,CLARITYGRADE,TYPEOFFINISH,PICFILENAME)"
            strSql += " SELECT TAGNO,DESCART,SHAPE,ESTTWT,COLORGRADE,CLARITYGRADE,TYPEOFFINISH,PICFILENAME"
            strSql += " FROM " & cnAdminDb & "..GURANTECARD WHERE ISNULL(TAGNO,'') = '" & cmbTagNo.Text & "'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()


            Dim strSize() As String = Split(txtSize.Text, vbCrLf)


            strWt = ""
            cntUpdate = 0
            For cnt As Integer = 0 To chkLstWt.Items.Count - 1
                If chkLstWt.GetItemCheckState(cnt) = CheckState.Checked Then
                    cntUpdate += 1
                    strSql = " UPDATE TEMPGURCARDPRINT SET ESTWT" & cntUpdate & " = '± " & chkLstWt.Items.Item(cnt).ToString & " Carat'"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                    strSql = " UPDATE TEMPGURCARDPRINT SET ESTMM" & cntUpdate & " = '± " & strSize.GetValue(cntUpdate - 1) & " mm'"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                End If
            Next
            'calno130913
            'If cntUpdate < 5 Then
            '    For CNT As Integer = cntUpdate + 1 To 5
            '        strSql = " UPDATE TEMPGURCARDPRINT SET ESTWT" & CNT & " = ''"
            '        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            '        cmd.ExecuteNonQuery()
            '        strSql = " UPDATE TEMPGURCARDPRINT SET ESTMM" & CNT & " = ''"
            '        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            '        cmd.ExecuteNonQuery()
            '    Next
            'End If

            strSql = "ALTER TABLE TEMPGURCARDPRINT ADD TAGIMAGE IMAGE"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()


            'Dim fileStr As New IO.FileStream(PicFileName)
            'Dim reader As New IO.BinaryReader(fileStr)
            'Dim result As Byte() = reader.ReadBytes(CType(fileStr.Length, Integer))
            'fileStr.Read(result, 0, result.Length)
            'fileStr.Close()

            Dim arrByte As Byte()
            If Not cardImagePath = Nothing Then
                If File.Exists(cardImagePath.ToString()) = True Then
                    'sqlCn.Open()
                    Dim Filest As New FileStream(cardImagePath, FileMode.Open, FileAccess.Read)
                    Dim reader As New IO.BinaryReader(Filest)
                    arrByte = reader.ReadBytes(CType(Filest.Length, Integer))
                    Filest.Read(arrByte, 0, arrByte.Length)
                    Filest.Close()
                    strSql = " UPDATE TEMPGURCARDPRINT SET TAGIMAGE = ? WHERE TAGNO = '" & cmbTagNo.Text & "'"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.Parameters.AddWithValue("@image", arrByte)
                    cmd.ExecuteNonQuery()
                    'strSql = " UPDATE TEMPGURCARDPRINT SET TAGIMAGE = @IMAGE WHERE TAGNO = '" & cmbTagNo.Text & "'"
                    'sqlCmd = New SqlClient.SqlCommand(strSql, sqlCn)
                    'sqlCmd.Parameters.AddWithValue("@image", arrByte)
                    'sqlCmd.ExecuteNonQuery()
                    'sqlCn.Close()
                End If
            End If
            'if (File.Exists(@picfiln.ToString()) == true)
            '{
            '    FileStream fileSt = new FileStream(@strCustPhotoPath.ToString().Trim(), FileMode.Open, FileAccess.Read);
            '    arrByte = new byte[fileSt.Length];
            '    fileSt.Read(arrByte, 0, arrByte.Length);
            '    fileSt.Close();
            '}

            ProgressBarShow(ProgressBarStyle.Marquee)
            ProgressBarStep("Loading Report")
            Dim objReport As New GiritechReport
            Dim objRptViewer As New frmReportViewer
            objRptViewer.CrystalReportViewer1.ReportSource = objReport.ShowReport(New crGuranteCard, cnDataSource)
            objRptViewer.MdiParent = Main
            objRptViewer.ShowInTaskbar = False
            objRptViewer.WindowState = FormWindowState.Maximized
            objRptViewer.Dock = DockStyle.Fill
            objRptViewer.Show()
            objRptViewer.CrystalReportViewer1.Select()
            ProgressBarHide()


        Catch ex As Exception
            'If sqlCn.State = ConnectionState.Open Then
            '    sqlCn.Close()
            'End If
            MsgBox("Message :" + ex.Message + "StackTrace   :" + ex.StackTrace)
        End Try
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Try
            funcNew()
        Catch ex As Exception
            MsgBox("Message :" + ex.Message + "StackTrace   :" + ex.StackTrace)
        End Try
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Public Sub funcNew()

        strSql = " SELECT DISTINCT TAGNO FROM " & cnAdminDb & "..ITEMTAG ORDER BY TAGNO"
        If TagNoEnb = True Then
            objGPack.FillCombo(strSql, cmbTagNo, True, False)
        End If

        strSql = "                     SELECT DISTINCT SHAPE FROM " & cnAdminDb & "..GURANTECARD ORDER BY SHAPE"
        objGPack.FillCombo(strSql, cmbShape_Own, True, True)

        'dt = New DataTable()
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dt)
        'comboShape.Text = ""
        'If dt.Rows.Count > 0 Then
        '    For cnt As Integer = 0 To dt.Rows.Count - 1
        '        comboShape.Items.Add(dt.Rows(cnt).Item("SHAPE").ToString())
        '    Next
        'End If

        strSql = "                     SELECT DISTINCT COLORGRADE FROM " & cnAdminDb & "..GURANTECARD ORDER BY COLORGRADE"
        objGPack.FillCombo(strSql, cmbColorGrade_Own, True, True)
        strSql = "                     SELECT DISTINCT CLARITYGRADE FROM " & cnAdminDb & "..GURANTECARD ORDER BY CLARITYGRADE"
        objGPack.FillCombo(strSql, cmbClarityGrade_Own, True, True)
        strSql = "                     SELECT DISTINCT TYPEOFFINISH FROM " & cnAdminDb & "..GURANTECARD ORDER BY TYPEOFFINISH"
        objGPack.FillCombo(strSql, cmbTypeOfFinish_Own, True, True)


        chkLstWt.Items.Clear()
        strSql = " SELECT WT,MM FROM " & cnAdminDb & "..GCARDWTVSMM ORDER BY WT,MM"
        dt = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            For cnt As Integer = 0 To dt.Rows.Count - 1
                chkLstWt.Items.Add(dt.Rows(cnt).Item("WT").ToString)
            Next
        End If

        If TagNoEnb = False Then
            cmbTagNo.Enabled = False
        Else
            cmbTagNo.Enabled = True
        End If

        strSql = "                     SELECT DESCART,SHAPE,ESTWTEACH,ESTTWT,COLORGRADE,CLARITYGRADE,TYPEOFFINISH"
        strSql += "                     FROM " & cnAdminDb & "..GURANTECARD WHERE ISNULL(TAGNO,'') ='" & cmbTagNo.Text & "'"
        dt = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            txtDescription.Text = dt.Rows(0).Item("DESCART").ToString
            cmbShape_Own.Text = dt.Rows(0).Item("SHAPE").ToString
            Dim estWtEach As String = ""
            For cnt As Integer = 1 To dt.Rows(0).Item("ESTWTEACH").ToString.Length
                If Mid(dt.Rows(0).Item("ESTWTEACH").ToString, cnt, 1) = "," Then
                    chkLstWt.SetItemChecked(chkLstWt.Items.IndexOf(estWtEach), True)
                    chkLstWt_SelectedValueChanged(Me, New EventArgs)
                    estWtEach = ""
                Else
                    estWtEach += Mid(dt.Rows(0).Item("ESTWTEACH").ToString, cnt, 1)
                    If cnt = dt.Rows(0).Item("ESTWTEACH").ToString.Length Then
                        chkLstWt.SetItemChecked(chkLstWt.Items.IndexOf(estWtEach), True)
                        chkLstWt_SelectedValueChanged(Me, New EventArgs)
                    End If
                End If
            Next
            txtTotWt.Text = dt.Rows(0).Item("ESTTWT").ToString
            cmbClarityGrade_Own.Text = dt.Rows(0).Item("CLARITYGRADE").ToString
            cmbColorGrade_Own.Text = dt.Rows(0).Item("COLORGRADE").ToString
            cmbTypeOfFinish_Own.Text = dt.Rows(0).Item("TYPEOFFINISH").ToString
            strSql = " DECLARE @DEFPATH VARCHAR(200)"
            strSql += " SELECT @DEFPATH = '" & defaultPic & "'"
            strSql += " SELECT @DEFPATH+PCTFILE AS PICFILENAME FROM " & cnAdminDb & "..ITEMTAG WHERE ISNULL(TAGNO,'') ='" & cmbTagNo.Text & "'"
            dt = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If dt.Rows(0).Item("PICFILENAME").ToString = "" Then
                    picTag.Image = My.Resources.no_photo
                Else
                    Dim Finfo As IO.FileInfo
                    Finfo = New IO.FileInfo(dt.Rows(0).Item("PICFILENAME").ToString)
                    'Finfo.IsReadOnly = False
                    Dim fStream As New IO.FileStream(dt.Rows(0).Item("PICFILENAME").ToString, IO.FileMode.Open, FileAccess.Read)
                    picTag.Image = Image.FromStream(fStream)
                    fStream.Close()
                    'picTag.Image = Image.FromFile(dt.Rows(0).Item("PICFILENAME").ToString)
                End If
                cardImagePath = dt.Rows(0).Item("PICFILENAME").ToString
            End If
        Else
            strSql = " DECLARE @DEFPATH VARCHAR(200)"
            strSql += " SELECT @DEFPATH = '" & defaultPic & "'"
            strSql += "                     SELECT "
            strSql += "                     (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMID,0) = ISNULL(T.ITEMID,0)) ITEMNAME"
            strSql += "                     ,GRSWT"
            strSql += " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
            strSql += " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
            strSql += " ,@DEFPATH + PCTFILE AS PICFILENAME"
            strSql += "                     FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += "                     WHERE ISNULL(TAGNO,'') ='" & cmbTagNo.Text & "'"
            Dim dtView As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtView)
            If dtView.Rows.Count > 0 Then
                txtTotWt.Text = IIf(Val(dtView.Rows(0).Item("DIAWT").ToString) > 0, Val(dtView.Rows(0).Item("DIAWT").ToString), "")
                If dtView.Rows(0).Item("PICFILENAME").ToString = "" Then
                    picTag.Image = My.Resources.no_photo
                Else
                    Dim Finfo As IO.FileInfo
                    Finfo = New IO.FileInfo(dtView.Rows(0).Item("PICFILENAME").ToString)
                    'Finfo.IsReadOnly = False
                    Dim fStream As New IO.FileStream(dtView.Rows(0).Item("PICFILENAME").ToString, IO.FileMode.Open, FileAccess.Read)
                    picTag.Image = Image.FromStream(fStream)
                    fStream.Close()
                    'picTag.Image = Image.FromFile(dtView.Rows(0).Item("PICFILENAME").ToString)
                End If
                cardImagePath = dtView.Rows(0).Item("PICFILENAME").ToString
            End If
            txtDescription.Text = ""
            If cmbShape_Own.Items.Count > 0 Then
                cmbShape_Own.SelectedIndex = 0
            End If
            If cmbClarityGrade_Own.Items.Count > 0 Then
                cmbClarityGrade_Own.SelectedIndex = 0
            End If
            If cmbTypeOfFinish_Own.Items.Count > 0 Then
                cmbTypeOfFinish_Own.SelectedIndex = 0
            End If
        End If
    End Sub
    Private Sub funcSize()
        Try
            txtSize.Text = ""
            For cnt As Integer = 0 To chkLstWt.Items.Count - 1
                If chkLstWt.GetItemChecked(cnt) = True Then
                    strSql = " SELECT MM FROM " & cnAdminDb & "..GCARDWTVSMM WHERE WT = '" & chkLstWt.Items(cnt).ToString & "'"
                    Dim dtSize As New DataTable
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(dtSize)
                    If txtSize.Text <> "" Then
                        txtSize.Text = txtSize.Text & vbCrLf & dtSize.Rows(0).Item("mm").ToString()
                    Else
                        txtSize.Text = dtSize.Rows(0).Item("mm").ToString()
                    End If
                End If
            Next
        Catch ex As Exception
            MsgBox("Message :" + ex.Message + "StackTrace   :" + ex.StackTrace)
        End Try
    End Sub

    Private Sub frmCardDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            If txtDescription.Focused = True Then
                Exit Sub
            End If
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub chkLstWt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chkLstWt.KeyPress
        Try
            If e.KeyChar = Convert.ToChar(Keys.Space) Then
                funcSize()
            End If
        Catch ex As Exception
            MsgBox("Message :" + ex.Message + "StackTrace   :" + ex.StackTrace)
        End Try
    End Sub

    Private Sub chkLstWt_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstWt.SelectedValueChanged
        funcSize()
    End Sub

    Private Sub txtSize_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSize.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtTotWt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTotWt.GotFocus
        If cmbTagNo.Text <> "" Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub cmbTagNo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTagNo.LostFocus
        Try
            strSql = "                     SELECT DESCART,SHAPE,ESTWTEACH,ESTTWT,COLORGRADE,CLARITYGRADE,TYPEOFFINISH"
            strSql += "                     FROM " & cnAdminDb & "..GURANTECARD WHERE ISNULL(TAGNO,'') ='" & cmbTagNo.Text & "'"
            dt = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                txtDescription.Text = dt.Rows(0).Item("DESCART").ToString
                cmbShape_Own.Text = dt.Rows(0).Item("SHAPE").ToString
                Dim estWtEach As String = ""
                For cnt As Integer = 1 To dt.Rows(0).Item("ESTWTEACH").ToString.Length
                    If Mid(dt.Rows(0).Item("ESTWTEACH").ToString, cnt, 1) = "," Then
                        chkLstWt.SetItemChecked(chkLstWt.Items.IndexOf(estWtEach), True)
                        chkLstWt_SelectedValueChanged(Me, New EventArgs)
                        estWtEach = ""
                    Else
                        estWtEach += Mid(dt.Rows(0).Item("ESTWTEACH").ToString, cnt, 1)
                        If cnt = dt.Rows(0).Item("ESTWTEACH").ToString.Length Then
                            chkLstWt.SetItemChecked(chkLstWt.Items.IndexOf(estWtEach), True)
                            chkLstWt_SelectedValueChanged(Me, New EventArgs)
                        End If
                    End If
                Next
                txtTotWt.Text = dt.Rows(0).Item("ESTTWT").ToString
                cmbClarityGrade_Own.Text = dt.Rows(0).Item("CLARITYGRADE").ToString
                cmbColorGrade_Own.Text = dt.Rows(0).Item("COLORGRADE").ToString
                cmbTypeOfFinish_Own.Text = dt.Rows(0).Item("TYPEOFFINISH").ToString
                strSql = " SELECT PICFILENAME FROM " & cnAdminDb & "..ITEMTAG WHERE ISNULL(TAGNO,'') ='" & cmbTagNo.Text & "'"
                dt = New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    If dt.Rows(0).Item("PICFILENAME").ToString = "" Then
                        picTag.Image = My.Resources.no_photo
                    Else
                        picTag.Image = Image.FromFile(dt.Rows(0).Item("PICFILENAME").ToString)
                    End If
                    cardImagePath = dt.Rows(0).Item("PICFILENAME").ToString
                End If
            Else
                strSql = " DECLARE @DEFPATH VARCHAR(200)"
                strSql += " SELECT @DEFPATH = '" & defaultPic & "'"
                strSql += "                     SELECT "
                strSql += "                     (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMID,0) = ISNULL(T.ITEMID,0)) ITEMNAME"
                strSql += "                     ,GRSWT"
                strSql += " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
                strSql += " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
                strSql += " ,@DEFPATH + PCTFILE AS PICFILENAME"
                strSql += "                     FROM " & cnAdminDb & "..ITEMTAG AS T"
                strSql += "                     WHERE ISNULL(TAGNO,'') ='" & cmbTagNo.Text & "'"

                'strSql = "                     SELECT "
                'strSql += "                     (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMID,0) = ISNULL(T.ITEMID,0)) ITEMNAME"
                'strSql += "                     ,GRSWT,DIAPCS,DIAWT,PICFILENAME"
                'strSql += "                     FROM " & cnAdminDb & "..ITEMTAG AS T"
                'strSql += "                     WHERE ISNULL(TAGNO,'') ='" & cmbTagNo.Text & "'"
                Dim dtView As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtView)
                If dtView.Rows.Count > 0 Then
                    txtTotWt.Text = IIf(Val(dtView.Rows(0).Item("DIAWT").ToString) <> 0, Val(dtView.Rows(0).Item("DIAWT").ToString), "")
                    If dtView.Rows(0).Item("PICFILENAME").ToString = "" Or IO.File.Exists(dtView.Rows(0).Item("PICFILENAME").ToString) = False Then
                        picTag.Image = My.Resources.no_photo
                    Else
                        Dim Finfo As IO.FileInfo
                        Finfo = New IO.FileInfo(dtView.Rows(0).Item("PICFILENAME").ToString)
                        'Finfo.IsReadOnly = False
                        Dim fStream As New IO.FileStream(dtView.Rows(0).Item("PICFILENAME").ToString, IO.FileMode.Open, FileAccess.Read)
                        picTag.Image = Image.FromStream(fStream)
                        fStream.Close()
                        'picTag.Image = Image.FromFile(dtView.Rows(0).Item("PICFILENAME").ToString)
                    End If
                    cardImagePath = dtView.Rows(0).Item("PICFILENAME").ToString
                End If
                txtDescription.Text = ""
                If cmbShape_Own.Items.Count > 0 Then
                    cmbShape_Own.SelectedIndex = 0
                End If
                If cmbClarityGrade_Own.Items.Count > 0 Then
                    cmbClarityGrade_Own.SelectedIndex = 0
                End If
                If cmbTypeOfFinish_Own.Items.Count > 0 Then
                    cmbTypeOfFinish_Own.SelectedIndex = 0
                End If
            End If
        Catch ex As Exception
            MsgBox("Message :" + ex.Message + "StackTrace   :" + ex.StackTrace)
        End Try
    End Sub

    Private Sub btnBrowse_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        Dim openDia As New OpenFileDialog
        Dim str As String
        str = "JPEG(*.jpg)|*.jpg"
        str += "|Bitmap(*.bmp)|*.bmp"
        str += "|GIF(*.gif)|*.gif"
        str += "|All Files(*.*)|*.*"
        openDia.Filter = str
        If openDia.ShowDialog = Windows.Forms.DialogResult.OK Then
            'picModel.Visible = True
            picTag.Image = Image.FromFile(openDia.FileName)
            cardImagePath = openDia.FileName
            btnPrint.Select()
        End If
    End Sub

    Private Sub btnBrowse_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btnBrowse.KeyDown
        If e.KeyCode = Keys.Escape Then
            btnPrint.Focus()
        End If
    End Sub
End Class