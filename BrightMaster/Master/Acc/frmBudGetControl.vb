Imports System.Data.OleDb
Public Class frmBudGetControl
    Dim strsql As String
    Dim dt As New DataTable
    Dim da As New OleDbDataAdapter
    Dim cmd As New OleDbCommand
    Dim days As Integer = 0
    Dim dtCostCentre As New DataTable
    Dim upflag As Boolean = False
    Dim BUDGETID As Integer
    Dim costval As String = "N"

    Private Sub frmBudjetControl_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabmain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabmain.Region = New Region(New RectangleF(Me.TabGen.Left, Me.TabGen.Top, Me.TabGen.Width, Me.TabGen.Height))
        funcnew()
    End Sub

    Private Sub frmBudjetControl_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyCode = Keys.Escape Then
            tabmain.SelectedTab = TabGen
        End If
    End Sub

#Region "USER DEFINED FUNCTION"
    Private Sub fillcombowithvalmem(ByVal cmbbox As ComboBox, ByVal qry As String, ByVal valmem As String, ByVal dismem As String)
        da = New OleDbDataAdapter(strsql, cn)
        dt = New DataTable()
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            cmbbox.DataSource = Nothing
            cmbbox.DataSource = dt
            cmbbox.ValueMember = valmem
            cmbbox.DisplayMember = dismem
            cmbbox.SelectedIndex = 0
            cmbbox.Enabled = True
        Else
            ' cmbbox.DataSource = Nothing
        End If
    End Sub
    Private Sub funcnew()
        strsql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE'"
        costval = objGPack.GetSqlValue(strsql, , "N")
        If costval = "Y" Then
            strsql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
            strsql += " UNION ALL"
            strsql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
            strsql += " ORDER BY RESULT,COSTNAME"
            dtCostCentre = New DataTable
            da = New OleDbDataAdapter(strsql, cn)
            da.Fill(dtCostCentre)
            BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME")
            BrighttechPack.GlobalMethods.FillCombo(Me.cmbCostcentreV, dtCostCentre, "COSTNAME")
        Else
            chkCmbCostCentre.Items.Clear()
            chkCmbCostCentre.Enabled = False
            cmbCostcentreV.Items.Clear()
            cmbCostcentreV.Enabled = False
        End If
        cmbmaingrpid_OWN.DataSource = Nothing
        cmbacgroupid_OWN.DataSource = Nothing
        cmbAcName_OWN.DataSource = Nothing
        strsql = "SELECT NULL AS ACGRPNAME,'0' AS ACGRPCODE UNION ALL SELECT DISTINCT ACGRPNAME,ACGRPCODE FROM " & cnAdminDb & "..ACGROUP   ORDER BY ACGRPNAME"
        fillcombowithvalmem(cmbmaingrpid_OWN, strsql, "ACGRPCODE", "ACGRPNAME")
        strsql = "SELECT NULL AS ACGRPNAME,'0' AS ACGRPCODE UNION ALL SELECT ACGRPNAME,ACGRPCODE FROM " & cnAdminDb & "..ACGROUP ORDER BY ACGRPNAME"
        fillcombowithvalmem(cmbacgroupid_OWN, strsql, "ACGRPCODE", "ACGRPNAME")
        strsql = "SELECT NULL AS ACNAME,'0' AS ACCODE UNION ALL SELECT  ACNAME,ACCODE FROM " & cnAdminDb & "..ACHEAD ORDER BY ACNAME"
        fillcombowithvalmem(cmbAcName_OWN, strsql, "ACCODE", "ACNAME")
        dtpFrom_OWN.Value = ("04/01/" & DateTime.Now.Year.ToString() & "")
        dtpTo_OWN.Value = ("03/31/" & getCurrentfinancialyear(True) & "")
        dtpFrom_OWN.ReadOnly = True
        dtpTo_OWN.ReadOnly = True
        upflag = False
        BUDGETID = 0
        txtvalue_NUM.Text = ""
        cmbmaingrpid_OWN.Enabled = True
        cmbacgroupid_OWN.Enabled = True
        cmbAcName_OWN.Enabled = True
        Panel1.Enabled = True
        btnSave.Text = "Save[F1]"
        cmbtranmode.SelectedIndex = 0
        chkCmbCostCentre.Focus()
    End Sub
    Private Function Acfilter(Optional ByVal defau As String = "") As String
        Dim retval As String = ""
        If cmbmaingrpid_OWN.Text <> "ALL" And cmbmaingrpid_OWN.Text <> "" Then
            retval = vbCrLf + " AND ACMGROUPID ='" & cmbmaingrpid_OWN.SelectedValue.ToString() & "'"
        End If
        If cmbacgroupid_OWN.Text <> "ALL" And cmbacgroupid_OWN.Text <> "" Then
            retval += vbCrLf + " AND ACGROUPID ='" & cmbacgroupid_OWN.SelectedValue.ToString() & "'"
        End If
        If cmbAcName_OWN.Text <> "ALL" And cmbAcName_OWN.Text <> "" Then
            retval += vbCrLf + " AND ACCODE ='" & cmbAcName_OWN.SelectedValue.ToString() & "'"
        End If
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            Dim costid As String = ""
            strsql = "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & ")"
            da = New OleDbDataAdapter(strsql, cn)
            Dim COSTDT As New DataTable
            da.Fill(COSTDT)
            If COSTDT.Rows.Count > 0 Then
                For i As Integer = 0 To COSTDT.Rows.Count - 1
                    costid = costid + "," + COSTDT.Rows(i).Item("COSTID").ToString()
                Next
                If costid <> "" Then
                    costid = Mid(costid, 2, costid.Length)
                End If
            End If
            retval += vbCrLf + " AND COSTID IN('" & costid.Replace(",", "','") & "')"
        End If
        Return retval
    End Function
    Private Sub funsavebasedoncostid(Optional ByVal costid As String = "")
        Dim mvalidmode As String

        If rbtValidY.Checked Then mvalidmode = "Y"
        If rbtValidm.Checked Then mvalidmode = "M"
        If rbtValidD.Checked Then mvalidmode = "D"

        If rbtYrly.Checked = True Then
            strsql = "SELECT 1 FROM " & cnStockDb & "..BUDGETCONTROL WHERE BUDYEAR=" & DateTime.Today.Year & " and TRANMODE='" & IIf(cmbtranmode.Text = "Credit", "C", "D") & "' " & Acfilter() & ""
            If Not Val(GetSqlValue(cn, strsql)) > 0 Then
                Dim MBUDGETID As Integer = Val(GetSqlValue(cn, "SELECT ISNULL(MAX(BUDGETID),0) FROM " & cnStockDb & "..BUDGETCONTROL")) + 1
                strsql = "INSERT INTO " & cnStockDb & "..BUDGETCONTROL(BUDGETID,ACMGROUPID,ACGROUPID,ACCODE,BUDMONTH,BUDYEAR,BUDEFFFROM,BUDEFFTO,BUDVALUE,COSTID,COMPANYID,BUDMODE,BUDCALMODE,TRANMODE)"
                strsql += vbCrLf + " VALUES(" & MBUDGETID & ",'" & cmbmaingrpid_OWN.SelectedValue.ToString() & "'"
                strsql += vbCrLf + ",'" & cmbacgroupid_OWN.SelectedValue.ToString() & "'"
                strsql += vbCrLf + ",'" & cmbAcName_OWN.SelectedValue.ToString() & "'"
                strsql += vbCrLf + ",0"
                strsql += vbCrLf + "," & DateTime.Today.Year & ""
                strsql += vbCrLf + ",'" & dtpFrom_OWN.Value.ToString() & "'"
                strsql += vbCrLf + ",'" & dtpTo_OWN.Value.ToString() & "'"
                strsql += vbCrLf + "," & Val(txtvalue_NUM.Text.ToString()) & ""
                strsql += vbCrLf + ",'" & costid & "'"
                strsql += vbCrLf + ",'" & cnCompanyId & "'"
                strsql += vbCrLf + ",'Y'"
                strsql += vbCrLf + ",'" & mvalidmode & "'"
                strsql += vbCrLf + ",'" & IIf(cmbtranmode.Text = "Credit", "C", "D") & "')"
                cmd = New OleDbCommand(strsql, cn)
                cmd.ExecuteNonQuery()
            Else
                MessageBox.Show("Budget was already set for this year.")
                Exit Sub
            End If
        ElseIf rbtMonthly.Checked = True Then
            For j As Integer = 4 To 12
                strsql = "SELECT 1 FROM " & cnStockDb & "..BUDGETCONTROL WHERE BUDMONTH=" & j & " AND BUDYEAR='" & DateTime.Today.Year & "' and TRANMODE='" & IIf(cmbtranmode.Text = "Credit", "C", "D") & "' " & Acfilter() & ""
                If Not Val(GetSqlValue(cn, strsql)) > 0 Then
                    Dim MBUDGETID1 As Integer = Val(GetSqlValue(cn, "SELECT ISNULL(MAX(BUDGETID),0) FROM " & cnStockDb & "..BUDGETCONTROL")) + 1
                    days = System.DateTime.DaysInMonth(DateTime.Today.Year, j)
                    strsql = "INSERT INTO " & cnStockDb & "..BUDGETCONTROL(BUDGETID,ACMGROUPID,ACGROUPID,ACCODE,BUDMONTH,BUDYEAR,BUDEFFFROM,BUDEFFTO,BUDVALUE,COSTID,COMPANYID,BUDMODE,BUDCALMODE,TRANMODE)"
                    strsql += vbCrLf + " VALUES(" & MBUDGETID1 & ",'" & cmbmaingrpid_OWN.SelectedValue.ToString() & "'"
                    strsql += vbCrLf + ",'" & cmbacgroupid_OWN.SelectedValue.ToString() & "'"
                    strsql += vbCrLf + ",'" & cmbAcName_OWN.SelectedValue.ToString() & "'"
                    strsql += vbCrLf + ",'" & j & "'"
                    strsql += vbCrLf + "," & DateTime.Today.Year & ""
                    strsql += vbCrLf + ",'" & "01-" & MonthName(j, True).ToString() & "-" & getCurrentfinancialyear(False) & "'"
                    strsql += vbCrLf + ",'" & days & "-" & MonthName(j, True).ToString() & "-" & getCurrentfinancialyear(False) & "'"
                    strsql += vbCrLf + "," & Val(txtvalue_NUM.Text.ToString()) & ""
                    strsql += vbCrLf + ",'" & costid & "'"
                    strsql += vbCrLf + ",'" & cnCompanyId & "'"
                    strsql += vbCrLf + ",'M'"
                    strsql += vbCrLf + ",'" & mvalidmode & "'"
                    strsql += vbCrLf + ",'" & IIf(cmbtranmode.Text = "Credit", "C", "D") & "')"
                    cmd = New OleDbCommand(strsql, cn)
                    cmd.ExecuteNonQuery()
                Else
                    MessageBox.Show("Budget was already set for this year.")
                    Exit Sub
                End If
            Next
            For k As Integer = 1 To 3
                strsql = "SELECT 1 FROM " & cnStockDb & "..BUDGETCONTROL WHERE BUDMONTH=" & k & " AND TRANMODE='" & IIf(cmbtranmode.Text = "Credit", "C", "D") & "' AND BUDYEAR='" & DateTime.Today.Year & "'" & Acfilter() & ""
                If Not Val(GetSqlValue(cn, strsql)) > 0 Then
                    Dim MBUDGETID1 As Integer = Val(GetSqlValue(cn, "SELECT ISNULL(MAX(BUDGETID),0) FROM " & cnStockDb & "..BUDGETCONTROL")) + 1
                    days = System.DateTime.DaysInMonth(Val(getCurrentfinancialyear(True)), k)
                    strsql = "INSERT INTO " & cnStockDb & "..BUDGETCONTROL(BUDGETID,ACMGROUPID,ACGROUPID,ACCODE,BUDMONTH,BUDYEAR,BUDEFFFROM,BUDEFFTO,BUDVALUE,COSTID,COMPANYID,BUDMODE,BUDCALMODE,TRANMODE)"
                    strsql += vbCrLf + " VALUES(" & MBUDGETID1 & ",'" & cmbmaingrpid_OWN.SelectedValue.ToString() & "'"
                    strsql += vbCrLf + ",'" & cmbacgroupid_OWN.SelectedValue.ToString() & "'"
                    strsql += vbCrLf + ",'" & cmbAcName_OWN.SelectedValue.ToString() & "'"
                    strsql += vbCrLf + ",'" & k & "'"
                    strsql += vbCrLf + "," & DateTime.Today.Year & ""
                    strsql += vbCrLf + ",'" & "01-" & MonthName(k, True).ToString() & "-" & getCurrentfinancialyear(True) & "'"
                    strsql += vbCrLf + ",'" & days & "-" & MonthName(k, True).ToString() & "-" & getCurrentfinancialyear(True) & "'"
                    strsql += vbCrLf + "," & Val(txtvalue_NUM.Text.ToString()) & ""
                    strsql += vbCrLf + ",'" & costid & "'"
                    strsql += vbCrLf + ",'" & cnCompanyId & "'"
                    strsql += vbCrLf + ",'M'"
                    strsql += vbCrLf + ",'" & mvalidmode & "'"
                    strsql += vbCrLf + ",'" & IIf(cmbtranmode.Text = "Credit", "C", "D") & "')"
                    cmd = New OleDbCommand(strsql, cn)
                    cmd.ExecuteNonQuery()
                Else
                    Exit Sub
                End If
            Next
        End If
        MessageBox.Show("Saved Successfully.", "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
    End Sub
    Private Sub Funsave()
        If txtvalue_NUM.Text = "" And Val(txtvalue_NUM.Text) = 0 Then
            MessageBox.Show("Budget value should not be empty.")
            txtvalue_NUM.Focus()
            Exit Sub
        End If
        If cmbacgroupid_OWN.Text = "" And cmbmaingrpid_OWN.Text = "" And cmbAcName_OWN.Text = "" Then
            MessageBox.Show("Any one Group or account belong to the budget")
            cmbmaingrpid_OWN.Focus()
            Exit Sub
        End If
        If chkCmbCostCentre.Text <> "ALL" And costval = "Y" Then
            strsql = "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & ")"
            da = New OleDbDataAdapter(strsql, cn)
            Dim COSTDT As New DataTable
            da.Fill(COSTDT)
            If COSTDT.Rows.Count > 0 Then
                For i As Integer = 0 To COSTDT.Rows.Count - 1
                    funsavebasedoncostid(COSTDT.Rows(i).Item("COSTID").ToString())
                Next
            End If

        Else
            funsavebasedoncostid()
        End If

        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub Funupdate(ByVal BUDID As Integer)
        If BUDID <> 0 Then
            strsql = "UPDATE " & cnStockDb & "..BUDGETCONTROL SET BUDVALUE=" & Val(txtvalue_NUM.Text.ToString()) & ""
            strsql += ", TRANMODE='" & IIf(cmbtranmode.Text = "Credit", "C", "D") & "' "
            strsql += " , ACCODE='" & cmbAcName_OWN.SelectedValue.ToString() & "'"
            strsql += " , ACMGROUPID='" & cmbmaingrpid_OWN.SelectedValue.ToString() & "'"
            strsql += " , ACGROUPID= '" & cmbacgroupid_OWN.SelectedValue.ToString() & "'"
            strsql += " WHERE BUDGETID=" & BUDID & ""
            'strsql += " AND ACCODE='" & cmbAcName_OWN.SelectedValue.ToString() & "'"
            'strsql += " AND ACMGROUPID='" & cmbmaingrpid_OWN.SelectedValue.ToString() & "'"
            'strsql += " AND ACGROUPID= '" & cmbacgroupid_OWN.SelectedValue.ToString() & "'"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
            MessageBox.Show("Record updated Successfully.")
            funcnew()
        End If
    End Sub
    Private Sub GetDetail()
        strsql = "IF EXISTS(SELECT * FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE='U' AND NAME='TEMPBUDGET') DROP TABLE " & cnAdminDb & "..TEMPBUDGET"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()
        strsql = vbCrLf + "SELECT DISTINCT B.BUDGETID,G.ACGRPNAME AS ACGRPNAME,GG.ACGRPNAME AS ACMGRPNAME"
        strsql += vbCrLf + ",A.ACNAME,B.ACCODE,B.ACGROUPID,B.ACMGROUPID,B.BUDMONTH,B.BUDYEAR,CONVERT(VARCHAR(13)"
        strsql += vbCrLf + ",B.BUDEFFFROM,103)BUDEFFFROM,CONVERT(VARCHAR(13),B.BUDEFFTO,103)BUDEFFTO ,B.COSTID,B.BUDMODE,B.BUDVALUE,C.COSTNAME"
        strsql += vbCrLf + " INTO " & cnAdminDb & "..TEMPBUDGET "
        strsql += vbCrLf + " FROM " & cnStockDb & "..BUDGETCONTROL AS B "
        strsql += vbCrLf + "LEFT JOIN " & cnAdminDb & "..ACHEAD AS A ON B.ACCODE=A.ACCODE"
        strsql += vbCrLf + "LEFT JOIN " & cnAdminDb & "..ACGROUP AS G ON B.ACGROUPID=G.ACGRPCODE "
        strsql += vbCrLf + "LEFT JOIN " & cnAdminDb & "..ACGROUP AS GG ON B.ACMGROUPID=GG.ACGRPCODE "
        strsql += vbCrLf + "LEFT JOIN " & cnAdminDb & "..COSTCENTRE AS C ON C.COSTID=B.COSTID WHERE 1=1 "
        If cmbmainac_OWN.Text <> "ALL" And cmbmainac_OWN.Text <> "" Then
            strsql += vbCrLf + " AND B.ACMGROUPID ='" & cmbmainac_OWN.SelectedValue.ToString() & "'"
        End If
        If cmbgrpac_OWN.Text <> "ALL" And cmbgrpac_OWN.Text <> "" Then
            strsql += vbCrLf + " AND B.ACGROUPID ='" & cmbgrpac_OWN.SelectedValue.ToString() & "'"
        End If
        If cmbacnam_OWN.Text <> "ALL" And cmbacnam_OWN.Text <> "" Then
            strsql += vbCrLf + " AND B.ACCODE ='" & cmbacnam_OWN.SelectedValue.ToString() & "'"
        End If
        Dim ChkCostids As String = GetSelectedCostId(cmbCostcentreV, True)
        If ChkCostids <> "" Then
            If ChkCostids <> "''" Then strsql += vbCrLf + " AND B.COSTID IN (" & ChkCostids & ")"
        End If
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        Dim totalbudvalue As Decimal
        totalbudvalue = Val(objGPack.GetSqlValue("SELECT SUM(BUDVALUE) FROM  " & cnAdminDb & "..TEMPBUDGET ", , ""))

        strsql = vbCrLf + "SELECT * FROM " & cnAdminDb & "..TEMPBUDGET"
        da = New OleDbDataAdapter(strsql, cn)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Dim ro As DataRow = Nothing
            ro = dt.NewRow
            ro("ACNAME") = "TOTAL VALUE"
            ro("BUDVALUE") = totalbudvalue
            dt.Rows.Add(ro)
            GridView.DataSource = Nothing
            GridView.DataSource = dt
            GridView.Columns("COSTID").Visible = False
            GridView.Columns("BUDMODE").Visible = False
            GridView.Columns("BUDMONTH").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            GridView.Columns("BUDYEAR").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            GridView.Columns("BUDGETID").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            GridView.Columns("BUDGETID").Width = 40
            GridView.Columns("BUDMONTH").Width = 60
            GridView.Columns("ACNAME").Width = 250
            GridView.Columns("ACGROUPID").Visible = False
            GridView.Columns("ACCODE").Visible = False
            GridView.Columns("ACMGROUPID").Visible = False
            GridView.Columns("ACGRPNAME").Visible = False
            GridView.Columns("ACMGRPNAME").Visible = False
            GridView.Columns("BUDGETID").HeaderText = "ID"
            GridView.Columns("ACNAME").HeaderText = "NAME"
            GridView.Columns("BUDMONTH").HeaderText = "MONTH"
            GridView.Columns("BUDYEAR").HeaderText = "YEAR"
            GridView.Columns("BUDEFFFROM").HeaderText = "FROM"
            GridView.Columns("BUDEFFTO").HeaderText = "TO"
            GridView.Columns("BUDVALUE").HeaderText = "VALUE"
            For i As Integer = 0 To GridView.Columns.Count - 2
                GridView.Columns(i).ReadOnly = True
            Next
            Dim rowscount As Integer = GridView.RowCount
            GridView.Rows(rowscount - 1).DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
            GridView.Rows(rowscount - 1).DefaultCellStyle.Font = New Font("", 8, FontStyle.Bold)
            GridView.Focus()
        Else
            GridView.DataSource = Nothing
            MessageBox.Show("Record not found.")
        End If
    End Sub
    Private Function getCurrentfinancialyear(ByVal yer As Boolean) As String
        Dim CurrentYear As Integer = DateTime.Today.Year
        Dim PreviousYear As Integer = DateTime.Today.Year - 1
        Dim NextYear As Integer = DateTime.Today.Year + 1
        Dim PreYear As String = PreviousYear.ToString()
        Dim NexYear As String = NextYear.ToString()
        Dim CurYear As String = CurrentYear.ToString()
        Dim FinYear As String = Nothing
        If yer = True Then
            FinYear = NexYear
        Else
            FinYear = CurYear
        End If
        Return FinYear.Trim()
    End Function
