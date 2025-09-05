Imports System.Drawing.Printing
Imports System.Data.OleDb
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Drawing.Graphics
Public Class frmChequePrint
#Region "TextBox Move Variable Only"
    Public txt As TextBox
    Private txtptX As Integer
    Private txtptY As Integer
    Private txtdrag As Boolean
    Private txtresize As Boolean
    Dim nowDragging As Boolean = True
    Dim flag As Boolean = True
    Dim Saveflag As Boolean = True
    Public activeTextBox As TextBox
    Public activeTextbox_Name, activeTextbox_Width, activeTextbox_Height As String
    Public activeTextbox_X, activeTextbox_Y As String
#End Region

#Region "Move TextBox Value Store"
    Public txt_acpayee_width, txt_acpayee_name, txt_acpayee_x, txt_acpayee_y As String

    Public txt_payee1_width, txt_payee1_name, txt_payee1_x, txt_payee1_y As String
    Public txt_payee2_width, txt_payee2_name, txt_payee2_x, txt_payee2_y As String

    Public txt_textline1_width, txt_textline1_name, txt_textline1_x, txt_textline1_y As String
    Public txt_textline2_width, txt_textline2_name, txt_textline2_x, txt_textline2_y As String
    Public txt_textline3_width, txt_textline3_name, txt_textline3_x, txt_textline3_y As String

    Public txt_amtword1_width, txt_amtword1_name, txt_amtword1_x, txt_amtword1_y As String
    Public txt_amtword2_width, txt_amtword2_name, txt_amtword2_x, txt_amtword2_y As String

    Public txt_date_width, txt_date_name, txt_date_x, txt_date_y As String
    Public txt_amt_width, txt_amt_name, txt_amt_x, txt_amt_y As String
    Public txt_bearer_width, txt_bearer_name, txt_bearer_x, txt_bearer_y As String
    Public txt_notaboveamt_width, txt_notaboveamt_name, txt_notaboveamt_x, txt_notaboveamt_y As String

#End Region

#Region "OLEDB Variable"
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim tran As OleDbTransaction
    Dim dt As DataTable
    Dim strSql As String
    Dim updFlag As Boolean
    
#End Region

#Region "Class Obect"
    Dim objChecked As New checkboxValueReturn
    Dim objFont As New font_name_size
    Dim objPicture As New pictureboxvaluereturn
    Dim obj_curfontsize As New textboxactive_name_x_y_width
#End Region

#Region "Font Variable"
    Public acpayee_font, acpayee_size, acpayee_style As String
    Public txtpayee1_font, txtpayee1_size, txtpayee1_style As String
    Public txtpayee2_font, txtpayee2_size, txtpayee2_style As String
    Public txtamt1_font, txtamt1_size, txtamt1_style As String
    Public txtamt2_font, txtamt2_size, txtamt2_style As String
    Public txtline1_font, txtline1_size, txtline1_style As String
    Public txtline2_font, txtline2_size, txtline2_style As String
    Public txtline3_font, txtline3_size, txtline3_style As String
    Public notabove_font, notabove_size, notabove_style As String
    Public txtdate_font, txtdate_size, txtdate_style As String
    Public txtamt_font, txtamt_size, txtamt_style As String
    Public txtbearer_font, txtbearer_size, txtbearer_style As String
#End Region

#Region "Form Event"
    Private Sub frmChequePrint_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Escape) Then
            If tabMain.SelectedTab.Name = tabView.Name Then

                btnBack_Click(Me, New EventArgs)
            End If
        ElseIf e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmChequePrint_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        chkbxDateFormat.Items.Clear()
        chkbxDateFormat.Items.Add("dd MM YYYY")
        chkbxDateFormat.Items.Add("dd/MM/YYYY")
        chkbxDateFormat.Items.Add("dd-MM-YYYY")
        chkbxDateFormat.Items.Add("dd-MM-YY")

        cmbxDateSpace.Items.Clear()
        cmbxDateSpace.Items.Add("1")
        cmbxDateSpace.Items.Add("2")
        cmbxDateSpace.Items.Add("3")
        cmbxDateSpace.Items.Add("4")
        cmbxDateSpace.Items.Add("5")
        cmbxDateSpace.Items.Add("6")

        cmbxAcname.Items.Clear()
        strSql = "SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE = 'B'"
        objGPack.FillCombo(strSql, cmbxAcname, False)

        cmbxAccode.Items.Clear()
        strSql = "SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE = 'B'"
        objGPack.FillCombo(strSql, cmbxAccode, False)
    End Sub
#End Region

#Region "TextBox Event"
    Private Sub txt_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txt_acpayee.MouseClick, txt_payee1.MouseClick, txt_payee2.MouseClick, txt_textline1.MouseClick, txt_textline2.MouseClick, _
    txt_textline3.MouseClick, txt_amtword1.MouseClick, txt_amtword2.MouseClick, txt_date.MouseClick, txt_amt.MouseClick, txt_bearer.MouseClick, txt_notaboveamt.MouseClick
        activeTextBox = CType(sender, TextBox)
    End Sub

    Private Sub txt_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txt_acpayee.MouseDoubleClick, txt_payee1.MouseDoubleClick, _
    txt_payee2.MouseDoubleClick, txt_textline1.MouseDoubleClick, txt_textline2.MouseDoubleClick, txt_textline3.MouseDoubleClick, txt_amtword1.MouseDoubleClick, txt_amtword2.MouseDoubleClick, _
    txt_date.MouseDoubleClick, txt_amt.MouseDoubleClick, txt_bearer.MouseDoubleClick, txt_notaboveamt.MouseDoubleClick
        nowDragging = Not nowDragging
        'Newly
        'txt = CType(sender, TextBox)
        '
        'activeTextBox = CType(sender, TextBox)
        If (flag = False) Then
            'TextBox6.BackColor = Color.Purple
            'activeTextBox.BackColor = Color.Green
            flag = True
        Else
            'TextBox6.BackColor = Color.Red
            'activeTextBox.BackColor = Color.Red
            flag = False
        End If
    End Sub

    Private Sub txt_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txt_acpayee.MouseDown, txt_payee1.MouseDown, _
    txt_payee2.MouseDown, txt_textline1.MouseDown, txt_textline2.MouseDown, txt_textline3.MouseDown, txt_amtword1.MouseDown, txt_amtword2.MouseDown, _
    txt_date.MouseDown, txt_amt.MouseDown, txt_bearer.MouseDown, txt_notaboveamt.MouseDown
        txtdrag = True
        txtresize = True
        'txt = CType(sender, TextBox)
        'activeTextBox = CType(sender, TextBox)
        txtptX = e.X : txtptY = e.Y
    End Sub

    Private Sub txt_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_acpayee.MouseLeave, txt_payee1.MouseLeave, txt_payee2.MouseLeave, txt_textline1.MouseLeave, txt_textline2.MouseLeave, _
    txt_textline3.MouseLeave, txt_amtword1.MouseLeave, txt_amtword2.MouseLeave, txt_date.MouseLeave, txt_amt.MouseLeave, txt_bearer.MouseLeave, txt_notaboveamt.MouseLeave
        'textbox stop postion
        'Newly
        'txt = CType(sender, TextBox)
        '
        'activeTextBox = CType(sender, TextBox)
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub txt_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txt_acpayee.MouseMove, txt_payee1.MouseMove, _
    txt_payee2.MouseMove, txt_textline1.MouseMove, txt_textline2.MouseMove, txt_textline3.MouseMove, txt_amtword1.MouseMove, txt_amtword2.MouseMove, _
    txt_date.MouseMove, txt_amt.MouseMove, txt_bearer.MouseMove, txt_notaboveamt.MouseMove

        txt = CType(sender, TextBox)

        If txtdrag = True And nowDragging Then
            If (txt.Location.X + e.X - txtptX >= 0 And txt.Location.X + e.X - txtptX <= 700 And txt.Location.Y + e.Y - txtptY > 1 And txt.Location.Y + e.Y - txtptY < 305) Then
                txt.Location = New Point(txt.Location.X + e.X - txtptX, txt.Location.Y + e.Y - txtptY)
                '''''''X & Y axis
                txtxaxis.Text = txt.Location.X.ToString
                txtyaxis.Text = txt.Location.Y.ToString
            End If
            txt.ForeColor = Color.Black

            '---------------'Database Store'----------------------

            activeTextbox_Name = txt.Name.ToString
            activeTextbox_X = txt.Location.X.ToString
            activeTextbox_Y = txt.Location.Y.ToString

            '-----------------------------------------------------
            txt_acpayee_name = txt_acpayee.Name.ToString
            txt_acpayee_width = txt_acpayee.Width.ToString
            txt_acpayee_x = txt_acpayee.Location.X.ToString
            txt_acpayee_y = txt_acpayee.Location.Y.ToString
            '-----------------------------------------------------------

            txt_payee1_width = txt_payee1.Width.ToString
            txt_payee1_name = txt_payee1.Name.ToString
            txt_payee1_x = txt_payee1.Location.X.ToString
            txt_payee1_y = txt_payee1.Location.Y.ToString

            '-----------------------------------------------

            txt_payee2_width = txt_payee2.Width.ToString
            txt_payee2_name = txt_payee2.Name.ToString
            txt_payee2_x = txt_payee2.Location.X.ToString
            txt_payee2_y = txt_payee2.Location.Y.ToString

            '-----------------------------

            txt_textline1_width = txt_textline1.Width.ToString
            txt_textline1_name = txt_textline1.Name.ToString
            txt_textline1_x = txt_textline1.Location.X.ToString
            txt_textline1_y = txt_textline1.Location.Y.ToString

            '------------------------


            txt_textline2_width = txt_textline2.Width.ToString
            txt_textline2_name = txt_textline2.Name.ToString
            txt_textline2_x = txt_textline2.Location.X.ToString
            txt_textline2_y = txt_textline2.Location.Y.ToString

            '-----------------------

            txt_textline3_width = txt_textline3.Width.ToString
            txt_textline3_name = txt_textline3.Name.ToString
            txt_textline3_x = txt_textline3.Location.X.ToString
            txt_textline3_y = txt_textline3.Location.Y.ToString

            '---------------------
            txt_amtword1_width = txt_amtword1.Width.ToString
            txt_amtword1_name = txt_amtword1.Name.ToString
            txt_amtword1_x = txt_amtword1.Location.X.ToString
            txt_amtword1_y = txt_amtword1.Location.Y.ToString

            '-----------------

            txt_amtword2_width = txt_amtword2.Width.ToString
            txt_amtword2_name = txt_amtword2.Name.ToString
            txt_amtword2_x = txt_amtword2.Location.X.ToString
            txt_amtword2_y = txt_amtword2.Location.Y.ToString

            '--------------


            txt_date_width = txt_date.Width.ToString
            txt_date_name = txt_date.Name.ToString
            txt_date_x = txt_date.Location.X.ToString
            txt_date_y = txt_date.Location.Y.ToString

            '------------------
            txt_amt_width = txt_amt.Width.ToString
            txt_amt_name = txt_amt.Name.ToString
            txt_amt_x = txt_amt.Location.X.ToString
            txt_amt_y = txt_amt.Location.Y.ToString


            '--------------------------

            txt_bearer_width = txt_bearer.Width.ToString
            txt_bearer_name = txt_bearer.Name.ToString
            txt_bearer_x = txt_bearer.Location.X.ToString
            txt_bearer_y = txt_bearer.Location.Y.ToString

            '-----------------
            txt_notaboveamt_width = txt_notaboveamt.Width.ToString
            txt_notaboveamt_name = txt_notaboveamt.Name.ToString
            txt_notaboveamt_x = txt_notaboveamt.Location.X.ToString
            txt_notaboveamt_y = txt_notaboveamt.Location.Y.ToString

            '------------------------------------

            Me.Refresh()
            txtdrag = True
        End If

        'Second If Loop 
        If txtresize = True And Not nowDragging Then
            txtdrag = False
            If txt.Cursor = Cursors.SizeAll Then

                ' Adjusted textbox resize & Width and Height Display
                If (e.X >= 20 And e.X <= 10000) Then

                    txt.Width = e.X
                    txt.Height = e.Y
                    txtwidth.Text = "" + txt.Width.ToString

                    activeTextbox_Width = txt.Width.ToString
                    activeTextbox_Height = txt.Height.ToString

                End If
                'Dim findactivebox As String = Me.ActiveControl.Name
                'MessageBox.Show(findactivebox)
                ''---------- Important----------
            Else
                If e.X >= txt.Width - 10 Then
                    txt.Cursor = Cursors.SizeAll
                Else
                    txt.Cursor = Cursors.IBeam
                End If

                If e.Y >= txt.Height - 10 Then
                    txt.Cursor = Cursors.SizeAll

                Else
                    txt.Cursor = Cursors.IBeam
                End If
            End If
        End If
    End Sub

    Private Sub txt_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txt_acpayee.MouseUp, _
    txt_payee1.MouseUp, txt_payee2.MouseUp, txt_textline1.MouseUp, txt_textline2.MouseUp, txt_textline3.MouseUp, txt_amtword1.MouseUp, txt_amtword2.MouseUp, _
    txt_date.MouseUp, txt_amt.MouseUp, txt_bearer.MouseUp, txt_notaboveamt.MouseUp
        txt = CType(sender, TextBox)
        'activeTextBox = CType(sender, TextBox)
        If txt.Cursor = Cursors.SizeAll Then
            txt.Cursor = Cursors.IBeam
        End If
        txtdrag = False
        txtresize = False

    End Sub

#End Region


