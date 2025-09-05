Imports System.Data.OleDb
Public Class ItemWiseProfit
    Dim objGridShower As frmGridDispDia
    Dim Cmd As OleDbCommand
    Dim StrSql As String
    Dim Da As OleDbDataAdapter


    Private Sub ItemWiseProfit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub ItemWiseProfit_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GrpContainer.Location = New Point((ScreenWid - GrpContainer.Width) / 2, ((ScreenHit - 128) - GrpContainer.Height) / 2)
        StrSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY METALNAME"
        objGPack.FillCombo(StrSql, cmbMetal)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        'chkTag.Checked = True
        'chkNonTag.Checked = True
        'cmbMetal.Text = "GOLD"
        Prop_Gets()
        dtpFrom.Select()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not chkTag.Checked And Not chkNonTag.Checked Then
            chkTag.Checked = True
            chkNonTag.Checked = True
        End If
        Dim tp As String = ""
        If chkTag.Checked Then tp += "T,"
        If chkNonTag.Checked Then tp += "N,"
        If tp <> "" Then
            tp = Mid(tp, 1, tp.Length - 1)
        End If
        StrSql = " EXEC " & cnStockDb & "..SP_RPT_ITEMWISEPROFIT"
        StrSql += " @FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += " ,@METAL = '" & cmbMetal.Text & "'"
        StrSql += " ,@OPTTAG = '" & tp & "'"
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.CommandTimeout = 1000
        Cmd.CommandTimeout = 2000
        Da = New OleDbDataAdapter(StrSql, cn)
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        Da.Fill(dtGrid)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "ITEM WISE PROFIT"
        Dim tit As String = "ITEM WISE PROFIT ANALYSIS" + vbCrLf
        tit += " DATE FROM " & dtpFrom.Text + " TO " + dtpTo.Text
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        DataGridView_SummaryFormatting(objGridShower.gridView)
        FormatGridColumns(objGridShower.gridView, False, False, , False)

        objGridShower.Show()
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        'objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("PARTICULAR")))
        Prop_Sets()
    End Sub

    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            .Columns("BILLTYPE").HeaderText = "TYPE"
            .Columns("BILLTYPE").Width = 40
            .Columns("TRANNO").Width = 60
            .Columns("TRANDATE").Width = 80
            .Columns("TAGNO").Width = 70
            .Columns("ITEMNAME").Width = 150
            .Columns("PCS").Width = 60
            .Columns("GRSWT").Width = 80
            .Columns("NETWT").Width = 80
            .Columns("GRSNET").Width = 40
            .Columns("RATE").Width = 100
            .Columns("TOUCH").Width = 60
            .Columns("TAGVALUE").Width = 100
            .Columns("AMOUNT").Width = 100
            .Columns("DIFF").Width = 100
            .Columns("DIFFPER").Width = 70
            .Columns("DAYS").Width = 60
            .Columns("SALESPERSON").Width = 120
            .Columns("KEYNO").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("RESULT").Visible = False
            FormatGridColumns(dgv, False, False, , False)
            .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New ItemWiseProfit_Properties
        GetSettingsObj(obj, Me.Name, GetType(ItemWiseProfit_Properties))
        cmbMetal.Text = obj.p_cmbMetal
        chkTag.Checked = obj.p_chkTag
        chkNonTag.Checked = obj.p_chkNonTag
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New ItemWiseProfit_Properties
        obj.p_cmbMetal = cmbMetal.Text
        obj.p_chkTag = chkTag.Checked
        obj.p_chkNonTag = chkNonTag.Checked
        SetSettingsObj(obj, Me.Name, GetType(ItemWiseProfit_Properties))
    End Sub

End Class
Public Class ItemWiseProfit_Properties
    Private cmbMetal As String = ""
    Public Property p_cmbMetal() As String
        Get
            Return cmbMetal
        End Get
        Set(ByVal value As String)
            cmbMetal = value
        End Set
    End Property
    Private chkTag As Boolean = True
    Public Property p_chkTag() As Boolean
        Get
            Return chkTag
        End Get
        Set(ByVal value As Boolean)
            chkTag = value
        End Set
    End Property
    Private chkNonTag As Boolean = True
    Public Property p_chkNonTag() As String
        Get
            Return chkNonTag
        End Get
        Set(ByVal value As String)
            chkNonTag = value
        End Set
    End Property
End Class