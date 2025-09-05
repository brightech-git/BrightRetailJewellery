Imports System.Data.OleDb
Public Class frmCashPointRpt
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim strSql As String
    Dim dt As DataTable
    Dim DtGrid As New DataTable
    Dim SelectedCompanyId As String
    Dim SelectCostcenter As String
    Dim Detailed As String = ""
    Dim temptable As String
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        AddHandler chkLstCompany.LostFocus, AddressOf CheckedListCompany_Lostfocus
        '  ProcAddNodeId()

    End Sub
    Private Sub frmCashPointRpt_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        btnNew_Click(Me, New EventArgs)
        LoadCompany(chkLstCompany)
    End Sub

    Private Sub frmCashPointRpt_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
        If UCase(e.KeyChar) = Chr(Keys.X) Then
            btnExport_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        gridview.DataSource = Nothing
        Prop_Gets()
        gridview.DataSource = Nothing
        funcAddCostCentre()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridview.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "CASH POINT REPORT", gridview, BrightPosting.GExport.GExportType.Print)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridview.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "CASH POINT REPORT", gridview, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmItemWiseSales_properties
        obj.p_chkCompanySelectAll = chkCompanySelectAll.Checked
        GetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany)
        SetSettingsObj(obj, Me.Name, GetType(frmItemWiseSales_properties))
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New frmItemWiseSales_properties
        GetSettingsObj(obj, Me.Name, GetType(frmItemWiseSales_properties))
        chkCompanySelectAll.Checked = obj.p_chkCompanySelectAll
        SetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany, strCompanyName)
    End Sub
   

  
    Private Sub chkCompanySelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCompanySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
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
#Region "frm Properties"
    Public Class frmCashPointRpt_properties
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
        Private chkLstCostCentre As New List(Of String)
        Public Property p_chkLstCostCentre() As List(Of String)
            Get
                Return chkLstCostCentre
            End Get
            Set(ByVal value As List(Of String))
                chkLstCostCentre = value
            End Set
        End Property
    End Class
