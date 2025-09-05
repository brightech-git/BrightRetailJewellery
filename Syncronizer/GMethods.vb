Imports System.Reflection
Imports System.Data.OleDb
Module GMethods
    Public _Cn1 As OleDbConnection
#Region "Delegates"
    Private Delegate Sub SetControlValueCallback(ByVal oControl As Object, ByVal propName As String, ByVal propValue As Object)
#End Region

#Region "Delegate Methods"
    Public Sub SetControlPropertyValue(ByVal oControl As Object, ByVal propName As String, ByVal propValue As Object)
        If oControl.InvokeRequired Then
            Dim d As New SetControlValueCallback(AddressOf SetControlPropertyValue)
            oControl.Invoke(d, New Object() {oControl, propName, propValue})
        Else
            Dim t As Type = oControl.[GetType]()
            Dim props As PropertyInfo() = t.GetProperties()
            For Each p As PropertyInfo In props
                If p.Name.ToUpper() = propName.ToUpper() Then
                    p.SetValue(oControl, propValue, Nothing)
                End If
            Next
        End If
    End Sub
#End Region

    Public Function GetSqlValue(ByVal cn As OleDbConnection, ByVal qry As String, Optional ByVal field As String = Nothing, Optional ByVal defValue As String = "", Optional ByRef tran As OleDbTransaction = Nothing) As String
        Dim G_DTable As New DataTable
        Dim G_DAdapter As OleDbDataAdapter
        Dim G_Cmd As OleDbCommand
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
    'Public Function Decrypt(ByVal Pwd As String) As String
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
    'Public Function Encrypt(ByVal Pwd As String) As String
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

    Public Function Decrypt(ByVal Str_Pwd As String) As String
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

    Public Function Encrypt(ByVal Str_Pwd As String) As String
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

    Public Function DecryptXml(ByVal Pwd As String) As String
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
    Public Function EncryptXml(ByVal Pwd As String) As String
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
End Module
