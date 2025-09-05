Imports System.Data.OleDb
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Security.Principal
Imports System.Security.Permissions

''' <summary>
''' Newly Add
''' </summary>
''' <remarks></remarks>
''' 

Imports System
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports TouchlessLib
'Imports System.IO
Imports System.Drawing.Imaging
'Imports System.Runtime.InteropServices
'Imports System.Threading
Imports Word = Microsoft.Office.Interop.Word


Public Class frmTagImageViewer_Format2

    'GCALID 584: CLIENT KAMESWARI JEWS: REQUIREMENTS-COLOR INDICATION IS REQUIRED FOR TAG SOLD: ALTER BY SATHYA 
    Dim tagNo As String
    Dim itemId As Integer
    Dim strSql As String
    Dim bMap As Bitmap
    Dim cmd As OleDbCommand
    Dim defaultPic As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICPATH'")
    Dim _MCONWTAMT As Boolean = IIf(GetAdmindbSoftValue("MC_ON_WTAMT", "W") = "W", True, False)
    Dim _MCONGRSNET As Boolean = IIf(GetAdmindbSoftValue("MC_ON_GRSNET", "Y") = "Y", True, False)
    Dim _McWithWastage As Boolean = IIf(GetAdmindbSoftValue("MCWITHWASTAGE", "N") = "Y", True, False)
    Dim _FourCMaintain As Boolean = IIf(GetAdmindbSoftValue("4CMAINTAIN", "Y") = "Y", True, False)
    Dim _MCCALCON_ITEM_GRS As Boolean = False
    Dim ShowPurchaseDetail As Boolean = True
    Dim dt As New DataTable
    Dim objGridShower As frmGridDispDia
    Dim notify As NotifyIcon
    Dim dtImg As New DataTable
    Dim dtImgCol As New DataColumn()
    Dim _TagDupPrint As Boolean = IIf(GetAdmindbSoftValue("TAGCHKDUPPRINT", "N") = "Y", True, False)
    Dim _TagImgPrint As Boolean = IIf(GetAdmindbSoftValue("TAGCHKIMGPRINT", "N") = "Y", True, False)
    Dim PUR_LANDCOST As Boolean = IIf(GetAdmindbSoftValue("PUR_LANDCOST", "N").ToUpper = "Y", True, False)
    Dim IMAGE_UPD_BILLTAG As Boolean = IIf(GetAdmindbSoftValue("IMAGE_UPD_BILLTAG", "N") = "Y", True, False)
    Dim defalutSourcePath As String = Nothing
    Dim picExtension As String = Nothing
    Dim defaultDestination As String
    Dim TAG_CHECK_ADDINFO As String = GetAdmindbSoftValue("TAG_CHECK_ADDINFO", "").ToString
    Dim i As Integer
    Dim dtImage As New DataTable
    Dim tempivalue As Integer




#Region "WebCam Variable"
    Dim _touchlessMgr As TouchlessMgr
    Dim _dtFrameLast As DateTime
    Dim _nFrameCount As Integer
    Dim _fAddingMarker As Boolean
    Dim _latestFrame As Image
    Dim _drawSelectionAdornment As Boolean
    Dim _markerCenter As Point
    Dim _markerRadius As Single
    Dim cmbxWebCam As New ComboBox
#End Region
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Public Sub New(ByVal tagNo As String, ByVal itemId As Integer, Optional ByVal PurchaseDetail As Boolean = False, Optional ByVal Tag As Boolean = True, Optional ByVal dtt As DataTable = Nothing)

        InitializeComponent()
        If _TagDupPrint Then btnDuplicate.Visible = True : btnDuplicate.Enabled = True
        If _TagImgPrint Then btnImagePrint.Visible = True : btnImagePrint.Enabled = True
        Me.ShowPurchaseDetail = PurchaseDetail
        If GetAdmindbSoftValue("PURTAB", "N") = "N" Then
            Me.ShowPurchaseDetail = False
        End If
        If Not defaultPic.EndsWith("\") Then defaultPic += "\"
        Me.tagNo = tagNo
        Me.itemId = itemId
        Me.dtImage = dtt.Copy
        Dim dtStone As New DataTable
        Dim dtMisc As New DataTable
        Dim dtMultiMetal As New DataTable

        Dim Rate As Decimal = Nothing
        Dim itemTypeId As Integer = Nothing
        Dim CheckDAte As Date = GetEntryDate(GetServerDate)
        itemTypeId = Val(objGPack.GetSqlValue("SELECT ITEMTYPEID FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAG", "CITEMTAG") & " WHERE TAGNO = '" & tagNo & "' AND ITEMID = " & itemId & ""))
        If itemTypeId <> 0 Then
            Dim PURITYID As String = objGPack.GetSqlValue("SELECT PURITYID FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = " & itemTypeId & " AND RATEGET = 'Y'", , )
            If PURITYID <> "" Then Rate = Val(GetRate_Purity(CheckDAte, PURITYID)) Else GoTo weight_rate
        Else
weight_rate:
            Rate = Val(GetRate(CheckDAte, objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & itemId & "")))
        End If
        strSql = " DECLARE @DEFPATH VARCHAR(200)"
        strSql += vbCrLf + " SELECT @DEFPATH = '" & defaultPic & "'"
        strSql += vbCrLf + " SELECT CASE WHEN PCTFILE <> '' THEN @DEFPATH + PCTFILE END AS PCTFILE,T.TAGNO,T.PCS,T.GRSWT,T.LESSWT,T.NETWT,T.SALVALUE "
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAGSTONE", "CITEMTAGSTONE") & " WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)AS DIAPCS"
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAGSTONE", "CITEMTAGSTONE") & " WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)AS DIAWT"
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAGSTONE", "CITEMTAGSTONE") & " WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')),0)AS STNPCS"
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAGSTONE", "CITEMTAGSTONE") & " WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S') ),0)AS STNWT"
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAGSTONE", "CITEMTAGSTONE") & " WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')),0)AS PREPCS"
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAGSTONE", "CITEMTAGSTONE") & " WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P') ),0)AS PREWT"
        strSql += vbCrLf + " ,CONVERT(VARCHAR,RECDATE,103)RECDATE"
        strSql += vbCrLf + " ,CONVERT(VARCHAR,CHKDATE,103)CHKDATE"
        strSql += vbCrLf + " ,CONVERT(VARCHAR,ISSDATE,103)ISSDATE,isnull(ISSREFNO,'') ISSREFNO"
        strSql += vbCrLf + " ,CASE WHEN ISNULL(CONVERT(VARCHAR,ISSDATE,103),'') = '' THEN DATEDIFF(DD,RECDATE,GETDATE()) ELSE DATEDIFF(DD,RECDATE,ISSDATE) END AGING"
        strSql += vbCrLf + " ,MAXWASTPER,MAXWAST,MAXMCGRM,MAXMC,ISNULL(NARRATION,'') NARRATION,ISNULL(STYLENO,'') STYLENO,ISNULL(ORDREPNO,'') ORDREPNO,T.ITEMID,T.SUBITEMID"
        strSql += vbCrLf + " ,(SELECT top 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID) + ' ['+ CONVERT(VARCHAR,T.ITEMID) + ']' AS ITEM"
        strSql += vbCrLf + " ,CASE WHEN T.SUBITEMID > 0 THEN (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = T.ITEMID AND SUBITEMID = T.SUBITEMID) + ' ['+ CONVERT(VARCHAR,T.SUBITEMID) + ']' ELSE '' END AS SUBITEM"
        strSql += vbCrLf + " ,(SELECT top 1 ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID)AS COUNTER"
        strSql += vbCrLf + " ,(SELECT DESIGNERNAME+ (CASE WHEN ISNULL(SEAL,'')<>'' THEN '('+SEAL+')' ELSE '' END) FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)AS DESIGNER"
        strSql += vbCrLf + " ,(SELECT top 1 NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = T.ITEMTYPEID)AS ITEMTYPE"
        strSql += vbCrLf + " ,(SELECT top 1 USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = T.USERID)AS USERNAME"
        strSql += vbCrLf + " ,(SELECT top 1 USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = R.USERID)AS SCANERNAME "
        strSql += vbCrLf + " ,(SELECT top 1 EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = R.EMPID)AS EMPNAME "
        strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = T.COSTID)AS COSTNAME,CONVERT(VARCHAR,ACTUALRECDATE,103)ACTUALRECDATE,APPROVAL"
        strSql += vbCrLf + " ,TOFLAG,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = T.TCOSTID)AS TCOSTNAME"
        strSql += vbCrLf + " ,(SELECT TOP 1 SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZEID = T.SIZEID AND ITEMID=T.ITEMID)AS SIZE"
        strSql += vbCrLf + " ,(SELECT TOP 1 DESCRIPT FROM " & cnAdminDb & "..PURITEMTAG WHERE TAGNO = T.TAGNO)DESCRIPT"
        strSql += vbCrLf + " ,(SELECT TOP 1 PURWASTAGE FROM " & cnAdminDb & "..PURITEMTAG WHERE TAGNO = T.TAGNO)PURWASTAGE"
        strSql += vbCrLf + " ,(SELECT TOP 1 PURTOUCH FROM " & cnAdminDb & "..PURITEMTAG WHERE TAGNO = T.TAGNO)PURTOUCH"
        strSql += vbCrLf + " ,(SELECT TOP 1 LOTNO FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = T.LOTSNO)LOTNO"
        strSql += vbCrLf + " ,(SELECT TOP 1 TRANNO FROM " & cnStockDb & "..RECEIPT WHERE SNO IN (SELECT TOP 1 RECSNO FROM " & cnStockDb & "..LOTISSUE WHERE LOTSNO = T.LOTSNO))RECEIPTNO"
        strSql += vbCrLf + " ,T.GRSNET"
        strSql += vbCrLf + " ,CONVERT(VARCHAR,RESDATE,103)DUPLDATE,ISNULL(TRANINVNO,'')TRANINVNO"
        strSql += vbCrLf + " FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAG", "CITEMTAG") & " AS T "
        strSql += vbCrLf + " LEFT JOIN " & cnStockDb & "..ISSUE AS R ON T.TAGNO=R.TAGNO"
        strSql += vbCrLf + "WHERE T.TAGNO = '" & tagNo & "' AND T.ITEMID = " & itemId & ""
        If Not cnCentStock Then strSql += vbCrLf + " AND T.COMPANYID = '" & GetStockCompId() & "'"
        strSql += vbCrLf + " ORDER BY R.TRANTYPE DESC"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            With dt.Rows(0)
                If IO.File.Exists(.Item("PCTFILE").ToString) = False Then
                    bMap = My.Resources.no_photo
                Else
                    Dim Finfo As IO.FileInfo
                    Finfo = New IO.FileInfo(.Item("PCTFILE").ToString)
                    Dim fileStr As New IO.FileStream(.Item("PCTFILE").ToString, IO.FileMode.Open, IO.FileAccess.Read)
                    Try
                        bMap = Bitmap.FromStream(fileStr)
                    Catch ex As Exception
                    Finally
                        fileStr.Close()
                    End Try
                End If
                AutoImageSizer(bMap, picTagImage, PictureBoxSizeMode.CenterImage)

                tBarImageZoomer.Minimum = picTagImage.Width
                tBarImageZoomer.Maximum = 1000
                SetLocation()
                Me.Text = .Item("ITEM").ToString & " [" & .Item("TAGNO").ToString & "]"
                lblItem.Text = .Item("ITEM").ToString
                lblTagNo.Text = .Item("TAGNO").ToString
                lblSubItem.Text = .Item("SUBITEM").ToString
                If Not Val(.Item("PCS").ToString) = 0 Then
                    lblPcs.Text = .Item("PCS").ToString
                End If
                If Not Val(Val(.Item("GRSWT").ToString)) = 0 Then
                    lblGrsWt.Text = Format(Val(.Item("GRSWT").ToString), "0.000")
                End If
                If Not (Val(.Item("LESSWT").ToString)) Then
                    lblLessWt.Text = Format(Val(.Item("LESSWT").ToString), "0.000")
                End If
                If Not Val(Val(.Item("NETWT").ToString)) Then
                    lblNetWt.Text = Format(Val(.Item("NETWT").ToString), "0.000")
                End If
                If Not (Val(.Item("SALVALUE").ToString)) Then
                    lblSaleValue.Text = Format(Val(.Item("SALVALUE").ToString), "0.00")
                End If
                If Not (Val(.Item("DIAPCS").ToString)) = 0 Then
                    lblDiaPcs.Text = .Item("DIAPCS").ToString
                End If
                If Not (Val(.Item("DIAWT").ToString)) = 0 Then
                    lblDiaWt.Text = .Item("DIAWT").ToString
                End If
                If Not (Val(.Item("STNPCS").ToString)) = 0 Then
                    lblStnPcs.Text = .Item("STNPCS").ToString
                End If
                If Not (Val(.Item("STNWT").ToString)) = 0 Then
                    lblStnWt.Text = .Item("STNWT").ToString
                End If
                If Not (Val(.Item("PREPCS").ToString)) = 0 Then
                    lblPrePcs.Text = .Item("PREPCS").ToString
                End If
                If Not (Val(.Item("PREWT").ToString)) = 0 Then
                    lblPreWt.Text = .Item("PREWT").ToString
                End If
                If Not (Val(.Item("MAXWASTPER").ToString)) = 0 Then
                    lblWastagePer.Text = .Item("MAXWASTPER").ToString
                End If
                If Not (Val(.Item("MAXWAST").ToString)) = 0 Then
                    lblWastage.Text = .Item("MAXWAST").ToString
                End If
                If Not (Val(.Item("MAXMCGRM").ToString)) = 0 Then
                    lblMcGrm.Text = .Item("MAXMCGRM").ToString
                End If
                If Not _MCONWTAMT And mcasvaper(Val(.Item("ITEMID").ToString), Val(.Item("SUBITEMID").ToString)) = True Then
                    Dim wast As Double = IIf(_McWithWastage, Val(lblWastage.Text), 0)
                    Dim mc As Double = 0
                    If _MCONGRSNET Then
                        mc = IIf(.Item("GRSNET").ToString = "G", (Val(lblGrsWt.Text) + wast) * Rate, _
                        (Val(lblNetWt.Text) + wast) * Rate)
                    Else
                        mc = IIf(_MCCALCON_ITEM_GRS, (Val(lblGrsWt.Text) + wast) * Rate, _
            (Val(lblNetWt.Text) + wast) * Rate)
                    End If
                    mc = (mc * Val(lblMcGrm.Text)) / 100
                    lblMc.Text = mc.ToString
                Else

                    If Not (Val(.Item("MAXMC").ToString)) = 0 Then
                        lblMc.Text = .Item("MAXMC").ToString
                    End If

                End If
                If .Item("APPROVAL").ToString = "A" Then
                    lblapprovalissue.Text = "Approval Issue"
                    lblapprovalissue.Visible = True
                Else
                    lblapprovalissue.Visible = False
                End If
                '584
                If .Item("ISSDATE").ToString <> "" And .Item("TOFLAG").ToString = "SA" Then
                    lblapprovalissue.Text = "Sales"
                    lblapprovalissue.Visible = True
                ElseIf .Item("ISSDATE").ToString <> "" And .Item("TOFLAG").ToString = "MI" Then
                    lblapprovalissue.Text = "Misc Issue"
                    lblapprovalissue.Visible = True
                ElseIf .Item("TOFLAG").ToString = "TR" Then
                    lblapprovalissue.Text = "Transfered To " & .Item("TCOSTNAME").ToString
                    lblapprovalissue.Visible = True
                Else
                    lblapprovalissue.Visible = False
                End If
                If .Item("APPROVAL").ToString = "R" Then
                    lblapprovalissue.Text = "Booked"
                    lblapprovalissue.Visible = True
                End If
                '584
                lblPath.Text = "FileName : " + .Item("PCTFILE").ToString '' + "\" + .Item("PCTFILE").ToString
                Dim recDate As String
                Dim actRecDate As String
                actRecDate = .Item("ACTUALRECDATE").ToString
                recDate = .Item("RECDATE").ToString
                If actRecDate <> recDate Then
                    lblRecDate.Text = recDate + "(" + actRecDate + ")"
                Else
                    lblRecDate.Text = recDate
                End If
                lblTrfNo.Text = .Item("TRANINVNO").ToString
                lblRecNo.Text = .Item("RECEIPTNO").ToString
                lblDupdate.Text = .Item("DUPLDATE").ToString
                lblIssDate.Text = .Item("ISSDATE").ToString
                lblLotNo.Text = .Item("LOTNO").ToString
                lblIssueNo.Text = IIf(.Item("ISSREFNO").ToString <> 0, .Item("ISSREFNO").ToString, "")
                lblAging.Text = .Item("AGING").ToString
                lblStyleno.Text = .Item("STYLENO").ToString
                lblTagDescript.Text = .Item("NARRATION").ToString
                lblChkDate.Text = .Item("CHKDATE").ToString
                lblDesigner.Text = .Item("DESIGNER").ToString
                lblCounter.Text = .Item("COUNTER").ToString
                lblItemType.Text = .Item("ITEMTYPE").ToString
                lblCostCentre.Text = .Item("COSTNAME").ToString
                lblOrderRepNo.Text = .Item("ORDREPNO").ToString
                Label24.Text = .Item("USERNAME").ToString
                lblscanname.Text = .Item("SCANERNAME").ToString
                lblEmpName.Text = .Item("EMPNAME").ToString
                lblDescript.Text = .Item("DESCRIPT").ToString
                lblPtouch.Text = .Item("PURTOUCH").ToString
                lblPwast.Text = .Item("PURWASTAGE").ToString
                lblSize.Text = .Item("SIZE").ToString
            End With
        End If

        If GetAdmindbSoftValue("TAGCHKPURRATE", "N") = "Y" Then
            strSql = "SELECT PURRATE FROM " & cnAdminDb & "..PURITEMTAG AS T WHERE T.TAGNO ='" & tagNo & "' AND T.ITEMID = " & itemId & ""
            Rate = Val(objGPack.GetSqlValue(strSql))
            If Rate = 0 Then
                strSql = "SELECT BOARDRATE FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAG", "CITEMTAG") & " AS T WHERE T.TAGNO ='" & tagNo & "' AND T.ITEMID = " & itemId & ""
                Rate = Val(objGPack.GetSqlValue(strSql))
            End If
        End If

        strSql = vbCrLf + "		IF (SELECT 1 FROM SYSOBJECTS WHERE NAME='TEMPRESULT') > 0 DROP TABLE TEMPRESULT"
        strSql += vbCrLf + "		SELECT DISTINCT T.ITEMID,CASE WHEN T.GRSNET ='N' THEN 'NET WEIGHT'  WHEN T.GRSNET ='G' THEN 'GROSS WEIGHT' END AS PARTICULAR,"
        strSql += vbCrLf + "		T.PCS,CASE WHEN T.GRSNET ='N' THEN T.NETWT  WHEN T.GRSNET ='G' THEN T.GRSWT END AS WEIGHT," & Rate & " AS RATE , 0 AS AMOUNT,T.MAXWAST,T.MAXMC,P.PURRATE,"
        strSql += vbCrLf + "		CASE WHEN P.PURGRSNET = 'N' THEN 'NET WEIGHT' WHEN P.PURGRSNET = 'G' THEN 'GROSS WEIGHT' END AS P_PARTICULAR,CASE WHEN P.PURGRSNET ='N' THEN P.PURNETWT  WHEN P.PURGRSNET ='G' THEN T.GRSWT END AS PWEIGHT,P.PURWASTAGE,P.PURMC,P.PURTOUCH,' 'COLHEAD"
        strSql += vbCrLf + "		,SALEMODE,SALVALUE INTO TEMPRESULT FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAG", "CITEMTAG") & " AS T LEFT OUTER JOIN " & cnAdminDb & "..PURITEMTAG AS P ON P.TAGSNO = T.SNO"
        strSql += vbCrLf + "        WHERE T.TAGNO ='" & tagNo & "' AND T.ITEMID = " & itemId & ""
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = vbCrLf + " ALTER TABLE TEMPRESULT ADD EMPTY VARCHAR(250)"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPSALEVALUE')>0 DROP TABLE TEMPSALEVALUE"
        strSql += vbCrLf + " SELECT SVALUE,STAX,SVALUE+STAX AS SNETVALUE"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),PURVALUE-ISNULL(PURTAX,0)) PVALUE,PURTAX PTAX,PURVALUE PNETVALUE,CONVERT(NUMERIC(15,3),(PURVALUE*PURLCOSTPER)/100) AS PURLCOSTPER,PURLCOST"
        strSql += vbCrLf + " INTO TEMPSALEVALUE"
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " 	SELECT SUM(SALVALUE)SVALUE,SUM(SALVALUE)*(CONVERT(NUMERIC(18,2),SALESTAX)/100) AS STAX"
        strSql += vbCrLf + " 	FROM"
        strSql += vbCrLf + " 	("
        strSql += vbCrLf + "    SELECT T.ITEMID"
        strSql += vbCrLf + "  	, CASE WHEN T.SALEMODE NOT IN ('F','R') THEN ((CASE WHEN T.GRSNET = 'G' THEN T.GRSWT ELSE T.NETWT END + MAXWAST)"
        strSql += vbCrLf + "  	* " & Rate & ")  + MAXMC ELSE SALVALUE END AS SALVALUE,C.SALESTAX"
        strSql += vbCrLf + " 	FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAG", "CITEMTAG") & " AS T "
        strSql += vbCrLf + " 	INNER JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID = T.ITEMID"
        strSql += vbCrLf + " 	INNER JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
        strSql += vbCrLf + " 	WHERE T.TAGNO = '" & tagNo & "' AND T.ITEMID = " & itemId & ""
        strSql += vbCrLf + " 	UNION ALL "
        strSql += vbCrLf + " 	SELECT T.STNITEMID,T.STNAMT,C.SALESTAX"
        strSql += vbCrLf + " 	FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAGSTONE", "CITEMTAGSTONE") & " AS T "
        strSql += vbCrLf + " 	INNER JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID = T.ITEMID"
        strSql += vbCrLf + " 	INNER JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
        strSql += vbCrLf + " 	WHERE T.TAGNO = '" & tagNo & "' AND T.ITEMID = " & itemId & ""
        strSql += vbCrLf + " 	)X GROUP BY SALESTAX"
        strSql += vbCrLf + " )Y"
        strSql += vbCrLf + " FULL JOIN (SELECT * FROM " & cnAdminDb & "..PURITEMTAG WHERE TAGNO = '" & tagNo & "' AND ITEMID = " & itemId & ")AS P ON P.TAGNO = '" & tagNo & "' AND P.ITEMID = " & itemId & ""
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        Dim salValue As Decimal = Val(objGPack.GetSqlValue("SELECT SVALUE FROM TEMPSALEVALUE"))
        If salValue <> 0 Then
            lblSaleValue.Text = Format(salValue, "0.00")
        End If
        strSql = vbCrLf + " SET CONCAT_NULL_YIELDS_NULL OFF"
        strSql += vbCrLf + " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPTAGSTONE') > 0 DROP TABLE TEMPTAGSTONE"
        strSql += vbCrLf + " SELECT DISTINCT CASE WHEN ISNULL(SM.SUBITEMNAME,'') = '' THEN IM.ITEMNAME "
        If _FourCMaintain Then
            strSql += vbCrLf + " + ' ' + SC.CUTNAME + ' ' + SR.COLORNAME + ' ' + ST.CLARITYNAME + ' ' + SP.SHAPENAME"
        End If
        strSql += vbCrLf + " ELSE SM.SUBITEMNAME "
        If _FourCMaintain Then
            strSql += vbCrLf + " + ' ' + SC.CUTNAME + ' ' + SR.COLORNAME + ' ' + ST.CLARITYNAME + ' ' + SP.SHAPENAME"
        End If
        strSql += vbCrLf + " END AS SPARTICULAR"
        strSql += vbCrLf + " ,T.STNPCS SPCS,T.STNWT SWEIGHT,T.STNRATE SRATE,T.STNAMT SAMOUNT"
        strSql += vbCrLf + " ,T.SNO KEYNO"
        strSql += vbCrLf + " INTO TEMPTAGSTONE"
        strSql += vbCrLf + " FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAGSTONE", "CITEMTAGSTONE") & " T "
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.STNITEMID"
        strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = T.STNITEMID AND SM.SUBITEMID = T.STNSUBITEMID"
        If _FourCMaintain Then
            strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..STNCUT  AS SC ON SC.CUTID =T.CUTID "
            strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..STNCOLOR  AS SR ON SR.COLORID =T.COLORID "
            strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..STNCLARITY  AS ST ON ST.CLARITYID=T.CLARITYID "
            strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..STNSHAPE  AS SP ON SP.SHAPEID=T.SHAPEID "
        End If
        strSql += vbCrLf + " WHERE T.TAGSNO IN (SELECT SNO FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAG", "CITEMTAG") & " WHERE TAGNO = '" & tagNo & "' AND ITEMID = " & itemId & ") "
        strSql += vbCrLf + " "
        strSql += vbCrLf + " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPPURTAGSTONE') > 0 DROP TABLE TEMPPURTAGSTONE"
        strSql += vbCrLf + " SELECT CASE WHEN ISNULL(SM.SUBITEMNAME,'') = '' THEN IM.ITEMNAME ELSE SM.SUBITEMNAME END AS PPARTICULAR"
        strSql += vbCrLf + " ,T.STNPCS PPCS,T.STNWT PWEIGHT,T.PURRATE PRATE,T.PURAMT PAMOUNT"
        strSql += vbCrLf + " ,T.STNSNO KEYNO"
        strSql += vbCrLf + " INTO TEMPPURTAGSTONE"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..PURITEMTAGSTONE T "
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.STNITEMID"
        strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = T.STNITEMID AND SM.SUBITEMID = T.STNSUBITEMID"
        strSql += vbCrLf + " WHERE T.TAGSNO IN (SELECT SNO FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAG", "CITEMTAG") & " WHERE TAGNO = '" & tagNo & "' AND ITEMID = " & itemId & ") "
        strSql += vbCrLf + " SET CONCAT_NULL_YIELDS_NULL ON"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + "		IF (SELECT 1 FROM SYSOBJECTS WHERE NAME='TEMPFINAL') > 0 DROP TABLE TEMPFINAL"
        strSql += vbCrLf + "		SELECT 1ID,ITEMID,PARTICULAR AS SPARTICULAR,PCS AS SPCS,WEIGHT AS SWEIGHT,RATE AS SRATE,AMOUNT AS SAMOUNT,1RESULT,EMPTY,COLHEAD,"
        strSql += vbCrLf + "		P_PARTICULAR AS PPARTICULAR,PCS AS PPCS,PWEIGHT AS PWEIGHT,CASE WHEN PURTOUCH > 0 THEN 0 ELSE PURRATE END AS PRATE,CONVERT(NUMERIC(15,3),AMOUNT) AS PAMOUNT,SALEMODE,SALVALUE	INTO TEMPFINAL FROM TEMPRESULT"
        strSql += vbCrLf + "		UNION ALL"
        If ShowPurchaseDetail Then
            strSql += vbCrLf + "		SELECT 2ID,ITEMID,CASE WHEN PURTOUCH > 0 THEN 'WASTAGE / WEIGHT ' + '* ' + CONVERT(vARCHAR,PURTOUCH) + '%' ELSE 'WASTAGE' END AS SPARTICULAR,0 AS SPCS,MAXWAST AS SWEIGHT,RATE AS SRATE,AMOUNT AS SAMOUNT,1RESULT,EMPTY,COLHEAD,"
            strSql += vbCrLf + "		CASE WHEN PURTOUCH > 0 THEN 'WEIGHT ' + '* ' + CONVERT(vARCHAR,PURTOUCH) + '%' ELSE 'WASTAGE' END AS PPARTICULAR,0 AS PPCS,CASE WHEN PURTOUCH > 0 THEN (PWEIGHT * (PURTOUCH/100)) ELSE PURWASTAGE END AS PWEIGHT,PURRATE AS PRATE,AMOUNT AS PAMOUNT,SALEMODE,SALVALUE	 FROM TEMPRESULT"
            strSql += vbCrLf + "		UNION ALL"
        Else
            strSql += vbCrLf + "		SELECT 2ID,ITEMID,'WASTAGE' AS SPARTICULAR,0 AS SPCS,MAXWAST AS SWEIGHT,RATE AS SRATE,AMOUNT AS SAMOUNT,1RESULT,EMPTY,COLHEAD,"
            strSql += vbCrLf + "		'WASTAGE' AS PPARTICULAR,0 AS PPCS,PURWASTAGE  AS PWEIGHT,PURRATE AS PRATE,AMOUNT AS PAMOUNT,SALEMODE,SALVALUE	 FROM TEMPRESULT"
            strSql += vbCrLf + "		UNION ALL"
        End If
        strSql += vbCrLf + "		SELECT 3ID,'' as ITEMID,'LABOUR' AS SPARTICULAR,0 AS SPCS,0 AS SWEIGHT,RATE AS SRATE,MAXMC AS SAMOUNT,1RESULT,EMPTY,COLHEAD,"
        strSql += vbCrLf + "		'LABOUR' AS PPARTICULAR,0 AS PCS,0 AS PWEIGHT,0 AS PRATE,PURMC AS PAMOUNT,SALEMODE,SALVALUE	 FROM TEMPRESULT"
        strSql += vbCrLf + "		UNION ALL"
        strSql += vbCrLf + "		SELECT DISTINCT 4ID,'' AS ITEMID,'STUDED DETAILS' AS SPARTICULAR,0 AS SPCS ,0 AS SWEIGHT,0 AS SRATE,0 AS SAMOUNT,1 RESULT,'','D',"
        strSql += vbCrLf + "		'STUDED DETAILS' AS PPARTICULAR,0 AS PPCS ,0 AS WEIGHT,0 AS RATE,0 AS AMOUNT,'' SALEMODE,0 SALVALUE"
        strSql += vbCrLf + "        FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAGSTONE", "CITEMTAGSTONE") & " T"
        strSql += vbCrLf + "        WHERE T.TAGSNO IN (SELECT SNO FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAG", "CITEMTAG") & " WHERE TAGNO = '" & tagNo & "' AND ITEMID = " & itemId & ")"
        strSql += vbCrLf + "		UNION ALL"
        strSql += vbCrLf + "        SELECT "
        strSql += vbCrLf + "        5ID,'' AS ITEMID,SPARTICULAR,SPCS"
        strSql += vbCrLf + "        ,SWEIGHT,SRATE,SAMOUNT,1 RESULT,' ' AS EMPTY,'' AS COLHEAD "
        strSql += vbCrLf + "        ,PPARTICULAR,PPCS,PWEIGHT,PRATE,PAMOUNT,'' SALEMODE,0 SALVALUE"
        strSql += vbCrLf + "        FROM TEMPTAGSTONE AS S"
        strSql += vbCrLf + "        FULL JOIN TEMPPURTAGSTONE AS P ON P.KEYNO = S.KEYNO"


        strSql += vbCrLf + "		UNION ALL"
        strSql += vbCrLf + "		SELECT DISTINCT 6ID,'' AS ITEMID,'MISC DETAILS' AS SPARTICULAR,0 AS SPCS ,0 AS SWEIGHT,0 AS SRATE,0 AS SAMOUNT,1 RESULT,'','D',"
        strSql += vbCrLf + "		'MISC DETAILS' AS PPARTICULAR,0 AS PPCS ,0 AS WEIGHT,0 AS RATE,0 AS AMOUNT,'' SALEMODE,0 SALVALUE"
        strSql += vbCrLf + "        FROM " & cnAdminDb & "..ITEMTAGMISCCHAR T"
        strSql += vbCrLf + "        WHERE T.TAGSNO IN (SELECT SNO FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAG", "CITEMTAG") & " WHERE TAGNO = '" & tagNo & "' AND ITEMID = " & itemId & ")"

        strSql += vbCrLf + "        UNION ALL"
        strSql += vbCrLf + "        SELECT 7ID,'' AS ITEMID"
        strSql += vbCrLf + "        ,MISCNAME AS SPARTICULAR "
        strSql += vbCrLf + "        ,NULL AS SPCS,NULL AS SWEIGHT,NULL AS SRATE,AMOUNT AS SAMOUNT,1 RESULT,' ' AS EMPTY,'' AS COLHEAD "
        strSql += vbCrLf + "        ,MISCNAME AS PPARTICULAR "
        strSql += vbCrLf + "        ,NULL AS PPCS,NULL AS RWEIGHT,NULL AS PRATE,PURAMOUNT AS PAMOUNT,'' SALEMODE,0 SALVALUE"
        strSql += vbCrLf + "        FROM  "
        strSql += vbCrLf + "            ( "
        strSql += vbCrLf + "            SELECT  M.MISCNAME,T.AMOUNT,P.PURAMOUNT"
        strSql += vbCrLf + "            FROM " & cnAdminDb & "..ITEMTAGMISCCHAR T"
        strSql += vbCrLf + "            INNER JOIN " & cnAdminDb & "..MISCCHARGES AS M ON M.MISCID = T.MISCID"
        strSql += vbCrLf + "            LEFT OUTER JOIN " & cnAdminDb & "..PURITEMTAGMISCCHAR AS P ON P.MISSNO = T.SNO"
        strSql += vbCrLf + "            WHERE T.TAGSNO IN (SELECT SNO FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAG", "CITEMTAG") & " WHERE TAGNO = '" & tagNo & "' AND ITEMID = " & itemId & ") "
        strSql += vbCrLf + "            )X"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + ""
        strSql += vbCrLf + "		UPDATE TEMPFINAL SET SAMOUNT = CASE WHEN SALEMODE = 'F' THEN SALVALUE ELSE SWEIGHT * SRATE END"
        strSql += vbCrLf + "		WHERE itemid = " & itemId
        strSql += vbCrLf + ""
        strSql += vbCrLf + "		UPDATE TEMPFINAL SET PAMOUNT = PWEIGHT * PRATE"
        strSql += vbCrLf + "		WHERE itemid = " & itemId
        strSql += vbCrLf + ""
        strSql += vbCrLf + "		INSERT INTO TEMPFINAL (ID,SPARTICULAR,SAMOUNT,RESULT,EMPTY,COLHEAD,PPARTICULAR,PAMOUNT) "
        strSql += vbCrLf + "        SELECT '-3','GRAND TOTAL'SPARTICULAR,SVALUE,1 RESULT,''EMPTY,'T'COLHEAD,'GRAND TOTAL',PVALUE"
        strSql += vbCrLf + "        FROM TEMPSALEVALUE"
        If PUR_LANDCOST Then
            strSql += vbCrLf + "        UNION ALL"
            strSql += vbCrLf + "        SELECT '-1','LANDING COST'SPARTICULAR,NULL,1 RESULT,''EMPTY,'T'COLHEAD,'',PURLCOSTPER"
            strSql += vbCrLf + "        FROM TEMPSALEVALUE"
        Else
            strSql += vbCrLf + "        UNION ALL"
            strSql += vbCrLf + "        SELECT '-2','TAX'SPARTICULAR,STAX,1 RESULT,''EMPTY,'T'COLHEAD,'TAX',PTAX"
            strSql += vbCrLf + "        FROM TEMPSALEVALUE"
        End If
        strSql += vbCrLf + "        UNION ALL"
        strSql += vbCrLf + "        SELECT '0','NET TOTAL'SPARTICULAR,SNETVALUE,1 RESULT,''EMPTY,'T'COLHEAD,'NET TOTAL'," & IIf(PUR_LANDCOST, "PURLCOST", "PNETVALUE") & ""
        strSql += vbCrLf + "        FROM TEMPSALEVALUE"
        'strSql += vbCrLf + "		SELECT '0','TOTAL',(SELECT SUM(SAMOUNT) FROM TEMPFINAL),1,'','T','TOTAL',(SELECT SUM(PAMOUNT) FROM TEMPFINAL)"
        strSql += vbCrLf + ""
        strSql += vbCrLf + "        UPDATE TEMPFINAL SET SRATE = 0 WHERE SWEIGHT = 0"
        strSql += vbCrLf + "        UPDATE TEMPFINAL SET PRATE = 0 WHERE PWEIGHT = 0"
        strSql += vbCrLf + "		UPDATE TEMPFINAL SET SPCS = CASE WHEN SPCS = 0 THEN NULL ELSE SPCS END ,"
        strSql += vbCrLf + "		SWEIGHT = CASE WHEN SWEIGHT = 0 THEN NULL ELSE SWEIGHT END,"
        strSql += vbCrLf + "		SRATE =  CASE WHEN SRATE = 0 THEN NULL ELSE SRATE END,"
        strSql += vbCrLf + "		SAMOUNT = CASE WHEN SAMOUNT = 0 THEN NULL ELSE SAMOUNT END,"
        strSql += vbCrLf + "		PPCS = CASE WHEN PPCS = 0 THEN NULL ELSE PPCS END,"
        strSql += vbCrLf + "		PWEIGHT = CASE WHEN PWEIGHT = 0 THEN NULL ELSE PWEIGHT END,"
        strSql += vbCrLf + "		PRATE =  CASE WHEN PRATE = 0 THEN NULL ELSE PRATE END,"
        strSql += vbCrLf + "		PAMOUNT = CASE WHEN PAMOUNT = 0 THEN NULL ELSE PAMOUNT END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = " IF (SELECT ISNULL(SAMOUNT,0) FROM TEMPFINAL WHERE SPARTICULAR='LABOUR')=0"
        strSql += vbCrLf + "		BEGIN  "
        strSql += vbCrLf + "		UPDATE TEMPFINAL SET SAMOUNT=("
        strSql += vbCrLf + "		SELECT SUM(MC)  FROM " & cnAdminDb & "..ITEMTAGMETAL AS M "
        strSql += vbCrLf + "		WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAG", "CITEMTAG") & " WHERE TAGNO = '" & tagNo & "' AND ITEMID = " & itemId & "))"
        strSql += vbCrLf + "		WHERE SPARTICULAR ='LABOUR'"
        strSql += vbCrLf + "		END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = ""
        strSql = "SELECT * FROM TEMPFINAL ORDER BY RESULT,[ID]"

        dtStone = New DataTable
        dtStone.Columns.Add("KEYNO", GetType(Integer))
        dtStone.Columns("KEYNO").AutoIncrement = True
        dtStone.Columns("KEYNO").AutoIncrementSeed = 0
        dtStone.Columns("KEYNO").AutoIncrementStep = 1
        da = New OleDb.OleDbDataAdapter(strSql, cn)
        da.Fill(dtStone)

        dtStone.Columns("KEYNO").SetOrdinal(dtStone.Columns.Count - 1)
        If Not dtStone.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        gridStoneView.DataSource = dtStone

        gridStoneView.BackgroundColor = Color.WhiteSmoke
        gridStoneView.Columns("COLHEAD").Visible = False
        gridStoneView.Columns("ID").Visible = False
        gridStoneView.Columns("ITEMID").Visible = False
        gridStoneView.Columns("RESULT").Visible = False
        gridStoneView.Columns("KEYNO").Visible = False
        gridStoneView.Columns("SALEMODE").Visible = False
        gridStoneView.Columns("SALVALUE").Visible = False

        
        gridStoneView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridStoneView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter


        With gridStoneView
            With .Columns("SPARTICULAR")
                .Resizable = DataGridViewTriState.False
                .Width = IIf(ShowPurchaseDetail = False, 30, 140)
                .HeaderText = "PARTICULAR"
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("SPCS")
                .Resizable = DataGridViewTriState.False
                .Width = 60 * IIf(ShowPurchaseDetail = False, 2, 1)
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .HeaderText = "PCS"
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("SWEIGHT")
                .Resizable = DataGridViewTriState.False
                .Width = 80 * IIf(ShowPurchaseDetail = False, 2, 1)
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "0.000"
                .HeaderText = "WEIGHT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("SRATE")
                .Resizable = DataGridViewTriState.False
                .Width = 80 * IIf(ShowPurchaseDetail = False, 2, 1)
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .HeaderText = "RATE"
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("SAMOUNT")
                .Resizable = DataGridViewTriState.False
                .Width = 100 * IIf(ShowPurchaseDetail = False, 2, 1)
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .HeaderText = "AMOUNT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("EMPTY")
                .HeaderText = ""
                .Resizable = DataGridViewTriState.False
                .Width = 10
                .Visible = ShowPurchaseDetail
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("PPARTICULAR")
                .Resizable = DataGridViewTriState.False
                .Width = 140 * IIf(ShowPurchaseDetail = False, 2, 1)
                '.Visible = ShowPurchaseDetail
                .Visible = False
                .HeaderText = "PARTICULAR"
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("SPARTICULAR")
                .Width = .Width + gridStoneView.Columns("PPARTICULAR").Width
            End With
            With .Columns("PPCS")
                .Width = 60 * IIf(ShowPurchaseDetail = False, 2, 1)
                .Resizable = DataGridViewTriState.False
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Visible = ShowPurchaseDetail
                .HeaderText = "PCS"
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("PWEIGHT")
                .Width = 80 * IIf(ShowPurchaseDetail = False, 2, 1)
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Resizable = DataGridViewTriState.False
                .DefaultCellStyle.Format = "0.000"
                .Visible = ShowPurchaseDetail
                .HeaderText = "WEIGHT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("PRATE")
                .Width = 80 * IIf(ShowPurchaseDetail = False, 2, 1)
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Resizable = DataGridViewTriState.False
                .Visible = ShowPurchaseDetail
                .HeaderText = "RATE"
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("PAMOUNT")
                .Width = 100 * IIf(ShowPurchaseDetail = False, 2, 1)
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Visible = ShowPurchaseDetail
                .Resizable = DataGridViewTriState.False
                .HeaderText = "AMOUNT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
        End With

        ''TAGMULTIMETAL
        strSql = " SELECT"
        strSql += " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = M.CATCODE) AS CATNAME"
        strSql += " ,GRSWT AS WEIGHT,RATE ,AMOUNT,WAST AS WASTAGE,MC "
        strSql += " FROM " & cnAdminDb & "..ITEMTAGMETAL AS M"
        strSql += " WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAG", "CITEMTAG") & " WHERE TAGNO = '" & tagNo & "' AND ITEMID = " & itemId & ")"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtMultiMetal)
        gridMultiMetal.DataSource = dtMultiMetal
        gridMultiMetal.BackgroundColor = Color.WhiteSmoke
        With gridMultiMetal
            With .Columns("CATNAME")
                .Width = 300
            End With
            With .Columns("WEIGHT")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("RATE")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("AMOUNT")
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
        End With

        Dim ServerSystemName As String = GetAdmindbSoftValue("SERVER_NAME")
        Dim ServerAdminName As String = GetAdmindbSoftValue("SERVER_ACCNAME")
        Dim ServerPwd As String = GetAdmindbSoftValue("SERVER_ACCPWD")
        If ServerSystemName <> "" And ServerAdminName <> "" And ServerPwd <> "" Then
            notify = New NotifyIcon()
            notify.Text = "Server UserName & Password Available "
            notify.Visible = True
            notify.Icon = My.Resources.email
            notify.ShowBalloonTip(20000, "Information", "UserName:" + ServerAdminName + " Pwd:" + ServerPwd, ToolTipIcon.Info)
        End If
        'i = i + 1
        'Me.AcceptButton = btnClose
    End Sub

    Private Function mcasvaper(ByVal ITEMID As Integer, ByVal SUBITEMID As Integer) As Boolean
        Dim mcasva As String
        mcasva = UCase(objGPack.GetSqlValue("Select MCASVAPER FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID =" & ITEMID & "", , "N"))
        If lblSubItem.Text <> "" Then
            mcasva = UCase(objGPack.GetSqlValue("SELECT MCASVAPER FROM " & cnAdminDb & "..SUBITEMMAST WHERE  ITEMID = " & ITEMID & " AND SUBITEMID = " & SUBITEMID & ""))
        End If
        If mcasva = "Y" Then Return True Else Return False
    End Function

    Function funcStyledgvViewHeader() As Integer
        Dim SalColWid As Integer = Nothing
        Dim EmtColWid As Integer = Nothing
        Dim PurColWid As Integer = Nothing
        If Not gridStoneView.Columns.Count > 0 Then Exit Function
        If Not dgvHeader.Columns.Count > 0 Then Exit Function
        With gridStoneView
            SalColWid = .Columns("SPARTICULAR").Width + _
                         .Columns("SPCS").Width + _
                        .Columns("SWEIGHT").Width + _
                        .Columns("SRATE").Width + _
                        .Columns("SWEIGHT").Width + 18

            EmtColWid = .Columns("EMPTY").Width

            PurColWid = .Columns("PPARTICULAR").Width + _
                         .Columns("PPCS").Width + _
                        .Columns("PWEIGHT").Width + _
                        .Columns("PRATE").Width + _
                        .Columns("PWEIGHT").Width + 18
        End With

        If dgvHeader.Columns.Count > 0 Then
            With dgvHeader
                .Columns("SALESDETAILS").Visible = True
                .Columns("SALESDETAILS").HeaderText = "SALES DETAILS"
                .Columns("SALESDETAILS").Width = SalColWid
                .Columns("EMPTY").Visible = True
                .Columns("EMPTY").HeaderText = ""
                .Columns("EMPTY").Width = EmtColWid
                .Columns("PURCHASEDETAILS").Visible = True
                .Columns("PURCHASEDETAILS").HeaderText = "PURCHASE DETAILS"
                .Columns("PURCHASEDETAILS").Width = PurColWid
             
            End With
        End If
    End Function

    Private Sub frmTagImageViewer_Format2_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        If btnCam.Visible = True Then
            If btnCam.Text = "Save" Then
                Try
                    _fAddingMarker = Not _fAddingMarker
                    RemoveHandler Me._touchlessMgr.CurrentCamera.OnImageCaptured, New EventHandler(Of CameraEventArgs)(AddressOf OnImageCaptured)
                    Me._touchlessMgr.CurrentCamera = Nothing
                    RemoveHandler Me.picTagImage.Paint, New PaintEventHandler(AddressOf drawLatestImage)
                Catch ex As Exception
                End Try
            End If
        End If
        If notify Is Nothing Then Exit Sub
        notify.Visible = False
        notify.Dispose()
    End Sub

    Private Sub frmTagImageViewer_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If pnlImage.Dock = DockStyle.Fill Then
                pnlImage.Dock = DockStyle.None
                pnlImage.Location = New Point(3, 3)
                pnlImage.Size = New Size(392, 346)
                pnlImage.Invalidate()
                picTagImage.Dock = DockStyle.Fill
                AutoImageSizer(bMap, picTagImage, PictureBoxSizeMode.CenterImage)
                tabMain.Visible = True
                pnlImage.BackColor = Color.Transparent
                SetLocation()
            Else
                Me.Close()
            End If
        End If
    End Sub
    Private Sub frmTagImageViewer_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        If btnCam.Visible = True Then
            If btnCam.Text = "Save" Then
                Try
                    RemoveHandler Me._touchlessMgr.CurrentCamera.OnImageCaptured, New EventHandler(Of CameraEventArgs)(AddressOf OnImageCaptured)
                    Me._touchlessMgr.CurrentCamera = Nothing
                    RemoveHandler Me.picTagImage.Paint, New PaintEventHandler(AddressOf drawLatestImage)
                Catch ex As Exception
                End Try
            End If
        End If
        Me.Close()
    End Sub
    Private Sub picTagImage_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles picTagImage.DoubleClick
        picTagImage.Dock = DockStyle.None
        AutoImageSizer(bMap, picTagImage, PictureBoxSizeMode.AutoSize)
        pnlImage.Dock = DockStyle.Fill
        tabMain.Visible = False
        pnlImage.BackColor = Color.White
        pnlImage.BringToFront()
        SetLocation()
    End Sub

    Private Sub picTagImage_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles picTagImage.MouseMove
        tBarImageZoomer.Focus()
    End Sub

    Private Sub frmTagImageViewer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If IMAGE_UPD_BILLTAG = False Then btnAttachImage.Visible = False : btnCam.Visible = False
        If TAG_CHECK_ADDINFO.Contains("C") Then
            lblCompanyName.Visible = True : Label36.Visible = True
        Else
            lblCompanyName.Visible = False : Label36.Visible = False
        End If
        If TAG_CHECK_ADDINFO.Contains("F") Then lblPath.Visible = True Else lblPath.Visible = False
        If TAG_CHECK_ADDINFO.Contains("P") Then btnWord.Visible = True Else btnWord.Visible = False
        For i As Integer = 0 To gridStoneView.RowCount - 1
            If gridStoneView.Rows(i).Cells("COLHEAD").Value.ToString = "T" Then
                gridStoneView.Rows(i).DefaultCellStyle.BackColor = Color.LightCoral
                gridStoneView.Rows(i).DefaultCellStyle.Font = New Font("Verdana", 8.25!, FontStyle.Bold)
            End If
            If gridStoneView.Rows(i).Cells("COLHEAD").Value.ToString = "D" Then
                gridStoneView.Rows(i).DefaultCellStyle.BackColor = Color.LightBlue
                gridStoneView.Rows(i).DefaultCellStyle.Font = New Font("Verdana", 8.25!, FontStyle.Bold)
            End If
        Next
        Dim dtdgvViewHeader As New DataTable
        dgvHeader.RowHeadersVisible = False
        dgvHeader.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvHeader.ColumnHeadersDefaultCellStyle.Font = New Font("Verdana", 9, FontStyle.Bold)
        dgvHeader.Enabled = False
        dgvHeader.Visible = ShowPurchaseDetail

        strSql = " SELECT '' AS SALESDETAILS,'' AS EMPTY,'' AS PURCHASEDETAILS"

        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtdgvViewHeader)
        dgvHeader.DataSource = dtdgvViewHeader

        Dim CompanyName As String = objGPack.GetSqlValue("SELECT COMPANYNAME  FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N' ")
        lblCompanyName.Text = CompanyName
        funcStyledgvViewHeader()

        'Newly Webcam
        _touchlessMgr = New TouchlessMgr()
        _dtFrameLast = DateTime.Now
        _fAddingMarker = False

        Webcam_Connection_Check()

    End Sub

    Private Sub btnPurhcase_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPurhcase.Click
        Dim objPur As New TagPurhcaseDetail
        objPur.LoadPurchaseDetail(itemId, tagNo)
        If objPur.gridView.RowCount > 0 Then
            objPur.ShowDialog()
        End If
    End Sub

    Private Sub gridStoneView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridStoneView.ColumnWidthChanged
        funcStyledgvViewHeader()
    End Sub
    Private Sub SetLocation()
        picTagImage.Invalidate()
        Dim x As Integer = (pnlImage.Width - picTagImage.Width) / 2
        Dim y As Integer = ((pnlImage.Height) - picTagImage.Height) / 2
        If pnlImage.Width <= picTagImage.Width Then x = 0
        If pnlImage.Height <= picTagImage.Height Then y = 0
        picTagImage.Location = New Point(x, y)
    End Sub

    Private Sub picTagImage_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles picTagImage.Resize
        SetLocation()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If dt.Rows(0).Item("APPROVAL").ToString = "A" Or dt.Rows(0).Item("ISSDATE").ToString <> "" Then
            If lblapprovalissue.Visible = False Then
                lblapprovalissue.Visible = True
            Else
                lblapprovalissue.Visible = False
            End If
        End If
    End Sub

    Private Sub btnTagHistory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTagHistory.Click
        strSql = "SELECT SEP AS PARTICULAR,RECDATE,TRANDATE,TAGNO, OLDTAGNO, IM.ITEMNAME,ITEMCTRNAME,COSTNAME AS COSTCENTRE, NARRATION  FROM ("
        strSql += vbCrLf + " SELECT CASE WHEN (ISNULL(ISSDATE,'')<>'' AND TOFLAG='SA') THEN 'SALES' WHEN (ISNULL(ISSDATE,'')<>'' AND TOFLAG='MI') THEN 'MISC ISSUE' "
        strSql += vbCrLf + " WHEN (ISNULL(ISSDATE,'')<>'' AND TOFLAG='TR') THEN 'TRANSFER '+TC.COSTNAME END SEP,"
        strSql += vbCrLf + " RECDATE,ISSDATE AS TRANDATE,TAGNO, '' OLDTAGNO, I.ITEMCTRNAME"
        strSql += vbCrLf + " ,T.ITEMID ,C.COSTNAME,CASE WHEN ISNULL(ISSDATE,'')='' THEN 2 WHEN TOFLAG IN ('SA','MI') THEN 1 ELSE 0 END ORDNO , '' AS NARRATION, 0 RESULT"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAG T "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER I ON T.ITEMCTRID=I.ITEMCTRID "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..COSTCENTRE C ON T.COSTID=C.COSTID "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..COSTCENTRE TC ON T.TCOSTID=TC.COSTID WHERE TAGNO= '" & tagNo & "'"
        strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..CTRANSFER WHERE TAGSNO=T.SNO)  "

        strSql += vbCrLf + " UNION ALL"

        strSql += vbCrLf + " SELECT CASE WHEN (ISNULL(ISSDATE,'')<>'' AND TOFLAG='SA') THEN 'SALES' WHEN (ISNULL(ISSDATE,'')<>'' AND TOFLAG='MI') THEN 'MISC ISSUE' "
        strSql += vbCrLf + " WHEN (ISNULL(ISSDATE,'')<>'' AND TOFLAG='TR') THEN 'TRANSFER '+TC.COSTNAME END SEP,"
        strSql += vbCrLf + " RECDATE,ISSDATE AS TRANDATE,TAGNO, T.OLDTAGNO ,"
        strSql += vbCrLf + " I.ITEMCTRNAME  ,T.ITEMID ,C.COSTNAME,"
        strSql += vbCrLf + " CASE WHEN ISNULL(ISSDATE,'')='' THEN 2 WHEN TOFLAG IN ('SA','MI') THEN 1 ELSE 0 END ORDNO , T.NARRATION AS NARRATION, 0 RESULT"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER I ON T.ITEMCTRID=I.ITEMCTRID LEFT JOIN " & cnAdminDb & "..COSTCENTRE C ON T.COSTID=C.COSTID "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..COSTCENTRE TC ON T.TCOSTID=TC.COSTID "
        strSql += vbCrLf + " WHERE T.TAGNO= '" & tagNo & "'"
        strSql += vbCrLf + " AND (NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..CTRANSFER WHERE TAGSNO=T.SNO) OR ISNULL(ISSDATE,'')<>'') "

        strSql += vbCrLf + " UNION ALL"

        strSql += vbCrLf + " SELECT CASE WHEN (ISNULL(T.ISSDATE,'')<>'' AND TOFLAG='SA') THEN 'SALES' WHEN (ISNULL(T.ISSDATE,'')<>'' AND TOFLAG='MI') THEN 'MISC ISSUE' "
        strSql += vbCrLf + " WHEN (ISNULL(T.ISSDATE,'')<>'' AND TOFLAG='TR') THEN 'TRANSFER '+TC.COSTNAME END SEP,T.RECDATE,T.ISSDATE AS TRANDATE,"
        strSql += vbCrLf + " T.TAGNO, '' OLDTAGNO, I.ITEMCTRNAME,T.ITEMID "
        strSql += vbCrLf + " ,C.COSTNAME,CASE WHEN ISNULL(T.ISSDATE,'')='' THEN 2 WHEN TOFLAG IN ('SA','MI') THEN 1 ELSE 0 END ORDNO  ,'' AS NARRATION, 0 RESULT"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..CTRANSFER T LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER I ON T.ITEMCTRID=I.ITEMCTRID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMTAG IT ON T.TAGSNO=IT.SNO AND T.COSTID=IT.COSTID AND T.ITEMCTRID=IT.ITEMCTRID LEFT JOIN " & cnAdminDb & "..COSTCENTRE TC ON IT.TCOSTID=TC.COSTID "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..COSTCENTRE C ON T.COSTID=C.COSTID WHERE T.TAGNO='" & tagNo & "'"
        strSql += vbCrLf + " UNION ALL"

        strSql += vbCrLf + " SELECT 'APPROVAL ISSUE TO-'+ ISNULL(P.PNAME,'') SEP,IT.RECDATE RECDATE,T.TRANDATE,T.TAGNO, '' OLDTAGNO, I.ITEMCTRNAME"
        strSql += vbCrLf + " ,T.ITEMID ,C.COSTNAME,0 ORDNO, '' AS NARRATION, 0 RESULT"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE T"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMTAG IT ON T.TAGNO=IT.TAGNO LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER I ON T.ITEMCTRID=I.ITEMCTRID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..COSTCENTRE C ON T.COSTID=C.COSTID "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CUSTOMERINFO CI ON CI.BATCHNO=T.BATCHNO"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..PERSONALINFO P ON P.SNO=CI.PSNO"
        strSql += vbCrLf + " WHERE T.TAGNO='" & tagNo & "' AND TRANTYPE='AI' AND ISNULL(T.CANCEL,'')=''"

        strSql += vbCrLf + " UNION ALL"

        strSql += vbCrLf + " SELECT 'APPROVAL RECEIPT FROM-'+ ISNULL(P.PNAME,'') SEP,IT.RECDATE RECDATE,T.TRANDATE,T.TAGNO, '' OLDTAGNO, I.ITEMCTRNAME"
        strSql += vbCrLf + " ,T.ITEMID ,C.COSTNAME,0 ORDNO, '' AS NARRATION, 0 RESULT  FROM " & cnStockDb & "..RECEIPT T"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMTAG IT ON T.TAGNO=IT.TAGNO LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER I ON T.ITEMCTRID=I.ITEMCTRID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..COSTCENTRE C ON T.COSTID=C.COSTID "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CUSTOMERINFO CI ON CI.BATCHNO=T.BATCHNO"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..PERSONALINFO P ON P.SNO=CI.PSNO"
        strSql += vbCrLf + " WHERE T.TAGNO='" & tagNo & "' AND TRANTYPE='AR' AND ISNULL(T.CANCEL,'')=''"

        strSql += vbCrLf + " UNION ALL"

        strSql += vbCrLf + " SELECT CASE WHEN (ISNULL(ISSDATE,'')<>'' AND TOFLAG='SA') THEN 'SALES' WHEN (ISNULL(ISSDATE,'')<>'' AND TOFLAG='MI') THEN 'MISC ISSUE' "
        strSql += vbCrLf + " WHEN (ISNULL(ISSDATE,'')<>'' AND TOFLAG='TR') THEN 'TRANSFER '+TC.COSTNAME END SEP,RECDATE,ISSDATE AS TRANDATE,"
        strSql += vbCrLf + " TAGNO, '' OLDTAGNO, I.ITEMCTRNAME"
        strSql += vbCrLf + " ,T.ITEMID ,C.COSTNAME,CASE WHEN ISNULL(ISSDATE,'')='' THEN 2 WHEN TOFLAG IN ('SA','MI') THEN 1 ELSE 0 END ORDNO ,'' AS NARRATION, 0 RESULT"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAG T "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER I ON T.ITEMCTRID=I.ITEMCTRID "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..COSTCENTRE C ON T.COSTID=C.COSTID "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..COSTCENTRE TC ON T.TCOSTID=TC.COSTID WHERE OLDTAGNO ='" & tagNo & "'"
        strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..CTRANSFER WHERE TAGSNO=T.SNO) "

        strSql += vbCrLf + " UNION ALL"

        strSql += vbCrLf + " SELECT '.' SEP,NULL RECDATE, NULL TRANDATE, NULL TAGNO, NULL OLDTAGNO,  NULL ITEMCTRNAME, NULL ITEMID, NULL COSTCENTRE, "
        strSql += vbCrLf + " NULL ORDNO, NULL  NARRATION, 1 RESULT"
        strSql += vbCrLf + " UNION ALL"

        strSql += vbCrLf + " SELECT CASE WHEN (ISNULL(ISSDATE,'')<>'' AND TOFLAG='SA') THEN 'SALES' WHEN (ISNULL(ISSDATE,'')<>'' AND TOFLAG='MI') THEN 'MISC ISSUE' "
        strSql += vbCrLf + " WHEN (ISNULL(ISSDATE,'')<>'' AND TOFLAG='TR') THEN 'TRANSFER '+TC.COSTNAME END SEP,"
        strSql += vbCrLf + " RECDATE,ISSDATE AS TRANDATE, T.TAGNO, T.OLDTAGNO ,  I.ITEMCTRNAME"
        strSql += vbCrLf + " ,T.ITEMID ,C.COSTNAME,CASE WHEN ISNULL(ISSDATE,'')='' THEN 2 WHEN TOFLAG IN ('SA','MI') THEN 1 ELSE 0 END ORDNO , T.NARRATION AS NARRATION, 2 RESULT"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER I ON T.ITEMCTRID=I.ITEMCTRID LEFT JOIN " & cnAdminDb & "..COSTCENTRE C ON T.COSTID=C.COSTID "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..COSTCENTRE TC ON T.TCOSTID=TC.COSTID WHERE OLDTAGNO='" & tagNo & "'"
        strSql += vbCrLf + " AND (NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..CTRANSFER WHERE TAGSNO=T.SNO) OR ISNULL(ISSDATE,'')<>'') "

        strSql += vbCrLf + " UNION ALL"

        strSql += vbCrLf + " SELECT CASE WHEN (ISNULL(T.ISSDATE,'')<>'' AND TOFLAG='SA') THEN 'SALES' WHEN (ISNULL(T.ISSDATE,'')<>'' AND TOFLAG='MI') THEN 'MISC ISSUE' "
        strSql += vbCrLf + " WHEN (ISNULL(T.ISSDATE,'')<>'' AND TOFLAG='TR') THEN 'TRANSFER '+TC.COSTNAME END SEP,"
        strSql += vbCrLf + " T.RECDATE,T.ISSDATE AS TRANDATE, T.TAGNO, '' OLDTAGNO, I.ITEMCTRNAME,T.ITEMID "
        strSql += vbCrLf + " ,C.COSTNAME,CASE WHEN ISNULL(T.ISSDATE,'')='' THEN 2 WHEN TOFLAG IN ('SA','MI') THEN 1 ELSE 0 END ORDNO  , '' AS NARRATION, 2 RESULT"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..CTRANSFER T LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER I ON T.ITEMCTRID=I.ITEMCTRID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMTAG IT ON T.TAGSNO=IT.SNO AND T.COSTID=IT.COSTID AND T.ITEMCTRID=IT.ITEMCTRID LEFT JOIN " & cnAdminDb & "..COSTCENTRE TC ON IT.TCOSTID=TC.COSTID "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..COSTCENTRE C ON T.COSTID=C.COSTID WHERE T.TAGNO='" & tagNo & "'"
        strSql += vbCrLf + " UNION ALL"

        strSql += vbCrLf + " SELECT 'APPROVAL ISSUE TO-'+ ISNULL(P.PNAME,'') SEP,IT.RECDATE RECDATE,T.TRANDATE,T.TAGNO, '' OLDTAGNO, I.ITEMCTRNAME"
        strSql += vbCrLf + " ,T.ITEMID ,C.COSTNAME,0 ORDNO, '' AS NARRATION, 2 RESULT  FROM " & cnStockDb & "..ISSUE T"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMTAG IT ON T.TAGNO=IT.TAGNO LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER I ON T.ITEMCTRID=I.ITEMCTRID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..COSTCENTRE C ON T.COSTID=C.COSTID "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CUSTOMERINFO CI ON CI.BATCHNO=T.BATCHNO"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..PERSONALINFO P ON P.SNO=CI.PSNO"
        strSql += vbCrLf + " WHERE IT.OLDTAGNO='" & tagNo & "' AND TRANTYPE='AI' AND ISNULL(T.CANCEL,'')=''"

        strSql += vbCrLf + " UNION ALL"

        strSql += vbCrLf + " SELECT 'APPROVAL RECEIPT FROM-'+ ISNULL(P.PNAME,'') SEP,IT.RECDATE RECDATE,T.TRANDATE,T.TAGNO,'' OLDTAGNO, I.ITEMCTRNAME"
        strSql += vbCrLf + " ,T.ITEMID ,C.COSTNAME,0 ORDNO , '' AS NARRATION, 2 RESULT FROM " & cnStockDb & "..RECEIPT T"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMTAG IT ON T.TAGNO=IT.TAGNO LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER I ON T.ITEMCTRID=I.ITEMCTRID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..COSTCENTRE C ON T.COSTID=C.COSTID "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CUSTOMERINFO CI ON CI.BATCHNO=T.BATCHNO"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..PERSONALINFO P ON P.SNO=CI.PSNO"
        strSql += vbCrLf + " WHERE IT.OLDTAGNO='" & tagNo & "' AND TRANTYPE='AR' AND ISNULL(T.CANCEL,'')=''"

        strSql += vbCrLf + " UNION ALL"

        strSql += vbCrLf + " SELECT (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = IT.TCOSTID)SEP"
        strSql += vbCrLf + " ,IT.RECDATE,RECDATE TRANDATE,IT.TAGNO,'' OLDTAGNO, I.ITEMCTRNAME"
        strSql += vbCrLf + " ,IT.ITEMID ,C.COSTNAME,0 ORDNO , '' AS NARRATION, 2 RESULT "
        strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAG IT "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER I ON IT.ITEMCTRID=I.ITEMCTRID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..COSTCENTRE C ON IT.COSTID=C.COSTID  "
        strSql += vbCrLf + " WHERE IT.TAGNO='" & tagNo & "'"

        strSql += vbCrLf + " )X LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON X.ITEMID=IM.ITEMID "

        strSql += vbCrLf + " WHERE ISNULL(SEP,'')<>''  "
        strSql += vbCrLf + " ORDER BY RESULT, ORDNO,TRANDATE   "

        cmd = New OleDbCommand(strSql, cn)
        da = New OleDbDataAdapter(cmd)
        Dim DttagHis As New DataTable
        da.Fill(DttagHis)
        If Not DttagHis.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "TAG HISTORY"
        Dim tit As String = ""
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(DttagHis)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        BrighttechPack.FormatGridColumns(objGridShower.gridView)
        objGridShower.gridView.Columns("PARTICULAR").Width = 150
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = False
        objGridShower.formuser = userId
        objGridShower.ShowDialog()

    End Sub
    Private Sub btnDuplicate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDuplicate.Click
        Dim toflag As String = objGPack.GetSqlValue("SELECT TOFLAG FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID = '" & itemId & "' AND TAGNO='" & tagNo & "'")
        Dim issdate As String = objGPack.GetSqlValue("SELECT ISSDATE FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID = '" & itemId & "' AND TAGNO='" & tagNo & "'")
        If toflag = "" And issdate = "" Then
            Dim oldItem As Integer = Nothing
            Dim paramStr As String = ""
            Dim prnmemsuffix As String = ""
            If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
            Dim write As StreamWriter
            Dim memfile As String = "\Barcodeprint" & prnmemsuffix & ".mem"
            write = IO.File.CreateText(Application.StartupPath & memfile)
            If oldItem <> Val(itemId) Then
                write.WriteLine(LSet("PROC", 7) & ":" & itemId)
                paramStr += LSet("PROC", 7) & ":" & itemId & ";"
                oldItem = Val(itemId)
            End If
            write.WriteLine(LSet("TAGNO", 7) & ":" & tagNo)
            paramStr += LSet("TAGNO", 7) & ":" & tagNo & ";"
            If paramStr.EndsWith(";") Then
                paramStr = Mid(paramStr, 1, paramStr.Length - 1)
            End If
            write.Flush()
            write.Close()
            If EXE_WITH_PARAM = False Then
                If IO.File.Exists(Application.StartupPath & "\PrjBarcodePrint.exe") Then
                    System.Diagnostics.Process.Start(Application.StartupPath & "\PrjBarcodePrint.exe")
                Else
                    MsgBox("Barcode print exe not found", MsgBoxStyle.Information)
                End If
            Else
                If IO.File.Exists(Application.StartupPath & "\PrjBarcodePrint.exe") Then
                    System.Diagnostics.Process.Start(Application.StartupPath & "\PrjBarcodePrint.exe", paramStr)
                Else
                    MsgBox("Barcode print exe not found", MsgBoxStyle.Information)
                End If
            End If
        Else
            MsgBox("Cannot Allow Duplicate Print ", MsgBoxStyle.Information)
            Exit Sub
        End If
    End Sub

    Private Sub btnImagePrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImagePrint.Click
        Dim _Path As String = ""
        strSql = " DECLARE @DEFPATH VARCHAR(200)"
        strSql += vbCrLf + " SELECT @DEFPATH = '" & defaultPic & "'"
        strSql += vbCrLf + " SELECT CASE WHEN PCTFILE <> '' THEN @DEFPATH + PCTFILE END AS PCTFILE"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T "
        strSql += vbCrLf + "WHERE T.TAGNO = '" & tagNo & "' AND T.ITEMID = " & itemId & ""
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtimage As DataTable
        dtimage = New DataTable
        da.Fill(dtimage)
        'dt()
        If dtimage.Rows.Count > 0 Then
            With dtimage.Rows(0)
                If IO.File.Exists(.Item("PCTFILE").ToString) = False Then
                    bMap = My.Resources.no_photo
                Else
                    Dim Finfo As IO.FileInfo
                    Finfo = New IO.FileInfo(.Item("PCTFILE").ToString)
                    Dim fileStr As New IO.FileStream(.Item("PCTFILE").ToString, IO.FileMode.Open, IO.FileAccess.Read)
                    _Path = fileStr.Name.ToString
                    Try
                        'bMap = Bitmap.FromStream(fileStr)
                    Catch ex As Exception
                    Finally
                        fileStr.Close()
                    End Try
                End If
            End With
        End If

        Dim bmp As Bitmap
        Dim dirandfile As String = _Path
        'bmp.Save(dirandfile)

        Try
            Process.Start(dirandfile)
        Catch ex As Exception
            _Path = Application.StartupPath & "noImage.jpg"
            bmp = My.Resources.no_photo
            bmp.Save(_Path)
            Process.Start(_Path)
        End Try
        Exit Sub

    End Sub
#Region "Unused"
    'Sub ResizeImage(ByVal dir As String, ByVal fileName As String, ByVal percentResize As Double)
    '    'following code resizes picture to fit
    '    Dim bm As New Bitmap("C:\" & dir & "\" & fileName)
    '    Dim width As Integer = bm.Width - (bm.Width * percentResize) 'image width. 
    '    Dim height As Integer = bm.Height - (bm.Height * percentResize)  'image height
    '    Dim thumb As New Bitmap(width, height)
    '    Dim g As Graphics = Graphics.FromImage(thumb)
    '    g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
    '    g.DrawImage(bm, New Rectangle(0, 0, width, height), New Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel)
    '    g.Dispose()
    '    bm.Dispose()
    '    'image path.
    '    thumb.Save("C:\" & dir & "\" & fileName, System.Drawing.Imaging.ImageFormat.Jpeg) 'can use any image format 
    '    thumb.Dispose()
    'End Sub

    'Private Function AutoImageSizer2(ByVal bmp As Bitmap, ByVal picBox As PictureBox, Optional ByVal pSizeMode As PictureBoxSizeMode = PictureBoxSizeMode.CenterImage)
    '    Try
    '        picBox.Image = bMap.Clone
    '        AutoImageSizer2(picBox, pSizeMode)
    '    Catch ex As Exception
    '        picBox.Image = My.Resources.no_photo
    '    End Try
    'End Function

    'Private Function AutoImageSizer2(ByVal picBox As PictureBox, Optional ByVal pSizeMode As PictureBoxSizeMode = PictureBoxSizeMode.CenterImage)
    '    Try
    '        picBox.SizeMode = pSizeMode
    '        Dim imgOrg As Bitmap
    '        Dim imgShow As Bitmap
    '        Dim g As Graphics
    '        Dim divideBy, divideByH, divideByW As Double
    '        imgOrg = DirectCast(picBox.Image, Bitmap)

    '        divideByW = imgOrg.Width / picBox.Width
    '        divideByH = imgOrg.Height / picBox.Height
    '        'divideByW = imgOrg.Width / 2000
    '        'divideByH = imgOrg.Height / 3000
    '        If divideByW > 1 Or divideByH > 1 Then
    '            If divideByW > divideByH Then
    '                divideBy = divideByW
    '            Else
    '                divideBy = divideByH
    '            End If

    '            imgShow = New Bitmap(CInt(CDbl(imgOrg.Width) / divideBy), CInt(CDbl(imgOrg.Height) / divideBy))
    '            imgShow.SetResolution(imgOrg.HorizontalResolution, imgOrg.VerticalResolution)
    '            g = Graphics.FromImage(imgShow)
    '            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
    '            g.DrawImage(imgOrg, New Rectangle(0, 0, CInt(CDbl(imgOrg.Width) / divideBy), CInt(CDbl(imgOrg.Height) / divideBy)), 0, 0, imgOrg.Width, imgOrg.Height, GraphicsUnit.Pixel)
    '            g.Dispose()
    '        Else
    '            imgShow = New Bitmap(imgOrg.Width, imgOrg.Height)
    '            imgShow.SetResolution(imgOrg.HorizontalResolution, imgOrg.VerticalResolution)
    '            g = Graphics.FromImage(imgShow)
    '            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
    '            g.DrawImage(imgOrg, New Rectangle(0, 0, imgOrg.Width, imgOrg.Height), 0, 0, imgOrg.Width, imgOrg.Height, GraphicsUnit.Pixel)
    '            g.Dispose()
    '        End If
    '        imgOrg.Dispose()
    '        'picBox.Image = imgShow
    '        'Dim dtImg As New DataTable
    '        'Dim dtImgCol As New DataColumn()
    '        dtImg = New DataTable
    '        dtImgCol = New DataColumn("IMAGE", GetType(Image))
    '        dtImg.Columns.Add(dtImgCol)
    '        Dim ro As DataRow
    '        ro = dtImg.NewRow
    '        ro("IMAGE") = imgShow
    '        dtImg.Rows.Add(ro)
    '        'gridView.Columns(0).Width = IIf(imgShow.Width > 1500, 1500, imgShow.Width)
    '        'gridView.Rows(0).Height = IIf(imgShow.Height > 2000, 2000, imgShow.Height)
    '        Return dtImg
    '    Catch ex As Exception
    '        'picBox.Image = My.Resources.no_photo
    '        'Dim dtImg As New DataTable
    '        'Dim dtImgCol As New DataColumn()
    '        dtImg = New DataTable
    '        dtImgCol = New DataColumn("IMAGE", GetType(Image))
    '        dtImg.Columns.Add(dtImgCol)
    '        Dim ro As DataRow
    '        ro = dtImg.NewRow
    '        ro("IMAGE") = My.Resources.no_photo
    '        dtImg.Rows.Add(ro)
    '        Return dtImg
    '    End Try

    'End Function
#End Region
    Private Sub btnAttachImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAttachImage.Click
        Try
            If lblTagNo.Text.ToString.Trim = "" Then Exit Sub
            strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICSOURCEPATH'"
            defalutSourcePath = UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", , tran))
            Dim openDia As New OpenFileDialog
            Dim str As String
            If IO.File.Exists(defalutSourcePath) Then openDia.InitialDirectory = defalutSourcePath
            str = "JPEG(*.jpg)|*.jpg"
            str += "|Bitmap(*.bmp)|*.bmp"
            str += "|GIF(*.gif)|*.gif"
            str += "|All Files(*.*)|*.*"
            openDia.Filter = str
            If openDia.ShowDialog = Windows.Forms.DialogResult.OK Then
                'SubItemPic = False
                Dim Finfo As FileInfo
                Finfo = New FileInfo(openDia.FileName)
                AutoImageSizer(openDia.FileName, picTagImage)
                PICPATH = openDia.FileName
                picExtension = Finfo.Extension
                Me.SelectNextControl(btnAttachImage, True, True, True, True)
            Else
                Me.SelectNextControl(btnAttachImage, True, True, True, True)
            End If
            FuncUpdatePic()
        Catch ex As Exception
            MsgBox(E0016, MsgBoxStyle.Exclamation)
        End Try
    End Sub
    Private Function FuncUpdatePic()
        If lblTagNo.Text.ToString.Trim = "" Then Exit Function
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICPATH'"
        defaultDestination = UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", , tran))
        If Not defaultDestination.EndsWith("\") And defaultDestination <> Nothing Then defaultDestination += "\"
        Dim fileDestPath As String = ""
        Try
            If File.Exists(PICPATH) = True Then
                Dim strLotNo As String = "" : Dim strItemID As String = ""
                strSql = vbCrLf + " SELECT TOP 1 LOTNO FROM " & cnAdminDb & "..ITEMLOT WHERE SNO IN ("
                strSql += vbCrLf + " SELECT TOP 1 LOTSNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO='" & lblTagNo.Text.ToString.Replace(":", "") & "')"
                strLotNo = objGPack.GetSqlValue(strSql)
                strSql = " SELECT TOP 1 ITEMID FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO='" & lblTagNo.Text.ToString.Replace(":", "") & "'"
                strItemID = objGPack.GetSqlValue(strSql)
                If strItemID = "" Then Exit Function
                strSql = "UPDATE " & cnAdminDb & "..ITEMTAG SET "
                strSql += " PCTPATH ='" & defaultDestination & "'"
                strSql += " ,PCTFILE = '" & "L" + strLotNo + "I" & strItemID.ToString.Trim & "T" & lblTagNo.Text.ToString.Replace(":", "") + IIf(picExtension.Contains("."), picExtension, "." & picExtension) & "'"
                strSql += " WHERE ITEMID='" & strItemID.ToString.Trim & "' AND TAGNO='" & lblTagNo.Text.ToString.Replace(":", "") & "'"
                ExecQuery(SyncMode.Stock, strSql, cn)
                fileDestPath = defaultDestination + "L" + strLotNo + "I" + strItemID.ToString.Trim + "T" + lblTagNo.Text.ToString.Replace(":", "") + IIf(picExtension.Contains("."), picExtension, "." & picExtension)
                Dim serverPath As String = Nothing
                Dim Finfo As FileInfo
                Finfo = New FileInfo(fileDestPath)
                If IO.Directory.Exists(Finfo.Directory.FullName) = False Then
                    MsgBox(Finfo.Directory.FullName & " Does not exist. Please make a corresponding path", MsgBoxStyle.Information)
                    Exit Function
                End If
                If UCase(PICPATH) <> fileDestPath.ToUpper Then
                    Dim cFile As New FileInfo(PICPATH)
                    cFile.CopyTo(fileDestPath, True)
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Function
    Private Sub btnCam_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCam.Click
        If btnCam.Text = "Capture" Then
            If (Me.cmbxWebCam.SelectedIndex >= 0) Then
                Try
                    Dim c As Camera = DirectCast(Me.cmbxWebCam.SelectedItem, Camera)
                    AddHandler c.OnImageCaptured, New EventHandler(Of CameraEventArgs)(AddressOf OnImageCaptured)
                    Me._touchlessMgr.CurrentCamera = c
                    _dtFrameLast = DateTime.Now
                    AddHandler Me.picTagImage.Paint, New PaintEventHandler(AddressOf drawLatestImage)
                Catch exception As System.Exception
                    Dim ex As System.Exception = exception
                    Me.cmbxWebCam.Text = "Select A Camera"
                    MessageBox.Show(ex.Message)
                End Try
            Else
                MsgBox("WebCam Device Not Connect", MsgBoxStyle.Information)
                btnCam.Enabled = False
                Exit Sub
            End If
            btnCam.Text = "Save"
        ElseIf btnCam.Text = "Save" Then
            Try
                picExtension = "jpg"
                RemoveHandler Me._touchlessMgr.CurrentCamera.OnImageCaptured, New EventHandler(Of CameraEventArgs)(AddressOf OnImageCaptured)
                Me._touchlessMgr.CurrentCamera = Nothing
                RemoveHandler Me.picTagImage.Paint, New PaintEventHandler(AddressOf drawLatestImage)
                FuncUpdatePicWebCam()
                btnCam.Text = "Capture"
            Catch ex As Exception
                MsgBox("WebCam Not Connect ", MsgBoxStyle.Information)
                Exit Sub
            End Try
        End If
    End Sub
    Private Function FuncUpdatePicWebCam()
        If lblTagNo.Text.ToString.Trim = "" Then Exit Function
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICPATH'"
        defaultDestination = UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", , tran))
        Dim fileDestPath As String = ""
        If Not defaultDestination.EndsWith("\") And defaultDestination <> Nothing Then defaultDestination += "\"
        Try
            Dim strLotNo As String = "" : Dim strItemID As String = ""
            strSql = vbCrLf + " SELECT TOP 1 LOTNO FROM " & cnAdminDb & "..ITEMLOT WHERE SNO IN ("
            strSql += vbCrLf + " SELECT TOP 1 LOTSNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO='" & lblTagNo.Text.ToString.Replace(":", "") & "')"
            strLotNo = objGPack.GetSqlValue(strSql)
            strSql = " SELECT TOP 1 ITEMID FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO='" & lblTagNo.Text.ToString.Replace(":", "") & "'"
            strItemID = objGPack.GetSqlValue(strSql)
            If strItemID = "" Then Exit Function
            strSql = "UPDATE " & cnAdminDb & "..ITEMTAG SET "
            strSql += " PCTPATH ='" & defaultDestination & "'"
            strSql += " ,PCTFILE = '" & "L" + strLotNo + "I" & strItemID.ToString.Trim & "T" & lblTagNo.Text.ToString.Replace(":", "") & "." + picExtension & "'"
            strSql += " WHERE ITEMID='" & strItemID.ToString.Trim & "' AND TAGNO='" & lblTagNo.Text.ToString.Replace(":", "") & "'"
            ExecQuery(SyncMode.Stock, strSql, cn)
            If (System.IO.Directory.Exists(defaultDestination)) Then
                fileDestPath = defaultDestination + "L" + strLotNo + "I" + strItemID.ToString.Trim + "T" + lblTagNo.Text.ToString.Replace(":", "") + "." + picExtension
                _latestFrame.Save(fileDestPath, System.Drawing.Imaging.ImageFormat.Bmp)
            Else
                MsgBox("Path Not Found", MsgBoxStyle.Information)
                Exit Function
            End If
        Catch ex As Exception
            If ex.Message.Contains("generic error occurred in GDI+") Then
                MsgBox("Check Directory Permission.", MsgBoxStyle.Information)
            Else
                MsgBox(ex.Message + vbCrLf + ex.StackTrace, MsgBoxStyle.Information)
            End If
        End Try
    End Function
#Region "WebCam Check Connection"
    Private Sub Webcam_Connection_Check()
        For Each cam As Camera In _touchlessMgr.Cameras
            cmbxWebCam.Items.Add(cam)
        Next
        If (cmbxWebCam.Items.Count <= 0) Then
            'MessageBox.Show(" Webcam Device Not Connect ")
            'Close App
            'Environment.[Exit](0)
        Else
            cmbxWebCam.SelectedIndex = 0
        End If
    End Sub
#End Region
#Region "WebCam Function"
    Public Sub OnImageCaptured(ByVal sender As Object, ByVal args As CameraEventArgs)
        _nFrameCount = _nFrameCount + 1
        Dim now As DateTime = DateTime.Now
        Dim milliseconds As Double = CDbl(((now.Ticks - _dtFrameLast.Ticks) / CLng(10000)))
        If (milliseconds >= 1000) Then
            Dim action As Action(Of Double) = New Action(Of Double)(AddressOf UpdateFPSInUI)
            Dim objArray() As Object = {CDbl(_nFrameCount) * 1000 / milliseconds}
            MyBase.BeginInvoke(action, objArray)
            _nFrameCount = 0
            _dtFrameLast = DateTime.Now
        End If
        If (Not _fAddingMarker) Then
            _latestFrame = args.Image
            picTagImage.Invalidate()
        End If
    End Sub
    Private Sub UpdateFPSInUI(ByVal fps As Double)
        Dim labelCameraFPSValue As String
        labelCameraFPSValue = String.Concat("", Math.Round(fps, 2))
    End Sub
    Private Sub drawLatestImage(ByVal sender As Object, ByVal e As PaintEventArgs)
        If (_latestFrame IsNot Nothing) Then
            e.Graphics.DrawImage(_latestFrame, 0, 0, Me.picTagImage.Width, Me.picTagImage.Height)
            If (_drawSelectionAdornment) Then
                Dim pen As System.Drawing.Pen = New System.Drawing.Pen(Brushes.Red, 1.0!)
                e.Graphics.DrawEllipse(pen, CSng(_markerCenter.X) - _markerRadius, CSng(_markerCenter.Y) - _markerRadius, 2.0! * _markerRadius, 2.0! * _markerRadius)
            End If
        End If
    End Sub
#End Region
#Region "PDF CREATE VARIABLE"
    Dim defaultPDFdesitnationPath As String
    Dim CheckPath As String
#End Region
#Region "Export Pdf Creator"
    Private Sub btnPdfCreator_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPdfCreator.Click
        Using ms As New MemoryStream
            Dim doc As New iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 10, 10, 50, 50)
            Dim contentFont As iTextSharp.text.Font = iTextSharp.text.FontFactory.GetFont("Verdana", 9, iTextSharp.text.Font.BOLD)
            Dim writer As iTextSharp.text.pdf.PdfWriter
            strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PDF_PICPATH'"
            defaultPDFdesitnationPath = UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", , tran))
            If Not defaultPDFdesitnationPath.EndsWith("\") And defaultPDFdesitnationPath <> Nothing Then defaultPDFdesitnationPath += "\"
            Try
                If (System.IO.Directory.Exists(defaultPDFdesitnationPath)) Then
                    CheckPath = defaultPDFdesitnationPath + "T" + lblTagNo.Text + ".pdf"
                    'Dim fs As New FileStream(CheckPath, FileMode.Create, FileAccess.Write, FileShare.None)
                    'If File.Exists(CheckPath) = True Then
                    '    File.Delete(CheckPath)
                    'End If
                    'Dim fs As New IO.FileStream(CheckPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read)
                    strSql = " DECLARE @DEFPATH VARCHAR(200)"
                    strSql += vbCrLf + " SELECT @DEFPATH = '" & defaultPic & "'"
                    strSql += vbCrLf + " SELECT CASE WHEN PCTFILE <> '' THEN @DEFPATH + PCTFILE END AS PCTFILE"
                    strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T "
                    strSql += vbCrLf + "WHERE T.TAGNO = '" & tagNo & "' AND T.ITEMID = " & itemId & ""
                    Dim ItextSharpImage As String = UCase(objGPack.GetSqlValue(strSql))
                    If ItextSharpImage.Contains("..jpg") Then
                        ItextSharpImage = ItextSharpImage.Replace("..", ".")
                    End If
                    Try
                        'writer.GetInstance(doc, fs)
                        writer.GetInstance(doc, ms)
                        Try
                            Try
                                doc.Open()
                                If ItextSharpImage <> "" Then
                                    Dim jpg As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(ItextSharpImage)
                                    jpg.ScaleToFit(140.0F, 140.0F)
                                    jpg.SpacingBefore = 10.0F
                                    jpg.SpacingAfter = 50.0F
                                    jpg.Alignment = 4
                                    doc.Add(jpg)
                                Else
                                    Dim bMap1 As Byte() = ImageToByte(My.Resources.no_photo)
                                    Dim jpg As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(bMap1)
                                    jpg.ScaleToFit(140.0F, 140.0F)
                                    jpg.SpacingBefore = 10.0F
                                    jpg.SpacingAfter = 50.0F
                                    jpg.Alignment = 4
                                    doc.Add(jpg)
                                End If

                                'If lblTagNo.Text <> "" Then
                                '    doc.Add(New iTextSharp.text.Paragraph("TAGNO            :" + "    " + lblTagNo.Text, contentFont))
                                'End If
                                'If lblGrsWt.Text <> "" Then
                                '    doc.Add(New iTextSharp.text.Paragraph("GRSWT            :" + "    " + lblGrsWt.Text, contentFont))
                                'End If
                                'If lblDiaPcs.Text <> "" And lblDiaWt.Text <> "" Then
                                '    doc.Add(New iTextSharp.text.Paragraph("DIAWT             :" + "     " + lblDiaPcs.Text + " - " + lblDiaWt.Text, contentFont))
                                'End If
                                'If lblStnPcs.Text <> "" And lblStnWt.Text <> "" Then
                                '    doc.Add(New iTextSharp.text.Paragraph("STNWT             :" + "     " + lblStnPcs.Text + " - " + lblStnWt.Text, contentFont))
                                'End If
                                'If lblItemType.Text <> "" Then
                                '    doc.Add(New iTextSharp.text.Paragraph("ITEMTYPE         :" + "    " + lblItemType.Text, contentFont))
                                'End If

                                If lblTagNo.Text <> "" Then
                                    doc.Add(New iTextSharp.text.Paragraph("TAGNO" + ChrW(9) + ":" + ChrW(9) + lblTagNo.Text, contentFont))
                                End If
                                If lblGrsWt.Text <> "" Then
                                    doc.Add(New iTextSharp.text.Paragraph("GRSWT" + ChrW(9) + ":" + ChrW(9) + lblGrsWt.Text, contentFont))
                                End If
                                If lblDiaPcs.Text <> "" And lblDiaWt.Text <> "" Then
                                    doc.Add(New iTextSharp.text.Paragraph("DIAWT" + ChrW(9) + " :" + ChrW(9) + lblDiaPcs.Text + " - " + lblDiaWt.Text, contentFont))
                                End If
                                If lblStnPcs.Text <> "" And lblStnWt.Text <> "" Then
                                    doc.Add(New iTextSharp.text.Paragraph("STNWT" + ChrW(9) + ":" + ChrW(9) + lblStnPcs.Text + " - " + lblStnWt.Text, contentFont))
                                End If
                                If lblItemType.Text <> "" Then
                                    doc.Add(New iTextSharp.text.Paragraph("ITEMTYPE" + ChrW(9) + ":" + ChrW(9) + lblItemType.Text, contentFont))
                                End If
                                doc.Close()
                                Dim content As Byte()
                                Dim contentLength As Integer
                                content = ms.ToArray()
                                contentLength = content.Length
                                Using fs As New FileStream(CheckPath, FileMode.OpenOrCreate, FileAccess.Write)
                                    fs.Write(content, 0, contentLength)
                                End Using
                                MsgBox("Export Completed.", MsgBoxStyle.Information)
                            Catch ex As Exception
                                MsgBox("" + ex.ToString, MsgBoxStyle.Information)
                                Exit Sub
                            Catch ex2 As DirectoryNotFoundException
                                MsgBox("Image Directory Not Found", MsgBoxStyle.Information)
                                Exit Sub
                            Catch ex1 As FileNotFoundException
                                MsgBox("Image Not Found", MsgBoxStyle.Information)
                                Exit Sub
                            Finally
                                'If (writer IsNot Nothing) Then
                                '    DirectCast(writer, IDisposable).Dispose()
                                'End If
                            End Try
                        Catch ex As Exception
                        Finally
                            'If (doc IsNot Nothing) Then
                            '    DirectCast(doc, IDisposable).Dispose()
                            'End If
                        End Try
                    Catch ex1 As Exception
                        MsgBox("" + ex1.ToString, MsgBoxStyle.Information)
                        Exit Sub
                    Catch ex2 As iTextSharp.text.DocumentException
                        MsgBox("" + ex2.ToString, MsgBoxStyle.Information)
                        Exit Sub
                    Catch ex3 As IOException
                        MsgBox("" + ex3.ToString, MsgBoxStyle.Information)
                        Exit Sub
                    Finally
                        'If (fs IsNot Nothing) Then
                        '    DirectCast(fs, IDisposable).Dispose()
                        'End If
                    End Try
                    Process.Start(CheckPath)
                    'fs.Dispose()
                    'fs.Close()
                Else
                    MsgBox("Pdf Path Not Found", MsgBoxStyle.Information)
                    Exit Sub
                End If
            Catch ex As Exception
                If ex.Message.Contains("generic error occurred in GDI+") Then
                    MsgBox("Check Directory Permission.", MsgBoxStyle.Information)
                Else
                    MsgBox(ex.Message + vbCrLf + ex.StackTrace, MsgBoxStyle.Information)
                End If
            Finally
                'Dim p As System.Diagnostics.Process
                'p.GetProcessesByName(CheckPath)
                'p.Kill()
            End Try
        End Using
    End Sub

#End Region
#Region "ImageFiletoBytes"
    Public Function ImageToByte(ByVal img As Image) As Byte()
        Dim imgStream As MemoryStream = New MemoryStream()
        img.Save(imgStream, System.Drawing.Imaging.ImageFormat.Jpeg)
        imgStream.Close()
        Dim byteArray As Byte() = imgStream.ToArray()
        imgStream.Dispose()
        Return byteArray
    End Function
#End Region
#Region "Export Word Process"
    Private Sub btnWord_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWord.Click
        Try
            Dim word_app As Word._Application = New Word.ApplicationClass()
            word_app.Visible = True
            Dim word_doc As Word._Document = word_app.Documents.Add()
            Dim para As Word.Paragraph = word_doc.Paragraphs.Add()

            If lblTagNo.Text <> "" Then
                para.Range.Text = ChrW(9) + "TAG NO" + ChrW(9) + ChrW(9) + "-" + ChrW(9) + lblTagNo.Text
                para.Range.InsertParagraphAfter()
            End If
            If lblGrsWt.Text <> "" Then
                para.Range.Text = ChrW(9) + "GRS WT" + ChrW(9) + ChrW(9) + "-" + ChrW(9) + lblGrsWt.Text
                para.Range.InsertParagraphAfter()
            End If
            If lblDiaPcs.Text <> "" And lblDiaWt.Text <> "" Then
                para.Range.Text = ChrW(9) + "DIA WT" + ChrW(9) + "-" + ChrW(9) + ChrW(9) + lblDiaPcs.Text + "D" + "/" + Format(Val(lblDiaWt.Text), "0.00") + "ct"
                para.Range.InsertParagraphAfter()
            End If
            If lblStnPcs.Text <> "" And lblStnWt.Text <> "" Then
                para.Range.Text = ChrW(9) + "SPW.WT" + ChrW(9) + "-" + ChrW(9) + ChrW(9) + lblStnPcs.Text + "D" + "/" + lblStnWt.Text + "ct"
                para.Range.InsertParagraphAfter()
            End If
            If lblItemType.Text <> "" Then
                para.Range.Text = ChrW(9) + "TAG.TYPE" + ChrW(9) + "-" + ChrW(9) + lblItemType.Text
                para.Range.InsertParagraphAfter()
            End If
            If lblItemType.Text = "C/S" Then
                Dim Temp_Val As Double = Val(gridStoneView.Rows(0).Cells("SAMOUNT").Value.ToString) + (Val(lblGrsWt.Text.ToString) * 500)
                Dim Temp_Val_Format As Double
                Dim t1 As String = Format(Val(gridStoneView.Rows(0).Cells("SAMOUNT").Value.ToString), "0.00")
                Dim T2 As String = Format((Val(lblGrsWt.Text.ToString) * 500), "0.00")
                'Temp_Val_Format = t1 + T2
                para.Range.Text = ChrW(9) + "APPROX.VALUE" + ChrW(9) + "-" + ChrW(9) + Temp_Val.ToString()
                para.Range.InsertParagraphAfter()
            ElseIf lblItemType.Text = "O/S" Then
                Dim Temp As String = gridStoneView.Rows(0).Cells("SAMOUNT").Value.ToString
                para.Range.Text = ChrW(9) + "APPROX.VALUE" + ChrW(9) + "-" + ChrW(9) + Temp.ToString
                para.Range.InsertParagraphAfter()
            End If
            Dim start_of_doc As Object = "\startofdoc"
            Dim start_range As Word.Range = word_doc.Bookmarks.Item(start_of_doc).Range
            strSql = " DECLARE @DEFPATH VARCHAR(200)"
            strSql += vbCrLf + " SELECT @DEFPATH = '" & defaultPic & "'"
            strSql += vbCrLf + " SELECT CASE WHEN PCTFILE <> '' THEN @DEFPATH + PCTFILE END AS PCTFILE"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T "
            strSql += vbCrLf + "WHERE T.TAGNO = '" & tagNo & "' AND T.ITEMID = " & itemId & ""
            Dim picture_file As String = UCase(objGPack.GetSqlValue(strSql))
            Dim inline_shape As Word.InlineShape
            'Dim inline_shape As Word.InlineShape = start_range.InlineShapes.AddPicture(picture_file)
            If picture_file <> "" Then
                inline_shape = start_range.InlineShapes.AddPicture(picture_file)
                'Else
                'inline_shape = start_range.InlineShapes.AddPicture(Application.StartupPath + "\noimage.jpg")
            End If
            Dim shape As Word.Shape = inline_shape.ConvertToShape()
            '''''''shape.LockAspectRatio = -1
            '''''''shape.ScaleHeight(1.5F, -1, 0)
            shape.Width = 150
            shape.Height = 150
            shape.WrapFormat.Type = Word.WdWrapType.wdWrapSquare
            shape.Top = CSng(Word.WdShapePosition.wdShapeTop)
            shape.Left = CSng(Word.WdShapePosition.wdShapeLeft)
            'word_doc.Close()
            'word_app.Quit()
        Catch ex As Exception
        End Try
    End Sub
#End Region

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        If dtImage.Rows.Count > 1 Then
            If i = dtImage.Rows.Count - 1 Then Exit Sub
            i = i + 1
            tempivalue = i
            tagdetails(dtImage.Rows(i).Item("TAGNO").ToString, dtImage.Rows(i).Item("ITEMID").ToString)
        End If

        ' Call New frmTagImageViewer(dtt.Rows(i).Item("TAGNO").ToString, dtt.Rows(i).Item("ITEMID").ToString).ShowDialog()
        'Me.Close()
        'Dim dtt As DataTable = frmTagImage.dtimage
        'Call New frmTagImageViewer_Format2(dtt.Rows(i).Item("TAGNO").ToString, dtt.Rows(i).Item("ITEMID").ToString).ShowDialog()
        'i = i + 1
        'tagdetails(dtt.Rows(i).Item("TAGNO").ToString, dtt.Rows(i).Item("ITEMID").ToString)
        'Dim obj_image As New frmTagImageViewer_Format2(dtt.Rows(i).Item("TAGNO").ToString, dtt.Rows(i).Item("ITEMID").ToString)
        'obj_image.ShowDialog()
        'Dim objTagViewer As New frmTagImageViewer_Format2(dtt.Rows(i).Item("TAGNO").ToString, dtt.Rows(i).Item("ITEMID").ToString, , , dtImagePath)
        'objTagViewer.ShowDialog()
        'Exit Sub
    End Sub
    Function tagdetails(ByVal tagNo As String, ByVal itemId As Integer, Optional ByVal PurchaseDetail As Boolean = False, Optional ByVal Tag As Boolean = True)
        If Not defaultPic.EndsWith("\") Then defaultPic += "\"
        Me.tagNo = tagNo
        Me.itemId = itemId

        Dim dtStone As New DataTable
        Dim dtMisc As New DataTable
        Dim dtMultiMetal As New DataTable
        Dim Rate As Decimal = Nothing
        Dim itemTypeId As Integer = Nothing
        Dim CheckDAte As Date = GetEntryDate(GetServerDate)
        itemTypeId = Val(objGPack.GetSqlValue("SELECT ITEMTYPEID FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAG", "CITEMTAG") & " WHERE TAGNO = '" & tagNo & "' AND ITEMID = " & itemId & ""))
        If itemTypeId <> 0 Then
            Dim PURITYID As String = objGPack.GetSqlValue("SELECT PURITYID FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = " & itemTypeId & " AND RATEGET = 'Y'", , )
            If PURITYID <> "" Then Rate = Val(GetRate_Purity(CheckDAte, PURITYID)) Else GoTo weight_rate
        Else
weight_rate:
            Rate = Val(GetRate(CheckDAte, objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & itemId & "")))
        End If
        strSql = " DECLARE @DEFPATH VARCHAR(200)"
        strSql += vbCrLf + " SELECT @DEFPATH = '" & defaultPic & "'"
        strSql += vbCrLf + " SELECT CASE WHEN PCTFILE <> '' THEN @DEFPATH + PCTFILE END AS PCTFILE,T.TAGNO,T.PCS,T.GRSWT,T.LESSWT,T.NETWT,T.SALVALUE "
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAGSTONE", "CITEMTAGSTONE") & " WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)AS DIAPCS"
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAGSTONE", "CITEMTAGSTONE") & " WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)AS DIAWT"
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAGSTONE", "CITEMTAGSTONE") & " WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')),0)AS STNPCS"
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAGSTONE", "CITEMTAGSTONE") & " WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S') ),0)AS STNWT"
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAGSTONE", "CITEMTAGSTONE") & " WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')),0)AS PREPCS"
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAGSTONE", "CITEMTAGSTONE") & " WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P') ),0)AS PREWT"
        strSql += vbCrLf + " ,CONVERT(VARCHAR,RECDATE,103)RECDATE"
        strSql += vbCrLf + " ,CONVERT(VARCHAR,CHKDATE,103)CHKDATE"
        strSql += vbCrLf + " ,CONVERT(VARCHAR,ISSDATE,103)ISSDATE,isnull(ISSREFNO,'') ISSREFNO"
        strSql += vbCrLf + " ,CASE WHEN ISNULL(CONVERT(VARCHAR,ISSDATE,103),'') = '' THEN DATEDIFF(DD,RECDATE,GETDATE()) ELSE DATEDIFF(DD,RECDATE,ISSDATE) END AGING"
        strSql += vbCrLf + " ,MAXWASTPER,MAXWAST,MAXMCGRM,MAXMC,ISNULL(NARRATION,'') NARRATION,ISNULL(STYLENO,'') STYLENO,ISNULL(ORDREPNO,'') ORDREPNO,T.ITEMID,T.SUBITEMID"
        strSql += vbCrLf + " ,(SELECT top 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID) + ' ['+ CONVERT(VARCHAR,T.ITEMID) + ']' AS ITEM"
        strSql += vbCrLf + " ,CASE WHEN T.SUBITEMID > 0 THEN (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = T.ITEMID AND SUBITEMID = T.SUBITEMID) + ' ['+ CONVERT(VARCHAR,T.SUBITEMID) + ']' ELSE '' END AS SUBITEM"
        strSql += vbCrLf + " ,(SELECT top 1 ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID)AS COUNTER"
        strSql += vbCrLf + " ,(SELECT DESIGNERNAME+ (CASE WHEN ISNULL(SEAL,'')<>'' THEN '('+SEAL+')' ELSE '' END) FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)AS DESIGNER"
        strSql += vbCrLf + " ,(SELECT top 1 NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = T.ITEMTYPEID)AS ITEMTYPE"
        strSql += vbCrLf + " ,(SELECT top 1 USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = T.USERID)AS USERNAME"
        strSql += vbCrLf + " ,(SELECT top 1 USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = R.USERID)AS SCANERNAME "
        strSql += vbCrLf + " ,(SELECT top 1 EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = R.EMPID)AS EMPNAME "
        strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = T.COSTID)AS COSTNAME,CONVERT(VARCHAR,ACTUALRECDATE,103)ACTUALRECDATE,APPROVAL"
        strSql += vbCrLf + " ,TOFLAG,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = T.TCOSTID)AS TCOSTNAME"
        strSql += vbCrLf + " ,(SELECT TOP 1 SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZEID = T.SIZEID AND ITEMID=T.ITEMID)AS SIZE"
        strSql += vbCrLf + " ,(SELECT TOP 1 DESCRIPT FROM " & cnAdminDb & "..PURITEMTAG WHERE TAGNO = T.TAGNO)DESCRIPT"
        strSql += vbCrLf + " ,(SELECT TOP 1 PURWASTAGE FROM " & cnAdminDb & "..PURITEMTAG WHERE TAGNO = T.TAGNO)PURWASTAGE"
        strSql += vbCrLf + " ,(SELECT TOP 1 PURTOUCH FROM " & cnAdminDb & "..PURITEMTAG WHERE TAGNO = T.TAGNO)PURTOUCH"
        strSql += vbCrLf + " ,(SELECT TOP 1 LOTNO FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = T.LOTSNO)LOTNO"
        strSql += vbCrLf + " ,(SELECT TOP 1 TRANNO FROM " & cnStockDb & "..RECEIPT WHERE SNO IN (SELECT TOP 1 RECSNO FROM " & cnStockDb & "..LOTISSUE WHERE LOTSNO = T.LOTSNO))RECEIPTNO"
        strSql += vbCrLf + " ,T.GRSNET"
        strSql += vbCrLf + " ,CONVERT(VARCHAR,RESDATE,103)DUPLDATE,ISNULL(TRANINVNO,'')TRANINVNO"
        strSql += vbCrLf + " FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAG", "CITEMTAG") & " AS T "
        strSql += vbCrLf + " LEFT JOIN " & cnStockDb & "..ISSUE AS R ON T.TAGNO=R.TAGNO"
        strSql += vbCrLf + "WHERE T.TAGNO = '" & tagNo & "' AND T.ITEMID = " & itemId & ""
        If Not cnCentStock Then strSql += vbCrLf + " AND T.COMPANYID = '" & GetStockCompId() & "'"
        strSql += vbCrLf + " ORDER BY R.TRANTYPE DESC"
        da = New OleDbDataAdapter(strSql, cn)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            With dt.Rows(0)
                If IO.File.Exists(.Item("PCTFILE").ToString) = False Then
                    bMap = My.Resources.no_photo
                Else
                    Dim Finfo As IO.FileInfo
                    Finfo = New IO.FileInfo(.Item("PCTFILE").ToString)
                    'Finfo.IsReadOnly = False
                    Dim fileStr As New IO.FileStream(.Item("PCTFILE").ToString, IO.FileMode.Open, IO.FileAccess.Read)
                    Try
                        bMap = Bitmap.FromStream(fileStr)
                    Catch ex As Exception
                    Finally
                        fileStr.Close()
                    End Try
                End If
                AutoImageSizer(bMap, picTagImage, PictureBoxSizeMode.CenterImage)



                tBarImageZoomer.Minimum = picTagImage.Width
                tBarImageZoomer.Maximum = 1000
                'SetLocation()
                Me.Text = .Item("ITEM").ToString & " [" & .Item("TAGNO").ToString & "]"
                lblItem.Text = .Item("ITEM").ToString
                lblTagNo.Text = .Item("TAGNO").ToString
                lblSubItem.Text = .Item("SUBITEM").ToString
                If Not Val(.Item("PCS").ToString) = 0 Then
                    lblPcs.Text = .Item("PCS").ToString
                End If
                If Not Val(Val(.Item("GRSWT").ToString)) = 0 Then
                    lblGrsWt.Text = Format(Val(.Item("GRSWT").ToString), "0.000")
                End If
                If Not (Val(.Item("LESSWT").ToString)) Then
                    lblLessWt.Text = Format(Val(.Item("LESSWT").ToString), "0.000")
                End If
                If Not Val(Val(.Item("NETWT").ToString)) Then
                    lblNetWt.Text = Format(Val(.Item("NETWT").ToString), "0.000")
                End If
                If Not (Val(.Item("SALVALUE").ToString)) Then
                    lblSaleValue.Text = Format(Val(.Item("SALVALUE").ToString), "0.00")
                End If
                If Not (Val(.Item("DIAPCS").ToString)) = 0 Then
                    lblDiaPcs.Text = .Item("DIAPCS").ToString
                End If
                If Not (Val(.Item("DIAWT").ToString)) = 0 Then
                    lblDiaWt.Text = .Item("DIAWT").ToString
                End If
                If Not (Val(.Item("STNPCS").ToString)) = 0 Then
                    lblStnPcs.Text = .Item("STNPCS").ToString
                End If
                If Not (Val(.Item("STNWT").ToString)) = 0 Then
                    lblStnWt.Text = .Item("STNWT").ToString
                End If
                If Not (Val(.Item("PREPCS").ToString)) = 0 Then
                    lblPrePcs.Text = .Item("PREPCS").ToString
                End If
                If Not (Val(.Item("PREWT").ToString)) = 0 Then
                    lblPreWt.Text = .Item("PREWT").ToString
                End If
                If Not (Val(.Item("MAXWASTPER").ToString)) = 0 Then
                    lblWastagePer.Text = .Item("MAXWASTPER").ToString
                End If
                If Not (Val(.Item("MAXWAST").ToString)) = 0 Then
                    lblWastage.Text = .Item("MAXWAST").ToString
                End If
                If Not (Val(.Item("MAXMCGRM").ToString)) = 0 Then
                    lblMcGrm.Text = .Item("MAXMCGRM").ToString
                End If
                If Not _MCONWTAMT And mcasvaper(Val(.Item("ITEMID").ToString), Val(.Item("SUBITEMID").ToString)) = True Then
                    Dim wast As Double = IIf(_McWithWastage, Val(lblWastage.Text), 0)
                    Dim mc As Double = 0
                    If _MCONGRSNET Then
                        mc = IIf(.Item("GRSNET").ToString = "G", (Val(lblGrsWt.Text) + wast) * Rate, _
                        (Val(lblNetWt.Text) + wast) * Rate)
                    Else
                        mc = IIf(_MCCALCON_ITEM_GRS, (Val(lblGrsWt.Text) + wast) * Rate, _
            (Val(lblNetWt.Text) + wast) * Rate)
                    End If
                    mc = (mc * Val(lblMcGrm.Text)) / 100
                    lblMc.Text = mc.ToString
                Else

                    If Not (Val(.Item("MAXMC").ToString)) = 0 Then
                        lblMc.Text = .Item("MAXMC").ToString
                    End If

                End If
                If .Item("APPROVAL").ToString = "A" Then
                    lblapprovalissue.Text = "Approval Issue"
                    lblapprovalissue.Visible = True
                Else
                    lblapprovalissue.Visible = False
                End If
                '584
                If .Item("ISSDATE").ToString <> "" And .Item("TOFLAG").ToString = "SA" Then
                    lblapprovalissue.Text = "Sales"
                    lblapprovalissue.Visible = True
                ElseIf .Item("ISSDATE").ToString <> "" And .Item("TOFLAG").ToString = "MI" Then
                    lblapprovalissue.Text = "Misc Issue"
                    lblapprovalissue.Visible = True
                ElseIf .Item("TOFLAG").ToString = "TR" Then
                    lblapprovalissue.Text = "Transfered To " & .Item("TCOSTNAME").ToString
                    lblapprovalissue.Visible = True
                Else
                    lblapprovalissue.Visible = False
                End If
                If .Item("APPROVAL").ToString = "R" Then
                    lblapprovalissue.Text = "Booked"
                    lblapprovalissue.Visible = True
                End If
                '584
                lblPath.Text = "FileName : " + .Item("PCTFILE").ToString '' + "\" + .Item("PCTFILE").ToString
                Dim recDate As String
                Dim actRecDate As String
                actRecDate = .Item("ACTUALRECDATE").ToString
                recDate = .Item("RECDATE").ToString
                If actRecDate <> recDate Then
                    lblRecDate.Text = recDate + "(" + actRecDate + ")"
                Else
                    lblRecDate.Text = recDate
                End If
                lblTrfNo.Text = .Item("TRANINVNO").ToString
                lblRecNo.Text = .Item("RECEIPTNO").ToString
                lblDupdate.Text = .Item("DUPLDATE").ToString
                lblIssDate.Text = .Item("ISSDATE").ToString
                lblLotNo.Text = .Item("LOTNO").ToString
                lblIssueNo.Text = IIf(.Item("ISSREFNO").ToString <> 0, .Item("ISSREFNO").ToString, "")
                lblAging.Text = .Item("AGING").ToString
                lblStyleno.Text = .Item("STYLENO").ToString
                lblTagDescript.Text = .Item("NARRATION").ToString
                lblChkDate.Text = .Item("CHKDATE").ToString
                lblDesigner.Text = .Item("DESIGNER").ToString
                lblCounter.Text = .Item("COUNTER").ToString
                lblItemType.Text = .Item("ITEMTYPE").ToString
                lblCostCentre.Text = .Item("COSTNAME").ToString
                lblOrderRepNo.Text = .Item("ORDREPNO").ToString
                Label24.Text = .Item("USERNAME").ToString
                lblscanname.Text = .Item("SCANERNAME").ToString
                lblEmpName.Text = .Item("EMPNAME").ToString
                lblDescript.Text = .Item("DESCRIPT").ToString
                lblPtouch.Text = .Item("PURTOUCH").ToString
                lblPwast.Text = .Item("PURWASTAGE").ToString
                lblSize.Text = .Item("SIZE").ToString
            End With
        End If

        If GetAdmindbSoftValue("TAGCHKPURRATE", "N") = "Y" Then
            strSql = "SELECT PURRATE FROM " & cnAdminDb & "..PURITEMTAG AS T WHERE T.TAGNO ='" & tagNo & "' AND T.ITEMID = " & itemId & ""
            Rate = Val(objGPack.GetSqlValue(strSql))
            If Rate = 0 Then
                strSql = "SELECT BOARDRATE FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAG", "CITEMTAG") & " AS T WHERE T.TAGNO ='" & tagNo & "' AND T.ITEMID = " & itemId & ""
                Rate = Val(objGPack.GetSqlValue(strSql))
            End If
        End If

        strSql = vbCrLf + "		IF (SELECT 1 FROM SYSOBJECTS WHERE NAME='TEMPRESULT') > 0 DROP TABLE TEMPRESULT"
        strSql += vbCrLf + "		SELECT DISTINCT T.ITEMID,CASE WHEN T.GRSNET ='N' THEN 'NET WEIGHT'  WHEN T.GRSNET ='G' THEN 'GROSS WEIGHT' END AS PARTICULAR,"
        strSql += vbCrLf + "		T.PCS,CASE WHEN T.GRSNET ='N' THEN T.NETWT  WHEN T.GRSNET ='G' THEN T.GRSWT END AS WEIGHT," & Rate & " AS RATE , 0 AS AMOUNT,T.MAXWAST,T.MAXMC,P.PURRATE,"
        strSql += vbCrLf + "		CASE WHEN P.PURGRSNET = 'N' THEN 'NET WEIGHT' WHEN P.PURGRSNET = 'G' THEN 'GROSS WEIGHT' END AS P_PARTICULAR,CASE WHEN P.PURGRSNET ='N' THEN P.PURNETWT  WHEN P.PURGRSNET ='G' THEN T.GRSWT END AS PWEIGHT,P.PURWASTAGE,P.PURMC,P.PURTOUCH,' 'COLHEAD"
        strSql += vbCrLf + "		,SALEMODE,SALVALUE INTO TEMPRESULT FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAG", "CITEMTAG") & " AS T LEFT OUTER JOIN " & cnAdminDb & "..PURITEMTAG AS P ON P.TAGSNO = T.SNO"
        strSql += vbCrLf + "        WHERE T.TAGNO ='" & tagNo & "' AND T.ITEMID = " & itemId & ""
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = vbCrLf + " ALTER TABLE TEMPRESULT ADD EMPTY VARCHAR(250)"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPSALEVALUE')>0 DROP TABLE TEMPSALEVALUE"
        strSql += vbCrLf + " SELECT SVALUE,STAX,SVALUE+STAX AS SNETVALUE"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),PURVALUE-ISNULL(PURTAX,0)) PVALUE,PURTAX PTAX,PURVALUE PNETVALUE,CONVERT(NUMERIC(15,3),(PURVALUE*PURLCOSTPER)/100) AS PURLCOSTPER,PURLCOST"
        strSql += vbCrLf + " INTO TEMPSALEVALUE"
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " 	SELECT SUM(SALVALUE)SVALUE,SUM(SALVALUE)*(CONVERT(NUMERIC(18,2),SALESTAX)/100) AS STAX"
        strSql += vbCrLf + " 	FROM"
        strSql += vbCrLf + " 	("
        strSql += vbCrLf + "    SELECT T.ITEMID"
        strSql += vbCrLf + "  	, CASE WHEN T.SALEMODE NOT IN ('F','R') THEN ((CASE WHEN T.GRSNET = 'G' THEN T.GRSWT ELSE T.NETWT END + MAXWAST)"
        strSql += vbCrLf + "  	* " & Rate & ")  + MAXMC ELSE SALVALUE END AS SALVALUE,C.SALESTAX"
        'strSql += vbCrLf + " 	SELECT T.ITEMID,T.SALVALUE - ISNULL((SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO),0) AS SALVALUE,C.SALESTAX"
        strSql += vbCrLf + " 	FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAG", "CITEMTAG") & " AS T "
        strSql += vbCrLf + " 	INNER JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID = T.ITEMID"
        strSql += vbCrLf + " 	INNER JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
        strSql += vbCrLf + " 	WHERE T.TAGNO = '" & tagNo & "' AND T.ITEMID = " & itemId & ""
        strSql += vbCrLf + " 	UNION ALL "
        strSql += vbCrLf + " 	SELECT T.STNITEMID,T.STNAMT,C.SALESTAX"
        strSql += vbCrLf + " 	FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAGSTONE", "CITEMTAGSTONE") & " AS T "
        strSql += vbCrLf + " 	INNER JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID = T.ITEMID"
        strSql += vbCrLf + " 	INNER JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
        strSql += vbCrLf + " 	WHERE T.TAGNO = '" & tagNo & "' AND T.ITEMID = " & itemId & ""
        strSql += vbCrLf + " 	)X GROUP BY SALESTAX"
        strSql += vbCrLf + " )Y"
        strSql += vbCrLf + " FULL JOIN (SELECT * FROM " & cnAdminDb & "..PURITEMTAG WHERE TAGNO = '" & tagNo & "' AND ITEMID = " & itemId & ")AS P ON P.TAGNO = '" & tagNo & "' AND P.ITEMID = " & itemId & ""
        'strSql += vbCrLf + " GROUP BY P.PURVALUE,P.PURTAX"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        Dim salValue As Decimal = Val(objGPack.GetSqlValue("SELECT SVALUE FROM TEMPSALEVALUE"))
        If salValue <> 0 Then
            lblSaleValue.Text = Format(salValue, "0.00")
        End If
        strSql = vbCrLf + " SET CONCAT_NULL_YIELDS_NULL OFF"
        strSql += vbCrLf + " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPTAGSTONE') > 0 DROP TABLE TEMPTAGSTONE"
        strSql += vbCrLf + " SELECT DISTINCT CASE WHEN ISNULL(SM.SUBITEMNAME,'') = '' THEN IM.ITEMNAME "
        If _FourCMaintain Then
            strSql += vbCrLf + " + ' ' + SC.CUTNAME + ' ' + SR.COLORNAME + ' ' + ST.CLARITYNAME + ' ' + SP.SHAPENAME"
        End If
        strSql += vbCrLf + " ELSE SM.SUBITEMNAME "
        If _FourCMaintain Then
            strSql += vbCrLf + " + ' ' + SC.CUTNAME + ' ' + SR.COLORNAME + ' ' + ST.CLARITYNAME + ' ' + SP.SHAPENAME"
        End If
        strSql += vbCrLf + " END AS SPARTICULAR"
        strSql += vbCrLf + " ,T.STNPCS SPCS,T.STNWT SWEIGHT,T.STNRATE SRATE,T.STNAMT SAMOUNT"
        'strSql += vbCrLf + " ,IDENTITY(INT,1,1)KEYNO"
        strSql += vbCrLf + " ,T.SNO KEYNO"
        strSql += vbCrLf + " INTO TEMPTAGSTONE"
        strSql += vbCrLf + " FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAGSTONE", "CITEMTAGSTONE") & " T "
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.STNITEMID"
        strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = T.STNITEMID AND SM.SUBITEMID = T.STNSUBITEMID"
        If _FourCMaintain Then
            strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..STNCUT  AS SC ON SC.CUTID =T.CUTID "
            strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..STNCOLOR  AS SR ON SR.COLORID =T.COLORID "
            strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..STNCLARITY  AS ST ON ST.CLARITYID=T.CLARITYID "
            strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..STNSHAPE  AS SP ON SP.SHAPEID=T.SHAPEID "
        End If
        strSql += vbCrLf + " WHERE T.TAGSNO IN (SELECT SNO FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAG", "CITEMTAG") & " WHERE TAGNO = '" & tagNo & "' AND ITEMID = " & itemId & ") "
        'strSql += vbCrLf + " ORDER BY T.SNO"
        strSql += vbCrLf + " "
        strSql += vbCrLf + " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPPURTAGSTONE') > 0 DROP TABLE TEMPPURTAGSTONE"
        strSql += vbCrLf + " SELECT CASE WHEN ISNULL(SM.SUBITEMNAME,'') = '' THEN IM.ITEMNAME ELSE SM.SUBITEMNAME END AS PPARTICULAR"
        strSql += vbCrLf + " ,T.STNPCS PPCS,T.STNWT PWEIGHT,T.PURRATE PRATE,T.PURAMT PAMOUNT"
        'strSql += vbCrLf + " ,IDENTITY(INT,1,1)KEYNO"
        strSql += vbCrLf + " ,T.STNSNO KEYNO"
        strSql += vbCrLf + " INTO TEMPPURTAGSTONE"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..PURITEMTAGSTONE T "
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.STNITEMID"
        strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = T.STNITEMID AND SM.SUBITEMID = T.STNSUBITEMID"
        strSql += vbCrLf + " WHERE T.TAGSNO IN (SELECT SNO FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAG", "CITEMTAG") & " WHERE TAGNO = '" & tagNo & "' AND ITEMID = " & itemId & ") "
        strSql += vbCrLf + " SET CONCAT_NULL_YIELDS_NULL ON"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + "		IF (SELECT 1 FROM SYSOBJECTS WHERE NAME='TEMPFINAL') > 0 DROP TABLE TEMPFINAL"
        strSql += vbCrLf + "		SELECT 1ID,ITEMID,PARTICULAR AS SPARTICULAR,PCS AS SPCS,WEIGHT AS SWEIGHT,RATE AS SRATE,AMOUNT AS SAMOUNT,1RESULT,EMPTY,COLHEAD,"
        strSql += vbCrLf + "		P_PARTICULAR AS PPARTICULAR,PCS AS PPCS,PWEIGHT AS PWEIGHT,CASE WHEN PURTOUCH > 0 THEN 0 ELSE PURRATE END AS PRATE,CONVERT(NUMERIC(15,3),AMOUNT) AS PAMOUNT,SALEMODE,SALVALUE	INTO TEMPFINAL FROM TEMPRESULT"
        strSql += vbCrLf + "		UNION ALL"
        If ShowPurchaseDetail Then
            strSql += vbCrLf + "		SELECT 2ID,ITEMID,CASE WHEN PURTOUCH > 0 THEN 'WASTAGE / WEIGHT ' + '* ' + CONVERT(vARCHAR,PURTOUCH) + '%' ELSE 'WASTAGE' END AS SPARTICULAR,0 AS SPCS,MAXWAST AS SWEIGHT,RATE AS SRATE,AMOUNT AS SAMOUNT,1RESULT,EMPTY,COLHEAD,"
            strSql += vbCrLf + "		CASE WHEN PURTOUCH > 0 THEN 'WEIGHT ' + '* ' + CONVERT(vARCHAR,PURTOUCH) + '%' ELSE 'WASTAGE' END AS PPARTICULAR,0 AS PPCS,CASE WHEN PURTOUCH > 0 THEN (PWEIGHT * (PURTOUCH/100)) ELSE PURWASTAGE END AS PWEIGHT,PURRATE AS PRATE,AMOUNT AS PAMOUNT,SALEMODE,SALVALUE	 FROM TEMPRESULT"
            strSql += vbCrLf + "		UNION ALL"
        Else
            strSql += vbCrLf + "		SELECT 2ID,ITEMID,'WASTAGE' AS SPARTICULAR,0 AS SPCS,MAXWAST AS SWEIGHT,RATE AS SRATE,AMOUNT AS SAMOUNT,1RESULT,EMPTY,COLHEAD,"
            strSql += vbCrLf + "		'WASTAGE' AS PPARTICULAR,0 AS PPCS,PURWASTAGE  AS PWEIGHT,PURRATE AS PRATE,AMOUNT AS PAMOUNT,SALEMODE,SALVALUE	 FROM TEMPRESULT"
            strSql += vbCrLf + "		UNION ALL"
        End If
        strSql += vbCrLf + "		SELECT 3ID,'' as ITEMID,'LABOUR' AS SPARTICULAR,0 AS SPCS,0 AS SWEIGHT,RATE AS SRATE,MAXMC AS SAMOUNT,1RESULT,EMPTY,COLHEAD,"
        strSql += vbCrLf + "		'LABOUR' AS PPARTICULAR,0 AS PCS,0 AS PWEIGHT,0 AS PRATE,PURMC AS PAMOUNT,SALEMODE,SALVALUE	 FROM TEMPRESULT"
        strSql += vbCrLf + "		UNION ALL"
        strSql += vbCrLf + "		SELECT DISTINCT 4ID,'' AS ITEMID,'STUDED DETAILS' AS SPARTICULAR,0 AS SPCS ,0 AS SWEIGHT,0 AS SRATE,0 AS SAMOUNT,1 RESULT,'','D',"
        strSql += vbCrLf + "		'STUDED DETAILS' AS PPARTICULAR,0 AS PPCS ,0 AS WEIGHT,0 AS RATE,0 AS AMOUNT,'' SALEMODE,0 SALVALUE"
        strSql += vbCrLf + "        FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAGSTONE", "CITEMTAGSTONE") & " T"
        strSql += vbCrLf + "        WHERE T.TAGSNO IN (SELECT SNO FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAG", "CITEMTAG") & " WHERE TAGNO = '" & tagNo & "' AND ITEMID = " & itemId & ")"
        strSql += vbCrLf + "		UNION ALL"
        strSql += vbCrLf + "        SELECT "
        strSql += vbCrLf + "        5ID,'' AS ITEMID,SPARTICULAR,SPCS"
        strSql += vbCrLf + "        ,SWEIGHT,SRATE,SAMOUNT,1 RESULT,' ' AS EMPTY,'' AS COLHEAD "
        strSql += vbCrLf + "        ,PPARTICULAR,PPCS,PWEIGHT,PRATE,PAMOUNT,'' SALEMODE,0 SALVALUE"
        strSql += vbCrLf + "        FROM TEMPTAGSTONE AS S"
        strSql += vbCrLf + "        FULL JOIN TEMPPURTAGSTONE AS P ON P.KEYNO = S.KEYNO"


        strSql += vbCrLf + "		UNION ALL"
        strSql += vbCrLf + "		SELECT DISTINCT 6ID,'' AS ITEMID,'MISC DETAILS' AS SPARTICULAR,0 AS SPCS ,0 AS SWEIGHT,0 AS SRATE,0 AS SAMOUNT,1 RESULT,'','D',"
        strSql += vbCrLf + "		'MISC DETAILS' AS PPARTICULAR,0 AS PPCS ,0 AS WEIGHT,0 AS RATE,0 AS AMOUNT,'' SALEMODE,0 SALVALUE"
        strSql += vbCrLf + "        FROM " & cnAdminDb & "..ITEMTAGMISCCHAR T"
        strSql += vbCrLf + "        WHERE T.TAGSNO IN (SELECT SNO FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAG", "CITEMTAG") & " WHERE TAGNO = '" & tagNo & "' AND ITEMID = " & itemId & ")"

        strSql += vbCrLf + "        UNION ALL"
        strSql += vbCrLf + "        SELECT 7ID,'' AS ITEMID"
        strSql += vbCrLf + "        ,MISCNAME AS SPARTICULAR "
        strSql += vbCrLf + "        ,NULL AS SPCS,NULL AS SWEIGHT,NULL AS SRATE,AMOUNT AS SAMOUNT,1 RESULT,' ' AS EMPTY,'' AS COLHEAD "
        strSql += vbCrLf + "        ,MISCNAME AS PPARTICULAR "
        strSql += vbCrLf + "        ,NULL AS PPCS,NULL AS RWEIGHT,NULL AS PRATE,PURAMOUNT AS PAMOUNT,'' SALEMODE,0 SALVALUE"
        strSql += vbCrLf + "        FROM  "
        strSql += vbCrLf + "            ( "
        strSql += vbCrLf + "            SELECT  M.MISCNAME,T.AMOUNT,P.PURAMOUNT"
        strSql += vbCrLf + "            FROM " & cnAdminDb & "..ITEMTAGMISCCHAR T"
        strSql += vbCrLf + "            INNER JOIN " & cnAdminDb & "..MISCCHARGES AS M ON M.MISCID = T.MISCID"
        strSql += vbCrLf + "            LEFT OUTER JOIN " & cnAdminDb & "..PURITEMTAGMISCCHAR AS P ON P.MISSNO = T.SNO"
        strSql += vbCrLf + "            WHERE T.TAGSNO IN (SELECT SNO FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAG", "CITEMTAG") & " WHERE TAGNO = '" & tagNo & "' AND ITEMID = " & itemId & ") "
        strSql += vbCrLf + "            )X"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + ""
        strSql += vbCrLf + "		UPDATE TEMPFINAL SET SAMOUNT = CASE WHEN SALEMODE = 'F' THEN SALVALUE ELSE SWEIGHT * SRATE END"
        strSql += vbCrLf + "		WHERE itemid = " & itemId
        strSql += vbCrLf + ""
        strSql += vbCrLf + "		UPDATE TEMPFINAL SET PAMOUNT = PWEIGHT * PRATE"
        strSql += vbCrLf + "		WHERE itemid = " & itemId
        strSql += vbCrLf + ""
        strSql += vbCrLf + "		INSERT INTO TEMPFINAL (ID,SPARTICULAR,SAMOUNT,RESULT,EMPTY,COLHEAD,PPARTICULAR,PAMOUNT) "
        strSql += vbCrLf + "        SELECT '-3','GRAND TOTAL'SPARTICULAR,SVALUE,1 RESULT,''EMPTY,'T'COLHEAD,'GRAND TOTAL',PVALUE"
        strSql += vbCrLf + "        FROM TEMPSALEVALUE"
        If PUR_LANDCOST Then
            strSql += vbCrLf + "        UNION ALL"
            strSql += vbCrLf + "        SELECT '-1','LANDING COST'SPARTICULAR,NULL,1 RESULT,''EMPTY,'T'COLHEAD,'',PURLCOSTPER"
            strSql += vbCrLf + "        FROM TEMPSALEVALUE"
        Else
            strSql += vbCrLf + "        UNION ALL"
            strSql += vbCrLf + "        SELECT '-2','TAX'SPARTICULAR,STAX,1 RESULT,''EMPTY,'T'COLHEAD,'TAX',PTAX"
            strSql += vbCrLf + "        FROM TEMPSALEVALUE"
        End If
        strSql += vbCrLf + "        UNION ALL"
        strSql += vbCrLf + "        SELECT '0','NET TOTAL'SPARTICULAR,SNETVALUE,1 RESULT,''EMPTY,'T'COLHEAD,'NET TOTAL'," & IIf(PUR_LANDCOST, "PURLCOST", "PNETVALUE") & ""
        strSql += vbCrLf + "        FROM TEMPSALEVALUE"
        'strSql += vbCrLf + "		SELECT '0','TOTAL',(SELECT SUM(SAMOUNT) FROM TEMPFINAL),1,'','T','TOTAL',(SELECT SUM(PAMOUNT) FROM TEMPFINAL)"
        strSql += vbCrLf + ""
        strSql += vbCrLf + "        UPDATE TEMPFINAL SET SRATE = 0 WHERE SWEIGHT = 0"
        strSql += vbCrLf + "        UPDATE TEMPFINAL SET PRATE = 0 WHERE PWEIGHT = 0"
        strSql += vbCrLf + "		UPDATE TEMPFINAL SET SPCS = CASE WHEN SPCS = 0 THEN NULL ELSE SPCS END ,"
        strSql += vbCrLf + "		SWEIGHT = CASE WHEN SWEIGHT = 0 THEN NULL ELSE SWEIGHT END,"
        strSql += vbCrLf + "		SRATE =  CASE WHEN SRATE = 0 THEN NULL ELSE SRATE END,"
        strSql += vbCrLf + "		SAMOUNT = CASE WHEN SAMOUNT = 0 THEN NULL ELSE SAMOUNT END,"
        strSql += vbCrLf + "		PPCS = CASE WHEN PPCS = 0 THEN NULL ELSE PPCS END,"
        strSql += vbCrLf + "		PWEIGHT = CASE WHEN PWEIGHT = 0 THEN NULL ELSE PWEIGHT END,"
        strSql += vbCrLf + "		PRATE =  CASE WHEN PRATE = 0 THEN NULL ELSE PRATE END,"
        strSql += vbCrLf + "		PAMOUNT = CASE WHEN PAMOUNT = 0 THEN NULL ELSE PAMOUNT END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = " IF (SELECT ISNULL(SAMOUNT,0) FROM TEMPFINAL WHERE SPARTICULAR='LABOUR')=0"
        strSql += vbCrLf + "		BEGIN  "
        strSql += vbCrLf + "		UPDATE TEMPFINAL SET SAMOUNT=("
        strSql += vbCrLf + "		SELECT SUM(MC)  FROM " & cnAdminDb & "..ITEMTAGMETAL AS M "
        strSql += vbCrLf + "		WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAG", "CITEMTAG") & " WHERE TAGNO = '" & tagNo & "' AND ITEMID = " & itemId & "))"
        strSql += vbCrLf + "		WHERE SPARTICULAR ='LABOUR'"
        strSql += vbCrLf + "		END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = ""
        strSql = "SELECT * FROM TEMPFINAL ORDER BY RESULT,[ID]"

        dtStone = New DataTable
        dtStone.Columns.Add("KEYNO", GetType(Integer))
        dtStone.Columns("KEYNO").AutoIncrement = True
        dtStone.Columns("KEYNO").AutoIncrementSeed = 0
        dtStone.Columns("KEYNO").AutoIncrementStep = 1
        da = New OleDb.OleDbDataAdapter(strSql, cn)
        da.Fill(dtStone)

        dtStone.Columns("KEYNO").SetOrdinal(dtStone.Columns.Count - 1)
        If Not dtStone.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Function
        End If
        gridStoneView.DataSource = dtStone

        gridStoneView.BackgroundColor = Color.WhiteSmoke
        'FillGridGroupStyle_KeyNoWise(gridStoneView)
        gridStoneView.Columns("COLHEAD").Visible = False
        gridStoneView.Columns("ID").Visible = False
        gridStoneView.Columns("ITEMID").Visible = False
        gridStoneView.Columns("RESULT").Visible = False
        gridStoneView.Columns("KEYNO").Visible = False
        gridStoneView.Columns("SALEMODE").Visible = False
        gridStoneView.Columns("SALVALUE").Visible = False

        gridStoneView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridStoneView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter


        With gridStoneView
            With .Columns("SPARTICULAR")
                .Resizable = DataGridViewTriState.False
                .Width = IIf(ShowPurchaseDetail = False, 30, 140)
                .HeaderText = "PARTICULAR"
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("SPCS")
                .Resizable = DataGridViewTriState.False
                .Width = 60 * IIf(ShowPurchaseDetail = False, 2, 1)
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .HeaderText = "PCS"
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("SWEIGHT")
                .Resizable = DataGridViewTriState.False
                .Width = 80 * IIf(ShowPurchaseDetail = False, 2, 1)
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "0.000"
                .HeaderText = "WEIGHT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("SRATE")
                .Resizable = DataGridViewTriState.False
                .Width = 80 * IIf(ShowPurchaseDetail = False, 2, 1)
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .HeaderText = "RATE"
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("SAMOUNT")
                .Resizable = DataGridViewTriState.False
                .Width = 100 * IIf(ShowPurchaseDetail = False, 2, 1)
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .HeaderText = "AMOUNT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("EMPTY")
                .HeaderText = ""
                .Resizable = DataGridViewTriState.False
                .Width = 10
                .Visible = ShowPurchaseDetail
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("PPARTICULAR")
                .Resizable = DataGridViewTriState.False
                .Width = 140 * IIf(ShowPurchaseDetail = False, 2, 1)
                '.Visible = ShowPurchaseDetail
                .Visible = False
                .HeaderText = "PARTICULAR"
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("SPARTICULAR")
                .Width = .Width + gridStoneView.Columns("PPARTICULAR").Width
            End With
            With .Columns("PPCS")
                .Width = 60 * IIf(ShowPurchaseDetail = False, 2, 1)
                .Resizable = DataGridViewTriState.False
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Visible = ShowPurchaseDetail
                .HeaderText = "PCS"
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("PWEIGHT")
                .Width = 80 * IIf(ShowPurchaseDetail = False, 2, 1)
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Resizable = DataGridViewTriState.False
                .DefaultCellStyle.Format = "0.000"
                .Visible = ShowPurchaseDetail
                .HeaderText = "WEIGHT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("PRATE")
                .Width = 80 * IIf(ShowPurchaseDetail = False, 2, 1)
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Resizable = DataGridViewTriState.False
                .Visible = ShowPurchaseDetail
                .HeaderText = "RATE"
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("PAMOUNT")
                .Width = 100 * IIf(ShowPurchaseDetail = False, 2, 1)
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Visible = ShowPurchaseDetail
                .Resizable = DataGridViewTriState.False
                .HeaderText = "AMOUNT"
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
        End With
        ''TAGMULTIMETAL
        strSql = " SELECT"
        strSql += " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = M.CATCODE) AS CATNAME"
        strSql += " ,GRSWT AS WEIGHT,RATE ,AMOUNT,WAST AS WASTAGE,MC "
        strSql += " FROM " & cnAdminDb & "..ITEMTAGMETAL AS M"
        strSql += " WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & ".." & IIf(Tag, "ITEMTAG", "CITEMTAG") & " WHERE TAGNO = '" & tagNo & "' AND ITEMID = " & itemId & ")"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtMultiMetal)
        gridMultiMetal.DataSource = dtMultiMetal
        gridMultiMetal.BackgroundColor = Color.WhiteSmoke
        With gridMultiMetal
            With .Columns("CATNAME")
                .Width = 300
            End With
            With .Columns("WEIGHT")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("RATE")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("AMOUNT")
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
        End With
        Dim ServerSystemName As String = GetAdmindbSoftValue("SERVER_NAME")
        Dim ServerAdminName As String = GetAdmindbSoftValue("SERVER_ACCNAME")
        Dim ServerPwd As String = GetAdmindbSoftValue("SERVER_ACCPWD")
        If ServerSystemName <> "" And ServerAdminName <> "" And ServerPwd <> "" Then
            notify = New NotifyIcon()
            notify.Text = "Server UserName & Password Available "
            notify.Visible = True
            notify.Icon = My.Resources.email
            notify.ShowBalloonTip(20000, "Information", "UserName:" + ServerAdminName + " Pwd:" + ServerPwd, ToolTipIcon.Info)
        End If
    End Function

    Private Sub btnprevious_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnprevious.Click
        If dtImage.Rows.Count > 1 Then
            i = tempivalue - 1
            ' If i = dtImage.Rows.Count - 1 Then btnprevious.Enabled = False
            If i >= 0 Then
                tagdetails(dtImage.Rows(i).Item("TAGNO").ToString, dtImage.Rows(i).Item("ITEMID").ToString)
                tempivalue = tempivalue - 1
            End If
        End If
       
    End Sub
End Class