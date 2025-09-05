Imports System.Data.OleDb
Imports System.Drawing.Text
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports ThoughtWorks.QRCode.Codec
Imports ThoughtWorks.QRCode.Codec.Data

Public Class Methods
    Private strSql As String

    Public Enum RightMode
        Save = 0
        Edit = 1
        Delete = 2
        View = 3
        Excel = 4
        Print = 5
        Authorize = 6
        MiscIssue = 7
        Cancel = 8
        Sale = 9
        Purchase = 10
        SReturn = 11
        Qsale = 12
    End Enum
    Public Sub New()

    End Sub


    Public Sub New(ByVal connection As OleDbConnection)
        G_Cn = connection
    End Sub

    Public Sub New _
    (
    ByVal connection As OleDbConnection _
    , ByVal cnAdmindb As String _
    , ByVal LangId As String _
    , ByVal FocusColor As Color _
    , ByVal LostFocusColor As Color _
    , ByVal Radio_Check_LostFocusColor As Color _
    , ByVal Button_LostFocusColor As Color _
    , ByVal GrdBackGroundColor As Color _
    , Optional ByVal BakImage As Image = Nothing _
    , Optional ByVal characterCasing As CharacterCasing = CharacterCasing.Upper _
    , Optional ByVal cnTranFromDate As Date = Nothing _
    , Optional ByVal cnTranToDate As Date = Nothing _
    , Optional ByVal CurrDecimal As Integer = 2
    )

        G_Cn = connection
        G_CnAdmindb = cnAdmindb
        G_LangId = LangId
        G_FocusColor = Color.LightGreen
        G_LostFocusColor = SystemColors.Window
        G_Radio_Check_LostFocusColor = Color.Transparent
        G_Button_LostFocusColor = Color.FromKnownColor(KnownColor.Control)
        G_GrdBackGroundColor = GrdBackGroundColor
        G_BakImage = BakImage
        G_TextCharacterCasing = characterCasing
        G_CnTranFromDate = IIf(cnTranFromDate = Nothing, DateTimePicker.MinimumDateTime.Date, cnTranFromDate)
        G_CnTranToDate = IIf(cnTranToDate = Nothing, DateTimePicker.MaximumDateTime.Date, cnTranToDate)
        G_Decimal = CurrDecimal
    End Sub

    Public Function qrcode_image(ByVal inputdata As String, ByVal fileref As String) As Integer
        Dim LocalTemp As String = System.Environment.GetEnvironmentVariable("temp")
        Dim qrCodeEncoder As New QRCodeEncoder
        qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE

        Try
            Dim scale As Integer = 4
            qrCodeEncoder.QRCodeScale = scale
        Catch ex As Exception

        End Try
        Try
            Dim version As Integer = 25 '17
            qrCodeEncoder.QRCodeVersion = version
        Catch ex As Exception

        End Try
        Dim PICQRCODE As New PictureBox
        Try
            qrCodeEncoder.QRCodeErrorCorrect = qrCodeEncoder.ERROR_CORRECTION.M
            Dim img As Image
            Dim data As String = inputdata
            qrCodeEncoder.Encode(data)
            img = qrCodeEncoder.Encode(data)
            PICQRCODE.Image = img
            'img.Save(Application.StartupPath & "\Q_" & inputdata & ".jpg")
            PICQRCODE.Image.Save(LocalTemp & "\QRCODE_" & fileref & ".jpeg", Imaging.ImageFormat.Jpeg)

        Catch ex As Exception
            MsgBox(ex.Message)


        End Try
    End Function

    Public Function qrcode_image2(ByVal inputdata As String, ByVal fileref As String) As Image
        'Dim LocalTemp As String = System.Environment.GetEnvironmentVariable("temp")
        Dim qrCodeEncoder As New QRCodeEncoder
        qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE

        Try
            Dim scale As Integer = 4
            qrCodeEncoder.QRCodeScale = scale
        Catch ex As Exception

        End Try
        Try
            Dim version As Integer = 25 '17
            qrCodeEncoder.QRCodeVersion = version
        Catch ex As Exception

        End Try
        'Dim PICQRCODE As New PictureBox
        Try
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M
            Dim img As Image
            Dim data As String = inputdata
            qrCodeEncoder.Encode(data)
            img = qrCodeEncoder.Encode(data)
            'PICQRCODE.Image = img
            'img.Save(Application.StartupPath & "\Q_" & inputdata & ".jpg")
            'PICQRCODE.Image.Save(LocalTemp & "\QRCODE_" & fileref & ".jpeg", Imaging.ImageFormat.Jpeg)
            Return img
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Public Function DupChecker(ByVal sender As Control, ByVal Qry As String, Optional ByVal tran As OleDbTransaction = Nothing) As Boolean
        If Not tran Is Nothing Then
            G_Cmd = New OleDbCommand(Qry, G_Cn, tran)
            G_DAdapter = New OleDbDataAdapter(G_Cmd)
        Else
            G_DAdapter = New OleDbDataAdapter(Qry, G_Cn)
        End If
        G_DTable = New DataTable
        G_DAdapter.Fill(G_DTable)
        If G_DTable.Rows.Count > 0 Then
            Dim fieldCaption As String = Nothing
            Dim parentForm As Form = GetParentControl(sender)
            If G_LangId <> Nothing Then
                fieldCaption = LanguageChange.Get_LanguageCaption(G_LangId, parentForm, parentForm.GetNextControl(sender, False))
            End If
            If fieldCaption = Nothing Then
                fieldCaption = parentForm.GetNextControl(sender, False).Text
                MsgBox(fieldCaption + E0002, MsgBoxStyle.Information) ''already Exist
                sender.Focus()
            End If
            Return True
        End If
    End Function

    Public Function DupCheck(ByVal qry As String, Optional ByVal TRAN As OleDbTransaction = Nothing) As Boolean
        If TRAN Is Nothing Then
            G_DAdapter = New OleDbDataAdapter(qry, G_Cn)
        Else
            G_Cmd = New OleDbCommand(qry, G_Cn, TRAN)
            G_DAdapter = New OleDbDataAdapter(G_Cmd)
        End If
        G_DTable = New DataTable
        G_DAdapter.Fill(G_DTable)
        If G_DTable.Rows.Count > 0 Then Return True
    End Function

    Public Function GetMax(ByVal field As String, ByVal tableName As String, ByVal dbName As String, Optional ByVal Tran As OleDbTransaction = Nothing) As String
        Dim strSql As String
        strSql = " SELECT ISNULL(MAX(" & field & "),0)+1 AS MAXVALUE FROM " & dbName & ".." & tableName & ""
        If Tran Is Nothing Then
            G_DAdapter = New OleDbDataAdapter(strSql, G_Cn)
        Else
            G_Cmd = New OleDbCommand(strSql, G_Cn, Tran)
            G_DAdapter = New OleDbDataAdapter(G_Cmd)
        End If

        G_DTable = New DataTable
        G_DAdapter.Fill(G_DTable)
        If G_DTable.Rows.Count > 0 Then Return Trim(G_DTable.Rows(0).Item("MAXVALUE"))
        Return "0"
    End Function

    Public Sub FillCombo(ByVal str As String, ByVal Sender As Object _
    , Optional ByVal clear As Boolean = True, Optional ByVal SetDefault As Boolean = True)
        Dim cmb As ComboBox = CType(Sender, ComboBox)
        cmb.Text = ""
        If clear = True Then cmb.Items.Clear()
        G_DAdapter = New OleDbDataAdapter(str, G_Cn)
        G_DTable = New DataTable
        G_DAdapter.Fill(G_DTable)
        For cnt As Integer = 0 To G_DTable.Rows.Count - 1
            cmb.Items.Add(G_DTable.Rows(cnt).Item(0).ToString)
        Next
        If SetDefault Then
            If cmb.Items.Count > 0 Then cmb.SelectedIndex = 0
        End If
    End Sub

    Public Sub FillCombonew(ByVal dtable As DataTable, ByVal Sender As Object _
   , Optional ByVal clear As Boolean = True, Optional ByVal SetDefault As Boolean = True)
        Dim cmb As ComboBox = CType(Sender, ComboBox)
        cmb.Text = ""
        If clear = True Then cmb.Items.Clear()
        For cnt As Integer = 0 To dtable.Rows.Count - 1
            cmb.Items.Add(dtable.Rows(cnt).Item(0).ToString)
        Next

        If SetDefault Then
            If cmb.Items.Count > 0 Then cmb.SelectedIndex = 0
        End If
    End Sub
    Public Function GetSqlValue(ByVal qry As String, Optional ByVal field As String = Nothing, Optional ByVal defValue As String = "", Optional ByRef tran As OleDbTransaction = Nothing) As String
        G_DTable = New DataTable
        If tran Is Nothing Then
            G_DAdapter = New OleDbDataAdapter(qry, G_Cn)
            G_DAdapter.Fill(G_DTable)
        Else
            G_Cmd = New OleDbCommand(qry, G_Cn, tran)
            G_DAdapter = New OleDbDataAdapter(G_Cmd)
            G_DAdapter.Fill(G_DTable)
        End If
        If field <> "" Then
            If G_DTable.Rows.Count > 0 Then Return G_DTable.Rows(0).Item(field).ToString
        Else
            If G_DTable.Rows.Count > 0 Then Return G_DTable.Rows(0).Item(0).ToString
        End If
        Return defValue
    End Function


    Public Function IsActive(ByVal ctrl As Control) As Boolean

        For cnt As Integer = 0 To ctrl.Controls.Count - 1
            If TypeOf (ctrl.Controls(cnt)) Is GroupBox Then
                Return IsActive(ctrl.Controls(cnt))
            ElseIf TypeOf (ctrl.Controls(cnt)) Is Panel Then
                Return IsActive(ctrl.Controls(cnt))
            ElseIf TypeOf (ctrl.Controls(cnt)) Is SplitContainer Then
                Return IsActive(ctrl.Controls(cnt))
            ElseIf TypeOf (ctrl.Controls(cnt)) Is TabControl Then
                Return IsActive(ctrl.Controls(cnt))
            ElseIf TypeOf (ctrl.Controls(cnt)) Is TabPage Then
                Return IsActive(ctrl.Controls(cnt))
            ElseIf TypeOf (ctrl.Controls(cnt)) Is DataGridView Then
                Continue For
            ElseIf ctrl.Controls(cnt).Controls.Count > 0 Then
                Return IsActive(ctrl.Controls(cnt))
            Else
                If ctrl.Controls(cnt).Focused Then Return True
                'Else
                '    If ctrl.Focused Then Return True
            End If
        Next
        'End If
    End Function

