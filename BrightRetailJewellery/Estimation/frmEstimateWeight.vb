Imports System.IO
Imports System.Data.OleDb
Public Class frmEstimateWeight
    Dim WT_BALANCE_FORMAT As Integer = GetAdmindbSoftValue("WT_BALANCE_FORMAT", 1)
    Dim Port_BaudRate As Integer = 9600
    Dim Port_DataBit As Integer = 8
    Dim Port_PortName As String = "COM1"
    Dim Port_Parity As String = "N"
    Dim objPropertyDia As New PropertyDia(SerialPort1)
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim EstWtTranno As Integer = 0
    Dim EstWtBillDate As Date = GetServerDate(Nothing)

    Public Sub New()
        InitializeComponent()
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
    End Sub
    Private Sub frmEstimateWeight_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY DISPLAYORDER"
        objGPack.FillCombo(strSql, cmbMetal, False)
        cmbMetal.Text = ""
        Dim portSettings() As String
        Dim ctrnameportsetting As String = System.Net.Dns.GetHostName().ToUpper & "-PORTSETTINGS"
        Dim portSettingstr As String = GetAdmindbSoftValue(ctrnameportsetting, "")
        If portSettingstr <> "" Then
            portSettings = portSettingstr.Split("/")
        Else
            portSettings = GetAdmindbSoftValue("PORTSETTINGS", "9600/COM1/8/N").Split("/")
        End If

        Port_BaudRate = IIf(Val(portSettings(0)) <> 0, Val(portSettings(0)), 9600)
        Port_PortName = IIf(portSettings(1) <> "", portSettings(1), "COM1")
        Port_DataBit = IIf(Val(portSettings(2)) <> 0, Val(portSettings(2)), 8)
        If portSettings.Length - 1 >= 3 Then Port_Parity = IIf(portSettings(3) <> "", portSettings(3), "N")
        SerialPort1.DataBits = Port_DataBit
        SerialPort1.BaudRate = Port_BaudRate
        SerialPort1.PortName = Port_PortName
        SerialPort1.Parity = GetParity(Port_Parity)
        objPropertyDia = New PropertyDia(SerialPort1)
        btnNew_Click(sender, e)
    End Sub
    Private Sub txtEstWt_WET_GotFocus(sender As Object, e As EventArgs) Handles txtEstWt_WET.GotFocus
        If cmbMetal.Text = "" Then Exit Sub
        If Val(txtEstWt_WET.Text) > 0 Then Exit Sub
        txtEstWt_WET.ReadOnly = True
        If WT_BALANCE_FORMAT = 2 Then
            If GetGrsWeightFromPort_Format3() = False Then
                btnExit_Click(sender, e)
                MsgBox("Please check com connection", MsgBoxStyle.Information)
            End If
        Else
            If GetGrsWeightFromPort() = False Then
                btnExit_Click(sender, e)
                MsgBox("Please check com connection", MsgBoxStyle.Information)
            End If
        End If
    End Sub
    Private Function GetGrsWeightFromPort_Format3() As Boolean
        Dim Wt_Balance_Sep As String = GetAdmindbSoftValue("WT_BALANCE_SEP", "")
Getnext:
        Dim Weight As Double = Nothing
        Try
            Dim objwt As New WtTran.clsGetWt()
            Weight = Val(objwt.GetWt(Port_BaudRate, Port_PortName).ToString)
            If SerialPort1.IsOpen Then SerialPort1.Close()
        Catch ex As Exception
            cmbMetal.Focus()
            If SerialPort1.IsOpen Then SerialPort1.Close()
            Return False
        End Try
        If Weight = 0 Then txtEstWt_WET.Text = "" : GoTo Getnext : Exit Function
        Dim rndDigit As Integer = 0

        rndDigit = 3
        Weight = Math.Round(Weight, rndDigit)
        txtEstWt_WET.Text = IIf(Weight <> 0, Format(Weight, "0.000"), Nothing)
        txtEstWt_WET.ReadOnly = True

        Return True
    End Function

    Private Function GetGrsWeightFromPort() As Boolean

        Dim Wt_Balance_Sep As String = GetAdmindbSoftValue("WT_BALANCE_SEP", "")
