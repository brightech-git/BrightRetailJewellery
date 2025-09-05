Imports System.Data.OleDb
Public Class frmRateView
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim dtGrid As DataTable

    Private Sub frmRateView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmRateView_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        dtpFrom.MinimumDate = (New DateTimePicker).MinDate
        dtpFrom.MaximumDate = (New DateTimePicker).MaxDate
        dtpTo.MinimumDate = (New DateTimePicker).MinDate
        dtpTo.MaximumDate = (New DateTimePicker).MaxDate

        lblTitle.Text = ""
        strSql = "  SELECT METALNAME FROM " & cnAdminDb & "..METALMAST AS M"
        strSql += "  WHERE EXISTS (sELECT 1 FROM " & cnAdminDb & "..RATEMAST WHERE METALID = M.METALID)"
        strSql += "  AND ACTIVE = 'Y'"
        strSql += "  ORDER BY DISPLAYORDER,METALNAME"
        objGPack.FillCombo(strSql, cmbMetalType)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        lblTitle.Text = ""
        gridView.DataSource = Nothing
        Dim metalId As String = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetalType.Text & "'")
        strSql = "  DECLARE @QRY NVARCHAR(4000)" + vbCrLf
        strSql += "  DECLARE @PURITY VARCHAR(15)" + vbCrLf
        strSql += "  SELECT @QRY = ' SELECT DISTINCT RDATE'" + vbCrLf
        strSql += "  DECLARE CUR CURSOR" + vbCrLf
        strSql += "  FOR SELECT DISTINCT PURITY FROM " & cnAdminDb & "..RATEMAST WHERE METALID = '" & metalId & "' AND PURITY <> 0 ORDER BY PURITY DESC" + vbCrLf
        strSql += "  OPEN CUR" + vbCrLf
        strSql += "  WHILE 1=1" + vbCrLf
        strSql += "  BEGIN" + vbCrLf
        strSql += "  FETCH NEXT FROM CUR INTO @PURITY" + vbCrLf
        strSql += "  IF @@FETCH_STATUS = -1 BREAK" + vbCrLf
        strSql += "  SELECT @QRY = @QRY + ' ,(SELECT TOP 1 SRATE FROM " & cnAdminDb & "..RATEMAST WHERE METALID = T.METALID AND RDATE = T.RDATE AND RATEGROUP = T.RATEGROUP AND PURITY = ' + @PURITY + ' ORDER BY RATEGROUP DESC)[' + @PURITY + ']'" + vbCrLf
        strSql += "  END" + vbCrLf
        strSql += "  CLOSE CUR" + vbCrLf
        strSql += "  DEALLOCATE CUR" + vbCrLf
        strSql += "  SELECT @QRY = @QRY + ' ,RTRIM(LTRIM(SUBSTRING(CONVERT(VARCHAR,UPTIME,0),12,LEN(UPTIME))))UPTIME'"
        strSql += "  SELECT @QRY = @QRY + ' ,RATEGROUP'" + vbCrLf
        strSql += "  SELECT @QRY = @QRY + ' FROM " & cnAdminDb & "..RATEMAST T'" + vbCrLf
        strSql += "  SELECT @QRY = @QRY + ' WHERE METALID = ''" & metalId & "'''" + vbCrLf
        strSql += "  SELECT @QRY = @QRY + ' AND RDATE BETWEEN ''" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'' AND ''" & dtpTo.Value.ToString("yyyy-MM-dd") & "'''" + vbCrLf
        strSql += "  SELECT @QRY = @QRY + ' ORDER BY RDATE,RATEGROUP'" + vbCrLf
        strSql += "  EXEC SP_EXECUTESQL @QRY" + vbCrLf
        dtGrid = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        lblTitle.Text = cmbMetalType.Text + " RATE FROM " & dtpFrom.Text & " TO " & dtpTo.Text
        gridView.DataSource = dtGrid
        gridView.Columns("RATEGROUP").Visible = False
        gridView.Columns("RDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
        For cnt As Integer = 1 To gridView.ColumnCount - 1
            gridView.Columns(cnt).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Next
        gridView.Focus()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
    End Sub

    Private Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
    End Sub

End Class