#Region "Validator"
    ''TextBoxName  :CtrlTypeCtrlName_DataType_Mandatory_Length
    ''ComboBox     :CtrlTypeCtrlName_Mandatory

    Enum DataTypeControls
        Text = 0
        Number = 1
        Weight = 2
        Amount = 3
        Percentage = 4
    End Enum

    Public Function Validator_Check(ByVal f As Object) As Boolean
        For Each obj As Object In CType(f, Control).Controls
            If UCase(CType(obj, Control).Name).Contains("_OWN") Then Continue For
            If TypeOf obj Is TextBox Then
                Dim txt As TextBox = CType(obj, TextBox)
                Dim s() As String = txt.Name.Split("_")
                Dim mand As Boolean = False
                If UCase(txt.Name).Contains("_MAN") Then
                    mand = True
                End If
                'If s.Length - 1 >= 2 Then ''Mand
                '    If s(2).Length > 0 Then mand = True
                'End If
                If Not txt.Enabled Then Continue For
                Dim fieldCaption As String = Nothing
                Dim parentForm As Form = GetParentControl(txt)
                fieldCaption = LanguageChange.Get_LanguageCaption(G_LangId, parentForm, GetNextLable(parentForm, txt, False))
                If fieldCaption = Nothing Then
                    fieldCaption = GetNextLable(parentForm, txt, False).Text
                End If
                If mand Then
                    If Not txt.Text.Length > 0 Then
                        MsgBox(fieldCaption + E0001, MsgBoxStyle.Information) ''empty msg
                        txt.Focus()
                        Return True
                    End If
                End If
            ElseIf TypeOf obj Is ComboBox Then
                Dim cmb As ComboBox = CType(obj, ComboBox)
                If Not cmb.Enabled Then Continue For
                If Not cmb.DropDownStyle = ComboBoxStyle.DropDownList Then
                    Dim s() As String = cmb.Name.Split("_")
                    Dim field As String = Nothing
                    Dim Mand As Boolean = False
                    If UCase(cmb.Name).Contains("_MAN") Then
                        Mand = True
                    End If
                    'If s.Length - 1 >= 1 Then ''MAN
                    '    If s(1) <> Nothing Then Mand = True
                    'End If
                    Dim fieldCaption As String = Nothing
                    Dim parentForm As Form = GetParentControl(cmb)
                    If G_LangId <> "ENG" Then
                        fieldCaption = LanguageChange.Get_LanguageCaption(G_LangId, parentForm, GetNextLable(parentForm, cmb, False))
                    End If
                    If fieldCaption = Nothing Then
                        fieldCaption = GetNextLable(parentForm, cmb, False).Text
                    End If
                    If Mand Then
                        If Not cmb.Text.Length > 0 Then
                            MsgBox(fieldCaption + E0001, MsgBoxStyle.Information) ''empty msg
                            cmb.Focus()
                            Return True
                        End If
                    End If
                    If cmb.Items.Count > 0 Then
                        If (Not cmb.Items.Contains(cmb.Text)) And cmb.Text <> "" Then
                            MsgBox(E0004 + fieldCaption, MsgBoxStyle.Information)
                            cmb.Focus()
                            Return True
                        End If
                    End If
                End If
            ElseIf TypeOf obj Is GroupBox _
                 Or TypeOf obj Is Panel _
                 Or TypeOf obj Is TabPage _
                 Or TypeOf obj Is TabControl _
                 Or TypeOf obj Is SplitContainer _
                 Then
                If Validator_Check(obj) Then Return True
            Else
                If Validator_Check(obj) Then Return True
            End If
        Next
    End Function

    Public Sub Validator_Object(ByVal f As Object)
        For Each obj As Object In CType(f, Control).Controls
            If TypeOf obj Is BrighttechPack.DatePicker Then
                If UCase(CType(obj, Control).Name).Contains("_OWN") Then Continue For
                CType(obj, BrighttechPack.DatePicker).MinimumDate = G_CnTranFromDate
                CType(obj, BrighttechPack.DatePicker).MaximumDate = G_CnTranToDate
                CType(obj, BrighttechPack.DatePicker).Seperator = "-"
                CType(obj, BrighttechPack.DatePicker).Value = Today.Date
            ElseIf TypeOf obj Is TextBox Then
                If UCase(CType(obj, Control).Name).Contains("_OWN") Then Continue For
                Dim txt As TextBox = obj
                txt.CharacterCasing = G_TextCharacterCasing
                Dim s() As String = UCase(txt.Name).Split("_")
                If UCase(txt.Name).Contains("_NUM") _
                Or UCase(txt.Name).Contains("_WET") _
                Or UCase(txt.Name).Contains("_AMT") _
                Or UCase(txt.Name).Contains("_PER") Then
                    txt.TextAlign = HorizontalAlignment.Right
                End If
                If s.Length - 1 >= 3 Then ''length
                    If Val(s(3)) > 0 Then txt.MaxLength = s(3)
                End If
                AddHandler txt.GotFocus, AddressOf TextBox_Validator_GotFocus
                AddHandler txt.KeyPress, AddressOf TextBox_Validator_KeyPress
                AddHandler txt.LostFocus, AddressOf TextBox_Validator_LostFocus
            ElseIf TypeOf obj Is CheckedComboBox Then
                If UCase(CType(obj, Control).Name).Contains("_OWN") Then Continue For
                Dim cmb As ComboBox = CType(obj, ComboBox)
                AddHandler cmb.GotFocus, AddressOf ComboBox_Validator_Gotfous
                AddHandler cmb.LostFocus, AddressOf CheckedComboBox_Validator_Lostfocus
            ElseIf TypeOf obj Is ComboBox Then
                If UCase(CType(obj, Control).Name).Contains("_OWN") Then Continue For
                Dim cmb As ComboBox = CType(obj, ComboBox)
                AddHandler cmb.GotFocus, AddressOf ComboBox_Validator_Gotfous
                AddHandler cmb.LostFocus, AddressOf ComboBox_Validator_Lostfocus
                If Not cmb.DropDownStyle = ComboBoxStyle.DropDownList Then
                    AddHandler cmb.KeyPress, AddressOf ComboBox_Validator_KeyPress
                End If
                AddHandler cmb.EnabledChanged, AddressOf ComboBox_Validator_EnableChanged
            ElseIf TypeOf obj Is Button Then
                If UCase(CType(obj, Control).Name).Contains("_OWN") Then Continue For
                Dim btn As Button = CType(obj, Button)
                btn.BackColor = G_LostFocusColor
                btn.Name = UCase(btn.Name)
                If btn.Name.Contains("SAVE") Then
                    btn.Image = My.Resources.filesave_22
                    btn.TextImageRelation = TextImageRelation.ImageBeforeText
                ElseIf btn.Name.Contains("OPEN") Then
                    btn.Image = My.Resources.Ex_22
                    btn.TextImageRelation = TextImageRelation.ImageBeforeText
                ElseIf btn.Name.Contains("NEW") Then
                    btn.Image = My.Resources.Snowflake_22
                    btn.TextImageRelation = TextImageRelation.ImageBeforeText
                ElseIf btn.Name.Contains("EXIT") Then
                    btn.Image = My.Resources.exit_22
                    btn.TextImageRelation = TextImageRelation.ImageBeforeText
                ElseIf btn.Name.Contains("DELETE") Then
                    btn.Image = My.Resources.Remove_22
                    btn.TextImageRelation = TextImageRelation.ImageBeforeText
                ElseIf btn.Name.Contains("SEARCH") Then
                    btn.Image = My.Resources.Find_Search_22
                    btn.TextImageRelation = TextImageRelation.ImageBeforeText
                ElseIf btn.Name.Contains("OK") Then
                    btn.Image = My.Resources.Symbol___Check_22
                    btn.TextImageRelation = TextImageRelation.ImageBeforeText
                ElseIf btn.Name.Contains("EXCEL") Then
                    btn.Image = My.Resources.Excel_icon_22
                    btn.TextImageRelation = TextImageRelation.ImageBeforeText
                ElseIf btn.Name.Contains("PRINT") Then
                    btn.Image = My.Resources.Printer_22
                    btn.TextImageRelation = TextImageRelation.ImageBeforeText
                ElseIf btn.Name.Contains("ADD") Then
                    btn.Image = My.Resources.add_22
                    btn.TextImageRelation = TextImageRelation.ImageBeforeText
                End If
                If CType(obj, Button).Name.Contains("_OWN") = False Then
                    AddHandler CType(obj, Button).GotFocus, AddressOf Button_Validator_Gotfocus
                    AddHandler CType(obj, Button).LostFocus, AddressOf Button_Validator_Lostfocus
                End If
            ElseIf TypeOf obj Is DataGridView Then
                If UCase(CType(obj, Control).Name).Contains("_OWN") Then Continue For
                Dim grid As DataGridView = CType(obj, DataGridView)
                grid.BorderStyle = BorderStyle.None
                grid.DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.Window
                grid.DefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.ControlText
                grid.RowTemplate.Resizable = DataGridViewTriState.False
                grid.RowHeadersVisible = False
                grid.BackgroundColor = G_GrdBackGroundColor
                grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
                grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                grid.RowTemplate.Height = 18
                grid.MultiSelect = False
                grid.Font = New Font("VERDANA", 8, FontStyle.Regular)
                AddHandler grid.GotFocus, AddressOf DataGridView_Validator_Gotfocus
                AddHandler grid.LostFocus, AddressOf DataGridView_Validator_LostFocus
            ElseIf TypeOf obj Is DateTimePicker Then
                If UCase(CType(obj, Control).Name).Contains("_OWN") Then Continue For
                CType(obj, DateTimePicker).MinDate = G_CnTranFromDate
                CType(obj, DateTimePicker).MaxDate = G_CnTranToDate
            ElseIf TypeOf obj Is CheckedListBox Then
                If UCase(CType(obj, Control).Name).Contains("_OWN") Then Continue For
                AddHandler CType(obj, CheckedListBox).GotFocus, AddressOf CheckedListBox_Validator_Gotfocus
                AddHandler CType(obj, CheckedListBox).LostFocus, AddressOf CheckedListBox_Validator_LostFocus
            ElseIf TypeOf obj Is ListBox Then
                If UCase(CType(obj, Control).Name).Contains("_OWN") Then Continue For
                AddHandler CType(obj, ListBox).GotFocus, AddressOf ListBox_Validator_Gotfocus
                AddHandler CType(obj, ListBox).LostFocus, AddressOf ListBox_Validator_LostFocus
            ElseIf TypeOf obj Is RadioButton Then
                If UCase(CType(obj, Control).Name).Contains("_OWN") Then Continue For
                AddHandler CType(obj, RadioButton).GotFocus, AddressOf Radio_Check_Validator_Gotfocus
                AddHandler CType(obj, RadioButton).LostFocus, AddressOf Radio_Check_Validator_LostFocus
            ElseIf TypeOf obj Is CheckBox Then
                If UCase(CType(obj, Control).Name).Contains("_OWN") Then Continue For
                AddHandler CType(obj, CheckBox).GotFocus, AddressOf Radio_Check_Validator_Gotfocus
                AddHandler CType(obj, CheckBox).LostFocus, AddressOf Radio_Check_Validator_LostFocus
            ElseIf TypeOf obj Is GroupBox _
                Or TypeOf obj Is Panel _
                Or TypeOf obj Is TabPage _
                Or TypeOf obj Is TabControl _
                Or TypeOf obj Is SplitContainer _
                Then
                If CType(obj, Control).Name.Contains("_OWN") = False Then
                    CType(obj, Control).BackColor = GetParentControl(CType(f, Control)).BackColor
                End If
                Validator_Object(obj)
            Else
                Validator_Object(obj)
                ''If UCase(CType(obj, Control).Name).Contains("_OWN") Then Continue For
            End If
        Next
    End Sub