#End Region

#Region "KEYS AND BUTTONS EVENTS"
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        tabmain.SelectedTab = TabGen
        TabGen.Show()
        funcnew()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        btnSave.Enabled = False
        If upflag = False Then
            Funsave()
        Else
            Funupdate(BUDGETID)
        End If
        btnSave.Enabled = True
    End Sub
    Private Sub btnOpen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        dt = New DataTable
        tabmain.SelectedTab = TabView
        TabView.Show()
        cmbmainac_OWN.DataSource = Nothing
        cmbgrpac_OWN.DataSource = Nothing
        cmbacnam_OWN.DataSource = Nothing
        strsql = "SELECT NULL AS ACGRPNAME,'0' AS ACGRPCODE UNION ALL SELECT DISTINCT ACGRPNAME,ACGRPCODE FROM " & cnAdminDb & "..ACGROUP where ACGRPCODE=ACMAINCODE ORDER BY ACGRPNAME"
        fillcombowithvalmem(cmbmainac_OWN, strsql, "ACGRPCODE", "ACGRPNAME")
        strsql = "SELECT NULL AS ACGRPNAME,'0' AS ACGRPCODE UNION ALL SELECT ACGRPNAME,ACGRPCODE FROM " & cnAdminDb & "..ACGROUP ORDER BY ACGRPNAME"
        fillcombowithvalmem(cmbgrpac_OWN, strsql, "ACGRPCODE", "ACGRPNAME")
        strsql = "SELECT NULL AS ACNAME,'0' AS ACCODE UNION ALL SELECT  ACNAME,ACCODE FROM " & cnAdminDb & "..ACHEAD ORDER BY ACNAME"
        fillcombowithvalmem(cmbacnam_OWN, strsql, "ACCODE", "ACNAME")
        cmbmainac_OWN.Select()
        cmbmainac_OWN.Focus()
        GridView.DataSource = Nothing
        lblacmain.Text = ""
        Label16.Text = ""
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Private Sub ExitToolStripMenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem3.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub SaveToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem1.Click
        btnSave_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem2.Click
        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub OpenToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem1.Click
        btnOpen_Click(Me, New EventArgs)
    End Sub

    Private Sub GridView_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles GridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            If GridView.RowCount > 0 Then
                cmbmaingrpid_OWN.Text = GridView.CurrentRow.Cells("ACMGRPNAME").Value.ToString()
                cmbacgroupid_OWN.Text = GridView.CurrentRow.Cells("ACGRPNAME").Value.ToString()
                cmbAcName_OWN.Text = GridView.CurrentRow.Cells("ACNAME").Value.ToString()
                If GridView.CurrentRow.Cells("BUDMODE").Value.ToString() = "Y" Then
                    rbtYrly.Checked = True
                ElseIf GridView.CurrentRow.Cells("BUDMODE").Value.ToString() = "M" Then
                    rbtMonthly.Checked = True
                End If
                'Dim DAT1 As String = GridView.CurrentRow.Cells("BUDEFFFROM").Value.ToString()
                '  Dim dattimepic1 As DateTime = DateTime.Parse(DAT1)
                '  dtpFrom_OWN.Value = dattimepic1.ToString("MM/dd/yyyy")
                '  Dim DAT As String = GridView.CurrentRow.Cells("BUDEFFTO").Value.ToString()
                '  Dim dattimepic As DateTime = DateTime.Parse(DAT.ToString())
                '  dtpTo_OWN.Value = dattimepic.ToString("MM/dd/yyyy")
                txtvalue_NUM.Text = GridView.CurrentRow.Cells("BUDVALUE").Value.ToString()
                BUDGETID = Val(GridView.CurrentRow.Cells("BUDGETID").Value.ToString())
                btnSave.Text = "Update[F1]"
                upflag = True
                'Newly Change
                'cmbmaingrpid_OWN.Enabled = False
                'cmbacgroupid_OWN.Enabled = False
                'cmbAcName_OWN.Enabled = False
                'Panel1.Enabled = False
                'dtpFrom_OWN.Enabled = False
                'dtpTo_OWN.Enabled = False
                cmbmaingrpid_OWN.Enabled = True
                cmbacgroupid_OWN.Enabled = True
                cmbAcName_OWN.Enabled = True
                Panel1.Enabled = True
                dtpFrom_OWN.Enabled = True
                dtpTo_OWN.Enabled = True

                tabmain.SelectedTab = TabGen
                TabGen.Show()
            End If
        End If
    End Sub

    Private Sub GridView_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GridView.SelectionChanged
        lblacmain.Text = GridView.CurrentRow.Cells("ACMGRPNAME").Value.ToString()
        Label16.Text = GridView.CurrentRow.Cells("ACGRPNAME").Value.ToString()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        GetDetail()
    End Sub

    Private Sub cmbmaingrpid_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbmaingrpid_OWN.SelectedIndexChanged
        If cmbmaingrpid_OWN.SelectedIndex <> 0 And cmbmaingrpid_OWN.SelectedIndex <> -1 Then
            strsql = "SELECT NULL AS ACGRPNAME,'0' AS ACGRPCODE UNION ALL SELECT ACGRPNAME,ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE='" & cmbmaingrpid_OWN.SelectedValue.ToString() & "'ORDER BY ACGRPNAME"
            fillcombowithvalmem(cmbacgroupid_OWN, strsql, "ACGRPCODE", "ACGRPNAME")

            strsql = "SELECT NULL AS ACNAME,'0' AS ACCODE UNION ALL SELECT ACNAME,ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACGRPCODE=" & cmbmaingrpid_OWN.SelectedValue.ToString() & " ORDER BY ACNAME  "
            fillcombowithvalmem(cmbAcName_OWN, strsql, "ACCODE", "ACNAME")
        End If
    End Sub

    Private Sub cmbacgroupid_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbacgroupid_OWN.SelectedIndexChanged
        If cmbacgroupid_OWN.SelectedIndex <> 0 And cmbacgroupid_OWN.SelectedIndex <> -1 Then
            strsql = "SELECT NULL AS ACNAME,'0' AS ACCODE UNION ALL SELECT ACNAME,ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACGRPCODE=" & cmbacgroupid_OWN.SelectedValue.ToString() & " ORDER BY ACNAME  "
            fillcombowithvalmem(cmbAcName_OWN, strsql, "ACCODE", "ACNAME")
        End If
    End Sub
    Private Sub cmbmainac_OWN_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbmainac_OWN.SelectedIndexChanged
        If cmbmainac_OWN.SelectedIndex <> 0 And cmbmainac_OWN.SelectedIndex <> -1 Then
            strsql = "SELECT NULL AS ACGRPNAME,'0' AS ACGRPCODE UNION ALL SELECT ACGRPNAME,ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACMAINCODE='" & cmbmainac_OWN.SelectedValue.ToString() & "' ORDER BY ACGRPNAME"
            fillcombowithvalmem(cmbgrpac_OWN, strsql, "ACGRPCODE", "ACGRPNAME")

            strsql = "SELECT NULL AS ACNAME,'0' AS ACCODE UNION ALL SELECT ACNAME,ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACGRPCODE=" & cmbmainac_OWN.SelectedValue.ToString() & " ORDER BY ACNAME   "
            fillcombowithvalmem(cmbacnam_OWN, strsql, "ACCODE", "ACNAME")
        End If
    End Sub

    Private Sub cmbgrpac_OWN_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbgrpac_OWN.SelectedIndexChanged
        If cmbgrpac_OWN.SelectedIndex <> 0 And cmbgrpac_OWN.SelectedIndex <> -1 Then
            strsql = "SELECT NULL AS ACNAME,'0' AS ACCODE UNION ALL SELECT ACNAME,ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACGRPCODE=" & cmbgrpac_OWN.SelectedValue.ToString() & " ORDER BY ACNAME   "
            fillcombowithvalmem(cmbacnam_OWN, strsql, "ACCODE", "ACNAME")
        End If
    End Sub

    Private Sub btndelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btndelete.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        If GridView.RowCount > 0 Then
            If MessageBox.Show("Are you sure to delete?", "Brighttech", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly) = Windows.Forms.DialogResult.Yes Then
                strsql = "DELETE FROM " & cnStockDb & "..BUDGETCONTROL WHERE 1=1"
                strsql += " AND ACCODE='" & GridView.CurrentRow.Cells("ACCODE").Value.ToString() & "'"
                strsql += " AND ACMGROUPID='" & GridView.CurrentRow.Cells("ACMGROUPID").Value.ToString() & "'"
                strsql += " AND ACGROUPID= '" & GridView.CurrentRow.Cells("ACGROUPID").Value.ToString() & "'"
                strsql += " AND BUDMODE= '" & GridView.CurrentRow.Cells("BUDMODE").Value.ToString() & "'"
                strsql += " AND COSTID= '" & GridView.CurrentRow.Cells("COSTID").Value.ToString() & "'"
                cmd = New OleDbCommand(strsql, cn)
                cmd.ExecuteNonQuery()
                MessageBox.Show("Record deleted Successfully.")
            End If
        End If
        btnSearch_Click(Me, New EventArgs)
    End Sub
#End Region

    Private Sub cmbacnam_OWN_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbacnam_OWN.SelectedIndexChanged

    End Sub
    Private Sub Label10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label10.Click

    End Sub
    Private Sub Label12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label12.Click

    End Sub
    Private Sub Label11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label11.Click

    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If GridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "BUDGET CONTROL", GridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub
End Class