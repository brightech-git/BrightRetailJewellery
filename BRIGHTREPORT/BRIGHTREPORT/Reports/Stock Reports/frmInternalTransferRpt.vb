Imports System.Data.OleDb
Imports System.Threading

Public Class frmInternalTransferRpt
    Dim strsql As String = String.Empty
    Dim title As String = String.Empty
    Dim dtMetalName As New DataTable
    Dim dsGridView As New DataSet
    Dim dtCostCentre As New DataTable
    Dim til As String = String.Empty
    Dim Chkcostid As String = String.Empty
    Dim ds As New DataSet
    Dim cmd As New OleDbCommand
    Private Sub FuncCostCenterLoad()
        strsql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strsql += " UNION ALL"
        strsql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strsql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentrefrom, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId Then chkCmbCostCentrefrom.Enabled = False
        BrighttechPack.GlobalMethods.FillCombo(chkCostCentreTo, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId Then chkCostCentreTo.Enabled = False
    End Sub
    Private Sub FuncNew()
        With Me
            .lblHeading.Text = ""
            .dtpFrmDate.Value = GetServerDate()
            .dtpToDate.Value = GetServerDate()
            .dGrid.DataSource = Nothing
            .dtpFrmDate.Focus()
            strsql = " SELECT 'ALL' METALNAME,'ALL' METALID,1 RESULT "
            strsql += "  UNION ALL"
            strsql += "  SELECT METALNAME,CONVERT(vARCHAR,METALID),2 RESULT FROM " & cnAdminDb & "..METALMAST ORDER BY RESULT,METALNAME"
            dtMetalName = New DataTable
            da = New OleDbDataAdapter(strsql, cn)
            da.Fill(dtMetalName)
            BrighttechPack.GlobalMethods.FillCombo(cmbMetal, dtMetalName, "METALNAME", )
            dGrid.DataSource = Nothing
            cmbMetal.SelectedIndex = 0
            dtpFrmDate.Focus()
        End With
    End Sub
    Private Sub btnview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnview.Click
        dGrid.DataSource = Nothing
        lblHeading.Text = ""
        lblStatus.Text = ""
        Me.Refresh()
        Dim TEMPTABLE As String = "TEMPTABLEDB..TEMP" & Trim(Guid.NewGuid.ToString.Substring(0, 8)) & "INTERNAL"
        strsql = "IF OBJECT_ID('" & TEMPTABLE & "') IS NOT NULL DROP TABLE " & TEMPTABLE
        cmd = New OleDbCommand(strsql, cn) : cmd.ExecuteNonQuery()

        Dim chkfrmCostId As String = "ALL"
        If chkCmbCostCentrefrom.Text <> "ALL" And chkCmbCostCentrefrom.Text <> "" Then
            chkfrmCostId = GetSelectedCostId(chkCmbCostCentrefrom, True)
        End If

        Dim chktoCostId As String = "ALL"
        If chkCostCentreTo.Text <> "ALL" And chkCostCentreTo.Text <> "" Then
            chktoCostId = GetSelectedCostId(chkCostCentreTo, True)
        End If

        Dim Refno As String = ""
        'If Me.txtRefNo.Text <> "" Then
        '    Dim Rno As String = String.Empty
        '    Rno = "  SELECT DISTINCT I.REFNO FROM " & cnStockDb & "..ISSUE AS I "
        '    Rno += " LEFT JOIN " & cnStockDb & "..RECEIPT AS R "
        '    Rno += "  ON  I.METALID = R.METALID"
        '    Rno += " LEFT JOIN " & cnAdminDb & "..PITEMTAG AS P ON  "
        '    Rno += " I.METALID = P.TRANINVNO "
        '    Rno += " WHERE I.REFNO ='" & Me.txtRefNo.Text & "' AND "
        '    Rno += " I.TRANTYPE = 'IIN' AND "
        '    Rno += " R.TRANTYPE = 'RIN' AND ISNULL(I.REFNO,'') <>'' "
        '    Rno += " AND  ISNULL(R.REFNO,'') <>''"
        '    Rno += " AND I.TRANDATE BETWEEN '" & dtpFrmDate.Value.ToString("yyyy/MM/dd") & "'"
        '    Rno += " AND '" & dtpToDate.Value.ToString("yyyy/MM/dd") & "'"
        '    Refno = objGPack.GetSqlValue(Rno, , , )
        'End If
        Dim MetalId As String = "ALL"
        If cmbMetal.Text <> "ALL" And cmbMetal.Text <> "" Then
            MetalId = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME ='" & cmbMetal.Text & "'", , , )
        End If
        strsql = "EXEC " & cnAdminDb & "..SP_RPT_INTERNALTRANSFER"
        strsql += vbCrLf + "   @DBNAME='" & cnStockDb & "'"
        strsql += vbCrLf + "  ,@FROMDATE='" & dtpFrmDate.Value.ToString("yyyy/MM/dd") & "'"
        strsql += vbCrLf + "  ,@TODATE='" & dtpToDate.Value.ToString("yyyy/MM/dd") & "'"
        strsql += vbCrLf + "  ,@FRMCOSTID = """ & chkfrmCostId & """"
        strsql += vbCrLf + "  ,@TOCOSTID = """ & chktoCostId & """"
        strsql += vbCrLf + "  ,@METAL = """ & MetalId & """"
        strsql += vbCrLf + "  ,@SYSTEMID = '" & systemId & "'"
        strsql += vbCrLf + "  ,@REFNO = '" & Me.txtRefNo.Text.Trim & "'"
        dsGridView = New DataSet
        cmd = New OleDbCommand(strsql, cn)
        da = New OleDbDataAdapter(cmd)
        da.Fill(dsGridView)
        If dsGridView.Tables(0).Rows.Count > 0 Then
            dGrid.DataSource = dsGridView.Tables(0)
            dGrid.Rows(dGrid.Rows.Count - 1).Cells("METALNAME").Value = ""
            funcGridStyle()
            FillGridGroupStyle_KeyNoWise(dGrid)

            Dim TITLE As String = String.Empty
            TITLE = " INTERNAL TRANSFER REPORT  " & dtpFrmDate.Text & " TO " & dtpToDate.Text & "   COST CENTRE FROM"
            TITLE += IIf(chkCmbCostCentrefrom.Text <> "" And chkCmbCostCentrefrom.Text <> "ALL", " : " & chkCmbCostCentrefrom.Text, "")
            TITLE += IIf(chkCostCentreTo.Text <> "" And chkCostCentreTo.Text <> "ALL", " TO " & chkCostCentreTo.Text, "")
            lblHeading.Font = New System.Drawing.Font("VERDANA", 9, FontStyle.Bold)
            lblHeading.Text = TITLE
        Else
            dGrid.DataSource = Nothing
            gridHead.DataSource = Nothing
            MsgBox("No records found.", MsgBoxStyle.Information, "Information")
            lblHeading.Text = ""
        End If
    End Sub
    
    Function FuncGridHead()
        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        With dtMergeHeader
            .Columns.Add("PARTICULAR", GetType(String))
            .Columns.Add("ITRANDATE~ITRANNO~COSTNAME~IPCS~IGRSWT~ISTNPCS~ISTNWT~IDIAPCS~IDIAWT~INETWT" & IIf(chkWithPurchaseValue.Checked, "~IPURVALUE", "") & IIf(chkWithNoofTags.Checked, "~NOOFTAGS", ""), GetType(String))
            .Columns.Add("RTRANDATE~RTRANNO~RCOSTNAME~RPCS~RGRSWT~RNETWT~RSTNPCS~RSTNWT~RDIAPCS~RDIAWT", GetType(String))
            .Columns.Add("PPCS~PGRSWT~PNETWT~PSTNPCS~PSTNWT~PDIAPCS~PDIAWT", GetType(String))
            .Columns.Add("DPCS~DGRSWT~DNETWT", GetType(String))
            .Columns.Add("SCROLL", GetType(String))
            .Columns("PARTICULAR").Caption = "PARTICULAR"
            .Columns("ITRANDATE~ITRANNO~COSTNAME~IPCS~IGRSWT~ISTNPCS~ISTNWT~IDIAPCS~IDIAWT~INETWT" & IIf(chkWithPurchaseValue.Checked, "~IPURVALUE", "") & IIf(chkWithNoofTags.Checked, "~NOOFTAGS", "")).Caption = "ISSUE"
            .Columns("RTRANDATE~RTRANNO~RCOSTNAME~RPCS~RGRSWT~RNETWT~RSTNPCS~RSTNWT~RDIAPCS~RDIAWT").Caption = "RECEIPT"
            .Columns("PPCS~PGRSWT~PNETWT~PSTNPCS~PSTNWT~PDIAPCS~PDIAWT").Caption = "PENDING"
            .Columns("DPCS~DGRSWT~DNETWT").Caption = "DIFFERENCE"
            .Columns("SCROLL").Caption = ""
        End With
        gridHead.DataSource = dtMergeHeader
    End Function
    Function funcGridStyle() As Integer
        With dGrid
            .Columns("TRANDATE").Visible = False
            .Columns("RECTRANDATE").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("REFNO").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("METALNAME").Visible = False
            .Columns("PARTICULARR").Visible = False
            .Columns("KEYNO").Visible = False
            .Columns("NOOFTAGS").Visible = chkWithNoofTags.Checked
            .Columns("IPURVALUE").Visible = chkWithPurchaseValue.Checked
            .Columns("PARTICULAR").Width = 130
            .Columns("ITRANDATE").Width = 80
            .Columns("ITRANDATE").HeaderText = "DATE"
            .Columns("ITRANNO").Width = 80
            .Columns("ITRANNO").HeaderText = "NO"
            .Columns("ITRANDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
            .Columns("RTRANDATE").Width = 80
            .Columns("RTRANNO").Width = 80
            .Columns("RTRANDATE").HeaderText = "DATE"
            .Columns("RTRANNO").HeaderText = "NO"
            .Columns("RTRANDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
            .Columns("IPCS").HeaderText = "PCS"
            .Columns("IGRSWT").HeaderText = "GRSWT"
            .Columns("ISTNPCS").HeaderText = "STNPCS"
            .Columns("ISTNWT").HeaderText = "STNWT"
            .Columns("IDIAPCS").HeaderText = "DIAPCS"
            .Columns("IDIAWT").HeaderText = "DIAWT"
            .Columns("INETWT").HeaderText = "NETWT"
            .Columns("IPURVALUE").HeaderText = "PURVALUE"
            .Columns("COSTNAME").HeaderText = "COSTNAME"
            .Columns("RPCS").HeaderText = "PCS"
            .Columns("RGRSWT").HeaderText = "GRSWT"
            .Columns("RSTNPCS").HeaderText = "STNPCS"
            .Columns("RSTNWT").HeaderText = "STNWT"
            .Columns("RDIAPCS").HeaderText = "DIAPCS"
            .Columns("RDIAWT").HeaderText = "DIAWT"
            .Columns("RNETWT").HeaderText = "NETWT"
            .Columns("RCOSTNAME").HeaderText = "COSTNAME"
            .Columns("DPCS").HeaderText = "PCS"
            .Columns("DGRSWT").HeaderText = "GRSWT"
            .Columns("DNETWT").HeaderText = "NETWT"
            .Columns("PPCS").HeaderText = "PCS"
            .Columns("PGRSWT").HeaderText = "GRSWT"
            .Columns("PSTNPCS").HeaderText = "STNPCS"
            .Columns("PSTNWT").HeaderText = "STNWT"
            .Columns("PDIAPCS").HeaderText = "DIAPCS"
            .Columns("PDIAWT").HeaderText = "DIAWT"
            .Columns("PNETWT").HeaderText = "NETWT"
            .Columns("PARTICULAR").SortMode = DataGridViewColumnSortMode.NotSortable
            Dim colhead(3) As String
            colhead(0) = "I"
            colhead(1) = "R"
            colhead(2) = "P"
            colhead(3) = "D"
            For i As Integer = 0 To colhead.Length - 1
                With .Columns(colhead(i) & "PCS")
                    .HeaderText = "PCS"
                    .Width = 50
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns(colhead(i) & "GRSWT")
                    .HeaderText = "GRSWT"
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Format = "#,##0.000"
                End With

                With .Columns(colhead(i) & "NETWT")
                    .HeaderText = "NETWT"
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Format = "#,##0.000"
                End With
            Next
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8.25!, FontStyle.Bold)
        End With
        FormatGridColumns(dGrid, False)
        funcGridHeaderStyle()
        Me.dGrid.Refresh()
    End Function
    Public Sub FormatGridColumns(ByVal grid As DataGridView, Optional ByVal colHeadVisibleSetFalse As Boolean = True, Optional ByVal colFormat As Boolean = True, Optional ByVal reeadOnly As Boolean = True, Optional ByVal sortColumns As Boolean = True)
        With grid
            If colHeadVisibleSetFalse Then .ColumnHeadersVisible = False
            For i As Integer = 0 To .ColumnCount - 1
                .Columns(i).ReadOnly = reeadOnly
                If .Columns(i).ValueType.Name Is GetType(Decimal).Name Then
                    If colFormat Then .Columns(i).DefaultCellStyle.Format = "0.000"
                    .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                ElseIf .Columns(i).ValueType.Name Is GetType(Double).Name Then
                    If colFormat Then .Columns(i).DefaultCellStyle.Format = "0.00"
                    .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                ElseIf .Columns(i).ValueType.Name Is GetType(Integer).Name Then
                    .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                ElseIf .Columns(i).ValueType.Name Is GetType(Date).Name Then
                    'If colFormat Then .Columns(i).DefaultCellStyle.Format = "dd/MM/yyyy"
                End If
                If Not sortColumns Then .Columns(i).SortMode = DataGridViewColumnSortMode.NotSortable
                '.Columns(i).Resizable = DataGridViewTriState.False 
            Next
        End With
    End Sub
    Function funcGridHeaderStyle() As Integer
        FuncGridHead()
        With gridHead
            With .Columns("PARTICULAR")
                .Width = dGrid.Columns("PARTICULAR").Width
                .HeaderText = ""
            End With

            Dim StrString As String = ""
            StrString = "ITRANDATE~ITRANNO~COSTNAME~IPCS~IGRSWT~ISTNPCS~ISTNWT~IDIAPCS~IDIAWT~INETWT"
            If chkWithPurchaseValue.Checked Then
                StrString += "~IPURVALUE"
            End If
            If chkWithNoofTags.Checked Then
                StrString += "~NOOFTAGS"
            End If

            With .Columns(StrString)
                .Width = dGrid.Columns("ITRANDATE").Width + dGrid.Columns("ITRANNO").Width + dGrid.Columns("COSTNAME").Width + dGrid.Columns("IPCS").Width _
                 + dGrid.Columns("IGRSWT").Width + dGrid.Columns("ISTNPCS").Width + dGrid.Columns("ISTNWT").Width _
                + dGrid.Columns("IDIAPCS").Width + dGrid.Columns("IDIAWT").Width + dGrid.Columns("INETWT").Width _
                + IIf(chkWithPurchaseValue.Checked, dGrid.Columns("IPURVALUE").Width, 0) + IIf(chkWithNoofTags.Checked, dGrid.Columns("NOOFTAGS").Width, 0)
                .HeaderText = "ISSUE"
            End With
            With .Columns("RTRANDATE~RTRANNO~RCOSTNAME~RPCS~RGRSWT~RNETWT~RSTNPCS~RSTNWT~RDIAPCS~RDIAWT")
                .Width = dGrid.Columns("RTRANDATE").Width + dGrid.Columns("RTRANNO").Width + dGrid.Columns("RCOSTNAME").Width + dGrid.Columns("RPCS").Width + dGrid.Columns("RGRSWT").Width + +dGrid.Columns("RNETWT").Width + dGrid.Columns("RSTNPCS").Width + dGrid.Columns("RSTNWT").Width + dGrid.Columns("RDIAPCS").Width + dGrid.Columns("RDIAWT").Width
                .HeaderText = "RECEIPT"
            End With

            With .Columns("PPCS~PGRSWT~PNETWT~PSTNPCS~PSTNWT~PDIAPCS~PDIAWT")
                .Width = dGrid.Columns("PPCS").Width + dGrid.Columns("PGRSWT").Width + dGrid.Columns("PNETWT").Width + dGrid.Columns("PSTNPCS").Width + dGrid.Columns("PSTNWT").Width + dGrid.Columns("PDIAPCS").Width + dGrid.Columns("PDIAWT").Width
                .HeaderText = "PENDING"
            End With

            With .Columns("DPCS~DGRSWT~DNETWT")
                .Width = dGrid.Columns("DPCS").Width + dGrid.Columns("DGRSWT").Width + dGrid.Columns("DNETWT").Width
                .HeaderText = "DIFFERENCE"
            End With
            With .Columns("SCROLL")
                .HeaderText = ""
                .Visible = False
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8.25!, FontStyle.Bold)
        End With
    End Function
    Private Sub btnnew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnnew.Click
        Call FuncNew()
        Call FuncCostCenterLoad()
        With Me
            lblHeading.Text = String.Empty
            .dtpFrmDate.Value = GetServerDate()
            .dtpToDate.Value = GetServerDate()
            .dGrid.DataSource = Nothing
            .gridHead.DataSource = Nothing
            .dtpFrmDate.Focus()
        End With
    End Sub
    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExcel.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If dGrid.RowCount > 0 Then
            BrightPosting.GExport.Post(Me.Name, "", lblHeading.Text, dGrid, BrightPosting.GExport.GExportType.Export, gridHead)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If dGrid.RowCount > 0 Then
            BrightPosting.GExport.Post(Me.Name, "", lblHeading.Text, dGrid, BrightPosting.GExport.GExportType.Print, gridHead)
        End If
    End Sub

    Private Sub btnexit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnexit.Click
        Me.Close()
    End Sub

    Private Sub frmInternal_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
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
                btnExcel_Click(btnExcel, Nothing)
                Exit Select
            Case Keys.P
                btnPrint_Click(btnPrint, Nothing)
                Exit Select
        End Select
    End Sub
    Private Sub RefNo()
            Dim Rno As String = String.Empty
            Rno = "  SELECT DISTINCT I.REFNO FROM " & cnStockDb & "..ISSUE AS I "
            Rno += " LEFT JOIN " & cnStockDb & "..RECEIPT AS R "
            Rno += "  ON  I.METALID = R.METALID"
            Rno += " LEFT JOIN " & cnAdminDb & "..PITEMTAG AS P ON  "
            Rno += " I.METALID = P.TRANINVNO "
            Rno += " WHERE I.REFNO ='" & Me.txtRefNo.Text & "' AND "
            Rno += " I.TRANTYPE = 'IIN' AND "
            Rno += " R.TRANTYPE = 'RIN' AND ISNULL(I.REFNO,'') <>'' "
            Rno += " AND  ISNULL(R.REFNO,'') <>''"
            Rno += " AND I.TRANDATE BETWEEN '" & dtpFrmDate.Value.ToString("yyyy/MM/dd") & "'"
            Rno += " AND '" & dtpToDate.Value.ToString("yyyy/MM/dd") & "'"
        Dim cmd As OleDb.OleDbCommand
        cmd = New OleDbCommand(strsql, cn)
        Dim dr As OleDbDataReader = cmd.ExecuteReader
        While (dr.Read())
            If dr.Read Then
                txtRefNo.AutoCompleteCustomSource.Add(dr("REFNO").ToString)
            Else
                MsgBox("Invalid RefNo.", MsgBoxStyle.Information)
            End If
        End While
        dr.Close()
    End Sub
    Private Sub frmInternal_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call FuncCostCenterLoad()
        Call FuncNew()
    End Sub
    Private Sub dGrid_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles dGrid.Scroll
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridHead.HorizontalScrollingOffset = e.NewValue
        End If
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridHead.HorizontalScrollingOffset = e.NewValue
                gridHead.Columns("SCROLL").Visible = CType(dGrid.Controls(0), HScrollBar).Visible
                gridHead.Columns("SCROLL").Width = CType(dGrid.Controls(1), VScrollBar).Width
            Else
                gridHead.Columns("SCROLL").Visible = False
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub

    Private Sub ResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResizeToolStripMenuItem.Click
        If dGrid.RowCount > 0 Then
            If ResizeToolStripMenuItem.Checked Then
                dGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                dGrid.Invalidate()
                For Each dgvCol As DataGridViewColumn In dGrid.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                dGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In dGrid.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                dGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
        End If
    End Sub
    Private Sub funcSplitbySno(ByVal RefNo As String, ByVal Trandate As Date, ByVal RecTrandate As Date, ByVal Cid As String)
        Dim ToCostId As String
        Dim ReceiptBatchno As String
        Dim IssueBatchno As String
        Dim RecTranNo As String
        Dim ChkCnt As Integer = 1
ReChk:
        strsql = " SELECT SUM(GRSWT)GRSWT FROM("
        strsql += " SELECT SUM(GRSWT)GRSWT FROM " & cnAdminDb & "..CITEMTAG WHERE TRANINVNO='" & RefNo & "'"
        strsql += " AND ISSDATE='" & Trandate & "'"
        If ChkCnt = 1 Then
            strsql += " AND COSTID=" & Cid & ""
        End If
        strsql += " UNION "
        strsql += " SELECT SUM(GRSWT)GRSWT FROM " & cnAdminDb & "..ITEMNONTAG WHERE REFNO='" & RefNo & "'"
        strsql += " AND RECDATE='" & Trandate & "' AND ISSTYPE='TR' AND RECISS='I'"
        If ChkCnt = 1 Then
            strsql += " AND COSTID=" & Cid & ""
        End If
        strsql += " )X"
        Dim TagIssWt As Decimal = Val(objGPack.GetSqlValue(strsql, "GRSWT", , tran).ToString)
        strsql = " SELECT SUM(GRSWT)GRSWT FROM " & cnStockDb & "..ISSUE "
        strsql += " WHERE TRANDATE='" & Trandate & "'"
        strsql += " AND REFNO='" & RefNo & "'"
        Dim IssWt As Decimal = Val(objGPack.GetSqlValue(strsql, "GRSWT", , tran).ToString)
        If IssWt <> TagIssWt And ChkCnt = 1 Then ChkCnt += 1 : GoTo ReChk
        If IssWt = TagIssWt And IssWt > 0 Then
            'strsql = vbCrLf + "SELECT SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT"
            'strsql += vbCrLf + ",SUM(STNPCS) AS STNPCS,SUM(STNWT) AS STNWT"
            'strsql += vbCrLf + ",SUM(STNAMT) AS STNAMT,STKTYPE,CATCODE,METALID "
            'strsql += vbCrLf + ",STNITEMID,STNSUBITEMID,STONEUNIT,STNCATCODE"
            'strsql += vbCrLf + "FROM("
            'strsql += vbCrLf + "SELECT SUM(PCS)PCS,SUM(GRSWT)GRSWT"
            'strsql += vbCrLf + ",SUM(NETWT)NETWT"
            'strsql += vbCrLf + ",CASE WHEN ISNULL(STKTYPE,'')='' THEN 'T' ELSE STKTYPE END AS STKTYPE "
            'strsql += vbCrLf + ",(SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=C.ITEMID)CATCODE"
            'strsql += vbCrLf + ",(SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=C.ITEMID)METALID"
            'strsql += vbCrLf + ",SUM(I.STNPCS) AS STNPCS"
            'strsql += vbCrLf + ",SUM(I.STNWT) AS STNWT"
            'strsql += vbCrLf + ",SUM(I.STNAMT) AS STNAMT"
            'strsql += vbCrLf + ",STNITEMID"
            'strsql += vbCrLf + ",STNSUBITEMID"
            'strsql += vbCrLf + ",STONEUNIT"
            'strsql += vbCrLf + ",(SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=I.STNITEMID)STNCATCODE"
            'strsql += vbCrLf + "FROM " & cnAdminDb & "..CITEMTAG C "
            'strsql += vbCrLf + "LEFT JOIN " & cnAdminDb & "..ITEMTAGSTONE I ON I.TAGSNO=C.SNO"
            'strsql += vbCrLf + "WHERE TRANINVNO='" & RefNo & "'"
            'strsql += vbCrLf + "AND C.ISSDATE='" & Trandate & "'"
            'strsql += vbCrLf + "GROUP BY STKTYPE,C.ITEMID,STNITEMID,STNSUBITEMID,STONEUNIT"
            'strsql += vbCrLf + ")X  GROUP BY STKTYPE,CATCODE,METALID,STNITEMID,STNSUBITEMID,STONEUNIT,STNCATCODE"

            strsql = vbCrLf + "SELECT SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT"
            strsql += vbCrLf + ",SUM(STNPCS) AS STNPCS,SUM(STNWT) AS STNWT"
            strsql += vbCrLf + ",SUM(STNAMT) AS STNAMT"
            strsql += vbCrLf + ",STKTYPE"
            strsql += vbCrLf + ",CATCODE"
            strsql += vbCrLf + ",METALID "
            strsql += vbCrLf + ",STNITEMID,STNSUBITEMID"
            strsql += vbCrLf + ",STONEUNIT"
            strsql += vbCrLf + ",STNCATCODE"
            strsql += vbCrLf + "FROM("
            strsql += vbCrLf + "SELECT SNO,SUM(PCS)PCS,SUM(GRSWT)GRSWT"
            strsql += vbCrLf + ",SUM(NETWT)NETWT"
            strsql += vbCrLf + ",CASE WHEN ISNULL(STKTYPE,'')='' THEN 'T' ELSE STKTYPE END AS STKTYPE "
            strsql += vbCrLf + ",(SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=C.ITEMID)CATCODE"
            strsql += vbCrLf + ",(SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=C.ITEMID)METALID"
            strsql += vbCrLf + ",NULL AS STNPCS"
            strsql += vbCrLf + ",NULL AS STNWT"
            strsql += vbCrLf + ",NULL AS STNAMT"
            strsql += vbCrLf + ",NULL STNITEMID"
            strsql += vbCrLf + ",NULL STNSUBITEMID"
            strsql += vbCrLf + ",NULL STONEUNIT"
            strsql += vbCrLf + ",NULL STNCATCODE"
            strsql += vbCrLf + ",'LOSE' AS TYPE"
            strsql += vbCrLf + "FROM " & cnAdminDb & "..CITEMTAG C "
            strsql += vbCrLf + "WHERE TRANINVNO='" & RefNo & "'"
            strsql += vbCrLf + "AND C.ISSDATE='" & Trandate & "'"
            strsql += vbCrLf + "AND C.COSTID=" & Cid & ""
            strsql += vbCrLf + "GROUP BY STKTYPE,C.ITEMID,SNO"
            strsql += vbCrLf + "UNION ALL"
            strsql += vbCrLf + "SELECT SNO,SUM(PCS)PCS,SUM(GRSWT)GRSWT"
            strsql += vbCrLf + ",SUM(NETWT)NETWT"
            strsql += vbCrLf + ",CASE WHEN ISNULL(STKTYPE,'')='' THEN 'T' ELSE STKTYPE END AS STKTYPE "
            strsql += vbCrLf + ",(SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=C.ITEMID)CATCODE"
            strsql += vbCrLf + ",(SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=C.ITEMID)METALID"
            strsql += vbCrLf + ",NULL AS STNPCS"
            strsql += vbCrLf + ",NULL AS STNWT"
            strsql += vbCrLf + ",NULL AS STNAMT"
            strsql += vbCrLf + ",NULL STNITEMID"
            strsql += vbCrLf + ",NULL STNSUBITEMID"
            strsql += vbCrLf + ",NULL STONEUNIT"
            strsql += vbCrLf + ",NULL STNCATCODE"
            strsql += vbCrLf + ",'LOSE' AS TYPE"
            strsql += vbCrLf + "FROM " & cnAdminDb & "..ITEMNONTAG C "
            strsql += vbCrLf + "WHERE REFNO='" & RefNo & "'"
            strsql += vbCrLf + "AND C.RECDATE='" & Trandate & "' AND ISSTYPE='TR' AND RECISS='I' "
            strsql += vbCrLf + "AND C.COSTID=" & Cid & ""
            strsql += vbCrLf + "GROUP BY STKTYPE,C.ITEMID,SNO"
            strsql += vbCrLf + "UNION ALL"
            strsql += vbCrLf + "SELECT TAGSNO,NULL PCS,NULL  GRSWT"
            strsql += vbCrLf + ",NULL NETWT"
            strsql += vbCrLf + ",(SELECT TOP 1 CASE WHEN ISNULL(STKTYPE,'')='' THEN 'T' ELSE STKTYPE END "
            strsql += vbCrLf + "FROM " & cnAdminDb & "..CITEMTAG WHERE SNO=I.TAGSNO)STKTYPE"
            strsql += vbCrLf + ",(SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=I.ITEMID)CATCODE"
            strsql += vbCrLf + ",(SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=I.ITEMID)METALID"
            strsql += vbCrLf + ",SUM(STNPCS) AS STNPCS"
            strsql += vbCrLf + ",SUM(STNWT) AS STNWT"
            strsql += vbCrLf + ",SUM(STNAMT) AS STNAMT"
            strsql += vbCrLf + ",STNITEMID"
            strsql += vbCrLf + ",STNSUBITEMID"
            strsql += vbCrLf + ",STONEUNIT"
            strsql += vbCrLf + ",(SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=I.STNITEMID)STNCATCODE"
            strsql += vbCrLf + ",'STUD' AS TYPE"
            strsql += vbCrLf + "FROM " & cnAdminDb & "..ITEMTAGSTONE I"
            strsql += vbCrLf + "WHERE TAGSNO IN(SELECT SNO FROM " & cnAdminDb & "..CITEMTAG WHERE TRANINVNO='" & RefNo & "' AND ISSDATE='" & Trandate & "' AND COSTID=" & Cid & ")"
            strsql += vbCrLf + "GROUP BY TAGSNO,STNITEMID,STNSUBITEMID,STONEUNIT,ITEMID"
            strsql += vbCrLf + ")X  GROUP BY STKTYPE,CATCODE,METALID,STNITEMID,STNSUBITEMID,STONEUNIT,STNCATCODE"

            Dim dt As New DataTable
            cmd = New OleDbCommand(strsql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(dt)
            Dim dtTag As DataTable = dt.DefaultView.ToTable(True, "CATCODE", "STKTYPE", "METALID")
            If dtTag.Rows.Count > 0 Then
                strsql = "SELECT * FROM " & cnStockDb & "..ISSUE WHERE TRANTYPE='IIN' "
                strsql += " AND TRANDATE='" & Trandate & "'"
                strsql += " AND REFNO='" & RefNo & "'"
                Dim dtIss As New DataTable
                cmd = New OleDbCommand(strsql, cn, tran)
                da = New OleDbDataAdapter(cmd)
                da.Fill(dtIss)

                strsql = "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE ACCODE='" & dtIss.Rows(0).Item("ACCODE").ToString & "'"
                ToCostId = objGPack.GetSqlValue(strsql, "COSTID", , tran).ToString
                If ToCostId = "" Then
                    strsql = "SELECT COSTID FROM  " & cnStockDb & "..RECEIPT TRANTYPE='RIN'"
                    strsql += " AND TRANDATE='" & RecTrandate & "'"
                    strsql += " AND REFNO='" & RefNo & "'"
                    strsql += " AND FLAG='X'"
                    ToCostId = objGPack.GetSqlValue(strsql, "COSTID", , tran).ToString
                End If
                strsql = "SELECT BATCHNO,TRANNO FROM  " & cnStockDb & "..RECEIPT WHERE TRANTYPE='RIN'"
                strsql += " AND TRANDATE='" & RecTrandate & "'"
                strsql += " AND REFNO='" & RefNo & "'"
                strsql += " AND FLAG='X'"
                Dim dr As DataRow = GetSqlRow(strsql, cn, tran)
                If Not dr Is Nothing Then
                    ReceiptBatchno = dr("BATCHNO").ToString
                    RecTranNo = Val(dr("TRANNO").ToString)
                End If

                IssueBatchno = dtIss.Rows(0).Item("BATCHNO").ToString
                If ReceiptBatchno = "" Then Exit Sub
                If IssueBatchno = "" Then Exit Sub

                strsql = "DELETE FROM " & cnStockDb & "..ISSUE WHERE TRANTYPE='IIN'"
                strsql += " AND BATCHNO='" & IssueBatchno & "'"
                If cnCostId = cnHOCostId Then
                    cmd = New OleDbCommand(strsql, cn, tran)
                    cmd.ExecuteNonQuery()
                Else
                    ExecQuery(SyncMode.Transaction, strsql, cn, tran, ToCostId)
                End If

                strsql = "DELETE FROM " & cnStockDb & "..ISSSTONE WHERE TRANTYPE='IIN'"
                strsql += " AND BATCHNO='" & IssueBatchno & "'"
                If cnCostId = cnHOCostId Then
                    cmd = New OleDbCommand(strsql, cn, tran)
                    cmd.ExecuteNonQuery()
                Else
                    ExecQuery(SyncMode.Transaction, strsql, cn, tran, ToCostId)
                End If

                strsql = "DELETE FROM " & cnStockDb & "..RECEIPT WHERE TRANTYPE='RIN'"
                strsql += " AND BATCHNO='" & ReceiptBatchno & "'"
                ExecQuery(SyncMode.Transaction, strsql, cn, tran, ToCostId)

                strsql = "DELETE FROM " & cnStockDb & "..RECEIPTSTONE WHERE TRANTYPE='RIN' "
                strsql += " AND BATCHNO='" & ReceiptBatchno & "'"
                ExecQuery(SyncMode.Transaction, strsql, cn, tran, ToCostId)


                For i As Integer = 0 To dtTag.Rows.Count - 1
                    Dim issSno As String
                    issSno = GetNewSno(TranSnoType.ISSUECODE, tran)
                    Dim RecSno As String
                    RecSno = GetNewSno(TranSnoType.RECEIPTCODE, tran)
                    With dtIss.Rows(0)

                        Dim CatCode As String = dtTag.Rows(i).Item("CATCODE").ToString
                        Dim Stktype As String = dtTag.Rows(i).Item("STKTYPE").ToString
                        Dim Metalid As String = dtTag.Rows(i).Item("METALID").ToString
                        Dim Pcs As Integer = Val(dt.Compute("SUM(PCS)", "CATCODE='" & CatCode & "' AND STKTYPE='" & Stktype & "' AND METALID='" & Metalid & "'").ToString)
                        Dim GrsWt As Decimal = Val(dt.Compute("SUM(GRSWT)", "CATCODE='" & CatCode & "' AND STKTYPE='" & Stktype & "' AND METALID='" & Metalid & "'").ToString)
                        Dim NetWt As Decimal = Val(dt.Compute("SUM(NETWT)", "CATCODE='" & CatCode & "' AND STKTYPE='" & Stktype & "' AND METALID='" & Metalid & "'").ToString)
                        Dim StnPcs As Integer = Val(dt.Compute("SUM(STNPCS)", "CATCODE='" & CatCode & "' AND STKTYPE='" & Stktype & "' AND METALID='" & Metalid & "'").ToString)
                        Dim Stnwt As Decimal = Val(dt.Compute("SUM(STNWT)", "CATCODE='" & CatCode & "' AND STKTYPE='" & Stktype & "' AND METALID='" & Metalid & "'").ToString)
                        Dim StnAmt As Double = Val(dt.Compute("SUM(STNAMT)", "CATCODE='" & CatCode & "' AND STKTYPE='" & Stktype & "' AND METALID='" & Metalid & "'").ToString)
                        strsql = "INSERT INTO " & cnStockDb & "..ISSUE(SNO,TRANNO,TRANDATE"
                        strsql += " ,TRANTYPE,PCS,GRSWT,NETWT,LESSWT,PUREWT,GRSNET,REFNO"
                        strsql += " ,REFDATE,COSTID,COMPANYID,FLAG"
                        strsql += " ,PURITY,CATCODE,OCATCODE,ACCODE,BATCHNO,REMARK1"
                        strsql += " ,REMARK2,USERID,UPDATED,METALID"
                        strsql += " ,STNAMT,APPVER,ORDSTATE_ID,JJFRMNO,MRKFLAG"
                        strsql += " ,STKTYPE"
                        strsql += " )"
                        strsql += " VALUES"
                        strsql += " ("
                        strsql += " '" & issSno & "'" 'SNO
                        strsql += " ," & .Item("TRANNO").ToString & "" 'TRANNO
                        strsql += " ,'" & .Item("TRANDATE").ToString & "'" 'TRANDATE
                        strsql += " ,'" & .Item("TRANTYPE").ToString & "'" 'TRANTYPE
                        strsql += " ," & Pcs & "" 'PCS
                        strsql += " ," & GrsWt & "" 'GRSWT
                        strsql += " ," & NetWt & "" 'NETWT
                        strsql += " ," & Stnwt & "" 'LESSWT
                        strsql += " ," & GrsWt - Stnwt & "" 'PUREWT
                        strsql += " ,'G'" 'GRSNET
                        strsql += " ,'" & RefNo & "'" 'REFNO
                        strsql += " ,'" & .Item("REFDATE").ToString & "'" 'REFDATE
                        strsql += " ,'" & .Item("COSTID").ToString & "'" 'COSTID
                        strsql += " ,'" & .Item("COMPANYID").ToString & "'" 'COMPANYID
                        strsql += " ,'" & .Item("FLAG").ToString & "'" 'FLAG
                        strsql += " ,'0'" 'PURITY
                        strsql += " ,'" & CatCode & "'" 'CATCODE
                        strsql += " ,'" & CatCode & "'" 'OCATCODE
                        strsql += " ,'" & .Item("ACCODE").ToString & "'" 'ACCODE
                        strsql += " ,'" & .Item("BATCHNO").ToString & "'" 'BATCHNO
                        strsql += " ,'" & .Item("REMARK1").ToString & "'" 'REMARK1
                        strsql += " ,'" & .Item("REMARK2").ToString & "'" 'REMARK2
                        strsql += " ,'" & .Item("USERID").ToString & "'" 'USERID
                        strsql += " ,'" & .Item("UPDATED").ToString & "'" 'UPDATED
                        strsql += " ,'" & Metalid & "'" 'METALID
                        strsql += " ," & StnAmt & "" 'STNAMT
                        strsql += " ,'EDREVERSE'" 'APPVER
                        strsql += " ,'" & .Item("ORDSTATE_ID").ToString & "'" 'ORDSTATE_ID
                        strsql += " ,'" & .Item("JJFRMNO").ToString & "'" 'JJFRMNO
                        strsql += " ,'" & .Item("MRKFLAG").ToString & "'" 'MRKFLG
                        strsql += " ,'" & Stktype & "'" 'STKTYPE
                        strsql += " )"
                        If cnCostId = cnHOCostId Then
                            cmd = New OleDbCommand(strsql, cn, tran)
                            cmd.ExecuteNonQuery()
                        Else
                            ExecQuery(SyncMode.Transaction, strsql, cn, tran, ToCostId)
                        End If

                        strsql = "INSERT INTO " & cnStockDb & "..RECEIPT(SNO,TRANNO,TRANDATE"
                        strsql += " ,TRANTYPE,PCS,GRSWT,NETWT,LESSWT,PUREWT,GRSNET,REFNO"
                        strsql += " ,REFDATE,COSTID,COMPANYID,FLAG"
                        strsql += " ,PURITY,CATCODE,OCATCODE,ACCODE,BATCHNO,REMARK1"
                        strsql += " ,REMARK2,USERID,UPDATED,METALID"
                        strsql += " ,STNAMT,APPVER,ORDSTATE_ID,JJFRMNO,MRKFLAG"
                        strsql += " ,STKTYPE"
                        strsql += " )"
                        strsql += " VALUES"
                        strsql += " ("
                        strsql += " '" & RecSno & "'" 'SNO
                        strsql += " ," & RecTranNo & "" 'TRANNO
                        strsql += " ,'" & RecTrandate & "'" 'TRANDATE
                        strsql += " ,'RIN'" 'TRANTYPE
                        strsql += " ," & Pcs & "" 'PCS
                        strsql += " ," & GrsWt & "" 'GRSWT
                        strsql += " ," & NetWt & "" 'NETWT
                        strsql += " ," & Stnwt & "" 'LESSWT
                        strsql += " ," & GrsWt - Stnwt & "" 'PUREWT
                        strsql += " ,'G'" 'GRSNET
                        strsql += " ,'" & RefNo & "'" 'REFNO
                        strsql += " ,'" & .Item("REFDATE").ToString & "'" 'REFDATE
                        strsql += " ,'" & ToCostId & "'" 'COSTID
                        strsql += " ,'" & .Item("COMPANYID").ToString & "'" 'COMPANYID
                        strsql += " ,'" & .Item("FLAG").ToString & "'" 'FLAG
                        strsql += " ,'0'" 'PURITY
                        strsql += " ,'" & CatCode & "'" 'CATCODE
                        strsql += " ,'" & CatCode & "'" 'OCATCODE
                        strsql += " ,'" & .Item("ACCODE").ToString & "'" 'ACCODE
                        strsql += " ,'" & ReceiptBatchno & "'" 'BATCHNO
                        strsql += " ,'" & .Item("REMARK1").ToString & "'" 'REMARK1
                        strsql += " ,'" & .Item("REMARK2").ToString & "'" 'REMARK2
                        strsql += " ,'" & .Item("USERID").ToString & "'" 'USERID
                        strsql += " ,'" & .Item("UPDATED").ToString & "'" 'UPDATED
                        strsql += " ,'" & Metalid & "'" 'METALID
                        strsql += " ," & StnAmt & "" 'STNAMT
                        strsql += " ,'EDREVERSE'" 'APPVER
                        strsql += " ,'" & .Item("ORDSTATE_ID").ToString & "'" 'ORDSTATE_ID
                        strsql += " ,'" & .Item("JJFRMNO").ToString & "'" 'JJFRMNO
                        strsql += " ,'" & .Item("MRKFLAG").ToString & "'" 'MRKFLG
                        strsql += " ,'" & Stktype & "'" 'STKTYPE
                        strsql += " )"
                        ExecQuery(SyncMode.Transaction, strsql, cn, tran, ToCostId)

                        If Stnwt > 0 Or StnPcs > 0 Then
                            For jj As Integer = 0 To dt.Rows.Count - 1
                                If dt.Rows(jj).Item("METALID").ToString = Metalid And _
                                dt.Rows(jj).Item("STKTYPE").ToString = Stktype And _
                                dt.Rows(jj).Item("CATCODE").ToString = CatCode Then
                                    If Val(dt.Rows(jj).Item("STNWT").ToString) = 0 And _
                                    Val(dt.Rows(jj).Item("STNPCS").ToString) = 0 Then Continue For
                                    Dim IssStnSno As String
                                    IssStnSno = GetNewSno(TranSnoType.ISSSTONECODE, tran)
                                    Dim RecStnSno As String
                                    RecStnSno = GetNewSno(TranSnoType.RECEIPTSTONECODE, tran)

                                    strsql = "INSERT INTO " & cnStockDb & "..ISSSTONE(SNO,ISSSNO,TRANNO,TRANDATE"
                                    strsql += " ,TRANTYPE,STNPCS,STNWT,STNAMT,STNITEMID,STNSUBITEMID,STONEUNIT"
                                    strsql += " ,COSTID,COMPANYID,BATCHNO,CATCODE,APPVER)"
                                    strsql += " VALUES"
                                    strsql += " ("
                                    strsql += " '" & IssStnSno & "'" 'SNO
                                    strsql += " ,'" & issSno & "'" 'ISSSNO
                                    strsql += " ," & .Item("TRANNO").ToString & "" 'TRANNO
                                    strsql += " ,'" & .Item("TRANDATE").ToString & "'" 'TRANDATE
                                    strsql += " ,'" & .Item("TRANTYPE").ToString & "'" 'TRANTYPE
                                    strsql += " ," & Val(dt.Rows(jj).Item("STNPCS").ToString) & "" 'STNPCS
                                    strsql += " ," & Val(dt.Rows(jj).Item("STNWT").ToString) & "" 'STNWT
                                    strsql += " ," & Val(dt.Rows(jj).Item("STNAMT").ToString) & "" 'STNAMT
                                    strsql += " ," & Val(dt.Rows(jj).Item("STNITEMID").ToString) & "" 'STNITEMID
                                    strsql += " ," & Val(dt.Rows(jj).Item("STNSUBITEMID").ToString) & "" 'STNSUBITEMID
                                    strsql += " ,'" & dt.Rows(jj).Item("STONEUNIT").ToString & "'" 'STONEUNIT
                                    strsql += " ,'" & .Item("COSTID").ToString & "'" 'COSTID
                                    strsql += " ,'" & .Item("COMPANYID").ToString & "'" 'COMPANYID
                                    strsql += " ,'" & .Item("BATCHNO").ToString & "'" 'BATCHNO
                                    strsql += " ,'" & dt.Rows(jj).Item("STNCATCODE").ToString & "'" 'CATCODE
                                    strsql += " ,'EDREVERSE'" 'APPVER
                                    strsql += " )"
                                    If cnCostId = cnHOCostId Then
                                        cmd = New OleDbCommand(strsql, cn, tran)
                                        cmd.ExecuteNonQuery()
                                    Else
                                        ExecQuery(SyncMode.Transaction, strsql, cn, tran, ToCostId)
                                    End If

                                    strsql = "INSERT INTO " & cnStockDb & "..RECEIPTSTONE(SNO,ISSSNO,TRANNO,TRANDATE"
                                    strsql += " ,TRANTYPE,STNPCS,STNWT,STNAMT,STNITEMID,STNSUBITEMID,STONEUNIT"
                                    strsql += " ,COSTID,COMPANYID,BATCHNO,CATCODE,APPVER)"
                                    strsql += " VALUES"
                                    strsql += " ("
                                    strsql += " '" & RecStnSno & "'" 'SNO
                                    strsql += " ,'" & RecSno & "'" 'ISSSNO
                                    strsql += " ," & RecTranNo & "" 'TRANNO
                                    strsql += " ,'" & RecTrandate & "'" 'TRANDATE
                                    strsql += " ,'RIN'" 'TRANTYPE
                                    strsql += " ," & Val(dt.Rows(jj).Item("STNPCS").ToString) & "" 'STNPCS
                                    strsql += " ," & Val(dt.Rows(jj).Item("STNWT").ToString) & "" 'STNWT
                                    strsql += " ," & Val(dt.Rows(jj).Item("STNAMT").ToString) & "" 'STNAMT
                                    strsql += " ," & Val(dt.Rows(jj).Item("STNITEMID").ToString) & "" 'STNITEMID
                                    strsql += " ," & Val(dt.Rows(jj).Item("STNSUBITEMID").ToString) & "" 'STNSUBITEMID
                                    strsql += " ,'" & dt.Rows(jj).Item("STONEUNIT").ToString & "'" 'STONEUNIT
                                    strsql += " ,'" & ToCostId & "'" 'COSTID
                                    strsql += " ,'" & .Item("COMPANYID").ToString & "'" 'COMPANYID
                                    strsql += " ,'" & ReceiptBatchno & "'" 'BATCHNO
                                    strsql += " ,'" & dt.Rows(jj).Item("STNCATCODE").ToString & "'" 'CATCODE
                                    strsql += " ,'EDREVERSE'" 'APPVER
                                    strsql += " )"
                                    ExecQuery(SyncMode.Transaction, strsql, cn, tran, ToCostId)
                                End If
                            Next
                        End If
                    End With
                Next
            End If
        End If
    End Sub

    Private Sub BtnSplit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSplit.Click
        'If cnCostId <> cnHOCostId Then MsgBox("Not allowed in Location...", MsgBoxStyle.Information) : Exit Sub
        If dGrid Is Nothing Then MsgBox("Record not found...", MsgBoxStyle.Information) : Exit Sub
        If dGrid.RowCount = 0 Then MsgBox("Record not found...", MsgBoxStyle.Information) : Exit Sub
        Dim chkfrmCostId As String = "ALL"
        If chkCmbCostCentrefrom.Text <> "ALL" And chkCmbCostCentrefrom.Text <> "" Then
            chkfrmCostId = GetSelectedCostId(chkCmbCostCentrefrom, True)
        End If
        Dim CostIds() As String = chkfrmCostId.Split(",")
        If CostIds.Length > 1 Then
            MsgBox("Required Single Costcentre Selection .", MsgBoxStyle.Information)
            chkCmbCostCentrefrom.Focus()
            Exit Sub
        End If
        Dim Refno As String
        lblStatus.Text = ""
        Dim objSecret As New frmAdminPassword()
        If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then
            Exit Sub
        End If
        For J As Integer = 0 To dGrid.RowCount - 1
            With dGrid.Rows(J)
                If Val(.Cells("RESULT").Value.ToString) <> 2 Then Continue For
                If Refno = .Cells("REFNO").Value.ToString Then Continue For
                Try
                    Refno = .Cells("REFNO").Value.ToString
                    tran = cn.BeginTransaction
                    lblStatus.Text = "Processing RefNo:" & Refno
                    lblStatus.Refresh()
                    funcSplitbySno(Refno, .Cells("TRANDATE").Value.ToString, .Cells("RECTRANDATE").Value.ToString, chkfrmCostId)
                    tran.Commit()
                    tran = Nothing
                    Thread.Sleep(5000)
                Catch ex As Exception
                    If Not tran Is Nothing Then tran.Rollback() : tran = Nothing
                    MsgBox(ex.Message + vbCrLf + ex.StackTrace, MsgBoxStyle.Information)
                    Exit Sub
                End Try
            End With
        Next
        MsgBox("Generated Successfully...", MsgBoxStyle.Information)
    End Sub
End Class