#Region "TextBox Validations"

    Private Sub TextBox_Validator_LostFocus(ByVal sender As Object, ByVal e As EventArgs)
        Dim txt As TextBox = sender
        txt.BackColor = G_LostFocusColor

        Dim s() As String = txt.Name.Split("_")
        Dim field As String = Nothing
        Dim mand As Boolean = False
        Dim length As Integer = Nothing

        Dim dataType As DataTypeControls
        If s Is Nothing Then
            Exit Sub
        End If
        If s.Length - 1 >= 0 Then ''ctlType ctlName

        End If
        If s.Length - 1 >= 1 Then ''Data
            Select Case UCase(s(1))
                Case "TXT"
                    dataType = DataTypeControls.Text
                Case "NUM"
                    dataType = DataTypeControls.Number
                Case "WET"
                    dataType = DataTypeControls.Weight
                Case "AMT"
                    dataType = DataTypeControls.Amount
                Case "PER"
                    dataType = DataTypeControls.Percentage
            End Select
        End If
        If dataType = DataTypeControls.Percentage Or dataType = DataTypeControls.Amount Then
            If G_Decimal = 3 And dataType = DataTypeControls.Amount Then
                txt.Text = IIf(Val(txt.Text) <> 0, Format(Val(txt.Text), "0.000"), txt.Text)
            Else
                txt.Text = IIf(Val(txt.Text) <> 0, Format(Val(txt.Text), "0.00"), txt.Text)
            End If
        ElseIf dataType = DataTypeControls.Weight Then
            txt.Text = IIf(Val(txt.Text) <> 0, Format(Val(txt.Text), "0.000"), txt.Text)
        End If
    End Sub

    Public Function txt_Keypress(ByVal sender As TextBox, ByVal dataType As DataTypeControls, ByVal e As KeyPressEventArgs) As Integer
        If e.KeyChar = Chr(Keys.Enter) Then Exit Function
        Dim preStr As String = Nothing ' sender.GetNextControl(sender, False).Text + vbCrLf
        If dataType = DataTypeControls.Number Then
            Select Case e.KeyChar
                Case "+", "-", "0" To "9", ChrW(Keys.Back), _
                    ChrW(Keys.Enter), ChrW(Keys.Escape)
                Case Else
                    e.Handled = True
                    MsgBox(preStr + "Digits only Allowed 0 to 9", MsgBoxStyle.Information)
                    sender.Focus()
            End Select
        ElseIf dataType = DataTypeControls.Weight Then
            If e.KeyChar = "." And sender.Text.Contains(".") Then
                e.Handled = True
                Exit Function
            End If
            Select Case e.KeyChar
                Case "+", "-", "0" To "9", ChrW(Keys.Back), ".", _
                ChrW(Keys.Enter), ChrW(Keys.Escape), ChrW(Keys.Space)
                Case Else
                    e.Handled = True
                    MsgBox(preStr + "Digits only Allowed 0 to 9", MsgBoxStyle.Information)
                    sender.Focus()
            End Select
            If sender.Text.Contains(".") Then
                Dim dotPos As Integer = InStr(sender.Text, ".", CompareMethod.Text)
                Dim sp() As String = sender.Text.Split(".")
                Dim curPos As Integer = sender.SelectionStart
                If sp.Length >= 2 Then
                    If curPos >= dotPos Then
                        If sp(1).Length > 2 Then
                            e.Handled = True
                        End If
                    End If
                End If
            End If
        ElseIf dataType = DataTypeControls.Amount Then
            If e.KeyChar = "." And sender.Text.Contains(".") Then
                e.Handled = True
                Exit Function
            End If
            Select Case e.KeyChar
                Case "+", "-", "0" To "9", ChrW(Keys.Back), ".", _
                ChrW(Keys.Enter), ChrW(Keys.Escape)
                Case Else
                    e.Handled = True
                    MsgBox(preStr + "Digits only Allowed 0 to 9", MsgBoxStyle.Information)
                    sender.Focus()
            End Select
            If CType(sender, TextBox).Text.Contains(".") Then
                Dim dotPos As Integer = InStr(CType(sender, TextBox).Text, ".", CompareMethod.Text)
                Dim sp() As String = CType(sender, TextBox).Text.Split(".")
                Dim curPos As Integer = CType(sender, TextBox).SelectionStart
                If sp.Length >= 2 Then
                    If curPos >= dotPos Then
                        If sp(1).Length > 1 Then
                            e.Handled = True
                        End If
                    End If
                End If
            End If
        ElseIf dataType = DataTypeControls.Percentage Then
            Dim value As String = sender.Text
            If e.KeyChar = "." And sender.Text.Contains(".") Or Val(sender.Text) = 100 Then
                e.Handled = True
                Exit Function
            End If
            Select Case e.KeyChar
                Case "+", "-", "0" To "9", ".", ChrW(Keys.Back), ChrW(Keys.Escape), ChrW(Keys.F10)
                    If sender.Text.Contains(".") Then
                        Dim sp() As String = sender.Text.Split(".")
                        If sp.Length >= 2 Then
                            If sender.SelectionStart >= InStr(sender.Text, ".", CompareMethod.Text) Then
                                If sp(1).Length > 1 Then
                                    e.Handled = True
                                    Exit Function
                                End If
                            End If
                        End If
                    End If
                    value += e.KeyChar
                    If Val(value) > 100 Then
                        e.Handled = True
                    End If
                Case Else
                    e.Handled = True
                    MsgBox(preStr + "Digits only Allowed 0 to 9", MsgBoxStyle.Information)
                    sender.Focus()
            End Select
        End If
    End Function

    Private Sub TextBox_Validator_GotFocus(ByVal sender As Object, ByVal e As EventArgs)
        Dim txt As TextBox = CType(sender, TextBox)
        If txt.Focused Then
            txt.BackColor = G_FocusColor
            txt.SelectAll()
        End If
    End Sub

    Private Sub TextBox_Validator_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = "'" Then
            e.Handled = True
            Exit Sub
        End If
        Dim txt As TextBox = sender
        Dim s() As String = txt.Name.Split("_")
        Dim mand As Boolean = False
        Dim length As Integer = Nothing

        Dim dataType As DataTypeControls
        If s Is Nothing Then
            Exit Sub
        End If
        If s.Length - 1 >= 0 Then ''ctlType and Name

        End If
        If UCase(txt.Name).Contains("_MAN") Then
            mand = True
        End If
        If UCase(txt.Name).Contains("_TXT") Then
            dataType = DataTypeControls.Text
        ElseIf UCase(txt.Name).Contains("_NUM") Then
            dataType = DataTypeControls.Number
        ElseIf UCase(txt.Name).Contains("_WET") Then
            dataType = DataTypeControls.Weight
        ElseIf UCase(txt.Name).Contains("_AMT") Then
            dataType = DataTypeControls.Amount
        ElseIf UCase(txt.Name).Contains("_PER") Then
            dataType = DataTypeControls.Percentage
        End If
        'If s.Length - 1 >= 1 Then ''Datatype
        '    Select Case UCase(s(1))
        '        Case "TXT"
        '            dataType = DataTypeControls.Text
        '        Case "NUM"
        '            dataType = DataTypeControls.Number
        '        Case "WET"
        '            dataType = DataTypeControls.Weight
        '        Case "AMT"
        '            dataType = DataTypeControls.Amount
        '        Case "PER"
        '            dataType = DataTypeControls.Percentage
        '    End Select
        'Else
        '    Exit Sub
        'End If
        'If s.Length - 1 >= 2 Then ''Mand
        '    If s(2).Length > 0 Then mand = True
        'End If
        If s.Length - 1 >= 3 Then ''length
            ''length = s(4)
        End If
        If e.KeyChar = Chr(Keys.Enter) Then
            If UCase(txt.Name).Contains("_") = False Then Exit Sub
            Dim fieldCaption As String = Nothing
            Dim parentForm As Form = GetParentControl(txt)
            fieldCaption = LanguageChange.Get_LanguageCaption(G_LangId, parentForm, GetNextLable(parentForm, txt, False))
            If fieldCaption = Nothing Then
                fieldCaption = GetNextLable(parentForm, txt, False).Text
            End If
            If mand Then
                If Not txt.Text.Length > 0 Then
                    MsgBox(fieldCaption + E0001, MsgBoxStyle.Information) ''empty msg
                    txt.Focus()
                    Exit Sub
                End If
            End If
        ElseIf (Not dataType = DataTypeControls.Text) And e.KeyChar <> Chr(Keys.Back) Then
            txt_Keypress(sender, dataType, e)
        End If
    End Sub