#Region "Button Event"
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click

        If chkbxpayeeline1.Checked = False And chkbxamt.Checked = False And chkbxdate.Checked = False And chkbxamtwords1.Checked = False Then
            MsgBox("PayeeName, Amount, AmountName Compulsory Select", MsgBoxStyle.Information)
            Exit Sub
        End If

        dt = New DataTable
        Dim layoutId As Integer
        Dim ImagePath As String = objPicture.Display_path
        Dim ImageLength As Integer

        If ImagePath = Nothing Then
            ImageLength = 0
        Else
            ImageLength = ImagePath.Length
        End If


        strSql = "SELECT ISNULL(MAX(LAYOUTID),0)+1 FROM " & cnAdminDb & "..CHQLAYOUT"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            layoutId = Val(dt.Rows(0).Item(0).ToString)
        End If

        If cmbxAcname.Text.ToUpper.Trim <> "" Then
            If Saveflag = True Then
                'INSERT QUERY
                Dim check As String = objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..CHQLAYOUT WHERE LAYOUTNAME = '" & cmbxAcname.Text.ToUpper.Trim & "'")
                If check <> "CHECK" Then
                    'Don't Used Trans
                    'tran = cn.BeginTransaction
                    Try
                        strSql = "INSERT INTO " & cnAdminDb & "..CHQLAYOUT"
                        strSql += " (LAYOUTID,LAYOUTNAME,CHQDATEFORMAT"
                        strSql += " ,IMAGEPATH,CHQDATESPACE,ACCODE)"
                        strSql += " VALUES"
                        strSql += " ( "
                        strSql += " " & layoutId ' layoutId
                        strSql += " , '" & cmbxAcname.Text.ToUpper.Trim & "'" 'LayoutName
                        strSql += " ,'" & txt_date.Text & "'" 'DateFormat
                        strSql += " ,''" 'Image Format
                        strSql += " ,'" & cmbxDateSpace.Text & "'"
                        strSql += " ,'" & cmbxAccode.Text & "'" 'Accode
                        strSql += " )"
                        cmd = New OleDbCommand(strSql, cn)
                        cmd.ExecuteNonQuery()

                        Try
                            If ImageLength > 0 Then
                                Dim ImageTemp As Byte() = ConvertImageFiletoBytes(objPicture.Display_path)
                                strSql = "UPDATE " & cnAdminDb & "..CHQLAYOUT SET IMAGEPATH = ? "
                                strSql += " WHERE  LAYOUTNAME = '" & cmbxAcname.Text.ToUpper.Trim & "'"
                                cmd = New OleDbCommand(strSql, cn)
                                'Dim picture As Image = Image.FromFile(objPicture.Display_path)
                                'Dim stream As New IO.MemoryStream
                                'picture.Save(stream, Imaging.ImageFormat.Jpeg)
                                cmd.Parameters.AddWithValue("@IMAGEPATH", ImageTemp)
                                'cmd.Parameters.Add("@IMAGEPATH", OleDbType.VarBinary).Value = ImageTemp
                                cmd.ExecuteNonQuery()
                            End If
                        Catch ex As Exception
                            MessageBox.Show(ex.ToString)
                        Finally
                            '-------------------------------------
                            If (objChecked.outputacpayee = True) Then
                                'cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE)VALUES( " & LAYOUTID & ",'" & txt_acpayee_name & "', " & CInt(txt_acpayee_x) & ", " & CInt(txt_acpayee_y) & ", " & CInt(txt_acpayee_width) & ", '" & txtfontname.Text & "', '" & txtfontsize.Text & "', 'Y')", cn)

                                If (acpayee_style <> "") Then
                                    'MsgBox("Already")
                                    cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE,FONTSTYLE)VALUES( " & layoutId & ",'txt_acpayee', " & CInt(txt_acpayee_x) & ", " & CInt(txt_acpayee_y) & ", " & CInt(txt_acpayee_width) & ", '" & acpayee_font & "', '" & acpayee_size & "', 'Y', '" & acpayee_style & "')", cn)
                                    cmd.ExecuteNonQuery()
                                Else
                                    'MsgBox("me")
                                    'Regular Convert to Integer
                                    cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE,FONTSTYLE)VALUES( " & layoutId & ",'txt_acpayee', " & CInt(txt_acpayee_x) & ", " & CInt(txt_acpayee_y) & ", " & CInt(txt_acpayee_width) & ", 'Verdana', '8', 'Y', '0')", cn)
                                    cmd.ExecuteNonQuery()
                                End If

                            Else
                                'WITHOUT TOUCH 
                                cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE,FONTSTYLE)VALUES(" & layoutId & ",'txt_acpayee',9,10,0,'Verdana','8', 'N', '0')", cn)
                                cmd.ExecuteNonQuery()
                            End If

                            '-------------------------------------

                            If (objChecked.outputpayeeline1 = True) Then
                                'cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE)VALUES(" & LAYOUTID & ",'" & txt_payee1_name & "', " & CInt(txt_payee1_x) & ", " & CInt(txt_payee1_y) & ", " & CInt(txt_payee1_width) & ", '" & txtfontname.Text & "', '" & txtfontsize.Text & "', 'Y')", cn)

                                If (txtpayee1_style <> "") Then
                                    'MsgBox("Already")
                                    cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE,FONTSTYLE)VALUES(" & layoutId & ",'txt_payee1', " & CInt(txt_payee1_x) & ", " & CInt(txt_payee1_y) & ", " & CInt(txt_payee1_width) & ", '" & txtpayee1_font & "', '" & txtpayee1_size & "', 'Y', '" & txtpayee1_style & "')", cn)
                                    cmd.ExecuteNonQuery()
                                Else
                                    'MsgBox("me")
                                    cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE,FONTSTYLE)VALUES(" & layoutId & ",'txt_payee1', " & CInt(txt_payee1_x) & ", " & CInt(txt_payee1_y) & ", " & CInt(txt_payee1_width) & ", 'Verdana', '8', 'Y', '0')", cn)
                                    cmd.ExecuteNonQuery()
                                End If

                            Else
                                'cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE)VALUES(1,'" & txt_payee1_name & "', " & CInt(txt_payee1_x) & ", " & CInt(txt_payee1_y) & ", " & CInt(txt_payee1_width) & ", '" & txtfontname.Text & "', '" & txtfontsize.Text & "', 'N')", cn)
                                cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE,FONTSTYLE)VALUES(" & layoutId & ",'txt_payee1',101,93,0,'Verdana','8', 'N', '0')", cn)
                                cmd.ExecuteNonQuery()
                            End If

                            '-------------------------------------


                            If (objChecked.outputpayeeline2 = True) Then
                                'cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE)VALUES(" & LAYOUTID & ",'" & txt_payee2_name & "', " & CInt(txt_payee2_x) & ", " & CInt(txt_payee2_y) & ", " & CInt(txt_payee2_width) & ", '" & txtfontname.Text & "', '" & txtfontsize.Text & "', 'Y')", cn)
                                If (txtpayee2_style <> "") Then
                                    'MsgBox("Already")
                                    cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE,FONTSTYLE)VALUES(" & layoutId & ",'txt_payee2', " & CInt(txt_payee2_x) & ", " & CInt(txt_payee2_y) & ", " & CInt(txt_payee2_width) & ", '" & txtpayee2_font & "', '" & txtpayee2_size & "', 'Y', '" & txtpayee2_style & "')", cn)
                                    cmd.ExecuteNonQuery()
                                Else
                                    'MsgBox("me")
                                    cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE,FONTSTYLE)VALUES(" & layoutId & ",'txt_payee2', " & CInt(txt_payee2_x) & ", " & CInt(txt_payee2_y) & ", " & CInt(txt_payee2_width) & ", 'Verdana', '8', 'Y', '0')", cn)
                                    cmd.ExecuteNonQuery()
                                End If

                            Else
                                'cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE)VALUES(1,'" & txt_payee2_name & "', " & CInt(txt_payee2_x) & ", " & CInt(txt_payee2_y) & ", " & CInt(txt_payee2_width) & ", '" & txtfontname.Text & "', '" & txtfontsize.Text & "', 'N')", cn)
                                cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE,FONTSTYLE)VALUES(" & layoutId & ",'txt_payee2',101,177,0,'Verdana','8', 'N', '0')", cn)
                                cmd.ExecuteNonQuery()
                            End If

                            '-------------------------------------

                            If (objChecked.outputamtwords1 = True) Then
                                'cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE)VALUES(" & LAYOUTID & ",'" & txt_amtword1_name & "', " & CInt(txt_amtword1_x) & ", " & CInt(txt_amtword1_y) & ", " & CInt(txt_amtword1_width) & ", '" & txtfontname.Text & "', '" & txtfontsize.Text & "', 'Y')", cn)

                                If (txtamt1_style <> "") Then
                                    'MsgBox("Already")
                                    cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE,FONTSTYLE)VALUES(" & layoutId & ",'txt_amtword1', " & CInt(txt_amtword1_x) & ", " & CInt(txt_amtword1_y) & ", " & CInt(txt_amtword1_width) & ", '" & txtamt1_font & "', '" & txtamt1_size & "', 'Y', '" & txtamt1_style & "')", cn)
                                    cmd.ExecuteNonQuery()
                                Else
                                    'MsgBox("me")
                                    cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE,FONTSTYLE)VALUES(" & layoutId & ",'txt_amtword1', " & CInt(txt_amtword1_x) & ", " & CInt(txt_amtword1_y) & ", " & CInt(txt_amtword1_width) & ", 'Verdana', '8', 'Y', '0')", cn)
                                    cmd.ExecuteNonQuery()
                                End If

                            Else
                                'cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE)VALUES(1,'" & txt_amtword1_name & "', " & CInt(txt_amtword1_x) & ", " & CInt(txt_amtword1_y) & ", " & CInt(txt_amtword1_width) & ", '" & txtfontname.Text & "', '" & txtfontsize.Text & "', 'N')", cn)
                                cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE,FONTSTYLE)VALUES(" & layoutId & ",'txt_amtword1',58,137,0,'Verdana','8', 'N', '0')", cn)
                                cmd.ExecuteNonQuery()
                            End If

                            '-------------------------------------

                            If (objChecked.outputamtwords2 = True) Then
                                'cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE)VALUES(" & LAYOUTID & ",'" & txt_amtword2_name & "', " & CInt(txt_amtword2_x) & ", " & CInt(txt_amtword2_y) & ", " & CInt(txt_amtword2_width) & ", '" & txtfontname.Text & "', '" & txtfontsize.Text & "', 'Y')", cn)

                                If (txtamt2_style <> "") Then
                                    'MsgBox("Already")
                                    cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE,FONTSTYLE)VALUES(" & layoutId & ",'txt_amtword2', " & CInt(txt_amtword2_x) & ", " & CInt(txt_amtword2_y) & ", " & CInt(txt_amtword2_width) & ", '" & txtamt2_font & "', '" & txtamt2_size & "', 'Y', '" & txtamt2_style & "')", cn)
                                    cmd.ExecuteNonQuery()
                                Else
                                    'MsgBox("me")
                                    cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE,FONTSTYLE)VALUES(" & layoutId & ",'txt_amtword2', " & CInt(txt_amtword2_x) & ", " & CInt(txt_amtword2_y) & ", " & CInt(txt_amtword2_width) & ", 'Verdana', '8', 'Y', '0')", cn)
                                    cmd.ExecuteNonQuery()
                                End If

                            Else
                                'cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE)VALUES(1,'" & txt_amtword2_name & "', " & CInt(txt_amtword2_x) & ", " & CInt(txt_amtword2_y) & ", " & CInt(txt_amtword2_width) & ", '" & txtfontname.Text & "', '" & txtfontsize.Text & "', 'N')", cn)
                                cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE,FONTSTYLE)VALUES(" & layoutId & ",'txt_amtword2',58,220,0,'Verdana','8', 'N', '0')", cn)
                                cmd.ExecuteNonQuery()
                            End If

                            '-------------------------------------
                            If (objChecked.outputtxtline1 = True) Then
                                'cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE)VALUES(" & LAYOUTID & ",'" & txt_textline1_name & "', " & CInt(txt_textline1_x) & ", " & CInt(txt_textline1_y) & ", " & CInt(txt_textline1_width) & ", '" & txtfontname.Text & "', '" & txtfontsize.Text & "', 'Y')", cn)
                                If (txtline1_style <> "") Then
                                    'MsgBox("Already")
                                    cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE,FONTSTYLE)VALUES(" & layoutId & ",'txt_textline1', " & CInt(txt_textline1_x) & ", " & CInt(txt_textline1_y) & ", " & CInt(txt_textline1_width) & ", '" & txtline1_font & "', '" & txtline1_size & "', 'Y', '" & txtline1_style & "')", cn)
                                    cmd.ExecuteNonQuery()
                                Else
                                    'MsgBox("me")
                                    cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE,FONTSTYLE)VALUES(" & layoutId & ",'txt_textline1', " & CInt(txt_textline1_x) & ", " & CInt(txt_textline1_y) & ", " & CInt(txt_textline1_width) & ", 'Verdana', '8', 'Y', '0')", cn)
                                    cmd.ExecuteNonQuery()
                                End If

                            Else
                                'cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE)VALUES(1,'" & txt_textline1_name & "', " & CInt(txt_textline1_x) & ", " & CInt(txt_textline1_y) & ", " & CInt(txt_textline1_width) & ", '" & txtfontname.Text & "', '" & txtfontsize.Text & "', 'N')", cn)
                                cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE,FONTSTYLE)VALUES(" & layoutId & ",'txt_textline1',393,57,0,'Verdana','8', 'N','0')", cn)
                                cmd.ExecuteNonQuery()
                            End If


                            '-------------------------------------

                            If (objChecked.outputtxtline2 = True) Then
                                'cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE)VALUES(" & LAYOUTID & ",'" & txt_textline2_name & "', " & CInt(txt_textline2_x) & ", " & CInt(txt_textline2_y) & ", " & CInt(txt_textline2_width) & ", '" & txtfontname.Text & "', '" & txtfontsize.Text & "', 'Y')", cn)
                                If (txtline2_style <> "") Then
                                    'MsgBox("Already")
                                    cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE,FONTSTYLE)VALUES(" & layoutId & ",'txt_textline2', " & CInt(txt_textline2_x) & ", " & CInt(txt_textline2_y) & ", " & CInt(txt_textline2_width) & ", '" & txtline2_font & "', '" & txtline2_size & "', 'Y', '" & txtline2_style & "')", cn)
                                    cmd.ExecuteNonQuery()
                                Else
                                    'MsgBox("me")
                                    cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE,FONTSTYLE)VALUES(" & layoutId & ",'txt_textline2', " & CInt(txt_textline2_x) & ", " & CInt(txt_textline2_y) & ", " & CInt(txt_textline2_width) & ", 'Verdana', '8', 'Y', '0')", cn)
                                    cmd.ExecuteNonQuery()
                                End If


                            Else
                                ' cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE)VALUES(1,'" & txt_textline2_name & "', " & CInt(txt_textline2_x) & ", " & CInt(txt_textline2_y) & ", " & CInt(txt_textline2_width) & ", '" & txtfontname.Text & "', '" & txtfontsize.Text & "', 'N')", cn)
                                cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE,FONTSTYLE)VALUES(" & layoutId & ",'txt_textline2',393,104,0,'Verdana','8', 'N', '0')", cn)
                                cmd.ExecuteNonQuery()
                            End If

                            '-------------------------------------

                            If (objChecked.outputtxtline3 = True) Then
                                'cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE)VALUES(" & LAYOUTID & ",'" & txt_textline3_name & "', " & CInt(txt_textline3_x) & ", " & CInt(txt_textline3_y) & ", " & CInt(txt_textline3_width) & ", '" & txtfontname.Text & "', '" & txtfontsize.Text & "', 'Y')", cn)
                                If (txtline3_style <> "") Then
                                    'MsgBox("Already")
                                    cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE,FONTSTYLE)VALUES(" & layoutId & ",'txt_textline3', " & CInt(txt_textline3_x) & ", " & CInt(txt_textline3_y) & ", " & CInt(txt_textline3_width) & ", '" & txtline3_font & "', '" & txtline3_size & "', 'Y', '" & txtline3_style & "')", cn)
                                    cmd.ExecuteNonQuery()
                                Else
                                    'MsgBox("me")
                                    cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE,FONTSTYLE)VALUES(" & layoutId & ",'txt_textline3', " & CInt(txt_textline3_x) & ", " & CInt(txt_textline3_y) & ", " & CInt(txt_textline3_width) & ", 'Verdana', '8', 'Y', '0')", cn)
                                    cmd.ExecuteNonQuery()
                                End If

                            Else
                                '    cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE)VALUES(1,'" & txt_textline3_name & "', " & CInt(txt_textline3_x) & ", " & CInt(txt_textline3_y) & ", " & CInt(txt_textline3_width) & ", '" & txtfontname.Text & "', '" & txtfontsize.Text & "', 'N')", cn)
                                cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE,FONTSTYLE)VALUES(" & layoutId & ",'txt_textline3',393,151,0,'Verdana','8', 'N', '0')", cn)
                                cmd.ExecuteNonQuery()

                            End If

                            '-------------------------------------

                            If (objChecked.outputnotabove = True) Then
                                'cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE)VALUES(" & LAYOUTID & ",'" & txt_notaboveamt_name & "', " & CInt(txt_notaboveamt_x) & ", " & CInt(txt_notaboveamt_y) & ", " & CInt(txt_notaboveamt_width) & ", '" & txtfontname.Text & "', '" & txtfontsize.Text & "', 'Y')", cn)
                                If (notabove_style <> "") Then
                                    'MsgBox("Already")
                                    cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE,FONTSTYLE)VALUES(" & layoutId & ",'txt_notaboveamt', " & CInt(txt_notaboveamt_x) & ", " & CInt(txt_notaboveamt_y) & ", " & CInt(txt_notaboveamt_width) & ", '" & notabove_font & "', '" & notabove_size & "', 'Y', '" & notabove_style & "')", cn)
                                    cmd.ExecuteNonQuery()
                                Else
                                    'MsgBox("me")
                                    cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE,FONTSTYLE)VALUES(" & layoutId & ",'txt_notaboveamt', " & CInt(txt_notaboveamt_x) & ", " & CInt(txt_notaboveamt_y) & ", " & CInt(txt_notaboveamt_width) & ", 'Verdana', '8', 'Y', '0')", cn)
                                    cmd.ExecuteNonQuery()
                                End If
                            Else
                                'cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE)VALUES(1,'" & txt_notaboveamt_name & "', " & CInt(txt_notaboveamt_x) & ", " & CInt(txt_notaboveamt_y) & ", " & CInt(txt_notaboveamt_width) & ", '" & txtfontname.Text & "', '" & txtfontsize.Text & "', 'N')", cn)
                                cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE,FONTSTYLE)VALUES(" & layoutId & ",'txt_notaboveamt',363,305,0,'Verdana','8', 'N', '0')", cn)
                                cmd.ExecuteNonQuery()

                            End If

                            '-------------------------------------

                            If (objChecked.outputdt_tx = True) Then

                                'cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE)VALUES(" & LAYOUTID & ",'" & txt_date_name & "', " & CInt(txt_date_x) & ", " & CInt(txt_date_y) & ", " & CInt(txt_date_width) & ", '" & txtfontname.Text & "', '" & txtfontsize.Text & "', 'Y')", cn)
                                If (txtdate_style <> "") Then
                                    'MsgBox("Already")
                                    cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE,FONTSTYLE)VALUES(" & layoutId & ",'txt_date', " & CInt(txt_date_x) & ", " & CInt(txt_date_y) & ", " & CInt(txt_date_width) & ", '" & txtdate_font & "', '" & txtdate_size & "', 'Y', '" & txtdate_style & "')", cn)
                                    cmd.ExecuteNonQuery()
                                Else
                                    'MsgBox("me")
                                    cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE,FONTSTYLE)VALUES(" & layoutId & ",'txt_date', " & CInt(txt_date_x) & ", " & CInt(txt_date_y) & ", " & CInt(txt_date_width) & ", 'Verdana', '8', 'Y', '0')", cn)
                                    cmd.ExecuteNonQuery()
                                End If

                            Else
                                '    cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE)VALUES(1,'" & txt_date_name & "', " & CInt(txt_date_x) & ", " & CInt(txt_date_y) & ", " & CInt(txt_date_width) & ", '" & txtfontname.Text & "', '" & txtfontsize.Text & "', 'N')", cn)
                                cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE,FONTSTYLE)VALUES(" & layoutId & ",'txt_date',604,18,0,'Verdana','8', 'N', '0')", cn)
                                cmd.ExecuteNonQuery()
                            End If

                            '-------------------------------------

                            If (objChecked.outputbearer = True) Then
                                'cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE)VALUES(" & LAYOUTID & ",'" & txt_bearer_name & "', " & CInt(txt_bearer_x) & ", " & CInt(txt_bearer_y) & ", " & CInt(txt_bearer_width) & ", '" & txtfontname.Text & "', '" & txtfontsize.Text & "', 'Y')", cn)

                                If (txtbearer_style <> "") Then
                                    'MsgBox("Already")
                                    cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE,FONTSTYLE)VALUES(" & layoutId & ",'txt_bearer', " & CInt(txt_bearer_x) & ", " & CInt(txt_bearer_y) & ", " & CInt(txt_bearer_width) & ", '" & txtbearer_font & "', '" & txtbearer_size & "', 'Y', '" & txtbearer_style & "')", cn)
                                    cmd.ExecuteNonQuery()
                                Else
                                    'MsgBox("me")
                                    cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE,FONTSTYLE)VALUES(" & layoutId & ",'txt_bearer', " & CInt(txt_bearer_x) & ", " & CInt(txt_bearer_y) & ", " & CInt(txt_bearer_width) & ", 'Verdana', '8', 'Y', '0')", cn)
                                    cmd.ExecuteNonQuery()
                                End If

                            Else
                                'cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE)VALUES(1,'" & txt_bearer_name & "', " & CInt(txt_bearer_x) & ", " & CInt(txt_bearer_y) & ", " & CInt(txt_bearer_width) & ", '" & txtfontname.Text & "', '" & txtfontsize.Text & "', 'N')", cn)
                                cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE,FONTSTYLE)VALUES(" & layoutId & ",'txt_bearer',612,93,0,'Verdana','8', 'N', '0')", cn)
                                cmd.ExecuteNonQuery()
                            End If


                            '------------------------------------- Newly

                            If (objChecked.outputamtno = True) Then
                                'cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE)VALUES(" & LAYOUTID & ",'" & txt_amt_name & "', " & CInt(txt_amt_x) & ", " & CInt(txt_amt_y) & ", " & CInt(txt_amt_width) & ", '" & txtfontname.Text & "', '" & txtfontsize.Text & "', 'Y')", cn)
                                If (txtamt_style <> "") Then
                                    'MsgBox("Already")
                                    cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE,FONTSTYLE)VALUES(" & layoutId & ",'txt_amt', " & CInt(txt_amt_x) & ", " & CInt(txt_amt_y) & ", " & CInt(txt_amt_width) & ", '" & txtamt_font & "', '" & txtamt_size & "', 'Y', '" & txtamt_style & "')", cn)
                                    cmd.ExecuteNonQuery()
                                Else
                                    'MsgBox("me")
                                    cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE,FONTSTYLE)VALUES(" & layoutId & ",'txt_amt', " & CInt(txt_amt_x) & ", " & CInt(txt_amt_y) & ", " & CInt(txt_amt_width) & ", 'Verdana', '8', 'Y', '0')", cn)
                                    cmd.ExecuteNonQuery()
                                End If

                            Else
                                '    cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE)VALUES(1,'" & txt_amt_name & "', " & CInt(txt_amt_x) & ", " & CInt(txt_amt_y) & ", " & CInt(txt_amt_width) & ", '" & txtfontname.Text & "', '" & txtfontsize.Text & "', 'N')", cn)
                                cmd = New OleDbCommand("INSERT INTO " & cnAdminDb & "..chqlayoutdetail(LAYOUTID,PARTICULARS,XAXIS,YAXIS,WIDTH,FONTNAME,FONTSIZE,ACTIVE,FONTSTYLE)VALUES(" & layoutId & ",'txt_amt',612,196,0,'Verdana','8', 'N', '0')", cn)
                                cmd.ExecuteNonQuery()
                            End If

                        End Try

                

                        '-------------------------------------
                        MsgBox("Saved", MsgBoxStyle.OkOnly)
                    Catch ex As Exception
                        MessageBox.Show(ex.ToString)
                    End Try
                Else
                    MsgBox("Layout Name Already Exist", MsgBoxStyle.Information)
                    cmbxAcname.Focus()
                End If

            Else

                ''START PLACE ''
                '''''''''''''''''UPDATE QUERY''''''''''''''''''''''''''''''''''
                Dim VALUE As String = objGPack.GetSqlValue("SELECT LAYOUTID FROM " & cnAdminDb & "..CHQLAYOUT WHERE LAYOUTNAME = '" & cmbxAcname.Text.ToUpper.Trim & "'")

                cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..CHQLAYOUT SET CHQDATEFORMAT = '" & txt_date.Text.ToString & "' WHERE LAYOUTID = " & VALUE & "", cn)
                cmd.ExecuteNonQuery()

                cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..CHQLAYOUT SET CHQDATESPACE = '" & cmbxDateSpace.Text & "' WHERE LAYOUTID = " & VALUE & "", cn)
                cmd.ExecuteNonQuery()

                Try
                    If ImageLength > 0 Then
                        Dim ImageTemp As Byte() = ConvertImageFiletoBytes(objPicture.Display_path)
                        strSql = "UPDATE " & cnAdminDb & "..CHQLAYOUT SET IMAGEPATH = ? "
                        strSql += " WHERE LAYOUTID = " & VALUE & ""
                        cmd = New OleDbCommand(strSql, cn)
                        'Dim picture As Image = Image.FromFile(objPicture.Display_path)
                        'Dim stream As New IO.MemoryStream
                        'picture.Save(stream, Imaging.ImageFormat.Jpeg)
                        cmd.Parameters.AddWithValue("@IMAGEPATH", ImageTemp)
                        'cmd.Parameters.Add("@IMAGEPATH", OleDbType.VarBinary).Value = ImageTemp
                        cmd.ExecuteNonQuery()
                    End If
                Catch ex As Exception
                    MessageBox.Show(ex.ToString)
                Finally
                    If (objChecked.outputacpayee = True And txt_acpayee.Visible = True) Then
                        If (acpayee_style <> "") Then
                            'MsgBox("User Select a Font")
                            cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = " & CInt(txt_acpayee_x) & ", YAXIS = " & CInt(txt_acpayee_y) & ", WIDTH = " & CInt(txt_acpayee_width) & ", ACTIVE = 'Y', FONTNAME='" & acpayee_font & "', FONTSIZE='" & acpayee_size & "', FONTSTYLE = '" & acpayee_style & "' WHERE LAYOUTID = " & VALUE & " AND PARTICULARS = '" & txt_acpayee_name & "'", cn)
                            cmd.ExecuteNonQuery()
                        Else
                            'MsgBox("Programmer Default Select Font")
                            cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = " & CInt(txt_acpayee_x) & ", YAXIS = " & CInt(txt_acpayee_y) & ", WIDTH = " & CInt(txt_acpayee_width) & ", ACTIVE = 'Y', FONTNAME='Verdana', FONTSIZE='8', FONTSTYLE = '0' WHERE LAYOUTID = " & VALUE & " AND PARTICULARS = '" & txt_acpayee_name & "'", cn)
                            cmd.ExecuteNonQuery()
                        End If

                    Else
                        'cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = 0, YAXIS = 0, WIDTH = 0, ACTIVE = 'N' WHERE LAYOUTID = 1 AND PARTICULARS = '" & txt_acpayee_name & "'", cn)
                        cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = 0, YAXIS = 0, WIDTH = 0, ACTIVE = 'N' WHERE LAYOUTID = " & VALUE & " AND PARTICULARS = 'txt_acpayee'", cn)
                        cmd.ExecuteNonQuery()
                    End If
                    '------------------------------------
                    If (objChecked.outputpayeeline1 = True And txt_payee1.Visible = True) Then
                        If (txtpayee1_style <> "") Then
                            ' user
                            cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = " & CInt(txt_payee1_x) & ", YAXIS = " & CInt(txt_payee1_y) & ", WIDTH = " & CInt(txt_payee1_width) & ", ACTIVE = 'Y', FONTNAME='" & txtpayee1_font & "', FONTSIZE='" & txtpayee1_size & "', FONTSTYLE = '" & txtpayee1_style & "' WHERE LAYOUTID = " & VALUE & " AND PARTICULARS = '" & txt_payee1_name & "'", cn)
                            cmd.ExecuteNonQuery()

                        Else
                            'developer
                            cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = " & CInt(txt_payee1_x) & ", YAXIS = " & CInt(txt_payee1_y) & ", WIDTH = " & CInt(txt_payee1_width) & ", ACTIVE = 'Y', FONTNAME='Verdana', FONTSIZE='8', FONTSTYLE = '0' WHERE LAYOUTID = " & VALUE & " AND PARTICULARS = '" & txt_payee1_name & "'", cn)
                            cmd.ExecuteNonQuery()

                        End If

                    Else
                        'cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = 0, YAXIS = 0, WIDTH = 0, ACTIVE = 'N' WHERE LAYOUTID = 1 AND PARTICULARS = '" & txt_payee1_name & "'", cn)
                        cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = 0, YAXIS = 0, WIDTH = 0, ACTIVE = 'N' WHERE LAYOUTID = " & VALUE & " AND PARTICULARS = 'txt_payee1'", cn)
                        cmd.ExecuteNonQuery()
                    End If

                    '-----------------------------------------------

                    If (objChecked.outputpayeeline2 = True And txt_payee2.Visible = True) Then

                        If (txtpayee2_style <> "") Then
                            'user
                            cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = " & CInt(txt_payee2_x) & ", YAXIS = " & CInt(txt_payee2_y) & ", WIDTH = " & CInt(txt_payee2_width) & ", ACTIVE = 'Y', FONTNAME='" & txtpayee2_font & "', FONTSIZE='" & txtpayee2_size & "', FONTSTYLE = '" & txtpayee2_style & "' WHERE LAYOUTID = " & VALUE & " AND PARTICULARS = '" & txt_payee2_name & "'", cn)
                            cmd.ExecuteNonQuery()
                        Else
                            'developer
                            cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = " & CInt(txt_payee2_x) & ", YAXIS = " & CInt(txt_payee2_y) & ", WIDTH = " & CInt(txt_payee2_width) & ", ACTIVE = 'Y', FONTNAME='Verdana', FONTSIZE='8', FONTSTYLE = '0' WHERE LAYOUTID = " & VALUE & " AND PARTICULARS = '" & txt_payee2_name & "'", cn)
                            cmd.ExecuteNonQuery()
                        End If
                    Else
                        'cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = 0, YAXIS = 0, WIDTH = 0, ACTIVE = 'N' WHERE LAYOUTID = 1 AND PARTICULARS = '" & txt_payee2_name & "'", cn)
                        cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = 0, YAXIS = 0, WIDTH = 0, ACTIVE = 'N' WHERE LAYOUTID = " & VALUE & " AND PARTICULARS = 'txt_payee2'", cn)
                        cmd.ExecuteNonQuery()
                    End If

                    '------------------------------------------------

                    If (objChecked.outputamtwords1 = True And txt_amtword1.Visible = True) Then

                        If (txtamt1_style <> "") Then
                            'user
                            cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = " & CInt(txt_amtword1_x) & ", YAXIS = " & CInt(txt_amtword1_y) & ", WIDTH = " & CInt(txt_amtword1_width) & ", ACTIVE = 'Y', FONTNAME='" & txtamt1_font & "', FONTSIZE='" & txtamt1_size & "', FONTSTYLE = '" & txtamt1_style & "' WHERE LAYOUTID = " & VALUE & " AND PARTICULARS = '" & txt_amtword1_name & "'", cn)
                            cmd.ExecuteNonQuery()
                        Else
                            'developer
                            cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = " & CInt(txt_amtword1_x) & ", YAXIS = " & CInt(txt_amtword1_y) & ", WIDTH = " & CInt(txt_amtword1_width) & ", ACTIVE = 'Y', FONTNAME='Verdana', FONTSIZE='8', FONTSTYLE = '0' WHERE LAYOUTID = " & VALUE & " AND PARTICULARS = '" & txt_amtword1_name & "'", cn)
                            cmd.ExecuteNonQuery()
                        End If
                    Else
                        'cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = 0, YAXIS = 0, WIDTH = 0, ACTIVE = 'N' WHERE LAYOUTID = 1 AND PARTICULARS = '" & txt_amtword1_name & "'", cn)
                        cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = 0, YAXIS = 0, WIDTH = 0, ACTIVE = 'N' WHERE LAYOUTID = " & VALUE & " AND PARTICULARS = 'txt_amtword1'", cn)
                        cmd.ExecuteNonQuery()
                    End If

                    '---------------------------------------

                    If (objChecked.outputamtwords2 = True And txt_amtword2.Visible = True) Then

                        If (txtamt2_style <> "") Then
                            'user
                            cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = " & CInt(txt_amtword2_x) & ", YAXIS = " & CInt(txt_amtword2_y) & ", WIDTH = " & CInt(txt_amtword2_width) & ", ACTIVE = 'Y', FONTNAME='" & txtamt2_font & "', FONTSIZE='" & txtamt2_size & "', FONTSTYLE = '" & txtamt2_style & "' WHERE LAYOUTID = " & VALUE & " AND PARTICULARS = '" & txt_amtword2_name & "'", cn)
                            cmd.ExecuteNonQuery()

                        Else
                            'developer
                            cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = " & CInt(txt_amtword2_x) & ", YAXIS = " & CInt(txt_amtword2_y) & ", WIDTH = " & CInt(txt_amtword2_width) & ", ACTIVE = 'Y', FONTNAME='Verdana', FONTSIZE='8', FONTSTYLE = '0' WHERE LAYOUTID = " & VALUE & " AND PARTICULARS = '" & txt_amtword2_name & "'", cn)
                            cmd.ExecuteNonQuery()

                        End If
                    Else
                        cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = 0, YAXIS = 0, WIDTH = 0, ACTIVE = 'N' WHERE LAYOUTID = " & VALUE & " AND PARTICULARS = 'txt_amtword2'", cn)
                        cmd.ExecuteNonQuery()
                    End If

                    '-------------------------------------------



                    If (objChecked.outputtxtline1 = True And txt_textline1.Visible = True) Then
                        If (txtline1_style <> "") Then
                            'user
                            cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = " & CInt(txt_textline1_x) & ", YAXIS = " & CInt(txt_textline1_y) & ", WIDTH = " & CInt(txt_textline1_width) & ", ACTIVE = 'Y', FONTNAME='" & txtline1_font & "', FONTSIZE='" & txtline1_size & "', FONTSTYLE = '" & txtline1_style & "' WHERE LAYOUTID = " & VALUE & " AND PARTICULARS = '" & txt_textline1_name & "'", cn)
                            cmd.ExecuteNonQuery()
                        Else
                            'developer
                            cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = " & CInt(txt_textline1_x) & ", YAXIS = " & CInt(txt_textline1_y) & ", WIDTH = " & CInt(txt_textline1_width) & ", ACTIVE = 'Y', FONTNAME='Verdana', FONTSIZE='8', FONTSTYLE = '0' WHERE LAYOUTID = " & VALUE & " AND PARTICULARS = '" & txt_textline1_name & "'", cn)
                            cmd.ExecuteNonQuery()
                        End If
                    Else

                        'cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = 0, YAXIS = 0, WIDTH = 0, ACTIVE = 'N' WHERE LAYOUTID = 1 AND PARTICULARS = '" & txt_textline1_name & "'", cn)
                        cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = 0, YAXIS = 0, WIDTH = 0, ACTIVE = 'N' WHERE LAYOUTID = " & VALUE & " AND PARTICULARS = 'txt_textline1'", cn)
                        cmd.ExecuteNonQuery()
                    End If

                    '----------------------------------------
                    If (objChecked.outputtxtline2 = True And txt_textline2.Visible = True) Then

                        If (txtline2_style <> "") Then
                            'user
                            cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = " & CInt(txt_textline2_x) & ", YAXIS = " & CInt(txt_textline2_y) & ", WIDTH = '" & CInt(txt_textline2_width) & ", ACTIVE = 'Y', FONTNAME='" & txtline2_font & "', FONTSIZE='" & txtline2_size & "', FONTSTYLE = '" & txtline2_style & "' WHERE LAYOUTID = " & VALUE & " AND PARTICULARS = '" & txt_textline2_name & "'", cn)
                            cmd.ExecuteNonQuery()
                        Else
                            'developer
                            cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = " & CInt(txt_textline2_x) & ", YAXIS = " & CInt(txt_textline2_y) & ", WIDTH = " & CInt(txt_textline2_width) & ", ACTIVE = 'Y', FONTNAME='Verdana', FONTSIZE='8', FONTSTYLE = '0' WHERE LAYOUTID = " & VALUE & " AND PARTICULARS = '" & txt_textline2_name & "'", cn)
                            cmd.ExecuteNonQuery()
                        End If
                    Else
                        'cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = 0, YAXIS = 0, WIDTH = 0, ACTIVE = 'N' WHERE LAYOUTID = 1 AND PARTICULARS = '" & txt_textline2_name & "'", cn)
                        cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = 0, YAXIS = 0, WIDTH = 0, ACTIVE = 'N' WHERE LAYOUTID = " & VALUE & " AND PARTICULARS = 'txt_textline2'", cn)
                        cmd.ExecuteNonQuery()
                    End If

                    '----------------------------------------

                    If (objChecked.outputtxtline3 = True And txt_textline3.Visible = True) Then
                        If (txtline3_style <> "") Then
                            'user
                            cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = " & CInt(txt_textline3_x) & ", YAXIS = " & CInt(txt_textline3_y) & ", WIDTH = " & CInt(txt_textline3_width) & ", ACTIVE = 'Y', FONTNAME='" & txtline3_font & "', FONTSIZE='" & txtline3_size & "', FONTSTYLE = '" & txtline3_style & "' WHERE LAYOUTID = " & VALUE & " AND PARTICULARS = '" & txt_textline3_name & "'", cn)
                            cmd.ExecuteNonQuery()
                        Else
                            'developer
                            cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = " & CInt(txt_textline3_x) & ", YAXIS = " & CInt(txt_textline3_y) & ", WIDTH = " & CInt(txt_textline3_width) & ", ACTIVE = 'Y', FONTNAME='Verdana', FONTSIZE='8', FONTSTYLE = '0' WHERE LAYOUTID = " & VALUE & " AND PARTICULARS = '" & txt_textline3_name & "'", cn)
                            cmd.ExecuteNonQuery()
                        End If
                    Else
                        'cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = 0, YAXIS = 0, WIDTH = 0, ACTIVE = 'N' WHERE LAYOUTID = 1 AND PARTICULARS = '" & txt_textline3_name & "'", cn)
                        cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = 0, YAXIS = 0, WIDTH = 0, ACTIVE = 'N' WHERE LAYOUTID = " & VALUE & " AND PARTICULARS = 'txt_textline3'", cn)
                        cmd.ExecuteNonQuery()
                    End If


                    '------------------------------------

                    If (objChecked.outputnotabove = True And txt_notaboveamt.Visible = True) Then

                        If (notabove_style <> "") Then
                            'user
                            cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = " & CInt(txt_notaboveamt_x) & ", YAXIS = " & CInt(txt_notaboveamt_y) & ", WIDTH = " & CInt(txt_notaboveamt_width) & ", ACTIVE = 'Y', FONTNAME='" & notabove_font & "', FONTSIZE='" & notabove_size & "', FONTSTYLE = '" & notabove_style & "' WHERE LAYOUTID = " & VALUE & " AND PARTICULARS = '" & txt_notaboveamt_name & "'", cn)
                            cmd.ExecuteNonQuery()
                        Else
                            'developer
                            cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = " & CInt(txt_notaboveamt_x) & ", YAXIS = " & CInt(txt_notaboveamt_y) & ", WIDTH = " & CInt(txt_notaboveamt_width) & ", ACTIVE = 'Y', FONTNAME='Verdana', FONTSIZE='8', FONTSTYLE = '0' WHERE LAYOUTID = " & VALUE & " AND PARTICULARS = '" & txt_notaboveamt_name & "'", cn)
                            cmd.ExecuteNonQuery()
                        End If
                    Else
                        'cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = 0, YAXIS = 0, WIDTH = 0, ACTIVE = 'N' WHERE LAYOUTID = 1 AND PARTICULARS = '" & txt_notaboveamt_name & "'", cn)
                        cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = 0, YAXIS = 0, WIDTH = 0, ACTIVE = 'N' WHERE LAYOUTID = " & VALUE & " AND PARTICULARS = 'txt_notaboveamt'", cn)
                        cmd.ExecuteNonQuery()

                    End If

                    '---------------------------------------

                    If (objChecked.outputdt_tx = True And txt_date.Visible = True) Then
                        If (txtdate_style <> "") Then
                            'user
                            cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = " & CInt(txt_date_x) & ", YAXIS = " & CInt(txt_date_y) & ", WIDTH = " & CInt(txt_date_width) & ", ACTIVE = 'Y', FONTNAME='" & txtdate_font & "', FONTSIZE='" & txtdate_size & "', FONTSTYLE = '" & txtdate_style & "' WHERE LAYOUTID = " & VALUE & " AND PARTICULARS = '" & txt_date_name & "'", cn)
                            cmd.ExecuteNonQuery()
                        Else
                            'developer
                            cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = " & CInt(txt_date_x) & ", YAXIS = " & CInt(txt_date_y) & ", WIDTH = " & CInt(txt_date_width) & ", ACTIVE = 'Y', FONTNAME='Verdana', FONTSIZE='8', FONTSTYLE = '0' WHERE LAYOUTID = " & VALUE & " AND PARTICULARS = '" & txt_date_name & "'", cn)
                            cmd.ExecuteNonQuery()
                        End If
                    Else
                        cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = 0, YAXIS = 0, WIDTH = 0, ACTIVE = 'N' WHERE LAYOUTID = " & VALUE & " AND PARTICULARS = 'txt_date'", cn)
                        cmd.ExecuteNonQuery()
                    End If

                    '-------------------------------------------------
                    If (objChecked.outputamtno = True And txt_amt.Visible = True) Then
                        If (txtamt_style <> "") Then
                            cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = " & CInt(txt_amt_x) & ", YAXIS = " & CInt(txt_amt_y) & ", WIDTH = " & CInt(txt_amt_width) & ", ACTIVE = 'Y', FONTNAME='" & txtamt_font & "', FONTSIZE='" & txtamt_size & "', FONTSTYLE = '" & txtamt_style & "' WHERE LAYOUTID = " & VALUE & " AND PARTICULARS = '" & txt_amt_name & "'", cn)
                            cmd.ExecuteNonQuery()
                        Else
                            cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = " & CInt(txt_amt_x) & ", YAXIS = " & CInt(txt_amt_y) & ", WIDTH = " & CInt(txt_amt_width) & ", ACTIVE = 'Y', FONTNAME='Verdana', FONTSIZE='8', FONTSTYLE = '0' WHERE LAYOUTID = " & VALUE & " AND PARTICULARS = '" & txt_amt_name & "'", cn)
                            cmd.ExecuteNonQuery()
                        End If
                    Else
                        'cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = 0, YAXIS = 0, WIDTH = 0, ACTIVE = 'N' WHERE LAYOUTID = 1 AND PARTICULARS = '" & txt_amt_name & "'", cn)
                        cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = 0, YAXIS = 0, WIDTH = 0, ACTIVE = 'N' WHERE LAYOUTID = " & VALUE & " AND PARTICULARS = 'txt_amt'", cn)
                        cmd.ExecuteNonQuery()
                    End If

                    '-------------------------------------------------------

                    If (objChecked.outputbearer = True And txt_bearer.Visible = True) Then
                        If (txtbearer_style <> "") Then
                            cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = " & CInt(txt_bearer_x) & ", YAXIS = " & CInt(txt_bearer_y) & ", WIDTH = " & CInt(txt_bearer_width) & ", ACTIVE = 'Y', FONTNAME='" & txtbearer_font & "', FONTSIZE='" & txtbearer_size & "', FONTSTYLE = '" & txtbearer_style & "' WHERE LAYOUTID = " & VALUE & " AND PARTICULARS = '" & txt_bearer_name & "'", cn)
                            cmd.ExecuteNonQuery()
                        Else
                            cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = " & CInt(txt_bearer_x) & ", YAXIS = " & CInt(txt_bearer_y) & ", WIDTH = " & CInt(txt_bearer_width) & ", ACTIVE = 'Y', FONTNAME='Verdana', FONTSIZE='8', FONTSTYLE = '0' WHERE LAYOUTID = " & VALUE & " AND PARTICULARS = '" & txt_bearer_name & "'", cn)
                            cmd.ExecuteNonQuery()
                        End If
                    Else
                        'cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = 0, YAXIS = 0, WIDTH = 0, ACTIVE = 'N' WHERE LAYOUTID = 1 AND PARTICULARS = '" & txt_bearer_name & "'", cn)
                        cmd = New OleDbCommand("UPDATE " & cnAdminDb & "..chqlayoutdetail SET XAXIS = 0, YAXIS = 0, WIDTH = 0, ACTIVE = 'N' WHERE LAYOUTID = " & VALUE & " AND PARTICULARS = 'txt_bearer'", cn)
                        cmd.ExecuteNonQuery()
                    End If
                End Try
                MsgBox("Updated")

            End If
        Else
            MsgBox("Enter The LayoutName")
            cmbxAcname.Focus()
        End If

    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click

        tabMain.SelectedTab = tabView
        dt = New DataTable()
        gridView.DataSource = Nothing
        'strSql = "SELECT * FROM  " & cnAdminDb & "..CHQLAYOUT"
        strSql = "SELECT DISTINCT C.LAYOUTID,C.LAYOUTNAME,C.CHQDATEFORMAT,C.CHQDATESPACE"
        strSql += vbCrLf + "FROM " & cnAdminDb & "..CHQLAYOUT AS C"
        strSql += vbCrLf + "INNER JOIN " & cnAdminDb & "..CHQLAYOUTDETAIL AS CD ON CD.LAYOUTID = C.LAYOUTID"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        'If dt.Columns.Contains("ID") Then dt.Columns.Remove()
        'If dt.Columns.Contains("IMAGEPATH") Then dt.Columns.Remove()
        'If dt.Columns.Contains("ACCODE") Then dt.Columns.Remove()
        gridView.DataSource = dt

        With gridView.Columns("LAYOUTID")
            .Width = 100
        End With

        With gridView.Columns("LAYOUTNAME")
            .Width = 100
        End With

        With gridView.Columns("CHQDATEFORMAT")
            .Width = 250
        End With

        With gridView.Columns("CHQDATESPACE")
            .Width = 100
        End With
        gridView.Focus()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        'txtLayoutName.Clear()
        'txtLayoutId.Clear()
        ' Image Clear
        picbxImage.Image = Nothing
        objPicture.path = ""
        '
        chkbxacpayee.Checked = False
        chkbxpayeeline1.Checked = False
        chkbxpayeeline2.Checked = False
        chkbxamtwords1.Checked = False
        chkbxamtwords2.Checked = False
        chkbxtextline1.Checked = False
        chkbxtextline2.Checked = False
        chkbxtextline3.Checked = False
        chkbxnotabove.Checked = False
        chkbxdate.Checked = False
        chkbxamt.Checked = False
        chkbxbareer.Checked = False
        txtpicwidth.Clear()
        txtpicheight.Clear()
        txtxaxis.Clear()
        txtyaxis.Clear()
        txtwidth.Clear()
        txtfontname.Clear()
        txtfontsize.Clear()
        'chkbxDateFormat.Items.Clear()
        'cmbxDateSpace.Items.Clear()
        cmbxAcname.Text = ""
        cmbxAccode.Text = ""
        cmbxAcname.Enabled = True
        Saveflag = True
        cmbxAcname.Focus()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnSelectImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectImage.Click
        If OpenFileDialog1.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            picbxImage.Image = Image.FromFile(OpenFileDialog1.FileName)
            objPicture.path = OpenFileDialog1.FileName.ToString
        End If
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub btn_setfont_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_setfont.Click
        Me.Refresh()
        Try
            If FontDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                'activeTextBox click 
                'txt IsNot Nothing is moving
                ' don't use txt.focus
                ' Newly 
                If (activeTextBox IsNot Nothing) Then
                    activeTextBox.Font = FontDialog1.Font

                    'set value
                    objFont.namefont = FontDialog1.Font.Name.ToString
                    objFont.sizefont = FontDialog1.Font.Size.ToString
                    'objfont.stylefont = FontDialog1.Font.Style.ToString
                    objFont.stylefont_value = FontDialog1.Font.Style

                    'get value
                    txtfontname.Text = objFont.display_namefont()
                    txtfontsize.Text = objFont.display_fontsize()
                    'fontstylevalue_global = objfont.display_stylefont_value()
                    'txtfontstyle.Text = objfont.display_stylefont()
                    txtfontstyle.Text = objFont.display_stylefont_value()


                    'Dim Current_Textbox As String = txt.Name
                    Dim Current_Textbox As String = activeTextBox.Name

                    Dim current_TextBoxFontname As String = txtfontname.Text
                    Dim Current_TextBoxFontsize As String = txtfontsize.Text
                    Dim current_TextBoxFontStyle As String = txtfontstyle.Text


                    If (Current_Textbox = "txt_acpayee") Then

                        obj_curfontsize.textacpayee_fontname = current_TextBoxFontname
                        obj_curfontsize.textacpayee_fontsize = Current_TextBoxFontsize
                        obj_curfontsize.textacpayee_fontstyle = current_TextBoxFontStyle


                        acpayee_font = obj_curfontsize.display_textacpayee_fontname()
                        acpayee_size = obj_curfontsize.display_textacpayee_fontsize()
                        acpayee_style = obj_curfontsize.display_textacpayee_fontstyle()
                        'Else
                        '    acpayee_font = "Microsoft Sans Serif"
                        '    acpayee_size = "8.25"
                    End If

                    If (Current_Textbox = "txt_payee1") Then

                        obj_curfontsize.a1_fontname = current_TextBoxFontname
                        obj_curfontsize.a2_fontsize = Current_TextBoxFontsize
                        obj_curfontsize.a3_fontstyle = current_TextBoxFontStyle

                        txtpayee1_font = obj_curfontsize.display_a1()
                        txtpayee1_size = obj_curfontsize.display_a2()
                        txtpayee1_style = obj_curfontsize.display_a3()
                        'Else
                        '    txtpayee1_font = "Microsoft Sans Serif"
                        '    txtpayee1_size = "8.25"

                    End If


                    If (Current_Textbox = "txt_payee2") Then

                        obj_curfontsize.b1_fontname = current_TextBoxFontname
                        obj_curfontsize.b2_fontsize = Current_TextBoxFontsize
                        obj_curfontsize.b3_fontstyle = current_TextBoxFontStyle

                        txtpayee2_font = obj_curfontsize.display_b1()
                        txtpayee2_size = obj_curfontsize.display_b2()
                        txtpayee2_style = obj_curfontsize.display_b3()
                        'Else
                        '    txtpayee2_font = "Microsoft Sans Serif"
                        '    txtpayee2_size = "8.25"

                    End If


                    If (Current_Textbox = "txt_amtword1") Then

                        obj_curfontsize.c1_fontname = current_TextBoxFontname
                        obj_curfontsize.c2_fontsize = Current_TextBoxFontsize
                        obj_curfontsize.c3_fontstyle = current_TextBoxFontStyle


                        txtamt1_font = obj_curfontsize.display_c1()
                        txtamt1_size = obj_curfontsize.display_c2()
                        txtamt1_style = obj_curfontsize.display_c3()
                        'Else
                        '    txtamt1_font = "Microsoft Sans Serif"
                        '    txtamt1_size = "8.25"

                    End If

                    If (Current_Textbox = "txt_amtword2") Then

                        obj_curfontsize.d1_fontname = current_TextBoxFontname
                        obj_curfontsize.d2_fontsize = Current_TextBoxFontsize
                        obj_curfontsize.d3_fontstyle = current_TextBoxFontStyle

                        txtamt2_font = obj_curfontsize.display_d1()
                        txtamt2_size = obj_curfontsize.display_d2()
                        txtamt2_style = obj_curfontsize.display_d3()
                        'Else
                        '    txtamt2_font = "Microsoft Sans Serif"
                        '    txtamt2_size = "8.25"

                    End If

                    If (Current_Textbox = "txt_textline1") Then

                        obj_curfontsize.e1_fontname = current_TextBoxFontname
                        obj_curfontsize.e2_fontsize = Current_TextBoxFontsize
                        obj_curfontsize.e3_fontstyle = current_TextBoxFontStyle

                        txtline1_font = obj_curfontsize.display_e1()
                        txtline1_size = obj_curfontsize.display_e2()
                        txtline1_style = obj_curfontsize.display_e3()

                        'Else
                        '    txtline1_font = "Microsoft Sans Serif"
                        '    txtline1_size = "8.25"

                    End If

                    If (Current_Textbox = "txt_textline2") Then

                        obj_curfontsize.f1_fontname = current_TextBoxFontname
                        obj_curfontsize.f2_fontsize = Current_TextBoxFontsize
                        obj_curfontsize.f3_fontstyle = current_TextBoxFontStyle

                        txtline2_font = obj_curfontsize.display_f1()
                        txtline2_size = obj_curfontsize.display_f2()
                        txtline2_style = obj_curfontsize.display_f3()

                        'Else
                        '    txtline2_font = "Microsoft Sans Serif"
                        '    txtline2_size = "8.25"
                    End If


                    If (Current_Textbox = "txt_textline3") Then

                        obj_curfontsize.g1_fontname = current_TextBoxFontname
                        obj_curfontsize.g2_fontsize = Current_TextBoxFontsize
                        obj_curfontsize.g3_fontstyle = current_TextBoxFontStyle

                        txtline3_font = obj_curfontsize.display_g1()
                        txtline3_size = obj_curfontsize.display_g2()
                        txtline3_style = obj_curfontsize.display_g3()
                        'Else
                        '    txtline3_font = "Microsoft Sans Serif"
                        '    txtline3_size = "8.25"

                    End If


                    If (Current_Textbox = "txt_notaboveamt") Then

                        obj_curfontsize.h1_fontname = current_TextBoxFontname
                        obj_curfontsize.h2_fontsize = Current_TextBoxFontsize
                        obj_curfontsize.h3_fontstyle = current_TextBoxFontStyle

                        notabove_font = obj_curfontsize.display_h1()
                        notabove_size = obj_curfontsize.display_h2()
                        notabove_style = obj_curfontsize.display_h3()

                        'Else
                        '    notabove_font = "Microsoft Sans Serif"
                        '    notabove_size = "8.25"

                    End If

                    If (Current_Textbox = "txt_date") Then

                        obj_curfontsize.i1_fontname = current_TextBoxFontname
                        obj_curfontsize.i2_fontsize = Current_TextBoxFontsize
                        obj_curfontsize.i3_fontstyle = current_TextBoxFontStyle


                        txtdate_font = obj_curfontsize.display_i1()
                        txtdate_size = obj_curfontsize.display_i2()
                        txtdate_style = obj_curfontsize.display_i3()

                        'Else
                        '    txtdate_font = "Microsoft Sans Serif"
                        '    txtdate_size = "8.25"
                    End If

                    If (Current_Textbox = "txt_amt") Then
                        obj_curfontsize.j1_fontname = current_TextBoxFontname
                        obj_curfontsize.j2_fontsize = Current_TextBoxFontsize
                        obj_curfontsize.j3_fontstyle = current_TextBoxFontStyle


                        txtamt_font = obj_curfontsize.display_j1()
                        txtamt_size = obj_curfontsize.display_j2()
                        txtamt_style = obj_curfontsize.display_j3()

                        'Else
                        '    txtamt_font = "Microsoft Sans Serif"
                        '    txtamt_size = "8.25"

                    End If

                    If (Current_Textbox = "txt_bearer") Then

                        obj_curfontsize.k1_fontname = current_TextBoxFontname
                        obj_curfontsize.k2_fontsize = Current_TextBoxFontsize
                        obj_curfontsize.k3_fontstyle = current_TextBoxFontStyle

                        txtbearer_font = obj_curfontsize.display_k1()
                        txtbearer_size = obj_curfontsize.display_k2()
                        txtbearer_style = obj_curfontsize.display_k3()
                        'Else
                        '    txtbearer_font = "Microsoft Sans Serif"
                        '    txtbearer_size = "8.25"
                    End If
                    'MessageBox.Show(Current_Textbox)
                    'MessageBox.Show(temp)
                    'MessageBox.Show(temp2)
                End If
                'objfont.namefont = FontDialog1.Font.ToString
                'setfont_size
                'If (FontDialog1.Font.Style = FontStyle.Bold) Then
                '    objfont.stylefont = FontStyle.Bold.ToString
                'End If

                'If (FontDialog1.Font.Style = FontStyle.Italic) Then
                '    objfont.stylefont = FontStyle.Italic.ToString
                'End If

                'If (FontDialog1.Font.Style = FontStyle.Bold + FontStyle.Italic) Then
                '    objfont.stylefont = FontStyle.Bold.ToString + FontStyle.Italic.ToString
                'End If

                'If (FontDialog1.Font.Style = FontStyle.Regular) Then
                '    objfont.stylefont = FontStyle.Regular.ToString
                'End If

                'If FontDialog1.Font.Strikeout = True Then
                '    'boolean -> objfont.effectstrikefont = FontStyle.Strikeout
                '    objfont.effectstrikefont = FontStyle.Strikeout.ToString
                'Else
                '    objfont.effectstrikefont = "NOT SELECTED"
                'End If

                'If FontDialog1.Font.Underline = True Then
                '    objfont.effectunderlinefont = FontStyle.Strikeout.ToString
                'Else
                '    objfont.effectunderlinefont = "Not Selected"
                'End If
                'OLD displayfont_size
                'txtfontname.Text = objfont.display_namefont()
                'txtfontsize.Text = objfont.display_fontsize()
                'txtfontstyle.Text = objfont.display_stylefont()
                'txtfonteffect_underline.Text = objfont.display_effectunderlinefont()
                'txtfonteffect_strikeout.Text = objfont.display_effectstrikefont()
                'Me.Refresh()

            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString)

        End Try
    End Sub
