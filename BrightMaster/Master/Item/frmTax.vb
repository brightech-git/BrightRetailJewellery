Imports System.Data.OleDb
Public Class frmTax
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim flagSave As Boolean = False
    Dim taxCode As String = Nothing
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.tabMain.ItemSize = New Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        objGPack.TextClear(grpInfo)
    End Sub
    Function funcCallGrid() As Integer
        Dim dt As New DataTable
        dt.Clear()
        strSql = " SELECT "
        strSql += " TAXCODE,TAXNAME,"
        strSql += " DISPLAYORDER AS DISP,STAX,"
        strSql += " SSC,SASC,PTAX,PSC,PASC"
        strSql += " FROM " & cnAdminDb & "..TAXMAST"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        gridView.DataSource = dt
        gridView.Columns("TAXCODE").Width = 70
        gridView.Columns("TAXCODE").HeaderText = "ID"
        gridView.Columns("TAXNAME").Width = 250
        For CNT As Integer = 5 To gridView.ColumnCount - 1
            gridView.Columns(CNT).Visible = False
        Next
        gridView.Focus()
        gridView.Select()
    End Function
    Function funcNew() As Integer
        Dim dt As New DataTable
        dt.Clear()
        objGPack.TextClear(Me)
        strSql = " Select isnull(Max(DisplayOrder),0)+1 as Disp from " & cnAdminDb & "..TaxMast"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            txtDisplayOrder_Num.Text = dt.Rows(0).Item("Disp").ToString
        End If
        taxCode = ""
        strSql = " SELECT ISNULL(MAX(CONVERT(INT,TAXCODE)),0)+1 TAXCODE FROM " & cnAdminDb & "..TAXMAST where IsNumeric(taxcode)=1"
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            taxCode = funcSetNumberStyle(dt.Rows(0).Item("TAXCODE").ToString, 2)
        End If
        tabMain.SelectedTab = tabGeneral
        flagSave = False
        txtTaxName__Man.Select()
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        If objGPack.DupChecker(txtTaxName__Man, "SELECT 1 FROM " & cnAdminDb & "..TAXMAST WHERE TAXNAME = '" & txtTaxName__Man.Text & "' AND TAXCODE <> '" & taxCode & "'") Then
            Exit Function
        End If
        If flagSave = False Then
            funcAdd()
        ElseIf flagSave = True Then
            funcUpdate()
        End If
    End Function
    Function funcOpen() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Function
        funcCallGrid()
        tabMain.SelectedTab = tabView
        gridView.Focus()
    End Function
    Function funcAdd() As Integer
        Dim tran As OleDbTransaction = Nothing
        Try
            tran = cn.BeginTransaction()
            strSql = " insert into " & cnAdminDb & "..TaxMast"
            strSql += " ("
            strSql += " Taxcode,"
            strSql += " Taxname,"
            strSql += " Displayorder,"
            strSql += " Stax,"
            strSql += " Ssc,"
            strSql += " SAsc,"
            strSql += " Ptax,"
            strSql += " Psc,"
            strSql += " Pasc,"
            strSql += " UserId,"
            strSql += " Updated,"
            strSql += " Uptime"
            strSql += " ) Values ("
            strSql += " '" & taxCode & "'"
            strSql += " ,'" & txtTaxName__Man.Text & "'"
            strSql += " ," & Val(txtDisplayOrder_Num.Text) & ""
            strSql += " ," & Val(txtSTax_Per.Text) & ""
            strSql += " ," & Val(txtSSurcharge_Per.Text) & ""
            strSql += " ," & Val(txtSAdditionalSc_Per.Text) & ""
            strSql += " ," & Val(txtPTax_Per.Text) & ""
            strSql += " ," & Val(txtPSurcharge_Per.Text) & ""
            strSql += " ," & Val(txtPAdditionalSc_Per.Text) & ""
            strSql += " ," & Val(userId) & ""
            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'"
            strSql += " ,'" & Date.Now.ToLongTimeString & "'"
            strSql += " )"
            ExecQuery(SyncMode.Master, strSql, cn, tran)
            InsertIntoBillControl("TAX-" + taxCode + "-SAL", txtTaxName__Man.Text + " SALES BILLNO", "N", "N", "", "P", tran)
            InsertIntoBillControl("TAX-" + taxCode + "-PUR", txtTaxName__Man.Text + " PURCHASE BILLNO", "N", "N", "", "P", tran)
            tran.Commit()
            funcNew()
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox("Message :" + ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Function
    Function funcUpdate()
        Dim dt As New DataTable
        dt.Clear()
        strSql = " Update " & cnAdminDb & "..TaxMast Set"
        strSql += " Taxcode = '" & taxCode & "'"
        strSql += " ,Taxname= '" & txtTaxName__Man.Text & "'"
        strSql += " ,Displayorder= '" & txtDisplayOrder_Num.Text & "'"
        strSql += " ,Stax= '" & txtSTax_Per.Text & "'"
        strSql += " ,Ssc= '" & txtSSurcharge_Per.Text & "'"
        strSql += " ,SAsc='" & txtSAdditionalSc_Per.Text & "'"
        strSql += " ,Ptax= '" & txtPTax_Per.Text & "'"
        strSql += " ,Psc='" & txtPSurcharge_Per.Text & "'"
        strSql += " ,Pasc='" & txtPAdditionalSc_Per.Text & "'"
        strSql += " ,UserId= '" & userId & "'"
        strSql += " ,Updated= '" & Today.Date.ToString("yyyy-MM-dd") & "'"
        strSql += " ,Uptime= '" & Date.Now.ToLongTimeString & "'"
        strSql += " where TaxCode = '" & taxCode & "'"
        ExecQuery(SyncMode.Master, strSql, cn, tran)
        funcNew()
        Return 0
    End Function
    Function funcGetDetails(ByVal tempTaxCode As String)
        Dim dt As New DataTable
        dt.Clear()
        strSql = " SELECT "
        strSql += " TAXCODE,TAXNAME,DISPLAYORDER,STAX,"
        strSql += " SSC,SASC,PTAX,PSC,PASC"
        strSql += " FROM " & cnAdminDb & "..TAXMAST WHERE TAXCODE = '" & tempTaxCode & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        With dt.Rows(0)
            taxCode = .Item("TAXCODE").ToString
            txtTaxName__Man.Text = .Item("TAXNAME").ToString
            txtDisplayOrder_Num.Text = .Item("DISPLAYORDER").ToString
            txtSTax_Per.Text = .Item("STAX").ToString
            txtSSurcharge_Per.Text = .Item("SSC").ToString
            txtSAdditionalSc_Per.Text = .Item("SASC").ToString
            txtPTax_Per.Text = .Item("PTAX").ToString
            txtPSurcharge_Per.Text = .Item("PSC").ToString
            txtPAdditionalSc_Per.Text = .Item("PASC").ToString
        End With
        flagSave = True
        Return 0
    End Function

    Private Sub frmTax_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                tabMain.SelectedTab = tabGeneral
                txtTaxName__Man.Select()
            End If
        End If
    End Sub

    Private Sub frmTax_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtTaxName__Man.Focused Then
                Exit Sub
            End If
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmTax_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcGridStyle(gridView)

        'funcNumberValidation(txtDisplayOrder_Num)

        'funcDecimalValidation(txtSTax_Per)
        'funcDecimalValidation(txtSSurcharge_Per)
        'funcDecimalValidation(txtSAdditionalSc_Per)
        'funcDecimalValidation(txtPTax_Per)
        'funcDecimalValidation(txtPSurcharge_Per)
        'funcDecimalValidation(txtPAdditionalSc_Per)
        'funcNumberValidation(txtSBillNo_Num)
        'funcNumberValidation(txtSRBillNo_Num)


        'AddHandler txtSTax_Per.KeyPress, AddressOf percentage_Keypress
        'AddHandler txtSSurcharge_Per.KeyPress, AddressOf percentage_Keypress
        'AddHandler txtSAdditionalSc_Per.KeyPress, AddressOf percentage_Keypress
        'AddHandler txtPTax_Per.KeyPress, AddressOf percentage_Keypress
        'AddHandler txtPSurcharge_Per.KeyPress, AddressOf percentage_Keypress
        'AddHandler txtPAdditionalSc_Per.KeyPress, AddressOf percentage_Keypress


        funcNew()
    End Sub
    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        funcSave()
    End Sub
    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        funcOpen()
    End Sub
    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        funcNew()
    End Sub
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        funcExit()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
    End Sub
    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        funcOpen()
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblStatus.Visible = True
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridView.RowCount > 0 Then
                gridView.CurrentCell = gridView.CurrentCell
                funcGetDetails(gridView.Item(0, gridView.CurrentRow.Index).Value.ToString)
                tabMain.SelectedTab = tabGeneral
                txtTaxName__Man.Focus()
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            txtTaxName__Man.Focus()
        End If
    End Sub


    Private Sub txtTaxName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTaxName__Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtTaxName__Man.Text = "" Then
                Exit Sub
            End If
            If objGPack.DupChecker(txtTaxName__Man, "SELECT 1 FROM " & cnAdminDb & "..TAXMAST WHERE TAXNAME = '" & txtTaxName__Man.Text & "' AND TAXCODE <> '" & taxCode & "'") Then
                Exit Sub
            Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        lblStatus.Visible = False

    End Sub

    Private Sub gridView_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.RowEnter
        objGPack.TextClear(grpInfo)
        With gridView.Rows(e.RowIndex)
            detSalesTax.Text = .Cells("STAX").Value.ToString
            detSalesSurcharge.Text = .Cells("SSC").Value.ToString
            detSalesAdlSc.Text = .Cells("SASC").Value.ToString
            detPurTax.Text = .Cells("PTAX").Value.ToString
            detPurSc.Text = .Cells("PSC").Value.ToString
            detPurAdlSc.Text = .Cells("PASC").Value.ToString
        End With
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        Dim delKey As String = gridView.Rows(gridView.CurrentRow.Index).Cells("TAXCODE").Value.ToString
        Dim chkQry As String = " SELECT TOP 1 TAXID FROM " & cnAdminDb & "..CATEGORY WHERE TAXID = '" & delKey & "'"
        DeleteItem(SyncMode.Master, chkQry, "DELETE FROM " & cnAdminDb & "..TAXMAST WHERE TAXCODE = '" & delKey & "'  AND ISNULL(AUTOGENERATOR,'') = '' ")
        funcCallGrid()
    End Sub
End Class