#End Region

#Region "Combox Validations"

    Private Sub ComboBox_Validator_Gotfous(ByVal sender As Object, ByVal e As EventArgs)
        Dim cmb As ComboBox = CType(sender, ComboBox)
        If cmb.Focused Then cmb.BackColor = G_FocusColor
        If cmb.DropDownStyle = ComboBoxStyle.DropDown And cmb.Items.Count = 1 And cmb.Text = "" Then
            If cmb.DataSource Is Nothing Then
                cmb.Text = cmb.Items(0)
                cmb.SelectAll()
            End If
        End If
    End Sub

    Private Sub ComboBox_Validator_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If G_TextCharacterCasing = CharacterCasing.Upper Then e.KeyChar = UCase(e.KeyChar)

        If e.KeyChar = "'" Then
            e.Handled = True
            Exit Sub
        End If
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim cmb As ComboBox = CType(sender, ComboBox)
            Dim s() As String = cmb.Name.Split("_")
            Dim field As String = Nothing
            Dim Mand As Boolean = False
            If s Is Nothing Then
                Exit Sub
            End If
            If s.Length - 1 >= 0 Then ''ctlType ctlName

            End If
            If s.Length - 1 >= 1 Then ''MAN
                If s(1) <> Nothing Then Mand = True
            End If
            Dim fieldCaption As String = Nothing
            Dim parentForm As Form = GetParentControl(cmb)
            If G_LangId <> "ENG" Then
                fieldCaption = LanguageChange.Get_LanguageCaption(G_LangId, parentForm, GetNextLable(parentForm, cmb, False)).Replace("&", "")
            End If
            If fieldCaption = Nothing Then
                fieldCaption = GetNextLable(parentForm, cmb, False).Text.Replace("&", "")
            End If

            If Mand Then
                If Not cmb.Text.Length > 0 Then
                    MsgBox(fieldCaption + E0001, MsgBoxStyle.Information) ''empty msg
                    cmb.Focus()
                    Exit Sub
                End If
            End If

            If cmb.Text <> "" Then
                Exit Sub
            End If

            If (Not cmb.Items.Contains(cmb.Text)) Or cmb.Text <> "" Then
                MsgBox(E0004 + fieldCaption, MsgBoxStyle.Information)
                cmb.Focus()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub CheckedComboBox_Validator_Lostfocus(ByVal sender As Object, ByVal e As EventArgs)
        Dim cmb As CheckedComboBox = CType(sender, CheckedComboBox)
        cmb.BackColor = G_LostFocusColor
        If cmb.Text.ToUpper.StartsWith("ALL,") And cmb.Text.ToUpper.Contains(",") Then
            For cnt As Integer = 0 To cmb.Items.Count - 1
                If cmb.Items(cnt).ToString <> "ALL" Then
                    cmb.SetItemChecked(cnt, False)
                End If
            Next
        End If
    End Sub


    Private Sub ComboBox_Validator_Lostfocus(ByVal sender As Object, ByVal e As EventArgs)
        Dim cmb As ComboBox = CType(sender, ComboBox)
        cmb.BackColor = G_LostFocusColor
    End Sub

    Private Sub ComboBox_Validator_EnableChanged(ByVal sender As Object, ByVal e As EventArgs)
        CType(sender, ComboBox).BackColor = Color.White
    End Sub
