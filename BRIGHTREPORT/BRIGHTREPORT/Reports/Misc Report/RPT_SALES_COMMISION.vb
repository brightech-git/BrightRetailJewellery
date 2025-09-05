Imports System.Data.OleDb

Public Class RPT_SALES_COMMISION
    Dim StrSql As String
    Dim Da As OleDbDataAdapter
    Dim Cmd As OleDbCommand
    Dim objGridShower As frmGridDispDia
    Dim dtCostCentre As New DataTable
    Dim dtEMP, dtmetal, dtsubITEM, dtCounter As New DataTable
    Dim dtITEM As New DataTable
    Dim dtCompany As New DataTable
    Dim itemfilter As String = ""
    Dim insper As Double = 0
    Dim Sysid As String
    Dim Comm_Costcentre_Based As Boolean = IIf(GetAdmindbSoftValue("COMM_COSTCENTRE_BASED", "") = "Y", True, False)

    Private Sub RPT_SALES_COMMISION_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyCode = Keys.F3 Then
            btnNew_Click(Me, New EventArgs)
        ElseIf e.KeyCode = Keys.F12 Then
            btnExit_Click(Me, New EventArgs)
        End If
    End Sub
    Private Sub RPT_SALES_COMMISION_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GrpContainer.Location = New Point((ScreenWid - GrpContainer.Width) / 2, ((ScreenHit - 128) - GrpContainer.Height) / 2)
        StrSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        StrSql += " UNION ALL"
        StrSql += " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        StrSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)

        StrSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        StrSql += " UNION ALL"
        StrSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        StrSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False

        StrSql = " SELECT 'ALL' METALNAME,'ALL' METALID,1 RESULT"
        StrSql += " UNION ALL"
        StrSql += " SELECT METALNAME,CONVERT(vARCHAR,METALID),2 RESULT FROM " & cnAdminDb & "..METALMAST"
        StrSql += " ORDER BY RESULT,METALNAME"
        dtmetal = New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtmetal)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbmetal, dtmetal, "METALNAME", , "ALL")

        StrSql = " SELECT 'ALL' ITEMCTRNAME,'ALL' ITEMCTRID,1 RESULT"
        StrSql += " UNION ALL"
        StrSql += " SELECT ITEMCTRNAME,CONVERT(vARCHAR,ITEMCTRID),2 RESULT FROM " & cnAdminDb & "..ITEMCOUNTER"
        StrSql += " ORDER BY RESULT,ITEMCTRNAME"
        dtCounter = New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtCounter)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbCounter, dtCounter, "ITEMCTRNAME", , "ALL")

        StrSql = " SELECT 'ALL' ITEMNAME,'ALL' ITEMID,1 RESULT"
        StrSql += " UNION ALL"
        StrSql += " SELECT ITEMNAME,CONVERT(vARCHAR,ITEMID),2 RESULT FROM " & cnAdminDb & "..ITEMMAST"
        StrSql += " ORDER BY RESULT,ITEMNAME"

        dtITEM = New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtITEM)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbitemname, dtITEM, "ITEMNAME", , "ALL")


        StrSql = " SELECT 'ALL' SUBITEMNAME,'ALL' SUBITEMID,1 RESULT"
        StrSql += " UNION ALL"
        StrSql += " SELECT SUBITEMNAME,CONVERT(vARCHAR,SUBITEMID),2 RESULT FROM " & cnAdminDb & "..SUBITEMMAST"
        StrSql += " ORDER BY RESULT,SUBITEMNAME"

        dtsubITEM = New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtsubITEM)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbsubitem, dtsubITEM, "SUBITEMNAME", , "ALL")

        cmbViewBy.Items.Add("COUNTER WISE SUMMARY")
        cmbViewBy.Items.Add("COUNTER,METAL,ITEM,SUB ITEM WISE SUMMARY")
        cmbViewBy.Items.Add("ITEM WISE CUMULATIVE")
        cmbViewBy.Items.Add("ITEM WISE SUMMARY")
        cmbViewBy.Items.Add("ITEM,SUB ITEM WISE SUMMARY")
        cmbViewBy.Items.Add("TRANDATE WISE SUMMARY")
        cmbViewBy.Items.Add("TRANDATE,EMPLOYEE WISE SUMMARY")

        StrSql = " SELECT 'ALL' EMPNAME,'ALL' EMPID,1 RESULT"
        StrSql += " UNION ALL"
        StrSql += " SELECT EMPNAME,CONVERT(vARCHAR,EMPID),2 RESULT FROM " & cnAdminDb & "..EMPMASTER"
        StrSql += " ORDER BY RESULT,EMPNAME"
        dtEMP = New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtEMP)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbemployee, dtEMP, "EMPNAME", , "ALL")
        If rbtAll.Checked = False And rbtGrpEmployee.Checked = False And
        rbtGrpCounter.Checked = False And rbtproduct.Checked = False And rbtMetal.Checked = False Then
            rbtGrpEmployee.Checked = True
        End If
        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        ' chkWithItem.Checked = False
        rbtGrpEmployee.Checked = True
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , "ALL")
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        BrighttechPack.GlobalMethods.FillCombo(chkcmbitemname, dtITEM, "ITEMNAME", , "ALL")
        BrighttechPack.GlobalMethods.FillCombo(chkcmbemployee, dtEMP, "EMPNAME", , "ALL")
        BrighttechPack.GlobalMethods.FillCombo(chkcmbsubitem, dtsubITEM, "SUBITEMNAME", , "ALL")
        BrighttechPack.GlobalMethods.FillCombo(chkcmbmetal, dtmetal, "METALNAME", , "ALL")
        BrighttechPack.GlobalMethods.FillCombo(chkcmbCounter, dtCounter, "ITEMCTRNAME", , "ALL")
        Prop_Gets()
        dtpFrom.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Prop_Sets()
        Sysid = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
        itemfilter = ""
        Dim chkMetalid As String = GetSelectedMetalid(chkcmbmetal, True)
        Dim chkCompId As String = GetQryStringForSp(chkCmbCompany.Text, cnAdminDb & "..COMPANY", "COMPANYID", "COMPANYNAME", True)
        Dim chkCostId As String = GetQryStringForSp(chkCmbCostCentre.Text, cnAdminDb & "..COSTCENTRE", "COSTID", "COSTNAME", True)
        Dim chkempid As String = GetQryStringForSp(chkcmbemployee.Text, cnAdminDb & "..EMPMASTER", "EMPID", "EMPNAME", True)
        Dim chkitemid As String = GetQryStringForSp(chkcmbitemname.Text, cnAdminDb & "..ITEMMAST", "ITEMID", "ITEMNAME", True)
        Dim chksubitemid As String = GetQryStringForSp(chkcmbsubitem.Text, cnAdminDb & "..SUBITEMMAST", "SUBITEMID", "SUBITEMNAME", True)
        Dim StoneDetail As Boolean = chkStoneDetails.Checked
        Dim chkItemCtr As String = GetQryStringForSp(chkcmbCounter.Text, cnAdminDb & "..ITEMCOUNTER", "ITEMCTRID", "ITEMCTRNAME", True)

        Dim strEmp As String = ""
        If cmbViewBy.Text = "TRANDATE,EMPLOYEE WISE SUMMARY" Then
            strEmp = "EM.EMPNAME "
        Else
            strEmp = "EM.EMPNAME + ' ( ' + CONVERT(VARCHAR,EM.EMPID) + ' )' "
        End If
        StrSql = "IF OBJECT_ID('" & cnAdminDb & "..FLATINCENTIVE')IS NULL CREATE TABLE " & cnAdminDb & "..FLATINCENTIVE(COSTID VARCHAR(2),MONTH VARCHAR(15),AMOUNT NUMERIC(15,3),CONSTRAINT [COSTID_MONTH] UNIQUE NONCLUSTERED(COSTID,MONTH))"
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.ExecuteNonQuery()

        CheckAndInsertsoftcontrol("INCENTIVEEXCLUDEITEM", "EXCLUDE ITEM FOR REPORT", "T", "", "A")
        StrSql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='EXCLUDEITEMINRPT'"
        itemfilter = GetSqlValue(cn, StrSql)

        CheckAndInsertsoftcontrol("INCENTIVEPERCENTAGE", "INCENTIVE PERCENTAGE FOR SALES COMMISION", "N", "0", "A")
        StrSql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='INCENTIVEPERCENTAGE'"
        insper = GetSqlValue(cn, StrSql)

        StrSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & "','U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & ""
        StrSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & "','U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & ""
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        StrSql = vbCrLf + " SELECT * INTO TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " FROM( "
        StrSql += vbCrLf + " SELECT  CONVERT(NUMERIC(15,2),0) AS COMMISION,CONVERT(NUMERIC(15,2),0) AS SALESCOMM,CONVERT(NUMERIC(15,2),0) AS RTNCOMM"
        StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0) AS OTHERCOMM,M.METALNAME AS METAL,IM.ITEMNAME +'(' +CONVERT (VARCHAR,IM.ITEMID)+')'AS ITEM"
        StrSql += vbCrLf + " ,SM.SUBITEMNAME +'(' +CONVERT (VARCHAR,SM.SUBITEMID)+')' AS SUBITEMNAME,I.TRANNO,I.TRANDATE,I.TAGNO"
        StrSql += vbCrLf + " ,(SELECT RECDATE FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID = I.ITEMID And SUBITEMID = I.SUBITEMID And TAGNO = I.TAGNO) RECDATE"
        StrSql += vbCrLf + " ,I.PCS,I.GRSWT,I.NETWT"
        If StoneDetail = True Then
            StrSql += vbCrLf + " ,0 STNPCS,0 STNWT_G,0 STNWT_C,0 DIAPCS,0 DIAWT_C"
        End If
        StrSql += vbCrLf + " ,I.WASTPER,I.AMOUNT"
        StrSql += vbCrLf + " ,ISNULL(SM.CALTYPE,IM.CALTYPE) AS CALTYPE,CONVERT(NUMERIC(15,2),0) AS COMM_FLAT"
        StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0) AS COMM_GRM,CONVERT(NUMERIC(15,2),0) AS COMM_PER"
        StrSql += vbCrLf + " ,CONVERT(VARCHAR(1),'')AS UPDATED"
        StrSql += vbCrLf + " ,I.ITEMCTRID,IM.METALID,I.ITEMID,I.SUBITEMID,I.EMPID," & strEmp & " EMPNAME,CO.ITEMCTRNAME,'SA' AS SEP"
        StrSql += vbCrLf + " ,I.COSTID FROM " & cnStockDb & "..ISSUE AS I"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = I.ITEMID"
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = I.ITEMID AND SM.SUBITEMID = I.SUBITEMID"
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..EMPMASTER AS EM ON EM.EMPID = I.EMPID "
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER AS CO ON CO.ITEMCTRID = I.ITEMCTRID "
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST AS M ON M.METALID=IM.METALID  "
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND I.TRANTYPE = 'SA' AND ISNULL(I.CANCEL,'') = ''"
        If chkMetalid <> "''" And chkMetalid <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(IM.METALID,'')IN(" & chkMetalid & ") "
        If chkempid <> "" And chkempid <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(I.EMPID,'')IN(" & chkempid & ") "
        If chkitemid <> "" And chkitemid <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(I.ITEMID,'')IN(" & chkitemid & ") "
        If chkItemCtr <> "" And chkItemCtr <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(I.ITEMCTRID,'')IN(" & chkItemCtr & ") "
        If chksubitemid <> "" And chksubitemid <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(I.SUBITEMID,'')IN(" & chksubitemid & ") "
        If chkCostId <> "" And chkCostId <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(I.COSTID,'') IN (" & chkCostId & ")"
        If chkCompId <> "" And chkCompId <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(I.COMPANYID,'') IN (" & chkCompId & ")"
        If itemfilter <> "" Then StrSql += " AND I.ITEMID NOT IN(" & itemfilter & ")"
        StrSql += vbCrLf + " UNION ALL "
        StrSql += vbCrLf + " SELECT CONVERT(NUMERIC(15,2),0) AS COMMISION,CONVERT(NUMERIC(15,2),0) AS SALESCOMM,CONVERT(NUMERIC(15,2),0) AS RTNCOMM"
        StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0) AS OTHERCOMM,M.METALNAME AS METAL,IM.ITEMNAME +'(' +CONVERT (VARCHAR,IM.ITEMID)+')' AS ITEM"
        StrSql += vbCrLf + " ,SM.SUBITEMNAME +'(' +CONVERT (VARCHAR,SM.SUBITEMID)+')' AS SUBITEMNAME ,ISS.TRANNO,ISS.TRANDATE,I.TAGNO"
        StrSql += vbCrLf + " ,(SELECT RECDATE FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID = I.ITEMID And SUBITEMID = I.SUBITEMID And TAGNO = I.TAGNO) RECDATE"
        StrSql += vbCrLf + " ,0 PCS"
        If Not StoneDetail Then
            StrSql += vbCrLf + " ,ISS.STNWT GRSWT,ISS.STNWT  NETWT"
        Else
            StrSql += vbCrLf + " ,0 GRSWT,0 NETWT"
            StrSql += vbCrLf + " ,(CASE WHEN IM.DIASTONE='S' THEN ISS.STNPCS END)AS STNPCS"
            StrSql += vbCrLf + " ,(CASE WHEN ISS.STONEUNIT='G' AND IM.DIASTONE='S' THEN ISS.STNWT END)STNWT_G"
            StrSql += vbCrLf + " ,(CASE WHEN ISS.STONEUNIT='C' AND IM.DIASTONE='S' THEN ISS.STNWT END)STNWT_C"
            StrSql += vbCrLf + " ,(CASE WHEN IM.DIASTONE='D' THEN ISS.STNPCS END)AS DIAPCS"
            StrSql += vbCrLf + " ,(CASE WHEN ISS.STONEUNIT='C' AND IM.DIASTONE='D' THEN ISS.STNWT END)DIAWT_C"
        End If
        StrSql += vbCrLf + " ,NULL WASTPER,ISS.STNAMT"
        StrSql += vbCrLf + " ,ISNULL(SM.CALTYPE,IM.CALTYPE) AS CALTYPE,CONVERT(NUMERIC(15,2),0) AS COMM_FLAT"
        StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0) AS COMM_GRM,CONVERT(NUMERIC(15,2),0) AS COMM_PER"
        StrSql += vbCrLf + " ,CONVERT(VARCHAR(1),'')AS UPDATED"
        StrSql += vbCrLf + " ,I.ITEMCTRID,IM.METALID,ISS.STNITEMID,ISS.STNSUBITEMID,I.EMPID," & strEmp & " EMPNAME,CO.ITEMCTRNAME,'SA' AS SEP"
        StrSql += vbCrLf + " ,I.COSTID FROM " & cnStockDb & "..ISSSTONE AS ISS"
        StrSql += vbCrLf + " INNER JOIN " & cnStockDb & "..ISSUE AS I ON ISS.ISSSNO = I.SNO"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ISS.STNITEMID"
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = ISS.STNITEMID AND SM.SUBITEMID = ISS.STNSUBITEMID"
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..EMPMASTER AS EM ON EM.EMPID = I.EMPID "
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER AS CO ON CO.ITEMCTRID = I.ITEMCTRID "
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST AS M ON M.METALID=IM.METALID  "
        StrSql += vbCrLf + " WHERE ISS.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND I.TRANTYPE = 'SA' AND ISNULL(I.CANCEL,'') = ''"
        If chkMetalid <> "''" And chkMetalid <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(IM.METALID,'')IN(" & chkMetalid & ") "
        If chkempid <> "" And chkempid <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(I.EMPID,'')IN(" & chkempid & ") "
        If chkitemid <> "" And chkitemid <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(ISS.STNITEMID,'')IN(" & chkitemid & ") "
        If chkItemCtr <> "" And chkItemCtr <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(I.ITEMCTRID,'')IN(" & chkItemCtr & ") "
        If chksubitemid <> "" And chksubitemid <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(ISS.STNSUBITEMID,'')IN(" & chksubitemid & ") "
        If chkCostId <> "" And chkCostId <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(ISS.COSTID,'') IN (" & chkCostId & ")"
        If chkCompId <> "" And chkCompId <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(ISS.COMPANYID,'') IN (" & chkCompId & ")"
        If itemfilter <> "" Then StrSql += " AND ISS.STNITEMID NOT IN(" & itemfilter & ")"
        StrSql += vbCrLf + " )X "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        StrSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & ""

        StrSql += vbCrLf + " SELECT * FROM (SELECT CONVERT(NUMERIC(15,2),0) AS COMMISION"
        StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0) AS SALESCOMM,CONVERT(NUMERIC(15,2),0) AS RTNCOMM,CONVERT(NUMERIC(15,2),0) AS OTHERCOMM"
        StrSql += vbCrLf + " ,M.METALNAME AS METAL,IM.ITEMNAME + '('+ CONVERT(VARCHAR,I.ITEMID) +')' AS ITEM,SM.SUBITEMNAME + '('+ CONVERT(VARCHAR,I.SUBITEMID) +')' SUBITEMNAME,I.TRANNO,I.TRANDATE"
        StrSql += vbCrLf + " ,I.TAGNO"
        StrSql += vbCrLf + " ,(SELECT RECDATE FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID = I.ITEMID And SUBITEMID = I.SUBITEMID And TAGNO = I.TAGNO) RECDATE"
        StrSql += vbCrLf + " ,I.PCS,I.GRSWT,I.NETWT"
        If StoneDetail = True Then
            StrSql += vbCrLf + " ,0 STNPCS,0 STNWT_G,0 STNWT_C,0 DIAPCS,0 DIAWT_C"
        End If
        StrSql += vbCrLf + " ,I.WASTPER,I.AMOUNT,ISNULL(SM.CALTYPE,IM.CALTYPE) AS CALTYPE"
        StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0) AS COMM_FLAT,CONVERT(NUMERIC(15,2),0) AS COMM_GRM"
        StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0) AS COMM_PER,CONVERT(VARCHAR(1),'')AS UPDATED"
        StrSql += vbCrLf + " ,I.ITEMCTRID,IM.METALID,I.ITEMID,I.SUBITEMID,I.EMPID," & strEmp & " EMPNAME,CO.ITEMCTRNAME,'SR' AS SEP"
        StrSql += vbCrLf + " ,I.COSTID FROM " & cnStockDb & "..RECEIPT AS I"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = I.ITEMID"
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = I.ITEMID AND SM.SUBITEMID = I.SUBITEMID"
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..EMPMASTER AS EM ON EM.EMPID = I.EMPID "
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER AS CO ON CO.ITEMCTRID = I.ITEMCTRID "
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST AS M ON M.METALID=IM.METALID  "
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND I.TRANTYPE = 'SR' AND ISNULL(I.CANCEL,'') = ''"
        If chkMetalid <> "''" And chkMetalid <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(IM.METALID,'')IN(" & chkMetalid & ") "
        If chkempid <> "" And chkempid <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(I.EMPID,'')IN(" & chkempid & ") "
        If chkitemid <> "" And chkitemid <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(I.ITEMID,'')IN(" & chkitemid & ") "
        If chkItemCtr <> "" And chkItemCtr <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(I.ITEMCTRID,'')IN(" & chkItemCtr & ") "

        If chksubitemid <> "" And chksubitemid <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(I.SUBITEMID,'')IN(" & chksubitemid & ") "
        If chkCostId <> "" And chkCostId <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(I.COSTID,'') IN (" & chkCostId & ")"
        If chkCompId <> "" And chkCompId <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(I.COMPANYID,'') IN (" & chkCompId & ")"
        If itemfilter <> "" Then StrSql += " AND I.ITEMID NOT IN(" & itemfilter & ")"
        StrSql += vbCrLf + " UNION ALL "
        StrSql += vbCrLf + " SELECT CONVERT(NUMERIC(15,2),0) AS COMMISION,CONVERT(NUMERIC(15,2),0) AS SALESCOMM,CONVERT(NUMERIC(15,2),0) AS RTNCOMM"
        StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0) AS OTHERCOMM,M.METALNAME AS METAL,IM.ITEMNAME +'(' +CONVERT (VARCHAR,IR.STNITEMID)+')' AS ITEM"
        StrSql += vbCrLf + " ,SM.SUBITEMNAME +'('+CONVERT (VARCHAR,IR.STNSUBITEMID)+')' AS SUBITEMNAME,IR.TRANNO,IR.TRANDATE,I.TAGNO"
        StrSql += vbCrLf + " ,(SELECT RECDATE FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID = I.ITEMID And SUBITEMID = I.SUBITEMID And TAGNO = I.TAGNO) RECDATE"
        StrSql += vbCrLf + " ,0 ISSPCS "
        If Not StoneDetail Then
            StrSql += vbCrLf + " ,IR.STNWT GRSWT,IR.STNWT NETWT"
        Else
            StrSql += vbCrLf + " ,0 GRSWT,0 NETWT"
            StrSql += vbCrLf + " ,(CASE WHEN IM.DIASTONE='S' THEN IR.STNPCS END)STNPCS"
            StrSql += vbCrLf + " ,(CASE WHEN IR.STONEUNIT='G' AND IM.DIASTONE='S' THEN IR.STNWT END)STNWT_G"
            StrSql += vbCrLf + " ,(CASE WHEN IR.STONEUNIT='C' AND IM.DIASTONE='S' THEN IR.STNWT END)STNWT_C"
            StrSql += vbCrLf + " ,(CASE WHEN IM.DIASTONE='C' THEN IR.STNPCS END)DIAPCS"
            StrSql += vbCrLf + " ,(CASE WHEN IR.STONEUNIT='C' AND IM.DIASTONE='D' THEN IR.STNWT END)DIAWT_C"
        End If

        StrSql += vbCrLf + " ,NULL WASTPER,IR.STNAMT"
        StrSql += vbCrLf + " ,ISNULL(SM.CALTYPE,IM.CALTYPE) AS CALTYPE,CONVERT(NUMERIC(15,2),0) AS COMM_FLAT"
        StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0) AS COMM_GRM,CONVERT(NUMERIC(15,2),0) AS COMM_PER"
        StrSql += vbCrLf + " ,CONVERT(VARCHAR(1),'')AS UPDATED"
        StrSql += vbCrLf + " ,I.ITEMCTRID,IM.METALID,IR.STNITEMID,IR.STNSUBITEMID,I.EMPID," & strEmp & " EMPNAME,CO.ITEMCTRNAME,'SR' AS SEP"
        StrSql += vbCrLf + " ,I.COSTID FROM " & cnStockDb & "..RECEIPTSTONE AS IR"
        StrSql += vbCrLf + " INNER JOIN " & cnStockDb & "..RECEIPT AS I ON IR.ISSSNO = I.SNO"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = IR.STNITEMID"
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = IR.STNITEMID AND SM.SUBITEMID = IR.STNSUBITEMID"
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..EMPMASTER AS EM ON EM.EMPID = I.EMPID "
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER AS CO ON CO.ITEMCTRID = I.ITEMCTRID "
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST AS M ON M.METALID=IM.METALID  "
        StrSql += vbCrLf + " WHERE IR.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND I.TRANTYPE = 'SR' AND ISNULL(I.CANCEL,'') = ''"
        If chkMetalid <> "''" And chkMetalid <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(IM.METALID,'')IN(" & chkMetalid & ") "
        If chkempid <> "" And chkempid <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(I.EMPID,'')IN(" & chkempid & ") "
        If chkitemid <> "" And chkitemid <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(IR.STNITEMID,'')IN(" & chkitemid & ") "
        If chkItemCtr <> "" And chkItemCtr <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(I.ITEMCTRID,'')IN(" & chkItemCtr & ") "
        If chksubitemid <> "" And chksubitemid <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(IR.STNSUBITEMID,'')IN(" & chksubitemid & ") "
        If chkCostId <> "" And chkCostId <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(IR.COSTID,'') IN (" & chkCostId & ")"
        If chkCompId <> "" And chkCompId <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(IR.COMPANYID,'') IN (" & chkCompId & ")"
        If itemfilter <> "" Then StrSql += " AND IR.STNITEMID NOT IN(" & itemfilter & ")"

        StrSql += vbCrLf + " )X "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        If rbtGrpCounter.Checked Then
            StrSql = vbCrLf + " UPDATE TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " SET COMM_FLAT = (SC.COMMISION*ISNULL(TS.PCS,1))"
            'StrSql += vbCrLf + " CASE WHEN SC.BASEDON IN('V') THEN  (SC.COMMISION*ISNULL(TS.PCS,1))ELSE SC.COMMISION END"
            StrSql += vbCrLf + ",COMM_GRM = SC.COMMISIONGRM,COMM_PER = SC.COMMISIONPER,UPDATED = 'Y'"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " AS TS"
            StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..SALES_COMMISION  AS SC ON SC.ITEMCTRID = TS.ITEMCTRID "
            If chkitemid <> "" And chkitemid <> "ALL" Then StrSql += vbCrLf + " AND SC.ITEMID = TS.ITEMID "
            If chksubitemid <> "" And chksubitemid <> "ALL" Then StrSql += vbCrLf + " AND SC.SUBITEMID = TS.SUBITEMID "
            StrSql += vbCrLf + " AND (CASE WHEN TS.CALTYPE IN ('R','F') THEN TS.AMOUNT ELSE TS.NETWT END) BETWEEN SC.FROM_VAL AND SC.TO_VAL"
            StrSql += vbCrLf + " WHERE ISNULL(TS.UPDATED,'') = ''"
            If chkCostId <> "" And chkCostId <> "ALL" And Comm_Costcentre_Based Then
                StrSql += vbCrLf + " AND ISNULL(SC.COSTID,'') IN (" & chkCostId & ") AND TS.COSTID =SC.COSTID "
            End If
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        End If

        StrSql = vbCrLf + " UPDATE TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " SET COMM_FLAT = (SC.COMMISION*ISNULL(TS.PCS,1))"
        'StrSql += vbCrLf + " CASE WHEN SC.BASEDON IN('V') THEN  (SC.COMMISION*ISNULL(TS.PCS,1))ELSE SC.COMMISION END"
        StrSql += vbCrLf + ",COMM_GRM = SC.COMMISIONGRM,COMM_PER = SC.COMMISIONPER,UPDATED = 'Y'"
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " AS TS"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..SALES_COMMISION  AS SC ON  SC.ITEMID = TS.ITEMID AND SC.TAGNO = TS.TAGNO "
        StrSql += vbCrLf + " AND SC.BASEDON = 'T'"
        StrSql += vbCrLf + " WHERE ISNULL(TS.UPDATED,'') = ''"
        If chkCostId <> "" And chkCostId <> "ALL" And Comm_Costcentre_Based Then
            StrSql += vbCrLf + " AND ISNULL(SC.COSTID,'') IN (" & chkCostId & ") AND TS.COSTID =SC.COSTID "
        End If
        'StrSql += vbCrLf + " AND CASE WHEN SC.BASEDON = 'T' THEN TS.PCS ELSE(CASE WHEN TS.CALTYPE IN ('R','F') THEN TS.AMOUNT ELSE TS.NETWT END)END BETWEEN SC.FROM_VAL AND SC.TO_VAL"
        'StrSql += vbCrLf + " WHERE ISNULL(TS.UPDATED,'') = ''"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()


        StrSql = vbCrLf + " UPDATE TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " SET COMM_FLAT = (SC.COMMISION*ISNULL(TS.PCS,1))"
        'StrSql += vbCrLf + " CASE WHEN SC.BASEDON IN('V') THEN  (SC.COMMISION*ISNULL(TS.PCS,1))ELSE SC.COMMISION END"
        StrSql += vbCrLf + ",COMM_GRM = SC.COMMISIONGRM,COMM_PER = SC.COMMISIONPER,UPDATED = 'Y'"
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " AS TS"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..SALES_COMMISION  AS SC ON (SC.ITEMCTRID = TS.ITEMCTRID OR ISNULL(SC.ITEMCTRID,0) = 0) AND SC.ITEMID = TS.ITEMID AND SC.SUBITEMID = TS.SUBITEMID "
        StrSql += vbCrLf + " AND SC.BASEDON = 'R'"
        StrSql += vbCrLf + " WHERE ISNULL(TS.UPDATED,'') = ''"
        StrSql += vbCrLf + " AND TS.TAGNO NOT IN (SELECT TAGNO FROM " & cnAdminDb & "..SALES_COMMISION WHERE BASEDON = 'T')"
        StrSql += vbCrLf + " AND TS.RECDATE IS NOT NULL AND RECDATE_FROM IS NOT NULL AND RECDATE_TO IS NOT NULL"
        StrSql += vbCrLf + " AND TS.RECDATE BETWEEN RECDATE_FROM AND RECDATE_TO "
        If chkCostId <> "" And chkCostId <> "ALL" And Comm_Costcentre_Based Then
            StrSql += vbCrLf + " AND ISNULL(SC.COSTID,'') IN (" & chkCostId & ") AND TS.COSTID =SC.COSTID "
        End If
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()


        StrSql = vbCrLf + " UPDATE TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " SET COMM_FLAT = (SC.COMMISION*ISNULL(TS.PCS,1))"
        'StrSql += vbCrLf + " CASE WHEN SC.BASEDON IN('V') THEN  (SC.COMMISION*ISNULL(TS.PCS,1))ELSE SC.COMMISION END"
        StrSql += vbCrLf + ",COMM_GRM = SC.COMMISIONGRM,COMM_PER = SC.COMMISIONPER,UPDATED = 'Y'"
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " AS TS"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..SALES_COMMISION  AS SC ON (SC.ITEMCTRID = TS.ITEMCTRID OR ISNULL(SC.ITEMCTRID,0) = 0) AND SC.ITEMID = TS.ITEMID AND SC.SUBITEMID = TS.SUBITEMID "
        StrSql += vbCrLf + " AND CASE WHEN SC.BASEDON = 'P' THEN TS.PCS WHEN SC.BASEDON = 'A' THEN DATEDIFF(DAY,RECDATE,TRANDATE) ELSE(CASE WHEN TS.CALTYPE IN ('R','F') THEN TS.AMOUNT ELSE TS.NETWT END)END BETWEEN SC.FROM_VAL AND SC.TO_VAL"
        StrSql += vbCrLf + " WHERE ISNULL(TS.UPDATED,'') = ''"
        StrSql += vbCrLf + " AND TS.TAGNO NOT IN (SELECT TAGNO FROM " & cnAdminDb & "..SALES_COMMISION WHERE BASEDON = 'T')"
        StrSql += vbCrLf + " AND RECDATE_FROM IS NULL AND RECDATE_TO IS NULL"
        If chkCostId <> "" And chkCostId <> "ALL" And Comm_Costcentre_Based Then
            StrSql += vbCrLf + " AND ISNULL(SC.COSTID,'') IN (" & chkCostId & ") AND TS.COSTID =SC.COSTID "
        End If
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        StrSql = vbCrLf + " UPDATE TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " Set COMM_FLAT = (SC.COMMISION*ISNULL(TS.PCS,1))"
        'StrSql += vbCrLf + " Case When SC.BASEDON In('V') THEN  (SC.COMMISION*ISNULL(TS.PCS,1))ELSE SC.COMMISION END"
        StrSql += vbCrLf + ",COMM_GRM = SC.COMMISIONGRM,COMM_PER = SC.COMMISIONPER,UPDATED = 'Y'"
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " AS TS"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..SALES_COMMISION  AS SC ON (SC.ITEMCTRID = TS.ITEMCTRID OR ISNULL(SC.ITEMCTRID,0) = 0) AND SC.ITEMID = TS.ITEMID AND SC.SUBITEMID = 0 "
        StrSql += vbCrLf + " AND CASE WHEN SC.BASEDON = 'P' THEN TS.PCS ELSE(CASE WHEN TS.CALTYPE IN ('R','F') THEN TS.AMOUNT ELSE TS.NETWT END) END BETWEEN SC.FROM_VAL AND SC.TO_VAL"
        StrSql += vbCrLf + " WHERE ISNULL(TS.UPDATED,'') = ''"
        StrSql += vbCrLf + " AND TS.TAGNO NOT IN (SELECT TAGNO FROM " & cnAdminDb & "..SALES_COMMISION WHERE BASEDON = 'T')"
        StrSql += vbCrLf + " AND RECDATE_FROM IS NULL AND RECDATE_TO IS NULL"
        If chkCostId <> "" And chkCostId <> "ALL" And Comm_Costcentre_Based Then
            StrSql += vbCrLf + " AND ISNULL(SC.COSTID,'') IN (" & chkCostId & ") AND TS.COSTID =SC.COSTID "
        End If
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        StrSql = vbCrLf + " UPDATE TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " SET COMMISION =  "
        StrSql += vbCrLf + " CASE WHEN ISNULL(COMM_PER,0) <> 0 THEN AMOUNT * (COMM_PER/100)else 0 end"
        StrSql += vbCrLf + " + CASE WHEN ISNULL(COMM_GRM,0) <> 0 THEN NETWT * COMM_GRM ELSE 0 END"
        StrSql += vbCrLf + " + CASE WHEN ISNULL(COMM_FLAT,0)<>0 THEN COMM_FLAT ELSE 0 END"
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " AS TS"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        If Val(txtCommPercentage_AMT.Text) <> 0 Then
            StrSql = vbCrLf + " UPDATE TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " SET SALESCOMM=  "
            StrSql += vbCrLf + " ((COMMISION /100) * " & Val(txtCommPercentage_AMT.Text) & ") "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " AS TS"
            StrSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " SET OTHERCOMM=  "
            StrSql += vbCrLf + " ((COMMISION/100) * " & (100 - Val(txtCommPercentage_AMT.Text)) & ") "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " AS TS"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        End If
        If chkshowroomincentive.Checked Then
            If insper = 0 Then MsgBox("Incentive percentage should not be Zero." & vbCrLf & "Pls update softcontrol Id is INCENTIVEPERCENTAGE")
            ShowroomIncentive()
            Exit Sub
        End If
        If rbtAll.Checked Then
            GroupingGridbyAll()
            Exit Sub
        End If
        If chkmnthwise.Checked Then
            If insper = 0 Then MsgBox("Incentive percentage should not be Zero." & vbCrLf & "Pls update softcontrol Id is INCENTIVEPERCENTAGE")
            GroupingGridbymonthwise()
        Else
            GroupingGridbydefault()
        End If
    End Sub

    Private Sub GroupingGridbyAll()
        Dim grouper As String = ""
        Dim particular As String = ""
        Dim StoneDetail As Boolean = chkStoneDetails.Checked
        If rbtGrpEmployee.Checked Then
            grouper = "EMPNAME"
            particular = "ITEM"
        ElseIf rbtGrpCounter.Checked Then
            grouper = "ITEMCTRNAME"
            particular = "ITEM"
        ElseIf rbtproduct.Checked Then
            grouper = "ITEM"
            particular = "EMPNAME"
        ElseIf rbtMetal.Checked Then
            grouper = "METAL"
            particular = "EMPNAME"
        Else
            grouper = "ITEMCTRNAME"
            particular = "EMPNAME"
        End If
        StrSql = vbCrLf + " SELECT PARTICULAR,COMMISION"
        StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),CASE WHEN (((CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SPCS,0) ELSE ISNULL(SAMOUNT,0)END)-(CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(RPCS,0) ELSE ISNULL(RAMOUNT,0)END)/CASE WHEN EI.WEIGHT=0 THEN 1 ELSE EI.WEIGHT END)*100) BETWEEN 90 AND 99 THEN (((ISNULL(SABACKAMT,0)-ISNULL(SRBACKAMT,0))/100)*" & insper & ") "
        StrSql += vbCrLf + " WHEN (((CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SPCS,0) ELSE ISNULL(SAMOUNT,0)END)-(CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(RPCS,0) ELSE ISNULL(RAMOUNT,0)END)/CASE WHEN EI.WEIGHT=0 THEN 1 ELSE EI.WEIGHT END)*100) >99 THEN ISNULL(SABACKAMT,0)-ISNULL(SRBACKAMT,0) ELSE 0 END) BACKINC"
        StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),CASE WHEN (((CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SPCS,0) ELSE ISNULL(SAMOUNT,0)END-(CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(RPCS,0) ELSE ISNULL(RAMOUNT,0)END))/CASE WHEN EI.WEIGHT=0 THEN 1 ELSE EI.WEIGHT END)*100) BETWEEN 90 AND 99 THEN (((ISNULL(SALESCOMM,0)-ISNULL(SALESCOMM,0))/100)*" & insper & ")+(ISNULL(SABACKAMT,0)-ISNULL(SRBACKAMT,0)) "
        StrSql += vbCrLf + " WHEN (((CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SPCS,0) ELSE ISNULL(SAMOUNT,0)END-(CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(RPCS,0) ELSE ISNULL(RAMOUNT,0)END))/CASE WHEN EI.WEIGHT=0 THEN 1 ELSE EI.WEIGHT END)*100) >99 THEN ISNULL(SALESCOMM,0)-ISNULL(SALESCOMM,0)+(ISNULL(SABACKAMT,0)-ISNULL(SRBACKAMT,0)) ELSE 0 END) BACKCOMM"
        StrSql += vbCrLf + " ,SALESCOMM,OTHERCOMM"
        StrSql += vbCrLf + " ,SUBITEMNAME,TRANNO,TRANDATE,TAGNO " & IIf(chkWithTagno.Checked, ",0 TAGNOS", "") & ""
        StrSql += vbCrLf + " ,SPCS,SGRSWT,SNETWT"
        If StoneDetail = True Then
            StrSql += vbCrLf + " ,SSTNPCS,SSTNWT_G,SSTNWT_C,SDIAPCS,SDIAWT_C"
        End If
        StrSql += vbCrLf + " ,SAMOUNT,RPCS,RGRSWT,RNETWT"
        If StoneDetail = True Then
            StrSql += vbCrLf + " ,RSTNPCS,RSTNWT_G,RSTNWT_C,RDIAPCS,RDIAWT_C"
        End If
        StrSql += vbCrLf + " ,RAMOUNT,EMPNAME,COUNTER"
        StrSql += vbCrLf + " ,SEP,COLHEAD,RESULT--,EMPID,COSTID,MNTH"
        StrSql += vbCrLf + " ,GROUPER,ITEM,METAL"
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & ""
        StrSql += vbCrLf + " FROM ("
        StrSql += vbCrLf + " SELECT CONVERT(VARCHAR(300)," & particular & ")PARTICULAR"
        StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' AND COMMISION <> 0 THEN -1*COMMISION WHEN SEP = 'SA' AND COMMISION > 0 THEN COMMISION ELSE NULL END AS COMMISION"
        StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' AND SALESCOMM <> 0 THEN -1*SALESCOMM WHEN SEP = 'SA' AND SALESCOMM > 0 THEN SALESCOMM ELSE NULL END AS SALESCOMM"
        StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' AND OTHERCOMM <> 0 THEN -1*OTHERCOMM WHEN SEP = 'SA' AND OTHERCOMM > 0 THEN OTHERCOMM ELSE NULL END AS OTHERCOMM"
        StrSql += vbCrLf + " ,SUBITEMNAME,TRANNO,TRANDATE,TAGNO " & IIf(chkWithTagno.Checked, ",0 TAGNOS", "") & ""
        StrSql += vbCrLf + " ,CASE WHEN SEP = 'SA' THEN PCS ELSE NULL END AS SPCS"
        StrSql += vbCrLf + " ,CASE WHEN SEP = 'SA' THEN GRSWT ELSE NULL END AS SGRSWT"
        StrSql += vbCrLf + " ,CASE WHEN SEP = 'SA' THEN NETWT ELSE NULL END AS SNETWT"
        If StoneDetail = True Then
            StrSql += vbCrLf + " ,CASE WHEN SEP = 'SA' THEN STNPCS ELSE NULL END AS SSTNPCS"
            StrSql += vbCrLf + " ,CASE WHEN SEP = 'SA' THEN STNWT_G ELSE NULL END AS SSTNWT_G"
            StrSql += vbCrLf + " ,CASE WHEN SEP = 'SA' THEN STNWT_C ELSE NULL END AS SSTNWT_C"
            StrSql += vbCrLf + " ,CASE WHEN SEP = 'SA' THEN DIAPCS ELSE NULL END AS SDIAPCS"
            StrSql += vbCrLf + " ,CASE WHEN SEP = 'SA' THEN DIAWT_C ELSE NULL END AS SDIAWT_C"
        End If
        StrSql += vbCrLf + " ,CASE WHEN SEP = 'SA' THEN S.AMOUNT ELSE NULL END AS SAMOUNT"
        StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' THEN PCS ELSE NULL END AS RPCS"
        StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' THEN GRSWT ELSE NULL END AS RGRSWT"
        StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' THEN NETWT ELSE NULL END AS RNETWT"
        If StoneDetail = True Then
            StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' THEN STNPCS ELSE NULL END AS RSTNPCS"
            StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' THEN STNWT_G ELSE NULL END AS RSTNWT_G"
            StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' THEN STNWT_C ELSE NULL END AS RSTNWT_C"
            StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' THEN DIAPCS ELSE NULL END AS RDIAPCS"
            StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' THEN DIAWT_C ELSE NULL END AS RDIAWT_C"
        End If
        StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' THEN S.AMOUNT ELSE NULL END AS RAMOUNT"
        StrSql += vbCrLf + " ,CASE WHEN SEP='SA' THEN (ISNULL(BI.AMOUNT,0)*(CASE WHEN ISNULL(S.METAL,'') IN('OTHERS','PLATINUM') THEN S.PCS ELSE S.NETWT END)) ELSE 0 END SABACKAMT "
        StrSql += vbCrLf + " ,CASE WHEN SEP='SR' THEN (ISNULL(BI.AMOUNT,0)*(CASE WHEN ISNULL(S.METAL,'') IN('OTHERS','PLATINUM') THEN S.PCS ELSE S.NETWT END)) ELSE 0 END SRBACKAMT"
        StrSql += vbCrLf + " ,CONVERT(VARCHAR(100),EMPNAME)AS EMPNAME,CONVERT(VARCHAR(100),ITEMCTRNAME) AS COUNTER"
        StrSql += vbCrLf + " ,CONVERT(VARCHAR(2),SEP)SEP,CONVERT(VARCHAR(3),'')AS COLHEAD,CONVERT(INT,1)AS RESULT"
        StrSql += vbCrLf + " ,CONVERT(VARCHAR(200)," & grouper & ")AS GROUPER,ITEM,METAL"
        StrSql += vbCrLf + " ,EMPID,S.COSTID"
        StrSql += vbCrLf + " ,CONVERT(VARCHAR(15),'2013-'+ SUBSTRING(DATENAME(MONTH,TRANDATE),0,4)+'-01')MNTH"
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " S"
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..BACKENDINCENTIVE AS BI ON BI.ITEMID=S.ITEMID AND DATENAME(MONTH,S.TRANDATE)=BI.MONTH "
        StrSql += vbCrLf + " )X"
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..EMPWISEINCENTIVE AS EI ON EI.EMPID=X.EMPID AND EI.COSTID=X.COSTID AND EI.METALID=(SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME=X.METAL) "
        StrSql += vbCrLf + " AND EI.MONTH=DATENAME(MONTH,X.MNTH)"
        If chkWithEmptyCommision.Checked = False Then StrSql += vbCrLf + " WHERE ISNULL(COMMISION,0) <> 0"

        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        If cmbViewBy.Text.Trim = "COUNTER,METAL,ITEM,SUB ITEM WISE SUMMARY" Then
            StrSql = "  IF OBJECT_ID('TEMPTABLEDB..TEMPSALESCOMMISION_RES_ALL')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPSALESCOMMISION_RES_ALL"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
            StrSql = "  SELECT * INTO TEMPTABLEDB..TEMPSALESCOMMISION_RES_ALL FROM ("
            StrSql += vbCrLf + "   SELECT DISTINCT EMPNAME AS PARTICULAR,EMPNAME,CONVERT(VARCHAR(200),'') COUNTER,"
            StrSql += vbCrLf + "   CONVERT(VARCHAR(200),'') METAL,CONVERT(VARCHAR(200),'') ITEM,CONVERT(VARCHAR(200),'') SUBITEMNAME,"
            StrSql += vbCrLf + "   NULL COMMISION,NULL SALESCOMM,NULL BACKINC,"
            StrSql += vbCrLf + "   NULL BACKCOMM,NULL OTHERCOMM,NULL SPCS,"
            StrSql += vbCrLf + "   NULL SGRSWT,NULL SNETWT,NULL RPCS,"
            StrSql += vbCrLf + "   NULL RGRSWT,NULL RNETWT,0 AS RESULT,'T' COLHEAD "
            StrSql += vbCrLf + "   FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " S WHERE RESULT = 1 GROUP BY EMPNAME"
            StrSql += vbCrLf + "  UNION ALL"
            StrSql += vbCrLf + "   SELECT '    ' + COUNTER AS PARTICULAR,EMPNAME,COUNTER,"
            StrSql += vbCrLf + "   CONVERT(VARCHAR(200),'') METAL,CONVERT(VARCHAR(200),'') ITEM,CONVERT(VARCHAR(200),'') SUBITEMNAME,"
            StrSql += vbCrLf + "   NULL COMMISION,NULL SALESCOMM,NULL BACKINC,"
            StrSql += vbCrLf + "   NULL BACKCOMM,NULL OTHERCOMM,NULL SPCS,"
            StrSql += vbCrLf + "   NULL SGRSWT,NULL SNETWT,NULL RPCS,"
            StrSql += vbCrLf + "   NULL RGRSWT,NULL RNETWT,1 AS RESULT,'T1' COLHEAD "
            StrSql += vbCrLf + "   FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " S WHERE RESULT = 1 GROUP BY EMPNAME,COUNTER"
            StrSql += vbCrLf + "   UNION ALL"
            StrSql += vbCrLf + "   SELECT '        ' + METAL AS PARTICULAR,EMPNAME,COUNTER,METAL,ITEM " & IIf(cmbViewBy.Text.Trim = "COUNTER,METAL,ITEM,SUB ITEM WISE SUMMARY", ",SUBITEMNAME ", ",''")
            StrSql += vbCrLf + "   ,SUM(COMMISION)COMMISION,SUM(SALESCOMM)SALESCOMM,SUM(BACKINC)BACKINC,SUM(BACKCOMM)BACKCOMM"
            StrSql += vbCrLf + "   ,SUM(OTHERCOMM)OTHERCOMM,SUM(SPCS)SPCS,SUM(SGRSWT)SGRSWT,SUM(SNETWT)SNETWT"
            StrSql += vbCrLf + "   ,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,2 AS RESULT,'X' COLHEAD"
            StrSql += vbCrLf + "   FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " S WHERE RESULT = 1 GROUP BY EMPNAME,COUNTER,METAL,ITEM" & IIf(cmbViewBy.Text.Trim = "COUNTER,METAL,ITEM,SUB ITEM WISE SUMMARY", ",SUBITEMNAME ", "")
            StrSql += vbCrLf + "   UNION ALL"
            StrSql += vbCrLf + "   SELECT '    SUB TOTAL '+COUNTER AS PARTICULAR,EMPNAME,COUNTER,"
            StrSql += vbCrLf + "   CONVERT(VARCHAR(200),'') METAL,CONVERT(VARCHAR(200),'') ITEM,CONVERT(VARCHAR(200),'') SUBITEMNAME"
            StrSql += vbCrLf + "   ,SUM(COMMISION)COMMISION,SUM(SALESCOMM)SALESCOMM,SUM(BACKINC)BACKINC,SUM(BACKCOMM)BACKCOMM"
            StrSql += vbCrLf + "   ,SUM(OTHERCOMM)OTHERCOMM,SUM(SPCS)SPCS,SUM(SGRSWT)SGRSWT,SUM(SNETWT)SNETWT"
            StrSql += vbCrLf + "   ,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,5 AS RESULT,'S' COLHEAD"
            StrSql += vbCrLf + "   FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " S WHERE RESULT = 1 GROUP BY EMPNAME,COUNTER"
            StrSql += vbCrLf + "   UNION ALL"
            StrSql += vbCrLf + "   SELECT 'TOTAL '+EMPNAME AS PARTICULAR, EMPNAME,CONVERT(VARCHAR(200),'ZZZZZ') COUNTER, "
            StrSql += vbCrLf + "   CONVERT(VARCHAR(200),'') METAL,CONVERT(VARCHAR(200),'') ITEM,CONVERT(VARCHAR(200),'') SUBITEMNAME"
            StrSql += vbCrLf + "   ,SUM(COMMISION)COMMISION,SUM(SALESCOMM)SALESCOMM,SUM(BACKINC)BACKINC,SUM(BACKCOMM)BACKCOMM"
            StrSql += vbCrLf + "   ,SUM(OTHERCOMM)OTHERCOMM,SUM(SPCS)SPCS,SUM(SGRSWT)SGRSWT,SUM(SNETWT)SNETWT"
            StrSql += vbCrLf + "   ,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,5 AS RESULT,'S2' COLHEAD"
            StrSql += vbCrLf + "   FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " S WHERE RESULT = 1 GROUP BY EMPNAME"
            StrSql += vbCrLf + "    UNION ALL"
            StrSql += vbCrLf + "   SELECT 'GRAND TOTAL ' AS PARTICULAR,'ZZZZZ' EMPNAME,CONVERT(VARCHAR(200),'ZZZZZ') COUNTER, "
            StrSql += vbCrLf + "   CONVERT(VARCHAR(200),'') METAL,CONVERT(VARCHAR(200),'') ITEM,CONVERT(VARCHAR(200),'') SUBITEMNAME"
            StrSql += vbCrLf + "   ,SUM(COMMISION)COMMISION,SUM(SALESCOMM)SALESCOMM,SUM(BACKINC)BACKINC,SUM(BACKCOMM)BACKCOMM"
            StrSql += vbCrLf + "   ,SUM(OTHERCOMM)OTHERCOMM,SUM(SPCS)SPCS,SUM(SGRSWT)SGRSWT,SUM(SNETWT)SNETWT"
            StrSql += vbCrLf + "   ,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,6 AS RESULT,'G' COLHEAD"
            StrSql += vbCrLf + "   FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " S WHERE RESULT = 1 "
            StrSql += vbCrLf + "   )X"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
            StrSql = "   SELECT * FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES_ALL ORDER BY EMPNAME,COUNTER,RESULT,METAL,ITEM,SUBITEMNAME"
        ElseIf cmbViewBy.Text.Trim = "COUNTER WISE SUMMARY" Then
            StrSql = vbCrLf + "   SELECT * FROM ("
            StrSql += vbCrLf + "    SELECT DISTINCT EMPNAME AS PARTICULAR,EMPNAME,CONVERT(VARCHAR(200),'') COUNTER,"
            StrSql += vbCrLf + "    CONVERT(VARCHAR(200),'') METAL,CONVERT(VARCHAR(200),'') ITEM,CONVERT(VARCHAR(200),'') SUBITEMNAME,"
            StrSql += vbCrLf + "    NULL COMMISION,NULL SALESCOMM,NULL BACKINC,"
            StrSql += vbCrLf + "    NULL BACKCOMM,NULL OTHERCOMM,NULL SPCS,"
            StrSql += vbCrLf + "    NULL SGRSWT,NULL SNETWT,NULL RPCS,"
            StrSql += vbCrLf + "    NULL RGRSWT,NULL RNETWT,0 AS RESULT,'T' COLHEAD "
            StrSql += vbCrLf + "    FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " S WHERE RESULT = 1 GROUP BY EMPNAME"
            StrSql += vbCrLf + "    UNION ALL"
            StrSql += vbCrLf + "    SELECT '    '+COUNTER AS PARTICULAR,EMPNAME,COUNTER,"
            StrSql += vbCrLf + "    CONVERT(VARCHAR(200),'') METAL,CONVERT(VARCHAR(200),'') ITEM,CONVERT(VARCHAR(200),'') SUBITEMNAME"
            StrSql += vbCrLf + "    ,SUM(COMMISION)COMMISION,SUM(SALESCOMM)SALESCOMM,SUM(BACKINC)BACKINC,SUM(BACKCOMM)BACKCOMM"
            StrSql += vbCrLf + "    ,SUM(OTHERCOMM)OTHERCOMM,SUM(SPCS)SPCS,SUM(SGRSWT)SGRSWT,SUM(SNETWT)SNETWT"
            StrSql += vbCrLf + "    ,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,5 AS RESULT,'X' COLHEAD"
            StrSql += vbCrLf + "    FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " S WHERE RESULT = 1 GROUP BY EMPNAME,COUNTER"
            StrSql += vbCrLf + "    UNION ALL"
            StrSql += vbCrLf + "    SELECT 'TOTAL '+EMPNAME AS PARTICULAR, EMPNAME,CONVERT(VARCHAR(200),'ZZZZZ') COUNTER, "
            StrSql += vbCrLf + "    CONVERT(VARCHAR(200),'') METAL,CONVERT(VARCHAR(200),'') ITEM,CONVERT(VARCHAR(200),'') SUBITEMNAME"
            StrSql += vbCrLf + "    ,SUM(COMMISION)COMMISION,SUM(SALESCOMM)SALESCOMM,SUM(BACKINC)BACKINC,SUM(BACKCOMM)BACKCOMM"
            StrSql += vbCrLf + "    ,SUM(OTHERCOMM)OTHERCOMM,SUM(SPCS)SPCS,SUM(SGRSWT)SGRSWT,SUM(SNETWT)SNETWT"
            StrSql += vbCrLf + "    ,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,5 AS RESULT,'S2' COLHEAD"
            StrSql += vbCrLf + "    FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " S WHERE RESULT = 1 GROUP BY EMPNAME"
            StrSql += vbCrLf + "     UNION ALL"
            StrSql += vbCrLf + "    SELECT 'GRAND TOTAL ' AS PARTICULAR,'ZZZZZ' EMPNAME,CONVERT(VARCHAR(200),'ZZZZZ') COUNTER, "
            StrSql += vbCrLf + "    CONVERT(VARCHAR(200),'') METAL,CONVERT(VARCHAR(200),'') ITEM,CONVERT(VARCHAR(200),'') SUBITEMNAME"
            StrSql += vbCrLf + "    ,SUM(COMMISION)COMMISION,SUM(SALESCOMM)SALESCOMM,SUM(BACKINC)BACKINC,SUM(BACKCOMM)BACKCOMM"
            StrSql += vbCrLf + "    ,SUM(OTHERCOMM)OTHERCOMM,SUM(SPCS)SPCS,SUM(SGRSWT)SGRSWT,SUM(SNETWT)SNETWT"
            StrSql += vbCrLf + "    ,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,6 AS RESULT,'G' COLHEAD"
            StrSql += vbCrLf + "    FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " S WHERE RESULT = 1 "
            StrSql += vbCrLf + "    )X ORDER BY EMPNAME,COUNTER,RESULT,METAL,ITEM,SUBITEMNAME"
        ElseIf cmbViewBy.Text.Trim = "TRANDATE WISE SUMMARY" Then
            StrSql = vbCrLf + "   SELECT * FROM ("
            StrSql += vbCrLf + "    SELECT DISTINCT EMPNAME AS PARTICULAR,EMPNAME,CONVERT(VARCHAR(200),'') COUNTER,"
            StrSql += vbCrLf + "    CONVERT(VARCHAR(200),'') METAL,CONVERT(VARCHAR(200),'') ITEM,CONVERT(VARCHAR(200),'') SUBITEMNAME,"
            StrSql += vbCrLf + "    NULL COMMISION,NULL SALESCOMM,NULL BACKINC,"
            StrSql += vbCrLf + "    NULL BACKCOMM,NULL OTHERCOMM,NULL SPCS,"
            StrSql += vbCrLf + "    NULL SGRSWT,NULL SNETWT,NULL RPCS,"
            StrSql += vbCrLf + "    NULL RGRSWT,NULL RNETWT,NULL TRANDATE,0 AS RESULT,'T' COLHEAD "
            StrSql += vbCrLf + "    FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " S WHERE RESULT = 1 GROUP BY EMPNAME"
            StrSql += vbCrLf + "   UNION ALL"
            StrSql += vbCrLf + "    SELECT '        ' + CONVERT(VARCHAR(12),TRANDATE,103) AS PARTICULAR,EMPNAME,'' COUNTER,'' METAL,'' ITEM,CONVERT(VARCHAR(200),'') SUBITEMNAME "
            StrSql += vbCrLf + "    ,SUM(COMMISION)COMMISION,SUM(SALESCOMM)SALESCOMM,SUM(BACKINC)BACKINC,SUM(BACKCOMM)BACKCOMM"
            StrSql += vbCrLf + "    ,SUM(OTHERCOMM)OTHERCOMM,SUM(SPCS)SPCS,SUM(SGRSWT)SGRSWT,SUM(SNETWT)SNETWT"
            StrSql += vbCrLf + "    ,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,TRANDATE,2 AS RESULT,'X' COLHEAD"
            StrSql += vbCrLf + "    FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " S WHERE RESULT = 1 GROUP BY EMPNAME,TRANDATE"
            StrSql += vbCrLf + "    UNION ALL"
            StrSql += vbCrLf + "    SELECT 'TOTAL '+EMPNAME AS PARTICULAR, EMPNAME,CONVERT(VARCHAR(200),'ZZZZZ') COUNTER, "
            StrSql += vbCrLf + "    CONVERT(VARCHAR(200),'') METAL,CONVERT(VARCHAR(200),'') ITEM,CONVERT(VARCHAR(200),'') SUBITEMNAME"
            StrSql += vbCrLf + "    ,SUM(COMMISION)COMMISION,SUM(SALESCOMM)SALESCOMM,SUM(BACKINC)BACKINC,SUM(BACKCOMM)BACKCOMM"
            StrSql += vbCrLf + "    ,SUM(OTHERCOMM)OTHERCOMM,SUM(SPCS)SPCS,SUM(SGRSWT)SGRSWT,SUM(SNETWT)SNETWT"
            StrSql += vbCrLf + "    ,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,NULL TRANDATE,5 AS RESULT,'S2' COLHEAD"
            StrSql += vbCrLf + "    FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " S WHERE RESULT = 1 GROUP BY EMPNAME"
            StrSql += vbCrLf + "     UNION ALL"
            StrSql += vbCrLf + "    SELECT 'GRAND TOTAL ' AS PARTICULAR,'ZZZZZ' EMPNAME,CONVERT(VARCHAR(200),'ZZZZZ') COUNTER, "
            StrSql += vbCrLf + "    CONVERT(VARCHAR(200),'') METAL,CONVERT(VARCHAR(200),'') ITEM,CONVERT(VARCHAR(200),'') SUBITEMNAME"
            StrSql += vbCrLf + "    ,SUM(COMMISION)COMMISION,SUM(SALESCOMM)SALESCOMM,SUM(BACKINC)BACKINC,SUM(BACKCOMM)BACKCOMM"
            StrSql += vbCrLf + "    ,SUM(OTHERCOMM)OTHERCOMM,SUM(SPCS)SPCS,SUM(SGRSWT)SGRSWT,SUM(SNETWT)SNETWT"
            StrSql += vbCrLf + "    ,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,NULL TRANDATE,6 AS RESULT,'G' COLHEAD"
            StrSql += vbCrLf + "    FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " S WHERE RESULT = 1 "
            StrSql += vbCrLf + "    )X ORDER BY EMPNAME,COUNTER,RESULT,TRANDATE"
        ElseIf cmbViewBy.Text.Trim = "ITEM,SUB ITEM WISE SUMMARY" Then
            StrSql = vbCrLf + "   SELECT * FROM ("
            StrSql += vbCrLf + "    SELECT DISTINCT EMPNAME AS PARTICULAR,EMPNAME,CONVERT(VARCHAR(200),'') COUNTER,"
            StrSql += vbCrLf + "    CONVERT(VARCHAR(200),'') METAL,CONVERT(VARCHAR(200),'') ITEM,CONVERT(VARCHAR(200),'') SUBITEMNAME,"
            StrSql += vbCrLf + "    NULL COMMISION,NULL SALESCOMM,NULL BACKINC,"
            StrSql += vbCrLf + "    NULL BACKCOMM,NULL OTHERCOMM,NULL SPCS,"
            StrSql += vbCrLf + "    NULL SGRSWT,NULL SNETWT,NULL RPCS,"
            StrSql += vbCrLf + "    NULL RGRSWT,NULL RNETWT,0 AS RESULT,'T' COLHEAD "
            StrSql += vbCrLf + "    FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " S WHERE RESULT = 1 GROUP BY EMPNAME   "
            StrSql += vbCrLf + "    UNION ALL"
            StrSql += vbCrLf + "    SELECT ITEM AS PARTICULAR,EMPNAME,'' COUNTER,'' METAL,ITEM,'' SUBITEMNAME "
            StrSql += vbCrLf + "    ,NULL COMMISION,NULL SALESCOMM,NULL BACKINC,NULL BACKCOMM"
            StrSql += vbCrLf + "    ,NULL OTHERCOMM,NULL SPCS,NULL SGRSWT,NULL SNETWT"
            StrSql += vbCrLf + "    ,NULL RPCS,NULL RGRSWT,NULL RNETWT,2 AS RESULT,'T1' COLHEAD"
            StrSql += vbCrLf + "    FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " S WHERE RESULT = 1 GROUP BY EMPNAME,ITEM   "
            StrSql += vbCrLf + "    UNION ALL"
            StrSql += vbCrLf + "    SELECT (CASE WHEN SUBITEMNAME <> '' THEN '     ' + SUBITEMNAME ELSE '     NO SUB ITEM' END) AS PARTICULAR"
            StrSql += vbCrLf + "    ,EMPNAME,COUNTER,METAL,ITEM,(CASE WHEN SUBITEMNAME <> '' THEN SUBITEMNAME ELSE 'NO SUB ITEM' END) SUBITEMNAME "
            StrSql += vbCrLf + "    ,SUM(COMMISION)COMMISION,SUM(SALESCOMM)SALESCOMM,SUM(BACKINC)BACKINC,SUM(BACKCOMM)BACKCOMM"
            StrSql += vbCrLf + "    ,SUM(OTHERCOMM)OTHERCOMM,SUM(SPCS)SPCS,SUM(SGRSWT)SGRSWT,SUM(SNETWT)SNETWT"
            StrSql += vbCrLf + "    ,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,2 AS RESULT,'X' COLHEAD"
            StrSql += vbCrLf + "    FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " S WHERE RESULT = 1 GROUP BY EMPNAME,ITEM,SUBITEMNAME,COUNTER,METAL"

            StrSql += vbCrLf + "    UNION ALL"
            StrSql += vbCrLf + "    SELECT ITEM AS PARTICULAR,EMPNAME,'' COUNTER,'' METAL,ITEM,'ZZZZZ' SUBITEMNAME "
            StrSql += vbCrLf + "    ,SUM(COMMISION)COMMISION,SUM(SALESCOMM)SALESCOMM,SUM(BACKINC)BACKINC,SUM(BACKCOMM)BACKCOMM"
            StrSql += vbCrLf + "    ,SUM(OTHERCOMM)OTHERCOMM,SUM(SPCS)SPCS,SUM(SGRSWT)SGRSWT,SUM(SNETWT)SNETWT"
            StrSql += vbCrLf + "    ,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,2 AS RESULT,'S1' COLHEAD"
            StrSql += vbCrLf + "    FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " S WHERE RESULT = 1 GROUP BY EMPNAME,ITEM   "
            StrSql += vbCrLf + "    UNION ALL"
            StrSql += vbCrLf + "    SELECT 'TOTAL '+EMPNAME AS PARTICULAR, EMPNAME,CONVERT(VARCHAR(200),'ZZZZZ') COUNTER, "
            StrSql += vbCrLf + "    CONVERT(VARCHAR(200),'') METAL,CONVERT(VARCHAR(200),'') ITEM,CONVERT(VARCHAR(200),'') SUBITEMNAME"
            StrSql += vbCrLf + "    ,SUM(COMMISION)COMMISION,SUM(SALESCOMM)SALESCOMM,SUM(BACKINC)BACKINC,SUM(BACKCOMM)BACKCOMM"
            StrSql += vbCrLf + "    ,SUM(OTHERCOMM)OTHERCOMM,SUM(SPCS)SPCS,SUM(SGRSWT)SGRSWT,SUM(SNETWT)SNETWT"
            StrSql += vbCrLf + "    ,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,5 AS RESULT,'S2' COLHEAD"
            StrSql += vbCrLf + "    FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " S WHERE RESULT = 1 GROUP BY EMPNAME"
            StrSql += vbCrLf + "     UNION ALL"
            StrSql += vbCrLf + "    SELECT 'GRAND TOTAL ' AS PARTICULAR,'ZZZZZ' EMPNAME,CONVERT(VARCHAR(200),'ZZZZZ') COUNTER, "
            StrSql += vbCrLf + "    CONVERT(VARCHAR(200),'') METAL,CONVERT(VARCHAR(200),'') ITEM,CONVERT(VARCHAR(200),'') SUBITEMNAME"
            StrSql += vbCrLf + "    ,SUM(COMMISION)COMMISION,SUM(SALESCOMM)SALESCOMM,SUM(BACKINC)BACKINC,SUM(BACKCOMM)BACKCOMM"
            StrSql += vbCrLf + "    ,SUM(OTHERCOMM)OTHERCOMM,SUM(SPCS)SPCS,SUM(SGRSWT)SGRSWT,SUM(SNETWT)SNETWT"
            StrSql += vbCrLf + "    ,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,6 AS RESULT,'G' COLHEAD"
            StrSql += vbCrLf + "    FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " S WHERE RESULT = 1 "
            StrSql += vbCrLf + "    )X ORDER BY EMPNAME,RESULT,ITEM,SUBITEMNAME"
        ElseIf cmbViewBy.Text = "TRANDATE,EMPLOYEE WISE SUMMARY" Then

            StrSql = vbCrLf + "    /*STEP 1*/"
            StrSql += vbCrLf + "    DECLARE @COL AS VARCHAR(100)"
            StrSql += vbCrLf + "    DECLARE @QRY AS VARCHAR(8000)"
            StrSql += vbCrLf + "    DECLARE CUR CURSOR FOR SELECT DISTINCT EMPNAME FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " ORDER BY EMPNAME"
            StrSql += vbCrLf + "    OPEN CUR "
            StrSql += vbCrLf + "    FETCH NEXT FROM CUR INTO @COL"
            StrSql += vbCrLf + "    IF (SELECT OBJECT_ID('TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP')) > 0 DROP TABLE TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP"
            StrSql += vbCrLf + "    SET @QRY = 'CREATE TABLE TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP (TRANDATE SMALLDATETIME '"
            StrSql += vbCrLf + "    WHILE @@FETCH_STATUS =0"
            StrSql += vbCrLf + "    BEGIN"
            StrSql += vbCrLf + "    SET @QRY = @QRY + ',[' + @COL + '] [NUMERIC](15,2) NULL' + CHAR(13)"
            StrSql += vbCrLf + "    FETCH NEXT FROM CUR INTO @COL "
            StrSql += vbCrLf + "    END "
            StrSql += vbCrLf + "    SET @QRY = @QRY + ' ,TOTCOMM [NUMERIC](15,2) NULL )'"
            StrSql += vbCrLf + "    CLOSE CUR "
            StrSql += vbCrLf + "    DEALLOCATE CUR "
            StrSql += vbCrLf + "    PRINT @QRY"
            StrSql += vbCrLf + "    EXEC (@QRY)"


            'StrSql = vbCrLf + "    /*STEP 1*/"
            'StrSql += vbCrLf + "        DECLARE @COL AS VARCHAR(100)"
            'StrSql += vbCrLf + "        DECLARE @QRY AS VARCHAR(8000)"
            'StrSql += vbCrLf + "        DECLARE @DESIGID AS INT"
            'StrSql += vbCrLf + "        DECLARE CUR CURSOR FOR     "
            'StrSql += vbCrLf + "        SELECT DISTINCT A.EMPNAME,C.DESIGID"
            'StrSql += vbCrLf + "        FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " AS A," & cnAdminDb & "..EMPMASTER AS B"
            'StrSql += vbCrLf + "        LEFT JOIN " & cnAdminDb & "..DESIGNATION AS C ON B.DESIGNATIONID=C.DESIGID"
            'StrSql += vbCrLf + "        WHERE A.EMPNAME=B.EMPNAME"
            'StrSql += vbCrLf + "        ORDER BY C.DESIGID,A.EMPNAME    "
            'StrSql += vbCrLf + "        OPEN CUR "
            'StrSql += vbCrLf + "        FETCH NEXT FROM CUR INTO @COL,@DESIGID"
            'StrSql += vbCrLf + "        IF (SELECT OBJECT_ID('TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP')) > 0 DROP TABLE TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP"
            'StrSql += vbCrLf + "        SET @QRY = 'CREATE TABLE TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP (TRANDATE SMALLDATETIME '"
            'StrSql += vbCrLf + "        WHILE @@FETCH_STATUS =0"
            'StrSql += vbCrLf + "        BEGIN"
            'StrSql += vbCrLf + "        SET @QRY = @QRY + ',[' + @COL + '] [NUMERIC](15,2) NULL' + CHAR(13)"
            ''StrSql += vbCrLf + "        SET @QRY = @QRY + ',[' + @COL + '-X' + CONVERT(VARCHAR,@DESIGID) + '] [NUMERIC](15,2) NULL' + CHAR(13)"
            'StrSql += vbCrLf + "        FETCH NEXT FROM CUR INTO @COL,@DESIGID"
            'StrSql += vbCrLf + "        END "
            'StrSql += vbCrLf + "        SET @QRY = @QRY + ' ,TOTCOMM [NUMERIC](15,2) NULL )'"
            'StrSql += vbCrLf + "        CLOSE CUR "
            'StrSql += vbCrLf + "        DEALLOCATE CUR "
            'StrSql += vbCrLf + "        PRINT @QRY"
            'StrSql += vbCrLf + "        EXEC (@QRY)"
            Cmd = New OleDbCommand(StrSql, cn)
            Cmd.ExecuteNonQuery()

            StrSql = vbCrLf + "    /*STEP 2*/"
            StrSql += vbCrLf + "    DECLARE @FROMDATE SMALLDATETIME,@TODATE SMALLDATETIME"
            StrSql += vbCrLf + "    SET @FROMDATE='" & dtpFrom.Value.AddDays(-1).Date.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + "    SET @TODATE='" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + "    INSERT INTO TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP(TRANDATE)"
            StrSql += vbCrLf + "    SELECT DATEADD(DAY,NUMBER+1,@FROMDATE)DATE"
            StrSql += vbCrLf + "    FROM master..spt_values WHERE type='P' "
            StrSql += vbCrLf + "    AND DATEADD(DAY,NUMBER+1,@FROMDATE)<=@TODATE  "
            Cmd = New OleDbCommand(StrSql, cn)
            Cmd.ExecuteNonQuery()

            StrSql = vbCrLf + "    /*STEP 3*/"
            StrSql += vbCrLf + "    DECLARE @COL3 AS VARCHAR(100)"
            StrSql += vbCrLf + "    DECLARE @QRY3 AS VARCHAR(8000)"
            StrSql += vbCrLf + "    DECLARE CUR CURSOR FOR SELECT DISTINCT EMPNAME FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " ORDER BY EMPNAME"
            StrSql += vbCrLf + "    OPEN CUR "
            StrSql += vbCrLf + "    FETCH NEXT FROM CUR INTO @COL3"
            StrSql += vbCrLf + "    SET @QRY3 = ' INSERT INTO TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP'"
            StrSql += vbCrLf + "    SET @QRY3 = @QRY3 + ' SELECT ''2050-01-01'' TRANDATE '"
            StrSql += vbCrLf + "    WHILE @@FETCH_STATUS =0"
            StrSql += vbCrLf + "    BEGIN"
            StrSql += vbCrLf + "    SET @QRY3 = @QRY3 + ',NULL [' + @COL3 + '] ' + CHAR(13)"
            StrSql += vbCrLf + "    FETCH NEXT FROM CUR INTO @COL3 "
            StrSql += vbCrLf + "    END "
            StrSql += vbCrLf + "    SET @QRY3 = @QRY3 + ' ,NULL TOTCOMM  '"
            StrSql += vbCrLf + "    CLOSE CUR "
            StrSql += vbCrLf + "    DEALLOCATE CUR "
            StrSql += vbCrLf + "    PRINT @QRY3"
            StrSql += vbCrLf + "    EXEC (@QRY3)"
            Cmd = New OleDbCommand(StrSql, cn)
            Cmd.ExecuteNonQuery()

            StrSql = vbCrLf + "    /*STEP 4*/"
            StrSql += vbCrLf + "    DECLARE @COL2 AS VARCHAR(100)"
            StrSql += vbCrLf + "    DECLARE @TRANDATE AS VARCHAR(12)"
            StrSql += vbCrLf + "    DECLARE @QRY2 AS VARCHAR(8000)"
            StrSql += vbCrLf + "    DECLARE CUR CURSOR FOR SELECT DISTINCT EMPNAME FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " ORDER BY EMPNAME"
            StrSql += vbCrLf + "    OPEN CUR "
            StrSql += vbCrLf + "    FETCH NEXT FROM CUR INTO @COL2"
            StrSql += vbCrLf + "    WHILE @@FETCH_STATUS =0"
            StrSql += vbCrLf + "    BEGIN"
            StrSql += vbCrLf + "    DECLARE CUR2 CURSOR FOR SELECT DISTINCT CONVERT(VARCHAR(12),TRANDATE,112) FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " "
            StrSql += vbCrLf + "    OPEN CUR2"
            StrSql += vbCrLf + "    FETCH NEXT FROM CUR2 INTO @TRANDATE"
            StrSql += vbCrLf + "    WHILE @@FETCH_STATUS=0"
            StrSql += vbCrLf + "    BEGIN"
            StrSql += vbCrLf + "    SET @QRY2='UPDATE TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP' + CHAR(13)"
            StrSql += vbCrLf + "    SET @QRY2=@QRY2 + 'SET [' + @COL2 + ']=(SELECT SUM(COMMISION) FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " WHERE TRANDATE='''+ @TRANDATE +'''  '"
            StrSql += vbCrLf + "    SET @QRY2=@QRY2 + 'AND EMPNAME='''+ @COL2 +''' ) WHERE TRANDATE='''+ @TRANDATE +''' ' + CHAR(13)"
            StrSql += vbCrLf + "    PRINT @QRY2"
            StrSql += vbCrLf + "    EXEC (@QRY2)"

            StrSql += vbCrLf + "    SET @QRY2='UPDATE TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP' + CHAR(13)"
            StrSql += vbCrLf + "    SET @QRY2=@QRY2 + 'SET TOTCOMM=(SELECT SUM(COMMISION) FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " WHERE TRANDATE='''+ @TRANDATE +'''  '"
            StrSql += vbCrLf + "    SET @QRY2=@QRY2 + ') WHERE TRANDATE='''+ @TRANDATE +''' ' + CHAR(13)"
            StrSql += vbCrLf + "    PRINT @QRY2"
            StrSql += vbCrLf + "    EXEC (@QRY2)"

            StrSql += vbCrLf + "    FETCH NEXT FROM CUR2 INTO @TRANDATE"
            StrSql += vbCrLf + "    END"
            StrSql += vbCrLf + "    CLOSE CUR2"
            StrSql += vbCrLf + "    DEALLOCATE CUR2"
            StrSql += vbCrLf + "    FETCH NEXT FROM CUR INTO @COL2 "
            StrSql += vbCrLf + "    END "
            StrSql += vbCrLf + "    CLOSE CUR "
            StrSql += vbCrLf + "    DEALLOCATE CUR "
            StrSql += vbCrLf + "    PRINT @QRY2"
            StrSql += vbCrLf + "    EXEC (@QRY2)"
            Cmd = New OleDbCommand(StrSql, cn)
            Cmd.ExecuteNonQuery()


            StrSql = vbCrLf + "    /*STEP 5*/"
            StrSql += vbCrLf + "    DECLARE @COL4 AS VARCHAR(100)"
            StrSql += vbCrLf + "    DECLARE @QRY4 AS VARCHAR(8000)"
            StrSql += vbCrLf + "    DECLARE CUR CURSOR FOR SELECT DISTINCT EMPNAME FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " ORDER BY EMPNAME"
            StrSql += vbCrLf + "    OPEN CUR "
            StrSql += vbCrLf + "    FETCH NEXT FROM CUR INTO @COL4"
            StrSql += vbCrLf + "    WHILE @@FETCH_STATUS =0"
            StrSql += vbCrLf + "    BEGIN"
            StrSql += vbCrLf + "    PRINT @COL4"

            StrSql += vbCrLf + "    SET @QRY4='UPDATE TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP' + CHAR(13)"
            StrSql += vbCrLf + "    SET @QRY4=@QRY4 + 'SET [' + @COL4 + ']=(SELECT SUM(COMMISION) FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " WHERE EMPNAME='''+ @COL4 +'''  '"
            StrSql += vbCrLf + "    SET @QRY4=@QRY4 + 'AND EMPNAME='''+ @COL4 +''' ) WHERE TRANDATE=''2050-01-01'' ' + CHAR(13)"
            StrSql += vbCrLf + "    PRINT @QRY4"
            StrSql += vbCrLf + "    EXEC (@QRY4)"

            StrSql += vbCrLf + "    SET @QRY4='UPDATE TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP' + CHAR(13)"
            StrSql += vbCrLf + "    SET @QRY4=@QRY4 + 'SET TOTCOMM=(SELECT SUM(COMMISION) FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " '"
            StrSql += vbCrLf + "    SET @QRY4=@QRY4 + ') WHERE TRANDATE=''2050-01-01'' ' + CHAR(13)"
            StrSql += vbCrLf + "    PRINT @QRY4"
            StrSql += vbCrLf + "    EXEC (@QRY4)"

            StrSql += vbCrLf + "    FETCH NEXT FROM CUR INTO @COL4 "
            StrSql += vbCrLf + "    END "
            StrSql += vbCrLf + "    CLOSE CUR "
            StrSql += vbCrLf + "    DEALLOCATE CUR "
            StrSql += vbCrLf + "    PRINT @QRY4"
            StrSql += vbCrLf + "    EXEC (@QRY4)"
            Cmd = New OleDbCommand(StrSql, cn)
            Cmd.ExecuteNonQuery()


            'StrSql = vbCrLf + "    DECLARE @COL AS VARCHAR(100)"
            'StrSql += vbCrLf + "    DECLARE @QRY AS VARCHAR(8000)"
            'StrSql += vbCrLf + "    IF (SELECT OBJECT_ID('TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP" & Sysid & "')) > 0 DROP TABLE TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP" & Sysid & ""
            'StrSql += vbCrLf + "    DECLARE CUR CURSOR FOR SELECT DISTINCT EMPNAME FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " ORDER BY EMPNAME"
            'StrSql += vbCrLf + "    OPEN CUR "
            'StrSql += vbCrLf + "    FETCH NEXT FROM CUR INTO @COL"
            'StrSql += vbCrLf + "    SET @QRY = 'SELECT CONVERT(VARCHAR(12),TRANDATE,103) PARTICULAR '"
            'StrSql += vbCrLf + "    WHILE @@FETCH_STATUS =0"
            'StrSql += vbCrLf + "    BEGIN"
            'StrSql += vbCrLf + "    SET @QRY = @QRY + ',[' + @COL + '] ' + CHAR(13)"
            'StrSql += vbCrLf + "    FETCH NEXT FROM CUR INTO @COL "
            'StrSql += vbCrLf + "    END "
            'StrSql += vbCrLf + "    SET @QRY = @QRY + ' ,TOTCOMM,IDENTITY(INT,1,1) SNO INTO TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP" & Sysid & " FROM TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP ORDER BY TRANDATE '"
            'StrSql += vbCrLf + "    CLOSE CUR "
            'StrSql += vbCrLf + "    DEALLOCATE CUR "
            'StrSql += vbCrLf + "    PRINT @QRY"
            'StrSql += vbCrLf + "    EXEC (@QRY)"



            StrSql = "    IF (SELECT OBJECT_ID('TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP" & Sysid & "NAME')) > 0 DROP TABLE TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP" & Sysid & "NAME"
            Cmd = New OleDbCommand(StrSql, cn)
            Cmd.ExecuteNonQuery()

            StrSql = " SELECT DISTINCT A.EMPNAME,C.DISPORDER INTO TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & "NAME "
            StrSql += vbCrLf + "        FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " AS A," & cnAdminDb & "..EMPMASTER AS B"
            StrSql += vbCrLf + "        LEFT JOIN " & cnAdminDb & "..DESIGNATION AS C ON B.DESIGNATIONID=C.DESIGID"
            StrSql += vbCrLf + "        WHERE A.EMPNAME=B.EMPNAME"
            StrSql += vbCrLf + "        ORDER BY C.DISPORDER,A.EMPNAME    "
            Cmd = New OleDbCommand(StrSql, cn)
            Cmd.ExecuteNonQuery()

            StrSql = vbCrLf + "    DECLARE @COL AS VARCHAR(100)"
            StrSql += vbCrLf + "    DECLARE @QRY AS VARCHAR(8000)"
            StrSql += vbCrLf + "    IF (SELECT OBJECT_ID('TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP" & Sysid & "')) > 0 DROP TABLE TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP" & Sysid & ""
            StrSql += vbCrLf + "        DECLARE @DESIGID AS INT"
            StrSql += vbCrLf + "    DECLARE CUR CURSOR FOR SELECT EMPNAME FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & "NAME ORDER BY DISPORDER,EMPNAME"
            StrSql += vbCrLf + "    OPEN CUR "
            StrSql += vbCrLf + "    FETCH NEXT FROM CUR INTO @COL"
            StrSql += vbCrLf + "    SET @QRY = 'SELECT CONVERT(VARCHAR(12),TRANDATE,103) PARTICULAR '"
            StrSql += vbCrLf + "    WHILE @@FETCH_STATUS =0"
            StrSql += vbCrLf + "    BEGIN"
            StrSql += vbCrLf + "    SET @QRY = @QRY + ',[' + @COL + '] ' + CHAR(13)"
            StrSql += vbCrLf + "    FETCH NEXT FROM CUR INTO @COL"
            StrSql += vbCrLf + "    END "
            StrSql += vbCrLf + "    SET @QRY = @QRY + ' ,TOTCOMM,IDENTITY(INT,1,1) SNO INTO TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP" & Sysid & " FROM TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP ORDER BY TRANDATE '"
            StrSql += vbCrLf + "    CLOSE CUR "
            StrSql += vbCrLf + "    DEALLOCATE CUR "
            StrSql += vbCrLf + "    PRINT @QRY"
            StrSql += vbCrLf + "    EXEC (@QRY)"
            Cmd = New OleDbCommand(StrSql, cn)
            Cmd.ExecuteNonQuery()
            StrSql = "  SELECT * FROM TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP" & Sysid & " ORDER BY SNO"

        ElseIf cmbViewBy.Text = "EMPLOYEE,TRANDATE WISE SUMMARY" Then

            StrSql = vbCrLf + "    /*STEP 1*/"
            StrSql += vbCrLf + "    DECLARE @COL AS VARCHAR(100)"
            StrSql += vbCrLf + "    DECLARE @QRY AS VARCHAR(8000)"
            StrSql += vbCrLf + "    DECLARE CUR CURSOR FOR SELECT DISTINCT EMPNAME FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " ORDER BY EMPNAME"
            StrSql += vbCrLf + "    OPEN CUR "
            StrSql += vbCrLf + "    FETCH NEXT FROM CUR INTO @COL"
            StrSql += vbCrLf + "    IF (SELECT OBJECT_ID('TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP')) > 0 DROP TABLE TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP"
            StrSql += vbCrLf + "    SET @QRY = 'CREATE TABLE TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP (TRANDATE SMALLDATETIME '"
            StrSql += vbCrLf + "    WHILE @@FETCH_STATUS =0"
            StrSql += vbCrLf + "    BEGIN"
            StrSql += vbCrLf + "    SET @QRY = @QRY + ',[' + @COL + '] [NUMERIC](15,2) NULL' + CHAR(13)"
            StrSql += vbCrLf + "    FETCH NEXT FROM CUR INTO @COL "
            StrSql += vbCrLf + "    END "
            StrSql += vbCrLf + "    SET @QRY = @QRY + ' ,TOTCOMM [NUMERIC](15,2) NULL )'"
            StrSql += vbCrLf + "    CLOSE CUR "
            StrSql += vbCrLf + "    DEALLOCATE CUR "
            StrSql += vbCrLf + "    PRINT @QRY"
            StrSql += vbCrLf + "    EXEC (@QRY)"
            Cmd = New OleDbCommand(StrSql, cn)
            Cmd.ExecuteNonQuery()

            StrSql = vbCrLf + "    /*STEP 2*/"
            StrSql += vbCrLf + "    DECLARE @FROMDATE SMALLDATETIME,@TODATE SMALLDATETIME"
            StrSql += vbCrLf + "    SET @FROMDATE='" & dtpFrom.Value.AddDays(-1).Date.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + "    SET @TODATE='" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + "    INSERT INTO TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP(TRANDATE)"
            StrSql += vbCrLf + "    SELECT DATEADD(DAY,NUMBER+1,@FROMDATE)DATE"
            StrSql += vbCrLf + "    FROM master..spt_values WHERE type='P' "
            StrSql += vbCrLf + "    AND DATEADD(DAY,NUMBER+1,@FROMDATE)<=@TODATE  "
            Cmd = New OleDbCommand(StrSql, cn)
            Cmd.ExecuteNonQuery()

            StrSql = vbCrLf + "    /*STEP 3*/"
            StrSql += vbCrLf + "    DECLARE @COL3 AS VARCHAR(100)"
            StrSql += vbCrLf + "    DECLARE @QRY3 AS VARCHAR(8000)"
            StrSql += vbCrLf + "    DECLARE CUR CURSOR FOR SELECT DISTINCT EMPNAME FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " ORDER BY EMPNAME"
            StrSql += vbCrLf + "    OPEN CUR "
            StrSql += vbCrLf + "    FETCH NEXT FROM CUR INTO @COL3"
            StrSql += vbCrLf + "    SET @QRY3 = ' INSERT INTO TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP'"
            StrSql += vbCrLf + "    SET @QRY3 = @QRY3 + ' SELECT ''2050-01-01'' TRANDATE '"
            StrSql += vbCrLf + "    WHILE @@FETCH_STATUS =0"
            StrSql += vbCrLf + "    BEGIN"
            StrSql += vbCrLf + "    SET @QRY3 = @QRY3 + ',NULL [' + @COL3 + '] ' + CHAR(13)"
            StrSql += vbCrLf + "    FETCH NEXT FROM CUR INTO @COL3 "
            StrSql += vbCrLf + "    END "
            StrSql += vbCrLf + "    SET @QRY3 = @QRY3 + ' ,NULL TOTCOMM  '"
            StrSql += vbCrLf + "    CLOSE CUR "
            StrSql += vbCrLf + "    DEALLOCATE CUR "
            StrSql += vbCrLf + "    PRINT @QRY3"
            StrSql += vbCrLf + "    EXEC (@QRY3)"
            Cmd = New OleDbCommand(StrSql, cn)
            Cmd.ExecuteNonQuery()

            StrSql = vbCrLf + "    /*STEP 4*/"
            StrSql += vbCrLf + "    DECLARE @COL2 AS VARCHAR(100)"
            StrSql += vbCrLf + "    DECLARE @TRANDATE AS VARCHAR(12)"
            StrSql += vbCrLf + "    DECLARE @QRY2 AS VARCHAR(8000)"
            StrSql += vbCrLf + "    DECLARE CUR CURSOR FOR SELECT DISTINCT EMPNAME FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " ORDER BY EMPNAME"
            StrSql += vbCrLf + "    OPEN CUR "
            StrSql += vbCrLf + "    FETCH NEXT FROM CUR INTO @COL2"
            StrSql += vbCrLf + "    WHILE @@FETCH_STATUS =0"
            StrSql += vbCrLf + "    BEGIN"
            StrSql += vbCrLf + "    DECLARE CUR2 CURSOR FOR SELECT DISTINCT CONVERT(VARCHAR(12),TRANDATE,112) FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " "
            StrSql += vbCrLf + "    OPEN CUR2"
            StrSql += vbCrLf + "    FETCH NEXT FROM CUR2 INTO @TRANDATE"
            StrSql += vbCrLf + "    WHILE @@FETCH_STATUS=0"
            StrSql += vbCrLf + "    BEGIN"
            StrSql += vbCrLf + "    SET @QRY2='UPDATE TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP' + CHAR(13)"
            StrSql += vbCrLf + "    SET @QRY2=@QRY2 + 'SET [' + @COL2 + ']=(SELECT SUM(COMMISION) FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " WHERE TRANDATE='''+ @TRANDATE +'''  '"
            StrSql += vbCrLf + "    SET @QRY2=@QRY2 + 'AND EMPNAME='''+ @COL2 +''' ) WHERE TRANDATE='''+ @TRANDATE +''' ' + CHAR(13)"
            StrSql += vbCrLf + "    PRINT @QRY2"
            StrSql += vbCrLf + "    EXEC (@QRY2)"

            StrSql += vbCrLf + "    SET @QRY2='UPDATE TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP' + CHAR(13)"
            StrSql += vbCrLf + "    SET @QRY2=@QRY2 + 'SET TOTCOMM=(SELECT SUM(COMMISION) FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " WHERE TRANDATE='''+ @TRANDATE +'''  '"
            StrSql += vbCrLf + "    SET @QRY2=@QRY2 + ') WHERE TRANDATE='''+ @TRANDATE +''' ' + CHAR(13)"
            StrSql += vbCrLf + "    PRINT @QRY2"
            StrSql += vbCrLf + "    EXEC (@QRY2)"

            StrSql += vbCrLf + "    FETCH NEXT FROM CUR2 INTO @TRANDATE"
            StrSql += vbCrLf + "    END"
            StrSql += vbCrLf + "    CLOSE CUR2"
            StrSql += vbCrLf + "    DEALLOCATE CUR2"
            StrSql += vbCrLf + "    FETCH NEXT FROM CUR INTO @COL2 "
            StrSql += vbCrLf + "    END "
            StrSql += vbCrLf + "    CLOSE CUR "
            StrSql += vbCrLf + "    DEALLOCATE CUR "
            StrSql += vbCrLf + "    PRINT @QRY2"
            StrSql += vbCrLf + "    EXEC (@QRY2)"
            Cmd = New OleDbCommand(StrSql, cn)
            Cmd.ExecuteNonQuery()


            StrSql = vbCrLf + "    /*STEP 5*/"
            StrSql += vbCrLf + "    DECLARE @COL4 AS VARCHAR(100)"
            StrSql += vbCrLf + "    DECLARE @QRY4 AS VARCHAR(8000)"
            StrSql += vbCrLf + "    DECLARE CUR CURSOR FOR SELECT DISTINCT EMPNAME FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " ORDER BY EMPNAME"
            StrSql += vbCrLf + "    OPEN CUR "
            StrSql += vbCrLf + "    FETCH NEXT FROM CUR INTO @COL4"
            StrSql += vbCrLf + "    WHILE @@FETCH_STATUS =0"
            StrSql += vbCrLf + "    BEGIN"
            StrSql += vbCrLf + "    PRINT @COL4"

            StrSql += vbCrLf + "    SET @QRY4='UPDATE TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP' + CHAR(13)"
            StrSql += vbCrLf + "    SET @QRY4=@QRY4 + 'SET [' + @COL4 + ']=(SELECT SUM(COMMISION) FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " WHERE EMPNAME='''+ @COL4 +'''  '"
            StrSql += vbCrLf + "    SET @QRY4=@QRY4 + 'AND EMPNAME='''+ @COL4 +''' ) WHERE TRANDATE=''2050-01-01'' ' + CHAR(13)"
            StrSql += vbCrLf + "    PRINT @QRY4"
            StrSql += vbCrLf + "    EXEC (@QRY4)"

            StrSql += vbCrLf + "    SET @QRY4='UPDATE TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP' + CHAR(13)"
            StrSql += vbCrLf + "    SET @QRY4=@QRY4 + 'SET TOTCOMM=(SELECT SUM(COMMISION) FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " '"
            StrSql += vbCrLf + "    SET @QRY4=@QRY4 + ') WHERE TRANDATE=''2050-01-01'' ' + CHAR(13)"
            StrSql += vbCrLf + "    PRINT @QRY4"
            StrSql += vbCrLf + "    EXEC (@QRY4)"

            StrSql += vbCrLf + "    FETCH NEXT FROM CUR INTO @COL4 "
            StrSql += vbCrLf + "    END "
            StrSql += vbCrLf + "    CLOSE CUR "
            StrSql += vbCrLf + "    DEALLOCATE CUR "
            StrSql += vbCrLf + "    PRINT @QRY4"
            StrSql += vbCrLf + "    EXEC (@QRY4)"
            Cmd = New OleDbCommand(StrSql, cn)
            Cmd.ExecuteNonQuery()
            StrSql = vbCrLf + "    DECLARE @COL AS VARCHAR(100)"
            StrSql += vbCrLf + "    DECLARE @QRY AS VARCHAR(8000)"
            StrSql += vbCrLf + "    IF (SELECT OBJECT_ID('TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP" & Sysid & "')) > 0 DROP TABLE TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP" & Sysid & ""
            StrSql += vbCrLf + "    DECLARE CUR CURSOR FOR SELECT DISTINCT EMPNAME FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " ORDER BY EMPNAME"
            StrSql += vbCrLf + "    OPEN CUR "
            StrSql += vbCrLf + "    FETCH NEXT FROM CUR INTO @COL"
            StrSql += vbCrLf + "    SET @QRY = 'SELECT CONVERT(VARCHAR(12),TRANDATE,103) PARTICULAR '"
            StrSql += vbCrLf + "    WHILE @@FETCH_STATUS =0"
            StrSql += vbCrLf + "    BEGIN"
            StrSql += vbCrLf + "    SET @QRY = @QRY + ',[' + @COL + '] ' + CHAR(13)"
            StrSql += vbCrLf + "    FETCH NEXT FROM CUR INTO @COL "
            StrSql += vbCrLf + "    END "
            StrSql += vbCrLf + "    SET @QRY = @QRY + ' ,TOTCOMM,IDENTITY(INT,1,1) SNO INTO TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP" & Sysid & " FROM TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP ORDER BY TRANDATE '"
            StrSql += vbCrLf + "    CLOSE CUR "
            StrSql += vbCrLf + "    DEALLOCATE CUR "
            StrSql += vbCrLf + "    PRINT @QRY"
            StrSql += vbCrLf + "    EXEC (@QRY)"
            Cmd = New OleDbCommand(StrSql, cn)
            Cmd.ExecuteNonQuery()
            StrSql = "  SELECT * FROM TEMPTABLEDB..TEMPSALESCOMMISION_TRANEMP" & Sysid & " ORDER BY SNO"
        ElseIf cmbViewBy.Text = "ITEM WISE CUMULATIVE" Then
            StrSql = vbCrLf + "IF OBJECT_ID('TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & "') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & ""
            StrSql += vbCrLf + "SELECT SUBITEMNAME AS PARTICULAR,EMPNAME,ITEM,SUBITEMNAME,METALID"
            StrSql += vbCrLf + ",COSTID,EMPID,ITEMID,SUBITEMID,CALTYPE"

            StrSql += vbCrLf + ",SUM(CASE WHEN SEP='SA' THEN PCS END)SPCS"
            StrSql += vbCrLf + ",SUM(CASE WHEN SEP='SA' THEN GRSWT END)SGRSWT"
            StrSql += vbCrLf + ",SUM(CASE WHEN SEP='SA' THEN NETWT END)SNETWT"
            StrSql += vbCrLf + ",SUM(CASE WHEN SEP='SA' THEN AMOUNT END)SAMOUNT"

            StrSql += vbCrLf + ",SUM(CASE WHEN SEP='SR' THEN PCS END)RPCS"
            StrSql += vbCrLf + ",SUM(CASE WHEN SEP='SR' THEN GRSWT END)RGRSWT"
            StrSql += vbCrLf + ",SUM(CASE WHEN SEP='SR' THEN NETWT END)RNETWT"
            StrSql += vbCrLf + ",SUM(CASE WHEN SEP='SR' THEN AMOUNT END)RAMOUNT"

            StrSql += vbCrLf + ",SUM(CASE WHEN SEP='SA' THEN PCS ELSE -1*PCS END)PCS"
            StrSql += vbCrLf + ",SUM(CASE WHEN SEP='SA' THEN GRSWT ELSE -1*GRSWT END)GRSWT"
            StrSql += vbCrLf + ",SUM(CASE WHEN SEP='SA' THEN NETWT ELSE -1*NETWT END)NETWT"
            StrSql += vbCrLf + ",SUM(CASE WHEN SEP='SA' THEN AMOUNT ELSE -1*AMOUNT END)AMOUNT"
            StrSql += vbCrLf + ",CONVERT(NUMERIC(15,2),0)COMM_RATE"
            StrSql += vbCrLf + ",CONVERT(NUMERIC(15,2),0)COMMISION"
            StrSql += vbCrLf + ",1 AS RESULT,'' AS COLHEAD"
            StrSql += vbCrLf + "INTO TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & ""
            StrSql += vbCrLf + "FROM TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " S "
            StrSql += vbCrLf + "GROUP BY EMPNAME,COSTID,EMPID,ITEM,SUBITEMNAME,METALID,EMPID,ITEMID,SUBITEMID,CALTYPE"
            Cmd = New OleDbCommand(StrSql, cn)
            Cmd.ExecuteNonQuery()
            StrSql = vbCrLf + "UPDATE A SET A.COMM_RATE=B.COMMISIONGRM"
            StrSql += vbCrLf + "FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " AS A,GEMADMINDB..SALES_COMMISION AS B "
            StrSql += vbCrLf + "WHERE A.ITEMID=B.ITEMID AND A.SUBITEMID=B.SUBITEMID AND A.GRSWT BETWEEN B.FROM_VAL AND TO_VAL"
            Cmd = New OleDbCommand(StrSql, cn)
            Cmd.ExecuteNonQuery()
            StrSql = vbCrLf + "UPDATE A SET COMMISION=CONVERT(NUMERIC(15,2),ISNULL(A.GRSWT,0)*ISNULL(A.COMM_RATE,0)) "
            StrSql += vbCrLf + "FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " AS A"
            Cmd = New OleDbCommand(StrSql, cn)
            Cmd.ExecuteNonQuery()
            StrSql = vbCrLf + "UPDATE A SET COMMISION=NULL,A.COMM_RATE=NULL "
            StrSql += vbCrLf + "FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " AS A WHERE ISNULL(A.COMM_RATE,0)=0"
            Cmd = New OleDbCommand(StrSql, cn)
            Cmd.ExecuteNonQuery()
            StrSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " (PARTICULAR,EMPNAME,RESULT,COLHEAD)"
            StrSql += vbCrLf + "SELECT "
            StrSql += vbCrLf + "EMPNAME,EMPNAME,0 AS RESULT,'T' AS COLHEAD"
            StrSql += vbCrLf + "FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " AS A WHERE RESULT=1 GROUP BY EMPNAME"
            Cmd = New OleDbCommand(StrSql, cn)
            Cmd.ExecuteNonQuery()
            StrSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " (PARTICULAR,EMPNAME,SPCS,SGRSWT,SNETWT,SAMOUNT,RPCS,RGRSWT,RNETWT,RAMOUNT,PCS,GRSWT,NETWT,AMOUNT,COMMISION,RESULT,COLHEAD)"
            StrSql += vbCrLf + "SELECT "
            StrSql += vbCrLf + "EMPNAME + ' TOTAL',EMPNAME"
            StrSql += vbCrLf + ",SUM(SPCS)SPCS,SUM(SGRSWT)SGRSWT,SUM(SNETWT)SNETWT,SUM(SAMOUNT)SAMOUNT"
            StrSql += vbCrLf + ",SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,SUM(RAMOUNT)RAMOUNT"
            StrSql += vbCrLf + ",SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT,SUM(COMMISION)COMMISION"
            StrSql += vbCrLf + ",3 AS RESULT,'S' AS COLHEAD"
            StrSql += vbCrLf + "FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " AS A WHERE RESULT=1 GROUP BY EMPNAME"
            Cmd = New OleDbCommand(StrSql, cn)
            Cmd.ExecuteNonQuery()
            StrSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " (PARTICULAR,EMPNAME,SPCS,SGRSWT,SNETWT,SAMOUNT,RPCS,RGRSWT,RNETWT,RAMOUNT,PCS,GRSWT,NETWT,AMOUNT,COMMISION,RESULT,COLHEAD)"
            StrSql += vbCrLf + "SELECT "
            StrSql += vbCrLf + "'GRAND TOTAL','ZZZZZZ' EMPNAME"
            StrSql += vbCrLf + ",SUM(SPCS)SPCS,SUM(SGRSWT)SGRSWT,SUM(SNETWT)SNETWT,SUM(SAMOUNT)SAMOUNT"
            StrSql += vbCrLf + ",SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,SUM(RAMOUNT)RAMOUNT"
            StrSql += vbCrLf + ",SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT)AMOUNT,SUM(COMMISION)COMMISION"
            StrSql += vbCrLf + ",3 AS RESULT,'S' AS COLHEAD"
            StrSql += vbCrLf + "FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " AS A WHERE RESULT=1"
            Cmd = New OleDbCommand(StrSql, cn)
            Cmd.ExecuteNonQuery()
            StrSql = vbCrLf + "SELECT * FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " AS A ORDER BY EMPNAME,RESULT"
        Else
            StrSql = "  SELECT * FROM ("
            StrSql += vbCrLf + "   SELECT DISTINCT EMPNAME AS PARTICULAR,EMPNAME,CONVERT(VARCHAR(200),'') COUNTER,"
            StrSql += vbCrLf + "   CONVERT(VARCHAR(200),'') METAL,CONVERT(VARCHAR(200),'') ITEM,CONVERT(VARCHAR(200),'') SUBITEMNAME,"
            StrSql += vbCrLf + "   NULL COMMISION,NULL SALESCOMM,NULL BACKINC,"
            StrSql += vbCrLf + "   NULL BACKCOMM,NULL OTHERCOMM,NULL SPCS,"
            StrSql += vbCrLf + "   NULL SGRSWT,NULL SNETWT,NULL RPCS,"
            StrSql += vbCrLf + "   NULL RGRSWT,NULL RNETWT,0 AS RESULT,'T' COLHEAD "
            StrSql += vbCrLf + "   FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " S WHERE RESULT = 1 GROUP BY EMPNAME"
            StrSql += vbCrLf + "  UNION ALL"
            StrSql += vbCrLf + "   SELECT '        ' + ITEM AS PARTICULAR,EMPNAME"
            If rbtAll.Checked And rbtSummary.Checked Then
                StrSql += vbCrLf + ",CONVERT(VARCHAR(200),'') COUNTER"
            Else
                StrSql += vbCrLf + "  ,COUNTER"
            End If
            StrSql += vbCrLf + "  ,METAL,ITEM,CONVERT(VARCHAR(200),'') SUBITEMNAME "
            StrSql += vbCrLf + "   ,SUM(COMMISION)COMMISION,SUM(SALESCOMM)SALESCOMM,SUM(BACKINC)BACKINC,SUM(BACKCOMM)BACKCOMM"
            StrSql += vbCrLf + "   ,SUM(OTHERCOMM)OTHERCOMM,SUM(SPCS)SPCS,SUM(SGRSWT)SGRSWT,SUM(SNETWT)SNETWT"
            StrSql += vbCrLf + "   ,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,2 AS RESULT,'X' COLHEAD"
            StrSql += vbCrLf + "   FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " S WHERE RESULT = 1 GROUP BY EMPNAME,ITEM,METAL"
            If rbtAll.Checked = False And rbtSummary.Checked = False Then StrSql += vbCrLf + ",COUNTER"

            StrSql += vbCrLf + "   UNION ALL"
            StrSql += vbCrLf + "   SELECT 'TOTAL '+EMPNAME AS PARTICULAR, EMPNAME,CONVERT(VARCHAR(200),'ZZZZZ') COUNTER, "
            StrSql += vbCrLf + "   CONVERT(VARCHAR(200),'') METAL,CONVERT(VARCHAR(200),'') ITEM,CONVERT(VARCHAR(200),'') SUBITEMNAME"
            StrSql += vbCrLf + "   ,SUM(COMMISION)COMMISION,SUM(SALESCOMM)SALESCOMM,SUM(BACKINC)BACKINC,SUM(BACKCOMM)BACKCOMM"
            StrSql += vbCrLf + "   ,SUM(OTHERCOMM)OTHERCOMM,SUM(SPCS)SPCS,SUM(SGRSWT)SGRSWT,SUM(SNETWT)SNETWT"
            StrSql += vbCrLf + "   ,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,5 AS RESULT,'S2' COLHEAD"
            StrSql += vbCrLf + "   FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " S WHERE RESULT = 1 GROUP BY EMPNAME"
            StrSql += vbCrLf + "    UNION ALL"
            StrSql += vbCrLf + "   SELECT 'GRAND TOTAL ' AS PARTICULAR,'ZZZZZ' EMPNAME,CONVERT(VARCHAR(200),'ZZZZZ') COUNTER, "
            StrSql += vbCrLf + "   CONVERT(VARCHAR(200),'') METAL,CONVERT(VARCHAR(200),'') ITEM,CONVERT(VARCHAR(200),'') SUBITEMNAME"
            StrSql += vbCrLf + "   ,SUM(COMMISION)COMMISION,SUM(SALESCOMM)SALESCOMM,SUM(BACKINC)BACKINC,SUM(BACKCOMM)BACKCOMM"
            StrSql += vbCrLf + "   ,SUM(OTHERCOMM)OTHERCOMM,SUM(SPCS)SPCS,SUM(SGRSWT)SGRSWT,SUM(SNETWT)SNETWT"
            StrSql += vbCrLf + "   ,SUM(RPCS)RPCS,SUM(RGRSWT)RGRSWT,SUM(RNETWT)RNETWT,6 AS RESULT,'G' COLHEAD"
            StrSql += vbCrLf + "   FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " S WHERE RESULT = 1 "
            StrSql += vbCrLf + "   )X ORDER BY EMPNAME,RESULT,ITEM,SUBITEMNAME,COUNTER,METAL"
        End If

        Dim dtSource As New DataTable
        dtSource.Columns.Add("KEYNO", GetType(Integer))
        dtSource.Columns("KEYNO").AutoIncrement = True
        dtSource.Columns("KEYNO").AutoIncrementSeed = 0
        dtSource.Columns("KEYNO").AutoIncrementStep = 1
        Cmd = New OleDbCommand(StrSql, cn)
        Da = New OleDbDataAdapter(Cmd)
        Da.Fill(dtSource)

        Dim objGridShower As New frmGridDispDia
        objGridShower.gridView.DataSource = Nothing
        objGridShower.gridView.DataSource = dtSource
        objGridShower.Name = Me.Name
        objGridShower.WindowState = FormWindowState.Maximized
        objGridShower.gridView.Name = Me.Name
        If Not cmbViewBy.Text = "TRANDATE,EMPLOYEE WISE SUMMARY" Then
            objGridShower.gridView.Columns("PARTICULAR").Visible = True
            objGridShower.gridView.Columns("EMPNAME").Visible = Not rbtGrpEmployee.Checked
            If objGridShower.gridView.Columns.Contains("COUNTER") Then objGridShower.gridView.Columns("COUNTER").Visible = Not rbtGrpCounter.Checked
            objGridShower.gridView.Columns("KEYNO").Visible = False
            objGridShower.gridView.Columns("KEYNO").Visible = False
            objGridShower.gridView.Columns("RESULT").Visible = False
            'objGridShower.gridView.Columns("ITEM").Visible = False
            If objGridShower.gridView.Columns.Contains("METAL") Then objGridShower.gridView.Columns("METAL").Visible = False
            objGridShower.gridView.Columns("COLHEAD").Visible = False
            If objGridShower.gridView.Columns.Contains("BACKINC") Then objGridShower.gridView.Columns("BACKINC").Visible = False
            If rbtSummary.Checked Then
                objGridShower.gridView.Columns("EMPNAME").Visible = False
                If objGridShower.gridView.Columns.Contains("COUNTER") Then objGridShower.gridView.Columns("COUNTER").Visible = False
            End If
            If cmbViewBy.Text.Trim = "COUNTER,METAL,ITEM,SUB ITEM WISE SUMMARY" Or cmbViewBy.Text.Trim = "ITEM,SUB ITEM WISE SUMMARY" Then
                objGridShower.gridView.Columns("SUBITEMNAME").Visible = True
            Else
                objGridShower.gridView.Columns("SUBITEMNAME").Visible = False
            End If
            If cmbViewBy.Text.Trim = "ITEM WISE SUMMARY" Or cmbViewBy.Text.Trim = "ITEM,SUB ITEM WISE SUMMARY" Then
                objGridShower.gridView.Columns("COUNTER").Visible = True
                objGridShower.gridView.Columns("METAL").Visible = True
                objGridShower.gridView.Columns("ITEM").Visible = False
            End If
            If cmbViewBy.Text.Trim = "ITEM,SUB ITEM WISE SUMMARY" Then
                objGridShower.gridView.Columns("ITEM").Visible = False
                objGridShower.gridView.Columns("SUBITEMNAME").Visible = False
            End If

            If cmbViewBy.Text.Trim = "ITEM WISE CUMULATIVE" Then
                objGridShower.gridView.Columns("ITEMID").Visible = False
                objGridShower.gridView.Columns("SUBITEMID").Visible = False
                objGridShower.gridView.Columns("METALID").Visible = False
                objGridShower.gridView.Columns("COSTID").Visible = False
                objGridShower.gridView.Columns("CALTYPE").Visible = False
                objGridShower.gridView.Columns("EMPID").Visible = False
            End If

            If cmbViewBy.Text.Trim = "COUNTER WISE SUMMARY" Then
                objGridShower.gridView.Columns("ITEM").Visible = False
                objGridShower.gridView.Columns("SUBITEMNAME").Visible = False
            End If
            If cmbViewBy.Text.Trim = "TRANDATE WISE SUMMARY" Then
                objGridShower.gridView.Columns("ITEM").Visible = False
            End If
            objGridShower.gridView.Columns("SUBITEMNAME").HeaderText = "SUB ITEM"
            If Val(txtCommPercentage_AMT.Text) <> 0 Then
                objGridShower.gridView.Columns("SALESCOMM").Visible = True
                objGridShower.gridView.Columns("OTHERCOMM").Visible = True
                objGridShower.gridView.Columns("SALESCOMM").HeaderText = "SALES COUNTER COMM"
                objGridShower.gridView.Columns("OTHERCOMM").HeaderText = "OTHER COUNTER COMM"
            Else
                If objGridShower.gridView.Columns.Contains("SALESCOMM") Then objGridShower.gridView.Columns("SALESCOMM").Visible = False
                If objGridShower.gridView.Columns.Contains("OTHERCOMM") Then objGridShower.gridView.Columns("OTHERCOMM").Visible = False
            End If
            FormatGridColumns(objGridShower.gridView, False, False, True, False)
            If cmbViewBy.Text.Trim = "TRANDATE WISE SUMMARY" Then
                objGridShower.gridView.Columns("TRANDATE").Visible = False
            End If
            objGridShower.gridView.Columns("SPCS").HeaderText = "SALES PCS"
            objGridShower.gridView.Columns("SGRSWT").HeaderText = "SALES GRSWT"
            objGridShower.gridView.Columns("SNETWT").HeaderText = "SALES NETWT"
            'objGridShower.gridView.Columns("SAMOUNT").HeaderText = "SALES AMOUNT"
            objGridShower.gridView.Columns("RPCS").HeaderText = "RETURN PCS"
            objGridShower.gridView.Columns("RGRSWT").HeaderText = "RETURN GRSWT"
            objGridShower.gridView.Columns("RNETWT").HeaderText = "RETURN NETWT"
            'objGridShower.gridView.Columns("RAMOUNT").HeaderText = "RETURN AMOUNT"
        End If
        Dim tit As String
        If rbtGrpEmployee.Checked Then
            tit = "EMPLOYEE"
        ElseIf rbtproduct.Checked Then
            tit = "PRODUCT"
        ElseIf rbtGrpCounter.Checked Then
            tit = "COUNTER"
        Else
            tit = "METAL"
        End If
        objGridShower.Text = tit & " WISE SALES COMMISION"
        tit += " WISE SALES COMMISION" + vbCrLf
        tit += " DATE FROM " & dtpFrom.Text + " TO " + dtpTo.Text
        tit += IIf(chkCmbCostCentre.Text <> "" And chkCmbCostCentre.Text <> "ALL", " :" & chkCmbCostCentre.Text, "")
        If chkcmbmetal.Text <> "ALL" And chkcmbmetal.Text <> "" Then
            tit += " FOR METAL " & chkcmbmetal.Text & ""
        End If
        If chkcmbCounter.Text <> "ALL" And chkcmbCounter.Text <> "" Then
            tit += "(" & chkcmbCounter.Text & ")"
        End If
        If chkcmbitemname.Text <> "ALL" And chkcmbitemname.Text <> "" Then
            tit += "(" & chkcmbitemname.Text & ")"
        End If

        If chkcmbsubitem.Text <> "ALL" And chkcmbsubitem.Text <> "" Then
            tit += Environment.NewLine + "(" & chkcmbsubitem.Text & ")"
        End If

        If chkcmbemployee.Text <> "ALL" And chkcmbemployee.Text <> "" Then
            tit += vbCrLf & "FOR " & chkcmbemployee.Text
        End If
        If cmbViewBy.Text <> "ALL" And cmbViewBy.Text <> "" Then
            tit += vbCrLf & "VIEW BY " & cmbViewBy.Text
        End If
        objGridShower.pnlHeader.Height = objGridShower.lblTitle.Height + 50
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        objGridShower.Show()

        objGridShower.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
        objGridShower.gridView.Invalidate()
        For Each dgvCol As DataGridViewColumn In objGridShower.gridView.Columns
            dgvCol.Width = dgvCol.Width
        Next
        objGridShower.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        If Not cmbViewBy.Text = "TRANDATE,EMPLOYEE WISE SUMMARY" Then
            For Each dgvRow As DataGridViewRow In objGridShower.gridView.Rows
                With dgvRow
                    Select Case .Cells("COLHEAD").Value.ToString
                        Case "T"
                            .Cells("PARTICULAR").Style.BackColor = reportHeadStyle.BackColor
                            .DefaultCellStyle.ForeColor = reportHeadStyle.ForeColor
                            .DefaultCellStyle.Font = reportHeadStyle.Font
                        Case "T1"
                            '.Cells("PARTICULAR").Style.BackColor = reportSubTotalStyle2.BackColor
                            .DefaultCellStyle.ForeColor = Color.Blue
                            .DefaultCellStyle.Font = reportSubTotalStyle1.Font
                            '.Cells("PARTICULAR").Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                        Case "S"
                            .Cells("PARTICULAR").Style.BackColor = reportTotalStyle.BackColor
                            .DefaultCellStyle.ForeColor = Color.Red
                            .DefaultCellStyle.Font = reportSubTotalStyle1.Font
                            '.Cells("PARTICULAR").Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                        Case "X"
                            '.Cells("PARTICULAR").Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                        Case "S1"
                            .DefaultCellStyle.ForeColor = Color.Red
                            .DefaultCellStyle.Font = reportHeadStyle.Font
                        Case "S2"
                            .Cells("PARTICULAR").Style.BackColor = Color.PaleGreen
                            .DefaultCellStyle.ForeColor = Color.DarkGreen
                            .DefaultCellStyle.Font = reportHeadStyle.Font
                            .Cells("COUNTER").Value = ""
                        Case "G"
                            .DefaultCellStyle.BackColor = reportHeadStyle.BackColor
                            .DefaultCellStyle.ForeColor = reportHeadStyle.ForeColor
                            .DefaultCellStyle.Font = reportHeadStyle.Font
                            .Cells("COUNTER").Value = ""
                    End Select
                End With
            Next
        Else
            objGridShower.gridView.Columns("KEYNO").Visible = False
            With objGridShower.gridView.Columns("PARTICULAR")
                .Width = 80
                .Frozen = True
            End With
            With objGridShower.gridView.Columns("SNO")
                .Visible = False
            End With
            With objGridShower.gridView.Columns("TOTCOMM")
                .HeaderText = "TOTAL COMMISSION"
            End With
            If objGridShower.gridView.Rows.Count > 0 Then
                For Each dgvRow As DataGridViewRow In objGridShower.gridView.Rows
                    With dgvRow
                        .Cells("TOTCOMM").Style.BackColor = Color.LightBlue
                        .Cells("TOTCOMM").Style.ForeColor = Color.Blue
                        .Cells("TOTCOMM").Style.Font = reportHeadStyle.Font
                        If .Cells.ToString <> "PARTICULAR" Then
                            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        End If
                    End With
                Next
                With objGridShower.gridView.Rows(objGridShower.gridView.Rows.Count - 1)
                    .Cells("PARTICULAR").Value = "TOTAL"
                    .Cells("PARTICULAR").Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                    .DefaultCellStyle.ForeColor = Color.Red
                    .DefaultCellStyle.Font = reportHeadStyle.Font
                    .Cells("TOTCOMM").Style.BackColor = Color.LightGreen
                    .Cells("TOTCOMM").Style.ForeColor = Color.DarkGreen
                End With

            End If
        End If
        'FillGridGroupStyle_KeyNoWise(objGridShower.gridView, "PARTICULAR")
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True

    End Sub

    Private Sub GroupingGridbydefault()
        Dim grouper As String = ""
        Dim particular As String = ""
        Dim StoneDetail As Boolean = chkStoneDetails.Checked
        If rbtGrpEmployee.Checked Then
            grouper = "EMPNAME"
            particular = "ITEM"
        ElseIf rbtGrpCounter.Checked Then
            grouper = "ITEMCTRNAME"
            particular = "ITEM"
        ElseIf rbtproduct.Checked Then
            grouper = "ITEM"
            particular = "EMPNAME"
        ElseIf rbtMetal.Checked Then
            grouper = "METAL"
            particular = "EMPNAME"
        End If

        'StrSql = vbCrLf + " SELECT CONVERT(VARCHAR(300)," & particular & ")PARTICULAR"
        'StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' AND COMMISION <> 0 THEN -1*COMMISION WHEN SEP = 'SA' AND COMMISION > 0 THEN COMMISION ELSE NULL END AS COMMISION"
        'StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' AND SALESCOMM <> 0 THEN -1*SALESCOMM WHEN SEP = 'SA' AND SALESCOMM > 0 THEN SALESCOMM ELSE NULL END AS SALESCOMM"
        'StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' AND OTHERCOMM <> 0 THEN -1*OTHERCOMM WHEN SEP = 'SA' AND OTHERCOMM > 0 THEN OTHERCOMM ELSE NULL END AS OTHERCOMM"
        'StrSql += vbCrLf + " ,SUBITEMNAME,TRANNO,TRANDATE,TAGNO " & IIf(chkWithTagno.Checked, ",0 TAGNOS", "") & ""
        'StrSql += vbCrLf + " ,CASE WHEN SEP = 'SA' THEN PCS ELSE NULL END AS SPCS"
        'StrSql += vbCrLf + " ,CASE WHEN SEP = 'SA' THEN GRSWT ELSE NULL END AS SGRSWT"
        'StrSql += vbCrLf + " ,CASE WHEN SEP = 'SA' THEN NETWT ELSE NULL END AS SNETWT"
        'StrSql += vbCrLf + " ,CASE WHEN SEP = 'SA' THEN AMOUNT ELSE NULL END AS SAMOUNT"
        'StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' THEN PCS ELSE NULL END AS RPCS"
        'StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' THEN GRSWT ELSE NULL END AS RGRSWT"
        'StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' THEN NETWT ELSE NULL END AS RNETWT"
        'StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' THEN AMOUNT ELSE NULL END AS RAMOUNT"
        'StrSql += vbCrLf + " ,CONVERT(VARCHAR(100),EMPNAME)AS EMPNAME,CONVERT(VARCHAR(100),ITEMCTRNAME) AS COUNTER"
        'StrSql += vbCrLf + " ,CONVERT(VARCHAR(2),SEP)SEP,CONVERT(VARCHAR(3),'')AS COLHEAD,CONVERT(INT,1)AS RESULT"
        'StrSql += vbCrLf + " ,CONVERT(VARCHAR(200)," & grouper & ")AS GROUPER,ITEM,METAL"
        'StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & ""
        'StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & ""
        'If chkWithEmptyCommision.Checked = False Then StrSql += vbCrLf + " WHERE ISNULL(COMMISION,0) <> 0"
        StrSql = vbCrLf + " SELECT PARTICULAR,COMMISION"
        StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),CASE WHEN (((CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SPCS,0) ELSE ISNULL(SAMOUNT,0)END)-(CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(RPCS,0) ELSE ISNULL(RAMOUNT,0)END)/CASE WHEN EI.WEIGHT=0 THEN 1 ELSE EI.WEIGHT END)*100) BETWEEN 90 AND 99 THEN (((ISNULL(SABACKAMT,0)-ISNULL(SRBACKAMT,0))/100)*" & insper & ") "
        StrSql += vbCrLf + " WHEN (((CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SPCS,0) ELSE ISNULL(SAMOUNT,0)END)-(CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(RPCS,0) ELSE ISNULL(RAMOUNT,0)END)/CASE WHEN EI.WEIGHT=0 THEN 1 ELSE EI.WEIGHT END)*100) >99 THEN ISNULL(SABACKAMT,0)-ISNULL(SRBACKAMT,0) ELSE 0 END) BACKINC"
        StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),CASE WHEN (((CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SPCS,0) ELSE ISNULL(SAMOUNT,0)END-(CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(RPCS,0) ELSE ISNULL(RAMOUNT,0)END))/CASE WHEN EI.WEIGHT=0 THEN 1 ELSE EI.WEIGHT END)*100) BETWEEN 90 AND 99 THEN (((ISNULL(SALESCOMM,0)-ISNULL(SALESCOMM,0))/100)*" & insper & ")+(ISNULL(SABACKAMT,0)-ISNULL(SRBACKAMT,0)) "
        StrSql += vbCrLf + " WHEN (((CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SPCS,0) ELSE ISNULL(SAMOUNT,0)END-(CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(RPCS,0) ELSE ISNULL(RAMOUNT,0)END))/CASE WHEN EI.WEIGHT=0 THEN 1 ELSE EI.WEIGHT END)*100) >99 THEN ISNULL(SALESCOMM,0)-ISNULL(SALESCOMM,0)+(ISNULL(SABACKAMT,0)-ISNULL(SRBACKAMT,0)) ELSE 0 END) BACKCOMM"
        StrSql += vbCrLf + " ,SALESCOMM,OTHERCOMM"
        StrSql += vbCrLf + " ,SUBITEMNAME,TRANNO,TRANDATE,TAGNO " & IIf(chkWithTagno.Checked, ",0 TAGNOS", "") & ""
        StrSql += vbCrLf + " ,SPCS,SGRSWT,SNETWT"
        If rbtDetailed.Checked Then
            StrSql += vbCrLf + " ,SWASTPER"
        End If
        If StoneDetail = True Then
            StrSql += vbCrLf + " ,SSTNPCS,SSTNWT_G,SSTNWT_C,SDIAPCS,SDIAWT_C"
        End If
        StrSql += vbCrLf + " ,SAMOUNT,RPCS,RGRSWT,RNETWT"
        If StoneDetail = True Then
            StrSql += vbCrLf + " ,RSTNPCS,RSTNWT_G,RSTNWT_C,RDIAPCS,RDIAWT_C"
        End If
        StrSql += vbCrLf + " ,RAMOUNT,EMPNAME,COUNTER"
        StrSql += vbCrLf + " ,SEP,COLHEAD,RESULT--,EMPID,COSTID,MNTH"
        StrSql += vbCrLf + " ,GROUPER,ITEM,METAL"
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & ""
        StrSql += vbCrLf + " FROM ("
        StrSql += vbCrLf + " SELECT CONVERT(VARCHAR(300)," & particular & ")PARTICULAR"
        StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' AND COMMISION <> 0 THEN -1*COMMISION WHEN SEP = 'SA' AND COMMISION > 0 THEN COMMISION ELSE NULL END AS COMMISION"
        StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' AND SALESCOMM <> 0 THEN -1*SALESCOMM WHEN SEP = 'SA' AND SALESCOMM > 0 THEN SALESCOMM ELSE NULL END AS SALESCOMM"
        StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' AND OTHERCOMM <> 0 THEN -1*OTHERCOMM WHEN SEP = 'SA' AND OTHERCOMM > 0 THEN OTHERCOMM ELSE NULL END AS OTHERCOMM"
        StrSql += vbCrLf + " ,SUBITEMNAME,TRANNO,TRANDATE,TAGNO " & IIf(chkWithTagno.Checked, ",0 TAGNOS", "") & ""
        StrSql += vbCrLf + " ,CASE WHEN SEP = 'SA' THEN PCS ELSE NULL END AS SPCS"
        StrSql += vbCrLf + " ,CASE WHEN SEP = 'SA' THEN GRSWT ELSE NULL END AS SGRSWT"
        StrSql += vbCrLf + " ,CASE WHEN SEP = 'SA' THEN NETWT ELSE NULL END AS SNETWT"
        If rbtDetailed.Checked Then
            StrSql += vbCrLf + " ,CASE WHEN SEP = 'SA' THEN WASTPER ELSE NULL END AS SWASTPER"
        End If
        If StoneDetail = True Then
            StrSql += vbCrLf + " ,CASE WHEN SEP = 'SA' THEN STNPCS ELSE NULL END AS SSTNPCS"
            StrSql += vbCrLf + " ,CASE WHEN SEP = 'SA' THEN STNWT_G ELSE NULL END AS SSTNWT_G"
            StrSql += vbCrLf + " ,CASE WHEN SEP = 'SA' THEN STNWT_C ELSE NULL END AS SSTNWT_C"
            StrSql += vbCrLf + " ,CASE WHEN SEP = 'SA' THEN DIAPCS ELSE NULL END AS SDIAPCS"
            StrSql += vbCrLf + " ,CASE WHEN SEP = 'SA' THEN DIAWT_C ELSE NULL END AS SDIAWT_C"
        End If
        StrSql += vbCrLf + " ,CASE WHEN SEP = 'SA' THEN S.AMOUNT ELSE NULL END AS SAMOUNT"
        StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' THEN PCS ELSE NULL END AS RPCS"
        StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' THEN GRSWT ELSE NULL END AS RGRSWT"
        StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' THEN NETWT ELSE NULL END AS RNETWT"
        If StoneDetail = True Then
            StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' THEN STNPCS ELSE NULL END AS RSTNPCS"
            StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' THEN STNWT_G ELSE NULL END AS RSTNWT_G"
            StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' THEN STNWT_C ELSE NULL END AS RSTNWT_C"
            StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' THEN DIAPCS ELSE NULL END AS RDIAPCS"
            StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' THEN DIAWT_C ELSE NULL END AS RDIAWT_C"
        End If
        StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' THEN S.AMOUNT ELSE NULL END AS RAMOUNT"
        StrSql += vbCrLf + " ,CASE WHEN SEP='SA' THEN (ISNULL(BI.AMOUNT,0)*(CASE WHEN ISNULL(S.METAL,'') IN('OTHERS','PLATINUM') THEN S.PCS ELSE S.NETWT END)) ELSE 0 END SABACKAMT "
        StrSql += vbCrLf + " ,CASE WHEN SEP='SR' THEN (ISNULL(BI.AMOUNT,0)*(CASE WHEN ISNULL(S.METAL,'') IN('OTHERS','PLATINUM') THEN S.PCS ELSE S.NETWT END)) ELSE 0 END SRBACKAMT"
        StrSql += vbCrLf + " ,CONVERT(VARCHAR(100),EMPNAME)AS EMPNAME,CONVERT(VARCHAR(100),ITEMCTRNAME) AS COUNTER"
        StrSql += vbCrLf + " ,CONVERT(VARCHAR(2),SEP)SEP,CONVERT(VARCHAR(3),'')AS COLHEAD,CONVERT(INT,1)AS RESULT"
        StrSql += vbCrLf + " ,CONVERT(VARCHAR(200)," & grouper & ")AS GROUPER,ITEM,METAL"
        StrSql += vbCrLf + " ,EMPID,S.COSTID"
        StrSql += vbCrLf + " ,CONVERT(VARCHAR(15),'2013-'+ SUBSTRING(DATENAME(MONTH,TRANDATE),0,4)+'-01')MNTH"
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " S"
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..BACKENDINCENTIVE AS BI ON BI.ITEMID=S.ITEMID AND DATENAME(MONTH,S.TRANDATE)=BI.MONTH "
        StrSql += vbCrLf + " )X"
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..EMPWISEINCENTIVE AS EI ON EI.EMPID=X.EMPID AND EI.COSTID=X.COSTID AND EI.METALID=(SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME=X.METAL) "
        StrSql += vbCrLf + " AND EI.MONTH=DATENAME(MONTH,X.MNTH)"
        If chkWithEmptyCommision.Checked = False Then StrSql += vbCrLf + " WHERE ISNULL(COMMISION,0) <> 0"

        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        If rbtDetailed.Checked Then
            StrSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & ")>0"
            StrSql += vbCrLf + " BEGIN"
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & "(PARTICULAR,COLHEAD,RESULT,GROUPER," & IIf(rbtGrpCounter.Checked, "COUNTER", grouper) & ")"
            StrSql += vbCrLf + " SELECT DISTINCT GROUPER,'T',0 RESULT,GROUPER," & IIf(rbtGrpCounter.Checked, "COUNTER", grouper) & " FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " WHERE RESULT = 1"
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & "(PARTICULAR,COLHEAD,RESULT,GROUPER," & IIf(rbtGrpCounter.Checked, "COUNTER", grouper) & ",COMMISION,BACKINC,BACKCOMM,SALESCOMM,OTHERCOMM,SPCS,SGRSWT,SNETWT"
            If StoneDetail = True Then
                StrSql += vbCrLf + " ,SSTNPCS,SSTNWT_G,SSTNWT_C,SDIAPCS,SDIAWT_C"
            End If
            StrSql += vbCrLf + " ,SAMOUNT,RPCS,RGRSWT,RNETWT"
            If StoneDetail = True Then
                StrSql += vbCrLf + " ,RSTNPCS,RSTNWT_G,RSTNWT_C,RDIAPCS,RDIAWT_C"
            End If
            StrSql += vbCrLf + " ,RAMOUNT)"
            StrSql += vbCrLf + " SELECT GROUPER + ' TOT','S',2 RESULT,GROUPER," & IIf(rbtGrpCounter.Checked, "COUNTER", grouper) & ",SUM(COMMISION),SUM(BACKINC),SUM(BACKCOMM),SUM(SALESCOMM),SUM(OTHERCOMM),SUM(SPCS),SUM(SGRSWT),SUM(SNETWT)"
            If StoneDetail = True Then
                StrSql += vbCrLf + " ,SUM(SSTNPCS),SUM(SSTNWT_G),SUM(SSTNWT_C),SUM(SDIAPCS),SUM(SDIAWT_C)"
            End If
            StrSql += vbCrLf + " ,SUM(SAMOUNT),SUM(RPCS),SUM(RGRSWT),SUM(RNETWT)"
            If StoneDetail = True Then
                StrSql += vbCrLf + " ,SUM(RSTNPCS),SUM(RSTNWT_G),SUM(RSTNWT_C),SUM(RDIAPCS),SUM(RDIAWT_C)"
            End If
            StrSql += vbCrLf + " ,SUM(RAMOUNT)"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " WHERE RESULT = 1 GROUP BY GROUPER," & IIf(rbtGrpCounter.Checked, "COUNTER", grouper) & ""
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & "(PARTICULAR,COLHEAD,RESULT,GROUPER,COMMISION,BACKINC,BACKCOMM,SALESCOMM,OTHERCOMM,SPCS,SGRSWT,SNETWT"
            If StoneDetail = True Then
                StrSql += vbCrLf + " ,SSTNPCS,SSTNWT_G,SSTNWT_C,SDIAPCS,SDIAWT_C"
            End If
            StrSql += vbCrLf + " ,SAMOUNT,RPCS,RGRSWT,RNETWT"
            If StoneDetail = True Then
                StrSql += vbCrLf + " ,RSTNPCS,RSTNWT_G,RSTNWT_C,RDIAPCS,RDIAWT_C"
            End If
            StrSql += vbCrLf + " ,RAMOUNT)"
            StrSql += vbCrLf + " SELECT 'GRAND TOTAL','G',3 RESULT,'ZZZZZZZ' AS GROUPER,SUM(COMMISION),SUM(BACKINC),SUM(BACKCOMM),SUM(SALESCOMM),SUM(OTHERCOMM),SUM(SPCS),SUM(SGRSWT),SUM(SNETWT)"
            If StoneDetail = True Then
                StrSql += vbCrLf + " ,SUM(SSTNPCS),SUM(SSTNWT_G),SUM(SSTNWT_C),SUM(SDIAPCS),SUM(SDIAWT_C)"
            End If
            StrSql += vbCrLf + " ,SUM(SAMOUNT),SUM(RPCS),SUM(RGRSWT),SUM(RNETWT)"
            If StoneDetail = True Then
                StrSql += vbCrLf + " ,SUM(RSTNPCS),SUM(RSTNWT_G),SUM(RSTNWT_C),SUM(RDIAPCS),SUM(RDIAWT_C)"
            End If
            StrSql += vbCrLf + " ,SUM(RAMOUNT)"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " WHERE RESULT = 1"
            StrSql += vbCrLf + " END"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        Else

            StrSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & ")>0"
            StrSql += vbCrLf + " BEGIN"
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & "(PARTICULAR,COLHEAD,RESULT,GROUPER," & IIf(rbtGrpCounter.Checked, "COUNTER", grouper) & ",COMMISION" & IIf(chkWithTagno.Checked, ",TAGNOS", "") & ",SALESCOMM,BACKINC,BACKCOMM,OTHERCOMM,SPCS,SGRSWT,SNETWT"
            If StoneDetail = True Then
                StrSql += vbCrLf + " ,SSTNPCS,SSTNWT_G,SSTNWT_C,SDIAPCS,SDIAWT_C"
            End If
            StrSql += vbCrLf + " ,RPCS,RGRSWT,RNETWT"
            If StoneDetail = True Then
                StrSql += vbCrLf + " ,RSTNPCS,RSTNWT_G,RSTNWT_C,RDIAPCS,RDIAWT_C"
            End If
            StrSql += vbCrLf + " ,SAMOUNT,RAMOUNT"
            StrSql += vbCrLf + " )"
            StrSql += vbCrLf + " SELECT GROUPER ,'',2 RESULT,GROUPER," & IIf(rbtGrpCounter.Checked, "COUNTER", grouper) & ",SUM(COMMISION)"
            StrSql += vbCrLf + IIf(chkWithTagno.Checked, ",(SELECT COUNT(TAGNO) FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " WHERE TAGNO <>'' AND GROUPER=S.GROUPER)", "") & ""
            StrSql += vbCrLf + " ,SUM(SALESCOMM),SUM(BACKINC),SUM(BACKCOMM),SUM(OTHERCOMM),SUM(SPCS),SUM(SGRSWT),SUM(SNETWT)"
            If StoneDetail = True Then
                StrSql += vbCrLf + " ,SUM(SSTNPCS),SUM(SSTNWT_G),SUM(SSTNWT_C),SUM(SDIAPCS),SUM(SDIAWT_C)"
            End If
            StrSql += vbCrLf + " ,SUM(RPCS),SUM(RGRSWT),SUM(RNETWT)"
            If StoneDetail = True Then
                StrSql += vbCrLf + " ,SUM(RSTNPCS),SUM(RSTNWT_G),SUM(RSTNWT_C),SUM(RDIAPCS),SUM(RDIAWT_C)"
            End If
            StrSql += vbCrLf + " ,SUM(SAMOUNT),SUM(RAMOUNT)"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " S WHERE RESULT = 1 GROUP BY GROUPER," & IIf(rbtGrpCounter.Checked, "COUNTER", grouper) & ""


            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & "(PARTICULAR,COLHEAD,RESULT,GROUPER,COMMISION" & IIf(chkWithTagno.Checked, ",TAGNOS", "") & ",SALESCOMM,BACKINC,BACKCOMM,OTHERCOMM,SPCS,SGRSWT,SNETWT"
            If StoneDetail = True Then
                StrSql += vbCrLf + " ,SSTNPCS,SSTNWT_G,SSTNWT_C,SDIAPCS,SDIAWT_C"
            End If
            StrSql += vbCrLf + " ,RPCS,RGRSWT,RNETWT"
            If StoneDetail = True Then
                StrSql += vbCrLf + " ,RSTNPCS,RSTNWT_G,RSTNWT_C,RDIAPCS,RDIAWT_C"
            End If
            StrSql += vbCrLf + " ,SAMOUNT,RAMOUNT"
            StrSql += vbCrLf + " )"
            StrSql += vbCrLf + " SELECT 'GRAND TOTAL','G',3 RESULT,'ZZZZZZZ' AS GROUPER,SUM(COMMISION)" & IIf(chkWithTagno.Checked, ",0", "") & ",SUM(SALESCOMM),SUM(BACKINC),SUM(BACKCOMM),SUM(OTHERCOMM),SUM(SPCS),SUM(SGRSWT),SUM(SNETWT)"
            If StoneDetail = True Then
                StrSql += vbCrLf + " ,SUM(SSTNPCS),SUM(SSTNWT_G),SUM(SSTNWT_C),SUM(SDIAPCS),SUM(SDIAWT_C)"
            End If
            StrSql += vbCrLf + " ,SUM(RPCS),SUM(RGRSWT),SUM(RNETWT)"
            If StoneDetail = True Then
                StrSql += vbCrLf + " ,SUM(RSTNPCS),SUM(RSTNWT_G),SUM(RSTNWT_C),SUM(RDIAPCS),SUM(RDIAWT_C)"
            End If
            StrSql += vbCrLf + " ,SUM(SAMOUNT),SUM(RAMOUNT)"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " WHERE RESULT = 1"
            StrSql += vbCrLf + " END"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
            StrSql = " DELETE FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " WHERE RESULT = 1"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        End If
        StrSql = vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & " ORDER BY " & IIf(chkdesc.Checked, IIf(rbtGrpCounter.Checked, "COUNTER", grouper) & " DESC ", "GROUPER") & ""
        StrSql += vbCrLf + ",RESULT,SEP " & IIf(rbtproduct.Checked = False, ",ITEM", "") & " " & IIf(chkdesc.Checked, " DESC ", "") & ",TRANDATE,TRANNO"
        Dim dtSource As New DataTable
        dtSource.Columns.Add("KEYNO", GetType(Integer))
        dtSource.Columns("KEYNO").AutoIncrement = True
        dtSource.Columns("KEYNO").AutoIncrementSeed = 0
        dtSource.Columns("KEYNO").AutoIncrementStep = 1
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtSource)

        Dim objGridShower As New frmGridDispDia
        objGridShower.gridView.DataSource = Nothing
        objGridShower.gridView.DataSource = dtSource
        objGridShower.Name = Me.Name
        objGridShower.WindowState = FormWindowState.Maximized
        objGridShower.gridView.Name = Me.Name
        objGridShower.gridView.Columns("PARTICULAR").Visible = True
        objGridShower.gridView.Columns("EMPNAME").Visible = Not rbtGrpEmployee.Checked
        objGridShower.gridView.Columns("COUNTER").Visible = Not rbtGrpCounter.Checked
        objGridShower.gridView.Columns("KEYNO").Visible = False
        objGridShower.gridView.Columns("COLHEAD").Visible = False
        objGridShower.gridView.Columns("KEYNO").Visible = False
        objGridShower.gridView.Columns("RESULT").Visible = False
        objGridShower.gridView.Columns("ITEM").Visible = False
        objGridShower.gridView.Columns("GROUPER").Visible = False
        objGridShower.gridView.Columns("SEP").Visible = False
        objGridShower.gridView.Columns("RESULT").Visible = False
        objGridShower.gridView.Columns("BACKINC").Visible = False
        If Val(txtCommPercentage_AMT.Text) <> 0 Then
            objGridShower.gridView.Columns("SALESCOMM").Visible = True
            objGridShower.gridView.Columns("OTHERCOMM").Visible = True
            objGridShower.gridView.Columns("SALESCOMM").HeaderText = "SALES COUNTER COMM"
            objGridShower.gridView.Columns("OTHERCOMM").HeaderText = "OTHER COUNTER COMM"
        Else
            objGridShower.gridView.Columns("SALESCOMM").Visible = False
            objGridShower.gridView.Columns("OTHERCOMM").Visible = False
        End If
        objGridShower.gridView.Columns(IIf(rbtGrpCounter.Checked, "COUNTER", grouper)).Visible = False


        objGridShower.gridView.Columns("SUBITEMNAME").Visible = rbtDetailed.Checked
        objGridShower.gridView.Columns("TAGNO").Visible = rbtDetailed.Checked
        objGridShower.gridView.Columns("TRANNO").Visible = rbtDetailed.Checked
        objGridShower.gridView.Columns("TRANDATE").Visible = rbtDetailed.Checked
        If rbtSummary.Checked Then
            objGridShower.gridView.Columns("EMPNAME").Visible = False
            objGridShower.gridView.Columns("COUNTER").Visible = False
        End If


        FormatGridColumns(objGridShower.gridView, False, False, True, False)
        objGridShower.gridView.Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"

        objGridShower.gridView.Columns("SPCS").HeaderText = "SALES PCS"
        objGridShower.gridView.Columns("SGRSWT").HeaderText = "SALES GRSWT"
        objGridShower.gridView.Columns("SNETWT").HeaderText = "SALES NETWT"
        If StoneDetail = True Then
            objGridShower.gridView.Columns("SSTNPCS").HeaderText = "SALES STNPCS"
            objGridShower.gridView.Columns("SSTNWT_G").HeaderText = "SALES STNWT_G"
            objGridShower.gridView.Columns("SSTNWT_C").HeaderText = "SALES STNWT_C"
            objGridShower.gridView.Columns("SDIAPCS").HeaderText = "SALES DIAPCS"
            objGridShower.gridView.Columns("SDIAWT_C").HeaderText = "SALES DIAWT_C"
        End If
        objGridShower.gridView.Columns("SAMOUNT").HeaderText = "SALES AMOUNT"
        objGridShower.gridView.Columns("RPCS").HeaderText = "RETURN PCS"
        objGridShower.gridView.Columns("RGRSWT").HeaderText = "RETURN GRSWT"
        objGridShower.gridView.Columns("RNETWT").HeaderText = "RETURN NETWT"
        If StoneDetail = True Then
            objGridShower.gridView.Columns("RSTNPCS").HeaderText = "RETURN STNPCS"
            objGridShower.gridView.Columns("RSTNWT_G").HeaderText = "RETURN STNWT_G"
            objGridShower.gridView.Columns("RSTNWT_C").HeaderText = "RETURN STNWT_C"
            objGridShower.gridView.Columns("RDIAPCS").HeaderText = "RETURN DIAPCS"
            objGridShower.gridView.Columns("RDIAWT_C").HeaderText = "RETURN DIAWT_C"
            For cnt As Integer = 0 To objGridShower.gridView.Rows.Count - 1
                If objGridShower.gridView.Rows(cnt).Cells("COLHEAD").Value.ToString = "G" Then
                    If Val(objGridShower.gridView.Rows(cnt).Cells("RSTNPCS").Value.ToString) = "0.0" Then objGridShower.gridView.Columns("RSTNPCS").Visible = False
                    If Val(objGridShower.gridView.Rows(cnt).Cells("RSTNWT_G").Value.ToString) = "0.0" Then objGridShower.gridView.Columns("RSTNWT_G").Visible = False
                    If Val(objGridShower.gridView.Rows(cnt).Cells("RSTNWT_C").Value.ToString) = "0.0" Then objGridShower.gridView.Columns("RSTNWT_C").Visible = False
                    If Val(objGridShower.gridView.Rows(cnt).Cells("RDIAPCS").Value.ToString) = "0.0" Then objGridShower.gridView.Columns("RDIAPCS").Visible = False
                    If Val(objGridShower.gridView.Rows(cnt).Cells("RDIAWT_C").Value.ToString) = "0.0" Then objGridShower.gridView.Columns("RDIAWT_C").Visible = False

                    If Val(objGridShower.gridView.Rows(cnt).Cells("SSTNPCS").Value.ToString) = "0.0" Then objGridShower.gridView.Columns("SSTNPCS").Visible = False
                    If Val(objGridShower.gridView.Rows(cnt).Cells("SSTNWT_G").Value.ToString) = "0.0" Then objGridShower.gridView.Columns("SSTNWT_G").Visible = False
                    If Val(objGridShower.gridView.Rows(cnt).Cells("SSTNWT_C").Value.ToString) = "0.0" Then objGridShower.gridView.Columns("SSTNWT_C").Visible = False
                    If Val(objGridShower.gridView.Rows(cnt).Cells("SDIAPCS").Value.ToString) = "0.0" Then objGridShower.gridView.Columns("SDIAPCS").Visible = False
                    If Val(objGridShower.gridView.Rows(cnt).Cells("SDIAWT_C").Value.ToString) = "0.0" Then objGridShower.gridView.Columns("SDIAWT_C").Visible = False
                End If
            Next
        End If
        objGridShower.gridView.Columns("RAMOUNT").HeaderText = "RETURN AMOUNT"
        Dim tit As String
        If rbtGrpEmployee.Checked Then
            tit = "EMPLOYEE"
        ElseIf rbtproduct.Checked Then
            tit = "PRODUCT"
        ElseIf rbtGrpCounter.Checked Then
            tit = "COUNTER"
        Else
            tit = "METAL"
        End If
        objGridShower.Text = tit & " WISE SALES COMMISION"
        tit += " WISE SALES COMMISION" + vbCrLf
        tit += " DATE FROM " & dtpFrom.Text + " TO " + dtpTo.Text
        tit += IIf(chkCmbCostCentre.Text <> "" And chkCmbCostCentre.Text <> "ALL", " :" & chkCmbCostCentre.Text, "")
        If chkcmbmetal.Text <> "ALL" And chkcmbmetal.Text <> "" Then
            tit += " FOR METAL " & chkcmbmetal.Text & ""
        End If
        If chkcmbCounter.Text <> "ALL" And chkcmbCounter.Text <> "" Then
            tit += "(" & chkcmbCounter.Text & ")"
        End If
        If chkcmbitemname.Text <> "ALL" And chkcmbitemname.Text <> "" Then
            tit += "(" & chkcmbitemname.Text & ")"
        End If

        If chkcmbsubitem.Text <> "ALL" And chkcmbsubitem.Text <> "" Then
            tit += Environment.NewLine + "(" & chkcmbsubitem.Text & ")"
        End If

        If chkcmbemployee.Text <> "ALL" And chkcmbemployee.Text <> "" Then
            tit += vbCrLf & "FOR " & chkcmbemployee.Text
        End If
        objGridShower.pnlHeader.Height = objGridShower.lblTitle.Height + 50
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        objGridShower.Show()
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView, "PARTICULAR")
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
    End Sub

