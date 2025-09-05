Imports System.Data.OleDb
Public Class AgeWiseOutstanding
    Dim objGridShower As frmGridDispDia
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand

    Private Sub AgeWiseOutstanding_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub AgeWiseOutstanding_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GrpContainer.Location = New Point((ScreenWid - GrpContainer.Width) / 2, ((ScreenHit - 128) - GrpContainer.Height) / 2)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpAsONDate.Value = GetServerDate()
        'rbtCustomer.Checked = True
        'rbtName.Checked = True
        Prop_Gets()
        ''chkOnlyGivenRange.Checked = False
        dtpAsONDate.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim Sysid As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
        strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP" & Sysid & "_DUE_AGEANALYSIS_DAYS') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & Sysid & "_DUE_AGEANALYSIS_DAYS"
        strSql += vbCrLf + " CREATE TABLE TEMPTABLEDB..TEMP" & Sysid & "_DUE_AGEANALYSIS_DAYS"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " FROMDAYS INT"
        strSql += vbCrLf + " ,TODAYS INT"
        strSql += vbCrLf + " )"
        If Val(txtFromDay1.Text) <> 0 And Val(txtToDay1.Text) <> 0 Then
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & Sysid & "_DUE_AGEANALYSIS_DAYS(FROMDAYS,TODAYS)VALUES(" & Val(txtFromDay1.Text) & "," & Val(txtToDay1.Text) & ")"
        End If
        If Val(txtFromDay2.Text) <> 0 And Val(txtToDay2.Text) <> 0 Then
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & Sysid & "_DUE_AGEANALYSIS_DAYS(FROMDAYS,TODAYS)VALUES(" & Val(txtFromDay2.Text) & "," & Val(txtToDay2.Text) & ")"
        End If
        If Val(txtFromDay3.Text) <> 0 And Val(txtToDay3.Text) <> 0 Then
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & Sysid & "_DUE_AGEANALYSIS_DAYS(FROMDAYS,TODAYS)VALUES(" & Val(txtFromDay3.Text) & "," & Val(txtToDay3.Text) & ")"
        End If
        If Val(txtFromDay4.Text) <> 0 And Val(txtToDay4.Text) <> 0 Then
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB.." & Sysid & "TEMP_DUE_AGEANALYSIS_DAYS(FROMDAYS,TODAYS)VALUES(" & Val(txtFromDay4.Text) & "," & Val(txtToDay4.Text) & ")"
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = vbCrLf + " EXEC " & cnStockDb & "..SP_RPT_AGEWISE_OUTSTANDING"
        strSql += vbCrLf + " @ASONDATE = '" & dtpAsONDate.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@ORDERBY = '" & IIf(rbtName.Checked, "N", "R") & "'"
        strSql += vbCrLf + " ,@ACGRPCODE = '" & IIf(rbtCustomer.Checked, "C", "D") & "'"
        strSql += vbCrLf + " ,@ONLYGIVENRANGE = '" & IIf(chkOnlyGivenRange.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@ADMINDB = '" & cnAdminDb & "'"
        strSql += vbCrLf + " ,@SYSTEMID = '" & Sysid & "'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 2000
        da = New OleDbDataAdapter(cmd)
        Dim ds As DataSet
        ds = New DataSet
        da.Fill(ds)
        If Not ds.Tables(1).Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        Dim dtGrid As New DataTable
        dtGrid = ds.Tables(1).Clone
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        Dim filterstr As String = ""
        Dim sortstr As String = ""
        If chkWithSubTotal.Checked = False Then
            filterstr = "RESULT <> 0 AND  RESULT <> 2"
            sortstr = "RESULT,TTRANDATE,TRANNO"
        End If
        For Each drr As DataRow In ds.Tables(1).Select(filterstr, sortstr)
            If chkWithSubTotal.Checked = False Then
                If Not Val(drr("RESULT").ToString) = 0 And Not Val(drr("RESULT").ToString) = 2 Then
                    dtGrid.ImportRow(drr)
                End If
            Else
                dtGrid.ImportRow(drr)
            End If
        Next
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "AGE WISE OUTSTANDING"
        Dim tit As String = "AGE WISE OUTSTANDING" + vbCrLf
        tit += " AS ON " & dtpAsONDate.Text & ""
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.WindowState = FormWindowState.Maximized
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        DataGridView_SummaryFormatting(objGridShower.gridView)
        objGridShower.Show()

        If objGridShower.gridView.RowCount > 0 Then
            objGridShower.gridView.Columns("PSNO").Visible = False
            If chkWithSubTotal.Checked = False Then
                objGridShower.gridView.Columns("PNAME").Visible = True
                objGridShower.gridView.Columns("PNAME").HeaderText = "NAME"
                objGridShower.gridView.Columns("RUNNO").Visible = True
                objGridShower.gridView.Columns("RUNNO").HeaderText = "REFNO"
                objGridShower.gridView.Columns("PARTICULAR").Visible = False
                If objGridShower.gridView.Columns.Contains("TTRANDATE") Then objGridShower.gridView.Columns("TTRANDATE").Visible = False
            Else
                objGridShower.gridView.Columns("PNAME").Visible = False
                objGridShower.gridView.Columns("RUNNO").Visible = False
            End If

            objGridShower.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            objGridShower.gridView.Invalidate()
            For Each dgvCol As DataGridViewColumn In objGridShower.gridView.Columns
                dgvCol.Width = dgvCol.Width
            Next
            objGridShower.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End If


        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("PARTICULAR")))
        Prop_Sets()
    End Sub
    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            .Columns("KEYNO").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("COLHEAD").Visible = False
            FormatGridColumns(dgv, False, False, , False)
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub
    Private Sub txtDif_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDif.TextChanged
        GenRangeVAlues()
    End Sub
    Private Sub GenRangeVAlues()
        txtFromDay1.Text = 0
        txtToDay1.Text = Val(txtFromDay1.Text) + Val(txtDif.Text)
        txtFromDay1.Text = 1

        txtFromDay2.Text = Val(txtToDay1.Text) + 1
        txtToDay2.Text = Val(txtFromDay2.Text) + Val(txtDif.Text) - 1

        txtFromDay3.Text = Val(txtToDay2.Text) + 1
        txtToDay3.Text = Val(txtFromDay3.Text) + Val(txtDif.Text) - 1

        txtFromDay4.Text = Val(txtToDay3.Text) + 1
        txtToDay4.Text = Val(txtFromDay4.Text) + Val(txtDif.Text) - 1
    End Sub


    Private Sub Prop_Sets()
        Dim obj As New AgeWiseOutstanding_Properties
        obj.p_rbtCustomer = rbtCustomer.Checked
        obj.p_rbtDealerSmith = rbtDealerSmith.Checked
        obj.p_txtDif = txtDif.Text
        obj.p_txtFromDay1 = txtFromDay1.Text
        obj.p_txtToDay1 = txtToDay1.Text
        obj.p_txtFromDay2 = txtFromDay2.Text
        obj.p_txtToDay2 = txtToDay2.Text
        obj.p_txtFromDay3 = txtFromDay3.Text
        obj.p_txtToDay3 = txtToDay3.Text
        obj.p_txtFromDay4 = txtFromDay4.Text
        obj.p_txtToDay4 = txtToDay4.Text
        obj.p_rbtName = rbtName.Checked
        obj.p_rbtRunno = rbtRunno.Checked
        obj.p_chkwithSubtotal = chkWithSubTotal.Checked
        obj.p_chkOnlyGivenRange = chkOnlyGivenRange.Checked
        SetSettingsObj(obj, Me.Name, GetType(AgeWiseOutstanding_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New AgeWiseOutstanding_Properties
        GetSettingsObj(obj, Me.Name, GetType(AgeWiseOutstanding_Properties))
        rbtCustomer.Checked = obj.p_rbtCustomer
        rbtDealerSmith.Checked = obj.p_rbtDealerSmith
        txtDif.Text = obj.p_txtDif
        txtFromDay1.Text = obj.p_txtFromDay1
        txtToDay1.Text = obj.p_txtToDay1
        txtFromDay2.Text = obj.p_txtFromDay2
        txtToDay2.Text = obj.p_txtToDay2
        txtFromDay3.Text = obj.p_txtFromDay3
        txtToDay3.Text = obj.p_txtToDay3
        txtFromDay4.Text = obj.p_txtFromDay4
        txtToDay4.Text = obj.p_txtToDay4
        rbtName.Checked = obj.p_rbtName
        rbtRunno.Checked = obj.p_rbtRunno
        chkWithSubTotal.Checked = obj.p_chkwithSubtotal
        chkOnlyGivenRange.Checked = obj.p_chkOnlyGivenRange
    End Sub
End Class

Public Class AgeWiseOutstanding_Properties
    Private rbtCustomer As Boolean = True
    Public Property p_rbtCustomer() As Boolean
        Get
            Return rbtCustomer
        End Get
        Set(ByVal value As Boolean)
            rbtCustomer = value
        End Set
    End Property
    Private rbtDealerSmith As Boolean = True
    Public Property p_rbtDealerSmith() As Boolean
        Get
            Return rbtDealerSmith
        End Get
        Set(ByVal value As Boolean)
            rbtDealerSmith = value
        End Set
    End Property
    Private txtDif As String = ""
    Public Property p_txtDif() As String
        Get
            Return txtDif
        End Get
        Set(ByVal value As String)
            txtDif = value
        End Set
    End Property
    Private txtFromDay1 As String = ""
    Public Property p_txtFromDay1() As String
        Get
            Return txtFromDay1
        End Get
        Set(ByVal value As String)
            txtFromDay1 = value
        End Set
    End Property
    Private txtToDay1 As String = ""
    Public Property p_txtToDay1() As String
        Get
            Return txtToDay1
        End Get
        Set(ByVal value As String)
            txtToDay1 = value
        End Set
    End Property

    Private txtFromDay2 As String = ""
    Public Property p_txtFromDay2() As String
        Get
            Return txtFromDay2
        End Get
        Set(ByVal value As String)
            txtFromDay2 = value
        End Set
    End Property
    Private txtToDay2 As String = ""
    Public Property p_txtToDay2() As String
        Get
            Return txtToDay2
        End Get
        Set(ByVal value As String)
            txtToDay2 = value
        End Set
    End Property
    Private txtFromDay3 As String = ""
    Public Property p_txtFromDay3() As String
        Get
            Return txtFromDay3
        End Get
        Set(ByVal value As String)
            txtFromDay3 = value
        End Set
    End Property
    Private txtToDay3 As String = ""
    Public Property p_txtToDay3() As String
        Get
            Return txtToDay3
        End Get
        Set(ByVal value As String)
            txtToDay3 = value
        End Set
    End Property
    Private txtFromDay4 As String = ""
    Public Property p_txtFromDay4() As String
        Get
            Return txtFromDay4
        End Get
        Set(ByVal value As String)
            txtFromDay4 = value
        End Set
    End Property
    Private txtToDay4 As String = ""
    Public Property p_txtToDay4() As String
        Get
            Return txtToDay4
        End Get
        Set(ByVal value As String)
            txtToDay4 = value
        End Set
    End Property
    Private rbtName As Boolean = True
    Public Property p_rbtName() As Boolean
        Get
            Return rbtName
        End Get
        Set(ByVal value As Boolean)
            rbtName = value
        End Set
    End Property
    Private rbtRunno As Boolean = True
    Public Property p_rbtRunno() As Boolean
        Get
            Return rbtRunno
        End Get
        Set(ByVal value As Boolean)
            rbtRunno = value
        End Set
    End Property

    Private chkwithSubtotal As Boolean = True
    Public Property p_chkwithSubtotal() As Boolean
        Get
            Return chkwithSubtotal
        End Get
        Set(ByVal value As Boolean)
            chkwithSubtotal = value
        End Set
    End Property
    Private chkOnlyGivenRange As Boolean = True
    Public Property p_chkOnlyGivenRange() As Boolean
        Get
            Return chkOnlyGivenRange
        End Get
        Set(ByVal value As Boolean)
            chkOnlyGivenRange = value
        End Set
    End Property
End Class