Imports System.Data.OleDb

Public Class frmTagStoneDetails
    Dim dt As New DataTable
    Dim sno As String
    Dim lblTit As String
    Dim StrSql As String = Nothing
    Private WithEvents btnExcelStoneDetails As New Button
    Private WithEvents btnPrintStoneDetails As New Button
    Private WithEvents btnExit As New Button

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Sub New(ByVal sno As String, ByVal lblTit As String)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        Me.sno = sno
        Me.lblTit = lblTit + " (STONE DETAILS)"
        If funcShowStoneDetails(sno) = False Then
            MsgBox("There is no Record", MsgBoxStyle.Exclamation)
            btnExit_Click(Me, New EventArgs)
        End If
        grpStoneDetails.Visible = True
        grpStoneDetails.BringToFront()
        Me.Text = "STONE DETAILS"
        If Not funcShowStoneDetails(sno) = False Then Me.ShowDialog()
        gridStoneDetails.Focus()
    End Sub

    Function funcShowStoneDetails(ByVal tagSno As String) As Boolean
        StrSql = " SELECT ITEMNAME,SUBITEMNAME,"
        StrSql += " CASE WHEN stnPcs = 0 THEN NULL ELSE STNPCS END STNPCS, "
        StrSql += " STONEUNIT,"
        StrSql += " CASE WHEN stnWt = 0 THEN NULL ELSE stnWt END stnWt, "
        StrSql += " CASE WHEN stnRate = 0 THEN NULL ELSE stnRate END stnRate, "
        StrSql += " calcMode, "
        StrSql += " CASE WHEN stnAmt = 0 THEN NULL ELSE stnAmt END stnAmt, "
        StrSql += " RESULT, COLHEAD FROM ("
        StrSql += " select"
        StrSql += " (select itemName from " & cnAdminDb & "..itemMast where itemId = s.StnItemId)ItemName,"
        StrSql += " isnull((select subItemName from " & cnAdminDb & "..subItemMast where subItemId = s.StnSubItemId),'')SubItemName,"
        StrSql += " STNPCS,"
        StrSql += " case when StoneUnit = 'C' then 'CARAT' else 'GRAM' end stoneUnit,"
        StrSql += " stnWt,stnRate, "
        StrSql += " case when calcMode = 'W' then 'WEIGHT' else 'RATE' end calcMode,"
        StrSql += " stnAmt, "
        StrSql += " '1'as Result,CONVERT(VARCHAR(1),'')COLHEAD"
        StrSql += " from " & cnAdminDb & "..ITEMTAGSTONE as s where TagSno = '" & tagSno & "'"
        StrSql += " Union All"
        StrSql += " select"
        StrSql += " 'TOTAL' as ItemName,"
        StrSql += " '' as SubItemName,"
        StrSql += " sum(stnPcs)stnPcs,"
        StrSql += " '' as stoneUnit,"
        StrSql += " sum(stnWt)stnWt,"
        StrSql += " sum(stnRate)stnWt,"
        StrSql += " '' as calcMode,"
        StrSql += " sum(stnAmt)stnAmt,"
        StrSql += " '2'as Result,'G' COLHEAD"
        StrSql += " from " & cnAdminDb & "..ITEMTAGSTONE as s where TagSno = '" & tagSno & "'"
        StrSql += " ) X"

        Dim dt As New DataTable("StoneDetails")
        dt.Clear()
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return False
        End If
        If dt.Rows(0).Item("Result") <> "1" Then
            Return False
        End If
        gridStoneDetails.DataSource = dt
        With gridStoneDetails
            .Columns("COLHEAD").Visible = False
            With .Columns("ItemName")
                .HeaderText = "ITEM NAME"
                .Width = 150
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("SubItemName")
                .HeaderText = "SUB ITEM"
                .Width = 150
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("stnPcs")
                .HeaderText = "PCS"
                .Width = 40
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("StoneUnit")
                .HeaderText = "UNIT"
                .Width = 50
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("stnWt")
                .HeaderText = "WEIGHT"
                .Width = 60
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("stnRate")
                .HeaderText = "RATE"
                .Width = 80
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("calcMode")
                .HeaderText = "CAL.MODE"
                .Width = 70
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("stnAmt")
                .HeaderText = "AMOUNT"
                .Width = 90
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("Result")
                .HeaderText = "Result"
                .Width = 0
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
            End With
        End With
        lblTitle.Text = lblTit
        Return True
    End Function

    Private Sub btnExcelStoneDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcelStoneDetails.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridStoneDetails.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridStoneDetails, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrintStoneDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintStoneDetails.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridStoneDetails.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridStoneDetails, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub gridStoneDetails_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridStoneDetails.CellFormatting
        With gridStoneDetails
            Select Case .Rows(e.RowIndex).Cells("COLHEAD").Value.ToString
                Case "G"
                    .Rows(e.RowIndex).DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                    .Rows(e.RowIndex).DefaultCellStyle.Font = reportTotalStyle.Font
            End Select
        End With
    End Sub

    Private Sub gridStoneDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridStoneDetails.KeyPress
        If e.KeyChar = Chr(Keys.Escape) Then
            btnExit_Click(Me, New EventArgs)
        ElseIf e.KeyChar = Chr(Keys.X) Or AscW(e.KeyChar) = 120 Then
            Me.btnExcelStoneDetails_Click(Me, New EventArgs)
        ElseIf e.KeyChar = Chr(Keys.P) Or UCase(e.KeyChar) = "P" Then
            Me.btnPrintStoneDetails_Click(Me, New EventArgs)
        End If
    End Sub
End Class