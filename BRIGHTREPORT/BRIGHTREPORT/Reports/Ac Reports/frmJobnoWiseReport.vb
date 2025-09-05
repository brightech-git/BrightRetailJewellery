Imports System.Data.OleDb
Public Class frmJobnoWiseReport
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim dtGridView As New DataTable
    Dim dsGridView As New DataSet
    Dim dtCostCentre As New DataTable
    Dim objGridShower As frmGridDispDia
    Dim JobWiseNew As Boolean = IIf(GetAdmindbSoftValue("REP-JOBWISEREPORT", "N") = "Y", True, False)
    Private Function FillAcname()
        strSql = " SELECT 'ALL' ACNAME,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ACNAME,2 RESULT FROM " & cnAdminDb & "..ACHEAD WHERE"
        strSql += "  ISNULL(ACTIVE,'Y') <>'N' "
        strSql += "  AND ACTYPE IN ('D','G','I','O')"
        strSql += " ORDER BY RESULT,ACNAME"
        Dim DtAcname As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(DtAcname)
        CmbAcname.Items.Clear()
        BrighttechPack.FillCombo(CmbAcname, DtAcname, "ACNAME", True)
    End Function
    Private Sub frmJobnoWiseReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        pnlContainer.Location = New Point((ScreenWid - pnlContainer.Width) / 2, ((ScreenHit - 128) - pnlContainer.Height) / 2)
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        FillAcname()
        If JobWiseNew Then
            ChkPendingOnly.Visible = True
        Else
            ChkPendingOnly.Visible = False
        End If
        Prop_Gets()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        dtpFrom.Select()
        If JobWiseNew Then
            ChkPendingOnly.Visible = True
        Else
            ChkPendingOnly.Visible = False
        End If
    End Sub

    Private Sub frmJobnoWiseReport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New frmJobnoWiseReport_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmJobnoWiseReport_Properties))
        ChkLotDet.Checked = obj.p_chkLotDet
        ChkPendingOnly.Checked = obj.p_chkPending
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkLstCostCentre, cnCostName)
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New frmJobnoWiseReport_Properties
        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkLstCostCentre)
        obj.p_chkLotDet = ChkLotDet.Checked
        obj.p_chkPending = ChkPendingOnly.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmJobnoWiseReport_Properties))
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If objGPack.Validator_Check(Me) Then Exit Sub
        Dim CostId As String
        If chkCmbCostCentre.Text <> "ALL" Then
            CostId = GetSelectedCostId(chkCmbCostCentre, False)
        Else
            CostId = "ALL"
        End If
        Dim _Accode As String
        If CmbAcname.Text <> "ALL" And CmbAcname.Text <> "" Then
            strSql = "SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & CmbAcname.Text & "'"
            _Accode = objGPack.GetSqlValue(strSql, "ACCODE", "").ToString
        Else
            _Accode = ""
        End If
        strSql = "EXEC " & cnAdminDb & "..SP_RPT_JOBNUMBER_WISE"
        strSql += vbCrLf + " @TEMPTABLE='TEMP" & systemId & "JOBNOWISE'"
        strSql += vbCrLf + ",@DBNAME='" & cnStockDb & "'"
        strSql += vbCrLf + ",@FROMDATE ='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + ",@TODATE='" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + ",@COMPANYIDS='" & strCompanyId & "'"
        strSql += vbCrLf + ",@COSTIDS ='" & CostId & "'"
        If JobWiseNew Then
            strSql += vbCrLf + ",@FORMAT ='Y'"
            strSql += vbCrLf + IIf(ChkPendingOnly.Checked = True, ",@PENDING ='Y'", ",@PENDING ='N'")
        Else
            strSql += vbCrLf + ",@FORMAT ='N'"
            strSql += vbCrLf + ",@PENDING ='N'"
        End If
        strSql += vbCrLf + ",@ACCODE='" & _Accode & "'"
        strSql += vbCrLf + ",@JOBNO='" & txtJobNo.Text & "'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)
        Dim dss As New DataSet
        da.Fill(dss)
        Dim dtGrid As New DataTable
        If dss.Tables.Contains("Table") Then
            dtGrid = dss.Tables(0)
        End If

        dss.Tables.Clear()

        If dtGrid.Rows.Count = 0 Then
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            Exit Sub
        End If
        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        With dtMergeHeader
            If JobWiseNew Then
                .Columns.Add("PARTICULAR~DATE~SMITH~ITEM", GetType(String))
            Else
                .Columns.Add("PARTICULAR~TRANNO~TRANDATE~ACNAME", GetType(String))
            End If
            .Columns.Add("RPCS~RGRSWT~RNETWT~RSTNPCS~RSTNWT~RDIAPCS~RDIAWT~RMC~RWAST", GetType(String))
            .Columns.Add("IPCS~IGRSWT~INETWT~ISTNPCS~ISTNWT~IDIAPCS~IDIAWT~IMC~IWAST", GetType(String))
            .Columns.Add("LPCS~LGRSWT~LNETWT", GetType(String))
            If JobWiseNew Then
                .Columns.Add("CPCS~CGRSWT~CNETWT~CSTNPCS~CSTNWT~CDIAPCS~CDIAWT", GetType(String))
            End If
            .Columns.Add("SCROLL", GetType(String))
            If JobWiseNew Then
                .Columns("PARTICULAR~DATE~SMITH~ITEM").Caption = "RECEIPT"
            Else
                .Columns("PARTICULAR~TRANNO~TRANDATE~ACNAME").Caption = "RECEIPT"
            End If
            .Columns("RPCS~RGRSWT~RNETWT~RSTNPCS~RSTNWT~RDIAPCS~RDIAWT~RMC~RWAST").Caption = "RECEIPT"
            .Columns("IPCS~IGRSWT~INETWT~ISTNPCS~ISTNWT~IDIAPCS~IDIAWT~IMC~IWAST").Caption = "ISSUE"
            .Columns("LPCS~LGRSWT~LNETWT").Caption = "LOT"
            If JobWiseNew Then
                .Columns("CPCS~CGRSWT~CNETWT~CSTNPCS~CSTNWT~CDIAPCS~CDIAWT").Caption = "PENDING"
            End If
            .Columns("SCROLL").Caption = ""
        End With
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "JOBNO WISE REPORT"
        Dim TITLE As String
        TITLE += " JOBNO WISE REPORT FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
        objGridShower.lblTitle.Text = TITLE

        AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.dsGrid.Tables.Add(dtMergeHeader)

        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.gridViewHeader.DataSource = objGridShower.dsGrid.Tables(1)

        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        ' DataGridView_SummaryFormatting(objGridShower.gridView)
        FormatGridColumns(objGridShower.gridView, False, False, , False)

        objGridShower.Show()
        objGridShower.WindowState = FormWindowState.Maximized
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        objGridShower.gridViewHeader.Visible = True
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        'objGridShower.gridView.Columns("ACNAME").Visible = False
        With objGridShower.gridView
            .Columns("COLHEAD").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("KEYNO").Visible = False
            .Columns("JOBNO").Visible = False
            If .Columns.Contains("ACNAME") Then .Columns("ACNAME").HeaderText = "NAME"
            If .Columns.Contains("BATCHNO") Then .Columns("BATCHNO").Visible = False
            .Columns("RPCS").HeaderText = "PCS"
            .Columns("RGRSWT").HeaderText = "GRSWT"
            .Columns("RNETWT").HeaderText = "NETWT"
            .Columns("RSTNPCS").HeaderText = "STNPCS"
            .Columns("RSTNWT").HeaderText = "STNWT"
            If .Columns.Contains("RDIAPCS") Then .Columns("RDIAPCS").HeaderText = "DIAPCS"
            If .Columns.Contains("RDIAWT") Then .Columns("RDIAWT").HeaderText = "DIAWT"
            If .Columns.Contains("RMC") Then .Columns("RMC").HeaderText = "MC"
            If .Columns.Contains("RWAST") Then .Columns("RWAST").HeaderText = "WAST"
            .Columns("IPCS").HeaderText = "PCS"
            .Columns("IGRSWT").HeaderText = "GRSWT"
            .Columns("INETWT").HeaderText = "NETWT"
            .Columns("ISTNPCS").HeaderText = "STNPCS"
            .Columns("ISTNWT").HeaderText = "STNWT"
            If .Columns.Contains("IDIAPCS") Then .Columns("IDIAPCS").HeaderText = "DIAPCS"
            If .Columns.Contains("IDIAWT") Then .Columns("IDIAWT").HeaderText = "DIAWT"
            If .Columns.Contains("IMC") Then .Columns("IMC").HeaderText = "MC"
            If .Columns.Contains("IWAST") Then .Columns("IWAST").HeaderText = "WAST"
            If .Columns.Contains("CDIAPCS") Then .Columns("CDIAPCS").HeaderText = "DIAPCS"
            If .Columns.Contains("CDIAWT") Then .Columns("CDIAWT").HeaderText = "DIAWT"
            If .Columns.Contains("CSTNPCS") Then .Columns("CSTNPCS").HeaderText = "STNPCS"
            If .Columns.Contains("CSTNWT") Then .Columns("CSTNWT").HeaderText = "STNWT"
            .Columns("LPCS").HeaderText = "PCS"
            .Columns("LGRSWT").HeaderText = "GRSWT"
            .Columns("LNETWT").HeaderText = "NETWT"

            .Columns("RPCS").Width = 50
            .Columns("RGRSWT").Width = 70
            .Columns("RNETWT").Width = 70
            .Columns("RSTNPCS").Width = 60
            .Columns("RSTNWT").Width = 60
            .Columns("IPCS").Width = 50
            .Columns("IGRSWT").Width = 70
            .Columns("INETWT").Width = 70
            .Columns("ISTNPCS").Width = 60
            .Columns("ISTNWT").Width = 60
            .Columns("LPCS").Width = 50
            .Columns("LGRSWT").Width = 70
            .Columns("LNETWT").Width = 70
            If JobWiseNew Then
                .Columns("CPCS").HeaderText = "PCS"
                .Columns("CGRSWT").HeaderText = "GRSWT"
                .Columns("CNETWT").HeaderText = "NETWT"
                .Columns("CPCS").Width = 50
                .Columns("CGRSWT").Width = 70
                .Columns("CNETWT").Width = 70
            End If

            '.Columns("RPCS").DefaultCellStyle.BackColor = Color.LightYellow
            '.Columns("RGRSWT").DefaultCellStyle.BackColor = Color.LightYellow
            '.Columns("RNETWT").DefaultCellStyle.BackColor = Color.LightYellow
            '.Columns("RSTNPCS").DefaultCellStyle.BackColor = Color.LightYellow
            '.Columns("RSTNWT").DefaultCellStyle.BackColor = Color.LightYellow

            .Columns("IPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("IGRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("INETWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ISTNPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("ISTNWT").DefaultCellStyle.BackColor = Color.AliceBlue

            .Columns("LPCS").DefaultCellStyle.BackColor = Color.LavenderBlush
            .Columns("LGRSWT").DefaultCellStyle.BackColor = Color.LavenderBlush
            .Columns("LNETWT").DefaultCellStyle.BackColor = Color.LavenderBlush
            If JobWiseNew Then
                .Columns("CPCS").DefaultCellStyle.BackColor = Color.AliceBlue
                .Columns("CGRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
                .Columns("CNETWT").DefaultCellStyle.BackColor = Color.AliceBlue
            End If
            If ChkLotDet.Checked Then
                .Columns("LPCS").Visible = True
                .Columns("LGRSWT").Visible = True
                .Columns("LNETWT").Visible = True
                .Columns("CPCS").Visible = True
                .Columns("CGRSWT").Visible = True
                .Columns("CNETWT").Visible = True
                If .Columns.Contains("CDIAWT") Then .Columns("CDIAWT").Visible = True
                If .Columns.Contains("CSTNPCS") Then .Columns("CSTNPCS").Visible = True
                If .Columns.Contains("CSTNWT") Then .Columns("CSTNWT").Visible = True
            Else
                .Columns("LPCS").Visible = False
                .Columns("LGRSWT").Visible = False
                .Columns("LNETWT").Visible = False
                .Columns("CPCS").Visible = False
                .Columns("CGRSWT").Visible = False
                .Columns("CNETWT").Visible = False
                If .Columns.Contains("CDIAWT") Then .Columns("CDIAWT").Visible = False
                If .Columns.Contains("CSTNPCS") Then .Columns("CSTNPCS").Visible = False
                If .Columns.Contains("CSTNWT") Then .Columns("CSTNWT").Visible = False
            End If
        End With
        If ChkPendingOnly.Checked And JobWiseNew Then
            For i As Integer = 5 To objGridShower.gridView.ColumnCount - 1
                objGridShower.gridView.Columns(i).Visible = False
            Next
            With objGridShower.gridView
                .Columns("CPCS").Visible = True
                .Columns("CGRSWT").Visible = True
                .Columns("CNETWT").Visible = True
            End With
        End If
        With objGridShower.gridView
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            .Invalidate()
            For Each dgvCol As DataGridViewColumn In .Columns
                dgvCol.Width = dgvCol.Width
            Next
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End With
        GridHead()
        Prop_Sets()
    End Sub
    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs)
        SetGridHeadColWidth(CType(sender, DataGridView))
    End Sub
    Private Sub GridHead()
        With objGridShower.gridView
            objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle = .ColumnHeadersDefaultCellStyle
            objGridShower.gridViewHeader.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised
            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken
            Dim TEMPCOLWIDTH As Integer = 0
            If JobWiseNew Then
                TEMPCOLWIDTH = .Columns("PARTICULAR").Width + .Columns("DATE").Width + .Columns("SMITH").Width + .Columns("ITEM").Width
            Else
                TEMPCOLWIDTH = .Columns("TRANNO").Width + .Columns("TRANDATE").Width + .Columns("ACNAME").Width + .Columns("PARTICULAR").Width
            End If
            objGridShower.gridViewHeader.Columns(0).Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns(0).HeaderText = ""
            If JobWiseNew Then
                TEMPCOLWIDTH = .Columns("RPCS").Width + .Columns("RGRSWT").Width + .Columns("RNETWT").Width + .Columns("RSTNPCS").Width + .Columns("RSTNWT").Width _
                            + .Columns("RDIAPCS").Width + .Columns("RDIAWT").Width
            Else
                TEMPCOLWIDTH = .Columns("RPCS").Width + .Columns("RGRSWT").Width + .Columns("RNETWT").Width + .Columns("RSTNPCS").Width + .Columns("RSTNWT").Width _
                                            + .Columns("RDIAPCS").Width + .Columns("RDIAWT").Width + .Columns("RMC").Width + .Columns("RWAST").Width
            End If
            objGridShower.gridViewHeader.Columns(1).Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns(1).HeaderText = "RECEIPT"
            If JobWiseNew Then
                TEMPCOLWIDTH = .Columns("IPCS").Width + .Columns("IGRSWT").Width + .Columns("INETWT").Width + .Columns("ISTNPCS").Width + .Columns("ISTNWT").Width _
                            + .Columns("IDIAPCS").Width + .Columns("IDIAWT").Width
            Else
                TEMPCOLWIDTH = .Columns("IPCS").Width + .Columns("IGRSWT").Width + .Columns("INETWT").Width + .Columns("ISTNPCS").Width + .Columns("ISTNWT").Width _
                                            + .Columns("IDIAPCS").Width + .Columns("IDIAWT").Width + .Columns("IMC").Width + .Columns("IWAST").Width
            End If
            objGridShower.gridViewHeader.Columns(2).Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns(2).HeaderText = "ISSUE"
            TEMPCOLWIDTH = .Columns("LPCS").Width + .Columns("LGRSWT").Width + .Columns("LNETWT").Width
            objGridShower.gridViewHeader.Columns(3).Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns(3).HeaderText = "LOT"
            objGridShower.gridViewHeader.Columns("SCROLL").Visible = False
            objGridShower.gridViewHeader.Columns(3).Visible = ChkLotDet.Checked
            If JobWiseNew Then
                TEMPCOLWIDTH = .Columns("CPCS").Width + .Columns("CGRSWT").Width + .Columns("CNETWT").Width _
                        + .Columns("CSTNPCS").Width + .Columns("CSTNWT").Width _
                        + .Columns("CDIAPCS").Width + .Columns("CDIAWT").Width
                objGridShower.gridViewHeader.Columns(4).Width = TEMPCOLWIDTH
                objGridShower.gridViewHeader.Columns(4).HeaderText = "PENDING"
                objGridShower.gridViewHeader.Columns(4).Visible = True
                If ChkPendingOnly.Checked Then
                    objGridShower.gridViewHeader.Columns(1).Visible = False
                    objGridShower.gridViewHeader.Columns(2).Visible = False
                    objGridShower.gridViewHeader.Columns(3).Visible = False
                End If
            End If
            objGridShower.gridViewHeader.Columns(4).Visible = ChkLotDet.Checked
        End With
    End Sub
End Class
Public Class frmJobnoWiseReport_Properties

    Private chkLstCostCentre As New List(Of String)
    Public Property p_chkLstCostCentre() As List(Of String)
        Get
            Return chkLstCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkLstCostCentre = value
        End Set
    End Property
    Private chkLotDet As Boolean
    Public Property p_chkLotDet() As Boolean
        Get
            Return chkLotDet
        End Get
        Set(ByVal value As Boolean)
            chkLotDet = value
        End Set
    End Property
    Private chkPending As Boolean
    Public Property p_chkPending() As Boolean
        Get
            Return chkPending
        End Get
        Set(ByVal value As Boolean)
            chkPending = value
        End Set
    End Property
End Class