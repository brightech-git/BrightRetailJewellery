Imports System.Data.OleDb
Public Class frmPendingTransfer
    Dim strSql As String
    Dim dtGridView As New DataTable
    Dim dtCatstk As New DataTable
    Dim chkColumn As New DataGridViewCheckBoxColumn
    Dim Dtitemnontag As New DataTable

    Dim cmd As OleDbCommand
    Dim tran As OleDbTransaction
    Dim dr As OleDbDataReader
    Dim BillDate As Date
    Dim Batchno As String = ""
    Dim _JobNoEnable As Boolean = IIf(GetAdmindbSoftValue("MRMIJOBNO", "N") = "Y", True, False)
    Dim _OG_EXPORT As Boolean = IIf(GetAdmindbSoftValue("OG_EXPORT", "N") = "Y", True, False)
    Public objSoftKeys As New SoftKeys
    Dim BJobNo As String = ""
    Dim IssNo As Integer = 0
    Dim CENTR_DB_BR As Boolean = IIf(GetAdmindbSoftValue("CENTR_DB_ALLCOSTID", "N") = "Y", True, False)
    Dim PREYEAR_RECORD As Boolean = IIf(GetAdmindbSoftValue("PENDTRF_PREYEAR", "N") = "Y", True, False)
    Dim PENDTRF_LOAD_ACNAME As Boolean = IIf(GetAdmindbSoftValue("PENDTRF_LOADACNAME", "N") = "Y", True, False)
    Dim PENDTRF_NETCALC As Boolean = IIf(GetAdmindbSoftValue("PENDTRF_NETCALC", "N") = "Y", True, False)
    Dim MetalBasedStone As Boolean = IIf(GetAdmindbSoftValue("MULTIMETALBASEDSTONE", "N") = "Y", True, False)
    Dim PSSR_CATPOST As String = GetAdmindbSoftValue("PSSR_CATEGORYPOST", "")
    Dim PSSR_CATPOSTING() As String

    Function funcNew() As Integer
        BillDate = GetEntryDate(GetServerDate)
        chkAll.Checked = True
        cmbTransferTo.Visible = False
        dtGridView.Rows.Clear()
        Batchno = ""
        BJobNo = ""
        IssNo = 0
        If _OG_EXPORT = True Then
            chkPosting.Visible = True
        Else
            chkPosting.Visible = False
        End If
        If cmbCostCentre_MAN.Enabled Then
            cmbCostCentre_MAN.Text = "ALL"
            cmbCostCentre_MAN.Focus()
        Else
            chkPurchase.Focus()
        End If
        grpPurchase.Visible = False
        chkPurchaseOwn.Checked = True
        chkPurchaseExchange.Checked = True
        CalcGridViewTotal()
        cmbEntryType_MAN.Text = "LOT"

        lblAcname.Visible = False
        CmbAcname_MAN.Visible = False
        CmbAcname_MAN.Text = "ALL"
        CmbAcname_MAN.Items.Clear()
        strSql = " SELECT 'ALL' ACNAME,1 RESULT UNION ALL SELECT ACNAME,2 RESULT FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE IN('G','D') "
        strSql += GetAcNameQryFilteration()
        strSql += vbCrLf + " ORDER BY RESULT,ACNAME"
        objGPack.FillCombo(strSql, CmbAcname_MAN, True, False)
        CmbAcname_MAN.Text = "ALL"
        cmbEntryType_MAN.Enabled = True
        cmbOpenMetal.Text = "ALL"
        cmbCompany_MAN.Text = "ALL"
        cmbOpenCompanyView.Text = "ALL"
    End Function
    Private Sub chkAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAll.CheckedChanged
        For cnt As Integer = 0 To gridView_OWN.RowCount - 1
            If PSSR_CATPOST <> "" Then
                gridView_OWN.CurrentCell = gridView_OWN.Rows(cnt).Cells("CHECK")
                chkpssrcolumns()
            Else
                gridView_OWN.Rows(cnt).Cells(0).Value = chkAll.Checked
            End If

        Next
    End Sub

    Private Sub chkpssrcolumns()
        Dim Sno As String = Nothing
        Dim BatNo As String = Nothing
        Dim dv As New DataView
        Dim bool As Boolean = chkAll.Checked
        BatNo = gridView_OWN.CurrentRow.Cells("BATCHNO").Value.ToString
        dv = New DataView
        dv = CType(gridView_OWN.DataSource, DataTable).Copy.DefaultView
        dv.RowFilter = "BATCHNO = '" & BatNo & "'"
        Dim dt As New DataTable
        dt = dv.ToTable
        Dim CheckState As Boolean
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
            For cnt1 As Integer = 0 To gridView_OWN.Rows.Count - 1
                If Not gridView_OWN.Rows(cnt1).Cells("SNO").Value.ToString = dt.Rows(cnt).Item("SNO").ToString Then Continue For
                gridView_OWN.Rows(cnt1).Cells("CHECK").Value = bool
            Next
        Next
    End Sub

    Private Sub btnLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoad.Click
        If chkToType.Checked Then cmbEntryType_MAN.Enabled = False
        strSql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COMPANYID'"
        Dim str2CompId As String = objGPack.GetSqlValue(strSql, , , tran)
        If str2CompId = strCompanyId And chkPosting.Checked Then chkPosting.Checked = False : MsgBox("Posting not Available for this Company...") : Exit Sub
        Dim pretrandb As String = ""
        Dim tdate As Date = dtpFrom.Value
        If PREYEAR_RECORD Then
            pretrandb = cnCompanyId + "T" + Mid(tdate.Year - 1, 3, 2) + Mid(tdate.Year, 3, 2)
            If Not objGPack.GetSqlValue("SELECT NAME FROM SYSDATABASES WHERE NAME = '" & pretrandb & "'").Length > 0 Then
                pretrandb = ""
            End If
        End If
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        If Not chkPurchase.Checked And Not chkMiscIssue.Checked And Not chkPartlySale.Checked And Not chkSalesReturn.Checked And Not chkPosting.Checked Then MsgBox("Please select one Type") : btnNew_Click(Me, e) : Exit Sub
        btnLoad.Enabled = False
        chkAll.Checked = True
        dtGridView.Rows.Clear()
        Dim qrystr As Boolean = False
        Dim CostId As String = ""
        strSql = ""
        If chkPurchase.Checked Then
            strSql = " SELECT C.METALID, "
            strSql += vbCrLf + " 'PURCHASE' AS TYPE"
            strSql += vbCrLf + " ,CONVERT(VARCHAR,TRANDATE,103)AS DATE,A.CATCODE,C.CATNAME,TRANNO,I.ITEMNAME"
            strSql += vbCrLf + " ,A.PCS,A.GRSWT,ISNULL(A.DUSTWT,0) AS DUSTWT"
            'strSql += vbcrlf + " ,ISNULL(A.GRSWT,0)-ISNULL(A.DUSTWT,0) AS NETWT"
            'strSql += vbCrLf + " ,ISNULL(NETWT,0) AS NETWT"
            If PENDTRF_NETCALC Then
                strSql += vbCrLf + " ,ISNULL(A.GRSWT,0)-ISNULL(A.DUSTWT,0) AS NETWT"
            Else
                strSql += vbCrLf + " ,ISNULL(NETWT,0) AS NETWT"
            End If
            strSql += vbCrLf + " ,ISNULL(A.PURITY,100) TOUCH"
            strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),ROUND((ISNULL(A.GRSWT,0)-ISNULL(A.DUSTWT,0))*(ISNULL(A.PURITY,100)/100),3)) PUREWT, A.NETWT AS CNETWT"
            strSql += vbCrLf + " ,CONVERT(DECIMAL(15,2),RATE)RATE,AMOUNT,BATCHNO,A.COSTID,E.EMPNAME"
            strSql += vbCrLf + " ,A.TRANTYPE TYPE,TRANDATE,SNO,A.PCS AS OPCS,A.GRSWT OGRSWT,NETWT ONETWT"
            strSql += vbCrLf + ",CONVERT(DECIMAL(15,3),ROUND(A.NETWT*(ISNULL(A.PURITY,100)/100),3)) OPUREWT"
            '
            strSql += vbCrLf + " ,(SELECT TOP 1 DESIGNERID FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO=A.TAGNO) AS DESIGNERID"
            strSql += vbCrLf + " ,(SELECT TOP 1 TABLECODE FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO=A.TAGNO) AS TABLECODE"
            '
            strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),(SELECT SUM(CASE WHEN STONEUNIT='C' THEN STNWT/5 ELSE STNWT END)STNWT FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO=A.SNO AND STNITEMID IN"
            strSql += vbCrLf + " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID='T'))) AS STNWT"
            strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),(SELECT SUM(STNWT)STNWT FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO=A.SNO AND STNITEMID IN"
            strSql += vbCrLf + " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID='D'))) AS DIAWT,'" & cnStockDb.ToString & "' TDBNAME "
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT A"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON A.CATCODE = C.CATCODE"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON A.ITEMID= I.ITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..EMPMASTER E ON A.EMPID= E.EMPID "
            strSql += vbCrLf + " WHERE A.TRANTYPE <> '' AND ISNULL(CANCEL,'') = '' "
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            If cmbOpenMetal.Text <> "ALL" Then strSql += vbCrLf + " AND C.METALID = ISNULL((SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME= '" & cmbOpenMetal.Text & "'),'')"
            If CmbCategory.Text <> "ALL" And CmbCategory.Text <> "" Then strSql += vbCrLf + " AND C.CATNAME= '" & CmbCategory.Text & "'"
            If chkPurchase.Checked And GetAdmindbSoftValue("PENDTRF_EXCLUDECATCODE", "") <> "" Then strSql += vbCrLf + "AND C.CATCODE NOT IN('" & GetAdmindbSoftValue("PENDTRF_EXCLUDECATCODE").ToString.Replace(",", "','") & "')"
            If cmbCompany_MAN.Text <> "ALL" And cmbCompany_MAN.Text <> "" Then strSql += vbCrLf + " AND A.COMPANYID = ISNULL((SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME= '" & cmbCompany_MAN.Text & "'),'')"
            strSql += vbCrLf + " AND ("
            strSql += vbCrLf + " A.TRANTYPE = ''"
            strSql += vbCrLf + " OR A.TRANTYPE = 'PU' "
            strSql += vbCrLf + " )"
            If chkPurchase.Checked And (chkPurchaseOwn.Checked = False) Then
                strSql += vbCrLf + " AND ISNULL(A.MAKE,'') <> 'W' "
            ElseIf chkPurchase.Checked And (chkPurchaseExchange.Checked = False) Then
                strSql += vbCrLf + " AND ISNULL(A.MAKE,'') <> 'O' "
            End If
            If cmbCostCentre_MAN.Enabled Then
                If UCase(cmbCostCentre_MAN.Text) <> "ALL" And cmbCostCentre_MAN.Text <> "" Then
                    strSql += vbCrLf + " AND A.COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "')"
                End If
            End If
            strSql += vbCrLf + " AND ISNULL(BAGNO,'') =''"
            strSql += vbCrLf + " AND ISNULL(MELT_RETAG,'') =''"
            strSql += vbCrLf + " AND NOT EXISTS (SELECT * FROM " & cnStockDb & "..ISSUE WHERE RTRIM(LTRIM(REMARK1))='SECOND HAND SALES' AND ISNULL(CANCEL,'')='' AND RUNNO=A.SNO AND A.TRANTYPE='PU')"
            If PREYEAR_RECORD And pretrandb <> "" Then
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT C.METALID, "
                strSql += vbCrLf + " 'PURCHASE' AS TYPE"
                strSql += vbCrLf + " ,CONVERT(VARCHAR,TRANDATE,103)AS DATE,A.CATCODE,C.CATNAME,TRANNO,I.ITEMNAME"
                strSql += vbCrLf + " ,A.PCS,A.GRSWT,ISNULL(A.DUSTWT,0) AS DUSTWT"
                'strSql += vbcrlf + " ,ISNULL(A.GRSWT,0)-ISNULL(A.DUSTWT,0) AS NETWT"
                'strSql += vbCrLf + " ,ISNULL(NETWT,0) AS NETWT"
                If PENDTRF_NETCALC Then
                    strSql += vbCrLf + " ,ISNULL(A.GRSWT,0)-ISNULL(A.DUSTWT,0) AS NETWT"
                Else
                    strSql += vbCrLf + " ,ISNULL(NETWT,0) AS NETWT"
                End If
                strSql += vbCrLf + " ,ISNULL(A.PURITY,100) TOUCH"
                strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),ROUND((ISNULL(A.GRSWT,0)-ISNULL(A.DUSTWT,0))*(ISNULL(A.PURITY,100)/100),3)) PUREWT, A.NETWT AS CNETWT"
                strSql += vbCrLf + " ,CONVERT(DECIMAL(15,2),RATE)RATE,AMOUNT,BATCHNO,A.COSTID,E.EMPNAME"
                strSql += vbCrLf + " ,A.TRANTYPE TYPE,TRANDATE,SNO,A.PCS AS OPCS,A.GRSWT OGRSWT,NETWT ONETWT"
                strSql += vbCrLf + ",CONVERT(DECIMAL(15,3),ROUND(A.NETWT*(ISNULL(A.PURITY,100)/100),3)) OPUREWT"
                '
                strSql += vbCrLf + " ,(SELECT TOP 1 DESIGNERID FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO=A.TAGNO) AS DESIGNERID"
                strSql += vbCrLf + " ,(SELECT TOP 1 TABLECODE FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO=A.TAGNO) AS TABLECODE"
                '
                strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),(SELECT SUM(CASE WHEN STONEUNIT='C' THEN STNWT/5 ELSE STNWT END)STNWT FROM " & pretrandb & "..RECEIPTSTONE WHERE ISSSNO=A.SNO AND STNITEMID IN"
                strSql += vbCrLf + " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID='T'))) AS STNWT"
                strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),(SELECT SUM(STNWT)STNWT FROM " & pretrandb & "..RECEIPTSTONE WHERE ISSSNO=A.SNO AND STNITEMID IN"
                strSql += vbCrLf + " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID='D'))) AS DIAWT ,'" & pretrandb.ToString & "' TDBNAME"
                strSql += vbCrLf + " FROM " & pretrandb & "..RECEIPT A"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON A.CATCODE = C.CATCODE"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON A.ITEMID= I.ITEMID"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..EMPMASTER E ON A.EMPID= E.EMPID "
                strSql += vbCrLf + " WHERE A.TRANTYPE <> '' AND ISNULL(CANCEL,'') = '' "
                strSql += vbCrLf + " AND TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
                If cmbOpenMetal.Text <> "ALL" Then strSql += vbCrLf + " AND C.METALID = ISNULL((SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME= '" & cmbOpenMetal.Text & "'),'')"
                If CmbCategory.Text <> "ALL" And CmbCategory.Text <> "" Then strSql += vbCrLf + " AND C.CATNAME= '" & CmbCategory.Text & "'"
                If chkPurchase.Checked And GetAdmindbSoftValue("PENDTRF_EXCLUDECATCODE", "") <> "" Then strSql += vbCrLf + "AND C.CATCODE NOT IN('" & GetAdmindbSoftValue("PENDTRF_EXCLUDECATCODE").ToString.Replace(",", "','") & "')"
                If cmbCompany_MAN.Text <> "ALL" And cmbCompany_MAN.Text <> "" Then strSql += vbCrLf + " AND A.COMPANYID = ISNULL((SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME= '" & cmbCompany_MAN.Text & "'),'')"
                strSql += vbCrLf + " AND ("
                strSql += vbCrLf + " A.TRANTYPE = ''"
                strSql += vbCrLf + " OR A.TRANTYPE = 'PU' "
                strSql += vbCrLf + " )"
                If chkPurchase.Checked And (chkPurchaseOwn.Checked = False) Then
                    strSql += vbCrLf + " AND ISNULL(A.MAKE,'') <> 'W' "
                ElseIf chkPurchase.Checked And (chkPurchaseExchange.Checked = False) Then
                    strSql += vbCrLf + " AND ISNULL(A.MAKE,'') <> 'O' "
                End If
                If cmbCostCentre_MAN.Enabled Then
                    If UCase(cmbCostCentre_MAN.Text) <> "ALL" And cmbCostCentre_MAN.Text <> "" Then
                        strSql += vbCrLf + " AND A.COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "')"
                    End If
                End If
                strSql += vbCrLf + " AND ISNULL(BAGNO,'') =''"
                strSql += vbCrLf + " AND ISNULL(MELT_RETAG,'') =''"
                strSql += vbCrLf + " AND NOT EXISTS (SELECT * FROM " & pretrandb & "..ISSUE WHERE RTRIM(LTRIM(REMARK1))='SECOND HAND SALES' AND ISNULL(CANCEL,'')='' AND RUNNO=A.SNO AND A.TRANTYPE='PU')"
            End If
            qrystr = True
        End If
        If chkSalesReturn.Checked Then
            If qrystr Then strSql += vbCrLf + " UNION ALL"
            strSql = " SELECT C.METALID, "
            strSql += vbCrLf + " 'SALES RETURN' AS TYPE"
            strSql += vbCrLf + " ,CONVERT(VARCHAR,TRANDATE,103)AS DATE,A.CATCODE,C.CATNAME,TRANNO,I.ITEMNAME"
            strSql += vbCrLf + " ,A.PCS,A.GRSWT,ISNULL(A.DUSTWT,0) AS DUSTWT"
            If PENDTRF_NETCALC Then
                strSql += vbCrLf + " ,ISNULL(A.GRSWT,0)-ISNULL(A.DUSTWT,0) AS NETWT"
            Else
                strSql += vbCrLf + " ,ISNULL(NETWT,0) AS NETWT"
            End If
            strSql += vbCrLf + " ,ISNULL(A.PURITY,100) TOUCH"
            strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),ROUND((ISNULL(A.GRSWT,0)-ISNULL(A.DUSTWT,0))*(ISNULL(A.PURITY,100)/100),3)) PUREWT, A.NETWT AS CNETWT"
            strSql += vbCrLf + " ,CONVERT(DECIMAL(15,2),RATE)RATE,AMOUNT,BATCHNO,A.COSTID,E.EMPNAME"
            strSql += vbCrLf + " ,A.TRANTYPE TYPE,TRANDATE,SNO,A.PCS AS OPCS,A.GRSWT OGRSWT,NETWT ONETWT"
            strSql += vbCrLf + ",CONVERT(DECIMAL(15,3),ROUND(A.NETWT*(ISNULL(A.PURITY,100)/100),3)) OPUREWT"
            '
            strSql += vbCrLf + " ,(SELECT TOP 1 DESIGNERID FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO=A.TAGNO) AS DESIGNERID"
            strSql += vbCrLf + " ,(SELECT TOP 1 TABLECODE FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO=A.TAGNO) AS TABLECODE"
            '
            strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),(SELECT SUM(CASE WHEN STONEUNIT='C' THEN STNWT/5 ELSE STNWT END)STNWT FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO=A.SNO AND STNITEMID IN"
            strSql += vbCrLf + " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID='T'))) AS STNWT"
            strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),(SELECT SUM(STNWT)STNWT FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO=A.SNO AND STNITEMID IN"
            strSql += vbCrLf + " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID='D'))) AS DIAWT,'" & cnStockDb.ToString & "' TDBNAME "
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT A"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON A.CATCODE = C.CATCODE"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON A.ITEMID= I.ITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..EMPMASTER E ON A.EMPID= E.EMPID "
            strSql += vbCrLf + " WHERE A.TRANTYPE <> '' AND ISNULL(CANCEL,'') = '' "
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            If cmbOpenMetal.Text <> "ALL" Then strSql += vbCrLf + " AND C.METALID = ISNULL((SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME= '" & cmbOpenMetal.Text & "'),'')"
            If CmbCategory.Text <> "ALL" And CmbCategory.Text <> "" Then strSql += vbCrLf + " AND C.CATNAME= '" & CmbCategory.Text & "'"
            If chkPurchase.Checked And GetAdmindbSoftValue("PENDTRF_EXCLUDECATCODE", "") <> "" Then strSql += vbCrLf + "AND C.CATCODE NOT IN('" & GetAdmindbSoftValue("PENDTRF_EXCLUDECATCODE").ToString.Replace(",", "','") & "')"
            If cmbCompany_MAN.Text <> "ALL" And cmbCompany_MAN.Text <> "" Then strSql += vbCrLf + " AND A.COMPANYID = ISNULL((SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME= '" & cmbCompany_MAN.Text & "'),'')"
            strSql += vbCrLf + " AND ("
            strSql += vbCrLf + " A.TRANTYPE = ''"
            strSql += vbCrLf + " OR A.TRANTYPE = 'SR'"
            strSql += vbCrLf + " )"
            If cmbCostCentre_MAN.Enabled Then
                If UCase(cmbCostCentre_MAN.Text) <> "ALL" And cmbCostCentre_MAN.Text <> "" Then
                    strSql += vbCrLf + " AND A.COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "')"
                End If
            End If
            strSql += vbCrLf + " AND ISNULL(BAGNO,'') =''"
            strSql += vbCrLf + " AND ISNULL(MELT_RETAG,'') =''"
            strSql += vbCrLf + " AND NOT EXISTS (SELECT * FROM " & cnStockDb & "..ISSUE WHERE RTRIM(LTRIM(REMARK1))='SECOND HAND SALES' AND ISNULL(CANCEL,'')='' AND RUNNO=A.SNO AND A.TRANTYPE='PU')"
            If MetalBasedStone Then '' added for vbj on 04-06-2022 as per magesh sir advice
                strSql += vbCrLf + " AND NOT EXISTS (SELECT * FROM " & cnStockDb & "..RECEIPTMETAL WHERE ISSSNO=A.SNO)"
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT C.METALID, "
                strSql += vbCrLf + " 'SALES RETURN' AS TYPE"
                strSql += vbCrLf + " ,CONVERT(VARCHAR,A.TRANDATE,103)AS DATE,B.CATCODE,C.CATNAME,A.TRANNO,I.ITEMNAME"
                strSql += vbCrLf + " ,0 PCS,B.GRSWT,0 AS DUSTWT"
                strSql += vbCrLf + " ,CASE WHEN ISNULL(B.NETWT,0)<>0 THEN ISNULL(B.NETWT,0) ELSE ISNULL(B.GRSWT,0) END AS NETWT"
                strSql += vbCrLf + " ,ISNULL(A.PURITY,100) TOUCH"
                strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),ROUND((ISNULL(B.GRSWT,0))*(ISNULL(A.PURITY,100)/100),3)) PUREWT"
                strSql += vbCrLf + " ,CASE WHEN ISNULL(B.NETWT,0)<>0 THEN ISNULL(B.NETWT,0) ELSE ISNULL(B.GRSWT,0) END AS CNETWT"
                strSql += vbCrLf + " ,CONVERT(DECIMAL(15,2),B.RATE)RATE,B.AMOUNT,A.BATCHNO,A.COSTID,E.EMPNAME"
                strSql += vbCrLf + " ,A.TRANTYPE TYPE,A.TRANDATE,A.SNO,A.PCS AS OPCS,B.GRSWT OGRSWT,CASE WHEN ISNULL(B.NETWT,0)<>0 THEN ISNULL(B.NETWT,0) ELSE ISNULL(B.GRSWT,0) END ONETWT"
                strSql += vbCrLf + ",CONVERT(DECIMAL(15,3),ROUND((CASE WHEN ISNULL(B.NETWT,0)<>0 THEN ISNULL(B.NETWT,0) ELSE ISNULL(B.GRSWT,0) END)*(ISNULL(A.PURITY,100)/100),3)) OPUREWT"
                '
                strSql += vbCrLf + " ,(SELECT TOP 1 DESIGNERID FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO=A.TAGNO) AS DESIGNERID"
                strSql += vbCrLf + " ,(SELECT TOP 1 TABLECODE FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO=A.TAGNO) AS TABLECODE"
                '
                strSql += vbCrLf + " ,CASE WHEN A.CATCODE=B.CATCODE THEN CONVERT(DECIMAL(15,3),(SELECT SUM(CASE WHEN STONEUNIT='C' THEN STNWT/5 ELSE STNWT END)STNWT FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO=A.SNO AND STNITEMID IN"
                strSql += vbCrLf + " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID='T'))) ELSE NULL END AS STNWT"
                strSql += vbCrLf + " ,CASE WHEN A.CATCODE=B.CATCODE THEN CONVERT(DECIMAL(15,3),(SELECT SUM(STNWT)STNWT FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO=A.SNO AND STNITEMID IN"
                strSql += vbCrLf + " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID='D'))) ELSE NULL END AS DIAWT"
                strSql += vbCrLf + " ,'" & cnStockDb.ToString & "' TDBNAME "
                strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT A"
                strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..RECEIPTMETAL B ON B.ISSSNO=A.SNO"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON B.CATCODE = C.CATCODE"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON A.ITEMID= I.ITEMID"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..EMPMASTER E ON A.EMPID= E.EMPID "
                strSql += vbCrLf + " WHERE A.TRANTYPE <> '' AND ISNULL(A.CANCEL,'') = '' "
                strSql += vbCrLf + " AND A.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
                If cmbOpenMetal.Text <> "ALL" Then strSql += vbCrLf + " AND C.METALID = ISNULL((SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME= '" & cmbOpenMetal.Text & "'),'')"
                If CmbCategory.Text <> "ALL" And CmbCategory.Text <> "" Then strSql += vbCrLf + " AND C.CATNAME= '" & CmbCategory.Text & "'"
                If chkPurchase.Checked And GetAdmindbSoftValue("PENDTRF_EXCLUDECATCODE", "") <> "" Then strSql += vbCrLf + "AND C.CATCODE NOT IN('" & GetAdmindbSoftValue("PENDTRF_EXCLUDECATCODE").ToString.Replace(",", "','") & "')"
                If cmbCompany_MAN.Text <> "ALL" And cmbCompany_MAN.Text <> "" Then strSql += vbCrLf + " AND A.COMPANYID = ISNULL((SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME= '" & cmbCompany_MAN.Text & "'),'')"
                strSql += vbCrLf + " AND ("
                strSql += vbCrLf + " A.TRANTYPE = ''"
                strSql += vbCrLf + " OR A.TRANTYPE = 'SR'"
                strSql += vbCrLf + " )"
                If cmbCostCentre_MAN.Enabled Then
                    If UCase(cmbCostCentre_MAN.Text) <> "ALL" And cmbCostCentre_MAN.Text <> "" Then
                        strSql += vbCrLf + " AND A.COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "')"
                    End If
                End If
                strSql += vbCrLf + " AND ISNULL(BAGNO,'') =''"
                strSql += vbCrLf + " AND ISNULL(MELT_RETAG,'') =''"
                strSql += vbCrLf + " AND NOT EXISTS (SELECT * FROM " & cnStockDb & "..ISSUE WHERE RTRIM(LTRIM(REMARK1))='SECOND HAND SALES' AND ISNULL(CANCEL,'')='' AND RUNNO=A.SNO AND A.TRANTYPE='PU')"
                strSql += vbCrLf + " AND EXISTS (SELECT * FROM " & cnStockDb & "..RECEIPTMETAL WHERE ISSSNO=A.SNO)"
            End If

            If PREYEAR_RECORD And pretrandb <> "" Then
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT C.METALID, "
                strSql += vbCrLf + " 'SALES RETURN' AS TYPE"
                strSql += vbCrLf + " ,CONVERT(VARCHAR,TRANDATE,103)AS DATE,A.CATCODE,C.CATNAME,TRANNO,I.ITEMNAME"
                strSql += vbCrLf + " ,A.PCS,A.GRSWT,ISNULL(A.DUSTWT,0) AS DUSTWT"
                If PENDTRF_NETCALC Then
                    strSql += vbCrLf + " ,ISNULL(A.GRSWT,0)-ISNULL(A.DUSTWT,0) AS NETWT"
                Else
                    strSql += vbCrLf + " ,ISNULL(NETWT,0) AS NETWT"
                End If
                strSql += vbCrLf + " ,ISNULL(A.PURITY,100) TOUCH"
                strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),ROUND((ISNULL(A.GRSWT,0)-ISNULL(A.DUSTWT,0))*(ISNULL(A.PURITY,100)/100),3)) PUREWT, A.NETWT AS CNETWT"
                strSql += vbCrLf + " ,CONVERT(DECIMAL(15,2),RATE)RATE,AMOUNT,BATCHNO,A.COSTID,E.EMPNAME"
                strSql += vbCrLf + " ,A.TRANTYPE TYPE,TRANDATE,SNO,A.PCS AS OPCS,A.GRSWT OGRSWT,NETWT ONETWT"
                strSql += vbCrLf + ",CONVERT(DECIMAL(15,3),ROUND(A.NETWT*(ISNULL(A.PURITY,100)/100),3)) OPUREWT"
                '
                strSql += vbCrLf + " ,(SELECT TOP 1 DESIGNERID FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO=A.TAGNO) AS DESIGNERID"
                strSql += vbCrLf + " ,(SELECT TOP 1 TABLECODE FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO=A.TAGNO) AS TABLECODE"
                '
                strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),(SELECT SUM(CASE WHEN STONEUNIT='C' THEN STNWT/5 ELSE STNWT END)STNWT FROM " & pretrandb & "..RECEIPTSTONE WHERE ISSSNO=A.SNO AND STNITEMID IN"
                strSql += vbCrLf + " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID='T'))) AS STNWT"
                strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),(SELECT SUM(STNWT)STNWT FROM " & pretrandb & "..RECEIPTSTONE WHERE ISSSNO=A.SNO AND STNITEMID IN"
                strSql += vbCrLf + " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID='D'))) AS DIAWT ,'" & pretrandb.ToString & "' TDBNAME"
                strSql += vbCrLf + " FROM " & pretrandb & "..RECEIPT A"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON A.CATCODE = C.CATCODE"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON A.ITEMID= I.ITEMID"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..EMPMASTER E ON A.EMPID= E.EMPID "
                strSql += vbCrLf + " WHERE A.TRANTYPE <> '' AND ISNULL(CANCEL,'') = '' "
                strSql += vbCrLf + " AND TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
                If cmbOpenMetal.Text <> "ALL" Then strSql += vbCrLf + " AND C.METALID = ISNULL((SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME= '" & cmbOpenMetal.Text & "'),'')"
                If CmbCategory.Text <> "ALL" And CmbCategory.Text <> "" Then strSql += vbCrLf + " AND C.CATNAME= '" & CmbCategory.Text & "'"
                If chkPurchase.Checked And GetAdmindbSoftValue("PENDTRF_EXCLUDECATCODE", "") <> "" Then strSql += vbCrLf + "AND C.CATCODE NOT IN('" & GetAdmindbSoftValue("PENDTRF_EXCLUDECATCODE").ToString.Replace(",", "','") & "')"
                If cmbCompany_MAN.Text <> "ALL" And cmbCompany_MAN.Text <> "" Then strSql += vbCrLf + " AND A.COMPANYID = ISNULL((SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME= '" & cmbCompany_MAN.Text & "'),'')"
                strSql += vbCrLf + " AND ("
                strSql += vbCrLf + " A.TRANTYPE = ''"
                strSql += vbCrLf + " OR A.TRANTYPE = 'SR'"
                strSql += vbCrLf + " )"
                If chkPurchase.Checked And (chkPurchaseOwn.Checked = False) Then
                    strSql += vbCrLf + " AND ISNULL(A.MAKE,'') <> 'W' "
                ElseIf chkPurchase.Checked And (chkPurchaseExchange.Checked = False) Then
                    strSql += vbCrLf + " AND ISNULL(A.MAKE,'') <> 'O' "
                End If
                If cmbCostCentre_MAN.Enabled Then
                    If UCase(cmbCostCentre_MAN.Text) <> "ALL" And cmbCostCentre_MAN.Text <> "" Then
                        strSql += vbCrLf + " AND A.COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "')"
                    End If
                End If
                strSql += vbCrLf + " AND ISNULL(BAGNO,'') =''"
                strSql += vbCrLf + " AND ISNULL(MELT_RETAG,'') =''"
                strSql += vbCrLf + " AND NOT EXISTS (SELECT * FROM " & pretrandb & "..ISSUE WHERE RTRIM(LTRIM(REMARK1))='SECOND HAND SALES' AND ISNULL(CANCEL,'')='' AND RUNNO=A.SNO AND A.TRANTYPE='PU')"

                If MetalBasedStone Then '' added for vbj on 04-06-2022 as per magesh sir advice
                    strSql += vbCrLf + " AND NOT EXISTS (SELECT * FROM " & pretrandb & "..RECEIPTMETAL WHERE ISSSNO=A.SNO)"
                    strSql += vbCrLf + " UNION ALL"
                    strSql += vbCrLf + " SELECT C.METALID, "
                    strSql += vbCrLf + " 'SALES RETURN' AS TYPE"
                    strSql += vbCrLf + " ,CONVERT(VARCHAR,A.TRANDATE,103)AS DATE,B.CATCODE,C.CATNAME,A.TRANNO,I.ITEMNAME"
                    strSql += vbCrLf + " ,0 PCS,B.GRSWT,0 AS DUSTWT"
                    strSql += vbCrLf + " ,CASE WHEN ISNULL(B.NETWT,0)<>0 THEN ISNULL(B.NETWT,0) ELSE ISNULL(B.GRSWT,0) END AS NETWT"
                    strSql += vbCrLf + " ,ISNULL(A.PURITY,100) TOUCH"
                    strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),ROUND((ISNULL(B.GRSWT,0))*(ISNULL(A.PURITY,100)/100),3)) PUREWT"
                    strSql += vbCrLf + " ,CASE WHEN ISNULL(B.NETWT,0)<>0 THEN ISNULL(B.NETWT,0) ELSE ISNULL(B.GRSWT,0) END AS CNETWT"
                    strSql += vbCrLf + " ,CONVERT(DECIMAL(15,2),B.RATE)RATE,B.AMOUNT,A.BATCHNO,A.COSTID,E.EMPNAME"
                    strSql += vbCrLf + " ,A.TRANTYPE TYPE,A.TRANDATE,A.SNO,A.PCS AS OPCS,B.GRSWT OGRSWT,CASE WHEN ISNULL(B.NETWT,0)<>0 THEN ISNULL(B.NETWT,0) ELSE ISNULL(B.GRSWT,0) END ONETWT"
                    strSql += vbCrLf + ",CONVERT(DECIMAL(15,3),ROUND((CASE WHEN ISNULL(B.NETWT,0)<>0 THEN ISNULL(B.NETWT,0) ELSE ISNULL(B.GRSWT,0) END)*(ISNULL(A.PURITY,100)/100),3)) OPUREWT"
                    '
                    strSql += vbCrLf + " ,(SELECT TOP 1 DESIGNERID FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO=A.TAGNO) AS DESIGNERID"
                    strSql += vbCrLf + " ,(SELECT TOP 1 TABLECODE FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO=A.TAGNO) AS TABLECODE"
                    '
                    strSql += vbCrLf + " ,CASE WHEN A.CATCODE=B.CATCODE THEN CONVERT(DECIMAL(15,3),(SELECT SUM(CASE WHEN STONEUNIT='C' THEN STNWT/5 ELSE STNWT END)STNWT FROM " & pretrandb & "..RECEIPTSTONE WHERE ISSSNO=A.SNO AND STNITEMID IN"
                    strSql += vbCrLf + " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID='T'))) ELSE NULL END AS STNWT"
                    strSql += vbCrLf + " ,CASE WHEN A.CATCODE=B.CATCODE THEN CONVERT(DECIMAL(15,3),(SELECT SUM(STNWT)STNWT FROM " & pretrandb & "..RECEIPTSTONE WHERE ISSSNO=A.SNO AND STNITEMID IN"
                    strSql += vbCrLf + " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID='D'))) ELSE NULL END AS DIAWT"
                    strSql += vbCrLf + " ,'" & pretrandb.ToString & "' TDBNAME "
                    strSql += vbCrLf + " FROM " & pretrandb & "..RECEIPT A"
                    strSql += vbCrLf + " INNER JOIN " & pretrandb & "..RECEIPTMETAL B ON B.ISSSNO=A.SNO"
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON B.CATCODE = C.CATCODE"
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON A.ITEMID= I.ITEMID"
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..EMPMASTER E ON A.EMPID= E.EMPID "
                    strSql += vbCrLf + " WHERE A.TRANTYPE <> '' AND ISNULL(A.CANCEL,'') = '' "
                    strSql += vbCrLf + " AND A.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
                    If cmbOpenMetal.Text <> "ALL" Then strSql += vbCrLf + " AND C.METALID = ISNULL((SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME= '" & cmbOpenMetal.Text & "'),'')"
                    If CmbCategory.Text <> "ALL" And CmbCategory.Text <> "" Then strSql += vbCrLf + " AND C.CATNAME= '" & CmbCategory.Text & "'"
                    If chkPurchase.Checked And GetAdmindbSoftValue("PENDTRF_EXCLUDECATCODE", "") <> "" Then strSql += vbCrLf + "AND C.CATCODE NOT IN('" & GetAdmindbSoftValue("PENDTRF_EXCLUDECATCODE").ToString.Replace(",", "','") & "')"
                    If cmbCompany_MAN.Text <> "ALL" And cmbCompany_MAN.Text <> "" Then strSql += vbCrLf + " AND A.COMPANYID = ISNULL((SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME= '" & cmbCompany_MAN.Text & "'),'')"
                    strSql += vbCrLf + " AND ("
                    strSql += vbCrLf + " A.TRANTYPE = ''"
                    strSql += vbCrLf + " OR A.TRANTYPE = 'SR'"
                    strSql += vbCrLf + " )"
                    If cmbCostCentre_MAN.Enabled Then
                        If UCase(cmbCostCentre_MAN.Text) <> "ALL" And cmbCostCentre_MAN.Text <> "" Then
                            strSql += vbCrLf + " AND A.COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "')"
                        End If
                    End If
                    strSql += vbCrLf + " AND ISNULL(BAGNO,'') =''"
                    strSql += vbCrLf + " AND ISNULL(MELT_RETAG,'') =''"
                    strSql += vbCrLf + " AND NOT EXISTS (SELECT * FROM " & pretrandb & "..ISSUE WHERE RTRIM(LTRIM(REMARK1))='SECOND HAND SALES' AND ISNULL(CANCEL,'')='' AND RUNNO=A.SNO AND A.TRANTYPE='PU')"
                    strSql += vbCrLf + " AND EXISTS (SELECT * FROM " & pretrandb & "..RECEIPTMETAL WHERE ISSSNO=A.SNO)"
                End If

            End If
            qrystr = True
        End If
        If chkMiscIssue.Checked Then
            If qrystr Then strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT C.METALID,"
            strSql += vbCrLf + " CASE WHEN A.TRANTYPE = 'MI' THEN 'MISCELLANIOUS' WHEN A.TRANTYPE = 'SR' THEN 'SALES RETURN' END AS TYPE"
            strSql += vbCrLf + " ,CONVERT(VARCHAR,TRANDATE,103)AS DATE,A.CATCODE,C.CATNAME,TRANNO,I.ITEMNAME"
            strSql += vbCrLf + " ,A.PCS,A.GRSWT,0 AS DUSTWT,NETWT,ISNULL(A.PURITY,100) TOUCH"
            strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),ROUND(ISNULL(A.GRSWT,0)*(ISNULL(A.PURITY,100)/100),3)) PUREWT,NETWT AS CNETWT"
            strSql += vbCrLf + " ,CONVERT(DECIMAL(15,2),RATE)RATE,AMOUNT,BATCHNO,A.COSTID,E.EMPNAME"
            strSql += vbCrLf + " ,A.TRANTYPE TYPE,TRANDATE,SNO,A.PCS AS OPCS,A.GRSWT OGRSWT,A.NETWT ONETWT"
            strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),ROUND(A.NETWT*(ISNULL(A.PURITY,100)/100),3)) OPUREWT"
            '
            strSql += vbCrLf + " ,(SELECT TOP 1 DESIGNERID FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO=A.TAGNO) AS DESIGNERID"
            strSql += vbCrLf + " ,(SELECT TOP 1 TABLECODE FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO=A.TAGNO) AS TABLECODE"
            '
            strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),0) AS STNWT"
            strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),0) AS DIAWT,'" & cnStockDb.ToString & "' TDBNAME "
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE A"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON A.CATCODE = C.CATCODE"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON A.ITEMID= I.ITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..EMPMASTER E ON A.EMPID= E.EMPID "
            strSql += vbCrLf + " WHERE A.TRANTYPE <> '' AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            If cmbOpenMetal.Text <> "ALL" Then strSql += vbCrLf + " AND C.METALID = ISNULL((SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME= '" & cmbOpenMetal.Text & "'),'')"
            If CmbCategory.Text <> "ALL" And CmbCategory.Text <> "" Then strSql += vbCrLf + " AND C.CATNAME= '" & CmbCategory.Text & "'"
            If cmbCompany_MAN.Text <> "ALL" And cmbCompany_MAN.Text <> "" Then strSql += vbCrLf + " AND A.COMPANYID = ISNULL((SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME= '" & cmbCompany_MAN.Text & "'),'')"
            strSql += vbCrLf + " AND"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " A.TRANTYPE = ''"
            If chkMiscIssue.Checked Then strSql += vbCrLf + " OR A.TRANTYPE = 'MI' "
            strSql += vbCrLf + " )"
            If cmbCostCentre_MAN.Enabled Then
                If UCase(cmbCostCentre_MAN.Text) <> "ALL" And cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND A.COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "')"
            End If
            strSql += vbCrLf + " AND ISNULL(BAGNO,'') =''"
            strSql += vbCrLf + " AND ISNULL(MELT_RETAG,'') =''"
            If PREYEAR_RECORD And pretrandb <> "" Then
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT C.METALID,"
                strSql += vbCrLf + " CASE WHEN A.TRANTYPE = 'MI' THEN 'MISCELLANIOUS' WHEN A.TRANTYPE = 'SR' THEN 'SALES RETURN' END AS TYPE"
                strSql += vbCrLf + " ,CONVERT(VARCHAR,TRANDATE,103)AS DATE,A.CATCODE,C.CATNAME,TRANNO,I.ITEMNAME"
                strSql += vbCrLf + " ,A.PCS,A.GRSWT,0 AS DUSTWT,NETWT,ISNULL(A.PURITY,100) TOUCH"
                strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),ROUND(ISNULL(A.GRSWT,0)*(ISNULL(A.PURITY,100)/100),3)) PUREWT,NETWT AS CNETWT"
                strSql += vbCrLf + " ,CONVERT(DECIMAL(15,2),RATE)RATE,AMOUNT,BATCHNO,A.COSTID,E.EMPNAME"
                strSql += vbCrLf + " ,A.TRANTYPE TYPE,TRANDATE,SNO,A.PCS AS OPCS,A.GRSWT OGRSWT,A.NETWT ONETWT"
                strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),ROUND(A.NETWT*(ISNULL(A.PURITY,100)/100),3)) OPUREWT"
                '
                strSql += vbCrLf + " ,(SELECT TOP 1 DESIGNERID FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO=A.TAGNO) AS DESIGNERID"
                strSql += vbCrLf + " ,(SELECT TOP 1 TABLECODE FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO=A.TAGNO) AS TABLECODE"
                '
                strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),0) AS STNWT"
                strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),0) AS DIAWT,'" & pretrandb.ToString & "' TDBNAME "
                strSql += vbCrLf + " FROM " & pretrandb & "..ISSUE A"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON A.CATCODE = C.CATCODE"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON A.ITEMID= I.ITEMID"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..EMPMASTER E ON A.EMPID= E.EMPID "
                strSql += vbCrLf + " WHERE A.TRANTYPE <> '' AND ISNULL(CANCEL,'') = ''"
                strSql += vbCrLf + " AND TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                If cmbOpenMetal.Text <> "ALL" Then strSql += vbCrLf + " AND C.METALID = ISNULL((SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME= '" & cmbOpenMetal.Text & "'),'')"
                If CmbCategory.Text <> "ALL" And CmbCategory.Text <> "" Then strSql += vbCrLf + " AND C.CATNAME= '" & CmbCategory.Text & "'"
                If cmbCompany_MAN.Text <> "ALL" And cmbCompany_MAN.Text <> "" Then strSql += vbCrLf + " AND A.COMPANYID = ISNULL((SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME= '" & cmbCompany_MAN.Text & "'),'')"
                strSql += vbCrLf + " AND"
                strSql += vbCrLf + " ("
                strSql += vbCrLf + " A.TRANTYPE = ''"
                If chkMiscIssue.Checked Then strSql += vbCrLf + " OR A.TRANTYPE = 'MI' "
                strSql += vbCrLf + " )"
                If cmbCostCentre_MAN.Enabled Then
                    If UCase(cmbCostCentre_MAN.Text) <> "ALL" And cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND A.COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "')"
                End If
                strSql += vbCrLf + " AND ISNULL(BAGNO,'') =''"
                strSql += vbCrLf + " AND ISNULL(MELT_RETAG,'') =''"
            End If
            qrystr = True
        End If
        If chkPartlySale.Checked Then
            If qrystr Then strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT C.METALID,'PARTLY SALES' AS TYPE"
            strSql += vbCrLf + " ,CONVERT(VARCHAR,TRANDATE,103)AS DATE,A.CATCODE,C.CATNAME,TRANNO,I.ITEMNAME"
            strSql += vbCrLf + " ,TAGPCS-A.PCS PCS, TAGGRSWT-A.GRSWT GRSWT, 0 AS DUSTWT,TAGNETWT-NETWT NETWT,ISNULL(A.PURITY,100) TOUCH "
            strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),ROUND((A.NETWT )*(ISNULL(A.PURITY,100)/100),3)) PUREWT,NETWT CNETWT"
            strSql += vbCrLf + " ,CONVERT(DECIMAL(15,2),RATE)RATE,AMOUNT,BATCHNO,A.COSTID,E.EMPNAME"
            strSql += vbCrLf + " ,A.TRANTYPE TYPE,TRANDATE,SNO,A.PCS AS OPCS,A.GRSWT OGRSWT,NETWT ONETWT"
            strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),ROUND((A.TAGNETWT-A.NETWT )*(ISNULL(A.PURITY,100)/100),3)) OPUREWT"
            '
            strSql += vbCrLf + " ,(SELECT TOP 1 DESIGNERID FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO=A.TAGNO) AS DESIGNERID"
            strSql += vbCrLf + " ,(SELECT TOP 1 TABLECODE FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO=A.TAGNO) AS TABLECODE"
            '
            strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),0) AS STNWT"
            strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),0) AS DIAWT,'" & cnStockDb.ToString & "' TDBNAME "
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE A"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON A.CATCODE = C.CATCODE"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON A.ITEMID= I.ITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..EMPMASTER E ON A.EMPID= E.EMPID "
            strSql += vbCrLf + " WHERE A.TRANTYPE = 'SA' AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            If cmbOpenMetal.Text <> "ALL" Then strSql += vbCrLf + " AND C.METALID = ISNULL((SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME= '" & cmbOpenMetal.Text & "'),'')"
            If CmbCategory.Text <> "ALL" And CmbCategory.Text <> "" Then strSql += vbCrLf + " AND C.CATNAME= '" & CmbCategory.Text & "'"
            If cmbCompany_MAN.Text <> "ALL" And cmbCompany_MAN.Text <> "" Then strSql += vbCrLf + " AND A.COMPANYID = ISNULL((SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME= '" & cmbCompany_MAN.Text & "'),'')"
            strSql += vbCrLf + " AND (TAGPCS <> PCS OR TAGGRSWT <> A.GRSWT OR TAGNETWT <> A.NETWT)"
            strSql += vbCrLf + " AND (TAGPCS <> 0 OR TAGGRSWT <> 0 OR TAGNETWT <> 0)"
            If cmbCostCentre_MAN.Enabled Then
                If UCase(cmbCostCentre_MAN.Text) <> "ALL" And cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND A.COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "')"
            End If
            strSql += vbCrLf + " AND ISNULL(BAGNO,'') =''"
            strSql += vbCrLf + " AND ISNULL(MELT_RETAG,'') =''"
            If MetalBasedStone Then '' added for vbj on 04-06-2022 as per magesh sir advice
                strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & cnStockDb & "..ISSMETAL WHERE ISSSNO =A.SNO) "
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT C.METALID,'PARTLY SALES' AS TYPE"
                strSql += vbCrLf + " ,CONVERT(VARCHAR,A.TRANDATE,103)AS DATE,B.CATCODE,C.CATNAME,A.TRANNO,I.ITEMNAME"
                strSql += vbCrLf + " ,0 PCS"
                strSql += vbCrLf + " ,(SELECT GRSWT FROM " & cnAdminDb & "..ITEMTAGMETAL  WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID=A.ITEMID And TAGNO=A.TAGNO) "
                strSql += vbCrLf + " And CATCODE =B.CATCODE)-B.GRSWT GRSWT"
                strSql += vbCrLf + " , 0 AS DUSTWT"
                strSql += vbCrLf + " ,(SELECT CASE WHEN ISNULL(NETWT,0)<>0 THEN NETWT ELSE GRSWT END FROM " & cnAdminDb & "..ITEMTAGMETAL  WHERE TAGSNO IN  "
                strSql += vbCrLf + " (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID=A.ITEMID And TAGNO=A.TAGNO) And CATCODE =B.CATCODE)"
                strSql += vbCrLf + " - (CASE WHEN ISNULL(B.NETWT,0)<>0 THEN B.NETWT ELSE B.GRSWT END ) NETWT "
                strSql += vbCrLf + " ,ISNULL(A.PURITY,100) TOUCH "
                strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),0) PUREWT"
                strSql += vbCrLf + " ,(CASE WHEN ISNULL(B.NETWT,0)<>0 THEN B.NETWT ELSE B.GRSWT END ) CNETWT"
                strSql += vbCrLf + " ,CONVERT(DECIMAL(15,2),B.RATE)RATE,B.AMOUNT,A.BATCHNO,A.COSTID,E.EMPNAME"
                strSql += vbCrLf + " ,A.TRANTYPE TYPE,A.TRANDATE,A.SNO,0 AS OPCS,B.GRSWT OGRSWT,(CASE WHEN ISNULL(B.NETWT,0)<>0 THEN B.NETWT ELSE B.GRSWT END ) ONETWT"
                strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),0) OPUREWT"
                '
                strSql += vbCrLf + " ,(SELECT TOP 1 DESIGNERID FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO=A.TAGNO) AS DESIGNERID"
                strSql += vbCrLf + " ,(SELECT TOP 1 TABLECODE FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO=A.TAGNO) AS TABLECODE"
                '
                strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),0) AS STNWT"
                strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),0) AS DIAWT,'" & cnStockDb.ToString & "' TDBNAME "
                strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE A"
                strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..ISSMETAL B ON B.ISSSNO =A.SNO"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON B.CATCODE = C.CATCODE"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON A.ITEMID= I.ITEMID"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..EMPMASTER E ON A.EMPID= E.EMPID "
                strSql += vbCrLf + " WHERE A.TRANTYPE = 'SA' AND ISNULL(A.CANCEL,'') = ''"
                strSql += vbCrLf + " AND A.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
                If cmbOpenMetal.Text <> "ALL" Then strSql += vbCrLf + " AND C.METALID = ISNULL((SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME= '" & cmbOpenMetal.Text & "'),'')"
                If CmbCategory.Text <> "ALL" And CmbCategory.Text <> "" Then strSql += vbCrLf + " AND C.CATNAME= '" & CmbCategory.Text & "'"
                If cmbCompany_MAN.Text <> "ALL" And cmbCompany_MAN.Text <> "" Then strSql += vbCrLf + " AND A.COMPANYID = ISNULL((SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME= '" & cmbCompany_MAN.Text & "'),'')"
                strSql += vbCrLf + " AND (TAGPCS <> PCS OR TAGGRSWT <> A.GRSWT OR TAGNETWT <> A.NETWT)"
                strSql += vbCrLf + " AND (TAGPCS <> 0 OR TAGGRSWT <> 0 OR TAGNETWT <> 0)"
                strSql += vbCrLf + " AND (((SELECT GRSWT FROM " & cnAdminDb & "..ITEMTAGMETAL  WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID=A.ITEMID And TAGNO=A.TAGNO) "
                strSql += vbCrLf + " And CATCODE =B.CATCODE)-B.GRSWT) <> 0"
                strSql += vbCrLf + " OR ((SELECT CASE WHEN ISNULL(NETWT,0)<>0 THEN NETWT ELSE GRSWT END FROM " & cnAdminDb & "..ITEMTAGMETAL  WHERE TAGSNO IN  "
                strSql += vbCrLf + " (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID=A.ITEMID And TAGNO=A.TAGNO) And CATCODE =B.CATCODE)"
                strSql += vbCrLf + " - (CASE WHEN ISNULL(B.NETWT,0)<>0 THEN B.NETWT ELSE B.GRSWT END )) <> 0) "
                If cmbCostCentre_MAN.Enabled Then
                    If UCase(cmbCostCentre_MAN.Text) <> "ALL" And cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND A.COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "')"
                End If
                strSql += vbCrLf + " AND ISNULL(BAGNO,'') =''"
                strSql += vbCrLf + " AND ISNULL(MELT_RETAG,'') =''"
                strSql += vbCrLf + " AND EXISTS (SELECT 1 FROM " & cnStockDb & "..ISSMETAL WHERE ISSSNO =A.SNO) "
            End If
            If PREYEAR_RECORD And pretrandb <> "" Then
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT C.METALID,'PARTLY SALES' AS TYPE"
                strSql += vbCrLf + " ,CONVERT(VARCHAR,TRANDATE,103)AS DATE,A.CATCODE,C.CATNAME,TRANNO,I.ITEMNAME"
                strSql += vbCrLf + " ,TAGPCS-A.PCS PCS, TAGGRSWT-A.GRSWT GRSWT, 0 AS DUSTWT,TAGNETWT-NETWT NETWT,ISNULL(A.PURITY,100) TOUCH "
                strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),ROUND((A.NETWT )*(ISNULL(A.PURITY,100)/100),3)) PUREWT,NETWT CNETWT"
                strSql += vbCrLf + " ,CONVERT(DECIMAL(15,2),RATE)RATE,AMOUNT,BATCHNO,A.COSTID,E.EMPNAME"
                strSql += vbCrLf + " ,A.TRANTYPE TYPE,TRANDATE,SNO,A.PCS AS OPCS,A.GRSWT OGRSWT,NETWT ONETWT"
                strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),ROUND((A.TAGNETWT-A.NETWT )*(ISNULL(A.PURITY,100)/100),3)) OPUREWT"
                '
                strSql += vbCrLf + " ,(SELECT TOP 1 DESIGNERID FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO=A.TAGNO) AS DESIGNERID"
                strSql += vbCrLf + " ,(SELECT TOP 1 TABLECODE FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO=A.TAGNO) AS TABLECODE"
                '
                strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),0) AS STNWT"
                strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),0) AS DIAWT,'" & pretrandb.ToString & "' TDBNAME "
                strSql += vbCrLf + " FROM " & pretrandb & "..ISSUE A"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON A.CATCODE = C.CATCODE"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON A.ITEMID= I.ITEMID"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..EMPMASTER E ON A.EMPID= E.EMPID "
                strSql += vbCrLf + " WHERE A.TRANTYPE = 'SA' AND ISNULL(CANCEL,'') = ''"
                strSql += vbCrLf + " AND TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
                If cmbOpenMetal.Text <> "ALL" Then strSql += vbCrLf + " AND C.METALID = ISNULL((SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME= '" & cmbOpenMetal.Text & "'),'')"
                If CmbCategory.Text <> "ALL" And CmbCategory.Text <> "" Then strSql += vbCrLf + " AND C.CATNAME= '" & CmbCategory.Text & "'"
                If cmbCompany_MAN.Text <> "ALL" And cmbCompany_MAN.Text <> "" Then strSql += vbCrLf + " AND A.COMPANYID = ISNULL((SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME= '" & cmbCompany_MAN.Text & "'),'')"
                strSql += vbCrLf + " AND (TAGPCS <> PCS OR TAGGRSWT <> A.GRSWT OR TAGNETWT <> A.NETWT)"
                strSql += vbCrLf + " AND (TAGPCS <> 0 OR TAGGRSWT <> 0 OR TAGNETWT <> 0)"
                If cmbCostCentre_MAN.Enabled Then
                    If UCase(cmbCostCentre_MAN.Text) <> "ALL" And cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND A.COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "')"
                End If
                strSql += vbCrLf + " AND ISNULL(BAGNO,'') =''"
                strSql += vbCrLf + " AND ISNULL(MELT_RETAG,'') =''"
                If MetalBasedStone Then '' added for vbj on 04-06-2022 as per magesh sir advice
                    strSql += vbCrLf + " AND  NOT EXISTS (SELECT 1 FROM " & pretrandb & "..ISSMETAL WHERE ISSSNO =A.SNO) "
                    strSql += vbCrLf + " UNION ALL"
                    strSql += vbCrLf + " SELECT C.METALID,'PARTLY SALES' AS TYPE"
                    strSql += vbCrLf + " ,CONVERT(VARCHAR,A.TRANDATE,103)AS DATE,B.CATCODE,C.CATNAME,A.TRANNO,I.ITEMNAME"
                    strSql += vbCrLf + " ,0 PCS"
                    strSql += vbCrLf + " ,(SELECT GRSWT FROM " & cnAdminDb & "..ITEMTAGMETAL  WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID=A.ITEMID And TAGNO=A.TAGNO) "
                    strSql += vbCrLf + " And CATCODE =B.CATCODE)-B.GRSWT GRSWT"
                    strSql += vbCrLf + " , 0 AS DUSTWT"
                    strSql += vbCrLf + " ,(SELECT CASE WHEN ISNULL(NETWT,0)<>0 THEN NETWT ELSE GRSWT END FROM " & cnAdminDb & "..ITEMTAGMETAL  WHERE TAGSNO IN  "
                    strSql += vbCrLf + " (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID=A.ITEMID And TAGNO=A.TAGNO) And CATCODE =B.CATCODE)"
                    strSql += vbCrLf + " - (CASE WHEN ISNULL(B.NETWT,0)<>0 THEN B.NETWT ELSE B.GRSWT END ) NETWT "
                    strSql += vbCrLf + " ,ISNULL(A.PURITY,100) TOUCH "
                    strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),0) PUREWT"
                    strSql += vbCrLf + " ,(CASE WHEN ISNULL(B.NETWT,0)<>0 THEN B.NETWT ELSE B.GRSWT END ) CNETWT"
                    strSql += vbCrLf + " ,CONVERT(DECIMAL(15,2),B.RATE)RATE,B.AMOUNT,A.BATCHNO,A.COSTID,E.EMPNAME"
                    strSql += vbCrLf + " ,A.TRANTYPE TYPE,A.TRANDATE,A.SNO,0 AS OPCS,B.GRSWT OGRSWT,(CASE WHEN ISNULL(B.NETWT,0)<>0 THEN B.NETWT ELSE B.GRSWT END ) ONETWT"
                    strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),0) OPUREWT"
                    '
                    strSql += vbCrLf + " ,(SELECT TOP 1 DESIGNERID FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO=A.TAGNO) AS DESIGNERID"
                    strSql += vbCrLf + " ,(SELECT TOP 1 TABLECODE FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO=A.TAGNO) AS TABLECODE"
                    '
                    strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),0) AS STNWT"
                    strSql += vbCrLf + " ,CONVERT(DECIMAL(15,3),0) AS DIAWT,'" & pretrandb.ToString & "' TDBNAME "
                    strSql += vbCrLf + " FROM " & pretrandb & "..ISSUE A"
                    strSql += vbCrLf + " INNER JOIN " & pretrandb & "..ISSMETAL B ON B.ISSSNO =A.SNO"
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON B.CATCODE = C.CATCODE"
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON A.ITEMID= I.ITEMID"
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..EMPMASTER E ON A.EMPID= E.EMPID "
                    strSql += vbCrLf + " WHERE A.TRANTYPE = 'SA' AND ISNULL(A.CANCEL,'') = ''"
                    strSql += vbCrLf + " AND A.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
                    If cmbOpenMetal.Text <> "ALL" Then strSql += vbCrLf + " AND C.METALID = ISNULL((SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME= '" & cmbOpenMetal.Text & "'),'')"
                    If CmbCategory.Text <> "ALL" And CmbCategory.Text <> "" Then strSql += vbCrLf + " AND C.CATNAME= '" & CmbCategory.Text & "'"
                    If cmbCompany_MAN.Text <> "ALL" And cmbCompany_MAN.Text <> "" Then strSql += vbCrLf + " AND A.COMPANYID = ISNULL((SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME= '" & cmbCompany_MAN.Text & "'),'')"
                    strSql += vbCrLf + " AND (TAGPCS <> PCS OR TAGGRSWT <> A.GRSWT OR TAGNETWT <> A.NETWT)"
                    strSql += vbCrLf + " AND (TAGPCS <> 0 OR TAGGRSWT <> 0 OR TAGNETWT <> 0)"
                    strSql += vbCrLf + " AND (((SELECT GRSWT FROM " & cnAdminDb & "..ITEMTAGMETAL  WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID=A.ITEMID And TAGNO=A.TAGNO) "
                    strSql += vbCrLf + " And CATCODE =B.CATCODE)-B.GRSWT) <> 0"
                    strSql += vbCrLf + " OR ((SELECT CASE WHEN ISNULL(NETWT,0)<>0 THEN NETWT ELSE GRSWT END FROM " & cnAdminDb & "..ITEMTAGMETAL  WHERE TAGSNO IN  "
                    strSql += vbCrLf + " (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID=A.ITEMID And TAGNO=A.TAGNO) And CATCODE =B.CATCODE)"
                    strSql += vbCrLf + " - (CASE WHEN ISNULL(B.NETWT,0)<>0 THEN B.NETWT ELSE B.GRSWT END )) <> 0) "
                    If cmbCostCentre_MAN.Enabled Then
                        If UCase(cmbCostCentre_MAN.Text) <> "ALL" And cmbCostCentre_MAN.Text <> "" Then strSql += vbCrLf + " AND A.COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "')"
                    End If
                    strSql += vbCrLf + " AND ISNULL(BAGNO,'') =''"
                    strSql += vbCrLf + " AND ISNULL(MELT_RETAG,'') =''"
                    strSql += vbCrLf + " AND EXISTS (SELECT 1 FROM " & pretrandb & "..ISSMETAL WHERE ISSSNO =A.SNO) "
                End If
            End If
        End If
        If chkPosting.Checked Then
            strSql = vbCrLf + "  SELECT /*A.TRANTYPE,TRANDATE,TRANNO,*/A.CATCODE,A.METALID,C.CATNAME,"
            strSql += vbCrLf + "  SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(PUREWT)PUREWT"
            strSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT AS A"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON A.CATCODE = C.CATCODE"
            strSql += vbCrLf + "  WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND A.TRANTYPE='PU' AND ISNULL(CANCEL,'')=''"
            strSql += vbCrLf + "  AND ISNULL(MELT_RETAG,'')<>'P' AND COMPANYID IN (SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COMPANYID')"
            If cmbOpenMetal.Text <> "ALL" Then strSql += vbCrLf + " AND C.METALID = ISNULL((SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME= '" & cmbOpenMetal.Text & "'),'')"
            If CmbCategory.Text <> "ALL" And CmbCategory.Text <> "" Then strSql += vbCrLf + " AND C.CATNAME= '" & CmbCategory.Text & "'"
            If cmbCompany_MAN.Text <> "ALL" And cmbCompany_MAN.Text <> "" Then strSql += vbCrLf + " AND A.COMPANYID = ISNULL((SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME= '" & cmbCompany_MAN.Text & "'),'')"
            strSql += vbCrLf + "  GROUP BY /*A.TRANTYPE,TRANDATE,TRANNO,*/ C.CATNAME,A.CATCODE,A.METALID"
        End If
        'strSql += vbcrlf + " UNION ALL"
        'strSql += vbcrlf + " SELECT 'PARTLY SALES' AS TYPE"
        'strSql += vbcrlf + " "
        'strSql += vbcrlf + " SELECT 'PARTLY SALES' AS TYPE"
        'strSql += vbcrlf + " ,CONVERT(VARCHAR,TRANDATE,103)AS DATE,A.CATCODE,C.CATNAME,TRANNO,I.ITEMNAME"
        'strSql += vbcrlf + " ,A.PCS,A.GRSWT,NETWT,AMOUNT,BATCHNO,COSTID"
        'strSql += vbcrlf + " ,A.TRANTYPE TYPE,TRANDATE"
        'strSql += vbcrlf + " FROM " & cnStockDb & "..ISSSTONE I"
        'strSql += vbcrlf + " WHERE TRANDATE BETWEEN  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        'strSql += vbcrlf + " AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE BETWEEN  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' AND TRANTYPE = 'SA')"
        'strSql += vbcrlf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')"
        'strSql += vbcrlf + " AND (TAGSTNPCS <> STNPCS OR TAGSTNWT <> STnWT)"
        'strSql += vbcrlf + " AND (TAGSTNPCS <> 0 OR TAGSTNWT <> 0)"
        'strSql += vbcrlf + " AND (TAGSTNPCS - STNPCS <> 0 OR TAGSTNWT - STNWT <> 0)"
        'strSql += vbcrlf + " AND COSTID = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'),'')"
        'strSql += vbcrlf + " AND COMPANYID = '" & strCompanyId & "'"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()

        If Not chkPosting.Checked Then strSql += vbCrLf + " ORDER BY A.TRANTYPE,TRANDATE,TRANNO"
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtTemp As New DataTable
        da.Fill(dtTemp)
        If PSSR_CATPOST <> "" And Not PSSR_CATPOSTING Is Nothing Then
            Dim dtCol1 As New DataColumn("PSSRCHECK", GetType(String))
            dtTemp.Columns.Add(dtCol1)
        End If
        If chkAll.Checked Then
            For cnt As Integer = 0 To dtTemp.Rows.Count - 1
                dtGridView.ImportRow(dtTemp.Rows(cnt))
                gridView_OWN.Rows(cnt).Cells("CHECK").Value = chkAll.Checked
                If chkToType.Checked Then
                    ''gridView.Rows(cnt).Cells("TRANSFERTYPE").Value = "TO LOT"
                    gridView_OWN.Rows(cnt).Cells("TRANSFERTYPE").Value = "TO " & cmbEntryType_MAN.Text
                Else
                    gridView_OWN.Rows(cnt).Cells("TRANSFERTYPE").Value = "TO MELTING"
                End If
                If PENDTRF_LOAD_ACNAME And cmbEntryType_MAN.Text = "DEALER" Then
                    If CmbAcname_MAN.Text <> "ALL" And CmbAcname_MAN.Text <> "" Then
                        If Val(objGPack.GetSqlValue("SELECT COUNT(*)CNT FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME ='" & CmbAcname_MAN.Text & "'", "CNT", 0).ToString) <> 0 Then
                            gridView_OWN.Rows(cnt).Cells("SMITH").Value = CmbAcname_MAN.Text
                        End If
                    End If
                End If
            Next
        Else
            For cnt As Integer = 0 To dtTemp.Rows.Count - 1
                If chkToType.Checked Then
                    ''gridView.Rows(cnt).Cells("TRANSFERTYPE").Value = "TO LOT"
                    gridView_OWN.Rows(cnt).Cells("TRANSFERTYPE").Value = "TO " & cmbEntryType_MAN.Text
                Else
                    gridView_OWN.Rows(cnt).Cells("TRANSFERTYPE").Value = "TO MELTING"
                End If
                If PENDTRF_LOAD_ACNAME And cmbEntryType_MAN.Text = "DEALER" Then
                    If CmbAcname_MAN.Text <> "ALL" And CmbAcname_MAN.Text <> "" Then
                        If Val(objGPack.GetSqlValue("SELECT COUNT(*)CNT FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME ='" & CmbAcname_MAN.Text & "'", "CNT", 0).ToString) <> 0 Then
                            gridView_OWN.Rows(cnt).Cells("SMITH").Value = CmbAcname_MAN.Text
                        End If
                    End If
                End If
            Next
        End If
        If PSSR_CATPOST <> "" And Not PSSR_CATPOSTING Is Nothing Then
            For Each drr As DataGridViewRow In gridView_OWN.Rows
                If drr.Cells("TYPE").Value.ToString = "PARTLY SALES" Or drr.Cells("TYPE").Value.ToString = "SALES RETURN" Then
                    For cnt As Integer = 0 To PSSR_CATPOSTING.Length - 1
                        If PSSR_CATPOSTING(cnt).ToString.Contains(drr.Cells("CATCODE").Value.ToString & ":") Then
                            drr.Cells("PSSRCHECK").Value = ""
                            drr.Cells(0).Value = IIf(chkAll.Checked, True, False)
                            Exit For
                        Else
                            drr.Cells("PSSRCHECK").Value = "Catcode not found in softcontrol PSSR_CATEGORYPOST"
                            drr.Cells(0).Value = False
                        End If
                    Next
                    ''If drr.Cells("STNCATCODE").Value.ToString <> "" Then
                    ''    For cnt As Integer = 0 To PSSR_CATPOSTING.Length - 1
                    ''        If PSSR_CATPOSTING(cnt).ToString.Contains(drr.Cells("STNCATCODE").Value.ToString & ":") Then
                    ''            drr.Cells("PSSRCHECK").Value = ""
                    ''            drr.Cells("CHECK").Value = IIf(chkAll.Checked, True, False)
                    ''            Exit For
                    ''        Else
                    ''            drr.Cells("PSSRCHECK").Value += "Stone Catcode not found in softcontrol PSSR_CATEGORYPOST"
                    ''            drr.Cells("CHECK").Value = False
                    ''        End If
                    ''    Next
                    ''End If
                End If
            Next
        End If
        If dtGridView.Rows.Count > 0 Then
            CalcGridViewTotal()
            If gridView_OWN.Columns.Contains("TDBNAME") Then gridView_OWN.Columns("TDBNAME").Visible = False
            If gridView_OWN.Columns.Contains("PSSRCHECK") And PSSR_CATPOST <> "" Then gridView_OWN.Columns("PSSRCHECK").Visible = False
            gridView_OWN.Focus()
            gridView_OWN.Columns(0).Selected = True
            gridView_OWN.Columns(1).Frozen = True
        Else
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            If cmbCostCentre_MAN.Enabled = True Then
                cmbCostCentre_MAN.Focus()
            Else
                chkPurchase.Focus()
            End If
        End If
        btnLoad.Enabled = True
    End Sub
    Private Sub gridView_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView_OWN.CellEnter
        Select Case gridView_OWN.Columns(e.ColumnIndex).Name
            Case "TRANSFERTYPE"
                Loadcombo(1)
                gridView_OWN.CurrentCell = gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("TRANSFERTYPE")
                Dim pt As Point = gridView_OWN.Location
                cmbTransferTo.Visible = True
                pt = pt + gridView_OWN.GetCellDisplayRectangle(gridView_OWN.Columns("TRANSFERTYPE").Index, gridView_OWN.CurrentRow.Index, False).Location
                cmbTransferTo.Text = gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("TRANSFERTYPE").Value.ToString
                cmbTransferTo.Location = pt
                cmbTransferTo.Width = gridView_OWN.Columns("TRANSFERTYPE").Width
                'cmbTransferTo.SelectedIndex = 0
                cmbTransferTo.Focus()
            Case "ITEMNAME"
                If gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("TRANSFERTYPE").Value.ToString.ToUpper = "TO LOT" Or gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("TRANSFERTYPE").Value.ToString.ToUpper = "TO NONTAG" Then
                    Loadcombo(4, gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("METALID").Value.ToString)
                    gridView_OWN.CurrentCell = gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("ITEMNAME")
                    Dim pt As Point = gridView_OWN.Location
                    cmbTransferTo.Visible = True
                    pt = pt + gridView_OWN.GetCellDisplayRectangle(gridView_OWN.Columns("ITEMNAME").Index, gridView_OWN.CurrentRow.Index, False).Location
                    cmbTransferTo.Text = gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("ITEMNAME").Value.ToString
                    cmbTransferTo.Location = pt
                    cmbTransferTo.Width = gridView_OWN.Columns("ITEMNAME").Width
                    'cmbTransferTo.SelectedIndex = 0
                    cmbTransferTo.Focus()
                End If
                'Case "TOUCH"
                '    If gridView.Rows(gridView.CurrentRow.Index).Cells("TRANSFERTYPE").Value.ToString.ToUpper = "TO DEALER" Then
                '        Loadcombo(5, gridView.Rows(gridView.CurrentRow.Index).Cells("METALID").Value.ToString)
                '        gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells("TOUCH")
                '        Dim pt As Point = gridView.Location
                '        cmbTransferTo.Visible = True
                '        pt = pt + gridView.GetCellDisplayRectangle(gridView.Columns("TOUCH").Index, gridView.CurrentRow.Index, False).Location
                '        cmbTransferTo.Text = gridView.Rows(gridView.CurrentRow.Index).Cells("TOUCH").Value.ToString
                '        cmbTransferTo.Location = pt
                '        cmbTransferTo.Width = gridView.Columns("TOUCH").Width
                '        'cmbTransferTo.SelectedIndex = 0
                '        cmbTransferTo.Focus()
                '    End If
            Case "SMITH"
                If gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("TRANSFERTYPE").Value.ToString.ToUpper = "TO DEALER" Then
                    Loadcombo(2)
                    gridView_OWN.CurrentCell = gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("SMITH")
                    Dim pt As Point = gridView_OWN.Location
                    cmbTransferTo.Visible = True
                    pt = pt + gridView_OWN.GetCellDisplayRectangle(gridView_OWN.Columns("SMITH").Index, gridView_OWN.CurrentRow.Index, False).Location
                    cmbTransferTo.Location = pt
                    cmbTransferTo.Width = gridView_OWN.Columns("SMITH").Width
                    cmbTransferTo.SelectedIndex = 0
                    cmbTransferTo.Focus()
                End If
            Case "PCS"
                'If gridView.Rows(gridView.CurrentRow.Index).Cells("TYPE").Value.ToString <> "PURCHASE" Then MsgBox("Cannot edit for " & gridView.Rows(gridView.CurrentRow.Index).Cells("TYPE").Value.ToString & " Item.", MsgBoxStyle.Information) : Exit Sub
                gridView_OWN.CurrentCell = gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("PCS")
                Dim pt As Point = gridView_OWN.Location
                txtGrid.Visible = True
                pt = pt + gridView_OWN.GetCellDisplayRectangle(gridView_OWN.Columns("PCS").Index, gridView_OWN.CurrentRow.Index, False).Location
                txtGrid.Text = gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("PCS").Value.ToString
                txtGrid.Location = pt
                txtGrid.Width = gridView_OWN.Columns("PCS").Width
                'txtGrid.Focus()
                cmbTransferTo.Focus()
            Case "GRSWT"
                If gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("TYPE").Value.ToString <> "PURCHASE" Then MsgBox("Cannot edit for " & gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("TYPE").Value.ToString & " Item.", MsgBoxStyle.Information) : Exit Sub
                gridView_OWN.CurrentCell = gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("GRSWT")
                Dim pt As Point = gridView_OWN.Location
                txtGrid.Visible = True
                pt = pt + gridView_OWN.GetCellDisplayRectangle(gridView_OWN.Columns("GRSWT").Index, gridView_OWN.CurrentRow.Index, False).Location
                txtGrid.Text = gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("GRSWT").Value.ToString
                txtGrid.Location = pt
                txtGrid.Width = gridView_OWN.Columns("GRSWT").Width
                txtGrid.Focus()
            Case "CNETWT"
                If gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("TYPE").Value.ToString <> "PURCHASE" Then MsgBox("Cannot edit for " & gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("TYPE").Value.ToString & " Item.", MsgBoxStyle.Information) : Exit Sub
                gridView_OWN.CurrentCell = gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("CNETWT")
                Dim pt As Point = gridView_OWN.Location
                txtGrid.Visible = True
                pt = pt + gridView_OWN.GetCellDisplayRectangle(gridView_OWN.Columns("CNETWT").Index, gridView_OWN.CurrentRow.Index, False).Location
                txtGrid.Text = gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("CNETWT").Value.ToString
                txtGrid.Location = pt
                txtGrid.Width = gridView_OWN.Columns("CNETWT").Width
                txtGrid.Focus()
            Case "PUREWT"
                If gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("TYPE").Value.ToString <> "PURCHASE" Then MsgBox("Cannot edit for " & gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("TYPE").Value.ToString & " Item.", MsgBoxStyle.Information) : Exit Sub
                gridView_OWN.CurrentCell = gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("PUREWT")
                Dim pt As Point = gridView_OWN.Location
                txtGrid.Visible = True
                pt = pt + gridView_OWN.GetCellDisplayRectangle(gridView_OWN.Columns("PUREWT").Index, gridView_OWN.CurrentRow.Index, False).Location
                txtGrid.Text = gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("PUREWT").Value.ToString
                txtGrid.Location = pt
                txtGrid.Width = gridView_OWN.Columns("PUREWT").Width
                txtGrid.Focus()
            Case "COUNTER"
                Loadcombo(3)
                gridView_OWN.CurrentCell = gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("COUNTER")
                Dim pt As Point = gridView_OWN.Location
                cmbTransferTo.Visible = True
                pt = pt + gridView_OWN.GetCellDisplayRectangle(gridView_OWN.Columns("COUNTER").Index, gridView_OWN.CurrentRow.Index, False).Location
                cmbTransferTo.Location = pt
                cmbTransferTo.Width = gridView_OWN.Columns("COUNTER").Width
                cmbTransferTo.SelectedIndex = 0
                cmbTransferTo.Focus()
        End Select
    End Sub
    Private Sub gridView_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView_OWN.CellValueChanged
        If gridView_OWN.Columns(e.ColumnIndex).Name.ToUpper = "CHECK" Then Call CalcGridViewTotal()
    End Sub
    Private Sub gridView_OWN_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles gridView_OWN.CellFormatting
        'If gridView_OWN.Rows.Count > 0 Then
        '    If gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name = "CHECK" Then
        '        For i As Integer = 0 To gridView_OWN.Rows.Count - 1
        '            With gridView_OWN.Rows(i)
        '                If .Cells("COLOR").Value.ToString = "Y" Then
        '                    .DefaultCellStyle.BackColor = Color.LightPink
        '                    Exit For
        '                End If
        '                If .Cells("COLOR").Value.ToString = "N" Then
        '                    .DefaultCellStyle.BackColor = Color.Transparent
        '                End If
        '            End With
        '        Next
        '    End If
        'End If
    End Sub
    Private Sub gridView_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles gridView_OWN.CellContentClick
        'If gridView_OWN.Rows.Count > 0 Then
        '    If gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name = "CHECK" Then
        '        Dim currentValue1 As String = gridView_OWN.CurrentRow.Cells("TRANNO").Value.ToString
        '        Dim currentValue2 As String = gridView_OWN.CurrentRow.Cells("CATNAME").Value.ToString
        '        Dim currentValue3 As String = gridView_OWN.CurrentRow.Cells("SNO").Value.ToString
        '        For i As Integer = 0 To gridView_OWN.Rows.Count - 1
        '            With gridView_OWN.Rows(i)
        '                If .Cells("TRANNO").Value.ToString = currentValue1 _
        '                    And .Cells("CATNAME").Value.ToString = currentValue2 _
        '                    And .Cells("SNO").Value.ToString = currentValue3 Then
        '                    If .Cells("COLOR").Value.ToString = "Y" Then
        '                        .Cells("COLOR").Value = "N"
        '                    Else
        '                        .Cells("COLOR").Value = "Y"
        '                    End If
        '                End If
        '            End With
        '        Next
        '    End If
        'End If
    End Sub
    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView_OWN.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name = "CHECK" Then
                gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("TRANSFERTYPE").Selected = True
            ElseIf gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name = "PCS" Then
                gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("PCS").Selected = True
                'ElseIf gridView.Columns(gridView.CurrentCell.ColumnIndex).Name = "GRSWT" Then
                '    gridView.Rows(gridView.CurrentRow.Index).Cells("NETWT").Selected = True
                'ElseIf gridView.Columns(gridView.CurrentCell.ColumnIndex).Name = "NETWT" Then
                '    gridView.Rows(gridView.CurrentRow.Index + 1).Cells("CHECK").Selected = True
            End If
        ElseIf e.KeyCode = Keys.P Then
            If gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("TYPE").Value.ToString <> "PURCHASE" Then MsgBox("Cannot edit for " & gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("TYPE").Value.ToString & " Item.", MsgBoxStyle.Information) : Exit Sub
            gridView_OWN.Columns("PCS").ReadOnly = False
            gridView_OWN.Columns("GRSWT").ReadOnly = False
            gridView_OWN.Columns("DUSTWT").ReadOnly = False
            gridView_OWN.Columns("NETWT").ReadOnly = False
            gridView_OWN.Columns("CNETWT").ReadOnly = False
            gridView_OWN.Columns("PUREWT").ReadOnly = False
            gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("PCS").Selected = True
        ElseIf e.KeyCode = Keys.Escape Then
            btnGenerate.Focus()   'cmbTransferTo.Focus()
        End If
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub
    Private Sub btnGenerate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        Try
            strSql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COMPANYID'"
            Dim str2CompId As String = objGPack.GetSqlValue(strSql, , , tran)
            If str2CompId = strCompanyId And chkPosting.Checked Then chkPosting.Checked = False : MsgBox("Posting not Available for this Company...") : Exit Sub

            Dtitemnontag.Rows.Clear()
            Dim instkrow As DataRow = Nothing
            Dim dt As New DataTable
            dt = CType(gridView_OWN.DataSource, DataTable).Copy
            Dim ros() As DataRow = Nothing
            ros = dt.Select("CHECK = TRUE")
            If Not ros.Length > 0 Then
                MsgBox("There is No Checked Record", MsgBoxStyle.Information)
                Exit Sub
            End If
            Dim lotSno As String
            Dim LotNo As Integer

            funLoadcatstk()

            tran = Nothing
            tran = cn.BeginTransaction


            If chkPosting.Checked Then
                If Not gridView_OWN.Rows.Count > 0 Then
                    MsgBox("There is No Checked Record", MsgBoxStyle.Information)
                    Exit Sub
                End If
                InsertReceipt_Post()
                tran.Commit()
                tran = Nothing
                MsgBox("Generated..")
                chkPosting.Checked = False
                btnNew_Click(Me, New EventArgs)
                Exit Sub
            End If

            Dim BagNo As String = Nothing
            If Val(gridViewtotal.Rows(1).Cells("PCS").Value.ToString) <> 0 Or Val(gridViewtotal.Rows(1).Cells("GRSWT").Value.ToString) <> 0 Or Val(gridViewtotal.Rows(3).Cells("PCS").Value.ToString) <> 0 Or Val(gridViewtotal.Rows(3).Cells("GRSWT").Value.ToString) <> 0 Then
