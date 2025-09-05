Imports System.Data.OleDb
Imports System.Drawing.Imaging
Imports System.Windows.Input

Public Class frmPurchaseOrderDetail
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim isLot As Boolean
    Public itemId As Integer
    Public itemName As String
    Public subItem As String
    Public pieces As Integer
    Sub New(metal As String, itemId As String, fromDate As Date, toDate As Date, poNo As String, item As String, totPieces As Integer)
        InitializeComponent()
        ShowDetail(metal, itemId, fromDate, toDate, poNo, item, totPieces)
    End Sub
    Sub New(dt As DataTable, _particular As String)
        InitializeComponent()
        ShowDetail(dt, _particular)
    End Sub
    Sub New(dt As DataTable, _isLot As Boolean)
        InitializeComponent()
        ShowDetail(dt)
        isLot = True
    End Sub
    Sub New(dt As DataTable)
        InitializeComponent()
        ShowDetail(dt)
    End Sub
    Private Sub ShowDetail(metal As String, itemId As String, fromDate As Date, toDate As Date, poNo As String, item As String, totPieces As Integer)
        Try
            'strSql = $"select cast(a.ITEMID as nvarchar(100))+ '-' + b.ITEMNAME ITEM,a.PARTICULAR,a.PO_PIECES,a.POFROMDATE POSTINGFROM,a.POTODATE POSTINGTO,a.PONUMBER,a.PODATE from {cnAdminDb}.. PURCHASEORDER a"
            strSql = $"select a.PARTICULAR,a.PO_PIECES,a.PODATE from {cnAdminDb}.. PURCHASEORDER a"
            strSql += $" join {cnAdminDb}..itemmast b on a.itemid = b.ITEMID"
            strSql += " where ISNULL(po_pieces,0)<>0"
            If metal <> "ALL" And metal <> "" Then
                strSql += " AND b.METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(metal) & "))"
            End If
            If itemId <> "ALL" Then strSql += $" And a.itemid in ({itemId})"
            strSql += $" And a.podate between '{fromDate}' and '{toDate}'"
            strSql += $" And a.PONUMBER = '{poNo}'"
            strSql += " order by ponumber"

            cmd = New OleDb.OleDbCommand(strSql, cn)
            da = New OleDbDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)

            Dim dr As DataRow = dt.NewRow
            dr(0) = " TOTAL" : dr(1) = totPieces
            dt.Rows.Add(dr)
            dt.AcceptChanges()

            DgView.DataSource = Nothing
            DgView.DataSource = dt

            Dim tit As String
            tit = " PURCHASE ORDER DETAIL" + vbCrLf
            tit += " DATE : " & fromDate.ToString("dd-MM-yyyy") + "  -  " + toDate.ToString("dd-MM-yyyy") + vbCrLf
            tit += " FOR ITEM : " & item + vbCrLf
            tit += " PONUMBER : " & poNo
            lblHead.Text = tit.ToString
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Private Sub ShowDetail(dt As DataTable, _particular As String)
        Try
            Dim newRow As DataRow = dt.NewRow()

            Dim sumIPCS As Double = 0
            Dim sumIGRSWT As Double = 0
            Dim sumINETWT As Double = 0
            Dim sumCPCS As Double = 0
            Dim sumCGRSWT As Double = 0
            Dim sumCNETWT As Double = 0

            For Each row As DataRow In dt.Rows
                sumIPCS += If(IsDBNull(row("IPCS")), 0, Convert.ToDouble(row("IPCS")))
                sumIGRSWT += If(IsDBNull(row("IGRSWT")), 0, Convert.ToDouble(row("IGRSWT")))
                sumINETWT += If(IsDBNull(row("INETWT")), 0, Convert.ToDouble(row("INETWT")))
                sumCPCS += If(IsDBNull(row("CPCS")), 0, Convert.ToDouble(row("CPCS")))
                sumCGRSWT += If(IsDBNull(row("CGRSWT")), 0, Convert.ToDouble(row("CGRSWT")))
                sumCNETWT += If(IsDBNull(row("CNETWT")), 0, Convert.ToDouble(row("CNETWT")))
            Next

            newRow("PARTICULAR") = "TOTAL"
            newRow("IPCS") = sumIPCS
            newRow("IGRSWT") = sumIGRSWT
            newRow("INETWT") = sumINETWT
            newRow("CPCS") = sumCPCS
            newRow("CGRSWT") = sumCGRSWT
            newRow("CNETWT") = sumCNETWT
            dt.Rows.Add(newRow)

            DgView.DataSource = Nothing
            DgView.DataSource = dt
            'DgView.Columns("PARTICULAR").Visible = False

            For i As Integer = 0 To DgView.Columns.Count - 1
                Dim column As DataGridViewColumn = DgView.Columns(i)
                If column.Name.StartsWith("I") Then
                    column.DefaultCellStyle.BackColor = GetLightColor(Color.LightBlue, 0.5)
                    column.DefaultCellStyle.ForeColor = Color.Black
                End If
            Next

            For i As Integer = 0 To DgView.Columns.Count - 1
                Dim column As DataGridViewColumn = DgView.Columns(i)
                If column.Name.StartsWith("C") Then
                    column.DefaultCellStyle.BackColor = GetLightColor(Color.LightPink, 0.5)
                    column.DefaultCellStyle.ForeColor = Color.Black
                End If
            Next

            Dim tit As String
            tit = " RANGEWISE STOCK ISSUE DETAIL" + vbCrLf
            tit += " PARTICULAR : " & _particular
            lblHead.Text = tit.ToString
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Private Sub ShowDetail(dt As DataTable)
        Try
            DgView.DataSource = Nothing
            DgView.DataSource = dt
            btnExport.Visible = False : btnPrint.Visible = False
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Private Function GetLightColor(baseColor As Color, factor As Double) As Color
        Dim r As Integer = Math.Min(255, CInt(baseColor.R + (255 - baseColor.R) * factor))
        Dim g As Integer = Math.Min(255, CInt(baseColor.G + (255 - baseColor.G) * factor))
        Dim b As Integer = Math.Min(255, CInt(baseColor.B + (255 - baseColor.B) * factor))
        Return Color.FromArgb(255, r, g, b)
    End Function
    Dim colorPalette As Color() = {
    Color.LightBlue,
    Color.LightCoral,
    Color.LightGreen,
    Color.LightGoldenrodYellow,
    Color.LightPink
}
    Private Sub Clear()
        lblHead.Text = ""
        DgView.DataSource = Nothing
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Try
            Clear()
            Me.DialogResult = Windows.Forms.DialogResult.OK
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If DgView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblHead.Text, DgView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If DgView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblHead.Text, DgView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub frmPurchaseOrderDetail_KeyDown(sender As Object, e As Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            btnBack_Click(sender, e)
        End If
    End Sub

    Private Sub DgView_KeyDown(sender As Object, e As Windows.Forms.KeyEventArgs) Handles DgView.KeyDown
        Try
            If isLot Then
                If e.KeyCode = Keys.Enter Then
                    itemId = DgView.Item("ITEMID", DgView.CurrentRow.Index).Value.ToString
                    itemName = DgView.Item("ITEMNAME", DgView.CurrentRow.Index).Value.ToString
                    subItem = DgView.Item("SUBITEM", DgView.CurrentRow.Index).Value.ToString
                    pieces = DgView.Item("PO_PIECES", DgView.CurrentRow.Index).Value.ToString
                    Me.DialogResult = DialogResult.OK
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
End Class