#End Region

#Region "Button Validations"

    Private Sub Button_Validator_Gotfocus(ByVal sender As Object, ByVal e As EventArgs)
        If CType(sender, Button).Focused Then CType(sender, Button).BackColor = G_FocusColor
    End Sub

    Private Sub Button_Validator_Lostfocus(ByVal sender As Object, ByVal e As EventArgs)

        CType(sender, Button).BackColor = G_LostFocusColor
    End Sub

#End Region

#Region "DataGridView Validations"

    Private Sub DataGridView_Validator_Gotfocus(ByVal sender As Object, ByVal e As EventArgs)
        If Not CType(sender, DataGridView).RowCount > 0 Then
            CType(sender, DataGridView).Parent.SelectNextControl(CType(sender, DataGridView), False, True, True, True)
            Exit Sub
        End If
        CType(sender, DataGridView).DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight
        CType(sender, DataGridView).DefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText
    End Sub

    Private Sub DataGridView_Validator_LostFocus(ByVal sender As Object, ByVal e As EventArgs)
        CType(sender, DataGridView).DefaultCellStyle.SelectionBackColor = CType(sender, DataGridView).DefaultCellStyle.BackColor ''System.Drawing.SystemColors.Window
        CType(sender, DataGridView).DefaultCellStyle.SelectionForeColor = CType(sender, DataGridView).DefaultCellStyle.ForeColor  ''System.Drawing.SystemColors.ControlText

    End Sub

