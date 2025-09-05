Imports System.Data.OleDb
Public Class frmAdvanceRateFixing
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim objGridShower As frmGridDispDia
    Dim dtCompany As New DataTable
    Dim dtCostCentre As New DataTable


    Private Sub frmOutstandingRpt_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)

        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub frmOutstandingRpt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click

        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()

        dtpFrom.Focus()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click


        Dim dtGrid As New DataTable

        strSql = " SELECT  O.TRANDATE,P.PNAME AS NAME,TRANNO ,SUBSTRING(RUNNO,6,LEN(RUNNO)) AS ADVNO,AMOUNT,RATE,CONVERT(DECIMAL(15,3),(AMOUNT/RATE)) AS WEIGHT FROM " & cnAdminDb & "..OUTSTANDING O"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CUSTOMERINFO C ON C.BATCHNO =O.BATCHNO "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..PERSONALINFO P ON P.SNO =C.PSNO "
        strSql += vbCrLf + " WHERE O.TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "' AND TRANTYPE='A' AND RECPAY='R'AND ISNULL(RATE,0)>0"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If

        'dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        'GridViewHeaderCreator(objGridShower.gridViewHeader)
        objGridShower.gridViewHeader.Font = New Font("VERDANA", 8, FontStyle.Bold)
        objGridShower.gridView.RowTemplate.Height = 21
        'AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "ADVANCE  RATE FIXING"
        Dim tit As String = "ADVANCE RATE FIXING FROM " & dtpFrom.Text & " TO " & dtpTo.Text
        If (chkCmbCompany.Text <> "ALL") And (chkCmbCompany.Text <> "") Then
            tit += vbCrLf & "For " & chkCmbCompany.Text
        End If
        If (chkCmbCostCentre.Text <> "ALL") And (chkCmbCostCentre.Text <> "") Then
            tit += vbCrLf & chkCmbCostCentre.Text
        End If
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = True
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        objGridShower.gridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        DataGridView_SummaryFormatting(objGridShower.gridView)
        objGridShower.Show()
        objGridShower.gridViewHeader.Visible = True
        objGridShower.pnlHeader.Size = New Size(objGridShower.pnlHeader.Size.Width, objGridShower.pnlHeader.Size.Height + 10)
        objGridShower.FormReLocation = True
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView, "PARTICULARS")
        'objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("PARTICULARS")))
        SetGridHeadColWidth(objGridShower.gridViewHeader)

    End Sub


    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            BrighttechPack.GlobalMethods.FormatGridColumns(dgv)

            .Columns("TRANDATE").Width = 120
            .Columns("NAME").Width = 150
            .Columns("NAME").HeaderText = "CUSTOMER NAME"
            .Columns("TRANNO").Width = 120
            .Columns("ADVNO").Width = 120
            .Columns("ADVNO").HeaderText = "ADVANCE NO"
            .Columns("AMOUNT").Width = 150
            .Columns("RATE").Width = 150
            .Columns("WEIGHT").Width = 150

            .Columns("WEIGHT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight


            For CNT As Integer = 9 To .ColumnCount - 1
                .Columns(CNT).Visible = False
            Next
        End With
    End Sub




    'Private Sub Prop_Sets()
    '    Dim obj As New frmOutstandingRpt_Properties
    '    GetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany)
    '    GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
    '    SetSettingsObj(obj, Me.Name, GetType(frmOutstandingRpt_Properties))
    'End Sub
    'Private Sub Prop_Gets()
    '    Dim obj As New frmOutstandingRpt_Properties
    '    GetSettingsObj(obj, Me.Name, GetType(frmOutstandingRpt_Properties))
    '    SetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany, strCompanyName)
    '    SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, cnCostName)
    'End Sub

End Class

'Public Class frmOutstandingRpt_Properties
'    Private chkCmbCompany As New List(Of String)
'    Public Property p_chkCmbCompany() As List(Of String)
'        Get
'            Return chkCmbCompany
'        End Get
'        Set(ByVal value As List(Of String))
'            chkCmbCompany = value
'        End Set
'    End Property
'    Private chkCmbCostCentre As New List(Of String)
'    Public Property p_chkCmbCostCentre() As List(Of String)
'        Get
'            Return chkCmbCostCentre
'        End Get
'        Set(ByVal value As List(Of String))
'            chkCmbCostCentre = value
'        End Set
'    End Property
'End Class