Public Class CLS_MAILCONFIGURATION
    Public Sub New()

    End Sub
    Private SmtpHostName As String = "smtp.gmail.com"
    Public Property pSmtpHostName() As String
        Get
            Return SmtpHostName
        End Get
        Set(ByVal value As String)
            SmtpHostName = value
        End Set
    End Property
    Private SmtpPort As Integer = 587
    Public Property pSmtpPort() As Integer
        Get
            Return SmtpPort
        End Get
        Set(ByVal value As Integer)
            SmtpPort = value
        End Set
    End Property
    Private EnableSSL As Boolean = False
    Public Property pEnableSSL() As Boolean
        Get
            Return EnableSSL
        End Get
        Set(ByVal value As Boolean)
            EnableSSL = value
        End Set
    End Property
    Private FromMailId As String = "aks.epostt@gmail.com"
    Public Property pFromMailId() As String
        Get
            Return FromMailId
        End Get
        Set(ByVal value As String)
            FromMailId = value
        End Set
    End Property
    Private FromMailPwd As String = Encrypt("giritech")
    Public Property pFromMailPwd() As String
        Get
            Return Decrypt(FromMailPwd)
        End Get
        Set(ByVal value As String)
            FromMailPwd = Encrypt(value)
        End Set
    End Property
    Private ToMailId As String
    Public Property pToMailId() As String
        Get
            Return ToMailId
        End Get
        Set(ByVal value As String)
            ToMailId = value
        End Set
    End Property
    Private Body As String = Nothing
    Public Property pBody() As String
        Get
            Return Body
        End Get
        Set(ByVal value As String)
            Body = value
        End Set
    End Property
    Private CellColor As Boolean = True
    Public Property pCellColor() As Boolean
        Get
            Return CellColor
        End Get
        Set(ByVal value As Boolean)
            CellColor = value
        End Set
    End Property
    Private HeaderForAllPages As Boolean = True
    Public Property pHeaderForAllPages() As Boolean
        Get
            Return HeaderForAllPages
        End Get
        Set(ByVal value As Boolean)
            HeaderForAllPages = value
        End Set
    End Property
    Private FitToPageWidth As Boolean
    Public Property pFitToPageWidth() As Boolean
        Get
            Return FitToPageWidth
        End Get
        Set(ByVal value As Boolean)
            FitToPageWidth = value
        End Set
    End Property
    Private Landscape As Boolean = True
    Public Property pLandscape() As Boolean
        Get
            Return Landscape
        End Get
        Set(ByVal value As Boolean)
            Landscape = value
        End Set
    End Property
    Private WithBorder As Boolean
    Public Property pWithBorder() As Boolean
        Get
            Return WithBorder
        End Get
        Set(ByVal value As Boolean)
            WithBorder = value
        End Set
    End Property
    Private AttachementView As Boolean = True
    Public Property pAttachementView() As Boolean
        Get
            Return AttachementView
        End Get
        Set(ByVal value As Boolean)
            AttachementView = value
        End Set
    End Property

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
End Class
