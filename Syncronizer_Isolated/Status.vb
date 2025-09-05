Imports System.Reflection
Public Class Status
    Private dtGrid As New DataTable
    Private StartTime As DateTime
    Private MeTitle As String
    Private tSpan As TimeSpan
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        dtGrid.Columns.Add("STATUS", GetType(String))
        'Dgv.DataSource = dtGrid
        Dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub
#Region "InternetConnectionVariables"
    Dim ConnectionQualityString As String = "Off"
    Dim ConnectionStateString As String = ""
    Private Declare Function InternetGetConnectedState Lib "wininet.dll" (ByRef _
    lpSFlags As Int32, ByVal dwReserved As Int32) As Boolean
    Public Enum InetConnState
        modem = &H1
        lan = &H2
        proxy = &H4
        ras = &H10
        offline = &H20
        configured = &H40
    End Enum
#End Region

#Region "InternetConnectionMethods"
    Public Function CheckInetConnectionState() As Boolean

        Dim lngFlags As Long

        If InternetGetConnectedState(lngFlags, 0) Then
            ' True
            If lngFlags And InetConnState.lan Then
                ConnectionStateString = "LAN."
                lblConnectionType.Image = ImageList1.Images(1)
            ElseIf lngFlags And InetConnState.modem Then
                ConnectionStateString = "Modem."
                lblConnectionType.Image = ImageList1.Images(1)
            ElseIf lngFlags And InetConnState.configured Then
                ConnectionStateString = "Configured."
                lblConnectionType.Image = ImageList1.Images(1)
            ElseIf lngFlags And InetConnState.proxy Then
                ConnectionStateString = "Proxy"
                lblConnectionType.Image = ImageList1.Images(1)
            ElseIf lngFlags And InetConnState.ras Then
                ConnectionStateString = "RAS."
                lblConnectionType.Image = ImageList1.Images(1)
            ElseIf lngFlags And InetConnState.offline Then
                ConnectionStateString = "Offline."
                lblConnectionType.Image = ImageList1.Images(2)
            End If
        Else
            ' False
            ConnectionStateString = "Not Connected."
            lblConnectionType.Image = ImageList1.Images(3)
        End If
        SetControlPropertyValue(lblConnectionType, "Text", "          Connection Type:  " & ConnectionStateString)
    End Function
    Public Function CheckInetConnection() As Boolean

        Dim lngFlags As Long

        If InternetGetConnectedState(lngFlags, 0) Then
            ' True
            If lngFlags And InetConnState.lan Then
                Select Case ConnectionQualityString
                    Case "Good"
                        lblConnectQuality.ForeColor = Color.Green
                        SetControlPropertyValue(lblConnectQuality, "Text", "Connection Quality:  Good")
                        ConnectionQualityString = "Good"
                    Case "Intermittent"
                        lblConnectQuality.ForeColor = Color.Green
                        SetControlPropertyValue(lblConnectQuality, "Text", "Connection Quality:  Good")
                        ConnectionQualityString = "Good"
                    Case "Off"
                        lblConnectQuality.ForeColor = Color.DarkOrange
                        SetControlPropertyValue(lblConnectQuality, "Text", "Connection Quality:  Intermittent")
                        ConnectionQualityString = "Intermittent"
                End Select
                'Me.Refresh()
            ElseIf lngFlags And InetConnState.modem Then
                Select Case ConnectionQualityString
                    Case "Good"
                        lblConnectQuality.ForeColor = Color.Green
                        SetControlPropertyValue(lblConnectQuality, "Text", "Connection Quality:  Good")
                        ConnectionQualityString = "Good"
                    Case "Intermittent"
                        lblConnectQuality.ForeColor = Color.Green
                        SetControlPropertyValue(lblConnectQuality, "Text", "Connection Quality:  Good")
                        ConnectionQualityString = "Good"
                    Case "Off"
                        lblConnectQuality.ForeColor = Color.DarkOrange
                        SetControlPropertyValue(lblConnectQuality, "Text", "Connection Quality:  Intermittent")
                        ConnectionQualityString = "Intermittent"
                End Select
            ElseIf lngFlags And InetConnState.configured Then
                Select Case ConnectionQualityString
                    Case "Good"
                        lblConnectQuality.ForeColor = Color.Green
                        SetControlPropertyValue(lblConnectQuality, "Text", "Connection Quality:  Good")
                        ConnectionQualityString = "Good"
                    Case "Intermittent"
                        lblConnectQuality.ForeColor = Color.Green
                        SetControlPropertyValue(lblConnectQuality, "Text", "Connection Quality:  Good")
                        ConnectionQualityString = "Good"
                    Case "Off"
                        lblConnectQuality.ForeColor = Color.DarkOrange
                        SetControlPropertyValue(lblConnectQuality, "Text", "Connection Quality:  Intermittent")
                        ConnectionQualityString = "Intermittent"
                End Select
            ElseIf lngFlags And InetConnState.proxy Then
                Select Case ConnectionQualityString
                    Case "Good"
                        lblConnectQuality.ForeColor = Color.Green
                        SetControlPropertyValue(lblConnectQuality, "Text", "Connection Quality:  Good")
                        ConnectionQualityString = "Good"
                    Case "Intermittent"
                        lblConnectQuality.ForeColor = Color.Green
                        SetControlPropertyValue(lblConnectQuality, "Text", "Connection Quality:  Good")
                        ConnectionQualityString = "Good"
                    Case "Off"
                        lblConnectQuality.ForeColor = Color.DarkOrange
                        SetControlPropertyValue(lblConnectQuality, "Text", "Connection Quality:  Intermittent")
                        ConnectionQualityString = "Intermittent"
                End Select
            ElseIf lngFlags And InetConnState.ras Then
                Select Case ConnectionQualityString
                    Case "Good"
                        lblConnectQuality.ForeColor = Color.Green
                        SetControlPropertyValue(lblConnectQuality, "Text", "Connection Quality:  Good")
                        ConnectionQualityString = "Good"
                    Case "Intermittent"
                        lblConnectQuality.ForeColor = Color.Green
                        SetControlPropertyValue(lblConnectQuality, "Text", "Connection Quality:  Good")
                        ConnectionQualityString = "Good"
                    Case "Off"
                        lblConnectQuality.ForeColor = Color.DarkOrange
                        SetControlPropertyValue(lblConnectQuality, "Text", "Connection Quality:  Intermittent")
                        ConnectionQualityString = "Intermittent"
                End Select
            ElseIf lngFlags And InetConnState.offline Then
                Select Case ConnectionQualityString
                    Case "Good"
                        lblConnectQuality.ForeColor = Color.Green
                        SetControlPropertyValue(lblConnectQuality, "Text", "Connection Quality:  Good")
                        ConnectionQualityString = "Good"
                    Case "Intermittent"
                        lblConnectQuality.ForeColor = Color.Green
                        SetControlPropertyValue(lblConnectQuality, "Text", "Connection Quality:  Good")
                        ConnectionQualityString = "Good"
                    Case "Off"
                        lblConnectQuality.ForeColor = Color.DarkOrange
                        SetControlPropertyValue(lblConnectQuality, "Text", "Connection Quality:  Intermittent")
                        ConnectionQualityString = "Intermittent"
                End Select
            End If
        Else
            ' False
            Select Case ConnectionQualityString
                Case "Good"
                    lblConnectQuality.ForeColor = Color.DarkOrange
                    SetControlPropertyValue(lblConnectQuality, "Text", "Connection Quality:  Intermittent")
                    ConnectionQualityString = "Intermittent"
                Case "Intermittent"
                    lblConnectQuality.ForeColor = Color.Red
                    SetControlPropertyValue(lblConnectQuality, "Text", "Connection Quality:  Off")
                    ConnectionQualityString = "Off"
                Case "Off"
                    lblConnectQuality.ForeColor = Color.Red
                    SetControlPropertyValue(lblConnectQuality, "Text", "Connection Quality:  Off")
                    ConnectionQualityString = "Off"
            End Select
        End If

    End Function
