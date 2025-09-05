Imports System.Data.OleDb
Public Class frmBillNoview
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim objGridShower As frmGridDispDia
    Dim dt As New DataTable

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    

    
    Function funcExit() As Integer
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Function
    Function funcFillGrid() As Integer
        gridView.DataSource = Nothing
        Dim dt As New DataTable
        dt.Clear()
        dt.Rows.Clear()

        strSql = " SELECT CTLNAME AS PARTICULAR,CTLTEXT AS [LAST NO] FROM " & cnStockDb & "..BILLCONTROL"
        strSql += " WHERE CTLMODE  <> 'N' AND CTLMODULE ='P' AND CTLTEXT NOT IN ('Y','N')"
        strSql += " AND COMPANYID = '" & strCompanyId & "'"
        strSql += " ORDER BY CTLNAME"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        gridView.DataSource = dt
        gridView.Columns("PARTICULAR").Width = 250

    End Function



    Private Sub frmRateMaster_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
        End If
    End Sub

    Private Sub frmRateMaster_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView.Focused Then
                Exit Sub
            End If
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmRateMaster_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        gridView.BorderStyle = BorderStyle.None
        gridView.SelectionMode = DataGridViewSelectionMode.CellSelect
        gridView.BackgroundColor = Color.WhiteSmoke
        gridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        gridView.ColumnHeadersHeight = 25
        gridView.RowTemplate.Height = 25
        gridView.RowTemplate.Resizable = DataGridViewTriState.False
        gridView.DefaultCellStyle.SelectionBackColor = Color.White
        gridView.DefaultCellStyle.SelectionForeColor = Color.Black
        funcFillGrid()

    End Sub
   

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        funcExit()
    End Sub

   
    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        gridView.DefaultCellStyle.SelectionBackColor = Color.LightGoldenrodYellow
        gridView.DefaultCellStyle.SelectionForeColor = Color.Black
    End Sub

   
    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        gridView.DefaultCellStyle.SelectionBackColor = Color.White
        gridView.DefaultCellStyle.SelectionForeColor = Color.Black
    End Sub

    

   
   
    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

       

        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dt)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = True
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        objGridShower.gridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        'DataGridView_SummaryFormatting(objGridShower.gridView)
        objGridShower.formuser = userId
        objGridShower.Show()
        'With objGridShower.gridView
        '    .Columns("METAL").Width = 100
        '    .Columns("PURITY").Width = 250
        '    .Columns("RATE").Width = 120
        '    .Columns("RATEGROUP").Visible = False

        '    .Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        'End With
        'DgvFormat(objGridShower.gridView)

        objGridShower.FormReLocation = True
        objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("METAL")))
    End Sub

    'Private Sub DgvFormat(ByVal Dgv As DataGridView)
    '    With Dgv
    '        Dim RateGroup As String = ""
    '        Dim Metal As String = ""
    '        Dim RowColor As Color = Color.White
    '        For cnt As Integer = 0 To .RowCount - 1
    '            If RateGroup <> .Rows(cnt).Cells("RATEGROUP").Value.ToString Then
    '                RowColor = IIf(RowColor = Color.White, Color.AliceBlue, Color.White)
    '                Metal = ""
    '                RateGroup = .Rows(cnt).Cells("RATEGROUP").Value.ToString
    '            End If
    '            If Metal <> .Rows(cnt).Cells("METAL").Value.ToString Then
    '                Metal = .Rows(cnt).Cells("METAL").Value.ToString
    '            Else
    '                .Rows(cnt).Cells("METAL").Value = DBNull.Value
    '            End If
    '            .Rows(cnt).DefaultCellStyle.BackColor = RowColor
    '        Next
    '    End With
    'End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        btnView_Click(Me, New EventArgs)
    End Sub

    Private Sub gridView_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellContentClick

    End Sub
End Class