#End Region

#Region "CheckListBox Validations"

    Private Sub CheckedListBox_Validator_Gotfocus(ByVal sender As Object, ByVal e As EventArgs)
        If CType(sender, CheckedListBox).Focused Then CType(sender, CheckedListBox).BackColor = G_FocusColor
    End Sub

    Private Sub CheckedListBox_Validator_LostFocus(ByVal sender As Object, ByVal e As EventArgs)
        CType(sender, CheckedListBox).BackColor = G_LostFocusColor
        CType(sender, CheckedListBox).ClearSelected()
    End Sub

#End Region

#Region "ListBox Validations"

    Private Sub ListBox_Validator_Gotfocus(ByVal sender As Object, ByVal e As EventArgs)
        If CType(sender, ListBox).Focused Then CType(sender, ListBox).BackColor = G_FocusColor
    End Sub

    Private Sub ListBox_Validator_LostFocus(ByVal sender As Object, ByVal e As EventArgs)
        CType(sender, ListBox).BackColor = G_LostFocusColor
    End Sub

#End Region

#Region "Radio_Check Button Validations"

    Private Sub Radio_Check_Validator_Gotfocus(ByVal sender As Object, ByVal e As EventArgs)
        If CType(sender, Control).Focused = False Then Exit Sub
        If TypeOf sender Is RadioButton Then
            CType(sender, RadioButton).BackColor = G_FocusColor
        ElseIf TypeOf sender Is CheckBox Then
            CType(sender, CheckBox).BackColor = G_FocusColor
        End If
    End Sub

    Private Sub Radio_Check_Validator_LostFocus(ByVal sender As Object, ByVal e As EventArgs)
        If TypeOf sender Is RadioButton Then
            CType(sender, RadioButton).BackColor = Color.Transparent
        ElseIf TypeOf sender Is CheckBox Then
            CType(sender, CheckBox).BackColor = Color.Transparent
        End If
    End Sub

#End Region

    Public Function GetParentControl(ByVal f As Control) As Form
        If TypeOf f Is Form Then
            Return f
        Else
            Return GetParentControl(f.Parent)
        End If
        Return Nothing
    End Function

    Public Function GetNextLable(ByVal frm As Form, ByVal f As Control, ByVal forward As Boolean) As Label
        If TypeOf frm.GetNextControl(f, forward) Is Label Then
            Return frm.GetNextControl(f, forward)
        Else
            Return GetNextLable(frm, frm.GetNextControl(f, forward), forward)
        End If
    End Function
#End Region

    Public Shared Function GetRights(ByVal dtRights As DataTable, ByVal frmName As String, ByVal RightMod As RightMode, Optional ByVal DispMsg As Boolean = True) As Boolean
        Dim filt As String = Nothing
        Select Case RightMod
            Case RightMode.Save
                filt = "_ADD = 'Y'"
            Case RightMode.Edit
                filt = "_EDIT = 'Y'"
            Case RightMode.View
                filt = "_VIEW = 'Y'"
            Case RightMode.Delete
                filt = "_DEL = 'Y'"
            Case RightMode.Excel
                filt = "_EXCEL = 'Y'"
            Case RightMode.Print
                filt = "_PRINT = 'Y'"
            Case RightMode.Authorize
                filt = "_AUTHORIZE = 'Y'"
            Case RightMode.MiscIssue
                filt = "_MISCISSUE = 'Y'"

            Case RightMode.Cancel
                filt = "_CANCEL = 'Y'"
            Case RightMode.Sale
                filt = "_SALE= 'Y'"
            Case RightMode.Qsale
                filt = "_QSALE= 'Y'"
            Case RightMode.Purchase
                filt = "_PURCHASE= 'Y'"
            Case RightMode.SReturn
                filt = "_RETURN= 'Y'"
        End Select
        Dim ro() As DataRow = dtRights.Select("ACCESSID LIKE '" & frmName & "%' AND " & filt)

        If Not ro.Length > 0 Then
            If DispMsg Then MsgBox("Access Denied", MsgBoxStyle.Information)
            Return False
        End If
        Return True
    End Function


    Public Sub TextClear(ByVal f As Object)
        For Each obj As Object In CType(f, Control).Controls
            If TypeOf obj Is BrighttechPack.DatePicker Then

            ElseIf TypeOf obj Is TextBox Then
                Dim txt As TextBox = obj
                txt.Clear()
            ElseIf TypeOf obj Is ComboBox Then
                Dim cmb As ComboBox = obj
                If cmb.DropDownStyle <> ComboBoxStyle.DropDownList Then cmb.Text = ""
            ElseIf TypeOf obj Is GroupBox _
                Or TypeOf obj Is Panel _
                Or TypeOf obj Is TabPage _
                Or TypeOf obj Is TabControl _
                Or TypeOf obj Is SplitContainer _
                Then
                TextClear(obj)
            Else
                TextClear(obj)
            End If
        Next
    End Sub