#End Region

#Region "ToolStripMenuItem"
    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        'btnSave_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        ' btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
#End Region

#Region "TabView Button"
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGeneral
        cmbxAcname.Focus()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        dt = New DataTable()
        gridView.DataSource = Nothing
        strSql = "SELECT DISTINCT C.LAYOUTID,C.LAYOUTNAME,C.CHQDATEFORMAT, C.CHQDATESPACE"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..CHQLAYOUT AS C"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CHQLAYOUTDETAIL AS CD ON CD.LAYOUTID = C.LAYOUTID"
        strSql += vbCrLf + " WHERE 1 = 1 "
        If txtLayoutNameSearch.Text <> "" Then
            strSql += vbCrLf + " AND LAYOUTNAME = '" & txtLayoutNameSearch.Text & "'"
        End If
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        gridView.DataSource = dt
        With gridView.Columns("LAYOUTID")
            .Width = 100
        End With
        With gridView.Columns("LAYOUTNAME")
            .Width = 100
        End With
        With gridView.Columns("CHQDATEFORMAT")
            .Width = 250
        End With
        With gridView.Columns("CHQDATESPACE")
            .Width = 100
        End With
        gridView.Focus()
    End Sub
#End Region

#Region "CheckBox"

    Private Sub checkCheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkbxacpayee.CheckedChanged, chkbxpayeeline1.CheckedChanged, _
    chkbxpayeeline2.CheckedChanged, chkbxamtwords1.CheckedChanged, chkbxamtwords2.CheckedChanged, chkbxtextline1.CheckedChanged, chkbxtextline2.CheckedChanged, chkbxtextline3.CheckedChanged, _
    chkbxdate.CheckedChanged, chkbxbareer.CheckedChanged, chkbxnotabove.CheckedChanged, chkbxamt.CheckedChanged
        setChkbxvalue()

        'txt_acpayee
        If (objChecked.outputacpayee = True) Then
            txt_acpayee.Visible = True
        Else
            txt_acpayee.Visible = False
        End If

        If (objChecked.outputpayeeline1 = True) Then
            txt_payee1.Visible = True
        Else
            txt_payee1.Visible = False
        End If

        If (objChecked.outputpayeeline2 = True) Then
            txt_payee2.Visible = True
        Else
            txt_payee2.Visible = False
        End If

        If (objChecked.outputamtwords1 = True) Then
            txt_amtword1.Visible = True
        Else
            txt_amtword1.Visible = False
        End If

        If (objChecked.outputamtwords2 = True) Then
            txt_amtword2.Visible = True
        Else
            txt_amtword2.Visible = False
        End If

        If (objChecked.outputtxtline1 = True) Then
            txt_textline1.Visible = True
        Else
            txt_textline1.Visible = False
        End If

        If (objChecked.outputtxtline2 = True) Then
            txt_textline2.Visible = True
        Else
            txt_textline2.Visible = False
        End If

        If (objChecked.outputtxtline3 = True) Then
            txt_textline3.Visible = True
        Else
            txt_textline3.Visible = False
        End If

        If (objChecked.outputnotabove = True) Then
            txt_notaboveamt.Visible = True
        Else
            txt_notaboveamt.Visible = False
        End If

        If (objChecked.outputdt_tx = True) Then
            txt_date.Visible = True
        Else
            txt_date.Visible = False
        End If

        If (objChecked.outputbearer = True) Then
            txt_bearer.Visible = True
        Else
            txt_bearer.Visible = False
        End If

        If (objChecked.outputamtno = True) Then
            txt_amt.Visible = True
        Else
            txt_amt.Visible = False
        End If

    End Sub

