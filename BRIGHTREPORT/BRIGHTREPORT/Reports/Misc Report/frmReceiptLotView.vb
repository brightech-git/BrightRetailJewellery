Imports System.Data.OleDb
Public Class frmReceiptLotView
    Dim strsql As String
    Dim objGridShower As frmGridDispDia
    Dim cmd As OleDbCommand
    Dim viewtype As String
    Dim TAGTOLLERENCE As Decimal = GetAdmindbSoftValue("TAGTOLERANCE", "0.000")

    Private Sub frmReceiptLotView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmReceiptLotView_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        objGPack.TextClear(Me)
        dtpFrom.Value = GetEntryDate(GetServerDate)
        dtpTo.Value = GetEntryDate(GetServerDate)
        cmbViewType.Items.Clear()
        cmbViewType.Items.Add("Detailed View")
        cmbViewType.Items.Add("Summary View")
        cmbViewType.SelectedIndex = 0
        Prop_Gets()
        dtpFrom.Select()
        rbtItem.Checked = True
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Prop_Sets()
        Dim BILS As String

        viewtype = ""
        BILS = ""
        If rbtItem.Checked = True Then BILS = "I"
        If rbtDelear.Checked = True Then BILS = "D"
        If cmbViewType.Text = "Detailed View" Then
            viewtype = "D"
        Else
            viewtype = "S"
        End If

        strsql = " EXEC " & cnAdminDb & "..SP_RPT_RECEIPTTAG "
        strsql += vbCrLf + " @FROMDATE='" & Format(dtpFrom.Value, "yyyy-MM-dd") & "'"
        strsql += vbCrLf + " ,@TODATE='" & Format(dtpTo.Value, "yyyy-MM-dd") & "'"
        strsql += vbCrLf + " ,@DBNAME='" & cnStockDb & "'"
        strsql += vbCrLf + " ,@SYSTEMID='" & systemId & "'"
        strsql += vbCrLf + " ,@GROUPBY='" & BILS & "' "
        strsql += vbCrLf + " ,@FILTER='" & IIf(rbttranno.Checked, "Y", "N") & "' "
        strsql += vbCrLf + " ,@VIEWTYPE='" & viewtype & "'"
        cmd = New OleDbCommand(strsql, cn)

        da = New OleDbDataAdapter(cmd)
        Dim dss As New DataSet
        da.Fill(dss)
        Dim dt As New DataTable
        Dim dtGrid1 As New DataTable
        dtGrid1 = dss.Tables(0)

        Dim dtGrid As New DataTable
        dtGrid = dtGrid1.Copy
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0

        dtGrid.Columns("KEYNO").AutoIncrementStep = 1

        'For v As Integer = 0 To dtGrid.Rows.Count - 1
        '    If Val(dtGrid.Rows(v).Item("DIFFGWT").ToString()) < TAGTOLLERENCE Then
        '        dtGrid.Rows(v).Item("DIFFGWT") = 0
        '        dtGrid.Rows(v).Item("DIFFNWT") = 0
        '    End If
        'Next


        If Not dtGrid.Rows.Count > 1 Then
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            Exit Sub
        End If

        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Size = New Size(778, 550)
        objGridShower.Text = "RECEIPT LOT VIEW "
        Dim tit As String = "RECEIPT LOT VIEW "
        tit += "DATE FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        AddHandler objGridShower.gridView.Scroll, AddressOf gridView_Scroll
        AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen


        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.pnlFooter.Visible = False
        objGridShower.FormReLocation = True
        objGridShower.Show()


        DataGridView_Summary_None(objGridShower.gridView)
        objGridShower.FormReSize = True
        objGridShower.gridViewHeader.Visible = True
        GridViewHeaderCreator(objGridShower.gridViewHeader)
        FillGridGroupStyle(objGridShower.gridView, "PARTICULAR")
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle = objGridShower.gridView.ColumnHeadersDefaultCellStyle
    End Sub
    Private Sub GridViewHeaderCreator(ByVal gridviewHead As DataGridView)
        Dim dtHead As New DataTable
        strsql = "SELECT ''[PARTICULAR],''[RDATE~RTRANNO~RPCS~RGRSWT~RNETWT~RDIAPCS~RDIAWT],''[LOTNO~LPCS~LGRSWT~LNETWT~LDIAPCS~LDIAWT],''[NOOFTAG~SPCS~SGRSWT~SNETWT~SDIAPCS~SDIAWT],''[DIFFPCS~DIFFGWT~DIFFNWT~DIFFDIAPCS~DIFFDIAWT],''SCROLL"
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtHead)
        gridviewHead.DataSource = dtHead
        gridviewHead.Columns("PARTICULAR").HeaderText = ""
        gridviewHead.Columns("RDATE~RTRANNO~RPCS~RGRSWT~RNETWT~RDIAPCS~RDIAWT").HeaderText = "RECEIPT"
        gridviewHead.Columns("LOTNO~LPCS~LGRSWT~LNETWT~LDIAPCS~LDIAWT").HeaderText = "LOT"
        gridviewHead.Columns("NOOFTAG~SPCS~SGRSWT~SNETWT~SDIAPCS~SDIAWT").HeaderText = "STOCK"
        gridviewHead.Columns("DIFFPCS~DIFFGWT~DIFFNWT~DIFFDIAPCS~DIFFDIAWT").HeaderText = "DIFFERENCE"
        gridviewHead.Columns("SCROLL").HeaderText = ""
        gridviewHead.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        SetGridHeadColWid(gridviewHead)
    End Sub
    Private Sub DataGridView_Summary_None(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            .Columns("PARTICULAR").Width = 150
            .Columns("RDATE").Width = 100
            .Columns("RTRANNO").Width = 60
            .Columns("RPCS").Width = 60
            .Columns("RGRSWT").Width = 80
            .Columns("RNETWT").Width = 80

            .Columns("LOTNO").Width = 80
            .Columns("LPCS").Width = 60
            .Columns("LGRSWT").Width = 80
            .Columns("LNETWT").Width = 80

            .Columns("NOOFTAG").Width = 80
            .Columns("SPCS").Width = 60
            .Columns("SGRSWT").Width = 80
            .Columns("SNETWT").Width = 80

            .Columns("RPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("RGRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("RNETWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("RDIAWT").DefaultCellStyle.BackColor = Color.AliceBlue

            .Columns("LPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("LGRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("LNETWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("LDIAWT").DefaultCellStyle.BackColor = Color.AliceBlue

            .Columns("SPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("SGRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("SNETWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("SDIAWT").DefaultCellStyle.BackColor = Color.AliceBlue

            .Columns("DIFFPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("DIFFGWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("DIFFNWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("DIFFDIAWT").DefaultCellStyle.BackColor = Color.AliceBlue

            .Columns("DIFFPCS").DefaultCellStyle.ForeColor = Color.Red
            .Columns("DIFFGWT").DefaultCellStyle.ForeColor = Color.Red
            .Columns("DIFFNWT").DefaultCellStyle.ForeColor = Color.Red
            .Columns("DIFFDIAWT").DefaultCellStyle.ForeColor = Color.Red

            .Columns("RDATE").HeaderText = "DATE"
            .Columns("RTRANNO").HeaderText = "TRANNO"
            .Columns("RPCS").HeaderText = "PCS"
            .Columns("RGRSWT").HeaderText = "GRSWT"
            .Columns("RNETWT").HeaderText = "NETWT"
            .Columns("RDIAWT").HeaderText = "DIAWT"
            .Columns("RDIAPCS").HeaderText = "DIAPCS"

            .Columns("LOTNO").HeaderText = "LOTNO"
            .Columns("LPCS").HeaderText = "PCS"
            .Columns("LGRSWT").HeaderText = "GRSWT"
            .Columns("LNETWT").HeaderText = "NETWT"
            .Columns("LDIAWT").HeaderText = "DIAWT"
            .Columns("LDIAPCS").HeaderText = "DIAPCS"

            .Columns("NOOFTAG").HeaderText = "NOOFTAGS"
            .Columns("SPCS").HeaderText = "PCS"
            .Columns("SGRSWT").HeaderText = "GRSWT"
            .Columns("SNETWT").HeaderText = "NETWT"
            .Columns("SDIAWT").HeaderText = "DIAWT"
            .Columns("SDIAPCS").HeaderText = "DIAPCS"
            If viewtype = "S" Then
                .Columns("RDATE").Visible = False
                .Columns("RTRANNO").Visible = False
            End If
            .Columns("ACNAME").Visible = False
            .Columns("ITEM").Visible = False

            .Columns("DIFFPCS").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("KEYNO").Visible = False
            BrighttechPack.GlobalMethods.FormatGridColumns(dgv, True)
            'FormatGridColumns(dgv, False)
        End With
    End Sub
  
    Private Sub SetGridHeadColWid(ByVal gridViewHeader As DataGridView)
        Dim f As frmGridDispDia
        f = objGPack.GetParentControl(gridViewHeader)
        If Not f.gridViewHeader.Visible Then Exit Sub
        If f.gridViewHeader Is Nothing Then Exit Sub
        If Not f.gridView.ColumnCount > 0 Then Exit Sub
        If Not f.gridViewHeader.ColumnCount > 0 Then Exit Sub
        With f.gridViewHeader
            .Columns("PARTICULAR").Width = f.gridView.Columns("PARTICULAR").Width
            .Columns("RDATE~RTRANNO~RPCS~RGRSWT~RNETWT~RDIAPCS~RDIAWT").Width = f.gridView.Columns("RDATE").Width + f.gridView.Columns("RTRANNO").Width + f.gridView.Columns("RPCS").Width + f.gridView.Columns("RGRSWT").Width + f.gridView.Columns("RNETWT").Width + f.gridView.Columns("RDIAWT").Width + f.gridView.Columns("RDIAPCS").Width
            .Columns("LOTNO~LPCS~LGRSWT~LNETWT~LDIAPCS~LDIAWT").Width = f.gridView.Columns("LOTNO").Width + f.gridView.Columns("LPCS").Width + f.gridView.Columns("LGRSWT").Width + f.gridView.Columns("LNETWT").Width + f.gridView.Columns("LDIAWT").Width + f.gridView.Columns("LDIAPCS").Width
            .Columns("NOOFTAG~SPCS~SGRSWT~SNETWT~SDIAPCS~SDIAWT").Width = f.gridView.Columns("NOOFTAG").Width + f.gridView.Columns("SPCS").Width + f.gridView.Columns("SGRSWT").Width + f.gridView.Columns("SNETWT").Width + f.gridView.Columns("SDIAWT").Width + f.gridView.Columns("SDIAPCS").Width
            .Columns("DIFFPCS~DIFFGWT~DIFFNWT~DIFFDIAPCS~DIFFDIAWT").Width = f.gridView.Columns("DIFFPCS").Width + f.gridView.Columns("DIFFGWT").Width + f.gridView.Columns("DIFFNWT").Width + f.gridView.Columns("DIFFDIAWT").Width + f.gridView.Columns("DIFFDIAPCS").Width
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To f.gridView.ColumnCount - 1
                If f.gridView.Columns(cnt).Visible Then colWid += f.gridView.Columns(cnt).Width
            Next
            If colWid >= f.gridView.Width Then
                f.gridViewHeader.Columns("SCROLL").Visible = CType(f.gridView.Controls(1), VScrollBar).Visible
                f.gridViewHeader.Columns("SCROLL").Width = CType(f.gridView.Controls(1), VScrollBar).Width
                f.gridViewHeader.Columns("SCROLL").HeaderText = ""
            Else
                f.gridViewHeader.Columns("SCROLL").Visible = False
            End If
        End With
    End Sub

    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs)
        SetGridHeadColWid(CType(sender, DataGridView))
    End Sub

    Private Sub gridView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs)
        Try
            Dim f As frmGridDispDia
            f = objGPack.GetParentControl(CType(sender, DataGridView))
            If Not f.gridViewHeader.Visible Then Exit Sub
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                f.gridViewHeader.HorizontalScrollingOffset = e.NewValue
                f.gridViewHeader.Columns("SCROLL").Visible = CType(f.gridView.Controls(1), VScrollBar).Visible
                f.gridViewHeader.Columns("SCROLL").Width = CType(f.gridView.Controls(1), VScrollBar).Width
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmReceiptLotView_Properties
        obj.p_rbtItem = rbtItem.Checked
        obj.p_rbtLotno = rbtDelear.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmReceiptLotView_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmReceiptLotView_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmReceiptLotView_Properties))
        rbtItem.Checked = obj.p_rbtItem
        rbtDelear.Checked = obj.p_rbtLotno
    End Sub

    Private Sub rbttranno_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbttranno.CheckedChanged

    End Sub

    Private Sub rbtDelear_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtDelear.CheckedChanged

    End Sub
End Class

Public Class frmReceiptLotView_Properties
    Private rbtItem As Boolean = True
    Public Property p_rbtItem() As Boolean
        Get
            Return rbtItem
        End Get
        Set(ByVal value As Boolean)
            rbtItem = value
        End Set
    End Property
    Private rbtLotno As Boolean = False
    Public Property p_rbtLotno() As Boolean
        Get
            Return rbtLotno
        End Get
        Set(ByVal value As Boolean)
            rbtLotno = value
        End Set
    End Property
End Class