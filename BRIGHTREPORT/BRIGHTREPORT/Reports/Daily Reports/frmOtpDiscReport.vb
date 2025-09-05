Imports System.Data.OleDb
Imports System.Threading
Imports System.IO
Imports System.Net.Mail
Public Class frmOtpDiscReport
    Dim strsql As String = String.Empty
    Dim title As String = String.Empty
    Dim dtOptions As New DataTable
    Dim dtCostCentre As New DataTable
    Dim til As String = String.Empty
    Private Sub frmExceptionReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            FuncNew()
            strsql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
            strsql += " UNION ALL"
            strsql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
            strsql += " ORDER BY RESULT,COSTNAME"
            dtCostCentre = New DataTable
            da = New OleDbDataAdapter(strsql, cn)
            da.Fill(dtCostCentre)
            BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
            If strUserCentrailsed <> "Y" And cnDefaultCostId Then chkCmbCostCentre.Enabled = False
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub
    Private Sub FuncNew()
        Me.lblTitle.Text = ""
        dtpEditDate.Value = GetServerDate()
        GridView.DataSource = Nothing
        dtpEditDate.Focus()
        Prop_Gets()
    End Sub

    Private Sub FuncView()
        Dim Optionid As String = ""
        AutoResizeToolStripMenuItem.Checked = False
        title = dtpEditDate.Value.ToString("MM/dd/yyyy")
        Dim CHKCOSTID As String = GetQryStringForSp(chkCmbCostCentre.Text, cnAdminDb & "..COSTCENTRE", "COSTID", "COSTNAME", False)

        strsql = "EXEC " & cnAdminDb & "..PROC_OTP_DISCOUNT "
        If chkDate.Checked = True Then
            strsql += vbCrLf + " @FROMDATE='" & cnTranFromDate.ToString("MM/dd/yyyy") & "'"
            strsql += vbCrLf + " ,@TODATE='" & dtpEditDate.Value.ToString("MM/dd/yyyy") & "'"
        Else
            strsql += vbCrLf + " @FROMDATE='" & dtpEditDate.Value.ToString("MM/dd/yyyy") & "'"
            strsql += vbCrLf + " ,@TODATE='" & dtpTo.Value.ToString("MM/dd/yyyy") & "'"
        End If
        strsql += vbCrLf + ",@DBNAME='" & cnStockDb & "'"
        strsql += vbCrLf + ",@COSTID='" & CHKCOSTID & "'"
        Dim ds As New DataSet
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(ds)
        dt = ds.Tables(0)
        If ds.Tables(0).Rows.Count > 0 Then
            GridView.DataSource = dt
            GridStyle()
            Heading()
        Else
            GridView.DataSource = Nothing
            Me.lblTitle.Text = ""
            MsgBox("No Records Found.", MsgBoxStyle.Information)
        End If
        Prop_Sets()
    End Sub
    Private Sub GridStyle()
        With GridView
            .Columns("BATCHNO").Visible = False
            .Columns("TRANDATE").Visible = False
            FormatGridColumns(GridView, False, False, False, False)
            .Focus()
        End With
    End Sub
    Private Sub Heading()
        If chkDate.Checked = True Then
            til = Me.Text & "  As OnDate :" & Me.dtpEditDate.Value.ToString("dd-MM-yyyy")
        Else
            til = Me.Text & " Date : " & Me.dtpEditDate.Value.ToString("dd-MM-yyyy")
            til += " To " & Me.dtpTo.Value.ToString("dd-MM-yyyy")
        End If
        til += " At :" & chkCmbCostCentre.Text
        Me.lblTitle.Font = New System.Drawing.Font("VERDANA", 9, FontStyle.Bold)
        Me.lblTitle.Text = til
    End Sub
    Private Sub btnview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnview.Click
        FuncView()
    End Sub
    Private Sub btnnew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnnew.Click
        FuncNew()
    End Sub
    Private Sub GridColor()
        For Each dgvRow As DataGridViewRow In GridView.Rows
            Select Case dgvRow.Cells("RESULT").Value.ToString
                Case 1
                    dgvRow.DefaultCellStyle.ForeColor = Color.Blue
                    dgvRow.DefaultCellStyle.Font = reportHeadStyle1.Font
            End Select
        Next
    End Sub
    Private Sub frmExceptionReport_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
        Select Case e.KeyCode
            Case Keys.F3
                btnnew_Click(btnnew, Nothing)
                Exit Select
            Case Keys.F12
                btnexit_Click(btnexit, Nothing)
                Exit Select
            Case Keys.V
                btnview_Click(btnview, Nothing)
                Exit Select
            Case Keys.X
                btnExcel_Click(btnExport, Nothing)
                Exit Select
            Case Keys.P
                btnPrint_Click(btnPrint, Nothing)
                Exit Select
                Exit Select
            Case e.KeyCode = Keys.Enter
                SendKeys.Send("{TAB}")
                Exit Select
        End Select
    End Sub
    Private Sub btnexit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnexit.Click
        Me.Close()
    End Sub
    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If GridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "Exception Report-" & title, GridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub chkDate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDate.CheckedChanged
        With Me
            If chkDate.Checked = True Then
                .chkDate.Text = "&As OnDate"
                .lblEditDate.Visible = False
                .dtpTo.Visible = False
            Else
                .chkDate.Text = "&Date From"
                .lblEditDate.Visible = True
                .dtpTo.Visible = True
            End If
        End With
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If GridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, GridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub AutoResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoResizeToolStripMenuItem.Click
        If GridView.RowCount > 0 Then
            If AutoResizeToolStripMenuItem.Checked Then
                GridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                GridView.Invalidate()
                For Each dgvCol As DataGridViewColumn In GridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                GridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In GridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                GridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
        End If
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmOtpDiscReport_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmOtpDiscReport_Properties))
        chkDate.Checked = obj.p_chkDate
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmOtpDiscReport_Properties
        obj.p_chkDate = chkDate.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmOtpDiscReport_Properties))
    End Sub

    Private Sub GridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles GridView.KeyPress
        If UCase(e.KeyChar) = Chr(Keys.Escape) Then
            dtpEditDate.Focus()
        End If
    End Sub
End Class

Public Class frmOtpDiscReport_Properties
    Private chkDate As Boolean = True
    Public Property p_chkDate() As Boolean
        Get
            Return chkDate
        End Get
        Set(ByVal value As Boolean)
            chkDate = value
        End Set
    End Property
    Private RbtFormat2 As Boolean = True
    Public Property p_RbtFormat2() As Boolean
        Get
            Return RbtFormat2
        End Get
        Set(ByVal value As Boolean)
            RbtFormat2 = value
        End Set
    End Property

    Private RbtFormat1 As Boolean = True
    Public Property p_RbtFormat1() As Boolean
        Get
            Return RbtFormat1
        End Get
        Set(ByVal value As Boolean)
            RbtFormat1 = value
        End Set
    End Property
    Private cmbOptions As String = "ALL"
    Public Property p_cmbOptions() As String
        Get
            Return cmbOptions
        End Get
        Set(ByVal value As String)
            cmbOptions = value
        End Set
    End Property
    Private chkWithDisc As Boolean = False
    Public Property p_chkWithDisc() As Boolean
        Get
            Return chkWithDisc
        End Get
        Set(ByVal value As Boolean)
            chkWithDisc = value
        End Set
    End Property

End Class