#End Region

#Region "User Define Function"

    Public Function ConvertImageFiletoBytes(ByVal ImageFilePath As String) As Byte()
        Dim _tempByte() As Byte = Nothing
        If String.IsNullOrEmpty(ImageFilePath) = True Then
            Throw New ArgumentNullException("Image File Name Cannot be Null or Empty", "ImageFilePath")
            Return Nothing
        End If
        Try
            Dim _fileInfo As New IO.FileInfo(ImageFilePath)
            Dim _NumBytes As Long = _fileInfo.Length
            Dim _FStream As New IO.FileStream(ImageFilePath, IO.FileMode.Open, IO.FileAccess.Read)
            Dim _BinaryReader As New IO.BinaryReader(_FStream)
            _tempByte = _BinaryReader.ReadBytes(Convert.ToInt32(_NumBytes))
            _fileInfo = Nothing
            _NumBytes = 0
            _FStream.Close()
            _FStream.Dispose()
            _BinaryReader.Close()
            Return _tempByte
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Private Sub setChkbxvalue()
        objChecked.chkacpayee = chkbxacpayee.Checked
        objChecked.chkpayeeline1 = chkbxpayeeline1.Checked
        objChecked.chkpayeeline2 = chkbxpayeeline2.Checked
        objChecked.chkamtword1 = chkbxamtwords1.Checked
        objChecked.chkamtword2 = chkbxamtwords2.Checked
        objChecked.chktxtline1 = chkbxtextline1.Checked
        objChecked.chktxtline2 = chkbxtextline2.Checked
        objChecked.chktxtline3 = chkbxtextline3.Checked
        objChecked.chkdate1 = chkbxdate.Checked
        objChecked.chkbearer = chkbxbareer.Checked
        objChecked.chknotabove = chkbxnotabove.Checked
        objChecked.chkamountno = chkbxamt.Checked
        'MessageBox.Show(objChecked.chkacpayee)
        'MessageBox.Show(objChecked.chkpayeeline1)
    End Sub

    ' Not used
    Private Sub getchkbxvalue()
        chkbxacpayee.Checked = objChecked.outputacpayee
        chkbxpayeeline1.Checked = objChecked.outputpayeeline1
        chkbxpayeeline2.Checked = objChecked.outputpayeeline2
        chkbxamtwords1.Checked = objChecked.outputamtwords1
        chkbxamtwords2.Checked = objChecked.outputamtwords2
        chkbxtextline1.Checked = objChecked.outputtxtline1
        chkbxtextline2.Checked = objChecked.outputtxtline2
        chkbxtextline3.Checked = objChecked.outputtxtline3
        chkbxnotabove.Checked = objChecked.outputnotabove
        chkbxdate.Checked = objChecked.outputdt_tx
        chkbxbareer.Checked = objChecked.outputbearer

    End Sub
