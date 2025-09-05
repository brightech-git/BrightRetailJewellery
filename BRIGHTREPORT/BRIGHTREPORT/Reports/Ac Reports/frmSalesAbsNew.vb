Imports System.Data.OleDb
Imports System.Threading
Imports System.IO
Public Class frmSalesAbsNew
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim dsGridView As New DataSet
    Dim strFtr As String
    Dim strFtr1 As String
    Dim strFtr2 As String
    Dim SepStnPost As String
    Dim SepDiaPost As String
    Dim SepPrePost As String
    Dim dtCostCentre As New DataTable

    Dim strprint As String
    Dim FileWrite As StreamWriter
    Dim PgNo As Integer
    Dim line As Integer
    Dim SpecificPrint As Boolean = False

    Function funcGetValues(ByVal field As String, ByVal def As String) As String
        strSql = " Select ctlText from " & cnAdminDb & "..SoftControl"
        strSql += vbCrLf + " where ctlId = '" & field & "'"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Return dt.Rows(0).Item("ctlText").ToString
        End If
        Return def
    End Function

    Private Sub SalesAbs()
        Try

            gridView.DataSource = Nothing
            Me.Refresh()
            Dim Sysid As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))

            strSql = " EXEC " & cnAdminDb & "..SP_RPT_SALESABSTRACT_NEW"
            strSql += vbCrLf + " @DBNAME ='" & cnStockDb & "'"
            strSql += vbCrLf + " ,@FROMDATE ='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE ='" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@SYSTEMID ='" & Sysid & "'"
            strSql += vbCrLf + " ,@COMPANYID ='" & strCompanyId & "'"
            strSql += vbCrLf + " ,@COSTID ='" & GetSelectedCostId(chkCmbCostCentre, False) & "'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            da = New OleDbDataAdapter(cmd)
            Dim DtGr As New DataTable
            Dim dss As New DataSet
            da.Fill(dss)
            DtGr = dss.Tables(0)
            Dim DtGrid As New DataTable
            DtGrid.Columns.Add("KEYNO", GetType(Integer))
            DtGrid.Columns("KEYNO").AutoIncrement = True
            DtGrid.Columns("KEYNO").AutoIncrementSeed = 0
            DtGrid.Columns("KEYNO").AutoIncrementStep = 1
            DtGrid.Merge(DtGr)
            If Not DtGrid.Rows.Count > 1 Then
                MsgBox("Record Not Found", MsgBoxStyle.Information)
                Exit Sub
            End If
            dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
            gridView.DataSource = dtGrid
            With gridView
                .Columns("KEYNO").Visible = False
                .Columns("COLHEAD").Visible = False
                .Columns("RESULT").Visible = False
                .Columns("TRANDATE").Width = 80
                .Columns("TRANDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
                .Columns("TRANNO").Width = 60
                .Columns("TRANNO").HeaderText = "BILLNO"
                .Columns("PARTICULAR").Width = 150
                .Columns("GRSWT").Width = 80
                .Columns("GRSWT").Visible = False
                .Columns("NETWT").Width = 80
                .Columns("AMOUNT").Width = 100
                .Columns("AMOUNT").HeaderText = "ORNAMENT VALUE"
                .Columns("VADDED").Width = 80
                .Columns("VADDED").HeaderText = "VALUE ADD"
                .Columns("TOTAL").Width = 100
                .Columns("DISCOUNT").Width = 80
                .Columns("TAX").Width = 80
                .Columns("TOTAMT").Width = 100
                .Columns("TOTAMT").HeaderText = "TOTAL AMOUNT"
                .Columns("NAME").Width = 120
                .Columns("NAME").HeaderText = "CUSTOMER"
                .Columns("ADDRESS").Width = 150
                .Columns("PHONENO").Width = 100
                .Columns("PHONENO").HeaderText = "PHONE"
            End With
            FormatGridColumns(gridView, False, False, True, False)
            FillGridGroupStyle_KeyNoWise(gridView)

            Dim TITLE As String
            TITLE += " SALES ABSTRACT FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
            TITLE += "  AT " & chkCmbCostCentre.Text & ""
            lblTitle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            lblTitle.Text = TITLE
            pnlHeading.Visible = True
            btnView_Search.Enabled = True
            Prop_Sets()
            gridView.Focus()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information) : Exit Sub
        End Try
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        pnlHeading.Visible = False
        SalesAbs()
    End Sub

    Private Sub frmSalesAbsNew_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmSalesAbsNew_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE WHERE  ISNULL(COMPANYID,'" & strCompanyId & "')LIKE'%" & strCompanyId & "%'"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExtToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        SepStnPost = funcGetValues("SEPSTNPOST", "N")
        SepDiaPost = funcGetValues("SEPDIAPOST", "N")
        SepPrePost = funcGetValues("SEPPREPOST", "N")

        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        pnlHeading.Visible = False
        gridView.DataSource = Nothing
        btnView_Search.Enabled = True
        Prop_Sets()
        dtpFrom.Focus()
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Function GridViewFormat() As Integer
        For Each dgvView As DataGridViewRow In gridView.Rows
            With dgvView
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "S"
                        .DefaultCellStyle.Font = reportSubTotalStyle.Font
                        .DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
                    Case "T"
                        .Cells("PARTICULAR").Style.BackColor = reportHeadStyle.BackColor
                        .Cells("PARTICULAR").Style.Font = reportHeadStyle.Font
                    Case "G"
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                End Select
            End With
        Next
    End Function
    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmSalesAbsNew_Properties
        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        SetSettingsObj(obj, Me.Name, GetType(frmSalesAbsNew_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmSalesAbsNew_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmSalesAbsNew_Properties))
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, "ALL")
    End Sub

End Class

Public Class frmSalesAbsNew_Properties
    Private chkCmbCostCentre As New List(Of String)
    Public Property p_chkCmbCostCentre() As List(Of String)
        Get
            Return chkCmbCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCostCentre = value
        End Set
    End Property
End Class