#Region "Convert Rupees"
    ' Function for conversion of a Indian Rupees into words
    '   Parameter - accept a Currency
    '   Returns the number in words format
    '   You can use this function in Excel, VBA, VB6,.NET
    '====================================================

    '****************************************************
    ' Code Created by Bharat Modha 
    ' Porbandar (Gujarat)-India
    ' Email : bharatmodha@yahoo.com
    '****************************************************

    Public Function RupeesToWord(ByVal MyNumber)
        Dim Temp
        Dim Rupees As String = Nothing, Paisa As String = Nothing
        Dim DecimalPlace, iCount
        Dim Hundreds As String, Words As String = Nothing
        Dim place(9) As String
        place(0) = " Thousand "
        place(2) = " Lakh "
        place(4) = " Crore "
        place(6) = " Arab  "
        place(8) = " Kharab  "
        On Error Resume Next
        ' Convert MyNumber to a string, trimming extra spaces.
        MyNumber = Trim(Str(MyNumber))

        ' Find decimal place.
        DecimalPlace = InStr(MyNumber, ".")

        ' If we find decimal place...
        If DecimalPlace > 0 Then
            ' Convert Paisa
            Temp = Left(Mid(MyNumber, DecimalPlace + 1) & "00", 2)
            Paisa = " and " & ConvertTens(Temp) & " Paisa"

            ' Strip off paisa from remainder to convert.
            MyNumber = Trim(Left(MyNumber, DecimalPlace - 1))
        End If

        '===============================================================
        Dim TM As String  ' If MyNumber between Rs.1 To 99 Only.
        TM = Right(MyNumber, 2)

        If Len(MyNumber) > 0 And Len(MyNumber) <= 2 Then
            If Len(TM) = 1 Then
                Words = ConvertDigit(TM)
                RupeesToWord = "Rupees " & Words & Paisa & " Only"

                Exit Function

            Else
                If Len(TM) = 2 Then
                    Words = ConvertTens(TM)
                    RupeesToWord = "Rupees " & Words & Paisa & " Only"
                    Exit Function

                End If
            End If
        End If
        '===============================================================


        ' Convert last 3 digits of MyNumber to ruppees in word.
        Hundreds = ConvertHundreds(Right(MyNumber, 3))
        ' Strip off last three digits
        MyNumber = Left(MyNumber, Len(MyNumber) - 3)

        iCount = 0
        Do While MyNumber <> ""
            'Strip last two digits
            Temp = Right(MyNumber, 2)
            If Len(MyNumber) = 1 Then


                If Trim(Words) = "Thousand" Or _
                Trim(Words) = "Lakh  Thousand" Or _
                Trim(Words) = "Lakh" Or _
                Trim(Words) = "Crore" Or _
                Trim(Words) = "Crore  Lakh  Thousand" Or _
                Trim(Words) = "Arab   Crore  Lakh  Thousand" Or _
                Trim(Words) = "Arab " Or _
                Trim(Words) = "Kharab   Arab   Crore  Lakh  Thousand" Or _
                Trim(Words) = "Kharab " Then

                    Words = ConvertDigit(Temp) & place(iCount)
                    MyNumber = Left(MyNumber, Len(MyNumber) - 1)

                Else

                    Words = ConvertDigit(Temp) & place(iCount) & Words
                    MyNumber = Left(MyNumber, Len(MyNumber) - 1)

                End If
            Else

                If Trim(Words) = "Thousand" Or _
                   Trim(Words) = "Lakh  Thousand" Or _
                   Trim(Words) = "Lakh" Or _
                   Trim(Words) = "Crore" Or _
                   Trim(Words) = "Crore  Lakh  Thousand" Or _
                   Trim(Words) = "Arab   Crore  Lakh  Thousand" Or _
                   Trim(Words) = "Arab " Then


                    Words = ConvertTens(Temp) & place(iCount)


                    MyNumber = Left(MyNumber, Len(MyNumber) - 2)
                Else

                    '=================================================================
                    ' if only Lakh, Crore, Arab , Kharab 

                    If Trim(ConvertTens(Temp) & place(iCount)) = "Lakh" Or _
                       Trim(ConvertTens(Temp) & place(iCount)) = "Crore" Or _
                       Trim(ConvertTens(Temp) & place(iCount)) = "Arab " Then

                        Words = Words
                        MyNumber = Left(MyNumber, Len(MyNumber) - 2)
                    Else
                        Words = ConvertTens(Temp) & place(iCount) & Words
                        MyNumber = Left(MyNumber, Len(MyNumber) - 2)
                    End If

                End If
            End If

            iCount = iCount + 2
        Loop

        RupeesToWord = "Rupees " & Words & Hundreds & Paisa & " Only"
    End Function

    ' Conversion for hundreds
    '*****************************************

    Private Function ConvertHundreds(ByVal MyNumber) As Integer
        Dim Result As String = Nothing

        ' Exit if there is nothing to convert.
        If Val(MyNumber) = 0 Then Exit Function

        ' Append leading zeros to number.
        MyNumber = Right("000" & MyNumber, 3)

        ' Do we have a hundreds place digit to convert?
        If Left(MyNumber, 1) <> "0" Then
            Result = ConvertDigit(Left(MyNumber, 1)) & " Hundreds "
        End If

        ' Do we have a tens place digit to convert?
        If Mid(MyNumber, 2, 1) <> "0" Then
            Result = Result & ConvertTens(Mid(MyNumber, 2))
        Else
            ' If not, then convert the ones place digit.
            Result = Result & ConvertDigit(Mid(MyNumber, 3))
        End If

        ConvertHundreds = Trim(Result)
    End Function

    ' Conversion for tens
    '*****************************************

    Private Function ConvertTens(ByVal MyTens)
        Dim Result As String = Nothing

        ' Is value between 10 and 19?
        If Val(Left(MyTens, 1)) = 1 Then
            Select Case Val(MyTens)
                Case 10 : Result = "Ten"
                Case 11 : Result = "Eleven"
                Case 12 : Result = "Twelve"
                Case 13 : Result = "Thirteen"
                Case 14 : Result = "Fourteen"
                Case 15 : Result = "Fifteen"
                Case 16 : Result = "Sixteen"
                Case 17 : Result = "Seventeen"
                Case 18 : Result = "Eighteen"
                Case 19 : Result = "Nineteen"
                Case Else
            End Select
        Else
            ' .. otherwise it's between 20 and 99.
            Select Case Val(Left(MyTens, 1))
                Case 2 : Result = "Twenty "
                Case 3 : Result = "Thirty "
                Case 4 : Result = "Forty "
                Case 5 : Result = "Fifty "
                Case 6 : Result = "Sixty "
                Case 7 : Result = "Seventy "
                Case 8 : Result = "Eighty "
                Case 9 : Result = "Ninety "
                Case Else
            End Select

            ' Convert ones place digit.
            Result = Result & ConvertDigit(Right(MyTens, 1))
        End If

        ConvertTens = Result
    End Function

    Private Function ConvertDigit(ByVal MyDigit)
        Select Case Val(MyDigit)
            Case 1 : ConvertDigit = "One"
            Case 2 : ConvertDigit = "Two"
            Case 3 : ConvertDigit = "Three"
            Case 4 : ConvertDigit = "Four"
            Case 5 : ConvertDigit = "Five"
            Case 6 : ConvertDigit = "Six"
            Case 7 : ConvertDigit = "Seven"
            Case 8 : ConvertDigit = "Eight"
            Case 9 : ConvertDigit = "Nine"
            Case Else : ConvertDigit = ""
        End Select
    End Function
