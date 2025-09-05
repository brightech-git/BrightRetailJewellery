Imports System.Data.OleDb
Public Module GlobalMethods
    Public Enum SyncMode
        Master = 0
        Stock = 1
        Transaction = 2
    End Enum

    Public Function GetSqlValue(ByVal cn As OleDbConnection, ByVal qry As String, Optional ByVal field As String = Nothing, Optional ByVal defValue As String = "", Optional ByRef tran As OleDbTransaction = Nothing) As String
        G_DTable = New DataTable
        If tran Is Nothing Then
            G_DAdapter = New OleDbDataAdapter(qry, cn)
            G_DAdapter.Fill(G_DTable)
        Else
            G_Cmd = New OleDbCommand(qry, cn, tran)
            G_DAdapter = New OleDbDataAdapter(G_Cmd)
            G_DAdapter.Fill(G_DTable)
        End If
        If G_DTable.Rows.Count > 0 Then Return G_DTable.Rows(0).Item(0).ToString
        Return defValue
    End Function
    Public Function GetServerDate(ByVal Cn As OleDbConnection, Optional ByVal tran As OleDbTransaction = Nothing) As Date
        Dim dd As Date = CType(GetSqlValue(Cn, "SELECT CONVERT(SMALLDATETIME,CONVERT(VARCHAR(12),GETDATE(),101))", , , tran), Date)
        Return Format(dd, "yyyy-MM-dd")
    End Function

    Public Function GetServerTime(ByVal Cn As OleDbConnection, Optional ByVal tran As OleDbTransaction = Nothing) As Date
        Return CType(GetSqlValue(Cn, "SELECT CONVERT(SMALLDATETIME,CONVERT(VARCHAR(12),GETDATE(),108))", , , tran), Date)
    End Function
    Public Sub DropTempTables(ByVal cn As OleDbConnection)
        Dim strSql As String = Nothing
        strSql = " DECLARE @QRY NVARCHAR(4000)"
        strSql += " DECLARE @TNAME VARCHAR(50)"
        strSql += " DECLARE CUR CURSOR"
        strSql += " FOR SELECT NAME FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME LIKE 'TEMP%'"
        strSql += " OPEN CUR"
        strSql += " WHILE 1=1"
        strSql += " BEGIN"
        strSql += " FETCH NEXT FROM CUR INTO @TNAME"
        strSql += " SET NOCOUNT ON"
        strSql += " 	IF @@FETCH_STATUS = -1 BREAK"
        strSql += " SELECT @QRY = 'DROP TABLE ' + @TNAME"
        strSql += " EXEC SP_EXECUTESQL @QRY"
        strSql += " END"
        strSql += " CLOSE CUR"
        strSql += " DEALLOCATE CUR"
        Dim cmd As OleDbCommand
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
    End Sub
    Public Sub ComboScript(ByRef cmb As ComboBox, ByVal e As System.Windows.Forms.KeyEventArgs)
        Dim sComboText As String = ""
        Dim iLoop As Integer
        Dim sTempString As String
        If e.KeyCode >= 65 And e.KeyCode <= 90 Then
            'only look at letters A-Z
            sTempString = cmb.Text
            If Len(sTempString) = 1 Then sComboText = sTempString
            For iLoop = 0 To (cmb.Items.Count - 1)
                If UCase((sTempString & Mid$(cmb.Items.Item(iLoop), _
                  Len(sTempString) + 1))) = UCase(cmb.Items.Item(iLoop)) Then
                    cmb.SelectedIndex = iLoop
                    cmb.Text = cmb.Items.Item(iLoop)
                    cmb.SelectionStart = Len(sTempString)
                    cmb.SelectionLength = Len(cmb.Text) - (Len(sTempString))
                    sComboText = sComboText & Mid$(sTempString, Len(sComboText) + 1)
                    Exit For
                Else
                    If InStr(UCase(sTempString), UCase(sComboText)) Then
                        sComboText = sComboText & Mid$(sTempString, Len(sComboText) _
                        + 1)
                        cmb.Text = sComboText
                        cmb.SelectionStart = Len(cmb.Text)
                    Else
                        sComboText = sTempString
                    End If
                End If
            Next iLoop
        End If
    End Sub
    Public Sub FormatGridColumns(ByVal grid As DataGridView, Optional ByVal ColFormat As Boolean = False, Optional ByVal ColReadOnly As Boolean = True, Optional ByVal ColSort As Boolean = False)
        With grid
            For i As Integer = 0 To .ColumnCount - 1
                .Columns(i).ReadOnly = ColReadOnly
                If .Columns(i).ValueType IsNot Nothing Then
                    If .Columns(i).ValueType.Name Is GetType(Decimal).Name Then
                        If ColFormat Then .Columns(i).DefaultCellStyle.Format = "0.000"
                        .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    ElseIf .Columns(i).ValueType.Name Is GetType(Double).Name Then
                        If ColFormat Then .Columns(i).DefaultCellStyle.Format = "0.00"
                        .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    ElseIf .Columns(i).ValueType.Name Is GetType(Integer).Name Then
                        .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    ElseIf .Columns(i).ValueType.Name Is GetType(Date).Name Then
                        If ColFormat Then .Columns(i).DefaultCellStyle.Format = "dd/MM/yyyy"
                    End If
                End If
                If Not ColSort Then .Columns(i).SortMode = DataGridViewColumnSortMode.NotSortable
            Next
        End With
    End Sub
    Public Function Decrypt_old(ByVal Pwd As String) As String
        Dim strDecryptPwd As String = Nothing
        Try
            For cnt As Integer = 1 To Len(Pwd)
                Dim IntAscii As Integer = 0
                IntAscii = (Asc(Strings.Mid(Pwd, cnt, 1)) - (cnt * 2) - 14)
                strDecryptPwd = strDecryptPwd & Chr(IntAscii)
            Next
        Catch ex As Exception
            MsgBox("ERROR :" & ex.Message & vbCrLf & vbCrLf & "POSITION :" & ex.StackTrace)
        End Try
        Return strDecryptPwd
    End Function
    Public Function Encrypt_old(ByVal Pwd As String) As String
        Dim strEncryptPwd As String = Nothing
        Try
            For cnt As Integer = 1 To Len(Pwd)
                Dim IntAscii As Integer = 0
                IntAscii = (Asc(Strings.Mid(Pwd, cnt, 1)) + (cnt * 2) + 14)
                strEncryptPwd = strEncryptPwd & Chr(IntAscii)
            Next
        Catch ex As Exception
            MsgBox("ERROR :" & ex.Message & vbCrLf & vbCrLf & "POSITION :" & ex.StackTrace)
        End Try
        Return strEncryptPwd
    End Function
    Public Function Decrypt(Str_Pwd As String) As String
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

    Public Function Encrypt(Str_Pwd As String) As String
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
            Encrypt = Str_Pass
        Catch ex As Exception
            Encrypt = ""
        End Try
    End Function
    Public Function DecryptStr(ByVal Pwd As String) As String
        Dim strDecryptPwd As String = Nothing
        Try
            For cnt As Integer = 1 To Len(Pwd)
                Dim IntAscii As Integer = 0
                IntAscii = (Asc(Strings.Mid(Pwd, cnt, 1)) - 2)
                strDecryptPwd = strDecryptPwd & ChrW(IntAscii)
            Next
        Catch ex As Exception
            MsgBox("ERROR :" & ex.Message & vbCrLf & vbCrLf & "POSITION :" & ex.StackTrace)
        End Try
        Return strDecryptPwd
    End Function
    Public Function EncryptStr(ByVal Pwd As String) As String
        Dim strEncryptPwd As String = Nothing
        Try
            For cnt As Integer = 1 To Len(Pwd)
                Dim IntAscii As Integer = 0
                IntAscii = (Asc(Strings.Mid(Pwd, cnt, 1)) + 2)
                strEncryptPwd = strEncryptPwd & ChrW(IntAscii)
            Next
        Catch ex As Exception
            MsgBox("ERROR :" & ex.Message & vbCrLf & vbCrLf & "POSITION :" & ex.StackTrace)
        End Try
        Return strEncryptPwd
    End Function
    Public Sub WeightValidation(ByVal sender As TextBox, ByVal e As KeyPressEventArgs, Optional ByVal wtRnd As Integer = 3)
        If e.KeyChar = "." And sender.Text.Contains(".") Then
            e.Handled = True
            Exit Sub
        End If
        Select Case e.KeyChar
            Case "+", "-", "0" To "9", "."
            Case ChrW(Keys.Enter), ChrW(Keys.Escape), ChrW(Keys.Space), Chr(Keys.Back)
                Exit Sub
            Case Else
                e.Handled = True
                sender.Focus()
        End Select
        If sender.Text.Contains(".") Then
            Dim dotPos As Integer = InStr(sender.Text, ".", CompareMethod.Text)
            Dim sp() As String = sender.Text.Split(".")
            Dim curPos As Integer = sender.SelectionStart
            If sp.Length >= 2 Then
                If curPos >= dotPos Then
                    If sp(1).Length > wtRnd - 1 Then
                        e.Handled = True
                    End If
                End If
            End If
        End If
    End Sub
    Public Function GetTableStructure(ByVal DbName As String, ByVal TableName As String, ByVal Con As OleDbConnection, Optional ByVal Tran As OleDbTransaction = Nothing) As DataTable
        Dim DtReturn As New DataTable
        DtReturn.TableName = DbName & ".." & TableName
        Dim Cmd As OleDbCommand
        Cmd = New OleDbCommand("SELECT * FROM " & DbName & ".." & TableName & " WHERE 1<>1", Con)
        If Not Tran Is Nothing Then Cmd.Transaction = Tran
        G_DAdapter = New OleDbDataAdapter(Cmd)
        G_DAdapter.Fill(DtReturn)
        Return DtReturn
    End Function
    Public Sub FillCombo(ByVal Combo As ComboBox, ByVal DtSource As DataTable, ByVal FillFieldName As String, Optional ByVal Clear As Boolean = True)
        If Clear Then Combo.Items.Clear()
        For Each Row As DataRow In DtSource.Rows
            Combo.Items.Add(Row.Item(FillFieldName).ToString)
        Next
    End Sub
    Public Sub FillCombo(ByVal ChkCombo As CheckedComboBox, ByVal DtSource As DataTable, ByVal FillFieldName As String, Optional ByVal Clear As Boolean = True, Optional ByVal DefaultCheckValues As String = "")
        Dim selValues As New List(Of String)
        Dim selVal() As String = DefaultCheckValues.ToString.Split(",")
        For Each s As String In selVal
            selValues.Add(s.ToUpper)
        Next
        If Clear Then ChkCombo.Items.Clear()
        For Each Row As DataRow In DtSource.Rows
            ChkCombo.Items.Add(Row.Item(FillFieldName).ToString)
            If selValues.Contains(Row.Item(FillFieldName).ToString.ToUpper) Then
                ChkCombo.SetItemChecked(ChkCombo.Items.Count - 1, True)
            End If
        Next
    End Sub
    Public Function InsertData( _
  ByVal Mode As SyncMode _
  , ByVal Dt As DataTable _
  , ByVal Con As OleDbConnection _
  , Optional ByVal Tran As OleDbTransaction = Nothing _
  , Optional ByVal toId As String = Nothing _
  , Optional ByVal stateId As String = Nothing _
  , Optional ByVal imagePath As String = Nothing _
  , Optional ByVal replaceTableName As String = Nothing _
  , Optional ByVal LocalExecution As Boolean = True _
  ) As Boolean
        Try
            Dim Qry As String = ""
            Dim _Column As String = ""
            Dim _Values As String = ""
            For Each Row As DataRow In Dt.Rows
                Qry = "INSERT INTO " & Row.Table.TableName & " ( "
                _Column = ""
                _Values = ""
                For Each dCol As DataColumn In Row.Table.Columns
                    _Column = _Column & "," + dCol.ColumnName
                    If dCol.DataType.Name = "String" Then
                        _Values = _Values & ",'" & Row.Item(dCol.ColumnName) & "'"
                    ElseIf dCol.DataType.Name = "DateTime" Then
                        If IsDBNull(Row.Item(dCol.ColumnName)) Then
                            _Values = _Values & "," + "NULL" + ""
                        Else
                            _Values = _Values & ",'" & Microsoft.VisualBasic.Format(Row.Item(dCol.ColumnName), "MM/dd/yyyy") & "'"
                        End If
                    ElseIf dCol.DataType.Name = GetType(Double).Name Or dCol.DataType.Name = GetType(Decimal).Name Then
                        _Values = _Values & "," & Val(Row.Item(dCol.ColumnName).ToString)
                    Else
                        _Values = _Values & "," & Row.Item(dCol.ColumnName)
                    End If
                Next
                _Column = _Column & ")"
                _Column = _Column.Substring(1)
                _Values = _Values.Substring(1)
                Qry = (Qry + _Column & " VALUES (") + _Values & ")"
                G_Cmd = New OleDbCommand(Qry, Con, Tran)
                G_Cmd.ExecuteNonQuery()
            Next
            Return True
        Catch ex As Exception
            If Not Tran Is Nothing Then Tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            Return False
        End Try
    End Function
    Public Function Isnumeric(ByVal val As String) As Boolean
        Dim pattern As String = "[0-9]"
        Dim match As System.Text.RegularExpressions.Match
        match = System.Text.RegularExpressions.Regex.Match(val, pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        If Not match.Success Then
            Return False
        End If
        Return True
    End Function

    Public Function Isalpha(ByVal val As String) As Boolean
        Dim pattern As String = "[a-z|A-Z|]"
        Dim match As System.Text.RegularExpressions.Match
        match = System.Text.RegularExpressions.Regex.Match(val, pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        If Not match.Success Then
            Return False
        End If
        Return True
    End Function
End Module