#End Region
    Private Function ViewDetails()

        If Not chkLstCompany.CheckedItems.Count > 0 Then chkCompanySelectAll.Checked = True
        SelectedCompanyId = GetSelectedCompanyId(chkLstCompany, False)
        Dim Costid As String
        If cmbCostCentre.Text <> "ALL" Then
            Costid = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre.Text & "'", "COSTID", "ALL")
        End If
        If Costid = Nothing Then Costid = "ALL"
        If chkAdvance.Checked = True Then
            Detailed = "N"
        Else
            Detailed = "Y"
        End If
        temptable = "TEMP" & systemId & "CASHPOINTRPT"
        strSql = " EXEC " & cnAdminDb & "..SP_CASHPOINT_REPORT"
        strSql += vbCrLf + " @DBNAME='" & cnStockDb & "'"
        strSql += vbCrLf + " ,@TEMPTAB='" & temptable & "'"
        strSql += vbCrLf + " ,@FROMDATE = '" & dtpFrom.Value.ToString("dd-MMM-yyyy") & "'"
        strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("dd-MMM-yyyy") & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & SelectedCompanyId & "'"
        strSql += vbCrLf + " ,@COSTID='" & Costid & "'"
        strSql += vbCrLf + " ,@DETAILED='" & Detailed & "'"
        strSql += vbCrLf + " ,@PENDING='" & IIf(chkpending.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@CANCEL='" & IIf(ChkCancel.Checked, "Y", "N") & "'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)
        Dim DtGrid As New DataTable
        Dim dss As New DataSet
        da.Fill(dss)
        DtGrid = New DataTable()
        DtGrid = dss.Tables(0)
        DtGrid.Columns.Add("KEYNO", GetType(Integer))
        DtGrid.Columns("KEYNO").AutoIncrement = True
        DtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        DtGrid.Columns("KEYNO").AutoIncrementStep = 1
        da.Fill(DtGrid)
        If Not DtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            btnView_Search.Enabled = True
            Exit Function
        End If
        DtGrid.Columns("KEYNO").SetOrdinal(DtGrid.Columns.Count - 1)
        gridview.DataSource = Nothing
        gridview.DataSource = DtGrid
        If Detailed = "Y" Then
            gridview.Columns("TRANDATE").Visible = False
        End If
        For i As Integer = 0 To gridview.Rows.Count - 1
            Select Case gridview.Rows(i).Cells("COLHEAD").Value
                Case "T"
                    gridview.Rows(i).Cells("TRANNO").Style.BackColor = Color.Green
                    gridview.Rows(i).Cells("TRANNO").Style.ForeColor = Color.White
                    gridview.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                Case "T1"
                    gridview.Rows(i).Cells("TRANNO").Style.BackColor = Color.LightGreen
                    gridview.Rows(i).Cells("TRANNO").Style.ForeColor = Color.Black
                    gridview.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                Case "S"
                    If Detailed = "Y" Then
                        gridview.Rows(i).Cells("TRANNO").Style.BackColor = Color.Red
                        gridview.Rows(i).Cells("TRANNO").Style.ForeColor = Color.Red
                        gridview.Rows(i).Cells("TRANNO").Style.BackColor = Color.LightGreen
                        gridview.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    End If
                Case "G"
                    gridview.Rows(i).DefaultCellStyle.BackColor = Color.LightYellow
                    gridview.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)

            End Select
            'End If

        Next
        gridview.Columns("SALES").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridview.Columns("PURCHASE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridview.Columns("CRECEIVED").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridview.Columns("CPAID").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridview.Columns("ADVANCE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridview.Columns("CREDIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridview.Columns("CHEQUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridview.Columns("CHIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridview.Columns("DISCOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridview.Columns("CRECEIVED").HeaderText = "CASH RECEIVED"
        gridview.Columns("CPAID").HeaderText = "CASH PAID"
        gridview.Columns("COLHEAD").Visible = False
        gridview.Columns("KEYNO").Visible = False
        gridview.Columns("RESULT").Visible = False
        If Detailed = "Y" Then
            gridview.Columns("COLUSER").HeaderText = "COLLECTING USER"
            gridview.Columns("BILLUSER").HeaderText = "BILLING USER"
            gridview.Columns("COLUSER").Width = 150
            gridview.Columns("BILLUSER").Width = 150
            gridview.Columns("BATCHNO").Visible = False
            gridview.Columns("TRANNO").Width = 150
        Else
            gridview.Columns("SALES").HeaderText = "AMOUNT"
        End If
        gridview.Columns("CRECEIVED").Width = 145
        FillGridGroupStyle_KeyNoWise(gridview)
        gridview.Focus()
    End Function

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        ViewDetails()
    End Sub

    Private Sub gridview_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridview.KeyPress
        Try
            If UCase(e.KeyChar) = "C" Then
                If chkAdvance.Checked = True Then Exit Sub
                If gridview.CurrentRow.Cells("COLHEAD").Value.ToString() = "P" Then
                    MsgBox("This Entry already in pending.")
                    Exit Sub
                End If
                Dim objSecret As New frmAdminPassword()
                If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then
                    Exit Sub
                End If

                strSql = "update " & cnStockDb & "..ACCTRAN SET CASHPOINTID='' WHERE TRANDATE='" & gridview.CurrentRow.Cells("TRANDATE").Value.ToString() & "' "
                strSql += vbCrLf + " AND BATCHNO='" & gridview.CurrentRow.Cells("BATCHNO").Value.ToString() & "'"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()

                strSql = "UPDATE  A SET A.USERID=I.USERID FROM " & cnStockDb & "..ACCTRAN A"
                strSql += vbCrLf + " LEFT JOIN " & cnStockDb & "..ISSUE I ON A.BATCHNO=I.BATCHNO"
                strSql += vbCrLf + " WHERE A.TRANDATE='" & gridview.CurrentRow.Cells("TRANDATE").Value.ToString() & "' "
                strSql += vbCrLf + " AND A.BATCHNO='" & gridview.CurrentRow.Cells("BATCHNO").Value.ToString() & "'"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()

                btnView_Search_Click(Me, New EventArgs)
            ElseIf UCase(e.KeyChar) = "D" Then
                If chkAdvance.Checked = True Then Exit Sub
                If gridview.CurrentRow.Cells("COLHEAD").Value.ToString() = "P" Then Exit Sub
                If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                    Dim prnmemsuffix As String = ""
                    If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                    Dim memfile As String = "\BillPrint" + prnmemsuffix.Trim & ".mem"

                    Dim write As IO.StreamWriter
                    write = IO.File.CreateText(Application.StartupPath & memfile)
                    write.WriteLine(LSet("TYPE", 15) & ":CXP")
                    write.WriteLine(LSet("BATCHNO", 15) & ":" & gridview.CurrentRow.Cells("BATCHNO").Value.ToString())
                    write.WriteLine(LSet("TRANDATE", 15) & ":" & gridview.CurrentRow.Cells("TRANDATE").Value.ToString)
                    write.WriteLine(LSet("DUPLICATE", 15) & ":N")
                    write.Flush()
                    write.Close()
                    If EXE_WITH_PARAM = False Then
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                    Else
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe", _
                        LSet("TYPE", 15) & ":CXP" & ";" & _
                        LSet("BATCHNO", 15) & ":" & gridview.CurrentRow.Cells("BATCHNO").Value.ToString & ";" & _
                        LSet("TRANDATE", 15) & ":" & gridview.CurrentRow.Cells("TRANDATE").Value.ToString & ";" & _
                        LSet("DUPLICATE", 15) & ":Y")
                    End If
                Else
                    MsgBox("Billprint exe not found", MsgBoxStyle.Information)
                End If


            End If
        Catch ex As Exception
            MsgBox("Error   :" + ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub gridview_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridview.CellContentClick

    End Sub

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click

    End Sub
End Class