#End Region

    'Public Shared Function Decrypt_old(ByVal Pwd As String) As String
    '    Dim strDecryptPwd As String = Nothing
    '    Try
    '        For cnt As Integer = 1 To Len(Pwd)
    '            Dim IntAscii As Integer = 0
    '            IntAscii = (Asc(Strings.Mid(Pwd, cnt, 1)) - (cnt * 2) - 14)
    '            strDecryptPwd = strDecryptPwd & Chr(IntAscii)
    '        Next
    '    Catch ex As Exception
    '        MsgBox("ERROR :" & ex.Message & vbCrLf & vbCrLf & "POSITION :" & ex.StackTrace)
    '    End Try
    '    Return strDecryptPwd
    'End Function


    'Public Shared Function Decrypt_old(Pwd As String) As String
    '    Dim strDecryptPwd As String = Nothing
    '    Try
    '        For cnt As Integer = 1 To Len(Pwd)
    '            Dim IntAscii As Integer = 0
    '            IntAscii = (Asc(Strings.Mid(Pwd, cnt, 1)) - (cnt * 2) - 14)
    '            strDecryptPwd = strDecryptPwd & Chr(IntAscii)
    '        Next
    '    Catch ex As Exception
    '        MsgBox("ERROR :" & ex.Message & vbCrLf & vbCrLf & "POSITION :" & ex.StackTrace)
    '    End Try
    '    Return strDecryptPwd
    'End Function


    'Public Shared Function Encrypt_old(ByVal Pwd As String) As String
    '    Dim strEncryptPwd As String = Nothing
    '    Try
    '        For cnt As Integer = 1 To Len(Pwd)
    '            Dim IntAscii As Integer = 0
    '            IntAscii = (Asc(Strings.Mid(Pwd, cnt, 1)) + (cnt * 2) + 14)
    '            strEncryptPwd = strEncryptPwd & Chr(IntAscii)
    '        Next
    '    Catch ex As Exception
    '        MsgBox("ERROR :" & ex.Message & vbCrLf & vbCrLf & "POSITION :" & ex.StackTrace)
    '    End Try
    '    Return strEncryptPwd
    'End Function


    Public Shared Function Decrypt(Str_Pwd As String) As String
        Dim vv As String = ""
        Dim x As Integer = 0
        For k As Integer = 0 To Str_Pwd.Length - 1
            Dim vIn As Char = Str_Pwd.Substring(x, 1)
            Dim vOut As String = vIn.ToString(vIn)
            Dim Byte_Data As Byte   ' For Reading Data
            Byte_Data = Asc(vOut)
            Byte_Data = Byte_Data - 23
            Byte_Data = Not Byte_Data
            vv += Chr(Byte_Data)
            x += 1
        Next
        Return vv
    End Function

    Public Shared Function Encrypt(Str_Pwd As String) As String
        Dim Str_Pass As String = String.Empty
        Dim Lng_Pos As Long     ' For Iterating through each byte
        Dim Byte_Data As Byte   ' For Reading Data
        Dim Byte_DataC As Byte  ' For storing Complemented (or encrypted) Byte_Data
        Dim Int_I As Integer

        Try
            Lng_Pos = 1
            For Int_I = 0 To Len(Str_Pwd) - 1
                Byte_Data = Asc(Mid(Str_Pwd, Lng_Pos, 1))
                Byte_DataC = Not (Byte_Data)                    'Encryption of the Data
                If Not Byte_DataC = 255 Then
                    Str_Pass = Str_Pass & Chr(Byte_DataC + 23)  'Writing Encrypted Data
                End If
                Lng_Pos = Lng_Pos + 1
            Next
            Encrypt = Str_Pass.ToString
        Catch ex As Exception
            Encrypt = ""
        End Try
    End Function

    Public Function Encrypt(Str_Pwd As String, Optional opt As Boolean = True) As String
        Dim Str_Pass As String = String.Empty
        Dim Lng_Pos As Long     ' For Iterating through each byte
        Dim Byte_Data As Byte   ' For Reading Data
        Dim Byte_DataC As Byte  ' For storing Complemented (or encrypted) Byte_Data
        Dim Int_I As Integer

        Try
            Lng_Pos = 1
            For Int_I = 0 To Len(Str_Pwd) - 1
                Byte_Data = Asc(Mid(Str_Pwd, Lng_Pos, 1))
                Byte_DataC = Not (Byte_Data)                    'Encryption of the Data
                If Not Byte_DataC = 255 Then
                    Str_Pass = Str_Pass & Chr(Byte_DataC + 23)  'Writing Encrypted Data
                End If
                Lng_Pos = Lng_Pos + 1
            Next
            Encrypt = Str_Pass.ToString
        Catch ex As Exception
            Encrypt = ""
        End Try
    End Function

    Public Shared Function EncryptNew(clearText As String) As String
        'Dim EncryptionKey As String = "MAKV2SPBNI99212"
        Dim EncryptionKey As String = "AKSHAYA2020"
        Dim clearBytes As Byte() = Encoding.Unicode.GetBytes(clearText)
        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D,
         &H65, &H64, &H76, &H65, &H64, &H65,
         &H76})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write)
                    cs.Write(clearBytes, 0, clearBytes.Length)
                    cs.Close()
                End Using
                clearText = Convert.ToBase64String(ms.ToArray())
            End Using
        End Using
        Return clearText
    End Function

    Public Shared Function DecryptNew(cipherText As String) As String
        'Dim EncryptionKey As String = "MAKV2SPBNI99212"
        Dim EncryptionKey As String = "AKSHAYA2020"
        Dim cipherBytes As Byte() = Convert.FromBase64String(cipherText)
        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D,
         &H65, &H64, &H76, &H65, &H64, &H65,
         &H76})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write)
                    cs.Write(cipherBytes, 0, cipherBytes.Length)
                    cs.Close()
                End Using
                cipherText = Encoding.Unicode.GetString(ms.ToArray())
            End Using
        End Using
        Return cipherText
    End Function


    Public Shared Function DecryptXml(ByVal Pwd As String) As String
        Dim strDecryptPwd As String = Nothing
        Try
            For cnt As Integer = 1 To Len(Pwd)
                Dim IntAscii As Integer = 0
                IntAscii = (AscW(Strings.Mid(Pwd, cnt, 1)) - (cnt * 2) - 14)
                strDecryptPwd = strDecryptPwd & ChrW(IntAscii)
            Next
        Catch ex As Exception
            MsgBox("ERROR :" & ex.Message & vbCrLf & vbCrLf & "POSITION :" & ex.StackTrace)
        End Try
        Return strDecryptPwd
    End Function
    Public Shared Function EncryptXml(ByVal Pwd As String) As String
        Dim strEncryptPwd As String = Nothing
        Try
            For cnt As Integer = 1 To Len(Pwd)
                Dim IntAscii As Integer = 0
                IntAscii = (AscW(Strings.Mid(Pwd, cnt, 1)) + (cnt * 2) + 14)
                strEncryptPwd = strEncryptPwd & ChrW(IntAscii)
            Next
        Catch ex As Exception
            MsgBox("ERROR :" & ex.Message & vbCrLf & vbCrLf & "POSITION :" & ex.StackTrace)
        End Try
        Return strEncryptPwd
    End Function

End Class
