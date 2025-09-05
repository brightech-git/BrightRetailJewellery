Imports System.Data.OleDb
Public Class frmDailyTransactionCollection
    Dim strSql As String = ""
    Dim SelectedCompanyId As String = ""
    Dim StrUseridFtr As String = ""
    Dim DT As New DataTable()
    Dim cmd As New OleDbCommand
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        AddHandler chkLstCompany.LostFocus, AddressOf CheckedListCompany_Lostfocus
    End Sub
    Private Sub frmDailyTransactionCollection_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        btnNew_Click(Me, New EventArgs())
        rbtDayWise.Checked = True
        lblTodate.Visible = False
        dtpTodate.Visible = False
        LoadCompany(chkLstCompany)
    End Sub
    Function funcAddCostCentre() As Integer
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE'"
        If objGPack.GetSqlValue(strSql, , "N") = "Y" Then
            strSql = "SELECT DISTINCT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            cmbCostCentre.Items.Clear()
            cmbCostCentre.Items.Add("ALL")
            objGPack.FillCombo(strSql, cmbCostCentre, False, False)
            cmbCostCentre.Text = IIf(cnDefaultCostId, "ALL", cnCostName)
            If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then cmbCostCentre.Enabled = False
        Else
            cmbCostCentre.Items.Clear()
            cmbCostCentre.Enabled = False
        End If
    End Function

    Private Sub chkCompanySelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCompanySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
    End Sub
#Region "Get&Set Properties"
    Private Sub Prop_Gets()
        Dim obj As New frmDailyTransactionCollection_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmDailyTransactionCollection_Properties))
        chkCompanySelectAll.Checked = obj.p_chkCompanySelectAll
        SetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany, strCompanyName)
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmDailyTransactionCollection_Properties
        obj.p_chkCompanySelectAll = chkCompanySelectAll.Checked
        GetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany)
        SetSettingsObj(obj, Me.Name, GetType(frmDailyTransactionCollection_Properties))
    End Sub

    Public Class frmDailyTransactionCollection_Properties

        Private chkCompanySelectAll As Boolean = False
        Public Property p_chkCompanySelectAll() As Boolean
            Get
                Return chkCompanySelectAll
            End Get
            Set(ByVal value As Boolean)
                chkCompanySelectAll = value
            End Set
        End Property
        Private chkLstCompany As New List(Of String)
        Public Property p_chkLstCompany() As List(Of String)
            Get
                Return chkLstCompany
            End Get
            Set(ByVal value As List(Of String))
                chkLstCompany = value
            End Set
        End Property
    End Class
