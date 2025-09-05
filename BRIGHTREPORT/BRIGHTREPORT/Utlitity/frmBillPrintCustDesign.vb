Imports System.Data.OleDb
Public Class frmBillPrintCustDesign

#Region " Variable"
    Dim strsql As String = ""
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim dt As New DataTable
    Dim dtGrid As New DataTable
    Dim flag As Boolean = False
    Dim c1 As Integer = 25  ' SNo
    Dim c2 As Integer = 70  ' Description
    Dim C3 As Integer = 250  ' Hsn
    Dim c4 As Integer = 350 ' Qty 
    Dim c5 As Integer = 430 ' Grs. Wt 
    Dim c6 As Integer = 510 ' Less. Wt
    Dim c7 As Integer = 560 ' VA
    Dim c8 As Integer = 630 ' Rate
    Dim c9 As Integer = 690 ' MC
    Dim c10 As Integer = 770 ' ' Amount
    Dim c11 As Integer = 800
    'Dim PagecountSale As Integer = 0
    Dim TopY As Integer = 135 ' TOP STARTING POSITION
    Dim bottomY As Integer = 960 'Ending POSITION
    Dim linespce As Integer = 18
#End Region

#Region " Constructor"
    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub
#End Region

#Region "Button Events"
    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        dtpfromdate.Value = GetServerDate()
        txtNo.Clear()

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnView.Click
        Dim dr As DataRow = Nothing
        If tabMain.SelectedTab.Name = "tabTaxInvoice" Then
            dr = Nothing
            strsql = " SELECT BATCHNO,TRANDATE FROM " & cnStockDb & "..ISSUE "
            strsql += vbCrLf + " WHERE TRANDATE = '" & Format(dtpfromdate.Value.Date, "yyyy-MM-dd") & "'  "
            strsql += vbCrLf + " AND TRANNO = '" & Val(txtNo.Text) & "' "
            strsql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
            strsql += vbCrLf + " AND ISNULL(CANCEL,'')='' "
            strsql += vbCrLf + " AND TRANTYPE IN('SA')"
            dr = GetSqlRow(strsql, cn)
            If Not dr Is Nothing Then
                If rbtTaxInvoiceM1.Checked = True Then
                    Dim obj As New frmBillPrintDocA4N("POS", dr.Item("BATCHNO").ToString, Format(dr.Item("TRANDATE"), "yyyy-MM-dd"), "Y")
                ElseIf rbtTaxInvoiceM2.Checked = True Then
                    Dim obj As New frmBillPrintDocB5("POS", dr.Item("BATCHNO").ToString, Format(dr.Item("TRANDATE"), "yyyy-MM-dd"), "Y")
                ElseIf rbtTaxInvoiceM3.Checked = True Then
                    Dim obj As New frmBillPrintDocA5("POS", dr.Item("BATCHNO").ToString, Format(dr.Item("TRANDATE"), "yyyy-MM-dd"), "Y")
                ElseIf rbtTaxInvoiceM4.Checked = True Then
                    Dim obj As New frmBillPrintDocB52cpy("POS", dr.Item("BATCHNO").ToString, Format(dr.Item("TRANDATE"), "yyyy-MM-dd"), "Y")
                End If
            Else
                MsgBox("No Record found", MsgBoxStyle.Information)

                Exit Sub
            End If
        ElseIf tabMain.SelectedTab.Name = "tabEstimation" Then
            dr = Nothing
            strsql = " SELECT BATCHNO,TRANDATE,TRANNO FROM " & cnStockDb & "..ESTISSUE "
            strsql += vbCrLf + " WHERE TRANDATE = '" & Format(dtpfromdate.Value.Date, "yyyy-MM-dd") & "'  "
            strsql += vbCrLf + " AND TRANNO = '" & Val(txtNo.Text) & "' "
            strsql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
            strsql += vbCrLf + " AND ISNULL(CANCEL,'')='' "
            strsql += vbCrLf + " AND TRANTYPE IN('SA')"
            dr = GetSqlRow(strsql, cn)
            If Not dr Is Nothing Then
                Dim obj As New frmBillPrintDocB5("EST", "S." & dr.Item("TRANNO").ToString & ":P." & dr.Item("TRANNO").ToString & "", Format(dr.Item("TRANDATE"), "yyyy-MM-dd"), "Y")
            Else
                MsgBox("No Record found", MsgBoxStyle.Information)
                Exit Sub
            End If
        ElseIf tabMain.SelectedTab.Name = "" Then
        End If
    End Sub
    Private Sub btnSave_Click_1(sender As Object, e As EventArgs) Handles btnSave.Click

        'If CmboBxColNme.Text = "" Then MsgBox("Enter the colmn") : CmboBxColNme.Focus() : Exit Sub
        'If CmboBxCseries.Text = "" Then MsgBox("Enter the H1") : CmboBxCseries.Focus() : Exit Sub
        'If txtbxCvalue.Text = "" Then MsgBox("Enter the H1") : txtbxCvalue.Focus() : Exit Sub

        If Txtstratpos_NUM.Text = "" Then MsgBox("Enter the Start Position") : Txtstratpos_NUM.Focus() : Exit Sub
        ' If TxtBoxH1_NUM.Text = "" Then MsgBox("Enter the H1") : TxtBoxH1_NUM.Focus() : Exit Sub

        If TxtBtmPos_NUM.Text = "" Then MsgBox("Enter the Bottom Position") : TxtBtmPos_NUM.Focus() : Exit Sub
        If txtlinespc_NUM.Text = "" Then MsgBox("Enter the Line Space") : txtlinespc_NUM.Focus() : Exit Sub

        strsql = vbCrLf + " DELETE FROM " & cnAdminDb & "..BILLPRINT_LAYOUT "
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()
        strsql = vbCrLf + " INSERT INTO " & cnAdminDb & "..BILLPRINT_LAYOUT (NAME, XPOS) VALUES('START_POS','" + Txtstratpos_NUM.Text + "')"
        'strsql += vbCrLf + " INSERT INTO " & cnAdminDb & "..BILLPRINT_LAYOUT (NAME,XPOS)VALUES('H1','" + TxtBoxH1_NUM.Text + "')"

        strsql += vbCrLf + " INSERT INTO " & cnAdminDb & "..BILLPRINT_LAYOUT (NAME,XPOS,HEADER)VALUES('" & GridViewbillprint.Rows(0).Cells(0).Value & "','" & GridViewbillprint.Rows(0).Cells(2).Value & "','" & GridViewbillprint.Rows(0).Cells(1).Value & "')"
        strsql += vbCrLf + " INSERT INTO " & cnAdminDb & "..BILLPRINT_LAYOUT (NAME,XPOS,HEADER)VALUES('" & GridViewbillprint.Rows(1).Cells(0).Value & "','" & GridViewbillprint.Rows(1).Cells(2).Value & "','" & GridViewbillprint.Rows(1).Cells(1).Value & "')"
        strsql += vbCrLf + " INSERT INTO " & cnAdminDb & "..BILLPRINT_LAYOUT (NAME,XPOS,HEADER)VALUES('" & GridViewbillprint.Rows(2).Cells(0).Value & "','" & GridViewbillprint.Rows(2).Cells(2).Value & "','" & GridViewbillprint.Rows(2).Cells(1).Value & "')"
        strsql += vbCrLf + " INSERT INTO " & cnAdminDb & "..BILLPRINT_LAYOUT (NAME,XPOS,HEADER)VALUES('" & GridViewbillprint.Rows(3).Cells(0).Value & "','" & GridViewbillprint.Rows(3).Cells(2).Value & "','" & GridViewbillprint.Rows(3).Cells(1).Value & "')"
        strsql += vbCrLf + " INSERT INTO " & cnAdminDb & "..BILLPRINT_LAYOUT (NAME,XPOS,HEADER)VALUES('" & GridViewbillprint.Rows(4).Cells(0).Value & "','" & GridViewbillprint.Rows(4).Cells(2).Value & "','" & GridViewbillprint.Rows(4).Cells(1).Value & "')"
        strsql += vbCrLf + " INSERT INTO " & cnAdminDb & "..BILLPRINT_LAYOUT (NAME,XPOS,HEADER)VALUES('" & GridViewbillprint.Rows(5).Cells(0).Value & "','" & GridViewbillprint.Rows(5).Cells(2).Value & "','" & GridViewbillprint.Rows(5).Cells(1).Value & "')"
        strsql += vbCrLf + " INSERT INTO " & cnAdminDb & "..BILLPRINT_LAYOUT (NAME,XPOS,HEADER)VALUES('" & GridViewbillprint.Rows(6).Cells(0).Value & "','" & GridViewbillprint.Rows(6).Cells(2).Value & "','" & GridViewbillprint.Rows(6).Cells(1).Value & "')"
        strsql += vbCrLf + " INSERT INTO " & cnAdminDb & "..BILLPRINT_LAYOUT (NAME,XPOS,HEADER)VALUES('" & GridViewbillprint.Rows(7).Cells(0).Value & "','" & GridViewbillprint.Rows(7).Cells(2).Value & "','" & GridViewbillprint.Rows(7).Cells(1).Value & "')"
        strsql += vbCrLf + " INSERT INTO " & cnAdminDb & "..BILLPRINT_LAYOUT (NAME,XPOS,HEADER)VALUES('" & GridViewbillprint.Rows(8).Cells(0).Value & "','" & GridViewbillprint.Rows(8).Cells(2).Value & "','" & GridViewbillprint.Rows(8).Cells(1).Value & "')"
        strsql += vbCrLf + " INSERT INTO " & cnAdminDb & "..BILLPRINT_LAYOUT (NAME,XPOS,HEADER)VALUES('" & GridViewbillprint.Rows(9).Cells(0).Value & "','" & GridViewbillprint.Rows(9).Cells(2).Value & "','" & GridViewbillprint.Rows(9).Cells(1).Value & "')"

        strsql += vbCrLf + " INSERT INTO " & cnAdminDb & "..BILLPRINT_LAYOUT (NAME,XPOS)VALUES('BOTTOM_POS','" + TxtBtmPos_NUM.Text + "')"
        strsql += vbCrLf + " INSERT INTO " & cnAdminDb & "..BILLPRINT_LAYOUT (NAME,XPOS)VALUES('LINE_SPACE','" + txtlinespc_NUM.Text + "')"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()
        MessageBox.Show("Insert Sucessfully ")
        cmbload()
    End Sub
    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