#End Region

#Region "Gridview Variable"

    'x,y,width 

    Dim acpayee_x, acpayee_y, acpayee_width As Integer
    Dim payee1_x, payee1_y, payee1_width As Integer
    Dim payee2_x, payee2_y, payee2_width As Integer
    Dim amtword1_x, amtword1_y, amtword1_width As Integer
    Dim amtword2_x, amtword2_y, amtword2_width As Integer
    Dim textline1_x, textline1_y, textline1_width As Integer
    Dim textline2_x, textline2_y, textline2_width As Integer
    Dim textline3_x, textline3_y, textline3_width As Integer
    Dim notaboveamt_x, notaboveamt_y, notaboveamt_width As Integer
    Dim date_x, date_y, date_width As Integer
    Dim bearer_x, bearer_y, bearer_width As Integer
    Dim amt_x, amt_y, amt_width As Integer


    'Fontname,size,style
    Dim acpayee_fontname As String
    Dim acpayee_fontsize As Integer
    'Dim acpayee_fontstyle As String
    Dim acpayee_fontstyle As Integer

    Dim payee1_fontname, payee2_fontname As String
    Dim payee1_fontsize, payee2_fontsize As Integer
    'Dim payee1_fontstyle, payee2_fontstyle As String
    Dim payee1_fontstyle, payee2_fontstyle As Integer

    Dim amtword1_fontname, amtword2_fontname As String
    Dim amtword1_fontsize, amtword2_fontsize As Integer
    'Dim amt1_fontstyle, amt2_fontstyle As String
    Dim amtword1_fontstyle, amtword2_fontstyle As Integer

    Dim textline1_fontname, textline2_fontname, textline3_fontname As String
    Dim textline1_fontsize, textline2_fontsize, textline3_fontsize As Integer
    'Dim txtline1_fontstyle, txtline2_fontstyle, txtline3_fontstyle As String
    Dim textline1_fontstyle, textline2_fontstyle, textline3_fontstyle As Integer

    Dim notaboveamt_fontname As String
    Dim notaboveamt_fontsize As Integer
    'Dim notaboveamt_fontstyle As String
    Dim notaboveamt_fontstyle As Integer

    Dim date_fontname As String
    Dim date_fontsize As Integer
    'Dim date_fontstyle As String
    Dim date_fontstyle As Integer

    Dim bearer_fontname As String
    Dim bearer_fontsize As Integer
    'Dim bearer_fontstyle As String
    Dim bearer_fontstyle As Integer

    Dim amt_fontname As String
    Dim amt_fontsize As Integer
    'Dim amt_fontstyle As String
    Dim amt_fontstyle As Integer

    'Active
    Dim acpayee_active As String
    Dim payee1_active, payee2_active As String
    Dim amtword1_active, amtword2_active As String
    Dim textline1_active, textline2_active, textline3_active As String
    Dim notaboveamt_active, date_active, bearer_active As String
    Dim amt_active As String


