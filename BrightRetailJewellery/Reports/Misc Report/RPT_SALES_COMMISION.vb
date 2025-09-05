Imports System.Data.OleDb

Public Class RPT_SALES_COMMISION
    Dim StrSql As String
    Dim Da As OleDbDataAdapter
    Dim Cmd As OleDbCommand
    Dim objGridShower As frmGridDispDia
    Dim dtCostCentre As New DataTable
    Dim dtEMP, dtmetal, dtsubITEM As New DataTable
    Dim dtITEM As New DataTable
    Dim dtCompany As New DataTable
    Dim itemfilter As String = ""

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
        GiritechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)

        StrSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        StrSql += " UNION ALL"
        StrSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        StrSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtCostCentre)
        GiritechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , "ALL")

        StrSql = " SELECT 'ALL' METALNAME,'ALL' METALID,1 RESULT"
        StrSql += " UNION ALL"
        StrSql += " SELECT METALNAME,CONVERT(vARCHAR,METALID),2 RESULT FROM " & cnAdminDb & "..METALMAST"
        StrSql += " ORDER BY RESULT,METALNAME"
        dtmetal = New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtmetal)
        GiritechPack.GlobalMethods.FillCombo(chkcmbmetal, dtmetal, "METALNAME", , "ALL")


        StrSql = " SELECT 'ALL' ITEMNAME,'ALL' ITEMID,1 RESULT"
        StrSql += " UNION ALL"
        StrSql += " SELECT ITEMNAME,CONVERT(vARCHAR,ITEMID),2 RESULT FROM " & cnAdminDb & "..ITEMMAST"
        StrSql += " ORDER BY RESULT,ITEMNAME"

        dtITEM = New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtITEM)
        GiritechPack.GlobalMethods.FillCombo(chkcmbitemname, dtITEM, "ITEMNAME", , "ALL")


        StrSql = " SELECT 'ALL' SUBITEMNAME,'ALL' SUBITEMID,1 RESULT"
        StrSql += " UNION ALL"
        StrSql += " SELECT SUBITEMNAME,CONVERT(vARCHAR,SUBITEMID),2 RESULT FROM " & cnAdminDb & "..SUBITEMMAST"
        StrSql += " ORDER BY RESULT,SUBITEMNAME"

        dtsubITEM = New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtsubITEM)
        GiritechPack.GlobalMethods.FillCombo(chkcmbsubitem, dtsubITEM, "SUBITEMNAME", , "ALL")


        StrSql = " SELECT 'ALL' EMPNAME,'ALL' EMPID,1 RESULT"
        StrSql += " UNION ALL"
        StrSql += " SELECT EMPNAME,CONVERT(vARCHAR,EMPID),2 RESULT FROM " & cnAdminDb & "..EMPMASTER"
        StrSql += " ORDER BY RESULT,EMPNAME"
        dtEMP = New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtEMP)
        GiritechPack.GlobalMethods.FillCombo(chkcmbemployee, dtEMP, "EMPNAME", , "ALL")


        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        ' chkWithItem.Checked = False
        rbtGrpEmployee.Checked = True
        GiritechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , "ALL")
        GiritechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , "ALL")
        GiritechPack.GlobalMethods.FillCombo(chkcmbitemname, dtITEM, "ITEMNAME", , "ALL")
        GiritechPack.GlobalMethods.FillCombo(chkcmbemployee, dtEMP, "EMPNAME", , "ALL")
        GiritechPack.GlobalMethods.FillCombo(chkcmbsubitem, dtsubITEM, "SUBITEMNAME", , "ALL")
        GiritechPack.GlobalMethods.FillCombo(chkcmbmetal, dtmetal, "METALNAME", , "ALL")
        Prop_Gets()
        dtpFrom.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Prop_Sets()
        itemfilter = ""
        Dim chkMetalid As String = GetSelectedMetalid(chkcmbmetal, True)
        Dim chkCompId As String = GetQryStringForSp(chkCmbCompany.Text, cnAdminDb & "..COMPANY", "COMPANYID", "COMPANYNAME", True)
        Dim chkCostId As String = GetQryStringForSp(chkCmbCostCentre.Text, cnAdminDb & "..COSTCENTRE", "COSTID", "COSTNAME", True)
        Dim chkempid As String = GetQryStringForSp(chkcmbemployee.Text, cnAdminDb & "..EMPMASTER", "EMPID", "EMPNAME", True)
        Dim chkitemid As String = GetQryStringForSp(chkcmbitemname.Text, cnAdminDb & "..ITEMMAST", "ITEMID", "ITEMNAME", True)
        Dim chksubitemid As String = GetQryStringForSp(chkcmbsubitem.Text, cnAdminDb & "..SUBITEMMAST", "SUBITEMID", "SUBITEMNAME", True)

        StrSql = "SELECT COUNT(*) FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='INCENTIVEEXCLUDEITEM'"
        Dim cnt As Integer = GetSqlValue(cn, StrSql)
        If cnt <= 0 Then
            StrSql = " INSERT INTO " & cnAdminDb & "..SOFTCONTROL(CTLID,CTLNAME,CTLTYPE,CTLTEXT,CTLMODULE)VALUES('INCENTIVEEXCLUDEITEM','INCENTIVE EXCLUDE ITEM FOR SALES COMMISSION REPORT','T','','A')"
            Cmd = New OleDbCommand(StrSql, cn)
            Cmd.ExecuteNonQuery()
        End If
        StrSql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='INCENTIVEEXCLUDEITEM'"
        itemfilter = GetSqlValue(cn, StrSql)



        StrSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPSALESCOMMISION','U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPSALESCOMMISION"
        StrSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPSALESCOMMISION_RES','U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPSALESCOMMISION_RES"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        StrSql = vbCrLf + " SELECT * INTO TEMPTABLEDB..TEMPSALESCOMMISION FROM( "
        StrSql += vbCrLf + " SELECT  CONVERT(NUMERIC(15,2),0) AS COMMISION,CONVERT(NUMERIC(15,2),0) AS SALESCOMM"
        StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0) AS OTHERCOMM,M.METALNAME AS METAL,IM.ITEMNAME AS ITEM"
        StrSql += vbCrLf + " ,SM.SUBITEMNAME,I.TRANNO,I.TRANDATE,I.TAGNO,I.PCS,I.GRSWT,I.NETWT,I.AMOUNT"
        StrSql += vbCrLf + " ,ISNULL(SM.CALTYPE,IM.CALTYPE) AS CALTYPE,CONVERT(NUMERIC(15,2),0) AS COMM_FLAT"
        StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0) AS COMM_GRM,CONVERT(NUMERIC(15,2),0) AS COMM_PER"
        StrSql += vbCrLf + " ,CONVERT(VARCHAR(1),'')AS UPDATED"
        StrSql += vbCrLf + " ,I.ITEMCTRID,IM.METALID,I.ITEMID,I.SUBITEMID,I.EMPID,EM.EMPNAME,CO.ITEMCTRNAME,'SA' AS SEP"
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
        If chksubitemid <> "" And chksubitemid <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(I.SUBITEMID,'')IN(" & chksubitemid & ") "
        If chkCostId <> "" And chkCostId <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(I.COSTID,'') IN (" & chkCostId & ")"
        If chkCompId <> "" And chkCompId <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(I.COMPANYID,'') IN (" & chkCompId & ")"
        If itemfilter <> "" Then StrSql += " AND I.ITEMID NOT IN(" & itemfilter & ")"
        StrSql += vbCrLf + " UNION ALL "
        StrSql += vbCrLf + " SELECT CONVERT(NUMERIC(15,2),0) AS COMMISION,CONVERT(NUMERIC(15,2),0) AS SALESCOMM"
        StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0) AS OTHERCOMM,M.METALNAME AS METAL,IM.ITEMNAME AS ITEM"
        StrSql += vbCrLf + " ,SM.SUBITEMNAME,ISS.TRANNO,ISS.TRANDATE,I.TAGNO,ISS.STNPCS,ISS.STNWT,ISS.STNWT,ISS.STNAMT"
        StrSql += vbCrLf + " ,ISNULL(SM.CALTYPE,IM.CALTYPE) AS CALTYPE,CONVERT(NUMERIC(15,2),0) AS COMM_FLAT"
        StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0) AS COMM_GRM,CONVERT(NUMERIC(15,2),0) AS COMM_PER"
        StrSql += vbCrLf + " ,CONVERT(VARCHAR(1),'')AS UPDATED"
        StrSql += vbCrLf + " ,I.ITEMCTRID,IM.METALID,ISS.STNITEMID,ISS.STNSUBITEMID,I.EMPID,EM.EMPNAME,CO.ITEMCTRNAME,'SA' AS SEP"
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
        If chksubitemid <> "" And chksubitemid <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(ISS.STNSUBITEMID,'')IN(" & chksubitemid & ") "
        If chkCostId <> "" And chkCostId <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(ISS.COSTID,'') IN (" & chkCostId & ")"
        If chkCompId <> "" And chkCompId <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(ISS.COMPANYID,'') IN (" & chkCompId & ")"
        If itemfilter <> "" Then StrSql += " AND ISS.STNITEMID NOT IN(" & itemfilter & ")"
        StrSql += vbCrLf + " )X "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        StrSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPSALESCOMMISION"

        StrSql += vbCrLf + " SELECT * FROM (SELECT CONVERT(NUMERIC(15,2),0) AS COMMISION"
        StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0) AS SALESCOMM,CONVERT(NUMERIC(15,2),0) AS OTHERCOMM"
        StrSql += vbCrLf + " ,M.METALNAME AS METAL,IM.ITEMNAME AS ITEM,SM.SUBITEMNAME,I.TRANNO,I.TRANDATE"
        StrSql += vbCrLf + " ,I.TAGNO,I.PCS,I.GRSWT,I.NETWT,I.AMOUNT,ISNULL(SM.CALTYPE,IM.CALTYPE) AS CALTYPE"
        StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0) AS COMM_FLAT,CONVERT(NUMERIC(15,2),0) AS COMM_GRM"
        StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0) AS COMM_PER,CONVERT(VARCHAR(1),'')AS UPDATED"
        StrSql += vbCrLf + " ,I.ITEMCTRID,IM.METALID,I.ITEMID,I.SUBITEMID,I.EMPID,EM.EMPNAME,CO.ITEMCTRNAME,'SR' AS SEP"
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
        If chksubitemid <> "" And chksubitemid <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(I.SUBITEMID,'')IN(" & chksubitemid & ") "
        If chkCostId <> "" And chkCostId <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(I.COSTID,'') IN (" & chkCostId & ")"
        If chkCompId <> "" And chkCompId <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(I.COMPANYID,'') IN (" & chkCompId & ")"
        If itemfilter <> "" Then StrSql += " AND I.ITEMID NOT IN(" & itemfilter & ")"
        StrSql += vbCrLf + " UNION ALL "
        StrSql += vbCrLf + " SELECT CONVERT(NUMERIC(15,2),0) AS COMMISION,CONVERT(NUMERIC(15,2),0) AS SALESCOMM"
        StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0) AS OTHERCOMM,M.METALNAME AS METAL,IM.ITEMNAME AS ITEM"
        StrSql += vbCrLf + " ,SM.SUBITEMNAME,IR.TRANNO,IR.TRANDATE,I.TAGNO,IR.STNPCS,IR.STNWT,IR.STNWT,IR.STNAMT"
        StrSql += vbCrLf + " ,ISNULL(SM.CALTYPE,IM.CALTYPE) AS CALTYPE,CONVERT(NUMERIC(15,2),0) AS COMM_FLAT"
        StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0) AS COMM_GRM,CONVERT(NUMERIC(15,2),0) AS COMM_PER"
        StrSql += vbCrLf + " ,CONVERT(VARCHAR(1),'')AS UPDATED"
        StrSql += vbCrLf + " ,I.ITEMCTRID,IM.METALID,IR.STNITEMID,IR.STNSUBITEMID,I.EMPID,EM.EMPNAME,CO.ITEMCTRNAME,'SR' AS SEP"
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
        If chksubitemid <> "" And chksubitemid <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(IR.STNSUBITEMID,'')IN(" & chksubitemid & ") "
        If chkCostId <> "" And chkCostId <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(IR.COSTID,'') IN (" & chkCostId & ")"
        If chkCompId <> "" And chkCompId <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(IR.COMPANYID,'') IN (" & chkCompId & ")"
        If itemfilter <> "" Then StrSql += " AND IR.STNITEMID NOT IN(" & itemfilter & ")"

        StrSql += vbCrLf + " )X "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        If rbtGrpCounter.Checked Then
            StrSql = vbCrLf + " UPDATE TEMPTABLEDB..TEMPSALESCOMMISION SET COMM_FLAT = (SC.COMMISION*ISNULL(TS.PCS,1))"
            'StrSql += vbCrLf + " CASE WHEN SC.BASEDON IN('V') THEN  (SC.COMMISION*ISNULL(TS.PCS,1))ELSE SC.COMMISION END"
            StrSql += vbCrLf + ",COMM_GRM = SC.COMMISIONGRM,COMM_PER = SC.COMMISIONPER,UPDATED = 'Y'"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMPSALESCOMMISION AS TS"
            StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..SALES_COMMISION  AS SC ON SC.ITEMCTRID = TS.ITEMCTRID "
            If chkitemid <> "" And chkitemid <> "ALL" Then StrSql += vbCrLf + " AND SC.ITEMID = TS.ITEMID "
            If chksubitemid <> "" And chksubitemid <> "ALL" Then StrSql += vbCrLf + " AND SC.SUBITEMID = TS.SUBITEMID "
            StrSql += vbCrLf + " AND (CASE WHEN TS.CALTYPE IN ('R','F') THEN TS.AMOUNT ELSE TS.NETWT END) BETWEEN SC.FROM_VAL AND SC.TO_VAL"
            StrSql += vbCrLf + " WHERE ISNULL(TS.UPDATED,'') = ''"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        End If

        StrSql = vbCrLf + " UPDATE TEMPTABLEDB..TEMPSALESCOMMISION SET COMM_FLAT = (SC.COMMISION*ISNULL(TS.PCS,1))"
        'StrSql += vbCrLf + " CASE WHEN SC.BASEDON IN('V') THEN  (SC.COMMISION*ISNULL(TS.PCS,1))ELSE SC.COMMISION END"
        StrSql += vbCrLf + ",COMM_GRM = SC.COMMISIONGRM,COMM_PER = SC.COMMISIONPER,UPDATED = 'Y'"
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMPSALESCOMMISION AS TS"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..SALES_COMMISION  AS SC ON (SC.ITEMCTRID = TS.ITEMCTRID OR SC.ITEMCTRID = 0) AND SC.ITEMID = TS.ITEMID AND SC.SUBITEMID = TS.SUBITEMID "
        StrSql += vbCrLf + " AND (CASE WHEN TS.CALTYPE IN ('R','F') THEN TS.AMOUNT ELSE TS.NETWT END) BETWEEN SC.FROM_VAL AND SC.TO_VAL"
        StrSql += vbCrLf + " WHERE ISNULL(TS.UPDATED,'') = ''"

        StrSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMPSALESCOMMISION SET COMM_FLAT = (SC.COMMISION*ISNULL(TS.PCS,1))"
        'StrSql += vbCrLf + " CASE WHEN SC.BASEDON IN('V') THEN  (SC.COMMISION*ISNULL(TS.PCS,1))ELSE SC.COMMISION END"
        StrSql += vbCrLf + ",COMM_GRM = SC.COMMISIONGRM,COMM_PER = SC.COMMISIONPER,UPDATED = 'Y'"
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMPSALESCOMMISION AS TS"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..SALES_COMMISION  AS SC ON (SC.ITEMCTRID = TS.ITEMCTRID OR SC.ITEMCTRID = 0) AND SC.ITEMID = TS.ITEMID AND SC.SUBITEMID = 0 "
        StrSql += vbCrLf + " AND (CASE WHEN TS.CALTYPE IN ('R','F') THEN TS.AMOUNT ELSE TS.NETWT END) BETWEEN SC.FROM_VAL AND SC.TO_VAL"
        StrSql += vbCrLf + " WHERE ISNULL(TS.UPDATED,'') = ''"

        StrSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMPSALESCOMMISION SET COMMISION =  "
        StrSql += vbCrLf + " CASE WHEN ISNULL(COMM_PER,0) <> 0 THEN AMOUNT * (COMM_PER/100)else 0 end"
        StrSql += vbCrLf + " + CASE WHEN ISNULL(COMM_GRM,0) <> 0 THEN NETWT * COMM_GRM ELSE 0 END"
        StrSql += vbCrLf + " + CASE WHEN ISNULL(COMM_FLAT,0)<>0 THEN COMM_FLAT ELSE 0 END"
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMPSALESCOMMISION AS TS"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        If Val(txtCommPercentage_AMT.Text) <> 0 Then
            StrSql = vbCrLf + " UPDATE TEMPTABLEDB..TEMPSALESCOMMISION SET SALESCOMM=  "
            StrSql += vbCrLf + " ((COMMISION /100) * " & Val(txtCommPercentage_AMT.Text) & ") "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMPSALESCOMMISION AS TS"
            StrSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMPSALESCOMMISION SET OTHERCOMM=  "
            StrSql += vbCrLf + " ((COMMISION/100) * " & (100 - Val(txtCommPercentage_AMT.Text)) & ") "
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMPSALESCOMMISION AS TS"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        End If
        If chkshowroomincentive.Checked Then
            ShowroomIncentive()
            Exit Sub
        End If
        If chkmnthwise.Checked Then
            GroupingGridbymonthwise()
        Else
            GroupingGridbydefault()
        End If
    End Sub

    Private Sub GroupingGridbydefault()
        Dim grouper As String = ""
        Dim particular As String = ""
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

        StrSql = vbCrLf + " SELECT CONVERT(VARCHAR(300)," & particular & ")PARTICULAR"
        StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' AND COMMISION <> 0 THEN -1*COMMISION WHEN SEP = 'SA' AND COMMISION > 0 THEN COMMISION ELSE NULL END AS COMMISION"
        StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' AND SALESCOMM <> 0 THEN -1*SALESCOMM WHEN SEP = 'SA' AND SALESCOMM > 0 THEN SALESCOMM ELSE NULL END AS SALESCOMM"
        StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' AND OTHERCOMM <> 0 THEN -1*OTHERCOMM WHEN SEP = 'SA' AND OTHERCOMM > 0 THEN OTHERCOMM ELSE NULL END AS OTHERCOMM"
        StrSql += vbCrLf + " ,SUBITEMNAME,TRANNO,TRANDATE,TAGNO " & IIf(chkWithTagno.Checked, ",0 TAGNOS", "") & ""
        StrSql += vbCrLf + " ,CASE WHEN SEP = 'SA' THEN PCS ELSE NULL END AS SPCS"
        StrSql += vbCrLf + " ,CASE WHEN SEP = 'SA' THEN GRSWT ELSE NULL END AS SGRSWT"
        StrSql += vbCrLf + " ,CASE WHEN SEP = 'SA' THEN NETWT ELSE NULL END AS SNETWT"
        StrSql += vbCrLf + " ,CASE WHEN SEP = 'SA' THEN AMOUNT ELSE NULL END AS SAMOUNT"
        StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' THEN PCS ELSE NULL END AS RPCS"
        StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' THEN GRSWT ELSE NULL END AS RGRSWT"
        StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' THEN NETWT ELSE NULL END AS RNETWT"
        StrSql += vbCrLf + " ,CASE WHEN SEP = 'SR' THEN AMOUNT ELSE NULL END AS RAMOUNT"
        StrSql += vbCrLf + " ,CONVERT(VARCHAR(100),EMPNAME)AS EMPNAME,CONVERT(VARCHAR(100),ITEMCTRNAME) AS COUNTER"
        StrSql += vbCrLf + " ,CONVERT(VARCHAR(2),SEP)SEP,CONVERT(VARCHAR(3),'')AS COLHEAD,CONVERT(INT,1)AS RESULT"
        StrSql += vbCrLf + " ,CONVERT(VARCHAR(200)," & grouper & ")AS GROUPER,ITEM,METAL"
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMPSALESCOMMISION_RES"
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMPSALESCOMMISION"
        If chkWithEmptyCommision.Checked = False Then StrSql += vbCrLf + " WHERE ISNULL(COMMISION,0) <> 0"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        If rbtDetailed.Checked Then
            StrSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES)>0"
            StrSql += vbCrLf + " BEGIN"
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPSALESCOMMISION_RES(PARTICULAR,COLHEAD,RESULT,GROUPER," & IIf(rbtGrpCounter.Checked, "COUNTER", grouper) & ")"
            StrSql += vbCrLf + " SELECT DISTINCT GROUPER,'T',0 RESULT,GROUPER," & IIf(rbtGrpCounter.Checked, "COUNTER", grouper) & " FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES WHERE RESULT = 1"
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPSALESCOMMISION_RES(PARTICULAR,COLHEAD,RESULT,GROUPER," & IIf(rbtGrpCounter.Checked, "COUNTER", grouper) & ",COMMISION,SALESCOMM,OTHERCOMM,SPCS,SGRSWT,SNETWT,SAMOUNT,RPCS,RGRSWT,RNETWT,RAMOUNT)"
            StrSql += vbCrLf + " SELECT GROUPER + ' TOT','S',2 RESULT,GROUPER," & IIf(rbtGrpCounter.Checked, "COUNTER", grouper) & ",SUM(COMMISION),SUM(SALESCOMM),SUM(OTHERCOMM),SUM(SPCS),SUM(SGRSWT),SUM(SNETWT),SUM(SAMOUNT),SUM(RPCS),SUM(RGRSWT),SUM(RNETWT),SUM(RAMOUNT)"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES WHERE RESULT = 1 GROUP BY GROUPER," & IIf(rbtGrpCounter.Checked, "COUNTER", grouper) & ""
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPSALESCOMMISION_RES(PARTICULAR,COLHEAD,RESULT,GROUPER,COMMISION,SALESCOMM,OTHERCOMM,SPCS,SGRSWT,SNETWT,SAMOUNT,RPCS,RGRSWT,RNETWT,RAMOUNT)"
            StrSql += vbCrLf + " SELECT 'GRAND TOTAL','G',3 RESULT,'ZZZZZZZ' AS GROUPER,SUM(COMMISION),SUM(SALESCOMM),SUM(OTHERCOMM),SUM(SPCS),SUM(SGRSWT),SUM(SNETWT),SUM(SAMOUNT),SUM(RPCS),SUM(RGRSWT),SUM(RNETWT),SUM(RAMOUNT)"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES WHERE RESULT = 1"
            StrSql += vbCrLf + " END"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        Else
            StrSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES)>0"
            StrSql += vbCrLf + " BEGIN"
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPSALESCOMMISION_RES(PARTICULAR,COLHEAD,RESULT,GROUPER," & IIf(rbtGrpCounter.Checked, "COUNTER", grouper) & ",COMMISION" & IIf(chkWithTagno.Checked, ",TAGNOS", "") & ",SALESCOMM,OTHERCOMM,SPCS,SGRSWT,SNETWT,RPCS,RGRSWT,RNETWT)"
            StrSql += vbCrLf + " SELECT GROUPER ,'',2 RESULT,GROUPER," & IIf(rbtGrpCounter.Checked, "COUNTER", grouper) & ",SUM(COMMISION)"
            StrSql += vbCrLf + IIf(chkWithTagno.Checked, ",(SELECT COUNT(TAGNO) FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES WHERE TAGNO <>'' AND GROUPER=S.GROUPER)", "") & ""
            StrSql += vbCrLf + " ,SUM(SALESCOMM),SUM(OTHERCOMM),SUM(SPCS),SUM(SGRSWT),SUM(SNETWT),SUM(RPCS),SUM(RGRSWT),SUM(RNETWT)"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES S WHERE RESULT = 1 GROUP BY GROUPER," & IIf(rbtGrpCounter.Checked, "COUNTER", grouper) & ""
            StrSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPSALESCOMMISION_RES(PARTICULAR,COLHEAD,RESULT,GROUPER,COMMISION" & IIf(chkWithTagno.Checked, ",TAGNOS", "") & ",SALESCOMM,OTHERCOMM,SPCS,SGRSWT,SNETWT,RPCS,RGRSWT,RNETWT)"
            StrSql += vbCrLf + " SELECT 'GRAND TOTAL','G',3 RESULT,'ZZZZZZZ' AS GROUPER,SUM(COMMISION)" & IIf(chkWithTagno.Checked, ",0", "") & ",SUM(SALESCOMM),SUM(OTHERCOMM),SUM(SPCS),SUM(SGRSWT),SUM(SNETWT),SUM(RPCS),SUM(RGRSWT),SUM(RNETWT)"
            StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES WHERE RESULT = 1"
            StrSql += vbCrLf + " END"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
            StrSql = " DELETE FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES WHERE RESULT = 1"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        End If
        StrSql = vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES ORDER BY " & IIf(chkdesc.Checked, IIf(rbtGrpCounter.Checked, "COUNTER", grouper) & " DESC ", "GROUPER") & ""
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
        objGridShower.gridView.Columns("SAMOUNT").HeaderText = "SALES AMOUNT"
        objGridShower.gridView.Columns("RPCS").HeaderText = "RETURN PCS"
        objGridShower.gridView.Columns("RGRSWT").HeaderText = "RETURN GRSWT"
        objGridShower.gridView.Columns("RNETWT").HeaderText = "RETURN NETWT"
        objGridShower.gridView.Columns("RAMOUNT").HeaderText = "RETURN AMOUNT"
        objGridShower.Text = "EMPLOYEE WISE SALES COMMISION"
        Dim tit As String = "EMPLOYEE WISE SALES COMMISION" + vbCrLf
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
        StrSql = "  IF OBJECT_ID('TEMPTABLEDB..SA_COMMISION')IS NOT NULL DROP TABLE TEMPTABLEDB..SA_COMMISION"
        StrSql += vbCrLf + "  SELECT " & particular & ",DATENAME(MONTH,X.MNTH)MNTHNAME,MNTH,EI.WEIGHT TARGET"
        StrSql += vbCrLf + "  ,SUM((CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SAPCS,0) ELSE 0 END))SAPCS"
        StrSql += vbCrLf + "  ,SUM((CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SRPCS,0) ELSE 0  END))SRPCS "
        StrSql += vbCrLf + "  ,SUM((CASE WHEN ISNULL(METAL,'') NOT IN('OTHERS')THEN ISNULL(SASALES,0)ELSE 0 END))SASALES"
        StrSql += vbCrLf + "  ,SUM((CASE WHEN ISNULL(METAL,'') NOT IN('OTHERS')THEN ISNULL(SRSALES,0)ELSE 0 END))SRSALES "
        StrSql += vbCrLf + "  ,SUM(ISNULL(SACOMMISION,0))SACOMMISION"
        StrSql += vbCrLf + "  ,SUM(ISNULL(SRCOMMISION,0))SRCOMMISION"
        StrSql += vbCrLf + "  ,SUM(ISNULL(SABACKAMT,0))SABACKAMT"
        StrSql += vbCrLf + "  ,SUM(ISNULL(SRBACKAMT,0))SRBACKAMT"
        StrSql += vbCrLf + "  ,CASE WHEN SUM((((CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SAPCS,0) ELSE ISNULL(SASALES,0)END)-(CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SRPCS,0) ELSE ISNULL(SRSALES,0)END))/EI.WEIGHT)*100) BETWEEN 90 AND 99 THEN SUM((((ISNULL(SACOMMISION,0)-ISNULL(SRCOMMISION,0))/100)*75)) "
        StrSql += vbCrLf + "  WHEN SUM((((CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SAPCS,0) ELSE ISNULL(SASALES,0)END)-(CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SRPCS,0) ELSE ISNULL(SRSALES,0)END))/EI.WEIGHT)*100) >99 THEN SUM(ISNULL(SACOMMISION,0)-ISNULL(SRCOMMISION,0)) ELSE 0 END STFSALESINC"
        StrSql += vbCrLf + "  ,CASE WHEN SUM((((CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SAPCS,0) ELSE ISNULL(SASALES,0)END)-(CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SRPCS,0) ELSE ISNULL(SRSALES,0)END))/EI.WEIGHT)*100) BETWEEN 90 AND 99 THEN SUM((((ISNULL(SABACKAMT,0)-ISNULL(SRBACKAMT,0))/100)*75)) "
        StrSql += vbCrLf + "  WHEN SUM((((CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SAPCS,0) ELSE ISNULL(SASALES,0)END)-(CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SRPCS,0) ELSE ISNULL(SRSALES,0)END))/EI.WEIGHT)*100) >99 THEN SUM(ISNULL(SABACKAMT,0)-ISNULL(SRBACKAMT,0)) ELSE 0 END STFBACKINC"
        StrSql += vbCrLf + "  ,CASE WHEN SUM((((CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SAPCS,0) ELSE ISNULL(SASALES,0)END)-(CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SRPCS,0) ELSE ISNULL(SRSALES,0)END))/EI.WEIGHT)*100) BETWEEN 90 AND 99 THEN SUM((((ISNULL(SACOMMISION,0)-ISNULL(SRCOMMISION,0))/100)*75)+(ISNULL(SABACKAMT,0)-ISNULL(SRBACKAMT,0))) "
        StrSql += vbCrLf + "  WHEN SUM((((CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SAPCS,0) ELSE ISNULL(SASALES,0)END)-(CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SRPCS,0) ELSE ISNULL(SRSALES,0)END))/EI.WEIGHT)*100) >99 THEN SUM(ISNULL(SACOMMISION,0)-ISNULL(SRCOMMISION,0)+(ISNULL(SABACKAMT,0)-ISNULL(SRBACKAMT,0))) ELSE 0 END COMMISSION"
        StrSql += vbCrLf + "  ,CONVERT(DECIMAL(13,2),SUM((((CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SAPCS,0) ELSE ISNULL(SASALES,0)END)-(CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SRPCS,0) ELSE ISNULL(SRSALES,0)END))/EI.WEIGHT)*100))PER"
        StrSql += vbCrLf + "  INTO TEMPTABLEDB..SA_COMMISION "
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
        StrSql += vbCrLf + "  ,COSTID FROM TEMPTABLEDB..TEMPSALESCOMMISION S"
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


        StrSql = "  IF OBJECT_ID('TEMPTABLEDB..TEMP_OUT')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_OUT"
        StrSql += vbCrLf + "  SELECT DISTINCT EMPID,CONVERT(VARCHAR(200),EMPNAME + '['+ CONVERT(VARCHAR(4),EMPID) +']')PARTICULAR,EMPNAME,METAL,2 RESULT,'D'"
        StrSql += vbCrLf + "  COLHEAD INTO TEMPTABLEDB..TEMP_OUT FROM TEMPTABLEDB..SA_COMMISION "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        StrSql = "  INSERT INTO TEMPTABLEDB..TEMP_OUT(PARTICULAR,METAL,RESULT,COLHEAD)"
        StrSql += vbCrLf + "  SELECT DISTINCT METAL,METAL,0,'T' FROM TEMPTABLEDB..TEMP_OUT WHERE RESULT=2"
        StrSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP_OUT(PARTICULAR,METAL,RESULT,COLHEAD)     "
        StrSql += vbCrLf + "  SELECT 'SUBTOTAL',METAL,3,'S' FROM TEMPTABLEDB..TEMP_OUT WHERE RESULT=0"
        StrSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP_OUT(PARTICULAR,METAL,RESULT,COLHEAD)"
        StrSql += vbCrLf + "  SELECT 'GRANDTOTAL','ZZZZZ',4,'G' "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        StrSql = "  DECLARE @QRY VARCHAR(4000)"
        StrSql += vbCrLf + "  SET @QRY=''"
        StrSql += vbCrLf + "  DECLARE @A AS VARCHAR(10)"
        StrSql += vbCrLf + "  DECLARE @MNTH AS VARCHAR(10)"
        StrSql += vbCrLf + "  DECLARE CUR CURSOR FOR SELECT DISTINCT DATEPART(MM,CONVERT(SMALLDATETIME,MNTH))A,DATENAME(MONTH,MNTH)MNTH FROM TEMPTABLEDB..SA_COMMISION ORDER BY DATEPART(MM,CONVERT(SMALLDATETIME,MNTH))"
        StrSql += vbCrLf + "  OPEN CUR WHILE (1 = 1) BEGIN FETCH NEXT FROM CUR INTO @A,@MNTH"
        StrSql += vbCrLf + "  IF @@FETCH_STATUS =-1 BREAK                        "
        StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT ADD ['+ SUBSTRING(@MNTH,0,4) +'-TARGET] DECIMAL(15,3) DEFAULT 0' PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT ADD ['+ SUBSTRING(@MNTH,0,4)+'-SAPCS] DECIMAL(15,3) DEFAULT 0' PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT ADD ['+ SUBSTRING(@MNTH,0,4)+'-SALES] DECIMAL(15,3) DEFAULT 0' PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT ADD ['+ SUBSTRING(@MNTH,0,4)+'-SARETURN] DECIMAL(15,3) DEFAULT 0' PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT ADD ['+ SUBSTRING(@MNTH,0,4)+'-SARETURNPCS] DECIMAL(15,3) DEFAULT 0' PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT ADD ['+ SUBSTRING(@MNTH,0,4)+'-SACOMMISION] DECIMAL(15,2) DEFAULT 0' PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT ADD ['+ SUBSTRING(@MNTH,0,4)+'-SRCOMMISION] DECIMAL(15,2) DEFAULT 0' PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT ADD ['+ SUBSTRING(@MNTH,0,4)+'-SABACKAMT] DECIMAL(15,2) DEFAULT 0' PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT ADD ['+ SUBSTRING(@MNTH,0,4)+'-SRBACKAMT] DECIMAL(15,2) DEFAULT 0' PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT ADD ['+ SUBSTRING(@MNTH,0,4)+'-PER] DECIMAL(15,2) DEFAULT 0' PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT ADD ['+ SUBSTRING(@MNTH,0,4)+'-STFSALESINC] DECIMAL(15,3) DEFAULT 0' PRINT @QRY EXEC(@QRY)"
        StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT ADD ['+ SUBSTRING(@MNTH,0,4)+'-STFBACKINC] DECIMAL(15,3) DEFAULT 0' PRINT @QRY EXEC(@QRY)"
        StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT ADD ['+ SUBSTRING(@MNTH,0,4) +'-INCENTIVE] DECIMAL(15,2) DEFAULT 0' PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-TARGET]=(SELECT SUM(TARGET) FROM TEMPTABLEDB..SA_COMMISION WHERE EMPID=TT.EMPID AND METAL=TT.METAL '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT TT WHERE EMPID=EMPID AND METAL=METAL '"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY) "
        StrSql += vbCrLf + "  SELECT @QRY='UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SALES]=(SELECT SUM(SASALES) FROM TEMPTABLEDB..SA_COMMISION WHERE EMPID=TT.EMPID AND METAL=TT.METAL '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT TT WHERE EMPID=EMPID AND METAL=METAL '"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY='UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SAPCS]=(SELECT SUM(SAPCS) FROM TEMPTABLEDB..SA_COMMISION WHERE EMPID=TT.EMPID AND METAL=TT.METAL '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT TT WHERE EMPID=EMPID AND METAL=METAL '"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY='UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SARETURN]=(SELECT SUM(SRSALES) FROM TEMPTABLEDB..SA_COMMISION WHERE EMPID=TT.EMPID AND METAL=TT.METAL '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT TT WHERE EMPID=EMPID AND METAL=METAL '"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY='UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SARETURNPCS]=(SELECT SUM(SRPCS) FROM TEMPTABLEDB..SA_COMMISION WHERE EMPID=TT.EMPID AND METAL=TT.METAL '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT TT WHERE EMPID=EMPID AND METAL=METAL '"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY='UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SACOMMISION]=(SELECT SUM(SACOMMISION) FROM TEMPTABLEDB..SA_COMMISION WHERE EMPID=TT.EMPID AND METAL=TT.METAL '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT TT WHERE EMPID=EMPID AND METAL=METAL '"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY='UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SRCOMMISION]=(SELECT SUM(SRCOMMISION) FROM TEMPTABLEDB..SA_COMMISION WHERE EMPID=TT.EMPID AND METAL=TT.METAL '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT TT WHERE EMPID=EMPID AND METAL=METAL '"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY='UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SABACKAMT]=(SELECT SUM(SABACKAMT) FROM TEMPTABLEDB..SA_COMMISION WHERE EMPID=TT.EMPID AND METAL=TT.METAL '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT TT WHERE EMPID=EMPID AND METAL=METAL '"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY='UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SRBACKAMT]=(SELECT SUM(SRBACKAMT) FROM TEMPTABLEDB..SA_COMMISION WHERE EMPID=TT.EMPID AND METAL=TT.METAL '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT TT WHERE EMPID=EMPID AND METAL=METAL '"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY) "
        StrSql += vbCrLf + "  SELECT @QRY='UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-PER]=(SELECT SUM(PER) FROM TEMPTABLEDB..SA_COMMISION WHERE EMPID=TT.EMPID AND METAL=TT.METAL '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT TT WHERE EMPID=EMPID AND METAL=METAL '"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY='UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-STFSALESINC]=(SELECT SUM(STFSALESINC) FROM TEMPTABLEDB..SA_COMMISION WHERE EMPID=TT.EMPID AND METAL=TT.METAL '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT TT WHERE EMPID=EMPID AND METAL=METAL '"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)  "
        StrSql += vbCrLf + "  SELECT @QRY='UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-STFBACKINC]=(SELECT SUM(STFBACKINC) FROM TEMPTABLEDB..SA_COMMISION WHERE EMPID=TT.EMPID AND METAL=TT.METAL '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''' AND ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-TARGET],0)<> 0) FROM TEMPTABLEDB..TEMP_OUT TT WHERE EMPID=EMPID AND METAL=METAL '"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)  "
        StrSql += vbCrLf + "  SELECT @QRY='UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-INCENTIVE]=(SELECT SUM(COMMISSION) FROM TEMPTABLEDB..SA_COMMISION WHERE EMPID=TT.EMPID AND METAL=TT.METAL '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT TT WHERE EMPID=EMPID AND METAL=METAL '"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)  "
        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-TARGET]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-TARGET],0)) FROM TEMPTABLEDB..TEMP_OUT WHERE METAL=TT.METAL '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + 'AND COLHEAD=''D'') FROM TEMPTABLEDB..TEMP_OUT TT WHERE METAL=METAL AND COLHEAD=''S'''"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SALES]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SALES],0)) FROM TEMPTABLEDB..TEMP_OUT WHERE METAL=TT.METAL '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + 'AND COLHEAD=''D'') FROM TEMPTABLEDB..TEMP_OUT TT WHERE METAL=METAL AND COLHEAD=''S'''"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SAPCS]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SAPCS],0)) FROM TEMPTABLEDB..TEMP_OUT WHERE METAL=TT.METAL '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + 'AND COLHEAD=''D'') FROM TEMPTABLEDB..TEMP_OUT TT WHERE METAL=METAL AND COLHEAD=''S'''"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SARETURN]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SARETURN],0)) FROM TEMPTABLEDB..TEMP_OUT WHERE METAL=TT.METAL '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + 'AND COLHEAD=''D'') FROM TEMPTABLEDB..TEMP_OUT TT WHERE METAL=METAL AND COLHEAD=''S'''"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SARETURNPCS]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SARETURNPCS],0)) FROM TEMPTABLEDB..TEMP_OUT WHERE METAL=TT.METAL '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + 'AND COLHEAD=''D'') FROM TEMPTABLEDB..TEMP_OUT TT WHERE METAL=METAL AND COLHEAD=''S'''"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SACOMMISION]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SACOMMISION],0)) FROM TEMPTABLEDB..TEMP_OUT WHERE METAL=TT.METAL '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + 'AND COLHEAD=''D'') FROM TEMPTABLEDB..TEMP_OUT TT WHERE METAL=METAL AND COLHEAD=''S'''"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SRCOMMISION]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SRCOMMISION],0)) FROM TEMPTABLEDB..TEMP_OUT WHERE METAL=TT.METAL '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + 'AND COLHEAD=''D'') FROM TEMPTABLEDB..TEMP_OUT TT WHERE METAL=METAL AND COLHEAD=''S'''"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SABACKAMT]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SABACKAMT],0)) FROM TEMPTABLEDB..TEMP_OUT WHERE METAL=TT.METAL '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + 'AND COLHEAD=''D'') FROM TEMPTABLEDB..TEMP_OUT TT WHERE METAL=METAL AND COLHEAD=''S'''"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SRBACKAMT]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SRBACKAMT],0)) FROM TEMPTABLEDB..TEMP_OUT WHERE METAL=TT.METAL '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + 'AND COLHEAD=''D'') FROM TEMPTABLEDB..TEMP_OUT TT WHERE METAL=METAL AND COLHEAD=''S'''"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "


        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-STFSALESINC]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-STFSALESINC],0)) FROM TEMPTABLEDB..TEMP_OUT WHERE METAL=TT.METAL '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + 'AND COLHEAD=''D''AND ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-TARGET],0)<> 0) FROM TEMPTABLEDB..TEMP_OUT TT WHERE METAL=METAL AND COLHEAD=''S'''"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "

        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-STFBACKINC]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-STFBACKINC],0)) FROM TEMPTABLEDB..TEMP_OUT WHERE METAL=TT.METAL '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + 'AND COLHEAD=''D'' AND ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-TARGET],0)<> 0) FROM TEMPTABLEDB..TEMP_OUT TT WHERE METAL=METAL AND COLHEAD=''S'''"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "

        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-INCENTIVE]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-INCENTIVE],0)) FROM TEMPTABLEDB..TEMP_OUT WHERE METAL=TT.METAL '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + 'AND COLHEAD=''D'' AND ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-TARGET],0)<> 0) FROM TEMPTABLEDB..TEMP_OUT TT WHERE METAL=METAL AND COLHEAD=''S'''"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY) "


        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-TARGET]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-TARGET],0)) FROM TEMPTABLEDB..TEMP_OUT WHERE COLHEAD=''D'''"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ') FROM TEMPTABLEDB..TEMP_OUT TT WHERE  COLHEAD=''G'''"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "

        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SALES]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SALES],0)) FROM TEMPTABLEDB..TEMP_OUT WHERE COLHEAD=''D'''"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ') FROM TEMPTABLEDB..TEMP_OUT TT WHERE  COLHEAD=''G'''"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SARETURN]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SARETURN],0)) FROM TEMPTABLEDB..TEMP_OUT WHERE COLHEAD=''D'' '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ') FROM TEMPTABLEDB..TEMP_OUT TT WHERE  COLHEAD=''G'''"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "

        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SAPCS]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SAPCS],0)) FROM TEMPTABLEDB..TEMP_OUT WHERE COLHEAD=''D'''"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ') FROM TEMPTABLEDB..TEMP_OUT TT WHERE  COLHEAD=''G'''"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SARETURNPCS]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SARETURNPCS],0)) FROM TEMPTABLEDB..TEMP_OUT WHERE COLHEAD=''D'' '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ') FROM TEMPTABLEDB..TEMP_OUT TT WHERE  COLHEAD=''G'''"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "


        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SACOMMISION]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SACOMMISION],0)) FROM TEMPTABLEDB..TEMP_OUT WHERE COLHEAD=''D'' '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ') FROM TEMPTABLEDB..TEMP_OUT TT WHERE  COLHEAD=''G'''"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SRCOMMISION]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SRCOMMISION],0)) FROM TEMPTABLEDB..TEMP_OUT WHERE COLHEAD=''D'' '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ') FROM TEMPTABLEDB..TEMP_OUT TT WHERE  COLHEAD=''G'''"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "


        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SABACKAMT]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SABACKAMT],0)) FROM TEMPTABLEDB..TEMP_OUT WHERE COLHEAD=''D'' '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ') FROM TEMPTABLEDB..TEMP_OUT TT WHERE  COLHEAD=''G'''"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-SRBACKAMT]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SRBACKAMT],0)) FROM TEMPTABLEDB..TEMP_OUT WHERE COLHEAD=''D'' '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ') FROM TEMPTABLEDB..TEMP_OUT TT WHERE  COLHEAD=''G'''"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "

        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-STFSALESINC]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-STFSALESINC],0)) FROM TEMPTABLEDB..TEMP_OUT WHERE COLHEAD=''D'' '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-TARGET],0)<> 0) FROM TEMPTABLEDB..TEMP_OUT TT WHERE  COLHEAD=''G'''"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY) "

        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-STFBACKINC]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-STFBACKINC],0)) FROM TEMPTABLEDB..TEMP_OUT WHERE COLHEAD=''D'' '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-TARGET],0)<> 0) FROM TEMPTABLEDB..TEMP_OUT TT WHERE  COLHEAD=''G'''"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY) "

        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-INCENTIVE]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-INCENTIVE],0)) FROM TEMPTABLEDB..TEMP_OUT WHERE COLHEAD=''D'' '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-TARGET],0)<> 0) FROM TEMPTABLEDB..TEMP_OUT TT WHERE  COLHEAD=''G'''"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY) "


        StrSql += vbCrLf + "  SELECT @QRY='UPDATE TEMPTABLEDB..TEMP_OUT SET ['+ SUBSTRING(@MNTH,0,4) +'-TARGET]=NULL WHERE ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-TARGET],0)=0'"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' UPDATE TEMPTABLEDB..TEMP_OUT SET ['+ SUBSTRING(@MNTH,0,4) +'-SALES]=NULL WHERE ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SALES],0)=0'"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' UPDATE TEMPTABLEDB..TEMP_OUT SET ['+ SUBSTRING(@MNTH,0,4) +'-SARETURN]=NULL WHERE ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SARETURN],0)=0'"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' UPDATE TEMPTABLEDB..TEMP_OUT SET ['+ SUBSTRING(@MNTH,0,4) +'-SAPCS]=NULL WHERE ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SAPCS],0)=0'"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' UPDATE TEMPTABLEDB..TEMP_OUT SET ['+ SUBSTRING(@MNTH,0,4) +'-SARETURNPCS]=NULL WHERE ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SARETURNPCS],0)=0'"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' UPDATE TEMPTABLEDB..TEMP_OUT SET ['+ SUBSTRING(@MNTH,0,4) +'-SACOMMISION]=NULL WHERE ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SACOMMISION],0)=0'"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' UPDATE TEMPTABLEDB..TEMP_OUT SET ['+ SUBSTRING(@MNTH,0,4) +'-SRCOMMISION]=NULL WHERE ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SRCOMMISION],0)=0'"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' UPDATE TEMPTABLEDB..TEMP_OUT SET ['+ SUBSTRING(@MNTH,0,4) +'-SABACKAMT]=NULL WHERE ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SABACKAMT],0)=0'"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' UPDATE TEMPTABLEDB..TEMP_OUT SET ['+ SUBSTRING(@MNTH,0,4) +'-SRBACKAMT]=NULL WHERE ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-SRBACKAMT],0)=0'"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' UPDATE TEMPTABLEDB..TEMP_OUT SET ['+ SUBSTRING(@MNTH,0,4) +'-INCENTIVE]=NULL WHERE ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-INCENTIVE],0)=0'"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' UPDATE TEMPTABLEDB..TEMP_OUT SET ['+ SUBSTRING(@MNTH,0,4) +'-STFSALESINC]=NULL WHERE ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-INCENTIVE],0)=0'"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' UPDATE TEMPTABLEDB..TEMP_OUT SET ['+ SUBSTRING(@MNTH,0,4) +'-STFBACKINC]=NULL WHERE ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-INCENTIVE],0)=0'"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY) "
        StrSql += vbCrLf + "  END CLOSE CUR DEALLOCATE CUR   "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        Dim mna As New System.Globalization.DateTimeFormatInfo
        Dim mnth As String = mna.GetMonthName(Val(dtpFrom.Value.ToString.Substring(5, 2))).ToString
        StrSql = "  SELECT * FROM TEMPTABLEDB..TEMP_OUT ORDER BY METAL,RESULT,[" & mnth.ToString.Substring(0, 3) & "-PER] DESC,EMPNAME "
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
    End Sub

    Private Sub ShowroomIncentive()

        Dim particular As String = ""
        particular = "COSTNAME,MNTHNAME,TARGETAMOUNT"

        StrSql = "  IF OBJECT_ID('TEMPTABLEDB..SA_COMMISION')IS NOT NULL DROP TABLE TEMPTABLEDB..SA_COMMISION"
        StrSql += vbCrLf + " SELECT COSTNAME,MNTHNAME,TARGETAMOUNT,SUM(SAAMOUT)SAAMOUT,SUM(SRAMOUT)SRAMOUT"
        StrSql += vbCrLf + " ,(CASE WHEN ((SUM(SAAMOUT-SRAMOUT)/TARGETAMOUNT)*100) BETWEEN 90 AND 99 THEN SUM(((ISNULL(SACOMMISION,0)-ISNULL(SRCOMMISION,0))/100)*75) "
        StrSql += vbCrLf + " WHEN ((SUM(SAAMOUT-SRAMOUT)/TARGETAMOUNT)*100) >99 THEN SUM((ISNULL(SACOMMISION,0)-ISNULL(SRCOMMISION,0))) END) INCENTIVE"
        StrSql += vbCrLf + " ,SUM(SACOMMISION)COMMISION,SUM(SRCOMMISION)RETCOMMISION"
        StrSql += vbCrLf + " ,CASE WHEN ((SUM(SAAMOUT-SRAMOUT)/TARGETAMOUNT)*100)between 90.001 and 100 THEN CONVERT(VARCHAR(50),CONVERT(DECIMAL(15,2),((SUM(SAAMOUT-SRAMOUT)/TARGETAMOUNT)*100)))"
        StrSql += vbCrLf + " WHEN ((SUM(SAAMOUT-SRAMOUT)/TARGETAMOUNT)*100)< 90  THEN 'Below 90'ELSE 'Above 100' END PER"
        StrSql += vbCrLf + " ,COSTID,MNTH INTO TEMPTABLEDB..SA_COMMISION "
        StrSql += vbCrLf + " FROM ("
        StrSql += vbCrLf + " SELECT C.COSTNAME,S.METAL,CONVERT(VARCHAR(15),'2013-'+ SUBSTRING(DATENAME(MONTH,TRANDATE),0,4)+'-01')MNTH"
        StrSql += vbCrLf + " ,DATENAME(MONTH,CONVERT(VARCHAR(15),'2013-'+ SUBSTRING(DATENAME(MONTH,TRANDATE),0,4)+'-01'))MNTHNAME"
        StrSql += vbCrLf + " ,F.AMOUNT TARGETAMOUNT"
        StrSql += vbCrLf + " ,(CASE WHEN SEP='SA' THEN (S.AMOUNT)ELSE 0 END) SAAMOUT"
        StrSql += vbCrLf + " ,(CASE WHEN SEP='SR' THEN (S.AMOUNT)ELSE 0 END) SRAMOUT "
        StrSql += vbCrLf + " ,(CASE WHEN SEP='SA' THEN (S.COMMISION)ELSE 0 END) SACOMMISION"
        StrSql += vbCrLf + " ,(CASE WHEN SEP='SR' THEN (S.COMMISION)ELSE 0 END) SRCOMMISION"
        StrSql += vbCrLf + " ,C.COSTID FROM TEMPTABLEDB..TEMPSALESCOMMISION S"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..FLATINCENTIVE AS F ON F.COSTID=S.COSTID "
        StrSql += vbCrLf + " AND F.MONTH=DATENAME(MONTH,CONVERT(VARCHAR(15),'2014-'+ SUBSTRING(DATENAME(MONTH,TRANDATE),0,4)+'-01'))"
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..COSTCENTRE AS C ON C.COSTID=S.COSTID"
        StrSql += vbCrLf + " WHERE ISNULL(S.COMMISION,0) <> 0"
        StrSql += vbCrLf + "  )X GROUP BY COSTNAME,TARGETAMOUNT,MNTHNAME,COSTID,MNTH"

        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()


        StrSql = "  IF OBJECT_ID('TEMPTABLEDB..TEMP_OUT')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_OUT"
        StrSql += vbCrLf + "  SELECT COSTID,CONVERT(VARCHAR(200),COSTNAME)PARTICULAR,COSTNAME,MNTH,2 RESULT,'D'"
        StrSql += vbCrLf + "  COLHEAD INTO TEMPTABLEDB..TEMP_OUT FROM TEMPTABLEDB..SA_COMMISION "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        StrSql = "  INSERT INTO TEMPTABLEDB..TEMP_OUT(PARTICULAR,COSTID,RESULT,COLHEAD)"
        StrSql += vbCrLf + "  SELECT 'GRANDTOTAL','ZZ',4,'G' "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        StrSql = "  DECLARE @QRY VARCHAR(4000)"
        StrSql += vbCrLf + "  SET @QRY=''"
        StrSql += vbCrLf + "  DECLARE @A AS VARCHAR(10)"
        StrSql += vbCrLf + "  DECLARE @MNTH AS VARCHAR(10)"
        StrSql += vbCrLf + "  DECLARE CUR CURSOR FOR SELECT DISTINCT DATEPART(MM,CONVERT(SMALLDATETIME,MNTH))A,DATENAME(MONTH,MNTH)MNTH FROM TEMPTABLEDB..SA_COMMISION ORDER BY DATEPART(MM,CONVERT(SMALLDATETIME,MNTH))"
        StrSql += vbCrLf + "  OPEN CUR WHILE (1 = 1) BEGIN FETCH NEXT FROM CUR INTO @A,@MNTH"
        StrSql += vbCrLf + "  IF @@FETCH_STATUS =-1 BREAK"
        StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT ADD ['+ SUBSTRING(@MNTH,0,4) +'-TARGET] DECIMAL(15,3) DEFAULT 0' PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT ADD ['+ SUBSTRING(@MNTH,0,4)+'-AMOUNT] DECIMAL(15,3) DEFAULT 0' PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT ADD ['+ SUBSTRING(@MNTH,0,4)+'-RAMOUNT] DECIMAL(15,3) DEFAULT 0' PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT ADD ['+ SUBSTRING(@MNTH,0,4)+'-COMMISION] DECIMAL(15,2) DEFAULT 0' PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT ADD ['+ SUBSTRING(@MNTH,0,4)+'-RETCOMMISION] DECIMAL(15,2) DEFAULT 0' PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT ADD ['+ SUBSTRING(@MNTH,0,4)+'-PER] VARCHAR(15) DEFAULT NULL' PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY='ALTER TABLE TEMPTABLEDB..TEMP_OUT ADD ['+ SUBSTRING(@MNTH,0,4)+'-INCENTIVE] DECIMAL(15,3) DEFAULT 0' PRINT @QRY EXEC(@QRY)"

        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-TARGET]=(SELECT SUM(TARGETAMOUNT) FROM TEMPTABLEDB..SA_COMMISION WHERE COSTID=TT.COSTID   '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT TT WHERE COSTID=COSTID '"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY) "
        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-AMOUNT]=(SELECT SUM(SAAMOUT) FROM TEMPTABLEDB..SA_COMMISION WHERE COSTID=TT.COSTID   '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT TT WHERE COSTID=COSTID '"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY) "
        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-RAMOUNT]=(SELECT SUM(SRAMOUT) FROM TEMPTABLEDB..SA_COMMISION WHERE COSTID=TT.COSTID   '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT TT WHERE COSTID=COSTID  '"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY) "
        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-COMMISION]=(SELECT SUM(COMMISION) FROM TEMPTABLEDB..SA_COMMISION WHERE COSTID=TT.COSTID   '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT TT WHERE COSTID=COSTID  '"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY) "
        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-RETCOMMISION]=(SELECT SUM(RETCOMMISION) FROM TEMPTABLEDB..SA_COMMISION WHERE COSTID=TT.COSTID   '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT TT WHERE COSTID=COSTID  '"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY) "
        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-PER]=(SELECT TOP 1 PER FROM TEMPTABLEDB..SA_COMMISION WHERE COSTID=TT.COSTID  '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT TT WHERE COSTID=COSTID  '"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY) "
        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-INCENTIVE]=(SELECT SUM(INCENTIVE) FROM TEMPTABLEDB..SA_COMMISION WHERE COSTID=TT.COSTID   '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' AND DATENAME(MONTH,MNTH)='''+ @MNTH +''') FROM TEMPTABLEDB..TEMP_OUT TT WHERE COSTID=COSTID  '"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY) "

        'StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-TARGET]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-TARGET],0)) FROM TEMPTABLEDB..TEMP_OUT WHERE METAL=TT.METAL '"
        'StrSql += vbCrLf + "  SELECT @QRY= @QRY + 'AND COLHEAD=''D'') FROM TEMPTABLEDB..TEMP_OUT TT WHERE  COLHEAD=''S'''"
        'StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
        'StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-AMOUNT]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-AMOUNT],0)) FROM TEMPTABLEDB..TEMP_OUT WHERE METAL=TT.METAL '"
        'StrSql += vbCrLf + "  SELECT @QRY= @QRY + 'AND COLHEAD=''D'') FROM TEMPTABLEDB..TEMP_OUT TT WHERE  COLHEAD=''S'''"
        'StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
        'StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-RAMOUNT]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-RAMOUNT],0)) FROM TEMPTABLEDB..TEMP_OUT WHERE METAL=TT.METAL '"
        'StrSql += vbCrLf + "  SELECT @QRY= @QRY + 'AND COLHEAD=''D'') FROM TEMPTABLEDB..TEMP_OUT TT WHERE  COLHEAD=''S'''"
        'StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
        'StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-COMMISION]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-COMMISION],0)) FROM TEMPTABLEDB..TEMP_OUT WHERE METAL=TT.METAL '"
        'StrSql += vbCrLf + "  SELECT @QRY= @QRY + 'AND COLHEAD=''D'') FROM TEMPTABLEDB..TEMP_OUT TT WHERE  COLHEAD=''S'''"
        'StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
        'StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-RETCOMMISION]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-RETCOMMISION],0)) FROM TEMPTABLEDB..TEMP_OUT WHERE METAL=TT.METAL '"
        'StrSql += vbCrLf + "  SELECT @QRY= @QRY + 'AND COLHEAD=''D'') FROM TEMPTABLEDB..TEMP_OUT TT WHERE COLHEAD=''S'''"
        'StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
        'StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-INCENTIVE]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-INCENTIVE],0)) FROM TEMPTABLEDB..TEMP_OUT WHERE METAL=TT.METAL '"
        'StrSql += vbCrLf + "  SELECT @QRY= @QRY + 'AND COLHEAD=''D'') FROM TEMPTABLEDB..TEMP_OUT TT WHERE COLHEAD=''S'''"
        'StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "

        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-TARGET]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-TARGET],0)) FROM TEMPTABLEDB..TEMP_OUT WHERE COLHEAD=''D'''"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ') FROM TEMPTABLEDB..TEMP_OUT TT WHERE  COLHEAD=''G'''"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-AMOUNT]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-AMOUNT],0)) FROM TEMPTABLEDB..TEMP_OUT WHERE COLHEAD=''D'''"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ') FROM TEMPTABLEDB..TEMP_OUT TT WHERE  COLHEAD=''G'''"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-RAMOUNT]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-RAMOUNT],0)) FROM TEMPTABLEDB..TEMP_OUT WHERE COLHEAD=''D'' '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ') FROM TEMPTABLEDB..TEMP_OUT TT WHERE  COLHEAD=''G'''"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-COMMISION]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-COMMISION],0)) FROM TEMPTABLEDB..TEMP_OUT WHERE COLHEAD=''D'' '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ') FROM TEMPTABLEDB..TEMP_OUT TT WHERE  COLHEAD=''G'''"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-RETCOMMISION]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-RETCOMMISION],0)) FROM TEMPTABLEDB..TEMP_OUT WHERE COLHEAD=''D'' '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ') FROM TEMPTABLEDB..TEMP_OUT TT WHERE  COLHEAD=''G'''"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY=' UPDATE TT SET ['+ SUBSTRING(@MNTH,0,4) +'-INCENTIVE]=(SELECT SUM(ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-INCENTIVE],0)) FROM TEMPTABLEDB..TEMP_OUT WHERE COLHEAD=''D'' '"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ') FROM TEMPTABLEDB..TEMP_OUT TT WHERE  COLHEAD=''G'''"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY)                "
        StrSql += vbCrLf + "  SELECT @QRY='UPDATE TEMPTABLEDB..TEMP_OUT SET ['+ SUBSTRING(@MNTH,0,4) +'-TARGET]=NULL WHERE ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-TARGET],0)=0'"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' UPDATE TEMPTABLEDB..TEMP_OUT SET ['+ SUBSTRING(@MNTH,0,4) +'-AMOUNT]=NULL WHERE ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-AMOUNT],0)=0'"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' UPDATE TEMPTABLEDB..TEMP_OUT SET ['+ SUBSTRING(@MNTH,0,4) +'-RAMOUNT]=NULL WHERE ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-RAMOUNT],0)=0'"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' UPDATE TEMPTABLEDB..TEMP_OUT SET ['+ SUBSTRING(@MNTH,0,4) +'-COMMISION]=NULL WHERE ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-COMMISION],0)=0'"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' UPDATE TEMPTABLEDB..TEMP_OUT SET ['+ SUBSTRING(@MNTH,0,4) +'-RETCOMMISION]=NULL WHERE ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-RETCOMMISION],0)=0'"
        StrSql += vbCrLf + "  SELECT @QRY= @QRY + ' UPDATE TEMPTABLEDB..TEMP_OUT SET ['+ SUBSTRING(@MNTH,0,4) +'-INCENTIVE]=NULL WHERE ISNULL(['+ SUBSTRING(@MNTH,0,4) +'-INCENTIVE],0)=0'"
        StrSql += vbCrLf + "  PRINT @QRY EXEC(@QRY) "
        StrSql += vbCrLf + "  END CLOSE CUR DEALLOCATE CUR   "

        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        Dim mna As New System.Globalization.DateTimeFormatInfo
        Dim mnth As String = mna.GetMonthName(Val(dtpFrom.Value.ToString.Substring(5, 2))).ToString
        StrSql = "  SELECT * FROM TEMPTABLEDB..TEMP_OUT ORDER BY RESULT,[" & mnth.ToString.Substring(0, 3) & "-PER] DESC,COSTNAME "
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
        SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, "ALL")
        rbtSummary.Checked = obj.p_rbtSummary
        rbtDetailed.Checked = obj.p_rbtDetailed
        rbtGrpCounter.Checked = obj.p_rbtGrpCounter
        rbtGrpEmployee.Checked = obj.p_rbtGrpEmployee
        rbtproduct.Checked = obj.p_rbtproduct
        chkWithEmptyCommision.Checked = obj.p_chkWithEmptyCommision
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New RPT_SALES_COMMISION_Properties
        GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        obj.p_rbtSummary = rbtSummary.Checked
        obj.p_rbtDetailed = rbtDetailed.Checked
        obj.p_chkWithEmptyCommision = chkWithEmptyCommision.Checked
        obj.p_rbtGrpCounter = rbtGrpCounter.Checked
        obj.p_rbtGrpEmployee = rbtGrpEmployee.Checked
        obj.p_rbtproduct = rbtproduct.Checked
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
        GiritechPack.GlobalMethods.FillCombo(chkcmbitemname, dtITEM, "ITEMNAME", , "ALL")
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
        GiritechPack.GlobalMethods.FillCombo(chkcmbsubitem, dtsubITEM, "SUBITEMNAME", , "ALL")

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

End Class