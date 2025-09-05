Imports System.IO
Public Class Coninfo
    Private pCompanyId As String
    Private pCompanyName As String
    Private pServerName As String
    Private pDbPath As String
    Private pDbUserId As String
    Private pDbPwd As String
    Private pDbLoginType As String
    ''Others
    Private pConnectionString As String
    Private pControlFocusColor As Color
    Private pControlLostFocusColor As Color
    Private pGridBackGroundColor As Color
    Private pRadio_Check_LostFocusColor As Color
    Private pButton_LostFocusColor As Color
    Private pCharacterCasing As CharacterCasing
    Private pFormBackColor As Color
    Private Function GetLineInfo(ByVal str As String) As String
        Dim RetStr As String = ""
        If str = "" Then Return str
        RetStr = Mid(str, str.IndexOf(":") + 2, str.Length)
        Return RetStr
    End Function


    Public Sub New(ByVal filePath As String, Optional ByVal strStartPoint As Integer = 21)
        Try
            Dim fil As New FileStream(filePath, FileMode.Open, FileAccess.Read)
            Dim fs As New StreamReader(fil, System.Text.Encoding.Default)
            'Dim fs As New StreamReader(fil)
            fs.BaseStream.Seek(0, SeekOrigin.Begin)
            pCompanyId = GetLineInfo(fs.ReadLine).ToUpper '1
            pCompanyName = GetLineInfo(fs.ReadLine).ToUpper '2
            pServerName = GetLineInfo(fs.ReadLine).ToUpper '3
            pDbPath = GetLineInfo(fs.ReadLine).ToUpper '4
            pDbPwd = GetLineInfo(fs.ReadLine) '.ToUpper '5
            pDbLoginType = GetLineInfo(fs.ReadLine).ToUpper '6
            If UCase(pDbLoginType) <> "S" And UCase(pDbLoginType) <> "S" Then pDbUserId = pDbLoginType
            fs.Close()

            pControlFocusColor = Color.LightGreen
            pControlLostFocusColor = SystemColors.Window
            pGridBackGroundColor = SystemColors.AppWorkspace
            pRadio_Check_LostFocusColor = Color.Transparent
            pButton_LostFocusColor = Color.FromKnownColor(KnownColor.Control)
            pCharacterCasing = CharacterCasing.Upper
            pFormBackColor = Color.Silver
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            Application.Exit()
            Exit Sub
        End Try
    End Sub

    Public ReadOnly Property lColorFormBack() As Color
        Get
            Return pFormBackColor
        End Get
    End Property

    Public ReadOnly Property lCharacterCasing() As CharacterCasing
        Get
            Return pCharacterCasing
        End Get
    End Property

    Public ReadOnly Property lColorLostFocusButton() As Color
        Get
            Return pButton_LostFocusColor
        End Get
    End Property

    Public ReadOnly Property lColorLostFocusRadio_Check() As Color
        Get
            Return pRadio_Check_LostFocusColor
        End Get
    End Property

    Public ReadOnly Property lColorBackGroundGrid() As Color
        Get
            Return pGridBackGroundColor
        End Get
    End Property

    Public ReadOnly Property lColorLostFocusControl() As Color
        Get
            Return pControlLostFocusColor
        End Get
    End Property

    Public ReadOnly Property lColorFocusControl() As Color
        Get
            Return pControlFocusColor
        End Get
    End Property

    Public ReadOnly Property lConnectionString() As String
        Get
            If UCase(lDbLoginType = "W") Then
                Return "Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=;Data Source=" & lServerName & ""
            Else
                Dim passWord As String = lDbPwd
                'If passWord <> "" Then passWord = GiritechPack.Methods.Decrypt(lDbPwd)
                If passWord <> "" Then passWord = Decrypt(lDbPwd)
                Return String.Format("PROVIDER = SQLOLEDB.1;Persist Security Info=False; Initial Catalog=;Data Source={0};User Id=" & IIf(lDbUserId <> "", lDbUserId, "sa") & ";password=" & passWord & ";", lServerName)
                'Return "PROVIDER = SQLOLEDB.1;Persist Security Info=False; Initial Catalog=;Data Source=(0);user id=" & IIf(lDbUserId <> "", lDbUserId, "sa") & ";password=" & lDbPwd & ";"

            End If
        End Get

    End Property

    Public ReadOnly Property lCompanyId() As String
        Get
            Return pCompanyId
        End Get
    End Property
    Public ReadOnly Property lCompanyName() As String
        Get
            Return pCompanyName
        End Get
    End Property
    Public ReadOnly Property lServerName() As String
        Get
            Return pServerName
        End Get
    End Property
    Public ReadOnly Property lDbPath() As String
        Get
            Return pDbPath
        End Get
    End Property
    Public ReadOnly Property lDbUserId() As String
        Get
            Return pDbUserId
        End Get
    End Property
    Public ReadOnly Property lDbPwd() As String
        Get
            Return pDbPwd
        End Get
    End Property
    Public ReadOnly Property lDbLoginType() As String
        Get
            Return pDbLoginType

        End Get
    End Property
    'Public ReadOnly Property lAddress1() As String
    '    Get
    '        Return pAddress1
    '    End Get
    'End Property
    'Public ReadOnly Property lAddress2() As String
    '    Get
    '        Return pAddress2
    '    End Get
    'End Property
    'Public ReadOnly Property lAddress3() As String
    '    Get
    '        Return pAddress3
    '    End Get
    'End Property
    'Public ReadOnly Property lAddress4() As String
    '    Get
    '        Return pAddress4
    '    End Get
    'End Property
    'Public ReadOnly Property lPhone() As String
    '    Get
    '        Return pPhone
    '    End Get
    'End Property

    'Public ReadOnly Property lEmail() As String
    '    Get
    '        Return pEmail
    '    End Get
    'End Property

    'Public ReadOnly Property lLocalTaxNo() As String
    '    Get
    '        Return pLocalTaxNo
    '    End Get
    'End Property
    'Public ReadOnly Property lCSTNo() As String
    '    Get
    '        Return pCSTNo
    '    End Get
    'End Property
    'Public ReadOnly Property lTINNo() As String
    '    Get
    '        Return pTINNo
    '    End Get
    'End Property
    'Public ReadOnly Property lPANNo() As String
    '    Get
    '        Return pPANNo
    '    End Get
    'End Property
    'Public ReadOnly Property lTDSNo() As String
    '    Get
    '        Return pTDSNo
    '    End Get
    'End Property
End Class

