Imports System.Data.OleDb
Imports System.Drawing
Imports System.Data.SqlClient

Public Class frmIssueReceiptView    
    Dim blagr_int As New AGR_Initializer
    Dim da As OleDbDataAdapter
    Dim strSql As String
    Dim dt, dtcost, dtmetal As New DataTable
    Dim costid As String = "ALL"
    Dim dCostid As String = "ALL"
    Dim OLtran As OleDbTransaction
    Dim BatchNo As String = Nothing
    Dim TRANNO As String = Nothing
    Dim Connectionmode As String
    Dim cmd As New OleDbCommand
    Dim tran As OleDbTransaction
    Public trantodate As System.DateTime
    Public BillDate As Date
    Dim prnmemsuffix As String = ""
    Dim dtcmb As New DataTable
    Dim _dtins As New DataTable
    Dim issno As String = "" : Dim recsno As String = "" : Dim jjfrmno As String = "" : Dim rectranno As String = "" : Dim acbatchno As String = ""
    Dim bagno As String
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

        Private Sub frmIssueReceiptView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            'If txt_Gridcell.Focused Or gridview.Focused Or grdcmb.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub Frmclose()
        Me.Close()
    End Sub

    Private Sub frmIssueReceiptView_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Leave

    End Sub
    Private Sub frmIssueReceiptView_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funnew()
        Panel1.Visible = False
    End Sub
    Private Sub funcReceiptIssue()

        Dim trantype As String = ""
        Dim Apptrantype As String = ""
        Dim temptableRec As String = "TEMPTABLEDB..TEMP" & systemId & "SMITHREC"
        Dim temptableIss As String = "TEMPTABLEDB..TEMP" & systemId & "SMITHISS"
        If Trim(cmbcostcentre_OWN.Text.ToString) = "" Or Trim(cmbcostcentre_OWN.Text.ToString) = "ALL" Then costid = "ALL" Else costid = "'" & cmbcostcentre_OWN.SelectedValue & "'"
        If rbtIssue.Checked Then
            trantype = "'IIN'"
        ElseIf rbtReceipt.Checked Then
            trantype = "'RIN'"
        Else
            trantype = "'RIN','IIN'"
        End If

        strSql = vbCrLf + "  IF OBJECT_ID('" & temptableRec & "','U')IS NOT NULL DROP TABLE " & temptableRec & ""
        strSql += vbCrLf + "  SELECT "
        strSql += vbCrLf + "  ' '+(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = I.ACCODE) PARTICULARS"
        strSql += vbCrLf + "  ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID=I.METALID)METAL"
        strSql += vbCrLf + "  ,(SELECT CASE WHEN CATGROUP='B' THEN 'BAR' WHEN CATGROUP='L' THEN 'OLD' WHEN CATGROUP='R' THEN 'REPAIR'"
        strSql += vbCrLf + "  WHEN CATGROUP='O' THEN 'ORDER' WHEN CATGROUP='P' THEN 'ORNAMENTS' ELSE 'PARTYMETALS' END"
        strSql += vbCrLf + "  FROM " & cnAdminDb & "..CATEGORY  WHERE CATCODE =I.CATCODE)CATEGORY"
        strSql += vbCrLf + "  ,TRANNO,CONVERT(VARCHAR,TRANDATE,103)TDATE"
        strSql += vbCrLf + "  ,REFNO BILLNO,REFDATE BILLDATE"
        strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = I.ACCODE) ACNAME"
        strSql += vbCrLf + "  ,CASE TRANTYPE  WHEN 'RRE' THEN 'RECEIPT' WHEN 'RAP' THEN 'APPROVAL RECEIPT' WHEN 'ROT' THEN 'OTHER RECEIPT' WHEN 'RPU' THEN 'PURCHASE' WHEN 'RIN' THEN 'INTERNAL TRANSFER' "
        strSql += vbCrLf + "   ELSE TRANTYPE END AS TRANTYPE"
        strSql += vbCrLf + "  ,CASE WHEN ITEMID = 0 THEN (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE) ELSE (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID) END AS [DESCRIPTION]"
        strSql += vbCrLf + "  ,CONVERT(INT,PCS)PCS,CONVERT(NUMERIC(15,3),GRSWT)GRSWT,CONVERT(NUMERIC(15,3),NETWT)NETWT,CONVERT(NUMERIC(15,3),LESSWT)LESSWT,CONVERT(NUMERIC(15,3),PUREWT)PUREWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),ALLOY)ALLOY,CONVERT(NUMERIC(15,3),WASTAGE)WASTAGE,CONVERT(NUMERIC(15,2),MCHARGE)MC,CONVERT(NUMERIC(15,2),AMOUNT )AMOUNT,CONVERT(NUMERIC(15,2),TAX )VAT,MRKFLAG,REMARK1,REMARK2"
        strSql += vbCrLf + "  ,TRANDATE,4 RESULT,'  'COLHEAD,BATCHNO,COSTID,CONVERT(VARCHAR(1),'R')TYPE,CANCEL,TRANTYPE TRANTYPE1,REFNO,REFDATE,CONVERT(VARCHAR(20),SNO) SNO"
        strSql += vbCrLf + "  INTO " & temptableRec & ""
        strSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT AS I"
        If chkAsOnDate.Checked = False Then
            strSql += vbCrLf + "  WHERE TRANDATE BETWEEN '" & dtpfromdate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtptodate.Value.ToString("yyyy-MM-dd") & "'"
        Else
            strSql += vbCrLf + "  WHERE TRANDATE <= '" & dtpfromdate.Value.ToString("yyyy-MM-dd") & "'"
        End If
        strSql += vbCrLf + "  AND LEN(TRANTYPE) > 2"
        strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
        If rbtmark.Checked = True Then
            strSql += vbCrLf + " AND ISNULL(MRKFLAG,'')<>'' "
        ElseIf rbtunmark.Checked = True Then
            strSql += vbCrLf + " AND ISNULL(MRKFLAG,'')='' "
        End If
        strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID IN (" & costid & "))"
        strSql += vbCrLf + " AND I.METALID IN ('" & cmbmetal_OWN.SelectedValue.ToString & "') "
        If trantype <> "" Then strSql += vbCrLf + " AND TRANTYPE IN(" & trantype & ")"
        If Apptrantype <> "" Then strSql += vbCrLf + " AND TRANTYPE NOT IN(" & Apptrantype & ")"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " IF (SELECT COUNT(*) FROM " & temptableRec & ")>0"
        strSql += vbCrLf + " BEGIN"

        strSql += vbCrLf + " INSERT INTO " & temptableRec & "(DESCRIPTION,RESULT,COLHEAD)"
        strSql += vbCrLf + " SELECT '          RECEIPT',0,'H'"

        strSql += vbCrLf + " INSERT INTO " & temptableRec & "(PARTICULARS,METAL,RESULT,COLHEAD)"
        strSql += vbCrLf + " SELECT DISTINCT METAL,METAL,1,'T' FROM " & temptableRec & " WHERE COLHEAD=''"

        strSql += vbCrLf + " INSERT INTO " & temptableRec & "(PARTICULARS,CATEGORY,METAL,RESULT,COLHEAD)"
        strSql += vbCrLf + " SELECT DISTINCT ' ' + CATEGORY,CATEGORY,METAL,2,'T1' FROM " & temptableRec & " WHERE COLHEAD=''"

        strSql += vbCrLf + " INSERT INTO " & temptableRec & "(PARTICULARS,CATEGORY,METAL,RESULT,COLHEAD"
        strSql += vbCrLf + " ,PCS,GRSWT,NETWT,LESSWT,PUREWT,STNWT,DIAWT,ALLOY,WASTAGE,MC,AMOUNT,VAT)"
        strSql += vbCrLf + " SELECT DISTINCT 'TOTAL',CATEGORY,METAL,5,'G'"
        strSql += vbCrLf + " ,SUM(ISNULL(PCS,0)),SUM(ISNULL(GRSWT,0)),SUM(ISNULL(NETWT,0)),SUM(ISNULL(LESSWT,0))"
        strSql += vbCrLf + " ,SUM(ISNULL(PUREWT,0)) ,SUM(ISNULL(STNWT,0)) ,SUM(ISNULL(DIAWT,0)) "
        strSql += vbCrLf + " ,SUM(ISNULL(ALLOY,0)) ,SUM(ISNULL(WASTAGE,0)) ,SUM(ISNULL(MC,0)) "
        strSql += vbCrLf + " ,SUM(ISNULL(AMOUNT,0)) ,SUM(ISNULL(VAT,0))"
        strSql += vbCrLf + " FROM " & temptableRec & " WHERE COLHEAD='' GROUP BY CATEGORY,METAL "

        strSql += vbCrLf + " INSERT INTO " & temptableRec & "(PARTICULARS,METAL,CATEGORY,RESULT,COLHEAD"
        strSql += vbCrLf + " ,PCS,GRSWT,NETWT,LESSWT,PUREWT,STNWT,DIAWT,ALLOY,WASTAGE,MC,AMOUNT,VAT)"
        strSql += vbCrLf + " SELECT DISTINCT METAL,METAL,'ZZZZZ',6,'G'"
        strSql += vbCrLf + " ,SUM(ISNULL(PCS,0)),SUM(ISNULL(GRSWT,0)),SUM(ISNULL(NETWT,0)),SUM(ISNULL(LESSWT,0))"
        strSql += vbCrLf + " ,SUM(ISNULL(PUREWT,0)) ,SUM(ISNULL(STNWT,0)) ,SUM(ISNULL(DIAWT,0)) "
        strSql += vbCrLf + " ,SUM(ISNULL(ALLOY,0)) ,SUM(ISNULL(WASTAGE,0)) ,SUM(ISNULL(MC,0)) "
        strSql += vbCrLf + " ,SUM(ISNULL(AMOUNT,0)) ,SUM(ISNULL(VAT,0))"
        strSql += vbCrLf + " FROM " & temptableRec & " WHERE COLHEAD='' GROUP BY METAL "

        strSql += vbCrLf + " INSERT INTO " & temptableRec & "(PARTICULARS,METAL,CATEGORY,RESULT,COLHEAD"
        strSql += vbCrLf + " ,PCS,GRSWT,NETWT,LESSWT,PUREWT,STNWT,DIAWT,ALLOY,WASTAGE,MC,AMOUNT,VAT)"
        strSql += vbCrLf + " SELECT DISTINCT 'TOTAL','ZZZZZ','ZZZZZ',7,'G'"
        strSql += vbCrLf + " ,SUM(ISNULL(PCS,0)),SUM(ISNULL(GRSWT,0)),SUM(ISNULL(NETWT,0)),SUM(ISNULL(LESSWT,0))"
        strSql += vbCrLf + " ,SUM(ISNULL(PUREWT,0)) ,SUM(ISNULL(STNWT,0)) ,SUM(ISNULL(DIAWT,0)) "
        strSql += vbCrLf + " ,SUM(ISNULL(ALLOY,0)) ,SUM(ISNULL(WASTAGE,0)) ,SUM(ISNULL(MC,0)) "
        strSql += vbCrLf + " ,SUM(ISNULL(AMOUNT,0)) ,SUM(ISNULL(VAT,0))"
        strSql += vbCrLf + " FROM " & temptableRec & " WHERE COLHEAD='' "
        strSql += vbCrLf + " UPDATE " & temptableRec & " SET PCS=NULL WHERE ISNULL(PCS,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableRec & " SET GRSWT=NULL WHERE ISNULL(GRSWT,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableRec & " SET NETWT=NULL WHERE ISNULL(NETWT,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableRec & " SET LESSWT=NULL WHERE ISNULL(LESSWT,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableRec & " SET PUREWT=NULL WHERE ISNULL(PUREWT,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableRec & " SET STNWT=NULL WHERE ISNULL(STNWT,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableRec & " SET DIAWT=NULL WHERE ISNULL(DIAWT,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableRec & " SET ALLOY=NULL WHERE ISNULL(ALLOY,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableRec & " SET WASTAGE=NULL WHERE ISNULL(WASTAGE,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableRec & " SET MC=NULL WHERE ISNULL(MC,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableRec & " SET AMOUNT=NULL WHERE ISNULL(AMOUNT,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableRec & " SET VAT=NULL WHERE ISNULL(VAT,0)=0 "
        strSql += vbCrLf + "  END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + "  IF OBJECT_ID('" & temptableIss & "','U')IS NOT NULL DROP TABLE " & temptableIss & ""
        strSql += vbCrLf + "  SELECT "
        strSql += vbCrLf + "  ' '+(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = I.ACCODE) PARTICULARS"
        strSql += vbCrLf + "  ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID=I.METALID)METAL"
        strSql += vbCrLf + "  ,(SELECT CASE WHEN CATGROUP='B' THEN 'BAR' WHEN CATGROUP='L' THEN 'OLD' WHEN CATGROUP='R' THEN 'REPAIR'"
        strSql += vbCrLf + "  WHEN CATGROUP='O' THEN 'ORDER' WHEN CATGROUP='P' THEN 'ORNAMENTS' ELSE 'PARTYMETALS' END"
        strSql += vbCrLf + "  FROM " & cnAdminDb & "..CATEGORY  WHERE CATCODE =I.CATCODE)CATEGORY"
        strSql += vbCrLf + "  ,TRANNO,CONVERT(VARCHAR,TRANDATE,103)TDATE"
        strSql += vbCrLf + "  ,REFNO BILLNO"
        strSql += vbCrLf + "  ,REFDATE BILLDATE"
        strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = I.ACCODE) ACNAME"
        strSql += vbCrLf + "  ,CASE TRANTYPE WHEN 'IIS' THEN 'ISSUE' WHEN 'IAP' THEN 'APPROVAL ISSUE' WHEN 'IOT' THEN 'OTHER ISSUE' WHEN 'IPU' THEN 'PURCHASE RETURN' WHEN 'IIN' THEN 'INTERNAL TRANSFER'"
        strSql += vbCrLf + "   ELSE TRANTYPE END AS TRANTYPE"
        strSql += vbCrLf + "  ,CASE WHEN ITEMID = 0 THEN (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE) ELSE (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID) END AS [DESCRIPTION]"
        strSql += vbCrLf + "  ,CONVERT(INT,PCS)PCS,CONVERT(NUMERIC(15,3),GRSWT)GRSWT,CONVERT(NUMERIC(15,3),NETWT)NETWT,CONVERT(NUMERIC(15,3),LESSWT)LESSWT,CONVERT(NUMERIC(15,3),PUREWT)PUREWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),ALLOY)ALLOY,CONVERT(NUMERIC(15,3),WASTAGE)WASTAGE,CONVERT(NUMERIC(15,2),MCHARGE)MC,CONVERT(NUMERIC(15,2),AMOUNT )AMOUNT,CONVERT(NUMERIC(15,2),TAX )VAT,MRKFLAG,REMARK1,REMARK2"
        strSql += vbCrLf + "  ,TRANDATE,14 RESULT,'  ' COLHEAD,BATCHNO,COSTID,CONVERT(VARCHAR(1),'I')TYPE,CANCEL,TRANTYPE AS TRANTYPE1,REFNO,REFDATE,CONVERT(VARCHAR(20),SNO) SNO"
        strSql += vbCrLf + "  INTO " & temptableIss & ""
        strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE AS I"
        If chkAsOnDate.Checked = False Then
            strSql += vbCrLf + "  WHERE TRANDATE BETWEEN '" & dtpfromdate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtptodate.Value.ToString("yyyy-MM-dd") & "'"
        Else
            strSql += vbCrLf + "  WHERE TRANDATE <= '" & dtpfromdate.Value.ToString("yyyy-MM-dd") & "'"
        End If
        strSql += vbCrLf + "  AND LEN(TRANTYPE) > 2"
        strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
        If rbtmark.Checked = True Then
            strSql += vbCrLf + " AND ISNULL(MRKFLAG,'')<>'' "
        ElseIf rbtunmark.Checked = True Then
            strSql += vbCrLf + " AND ISNULL(MRKFLAG,'')='' "
        End If
        strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID IN (" & costid & "))"
        strSql += vbCrLf + " AND I.METALID IN ('" & cmbmetal_OWN.SelectedValue.ToString & "') "
        If trantype <> "" Then strSql += vbCrLf + " AND TRANTYPE IN(" & trantype & ")"
        If Apptrantype <> "" Then strSql += vbCrLf + " AND TRANTYPE NOT IN(" & Apptrantype & ")"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " IF (SELECT COUNT(*) FROM " & temptableIss & ")>0"
        strSql += vbCrLf + " BEGIN"

        strSql += vbCrLf + " INSERT INTO " & temptableIss & "(DESCRIPTION,RESULT,COLHEAD)"
        strSql += vbCrLf + " SELECT '          ISSUE',10,'H'"

        strSql += vbCrLf + " INSERT INTO " & temptableIss & "(PARTICULARS,METAL,RESULT,COLHEAD)"
        strSql += vbCrLf + " SELECT DISTINCT METAL,METAL,11,'T' FROM " & temptableIss & " WHERE COLHEAD=''"

        strSql += vbCrLf + " INSERT INTO " & temptableIss & "(PARTICULARS,CATEGORY,METAL,RESULT,COLHEAD)"
        strSql += vbCrLf + " SELECT DISTINCT ' ' + CATEGORY,CATEGORY,METAL,12,'T1' FROM " & temptableIss & " WHERE COLHEAD=''"

        strSql += vbCrLf + " INSERT INTO " & temptableIss & "(PARTICULARS,CATEGORY,METAL,RESULT,COLHEAD"
        strSql += vbCrLf + " ,PCS,GRSWT,NETWT,LESSWT,PUREWT,STNWT,DIAWT,ALLOY,WASTAGE,MC,AMOUNT,VAT)"
        strSql += vbCrLf + " SELECT DISTINCT 'TOTAL',CATEGORY,METAL,15,'G'"
        strSql += vbCrLf + " ,SUM(ISNULL(PCS,0)),SUM(ISNULL(GRSWT,0)),SUM(ISNULL(NETWT,0)),SUM(ISNULL(LESSWT,0))"
        strSql += vbCrLf + " ,SUM(ISNULL(PUREWT,0)) ,SUM(ISNULL(STNWT,0)) ,SUM(ISNULL(DIAWT,0)) "
        strSql += vbCrLf + " ,SUM(ISNULL(ALLOY,0)) ,SUM(ISNULL(WASTAGE,0)) ,SUM(ISNULL(MC,0)) "
        strSql += vbCrLf + " ,SUM(ISNULL(AMOUNT,0)) ,SUM(ISNULL(VAT,0))"
        strSql += vbCrLf + " FROM " & temptableIss & " WHERE COLHEAD='' GROUP BY CATEGORY,METAL "

        strSql += vbCrLf + " INSERT INTO " & temptableIss & "(PARTICULARS,METAL,CATEGORY,RESULT,COLHEAD"
        strSql += vbCrLf + " ,PCS,GRSWT,NETWT,LESSWT,PUREWT,STNWT,DIAWT,ALLOY,WASTAGE,MC,AMOUNT,VAT)"
        strSql += vbCrLf + " SELECT DISTINCT METAL,METAL,'ZZZZZ',16,'G'"
        strSql += vbCrLf + " ,SUM(ISNULL(PCS,0)),SUM(ISNULL(GRSWT,0)),SUM(ISNULL(NETWT,0)),SUM(ISNULL(LESSWT,0))"
        strSql += vbCrLf + " ,SUM(ISNULL(PUREWT,0)) ,SUM(ISNULL(STNWT,0)) ,SUM(ISNULL(DIAWT,0)) "
        strSql += vbCrLf + " ,SUM(ISNULL(ALLOY,0)) ,SUM(ISNULL(WASTAGE,0)) ,SUM(ISNULL(MC,0)) "
        strSql += vbCrLf + " ,SUM(ISNULL(AMOUNT,0)) ,SUM(ISNULL(VAT,0))"
        strSql += vbCrLf + " FROM " & temptableIss & " WHERE COLHEAD='' GROUP BY METAL "

        strSql += vbCrLf + " INSERT INTO " & temptableIss & "(PARTICULARS,METAL,CATEGORY,RESULT,COLHEAD"
        strSql += vbCrLf + " ,PCS,GRSWT,NETWT,LESSWT,PUREWT,STNWT,DIAWT,ALLOY,WASTAGE,MC,AMOUNT,VAT)"
        strSql += vbCrLf + " SELECT DISTINCT 'TOTAL','ZZZZZ','ZZZZZ',17,'G'"
        strSql += vbCrLf + " ,SUM(ISNULL(PCS,0)),SUM(ISNULL(GRSWT,0)),SUM(ISNULL(NETWT,0)),SUM(ISNULL(LESSWT,0))"
        strSql += vbCrLf + " ,SUM(ISNULL(PUREWT,0)) ,SUM(ISNULL(STNWT,0)) ,SUM(ISNULL(DIAWT,0)) "
        strSql += vbCrLf + " ,SUM(ISNULL(ALLOY,0)) ,SUM(ISNULL(WASTAGE,0)) ,SUM(ISNULL(MC,0)) "
        strSql += vbCrLf + " ,SUM(ISNULL(AMOUNT,0)) ,SUM(ISNULL(VAT,0))"
        strSql += vbCrLf + " FROM " & temptableIss & " WHERE COLHEAD='' "
        strSql += vbCrLf + " UPDATE " & temptableIss & " SET PCS=NULL WHERE ISNULL(PCS,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableIss & " SET GRSWT=NULL WHERE ISNULL(GRSWT,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableIss & " SET NETWT=NULL WHERE ISNULL(NETWT,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableIss & " SET LESSWT=NULL WHERE ISNULL(LESSWT,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableIss & " SET PUREWT=NULL WHERE ISNULL(PUREWT,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableIss & " SET STNWT=NULL WHERE ISNULL(STNWT,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableIss & " SET DIAWT=NULL WHERE ISNULL(DIAWT,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableIss & " SET ALLOY=NULL WHERE ISNULL(ALLOY,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableIss & " SET WASTAGE=NULL WHERE ISNULL(WASTAGE,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableIss & " SET MC=NULL WHERE ISNULL(MC,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableIss & " SET AMOUNT=NULL WHERE ISNULL(AMOUNT,0)=0 "
        strSql += vbCrLf + " UPDATE " & temptableIss & " SET VAT=NULL WHERE ISNULL(VAT,0)=0 "
        strSql += vbCrLf + " END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " SELECT PARTICULARS,TRANNO,BILLDATE,ACNAME,DESCRIPTION,PCS"
        strSql += vbCrLf + " ,GRSWT,NETWT,LESSWT,PUREWT,STNWT,DIAWT,ALLOY,WASTAGE,MC,AMOUNT,VAT"
        strSql += vbCrLf + " ,RESULT,COLHEAD,TRANDATE"
        strSql += vbCrLf + "  FROM " & temptableRec & " ORDER BY METAL,CATEGORY,RESULT,TRANDATE"
        Dim dtGridRec As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGridRec)
        strSql = vbCrLf + "  SELECT PARTICULARS,TRANNO,BILLDATE,ACNAME,DESCRIPTION,PCS"
        strSql += vbCrLf + " ,GRSWT,NETWT,LESSWT,PUREWT,STNWT,DIAWT,ALLOY,WASTAGE,MC,AMOUNT,VAT"
        strSql += vbCrLf + " ,RESULT,COLHEAD,TRANDATE"
        strSql += vbCrLf + "  FROM " & temptableIss & " ORDER BY METAL,CATEGORY,RESULT,TRANDATE"
        Dim dtGridIss As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGridIss)
        dtGridRec.Merge(dtGridIss)

        If Not dtGridRec.Rows.Count > 0 Then
            chkcheck.Visible = False
            gridview.DataSource = Nothing
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If

    End Sub
    Private Sub Btnsearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btnsearch.Click
        Try
            If Trim(cmbcostcentre_OWN.Text.ToString) = "" Or Trim(cmbcostcentre_OWN.Text.ToString) = "ALL" Then costid = "ALL" Else costid = "'" & cmbcostcentre_OWN.SelectedValue & "'"

            Dim trantype As String = ""
            Dim Apptrantype As String = ""
            If rbtIssue.Checked Then
                trantype = "'IIN','IAP'"
            Else
                trantype = "'RIN','RAP'"
            End If

            If rbtBoth.Checked Then funcReceiptIssue() : Exit Sub
            strSql = vbCrLf + "  IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "SMITHISSREC')> 0 DROP TABLE TEMP" & systemId & "SMITHISSREC"
            strSql += vbCrLf + "   SELECT  TRANNO,JJFRMNO,TDATE,BILLNO,BILLDATE,ACNAME,TRANTYPE,"
            strSql += vbCrLf + "    DESCRIPTION, "
            strSql += vbCrLf + " CATNAME ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT )NETWT,SUM(LESSWT)LESSWT,"
            strSql += vbCrLf + " SUM(PUREWT)PUREWT,SUM(STNWT)STNWT,SUM(PREWT)PREWT,SUM(DIAWT)DIAWT,SUM(ALLOY)ALLOY,SUM(WASTAGE)WASTAGE,SUM(MC)MC,SUM(AMOUNT)AMOUNT,SUM(VAT)VAT,MRKFLAG"
            strSql += vbCrLf + ",TRANDATE, RESULT, COLHEAD,BATCHNO ,COSTID, Type, CONVERT(VARCHAR(6),CANCEL)CANCEL, TRANTYPE1, REFNO, REFDATE, CATCODE"
            strSql += vbCrLf + ",REMARK1 ,REMARK2,SNO "
            strSql += vbCrLf + "  INTO TEMP" & systemId & "SMITHISSREC"
            strSql += vbCrLf + "  FROM ( "
            If rbtIssue.Checked Then
                strSql += vbCrLf + "  SELECT "
                strSql += vbCrLf + "  TRANNO,JJFRMNO,TRANDATE TDATE"
                strSql += vbCrLf + "  ,REFNO BILLNO,REFDATE BILLDATE"
                strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = I.ACCODE) ACNAME"
                strSql += vbCrLf + "  ,CASE TRANTYPE WHEN 'IIS' THEN 'ISSUE' WHEN 'IAP' THEN 'APPROVAL ISSUE' WHEN 'IOT' THEN 'OTHER ISSUE' WHEN 'IPU' THEN 'PURCHASE RETURN' WHEN 'IIN' THEN 'INTERNAL TRANSFER'"
                strSql += vbCrLf + "   ELSE TRANTYPE END AS TRANTYPE"
                strSql += vbCrLf + "  ,CASE WHEN ITEMID = 0 THEN (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE) ELSE (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID) END AS [DESCRIPTION]"
                strSql += vbCrLf + "  ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE) AS CATNAME"
                strSql += vbCrLf + "  ,CONVERT(INT,PCS)PCS,CONVERT(NUMERIC(15,3),GRSWT)GRSWT,CONVERT(NUMERIC(15,3),NETWT)NETWT,CONVERT(NUMERIC(15,3),LESSWT)LESSWT,CONVERT(NUMERIC(15,3),PUREWT)PUREWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P'))) AS PREWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),ALLOY)ALLOY,CONVERT(NUMERIC(15,3),WASTAGE)WASTAGE,CONVERT(NUMERIC(15,2),CASE WHEN TRANTYPE <>'MI' THEN MCHARGE ELSE 0 END)MC,CONVERT(NUMERIC(15,2),CASE WHEN TRANTYPE <>'MI' THEN AMOUNT ELSE 0 END)AMOUNT,CONVERT(NUMERIC(15,2),CASE WHEN TRANTYPE <>'MI' THEN TAX ELSE 0 END)VAT,MRKFLAG,REMARK1,REMARK2"
                strSql += vbCrLf + "  ,TRANDATE,1 RESULT,' ' COLHEAD,BATCHNO,COSTID,CONVERT(VARCHAR(1),'I')TYPE,CANCEL,TRANTYPE AS TRANTYPE1,REFNO,REFDATE,CONVERT(VARCHAR(20),SNO) SNO,I.CATCODE"
                strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE AS I"
            End If
            If rbtReceipt.Checked Then
                strSql += vbCrLf + "  SELECT "
                strSql += vbCrLf + "  TRANNO,JJFRMNO,CONVERT(VARCHAR,TRANDATE,103)TDATE"
                strSql += vbCrLf + "  ,REFNO BILLNO,REFDATE BILLDATE"
                strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = I.ACCODE) ACNAME"
                strSql += vbCrLf + "  ,CASE TRANTYPE  WHEN 'RRE' THEN 'RECEIPT' WHEN 'RAP' THEN 'APPROVAL RECEIPT' WHEN 'ROT' THEN 'OTHER RECEIPT' WHEN 'RPU' THEN 'PURCHASE' WHEN 'RIN' THEN 'INTERNAL TRANSFER' "
                strSql += vbCrLf + "   ELSE TRANTYPE END AS TRANTYPE"
                strSql += vbCrLf + "  ,CASE WHEN ITEMID = 0 THEN (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE) ELSE (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID) END AS [DESCRIPTION]"
                strSql += vbCrLf + "  ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE) AS CATNAME"
                strSql += vbCrLf + "  ,CONVERT(INT,PCS)PCS,CONVERT(NUMERIC(15,3),GRSWT)GRSWT,CONVERT(NUMERIC(15,3),NETWT)NETWT,CONVERT(NUMERIC(15,3),LESSWT)LESSWT,CONVERT(NUMERIC(15,3),PUREWT)PUREWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))) AS STNWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P'))) AS PREWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),(SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))) AS DIAWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),ALLOY)ALLOY,CONVERT(NUMERIC(15,3),WASTAGE)WASTAGE,CONVERT(NUMERIC(15,2),MCHARGE)MC,CONVERT(NUMERIC(15,2),AMOUNT )AMOUNT,CONVERT(NUMERIC(15,2),TAX )VAT,MRKFLAG,REMARK1,REMARK2"
                strSql += vbCrLf + "  ,TRANDATE,1 RESULT,' 'COLHEAD,BATCHNO,COSTID,CONVERT(VARCHAR(1),'R')TYPE,CANCEL,TRANTYPE TRANTYPE1,REFNO,REFDATE,"
                strSql += vbCrLf + "  CONVERT(VARCHAR(20),SNO) SNO,"
                strSql += vbCrLf + "  I.CATCODE"
                strSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT AS I"
            End If
            If chkAsOnDate.Checked = False Then
                strSql += vbCrLf + "  WHERE TRANDATE BETWEEN '" & dtpfromdate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtptodate.Value.ToString("yyyy-MM-dd") & "'"
            Else
                strSql += vbCrLf + "  WHERE TRANDATE <= '" & dtpfromdate.Value.ToString("yyyy-MM-dd") & "'"
            End If
            strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + "  AND (LEN(TRANTYPE) > 2 OR (TRANTYPE = 'MI' AND ISNULL(ACCODE,'')<> ''))"
            strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
            If rbtmark.Checked = True Then
                strSql += vbCrLf + " AND ISNULL(MRKFLAG,'')='Y' "
            ElseIf rbtunmark.Checked = True Then
                strSql += vbCrLf + " AND ISNULL(MRKFLAG,'')='' "
            ElseIf rbtall.Checked = True Then
                strSql += vbCrLf + " AND ISNULL(MRKFLAG,'')<>'N' "
            End If
            strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID IN (" & costid & "))"
            strSql += vbCrLf + " AND I.METALID IN ('" & cmbmetal_OWN.SelectedValue.ToString & "') "
            If trantype <> "" Then strSql += vbCrLf + " AND TRANTYPE IN(" & trantype & ")"
            strSql += vbCrLf + " )X  GROUP BY TRANNO,JJFRMNO,TDATE ,BILLNO ,BILLDATE,ACNAME,TRANTYPE,"
            strSql += vbCrLf + "DESCRIPTION ,"
            strSql += vbCrLf + "CATNAME ,TRANDATE ,RESULT ,COLHEAD ,BATCHNO,COSTID ,TYPE ,CANCEL ,TRANTYPE1,REFNO,REFDATE,CATCODE,MRKFLAG "
            strSql += vbCrLf + ",REMARK1 ,REMARK2,SNO "
            If Not cn.State = ConnectionState.Open Then cn.Open()
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = vbCrLf + "  UPDATE TEMP" & systemId & "SMITHISSREC SET PCS=NULL,GRSWT=NULL,NETWT=NULL,LESSWT=NULL,PUREWT=NULL"
            strSql += vbCrLf + " ,STNWT=T.GRSWT,DIAWT=NULL,PREWT=NULL FROM TEMP" & systemId & "SMITHISSREC T "
            strSql += vbCrLf + " WHERE CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE METALID='T' AND DIASTONE='S'))"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "SMITHISSREC SET PCS=NULL,GRSWT=NULL,NETWT=NULL,LESSWT=NULL,PUREWT=NULL"
            strSql += vbCrLf + " ,STNWT=NULL,DIAWT=T.GRSWT,PREWT=NULL FROM TEMP" & systemId & "SMITHISSREC T "
            strSql += vbCrLf + " WHERE CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID = 'D')"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "SMITHISSREC SET PCS=NULL,GRSWT=NULL,NETWT=NULL,LESSWT=NULL,PUREWT=NULL"
            strSql += vbCrLf + " ,STNWT=NULL,DIAWT=NULL,PREWT=T.GRSWT FROM TEMP" & systemId & "SMITHISSREC T "
            strSql += vbCrLf + " WHERE CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE METALID='T' AND DIASTONE='P'))"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "SMITHISSREC SET CANCEL='Cancel' WHERE CANCEL='Y'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "SMITHISSREC)>0 "
            strSql += vbCrLf + "  BEGIN"
            strSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "SMITHISSREC(ACNAME,PCS,GRSWT,NETWT,LESSWT,PUREWT,STNWT,DIAWT,PREWT,ALLOY,WASTAGE,MC,AMOUNT,VAT,RESULT,COLHEAD)"
            strSql += vbCrLf + "  SELECT 'TOTAL'TDATE,SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(LESSWT),SUM(PUREWT),SUM(STNWT),SUM(DIAWT),SUM(PREWT),SUM(ALLOY),SUM(WASTAGE),SUM(MC),SUM(AMOUNT),SUM(VAT),3 RESULT,'G'COLHEAD"
            strSql += vbCrLf + "  FROM TEMP" & systemId & "SMITHISSREC"
            strSql += vbCrLf + "  WHERE ISNULL(CANCEL,'')=''"
            strSql += vbCrLf + "  END"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = "  SELECT * FROM TEMP" & systemId & "SMITHISSREC "
            strSql += vbCrLf + "  ORDER BY "
            strSql += vbCrLf + "  RESULT,TRANDATE,TRANNO"
            Dim dtGrid As New DataTable
            dtGrid.Columns.Add("CHECK", Type.GetType("System.Boolean"))
            dtGrid.Columns.Add("KEYNO", GetType(String))
            dtGrid.Columns("KEYNO").AutoIncrement = True
            dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
            dtGrid.Columns("KEYNO").AutoIncrementStep = 1
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGrid)
            dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
            If Not dtGrid.Rows.Count > 0 Then
                chkcheck.Visible = False
                gridview.DataSource = Nothing
                MsgBox("Record not found", MsgBoxStyle.Information)
                Exit Sub
            End If
            gridview.DataSource = Nothing
            If dtGrid.Rows.Count > 0 Then
                gridview.DataSource = dtGrid
                gridview.Columns("TRANNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridview.Columns("TDATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridview.Columns("BILLNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridview.Columns("BILLDATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridview.Columns("ACNAME").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridview.Columns("TRANTYPE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridview.Columns("DESCRIPTION").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                gridview.Columns("CATNAME").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                gridview.Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridview.Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridview.Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridview.Columns("LESSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridview.Columns("PUREWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridview.Columns("STNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridview.Columns("PREWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridview.Columns("DIAWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridview.Columns("WASTAGE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridview.Columns("MC").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridview.Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridview.Columns("VAT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridview.Columns("ALLOY").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridview.Columns("PCS").Width = 40
                gridview.Columns("NETWT").Width = 70
                gridview.Columns("GRSWT").Width = 70
                gridview.Columns("CHECK").Width = 20
                gridview.Columns("LESSWT").Width = 70
                gridview.Columns("PUREWT").Width = 70
                gridview.Columns("STNWT").Width = 70
                gridview.Columns("PREWT").Width = 70
                gridview.Columns("DIAWT").Width = 70
                gridview.Columns("ALLOY").Width = 70
                gridview.Columns("WASTAGE").Width = 70
                gridview.Columns("MC").Width = 70
                gridview.Columns("AMOUNT").Width = 90
                gridview.Columns("VAT").Width = 80
                gridview.Columns("TRANDATE").Width = 130
                gridview.Columns("REMARK1").Width = 120
                gridview.Columns("REMARK2").Width = 120

                ''DISPLAYINDEX
                gridview.Columns("CHECK").DisplayIndex = 0

                ''NON DISPLAY COLUMNS
                gridview.Columns("MRKFLAG").Visible = False
                gridview.Columns("KEYNO").Visible = False
                gridview.Columns("BATCHNO").Visible = False
                gridview.Columns("SNO").Visible = False
                gridview.Columns("RESULT").Visible = False
                gridview.Columns("COLHEAD").Visible = False
                gridview.Columns("TRANTYPE1").Visible = False
                gridview.Columns("CATCODE").Visible = False
                gridview.Columns("CANCEL").Visible = False
                gridview.Columns("COSTID").Visible = False
                gridview.Columns("TYPE").Visible = False
                gridview.Columns("CANCEL").Visible = False
                gridview.Columns("REFNO").Visible = False
                gridview.Columns("REFDATE").Visible = False
                gridview.Columns("CHECK").HeaderText = ""
                For i As Integer = 0 To gridview.Columns.Count - 1
                    If gridview.Columns(i).HeaderText <> "" Then
                        gridview.Columns(i).ReadOnly = True
                    End If
                Next
                For I As Integer = 0 To gridview.Rows.Count - 1
                    If gridview.Rows(I).Cells("ACNAME").Value <> "TOTAL" Then gridview.Rows(I).Cells("CHECK").Value = chkcheck.Checked
                    If gridview.Rows(I).Cells("ACNAME").Value.ToString = "TOTAL" Then
                        gridview.Rows(I).DefaultCellStyle.BackColor = Color.LightYellow
                        gridview.Rows(I).DefaultCellStyle.ForeColor = Color.Black
                        gridview.Rows(I).DefaultCellStyle.Font = New Font("VERDANA", 9, FontStyle.Bold)
                    ElseIf gridview.Rows(I).Cells("MRKFLAG").Value.ToString = "Y" Then
                        gridview.Rows(I).DefaultCellStyle.BackColor = Color.PowderBlue
                        gridview.Rows(I).DefaultCellStyle.ForeColor = Color.Black
                    ElseIf gridview.Rows(I).Cells("MRKFLAG").Value.ToString.Trim = "" Then
                        gridview.Rows(I).DefaultCellStyle.BackColor = Color.PaleGoldenrod
                        gridview.Rows(I).DefaultCellStyle.ForeColor = Color.Red
                    End If
                Next
                FormatGridColumns(gridview, False, False, False, False)
                gridview.Focus()
                gridview.Refresh()
                chkcheck.Visible = True
                txtremark1.Clear()
                txtremark2.Clear()
            Else
                chkcheck.Visible = False
                gridview.DataSource = Nothing
                MsgBox("Records not found.")
                dtpfromdate.Focus()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
        End Try
    End Sub
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
                    If colFormat Then .Columns(i).DefaultCellStyle.Format = "dd/MM/yyyy"
                End If
                If Not sortColumns Then .Columns(i).SortMode = DataGridViewColumnSortMode.NotSortable
            Next
        End With
    End Sub

    Private Sub btnexport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnexport.Click
        BrightPosting.GExport.Post(Me.Name, strCompanyId, "Issue Receipt Transfer Report", gridview, BrightPosting.GExport.GExportType.Export)
    End Sub

    Private Sub btnexit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnexit.Click
        Me.Close()
    End Sub

    Private Sub gridview_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridview.KeyDown
        If e.KeyCode = Keys.Space Then
            If gridview.CurrentRow.Cells("MRKFLAG").Value.ToString.Trim <> "" Or gridview.CurrentRow.Cells("ACNAME").Value = "TOTAL" Then Exit Sub
            TRANNO = gridview.CurrentRow.Cells("BATCHNO").Value.ToString
            _dtins = New DataTable()
            _dtins = CType(gridview.DataSource, DataTable).Copy
            Dim dv As New DataView
            dv = _dtins.DefaultView()
            dv.RowFilter = "BATCHNO = '" & TRANNO & "'"
            Dim dt As New DataTable
            dt = dv.ToTable
            Dim Check As Boolean = gridview.CurrentRow.Cells("CHECK").Value
            If dt.Rows.Count > 0 Then
                For I As Integer = 0 To dt.Rows.Count - 1
                    With dt.Rows(I)
                        gridview.Rows(Val(.Item("KEYNO").ToString)).Cells("CHECK").Value = Not Check
                    End With
                Next
            End If
        ElseIf e.KeyCode = Keys.Escape Then
        End If
    End Sub
    Private Sub gridview_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles gridview.MouseClick
        If gridview.CurrentRow.Cells("MRKFLAG").Value.ToString.Trim <> "" Or gridview.CurrentRow.Cells("ACNAME").Value = "TOTAL" Then Exit Sub
        TRANNO = gridview.CurrentRow.Cells("BATCHNO").Value.ToString
        _dtins = New DataTable()
        _dtins = CType(gridview.DataSource, DataTable).Copy
        Dim dv As New DataView
        dv = _dtins.DefaultView()
        dv.RowFilter = "BATCHNO = '" & TRANNO & "'"
        Dim dt As New DataTable
        dt = dv.ToTable
        Dim Check As Boolean = gridview.CurrentRow.Cells("CHECK").Value
        If dt.Rows.Count > 0 Then
            For I As Integer = 0 To dt.Rows.Count - 1
                With dt.Rows(I)
                    gridview.Rows(Val(.Item("KEYNO").ToString)).Cells("CHECK").Value = Not Check
                End With
            Next
        End If
    End Sub

    Private Sub GridtrfTo()
        'grdcmb.DataSource = Nothing
        Dim rocmb As DataRow
        dtcmb = New DataTable
        dtcmb.Columns.Add("TRFTO")

        rocmb = dtcmb.NewRow()
        rocmb("TRFTO") = "TO LOT"
        dtcmb.Rows.Add(rocmb)
        rocmb = dtcmb.NewRow()
        rocmb("TRFTO") = "TO MELTING"
        dtcmb.Rows.Add(rocmb)

        'grdcmb.DataSource = dtcmb
        'grdcmb.DisplayMember = "TRFTO"
        'grdcmb.ValueMember = "TRFTO"
    End Sub

    Private Sub GridDesignername()
        'grdcmb.DataSource = Nothing
        dtcmb = New DataTable
        dtcmb = FillDesignerfromadmindb()
        If dt.Rows.Count <= 0 Then
            'grdcmb.Enabled = False
            Exit Sub
        Else
            'grdcmb.DataSource = dtcmb
            'grdcmb.DisplayMember = "DESIGNERNAME"
            'grdcmb.ValueMember = "DESIGNERID"
            'grdcmb.SelectedIndex = 0
            'grdcmb.Enabled = True
        End If
    End Sub

    Private Sub GridItemname(ByVal catcode As String)
        'grdcmb.DataSource = Nothing
        dtcmb = New DataTable
        dtcmb = Fillitemfromadmindb(catcode)
        If dt.Rows.Count <= 0 Then
            'grdcmb.Enabled = False
            Exit Sub
        Else
            'grdcmb.DataSource = dtcmb
            'grdcmb.DisplayMember = "ITEMNAME"
            'grdcmb.ValueMember = "ITEMID"
            'grdcmb.SelectedIndex = 0
            'grdcmb.Enabled = True
        End If
    End Sub

    Private Sub GridCatname(ByVal loadtype As String, ByVal metalid As String)
        'grdcmb.DataSource = Nothing
        dtcmb = New DataTable
        dtcmb = FillCategoryfromadmindb(loadtype, metalid)
        If dt.Rows.Count <= 0 Then
            'grdcmb.Enabled = False
            Exit Sub
        Else
            'grdcmb.DataSource = dtcmb
            'grdcmb.DisplayMember = "CATNAME"
            'grdcmb.ValueMember = "CATCODE"
            'grdcmb.SelectedIndex = 0
            'grdcmb.Enabled = True
        End If
    End Sub

    Function FillDesignerfromadmindb()
        strSql = "SELECT ACCODE DESIGNERID,ACNAME DESIGNERNAME FROM  " & cnAdminDb & "..ACHEAD where isnull(ACTYPE,'') IN('D','S','G','I','C') ORDER BY ACNAME  "
        dt = New DataTable
        dt = getData(strSql)
        Return dt
    End Function

    Function FillCategoryfromadmindb(ByVal type As String, ByVal metal As String) As DataTable
        strSql = "SELECT CATCODE,CATNAME FROM  " & cnAdminDb & "..CATEGORY where "
        If type = "TO MELTING" Then
            strSql += " ISNULL(GS11,'')='Y' "
        Else
            strSql += " ISNULL(GS12,'')='Y'"
        End If
        strSql += "  AND ISNULL(METALID,'')='" & metal & "'ORDER BY CATNAME  "
        dt = New DataTable
        dt = getData(strSql)
        Return dt
    End Function

    Function Fillitemfromadmindb(ByVal catcode As String) As DataTable
        strSql = "SELECT ITEMID,ITEMNAME FROM  " & cnAdminDb & "..ITEMMAST where CATCODE IN(SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE "
        strSql += " ISNULL(CATCODE,'')='" & catcode & "') ORDER BY ITEMNAME"
        dt = New DataTable
        dt = getData(strSql)
        Return dt
    End Function

    Private Sub btnupdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MsgBox("Update Successfully.")
    End Sub
    Private Sub searchToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles searchToolStripMenuItem1.Click
        Btnsearch_Click(Me, New EventArgs())
    End Sub

    Private Sub exportToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles exportToolStripMenuItem2.Click
        btnexport_Click(Me, New EventArgs())
    End Sub

    Private Sub exitToolStripMenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles exitToolStripMenuItem3.Click
        Me.Close()
    End Sub
    Private Sub cmbfrmcostcentre_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbfrmcostcentre_OWN.Leave
        If Trim(cmbfrmcostcentre_OWN.Text.ToString) = "" Or Trim(cmbfrmcostcentre_OWN.Text.ToString) = "ALL" Then costid = "ALL" Else costid = "'" & cmbfrmcostcentre_OWN.SelectedValue & "'"
    End Sub


    Private Sub cmbcostcentre_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbcostcentre_OWN.Leave
        If Trim(cmbcostcentre_OWN.Text.ToString) = "" Or Trim(cmbcostcentre_OWN.Text.ToString) = "ALL" Then costid = "ALL" Else costid = "'" & cmbcostcentre_OWN.SelectedValue & "'"
    End Sub
    Private Sub chkcostcentrewise_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        cmbfrmcostcentre_OWN.Visible = False
        cmbcostcentre_OWN.Visible = True
    End Sub
    Private Sub updaterec(ByVal bagno As String, ByVal sno As String, ByVal melt_retag As String, ByVal tran As OleDbTransaction)
        strSql = " UPDATE " & cnStockDb & "..RECEIPT SET MRKFLAG='" & melt_retag & "' "
        strSql += ",JJFRMNO='" & bagno & "' WHERE 1=1 AND SNO='" & sno & "'"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
    End Sub
    Private Sub updateiss(ByVal bagno As String, ByVal sno As String, ByVal melt_retag As String, ByVal tran As OleDbTransaction)
        strSql = " UPDATE " & cnStockDb & "..ISSUE SET MRKFLAG='" & melt_retag & "' "
        strSql += ",JJFRMNO='" & bagno & "' WHERE 1=1 AND SNO='" & sno & "'"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
    End Sub

    Private Sub updateissrmk(ByVal bagno As String, ByVal sno As String, ByVal melt_retag As String, ByVal tran As OleDbTransaction)
        strSql = " UPDATE " & cnStockDb & "..ISSUE SET JJRMK1='" & melt_retag & "' "
        strSql += ",JJRMK2='" & bagno & "' WHERE 1=1 AND SNO='" & sno & "'"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
    End Sub
    Private Sub updateissstone(ByVal bagno As String, ByVal sno As String, ByVal melt_retag As String, ByVal tran As OleDbTransaction)
        strSql = " UPDATE " & cnStockDb & "..ISSSTONE SET MRKFLAG='" & melt_retag & "' "
        strSql += ",JJFRMNO='" & bagno & "' WHERE 1=1 AND BATCHNO='" & sno & "'"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
    End Sub
    Function Getbagno(ByVal tran As OleDbTransaction) As String
        Dim retval As String = ""
        strSql = "SELECT CONVERT(INT,CTLTEXT)BAGNO FROM " & cnStockDb & "..SOFTCONTROLTRAN WHERE CTLID = 'BAGNO'"
        cmd = New OleDbCommand(strSql, cn, tran)
        da = New OleDbDataAdapter(cmd)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            retval = Val(dt.Rows(0).Item("BAGNO").ToString) + 1
        End If
        strSql = "UPDATE " & cnStockDb & "..SOFTCONTROLTRAN SET CTLTEXT='" & Val(dt.Rows(0).Item("BAGNO").ToString) + 1 & "'WHERE CTLID = 'BAGNO'"
        cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
        Return cnCostId & "B" & Mid(Format(trantodate, "dd/MM/yyyy"), 9, 2).ToString & retval
    End Function
    Public Function Getselectedcostid(ByVal chkbox As Object, Optional ByVal withquot As Boolean = True) As String
        Dim costname, costid As String
        costid = ""
        costname = checked_unchecked_chklist(chkbox, "C")
        dt = New DataTable
        dt = GetId("COSTID", "COSTCENTRE", "COSTNAME", costname)
        If Not dt.Rows.Count > 0 Then Return ""
        For i As Integer = 0 To dt.Rows.Count - 1
            If withquot Then costid += "'"
            costid += dt.Rows(i).Item("COSTID").ToString
            If withquot Then costid += "'," Else costid += ","
        Next
        costid = Mid(costid, 1, Len(costid) - 1)
        Return costid
    End Function
    Public Function GetId(ByVal columname As String, ByVal tablename As String, ByVal Cond As String, ByVal name As String)
        strSql = " select " & columname & " from " & tablename & " where   " & Cond & " in (" & name & ")"
        Return getData(strSql)
    End Function
    Function checked_unchecked_chklist(ByVal chkbox As Object, ByVal check_uncheck As String) As String
        Dim Retstring As String = ""
        Dim cntChkItem As Integer = 0
        If (check_uncheck = "C") Then
            For cnt As Integer = 0 To chkbox.checkedItems.Count - 1
                Retstring = Retstring & "'" & chkbox.CheckedItems(cnt).ToString() & "',"
            Next
        ElseIf (check_uncheck = "U") Then
            For cnt As Integer = 0 To chkbox.items.count - 1
                If chkbox.GetItemChecked(cnt) = False Then Retstring = Retstring & "'" & chkbox.Items(cnt).ToString() & "',"
            Next
        End If
        If Retstring.Length > 0 Then Retstring = Mid(Retstring, 1, Len(Retstring) - 1)
        Return Retstring
    End Function
    Function UpdateMarkandUnmark(ByVal confirm As Boolean) As Integer
        Try
            tran = Nothing : OLtran = Nothing
            Dim _dtins1 As DataTable
            _dtins1 = New DataTable()
            _dtins1 = CType(gridview.DataSource, DataTable).Copy
            _dtins = New DataTable()
            _dtins = CType(gridview.DataSource, DataTable).Copy
            tran = cn.BeginTransaction()
            confirm = True
            If confirm = True Then
                Dim dv As New DataView
                dv = _dtins.DefaultView()
                dv.RowFilter = ("ISNULL(MRKFLAG,'')='' AND CHECK=TRUE AND ACNAME<>'TOTAL' ")
                _dtins = dv.ToTable
                If _dtins.Rows.Count > 0 Then
                    jjfrmno = ""
                    jjfrmno = GetMaxJJFRMNo(cnAdminDb, "JJFORM_NO", tran)
                    For i As Integer = 0 To _dtins.Rows.Count - 1
                        With _dtins.Rows(i)
                            'UPDATE MARKED 
                            If rbtIssue.Checked = True Then
                                updateiss(jjfrmno, .Item("SNO").ToString, "Y", tran)
                                updateissrmk(txtremark2.Text, .Item("SNO").ToString, txtremark1.Text, tran)
                                'updateissstone(jjfrmno, .Item("BATCHNO").ToString, "Y", OLtran)
                            Else
                                updaterec(jjfrmno, .Item("SNO").ToString, "Y", tran)
                            End If
                        End With
                    Next
                    If confirm = True Then MsgBox("Generated Successfully..", MsgBoxStyle.Information, "Brighttech Retail Jewellery") Else MsgBox("Not Generated.")
                    dCostid = Mid(costid, 2, 2)                   
                Else
                    MsgBox("You Should Mark Atleast one Record..", MsgBoxStyle.OkOnly)
                    confirm = False
                End If
            End If
            tran.Commit()
            If confirm = False Then MsgBox("Not Generated.")
            Btnsearch_Click(Me, New EventArgs())
        Catch ex As Exception
            tran.Rollback()
            MsgBox(ex.Message)
        End Try
        If confirm = True Then
            Try
                If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                    Dim write As IO.StreamWriter
                    Dim memfile As String = "\BillPrint" & prnmemsuffix.Trim & ".mem"
                    write = IO.File.CreateText(Application.StartupPath & memfile)
                    write.WriteLine(LSet("TYPE", 15) & ":IIN")
                    write.WriteLine(LSet("BATCHNO", 15) & ":" & jjfrmno)
                    write.WriteLine(LSet("TRANDATE", 15) & ":" & GetServerDate(tran))
                    write.WriteLine(LSet("DUPLICATE", 15) & ":Y")
                    write.WriteLine(LSet("COSTID", 15) & ":" & dCostid)
                    write.Flush()
                    write.Close()
                    If EXE_WITH_PARAM = False Then
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                    Else
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe", _
                        LSet("TYPE", 15) & ":IIN" & ";" & _
                        LSet("BATCHNO", 15) & ":" & jjfrmno & ";" & _
                        LSet("TRANDATE", 15) & ":" & GetServerDate(tran) & ";" & _
                        LSet("DUPLICATE", 15) & ":Y")
                    End If

                Else

                    MsgBox("Billprint exe not found", MsgBoxStyle.Information)
                End If
            Catch ex As Exception
            End Try
        End If
    End Function
    Public Function GetMaxJJFRMNo(ByVal stockdb As String, ByVal CTLID As String, ByVal Transact As OleDbTransaction) As String
        Dim Strsql As String
        Strsql = " SELECT CTLTEXT FROM " & stockdb & "..SOFTCONTROL WHERE CTLID='" & CTLID & "'"
        Dim dtt As New DataTable
        Dim ds As New DataSet
        cmd = New OleDbCommand(Strsql, cn, Transact)
        da = New OleDbDataAdapter(cmd)
        da.Fill(ds)
        dtt = ds.Tables(0)
        Dim AA As String = ""
        If dtt.Rows.Count > 0 Then
            AA = Val(dtt.Rows(0).Item("CTLTEXT").ToString) + 1
        End If
        Strsql = " UPDATE B SET CTLTEXT=" & Val(AA) & " FROM " & stockdb & "..SOFTCONTROL B WHERE CTLID='" & CTLID & "'"
        cmd = New OleDbCommand(Strsql, cn, Transact)
        cmd.ExecuteNonQuery()
        Return AA
    End Function
    Function funcUpdateIssRec(ByVal Remarksno As String, ByVal tran As OleDbTransaction)
        strSql = " UPDATE " & cnStockDb & "..RECEIPT SET CANCEL='Y' "
        strSql += "WHERE 1=1 AND REFNO='" & Remarksno & "'"        
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
        strSql = " UPDATE " & cnStockDb & "..ISSUE SET CANCEL='Y' "
        strSql += "WHERE 1=1 AND REFNO='" & Remarksno & "'"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
        strSql = " UPDATE " & cnAdminDb & "..ITEMLOT SET CANCEL='Y' "
        strSql += "WHERE 1=1 AND BILLNO='" & Remarksno & "'"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
    End Function

    Private Sub funnew()
        dtpfromdate.Value = System.DateTime.Now
        dtptodate.Value = System.DateTime.Now
        Dim dtcostid As DataTable
        dtcostid = Getcostcenter(GetSqlValue(cn, "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID ='COSTID'"), "A")
        If dtcostid.Rows.Count > 0 Then
            cmbcostcentre_OWN.DataSource = dtcostid
            cmbcostcentre_OWN.DisplayMember = "COSTNAME"
            cmbcostcentre_OWN.ValueMember = "COSTID"
        End If
        dtcost = Getcostcenter(GetSqlValue(cn, "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID ='COSTID'"), "A", True)
        cmbfrmcostcentre_OWN.Items.Clear()
        For i As Integer = 0 To dtcost.Rows.Count - 1
            cmbfrmcostcentre_OWN.Items.Add(dtcost.Rows(i).Item("COSTNAME"))
            cmbfrmcostcentre_OWN.SetItemChecked(i, False)
        Next
        dtmetal = New DataTable
        dtmetal = Getmetal()
        If dtmetal.Rows.Count > 0 Then
            cmbmetal_OWN.DataSource = Nothing
            cmbmetal_OWN.DataSource = dtmetal
            cmbmetal_OWN.DisplayMember = "METALNAME"
            cmbmetal_OWN.ValueMember = "METALID"
            cmbmetal_OWN.SelectedValue = "G"
        Else
            cmbmetal_OWN.DataSource = Nothing
        End If
        If chkAsOnDate.Checked = False Then
            Label1.Visible = True
            dtptodate.Visible = True
        End If
        chkcheck.Visible = False
        rbtall.Checked = True
        dtpfromdate.Focus()
        gridview.DataSource = Nothing
        Panel2.Visible = True
        PictureBox3.Visible = True
        PictureBox4.Visible = True
        cmbfrmcostcentre_OWN.Text = GetSqlValue(cn, "SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID='" & cnCostId & "'")
        cmbcostcentre_OWN.Text = GetSqlValue(cn, "SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID='" & cnCostId & "'")
        txtremark1.Clear()
        txtremark2.Clear()
    End Sub
    Public Function getData(ByVal qry As String, Optional ByVal tran As OleDbTransaction = Nothing) As DataTable
        Try
            Dim dtm As New DataTable
            Dim ds As New DataSet
            Dim Dbnames As String
            Dim CrystalDbname As String
            'Getconstrings()
resm:
            If Connectionmode = "SQL" Then
                ''Dim sqlcon As SqlClient.SqlConnection = Getconsql(Dbnames)
                'cmd = New OleDbCommand(qry, cn, tran)
                'da = New OleDbDataAdapter(cmd)
                'da.Fill(dtm)
            Else
                cmd = New OleDbCommand(qry, cn, tran)
                cmd.CommandTimeout = 2000
                da = New OleDbDataAdapter(cmd)
                da.Fill(ds)
                If ds.Tables.Count > 0 Then
                    dtm = ds.Tables(0)
                End If
            End If
            Return dtm
        Catch ex As Exception
            'If ex.Message = "Cannot open database """ & strCompanyId & """ requested by the login. The login failed." Then Dbnames = Replace(Dbnames, "ADMINDB", "MAINDB") : CrystalDbname = Dbnames : GoTo resm
            MsgBox(ex.Message & vbCrLf & ex.StackTrace)
        End Try
    End Function
    Public Function Getmetal(Optional ByVal metalids As String = Nothing, Optional ByVal withall As Boolean = False) As DataTable
        strSql = ""
        If withall = True Then
            strSql = " SELECT 'Z' METALID,'ALL' METALNAME,'' TTYPE,0 RESULT UNION ALL "
        End If
        strSql = strSql + "SELECT METALID,METALNAME,TTYPE, 1 RESULT FROM " & cnAdminDb & "..METALMAST"
        If metalids <> Nothing Then strSql = strSql + " WHERE METALID IN (" & metalids & ")"
        If withall = False Then strSql = strSql + " ORDER BY DISPLAYORDER"
        If withall = True Then strSql = strSql + " ORDER BY RESULT,METALNAME"
        Return getData(strSql)
    End Function
    Public Function Getcostcenter(Optional ByVal costids As String = "", Optional ByVal orderbasedon As String = "M", Optional ByVal withall As Boolean = False) As DataTable
        Try
            Dim chkbranch As String
            strSql = ""
            If withall = True Then
                strSql = " SELECT 'ALL' COSTID,'ALL' COSTNAME,0 RESULT UNION ALL "
            End If
            chkbranch = GetSqlValue(cn, "select MAIN from " & cnAdminDb & "..synccostcentre WHERE COSTID='" & costids & "'")
            If Trim(chkbranch) = "Y" Then
                strSql = strSql + "SELECT COSTID,COSTNAME,1 RESULT  FROM " & cnAdminDb & "..COSTCENTRE WHERE 1=1 "
            Else
                strSql = strSql + "SELECT COSTID,COSTNAME,1 RESULT  FROM " & cnAdminDb & "..COSTCENTRE WHERE 1=1 AND COSTID='" & costids & "'"
            End If            
            ' If costids <> "" Then strSql = strSql & " AND COSTID IN (" & costids & ") "
            strSql += " AND ISNULL(ACTIVE,'')<>'N'"
            If orderbasedon = "M" Then strSql = strSql & " ORDER BY DISPORDER,RESULT"
            If orderbasedon = "A" Then strSql = strSql & " ORDER BY COSTNAME,RESULT"
            Return getData(strSql)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Private Sub btnconfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If gridview.Rows.Count < 0 Then Exit Sub
        If MsgBox("Are you sure to Transfer selected Items.", MsgBoxStyle.OkCancel) = MsgBoxResult.Cancel Then Exit Sub
        UpdateMarkandUnmark(True)
    End Sub

    Private Sub btnnew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnnew.Click
        funnew()
    End Sub
    Private Sub chkcheck_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkcheck.CheckedChanged
        If gridview.Columns.Contains("CHECK") = False Then Exit Sub
        For i As Integer = 0 To gridview.Rows.Count - 1
            gridview.Rows(i).Cells("CHECK").Value = chkcheck.Checked
        Next
    End Sub
    Private Sub chkAsOnDate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAsOnDate.CheckedChanged
        If chkAsOnDate.Checked = True Then
            Label1.Visible = False
            Label10.Visible = False
            Label7.Visible = False
            dtptodate.Visible = False
        Else
            Label1.Visible = True
            Label10.Visible = True
            Label7.Visible = True
            dtptodate.Visible = True
        End If
    End Sub

    Private Sub btnGenerate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        If Val(gridview.Rows.Count) = 0 Then Exit Sub
        If MsgBox("Are you sure generate marked entries.", MsgBoxStyle.OkCancel) = MsgBoxResult.Cancel Then Exit Sub
        UpdateMarkandUnmark(False)
    End Sub
    Private Sub rbtIssue_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtIssue.GotFocus
        Dim dtcostid As DataTable
        dtcostid = Getcostcenter("'CO'", "A")
        cmbfrmcostcentre_OWN.Items.Clear()
        For i As Integer = 0 To dtcostid.Rows.Count - 1
            cmbfrmcostcentre_OWN.Items.Add(dtcostid.Rows(i).Item("COSTNAME"))
            cmbfrmcostcentre_OWN.SetItemChecked(i, False)
        Next
    End Sub
    Private Sub rbtReceipt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtReceipt.GotFocus
        Dim dtcostid As DataTable
        dtcostid = Getcostcenter("'CO'", "A")
        cmbfrmcostcentre_OWN.Items.Clear()
        For i As Integer = 0 To dtcostid.Rows.Count - 1
            cmbfrmcostcentre_OWN.Items.Add(dtcostid.Rows(i).Item("COSTNAME"))
            cmbfrmcostcentre_OWN.SetItemChecked(i, False)
        Next
    End Sub
    Private Sub gridview_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridview.KeyPress
        If gridview.RowCount > 0 Then
            Dim jjfrmno As String = gridview.Item("JJFRMNO", gridview.CurrentRow.Index).Value.ToString
            If UCase(e.KeyChar) = "D" Then
                dCostid = Mid(costid, 2, 2)
                If jjfrmno.Trim <> "" Then
                    Try
                        If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                            Dim write As IO.StreamWriter
                            Dim memfile As String = "\BillPrint" & prnmemsuffix.Trim & ".mem"
                            write = IO.File.CreateText(Application.StartupPath & memfile)
                            write.WriteLine(LSet("TYPE", 15) & ":IIN")
                            write.WriteLine(LSet("BATCHNO", 15) & ":" & jjfrmno)
                            write.WriteLine(LSet("TRANDATE", 15) & ":" & GetServerDate(tran))
                            write.WriteLine(LSet("DUPLICATE", 15) & ":Y")
                            write.WriteLine(LSet("COSTID", 15) & ":" & dCostid)
                            write.Flush()
                            write.Close()
                            If EXE_WITH_PARAM = False Then
                                System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                            Else
                                System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe", _
                                LSet("TYPE", 15) & ":IIN" & ";" & _
                                LSet("BATCHNO", 15) & ":" & jjfrmno & ";" & _
                                LSet("TRANDATE", 15) & ":" & GetServerDate(tran) & ";" & _
                                LSet("DUPLICATE", 15) & ":Y")
                            End If

                        Else

                            MsgBox("Billprint exe not found", MsgBoxStyle.Information)
                        End If
                    Catch ex As Exception
                    End Try
                End If
            End If
        End If
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnnew_Click(Me, New EventArgs)
    End Sub

    Private Sub actualToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles actualToolStripMenuItem1.Click
        btnGenerate_Click(Me, New EventArgs)
    End Sub
End Class