#Region "CORRECTION FOR NATHELLA."
    'WE HAVE ADD THE OPTION MONTHWISE REPORT
    Private Sub GroupingGridbymonthwise()
        Dim particular As String = ""

        If rbtSummary.Checked Then
            particular = "X.EMPID,X.EMPNAME,X.METAL"
        ElseIf rbtDetailed.Checked Then
            particular = "X.ITEMID,X.ITEM,X.EMPID,X.EMPNAME,X.METAL"
        End If
        StrSql = "  IF OBJECT_ID('TEMPTABLEDB..SA_COMMISION" & Sysid & "')IS NOT NULL DROP TABLE TEMPTABLEDB..SA_COMMISION" & Sysid & ""
        StrSql += vbCrLf + "  SELECT " & particular & ",DATENAME(MONTH,X.MNTH)MNTHNAME,MNTH,EI.WEIGHT TARGET"
        StrSql += vbCrLf + "  ,SUM((CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SAPCS,0) ELSE 0 END))SAPCS"
        StrSql += vbCrLf + "  ,SUM((CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SRPCS,0) ELSE 0  END))SRPCS "
        StrSql += vbCrLf + "  ,SUM((CASE WHEN ISNULL(METAL,'') NOT IN('OTHERS')THEN ISNULL(SASALES,0)ELSE 0 END))SASALES"
        StrSql += vbCrLf + "  ,SUM((CASE WHEN ISNULL(METAL,'') NOT IN('OTHERS')THEN ISNULL(SRSALES,0)ELSE 0 END))SRSALES "
        StrSql += vbCrLf + "  ,SUM(ISNULL(SACOMMISION,0))SACOMMISION"
        StrSql += vbCrLf + "  ,SUM(ISNULL(SRCOMMISION,0))SRCOMMISION"
        StrSql += vbCrLf + "  ,SUM(ISNULL(SABACKAMT,0))SABACKAMT"
        StrSql += vbCrLf + "  ,SUM(ISNULL(SRBACKAMT,0))SRBACKAMT"
        StrSql += vbCrLf + "  ,CASE WHEN SUM((((CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SAPCS,0) ELSE ISNULL(SASALES,0)END)-(CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SRPCS,0) ELSE ISNULL(SRSALES,0)END))/EI.WEIGHT)*100) BETWEEN 90 AND 99 THEN SUM((((ISNULL(SACOMMISION,0)-ISNULL(SRCOMMISION,0))/100)*" & insper & ")) "
        StrSql += vbCrLf + "  WHEN SUM((((CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SAPCS,0) ELSE ISNULL(SASALES,0)END)-(CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SRPCS,0) ELSE ISNULL(SRSALES,0)END))/EI.WEIGHT)*100) >99 THEN SUM(ISNULL(SACOMMISION,0)-ISNULL(SRCOMMISION,0)) ELSE 0 END STFSALESINC"
        StrSql += vbCrLf + "  ,CASE WHEN SUM((((CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SAPCS,0) ELSE ISNULL(SASALES,0)END)-(CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SRPCS,0) ELSE ISNULL(SRSALES,0)END))/EI.WEIGHT)*100) BETWEEN 90 AND 99 THEN SUM((((ISNULL(SABACKAMT,0)-ISNULL(SRBACKAMT,0))/100)*" & insper & ")) "
        StrSql += vbCrLf + "  WHEN SUM((((CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SAPCS,0) ELSE ISNULL(SASALES,0)END)-(CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SRPCS,0) ELSE ISNULL(SRSALES,0)END))/EI.WEIGHT)*100) >99 THEN SUM(ISNULL(SABACKAMT,0)-ISNULL(SRBACKAMT,0)) ELSE 0 END STFBACKINC"
        StrSql += vbCrLf + "  ,CASE WHEN SUM((((CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SAPCS,0) ELSE ISNULL(SASALES,0)END)-(CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SRPCS,0) ELSE ISNULL(SRSALES,0)END))/EI.WEIGHT)*100) BETWEEN 90 AND 99 THEN SUM((((ISNULL(SACOMMISION,0)-ISNULL(SRCOMMISION,0))/100)*" & insper & ")+(ISNULL(SABACKAMT,0)-ISNULL(SRBACKAMT,0))) "
        StrSql += vbCrLf + "  WHEN SUM((((CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SAPCS,0) ELSE ISNULL(SASALES,0)END)-(CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SRPCS,0) ELSE ISNULL(SRSALES,0)END))/EI.WEIGHT)*100) >99 THEN SUM(ISNULL(SACOMMISION,0)-ISNULL(SRCOMMISION,0)+(ISNULL(SABACKAMT,0)-ISNULL(SRBACKAMT,0))) ELSE 0 END COMMISSION"
        StrSql += vbCrLf + "  ,CONVERT(DECIMAL(13,2),SUM((((CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SAPCS,0) ELSE ISNULL(SASALES,0)END)-(CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SRPCS,0) ELSE ISNULL(SRSALES,0)END))/EI.WEIGHT)*100))PER"
        StrSql += vbCrLf + "  INTO TEMPTABLEDB..SA_COMMISION" & Sysid & " "
        StrSql += vbCrLf + "  FROM("
        StrSql += vbCrLf + "  SELECT S.ITEMID,S.ITEM,S.EMPID,S.EMPNAME,S.METAL,CONVERT(VARCHAR(15),'2013-'+ SUBSTRING(DATENAME(MONTH,TRANDATE),0,4)+'-01')MNTH"
        StrSql += vbCrLf + "  ,CASE WHEN SEP='SA' THEN (S.NETWT)ELSE 0 END SASALES "
        StrSql += vbCrLf + "  ,CASE WHEN SEP='SR' THEN (S.NETWT)ELSE 0 END SRSALES "
        StrSql += vbCrLf + "  ,CASE WHEN SEP='SA' THEN (S.PCS)ELSE 0 END SAPCS "
        StrSql += vbCrLf + "  ,CASE WHEN SEP='SR' THEN (S.PCS)ELSE 0 END SRPCS "
        StrSql += vbCrLf + "  ,CASE WHEN SEP='SA' THEN (S.COMMISION)ELSE 0 END SACOMMISION "
        StrSql += vbCrLf + "  ,CASE WHEN SEP='SR' THEN (S.COMMISION)ELSE 0 END SRCOMMISION "
        StrSql += vbCrLf + "  ,CASE WHEN SEP='SA' THEN (ISNULL(BI.AMOUNT,0)*(CASE WHEN ISNULL(S.METAL,'') IN('OTHERS','PLATINUM') THEN S.PCS ELSE S.NETWT END)) ELSE 0 END SABACKAMT "
        StrSql += vbCrLf + "  ,CASE WHEN SEP='SR' THEN (ISNULL(BI.AMOUNT,0)*(CASE WHEN ISNULL(S.METAL,'') IN('OTHERS','PLATINUM') THEN S.PCS ELSE S.NETWT END)) ELSE 0 END SRBACKAMT"
        StrSql += vbCrLf + "  ,S.COSTID FROM TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " S"
        StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..BACKENDINCENTIVE AS BI ON BI.ITEMID=S.ITEMID"
        StrSql += vbCrLf + "  AND DATENAME(MONTH,S.TRANDATE)=BI.MONTH "
        If chkWithEmptyCommision.Checked = False Then StrSql += vbCrLf + " WHERE ISNULL(S.COMMISION,0) <> 0"
        StrSql += vbCrLf + "  )X"
        StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..EMPWISEINCENTIVE AS EI ON EI.EMPID=X.EMPID AND EI.COSTID=X.COSTID AND EI.METALID=(SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME=X.METAL) "
        StrSql += vbCrLf + "  AND EI.MONTH=DATENAME(MONTH,X.MNTH)"
        StrSql += vbCrLf + "  AND ISNULL(EI.WEIGHT,0)<>0"
        StrSql += vbCrLf + "  GROUP BY " & particular & ",X.MNTH,EI.WEIGHT"
        StrSql += vbCrLf + "  HAVING SUM(ISNULL(SACOMMISION,0))<>0 "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()


        StrSql = "  IF OBJECT_ID('TEMPTABLEDB..TEMP_OUT" & Sysid & "')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_OUT" & Sysid & ""
        StrSql += vbCrLf + "  SELECT DISTINCT EMPID,CONVERT(VARCHAR(200),EMPNAME + '['+ CONVERT(VARCHAR(4),EMPID) +']')PARTICULAR,EMPNAME,METAL,2 RESULT,'D'"
        StrSql += vbCrLf + "  COLHEAD INTO TEMPTABLEDB..TEMP_OUT" & Sysid & " FROM TEMPTABLEDB..SA_COMMISION" & Sysid & " "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        StrSql = "SELECT COUNT(*) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & ""
        Dim CNT As Integer = Val(GetSqlValue(cn, StrSql))
        If CNT > 0 Then
            StrSql = "  INSERT INTO TEMPTABLEDB..TEMP_OUT" & Sysid & "(PARTICULAR,METAL,RESULT,COLHEAD)"
            StrSql += vbCrLf + "  SELECT DISTINCT METAL,METAL,0,'T' FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE RESULT=2"
            StrSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP_OUT" & Sysid & "(PARTICULAR,METAL,RESULT,COLHEAD)     "
            StrSql += vbCrLf + "  SELECT 'SUBTOTAL',METAL,3,'S' FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE RESULT=0"
            StrSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP_OUT" & Sysid & "(PARTICULAR,METAL,RESULT,COLHEAD)"
            StrSql += vbCrLf + "  SELECT 'GRANDTOTAL','ZZZZZ',4,'G' "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

            StrSql = "  DECLARE @QRY VARCHAR(4000)"
            StrSql += vbCrLf + "  SET @QRY=''"
            StrSql += vbCrLf + "  DECLARE @A AS VARCHAR(10)"
            StrSql += vbCrLf + "  DECLARE @MNTH AS VARCHAR(10)"
            StrSql += vbCrLf + "  DECLARE CUR CURSOR FOR SELECT DISTINCT DATEPART(MM,CONVERT(SMALLDATETIME,MNTH))A,DATENAME(MONTH,MNTH)MNTH FROM TEMPTABLEDB..SA_COMMISION" & Sysid & " ORDER BY DATEPART(MM,CONVERT(SMALLDATETIME,MNTH))"
            StrSql += vbCrLf + "  OPEN CUR WHILE (1 = 1) BEGIN FETCH NEXT FROM CUR INTO @A,@MNTH"
            StrSql += vbCrLf + "  IF @@FETCH_STATUS =-1 BREAK                        "
            StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT" & Sysid & " ADD ['+ SUBSTRING(@MNTH,0,4) +'-TARGET] DECIMAL(15,3) DEFAULT 0' PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT" & Sysid & " ADD ['+ SUBSTRING(@MNTH,0,4)+'-SAPCS] DECIMAL(15,3) DEFAULT 0' PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT" & Sysid & " ADD ['+ SUBSTRING(@MNTH,0,4)+'-SALES] DECIMAL(15,3) DEFAULT 0' PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT" & Sysid & " ADD ['+ SUBSTRING(@MNTH,0,4)+'-SARETURN] DECIMAL(15,3) DEFAULT 0' PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT" & Sysid & " ADD ['+ SUBSTRING(@MNTH,0,4)+'-SARETURNPCS] DECIMAL(15,3) DEFAULT 0' PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT" & Sysid & " ADD ['+ SUBSTRING(@MNTH,0,4)+'-SACOMMISION] DECIMAL(15,2) DEFAULT 0' PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT" & Sysid & " ADD ['+ SUBSTRING(@MNTH,0,4)+'-SRCOMMISION] DECIMAL(15,2) DEFAULT 0' PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT" & Sysid & " ADD ['+ SUBSTRING(@MNTH,0,4)+'-SABACKAMT] DECIMAL(15,2) DEFAULT 0' PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT" & Sysid & " ADD ['+ SUBSTRING(@MNTH,0,4)+'-SRBACKAMT] DECIMAL(15,2) DEFAULT 0' PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT" & Sysid & " ADD ['+ SUBSTRING(@MNTH,0,4)+'-PER] DECIMAL(15,2) DEFAULT 0' PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT" & Sysid & " ADD ['+ SUBSTRING(@MNTH,0,4)+'-STFSALESINC] DECIMAL(15,3) DEFAULT 0' PRINT @QRY EXEC(@QRY)"
            StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT" & Sysid & " ADD ['+ SUBSTRING(@MNTH,0,4)+'-STFBACKINC] DECIMAL(15,3) DEFAULT 0' PRINT @QRY EXEC(@QRY)"
            StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT" & Sysid & " ADD ['+ SUBSTRING(@MNTH,0,4) +'-INCENTIVE] DECIMAL(15,2) DEFAULT 0' PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-TARGET]=(SELECT SUM(TARGET) FROM TEMPTABLEDB..SA_COMMISION" & Sysid & " WHERE EMPID=TT.EMPID AND METAL=TT.METAL '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE EMPID=EMPID AND METAL=METAL '"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY) "
            StrSql += vbCrLf + "  SELECT @QRY='UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SALES]=(SELECT SUM(SASALES) FROM TEMPTABLEDB..SA_COMMISION" & Sysid & " WHERE EMPID=TT.EMPID AND METAL=TT.METAL '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE EMPID=EMPID AND METAL=METAL '"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY='UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SAPCS]=(SELECT SUM(SAPCS) FROM TEMPTABLEDB..SA_COMMISION" & Sysid & " WHERE EMPID=TT.EMPID AND METAL=TT.METAL '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE EMPID=EMPID AND METAL=METAL '"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY='UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SARETURN]=(SELECT SUM(SRSALES) FROM TEMPTABLEDB..SA_COMMISION" & Sysid & " WHERE EMPID=TT.EMPID AND METAL=TT.METAL '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE EMPID=EMPID AND METAL=METAL '"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY='UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SARETURNPCS]=(SELECT SUM(SRPCS) FROM TEMPTABLEDB..SA_COMMISION" & Sysid & " WHERE EMPID=TT.EMPID AND METAL=TT.METAL '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE EMPID=EMPID AND METAL=METAL '"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY='UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SACOMMISION]=(SELECT SUM(SACOMMISION) FROM TEMPTABLEDB..SA_COMMISION" & Sysid & " WHERE EMPID=TT.EMPID AND METAL=TT.METAL '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE EMPID=EMPID AND METAL=METAL '"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY='UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SRCOMMISION]=(SELECT SUM(SRCOMMISION) FROM TEMPTABLEDB..SA_COMMISION" & Sysid & " WHERE EMPID=TT.EMPID AND METAL=TT.METAL '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE EMPID=EMPID AND METAL=METAL '"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY='UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SABACKAMT]=(SELECT SUM(SABACKAMT) FROM TEMPTABLEDB..SA_COMMISION" & Sysid & " WHERE EMPID=TT.EMPID AND METAL=TT.METAL '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE EMPID=EMPID AND METAL=METAL '"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY='UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SRBACKAMT]=(SELECT SUM(SRBACKAMT) FROM TEMPTABLEDB..SA_COMMISION" & Sysid & " WHERE EMPID=TT.EMPID AND METAL=TT.METAL '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE EMPID=EMPID AND METAL=METAL '"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY) "
            StrSql += vbCrLf + "  SELECT @QRY='UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-PER]=(SELECT SUM(PER) FROM TEMPTABLEDB..SA_COMMISION" & Sysid & " WHERE EMPID=TT.EMPID AND METAL=TT.METAL '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE EMPID=EMPID AND METAL=METAL '"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY='UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-STFSALESINC]=(SELECT SUM(STFSALESINC) FROM TEMPTABLEDB..SA_COMMISION" & Sysid & " WHERE EMPID=TT.EMPID AND METAL=TT.METAL '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE EMPID=EMPID AND METAL=METAL '"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)  "
            StrSql += vbCrLf + "  SELECT @QRY='UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-STFBACKINC]=(SELECT SUM(STFBACKINC) FROM TEMPTABLEDB..SA_COMMISION" & Sysid & " WHERE EMPID=TT.EMPID AND METAL=TT.METAL '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''' AND ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-TARGET],0)<> 0) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE EMPID=EMPID AND METAL=METAL '"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)  "
            StrSql += vbCrLf + "  SELECT @QRY='UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-INCENTIVE]=(SELECT SUM(COMMISSION) FROM TEMPTABLEDB..SA_COMMISION" & Sysid & " WHERE EMPID=TT.EMPID AND METAL=TT.METAL '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE EMPID=EMPID AND METAL=METAL '"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)  "
            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-TARGET]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-TARGET],0)) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE METAL=TT.METAL '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + 'AND COLHEAD=''D'') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE METAL=METAL AND COLHEAD=''S'''"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SALES]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SALES],0)) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE METAL=TT.METAL '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + 'AND COLHEAD=''D'') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE METAL=METAL AND COLHEAD=''S'''"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SAPCS]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SAPCS],0)) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE METAL=TT.METAL '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + 'AND COLHEAD=''D'') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE METAL=METAL AND COLHEAD=''S'''"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SARETURN]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SARETURN],0)) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE METAL=TT.METAL '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + 'AND COLHEAD=''D'') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE METAL=METAL AND COLHEAD=''S'''"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SARETURNPCS]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SARETURNPCS],0)) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE METAL=TT.METAL '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + 'AND COLHEAD=''D'') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE METAL=METAL AND COLHEAD=''S'''"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SACOMMISION]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SACOMMISION],0)) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE METAL=TT.METAL '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + 'AND COLHEAD=''D'') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE METAL=METAL AND COLHEAD=''S'''"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SRCOMMISION]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SRCOMMISION],0)) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE METAL=TT.METAL '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + 'AND COLHEAD=''D'') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE METAL=METAL AND COLHEAD=''S'''"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SABACKAMT]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SABACKAMT],0)) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE METAL=TT.METAL '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + 'AND COLHEAD=''D'') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE METAL=METAL AND COLHEAD=''S'''"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SRBACKAMT]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SRBACKAMT],0)) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE METAL=TT.METAL '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + 'AND COLHEAD=''D'') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE METAL=METAL AND COLHEAD=''S'''"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "


            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-STFSALESINC]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-STFSALESINC],0)) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE METAL=TT.METAL '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + 'AND COLHEAD=''D''AND ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-TARGET],0)<> 0) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE METAL=METAL AND COLHEAD=''S'''"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "

            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-STFBACKINC]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-STFBACKINC],0)) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE METAL=TT.METAL '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + 'AND COLHEAD=''D'' AND ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-TARGET],0)<> 0) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE METAL=METAL AND COLHEAD=''S'''"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "

            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-INCENTIVE]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-INCENTIVE],0)) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE METAL=TT.METAL '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + 'AND COLHEAD=''D'' AND ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-TARGET],0)<> 0) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE METAL=METAL AND COLHEAD=''S'''"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY) "


            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-TARGET]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-TARGET],0)) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE COLHEAD=''D'''"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE  COLHEAD=''G'''"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "

            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SALES]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SALES],0)) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE COLHEAD=''D'''"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE  COLHEAD=''G'''"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SARETURN]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SARETURN],0)) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE COLHEAD=''D'' '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE  COLHEAD=''G'''"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "

            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SAPCS]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SAPCS],0)) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE COLHEAD=''D'''"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE  COLHEAD=''G'''"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SARETURNPCS]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SARETURNPCS],0)) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE COLHEAD=''D'' '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE  COLHEAD=''G'''"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "


            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SACOMMISION]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SACOMMISION],0)) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE COLHEAD=''D'' '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE  COLHEAD=''G'''"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SRCOMMISION]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SRCOMMISION],0)) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE COLHEAD=''D'' '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE  COLHEAD=''G'''"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "


            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SABACKAMT]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SABACKAMT],0)) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE COLHEAD=''D'' '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE  COLHEAD=''G'''"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SRBACKAMT]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SRBACKAMT],0)) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE COLHEAD=''D'' '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE  COLHEAD=''G'''"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "

            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-STFSALESINC]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-STFSALESINC],0)) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE COLHEAD=''D'' '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-TARGET],0)<> 0) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE  COLHEAD=''G'''"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY) "

            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-STFBACKINC]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-STFBACKINC],0)) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE COLHEAD=''D'' '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-TARGET],0)<> 0) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE  COLHEAD=''G'''"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY) "

            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-INCENTIVE]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-INCENTIVE],0)) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE COLHEAD=''D'' '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-TARGET],0)<> 0) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE  COLHEAD=''G'''"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY) "


            StrSql += vbCrLf + "  SELECT @QRY='UPDATE TEMPTABLEDB..TEMP_OUT" & Sysid & " SET ['+ SUBSTRING(@MNTH,0,4) +'-TARGET]=NULL WHERE ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-TARGET],0)=0'"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' UPDATE TEMPTABLEDB..TEMP_OUT" & Sysid & " SET ['+ SUBSTRING(@MNTH,0,4) +'-SALES]=NULL WHERE ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SALES],0)=0'"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' UPDATE TEMPTABLEDB..TEMP_OUT" & Sysid & " SET ['+ SUBSTRING(@MNTH,0,4) +'-SARETURN]=NULL WHERE ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SARETURN],0)=0'"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' UPDATE TEMPTABLEDB..TEMP_OUT" & Sysid & " SET ['+ SUBSTRING(@MNTH,0,4) +'-SAPCS]=NULL WHERE ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SAPCS],0)=0'"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' UPDATE TEMPTABLEDB..TEMP_OUT" & Sysid & " SET ['+ SUBSTRING(@MNTH,0,4) +'-SARETURNPCS]=NULL WHERE ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SARETURNPCS],0)=0'"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' UPDATE TEMPTABLEDB..TEMP_OUT" & Sysid & " SET ['+ SUBSTRING(@MNTH,0,4) +'-SACOMMISION]=NULL WHERE ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SACOMMISION],0)=0'"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' UPDATE TEMPTABLEDB..TEMP_OUT" & Sysid & " SET ['+ SUBSTRING(@MNTH,0,4) +'-SRCOMMISION]=NULL WHERE ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SRCOMMISION],0)=0'"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' UPDATE TEMPTABLEDB..TEMP_OUT" & Sysid & " SET ['+ SUBSTRING(@MNTH,0,4) +'-SABACKAMT]=NULL WHERE ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SABACKAMT],0)=0'"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' UPDATE TEMPTABLEDB..TEMP_OUT" & Sysid & " SET ['+ SUBSTRING(@MNTH,0,4) +'-SRBACKAMT]=NULL WHERE ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SRBACKAMT],0)=0'"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' UPDATE TEMPTABLEDB..TEMP_OUT" & Sysid & " SET ['+ SUBSTRING(@MNTH,0,4) +'-INCENTIVE]=NULL WHERE ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-INCENTIVE],0)=0'"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' UPDATE TEMPTABLEDB..TEMP_OUT" & Sysid & " SET ['+ SUBSTRING(@MNTH,0,4) +'-STFSALESINC]=NULL WHERE ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-INCENTIVE],0)=0'"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' UPDATE TEMPTABLEDB..TEMP_OUT" & Sysid & " SET ['+ SUBSTRING(@MNTH,0,4) +'-STFBACKINC]=NULL WHERE ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-INCENTIVE],0)=0'"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY) "
            StrSql += vbCrLf + "  END CLOSE CUR DEALLOCATE CUR   "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
            Dim mna As New System.Globalization.DateTimeFormatInfo
            Dim mnth As String = mna.GetMonthName(Val(dtpFrom.Value.ToString.Substring(5, 2))).ToString
            StrSql = "  SELECT * FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " ORDER BY METAL,RESULT,[" & mnth.ToString.Substring(0, 3) & "-PER] DESC,EMPNAME "
            Dim dtSource As New DataTable
            dtSource.Columns.Add("KEYNO", GetType(Integer))
            dtSource.Columns("KEYNO").AutoIncrement = True
            dtSource.Columns("KEYNO").AutoIncrementSeed = 0
            dtSource.Columns("KEYNO").AutoIncrementStep = 1
            Da = New OleDbDataAdapter(StrSql, cn)
            Da.Fill(dtSource)

            Dim objGridShower As New frmGridDispDia
            objGridShower.gridView.DataSource = Nothing
            objGridShower.gridView.DataSource = dtSource
            objGridShower.Name = Me.Name
            objGridShower.WindowState = FormWindowState.Maximized
            objGridShower.gridView.Name = Me.Name
            objGridShower.gridView.Columns("PARTICULAR").Visible = True
            objGridShower.gridView.Columns("PARTICULAR").Width = 250
            objGridShower.gridView.Columns("EMPID").Visible = False
            objGridShower.gridView.Columns("EMPNAME").Visible = False
            objGridShower.gridView.Columns("COLHEAD").Visible = False
            objGridShower.gridView.Columns("KEYNO").Visible = False
            objGridShower.gridView.Columns("RESULT").Visible = False
            objGridShower.gridView.Columns("METAL").Visible = False
            FormatGridColumns(objGridShower.gridView, False, False, True, False)
            objGridShower.Text = "MONTH WISE SALES COMMISION"
            Dim tit As String = "MONTH WISE SALES COMMISION" + vbCrLf
            tit += " DATE FROM " & dtpFrom.Text + " TO " + dtpTo.Text
            If chkcmbmetal.Text <> "ALL" And chkcmbmetal.Text <> "" Then
                tit += " FOR METAL " & chkcmbmetal.Text & ""
            End If
            If chkcmbCounter.Text <> "ALL" And chkcmbCounter.Text <> "" Then
                tit += "(" & chkcmbCounter.Text & ")"
            End If
            If chkcmbitemname.Text <> "ALL" And chkcmbitemname.Text <> "" Then
                tit += "(" & chkcmbitemname.Text & ")"
            End If
            objGridShower.lblTitle.Text = tit
            objGridShower.StartPosition = FormStartPosition.CenterScreen
            objGridShower.dsGrid.DataSetName = objGridShower.Name
            objGridShower.FormReSize = False
            objGridShower.FormReLocation = False
            objGridShower.pnlFooter.Visible = False
            objGridShower.Show()
            FillGridGroupStyle_KeyNoWise(objGridShower.gridView, "EMPNAME")
            objGridShower.FormReSize = True
            objGridShower.FormReLocation = True
        Else
            MsgBox("No Records found.")
        End If


    End Sub

    Private Sub ShowroomIncentive()

        Dim particular As String = ""
        particular = "COSTNAME,MNTHNAME,TARGETAMOUNT"
        StrSql = "  IF OBJECT_ID('TEMPTABLEDB..SA_COMMISION" & Sysid & "')IS NOT NULL DROP TABLE TEMPTABLEDB..SA_COMMISION" & Sysid & ""
        StrSql += vbCrLf + " SELECT COSTNAME,MNTHNAME,TARGETAMOUNT,SUM(SAAMOUT)SAAMOUT,SUM(SRAMOUT)SRAMOUT"
        StrSql += vbCrLf + " ,(CASE WHEN ((SUM(SAAMOUT-SRAMOUT)/TARGETAMOUNT)*100) BETWEEN 90 AND 99 THEN SUM(((ISNULL(SACOMMISION,0)-ISNULL(SRCOMMISION,0))/100)*" & insper & ") "
        StrSql += vbCrLf + " WHEN ((SUM(SAAMOUT-SRAMOUT)/TARGETAMOUNT)*100) >99 THEN SUM((ISNULL(SACOMMISION,0)-ISNULL(SRCOMMISION,0))) END) INCENTIVE"
        StrSql += vbCrLf + " ,SUM(SACOMMISION)COMMISION,SUM(SRCOMMISION)RETCOMMISION"
        StrSql += vbCrLf + " ,CASE WHEN ((SUM(SAAMOUT-SRAMOUT)/TARGETAMOUNT)*100)between 90.001 and 100 THEN CONVERT(VARCHAR(50),CONVERT(DECIMAL(15,2),((SUM(SAAMOUT-SRAMOUT)/TARGETAMOUNT)*100)))"
        StrSql += vbCrLf + " WHEN ((SUM(SAAMOUT-SRAMOUT)/TARGETAMOUNT)*100)< 90  THEN 'Below 90'ELSE 'Above 100' END PER"
        StrSql += vbCrLf + " ,COSTID,MNTH INTO TEMPTABLEDB..SA_COMMISION" & Sysid & " "
        StrSql += vbCrLf + " FROM ("
        StrSql += vbCrLf + " SELECT C.COSTNAME,S.METAL,CONVERT(VARCHAR(15),'2013-'+ SUBSTRING(DATENAME(MONTH,TRANDATE),0,4)+'-01')MNTH"
        StrSql += vbCrLf + " ,DATENAME(MONTH,CONVERT(VARCHAR(15),'2013-'+ SUBSTRING(DATENAME(MONTH,TRANDATE),0,4)+'-01'))MNTHNAME"
        StrSql += vbCrLf + " ,F.AMOUNT TARGETAMOUNT"
        StrSql += vbCrLf + " ,(CASE WHEN SEP='SA' THEN (S.AMOUNT)ELSE 0 END) SAAMOUT"
        StrSql += vbCrLf + " ,(CASE WHEN SEP='SR' THEN (S.AMOUNT)ELSE 0 END) SRAMOUT "
        StrSql += vbCrLf + " ,(CASE WHEN SEP='SA' THEN (S.COMMISION)ELSE 0 END) SACOMMISION"
        StrSql += vbCrLf + " ,(CASE WHEN SEP='SR' THEN (S.COMMISION)ELSE 0 END) SRCOMMISION"
        StrSql += vbCrLf + " ,C.COSTID FROM TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " S"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..FLATINCENTIVE AS F ON F.COSTID=S.COSTID "
        StrSql += vbCrLf + " AND F.MONTH=DATENAME(MONTH,CONVERT(VARCHAR(15),'2014-'+ SUBSTRING(DATENAME(MONTH,TRANDATE),0,4)+'-01'))"
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..COSTCENTRE AS C ON C.COSTID=S.COSTID"
        StrSql += vbCrLf + " WHERE ISNULL(S.COMMISION,0) <> 0"
        StrSql += vbCrLf + "  )X GROUP BY COSTNAME,TARGETAMOUNT,MNTHNAME,COSTID,MNTH"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        StrSql = "  IF OBJECT_ID('TEMPTABLEDB..TEMP_OUT" & Sysid & "')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_OUT" & Sysid & ""
        StrSql += vbCrLf + "  SELECT COSTID,CONVERT(VARCHAR(200),COSTNAME)PARTICULAR,COSTNAME,MNTH,2 RESULT,'D'"
        StrSql += vbCrLf + "  COLHEAD INTO TEMPTABLEDB..TEMP_OUT" & Sysid & " FROM TEMPTABLEDB..SA_COMMISION" & Sysid & " "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        StrSql = "SELECT COUNT(*) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & ""
        Dim CNT As Integer = Val(GetSqlValue(cn, StrSql))
        If CNT > 0 Then

            StrSql = "  INSERT INTO TEMPTABLEDB..TEMP_OUT" & Sysid & "(PARTICULAR,COSTID,RESULT,COLHEAD)"
            StrSql += vbCrLf + "  SELECT 'GRANDTOTAL','ZZ',4,'G' "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

            StrSql = "  DECLARE @QRY VARCHAR(4000)"
            StrSql += vbCrLf + "  SET @QRY=''"
            StrSql += vbCrLf + "  DECLARE @A AS VARCHAR(10)"
            StrSql += vbCrLf + "  DECLARE @MNTH AS VARCHAR(10)"
            StrSql += vbCrLf + "  DECLARE CUR CURSOR FOR SELECT DISTINCT DATEPART(MM,CONVERT(SMALLDATETIME,MNTH))A,DATENAME(MONTH,MNTH)MNTH FROM TEMPTABLEDB..SA_COMMISION" & Sysid & " ORDER BY DATEPART(MM,CONVERT(SMALLDATETIME,MNTH))"
            StrSql += vbCrLf + "  OPEN CUR WHILE (1 = 1) BEGIN FETCH NEXT FROM CUR INTO @A,@MNTH"
            StrSql += vbCrLf + "  IF @@FETCH_STATUS =-1 BREAK"
            StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT" & Sysid & " ADD ['+ SUBSTRING(@MNTH,0,4) +'-TARGET] DECIMAL(15,3) DEFAULT 0' PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT" & Sysid & " ADD ['+ SUBSTRING(@MNTH,0,4)+'-AMOUNT] DECIMAL(15,3) DEFAULT 0' PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT" & Sysid & " ADD ['+ SUBSTRING(@MNTH,0,4)+'-RAMOUNT] DECIMAL(15,3) DEFAULT 0' PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT" & Sysid & " ADD ['+ SUBSTRING(@MNTH,0,4)+'-COMMISION] DECIMAL(15,2) DEFAULT 0' PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT" & Sysid & " ADD ['+ SUBSTRING(@MNTH,0,4)+'-RETCOMMISION] DECIMAL(15,2) DEFAULT 0' PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT" & Sysid & " ADD ['+ SUBSTRING(@MNTH,0,4)+'-PER] VARCHAR(15) DEFAULT NULL' PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT" & Sysid & " ADD ['+ SUBSTRING(@MNTH,0,4)+'-INCENTIVE] DECIMAL(15,3) DEFAULT 0' PRINT @QRY EXEC(@QRY)"

            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-TARGET]=(SELECT SUM(TARGETAMOUNT) FROM TEMPTABLEDB..SA_COMMISION" & Sysid & " WHERE COSTID=TT.COSTID   '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE COSTID=COSTID '"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY) "
            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-AMOUNT]=(SELECT SUM(SAAMOUT) FROM TEMPTABLEDB..SA_COMMISION" & Sysid & " WHERE COSTID=TT.COSTID   '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE COSTID=COSTID '"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY) "
            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-RAMOUNT]=(SELECT SUM(SRAMOUT) FROM TEMPTABLEDB..SA_COMMISION" & Sysid & " WHERE COSTID=TT.COSTID   '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE COSTID=COSTID  '"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY) "
            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-COMMISION]=(SELECT SUM(COMMISION) FROM TEMPTABLEDB..SA_COMMISION" & Sysid & " WHERE COSTID=TT.COSTID   '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE COSTID=COSTID  '"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY) "
            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-RETCOMMISION]=(SELECT SUM(RETCOMMISION) FROM TEMPTABLEDB..SA_COMMISION" & Sysid & " WHERE COSTID=TT.COSTID   '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE COSTID=COSTID  '"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY) "
            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-PER]=(SELECT TOP 1 PER FROM TEMPTABLEDB..SA_COMMISION" & Sysid & " WHERE COSTID=TT.COSTID  '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE COSTID=COSTID  '"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY) "
            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-INCENTIVE]=(SELECT SUM(INCENTIVE) FROM TEMPTABLEDB..SA_COMMISION" & Sysid & " WHERE COSTID=TT.COSTID   '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE COSTID=COSTID  '"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY) "

            'StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-TARGET]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-TARGET],0)) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE METAL=TT.METAL '"
            'StrSql += vbCrLf + "  SELECT @QRY= @QRY + 'AND COLHEAD=''D'') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE  COLHEAD=''S'''"
            'StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
            'StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-AMOUNT]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-AMOUNT],0)) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE METAL=TT.METAL '"
            'StrSql += vbCrLf + "  SELECT @QRY= @QRY + 'AND COLHEAD=''D'') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE  COLHEAD=''S'''"
            'StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
            'StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-RAMOUNT]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-RAMOUNT],0)) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE METAL=TT.METAL '"
            'StrSql += vbCrLf + "  SELECT @QRY= @QRY + 'AND COLHEAD=''D'') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE  COLHEAD=''S'''"
            'StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
            'StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-COMMISION]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-COMMISION],0)) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE METAL=TT.METAL '"
            'StrSql += vbCrLf + "  SELECT @QRY= @QRY + 'AND COLHEAD=''D'') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE  COLHEAD=''S'''"
            'StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
            'StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-RETCOMMISION]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-RETCOMMISION],0)) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE METAL=TT.METAL '"
            'StrSql += vbCrLf + "  SELECT @QRY= @QRY + 'AND COLHEAD=''D'') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE COLHEAD=''S'''"
            'StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
            'StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-INCENTIVE]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-INCENTIVE],0)) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE METAL=TT.METAL '"
            'StrSql += vbCrLf + "  SELECT @QRY= @QRY + 'AND COLHEAD=''D'') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE COLHEAD=''S'''"
            'StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "

            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-TARGET]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-TARGET],0)) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE COLHEAD=''D'''"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE  COLHEAD=''G'''"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-AMOUNT]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-AMOUNT],0)) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE COLHEAD=''D'''"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE  COLHEAD=''G'''"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-RAMOUNT]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-RAMOUNT],0)) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE COLHEAD=''D'' '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE  COLHEAD=''G'''"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-COMMISION]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-COMMISION],0)) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE COLHEAD=''D'' '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE  COLHEAD=''G'''"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-RETCOMMISION]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-RETCOMMISION],0)) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE COLHEAD=''D'' '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE  COLHEAD=''G'''"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-INCENTIVE]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-INCENTIVE],0)) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE COLHEAD=''D'' '"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ') FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " TT WHERE  COLHEAD=''G'''"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
            StrSql += vbCrLf + "  SELECT @QRY='UPDATE TEMPTABLEDB..TEMP_OUT" & Sysid & " SET ['+ SUBSTRING(@MNTH,0,4) +'-TARGET]=NULL WHERE ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-TARGET],0)=0'"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' UPDATE TEMPTABLEDB..TEMP_OUT" & Sysid & " SET ['+ SUBSTRING(@MNTH,0,4) +'-AMOUNT]=NULL WHERE ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-AMOUNT],0)=0'"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' UPDATE TEMPTABLEDB..TEMP_OUT" & Sysid & " SET ['+ SUBSTRING(@MNTH,0,4) +'-RAMOUNT]=NULL WHERE ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-RAMOUNT],0)=0'"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' UPDATE TEMPTABLEDB..TEMP_OUT" & Sysid & " SET ['+ SUBSTRING(@MNTH,0,4) +'-COMMISION]=NULL WHERE ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-COMMISION],0)=0'"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' UPDATE TEMPTABLEDB..TEMP_OUT" & Sysid & " SET ['+ SUBSTRING(@MNTH,0,4) +'-RETCOMMISION]=NULL WHERE ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-RETCOMMISION],0)=0'"
            StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' UPDATE TEMPTABLEDB..TEMP_OUT" & Sysid & " SET ['+ SUBSTRING(@MNTH,0,4) +'-INCENTIVE]=NULL WHERE ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-INCENTIVE],0)=0'"
            StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY) "
            StrSql += vbCrLf + "  END CLOSE CUR DEALLOCATE CUR   "

            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
            Dim mna As New System.Globalization.DateTimeFormatInfo
            Dim mnth As String = mna.GetMonthName(Val(dtpFrom.Value.ToString.Substring(5, 2))).ToString
            StrSql = "SELECT * FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " ORDER BY RESULT,[" & mnth.ToString.Substring(0, 3) & "-PER] DESC,COSTNAME "
            Dim dtSource As New DataTable
            dtSource.Columns.Add("KEYNO", GetType(Integer))
            dtSource.Columns("KEYNO").AutoIncrement = True
            dtSource.Columns("KEYNO").AutoIncrementSeed = 0
            dtSource.Columns("KEYNO").AutoIncrementStep = 1
            Da = New OleDbDataAdapter(StrSql, cn)
            Da.Fill(dtSource)

            Dim objGridShower As New frmGridDispDia
            objGridShower.gridView.DataSource = Nothing
            objGridShower.gridView.DataSource = dtSource
            objGridShower.Name = Me.Name
            objGridShower.WindowState = FormWindowState.Maximized
            objGridShower.gridView.Name = Me.Name
            objGridShower.gridView.Columns("PARTICULAR").Visible = True
            objGridShower.gridView.Columns("PARTICULAR").Width = 250
            objGridShower.gridView.Columns("COSTID").Visible = False
            objGridShower.gridView.Columns("COSTNAME").Visible = False
            objGridShower.gridView.Columns("COLHEAD").Visible = False
            objGridShower.gridView.Columns("KEYNO").Visible = False
            objGridShower.gridView.Columns("RESULT").Visible = False
            objGridShower.gridView.Columns("MNTH").Visible = False
            FormatGridColumns(objGridShower.gridView, False, False, True, False)
            objGridShower.Text = "MONTH WISE AMOUNT COMMISION"
            Dim tit As String = "MONTH WISE AMOUNT COMMISION" + vbCrLf
            tit += " DATE FROM " & dtpFrom.Text + " TO " + dtpTo.Text
            objGridShower.lblTitle.Text = tit
            objGridShower.StartPosition = FormStartPosition.CenterScreen
            objGridShower.dsGrid.DataSetName = objGridShower.Name
            objGridShower.FormReSize = False
            objGridShower.FormReLocation = False
            objGridShower.pnlFooter.Visible = False
            objGridShower.Show()
            FillGridGroupStyle_KeyNoWise(objGridShower.gridView, "PARTICULAR")
            objGridShower.FormReSize = True
            objGridShower.FormReLocation = True
        Else
            MsgBox("No Records found.")
        End If
    End Sub