Getnext:
        Dim Weight As Double = Nothing
        Try
            If SerialPort1.IsOpen Then SerialPort1.Close()
            SerialPort1.Open()
            If SerialPort1.IsOpen Then
                Dim readStr As String = Nothing
                If Wt_Balance_Sep <> "" Then
                    readStr = UCase(SerialPort1.ReadTo(Wt_Balance_Sep))
                    If IsNumeric(readStr.Substring(readStr.Length - 6)) Then
                        readStr = (Val(readStr.Substring(readStr.Length - 6)) / 1000).ToString()
                    End If
                    Weight = Val(Trim(readStr))
                Else
                    For cnt As Integer = 1 To 10
                        readStr = UCase(SerialPort1.ReadLine)
                        If readStr.Contains(".") Then
                            Exit For
                        End If
                    Next
                    Dim wt() As String = readStr.Split(Environment.NewLine)
                    Dim wet As String = ""
                    For Each c As Char In wt(0)
                        If c = "," Then Continue For
                        If Char.IsPunctuation(c) Then wet += c
                        If Char.IsNumber(c) Then wet += c
                    Next
                    Weight = Val(Trim(wet))
                    ReadData.Text = readStr
                    SplitData.Text = wt(0)
                    ModifyData.Text = wet
                End If
            End If

            If SerialPort1.IsOpen Then SerialPort1.Close()
        Catch ex As Exception
            cmbMetal.Focus()
            If SerialPort1.IsOpen Then SerialPort1.Close()
            Return False
        End Try
        If Weight = 0 Then txtEstWt_WET.Text = "" : GoTo Getnext : Exit Function
        Dim rndDigit As Integer = 0

        rndDigit = 3
        Weight = Math.Round(Weight, rndDigit)
        txtEstWt_WET.Text = IIf(Weight <> 0, Format(Weight, "0.000"), Nothing)
        Return True
    End Function

    Private Function GetParity(ByVal ParityName As String) As System.IO.Ports.Parity

        Select Case ParityName
            Case "N"
                Return Ports.Parity.None
            Case "E"
                Return Ports.Parity.Even
            Case "M"
                Return Ports.Parity.Mark
            Case "O"
                Return Ports.Parity.Odd
            Case "S"
                Return Ports.Parity.Space
            Case Else
                Return Ports.Parity.None
        End Select
    End Function

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        txtEstWt_WET.Clear()
        cmbMetal.Text = ""
        cmbMetal.Focus()
    End Sub
    Private Sub WeighingScalePropertyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WeighingScalePropertyToolStripMenuItem.Click
        objPropertyDia = New PropertyDia(SerialPort1)
        objPropertyDia.ShowDialog()
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            If Val(txtEstWt_WET.Text) <= 0 Then
                MsgBox("Weight Should Not Be Empty", MsgBoxStyle.Information)
                Exit Sub
            End If

            If cmbMetal.Text = "" Then
                MsgBox("Metal Should Not Be Empty", MsgBoxStyle.Information)
                Exit Sub
            End If

            Dim _MetalId As String = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal.Text & "'")


            tran = Nothing
            tran = cn.BeginTransaction(IsolationLevel.Serializable)
            EstWtTranno = GetEstNoValue("ESTWTPURNO", tran)
            EstWtBillDate = GetEntryDate(EstWtBillDate, tran)
            Dim EstWtissSno As String = GetNewSno(TranSnoType.ESTRECEIPTCODE, tran)
            strSql = " INSERT INTO " & cnStockDb & "..ESTWEIGHT"
            strSql += vbCrLf + " (SNO,DESCRIPTION,TRANNO,TRANDATE,METALID,PCS,GRSWT,USERID,UPDATED,UPTIME,SYSTEMID)"
            strSql += vbCrLf + " VALUES("
            strSql += vbCrLf + " '" & EstWtissSno & "'" 'SNO
            strSql += vbCrLf + " ,'" & txtDescription_OWN.Text & "'" 'DESCRIPTION
            strSql += vbCrLf + " ,'" & EstWtTranno & "'" 'Tranno
            strSql += vbCrLf + " ,'" & GetEntryDate(EstWtBillDate, tran).ToString("yyyy-MM-dd") & "'" 'Trandate
            strSql += vbCrLf + " ,'" & _MetalId & "'" 'MetalId
            strSql += vbCrLf + " ,'" & txtEstPcs_NUM.Text & "'" ' PCS
            strSql += vbCrLf + " ,'" & txtEstWt_WET.Text & "'" ' Grswt
            strSql += vbCrLf + " ,'" & userId & "'" 'USERID
            strSql += vbCrLf + " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
            strSql += vbCrLf + " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
            strSql += vbCrLf + " ,'" & systemId & "'" 'SYSTEMID
            strSql += vbCrLf + " )"
            cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
            tran.Commit()
            tran = Nothing

            'Dim prnmemsuffix As String = ""
            'If GetAdmindbSoftValuefromDt(dtSoftKeys, "PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = IIf(systemId Is Nothing, "", systemId)
            'If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
            '    Dim write As IO.StreamWriter
            '    Dim memfile As String = "\BillPrint" & prnmemsuffix.Trim & ".mem"
            '    write = IO.File.CreateText(Application.StartupPath & memfile)
            '    write.WriteLine(LSet("TYPE", 15) & ":ESTWT")
            '    write.WriteLine(LSet("ESTNO", 15) & ":S.0;P." & EstWtTranno.ToString & "")
            '    write.WriteLine(LSet("TRANDATE", 15) & ":" & EstWtBillDate.ToString("yyyy-MM-dd"))
            '    write.WriteLine(LSet("DUPLICATE", 15) & ":N")
            '    If COSTCENTRE_SINGLE Then write.WriteLine(LSet("COSTCENTRE", 15) & ":" & cnCostId & "")
            '    write.Flush()
            '    write.Close()
            '    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
            'Else
            '    MsgBox("Billprint exe not found", MsgBoxStyle.Information)
            'End If
            PRINT40COLSUMMARY(EstWtTranno, EstWtBillDate)
            btnNew_Click(sender, e)
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            If Not tran Is Nothing Then tran.Dispose()
        End Try
    End Sub

    Private Sub txtEstWt_WET_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtEstWt_WET.KeyPress
        If e.KeyChar = Chr(Keys.Space) Then
            'GetPuGrsWeightFromPort()

            If WT_BALANCE_FORMAT = 2 Then
                If GetGrsWeightFromPort_Format3() = False Then
                    btnExit_Click(sender, e)
                    MsgBox("Please check com connection", MsgBoxStyle.Information)
                End If
            Else
                If GetGrsWeightFromPort() = False Then
                    btnExit_Click(sender, e)
                    MsgBox("Please check com connection", MsgBoxStyle.Information)
                End If
            End If
            e.Handled = True
        End If
    End Sub

    Private Sub frmEstimateWeight_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub PRINT40COLSUMMARY(ByVal billno As Integer, ByVal billdate As DateTime)

        Dim dtestwt As New DataTable
        strSql = " SELECT * FROM " & cnStockDb & "..ESTWEIGHT WHERE "
        strSql += " TRANDATE='" & billdate.ToString("yyyy-MM-dd") & "'"
        strSql += " AND TRANNO='" & billno & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtestwt)

        If dtestwt.Rows.Count > 0 Then
            Dim estMetalName As String = GetSqlValue(cn, "SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID='" & dtestwt.Rows(0).Item("METALID").ToString & "'")
            If IO.File.Exists(Application.StartupPath & "\ESTWTPRINT.TXT") Then
                IO.File.Delete(Application.StartupPath & "\ESTWTPRINT.TXT")
            End If
            Dim write As StreamWriter
            write = IO.File.CreateText(Application.StartupPath & "\ESTWTPRINT.TXT")
            Dim lineprn As String = Space(50)
            write.WriteLine("")
            write.WriteLine("")
            write.WriteLine(Space(10) & "ESTIMATION WEIGHT SLIP")
            write.WriteLine("")
            write.WriteLine("------------------------------------------")
            write.WriteLine("EST DATE :" + billdate.ToString("dd-MM-yyyy") & " " & GetServerTime().ToString("hh:mm:tt") + RSet("EST NO :" & billno.ToString, 20))
            write.WriteLine("------------------------------------------")
            Dim SName As String = "METAL  : " & estMetalName.ToString
            Dim SDESC As String = "DESC   : " & dtestwt.Rows(0).Item("DESCRIPTION").ToString
            Dim PCS As String = "Pcs    : " & dtestwt.Rows(0).Item("PCS").ToString
            Dim Grswt As String = "GRSWT  : " & dtestwt.Rows(0).Item("GRSWT").ToString & " gms"
            SName = LSet(SName, 30)
            SDESC = LSet(SDESC, 45)
            Grswt = LSet(Grswt, 30)
            write.WriteLine(Space(5) & SName)
            write.WriteLine(Space(5) & SDESC)
            write.WriteLine(Space(5) & PCS)
            write.WriteLine(Space(5) & Grswt)
            write.WriteLine("------------------------------------------")

            For i As Integer = 0 To 6
                write.WriteLine()
            Next
            write.WriteLine("i")
            write.Close()
            If IO.File.Exists(Application.StartupPath & "\ESTWTPRINT.BAT") Then
                IO.File.Delete(Application.StartupPath & "\ESTWTPRINT.BAT")
            End If
            Dim writebat As StreamWriter

            Dim PrnName As String = ""
            Dim CondId As String = ""
            Try
                CondId = "'ESTWTPRINT" & Environ("NODE-ID").ToString & "'"
            Catch ex As Exception
                MsgBox("Set Node-Id", MsgBoxStyle.Information)
                Exit Sub
            End Try
            writebat = IO.File.CreateText(Application.StartupPath & "\ESTWTPRINT.BAT")
            strSql = "SELECT * FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID=" & CondId & ""
            Dim dt As New DataTable
            dt = GetSqlTable(strSql, cn)
            If dt.Rows.Count <> 0 Then
                PrnName = dt.Rows(0).Item("CTLTEXT").ToString
            Else
                'Dim settings As Printing.PrinterSettings = New Printing.PrinterSettings()
                'PrnName = settings.PrinterName
                Dim compname As String = Environment.MachineName
                PrnName = "\\" & compname & "\EST"
            End If
            writebat.WriteLine("TYPE " & Application.StartupPath & "\ESTWTPRINT.TXT>" & PrnName)
            writebat.Flush()
            writebat.Close()
            Shell(Application.StartupPath & "\ESTWTPRINT.BAT")
            Shell(Application.StartupPath & "\ESTWTPRINT.BAT")
        End If
    End Sub
End Class