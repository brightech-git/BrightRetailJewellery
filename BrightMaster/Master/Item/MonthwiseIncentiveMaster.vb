Imports System.Data.OleDb
Imports System.IO
Public Class MonthwiseIncentiveMaster
    'Developed By Sathyaraj on(04-01-2014)
#Region "Variable Declaration"

    Dim strSql As String = Nothing
    Private DtTran As New DataTable
    Dim da As New OleDbDataAdapter
    Dim Tablename As New List(Of String)
    Private _Tran As OleDbTransaction
    Dim cmd As New OleDbCommand
    Dim dttemp As New DataTable
    Dim dtcos, dt As New DataTable
    Dim flag As String = "M" 'FLAG="M" THEN MONTHWISE INCENTIVE ELSE BACKEND INCENTIVE
    Dim excelflag As Boolean = False

#End Region

#Region "CONSTRUCTOR"
    Public Sub New(ByVal type As String)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Me.WindowState = FormWindowState.Maximized
        flag = type
        ' Add any initialization after the InitializeComponent() call.
    End Sub
#End Region

#Region "TOOL STRIP"
    Private Sub saveToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles saveToolStripMenuItem1.Click
        If flag = "M" Then
            btnSave_Click(e, New EventArgs())
        Else
            btnsaveB_Click(e, New EventArgs())
        End If
    End Sub

    Private Sub openToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles openToolStripMenuItem2.Click
        If flag = "M" Then
            btnView_Click(e, New EventArgs())
        Else
            btnveiwB_Click(e, New EventArgs())
        End If
    End Sub

    Private Sub newToolStripMenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles newToolStripMenuItem3.Click
        If flag = "M" Then
            btnNew_Click(e, New EventArgs())
        Else
            btnNewB_Click(e, New EventArgs())
        End If
    End Sub

    Private Sub exitToolStripMenuItem4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles exitToolStripMenuItem4.Click
        If flag = "M" Then
            btnExit_Click(e, New EventArgs())
        Else
            BtnExitB_Click(e, New EventArgs())
        End If
    End Sub

#End Region
    
#Region "Forms Events"

    Private Sub MetalwiseIncentiveMaster_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dtGrid.Focused Or txt_Gridcell.Focused Or gridviewB.Focused Or txtGridB.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        ElseIf e.KeyCode = Keys.F12 Then
            Me.Close()
        ElseIf e.KeyCode = Keys.Escape Then
            If flag = "M" Then If dtGrid.Focused Or txt_Gridcell.Focused Then txt_Gridcell_Leave(Me, New EventArgs()) : txt_Gridcell.Hide() : btnSave.Focus()
            If flag = "B" Then If gridviewB.Focused Or txtGridB.Focused Then btnsaveB.Focus()
        End If
    End Sub

    Private Sub MetalwiseIncentiveMaster_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        funcDecimalValidation(txt_Gridcell)
        funcDecimalValidation(txtGridB)
        FuncNew()
    End Sub

#End Region