#End Region

#Region "Gridview"
    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress

        If e.KeyChar = Chr(Keys.Enter) Then
            dt = New DataTable
            e.Handled = True

            Dim temp As String = gridView.CurrentRow.Cells("LAYOUTNAME").Value.ToString
            Dim Style As FontStyle
            Dim tempbyte As Byte() = Nothing
            Dim imgMemoryStream As MemoryStream = New MemoryStream()

            strSql = vbCrLf + "SELECT C.LAYOUTID,C.LAYOUTNAME,C.CHQDATEFORMAT,C.CHQDATESPACE,C.ACCODE"
            strSql += vbCrLf + ",C.IMAGEPATH,CD.PARTICULARS,CD.XAXIS,CD.YAXIS"
            strSql += vbCrLf + ",CD.WIDTH,CD.FONTNAME,CD.FONTSIZE,CD.FONTSTYLE,CD.ACTIVE"
            strSql += vbCrLf + "FROM " & cnAdminDb & "..CHQLAYOUT AS C"
            strSql += vbCrLf + "INNER JOIN " & cnAdminDb & "..CHQLAYOUTDETAIL AS CD ON CD.LAYOUTID = C.LAYOUTID"
            strSql += vbCrLf + "WHERE C.LAYOUTNAME = '" & gridView.CurrentRow.Cells("LAYOUTNAME").Value.ToString & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)

            If dt.Rows.Count > 0 Then

                cmbxAcname.Text = dt.Rows(0).Item("LAYOUTNAME")
                cmbxDateSpace.Text = dt.Rows(0).Item("CHQDATESPACE")
                chkbxDateFormat.Text = dt.Rows(0).Item("CHQDATEFORMAT")
                cmbxAccode.Text = dt.Rows(0).Item("ACCODE")

                tempbyte = dt.Rows(0).Item("IMAGEPATH")

                If (IsDBNull(dt.Rows(0).Item("IMAGEPATH")) = True) Then
                    picbxImage.Image = Nothing
                Else
                    If tempbyte.Length > 0 Then
                        imgMemoryStream = New MemoryStream(tempbyte)
                        picbxImage.Image = Drawing.Image.FromStream(imgMemoryStream)
                        picbxImage.SizeMode = PictureBoxSizeMode.Zoom
                    End If
                End If

                acpayee_x = dt.Rows(0).Item("XAXIS")
                acpayee_y = dt.Rows(0).Item("YAXIS")
                acpayee_width = dt.Rows(0).Item("WIDTH")
                acpayee_active = dt.Rows(0).Item("ACTIVE")
                acpayee_fontname = dt.Rows(0).Item("FONTNAME")
                acpayee_fontsize = CInt(dt.Rows(0).Item("FONTSIZE"))
                acpayee_fontstyle = CInt(dt.Rows(0).Item("FONTSTYLE"))

                payee1_x = dt.Rows(1).Item("XAXIS")
                payee1_y = dt.Rows(1).Item("YAXIS")
                payee1_width = dt.Rows(1).Item("WIDTH")
                payee1_active = dt.Rows(1).Item("ACTIVE")
                payee1_fontname = dt.Rows(1).Item("FONTNAME")
                payee1_fontsize = CInt(dt.Rows(1).Item("FONTSIZE"))
                payee1_fontstyle = CInt(dt.Rows(1).Item("FONTSTYLE"))


                payee2_x = dt.Rows(2).Item("XAXIS")
                payee2_y = dt.Rows(2).Item("YAXIS")
                payee2_width = dt.Rows(2).Item("WIDTH")
                payee2_active = dt.Rows(2).Item("ACTIVE")
                payee2_fontname = dt.Rows(2).Item("FONTNAME")
                payee2_fontsize = CInt(dt.Rows(2).Item("FONTSIZE"))
                payee2_fontstyle = CInt(dt.Rows(2).Item("FONTSTYLE"))

                amtword1_x = dt.Rows(3).Item("XAXIS")
                amtword1_y = dt.Rows(3).Item("YAXIS")
                amtword1_width = dt.Rows(3).Item("WIDTH")
                amtword1_active = dt.Rows(3).Item("ACTIVE")
                amtword1_fontname = dt.Rows(3).Item("FONTNAME")
                amtword1_fontsize = CInt(dt.Rows(3).Item("FONTSIZE"))
                amtword1_fontstyle = CInt(dt.Rows(3).Item("FONTSTYLE"))

                amtword2_x = dt.Rows(4).Item("XAXIS")
                amtword2_y = dt.Rows(4).Item("YAXIS")
                amtword2_width = dt.Rows(4).Item("WIDTH")
                amtword2_active = dt.Rows(4).Item("ACTIVE")
                amtword2_fontname = dt.Rows(4).Item("FONTNAME")
                amtword2_fontsize = CInt(dt.Rows(4).Item("FONTSIZE"))
                amtword2_fontstyle = CInt(dt.Rows(4).Item("FONTSTYLE"))

                textline1_x = dt.Rows(5).Item("XAXIS")
                textline1_y = dt.Rows(5).Item("YAXIS")
                textline1_width = dt.Rows(5).Item("WIDTH")
                textline1_active = dt.Rows(5).Item("ACTIVE")
                textline1_fontname = dt.Rows(5).Item("FONTNAME")
                textline1_fontsize = CInt(dt.Rows(5).Item("FONTSIZE"))
                textline1_fontstyle = CInt(dt.Rows(5).Item("FONTSTYLE"))


                textline2_x = dt.Rows(6).Item("XAXIS")
                textline2_y = dt.Rows(6).Item("YAXIS")
                textline2_width = dt.Rows(6).Item("WIDTH")
                textline2_active = dt.Rows(6).Item("ACTIVE")
                textline2_fontname = dt.Rows(6).Item("FONTNAME")
                textline2_fontsize = CInt(dt.Rows(6).Item("FONTSIZE"))
                textline2_fontstyle = CInt(dt.Rows(6).Item("FONTSTYLE"))


                textline3_x = dt.Rows(7).Item("XAXIS")
                textline3_y = dt.Rows(7).Item("YAXIS")
                textline3_width = dt.Rows(7).Item("WIDTH")
                textline3_active = dt.Rows(7).Item("ACTIVE")
                textline3_fontname = dt.Rows(7).Item("FONTNAME")
                textline3_fontsize = CInt(dt.Rows(7).Item("FONTSIZE"))
                textline3_fontstyle = CInt(dt.Rows(7).Item("FONTSTYLE"))


                notaboveamt_x = dt.Rows(8).Item("XAXIS")
                notaboveamt_y = dt.Rows(8).Item("YAXIS")
                notaboveamt_width = dt.Rows(8).Item("WIDTH")
                notaboveamt_active = dt.Rows(8).Item("ACTIVE")
                notaboveamt_fontname = dt.Rows(8).Item("FONTNAME")
                notaboveamt_fontsize = CInt(dt.Rows(8).Item("FONTSIZE"))
                notaboveamt_fontstyle = CInt(dt.Rows(8).Item("FONTSTYLE"))


                date_x = dt.Rows(9).Item("XAXIS")
                date_y = dt.Rows(9).Item("YAXIS")
                date_width = dt.Rows(9).Item("WIDTH")
                date_active = dt.Rows(9).Item("ACTIVE")
                date_fontname = dt.Rows(9).Item("FONTNAME")
                date_fontsize = CInt(dt.Rows(9).Item("FONTSIZE"))
                date_fontstyle = CInt(dt.Rows(9).Item("FONTSTYLE"))


                bearer_x = dt.Rows(10).Item("XAXIS")
                bearer_y = dt.Rows(10).Item("YAXIS")
                bearer_width = dt.Rows(10).Item("WIDTH")
                bearer_active = dt.Rows(10).Item("ACTIVE")
                bearer_fontname = dt.Rows(10).Item("FONTNAME")
                bearer_fontsize = CInt(dt.Rows(10).Item("FONTSIZE"))
                bearer_fontstyle = CInt(dt.Rows(10).Item("FONTSTYLE"))


                amt_x = dt.Rows(11).Item("XAXIS")
                amt_y = dt.Rows(11).Item("YAXIS")
                amt_width = dt.Rows(11).Item("WIDTH")
                amt_active = dt.Rows(11).Item("ACTIVE")
                amt_fontname = dt.Rows(11).Item("FONTNAME")
                amt_fontsize = CInt(dt.Rows(11).Item("FONTSIZE"))
                amt_fontstyle = CInt(dt.Rows(11).Item("FONTSTYLE"))

                If acpayee_active = "Y" Then
                    txt_acpayee.Visible = True
                    chkbxacpayee.Checked = True
                    Style = acpayee_fontstyle
                    txt_acpayee.Location = New Point(acpayee_x, acpayee_y)
                    txt_acpayee.Font = New Font(acpayee_fontname, acpayee_fontsize, Style)
                Else
                    chkbxacpayee.Checked = False
                    txt_acpayee.Visible = False
                End If

                If payee1_active = "Y" Then
                    txt_payee1.Visible = True
                    chkbxpayeeline1.Checked = True
                    Style = payee1_fontstyle
                    txt_payee1.Location = New Point(payee1_x, payee1_y)
                    txt_payee1.Font = New Font(payee1_fontname, payee1_fontsize, Style)

                Else
                    chkbxpayeeline1.Checked = False
                    txt_payee1.Visible = False
                End If

                If payee2_active = "Y" Then
                    txt_payee2.Visible = True
                    chkbxpayeeline2.Checked = True
                    Style = payee2_fontstyle
                    txt_payee2.Location = New Point(payee2_x, payee2_y)
                    txt_payee2.Font = New Font(payee2_fontname, payee2_fontsize, Style)
                Else
                    chkbxpayeeline2.Checked = False
                    txt_payee2.Visible = False
                End If

                If amtword1_active = "Y" Then
                    txt_amtword1.Visible = True
                    chkbxamtwords1.Checked = True
                    Style = amtword1_fontstyle
                    txt_amtword1.Location = New Point(amtword1_x, amtword1_y)
                    txt_amtword1.Font = New Font(amtword1_fontname, amtword1_fontsize, Style)
                Else
                    chkbxamtwords1.Checked = False
                    txt_amtword1.Visible = False
                End If

                If amtword2_active = "Y" Then
                    txt_amtword2.Visible = True
                    chkbxamtwords2.Checked = True
                    Style = amtword2_fontstyle
                    txt_amtword2.Location = New Point(amtword2_x, amtword2_y)
                    txt_amtword2.Font = New Font(amtword2_fontname, amtword2_fontsize, Style)
                Else
                    chkbxamtwords2.Checked = False
                    txt_amtword2.Visible = False
                End If


                If textline1_active = "Y" Then
                    txt_textline1.Visible = True
                    chkbxtextline1.Checked = True
                    Style = textline1_fontstyle
                    txt_textline1.Location = New Point(textline1_x, textline1_y)
                    txt_textline1.Font = New Font(textline1_fontname, textline1_fontsize, Style)
                Else
                    chkbxtextline1.Checked = False
                    txt_textline1.Visible = False
                End If

                If textline2_active = "Y" Then
                    txt_textline2.Visible = True
                    chkbxtextline2.Checked = True
                    Style = textline2_fontstyle
                    txt_textline2.Location = New Point(textline2_x, textline2_y)
                    txt_textline2.Font = New Font(textline2_fontname, textline2_fontsize, Style)
                Else
                    chkbxtextline2.Checked = False
                    txt_textline2.Visible = False
                End If

                If textline3_active = "Y" Then
                    txt_textline3.Visible = True
                    chkbxtextline3.Checked = True
                    Style = textline3_fontstyle
                    txt_textline3.Location = New Point(textline3_x, textline3_y)
                    txt_textline3.Font = New Font(textline3_fontname, textline3_fontsize, Style)
                Else
                    chkbxtextline3.Checked = False
                    txt_textline3.Visible = False
                End If

                If date_active = "Y" Then
                    txt_date.Visible = True
                    chkbxdate.Checked = True
                    Style = date_fontstyle
                    txt_date.Location = New Point(date_x, date_y)
                    txt_date.Font = New Font(date_fontname, date_fontsize, Style)
                Else
                    chkbxdate.Checked = False
                    txt_date.Visible = False
                End If

                If bearer_active = "Y" Then
                    txt_bearer.Visible = True
                    chkbxbareer.Checked = True
                    Style = bearer_fontstyle
                    txt_bearer.Location = New Point(bearer_x, bearer_y)
                    txt_bearer.Font = New Font(bearer_fontname, bearer_fontsize, Style)
                Else
                    chkbxbareer.Checked = False
                    txt_bearer.Visible = False
                End If

                If amt_active = "Y" Then
                    txt_amt.Visible = True
                    chkbxamt.Checked = True
                    Style = amt_fontstyle
                    txt_amt.Location = New Point(amt_x, amt_y)
                    txt_amt.Font = New Font(amt_fontname, amt_fontsize, Style)
                Else
                    chkbxamt.Checked = False
                    txt_amt.Visible = False
                End If

                If notaboveamt_active = "Y" Then
                    txt_notaboveamt.Visible = True
                    chkbxnotabove.Checked = True
                    Style = notabove_style
                    txt_notaboveamt.Location = New Point(notaboveamt_x, notaboveamt_y)
                    txt_notaboveamt.Font = New Font(notaboveamt_fontname, notaboveamt_fontsize, Style)
                Else
                    chkbxnotabove.Checked = False
                    txt_notaboveamt.Visible = False
                End If

            End If
            Saveflag = False
            tabMain.SelectedTab = tabGeneral
            cmbxAcname.Enabled = False
            cmbxAcname.Focus()
        Else
            'MsgBox("No Record Found")
        End If
    End Sub