GENBAGNO:
                BagNo = cnCostId & "B" & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString & GetTranDbSoftControlValue("BAGNO", True, tran)

                'BagNo = cnCostId & 
                ''check
                strSql = " SELECT 'CHECK' FROM " & cnStockDb & "..RECEIPT"
                strSql += vbCrLf + "  WHERE BAGNO = '" & BagNo & "'"
                strSql += vbCrLf + "UNION ALL SELECT 'CHECK' FROM " & cnStockDb & "..ISSUE"
                strSql += vbCrLf + "  WHERE BAGNO = '" & BagNo & "'"
                If objGPack.GetSqlValue(strSql, , , tran).Length > 0 Then
                    GoTo GENBAGNO
                End If
            End If
            If Val(gridViewtotal.Rows(2).Cells("PCS").Value.ToString) <> 0 Or Val(gridViewtotal.Rows(2).Cells("GRSWT").Value.ToString) <> 0 Then
GENLOTNO:
                strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'LOTNO'"
                LotNo = Val(objGPack.GetSqlValue(strSql, , , tran))
                strSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT = '" & LotNo + 1 & "' "
                strSql += vbCrLf + " WHERE CTLID = 'LOTNO' AND CTLTEXT = " & LotNo & ""
                cmd = New OleDbCommand(strSql, cn, tran)
                If cmd.ExecuteNonQuery() = 0 Then
                    GoTo GENLOTNO
                End If
                LotNo += 1
            End If

            Dim batchno As String = ""
            Dim Sno As String = ""
            Dim TDbame As String = ""
            Dim entord As Integer = 0
            Dim KEY As String = ""
            'dtCatstk.Rows.Clear()


            Dim billcontrolid As String = "GEN-SM-INTISS"
            strSql = "SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL"
            strSql += " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
            If UCase(objGPack.GetSqlValue(strSql, , , tran)) <> "Y" Then
                billcontrolid = "GEN-STKREFNO"
            End If
            Dim NEWBILLNO As Integer
            If Not PSSR_CATPOSTING Is Nothing Then
                strSql = "SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = '" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
                NEWBILLNO = Val(objGPack.GetSqlValue(strSql, , , tran)) + 1