#Region "User Define Function"

    Function FuncNew()
        cmbMetal_OWN.Items.Clear()
        cmbMonth.Items.Clear()
        objGPack.FillCombo("SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE ISNULL(ACTIVE,'')='Y'", cmbMetal_OWN, False, False)
        objGPack.FillCombo("SELECT DATENAME(MONTH, CAST('2013 -'+ LTRIM(RTRIM(CAST(NUMBER AS VARCHAR(2)))) + '-1' AS DATETIME)) MONTHNAME FROM MASTER..SPT_VALUES WHERE TYPE = 'P' AND NUMBER BETWEEN 1 AND 12", cmbMonth, False, False)
        objGPack.FillCombo("SELECT DATENAME(MONTH, CAST('2013 -'+ LTRIM(RTRIM(CAST(NUMBER AS VARCHAR(2)))) + '-1' AS DATETIME)) MONTHNAME FROM MASTER..SPT_VALUES WHERE TYPE = 'P' AND NUMBER BETWEEN 1 AND 12", cmbMonthB_OWN, False, False)
        objGPack.FillCombo("SELECT DATENAME(MONTH, CAST('2013 -'+ LTRIM(RTRIM(CAST(NUMBER AS VARCHAR(2)))) + '-1' AS DATETIME)) MONTHNAME FROM MASTER..SPT_VALUES WHERE TYPE = 'P' AND NUMBER BETWEEN 1 AND 12", cmbMonth_TarB_OWN, False, False)
        objGPack.FillCombo("SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE ISNULL(ACTIVE,'')='Y'", cmbcostcentre_OWN, False, False)
        cmbEntryType_OWN.SelectedIndex = 0
        cmbentrytypeB_OWN.SelectedIndex = 0
        dtGrid.DataSource = Nothing
        gridviewB.DataSource = Nothing
        dtpFrom.Value = System.DateTime.Now.ToString
        dtpTo.Value = System.DateTime.Now.ToString
        txtGridB.Text = ""
        txt_Gridcell.Text = ""
        txtGridB.Visible = False
        lbldel.Visible = False
        txt_Gridcell.Visible = False
        tabmain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabmain.Region = New Region(New RectangleF(Me.Tabempwiseinc.Left, Me.Tabempwiseinc.Top, Me.Tabempwiseinc.Width, Me.Tabempwiseinc.Height))
        If flag = "M" Then
            tabmain.SelectedTab = Tabempwiseinc
        Else
            tabmain.SelectedTab = Tabbackendinc
        End If
    End Function

    Function FuncSave()
        Try
            If chkflatincentive.Checked Then
                If cmbMonth.Text = "" Then MsgBox("Month Should not empty") : cmbMonth.Focus() : Return 0
                If cmbEntryType_OWN.Text = "" Then MsgBox("Month Should not empty") : cmbEntryType_OWN.Focus() : Return 0
                If cmbEntryType_OWN.Text = "INSERT" Then
                    If MsgBox("Are you sure to Save.", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Return 0
                    For i As Integer = 0 To dtGrid.Rows.Count - 1
                        If dtGrid.Rows(i).Cells("AMOUNT").Value = 0 Or dtGrid.Rows(i).Cells("COSTNAME").Value.ToString = "TOTAL" Then Continue For
                        strSql = " SELECT COUNT(*) FROM " & cnAdminDb & "..FLATINCENTIVE  WHERE MONTH='" & cmbMonth.Text.ToString & "'"
                        strSql += vbCrLf + " AND COSTID='" & dtGrid.Rows(i).Cells("COSTID").Value.ToString & "'"
                        If Val(GetSqlValue(cn, strSql)) > 0 Then
                            If MsgBox("Target was already alloted for the Costid[" & Val(dtGrid.Rows(i).Cells("costid").Value.ToString) & "]" _
                            & vbCrLf & "Are you like to skip this. Press ok else Press cancel to cancel save..", MsgBoxStyle.OkCancel) = MsgBoxResult.Cancel Then GoTo SKIP Else Continue For
                        End If
                        strSql = " INSERT INTO " & cnAdminDb & "..FLATINCENTIVE(COSTID,MONTH,AMOUNT) "
                        strSql += vbCrLf + " VALUES( "
                        strSql += vbCrLf + " '" & dtGrid.Rows(i).Cells("COSTID").Value.ToString & "'"
                        strSql += vbCrLf + ",'" & cmbMonth.Text.ToString & "'"
                        strSql += vbCrLf + ",'" & Val(dtGrid.Rows(i).Cells("AMOUNT").Value.ToString) & "')"
                        cmd = New OleDbCommand(strSql, cn)
                        cmd.ExecuteNonQuery()
                    Next
                    MsgBox("Saved Sucessfully", MsgBoxStyle.Information)
                    btnNew_Click(Me, New EventArgs())
                ElseIf cmbEntryType_OWN.Text = "UPDATE" Then
                    If MsgBox("Are you sure to Update.", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Return 0
                    For i As Integer = 0 To dtGrid.Rows.Count - 1
                        If dtGrid.Rows(i).Cells("COSTNAME").Value.ToString = "TOTAL" Then Continue For
                        strSql = " UPDATE " & cnAdminDb & "..FLATINCENTIVE SET AMOUNT= '" & Val(dtGrid.Rows(i).Cells("AMOUNT").Value.ToString) & "' "
                        strSql += vbCrLf + " WHERE "
                        strSql += vbCrLf + " COSTID='" & dtGrid.Rows(i).Cells("COSTID").Value.ToString & "'"
                        strSql += vbCrLf + " AND MONTH='" & cmbMonth.Text & "'"
                        cmd = New OleDbCommand(strSql, cn)
                        cmd.ExecuteNonQuery()
                    Next
                    MsgBox("Updated Sucessfully", MsgBoxStyle.Information)
                    btnNew_Click(Me, New EventArgs())
                End If
            Else
                If validation("S") = False Then Exit Function
                If flag = "M" Then
                    If cmbEntryType_OWN.Text = "INSERT" Then
                        For i As Integer = 0 To dtGrid.Rows.Count - 1
                            If dtGrid.Rows(i).Cells("TARGETWEIGHT").Value = 0 Or dtGrid.Rows(i).Cells("EMPID").Value.ToString = "TOTAL" Then Continue For
                            strSql = " SELECT COUNT(*) FROM " & cnAdminDb & "..EMPWISEINCENTIVE  WHERE MONTH='" & cmbMonth.Text.ToString & "'"
                            strSql += vbCrLf + " AND EMPID='" & Val(dtGrid.Rows(i).Cells("EMPID").Value.ToString) & "' AND METALID='" & dtGrid.Rows(i).Cells("METALID").Value.ToString & "'"
                            strSql += vbCrLf + " AND COSTID='" & dtGrid.Rows(i).Cells("COSTID").Value.ToString & "'"
                            If Val(GetSqlValue(cn, strSql)) > 0 Then
                                If MsgBox("Target was already alloted for the Empid[" & Val(dtGrid.Rows(i).Cells("EMPID").Value.ToString) & "]" _
                                & vbCrLf & "Are you like to skip this. Press ok else Press cancel to cancel save..", MsgBoxStyle.OkCancel) = MsgBoxResult.Cancel Then GoTo SKIP Else Continue For
                            End If
                            strSql = " INSERT INTO " & cnAdminDb & "..EMPWISEINCENTIVE(EMPID,MONTH,METALID,COSTID,WEIGHT) "
                            strSql += vbCrLf + " VALUES( "
                            strSql += vbCrLf + " '" & Val(dtGrid.Rows(i).Cells("EMPID").Value.ToString) & "'"
                            strSql += vbCrLf + ",'" & cmbMonth.Text.ToString & "'"
                            strSql += vbCrLf + ",'" & dtGrid.Rows(i).Cells("METALID").Value.ToString & "'"
                            strSql += vbCrLf + ",'" & dtGrid.Rows(i).Cells("COSTID").Value.ToString & "'"
                            strSql += vbCrLf + ",'" & Val(dtGrid.Rows(i).Cells("TARGETWEIGHT").Value.ToString) & "')"
                            cmd = New OleDbCommand(strSql, cn)
                            cmd.ExecuteNonQuery()
                        Next
                        MsgBox("Saved Sucessfully", MsgBoxStyle.Information)
                        btnNew_Click(Me, New EventArgs())
                    ElseIf cmbEntryType_OWN.Text = "UPDATE" Then
                        For i As Integer = 0 To dtGrid.Rows.Count - 1
                            If dtGrid.Rows(i).Cells("EMPID").Value.ToString = "TOTAL" Then Continue For
                            If dtGrid.Rows(i).Cells("TARGETWEIGHT").Value = 0 Or dtGrid.Rows(i).Cells("EMPID").Value.ToString = "TOTAL" Then Continue For
                            strSql = " SELECT COUNT(*) FROM " & cnAdminDb & "..EMPWISEINCENTIVE  WHERE MONTH='" & cmbMonth.Text.ToString & "'"
                            strSql += vbCrLf + " AND EMPID='" & Val(dtGrid.Rows(i).Cells("EMPID").Value.ToString) & "' AND METALID='" & dtGrid.Rows(i).Cells("METALID").Value.ToString & "'"
                            strSql += vbCrLf + " AND COSTID='" & dtGrid.Rows(i).Cells("COSTID").Value.ToString & "'"
                            If Val(GetSqlValue(cn, strSql)) > 0 Then
                                strSql = " UPDATE " & cnAdminDb & "..EMPWISEINCENTIVE SET WEIGHT= '" & Val(dtGrid.Rows(i).Cells("TARGETWEIGHT").Value.ToString) & "' "
                                strSql += vbCrLf + " WHERE "
                                strSql += vbCrLf + " EMPID='" & Val(dtGrid.Rows(i).Cells("EMPID").Value.ToString) & "'"
                                strSql += vbCrLf + " AND MONTH='" & cmbMonth.Text & "'"
                                strSql += vbCrLf + " AND METALID='" & dtGrid.Rows(i).Cells("METALID").Value.ToString & "'"
                                strSql += vbCrLf + " AND COSTID='" & dtGrid.Rows(i).Cells("COSTID").Value.ToString & "'"
                                cmd = New OleDbCommand(strSql, cn)
                                cmd.ExecuteNonQuery()
                            Else
                                strSql = " INSERT INTO " & cnAdminDb & "..EMPWISEINCENTIVE "
                                strSql += vbCrLf + " ("
                                strSql += vbCrLf + " EMPID,MONTH,METALID,WEIGHT,COSTID)"
                                strSql += vbCrLf + " VALUES "
                                strSql += vbCrLf + " ("
                                strSql += vbCrLf + " " & Val(dtGrid.Rows(i).Cells("EMPID").Value.ToString) 'EMPID
                                strSql += vbCrLf + " ,'" & cmbMonth.Text & "'" 'MONTH
                                strSql += vbCrLf + " ,'" & dtGrid.Rows(i).Cells("METALID").Value.ToString & "'" 'METALID
                                strSql += vbCrLf + " ," & Val(dtGrid.Rows(i).Cells("TARGETWEIGHT").Value.ToString) 'TARGETWEIGHT
                                strSql += vbCrLf + " ,'" & dtGrid.Rows(i).Cells("COSTID").Value.ToString & "'" 'COSTID
                                strSql += vbCrLf + " )"
                                cmd = New OleDbCommand(strSql, cn)
                                cmd.ExecuteNonQuery()
                            End If
                        Next
                        MsgBox("Updated Sucessfully", MsgBoxStyle.Information)
                        btnNew_Click(Me, New EventArgs())
                    End If
                Else
                    If gridviewB.Rows.Count <= 0 Then MsgBox("No record is there to save..") : btnopenB.Focus() : Exit Function
                    If cmbentrytypeB_OWN.Text = "INSERT" Or cmbentrytypeB_OWN.Text = "EXTENSION" Then
                        If MsgBox("Are you sure to save..", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Exit Function
                        For i As Integer = 0 To gridviewB.Rows.Count - 2
                            If Val(gridviewB.Rows(i).Cells("AMOUNT").Value.ToString) = 0 Or gridviewB.Rows(i).Cells("ITEMNAME").Value.ToString = "TOTAL" Then Continue For
                            Dim itemid As Integer = Val(gridviewB.Rows(i).Cells("ITEMID").Value.ToString)
                            Dim amount As Double = Val(gridviewB.Rows(i).Cells("AMOUNT").Value.ToString)
                            strSql = " SELECT COUNT(*) FROM " & cnAdminDb & "..BACKENDINCENTIVE  WHERE MONTH='" & IIf(cmbentrytypeB_OWN.Text = "INSERT", cmbMonthB_OWN.Text, cmbMonth_TarB_OWN.Text) & "'"
                            strSql += " AND ITEMID='" & itemid & "'"
                            If Val(GetSqlValue(cn, strSql)) > 0 Then
                                If MsgBox("Incentive was already alloted for the itemid[" & itemid & "]" & vbCrLf & "Are you like to skip this. Press ok else Press cancel to cance save..", MsgBoxStyle.OkCancel) = MsgBoxResult.Cancel Then GoTo SKIP Else Continue For
                            End If
                            strSql = " INSERT INTO " & cnAdminDb & "..BACKENDINCENTIVE(ITEMID,MONTH,AMOUNT) "
                            strSql += " VALUES( '" & itemid & "','" & IIf(cmbentrytypeB_OWN.Text = "INSERT", cmbMonthB_OWN.Text, cmbMonth_TarB_OWN.Text) & "','" & amount & "' )"
                            cmd = New OleDbCommand(strSql, cn)
                            cmd.ExecuteNonQuery()
                        Next
                        MsgBox("Saved Sucessfully", MsgBoxStyle.Information)
SKIP:
                    Else
                        For i As Integer = 0 To gridviewB.Rows.Count - 2
                            If gridviewB.Rows(i).Cells("ITEMNAME").Value.ToString = "TOTAL" Then Continue For
                            Dim itemid As Integer = Val(gridviewB.Rows(i).Cells("ITEMID").Value.ToString)
                            Dim amount As Double = Val(gridviewB.Rows(i).Cells("AMOUNT").Value.ToString)
                            strSql = " UPDATE " & cnAdminDb & "..BACKENDINCENTIVE SET AMOUNT=" & amount & " where ITEMID=" & itemid & " and MONTH='" & cmbMonthB_OWN.Text & "' "
                            cmd = New OleDbCommand(strSql, cn)
                            cmd.ExecuteNonQuery()
                        Next
                        MsgBox("Updated Sucessfully", MsgBoxStyle.Information)
                    End If
                    gridviewB.DataSource = Nothing
                    txtGridB.Text = ""
                    txtGridB.Visible = False
                    cmbentrytypeB_OWN.Select()
                End If
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Function

    Function validation(Optional ByVal chk As String = "O") As Boolean
        If flag = "M" Then
            If chk = "S" Then
                If dtGrid.Rows.Count <= 0 Then MsgBox("No record is there to save..") : btnopen.Focus() : Return False
                If MsgBox("Are you sure to save..", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Return False
            End If
            If excelflag = False Then
                If cmbEntryType_OWN.Text.Trim = "" Then
                    MsgBox("EntryType Should Not Be Empty", MsgBoxStyle.Information)
                    cmbEntryType_OWN.Select()
                    Return False
                End If
                If cmbcostcentre_OWN.Text.Trim = "" Then
                    MsgBox("CostCentre Should Not Be Empty", MsgBoxStyle.Information)
                    cmbcostcentre_OWN.Select()
                    Return False
                End If
                If cmbMetal_OWN.Text.Trim = "" Then
                    MsgBox("Metal Should Not Be Empty", MsgBoxStyle.Information)
                    cmbMetal_OWN.Select()
                    Return False
                End If
            Else
                For i As Integer = 0 To dtGrid.Rows.Count - 2
                    With dtGrid.Rows(i)
                        If Val(.Cells("TARGETWEIGHT").Value) = 0 Or .Cells("EMPID").Value.ToString = "TOTAL" Then Continue For
                        If Trim(.Cells("EMPID").Value) = "" Then MsgBox("EmpId should not be empty." & vbCrLf & " Pls update the excel Sheet and try again.") : dtGrid.Item("EMPID", i).Selected = True : Return False
                        If Trim(.Cells("METALID").Value) = "" Then MsgBox("Metalid is empty in some column of excel." & vbCrLf & " Pls update the excel Sheet and try again.") : Return False
                    End With
                Next
            End If
            If cmbMonth.Text.Trim = "" Then
                MsgBox("Month Should Not Be Empty", MsgBoxStyle.Information)
                cmbMonth.Select()
                Return False
            End If
            Return True
        ElseIf flag = "B" Then
            If chk = "S" Then
                If gridviewB.Rows.Count <= 0 Then MsgBox("No record is there to save..") : btnopenB.Focus() : Return False
                If MsgBox("Are you sure to save..", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Return False
            End If
            If cmbentrytypeB_OWN.Text.Trim = "" Then
                MsgBox("EntryType Should Not Be Empty", MsgBoxStyle.Information)
                cmbentrytypeB_OWN.Select()
                Return False
            End If
            If cmbMonthB_OWN.Text.Trim = "" Then
                MsgBox("Month Should Not Be Empty", MsgBoxStyle.Information)
                cmbMonthB_OWN.Select()
                Return False
            End If
            If IIf(cmbMonth_TarB_OWN.Enabled = True, cmbMonth_TarB_OWN.Text.Trim, "1") = "" Then
                MsgBox("TargetMonth Should Not Be Empty", MsgBoxStyle.Information)
                cmbMonth_TarB_OWN.Select()
                Return False
            End If
            Return True
        End If
    End Function

    Private Sub LoadFromExel(ByVal Path As String)
        Try
            DtTran.Rows.Clear()
            DtTran.AcceptChanges()
            Dim MyConnection As System.Data.OleDb.OleDbConnection
            MyConnection = New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" & Path & "';Extended Properties=""Excel 12.0 Xml;HDR=YES;""")
            strSql = "SELECT EMPID,EMPNAME,COSTID,TARGETWEIGHT,METALID  FROM [SHEET1$]"
            Da = New OleDbDataAdapter(StrSql, MyConnection)
            Dim Dt As New DataTable
            Dt.Columns.Add("EMPID", Type.GetType("System.String"))
            Dt.Columns.Add("EMPNAME", Type.GetType("System.String"))
            Dt.Columns.Add("COSTID", Type.GetType("System.String"))
            Dt.Columns.Add("TARGETWEIGHT", Type.GetType("System.Double"))
            Dt.Columns.Add("METALID", Type.GetType("System.String"))
            da.Fill(Dt)
            MyConnection.Close()
            If Dt.Rows.Count > 1 Then
                Dim ro As DataRow
                ro = Dt.NewRow()
                ro("EMPID") = "TOTAL"
                ro("TARGETWEIGHT") = Dt.Compute("SUM(TARGETWEIGHT)", "EMPID<>'TOTAL'")
                Dt.Rows.Add(ro)
                dtGrid.DataSource = Nothing
                dtGrid.DataSource = Dt
                dtGrid.Columns("EMPNAME").Width = 250
                dtGrid.Columns("METALID").Visible = False
                dtGrid.Columns("EMPNAME").ReadOnly = True
                dtGrid.Columns("EMPID").ReadOnly = True
                dtGrid.Columns("COSTID").ReadOnly = True
                dtGrid.Columns("TARGETWEIGHT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                dtGrid.Columns("TARGETWEIGHT").DefaultCellStyle.Format = "#0.000"
                dtGrid.Focus()
                dtGrid.CurrentCell = dtGrid.Item(3, 0)
            End If
        Catch ex As Exception
            DtTran.Rows.Clear()
            DtTran.AcceptChanges()
            'MsgBox("Invalid File Format", MsgBoxStyle.Information)
            MsgBox(ex.Message)
        End Try

    End Sub

    Function exceltemplate()
        Dim rwStart As Integer = 0
        Dim oXL As Excel.Application
        Dim oWB As Excel.Workbook
        Dim oSheet As Excel.Worksheet
        Dim oRng As Excel.Range

        oXL = CreateObject("Excel.Application")
        oXL.Visible = True
        oWB = oXL.Workbooks.Add
        oSheet = oWB.ActiveSheet
        oSheet.Range("A1").Value = "EMPID"
        oSheet.Range("A1").ColumnWidth = 30
        oSheet.Range("B1").Value = "EMPNAME"
        oSheet.Range("B1").ColumnWidth = 50
        oSheet.Range("C1").Value = "COSTID"
        oSheet.Range("C1").ColumnWidth = 20
        oSheet.Range("D1").Value = "METALID"
        oSheet.Range("D1").ColumnWidth = 20
        oSheet.Range("E1").Value = "TARGETWEIGHT"
        oSheet.Range("E1").ColumnWidth = 30
        oSheet.Range("A1:B1:C1:D1:E1").Font.Bold = True
        oSheet.Range("A1:B1:C1:D1:E1").Font.Name = "VERDANA"
        oSheet.Range("A1:B1:C1:D1:E1").Font.Size = 8
        oSheet.Range("A1:B1:C1:D1:E1").HorizontalAlignment = Excel.Constants.xlCenter
    End Function

    Function FillCostnameCombo()
        strSql = "SELECT METALNAME,METALID FROM " & cnAdminDb & "..METALMAST WHERE ISNULL(ACTIVE,'')='Y' ORDER BY METALNAME "
        dt = New DataTable
        dt = GetSqlTable(strSql, cn)
        Return dt
    End Function

    Function funcDecimalValidation(ByVal f As TextBox) As Integer
        f.TextAlign = HorizontalAlignment.Right
        f.MaxLength = 15
        RemoveHandler (f.KeyPress), AddressOf Decimal_KeyPress
        AddHandler (f.KeyPress), AddressOf Decimal_KeyPress
        Return 0
    End Function

    Private Sub GetDetailForMonthorBackendIncentive()
        excelflag = False
        Dim metalfilter = " (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME='" & cmbMetal_OWN.Text & "')"
        Dim Costfilter = " (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbcostcentre_OWN.Text & "')"
        dtGrid.DataSource = Nothing
        txt_Gridcell.Visible = False
        txt_Gridcell.Text = ""
        If cmbEntryType_OWN.Text.Trim <> "" Then
            If cmbEntryType_OWN.Text.Trim = "INSERT" Then
                strSql = "SELECT CONVERT(VARCHAR(50),EMPID)EMPID,EMPNAME," & metalfilter & " METALID," & Costfilter & " COSTID,CONVERT(NUMERIC(15,3),0)TARGETWEIGHT FROM " & cnAdminDb & "..EMPMASTER"
                strSql += " WHERE EMPID NOT IN (SELECT EMPID FROM " & cnAdminDb & "..EMPWISEINCENTIVE "
                strSql += " WHERE 1=1 AND METALID IN " & metalfilter & " AND MONTH='" & cmbMonth.Text & "'AND ISNULL(COSTID,'') IN " & Costfilter & " )"
                strSql += " AND ISNULL(ACTIVE,'')='Y'"
                strSql += "ORDER BY EMPNAME,TARGETWEIGHT "
            ElseIf cmbEntryType_OWN.Text.Trim = "UPDATE" Then
                strSql = "SELECT CONVERT(VARCHAR(50),EI.EMPID)EMPID,EM.EMPNAME,METALID,EI.COSTID,EI.WEIGHT AS TARGETWEIGHT FROM " & cnAdminDb & "..EMPWISEINCENTIVE EI  "
                strSql += " INNER JOIN " & cnAdminDb & "..EMPMASTER EM on EI.EMPID=EM.EMPID WHERE 1=1"
                strSql += " AND ISNULL(EM.ACTIVE,'')='Y' AND METALID IN " & metalfilter & " AND ISNULL(EI.COSTID,'') IN " & Costfilter
                strSql += " AND ISNULL(MONTH,'')='" & cmbMonth.Text & "'"
                strSql += " ORDER BY EMPNAME "
            End If
            dt = New DataTable
            dt = GetSqlTable(strSql, cn)
            If dt.Rows.Count > 0 Then
                Dim ro As DataRow
                ro = dt.NewRow()
                ro("EMPID") = "TOTAL"
                ro("TARGETWEIGHT") = dt.Compute("SUM(TARGETWEIGHT)", "EMPID<>'TOTAL'")
                dt.Rows.Add(ro)
                dtGrid.DataSource = dt
                dtGrid.Columns("EMPNAME").Width = 250
                dtGrid.Columns("METALID").Visible = False
                dtGrid.Columns("EMPNAME").ReadOnly = True
                dtGrid.Columns("EMPID").ReadOnly = True
                dtGrid.Columns("COSTID").ReadOnly = True
                dtGrid.Columns("TARGETWEIGHT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                dtGrid.Focus()
                dtGrid.CurrentCell = dtGrid.Item(4, 0)
            Else
                dtGrid.DataSource = Nothing
                MsgBox("No record found..")
            End If
        Else
            MsgBox("Select EntryType..", MsgBoxStyle.Information)
            cmbEntryType_OWN.Select()
            Exit Sub
        End If
    End Sub

    Private Sub GetDetailForMonthwiseFlatIncentive()
        dtGrid.DataSource = Nothing
        dtGrid.Refresh()
        strSql = "IF OBJECT_ID('" & cnAdminDb & "..FLATINCENTIVE')IS NULL CREATE TABLE " & cnAdminDb & "..FLATINCENTIVE(COSTID VARCHAR(2),MONTH VARCHAR(15),AMOUNT NUMERIC(15,3),CONSTRAINT [COSTID_MONTH] UNIQUE NONCLUSTERED(COSTID,MONTH))"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        If cmbEntryType_OWN.Text.Trim <> "" Then
            If cmbEntryType_OWN.Text.Trim = "INSERT" Then
                strSql = " SELECT DISTINCT C.COSTNAME,'" & cmbMonth.Text & "'MONTH,ISNULL(AMOUNT,0)AMOUNT,C.COSTID  FROM " & cnAdminDb & "..COSTCENTRE C"
                strSql += " LEFT JOIN " & cnAdminDb & "..FLATINCENTIVE AS F ON C.COSTID=F.COSTID AND F.MONTH='" & cmbMonth.Text & "'"
                strSql += " WHERE C.COSTID NOT IN(SELECT COSTID FROM " & cnAdminDb & "..FLATINCENTIVE WHERE MONTH='" & cmbMonth.Text & "')"

            ElseIf cmbEntryType_OWN.Text.Trim = "UPDATE" Then
                strSql = " SELECT DISTINCT C.COSTNAME,ISNULL(F.MONTH,'')MONTH, ISNULL(AMOUNT,0)AMOUNT,C.COSTID  FROM " & cnAdminDb & "..COSTCENTRE C"
                strSql += " LEFT JOIN " & cnAdminDb & "..FLATINCENTIVE AS F ON C.COSTID=F.COSTID"
                strSql += " WHERE C.COSTID IN(SELECT COSTID FROM " & cnAdminDb & "..FLATINCENTIVE WHERE MONTH='" & cmbMonth.Text & "')"
            End If
            dt = New DataTable
            dt = GetSqlTable(strSql, cn)
            If dt.Rows.Count > 0 Then
                Dim ro As DataRow
                ro = dt.NewRow()
                ro("COSTNAME") = "TOTAL"
                ro("AMOUNT") = dt.Compute("SUM(AMOUNT)", "COSTNAME<>'TOTAL'")
                dt.Rows.Add(ro)
                dtGrid.DataSource = dt
                dtGrid.Columns("COSTNAME").Width = 250
                dtGrid.Columns("COSTNAME").ReadOnly = True
                dtGrid.Columns("MONTH").Visible = False
                dtGrid.Columns("COSTID").Visible = False
                dtGrid.Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                dtGrid.Focus()
                dtGrid.CurrentCell = dtGrid.Item(2, 0)
            Else
                dtGrid.DataSource = Nothing
                MsgBox("No record found..")
            End If
        End If
    End Sub

#End Region

#Region "MonthWiseIncentive Coding"

#Region "Controls Events"

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        FuncNew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub dtGrid_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtGrid.KeyDown
        If chkflatincentive.Checked = False Then Exit Sub
        If dtGrid.Rows.Count <= 0 Or dtGrid.CurrentRow.Cells("COSTNAME").Value = "TOTAL" Then Exit Sub
        If e.KeyCode = Keys.Delete Then
            If MsgBox("Are you sure to Delete current row.", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Exit Sub
            strSql = " DELETE FROM " & cnAdminDb & "..FLATINCENTIVE WHERE AMOUNT= '" & Val(dtGrid.CurrentRow.Cells("AMOUNT").Value.ToString) & "' "
            strSql += vbCrLf + " AND COSTID='" & dtGrid.CurrentRow.Cells("COSTID").Value.ToString & "'"
            strSql += vbCrLf + " AND MONTH='" & cmbMonth.Text.ToString & "'"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            MsgBox("Delete Successfully.")
        End If
    End Sub

    Private Sub dtGrid_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtGrid.Leave
        lbldel.Visible = False
        If dtGrid.RowCount <= 0 Then txt_Gridcell.Visible = False
    End Sub

    Private Sub dtGrid_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles dtGrid.Scroll
        txt_Gridcell.Text = ""
        txt_Gridcell.Visible = False
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnopen.Click
        txt_Gridcell.Visible = False
        If chkflatincentive.Checked Then
            If cmbMonth.Text = "" Then MsgBox("Month Should not empty.") : cmbMonth.Focus() : Exit Sub
            GetDetailForMonthwiseFlatIncentive()
        Else
            If validation() = False Then Exit Sub
            GetDetailForMonthorBackendIncentive()
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        FuncSave()
    End Sub

    Private Sub btnexport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnexport.Click
        If dtGrid.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "", dtGrid, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub Decimal_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs)
        Dim keyChar As String
        keyChar = e.KeyChar
        If AscW(keyChar) = 46 Then
            If CType(sender, TextBox).Text.Contains(".") Then
                e.Handled = True
            End If
        End If
        If (Not Char.IsDigit(keyChar)) And AscW(keyChar) <> 8 And AscW(keyChar) <> 13 And AscW(keyChar) <> 46 And e.KeyChar <> Chr(Keys.Escape) And e.KeyChar <> "-" Then
            e.Handled = True
            MsgBox("Digits only Allowed 1 to 9")
            CType(sender, TextBox).Focus()
        End If
    End Sub

    Private Sub dtGrid_CellEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dtGrid.CellEnter
        If dtGrid.Rows.Count <= 0 Then Exit Sub
        If chkflatincentive.Checked Then lbldel.Visible = True Else lbldel.Visible = False
        If dtGrid.Columns(dtGrid.CurrentCell.ColumnIndex).Name = "TARGETWEIGHT" Then
            If dtGrid.CurrentCell.ReadOnly = True Or dtGrid.CurrentRow.Cells("EMPID").Value.ToString = "TOTAL" Then txt_Gridcell.Hide() : Exit Sub
            If dtGrid.CurrentCell.Value Is Nothing Or IsDBNull(dtGrid.CurrentCell.Value) Then : txt_Gridcell.Text = String.Empty
            Else : txt_Gridcell.Text = dtGrid.CurrentCell.Value : End If
            Dim CurrentCellx As Integer = dtGrid.GetCellDisplayRectangle(dtGrid.CurrentCell.ColumnIndex, dtGrid.CurrentRow.Index, False).Left + dtGrid.Left
            Dim CurrentCelly As Integer = dtGrid.GetCellDisplayRectangle(dtGrid.CurrentCell.ColumnIndex, dtGrid.CurrentRow.Index, False).Top + dtGrid.Top
            txt_Gridcell.Location = New Point(CurrentCellx, CurrentCelly)
            txt_Gridcell.Size = New Size(dtGrid.CurrentCell.Size.Width - 2, dtGrid.CurrentCell.Size.Height)
            txt_Gridcell.BorderStyle = BorderStyle.None
            txt_Gridcell.Show()
            txt_Gridcell.BringToFront()
            txt_Gridcell.Select()
        ElseIf dtGrid.Columns(dtGrid.CurrentCell.ColumnIndex).Name = "AMOUNT" And chkflatincentive.Checked Then
            If dtGrid.CurrentCell.ReadOnly = True Or dtGrid.CurrentRow.Cells("COSTNAME").Value.ToString = "TOTAL" Then txt_Gridcell.Hide() : Exit Sub
            If dtGrid.CurrentCell.Value Is Nothing Or IsDBNull(dtGrid.CurrentCell.Value) Then : txt_Gridcell.Text = String.Empty
            Else : txt_Gridcell.Text = dtGrid.CurrentCell.Value : End If
            Dim CurrentCellx As Integer = dtGrid.GetCellDisplayRectangle(dtGrid.CurrentCell.ColumnIndex, dtGrid.CurrentRow.Index, False).Left + dtGrid.Left
            Dim CurrentCelly As Integer = dtGrid.GetCellDisplayRectangle(dtGrid.CurrentCell.ColumnIndex, dtGrid.CurrentRow.Index, False).Top + dtGrid.Top
            txt_Gridcell.Location = New Point(CurrentCellx, CurrentCelly)
            txt_Gridcell.Size = New Size(dtGrid.CurrentCell.Size.Width - 2, dtGrid.CurrentCell.Size.Height)
            txt_Gridcell.BorderStyle = BorderStyle.None
            txt_Gridcell.Show()
            txt_Gridcell.BringToFront()
            txt_Gridcell.Select()
        Else
            txt_Gridcell.Hide()
        End If
    End Sub

    Private Sub txt_Gridcell_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Gridcell.KeyDown
        If dtGrid.Rows.Count <= 0 Then Exit Sub
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Up Or e.KeyCode = Keys.Down Then
            e.Handled = True
            Dim crtcolumn As String = ""
            If dtGrid.Columns(dtGrid.CurrentCell.ColumnIndex).Name = "TARGETWEIGHT" Then
                crtcolumn = "TARGETWEIGHT"
            ElseIf dtGrid.Columns(dtGrid.CurrentCell.ColumnIndex).Name = "AMOUNT" And chkflatincentive.Checked Then
                crtcolumn = "AMOUNT"
            Else
                Exit Sub
            End If
            If Trim(txt_Gridcell.Text) <> String.Empty Then
                dtGrid.CurrentRow.Cells(crtcolumn).Value = txt_Gridcell.Text.ToString
                dtGrid.CurrentRow.Cells(crtcolumn).Style.Format = IIf(chkflatincentive.Checked, "0.00", "0.000")
                txt_Gridcell.TextAlign = HorizontalAlignment.Right
                txt_Gridcell.Focus()
                If Val(dtGrid.CurrentRow.Index) < Val(dtGrid.Rows.Count - 1) And (e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Down) Then
                    dtGrid.Item(crtcolumn, dtGrid.CurrentRow.Index + 1).Selected = True
                    dtGrid.CurrentCell = dtGrid.CurrentRow.Cells(crtcolumn)
                    dtGrid.BeginEdit(True)
                    txt_Gridcell.TextAlign = HorizontalAlignment.Right
                    txt_Gridcell.Text = dtGrid.CurrentRow.Cells(crtcolumn).Value.ToString
                    txt_Gridcell.Focus()
                    txt_Gridcell.SelectAll()
                ElseIf Val(dtGrid.CurrentRow.Index) > 0 And e.KeyCode = Keys.Up Then
                    dtGrid.Item(crtcolumn, dtGrid.CurrentRow.Index - 1).Selected = True
                    dtGrid.CurrentCell = dtGrid.CurrentRow.Cells(crtcolumn)
                    dtGrid.BeginEdit(True)
                    txt_Gridcell.TextAlign = HorizontalAlignment.Right
                    txt_Gridcell.Text = dtGrid.CurrentRow.Cells(crtcolumn).Value.ToString
                    txt_Gridcell.Focus()
                    txt_Gridcell.SelectAll()
                End If
                If dtGrid.CurrentRow.Index = dtGrid.Rows.Count - 1 Then btnSave.Focus()
            End If
        ElseIf e.KeyCode = Keys.Delete Then
            If chkflatincentive.Checked = False Then Exit Sub
            If dtGrid.Rows.Count <= 0 Or dtGrid.CurrentRow.Cells("COSTNAME").Value = "TOTAL" Then Exit Sub
            If e.KeyCode = Keys.Delete Then
                If MsgBox("Are you sure to Delete current row.", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Exit Sub
                strSql = " DELETE FROM " & cnAdminDb & "..FLATINCENTIVE WHERE AMOUNT= '" & Val(dtGrid.CurrentRow.Cells("AMOUNT").Value.ToString) & "' "
                strSql += vbCrLf + " AND COSTID='" & dtGrid.CurrentRow.Cells("COSTID").Value.ToString & "'"
                strSql += vbCrLf + " AND MONTH='" & cmbMonth.Text.ToString & "'"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
                MsgBox("Delete Successfully.")
            End If
        End If
    End Sub
    Private Sub txt_Gridcell_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Gridcell.Leave
        If (dtGrid.Rows.Count = 0 And Trim(txt_Gridcell.Text.ToString) = "") Then Exit Sub
        If (dtGrid.Rows.Count <= 0 Or dtGrid.CurrentRow.Index >= dtGrid.Rows.Count - 1) And Trim(txt_Gridcell.Text.ToString) = "" Then Exit Sub
        If chkflatincentive.Checked Then
            dtGrid.CurrentRow.Cells("AMOUNT").Value = Val(txt_Gridcell.Text.ToString)
            dtGrid.CurrentRow.Cells("AMOUNT").Style.Format = "0.00"
            Dim tgwt As Double = 0
            tgwt = 0
            For i As Integer = 0 To dtGrid.Rows.Count - 2
                With dtGrid.Rows(i)
                    If Val(.Cells("AMOUNT").Value.ToString) = 0 Or .Cells("COSTNAME").Value.ToString = "TOTAL" Then Continue For
                    tgwt += Val(.Cells("AMOUNT").Value.ToString)
                End With
            Next
            dtGrid.Rows(dtGrid.Rows.Count - 1).Cells("AMOUNT").Value = Format(tgwt, "0.00")
        Else
            dtGrid.CurrentRow.Cells("TARGETWEIGHT").Value = Val(txt_Gridcell.Text.ToString)
            dtGrid.CurrentRow.Cells("TARGETWEIGHT").Style.Format = "0.000"
            Dim tgwt As Double = 0
            tgwt = 0
            For i As Integer = 0 To dtGrid.Rows.Count - 2
                With dtGrid.Rows(i)
                    If Val(.Cells("TARGETWEIGHT").Value.ToString) = 0 Or .Cells("EMPID").Value.ToString = "TOTAL" Then Continue For
                    tgwt += Val(.Cells("TARGETWEIGHT").Value.ToString)
                End With
            Next
            dtGrid.Rows(dtGrid.Rows.Count - 1).Cells("TARGETWEIGHT").Value = Format(tgwt, "0.000")
        End If
        
    End Sub

    Private Sub btnImport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImport.Click
        Dim OpenDialog As New OpenFileDialog
        Dim Str As String
        excelflag = True
        Str = "(*.xls) | *.xls"
        Str += "|(*.xlsx) | *.xlsx"
        OpenDialog.Filter = Str
        If OpenDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim path As String = OpenDialog.FileName
            If path <> "" Then
                LoadFromExel(path)
            End If
        End If
    End Sub

    Private Sub btnTemplate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTemplate.Click
        exceltemplate()
    End Sub

    Private Sub chkflatincentive_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkflatincentive.CheckedChanged
        If chkflatincentive.Checked Then
            cmbcostcentre_OWN.Enabled = False
            cmbMetal_OWN.Enabled = False
            btnTemplate.Enabled = False
            btnImport.Enabled = False
        Else
            cmbcostcentre_OWN.Enabled = False
            cmbMetal_OWN.Enabled = False
            btnTemplate.Enabled = False
            btnImport.Enabled = False
        End If
    End Sub

#End Region

#End Region

#Region "BackendIncentive Coding"

#Region "Controls Events"

    Private Sub btnNewB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNewB.Click
        FuncNew()
    End Sub

    Private Sub BtnExitB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnExitB.Click
        Me.Close()
    End Sub

    Private Sub btnsaveB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnsaveB.Click
        FuncSave()
    End Sub

    Private Sub btnveiwB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnopenB.Click
        If validation() = False Then Exit Sub
        gridviewB.DataSource = Nothing
        txtGridB.Visible = False
        If cmbentrytypeB_OWN.Text.Trim = "INSERT" Then
            strSql = "SELECT ITEMNAME,ITEMID,'" & cmbMonthB_OWN.Text & "' MONTH,CONVERT(NUMERIC(15,2),0)AMOUNT FROM " & cnAdminDb & "..ITEMMAST "
            strSql += " WHERE ITEMID NOT IN (SELECT ITEMID FROM " & cnAdminDb & "..BACKENDINCENTIVE "
            strSql += " WHERE MONTH='" & cmbMonthB_OWN.Text & "')"
            strSql += " AND ISNULL(ACTIVE,'')='Y' "
            strSql += "ORDER BY ITEMNAME,AMOUNT "

        ElseIf cmbentrytypeB_OWN.Text.Trim = "UPDATE" Then
            strSql = "SELECT EM.ITEMNAME,EI.AMOUNT,EI.ITEMID FROM " & cnAdminDb & "..BACKENDINCENTIVE EI  "
            strSql += " INNER JOIN " & cnAdminDb & "..ITEMMAST EM on EI.ITEMID=EM.ITEMID WHERE 1=1"
            strSql += " AND ISNULL(ACTIVE,'')='Y' AND MONTH='" & cmbMonthB_OWN.Text & "'"
            strSql += " ORDER BY ITEMNAME,MONTH,AMOUNT"
        Else
            If Trim(cmbMonth_TarB_OWN.Text) = "" Then MsgBox("Target Month Should not empty..") : cmbMonth_TarB_OWN.Focus() : Exit Sub
            If Trim(cmbMonth_TarB_OWN.Text) = Trim(cmbMonthB_OWN.Text) Then MsgBox("Both Month are same." & vbCrLf & "We can't extending.") : cmbMonth_TarB_OWN.Focus() : Exit Sub

            strSql = " SELECT ITEMNAME,B.ITEMID,'" & cmbMonth_TarB_OWN.Text & "' MONTH,AMOUNT FROM " & cnAdminDb & "..BACKENDINCENTIVE B "
            strSql += " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS I ON I.ITEMID=B.ITEMID"
            strSql += " WHERE MONTH='" & cmbMonthB_OWN.Text & "'"
            strSql += " ORDER BY I.ITEMNAME,AMOUNT "
        End If
        dt = New DataTable
        dt = GetSqlTable(strSql, cn)
        If dt.Rows.Count > 0 Then
            Dim ro As DataRow
            ro = dt.NewRow()
            ro("ITEMNAME") = "TOTAL"
            ro("AMOUNT") = dt.Compute("SUM(AMOUNT)", "ITEMNAME<>'TOTAL'")
            dt.Rows.Add(ro)
            gridviewB.DataSource = Nothing
            gridviewB.DataSource = dt
            gridviewB.Columns("ITEMNAME").Width = 250
            gridviewB.Columns("ITEMID").Visible = False
            gridviewB.Columns("ITEMNAME").ReadOnly = True
            'gridviewB.Columns("MONTH").ReadOnly = True
            gridviewB.Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            gridviewB.Focus()
            gridviewB.CurrentCell = gridviewB.Item("AMOUNT", 0)
        Else
            gridviewB.DataSource = Nothing
            MsgBox("No record found..")
        End If
    End Sub

    Private Sub btnexportB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnexportB.Click
        If gridviewB.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "", gridviewB, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub txtGridB_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtGridB.KeyDown
        If gridviewB.Rows.Count <= 0 Then Exit Sub
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Up Or e.KeyCode = Keys.Down Then
            e.Handled = True
            If txtGridB.Text <> "" Or txtGridB.Text <> String.Empty Then
                With gridviewB
                    If .Columns(.CurrentCell.ColumnIndex).Name = "AMOUNT" Then
                        .CurrentRow.Cells("AMOUNT").Value = txtGridB.Text
                        .CurrentRow.Cells("AMOUNT").Style.Format = "0.00"
                        If Val(.CurrentRow.Index) < Val(.Rows.Count - 1) And (e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Down) Then
                            .Item(.CurrentCell.ColumnIndex, .CurrentRow.Index + 1).Selected = True
                            .CurrentCell = .Item("AMOUNT", .CurrentRow.Index)
                            .BeginEdit(True)
                            txtGridB.TextAlign = HorizontalAlignment.Right
                            txtGridB.Text = .CurrentRow.Cells("AMOUNT").Value.ToString
                            txtGridB.Focus()

                        ElseIf Val(.CurrentRow.Index) > 0 And e.KeyCode = Keys.Up Then
                            .Item("AMOUNT", .CurrentRow.Index - 1).Selected = True
                            .CurrentCell = .CurrentRow.Cells("AMOUNT")
                            .BeginEdit(True)
                            txtGridB.TextAlign = HorizontalAlignment.Right
                            txtGridB.Text = .CurrentRow.Cells("AMOUNT").Value.ToString
                            txtGridB.Focus()
                            txtGridB.SelectAll()
                        End If
                        If .CurrentRow.Index = .Rows.Count - 1 Then btnsaveB.Focus()
                    End If
                End With
            End If
        End If
    End Sub

    Private Sub gridviewB_CellEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridviewB.CellEnter
        If gridviewB.Rows.Count <= 0 Then Exit Sub
        With gridviewB
            If .Columns(.CurrentCell.ColumnIndex).Name = "AMOUNT" Then
                If .CurrentRow.Cells("ITEMNAME").Value = "TOTAL" Then Exit Sub
                If .CurrentCell.ReadOnly = True Then txtGridB.Text = "" : txtGridB.Visible = False : Exit Sub
                If .CurrentCell.Value Is Nothing Or IsDBNull(.CurrentCell.Value) Then : txtGridB.Text = String.Empty
                Else : txtGridB.Text = .CurrentCell.Value : End If
                Dim CurrentCellx As Integer = .GetCellDisplayRectangle(.CurrentCell.ColumnIndex, .CurrentRow.Index, False).Left + .Left
                Dim CurrentCelly As Integer = .GetCellDisplayRectangle(.CurrentCell.ColumnIndex, .CurrentRow.Index, False).Top + .Top
                txtGridB.Location = New Point(CurrentCellx, CurrentCelly)
                txtGridB.Size = New Size(.CurrentCell.Size.Width - 2, .CurrentCell.Size.Height)
                txtGridB.BorderStyle = BorderStyle.None
                txtGridB.Show()
                txtGridB.BringToFront()
                txtGridB.Select()
                txtGridB.TextAlign = HorizontalAlignment.Right
            End If
        End With
    End Sub

    Private Sub cmbentrytypeB_OWN_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbentrytypeB_OWN.SelectedIndexChanged
        If cmbentrytypeB_OWN.Text = "EXTENSION" Then
            cmbMonth_TarB_OWN.Enabled = True
        Else
            cmbMonth_TarB_OWN.Enabled = False
        End If
    End Sub

    Private Sub txtGridB_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtGridB.Leave
        With gridviewB
            If (.Rows.Count <= 0 Or .CurrentRow.Index >= .Rows.Count - 1) And Trim(txtGridB.Text) = "" Then Exit Sub
            .CurrentRow.Cells("AMOUNT").Value = Val(txtGridB.Text.ToString)
            .CurrentRow.Cells("AMOUNT").Style.Format = "0.000"
            Dim tgwt As Double = 0
            tgwt = 0
            For i As Integer = 0 To .Rows.Count - 2
                With .Rows(i)
                    If Val(.Cells("AMOUNT").Value.ToString) = 0 Or .Cells("ITEMNAME").Value.ToString = "TOTAL" Then Continue For
                    tgwt += (.Cells("AMOUNT").Value.ToString)
                End With
            Next
            .Rows(.Rows.Count - 1).Cells("AMOUNT").Value = Format(tgwt, "0.000")
        End With


        'If dtGrid.Rows.Count <= 0 And Trim(txtGridB.Text) = "" Then Exit Sub
        'If txtGridB.Text <> "" Or txtGridB.Text <> String.Empty Then
        '    With gridviewB
        '        If .Columns(.CurrentCell.ColumnIndex).Name = "AMOUNT" Then
        '            .CurrentRow.Cells("AMOUNT").Value = txtGridB.Text
        '            .CurrentRow.Cells("AMOUNT").Style.Format = "0.00"
        '            If .CurrentRow.Index < .Rows.Count - 1 Then
        '                '.Item(.CurrentCell.ColumnIndex, .CurrentRow.Index + 1).Selected = True
        '                .CurrentCell = .Item("AMOUNT", .CurrentRow.Index)
        '                '.BeginEdit(True)
        '                txtGridB.TextAlign = HorizontalAlignment.Right
        '                txtGridB.Text = .CurrentRow.Cells("AMOUNT").Value.ToString
        '                txtGridB.Focus()
        '            End If
        '        End If
        '    End With
        '    Dim gdt As New DataTable
        '    Dim AMT As Double = 0
        '    gdt = CType(gridviewB.DataSource, DataTable)
        '    AMT = gdt.Compute("SUM(AMOUNT)", "ITEMNAME<>'TOTAL'")
        '    gridviewB.Rows(gridviewB.Rows.Count - 1).Cells("AMOUNT").Value = Format(AMT, "0.00")
        'End If
        'dtGrid.CurrentRow.Cells("AMOUNT").Value = Val(txtGridB.Text)
    End Sub

#End Region

#End Region
   
    
    
End Class