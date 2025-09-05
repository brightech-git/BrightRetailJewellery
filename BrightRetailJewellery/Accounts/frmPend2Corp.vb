Imports System.Data.OleDb
Public Class frmPend2Corp
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim dtGridView As New DataTable
    Dim dtGrid As New DataTable
    Dim dtGridExcel As New DataTable
    Dim WithEvents lstSearch As New ListBox
    Dim COST_ID As String
    Dim COST_NAME As String
    Dim searchSender As Control = Nothing
    Dim listItem As New List(Of String)
    Dim listCounter As New List(Of String)
    Dim tempText As New TextBox
    Dim dtSummary As New DataTable
    Dim PENDTRF_MI As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PENDTRF_MI'", , "N", tran)
    Dim CheckState As Boolean
    Private WithEvents txtCheckedItems As New TextBox
    Dim AUTOBOOKVALUE As String = GetAdmindbSoftValue("AUTOBOOKVALUE", "N,0,0,0,0,0")
    Dim AUTOBOOK_VOUCHER As String = GetAdmindbSoftValue("AUTOBOOK_VOUCHER", "N")
    Dim AUTOBOOKVALUEARRY() As String = Split(AUTOBOOKVALUE, ",")
    Dim AUTOBOOKVALUEENABLE As String = AUTOBOOKVALUEARRY(0).ToString
    Dim CENTR_DB_BR As Boolean = IIf(GetAdmindbSoftValue("CENTR_DB_ALLCOSTID", "N") = "Y", True, False)
    Dim LOCK_MRMI_TRSDATE As Boolean = IIf(GetAdmindbSoftValue("LOCK_MRMI_TRSDATE", "Y") = "Y", True, False)
    Dim PENDTRF_ASON As Boolean = IIf(GetAdmindbSoftValue("PENDTRF_ASON", "Y") = "Y", True, False)
    Dim PENDTRF_SNOBASE As Boolean = IIf(GetAdmindbSoftValue("PENDTRF_SNOBASE", "N") = "Y", True, False)
    Dim preTranDb As String = Nothing
    Dim TAG_DUMP_ACCODE As String = ""
    Dim TAG_DUMP As String = GetAdmindbSoftValue("TAG_DUMP", "")
    Dim dtCategory As New DataTable
    Dim PENDTRF_AMOUNT_VISIBLE As String = GetAdmindbSoftValue("PENDTRF_AMOUNT_VISIBLE", "N")
    Dim TITLE As String
    Dim PENDTRF_ENTRYDATE As Boolean = IIf(GetAdmindbSoftValue("PENDTRF_ENTRYDATE", "Y") = "Y", True, False)

    Dim StateId As Integer
    Dim XCnAdmin As OleDbConnection = Nothing
    Public Xtran As OleDbTransaction = Nothing
    Private XSyncdb As String = Replace(cnAdminDb, "ADMINDB", "UTILDB")
    Dim GstRecCode As String = GetAdmindbSoftValue("GSTACCODE_INTTRF_REC", "")
    Dim GstRecAcc() As String
    Dim SCode As String
    Dim CCode As String
    Dim ICode As String
    Dim STKTRAN_ContraPost As Boolean = IIf(GetAdmindbSoftValue("STKTRAN_ACCOUNT_POST", "N") = "Y", True, False)
    Dim PSSR_CATPOST As String = GetAdmindbSoftValue("PSSR_CATEGORYPOST", "")
    Dim PSSR_CATPOSTING() As String
    Dim NeedItemType_accpost As Boolean = IIf(GetAdmindbSoftValue("POS_SEPACCPOST_ITEMTYPE", "N") = "Y", True, False)
    Dim MetalBasedStone As Boolean = IIf(GetAdmindbSoftValue("MULTIMETALBASEDSTONE", "N") = "Y", True, False)

    Private Sub funcGridTotal()
        strSql = " SELECT '' TRANTYPE,'' TRANDATE,''TRANNO,''DESCRIPTION"
        strSql += " ,CAST(NULL AS INTEGER) PCS"
        strSql += " ,CAST(NULL AS DECIMAL(15,3)) GRSWT,CAST(NULL AS DECIMAL(15,3)) NETWT"
        strSql += " ,CAST(NULL AS INTEGER) DIAPCS,CAST(NULL AS DECIMAL(15,3)) DIAWT"
        strSql += " ,CAST(NULL AS DECIMAL(15,3)) STNWT,CAST(NULL AS DECIMAL(15,3)) PREWT"
        strSql += " ,CAST(NULL AS DECIMAL(15,2))RATE"
        strSql += " ,CAST(NULL AS DECIMAL(15,2))STNAMT"
        strSql += " ,CAST(NULL AS DECIMAL(15,2))AMOUNT WHERE 1<>1"
        dtGridView = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        dtGridView.Columns.Add("CHECK", GetType(Boolean))
        dtGridView.Columns.Add("TRANTYPE", GetType(String))
        da.Fill(dtGridView)
        Dim dtGridViewTotal As New DataTable
        dtGridViewTotal = dtGridView.Copy
        dtGridViewTotal.Rows.Clear()
        dtGridViewTotal.Rows.Add()
        dtGridViewTotal.Rows.Add()
        dtGridViewTotal.Rows.Add()
        dtGridViewTotal.Rows.Add()
        dtGridViewTotal.Rows.Add()
        gridViewtotal.ColumnHeadersVisible = False
        gridViewtotal.DataSource = dtGridViewTotal
        For Each col As DataGridViewColumn In gridView.Columns
            With gridViewtotal.Columns(col.Name)
                .Visible = col.Visible
                .Width = col.Width
                .DefaultCellStyle = col.DefaultCellStyle
            End With
        Next
        dtGridViewTotal.Columns.Remove("CHECK")
        CalcGridViewTotalN()
    End Sub
    Private Sub CalcGridViewTotalN()
        DgvView.Update()
        Dim LAmt As Double = Nothing
        Dim LStnAmt As Double = Nothing
        Dim LPcs As Integer = Nothing
        Dim LGrsWt As Decimal = Nothing
        Dim LNetWt As Decimal = Nothing
        Dim LDiaPcs As Integer = Nothing
        Dim LDiaWt As Decimal = Nothing
        Dim LStnWt As Decimal = Nothing
        Dim LPreWt As Decimal = Nothing
        Dim RAmt As Double = Nothing
        Dim RStnAmt As Double = Nothing
        Dim RPcs As Integer = Nothing
        Dim RGrsWt As Decimal = Nothing
        Dim RNetWt As Decimal = Nothing
        Dim RDiaPcs As Integer = Nothing
        Dim RDiaWt As Decimal = Nothing
        Dim RStnWt As Decimal = Nothing
        Dim RPreWt As Decimal = Nothing
        Dim PDiaPcs As Integer = Nothing
        Dim PDiaWt As Decimal = Nothing
        Dim PStnWt As Decimal = Nothing
        Dim PPreWt As Decimal = Nothing
        Dim PAmt As Double = Nothing
        Dim PStnAmt As Double = Nothing
        Dim PPcs As Integer = Nothing
        Dim PGrsWt As Decimal = Nothing
        Dim PNetWt As Decimal = Nothing
        Dim UnAmt As Double = Nothing
        Dim UnStnAmt As Double = Nothing
        Dim UnPcs As Integer = Nothing
        Dim UnGrsWt As Decimal = Nothing
        Dim UnNetWt As Decimal = Nothing
        Dim UDiaPcs As Integer = Nothing
        Dim UDiaWt As Decimal = Nothing
        Dim UStnWt As Decimal = Nothing
        Dim UPreWt As Decimal = Nothing
        Dim MAmt As Double = Nothing
        Dim MStnAmt As Double = Nothing
        Dim MPcs As Integer = Nothing
        Dim MGrsWt As Decimal = Nothing
        Dim MNetWt As Decimal = Nothing
        Dim MDiaPcs As Integer = Nothing
        Dim MDiaWt As Decimal = Nothing
        Dim MStnWt As Decimal = Nothing
        Dim MPreWt As Decimal = Nothing
        For i As Integer = 0 To DgvView.RowCount - 1
            With DgvView.Rows(i)
                If .Cells("CHECK").Value = True Then
                    If .Cells("TRANTYPE").Value.ToString.Trim = "PARTSALE" Then
                        LPcs += Val(.Cells("PCS").Value.ToString)
                        LGrsWt += Val(.Cells("GRSWT").Value.ToString)
                        LNetWt += Val(.Cells("NETWT").Value.ToString)
                        LAmt += Val(.Cells("AMOUNT").Value.ToString)
                        If .Cells("COLHEAD").Value.ToString = "A" Then
                            LStnAmt += Val(.Cells("STNAMT").Value.ToString)
                        End If
                        LDiaPcs += Val(.Cells("DIAPCS").Value.ToString)
                        LDiaWt += Val(.Cells("DIAWT").Value.ToString)
                        LStnWt += Val(.Cells("STNWT").Value.ToString)
                        LPreWt += Val(.Cells("PREWT").Value.ToString)
                    ElseIf .Cells("TRANTYPE").Value.ToString.Trim = "SALERETURN" Then
                        RPcs += Val(.Cells("PCS").Value.ToString)
                        RGrsWt += Val(.Cells("GRSWT").Value.ToString)
                        RNetWt += Val(.Cells("NETWT").Value.ToString)
                        If .Cells("COLHEAD").Value.ToString = "A" Then
                            RStnAmt += Val(.Cells("STNAMT").Value.ToString)
                        End If
                        RAmt += Val(.Cells("AMOUNT").Value.ToString)
                        RDiaPcs += Val(.Cells("DIAPCS").Value.ToString)
                        RDiaWt += Val(.Cells("DIAWT").Value.ToString)
                        RStnWt += Val(.Cells("STNWT").Value.ToString)
                        RPreWt += Val(.Cells("PREWT").Value.ToString)
                    ElseIf .Cells("TRANTYPE").Value.ToString.Trim = "PURCHASE" Then
                        PPcs += Val(.Cells("PCS").Value.ToString)
                        PGrsWt += Val(.Cells("GRSWT").Value.ToString)
                        PNetWt += Val(.Cells("NETWT").Value.ToString)
                        PAmt += Val(.Cells("AMOUNT").Value.ToString)
                        If .Cells("COLHEAD").Value.ToString = "A" Then
                            PStnAmt += Val(.Cells("STNAMT").Value.ToString)
                        End If
                        PDiaPcs += Val(.Cells("DIAPCS").Value.ToString)
                        PDiaWt += Val(.Cells("DIAWT").Value.ToString)
                        PStnWt += Val(.Cells("STNWT").Value.ToString)
                        PPreWt += Val(.Cells("PREWT").Value.ToString)
                    ElseIf .Cells("TRANTYPE").Value.ToString.Trim = "MISCISSUE" Then
                        MPcs += Val(.Cells("PCS").Value.ToString)
                        MGrsWt += Val(.Cells("GRSWT").Value.ToString)
                        MNetWt += Val(.Cells("NETWT").Value.ToString)
                        MAmt += Val(.Cells("AMOUNT").Value.ToString)
                        If .Cells("COLHEAD").Value.ToString = "A" Then
                            MStnAmt += Val(.Cells("STNAMT").Value.ToString)
                        End If
                        MDiaPcs += Val(.Cells("DIAPCS").Value.ToString)
                        MDiaWt += Val(.Cells("DIAWT").Value.ToString)
                        MStnWt += Val(.Cells("STNWT").Value.ToString)
                        MPreWt += Val(.Cells("PREWT").Value.ToString)
                    End If
                End If
                UnPcs += Val(.Cells("PCS").Value.ToString)
                UnGrsWt += Val(.Cells("GRSWT").Value.ToString)
                UnNetWt += Val(.Cells("NETWT").Value.ToString)
                UnAmt += Val(.Cells("AMOUNT").Value.ToString)
                If .Cells("COLHEAD").Value.ToString = "A" Then
                    UnStnAmt += Val(.Cells("STNAMT").Value.ToString)
                End If
                UDiaPcs += Val(.Cells("DIAPCS").Value.ToString)
                UDiaWt += Val(.Cells("DIAWT").Value.ToString)
                UStnWt += Val(.Cells("STNWT").Value.ToString)
                UPreWt += Val(.Cells("PREWT").Value.ToString)
            End With
        Next
        If gridViewtotal.RowCount > 0 Then
            With gridViewtotal.Rows(0)
                .Cells("TRANTYPE").Value = "TOTAL"
                .Cells("PCS").Value = IIf(UnPcs <> 0, UnPcs, DBNull.Value)
                .Cells("GRSWT").Value = IIf(UnGrsWt <> 0, UnGrsWt, DBNull.Value)
                .Cells("NETWT").Value = IIf(UnNetWt <> 0, UnNetWt, DBNull.Value)
                .Cells("DIAPCS").Value = IIf(UDiaPcs <> 0, UDiaPcs, DBNull.Value)
                .Cells("DIAWT").Value = IIf(UDiaWt <> 0, UDiaWt, DBNull.Value)
                .Cells("STNWT").Value = IIf(UStnWt <> 0, UStnWt, DBNull.Value)
                .Cells("PREWT").Value = IIf(UPreWt <> 0, UPreWt, DBNull.Value)
                .Cells("STNAMT").Value = IIf(UnStnAmt <> 0, UnStnAmt, DBNull.Value)
                .Cells("AMOUNT").Value = IIf(UnAmt <> 0, UnAmt, DBNull.Value)
                .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                .DefaultCellStyle.ForeColor = Color.Blue
                .DefaultCellStyle.SelectionForeColor = Color.Blue
                .DefaultCellStyle.BackColor = Color.LightYellow
            End With

            With gridViewtotal.Rows(1)
                .Cells("TRANTYPE").Value = "PARTSALE"
                .Cells("PCS").Value = IIf(LPcs <> 0, LPcs, DBNull.Value)
                .Cells("GRSWT").Value = IIf(LGrsWt <> 0, LGrsWt, DBNull.Value)
                .Cells("NETWT").Value = IIf(LNetWt <> 0, LNetWt, DBNull.Value)
                .Cells("AMOUNT").Value = IIf(LAmt <> 0, LAmt, DBNull.Value)
                .Cells("STNAMT").Value = IIf(LStnAmt <> 0, LStnAmt, DBNull.Value)
                .Cells("DIAPCS").Value = IIf(LDiaPcs <> 0, LDiaPcs, DBNull.Value)
                .Cells("DIAWT").Value = IIf(LDiaWt <> 0, LDiaWt, DBNull.Value)
                .Cells("STNWT").Value = IIf(LStnWt <> 0, LStnWt, DBNull.Value)
                .Cells("PREWT").Value = IIf(LPreWt <> 0, LPreWt, DBNull.Value)
                .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                .DefaultCellStyle.ForeColor = Color.Blue
                .DefaultCellStyle.SelectionForeColor = Color.Blue
                .DefaultCellStyle.BackColor = Color.LightYellow
            End With
            With gridViewtotal.Rows(2)
                .Cells("TRANTYPE").Value = "SALERETURN"
                .Cells("PCS").Value = IIf(RPcs <> 0, RPcs, DBNull.Value)
                .Cells("GRSWT").Value = IIf(RGrsWt <> 0, RGrsWt, DBNull.Value)
                .Cells("NETWT").Value = IIf(RNetWt <> 0, RNetWt, DBNull.Value)
                .Cells("AMOUNT").Value = IIf(RAmt <> 0, RAmt, DBNull.Value)
                .Cells("STNAMT").Value = IIf(RStnAmt <> 0, RStnAmt, DBNull.Value)
                .Cells("DIAPCS").Value = IIf(RDiaPcs <> 0, RDiaPcs, DBNull.Value)
                .Cells("DIAWT").Value = IIf(RDiaWt <> 0, RDiaWt, DBNull.Value)
                .Cells("STNWT").Value = IIf(RStnWt <> 0, RStnWt, DBNull.Value)
                .Cells("PREWT").Value = IIf(RPreWt <> 0, RPreWt, DBNull.Value)
                .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                .DefaultCellStyle.ForeColor = Color.Blue
                .DefaultCellStyle.SelectionForeColor = Color.Blue
                .DefaultCellStyle.BackColor = Color.LightYellow
            End With
            With gridViewtotal.Rows(3)
                .Cells("TRANTYPE").Value = "PURCHASE"
                .Cells("PCS").Value = IIf(PPcs <> 0, PPcs, DBNull.Value)
                .Cells("GRSWT").Value = IIf(PGrsWt <> 0, PGrsWt, DBNull.Value)
                .Cells("NETWT").Value = IIf(PNetWt <> 0, PNetWt, DBNull.Value)
                .Cells("AMOUNT").Value = IIf(PAmt <> 0, PAmt, DBNull.Value)
                .Cells("STNAMT").Value = IIf(PStnAmt <> 0, PStnAmt, DBNull.Value)
                .Cells("DIAPCS").Value = IIf(PDiaPcs <> 0, PDiaPcs, DBNull.Value)
                .Cells("DIAWT").Value = IIf(PDiaWt <> 0, PDiaWt, DBNull.Value)
                .Cells("STNWT").Value = IIf(PStnWt <> 0, PStnWt, DBNull.Value)
                .Cells("PREWT").Value = IIf(PPreWt <> 0, PPreWt, DBNull.Value)
                .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                .DefaultCellStyle.ForeColor = Color.Blue
                .DefaultCellStyle.SelectionForeColor = Color.Blue
                .DefaultCellStyle.BackColor = Color.LightYellow
            End With
            With gridViewtotal.Rows(4)
                .Cells("TRANTYPE").Value = "MISCISSUE"
                .Cells("PCS").Value = IIf(MPcs <> 0, MPcs, DBNull.Value)
                .Cells("GRSWT").Value = IIf(MGrsWt <> 0, MGrsWt, DBNull.Value)
                .Cells("NETWT").Value = IIf(MNetWt <> 0, MNetWt, DBNull.Value)
                .Cells("AMOUNT").Value = IIf(MAmt <> 0, MAmt, DBNull.Value)
                .Cells("STNAMT").Value = IIf(MStnAmt <> 0, MStnAmt, DBNull.Value)
                .Cells("DIAPCS").Value = IIf(MDiaPcs <> 0, MDiaPcs, DBNull.Value)
                .Cells("DIAWT").Value = IIf(MDiaWt <> 0, MDiaWt, DBNull.Value)
                .Cells("STNWT").Value = IIf(MStnWt <> 0, MStnWt, DBNull.Value)
                .Cells("PREWT").Value = IIf(MPreWt <> 0, MPreWt, DBNull.Value)
                .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                .DefaultCellStyle.ForeColor = Color.Blue
                .DefaultCellStyle.SelectionForeColor = Color.Blue
                .DefaultCellStyle.BackColor = Color.LightYellow
            End With
        End If
        If DgvView.RowCount > 0 Then
            If gridViewtotal.Rows.Count = 0 Then Exit Sub
            With gridViewtotal
                .Visible = True
                .Columns("TRANTYPE").Width = DgvView.Columns("TRANTYPE").Width + DgvView.Columns("CHECK").Width
                .Columns("TRANDATE").Width = DgvView.Columns("TRANDATE").Width
                .Columns("TRANNO").Width = DgvView.Columns("TRANNO").Width
                .Columns("DESCRIPTION").Width = DgvView.Columns("DESCRIPTION").Width
                .Columns("PCS").Width = DgvView.Columns("PCS").Width
                .Columns("GRSWT").Width = DgvView.Columns("GRSWT").Width
                .Columns("NETWT").Width = DgvView.Columns("NETWT").Width
                .Columns("DIAPCS").Width = DgvView.Columns("DIAPCS").Width
                .Columns("DIAWT").Width = DgvView.Columns("DIAWT").Width
                .Columns("STNWT").Width = DgvView.Columns("STNWT").Width
                .Columns("PREWT").Width = DgvView.Columns("PREWT").Width
                .Columns("AMOUNT").Width = DgvView.Columns("AMOUNT").Width
                .Columns("STNAMT").Width = DgvView.Columns("STNAMT").Width
                .Columns("RATE").Width = DgvView.Columns("RATE").Width
                .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("DIAPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("DIAWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("PREWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("STNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("STNAMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("RATE").DefaultCellStyle.Format = "0.00"
                .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
                .Columns("STNAMT").DefaultCellStyle.Format = "0.00"
                .Columns("GRSWT").DefaultCellStyle.Format = "0.000"
                .Columns("NETWT").DefaultCellStyle.Format = "0.000"
                .Columns("DIAWT").DefaultCellStyle.Format = "0.000"
                .Columns("STNWT").DefaultCellStyle.Format = "0.000"
                .Columns("PREWT").DefaultCellStyle.Format = "0.000"
            End With
        End If
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        tabMain.SelectedTab = tabGeneral
        dtpFrom.Select()
        btnSearch.Enabled = True
        dtpFrom.Value = GetEntryDate(dtpFrom.Value)
        dtpTo.Value = GetEntryDate(dtpTo.Value)
        Dim TrsDate As Date = GetEntryDate(dtpFrom.Value)
        lblTrsDate.Text = "TransferDate:" & Format(TrsDate, "dd-MM-yyyy")
        If LOCK_MRMI_TRSDATE Then
            lblTrsDate.Text = "TransferDate:" & Format(GetEntryDate(dtpFrom.Value), "dd-MM-yyyy")
        End If
        DgvView.DataSource = Nothing
        DgvView.Refresh()
        gridViewtotal.DataSource = Nothing
        gridViewtotal.Refresh()
        btnTransfer.Enabled = False
        dtGridView.Clear()
        Prop_Gets()
        strSql = " SELECT 'ALL' CATNAME,'ALL' CATCODE,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT CATNAME,CATCODE,2 RESULT FROM " & cnAdminDb & "..CATEGORY "
        strSql += " ORDER BY RESULT,CATNAME"
        dtCategory = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCategory)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCategory, dtCategory, "CATNAME", , "ALL")
        If PENDTRF_MI = "Y" Then
            ChkWithMisc.Enabled = True
        Else
            ChkWithMisc.Checked = False
            ChkWithMisc.Enabled = False
        End If
        btnSearch.Enabled = True
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGeneral
        If lstSearch.Visible And (Not searchSender Is Nothing) Then
            searchSender.Select()
            lstSearch.Visible = False
            txtGrid.Visible = False
        End If
        gbSummary.Visible = False
        dtpFrom.Focus()
    End Sub

    Private Sub frmPend_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If tabMain.SelectedTab.Name = tabGeneral.Name Then
                If gbSummary.Visible = True Then
                    gbSummary.Visible = False
                    DgvView.Focus()
                    lblHelp.Visible = True
                    Exit Sub
                End If
            End If
        ElseIf e.KeyCode = Keys.S Then
            If tabMain.SelectedTab.Name = tabGeneral.Name Then
                If gbSummary.Visible = False Then
                    Call Trfsummary()
                    lblHelp.Visible = False
                    Exit Sub
                End If
            End If
        End If
    End Sub

    Private Sub frmPend_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView.Focused Then Exit Sub
            If lstSearch.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmPend_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        If GetAdmindbSoftValue("COSTCENTRE", "N") = "Y" Then
            cmbCostCentre_MAN.Enabled = True
            strSql = "SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE ISNULL(ACTIVE,'Y')<>'N' ORDER BY COSTNAME"
            objGPack.FillCombo(strSql, cmbCostCentre_MAN, , True)
            cmbCostCentre_MAN.Text = cnCostName
        Else
            cmbCostCentre_MAN.Text = ""
        End If
        cmbCostCentre_MAN.Enabled = False
        If PENDTRF_ASON = False Then
            lblToDate.Visible = True
            dtpTo.Visible = True
            lblFromDate.Text = "From Date"
        End If
        ''Load Metal Name
        strSql = " SELECT 'ALL' METALNAME "
        strSql += " UNION ALL"
        strSql += " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY METALNAME"
        'Dim dtMetal As New DataTable
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dtMetal)
        'CmbMetal_MAN.Items.Clear()
        'For Each dr As DataRow In dtMetal.Rows
        '    CmbMetal_MAN.Items.Add(dr!METALNAME.ToString)
        'Next
        objGPack.FillCombo(strSql, CmbMetal_MAN, , True)
        'CmbMetal_MAN.Text = "ALL"
        gridView.RowTemplate.Height = 21
        gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect

        ''Load Company Name
        strSql = " SELECT 'ALL' COMPANYNAME "
        strSql += " UNION ALL"
        strSql += " SELECT COMPANYNAME FROM " & cnAdminDb & "..COMPANY ORDER BY COMPANYNAME"
        objGPack.FillCombo(strSql, CmbCompany_MAN, , True)
        CmbCompany_MAN.Text = strCompanyName
        gridView.RowTemplate.Height = 21
        gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect

        strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER ORDER BY ITEMCTRNAME"
        Dim dtcounter As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtcounter)
        listCounter.Clear()
        For Each dr As DataRow In dtcounter.Rows
            listCounter.Add(dr!ITEMCTRNAME.ToString)
        Next

        If GstRecCode.Contains(",") Then
            GstRecAcc = GstRecCode.Split(",")
            If GstRecAcc.Length <> 3 Then
                MsgBox("GST Account not set Properly Control Id[GSTACCODE_INTTRF].", MsgBoxStyle.Information)
            Else
                SCode = GstRecAcc(0).ToString
                CCode = GstRecAcc(1).ToString
                ICode = GstRecAcc(2).ToString
            End If
        End If

        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE ='N' ORDER BY ITEMNAME"
        Dim dtitem As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtitem)
        listItem.Clear()
        For Each dr As DataRow In dtitem.Rows
            listItem.Add(dr!ITEMNAME.ToString)
        Next
        txtGrid.Visible = False
        lstSearch.Visible = False
        Dim TAG() As String
        TAG = TAG_DUMP.Split(",")
        If TAG.Length > 2 Then
            TAG_DUMP_ACCODE = TAG(2).ToString
        End If

        If PSSR_CATPOST <> "" Then
            PSSR_CATPOSTING = PSSR_CATPOST.ToString.Split(",")
        End If

        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        DgvView.DataSource = Nothing
        DgvView.Refresh()
        lstSearch.Visible = False
        txtGrid.Visible = False
        btnSearch.Enabled = False
        ChkAll.Checked = False
        Prop_Sets()
        Dim TrsDate As Date = dtpFrom.Value
        gridViewtotal.BackgroundColor = Me.BackColor
        If LOCK_MRMI_TRSDATE = False Then
            lblTrsDate.Text = "TransferDate:" & Format(TrsDate, "dd-MM-yyyy")
        End If
        Dim tDate As Date = cnTranToDate
        If tDate.Month > 3 Then
            preTranDb = cnCompanyId + "T" + Mid(tDate.Year - 1, 3, 2) + Mid(tDate.Year, 3, 2)
        Else
            preTranDb = cnCompanyId + "T" + Mid(tDate.Year - 2, 3, 2) + Mid(tDate.Year - 1, 3, 2)
        End If
        If Not objGPack.GetSqlValue("SELECT NAME FROM SYSDATABASES WHERE NAME = '" & preTranDb & "'").Length > 0 Then
            preTranDb = Nothing
        End If
        Dim flag As Boolean = False
        If ChkWithMisc.Checked = False And ChKSAL.Checked = False And ChkSr.Checked = False And Chkpurchase.Checked = False Then MsgBox("Selection Empty", MsgBoxStyle.Information) : Exit Sub
        strSql = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "PENDTRANS') > 0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "PENDTRANS"
        If ChkWithMisc.Checked Or ChKSAL.Checked Or ChkSr.Checked Or Chkpurchase.Checked Then
            strSql += vbCrLf + "  SELECT *   INTO TEMPTABLEDB..TEMP" & systemId & "PENDTRANS FROM ( "
            '' PARTY SALES 
            If ChKSAL.Checked Then
                strSql += vbCrLf + " SELECT T.TRANDATE,T.TRANNO,T.TRANTYPE/*,T.CATCODE*/"

                If NeedItemType_accpost Then
                    strSql += vbCrLf + " ,CASE WHEN ISNULL(T.ITEMTYPEID,0)<>0 THEN CASE WHEN ISNULL((SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =T.ITEMTYPEID),'') <>''"
                    strSql += vbCrLf + " THEN (SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =T.ITEMTYPEID) ELSE T.CATCODE END ELSE  T.CATCODE END CATCODE "
                Else
                    strSql += vbCrLf + " ,T.CATCODE"
                End If
                strSql += vbCrLf + " ,T.METALID,T.ITEMID"
                strSql += vbCrLf + " ,T.TAGPCS-T.PCS PCS,T.TAGGRSWT-T.GRSWT GRSWT,T.TAGNETWT-T.NETWT NETWT "
                strSql += vbCrLf + " ,CONVERT(VARCHAR(7),NULL) STNCATCODE"
                strSql += vbCrLf + " ,CONVERT(INT,NULL)DIAPCS,CONVERT(NUMERIC(15,3),NULL)DIAWT"
                strSql += vbCrLf + " ,CONVERT(INT,NULL)STNPCS,CONVERT(NUMERIC(15,3),NULL)STNWT"
                strSql += vbCrLf + " ,CONVERT(INT,NULL)PREPCS,CONVERT(NUMERIC(15,3),NULL)PREWT"
                strSql += vbCrLf + " ,T.SNO,T.BATCHNO"
                strSql += vbCrLf + " ,RATE,CONVERT(NUMERIC(20,2),NULL)STNAMT"
                If PENDTRF_AMOUNT_VISIBLE = "N" Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),NULL) AMOUNT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),AMOUNT) AMOUNT"
                End If
                strSql += vbCrLf + " ,'I' AS TYPE,'A' COLHEAD"
                strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE T "
                If PENDTRF_ASON Then
                    strSql += vbCrLf + " WHERE T.TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                Else
                    strSql += vbCrLf + " WHERE T.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                End If
                strSql += vbCrLf + " AND T.TRANTYPE IN('SA','RD','OD') AND ISNULL(T.CANCEL,'') = ''"
                strSql += vbCrLf + " AND (T.TAGPCS <> T.PCS OR T.TAGGRSWT <> T.GRSWT )"
                strSql += vbCrLf + " AND (T.TAGPCS <> 0 OR T.TAGGRSWT <> 0 )"
                If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND ISNULL(T.COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                If CmbMetal_MAN.Text <> "" And CmbMetal_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND T.METALID=(SELECT TOP 1 METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & CmbMetal_MAN.Text & "')"
                If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND T.COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                If chkCmbCategory.Text <> "" And chkCmbCategory.Text <> "ALL" Then strSql += vbCrLf + " AND T.CATCODE=(SELECT TOP 1 CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & GetQryString(chkCmbCategory.Text) & "))"
                'strSql += vbCrLf + " AND T.COMPANYID = '" & strCompanyId & "'"
                strSql += vbCrLf + " AND ISNULL(T.BAGNO,'')= ''"
                strSql += vbCrLf + " AND ISNULL(T.TRFNO,'')= ''"
                If MetalBasedStone Then '' added for vbj on 04-06-2022 as per magesh sir advice
                    strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnStockDb & "..ISSMETAL WHERE ISSSNO =T.SNO) "
                    strSql += vbCrLf + " UNION ALL "
                    strSql += vbCrLf + " SELECT T.TRANDATE,T.TRANNO,T.TRANTYPE,TIM.CATCODE"
                    strSql += vbCrLf + " ,(SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=TIM.CATCODE) METALID"
                    strSql += vbCrLf + " ,0 ITEMID,0 PCS"
                    strSql += vbCrLf + " ,(SELECT GRSWT FROM " & cnAdminDb & "..ITEMTAGMETAL  WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID=T.ITEMID And TAGNO=T.TAGNO) "
                    strSql += vbCrLf + " And CATCODE =TIM.CATCODE)-TIM.GRSWT GRSWT"
                    strSql += vbCrLf + " ,(SELECT CASE WHEN ISNULL(NETWT,0)<>0 THEN NETWT ELSE GRSWT END FROM " & cnAdminDb & "..ITEMTAGMETAL  WHERE TAGSNO IN  "
                    strSql += vbCrLf + " (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID=T.ITEMID And TAGNO=T.TAGNO) And CATCODE =TIM.CATCODE)"
                    strSql += vbCrLf + " - (CASE WHEN ISNULL(TIM.NETWT,0)<>0 THEN TIM.NETWT ELSE TIM.GRSWT END ) NETWT "
                    strSql += vbCrLf + " ,CONVERT(VARCHAR(7),NULL) STNCATCODE"
                    strSql += vbCrLf + " ,CONVERT(INT,NULL)DIAPCS,CONVERT(NUMERIC(15,3),NULL)DIAWT"
                    strSql += vbCrLf + " ,CONVERT(INT,NULL)STNPCS,CONVERT(NUMERIC(15,3),NULL)STNWT"
                    strSql += vbCrLf + " ,CONVERT(INT,NULL)PREPCS,CONVERT(NUMERIC(15,3),NULL)PREWT"
                    strSql += vbCrLf + " ,T.SNO,T.BATCHNO"
                    strSql += vbCrLf + " ,(" & cnAdminDb & ".dbo.GET_TODAY_RATE(TIM.CATCODE,'" & dtpFrom.Value.ToString("yyyy-MM-dd") & "')) RATE,CONVERT(NUMERIC(20,2),NULL)STNAMT"
                    If PENDTRF_AMOUNT_VISIBLE = "N" Then
                        strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),NULL) AMOUNT"
                    Else
                        strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),TIM.AMOUNT) AMOUNT"
                    End If
                    strSql += vbCrLf + " ,'I' AS TYPE,'A' COLHEAD"
                    strSql += vbCrLf + " FROM " & cnStockDb & "..ISSMETAL TIM "
                    strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..ISSUE T ON TIM.ISSSNO =T.SNO"
                    If PENDTRF_ASON Then
                        strSql += vbCrLf + " WHERE T.TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                    Else
                        strSql += vbCrLf + " WHERE T.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                    End If
                    strSql += vbCrLf + " AND T.TRANTYPE IN('SA','RD','OD') AND ISNULL(T.CANCEL,'') = ''"
                    strSql += vbCrLf + " AND (T.TAGPCS <> T.PCS OR T.TAGGRSWT <> T.GRSWT )"
                    strSql += vbCrLf + " AND (T.TAGPCS <> 0 OR T.TAGGRSWT <> 0 )"
                    strSql += vbCrLf + " AND (((SELECT GRSWT FROM " & cnAdminDb & "..ITEMTAGMETAL  WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID=T.ITEMID And TAGNO=T.TAGNO) "
                    strSql += vbCrLf + " And CATCODE =TIM.CATCODE)-TIM.GRSWT) <> 0"
                    strSql += vbCrLf + " OR ((SELECT CASE WHEN ISNULL(NETWT,0)<>0 THEN NETWT ELSE GRSWT END FROM " & cnAdminDb & "..ITEMTAGMETAL  WHERE TAGSNO IN  "
                    strSql += vbCrLf + " (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID=T.ITEMID And TAGNO=T.TAGNO) And CATCODE =TIM.CATCODE)"
                    strSql += vbCrLf + " - (CASE WHEN ISNULL(TIM.NETWT,0)<>0 THEN TIM.NETWT ELSE TIM.GRSWT END )) <> 0) "
                    If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND ISNULL(T.COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                    If CmbMetal_MAN.Text <> "" And CmbMetal_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND T.METALID=(SELECT TOP 1 METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & CmbMetal_MAN.Text & "')"
                    If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND T.COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                    If chkCmbCategory.Text <> "" And chkCmbCategory.Text <> "ALL" Then strSql += vbCrLf + " AND T.CATCODE=(SELECT TOP 1 CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & GetQryString(chkCmbCategory.Text) & "))"
                    'strSql += vbCrLf + " AND T.COMPANYID = '" & strCompanyId & "'"
                    strSql += vbCrLf + " AND ISNULL(T.BAGNO,'')= ''"
                    strSql += vbCrLf + " AND ISNULL(T.TRFNO,'')= ''"
                    strSql += vbCrLf + " AND  EXISTS (SELECT 1 FROM " & cnStockDb & "..ISSMETAL WHERE ISSSNO =T.SNO) "
                End If
                If preTranDb <> Nothing Then
                    strSql += vbCrLf + " UNION ALL "
                    strSql += vbCrLf + " SELECT T.TRANDATE,T.TRANNO,T.TRANTYPE"

                    If NeedItemType_accpost Then
                        strSql += vbCrLf + " ,CASE WHEN ISNULL(T.ITEMTYPEID,0)<>0 THEN CASE WHEN ISNULL((SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =T.ITEMTYPEID),'') <>''"
                        strSql += vbCrLf + " THEN (SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =T.ITEMTYPEID) ELSE T.CATCODE END ELSE  T.CATCODE END CATCODE "
                    Else
                        strSql += vbCrLf + " ,T.CATCODE"
                    End If
                    strSql += vbCrLf + " ,T.METALID,T.ITEMID"
                    strSql += vbCrLf + " ,T.TAGPCS-T.PCS PCS,T.TAGGRSWT-T.GRSWT GRSWT,T.TAGNETWT-T.NETWT NETWT "
                    strSql += vbCrLf + " ,CONVERT(VARCHAR(7),NULL) STNCATCODE"
                    strSql += vbCrLf + " ,CONVERT(INT,NULL)DIAPCS,CONVERT(NUMERIC(15,3),NULL)DIAWT"
                    strSql += vbCrLf + " ,CONVERT(INT,NULL)STNPCS,CONVERT(NUMERIC(15,3),NULL)STNWT"
                    strSql += vbCrLf + " ,CONVERT(INT,NULL)PREPCS,CONVERT(NUMERIC(15,3),NULL)PREWT"
                    strSql += vbCrLf + " ,T.SNO,T.BATCHNO"
                    strSql += vbCrLf + " ,RATE,CONVERT(NUMERIC(20,2),NULL)STNAMT"
                    'strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),NULL) AMOUNT"
                    If PENDTRF_AMOUNT_VISIBLE = "N" Then
                        strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),NULL) AMOUNT"
                    Else
                        strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),AMOUNT) AMOUNT"
                    End If
                    strSql += vbCrLf + " ,'I' AS TYPE,'A' COLHEAD"
                    strSql += vbCrLf + " FROM " & preTranDb & "..ISSUE T "
                    If PENDTRF_ASON Then
                        strSql += vbCrLf + " WHERE T.TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                    Else
                        strSql += vbCrLf + " WHERE T.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                    End If
                    strSql += vbCrLf + " AND T.TRANTYPE IN('SA','RD','OD') AND ISNULL(T.CANCEL,'') = ''"
                    strSql += vbCrLf + " AND (T.TAGPCS <> T.PCS OR T.TAGGRSWT <> T.GRSWT )"
                    strSql += vbCrLf + " AND (T.TAGPCS <> 0 OR T.TAGGRSWT <> 0 )"
                    If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND ISNULL(T.COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                    If CmbMetal_MAN.Text <> "" And CmbMetal_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND T.METALID=(SELECT TOP 1 METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & CmbMetal_MAN.Text & "')"
                    If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND T.COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                    If chkCmbCategory.Text <> "" And chkCmbCategory.Text <> "ALL" Then strSql += vbCrLf + " AND T.CATCODE=(SELECT TOP 1 CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & GetQryString(chkCmbCategory.Text) & "))"
                    'strSql += vbCrLf + " AND T.COMPANYID = '" & strCompanyId & "'"
                    strSql += vbCrLf + " AND ISNULL(T.BAGNO,'')= ''"
                    strSql += vbCrLf + " AND ISNULL(T.TRFNO,'')= ''"
                    If MetalBasedStone Then '' added for vbj on 04-06-2022 as per magesh sir advice
                        strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & preTranDb & "..ISSMETAL WHERE ISSSNO =T.SNO) "
                        strSql += vbCrLf + " UNION ALL "
                        strSql += vbCrLf + " SELECT T.TRANDATE,T.TRANNO,T.TRANTYPE,TIM.CATCODE"
                        strSql += vbCrLf + " ,(SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=TIM.CATCODE) METALID"
                        strSql += vbCrLf + " ,0 ITEMID,0 PCS"
                        strSql += vbCrLf + " ,(SELECT GRSWT FROM " & cnAdminDb & "..ITEMTAGMETAL  WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID=T.ITEMID And TAGNO=T.TAGNO) "
                        strSql += vbCrLf + " And CATCODE =TIM.CATCODE)-TIM.GRSWT GRSWT"
                        strSql += vbCrLf + " ,(SELECT CASE WHEN ISNULL(NETWT,0)<>0 THEN NETWT ELSE GRSWT END FROM " & cnAdminDb & "..ITEMTAGMETAL  WHERE TAGSNO IN  "
                        strSql += vbCrLf + " (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID=T.ITEMID And TAGNO=T.TAGNO) And CATCODE =TIM.CATCODE) "
                        strSql += vbCrLf + " - (CASE WHEN ISNULL(TIM.NETWT,0)<>0 THEN TIM.NETWT ELSE TIM.GRSWT END) NETWT "
                        strSql += vbCrLf + " ,CONVERT(VARCHAR(7),NULL) STNCATCODE"
                        strSql += vbCrLf + " ,CONVERT(INT,NULL)DIAPCS,CONVERT(NUMERIC(15,3),NULL)DIAWT"
                        strSql += vbCrLf + " ,CONVERT(INT,NULL)STNPCS,CONVERT(NUMERIC(15,3),NULL)STNWT"
                        strSql += vbCrLf + " ,CONVERT(INT,NULL)PREPCS,CONVERT(NUMERIC(15,3),NULL)PREWT"
                        strSql += vbCrLf + " ,T.SNO,T.BATCHNO"
                        strSql += vbCrLf + " ,(" & cnAdminDb & ".dbo.GET_TODAY_RATE(TIM.CATCODE,'" & dtpFrom.Value.ToString("yyyy-MM-dd") & "')) RATE,CONVERT(NUMERIC(20,2),NULL)STNAMT"
                        If PENDTRF_AMOUNT_VISIBLE = "N" Then
                            strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),NULL) AMOUNT"
                        Else
                            strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),TIM.AMOUNT) AMOUNT"
                        End If
                        strSql += vbCrLf + " ,'I' AS TYPE,'A' COLHEAD"
                        strSql += vbCrLf + " FROM " & preTranDb & "..ISSMETAL TIM "
                        strSql += vbCrLf + " INNER JOIN " & preTranDb & "..ISSUE T ON TIM.ISSSNO =T.SNO"
                        If PENDTRF_ASON Then
                            strSql += vbCrLf + " WHERE T.TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                        Else
                            strSql += vbCrLf + " WHERE T.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                        End If
                        strSql += vbCrLf + " AND T.TRANTYPE IN('SA','RD','OD') AND ISNULL(T.CANCEL,'') = ''"
                        strSql += vbCrLf + " AND (T.TAGPCS <> T.PCS OR T.TAGGRSWT <> T.GRSWT )"
                        strSql += vbCrLf + " AND (T.TAGPCS <> 0 OR T.TAGGRSWT <> 0 )"

                        strSql += vbCrLf + " AND (((SELECT GRSWT FROM " & cnAdminDb & "..ITEMTAGMETAL  WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID=T.ITEMID And TAGNO=T.TAGNO) "
                        strSql += vbCrLf + " And CATCODE =TIM.CATCODE)-TIM.GRSWT) <> 0"
                        strSql += vbCrLf + " OR ((SELECT CASE WHEN ISNULL(NETWT,0)<>0 THEN NETWT ELSE GRSWT END FROM " & cnAdminDb & "..ITEMTAGMETAL  WHERE TAGSNO IN  "
                        strSql += vbCrLf + " (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID=T.ITEMID And TAGNO=T.TAGNO) And CATCODE =TIM.CATCODE)"
                        strSql += vbCrLf + " - (CASE WHEN ISNULL(TIM.NETWT,0)<>0 THEN TIM.NETWT ELSE TIM.GRSWT END )) <> 0) "

                        If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND ISNULL(T.COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                        If CmbMetal_MAN.Text <> "" And CmbMetal_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND T.METALID=(SELECT TOP 1 METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & CmbMetal_MAN.Text & "')"
                        If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND T.COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                        If chkCmbCategory.Text <> "" And chkCmbCategory.Text <> "ALL" Then strSql += vbCrLf + " AND T.CATCODE=(SELECT TOP 1 CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & GetQryString(chkCmbCategory.Text) & "))"
                        'strSql += vbCrLf + " AND T.COMPANYID = '" & strCompanyId & "'"
                        strSql += vbCrLf + " AND ISNULL(T.BAGNO,'')= ''"
                        strSql += vbCrLf + " AND ISNULL(T.TRFNO,'')= ''"
                        strSql += vbCrLf + " AND  EXISTS (SELECT 1 FROM " & preTranDb & "..ISSMETAL WHERE ISSSNO =T.SNO) "
                    End If
                End If
                flag = True
            End If
            ''SALES RETURN
            If ChkSr.Checked Then
                If flag Then strSql += vbCrLf + " UNION ALL "
                strSql += vbCrLf + " SELECT T.TRANDATE,T.TRANNO,TRANTYPE"

                If NeedItemType_accpost Then
                    strSql += vbCrLf + " ,CASE WHEN ISNULL(T.ITEMTYPEID,0)<>0 THEN CASE WHEN ISNULL((SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =T.ITEMTYPEID),'') <>''"
                    strSql += vbCrLf + " THEN (SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =T.ITEMTYPEID) ELSE T.CATCODE END ELSE  T.CATCODE END CATCODE "
                Else
                    strSql += vbCrLf + " ,T.CATCODE"
                End If
                strSql += vbCrLf + " ,T.METALID,T.ITEMID"
                strSql += vbCrLf + " ,T.PCS,T.GRSWT,T.NETWT"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(7),NULL) STNCATCODE"
                strSql += vbCrLf + " ,CONVERT(INT,NULL)DIAPCS,CONVERT(NUMERIC(15,3),NULL)DIAWT"
                strSql += vbCrLf + " ,CONVERT(INT,NULL)STNPCS,CONVERT(NUMERIC(15,3),NULL)STNWT"
                strSql += vbCrLf + " ,CONVERT(INT,NULL)PREPCS,CONVERT(NUMERIC(15,3),NULL)PREWT"
                strSql += vbCrLf + " ,T.SNO,T.BATCHNO"
                strSql += vbCrLf + " ,RATE"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),ISNULL(STNAMT,0)) STNAMT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),AMOUNT) AMOUNT"
                strSql += vbCrLf + " ,'R' AS TYPE,'A' COLHEAD"
                strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT T "
                If PENDTRF_ASON Then
                    strSql += vbCrLf + " WHERE TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                Else
                    strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                End If
                strSql += vbCrLf + " AND TRANTYPE = 'SR' "
                strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
                If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                If CmbMetal_MAN.Text <> "" And CmbMetal_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND T.METALID=(SELECT TOP 1 METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & CmbMetal_MAN.Text & "')"
                If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND T.COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                If chkCmbCategory.Text <> "" And chkCmbCategory.Text <> "ALL" Then strSql += vbCrLf + " AND T.CATCODE=(SELECT TOP 1 CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & GetQryString(chkCmbCategory.Text) & "))"
                'strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                strSql += vbCrLf + " AND ISNULL(T.BAGNO,'')= ''"
                strSql += vbCrLf + " AND ISNULL(T.TRFNO,'')= ''"
                If MetalBasedStone Then '' added for vbj on 04-06-2022 as per magesh sir advice
                    strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnStockDb & "..RECEIPTMETAL WHERE ISSSNO =T.SNO) "
                    strSql += vbCrLf + " UNION ALL "
                    strSql += vbCrLf + " SELECT T.TRANDATE,T.TRANNO,T.TRANTYPE,TIM.CATCODE"
                    strSql += vbCrLf + " ,(SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=TIM.CATCODE) METALID"
                    strSql += vbCrLf + " ,0 ITEMID,0 PCS,TIM.GRSWT,TIM.NETWT"
                    strSql += vbCrLf + " ,CONVERT(VARCHAR(7),NULL) STNCATCODE"
                    strSql += vbCrLf + " ,CONVERT(INT,NULL)DIAPCS,CONVERT(NUMERIC(15,3),NULL)DIAWT"
                    strSql += vbCrLf + " ,CONVERT(INT,NULL)STNPCS,CONVERT(NUMERIC(15,3),NULL)STNWT"
                    strSql += vbCrLf + " ,CONVERT(INT,NULL)PREPCS,CONVERT(NUMERIC(15,3),NULL)PREWT"
                    strSql += vbCrLf + " ,T.SNO,T.BATCHNO"
                    strSql += vbCrLf + " ,(" & cnAdminDb & ".dbo.GET_TODAY_RATE(TIM.CATCODE,'" & dtpFrom.Value.ToString("yyyy-MM-dd") & "')) RATE"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),NULL) STNAMT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),TIM.AMOUNT) AMOUNT"
                    strSql += vbCrLf + " ,'R' AS TYPE,'A' COLHEAD"
                    strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTMETAL TIM "
                    strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..RECEIPT T ON TIM.ISSSNO =T.SNO"
                    If PENDTRF_ASON Then
                        strSql += vbCrLf + " WHERE T.TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                    Else
                        strSql += vbCrLf + " WHERE T.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                    End If
                    strSql += vbCrLf + " AND T.TRANTYPE = 'SR' "
                    strSql += vbCrLf + " AND ISNULL(T.CANCEL,'') = ''"
                    If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND ISNULL(T.COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                    If CmbMetal_MAN.Text <> "" And CmbMetal_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND T.METALID=(SELECT TOP 1 METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & CmbMetal_MAN.Text & "')"
                    If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND T.COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                    If chkCmbCategory.Text <> "" And chkCmbCategory.Text <> "ALL" Then strSql += vbCrLf + " AND T.CATCODE=(SELECT TOP 1 CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & GetQryString(chkCmbCategory.Text) & "))"
                    'strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                    strSql += vbCrLf + " AND ISNULL(T.BAGNO,'')= ''"
                    strSql += vbCrLf + " AND ISNULL(T.TRFNO,'')= ''"
                    strSql += vbCrLf + " AND EXISTS (SELECT 1 FROM " & cnStockDb & "..RECEIPTMETAL WHERE ISSSNO =T.SNO) "
                End If
                If preTranDb <> Nothing Then
                    strSql += vbCrLf + " UNION ALL "
                    strSql += vbCrLf + " SELECT T.TRANDATE,T.TRANNO,TRANTYPE"
                    If NeedItemType_accpost Then
                        strSql += vbCrLf + " ,CASE WHEN ISNULL(T.ITEMTYPEID,0)<>0 THEN CASE WHEN ISNULL((SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =T.ITEMTYPEID),'') <>''"
                        strSql += vbCrLf + " THEN (SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =T.ITEMTYPEID) ELSE T.CATCODE END ELSE  T.CATCODE END CATCODE "
                    Else
                        strSql += vbCrLf + " ,T.CATCODE"
                    End If
                    strSql += vbCrLf + " ,T.METALID,T.ITEMID"
                    strSql += vbCrLf + " ,T.PCS,T.GRSWT,T.NETWT"
                    strSql += vbCrLf + " ,CONVERT(VARCHAR(7),NULL) STNCATCODE"
                    strSql += vbCrLf + " ,CONVERT(INT,NULL)DIAPCS,CONVERT(NUMERIC(15,3),NULL)DIAWT"
                    strSql += vbCrLf + " ,CONVERT(INT,NULL)STNPCS,CONVERT(NUMERIC(15,3),NULL)STNWT"
                    strSql += vbCrLf + " ,CONVERT(INT,NULL)PREPCS,CONVERT(NUMERIC(15,3),NULL)PREWT"
                    strSql += vbCrLf + " ,T.SNO,T.BATCHNO"
                    strSql += vbCrLf + " ,RATE"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),ISNULL(STNAMT,0)) STNAMT"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),AMOUNT) AMOUNT"
                    strSql += vbCrLf + " ,'R' AS TYPE,'A' COLHEAD"
                    strSql += vbCrLf + " FROM " & preTranDb & "..RECEIPT T "
                    If PENDTRF_ASON Then
                        strSql += vbCrLf + " WHERE TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                    Else
                        strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                    End If
                    strSql += vbCrLf + " AND TRANTYPE = 'SR' "
                    strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
                    If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                    If CmbMetal_MAN.Text <> "" And CmbMetal_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND T.METALID=(SELECT TOP 1 METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & CmbMetal_MAN.Text & "')"
                    If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND T.COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                    If chkCmbCategory.Text <> "" And chkCmbCategory.Text <> "ALL" Then strSql += vbCrLf + " AND T.CATCODE=(SELECT TOP 1 CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & GetQryString(chkCmbCategory.Text) & "))"
                    'strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                    strSql += vbCrLf + " AND ISNULL(T.BAGNO,'')= ''"
                    strSql += vbCrLf + " AND ISNULL(T.TRFNO,'')= ''"
                    If MetalBasedStone Then '' added for vbj on 04-06-2022 as per magesh sir advice
                        strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & preTranDb & "..RECEIPTMETAL WHERE ISSSNO =T.SNO) "
                        strSql += vbCrLf + " UNION ALL "
                        strSql += vbCrLf + " SELECT T.TRANDATE,T.TRANNO,T.TRANTYPE,TIM.CATCODE"
                        strSql += vbCrLf + " ,(SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=TIM.CATCODE) METALID"
                        strSql += vbCrLf + " ,0 ITEMID,0 PCS,TIM.GRSWT,TIM.NETWT"
                        strSql += vbCrLf + " ,CONVERT(VARCHAR(7),NULL) STNCATCODE"
                        strSql += vbCrLf + " ,CONVERT(INT,NULL)DIAPCS,CONVERT(NUMERIC(15,3),NULL)DIAWT"
                        strSql += vbCrLf + " ,CONVERT(INT,NULL)STNPCS,CONVERT(NUMERIC(15,3),NULL)STNWT"
                        strSql += vbCrLf + " ,CONVERT(INT,NULL)PREPCS,CONVERT(NUMERIC(15,3),NULL)PREWT"
                        strSql += vbCrLf + " ,T.SNO,T.BATCHNO"
                        strSql += vbCrLf + " ,(" & cnAdminDb & ".dbo.GET_TODAY_RATE(TIM.CATCODE,'" & dtpFrom.Value.ToString("yyyy-MM-dd") & "')) RATE"
                        strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),NULL) STNAMT"
                        strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),TIM.AMOUNT) AMOUNT"
                        strSql += vbCrLf + " ,'R' AS TYPE,'A' COLHEAD"
                        strSql += vbCrLf + " FROM " & preTranDb & "..RECEIPTMETAL TIM "
                        strSql += vbCrLf + " INNER JOIN " & preTranDb & "..RECEIPT T ON TIM.ISSSNO =T.SNO"
                        If PENDTRF_ASON Then
                            strSql += vbCrLf + " WHERE T.TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                        Else
                            strSql += vbCrLf + " WHERE T.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                        End If
                        strSql += vbCrLf + " AND T.TRANTYPE = 'SR' "
                        strSql += vbCrLf + " AND ISNULL(T.CANCEL,'') = ''"
                        If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND ISNULL(T.COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                        If CmbMetal_MAN.Text <> "" And CmbMetal_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND T.METALID=(SELECT TOP 1 METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & CmbMetal_MAN.Text & "')"
                        If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND T.COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                        If chkCmbCategory.Text <> "" And chkCmbCategory.Text <> "ALL" Then strSql += vbCrLf + " AND T.CATCODE=(SELECT TOP 1 CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & GetQryString(chkCmbCategory.Text) & "))"
                        'strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                        strSql += vbCrLf + " AND ISNULL(T.BAGNO,'')= ''"
                        strSql += vbCrLf + " AND ISNULL(T.TRFNO,'')= ''"
                        strSql += vbCrLf + " AND EXISTS (SELECT 1 FROM " & preTranDb & "..RECEIPTMETAL WHERE ISSSNO =T.SNO) "
                    End If
                End If
                flag = True
            End If
            ''PURCHASE
            If Chkpurchase.Checked = True Then
                If flag Then strSql += vbCrLf + " UNION ALL "
                strSql += vbCrLf + " SELECT T.TRANDATE,T.TRANNO,TRANTYPE,T.CATCODE,T.METALID,T.ITEMID"
                strSql += vbCrLf + " ,T.PCS,T.GRSWT,T.NETWT"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(7),NULL) STNCATCODE"
                strSql += vbCrLf + " ,CONVERT(INT,NULL)DIAPCS,CONVERT(NUMERIC(15,3),NULL)DIAWT"
                strSql += vbCrLf + " ,CONVERT(INT,NULL)STNPCS,CONVERT(NUMERIC(15,3),NULL)STNWT"
                strSql += vbCrLf + " ,CONVERT(INT,NULL)PREPCS,CONVERT(NUMERIC(15,3),NULL)PREWT"
                strSql += vbCrLf + " ,T.SNO,T.BATCHNO "
                strSql += vbCrLf + " ,RATE"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),ISNULL(STNAMT,0)) STNAMT"
                strSql += vbCrLf + " ,AMOUNT AS AMOUNT"
                strSql += vbCrLf + " ,'R' AS TYPE,'A' COLHEAD"
                strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT T "
                If PENDTRF_ASON Then
                    strSql += vbCrLf + " WHERE TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                Else
                    strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                End If
                strSql += vbCrLf + " AND TRANTYPE = 'PU' "
                strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
                If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                If CmbMetal_MAN.Text <> "" And CmbMetal_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND T.METALID=(SELECT TOP 1 METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & CmbMetal_MAN.Text & "')"
                If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND T.COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                If chkCmbCategory.Text <> "" And chkCmbCategory.Text <> "ALL" Then strSql += vbCrLf + " AND T.CATCODE=(SELECT TOP 1 CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & GetQryString(chkCmbCategory.Text) & "))"
                'strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                strSql += vbCrLf + " AND ISNULL(T.BAGNO,'')= ''"
                strSql += vbCrLf + " AND ISNULL(T.TRFNO,'')= ''"
                strSql += vbCrLf + " AND NOT EXISTS (SELECT * FROM " & cnStockDb & "..ISSUE WHERE RTRIM(LTRIM(REMARK1))='SECOND HAND SALES' AND ISNULL(CANCEL,'')='' AND RUNNO=T.SNO AND T.TRANTYPE='PU')"
                If TAG_DUMP_ACCODE <> "" Then
                    ''DIR PURCHASE
                    strSql += vbCrLf + " UNION ALL "
                    strSql += vbCrLf + " SELECT T.TRANDATE,T.TRANNO,TRANTYPE,T.CATCODE,T.METALID,T.ITEMID"
                    strSql += vbCrLf + " ,T.PCS,T.GRSWT,T.NETWT"
                    strSql += vbCrLf + " ,CONVERT(VARCHAR(7),NULL) STNCATCODE"
                    strSql += vbCrLf + " ,CONVERT(INT,NULL)DIAPCS,CONVERT(NUMERIC(15,3),NULL)DIAWT"
                    strSql += vbCrLf + " ,CONVERT(INT,NULL)STNPCS,CONVERT(NUMERIC(15,3),NULL)STNWT"
                    strSql += vbCrLf + " ,CONVERT(INT,NULL)PREPCS,CONVERT(NUMERIC(15,3),NULL)PREWT"
                    strSql += vbCrLf + " ,T.SNO,T.BATCHNO "
                    strSql += vbCrLf + " ,RATE"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),ISNULL(STNAMT,0)) STNAMT"
                    strSql += vbCrLf + " ,AMOUNT AS AMOUNT"
                    strSql += vbCrLf + " ,'R' AS TYPE,'A' COLHEAD"
                    strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT T "
                    If PENDTRF_ASON Then
                        strSql += vbCrLf + " WHERE TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                    Else
                        strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                    End If
                    strSql += vbCrLf + " AND TRANTYPE = 'RPU' "
                    strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
                    strSql += vbCrLf + " AND ACCODE='" & TAG_DUMP_ACCODE & "'"
                    If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                    If CmbMetal_MAN.Text <> "" And CmbMetal_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND T.METALID=(SELECT TOP 1 METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & CmbMetal_MAN.Text & "')"
                    If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND T.COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                    If chkCmbCategory.Text <> "" And chkCmbCategory.Text <> "ALL" Then strSql += vbCrLf + " AND T.CATCODE=(SELECT TOP 1 CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & GetQryString(chkCmbCategory.Text) & "))"
                    'strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                    strSql += vbCrLf + " AND ISNULL(T.BAGNO,'')= ''"
                    strSql += vbCrLf + " AND ISNULL(T.TRFNO,'')= ''"
                End If
                If preTranDb <> Nothing Then
                    strSql += vbCrLf + " UNION ALL "
                    strSql += vbCrLf + " SELECT T.TRANDATE,T.TRANNO,TRANTYPE,T.CATCODE,T.METALID,T.ITEMID"
                    strSql += vbCrLf + " ,T.PCS,T.GRSWT,T.NETWT"
                    strSql += vbCrLf + " ,CONVERT(VARCHAR(7),NULL) STNCATCODE"
                    strSql += vbCrLf + " ,CONVERT(INT,NULL)DIAPCS,CONVERT(NUMERIC(15,3),NULL)DIAWT"
                    strSql += vbCrLf + " ,CONVERT(INT,NULL)STNPCS,CONVERT(NUMERIC(15,3),NULL)STNWT"
                    strSql += vbCrLf + " ,CONVERT(INT,NULL)PREPCS,CONVERT(NUMERIC(15,3),NULL)PREWT"
                    strSql += vbCrLf + " ,T.SNO,T.BATCHNO "
                    strSql += vbCrLf + " ,RATE"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),ISNULL(STNAMT,0)) STNAMT"
                    strSql += vbCrLf + " ,AMOUNT AS AMOUNT"
                    strSql += vbCrLf + " ,'R' AS TYPE,'A' COLHEAD"
                    strSql += vbCrLf + " FROM " & preTranDb & "..RECEIPT T "
                    If PENDTRF_ASON Then
                        strSql += vbCrLf + " WHERE TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                    Else
                        strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                    End If
                    strSql += vbCrLf + " AND TRANTYPE = 'PU' "
                    strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
                    If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                    If CmbMetal_MAN.Text <> "" And CmbMetal_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND T.METALID=(SELECT TOP 1 METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & CmbMetal_MAN.Text & "')"
                    If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND T.COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                    If chkCmbCategory.Text <> "" And chkCmbCategory.Text <> "ALL" Then strSql += vbCrLf + " AND T.CATCODE=(SELECT TOP 1 CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & GetQryString(chkCmbCategory.Text) & "))"
                    'strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                    strSql += vbCrLf + " AND ISNULL(T.BAGNO,'')= ''"
                    strSql += vbCrLf + " AND ISNULL(T.TRFNO,'')= ''"
                    If TAG_DUMP_ACCODE <> "" Then
                        ''DIR PURCHASE
                        strSql += vbCrLf + " UNION ALL "
                        strSql += vbCrLf + " SELECT T.TRANDATE,T.TRANNO,TRANTYPE,T.CATCODE,T.METALID,T.ITEMID"
                        strSql += vbCrLf + " ,T.PCS,T.GRSWT,T.NETWT"
                        strSql += vbCrLf + " ,CONVERT(VARCHAR(7),NULL) STNCATCODE"
                        strSql += vbCrLf + " ,CONVERT(INT,NULL)DIAPCS,CONVERT(NUMERIC(15,3),NULL)DIAWT"
                        strSql += vbCrLf + " ,CONVERT(INT,NULL)STNPCS,CONVERT(NUMERIC(15,3),NULL)STNWT"
                        strSql += vbCrLf + " ,CONVERT(INT,NULL)PREPCS,CONVERT(NUMERIC(15,3),NULL)PREWT"
                        strSql += vbCrLf + " ,T.SNO,T.BATCHNO "
                        strSql += vbCrLf + " ,RATE"
                        strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),ISNULL(STNAMT,0)) STNAMT"
                        strSql += vbCrLf + " ,AMOUNT AS AMOUNT"
                        strSql += vbCrLf + " ,'R' AS TYPE,'A' COLHEAD"
                        strSql += vbCrLf + " FROM " & preTranDb & "..RECEIPT T "
                        If PENDTRF_ASON Then
                            strSql += vbCrLf + " WHERE TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                        Else
                            strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                        End If
                        strSql += vbCrLf + " AND TRANTYPE = 'RPU' "
                        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
                        strSql += vbCrLf + " AND ACCODE='" & TAG_DUMP_ACCODE & "'"
                        If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                        If CmbMetal_MAN.Text <> "" And CmbMetal_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND T.METALID=(SELECT TOP 1 METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & CmbMetal_MAN.Text & "')"
                        If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND T.COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                        If chkCmbCategory.Text <> "" And chkCmbCategory.Text <> "ALL" Then strSql += vbCrLf + " AND T.CATCODE=(SELECT TOP 1 CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & GetQryString(chkCmbCategory.Text) & "))"
                        'strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                        strSql += vbCrLf + " AND ISNULL(T.BAGNO,'')= ''"
                        strSql += vbCrLf + " AND ISNULL(T.TRFNO,'')= ''"
                    End If
                End If
                flag = True
            End If
            ''MIS ISSUE
            If ChkWithMisc.Checked Then
                If flag Then strSql += vbCrLf + " UNION ALL "
                strSql += vbCrLf + " SELECT T.TRANDATE,T.TRANNO,TRANTYPE,T.CATCODE,T.METALID,T.ITEMID"
                strSql += vbCrLf + " ,T.PCS,T.GRSWT,T.NETWT"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(7),NULL) STNCATCODE"
                strSql += vbCrLf + " ,CONVERT(INT,NULL)DIAPCS,CONVERT(NUMERIC(15,3),NULL)DIAWT"
                strSql += vbCrLf + " ,CONVERT(INT,NULL)STNPCS,CONVERT(NUMERIC(15,3),NULL)STNWT"
                strSql += vbCrLf + " ,CONVERT(INT,NULL)PREPCS,CONVERT(NUMERIC(15,3),NULL)PREWT"
                strSql += vbCrLf + " ,T.SNO,T.BATCHNO "
                strSql += vbCrLf + " ,RATE,CONVERT(NUMERIC(20,2),NULL) STNAMT"
                'strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),NULL) AMOUNT"
                If PENDTRF_AMOUNT_VISIBLE = "N" Then
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),NULL) AMOUNT"
                Else
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),AMOUNT) AMOUNT"
                End If
                strSql += vbCrLf + " ,'I' AS TYPE,'A' COLHEAD"
                strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE T "
                If PENDTRF_ASON Then
                    strSql += vbCrLf + " WHERE TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                Else
                    strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                End If
                strSql += vbCrLf + " AND TRANTYPE ='MI' "
                strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
                If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                If CmbMetal_MAN.Text <> "" And CmbMetal_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND T.METALID=(SELECT TOP 1 METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & CmbMetal_MAN.Text & "')"
                If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND T.COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                If chkCmbCategory.Text <> "" And chkCmbCategory.Text <> "ALL" Then strSql += vbCrLf + " AND T.CATCODE=(SELECT TOP 1 CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & GetQryString(chkCmbCategory.Text) & "))"
                'strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                strSql += vbCrLf + " AND ISNULL(T.BAGNO,'')= ''"
                strSql += vbCrLf + " AND ISNULL(T.TRFNO,'')= ''"
                If preTranDb <> Nothing Then
                    strSql += vbCrLf + " UNION ALL "
                    strSql += vbCrLf + " SELECT T.TRANDATE,T.TRANNO,TRANTYPE,T.CATCODE,T.METALID,T.ITEMID"
                    strSql += vbCrLf + " ,T.PCS,T.GRSWT,T.NETWT"
                    strSql += vbCrLf + " ,CONVERT(VARCHAR(7),NULL) STNCATCODE"
                    strSql += vbCrLf + " ,CONVERT(INT,NULL)DIAPCS,CONVERT(NUMERIC(15,3),NULL)DIAWT"
                    strSql += vbCrLf + " ,CONVERT(INT,NULL)STNPCS,CONVERT(NUMERIC(15,3),NULL)STNWT"
                    strSql += vbCrLf + " ,CONVERT(INT,NULL)PREPCS,CONVERT(NUMERIC(15,3),NULL)PREWT"
                    strSql += vbCrLf + " ,T.SNO,T.BATCHNO "
                    strSql += vbCrLf + " ,RATE,CONVERT(NUMERIC(20,2),NULL) STNAMT"
                    'strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),NULL) AMOUNT"
                    If PENDTRF_AMOUNT_VISIBLE = "N" Then
                        strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),NULL) AMOUNT"
                    Else
                        strSql += vbCrLf + " ,CONVERT(NUMERIC(20,2),AMOUNT) AMOUNT"
                    End If
                    strSql += vbCrLf + " ,'I' AS TYPE,'A' COLHEAD"
                    strSql += vbCrLf + " FROM " & preTranDb & "..ISSUE T "
                    If PENDTRF_ASON Then
                        strSql += vbCrLf + " WHERE TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                    Else
                        strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                    End If
                    strSql += vbCrLf + " AND TRANTYPE ='MI' "
                    strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
                    If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                    If CmbMetal_MAN.Text <> "" And CmbMetal_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND T.METALID=(SELECT TOP 1 METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & CmbMetal_MAN.Text & "')"
                    If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND T.COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                    If chkCmbCategory.Text <> "" And chkCmbCategory.Text <> "ALL" Then strSql += vbCrLf + " AND T.CATCODE=(SELECT TOP 1 CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & GetQryString(chkCmbCategory.Text) & "))"
                    'strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                    strSql += vbCrLf + " AND ISNULL(T.BAGNO,'')= ''"
                    strSql += vbCrLf + " AND ISNULL(T.TRFNO,'')= ''"
                End If
            End If
            strSql += vbCrLf + ")X "
        End If
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "PENDTRANS SET AMOUNT=(RATE*GRSWT) WHERE TRANTYPE NOT IN('PU','SR','RPU')"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        flag = False
        If ChkWithMisc.Checked Or ChKSAL.Checked Then
            strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "PENDTRANS"
            strSql += vbCrLf + " SELECT TRANDATE,TRANNO,TRANTYPE"
            strSql += vbCrLf + " ,ORNCATCODE"
            strSql += vbCrLf + " ,METALID,STNITEMID,NULL,NULL,NULL,CATCODE"
            strSql += vbCrLf + " ,SUM(DIAPCS)DIAPCS,SUM(DIAWT)DIAWT  "
            strSql += vbCrLf + " ,SUM(STNPCS)STNPCS,SUM(STNWT)STNWT"
            strSql += vbCrLf + " ,SUM(PREPCS)PREPCS,SUM(PREWT)PREWT"
            strSql += vbCrLf + " ,SNO,BATCHNO,STNRATE,STNAMT,NULL,TYPE,COLHEAD"
            strSql += vbCrLf + " FROM ( "
            If ChKSAL.Checked Then
                strSql += vbCrLf + " SELECT ISSSNO SNO,BATCHNO,TRANDATE,TRANNO,TRANTYPE,'T' METALID,CATCODE,STNITEMID "
                strSql += vbCrLf + " ,TAGSTNPCS-STNPCS STNPCS"
                strSql += vbCrLf + " ,TAGSTNWT - STNWT STNWT"
                strSql += vbCrLf + " ,CONVERT(DECIMAL,NULL) DIAPCS,CONVERT(DECIMAL,NULL) DIAWT "
                strSql += vbCrLf + " ,CONVERT(DECIMAL,NULL) PREPCS,CONVERT(DECIMAL,NULL) PREWT "
                strSql += vbCrLf + " ,STNRATE,(TAGSTNWT - STNWT)*STNRATE AS STNAMT,'I' AS TYPE,'S' COLHEAD"
                strSql += vbCrLf + " ,(SELECT TOP 1 CATCODE FROM " & cnStockDb & "..ISSUE WHERE SNO=I.ISSSNO)AS ORNCATCODE"
                strSql += vbCrLf + " FROM " & cnStockDb & "..ISSSTONE I"
                If PENDTRF_ASON Then
                    strSql += vbCrLf + " WHERE TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                Else
                    strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                End If
                strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS WHERE TYPE='I') "
                strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE ='S')"
                strSql += vbCrLf + " AND (TAGSTNPCS <> STNPCS OR TAGSTNWT <> STNWT)"
                strSql += vbCrLf + " AND (TAGSTNPCS <> 0 OR TAGSTNWT <> 0)"
                strSql += vbCrLf + " AND TAGSTNWT - STNWT > 0"
                If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                'strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                strSql += vbCrLf + " UNION ALL "
                strSql += vbCrLf + " SELECT ISSSNO SNO,BATCHNO,TRANDATE,TRANNO,TRANTYPE,'D' METALID,CATCODE,STNITEMID ,0 STNPCS,0 STNWT"
                strSql += vbCrLf + " ,TAGSTNPCS-STNPCS DIAPCS"
                strSql += vbCrLf + " ,TAGSTNWT - STNWT DIAWT"
                strSql += vbCrLf + " ,CONVERT(DECIMAL,NULL) PREPCS,CONVERT(DECIMAL,NULL) PREWT "
                strSql += vbCrLf + " ,STNRATE,(TAGSTNWT - STNWT)*STNRATE AS STNAMT,'I' AS TYPE,'D' COLHEAD"
                strSql += vbCrLf + " ,(SELECT TOP 1 CATCODE FROM " & cnStockDb & "..ISSUE WHERE SNO=I.ISSSNO)AS ORNCATCODE"
                strSql += vbCrLf + " FROM " & cnStockDb & "..ISSSTONE I"
                If PENDTRF_ASON Then
                    strSql += vbCrLf + " WHERE TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                Else
                    strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                End If
                strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS WHERE TYPE='I') "
                strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE ='D')"
                strSql += vbCrLf + " AND (TAGSTNPCS <> STNPCS OR TAGSTNWT <> STNWT)"
                strSql += vbCrLf + " AND (TAGSTNPCS <> 0 OR TAGSTNWT <> 0)"
                strSql += vbCrLf + " AND TAGSTNWT - STNWT > 0"
                If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                'strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                strSql += vbCrLf + " UNION ALL "
                strSql += vbCrLf + " SELECT ISSSNO SNO,BATCHNO,TRANDATE,TRANNO,TRANTYPE,'P' METALID,CATCODE,STNITEMID ,0 STNPCS,0 STNWT,0 DIAPCS,0 DIAWT"
                strSql += vbCrLf + " ,TAGSTNPCS-STNPCS PREPCS"
                strSql += vbCrLf + " ,TAGSTNWT - STNWT PREWT"
                strSql += vbCrLf + " ,STNRATE,(TAGSTNWT - STNWT)*STNRATE AS STNAMT,'I' AS TYPE,'P' COLHEAD"
                strSql += vbCrLf + " ,(SELECT TOP 1 CATCODE FROM " & cnStockDb & "..ISSUE WHERE SNO=I.ISSSNO)AS ORNCATCODE"
                strSql += vbCrLf + " FROM " & cnStockDb & "..ISSSTONE I"
                If PENDTRF_ASON Then
                    strSql += vbCrLf + " WHERE TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                Else
                    strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                End If
                strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS WHERE TYPE='I') "
                strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE ='P')"
                strSql += vbCrLf + " AND (TAGSTNPCS <> STNPCS OR TAGSTNWT <> STNWT)"
                strSql += vbCrLf + " AND (TAGSTNPCS <> 0 OR TAGSTNWT <> 0)"
                strSql += vbCrLf + " AND TAGSTNWT - STNWT > 0"
                If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                'strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                If preTranDb <> Nothing Then
                    strSql += vbCrLf + " UNION ALL "
                    strSql += vbCrLf + " SELECT ISSSNO SNO,BATCHNO,TRANDATE,TRANNO,TRANTYPE,'T' METALID,CATCODE,STNITEMID "
                    strSql += vbCrLf + " ,TAGSTNPCS-STNPCS STNPCS"
                    strSql += vbCrLf + " ,TAGSTNWT - STNWT STNWT"
                    strSql += vbCrLf + " ,CONVERT(DECIMAL,NULL) DIAPCS,CONVERT(DECIMAL,NULL) DIAWT "
                    strSql += vbCrLf + " ,CONVERT(DECIMAL,NULL) PREPCS,CONVERT(DECIMAL,NULL) PREWT "
                    strSql += vbCrLf + " ,STNRATE,(TAGSTNWT - STNWT)*STNRATE AS STNAMT,'I' AS TYPE,'S' COLHEAD"
                    strSql += vbCrLf + " ,(SELECT TOP 1 CATCODE FROM " & preTranDb & "..ISSUE WHERE SNO=I.ISSSNO)AS ORNCATCODE"
                    strSql += vbCrLf + " FROM " & preTranDb & "..ISSSTONE I"
                    If PENDTRF_ASON Then
                        strSql += vbCrLf + " WHERE TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                    Else
                        strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                    End If
                    strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS WHERE TYPE='I') "
                    strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE ='S')"
                    strSql += vbCrLf + " AND (TAGSTNPCS <> STNPCS OR TAGSTNWT <> STNWT)"
                    strSql += vbCrLf + " AND (TAGSTNPCS <> 0 OR TAGSTNWT <> 0)"
                    strSql += vbCrLf + " AND TAGSTNWT - STNWT > 0"
                    If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                    If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                    'strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                    strSql += vbCrLf + " UNION ALL "
                    strSql += vbCrLf + " SELECT ISSSNO SNO,BATCHNO,TRANDATE,TRANNO,TRANTYPE,'D' METALID,CATCODE,STNITEMID ,0 STNPCS,0 STNWT"
                    strSql += vbCrLf + " ,TAGSTNPCS-STNPCS DIAPCS"
                    strSql += vbCrLf + " ,TAGSTNWT - STNWT DIAWT"
                    strSql += vbCrLf + " ,CONVERT(DECIMAL,NULL) PREPCS,CONVERT(DECIMAL,NULL) PREWT "
                    strSql += vbCrLf + " ,STNRATE,(TAGSTNWT - STNWT)*STNRATE AS STNAMT,'I' AS TYPE,'D' COLHEAD"
                    strSql += vbCrLf + " ,(SELECT TOP 1 CATCODE FROM " & preTranDb & "..ISSUE WHERE SNO=I.ISSSNO)AS ORNCATCODE"
                    strSql += vbCrLf + " FROM " & preTranDb & "..ISSSTONE I"
                    If PENDTRF_ASON Then
                        strSql += vbCrLf + " WHERE TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                    Else
                        strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                    End If
                    strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS WHERE TYPE='I') "
                    strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE ='D')"
                    strSql += vbCrLf + " AND (TAGSTNPCS <> STNPCS OR TAGSTNWT <> STNWT)"
                    strSql += vbCrLf + " AND (TAGSTNPCS <> 0 OR TAGSTNWT <> 0)"
                    strSql += vbCrLf + " AND TAGSTNWT - STNWT > 0"
                    If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                    If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                    'strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                    strSql += vbCrLf + " UNION ALL "
                    strSql += vbCrLf + " SELECT ISSSNO SNO,BATCHNO,TRANDATE,TRANNO,TRANTYPE,'P' METALID,CATCODE,STNITEMID ,0 STNPCS,0 STNWT,0 DIAPCS,0 DIAWT"
                    strSql += vbCrLf + " ,TAGSTNPCS-STNPCS PREPCS"
                    strSql += vbCrLf + " ,TAGSTNWT - STNWT PREWT"
                    strSql += vbCrLf + " ,STNRATE,(TAGSTNWT - STNWT)*STNRATE AS STNAMT,'I' AS TYPE,'P' COLHEAD"
                    strSql += vbCrLf + " ,(SELECT TOP 1 CATCODE FROM " & preTranDb & "..ISSUE WHERE SNO=I.ISSSNO)AS ORNCATCODE"
                    strSql += vbCrLf + " FROM " & preTranDb & "..ISSSTONE I"
                    If PENDTRF_ASON Then
                        strSql += vbCrLf + " WHERE TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                    Else
                        strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                    End If
                    strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS WHERE TYPE='I') "
                    strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE ='P')"
                    strSql += vbCrLf + " AND (TAGSTNPCS <> STNPCS OR TAGSTNWT <> STNWT)"
                    strSql += vbCrLf + " AND (TAGSTNPCS <> 0 OR TAGSTNWT <> 0)"
                    strSql += vbCrLf + " AND TAGSTNWT - STNWT > 0"
                    If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                    If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                    'strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                End If
                flag = True
            End If
            If ChkWithMisc.Checked Then
                If flag Then strSql += vbCrLf + " UNION ALL "
                strSql += vbCrLf + " SELECT ISSSNO SNO,BATCHNO,TRANDATE,TRANNO,TRANTYPE,'T' AS METALID,CATCODE,STNITEMID "
                strSql += vbCrLf + " ,STNPCS"
                strSql += vbCrLf + " ,STNWT STNWT"
                strSql += vbCrLf + " ,CONVERT(DECIMAL,NULL) DIAPCS,CONVERT(DECIMAL,NULL) DIAWT "
                strSql += vbCrLf + " ,CONVERT(DECIMAL,NULL) PREPCS,CONVERT(DECIMAL,NULL) PREWT "
                strSql += vbCrLf + " ,STNRATE,NULL STNAMT,'I' AS TYPE,'S' AS COLHEAD"
                strSql += vbCrLf + " ,(SELECT TOP 1 CATCODE FROM " & cnStockDb & "..ISSUE WHERE SNO=I.ISSSNO)AS ORNCATCODE"
                strSql += vbCrLf + " FROM " & cnStockDb & "..ISSSTONE I"
                If PENDTRF_ASON Then
                    strSql += vbCrLf + " WHERE TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                Else
                    strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                End If
                strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS WHERE TYPE='I') "
                strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE ='S')"
                If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                'strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                strSql += vbCrLf + " UNION ALL "
                strSql += vbCrLf + " SELECT ISSSNO SNO,BATCHNO,TRANDATE,TRANNO,TRANTYPE,'D' AS METALID,CATCODE,STNITEMID ,0 STNPCS,0 STNWT"
                strSql += vbCrLf + " ,STNPCS DIAPCS"
                strSql += vbCrLf + " ,STNWT DIAWT"
                strSql += vbCrLf + " ,CONVERT(DECIMAL,NULL) PREPCS,CONVERT(DECIMAL,NULL) PREWT "
                strSql += vbCrLf + " ,STNRATE,NULL STNAMT,'I' AS TYPE,'D' AS COLHEAD"
                strSql += vbCrLf + " ,(SELECT TOP 1 CATCODE FROM " & cnStockDb & "..ISSUE WHERE SNO=I.ISSSNO)AS ORNCATCODE"
                strSql += vbCrLf + " FROM " & cnStockDb & "..ISSSTONE I"
                If PENDTRF_ASON Then
                    strSql += vbCrLf + " WHERE TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                Else
                    strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                End If
                strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS WHERE TYPE='I') "
                strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE ='D')"
                If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                'strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                strSql += vbCrLf + " UNION ALL "
                strSql += vbCrLf + " SELECT ISSSNO SNO,BATCHNO,TRANDATE,TRANNO,TRANTYPE,'P' AS METALID,CATCODE,STNITEMID ,0 STNPCS,0 STNWT,0 DIAPCS,0 DIAWT"
                strSql += vbCrLf + " ,STNPCS PREPCS"
                strSql += vbCrLf + " ,STNWT PREWT"
                strSql += vbCrLf + " ,STNRATE,NULL STNAMT,'I' AS TYPE,'P' AS COLHEAD"
                strSql += vbCrLf + " ,(SELECT TOP 1 CATCODE FROM " & cnStockDb & "..ISSUE WHERE SNO=I.ISSSNO)AS ORNCATCODE"
                strSql += vbCrLf + " FROM " & cnStockDb & "..ISSSTONE I"
                If PENDTRF_ASON Then
                    strSql += vbCrLf + " WHERE TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                Else
                    strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                End If
                strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS WHERE TYPE='I') "
                strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE ='P')"
                If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                'strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                If preTranDb <> Nothing Then
                    strSql += vbCrLf + " UNION ALL "
                    strSql += vbCrLf + " SELECT ISSSNO SNO,BATCHNO,TRANDATE,TRANNO,TRANTYPE,'T' AS METALID,CATCODE,STNITEMID "
                    strSql += vbCrLf + " ,STNPCS"
                    strSql += vbCrLf + " ,STNWT STNWT"
                    strSql += vbCrLf + " ,CONVERT(DECIMAL,NULL) DIAPCS,CONVERT(DECIMAL,NULL) DIAWT "
                    strSql += vbCrLf + " ,CONVERT(DECIMAL,NULL) PREPCS,CONVERT(DECIMAL,NULL) PREWT "
                    strSql += vbCrLf + " ,STNRATE,NULL STNAMT,'I' AS TYPE,'S' AS COLHEAD"
                    strSql += vbCrLf + " ,(SELECT TOP 1 CATCODE FROM " & preTranDb & "..ISSUE WHERE SNO=I.ISSSNO)AS ORNCATCODE"
                    strSql += vbCrLf + " FROM " & preTranDb & "..ISSSTONE I"
                    If PENDTRF_ASON Then
                        strSql += vbCrLf + " WHERE TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                    Else
                        strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                    End If
                    strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS WHERE TYPE='I') "
                    strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE ='S')"
                    If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                    If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                    'strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                    strSql += vbCrLf + " UNION ALL "
                    strSql += vbCrLf + " SELECT ISSSNO SNO,BATCHNO,TRANDATE,TRANNO,TRANTYPE,'D' AS METALID,CATCODE,STNITEMID ,0 STNPCS,0 STNWT"
                    strSql += vbCrLf + " ,STNPCS DIAPCS"
                    strSql += vbCrLf + " ,STNWT DIAWT"
                    strSql += vbCrLf + " ,CONVERT(DECIMAL,NULL) PREPCS,CONVERT(DECIMAL,NULL) PREWT "
                    strSql += vbCrLf + " ,STNRATE,NULL STNAMT,'I' AS TYPE,'D' AS COLHEAD"
                    strSql += vbCrLf + " ,(SELECT TOP 1 CATCODE FROM " & preTranDb & "..ISSUE WHERE SNO=I.ISSSNO)AS ORNCATCODE"
                    strSql += vbCrLf + " FROM " & preTranDb & "..ISSSTONE I"
                    strSql += vbCrLf + " WHERE TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                    strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS WHERE TYPE='I') "
                    strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE ='D')"
                    If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                    If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                    'strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                    strSql += vbCrLf + " UNION ALL "
                    strSql += vbCrLf + " SELECT ISSSNO SNO,BATCHNO,TRANDATE,TRANNO,TRANTYPE,'P' AS METALID,CATCODE,STNITEMID ,0 STNPCS,0 STNWT,0 DIAPCS,0 DIAWT"
                    strSql += vbCrLf + " ,STNPCS PREPCS"
                    strSql += vbCrLf + " ,STNWT PREWT"
                    strSql += vbCrLf + " ,STNRATE,NULL STNAMT,'I' AS TYPE,'P' AS COLHEAD"
                    strSql += vbCrLf + " ,(SELECT TOP 1 CATCODE FROM " & preTranDb & "..ISSUE WHERE SNO=I.ISSSNO)AS ORNCATCODE"
                    strSql += vbCrLf + " FROM " & preTranDb & "..ISSSTONE I"
                    If PENDTRF_ASON Then
                        strSql += vbCrLf + " WHERE TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                    Else
                        strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                    End If
                    strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS WHERE TYPE='I') "
                    strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE ='P')"
                    If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                    If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                    'strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                End If
            End If
            strSql += vbCrLf + " )X GROUP BY SNO,BATCHNO,TRANDATE,ORNCATCODE,CATCODE,TRANNO,TRANTYPE,STNRATE,STNAMT,TYPE,COLHEAD,METALID,STNITEMID"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            flag = False
        End If
        If ChkSr.Checked Or Chkpurchase.Checked Then
            strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "PENDTRANS"
            strSql += vbCrLf + " SELECT TRANDATE,TRANNO,TRANTYPE"
            strSql += vbCrLf + " ,ORNCATCODE"
            strSql += vbCrLf + " ,METALID,STNITEMID,NULL,NULL,NULL,CATCODE"
            strSql += vbCrLf + " ,SUM(DIAPCS)DIAPCS,SUM(DIAWT)DIAWT  "
            strSql += vbCrLf + " ,SUM(STNPCS)STNPCS,SUM(STNWT)STNWT"
            strSql += vbCrLf + " ,SUM(PREPCS)PREPCS,SUM(PREWT)PREWT"
            strSql += vbCrLf + " ,SNO,BATCHNO,STNRATE,STNAMT,NULL,TYPE,COLHEAD"
            strSql += vbCrLf + " FROM ( "
            ''SALES RETURN
            If ChkSr.Checked Then
                strSql += vbCrLf + " SELECT TRANDATE,TRANNO,TRANTYPE,'T' AS METALID,ISSSNO SNO,BATCHNO,STNITEMID,CATCODE"
                strSql += vbCrLf + " ,STNPCS STNPCS"
                strSql += vbCrLf + " ,STNWT STNWT"
                strSql += vbCrLf + " ,CONVERT(DECIMAL,NULL) DIAPCS,CONVERT(DECIMAL,NULL) DIAWT "
                strSql += vbCrLf + " ,CONVERT(DECIMAL,NULL) PREPCS,CONVERT(DECIMAL,NULL) PREWT "
                strSql += vbCrLf + " ,STNRATE,STNAMT,'R' AS TYPE,'S' AS COLHEAD"
                strSql += vbCrLf + " ,(SELECT TOP 1 CATCODE FROM " & cnStockDb & "..RECEIPT WHERE SNO=R.ISSSNO)AS ORNCATCODE"
                strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE R"
                If PENDTRF_ASON Then
                    strSql += vbCrLf + " WHERE TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                Else
                    strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                End If
                strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS WHERE TYPE='R' AND TRANTYPE='SR')"
                strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE ='S')"
                If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                'strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                strSql += vbCrLf + " UNION ALL "
                strSql += vbCrLf + " SELECT TRANDATE,TRANNO,TRANTYPE,'D' AS METALID,ISSSNO SNO,BATCHNO,STNITEMID,CATCODE,0 STNPCS ,0 STNWT"
                strSql += vbCrLf + " ,STNPCS DIAPCS"
                strSql += vbCrLf + " ,STNWT DIAWT"
                strSql += vbCrLf + " ,CONVERT(DECIMAL,NULL) PREPCS,CONVERT(DECIMAL,NULL) PREWT "
                strSql += vbCrLf + " ,STNRATE,STNAMT,'R' AS TYPE,'D' AS COLHEAD"
                strSql += vbCrLf + " ,(SELECT TOP 1 CATCODE FROM " & cnStockDb & "..RECEIPT WHERE SNO=R.ISSSNO)AS ORNCATCODE"
                strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE R"
                If PENDTRF_ASON Then
                    strSql += vbCrLf + " WHERE TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                Else
                    strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                End If
                strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS WHERE TYPE='R' AND TRANTYPE='SR')"
                strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE ='D')"
                If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                'strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                strSql += vbCrLf + " UNION ALL "
                strSql += vbCrLf + " SELECT TRANDATE,TRANNO,TRANTYPE,'P' AS METALID,ISSSNO SNO,BATCHNO,STNITEMID,CATCODE,0 STNPCS ,0 STNWT,0 DIAPCS ,0 DIAWT"
                strSql += vbCrLf + " ,STNPCS PREPCS"
                strSql += vbCrLf + " ,STNWT PREWT"
                strSql += vbCrLf + " ,STNRATE,STNAMT,'R' AS TYPE,'P' AS COLHEAD"
                strSql += vbCrLf + " ,(SELECT TOP 1 CATCODE FROM " & cnStockDb & "..RECEIPT WHERE SNO=R.ISSSNO)AS ORNCATCODE"
                strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE R"
                If PENDTRF_ASON Then
                    strSql += vbCrLf + " WHERE TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                Else
                    strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                End If
                strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS WHERE TYPE='R' AND TRANTYPE='SR')"
                strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE ='P')"
                If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                'strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                If preTranDb <> Nothing Then
                    strSql += vbCrLf + " UNION ALL "
                    strSql += vbCrLf + " SELECT TRANDATE,TRANNO,TRANTYPE,'T' AS METALID,ISSSNO SNO,BATCHNO,STNITEMID,CATCODE"
                    strSql += vbCrLf + " ,STNPCS STNPCS"
                    strSql += vbCrLf + " ,STNWT STNWT"
                    strSql += vbCrLf + " ,CONVERT(DECIMAL,NULL) DIAPCS,CONVERT(DECIMAL,NULL) DIAWT "
                    strSql += vbCrLf + " ,CONVERT(DECIMAL,NULL) PREPCS,CONVERT(DECIMAL,NULL) PREWT "
                    strSql += vbCrLf + " ,STNRATE,STNAMT,'R' AS TYPE,'S' AS COLHEAD"
                    strSql += vbCrLf + " ,(SELECT TOP 1 CATCODE FROM " & preTranDb & "..RECEIPT WHERE SNO=R.ISSSNO)AS ORNCATCODE"
                    strSql += vbCrLf + " FROM " & preTranDb & "..RECEIPTSTONE R"
                    If PENDTRF_ASON Then
                        strSql += vbCrLf + " WHERE TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                    Else
                        strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                    End If
                    strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS WHERE TYPE='R' AND TRANTYPE='SR')"
                    strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE ='S')"
                    If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                    If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                    'strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                    strSql += vbCrLf + " UNION ALL "
                    strSql += vbCrLf + " SELECT TRANDATE,TRANNO,TRANTYPE,'D' AS METALID,ISSSNO SNO,BATCHNO,STNITEMID,CATCODE,0 STNPCS ,0 STNWT"
                    strSql += vbCrLf + " ,STNPCS DIAPCS"
                    strSql += vbCrLf + " ,STNWT DIAWT"
                    strSql += vbCrLf + " ,CONVERT(DECIMAL,NULL) PREPCS,CONVERT(DECIMAL,NULL) PREWT "
                    strSql += vbCrLf + " ,STNRATE,STNAMT,'R' AS TYPE,'D' AS COLHEAD"
                    strSql += vbCrLf + " ,(SELECT TOP 1 CATCODE FROM " & preTranDb & "..RECEIPT WHERE SNO=R.ISSSNO)AS ORNCATCODE"
                    strSql += vbCrLf + " FROM " & preTranDb & "..RECEIPTSTONE R"
                    If PENDTRF_ASON Then
                        strSql += vbCrLf + " WHERE TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                    Else
                        strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                    End If
                    strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS WHERE TYPE='R' AND TRANTYPE='SR')"
                    strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE ='D')"
                    If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                    If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                    'strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                    strSql += vbCrLf + " UNION ALL "
                    strSql += vbCrLf + " SELECT TRANDATE,TRANNO,TRANTYPE,'P' AS METALID,ISSSNO SNO,BATCHNO,STNITEMID,CATCODE,0 STNPCS ,0 STNWT,0 DIAPCS ,0 DIAWT"
                    strSql += vbCrLf + " ,STNPCS PREPCS"
                    strSql += vbCrLf + " ,STNWT PREWT"
                    strSql += vbCrLf + " ,STNRATE,STNAMT,'R' AS TYPE,'P' AS COLHEAD"
                    strSql += vbCrLf + " ,(SELECT TOP 1 CATCODE FROM " & preTranDb & "..RECEIPT WHERE SNO=R.ISSSNO)AS ORNCATCODE"
                    strSql += vbCrLf + " FROM " & preTranDb & "..RECEIPTSTONE R"
                    If PENDTRF_ASON Then
                        strSql += vbCrLf + " WHERE TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                    Else
                        strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                    End If
                    strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS WHERE TYPE='R' AND TRANTYPE='SR')"
                    strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE ='P')"
                    If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                    If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                    'strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                End If
                flag = True
            End If
            ''PURCHASE
            If Chkpurchase.Checked Then
                If flag Then strSql += vbCrLf + " UNION ALL "
                strSql += vbCrLf + " SELECT TRANDATE,TRANNO,TRANTYPE,'T' AS METALID,ISSSNO SNO,BATCHNO,STNITEMID,CATCODE"
                strSql += vbCrLf + " ,STNPCS STNPCS"
                strSql += vbCrLf + " ,STNWT STNWT"
                strSql += vbCrLf + " ,CONVERT(DECIMAL,NULL) DIAPCS,CONVERT(DECIMAL,NULL) DIAWT "
                strSql += vbCrLf + " ,CONVERT(DECIMAL,NULL) PREPCS,CONVERT(DECIMAL,NULL) PREWT "
                strSql += vbCrLf + " ,STNRATE,STNAMT,'R' AS TYPE,'S' AS COLHEAD"
                strSql += vbCrLf + " ,(SELECT TOP 1 CATCODE FROM " & cnStockDb & "..RECEIPT WHERE SNO=R.ISSSNO)AS ORNCATCODE"
                strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE R"
                If PENDTRF_ASON Then
                    strSql += vbCrLf + " WHERE TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                Else
                    strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                End If
                strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS WHERE TYPE='R' AND TRANTYPE IN('PU','RPU'))"
                strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE ='S')"
                If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                'strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                strSql += vbCrLf + " UNION ALL "
                strSql += vbCrLf + " SELECT TRANDATE,TRANNO,TRANTYPE,'D' AS METALID,ISSSNO SNO,BATCHNO,STNITEMID,CATCODE,0 STNPCS ,0 STNWT"
                strSql += vbCrLf + " ,STNPCS DIAPCS"
                strSql += vbCrLf + " ,STNWT DIAWT"
                strSql += vbCrLf + " ,CONVERT(DECIMAL,NULL) PREPCS,CONVERT(DECIMAL,NULL) PREWT "
                strSql += vbCrLf + " ,STNRATE,STNAMT,'R' AS TYPE,'D' AS COLHEAD"
                strSql += vbCrLf + " ,(SELECT TOP 1 CATCODE FROM " & cnStockDb & "..RECEIPT WHERE SNO=R.ISSSNO)AS ORNCATCODE"
                strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE R"
                If PENDTRF_ASON Then
                    strSql += vbCrLf + " WHERE TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                Else
                    strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                End If
                strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS WHERE TYPE='R' AND TRANTYPE IN('PU','RPU'))"
                strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE ='D')"
                If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                'strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                strSql += vbCrLf + " UNION ALL "
                strSql += vbCrLf + " SELECT TRANDATE,TRANNO,TRANTYPE,'P' AS METALID,ISSSNO SNO,BATCHNO,STNITEMID,CATCODE,0 STNPCS ,0 STNWT,0 DIAPCS ,0 DIAWT"
                strSql += vbCrLf + " ,STNPCS PREPCS"
                strSql += vbCrLf + " ,STNWT PREWT"
                strSql += vbCrLf + " ,STNRATE,STNAMT,'R' AS TYPE,'P' AS COLHEAD"
                strSql += vbCrLf + " ,(SELECT TOP 1 CATCODE FROM " & cnStockDb & "..RECEIPT WHERE SNO=R.ISSSNO)AS ORNCATCODE"
                strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE R"
                If PENDTRF_ASON Then
                    strSql += vbCrLf + " WHERE TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                Else
                    strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                End If
                strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS WHERE TYPE='R' AND TRANTYPE IN('PU','RPU'))"
                strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE ='P')"
                If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                'strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                If preTranDb <> Nothing Then
                    strSql += vbCrLf + " UNION ALL "
                    strSql += vbCrLf + " SELECT TRANDATE,TRANNO,TRANTYPE,'T' AS METALID,ISSSNO SNO,BATCHNO,STNITEMID,CATCODE"
                    strSql += vbCrLf + " ,STNPCS STNPCS"
                    strSql += vbCrLf + " ,STNWT STNWT"
                    strSql += vbCrLf + " ,CONVERT(DECIMAL,NULL) DIAPCS,CONVERT(DECIMAL,NULL) DIAWT "
                    strSql += vbCrLf + " ,CONVERT(DECIMAL,NULL) PREPCS,CONVERT(DECIMAL,NULL) PREWT "
                    strSql += vbCrLf + " ,STNRATE,STNAMT,'R' AS TYPE,'S' AS COLHEAD"
                    strSql += vbCrLf + " ,(SELECT TOP 1 CATCODE FROM " & preTranDb & "..RECEIPT WHERE SNO=R.ISSSNO)AS ORNCATCODE"
                    strSql += vbCrLf + " FROM " & preTranDb & "..RECEIPTSTONE R"
                    If PENDTRF_ASON Then
                        strSql += vbCrLf + " WHERE TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                    Else
                        strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                    End If
                    strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS WHERE TYPE='R' AND TRANTYPE IN('PU','RPU'))"
                    strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE ='S')"
                    If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                    If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                    'strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                    strSql += vbCrLf + " UNION ALL "
                    strSql += vbCrLf + " SELECT TRANDATE,TRANNO,TRANTYPE,'D' AS METALID,ISSSNO SNO,BATCHNO,STNITEMID,CATCODE,0 STNPCS ,0 STNWT"
                    strSql += vbCrLf + " ,STNPCS DIAPCS"
                    strSql += vbCrLf + " ,STNWT DIAWT"
                    strSql += vbCrLf + " ,CONVERT(DECIMAL,NULL) PREPCS,CONVERT(DECIMAL,NULL) PREWT "
                    strSql += vbCrLf + " ,STNRATE,STNAMT,'R' AS TYPE,'D' AS COLHEAD"
                    strSql += vbCrLf + " ,(SELECT TOP 1 CATCODE FROM " & preTranDb & "..RECEIPT WHERE SNO=R.ISSSNO)AS ORNCATCODE"
                    strSql += vbCrLf + " FROM " & preTranDb & "..RECEIPTSTONE R"
                    If PENDTRF_ASON Then
                        strSql += vbCrLf + " WHERE TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                    Else
                        strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                    End If
                    strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS WHERE TYPE='R' AND TRANTYPE IN('PU','RPU'))"
                    strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE ='D')"
                    If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                    If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                    'strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                    strSql += vbCrLf + " UNION ALL "
                    strSql += vbCrLf + " SELECT TRANDATE,TRANNO,TRANTYPE,'P' AS METALID,ISSSNO SNO,BATCHNO,STNITEMID,CATCODE,0 STNPCS ,0 STNWT,0 DIAPCS ,0 DIAWT"
                    strSql += vbCrLf + " ,STNPCS PREPCS"
                    strSql += vbCrLf + " ,STNWT PREWT"
                    strSql += vbCrLf + " ,STNRATE,STNAMT,'R' AS TYPE,'P' AS COLHEAD"
                    strSql += vbCrLf + " ,(SELECT TOP 1 CATCODE FROM " & preTranDb & "..RECEIPT WHERE SNO=R.ISSSNO)AS ORNCATCODE"
                    strSql += vbCrLf + " FROM " & preTranDb & "..RECEIPTSTONE R"
                    If PENDTRF_ASON Then
                        strSql += vbCrLf + " WHERE TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                    Else
                        strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
                    End If
                    strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS WHERE TYPE='R' AND TRANTYPE IN('PU','RPU'))"
                    strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE ='P')"
                    If cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
                    If CmbCompany_MAN.Text <> "" And CmbCompany_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND COMPANYID = (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text & "')"
                    'strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                End If
            End If
            strSql += vbCrLf + " )X GROUP BY SNO,BATCHNO,TRANDATE,CATCODE,ORNCATCODE,TRANNO,TRANTYPE,STNRATE,STNAMT,TYPE,COLHEAD,METALID,STNITEMID"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
        End If

        strSql = " UPDATE A SET STNAMT=(SELECT SUM(ISNULL(STNAMT,0)) FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS WHERE SNO=A.SNO AND COLHEAD<>'A')"
        strSql += " FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS A WHERE COLHEAD='A' "
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        strSql = " UPDATE A SET AMOUNT=AMOUNT+ISNULL(STNAMT,0)"
        strSql += " FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS A WHERE COLHEAD='A' AND TRANTYPE NOT IN('PU','SR','MI','RPU')"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        If ChkWithMisc.Checked Or ChKSAL.Checked Or ChkSr.Checked Or Chkpurchase.Checked Then
            strSql = " SELECT CASE WHEN T.TRANTYPE IN('PU','RPU','RRE') THEN 'PURCHASE' "
            strSql += vbCrLf + " WHEN T.TRANTYPE='SR' THEN 'SALERETURN' "
            strSql += vbCrLf + " WHEN T.TRANTYPE='MI' THEN 'MISCISSUE' "
            strSql += vbCrLf + " ELSE 'PARTSALE' END AS TRANTYPE"
            strSql += vbCrLf + " ,T.TRANDATE,T.TRANNO,T.ITEMID,T.CATCODE,T.STNCATCODE"
            strSql += vbCrLf + " ,ISNULL(IM.ITEMNAME,IC.CATNAME) AS DESCRIPTION"
            strSql += vbCrLf + " ,CASE WHEN ISNULL(SUM(T.PCS),0)=0 THEN NULL ELSE SUM(T.PCS) END PCS"
            strSql += vbCrLf + " ,CASE WHEN ISNULL(SUM(T.GRSWT),0)=0 THEN NULL ELSE SUM(T.GRSWT) END GRSWT"
            strSql += vbCrLf + " ,CASE WHEN ISNULL(SUM(T.NETWT),0)=0 THEN NULL ELSE SUM(T.NETWT) END NETWT"
            strSql += vbCrLf + " ,CASE WHEN ISNULL(SUM(T.DIAPCS),0)=0 THEN NULL ELSE SUM(T.DIAPCS) END DIAPCS"
            strSql += vbCrLf + " ,CASE WHEN ISNULL(SUM(T.DIAWT),0)=0 THEN NULL ELSE SUM(T.DIAWT) END DIAWT"
            strSql += vbCrLf + " ,CASE WHEN ISNULL(SUM(T.STNPCS),0)=0 THEN NULL ELSE SUM(T.STNPCS) END STNPCS"
            strSql += vbCrLf + " ,CASE WHEN ISNULL(SUM(T.STNWT),0)=0 THEN NULL ELSE SUM(T.STNWT) END STNWT"
            strSql += vbCrLf + " ,CASE WHEN ISNULL(SUM(T.PREPCS),0)=0 THEN NULL ELSE SUM(T.PREPCS) END PREPCS"
            strSql += vbCrLf + " ,CASE WHEN ISNULL(SUM(T.PREWT),0)=0 THEN NULL ELSE SUM(T.PREWT) END PREWT"
            strSql += vbCrLf + " ,T.RATE"
            If AUTOBOOKVALUEENABLE = "Y" Then
                strSql += vbCrLf + " ,SUM(STNAMT)STNAMT"
                strSql += vbCrLf + " ,SUM(AMOUNT)AMOUNT"
            Else
                strSql += vbCrLf + " ,NULL STNAMT"
                strSql += vbCrLf + " ,NULL AMOUNT"
            End If
            strSql += vbCrLf + " ,T.SNO,T.BATCHNO,T.COLHEAD"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS AS T"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON T.ITEMID = IM.ITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY IC ON T.CATCODE = IC.CATCODE"
            strSql += vbCrLf + " GROUP BY T.SNO,T.BATCHNO,T.TRANTYPE,T.TRANDATE,T.TRANNO,T.ITEMID,T.CATCODE"
            strSql += vbCrLf + " ,IC.CATNAME,IM.ITEMNAME,T.RATE,T.COLHEAD,T.STNCATCODE"
            strSql += vbCrLf + " ORDER BY TRANDATE DESC,TRANTYPE,TRANNO,COLHEAD,IM.ITEMNAME"
        End If
        Try
            dtGrid = New DataTable
            Dim dtCol As New DataColumn("CHECK", GetType(Boolean))
            dtCol.DefaultValue = IIf(ChkAll.Checked, True, False)
            dtGrid.Columns.Add(dtCol)
            dtGrid.Columns.Add("KEYNO", GetType(Integer))
            dtGrid.Columns("KEYNO").AutoIncrement = True

            dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
            dtGrid.Columns("KEYNO").AutoIncrementStep = 1
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGrid)
            If Not dtGrid.Rows.Count > 0 Then
                MsgBox("Record not found", MsgBoxStyle.Information)
                btnNew.Focus()
                btnSearch.Enabled = True
                Exit Sub
            End If
            If PSSR_CATPOST <> "" And Not PSSR_CATPOSTING Is Nothing Then
                Dim dtCol1 As New DataColumn("PSSRCHECK", GetType(String))
                dtGrid.Columns.Add(dtCol1)
            End If
            dtGrid.Columns("CHECK").SetOrdinal(0)
            dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
            DgvView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
            DgvView.DataSource = dtGrid
            With DgvView
                .Columns("CHECK").DisplayIndex = 0
                .Columns("CHECK").HeaderText = ""
                .Columns("CHECK").Width = 25
                .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
                .Columns("TRANDATE").Width = 75
                .Columns("RATE").Width = 60
                .Columns("TRANNO").Width = 55
                .Columns("CATCODE").Visible = False
                .Columns("TRANTYPE").Width = 85
                .Columns("DESCRIPTION").Width = 125
                .Columns("PCS").Width = 40
                .Columns("GRSWT").Width = 80
                .Columns("NETWT").Width = 80
                .Columns("STNAMT").Width = 55
                .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("STNPCS").Visible = False
                .Columns("PREPCS").Visible = False
                .Columns("STNWT").Width = 55
                .Columns("DIAPCS").Width = 55
                .Columns("DIAWT").Width = 60
                .Columns("PREPCS").Width = 55
                .Columns("PREWT").Width = 55
                .Columns("DIAPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("DIAWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("PREPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("PREWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("STNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("TRANNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("STNAMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("RATE").DefaultCellStyle.Format = "0.00"
                .Columns("SNO").Visible = False
                .Columns("ITEMID").Visible = False
                .Columns("COLHEAD").Visible = False
                .Columns("KEYNO").Visible = False
                .Columns("STNCATCODE").Visible = False
                If .Columns.Contains("PSSRCHECK") And PSSR_CATPOST <> "" Then .Columns("PSSRCHECK").Visible = False
                For cnt As Integer = 0 To .ColumnCount - 1
                    If .Columns(cnt).Name = "CHECK" Then
                        .Columns(cnt).ReadOnly = False
                    Else
                        .Columns(cnt).ReadOnly = True
                    End If
                    .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
                Next
            End With
            If PSSR_CATPOST <> "" And Not PSSR_CATPOSTING Is Nothing Then
                For Each drr As DataGridViewRow In DgvView.Rows
                    If drr.Cells("TRANTYPE").Value.ToString = "PARTSALE" Or drr.Cells("TRANTYPE").Value.ToString = "SALERETURN" Then
                        For cnt As Integer = 0 To PSSR_CATPOSTING.Length - 1
                            If PSSR_CATPOSTING(cnt).ToString.Contains(drr.Cells("CATCODE").Value.ToString & ":") Then
                                drr.Cells("PSSRCHECK").Value = ""
                                drr.Cells("CHECK").Value = IIf(ChkAll.Checked, True, False)
                                Exit For
                            Else
                                drr.Cells("PSSRCHECK").Value = "Catcode not found in softcontrol PSSR_CATEGORYPOST"
                                drr.Cells("CHECK").Value = False
                            End If
                        Next
                        If drr.Cells("STNCATCODE").Value.ToString <> "" Then
                            For cnt As Integer = 0 To PSSR_CATPOSTING.Length - 1
                                If PSSR_CATPOSTING(cnt).ToString.Contains(drr.Cells("STNCATCODE").Value.ToString & ":") Then
                                    drr.Cells("PSSRCHECK").Value = ""
                                    drr.Cells("CHECK").Value = IIf(ChkAll.Checked, True, False)
                                    Exit For
                                Else
                                    drr.Cells("PSSRCHECK").Value += "Stone Catcode not found in softcontrol PSSR_CATEGORYPOST"
                                    drr.Cells("CHECK").Value = False
                                End If
                            Next
                        End If
                    End If
                Next
            End If

            COST_ID = GetSqlValue(cn, "SELECT TOP 1 COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE MAIN='Y' ")
            COST_NAME = GetSqlValue(cn, "SELECT TOP 1 COSTNAME FROM " & cnAdminDb & ".. COSTCENTRE WHERE COSTID='" & COST_ID & "'")
            ''FOR EXCEL 
            strSql = vbCrLf + "  SELECT TRANTYPE,TRANDATE, TRANNO, ITEMID,CATCODE,STNCATCODE, DESCRIPTION , PCS"
            strSql += vbCrLf + " ,GRSWT, NETWT,DIAPCS, DIAWT,STNPCS ,STNWT , PREPCS ,PREWT , RATE , STNAMT,AMOUNT"
            strSql += vbCrLf + " , SNO,BATCHNO,COLHEAD FROM ("
            strSql += vbCrLf + " SELECT CASE WHEN T.TRANTYPE IN('PU','RPU','RRE') THEN 'PURCHASE' "
            strSql += vbCrLf + " WHEN T.TRANTYPE='SR' THEN 'SALERETURN' "
            strSql += vbCrLf + " WHEN T.TRANTYPE='MI' THEN 'MISCISSUE' "
            strSql += vbCrLf + " ELSE 'PARTSALE' END AS TRANTYPE"
            strSql += vbCrLf + " ,T.TRANDATE,T.TRANNO,T.ITEMID,T.CATCODE,T.STNCATCODE"
            strSql += vbCrLf + " ,ISNULL(IM.ITEMNAME,IC.CATNAME) AS DESCRIPTION"
            strSql += vbCrLf + " ,CASE WHEN ISNULL(SUM(T.PCS),0)=0 THEN NULL ELSE SUM(T.PCS) END PCS"
            strSql += vbCrLf + " ,CASE WHEN ISNULL(SUM(T.GRSWT),0)=0 THEN NULL ELSE SUM(T.GRSWT) END GRSWT"
            strSql += vbCrLf + " ,CASE WHEN ISNULL(SUM(T.NETWT),0)=0 THEN NULL ELSE SUM(T.NETWT) END NETWT"
            strSql += vbCrLf + " ,CASE WHEN ISNULL(SUM(T.DIAPCS),0)=0 THEN NULL ELSE SUM(T.DIAPCS) END DIAPCS"
            strSql += vbCrLf + " ,CASE WHEN ISNULL(SUM(T.DIAWT),0)=0 THEN NULL ELSE SUM(T.DIAWT) END DIAWT"
            strSql += vbCrLf + " ,CASE WHEN ISNULL(SUM(T.STNPCS),0)=0 THEN NULL ELSE SUM(T.STNPCS) END STNPCS"
            strSql += vbCrLf + " ,CASE WHEN ISNULL(SUM(T.STNWT),0)=0 THEN NULL ELSE SUM(T.STNWT) END STNWT"
            strSql += vbCrLf + " ,CASE WHEN ISNULL(SUM(T.PREPCS),0)=0 THEN NULL ELSE SUM(T.PREPCS) END PREPCS"
            strSql += vbCrLf + " ,CASE WHEN ISNULL(SUM(T.PREWT),0)=0 THEN NULL ELSE SUM(T.PREWT) END PREWT"
            strSql += vbCrLf + " ,T.RATE"
            strSql += vbCrLf + " ,NULL STNAMT"
            strSql += vbCrLf + " ,NULL AMOUNT"
            strSql += vbCrLf + " ,T.SNO,T.BATCHNO,T.COLHEAD"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS AS T"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON T.ITEMID = IM.ITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY IC ON T.CATCODE = IC.CATCODE"
            strSql += vbCrLf + " GROUP BY T.SNO,T.BATCHNO,T.TRANTYPE,T.TRANDATE,T.TRANNO,T.ITEMID,T.CATCODE"
            strSql += vbCrLf + " ,IC.CATNAME,IM.ITEMNAME,T.RATE,T.COLHEAD,T.STNCATCODE"

            strSql += vbCrLf + " UNION ALL"

            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + " 'TOTAL'  AS TRANTYPE"
            strSql += vbCrLf + " ,NULL  TRANDATE,0 TRANNO,0 ITEMID,'' CATCODE,'' STNCATCODE"
            strSql += vbCrLf + " ,'' AS DESCRIPTION"
            strSql += vbCrLf + " ,CASE WHEN ISNULL(SUM(T.PCS),0)=0 THEN NULL ELSE SUM(T.PCS) END PCS"
            strSql += vbCrLf + " ,CASE WHEN ISNULL(SUM(T.GRSWT),0)=0 THEN NULL ELSE SUM(T.GRSWT) END GRSWT"
            strSql += vbCrLf + " ,CASE WHEN ISNULL(SUM(T.NETWT),0)=0 THEN NULL ELSE SUM(T.NETWT) END NETWT"
            strSql += vbCrLf + " ,CASE WHEN ISNULL(SUM(T.DIAPCS),0)=0 THEN NULL ELSE SUM(T.DIAPCS) END DIAPCS"
            strSql += vbCrLf + " ,CASE WHEN ISNULL(SUM(T.DIAWT),0)=0 THEN NULL ELSE SUM(T.DIAWT) END DIAWT"
            strSql += vbCrLf + " ,CASE WHEN ISNULL(SUM(T.STNPCS),0)=0 THEN NULL ELSE SUM(T.STNPCS) END STNPCS"
            strSql += vbCrLf + " ,CASE WHEN ISNULL(SUM(T.STNWT),0)=0 THEN NULL ELSE SUM(T.STNWT) END STNWT"
            strSql += vbCrLf + " ,CASE WHEN ISNULL(SUM(T.PREPCS),0)=0 THEN NULL ELSE SUM(T.PREPCS) END PREPCS"
            strSql += vbCrLf + " ,CASE WHEN ISNULL(SUM(T.PREWT),0)=0 THEN NULL ELSE SUM(T.PREWT) END PREWT"
            strSql += vbCrLf + " ,0 RATE ,NULL STNAMT"
            strSql += vbCrLf + " ,NULL AMOUNT ,'' SNO,'' BATCHNO,'T'COLHEAD"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS AS T"
            strSql += vbCrLf + " )X"
            strSql += vbCrLf + " ORDER BY TRANDATE DESC,TRANTYPE,TRANNO,COLHEAD"
            dtGridExcel = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGridExcel)
            dgvexcel1.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
            dgvexcel1.DataSource = dtGridExcel
            With dgvexcel1
                .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
                .Columns("TRANDATE").Width = 75
                .Columns("RATE").Width = 60
                .Columns("TRANNO").Width = 55
                .Columns("CATCODE").Visible = False
                .Columns("TRANTYPE").Width = 85
                .Columns("DESCRIPTION").Width = 125
                .Columns("PCS").Width = 40
                .Columns("GRSWT").Width = 80
                .Columns("NETWT").Width = 80
                .Columns("STNAMT").Width = 55
                .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("STNPCS").Visible = False
                .Columns("PREPCS").Visible = False
                .Columns("STNWT").Width = 55
                .Columns("DIAPCS").Width = 55
                .Columns("DIAWT").Width = 60
                .Columns("PREPCS").Width = 55
                .Columns("PREWT").Width = 55
                .Columns("DIAPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("DIAWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("PREPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("PREWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("STNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("TRANNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("STNAMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("RATE").DefaultCellStyle.Format = "0.00"
                .Columns("SNO").Visible = False
                .Columns("ITEMID").Visible = False
                .Columns("COLHEAD").Visible = False
                .Columns("STNCATCODE").Visible = False
            End With
            For Each Gv As DataGridViewRow In dgvexcel1.Rows
                With Gv
                    Select Case .Cells("COLHEAD").Value.ToString
                        Case "T"
                            .DefaultCellStyle = reportTotalStyle
                    End Select
                End With
            Next
            btnTransfer.Enabled = True
            btnTransfer.Focus()
            funcGridTotal()
            lblHelp.Visible = True
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnTransfer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTransfer.Click
        Dim key As String = Nothing
        Try
            InternalIssuecreate()
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Private Sub InternalIssuecreate()
        Dim dt As New DataTable
        dt = DgvView.DataSource
        dt.AcceptChanges()
        Dim ros() As DataRow = Nothing
        ros = dt.Select("CHECK = TRUE")
        If Not ros.Length > 0 Then
            MsgBox("There is No Checked Record", MsgBoxStyle.Information)
            Exit Sub
        End If
        If MessageBox.Show("Do you want to Transfer the above items?", "Transfer Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then Exit Sub

        Try
            Dim costId As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'")
            Dim Accode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID ='" & cnHOCostId & "'")
            Dim TAccode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID ='" & costId & "'")
            Dim TCostname As String = objGPack.GetSqlValue("SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID ='" & cnHOCostId & "'")
            If StateId = 0 Then StateId = Val(objGPack.GetSqlValue("SELECT STATEID FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = (SELECT ISNULL(ACCODE,'')ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE costid='" & costId.ToString & "')").ToString)
            Dim mTotPcs As Decimal = 0
            Dim mTotGwt As Decimal = 0
            Dim mTotNwt As Decimal = 0
            Dim mTotStnwt As Decimal = 0
            Dim mTotStnpcs As Decimal = 0
            Dim mTotDiawt As Decimal = 0
            Dim mTotDiapcs As Decimal = 0
            Dim Recsno As String = ""
            Dim Isssno As String = ""
            Dim BAccode As String
            BAccode = TAccode
            TAccode = "STKTRAN"
            If Accode = "" Or TAccode = "" Then MsgBox("Internal Transfer Accode is Empty", MsgBoxStyle.Information) : Exit Sub
            Dim dtStnItem As DataTable = dt.DefaultView.ToTable(True, "ITEMID")
            Dim dtTrantype As DataTable = dt.DefaultView.ToTable(True, "TRANTYPE")
            Dim trdate As Date
            If GetAdmindbSoftValue("GLOBALDATE").ToUpper = "Y" Then
                If PENDTRF_ENTRYDATE = False Then
                    trdate = GetEntryDate(GetServerDate())
                Else
                    If PENDTRF_ASON Then
                        trdate = dtpFrom.Value
                    Else
                        trdate = dtpTo.Value
                    End If
                End If
            Else
                If PENDTRF_ASON Then
                    trdate = dtpFrom.Value
                Else
                    trdate = dtpTo.Value
                End If
            End If
            dtSummary.Clear()
            Dim dtCatcode As DataTable = dt.DefaultView.ToTable(True, "CATCODE")
            Dim mCatcode As String
            Dim mStnCatcode As String
            Dim mStnItem As Integer
            Dim mType As String
            For KK As Integer = 0 To dtTrantype.Rows.Count - 1
                mType = dtTrantype.Rows(KK).Item("TRANTYPE").ToString
                For II As Integer = 0 To dtCatcode.Rows.Count - 1
                    mCatcode = dtCatcode.Rows(II).Item("CATCODE").ToString
                    Dim mpcs As Integer = 0
                    Dim mgrswt As Decimal = 0
                    Dim mnetwt As Decimal = 0
                    Dim mdiapcs As Integer = 0
                    Dim mdiawt As Decimal = 0
                    Dim mstnpcs As Integer = 0
                    Dim mstnwt As Decimal = 0
                    Dim mprepcs As Integer = 0
                    Dim mPrewt As Decimal = 0
                    Dim mAmount As Decimal = 0
                    Dim mStnAmt As Decimal = 0
                    For Each row As DataRow In ros
                        If row.Item("TRANTYPE").ToString = mType.ToString And row.Item("CATCODE").ToString = mCatcode.ToString Then
                            mpcs += Val(row.Item("PCS").ToString)
                            mgrswt += Val(row.Item("GRSWT").ToString)
                            mnetwt += Val(row.Item("NETWT").ToString)
                            mAmount += Val(row.Item("AMOUNT").ToString)
                            If row.Item("COLHEAD").ToString = "A" Then
                                mStnAmt += Val(row.Item("STNAMT").ToString)
                            End If
                        End If
                    Next
                    Dim drows As DataRow
                    drows = dtSummary.NewRow
                    strSql = "SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & mCatcode & "'"
                    drows!KEYNO = KK
                    drows!CATCODE = mCatcode
                    drows!CATNAME = objGPack.GetSqlValue(strSql).ToString
                    drows!PCS = mpcs
                    drows!GRSWT = Format(mgrswt, "#0.000")
                    drows!NETWT = Format(mnetwt, "#0.000")
                    drows!DIAPCS = IIf(mdiapcs = 0, DBNull.Value, mdiapcs)
                    drows!DIAWT = IIf(mdiawt = 0, DBNull.Value, Format(mdiawt, "#0.000"))
                    drows!STNPCS = IIf(mstnpcs = 0, DBNull.Value, mstnpcs)
                    drows!STNWT = IIf(mstnwt = 0, DBNull.Value, Format(mstnwt, "#0.000"))
                    drows!PREPCS = IIf(mprepcs = 0, DBNull.Value, mprepcs)
                    drows!PREWT = IIf(mPrewt = 0, DBNull.Value, Format(mPrewt, "#0.000"))
                    drows!AMOUNT = IIf(mAmount = 0, DBNull.Value, Format(mAmount, "#0.00"))
                    drows!STNAMT = IIf(mStnAmt = 0, DBNull.Value, Format(mStnAmt, "#0.00"))
                    drows!TYPE = mType
                    dtSummary.Rows.Add(drows)
                    dtSummary.AcceptChanges()
                    mgrswt = 0 : mnetwt = 0 : mpcs = 0
                    mPrewt = 0 : mdiawt = 0 : mstnwt = 0 : mdiapcs = 0 : mprepcs = 0 : mstnpcs = 0
                    For JJ As Integer = 0 To dtStnItem.Rows.Count - 1
                        mStnItem = Val(dtStnItem.Rows(JJ).Item("ITEMID").ToString)
                        For Each row As DataRow In ros
                            If Val(row.Item("ITEMID").ToString) = mStnItem Then
                                mstnpcs = Val(dt.Compute("SUM(STNPCS)", "CATCODE='" & mCatcode & "' AND ITEMID='" & mStnItem & "' AND TRANTYPE='" & mType & "' AND CHECK='TRUE'").ToString)
                                mstnwt = Val(dt.Compute("SUM(STNWT)", "CATCODE='" & mCatcode & "' AND ITEMID='" & mStnItem & "' AND TRANTYPE='" & mType & "' AND CHECK='TRUE'").ToString)
                                mdiapcs = Val(dt.Compute("SUM(DIAPCS)", "CATCODE='" & mCatcode & "' AND ITEMID='" & mStnItem & "' AND TRANTYPE='" & mType & "' AND CHECK='TRUE'").ToString)
                                mdiawt = Val(dt.Compute("SUM(DIAWT)", "CATCODE='" & mCatcode & "' AND ITEMID='" & mStnItem & "' AND TRANTYPE='" & mType & "' AND CHECK='TRUE'").ToString)
                                mprepcs = Val(dt.Compute("SUM(PREPCS)", "CATCODE='" & mCatcode & "' AND ITEMID='" & mStnItem & "' AND TRANTYPE='" & mType & "' AND CHECK='TRUE'").ToString)
                                mPrewt = Val(dt.Compute("SUM(PREWT)", "CATCODE='" & mCatcode & "' AND ITEMID='" & mStnItem & "' AND TRANTYPE='" & mType & "' AND CHECK='TRUE'").ToString)
                                mStnAmt = Val(dt.Compute("SUM(STNAMT)", "CATCODE='" & mCatcode & "' AND ITEMID='" & mStnItem & "' AND TRANTYPE='" & mType & "' AND CHECK='TRUE'").ToString)
                            End If
                        Next
                        If mPrewt > 0 Or mdiawt > 0 Or mstnwt > 0 Then
                            drows = dtSummary.NewRow
                            strSql = "SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & mStnItem
                            mStnCatcode = objGPack.GetSqlValue(strSql).ToString()
                            drows!KEYNO = KK
                            drows!CATCODE = mCatcode
                            drows!CATNAME = ""
                            drows!PCS = DBNull.Value
                            drows!GRSWT = DBNull.Value
                            drows!NETWT = DBNull.Value
                            drows!DIAPCS = IIf(mdiapcs = 0, DBNull.Value, mdiapcs)
                            drows!DIAWT = IIf(mdiawt = 0, DBNull.Value, Format(mdiawt, "#0.000"))
                            drows!STNPCS = IIf(mstnpcs = 0, DBNull.Value, mstnpcs)
                            drows!STNWT = IIf(mstnwt = 0, DBNull.Value, Format(mstnwt, "#0.000"))
                            drows!PREPCS = IIf(mprepcs = 0, DBNull.Value, mprepcs)
                            drows!PREWT = IIf(mPrewt = 0, DBNull.Value, Format(mPrewt, "#0.000"))
                            drows!AMOUNT = DBNull.Value
                            drows!STNITEM = mStnItem
                            drows!STNCATCODE = mStnCatcode
                            drows!STNAMT = IIf(mStnAmt = 0, DBNull.Value, Format(mStnAmt, "#0.00"))
                            drows!TYPE = mType
                            dtSummary.Rows.Add(drows)
                            dtSummary.AcceptChanges()
                            mPrewt = 0 : mdiawt = 0 : mstnwt = 0 : mdiapcs = 0 : mprepcs = 0 : mstnpcs = 0
                        End If
                    Next
                Next
            Next
            Dim TrGrswt As Decimal = Val(dtSummary.Compute("SUM(GRSWT)", Nothing).ToString)
            Dim TrNetwt As Decimal = Val(dtSummary.Compute("SUM(NETWT)", Nothing).ToString)
            Dim TrStnwt As Decimal = Val(dtSummary.Compute("SUM(STNWT)", Nothing).ToString)
            Dim TrDiawt As Decimal = Val(dtSummary.Compute("SUM(DIAWT)", Nothing).ToString)
            Dim TrPrewt As Decimal = Val(dtSummary.Compute("SUM(PREWT)", Nothing).ToString)
            Dim dtMark As New DataTable
            dtMark = gridViewtotal.DataSource
            Dim MrGrswt As Decimal = Val(dtMark.Compute("SUM(GRSWT)", " TRANTYPE<>'TOTAL' ").ToString)
            Dim MrNetwt As Decimal = Val(dtMark.Compute("SUM(NETWT)", " TRANTYPE<>'TOTAL' ").ToString)
            Dim MrStnwt As Decimal = Val(dtMark.Compute("SUM(STNWT)", " TRANTYPE<>'TOTAL' ").ToString)
            Dim MrDiawt As Decimal = Val(dtMark.Compute("SUM(DIAWT)", " TRANTYPE<>'TOTAL' ").ToString)
            Dim MrPrewt As Decimal = Val(dtMark.Compute("SUM(PREWT)", " TRANTYPE<>'TOTAL' ").ToString)
            If MrGrswt <> TrGrswt Then
                MsgBox("GrsWt Mismatch", MsgBoxStyle.Information)
                Exit Sub
            End If
            If MrNetwt <> TrNetwt Then
                MsgBox("NetWt Mismatch", MsgBoxStyle.Information)
                Exit Sub
            End If
            If MrStnwt <> TrStnwt Then
                MsgBox("StnWt Mismatch,Marked Wt[" & MrStnwt & "],Total Wt[" & TrStnwt & "]", MsgBoxStyle.Information)
                Exit Sub
            End If
            If MrDiawt <> TrDiawt Then
                MsgBox("DiaWt Mismatch", MsgBoxStyle.Information)
                Exit Sub
            End If
            If MrPrewt <> TrPrewt Then
                MsgBox("PreWt Mismatch", MsgBoxStyle.Information)
                Exit Sub
            End If
            tran = Nothing
            tran = cn.BeginTransaction
            Dim PTrfNo As String = Nothing
GENBAGNO:
            Dim Batchno As String = GetNewBatchno(cnCostId, trdate, tran)
            Dim billcontrolid As String = "GEN-SM-INTISS"
            strSql = "SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL"
            strSql += " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
            If UCase(objGPack.GetSqlValue(strSql, , , tran)) <> "Y" Then
                billcontrolid = "GEN-STKREFNO"
            End If
            Dim NEWBILLNO As Integer
            strSql = "SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = '" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
            NEWBILLNO = Val(objGPack.GetSqlValue(strSql, , , tran)) + 1
GenerateNewBillNo:
            PTrfNo = cnCostId & "T" & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString & NEWBILLNO.ToString
            strSql = " SELECT TOP 1 SNO FROM " & cnStockDb & "..ISSUE WHERE TRANTYPE = 'IIN' AND COSTID = '" & cnCostId & "' AND TRANNO = " & NEWBILLNO
            If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
                NEWBILLNO = NEWBILLNO + 1
                GoTo GenerateNewBillNo
            End If
            strSql = "UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & NEWBILLNO.ToString & "'"
            strSql += " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            If cmd.ExecuteNonQuery() = 0 Then
                GoTo GenerateNewBillNo
            End If

            For i As Integer = 0 To dtSummary.Rows.Count - 1
                With dtSummary.Rows(i)
                    Dim Catcode As String : Dim Sno As String : Dim Amount As Decimal : Dim _PSSRSNO As String
                    Dim Pcs As Decimal : Dim Grswt As Decimal : Dim NetWt As Decimal
                    Dim DiaPcs As Decimal : Dim Diawt As Decimal
                    Dim StnPcs As Decimal : Dim StnWt As Decimal
                    Dim PrePcs As Decimal : Dim PreWt As Decimal
                    Dim Purity As Decimal : Dim StnAmt As Decimal
                    Dim StnItemid As Integer : Dim StnCatcode As String : Dim Trantype As String
                    Catcode = .Item("CATCODE").ToString
                    Pcs = Val(.Item("PCS").ToString)
                    Grswt = Val(.Item("GRSWT").ToString)
                    NetWt = Val(.Item("NETWT").ToString)
                    Amount = Val(.Item("AMOUNT").ToString)
                    StnAmt = Val(.Item("STNAMT").ToString)
                    DiaPcs = Val(.Item("DIAPCS").ToString)
                    Diawt = Val(.Item("DIAWT").ToString)
                    StnPcs = Val(.Item("STNPCS").ToString)
                    StnWt = Val(.Item("STNWT").ToString)
                    PrePcs = Val(.Item("PREPCS").ToString)
                    PreWt = Val(.Item("PREWT").ToString)
                    StnItemid = Val(.Item("STNITEM").ToString)
                    StnCatcode = .Item("STNCATCODE").ToString
                    Trantype = .Item("TYPE").ToString
                    If Pcs = 0 And Grswt = 0 And NetWt = 0 And StnAmt = 0 And StnPcs = 0 And StnWt = 0 _
                    And DiaPcs = 0 And Diawt = 0 And DiaPcs = 0 _
                    And Diawt = 0 And PrePcs = 0 And PreWt = 0 Then Continue For

                    strSql = "SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE='" & Catcode & "'"
                    Dim MetalId As String = objGPack.GetSqlValue(strSql, "METALID", "G", tran)

                    strSql = "SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID=(SELECT TOP 1 PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE='" & Catcode & "')"
                    Purity = Val(objGPack.GetSqlValue(strSql, "PURITY", "91.6", tran))

                    'NEWLY added
                    Dim IGSTPER As Decimal = 0
                    Dim IGSTAccode As String = ""
                    Dim IGST As Decimal = 0
                    If AUTOBOOK_VOUCHER = "Y" Then ' If chkRepairentry.Checked = True Then
                        If GST And StateId <> CompanyStateId And Amount > 0 Then
                            Dim GstcalcCatcode As String = Catcode
                            If Catcode <> "" Then
                                strSql = "SELECT TOP 1 S_IGSTTAX FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE ='" & GstcalcCatcode & "'"
                                IGSTPER = Val(objGPack.GetSqlValue(strSql, "S_IGSTTAX", "3", tran))
                                strSql = "SELECT TOP 1 S_IGSTID  FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE ='" & GstcalcCatcode & "'"
                                IGSTAccode = objGPack.GetSqlValue(strSql, "", "", tran).Trim.ToString
                            End If

                            If Val(IGSTPER) > 0 Then
                                If StateId <> CompanyStateId Then
                                    IGST = Math.Round((Val(Amount) * IGSTPER) / 100, 2)
                                    IGST = CalcRoundoffAmt(IGST, 2)
                                End If
                            Else
                                IGST = 0
                            End If
                        End If
                    Else
                        IGST = 0
                    End If

                    If Val(IGST) > 0 Then
                        Amount = Format(Val(Amount) + Val(IGST), "0.00")
                    Else
                        Amount = Format(Val(Amount), "0.00")
                        IGST = 0
                    End If
                    'End Newly Added


                    If StnCatcode = "" Then
                        Sno = GetNewSno(TranSnoType.ISSUECODE, tran)
                        If Trantype <> "MISCISSUE" Then
                            strSql = " INSERT INTO " & cnStockDb & "..ISSUE"
                            strSql += " ("
                            strSql += " SNO,COMPANYID,COSTID,CATCODE,METALID,TRANDATE,TRANNO,TRANTYPE,"
                            strSql += " PCS,GRSWT,LESSWT,NETWT,STNAMT,AMOUNT,BATCHNO, CANCEL,TRFNO,REFNO,REMARK1,REMARK2,ACCODE, "
                            strSql += " USERID,UPDATED,UPTIME,SYSTEMID,APPVER,PURITY,FLAG,TAX)VALUES("
                            strSql += " '" & Sno & "','" & GetStockCompId() & "','" & costId & "'"
                            strSql += ",'" & Catcode & "'" 'CATCODE
                            strSql += ",'" & MetalId & "'" 'METALID
                            strSql += ",'" & trdate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                            strSql += " ," & NEWBILLNO & " ,'IIN'"
                            strSql += " ," & Val(Pcs) & "" 'PCS
                            strSql += " ," & Val(Grswt) & "" 'GRSWT
                            strSql += " ," & Val(Grswt) - Val(NetWt) & "" 'LESSWT
                            strSql += " ," & Val(NetWt) & "" 'NETWT
                            strSql += " ," & Val(StnAmt) & "" 'STNAMT
                            If Val(IGST) > 0 Then
                                strSql += " ," & Format(Val(Amount) - Val(IGST), "0.00") & "" 'AMOUNT
                            Else
                                strSql += " ," & Val(Amount) & "" 'AMOUNT
                            End If
                            strSql += " ,'" & Batchno & "' ,''" 'CANCEL
                            strSql += " ,'" & PTrfNo & "'" 'TRFNO
                            strSql += " ,'" & PTrfNo & "'" 'REFNO
                            strSql += " ,'TRANSFER TO " & TCostname & "'" 'REMARK1
                            strSql += " ,'" & Trantype & "'" 'REMARK2
                            strSql += " ,'" & Accode & "' " 'ACCODE
                            strSql += " ," & userId & "" 'USERID
                            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                            strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                            strSql += " ,'" & systemId & "'" 'SYSTEMID
                            strSql += " ,'" & VERSION & "'" 'APPVER
                            strSql += " ," & Purity  'PURITY
                            strSql += " ,'T'" 'FLAG
                            If Val(IGST) > 0 Then
                                strSql += " ,'" & Format(Val(IGST), "0.00") & "'"   'TAX
                            Else
                                strSql += " ,'0.00'"   'TAX
                            End If
                            strSql += " )"
                            If CENTR_DB_BR Then
                                cmd = New OleDbCommand(strSql, cn, tran)
                                cmd.ExecuteNonQuery()
                            Else
                                ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
                            End If
                        End If

                        strSql = " INSERT INTO " & cnStockDb & ".." & IIf(CENTR_DB_BR, "TRECEIPT", "RECEIPT")
                        strSql += " ("
                        strSql += " SNO,COMPANYID,COSTID,CATCODE,ISSNO,METALID,TRANDATE,TRANNO,TRANTYPE,"
                        strSql += " PCS,GRSWT,LESSWT,NETWT,STNAMT,AMOUNT,BATCHNO, CANCEL,TRFNO,REFNO,REMARK1,REMARK2,ACCODE, "
                        strSql += " USERID,UPDATED,UPTIME,SYSTEMID,APPVER,PURITY,FLAG,TAX)VALUES("
                        strSql += " '" & Sno & "','" & GetStockCompId() & "','" & cnHOCostId & "'"
                        strSql += ",'" & Catcode & "'" 'CATCODE
                        strSql += ",'" & Sno & "'" 'ISSNO
                        strSql += ",'" & MetalId & "'" 'METALID
                        strSql += ",'" & trdate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                        strSql += " ," & NEWBILLNO & " ,'RIN'"
                        strSql += " ," & Val(Pcs) & "" 'PCS
                        strSql += " ," & Val(Grswt) & "" 'GRSWT
                        strSql += " ," & Val(Grswt) - Val(NetWt) & "" 'LESSWT
                        strSql += " ," & Val(NetWt) & "" 'NETWT
                        strSql += " ," & Val(StnAmt) & "" 'STNAMT
                        If Val(IGST) > 0 Then
                            strSql += " ," & Format(Val(Amount) - Val(IGST), "0.00") & "" 'AMOUNT
                        Else
                            strSql += " ," & Val(Amount) & "" 'AMOUNT
                        End If
                        strSql += " ,'" & Batchno & "' ,''" 'CANCEL
                        strSql += " ,'" & PTrfNo & "'" 'TRFNO
                        strSql += " ,'" & PTrfNo & "'" 'REFNO
                        strSql += " ,'TRANSFER FROM " & cmbCostCentre_MAN.Text & "'" 'REMARK1
                        strSql += " ,'" & Trantype & "'" 'REMARK2
                        strSql += " ,'" & BAccode & "' " 'ACCODE
                        strSql += " ," & userId & "" 'USERID
                        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                        strSql += " ,'" & systemId & "'" 'SYSTEMID
                        strSql += " ,'" & VERSION & "'" 'APPVER
                        strSql += " ," & Purity  'PURITY
                        strSql += " ,'T'" 'FLAG
                        If Val(IGST) > 0 Then
                            strSql += " ,'" & Format(Val(IGST), "0.00") & "'"   'TAX
                        Else
                            strSql += " ,'0.00'"   'TAX
                        End If
                        strSql += " )"
                        If CENTR_DB_BR Then
                            cmd = New OleDbCommand(strSql, cn, tran)
                            cmd.ExecuteNonQuery()
                        Else
                            ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId, , , "TRECEIPT", False)
                        End If
                        '' newly added on 05-MAY-2022 for sep ps/sr catcode posting
                        If Not PSSR_CATPOSTING Is Nothing And (Trantype = "PARTSALE" Or Trantype = "SALERETURN") Then
                            Dim _tempPssr_post() As String
                            Dim _tempPssr_Catcode As String = ""
                            For cnt As Integer = 0 To PSSR_CATPOSTING.Length - 1
                                If PSSR_CATPOSTING(cnt).ToString.Contains(Catcode & ":") Then
                                    _tempPssr_post = PSSR_CATPOSTING(cnt).ToString.Split(":")
                                    If Trantype = "PARTSALE" And _tempPssr_post.Length = 3 Then
                                        _tempPssr_Catcode = _tempPssr_post(1).ToString
                                    ElseIf Trantype = "SALERETURN" And _tempPssr_post.Length = 3 Then
                                        _tempPssr_Catcode = _tempPssr_post(2).ToString
                                    End If
                                    Exit For
                                End If
                            Next
                            If _tempPssr_post.Length = 3 And _tempPssr_Catcode <> "" Then
                                _PSSRSNO = GetNewSno(TranSnoType.ISSUECODE, tran)
                                strSql = " INSERT INTO " & cnStockDb & ".." & IIf(CENTR_DB_BR, "TISSUE", "ISSUE")
                                strSql += " ("
                                strSql += " SNO,COMPANYID,COSTID,CATCODE,METALID,TRANDATE,TRANNO,TRANTYPE,"
                                strSql += " PCS,GRSWT,LESSWT,NETWT,STNAMT,AMOUNT,BATCHNO, CANCEL,TRFNO,REFNO,REMARK1,REMARK2,ACCODE, "
                                strSql += " USERID,UPDATED,UPTIME,SYSTEMID,APPVER,PURITY,FLAG,TAX)VALUES("
                                strSql += " '" & _PSSRSNO & "','" & GetStockCompId() & "','" & cnHOCostId & "'"
                                strSql += ",'" & Catcode & "'" 'CATCODE
                                strSql += ",'" & MetalId & "'" 'METALID
                                strSql += ",'" & trdate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                                strSql += " ," & NEWBILLNO & " ,'IIN'"
                                strSql += " ," & Val(Pcs) & "" 'PCS
                                strSql += " ," & Val(Grswt) & "" 'GRSWT
                                strSql += " ," & Val(Grswt) - Val(NetWt) & "" 'LESSWT
                                strSql += " ," & Val(NetWt) & "" 'NETWT
                                strSql += " ," & Val(0) & "" 'STNAMT
                                strSql += " ," & Format(Val(0), "0.00") & "" 'AMOUNT
                                strSql += " ,'" & Batchno & "' ,''" 'CANCEL
                                strSql += " ,'" & PTrfNo & "'" 'TRFNO
                                strSql += " ,'" & PTrfNo & "'" 'REFNO
                                strSql += " ,'TRANSFER TO " & TCostname & "'" 'REMARK1
                                strSql += " ,'" & Trantype & "'" 'REMARK2
                                strSql += " ,'" & Accode & "' " 'ACCODE
                                strSql += " ," & userId & "" 'USERID
                                strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                                strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                                strSql += " ,'" & systemId & "'" 'SYSTEMID
                                strSql += " ,'" & VERSION & "'" 'APPVER
                                strSql += " ," & Purity  'PURITY
                                strSql += " ,'T'" 'FLAG
                                strSql += " ,'" & Format(Val(0), "0.00") & "'"   'TAX
                                strSql += " )"
                                If CENTR_DB_BR Then
                                    cmd = New OleDbCommand(strSql, cn, tran)
                                    cmd.ExecuteNonQuery()
                                Else
                                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId, , , "TISSUE", False)
                                End If

                                strSql = " INSERT INTO " & cnStockDb & ".." & IIf(CENTR_DB_BR, "TRECEIPT", "RECEIPT")
                                strSql += " ("
                                strSql += " SNO,COMPANYID,COSTID,CATCODE,ISSNO,METALID,TRANDATE,TRANNO,TRANTYPE,"
                                strSql += " PCS,GRSWT,LESSWT,NETWT,STNAMT,AMOUNT,BATCHNO, CANCEL,TRFNO,REFNO,REMARK1,REMARK2,ACCODE, "
                                strSql += " USERID,UPDATED,UPTIME,SYSTEMID,APPVER,PURITY,FLAG,TAX)VALUES("
                                strSql += " '" & _PSSRSNO & "','" & GetStockCompId() & "','" & cnHOCostId & "'"
                                strSql += ",'" & _tempPssr_Catcode & "'" 'CATCODE
                                strSql += ",'" & _PSSRSNO & "'" 'ISSNO
                                strSql += ",'" & MetalId & "'" 'METALID
                                strSql += ",'" & trdate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                                strSql += " ," & NEWBILLNO & " ,'RIN'"
                                strSql += " ," & Val(Pcs) & "" 'PCS
                                strSql += " ," & Val(Grswt) & "" 'GRSWT
                                strSql += " ," & Val(Grswt) - Val(NetWt) & "" 'LESSWT
                                strSql += " ," & Val(NetWt) & "" 'NETWT
                                strSql += " ," & Val(0) & "" 'STNAMT
                                strSql += " ," & Format(Val(0), "0.00") & "" 'AMOUNT
                                strSql += " ,'" & Batchno & "' ,''" 'CANCEL
                                strSql += " ,'" & PTrfNo & "'" 'TRFNO
                                strSql += " ,'" & PTrfNo & "'" 'REFNO
                                strSql += " ,'TRANSFER FROM " & cmbCostCentre_MAN.Text & "'" 'REMARK1
                                strSql += " ,'" & Trantype & "'" 'REMARK2
                                strSql += " ,'" & BAccode & "' " 'ACCODE
                                strSql += " ," & userId & "" 'USERID
                                strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                                strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                                strSql += " ,'" & systemId & "'" 'SYSTEMID
                                strSql += " ,'" & VERSION & "'" 'APPVER
                                strSql += " ," & Purity  'PURITY
                                strSql += " ,'T'" 'FLAG
                                strSql += " ,'" & Format(Val(0), "0.00") & "'"   'TAX
                                strSql += " )"
                                If CENTR_DB_BR Then
                                    cmd = New OleDbCommand(strSql, cn, tran)
                                    cmd.ExecuteNonQuery()
                                Else
                                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId, , , "TRECEIPT", False)
                                End If
                            End If
                        End If
                        '' END newly added on 05-MAY-2022 for sep ps/sr catcode posting
                    End If
                    If Diawt > 0 Or StnWt > 0 Or PreWt > 0 Then
                        If StnWt > 0 Then
                            Dim IssNo As String = GetNewSno(TranSnoType.ISSSTONECODE, tran)
                            If Trantype <> "MISCISSUE" Then
                                strSql = " INSERT INTO " & cnStockDb & "..ISSSTONE"
                                strSql += " ("
                                strSql += " SNO,ISSSNO,COMPANYID,COSTID,STONEUNIT"
                                strSql += " ,STNITEMID,CATCODE,TRANDATE,TRANNO,TRANTYPE"
                                strSql += " ,STNPCS,STNWT,STNAMT,BATCHNO "
                                strSql += " ,SYSTEMID,APPVER)VALUES("
                                strSql += " '" & IssNo & "'" 'SNO
                                strSql += " ,'" & Sno & "'" 'ISSSNO
                                strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                                strSql += " ,'" & costId & "'" 'COSTID
                                strSql += " ,'C'" 'STONEUNIT
                                strSql += " ," & StnItemid & "" ''STNITEMID
                                strSql += ",'" & StnCatcode & "'" 'CATCODE
                                strSql += ",'" & trdate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                                strSql += " ," & NEWBILLNO & "" ''TRANNO
                                strSql += " ,'IIN'" ''TRANTYPE
                                strSql += " ," & Val(StnPcs) & "" 'STNPCS
                                strSql += " ," & Val(StnWt) & "" 'STNWT
                                strSql += " ," & Val(StnAmt) & "" 'STNAMT
                                strSql += " ,'" & Batchno & "' " 'BATCHNO
                                strSql += " ,'" & systemId & "'" 'SYSTEMID
                                strSql += " ,'" & VERSION & "'" 'APPVER
                                strSql += " )"
                                If CENTR_DB_BR Then
                                    cmd = New OleDbCommand(strSql, cn, tran)
                                    cmd.ExecuteNonQuery()
                                Else
                                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
                                End If
                            End If
                            strSql = " INSERT INTO " & cnStockDb & ".." & IIf(CENTR_DB_BR, "TRECEIPTSTONE", "RECEIPTSTONE")
                            strSql += " ("
                            strSql += " SNO,ISSSNO,COMPANYID,COSTID,STONEUNIT"
                            strSql += " ,STNITEMID,CATCODE,TRANDATE,TRANNO,TRANTYPE"
                            strSql += " ,STNPCS,STNWT,STNAMT,BATCHNO "
                            strSql += " ,SYSTEMID,APPVER)VALUES("
                            strSql += " '" & IssNo & "'" 'SNO
                            strSql += " ,'" & Sno & "'" 'ISSSNO
                            strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                            strSql += " ,'" & cnHOCostId & "'" 'COSTID
                            strSql += " ,'C'" 'STONEUNIT
                            strSql += " ," & StnItemid & "" ''STNITEMID
                            strSql += ",'" & StnCatcode & "'" 'CATCODE
                            strSql += ",'" & trdate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                            strSql += " ," & NEWBILLNO & "" ''TRANNO
                            strSql += " ,'RIN'" ''TRANTYPE
                            strSql += " ," & Val(StnPcs) & "" 'STNPCS
                            strSql += " ," & Val(StnWt) & "" 'STNWT
                            strSql += " ," & Val(StnAmt) & "" 'STNAMT
                            strSql += " ,'" & Batchno & "' " 'BATCHNO
                            strSql += " ,'" & systemId & "'" 'SYSTEMID
                            strSql += " ,'" & VERSION & "'" 'APPVER
                            strSql += " )"
                            If CENTR_DB_BR Then
                                cmd = New OleDbCommand(strSql, cn, tran)
                                cmd.ExecuteNonQuery()
                            Else
                                ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId, , , "TRECEIPTSTONE", False)
                            End If

                            '' newly added on 05-MAY-2022 for sep ps/sr catcode posting
                            If Not PSSR_CATPOSTING Is Nothing And (Trantype = "PARTSALE" Or Trantype = "SALERETURN") Then
                                Dim _tempPssr_post() As String
                                Dim _tempPssr_Catcode As String = ""
                                For cnt As Integer = 0 To PSSR_CATPOSTING.Length - 1
                                    If PSSR_CATPOSTING(cnt).ToString.Contains(StnCatcode & ":") Then
                                        _tempPssr_post = PSSR_CATPOSTING(cnt).ToString.Split(":")
                                        If Trantype = "PARTSALE" And _tempPssr_post.Length = 3 Then
                                            _tempPssr_Catcode = _tempPssr_post(1).ToString
                                        ElseIf Trantype = "SALERETURN" And _tempPssr_post.Length = 3 Then
                                            _tempPssr_Catcode = _tempPssr_post(2).ToString
                                        End If
                                        Exit For
                                    End If
                                Next
                                If _tempPssr_post.Length = 3 And _tempPssr_Catcode <> "" Then
                                    IssNo = GetNewSno(TranSnoType.ISSSTONECODE, tran)
                                    strSql = " INSERT INTO " & cnStockDb & ".." & IIf(CENTR_DB_BR, "TISSSTONE", "ISSSTONE")
                                    strSql += " ("
                                    strSql += " SNO,ISSSNO,COMPANYID,COSTID,STONEUNIT"
                                    strSql += " ,STNITEMID,CATCODE,TRANDATE,TRANNO,TRANTYPE"
                                    strSql += " ,STNPCS,STNWT,STNAMT,BATCHNO "
                                    strSql += " ,SYSTEMID,APPVER)VALUES("
                                    strSql += " '" & IssNo & "'" 'SNO
                                    strSql += " ,'" & _PSSRSNO & "'" 'ISSSNO
                                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                                    strSql += " ,'" & cnHOCostId & "'" 'COSTID
                                    strSql += " ,'C'" 'STONEUNIT
                                    strSql += " ," & StnItemid & "" ''STNITEMID
                                    strSql += ",'" & StnCatcode & "'" 'CATCODE
                                    strSql += ",'" & trdate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                                    strSql += " ," & NEWBILLNO & "" ''TRANNO
                                    strSql += " ,'IIN'" ''TRANTYPE
                                    strSql += " ," & Val(StnPcs) & "" 'STNPCS
                                    strSql += " ," & Val(StnWt) & "" 'STNWT
                                    strSql += " ," & Val(0) & "" 'STNAMT
                                    strSql += " ,'" & Batchno & "' " 'BATCHNO
                                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                                    strSql += " ,'" & VERSION & "'" 'APPVER
                                    strSql += " )"
                                    If CENTR_DB_BR Then
                                        cmd = New OleDbCommand(strSql, cn, tran)
                                        cmd.ExecuteNonQuery()
                                    Else
                                        ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId, , , "TISSSTONE", False)
                                    End If

                                    strSql = " INSERT INTO " & cnStockDb & ".." & IIf(CENTR_DB_BR, "TRECEIPTSTONE", "RECEIPTSTONE")
                                    strSql += " ("
                                    strSql += " SNO,ISSSNO,COMPANYID,COSTID,STONEUNIT"
                                    strSql += " ,STNITEMID,CATCODE,TRANDATE,TRANNO,TRANTYPE"
                                    strSql += " ,STNPCS,STNWT,STNAMT,BATCHNO "
                                    strSql += " ,SYSTEMID,APPVER)VALUES("
                                    strSql += " '" & IssNo & "'" 'SNO
                                    strSql += " ,'" & _PSSRSNO & "'" 'ISSSNO
                                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                                    strSql += " ,'" & cnHOCostId & "'" 'COSTID
                                    strSql += " ,'C'" 'STONEUNIT
                                    strSql += " ," & StnItemid & "" ''STNITEMID
                                    strSql += ",'" & _tempPssr_Catcode & "'" 'CATCODE
                                    strSql += ",'" & trdate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                                    strSql += " ," & NEWBILLNO & "" ''TRANNO
                                    strSql += " ,'RIN'" ''TRANTYPE
                                    strSql += " ," & Val(StnPcs) & "" 'STNPCS
                                    strSql += " ," & Val(StnWt) & "" 'STNWT
                                    strSql += " ," & Val(0) & "" 'STNAMT
                                    strSql += " ,'" & Batchno & "' " 'BATCHNO
                                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                                    strSql += " ,'" & VERSION & "'" 'APPVER
                                    strSql += " )"
                                    If CENTR_DB_BR Then
                                        cmd = New OleDbCommand(strSql, cn, tran)
                                        cmd.ExecuteNonQuery()
                                    Else
                                        ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId, , , "TRECEIPTSTONE", False)
                                    End If
                                End If
                            End If
                            '' END newly added on 05-MAY-2022 for sep ps/sr catcode posting

                        End If
                        If Diawt > 0 Then
                            Dim IssNo As String = GetNewSno(TranSnoType.ISSSTONECODE, tran)
                            If Trantype <> "MISCISSUE" Then
                                strSql = " INSERT INTO " & cnStockDb & "..ISSSTONE"
                                strSql += " ("
                                strSql += " SNO,ISSSNO,COMPANYID,COSTID,STONEUNIT"
                                strSql += " ,STNITEMID,CATCODE,TRANDATE,TRANNO,TRANTYPE"
                                strSql += " ,STNPCS,STNWT,STNAMT,BATCHNO "
                                strSql += " ,SYSTEMID,APPVER)VALUES("
                                strSql += " '" & IssNo & "'" 'SNO
                                strSql += " ,'" & Sno & "'" 'ISSSNO
                                strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                                strSql += " ,'" & costId & "'" 'COSTID
                                strSql += " ,'C'" 'STONEUNIT
                                strSql += " ," & StnItemid & "" ''STNITEMID
                                strSql += ",'" & StnCatcode & "'" 'CATCODE
                                strSql += ",'" & trdate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                                strSql += " ," & NEWBILLNO & "" ''TRANNO
                                strSql += " ,'IIN'" ''TRANTYPE
                                strSql += " ," & Val(DiaPcs) & "" 'STNPCS
                                strSql += " ," & Val(Diawt) & "" 'STNWT
                                strSql += " ," & Val(StnAmt) & "" 'STNAMT
                                strSql += " ,'" & Batchno & "' " 'BATCHNO
                                strSql += " ,'" & systemId & "'" 'SYSTEMID
                                strSql += " ,'" & VERSION & "'" 'APPVER
                                strSql += " )"
                                If CENTR_DB_BR Then
                                    cmd = New OleDbCommand(strSql, cn, tran)
                                    cmd.ExecuteNonQuery()
                                Else
                                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
                                End If
                            End If
                            strSql = " INSERT INTO " & cnStockDb & ".." & IIf(CENTR_DB_BR, "TRECEIPTSTONE", "RECEIPTSTONE")
                            strSql += " ("
                            strSql += " SNO,ISSSNO,COMPANYID,COSTID,STONEUNIT"
                            strSql += " ,STNITEMID,CATCODE,TRANDATE,TRANNO,TRANTYPE"
                            strSql += " ,STNPCS,STNWT,STNAMT,BATCHNO "
                            strSql += " ,SYSTEMID,APPVER)VALUES("
                            strSql += " '" & IssNo & "'" 'SNO
                            strSql += " ,'" & Sno & "'" 'ISSSNO
                            strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                            strSql += " ,'" & cnHOCostId & "'" 'COSTID
                            strSql += " ,'C'" 'STONEUNIT
                            strSql += " ," & StnItemid & "" ''STNITEMID
                            strSql += ",'" & StnCatcode & "'" 'CATCODE
                            strSql += ",'" & trdate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                            strSql += " ," & NEWBILLNO & "" ''TRANNO
                            strSql += " ,'RIN'" ''TRANTYPE
                            strSql += " ," & Val(DiaPcs) & "" 'STNPCS
                            strSql += " ," & Val(Diawt) & "" 'STNWT
                            strSql += " ," & Val(StnAmt) & "" 'STNAMT
                            strSql += " ,'" & Batchno & "' " 'BATCHNO
                            strSql += " ,'" & systemId & "'" 'SYSTEMID
                            strSql += " ,'" & VERSION & "'" 'APPVER
                            strSql += " )"
                            If CENTR_DB_BR Then
                                cmd = New OleDbCommand(strSql, cn, tran)
                                cmd.ExecuteNonQuery()
                            Else
                                ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId, , , "TRECEIPTSTONE", False)
                            End If

                            '' newly added on 05-MAY-2022 for sep ps/sr catcode posting
                            If Not PSSR_CATPOSTING Is Nothing And (Trantype = "PARTSALE" Or Trantype = "SALERETURN") Then
                                Dim _tempPssr_post() As String
                                Dim _tempPssr_Catcode As String = ""
                                For cnt As Integer = 0 To PSSR_CATPOSTING.Length - 1
                                    If PSSR_CATPOSTING(cnt).ToString.Contains(StnCatcode & ":") Then
                                        _tempPssr_post = PSSR_CATPOSTING(cnt).ToString.Split(":")
                                        If Trantype = "PARTSALE" And _tempPssr_post.Length = 3 Then
                                            _tempPssr_Catcode = _tempPssr_post(1).ToString
                                        ElseIf Trantype = "SALERETURN" And _tempPssr_post.Length = 3 Then
                                            _tempPssr_Catcode = _tempPssr_post(2).ToString
                                        End If
                                        Exit For
                                    End If
                                Next
                                If _tempPssr_post.Length = 3 And _tempPssr_Catcode <> "" Then
                                    IssNo = GetNewSno(TranSnoType.ISSSTONECODE, tran)
                                    strSql = " INSERT INTO " & cnStockDb & ".." & IIf(CENTR_DB_BR, "TISSSTONE", "ISSSTONE")
                                    strSql += " ("
                                    strSql += " SNO,ISSSNO,COMPANYID,COSTID,STONEUNIT"
                                    strSql += " ,STNITEMID,CATCODE,TRANDATE,TRANNO,TRANTYPE"
                                    strSql += " ,STNPCS,STNWT,STNAMT,BATCHNO "
                                    strSql += " ,SYSTEMID,APPVER)VALUES("
                                    strSql += " '" & IssNo & "'" 'SNO
                                    strSql += " ,'" & _PSSRSNO & "'" 'ISSSNO
                                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                                    strSql += " ,'" & cnHOCostId & "'" 'COSTID
                                    strSql += " ,'C'" 'STONEUNIT
                                    strSql += " ," & StnItemid & "" ''STNITEMID
                                    strSql += ",'" & StnCatcode & "'" 'CATCODE
                                    strSql += ",'" & trdate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                                    strSql += " ," & NEWBILLNO & "" ''TRANNO
                                    strSql += " ,'IIN'" ''TRANTYPE
                                    strSql += " ," & Val(DiaPcs) & "" 'STNPCS
                                    strSql += " ," & Val(Diawt) & "" 'STNWT
                                    strSql += " ," & Val(0) & "" 'STNAMT
                                    strSql += " ,'" & Batchno & "' " 'BATCHNO
                                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                                    strSql += " ,'" & VERSION & "'" 'APPVER
                                    strSql += " )"
                                    If CENTR_DB_BR Then
                                        cmd = New OleDbCommand(strSql, cn, tran)
                                        cmd.ExecuteNonQuery()
                                    Else
                                        ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId, , , "TISSSTONE", False)
                                    End If

                                    strSql = " INSERT INTO " & cnStockDb & ".." & IIf(CENTR_DB_BR, "TRECEIPTSTONE", "RECEIPTSTONE")
                                    strSql += " ("
                                    strSql += " SNO,ISSSNO,COMPANYID,COSTID,STONEUNIT"
                                    strSql += " ,STNITEMID,CATCODE,TRANDATE,TRANNO,TRANTYPE"
                                    strSql += " ,STNPCS,STNWT,STNAMT,BATCHNO "
                                    strSql += " ,SYSTEMID,APPVER)VALUES("
                                    strSql += " '" & IssNo & "'" 'SNO
                                    strSql += " ,'" & _PSSRSNO & "'" 'ISSSNO
                                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                                    strSql += " ,'" & cnHOCostId & "'" 'COSTID
                                    strSql += " ,'C'" 'STONEUNIT
                                    strSql += " ," & StnItemid & "" ''STNITEMID
                                    strSql += ",'" & _tempPssr_Catcode & "'" 'CATCODE
                                    strSql += ",'" & trdate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                                    strSql += " ," & NEWBILLNO & "" ''TRANNO
                                    strSql += " ,'RIN'" ''TRANTYPE
                                    strSql += " ," & Val(DiaPcs) & "" 'STNPCS
                                    strSql += " ," & Val(Diawt) & "" 'STNWT
                                    strSql += " ," & Val(0) & "" 'STNAMT
                                    strSql += " ,'" & Batchno & "' " 'BATCHNO
                                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                                    strSql += " ,'" & VERSION & "'" 'APPVER
                                    strSql += " )"
                                    If CENTR_DB_BR Then
                                        cmd = New OleDbCommand(strSql, cn, tran)
                                        cmd.ExecuteNonQuery()
                                    Else
                                        ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId, , , "TRECEIPTSTONE", False)
                                    End If
                                End If
                            End If
                            '' END newly added on 05-MAY-2022 for sep ps/sr catcode posting

                        End If
                        If PreWt > 0 Then
                            Dim IssNo As String = GetNewSno(TranSnoType.ISSSTONECODE, tran)
                            If Trantype <> "MISCISSUE" Then
                                strSql = " INSERT INTO " & cnStockDb & "..ISSSTONE"
                                strSql += " ("
                                strSql += " SNO,ISSSNO,COMPANYID,COSTID,STONEUNIT"
                                strSql += " ,STNITEMID,CATCODE,TRANDATE,TRANNO,TRANTYPE"
                                strSql += " ,STNPCS,STNWT,STNAMT,BATCHNO "
                                strSql += " ,SYSTEMID,APPVER)VALUES("
                                strSql += " '" & IssNo & "'" 'SNO
                                strSql += " ,'" & Sno & "'" 'ISSSNO
                                strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                                strSql += " ,'" & costId & "'" 'COSTID
                                strSql += " ,'C'" 'STONEUNIT
                                strSql += " ," & StnItemid & "" ''STNITEMID
                                strSql += ",'" & StnCatcode & "'" 'CATCODE
                                strSql += ",'" & trdate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                                strSql += " ," & NEWBILLNO & "" ''TRANNO
                                strSql += " ,'IIN'" ''TRANTYPE
                                strSql += " ," & Val(PrePcs) & "" 'STNPCS
                                strSql += " ," & Val(PreWt) & "" 'STNWT
                                strSql += " ," & Val(StnAmt) & "" 'STNAMT
                                strSql += " ,'" & Batchno & "' " 'BATCHNO
                                strSql += " ,'" & systemId & "'" 'SYSTEMID
                                strSql += " ,'" & VERSION & "'" 'APPVER
                                strSql += " )"
                                If CENTR_DB_BR Then
                                    cmd = New OleDbCommand(strSql, cn, tran)
                                    cmd.ExecuteNonQuery()
                                Else
                                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
                                End If
                            End If
                            strSql = " INSERT INTO " & cnStockDb & ".." & IIf(CENTR_DB_BR, "TRECEIPTSTONE", "RECEIPTSTONE")
                            strSql += " ("
                            strSql += " SNO,ISSSNO,COMPANYID,COSTID,STONEUNIT"
                            strSql += " ,STNITEMID,CATCODE,TRANDATE,TRANNO,TRANTYPE"
                            strSql += " ,STNPCS,STNWT,STNAMT,BATCHNO "
                            strSql += " ,SYSTEMID,APPVER)VALUES("
                            strSql += " '" & IssNo & "'" 'SNO
                            strSql += " ,'" & Sno & "'" 'ISSSNO
                            strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                            strSql += " ,'" & cnHOCostId & "'" 'COSTID
                            strSql += " ,'C'" 'STONEUNIT
                            strSql += " ," & StnItemid & "" ''STNITEMID
                            strSql += ",'" & StnCatcode & "'" 'CATCODE
                            strSql += ",'" & trdate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                            strSql += " ," & NEWBILLNO & "" ''TRANNO
                            strSql += " ,'RIN'" ''TRANTYPE
                            strSql += " ," & Val(PrePcs) & "" 'STNPCS
                            strSql += " ," & Val(PreWt) & "" 'STNWT
                            strSql += " ," & Val(StnAmt) & "" 'STNAMT
                            strSql += " ,'" & Batchno & "' " 'BATCHNO
                            strSql += " ,'" & systemId & "'" 'SYSTEMID
                            strSql += " ,'" & VERSION & "'" 'APPVER
                            strSql += " )"
                            If CENTR_DB_BR Then
                                cmd = New OleDbCommand(strSql, cn, tran)
                                cmd.ExecuteNonQuery()
                            Else
                                ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId, , , "TRECEIPTSTONE", False)
                            End If

                            '' newly added on 05-MAY-2022 for sep ps/sr catcode posting
                            If Not PSSR_CATPOSTING Is Nothing And (Trantype = "PARTSALE" Or Trantype = "SALERETURN") Then
                                Dim _tempPssr_post() As String
                                Dim _tempPssr_Catcode As String = ""
                                For cnt As Integer = 0 To PSSR_CATPOSTING.Length - 1
                                    If PSSR_CATPOSTING(cnt).ToString.Contains(StnCatcode & ":") Then
                                        _tempPssr_post = PSSR_CATPOSTING(cnt).ToString.Split(":")
                                        If Trantype = "PARTSALE" And _tempPssr_post.Length = 3 Then
                                            _tempPssr_Catcode = _tempPssr_post(1).ToString
                                        ElseIf Trantype = "SALERETURN" And _tempPssr_post.Length = 3 Then
                                            _tempPssr_Catcode = _tempPssr_post(2).ToString
                                        End If
                                        Exit For
                                    End If
                                Next
                                If _tempPssr_post.Length = 3 And _tempPssr_Catcode <> "" Then
                                    IssNo = GetNewSno(TranSnoType.ISSSTONECODE, tran)
                                    strSql = " INSERT INTO " & cnStockDb & ".." & IIf(CENTR_DB_BR, "TISSSTONE", "ISSSTONE")
                                    strSql += " ("
                                    strSql += " SNO,ISSSNO,COMPANYID,COSTID,STONEUNIT"
                                    strSql += " ,STNITEMID,CATCODE,TRANDATE,TRANNO,TRANTYPE"
                                    strSql += " ,STNPCS,STNWT,STNAMT,BATCHNO "
                                    strSql += " ,SYSTEMID,APPVER)VALUES("
                                    strSql += " '" & IssNo & "'" 'SNO
                                    strSql += " ,'" & _PSSRSNO & "'" 'ISSSNO
                                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                                    strSql += " ,'" & cnHOCostId & "'" 'COSTID
                                    strSql += " ,'C'" 'STONEUNIT
                                    strSql += " ," & StnItemid & "" ''STNITEMID
                                    strSql += ",'" & StnCatcode & "'" 'CATCODE
                                    strSql += ",'" & trdate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                                    strSql += " ," & NEWBILLNO & "" ''TRANNO
                                    strSql += " ,'IIN'" ''TRANTYPE
                                    strSql += " ," & Val(PrePcs) & "" 'STNPCS
                                    strSql += " ," & Val(PreWt) & "" 'STNWT
                                    strSql += " ," & Val(0) & "" 'STNAMT
                                    strSql += " ,'" & Batchno & "' " 'BATCHNO
                                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                                    strSql += " ,'" & VERSION & "'" 'APPVER
                                    strSql += " )"
                                    If CENTR_DB_BR Then
                                        cmd = New OleDbCommand(strSql, cn, tran)
                                        cmd.ExecuteNonQuery()
                                    Else
                                        ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId, , , "TISSSTONE", False)
                                    End If

                                    strSql = " INSERT INTO " & cnStockDb & ".." & IIf(CENTR_DB_BR, "TRECEIPTSTONE", "RECEIPTSTONE")
                                    strSql += " ("
                                    strSql += " SNO,ISSSNO,COMPANYID,COSTID,STONEUNIT"
                                    strSql += " ,STNITEMID,CATCODE,TRANDATE,TRANNO,TRANTYPE"
                                    strSql += " ,STNPCS,STNWT,STNAMT,BATCHNO "
                                    strSql += " ,SYSTEMID,APPVER)VALUES("
                                    strSql += " '" & IssNo & "'" 'SNO
                                    strSql += " ,'" & _PSSRSNO & "'" 'ISSSNO
                                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                                    strSql += " ,'" & cnHOCostId & "'" 'COSTID
                                    strSql += " ,'C'" 'STONEUNIT
                                    strSql += " ," & StnItemid & "" ''STNITEMID
                                    strSql += ",'" & _tempPssr_Catcode & "'" 'CATCODE
                                    strSql += ",'" & trdate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                                    strSql += " ," & NEWBILLNO & "" ''TRANNO
                                    strSql += " ,'RIN'" ''TRANTYPE
                                    strSql += " ," & Val(PrePcs) & "" 'STNPCS
                                    strSql += " ," & Val(PreWt) & "" 'STNWT
                                    strSql += " ," & Val(0) & "" 'STNAMT
                                    strSql += " ,'" & Batchno & "' " 'BATCHNO
                                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                                    strSql += " ,'" & VERSION & "'" 'APPVER
                                    strSql += " )"
                                    If CENTR_DB_BR Then
                                        cmd = New OleDbCommand(strSql, cn, tran)
                                        cmd.ExecuteNonQuery()
                                    Else
                                        ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId, , , "TRECEIPTSTONE", False)
                                    End If
                                End If
                            End If
                            '' END newly added on 05-MAY-2022 for sep ps/sr catcode posting

                        End If
                    End If
                    If Amount > 0 And AUTOBOOK_VOUCHER = "Y" Then

                        If STKTRAN_ContraPost Then
                            strSql = InsertIntoAcctran(costId, NEWBILLNO, trdate.ToString("yyyy-MM-dd"), Batchno, Accode, BAccode, Val(Amount), "C", PTrfNo)
                            ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
                            strSql = InsertIntoAcctran(costId, NEWBILLNO, trdate.ToString("yyyy-MM-dd"), Batchno, BAccode, Accode, Format(Val(Amount) - Val(IGST), "0.00"), "D", PTrfNo)
                            ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)

                            strSql = InsertIntoAcctran(costId, NEWBILLNO, trdate.ToString("yyyy-MM-dd"), Batchno, BAccode, Accode, Val(Amount) - Val(IGST), "C", PTrfNo)
                            ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId, , , "TACCTRAN", False)
                            strSql = InsertIntoAcctran(costId, NEWBILLNO, trdate.ToString("yyyy-MM-dd"), Batchno, Accode, BAccode, Val(Amount), "D", PTrfNo)
                            ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId, , , "TACCTRAN", False)
                        Else
                            strSql = InsertIntoAcctran(costId, NEWBILLNO, trdate.ToString("yyyy-MM-dd"), Batchno, Accode, TAccode, Val(Amount), "D", PTrfNo)
                            ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
                            strSql = InsertIntoAcctran(costId, NEWBILLNO, trdate.ToString("yyyy-MM-dd"), Batchno, TAccode, Accode, Format(Val(Amount) - Val(IGST), "0.00"), "C", PTrfNo)
                            ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)

                            strSql = InsertIntoAcctran(costId, NEWBILLNO, trdate.ToString("yyyy-MM-dd"), Batchno, TAccode, BAccode, Val(Amount) - Val(IGST), "D", PTrfNo)
                            ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId, , , "TACCTRAN", False)
                            strSql = InsertIntoAcctran(costId, NEWBILLNO, trdate.ToString("yyyy-MM-dd"), Batchno, BAccode, TAccode, Val(Amount), "C", PTrfNo)
                            ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId, , , "TACCTRAN", False)
                        End If


                        'newly added
                        If AUTOBOOK_VOUCHER = "Y" Then ' If chkRepairentry.Checked = True Then
                            If IGST > 0 Then
                                If STKTRAN_ContraPost Then
                                    strSql = InsertIntoAcctran(costId, NEWBILLNO, trdate.ToString("yyyy-MM-dd"), Batchno, IGSTAccode, Accode, Val(IGST), "D", PTrfNo)
                                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)

                                    strSql = InsertIntoAcctran(costId, NEWBILLNO, trdate.ToString("yyyy-MM-dd"), Batchno, ICode, BAccode, Val(IGST), "C", PTrfNo)
                                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId, , , "TACCTRAN", False)
                                Else
                                    strSql = InsertIntoAcctran(costId, NEWBILLNO, trdate.ToString("yyyy-MM-dd"), Batchno, IGSTAccode, Accode, Val(IGST), "C", PTrfNo)
                                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)

                                    strSql = InsertIntoAcctran(costId, NEWBILLNO, trdate.ToString("yyyy-MM-dd"), Batchno, ICode, TAccode, Val(IGST), "D", PTrfNo)
                                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId, , , "TACCTRAN", False)
                                End If

                                If GST And StateId <> CompanyStateId And Val(Amount) > 0 And Val(IGST) > 0 Then
                                    If GST And StateId <> CompanyStateId And IGST > 0 Then
                                        strSql = "INSERT INTO " & cnStockDb & "..TAXTRAN"
                                        strSql += " ("
                                        strSql += " SNO,ISSSNO,ACCODE,CONTRA,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID,AMOUNT,TAXPER,TAXAMOUNT,TAXTYPE,TSNO,COSTID"
                                        strSql += " ,COMPANYID,STUDDED)VALUES("
                                        strSql += " '" & GetNewSnoNew(TranSnoType.TAXTRANCODE, cn, tran) & "'" 'SNO
                                        strSql += " ,'" & Sno & "'" 'ISSSNO
                                        strSql += " ,''" 'ACCODE
                                        strSql += " ,''" 'CONTRA
                                        strSql += " ," & NEWBILLNO & "" 'TRANNO
                                        strSql += " ,'" & trdate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                                        strSql += " ,'IIN'" 'TRANTYPE
                                        strSql += " ,'" & Batchno & "'" 'BATCHNO
                                        strSql += " ,'IG'" 'TAXID
                                        strSql += " ," & Val(Amount) - Val(IGST) & "" 'AMOUNT
                                        strSql += " ," & Val(IGSTPER) & "" 'TAXPER
                                        If Val(Amount) = 0 Then
                                            IGST = 0
                                        End If
                                        strSql += " ," & Val(IGST) & "" 'TAXAMOUNT
                                        strSql += " ,''"  'TAXTYPE
                                        strSql += " ,3" 'TSNO
                                        strSql += " ,'" & costId & "'" 'COSTID
                                        strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                                        strSql += " ,'N'"  'STUDDED
                                        strSql += " )"
                                        ExecQuery(SyncMode.Transaction, strSql, cn, tran, costId)
                                    End If
                                End If
                            End If
                        End If
                        'End Newly Added

                    End If
                End With
            Next
            For Each row As DataRow In dt.Select("TRANTYPE='PARTSALE' AND CHECK = TRUE", Nothing)
                strSql = " UPDATE " & cnStockDb & "..ISSUE"
                strSql += vbCrLf + "  SET TRFNO = '" & PTrfNo & "',TRANFLAG ='S'"
                strSql += vbCrLf + "  WHERE SNO = '" & row.Item("SNO").ToString & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                If preTranDb <> Nothing Then
                    strSql = " UPDATE " & preTranDb & "..ISSUE"
                    strSql += vbCrLf + "  SET TRFNO = '" & PTrfNo & "',TRANFLAG ='S'"
                    strSql += vbCrLf + "  WHERE SNO = '" & row.Item("SNO").ToString & "'"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                End If
            Next
            For Each row As DataRow In dt.Select("TRANTYPE='MISCISSUE' AND CHECK = TRUE", Nothing)
                strSql = " UPDATE " & cnStockDb & "..ISSUE"
                strSql += vbCrLf + "  SET TRFNO = '" & PTrfNo & "',TRANFLAG ='S'"
                strSql += vbCrLf + "  WHERE SNO = '" & row.Item("SNO").ToString & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                If preTranDb <> Nothing Then
                    strSql = " UPDATE " & preTranDb & "..ISSUE"
                    strSql += vbCrLf + "  SET TRFNO = '" & PTrfNo & "',TRANFLAG ='S'"
                    strSql += vbCrLf + "  WHERE SNO = '" & row.Item("SNO").ToString & "'"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                End If
            Next
            For Each row As DataRow In dt.Select("TRANTYPE='PURCHASE' AND CHECK = TRUE", Nothing)
                strSql = " UPDATE " & cnStockDb & "..RECEIPT"
                strSql += vbCrLf + "  SET TRFNO = '" & PTrfNo & "',TRANFLAG ='S'"
                strSql += vbCrLf + "  WHERE SNO = '" & row.Item("SNO").ToString & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                If preTranDb <> Nothing Then
                    strSql = " UPDATE " & preTranDb & "..RECEIPT"
                    strSql += vbCrLf + "  SET TRFNO = '" & PTrfNo & "',TRANFLAG ='S'"
                    strSql += vbCrLf + "  WHERE SNO = '" & row.Item("SNO").ToString & "'"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                End If
            Next
            For Each row As DataRow In dt.Select("TRANTYPE='SALERETURN' AND CHECK = TRUE", Nothing)
                strSql = " UPDATE " & cnStockDb & "..RECEIPT"
                strSql += vbCrLf + "  SET TRFNO = '" & PTrfNo & "',TRANFLAG ='S'"
                strSql += vbCrLf + "  WHERE SNO = '" & row.Item("SNO").ToString & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                If preTranDb <> Nothing Then
                    strSql = " UPDATE " & preTranDb & "..RECEIPT"
                    strSql += vbCrLf + "  SET TRFNO = '" & PTrfNo & "',TRANFLAG ='S'"
                    strSql += vbCrLf + "  WHERE SNO = '" & row.Item("SNO").ToString & "'"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                End If
            Next

            tran.Commit()
            tran = Nothing
            MsgBox("Transfered successfully.." & vbCrLf & "Tranfer No.  : " & PTrfNo & " Generated..")
            Dim Pbatchno As String = Batchno
            Dim Pbilldate As Date = trdate
            Dim prnmemsuffix As String = ""
            If GetAdmindbSoftValuefromDt(dtSoftKeys, "PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
            Dim BillPrintExe As Boolean = IIf(GetAdmindbSoftValue("CALLBILLPRINTEXE", "Y") = "Y", True, False)
            Dim BillPrint_Format As String = GetAdmindbSoftValue("BILLPRINT_FORMAT", "")
            If GST And BillPrint_Format.ToString = "M1" Then
                Dim obj1 As New BrighttechREPORT.frmBillPrintDocA4N("POS", Pbatchno.ToString, Pbilldate.ToString("yyyy-MM-dd"), "N")
                Me.Refresh()
            ElseIf GST And BillPrint_Format.ToString = "M2" Then
                Dim obj1 As New BrighttechREPORT.frmBillPrintDocB5("POS", Pbatchno.ToString, Pbilldate.ToString("yyyy-MM-dd"), "N")
                Me.Refresh()
            ElseIf GST And BillPrint_Format.ToString = "M3" Then
                Dim obj1 As New BrighttechREPORT.frmBillPrintDocA5("POS", Pbatchno.ToString, Pbilldate.ToString("yyyy-MM-dd"), "N")
                Me.Refresh()
            ElseIf GST And BillPrint_Format.ToString = "M4" Then
                Dim obj1 As New BrighttechREPORT.frmBillPrintDocB52cpy("POS", Pbatchno.ToString, Pbilldate.ToString("yyyy-MM-dd"), "N")
                Me.Refresh()
            ElseIf GST And BillPrintExe = False Then
                Dim billDoc As New frmBillPrintDoc("POS", Pbatchno.ToString, Pbilldate.ToString("yyyy-MM-dd"), "N")
                Me.Refresh()
            Else
                If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                    ''Dim prnmemsuffix As String = ""
                    ''If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                    Dim memfile As String = "\BillPrint" + prnmemsuffix.Trim & ".mem"
                    Dim write As IO.StreamWriter
                    write = IO.File.CreateText(Application.StartupPath & memfile)
                    write.WriteLine(LSet("TYPE", 15) & ":IIN")
                    write.WriteLine(LSet("BATCHNO", 15) & ":" & Pbatchno)
                    write.WriteLine(LSet("TRANDATE", 15) & ":" & Pbilldate.ToString("yyyy-MM-dd"))
                    write.WriteLine(LSet("DUPLICATE", 15) & ":N")
                    write.Flush()
                    write.Close()
                    If EXE_WITH_PARAM = False Then
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                    Else
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe",
                        LSet("TYPE", 15) & ":IIN" & ";" &
                        LSet("BATCHNO", 15) & ":" & Pbatchno & ";" &
                        LSet("TRANDATE", 15) & ":" & Pbilldate.ToString("yyyy-MM-dd") & ";" &
                        LSet("DUPLICATE", 15) & ":N")
                    End If
                Else
                    MsgBox("Billprint exe not found", MsgBoxStyle.Information)
                End If
            End If
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback() : tran = Nothing
            btnNew_Click(Me, New EventArgs)
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Function InsertIntoAcctran(ByVal costId As String, ByVal NEWBILLNO As Integer _
                                , ByVal trdate As String, ByVal Batchno As String _
                                , ByVal Accode As String, ByVal Contra As String _
                                , ByVal Amount As Decimal, ByVal TranMode As String _
                                , ByVal Refno As String) As String
        strSql = "INSERT INTO " & cnStockDb & "..ACCTRAN"
        strSql += " ("
        strSql += " SNO,COMPANYID,COSTID,TRANNO,TRANDATE,BATCHNO"
        strSql += " ,ACCODE,CONTRA,AMOUNT,TRANMODE"
        strSql += " ,PAYMODE,SYSTEMID,APPVER,FROMFLAG,USERID,UPDATED,UPTIME"
        strSql += " ,REFNO)"
        strSql += " VALUES"
        strSql += " ("
        strSql += "'" & GetNewSno(TranSnoType.ACCTRANCODE, tran) & "'"
        strSql += ",'" & GetStockCompId() & "'"
        strSql += ",'" & costId & "'"
        strSql += " ," & NEWBILLNO & " "
        strSql += " ,'" & trdate & "'" 'TRANDATE
        strSql += " ,'" & Batchno & "'" 'BATCHNO
        strSql += " ,'" & Accode & "' "     'ACCODE
        strSql += " ,'" & Contra & "' "     'CONTRA
        strSql += " ," & Val(Amount) 'AMOUNT
        strSql += " ,'" & TranMode & "'"
        strSql += " ,'TI'"
        strSql += " ,'" & systemId & "'" 'SYSTEMID
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,'A'"
        strSql += " ," & userId & "" 'USERID
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += ",'" & Refno & "'"
        strSql += " )"
        Return strSql
    End Function

    Private Sub gridView_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellEnter
        Dim pt As Point = gridView.Location
        txtGrid.ReadOnly = False
        txtGrid.Clear()
        txtGrid.SelectAll()
    End Sub

    Private Sub gridView_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellLeave
        Select Case gridView.Columns(e.ColumnIndex).Name

        End Select
        txtGrid.Clear()
        txtGrid.Visible = False
        lstSearch.Visible = False
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        ElseIf e.KeyCode = Keys.Escape Then
            btnTransfer.Focus()

        ElseIf e.KeyCode = Keys.Space Then
            gridView.CurrentRow.Cells("CHECK").Value = Not gridView.CurrentRow.Cells("CHECK").Value
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells(gridView.CurrentCell.ColumnIndex)
        End If
    End Sub

    Private Sub textGridValidator()
        If lstSearch.Items.Contains(txtGrid.Text) = False Then
            txtGrid.Text = tempText.Text
        End If
    End Sub

    Private Sub Trfsummary()
        dtSummary.Clear()
        DgTrfSummary.DataSource = Nothing
        Dim dts As New DataTable
        dts = DgvView.DataSource
        dts.AcceptChanges()
        Dim ross() As DataRow = Nothing
        ross = dts.Select("CHECK = TRUE")
        If Not ross.Length > 0 Then
            MsgBox("There is no checked record", MsgBoxStyle.Information)
            Exit Sub
        End If
        Dim dtsitemdet As DataTable = dts.DefaultView.ToTable(True, "CATCODE")
        Dim mitemid As String
        For II As Integer = 0 To dtsitemdet.Rows.Count - 1
            mitemid = dtsitemdet.Rows(II).Item("CATCODE").ToString
            Dim mpcs As Integer = 0
            Dim mgrswt As Decimal = 0
            Dim mnetwt As Decimal = 0
            Dim mdiapcs As Integer = 0
            Dim mdiawt As Decimal = 0
            Dim mstnwt As Decimal = 0
            Dim mPrewt As Decimal = 0
            Dim mAmount As Decimal = 0
            Dim mStnAmt As Decimal = 0
            For Each row As DataRow In ross
                If row.Item("CATCODE").ToString = mitemid.ToString Then
                    mpcs += Val(row.Item("PCS").ToString)
                    mgrswt += Val(row.Item("GRSWT").ToString)
                    mnetwt += Val(row.Item("NETWT").ToString)
                    mdiapcs += Val(row.Item("DIAPCS").ToString)
                    mdiawt += Val(row.Item("DIAWT").ToString)
                    mstnwt += Val(row.Item("STNWT").ToString)
                    mPrewt += Val(row.Item("PREWT").ToString)
                    mAmount += Val(row.Item("AMOUNT").ToString)
                    mStnAmt += Val(row.Item("STNAMT").ToString)
                End If
            Next
            Dim drows As DataRow
            drows = dtSummary.NewRow
            strSql = "SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & mitemid & "'"
            drows!CATCODE = mitemid
            drows!CATNAME = objGPack.GetSqlValue(strSql).ToString
            drows!PCS = IIf(mpcs = 0, DBNull.Value, mpcs)
            drows!GRSWT = Format(mgrswt, "#0.000")
            drows!NETWT = Format(mnetwt, "#0.000")
            drows!DIAPCS = IIf(mdiapcs = 0, DBNull.Value, mdiapcs)
            drows!DIAWT = IIf(mdiawt = 0, DBNull.Value, Format(mdiawt, "#0.000"))
            drows!STNWT = IIf(mstnwt = 0, DBNull.Value, Format(mstnwt, "#0.000"))
            drows!PREWT = IIf(mPrewt = 0, DBNull.Value, Format(mPrewt, "#0.000"))
            drows!STNAMT = IIf(mStnAmt = 0, DBNull.Value, Format(mStnAmt, "#0.00"))
            drows!AMOUNT = IIf(mAmount = 0, DBNull.Value, Format(mAmount, "#0.00"))
            dtSummary.Rows.Add(drows)
            dtSummary.AcceptChanges()
            If II = dtsitemdet.Rows.Count - 1 Then
                drows = dtSummary.NewRow
                drows!CATNAME = "TOTAL"
                drows!PCS = dtSummary.Compute("SUM(PCS)", Nothing)
                drows!GRSWT = dtSummary.Compute("SUM(GRSWT)", Nothing)
                drows!NETWT = dtSummary.Compute("SUM(NETWT)", Nothing)
                drows!DIAPCS = dtSummary.Compute("SUM(DIAPCS)", Nothing)
                drows!DIAWT = dtSummary.Compute("SUM(DIAWT)", Nothing)
                drows!STNWT = dtSummary.Compute("SUM(STNWT)", Nothing)
                drows!PREWT = dtSummary.Compute("SUM(PREWT)", Nothing)
                drows!STNAMT = dtSummary.Compute("SUM(STNAMT)", Nothing)
                drows!AMOUNT = dtSummary.Compute("SUM(AMOUNT)", Nothing)
                dtSummary.Rows.Add(drows)
                dtSummary.AcceptChanges()
            End If
        Next
        With DgTrfSummary
            .DataSource = dtSummary
            .Columns("CATNAME").Width = 175
            .Columns("PCS").Width = 40
            .Columns("GRSWT").Width = 90
            .Columns("NETWT").Width = 90
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIAPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIAWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PREWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("STNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("STNAMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("STNWT").Width = 60
            .Columns("DIAPCS").Width = 55
            .Columns("DIAWT").Width = 60
            .Columns("PREWT").Width = 60
            .Columns("AMOUNT").Width = 90
            .Columns("STNAMT").Width = 75
            .Columns("CATCODE").Visible = False
            .Columns("STNPCS").Visible = False
            .Columns("PREPCS").Visible = False
            .Columns("STNCATCODE").Visible = False
            .Columns("STNITEM").Visible = False
            .Columns("KEYNO").Visible = False
            For cnt As Integer = 0 To .ColumnCount - 1
                .Columns(cnt).ReadOnly = True
                .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            gbSummary.Visible = True
            gbSummary.BringToFront()
            DgTrfSummary.Focus()
            .Rows(.Rows.Count - 1).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .Rows(.Rows.Count - 1).DefaultCellStyle.BackColor = Color.LightBlue
            .Rows(.Rows.Count - 1).DefaultCellStyle.ForeColor = Color.Red
        End With
    End Sub
    Private Sub txtGrid_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtGrid.KeyDown
        With gridView
            If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Left Or e.KeyCode = Keys.Right Or e.KeyCode = Keys.Down Or e.KeyCode = Keys.Up Then
                If gridView.Columns(gridView.CurrentCell.ColumnIndex).Name = "NEWITEM" Then
                    textGridValidator()
                ElseIf gridView.Columns(gridView.CurrentCell.ColumnIndex).Name = "NEWCOUNTER" Then
                    textGridValidator()
                End If
            End If

            If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Tab Then
                lstSearch.Visible = False
            ElseIf e.KeyCode = Keys.Up Then
                e.Handled = True
                If .CurrentCell.RowIndex <> 0 Then
                    .CurrentCell = .Rows(.CurrentCell.RowIndex - 1).Cells(.CurrentCell.ColumnIndex)
                End If
            ElseIf e.KeyCode = Keys.Down Then
                e.Handled = True
                If lstSearch.Visible Then
                    lstSearch.Select()
                ElseIf .CurrentCell.RowIndex <> .RowCount - 1 Then
                    .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells(.CurrentCell.ColumnIndex)
                End If
            ElseIf e.KeyCode = Keys.Left Then
                e.Handled = True
                If .CurrentCell.ColumnIndex <> 0 Then
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex - 1)
                End If
            ElseIf e.KeyCode = Keys.Right Then
                e.Handled = True
                If gridView.Columns(gridView.CurrentCell.ColumnIndex).Name <> "NEWCTRID" Then
                    .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells(.CurrentCell.ColumnIndex + 1)
                End If
            End If
        End With
    End Sub

    Private Sub txtGrid_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtGrid.KeyPress

        If e.KeyChar = Chr(Keys.Enter) Then
            With gridView
                If Not .RowCount > 0 Then
                    btnBack.Focus()
                    Exit Sub
                End If
                Select Case .Columns(.CurrentCell.ColumnIndex).Name
                    Case "NEWITEM"
                        Dim NewItemid As Integer
                        NewItemid = objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & txtGrid.Text & "' ", "ITEMID", "0")
                        gridView.CurrentRow.Cells("NEWITEMID").Value = NewItemid
                        gridView.CurrentCell.Value = txtGrid.Text
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells("NEWCOUNTER")
                        .CurrentCell.Selected = True
                    Case "NEWCOUNTER"
                        Dim NewItemCtrid As Integer
                        NewItemCtrid = objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME='" & txtGrid.Text & "' ", "ITEMCTRID", "0")
                        gridView.CurrentRow.Cells("NEWCTRID").Value = NewItemCtrid
                        gridView.CurrentCell.Value = txtGrid.Text
                        If .CurrentCell.RowIndex = .RowCount - 1 Then
                            'gridView.CurrentCell.Value = txtGrid.Text
                            txtGrid.Visible = False
                            btnTransfer.Focus()
                            SendKeys.Send("{TAB}")
                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells("NEWITEM")
                            .CurrentCell.Selected = True
                        End If
                End Select

            End With
        End If
    End Sub

    Private Sub txtGrid_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtGrid.KeyUp
        TextScript(txtGrid, e)
    End Sub
    Private Sub txtGrid_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGrid.TextChanged
        showSearch(txtGrid)
    End Sub

    Private Sub showSearch(ByVal sender As Control)
        loadLstSearch()
        searchSender = sender
        Dim pt As New Point(GetControlLocation(sender, New Point))
        Me.Controls.Add(lstSearch)
        lstSearch.Location = New Point(pt.X, pt.Y + sender.Height)
        If gridView.Columns(gridView.CurrentCell.ColumnIndex).Name = "NEWITEM" Or
        gridView.Columns(gridView.CurrentCell.ColumnIndex).Name = "NEWCOUNTER" Then
            lstSearch.Size = New Size(sender.Width, 80)
        Else
            lstSearch.Size = New Size(sender.Width, 120)
        End If
        lstSearch.BringToFront()
        If lstSearch.Items.Count > 0 Then
            lstSearch.Visible = True
        Else
            lstSearch.Visible = False
        End If
    End Sub
    Private Sub loadLstSearch()
        lstSearch.Items.Clear()
        With gridView
            Select Case .Columns(.CurrentCell.ColumnIndex).Name
                Case "NEWITEM"
                    For i As Integer = 0 To listItem.Count - 1
                        lstSearch.Items.Add(listItem.Item(i))
                    Next
                Case "NEWCOUNTER"
                    For i As Integer = 0 To listCounter.Count - 1
                        lstSearch.Items.Add(listCounter.Item(i))
                    Next
            End Select
        End With
    End Sub
    Private Function GetControlLocation(ByVal ctrl As Control, ByRef pt As Point) As Point
        If TypeOf ctrl Is Form Then
            Return pt
        Else
            pt += ctrl.Location
            Return GetControlLocation(ctrl.Parent, pt)
        End If
        Return pt
    End Function

    Public Sub TextScript(ByRef txt As TextBox, ByVal e As System.Windows.Forms.KeyEventArgs)
        Dim sComboText As String = ""
        Dim iLoop As Integer
        Dim sTempString As String
        If e.KeyCode >= 65 And e.KeyCode <= 90 Then
            'only look at letters A-Z
            sTempString = txt.Text
            If Len(sTempString) = 1 Then sComboText = sTempString
            For iLoop = 0 To (lstSearch.Items.Count - 1)
                If UCase((sTempString & Mid$(lstSearch.Items.Item(iLoop),
                  Len(sTempString) + 1))) = UCase(lstSearch.Items.Item(iLoop)) Then
                    lstSearch.SelectedIndex = iLoop
                    txt.Text = lstSearch.Items.Item(iLoop)
                    txt.SelectionStart = Len(sTempString)
                    txt.SelectionLength = Len(txt.Text) - (Len(sTempString))
                    sComboText = sComboText & Mid$(sTempString, Len(sComboText) + 1)
                    Exit For
                Else
                    If InStr(UCase(sTempString), UCase(sComboText)) Then
                        sComboText = sComboText & Mid$(sTempString, Len(sComboText) _
                        + 1)
                        txt.Text = sComboText
                        txt.SelectionStart = Len(txt.Text)
                    Else
                        sComboText = sTempString
                    End If
                End If
            Next iLoop
        End If
    End Sub

    Private Sub btnTransfer_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTransfer.GotFocus
        txtGrid.Visible = False
    End Sub

    Private Sub lstSearch_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lstSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            If lstSearch.SelectedItem Is Nothing Then Exit Sub
            With gridView
                Select Case .Columns(.CurrentCell.ColumnIndex).Name
                    Case "NEWITEM"
                        txtGrid.Text = lstSearch.SelectedItem.ToString
                        Dim NewItemid As Integer
                        NewItemid = objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & txtGrid.Text & "' ", "ITEMID", "0")
                        gridView.CurrentRow.Cells("NEWITEMID").Value = NewItemid
                        .CurrentCell.Value = txtGrid.Text
                        lstSearch.Visible = False
                        txtGrid.SelectAll()
                    Case "NEWCOUNTER"
                        txtGrid.Text = lstSearch.SelectedItem.ToString
                        .CurrentCell.Value = txtGrid.Text
                        Dim NewItemCtrid As Integer
                        NewItemCtrid = objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME='" & txtGrid.Text & "' ", "ITEMCTRID", "0")
                        gridView.CurrentRow.Cells("NEWCTRID").Value = NewItemCtrid
                        lstSearch.Visible = False
                        txtGrid.SelectAll()
                End Select
            End With
        End If

    End Sub

    Public Sub New()
        InitializeComponent()
        With dtSummary
            .Columns.Add("KEYNO", GetType(Integer))
            .Columns.Add("CATCODE", GetType(String))
            .Columns.Add("CATNAME", GetType(String))
            .Columns.Add("PCS", GetType(Integer))
            .Columns.Add("GRSWT", GetType(Decimal))
            .Columns.Add("NETWT", GetType(Decimal))
            .Columns.Add("STNPCS", GetType(Decimal))
            .Columns.Add("STNWT", GetType(Decimal))
            .Columns.Add("DIAPCS", GetType(Decimal))
            .Columns.Add("DIAWT", GetType(Decimal))
            .Columns.Add("PREPCS", GetType(Decimal))
            .Columns.Add("PREWT", GetType(Decimal))
            .Columns.Add("STNITEM", GetType(Integer))
            .Columns.Add("STNCATCODE", GetType(String))
            .Columns.Add("STNAMT", GetType(Decimal))
            .Columns.Add("AMOUNT", GetType(Decimal))
            .Columns.Add("TYPE", GetType(String))
        End With
        dtSummary.AcceptChanges()
    End Sub

    Private Sub DgTrfSummary_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Escape Then DgTrfSummary.Visible = False : gridView.Focus()
    End Sub

    Private Sub ChkAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkAll.CheckedChanged
        For i As Integer = 0 To DgvView.RowCount - 1
            If PSSR_CATPOST <> "" Then
                DgvView.CurrentCell = DgvView.Rows(i).Cells("CHECK")
                chkpssrcolumns()
            Else
                DgvView.Rows(i).Cells("CHECK").Value = ChkAll.Checked
            End If
        Next
        DgvView.Update()
        CalcGridViewTotalN()
    End Sub

    Private Sub chkpssrcolumns()
        Dim Sno As String = Nothing
        Dim BatNo As String = Nothing
        Dim dv As New DataView
        Dim bool As Boolean = ChkAll.Checked
        BatNo = DgvView.CurrentRow.Cells("BATCHNO").Value.ToString
        If PENDTRF_SNOBASE <> False Then
            Sno = DgvView.CurrentRow.Cells("SNO").Value.ToString
            dv = New DataView
            dv = CType(DgvView.DataSource, DataTable).Copy.DefaultView
            dv.RowFilter = "BATCHNO = '" & BatNo & "' AND SNO='" & Sno & "'"
        Else
            dv = New DataView
            dv = CType(DgvView.DataSource, DataTable).Copy.DefaultView
            dv.RowFilter = "BATCHNO = '" & BatNo & "'"
        End If
        Dim dt As New DataTable
        dt = dv.ToTable
        CheckState = True
        If PSSR_CATPOST.ToString <> "" And Not PSSR_CATPOSTING Is Nothing And bool Then
            If dt.Columns.Contains("PSSRCHECK") Then
                Dim _tempmsg As String = ""
                For cnt As Integer = 0 To dt.Rows.Count - 1
                    If dt.Rows(cnt).Item("PSSRCHECK").ToString <> "" Then
                        _tempmsg += dt.Rows(cnt).Item("PSSRCHECK").ToString & vbCrLf
                        bool = False
                    End If
                Next
            End If
        End If

        For cnt As Integer = 0 To dt.Rows.Count - 1
            If bool Then
                txtCheckedItems.Text = Val(txtCheckedItems.Text) + 1
                'DgvView.Rows(Val(dt.Rows(cnt).Item("KEYNO").ToString)).DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
            Else
                txtCheckedItems.Text = Val(txtCheckedItems.Text) - 1
                'DgvView.Rows(Val(dt.Rows(cnt).Item("KEYNO").ToString)).DefaultCellStyle.BackColor = Color.White
            End If
            DgvView.Rows(Val(dt.Rows(cnt).Item("KEYNO").ToString)).Cells("CHECK").Value = bool
        Next
        'If DgvView.Columns(e.ColumnIndex).Name.ToUpper = "CHECK" Then SendKeys.Send("{TAB}")
        CalcGridViewTotalN()
        DgvView.Refresh()
    End Sub

    Private Sub DgvView_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DgvView.CellValueChanged, DgvView.CellClick
        If ChkAll.Focused = True Then Exit Sub
        If CheckState = False Then Exit Sub
        Dim Sno As String = Nothing
        Dim BatNo As String = Nothing
        Dim dv As New DataView
        Dim bool As Boolean = CType(IIf(IsDBNull(DgvView.CurrentRow.Cells("CHECK").Value), False, DgvView.CurrentRow.Cells("CHECK").Value), Boolean)
        BatNo = DgvView.CurrentRow.Cells("BATCHNO").Value.ToString
        If PENDTRF_SNOBASE <> False Then
            Sno = DgvView.CurrentRow.Cells("SNO").Value.ToString
            dv = New DataView
            dv = CType(DgvView.DataSource, DataTable).Copy.DefaultView
            dv.RowFilter = "BATCHNO = '" & BatNo & "' AND SNO='" & Sno & "'"
        Else
            dv = New DataView
            dv = CType(DgvView.DataSource, DataTable).Copy.DefaultView
            dv.RowFilter = "BATCHNO = '" & BatNo & "'"
        End If
        Dim dt As New DataTable
        dt = dv.ToTable
        CheckState = True
        If PSSR_CATPOST.ToString <> "" And Not PSSR_CATPOSTING Is Nothing And bool Then
            If dt.Columns.Contains("PSSRCHECK") Then
                Dim _tempmsg As String = ""
                For cnt As Integer = 0 To dt.Rows.Count - 1
                    If dt.Rows(cnt).Item("PSSRCHECK").ToString <> "" Then
                        _tempmsg += dt.Rows(cnt).Item("PSSRCHECK").ToString & vbCrLf
                        bool = False
                    End If
                Next
                If _tempmsg.ToString <> "" Then
                    MsgBox(_tempmsg.ToString, MsgBoxStyle.Information)
                End If
            End If
        End If

        For cnt As Integer = 0 To dt.Rows.Count - 1
            If bool Then
                txtCheckedItems.Text = Val(txtCheckedItems.Text) + 1
                'DgvView.Rows(Val(dt.Rows(cnt).Item("KEYNO").ToString)).DefaultCellStyle.BackColor = Color.LightGoldenrodYellow
            Else
                txtCheckedItems.Text = Val(txtCheckedItems.Text) - 1
                'DgvView.Rows(Val(dt.Rows(cnt).Item("KEYNO").ToString)).DefaultCellStyle.BackColor = Color.White
            End If
            DgvView.Rows(Val(dt.Rows(cnt).Item("KEYNO").ToString)).Cells("CHECK").Value = bool
        Next
        'If DgvView.Columns(e.ColumnIndex).Name.ToUpper = "CHECK" Then SendKeys.Send("{TAB}")
        CalcGridViewTotalN()
        DgvView.Refresh()
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New frmPend2Corp_Properties
        obj.p_chkMisc = ChkWithMisc.Checked
        obj.p_chkPur = Chkpurchase.Checked
        obj.p_chkreturn = ChkSr.Checked
        obj.p_chkSales = ChKSAL.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmPend2Corp_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmPend2Corp_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmPend2Corp_Properties))
        ChkWithMisc.Checked = obj.p_chkMisc
        ChkSr.Checked = obj.p_chkreturn
        Chkpurchase.Checked = obj.p_chkPur
        ChKSAL.Checked = obj.p_chkSales
    End Sub

    Private Sub gridViewtotal_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridViewtotal.CellFormatting
        e.CellStyle.SelectionBackColor = e.CellStyle.BackColor
        e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpFrom.LostFocus
        Dim TrsDate As Date = dtpFrom.Value
        If LOCK_MRMI_TRSDATE = False Then
            lblTrsDate.Text = "TransferDate:" & Format(TrsDate, "dd-MM-yyyy")
        Else
            If dtpFrom.Value > GetEntryDate(TrsDate) Then
                MsgBox("Date Should not Exceed Entry date", MsgBoxStyle.Information)
                dtpFrom.Focus()
            End If
        End If
    End Sub

    Private Sub DgvView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DgvView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        ElseIf e.KeyCode = Keys.Escape Then
            btnTransfer.Focus()

        ElseIf e.KeyCode = Keys.Space Then
            DgvView.CurrentRow.Cells("CHECK").Value = Not DgvView.CurrentRow.Cells("CHECK").Value
        End If
    End Sub

    Private Sub DgvView_CurrentCellDirtyStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DgvView.CurrentCellDirtyStateChanged
        CheckState = True
        DgvView.CommitEdit(DataGridViewDataErrorContexts.Commit)
    End Sub

    Private Sub dtpTo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpTo.LostFocus
        Dim TrsDate As Date = dtpTo.Value
        If LOCK_MRMI_TRSDATE = False Then
            lblTrsDate.Text = "TransferDate:" & Format(TrsDate, "dd-MM-yyyy")
        Else
            If dtpTo.Value > GetEntryDate(TrsDate) Then
                MsgBox("Date Should not Exceed Entry date", MsgBoxStyle.Information)
                dtpTo.Focus()
            End If
        End If
    End Sub

    Private Sub CmbMetal_MAN_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbMetal_MAN.SelectedIndexChanged
        If CmbMetal_MAN.Text.ToString <> "ALL" And CmbMetal_MAN.Text.ToString <> "" Then
            'strSql = "SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME='" & CmbMetal_MAN.Text.ToString & "') ORDER BY CATNAME"
            'objGPack.FillCombo(strSql, CmbMetal_MAN, True)
            strSql = " SELECT 'ALL' CATNAME,'ALL' CATCODE,1 RESULT"
            strSql += " UNION ALL"
            strSql += " SELECT CATNAME,CATCODE,2 RESULT FROM " & cnAdminDb & "..CATEGORY WHERE METALID in (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN  ('" & CmbMetal_MAN.Text.ToString & "'))"
            strSql += " ORDER BY RESULT,CATNAME"
            dtCategory = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCategory)
            BrighttechPack.GlobalMethods.FillCombo(chkCmbCategory, dtCategory, "CATNAME", , "ALL")
        Else
            'strSql = "SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY ORDER BY CATNAME"
            'objGPack.FillCombo(strSql, CmbMetal_MAN, True)
            strSql = " SELECT 'ALL' CATNAME,'ALL' CATCODE,1 RESULT"
            strSql += " UNION ALL"
            strSql += " SELECT CATNAME,CATCODE,2 RESULT FROM " & cnAdminDb & "..CATEGORY "
            strSql += " ORDER BY RESULT,CATNAME"
            dtCategory = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCategory)
            BrighttechPack.GlobalMethods.FillCombo(chkCmbCategory, dtCategory, "CATNAME", , "ALL")
        End If
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click


        TITLE = " Pending Transfer Report From " & dtpFrom.Value.ToString("dd-MM-yyyy") & " To "
        TITLE += dtpTo.Value.ToString("dd-MM-yyyy") & " Branch " & cmbCostCentre_MAN.Text & " To " & COST_NAME
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If dgvexcel1.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, TITLE, dgvexcel1, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub cmbCostCentre_MAN_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cmbCostCentre_MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            StateId = objGPack.GetSqlValue("SELECT STATEID FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE IN(" & "SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre_MAN.Text & "'" & ")")
            If StateId = 0 Then
                MsgBox("Please Update State for the Account [" & cmbCostCentre_MAN.Text & "]", MsgBoxStyle.Information)
                Exit Sub
            End If
        End If
    End Sub
End Class
Public Class frmPend2Corp_Properties
    Private chkMisc As Boolean = True
    Private chkReturn As Boolean = True
    Private chkPur As Boolean = True
    Private chkSales As Boolean = True
    Public Property p_chkMisc() As Boolean
        Get
            Return chkMisc
        End Get
        Set(ByVal value As Boolean)
            chkMisc = value
        End Set
    End Property
    Public Property p_chkPur() As Boolean
        Get
            Return chkPur
        End Get
        Set(ByVal value As Boolean)
            chkPur = value
        End Set
    End Property
    Public Property p_chkreturn() As Boolean
        Get
            Return chkReturn
        End Get
        Set(ByVal value As Boolean)
            chkReturn = value
        End Set
    End Property
    Public Property p_chkSales() As Boolean
        Get
            Return chkSales
        End Get
        Set(ByVal value As Boolean)
            chkSales = value
        End Set
    End Property
End Class