GenerateNewBillNo:
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
            End If


            For Each row As DataRow In ros
                TDbame = IIf(row.Item("TDBNAME").ToString <> "", row.Item("TDBNAME").ToString, cnStockDb.ToString)
                If row.Item("TRANSFERTYPE").ToString.ToUpper.Trim = "TO MELTING" Or row.Item("TRANSFERTYPE").ToString.ToUpper.Trim = "TO DEALER" Then
                    batchno = "'" & row.Item("BATCHNO").ToString & "'"
                    Sno = "'" & row.Item("SNO").ToString & "'"
                    strSql = " UPDATE " & TDbame & "..RECEIPT"
                    strSql += vbCrLf + "  SET MELT_RETAG='M', BAGNO = '" & BagNo & "'"
                    strSql += vbCrLf + "  WHERE BATCHNO =" & batchno & " and sno = " & Sno & ""
                    strSql += vbCrLf + " UPDATE " & TDbame & "..ISSUE"
                    strSql += vbCrLf + "  SET MELT_RETAG='M', BAGNO = '" & BagNo & "'"
                    strSql += vbCrLf + "  WHERE BATCHNO =" & batchno & " AND SNO = " & Sno
                    'ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                    If CENTR_DB_BR Then
                        cmd = New OleDbCommand(strSql, cn, tran)
                        cmd.ExecuteNonQuery()
                    Else
                        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                    End If
                ElseIf row.Item("TRANSFERTYPE").ToString.ToUpper.Trim = "TO NONTAG" Then
                    Dim drrow As DataRow
                    drrow = Dtitemnontag.NewRow
                    drrow.Item("SNO") = row!SNO.ToString
                    drrow.Item("METALID") = row!METALID.ToString
                    drrow.Item("TRANTYPE") = row!TYPE.ToString
                    drrow.Item("PCS") = Val(row!PCS.ToString)
                    drrow.Item("GRSWT") = Val(row!GRSWT.ToString)
                    drrow.Item("NETWT") = Val(row!NETWT.ToString)
                    drrow.Item("PUREWT") = Val(row!PUREWT.ToString)
                    drrow.Item("AMOUNT") = Val(row!AMOUNT.ToString)
                    drrow.Item("COUNTER") = row!COUNTER.ToString
                    Dtitemnontag.Rows.Add(drrow)
                    Dtitemnontag.AcceptChanges()

                    strSql = " UPDATE " & TDbame & "..RECEIPT"
                    strSql += vbCrLf + "  SET MELT_RETAG='N', BAGNO = '" & BagNo & "'"
                    strSql += vbCrLf + "  WHERE BATCHNO ='" & row!batchno.ToString & "' AND SNO ='" & row!SNO.ToString & "'"
                    strSql += vbCrLf + " UPDATE " & TDbame & "..ISSUE"
                    strSql += vbCrLf + "  SET MELT_RETAG='N', BAGNO = '" & BagNo & "'"
                    strSql += vbCrLf + "  WHERE BATCHNO ='" & row!batchno.ToString & "' AND SNO = '" & row!SNO.ToString & "'"
                    'ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                    If CENTR_DB_BR Then
                        cmd = New OleDbCommand(strSql, cn, tran)
                        cmd.ExecuteNonQuery()
                    Else
                        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                    End If
                Else
                    entord += 1
                    Dim itemid As Integer = 0
                    Dim pcs As Integer = Val(row!pcs.ToString)
                    If row!itemname.ToString <> "" Then
                        itemid = Val(objGPack.GetSqlValue("select itemid from " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & row!itemname.ToString & "'", , , tran).ToString)
                    Else
                        If row.Item(3).ToString = "PARTLY SALES" Then
                            KEY = "PARTLY-"
                        ElseIf row.Item(3).ToString = "SALES RETURN" Then
                            KEY = "RETURN-"
                        ElseIf row.Item(3).ToString = "PURCHASE" Then
                            KEY = "PURCHASE-"
                        End If
                        KEY = KEY & row!metalId.ToString
                        itemid = Val(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = '" & KEY & "'", , , tran))
                    End If
                    If row.Item(3).ToString = "PARTLY SALES" And pcs = 0 Then pcs = 1

                    Dim subitemid As Integer = 0
                    Dim designerid As Integer = 0
                    If row.Item(3).ToString = "PARTLY SALES" Or row.Item(3).ToString = "MISC ISSUE" Then
                        Dim diss As DataRow = GetSqlRow("SELECT SUBITEMID,TAGDESIGNER FROM " & cnStockDb & "..ISSUE WHERE SNO = '" & row!sno.ToString & "'", cn, tran)
                        subitemid = Val(diss(0).ToString)
                        designerid = Val(diss(1).ToString)
                    End If
                    If itemid = 0 Then MsgBox("Posing Item Empty" & vbCrLf & " Please set the item id in the soft control " & KEY) : tran.Rollback() : Exit Sub
                    Dim Defcounterid As Integer = Val(GetAdmindbSoftValue("PENDTRF_CTRID", "0", tran).ToString)

                    lotSno = GetNewSno(TranSnoType.ITEMLOTCODE, tran, "GET_ADMINSNO_TRAN") '  GetWSno(TranSnoType.ITEMLOTCODE, Tran, CnStockdb)

                    Dim stocktype As String
                    Dim noOfTag As Integer
                    Dim itemCounterId As String
                    Dim Isscatcode As String
                    Dim VALUEADDEDTYPE As String
                    strSql = "SELECT ISNULL(STOCKTYPE,'') STOCKTYPE,ISNULL(NOOFPIECE,0) NOOFPIECE,ISNULL(DEFAULTCOUNTER,0) DEFCOUNTER,VALUEADDEDTYPE,CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & itemid
                    Dim dritem As DataRow = GetSqlRow(strSql, cn, tran)

                    If Not dritem Is Nothing Then
                        stocktype = dritem.Item(0).ToString()
                        noOfTag = dritem.Item(1).ToString()
                        itemCounterId = dritem.Item(2).ToString()
                        VALUEADDEDTYPE = dritem.Item(3).ToString
                        Isscatcode = dritem.Item(4).ToString
                    End If


                    If row!COUNTER.ToString <> "" Then itemCounterId = objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME='" & row!COUNTER.ToString & "'", , "", tran)
                    Dim StrItemid As Integer
                    If row!ITEMNAME.ToString <> "" Then
                        StrItemid = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & row!ITEMNAME.ToString & "'", , "", tran))
                    End If
                    Dim TagNo As String
                    If row.Item(3).ToString = "SALES RETURN" Then
                        TagNo = objGPack.GetSqlValue("SELECT TAGNO FROM " & cnStockDb & "..RECEIPT WHERE SNO='" & row!SNO.ToString & "'", , "", tran)
                    Else
                        TagNo = objGPack.GetSqlValue("SELECT TAGNO FROM " & cnStockDb & "..ISSUE WHERE SNO='" & row!SNO.ToString & "'", , "", tran)
                    End If
                    strSql = "SELECT STKTYPE FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO='" & TagNo & "' AND ITEMID=" & StrItemid
                    Dim Stktype As String = objGPack.GetSqlValue(strSql, "STKTYPE", "", tran)
                    If row.Item(3).ToString = "SALES RETURN" Then
                        Stktype = "T"
                    End If
                    Dim entryType As String = "R"
                    Dim mwastper As Decimal = 0
                    Dim mmcgrm As Decimal = 0

                    If Defcounterid <> 0 Then itemCounterId = Defcounterid

                    strSql = " INSERT INTO " & cnAdminDb & "..ITEMLOT "
                    strSql += vbCrLf + " ("
                    strSql += vbCrLf + " SNO,ENTRYTYPE,LOTDATE,LOTNO,DESIGNERID,TABLECODE,TRANINVNO,"
                    strSql += vbCrLf + " BILLNO,COSTID,ENTRYORDER,ORDREPNO,ORDENTRYORDER,"
                    strSql += vbCrLf + " ITEMID,SUBITEMID,PCS,GRSWT,STNPCS,STNWT,STNUNIT,"
                    strSql += vbCrLf + " DIAPCS,DIAWT,NETWT,NOOFTAG,RATE,ITEMCTRID,WMCTYPE,"
                    strSql += vbCrLf + " BULKLOT,MULTIPLETAGS,NARRATION,FINERATE,TUCH,"
                    strSql += vbCrLf + " WASTPER,MCGRM,OTHCHARGE,STARTTAGNO,ENDTAGNO,CURTAGNO,"
                    strSql += vbCrLf + " COMPANYID,CPCS,CGRSWT,COMPLETED,CANCEL,ACCESSING,USERID,UPDATED,"
                    strSql += vbCrLf + " UPTIME,SYSTEMID,APPVER,ITEMTYPEID,STKTYPE)VALUES("
                    strSql += vbCrLf + " '" & lotSno & "'" 'SNO
                    strSql += vbCrLf + " ,'" & entryType & "'" 'ENTRYTYPE
                    strSql += vbCrLf + " ,'" & BillDate.ToString("yyyy-MM-dd") & "'" 'LOTDATE 'DateTime.Parse(RoIssue(CNT).ITEM("LOTDATE"), ukCultureInfo.DateTimeFormat).ToString("yyyy-MM-dd")
                    strSql += vbCrLf + " ," & LotNo & "" 'LOTNO
                    'strSql += vbcrlf + " ," & designerid & "" 'DESIGNERID
                    strSql += vbCrLf + " ,'" & row!DESIGNERID & "'" 'DESIGNERID
                    strSql += vbCrLf + " ,'" & row!TABLECODE & "'" 'DESIGNERID
                    strSql += vbCrLf + " ,''" 'TRANINVNO
                    strSql += vbCrLf + " ,''" 'BILLNO
                    strSql += vbCrLf + " ,'" & row!CostId & "'" 'COSTID
                    strSql += vbCrLf + " ," & entord & "" 'ENTRYORDER
                    strSql += vbCrLf + " ,''" 'ORDREPNO
                    strSql += vbCrLf + " ,0" 'ORDENTRYORDER
                    strSql += vbCrLf + " ," & itemid & "" 'ITEMID
                    strSql += vbCrLf + " ,0"
                    strSql += vbCrLf + " ," & pcs & "" 'PCS
                    strSql += vbCrLf + " ," & Val(row!Grswt.ToString) & "" 'GRSWT
                    strSql += vbCrLf + " ,0" 'STNPCS
                    strSql += vbCrLf + " ," & Val(row!STNWT.ToString) & "" 'STNWT
                    strSql += vbCrLf + " ,'G'" 'STNUNIT
                    strSql += vbCrLf + " ,0" 'DIAPCS
                    strSql += vbCrLf + " ," & Val(row!DIAWT.ToString) & "" 'DIAWT
                    strSql += vbCrLf + " ," & Val(row!Netwt.ToString) & "" 'NETWT
                    strSql += vbCrLf + " ," & IIf(noOfTag = 0, 1, noOfTag) & "" 'NOOFTAG
                    strSql += vbCrLf + " ,0" 'RATE
                    strSql += vbCrLf + " ," & Val(itemCounterId) & "" 'ITEMCTRID
                    strSql += vbCrLf + " ,'" & VALUEADDEDTYPE & "'" 'WMCTYPE
                    strSql += vbCrLf + " ,'N'" 'BULKLOT
                    strSql += vbCrLf + " ,'N'" 'MULTIPLETAGS
                    strSql += vbCrLf + " ,'TRF. FROM " & KEY & "'" 'NARRATION
                    strSql += vbCrLf + " ,0" 'FINERATE
                    strSql += vbCrLf + " ,0"
                    strSql += vbCrLf + " ," & mwastper & "" 'WASTPER
                    strSql += vbCrLf + " ," & mmcgrm & "" 'MCGRM
                    strSql += vbCrLf + " ,0" 'OTHCHARGE
                    strSql += vbCrLf + " ,''" 'STARTTAGNO
                    strSql += vbCrLf + " ,''" 'ENDTAGNO
                    strSql += vbCrLf + " ,''" 'CURTAGNO
                    strSql += vbCrLf + " ,'" & GetStockCompId() & "'" 'sCOMPANYID
                    strSql += vbCrLf + " ,0" 'CPIECE
                    strSql += vbCrLf + " ,0" 'CWEIGHT
                    strSql += vbCrLf + " ,''" 'COMPLETED
                    strSql += vbCrLf + " ,''" 'CANCEL
                    strSql += vbCrLf + " ,''" 'ACCESSING
                    strSql += vbCrLf + " ," & userId & "" 'USERID
                    strSql += vbCrLf + " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                    strSql += vbCrLf + " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                    strSql += vbCrLf + " ,'" & systemId & "'" 'SYSTEMID
                    strSql += vbCrLf + " ,'" & VERSION & "'" 'APPVER
                    strSql += vbCrLf + " ,0" 'ITEMTYPEID"
                    strSql += vbCrLf + " ,'" & Stktype & "'" 'STKTYPE
                    strSql += vbCrLf + " )"
                    'ExecQuery(SyncMode.Stock, strSql, cn, tran, cnCostId)
                    If CENTR_DB_BR Then
                        cmd = New OleDbCommand(strSql, cn, tran)
                        cmd.ExecuteNonQuery()
                    Else
                        ExecQuery(SyncMode.Stock, strSql, cn, tran, cnCostId)
                    End If

                    strSql = " UPDATE " & TDbame & "..RECEIPT"
                    strSql += vbCrLf + "  SET MELT_RETAG='L', BAGNO = '" & LotNo & "'"
                    strSql += vbCrLf + "  WHERE BATCHNO ='" & row!batchno.ToString & "' AND SNO ='" & row!SNO.ToString & "'"
                    strSql += vbCrLf + " UPDATE " & TDbame & "..ISSUE"
                    strSql += vbCrLf + "  SET MELT_RETAG='L', BAGNO = '" & LotNo & "'"
                    strSql += vbCrLf + "  WHERE BATCHNO ='" & row!batchno.ToString & "' AND SNO = '" & row!SNO.ToString & "'"
                    'ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                    If CENTR_DB_BR Then
                        cmd = New OleDbCommand(strSql, cn, tran)
                        cmd.ExecuteNonQuery()
                    Else
                        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                    End If

                    strSql = "SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME='" & row!CATNAME.ToString & "'"
                    Dim Catcode As String = objGPack.GetSqlValue(strSql, , , tran)
                    If Isscatcode = Catcode Then Continue For

                    Dim dtrow() As DataRow
                    dtrow = dtCatstk.Select("CATCODE='" & Catcode & "' AND TRANTYPE='IIN'")
                    If dtrow.Length > 0 Then
                        Dim CurRowindex As Integer = Val(dtrow(0).Item("SNO").ToString)
                        With dtCatstk.Rows(CurRowindex)
                            .Item("CATCODE") = Catcode
                            .Item("METALID") = row!metalId.ToString
                            .Item("PCS") = Val(dtCatstk.Rows(CurRowindex).Item("PCS").ToString) + Val(row!PCS.ToString)
                            .Item("GRSWT") = Val(dtCatstk.Rows(CurRowindex).Item("GRSWT").ToString) + Val(row!GRSWT.ToString)
                            .Item("NETWT") = Val(dtCatstk.Rows(CurRowindex).Item("NETWT").ToString) + Val(row!NETWT.ToString)
                            .Item("TRANTYPE") = "IIN"
                            .Item("REMARK1") = "ISSUE TO RE-TAG"
                        End With
                    Else
                        instkrow = dtCatstk.NewRow
                        With instkrow
                            .Item("CATCODE") = Catcode
                            .Item("METALID") = row!metalId.ToString
                            .Item("PCS") = row!PCS.ToString
                            .Item("GRSWT") = row!GRSWT.ToString
                            .Item("NETWT") = row!NETWT.ToString
                            .Item("TRANTYPE") = "IIN"
                            .Item("REMARK1") = "ISSUE TO RE-TAG"
                        End With
                        dtCatstk.Rows.Add(instkrow)
                    End If
                    dtCatstk.AcceptChanges()

                    Dim dtrow1() As DataRow
                    dtrow1 = dtCatstk.Select("CATCODE='" & Isscatcode & "' AND TRANTYPE='RIN'")
                    If dtrow.Length > 0 Then
                        Dim CurRowindex As Integer = Val(dtrow1(0).Item("SNO").ToString)
                        With dtCatstk.Rows(CurRowindex)
                            .Item("CATCODE") = Isscatcode
                            .Item("METALID") = row!metalId.ToString
                            .Item("PCS") = Val(dtCatstk.Rows(CurRowindex).Item("PCS").ToString) + Val(row!PCS.ToString)
                            .Item("GRSWT") = Val(dtCatstk.Rows(CurRowindex).Item("GRSWT").ToString) + Val(row!GRSWT.ToString)
                            .Item("NETWT") = Val(dtCatstk.Rows(CurRowindex).Item("NETWT").ToString) + Val(row!NETWT.ToString)
                            .Item("TRANTYPE") = "RIN"
                            .Item("REMARK1") = "RECD FOR RE-TAG"
                        End With
                    Else
                        instkrow = dtCatstk.NewRow
                        With instkrow
                            .Item("CATCODE") = Isscatcode
                            .Item("METALID") = row!metalId.ToString
                            .Item("PCS") = row!PCS.ToString
                            .Item("GRSWT") = row!GRSWT.ToString
                            .Item("NETWT") = row!NETWT.ToString
                            .Item("TRANTYPE") = "RIN"
                            .Item("REMARK1") = "RECD FOR RE-TAG"
                        End With
                        dtCatstk.Rows.Add(instkrow)
                    End If
                    dtCatstk.AcceptChanges()
                End If
                InsertPartwtIssRes(row)
                If Not PSSR_CATPOSTING Is Nothing And (row.Item("TYPE").ToString = "PARTLY SALES" Or row.Item("TYPE").ToString = "SALES RETURN") Then
                    If Val(NEWBILLNO) = 0 Then GoTo GenerateNewBillNo
                    InsertPartSRwtIss(row, BagNo, NEWBILLNO)
                End If
            Next
            If Dtitemnontag.Rows.Count > 0 Then
                Dim msg As String = ""
                msg = InsertItemnontag(Dtitemnontag, BagNo)
                If msg <> "" Then Throw New Exception(msg)
            End If
            If dtCatstk.Rows.Count > 0 Then
                Dim driss() As DataRow = dtCatstk.Select("TRANTYPE='IIN'")
                If Not driss Is Nothing Then InsertIssueReceipt("IIN", driss)
                Dim drrec() As DataRow = dtCatstk.Select("TRANTYPE='RIN'")
                If Not drrec Is Nothing Then InsertIssueReceipt("RIN", drrec)
            End If
            Dim dxChk() As DataRow = dt.Select("CHECK = TRUE AND TRANSFERTYPE='TO DEALER' AND SMITH<>'' AND CATNAME<>''")
            If dxChk.Length > 0 Then InsertSADetails(dt, BagNo)
            tran.Commit()
            tran = Nothing
            MsgBox(IIf(BagNo <> "", " BagNo   : " & BagNo & vbCrLf, "") _
            & IIf(LotNo <> 0, vbCrLf & " Lot No : " & LotNo & vbCrLf, "") _
            & IIf(BJobNo <> "", " JobNo   : " & BJobNo & vbCrLf, "") _
            & IIf(IssNo <> 0, " IssueNo   : " & IssNo & vbCrLf, "") _
            & " Generated..")
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try

        'btnLoad_Click(Me, New EventArgs)
    End Sub

    Private Function InsertItemnontag(ByVal DtNontag As DataTable, ByVal BagNo As String) As String
        Dim KEY As String
        Dim itemid As Integer = 0
        Dim dttrantype As New DataTable
        Dim msg As String = ""
        dttrantype = DtNontag.DefaultView.ToTable(True, "TRANTYPE")
        Dim dtmetal As New DataTable
        dtmetal = DtNontag.DefaultView.ToTable(True, "METALID")
        Dim Defcounterid As Integer = Val(GetAdmindbSoftValue("PENDTRF_CTRID", "0", tran).ToString)
        For i As Integer = 0 To dttrantype.Rows.Count - 1
            For v As Integer = 0 To dtmetal.Rows.Count - 1
                Dim row() As DataRow = DtNontag.Select("TRANTYPE='" & dttrantype.Rows(i).Item("TRANTYPE").ToString & "' AND METALID='" & dtmetal.Rows(v).Item("METALID").ToString & "'")

                If dttrantype.Rows(i).Item("TRANTYPE").ToString = "PARTLY SALES" Then
                    KEY = "PARTLY-"
                ElseIf dttrantype.Rows(i).Item("TRANTYPE").ToString = "SALES RETURN" Then
                    KEY = "RETURN-"
                ElseIf dttrantype.Rows(i).Item("TRANTYPE").ToString = "PURCHASE" Then
                    KEY = "PURCHASE-"
                End If
                KEY = KEY & dtmetal.Rows(v).Item("METALID").ToString
                Dim grswt As Decimal = IIf(IsDBNull(DtNontag.Compute("SUM(GRSWT)", "TRANTYPE='" & dttrantype.Rows(i).Item("TRANTYPE").ToString & "' AND METALID='" & dtmetal.Rows(v).Item("METALID").ToString & "'")), 0, DtNontag.Compute("SUM(GRSWT)", "TRANTYPE='" & dttrantype.Rows(i).Item("TRANTYPE").ToString & "' AND METALID='" & dtmetal.Rows(v).Item("METALID").ToString & "'"))
                Dim netwt As Decimal = IIf(IsDBNull(DtNontag.Compute("SUM(NETWT)", "TRANTYPE='" & dttrantype.Rows(i).Item("TRANTYPE").ToString & "' AND METALID='" & dtmetal.Rows(v).Item("METALID").ToString & "'")), 0, DtNontag.Compute("SUM(NETWT)", "TRANTYPE='" & dttrantype.Rows(i).Item("TRANTYPE").ToString & "' AND METALID='" & dtmetal.Rows(v).Item("METALID").ToString & "'"))

                itemid = Val(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = '" & KEY & "'", , , tran))
                Dim subitemid As Integer = 0
                Dim designerid As Integer = 0

                'If dttrantype.Rows(i).Item("TRANTYPE").ToString = "PARTLY SALES" Or dttrantype.Rows(i).Item("TRANTYPE").ToString = "MISC ISSUE" Then
                '    Dim diss As DataRow = GetSqlRow("SELECT SUBITEMID,TAGDESIGNER FROM " & cnStockDb & "..ISSUE WHERE SNO = '" & row(0).Item("SNO").ToString & "'", cn, tran)
                '    If Not diss Is Nothing Then
                '        subitemid = Val(diss(0).ToString)
                '        designerid = Val(diss(1).ToString)
                '    End If
                'End If

                If itemid = 0 Then msg = "Posting Item Empty" & vbCrLf & " Please set the item id in the soft control " & KEY : Return msg

                Dim NontagSno As String = GetNewSno(TranSnoType.ITEMNONTAGCODE, tran, "GET_ADMINSNO_TRAN")

                strSql = " SELECT ISNULL(STOCKTYPE,'') STOCKTYPE,ISNULL(DEFAULTCOUNTER,0) DEFCOUNTER FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & itemid
                Dim dritem As DataRow = GetSqlRow(strSql, cn, tran)
                Dim stocktype As String
                Dim itemCounterId As String
                If Not dritem Is Nothing Then
                    stocktype = dritem.Item(0).ToString()
                    itemCounterId = Val(dritem.Item(1).ToString)
                End If
                If row(0).Item("COUNTER").ToString <> "" Then itemCounterId = objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME='" & row(0).Item("COUNTER").ToString & "'", , "", tran)

                If Defcounterid <> 0 Then itemCounterId = Defcounterid
                If stocktype = "T" Then MsgBox("Posing Item is taged item." & vbCrLf & " Please set nontag itemid in the soft control " & KEY) : tran.Rollback() : Exit Function

                strSql = " INSERT INTO " & cnAdminDb & "..ITEMNONTAG"
                strSql += vbCrLf + " ("
                strSql += vbCrLf + " SNO,ITEMID,SUBITEMID,COMPANYID,RECDATE,PCS,GRSWT,LESSWT,NETWT,FINRATE"
                strSql += vbCrLf + " ,ISSTYPE,RECISS,POSTED,LOTNO,PACKETNO,DREFNO,ITEMCTRID,ORDREPNO,ORSNO"
                strSql += vbCrLf + " ,NARRATION,PURWASTAGE,PURRATE,PURMC,RATE,COSTID,TCOSTID,CTGRM,DESIGNERID"
                strSql += vbCrLf + " ,ITEMTYPEID,CARRYFLAG,REASON,BATCHNO,CANCEL,USERID,UPDATED,UPTIME"
                strSql += vbCrLf + " ,SYSTEMID,APPVER,WASTPER,WASTAGE,MCPERGRM,MC,LOTSNO,EXTRAWT,STYLENO"
                strSql += vbCrLf + " )VALUES("
                strSql += vbCrLf + " '" & NontagSno & "'" 'SNO
                strSql += vbCrLf + " ," & itemid & "" 'ITEMID
                strSql += vbCrLf + " ," & subitemid & "" 'SUBITEMID
                strSql += vbCrLf + " ,'" & GetStockCompId() & "'" 'COMPANYID
                strSql += vbCrLf + " ,'" & BillDate.ToString("yyyy-MM-dd") & "'" 'RECDATE
                strSql += vbCrLf + " ,0" 'PCS
                strSql += vbCrLf + " ," & grswt & "" 'GRSWT
                strSql += vbCrLf + " ," & grswt - netwt & "" 'LESSWT
                strSql += vbCrLf + " ," & netwt & "" 'NETWT
                strSql += vbCrLf + " ,0" 'FINRATE
                strSql += vbCrLf + " ,''" 'ISSTYPE
                strSql += vbCrLf + " ,'R'" 'RECISS
                strSql += vbCrLf + " ,''" 'POSTED
                strSql += vbCrLf + " ,0" 'LOTNO
                strSql += vbCrLf + " ,'" & BagNo & "'" 'PACKETNO
                strSql += vbCrLf + " ,0" 'DREFNO
                strSql += vbCrLf + " ," & itemCounterId & "" 'ITEMCTRID
                strSql += vbCrLf + " ,''" 'ORDREPNO
                strSql += vbCrLf + " ,''" 'ORSNO
                strSql += vbCrLf + " ,'TRANS FROM PENDING'" 'NARRATION
                strSql += vbCrLf + " ,0" 'PURWASTAGE
                strSql += vbCrLf + " ,0" 'PURRATE
                strSql += vbCrLf + " ,0" 'PURMC
                strSql += vbCrLf + " ,0" 'RATE
                strSql += vbCrLf + " ,'" & cnCostId & "'" 'COSTID
                strSql += vbCrLf + " ,'" & cnCostId & "'" 'TCOSTID
                strSql += vbCrLf + " ,''" 'CTGRM
                strSql += vbCrLf + " ," & designerid & "" 'DESIGNERID
                strSql += vbCrLf + " ,0" 'ITEMTYPEID
                strSql += vbCrLf + " ,''" 'CARRYFLAG
                strSql += vbCrLf + " ,'0'" 'REASON
                strSql += vbCrLf + " ,''" 'BATCHNO
                strSql += vbCrLf + " ,''" 'CANCEL
                strSql += vbCrLf + " ," & userId & "" 'USERID
                strSql += vbCrLf + " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                strSql += vbCrLf + " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                strSql += vbCrLf + " ,'" & systemId & "'" 'SYSTEMID
                strSql += vbCrLf + " ,'" & VERSION & "'" 'APPVER
                strSql += vbCrLf + " ,0" 'WASTAGEPER
                strSql += vbCrLf + " ,0" 'WASTAGE
                strSql += vbCrLf + " ,0" 'MCGRM
                strSql += vbCrLf + " ,0" 'MC
                strSql += vbCrLf + " ,''" 'LOTSNO
                strSql += vbCrLf + " ,'0'" 'EXTRA WEIGHT
                strSql += vbCrLf + " ,''" 'STYLENO
                strSql += vbCrLf + " )"
                'ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId, , , "TITEMNONTAG", , False)
                If CENTR_DB_BR Then
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()
                Else
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId, , , "TITEMNONTAG", , False)
                End If
            Next
        Next
        Return msg
    End Function
    Private Sub InsertPartwtIssRes(ByVal dtr As DataRow)
        'If dtr.Item(3).ToString = "PARTLY SALES" Then Exit Sub
        If Val(dtr.Item("PCS").ToString) <> Val(dtr.Item("OPCS").ToString) Or Val(dtr.Item("GRSWT").ToString) <> Val(dtr.Item("OGRSWT").ToString) Or Val(dtr.Item("CNETWT").ToString) <> Val(dtr.Item("ONETWT").ToString) Then
            Dim issSno As String = Nothing
            Dim amount As Double = 0
            Dim vat As Double = 0
            Dim DTitemdet As New DataTable
            If dtr.Item("TYPE").ToString <> "PURCHASE" Then Exit Sub
            If dtr.Item("TYPE").ToString = "SALES RETURN" Or dtr.Item("TYPE").ToString = "PURCHASE" Then
                strSql = " SELECT * FROM " & cnStockDb & "..RECEIPT WHERE SNO='" & dtr.Item("SNO").ToString & "' AND BATCHNO='" & dtr.Item("BATCHNO").ToString & "'"
                'Else
                '    strSql = " SELECT * FROM " & cnStockDb & "..ISSUE WHERE SNO='" & dtr.Item("SNO").ToString & "' AND BATCHNO='" & dtr.Item("BATCHNO").ToString & "'"
            End If
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(DTitemdet)
            If DTitemdet.Rows.Count <= 0 Then Exit Sub
            If Val(dtr.Item("GRSWT").ToString) <> 0 Then
                amount = CalcRoundoffAmt(((Val(DTitemdet.Rows(0).Item("AMOUNT").ToString)) / (Val(DTitemdet.Rows(0).Item("GRSWT").ToString))) * Val(dtr.Item("GRSWT").ToString), objSoftKeys.RoundOff_Gross)
                vat = CalcRoundoffAmt(((Val(DTitemdet.Rows(0).Item("TAX").ToString)) / (Val(DTitemdet.Rows(0).Item("GRSWT").ToString))) * Val(dtr.Item("GRSWT").ToString), objSoftKeys.RoundOff_Vat)
            ElseIf Val(dtr.Item("CNETWT").ToString) <> 0 Then
                amount = CalcRoundoffAmt(((Val(DTitemdet.Rows(0).Item("AMOUNT").ToString)) / (Val(DTitemdet.Rows(0).Item("NETWT").ToString))) * Val(dtr.Item("CNETWT").ToString), objSoftKeys.RoundOff_Gross)
                vat = CalcRoundoffAmt(((Val(DTitemdet.Rows(0).Item("TAX").ToString)) / (Val(DTitemdet.Rows(0).Item("NETWT").ToString))) * Val(dtr.Item("CNETWT").ToString), objSoftKeys.RoundOff_Vat)
            Else
                amount = 0
                vat = 0
            End If

            If dtr.Item("TYPE").ToString = "SALES RETURN" Or dtr.Item("TYPE").ToString = "PURCHASE" Then
                strSql = " UPDATE " & cnStockDb & "..RECEIPT"
                'Else
                '    strSql = " UPDATE " & cnStockDb & "..ISSUE"
            End If
            strSql += vbCrLf + " SET PCS=" & Val(dtr.Item("PCS").ToString) & ",GRSWT=" & Val(dtr.Item("GRSWT").ToString) & ""
            strSql += vbCrLf + " ,NETWT=" & Val(dtr.Item("CNETWT").ToString) & ""
            strSql += vbCrLf + " ,AMOUNT=" & amount & ",TAX=" & vat & ""
            strSql += vbCrLf + " WHERE SNO='" & dtr.Item("SNO").ToString & "' AND BATCHNO='" & dtr.Item("BATCHNO").ToString & "'"
            'ExecQuery(SyncMode.Transaction, strSql, cn, tran, DTitemdet.Rows(0).Item("COSTID").ToString)
            If CENTR_DB_BR Then
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
            Else
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, DTitemdet.Rows(0).Item("COSTID").ToString)
            End If

            Dim pcs As Integer = Math.Abs(Val(dtr.Item("OPCS").ToString) - Val(dtr.Item("PCS").ToString))
            Dim grswt As Decimal = Math.Abs(Val(dtr.Item("OGRSWT").ToString) - Val(dtr.Item("GRSWT").ToString))
            Dim netwt As Decimal = Math.Abs(Val(dtr.Item("ONETWT").ToString) - Val(dtr.Item("CNETWT").ToString))

            If Val(dtr.Item("GRSWT").ToString) <> 0 Then
                amount = CalcRoundoffAmt(((Val(DTitemdet.Rows(0).Item("AMOUNT").ToString)) / (Val(DTitemdet.Rows(0).Item("GRSWT").ToString))) * grswt, objSoftKeys.RoundOff_Gross)
                vat = CalcRoundoffAmt(((Val(DTitemdet.Rows(0).Item("TAX").ToString)) / (Val(DTitemdet.Rows(0).Item("GRSWT").ToString))) * grswt, objSoftKeys.RoundOff_Vat)
            ElseIf Val(dtr.Item("CNETWT").ToString) <> 0 Then
                amount = CalcRoundoffAmt(((Val(DTitemdet.Rows(0).Item("AMOUNT").ToString)) / (Val(DTitemdet.Rows(0).Item("NETWT").ToString))) * netwt, objSoftKeys.RoundOff_Gross)
                vat = CalcRoundoffAmt(((Val(DTitemdet.Rows(0).Item("TAX").ToString)) / (Val(DTitemdet.Rows(0).Item("NETWT").ToString))) * netwt, objSoftKeys.RoundOff_Vat)
            Else
                amount = 0
                vat = 0
            End If

            With DTitemdet.Rows(0)
                If dtr.Item("TYPE").ToString = "SALES RETURN" Or dtr.Item("TYPE").ToString = "PURCHASE" Then
                    strSql = " INSERT INTO " & cnStockDb & "..RECEIPT"
                    issSno = GetNewSno(TranSnoType.RECEIPTCODE, tran)
                    'Else
                    '    strSql = " INSERT INTO " & cnStockDb & "..ISSUE"
                    '    issSno = GetNewSno(TranSnoType.ISSUECODE, tran)
                End If
                strSql += vbCrLf + " (SNO,TRANNO,TRANDATE,TRANTYPE,PCS,GRSWT,NETWT,LESSWT,PUREWT,TAGNO,ITEMID,SUBITEMID"
                strSql += vbCrLf + " ,WASTPER,WASTAGE,MCGRM,MCHARGE,AMOUNT,RATE,BOARDRATE,SALEMODE,GRSNET,TRANSTATUS"
                strSql += vbCrLf + " ,REFNO,REFDATE,COSTID,COMPANYID,FLAG,EMPID,TAGPCS,TAGGRSWT,TAGNETWT,TAGRATEID"
                strSql += vbCrLf + " ,TAGSVALUE,TAGDESIGNER,ITEMCTRID,ITEMTYPEID,PURITY,TABLECODE,INCENTIVE,WEIGHTUNIT"
                strSql += vbCrLf + " ,CATCODE,OCATCODE,ACCODE,ALLOY,BATCHNO,REMARK1,REMARK2,USERID,UPDATED,UPTIME"
                strSql += vbCrLf + " ,SYSTEMID,DISCOUNT,RUNNO,CASHID,TAX,SC,STNAMT,MISCAMT,METALID,STONEUNIT,APPVER"
                strSql += vbCrLf + " ,TOUCH,ESTSNO,FIN_DISCOUNT,RATEID,SETGRPID,TRANFLAG,ORDSTATE_ID )"
                strSql += vbCrLf + " VALUES("
                strSql += vbCrLf + " '" & issSno & "'" ''SNO
                strSql += vbCrLf + " ," & Val(.Item("TRANNO").ToString) & "" 'TRANNO
                strSql += vbCrLf + " ,'" & Format(.Item("TRANDATE"), "yyyy-MM-dd") & "'" 'TRANDATE 
                strSql += vbCrLf + " ,'" & .Item("TRANTYPE").ToString & "'" 'TRANTYPE
                strSql += vbCrLf + " ," & pcs & "" 'PCS
                strSql += vbCrLf + " ," & grswt & "" 'GRSWT
                strSql += vbCrLf + " ," & netwt & "" 'NETWT
                strSql += vbCrLf + " ," & Math.Abs(grswt - netwt) & "" 'LESSWT
                strSql += vbCrLf + " ," & Val(.Item("PUREWT").ToString) & "" 'PUREWT '
                strSql += vbCrLf + " ,'" & .Item("TAGNO").ToString & "'" 'TAGNO
                strSql += vbCrLf + " ," & Val(.Item("ITEMID").ToString) & "" 'ITEMID
                strSql += vbCrLf + " ," & Val(.Item("SUBITEMID").ToString) & "" 'SUBITEMID
                strSql += vbCrLf + " ," & Val(.Item("WASTPER").ToString) & "" 'WASTPER
                strSql += vbCrLf + " ," & Val(.Item("WASTAGE").ToString) & "" 'WASTAGE
                strSql += vbCrLf + " ," & Val(.Item("MCGRM").ToString) & "" 'MCGRM
                strSql += vbCrLf + " ," & Val(.Item("MCHARGE").ToString) & "" 'MCHARGE
                strSql += vbCrLf + " ," & amount & "" 'AMOUNT
                strSql += vbCrLf + " ," & Val(.Item("RATE").ToString) & "" 'RATE
                strSql += vbCrLf + " ," & Val(.Item("BOARDRATE").ToString) & "" 'BOARDRATE
                strSql += vbCrLf + " ,'" & .Item("SALEMODE").ToString & "'" 'SALEMODE
                strSql += vbCrLf + " ,'" & .Item("GRSNET").ToString & "'" 'GRSNET
                strSql += vbCrLf + " ,''" 'TRANSTATUS ''
                strSql += vbCrLf + " ,'" & .Item("REFNO").ToString & "'" 'REFNO ''
                If .Item("REFDATE").ToString <> Nothing Then
                    strSql += vbCrLf + " ,'" & .Item("REFDATE").ToString & "'" 'REFDATE NULL
                Else
                    strSql += vbCrLf + " ,NULL" 'REFDATE NULL
                End If
                strSql += vbCrLf + " ,'" & .Item("COSTID").ToString & "'" 'COSTID 
                strSql += vbCrLf + " ,'" & .Item("COMPANYID").ToString & "'" 'COMPANYID
                strSql += vbCrLf + " ,'" & .Item("FLAG").ToString & "'" 'FLAG 
                strSql += vbCrLf + " ," & Val(.Item("EMPID").ToString) & "" 'EMPID
                strSql += vbCrLf + " ," & Val(.Item("TAGPCS").ToString) & "" 'TAGPCS
                strSql += vbCrLf + " ," & Val(.Item("TAGGRSWT").ToString) & "" 'TAGGRSWT
                strSql += vbCrLf + " ," & Val(.Item("TAGNETWT").ToString) & "" 'TAGNETWT
                strSql += vbCrLf + " ," & Val(.Item("TAGRATEID").ToString) & "" 'TAGRATEID
                strSql += vbCrLf + " ," & Val(.Item("TAGSVALUE").ToString) & "" 'TAGSVALUE
                strSql += vbCrLf + " ,'" & .Item("TAGDESIGNER").ToString & "'" 'TAGDESIGNER
                strSql += vbCrLf + " ," & Val(.Item("ITEMCTRID").ToString) & "" 'ITEMCTRID
                strSql += vbCrLf + " ," & Val(.Item("ITEMTYPEID").ToString) & "" 'ITEMTYPEID
                strSql += vbCrLf + " ," & Val(.Item("PURITY").ToString) & "" 'PURITY
                strSql += vbCrLf + " ,'" & .Item("TABLECODE").ToString & "'" 'TABLECODE
                strSql += vbCrLf + " ,''" 'INCENTIVE
                strSql += vbCrLf + " ,''" 'WEIGHTUNIT
                strSql += vbCrLf + " ,'" & .Item("CATCODE").ToString & "'" 'CATCODE
                strSql += vbCrLf + " ,''" 'OCATCODE
                strSql += vbCrLf + " ,'" & .Item("ACCODE").ToString & "'" 'ACCODE
                strSql += vbCrLf + " ,0" 'ALLOY
                strSql += vbCrLf + " ,'" & .Item("BATCHNO").ToString & "'" 'BATCHNO
                strSql += vbCrLf + " ,'" & .Item("REMARK1").ToString & "'" 'REMARK1
                strSql += vbCrLf + " ,'" & .Item("REMARK2").ToString & "'" 'REMARK2
                strSql += vbCrLf + " ,'" & userId & "'" 'USERID
                strSql += vbCrLf + " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                strSql += vbCrLf + " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                strSql += vbCrLf + " ,'" & systemId & "'" 'SYSTEMID
                strSql += vbCrLf + " ," & Val(.Item("DISCOUNT").ToString) & "" 'DISCOUNT
                strSql += vbCrLf + " ,'" & .Item("RUNNO").ToString & "'" 'RUNNO
                strSql += vbCrLf + " ,'" & .Item("CASHID").ToString & "'" 'CASHID
                strSql += vbCrLf + " ," & vat & "" 'TAX
                strSql += vbCrLf + " ," & Val(.Item("SC").ToString) & "" 'SC
                strSql += vbCrLf + " ," & Val(.Item("STNAMT").ToString) & "" 'STNAMT
                strSql += vbCrLf + " ," & Val(.Item("MISCAMT").ToString) & "" 'MISCAMT
                strSql += vbCrLf + " ,'" & .Item("METALID").ToString & "'" 'MTALID
                strSql += vbCrLf + " ,'" & .Item("STONEUNIT").ToString & "'" 'STONEUNIT
                strSql += vbCrLf + " ,'" & VERSION & "'" 'APPVER
                strSql += vbCrLf + " ," & Val(.Item("TOUCH").ToString) & "" 'TOUCH '
                If Val(.Item("ESTSNO").ToString) > 0 Then
                    strSql += vbCrLf + " ,'" & Val(.Item("ESTSNO").ToString) & "'" 'ESSTNO'
                Else
                    strSql += vbCrLf + " ,''" 'ESTNO'
                End If
                strSql += vbCrLf + " ," & Val(.Item("FIN_DISCOUNT").ToString) & "" 'DISCOUNT
                strSql += vbCrLf + " ," & Val(.Item("RATEID").ToString) & "" 'RATEID
                strSql += vbCrLf + " ,'" & .Item("SETGRPID").ToString & "'" 'SETGRPID
                strSql += vbCrLf + " ,'" & .Item("TRANFLAG").ToString & "'" 'TRANFLAG
                strSql += vbCrLf + "," & Val(.Item("ORDSTATE_ID").ToString)
                strSql += vbCrLf + " )"
                'ExecQuery(SyncMode.Transaction, strSql, cn, tran, .Item("COSTID").ToString)
                If CENTR_DB_BR Then
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()
                Else
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, .Item("COSTID").ToString)
                End If
            End With
        End If
    End Sub


    Private Sub InsertPartSRwtIss(ByVal dtr As DataRow, ByVal PTrfNo As String, ByVal Tranno As Integer)
        '' newly added on 04-JUNE-2022 for sep ps/sr catcode posting as per mahesh sir advice FOR VBJ
        If Batchno = "" Then Batchno = GetNewBatchnoNew(cnCostId, BillDate, cn, tran)
        Dim issSno As String = Nothing
        Dim amount As Double = 0
        Dim vat As Double = 0


        Dim pcs As Integer = Math.Abs(Val(dtr.Item("PCS").ToString))
        Dim grswt As Decimal = Math.Abs(Val(dtr.Item("GRSWT").ToString))
        Dim netwt As Decimal = Math.Abs(Val(dtr.Item("NETWT").ToString))
        Dim _PSSRSNO As String = "" : Dim Trantype As String = "" : Dim Catcode As String = ""


        Catcode = dtr.Item("CATCODE").ToString
        Trantype = dtr.Item("TYPE").ToString
        If Not (Trantype = "PARTLY SALES" Or Trantype = "SALES RETURN") Then Exit Sub
        Dim TCostname As String = objGPack.GetSqlValue("SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID ='" & dtr.Item("COSTID").ToString & "'", "COSTNAME", dtr.Item("COSTID").ToString, tran)
        Dim TACCODE As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID ='" & dtr.Item("COSTID").ToString & "'", "ACCODE", "", tran)

        Dim NEWBILLNO As Integer = Tranno

        If Not PSSR_CATPOSTING Is Nothing And (Trantype = "PARTLY SALES" Or Trantype = "SALES RETURN") Then
            Dim _tempPssr_post() As String
            Dim _tempPssr_Catcode As String = ""
            For cnt As Integer = 0 To PSSR_CATPOSTING.Length - 1
                If PSSR_CATPOSTING(cnt).ToString.Contains(Catcode & ":") Then
                    _tempPssr_post = PSSR_CATPOSTING(cnt).ToString.Split(":")
                    If Trantype = "PARTLY SALES" And _tempPssr_post.Length = 3 Then
                        _tempPssr_Catcode = _tempPssr_post(1).ToString
                    ElseIf Trantype = "SALES RETURN" And _tempPssr_post.Length = 3 Then
                        _tempPssr_Catcode = _tempPssr_post(2).ToString
                    End If
                    Exit For
                End If
            Next
            If _tempPssr_post.Length = 3 And _tempPssr_Catcode <> "" Then
                _PSSRSNO = GetNewSno(TranSnoType.ISSUECODE, tran)
                strSql = " INSERT INTO " & cnStockDb & "..ISSUE"
                strSql += " ("
                strSql += " SNO,COMPANYID,COSTID,CATCODE,METALID,TRANDATE,TRANNO,TRANTYPE,"
                strSql += " PCS,GRSWT,LESSWT,NETWT,STNAMT,AMOUNT,BATCHNO, CANCEL,REFNO,REMARK1,REMARK2,ACCODE, "
                strSql += " USERID,UPDATED,UPTIME,SYSTEMID,APPVER,FLAG,TAX)VALUES("
                strSql += " '" & _PSSRSNO & "','" & GetStockCompId() & "','" & cnCostId & "'"
                strSql += ",'" & _tempPssr_Catcode & "'" 'CATCODE
                strSql += ",'" & dtr.Item("METALID").ToString & "'" 'METALID
                strSql += ",'" & BillDate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                strSql += " ," & NEWBILLNO & " ,'IIN'"
                strSql += " ," & Val(pcs) & "" 'PCS
                strSql += " ," & Val(grswt) & "" 'GRSWT
                strSql += " ," & Val(grswt) - Val(netwt) & "" 'LESSWT
                strSql += " ," & Val(netwt) & "" 'NETWT
                strSql += " ," & Val(0) & "" 'STNAMT
                strSql += " ," & Format(Val(0), "0.00") & "" 'AMOUNT
                strSql += " ,'" & Batchno & "' ,''" 'CANCEL
                strSql += " ,'" & PTrfNo & "'" 'REFNO
                strSql += " ,'TRANSFER FROM " & TCostname & "'" 'REMARK1
                strSql += " ,'" & Trantype & "'" 'REMARK2
                strSql += " ,'" & TACCODE & "' " 'ACCODE
                strSql += " ," & userId & "" 'USERID
                strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                strSql += " ,'" & systemId & "'" 'SYSTEMID
                strSql += " ,'" & VERSION & "'" 'APPVER
                strSql += " ,'T'" 'FLAG
                strSql += " ,'" & Format(Val(0), "0.00") & "'"   'TAX
                strSql += " )"
                If CENTR_DB_BR Then
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()
                Else
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                End If
            End If
        End If
        '' END newly added on 04-JUNE-2022 for sep ps/sr catcode posting as per mahesh sir advice FOR VBJ

    End Sub

    Public Sub InsertSADetails(ByVal dtt As DataTable, ByVal BagNo As String)
        If Batchno = "" Then Batchno = GetNewBatchnoNew(cnCostId, BillDate, cn, tran)
        Dim DtSmith As New DataTable
        DtSmith = dtt.DefaultView.ToTable(True, "SMITH")

        Dim Dtcatname As New DataTable
        Dtcatname = dtt.DefaultView.ToTable(True, "CATNAME")
        Dim tranno As Integer = 0
        Dim drChk() As DataRow
        drChk = dtt.Select("CHECK = TRUE AND TRANSFERTYPE='TO DEALER' AND SMITH<>'' AND CATNAME<>''")
        If drChk.Length > 0 Then
            For i As Integer = 0 To DtSmith.Rows.Count - 1
                If DtSmith.Rows(i).Item("SMITH").ToString.Trim = "NONE" Or DtSmith.Rows(i).Item("SMITH").ToString.Trim = "" Then Continue For