#End Region

#Region "combox Event"
    Private Sub chkbxDateFormat_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkbxDateFormat.SelectedIndexChanged
        txt_date.Text = chkbxDateFormat.Text
    End Sub

    Private Sub cmbxAcname_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbxAcname.SelectedIndexChanged
        cmbxAccode.Items.Clear()
        strSql = "SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE = 'B' AND ACNAME = '" & cmbxAcname.Text & " '"
        objGPack.FillCombo(strSql, cmbxAccode, False)
    End Sub
    Private Sub cmbxDateSpace_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbxDateSpace.SelectedIndexChanged

        If cmbxDateSpace.Text = "1" Then
        ElseIf cmbxDateSpace.Text = "2" Then
        ElseIf cmbxDateSpace.Text = "3" Then
        ElseIf cmbxDateSpace.Text = "4" Then
        ElseIf cmbxDateSpace.Text = "5" Then
        ElseIf cmbxDateSpace.Text = "6" Then
        End If

    End Sub
#End Region

End Class

#Region "CheckBox Value"
Public Class checkboxValueReturn

    Public chkacpayee = True
    Public Property outputacpayee() As Boolean
        Get
            Return chkacpayee
        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property

    Public chkpayeeline1 = True
    Public Property outputpayeeline1() As Boolean
        Get
            Return chkpayeeline1
        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property


    Public chkpayeeline2 = True
    Public Property outputpayeeline2() As Boolean
        Get
            Return chkpayeeline2
        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property

    Public chkamtword1 = True

    Public Property outputamtwords1() As Boolean
        Get
            Return chkamtword1

        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property


    Public chkamtword2 = True

    Public Property outputamtwords2() As Boolean
        Get
            Return chkamtword2
        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property

    Public chktxtline1 = True

    Public Property outputtxtline1() As Boolean
        Get
            Return chktxtline1
        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property


    Public chktxtline2 = True

    Public Property outputtxtline2() As Boolean
        Get
            Return chktxtline2
        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property


    Public chktxtline3 = True

    Public Property outputtxtline3() As Boolean
        Get
            Return chktxtline3
        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property

    Public chknotabove = True

    Public Property outputnotabove() As Boolean
        Get
            Return chknotabove
        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property


    Public chkdate1 = True

    Public Property outputdt_tx() As Boolean
        Get
            Return chkdate1
        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property

    Public chkbearer = True

    Public Property outputbearer() As Boolean
        Get
            Return chkbearer
        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property

    Public chkamountno = True

    Public Property outputamtno() As Boolean
        Get
            Return chkamountno
        End Get
        Set(ByVal value As Boolean)

        End Set
    End Property