#End Region

    

    Private Sub Status_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        StartTime = DateTime.Now
        MeTitle = Me.Text
        Timer1.Enabled = True
        Me.Icon = My.Resources.Mobile_Syncronization
        Dim t As New Threading.Thread(AddressOf initiator)
        t.IsBackground = True
        t.Priority = Threading.ThreadPriority.Lowest
        t.Start()
        dtGrid.AcceptChanges()
        Dgv.DataSource = dtGrid
        Dgv.BackgroundColor = Me.BackColor
        If Dgv.RowCount > 0 Then
            Dgv.Rows(0).Selected = False
        End If
        btnStatus.Text = "Show Status >>"
    End Sub

    Private Sub initiator()
        Try
            While 1 = 1
                tSpan = DateTime.Now.Subtract(StartTime)
                SetControlPropertyValue(Me, "TEXT", MeTitle & " " & DateTime.Now.ToString("dd/MM/yyyy"))
                CheckInetConnectionState()
                CheckInetConnection()
            End While
        Catch ex As Exception
        End Try
    End Sub

    Public Sub AddStatus(ByVal status As String)
        If dtGrid.Columns.Contains("FROMID") Then dtGrid.Columns.Remove("FROMID")
        If dtGrid.Columns.Contains("TOID") Then dtGrid.Columns.Remove("TOID")
        If dtGrid.Columns.Contains("RECORDS") Then dtGrid.Columns.Remove("RECORDS")
        If dtGrid.Columns.Contains("STATUS") = False Then dtGrid.Columns.Add("STATUS", GetType(String))
        Dim Ro As DataRow = dtGrid.NewRow
        Ro!status = status
        dtGrid.Rows.Add(Ro)
    End Sub

    Public Sub AddStatus(ByVal Datatble As DataTable)
        dtGrid.Rows.Clear()
        dtGrid = Datatble.Copy
    End Sub
    Private Sub btnHide_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHide.Click
        Me.Hide()

    End Sub

    Private Sub btnStatus_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStatus.Click
        If btnStatus.Text = "<< Hide Status" Then
            btnStatus.Text = "Show Status >>"
        Else
            btnStatus.Text = "<< Hide Status"
        End If
    End Sub

    Private Sub btnStatus_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStatus.TextChanged
        If btnStatus.Text = "<< Hide Status" Then
            Me.Size = New Size(345, 304)
        Else
            Me.Size = New Size(345, 133)
        End If
    End Sub

    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If tSpan.Seconds = 20 And Not CheckBox1.Checked Then
            Me.Hide()
        End If
        'If (DateTime.Now.Minute - StartTime.Minute) >= 1 Then
        '    Me.Hide()
        'End If
    End Sub

    Private Sub Dgv_CellMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles Dgv.CellMouseClick

        If Dgv.ColumnCount = 1 Then
            Try

            
                Dim stat As String
                Dim uid As String
                Dim Startind, Endindex As Integer
                stat = Dgv.CurrentRow.Cells(0).Value.ToString.Replace("Unique Id:", "~")
                Startind = stat.IndexOf("~", 1)
                Endindex = stat.IndexOf("#", 1)
                uid = stat.Substring(Startind + 1, Endindex - Startind - 1).Trim
                Dim objErrupd As New frmErrorupd(uid)
                objErrupd.ShowDialog()
            Catch ex As Exception
            End Try
        End If
    End Sub
End Class