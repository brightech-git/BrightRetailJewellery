Imports System.Data.OleDb
Imports System.IO
Public Class frmCashCounterStock

    Dim strSql As String = ""
    Dim SelectedCompanyId As String = ""
    Dim DT As New DataTable()
    Dim temptable As String
    Dim cmd As New OleDbCommand
    Dim dtMergeHeader As DataTable

    Dim strprint As String
    Dim FileWrite As StreamWriter
    Dim PgNo As Integer
    Dim line As Integer
    Dim SpecificPrint As Boolean = False

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        AddHandler chkLstCompany.LostFocus, AddressOf CheckedListCompany_Lostfocus
    End Sub

    Private Sub frmCashCounterStock_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnNew_Click(Me, New EventArgs())
        LoadCompany(chkLstCompany)
    End Sub

    Function funcAddCostCentre() As Integer
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE'"
        If objGPack.GetSqlValue(strSql, , "N") = "Y" Then
            strSql = "select DISTINCT CostName from " & cnAdminDb & "..CostCentre order by CostName"
            cmbCostCentre.Items.Clear()
            cmbCostCentre.Items.Add("ALL")
            objGPack.FillCombo(strSql, cmbCostCentre, False, False)
            cmbCostCentre.Text = IIf(cnDefaultCostId, "ALL", cnCostName)
            If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then cmbCostCentre.Enabled = False
        Else
            cmbCostCentre.Items.Clear()
            cmbCostCentre.Enabled = False
        End If
    End Function
    Function funcAddCashCounter() As Integer

        cmbCounterID_own.DataSource = Nothing
        cmbCounterID_own.Items.Add("ALL")
        strSql = "SELECT CASHNAME,CASHID FROM " & cnAdminDb & "..CASHCOUNTER ORDER BY CASHID"
        DT = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(DT)
        If DT.Rows.Count > 0 Then
            cmbCounterID_own.DataSource = DT
            cmbCounterID_own.DisplayMember = "CASHNAME"
            cmbCounterID_own.ValueMember = "CASHID"
        Else
            cmbCounterID_own.Items.Clear()
            cmbCounterID_own.Enabled = False
        End If
    End Function
    Private Function GetViewDetails()
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Function
        'btnView_Search.Enabled = False
        Dim DtGrid As New DataTable
        SelectedCompanyId = GetSelectedCompanyId(chkLstCompany, False)
        temptable = "TEMP" & systemId & "CASHCOUNTSTOCK"

        strSql = " EXEC " & cnAdminDb & "..SP_CASHCOUNTER_STOCK"
        strSql += vbCrLf + "@DBNAME='" & cnStockDb & "'"
        strSql += vbCrLf + ",@TEMPTABLE='" & temptable & "'"
        strSql += vbCrLf + " ,@DATE = '" & dtpFrom.Value.ToString("dd-MMM-yyyy") & "'"
        strSql += vbCrLf + " ,@BILLCOUNTID = '" & cmbCounterID_own.SelectedValue.ToString() & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & SelectedCompanyId & "'"
        strSql += vbCrLf + " ,@COSTID='" & cmbCostCentre.Text & "'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)

        Dim dss As New DataSet
        da.Fill(dss)

        DtGrid = dss.Tables(0)
        DtGrid.Columns.Add("KEYNO", GetType(Integer))
        DtGrid.Columns("KEYNO").AutoIncrement = True
        DtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        DtGrid.Columns("KEYNO").AutoIncrementStep = 1

        DtGrid.Columns.Add("METALNAME", GetType(String))
        For i As Integer = 0 To DtGrid.Rows.Count - 1
            If DtGrid.Rows(i).Item("METALID").ToString = "G" Then
                DtGrid.Rows(i).Item("METALNAME") = "GOLD"
            ElseIf DtGrid.Rows(i).Item("METALID").ToString = "D" Then
                DtGrid.Rows(i).Item("METALNAME") = "DIAMOND"
            ElseIf DtGrid.Rows(i).Item("METALID").ToString = "S" Then
                DtGrid.Rows(i).Item("METALNAME") = "SILVER"
            Else
                DtGrid.Rows(i).Item("METALNAME") = ""
            End If
        Next
        If Not DtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            btnView_Search.Enabled = True
            Exit Function
        End If
        GridView2.DataSource = DtGrid
        DtGrid.Columns("KEYNO").SetOrdinal(DtGrid.Columns.Count - 1)
        GridView2.Columns("R_PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        GridView2.Columns("R_GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        GridView2.Columns("R_NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        GridView2.Columns("I_PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        GridView2.Columns("I_GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        GridView2.Columns("I_NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        GridView2.Columns("KEYNO").Visible = False
        GridView2.Columns("METALID").Visible = False
        GridView2.Columns("COLHEAD").Visible = False
        'GridView2.Columns("METALID").Visible = False

        GridView2.Columns("R_GRSWT").DefaultCellStyle.Format = "0.000"
        GridView2.Columns("R_NETWT").DefaultCellStyle.Format = "0.000"
        GridView2.Columns("I_GRSWT").DefaultCellStyle.Format = "0.000"
        GridView2.Columns("I_NETWT").DefaultCellStyle.Format = "0.000"

        'FillGridGroupStyle_KeyNoWise(GridView2)
        GridViewFormat()
        GridView2.Focus()
    End Function

    Private Sub chkCompanySelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCompanySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        dtpFrom.Value = GetServerDate()
        funcAddCostCentre()
        funcAddCashCounter()
        GridView2.DataSource = Nothing
        Prop_Gets()
        cmbCostCentre.Text = IIf(cnDefaultCostId, "ALL", cnCostName)
        btnView_Search.Enabled = True
        chkCompanySelectAll.Focus()
    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        GridView2.DataSource = Nothing
        GetViewDetails()
        Prop_Sets()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

#Region "Get&Set Properties"
    Private Sub Prop_Gets()
        Dim obj As New frmCashCounterStock_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmCashCounterStock_Properties))
        chkCompanySelectAll.Checked = obj.p_chkCompanySelectAll
        SetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany, strCompanyName)
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmCashCounterStock_Properties
        obj.p_chkCompanySelectAll = chkCompanySelectAll.Checked
        GetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany)
        SetSettingsObj(obj, Me.Name, GetType(frmCashCounterStock_Properties))
    End Sub
    Public Class frmCashCounterStock_Properties

        Private chkCompanySelectAll As Boolean = False
        Public Property p_chkCompanySelectAll() As Boolean
            Get
                Return chkCompanySelectAll
            End Get
            Set(ByVal value As Boolean)
                chkCompanySelectAll = value
            End Set
        End Property
        Private chkLstCompany As New List(Of String)
        Public Property p_chkLstCompany() As List(Of String)
            Get
                Return chkLstCompany
            End Get
            Set(ByVal value As List(Of String))
                chkLstCompany = value
            End Set
        End Property
    End Class
