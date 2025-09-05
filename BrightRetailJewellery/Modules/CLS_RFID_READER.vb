Imports VB = Microsoft.VisualBasic
Public Class CLS_RFID_READER
    Private WithEvents TagReader As New AxKITagReader.AxTagReader
    Private BaudRate As Integer
    Private ReadBuffer(1024) As Byte
    Public Property pBaudRate() As Integer
        Get
            Return BaudRate
        End Get
        Set(ByVal value As Integer)
            BaudRate = value
        End Set
    End Property
    Private Flag As String
    Public Property pFlag() As String
        Get
            Return Flag
        End Get
        Set(ByVal value As String)
            Flag = value
        End Set
    End Property
    Private BaudRateList As New List(Of String)
    Public ReadOnly Property pBaudRateList() As List(Of String)
        Get
            Return BaudRateList
        End Get
    End Property
    Private FlagList As New List(Of String)
    Public ReadOnly Property pFlagList() As List(Of String)
        Get
            Return FlagList
        End Get
    End Property
    Private DtActiveReaders As New DataTable("AvailableReaders")
    Public ReadOnly Property pDtActiveReaders() As DataTable
        Get
            Return DtActiveReaders
        End Get
    End Property
    Private PortNumber As Integer = 1
    Public Property pPortNumber() As Integer
        Get
            Return PortNumber
        End Get
        Set(ByVal value As Integer)
            PortNumber = value
        End Set
    End Property
    Private ReadedData As New List(Of String)
    Public ReadOnly Property pReadedData() As List(Of String)
        Get
            Return ReadedData
        End Get
    End Property

    Public Sub New(ByVal TgR As AxKITagReader.AxTagReader)
        Me.TagReader = TgR

        pBaudRateList.Clear()
        pBaudRateList.Add("1200")
        pBaudRateList.Add("2400")
        pBaudRateList.Add("4800")
        pBaudRateList.Add("9600")
        pBaudRateList.Add("19200")
        pBaudRateList.Add("38400")
        pBaudRateList.Add("115200")
        pBaudRateList.Add("230400")
        pFlagList.Clear()
        pFlagList.Add("8O1")
        pFlagList.Add("8E1")
        pFlagList.Add("8N1")

        pBaudRate = 38400
        pFlag = "8E1"

        DtActiveReaders.TableName = "AvailableReaders"
        DtActiveReaders.Columns.Add(New DataColumn("ReaderId", GetType(System.Int32)))
        DtActiveReaders.Columns.Add(New DataColumn("ReaderName", GetType(System.String)))
        DtActiveReaders.Columns.Add(New DataColumn("PortType", GetType(System.String)))
        DtActiveReaders.Columns.Add(New DataColumn("PortNum", GetType(System.Int32)))
        DtActiveReaders.Columns.Add(New DataColumn("IPAddress", GetType(System.String)))
        DtActiveReaders.Columns.Add(New DataColumn("ReaderLocation", GetType(System.String)))
        DtActiveReaders.Columns.Add(New DataColumn("Flags", GetType(System.String)))
        DtActiveReaders.Columns.Add(New DataColumn("BaudRate", GetType(System.String)))
        DtActiveReaders.Columns.Add(New DataColumn("pBaudRateList", GetType(System.Int32)))
        DtActiveReaders.Columns.Add(New DataColumn("Active", GetType(System.String)))
        DtActiveReaders.AcceptChanges()
    End Sub

    Private Function isConnect() As Boolean
        For i As Integer = 1 To PortNumber
            If Hs0701(i, pBaudRate) Then
                Return True
            End If
        Next
    End Function

    Private Function Hs0701(ByVal cnt As Integer, ByVal BuadRate As Integer) As Boolean
        Dim Con As Boolean = False
        If TagReader.OpenPort() Then
            For i As Integer = 1 To cnt
                If TagReader.OpenPort("Com" & i, BuadRate) Then
                    If TagReader.HardWareType <> "" And TagReader.HardWareType <> "(No Device Connected)" Then
                        Con = True
                        Dim Found As Boolean = False
                        Dim nrow As DataRow
                        Dim Readernm As String = ""
                        Dim DrwFind As DataRow() = DtActiveReaders.Select("ReaderId=1")
                        If DrwFind.Length > 0 Then
                            DrwFind(0)("PortType") = "Com" & i
                            DrwFind(0)("Baudrate") = BuadRate
                            DrwFind(0)("PortNum") = i
                            DrwFind(0)("Flags") = pFlag
                            DrwFind(0)("IPAddress") = "0"
                            DrwFind(0)("ReaderLocation") = "Local"
                            DtActiveReaders.WriteXml("AvailableReaders.xml")
                            Found = True
                        End If
                        If Not Found Then
                            nrow = DtActiveReaders.NewRow
                            nrow("ReaderId") = 1
                            nrow("ReaderName") = "Hs0701"
                            nrow("PortType") = "Com" & i
                            nrow("Flags") = pFlag
                            nrow("PortNum") = i
                            nrow("ReaderLocation") = "Local"
                            nrow("IPAddress") = "0"
                            nrow("Baudrate") = BuadRate
                            nrow("Active") = True
                            DtActiveReaders.Rows.Add(nrow)
                            DtActiveReaders.WriteXml("AvailableReaders.xml")
                        End If
                        TagReader.ClosePort()
                        Exit For
                    End If
                End If
            Next
        End If
        Return Con
    End Function

    Public Function Read() As List(Of String)
        ReadedData.Clear()
        For cnt As Integer = 1 To 2
            If isConnect() = False Then
                If cnt = 2 Then
                    MsgBox("Reader is not Connected", MsgBoxStyle.Information, "CSR RFID")
                    Return ReadedData
                End If
            Else
                Exit For
            End If
        Next
        StopHs0701()
        If StartHs0701() = False Then Return ReadedData
        Hs0701Reading()
        TagReader.ClosePort()
        TagReader = Nothing
        Return ReadedData
    End Function

    Private Sub StopHs0701()
        While Not TagReader.ClosePort()

        End While
        ReadedData.Clear()
    End Sub

    Private Function StartHs0701() As Boolean
        ReadedData.Clear()
        Dim Con As Boolean = False
        If TagReader.OpenPort("Com" & PortNumber, BaudRate) Then
            If TagReader.HardWareType <> "" And TagReader.HardWareType <> "(No Device Connected)" Then
                Con = True
            End If
        End If
        If Not Con Then
            Con = False
            TagReader.ClosePort()
            MsgBox("Could not Detect the Reader,click Ok and try again.", MsgBoxStyle.Information, "CSR RFID")
        End If
        Return Con
    End Function

    Sub Hs0701Reading()
        Try
            Dim iBlockCnt, iDataLen, j, i1stEmpty As Short
            Dim sTemp As String
            Dim bFlag As Byte
            ' As FSK is not supported on short and mid range readers,
            ' I need to check and use the appropriate ASK/FSK flag.
            Select Case TagReader.HardWareType
                Case "(Device - Long Range Tag Reader)"
                    bFlag = &H7S ' FSK Support
                Case "(Device - EzPos II)", "(Device - EzScan)"
                    bFlag = &H6S ' ASK Support
            End Select

            iDataLen = TagReader.CMD_ISO_Inventory(TagReader.VBBytePtr(ReadBuffer(0)), bFlag, 0, False)
            Call TagReader.CMD_CSC_LastCmdRepeat(TagReader.VBBytePtr(ReadBuffer(0)), iDataLen)
            If iDataLen < 0 Then
                ' MsgBox("Could not Detect the Reader,click Ok and try again.", MsgBoxStyle.Information, "CSR RFID")
            End If

            Dim Cnt As Integer = 0
            While 1 = 1
                Cnt += 1
                If Cnt >= 1000 Then
                    Throw New Exception("Deduct Deadlock")
                End If
                'If Not Flg Then Exit Sub
                ' Show the number of items i currently has scanned.
                ' lblCount.Text = "   Item Count : " & bNoOfTags
                ' Show some movement
                ' Using RecvLoopReply to continueously wait for reply from the reader
                iDataLen = TagReader.RecvLoopReply(TagReader.VBBytePtr(ReadBuffer(0)))
                'tagreader1.RecvLoopReply 
                If iDataLen > 0 Then
                    ' Timer2.Enabled = False
                    ' I am checking how many tag IDs i received...
                    iBlockCnt = iDataLen / 10
                    If iBlockCnt > 0 Then
                        'If Not Flg Then Exit Sub
                        ' Loop through the IDs i just received...
                        For j = 0 To iBlockCnt - 1
                            'MsgBox("readbuf:" & ReadBuffer(j * 10).ToString)
                            If ReadBuffer(j * 10) = 0 Then
                                sTemp = VB.Right("00" & Hex(ReadBuffer((j * 10) + 9)), 2) & VB.Right("00" & Hex(ReadBuffer((j * 10) + 8)), 2) & VB.Right("00" & Hex(ReadBuffer((j * 10) + 7)), 2) & VB.Right("00" & Hex(ReadBuffer((j * 10) + 6)), 2) & VB.Right("00" & Hex(ReadBuffer((j * 10) + 5)), 2) & VB.Right("00" & Hex(ReadBuffer((j * 10) + 4)), 2) & VB.Right("00" & Hex(ReadBuffer((j * 10) + 3)), 2) & VB.Right("00" & Hex(ReadBuffer((j * 10) + 2)), 2)
                                i1stEmpty = -1
                                If sTemp.Trim <> "" Then
                                    If Not IsNothing(sTemp.ToUpper) Then
                                        If sTemp.StartsWith("E") Then
                                            If ReadedData.Contains(sTemp.ToUpper) = False Then
                                                ReadedData.Add(sTemp.ToUpper)
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        Next j
                    End If
                Else
                    Exit While
                    Call TagReader.CMD_CSC_LastCmdRepeat(TagReader.VBBytePtr(ReadBuffer(0)), iDataLen)
                End If
                System.Windows.Forms.Application.DoEvents()
            End While
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
End Class