End Class
#End Region

#Region "Font Value"
Public Class font_name_size

    Public fontsize As String
    Public fontname As String
    'Public fontcolor As String
    Public fontsytle_name As String
    Public fontstyle_value As String

    'Not used Now
    Public fontunderlineeffect As String
    Public fontstrikeouteffect As String
    'Not used Now
    Public font_fontstyle_size As Font
    Public font_effect As FontStyle

    Public WriteOnly Property fontdesign()
        Set(ByVal value10)
            font_fontstyle_size = value10
        End Set
    End Property
    Public Function display_fontdesign() As Font
        Return font_fontstyle_size
    End Function


    Public WriteOnly Property fonteffect()
        Set(ByVal value11)
            font_effect = value11
        End Set
    End Property
    Public Function display_fonteffect() As FontStyle
        Return font_effect
    End Function
    Public WriteOnly Property sizefont()
        Set(ByVal value1)
            fontsize = value1
        End Set
    End Property


    Public Function display_fontsize() As String
        Return fontsize
    End Function

    Public WriteOnly Property namefont()
        Set(ByVal value2)
            fontname = value2
        End Set
    End Property

    Public Function display_namefont() As String
        Return fontname
    End Function

    Public WriteOnly Property stylefont()
        Set(ByVal value3)
            fontsytle_name = value3
        End Set
    End Property

    Public Function display_stylefont() As String
        Return fontsytle_name
    End Function
    '---------------------------------------------------------------
    Public WriteOnly Property stylefont_value()
        Set(ByVal value6)
            fontstyle_value = value6
        End Set
    End Property

    Public Function display_stylefont_value() As String
        Return fontstyle_value
    End Function
    '----------------------------------------------------
    Public WriteOnly Property effectunderlinefont()
        Set(ByVal value4)
            fontunderlineeffect = value4
        End Set
    End Property

    Public Function display_effectunderlinefont() As String
        Return fontunderlineeffect
    End Function


    Public WriteOnly Property effectstrikefont()
        Set(ByVal value5)
            fontstrikeouteffect = value5
        End Set
    End Property

    Public Function display_effectstrikefont() As String
        Return fontstrikeouteffect
    End Function

End Class
#End Region

#Region "ImagePath"

Public Class pictureboxvaluereturn
    Public picbx_path As String

    Public WriteOnly Property path()
        'Get
        '    Return picbx_path
        'End Get
        Set(ByVal value1)
            picbx_path = value1
        End Set

    End Property

    Public Function Display_path() As String
        'Console.WriteLine(picbx_path)
        Return picbx_path
    End Function
End Class

#End Region

#Region "FontSize,Name,Style"
Public Class textboxactive_name_x_y_width

    Public acpayee_fontname, acpayee_fontsize, acpayee_fontstyle As String

    Public a1, a2, a3 As String ' txt_Amt1
    Public b1, b2, b3 As String ' txt_Amt2

    Public c1, c2, c3 As String ' txt_payee1
    Public d1, d2, d3 As String ' txt_payee2

    Public e1, e2, e3 As String ' txt_line1
    Public f1, f2, f3 As String ' txt_line2
    Public g1, g2, g3 As String ' txt_line3

    Public h1, h2, h3 As String ' txt_notaboveamt

    Public i1, i2, i3 As String ' txt_date

    Public j1, j2, j3 As String ' txt_amt

    Public k1, k2, k3 As String ' txt_bearer


    Public WriteOnly Property textacpayee_fontname()
        Set(ByVal value)
            acpayee_fontname = value
        End Set
    End Property

    Public Function display_textacpayee_fontname()
        Return acpayee_fontname
    End Function

    Public WriteOnly Property textacpayee_fontsize()
        Set(ByVal value)
            acpayee_fontsize = value
        End Set
    End Property

    Public Function display_textacpayee_fontsize()
        Return acpayee_fontsize
    End Function

    Public WriteOnly Property textacpayee_fontstyle()
        Set(ByVal value)
            acpayee_fontstyle = value
        End Set
    End Property

    Public Function display_textacpayee_fontstyle()
        Return acpayee_fontstyle
    End Function

    '-------------------------------------

    Public WriteOnly Property a1_fontname()
        Set(ByVal value)
            a1 = value
        End Set
    End Property

    Public Function display_a1()
        Return a1
    End Function

    Public WriteOnly Property a2_fontsize()
        Set(ByVal value)
            a2 = value
        End Set
    End Property

    Public Function display_a2()
        Return a2
    End Function

    Public WriteOnly Property a3_fontstyle()
        Set(ByVal value)
            a3 = value
        End Set
    End Property

    Public Function display_a3()
        Return a3
    End Function


    '----------------------------


    Public WriteOnly Property b1_fontname()
        Set(ByVal value)
            b1 = value
        End Set
    End Property

    Public Function display_b1()
        Return b1
    End Function

    Public WriteOnly Property b2_fontsize()
        Set(ByVal value)
            b2 = value
        End Set
    End Property

    Public Function display_b2()
        Return b2
    End Function

    Public WriteOnly Property b3_fontstyle()
        Set(ByVal value)
            b3 = value
        End Set
    End Property

    Public Function display_b3()
        Return b3
    End Function


    '---------------------------------

    Public WriteOnly Property c1_fontname()
        Set(ByVal value)
            c1 = value
        End Set
    End Property

    Public Function display_c1()
        Return c1
    End Function

    Public WriteOnly Property c2_fontsize()
        Set(ByVal value)
            c2 = value
        End Set
    End Property

    Public Function display_c2()
        Return c2
    End Function

    Public WriteOnly Property c3_fontstyle()
        Set(ByVal value)
            c3 = value
        End Set
    End Property

    Public Function display_c3()
        Return c3
    End Function

    '----------------------



    Public WriteOnly Property d1_fontname()
        Set(ByVal value)
            d1 = value
        End Set
    End Property

    Public Function display_d1()
        Return d1
    End Function

    Public WriteOnly Property d2_fontsize()
        Set(ByVal value)
            d2 = value
        End Set
    End Property

    Public Function display_d2()
        Return d2
    End Function

    Public WriteOnly Property d3_fontstyle()
        Set(ByVal value)
            d3 = value
        End Set
    End Property

    Public Function display_d3()
        Return d3
    End Function

    '-----------------------



    Public WriteOnly Property e1_fontname()
        Set(ByVal value)
            e1 = value
        End Set
    End Property

    Public Function display_e1()
        Return e1
    End Function

    Public WriteOnly Property e2_fontsize()
        Set(ByVal value)
            e2 = value
        End Set
    End Property

    Public Function display_e2()
        Return e2
    End Function

    Public WriteOnly Property e3_fontstyle()
        Set(ByVal value)
            e3 = value
        End Set
    End Property

    Public Function display_e3()
        Return e3
    End Function

    '--------------------

    Public WriteOnly Property f1_fontname()
        Set(ByVal value)
            f1 = value
        End Set
    End Property

    Public Function display_f1()
        Return f1
    End Function

    Public WriteOnly Property f2_fontsize()
        Set(ByVal value)
            f2 = value
        End Set
    End Property

    Public Function display_f2()
        Return f2
    End Function

    Public WriteOnly Property f3_fontstyle()
        Set(ByVal value)
            f3 = value
        End Set
    End Property

    Public Function display_f3()
        Return f3
    End Function

    '---------------------

    Public WriteOnly Property g1_fontname()
        Set(ByVal value)
            g1 = value
        End Set
    End Property

    Public Function display_g1() As String
        Return g1
    End Function

    Public WriteOnly Property g2_fontsize()
        Set(ByVal value)
            g2 = value
        End Set
    End Property

    Public Function display_g2() As String
        Return g2
    End Function

    Public WriteOnly Property g3_fontstyle()
        Set(ByVal value)
            g3 = value
        End Set
    End Property

    Public Function display_g3()
        Return g3
    End Function

    '-----------------

    Public WriteOnly Property h1_fontname()
        Set(ByVal value)
            h1 = value
        End Set
    End Property

    Public Function display_h1() As String
        Return h1
    End Function

    Public WriteOnly Property h2_fontsize()
        Set(ByVal value)
            h2 = value
        End Set
    End Property

    Public Function display_h2() As String
        Return h2
    End Function

    Public WriteOnly Property h3_fontstyle()
        Set(ByVal value)
            h3 = value
        End Set
    End Property

    Public Function display_h3()
        Return h3
    End Function

    '---------------------

    Public WriteOnly Property i1_fontname()
        Set(ByVal value)
            i1 = value
        End Set
    End Property

    Public Function display_i1() As String
        Return i1
    End Function

    Public WriteOnly Property i2_fontsize()
        Set(ByVal value)
            i2 = value
        End Set
    End Property

    Public Function display_i2() As String
        Return i2
    End Function

    Public WriteOnly Property i3_fontstyle()
        Set(ByVal value)
            i3 = value
        End Set
    End Property

    Public Function display_i3()
        Return i3
    End Function

    '---------------------


    Public WriteOnly Property j1_fontname()
        Set(ByVal value)
            j1 = value
        End Set
    End Property

    Public Function display_j1() As String
        Return j1
    End Function

    Public WriteOnly Property j2_fontsize()
        Set(ByVal value)
            j2 = value
        End Set
    End Property

    Public Function display_j2() As String
        Return j2
    End Function

    Public WriteOnly Property j3_fontstyle()
        Set(ByVal value)
            j3 = value
        End Set
    End Property

    Public Function display_j3()
        Return j3
    End Function

    '-------------------


    Public WriteOnly Property k1_fontname()
        Set(ByVal value)
            k1 = value
        End Set
    End Property

    Public Function display_k1() As String
        Return k1
    End Function

    Public WriteOnly Property k2_fontsize()
        Set(ByVal value)
            k2 = value
        End Set
    End Property

    Public Function display_k2() As String
        Return k2
    End Function


    Public WriteOnly Property k3_fontstyle()
        Set(ByVal value)
            k3 = value
        End Set
    End Property

    Public Function display_k3()
        Return k3
    End Function

End Class

#End Region