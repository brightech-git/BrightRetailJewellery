Imports System.Data.OleDb
Public Class frmTagTransfer
    Private _IsSelectAllChecked As Boolean
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim dtGridView As DataTable
    Dim dtCatMast As DataTable
    Dim defalutDestination As String
    Dim ReplSubItemid As Boolean = IIf(GetAdmindbSoftValue("STKTRAN_NEW_SUBID", "N") = "Y", True, False)
    'Dim ChkTagTran As Boolean = IIf(GetAdmindbSoftValue("CHKTAGTRAN", "N") = "Y", True, False)
    Dim ChkTagTran As Boolean = True
    Dim ReplRecdate As Boolean = IIf(GetAdmindbSoftValue("STKTRAN_REC_DATE", "N") = "Y", True, False)
    Dim PRODTAGSEP As String = GetAdmindbSoftValue("PRODTAGSEP")
    Dim BARCODE2DSEP As String = GetAdmindbSoftValue("BARCODE2DSEP")
    Dim AUTOBOOKTRAN As Boolean = IIf(GetAdmindbSoftValue("AUTOBOOKTRAN", "N") = "Y", True, False)
    Dim IS_IMAGE_TRF As Boolean = IIf(GetAdmindbSoftValue("STKTRANWITHIMAGE", "N") = "Y", True, False)
    Dim Isbulkupdate As Boolean = IIf(GetAdmindbSoftValue("BULKTAGTRANSFER", "N") = "Y", True, False)
    Dim NonTag_trf_Lotno As Boolean = IIf(GetAdmindbSoftValue("NONTAG_TRF_LOTNO ", "N") = "Y", True, False)
    Dim AUTOBOOKVALUE As String = GetAdmindbSoftValue("AUTOBOOKVALUE", "N,0,0,0,0,0")
    Dim AUTOINTERNAL_VOUCHER As String = GetAdmindbSoftValue("AUTOBOOK_VOUCHER", "N")
    Dim IsSearchKeyRestrict As Boolean = IIf(GetAdmindbSoftValue("RESTRICT_SEARCHKEY", "N") = "Y", True, False)
    Dim STKTRAN_MULTIMETAL As Boolean = IIf(GetAdmindbSoftValue("STKTRAN_MULTIMETAL", "Y") = "Y", True, False)
    Dim CENTR_DB_GLB As Boolean = IIf(GetAdmindbSoftValue("CENTR_DB_ALLCOSTID", "N") = "Y", True, False)
    Dim INTRBRANCHTRF_DISABLE As Boolean = IIf(GetAdmindbSoftValue("RESTRICT_INTRBRTRF", "N") = "Y", True, False)
    Dim TagTranCorpOnly As Boolean = IIf(GetAdmindbSoftValue("TAGTRANCORPONLY", "N") = "Y", True, False)
    Dim AUTOBOOKVALUEARRY() As String = Split(AUTOBOOKVALUE, ",")
    Dim AUTOBOOKVALUEENABLE As String
    Dim AUTOBOOKVALUEG_PER As Decimal
    Dim AUTOBOOKVALUES_PER As Decimal
    Dim AUTOBOOKVALUEP_PER As Decimal
    Dim AUTOBOOKVALUED_PER As Decimal
    Dim AUTOBOOKVALUET_PER As Decimal
    Dim TransistNo As Integer
    Dim objMultiSelect As MultiSelectRowDia = Nothing
    Dim XCnAdmin As OleDbConnection = Nothing
    Public Xtran As OleDbTransaction = Nothing
    Private XSyncdb As String = Replace(cnAdminDb, "ADMINDB", "UTILDB")

    Dim BillDate As Date
    Dim TranNo As Integer
    Dim Batchno As String
    Public objSoftKeys As New SoftKeys
    Dim GEN_SKUFILE As Boolean = IIf(GetAdmindbSoftValue("GEN_SKUFILE", "N") = "Y", True, False)
    Dim SKUFILEPATH As String = GetAdmindbSoftValue("SKUFILEPATH", "")
    Dim HeaderCheckBox As CheckBox = Nothing
    Dim NonTag_trf_Sno As Boolean = IIf(GetAdmindbSoftValue("NONTAG_TRF_SNO ", "N") = "Y", True, False)
    Dim REQ_FRANCHISEE As Boolean = IIf(GetAdmindbSoftValue("REQ_FRANCHISEE", "N") = "Y", True, False)
    Dim FRANCHISEE_VALUE As String = GetAdmindbSoftValue("FRANCHISEE_VALUE", "N,0,0,0,0,0,0,0,0,0")
    Dim FRANCHISEE_VALUEARY() As String = Split(FRANCHISEE_VALUE, ",")
    Dim TagTranTotal As Boolean = IIf(GetAdmindbSoftValue("TAGTRANTOTAL", "Y") = "Y", True, False)
    Dim _isFrachisee As Boolean = False
    Dim ObjMaxMinValue As TagMaxMinValues
    Dim IsRemarkRestrict As Boolean = IIf(GetAdmindbSoftValue("RESTRICT_REMARK", "Y") = "Y", True, False)
    Dim IsFilterRestrict As Boolean = IIf(GetAdmindbSoftValue("RESTRICT_FILTERATION", "N") = "Y", True, False)
    Dim dtExemption As New DataTable
    Dim OTP_Password As String
    Dim STKTRAN_PRINTNEW As Boolean = IIf(GetAdmindbSoftValue("STKTRAN_PRINTNEW", "N") = "Y", True, False)
    Dim SPECIFICFORMAT As String = GetAdmindbSoftValue("SPECIFICFORMAT", "")
    'GST
    Dim SGST As Double = Nothing
    Dim CGST As Double = Nothing
    Dim IGST As Double = Nothing
    Dim gstAmt As Double = Nothing
    Dim fgstAmt As Double = Nothing
    Dim StateId As Integer
    Dim SGSTPER As Decimal
    Dim CGSTPER As Decimal
    Dim IGSTPER As Decimal
    Dim GstRecCode As String = GetAdmindbSoftValue("GSTACCODE_INTTRF", "")
    Dim GstRecAcc() As String
    Dim SCode As String
    Dim CCode As String
    Dim ICode As String
    Dim NeedItemType_accpost As Boolean = IIf(GetAdmindbSoftValue("POS_SEPACCPOST_ITEMTYPE", "N") = "Y", True, False)
    Dim STKTRAN_ContraPost As Boolean = IIf(GetAdmindbSoftValue("STKTRAN_ACCOUNT_POST", "N") = "Y", True, False)
    Dim MetalBasedStone As Boolean = IIf(GetAdmindbSoftValue("MULTIMETALBASEDSTONE", "N") = "Y", True, False)
    Dim STKTRAN_REPAIR_SEPPOST As Boolean = IIf(GetAdmindbSoftValue("STKTRAN_REPAIR_SEPPOST", "N") = "Y", True, False)
    Dim OR_REP_NewCatCode As String = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & GetAdmindbSoftValue("REPAIR_NEW_CAT", "00018") & "'", , "00018")
    Dim OR_REP_NewCatCode_S As String = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & GetAdmindbSoftValue("REPAIR_NEW_CAT_S", "00014") & "'", , "00014")
    Dim OR_REP_NewCatCode_P As String = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & GetAdmindbSoftValue("REPAIR_NEW_CAT_P", "") & "'", , "")
    Dim STKTRAN_StoreAmt As Boolean = IIf(GetAdmindbSoftValue("STKTRAN_STORE_AMOUNT", "N") = "Y", True, False)
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        With dtExemption
            .Columns.Add("OPTIONID", GetType(Integer))
            .Columns.Add("EXEMPUSER", GetType(Integer))
            .Columns.Add("EXEMPOPEN", GetType(String))
            .Columns.Add("EXEMPDATE", GetType(String))
            .Columns.Add("EXEMPTIME", GetType(String))
            .Columns.Add("DESCRIPTION", GetType(String))
            Dim col As New DataColumn("KEYNO")
            col.AutoIncrement = True
            col.AutoIncrementSeed = 1
            col.AutoIncrementStep = 1
            .Columns.Add(col)
        End With
        AddHandler gridView.CellValueChanged, AddressOf gridView_CellChecked
    End Sub

    Private Sub frmTagTransfer_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtTagNo.Focused Then Exit Sub
            If txtItemId.Focused Then Exit Sub
            'If txtNetWt_WET.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub GridStyle()

    End Sub

    Private Sub frmTagTransfer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If ChkTagTran Then
            chkSelect.Visible = True
        Else
            chkSelect.Visible = False
        End If
        If XSyncdb <> "" Then

            If UCase(ConInfo.lDbLoginType = "W") Then
                XCnAdmin = New OleDbConnection("Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=" & XSyncdb & ";Data Source=" & ConInfo.lServerName & "")
            Else
                Dim passWord As String = ConInfo.lDbPwd
                If passWord <> "" Then passWord = BrighttechPack.Methods.Decrypt(passWord)
                XCnAdmin = New OleDbConnection("PROVIDER = SQLOLEDB.1;Persist Security Info=False; Initial Catalog=" & XSyncdb & ";Data Source=" & ConInfo.lServerName & ";user id=" & IIf(ConInfo.lDbUserId <> "", ConInfo.lDbUserId, "sa") & ";password=" & passWord & ";")
            End If
            'XCnAdmin = cn
            XCnAdmin.Open()
        Else
            XCnAdmin = cn
        End If



        If AUTOBOOKTRAN = True Then
            If AUTOBOOKVALUEARRY.Length < 6 Then MsgBox("Please Reset the value <AUTOBOOKVALUE> ex(N,0,0,0,0,0,0)") : Me.Close()
            AUTOBOOKVALUEENABLE = AUTOBOOKVALUEARRY(0).ToString
            AUTOBOOKVALUEG_PER = Val(AUTOBOOKVALUEARRY(1).ToString)
            AUTOBOOKVALUES_PER = Val(AUTOBOOKVALUEARRY(2).ToString)
            AUTOBOOKVALUEP_PER = Val(AUTOBOOKVALUEARRY(3).ToString)
            AUTOBOOKVALUED_PER = Val(AUTOBOOKVALUEARRY(4).ToString)
            AUTOBOOKVALUET_PER = Val(AUTOBOOKVALUEARRY(5).ToString)
        Else
            lblAcname.Enabled = False
            CmbAcname.Enabled = False
        End If
        Dim dtMetal As New DataTable
        Dim dtDesigner As New DataTable
        Dim dtCounter As New DataTable
        defalutDestination = GetAdmindbSoftValue("PICPATH")
        If Not defalutDestination.EndsWith("\") And defalutDestination <> "" Then defalutDestination += "\"

        dtGridView = New DataTable
        dtGridView.Columns.Add("SNO", GetType(String))
        dtGridView.Columns.Add("COUNTER", GetType(String))
        dtGridView.Columns.Add("METALID", GetType(String))
        dtGridView.Columns.Add("ITEM", GetType(String))
        dtGridView.Columns.Add("SUBITEM", GetType(String))
        dtGridView.Columns.Add("TAGNO", GetType(String))
        dtGridView.Columns.Add("PCS", GetType(Integer))
        dtGridView.Columns.Add("GRSWT", GetType(Decimal))
        dtGridView.Columns.Add("LESSWT", GetType(Decimal))
        dtGridView.Columns.Add("NETWT", GetType(Decimal))

        dtGridView.Columns.Add("MAXMCGRM", GetType(Decimal))
        dtGridView.Columns.Add("MAXMC", GetType(Decimal))
        dtGridView.Columns.Add("MAXWASTPER", GetType(Decimal))
        dtGridView.Columns.Add("MAXWAST", GetType(Decimal))

        dtGridView.Columns.Add("STNPCS", GetType(Integer))
        dtGridView.Columns.Add("STNWT", GetType(Decimal))
        dtGridView.Columns.Add("PSTNPCS", GetType(Integer))
        dtGridView.Columns.Add("PSTNWT", GetType(Decimal))
        dtGridView.Columns.Add("DIAPCS", GetType(Integer))
        dtGridView.Columns.Add("DIAWT", GetType(Decimal))
        dtGridView.Columns.Add("SALVALUE", GetType(Double))
        dtGridView.Columns.Add("RATE", GetType(Double))
        dtGridView.Columns.Add("COLHEAD", GetType(String))
        dtGridView.Columns.Add("PCTFILE", GetType(String))
        dtGridView.Columns.Add("STOCKMODE", GetType(String))
        dtGridView.Columns.Add("SALEMODE", GetType(String))
        dtGridView.Columns.Add("STKTYPE", GetType(String))
        dtGridView.Columns.Add("DESIGNERID", GetType(String))
        'GST
        dtGridView.Columns.Add("SGST", GetType(Double))
        dtGridView.Columns.Add("CGST", GetType(Double))
        dtGridView.Columns.Add("IGST", GetType(Double))
        dtGridView.AcceptChanges()
        gridView.DataSource = dtGridView
        With gridView
            For cnt As Integer = 0 To .ColumnCount - 1
                .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("LESSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SALVALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").DefaultCellStyle.Format = "0.000"
            .Columns("LESSWT").DefaultCellStyle.Format = "0.000"
            .Columns("NETWT").DefaultCellStyle.Format = "0.000"
            .Columns("STNWT").DefaultCellStyle.Format = "0.000"
            .Columns("PSTNWT").DefaultCellStyle.Format = "0.000"
            .Columns("DIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            .Columns("SALVALUE").DefaultCellStyle.Format = "0.00"
            .Columns("COLHEAD").Visible = False
            .Columns("SNO").Visible = False
            .Columns("STOCKMODE").Visible = False
            .Columns("DESIGNERID").Visible = False
            .Columns("METALID").Visible = False
            .Columns("RATE").Visible = False
            .Columns("PCTFILE").Visible = False
            gridView.Sort(gridView.Columns("COLHEAD"), System.ComponentModel.ListSortDirection.Ascending)
        End With
        cmbCostCentre_MAN.Items.Clear()
        Dim MainCostId As String = GetAdmindbSoftValue("SYNC-TO", "")
        strSql = "SELECT MAIN FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE COSTID = '" & cnCostId & "'"
        Dim ThisCO As Boolean = IIf(objGPack.GetSqlValue(strSql).ToString = "Y", True, False)


        strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID <> '" & cnCostId & "'"
        If TagTranCorpOnly = True And ThisCO = False Then
            If strBCostid Is Nothing Then strSql += " AND COSTID IN(SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE ISNULL(ACTIVE,'')<>'N')"
        Else
            If strBCostid Is Nothing Then strSql += " AND COSTID IN(SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE ISNULL(ACTIVE,'')<>'N')"
        End If
        If SPECIFICFORMAT.ToString = "1" Then
            strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COMPANYID LIKE '%" & strCompanyId & "%')"
        End If

        If INTRBRANCHTRF_DISABLE Then
            If Not ThisCO And MainCostId <> "" Then strSql += " AND COSTID ='" & MainCostId & "'"
        End If
        strSql += " ORDER BY COSTNAME"
        objGPack.FillCombo(strSql, cmbCostCentre_MAN)

        '' LOAD METAL
        strSql = vbCrLf + " SELECT 'ALL' METALNAME,'ALL' METALID,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT METALNAME,CONVERT(VARCHAR,METALID),2 RESULT FROM " & cnAdminDb & "..METALMAST"
        strSql += vbCrLf + " ORDER BY RESULT,METALNAME"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtMetal)
        cmbMetal.Items.Clear()
        BrighttechPack.GlobalMethods.FillCombo(cmbMetal, dtMetal, "METALNAME", , "ALL")

        strSql = vbCrLf + " SELECT 'ALL' DESIGNERNAME,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT DESIGNERNAME,2 RESULT "
        strSql += vbCrLf + " FROM " & cnAdminDb & "..DESIGNER"
        strSql += vbCrLf + " WHERE ISNULL(ACTIVE,'Y')<>'N'"
        strSql += vbCrLf + " ORDER BY RESULT,DESIGNERNAME"
        objGPack.FillCombo(strSql, CmbDesigner)

        strSql = vbCrLf + " SELECT 'ALL' COUNTERNAME,'ALL' COUNTERID,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT ITEMCTRNAME as COUNTERNAME,CONVERT(VARCHAR,ITEMCTRID),2 RESULT FROM " & cnAdminDb & "..ITEMCOUNTER ORDER BY RESULT,COUNTERNAME"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCounter)
        cmbCounter_OWN.Items.Clear()
        BrighttechPack.GlobalMethods.FillCombo(cmbCounter_OWN, dtCounter, "COUNTERNAME", , "ALL")
        If ReplSubItemid = False Then
            cmbSubitem.Visible = False
            lblSubitemid.Visible = False
        End If

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

        strSql = " SELECT * FROM " & cnAdminDb & "..CATEGORY "
        Dim daCat As New OleDbDataAdapter()
        daCat = New OleDbDataAdapter(strSql, cn)
        dtCatMast = New DataTable
        daCat.Fill(dtCatMast)

        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        objGPack.TextClear(Me)
        dtGridView.Rows.Clear()
        cmbCounter_OWN.Text = ""
        strSql = vbCrLf + " SELECT NAME FROM " & cnAdminDb & "..SYSCOLUMNS WHERE ID IN (SELECT ID FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE='U' AND NAME='ITEMTAG')"
        strSql += vbCrLf + " ORDER BY NAME"
        objGPack.FillCombo(strSql, cmbSearchKey, True, False)
        cmbSearchKey.Text = ""
        chkCheckByScan.Checked = False
        chkRecDate.Checked = True
        chkRecDate.Checked = False
        dtpDate.Value = GetEntryDate(GetServerDate)
        dtGridView.Rows.Clear()
        dtGridView.Clear()
        cmbCostCentre_MAN.Enabled = True
        AddGrandTotal()
        cmbCostCentre_MAN.Focus()
        'If gridView.Columns.Contains("CHECKBOXCOLUMN") Then
        '    gridView.Columns.Remove("CHECKBOXCOLUMN")
        'End If
        gridView.DataSource = Nothing
        LoadTransactionType()
        CmbStockType.Text = "ALL"
        Prop_Gets()
        If chkSelect.Checked = True Then
            chkSelect.Checked = False
        End If
        'chkSelect.Visible = False
        _isFrachisee = False
        ObjMaxMinValue = New TagMaxMinValues
        dtExemption.Rows.Clear()
        dtExemption.Clear()
    End Sub

    Private Function GSTCALC(ByVal grsamt As Double, ByVal catcode As String)
        ' speed access if and endif only
        If grsamt = 0 Then Exit Function
        grsamt = Math.Round(grsamt, 2)
        Dim dr() As DataRow = dtCatMast.Select("CATCODE='" & catcode & "'", Nothing)
        If dr.Length > 0 Then
            SGSTPER = Val(dr(0).Item("S_SGSTTAX").ToString)
            CGSTPER = Val(dr(0).Item("S_CGSTTAX").ToString)
            IGSTPER = Val(dr(0).Item("S_IGSTTAX").ToString)
        End If
        SGST = 0 : CGST = 0 : IGST = 0
        If SGSTPER > 0 Or CGSTPER > 0 Or IGSTPER > 0 Then
            gstAmt = grsamt
            If StateId = CompanyStateId Then
                'SGST = Math.Round(Val(gstAmt) * SGSTPER / 100, 2)
                'SGST = CalcRoundoffAmt(SGST, objSoftKeys.RoundOff_Vat)
                'CGST = Math.Round(Val(gstAmt) * CGSTPER / 100, 2)
                'CGST = CalcRoundoffAmt(CGST, objSoftKeys.RoundOff_Vat)
            Else
                IGST = Math.Round(Val(gstAmt) * IGSTPER / 100, 2)
                IGST = CalcRoundoffAmt(IGST, objSoftKeys.RoundOff_Vat)
            End If
            fgstAmt = Val(gstAmt)
        Else
            grsamt = grsamt
            SGST = 0
            CGST = 0
            IGST = 0
            fgstAmt = Val(gstAmt)
        End If
    End Function


    Private Sub LoadTransactionType()
        CmbStockType.Items.Clear()
        Dim ExistingTranType As New List(Of String)
        Dim Row() As DataRow = dtGridView.Select("STKTYPE <> ''")
        For Each Ro As DataRow In Row
            ExistingTranType.Add(Ro.Item("STKTYPE").ToString)
        Next
        If ExistingTranType.Count > 0 Then
            CmbStockType.Items.Add(ExistingTranType(0))
            CmbStockType.SelectedIndex = 0
        Else
            CmbStockType.Items.Add("ALL")
            CmbStockType.Items.Add("TRADING")
            CmbStockType.Items.Add("EXEMPTED")
            CmbStockType.Items.Add("MANUFACTURING")
            CmbStockType.SelectedIndex = 0
        End If
    End Sub
    Private Sub AddGrandTotal()
        If TagTranTotal = False Then Exit Sub
        Dim Ro As DataRow = Nothing
        Dim pcs As Integer = Val(dtGridView.Compute("SUM(PCS)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)
        Dim grsWt As Decimal = Val(dtGridView.Compute("SUM(GRSWT)", "COLHEAD <> 'G' or COLHEAD IS NULL or COLHEAD IS NULL").ToString)
        Dim lessWT As Decimal = Val(dtGridView.Compute("SUM(LESSWT)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)
        Dim netWt As Decimal = Val(dtGridView.Compute("SUM(NETWT)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)

        Dim MAXMCGRM As Decimal = Val(dtGridView.Compute("SUM(MAXMCGRM)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)
        Dim MAXMC As Decimal = Val(dtGridView.Compute("SUM(MAXMC)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)
        Dim MAXWASTPER As Decimal = Val(dtGridView.Compute("SUM(MAXWASTPER)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)
        Dim MAXWAST As Decimal = Val(dtGridView.Compute("SUM(MAXWAST)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)


        Dim stnpcs As Decimal = Val(dtGridView.Compute("SUM(STNPCS)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)
        Dim stnWt As Decimal = Val(dtGridView.Compute("SUM(STNWT)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)
        Dim Pstnpcs As Decimal = Val(dtGridView.Compute("SUM(PSTNPCS)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)
        Dim PstnWt As Decimal = Val(dtGridView.Compute("SUM(PSTNWT)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)

        Dim diaPcs As Decimal = Val(dtGridView.Compute("SUM(DIAPCS)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)
        Dim diaWt As Decimal = Val(dtGridView.Compute("SUM(DIAWT)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)
        Dim salValue As Decimal = Val(dtGridView.Compute("SUM(SALVALUE)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)
        Dim NoofTag As Integer = Val(dtGridView.Compute("COUNT(PCS)", "COLHEAD <> 'G' or COLHEAD IS NULL").ToString)

        If dtGridView.Rows.Count > 0 Then
            Ro = dtGridView.Select("COLHEAD = 'G'")(0)
        Else
            Ro = dtGridView.NewRow
        End If
        'Ro = dtGridView.NewRow
        Ro.Item("ITEM") = "GRAND TOTAL"
        Ro.Item("PCS") = IIf(pcs <> 0, pcs, DBNull.Value)
        Ro.Item("TAGNO") = IIf(NoofTag <> 0, NoofTag, DBNull.Value)
        Ro.Item("GRSWT") = IIf(grsWt <> 0, Format(grsWt, "0.000"), DBNull.Value)

        Ro.Item("MAXMCGRM") = IIf(MAXMCGRM <> 0, Format(MAXMCGRM, "0.000"), DBNull.Value)
        Ro.Item("MAXMC") = IIf(MAXMC <> 0, Format(MAXMC, "0.000"), DBNull.Value)
        Ro.Item("MAXWASTPER") = IIf(MAXWASTPER <> 0, Format(MAXWASTPER, "0.000"), DBNull.Value)
        Ro.Item("MAXWAST") = IIf(MAXWAST <> 0, Format(MAXWAST, "0.000"), DBNull.Value)


        Ro.Item("STNPCS") = IIf(stnpcs <> 0, stnpcs, DBNull.Value)
        Ro.Item("STNWT") = IIf(stnWt <> 0, Format(stnWt, "0.000"), DBNull.Value)
        Ro.Item("PSTNPCS") = IIf(stnpcs <> 0, Pstnpcs, DBNull.Value)
        Ro.Item("PSTNWT") = IIf(stnWt <> 0, Format(PstnWt, "0.000"), DBNull.Value)
        Ro.Item("DIAPCS") = IIf(diaPcs <> 0, diaPcs, DBNull.Value)
        Ro.Item("DIAWT") = IIf(diaWt <> 0, Format(diaWt, "0.000"), DBNull.Value)
        Ro.Item("LESSWT") = IIf(lessWT <> 0, Format(lessWT, "0.000"), DBNull.Value)
        Ro.Item("NETWT") = IIf(netWt <> 0, Format(netWt, "0.000"), DBNull.Value)
        Ro.Item("SALVALUE") = IIf(salValue <> 0, Format(salValue, "0.00"), DBNull.Value)
        Ro.Item("COLHEAD") = "G"
        If dtGridView.Rows.Count > 0 Then

        Else
            dtGridView.Rows.Add(Ro)
        End If
        dtGridView.AcceptChanges()
        gridView.DataSource = dtGridView
        If gridView.Rows.Count > 0 Then gridView.Rows(gridView.RowCount - 1).DefaultCellStyle = reportTotalStyle
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub txtTagNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTagNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Not chkCheckByScan.Checked Then
                If ReplSubItemid Then cmbSubitem.Focus() : Exit Sub
                SendKeys.Send("{TAB}")
                Exit Sub
            End If
            If txtTagNo.Text = "" Then Exit Sub
            If chkCheckByScan.Checked Then
                btnSearch_Click(Me, New EventArgs)
            End If

            '    If dtpDate.Enabled = False Then
            '        MsgBox("Please select tag receipt date", MsgBoxStyle.Information)
            '        Exit Sub
            '    End If
            '    If MessageBox.Show("Do you want to load all tag items?", "Tag Transfer", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
            '        Exit Sub
            '    End If

        End If
    End Sub

    Private Sub chkRecDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkRecDate.CheckedChanged
        dtpDate.Enabled = chkRecDate.Checked
    End Sub

    Private Sub gridView_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridView.UserDeletedRow
        dtGridView.AcceptChanges()
        AddGrandTotal()
    End Sub

    Private Sub CreateInsertGenrator()
        strSql = " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'InsertGenerator')>0 DROP PROC InsertGenerator"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = "  create PROC InsertGenerator    "
        strSql += "  (@tableName varchar(100)) as    "
        strSql += "  DECLARE @query nvarchar(4000) -- provide for the whole query,     "
        strSql += "                                -- you may increase the size    "
        strSql += "  DECLARE cursCol CURSOR FAST_FORWARD FOR    "
        strSql += "  select b.name, c.name    "
        strSql += "  from sysobjects a, syscolumns b, systypes c    "
        strSql += "  where a.id = b.id and b.xtype = c.xtype and    "
        strSql += "    a.name = @tableName    "
        strSql += "       "
        strSql += "  OPEN cursCol    "
        strSql += "  DECLARE @string nvarchar(3000) --for storing the first half     "
        strSql += "                                 --of INSERT statement    "
        strSql += "  DECLARE @stringData nvarchar(3000) --for storing the data     "
        strSql += "                                     --(VALUES) related statement    "
        strSql += "  DECLARE @dataType nvarchar(1000) --data types returned     "
        strSql += "                                   --for respective columns    "
        strSql += "  SET @string='INSERT '+@tableName+'('    "
        strSql += "  SET @stringData=''    "
        strSql += "       "
        strSql += "  DECLARE @colName nvarchar(50)    "
        strSql += "       "
        strSql += "  FETCH NEXT FROM cursCol INTO @colName,@dataType    "
        strSql += "       "
        strSql += "  IF @@fetch_status<>0    "
        strSql += "      begin    "
        strSql += "      print 'Table '+@tableName+' not found, processing skipped.'    "
        strSql += "      close curscol    "
        strSql += "      deallocate curscol    "
        strSql += "      return    "
        strSql += "  END    "
        strSql += "  WHILE @@FETCH_STATUS=0    "
        strSql += "  BEGIN    "
        strSql += "  IF @dataType in ('varchar','char','nchar','nvarchar')    "
        strSql += "   BEGIN    "
        strSql += "       SET @stringData=@stringData+'''''''''+    "
        strSql += "               isnull('+@colName+','''')+'''''',''+'    "
        strSql += "   END    "
        strSql += "  ELSE    "
        strSql += "   if @dataType in ('text','ntext') --if the datatype  --is text or something else     "
        strSql += "    BEGIN    "
        strSql += "        SET @stringData=@stringData+'''''''''+    "
        strSql += "              isnull(cast('+@colName+' as varchar(2000)),'''')+'''''',''+'    "
        strSql += "    END    "
        strSql += "   ELSE    "
        strSql += "    IF @dataType = 'money' --because money doesn't get converted  --from varchar implicitly    "
        strSql += "     BEGIN    "
        strSql += "         SET @stringData=@stringData+'''convert(money,''''''+    "
        strSql += "             isnull(cast('+@colName+' as varchar(200)),''0.0000'')+''''''),''+'    "
        strSql += "     END    "
        strSql += "    ELSE     "
        strSql += "     IF @dataType='datetime'    "
        strSql += "      BEGIN    "
        strSql += "          SET @stringData=@stringData+'''convert(datetime,''''''+    "
        strSql += "              isnull(cast('+@colName+' as varchar(200)),''0'')+''''''),''+'    "
        strSql += "      END    "
        strSql += "     ELSE     "
        strSql += "      IF @dataType='image'     "
        strSql += "       BEGIN    "
        strSql += "           SET @stringData=@stringData+'''''''''+    "
        strSql += "              isnull(cast(convert(varbinary,'+@colName+')     "
        strSql += "               as varchar(6)),''0'')+'''''',''+'    "
        strSql += "       END    "
        strSql += "      ELSE --presuming the data type is int,bit,numeric,decimal     "
        strSql += "       BEGIN    "
        strSql += "           SET @stringData=@stringData+'''''''''+    "
        strSql += "                 isnull(cast('+@colName+' as varchar(200)),''0'')+'''''',''+'    "
        strSql += "       END    "
        strSql += "       "
        strSql += "  SET @string=@string+@colName+','    "
        strSql += "       "
        strSql += "  FETCH NEXT FROM cursCol INTO @colName,@dataType    "
        strSql += "  END     "
        strSql += "       "
        strSql += "  SET @query ='SELECT '''+substring(@string,0,len(@string)) + ')     "
        strSql += "      VALUES(''+ ' + substring(@stringData,0,len(@stringData)-2)+'''+'')''     "
        strSql += "      FROM '+@tableName    "
        strSql += "  exec sp_executesql @query --load and run the built query    "
        strSql += "  CLOSE cursCol    "
        strSql += "  DEALLOCATE cursCol    "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Sub


    Private Sub CreateInternalTransfernew(ByVal ToCostId As String, ByVal RefNo As String, ByVal RefDate As Date, ByVal Tranno As Long, ByVal Transistno As Integer, Optional ByVal Fromcostid As String = Nothing, Optional ByVal Tocostacid As String = "")
        ''alter in transfer voucher generation in utility also
        strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE"
        cmd = New OleDbCommand(strSql, XCnAdmin, tran)
        ': cmd.CommandTimeout = 1000 : 
        cmd.ExecuteNonQuery()

        Dim _contraaccode As String = ""
        If STKTRAN_ContraPost Then
            _contraaccode = GetSqlValue("SELECT ISNULL(ACCODE,'')ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE costid='" & cnCostId.ToString & "'", XCnAdmin, tran)
            If _contraaccode.ToString = "" Then
                _contraaccode = "STKTRAN"
            End If
        Else
            _contraaccode = "STKTRAN"
        End If

        strSql = vbCrLf + " SELECT AX.*" ',IDENTITY(INT,1,1)AS KEYNO"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS "
        strSql += vbCrLf + " FROM ("
        strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
        strSql += vbCrLf + " ,'IIN' AS TRANTYPE "
        strSql += vbCrLf + " ,SUM(T.PCS)PCS,SUM(T.GRSWT)GRSWT,SUM(T.NETWT)NETWT,SUM(T.LESSWT)LESSWT,CONVERT(NUMERIC(15,3),(SUM(T.NETWT)*T.PURITY)/100) AS PUREWT"
        strSql += vbCrLf + " ,0 ITEMID,0 SUBITEMID"
        strSql += vbCrLf + " ,CASE WHEN SALEMODE <> 'W' OR IM.METALID IN ('D','T') THEN SUM(SALVALUE) ELSE 0 END RATE"
        strSql += vbCrLf + " , SUM(TRFVALUE) AS AMOUNT"
        strSql += vbCrLf + " ,'" & cnCostId & "' COSTID,T.COMPANYID,T.PURITY" '',IM.CATCODE
        If NeedItemType_accpost Then
            strSql += vbCrLf + " ,CASE WHEN ISNULL(T.ITEMTYPEID,0)<>0 THEN CASE WHEN ISNULL((SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =T.ITEMTYPEID),'') <>''"
            strSql += vbCrLf + " THEN (SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =T.ITEMTYPEID) ELSE IM.CATCODE END ELSE  IM.CATCODE END CATCODE "
        Else
            strSql += vbCrLf + " ,IM.CATCODE"
        End If

        strSql += vbCrLf + " ,'" & Tocostacid & "' AS ACCODE"
        strSql += vbCrLf + " ,IM.METALID,T.STKTYPE"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
        strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
        strSql += vbCrLf + " WHERE IM.BOOKSTOCK = 'W' AND T.SNO IN (select TAGSNO FROM TRANS_STATUS WHERE STOCKMODE = 'T' AND STATUS ='U' AND MOVETYPE = 'O')"
        If MetalBasedStone Then '' added for vbj on 11-04-2022 as per magesh sir advice
            strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =T.SNO) "
        End If
        If STKTRAN_REPAIR_SEPPOST Then '' added for vbj on 16-05-2022 as per magesh sir advice
            strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = T.ORDREPNO AND ORTYPE='R') "
        End If
        strSql += vbCrLf + " GROUP BY IM.CATCODE,T.COMPANYID,T.PURITY,IM.METALID,T.SALEMODE,T.STKTYPE"
        If NeedItemType_accpost Then
            strSql += vbCrLf + " ,T.ITEMTYPEID"
        End If
        strSql += vbCrLf + " UNION ALL "
        strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
        strSql += vbCrLf + " ,'IIN' AS TRANTYPE "
        strSql += vbCrLf + " ,SUM(T.PCS)PCS,SUM(T.GRSWT)GRSWT,SUM(T.NETWT)NETWT,SUM(T.LESSWT)LESSWT,CONVERT(NUMERIC(15,3),(SUM(T.NETWT)*T.PURITY)/100) AS PUREWT"
        strSql += vbCrLf + " ,ISNULL(T.ITEMID,0) ITEMID,0 SUBITEMID"
        'strSql += vbCrLf + " ,CASE WHEN SALEMODE <> 'W' OR IM.METALID IN ('D','T') THEN SUM(SALVALUE) ELSE 0 END RATE, SUM(TRFVALUE) AS AMOUNT,'" & cnCostId & "' COSTID,T.COMPANYID,T.PURITY,IM.CATCODE"
        strSql += vbCrLf + " ,0 RATE, SUM(TRFVALUE) AS AMOUNT,'" & cnCostId & "' COSTID,T.COMPANYID,T.PURITY" '',IM.CATCODE
        If NeedItemType_accpost Then
            strSql += vbCrLf + " ,CASE WHEN ISNULL(T.ITEMTYPEID,0)<>0 THEN CASE WHEN ISNULL((SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =T.ITEMTYPEID),'') <>''"
            strSql += vbCrLf + " THEN (SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =T.ITEMTYPEID) ELSE IM.CATCODE END ELSE  IM.CATCODE END CATCODE "
        Else
            strSql += vbCrLf + " ,IM.CATCODE"
        End If
        strSql += vbCrLf + " ,'" & Tocostacid & "' AS ACCODE"
        strSql += vbCrLf + " ,IM.METALID,T.STKTYPE"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
        strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
        strSql += " WHERE IM.BOOKSTOCK not in('W','N') AND T.SNO IN (select TAGSNO FROM TRANS_STATUS WHERE STOCKMODE = 'T' AND STATUS ='U' AND MOVETYPE = 'O')"
        If MetalBasedStone Then '' added for vbj on 11-04-2022 as per magesh sir advice
            strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =T.SNO) "
        End If
        If STKTRAN_REPAIR_SEPPOST Then '' added for vbj on 16-05-2022 as per magesh sir advice
            strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = T.ORDREPNO AND ORTYPE='R') "
        End If
        strSql += vbCrLf + " GROUP BY IM.CATCODE,T.ITEMID,T.COMPANYID,T.PURITY,IM.METALID,T.SALEMODE,T.STKTYPE"
        If NeedItemType_accpost Then
            strSql += vbCrLf + " ,T.ITEMTYPEID"
        End If

        If MetalBasedStone Then '' added for vbj on 11-04-2022 as per magesh sir advice
            strSql += vbCrLf + " UNION ALL "
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
            strSql += vbCrLf + " ,'IIN' AS TRANTYPE "
            strSql += vbCrLf + " ,0 PCS,SUM(MT.GRSWT)GRSWT,SUM(MT.NETWT)NETWT,0 LESSWT,CONVERT(NUMERIC(15,3),0) AS PUREWT "
            strSql += vbCrLf + " ,0 ITEMID,0 SUBITEMID,0 RATE"
            strSql += vbCrLf + " ,SUM(CONVERT(NUMERIC(15,2),((CASE WHEN T.GRSNET='N' THEN MT.NETWT ELSE MT.GRSWT END)* "
            strSql += vbCrLf + " (" & cnAdminDb & ".dbo.GET_TODAY_RATE(MT.CATCODE,'" & BillDate.ToString("yyyy-MM-dd") & "') "
            strSql += vbCrLf + " + (" & cnAdminDb & ".dbo.GET_TODAY_RATE(MT.CATCODE,'" & BillDate.ToString("yyyy-MM-dd") & "') "
            strSql += vbCrLf + " *((CASE WHEN C.METALID='G' THEN " & Val(AUTOBOOKVALUEG_PER.ToString)
            strSql += vbCrLf + " WHEN C.METALID='S' THEN " & Val(AUTOBOOKVALUES_PER.ToString)
            strSql += vbCrLf + " WHEN C.METALID='P' THEN " & Val(AUTOBOOKVALUEP_PER.ToString) & " ELSE 0 END"
            strSql += vbCrLf + " )/100)))))) AS AMOUNT"
            strSql += vbCrLf + " ,'" & cnCostId & "' COSTID,T.COMPANYID,T.PURITY,MT.CATCODE CATCODE"
            strSql += vbCrLf + " ,'" & Tocostacid & "' AS ACCODE"
            strSql += vbCrLf + " ,C.METALID,T.STKTYPE"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGMETAL AS MT"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMTAG AS T ON T.SNO = MT.TAGSNO"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE = MT.CATCODE"
            strSql += vbCrLf + " WHERE IM.BOOKSTOCK = 'W' AND T.SNO IN (select TAGSNO FROM TRANS_STATUS WHERE STOCKMODE = 'T' AND STATUS ='U' AND MOVETYPE = 'O')"
            strSql += vbCrLf + " AND  EXISTS (SELECT 1 FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =T.SNO) "
            If STKTRAN_REPAIR_SEPPOST Then '' added for vbj on 16-05-2022 as per magesh sir advice
                strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = T.ORDREPNO AND ORTYPE='R') "
            End If
            strSql += vbCrLf + " GROUP BY MT.CATCODE,T.COMPANYID,T.PURITY,C.METALID,T.SALEMODE,T.STKTYPE"
            strSql += vbCrLf + " UNION ALL "
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
            strSql += vbCrLf + " ,'IIN' AS TRANTYPE "
            strSql += vbCrLf + " ,0 PCS,SUM(MT.GRSWT)GRSWT,SUM(MT.NETWT)NETWT,0 LESSWT,CONVERT(NUMERIC(15,3),0) AS PUREWT "
            strSql += vbCrLf + " ,0 ITEMID,0 SUBITEMID,0 RATE"
            strSql += vbCrLf + " ,SUM(CONVERT(NUMERIC(15,2),((CASE WHEN T.GRSNET='N' THEN MT.NETWT ELSE MT.GRSWT END)* "
            strSql += vbCrLf + " (" & cnAdminDb & ".dbo.GET_TODAY_RATE(MT.CATCODE,'" & BillDate.ToString("yyyy-MM-dd") & "') "
            strSql += vbCrLf + " + (" & cnAdminDb & ".dbo.GET_TODAY_RATE(MT.CATCODE,'" & BillDate.ToString("yyyy-MM-dd") & "') "
            strSql += vbCrLf + " *((CASE WHEN C.METALID='G' THEN " & Val(AUTOBOOKVALUEG_PER.ToString)
            strSql += vbCrLf + " WHEN C.METALID='S' THEN " & Val(AUTOBOOKVALUES_PER.ToString)
            strSql += vbCrLf + " WHEN C.METALID='P' THEN " & Val(AUTOBOOKVALUEP_PER.ToString) & " ELSE 0 END"
            strSql += vbCrLf + " )/100)))))) AS AMOUNT"
            strSql += vbCrLf + " ,'" & cnCostId & "' COSTID,T.COMPANYID,T.PURITY,MT.CATCODE CATCODE"
            strSql += vbCrLf + " ,'" & Tocostacid & "' AS ACCODE"
            strSql += vbCrLf + " ,C.METALID,T.STKTYPE"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGMETAL AS MT"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMTAG AS T ON T.SNO = MT.TAGSNO"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE = MT.CATCODE"
            strSql += vbCrLf + " WHERE IM.BOOKSTOCK NOT IN('W','N') AND T.SNO IN (SELECT TAGSNO FROM TRANS_STATUS WHERE STOCKMODE = 'T' AND STATUS ='U' AND MOVETYPE = 'O')"
            strSql += vbCrLf + " AND  EXISTS (SELECT 1 FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =T.SNO) "
            If STKTRAN_REPAIR_SEPPOST Then '' added for vbj on 16-05-2022 as per magesh sir advice
                strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = T.ORDREPNO AND ORTYPE='R') "
            End If
            strSql += vbCrLf + " GROUP BY MT.CATCODE,T.COMPANYID,T.PURITY,C.METALID,T.SALEMODE,T.STKTYPE"
        End If

        If STKTRAN_REPAIR_SEPPOST Then '' added for vbj on 16-05-2022 as per magesh sir advice
            strSql += vbCrLf + " UNION ALL "
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
            strSql += vbCrLf + " ,'IIN' AS TRANTYPE "
            strSql += vbCrLf + " ,SUM(T.PCS)PCS,SUM(T.GRSWT-ISNULL(T.OREXCESSWT,0))GRSWT,SUM(T.NETWT-ISNULL(T.OREXCESSWT,0))NETWT,SUM(T.LESSWT)LESSWT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SUM(T.NETWT-ISNULL(T.OREXCESSWT,0))*T.PURITY)/100) AS PUREWT"
            strSql += vbCrLf + " ,0 ITEMID,0 SUBITEMID"
            strSql += vbCrLf + " , 0 RATE"
            strSql += vbCrLf + " , SUM(TRFVALUE) AS AMOUNT"
            strSql += vbCrLf + " ,'" & cnCostId & "' COSTID,T.COMPANYID,T.PURITY" '',IM.CATCODE
            strSql += vbCrLf + " ,CASE WHEN IM.METALID='G' THEN '" & OR_REP_NewCatCode & "' WHEN IM.METALID='S' THEN '" & OR_REP_NewCatCode_S & "' "
            strSql += vbCrLf + " WHEN IM.METALID='P' THEN " & IIf(OR_REP_NewCatCode_P.ToString <> "", "'" & OR_REP_NewCatCode_P & "'", "IM.CATCODE") & " ELSE IM.CATCODE END CATCODE"
            strSql += vbCrLf + " ,'" & Tocostacid & "' AS ACCODE"
            strSql += vbCrLf + " ,IM.METALID,T.STKTYPE"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
            strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
            strSql += vbCrLf + " WHERE IM.BOOKSTOCK = 'W' AND T.SNO IN (select TAGSNO FROM TRANS_STATUS WHERE STOCKMODE = 'T' AND STATUS ='U' AND MOVETYPE = 'O')"
            strSql += vbCrLf + " AND EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = T.ORDREPNO AND ORTYPE='R') "
            strSql += vbCrLf + " GROUP BY IM.CATCODE,T.COMPANYID,T.PURITY,IM.METALID,T.SALEMODE,T.STKTYPE"
            strSql += vbCrLf + " UNION ALL "
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
            strSql += vbCrLf + " ,'IIN' AS TRANTYPE "
            strSql += vbCrLf + " ,SUM(T.PCS)PCS,SUM(T.GRSWT-ISNULL(T.OREXCESSWT,0))GRSWT,SUM(T.NETWT-ISNULL(T.OREXCESSWT,0))NETWT,SUM(T.LESSWT)LESSWT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SUM(T.NETWT-ISNULL(T.OREXCESSWT,0))*T.PURITY)/100) AS PUREWT"
            strSql += vbCrLf + " ,ISNULL(T.ITEMID,0) ITEMID,0 SUBITEMID"
            'strSql += vbCrLf + " ,CASE WHEN SALEMODE <> 'W' OR IM.METALID IN ('D','T') THEN SUM(SALVALUE) ELSE 0 END RATE, SUM(TRFVALUE) AS AMOUNT,'" & cnCostId & "' COSTID,T.COMPANYID,T.PURITY,IM.CATCODE"
            strSql += vbCrLf + " ,0 RATE, SUM(TRFVALUE) AS AMOUNT,'" & cnCostId & "' COSTID,T.COMPANYID,T.PURITY" '',IM.CATCODE
            strSql += vbCrLf + " ,CASE WHEN IM.METALID='G' THEN '" & OR_REP_NewCatCode & "' WHEN IM.METALID='S' THEN '" & OR_REP_NewCatCode_S & "' "
            strSql += vbCrLf + " WHEN IM.METALID='P' THEN " & IIf(OR_REP_NewCatCode_P.ToString <> "", "'" & OR_REP_NewCatCode_P & "'", "IM.CATCODE") & " ELSE IM.CATCODE END CATCODE"
            strSql += vbCrLf + " ,'" & Tocostacid & "' AS ACCODE"
            strSql += vbCrLf + " ,IM.METALID,T.STKTYPE"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
            strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
            strSql += " WHERE IM.BOOKSTOCK not in('W','N') AND T.SNO IN (select TAGSNO FROM TRANS_STATUS WHERE STOCKMODE = 'T' AND STATUS ='U' AND MOVETYPE = 'O')"
            strSql += vbCrLf + " AND  EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = T.ORDREPNO AND ORTYPE='R') "
            strSql += vbCrLf + " GROUP BY IM.CATCODE,T.ITEMID,T.COMPANYID,T.PURITY,IM.METALID,T.SALEMODE,T.STKTYPE"
            ''excess wt
            strSql += vbCrLf + " UNION ALL "
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
            strSql += vbCrLf + " ,'IIN' AS TRANTYPE "
            strSql += vbCrLf + " ,0 PCS,SUM(ISNULL(T.OREXCESSWT,0))GRSWT,SUM(ISNULL(T.OREXCESSWT,0))NETWT,0 LESSWT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),0) AS PUREWT"
            strSql += vbCrLf + " ,0 ITEMID,0 SUBITEMID"
            strSql += vbCrLf + " , 0 RATE"
            strSql += vbCrLf + " , 0 AS AMOUNT"
            strSql += vbCrLf + " ,'" & cnCostId & "' COSTID,T.COMPANYID,T.PURITY"
            If NeedItemType_accpost Then
                strSql += vbCrLf + " ,CASE WHEN ISNULL(T.ITEMTYPEID,0)<>0 THEN CASE WHEN ISNULL((SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =T.ITEMTYPEID),'') <>''"
                strSql += vbCrLf + " THEN (SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =T.ITEMTYPEID) ELSE IM.CATCODE END ELSE  IM.CATCODE END CATCODE "
            Else
                strSql += vbCrLf + " ,IM.CATCODE"
            End If
            strSql += vbCrLf + " ,'" & Tocostacid & "' AS ACCODE"
            strSql += vbCrLf + " ,IM.METALID,T.STKTYPE"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
            strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
            strSql += vbCrLf + " WHERE IM.BOOKSTOCK = 'W' AND T.SNO IN (select TAGSNO FROM TRANS_STATUS WHERE STOCKMODE = 'T' AND STATUS ='U' AND MOVETYPE = 'O')"
            strSql += vbCrLf + " AND ISNULL(T.OREXCESSWT,0)<>0 "
            strSql += vbCrLf + " AND EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = T.ORDREPNO AND ORTYPE='R') "
            strSql += vbCrLf + " GROUP BY IM.CATCODE,T.COMPANYID,T.PURITY,IM.METALID,T.SALEMODE,T.STKTYPE"
            If NeedItemType_accpost Then
                strSql += vbCrLf + " ,T.ITEMTYPEID"
            End If
            strSql += vbCrLf + " UNION ALL "
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
            strSql += vbCrLf + " ,'IIN' AS TRANTYPE "
            strSql += vbCrLf + " ,0 PCS,SUM(ISNULL(T.OREXCESSWT,0))GRSWT,SUM(ISNULL(T.OREXCESSWT,0))NETWT,0 LESSWT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),0) AS PUREWT"
            strSql += vbCrLf + " ,0 ITEMID,0 SUBITEMID"
            strSql += vbCrLf + " ,0 RATE, 0 AS AMOUNT,'" & cnCostId & "' COSTID,T.COMPANYID,T.PURITY"
            If NeedItemType_accpost Then
                strSql += vbCrLf + " ,CASE WHEN ISNULL(T.ITEMTYPEID,0)<>0 THEN CASE WHEN ISNULL((SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =T.ITEMTYPEID),'') <>''"
                strSql += vbCrLf + " THEN (SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =T.ITEMTYPEID) ELSE IM.CATCODE END ELSE  IM.CATCODE END CATCODE "
            Else
                strSql += vbCrLf + " ,IM.CATCODE"
            End If
            strSql += vbCrLf + " ,'" & Tocostacid & "' AS ACCODE"
            strSql += vbCrLf + " ,IM.METALID,T.STKTYPE"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
            strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
            strSql += " WHERE IM.BOOKSTOCK not in('W','N') AND T.SNO IN (select TAGSNO FROM TRANS_STATUS WHERE STOCKMODE = 'T' AND STATUS ='U' AND MOVETYPE = 'O')"
            strSql += vbCrLf + " AND  EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = T.ORDREPNO AND ORTYPE='R') "
            strSql += vbCrLf + " AND ISNULL(T.OREXCESSWT,0)<>0 "
            strSql += vbCrLf + " GROUP BY IM.CATCODE,T.ITEMID,T.COMPANYID,T.PURITY,IM.METALID,T.SALEMODE,T.STKTYPE"
            If NeedItemType_accpost Then
                strSql += vbCrLf + " ,T.ITEMTYPEID"
            End If
        End If

        strSql += vbCrLf + " UNION ALL "
        strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
        strSql += vbCrLf + " ,'IIN' AS TRANTYPE "
        strSql += vbCrLf + " ,SUM(T.PCS)PCS,SUM(T.GRSWT)GRSWT,SUM(T.NETWT)NETWT,SUM(T.LESSWT)LESSWT,CONVERT(NUMERIC(15,3),(SUM(T.NETWT)*ISNULL((SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = CA.PURITYID),1))/100) AS PUREWT"
        strSql += vbCrLf + " ,0 AS ITEMID ,0 AS SUBITEMID"
        strSql += vbCrLf + " ,0 AS RATE, SUM(TRFVALUE) AS AMOUNT,'" & cnCostId & "' COSTID,T.COMPANYID,(SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = CA.PURITYID) AS PURITY,IM.CATCODE"
        strSql += vbCrLf + " ,'" & Tocostacid & "' AS ACCODE"
        strSql += vbCrLf + " ,IM.METALID,T.STKTYPE"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAG AS T"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE"
        strSql += vbCrLf + " WHERE IM.BOOKSTOCK = 'W' AND T.SNO IN (select TAGSNO FROM TRANS_STATUS WHERE STOCKMODE = 'N' AND STATUS ='U' AND MOVETYPE = 'O')"
        strSql += vbCrLf + " GROUP BY IM.CATCODE,T.COMPANYID,IM.METALID ,CA.PURITYID,T.STKTYPE"
        strSql += vbCrLf + " UNION ALL "
        strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
        strSql += vbCrLf + " ,'IIN' AS TRANTYPE "
        strSql += vbCrLf + " ,SUM(T.PCS)PCS,SUM(T.GRSWT)GRSWT,SUM(T.NETWT)NETWT,SUM(T.LESSWT)LESSWT,CONVERT(NUMERIC(15,3),(SUM(T.NETWT)*ISNULL((SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = CA.PURITYID),1))/100) AS PUREWT"
        strSql += vbCrLf + " ,ISNULL(T.ITEMID,0)AS ITEMID ,0 AS SUBITEMID"
        strSql += vbCrLf + " ,0 AS RATE, SUM(TRFVALUE) AS AMOUNT,'" & cnCostId & "' COSTID,T.COMPANYID,(SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = CA.PURITYID) AS PURITY,IM.CATCODE"
        strSql += vbCrLf + " ,'" & Tocostacid & "' AS ACCODE"
        strSql += vbCrLf + " ,IM.METALID,T.STKTYPE"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAG AS T"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE"
        strSql += vbCrLf + " WHERE IM.BOOKSTOCK NOT IN ('W','N') AND T.SNO IN (select TAGSNO FROM TRANS_STATUS WHERE STOCKMODE = 'N' AND STATUS ='U' AND MOVETYPE = 'O')"
        strSql += vbCrLf + " GROUP BY IM.CATCODE,T.ITEMID,T.COMPANYID,IM.METALID,CA.PURITYID,T.STKTYPE"
        strSql += vbCrLf + " ) AX"
        cmd = New OleDbCommand(strSql, XCnAdmin, tran)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        'strSql = vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
        'strSql += vbCrLf + " ,'IIN' AS TRANTYPE "
        'strSql += vbCrLf + " ,SUM(T.PCS)PCS,SUM(T.GRSWT)GRSWT,SUM(T.NETWT)NETWT,SUM(T.LESSWT)LESSWT,CONVERT(NUMERIC(15,3),(SUM(T.NETWT)*T.PURITY)/100) AS PUREWT"
        'strSql += vbCrLf + " ,ISNULL(T.ITEMID,0) ITEMID,0 SUBITEMID"
        ''strSql += vbCrLf + " ,CASE WHEN SALEMODE <> 'W' OR IM.METALID IN ('D','T') THEN SUM(SALVALUE) ELSE 0 END RATE, SUM(TRFVALUE) AS AMOUNT,'" & cnCostId & "' COSTID,T.COMPANYID,T.PURITY,IM.CATCODE"
        'strSql += vbCrLf + " ,0 RATE, SUM(TRFVALUE) AS AMOUNT,'" & cnCostId & "' COSTID,T.COMPANYID,T.PURITY,IM.CATCODE"
        'strSql += vbCrLf + " ,'" & Tocostacid & "' AS ACCODE"
        'strSql += vbCrLf + " ,IM.METALID,T.STKTYPE"
        'strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
        'strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
        'strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
        'strSql += " WHERE IM.BOOKSTOCK not in('W','N') AND T.SNO IN (select TAGSNO FROM TRANS_STATUS WHERE STOCKMODE = 'T' AND STATUS ='U' AND MOVETYPE = 'O')"
        'strSql += vbCrLf + " GROUP BY IM.CATCODE,T.ITEMID,T.COMPANYID,T.PURITY,IM.METALID,T.SALEMODE,T.STKTYPE"

        'Dim dttemp As New DataTable
        'cmd = New OleDbCommand(strSql, XCnAdmin, tran)
        'Dim das As New OleDbDataAdapter
        'das = New OleDbDataAdapter(cmd)
        'das.Fill(dttemp)

        strSql = vbCrLf + " SELECT IDENTITY(INT,1,1)AS KEYNO,CONVERT(VARCHAR(15),NULL)AS SNO"
        strSql += vbCrLf + " ,'IIN' AS TRANTYPE "
        strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(LESSWT)LESSWT,SUM(PUREWT) AS PUREWT"
        strSql += vbCrLf + " ,ITEMID,'G' GRSNET,0 RATE, SUM(AMOUNT) AS AMOUNT,COSTID,COMPANYID,CATCODE,CATCODE OCATCODE,'X' FLAG"
        strSql += vbCrLf + " ,ACCODE,METALID,53 ORDSTATE_ID,STKTYPE"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW FROM TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS"
        strSql += vbCrLf + " GROUP BY CATCODE,ITEMID,COMPANYID,METALID,ACCODE,COSTID,STKTYPE"
        cmd = New OleDbCommand(strSql, XCnAdmin, tran)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        strSql = vbCrLf + " UPDATE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW SET SNO = KEYNO"
        strSql += vbCrLf + " "
        strSql += vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
        strSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
        strSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
        strSql += vbCrLf + " ,@CTLID = 'ISSUECODE'"
        strSql += vbCrLf + " ,@CHECK_TABLENAME = 'ISSUE'"
        strSql += vbCrLf + " ,@UPD_DBNAME = 'TEMPTABLEDB'"
        strSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP_" & systemId & "TRANTRANS_NEW'"
        cmd = New OleDbCommand(strSql, XCnAdmin, tran)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " SELECT AXX.*"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW"
        strSql += vbCrLf + " FROM ("

        strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)ISSSNO"
        strSql += vbCrLf + " ,'IIN'TRANTYPE"
        strSql += vbCrLf + " ,STNPCS,STNWT,CONVERT(NUMERIC(15,2),0)AS STNRATE,CONVERT(NUMERIC(15,2),ST.TRFVALUE)AS STNAMT"
        strSql += vbCrLf + " ,ISNULL(ST.STNITEMID,0) STNITEMID,ST.STONEUNIT AS STONEUNIT"
        strSql += vbCrLf + " ,'" & cnCostId & "' COSTID,TG.COMPANYID"
        If NeedItemType_accpost Then
            strSql += vbCrLf + " ,CASE WHEN ISNULL(TG.ITEMTYPEID,0)<>0 THEN CASE WHEN ISNULL((SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =TG.ITEMTYPEID),'') <>''"
            strSql += vbCrLf + " THEN (SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =TG.ITEMTYPEID) ELSE TIM.CATCODE END ELSE  TIM.CATCODE END CATCODE "
        Else
            strSql += vbCrLf + " ,TIM.CATCODE AS CATCODE"
        End If
        strSql += vbCrLf + " ,IM.CATCODE AS TCATCODE,TG.COMPANYID AS TCOMPANYID,TG.PURITY AS TPURITY,C.METALID AS TMETALID"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE AS ST"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMTAG AS TG ON TG.SNO = ST.TAGSNO"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS TIM ON TIM.ITEMID = ST.ITEMID"
        strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
        strSql += " WHERE ST.TAGSNO IN (select TAGSNO FROM TRANS_STATUS WHERE STOCKMODE = 'T' AND STATUS ='U' and MOVETYPE = 'O')"
        If STKTRAN_REPAIR_SEPPOST Then '' added for vbj on 16-05-2022 as per magesh sir advice
            strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = TG.ORDREPNO AND ORTYPE='R') "
        End If
        If MetalBasedStone Then '' added for vbj on 11-04-2022 as per magesh sir advice
            strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =TG.SNO) "
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)ISSSNO"
            strSql += vbCrLf + " ,'IIN'TRANTYPE"
            strSql += vbCrLf + " ,STNPCS,STNWT,CONVERT(NUMERIC(15,2),0)AS STNRATE,CONVERT(NUMERIC(15,2),ST.TRFVALUE)AS STNAMT"
            strSql += vbCrLf + " ,ISNULL(ST.STNITEMID,0) STNITEMID,ST.STONEUNIT AS STONEUNIT"
            strSql += vbCrLf + " ,'" & cnCostId & "' COSTID,TG.COMPANYID"
            strSql += vbCrLf + " ,MT.CATCODE AS CATCODE"
            strSql += vbCrLf + " ,IM.CATCODE AS TCATCODE,TG.COMPANYID AS TCOMPANYID,TG.PURITY AS TPURITY,C.METALID AS TMETALID"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE AS ST"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMTAG AS TG ON TG.SNO = ST.TAGSNO"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMTAGMETAL AS MT ON TG.SNO = MT.TAGSNO AND MT.SNO=ST.TAGMSNO "
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
            strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
            strSql += " WHERE ST.TAGSNO IN (select TAGSNO FROM TRANS_STATUS WHERE STOCKMODE = 'T' AND STATUS ='U' and MOVETYPE = 'O')"
            strSql += vbCrLf + " AND  EXISTS (SELECT 1 FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =TG.SNO) "
            strSql += vbCrLf + " AND ISNULL(ST.TAGMSNO,'') IN (SELECT ISNULL(SNO,'') FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =TG.SNO) "
            If STKTRAN_REPAIR_SEPPOST Then '' added for vbj on 16-05-2022 as per magesh sir advice
                strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = TG.ORDREPNO AND ORTYPE='R') "
            End If
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)ISSSNO"
            strSql += vbCrLf + " ,'IIN'TRANTYPE"
            strSql += vbCrLf + " ,STNPCS,STNWT,CONVERT(NUMERIC(15,2),0)AS STNRATE,CONVERT(NUMERIC(15,2),ST.TRFVALUE)AS STNAMT"
            strSql += vbCrLf + " ,ISNULL(ST.STNITEMID,0) STNITEMID,ST.STONEUNIT AS STONEUNIT"
            strSql += vbCrLf + " ,'" & cnCostId & "' COSTID,TG.COMPANYID"
            strSql += vbCrLf + " ,(SELECT TOP 1  CATCODE FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =TG.SNO) AS CATCODE"
            strSql += vbCrLf + " ,IM.CATCODE AS TCATCODE,TG.COMPANYID AS TCOMPANYID,TG.PURITY AS TPURITY,C.METALID AS TMETALID"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE AS ST"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMTAG AS TG ON TG.SNO = ST.TAGSNO"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
            strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
            strSql += " WHERE ST.TAGSNO IN (select TAGSNO FROM TRANS_STATUS WHERE STOCKMODE = 'T' AND STATUS ='U' and MOVETYPE = 'O')"
            strSql += vbCrLf + " AND  EXISTS (SELECT 1 FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =TG.SNO) "
            strSql += vbCrLf + " AND ISNULL(ST.TAGMSNO,'') NOT IN (SELECT ISNULL(SNO,'') FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO =TG.SNO) "
            If STKTRAN_REPAIR_SEPPOST Then '' added for vbj on 16-05-2022 as per magesh sir advice
                strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = TG.ORDREPNO AND ORTYPE='R') "
            End If
        End If
        If STKTRAN_REPAIR_SEPPOST Then '' added for vbj on 16-05-2022 as per magesh sir advice
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)ISSSNO"
            strSql += vbCrLf + " ,'IIN'TRANTYPE"
            strSql += vbCrLf + " ,STNPCS,STNWT,CONVERT(NUMERIC(15,2),0)AS STNRATE,CONVERT(NUMERIC(15,2),ST.TRFVALUE)AS STNAMT"
            strSql += vbCrLf + " ,ISNULL(ST.STNITEMID,0) STNITEMID,ST.STONEUNIT AS STONEUNIT"
            strSql += vbCrLf + " ,'" & cnCostId & "' COSTID,TG.COMPANYID"
            strSql += vbCrLf + " ,CASE WHEN TIM.METALID='G' THEN '" & OR_REP_NewCatCode & "' WHEN TIM.METALID='S' THEN '" & OR_REP_NewCatCode_S & "' "
            strSql += vbCrLf + " WHEN IM.METALID='P' THEN " & IIf(OR_REP_NewCatCode_P.ToString <> "", "'" & OR_REP_NewCatCode_P & "'", "IM.CATCODE") & " ELSE IM.CATCODE END CATCODE"
            strSql += vbCrLf + " ,IM.CATCODE AS TCATCODE,TG.COMPANYID AS TCOMPANYID,TG.PURITY AS TPURITY,C.METALID AS TMETALID"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE AS ST"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMTAG AS TG ON TG.SNO = ST.TAGSNO"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS TIM ON TIM.ITEMID = ST.ITEMID"
            strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
            strSql += " WHERE ST.TAGSNO IN (SELECT TAGSNO FROM TRANS_STATUS WHERE STOCKMODE = 'T' AND STATUS ='U' and MOVETYPE = 'O')"
            strSql += vbCrLf + " AND EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORMAST WHERE ORNO = TG.ORDREPNO AND ORTYPE='R') "
        End If
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)ISSSNO"
        strSql += vbCrLf + " ,'IIN'TRANTYPE"
        strSql += vbCrLf + " ,STNPCS,STNWT,CONVERT(NUMERIC(15,2),0)AS STNRATE,CONVERT(NUMERIC(15,2),0)AS STNAMT"
        strSql += vbCrLf + " ,ISNULL((SELECT ITEMID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = IM.CATCODE),0)AS STNITEMID"
        strSql += vbCrLf + " ,ST.STONEUNIT AS STONEUNIT,'" & cnCostId & "' COSTID,TG.COMPANYID"
        strSql += vbCrLf + " ,TIM.CATCODE AS CATCODE,IM.CATCODE AS TCATCODE,TG.COMPANYID AS TCOMPANYID,CONVERT(NUMERIC(15,2),0) AS TPURITY,C.METALID AS TMETALID"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAGSTONE AS ST"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMNONTAG AS TG ON TG.SNO = ST.TAGSNO"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS TIM ON TIM.ITEMID = ST.ITEMID"
        strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
        strSql += " WHERE ST.TAGSNO IN (SELECT TAGSNO FROM TRANS_STATUS WHERE STOCKMODE = 'N' AND STATUS ='U' and MOVETYPE = 'O')"
        strSql += vbCrLf + " ) AS AXX"

        strSql += vbCrLf + " UPDATE SV  SET SV.ISSSNO = T.SNO"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW AS SV"
        strSql += vbCrLf + " INNER JOIN TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW AS T ON T.COMPANYID = SV.TCOMPANYID"
        strSql += vbCrLf + " AND T.CATCODE = SV.CATCODE "
        strSql += vbCrLf + " "
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " CONVERT(VARCHAR(15),NULL)SNO,ISSSNO,TRANTYPE,STNITEMID,SUM(STNPCS)STNPCS,SUM(STNWT)STNWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0) AS STNRATE,SUM(STNAMT) AS STNAMT"
        strSql += vbCrLf + " ,TCATCODE AS CATCODE,COSTID,COMPANYID,STONEUNIT"
        strSql += vbCrLf + " ,IDENTITY(INT,1,1)KEYNO,TMETALID AS STONEMODE"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW "
        strSql += vbCrLf + " GROUP BY ISSSNO,TRANTYPE,STNITEMID,TCATCODE,TMETALID,COSTID,COMPANYID,STONEUNIT"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE SET SNO = KEYNO"

        strSql += vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
        strSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
        strSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
        strSql += vbCrLf + " ,@CTLID = 'ISSSTONECODE'"
        strSql += vbCrLf + " ,@CHECK_TABLENAME = 'ISSSTONE'"
        strSql += vbCrLf + " ,@UPD_DBNAME = 'TEMPTABLEDB'"
        strSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP_" & systemId & "TRANTRANS_STONE'"
        cmd = New OleDbCommand(strSql, XCnAdmin, tran)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        Dim DtTag As New DataTable
        strSql = "  SELECT * FROM TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW"
        cmd = New OleDbCommand(strSql, XCnAdmin, tran)
        da = New OleDbDataAdapter(cmd)
        da.Fill(DtTag)

        Dim DtTagStone As New DataTable
        strSql = "  SELECT * FROM TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE"
        cmd = New OleDbCommand(strSql, XCnAdmin, tran)
        da = New OleDbDataAdapter(cmd)
        da.Fill(DtTagStone)

        strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE"
        cmd = New OleDbCommand(strSql, XCnAdmin, tran)
        ': cmd.CommandTimeout = 1000 : 
        cmd.ExecuteNonQuery()



        Dim DtIssue As New DataTable
        Dim DtIssStone As New DataTable
        DtIssue = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, "ISSUE", XCnAdmin, tran)
        DtIssStone = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, "ISSSTONE", XCnAdmin, tran)
        Dim DtAcctran As New DataTable
        DtAcctran = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, "ACCTRAN", XCnAdmin, tran)
        Dim DtTaxtran As New DataTable
        DtTaxtran = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, "TAXTRAN", XCnAdmin, tran)

        Dim RoIns As DataRow = Nothing
        For Each Ro As DataRow In DtTag.Rows
            RoIns = DtIssue.NewRow
            For Each Col As DataColumn In DtTag.Columns
                If DtIssue.Columns.Contains(Col.ColumnName) = False Then Continue For
                RoIns.Item(Col.ColumnName) = Ro.Item(Col.ColumnName)
            Next
            DtIssue.Rows.Add(RoIns)
        Next
        For Each Ro As DataRow In DtTagStone.Rows
            RoIns = DtIssStone.NewRow
            For Each Col As DataColumn In DtTagStone.Columns
                If DtIssStone.Columns.Contains(Col.ColumnName) = False Then Continue For
                RoIns.Item(Col.ColumnName) = Ro.Item(Col.ColumnName)
            Next
            DtIssStone.Rows.Add(RoIns)
        Next
        Dim RoGstTax As DataRow = Nothing

        For Each Ro As DataRow In DtTag.Rows

            GSTCALC(Val(Ro.Item("AMOUNT").ToString), Ro.Item("CATCODE").ToString)
            If GST And StateId <> CompanyStateId And IGST > 0 Then
                RoGstTax = DtTaxtran.NewRow
                RoGstTax.Item("SNO") = GetNewSnoNew(TranSnoType.TAXTRANCODE, XCnAdmin, tran)  ''SNO
                RoGstTax.Item("ISSSNO") = Ro.Item("SNO")
                RoGstTax.Item("TRANNO") = Tranno
                RoGstTax.Item("TRANDATE") = BillDate
                RoGstTax.Item("TRANTYPE") = "IIN"
                RoGstTax.Item("BATCHNO") = Batchno
                RoGstTax.Item("TAXID") = "IG"
                RoGstTax.Item("AMOUNT") = Ro.Item("AMOUNT")
                If Val(Ro.Item("AMOUNT").ToString) = 0 Then
                    IGST = 0
                End If
                RoGstTax.Item("TAXAMOUNT") = IGST
                RoGstTax.Item("TAXPER") = IGSTPER
                RoGstTax.Item("TAXTYPE") = DBNull.Value
                RoGstTax.Item("TSNO") = "3"
                RoGstTax.Item("COSTID") = Ro.Item("COSTID")
                RoGstTax.Item("COMPANYID") = strCompanyId
                DtTaxtran.Rows.Add(RoGstTax)
            End If
        Next

        'Batchno = GetNewBatchno(cnCostId, BillDate, tran)
        For Each Ro As DataRow In DtIssue.Rows
            Ro.Item("TRANNO") = Tranno
            Ro.Item("TRANDATE") = BillDate
            Ro.Item("USERID") = userId
            Ro.Item("UPDATED") = GetServerDate()
            'Ro.Item("UPTIME") = Date.Now.ToLongTimeString
            Ro.Item("APPVER") = VERSION
            Ro.Item("BATCHNO") = Batchno
            Ro.Item("REFNO") = RefNo
            Ro.Item("BAGNO") = Transistno.ToString
            Ro.Item("REFDATE") = RefDate
            Ro.Item("REMARK1") = txtRemark1.Text
            Ro.Item("REMARK2") = txtRemark2.Text
            Ro.Item("STNAMT") = Val(DtIssStone.Compute("SUM(STNAMT)", "ISSSNO='" & Ro.Item("SNO").ToString & "'").ToString)
            If GST And StateId <> CompanyStateId Then
                GSTCALC(Val(Ro.Item("AMOUNT").ToString), Ro.Item("CATCODE").ToString)
            End If
            'If GST And StateId <> CompanyStateId Then GSTCALC(ISNULL(Val(Ro.Item("AMOUNT")), , Ro.Item("CATCODE").ToString)
            If GST And StateId <> CompanyStateId And IGST > 0 Then
                If Val(Ro.Item("AMOUNT").ToString) = 0 Then
                    IGST = 0
                End If
                Ro.Item("TAX") = IGST
            End If
            If _isFrachisee Then
                Ro.Item("WASTPER") = ObjMaxMinValue.txtMaxWastage_Per.Text
                Ro.Item("MCGRM") = ObjMaxMinValue.txtMaxMcPerGram_Amt.Text
            End If

            If AUTOBOOKVALUEENABLE = "Y" Or _isFrachisee Then
                Dim mamount As Decimal = Val(Ro.Item("AMOUNT").ToString)
                If mamount <> 0 Then
                    If AUTOINTERNAL_VOUCHER = "Y" Or _isFrachisee Then
                        Dim Roacct As DataRow = Nothing
                        If STKTRAN_ContraPost Then
                            Roacct = DtAcctran.NewRow
                            With Roacct
                                .Item("SNO") = GetNewSnoNew(TranSnoType.ACCTRANCODE, XCnAdmin, tran)
                                .Item("BATCHNO") = Batchno : .Item("REFNO") = RefNo
                                .Item("USERID") = userId : .Item("UPDATED") = GetServerDate() : .Item("APPVER") = VERSION
                                .Item("COMPANYID") = Ro.Item("COMPANYID") : .Item("COSTID") = cnCostId : .Item("FROMFLAG") = "A"
                                .Item("TRANNO") = Ro.Item("TRANNO") : .Item("TRANDATE") = Ro.Item("TRANDATE") : .Item("ACCODE") = Ro.Item("ACCODE") : .Item("TRANMODE") = "C" : .Item("PAYMODE") = "TI"
                                .Item("CONTRA") = IIf(_contraaccode.ToString <> "", _contraaccode.ToString, "STKTRAN") : .Item("PCS") = Ro.Item("PCS") : .Item("GRSWT") = Ro.Item("GRSWT")
                                .Item("NETWT") = Ro.Item("NETWT") : .Item("AMOUNT") = mamount
                                If GST And StateId <> CompanyStateId And IGST > 0 Then
                                    .Item("AMOUNT") = Val(mamount + IGST)
                                Else
                                    .Item("AMOUNT") = mamount
                                End If
                                .Item("REMARK1") = txtRemark1.Text
                                .Item("REMARK2") = txtRemark2.Text
                            End With
                            DtAcctran.Rows.Add(Roacct)
                            DtAcctran.AcceptChanges()
                            Roacct = DtAcctran.NewRow
                            With Roacct
                                .Item("SNO") = GetNewSnoNew(TranSnoType.ACCTRANCODE, XCnAdmin, tran)
                                .Item("BATCHNO") = Batchno : .Item("REFNO") = RefNo
                                .Item("USERID") = userId : .Item("UPDATED") = GetServerDate() : .Item("APPVER") = VERSION
                                .Item("COMPANYID") = Ro.Item("COMPANYID") : .Item("COSTID") = cnCostId : .Item("FROMFLAG") = "A"
                                .Item("TRANNO") = Ro.Item("TRANNO") : .Item("TRANDATE") = Ro.Item("TRANDATE") : .Item("ACCODE") = IIf(_contraaccode.ToString <> "", _contraaccode.ToString, "STKTRAN")
                                .Item("TRANMODE") = "D" : .Item("PAYMODE") = "TI"
                                .Item("CONTRA") = Ro.Item("ACCODE") : .Item("PCS") = Ro.Item("PCS") : .Item("GRSWT") = Ro.Item("GRSWT") : .Item("NETWT") = Ro.Item("NETWT") : .Item("AMOUNT") = mamount
                                .Item("REMARK1") = txtRemark1.Text
                                .Item("REMARK2") = txtRemark2.Text
                            End With
                            DtAcctran.Rows.Add(Roacct)
                            DtAcctran.AcceptChanges()
                            'GST                        
                            If GST And StateId <> CompanyStateId And IGST > 0 Then
                                Roacct = DtAcctran.NewRow
                                With Roacct
                                    .Item("SNO") = GetNewSnoNew(TranSnoType.ACCTRANCODE, XCnAdmin, tran)
                                    .Item("BATCHNO") = Batchno : .Item("REFNO") = RefNo
                                    .Item("USERID") = userId : .Item("UPDATED") = GetServerDate() : .Item("APPVER") = VERSION
                                    .Item("COMPANYID") = Ro.Item("COMPANYID") : .Item("COSTID") = cnCostId : .Item("FROMFLAG") = "A"
                                    .Item("TRANNO") = Ro.Item("TRANNO") : .Item("TRANDATE") = Ro.Item("TRANDATE") : .Item("ACCODE") = ICode : .Item("TRANMODE") = "D" : .Item("PAYMODE") = "TI"
                                    .Item("CONTRA") = Ro.Item("ACCODE")
                                    .Item("PCS") = 0
                                    .Item("GRSWT") = 0
                                    .Item("NETWT") = 0
                                    .Item("AMOUNT") = IGST
                                    .Item("REMARK1") = "/" & txtRemark1.Text
                                    .Item("REMARK2") = txtRemark2.Text
                                End With
                                DtAcctran.Rows.Add(Roacct)
                                DtAcctran.AcceptChanges()
                            End If
                        Else
                            Roacct = DtAcctran.NewRow
                            With Roacct
                                .Item("SNO") = GetNewSnoNew(TranSnoType.ACCTRANCODE, XCnAdmin, tran)
                                .Item("BATCHNO") = Batchno : .Item("REFNO") = RefNo
                                .Item("USERID") = userId : .Item("UPDATED") = GetServerDate() : .Item("APPVER") = VERSION
                                .Item("COMPANYID") = Ro.Item("COMPANYID") : .Item("COSTID") = cnCostId : .Item("FROMFLAG") = "A"
                                .Item("TRANNO") = Ro.Item("TRANNO") : .Item("TRANDATE") = Ro.Item("TRANDATE") : .Item("ACCODE") = Ro.Item("ACCODE") : .Item("TRANMODE") = "D" : .Item("PAYMODE") = "TI"
                                .Item("CONTRA") = "STKTRAN" : .Item("PCS") = Ro.Item("PCS") : .Item("GRSWT") = Ro.Item("GRSWT")
                                .Item("NETWT") = Ro.Item("NETWT") : .Item("AMOUNT") = mamount
                                If GST And StateId <> CompanyStateId And IGST > 0 Then
                                    .Item("AMOUNT") = Val(mamount + IGST)
                                Else
                                    .Item("AMOUNT") = mamount
                                End If
                                .Item("REMARK1") = txtRemark1.Text
                                .Item("REMARK2") = txtRemark2.Text
                            End With
                            DtAcctran.Rows.Add(Roacct)
                            DtAcctran.AcceptChanges()
                            Roacct = DtAcctran.NewRow
                            With Roacct
                                .Item("SNO") = GetNewSnoNew(TranSnoType.ACCTRANCODE, XCnAdmin, tran)
                                .Item("BATCHNO") = Batchno : .Item("REFNO") = RefNo
                                .Item("USERID") = userId : .Item("UPDATED") = GetServerDate() : .Item("APPVER") = VERSION
                                .Item("COMPANYID") = Ro.Item("COMPANYID") : .Item("COSTID") = cnCostId : .Item("FROMFLAG") = "A"
                                .Item("TRANNO") = Ro.Item("TRANNO") : .Item("TRANDATE") = Ro.Item("TRANDATE") : .Item("ACCODE") = "STKTRAN"
                                .Item("TRANMODE") = "C" : .Item("PAYMODE") = "TI"
                                .Item("CONTRA") = Ro.Item("ACCODE") : .Item("PCS") = Ro.Item("PCS") : .Item("GRSWT") = Ro.Item("GRSWT") : .Item("NETWT") = Ro.Item("NETWT") : .Item("AMOUNT") = mamount
                                .Item("REMARK1") = txtRemark1.Text
                                .Item("REMARK2") = txtRemark2.Text
                            End With
                            DtAcctran.Rows.Add(Roacct)
                            DtAcctran.AcceptChanges()
                            'GST                        
                            If GST And StateId <> CompanyStateId And IGST > 0 Then
                                Roacct = DtAcctran.NewRow
                                With Roacct
                                    .Item("SNO") = GetNewSnoNew(TranSnoType.ACCTRANCODE, XCnAdmin, tran)
                                    .Item("BATCHNO") = Batchno : .Item("REFNO") = RefNo
                                    .Item("USERID") = userId : .Item("UPDATED") = GetServerDate() : .Item("APPVER") = VERSION
                                    .Item("COMPANYID") = Ro.Item("COMPANYID") : .Item("COSTID") = cnCostId : .Item("FROMFLAG") = "A"
                                    .Item("TRANNO") = Ro.Item("TRANNO") : .Item("TRANDATE") = Ro.Item("TRANDATE") : .Item("ACCODE") = ICode : .Item("TRANMODE") = "C" : .Item("PAYMODE") = "TI"
                                    .Item("CONTRA") = Ro.Item("ACCODE")
                                    .Item("PCS") = 0
                                    .Item("GRSWT") = 0
                                    .Item("NETWT") = 0
                                    .Item("AMOUNT") = IGST
                                    .Item("REMARK1") = "/" & txtRemark1.Text
                                    .Item("REMARK2") = txtRemark2.Text
                                End With
                                DtAcctran.Rows.Add(Roacct)
                                DtAcctran.AcceptChanges()
                            End If
                        End If

                    End If
                End If
                'Ro.Item("AMOUNT") = mamount

            End If

        Next
        DtIssue.AcceptChanges()
        For Each Ro As DataRow In DtIssStone.Rows
            Ro.Item("TRANNO") = Tranno
            Ro.Item("TRANDATE") = BillDate
            Ro.Item("APPVER") = VERSION
            Ro.Item("BATCHNO") = Batchno
            Ro.Item("STONEMODE") = ""
        Next
        DtIssStone.AcceptChanges()

        InsertData(SyncMode.Transaction, DtIssue, XCnAdmin, tran, cnCostId)
        InsertData(SyncMode.Transaction, DtIssStone, XCnAdmin, tran, cnCostId)
        If AUTOINTERNAL_VOUCHER = "Y" Or _isFrachisee Then InsertData(SyncMode.Transaction, DtAcctran, XCnAdmin, tran, cnCostId) : InsertData(SyncMode.Transaction, DtTaxtran, XCnAdmin, tran, cnCostId)
    End Sub

    Private Sub CreateSalesInvoice(ByVal ToCostId As String, ByVal RefNo As String, ByVal RefDate As Date, ByVal Tranno As Long, ByVal Transistno As Integer, Optional ByVal Fromcostid As String = Nothing, Optional ByVal Tocostacid As String = "")
        strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS"
        strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE"
        cmd = New OleDbCommand(strSql, XCnAdmin, tran)
        ': cmd.CommandTimeout = 1000 : 
        cmd.ExecuteNonQuery()


        strSql = vbCrLf + " SELECT AX.*" ',IDENTITY(INT,1,1)AS KEYNO"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS "
        strSql += vbCrLf + " FROM ("
        strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
        strSql += vbCrLf + " ,'SA' AS TRANTYPE "
        strSql += vbCrLf + " ,SUM(T.PCS)PCS,SUM(T.GRSWT)GRSWT,SUM(T.NETWT)NETWT,SUM(T.LESSWT)LESSWT,CONVERT(NUMERIC(15,3),(SUM(T.NETWT)*T.PURITY)/100) AS PUREWT"
        strSql += vbCrLf + " ,T.ITEMID,T.SUBITEMID"
        strSql += vbCrLf + " ,SUM(TRFVALUE) AS AMOUNT,'" & cnCostId & "' COSTID,T.COMPANYID,T.PURITY" '',IM.CATCODE
        If NeedItemType_accpost Then
            strSql += vbCrLf + " ,CASE WHEN ISNULL(T.ITEMTYPEID,0)<>0 THEN CASE WHEN ISNULL((SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =T.ITEMTYPEID),'') <>''"
            strSql += vbCrLf + " THEN (SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =T.ITEMTYPEID) ELSE IM.CATCODE END ELSE  IM.CATCODE END CATCODE "
        Else
            strSql += vbCrLf + " ,IM.CATCODE"
        End If
        strSql += vbCrLf + " ,'" & Tocostacid & "' AS ACCODE"
        strSql += vbCrLf + " ,IM.METALID,T.TAGNO,T.RATE"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
        strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
        strSql += vbCrLf + " WHERE IM.BOOKSTOCK = 'W' AND T.SNO IN (select TAGSNO FROM TRANS_STATUS WHERE STOCKMODE = 'T' AND STATUS ='U' AND MOVETYPE = 'O')"
        strSql += vbCrLf + " GROUP BY IM.CATCODE,T.COMPANYID,T.PURITY,IM.METALID,T.SALEMODE,T.SUBITEMID,T.ITEMID,T.TAGNO,T.RATE"
        If NeedItemType_accpost Then
            strSql += vbCrLf + " ,T.ITEMTYPEID"
        End If
        strSql += vbCrLf + " UNION ALL "
        strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
        strSql += vbCrLf + " ,'SA' AS TRANTYPE "
        strSql += vbCrLf + " ,SUM(T.PCS)PCS,SUM(T.GRSWT)GRSWT,SUM(T.NETWT)NETWT,SUM(T.LESSWT)LESSWT,CONVERT(NUMERIC(15,3),(SUM(T.NETWT)*T.PURITY)/100) AS PUREWT"
        strSql += vbCrLf + " ,ISNULL(T.ITEMID,0) ITEMID, T.SUBITEMID"
        strSql += vbCrLf + " ,SUM(TRFVALUE) AS AMOUNT,'" & cnCostId & "' COSTID,T.COMPANYID,T.PURITY" '',IM.CATCODE
        If NeedItemType_accpost Then
            strSql += vbCrLf + " ,CASE WHEN ISNULL(T.ITEMTYPEID,0)<>0 THEN CASE WHEN ISNULL((SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =T.ITEMTYPEID),'') <>''"
            strSql += vbCrLf + " THEN (SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =T.ITEMTYPEID) ELSE IM.CATCODE END ELSE  IM.CATCODE END CATCODE "
        Else
            strSql += vbCrLf + " ,IM.CATCODE"
        End If
        strSql += vbCrLf + " ,'" & Tocostacid & "' AS ACCODE"
        strSql += vbCrLf + " ,IM.METALID,T.TAGNO,T.RATE"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
        strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
        strSql += " WHERE IM.BOOKSTOCK not in('W','N') AND T.SNO IN (select TAGSNO FROM TRANS_STATUS WHERE STOCKMODE = 'T' AND STATUS ='U' AND MOVETYPE = 'O')"
        strSql += vbCrLf + " GROUP BY IM.CATCODE,T.ITEMID,T.COMPANYID,T.PURITY,IM.METALID,T.SALEMODE,T.SUBITEMID,T.TAGNO,T.RATE"
        If NeedItemType_accpost Then
            strSql += vbCrLf + " ,T.ITEMTYPEID"
        End If
        strSql += vbCrLf + " UNION ALL "
        strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
        strSql += vbCrLf + " ,'SA' AS TRANTYPE "
        strSql += vbCrLf + " ,SUM(T.PCS)PCS,SUM(T.GRSWT)GRSWT,SUM(T.NETWT)NETWT,SUM(T.LESSWT)LESSWT,CONVERT(NUMERIC(15,3),(SUM(T.NETWT)*ISNULL((SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = CA.PURITYID),1))/100) AS PUREWT"
        strSql += vbCrLf + " ,T.ITEMID ,T.SUBITEMID"
        strSql += vbCrLf + " ,SUM(TRFVALUE) AS AMOUNT,'" & cnCostId & "' COSTID,T.COMPANYID,(SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = CA.PURITYID) AS PURITY,IM.CATCODE"
        strSql += vbCrLf + " ,'" & Tocostacid & "' AS ACCODE"
        strSql += vbCrLf + " ,IM.METALID,NULL AS TAGNO,T.RATE"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAG AS T"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE"
        strSql += vbCrLf + " WHERE IM.BOOKSTOCK = 'W' AND T.SNO IN (select TAGSNO FROM TRANS_STATUS WHERE STOCKMODE = 'N' AND STATUS ='U' AND MOVETYPE = 'O')"
        strSql += vbCrLf + " GROUP BY IM.CATCODE,T.COMPANYID,IM.METALID ,CA.PURITYID,T.SUBITEMID,T.ITEMID,T.RATE"
        strSql += vbCrLf + " UNION ALL "
        strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
        strSql += vbCrLf + " ,'SA' AS TRANTYPE "
        strSql += vbCrLf + " ,SUM(T.PCS)PCS,SUM(T.GRSWT)GRSWT,SUM(T.NETWT)NETWT,SUM(T.LESSWT)LESSWT,CONVERT(NUMERIC(15,3),(SUM(T.NETWT)*ISNULL((SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = CA.PURITYID),1))/100) AS PUREWT"
        strSql += vbCrLf + " ,ISNULL(T.ITEMID,0)AS ITEMID ,T.SUBITEMID"
        strSql += vbCrLf + " ,SUM(TRFVALUE) AS AMOUNT,'" & cnCostId & "' COSTID,T.COMPANYID,(SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = CA.PURITYID) AS PURITY,IM.CATCODE"
        strSql += vbCrLf + " ,'" & Tocostacid & "' AS ACCODE"
        strSql += vbCrLf + " ,IM.METALID,NULL AS TAGNO,T.RATE"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAG AS T"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE"
        strSql += vbCrLf + " WHERE IM.BOOKSTOCK NOT IN ('W','N') AND T.SNO IN (select TAGSNO FROM TRANS_STATUS WHERE STOCKMODE = 'N' AND STATUS ='U' AND MOVETYPE = 'O')"
        strSql += vbCrLf + " GROUP BY IM.CATCODE,T.COMPANYID,IM.METALID,CA.PURITYID,T.SUBITEMID,T.ITEMID,T.RATE"
        strSql += vbCrLf + " ) AX"
        cmd = New OleDbCommand(strSql, XCnAdmin, tran)
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " SELECT IDENTITY(INT,1,1)AS KEYNO,CONVERT(VARCHAR(15),NULL)AS SNO"
        strSql += vbCrLf + " ,'SA' AS TRANTYPE "
        strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(LESSWT)LESSWT,SUM(PUREWT) AS PUREWT"
        strSql += vbCrLf + " ,ITEMID,'G' GRSNET, RATE, SUM(AMOUNT) AS AMOUNT,COSTID,COMPANYID,CATCODE,CATCODE OCATCODE,'' FLAG"
        strSql += vbCrLf + " ,ACCODE,METALID,53 ORDSTATE_ID,SUBITEMID,TAGNO"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW FROM TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS"
        strSql += vbCrLf + " GROUP BY CATCODE,ITEMID,COMPANYID,METALID,ACCODE,COSTID,SUBITEMID,TAGNO,RATE"
        cmd = New OleDbCommand(strSql, XCnAdmin, tran)
        cmd.ExecuteNonQuery()


        strSql = vbCrLf + " UPDATE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW SET SNO = KEYNO"
        strSql += vbCrLf + " "
        strSql += vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
        strSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
        strSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
        strSql += vbCrLf + " ,@CTLID = 'ISSUECODE'"
        strSql += vbCrLf + " ,@CHECK_TABLENAME = 'ISSUE'"
        strSql += vbCrLf + " ,@UPD_DBNAME = 'TEMPTABLEDB'"
        strSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP_" & systemId & "TRANTRANS_NEW'"
        cmd = New OleDbCommand(strSql, XCnAdmin, tran)
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " SELECT AXX.*"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW"
        strSql += vbCrLf + " FROM ("

        strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)ISSSNO"
        strSql += vbCrLf + " ,'SA'TRANTYPE"
        strSql += vbCrLf + " ,STNPCS,STNWT,CONVERT(NUMERIC(15,2),0)AS STNRATE,CONVERT(NUMERIC(15,2),ST.TRFVALUE)AS STNAMT"
        strSql += vbCrLf + " ,ISNULL(ST.STNITEMID,0) STNITEMID,ST.STONEUNIT AS STONEUNIT"
        strSql += vbCrLf + " ,'" & cnCostId & "' COSTID,TG.COMPANYID"
        ''strSql += vbCrLf + " ,TIM.CATCODE AS CATCODE,IM.CATCODE AS TCATCODE,TG.COMPANYID AS TCOMPANYID,TG.PURITY AS TPURITY,C.METALID AS TMETALID"
        If NeedItemType_accpost Then
            strSql += vbCrLf + " ,CASE WHEN ISNULL(TG.ITEMTYPEID,0)<>0 THEN CASE WHEN ISNULL((SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =TG.ITEMTYPEID),'') <>''"
            strSql += vbCrLf + " THEN (SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =TG.ITEMTYPEID) ELSE TIM.CATCODE END ELSE  TIM.CATCODE END CATCODE "
        Else
            strSql += vbCrLf + " ,TIM.CATCODE AS CATCODE"
        End If
        strSql += vbCrLf + " ,IM.CATCODE AS TCATCODE,TG.COMPANYID AS TCOMPANYID,TG.PURITY AS TPURITY,C.METALID AS TMETALID"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE AS ST"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMTAG AS TG ON TG.SNO = ST.TAGSNO"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS TIM ON TIM.ITEMID = ST.ITEMID"
        strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
        strSql += " WHERE ST.TAGSNO IN (select TAGSNO FROM TRANS_STATUS WHERE STOCKMODE = 'T' AND STATUS ='U' and MOVETYPE = 'O')"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)ISSSNO"
        strSql += vbCrLf + " ,'SA'TRANTYPE"
        strSql += vbCrLf + " ,STNPCS,STNWT,CONVERT(NUMERIC(15,2),0)AS STNRATE,CONVERT(NUMERIC(15,2),0)AS STNAMT"
        strSql += vbCrLf + " ,ISNULL((SELECT ITEMID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = IM.CATCODE),0)AS STNITEMID"
        strSql += vbCrLf + " ,ST.STONEUNIT AS STONEUNIT,'" & cnCostId & "' COSTID,TG.COMPANYID"
        strSql += vbCrLf + " ,TIM.CATCODE AS CATCODE,IM.CATCODE AS TCATCODE,TG.COMPANYID AS TCOMPANYID,CONVERT(NUMERIC(15,2),0) AS TPURITY,C.METALID AS TMETALID"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAGSTONE AS ST"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMNONTAG AS TG ON TG.SNO = ST.TAGSNO"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS TIM ON TIM.ITEMID = ST.ITEMID"
        strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
        strSql += " WHERE ST.TAGSNO IN (select TAGSNO FROM TRANS_STATUS WHERE STOCKMODE = 'N' AND STATUS ='U' and MOVETYPE = 'O')"
        strSql += vbCrLf + " ) AS AXX"

        strSql += vbCrLf + " UPDATE SV  SET SV.ISSSNO = T.SNO"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW AS SV"
        strSql += vbCrLf + " INNER JOIN TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW AS T ON T.COMPANYID = SV.TCOMPANYID"
        strSql += vbCrLf + " AND T.CATCODE = SV.CATCODE "
        strSql += vbCrLf + " "
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " CONVERT(VARCHAR(15),NULL)SNO,ISSSNO,TRANTYPE,STNITEMID,SUM(STNPCS)STNPCS,SUM(STNWT)STNWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0) AS STNRATE,SUM(STNAMT) AS STNAMT"
        strSql += vbCrLf + " ,TCATCODE AS CATCODE,COSTID,COMPANYID,STONEUNIT"
        strSql += vbCrLf + " ,IDENTITY(INT,1,1)KEYNO,TMETALID AS STONEMODE"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW "
        strSql += vbCrLf + " GROUP BY ISSSNO,TRANTYPE,STNITEMID,TCATCODE,TMETALID,COSTID,COMPANYID,STONEUNIT"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE SET SNO = KEYNO"

        strSql += vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
        strSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
        strSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
        strSql += vbCrLf + " ,@CTLID = 'ISSSTONECODE'"
        strSql += vbCrLf + " ,@CHECK_TABLENAME = 'ISSSTONE'"
        strSql += vbCrLf + " ,@UPD_DBNAME = 'TEMPTABLEDB'"
        strSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP_" & systemId & "TRANTRANS_STONE'"
        cmd = New OleDbCommand(strSql, XCnAdmin, tran)
        cmd.ExecuteNonQuery()

        Dim DtTag As New DataTable
        strSql = "  SELECT * FROM TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW"
        cmd = New OleDbCommand(strSql, XCnAdmin, tran)
        da = New OleDbDataAdapter(cmd)
        da.Fill(DtTag)

        Dim DtTagStone As New DataTable
        strSql = "  SELECT * FROM TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE"
        cmd = New OleDbCommand(strSql, XCnAdmin, tran)
        da = New OleDbDataAdapter(cmd)
        da.Fill(DtTagStone)

        strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_NEW"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE"
        cmd = New OleDbCommand(strSql, XCnAdmin, tran)
        ': cmd.CommandTimeout = 1000 : 
        cmd.ExecuteNonQuery()



        Dim DtIssue As New DataTable
        Dim DtIssStone As New DataTable
        DtIssue = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, "ISSUE", XCnAdmin, tran)
        DtIssStone = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, "ISSSTONE", XCnAdmin, tran)
        Dim DtAcctran As New DataTable
        DtAcctran = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, "ACCTRAN", XCnAdmin, tran)

        Dim RoIns As DataRow = Nothing
        For Each Ro As DataRow In DtTag.Rows
            RoIns = DtIssue.NewRow
            For Each Col As DataColumn In DtTag.Columns
                If DtIssue.Columns.Contains(Col.ColumnName) = False Then Continue For
                RoIns.Item(Col.ColumnName) = Ro.Item(Col.ColumnName)
            Next
            DtIssue.Rows.Add(RoIns)
        Next
        For Each Ro As DataRow In DtTagStone.Rows
            RoIns = DtIssStone.NewRow
            For Each Col As DataColumn In DtTagStone.Columns
                If DtIssStone.Columns.Contains(Col.ColumnName) = False Then Continue For
                RoIns.Item(Col.ColumnName) = Ro.Item(Col.ColumnName)
            Next
            DtIssStone.Rows.Add(RoIns)
        Next
        Dim dtCategory As New DataTable
        dtCategory = DtIssue.DefaultView.ToTable(True, "CATCODE")

        For Each Ro As DataRow In DtIssue.Rows
            Ro.Item("TRANNO") = Tranno
            Ro.Item("TRANDATE") = BillDate
            Ro.Item("USERID") = userId
            Ro.Item("UPDATED") = GetServerDate()
            Ro.Item("APPVER") = VERSION
            Ro.Item("BATCHNO") = Batchno
            Ro.Item("REFNO") = RefNo
            Ro.Item("BAGNO") = Transistno.ToString
            Ro.Item("REFDATE") = RefDate
            Ro.Item("REMARK1") = txtRemark1.Text
            Ro.Item("REMARK2") = txtRemark2.Text
            Ro.Item("STNAMT") = Val(DtIssStone.Compute("SUM(STNAMT)", "ISSSNO='" & Ro.Item("SNO").ToString & "'").ToString)
            Dim objWmc As New CLS_MAXMINVALUES("I", Val(Ro.Item("GRSWT").ToString), Val(Ro.Item("ITEMID")), Val(Ro.Item("SUBITEMID")), ToCostId)
            If objWmc.pDtWmc.Rows.Count > 0 Then
                Ro.Item("WASTPER") = Val(objWmc.pDtWmc.Rows(0).Item("MAXWASTPER").ToString)
                Ro.Item("MCGRM") = Val(objWmc.pDtWmc.Rows(0).Item("MAXMCGRM").ToString)
            End If
            strSql = " SELECT SALESTAX FROM " & cnAdminDb & "..CATEGORY"
            strSql += " WHERE CATCODE = '" & Ro.Item("CATCODE").ToString & "'"
            Dim Vatper As Decimal = Val(GetSqlValue(strSql, XCnAdmin, tran).ToString)
            ''Ro.Item("TAX") = CalcRoundoffAmt(Val(Ro.Item("AMOUNT").ToString * Vatper) / 100, objSoftKeys.RoundOff_Vat)
        Next
        DtIssue.AcceptChanges()
        For Each Ro As DataRow In DtIssStone.Rows
            Ro.Item("TRANNO") = Tranno
            Ro.Item("TRANDATE") = BillDate
            Ro.Item("APPVER") = VERSION
            Ro.Item("BATCHNO") = Batchno
            Ro.Item("STONEMODE") = ""
        Next
        DtIssStone.AcceptChanges()
        For Each roCategory As DataRow In dtCategory.Rows
            strSql = " SELECT SALESID FROM " & cnAdminDb & "..CATEGORY"
            strSql += " WHERE CATCODE = '" & roCategory.Item("CATCODE").ToString & "'"
            Dim accode As String = GetSqlValue(strSql, XCnAdmin, tran)
            strSql = " SELECT SALESTAX FROM " & cnAdminDb & "..CATEGORY"
            strSql += " WHERE CATCODE = '" & roCategory.Item("CATCODE").ToString & "'"
            Dim Vatper As Decimal = Val(GetSqlValue(strSql, XCnAdmin, tran).ToString)
            Dim Amt As Double = Val(DtIssue.Compute("SUM(AMOUNT)", "CATCODE='" & roCategory.Item("CATCODE").ToString & "'").ToString)
            Dim Tax As Double = 0 ''CalcRoundoffAmt((Amt * Vatper) / 100, objSoftKeys.RoundOff_Vat)
            Dim Pcs As Integer = Val(DtIssue.Compute("SUM(PCS)", "CATCODE='" & roCategory.Item("CATCODE").ToString & "'").ToString)
            Dim Grswt As Decimal = Val(DtIssue.Compute("SUM(GRSWT)", "CATCODE='" & roCategory.Item("CATCODE").ToString & "'").ToString)
            Dim Netwt As Decimal = Val(DtIssue.Compute("SUM(NETWT)", "CATCODE='" & roCategory.Item("CATCODE").ToString & "'").ToString)
            Dim StnAmt As Double = Val(DtIssue.Compute("SUM(STNAMT)", "CATCODE='" & roCategory.Item("CATCODE").ToString & "'").ToString)
            If Amt <> 0 Then
                Dim Roacct As DataRow = Nothing
                Roacct = DtAcctran.NewRow
                With Roacct
                    .Item("SNO") = GetNewSnoNew(TranSnoType.ACCTRANCODE, XCnAdmin, tran)
                    .Item("BATCHNO") = Batchno
                    .Item("REFNO") = RefNo
                    .Item("USERID") = userId
                    .Item("UPDATED") = GetServerDate()
                    .Item("APPVER") = VERSION
                    .Item("COMPANYID") = strCompanyId
                    .Item("COSTID") = cnCostId
                    .Item("FROMFLAG") = "A"
                    .Item("TRANNO") = Tranno
                    .Item("TRANDATE") = BillDate
                    .Item("ACCODE") = Tocostacid
                    .Item("TRANMODE") = "D"
                    .Item("PAYMODE") = "DU"
                    .Item("CONTRA") = accode
                    .Item("PCS") = 0
                    .Item("GRSWT") = 0
                    .Item("NETWT") = 0
                    .Item("AMOUNT") = Amt + Tax
                    .Item("REMARK1") = txtRemark1.Text
                    .Item("REMARK2") = txtRemark2.Text
                End With
                DtAcctran.Rows.Add(Roacct)
                DtAcctran.AcceptChanges()
                Roacct = DtAcctran.NewRow
                With Roacct
                    .Item("SNO") = GetNewSnoNew(TranSnoType.ACCTRANCODE, XCnAdmin, tran)
                    .Item("BATCHNO") = Batchno
                    .Item("REFNO") = RefNo
                    .Item("USERID") = userId
                    .Item("UPDATED") = GetServerDate()
                    .Item("APPVER") = VERSION
                    .Item("COMPANYID") = strCompanyId
                    .Item("COSTID") = cnCostId
                    .Item("FROMFLAG") = "A"
                    .Item("TRANNO") = Tranno
                    .Item("TRANDATE") = BillDate
                    .Item("ACCODE") = accode
                    .Item("TRANMODE") = "C"
                    .Item("PAYMODE") = "SA"
                    .Item("CONTRA") = Tocostacid
                    .Item("PCS") = Pcs
                    .Item("GRSWT") = Grswt
                    .Item("NETWT") = Netwt
                    .Item("AMOUNT") = Amt
                    .Item("REMARK1") = txtRemark1.Text
                    .Item("REMARK2") = txtRemark2.Text
                End With
                DtAcctran.Rows.Add(Roacct)
                DtAcctran.AcceptChanges()
                Roacct = DtAcctran.NewRow
                With Roacct
                    .Item("SNO") = GetNewSnoNew(TranSnoType.ACCTRANCODE, XCnAdmin, tran)
                    .Item("BATCHNO") = Batchno
                    .Item("REFNO") = RefNo
                    .Item("USERID") = userId
                    .Item("UPDATED") = GetServerDate()
                    .Item("APPVER") = VERSION
                    .Item("COMPANYID") = strCompanyId
                    .Item("COSTID") = cnCostId
                    .Item("FROMFLAG") = "A"
                    .Item("TRANNO") = Tranno
                    .Item("TRANDATE") = BillDate
                    .Item("ACCODE") = accode
                    .Item("TRANMODE") = "C"
                    .Item("PAYMODE") = "SV"
                    .Item("CONTRA") = Tocostacid
                    .Item("PCS") = 0
                    .Item("GRSWT") = 0
                    .Item("NETWT") = 0
                    .Item("AMOUNT") = Tax
                    .Item("REMARK1") = txtRemark1.Text
                    .Item("REMARK2") = txtRemark2.Text
                End With
                DtAcctran.Rows.Add(Roacct)
                DtAcctran.AcceptChanges()
            End If
        Next
        InsertData(SyncMode.Transaction, DtIssue, XCnAdmin, tran, cnCostId)
        InsertData(SyncMode.Transaction, DtIssStone, XCnAdmin, tran, cnCostId)
        InsertData(SyncMode.Transaction, DtAcctran, XCnAdmin, tran, cnCostId)
        If DtIssue.Rows.Count > 0 Then
            InsertIntoPersonalinfo(Tocostacid, BillDate)
        End If
    End Sub


    Private Sub InsertIntoPersonalinfo(ByVal _accode As String, ByVal _billdate As Date)

        strSql = "SELECT TOP 1 * FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE ='" & _accode & "'"
        Dim drrachead As DataRow = GetSqlRow(strSql, XCnAdmin, tran)
        If drrachead Is Nothing Then Exit Sub
        Dim psno As String = GetPersonalInfoSno(tran)
        strSql = " INSERT INTO " & cnAdminDb & "..PERSONALINFO"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SNO,ACCODE,TRANDATE,TITLE"
        strSql += vbCrLf + " ,INITIAL,PNAME,DOORNO,ADDRESS1"
        strSql += vbCrLf + " ,ADDRESS2,ADDRESS3,AREA,CITY"
        strSql += vbCrLf + " ,STATE,COUNTRY,PINCODE,PHONERES"
        strSql += vbCrLf + " ,MOBILE,EMAIL,FAX,APPVER"
        strSql += vbCrLf + " ,PREVILEGEID,COMPANYID,COSTID,PAN"
        strSql += vbCrLf + " ,GSTNO,STATEID"
        strSql += vbCrLf + " )"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & psno & "'" ''SNO
        strSql += " ,'" & _accode & "'" 'ACCODE
        strSql += " ,'" & _billdate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
        strSql += " ,'" & drrachead("TITLE").ToString & "'" 'TITLE
        strSql += " ,'" & drrachead("INITIAL").ToString & "'" 'INITIAL
        strSql += " ,'" & drrachead("ACNAME").ToString & "'" 'PNAME
        strSql += " ,'" & drrachead("DOORNO").ToString & "'" 'DOORNO
        strSql += " ,'" & drrachead("ADDRESS1").ToString & "'" 'ADDRESS1
        strSql += " ,'" & drrachead("ADDRESS2").ToString & "'" 'ADDRESS2
        strSql += " ,'" & drrachead("ADDRESS3").ToString & "'" 'ADDRESS3
        strSql += " ,'" & drrachead("AREA").ToString & "'" 'AREA
        strSql += " ,'" & drrachead("CITY").ToString & "'" 'CITY
        strSql += " ,'" & drrachead("STATE").ToString & "'" 'STATE
        strSql += " ,'" & drrachead("COUNTRY").ToString & "'" 'COUNTRY
        strSql += " ,'" & drrachead("PINCODE").ToString & "'" 'PINCODE
        strSql += " ,'" & drrachead("PHONENO").ToString & "'" 'PHONERES
        strSql += " ,'" & drrachead("MOBILE").ToString & "'" 'MOBILE
        strSql += " ,'" & drrachead("EMAILID").ToString & "'" 'EMAIL
        strSql += " ,'" & drrachead("FAX").ToString & "'" 'Fax
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,'" & drrachead("PREVILEGEID").ToString & "'" 'PREVILEGEID
        strSql += " ,'" & cnCompanyId & "'" 'COMPANYID
        strSql += " ,'" & cnCostId & "'" 'COSTID
        strSql += " ,'" & drrachead("PAN").ToString & "'" 'PAN
        strSql += " ,'" & drrachead("GSTNO").ToString & "'" 'GSTNO
        strSql += " ,'" & drrachead("STATEID").ToString & "'" 'STATEID
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, XCnAdmin, tran, cnCostId)

        strSql = " INSERT INTO " & cnAdminDb & "..CUSTOMERINFO"
        strSql += " (BATCHNO,PSNO,COSTID,PAN)VALUES"
        strSql += " ('" & Batchno & "','" & psno & "','" & cnCostId & "','" & drrachead("PAN").ToString & "')"
        ExecQuery(SyncMode.Transaction, strSql, XCnAdmin, tran, cnCostId)
    End Sub

    Private Function GetPersonalInfoSno(ByVal tran1 As OleDbTransaction) As String
GETNSNO:
        Dim tSno As Integer = 0
        Dim strSql As String
        Dim cmd As OleDbCommand
        strSql = " SELECT CTLTEXT AS TAGSNO FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PERSONALINFOCODE'"
        tSno = Val(GetSqlValue(strSql, XCnAdmin, tran).ToString)
        ''UPDATING 
        ''TAGNO INTO SOFTCONTROL
        strSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT = '" & tSno + 1 & "' "
        strSql += vbCrLf + " WHERE CTLID = 'PERSONALINFOCODE' AND CONVERT(INT,CTLTEXT) = " & tSno & ""
        cmd = New OleDbCommand(strSql, XCnAdmin, tran1)
        If cmd.ExecuteNonQuery() = 0 Then
            GoTo GETNSNO
        End If
        strSql = " SELECT SNO FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = '" & GetCostId(cnCostId) & (tSno + 1).ToString & "'"
        If GetSqlValue(strSql, XCnAdmin, tran).ToString <> "" Then
            GoTo GETNSNO
        End If
        Return GetCostId(cnCostId) & (tSno + 1).ToString
    End Function

    '    Private Sub btnTransfer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTransfer.Click
    '        If objGPack.Validator_Check(Me) Then
    '            Exit Sub
    '        End If
    '        If Not gridView.RowCount > 0 Then
    '            MsgBox("Record not found", MsgBoxStyle.Information)
    '            Exit Sub
    '        End If
    '        Dim toCostId As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'")

    '        If toCostId = "" Then
    '            MsgBox("Cost centre should not empty", MsgBoxStyle.Information)
    '            Exit Sub
    '        End If
    '        If txtRemark1.Text.Trim = "" Then
    '            MsgBox("Remark1 should not empty", MsgBoxStyle.Information)
    '            txtRemark1.Focus()
    '            Exit Sub
    '        End If
    '        If txtRemark2.Text = "" Then
    '            MsgBox("Remark2 should not empty", MsgBoxStyle.Information)
    '            txtRemark2.Focus()
    '            Exit Sub
    '        End If
    '        If AUTOBOOKTRAN And Val(objGPack.GetSqlValue("SELECT 1 FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & toCostId & "')").ToString) <> 1 Then MsgBox("AC Code Not found in master") : Exit Sub
    '        If MessageBox.Show("Sure, Do you want to transfer the loaded Tags?", "Transfer Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
    '            Exit Sub
    '        End If
    '        Dim frmCostId As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbTagsCostName_MAN.Text & "'")
    '        Dim syncdb As String = cnAdminDb
    '        Dim uprefix As String = Mid(cnAdminDb, 1, 3)
    '        If GetAdmindbSoftValue("SYNC-SEPDATADB", "N", tran) = "Y" Then
    '            Dim usuffix As String = "UTILDB"
    '            If DbChecker(uprefix + usuffix) <> 0 Then syncdb = uprefix + usuffix
    '        End If

    '        Dim obj As TrasitIssRec
    '        Dim RefNo As String
    '        Dim TransSno As String = ""
    '        Dim NTransSno As String = ""
    '        BillDate = GetEntryDate(GetServerDate)
    '        Try

    '            strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BILLNO')> 0"
    '            strSql += " DROP TABLE TEMP" & systemId & "BILLNO"
    '            cmd = New OleDbCommand(strSql, cn)
    '            cmd.ExecuteNonQuery()

    '            Dim billcontrolid As String = "GEN-SM-INTISS"
    '            strSql = "SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL"
    '            strSql += " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
    '            If strBCostid <> Nothing Then strSql += " AND ISNULL(COSTID,'') ='" & strBCostid & "'"
    '            If UCase(objGPack.GetSqlValue(strSql)) <> "Y" Then
    '                billcontrolid = "GEN-STKREFNO"
    '            End If

    '            strSql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='RECNO_AS_STKTRANNO' "
    '            If UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", "N")) = "Y" Then
    '                billcontrolid = "GEN-SM-ISS"
    '            End If

    '            Dim NEWBILLNO As Integer
    '            strSql = "SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = '" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
    '            If strBCostid <> Nothing Then strSql += " AND ISNULL(COSTID,'') ='" & strBCostid & "'"
    '            NEWBILLNO = Val(objGPack.GetSqlValue(strSql)) + 1

    'GenerateNewBillNo:
    '            RefNo = cnCostId & "T" & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString & NEWBILLNO.ToString
    '            strSql = "SELECT TOP 1 SNO FROM " & cnAdminDb & "..ITEMTAG WHERE CONVERT(VARCHAR,REFNO) = '" & RefNo & "'"
    '            strSql += vbCrLf + " UNION ALL"
    '            strSql += vbCrLf + " SELECT TOP 1 SNO FROM " & cnAdminDb & "..CITEMTAG WHERE CONVERT(VARCHAR,REFNO) = '" & RefNo & "'"
    '            strSql += vbCrLf + " UNION ALL"
    '            strSql += vbCrLf + " SELECT TOP 1 SNO FROM " & cnAdminDb & "..ITEMNONTAG WHERE CONVERT(VARCHAR,REFNO) = '" & RefNo & "'"
    '            If AUTOBOOKTRAN Then
    '                strSql += vbCrLf + " UNION ALL"
    '                strSql += vbCrLf + " SELECT TOP 1 SNO FROM " & cnStockDb & "..ISSUE WHERE TRANTYPE = 'IIN' AND COSTID = '" & cnCostId & "' AND TRANNO = " & NEWBILLNO
    '            End If
    '            If objGPack.GetSqlValue(strSql, , "-1", Nothing) <> "-1" Then
    '                NEWBILLNO = NEWBILLNO + 1
    '                GoTo GenerateNewBillNo
    '            End If

    '            strSql = "UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & NEWBILLNO.ToString & "'"
    '            strSql += " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
    '            If strBCostid <> Nothing Then strSql += " AND ISNULL(COSTID,'') ='" & strBCostid & "'"
    '            cmd = New OleDbCommand(strSql, cn, Nothing)
    '            If cmd.ExecuteNonQuery() = 0 Then
    '                If strBCostid <> Nothing Then MsgBox("No bill control for this cost id " & strBCostid) : Exit Sub
    '                GoTo GenerateNewBillNo
    '            End If


    '            Dim billcontrolidX As String = "GEN-TRANSISTNO"
    '            strSql = "SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL"
    '            strSql += " WHERE CTLID ='" & billcontrolidX & "' AND COMPANYID = '" & strCompanyId & "'"
    '            If UCase(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, strSql, , , tran)) = "Y" Then
    '                strSql = "SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = '" & billcontrolidX & "' AND COMPANYID = '" & strCompanyId & "'"
    '                TransistNo = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, strSql, , , tran))
    'GenerateNewBillNoX:
    '                strSql = " SELECT TOP 1 SNO FROM " & cnStockDb & "..ISSUE WHERE TRANTYPE = 'IIN' AND BAGNO = '" & TransistNo.ToString & "'"
    '                If BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, strSql, , "-1", tran) <> "-1" Then
    '                    TransistNo = TransistNo + 1
    '                    GoTo GenerateNewBillNoX
    '                End If
    '                strSql = "UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & TransistNo.ToString & "'"
    '                strSql += " WHERE CTLID ='" & billcontrolidX & "' AND COMPANYID = '" & strCompanyId & "'"
    '                cmd = New OleDbCommand(strSql, XCnAdmin, tran)
    '                If cmd.ExecuteNonQuery() = 0 Then
    '                    GoTo GenerateNewBillNoX
    '                End If
    '            End If
    '            Dim Toaccountid As String = ""
    '            If AUTOBOOKTRAN Then Toaccountid = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME ='" & CmbAcname.Text & "'")

    '            tran = Nothing
    '            tran = XCnAdmin.BeginTransaction
    '            Isbulkupdate = False
    '            Dim xitemid As Integer
    '            Dim xsubitemid As Integer
    '            If AUTOBOOKTRAN Then Batchno = GetNewBatchnoNew(cnCostId, BillDate, XCnAdmin, tran)
    '            Dim mxrate As Decimal = 0

    '            For Each ro As DataRow In dtGridView.Rows

    '                Dim dSnoNullCheck As String = Convert.ToString(ro.Item("SNO"))
    '                If dSnoNullCheck <> "" Then
    '                    For Each row As DataGridViewRow In gridView.Rows()
    '                        Dim gSno As String = Convert.ToString(row.Cells("SNO").Value)
    '                        Dim dSno As String = Convert.ToString(ro.Item("SNO"))
    '                        If String.Equals(ro.Item("SNO"), gSno) Then
    '                            Dim isSelected As Boolean = Convert.ToBoolean(row.Cells("checkBoxColumn").Value)
    '                            If isSelected Then
    '                                mxrate = 0
    '                                Dim mamount As Decimal = 0
    '                                If ro.Item("STOCKMODE").ToString = "N" Then
    '                                    xitemid = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & ro.Item("Item").ToString & "'", , , tran).ToString)
    '                                    xsubitemid = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & ro.Item("SubItem").ToString & "' AND ITEMID = " & xitemid, , , tran).ToString)
    '                                    Dim xitemctrid As Integer = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & ro.Item("COUNTER").ToString & "'", , , tran).ToString)
    '                                    mamount = 0
    '                                    If AUTOBOOKTRAN Then
    '                                        Dim mitemrow As DataRow = GetSqlRow("SELECT CATCODE,METALID,CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID= " & xitemid & "", XCnAdmin, tran)
    '                                        mxrate = Val(GetRate(BillDate, mitemrow(0).ToString, ))
    '                                        If AUTOBOOKVALUEG_PER <> 0 And mitemrow(1).ToString = "G" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUEG_PER / 100))
    '                                        If AUTOBOOKVALUES_PER <> 0 And mitemrow(1).ToString = "S" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUES_PER / 100))
    '                                        If AUTOBOOKVALUEP_PER <> 0 And mitemrow(1).ToString = "P" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUEP_PER / 100))
    '                                        If mitemrow(2).ToString = "W" Or mitemrow(2).ToString = "B" Then
    '                                            If AUTOBOOKVALUED_PER <> 0 And mitemrow(1).ToString = "D" Then mxrate = Val(ro.Item("Rate").ToString) : mxrate = mxrate + (mxrate * (AUTOBOOKVALUED_PER / 100))
    '                                            If AUTOBOOKVALUET_PER <> 0 And mitemrow(1).ToString = "T" Then mxrate = Val(ro.Item("Rate").ToString) : mxrate = mxrate + (mxrate * (AUTOBOOKVALUET_PER / 100))
    '                                        End If
    '                                        If mitemrow(2).ToString = "W" Or mitemrow(2).ToString = "B" Then mamount = mxrate * Val(ro.Item("GRSWT").ToString)
    '                                        If mitemrow(2).ToString = "F" Or mitemrow(2).ToString = "R" Then mamount = Val(ro.Item("SALVALUE").ToString)
    '                                        Dim Stnamount As Decimal = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID = A.STNITEMID WHERE A.TAGSNO = '" & ro.Item("SNO") & "' AND I.METALID='T' ", , 0, tran).ToString)
    '                                        Dim Diaamount As Decimal = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID = A.STNITEMID WHERE A.TAGSNO = '" & ro.Item("SNO") & "' AND I.METALID='D' ", , 0, tran).ToString)
    '                                        If AUTOBOOKVALUET_PER <> 0 Then Stnamount = Stnamount + (Stnamount * (AUTOBOOKVALUET_PER / 100))
    '                                        If AUTOBOOKVALUED_PER <> 0 Then Diaamount = Diaamount + (Diaamount * (AUTOBOOKVALUED_PER / 100))
    '                                        mamount = mamount + Stnamount + Diaamount
    '                                        mamount = CalcRoundoffAmt(mamount, "H")
    '                                        strSql = " UPDATE " & cnAdminDb & "..ITEMNONTAG SET POSTED ='T',TRFVALUE= " & mamount & " WHERE SNO ='" & ro.Item("SNO").ToString & "'"
    '                                        cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
    '                                        If AUTOBOOKVALUEENABLE = "Y" And mamount <> 0 Then
    '                                            strSql = " UPDATE A SET A.TRFVALUE = A.TRFVALUE+ISNULL(A.STNAMT,0)+(ISNULL(A.STNAMT,0)*" & (AUTOBOOKVALUED_PER / 100) & ") FROM " & cnAdminDb & "..ITEMNONTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON A.STNITEMID = I.ITEMID WHERE A.TAGSNO ='" & ro.Item("SNO").ToString & "' AND I.METALID = 'D'"
    '                                            cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
    '                                            strSql = " UPDATE A SET A.TRFVALUE = A.TRFVALUE+ISNULL(A.STNAMT,0)+(ISNULL(A.STNAMT,0)*" & (AUTOBOOKVALUET_PER / 100) & ") FROM " & cnAdminDb & "..ITEMNONTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON A.STNITEMID = I.ITEMID WHERE A.TAGSNO ='" & ro.Item("SNO").ToString & "' AND I.METALID = 'T'"
    '                                            cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
    '                                        End If
    '                                    End If
    '                                    'Dim Sno As String = GetNewSno(TranSnoType.ITEMNONTAGCODE, tran, "GET_ADMINSNO_TRAN")
    '                                    Dim Sno As String = GetNewSnoNew(TranSnoType.ITEMNONTAGCODE, XCnAdmin, tran, "GET_ADMINSNO_TRAN")
    '                                    NTransSno += "'" & Sno.ToString & "',"
    '                                    Dim mpcs As Integer = Val(ro.Item("PCS").ToString)
    '                                    Dim mGwt As Double = Val(ro.Item("GRSWT").ToString)
    '                                    Dim mNwt As Double = Val(ro.Item("NETWT").ToString)

    '                                    If ro.Item("METALID").ToString = "T" Then mpcs += Val(ro.Item("STNPCS").ToString) + Val(ro.Item("PSTNPCS").ToString) : mGwt += Val(ro.Item("STNWT").ToString) + Val(ro.Item("PSTNWT").ToString) : mNwt += Val(ro.Item("STNWT").ToString) + Val(ro.Item("PSTNWT").ToString)
    '                                    If ro.Item("METALID").ToString = "D" Then mpcs += Val(ro.Item("DIAPCS").ToString) : mGwt += Val(ro.Item("DIAWT").ToString) : mNwt += Val(ro.Item("DIAWT").ToString)

    '                                    InsertIntoNonTag(frmCostId, toCostId, xitemid, xsubitemid, Val(ro.Item("DESIGNERID").ToString), mpcs, mGwt, mNwt, RefNo, NEWBILLNO, ro.Item("TAGNO").ToString, xitemctrid, mamount, Sno, ro.Item("SNO").ToString)

    '                                    strSql = "INSERT INTO TRANS_STATUS (STOCKMODE,TAGSNO,STATUS,MOVETYPE) SELECT 'N','" & Sno.ToString & "','U','O'"
    '                                    cmd = New OleDbCommand(strSql, XCnAdmin, tran)
    '                                    cmd.ExecuteNonQuery()
    '                                Else

    '                                    If ro.Item("SNO").ToString = "" Then Continue For
    '                                    If AUTOBOOKTRAN Then
    '                                        Dim mitemrow As DataRow = GetSqlRow("SELECT CATCODE,METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & ro.Item("Item").ToString & "'", XCnAdmin, tran)
    '                                        mxrate = Val(GetRate(BillDate, mitemrow(0).ToString, ))
    '                                        If AUTOBOOKVALUEG_PER <> 0 And mitemrow(1).ToString = "G" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUEG_PER / 100))
    '                                        If AUTOBOOKVALUES_PER <> 0 And mitemrow(1).ToString = "S" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUES_PER / 100))
    '                                        If AUTOBOOKVALUEP_PER <> 0 And mitemrow(1).ToString = "P" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUEP_PER / 100))
    '                                        'If mitemrow(1).ToString = "D" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUED_PER / 100))
    '                                        'If mitemrow(1).ToString = "T" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUET_PER / 100))
    '                                        If ro.Item("SALEMODE").ToString = "W" Or ro.Item("SALEMODE").ToString = "B" Then
    '                                            If AUTOBOOKVALUED_PER <> 0 And mitemrow(1).ToString = "D" Then mxrate = Val(ro.Item("Rate").ToString) : mxrate = mxrate + (mxrate * (AUTOBOOKVALUED_PER / 100))
    '                                            If AUTOBOOKVALUET_PER <> 0 And mitemrow(1).ToString = "T" Then mxrate = Val(ro.Item("Rate").ToString) : mxrate = mxrate + (mxrate * (AUTOBOOKVALUET_PER / 100))
    '                                        End If

    '                                        If ro.Item("SALEMODE").ToString = "W" Or ro.Item("SALEMODE").ToString = "B" Then mamount = mxrate * Val(ro.Item("GRSWT").ToString)
    '                                        If ro.Item("SALEMODE").ToString = "F" Or ro.Item("SALEMODE").ToString = "R" Or ro.Item("SALEMODE").ToString = "M" Then mamount = Val(ro.Item("SALVALUE").ToString)

    '                                        Dim Stnamount As Decimal = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID = A.STNITEMID WHERE A.TAGSNO = '" & ro.Item("SNO") & "' AND I.METALID='T' ", , 0, tran).ToString)
    '                                        Dim Diaamount As Decimal = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID = A.STNITEMID WHERE A.TAGSNO = '" & ro.Item("SNO") & "' AND I.METALID='D' ", , 0, tran).ToString)
    '                                        If AUTOBOOKVALUET_PER <> 0 Then Stnamount = Stnamount + (Stnamount * (AUTOBOOKVALUET_PER / 100))
    '                                        If AUTOBOOKVALUED_PER <> 0 Then Diaamount = Diaamount + (Diaamount * (AUTOBOOKVALUED_PER / 100))
    '                                        mamount = mamount + Stnamount + Diaamount
    '                                        mamount = CalcRoundoffAmt(mamount, "H")
    '                                    End If

    '                                    If ReplSubItemid Then
    '                                        xitemid = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & ro.Item("Item").ToString & "'", , , tran).ToString)
    '                                        xsubitemid = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & ro.Item("SubItem").ToString & "' AND ITEMID = " & xitemid, , , tran).ToString)
    '                                        strSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET SUBITEMID= " & xsubitemid & " WHERE SNO ='" & ro.Item("SNO").ToString & "'"
    '                                        cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
    '                                    End If
    '                                    strSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET TRANSFERDATE= '" & BillDate.ToString & "' WHERE SNO ='" & ro.Item("SNO").ToString & "'"
    '                                    cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
    '                                    If AUTOBOOKVALUEENABLE = "Y" And mamount <> 0 Then
    '                                        strSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET TRFVALUE= " & mamount & " WHERE SNO ='" & ro.Item("SNO").ToString & "'"
    '                                        cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
    '                                        strSql = " UPDATE A SET A.TRFVALUE = ISNULL(A.STNAMT,0)+(ISNULL(A.STNAMT,0)*" & (AUTOBOOKVALUED_PER / 100) & ") FROM " & cnAdminDb & "..ITEMTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON A.STNITEMID = I.ITEMID WHERE A.TAGSNO ='" & ro.Item("SNO").ToString & "' AND I.METALID = 'D'"
    '                                        cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
    '                                        strSql = " UPDATE A SET A.TRFVALUE = ISNULL(A.STNAMT,0)+(ISNULL(A.STNAMT,0)*" & (AUTOBOOKVALUET_PER / 100) & ") FROM " & cnAdminDb & "..ITEMTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON A.STNITEMID = I.ITEMID WHERE A.TAGSNO ='" & ro.Item("SNO").ToString & "' AND I.METALID = 'T'"
    '                                        cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
    '                                    End If

    '                                    If Not Isbulkupdate Then
    '                                        obj = New TrasitIssRec(cnCostId, toCostId, "I", BillDate, ro.Item("SNO").ToString, XCnAdmin, tran, RefNo)
    '                                        If Not obj.InsertTagIssue(True) Then
    '                                            Continue For
    '                                        End If
    '                                        TransSno += "'" & ro.Item("SNO").ToString & "',"
    '                                        strSql = "INSERT INTO TRANS_STATUS (STOCKMODE,TAGSNO,STATUS,MOVETYPE) SELECT 'T','" & ro.Item("Sno").ToString & "','U','O'"
    '                                        cmd = New OleDbCommand(strSql, XCnAdmin, tran)
    '                                        cmd.ExecuteNonQuery()

    '                                    End If
    '                                    TransSno += "'" & ro.Item("SNO").ToString & "',"
    '                                    If IS_IMAGE_TRF = True Then
    '                                        If ro.Item("PCTFILE").ToString <> "" And defalutDestination <> "" Then
    '                                            If IO.File.Exists(defalutDestination & ro.Item("PCTFILE").ToString) Then
    '                                                strSql = " INSERT INTO " & syncdb & "..SENDSYNC(FROMID,TOID"
    '                                                strSql += ",TAGIMAGE,TAGIMAGENAME,IMAGE_CTRLID"
    '                                                strSql += " )"
    '                                                strSql += " VALUES"
    '                                                strSql += " ('" & cnCostId & "','" & toCostId & "',?,?,'PICPATH')"
    '                                                cmd = New OleDbCommand(strSql, XCnAdmin, tran)
    '                                                Dim fileStr As New IO.FileStream(defalutDestination & ro.Item("PCTFILE").ToString, IO.FileMode.Open, IO.FileAccess.Read)
    '                                                Dim reader As New IO.BinaryReader(fileStr)
    '                                                Dim result As Byte() = reader.ReadBytes(CType(fileStr.Length, Integer))
    '                                                fileStr.Read(result, 0, result.Length)
    '                                                fileStr.Close()
    '                                                cmd.Parameters.AddWithValue("@TAGIMAGE", result)
    '                                                Dim fInfo As New IO.FileInfo(defalutDestination & ro.Item("PCTFILE").ToString)
    '                                                cmd.Parameters.AddWithValue("@TAGIMAGENAME", fInfo.Name)
    '                                                cmd.ExecuteNonQuery()
    '                                            End If
    '                                        End If
    '                                    End If

    '                                    strSql = "INSERT INTO " & cnStockDb & "..TRANSIT_AUDIT"
    '                                    strSql += " (FROMID,TOID,TRANDATE,TAGSNO,ISSREC,STOCKTYPE)"
    '                                    strSql += " SELECT '" & cnCostId & "','" & toCostId & "','" & BillDate.ToString("yyyy-MM-dd") & "','" & ro.Item("SNO").ToString & "','I','T'"
    '                                    cmd = New OleDbCommand(strSql, XCnAdmin, tran)
    '                                    cmd.ExecuteNonQuery()

    '                                    strSql = " EXEC " & cnAdminDb & "..SP_CCTRANSFER "
    '                                    strSql += vbCrLf + " @TAGSNO = '" & ro.Item("SNO").ToString & "',@COSTID = '" & cnCostId & "',@ISSDATE = '" & BillDate.ToString("yyyy-MM-dd") & "',@ISSTIME=NULL,@VERSION='" & GlobalVariables.VERSION & "',@USERID = " & GlobalVariables.userId
    '                                    cmd = New OleDbCommand(strSql, XCnAdmin, tran)
    '                                    cmd.ExecuteNonQuery()
    '                                End If
    '                            End If
    '                        End If
    '                    Next
    '                End If



    '                'mxrate = 0
    '                'Dim mamount As Decimal = 0
    '                'If ro.Item("STOCKMODE").ToString = "N" Then
    '                '    xitemid = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & ro.Item("Item").ToString & "'", , , tran).ToString)
    '                '    xsubitemid = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & ro.Item("SubItem").ToString & "' AND ITEMID = " & xitemid, , , tran).ToString)
    '                '    Dim xitemctrid As Integer = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & ro.Item("COUNTER").ToString & "'", , , tran).ToString)
    '                '    mamount = 0
    '                '    If AUTOBOOKTRAN Then
    '                '        Dim mitemrow As DataRow = GetSqlRow("SELECT CATCODE,METALID,CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID= " & xitemid & "", XCnAdmin, tran)
    '                '        mxrate = Val(GetRate(BillDate, mitemrow(0).ToString, ))
    '                '        If AUTOBOOKVALUEG_PER <> 0 And mitemrow(1).ToString = "G" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUEG_PER / 100))
    '                '        If AUTOBOOKVALUES_PER <> 0 And mitemrow(1).ToString = "S" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUES_PER / 100))
    '                '        If AUTOBOOKVALUEP_PER <> 0 And mitemrow(1).ToString = "P" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUEP_PER / 100))
    '                '        If mitemrow(2).ToString = "W" Or mitemrow(2).ToString = "B" Then
    '                '            If AUTOBOOKVALUED_PER <> 0 And mitemrow(1).ToString = "D" Then mxrate = Val(ro.Item("Rate").ToString) : mxrate = mxrate + (mxrate * (AUTOBOOKVALUED_PER / 100))
    '                '            If AUTOBOOKVALUET_PER <> 0 And mitemrow(1).ToString = "T" Then mxrate = Val(ro.Item("Rate").ToString) : mxrate = mxrate + (mxrate * (AUTOBOOKVALUET_PER / 100))
    '                '        End If
    '                '        If mitemrow(2).ToString = "W" Or mitemrow(2).ToString = "B" Then mamount = mxrate * Val(ro.Item("GRSWT").ToString)
    '                '        If mitemrow(2).ToString = "F" Or mitemrow(2).ToString = "R" Then mamount = Val(ro.Item("SALVALUE").ToString)
    '                '        Dim Stnamount As Decimal = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID = A.STNITEMID WHERE A.TAGSNO = '" & ro.Item("SNO") & "' AND I.METALID='T' ", , 0, tran).ToString)
    '                '        Dim Diaamount As Decimal = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID = A.STNITEMID WHERE A.TAGSNO = '" & ro.Item("SNO") & "' AND I.METALID='D' ", , 0, tran).ToString)
    '                '        If AUTOBOOKVALUET_PER <> 0 Then Stnamount = Stnamount + (Stnamount * (AUTOBOOKVALUET_PER / 100))
    '                '        If AUTOBOOKVALUED_PER <> 0 Then Diaamount = Diaamount + (Diaamount * (AUTOBOOKVALUED_PER / 100))
    '                '        mamount = mamount + Stnamount + Diaamount
    '                '        mamount = CalcRoundoffAmt(mamount, "H")
    '                '        strSql = " UPDATE " & cnAdminDb & "..ITEMNONTAG SET POSTED ='T',TRFVALUE= " & mamount & " WHERE SNO ='" & ro.Item("SNO").ToString & "'"
    '                '        cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
    '                '        If AUTOBOOKVALUEENABLE = "Y" And mamount <> 0 Then
    '                '            strSql = " UPDATE A SET A.TRFVALUE = A.TRFVALUE+ISNULL(A.STNAMT,0)+(ISNULL(A.STNAMT,0)*" & (AUTOBOOKVALUED_PER / 100) & ") FROM " & cnAdminDb & "..ITEMNONTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON A.STNITEMID = I.ITEMID WHERE A.TAGSNO ='" & ro.Item("SNO").ToString & "' AND I.METALID = 'D'"
    '                '            cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
    '                '            strSql = " UPDATE A SET A.TRFVALUE = A.TRFVALUE+ISNULL(A.STNAMT,0)+(ISNULL(A.STNAMT,0)*" & (AUTOBOOKVALUET_PER / 100) & ") FROM " & cnAdminDb & "..ITEMNONTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON A.STNITEMID = I.ITEMID WHERE A.TAGSNO ='" & ro.Item("SNO").ToString & "' AND I.METALID = 'T'"
    '                '            cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
    '                '        End If
    '                '    End If
    '                '    'Dim Sno As String = GetNewSno(TranSnoType.ITEMNONTAGCODE, tran, "GET_ADMINSNO_TRAN")
    '                '    Dim Sno As String = GetNewSnoNew(TranSnoType.ITEMNONTAGCODE, XCnAdmin, tran, "GET_ADMINSNO_TRAN")
    '                '    NTransSno += "'" & Sno.ToString & "',"
    '                '    Dim mpcs As Integer = Val(ro.Item("PCS").ToString)
    '                '    Dim mGwt As Double = Val(ro.Item("GRSWT").ToString)
    '                '    Dim mNwt As Double = Val(ro.Item("NETWT").ToString)

    '                '    If ro.Item("METALID").ToString = "T" Then mpcs += Val(ro.Item("STNPCS").ToString) + Val(ro.Item("PSTNPCS").ToString) : mGwt += Val(ro.Item("STNWT").ToString) + Val(ro.Item("PSTNWT").ToString) : mNwt += Val(ro.Item("STNWT").ToString) + Val(ro.Item("PSTNWT").ToString)
    '                '    If ro.Item("METALID").ToString = "D" Then mpcs += Val(ro.Item("DIAPCS").ToString) : mGwt += Val(ro.Item("DIAWT").ToString) : mNwt += Val(ro.Item("DIAWT").ToString)

    '                '    InsertIntoNonTag(frmCostId, toCostId, xitemid, xsubitemid, Val(ro.Item("DESIGNERID").ToString), mpcs, mGwt, mNwt, RefNo, NEWBILLNO, ro.Item("TAGNO").ToString, xitemctrid, mamount, Sno, ro.Item("SNO").ToString)

    '                '    strSql = "INSERT INTO TRANS_STATUS (STOCKMODE,TAGSNO,STATUS,MOVETYPE) SELECT 'N','" & Sno.ToString & "','U','O'"
    '                '    cmd = New OleDbCommand(strSql, XCnAdmin, tran)
    '                '    cmd.ExecuteNonQuery()
    '                'Else

    '                '    If ro.Item("SNO").ToString = "" Then Continue For
    '                '    If AUTOBOOKTRAN Then
    '                '        Dim mitemrow As DataRow = GetSqlRow("SELECT CATCODE,METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & ro.Item("Item").ToString & "'", XCnAdmin, tran)
    '                '        mxrate = Val(GetRate(BillDate, mitemrow(0).ToString, ))
    '                '        If AUTOBOOKVALUEG_PER <> 0 And mitemrow(1).ToString = "G" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUEG_PER / 100))
    '                '        If AUTOBOOKVALUES_PER <> 0 And mitemrow(1).ToString = "S" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUES_PER / 100))
    '                '        If AUTOBOOKVALUEP_PER <> 0 And mitemrow(1).ToString = "P" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUEP_PER / 100))
    '                '        'If mitemrow(1).ToString = "D" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUED_PER / 100))
    '                '        'If mitemrow(1).ToString = "T" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUET_PER / 100))
    '                '        If ro.Item("SALEMODE").ToString = "W" Or ro.Item("SALEMODE").ToString = "B" Then
    '                '            If AUTOBOOKVALUED_PER <> 0 And mitemrow(1).ToString = "D" Then mxrate = Val(ro.Item("Rate").ToString) : mxrate = mxrate + (mxrate * (AUTOBOOKVALUED_PER / 100))
    '                '            If AUTOBOOKVALUET_PER <> 0 And mitemrow(1).ToString = "T" Then mxrate = Val(ro.Item("Rate").ToString) : mxrate = mxrate + (mxrate * (AUTOBOOKVALUET_PER / 100))
    '                '        End If

    '                '        If ro.Item("SALEMODE").ToString = "W" Or ro.Item("SALEMODE").ToString = "B" Then mamount = mxrate * Val(ro.Item("GRSWT").ToString)
    '                '        If ro.Item("SALEMODE").ToString = "F" Or ro.Item("SALEMODE").ToString = "R" Or ro.Item("SALEMODE").ToString = "M" Then mamount = Val(ro.Item("SALVALUE").ToString)

    '                '        Dim Stnamount As Decimal = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID = A.STNITEMID WHERE A.TAGSNO = '" & ro.Item("SNO") & "' AND I.METALID='T' ", , 0, tran).ToString)
    '                '        Dim Diaamount As Decimal = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID = A.STNITEMID WHERE A.TAGSNO = '" & ro.Item("SNO") & "' AND I.METALID='D' ", , 0, tran).ToString)
    '                '        If AUTOBOOKVALUET_PER <> 0 Then Stnamount = Stnamount + (Stnamount * (AUTOBOOKVALUET_PER / 100))
    '                '        If AUTOBOOKVALUED_PER <> 0 Then Diaamount = Diaamount + (Diaamount * (AUTOBOOKVALUED_PER / 100))
    '                '        mamount = mamount + Stnamount + Diaamount
    '                '        mamount = CalcRoundoffAmt(mamount, "H")
    '                '    End If

    '                '    If ReplSubItemid Then
    '                '        xitemid = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & ro.Item("Item").ToString & "'", , , tran).ToString)
    '                '        xsubitemid = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & ro.Item("SubItem").ToString & "' AND ITEMID = " & xitemid, , , tran).ToString)
    '                '        strSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET SUBITEMID= " & xsubitemid & " WHERE SNO ='" & ro.Item("SNO").ToString & "'"
    '                '        cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
    '                '    End If
    '                '    strSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET TRANSFERDATE= '" & BillDate.ToString & "' WHERE SNO ='" & ro.Item("SNO").ToString & "'"
    '                '    cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
    '                '    If AUTOBOOKVALUEENABLE = "Y" And mamount <> 0 Then
    '                '        strSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET TRFVALUE= " & mamount & " WHERE SNO ='" & ro.Item("SNO").ToString & "'"
    '                '        cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
    '                '        strSql = " UPDATE A SET A.TRFVALUE = ISNULL(A.STNAMT,0)+(ISNULL(A.STNAMT,0)*" & (AUTOBOOKVALUED_PER / 100) & ") FROM " & cnAdminDb & "..ITEMTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON A.STNITEMID = I.ITEMID WHERE A.TAGSNO ='" & ro.Item("SNO").ToString & "' AND I.METALID = 'D'"
    '                '        cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
    '                '        strSql = " UPDATE A SET A.TRFVALUE = ISNULL(A.STNAMT,0)+(ISNULL(A.STNAMT,0)*" & (AUTOBOOKVALUET_PER / 100) & ") FROM " & cnAdminDb & "..ITEMTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON A.STNITEMID = I.ITEMID WHERE A.TAGSNO ='" & ro.Item("SNO").ToString & "' AND I.METALID = 'T'"
    '                '        cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
    '                '    End If

    '                '    If Not Isbulkupdate Then
    '                '        obj = New TrasitIssRec(cnCostId, toCostId, "I", BillDate, ro.Item("SNO").ToString, XCnAdmin, tran, RefNo)
    '                '        If Not obj.InsertTagIssue(True) Then
    '                '            Continue For
    '                '        End If
    '                '        TransSno += "'" & ro.Item("SNO").ToString & "',"
    '                '        strSql = "INSERT INTO TRANS_STATUS (STOCKMODE,TAGSNO,STATUS,MOVETYPE) SELECT 'T','" & ro.Item("Sno").ToString & "','U','O'"
    '                '        cmd = New OleDbCommand(strSql, XCnAdmin, tran)
    '                '        cmd.ExecuteNonQuery()

    '                '    End If
    '                '    TransSno += "'" & ro.Item("SNO").ToString & "',"
    '                '    If IS_IMAGE_TRF = True Then
    '                '        If ro.Item("PCTFILE").ToString <> "" And defalutDestination <> "" Then
    '                '            If IO.File.Exists(defalutDestination & ro.Item("PCTFILE").ToString) Then
    '                '                strSql = " INSERT INTO " & syncdb & "..SENDSYNC(FROMID,TOID"
    '                '                strSql += ",TAGIMAGE,TAGIMAGENAME,IMAGE_CTRLID"
    '                '                strSql += " )"
    '                '                strSql += " VALUES"
    '                '                strSql += " ('" & cnCostId & "','" & toCostId & "',?,?,'PICPATH')"
    '                '                cmd = New OleDbCommand(strSql, XCnAdmin, tran)
    '                '                Dim fileStr As New IO.FileStream(defalutDestination & ro.Item("PCTFILE").ToString, IO.FileMode.Open, IO.FileAccess.Read)
    '                '                Dim reader As New IO.BinaryReader(fileStr)
    '                '                Dim result As Byte() = reader.ReadBytes(CType(fileStr.Length, Integer))
    '                '                fileStr.Read(result, 0, result.Length)
    '                '                fileStr.Close()
    '                '                cmd.Parameters.AddWithValue("@TAGIMAGE", result)
    '                '                Dim fInfo As New IO.FileInfo(defalutDestination & ro.Item("PCTFILE").ToString)
    '                '                cmd.Parameters.AddWithValue("@TAGIMAGENAME", fInfo.Name)
    '                '                cmd.ExecuteNonQuery()
    '                '            End If
    '                '        End If
    '                '    End If

    '                '    strSql = "INSERT INTO " & cnStockDb & "..TRANSIT_AUDIT"
    '                '    strSql += " (FROMID,TOID,TRANDATE,TAGSNO,ISSREC,STOCKTYPE)"
    '                '    strSql += " SELECT '" & cnCostId & "','" & toCostId & "','" & BillDate.ToString("yyyy-MM-dd") & "','" & ro.Item("SNO").ToString & "','I','T'"
    '                '    cmd = New OleDbCommand(strSql, XCnAdmin, tran)
    '                '    cmd.ExecuteNonQuery()

    '                '    strSql = " EXEC " & cnAdminDb & "..SP_CCTRANSFER "
    '                '    strSql += vbCrLf + " @TAGSNO = '" & ro.Item("SNO").ToString & "',@COSTID = '" & cnCostId & "',@ISSDATE = '" & BillDate.ToString("yyyy-MM-dd") & "',@ISSTIME=NULL,@VERSION='" & GlobalVariables.VERSION & "',@USERID = " & GlobalVariables.userId
    '                '    cmd = New OleDbCommand(strSql, XCnAdmin, tran)
    '                '    cmd.ExecuteNonQuery()
    '                'End If
    '            Next
    '            If NTransSno <> "" Then NTransSno = Mid(NTransSno, 1, NTransSno.Length - 1)
    '            If TransSno <> "" Then TransSno = Mid(TransSno, 1, TransSno.Length - 1)
    '            'strSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET REFNO = '" & RefNo & "',REFDATE = '" & BillDate.ToString("yyyy-MM-dd") & "'"
    '            'strSql += " WHERE SNO IN (" & TransSno & ")"
    '            'cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
    '            'If AUTOBOOKTRAN Then
    '            '    CreateInternalTransfer(TransSno, toCostId, RefNo, BillDate, NEWBILLNO)
    '            'End If

    '            strSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET REFNO = '" & RefNo & "',REFDATE = '" & BillDate.ToString("yyyy-MM-dd") & "'"
    '            strSql += " WHERE SNO IN (select TAGSNO FROM TRANS_STATUS WHERE STOCKMODE = 'T' AND STATUS ='U' AND MOVETYPE = 'O')"
    '            cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
    '            strSql = " UPDATE " & cnAdminDb & "..ITEMNONTAG SET REFNO = '" & RefNo & "',REFDATE = '" & BillDate.ToString("yyyy-MM-dd") & "'"
    '            strSql += " WHERE SNO IN (select TAGSNO FROM TRANS_STATUS WHERE STOCKMODE = 'N' AND STATUS ='U' AND MOVETYPE = 'O')"
    '            cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
    '            If AUTOBOOKTRAN Then
    '                CreateInternalTransfernew(toCostId, RefNo, BillDate, NEWBILLNO, TransistNo, frmCostId, Toaccountid)
    '            End If
    '            GenerateSkuFile(XCnAdmin, tran, "", "", RefNo)
    '            strSql = " DELETE " & XSyncdb & "..TRANS_STATUS WHERE STATUS = 'U'  AND MOVETYPE = 'O'"
    '            cmd = New OleDbCommand(strSql, XCnAdmin, tran)
    '            cmd.ExecuteNonQuery()
    '            tran.Commit()
    '            tran = Nothing
    '            Dim pBatchno As String = Batchno
    '            Dim pBilldate As Date = BillDate
    '            btnNew_Click(Me, New EventArgs)
    '            MsgBox("Data Transfered,Please do the Syncronization." & vbCrLf & " Reference no is: " & RefNo)

    '            If GetAdmindbSoftValue("PRN_STKTRANSFER", "N") = "Y" Then
    '                PrintStockTransfer(RefNo, pBilldate)
    '            End If
    '            If AUTOBOOKTRAN Then
    '                If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
    '                    Dim prnmemsuffix As String = ""
    '                    If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
    '                    Dim memfile As String = "\BillPrint" + prnmemsuffix.Trim & ".mem"

    '                    Dim write As IO.StreamWriter
    '                    write = IO.File.CreateText(Application.StartupPath & memfile)
    '                    write.WriteLine(LSet("TYPE", 15) & ":SMI")
    '                    write.WriteLine(LSet("BATCHNO", 15) & ":" & pBatchno)
    '                    write.WriteLine(LSet("TRANDATE", 15) & ":" & pBilldate.ToString("yyyy-MM-dd"))
    '                    write.WriteLine(LSet("DUPLICATE", 15) & ":N")
    '                    write.Flush()
    '                    write.Close()
    '                    If EXE_WITH_PARAM = False Then
    '                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
    '                    Else
    '                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe", _
    '                        LSet("TYPE", 15) & ":SMI" & ";" & _
    '                        LSet("BATCHNO", 15) & ":" & pBatchno & ";" & _
    '                        LSet("TRANDATE", 15) & ":" & pBilldate.ToString("yyyy-MM-dd") & ";" & _
    '                        LSet("DUPLICATE", 15) & ":N")
    '                    End If
    '                Else
    '                    MsgBox("Billprint exe not found", MsgBoxStyle.Information)
    '                End If
    '            End If
    '        Catch ex As Exception
    '            If tran IsNot Nothing Then tran.Rollback()
    '            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
    '        End Try
    '    End Sub
    Private Sub btnTransfer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTransfer.Click
        Dim SelectedCheckBox As Boolean
        If objGPack.Validator_Check(Me) Then
            Exit Sub
        End If
        If Not gridView.RowCount > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        If ChkTagTran Then
            For Each row As DataGridViewRow In gridView.Rows()
                Dim isSelected As Boolean = Convert.ToBoolean(row.Cells("checkBoxColumn").Value)
                If isSelected Then
                    SelectedCheckBox = True
                    Exit For
                End If
            Next

            If SelectedCheckBox = True Then
            Else
                MsgBox("Please Select any Item...!", MsgBoxStyle.Information)
                Exit Sub
            End If
        End If
        Dim toCostId As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'")

        If toCostId = "" Then
            MsgBox("Cost centre should not empty", MsgBoxStyle.Information)
            Exit Sub
        End If
        If txtRemark1.Text.Trim = "" And IsRemarkRestrict Then
            MsgBox("Remark1 should not empty", MsgBoxStyle.Information)
            txtRemark1.Focus()
            Exit Sub
        End If
        If txtRemark2.Text = "" And IsRemarkRestrict Then
            MsgBox("Remark2 should not empty", MsgBoxStyle.Information)
            txtRemark2.Focus()
            Exit Sub
        End If

        If OTP_Check() = False Then Exit Sub
        If AUTOBOOKTRAN And Val(objGPack.GetSqlValue("SELECT 1 FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & toCostId & "')").ToString) <> 1 Then MsgBox("AC Code Not found in master") : Exit Sub
        If _isFrachisee And Val(objGPack.GetSqlValue("SELECT 1 FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & toCostId & "')").ToString) <> 1 Then MsgBox("AC Code Not found in master") : Exit Sub
        If MessageBox.Show("Sure, Do you want to transfer the loaded Tags?", "Transfer Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If
        Dim frmCostId As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbTagsCostName_MAN.Text & "'")
        Dim syncdb As String = cnAdminDb
        Dim uprefix As String = Mid(cnAdminDb, 1, 3)
        If GetAdmindbSoftValue("SYNC-SEPDATADB", "N", tran) = "Y" Then
            Dim usuffix As String = "UTILDB"
            If DbChecker(uprefix + usuffix) <> 0 Then syncdb = uprefix + usuffix
        End If

        Dim obj As TrasitIssRec
        Dim RefNo As String
        Dim TransSno As String = ""
        Dim NTransSno As String = ""
        BillDate = GetEntryDate(GetServerDate)
        Dim Toaccountid As String = ""
        If AUTOBOOKTRAN Or _isFrachisee Then Toaccountid = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME ='" & CmbAcname.Text & "'")
        Try
            tran = Nothing
            tran = XCnAdmin.BeginTransaction
            strSql = " IF (SELECT COUNT(*) FROM " & cnAdminDb & "..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BILLNO')> 0"
            strSql += " DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "BILLNO"
            cmd = New OleDbCommand(strSql, XCnAdmin, tran)
            cmd.ExecuteNonQuery()
            Dim NEWBILLNO As Integer
            Dim billcontrolid As String = ""

            billcontrolid = "GEN-SM-INTISS"
            strSql = "SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL"
            strSql += " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
            If strBCostid <> Nothing Then strSql += " AND ISNULL(COSTID,'') ='" & strBCostid & "'"
            If UCase(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, strSql,,, tran)) <> "Y" Then
                billcontrolid = "GEN-STKREFNO"
            End If

            strSql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='RECNO_AS_STKTRANNO' "
            If UCase(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, strSql, "CTLTEXT", "N", tran)) = "Y" Then
                billcontrolid = "GEN-SM-ISS"
            End If
            strSql = "SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = '" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
            If strBCostid <> Nothing Then strSql += " AND ISNULL(COSTID,'') ='" & strBCostid & "'"
            NEWBILLNO = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, strSql,,, tran)) + 1

GenerateNewBillNo:
            RefNo = cnCostId & "T" & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString & NEWBILLNO.ToString
            strSql = "SELECT TOP 1 SNO FROM " & cnAdminDb & "..ITEMTAG WHERE CONVERT(VARCHAR,REFNO) = '" & RefNo & "'"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT TOP 1 SNO FROM " & cnAdminDb & "..CITEMTAG WHERE CONVERT(VARCHAR,REFNO) = '" & RefNo & "'"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT TOP 1 SNO FROM " & cnAdminDb & "..ITEMNONTAG WHERE CONVERT(VARCHAR,REFNO) = '" & RefNo & "'"
            If AUTOBOOKTRAN Or _isFrachisee Then
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT TOP 1 SNO FROM " & cnStockDb & "..ISSUE WHERE TRANTYPE = 'IIN' AND COSTID = '" & cnCostId & "' AND TRANNO = " & NEWBILLNO
            End If
            If BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, strSql,, "-1", tran) <> "-1" Then
                NEWBILLNO = NEWBILLNO + 1
                GoTo GenerateNewBillNo
            End If

            If _isFrachisee = False Then
                strSql = "UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & NEWBILLNO.ToString & "'"
                strSql += " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
                If strBCostid <> Nothing Then strSql += " AND ISNULL(COSTID,'') ='" & strBCostid & "'"
                cmd = New OleDbCommand(strSql, XCnAdmin, tran)
                If cmd.ExecuteNonQuery() = 0 Then
                    If strBCostid <> Nothing Then If tran IsNot Nothing Then tran.Rollback() : tran = Nothing : MsgBox("No bill control for this cost id " & strBCostid) : Exit Sub
                    GoTo GenerateNewBillNo
                End If
            End If


            Dim billcontrolidX As String = "GEN-TRANSISTNO"
            strSql = "SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL"
            strSql += " WHERE CTLID ='" & billcontrolidX & "' AND COMPANYID = '" & strCompanyId & "'"
            If UCase(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, strSql, , , tran)) = "Y" Then
                strSql = "SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = '" & billcontrolidX & "' AND COMPANYID = '" & strCompanyId & "'"
                TransistNo = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, strSql, , , tran))
GenerateNewBillNoX:
                strSql = " SELECT TOP 1 SNO FROM " & cnStockDb & "..ISSUE WHERE TRANTYPE = 'IIN' AND BAGNO = '" & TransistNo.ToString & "'"
                If BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, strSql, , "-1", tran) <> "-1" Then
                    TransistNo = TransistNo + 1
                    GoTo GenerateNewBillNoX
                End If
                strSql = "UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & TransistNo.ToString & "'"
                strSql += " WHERE CTLID ='" & billcontrolidX & "' AND COMPANYID = '" & strCompanyId & "'"
                cmd = New OleDbCommand(strSql, XCnAdmin, tran)
                If cmd.ExecuteNonQuery() = 0 Then
                    GoTo GenerateNewBillNoX
                End If
            End If


            If _isFrachisee Then
                billcontrolid = "GEN-SALESBILLNO"
                strSql = "SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = '" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
                If strBCostid <> Nothing Then strSql += " AND ISNULL(COSTID,'') ='" & strBCostid & "'"
                NEWBILLNO = Val(GetSqlValue(strSql, XCnAdmin, tran)) + 1
SAGenerateNewBillNo:
                strSql = "UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & NEWBILLNO.ToString & "'"
                strSql += " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
                If strBCostid <> Nothing Then strSql += " AND ISNULL(COSTID,'') ='" & strBCostid & "'"
                cmd = New OleDbCommand(strSql, XCnAdmin, tran)

                strSql += vbCrLf + " SELECT TOP 1 SNO FROM " & cnStockDb & "..ISSUE WHERE TRANTYPE = 'SA' AND COSTID = '" & cnCostId & "' AND TRANNO = " & NEWBILLNO
                If BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, strSql,, "-1", tran) <> "-1" Then
                    NEWBILLNO = NEWBILLNO + 1
                    GoTo SAGenerateNewBillNo
                End If
                If cmd.ExecuteNonQuery() = 0 Then
                    If strBCostid <> Nothing Then MsgBox("No bill control for this cost id " & strBCostid) : Exit Sub
                    GoTo GenerateNewBillNo
                End If
            End If
            Isbulkupdate = False
            Dim xitemid As Integer
            Dim xsubitemid As Integer
            Dim xItemTypeCatcode As String = ""
            If AUTOBOOKTRAN Or _isFrachisee Then Batchno = GetNewBatchnoNew(cnCostId, BillDate, XCnAdmin, tran)
            Dim mxrate As Decimal = 0

            If ChkTagTran Then
                For Each ro As DataRow In dtGridView.Rows
                    Dim dSnoNullCheck As String = Convert.ToString(ro.Item("SNO"))
                    If dSnoNullCheck <> "" Then
                        For Each row As DataGridViewRow In gridView.Rows()
                            Dim gSno As String = Convert.ToString(row.Cells("SNO").Value)
                            Dim dSno As String = Convert.ToString(ro.Item("SNO"))
                            If String.Equals(ro.Item("SNO"), gSno) Then
                                Dim isSelected As Boolean = Convert.ToBoolean(row.Cells("checkBoxColumn").Value)
                                If isSelected Then
                                    mxrate = 0
                                    Dim mamount As Decimal = 0
                                    xitemid = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & ro.Item("Item").ToString & "'", , , tran).ToString)
                                    xsubitemid = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & ro.Item("SubItem").ToString & "' AND ITEMID = " & xitemid, , , tran).ToString)
                                    If ro.Item("STOCKMODE").ToString = "N" Then
                                        Dim xitemctrid As Integer = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & ro.Item("COUNTER").ToString & "'", , , tran).ToString)
                                        mamount = 0
                                        If AUTOBOOKTRAN And _isFrachisee = False Then
                                            ''Dim mitemrow As DataRow = GetSqlRow("SELECT CATCODE,METALID,CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID= " & xitemid & "", XCnAdmin, tran)
                                            Dim mitemrow As DataRow = Nothing
                                            If NeedItemType_accpost Then
                                                strSql = " SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMNONTAG WHERE SNO ='" & ro.Item("SNO").ToString & "'"
                                                Dim _xitemtypeid As Integer = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, strSql, , , tran).ToString)
                                                xItemTypeCatcode = BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = '" & _xitemtypeid & "'", , "", tran).ToString
                                            Else
                                                xItemTypeCatcode = ""
                                            End If
                                            If xItemTypeCatcode.ToString <> "" Then
                                                mitemrow = GetSqlRow("SELECT '" & xItemTypeCatcode.ToString & "' CATCODE,METALID,CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID= " & xitemid & "", XCnAdmin, tran)
                                            Else
                                                mitemrow = GetSqlRow("SELECT CATCODE,METALID,CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID= " & xitemid & "", XCnAdmin, tran)
                                            End If


                                            mxrate = Val(GetRate(BillDate, mitemrow(0).ToString, ))
                                            If AUTOBOOKVALUEG_PER <> 0 And mitemrow(1).ToString = "G" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUEG_PER / 100))
                                            If AUTOBOOKVALUES_PER <> 0 And mitemrow(1).ToString = "S" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUES_PER / 100))
                                            If AUTOBOOKVALUEP_PER <> 0 And mitemrow(1).ToString = "P" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUEP_PER / 100))
                                            If mitemrow(2).ToString = "W" Or mitemrow(2).ToString = "B" Then
                                                If AUTOBOOKVALUED_PER <> 0 And mitemrow(1).ToString = "D" Then mxrate = Val(ro.Item("Rate").ToString) : mxrate = mxrate + (mxrate * (AUTOBOOKVALUED_PER / 100))
                                                If AUTOBOOKVALUET_PER <> 0 And mitemrow(1).ToString = "T" Then mxrate = Val(ro.Item("Rate").ToString) : mxrate = mxrate + (mxrate * (AUTOBOOKVALUET_PER / 100))
                                            End If
                                            If mitemrow(2).ToString = "W" Or mitemrow(2).ToString = "B" Then mamount = mxrate * Val(ro.Item("GRSWT").ToString)
                                            If mitemrow(2).ToString = "F" Or mitemrow(2).ToString = "R" Then mamount = Val(ro.Item("SALVALUE").ToString)
                                            Dim Stnamount As Decimal = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID = A.STNITEMID WHERE A.TAGSNO = '" & ro.Item("SNO") & "' AND I.METALID='T' ", , 0, tran).ToString)
                                            Dim Diaamount As Decimal = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID = A.STNITEMID WHERE A.TAGSNO = '" & ro.Item("SNO") & "' AND I.METALID='D' ", , 0, tran).ToString)
                                            If AUTOBOOKVALUET_PER <> 0 Then Stnamount = Stnamount + (Stnamount * (AUTOBOOKVALUET_PER / 100))
                                            If AUTOBOOKVALUED_PER <> 0 Then Diaamount = Diaamount + (Diaamount * (AUTOBOOKVALUED_PER / 100))
                                            mamount = mamount + Stnamount + Diaamount
                                            mamount = CalcRoundoffAmt(mamount, "H")
                                            strSql = " UPDATE " & cnAdminDb & "..ITEMNONTAG SET POSTED ='T',TRFVALUE= " & mamount & " WHERE SNO ='" & ro.Item("SNO").ToString & "'"
                                            cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
                                            If (AUTOBOOKVALUEENABLE = "Y" Or STKTRAN_StoreAmt) And mamount <> 0 Then
                                                strSql = " UPDATE A SET A.TRFVALUE = A.TRFVALUE+ISNULL(A.STNAMT,0)+(ISNULL(A.STNAMT,0)*" & (AUTOBOOKVALUED_PER / 100) & ") FROM " & cnAdminDb & "..ITEMNONTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON A.STNITEMID = I.ITEMID WHERE A.TAGSNO ='" & ro.Item("SNO").ToString & "' AND I.METALID = 'D'"
                                                cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
                                                strSql = " UPDATE A SET A.TRFVALUE = A.TRFVALUE+ISNULL(A.STNAMT,0)+(ISNULL(A.STNAMT,0)*" & (AUTOBOOKVALUET_PER / 100) & ") FROM " & cnAdminDb & "..ITEMNONTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON A.STNITEMID = I.ITEMID WHERE A.TAGSNO ='" & ro.Item("SNO").ToString & "' AND I.METALID = 'T'"
                                                cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
                                            End If
                                        ElseIf _isFrachisee Then
                                            ''Dim mitemrow As DataRow = GetSqlRow("SELECT CATCODE,METALID,CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID= " & xitemid & "", XCnAdmin, tran)
                                            Dim mitemrow As DataRow = Nothing
                                            If NeedItemType_accpost Then
                                                strSql = " SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMNONTAG WHERE SNO ='" & ro.Item("SNO").ToString & "'"
                                                Dim _xitemtypeid As Integer = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, strSql, , , tran).ToString)
                                                xItemTypeCatcode = BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = '" & _xitemtypeid & "'", , "", tran).ToString
                                            Else
                                                xItemTypeCatcode = ""
                                            End If
                                            If xItemTypeCatcode.ToString <> "" Then
                                                mitemrow = GetSqlRow("SELECT '" & xItemTypeCatcode.ToString & "' CATCODE,METALID,CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID= " & xitemid & "", XCnAdmin, tran)
                                            Else
                                                mitemrow = GetSqlRow("SELECT CATCODE,METALID,CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID= " & xitemid & "", XCnAdmin, tran)
                                            End If

                                            mxrate = Val(GetRate(BillDate, mitemrow(0).ToString, ))
                                            If mitemrow(2).ToString = "W" Or mitemrow(2).ToString = "B" Then
                                                Dim WastVal As Double = (Val(ro.Item("GRSWT").ToString) * Val(ObjMaxMinValue.txtMaxWastage_Per.Text)) / 100
                                                mamount = mxrate * Val(ro.Item("GRSWT").ToString)
                                                mamount += (Val(ro.Item("GRSWT").ToString) * Val(ObjMaxMinValue.txtMaxMcPerGram_Amt.Text))
                                                mamount += (mxrate * WastVal)
                                            End If
                                            If mitemrow(2).ToString = "F" Or mitemrow(2).ToString = "R" Then mamount = Val(ro.Item("SALVALUE").ToString)
                                            Dim Stnamount As Decimal = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID = A.STNITEMID WHERE A.TAGSNO = '" & ro.Item("SNO") & "' AND I.METALID='T' ", , 0, tran).ToString)
                                            Dim Diaamount As Decimal = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID = A.STNITEMID WHERE A.TAGSNO = '" & ro.Item("SNO") & "' AND I.METALID='D' ", , 0, tran).ToString)
                                            mamount = mamount + Stnamount + Diaamount
                                            mamount = CalcRoundoffAmt(mamount, "H")
                                            strSql = " UPDATE " & cnAdminDb & "..ITEMNONTAG SET POSTED ='T',TRFVALUE= " & mamount & " WHERE SNO ='" & ro.Item("SNO").ToString & "'"
                                            cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
                                            If mamount <> 0 Then
                                                strSql = " UPDATE A SET A.TRFVALUE = A.TRFVALUE+ISNULL(A.STNAMT,0)  FROM " & cnAdminDb & "..ITEMNONTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON A.STNITEMID = I.ITEMID WHERE A.TAGSNO ='" & ro.Item("SNO").ToString & "' AND I.METALID = 'D'"
                                                cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
                                                strSql = " UPDATE A SET A.TRFVALUE = A.TRFVALUE+ISNULL(A.STNAMT,0)  FROM " & cnAdminDb & "..ITEMNONTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON A.STNITEMID = I.ITEMID WHERE A.TAGSNO ='" & ro.Item("SNO").ToString & "' AND I.METALID = 'T'"
                                                cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
                                            End If
                                        End If
                                        'Dim Sno As String = GetNewSno(TranSnoType.ITEMNONTAGCODE, tran, "GET_ADMINSNO_TRAN")
                                        Dim Sno As String = GetNewSnoNew(TranSnoType.ITEMNONTAGCODE, XCnAdmin, tran, "GET_ADMINSNO_TRAN")
                                        NTransSno += "'" & Sno.ToString & "',"
                                        Dim mpcs As Integer = Val(ro.Item("PCS").ToString)
                                        Dim mGwt As Double = Val(ro.Item("GRSWT").ToString)
                                        Dim mNwt As Double = Val(ro.Item("NETWT").ToString)

                                        If ro.Item("METALID").ToString = "T" Then mpcs += Val(ro.Item("STNPCS").ToString) + Val(ro.Item("PSTNPCS").ToString) : mGwt += Val(ro.Item("STNWT").ToString) + Val(ro.Item("PSTNWT").ToString) : mNwt += Val(ro.Item("STNWT").ToString) + Val(ro.Item("PSTNWT").ToString)
                                        If ro.Item("METALID").ToString = "D" Then mpcs += Val(ro.Item("DIAPCS").ToString) : mGwt += Val(ro.Item("DIAWT").ToString) : mNwt += Val(ro.Item("DIAWT").ToString)

                                        InsertIntoNonTag(frmCostId, toCostId, xitemid, xsubitemid, Val(ro.Item("DESIGNERID").ToString), mpcs, mGwt, mNwt, RefNo, NEWBILLNO, ro.Item("TAGNO").ToString, xitemctrid, mamount, Sno, ro.Item("SNO").ToString, ro)

                                        strSql = "INSERT INTO TRANS_STATUS (STOCKMODE,TAGSNO,STATUS,MOVETYPE) SELECT 'N','" & Sno.ToString & "','U','O'"
                                        cmd = New OleDbCommand(strSql, XCnAdmin, tran)
                                        cmd.ExecuteNonQuery()
                                    Else
                                        If ro.Item("SNO").ToString = "" Then Continue For
                                        If AUTOBOOKTRAN And _isFrachisee = False Then
                                            ''Dim mitemrow As DataRow = GetSqlRow("SELECT CATCODE,METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & ro.Item("Item").ToString & "'", XCnAdmin, tran)
                                            Dim mitemrow As DataRow = Nothing
                                            If NeedItemType_accpost Then
                                                strSql = " SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTAG WHERE SNO ='" & ro.Item("SNO").ToString & "'"
                                                Dim _xitemtypeid As Integer = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, strSql, , , tran).ToString)
                                                xItemTypeCatcode = BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = '" & _xitemtypeid & "'", , "", tran).ToString
                                            Else
                                                xItemTypeCatcode = ""
                                            End If
                                            If xItemTypeCatcode.ToString <> "" Then
                                                mitemrow = GetSqlRow("SELECT '" & xItemTypeCatcode.ToString & "' CATCODE,METALID,CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID= " & xitemid & "", XCnAdmin, tran)
                                            Else
                                                mitemrow = GetSqlRow("SELECT CATCODE,METALID,CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID= " & xitemid & "", XCnAdmin, tran)
                                            End If

                                            mxrate = Val(GetRate(BillDate, mitemrow(0).ToString, ))
                                            If AUTOBOOKVALUEG_PER <> 0 And mitemrow(1).ToString = "G" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUEG_PER / 100))
                                            If AUTOBOOKVALUES_PER <> 0 And mitemrow(1).ToString = "S" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUES_PER / 100))
                                            If AUTOBOOKVALUEP_PER <> 0 And mitemrow(1).ToString = "P" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUEP_PER / 100))
                                            'If mitemrow(1).ToString = "D" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUED_PER / 100))
                                            'If mitemrow(1).ToString = "T" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUET_PER / 100))
                                            If ro.Item("SALEMODE").ToString = "W" Or ro.Item("SALEMODE").ToString = "B" Then
                                                If AUTOBOOKVALUED_PER <> 0 And mitemrow(1).ToString = "D" Then mxrate = Val(ro.Item("Rate").ToString) : mxrate = mxrate + (mxrate * (AUTOBOOKVALUED_PER / 100))
                                                If AUTOBOOKVALUET_PER <> 0 And mitemrow(1).ToString = "T" Then mxrate = Val(ro.Item("Rate").ToString) : mxrate = mxrate + (mxrate * (AUTOBOOKVALUET_PER / 100))
                                            End If

                                            If ro.Item("SALEMODE").ToString = "W" Or ro.Item("SALEMODE").ToString = "B" Then mamount = mxrate * Val(ro.Item("GRSWT").ToString)
                                            If ro.Item("SALEMODE").ToString = "F" Or ro.Item("SALEMODE").ToString = "R" Or ro.Item("SALEMODE").ToString = "M" Then mamount = Val(ro.Item("SALVALUE").ToString)

                                            Dim Stnamount As Decimal = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID = A.STNITEMID WHERE A.TAGSNO = '" & ro.Item("SNO") & "' AND I.METALID='T' ", , 0, tran).ToString)
                                            Dim Diaamount As Decimal = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID = A.STNITEMID WHERE A.TAGSNO = '" & ro.Item("SNO") & "' AND I.METALID='D' ", , 0, tran).ToString)
                                            If AUTOBOOKVALUET_PER <> 0 Then Stnamount = Stnamount + (Stnamount * (AUTOBOOKVALUET_PER / 100))
                                            If AUTOBOOKVALUED_PER <> 0 Then Diaamount = Diaamount + (Diaamount * (AUTOBOOKVALUED_PER / 100))
                                            mamount = mamount + Stnamount + Diaamount
                                            mamount = CalcRoundoffAmt(mamount, "H")
                                        ElseIf _isFrachisee Then
                                            ''Dim mitemrow As DataRow = GetSqlRow("SELECT CATCODE,METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & ro.Item("Item").ToString & "'", XCnAdmin, tran)
                                            Dim mitemrow As DataRow = Nothing
                                            If NeedItemType_accpost Then
                                                strSql = " SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTAG WHERE SNO ='" & ro.Item("SNO").ToString & "'"
                                                Dim _xitemtypeid As Integer = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, strSql, , , tran).ToString)
                                                xItemTypeCatcode = BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = '" & _xitemtypeid & "'", , "", tran).ToString
                                            Else
                                                xItemTypeCatcode = ""
                                            End If
                                            If xItemTypeCatcode.ToString <> "" Then
                                                mitemrow = GetSqlRow("SELECT '" & xItemTypeCatcode.ToString & "' CATCODE,METALID,CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID= " & xitemid & "", XCnAdmin, tran)
                                            Else
                                                mitemrow = GetSqlRow("SELECT CATCODE,METALID,CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID= " & xitemid & "", XCnAdmin, tran)
                                            End If

                                            mxrate = Val(GetRate(BillDate, mitemrow(0).ToString, ))
                                            If FRANCHISEE_VALUEARY(0).ToString = "Y" Then
                                                If ro.Item("SALEMODE").ToString = "W" Or ro.Item("SALEMODE").ToString = "B" Then
                                                    Dim WastVal As Double = 0
                                                    Dim McVal As Double = 0
                                                    If Val(ro.Item("MAXMC").ToString) <> 0 And Val(FRANCHISEE_VALUEARY(1).ToString) <> 0 Then
                                                        McVal = Val(ro.Item("MAXMC").ToString) * (Val(FRANCHISEE_VALUEARY(1).ToString) / 100)
                                                    End If
                                                    If Val(ro.Item("MAXWASTPER").ToString) <> 0 And Val(FRANCHISEE_VALUEARY(2).ToString) <> 0 Then
                                                        WastVal = Val(ro.Item("GRSWT").ToString) * ((Val(ro.Item("MAXWASTPER").ToString) - Val(FRANCHISEE_VALUEARY(2).ToString)) / 100)
                                                    End If
                                                    Dim _Purity As Decimal = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT TOP 1 PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID =(SELECT PURITYID FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =(SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTAG WHERE SNO ='" & ro.Item("SNO").ToString & "'))", , 0, tran).ToString)
                                                    If mitemrow(1).ToString = "G" Then
                                                        If Val(_Purity) > 91 And Val(_Purity) < 92 Then
                                                            If Val(FRANCHISEE_VALUEARY(7).ToString) <> 0 Then
                                                                mxrate = Val(FRANCHISEE_VALUEARY(7).ToString)
                                                            End If
                                                        Else
                                                            If Val(FRANCHISEE_VALUEARY(6).ToString) <> 0 Then
                                                                mxrate = Val(FRANCHISEE_VALUEARY(6).ToString)
                                                            End If
                                                        End If
                                                    ElseIf mitemrow(1).ToString = "S" Then
                                                        If Val(FRANCHISEE_VALUEARY(9).ToString) <> 0 Then
                                                            mxrate = Val(FRANCHISEE_VALUEARY(9).ToString)
                                                        End If
                                                    ElseIf mitemrow(1).ToString = "P" Then
                                                        If Val(FRANCHISEE_VALUEARY(8).ToString) <> 0 Then
                                                            mxrate = Val(FRANCHISEE_VALUEARY(8).ToString)
                                                        End If
                                                    End If
                                                    Dim _CGROUPID As String = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT TOP 1 CGROUPID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE =(SELECT CATCODE FROM " & cnAdminDb & "..ITEMTAG WHERE SNO ='" & ro.Item("SNO").ToString & "')", , 0, tran).ToString)
                                                    If mitemrow(1).ToString = "D" Then
                                                        If FRANCHISEE_VALUEARY(5).Length = 2 Then
                                                            Dim _tempspl() As String = FRANCHISEE_VALUEARY(5).Split(":")
                                                            If _tempspl(0).ToString = _CGROUPID.ToString Then
                                                                mxrate = Val(_tempspl(1).ToString)
                                                            ElseIf Val(FRANCHISEE_VALUEARY(4).ToString) <> 0 Then
                                                                mxrate = Val(FRANCHISEE_VALUEARY(4).ToString)
                                                            End If
                                                        ElseIf Val(FRANCHISEE_VALUEARY(4).ToString) <> 0 Then
                                                            mxrate = Val(FRANCHISEE_VALUEARY(4).ToString)
                                                        End If

                                                    End If
                                                    If mitemrow(1).ToString = "T" Then mxrate = Val(ro.Item("Rate").ToString) : mxrate = mxrate * (Val(FRANCHISEE_VALUEARY(3).ToString) / 100)

                                                    mamount = mxrate * Val(ro.Item("GRSWT").ToString)
                                                    mamount += McVal
                                                    mamount += (mxrate * WastVal)
                                                End If
                                                If ro.Item("SALEMODE").ToString = "F" Or ro.Item("SALEMODE").ToString = "R" Or ro.Item("SALEMODE").ToString = "M" Then mamount = Val(ro.Item("SALVALUE").ToString)

                                                Dim _dttempstone As DataTable
                                                Dim _qry As String = ""
                                                _qry = " SELECT (SELECT TOP 1 CGROUPID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE =I.CATCODE)CGROUPID,* FROM " & cnAdminDb & "..ITEMTAGSTONE A "
                                                _qry += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID = A.STNITEMID WHERE A.TAGSNO = '" & ro.Item("SNO") & "'"
                                                cmd = New OleDbCommand(_qry, XCnAdmin, tran)
                                                da = New OleDbDataAdapter(cmd)
                                                _dttempstone = New DataTable
                                                da.Fill(_dttempstone)
                                                Dim Stnamount As Decimal = 0
                                                Dim Diaamount As Decimal = 0
                                                If _dttempstone.Rows.Count > 0 Then
                                                    For Each _row As DataRow In _dttempstone.Rows
                                                        Dim _tempstone As Decimal = 0
                                                        If _row("METALID").ToString = "T" Then
                                                            If Val(FRANCHISEE_VALUEARY(3).ToString) <> 0 Then _tempstone = (Val(_row("STNAMT").ToString) * (Val(FRANCHISEE_VALUEARY(3).ToString) / 100))
                                                            strSql = " UPDATE " & cnAdminDb & "..ITEMTAGSTONE SET TRFVALUE= " & _tempstone & " WHERE SNO ='" & _row.Item("SNO").ToString & "'"
                                                            cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
                                                            Stnamount += _tempstone
                                                        End If
                                                        If _row("METALID").ToString = "D" Then
                                                            Dim _tempspl() As String = Nothing
                                                            If FRANCHISEE_VALUEARY(5).ToString <> "" Then
                                                                _tempspl = FRANCHISEE_VALUEARY(5).Split(":")
                                                            End If
                                                            If Not _tempspl Is Nothing Then
                                                                If _tempspl.Length = 2 Then
                                                                    If _tempspl(0).ToString = _row("CGROUPID").ToString Then
                                                                        If Val(_tempspl(1).ToString) <> 0 Then
                                                                            _tempstone = (Val(_row("STNWT").ToString) * Val(FRANCHISEE_VALUEARY(4).ToString))
                                                                        Else
                                                                            _tempstone = Val(_row("STNAMT").ToString)
                                                                        End If
                                                                    ElseIf Val(FRANCHISEE_VALUEARY(4).ToString) <> 0 Then
                                                                        _tempstone = (Val(_row("STNWT").ToString) * Val(FRANCHISEE_VALUEARY(4).ToString))
                                                                    Else
                                                                        _tempstone = Val(_row("STNAMT").ToString)
                                                                    End If
                                                                ElseIf Val(FRANCHISEE_VALUEARY(4).ToString) <> 0 Then
                                                                    _tempstone = (Val(_row("STNWT").ToString) * Val(FRANCHISEE_VALUEARY(4).ToString))
                                                                Else
                                                                    _tempstone = Val(_row("STNAMT").ToString)
                                                                End If
                                                            ElseIf Val(FRANCHISEE_VALUEARY(4).ToString) <> 0 Then
                                                                _tempstone = (Val(_row("STNWT").ToString) * Val(FRANCHISEE_VALUEARY(4).ToString))
                                                            Else
                                                                _tempstone = Val(_row("STNAMT").ToString)
                                                            End If
                                                            strSql = " UPDATE " & cnAdminDb & "..ITEMTAGSTONE SET TRFVALUE= " & _tempstone & " WHERE SNO ='" & _row.Item("SNO").ToString & "'"
                                                            cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
                                                            Diaamount += _tempstone
                                                        End If
                                                    Next
                                                End If
                                                mamount = mamount + Stnamount + Diaamount
                                                mamount = CalcRoundoffAmt(mamount, "H")
                                                strSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET TRFVALUE= " & mamount & ",RATE=" & mxrate & " WHERE SNO ='" & ro.Item("SNO").ToString & "'"
                                                cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
                                            Else
                                                If ro.Item("SALEMODE").ToString = "W" Or ro.Item("SALEMODE").ToString = "B" Then
                                                    Dim objWmc As New CLS_MAXMINVALUES("I", Val(ro.Item("GRSWT").ToString), xitemid, xsubitemid, toCostId)
                                                    Dim WastVal As Double
                                                    Dim McVal As Double
                                                    If objWmc.pDtWmc.Rows.Count > 0 Then
                                                        With objWmc.pDtWmc.Rows(0)
                                                            WastVal = (Val(ro.Item("GRSWT").ToString) * Val(.Item("MAXWASTPER").ToString)) / 100
                                                            McVal = (Val(ro.Item("GRSWT").ToString) * Val(.Item("MAXMCGRM").ToString))
                                                        End With
                                                    End If
                                                    mamount = mxrate * Val(ro.Item("GRSWT").ToString)
                                                    mamount += (Val(ro.Item("GRSWT").ToString) * McVal)
                                                    mamount += (mxrate * WastVal)
                                                End If
                                                If ro.Item("SALEMODE").ToString = "F" Or ro.Item("SALEMODE").ToString = "R" Or ro.Item("SALEMODE").ToString = "M" Then mamount = Val(ro.Item("SALVALUE").ToString)
                                                Dim Stnamount As Decimal = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID = A.STNITEMID WHERE A.TAGSNO = '" & ro.Item("SNO") & "' AND I.METALID='T' ", , 0, tran).ToString)
                                                Dim Diaamount As Decimal = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID = A.STNITEMID WHERE A.TAGSNO = '" & ro.Item("SNO") & "' AND I.METALID='D' ", , 0, tran).ToString)
                                                mamount = mamount + Stnamount + Diaamount
                                                mamount = CalcRoundoffAmt(mamount, "H")
                                                strSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET TRFVALUE= " & mamount & ",RATE=" & mxrate & " WHERE SNO ='" & ro.Item("SNO").ToString & "'"
                                                cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
                                                strSql = " UPDATE A SET A.TRFVALUE = ISNULL(A.STNAMT,0)  FROM " & cnAdminDb & "..ITEMTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON A.STNITEMID = I.ITEMID WHERE A.TAGSNO ='" & ro.Item("SNO").ToString & "' AND I.METALID = 'D'"
                                                cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
                                                strSql = " UPDATE A SET A.TRFVALUE = ISNULL(A.STNAMT,0)  FROM " & cnAdminDb & "..ITEMTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON A.STNITEMID = I.ITEMID WHERE A.TAGSNO ='" & ro.Item("SNO").ToString & "' AND I.METALID = 'T'"
                                                cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
                                            End If
                                        End If

                                        If ReplSubItemid Then
                                            xitemid = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & ro.Item("Item").ToString & "'", , , tran).ToString)
                                            xsubitemid = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & ro.Item("SubItem").ToString & "' AND ITEMID = " & xitemid, , , tran).ToString)
                                            strSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET SUBITEMID= " & xsubitemid & " WHERE SNO ='" & ro.Item("SNO").ToString & "'"
                                            cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
                                        End If
                                        strSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET TRANSFERDATE= '" & BillDate.ToString & "' WHERE SNO ='" & ro.Item("SNO").ToString & "'"
                                        cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
                                        If (AUTOBOOKVALUEENABLE = "Y" Or STKTRAN_StoreAmt) And mamount <> 0 Then
                                            strSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET TRFVALUE= " & mamount & " WHERE SNO ='" & ro.Item("SNO").ToString & "'"
                                            cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
                                            strSql = " UPDATE A SET A.TRFVALUE = ISNULL(A.STNAMT,0)+(ISNULL(A.STNAMT,0)*" & (AUTOBOOKVALUED_PER / 100) & ") FROM " & cnAdminDb & "..ITEMTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON A.STNITEMID = I.ITEMID WHERE A.TAGSNO ='" & ro.Item("SNO").ToString & "' AND I.METALID = 'D'"
                                            cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
                                            strSql = " UPDATE A SET A.TRFVALUE = ISNULL(A.STNAMT,0)+(ISNULL(A.STNAMT,0)*" & (AUTOBOOKVALUET_PER / 100) & ") FROM " & cnAdminDb & "..ITEMTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON A.STNITEMID = I.ITEMID WHERE A.TAGSNO ='" & ro.Item("SNO").ToString & "' AND I.METALID = 'T'"
                                            cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
                                        End If

                                        If Not Isbulkupdate Then
                                            obj = New TrasitIssRec(cnCostId, toCostId, "I", BillDate, ro.Item("SNO").ToString, XCnAdmin, tran, RefNo)
                                            If _isFrachisee Then
                                                If Not obj.InsertTagIssue(True, Val(ObjMaxMinValue.txtMaxWastage_Per.Text) _
                                                , Val(ObjMaxMinValue.txtMinWastage_Per.Text) _
                                                , Val(ObjMaxMinValue.txtMaxMcPerGram_Amt.Text) _
                                                , Val(ObjMaxMinValue.txtMinMcPerGram_Amt.Text)) Then
                                                    Continue For
                                                End If
                                            Else
                                                If Not obj.InsertTagIssue(True) Then
                                                    Continue For
                                                End If
                                            End If
                                            TransSno += "'" & ro.Item("SNO").ToString & "',"
                                            strSql = "INSERT INTO TRANS_STATUS (STOCKMODE,TAGSNO,STATUS,MOVETYPE) SELECT 'T','" & ro.Item("Sno").ToString & "','U','O'"
                                            cmd = New OleDbCommand(strSql, XCnAdmin, tran)
                                            cmd.ExecuteNonQuery()

                                        End If
                                        TransSno += "'" & ro.Item("SNO").ToString & "',"
                                        If IS_IMAGE_TRF = True Then
                                            If ro.Item("PCTFILE").ToString <> "" And defalutDestination <> "" Then
                                                If IO.File.Exists(defalutDestination & ro.Item("PCTFILE").ToString) Then
                                                    strSql = " INSERT INTO " & syncdb & "..SENDSYNC(FROMID,TOID"
                                                    strSql += ",TAGIMAGE,TAGIMAGENAME,IMAGE_CTRLID"
                                                    strSql += " )"
                                                    strSql += " VALUES"
                                                    strSql += " ('" & cnCostId & "','" & toCostId & "',?,?,'PICPATH')"
                                                    cmd = New OleDbCommand(strSql, XCnAdmin, tran)
                                                    Dim fileStr As New IO.FileStream(defalutDestination & ro.Item("PCTFILE").ToString, IO.FileMode.Open, IO.FileAccess.Read)
                                                    Dim reader As New IO.BinaryReader(fileStr)
                                                    Dim result As Byte() = reader.ReadBytes(CType(fileStr.Length, Integer))
                                                    fileStr.Read(result, 0, result.Length)
                                                    fileStr.Close()
                                                    cmd.Parameters.AddWithValue("@TAGIMAGE", result)
                                                    Dim fInfo As New IO.FileInfo(defalutDestination & ro.Item("PCTFILE").ToString)
                                                    cmd.Parameters.AddWithValue("@TAGIMAGENAME", fInfo.Name)
                                                    cmd.ExecuteNonQuery()
                                                End If
                                            End If
                                        End If

                                        strSql = "INSERT INTO " & cnStockDb & "..TRANSIT_AUDIT"
                                        strSql += " (COSTID,REFNO,FROMID,TOID,TRANDATE,TAGSNO,ISSREC,STOCKTYPE)"
                                        strSql += " SELECT '" & cnCostId & "','" & RefNo & "','" & cnCostId & "','" & toCostId & "','" & BillDate.ToString("yyyy-MM-dd") & "','" & ro.Item("SNO").ToString & "','I','T'"
                                        cmd = New OleDbCommand(strSql, XCnAdmin, tran)
                                        cmd.ExecuteNonQuery()

                                        strSql = " EXEC " & cnAdminDb & "..SP_CCTRANSFER "
                                        strSql += vbCrLf + " @TAGSNO = '" & ro.Item("SNO").ToString & "',@COSTID = '" & cnCostId & "',@ISSDATE = '" & BillDate.ToString("yyyy-MM-dd") & "',@ISSTIME=NULL,@VERSION='" & GlobalVariables.VERSION & "',@USERID = " & GlobalVariables.userId & ",@TRFNO='" & RefNo & "',@FLAG='I'"
                                        cmd = New OleDbCommand(strSql, XCnAdmin, tran)
                                        cmd.ExecuteNonQuery()
                                    End If
                                End If
                            End If
                        Next
                    End If
                Next
            Else
                For Each ro As DataRow In dtGridView.Rows
                    mxrate = 0
                    Dim mamount As Decimal = 0
                    xitemid = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & ro.Item("Item").ToString & "'", , , tran).ToString)
                    xsubitemid = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & ro.Item("SubItem").ToString & "' AND ITEMID = " & xitemid, , , tran).ToString)
                    If ro.Item("STOCKMODE").ToString = "N" Then
                        Dim xitemctrid As Integer = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & ro.Item("COUNTER").ToString & "'", , , tran).ToString)
                        mamount = 0
                        If AUTOBOOKTRAN And _isFrachisee = False Then
                            ''Dim mitemrow As DataRow = GetSqlRow("SELECT CATCODE,METALID,CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID= " & xitemid & "", XCnAdmin, tran)
                            Dim mitemrow As DataRow = Nothing
                            If NeedItemType_accpost Then
                                strSql = " SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMNONTAG WHERE SNO ='" & ro.Item("SNO").ToString & "'"
                                Dim _xitemtypeid As Integer = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, strSql, , , tran).ToString)
                                xItemTypeCatcode = BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = '" & _xitemtypeid & "'", , "", tran).ToString
                            Else
                                xItemTypeCatcode = ""
                            End If
                            If xItemTypeCatcode.ToString <> "" Then
                                mitemrow = GetSqlRow("SELECT '" & xItemTypeCatcode.ToString & "' CATCODE,METALID,CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID= " & xitemid & "", XCnAdmin, tran)
                            Else
                                mitemrow = GetSqlRow("SELECT CATCODE,METALID,CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID= " & xitemid & "", XCnAdmin, tran)
                            End If

                            mxrate = Val(GetRate(BillDate, mitemrow(0).ToString, ))
                            If AUTOBOOKVALUEG_PER <> 0 And mitemrow(1).ToString = "G" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUEG_PER / 100))
                            If AUTOBOOKVALUES_PER <> 0 And mitemrow(1).ToString = "S" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUES_PER / 100))
                            If AUTOBOOKVALUEP_PER <> 0 And mitemrow(1).ToString = "P" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUEP_PER / 100))
                            If mitemrow(2).ToString = "W" Or mitemrow(2).ToString = "B" Then
                                If AUTOBOOKVALUED_PER <> 0 And mitemrow(1).ToString = "D" Then mxrate = Val(ro.Item("Rate").ToString) : mxrate = mxrate + (mxrate * (AUTOBOOKVALUED_PER / 100))
                                If AUTOBOOKVALUET_PER <> 0 And mitemrow(1).ToString = "T" Then mxrate = Val(ro.Item("Rate").ToString) : mxrate = mxrate + (mxrate * (AUTOBOOKVALUET_PER / 100))
                            End If
                            If mitemrow(2).ToString = "W" Or mitemrow(2).ToString = "B" Then mamount = mxrate * Val(ro.Item("GRSWT").ToString)
                            If mitemrow(2).ToString = "F" Or mitemrow(2).ToString = "R" Then mamount = Val(ro.Item("SALVALUE").ToString)
                            Dim Stnamount As Decimal = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID = A.STNITEMID WHERE A.TAGSNO = '" & ro.Item("SNO") & "' AND I.METALID='T' ", , 0, tran).ToString)
                            Dim Diaamount As Decimal = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID = A.STNITEMID WHERE A.TAGSNO = '" & ro.Item("SNO") & "' AND I.METALID='D' ", , 0, tran).ToString)
                            If AUTOBOOKVALUET_PER <> 0 Then Stnamount = Stnamount + (Stnamount * (AUTOBOOKVALUET_PER / 100))
                            If AUTOBOOKVALUED_PER <> 0 Then Diaamount = Diaamount + (Diaamount * (AUTOBOOKVALUED_PER / 100))
                            mamount = mamount + Stnamount + Diaamount
                            mamount = CalcRoundoffAmt(mamount, "H")
                            strSql = " UPDATE " & cnAdminDb & "..ITEMNONTAG SET POSTED ='T',TRFVALUE= " & mamount & " WHERE SNO ='" & ro.Item("SNO").ToString & "'"
                            cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
                            If (AUTOBOOKVALUEENABLE = "Y" Or STKTRAN_StoreAmt) And mamount <> 0 Then
                                strSql = " UPDATE A SET A.TRFVALUE = A.TRFVALUE+ISNULL(A.STNAMT,0)+(ISNULL(A.STNAMT,0)*" & (AUTOBOOKVALUED_PER / 100) & ") FROM " & cnAdminDb & "..ITEMNONTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON A.STNITEMID = I.ITEMID WHERE A.TAGSNO ='" & ro.Item("SNO").ToString & "' AND I.METALID = 'D'"
                                cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
                                strSql = " UPDATE A SET A.TRFVALUE = A.TRFVALUE+ISNULL(A.STNAMT,0)+(ISNULL(A.STNAMT,0)*" & (AUTOBOOKVALUET_PER / 100) & ") FROM " & cnAdminDb & "..ITEMNONTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON A.STNITEMID = I.ITEMID WHERE A.TAGSNO ='" & ro.Item("SNO").ToString & "' AND I.METALID = 'T'"
                                cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
                            End If
                        ElseIf _isFrachisee Then
                            ''Dim mitemrow As DataRow = GetSqlRow("SELECT CATCODE,METALID,CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID= " & xitemid & "", XCnAdmin, tran)
                            Dim mitemrow As DataRow = Nothing
                            If NeedItemType_accpost Then
                                strSql = " SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMNONTAG WHERE SNO ='" & ro.Item("SNO").ToString & "'"
                                Dim _xitemtypeid As Integer = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, strSql, , , tran).ToString)
                                xItemTypeCatcode = BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = '" & _xitemtypeid & "'", , "", tran).ToString
                            Else
                                xItemTypeCatcode = ""
                            End If
                            If xItemTypeCatcode.ToString <> "" Then
                                mitemrow = GetSqlRow("SELECT '" & xItemTypeCatcode.ToString & "' CATCODE,METALID,CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID= " & xitemid & "", XCnAdmin, tran)
                            Else
                                mitemrow = GetSqlRow("SELECT CATCODE,METALID,CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID= " & xitemid & "", XCnAdmin, tran)
                            End If

                            mxrate = Val(GetRate(BillDate, mitemrow(0).ToString, ))
                            If mitemrow(2).ToString = "W" Or mitemrow(2).ToString = "B" Then
                                Dim WastVal As Double = (Val(ro.Item("GRSWT").ToString) * Val(ObjMaxMinValue.txtMaxWastage_Per.Text)) / 100
                                mamount = mxrate * Val(ro.Item("GRSWT").ToString)
                                mamount += (Val(ro.Item("GRSWT").ToString) * Val(ObjMaxMinValue.txtMaxMcPerGram_Amt.Text))
                                mamount += (mxrate * WastVal)
                            End If
                            If mitemrow(2).ToString = "F" Or mitemrow(2).ToString = "R" Then mamount = Val(ro.Item("SALVALUE").ToString)
                            Dim Stnamount As Decimal = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID = A.STNITEMID WHERE A.TAGSNO = '" & ro.Item("SNO") & "' AND I.METALID='T' ", , 0, tran).ToString)
                            Dim Diaamount As Decimal = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID = A.STNITEMID WHERE A.TAGSNO = '" & ro.Item("SNO") & "' AND I.METALID='D' ", , 0, tran).ToString)
                            mamount = mamount + Stnamount + Diaamount
                            mamount = CalcRoundoffAmt(mamount, "H")
                            strSql = " UPDATE " & cnAdminDb & "..ITEMNONTAG SET POSTED ='T',TRFVALUE= " & mamount & " WHERE SNO ='" & ro.Item("SNO").ToString & "'"
                            cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
                            If mamount <> 0 Then
                                strSql = " UPDATE A SET A.TRFVALUE = A.TRFVALUE+ISNULL(A.STNAMT,0)  FROM " & cnAdminDb & "..ITEMNONTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON A.STNITEMID = I.ITEMID WHERE A.TAGSNO ='" & ro.Item("SNO").ToString & "' AND I.METALID = 'D'"
                                cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
                                strSql = " UPDATE A SET A.TRFVALUE = A.TRFVALUE+ISNULL(A.STNAMT,0)  FROM " & cnAdminDb & "..ITEMNONTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON A.STNITEMID = I.ITEMID WHERE A.TAGSNO ='" & ro.Item("SNO").ToString & "' AND I.METALID = 'T'"
                                cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
                            End If
                        End If
                        'Dim Sno As String = GetNewSno(TranSnoType.ITEMNONTAGCODE, tran, "GET_ADMINSNO_TRAN")
                        Dim Sno As String = GetNewSnoNew(TranSnoType.ITEMNONTAGCODE, XCnAdmin, tran, "GET_ADMINSNO_TRAN")
                        NTransSno += "'" & Sno.ToString & "',"
                        Dim mpcs As Integer = Val(ro.Item("PCS").ToString)
                        Dim mGwt As Double = Val(ro.Item("GRSWT").ToString)
                        Dim mNwt As Double = Val(ro.Item("NETWT").ToString)

                        If ro.Item("METALID").ToString = "T" Then mpcs += Val(ro.Item("STNPCS").ToString) + Val(ro.Item("PSTNPCS").ToString) : mGwt += Val(ro.Item("STNWT").ToString) + Val(ro.Item("PSTNWT").ToString) : mNwt += Val(ro.Item("STNWT").ToString) + Val(ro.Item("PSTNWT").ToString)
                        If ro.Item("METALID").ToString = "D" Then mpcs += Val(ro.Item("DIAPCS").ToString) : mGwt += Val(ro.Item("DIAWT").ToString) : mNwt += Val(ro.Item("DIAWT").ToString)

                        InsertIntoNonTag(frmCostId, toCostId, xitemid, xsubitemid, Val(ro.Item("DESIGNERID").ToString), mpcs, mGwt, mNwt, RefNo, NEWBILLNO, ro.Item("TAGNO").ToString, xitemctrid, mamount, Sno, ro.Item("SNO").ToString, ro)

                        strSql = "INSERT INTO TRANS_STATUS (STOCKMODE,TAGSNO,STATUS,MOVETYPE) SELECT 'N','" & Sno.ToString & "','U','O'"
                        cmd = New OleDbCommand(strSql, XCnAdmin, tran)
                        cmd.ExecuteNonQuery()
                    Else

                        If ro.Item("SNO").ToString = "" Then Continue For
                        If AUTOBOOKTRAN And _isFrachisee = False Then
                            ''Dim mitemrow As DataRow = GetSqlRow("SELECT CATCODE,METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & ro.Item("Item").ToString & "'", XCnAdmin, tran)
                            Dim mitemrow As DataRow = Nothing
                            If NeedItemType_accpost Then
                                strSql = " SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTAG WHERE SNO ='" & ro.Item("SNO").ToString & "'"
                                Dim _xitemtypeid As Integer = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, strSql, , , tran).ToString)
                                xItemTypeCatcode = BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = '" & _xitemtypeid & "'", , "", tran).ToString
                            Else
                                xItemTypeCatcode = ""
                            End If
                            If xItemTypeCatcode.ToString <> "" Then
                                mitemrow = GetSqlRow("SELECT '" & xItemTypeCatcode.ToString & "' CATCODE,METALID,CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID= " & xitemid & "", XCnAdmin, tran)
                            Else
                                mitemrow = GetSqlRow("SELECT CATCODE,METALID,CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID= " & xitemid & "", XCnAdmin, tran)
                            End If

                            mxrate = Val(GetRate(BillDate, mitemrow(0).ToString, ))
                            If AUTOBOOKVALUEG_PER <> 0 And mitemrow(1).ToString = "G" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUEG_PER / 100))
                            If AUTOBOOKVALUES_PER <> 0 And mitemrow(1).ToString = "S" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUES_PER / 100))
                            If AUTOBOOKVALUEP_PER <> 0 And mitemrow(1).ToString = "P" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUEP_PER / 100))
                            'If mitemrow(1).ToString = "D" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUED_PER / 100))
                            'If mitemrow(1).ToString = "T" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUET_PER / 100))
                            If ro.Item("SALEMODE").ToString = "W" Or ro.Item("SALEMODE").ToString = "B" Then
                                If AUTOBOOKVALUED_PER <> 0 And mitemrow(1).ToString = "D" Then mxrate = Val(ro.Item("Rate").ToString) : mxrate = mxrate + (mxrate * (AUTOBOOKVALUED_PER / 100))
                                If AUTOBOOKVALUET_PER <> 0 And mitemrow(1).ToString = "T" Then mxrate = Val(ro.Item("Rate").ToString) : mxrate = mxrate + (mxrate * (AUTOBOOKVALUET_PER / 100))
                            End If

                            If ro.Item("SALEMODE").ToString = "W" Or ro.Item("SALEMODE").ToString = "B" Then mamount = mxrate * Val(ro.Item("GRSWT").ToString)
                            If ro.Item("SALEMODE").ToString = "F" Or ro.Item("SALEMODE").ToString = "R" Or ro.Item("SALEMODE").ToString = "M" Then mamount = Val(ro.Item("SALVALUE").ToString)

                            Dim Stnamount As Decimal = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID = A.STNITEMID WHERE A.TAGSNO = '" & ro.Item("SNO") & "' AND I.METALID='T' ", , 0, tran).ToString)
                            Dim Diaamount As Decimal = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID = A.STNITEMID WHERE A.TAGSNO = '" & ro.Item("SNO") & "' AND I.METALID='D' ", , 0, tran).ToString)
                            If AUTOBOOKVALUET_PER <> 0 Then Stnamount = Stnamount + (Stnamount * (AUTOBOOKVALUET_PER / 100))
                            If AUTOBOOKVALUED_PER <> 0 Then Diaamount = Diaamount + (Diaamount * (AUTOBOOKVALUED_PER / 100))
                            mamount = mamount + Stnamount + Diaamount
                            mamount = CalcRoundoffAmt(mamount, "H")
                        ElseIf _isFrachisee Then
                            ''Dim mitemrow As DataRow = GetSqlRow("SELECT CATCODE,METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & ro.Item("Item").ToString & "'", XCnAdmin, tran)
                            Dim mitemrow As DataRow = Nothing
                            If NeedItemType_accpost Then
                                strSql = " SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTAG WHERE SNO ='" & ro.Item("SNO").ToString & "'"
                                Dim _xitemtypeid As Integer = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, strSql, , , tran).ToString)
                                xItemTypeCatcode = BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = '" & _xitemtypeid & "'", , "", tran).ToString
                            Else
                                xItemTypeCatcode = ""
                            End If
                            If xItemTypeCatcode.ToString <> "" Then
                                mitemrow = GetSqlRow("SELECT '" & xItemTypeCatcode.ToString & "' CATCODE,METALID,CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID= " & xitemid & "", XCnAdmin, tran)
                            Else
                                mitemrow = GetSqlRow("SELECT CATCODE,METALID,CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID= " & xitemid & "", XCnAdmin, tran)
                            End If

                            mxrate = Val(GetRate(BillDate, mitemrow(0).ToString, ))
                            If FRANCHISEE_VALUEARY(0).ToString = "Y" Then
                                If ro.Item("SALEMODE").ToString = "W" Or ro.Item("SALEMODE").ToString = "B" Then
                                    Dim WastVal As Double = 0
                                    Dim McVal As Double = 0
                                    If Val(ro.Item("MAXMC").ToString) <> 0 And Val(FRANCHISEE_VALUEARY(1).ToString) <> 0 Then
                                        McVal = Val(ro.Item("MAXMC").ToString) * (Val(FRANCHISEE_VALUEARY(1).ToString) / 100)
                                    End If
                                    If Val(ro.Item("MAXWASTPER").ToString) <> 0 And Val(FRANCHISEE_VALUEARY(2).ToString) <> 0 Then
                                        WastVal = Val(ro.Item("GRSWT").ToString) * ((Val(ro.Item("MAXWASTPER").ToString) - Val(FRANCHISEE_VALUEARY(2).ToString)) / 100)
                                    End If
                                    Dim _Purity As Decimal = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT TOP 1 PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID =(SELECT PURITYID FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID =(SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTAG WHERE SNO ='" & ro.Item("SNO").ToString & "'))", , 0, tran).ToString)
                                    If mitemrow(1).ToString = "G" Then
                                        If Val(_Purity) > 91 And Val(_Purity) < 92 Then
                                            If Val(FRANCHISEE_VALUEARY(7).ToString) <> 0 Then
                                                mxrate = Val(FRANCHISEE_VALUEARY(7).ToString)
                                            End If
                                        Else
                                            If Val(FRANCHISEE_VALUEARY(6).ToString) <> 0 Then
                                                mxrate = Val(FRANCHISEE_VALUEARY(6).ToString)
                                            End If
                                        End If
                                    ElseIf mitemrow(1).ToString = "S" Then
                                        If Val(FRANCHISEE_VALUEARY(9).ToString) <> 0 Then
                                            mxrate = Val(FRANCHISEE_VALUEARY(9).ToString)
                                        End If
                                    ElseIf mitemrow(1).ToString = "P" Then
                                        If Val(FRANCHISEE_VALUEARY(8).ToString) <> 0 Then
                                            mxrate = Val(FRANCHISEE_VALUEARY(8).ToString)
                                        End If
                                    End If
                                    Dim _CGROUPID As String = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT TOP 1 CGROUPID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE =(SELECT CATCODE FROM " & cnAdminDb & "..ITEMTAG WHERE SNO ='" & ro.Item("SNO").ToString & "')", , 0, tran).ToString)
                                    If mitemrow(1).ToString = "D" Then
                                        If FRANCHISEE_VALUEARY(5).Length = 2 Then
                                            Dim _tempspl() As String = FRANCHISEE_VALUEARY(5).Split(":")
                                            If _tempspl(0).ToString = _CGROUPID.ToString Then
                                                mxrate = Val(_tempspl(1).ToString)
                                            ElseIf Val(FRANCHISEE_VALUEARY(4).ToString) <> 0 Then
                                                mxrate = Val(FRANCHISEE_VALUEARY(4).ToString)
                                            End If
                                        ElseIf Val(FRANCHISEE_VALUEARY(4).ToString) <> 0 Then
                                            mxrate = Val(FRANCHISEE_VALUEARY(4).ToString)
                                        End If

                                    End If
                                    If mitemrow(1).ToString = "T" Then mxrate = Val(ro.Item("Rate").ToString) : mxrate = mxrate * (Val(FRANCHISEE_VALUEARY(3).ToString) / 100)

                                    mamount = mxrate * Val(ro.Item("GRSWT").ToString)
                                    mamount += McVal
                                    mamount += (mxrate * WastVal)
                                End If
                                If ro.Item("SALEMODE").ToString = "F" Or ro.Item("SALEMODE").ToString = "R" Or ro.Item("SALEMODE").ToString = "M" Then mamount = Val(ro.Item("SALVALUE").ToString)

                                Dim _dttempstone As DataTable
                                Dim _qry As String = ""
                                _qry = " SELECT (SELECT TOP 1 CGROUPID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE =I.CATCODE)CGROUPID,* FROM " & cnAdminDb & "..ITEMTAGSTONE A "
                                _qry += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID = A.STNITEMID WHERE A.TAGSNO = '" & ro.Item("SNO") & "'"
                                cmd = New OleDbCommand(_qry, XCnAdmin, tran)
                                da = New OleDbDataAdapter(cmd)
                                _dttempstone = New DataTable
                                da.Fill(_dttempstone)
                                Dim Stnamount As Decimal = 0
                                Dim Diaamount As Decimal = 0
                                If _dttempstone.Rows.Count > 0 Then
                                    For Each _row As DataRow In _dttempstone.Rows
                                        Dim _tempstone As Decimal = 0
                                        If _row("METALID").ToString = "T" Then
                                            If Val(FRANCHISEE_VALUEARY(3).ToString) <> 0 Then _tempstone = (Val(_row("STNAMT").ToString) * (Val(FRANCHISEE_VALUEARY(3).ToString) / 100))
                                            strSql = " UPDATE " & cnAdminDb & "..ITEMTAGSTONE SET TRFVALUE= " & _tempstone & " WHERE SNO ='" & _row.Item("SNO").ToString & "'"
                                            cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
                                            Stnamount += _tempstone
                                        End If
                                        If _row("METALID").ToString = "D" Then
                                            Dim _tempspl() As String = Nothing
                                            If FRANCHISEE_VALUEARY(5).ToString <> "" Then
                                                _tempspl = FRANCHISEE_VALUEARY(5).Split(":")
                                            End If
                                            If Not _tempspl Is Nothing Then
                                                If _tempspl.Length = 2 Then
                                                    If _tempspl(0).ToString = _row("CGROUPID").ToString Then
                                                        If Val(_tempspl(1).ToString) <> 0 Then
                                                            _tempstone = (Val(_row("STNWT").ToString) * Val(FRANCHISEE_VALUEARY(4).ToString))
                                                        Else
                                                            _tempstone = Val(_row("STNAMT").ToString)
                                                        End If
                                                    ElseIf Val(FRANCHISEE_VALUEARY(4).ToString) <> 0 Then
                                                        _tempstone = (Val(_row("STNWT").ToString) * Val(FRANCHISEE_VALUEARY(4).ToString))
                                                    Else
                                                        _tempstone = Val(_row("STNAMT").ToString)
                                                    End If
                                                ElseIf Val(FRANCHISEE_VALUEARY(4).ToString) <> 0 Then
                                                    _tempstone = (Val(_row("STNWT").ToString) * Val(FRANCHISEE_VALUEARY(4).ToString))
                                                Else
                                                    _tempstone = Val(_row("STNAMT").ToString)
                                                End If
                                            ElseIf Val(FRANCHISEE_VALUEARY(4).ToString) <> 0 Then
                                                _tempstone = (Val(_row("STNWT").ToString) * Val(FRANCHISEE_VALUEARY(4).ToString))
                                            Else
                                                _tempstone = Val(_row("STNAMT").ToString)
                                            End If
                                            strSql = " UPDATE " & cnAdminDb & "..ITEMTAGSTONE SET TRFVALUE= " & _tempstone & " WHERE SNO ='" & _row.Item("SNO").ToString & "'"
                                            cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
                                            Diaamount += _tempstone
                                        End If
                                    Next
                                End If
                                mamount = mamount + Stnamount + Diaamount
                                mamount = CalcRoundoffAmt(mamount, "H")
                                strSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET TRFVALUE= " & mamount & ",RATE=" & mxrate & " WHERE SNO ='" & ro.Item("SNO").ToString & "'"
                                cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()

                            Else
                                If ro.Item("SALEMODE").ToString = "W" Or ro.Item("SALEMODE").ToString = "B" Then
                                    Dim objWmc As New CLS_MAXMINVALUES("I", Val(ro.Item("GRSWT").ToString), xitemid, xsubitemid, toCostId)
                                    Dim WastVal As Double
                                    Dim McVal As Double
                                    If objWmc.pDtWmc.Rows.Count > 0 Then
                                        With objWmc.pDtWmc.Rows(0)
                                            WastVal = (Val(ro.Item("GRSWT").ToString) * Val(.Item("MAXWASTPER").ToString)) / 100
                                            McVal = (Val(ro.Item("GRSWT").ToString) * Val(.Item("MAXMCGRM").ToString))
                                        End With
                                    End If
                                    mamount = mxrate * Val(ro.Item("GRSWT").ToString)
                                    mamount += (Val(ro.Item("GRSWT").ToString) * McVal)
                                    mamount += (mxrate * WastVal)
                                End If
                                If ro.Item("SALEMODE").ToString = "F" Or ro.Item("SALEMODE").ToString = "R" Or ro.Item("SALEMODE").ToString = "M" Then mamount = Val(ro.Item("SALVALUE").ToString)
                                Dim Stnamount As Decimal = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID = A.STNITEMID WHERE A.TAGSNO = '" & ro.Item("SNO") & "' AND I.METALID='T' ", , 0, tran).ToString)
                                Dim Diaamount As Decimal = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID = A.STNITEMID WHERE A.TAGSNO = '" & ro.Item("SNO") & "' AND I.METALID='D' ", , 0, tran).ToString)
                                mamount = mamount + Stnamount + Diaamount
                                mamount = CalcRoundoffAmt(mamount, "H")
                                strSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET TRFVALUE= " & mamount & ",RATE=" & mxrate & " WHERE SNO ='" & ro.Item("SNO").ToString & "'"
                                cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
                                strSql = " UPDATE A SET A.TRFVALUE = ISNULL(A.STNAMT,0)  FROM " & cnAdminDb & "..ITEMTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON A.STNITEMID = I.ITEMID WHERE A.TAGSNO ='" & ro.Item("SNO").ToString & "' AND I.METALID = 'D'"
                                cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
                                strSql = " UPDATE A SET A.TRFVALUE = ISNULL(A.STNAMT,0)  FROM " & cnAdminDb & "..ITEMTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON A.STNITEMID = I.ITEMID WHERE A.TAGSNO ='" & ro.Item("SNO").ToString & "' AND I.METALID = 'T'"
                                cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
                            End If
                        End If

                        If ReplSubItemid Then
                            xitemid = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & ro.Item("Item").ToString & "'", , , tran).ToString)
                            xsubitemid = Val(BrighttechPack.GlobalMethods.GetSqlValue(XCnAdmin, "SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & ro.Item("SubItem").ToString & "' AND ITEMID = " & xitemid, , , tran).ToString)
                            strSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET SUBITEMID= " & xsubitemid & " WHERE SNO ='" & ro.Item("SNO").ToString & "'"
                            cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
                        End If

                        strSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET TRANSFERDATE= '" & BillDate.ToString & "' WHERE SNO ='" & ro.Item("SNO").ToString & "'"
                        cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
                        If (AUTOBOOKVALUEENABLE = "Y" Or STKTRAN_StoreAmt) And mamount <> 0 And _isFrachisee = False Then
                            strSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET TRFVALUE= " & mamount & " WHERE SNO ='" & ro.Item("SNO").ToString & "'"
                            cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
                            strSql = " UPDATE A SET A.TRFVALUE = ISNULL(A.STNAMT,0)+(ISNULL(A.STNAMT,0)*" & (AUTOBOOKVALUED_PER / 100) & ") FROM " & cnAdminDb & "..ITEMTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON A.STNITEMID = I.ITEMID WHERE A.TAGSNO ='" & ro.Item("SNO").ToString & "' AND I.METALID = 'D'"
                            cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
                            strSql = " UPDATE A SET A.TRFVALUE = ISNULL(A.STNAMT,0)+(ISNULL(A.STNAMT,0)*" & (AUTOBOOKVALUET_PER / 100) & ") FROM " & cnAdminDb & "..ITEMTAGSTONE A INNER JOIN " & cnAdminDb & "..ITEMMAST I ON A.STNITEMID = I.ITEMID WHERE A.TAGSNO ='" & ro.Item("SNO").ToString & "' AND I.METALID = 'T'"
                            cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
                        End If

                        If Not Isbulkupdate Then
                            obj = New TrasitIssRec(cnCostId, toCostId, "I", BillDate, ro.Item("SNO").ToString, XCnAdmin, tran, RefNo)
                            If _isFrachisee Then
                                If Not obj.InsertTagIssue(True, Val(ObjMaxMinValue.txtMaxWastage_Per.Text) _
                                , Val(ObjMaxMinValue.txtMinWastage_Per.Text) _
                                , Val(ObjMaxMinValue.txtMaxMcPerGram_Amt.Text) _
                                , Val(ObjMaxMinValue.txtMinMcPerGram_Amt.Text)) Then
                                    Continue For
                                End If
                            Else
                                If Not obj.InsertTagIssue(True) Then
                                    Continue For
                                End If
                            End If
                            TransSno += "'" & ro.Item("SNO").ToString & "',"
                            strSql = "INSERT INTO TRANS_STATUS (STOCKMODE,TAGSNO,STATUS,MOVETYPE) SELECT 'T','" & ro.Item("Sno").ToString & "','U','O'"
                            cmd = New OleDbCommand(strSql, XCnAdmin, tran)
                            cmd.ExecuteNonQuery()

                        End If
                        TransSno += "'" & ro.Item("SNO").ToString & "',"
                        If IS_IMAGE_TRF = True Then
                            If ro.Item("PCTFILE").ToString <> "" And defalutDestination <> "" Then
                                If IO.File.Exists(defalutDestination & ro.Item("PCTFILE").ToString) Then
                                    strSql = " INSERT INTO " & syncdb & "..SENDSYNC(FROMID,TOID"
                                    strSql += ",TAGIMAGE,TAGIMAGENAME,IMAGE_CTRLID"
                                    strSql += " )"
                                    strSql += " VALUES"
                                    strSql += " ('" & cnCostId & "','" & toCostId & "',?,?,'PICPATH')"
                                    cmd = New OleDbCommand(strSql, XCnAdmin, tran)
                                    Dim fileStr As New IO.FileStream(defalutDestination & ro.Item("PCTFILE").ToString, IO.FileMode.Open, IO.FileAccess.Read)
                                    Dim reader As New IO.BinaryReader(fileStr)
                                    Dim result As Byte() = reader.ReadBytes(CType(fileStr.Length, Integer))
                                    fileStr.Read(result, 0, result.Length)
                                    fileStr.Close()
                                    cmd.Parameters.AddWithValue("@TAGIMAGE", result)
                                    Dim fInfo As New IO.FileInfo(defalutDestination & ro.Item("PCTFILE").ToString)
                                    cmd.Parameters.AddWithValue("@TAGIMAGENAME", fInfo.Name)
                                    cmd.ExecuteNonQuery()
                                End If
                            End If
                        End If

                        strSql = "INSERT INTO " & cnStockDb & "..TRANSIT_AUDIT"
                        strSql += " (COSTID,REFNO,FROMID,TOID,TRANDATE,TAGSNO,ISSREC,STOCKTYPE)"
                        strSql += " SELECT '" & cnCostId & "','" & RefNo & "','" & cnCostId & "','" & toCostId & "','" & BillDate.ToString("yyyy-MM-dd") & "','" & ro.Item("SNO").ToString & "','I','T'"
                        cmd = New OleDbCommand(strSql, XCnAdmin, tran)
                        cmd.ExecuteNonQuery()

                        strSql = " EXEC " & cnAdminDb & "..SP_CCTRANSFER "
                        strSql += vbCrLf + " @TAGSNO = '" & ro.Item("SNO").ToString & "',@COSTID = '" & cnCostId & "',@ISSDATE = '" & BillDate.ToString("yyyy-MM-dd") & "',@ISSTIME=NULL,@VERSION='" & GlobalVariables.VERSION & "',@USERID = " & GlobalVariables.userId & ",@TRFNO='" & RefNo & "',@FLAG='I'"
                        cmd = New OleDbCommand(strSql, XCnAdmin, tran)
                        cmd.ExecuteNonQuery()
                    End If
                Next

            End If

            If NTransSno <> "" Then NTransSno = Mid(NTransSno, 1, NTransSno.Length - 1)
            If TransSno <> "" Then TransSno = Mid(TransSno, 1, TransSno.Length - 1)
            'strSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET REFNO = '" & RefNo & "',REFDATE = '" & BillDate.ToString("yyyy-MM-dd") & "'"
            'strSql += " WHERE SNO IN (" & TransSno & ")"
            'cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
            'If AUTOBOOKTRAN Then
            '    CreateInternalTransfer(TransSno, toCostId, RefNo, BillDate, NEWBILLNO)
            'End If


            strSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET REFNO = '" & RefNo & "',REFDATE = '" & BillDate.ToString("yyyy-MM-dd") & "'"
            strSql += " WHERE SNO IN (select TAGSNO FROM TRANS_STATUS WHERE STOCKMODE = 'T' AND STATUS ='U' AND MOVETYPE = 'O')"
            cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
            strSql = " UPDATE " & cnAdminDb & "..ITEMNONTAG SET REFNO = '" & RefNo & "',REFDATE = '" & BillDate.ToString("yyyy-MM-dd") & "'"
            strSql += " WHERE SNO IN (select TAGSNO FROM TRANS_STATUS WHERE STOCKMODE = 'N' AND STATUS ='U' AND MOVETYPE = 'O')"
            cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
            InsertExemptiondetails(RefNo, tran)
            If _isFrachisee Then
                CreateSalesInvoice(toCostId, RefNo, BillDate, NEWBILLNO, TransistNo, frmCostId, Toaccountid)
            ElseIf AUTOBOOKTRAN Then
                CreateInternalTransfernew(toCostId, RefNo, BillDate, NEWBILLNO, TransistNo, frmCostId, Toaccountid)
            End If
            GenerateSkuFile(XCnAdmin, tran, "", "", RefNo)
            strSql = " DELETE " & XSyncdb & "..TRANS_STATUS WHERE STATUS = 'U'  AND MOVETYPE = 'O'"
            cmd = New OleDbCommand(strSql, XCnAdmin, tran)
            cmd.ExecuteNonQuery()
            tran.Commit()
            tran = Nothing
            Dim pBatchno As String = Batchno
            Dim pBilldate As Date = BillDate
            btnNew_Click(Me, New EventArgs)
            If RefNo Is Nothing Or RefNo = "" Then
                MsgBox("Data Transfered,Please do the Syncronization." & vbCrLf & " TranNo no is: " & NEWBILLNO)
            Else
                MsgBox("Data Transfered,Please do the Syncronization." & vbCrLf & " Reference no is: " & RefNo)
            End If

            '''''
            If ChkTagTran Then
                If chkSelect.Checked = True Then
                    chkSelect.Checked = False
                End If
            Else
                chkSelect.Visible = False
            End If
            If STKTRAN_PRINTNEW Then
                PrintStockTransferNew(RefNo, pBilldate, "IIN")
            Else
                If GetAdmindbSoftValue("PRN_STKTRANSFER", "N") = "Y" Then
                    ''PrintStockTransfer(RefNo, pBilldate, "IIN")
                    If GetAdmindbSoftValue("PRN_STKTRANSFER_DETSUMM", "N") = "Y" Then
                        PrintStockTransfer_DETSUM(RefNo, pBilldate, "IIN")
                    Else
                        PrintStockTransfer(RefNo, pBilldate, "IIN")
                    End If
                End If
            End If
            If AUTOBOOKTRAN Or _isFrachisee Then
                Dim prnmemsuffix As String = ""
                If GetAdmindbSoftValuefromDt(dtSoftKeys, "PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                Dim BillPrintExe As Boolean = IIf(GetAdmindbSoftValue("CALLBILLPRINTEXE", "Y") = "Y", True, False)
                Dim BillPrint_Format As String = GetAdmindbSoftValue("BILLPRINT_FORMAT", "")
                If GST And BillPrint_Format.ToString = "M1" Then
                    Dim obj1 As New BrighttechREPORT.frmBillPrintDocA4N("POS", pBatchno.ToString, pBilldate.ToString("yyyy-MM-dd"), "N")
                    Me.Refresh()
                ElseIf GST And BillPrint_Format.ToString = "M2" Then
                    Dim obj1 As New BrighttechREPORT.frmBillPrintDocB5("POS", pBatchno.ToString, pBilldate.ToString("yyyy-MM-dd"), "N")
                    Me.Refresh()
                ElseIf GST And BillPrint_Format.ToString = "M3" Then
                    Dim obj1 As New BrighttechREPORT.frmBillPrintDocA5("POS", pBatchno.ToString, pBilldate.ToString("yyyy-MM-dd"), "N")
                    Me.Refresh()
                ElseIf GST And BillPrint_Format.ToString = "M4" Then
                    Dim obj1 As New BrighttechREPORT.frmBillPrintDocB52cpy("POS", pBatchno.ToString, pBilldate.ToString("yyyy-MM-dd"), "N")
                    Me.Refresh()
                ElseIf GST And BillPrintExe = False Then
                    Dim billDoc As New frmBillPrintDoc("POS", pBatchno.ToString, pBilldate.ToString("yyyy-MM-dd"), "N")
                    Me.Refresh()
                Else
                    If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                        ''Dim prnmemsuffix As String = ""
                        ''If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                        Dim memfile As String = "\BillPrint" + prnmemsuffix.Trim & ".mem"

                        Dim write As IO.StreamWriter
                        Dim Type As String = "SMI"
                        If _isFrachisee Then Type = "POS"
                        write = IO.File.CreateText(Application.StartupPath & memfile)
                        write.WriteLine(LSet("TYPE", 15) & ":" & Type)
                        write.WriteLine(LSet("BATCHNO", 15) & ":" & pBatchno)
                        write.WriteLine(LSet("TRANDATE", 15) & ":" & pBilldate.ToString("yyyy-MM-dd"))
                        write.WriteLine(LSet("DUPLICATE", 15) & ":N")
                        write.Flush()
                        write.Close()
                        If EXE_WITH_PARAM = False Then
                            System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                        Else
                            System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe",
                            LSet("TYPE", 15) & ":" & Type & ";" &
                            LSet("BATCHNO", 15) & ":" & pBatchno & ";" &
                            LSet("TRANDATE", 15) & ":" & pBilldate.ToString("yyyy-MM-dd") & ";" &
                            LSet("DUPLICATE", 15) & ":N")
                        End If
                    Else
                        MsgBox("Billprint exe not found", MsgBoxStyle.Information)
                    End If
                End If
            End If
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub InsertIntoNonTag(ByVal tCostId As String, ByVal TransCostId As String,
            ByVal itemid As Integer, ByVal subitemid As Integer, ByVal designid As Integer,
            ByVal pcs As Integer, ByVal grswt As Decimal, ByVal netwt As Decimal,
            ByVal refno As String, ByVal Tranno As Long, ByVal Pktno As String,
            ByVal Itemctrid As Integer, ByVal trfvalue As Double,
            ByVal sno As String, ByVal Oldsno As String, ByVal rw As DataRow)

        'Dim Sno As String = GetNewSnoNew(TranSnoType.ITEMNONTAGCODE, XCnAdmin, tran, "GET_ADMINSNO_TRAN")
        strSql = " INSERT INTO " & cnAdminDb & "..ITEMNONTAG"
        strSql += " ("
        strSql += " SNO,ITEMID,SUBITEMID,COMPANYID,RECDATE,"
        strSql += " PCS,GRSWT,LESSWT,NETWT,"
        strSql += " FINRATE,ISSTYPE,RECISS,POSTED,"
        strSql += " PACKETNO,DREFNO,ITEMCTRID,"
        strSql += " ORDREPNO,ORSNO,NARRATION,"
        strSql += " RATE,COSTID,"
        strSql += " CTGRM,DESIGNERID,ITEMTYPEID,"
        strSql += " CARRYFLAG,REASON,BATCHNO,CANCEL,"
        strSql += " USERID,UPDATED,UPTIME,SYSTEMID,APPVER,TCOSTID,REFNO,REFDATE,TRFVALUE,RECSNO)VALUES("
        strSql += " '" & sno & "'" 'SNO
        strSql += " ," & itemid
        strSql += " ," & subitemid
        strSql += " ,'" & GetStockCompId() & "'" 'Companyid
        strSql += " ,'" & GetEntryDate(dtpDate.Value, ).ToString("yyyy-MM-dd") & "'" 'Recdate
        strSql += " ," & pcs
        strSql += " ," & grswt
        strSql += " ," & grswt - netwt
        strSql += " ," & netwt
        strSql += " ,0" 'FinRate
        strSql += " ,'TR'" 'Isstype
        strSql += " ,'I'" 'RecIss
        strSql += " ,'C'" 'Posted
        strSql += " ,'" & Pktno & "'" 'Packetno
        strSql += " ,0" 'DRefno
        strSql += " ," & Itemctrid & "" '& Val(objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbItemCounter_MAN.Text & "'", , , tran)) & "" 'ItemCtrId
        strSql += " ,''" 'OrdRepNo
        strSql += " ,''" 'OrSNO
        strSql += " ,''" 'Narration
        strSql += " ,0" 'Rate
        strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, tCostId) & "'" 'COSTID
        strSql += " ,''"
        strSql += " ," & designid & ""
        '& Val(objGPack.GetSqlValue("SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & Designer & "'", , , tran)) & "" 'DesignerId
        strSql += " ,0"
        '& Val(objGPack.GetSqlValue("SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType_MAN.Text & "'", , , tran)) & "" 'ItemTypeID
        strSql += " ,''" 'Carryflag
        strSql += " ,'0'" 'Reason
        strSql += " ,''" 'BatchNo
        strSql += " ,''" 'Cancel
        strSql += " ," & userId & "" 'UserId
        strSql += " ,'" & GetEntryDate(GetServerDate(), ) & "'" 'Updated
        strSql += " ,'" & GetServerTime() & "'" 'Uptime
        strSql += " ,'" & systemId & "'" 'Systemid
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,'" & TransCostId & "'" 'TCOSTID
        strSql += " ,'" & refno & "'" 'REFNO
        strSql += " ,'" & GetEntryDate(dtpDate.Value, ).ToString("yyyy-MM-dd") & "'" 'REFDATE
        strSql += " ," & trfvalue  'TRFVALUE
        strSql += " ,'" & Oldsno & "'" 'RECSNO
        strSql += " )"
        cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()

        strSql = " INSERT INTO " & cnAdminDb & "..TITEMNONTAG"
        strSql += " ("
        strSql += " SNO,ITEMID,SUBITEMID,COMPANYID,RECDATE,"
        strSql += " PCS,GRSWT,LESSWT,NETWT,"
        strSql += " FINRATE,ISSTYPE,RECISS,POSTED,"
        strSql += " PACKETNO,DREFNO,ITEMCTRID,"
        strSql += " ORDREPNO,ORSNO,NARRATION,"
        strSql += " RATE,COSTID,"
        strSql += " CTGRM,DESIGNERID,ITEMTYPEID,"
        strSql += " CARRYFLAG,REASON,BATCHNO,CANCEL,"
        strSql += " USERID,UPDATED,UPTIME,SYSTEMID,APPVER,TCOSTID,REFNO,REFDATE,TRFVALUE)VALUES("
        strSql += " '" & sno & "'" 'SNO
        strSql += " ," & itemid
        strSql += " ," & subitemid
        strSql += " ,'" & GetStockCompId() & "'" 'Companyid
        strSql += " ,'" & GetEntryDate(dtpDate.Value, ).ToString("yyyy-MM-dd") & "'" 'Recdate
        strSql += " ," & pcs
        strSql += " ," & grswt
        strSql += " ," & grswt - netwt
        strSql += " ," & netwt
        strSql += " ,0" 'FinRate
        strSql += " ,'TR'" 'Isstype
        strSql += " ,'R'" 'RecIss
        strSql += " ,'C'" 'Posted
        strSql += " ,'" & Pktno & "'" 'Packetno
        strSql += " ,0" 'DRefno
        strSql += " ," & Itemctrid & "" '& Val(objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbItemCounter_MAN.Text & "'", , , tran)) & "" 'ItemCtrId
        strSql += " ,''" 'OrdRepNo
        strSql += " ,''" 'OrSNO
        strSql += " ,''" 'Narration
        strSql += " ,0" 'Rate
        strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, tCostId) & "'" 'IIf(tCostId = TransCostId, cnCostId, TransCostId) & "'" 'COSTID
        strSql += " ,''"
        strSql += " ," & designid '& Val(objGPack.GetSqlValue("SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesigner_MAN.Text & "'", , , tran)) & "" 'DesignerId
        strSql += " ,0" ' & Val(objGPack.GetSqlValue("SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType_MAN.Text & "'", , , tran)) & "" 'ItemTypeID
        strSql += " ,''" 'Carryflag
        strSql += " ,'0'" 'Reason
        strSql += " ,''" 'BatchNo
        strSql += " ,''" 'Cancel
        strSql += " ," & userId & "" 'UserId
        strSql += " ,'" & GetEntryDate(GetServerDate(), ) & "'" 'Updated
        strSql += " ,'" & GetServerTime() & "'" 'Uptime
        strSql += " ,'" & systemId & "'" 'Systemid
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,'" & TransCostId & "'" 'TCOSTID
        strSql += " ,'" & refno & "'" 'REFNO
        strSql += " ,'" & GetEntryDate(dtpDate.Value, ).ToString("yyyy-MM-dd") & "'" 'REFDATE
        strSql += " ," & trfvalue  'TRFVALUE
        strSql += " )"
        If Not CENTR_DB_GLB Then
            Exec(strSql.Replace("'", "''"), XCnAdmin, TransCostId, Nothing, tran)
            strSql = Replace(strSql, cnAdminDb & "..TITEMNONTAG", cnAdminDb & "..PITEMNONTAG")
            cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
        Else
            cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
        End If
        Dim StnPcs As Integer = 0
        Dim DiaPcs As Integer = 0
        Dim StnWt As Decimal = 0
        Dim DiaWt As Decimal = 0
        Dim StudItemid As Integer = 0
        Dim StudSubItemid As Integer = 0
        Dim StnRate As Decimal = 0
        Dim StnAmt As Decimal = 0
        Dim StnCalcMode As String = "W"
        Dim StnUnit As String = "C"
        If rw.Item("METALID").ToString = "" Then
            DiaPcs = Val(rw.Item("DIAPCS").ToString)
            DiaWt = Val(rw.Item("DIAWT").ToString)
            StnPcs = Val(rw.Item("STNPCS").ToString)
            StnWt = Val(rw.Item("STNWT").ToString)
            strSql = " SELECT STNITEMID,STNSUBITEMID,STNRATE,CALCMODE,STONEUNIT "
            strSql += " FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO='" & Oldsno & "'"
            strSql += " AND STNITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D')"
            Dim dr As DataRow = GetSqlRow(strSql, XCnAdmin, tran)
            If Not dr Is Nothing Then
                StudItemid = Val(dr.Item("STNITEMID").ToString)
                StudSubItemid = Val(dr.Item("STNSUBITEMID").ToString)
                StnRate = Val(dr.Item("STNRATE").ToString)
            End If
        End If
        If DiaPcs <> 0 Or DiaWt <> 0 Then
            strSql = " INSERT INTO " & cnAdminDb & "..ITEMNONTAGSTONE"
            strSql += " ("
            strSql += " SNO,TAGSNO,ITEMID,STNITEMID,STNSUBITEMID,COMPANYID,RECDATE,"
            strSql += " STNPCS,STNWT,STNRATE,STNAMT,"
            strSql += " RECISS,CALCMODE,STONEUNIT,"
            strSql += " COSTID,SYSTEMID,APPVER"
            strSql += " )VALUES("
            strSql += " '" & GetNewSnoNew(TranSnoType.ITEMNONTAGSTONECODE, XCnAdmin, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO
            strSql += " ,'" & sno & "'" 'TAGSNO
            strSql += " ," & itemid
            strSql += " ," & StudItemid
            strSql += " ," & StudSubItemid
            strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
            strSql += " ,'" & GetEntryDate(dtpDate.Value, ).ToString("yyyy-MM-dd") & "'" 'Recdate
            strSql += " ," & DiaPcs
            strSql += " ," & DiaWt
            strSql += " ," & StnRate
            strSql += " ," & (DiaWt * StnRate)
            strSql += " ,'I'" 'RECISS
            strSql += " ,'" & StnCalcMode & "'" 'CALCMODE
            strSql += " ,'" & StnUnit & "'" 'STONEUNIT
            strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, tCostId) & "'"  'COSTID
            strSql += " ,'" & systemId & "'" 'Systemid
            strSql += " ,'" & VERSION & "'" 'APPVER
            strSql += " )"
            cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()

            strSql = " INSERT INTO " & cnAdminDb & "..TITEMNONTAGSTONE"
            strSql += " ("
            strSql += " SNO,TAGSNO,ITEMID,STNITEMID,STNSUBITEMID,COMPANYID,RECDATE,"
            strSql += " STNPCS,STNWT,STNRATE,STNAMT,"
            strSql += " RECISS,CALCMODE,STONEUNIT,"
            strSql += " COSTID,SYSTEMID,APPVER"
            strSql += " )VALUES("
            strSql += " '" & GetNewSnoNew(TranSnoType.ITEMNONTAGSTONECODE, XCnAdmin, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO
            strSql += " ,'" & sno & "'" 'TAGSNO
            strSql += " ," & itemid
            strSql += " ," & StudItemid
            strSql += " ," & StudSubItemid
            strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
            strSql += " ,'" & GetEntryDate(dtpDate.Value, ).ToString("yyyy-MM-dd") & "'" 'Recdate
            strSql += " ," & DiaPcs
            strSql += " ," & DiaWt
            strSql += " ," & StnRate
            strSql += " ," & (DiaWt * StnRate)
            strSql += " ,'R'" 'RECISS
            strSql += " ,'" & StnCalcMode & "'" 'CALCMODE
            strSql += " ,'" & StnUnit & "'" 'STONEUNIT
            strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, tCostId) & "'"  'COSTID
            strSql += " ,'" & systemId & "'" 'Systemid
            strSql += " ,'" & VERSION & "'" 'APPVER
            strSql += " )"
            If Not CENTR_DB_GLB Then
                Exec(strSql.Replace("'", "''"), XCnAdmin, TransCostId, Nothing, tran)
                strSql = Replace(strSql, cnAdminDb & "..TITEMNONTAGSTONE", cnAdminDb & "..PITEMNONTAGSTONE")
                cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
            Else
                cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
            End If
        End If
        If StnPcs <> 0 Or StnWt <> 0 Then
            strSql = " SELECT STNITEMID,STNSUBITEMID,STNRATE,CALCMODE,STONEUNIT "
            strSql += " FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO='" & Oldsno & "'"
            strSql += " AND STNITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='S')"
            Dim dr As DataRow = GetSqlRow(strSql, XCnAdmin, tran)
            If Not dr Is Nothing Then
                StudItemid = Val(dr.Item("STNITEMID").ToString)
                StudSubItemid = Val(dr.Item("STNSUBITEMID").ToString)
                StnRate = Val(dr.Item("STNRATE").ToString)
            End If
            strSql = " INSERT INTO " & cnAdminDb & "..ITEMNONTAGSTONE"
            strSql += " ("
            strSql += " SNO,TAGSNO,ITEMID,STNITEMID,STNSUBITEMID,COMPANYID,RECDATE,"
            strSql += " STNPCS,STNWT,STNRATE,STNAMT,"
            strSql += " RECISS,CALCMODE,STONEUNIT,"
            strSql += " COSTID,SYSTEMID,APPVER"
            strSql += " )VALUES("
            strSql += " '" & GetNewSnoNew(TranSnoType.ITEMNONTAGSTONECODE, XCnAdmin, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO
            strSql += " ,'" & sno & "'" 'TAGSNO
            strSql += " ," & itemid
            strSql += " ," & StudItemid
            strSql += " ," & StudSubItemid
            strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
            strSql += " ,'" & GetEntryDate(dtpDate.Value, ).ToString("yyyy-MM-dd") & "'" 'Recdate
            strSql += " ," & StnPcs
            strSql += " ," & StnWt
            strSql += " ," & StnRate
            strSql += " ," & (StnWt * StnRate)
            strSql += " ,'I'" 'RECISS
            strSql += " ,'" & StnCalcMode & "'" 'CALCMODE
            strSql += " ,'" & StnUnit & "'" 'STONEUNIT
            strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, tCostId) & "'"  'COSTID
            strSql += " ,'" & systemId & "'" 'Systemid
            strSql += " ,'" & VERSION & "'" 'APPVER
            strSql += " )"
            cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
            strSql = " INSERT INTO " & cnAdminDb & "..TITEMNONTAGSTONE"
            strSql += " ("
            strSql += " SNO,TAGSNO,ITEMID,STNITEMID,STNSUBITEMID,COMPANYID,RECDATE,"
            strSql += " STNPCS,STNWT,STNRATE,STNAMT,"
            strSql += " RECISS,CALCMODE,STONEUNIT,"
            strSql += " COSTID,SYSTEMID,APPVER"
            strSql += " )VALUES("
            strSql += " '" & GetNewSnoNew(TranSnoType.ITEMNONTAGSTONECODE, XCnAdmin, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO
            strSql += " ,'" & sno & "'" 'TAGSNO
            strSql += " ," & itemid
            strSql += " ," & StudItemid
            strSql += " ," & StudSubItemid
            strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
            strSql += " ,'" & GetEntryDate(dtpDate.Value, ).ToString("yyyy-MM-dd") & "'" 'Recdate
            strSql += " ," & StnPcs
            strSql += " ," & StnWt
            strSql += " ," & StnRate
            strSql += " ," & (StnWt * StnRate)
            strSql += " ,'R'" 'RECISS
            strSql += " ,'" & StnCalcMode & "'" 'CALCMODE
            strSql += " ,'" & StnUnit & "'" 'STONEUNIT
            strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, tCostId) & "'"  'COSTID
            strSql += " ,'" & systemId & "'" 'Systemid
            strSql += " ,'" & VERSION & "'" 'APPVER
            strSql += " )"
            If Not CENTR_DB_GLB Then
                Exec(strSql.Replace("'", "''"), XCnAdmin, TransCostId, Nothing, tran)
                strSql = Replace(strSql, cnAdminDb & "..TITEMNONTAGSTONE", cnAdminDb & "..PITEMNONTAGSTONE")
                cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
            Else
                cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.ExecuteNonQuery()
            End If
        End If
    End Sub


    'Private Sub CreateInternalTransferForNonTag(ByVal TransSnos As String, ByVal ToCostId As String, ByVal RefNo As String, ByVal RefDate As Date, ByVal Tranno As Long)
    '    strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS"
    '    strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW"
    '    strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE"
    '    cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
    '    strSql = vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)AS SNO"
    '    strSql += vbCrLf + " ,'IIN' AS TRANTYPE "
    '    strSql += vbCrLf + " ,SUM(T.PCS)PCS,SUM(T.GRSWT)GRSWT,SUM(T.NETWT)NETWT,SUM(T.LESSWT)LESSWT,CONVERT(NUMERIC(15,3),(SUM(T.NETWT)*ISNULL((SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = CA.PURITYID),1))/100) AS PUREWT"
    '    strSql += vbCrLf + " ,ISNULL((SELECT ITEMID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = IM.CATCODE),0)AS ITEMID"
    '    strSql += vbCrLf + " ,ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = IM.CATCODE),0)AS SUBITEMID"
    '    strSql += vbCrLf + " ,'G' GRSNET,'" & cnCostId & "' COSTID,T.COMPANYID,'O'FLAG,(SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = CA.PURITYID) AS PURITY,IM.CATCODE,IM.CATCODE AS OCATCODE"
    '    strSql += vbCrLf + " ,ISNULL((SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & ToCostId & "'),'')AS ACCODE"
    '    strSql += vbCrLf + " ,IM.METALID,sum(trfvalue) as AMOUNT"
    '    strSql += vbCrLf + " ,53 ORDSTATE_ID"
    '    strSql += vbCrLf + " ,IDENTITY(INT,1,1)AS KEYNO"
    '    strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS FROM " & cnAdminDb & "..ITEMNONTAG AS T"
    '    strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
    '    strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE"
    '    strSql += vbCrLf + " WHERE T.SNO IN (" & TransSnos & ")"
    '    strSql += vbCrLf + " GROUP BY IM.CATCODE,T.COMPANYID,IM.METALID,CA.PURITYID"
    '    strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS SET SNO = KEYNO"
    '    strSql += vbCrLf + " "
    '    strSql += vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
    '    strSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
    '    strSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
    '    strSql += vbCrLf + " ,@CTLID = 'ISSUECODE'"
    '    strSql += vbCrLf + " ,@CHECK_TABLENAME = 'ISSUE'"
    '    strSql += vbCrLf + " ,@UPD_DBNAME = 'TEMPTABLEDB'"
    '    strSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP_" & systemId & "TRANTRANS'"
    '    strSql += vbCrLf + " "
    '    strSql += vbCrLf + " SELECT CONVERT(VARCHAR(15),NULL)ISSSNO"
    '    strSql += vbCrLf + " ,'IIN'TRANTYPE"
    '    strSql += vbCrLf + " ,STNPCS,STNWT,CONVERT(NUMERIC(15,2),0)AS STNRATE,CONVERT(NUMERIC(15,2),0)AS STNAMT"
    '    strSql += vbCrLf + " ,ISNULL((SELECT ITEMID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = IM.CATCODE),0)AS STNITEMID"
    '    strSql += vbCrLf + " ,ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = IM.CATCODE),0)AS STNSUBITEMID"
    '    strSql += vbCrLf + " ,'" & cnCostId & "' COSTID,TG.COMPANYID"
    '    strSql += vbCrLf + " ,TIM.CATCODE AS TCATCODE,TG.COMPANYID AS TCOMPANYID,CONVERT(NUMERIC(15,2),0) AS TPURITY,TIM.METALID AS TMETALID"
    '    strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW"
    '    strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE AS ST"
    '    strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMNONTAG AS TG ON TG.SNO = ST.TAGSNO"
    '    strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.ITEMID"
    '    strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS TIM ON TIM.ITEMID = TG.ITEMID"
    '    strSql += vbCrLf + " WHERE ST.TAGSNO IN (" & TransSnos & ")"

    '    strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW SET ISSSNO = T.SNO"
    '    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW AS SV"
    '    strSql += vbCrLf + " INNER JOIN TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS AS T ON T.CATCODE = SV.TCATCODE AND T.COMPANYID = SV.TCOMPANYID "
    '    strSql += vbCrLf + " AND T.PURITY = SV.TPURITY AND T.METALID = TMETALID"
    '    strSql += vbCrLf + " "
    '    strSql += vbCrLf + " SELECT "
    '    strSql += vbCrLf + " CONVERT(VARCHAR(15),NULL)SNO,ISSSNO,TRANTYPE,SUM(STNPCS)STNPCS,SUM(STNWT)STNWT"
    '    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0) AS STNRATE,CONVERT(NUMERIC(15,2),0) AS STNAMT"
    '    strSql += vbCrLf + " ,STNITEMID,STNSUBITEMID,TMETALID AS STONEMODE,COSTID,COMPANYID"
    '    strSql += vbCrLf + " ,IDENTITY(INT,1,1)KEYNO"
    '    strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE"
    '    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW "
    '    strSql += vbCrLf + " GROUP BY ISSSNO,TRANTYPE,STNITEMID,STNSUBITEMID,TMETALID,COSTID,COMPANYID"
    '    strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE SET SNO = KEYNO"
    '    strSql += vbCrLf + " "
    '    strSql += vbCrLf + " EXEC " & cnAdminDb & "..SP_UPDATE_SNO"
    '    strSql += vbCrLf + " @PROCNAME = 'GET_TRANSNO_TRAN'"
    '    strSql += vbCrLf + " ,@PROCDBNAME = '" & cnStockDb & "'"
    '    strSql += vbCrLf + " ,@CTLID = 'ISSSTONECODE'"
    '    strSql += vbCrLf + " ,@CHECK_TABLENAME = 'ISSSTONE'"
    '    strSql += vbCrLf + " ,@UPD_DBNAME = 'TEMPTABLEDB'"
    '    strSql += vbCrLf + " ,@UPD_TABLENAME = 'TEMP_" & systemId & "TRANTRANS_STONE'"
    '    cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

    '    Dim DtTag As New DataTable
    '    strSql = "  SELECT * FROM TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS"
    '    cmd = New OleDbCommand(strSql, XCnAdmin, tran)
    '    da = New OleDbDataAdapter(cmd)
    '    da.Fill(DtTag)

    '    Dim DtTagStone As New DataTable
    '    strSql = "  SELECT * FROM TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE"
    '    cmd = New OleDbCommand(strSql, XCnAdmin, tran)
    '    da = New OleDbDataAdapter(cmd)
    '    da.Fill(DtTagStone)

    '    strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS"
    '    strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE_VIEW"
    '    strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_" & systemId & "TRANTRANS_STONE"
    '    cmd = New OleDbCommand(strSql, XCnAdmin, tran) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

    '    Dim DtIssue As New DataTable
    '    Dim DtIssStone As New DataTable
    '    DtIssue = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, "ISSUE", XCnAdmin, tran)
    '    DtIssStone = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, "ISSSTONE", XCnAdmin, tran)
    '    Dim DtAcctran As New DataTable
    '    DtAcctran = BrighttechPack.GlobalMethods.GetTableStructure(cnStockDb, "ACCTRAN", XCnAdmin, tran)

    '    Dim RoIns As DataRow = Nothing
    '    For Each Ro As DataRow In DtTag.Rows
    '        RoIns = DtIssue.NewRow
    '        For Each Col As DataColumn In DtTag.Columns
    '            If DtIssue.Columns.Contains(Col.ColumnName) = False Then Continue For
    '            RoIns.Item(Col.ColumnName) = Ro.Item(Col.ColumnName)
    '        Next
    '        DtIssue.Rows.Add(RoIns)
    '    Next
    '    For Each Ro As DataRow In DtTagStone.Rows
    '        RoIns = DtIssStone.NewRow
    '        For Each Col As DataColumn In DtTagStone.Columns
    '            If DtIssStone.Columns.Contains(Col.ColumnName) = False Then Continue For
    '            RoIns.Item(Col.ColumnName) = Ro.Item(Col.ColumnName)
    '        Next
    '        DtIssStone.Rows.Add(RoIns)
    '    Next
    '    strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BILLNO')> 0"
    '    strSql += " DROP TABLE TEMP" & systemId & "BILLNO"
    '    cmd = New OleDbCommand(strSql, XCnAdmin, tran)
    '    cmd.ExecuteNonQuery()
    '    'TranNo = GetBillNoValue("GEN-SM-ISS", tran)
    '    ' Batchno = GetNewBatchno(cnCostId, BillDate, tran)
    '    For Each Ro As DataRow In DtIssue.Rows
    '        Ro.Item("TRANNO") = TranNo
    '        Ro.Item("TRANDATE") = BillDate
    '        Ro.Item("USERID") = userId
    '        Ro.Item("UPDATED") = GetServerDate()
    '        Ro.Item("UPTIME") = Now
    '        Ro.Item("APPVER") = VERSION
    '        Ro.Item("BATCHNO") = Batchno
    '        Ro.Item("REFNO") = RefNo
    '        Ro.Item("BAGNO") = TransistNo.ToString
    '        Ro.Item("REFDATE") = RefDate
    '        Ro.Item("REMARK1") = txtRemark1.Text
    '        Ro.Item("REMARK2") = txtRemark2.Text

    '        If AUTOBOOKVALUEENABLE = "Y" Then

    '            'Dim mxrate As Decimal = 0
    '            'mxrate = Val(GetRate(BillDate, Ro.Item("CATCODE").ToString, ))
    '            'If Ro.Item("METALID").ToString = "G" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUEG_PER / 100))
    '            'If Ro.Item("METALID").ToString = "S" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUES_PER / 100))
    '            'If Ro.Item("METALID").ToString = "P" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUEP_PER / 100))
    '            ''If Ro.Item("METALID").ToString = "G" Then mxrate = mxrate + (mxrate * (AUTOBOOKVALUEG_PER / 100))
    '            Dim mamount As Decimal = Val(Ro.Item("AMOUNT").ToString)
    '            'Dim Stnamount As Decimal = Val(DtIssStone.Compute("sum(stnamt)", "ISSSNO= '" & Ro.Item("SNO") & "' and STONEMODE NOT IN('D')").ToString)
    '            'Dim Diaamount As Decimal = Val(DtIssStone.Compute("sum(stnamt)", "ISSSNO= '" & Ro.Item("SNO") & "' and STONEMODE = 'D'").ToString)
    '            'If AUTOBOOKVALUET_PER <> 0 Then Stnamount = Stnamount + (Stnamount * (AUTOBOOKVALUET_PER / 100))
    '            'If AUTOBOOKVALUED_PER <> 0 Then Diaamount = Diaamount + (Diaamount * (AUTOBOOKVALUED_PER / 100))


    '            If mamount <> 0 Then

    '                If AUTOINTERNAL_VOUCHER = "Y" Then
    '                    Dim Roacct As DataRow = Nothing
    '                    Roacct = DtAcctran.NewRow
    '                    With Roacct
    '                        .Item("SNO") = GetNewSnoNew(TranSnoType.ACCTRANCODE, XCnAdmin, tran)
    '                        .Item("BATCHNO") = Batchno : .Item("REFNO") = RefNo
    '                        .Item("USERID") = userId : .Item("UPDATED") = GetServerDate() : .Item("APPVER") = VERSION
    '                        .Item("COMPANYID") = Ro.Item("COMPANYID") : .Item("COSTID") = cnCostId : .Item("FROMFLAG") = "A"
    '                        .Item("TRANNO") = Ro.Item("TRANNO") : .Item("TRANDATE") = Ro.Item("TRANDATE") : .Item("ACCODE") = Ro.Item("ACCODE") : .Item("TRANMODE") = "D" : .Item("PAYMODE") = "TI"
    '                        .Item("CONTRA") = "STKTRAN" : .Item("PCS") = Ro.Item("PCS") : .Item("GRSWT") = Ro.Item("GRSWT") : .Item("NETWT") = Ro.Item("NETWT") : .Item("AMOUNT") = mamount
    '                        .Item("REMARK1") = txtRemark1.Text
    '                        .Item("REMARK2") = txtRemark2.Text
    '                    End With
    '                    DtAcctran.Rows.Add(Roacct)
    '                    DtAcctran.AcceptChanges()
    '                    Roacct = DtAcctran.NewRow
    '                    With Roacct
    '                        .Item("SNO") = GetNewSnoNew(TranSnoType.ACCTRANCODE, XCnAdmin, tran)
    '                        .Item("BATCHNO") = Batchno : .Item("REFNO") = RefNo
    '                        .Item("USERID") = userId : .Item("UPDATED") = GetServerDate() : .Item("APPVER") = VERSION
    '                        .Item("COMPANYID") = Ro.Item("COMPANYID") : .Item("COSTID") = cnCostId : .Item("FROMFLAG") = "A"
    '                        .Item("TRANNO") = Ro.Item("TRANNO") : .Item("TRANDATE") = Ro.Item("TRANDATE") : .Item("ACCODE") = "STKTRAN" : .Item("TRANMODE") = "C" : .Item("PAYMODE") = "TI"
    '                        .Item("CONTRA") = Ro.Item("ACCODE") : .Item("PCS") = Ro.Item("PCS") : .Item("GRSWT") = Ro.Item("GRSWT") : .Item("NETWT") = Ro.Item("NETWT") : .Item("AMOUNT") = mamount
    '                        .Item("REMARK1") = txtRemark1.Text
    '                        .Item("REMARK2") = txtRemark2.Text
    '                    End With
    '                    DtAcctran.Rows.Add(Roacct)
    '                    DtAcctran.AcceptChanges()
    '                End If
    '            End If

    '        End If
    '    Next

    '    DtIssue.AcceptChanges()
    '    For Each Ro As DataRow In DtIssStone.Rows
    '        Ro.Item("TRANNO") = TranNo
    '        Ro.Item("TRANDATE") = BillDate
    '        Ro.Item("APPVER") = VERSION
    '        Ro.Item("BATCHNO") = Batchno
    '        Ro.Item("STONEMODE") = ""
    '    Next
    '    DtIssStone.AcceptChanges()
    '    InsertData(SyncMode.Transaction, DtIssue, XCnAdmin, tran, cnCostId)
    '    InsertData(SyncMode.Transaction, DtIssStone, XCnAdmin, tran, cnCostId)
    '    If AUTOINTERNAL_VOUCHER = "Y" Then InsertData(SyncMode.Transaction, DtAcctran, XCnAdmin, tran, cnCostId)
    'End Sub


    Private Sub txtItemId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtItemId.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadSalesItemName()
        End If
    End Sub

    Private Sub txtItemId_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtItemId.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim dritem As DataRow
            Dim barcode2d() As String = txtItemId.Text.Split(BARCODE2DSEP)
            If Not chkCheckByScan.Checked Then
                If barcode2d.Length > 2 Then
                    If Not Barcode2dfound(txtItemId.Text) Then MsgBox("Stock Not Available" & vbCrLf & "Please check data", MsgBoxStyle.Critical) : Exit Sub
                End If
                If Val(txtItemId.Text) <> 0 Then
                    dritem = GetSqlRow("SELECT STOCKTYPE,SUBITEM FROM " & cnAdminDb & "..ITEMMAST WHERE CONVERT(VARCHAR,ITEMID) = '" & Trim(txtItemId.Text) & "'", cn)
                    If dritem Is Nothing Then Exit Sub
                    If dritem(0).ToString = "N" Or dritem(0).ToString = "P" Then
                        ChkNontag.Checked = True
                    Else
                        ChkNontag.Checked = False
                    End If
                    If dritem(1).ToString = "Y" And ReplSubItemid Then
                        strSql = vbCrLf + " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = " & Val(txtItemId.Text.ToString)
                        strSql += vbCrLf + " ORDER BY SUBITEMNAME"
                        objGPack.FillCombo(strSql, cmbSubitem, True, False)
                    End If
                End If
                SendKeys.Send("{TAB}")
                Exit Sub
            Else
                If barcode2d.Length > 2 Then
                    If Not Barcode2dfound(txtItemId.Text) Then MsgBox("Stock Not Available" & vbCrLf & "Please check data", MsgBoxStyle.Critical) : Exit Sub
                End If
            End If


            Dim sp() As String = txtItemId.Text.Split(PRODTAGSEP)
            If PRODTAGSEP <> "" And txtItemId.Text <> "" Then
                sp = txtItemId.Text.Split(PRODTAGSEP)
                txtItemId.Text = Trim(sp(0))
            End If

            dritem = GetSqlRow("SELECT STOCKTYPE,SUBITEM FROM " & cnAdminDb & "..ITEMMAST WHERE CONVERT(VARCHAR,ITEMID) = '" & Trim(txtItemId.Text) & "'", cn)
            If Not dritem Is Nothing Then
                If dritem(0).ToString = "N" Or dritem(0).ToString = "P" Then
                    ChkNontag.Checked = True
                    Exit Sub
                End If
            End If

            If txtItemId.Text.StartsWith("#") Then txtItemId.Text = txtItemId.Text.Remove(0, 1)
CheckItem:
            If txtItemId.Text = "" Then
                LoadSalesItemName()
            ElseIf txtItemId.Text <> "" And objGPack.DupCheck("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE CONVERT(VARCHAR,ITEMID) = '" & Trim(txtItemId.Text) & "'") = False Then
                strSql = " SELECT ITEMID,TAGNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGKEY = '" & Trim(txtItemId.Text) & "'"
                Dim dtItemDet As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtItemDet)
                If dtItemDet.Rows.Count > 0 Then
                    txtItemId.Text = dtItemDet.Rows(0).Item("ITEMID").ToString
                    txtTagNo.Text = dtItemDet.Rows(0).Item("TAGNO").ToString
                    GoTo CheckItem
                Else
                    LoadSalesItemName()
                End If
            Else
                LoadSalesItemNameDetail()
            End If
            If sp.Length > 1 And PRODTAGSEP <> "" And txtItemId.Text <> "" Then
                txtTagNo.Text = Trim(sp(1))
            End If
            If txtTagNo.Text <> "" Then
                txtTagNo.Focus()
                txtTagNo_KeyPress(Me, New KeyPressEventArgs(e.KeyChar))
            End If
        End If
    End Sub
    Private Function Barcode2dfound(ByVal barcode2dstring As String) As Boolean
        Dim foundstnstr As Integer = InStr(barcode2dstring, "BEGINSTN")
        Dim barcode2d1 As String
        If foundstnstr <> 0 Then barcode2d1 = Mid(barcode2dstring, 1, foundstnstr - 1) Else barcode2d1 = barcode2dstring
        Dim barcode2darray1() As String = barcode2d1.Split(objSoftKeys.BARCODE2DSEP)
        strSql = " SELECT ITEMID,TAGNO,PCS FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID = " & Val(barcode2darray1(0).ToString) & " AND TAGNO = '" & barcode2darray1(2).ToString & "'"
        Dim dtItemDet As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtItemDet)
        If dtItemDet.Rows.Count > 0 Then
            txtItemId.Text = barcode2darray1(0).ToString
            txtTagNo.Text = barcode2darray1(2).ToString
            Return True
        Else
            Return False
        End If
    End Function


    Private Sub LoadSalesItemName()
        strSql = " SELECT"
        strSql += " ITEMID AS ITEMID,ITEMNAME AS ITEMNAME,"
        strSql += " CASE WHEN CALTYPE = 'W' THEN 'WEIGHT'"
        strSql += " WHEN CALTYPE = 'W' THEN 'WEIGHT'"
        strSql += " WHEN CALTYPE = 'R' THEN 'RATE'"
        strSql += " WHEN CALTYPE = 'F' THEN 'FIXED'"
        strSql += " WHEN CALTYPE = 'B' THEN 'BOTH' ELSE 'METAL RATE' END AS CAL_TYPE,"
        strSql += " CASE WHEN STOCKTYPE = 'T' THEN 'TAGED' "
        strSql += " WHEN STOCKTYPE = 'N' THEN 'NON TAGED' ELSE 'PACKET BASED' END AS STOCKTYPE"
        strSql += " FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ISNULL(STUDDED,'') <> 'S' AND ACTIVE = 'Y'" '' AND STOCKTYPE = 'T'"

        Dim itemId As Integer = Val(BrighttechPack.SearchDialog.Show("Find ItemName", strSql, cn, 1, , , txtItemId.Text))
        If itemId > 0 Then

            txtItemId.Text = itemId
            If objGPack.GetSqlValue("select STOCKTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID =" & itemId) = "N" Then Exit Sub
            LoadSalesItemNameDetail()
        Else
            txtItemId.Focus()
            txtItemId.SelectAll()
        End If
    End Sub

    Private Sub LoadSalesSubItemName()
        strSql = " SELECT"
        strSql += " SUBITEMID AS SUBITEMID,SUBITEMNAME AS SUBITEMNAME,"
        strSql += " CASE WHEN CALTYPE = 'W' THEN 'WEIGHT'"
        strSql += " WHEN CALTYPE = 'W' THEN 'WEIGHT'"
        strSql += " WHEN CALTYPE = 'R' THEN 'RATE'"
        strSql += " WHEN CALTYPE = 'F' THEN 'FIXED'"
        strSql += " WHEN CALTYPE = 'B' THEN 'BOTH' ELSE 'METAL RATE' END AS CAL_TYPE"
        'strSql += " ,CASE WHEN STOCKTYPE = 'T' THEN 'TAGED' "
        'strSql += " WHEN STOCKTYPE = 'N' THEN 'NON TAGED' ELSE 'PACKET BASED' END AS STOCKTYPE"
        strSql += " FROM " & cnAdminDb & "..SUBITEMMAST"
        strSql += " WHERE ACTIVE = 'Y'" '' AND STOCKTYPE = 'T'"
        strSql += " AND ITEMID = '" + txtItemId.Text + "'"

        Dim SubitemId As Integer = Val(BrighttechPack.SearchDialog.Show("Find SubItemName", strSql, cn, 1, , , txtSubItemID.Text))
        If SubitemId > 0 Then
            txtSubItemID.Text = SubitemId
            'If objGPack.GetSqlValue("select STOCKTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID =" & SubitemId) = "N" Then Exit Sub
            LoadSalesSubItemNameDetail()
        Else
            txtSubItemID.Focus()
            txtSubItemID.SelectAll()
        End If
    End Sub

    Private Sub LoadSalesItemNameDetail()
        If txtItemId.Text = "" Then
            Exit Sub
        Else
            Me.SelectNextControl(txtItemId, True, True, True, True)
        End If
    End Sub
    Private Sub LoadSalesSubItemNameDetail()
        If txtSubItemID.Text = "" Then
            Exit Sub
        Else
            Me.SelectNextControl(txtSubItemID, True, True, True, True)
        End If
    End Sub
    Dim str As String = ""
    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        str = ""
        If txtLotNo_NUM.Text <> "" Then str = "LOT NO : " & txtLotNo_NUM.Text
        BrightPosting.GExport.Post(Me.Name, strCompanyName, "Tag Transfer " & cmbTagsCostName_MAN.Text & " " & str, gridView, BrightPosting.GExport.GExportType.Export)
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        str = ""
        If txtLotNo_NUM.Text <> "" Then str = "LOT NO : " & txtLotNo_NUM.Text
        BrightPosting.GExport.Post(Me.Name, strCompanyName, "Tag Transfer " & cmbTagsCostName_MAN.Text & " " & str, gridView, BrightPosting.GExport.GExportType.Print)
    End Sub

    Private Sub ResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResizeToolStripMenuItem.Click
        If gridView.RowCount > 0 Then
            If ResizeToolStripMenuItem.Checked Then
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridView.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
            'gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End If
    End Sub
    Private Sub funcloadNonTagDetailsSnoWise()
        'Load Nontag summary balance
        Dim validateColName As String = ""
        Dim Sno As String = ""
        Dim selMetalId As String = ""
        Dim STONEDIAMETAL As String = "'D','T'"
        If cmbMetal.Text <> "ALL" And cmbMetal.Text <> "" And cmbMetal.Enabled = True Then
            Dim sql As String = "SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(cmbMetal.Text) & ")"
            Dim dtMetal As New DataTable()
            da = New OleDbDataAdapter(sql, cn)
            da.Fill(dtMetal)
            If dtMetal.Rows.Count > 0 Then
                For j As Integer = 0 To dtMetal.Rows.Count - 1
                    selMetalId += "'"
                    selMetalId += dtMetal.Rows(j).Item("METALID").ToString + ""
                    selMetalId += "'"
                    selMetalId += ","
                Next
                If selMetalId <> "" Then
                    selMetalId = Mid(selMetalId, 1, selMetalId.Length - 1)
                End If
            End If
        End If
        'validateColName = "SNO"
        Dim mtagcostid As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbTagsCostName_MAN.Text & "'")

        strSql = vbCrLf + " SELECT SNO,ITEM,SUBITEM,PACKETNO AS TAGNO,DESIGNERID,SUM(CASE WHEN RECISS='R' THEN PCS ELSE (-1)*PCS END) PCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN RECISS='R' THEN GRSWT ELSE (-1)* GRSWT END)GRSWT,SUM(CASE WHEN RECISS='R' THEN LESSWT ELSE (-1)* LESSWT END) LESSWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN RECISS='R' THEN NETWT ELSE (-1)* NETWT END) NETWT "
        strSql += vbCrLf + " ,SUM(CASE WHEN RECISS='R' THEN STNPCS ELSE (-1)* STNPCS END) STNPCS,SUM(CASE WHEN RECISS='R' THEN STNWT ELSE (-1)* STNWT END) STNWT "
        strSql += vbCrLf + " ,SUM(CASE WHEN RECISS='R' THEN PSTNPCS ELSE (-1)*PSTNPCS END)PSTNPCS,SUM(CASE WHEN RECISS='R' THEN PSTNWT ELSE (-1)* PSTNWT END) PSTNWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN RECISS='R' THEN DIAPCS ELSE (-1)* DIAPCS END)DIAPCS,SUM(CASE WHEN RECISS='R' THEN DIAWT ELSE (-1)* DIAWT END) DIAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN RECISS='R' THEN SALVALUE ELSE (-1)* SALVALUE END) SALVALUE ,COUNTER,STOCKMODE FROM ("
        strSql += vbCrLf + "  SELECT  CASE WHEN RECISS='R' THEN T.SNO ELSE '' END  AS SNO,IM.ITEMNAME AS ITEM,ISNULL(SM.SUBITEMNAME,'') AS SUBITEM,PACKETNO PACKETNO,T.PCS,T.GRSWT,LESSWT,NETWT "
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='S')),0) STNPCS"
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='S')),0) STNWT"
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='P')),0) PSTNPCS"
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='P')),0) PSTNWT"
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D')),0) DIAPCS"
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D')),0) DIAWT"
        strSql += vbCrLf + " ,0 SALVALUE,IC.ITEMCTRNAME AS COUNTER,'N' STOCKMODE,RECISS,T.DESIGNERID"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAG T"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON T.ITEMID=IM.ITEMID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SM ON T.ITEMID=SM.ITEMID AND T.SUBITEMID=SM.SUBITEMID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER IC ON T.ITEMCTRID=IC.ITEMCTRID"
        strSql += vbCrLf + " WHERE ISNULL(T.COSTID,'') = '" & cnCostId & "'"
        'strSql += vbCrLf + " WHERE (ISNULL(REFNO,'') = '' OR (ISSTYPE ='TR' AND REFNO <> ''  AND ISNULL(T.COSTID,'') = '" & cnCostId & "'))"
        If chkRecDate.Checked = True Then strSql += vbCrLf + " AND RECDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' "
        strSql += vbCrLf + " AND RECISS='R' AND T.TCOSTID ='" & mtagcostid & "'"
        If txtItemId.Text.Trim <> "" Then strSql += vbCrLf + " AND T.ITEMID = " & Val(txtItemId.Text)
        If selMetalId <> "" Then strSql += " AND IM.METALID IN (" & selMetalId & ")"
        strSql += vbCrLf + " AND T.COMPANYID = '" & GetStockCompId() & "'"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        'strSql += vbCrLf + " AND METALID NOT IN(" & STONEDIAMETAL & ")"
        strSql += vbCrLf + " AND ISNULL(IM.STUDDED,'') NOT IN ('S')"
        If txtLotNo_NUM.Text.Trim <> "" Then strSql += vbCrLf + " AND LOTNO='" & txtLotNo_NUM.Text.Trim & "'"

        ''''''''''''''''''''''''''''''''''Commented On 2019-01-11 For Call Manapally call-Id = 27149''''''''''''''''''''''
        'strSql += vbCrLf + " UNION ALL "
        'strSql += vbCrLf + " SELECT  CASE WHEN RECISS='R' THEN T.SNO ELSE '' END  AS SNO,IM.ITEMNAME AS ITEM,ISNULL(SM.SUBITEMNAME,'') AS SUBITEM,PACKETNO PACKETNO,0 PCS,0 GRSWT,0 LESSWT,0 NETWT "
        'strSql += vbCrLf + ",CASE WHEN IM.DIASTONE = 'S' THEN T.PCS ELSE 0 END STNPCS,CASE WHEN IM.DIASTONE = 'S' THEN T.GRSWT ELSE 0 END STNWT"
        'strSql += vbCrLf + ",CASE WHEN IM.DIASTONE = 'P' THEN T.PCS ELSE 0 END PSTNPCS,CASE WHEN IM.DIASTONE = 'P' THEN T.GRSWT ELSE 0 END PSTNWT"
        'strSql += vbCrLf + ",CASE WHEN IM.DIASTONE = 'D' THEN T.PCS ELSE 0 END DIAPCS,CASE WHEN IM.DIASTONE = 'D' THEN T.GRSWT ELSE 0 END DIAWT"
        'strSql += vbCrLf + " ,0 SALVALUE,IC.ITEMCTRNAME AS COUNTER,'N' STOCKMODE,RECISS,T.DESIGNERID"
        'strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAG T"
        'strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON T.ITEMID=IM.ITEMID"
        'strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SM ON T.ITEMID=SM.ITEMID AND T.SUBITEMID=SM.SUBITEMID"
        'strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER IC ON T.ITEMCTRID=IC.ITEMCTRID"
        'strSql += vbCrLf + " WHERE ISNULL(T.COSTID,'') = '" & cnCostId & "'"
        ''strSql += vbCrLf + " WHERE (ISNULL(REFNO,'') = '' OR (ISSTYPE ='TR' AND REFNO <> ''  AND ISNULL(T.COSTID,'') = '" & cnCostId & "'))"
        'If chkRecDate.Checked = True Then strSql += vbCrLf + " AND RECDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' "
        'strSql += vbCrLf + " AND RECISS='R' AND T.TCOSTID ='" & mtagcostid & "'"
        'If txtItemId.Text.Trim <> "" Then strSql += vbCrLf + " AND T.ITEMID = " & Val(txtItemId.Text)
        'If selMetalId <> "" Then strSql += " AND IM.METALID IN (" & selMetalId & ")"
        'strSql += vbCrLf + " AND T.COMPANYID = '" & GetStockCompId() & "'"
        'strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        ''strSql += vbCrLf + " AND METALID IN(" & STONEDIAMETAL & ")"
        'strSql += vbCrLf + " AND ISNULL(IM.STUDDED,'') NOT IN ('S')"
        'If txtLotNo_NUM.Text.Trim <> "" Then strSql += vbCrLf + " AND LOTNO='" & txtLotNo_NUM.Text.Trim & "'"
        'strSql += vbCrLf + " UNION ALL "
        'strSql += vbCrLf + " SELECT  CASE WHEN T1.RECISS='R' THEN T1.SNO ELSE '' END  AS SNO,IM.ITEMNAME AS ITEM,ISNULL(SM.SUBITEMNAME,'') AS SUBITEM,T.PACKETNO PACKETNO,0 PCS,0 GRSWT,0 LESSWT,0 NETWT "
        'strSql += vbCrLf + ",CASE WHEN IM.DIASTONE = 'S' THEN T.PCS ELSE 0 END STNPCS,CASE WHEN IM.DIASTONE = 'S' THEN T.GRSWT ELSE 0 END STNWT"
        'strSql += vbCrLf + ",CASE WHEN IM.DIASTONE = 'P' THEN T.PCS ELSE 0 END PSTNPCS,CASE WHEN IM.DIASTONE = 'P' THEN T.GRSWT ELSE 0 END PSTNWT"
        'strSql += vbCrLf + ",CASE WHEN IM.DIASTONE = 'D' THEN T.PCS ELSE 0 END DIAPCS,CASE WHEN IM.DIASTONE = 'D' THEN T.GRSWT ELSE 0 END DIAWT"
        'strSql += vbCrLf + " ,0 SALVALUE,IC.ITEMCTRNAME AS COUNTER,'N' STOCKMODE,T.RECISS,T.DESIGNERID"
        'strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAG T"
        'strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMNONTAG AS T1 ON T.RECSNO=T1.SNO "
        'strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON T.ITEMID=IM.ITEMID"
        'strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SM ON T.ITEMID=SM.ITEMID AND T.SUBITEMID=SM.SUBITEMID"
        'strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER IC ON T.ITEMCTRID=IC.ITEMCTRID"
        'strSql += vbCrLf + " WHERE ISNULL(T.COSTID,'') = '" & cnCostId & "'"
        'strSql += vbCrLf + " AND T.RECISS='I' AND T1.RECISS='R'"
        'If chkRecDate.Checked = True Then strSql += vbCrLf + " AND T1.RECDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' "
        'If txtItemId.Text.Trim <> "" Then strSql += vbCrLf + " AND T.ITEMID = " & Val(txtItemId.Text)
        'If selMetalId <> "" Then strSql += " AND IM.METALID IN (" & selMetalId & ")"
        'strSql += vbCrLf + " AND T.COMPANYID = '" & GetStockCompId() & "'"
        'strSql += vbCrLf + " AND ISNULL(T.CANCEL,'') = ''"
        ''strSql += vbCrLf + " AND METALID IN(" & STONEDIAMETAL & ")"
        'strSql += vbCrLf + " AND ISNULL(IM.STUDDED,'') NOT IN ('S')"
        'If txtLotNo_NUM.Text.Trim <> "" Then strSql += vbCrLf + " AND T.LOTNO='" & txtLotNo_NUM.Text.Trim & "'"
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        strSql += vbCrLf + " UNION ALL "
        strSql += vbCrLf + "  SELECT  CASE WHEN T1.RECISS='R' THEN T1.SNO ELSE '' END  AS SNO,IM.ITEMNAME AS ITEM,ISNULL(SM.SUBITEMNAME,'') AS SUBITEM,T.PACKETNO PACKETNO,T.PCS,T.GRSWT,T.LESSWT,T.NETWT "
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='S')),0) STNPCS"
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='S')),0) STNWT"
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='P')),0) PSTNPCS"
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='P')),0) PSTNWT"
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D')),0) DIAPCS"
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D')),0) DIAWT"
        strSql += vbCrLf + " ,0 SALVALUE,IC.ITEMCTRNAME AS COUNTER,'N' STOCKMODE,T.RECISS,T.DESIGNERID"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAG T"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMNONTAG AS T1 ON T.RECSNO=T1.SNO "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON T.ITEMID=IM.ITEMID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SM ON T.ITEMID=SM.ITEMID AND T.SUBITEMID=SM.SUBITEMID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER IC ON T.ITEMCTRID=IC.ITEMCTRID"
        strSql += vbCrLf + " WHERE ISNULL(T.COSTID,'') = '" & cnCostId & "'"
        strSql += vbCrLf + " AND T.RECISS='I' AND T1.RECISS='R' "
        If chkRecDate.Checked = True Then strSql += vbCrLf + " AND T1.RECDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' "
        If txtItemId.Text.Trim <> "" Then strSql += vbCrLf + " AND T.ITEMID = " & Val(txtItemId.Text)
        If selMetalId <> "" Then strSql += " AND IM.METALID IN (" & selMetalId & ")"
        strSql += vbCrLf + " AND T.COMPANYID = '" & GetStockCompId() & "'"
        strSql += vbCrLf + " AND ISNULL(T.CANCEL,'') = ''"
        'strSql += vbCrLf + " AND METALID NOT IN(" & STONEDIAMETAL & ")"
        strSql += vbCrLf + " AND ISNULL(IM.STUDDED,'') NOT IN ('S')"
        If txtLotNo_NUM.Text.Trim <> "" Then strSql += vbCrLf + " AND T1.LOTNO='" & txtLotNo_NUM.Text.Trim & "'"
        strSql += vbCrLf + " ) X GROUP BY ITEM,SUBITEM,PACKETNO,COUNTER,STOCKMODE,DESIGNERID"
        strSql += vbCrLf + ",SNO"
        strSql += vbCrLf + " HAVING (SUM(CASE WHEN RECISS='R' THEN PCS ELSE (-1)*PCS END) >0 OR SUM(CASE WHEN RECISS='R' THEN GRSWT ELSE (-1)* GRSWT END)>0"
        strSql += vbCrLf + " OR SUM(CASE WHEN RECISS='R' THEN STNPCS ELSE (-1)*STNPCS END) >0 OR SUM(CASE WHEN RECISS='R' THEN STNWT ELSE (-1)* STNWT END)>0"
        strSql += vbCrLf + " OR SUM(CASE WHEN RECISS='R' THEN PSTNPCS ELSE (-1)*PSTNPCS END) >0 OR SUM(CASE WHEN RECISS='R' THEN PSTNWT ELSE (-1)* PSTNWT END)>0"
        strSql += vbCrLf + " OR SUM(CASE WHEN RECISS='R' THEN DIAPCS ELSE (-1)*DIAPCS END) >0 OR SUM(CASE WHEN RECISS='R' THEN DIAWT ELSE (-1)* DIAWT END)>0)"
        strSql += vbCrLf + " ORDER BY SNO"
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("CHECK", GetType(Boolean))
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        dtGrid.Columns("SNO").Caption = "VISIBLE_FALSE"
        Dim editcollist As String = "PCS,GRSWT,NETWT,STNPCS,STNWT,PSTNPCS,PSTNWT,DIAPCS,DIAWT,LESSWT"
        objMultiSelect = New MultiSelectRowDia(dtGrid, validateColName, editcollist)
        objMultiSelect.txtSearchTagNo.Visible = False

        objMultiSelect.chkAppSales.Visible = False
        objMultiSelect.DateCheck = False
        If dtGrid.Rows.Count > 0 Then
            If objMultiSelect.ShowDialog = Windows.Forms.DialogResult.OK Then
                Me.Refresh()
                Sno = ""
                Dim TotPcs As Integer = 0
                Dim TotGrwt As Double = 0
                Dim TotNetwt As Double = 0

                For Each Ro As DataRow In objMultiSelect.RowSelected
                    Sno += "'" & Ro.Item("SNO").ToString & "',"
                    TotPcs += Val(Ro.Item("PCS").ToString)
                    TotGrwt += Val(Ro.Item("Grswt").ToString)
                    TotNetwt += Val(Ro.Item("Netwt").ToString)
                Next

                Dim dtt As New DataTable
                dtt = CType(objMultiSelect.Dgv.DataSource, DataTable).Copy
                Dim ros() As DataRow = Nothing
                ros = dtt.Select("CHECK = TRUE")


                If Sno <> "" Then Sno = Mid(Sno, 1, Sno.Length - 1)
                strSql = " SELECT  T.SNO AS SNO,METALID,IM.ITEMNAME AS ITEM,ISNULL(SM.SUBITEMNAME,'') AS SUBITEM,PACKETNO TAGNO," & TotPcs.ToString & "PCS," & TotGrwt.ToString & " GRSWT,LESSWT," & TotNetwt.ToString & " NETWT "
                strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='S')),0) STNPCS"
                strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='S')),0) STNWT"
                strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='P')),0) PSTNPCS"
                strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='P')),0) PSTNWT"
                strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D')),0) DIAPCS"
                strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D')),0) DIAWT"
                strSql += vbCrLf + " ,0 SALVALUE"
                strSql += vbCrLf + " ,IC.ITEMCTRNAME AS COUNTER,DESIGNERID"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(3),NULL)COLHEAD,'' PCTFILE,'N' AS STOCKMODE"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAG T"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON T.ITEMID=IM.ITEMID"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SM ON T.ITEMID=SM.ITEMID AND T.SUBITEMID=SM.SUBITEMID"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER IC ON T.ITEMCTRID=IC.ITEMCTRID"
                strSql += vbCrLf + " WHERE 1=2 "

                If (cmbCounter_OWN.Text = "" Or cmbCounter_OWN.Text <> "ALL") And dtpDate.Enabled = False And Val(txtItemId.Text) = 0 And txtTagNo.Text = "" And Val(txtLotNo_NUM.Text) = 0 Then
                    If MessageBox.Show("Do you want to load all tag items?", "Tag Transfer", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
                        Exit Sub
                    End If
                End If
                Dim dtTemp As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtTemp)
                If Not ros.Length > 0 Then
                    MsgBox("Record not found", MsgBoxStyle.Information)
                    txtTagNo.Focus()
                    If chkCheckByScan.Checked Then
                        txtTagNo.Focus()
                    End If
                    If Val(txtEstNo.Text) <> 0 Then txtEstNo.Select()
                    Exit Sub
                ElseIf ros.Length > 0 Then
                    ChkNontag.Focus()
                End If
                For Each ro As DataRow In ros
                    If ro.Item("STOCKMODE").ToString <> "T" Then
                        If Isgridexist(ro.Item("SNO").ToString, ro.Item("STOCKMODE").ToString) = False Then
                            dtGridView.ImportRow(ro)
                        End If
                    Else
                        Dim RowCol() As DataRow = dtGridView.Select("SNO = '" & ro.Item("SNO").ToString & "'")
                        If RowCol.Length = 0 Then
                            dtGridView.ImportRow(ro)
                        End If
                    End If
                Next
                dtGridView.AcceptChanges()
                gridView.DataSource = dtTemp
                With gridView
                    If ChkTagTran Then
                        If gridView.Columns.Contains("checkBoxColumn") Then
                        Else
                            Dim checkBoxColumn As New DataGridViewCheckBoxColumn()
                            checkBoxColumn.HeaderText = ""
                            checkBoxColumn.Width = 30
                            checkBoxColumn.Name = "checkBoxColumn"
                            checkBoxColumn.TrueValue = 1
                            Dim iCount As Integer = gridView.Columns.Count
                            gridView.Columns.Insert(0, checkBoxColumn)
                        End If
                    Else
                        'chkSelect.Visible = False
                    End If
                    For cnt As Integer = 0 To .ColumnCount - 1
                        .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
                    Next
                    .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("LESSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("SALVALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("STNPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("STNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("PSTNPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("PSTNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("DIAPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("DIAWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("GRSWT").DefaultCellStyle.Format = "0.000"
                    .Columns("LESSWT").DefaultCellStyle.Format = "0.000"
                    .Columns("NETWT").DefaultCellStyle.Format = "0.000"
                    .Columns("STNWT").DefaultCellStyle.Format = "0.000"
                    .Columns("PSTNWT").DefaultCellStyle.Format = "0.000"
                    .Columns("DIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
                    .Columns("SALVALUE").DefaultCellStyle.Format = "0.00"
                    .Columns("COLHEAD").Visible = False
                    .Columns("SALVALUE").Visible = False
                    .Columns("SNO").Visible = False
                    .Columns("PCTFILE").Visible = False

                End With
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
                AddGrandTotal()
                gridView.Sort(gridView.Columns("COLHEAD"), System.ComponentModel.ListSortDirection.Ascending)
                cmbCostCentre_MAN.Enabled = False
                If chkCheckByScan.Checked Then
                    txtTagNo.Clear()
                    txtItemId.Select()
                    If Val(txtEstNo.Text) <> 0 Then txtEstNo.Select()
                End If
            End If
        End If
    End Sub
    Private Sub loadNonTagDetailsNew()
        'Load Nontag summary balance
        Dim validateColName As String = ""
        Dim Sno As String = ""
        Dim selMetalId As String = ""
        Dim STONEDIAMETAL As String = "'D','T'"
        If cmbMetal.Text <> "ALL" And cmbMetal.Text <> "" And cmbMetal.Enabled = True Then
            Dim sql As String = "SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(cmbMetal.Text) & ")"
            Dim dtMetal As New DataTable()
            da = New OleDbDataAdapter(sql, cn)
            da.Fill(dtMetal)
            If dtMetal.Rows.Count > 0 Then
                For j As Integer = 0 To dtMetal.Rows.Count - 1
                    selMetalId += "'"
                    selMetalId += dtMetal.Rows(j).Item("METALID").ToString + ""
                    selMetalId += "'"
                    selMetalId += ","
                Next
                If selMetalId <> "" Then
                    selMetalId = Mid(selMetalId, 1, selMetalId.Length - 1)
                End If
            End If
        End If
        'validateColName = "SNO"
        Dim mtocostid As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'")
        Dim mtagcostid As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbTagsCostName_MAN.Text & "'")

        strSql = vbCrLf + " SELECT MAX(SNO) AS SNO,ITEM,SUBITEM,PACKETNO AS TAGNO,DESIGNERID,SUM(CASE WHEN RECISS='R' THEN PCS ELSE (-1)*PCS END) PCS"
        strSql += vbCrLf + " ,SUM(CASE WHEN RECISS='R' THEN GRSWT ELSE (-1)* GRSWT END)GRSWT,SUM(CASE WHEN RECISS='R' THEN LESSWT ELSE (-1)* LESSWT END) LESSWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN RECISS='R' THEN NETWT ELSE (-1)* NETWT END) NETWT "
        strSql += vbCrLf + " ,SUM(CASE WHEN RECISS='R' THEN STNPCS ELSE (-1)* STNPCS END) STNPCS,SUM(CASE WHEN RECISS='R' THEN STNWT ELSE (-1)* STNWT END) STNWT "
        strSql += vbCrLf + " ,SUM(CASE WHEN RECISS='R' THEN PSTNPCS ELSE (-1)*PSTNPCS END)PSTNPCS,SUM(CASE WHEN RECISS='R' THEN PSTNWT ELSE (-1)* PSTNWT END) PSTNWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN RECISS='R' THEN DIAPCS ELSE (-1)* DIAPCS END)DIAPCS,SUM(CASE WHEN RECISS='R' THEN DIAWT ELSE (-1)* DIAWT END) DIAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN RECISS='R' THEN SALVALUE ELSE (-1)* SALVALUE END) SALVALUE ,COUNTER,STOCKMODE "
        strSql += vbCrLf + " ,CASE WHEN STKTYPE='M' THEN 'MANUFACTURING' WHEN STKTYPE='E' THEN 'EXEMPTED' ELSE 'TRADING' END AS STKTYPE FROM ("
        strSql += vbCrLf + "  SELECT  CASE WHEN RECISS='R' THEN T.SNO ELSE '' END  AS SNO,IM.ITEMNAME AS ITEM,ISNULL(SM.SUBITEMNAME,'') AS SUBITEM,PACKETNO PACKETNO,T.PCS,T.GRSWT,LESSWT,NETWT "
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='S')),0) STNPCS"
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='S')),0) STNWT"
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='P')),0) PSTNPCS"
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='P')),0) PSTNWT"
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D')),0) DIAPCS"
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D')),0) DIAWT"
        strSql += vbCrLf + " ,0 SALVALUE,IC.ITEMCTRNAME AS COUNTER,'N' STOCKMODE,RECISS,T.DESIGNERID,STKTYPE"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAG T"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON T.ITEMID=IM.ITEMID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SM ON T.ITEMID=SM.ITEMID AND T.SUBITEMID=SM.SUBITEMID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER IC ON T.ITEMCTRID=IC.ITEMCTRID"
        strSql += vbCrLf + " WHERE ISNULL(T.COSTID,'') = '" & cnCostId & "'"
        'strSql += vbCrLf + " WHERE (ISNULL(REFNO,'') = '' OR (ISSTYPE ='TR' AND REFNO <> ''  AND ISNULL(T.COSTID,'') = '" & cnCostId & "'))"
        If chkRecDate.Checked = True Then strSql += vbCrLf + " AND RECDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' AND RECISS='R' AND T.TCOSTID ='" & mtagcostid & "'"
        If txtItemId.Text.Trim <> "" Then strSql += vbCrLf + " AND T.ITEMID = " & Val(txtItemId.Text)
        If selMetalId <> "" Then strSql += " AND IM.METALID IN (" & selMetalId & ")"
        'strSql += vbCrLf + " AND T.TCOSTID ='" & mtagcostid & "'"
        strSql += vbCrLf + " AND T.COMPANYID = '" & GetStockCompId() & "'"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        'strSql += vbCrLf + " AND METALID NOT IN(" & STONEDIAMETAL & ")"
        strSql += vbCrLf + " AND ISNULL(IM.STUDDED,'') NOT IN ('S')"
        If txtLotNo_NUM.Text.Trim <> "" Then strSql += vbCrLf + " AND LOTNO='" & txtLotNo_NUM.Text.Trim & "'"
        If CmbStockType.Text <> "ALL" Then
            If CmbStockType.Text = "MANUFACTURING" Then
                strSql += vbCrLf + " AND T.STKTYPE ='M'"
            ElseIf CmbStockType.Text = "EXEMPTED" Then
                strSql += vbCrLf + " AND T.STKTYPE ='E'"
            Else
                strSql += vbCrLf + " AND ISNULL(T.STKTYPE,'') NOT IN('M','E') "
            End If
        End If
        ''''''''''''''''''''''''''''''''''Commented On 2019-01-11 For Call Manapally call-Id = 27149''''''''''''''''''''''
        'strSql += vbCrLf + " UNION ALL "
        'strSql += vbCrLf + " SELECT  CASE WHEN RECISS='R' THEN T.SNO ELSE '' END  AS SNO,IM.ITEMNAME AS ITEM,ISNULL(SM.SUBITEMNAME,'') AS SUBITEM,PACKETNO PACKETNO,0 PCS,0 GRSWT,0 LESSWT,0 NETWT "
        'strSql += vbCrLf + ",CASE WHEN IM.DIASTONE = 'S' THEN T.PCS ELSE 0 END STNPCS,CASE WHEN IM.DIASTONE = 'S' THEN T.GRSWT ELSE 0 END STNWT"
        'strSql += vbCrLf + ",CASE WHEN IM.DIASTONE = 'P' THEN T.PCS ELSE 0 END PSTNPCS,CASE WHEN IM.DIASTONE = 'P' THEN T.GRSWT ELSE 0 END PSTNWT"
        'strSql += vbCrLf + ",CASE WHEN IM.DIASTONE = 'D' THEN T.PCS ELSE 0 END DIAPCS,CASE WHEN IM.DIASTONE = 'D' THEN T.GRSWT ELSE 0 END DIAWT"
        'strSql += vbCrLf + " ,0 SALVALUE,IC.ITEMCTRNAME AS COUNTER,'N' STOCKMODE,RECISS,T.DESIGNERID,STKTYPE"
        'strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAG T"
        'strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON T.ITEMID=IM.ITEMID"
        'strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SM ON T.ITEMID=SM.ITEMID AND T.SUBITEMID=SM.SUBITEMID"
        'strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER IC ON T.ITEMCTRID=IC.ITEMCTRID"
        'strSql += vbCrLf + " WHERE ISNULL(T.COSTID,'') = '" & cnCostId & "'"
        ''strSql += vbCrLf + " WHERE (ISNULL(REFNO,'') = '' OR (ISSTYPE ='TR' AND REFNO <> ''  AND ISNULL(T.COSTID,'') = '" & cnCostId & "'))"
        'If chkRecDate.Checked = True Then strSql += vbCrLf + " AND RECDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' AND RECISS='R' AND T.TCOSTID ='" & mtagcostid & "'"
        'If txtItemId.Text.Trim <> "" Then strSql += vbCrLf + " AND T.ITEMID = " & Val(txtItemId.Text)
        'If selMetalId <> "" Then strSql += " AND IM.METALID IN (" & selMetalId & ")"
        ''strSql += vbCrLf + " AND T.TCOSTID ='" & mtagcostid & "'"
        'strSql += vbCrLf + " AND T.COMPANYID = '" & GetStockCompId() & "'"
        'strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        ''strSql += vbCrLf + " AND METALID IN(" & STONEDIAMETAL & ")"
        'strSql += vbCrLf + " AND ISNULL(IM.STUDDED,'') NOT IN ('S')"
        'If txtLotNo_NUM.Text.Trim <> "" Then strSql += vbCrLf + " AND LOTNO='" & txtLotNo_NUM.Text.Trim & "'"
        'If CmbStockType.Text <> "ALL" Then
        '    If CmbStockType.Text = "MANUFACTURING" Then
        '        strSql += vbCrLf + " AND T.STKTYPE ='M'"
        '    ElseIf CmbStockType.Text = "EXEMPTED" Then
        '        strSql += vbCrLf + " AND T.STKTYPE ='E'"
        '    Else
        '        strSql += vbCrLf + " AND ISNULL(T.STKTYPE,'') NOT IN('M','E') "
        '    End If
        'End If
        If chkRecDate.Checked Or txtLotNo_NUM.Text.Trim <> "" Then
            strSql += vbCrLf + " UNION ALL "
            strSql += vbCrLf + "  SELECT  CASE WHEN T1.RECISS='R' THEN T1.SNO ELSE '' END  AS SNO,IM.ITEMNAME AS ITEM,ISNULL(SM.SUBITEMNAME,'') AS SUBITEM,T.PACKETNO PACKETNO,T.PCS,T.GRSWT,T.LESSWT,T.NETWT "
            strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='S')),0) STNPCS"
            strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='S')),0) STNWT"
            strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='P')),0) PSTNPCS"
            strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='P')),0) PSTNWT"
            strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D')),0) DIAPCS"
            strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D')),0) DIAWT"
            strSql += vbCrLf + " ,0 SALVALUE,IC.ITEMCTRNAME AS COUNTER,'N' STOCKMODE,T.RECISS,T.DESIGNERID,T.STKTYPE"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAG T"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMNONTAG AS T1 ON T.RECSNO=T1.SNO "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON T.ITEMID=IM.ITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SM ON T.ITEMID=SM.ITEMID AND T.SUBITEMID=SM.SUBITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER IC ON T.ITEMCTRID=IC.ITEMCTRID"
            strSql += vbCrLf + " WHERE ISNULL(T.COSTID,'') = '" & cnCostId & "'"
            strSql += vbCrLf + " AND T.RECISS='I' AND T1.RECISS='R' "
            If chkRecDate.Checked = True Then strSql += vbCrLf + " AND T1.RECDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' "
            strSql += vbCrLf + " AND T.TCOSTID ='" & mtocostid & "'"
            If txtItemId.Text.Trim <> "" Then strSql += vbCrLf + " AND T.ITEMID = " & Val(txtItemId.Text)
            If selMetalId <> "" Then strSql += " AND IM.METALID IN (" & selMetalId & ")"
            strSql += vbCrLf + " AND T.COMPANYID = '" & GetStockCompId() & "'"
            strSql += vbCrLf + " AND ISNULL(T.CANCEL,'') = ''"
            'strSql += vbCrLf + " AND METALID NOT IN(" & STONEDIAMETAL & ")"
            strSql += vbCrLf + " AND ISNULL(IM.STUDDED,'') NOT IN ('S')"
            If txtLotNo_NUM.Text.Trim <> "" Then strSql += vbCrLf + " AND T1.LOTNO='" & txtLotNo_NUM.Text.Trim & "'"
            If CmbStockType.Text <> "ALL" Then
                If CmbStockType.Text = "MANUFACTURING" Then
                    strSql += vbCrLf + " AND T.STKTYPE ='M'"
                ElseIf CmbStockType.Text = "EXEMPTED" Then
                    strSql += vbCrLf + " AND T.STKTYPE ='E'"
                Else
                    strSql += vbCrLf + " AND ISNULL(T.STKTYPE,'') NOT IN('M','E') "
                End If
            End If
            ''''''''''''''''''''''''''''''''''Commented On 2019-01-11 For Call Manapally call-Id = 27149''''''''''''''''''''''
            'strSql += vbCrLf + " UNION ALL "
            'strSql += vbCrLf + " SELECT  CASE WHEN T1.RECISS='R' THEN T1.SNO ELSE '' END  AS SNO,IM.ITEMNAME AS ITEM,ISNULL(SM.SUBITEMNAME,'') AS SUBITEM,T.PACKETNO PACKETNO,0 PCS,0 GRSWT,0 LESSWT,0 NETWT "
            'strSql += vbCrLf + ",CASE WHEN IM.DIASTONE = 'S' THEN T.PCS ELSE 0 END STNPCS,CASE WHEN IM.DIASTONE = 'S' THEN T.GRSWT ELSE 0 END STNWT"
            'strSql += vbCrLf + ",CASE WHEN IM.DIASTONE = 'P' THEN T.PCS ELSE 0 END PSTNPCS,CASE WHEN IM.DIASTONE = 'P' THEN T.GRSWT ELSE 0 END PSTNWT"
            'strSql += vbCrLf + ",CASE WHEN IM.DIASTONE = 'D' THEN T.PCS ELSE 0 END DIAPCS,CASE WHEN IM.DIASTONE = 'D' THEN T.GRSWT ELSE 0 END DIAWT"
            'strSql += vbCrLf + " ,0 SALVALUE,IC.ITEMCTRNAME AS COUNTER,'N' STOCKMODE,T.RECISS,T.DESIGNERID,T.STKTYPE"
            'strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAG T"
            'strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMNONTAG AS T1 ON T.RECSNO=T1.SNO "
            'strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON T.ITEMID=IM.ITEMID"
            'strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SM ON T.ITEMID=SM.ITEMID AND T.SUBITEMID=SM.SUBITEMID"
            'strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER IC ON T.ITEMCTRID=IC.ITEMCTRID"
            'strSql += vbCrLf + " WHERE ISNULL(T.COSTID,'') = '" & cnCostId & "'"
            'strSql += vbCrLf + " AND T.RECISS='I' AND T1.RECISS='R'"
            'If chkRecDate.Checked = True Then strSql += vbCrLf + " AND T1.RECDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "' "
            'strSql += vbCrLf + " AND T.TCOSTID ='" & mtocostid & "'"
            'If txtItemId.Text.Trim <> "" Then strSql += vbCrLf + " AND T.ITEMID = " & Val(txtItemId.Text)
            'If selMetalId <> "" Then strSql += " AND IM.METALID IN (" & selMetalId & ")"
            'strSql += vbCrLf + " AND T.COMPANYID = '" & GetStockCompId() & "'"
            'strSql += vbCrLf + " AND ISNULL(T.CANCEL,'') = ''"
            ''strSql += vbCrLf + " AND METALID IN(" & STONEDIAMETAL & ")"
            'strSql += vbCrLf + " AND ISNULL(IM.STUDDED,'') NOT IN ('S')"
            'If txtLotNo_NUM.Text.Trim <> "" Then strSql += vbCrLf + " AND T.LOTNO='" & txtLotNo_NUM.Text.Trim & "'"
            'If CmbStockType.Text <> "ALL" Then
            '    If CmbStockType.Text = "MANUFACTURING" Then
            '        strSql += vbCrLf + " AND T.STKTYPE ='M'"
            '    ElseIf CmbStockType.Text = "EXEMPTED" Then
            '        strSql += vbCrLf + " AND T.STKTYPE ='E'"
            '    Else
            '        strSql += vbCrLf + " AND ISNULL(T.STKTYPE,'') NOT IN('M','E') "
            '    End If
            'End If
        End If
        strSql += vbCrLf + " ) X GROUP BY ITEM,SUBITEM,PACKETNO,COUNTER,STOCKMODE,DESIGNERID,STKTYPE"
        If chkRecDate.Checked = True Then strSql += vbCrLf + ",SNO"
        strSql += vbCrLf + " HAVING (SUM(CASE WHEN RECISS='R' THEN PCS ELSE (-1)*PCS END) >0 OR SUM(CASE WHEN RECISS='R' THEN GRSWT ELSE (-1)* GRSWT END)>0"
        strSql += vbCrLf + " OR SUM(CASE WHEN RECISS='R' THEN STNPCS ELSE (-1)*STNPCS END) >0 OR SUM(CASE WHEN RECISS='R' THEN STNWT ELSE (-1)* STNWT END)>0"
        strSql += vbCrLf + " OR SUM(CASE WHEN RECISS='R' THEN PSTNPCS ELSE (-1)*PSTNPCS END) >0 OR SUM(CASE WHEN RECISS='R' THEN PSTNWT ELSE (-1)* PSTNWT END)>0"
        strSql += vbCrLf + " OR SUM(CASE WHEN RECISS='R' THEN DIAPCS ELSE (-1)*DIAPCS END) >0 OR SUM(CASE WHEN RECISS='R' THEN DIAWT ELSE (-1)* DIAWT END)>0)"

        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("CHECK", GetType(Boolean))
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        dtGrid.Columns("SNO").Caption = "VISIBLE_FALSE"
        Dim editcollist As String = "PCS,GRSWT,NETWT,STNPCS,STNWT,PSTNPCS,PSTNWT,DIAPCS,DIAWT,LESSWT"
        objMultiSelect = New MultiSelectRowDia(dtGrid, validateColName, editcollist)
        objMultiSelect.txtSearchTagNo.Visible = False

        objMultiSelect.chkAppSales.Visible = False
        objMultiSelect.DateCheck = False
        If dtGrid.Rows.Count > 0 Then
            If objMultiSelect.ShowDialog = Windows.Forms.DialogResult.OK Then
                Me.Refresh()
                Sno = ""
                Dim TotPcs As Integer = 0
                Dim TotGrwt As Double = 0
                Dim TotNetwt As Double = 0

                For Each Ro As DataRow In objMultiSelect.RowSelected
                    Sno += "'" & Ro.Item("SNO").ToString & "',"
                    TotPcs += Val(Ro.Item("PCS").ToString)
                    TotGrwt += Val(Ro.Item("Grswt").ToString)
                    TotNetwt += Val(Ro.Item("Netwt").ToString)
                Next

                Dim dtt As New DataTable
                dtt = CType(objMultiSelect.Dgv.DataSource, DataTable).Copy
                Dim ros() As DataRow = Nothing
                ros = dtt.Select("CHECK = TRUE")


                If Sno <> "" Then Sno = Mid(Sno, 1, Sno.Length - 1)
                strSql = " SELECT  T.SNO AS SNO,METALID,IM.ITEMNAME AS ITEM,ISNULL(SM.SUBITEMNAME,'') AS SUBITEM,PACKETNO TAGNO," & TotPcs.ToString & "PCS," & TotGrwt.ToString & " GRSWT,LESSWT," & TotNetwt.ToString & " NETWT "
                strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='S')),0) STNPCS"
                strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='S')),0) STNWT"
                strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='P')),0) PSTNPCS"
                strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='P')),0) PSTNWT"
                strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D')),0) DIAPCS"
                strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D')),0) DIAWT"
                strSql += vbCrLf + " ,0 SALVALUE"
                strSql += vbCrLf + " ,IC.ITEMCTRNAME AS COUNTER,DESIGNERID"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(3),NULL)COLHEAD,'' PCTFILE,'N' AS STOCKMODE"
                strSql += vbCrLf + " ,CASE WHEN STKTYPE='M' THEN 'MANUFACTURING' WHEN STKTYPE='E' THEN 'EXEMPTED' ELSE 'TRADING' END AS STKTYPE "
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAG T"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON T.ITEMID=IM.ITEMID"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SM ON T.ITEMID=SM.ITEMID AND T.SUBITEMID=SM.SUBITEMID"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER IC ON T.ITEMCTRID=IC.ITEMCTRID"
                strSql += vbCrLf + " WHERE 1=2 "

                If (cmbCounter_OWN.Text = "" Or cmbCounter_OWN.Text <> "ALL") And dtpDate.Enabled = False And Val(txtItemId.Text) = 0 And txtTagNo.Text = "" And Val(txtLotNo_NUM.Text) = 0 Then
                    If MessageBox.Show("Do you want to load all tag items?", "Tag Transfer", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
                        Exit Sub
                    End If
                End If
                Dim dtTemp As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtTemp)

                'If Not dtTemp.Rows.Count > 0 Then
                '    MsgBox("Record not found", MsgBoxStyle.Information)
                '    txtTagNo.Focus()
                '    If chkCheckByScan.Checked Then
                '        txtTagNo.Focus()
                '    End If
                '    Exit Sub
                '    'ElseIf dtTemp.Rows.Count > 0 Then
                '    '    If grpNonTag.Visible Then txtItemId.Focus() Else txtTagNo.Focus()
                'Else
                '    If ChkTagTran Then
                '        chkSelect.Visible = True
                '    Else
                '        chkSelect.Visible = False
                '    End If
                'End If
                If Not ros.Length > 0 Then
                    MsgBox("Record not found", MsgBoxStyle.Information)
                    txtTagNo.Focus()
                    If chkCheckByScan.Checked Then
                        txtTagNo.Focus()
                    End If
                    If Val(txtEstNo.Text) <> 0 Then txtEstNo.Select()
                    Exit Sub
                ElseIf ros.Length > 0 Then
                    ChkNontag.Focus()
                End If
                For Each ro As DataRow In ros
                    If ro.Item("STOCKMODE").ToString <> "T" Then
                        If Isgridexist(ro.Item("SNO").ToString, ro.Item("STOCKMODE").ToString) = False Then
                            dtGridView.ImportRow(ro)
                        End If
                    Else
                        Dim RowCol() As DataRow = dtGridView.Select("SNO = '" & ro.Item("SNO").ToString & "'")
                        If RowCol.Length = 0 Then
                            dtGridView.ImportRow(ro)
                        End If
                    End If
                Next
                dtGridView.AcceptChanges()
                gridView.DataSource = dtTemp
                With gridView
                    If ChkTagTran Then
                        If gridView.Columns.Contains("checkBoxColumn") Then
                        Else
                            Dim checkBoxColumn As New DataGridViewCheckBoxColumn()
                            checkBoxColumn.HeaderText = ""
                            checkBoxColumn.Width = 30
                            checkBoxColumn.Name = "checkBoxColumn"
                            checkBoxColumn.TrueValue = 1
                            Dim iCount As Integer = gridView.Columns.Count
                            gridView.Columns.Insert(0, checkBoxColumn)
                        End If

                    Else
                        'chkSelect.Visible = False
                    End If
                    For cnt As Integer = 0 To .ColumnCount - 1
                        .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
                    Next
                    .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("LESSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("SALVALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("STNPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("STNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("PSTNPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("PSTNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("DIAPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("DIAWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("GRSWT").DefaultCellStyle.Format = "0.000"
                    .Columns("LESSWT").DefaultCellStyle.Format = "0.000"
                    .Columns("NETWT").DefaultCellStyle.Format = "0.000"
                    .Columns("STNWT").DefaultCellStyle.Format = "0.000"
                    .Columns("PSTNWT").DefaultCellStyle.Format = "0.000"
                    .Columns("DIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
                    .Columns("SALVALUE").DefaultCellStyle.Format = "0.00"
                    .Columns("COLHEAD").Visible = False
                    .Columns("SALVALUE").Visible = False
                    .Columns("SNO").Visible = False
                    .Columns("PCTFILE").Visible = False
                    If .Columns.Contains("SGST") Then .Columns("SGST").Visible = False
                    If .Columns.Contains("CGST") Then .Columns("CGST").Visible = False
                    If .Columns.Contains("IGST") Then .Columns("IGST").Visible = False
                End With
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
                AddGrandTotal()
                gridView.Sort(gridView.Columns("COLHEAD"), System.ComponentModel.ListSortDirection.Ascending)
                cmbCostCentre_MAN.Enabled = False
                If chkCheckByScan.Checked Then
                    txtTagNo.Clear()
                    txtItemId.Select()
                    If Val(txtEstNo.Text) <> 0 Then txtEstNo.Select()
                End If
            End If
        Else

        End If
        LoadTransactionType()
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim selMetalId As String = Nothing
        Dim selCounterId As String = Nothing
        If objGPack.Validator_Check(Me) Then Exit Sub
        If IsFilterRestrict Then
            If Trim(txtTagNo.Text) = "" And Trim(txtLotNo_NUM.Text) = "" Then
                If Trim(txtTagNo.Text) = "" Then
                    MsgBox("TagNo No Should Not be Empty", MsgBoxStyle.Information)
                    txtTagNo.Focus()
                    Exit Sub
                End If
                If Trim(txtLotNo_NUM.Text) = "" Then
                    MsgBox("Lot No Should Not be Empty", MsgBoxStyle.Information)
                    txtTagNo.Focus()
                    Exit Sub
                End If
            End If
        End If
        If StateId = 0 Then
            MsgBox("Please Update State for the Account [" & CmbAcname.Text & "]", MsgBoxStyle.Information)
            Exit Sub
        End If
        If IsSearchKeyRestrict Then
            If cmbSearchKey.Text.Trim = "" Then
                MsgBox("Search Key Should Not be Empty", MsgBoxStyle.Information)
                cmbSearchKey.Focus()
                Exit Sub
            ElseIf txtSearch.Text.Trim = "" Then
                MsgBox("Search Text Should Not be Empty", MsgBoxStyle.Information)
                txtSearch.Focus()
                Exit Sub
            End If
        End If
        _isFrachisee = False
        If REQ_FRANCHISEE Then
            strSql = "SELECT COSTTYPE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre_MAN.Text & "'"
            If objGPack.GetSqlValue(strSql, "COSTTYPE", "B").ToString = "F" Then
                _isFrachisee = True
            End If
        End If

        If REQ_FRANCHISEE = True And _isFrachisee Then
            If FRANCHISEE_VALUEARY.Length <> 10 Then MsgBox("Please Reset the value <FRANCHISEE_VALUE> ex(N,0,0,0,0,0,0,0,0,0)") : Me.Close()
        End If

        If cmbMetal.Text = "ALL" Then
            strSql = " SELECT METALID FROM " & cnAdminDb & "..METALMAST"
            strSql += " ORDER BY METALNAME"
            Dim dt As New DataTable()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    selMetalId += "'" & dt.Rows(i).Item("METALID").ToString & "',"
                Next
                If selMetalId <> "" Then
                    selMetalId = Mid(selMetalId, 1, selMetalId.Length - 1)
                End If
            End If
        ElseIf cmbMetal.Text <> "" Then
            Dim sql As String = "SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(cmbMetal.Text) & ")"
            Dim dtMetal As New DataTable()
            da = New OleDbDataAdapter(sql, cn)
            da.Fill(dtMetal)
            If dtMetal.Rows.Count > 0 Then
                For j As Integer = 0 To dtMetal.Rows.Count - 1
                    selMetalId += "'"
                    selMetalId += dtMetal.Rows(j).Item("METALID").ToString + ""
                    selMetalId += "'"
                    selMetalId += ","
                Next
                If selMetalId <> "" Then
                    selMetalId = Mid(selMetalId, 1, selMetalId.Length - 1)
                End If
            End If
        End If
        If chkCounter.Checked = True Then
            Dim Selectmetals() As String = selMetalId.Split(",")
            If Not STKTRAN_MULTIMETAL And Selectmetals.Length > 1 Then MsgBox("Multi metal items transfer not possible", MsgBoxStyle.Information) : Exit Sub
        End If
        'Dim dtrow() As DataRow = dtGridView.Select("METALID <> " & selMetalId & "")

        'AddSelectAllCheckBox(gridView)

        If cmbCounter_OWN.Text = "ALL" Then
            strSql = " SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER"
            strSql += " ORDER BY ITEMCTRNAME"
            Dim dt1 As New DataTable()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                For k As Integer = 0 To dt1.Rows.Count - 1
                    selCounterId += "'" & dt1.Rows(k).Item("ITEMCTRID").ToString & "',"
                Next
                If selCounterId <> "" Then
                    selCounterId = Mid(selCounterId, 1, selCounterId.Length - 1)
                End If
            End If
        ElseIf cmbCounter_OWN.Text <> "" Then
            Dim sql As String = "SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & GetQryString(cmbCounter_OWN.Text) & ")"
            Dim dtCounter As New DataTable()
            da = New OleDbDataAdapter(sql, cn)
            da.Fill(dtCounter)
            If dtCounter.Rows.Count > 0 Then
                For m As Integer = 0 To dtCounter.Rows.Count - 1
                    selCounterId += "'" & dtCounter.Rows(m).Item("ITEMCTRID").ToString + "',"
                Next
                If selCounterId <> "" Then
                    selCounterId = Mid(selCounterId, 1, selCounterId.Length - 1)
                End If
            End If
        End If
        If txtTagNo.Text <> "" And gridView.Rows.Count > 0 Then
            For Each row As DataGridViewRow In gridView.Rows
                'For i As Integer = 0 To gridView.Rows.Count - 1

                'Next
                Dim TagNoCellText As String
                TagNoCellText = row.Cells("TAGNO").Value
                Dim firstcolumn As String = row.Cells(2).ToString() '
                If TagNoCellText <> "" And TagNoCellText <> Nothing Then
                    If txtTagNo.Text = TagNoCellText.ToString() Then
                        If chkCheckByScan.Checked And ChkTagTran Then
                            If row.Cells("checkBoxColumn").Value = False Then
                                row.Cells("checkBoxColumn").Value = True
                            Else
                                MsgBox("This Tag No already Marked...!", MsgBoxStyle.Information)
                            End If
                            txtTagNo.Text = ""
                            Exit Sub
                        Else
                            MsgBox("This Tag No already Exist...!", MsgBoxStyle.Information)
                            Exit Sub
                        End If
                        'row.Cells(0).ReadOnly = True
                        'MsgBox("This Tag No already Exist...!", MsgBoxStyle.Information)
                        'Exit Sub
                    End If
                End If
            Next
        End If


        Dim STONEDIAMETAL As String = "'D','T'"
        If chkCounter.Checked Then
            If gridView.Rows.Count > 1 Then
                strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPTAGTRANSFER1 "
                strSql += vbCrLf + " SELECT SNO,IT.ITEMNAME ITEM"
                If ReplSubItemid Then
                    strSql += vbCrLf + " ,'" & cmbSubitem.Text & "' SUBITEM"
                Else
                    strSql += vbCrLf + " ,SUB.SUBITEMNAME SUBITEM"
                End If
                strSql += vbCrLf + " ,T.TAGNO,T.PCS,T.GRSWT,T.LESSWT,T.NETWT"
                strSql += vbCrLf + " ,T.MAXMCGRM, T.MAXMC"
                strSql += vbCrLf + " ,T.MAXWASTPER,T.MAXWAST"
                strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNPCS"
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
                strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PSTNPCS"
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PSTNWT"
                strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
                strSql += vbCrLf + " ,SALVALUE,RATE"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(3),NULL)COLHEAD,T.PCTFILE"
                strSql += vbCrLf + " ,CONVERT(INT,1) AS RESULT,IT.ITEMNAME TITEM,CTR.ITEMCTRNAME TCOUNTER,CTR.ITEMCTRNAME COUNTER  "
                strSql += vbCrLf + " ,IT.METALID METALID,M.METALNAME  TMETAL,'T' STOCKMODE,SALEMODE"
                strSql += vbCrLf + " ,CASE WHEN STKTYPE='M' THEN 'MANUFACTURING' WHEN STKTYPE='E' THEN 'EXEMPTED' ELSE 'TRADING' END AS STKTYPE"
                strSql += vbCrLf + " ,(SELECT TOP 1 SEAL FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID=T.DESIGNERID)DESIGNERID "
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IT ON IT.ITEMID = T.ITEMID"
                If ReplSubItemid = False Then
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SUB ON SUB.SUBITEMID = T.SUBITEMID"
                End If
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER CTR ON CTR.ITEMCTRID= T.ITEMCTRID"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON M.METALID= IT.METALID "
                strSql += vbCrLf + " WHERE ISSDATE IS NULL"
                strSql += vbCrLf + " AND SNO NOT IN (SELECT SNO FROM TEMPTABLEDB..TEMPTAGTRANSFER1)"
                strSql += vbCrLf + " AND IT.METALID IN (" & selMetalId & ")"
                strSql += vbCrLf + " AND T.ITEMCTRID IN (" & selCounterId & ")"
                If chkWithTransferStk.Checked = False Then
                    strSql += vbCrLf + " AND ISNULL(T.TOFLAG,'')='' "
                    strSql += vbCrLf + " AND NOT EXISTS(SELECT 1 FROM " & cnAdminDb & "..CITEMTAG WHERE ITEMID =T.ITEMID AND TAGNO=T.TAGNO AND COMPANYID=T.COMPANYID AND ISNULL(TOFLAG,'')='TR') "
                End If
                If CmbDesigner.Text <> "ALL" And CmbDesigner.Text <> "" Then
                    strSql += vbCrLf + " AND T.DESIGNERID=(SELECT TOP 1 DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME='" & CmbDesigner.Text & "')"
                End If
                strSql += vbCrLf + " AND ISNULL(APPROVAL,'') = ''"
                If Val(txtLotNo_NUM.Text) <> 0 Then
                    strSql += vbCrLf + " AND T.LOTSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE LOTNO = " & Val(txtLotNo_NUM.Text) & ")"
                End If
                If chkCounter.Checked = False Then
                    If cmbCounter_OWN.Text <> "" And cmbCounter_OWN.Text <> "ALL" Then
                        strSql += vbCrLf + " AND ITEMCTRID = (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbCounter_OWN.Text & "')"
                    End If
                End If
                If dtpDate.Enabled Then strSql += vbCrLf + " AND RECDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "'"
                If txtTagNo.Text <> "" Then strSql += vbCrLf + " AND T.TAGNO = '" & txtTagNo.Text & "'"
                If Val(txtItemId.Text) > 0 Then strSql += vbCrLf + " AND t.ITEMID = " & Val(txtItemId.Text) & ""
                If Val(txtSubItemID.Text) > 0 Then strSql += vbCrLf + " AND t.SUBITEMID = " & Val(txtSubItemID.Text) & ""
                If Val(txtEstNo.Text) <> 0 Then strSql += vbCrLf + " AND CAST(T.ITEMID AS VARCHAR)+TAGNO IN (SELECT CAST(T.ITEMID AS VARCHAR)+TAGNO FROM " & cnStockDb & "..ESTISSUE WHERE TRANNO=" & Val(txtEstNo.Text) & ")"
                strSql += vbCrLf + " AND TCOSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbTagsCostName_MAN.Text & "')"
                If SPECIFICFORMAT.ToString <> "1" Then strSql += vbCrLf + " AND COSTID = '" & cnCostId & "'"
                strSql += vbCrLf + " AND IT.METALID NOT IN(" & STONEDIAMETAL & ")"
                If cmbSearchKey.Text.Trim <> "" And txtSearch.Text.Trim <> "" Then strSql += " AND T." & cmbSearchKey.Text & " ='" & txtSearch.Text & "'"
                If CmbStockType.Text <> "ALL" Then
                    If CmbStockType.Text = "MANUFACTURING" Then
                        strSql += vbCrLf + " AND T.STKTYPE ='M'"
                    ElseIf CmbStockType.Text = "EXEMPTED" Then
                        strSql += vbCrLf + " AND T.STKTYPE ='E'"
                    Else
                        strSql += vbCrLf + " AND ISNULL(T.STKTYPE,'') NOT IN('M','E') "
                    End If
                End If
                strSql += vbCrLf + " UNION ALL "
                strSql += vbCrLf + " SELECT SNO,IT.ITEMNAME ITEM"
                If ReplSubItemid Then
                    strSql += vbCrLf + " ,'" & cmbSubitem.Text & "' SUBITEM"
                Else
                    strSql += vbCrLf + " ,SUB.SUBITEMNAME SUBITEM"
                End If
                strSql += vbCrLf + " ,T.TAGNO,0 PCS,0 GRSWT,0 LESSWT,0 NETWT"
                strSql += vbCrLf + " ,0 MAXMCGRM, 0 MAXMC"
                strSql += vbCrLf + " ,0 MAXWASTPER,0 MAXWAST"
                strSql += vbCrLf + " ,CASE WHEN IT.DIASTONE = 'T' THEN T.PCS ELSE 0 END STNPCS,CASE WHEN IT.DIASTONE = 'T' THEN T.GRSWT ELSE 0 END STNWT"
                strSql += vbCrLf + " ,CASE WHEN IT.DIASTONE = 'P' THEN T.PCS ELSE 0 END PSTNPCS,CASE WHEN IT.DIASTONE = 'P' THEN T.GRSWT ELSE 0 END PSTNWT"
                strSql += vbCrLf + " ,CASE WHEN IT.DIASTONE = 'D' THEN T.PCS ELSE 0 END DIAPCS,CASE WHEN IT.DIASTONE = 'T' THEN T.GRSWT ELSE 0 END DIAWT"
                strSql += vbCrLf + " ,SALVALUE,RATE"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(3),NULL)COLHEAD,T.PCTFILE"
                strSql += vbCrLf + " ,CONVERT(INT,1) AS RESULT,IT.ITEMNAME TITEM,CTR.ITEMCTRNAME TCOUNTER,CTR.ITEMCTRNAME COUNTER  "
                strSql += vbCrLf + " ,IT.METALID METALID,M.METALNAME  TMETAL,'T' STOCKMODE,SALEMODE"
                strSql += vbCrLf + " ,CASE WHEN STKTYPE='M' THEN 'MANUFACTURING' WHEN STKTYPE='E' THEN 'EXEMPTED' ELSE 'TRADING' END AS STKTYPE"
                strSql += vbCrLf + " ,(SELECT TOP 1 SEAL FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID=T.DESIGNERID)DESIGNERID "
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IT ON IT.ITEMID = T.ITEMID"
                If ReplSubItemid = False Then
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SUB ON SUB.SUBITEMID = T.SUBITEMID"
                End If
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER CTR ON CTR.ITEMCTRID= T.ITEMCTRID"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON M.METALID= IT.METALID "
                strSql += vbCrLf + " WHERE ISSDATE IS NULL"
                strSql += vbCrLf + " AND SNO NOT IN (SELECT SNO FROM TEMPTABLEDB..TEMPTAGTRANSFER1)"
                strSql += vbCrLf + " AND IT.METALID IN (" & selMetalId & ")"
                strSql += vbCrLf + " AND T.ITEMCTRID IN (" & selCounterId & ")"
                If chkWithTransferStk.Checked = False Then
                    strSql += vbCrLf + " AND ISNULL(T.TOFLAG,'')='' "
                    strSql += vbCrLf + " AND NOT EXISTS(SELECT 1 FROM " & cnAdminDb & "..CITEMTAG WHERE ITEMID =T.ITEMID AND TAGNO=T.TAGNO AND COMPANYID=T.COMPANYID AND ISNULL(TOFLAG,'')='TR') "
                End If
                If CmbDesigner.Text <> "ALL" And CmbDesigner.Text <> "" Then
                    strSql += vbCrLf + " AND T.DESIGNERID=(SELECT TOP 1 DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME='" & CmbDesigner.Text & "')"
                End If
                strSql += vbCrLf + " AND ISNULL(APPROVAL,'') = ''"
                If Val(txtLotNo_NUM.Text) <> 0 Then
                    strSql += vbCrLf + " AND T.LOTSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE LOTNO = " & Val(txtLotNo_NUM.Text) & ")"
                End If
                If chkCounter.Checked = False Then
                    If cmbCounter_OWN.Text <> "" And cmbCounter_OWN.Text <> "ALL" Then
                        strSql += vbCrLf + " AND ITEMCTRID = (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbCounter_OWN.Text & "')"
                    End If
                End If

                If dtpDate.Enabled Then strSql += vbCrLf + " AND RECDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "'"
                If txtTagNo.Text <> "" Then strSql += vbCrLf + " AND T.TAGNO = '" & txtTagNo.Text & "'"
                If Val(txtItemId.Text) > 0 Then strSql += vbCrLf + " AND t.ITEMID = " & Val(txtItemId.Text) & ""
                If Val(txtSubItemID.Text) > 0 Then strSql += vbCrLf + " AND t.SUBITEMID = " & Val(txtSubItemID.Text) & ""
                If Val(txtEstNo.Text) <> 0 Then strSql += vbCrLf + " AND CAST(T.ITEMID AS VARCHAR)+TAGNO IN (SELECT CAST(T.ITEMID AS VARCHAR)+TAGNO FROM " & cnStockDb & "..ESTISSUE WHERE TRANNO=" & Val(txtEstNo.Text) & ")"
                strSql += vbCrLf + " AND TCOSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbTagsCostName_MAN.Text & "')"
                If SPECIFICFORMAT.ToString <> "1" Then strSql += vbCrLf + " AND COSTID = '" & cnCostId & "'"
                strSql += vbCrLf + " AND IT.METALID IN(" & STONEDIAMETAL & ")"
                If cmbSearchKey.Text.Trim <> "" And txtSearch.Text.Trim <> "" Then strSql += " AND T." & cmbSearchKey.Text & " ='" & txtSearch.Text & "'"
                If CmbStockType.Text <> "ALL" Then
                    If CmbStockType.Text = "MANUFACTURING" Then
                        strSql += vbCrLf + " AND T.STKTYPE ='M'"
                    ElseIf CmbStockType.Text = "EXEMPTED" Then
                        strSql += vbCrLf + " AND T.STKTYPE ='E'"
                    Else
                        strSql += vbCrLf + " AND ISNULL(T.STKTYPE,'') NOT IN('M','E') "
                    End If
                End If
                cmd = New OleDbCommand(strSql, cn)
                cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            Else
                strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPTAGTRANSFER1') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPTAGTRANSFER1 "
                strSql += vbCrLf + " SELECT SNO,IT.ITEMNAME ITEM"
                If ReplSubItemid Then
                    strSql += vbCrLf + " ,'" & cmbSubitem.Text & "' SUBITEM"
                Else
                    strSql += vbCrLf + " ,SUB.SUBITEMNAME SUBITEM"
                End If
                strSql += vbCrLf + " ,T.TAGNO,T.PCS,T.GRSWT,T.LESSWT,T.NETWT"
                strSql += vbCrLf + " ,T.MAXMCGRM, T.MAXMC"
                strSql += vbCrLf + " ,T.MAXWASTPER,T.MAXWAST"
                strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNPCS"
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
                strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PSTNPCS"
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PSTNWT"
                strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
                strSql += vbCrLf + " ,SALVALUE,RATE"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(3),NULL)COLHEAD,T.PCTFILE"
                strSql += vbCrLf + " ,CONVERT(INT,1) AS RESULT,IT.ITEMNAME TITEM,CTR.ITEMCTRNAME TCOUNTER,CTR.ITEMCTRNAME COUNTER  "
                strSql += vbCrLf + " ,IT.METALID METALID,M.METALNAME  TMETAL,'T' STOCKMODE,SALEMODE"
                strSql += vbCrLf + " ,CASE WHEN STKTYPE='M' THEN 'MANUFACTURING' WHEN STKTYPE='E' THEN 'EXEMPTED' ELSE 'TRADING' END AS STKTYPE"
                strSql += vbCrLf + " ,(SELECT TOP 1 SEAL FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID=T.DESIGNERID)DESIGNERID "
                strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPTAGTRANSFER1"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IT ON IT.ITEMID = T.ITEMID"
                If ReplSubItemid = False Then
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SUB ON SUB.SUBITEMID = T.SUBITEMID"
                End If
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER CTR ON CTR.ITEMCTRID= T.ITEMCTRID"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON M.METALID= IT.METALID "
                strSql += vbCrLf + " WHERE ISSDATE IS NULL"
                'strSql += vbCrLf + " AND (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID) IN (" & selMetalId & ")"
                strSql += vbCrLf + " AND IT.METALID IN (" & selMetalId & ")"
                'strSql += vbCrLf + " AND T.ITEMCTRID IN (" & selCounterId & ")"
                strSql += vbCrLf + " AND IT.METALID NOT IN(" & STONEDIAMETAL & ")"
                strSql += vbCrLf + " AND ISNULL(APPROVAL,'') = ''"
                If chkWithTransferStk.Checked = False Then
                    strSql += vbCrLf + " AND ISNULL(T.TOFLAG,'')='' "
                    strSql += vbCrLf + " AND NOT EXISTS(SELECT 1 FROM " & cnAdminDb & "..CITEMTAG WHERE ITEMID =T.ITEMID AND TAGNO=T.TAGNO AND COMPANYID=T.COMPANYID AND ISNULL(TOFLAG,'')='TR') "
                End If
                If Val(txtLotNo_NUM.Text) <> 0 Then
                    strSql += vbCrLf + " AND T.LOTSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE LOTNO = " & Val(txtLotNo_NUM.Text) & ")"
                End If
                If chkCounter.Checked = False Then
                    If cmbCounter_OWN.Text <> "" And cmbCounter_OWN.Text <> "ALL" Then strSql += vbCrLf & " AND T.ITEMCTRID IN (" & selCounterId & ")"
                End If
                If dtpDate.Enabled Then strSql += vbCrLf + " AND RECDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "'"
                If txtTagNo.Text <> "" Then strSql += vbCrLf + " AND T.TAGNO = '" & txtTagNo.Text & "'"
                If Val(txtItemId.Text) > 0 Then strSql += vbCrLf + " AND t.ITEMID = " & Val(txtItemId.Text) & ""
                If Val(txtSubItemID.Text) > 0 Then strSql += vbCrLf + " AND t.SUBITEMID = " & Val(txtSubItemID.Text) & ""
                If Val(txtEstNo.Text) <> 0 Then strSql += vbCrLf + " AND CAST(T.ITEMID AS VARCHAR)+TAGNO IN (SELECT CAST(ITEMID AS VARCHAR)+TAGNO FROM " & cnStockDb & "..ESTISSUE WHERE TRANNO=" & Val(txtEstNo.Text) & ")"

                strSql += vbCrLf + " AND TCOSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbTagsCostName_MAN.Text & "')"

                If SPECIFICFORMAT.ToString <> "1" Then strSql += vbCrLf + " AND COSTID = '" & cnCostId & "'"
                If cmbSearchKey.Text.Trim <> "" And txtSearch.Text.Trim <> "" Then strSql += " AND T." & cmbSearchKey.Text & " ='" & txtSearch.Text & "'"
                If CmbStockType.Text <> "ALL" Then
                    If CmbStockType.Text = "MANUFACTURING" Then
                        strSql += vbCrLf + " AND T.STKTYPE ='M'"
                    ElseIf CmbStockType.Text = "EXEMPTED" Then
                        strSql += vbCrLf + " AND T.STKTYPE ='E'"
                    Else
                        strSql += vbCrLf + " AND ISNULL(T.STKTYPE,'') NOT IN('M','E') "
                    End If
                End If
                cmd = New OleDbCommand(strSql, cn)
                cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If

            strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPTAGTRANSFER') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPTAGTRANSFER"
            strSql += vbCrLf + " SELECT * INTO TEMPTABLEDB..TEMPTAGTRANSFER FROM TEMPTABLEDB..TEMPTAGTRANSFER1 ORDER BY TCOUNTER,TMETAL,TITEM,RESULT,COLHEAD"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            'strSql = vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMPTAGTRANSFER1 ORDER BY TCOUNTER,TMETAL,TITEM,RESULT,COLHEAD"
            'Dim dtTempx As New DataTable
            'dtTemp.Clear()
            'gridView.DataSource = Nothing
            'da = New OleDbDataAdapter(strSql, cn)
            'da.Fill(dtTempx)
            'For Each ro As DataRow In dtGridView.Select("Sno<>''")
            '    dtTempx.ImportRow(ro)
            'Next

            strSql = vbCrLf + " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPTAGTRANSFER)>0"
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPTAGTRANSFER(TCOUNTER,TMETAL,ITEM,SUBITEM,TAGNO,RESULT,COLHEAD,SNO,STOCKMODE,LESSWT,STNPCS,STNWT,DIAPCS,DIAWT,SALVALUE,RATE,PCTFILE,SALEMODE,STKTYPE,MAXMCGRM,MAXMC,MAXWASTPER,MAXWAST)"
            strSql += vbCrLf + " SELECT DISTINCT TCOUNTER ,TMETAL,TCOUNTER,'' SUBITEM,'' TAGNO,0 RESULT,'T' COLHEAD,'' SNO,'' STOCKMODE,0 LESSWT,0 STNPCS,0 STNWT,0 DIAPCS,0 DIAWT,0 SALVALUE,0 RATE,'' PCTFILE,'' SALEMODE,'' STKTYPE,0 MAXMCGRM,0 MAXMC,0 MAXWASTPER,0 MAXWAST  FROM TEMPTABLEDB..TEMPTAGTRANSFER "

            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPTAGTRANSFER (TCOUNTER,TMETAL,ITEM,SUBITEM,TAGNO,RESULT,COLHEAD,SNO,STOCKMODE,LESSWT,STNPCS,STNWT,DIAPCS,DIAWT,SALVALUE,RATE,PCTFILE,SALEMODE,STKTYPE,MAXMCGRM,MAXMC,MAXWASTPER,MAXWAST)"
            strSql += vbCrLf + " SELECT DISTINCT TCOUNTER,TMETAL,TMETAL,'' SUBITEM,''TAGNO,6 RESULT,'N' COLHEAD,'' SNO ,'' STOCKMODE,0 LESSWT,0 STNPCS,0 STNWT,0 DIAPCS,0 DIAWT,0 SALVALUE,0 RATE,'' PCTFILE,'' SALEMODE,'' STKTYPE,0 MAXMCGRM,0 MAXMC,0 MAXWASTPER,0 MAXWAST  FROM TEMPTABLEDB..TEMPTAGTRANSFER"

            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPTAGTRANSFER (TCOUNTER,TMETAL,TITEM,ITEM,SUBITEM,TAGNO,RESULT,COLHEAD,PCS,GRSWT,LESSWT,NETWT,STNPCS,STNWT,DIAPCS,DIAWT,SALVALUE,RATE,SNO,STOCKMODE,PCTFILE,SALEMODE,STKTYPE,MAXMCGRM,MAXMC,MAXWASTPER,MAXWAST)"
            strSql += vbCrLf + " SELECT DISTINCT TCOUNTER,TMETAL,TITEM,'ITEM SUB TOTAL','' SUBITEM,''TAGNO,2 RESULT,'S' COLHEAD,"
            strSql += vbCrLf + " SUM(PCS),SUM(GRSWT),SUM(LESSWT),SUM(NETWT),SUM(STNPCS),SUM(STNWT),SUM(DIAPCS),SUM(DIAWT),SUM(SALVALUE),0 RATE,'' SNO,'' STOCKMODE,''PCTFILE,'' SALEMODE,'' STKTYPE,SUM(MAXMCGRM) MAXMCGRM,SUM(MAXMC) MAXMC,SUM(MAXWASTPER) MAXWASTPER,SUM(MAXWAST) MAXWAST"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPTAGTRANSFER GROUP BY TCOUNTER,TMETAL,TITEM,SALEMODE,STKTYPE"

            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPTAGTRANSFER(TCOUNTER,TMETAL,ITEM,SUBITEM,TAGNO,RESULT,COLHEAD,PCS,GRSWT,LESSWT,NETWT,STNPCS,STNWT,DIAPCS,DIAWT,SALVALUE,RATE,SNO,TITEM,STOCKMODE,PCTFILE,SALEMODE,STKTYPE,MAXMCGRM,MAXMC,MAXWASTPER,MAXWAST)  "
            strSql += vbCrLf + " SELECT DISTINCT 'ZZZZZZZ','ZZZZZZZZZZZZ','GRAND TOTAL','' SUBITEM,'' TAGNO,3 RESULT,'G' COLHEAD"
            strSql += vbCrLf + " ,SUM(PCS),SUM(GRSWT),SUM(LESSWT),SUM(NETWT),SUM(STNPCS),SUM(STNWT),SUM(DIAPCS),SUM(DIAWT),SUM(SALVALUE),0 RATE,'' SNO,'GRAND TOTAL','' STOCKMODE,'' PCTFILE,''SALEMODE,'' STKTYPE,SUM(MAXMCGRM) MAXMCGRM,SUM(MAXMC) MAXMC,SUM(MAXWASTPER) MAXWASTPER,SUM(MAXWAST) MAXWAST"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPTAGTRANSFER WHERE RESULT IN (1) "
            strSql += vbCrLf + " End"

            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " DELETE FROM TEMPTABLEDB..TEMPTAGTRANSFER WHERE TITEM IS NULL AND COLHEAD='S'"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMPTAGTRANSFER ORDER BY TCOUNTER,TMETAL,TITEM,RESULT,COLHEAD"

            If (cmbCounter_OWN.Text = "" Or cmbCounter_OWN.Text <> "ALL") And dtpDate.Enabled = False And Val(txtItemId.Text) = 0 And txtTagNo.Text = "" And Val(txtLotNo_NUM.Text) = 0 Then
                If MessageBox.Show("Do you want to load all tag items?", "Tag Transfer", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub
                End If
            End If

            Dim dtTemp As New DataTable
            'dtTemp.Columns.Add("CHECK", GetType(Boolean))
            'dtTemp.Clear()
            'gridView.DataSource = Nothing
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTemp)
            'dtTemp = dtTempx.Copy
            If Not dtTemp.Rows.Count > 0 Then
                MsgBox("Record not found", MsgBoxStyle.Information)
                txtTagNo.Focus()
                If chkCheckByScan.Checked Then
                    txtTagNo.Focus()
                End If
                Exit Sub
                'ElseIf dtTemp.Rows.Count > 0 Then
                '    If grpNonTag.Visible Then txtItemId.Focus() Else txtTagNo.Focus()
            Else
                If ChkTagTran Then
                    chkSelect.Visible = True
                Else
                    chkSelect.Visible = False
                End If
            End If

            Dim DTMETALDET As New DataTable
            If cmbMetal.Text = "ALL" Then
                strSql = " SELECT M.METALNAME  ITEM,'' as SUBITEM"
                strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(STNPCS)STNPCS,SUM(STNWT)STNWT,SUM(DIAPCS)DIAPCS,SUM(DIAWT)DIAWT,SUM(SALVALUE)SALVALUE,'M' COLHEAD,0 RESULT"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPTAGTRANSFER T"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON M.METALNAME = T.TMETAL WHERE TMETAL IN (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST)"
                strSql += vbCrLf + " AND COLHEAD='S'"
                strSql += vbCrLf + " GROUP BY M.METALNAME ,TMETAL"

                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(DTMETALDET)
                If DTMETALDET.Rows.Count > 0 Then
                    dtTemp.Merge(DTMETALDET)
                End If
            ElseIf cmbMetal.Text <> "" Then
                strSql = " SELECT M.METALNAME  ITEM,'' as SUBITEM"
                strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(STNPCS)STNPCS,SUM(STNWT)STNWT,SUM(DIAPCS)DIAPCS,SUM(DIAWT)DIAWT,SUM(SALVALUE)SALVALUE,'M' COLHEAD,0 RESULT"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPTAGTRANSFER T "
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON M.METALNAME = T.TMETAL WHERE TMETAL IN (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST)"
                strSql += vbCrLf + " AND COLHEAD='S'"
                strSql += vbCrLf + " GROUP BY TMETAL,m.METALNAME"

                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(DTMETALDET)
                If DTMETALDET.Rows.Count > 0 Then
                    dtTemp.Merge(DTMETALDET)
                End If
            End If

            'For Each ro As DataRow In dtTemp.Rows
            '    Dim RowCol() As DataRow = dtGridView.Select("SNO = '" & ro.Item("SNO").ToString & "'")
            '    If RowCol.Length = 0 Then
            '        dtGridView.ImportRow(ro)
            '    End If
            'Next

            dtGridView.AcceptChanges()
            'gridView.Rows.Clear()
            gridView.DataSource = dtTemp
            dtGridView = dtTemp.Copy
            With gridView
                For cnt As Integer = 0 To .ColumnCount - 1
                    .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
                Next
                .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("LESSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("SALVALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("STNPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("STNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("DIAPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("DIAWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("GRSWT").DefaultCellStyle.Format = "0.000"
                .Columns("LESSWT").DefaultCellStyle.Format = "0.000"
                .Columns("NETWT").DefaultCellStyle.Format = "0.000"
                .Columns("STNWT").DefaultCellStyle.Format = "0.000"
                .Columns("DIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
                .Columns("SALVALUE").DefaultCellStyle.Format = "0.00"
                .Columns("COLHEAD").Visible = False
                .Columns("RATE").Visible = False
                .Columns("SALVALUE").Visible = False
                .Columns("SNO").Visible = False
                .Columns("PCTFILE").Visible = False
                .Columns("RESULT").Visible = False
                .Columns("TITEM").Visible = False
                .Columns("TCOUNTER").Visible = False
                .Columns("TMETAL").Visible = False
                If .Columns.Contains("SGST") Then .Columns("SGST").Visible = False
                If .Columns.Contains("CGST") Then .Columns("CGST").Visible = False
                If .Columns.Contains("IGST") Then .Columns("IGST").Visible = False
                .Columns("ITEM").Width = 150
                .Columns("SUBITEM").Width = 150
                .Columns("STKTYPE").HeaderText = "STOCKTYPE"
                'gridView.Sort(gridView.Columns("COLHEAD"), System.ComponentModel.ListSortDirection.Ascending)
            End With
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            'AddGrandTotal()
            GridViewFormat()
            'grpNonTag.Visible = False
            cmbCostCentre_MAN.Enabled = False
            If chkCheckByScan.Checked Then
                txtTagNo.Clear()
                txtItemId.Select()
                If Val(txtEstNo.Text) <> 0 Then txtEstNo.Clear() : txtEstNo.Select()
            End If
            Prop_Sets()
            LoadTransactionType()
        ElseIf chkCounter.Checked = False Then
            strSql = " SELECT SNO,IT.ITEMNAME ITEM"
            If ReplSubItemid Then
                strSql += vbCrLf + " ,'" & cmbSubitem.Text & "' SUBITEM"
            Else
                strSql += vbCrLf + " ,SUB.SUBITEMNAME SUBITEM"
            End If
            strSql += " ,T.TAGNO,T.PCS,T.GRSWT,T.LESSWT,T.NETWT"
            strSql += vbCrLf + " ,T.MAXMCGRM, T.MAXMC"
            strSql += vbCrLf + " ,T.MAXWASTPER,T.MAXWAST "
            strSql += " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNPCS"
            strSql += " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
            strSql += " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PSTNPCS"
            strSql += " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PSTNWT"
            strSql += " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
            strSql += " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
            strSql += " ,SALVALUE,RATE,IT.METALID METALID "
            strSql += " ,CTR.ITEMCTRNAME AS COUNTER"
            'strSql += " ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID)AS COUNTER"
            strSql += " ,CONVERT(VARCHAR(3),NULL)COLHEAD,T.PCTFILE,'T' AS STOCKMODE,SALEMODE"
            strSql += " ,CASE WHEN STKTYPE='M' THEN 'MANUFACTURING' WHEN STKTYPE='E' THEN 'EXEMPTED' ELSE 'TRADING' END AS STKTYPE"
            strSql += vbCrLf + " ,(SELECT TOP 1 SEAL FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID=T.DESIGNERID)DESIGNERID "
            strSql += " FROM " & cnAdminDb & "..ITEMTAG T"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IT ON IT.ITEMID = T.ITEMID"
            If ReplSubItemid = False Then
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SUB ON SUB.SUBITEMID = T.SUBITEMID"
            End If
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER CTR ON CTR.ITEMCTRID= T.ITEMCTRID"
            strSql += " WHERE ISSDATE IS NULL"
            strSql += " AND ISNULL(APPROVAL,'') = ''"
            If chkWithTransferStk.Checked = False Then
                strSql += vbCrLf + " AND ISNULL(T.TOFLAG,'')='' "
                strSql += vbCrLf + " AND NOT EXISTS(SELECT 1 FROM " & cnAdminDb & "..CITEMTAG WHERE ITEMID =T.ITEMID AND TAGNO=T.TAGNO AND COMPANYID=T.COMPANYID AND ISNULL(TOFLAG,'')='TR') "
            End If
            If Val(txtLotNo_NUM.Text) <> 0 Then
                strSql += " AND T.LOTSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE LOTNO = " & Val(txtLotNo_NUM.Text) & ")"
            End If
            If cmbCounter_OWN.Text <> "" And cmbCounter_OWN.Text <> "ALL" Then
                strSql += " AND T.ITEMCTRID IN (" & selCounterId & ")"
                '(SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbCounter_OWN.Text & "')"
            End If
            If CmbDesigner.Text <> "ALL" And CmbDesigner.Text <> "" Then
                strSql += vbCrLf + " AND T.DESIGNERID=(SELECT TOP 1 DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME='" & CmbDesigner.Text & "')"
            End If
            If dtpDate.Enabled Then strSql += " AND RECDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "'"
            If txtTagNo.Text <> "" Then strSql += " AND TAGNO = '" & txtTagNo.Text & "'"

            If Val(txtItemId.Text) > 0 Then strSql += " AND t.ITEMID = " & Val(txtItemId.Text) & ""

            If Val(txtSubItemID.Text) > 0 Then strSql += " AND t.SUBITEMID = " & Val(txtSubItemID.Text) & ""

            If Val(txtEstNo.Text) <> 0 Then strSql += vbCrLf + " AND CAST(T.ITEMID AS VARCHAR)+TAGNO IN (SELECT CAST(ITEMID AS VARCHAR)+TAGNO FROM " & cnStockDb & "..ESTISSUE WHERE TRANNO=" & Val(txtEstNo.Text) & ")"

            strSql += " AND TCOSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbTagsCostName_MAN.Text & "')"
            If SPECIFICFORMAT.ToString <> "1" Then strSql += " AND COSTID = '" & cnCostId & "'"
            If SPECIFICFORMAT.ToString = "1" Then strSql += " AND T.COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " AND METALID NOT IN(" & STONEDIAMETAL & ")"
            'strSql += vbCrLf + " AND IT.STUDDED NOT IN('S')"
            If cmbSearchKey.Text.Trim <> "" And txtSearch.Text.Trim <> "" Then strSql += " AND T." & cmbSearchKey.Text & " ='" & txtSearch.Text & "'"
            If CmbStockType.Text <> "ALL" Then
                If CmbStockType.Text = "MANUFACTURING" Then
                    strSql += vbCrLf + " AND T.STKTYPE ='M'"
                ElseIf CmbStockType.Text = "EXEMPTED" Then
                    strSql += vbCrLf + " AND T.STKTYPE ='E'"
                Else
                    strSql += vbCrLf + " AND ISNULL(T.STKTYPE,'') NOT IN('M','E') "
                End If
            End If
            strSql += " UNION ALL"
            strSql += " SELECT SNO,IT.ITEMNAME ITEM"
            If ReplSubItemid Then
                strSql += vbCrLf + " ,'" & cmbSubitem.Text & "' SUBITEM"
            Else
                strSql += vbCrLf + " ,SUB.SUBITEMNAME SUBITEM"
            End If
            strSql += " ,T.TAGNO,CASE WHEN IT.STUDDED = 'S' THEN 0 ELSE T.PCS END PCS,CASE WHEN IT.STUDDED = 'S' THEN 0 ELSE T.GRSWT END GRSWT,CASE WHEN IT.STUDDED = 'S' THEN 0 ELSE T.LESSWT END LESSWT, CASE WHEN IT.STUDDED = 'S' THEN 0 ELSE T.NETWT END NETWT"
            strSql += vbCrLf + " ,0 MAXMCGRM, 0 MAXMC"
            strSql += vbCrLf + " ,0 MAXWASTPER,0 MAXWAST"
            strSql += vbCrLf + ",CASE WHEN IT.DIASTONE = 'S' AND IT.STUDDED = 'S' THEN T.PCS ELSE 0 END STNPCS,CASE WHEN IT.DIASTONE = 'S'  AND IT.STUDDED = 'S' THEN T.GRSWT ELSE 0 END STNWT"
            strSql += vbCrLf + ",CASE WHEN IT.DIASTONE = 'P'  AND IT.STUDDED = 'S' THEN T.PCS ELSE 0 END PSTNPCS,CASE WHEN IT.DIASTONE = 'P'  AND IT.STUDDED = 'S' THEN T.GRSWT ELSE 0 END PSTNWT"
            strSql += vbCrLf + ",CASE WHEN IT.DIASTONE = 'D'  AND IT.STUDDED = 'S' THEN T.PCS ELSE 0 END DIAPCS,CASE WHEN IT.DIASTONE = 'D'  AND IT.STUDDED = 'S' THEN T.GRSWT ELSE 0 END DIAWT"
            strSql += " ,SALVALUE,RATE,IT.METALID METALID "
            strSql += " ,CTR.ITEMCTRNAME AS COUNTER"
            'strSql += " ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID)AS COUNTER"
            strSql += " ,CONVERT(VARCHAR(3),NULL)COLHEAD,T.PCTFILE,'T' AS STOCKMODE,SALEMODE"
            strSql += " ,CASE WHEN STKTYPE='M' THEN 'MANUFACTURING' WHEN STKTYPE='E' THEN 'EXEMPTED' ELSE 'TRADING' END AS STKTYPE"
            strSql += vbCrLf + " ,(SELECT TOP 1 SEAL FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID=T.DESIGNERID)DESIGNERID "
            strSql += " FROM " & cnAdminDb & "..ITEMTAG T"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IT ON IT.ITEMID = T.ITEMID"
            If ReplSubItemid = False Then
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SUB ON SUB.SUBITEMID = T.SUBITEMID"
            End If
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER CTR ON CTR.ITEMCTRID= T.ITEMCTRID"
            strSql += " WHERE ISSDATE IS NULL"
            strSql += " AND ISNULL(APPROVAL,'') = ''"
            If chkWithTransferStk.Checked = False Then
                strSql += vbCrLf + " AND ISNULL(T.TOFLAG,'')='' "
                strSql += vbCrLf + " AND NOT EXISTS(SELECT 1 FROM " & cnAdminDb & "..CITEMTAG WHERE ITEMID =T.ITEMID AND TAGNO=T.TAGNO AND COMPANYID=T.COMPANYID AND ISNULL(TOFLAG,'')='TR') "
            End If
            If Val(txtLotNo_NUM.Text) <> 0 Then
                strSql += " AND T.LOTSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE LOTNO = " & Val(txtLotNo_NUM.Text) & ")"
            End If
            If cmbCounter_OWN.Text <> "" And cmbCounter_OWN.Text <> "ALL" Then strSql += " AND T.ITEMCTRID IN (" & selCounterId & ")"
            If CmbDesigner.Text <> "ALL" And CmbDesigner.Text <> "" Then
                strSql += vbCrLf + " AND T.DESIGNERID=(SELECT TOP 1 DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME='" & CmbDesigner.Text & "')"
            End If
            If dtpDate.Enabled Then strSql += " AND RECDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "'"
            If txtTagNo.Text <> "" Then strSql += " AND TAGNO = '" & txtTagNo.Text & "'"
            If Val(txtItemId.Text) > 0 Then strSql += " AND t.ITEMID = " & Val(txtItemId.Text) & ""
            If Val(txtSubItemID.Text) > 0 Then strSql += " AND t.SUBITEMID = " & Val(txtSubItemID.Text) & ""
            If Val(txtEstNo.Text) <> 0 Then strSql += vbCrLf + " AND CAST(T.ITEMID AS VARCHAR)+TAGNO IN (SELECT CAST(ITEMID AS VARCHAR)+TAGNO FROM " & cnStockDb & "..ESTISSUE WHERE TRANNO=" & Val(txtEstNo.Text) & ")"
            strSql += " AND TCOSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbTagsCostName_MAN.Text & "')"
            If SPECIFICFORMAT.ToString <> "1" Then strSql += " AND COSTID = '" & cnCostId & "'"
            If SPECIFICFORMAT.ToString = "1" Then strSql += " AND T.COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " AND METALID IN(" & STONEDIAMETAL & ")"
            'strSql += vbCrLf + " AND IT.STUDDED NOT IN('S')"
            If cmbSearchKey.Text.Trim <> "" And txtSearch.Text.Trim <> "" Then strSql += " AND T." & cmbSearchKey.Text & " ='" & txtSearch.Text & "'"
            If CmbStockType.Text <> "ALL" Then
                If CmbStockType.Text = "MANUFACTURING" Then
                    strSql += vbCrLf + " AND T.STKTYPE ='M'"
                ElseIf CmbStockType.Text = "EXEMPTED" Then
                    strSql += vbCrLf + " AND T.STKTYPE ='E'"
                Else
                    strSql += vbCrLf + " AND ISNULL(T.STKTYPE,'') NOT IN('M','E') "
                End If
            End If
            If (cmbCounter_OWN.Text = "" Or cmbCounter_OWN.Text <> "ALL") And dtpDate.Enabled = False And Val(txtItemId.Text) = 0 And txtTagNo.Text = "" And Val(txtLotNo_NUM.Text) = 0 Then
                If MessageBox.Show("Do you want to load all tag items?", "Tag Transfer", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub
                End If
            End If
            Dim dtTemp As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTemp)
            If Not dtTemp.Rows.Count > 0 Then
                MsgBox("Record not found", MsgBoxStyle.Information)
                'grpNonTag.Visible = False
                txtTagNo.Focus()
                If chkCheckByScan.Checked Then
                    txtTagNo.Focus()
                End If
                If Val(txtEstNo.Text) <> 0 Then txtEstNo.Select()
                Exit Sub
                'ElseIf dtTemp.Rows.Count > 0 Then
                '    If grpNonTag.Visible = True Then txtItemId.Focus() Else txtTagNo.Focus()
            Else
                If ChkTagTran Then
                    chkSelect.Visible = True
                Else
                    chkSelect.Visible = False
                End If
            End If
            Dim dMdt1 As New DataTable
            dMdt1 = dtTemp.DefaultView.ToTable(True, "METALID")
            If Not STKTRAN_MULTIMETAL And dMdt1.Rows.Count > 1 Then MsgBox("Multi metal items transfer not possible", MsgBoxStyle.Information) : Exit Sub
            Dim dMdt2 As New DataTable
            dMdt2 = dtGridView.DefaultView.ToTable(True, "METALID")
            Dim DmDt3 As New DataView
            DmDt3 = dMdt2.DefaultView
            DmDt3.RowFilter = "METALID<>''"
            dMdt2 = DmDt3.ToTable
            If Not STKTRAN_MULTIMETAL And dMdt2.Rows.Count > 1 Then MsgBox("Multi metal items transfer not possible", MsgBoxStyle.Information) : Exit Sub


            For Each ro As DataRow In dtTemp.Rows
                If ro.Item("STOCKMODE").ToString <> "N" Then
                    If Isgridexist(ro.Item("SNO").ToString, ro.Item("STOCKMODE").ToString) = False Then
                        dtGridView.ImportRow(ro)
                    End If
                Else
                    Dim RowCol() As DataRow = dtGridView.Select("SNO = '" & ro.Item("SNO").ToString & "' AND STOCKMODE = '" & ro.Item("STOCKMODE").ToString & "'")
                    If RowCol.Length = 0 Then
                        dtGridView.ImportRow(ro)
                    End If
                End If
            Next
            dtGridView.AcceptChanges()
            'If gridView.Rows.Count > 0 Then
            '    'gridView.Rows.Clear()
            '    gridView.DataSource = Nothing
            'End If
            'dtTemp.Columns.Add("CHECK", GetType(Boolean))
            gridView.DataSource = dtTemp
            With gridView
                If ChkTagTran Then
                    If gridView.Columns.Contains("checkBoxColumn") Then
                    Else
                        Dim checkBoxColumn As New DataGridViewCheckBoxColumn()
                        checkBoxColumn.HeaderText = ""
                        checkBoxColumn.Width = 30
                        checkBoxColumn.Name = "checkBoxColumn"
                        checkBoxColumn.TrueValue = 1
                        Dim iCount As Integer = gridView.Columns.Count
                        gridView.Columns.Insert(0, checkBoxColumn)
                    End If

                Else
                    'chkSelect.Visible = False
                End If



                For cnt As Integer = 0 To .ColumnCount - 1
                    .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
                    If cnt > 0 Then
                        .Columns(cnt).ReadOnly = True
                    Else
                        gridView.Columns(0).ReadOnly = False
                    End If
                Next
                .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("LESSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("MAXMCGRM").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("MAXMC").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("MAXWASTPER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("MAXWAST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("SALVALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("STNPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("STNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("PSTNPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("PSTNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("DIAPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("DIAWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("GRSWT").DefaultCellStyle.Format = "0.000"
                .Columns("LESSWT").DefaultCellStyle.Format = "0.000"
                .Columns("NETWT").DefaultCellStyle.Format = "0.000"
                .Columns("STNWT").DefaultCellStyle.Format = "0.000"

                .Columns("MAXMCGRM").DefaultCellStyle.Format = "0.000"
                .Columns("MAXMC").DefaultCellStyle.Format = "0.000"
                .Columns("MAXWASTPER").DefaultCellStyle.Format = "0.000"
                .Columns("MAXWAST").DefaultCellStyle.Format = "0.000"

                .Columns("DIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
                .Columns("SALVALUE").DefaultCellStyle.Format = "0.00"
                .Columns("COLHEAD").Visible = False
                If SPECIFICFORMAT.ToString = "1" Then
                    .Columns("RATE").Visible = True
                    .Columns("SALVALUE").Visible = True
                Else
                    .Columns("RATE").Visible = False
                    .Columns("SALVALUE").Visible = False
                End If
                .Columns("SNO").Visible = False
                .Columns("PCTFILE").Visible = False
                If .Columns.Contains("SGST") Then .Columns("SGST").Visible = False
                If .Columns.Contains("CGST") Then .Columns("CGST").Visible = False
                If .Columns.Contains("IGST") Then .Columns("IGST").Visible = False
                .Columns("STKTYPE").HeaderText = "STOCKTYPE"
            End With

            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            AddGrandTotal()
            gridView.Sort(gridView.Columns("COLHEAD"), System.ComponentModel.ListSortDirection.Ascending)
            cmbCostCentre_MAN.Enabled = False
            'grpNonTag.Visible = False
            If txtItemId.Text <> "" Then txtItemId.Clear() : txtItemId.Focus()
            If txtSubItemID.Text <> "" Then txtSubItemID.Clear() : txtSubItemID.Focus()
            If txtEstNo.Text <> "" Then txtEstNo.Clear() : txtEstNo.Focus()
            If txtTagNo.Text <> "" Then txtTagNo.Clear() : txtTagNo.Focus()

            If chkCheckByScan.Checked Then
                txtTagNo.Clear()
                txtItemId.Select()
                'txtSubItemID.Select()
                If Val(txtEstNo.Text) <> 0 Then txtEstNo.Select()
            End If
            Prop_Sets()
            'LoadTransactionType()
            If ChkTagTran Then
                Dim i As Integer = gridView.Rows.Count
                For Each row As DataGridViewRow In gridView.Rows
                    Dim firstCellText As String
                    firstCellText = row.Cells("ITEM").Value
                    Dim firstcolumn As String = row.Cells(1).ToString() '
                    'If row.Cells("ITEM").Value.ToString() = "" Then

                    'End If
                    If firstCellText = "GRAND TOTAL" Then
                        'If row.Cells.Contains("checkBoxColumn") Then
                        '    row.Cells("checkBoxColumn").ReadOnly = False
                        '    row.Cells("checkBoxColumn").Visible = False
                        'End If
                        row.Cells(0).ReadOnly = True

                        'row.DefaultCellStyle.ForeColor = row.DefaultCellStyle.BackColor

                        'row.Visible = False
                        'row.Cells(0).Visible = False
                        'gridView.Rows(i).Cells("checkBoxColumn").Visible = False
                        'row.Cells("checkBoxColumn").Visible = False

                    End If
                    If firstCellText = Nothing Then
                        'row.Cells("checkBoxColumn").ReadOnly = False
                        ''row.Cells("checkBoxColumn").Visible = False
                        row.Cells(0).ReadOnly = True
                        ''row.Cells(0).Visible = False
                        'row.Cells("checkBoxColumn").Visible = False
                    End If
                Next

            End If
        End If

    End Sub


    Private Sub AddSelectAllCheckBox(ByVal theDataGridView As DataGridView)
        Dim cbx As New CheckBox
        cbx.Name = "SelectAll"
        'The box size
        'cbx.Size = New Size(20, 20)
        cbx.Size = New Size(30, 30)

        Dim rect As Rectangle
        'rect = gridView.GetCellDisplayRectangle(0, 1, True)
        'rect = gridView.GetCellDisplayRectangle(-1, -1, True)
        rect = gridView.GetCellDisplayRectangle(-1, -1, True)


        'Put CheckBox in the middle-center of the column header.
        cbx.Location = New System.Drawing.Point(rect.Location.X + ((rect.Width - cbx.Width) / 3), rect.Location.Y + ((rect.Height - cbx.Height) / 3))
        cbx.BackColor = Color.Gray
        gridView.Controls.Add(cbx)

        'Handle header CheckBox check/uncheck function
        AddHandler cbx.Click, AddressOf HeaderCheckBox_Click
        'When any CheckBox value in the DataGridViewRows changed,
        'check/uncheck the header CheckBox accordingly.
        AddHandler gridView.CellValueChanged, AddressOf gridView_CellChecked
        'This event handler is necessary to commit new CheckBox cell value right after
        'user clicks the CheckBox.
        'Without it, CellValueChanged event occurs until the CheckBox cell lose focus
        'which means the header CheckBox won't display corresponding checked state instantly when user
        'clicks any one of the CheckBoxes.
        AddHandler gridView.CurrentCellDirtyStateChanged, AddressOf gridView_CurrentCellDirtyStateChanged
    End Sub
    Private Sub HeaderCheckBox_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me._IsSelectAllChecked = True
        Dim cbx As CheckBox
        cbx = DirectCast(sender, CheckBox)
        Dim gridView As DataGridView = cbx.Parent
        For Each row As DataGridViewRow In gridView.Rows
            'For Each row As DataGridViewRow In gridView.Rows
            '    Dim firstCellText As String
            '    firstCellText = row.Cells("ITEM").Value
            '    Dim firstcolumn As String = row.Cells(1).ToString() '
            '    'If row.Cells("ITEM").Value.ToString() = "" Then

            '    'End If
            '    If firstCellText = "GRAND TOTAL" Then
            '        'If row.Cells.Contains("checkBoxColumn") Then
            '        '    row.Cells("checkBoxColumn").ReadOnly = False
            '        '    row.Cells("checkBoxColumn").Visible = False
            '        'End If

            '        row.Cells(0).ReadOnly = True
            '        'row.Cells(0).Visible = False
            '    End If
            '    If firstCellText = Nothing Then
            '        row.Cells("checkBoxColumn").ReadOnly = False
            '        'row.Cells("checkBoxColumn").Visible = False
            '        'row.Cells(0).ReadOnly = True
            '        'row.Cells(0).Visible = False
            '    End If

            'Next
            Dim firstCellText As String
            firstCellText = row.Cells("ITEM").Value
            If firstCellText = "GRAND TOTAL" Then
                row.Cells(0).Value = cbx.Checked.FalseString
            ElseIf firstCellText = "GRAND TOTAL" Then
                row.Cells(0).Value = cbx.Checked.FalseString
                'If row.Cells.Contains("checkBoxColumn") Then

                'End If
            Else
                row.Cells(0).Value = cbx.Checked
            End If

        Next
        gridView.EndEdit()
        Me._IsSelectAllChecked = False
    End Sub
    Function OTP_Check() As Boolean
        If OTPOPTIONCHK("TAGTRANSFER") Then 'IS_USERLEVELPWD Then
            If MsgBox("Do You Have OTP to proceed?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                OTP_Password = usrpwdid("TAGTRANSFER", True)
                If OTP_Password = 0 Then MsgBox("OTP Not generated", MsgBoxStyle.OkOnly) : AddExemptionrow(6, "OTP", "Stock Transfer allowed") : Exit Function
                If OTP_Password <> 0 Then Return True : Exit Function ''''''''''''''2018-09-21
            Else
                Return False : Exit Function
            End If
        Else
            Return True : Exit Function
        End If
    End Function
    Private Function OTPOPTIONCHK(ByVal PWDCTRL As String) As Boolean
        Dim Sqlqry As String = "Select Optionid from " & cnAdminDb & "..PRJPWDOPTION where OPTIONNAME ='" & PWDCTRL & "' AND active = 'Y'"
        Dim Optionid As Integer = Val("" & objGPack.GetSqlValue(Sqlqry, , , tran))
        If Optionid = 0 Then Return False Else Return True
    End Function
    Private Function AddExemptionrow(ByVal mxOptionid As Integer, ByVal mxOpened As String, ByVal mxDesc As String)
        Dim Exrow As DataRow
        Exrow = dtExemption.NewRow
        Exrow!OPTIONID = mxOptionid
        Exrow!EXEMPUSER = userId
        Exrow!EXEMPOPEN = mxOpened
        Exrow!EXEMPDATE = "'" & Now.Date.ToString("dd-MMM-yyyy") & "'"
        Exrow!EXEMPTIME = "'" & Date.Now.ToLongTimeString & "'"
        Exrow!DESCRIPTION = mxDesc
        dtExemption.Rows.Add(Exrow)
    End Function
    Private Sub InsertExemptiondetails(ByVal Refno As String, ByVal tran As OleDbTransaction)
        For Each ro As DataRow In dtExemption.Rows
            strSql = " INSERT INTO " & cnAdminDb & "..EXEMPTION"
            strSql += " ("
            strSql += " Exempid,OPTIONID,COSTID,EXEMPDATE,EXEMPTIME,EXEMPUSER,EXEMPOPEN,DESCRIPTION"
            strSql += " )"
            strSql += " VALUES"
            strSql += " ("
            strSql += " " & Val(objGPack.GetMax("Exempid", "EXEMPTION", cnAdminDb, tran).ToString)
            strSql += " ," & ro.Item("OPTIONID") 'ISSSNO
            strSql += " ,'" & cnCostId & "'"
            strSql += " ," & ro.Item("EXEMPDATE")
            strSql += " ," & ro.Item("EXEMPTIME")
            strSql += " ," & ro.Item("EXEMPUSER")
            strSql += " ,'" & ro.Item("EXEMPOPEN") & "'"
            strSql += " ,'" & ro.Item("DESCRIPTION") & "-" & Refno & "'"
            strSql += " )"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()
        Next
    End Sub
    Private Sub gridView_CellChecked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
        If e.ColumnIndex = 0 Then
            RemoveHandler Me.chkSelect.CheckedChanged, AddressOf Me.chkSelect_CheckedChanged
            Dim dataGridView As DataGridView = DirectCast(sender, DataGridView)
            If Not Me._IsSelectAllChecked Then
                If dataGridView.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = False Then
                    'When any single CheckBox is unchecked, uncheck the header CheckBox.
                    'DirectCast(dataGridView.Controls.Item("SelectAll"), CheckBox).Checked = False
                    chkSelect.Checked = False
                Else
                    'When any single CheckBox is checked, loop through all CheckBoxes to determine
                    'if the header CheckBox needs to be unchecked.
                    Dim isAllChecked As Boolean = True
                    For Each row As DataGridViewRow In dataGridView.Rows
                        If row.Cells(4).Value = "GRAND TOTAL" Then Continue For
                        If row.Cells(0).Value = False Then
                            isAllChecked = False
                            Exit For
                        End If
                    Next
                    'DirectCast(dataGridView.Controls.Item("SelectAll"), CheckBox).Checked = isAllChecked
                    chkSelect.Checked = isAllChecked
                End If
            End If
            AddHandler Me.chkSelect.CheckedChanged, AddressOf Me.chkSelect_CheckedChanged
        End If
    End Sub

    'The CurrentCellDirtyStateChanged event happens after user change the cell value,
    'before the cell lose focus and CellValueChanged event.
    Private Sub gridView_CurrentCellDirtyStateChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim dataGridView As DataGridView = DirectCast(sender, DataGridView)
        If TypeOf dataGridView.CurrentCell Is DataGridViewCheckBoxCell Then
            'When the value changed cell is DataGridViewCheckBoxCell, commit the change
            'to invoke the CellValueChanged event.
            dataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit)
        End If
    End Sub



    'Private Sub loadNonTagDetails()
    '    Dim validateColName As String = ""
    '    Dim Sno As String = ""
    '    Dim selMetalId As String = ""
    '    Dim STONEDIAMETAL As String = "'D','T'"
    '    If cmbMetal.Text <> "ALL" And cmbMetal.Text <> "" And cmbMetal.Enabled = True Then
    '        Dim sql As String = "SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(cmbMetal.Text) & ")"
    '        Dim dtMetal As New DataTable()
    '        da = New OleDbDataAdapter(sql, cn)
    '        da.Fill(dtMetal)
    '        If dtMetal.Rows.Count > 0 Then
    '            For j As Integer = 0 To dtMetal.Rows.Count - 1
    '                selMetalId += "'"
    '                selMetalId += dtMetal.Rows(j).Item("METALID").ToString + ""
    '                selMetalId += "'"
    '                selMetalId += ","
    '            Next
    '            If selMetalId <> "" Then
    '                selMetalId = Mid(selMetalId, 1, selMetalId.Length - 1)
    '            End If
    '        End If
    '    End If
    '    'validateColName = "SNO"
    '    Dim mtocostid As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'")
    '    Dim mtagcostid As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbTagsCostName_MAN.Text & "'")
    '    strSql = " SELECT  T.SNO AS SNO,IM.ITEMNAME AS ITEM,ISNULL(SM.SUBITEMNAME,'') AS SUBITEM,PACKETNO PACKETNO,T.PCS,T.GRSWT,LESSWT,NETWT "
    '    strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='S')),0) STNPCS"
    '    strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='S')),0) STNWT"
    '    strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='P')),0) PSTNPCS"
    '    strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='P')),0) PSTNWT"
    '    strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D')),0) DIAPCS"
    '    strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D')),0) DIAWT"
    '    strSql += vbCrLf + " ,0 SALVALUE,IC.ITEMCTRNAME AS COUNTER,'N' STOCKMODE"
    '    strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAG T"
    '    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON T.ITEMID=IM.ITEMID"
    '    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SM ON T.ITEMID=SM.ITEMID AND T.SUBITEMID=SM.SUBITEMID"
    '    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER IC ON T.ITEMCTRID=IC.ITEMCTRID"
    '    strSql += vbCrLf + " WHERE RECISS ='R'"
    '    If chkRecDate.Checked = True Then strSql += " AND RECDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "'"
    '    If txtItemId.Text.Trim <> "" Then strSql += " AND T.ITEMID = " & Val(txtItemId.Text)
    '    If selMetalId <> "" Then strSql += " AND IM.METALID IN (" & selMetalId & ")"
    '    strSql += " AND ISNULL(POSTED,'') <> 'T' AND (ISNULL(REFNO,'') = '' OR (ISSTYPE ='TR' AND REFNO <> ''  AND ISNULL(T.COSTID,'') = '" & cnCostId & "'))"
    '    strSql += " AND T.TCOSTID ='" & mtagcostid & "'"
    '    strSql += " AND T.COMPANYID = '" & GetStockCompId() & "'"
    '    strSql += " AND ISNULL(CANCEL,'') <> 'Y'"
    '    strSql += vbCrLf + " AND METALID NOT IN(" & STONEDIAMETAL & ")"
    '    If txtLotNo_NUM.Text.Trim <> "" Then strSql += vbCrLf + " AND LOTNO='" & txtLotNo_NUM.Text.Trim & "'"
    '    strSql += " UNION ALL "
    '    strSql += " SELECT  T.SNO AS SNO,IM.ITEMNAME AS ITEM,ISNULL(SM.SUBITEMNAME,'') AS SUBITEM,PACKETNO PACKETNO,0 PCS,0 GRSWT,0 LESSWT,0 NETWT "
    '    strSql += vbCrLf + ",CASE WHEN IM.DIASTONE = 'S' THEN T.PCS ELSE 0 END STNPCS,CASE WHEN IM.DIASTONE = 'S' THEN T.GRSWT ELSE 0 END STNWT"
    '    strSql += vbCrLf + ",CASE WHEN IM.DIASTONE = 'P' THEN T.PCS ELSE 0 END PSTNPCS,CASE WHEN IM.DIASTONE = 'P' THEN T.GRSWT ELSE 0 END PSTNWT"
    '    strSql += vbCrLf + ",CASE WHEN IM.DIASTONE = 'D' THEN T.PCS ELSE 0 END DIAPCS,CASE WHEN IM.DIASTONE = 'D' THEN T.GRSWT ELSE 0 END DIAWT"
    '    strSql += vbCrLf + " ,0 SALVALUE,IC.ITEMCTRNAME AS COUNTER,'N' STOCKMODE"
    '    strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAG T"
    '    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON T.ITEMID=IM.ITEMID"
    '    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SM ON T.ITEMID=SM.ITEMID AND T.SUBITEMID=SM.SUBITEMID"
    '    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER IC ON T.ITEMCTRID=IC.ITEMCTRID"
    '    strSql += vbCrLf + " WHERE RECISS ='R'"
    '    If chkRecDate.Checked = True Then strSql += " AND RECDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "'"
    '    If txtItemId.Text.Trim <> "" Then strSql += " AND T.ITEMID = " & Val(txtItemId.Text)
    '    If selMetalId <> "" Then strSql += " AND IM.METALID IN (" & selMetalId & ")"
    '    strSql += " AND ISNULL(POSTED,'') <> 'T' AND (ISNULL(REFNO,'') = '' OR (ISSTYPE ='TR' AND REFNO <> ''  AND ISNULL(T.COSTID,'') = '" & cnCostId & "'))"
    '    strSql += " AND T.TCOSTID ='" & mtagcostid & "'"
    '    strSql += " AND T.COMPANYID = '" & GetStockCompId() & "'"
    '    strSql += " AND ISNULL(CANCEL,'') <> 'Y'"
    '    strSql += vbCrLf + " AND METALID IN(" & STONEDIAMETAL & ")"
    '    If txtLotNo_NUM.Text.Trim <> "" Then strSql += vbCrLf + " AND LOTNO='" & txtLotNo_NUM.Text.Trim & "'"


    '    Dim dtGrid As New DataTable
    '    dtGrid.Columns.Add("CHECK", GetType(Boolean))
    '    da = New OleDbDataAdapter(strSql, cn)
    '    da.Fill(dtGrid)
    '    If Not dtGrid.Rows.Count > 0 Then
    '        MsgBox("Record not found", MsgBoxStyle.Information)
    '        Exit Sub
    '    End If
    '    dtGrid.Columns("SNO").Caption = "VISIBLE_FALSE"
    '    Dim editcollist As String = "PCS,GRSWT,NETWT"
    '    objMultiSelect = New MultiSelectRowDia(dtGrid, validateColName, editcollist)
    '    objMultiSelect.txtSearchTagNo.Visible = False

    '    objMultiSelect.chkAppSales.Visible = False
    '    objMultiSelect.DateCheck = False
    '    If dtGrid.Rows.Count > 0 Then
    '        If objMultiSelect.ShowDialog = Windows.Forms.DialogResult.OK Then
    '            Me.Refresh()
    '            Sno = ""
    '            Dim TotPcs As Integer = 0
    '            Dim TotGrwt As Double = 0
    '            Dim TotNetwt As Double = 0

    '            For Each Ro As DataRow In objMultiSelect.RowSelected
    '                Sno += "'" & Ro.Item("SNO").ToString & "',"
    '                TotPcs += Val(Ro.Item("PCS").ToString)
    '                TotGrwt += Val(Ro.Item("Grswt").ToString)
    '                TotNetwt += Val(Ro.Item("Netwt").ToString)
    '            Next

    '            If Sno <> "" Then Sno = Mid(Sno, 1, Sno.Length - 1)
    '            strSql = " SELECT  T.SNO AS SNO,METALID,IM.ITEMNAME AS ITEM,ISNULL(SM.SUBITEMNAME,'') AS SUBITEM,PACKETNO TAGNO," & TotPcs.ToString & "PCS," & TotGrwt.ToString & " GRSWT,LESSWT," & TotNetwt.ToString & " NETWT "
    '            strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='S')),0) STNPCS"
    '            strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='S')),0) STNWT"
    '            strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='P')),0) PSTNPCS"
    '            strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='P')),0) PSTNWT"
    '            strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D')),0) DIAPCS"
    '            strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D')),0) DIAWT"
    '            strSql += vbCrLf + " ,0 SALVALUE"
    '            strSql += vbCrLf + " ,IC.ITEMCTRNAME AS COUNTER"
    '            strSql += vbCrLf + " ,CONVERT(VARCHAR(3),NULL)COLHEAD,'' PCTFILE,'N' AS STOCKMODE"
    '            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAG T"
    '            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON T.ITEMID=IM.ITEMID"
    '            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SM ON T.ITEMID=SM.ITEMID AND T.SUBITEMID=SM.SUBITEMID"
    '            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER IC ON T.ITEMCTRID=IC.ITEMCTRID"
    '            strSql += vbCrLf + " WHERE RECISS ='R' "
    '            If chkRecDate.Checked = True Then strSql += " AND RECDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "'"
    '            If txtItemId.Text.Trim <> "" Then strSql += " AND T.ITEMID = " & Val(txtItemId.Text)
    '            strSql += " AND ISNULL(T.COSTID,'') = '" & cnCostId & "'"
    '            strSql += " AND ISNULL(T.TCOSTID,'') = '" & mtagcostid & "'"
    '            strSql += vbCrLf + " AND METALID NOT IN(" & STONEDIAMETAL & ")"
    '            'strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbTagsCostName_MAN.Text & "'),'')"
    '            strSql += " AND T.COMPANYID = '" & GetStockCompId() & "'"
    '            strSql += " AND ISNULL(CANCEL,'') <> 'Y'"
    '            strSql += vbCrLf + " AND SNO IN(" & Sno & ")"
    '            strSql += " UNION ALL "
    '            strSql += " SELECT  T.SNO AS SNO,METALID,IM.ITEMNAME AS ITEM,ISNULL(SM.SUBITEMNAME,'') AS SUBITEM,PACKETNO TAGNO,0 PCS,0 GRSWT,0 LESSWT,0 NETWT "
    '            strSql += vbCrLf + ",CASE WHEN IM.DIASTONE = 'S' THEN T.PCS ELSE 0 END STNPCS,CASE WHEN IM.DIASTONE = 'S' THEN T.GRSWT ELSE 0 END STNWT"
    '            strSql += vbCrLf + ",CASE WHEN IM.DIASTONE = 'P' THEN T.PCS ELSE 0 END PSTNPCS,CASE WHEN IM.DIASTONE = 'P' THEN T.GRSWT ELSE 0 END PSTNWT"
    '            strSql += vbCrLf + ",CASE WHEN IM.DIASTONE = 'D' THEN T.PCS ELSE 0 END DIAPCS,CASE WHEN IM.DIASTONE = 'D' THEN T.GRSWT ELSE 0 END DIAWT"
    '            strSql += vbCrLf + " ,0 SALVALUE"
    '            strSql += vbCrLf + " ,IC.ITEMCTRNAME AS COUNTER"
    '            strSql += vbCrLf + " ,CONVERT(VARCHAR(3),NULL)COLHEAD,'' PCTFILE,'N' AS STOCKMODE"
    '            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAG T"
    '            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON T.ITEMID=IM.ITEMID"
    '            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SM ON T.ITEMID=SM.ITEMID AND T.SUBITEMID=SM.SUBITEMID"
    '            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER IC ON T.ITEMCTRID=IC.ITEMCTRID"
    '            strSql += vbCrLf + " WHERE RECISS ='R' "
    '            If chkRecDate.Checked = True Then strSql += " AND RECDATE = '" & dtpDate.Value.ToString("yyyy-MM-dd") & "'"
    '            If txtItemId.Text.Trim <> "" Then strSql += " AND T.ITEMID = " & Val(txtItemId.Text)
    '            strSql += " AND ISNULL(T.COSTID,'') = '" & cnCostId & "'"
    '            strSql += " AND ISNULL(T.TCOSTID,'') = '" & mtagcostid & "'"
    '            strSql += vbCrLf + " AND METALID IN(" & STONEDIAMETAL & ")"
    '            'strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbTagsCostName_MAN.Text & "'),'')"
    '            strSql += " AND T.COMPANYID = '" & GetStockCompId() & "'"
    '            strSql += " AND ISNULL(CANCEL,'') <> 'Y'"
    '            strSql += vbCrLf + " AND SNO IN(" & Sno & ")"

    '            If (cmbCounter_OWN.Text = "" Or cmbCounter_OWN.Text <> "ALL") And dtpDate.Enabled = False And Val(txtItemId.Text) = 0 And txtTagNo.Text = "" And Val(txtLotNo_NUM.Text) = 0 Then
    '                If MessageBox.Show("Do you want to load all tag items?", "Tag Transfer", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
    '                    Exit Sub
    '                End If
    '            End If
    '            Dim dtTemp As New DataTable
    '            ' dtGridView.Clear()
    '            ' gridView.DataSource = Nothing
    '            da = New OleDbDataAdapter(strSql, cn)
    '            da.Fill(dtTemp)
    '            If Not dtTemp.Rows.Count > 0 Then
    '                MsgBox("Record not found", MsgBoxStyle.Information)
    '                txtTagNo.Focus()
    '                If chkCheckByScan.Checked Then
    '                    txtTagNo.Focus()
    '                End If
    '                If Val(txtEstNo.Text) <> 0 Then txtEstNo.Select()
    '                Exit Sub
    '            ElseIf dtTemp.Rows.Count > 0 Then
    '                ChkNontag.Focus()
    '            End If
    '            For Each ro As DataRow In dtTemp.Rows
    '                If ro.Item("STOCKMODE").ToString <> "T" Then
    '                    If Isgridexist(ro.Item("sno").ToString) = False Then
    '                        dtGridView.ImportRow(ro)
    '                    End If
    '                Else
    '                    Dim RowCol() As DataRow = dtGridView.Select("SNO = '" & ro.Item("SNO").ToString & "'")
    '                    If RowCol.Length = 0 Then
    '                        dtGridView.ImportRow(ro)
    '                    End If

    '                End If

    '            Next
    '            dtGridView.AcceptChanges()
    '            gridView.DataSource = dtTemp
    '            With gridView
    '                For cnt As Integer = 0 To .ColumnCount - 1
    '                    .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
    '                Next
    '                .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
    '                .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
    '                .Columns("LESSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
    '                .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
    '                .Columns("SALVALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
    '                .Columns("STNPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
    '                .Columns("STNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
    '                .Columns("PSTNPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
    '                .Columns("PSTNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
    '                .Columns("DIAPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
    '                .Columns("DIAWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
    '                .Columns("GRSWT").DefaultCellStyle.Format = "0.000"
    '                .Columns("LESSWT").DefaultCellStyle.Format = "0.000"
    '                .Columns("NETWT").DefaultCellStyle.Format = "0.000"
    '                .Columns("STNWT").DefaultCellStyle.Format = "0.000"
    '                .Columns("PSTNWT").DefaultCellStyle.Format = "0.000"
    '                .Columns("DIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
    '                .Columns("SALVALUE").DefaultCellStyle.Format = "0.00"
    '                .Columns("COLHEAD").Visible = False
    '                .Columns("SALVALUE").Visible = False
    '                .Columns("SNO").Visible = False
    '                .Columns("PCTFILE").Visible = False
    '            End With

    '            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
    '            AddGrandTotal()
    '            gridView.Sort(gridView.Columns("COLHEAD"), System.ComponentModel.ListSortDirection.Ascending)
    '            cmbCostCentre_MAN.Enabled = False
    '            If chkCheckByScan.Checked Then
    '                txtTagNo.Clear()
    '                txtItemId.Select()
    '                If Val(txtEstNo.Text) <> 0 Then txtEstNo.Select()
    '            End If
    '        End If
    '    Else

    '    End If

    'End Sub



    Private Function Isgridexist(ByVal sno As String, Optional ByVal stockmode As String = "") As Boolean
        'For ii As Integer = 0 To gridView.Rows.Count - 1
        '    If gridView.Rows(ii).Cells("SNO").Value.ToString <> "" Then
        '        If gridView.Rows(ii).Cells("SNO").Value.ToString = sno And gridView.Rows(ii).Cells("STOCKMODE").Value.ToString = stockmode Then
        '            Return True
        '            Exit Function
        '        End If
        '    End If
        'Next
        'Return False
        For ii As Integer = 0 To gridView.Rows.Count - 1
            If Not gridView.Columns.Contains("SNO") Then Exit For
            If gridView.Rows(ii).Cells("SNO").Value.ToString <> "" Then
                If gridView.Rows(ii).Cells("SNO").Value.ToString = sno And gridView.Rows(ii).Cells("STOCKMODE").Value.ToString = stockmode Then
                    Return True
                    Exit Function
                End If
            End If
        Next
        Return False
    End Function

    Private Sub cmbCostCentre_MAN_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCostCentre_MAN.LostFocus
        If SPECIFICFORMAT.ToString = "1" Then
            Dim MainCostId As String = GetAdmindbSoftValue("SYNC-TO", "")
            strSql = "SELECT MAIN FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE COSTID = '" & cnCostId & "'"
            Dim ThisCO As Boolean = IIf(objGPack.GetSqlValue(strSql).ToString = "Y", True, False)
            If ThisCO Then
                'strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID <> '" & cnCostId & "' "
                strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE 1=1 "
            Else
                strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE 1=1 "
            End If
            If TagTranCorpOnly = True And ThisCO = False Then
                If strBCostid Is Nothing Then strSql += " AND COSTID IN(SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE ISNULL(ACTIVE,'')<>'N')"
            Else
                If strBCostid Is Nothing Then strSql += " AND COSTID IN(SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE ISNULL(ACTIVE,'')<>'N')"
            End If
            strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COMPANYID LIKE '%" & strCompanyId & "%')"
            If INTRBRANCHTRF_DISABLE Then
                If Not ThisCO And MainCostId <> "" Then strSql += " AND COSTID ='" & MainCostId & "'"
            End If
            strSql += " ORDER BY COSTNAME"
            cmbTagsCostName_MAN.Items.Clear()
            objGPack.FillCombo(strSql, cmbTagsCostName_MAN)
        Else
            cmbTagsCostName_MAN.Items.Clear()
            cmbTagsCostName_MAN.Items.Add(cmbCostCentre_MAN.Text)
            cmbTagsCostName_MAN.Items.Add(cnCostName)
            cmbTagsCostName_MAN.Text = cmbCostCentre_MAN.Text
        End If
    End Sub
    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "G"
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                    Case "S"
                        .DefaultCellStyle.BackColor = Color.MistyRose
                        .DefaultCellStyle.Font = reportSubTotalStyle.Font
                    Case "T"
                        .DefaultCellStyle.BackColor = reportHeadStyle.BackColor
                        .DefaultCellStyle.Font = reportHeadStyle.Font
                    Case "N"
                        .DefaultCellStyle.Font = reportHeadStyle.Font
                    Case "M"
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                End Select
            End With
        Next
    End Function
    Private Sub Prop_Sets()
        Dim obj As New frmTagTransfer_Properties
        obj.p_cmbMetal = cmbMetal.Text
        obj.p_cmbCounter = cmbCounter_OWN.Text
        SetSettingsObj(obj, Me.Name, GetType(frmTagTransfer_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmTagTransfer_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmTagTransfer_Properties))
        cmbMetal.Text = obj.p_cmbMetal
        cmbCounter_OWN.Text = obj.p_cmbCounter
    End Sub

    Private Sub chkCounter_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCounter.CheckedChanged
        If chkCounter.Checked Then
            cmbMetal.Enabled = True
        Else
            cmbMetal.Enabled = False
        End If
    End Sub

    Private Sub txtEstNo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtEstNo.TextChanged

    End Sub

    Private Sub ChkwithImage_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub txtItemId_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtItemId.TextChanged

    End Sub
    Private Sub txtNetWt_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = Chr(Keys.Enter) Then btnSearch.Focus() : Exit Sub

    End Sub
    Private Sub ChkNontag_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkNontag.CheckStateChanged
        If ChkNontag.Checked = True Then
            'loadNonTagDetails()
            If NonTag_trf_Lotno Then
                If Val(txtLotNo_NUM.Text) = 0 Then MsgBox("Lot No. must be given", MsgBoxStyle.Critical) : ChkNontag.Checked = False : Exit Sub
            End If
            If NonTag_trf_Sno Then
                funcloadNonTagDetailsSnoWise()
            Else
                loadNonTagDetailsNew()
            End If
            ChkNontag.Checked = False
        End If
    End Sub



    Private Sub txtRemark1_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRemark1.Leave
        If txtRemark1.Text.Trim = "" And IsRemarkRestrict Then
            MsgBox("Remark1 should not empty", MsgBoxStyle.Information)
            txtRemark1.Focus()
            Exit Sub
        End If
    End Sub

    Private Sub txtRemark2_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRemark2.Leave
        If txtRemark2.Text = "" And IsRemarkRestrict Then
            MsgBox("Remark2 should not empty", MsgBoxStyle.Information)
            txtRemark2.Focus()
            Exit Sub
        End If
    End Sub

    Private Sub txtTagNo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTagNo.TextChanged

    End Sub

    Private Sub cmbCostCentre_MAN_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCostCentre_MAN.SelectedIndexChanged
        If cmbCostCentre_MAN.Text <> "" Then
            CmbAcname.Items.Clear()
            Dim acname, Aacname As String
            acname = objGPack.GetSqlValue("SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE =(SELECT ISNULL(ACCODE,'') FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre_MAN.Text & "')")
            If acname <> "" Then CmbAcname.Items.Add(acname)
            Aacname = objGPack.GetSqlValue("SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE =(SELECT ISNULL(AACCODE,'') FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre_MAN.Text & "')")
            If Aacname <> "" Then CmbAcname.Items.Add(Aacname) : CmbAcname.Enabled = True Else CmbAcname.Enabled = False
            If CmbAcname.Items.Count > 0 Then CmbAcname.SelectedIndex = 0
            StateId = Val(objGPack.GetSqlValue("SELECT STATEID FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & CmbAcname.Text & "'").ToString)
            If StateId = 0 Then
                MsgBox("Please Update State for the Account [" & CmbAcname.Text & "]", MsgBoxStyle.Information)
                Exit Sub
            End If
        End If
    End Sub

    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel1.Paint

    End Sub



    Private Sub gridView_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellContentClick

    End Sub

    Private Sub chkSelect_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'If chkSelect.Checked = True Then
        '    Dim iCheck As Integer = gridView.Rows.Count - 1
        '    For i As Integer = 0 To gridView.Rows.Count - 1
        '        'For i As Integer = 0 To iCheck
        '        gridView.Rows(i).Cells("checkBoxColumn").Value = CheckState.Checked
        '        'If gridView.Rows(i) <= iCheck Then
        '        'End If

        '        If gridView.Rows(i).Cells("ITEM").Value.ToString() = "" Then

        '        Else
        '            Dim firstCellText As String
        '            firstCellText = gridView.Rows(i).Cells("ITEM").Value.ToString()
        '            'Dim firstcolumn As String = row.Cells(1).ToString()
        '            If firstCellText = "GRAND TOTAL" Then
        '                gridView.Rows(i).Cells("checkBoxColumn").Value = CheckState.Unchecked
        '            End If
        '        End If
        '    Next
        'Else
        '    For i As Integer = 0 To gridView.Rows.Count - 1
        '        gridView.Rows(i).Cells("checkBoxColumn").Value = CheckState.Unchecked
        '    Next
        'End If
        RemoveHandler gridView.CellValueChanged, AddressOf gridView_CellChecked
        If chkSelect.Checked = True Then
            For i As Integer = 0 To gridView.Rows.Count - 1
                gridView.Rows(i).Cells("checkBoxColumn").Value = CheckState.Checked

                For Each row As DataGridViewRow In gridView.Rows
                    Dim firstCellText As String
                    firstCellText = row.Cells("ITEM").Value
                    Dim firstcolumn As String = row.Cells(1).ToString() '                    
                    If firstCellText = "GRAND TOTAL" Then
                        row.Cells("checkBoxColumn").Value = CheckState.Unchecked
                    End If
                    If firstCellText = Nothing Then
                        row.Cells("checkBoxColumn").Value = CheckState.Unchecked
                    End If
                Next
            Next

        Else
            For i As Integer = 0 To gridView.Rows.Count - 1
                gridView.Rows(i).Cells("checkBoxColumn").Value = CheckState.Unchecked
            Next
        End If
        AddHandler gridView.CellValueChanged, AddressOf gridView_CellChecked
    End Sub

    Private Sub gridView_CellPainting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles gridView.CellPainting

        'If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
        '    If gridView.Columns(e.ColumnIndex).Name = "checkBoxColumn" Is Not Nothing Then
        '        Dim UseValue As Boolean = False
        '        e.PaintBackground(e.ClipBounds, True) '
        '        e.Handled = True
        '    End If
        'End If

        'If Me.gridView.Rows(e.RowIndex).Cells("checkBoxColumn").Value <> Me.gridView.SelectedValue And _
        '    e.ColumnIndex = Me.gridView.Columns("Check").Index Then
        '    ' cancel paint of this cell so the checkbox does not show
        'End If

        'Dim num As Integer
        'If e.RowIndex > -1 And e.ColumnIndex = 1 Then
        '    'If Integer.TryParse(Me.checkb.Text, num) = True Then
        '    'If num <> 0 Then
        '    If num = Me.gridView.Item(0, e.RowIndex).Value Then
        '        e.PaintBackground(e.ClipBounds, True)
        '        e.Handled = True
        '    End If
        '    'End If
        '    'End If
        'End If

        'If gridView.Rows(e.RowIndex).Cells("Grand Total").Value.ToString = "checkBoxColumn" Then
        '    'gridView.Rows(e.RowIndex).Cells(0).Visible = False
        '    gridView.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.LawnGreen

        'End If

    End Sub

    Private Sub txtSubItemID_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSubItemID.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadSalesSubItemName()
        End If
    End Sub

    Private Sub txtSubItemID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSubItemID.KeyPress
        '        If e.KeyChar = Chr(Keys.Enter) Then
        '            Dim dritem As DataRow
        '            Dim barcode2d() As String = txtItemId.Text.Split(BARCODE2DSEP)
        '            If Not chkCheckByScan.Checked Then
        '                If barcode2d.Length > 2 Then
        '                    If Not Barcode2dfound(txtItemId.Text) Then MsgBox("Stock Not Available" & vbCrLf & "Please check data", MsgBoxStyle.Critical) : Exit Sub
        '                End If
        '                If Val(txtItemId.Text) <> 0 Then
        '                    dritem = GetSqlRow("SELECT STOCKTYPE,SUBITEM FROM " & cnAdminDb & "..ITEMMAST WHERE CONVERT(VARCHAR,ITEMID) = '" & Trim(txtItemId.Text) & "'", cn)
        '                    If dritem Is Nothing Then Exit Sub
        '                    If dritem(0).ToString = "N" Or dritem(0).ToString = "P" Then
        '                        ChkNontag.Checked = True
        '                    Else
        '                        ChkNontag.Checked = False
        '                    End If
        '                    If dritem(1).ToString = "Y" And ReplSubItemid Then
        '                        strSql = vbCrLf + " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = " & Val(txtItemId.Text.ToString)
        '                        strSql += vbCrLf + " ORDER BY SUBITEMNAME"
        '                        objGPack.FillCombo(strSql, cmbSubitem, True, False)
        '                    End If
        '                End If
        '                SendKeys.Send("{TAB}")
        '                Exit Sub
        '            Else
        '                If barcode2d.Length > 2 Then
        '                    If Not Barcode2dfound(txtItemId.Text) Then MsgBox("Stock Not Available" & vbCrLf & "Please check data", MsgBoxStyle.Critical) : Exit Sub
        '                End If
        '            End If


        '            Dim sp() As String = txtItemId.Text.Split(PRODTAGSEP)
        '            If PRODTAGSEP <> "" And txtItemId.Text <> "" Then
        '                sp = txtItemId.Text.Split(PRODTAGSEP)
        '                txtItemId.Text = Trim(sp(0))
        '            End If

        '            dritem = GetSqlRow("SELECT STOCKTYPE,SUBITEM FROM " & cnAdminDb & "..ITEMMAST WHERE CONVERT(VARCHAR,ITEMID) = '" & Trim(txtItemId.Text) & "'", cn)
        '            If Not dritem Is Nothing Then
        '                If dritem(0).ToString = "N" Or dritem(0).ToString = "P" Then
        '                    ChkNontag.Checked = True
        '                    Exit Sub
        '                End If
        '            End If

        '            If txtItemId.Text.StartsWith("#") Then txtItemId.Text = txtItemId.Text.Remove(0, 1)
        'CheckItem:
        '            If txtItemId.Text = "" Then
        '                LoadSalesItemName()
        '            ElseIf txtItemId.Text <> "" And objGPack.DupCheck("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE CONVERT(VARCHAR,ITEMID) = '" & Trim(txtItemId.Text) & "'") = False Then
        '                strSql = " SELECT ITEMID,TAGNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGKEY = '" & Trim(txtItemId.Text) & "'"
        '                Dim dtItemDet As New DataTable
        '                da = New OleDbDataAdapter(strSql, cn)
        '                da.Fill(dtItemDet)
        '                If dtItemDet.Rows.Count > 0 Then
        '                    txtItemId.Text = dtItemDet.Rows(0).Item("ITEMID").ToString
        '                    txtTagNo.Text = dtItemDet.Rows(0).Item("TAGNO").ToString
        '                    GoTo CheckItem
        '                Else
        '                    LoadSalesItemName()
        '                End If
        '            Else
        '                LoadSalesItemNameDetail()
        '            End If
        '            If sp.Length > 1 And PRODTAGSEP <> "" And txtItemId.Text <> "" Then
        '                txtTagNo.Text = Trim(sp(1))
        '            End If
        '            If txtTagNo.Text <> "" Then
        '                txtTagNo.Focus()
        '                txtTagNo_KeyPress(Me, New KeyPressEventArgs(e.KeyChar))
        '            End If
        '        End If
    End Sub

    Private Sub txtSearch_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSearch.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbSearchKey.Text = "HM_BILLNO" And txtSearch.Text <> "" Then
                strSql = "SELECT CASE WHEN STKTYPE='M' THEN 'MANUFACTURING' WHEN STKTYPE='E' THEN 'EXEMPTED' ELSE 'TRADING' END AS STKTYPE"
                strSql += " FROM " & cnAdminDb & "..ITEMTAG WHERE ISSDATE IS NULL AND HM_BILLNO='" & txtSearch.Text & "'"
                CmbStockType.Text = objGPack.GetSqlValue(strSql, "STKTYPE", "TRADING").ToString
            End If
        End If
    End Sub


    Private Sub FindToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FindToolStripMenuItem.Click
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        If Not gridView.RowCount > 0 Then Exit Sub
        gridView.Select()
        Dim objSearch As New frmGridSearch(gridView, 1)
        objSearch.ShowDialog()
    End Sub


    Private Sub chkWithTransferStk_CheckedChanged(sender As Object, e As EventArgs) Handles chkWithTransferStk.CheckedChanged
        If dtGridView Is Nothing Then Exit Sub
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub cmbCostCentre_MAN_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cmbCostCentre_MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            StateId = Val(objGPack.GetSqlValue("SELECT STATEID FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & CmbAcname.Text & "'").ToString)
            If StateId = 0 Then
                MsgBox("Please Update State for the Account [" & CmbAcname.Text & "]", MsgBoxStyle.Information)
                Exit Sub
            End If
        End If
    End Sub
End Class

Public Class frmStkTransfer_Properties
    Private cmbMetal As String = "ALL"
    Public Property p_cmbMetal() As String
        Get
            Return cmbMetal
        End Get
        Set(ByVal value As String)
            cmbMetal = value
        End Set
    End Property
    Private cmbCounter As String = "ALL"
    Public Property p_cmbCounter() As String
        Get
            Return cmbCounter
        End Get
        Set(ByVal value As String)
            cmbCounter = value
        End Set
    End Property
End Class