#End Region
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        dtpFrom.Value = GetServerDate()
        dtpTodate.Value = GetServerDate()
        funcAddCostCentre()
        GridView2.DataSource = Nothing
        Prop_Gets()
        cmbCostCentre.Text = IIf(cnDefaultCostId, "ALL", cnCostName)
        btnView_Search.Enabled = True
        rbtDayWise.Focus()
    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        btnView_Search.Enabled = False
        GridView2.DataSource = Nothing
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        SelectedCompanyId = GetSelectedCompanyId(chkLstCompany, True)
        StrUseridFtr = ""
        If rbtDayWise.Checked Then
            DayWiseReport()
        Else
            DateWiseReport()
        End If

        btnView_Search.Enabled = True
        Prop_Sets()
    End Sub

    Private Sub DayWiseReport()
        Try
            Dim Costid As String = ""
            If cmbCostCentre.Text <> "ALL" Then
                Costid = GetSqlValue(cn, "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN('" & cmbCostCentre.Text & "')")
            End If
            strSql = vbCrLf + " SELECT DESCRIP DESCRIPTION,VALUE FROM ("
            strSql += vbCrLf + " SELECT DESCRIP + (CASE WHEN TTYPE <>'C' THEN ' ' + (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID=X.METALID) ELSE '' END )DESCRIP"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(100),SUM(VALUE))VALUE,RESULT,(SELECT DISPLAYORDER FROM " & cnAdminDb & "..METALMAST WHERE METALID=X.METALID)DORD FROM ("
            strSql += vbCrLf + " SELECT METALID,SUM(GRSWT)VALUE,'I' TTYPE,'SALES' DESCRIP,1 RESULT "
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE WHERE TRANDATE='" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' "
            If Costid.ToString <> "" Then
                strSql += vbCrLf + " AND COSTID ='" & Costid & "'"
            End If
            strSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'Y'"
            strSql += vbCrLf + " AND TRANTYPE IN ('SA','RD','OD')"
            strSql += vbCrLf + " GROUP BY METALID"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT METALID,SUM(VALUE)VALUE,'I' TTYPE,'SALES' DESCRIP,1 RESULT  FROM ("
            strSql += vbCrLf + " SELECT (SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=ISS.CATCODE )METALID,SUM(ISS.STNWT)VALUE "
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
            strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..ISSSTONE ISS ON ISS.ISSSNO =I.SNO AND ISS.BATCHNO =I.BATCHNO"
            strSql += vbCrLf + " WHERE I.TRANDATE='" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' "
            If Costid.ToString <> "" Then
                strSql += vbCrLf + " AND I.COSTID ='" & Costid & "'"
            End If
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'')<>'Y'"
            strSql += vbCrLf + " AND I.TRANTYPE IN ('SA','RD','OD')"
            strSql += vbCrLf + " AND (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = ISS.STNITEMID) = 'D'"
            strSql += vbCrLf + " GROUP BY ISS.CATCODE)Y GROUP BY METALID"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT METALID,SUM(TAGGRSWT-GRSWT)VALUE,'I' TTYPE,'PARTLY SALES' DESCRIP,2 RESULT "
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE WHERE TRANDATE='" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' "
            If Costid.ToString <> "" Then
                strSql += vbCrLf + " AND COSTID ='" & Costid & "'"
            End If
            strSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'Y'"
            strSql += vbCrLf + " AND ISNULL(TAGGRSWT,0) <> 0 AND ISNULL(TAGGRSWT,0) <> ISNULL(GRSWT,0)"
            strSql += vbCrLf + " AND TRANTYPE IN ('SA','RD','OD')"
            strSql += vbCrLf + " GROUP BY METALID"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT METALID,SUM(GRSWT)VALUE,'R' TTYPE,CASE WHEN TRANTYPE='PU' THEN 'PURCHASE' ELSE 'RETURN' END DESCRIP,CASE WHEN TRANTYPE='PU' THEN 3 ELSE 4 END RESULT "
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE='" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' "
            If Costid.ToString <> "" Then
                strSql += vbCrLf + " AND COSTID ='" & Costid & "'"
            End If
            strSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'Y'"
            strSql += vbCrLf + " AND TRANTYPE IN ('PU','SR')"
            strSql += vbCrLf + " GROUP BY METALID,TRANTYPE"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT METALID,SUM(VALUE)VALUE,'R' TTYPE,CASE WHEN TRANTYPE='PU' THEN 'PURCHASE' ELSE 'RETURN' END DESCRIP,CASE WHEN TRANTYPE='PU' THEN 3 ELSE 4 END RESULT FROM ("
            strSql += vbCrLf + " SELECT (SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=ISS.CATCODE )METALID,SUM(ISS.STNWT)VALUE,I.TRANTYPE "
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT I"
            strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..RECEIPTSTONE ISS ON ISS.ISSSNO =I.SNO AND ISS.BATCHNO =I.BATCHNO"
            strSql += vbCrLf + " WHERE I.TRANDATE='" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' "
            If Costid.ToString <> "" Then
                strSql += vbCrLf + " AND I.COSTID ='" & Costid & "'"
            End If
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'')<>'Y'"
            strSql += vbCrLf + " AND I.TRANTYPE IN ('PU','SR')"
            strSql += vbCrLf + " AND (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = ISS.STNITEMID) = 'D'"
            strSql += vbCrLf + " GROUP BY ISS.CATCODE,I.TRANTYPE)Y GROUP BY METALID,TRANTYPE"
            strSql += vbCrLf + " )X GROUP BY DESCRIP,METALID,TTYPE,RESULT"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'PURCHASE AMOUNT' DESCRIP,CONVERT(VARCHAR(100),SUM(AMOUNT))VALUE,5 RESULT,99 DORD"
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE='" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' "
            If Costid.ToString <> "" Then
                strSql += vbCrLf + " AND COSTID ='" & Costid & "'"
            End If
            strSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'Y' "
            strSql += vbCrLf + " AND TRANTYPE='PU'"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'ADVANCE ADJUSTED' DESCRIP"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(100),SUM(AMOUNT)) VALUE,6 RESULT,99 DORD"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING AS A WHERE "
            strSql += vbCrLf + "  TRANDATE = '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' "
            If Costid.ToString <> "" Then
                strSql += vbCrLf + " AND COSTID ='" & Costid & "'"
            End If
            strSql += vbCrLf + " AND RECPAY  = 'P'"
            strSql += vbCrLf + " AND PAYMODE IN ('AA','AP')"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') <> 'Y'"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT 'CREDIT' DESCRIP "
            strSql += vbCrLf + " ,CONVERT(VARCHAR(100),SUM(AMOUNT)) VALUE,7 RESULT,99 DORD"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING AS A WHERE TRANDATE = '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "'  "
            strSql += vbCrLf + " AND PAYMODE = 'DU' "
            If Costid.ToString <> "" Then
                strSql += vbCrLf + " AND COSTID ='" & Costid & "'"
            End If
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') <> 'Y'"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT DESCRIP"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(100),(CASE WHEN SUM(AMOUNT)< 0  THEN -1*SUM(AMOUNT) ELSE SUM(AMOUNT) END)) VALUE,8 RESULT,99 DORD "
            strSql += vbCrLf + " FROM ("
            strSql += vbCrLf + " SELECT CASE WHEN PAYMODE ='CC' THEN 'CREDIT CARD' WHEN PAYMODE ='CH' THEN 'CHEQUE' ELSE 'CASH' END DESCRIP "
            strSql += vbCrLf + " ,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE -AMOUNT END)) AMOUNT"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN AS A WHERE "
            strSql += vbCrLf + " TRANDATE = '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' "
            If Costid.ToString <> "" Then
                strSql += vbCrLf + " AND COSTID ='" & Costid & "'"
            End If
            strSql += vbCrLf + " AND PAYMODE IN ('CA','CC','CH')"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') <> 'Y'"
            strSql += vbCrLf + " GROUP BY PAYMODE)Z GROUP BY DESCRIP"
            strSql += vbCrLf + " )AA ORDER BY RESULT,DORD"
            DT = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(DT)
            If DT.Rows.Count <= 0 Then
                MsgBox("Record Not Found", MsgBoxStyle.Information)
                Exit Sub
            End If
            GridView2.DataSource = Nothing
            GridView2.DataSource = DT
            GridView2.ReadOnly = True
            GridView2.Columns("DESCRIPTION").Width = 200
            GridView2.Columns("DESCRIPTION").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            GridView2.Columns("VALUE").Width = 150
            GridView2.Columns("VALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Catch e As Exception
            MsgBox(e.Message, MsgBoxStyle.Information)
            MsgBox(e.Message + vbCrLf + e.StackTrace)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub DateWiseReport()
        Try
            Dim Costid As String = ""
            If cmbCostCentre.Text <> "ALL" Then
                Costid = GetSqlValue(cn, "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN('" & cmbCostCentre.Text & "')")
            End If

            strSql = " IF OBJECT_ID ('TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM"
            strSql += vbCrLf + " SELECT TRANDATE,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID=X.METALID)METALNAME"
            strSql += vbCrLf + " ,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,CONVERT(VARCHAR(100),TTYPE)TTYPE "
            strSql += vbCrLf + " ,X.METALID,RESULT"
            strSql += vbCrLf + " ,(SELECT DISPLAYORDER FROM " & cnAdminDb & "..METALMAST WHERE METALID=X.METALID)DORD"
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM"
            strSql += vbCrLf + " FROM ("
            strSql += vbCrLf + " SELECT TRANDATE"
            strSql += vbCrLf + " ,METALID,GRSWT,NETWT,'SALES' TTYPE,1 RESULT"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I WHERE TRANDATE BETWEEN "
            strSql += vbCrLf + " '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTodate.Value.Date.ToString("yyyy-MM-dd") & "' "
            If Costid.ToString <> "" Then
                strSql += vbCrLf + " AND I.COSTID ='" & Costid & "'"
            End If
            strSql += vbCrLf + " And TRANTYPE IN ('SA','OD','RD')  AND ISNULL(I.CANCEL,'')<>'Y'"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT I.TRANDATE"
            strSql += vbCrLf + " ,(SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=ISS.STNITEMID)METALID"
            strSql += vbCrLf + " ,ISS.STNWT GRSWT,ISS.STNWT NETWT,'SALES' TTYPE,1 RESULT"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSSTONE ISS"
            strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..ISSUE I ON I.BATCHNO =ISS.BATCHNO AND I.SNO=ISS.ISSSNO"
            strSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTodate.Value.Date.ToString("yyyy-MM-dd") & "' "
            If Costid.ToString <> "" Then
                strSql += vbCrLf + " AND I.COSTID ='" & Costid & "'"
            End If
            strSql += vbCrLf + " And I.TRANTYPE IN ('SA','OD','RD') AND ISNULL(I.CANCEL,'')<>'Y'"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT TRANDATE"
            strSql += vbCrLf + " ,METALID,GRSWT,NETWT,'PURCHASE' TTYPE,2 RESULT"
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT I WHERE TRANDATE BETWEEN "
            strSql += vbCrLf + " '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTodate.Value.Date.ToString("yyyy-MM-dd") & "' "
            If Costid.ToString <> "" Then
                strSql += vbCrLf + " AND I.COSTID ='" & Costid & "'"
            End If
            strSql += vbCrLf + " And TRANTYPE IN ('PU') AND ISNULL(I.CANCEL,'')<>'Y'"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT I.TRANDATE"
            strSql += vbCrLf + " ,(SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=ISS.STNITEMID)METALID"
            strSql += vbCrLf + " ,ISS.STNWT GRSWT,ISS.STNWT NETWT,'PURCHASE' TTYPE,2 RESULT"
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE ISS"
            strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..RECEIPT I ON I.BATCHNO =ISS.BATCHNO AND I.SNO=ISS.ISSSNO"
            strSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTodate.Value.Date.ToString("yyyy-MM-dd") & "' "
            If Costid.ToString <> "" Then
                strSql += vbCrLf + " AND I.COSTID ='" & Costid & "'"
            End If
            strSql += vbCrLf + " And I.TRANTYPE IN ('PU')  AND ISNULL(I.CANCEL,'')<>'Y'"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT TRANDATE"
            strSql += vbCrLf + " ,METALID,GRSWT,NETWT,'RETURN' TTYPE,3 RESULT"
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT I WHERE TRANDATE BETWEEN "
            strSql += vbCrLf + " '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTodate.Value.Date.ToString("yyyy-MM-dd") & "' "
            If Costid.ToString <> "" Then
                strSql += vbCrLf + " AND I.COSTID ='" & Costid & "'"
            End If
            strSql += vbCrLf + " And TRANTYPE IN ('SR')  AND ISNULL(I.CANCEL,'')<>'Y'"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT I.TRANDATE"
            strSql += vbCrLf + " ,(SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=ISS.STNITEMID)METALID"
            strSql += vbCrLf + " ,ISS.STNWT GRSWT,ISS.STNWT NETWT,'RETURN' TTYPE,3 RESULT"
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE ISS"
            strSql += vbCrLf + " INNER JOIN " & cnStockDb & "..RECEIPT I ON I.BATCHNO =ISS.BATCHNO AND I.SNO=ISS.ISSSNO"
            strSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTodate.Value.Date.ToString("yyyy-MM-dd") & "' "
            If Costid.ToString <> "" Then
                strSql += vbCrLf + " AND I.COSTID ='" & Costid & "'"
            End If
            strSql += vbCrLf + " And I.TRANTYPE IN ('SR')  AND ISNULL(I.CANCEL,'')<>'Y'"
            strSql += vbCrLf + " )X"
            strSql += vbCrLf + " GROUP BY TRANDATE,METALID,TTYPE,RESULT"
            strSql += vbCrLf + " ORDER BY TRANDATE,RESULT,(SELECT DISPLAYORDER FROM " & cnAdminDb & "..METALMAST WHERE METALID=X.METALID)"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()


            strSql = vbCrLf + " IF OBJECT_ID ('TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_AMT') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_AMT"
            strSql += vbCrLf + " SELECT * INTO TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_AMT FROM("
            strSql += vbCrLf + "  SELECT 'ADVANCE' DESCRIP,TRANDATE"
            strSql += vbCrLf + "  ,SUM(AMOUNT) VALUE,6 RESULT,99 DORD"
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..OUTSTANDING AS A WHERE "
            strSql += vbCrLf + "   TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTodate.Value.Date.ToString("yyyy-MM-dd") & "'"
            If Costid.ToString <> "" Then
                strSql += vbCrLf + " AND COSTID ='" & Costid & "'"
            End If
            strSql += vbCrLf + "  AND RECPAY  = 'P'"
            strSql += vbCrLf + "  AND PAYMODE IN ('AA','AP')"
            strSql += vbCrLf + "  AND ISNULL(CANCEL,'') <> 'Y'"
            strSql += vbCrLf + "  GROUP BY TRANDATE"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT 'CREDIT' DESCRIP,TRANDATE "
            strSql += vbCrLf + "  ,SUM(AMOUNT) VALUE,7 RESULT,99 DORD"
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..OUTSTANDING AS A WHERE TRANDATE BETWEEN "
            strSql += vbCrLf + "  '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTodate.Value.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + "  AND PAYMODE = 'DU' "
            If Costid.ToString <> "" Then
                strSql += vbCrLf + " AND COSTID ='" & Costid & "'"
            End If
            strSql += vbCrLf + "  AND ISNULL(CANCEL,'') <> 'Y'"
            strSql += vbCrLf + "  GROUP BY TRANDATE"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT DESCRIP,TRANDATE"
            strSql += vbCrLf + "  ,(CASE WHEN SUM(AMOUNT)< 0  THEN -1*SUM(AMOUNT) ELSE SUM(AMOUNT) END) VALUE,8 RESULT,99 DORD "
            strSql += vbCrLf + "  FROM ("
            strSql += vbCrLf + "  SELECT CASE WHEN PAYMODE ='CC' THEN 'CARD' WHEN PAYMODE ='CH' THEN 'CHEQUE' ELSE 'CASH' END DESCRIP "
            strSql += vbCrLf + "  ,SUM((CASE WHEN A.TRANMODE = 'C' THEN AMOUNT ELSE -AMOUNT END)) AMOUNT,TRANDATE"
            strSql += vbCrLf + "  FROM " & cnStockDb & "..ACCTRAN AS A WHERE "
            strSql += vbCrLf + "  TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTodate.Value.Date.ToString("yyyy-MM-dd") & "'"
            If Costid.ToString <> "" Then
                strSql += vbCrLf + " AND COSTID ='" & Costid & "'"
            End If
            strSql += vbCrLf + "  AND PAYMODE IN ('CA','CC','CH')"
            strSql += vbCrLf + "  AND ISNULL(CANCEL,'') <> 'Y'"
            strSql += vbCrLf + "  GROUP BY PAYMODE,TRANDATE)Z GROUP BY DESCRIP,TRANDATE)Y ORDER BY TRANDATE"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()


            strSql = vbCrLf + " IF OBJECT_ID ('TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_RES') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_RES"
            strSql += vbCrLf + " SELECT IDENTITY(INT,1,1) TSNO,* INTO TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_RES FROM ( "
            strSql += vbCrLf + " SELECT DISTINCT CONVERT(VARCHAR(12),TRANDATE,105) PARTICULAR,TRANDATE TTRANDATE FROM "
            strSql += vbCrLf + " TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM "
            strSql += vbCrLf + " )X ORDER BY TTRANDATE"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " IF OBJECT_ID ('TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_RES') IS NOT NULL"
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + "    INSERT INTO TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_RES (PARTICULAR,TTRANDATE) VALUES ('TOTAL',NULL) "
            strSql += vbCrLf + " END"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " IF OBJECT_ID ('TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_RES') IS NOT NULL"
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + " 	DECLARE @METALNAME VARCHAR(100)"
            strSql += vbCrLf + " 	DECLARE @TTYPE VARCHAR(100)"
            strSql += vbCrLf + " 	DECLARE @DORD INT"
            strSql += vbCrLf + " 	DECLARE @RESULT INT"
            strSql += vbCrLf + " 	DECLARE @COLGNAME VARCHAR(500)"
            strSql += vbCrLf + " 	DECLARE @COLNNAME VARCHAR(500)"
            strSql += vbCrLf + " 	DECLARE @QRY VARCHAR(500)"
            strSql += vbCrLf + " 	DECLARE CUR_COLLECTWT CURSOR"
            strSql += vbCrLf + " 	FOR SELECT DISTINCT METALNAME,TTYPE,DORD,RESULT FROM TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM X ORDER BY RESULT,DORD"
            strSql += vbCrLf + " 	OPEN CUR_COLLECTWT"
            strSql += vbCrLf + " 	FETCH NEXT FROM CUR_COLLECTWT INTO @METALNAME,@TTYPE,@DORD,@RESULT"
            strSql += vbCrLf + " 	WHILE @@FETCH_STATUS = 0"
            strSql += vbCrLf + " 	BEGIN"
            strSql += vbCrLf + " 		SET @COLGNAME=''"
            strSql += vbCrLf + " 		SET @COLGNAME= @TTYPE + '_' + @METALNAME + '_GRSWT' "
            strSql += vbCrLf + " 		SET @COLNNAME=''"
            strSql += vbCrLf + " 		SET @COLNNAME= @TTYPE + '_' + @METALNAME + '_NETWT'"

            strSql += vbCrLf + " 		SET @QRY ='ALTER TABLE TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_RES ADD '+@COLGNAME+' NUMERIC(15,3)'"
            strSql += vbCrLf + " 		PRINT @QRY"
            strSql += vbCrLf + " 		EXEC (@QRY)"
            strSql += vbCrLf + " 		SET @QRY ='ALTER TABLE TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_RES ADD '+@COLNNAME+' NUMERIC(15,3)'"
            strSql += vbCrLf + " 		PRINT @QRY"
            strSql += vbCrLf + " 		EXEC (@QRY)"

            strSql += vbCrLf + " 		SET @QRY ='UPDATE A SET [' +@COLGNAME + ']='"
            strSql += vbCrLf + " 		SET @QRY = @QRY + CHAR(13) +  '(SELECT SUM(GRSWT) FROM TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM WHERE TRANDATE=A.TTRANDATE AND METALNAME='''+ @METALNAME +''''"
            strSql += vbCrLf + " 		SET @QRY = @QRY + CHAR(13) +  ' AND TTYPE='''+ @TTYPE +''' )'"
            strSql += vbCrLf + " 		SET @QRY = @QRY + CHAR(13) +  ' FROM TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_RES A '"
            strSql += vbCrLf + " 		PRINT @QRY"
            strSql += vbCrLf + " 		EXEC (@QRY)"

            ''TOTAL
            strSql += vbCrLf + " 		SET @QRY ='UPDATE A SET [' +@COLGNAME + ']='"
            strSql += vbCrLf + " 		SET @QRY = @QRY + CHAR(13) +  '(SELECT SUM(GRSWT) FROM TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM WHERE METALNAME='''+ @METALNAME +''''"
            strSql += vbCrLf + " 		SET @QRY = @QRY + CHAR(13) +  ' AND TTYPE='''+ @TTYPE +''' )'"
            strSql += vbCrLf + " 		SET @QRY = @QRY + CHAR(13) +  ' FROM TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_RES A WHERE PARTICULAR =''TOTAL'' '"
            strSql += vbCrLf + " 		PRINT @QRY"
            strSql += vbCrLf + " 		EXEC (@QRY)"
            ''END TOTAL

            strSql += vbCrLf + " 		SET @QRY ='UPDATE A SET [' +@COLNNAME + ']='"
            strSql += vbCrLf + " 		SET @QRY = @QRY + CHAR(13) +  '(SELECT SUM(NETWT) FROM TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM WHERE TRANDATE=A.TTRANDATE AND METALNAME='''+ @METALNAME +''''"
            strSql += vbCrLf + " 		SET @QRY = @QRY + CHAR(13) +  ' AND TTYPE='''+ @TTYPE +''' )'"
            strSql += vbCrLf + " 		SET @QRY = @QRY + CHAR(13) +  ' FROM TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_RES A '"
            strSql += vbCrLf + " 		PRINT @QRY"
            strSql += vbCrLf + " 		EXEC (@QRY)"

            ''TOTAL
            strSql += vbCrLf + " 		SET @QRY ='UPDATE A SET [' +@COLNNAME + ']='"
            strSql += vbCrLf + " 		SET @QRY = @QRY + CHAR(13) +  '(SELECT SUM(NETWT) FROM TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM WHERE METALNAME='''+ @METALNAME +''''"
            strSql += vbCrLf + " 		SET @QRY = @QRY + CHAR(13) +  ' AND TTYPE='''+ @TTYPE +''' )'"
            strSql += vbCrLf + " 		SET @QRY = @QRY + CHAR(13) +  ' FROM TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_RES A WHERE PARTICULAR =''TOTAL'' '"
            strSql += vbCrLf + " 		PRINT @QRY"
            strSql += vbCrLf + " 		EXEC (@QRY)"
            ''END TOTAL

            strSql += vbCrLf + " 		FETCH NEXT FROM CUR_COLLECTWT INTO @METALNAME,@TTYPE,@DORD,@RESULT"
            strSql += vbCrLf + " 	END"
            strSql += vbCrLf + " 	CLOSE CUR_COLLECTWT"
            strSql += vbCrLf + " 	DEALLOCATE CUR_COLLECTWT"


            strSql += vbCrLf + " 	SET @QRY ='ALTER TABLE TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_RES ADD [ADVANCE ADJ] NUMERIC(15,2)'"
            strSql += vbCrLf + " 	SET @QRY =@QRY + CHAR(13) +  ' ALTER TABLE TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_RES ADD CREDIT NUMERIC(15,2)'"
            strSql += vbCrLf + " 	SET @QRY =@QRY + CHAR(13) +  ' ALTER TABLE TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_RES ADD CASH NUMERIC(15,2)'"
            strSql += vbCrLf + " 	SET @QRY =@QRY + CHAR(13) +  ' ALTER TABLE TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_RES ADD CARD NUMERIC(15,2)'"
            strSql += vbCrLf + " 	SET @QRY =@QRY + CHAR(13) +  ' ALTER TABLE TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_RES ADD CHEQUE NUMERIC(15,2)'"
            strSql += vbCrLf + " 	PRINT @QRY"
            strSql += vbCrLf + " 	EXEC (@QRY)"

            strSql += vbCrLf + " 	SET @QRY ='UPDATE A SET [ADVANCE ADJ]='"
            strSql += vbCrLf + " 	SET @QRY = @QRY + CHAR(13) +  '(SELECT SUM(VALUE) FROM TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_AMT WHERE TRANDATE=A.TTRANDATE AND DESCRIP=''ADVANCE'')'"
            strSql += vbCrLf + " 	SET @QRY = @QRY + CHAR(13) +  ' FROM TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_RES A '"
            strSql += vbCrLf + " 	PRINT @QRY"
            strSql += vbCrLf + " 	EXEC (@QRY)"
            strSql += vbCrLf + " 	SET @QRY ='UPDATE A SET [CREDIT]='"
            strSql += vbCrLf + " 	SET @QRY = @QRY + CHAR(13) +  '(SELECT SUM(VALUE) FROM TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_AMT WHERE TRANDATE=A.TTRANDATE AND DESCRIP=''CREDIT'')'"
            strSql += vbCrLf + " 	SET @QRY = @QRY + CHAR(13) +  ' FROM TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_RES A '"
            strSql += vbCrLf + " 	PRINT @QRY"
            strSql += vbCrLf + " 	EXEC (@QRY)"
            strSql += vbCrLf + " 	SET @QRY ='UPDATE A SET [CASH]='"
            strSql += vbCrLf + " 	SET @QRY = @QRY + CHAR(13) +  '(SELECT SUM(VALUE) FROM TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_AMT WHERE TRANDATE=A.TTRANDATE AND DESCRIP=''CASH'')'"
            strSql += vbCrLf + " 	SET @QRY = @QRY + CHAR(13) +  ' FROM TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_RES A '"
            strSql += vbCrLf + " 	PRINT @QRY"
            strSql += vbCrLf + " 	EXEC (@QRY)"
            strSql += vbCrLf + " 	SET @QRY ='UPDATE A SET [CARD]='"
            strSql += vbCrLf + " 	SET @QRY = @QRY + CHAR(13) +  '(SELECT SUM(VALUE) FROM TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_AMT WHERE TRANDATE=A.TTRANDATE AND DESCRIP=''CARD'')'"
            strSql += vbCrLf + " 	SET @QRY = @QRY + CHAR(13) +  ' FROM TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_RES A '"
            strSql += vbCrLf + " 	PRINT @QRY"
            strSql += vbCrLf + " 	EXEC (@QRY)"
            strSql += vbCrLf + " 	SET @QRY ='UPDATE A SET [CHEQUE]='"
            strSql += vbCrLf + " 	SET @QRY = @QRY + CHAR(13) +  '(SELECT SUM(VALUE) FROM TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_AMT WHERE TRANDATE=A.TTRANDATE AND DESCRIP=''CHEQUE'')'"
            strSql += vbCrLf + " 	SET @QRY = @QRY + CHAR(13) +  ' FROM TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_RES A '"
            strSql += vbCrLf + " 	PRINT @QRY"
            strSql += vbCrLf + " 	EXEC (@QRY)"

            ''TOTAL
            strSql += vbCrLf + " 	SET @QRY ='UPDATE A SET [ADVANCE ADJ]='"
            strSql += vbCrLf + " 	SET @QRY = @QRY + CHAR(13) +  '(SELECT SUM(VALUE) FROM TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_AMT WHERE DESCRIP=''ADVANCE'')'"
            strSql += vbCrLf + " 	SET @QRY = @QRY + CHAR(13) +  ' FROM TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_RES A WHERE PARTICULAR =''TOTAL'' '"
            strSql += vbCrLf + " 	PRINT @QRY"
            strSql += vbCrLf + " 	EXEC (@QRY)"
            strSql += vbCrLf + " 	SET @QRY ='UPDATE A SET [CREDIT]='"
            strSql += vbCrLf + " 	SET @QRY = @QRY + CHAR(13) +  '(SELECT SUM(VALUE) FROM TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_AMT WHERE DESCRIP=''CREDIT'')'"
            strSql += vbCrLf + " 	SET @QRY = @QRY + CHAR(13) +  ' FROM TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_RES A WHERE PARTICULAR =''TOTAL'' '"
            strSql += vbCrLf + " 	PRINT @QRY"
            strSql += vbCrLf + " 	EXEC (@QRY)"
            strSql += vbCrLf + " 	SET @QRY ='UPDATE A SET [CASH]='"
            strSql += vbCrLf + " 	SET @QRY = @QRY + CHAR(13) +  '(SELECT SUM(VALUE) FROM TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_AMT WHERE DESCRIP=''CASH'')'"
            strSql += vbCrLf + " 	SET @QRY = @QRY + CHAR(13) +  ' FROM TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_RES A WHERE PARTICULAR =''TOTAL'' '"
            strSql += vbCrLf + " 	PRINT @QRY"
            strSql += vbCrLf + " 	EXEC (@QRY)"
            strSql += vbCrLf + " 	SET @QRY ='UPDATE A SET [CARD]='"
            strSql += vbCrLf + " 	SET @QRY = @QRY + CHAR(13) +  '(SELECT SUM(VALUE) FROM TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_AMT WHERE DESCRIP=''CARD'')'"
            strSql += vbCrLf + " 	SET @QRY = @QRY + CHAR(13) +  ' FROM TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_RES A WHERE PARTICULAR =''TOTAL'' '"
            strSql += vbCrLf + " 	PRINT @QRY"
            strSql += vbCrLf + " 	EXEC (@QRY)"
            strSql += vbCrLf + " 	SET @QRY ='UPDATE A SET [CHEQUE]='"
            strSql += vbCrLf + " 	SET @QRY = @QRY + CHAR(13) +  '(SELECT SUM(VALUE) FROM TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_AMT WHERE DESCRIP=''CHEQUE'')'"
            strSql += vbCrLf + " 	SET @QRY = @QRY + CHAR(13) +  ' FROM TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_RES A WHERE PARTICULAR =''TOTAL'' '"
            strSql += vbCrLf + " 	PRINT @QRY"
            strSql += vbCrLf + " 	EXEC (@QRY)"
            ''END TOTAL
            strSql += vbCrLf + " END"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()


            strSql = vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "DAILYCOLLECTIONSUMM_RES ORDER BY TSNO,TTRANDATE"
            DT = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(DT)
            If DT.Rows.Count <= 0 Then
                MsgBox("Record Not Found", MsgBoxStyle.Information)
                Exit Sub
            End If
            GridView2.DataSource = Nothing
            GridView2.DataSource = DT
            GridView2.ReadOnly = True
            For Each dcc As DataGridViewColumn In GridView2.Columns
                If dcc.HeaderText.Contains("_NETWT") Then
                    dcc.Visible = False
                ElseIf dcc.HeaderText.Contains("_GRSWT") Then
                    dcc.HeaderText = dcc.HeaderText.ToString.Replace("_GRSWT", "WT")
                    dcc.HeaderText = dcc.HeaderText.ToString.Replace("_", " ")
                    dcc.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dcc.Width = 100
                Else
                    dcc.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dcc.Width = 100
                End If
            Next
            For cnt As Integer = 0 To GridView2.Rows.Count - 1
                If GridView2.Rows(cnt).Cells("PARTICULAR").Value.ToString = "TOTAL" Then
                    GridView2.Rows(cnt).DefaultCellStyle.BackColor = Color.Lavender
                    GridView2.Rows(cnt).DefaultCellStyle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold)
                End If
            Next
            GridView2.Columns("TTRANDATE").Visible = False
            GridView2.Columns("TSNO").Visible = False
            GridView2.Columns("PARTICULAR").Width = 200
            GridView2.Columns("PARTICULAR").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

        Catch e As Exception
            MsgBox(e.Message, MsgBoxStyle.Information)
            MsgBox(e.Message + vbCrLf + e.StackTrace)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub


    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub frmDailyTransactionCollection_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If GridView2.Rows.Count > 0 Then
            Dim title As String
            title = "Daily Collection Report"
            If rbtDayWise.Checked Then
                title += " For Date : " & dtpFrom.Text & "  " & GetServerTime().ToString("hh:mm:ss:tt") & " ."
            Else
                title += " From Date : " & dtpFrom.Text & " To Date : " & dtpTodate.Text & "  " & GetServerTime().ToString("hh:mm:ss:tt") & " ."
            End If

            If cmbCostCentre.Text <> "ALL" And cmbCostCentre.Text <> "" Then title += " Costcentre : " & cmbCostCentre.Text & " ."
            BrightPosting.GExport.Post(Me.Name, strCompanyName, title, GridView2, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If GridView2.Rows.Count > 0 Then
            Dim title As String
            title = "Daily Collection Report"
            If rbtDayWise.Checked Then
                title += " For Date : " & dtpFrom.Text & "  " & GetServerTime().ToString("hh:mm:ss:tt") & " ."
            Else
                title += " From Date : " & dtpFrom.Text & " To Date : " & dtpTodate.Text & "  " & GetServerTime().ToString("hh:mm:ss:tt") & " ."
            End If
            If cmbCostCentre.Text <> "ALL" And cmbCostCentre.Text <> "" Then title += " Costcentre : " & cmbCostCentre.Text & " ."
            BrightPosting.GExport.Post(Me.Name, strCompanyName, title, GridView2, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub rbtDayWise_CheckedChanged(sender As Object, e As EventArgs) Handles rbtDayWise.CheckedChanged
        If rbtDayWise.Checked Then
            lblTodate.Visible = False
            dtpTodate.Visible = False
            lblFromDate.Text = "Date"
        Else
            lblTodate.Visible = True
            dtpTodate.Visible = True
            lblFromDate.Text = "From Date"
        End If
    End Sub

    Private Sub AutoReziseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AutoReziseToolStripMenuItem.Click
        If GridView2.RowCount > 0 Then
            If AutoReziseToolStripMenuItem.Checked Then
                GridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                GridView2.Invalidate()
                For Each dgvCol As DataGridViewColumn In GridView2.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                GridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In GridView2.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                GridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
        End If
    End Sub
End Class