#End Region

  
    Private Sub frmCashCounterStock_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If

    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If GridView2.Rows.Count > 0 Then
            Dim title As String
            title = "STOCK REPORT"
            title += " For Date : " & dtpFrom.Text & " ."
            If cmbCostCentre.Text <> "ALL" And cmbCostCentre.Text <> "" Then title += " Costcentre : " & cmbCostCentre.Text & " ."
            BrightPosting.GExport.Post(Me.Name, strCompanyName, title, GridView2, BrightPosting.GExport.GExportType.Print)
        End If

    End Sub
    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In GridView2.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "T1"

                    Case "T2"
                        .Cells("PARTICULARS").Style.BackColor = Color.LightBlue
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "T3"
                        .Cells("PARTICULARS").Style.BackColor = Color.LightGreen
                        .Cells("PARTICULARS").Style.ForeColor = Color.Red
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                End Select

                Select Case .Cells("METALNAME").Value.ToString
                    Case "GOLD"
                        .Cells("METALNAME").Style.BackColor = Color.Gold
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "DIAMOND"
                        .Cells("METALNAME").Style.BackColor = Color.LavenderBlush
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "SILVER"
                        .Cells("METALNAME").Style.BackColor = Color.LightSkyBlue
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                End Select
            End With
        Next
    End Function

    'Function DotmatrixPrint()

    '    Dim dtprint As New DataTable
    '    Dim i As Integer
    '    Dim dt As New DataTable
    '    Dim headname As String
    '    Dim title As String = "STOCK REPORT"
    '    Dim metalname As String

    '    dtprint.Clear()
    '    dtprint = GridView2.DataSource

    '    'strSql = "SELECT COMPANYNAME,ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4 FROM " & cnAdminDb & "..COMPANY"
    '    'da = New OleDbDataAdapter(strSql, cn)
    '    'da.Fill(dt)

    '    'CompanyName = dt.Rows(1).Item("COMPANYNAME").ToString
    '    'Address1 = dt.Rows(1).Item("ADDRESS1").ToString
    '    'Address2 = dt.Rows(1).Item("ADDRESS2").ToString
    '    'Address3 = dt.Rows(1).Item("ADDRESS3").ToString


    '    strSql = "SELECT METALID FROM TEMPTABLEDB.." & temptable & " GROUP BY METALID "
    '    da = New OleDbDataAdapter(strSql, cn)
    '    da.Fill(dt)


    '    FileWrite = File.CreateText(Application.StartupPath + "\FilePrint.txt")
    '    PgNo = 0
    '    line = 0
    '    strprint = Chr(27) + "M"
    '    FileWrite.WriteLine(strprint)
    '    strprint = "-----------------------------------------------------------------------------------------------"
    '    FileWrite.WriteLine(Trim(strprint)) : line += 1
    '    strprint = Space((80 - Len(title)) / 2) + title
    '    FileWrite.WriteLine(strprint) : line += 1
    '    strprint = "-----------------------------------------------------------------------------------------------"
    '    FileWrite.WriteLine(Trim(strprint)) : line += 1

    '    strprint = "CASH COUNTER" & cmbCounterID_own.Text
    '    FileWrite.WriteLine(Trim(strprint)) : line += 1
    '    'strprint = "LOCATION" & cmbCounterID_own.Text
    '    'FileWrite.WriteLine(Trim(strprint)) : line += 1

    '    Dim str1 As String = Space(30) : Dim str2 As String = Space(10) : Dim str3 As String = Space(10)
    '    Dim str4 As String = Space(10) : Dim str5 As String = Space(10) : Dim str6 As String = Space(10)
    '    Dim str7 As String = Space(10)

    '    If dt.Rows.Count > 0 Then
    '        For ij As Integer = 0 To dt.Rows.Count - 1
    '            headname = dt.Rows(ij).Item("METALID").ToString
    '            If headname = "G" Then
    '                metalname = "GOLD"
    '                Printhead(metalname)
    '            ElseIf headname = "D" Then
    '                metalname = "DIAMOND"
    '                Printhead(metalname)
    '            ElseIf headname = "S" Then
    '                metalname = "SELVER"
    '                Printhead(metalname)
    '            Else
    '                metalname = ""
    '            End If

    '            Dim dtrows() As DataRow = dtprint.Select("AcName ='" & headname & "'")
    '            If dtrows.Length > 0 Then
    '                For i = 0 To dtrows.Length - 1

    '                    str1 = LSet(dtrows(i).Item("PARTICULAR").ToString, 30)
    '                    str2 = RSet(dtrows(i).Item("R_PCS").ToString, 10)
    '                    str3 = RSet(dtrows(i).Item("R_GRSWT").ToString, 10)
    '                    str4 = LSet(dtrows(i).Item("R_NETWT").ToString, 10)
    '                    str5 = RSet(dtrows(i).Item("I_PCS").ToString, 10)
    '                    str6 = LSet(dtrows(i).Item("I_GRSWT").ToString, 10)
    '                    str7 = RSet(dtrows(i).Item("I_NETWT").ToString, 10)

    '                    strprint = str1 + str2 + str3 + str4 + str5 + str6 + str7
    '                    FileWrite.WriteLine(strprint)
    '                    line += 1

    '                    If line >= 61 Then
    '                        strprint = "-----------------------------------------------------------------------------------------------"
    '                        FileWrite.WriteLine(strprint)
    '                        strprint = Chr(12)
    '                        FileWrite.WriteLine(strprint)
    '                        Printheader()
    '                    End If
    '                Next
    '            End If
    '        Next
    '    End If
    '    FileWrite.Close()
    '    line += 1
    '    frmPrinterSelect.Show()
    'End Function

    'Function Printhead(ByVal metalname As String) As Integer

    '    Dim str1 As String = Space(30) : Dim str2 As String = Space(10) : Dim str3 As String = Space(10)
    '    str1 = LSet("-STOCK", 30)
    '    str2 = RSet("RECEIPT", 10)
    '    str3 = RSet("ISSUE", 10)

    '    strprint = str1 + Space(10) + str2 + Space(20) + str3
    '    FileWrite.WriteLine(strprint) : line += 1
    '    strprint = "-----------------------------------------------------------------------------------------------"
    '    FileWrite.WriteLine(Trim(strprint)) : line += 1


    'End Function
    'Function Printheader() As Integer
    '    ' PgNo += 1
    '    line = 0

    '    strprint = "-----------------------------------------------------------------------------------------------"
    '    FileWrite.WriteLine(Trim(strprint)) : line += 1

    '    Dim str1 As String = Space(30) : Dim str2 As String = Space(10) : Dim str3 As String = Space(10)
    '    Dim str4 As String = Space(10) : Dim str5 As String = Space(10) : Dim str6 As String = Space(10)
    '    Dim str7 As String = Space(10)

    '    str1 = LSet("Particulars", 30)
    '    str2 = RSet("Pcs", 10)
    '    str3 = RSet("Gross Wt", 10)
    '    str4 = LSet("Net Wt ", 10)
    '    str5 = RSet("Pcs", 10)
    '    str6 = RSet("Gross Wt", 10)
    '    str7 = LSet("Net Wt ", 10)

    '    strprint = str1 + str2 + str3 + str4 + str5 + str6 + str7
    '    FileWrite.WriteLine(strprint) : line += 1
    '    strprint = "-----------------------------------------------------------------------------------------------"
    '    FileWrite.WriteLine(Trim(strprint)) : line += 1
    'End Function
End Class