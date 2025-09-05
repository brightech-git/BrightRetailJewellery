Imports System.Data.OleDb
Public Class frmMIMRAcnameWiseDetailReport

#Region " Varible"
    Dim strsql As String = ""
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim ds As New DataSet
    Dim dt As New DataTable
    Dim Batchno As String = ""
    Dim Acname As String = ""
#End Region

#Region " User Define function"
    Private Sub AutoResize2(ByVal grid1 As DataGridView)
        If grid1.RowCount > 0 Then
            If True Then
                grid1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                grid1.Invalidate()
                For Each dgvCol As DataGridViewColumn In grid1.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                grid1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In grid1.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                grid1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
        End If
    End Sub
#End Region

#Region " constructor"
    Public Sub New(ByVal _batchno As String, ByVal _AcName As String)
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        Batchno = _batchno
        Acname = _AcName
        strsql = ""
        strsql += vbCrLf + " EXEC " & cnAdminDb & "..[SP_RPT_MIMR_NEW_ACNAME_DETAIL] "
        strsql += vbCrLf + "@DBNAME = '" & cnStockDb & "'"
        strsql += vbCrLf + ",@ADMINDB ='" & cnAdminDb & "'"
        strsql += vbCrLf + ",@SYSTEMID ='" & systemId & "'"
        strsql += vbCrLf + ",@BATCHNO ='" & Batchno & "'"
        cmd = New OleDbCommand(strsql, cn)
        da = New OleDbDataAdapter(cmd)
        ds = New DataSet
        da.Fill(ds)
        dt = ds.Tables(0)
        If dt.Rows.Count > 0 Then
            With gridView
                .DataSource = Nothing
                .DataSource = dt
                .Columns("SNO").Visible = False
                .Columns("ACCODE").Visible = False
                .Columns("RESULT").Visible = False
                .Columns("COLHEAD").Visible = False
                .Columns("ROWID").Visible = False
                .Columns("CATCODE").Visible = False
                .Columns("BATCHNO").Visible = False
                .Columns("TABLENAME").Visible = False
            End With
        End If
    End Sub
#End Region

#Region " Form Load"
    Private Sub frmMIMRAcnameWiseDetailReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If gridView.Rows.Count > 0 Then
            FormatGridColumns(gridView, False, True, True, False)
            AutoResize2(gridView)
        End If
    End Sub

    Private Sub frmMIMRAcnameWiseDetailReport_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{tab}")
        ElseIf e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub btnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.Click
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "MATERIAL RECEIPT/ISSUE ACNAME DETAIL VIEW " & Acname, gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "MATERIAL RECEIPT/ISSUE ACNAME DETAIL VIEW " & Acname, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub
#End Region
End Class