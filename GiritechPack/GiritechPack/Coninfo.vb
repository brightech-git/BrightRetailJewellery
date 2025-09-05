Imports System.IO
Public Class Coninfo
    Private pCompanyId As String
    Private pCompanyName As String
    Private pServerName As String
    Private pDbPath As String
    Private pDbUserId As String
    Private pDbPwd As String
    Private pDbLoginType As String
    Private pDfCostid As String
    Private pAddress1 As String
    Private pDecimal As Integer = 2
    Private pAddress2 As String
    Private pAddress3 As String
    Private pAddress4 As String
    Private pPhone As String
    Private pEmail As String
    Private pLocalTaxNo As String
    Private pCSTNo As String
    Private pTINNo As String
    Private pPANNo As String
    Private pTDSNo As String

    ''Others
    Private pAdminDb As String
    Private pTranDb As String
    Private pConnectionString As String
    Private pControlFocusColor As Color
    Private pControlLostFocusColor As Color
    Private pGridBackGroundColor As Color
    Private pRadio_Check_LostFocusColor As Color
    Private pButton_LostFocusColor As Color
    Private pCharacterCasing As CharacterCasing
    Private pFormBackColor As Color


    Public Sub New(ByVal filePath As String, Optional ByVal strStartPoint As Integer = 21)
        Try
            Dim fil As New FileStream(filePath, FileMode.Open, FileAccess.Read)
            Dim fs As New StreamReader(fil, System.Text.Encoding.Default)
            fs.BaseStream.Seek(0, SeekOrigin.Begin)
            pCompanyId = Mid(fs.ReadLine, 21) '1
            pCompanyName = Mid(fs.ReadLine, 21) '2
            pServerName = Mid(fs.ReadLine, 21) '3
            pDbPath = Mid(fs.ReadLine, 21) '4
            pDbPwd = Mid(fs.ReadLine, 21) '5
            pDbLoginType = Mid(fs.ReadLine, 21) '6
            pDecimal = Val(Mid(fs.ReadLine, 21)) '7
            If pDecimal = 0 Then pDecimal = 2
            'RAJKUMAR ON 08/09/2015 FOR DECIMAL PLACES
            ''''''''''''''''''''''''''''''''''
            pDfCostid = Mid(fs.ReadLine, 21) '7
            pAddress1 = Mid(fs.ReadLine, 21) '7
            pAddress2 = Mid(fs.ReadLine, 21) '8
            pAddress3 = Mid(fs.ReadLine, 21) '9
            pAddress4 = Mid(fs.ReadLine, 21) '10
            pPhone = Mid(fs.ReadLine, 21) '11
            pEmail = Mid(fs.ReadLine, 21) '12
            pLocalTaxNo = Mid(fs.ReadLine, 21) '13
            pCSTNo = Mid(fs.ReadLine, 21) '14
            pTINNo = Mid(fs.ReadLine, 21) '15
            pPANNo = Mid(fs.ReadLine, 21) '16
            pTDSNo = Mid(fs.ReadLine, 21) '17
            fs.Close()

            pAdminDb = lCompanyId & "ADMINDB"
            pControlFocusColor = Color.LightGreen
            pControlLostFocusColor = SystemColors.Window
            pGridBackGroundColor = SystemColors.AppWorkspace
            pRadio_Check_LostFocusColor = Color.Transparent
            pButton_LostFocusColor = Color.FromKnownColor(KnownColor.Control)
            pCharacterCasing = CharacterCasing.Upper
            pFormBackColor = Color.Silver
            If pDbLoginType.ToUpper <> "W" And pDbLoginType.ToUpper <> "S" Then pDbUserId = pDbLoginType.ToUpper
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
            If lDbLoginType <> "W" Then 'If lDbLoginType = "S" Then
                Return "PROVIDER = SQLOLEDB.1;Persist Security Info=False; Initial Catalog=;Data Source=" & lServerName & ";user id=" & IIf(lDbUserId <> "", lDbUserId, "SA") & ";password=" & lDbPwd & ";"
            Else
                Return "Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=;Data Source=" & lServerName & ""
            End If
        End Get
    End Property

    Public ReadOnly Property lAdmindb() As String
        Get
            Return pAdminDb
        End Get
    End Property

    Public Property lTranDb() As String
        Get
            Return pTranDb
        End Get
        Set(ByVal value As String)
            pTranDb = value
        End Set
    End Property

    Public ReadOnly Property lCompanyId() As String
        Get
            Return pCompanyId
        End Get
    End Property
    Public ReadOnly Property lCostId() As String
        Get
            Return pDfCostid
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
    Public ReadOnly Property lAddress1() As String
        Get
            Return pAddress1
        End Get
    End Property
    Public ReadOnly Property lAddress2() As String
        Get
            Return pAddress2
        End Get
    End Property
    Public ReadOnly Property lAddress3() As String
        Get
            Return pAddress3
        End Get
    End Property
    Public ReadOnly Property lAddress4() As String
        Get
            Return pAddress4
        End Get
    End Property
    Public ReadOnly Property lPhone() As String
        Get
            Return pPhone
        End Get
    End Property

    Public ReadOnly Property lEmail() As String
        Get
            Return pEmail
        End Get
    End Property

    Public ReadOnly Property lLocalTaxNo() As String
        Get
            Return pLocalTaxNo
        End Get
    End Property
    Public ReadOnly Property lCSTNo() As String
        Get
            Return pCSTNo
        End Get
    End Property
    Public ReadOnly Property lTINNo() As String
        Get
            Return pTINNo
        End Get
    End Property
    Public ReadOnly Property lPANNo() As String
        Get
            Return pPANNo
        End Get
    End Property
    Public ReadOnly Property lTDSNo() As String
        Get
            Return pTDSNo
        End Get
    End Property
    Public ReadOnly Property lDecimal() As Integer
        Get
            Return pDecimal
        End Get
    End Property
End Class