#End Region


    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = False
            Next
            .Columns("PARTICULAR").Visible = True
            .Columns("TRANNO").Visible = rbtDetailed.Checked
            .Columns("TRANDATE").Visible = rbtDetailed.Checked
            .Columns("TAGNO").Visible = rbtDetailed.Checked
            .Columns("PCS").Visible = rbtDetailed.Checked
            .Columns("SA_GRSWT").Visible = rbtDetailed.Checked
            .Columns("SA_NETWT").Visible = rbtDetailed.Checked
            .Columns("SA_AMOUNT").Visible = rbtDetailed.Checked
            .Columns("SR_GRSWT").Visible = rbtDetailed.Checked
            .Columns("SR_NETWT").Visible = rbtDetailed.Checked
            .Columns("SR_AMOUNT").Visible = rbtDetailed.Checked
            .Columns("COMMISION").Visible = True
            FormatGridColumns(dgv, False, False, , False)

            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SA_GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SA_NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SA_AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SR_GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SR_NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SR_AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"

            .Columns("SA_GRSWT").HeaderText = "SALES GRSWT"
            .Columns("SA_NETWT").HeaderText = "SALES NETWT"
            .Columns("SA_AMOUNT").HeaderText = "SALES AMOUNT"
            .Columns("SR_GRSWT").HeaderText = "RETURN GRSWT"
            .Columns("SR_NETWT").HeaderText = "RETURN NETWT"
            .Columns("SR_AMOUNT").HeaderText = "RETURN AMOUNT"
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New RPT_SALES_COMMISION_Properties
        GetSettingsObj(obj, Me.Name, GetType(RPT_SALES_COMMISION_Properties))
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, "ALL")
        rbtSummary.Checked = obj.p_rbtSummary
        rbtDetailed.Checked = obj.p_rbtDetailed
        rbtGrpCounter.Checked = obj.p_rbtGrpCounter
        rbtGrpEmployee.Checked = obj.p_rbtGrpEmployee
        rbtproduct.Checked = obj.p_rbtproduct
        rbtAll.Checked = obj.p_rbtAll
        chkWithEmptyCommision.Checked = obj.p_chkWithEmptyCommision
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New RPT_SALES_COMMISION_Properties
        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        obj.p_rbtSummary = rbtSummary.Checked
        obj.p_rbtDetailed = rbtDetailed.Checked
        obj.p_chkWithEmptyCommision = chkWithEmptyCommision.Checked
        obj.p_rbtGrpCounter = rbtGrpCounter.Checked
        obj.p_rbtGrpEmployee = rbtGrpEmployee.Checked
        obj.p_rbtproduct = rbtproduct.Checked
        obj.p_rbtAll = rbtAll.Checked
        SetSettingsObj(obj, Me.Name, GetType(RPT_SALES_COMMISION_Properties))
    End Sub

    Private Sub chkcmbmetal_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkcmbmetal.Leave
        Call chkcmbmetal_SelectionChangeCommitted(sender, e)
    End Sub

    Private Sub chkcmbmetal_SelectionChangeCommitted(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkcmbmetal.SelectionChangeCommitted
        Dim metalid As String = GetSelectedMetalid(chkcmbmetal, True)
        StrSql = " SELECT 'ALL' ITEMNAME,'ALL' ITEMID,1 RESULT"
        StrSql += " UNION ALL"
        StrSql += " SELECT ITEMNAME,CONVERT(vARCHAR,ITEMID),2 RESULT FROM " & cnAdminDb & "..ITEMMAST WHERE 1=1"
        If chkcmbmetal.Text <> "ALL" Then StrSql += " AND METALID IN(" & metalid & ")"
        StrSql += " ORDER BY RESULT,ITEMNAME"
        dtITEM = New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtITEM)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbitemname, dtITEM, "ITEMNAME", , "ALL")
    End Sub
    Private Sub rbtSummary_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtSummary.CheckedChanged
        If rbtSummary.Checked = True Then
            chkWithTagno.Enabled = True
        Else
            chkWithTagno.Enabled = False
            chkWithTagno.Checked = False
        End If
    End Sub

    Private Sub chkcmbitemname_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkcmbitemname.Leave
        Call chkcmbitemname_SelectedValueChanged(sender, e)
    End Sub

    Private Sub chkcmbitemname_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkcmbitemname.SelectedValueChanged
        Dim chkitemid As String = GetSelecteditemid(chkcmbitemname, True)
        StrSql = " SELECT 'ALL' SUBITEMNAME,'ALL' SUBITEMID,1 RESULT"
        StrSql += " UNION ALL"
        StrSql += " SELECT SUBITEMNAME,CONVERT(vARCHAR,SUBITEMID),2 RESULT FROM " & cnAdminDb & "..SUBITEMMAST WHERE 1=1"
        If chkcmbitemname.Text <> "ALL" Then StrSql += " AND ITEMID IN(" & chkitemid & ")"
        StrSql += " ORDER BY RESULT,SUBITEMNAME"
        dtsubITEM = New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtsubITEM)
            BrighttechPack.GlobalMethods.FillCombo(chkcmbsubitem, dtsubITEM, "SUBITEMNAME", , "ALL")

    End Sub

    Private Sub rbtGrpCounter_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtGrpCounter.CheckedChanged
        If rbtGrpCounter.Checked = True Then
            pnlComm.Visible = True
        Else
            pnlComm.Visible = False
        End If
    End Sub

    Private Sub chkmnthwise_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkmnthwise.CheckedChanged
        If chkmnthwise.Checked = True Then
            grppannel.Enabled = False
            pnlrpttype.Enabled = False
            rbtMetal.Checked = True
            rbtSummary.Checked = True
        Else
            grppannel.Enabled = True
            pnlrpttype.Enabled = True
            rbtproduct.Checked = True
        End If
    End Sub

    Private Sub rbtAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtAll.CheckedChanged, rbtGrpEmployee.CheckedChanged, rbtGrpEmployee.CheckedChanged, rbtGrpCounter.CheckedChanged, rbtMetal.CheckedChanged
        If rbtAll.Checked Then
            rbtDetailed.Enabled = False
            chkdesc.Enabled = False
            rbtSummary.Checked = True
            pnlSumType.Enabled = True
        Else
            rbtDetailed.Enabled = True
            chkdesc.Enabled = True
            pnlSumType.Enabled = False
        End If
    End Sub

    Private Sub btnEmptyComm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEmptyComm.Click
        Dim chkempid As String = GetQryStringForSp(chkcmbemployee.Text, cnAdminDb & "..EMPMASTER", "EMPID", "EMPNAME", True)
        Dim chkCostId As String = GetQryStringForSp(chkCmbCostCentre.Text, cnAdminDb & "..COSTCENTRE", "COSTID", "COSTNAME", True)
        Dim chkitemid As String = GetQryStringForSp(chkcmbitemname.Text, cnAdminDb & "..ITEMMAST", "ITEMID", "ITEMNAME", True)
        Dim chksubitemid As String = GetQryStringForSp(chkcmbsubitem.Text, cnAdminDb & "..SUBITEMMAST", "SUBITEMID", "SUBITEMNAME", True)
        Dim msgStr1 As String = "" : Dim msgStr2 As String = ""


        StrSql = " SELECT DISTINCT X.ITEMID,I.ITEMNAME,X.SUBITEMID,S.SUBITEMNAME FROM ("
        StrSql += vbCrLf + " SELECT DISTINCT ITEMID,NULL AS SUBITEMID FROM " & cnStockDb & "..ISSUE I"
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND I.TRANTYPE = 'SA' AND ISNULL(I.CANCEL,'') = ''"
        If chkempid <> "ALL" And chkempid <> "" Then
            StrSql += vbCrLf + " AND EMPID IN(" & chkempid & ")"
        End If
        If chkCostId <> "ALL" And chkCostId <> "" Then
            StrSql += vbCrLf + " AND COSTID IN(" & chkCostId & ")"
        End If
        If chkitemid <> "ALL" And chkitemid <> "" Then
            StrSql += vbCrLf + " AND ITEMID IN(" & chkitemid & ")"
        End If
        StrSql += vbCrLf + " AND ITEMID NOT IN(SELECT ITEMID FROM " & cnAdminDb & "..SALES_COMMISION UNION SELECT 0)"
        StrSql += vbCrLf + " UNION ALL"


        StrSql += vbCrLf + " SELECT DISTINCT STNITEMID AS ITEMID,NULL AS SUBITEMID FROM " & cnStockDb & "..ISSSTONE I"
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND I.TRANTYPE = 'SA' "
        If chkempid <> "ALL" And chkempid <> "" Then
            StrSql += vbCrLf + " AND EMPID IN(" & chkempid & ")"
        End If
        If chkCostId <> "ALL" And chkCostId <> "" Then
            StrSql += vbCrLf + " AND COSTID IN(" & chkCostId & ")"
        End If
        If chkitemid <> "ALL" And chkitemid <> "" Then
            StrSql += vbCrLf + " AND STNITEMID IN(" & chkitemid & ")"
        End If
        StrSql += vbCrLf + " AND STNITEMID NOT IN(SELECT ITEMID FROM " & cnAdminDb & "..SALES_COMMISION UNION SELECT 0)"
        StrSql += vbCrLf + " UNION ALL"




        StrSql += vbCrLf + " SELECT DISTINCT  ITEMID,SUBITEMID FROM " & cnStockDb & "..ISSUE I"
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND I.TRANTYPE = 'SA' AND ISNULL(I.CANCEL,'') = ''"
        If chkempid <> "ALL" And chkempid <> "" Then
            StrSql += vbCrLf + " AND EMPID IN(" & chkempid & ")"
        End If
        If chkCostId <> "ALL" And chkCostId <> "" Then
            StrSql += vbCrLf + " AND COSTID IN(" & chkCostId & ")"
        End If
        If chkitemid <> "ALL" And chkitemid <> "" Then
            StrSql += vbCrLf + " AND ITEMID IN(" & chkitemid & ")"
        End If
        If chksubitemid <> "ALL" And chksubitemid <> "" Then
            StrSql += vbCrLf + " AND SUBITEMID IN(" & chksubitemid & ")"
        End If
        StrSql += vbCrLf + " AND SUBITEMID NOT IN(SELECT SUBITEMID FROM " & cnAdminDb & "..SALES_COMMISION UNION SELECT 0)"
        StrSql += vbCrLf + " AND ITEMID NOT IN(SELECT ITEMID FROM " & cnAdminDb & "..SALES_COMMISION WHERE SUBITEMID=0)"
        StrSql += vbCrLf + " UNION ALL"

        StrSql += vbCrLf + " SELECT DISTINCT  STNITEMID,STNSUBITEMID FROM " & cnStockDb & "..ISSSTONE I"
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND I.TRANTYPE = 'SA' "
        If chkempid <> "ALL" And chkempid <> "" Then
            StrSql += vbCrLf + " AND EMPID IN(" & chkempid & ")"
        End If
        If chkCostId <> "ALL" And chkCostId <> "" Then
            StrSql += vbCrLf + " AND COSTID IN(" & chkCostId & ")"
        End If
        If chkitemid <> "ALL" And chkitemid <> "" Then
            StrSql += vbCrLf + " AND STNITEMID IN(" & chkitemid & ")"
        End If
        If chksubitemid <> "ALL" And chksubitemid <> "" Then
            StrSql += vbCrLf + " AND STNSUBITEMID IN(" & chksubitemid & ")"
        End If
        StrSql += vbCrLf + " AND STNSUBITEMID NOT IN(SELECT SUBITEMID FROM " & cnAdminDb & "..SALES_COMMISION UNION SELECT 0)"
        StrSql += vbCrLf + " AND STNITEMID NOT IN(SELECT ITEMID FROM " & cnAdminDb & "..SALES_COMMISION WHERE SUBITEMID=0)"


        StrSql += vbCrLf + " )X "
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS I ON X.ITEMID=I.ITEMID "
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS S ON X.SUBITEMID=S.SUBITEMID  AND X.ITEMID=I.ITEMID "
        StrSql += vbCrLf + " ORDER BY ITEMNAME,SUBITEMNAME"
        Dim da As New OleDbDataAdapter
        da = New OleDbDataAdapter(StrSql, cn)
        Dim dt As New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            objGridShower = New frmGridDispDia
            objGridShower.BaseName = "Sold Items on empty commission"
            objGridShower.Name = Me.Name
            objGridShower.WindowState = FormWindowState.Normal
            objGridShower.gridView.RowTemplate.Height = 21
            objGridShower.gridView.DataSource = dt
            'objGridShower.Size = New Size(778, 550)
            objGridShower.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            objGridShower.gridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            objGridShower.gridView.BackgroundColor = objGridShower.BackColor
            'objGridShower.gridView.ColumnHeadersDefaultCellStyle = gridView_OWN.ColumnHeadersDefaultCellStyle
            'objGridShower.gridView.ColumnHeadersHeightSizeMode = gridView_OWN.ColumnHeadersHeightSizeMode
            'objGridShower.gridView.ColumnHeadersHeight = gridView_OWN.ColumnHeadersHeight
            Dim tit As String = ""
            tit = " SOLD ITEMS ON EMPTY COMMISSION " + vbCrLf
            tit += " DATE FROM " & dtpFrom.Text + " TO " + dtpTo.Text
            objGridShower.lblTitle.Text = tit
            objGridShower.pnlHeader.Height = objGridShower.lblTitle.Height + 20
            objGridShower.StartPosition = FormStartPosition.CenterScreen
            objGridShower.Show()
        Else
            MsgBox("Record not found....", MsgBoxStyle.Information) : Exit Sub
        End If
        Exit Sub

        StrSql = "SELECT DISTINCT ITEMID FROM " & cnStockDb & "..ISSUE I"
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND I.TRANTYPE = 'SA' AND ISNULL(I.CANCEL,'') = ''"
        If chkempid <> "ALL" And chkempid <> "" Then
            StrSql += vbCrLf + " AND EMPID IN(" & chkempid & ")"
        End If
        If chkCostId <> "ALL" And chkCostId <> "" Then
            StrSql += vbCrLf + " AND COSTID IN(" & chkCostId & ")"
        End If
        If chkitemid <> "ALL" And chkitemid <> "" Then
            StrSql += vbCrLf + " AND ITEMID IN(" & chkitemid & ")"
        End If
        StrSql += vbCrLf + " AND ITEMID NOT IN(SELECT ITEMID FROM " & cnAdminDb & "..SALES_COMMISION UNION SELECT 0)"
        da = New OleDbDataAdapter(StrSql, cn)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Dim Itemid As String = ""
            For i As Integer = 0 To dt.Rows.Count - 1
                If i = dt.Rows.Count - 1 Then
                    Itemid += dt.Rows(i).Item("ITEMID").ToString
                Else
                    Itemid += dt.Rows(i).Item("ITEMID").ToString + ","
                End If
            Next
            msgStr1 = "Commission not Set for Items" & vbCrLf & Itemid
            'MsgBox(msgStr1, MsgBoxStyle.Information) : Exit Sub
        End If
        StrSql = "SELECT DISTINCT  ITEMID,SUBITEMID FROM " & cnStockDb & "..ISSUE I"
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND I.TRANTYPE = 'SA' AND ISNULL(I.CANCEL,'') = ''"
        If chkempid <> "ALL" And chkempid <> "" Then
            StrSql += vbCrLf + " AND EMPID IN(" & chkempid & ")"
        End If
        If chkCostId <> "ALL" And chkCostId <> "" Then
            StrSql += vbCrLf + " AND COSTID IN(" & chkCostId & ")"
        End If
        If chksubitemid <> "ALL" And chksubitemid <> "" Then
            StrSql += vbCrLf + " AND SUBITEMID IN(" & chkempid & ")"
        End If
        StrSql += vbCrLf + " AND SUBITEMID NOT IN(SELECT SUBITEMID FROM " & cnAdminDb & "..SALES_COMMISION UNION SELECT 0)"
        dt = New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Dim Itemid As String = ""
            For i As Integer = 0 To dt.Rows.Count - 1
                StrSql = " SELECT 1 FROM " & cnAdminDb & "..SALES_COMMISION WHERE ITEMID=" & dt.Rows(i).Item("ITEMID").ToString & " AND SUBITEMID=0"
                If Val(objGPack.GetSqlValue(StrSql, , "")) > 0 Then Continue For
                If i = dt.Rows.Count - 1 Then
                    Itemid += dt.Rows(i).Item("SUBITEMID").ToString
                Else
                    Itemid += dt.Rows(i).Item("SUBITEMID").ToString + ","
                End If
            Next
            msgStr2 = "Commission not Set for Subitems" & vbCrLf & Itemid
            'MsgBox(msgStr2, MsgBoxStyle.Information) : Exit Sub
        End If
        If msgStr1 <> "" Or msgStr2 <> "" Then
            MsgBox(msgStr1 + vbCrLf + msgStr2, MsgBoxStyle.Information) : Exit Sub
        Else
            MsgBox("Record not found....", MsgBoxStyle.Information) : Exit Sub
        End If
    End Sub