#End Region

#Region " Form Load"
    Private Sub frmBillPrintCustDesign_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        strsql = vbCrLf + "SELECT NAME,HEADER,XPOS FROM " & cnAdminDb & "..BILLPRINT_LAYOUT "
        cmd = New OleDbCommand(strsql, cn)
        da = New OleDbDataAdapter(cmd)
        da.Fill(dt)

        If dt.Rows.Count Then
            Txtstratpos_NUM.Text = Val(objGPack.GetSqlValue("SELECT XPOS FROM " & cnAdminDb & "..BILLPRINT_LAYOUT WHERE NAME='START_POS'").ToString)
            ' TxtBoxH1_NUM.Text = Val(objGPack.GetSqlValue("SELECT XPOS FROM " & cnAdminDb & "..BILLPRINT_LAYOUT WHERE NAME='H1'").ToString)
            TxtBtmPos_NUM.Text = Val(objGPack.GetSqlValue("SELECT XPOS FROM " & cnAdminDb & "..BILLPRINT_LAYOUT WHERE NAME='BOTTOM_POS'").ToString)
            txtlinespc_NUM.Text = Val(objGPack.GetSqlValue("SELECT XPOS FROM " & cnAdminDb & "..BILLPRINT_LAYOUT WHERE NAME='LINE_SPACE'").ToString)
            btnNew_Click(Me, New System.EventArgs)
            gridloadvalue()
        Else
            Txtstratpos_NUM.Text = 135
            'TxtBoxH1_NUM.Text = 250
            txtlinespc_NUM.Text = 18
            TxtBtmPos_NUM.Text = 1060
            dt.Rows.Add("C1", "", 25)
            dt.Rows.Add("C2", "", 70)
            dt.Rows.Add("c3", "", 250)
            dt.Rows.Add("C4", "", 350)
            dt.Rows.Add("C5", "", 430)
            dt.Rows.Add("C6", "", 510)
            dt.Rows.Add("C7", "", 560)
            dt.Rows.Add("C8", "", 630)
            dt.Rows.Add("C9", "", 690)
            dt.Rows.Add("C10", "", 770)

            GridViewbillprint.DataSource = dt
        End If

        cmbload()

    End Sub
    Private Sub cmbload()
        CmboBxCseries.Items.Clear()
        CmboBxCseries.Items.Add("C1")
        CmboBxCseries.Items.Add("C2")
        CmboBxCseries.Items.Add("C3")
        CmboBxCseries.Items.Add("C4")
        CmboBxCseries.Items.Add("C5")
        CmboBxCseries.Items.Add("C6")
        CmboBxCseries.Items.Add("C7")
        CmboBxCseries.Items.Add("C8")
        CmboBxCseries.Items.Add("C9")
        CmboBxCseries.Items.Add("C10")

        CmboBxColNme.Items.Clear()
        'CmboBxColNme.Items.Add("SRNO")
        'CmboBxColNme.Items.Add("TRANNO")
        'CmboBxColNme.Items.Add("BILLPREFIX")
        'CmboBxColNme.Items.Add("BILLDATE")
        'CmboBxColNme.Items.Add("BATCHNO")
        CmboBxColNme.Items.Add("SNO")
        CmboBxColNme.Items.Add("DESCRIPTION")
        CmboBxColNme.Items.Add("ITEMNAME")
        CmboBxColNme.Items.Add("HSN")
        CmboBxColNme.Items.Add("QTY")
        CmboBxColNme.Items.Add("GRSWT")
        CmboBxColNme.Items.Add("NETWT")
        CmboBxColNme.Items.Add("LESSWT")
        CmboBxColNme.Items.Add("WASTAGE")
        CmboBxColNme.Items.Add("RATE")
        CmboBxColNme.Items.Add("VA")
        CmboBxColNme.Items.Add("MC")
        CmboBxColNme.Items.Add("TAX")
        CmboBxColNme.Items.Add("AMOUNT")
        CmboBxColNme.Items.Add("BILLNAME")
        'CmboBxColNme.Items.Add("RESULT")
        'CmboBxColNme.Items.Add("TYPE")
        'CmboBxColNme.Items.Add("COLHEAD")
        'CmboBxColNme.Items.Add("CALTYPE")
    End Sub

    Private Sub frmBillPrintCustDesign_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub tabBillLayout_MouseClick(sender As Object, e As MouseEventArgs) Handles tabBillLayout.MouseClick, CmboBxColNme.MouseClick
        ' btnExit.Visible =False
        btnNew.Visible = False
        btnView.Visible = False
        'Me.WindowState = FormWindowState.Maximized
    End Sub
    Private Sub tabAdvance_Click(sender As Object, e As EventArgs) Handles tabAdvance.Click
        btnExit.Visible = True
        btnNew.Visible = True
        btnView.Visible = True

        Me.WindowState = FormWindowState.Normal
    End Sub
    Private Sub tabEstimation_Click(sender As Object, e As EventArgs) Handles tabEstimation.Click
        btnExit.Visible = True
        btnNew.Visible = True
        btnView.Visible = True
        Me.WindowState = FormWindowState.Normal
    End Sub
    Private Sub tabTaxInvoice_Click(sender As Object, e As EventArgs) Handles tabTaxInvoice.Click
        btnExit.Visible = True
        btnNew.Visible = True
        btnView.Visible = True
        Me.WindowState = FormWindowState.Normal
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        'btnreset_NUM.Visible = False
        strsql = vbCrLf + "SELECT * FROM " & cnAdminDb & "..TEMPBILLPRINT_LAYOUT "
        cmd = New OleDbCommand(strsql, cn)
        da = New OleDbDataAdapter(cmd)
        da.Fill(dt)
        If dt.Rows.Count Then
            Txtstratpos_NUM.Text = Val(objGPack.GetSqlValue("SELECT XPOS FROM " & cnAdminDb & "..TEMPBILLPRINT_LAYOUT WHERE NAME='START_POS'").ToString)
            TxtBtmPos_NUM.Text = Val(objGPack.GetSqlValue("SELECT XPOS FROM " & cnAdminDb & "..TEMPBILLPRINT_LAYOUT WHERE NAME='BOTTOM_POS'").ToString)
            txtlinespc_NUM.Text = Val(objGPack.GetSqlValue("SELECT XPOS FROM " & cnAdminDb & "..TEMPBILLPRINT_LAYOUT WHERE NAME='LINE_SPACE'").ToString)
        End If


    End Sub

    Public Sub gridloadvalue()

        dtGrid.Clear()
        strsql = vbCrLf + "SELECT NAME,HEADER,XPOS,LAYOUT FROM " & cnAdminDb & "..BILLPRINT_LAYOUT WHERE NAME ='C1' "
        strsql += vbCrLf + " OR NAME = 'C2'"
        strsql += vbCrLf + " OR NAME = 'C3' "
        strsql += vbCrLf + " OR NAME = 'C4'"
        strsql += vbCrLf + " OR NAME = 'C5'"
        strsql += vbCrLf + " OR NAME = 'C6'"
        strsql += vbCrLf + " OR NAME = 'C7'"
        strsql += vbCrLf + " OR NAME = 'C8'"
        strsql += vbCrLf + " OR NAME = 'C9'"
        strsql += vbCrLf + " OR NAME = 'C10'"

        cmd = New OleDbCommand(strsql, cn)
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtGrid)
        GridViewbillprint.DataSource = Nothing
        GridViewbillprint.DataSource = dtGrid

    End Sub
    Private Sub GridViewbillprint_Click(sender As Object, e As EventArgs) Handles GridViewbillprint.Click
    End Sub
    Private Sub GridViewbillprint_KeyDown(sender As Object, e As KeyEventArgs) Handles GridViewbillprint.KeyDown
        If e.KeyCode = Keys.Enter Then
            'For Each ro1 As DataRow In dtGrid.Rows
            '    CmboBxColNme.Items.Remove(ro1!header)
            'Next
            'CmboBxColNme.Items.Add(GridViewbillprint.CurrentRow.Cells("HEADER").Value.ToString)
            CmboBxCseries.Text = GridViewbillprint.CurrentRow.Cells("NAME").Value.ToString
            CmboBxColNme.Text = GridViewbillprint.CurrentRow.Cells("HEADER").Value.ToString
            txtbxCvalue.Text = GridViewbillprint.CurrentRow.Cells("XPOS").Value.ToString
            CmboBxCseries.Focus()
        End If
    End Sub
    Private Sub txtbxCvalue_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtbxCvalue.KeyPress

        If e.KeyChar = Chr(Keys.Enter) Then

            If CmboBxCseries.Text = "C1" Then
                GridViewbillprint.Rows(0).Cells(0).Value = CmboBxCseries.Text
                GridViewbillprint.Rows(0).Cells(1).Value = CmboBxColNme.Text
                GridViewbillprint.Rows(0).Cells(2).Value = txtbxCvalue.Text
            ElseIf CmboBxCseries.Text = "C2" Then
                GridViewbillprint.Rows(1).Cells(0).Value = CmboBxCseries.Text
                GridViewbillprint.Rows(1).Cells(1).Value = CmboBxColNme.Text
                GridViewbillprint.Rows(1).Cells(2).Value = txtbxCvalue.Text
            ElseIf CmboBxCseries.Text = "C3" Then
                GridViewbillprint.Rows(2).Cells(0).Value = CmboBxCseries.Text
                GridViewbillprint.Rows(2).Cells(1).Value = CmboBxColNme.Text
                GridViewbillprint.Rows(2).Cells(2).Value = txtbxCvalue.Text
            ElseIf CmboBxCseries.Text = "C4" Then
                GridViewbillprint.Rows(3).Cells(0).Value = CmboBxCseries.Text
                GridViewbillprint.Rows(3).Cells(1).Value = CmboBxColNme.Text
                GridViewbillprint.Rows(3).Cells(2).Value = txtbxCvalue.Text
            ElseIf CmboBxCseries.Text = "C5" Then
                GridViewbillprint.Rows(4).Cells(0).Value = CmboBxCseries.Text
                GridViewbillprint.Rows(4).Cells(1).Value = CmboBxColNme.Text
                GridViewbillprint.Rows(4).Cells(2).Value = txtbxCvalue.Text
            ElseIf CmboBxCseries.Text = "C6" Then
                GridViewbillprint.Rows(5).Cells(0).Value = CmboBxCseries.Text
                GridViewbillprint.Rows(5).Cells(1).Value = CmboBxColNme.Text
                GridViewbillprint.Rows(5).Cells(2).Value = txtbxCvalue.Text
            ElseIf CmboBxCseries.Text = "C7" Then
                GridViewbillprint.Rows(6).Cells(0).Value = CmboBxCseries.Text
                GridViewbillprint.Rows(6).Cells(1).Value = CmboBxColNme.Text
                GridViewbillprint.Rows(6).Cells(2).Value = txtbxCvalue.Text
            ElseIf CmboBxCseries.Text = "C8" Then
                GridViewbillprint.Rows(7).Cells(0).Value = CmboBxCseries.Text
                GridViewbillprint.Rows(7).Cells(1).Value = CmboBxColNme.Text
                GridViewbillprint.Rows(7).Cells(2).Value = txtbxCvalue.Text
            ElseIf CmboBxCseries.Text = "C9" Then
                GridViewbillprint.Rows(8).Cells(0).Value = CmboBxCseries.Text
                GridViewbillprint.Rows(8).Cells(1).Value = CmboBxColNme.Text
                GridViewbillprint.Rows(8).Cells(2).Value = txtbxCvalue.Text
            ElseIf CmboBxCseries.Text = "C10" Then
                GridViewbillprint.Rows(9).Cells(0).Value = CmboBxCseries.Text
                GridViewbillprint.Rows(9).Cells(1).Value = CmboBxColNme.Text
                GridViewbillprint.Rows(9).Cells(2).Value = txtbxCvalue.Text

            End If
        End If
    End Sub
    Private Sub GridViewbillprint_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles GridViewbillprint.ColumnAdded
         e.Column.SortMode = DataGridViewColumnSortMode.NotSortable
    End Sub

#End Region
End Class