GenBillNo:
                tranno = Val(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = 'GEN-SM-ISS'  AND COMPANYID = '" & strCompanyId & "'", , , tran))
                strSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & tranno + 1 & "' WHERE CTLID = 'GEN-SM-ISS'  AND COMPANYID = '" & strCompanyId & "'"
                strSql += vbCrLf + " AND CONVERT(INT,CTLTEXT) = " & tranno & ""
                cmd = New OleDbCommand(strSql, cn, tran)
                If Not cmd.ExecuteNonQuery() > 0 Then
                    GoTo GenBillNo
                End If
                tranno += 1
                Dim JobNo As Integer = 0
                Dim mJobNo As String = ""
                If _JobNoEnable Then
                    strSql = " SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = 'GEN-JOBNO' AND COMPANYID = '" & strCompanyId & "'"
                    JobNo = Val(objGPack.GetSqlValue(strSql, , , tran))
                    mJobNo = GetCostId(cnCostId) + GetCompanyId(strCompanyId) + "J" & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString & (JobNo + 1)
                    BJobNo = "J" & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString & (JobNo + 1)
                End If
                For k As Integer = 0 To Dtcatname.Rows.Count - 1
                    Dim drMelIss() As DataRow
                    drMelIss = dtt.Select("CHECK = TRUE AND TRANSFERTYPE='TO DEALER' AND SMITH='" & DtSmith.Rows(i).Item("SMITH").ToString & "' AND CATNAME='" & Dtcatname.Rows(k).Item("CATNAME").ToString & "'")
                    If drMelIss.Length > 0 Then
                        Dim catCode As String = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & Dtcatname.Rows(k).Item("CATNAME").ToString & "'", , , tran)
                        Dim Metalid As String = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & Dtcatname.Rows(k).Item("CATNAME").ToString & "'", , , tran)
                        Dim metal As String = objGPack.GetSqlValue("SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID='" & Metalid & "'", , "", tran)
                        Dim mAccode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & DtSmith.Rows(i).Item("SMITH").ToString & "'", , , tran)
                        Dim wast As Double = Nothing
                        Dim wastPer As Double = Nothing
                        Dim alloy As Double = Nothing
                        Dim type As String = objGPack.GetSqlValue("SELECT METALTYPE FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & Dtcatname.Rows(k).Item("CATNAME").ToString & "')", , , tran)

                        Dim MTotPcs As Decimal = dtt.Compute("SUM(PCS)", "TRANSFERTYPE='TO DEALER' AND CATNAME='" & Dtcatname.Rows(k).Item("CATNAME").ToString & "' AND SMITH='" & DtSmith.Rows(i).Item("SMITH").ToString & "'")
                        Dim MTotWeight As Decimal = dtt.Compute("SUM(GRSWT)", "TRANSFERTYPE='TO DEALER' AND CATNAME='" & Dtcatname.Rows(k).Item("CATNAME").ToString & "' AND SMITH='" & DtSmith.Rows(i).Item("SMITH").ToString & "'")
                        Dim MTotcNetwt As Decimal = dtt.Compute("SUM(CNETWT)", "TRANSFERTYPE='TO DEALER' AND CATNAME='" & Dtcatname.Rows(k).Item("CATNAME").ToString & "' AND SMITH='" & DtSmith.Rows(i).Item("SMITH").ToString & "'")
                        Dim MTotNetwt As Decimal = dtt.Compute("SUM(NETWT)", "TRANSFERTYPE='TO DEALER' AND CATNAME='" & Dtcatname.Rows(k).Item("CATNAME").ToString & "' AND SMITH='" & DtSmith.Rows(i).Item("SMITH").ToString & "'")
                        Dim MTotPurewt As Decimal = dtt.Compute("SUM(PUREWT)", "TRANSFERTYPE='TO DEALER' AND CATNAME='" & Dtcatname.Rows(k).Item("CATNAME").ToString & "' AND SMITH='" & DtSmith.Rows(i).Item("SMITH").ToString & "'")
                        Dim MtotLesswt As Double = dtt.Compute("SUM(DUSTWT)", "TRANSFERTYPE='TO DEALER' AND CATNAME='" & Dtcatname.Rows(k).Item("CATNAME").ToString & "' AND SMITH='" & DtSmith.Rows(i).Item("SMITH").ToString & "'")
                        Dim MTotValue As Double = 0 'dtt.Compute("SUM(AMOUNT)", "TRANSFERTYPE='TO DEALER' AND CATNAME='" & Dtcatname.Rows(k).Item("CATNAME").ToString & "' AND SMITH='" & DtSmith.Rows(i).Item("SMITH").ToString & "'")
                        Dim avgtouch As Double = Math.Round((IIf(MTotPurewt <> 0, MTotPurewt, 1) / IIf(MTotNetwt <> 0, MTotNetwt, 1)) * 100, 2)
                        strSql = " INSERT INTO " & cnStockDb & "..ISSUE"
                        strSql += vbCrLf + " ("
                        strSql += vbCrLf + " SNO,TRANNO,TRANDATE,TRANTYPE,PCS,GRSWT,NETWT,LESSWT,PUREWT,TAGNO,ITEMID,SUBITEMID"
                        strSql += vbCrLf + " ,WASTPER,WASTAGE,MCGRM,MCHARGE,AMOUNT,RATE,BOARDRATE,SALEMODE,GRSNET,TRANSTATUS"
                        strSql += vbCrLf + " ,REFNO,REFDATE,COSTID,COMPANYID,FLAG,EMPID,TAGGRSWT,TAGNETWT,TAGRATEID,TAGSVALUE"
                        strSql += vbCrLf + " ,TAGDESIGNER,ITEMCTRID,ITEMTYPEID,PURITY,TABLECODE,INCENTIVE,WEIGHTUNIT,CATCODE"
                        strSql += vbCrLf + " ,OCATCODE,ACCODE,ALLOY,BATCHNO,REMARK1,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID"
                        strSql += vbCrLf + " ,DISCOUNT,RUNNO,CASHID,TAX,STNAMT,MISCAMT,METALID,STONEUNIT,APPVER,BAGNO"
                        strSql += vbCrLf + " ,TOUCH,JOBNO)"
                        strSql += vbCrLf + " VALUES("
                        strSql += vbCrLf + " '" & GetNewSno(TranSnoType.ISSUECODE, tran) & "'" ''SNO
                        strSql += vbCrLf + " ," & tranno & "" 'TRANNO
                        strSql += vbCrLf + " ,'" & BillDate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
                        strSql += vbCrLf + " ,'IIS'" 'TRANTYPE
                        strSql += vbCrLf + " ," & MTotPcs & "" 'PCS
                        strSql += vbCrLf + " ," & MTotWeight & "" 'GRSWT
                        strSql += vbCrLf + " ," & MTotcNetwt & "" 'NETWT
                        strSql += vbCrLf + " ," & MtotLesswt & "" 'LESSWT
                        strSql += vbCrLf + " ," & MTotPurewt & "" 'PUREWT '0
                        strSql += vbCrLf + " ,''" 'TAGNO
                        strSql += vbCrLf + " ,0" 'ITEMID
                        strSql += vbCrLf + " ,0" 'SUBITEMID
                        strSql += vbCrLf + " ," & wastPer & "" 'WASTPER
                        strSql += vbCrLf + " ," & wast & "" 'WASTAGE
                        strSql += vbCrLf + " ,0" 'MCGRM
                        strSql += vbCrLf + " ,0" 'MCHARGE
                        strSql += vbCrLf + " ," & MTotValue & "" 'AMOUNT
                        strSql += vbCrLf + " ," & 0 & "" 'RATE
                        strSql += vbCrLf + " ," & 0 & "" 'BOARDRATE
                        strSql += vbCrLf + " ,''" 'SALEMODE
                        strSql += vbCrLf + " ,'N'" 'GRSNET
                        strSql += vbCrLf + " ,''" 'TRANSTATUS ''
                        strSql += vbCrLf + " ,''" 'REFNO ''
                        strSql += vbCrLf + " ,NULL" 'REFDATE NULL
                        strSql += vbCrLf + " ,'" & cnCostId & "'" 'COSTID 
                        strSql += vbCrLf + " ,'" & strCompanyId & "'" 'COMPANYID
                        Select Case type
                            Case "O"
                                strSql += vbCrLf + " ,'O'" 'FLAG 
                            Case "M"
                                strSql += vbCrLf + " ,'M'" 'FLAG 
                            Case Else
                                strSql += vbCrLf + " ,'T'" 'FLAG 
                        End Select
                        strSql += vbCrLf + " ,0" 'EMPID
                        strSql += vbCrLf + " ,0" 'TAGGRSWT
                        strSql += vbCrLf + " ,0" 'TAGNETWT
                        strSql += vbCrLf + " ,0" 'TAGRATEID
                        strSql += vbCrLf + " ,0" 'TAGSVALUE
                        strSql += vbCrLf + " ,''" 'TAGDESIGNER  
                        strSql += vbCrLf + " ,0" 'ITEMCTRID
                        strSql += vbCrLf + " ,0" 'ITEMTYPEID
                        strSql += vbCrLf + " ," & 0 & "" 'PURITY
                        strSql += vbCrLf + " ,''" 'TABLECODE
                        strSql += vbCrLf + " ,''" 'INCENTIVE
                        strSql += vbCrLf + " ,''" 'WEIGHTUNIT
                        strSql += vbCrLf + " ,'" & catCode & "'" 'CATCODE
                        strSql += vbCrLf + " ,''" 'OCATCODE
                        strSql += vbCrLf + " ,'" & mAccode & "'" 'ACCODE
                        strSql += vbCrLf + " ," & alloy & "" 'ALLOY
                        strSql += vbCrLf + " ,'" & Batchno & "'" 'BATCHNO
                        strSql += vbCrLf + " ,''" 'REMARK1
                        strSql += vbCrLf + " ,''" 'REMARK2
                        strSql += vbCrLf + " ,'" & userId & "'" 'USERID
                        strSql += vbCrLf + " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                        strSql += vbCrLf + " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                        strSql += vbCrLf + " ,'" & systemId & "'" 'SYSTEMID
                        strSql += vbCrLf + " ,0" 'DISCOUNT
                        strSql += vbCrLf + " ,''" 'RUNNO
                        strSql += vbCrLf + " ,''" 'CASHID
                        strSql += vbCrLf + " ,0" 'TAX
                        strSql += vbCrLf + " ,0" 'STONEAMT
                        strSql += vbCrLf + " ,0" 'MISCAMT
                        strSql += vbCrLf + " ,'" & Metalid & "'" 'METALID
                        strSql += vbCrLf + " ,'" & objGPack.GetSqlValue("SELECT CASE WHEN DIASTNTYPE = 'T' THEN 'G' WHEN DIASTNTYPE = 'D' THEN 'C' WHEN DIASTNTYPE = 'P' THEN 'C' ELSE '' END AS STONEUNIT FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & catCode & "'", , , tran) & "'" 'STONEUNIT
                        strSql += vbCrLf + " ,'" & VERSION & "'" 'APPVER
                        strSql += vbCrLf + " ,'" & BagNo & "'" 'BAGNO
                        strSql += vbCrLf + " ," & avgtouch & "" 'BAGNO
                        strSql += vbCrLf + " ,'" & mJobNo & "'" 'BAGNO
                        strSql += vbCrLf + " )"
                        'ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                        If CENTR_DB_BR Then
                            cmd = New OleDbCommand(strSql, cn, tran)
                            cmd.ExecuteNonQuery()
                        Else
                            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                        End If

                        'strSql = " INSERT INTO " & cnStockDb & "..MELTINGDETAIL"
                        'strSql += vbcrlf + " (TRANNO,TRANDATE,RECISS,TRANTYPE,ACCODE,CATCODE,METALID,PCS,GRSWT,NETWT,RATE,AMOUNT"
                        'strSql += vbcrlf + " ,BAGNO,BATCHNO,APPVER,COMPANYID,USERID,UPDATED"
                        'strSql += vbcrlf + " ,UPTIME,CANCEL)"
                        'strSql += vbcrlf + " VALUES"
                        'strSql += vbcrlf + " ("
                        'strSql += vbcrlf + " " & tranno & "" 'TRANNO
                        'strSql += vbcrlf + " ,'" & BillDate.ToString("yyyy-MM-dd") & "'" 'trandate
                        'strSql += vbcrlf + " ,'I','MI'" 'RECISS,TRANTYPE
                        'strSql += vbcrlf + " ,'" & mAccode & "'" 'accode
                        'strSql += vbcrlf + " ,'" & catCode & "'" 'CATCODE.
                        'strSql += vbcrlf + " ,'" & Metalid & "'" 'METAL.
                        'strSql += vbcrlf + " ," & MTotPcs & "" 'PCS    
                        'strSql += vbcrlf + " ," & MTotWeight & "" 'NETWT
                        'strSql += vbcrlf + " ," & MTotcNetwt & "" 'NETWT
                        'strSql += vbcrlf + " ," & 0 & "" 'RATE
                        'strSql += vbcrlf + " ," & MTotValue & "" 'AMOUNT
                        'strSql += vbcrlf + " ,'" & BagNo & "'" 'BAGNO
                        'strSql += vbcrlf + " ,'" & Batchno & "'" 'BATCHNO
                        'strSql += vbcrlf + " ,'" & VERSION & "'" 'APPVER
                        'strSql += vbcrlf + " ,'" & strCompanyId & "'" 'COMPANYID
                        'strSql += vbcrlf + " ,'" & userId & "'" 'USERID
                        'strSql += vbcrlf + " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                        'strSql += vbcrlf + " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                        'strSql += vbcrlf + " ,''" 'CANCEL
                        'strSql += vbcrlf + " )"
                        'ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

                        IssNo = tranno
                    End If
                Next
                If _JobNoEnable = True Then
                    strSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & JobNo + 1 & "' "
                    strSql += vbCrLf + " WHERE CTLID = 'GEN-JOBNO' AND COMPANYID = '" & strCompanyId & "'"
                    strSql += vbCrLf + " AND CONVERT(INT,CTLTEXT) = " & JobNo & ""
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()
                End If

            Next
        End If

    End Sub
    Private Sub InsertIssueReceipt(ByVal Trantype As String, ByVal dtr() As DataRow)

        If Batchno = "" Then Batchno = GetNewBatchnoNew(cnCostId, BillDate, cn, tran)

        strSql = " IF (SELECT COUNT(*) FROM " & cnAdminDb & "..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BILLNO')> 0"
        strSql += vbCrLf + " DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "BILLNO"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        Dim billcontrolid As String = "GEN-SM-INTISS"
        If Trantype = "IIN" Then billcontrolid = "GEN-SM-INTISS"
        If Trantype = "RIN" Then billcontrolid = "GEN-SM-INTREC"
        strSql = "SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL"
        strSql += vbCrLf + " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
        If strBCostid <> Nothing Then strSql += vbCrLf + " AND ISNULL(COSTID,'') ='" & strBCostid & "'"
        If UCase(objGPack.GetSqlValue(strSql, , , tran)) <> "Y" Then
            billcontrolid = "GEN-STKREFNO"
        End If
        Dim NEWBILLNO As Integer
        strSql = "SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = '" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
        If strBCostid <> Nothing Then strSql += vbCrLf + " AND ISNULL(COSTID,'') ='" & strBCostid & "'"
        NEWBILLNO = Val(objGPack.GetSqlValue(strSql, , , tran)) + 1
GenerateNewBillNo:
        If Trantype = "IIN" Then
            strSql = vbCrLf + " SELECT TOP 1 SNO FROM " & cnStockDb & "..ISSUE WHERE TRANTYPE = 'IIN' AND COSTID = '" & cnCostId & "' AND TRANNO = " & NEWBILLNO
        Else
            strSql = vbCrLf + " SELECT TOP 1 SNO FROM " & cnStockDb & "..RECEIPT WHERE TRANTYPE = 'RIN' AND COSTID = '" & cnCostId & "' AND TRANNO = " & NEWBILLNO
        End If
        If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
            NEWBILLNO = NEWBILLNO + 1
            GoTo GenerateNewBillNo
        End If
        strSql = "UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & NEWBILLNO.ToString & "'"
        strSql += vbCrLf + " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
        If strBCostid <> Nothing Then strSql += vbCrLf + " AND ISNULL(COSTID,'') ='" & strBCostid & "'"
        cmd = New OleDbCommand(strSql, cn, tran)
        If cmd.ExecuteNonQuery() = 0 Then
            If strBCostid <> Nothing Then MsgBox("No bill control for this cost id " & strBCostid) : Exit Sub
            GoTo GenerateNewBillNo
        End If

        For Each row As DataRow In dtr
            Dim issSno As String = GetNewSno(IIf(Trantype = "IIN", TranSnoType.ISSUECODE, TranSnoType.RECEIPTCODE), tran)
            strSql = "INSERT INTO " & cnStockDb & ".." & IIf(Trantype = "IIN", "ISSUE", "RECEIPT") & "("
            strSql += vbCrLf + " SNO,TRANNO,TRANDATE,TRANTYPE,PCS"
            strSql += vbCrLf + " ,GRSWT,NETWT,LESSWT,PUREWT"
            strSql += vbCrLf + " ,TAGNO,ITEMID,SUBITEMID,WASTPER"
            strSql += vbCrLf + " ,WASTAGE,MCGRM,MCHARGE,AMOUNT"
            strSql += vbCrLf + " ,RATE,BOARDRATE,SALEMODE,GRSNET"
            strSql += vbCrLf + " ,TRANSTATUS,REFNO,REFDATE,COSTID"
            strSql += vbCrLf + " ,COMPANYID,FLAG,EMPID,TAGGRSWT"
            strSql += vbCrLf + " ,TAGNETWT,TAGRATEID,TAGSVALUE,TAGDESIGNER"
            strSql += vbCrLf + " ,ITEMCTRID,ITEMTYPEID,PURITY,TABLECODE"
            strSql += vbCrLf + " ,INCENTIVE,WEIGHTUNIT,CATCODE,OCATCODE"
            strSql += vbCrLf + " ,ACCODE,ALLOY,BATCHNO,REMARK1"
            strSql += vbCrLf + " ,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,DISCOUNT,RUNNO,CASHID,TAX"
            strSql += vbCrLf + " ,STNAMT,MISCAMT,METALID,STONEUNIT,APPVER,BAGNO"
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " VALUES("
            strSql += vbCrLf + " '" & issSno & "'" ''SNO
            strSql += vbCrLf + " ," & NEWBILLNO & "" 'TRANNO
            strSql += vbCrLf + " ,'" & BillDate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
            strSql += vbCrLf + " ,'" & Trantype & "'" 'TRANTYPE
            strSql += vbCrLf + " ," & Val(row.Item("PCS").ToString) 'PCS
            strSql += vbCrLf + " ," & Val(row.Item("GRSWT").ToString) & "" 'GRSWT
            strSql += vbCrLf + " ," & Val(row.Item("NETWT").ToString) & "" 'NETWT
            strSql += vbCrLf + " ,0" 'LESSWT
            strSql += vbCrLf + " ,0" '& Val(.Cells("PUREWT").Value.ToString) & "" 'PUREWT '0
            strSql += vbCrLf + " ,''" 'TAGNO
            strSql += vbCrLf + " ,0" 'ITEMID
            strSql += vbCrLf + " ,0" 'SUBITEMID
            strSql += vbCrLf + " ,0" 'WASTPER
            strSql += vbCrLf + " ,0" 'WASTAGE
            strSql += vbCrLf + " ,0" 'MCGRM
            strSql += vbCrLf + " ,0" 'MCHARGE
            strSql += vbCrLf + " ,0" 'AMOUNT
            strSql += vbCrLf + " ,0" ' & Val(.Cells("RATE").Value.ToString) & "" 'RATE
            strSql += vbCrLf + " ,0" '& Val(.Cells("RATE").Value.ToString) & "" 'BOARDRATE
            strSql += vbCrLf + " ,''" 'SALEMODE
            strSql += vbCrLf + " ,'N'" 'GRSNET
            strSql += vbCrLf + " ,''" 'TRANSTATUS ''
            strSql += vbCrLf + " ,''" 'REFNO ''
            strSql += vbCrLf + " ,NULL" 'REFDATE NULL
            strSql += vbCrLf + " ,'" & cnCostId & "'" 'COSTID 
            strSql += vbCrLf + " ,'" & strCompanyId & "'" 'COMPANYID
            strSql += vbCrLf + " ,'O'" 'FLAG 
            strSql += vbCrLf + " ,0" 'EMPID
            strSql += vbCrLf + " ,0" 'TAGGRSWT
            strSql += vbCrLf + " ,0" 'TAGNETWT
            strSql += vbCrLf + " ,0" 'TAGRATEID
            strSql += vbCrLf + " ,0" 'TAGSVALUE
            strSql += vbCrLf + " ,''" 'TAGDESIGNER  
            strSql += vbCrLf + " ,0" 'ITEMCTRID
            strSql += vbCrLf + " ,0" 'ITEMTYPEID
            strSql += vbCrLf + " ,0" 'PURITY
            strSql += vbCrLf + " ,''" 'TABLECODE
            strSql += vbCrLf + " ,''" 'INCENTIVE
            strSql += vbCrLf + " ,''" 'WEIGHTUNIT
            strSql += vbCrLf + " ,'" & row.Item("catCode").ToString & "'" 'CATCODE
            strSql += vbCrLf + " ,'" & row.Item("catCode").ToString & "'" 'OCATCODE
            strSql += vbCrLf + " ,'STKTRAN'" 'ACCODE
            strSql += vbCrLf + " ,0" 'ALLOY
            strSql += vbCrLf + " ,'" & Batchno & "'" 'BATCHNO
            strSql += vbCrLf + " ,'" & row.Item("REMARK1").ToString & "'" 'REMARK1
            strSql += vbCrLf + " ,''" 'REMARK2
            strSql += vbCrLf + " ,'" & userId & "'" 'USERID
            strSql += vbCrLf + " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
            strSql += vbCrLf + " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
            strSql += vbCrLf + " ,'" & systemId & "'" 'SYSTEMID
            strSql += vbCrLf + " ,0" 'DISCOUNT
            strSql += vbCrLf + " ,''" 'RUNNO
            strSql += vbCrLf + " ,''" 'CASHID
            strSql += vbCrLf + " ,0" 'TAX
            strSql += vbCrLf + " ,0" 'STONEAMT
            strSql += vbCrLf + " ,0" 'MISCAMT
            strSql += vbCrLf + " ,'" & row.Item("METALID").ToString & "'" 'METALID
            strSql += vbCrLf + " ,''" 'STONEUNIT
            strSql += vbCrLf + " ,'" & VERSION & "'" 'APPVER
            strSql += vbCrLf + " ,''" 'BAGNO
            strSql += vbCrLf + " )"
            'ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            If CENTR_DB_BR Then
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
            Else
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            End If
        Next
    End Sub

    Private Sub InsertReceipt_Post()
        If Batchno = "" Then Batchno = GetNewBatchnoNew(cnCostId, BillDate, cn, tran)
        Dim Trantype As String = "PU"
        strSql = " IF (SELECT COUNT(*) FROM " & cnAdminDb & "..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BILLNO')> 0"
        strSql += vbCrLf + " DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "BILLNO"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        Dim billcontrolid As String = "GEN-PURBILLNO"
        strSql = "SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL"
        strSql += vbCrLf + " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
        If strBCostid <> Nothing Then strSql += vbCrLf + " AND ISNULL(COSTID,'') ='" & strBCostid & "'"
        Dim NEWBILLNO As Integer
        strSql = "SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = '" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
        If strBCostid <> Nothing Then strSql += vbCrLf + " AND ISNULL(COSTID,'') ='" & strBCostid & "'"
        NEWBILLNO = Val(objGPack.GetSqlValue(strSql, , , tran)) + 1
GenerateNewBillNo:
        If Trantype = "IIN" Then
            strSql = vbCrLf + " SELECT TOP 1 SNO FROM " & cnStockDb & "..ISSUE WHERE TRANTYPE = 'IIN' AND COSTID = '" & cnCostId & "' AND TRANNO = " & NEWBILLNO
        Else
            strSql = vbCrLf + " SELECT TOP 1 SNO FROM " & cnStockDb & "..RECEIPT WHERE TRANTYPE = 'RIN' AND COSTID = '" & cnCostId & "' AND TRANNO = " & NEWBILLNO
        End If
        If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
            NEWBILLNO = NEWBILLNO + 1
            GoTo GenerateNewBillNo
        End If
        strSql = "UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & NEWBILLNO.ToString & "'"
        strSql += vbCrLf + " WHERE CTLID ='" & billcontrolid & "' AND COMPANYID = '" & strCompanyId & "'"
        If strBCostid <> Nothing Then strSql += vbCrLf + " AND ISNULL(COSTID,'') ='" & strBCostid & "'"
        cmd = New OleDbCommand(strSql, cn, tran)
        If cmd.ExecuteNonQuery() = 0 Then
            If strBCostid <> Nothing Then MsgBox("No bill control for this cost id " & strBCostid) : Exit Sub
            GoTo GenerateNewBillNo
        End If

        strSql = vbCrLf + "  UPDATE A SET MELT_RETAG='P'"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT AS A "
        strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..CATEGORY C ON A.CATCODE = C.CATCODE"
        strSql += vbCrLf + "  WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND A.TRANTYPE='PU' AND ISNULL(CANCEL,'')=''"
        strSql += vbCrLf + "  AND COMPANYID IN (SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COMPANYID') "
        If cmbOpenMetal.Text <> "ALL" Then strSql += vbCrLf + " AND C.METALID = ISNULL((SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME= '" & cmbOpenMetal.Text & "'),'')"
        If CmbCategory.Text <> "ALL" And CmbCategory.Text <> "" Then strSql += vbCrLf + " AND C.CATNAME= '" & CmbCategory.Text & "'"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()

        For Each row As DataGridViewRow In gridView_OWN.Rows
            strSql = "SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & row.Cells("CATNAME").Value.ToString & "' "
            Dim strCatCode As String = objGPack.GetSqlValue(strSql, , , tran)

            strSql = "SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'TAB_COMPANY')"
            Dim str2CompId As String = objGPack.GetSqlValue(strSql, , , tran)
            If str2CompId = "" Then Exit Sub

            Dim issSno As String = GetNewSno(IIf(Trantype = "PU", TranSnoType.ISSUECODE, TranSnoType.RECEIPTCODE), tran)
            strSql = "INSERT INTO " & cnStockDb & "..RECEIPT ("
            strSql += vbCrLf + " SNO,TRANNO,TRANDATE,TRANTYPE,PCS"
            strSql += vbCrLf + " ,GRSWT,NETWT,LESSWT,PUREWT"
            strSql += vbCrLf + " ,TAGNO,ITEMID,SUBITEMID,WASTPER"
            strSql += vbCrLf + " ,WASTAGE,MCGRM,MCHARGE,AMOUNT"
            strSql += vbCrLf + " ,RATE,BOARDRATE,SALEMODE,GRSNET"
            strSql += vbCrLf + " ,TRANSTATUS,REFNO,REFDATE,COSTID"
            strSql += vbCrLf + " ,COMPANYID,FLAG,EMPID,TAGGRSWT"
            strSql += vbCrLf + " ,TAGNETWT,TAGRATEID,TAGSVALUE,TAGDESIGNER"
            strSql += vbCrLf + " ,ITEMCTRID,ITEMTYPEID,PURITY,TABLECODE"
            strSql += vbCrLf + " ,INCENTIVE,WEIGHTUNIT,CATCODE,OCATCODE"
            strSql += vbCrLf + " ,ACCODE,ALLOY,BATCHNO,REMARK1"
            strSql += vbCrLf + " ,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,DISCOUNT,RUNNO,CASHID,TAX"
            strSql += vbCrLf + " ,STNAMT,MISCAMT,METALID,STONEUNIT,APPVER,BAGNO"
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " VALUES("
            strSql += vbCrLf + " '" & issSno & "'" ''SNO
            strSql += vbCrLf + " ," & NEWBILLNO & "" 'TRANNO
            strSql += vbCrLf + " ,'" & BillDate.ToString("yyyy-MM-dd") & "'" 'TRANDATE
            strSql += vbCrLf + " ,'" & Trantype & "'" 'TRANTYPE
            strSql += vbCrLf + " ," & Val(row.Cells("PCS").Value.ToString) 'PCS
            strSql += vbCrLf + " ," & Val(row.Cells("GRSWT").Value.ToString) & "" 'GRSWT
            strSql += vbCrLf + " ," & Val(row.Cells("NETWT").Value.ToString) & "" 'NETWT
            strSql += vbCrLf + " ,0" 'LESSWT
            strSql += vbCrLf + " ,0" '& Val(.Cells("PUREWT").Value.ToString) & "" 'PUREWT '0
            strSql += vbCrLf + " ,''" 'TAGNO
            strSql += vbCrLf + " ,0" 'ITEMID
            strSql += vbCrLf + " ,0" 'SUBITEMID
            strSql += vbCrLf + " ,0" 'WASTPER
            strSql += vbCrLf + " ,0" 'WASTAGE
            strSql += vbCrLf + " ,0" 'MCGRM
            strSql += vbCrLf + " ,0" 'MCHARGE
            strSql += vbCrLf + " ,0" 'AMOUNT
            strSql += vbCrLf + " ,0" ' & Val(.Cells("RATE").Value.ToString) & "" 'RATE
            strSql += vbCrLf + " ,0" '& Val(.Cells("RATE").Value.ToString) & "" 'BOARDRATE
            strSql += vbCrLf + " ,''" 'SALEMODE
            strSql += vbCrLf + " ,'N'" 'GRSNET
            strSql += vbCrLf + " ,''" 'TRANSTATUS ''
            strSql += vbCrLf + " ,''" 'REFNO ''
            strSql += vbCrLf + " ,NULL" 'REFDATE NULL
            strSql += vbCrLf + " ,'" & cnCostId & "'" 'COSTID 
            strSql += vbCrLf + " ,'" & strCompanyId & "'" 'COMPANYID
            strSql += vbCrLf + " ,'O'" 'FLAG 
            strSql += vbCrLf + " ,0" 'EMPID
            strSql += vbCrLf + " ,0" 'TAGGRSWT
            strSql += vbCrLf + " ,0" 'TAGNETWT
            strSql += vbCrLf + " ,0" 'TAGRATEID
            strSql += vbCrLf + " ,0" 'TAGSVALUE
            strSql += vbCrLf + " ,''" 'TAGDESIGNER  
            strSql += vbCrLf + " ,0" 'ITEMCTRID
            strSql += vbCrLf + " ,0" 'ITEMTYPEID
            strSql += vbCrLf + " ,0" 'PURITY
            strSql += vbCrLf + " ,''" 'TABLECODE
            strSql += vbCrLf + " ,''" 'INCENTIVE
            strSql += vbCrLf + " ,''" 'WEIGHTUNIT
            strSql += vbCrLf + " ,'" & strCatCode & "'" 'CATCODE
            strSql += vbCrLf + " ,'" & strCatCode & "'" 'OCATCODE
            strSql += vbCrLf + " ,'STKTRAN'" 'ACCODE
            strSql += vbCrLf + " ,0" 'ALLOY
            strSql += vbCrLf + " ,'" & Batchno & "'" 'BATCHNO
            strSql += vbCrLf + " ,'TRANSFERED ENTRY'" 'REMARK1
            strSql += vbCrLf + " ,''" 'REMARK2
            strSql += vbCrLf + " ,'" & userId & "'" 'USERID
            strSql += vbCrLf + " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
            strSql += vbCrLf + " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
            strSql += vbCrLf + " ,'" & systemId & "'" 'SYSTEMID
            strSql += vbCrLf + " ,0" 'DISCOUNT
            strSql += vbCrLf + " ,''" 'RUNNO
            strSql += vbCrLf + " ,''" 'CASHID
            strSql += vbCrLf + " ,0" 'TAX
            strSql += vbCrLf + " ,0" 'STONEAMT
            strSql += vbCrLf + " ,0" 'MISCAMT
            strSql += vbCrLf + " ,'" & row.Cells("METALID").Value.ToString & "'" 'METALID
            strSql += vbCrLf + " ,''" 'STONEUNIT
            strSql += vbCrLf + " ,'" & VERSION & "'" 'APPVER
            strSql += vbCrLf + " ,''" 'BAGNO
            strSql += vbCrLf + " )"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
        Next
    End Sub

    Private Sub LOTISSUES(ByVal drow() As DataRow)
        'ByVal Tranno As Integer, ByVal Recsno As String, ByVal accode As String, ByVal itemid As Integer, ByVal subitemid As Integer, ByVal cnt As Integer, ByVal Pcs As Integer, ByVal Grswt As Decimal, ByVal Netwt As Decimal, ByVal Touch As Decimal, ByVal Wastage As Decimal, ByVal Mcharge As Decimal, ByVal Isediting As Boolean)
        Dim lotSno As String
        Dim LotNo As Integer
        Dim Defcounterid As Integer = Val(GetAdmindbSoftValue("PENDTRF_CTRID", "0").ToString)

        Try
            tran = Nothing
            tran = cn.BeginTransaction
GENLOTNO:
            strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'LOTNO'"
            LotNo = Val(objGPack.GetSqlValue(strSql, , , tran))
            strSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT = '" & LotNo + 1 & "' "
            strSql += vbCrLf + " WHERE CTLID = 'LOTNO' AND CTLTEXT = " & LotNo & ""
            cmd = New OleDbCommand(strSql, cn, tran)
            If cmd.ExecuteNonQuery() = 0 Then
                GoTo GENLOTNO
            End If
            LotNo += 1

            Dim entord As Integer = 0
            Dim KEY As String = ""
            For Each row As DataRow In drow
                entord += 1
                Dim itemid As Integer = 0
                If row!itemname.ToString <> "" Then
                    itemid = Val(objGPack.GetSqlValue("select itemid from " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & row!itemname.ToString & "'", , , tran).ToString)
                Else
                    If row.Item(2).ToString = "PARTLY SALES" Then
                        KEY = "PARTLY-"
                    ElseIf row.Item(2).ToString = "SALES RETURN" Then
                        KEY = "RETURN-"
                    ElseIf row.Item(2).ToString = "PURCHASE" Then
                        KEY = "PURCHASE-"
                    End If
                    itemid = Val(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = '" & KEY + row!metalId & "'", , , tran))
                End If
                Dim subitemid As Integer = 0
                Dim designerid As Integer = 0
                If row.Item(2).ToString = "PARTLY SALES" Or row.Item(1).ToString = "MISC ISSUE" Then
                    Dim diss As DataRow = GetSqlRow("select subitemid,tagdesigner from " & cnStockDb & "..issue where sno = '" & row!sno.ToString & "'", cn, tran)
                    subitemid = Val(diss(0).ToString)
                    designerid = Val(diss(1).ToString)
                End If
                lotSno = GetNewSno(TranSnoType.ITEMLOTCODE, tran, "GET_ADMINSNO_TRAN") '  GetWSno(TranSnoType.ITEMLOTCODE, Tran, CnStockdb)
                strSql = "select isnull(STOCKTYPE,'') STOCKTYPE,isnull(NOOFPIECE,0) noofpiece,isnull(DEFAULTCOUNTER,0) DEFCOUNTER,VALUEADDEDTYPE from " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & itemid
                Dim dritem As DataRow = GetSqlRow(strSql, cn, tran)

                Dim stocktype As String = dritem.Item(0).ToString
                Dim noOfTag As Integer = dritem.Item(1).ToString
                Dim itemCounterId As Integer = Val(dritem.Item(2).ToString)
                Dim VALUEADDEDTYPE As String = dritem.Item(3).ToString

                If Defcounterid <> 0 Then itemCounterId = Defcounterid
                'strSql = "select top 1 designerid from " & cnAdminDb & "..DESIGNER WHERE ACCODE = '" & accode & "'"
                'Dim dridesg As DataRow = GetSqlRow(strSql, cn, tran)
                'If Not (dridesg Is Nothing) Then DesignerId = Val(dridesg.Item(0).ToString)
                Dim entryType As String = "R"

                Dim mwastper As Decimal = 0

                Dim mmcgrm As Decimal = 0


                strSql = " INSERT INTO " & cnAdminDb & "..ITEMLOT "
                strSql += vbCrLf + " ("
                strSql += vbCrLf + " SNO,ENTRYTYPE,LOTDATE,LOTNO,DESIGNERID,TRANINVNO,"
                strSql += vbCrLf + " BILLNO,COSTID,ENTRYORDER,ORDREPNO,ORDENTRYORDER,"
                strSql += vbCrLf + " ITEMID,SUBITEMID,PCS,GRSWT,STNPCS,STNWT,STNUNIT,"
                strSql += vbCrLf + " DIAPCS,DIAWT,NETWT,NOOFTAG,RATE,ITEMCTRID,WMCTYPE,"
                strSql += vbCrLf + " BULKLOT,MULTIPLETAGS,NARRATION,FINERATE,TUCH,"
                strSql += vbCrLf + " WASTPER,MCGRM,OTHCHARGE,STARTTAGNO,ENDTAGNO,CURTAGNO,"
                strSql += vbCrLf + " COMPANYID,CPCS,CGRSWT,COMPLETED,CANCEL,"
                strSql += vbCrLf + " ACCESSING,USERID,UPDATED,"
                strSql += vbCrLf + " UPTIME,SYSTEMID,APPVER,ITEMTYPEID)VALUES("
                strSql += vbCrLf + " '" & lotSno & "'" 'SNO
                strSql += vbCrLf + " ,'" & entryType & "'" 'ENTRYTYPE
                strSql += vbCrLf + " ,'" & BillDate.ToString("yyyy-MM-dd") & "'" 'LOTDATE 'DateTime.Parse(RoIssue(CNT).ITEM("LOTDATE"), ukCultureInfo.DateTimeFormat).ToString("yyyy-MM-dd")
                strSql += vbCrLf + " ," & LotNo & "" 'LOTNO
                strSql += vbCrLf + " ," & designerid & "" 'DESIGNERID
                strSql += vbCrLf + " ,''" 'TRANINVNO
                strSql += vbCrLf + " ,''" 'BILLNO
                strSql += vbCrLf + " ,'" & row!CostId & "'" 'COSTID
                strSql += vbCrLf + " ," & entord & "" 'ENTRYORDER
                strSql += vbCrLf + " ,''" 'ORDREPNO
                strSql += vbCrLf + " ,0" 'ORDENTRYORDER
                strSql += vbCrLf + " ," & itemid & "" 'ITEMID
                strSql += vbCrLf + " ,0"
                strSql += vbCrLf + " ," & Val(row!pcs.ToString) & "" 'PCS
                strSql += vbCrLf + " ," & Val(row!Grswt.ToString) & "" 'GRSWT
                strSql += vbCrLf + " ,0" 'STNPCS
                strSql += vbCrLf + " ,0" 'STNWT
                strSql += vbCrLf + " ,'G'" 'STNUNIT
                strSql += vbCrLf + " ,0" 'DIAPCS
                strSql += vbCrLf + " ,0" 'DIAWT
                strSql += vbCrLf + " ," & Val(row!Netwt.ToString) & "" 'NETWT
                strSql += vbCrLf + " ," & IIf(noOfTag = 0, 1, noOfTag) & "" 'NOOFTAG
                strSql += vbCrLf + " ,0" 'RATE
                strSql += vbCrLf + " ," & itemCounterId & "" 'ITEMCTRID
                strSql += vbCrLf + " ,'" & VALUEADDEDTYPE & "'" 'WMCTYPE
                strSql += vbCrLf + " ,'N'" 'BULKLOT
                strSql += vbCrLf + " ,'N'" 'MULTIPLETAGS
                strSql += vbCrLf + " ,''" 'NARRATION
                strSql += vbCrLf + " ,0" 'FINERATE
                strSql += vbCrLf + " ,0"
                strSql += vbCrLf + " ," & mwastper & "" 'WASTPER
                strSql += vbCrLf + " ," & mmcgrm & "" 'MCGRM
                strSql += vbCrLf + " ,0" 'OTHCHARGE
                strSql += vbCrLf + " ,''" 'STARTTAGNO
                strSql += vbCrLf + " ,''" 'ENDTAGNO
                strSql += vbCrLf + " ,''" 'CURTAGNO
                strSql += vbCrLf + " ,'" & GetStockCompId() & "'" 'sCOMPANYID
                strSql += vbCrLf + " ,0" 'CPIECE
                strSql += vbCrLf + " ,0" 'CWEIGHT
                strSql += vbCrLf + " ,''" 'COMPLETED
                strSql += vbCrLf + " ,''" 'CANCEL
                strSql += vbCrLf + " ,''" 'ACCESSING
                strSql += vbCrLf + " ," & userId & "" 'USERID
                strSql += vbCrLf + " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                strSql += vbCrLf + " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                strSql += vbCrLf + " ,'" & systemId & "'" 'SYSTEMID
                strSql += vbCrLf + " ,'" & VERSION & "'" 'APPVER
                strSql += vbCrLf + " ,0" 'ITEMTYPEID"
                strSql += vbCrLf + " )"
                'ExecQuery(SyncMode.Stock, strSql, cn, tran, cnCostId)
                If CENTR_DB_BR Then
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()
                Else
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                End If

                strSql = " UPDATE " & cnStockDb & "..RECEIPT"
                strSql += vbCrLf + "  SET MELT_RETAG='L', BAGNO = '" & LotNo & "'"
                strSql += vbCrLf + "  WHERE BATCHNO ='" & row!batchno.ToString & "' AND SNO ='" & row!SNO.ToString & "'"
                strSql += vbCrLf + " UPDATE " & cnStockDb & "..ISSUE"
                strSql += vbCrLf + "  SET MELT_RETAG='L', BAGNO = '" & LotNo & "'"
                strSql += vbCrLf + "  WHERE BATCHNO ='" & row!batchno.ToString & "' AND SNO = '" & row!SNO.ToString & "'"
                'ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                If CENTR_DB_BR Then
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()
                Else
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                End If

            Next
            tran.Commit()
            tran = Nothing
            MsgBox("Lot No : " & LotNo & " Generated..")
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try


    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub frmPendingTransfer_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.L
                'btnLoad_Click(btnLoad, Nothing)
                'Exit Select
            Case Keys.F3
                btnNew_Click(btnNew, Nothing)
                Exit Select
            Case Keys.F2
                btnOpen_Click(btnOpen, Nothing)
                Exit Select
            Case Keys.F12
                btnExit_Click(btnExit, Nothing)
                Exit Select
            Case Keys.S
                If tabMain.SelectedTab.Name = page1.Name Then Exit Sub
                btnSearch_Click(btnSearch, Nothing)
                Exit Select
            Case Keys.Escape
                btnBack_Click(btnBack, Nothing)
                Exit Select
        End Select
    End Sub

    Private Sub frmPendingTransfer_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView_OWN.Focused Then Exit Sub
            If cmbTransferTo.Focused Then Exit Sub
            If txtGrid.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub funLoadcatstk()
        strSql = " SELECT '' METALID,'' TRANTYPE,''CATCODE,''PCS,''GRSWT,'' DUSTWT,''NETWT,'' TOUCH,''PUREWT,'' CNETWT"
        strSql += vbCrLf + " ,'' RATE,''AMOUNT,''REMARK1,''BATCHNO,''COSTID WHERE 1<>1"
        da = New OleDbDataAdapter(strSql, cn)
        dtCatstk = New DataTable
        dtCatstk.Columns.Add("SNO", GetType(Integer))
        dtCatstk.Columns("SNO").AutoIncrement = True
        dtCatstk.Columns("SNO").AutoIncrementSeed = 0
        dtCatstk.Columns("SNO").AutoIncrementStep = 1
        da.Fill(dtCatstk)
    End Sub
    Private Sub frmPendingTransfer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.page1.Left, Me.page1.Top, Me.page1.Width, Me.page1.Height))
        chkColumn.Name = "chkGridCheck"
        chkColumn.Width = 20
        chkAll.Text = ""
        gridView_OWN.Controls.Add(chkAll)
        chkAll.Location = New System.Drawing.Point(4, 4)
        chkAll.BringToFront()
        funLoadcatstk()
        strSql = "SELECT ''SNO,'' METALID,'' TRANTYPE,CONVERT(INT,NULL) PCS,CONVERT(NUMERIC(15,3),NULL)GRSWT"
        strSql += vbCrLf + ",CONVERT(NUMERIC(15,3),NULL)NETWT,CONVERT(NUMERIC(15,3),NULL)PUREWT"
        strSql += vbCrLf + ",CONVERT(NUMERIC(15,2),NULL)AMOUNT,CONVERT(VARCHAR(50),'')COUNTER WHERE 1<>1"
        da = New OleDbDataAdapter(strSql, cn)
        Dtitemnontag = New DataTable
        da.Fill(Dtitemnontag)

        If PSSR_CATPOST <> "" Then
            PSSR_CATPOSTING = PSSR_CATPOST.ToString.Split(",")
        End If

        ''load dtGridView
        strSql = " SELECT '' METALID,''TYPE,''DATE,''CATNAME,''TRANNO,'' ITEMNAME,CAST(NULL AS INTEGER) PCS"
        strSql += vbCrLf + " ,CAST(NULL AS DECIMAL(15,3)) GRSWT,CAST(NULL AS DECIMAL(15,3)) DUSTWT,CAST(NULL AS DECIMAL(15,3)) NETWT"
        strSql += vbCrLf + " ,CAST(NULL AS DECIMAL(10,2)) TOUCH,CAST(NULL AS DECIMAL(15,3)) PUREWT,CAST(NULL AS DECIMAL(15,3)) CNETWT"
        strSql += vbCrLf + " ,CAST(NULL AS DECIMAL(15,2))RATE,CAST(NULL AS DECIMAL(15,2))AMOUNT,''BATCHNO,''COSTID,''TRANDATE,'' SNO"
        strSql += vbCrLf + " ,'' SMITH,'' EMPNAME,'' COUNTER,CAST(NULL AS INTEGER) OPCS,CAST(NULL AS DECIMAL(15,3)) OGRSWT"
        strSql += vbCrLf + " ,CAST(NULL AS DECIMAL(15,3)) ONETWT,CAST(NULL AS DECIMAL(15,3)) OPUREWT,'' DESIGNERID,'' TABLECODE,CAST(NULL AS DECIMAL(15,3)) STNWT,CAST(NULL AS DECIMAL(15,3)) DIAWT,'' TDBNAME  "
        If PSSR_CATPOST <> "" Then
            strSql += vbCrLf + " ,''CATCODE,'' PSSRCHECK"
        End If
        strSql += vbCrLf + " WHERE 1<>1"
        da = New OleDbDataAdapter(strSql, cn)
        dtGridView.Columns.Add("CHECK", GetType(Boolean))
        dtGridView.Columns.Add("TRANSFERTYPE", GetType(String))
        dtGridView.Columns.Add("COLOR", GetType(String))
        da.Fill(dtGridView)
        'gridView.Columns.Add(chkColumn)
        gridView_OWN.DataSource = dtGridView
        With gridView_OWN
            '.Columns("chkGridCheck").HeaderText = ""
            .Columns("Check").HeaderText = ""
            .Columns("Check").Width = 20
            With .Columns("TRANSFERTYPE")
                .HeaderText = "TR_TYPE"
                .Width = 140
            End With
            With .Columns("COLOR")
                .Visible = False
            End With
            With .Columns("DATE")
                .HeaderText = "TRANDATE"
                .ReadOnly = True
                .Width = 80
            End With
            With .Columns("CATNAME")
                .HeaderText = "CATEGORY"
                .ReadOnly = True
                .Width = 140
            End With

            With .Columns("TRANNO")
                .ReadOnly = True
                .Width = 60
            End With
            With .Columns("ITEMNAME")
                .HeaderText = "ITEM NAME"
                .ReadOnly = True
                .Width = 140
            End With
            With .Columns("PCS")
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 40
            End With
            With .Columns("GRSWT")
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 75
            End With
            With .Columns("DUSTWT")
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 70
            End With

            With .Columns("NETWT")
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 75
            End With
            With .Columns("CNETWT")
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 75
            End With
            With .Columns("TOUCH")
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 50
            End With

            With .Columns("PUREWT")
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 75
            End With
            With .Columns("RATE")
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 70
                .Visible = True
            End With
            With .Columns("AMOUNT")
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Width = 80
                .Visible = True
            End With
            With .Columns("TYPE")
                .ReadOnly = True
                .Width = 100
            End With
            With .Columns("BATCHNO")
                .ReadOnly = True
                .Visible = False
            End With
            With .Columns("COSTID")
                .ReadOnly = True
                .Visible = False
            End With
            With .Columns("TRANDATE")
                .ReadOnly = True
                .Visible = False
            End With
            With .Columns("SNO")
                .ReadOnly = True
                .Visible = False
            End With
            With .Columns("METALID")
                .ReadOnly = True
                .Visible = False
            End With
            With .Columns("SMITH")
                .HeaderText = "DEALER"
                .Width = 150
            End With
            With .Columns("EMPNAME")
                .HeaderText = "EMPLOYEE"
                .Width = 100
            End With
            With .Columns("COUNTER")
                .HeaderText = "COUNTER"
                .Width = 100
            End With
            With .Columns("OPCS")
                .ReadOnly = True
                .Visible = False
            End With
            With .Columns("OGRSWT")
                .ReadOnly = True
                .Visible = False
            End With
            With .Columns("ONETWT")
                .ReadOnly = True
                .Visible = False
            End With
            With .Columns("OPUREWT")
                .ReadOnly = True
                .Visible = False
            End With
            If .Columns.Contains("CATCODE") Then
                With .Columns("CATCODE")
                    .Visible = False
                End With
            End If
            If .Columns.Contains("PSSRCHECK") Then
                With .Columns("PSSRCHECK")
                    .Visible = False
                End With
            End If
        End With

        gridView_OWN.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        gridView_OWN.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)

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
        For Each col As DataGridViewColumn In gridView_OWN.Columns
            With gridViewtotal.Columns(col.Name)

                .Visible = col.Visible
                .Width = col.Width
                .DefaultCellStyle = col.DefaultCellStyle
            End With
        Next
        dtGridViewTotal.Columns.Remove("CHECK")
        gridViewtotal.Columns(0).Width = (gridViewtotal.Columns(0).Width) + 20
        CalcGridViewTotal()

        ''LOAD COSTCENTRE
        If funcCheckCostCentreStatusFalse() Then
            cmbCostCentre_MAN.Enabled = False
            cmbCostCenterName.Enabled = False
        End If
        If cmbCostCentre_MAN.Enabled = True Then
            cmbCostCentre_MAN.Items.Add("ALL")
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            objGPack.FillCombo(strSql, cmbCostCentre_MAN, False, False)
        End If

        If cmbCostCenterName.Enabled = True Then
            cmbCostCenterName.Items.Add("ALL")
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            objGPack.FillCombo(strSql, cmbCostCenterName, False, False)
        End If

        cmbOpenMetal.Items.Add("ALL")
        cmbOpenMetalView.Items.Add("ALL")
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE ISNULL(ACTIVE,'') <> 'N' ORDER BY METALNAME"
        objGPack.FillCombo(strSql, cmbOpenMetal, False)
        objGPack.FillCombo(strSql, cmbOpenMetalView, False)

        strSql = " SELECT 'ALL' CATNAME,0 RESULT UNION ALL"
        strSql += vbCrLf + " SELECT CATNAME,1 RESULT FROM " & cnAdminDb & "..CATEGORY"
        strSql += vbCrLf + " ORDER BY RESULT,CATNAME"
        objGPack.FillCombo(strSql, CmbCategory, True, True)
        cmbOpenMetal.Text = ""

        cmbCompany_MAN.Items.Add("ALL")
        cmbOpenCompanyView.Items.Add("ALL")
        strSql = " SELECT COMPANYNAME FROM " & cnAdminDb & "..COMPANY ORDER BY COMPANYID"
        objGPack.FillCombo(strSql, cmbCompany_MAN, False, False)
        objGPack.FillCombo(strSql, cmbOpenCompanyView, False, False)

        cmbEntryType_MAN.Items.Clear()
        cmbEntryType_MAN.Items.Add("DEALER")
        cmbEntryType_MAN.Items.Add("LOT")
        cmbEntryType_MAN.Items.Add("MELTING")
        cmbEntryType_MAN.Items.Add("NONTAG")
        cmbEntryType_MAN.Text = "LOT"



        funcNew()
    End Sub
    Private Sub CalcGridViewTotal()
        Dim LAmt As Double = Nothing
        Dim LPcs As Integer = Nothing
        Dim LGrsWt As Decimal = Nothing
        Dim LNetWt As Decimal = Nothing
        Dim LDustWt As Decimal = Nothing
        Dim LCNetWt As Decimal = Nothing
        Dim LPureWt As Decimal = Nothing
        Dim MAmt As Double = Nothing
        Dim MPcs As Integer = Nothing
        Dim MGrsWt As Decimal = Nothing
        Dim MNetWt As Decimal = Nothing
        Dim MDustWt As Decimal = Nothing
        Dim MCNetWt As Decimal = Nothing
        Dim MPureWt As Decimal = Nothing
        Dim UnAmt As Double = Nothing
        Dim UnPcs As Integer = Nothing
        Dim UnGrsWt As Decimal = Nothing
        Dim UnNetWt As Decimal = Nothing
        Dim UnDustWt As Decimal = Nothing
        Dim UnCNetWt As Decimal = Nothing
        Dim UnPureWt As Decimal = Nothing
        Dim NAmt As Double = Nothing
        Dim NPcs As Integer = Nothing
        Dim NGrsWt As Decimal = Nothing
        Dim NNetWt As Decimal = Nothing
        Dim NDustWt As Decimal = Nothing
        Dim NCNetWt As Decimal = Nothing
        Dim NPureWt As Decimal = Nothing
        Dim DAmt As Double = Nothing
        Dim DPcs As Integer = Nothing
        Dim DGrsWt As Decimal = Nothing
        Dim DNetWt As Decimal = Nothing
        Dim DDustWt As Decimal = Nothing
        Dim DCNetWt As Decimal = Nothing
        Dim DPureWt As Decimal = Nothing
        For i As Integer = 0 To gridView_OWN.RowCount - 1
            With gridView_OWN.Rows(i)
                If .Cells("CHECK").Value = True Then
                    If .Cells("TRANSFERTYPE").Value.ToString.Trim.ToUpper = "TO LOT" Then
                        LPcs += Val(.Cells("PCS").Value.ToString)
                        LGrsWt += Val(.Cells("GRSWT").Value.ToString)
                        LNetWt += Val(.Cells("NETWT").Value.ToString)
                        LDustWt += Val(.Cells("DUSTWT").Value.ToString)
                        LCNetWt += Val(.Cells("CNETWT").Value.ToString)
                        LPureWt += Val(.Cells("PUREWT").Value.ToString)
                        LAmt += Val(.Cells("AMOUNT").Value.ToString)
                    ElseIf .Cells("TRANSFERTYPE").Value.ToString.Trim.ToUpper = "TO NONTAG" Then
                        NPcs += Val(.Cells("PCS").Value.ToString)
                        NGrsWt += Val(.Cells("GRSWT").Value.ToString)
                        NNetWt += Val(.Cells("NETWT").Value.ToString)
                        NDustWt += Val(.Cells("DUSTWT").Value.ToString)
                        NCNetWt += Val(.Cells("CNETWT").Value.ToString)

                        NPureWt += Val(.Cells("PUREWT").Value.ToString)
                        NAmt += Val(.Cells("AMOUNT").Value.ToString)
                    ElseIf .Cells("TRANSFERTYPE").Value.ToString.Trim.ToUpper = "TO DEALER" Then
                        DPcs += Val(.Cells("PCS").Value.ToString)
                        DGrsWt += Val(.Cells("GRSWT").Value.ToString)
                        DNetWt += Val(.Cells("NETWT").Value.ToString)
                        DDustWt += Val(.Cells("DUSTWT").Value.ToString)
                        DCNetWt += Val(.Cells("CNETWT").Value.ToString)

                        DPureWt += Val(.Cells("PUREWT").Value.ToString)
                        DAmt += Val(.Cells("AMOUNT").Value.ToString)


                    Else
                        MPcs += Val(.Cells("PCS").Value.ToString)
                        MGrsWt += Val(.Cells("GRSWT").Value.ToString)
                        MNetWt += Val(.Cells("NETWT").Value.ToString)
                        MDustWt += Val(.Cells("DUSTWT").Value.ToString)
                        MCNetWt += Val(.Cells("CNETWT").Value.ToString)
                        MPureWt += Val(.Cells("PUREWT").Value.ToString)
                        MAmt += Val(.Cells("AMOUNT").Value.ToString)
                    End If
                End If
                UnPcs += Val(.Cells("PCS").Value.ToString)
                UnGrsWt += Val(.Cells("GRSWT").Value.ToString)
                UnNetWt += Val(.Cells("NETWT").Value.ToString)
                UnDustWt += Val(.Cells("DUSTWT").Value.ToString)
                UnCNetWt += Val(.Cells("CNETWT").Value.ToString)

                UnPureWt += Val(.Cells("PUREWT").Value.ToString)
                UnAmt += Val(.Cells("AMOUNT").Value.ToString)
            End With
        Next
        If gridViewtotal.RowCount > 0 Then
            With gridViewtotal.Rows(0)
                .Cells("TRANSFERTYPE").Value = "TOTAL"
                .Cells("PCS").Value = IIf(UnPcs <> 0, UnPcs, DBNull.Value)
                .Cells("GRSWT").Value = IIf(UnGrsWt <> 0, UnGrsWt, DBNull.Value)
                .Cells("NETWT").Value = IIf(UnNetWt <> 0, UnNetWt, DBNull.Value)
                .Cells("DUSTWT").Value = IIf(UnDustWt <> 0, UnDustWt, DBNull.Value)
                .Cells("CNETWT").Value = IIf(UnCNetWt <> 0, UnCNetWt, DBNull.Value)
                Dim mtouch As Double = 0
                If UnNetWt <> 0 And UnPureWt <> 0 Then mtouch = Math.Round((UnPureWt / UnNetWt) * 100, 2)
                .Cells("TOUCH").Value = IIf(mtouch <> 0, mtouch, DBNull.Value)
                .Cells("PUREWT").Value = IIf(UnPureWt <> 0, UnPureWt, DBNull.Value)
                .Cells("AMOUNT").Value = IIf(UnAmt <> 0, UnAmt, DBNull.Value)
                .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                .DefaultCellStyle.ForeColor = Color.Blue
                .DefaultCellStyle.SelectionForeColor = Color.Gray
                .Cells("TRANSFERTYPE").Style.BackColor = Color.LightCoral
            End With

            With gridViewtotal.Rows(1)
                .Cells("TRANSFERTYPE").Value = "MELTING TOT"
                .Cells("PCS").Value = IIf(MPcs <> 0, MPcs, DBNull.Value)
                .Cells("GRSWT").Value = IIf(MGrsWt <> 0, MGrsWt, DBNull.Value)
                .Cells("NETWT").Value = IIf(MNetWt <> 0, MNetWt, DBNull.Value)
                .Cells("DUSTWT").Value = IIf(MDustWt <> 0, MDustWt, DBNull.Value)
                .Cells("CNETWT").Value = IIf(MCNetWt <> 0, MCNetWt, DBNull.Value)
                Dim mtouch As Double = 0
                If MCNetWt <> 0 And MPureWt <> 0 Then mtouch = Math.Round((MPureWt / MCNetWt) * 100, 2)
                .Cells("TOUCH").Value = IIf(mtouch <> 0, mtouch, DBNull.Value)
                .Cells("PUREWT").Value = IIf(MPureWt <> 0, MPureWt, DBNull.Value)
                .Cells("AMOUNT").Value = IIf(MAmt <> 0, MAmt, DBNull.Value)
                .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                .DefaultCellStyle.ForeColor = Color.Blue
                .DefaultCellStyle.SelectionForeColor = Color.Blue
                .Cells("TRANSFERTYPE").Style.BackColor = Color.LightYellow
            End With
            With gridViewtotal.Rows(2)
                .Cells("TRANSFERTYPE").Value = "LOT TOT"
                .Cells("PCS").Value = IIf(LPcs <> 0, LPcs, DBNull.Value)
                .Cells("GRSWT").Value = IIf(LGrsWt <> 0, LGrsWt, DBNull.Value)
                .Cells("NETWT").Value = IIf(LNetWt <> 0, LNetWt, DBNull.Value)
                .Cells("DUSTWT").Value = IIf(LDustWt <> 0, LDustWt, DBNull.Value)
                .Cells("CNETWT").Value = IIf(LCNetWt <> 0, LCNetWt, DBNull.Value)
                Dim mtouch As Double = 0
                If LCNetWt <> 0 And MPureWt <> 0 Then mtouch = Math.Round((LPureWt / LCNetWt) * 100, 2)
                .Cells("TOUCH").Value = IIf(mtouch <> 0, mtouch, DBNull.Value)
                .Cells("PUREWT").Value = IIf(LPureWt <> 0, LPureWt, DBNull.Value)
                .Cells("AMOUNT").Value = IIf(LAmt <> 0, LAmt, DBNull.Value)
                .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                .DefaultCellStyle.ForeColor = Color.Blue
                .DefaultCellStyle.SelectionForeColor = Color.Blue
                .Cells("TRANSFERTYPE").Style.BackColor = Color.LightYellow
            End With
            With gridViewtotal.Rows(3)
                .Cells("TRANSFERTYPE").Value = "NONTAG TOT"
                .Cells("PCS").Value = IIf(NPcs <> 0, NPcs, DBNull.Value)
                .Cells("GRSWT").Value = IIf(NGrsWt <> 0, NGrsWt, DBNull.Value)
                .Cells("NETWT").Value = IIf(NNetWt <> 0, NNetWt, DBNull.Value)
                .Cells("DUSTWT").Value = IIf(NDustWt <> 0, NDustWt, DBNull.Value)
                .Cells("CNETWT").Value = IIf(NCNetWt <> 0, NCNetWt, DBNull.Value)
                Dim mtouch As Double = 0
                If NNetWt <> 0 And NPureWt <> 0 Then mtouch = Math.Round((NPureWt / NNetWt) * 100, 2)
                .Cells("TOUCH").Value = IIf(mtouch <> 0, mtouch, DBNull.Value)
                .Cells("PUREWT").Value = IIf(NPureWt <> 0, NPureWt, DBNull.Value)
                .Cells("AMOUNT").Value = IIf(NAmt <> 0, NAmt, DBNull.Value)
                .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                .DefaultCellStyle.ForeColor = Color.Blue
                .DefaultCellStyle.SelectionForeColor = Color.Blue
                .Cells("TRANSFERTYPE").Style.BackColor = Color.LightYellow
            End With
            With gridViewtotal.Rows(4)
                .Cells("TRANSFERTYPE").Value = "TO DEALER"
                .Cells("PCS").Value = IIf(DPcs <> 0, DPcs, DBNull.Value)
                .Cells("GRSWT").Value = IIf(DGrsWt <> 0, DGrsWt, DBNull.Value)
                .Cells("NETWT").Value = IIf(DNetWt <> 0, DNetWt, DBNull.Value)
                .Cells("DUSTWT").Value = IIf(DDustWt <> 0, DDustWt, DBNull.Value)
                .Cells("CNETWT").Value = IIf(DCNetWt <> 0, DCNetWt, DBNull.Value)
                Dim mtouch As Double = 0
                If DCNetWt <> 0 And DPureWt <> 0 Then mtouch = Math.Round((DPureWt / DNetWt) * 100, 2)
                .Cells("TOUCH").Value = IIf(mtouch <> 0, mtouch, DBNull.Value)
                .Cells("PUREWT").Value = IIf(DPureWt <> 0, DPureWt, DBNull.Value)
                .Cells("AMOUNT").Value = IIf(DAmt <> 0, DAmt, DBNull.Value)
                .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                .DefaultCellStyle.ForeColor = Color.Blue
                .DefaultCellStyle.SelectionForeColor = Color.Blue
                .Cells("TRANSFERTYPE").Style.BackColor = Color.LightYellow
            End With
        End If
        If gridView_OWN.RowCount > 0 Then
            gridViewtotal.Visible = True
        End If
    End Sub

    Private Sub cmbTransferTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbTransferTo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name = "TRANSFERTYPE" Then
                Dim pt As Point = gridView_OWN.Location
                gridView_OWN.CurrentRow.Cells("TRANSFERTYPE").Value = cmbTransferTo.Text
                dtGridView.AcceptChanges()
                CalcGridViewTotal()
                cmbTransferTo.Visible = False
                If gridView_OWN.CurrentRow.Cells("TRANSFERTYPE").Value = "TO DEALER" Then
                    gridView_OWN.CurrentRow.Cells("SMITH").Selected = True
                ElseIf gridView_OWN.CurrentRow.Cells("TRANSFERTYPE").Value = "TO LOT" Or gridView_OWN.CurrentRow.Cells("TRANSFERTYPE").Value = "TO NONTAG" Then
                    gridView_OWN.CurrentRow.Cells("ITEMNAME").Selected = True
                Else
                    gridView_OWN.CurrentRow.Cells("SMITH").Value = ""
                    If gridView_OWN.CurrentRow.Index + 1 <= gridView_OWN.RowCount - 1 Then
                        ' gridView.Rows(gridView.CurrentRow.Index + 1).Cells("TRANSFERTYPE").Selected = True
                        gridView_OWN.Focus()
                    Else
                        gridView_OWN.Focus()
                    End If
                End If
                Exit Sub
            ElseIf gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name = "ITEMNAME" Then
                If cmbTransferTo.Text.Trim = "" Then gridView_OWN.CurrentRow.Cells("ITEMNAME").Selected = True : Exit Sub
                Dim pt As Point = gridView_OWN.Location
                gridView_OWN.CurrentRow.Cells("ITEMNAME").Value = cmbTransferTo.Text
                dtGridView.AcceptChanges()
                cmbTransferTo.Visible = False
                If gridView_OWN.CurrentRow.Index + 1 <= gridView_OWN.RowCount - 1 Then
                    'gridView.Focus()
                    Select Case gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name
                        Case "CHECK"
                            If gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("TRANSFERTYPE").Value.ToString.ToUpper = "TO LOT" _
                            Or gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("TRANSFERTYPE").Value.ToString.ToUpper = "TO NONTAG" Then
                                If gridView_OWN.CurrentRow.Index + 1 <= gridView_OWN.Rows.Count - 1 Then
                                    Dim Crow As Integer = 0
                                    Dim Colstr As String = "CHECK"
                                    If True Then
                                        If gridView_OWN.CurrentRow Is Nothing Then
                                            Crow = 0
                                        Else
                                            Crow = gridView_OWN.CurrentRow.Index + 1
                                        End If
                                        gridView_OWN.Item(Colstr, Crow).Selected = True
                                        gridView_OWN.CurrentCell = gridView_OWN.Item(Colstr, Crow)
                                        gridView_OWN.BeginEdit(True)
                                        'cmbTransferTo.Focus()
                                        gridView_OWN.Focus()
                                    End If
                                End If
                            End If
                    End Select
                Else
                    CalcGridViewTotal()
                    gridView_OWN.Focus()
                End If
                Exit Sub
                'ElseIf gridView.Columns(gridView.CurrentCell.ColumnIndex).Name = "TOUCH" Then
                '    If cmbTransferTo.Text.Trim = "" Then gridView.CurrentRow.Cells("TOUCH").Selected = True : Exit Sub
                '    Dim pt As Point = gridView.Location
                '    gridView.CurrentRow.Cells("TOUCH").Value = cmbTransferTo.Text
                '    dtGridView.AcceptChanges()
                '    cmbTransferTo.Visible = False
                '    If gridView.CurrentRow.Cells("TRANSFERTYPE").Value = "TO DEALER" Then
                '        gridView.CurrentRow.Cells("SMITH").Selected = True
                '    ElseIf gridView.CurrentRow.Index + 1 <= gridView.RowCount - 1 Then
                '        gridView.Focus()
                '    Else
                '        CalcGridViewTotal()
                '        gridView.Focus()
                '    End If
                '    Exit Sub
            ElseIf gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name = "SMITH" Then
                If cmbTransferTo.Text.Trim = "" Then gridView_OWN.CurrentRow.Cells("SMITH").Selected = True : Exit Sub
                Dim pt As Point = gridView_OWN.Location
                gridView_OWN.CurrentRow.Cells("SMITH").Value = cmbTransferTo.Text
                dtGridView.AcceptChanges()
                cmbTransferTo.Visible = False
                If gridView_OWN.CurrentRow.Index + 1 <= gridView_OWN.RowCount - 1 Then
                    gridView_OWN.Rows(gridView_OWN.CurrentRow.Index + 1).Cells("TRANSFERTYPE").Selected = True
                    gridView_OWN.Focus()
                Else
                    CalcGridViewTotal()
                    gridView_OWN.Focus()
                End If
                Exit Sub
            ElseIf gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name = "COUNTER" Then
                If cmbTransferTo.Text.Trim = "" Then gridView_OWN.CurrentRow.Cells("COUNTER").Selected = True : Exit Sub
                Dim pt As Point = gridView_OWN.Location
                gridView_OWN.CurrentRow.Cells("COUNTER").Value = cmbTransferTo.Text
                dtGridView.AcceptChanges()
                cmbTransferTo.Visible = False
                If gridView_OWN.CurrentRow.Index + 1 <= gridView_OWN.RowCount - 1 Then
                    'gridView.Rows(gridView.CurrentRow.Index + 1).Cells("TRANSFERTYPE").Selected = True
                    gridView_OWN.Focus()
                Else
                    CalcGridViewTotal()
                    gridView_OWN.Focus()
                End If
                Exit Sub
            End If
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            cmbTransferTo.Text = "" : cmbTransferTo.Visible = False : btnGenerate.Focus()
        End If
    End Sub

    Private Function Loadcombo(ByVal TYPE As Integer, Optional ByVal MetalId As String = "")
        cmbTransferTo.Items.Clear()
        If TYPE = 1 Then
            ''Load Transfer To
            cmbTransferTo.Items.Add("TO LOT")
            cmbTransferTo.Items.Add("TO MELTING")
            cmbTransferTo.Items.Add("TO DEALER")
            cmbTransferTo.Items.Add("TO NONTAG")
        ElseIf TYPE = 2 Then
            strSql = " SELECT ACNAME,2 RESULT FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE IN('G','D') "
            strSql += GetAcNameQryFilteration()
            strSql += vbCrLf + " ORDER BY RESULT,ACNAME"
            objGPack.FillCombo(strSql, cmbTransferTo, True, False)
        ElseIf TYPE = 3 Then
            strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ISNULL(ACTIVE,'')<>'N'"
            strSql += vbCrLf + " ORDER BY ITEMCTRNAME"
            objGPack.FillCombo(strSql, cmbTransferTo, True, False)
        ElseIf TYPE = 4 Then
            strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ACTIVE,'')<>'N' "
            If MetalId <> "" Then strSql += vbCrLf + " AND CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID='" & MetalId & "')"
            strSql += vbCrLf + " ORDER BY ITEMNAME"
            objGPack.FillCombo(strSql, cmbTransferTo, True, False)
            'ElseIf TYPE = 5 Then
            '    strSql = " SELECT DISTINCT CONVERT(NUMERIC(15,2),PURITY) FROM " & cnAdminDb & "..PURITYMAST   "
            '    If MetalId <> "" Then strSql += vbcrlf + " WHERE  METALID='" & MetalId & "'"
            '    objGPack.FillCombo(strSql, cmbTransferTo, True, False)
        End If
    End Function

    Private Sub cmbTransferTo_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTransferTo.Leave
        cmbTransferTo.Visible = False
        cmbTransferTo_KeyPress(Me, New KeyPressEventArgs(Chr(Keys.Enter)))
    End Sub

    Private Sub cmbTransferTo_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTransferTo.SelectedValueChanged
        If gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name = "TOUCH" Then
            Dim Purity As Decimal = Val(cmbTransferTo.Text)
            Dim GrsWt As Decimal = Val(gridView_OWN.CurrentRow.Cells("GRSWT").Value.ToString)
            If GrsWt <> 0 And Purity <> 0 Then
                gridView_OWN.CurrentRow.Cells("PUREWT").Value = Format((GrsWt * Purity) / 100, "#0.000")
            End If
            CalcGridViewTotal()
        End If
    End Sub

    Private Sub txtGrid_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtGrid.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name = "PCS" Then
                Dim pt As Point = gridView_OWN.Location
                gridView_OWN.CurrentRow.Cells("PCS").Value = Val(txtGrid.Text)
                dtGridView.AcceptChanges()
                CalcGridViewTotal()
                txtGrid.Visible = False
                gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("GRSWT").Selected = True
                'gridView.Focus()
                Exit Sub
            ElseIf gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name = "GRSWT" Then
                Dim pt As Point = gridView_OWN.Location
                gridView_OWN.CurrentRow.Cells("GRSWT").Value = Format(Math.Round(Val(txtGrid.Text), 3), "#0.000")
                gridView_OWN.CurrentRow.Cells("NETWT").Value = Format(Math.Round(Val(txtGrid.Text), 3), "#0.000")
                dtGridView.AcceptChanges()
                CalcGridViewTotal()
                txtGrid.Visible = False
                gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("CNETWT").Selected = True
                'gridView.Focus()
                Exit Sub
            ElseIf gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name = "CNETWT" Then
                Dim pt As Point = gridView_OWN.Location
                If Val(gridView_OWN.CurrentRow.Cells("GRSWT").Value.ToString) < Val(txtGrid.Text) Then
                    MsgBox("Netwt must be less then or equal to grswt.", MsgBoxStyle.Information)
                    Exit Sub
                End If

                gridView_OWN.CurrentRow.Cells("CNETWT").Value = Format(Math.Round(Val(txtGrid.Text), 3), "#0.000")
                dtGridView.AcceptChanges()
                CalcGridViewTotal()
                txtGrid.Visible = False
                'gridView.Rows(gridView.CurrentRow.Index + 1).Cells("CHECK").Selected = True
                gridView_OWN.Focus()
                Exit Sub
            End If
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            txtGrid.Visible = False : btnGenerate.Focus()
        End If
    End Sub

    Private Sub txtGrid_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGrid.Leave
        txtGrid.Visible = False
    End Sub
    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        OpenToolStripMenuItem_Click(OpenToolStripMenuItem, Nothing)
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = page1
        dtpFrom.Focus()
        GroupBox2.Visible = True
        dGridView.DataSource = Nothing
    End Sub
    Dim chk As Boolean = False
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not chkPurchaseView.Checked And Not chkSalesReturnView.Checked And Not chkMiscIssueView.Checked And Not chkPartysaleView.Checked Then MsgBox("Please Select one Type") : Exit Sub
        dtGridView.Rows.Clear()
        Dim qrystr As Boolean = False
        strSql = ""
        If chkSalesReturnView.Checked Or chkPurchaseView.Checked Then
            chk = True
            strSql = " SELECT C.METALID, "
            strSql += vbCrLf + " CASE WHEN A.TRANTYPE = 'PU' THEN 'PURCHASE' WHEN A.TRANTYPE = 'SR' THEN 'SALES RETURN' END AS TYPE"
            strSql += vbCrLf + " ,BAGNO,CASE WHEN MELT_RETAG = 'L' THEN 'LOT' ELSE 'MELTING' END TR_TYPE ,CONVERT(VARCHAR,TRANDATE,103)AS DATE,A.CATCODE,C.CATNAME,TRANNO,I.ITEMNAME"
            strSql += vbCrLf + " ,A.PCS,A.GRSWT,NETWT,AMOUNT,BATCHNO,COSTID"
            strSql += vbCrLf + " ,A.TRANTYPE TYPE,TRANDATE,SNO,A.PCS AS OPCS,A.GRSWT OGRSWT,NETWT ONETWT"
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT A"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON A.CATCODE = C.CATCODE"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON A.ITEMID= I.ITEMID"
            'strSql += vbcrlf + " LEFT JOIN " & cnAdminDb & "..EMPMAST E ON A.EMPID= E.EMPID"
            strSql += vbCrLf + " WHERE A.TRANTYPE <> '' AND ISNULL(CANCEL,'') = '' "
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & dtpFromView.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpToView.Value.ToString("yyyy-MM-dd") & "'"
            If cmbOpenMetalView.Text <> "ALL" Then strSql += vbCrLf + " AND C.METALID = ISNULL((SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME= '" & cmbOpenMetalView.Text & "'),'')"
            If cmbOpenCompanyView.Text <> "ALL" And cmbOpenCompanyView.Text <> "" Then strSql += vbCrLf + " AND A.COMPANYID = ISNULL((SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME= '" & cmbOpenCompanyView.Text & "'),'')"
            strSql += vbCrLf + " AND ("
            strSql += vbCrLf + " A.TRANTYPE = ''"
            If chkPurchaseView.Checked Then
                strSql += vbCrLf + " OR A.TRANTYPE = 'PU' "
            End If
            If chkSalesReturnView.Checked Then
                strSql += vbCrLf + " OR A.TRANTYPE = 'SR'"
            End If
            strSql += vbCrLf + " )"
            If cmbCostCenterName.Enabled Then
                If UCase(cmbCostCentre_MAN.Text) <> "ALL" And cmbCostCenterName.Text <> "" Then
                    strSql += vbCrLf + " AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenterName.Text & "')"
                End If
            End If
            strSql += vbCrLf + " AND ISNULL(BAGNO,'') <>'' AND ISNULL(MELT_RETAG,'')<>''"
            qrystr = True
        End If
        If chkMiscIssueView.Checked Then
            chk = False
            If qrystr Then strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT C.METALID,"
            strSql += vbCrLf + " CASE WHEN A.TRANTYPE = 'MI' THEN 'MISCELLANIOUS' WHEN A.TRANTYPE = 'SR' THEN 'SALES RETURN' END AS TYPE"
            strSql += vbCrLf + " ,BAGNO,CASE WHEN MELT_RETAG = 'L' THEN 'LOT' ELSE 'MELTING' END TR_TYPE,CONVERT(VARCHAR,TRANDATE,103)AS DATE,A.CATCODE,C.CATNAME,TRANNO,I.ITEMNAME"
            strSql += vbCrLf + " ,A.PCS,A.GRSWT,NETWT,AMOUNT,BATCHNO,COSTID"
            strSql += vbCrLf + " ,A.TRANTYPE TYPE,TRANDATE,SNO,A.PCS AS OPCS,A.GRSWT OGRSWT,NETWT ONETWT"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE A"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON A.CATCODE = C.CATCODE"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON A.ITEMID= I.ITEMID"
            strSql += vbCrLf + " WHERE A.TRANTYPE <> '' AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & dtpFromView.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpToView.Value.ToString("yyyy-MM-dd") & "'"
            If cmbOpenMetalView.Text <> "ALL" Then strSql += vbCrLf + " AND C.METALID = ISNULL((SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME= '" & cmbOpenMetalView.Text & "'),'')"
            If cmbOpenCompanyView.Text <> "ALL" And cmbOpenCompanyView.Text <> "" Then strSql += vbCrLf + " AND A.COMPANYID = ISNULL((SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME= '" & cmbOpenCompanyView.Text & "'),'')"
            strSql += vbCrLf + " AND"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " A.TRANTYPE = ''"
            If chkMiscIssueView.Checked Then strSql += vbCrLf + " OR A.TRANTYPE = 'MI' "
            strSql += vbCrLf + " )"
            If cmbCostCenterName.Enabled Then
                If UCase(cmbCostCenterName.Text) <> "ALL" And cmbCostCenterName.Text <> "" Then strSql += vbCrLf + " AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenterName.Text & "')"
            End If
            strSql += vbCrLf + " AND ISNULL(BAGNO,'') <>'' AND ISNULL(MELT_RETAG,'')<>''"
            qrystr = True
        End If
        If chkPartysaleView.Checked Then
            If qrystr Then strSql += vbCrLf + " UNION ALL"

            strSql += vbCrLf + " SELECT C.METALID,'PARTLY SALES' AS TYPE"
            strSql += vbCrLf + " ,BAGNO,CASE WHEN MELT_RETAG = 'L' THEN 'LOT' ELSE 'MELTING' END TR_TYPE,CONVERT(VARCHAR,TRANDATE,103)AS DATE,A.CATCODE,C.CATNAME,TRANNO,I.ITEMNAME"
            strSql += vbCrLf + " , TAGPCS-A.PCS PCS, TAGGRSWT-A.GRSWT GRSWT, TAGNETWT-NETWT NETWT ,AMOUNT,BATCHNO,COSTID"
            strSql += vbCrLf + " ,A.TRANTYPE TYPE,TRANDATE,SNO,A.PCS AS OPCS,A.GRSWT OGRSWT,NETWT ONETWT"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE A"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON A.CATCODE = C.CATCODE"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON A.ITEMID= I.ITEMID"
            strSql += vbCrLf + " WHERE A.TRANTYPE = 'SA' AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + " AND TRANDATE BETWEEN '" & dtpFromView.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpToView.Value.ToString("yyyy-MM-dd") & "'"
            If cmbOpenMetalView.Text <> "ALL" Then strSql += vbCrLf + " AND C.METALID = ISNULL((SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME= '" & cmbOpenMetalView.Text & "'),'')"
            If cmbOpenCompanyView.Text <> "ALL" And cmbOpenCompanyView.Text <> "" Then strSql += vbCrLf + " AND A.COMPANYID = ISNULL((SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME= '" & cmbOpenCompanyView.Text & "'),'')"
            strSql += vbCrLf + " AND (TAGPCS <> PCS OR TAGGRSWT <> A.GRSWT )"
            strSql += vbCrLf + " AND (TAGPCS <> 0 OR TAGGRSWT <> 0 )"
            If cmbCostCenterName.Enabled Then
                If UCase(cmbCostCenterName.Text) <> "ALL" And cmbCostCenterName.Text <> "" Then strSql += vbCrLf + " AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenterName.Text & "')"
            End If
            strSql += vbCrLf + " AND ISNULL(BAGNO,'') <>'' AND ISNULL(MELT_RETAG,'')<>''"
        End If
        strSql += vbCrLf + " ORDER BY A.TRANTYPE,TRANDATE,TRANNO"
        Dim dsGridView As DataSet
        dsGridView = New DataSet
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dsGridView)
        If dsGridView.Tables(0).Rows.Count > 0 Then
            dGridView.DataSource = dsGridView.Tables(0)
            Call ViewGrid()
        Else
            dGridView.DataSource = Nothing
            MsgBox("No Records Found.", MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub dGridView_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dGridView.KeyPress
        If UCase(e.KeyChar) = "C" Then
            Try
                Dim tblName As String
                If dGridView.CurrentRow.Cells("TYPE1").Value.ToString = "PU" Or dGridView.CurrentRow.Cells("TYPE1").Value.ToString = "SR" Then
                    tblName = "RECEIPT"
                Else
                    tblName = "ISSUE"
                End If
                If dGridView.CurrentRow.Cells("TR_TYPE").Value.ToString = "LOT" Then
                    strSql = "SELECT 'CHECK' FROM " & cnAdminDb & "..ITEMLOT "
                    strSql += vbCrLf + " WHERE ISNULL(LOTNO,0) = " & Val(dGridView.CurrentRow.Cells("BAGNO").Value.ToString) & ""
                    strSql += vbCrLf + " AND ISNULL(CPCS,0)=0 AND ISNULL(CGRSWT,0)=0 AND ISNULL(CNETWT,0)=0 "
                    If objGPack.GetSqlValue(strSql).Length = 0 Then
                        MsgBox("Tag made against this lotno." & vbCrLf & "Cannot cancel this entry.", MsgBoxStyle.Information)
                        Exit Sub
                    End If
                Else
                    strSql = "SELECT 'CHECK' FROM " & cnStockDb & "..MELTINGDETAIL "
                    strSql += vbCrLf + " WHERE ISNULL(BAGNO,'') = '" & dGridView.CurrentRow.Cells("BAGNO").Value.ToString & "'"
                    strSql += vbCrLf + " AND RECISS = 'R' AND ISNULL(CANCEL,'') = '' AND TRANTYPE='MR'"
                    If objGPack.GetSqlValue(strSql).Length > 0 Then
                        MsgBox("Melting Receipt made against this bagno." & vbCrLf & "Cannot cancel this entry.", MsgBoxStyle.Information)
                        Exit Sub
                    End If
                End If

                If MessageBox.Show("Do you want to Cancel?", "Cancel Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
                    Exit Sub
                End If
                Dim objSecret As New frmAdminPassword()
                If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then
                    Exit Sub
                End If
                'strSql += vbCrLf + " WHERE SNO = '" & dGridView.CurrentRow.Cells("SNO").Value.ToString & "' "
                'strSql += vbCrLf + " AND BATCHNO = '" & dGridView.CurrentRow.Cells("BATCHNO").Value.ToString & "'"
                'Comment on 18May2022 Siva villa 
                strSql = " UPDATE " & cnStockDb & ".." & tblName & " SET BAGNO='',MELT_RETAG='' "
                strSql += vbCrLf + " WHERE BAGNO = '" & dGridView.CurrentRow.Cells("BAGNO").Value.ToString & "' "
                cmd = New OleDb.OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
                If dGridView.CurrentRow.Cells("TR_TYPE").Value.ToString = "LOT" Then
                    strSql = "UPDATE " & cnAdminDb & "..ITEMLOT SET CPCS=ISNULL(PCS,0),CGRSWT=ISNULL(GRSWT,0),CNETWT=ISNULL(NETWT,0)"
                    strSql += vbCrLf + " WHERE ISNULL(LOTNO,0) = " & Val(dGridView.CurrentRow.Cells("BAGNO").Value.ToString) & ""
                    strSql += vbCrLf + " AND ISNULL(CPCS,0)=0 AND ISNULL(CGRSWT,0)=0 AND ISNULL(CNETWT,0)=0 "
                    cmd = New OleDb.OleDbCommand(strSql, cn)
                    cmd.ExecuteNonQuery()
                Else
                    strSql = " UPDATE I SET I.CANCEL='Y' FROM " & cnStockDb & "..ISSUE I"
                    strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..MELTINGDETAIL M ON I.BAGNO=M.BAGNO AND I.BATCHNO=M.BATCHNO "
                    strSql += vbCrLf + " WHERE I.BAGNO='" & dGridView.CurrentRow.Cells("BAGNO").Value.ToString & "' AND M.RECISS='I'"
                    cmd = New OleDb.OleDbCommand(strSql, cn)
                    cmd.ExecuteNonQuery()
                    strSql = " UPDATE " & cnStockDb & "..MELTINGDETAIL SET CANCEL='Y' "
                    strSql += vbCrLf + " WHERE BAGNO='" & dGridView.CurrentRow.Cells("BAGNO").Value.ToString & "' AND RECISS='I'"
                    cmd = New OleDb.OleDbCommand(strSql, cn)
                    cmd.ExecuteNonQuery()
                End If
                MsgBox("Successfully Cancelled", MsgBoxStyle.Information)
                btnSearch_Click(btnSearch, Nothing)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical)
            End Try
        End If
    End Sub

    Private Sub ViewGrid()
        dGridView.Columns("TYPE1").Visible = False
        dGridView.Columns("METALID").Visible = False
        dGridView.Columns("CATNAME").HeaderText = "CATEGORY"
        dGridView.Columns("ITEMNAME").Visible = False
        dGridView.Columns("AMOUNT").Visible = False
        dGridView.Columns("SNO").Visible = False
        dGridView.Columns("OPCS").Visible = False
        dGridView.Columns("OGRSWT").Visible = False
        dGridView.Columns("ONETWT").Visible = False
        dGridView.Columns("CATCODE").Visible = False
        dGridView.Columns("TRANDATE").Visible = False
        dGridView.Columns("COSTID").Visible = False
        dGridView.Columns("DATE").DefaultCellStyle.Format = "dd-MM-yyyy"
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        tabMain.SelectedTab = page2
        dtpFromView.Focus()
        GroupBox2.Visible = False
    End Sub

    Private Sub cmbOpenMetal_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbOpenMetal.SelectedIndexChanged
        If cmbOpenMetal.Text <> "" And cmbOpenMetal.Text <> "ALL" Then
            strSql = " SELECT 'ALL' CATNAME,0 RESULT UNION ALL"
            strSql += vbCrLf + " SELECT CATNAME,1 RESULT FROM " & cnAdminDb & "..CATEGORY  "
            strSql += vbCrLf + " WHERE METALID IN(SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME='" & cmbOpenMetal.Text & "')"
            strSql += vbCrLf + " ORDER BY RESULT,CATNAME"
            objGPack.FillCombo(strSql, CmbCategory, True, True)
        End If
    End Sub

    Private Sub chkPosting_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPosting.CheckedChanged
        If chkPosting.Checked Then
            strSql = "SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'TAB_COMPANY')"
            Dim str2CompId As String = objGPack.GetSqlValue(strSql, , , tran)
            If str2CompId <> strCompanyId Then chkPosting.Checked = False : Exit Sub
            chkPurchase.Checked = False
            chkSalesReturn.Checked = False
            chkMiscIssue.Checked = False
            chkPartlySale.Checked = False
        End If
    End Sub

    Private Sub chkPurchase_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPurchase.CheckedChanged
        If chkPurchase.Checked Then
            chkPosting.Checked = False
        End If
        grpPurchase.Visible = chkPurchase.Checked
    End Sub

    Private Sub chkSalesReturn_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSalesReturn.CheckedChanged
        If chkSalesReturn.Checked Then
            chkPosting.Checked = False
        End If
    End Sub

    Private Sub chkMiscIssue_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMiscIssue.CheckedChanged
        If chkMiscIssue.Checked Then
            chkPosting.Checked = False
        End If
    End Sub

    Private Sub chkPartlySale_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPartlySale.CheckedChanged
        If chkPartlySale.Checked Then
            chkPosting.Checked = False
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        BrightPosting.GExport.Post(Me.Name, strCompanyName, "PENDING TRANSFER", dGridView, BrightPosting.GExport.GExportType.Print)
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        BrightPosting.GExport.Post(Me.Name, strCompanyName, "PENDING TRANSFER", dGridView, BrightPosting.GExport.GExportType.Export)
    End Sub

    Private Sub cmbEntryType_MAN_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbEntryType_MAN.SelectedIndexChanged
        If cmbEntryType_MAN.Text = "DEALER" And chkToType.Checked And PENDTRF_LOAD_ACNAME Then
            lblAcname.Visible = True
            CmbAcname_MAN.Visible = True
            CmbAcname_MAN.Text = "ALL"
        Else
            lblAcname.Visible = False
            CmbAcname_MAN.Visible = False
            CmbAcname_MAN.Text = "ALL"
        End If
    End Sub

    Private Sub chkToType_CheckedChanged(sender As Object, e As EventArgs) Handles chkToType.CheckedChanged
        If chkToType.Checked And cmbEntryType_MAN.Text = "DEALER" And PENDTRF_LOAD_ACNAME Then
            lblAcname.Visible = True
            CmbAcname_MAN.Visible = True
            CmbAcname_MAN.Text = "ALL"
        Else
            lblAcname.Visible = False
            CmbAcname_MAN.Visible = False
            CmbAcname_MAN.Text = "ALL"
        End If
    End Sub

    Private Sub btnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.Click
        If gridView_OWN.Rows.Count > 0 Then
            Dim str As String = ""
            BrightPosting.GExport.Post(Me.Name, strCompanyName, str, gridView_OWN, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub
End Class