End Class

Public Class RPT_SALES_COMMISION_Properties
    Private chkCmbCompany As New List(Of String)
    Public Property p_chkCmbCompany() As List(Of String)
        Get
            Return chkCmbCompany
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCompany = value
        End Set
    End Property
    Private chkCmbCostCentre As New List(Of String)
    Public Property p_chkCmbCostCentre() As List(Of String)
        Get
            Return chkCmbCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCostCentre = value
        End Set
    End Property
    Private rbtSummary As Boolean = True
    Public Property p_rbtSummary() As Boolean
        Get
            Return rbtSummary
        End Get
        Set(ByVal value As Boolean)
            rbtSummary = value
        End Set
    End Property
    Private rbtDetailed As Boolean = False
    Public Property p_rbtDetailed() As Boolean
        Get
            Return rbtDetailed
        End Get
        Set(ByVal value As Boolean)
            rbtDetailed = value
        End Set
    End Property
    Private rbtproduct As Boolean = False
    Public Property p_rbtproduct() As Boolean
        Get
            Return rbtproduct
        End Get
        Set(ByVal value As Boolean)
            rbtproduct = value
        End Set
    End Property
    Private rbtGrpCounter As Boolean = False
    Public Property p_rbtGrpCounter() As Boolean
        Get
            Return rbtGrpCounter
        End Get
        Set(ByVal value As Boolean)
            rbtGrpCounter = value
        End Set
    End Property
    Private rbtGrpEmployee As Boolean = True
    Public Property p_rbtGrpEmployee() As Boolean
        Get
            Return rbtGrpEmployee
        End Get
        Set(ByVal value As Boolean)
            rbtGrpEmployee = value
        End Set
    End Property
    Private chkWithEmptyCommision As Boolean = False
    Public Property p_chkWithEmptyCommision() As Boolean
        Get
            Return chkWithEmptyCommision
        End Get
        Set(ByVal value As Boolean)
            chkWithEmptyCommision = value
        End Set
    End Property
    Private chkmnthwise As Boolean = False
    Public Property p_chkmnthwise() As Boolean
        Get
            Return chkmnthwise
        End Get
        Set(ByVal value As Boolean)
            chkmnthwise = value
        End Set
    End Property

    Private rbtAll As Boolean = False
    Public Property p_rbtAll() As Boolean
        Get
            Return rbtAll
        End Get
        Set(ByVal value As Boolean)
            rbtAll = value
        End Set
    End Property

End Class