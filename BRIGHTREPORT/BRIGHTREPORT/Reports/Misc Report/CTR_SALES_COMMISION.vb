Imports System.Data.OleDb

Public Class CTR_SALES_COMMISION
    Dim StrSql As String
    Dim Da As OleDbDataAdapter
    Dim Cmd As OleDbCommand
    Dim objGridShower As frmGridDispDia
    Dim dtCostCentre As New DataTable
    Dim dtEMP, dtmetal, dtsubITEM, dtCounter As New DataTable
    Dim dtITEM As New DataTable
    Dim dtCompany As New DataTable
    Dim itemfilter As String = ""
    Dim Cashfilter As String = ""
    Dim insper As Double = 0
    Dim Sysid As String



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

        StrSql = " SELECT 'ALL' EMPNAME,'ALL' EMPID,1 RESULT"
        StrSql += " UNION ALL"
        StrSql += " SELECT EMPNAME,CONVERT(vARCHAR,EMPID),2 RESULT FROM " & cnAdminDb & "..EMPMASTER"
        StrSql += " ORDER BY RESULT,EMPNAME"
        dtEMP = New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtEMP)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbemployee, dtEMP, "EMPNAME", , "ALL")
        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        ' chkWithItem.Checked = False
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , "ALL")
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        BrighttechPack.GlobalMethods.FillCombo(chkcmbitemname, dtITEM, "ITEMNAME", , "ALL")
        BrighttechPack.GlobalMethods.FillCombo(chkcmbemployee, dtEMP, "EMPNAME", , "ALL")
        BrighttechPack.GlobalMethods.FillCombo(chkcmbsubitem, dtsubITEM, "SUBITEMNAME", , "ALL")
        BrighttechPack.GlobalMethods.FillCombo(chkcmbmetal, dtmetal, "METALNAME", , "ALL")
        BrighttechPack.GlobalMethods.FillCombo(chkcmbCounter, dtCounter, "ITEMCTRNAME", , "ALL")
        dtpFrom.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Sysid = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
        itemfilter = ""
        Cashfilter = ""
        Dim chkMetalid As String = GetSelectedMetalid(chkcmbmetal, True)
        Dim chkCompId As String = GetQryStringForSp(chkCmbCompany.Text, cnAdminDb & "..COMPANY", "COMPANYID", "COMPANYNAME", True)
        Dim chkCostId As String = GetQryStringForSp(chkCmbCostCentre.Text, cnAdminDb & "..COSTCENTRE", "COSTID", "COSTNAME", True)
        Dim chkempid As String = GetQryStringForSp(chkcmbemployee.Text, cnAdminDb & "..EMPMASTER", "EMPID", "EMPNAME", True)
        Dim chkitemid As String = GetQryStringForSp(chkcmbitemname.Text, cnAdminDb & "..ITEMMAST", "ITEMID", "ITEMNAME", True)
        Dim chksubitemid As String = GetQryStringForSp(chkcmbsubitem.Text, cnAdminDb & "..SUBITEMMAST", "SUBITEMID", "SUBITEMNAME", True)
        Dim StoneDetail As Boolean = True
        Dim chkItemCtr As String = GetQryStringForSp(chkcmbCounter.Text, cnAdminDb & "..ITEMCOUNTER", "ITEMCTRID", "ITEMCTRNAME", True)

        Dim strEmp As String = ""
        'If cmbViewBy.Text = "TRANDATE,EMPLOYEE WISE SUMMARY" Then
        strEmp = "EM.EMPNAME "
        'Else
        '    strEmp = "EM.EMPNAME + ' ( ' + CONVERT(VARCHAR,EM.EMPID) + ' )' "
        'End If
        StrSql = "IF OBJECT_ID('" & cnAdminDb & "..FLATINCENTIVE')IS NULL CREATE TABLE " & cnAdminDb & "..FLATINCENTIVE(COSTID VARCHAR(2),MONTH VARCHAR(15),AMOUNT NUMERIC(15,3),CONSTRAINT [COSTID_MONTH] UNIQUE NONCLUSTERED(COSTID,MONTH))"
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.ExecuteNonQuery()

        CheckAndInsertsoftcontrol("INCENTIVEEXCLUDEITEM", "EXCLUDE ITEM FOR REPORT", "T", "", "A")
        StrSql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='INCENTIVEEXCLUDEITEM'"
        itemfilter = GetSqlValue(cn, StrSql)

        CheckAndInsertsoftcontrol("INCENTIVEEXCLUDE_CASHCTR", "EXCLUDE CASH COUNTER FOR REPORT", "T", "", "A")
        StrSql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='INCENTIVEEXCLUDE_CASHCTR'"
        Dim _Cashfilter() As String = GetSqlValue(cn, StrSql).ToString.Split(",")
        For k As Integer = 0 To _Cashfilter.Length - 1
            If Cashfilter <> "" And _Cashfilter(k).ToString <> "" Then
                Cashfilter += ",'" & _Cashfilter(k).ToString & "'"
            Else
                Cashfilter += "'" & _Cashfilter(k).ToString & "'"
            End If

        Next

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
        StrSql += vbCrLf + " ,I.PCS,I.GRSWT,I.NETWT"
        If StoneDetail = True Then
            StrSql += vbCrLf + " ,0 STNPCS,0 STNWT_G,0 STNWT_C,0 DIAPCS,0 DIAWT_C"
        End If
        StrSql += vbCrLf + " ,I.AMOUNT"
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
        If Cashfilter <> "" Then StrSql += " AND I.CASHID NOT IN(" & Cashfilter & ")"

        StrSql += vbCrLf + " UNION ALL "
        StrSql += vbCrLf + " SELECT CONVERT(NUMERIC(15,2),0) AS COMMISION,CONVERT(NUMERIC(15,2),0) AS SALESCOMM,CONVERT(NUMERIC(15,2),0) AS RTNCOMM"
        StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0) AS OTHERCOMM,M.METALNAME AS METAL,IM.ITEMNAME +'(' +CONVERT (VARCHAR,IM.ITEMID)+')' AS ITEM"
        StrSql += vbCrLf + " ,SM.SUBITEMNAME +'(' +CONVERT (VARCHAR,SM.SUBITEMID)+')' AS SUBITEMNAME ,ISS.TRANNO,ISS.TRANDATE,I.TAGNO,0 PCS"
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
        StrSql += vbCrLf + " ,ISS.STNAMT"
        StrSql += vbCrLf + " ,ISNULL(SM.CALTYPE,IM.CALTYPE) AS CALTYPE,CONVERT(NUMERIC(15,2),0) AS COMM_FLAT"
        StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0) AS COMM_GRM,CONVERT(NUMERIC(15,2),0) AS COMM_PER"
        StrSql += vbCrLf + " ,CONVERT(VARCHAR(1),'S')AS UPDATED"
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
        'Cashfilter

        StrSql += vbCrLf + " )X "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        StrSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & ""

        StrSql += vbCrLf + " SELECT * FROM (SELECT CONVERT(NUMERIC(15,2),0) AS COMMISION"
        StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0) AS SALESCOMM,CONVERT(NUMERIC(15,2),0) AS RTNCOMM,CONVERT(NUMERIC(15,2),0) AS OTHERCOMM"
        StrSql += vbCrLf + " ,M.METALNAME AS METAL,IM.ITEMNAME + '('+ CONVERT(VARCHAR,I.ITEMID) +')' AS ITEM,SM.SUBITEMNAME + '('+ CONVERT(VARCHAR,I.SUBITEMID) +')' SUBITEMNAME,I.TRANNO,I.TRANDATE"
        StrSql += vbCrLf + " ,I.TAGNO"
        StrSql += vbCrLf + " ,I.PCS,I.GRSWT,I.NETWT"
        If StoneDetail = True Then
            StrSql += vbCrLf + " ,0 STNPCS,0 STNWT_G,0 STNWT_C,0 DIAPCS,0 DIAWT_C"
        End If
        StrSql += vbCrLf + " ,I.AMOUNT,ISNULL(SM.CALTYPE,IM.CALTYPE) AS CALTYPE"
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
        If Cashfilter <> "" Then StrSql += " AND I.CASHID NOT IN(" & Cashfilter & ")"


        StrSql += vbCrLf + " UNION ALL "
        StrSql += vbCrLf + " SELECT CONVERT(NUMERIC(15,2),0) AS COMMISION,CONVERT(NUMERIC(15,2),0) AS SALESCOMM,CONVERT(NUMERIC(15,2),0) AS RTNCOMM"
        StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0) AS OTHERCOMM,M.METALNAME AS METAL,IM.ITEMNAME +'(' +CONVERT (VARCHAR,IR.STNITEMID)+')' AS ITEM"
        StrSql += vbCrLf + " ,SM.SUBITEMNAME +'('+CONVERT (VARCHAR,IR.STNSUBITEMID)+')' AS SUBITEMNAME,IR.TRANNO,IR.TRANDATE,I.TAGNO"
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

        StrSql += vbCrLf + " ,IR.STNAMT"
        StrSql += vbCrLf + " ,ISNULL(SM.CALTYPE,IM.CALTYPE) AS CALTYPE,CONVERT(NUMERIC(15,2),0) AS COMM_FLAT"
        StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0) AS COMM_GRM,CONVERT(NUMERIC(15,2),0) AS COMM_PER"
        StrSql += vbCrLf + " ,CONVERT(VARCHAR(1),'S')AS UPDATED"
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
        'Cashfilter
        StrSql += vbCrLf + " )X "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        'If rbtGrpCounter.Checked Then
        StrSql = vbCrLf + " UPDATE TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " SET COMM_FLAT = (SC.COMMISION*ISNULL(TS.PCS,1))"
        'StrSql += vbCrLf + " CASE WHEN SC.BASEDON IN('V') THEN  (SC.COMMISION*ISNULL(TS.PCS,1))ELSE SC.COMMISION END"
        StrSql += vbCrLf + ",COMM_GRM = SC.COMMISIONGRM,COMM_PER = SC.COMMISIONPER,UPDATED = 'Y'"
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " AS TS"
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SALES_COMMISION  AS SC ON SC.ITEMCTRID = TS.ITEMCTRID "
        StrSql += vbCrLf + " AND SC.ITEMID = TS.ITEMID "
        StrSql += vbCrLf + " AND SC.SUBITEMID = TS.SUBITEMID "
        StrSql += vbCrLf + " AND (CASE WHEN TS.CALTYPE IN ('R','F') THEN TS.AMOUNT ELSE TS.NETWT END) BETWEEN SC.FROM_VAL AND SC.TO_VAL"
        StrSql += vbCrLf + " WHERE ISNULL(TS.UPDATED,'') = ''"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        ''Newly Added for Stone/diamond Commission
        StrSql = vbCrLf + " UPDATE TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " SET COMM_FLAT = (SC.COMMISION*ISNULL(TS.PCS,1))"
        StrSql += vbCrLf + ",COMM_GRM = SC.COMMISIONGRM,COMM_PER = SC.COMMISIONPER,UPDATED = 'Y'"
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " AS TS"
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SALES_COMMISION  AS SC ON SC.ITEMCTRID = TS.ITEMCTRID "
        StrSql += vbCrLf + " AND SC.ITEMID = TS.ITEMID "
        StrSql += vbCrLf + " AND SC.SUBITEMID = TS.SUBITEMID "
        StrSql += vbCrLf + " AND (CASE WHEN TS.CALTYPE IN ('R','F') THEN TS.AMOUNT ELSE "
        StrSql += vbCrLf + " (CASE WHEN ISNULL(TS.STNWT_G,0)<>0 THEN ISNULL(TS.STNWT_G,0) WHEN ISNULL(TS.STNWT_C,0)<>0 THEN ISNULL(TS.STNWT_C,0)  "
        StrSql += vbCrLf + " WHEN ISNULL(TS.DIAWT_C,0)<>0 THEN ISNULL(TS.DIAWT_C,0) END) "
        StrSql += vbCrLf + " END) BETWEEN SC.FROM_VAL And SC.TO_VAL"
        StrSql += vbCrLf + " WHERE ISNULL(TS.UPDATED,'') = 'S'"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        ''End Newly Added 
        'End If

        'StrSql = vbCrLf + " UPDATE TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " SET COMM_FLAT = (SC.COMMISION*ISNULL(TS.PCS,1))"
        ''StrSql += vbCrLf + " CASE WHEN SC.BASEDON IN('V') THEN  (SC.COMMISION*ISNULL(TS.PCS,1))ELSE SC.COMMISION END"
        'StrSql += vbCrLf + ",COMM_GRM = SC.COMMISIONGRM,COMM_PER = SC.COMMISIONPER,UPDATED = 'Y'"
        'StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " AS TS"
        'StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..SALES_COMMISION  AS SC ON (SC.ITEMCTRID = TS.ITEMCTRID OR ISNULL(SC.ITEMCTRID,0) = 0) AND SC.ITEMID = TS.ITEMID AND SC.SUBITEMID = TS.SUBITEMID "
        'StrSql += vbCrLf + " AND (CASE WHEN TS.CALTYPE IN ('R','F') THEN TS.AMOUNT ELSE TS.NETWT END) BETWEEN SC.FROM_VAL AND SC.TO_VAL"
        'StrSql += vbCrLf + " WHERE ISNULL(TS.UPDATED,'') = ''"

        'StrSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " SET COMM_FLAT = (SC.COMMISION*ISNULL(TS.PCS,1))"
        ''StrSql += vbCrLf + " CASE WHEN SC.BASEDON IN('V') THEN  (SC.COMMISION*ISNULL(TS.PCS,1))ELSE SC.COMMISION END"
        'StrSql += vbCrLf + ",COMM_GRM = SC.COMMISIONGRM,COMM_PER = SC.COMMISIONPER,UPDATED = 'Y'"
        'StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " AS TS"
        'StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..SALES_COMMISION  AS SC ON (SC.ITEMCTRID = TS.ITEMCTRID OR ISNULL(SC.ITEMCTRID,0) = 0) AND SC.ITEMID = TS.ITEMID AND SC.SUBITEMID = 0 "
        'StrSql += vbCrLf + " AND (CASE WHEN TS.CALTYPE IN ('R','F') THEN TS.AMOUNT ELSE TS.NETWT END) BETWEEN SC.FROM_VAL AND SC.TO_VAL"
        'StrSql += vbCrLf + " WHERE ISNULL(TS.UPDATED,'') = ''"

        StrSql = vbCrLf + " UPDATE TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " SET COMMISION =  "
        StrSql += vbCrLf + " CASE WHEN ISNULL(COMM_PER,0) <> 0 THEN AMOUNT * (COMM_PER/100)else 0 end"
        StrSql += vbCrLf + " + CASE WHEN ISNULL(COMM_GRM,0) <> 0 THEN NETWT * COMM_GRM ELSE 0 END"
        StrSql += vbCrLf + " + CASE WHEN ISNULL(COMM_FLAT,0)<>0 THEN COMM_FLAT ELSE 0 END"
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " AS TS"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        GroupingGridbydefault()
    End Sub

    Private Sub GroupingGridbydefault()
        StrSql = " IF OBJECT_ID('TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & "1') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & "1  "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        Dim dtSource As New DataTable
        If rbtDetailed.Checked Then
            StrSql = "    SELECT CONVERT(VARCHAR(300),EMPNAME)PARTICULAR,CONVERT(VARCHAR(100),ITEMCTRNAME) AS COUNTER "
            StrSql += vbCrLf + "    ,CASE WHEN SEP = 'SA' THEN PCS ELSE NULL END AS SPCS"
            StrSql += vbCrLf + "    ,CASE WHEN SEP = 'SA' THEN GRSWT ELSE NULL END AS SGRSWT"
            StrSql += vbCrLf + "    ,CASE WHEN SEP = 'SA' THEN NETWT ELSE NULL END AS SNETWT"
            StrSql += vbCrLf + "    ,CASE WHEN SEP = 'SA' THEN DIAWT_C ELSE NULL END AS SDIAWT_C "
            StrSql += vbCrLf + "    ,CASE WHEN SEP = 'SA' THEN S.AMOUNT ELSE NULL END AS SAMOUNT"
            StrSql += vbCrLf + "    ,CASE WHEN SEP = 'SR' THEN PCS ELSE NULL END AS RPCS"
            StrSql += vbCrLf + "    ,CASE WHEN SEP = 'SR' THEN GRSWT ELSE NULL END AS RGRSWT"
            StrSql += vbCrLf + "    ,CASE WHEN SEP = 'SR' THEN NETWT ELSE NULL END AS RNETWT"
            StrSql += vbCrLf + "    ,CASE WHEN SEP = 'SR' THEN DIAWT_C ELSE NULL END AS RDIAWT_C"
            StrSql += vbCrLf + "    ,CASE WHEN SEP = 'SR' THEN S.AMOUNT ELSE NULL END AS RAMOUNT "
            StrSql += vbCrLf + "    ,CASE WHEN SEP = 'SA' THEN PCS ELSE -1*PCS END AS TPCS"
            StrSql += vbCrLf + "    ,CASE WHEN SEP = 'SA' THEN GRSWT ELSE -1*GRSWT END AS TGRSWT"
            StrSql += vbCrLf + "    ,CASE WHEN SEP = 'SA' THEN NETWT ELSE -1*NETWT END AS TNETWT"
            StrSql += vbCrLf + "    ,CASE WHEN SEP = 'SA' THEN DIAWT_C ELSE -1*DIAWT_C END AS TDIAWT_C"
            StrSql += vbCrLf + "    ,CASE WHEN SEP = 'SA' THEN S.AMOUNT ELSE -1*S.AMOUNT END AS TAMOUNT "
            StrSql += vbCrLf + "    ,CASE WHEN SEP = 'SR' AND COMMISION <> 0 THEN -1*COMMISION WHEN SEP = 'SA' AND COMMISION > 0 THEN COMMISION ELSE NULL END AS COMMISION"
            StrSql += vbCrLf + "    ,SUBITEMNAME,TRANNO,TRANDATE,TAGNO "
            StrSql += vbCrLf + "    ,CONVERT(VARCHAR(100),EMPNAME)AS EMPNAME"
            StrSql += vbCrLf + "    ,CONVERT(VARCHAR(2),SEP)SEP,CONVERT(VARCHAR(3),'')AS COLHEAD,CONVERT(INT,1)AS RESULT"
            StrSql += vbCrLf + "    ,CONVERT(VARCHAR(200),METAL)AS GROUPER,ITEM,METAL"
            StrSql += vbCrLf + "    ,EMPID,S.COSTID "
            StrSql += vbCrLf + "    INTO TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & "1 "
            StrSql += vbCrLf + "    FROM TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " S"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

            StrSql = " INSERT INTO TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & "1 (PARTICULAR,METAL,RESULT,COLHEAD)"
            StrSql += vbCrLf + " SELECT DISTINCT METAL,METAL,0 RESULT,'T' AS COLHEAD FROM TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " S"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

            StrSql = "    INSERT INTO TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & "1"
            StrSql += vbCrLf + "    (PARTICULAR,SPCS,SGRSWT,SNETWT,SDIAWT_C,SAMOUNT,"
            StrSql += vbCrLf + "    RPCS,RGRSWT,RNETWT,RDIAWT_C,RAMOUNT,"
            StrSql += vbCrLf + "    TPCS,TGRSWT,TNETWT,TDIAWT_C,TAMOUNT,COMMISION,EMPNAME,METAL,RESULT,COLHEAD)"
            StrSql += vbCrLf + "    SELECT CONVERT(VARCHAR(300),METAL)PARTICULAR    "
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SA' THEN PCS ELSE 0 END ) AS SPCS"
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SA' THEN GRSWT ELSE 0 END ) AS SGRSWT"
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SA' THEN NETWT ELSE 0 END ) AS SNETWT"
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SA' THEN DIAWT_C ELSE 0 END ) AS SDIAWT_C "
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SA' THEN S.AMOUNT ELSE 0 END ) AS SAMOUNT"
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SR' THEN PCS ELSE 0 END ) AS RPCS"
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SR' THEN GRSWT ELSE 0 END ) AS RGRSWT"
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SR' THEN NETWT ELSE 0 END ) AS RNETWT"
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SR' THEN DIAWT_C ELSE 0 END ) AS RDIAWT_C"
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SR' THEN S.AMOUNT ELSE 0 END ) AS RAMOUNT "
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SA' THEN PCS ELSE -1*PCS END ) AS TPCS"
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SA' THEN GRSWT ELSE -1*GRSWT END ) AS TGRSWT"
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SA' THEN NETWT ELSE -1*NETWT END ) AS TNETWT"
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SA' THEN DIAWT_C ELSE -1*DIAWT_C END ) AS TDIAWT_C"
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SA' THEN S.AMOUNT ELSE -1*S.AMOUNT END ) AS TAMOUNT "
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SR' AND COMMISION <> 0 THEN -1*COMMISION WHEN SEP = 'SA' AND COMMISION > 0 THEN COMMISION ELSE 0 END ) AS COMMISION"
            StrSql += vbCrLf + "    ,'ZZZZZZ',METAL,2 RESULT,'S' AS COLHEAD "
            StrSql += vbCrLf + "    FROM TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " S"
            StrSql += vbCrLf + "    GROUP BY METAL"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
            StrSql = " SELECT * FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & "1 ORDER BY METAL,RESULT,EMPNAME,TRANDATE,TRANNO "
            dtSource.Columns.Add("KEYNO", GetType(Integer))
            dtSource.Columns("KEYNO").AutoIncrement = True
            dtSource.Columns("KEYNO").AutoIncrementSeed = 0
            dtSource.Columns("KEYNO").AutoIncrementStep = 1
            Da = New OleDbDataAdapter(StrSql, cn)
            Da.Fill(dtSource)
        Else

            StrSql = "    SELECT CONVERT(VARCHAR(300),EMPNAME)PARTICULAR    "
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SA' THEN PCS ELSE 0 END ) AS SPCS"
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SA' THEN GRSWT ELSE 0 END ) AS SGRSWT"
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SA' THEN NETWT ELSE 0 END ) AS SNETWT"
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SA' THEN DIAWT_C ELSE 0 END ) AS SDIAWT_C "
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SA' THEN S.AMOUNT ELSE 0 END ) AS SAMOUNT"
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SR' THEN PCS ELSE 0 END ) AS RPCS"
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SR' THEN GRSWT ELSE 0 END ) AS RGRSWT"
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SR' THEN NETWT ELSE 0 END ) AS RNETWT"
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SR' THEN DIAWT_C ELSE 0 END ) AS RDIAWT_C"
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SR' THEN S.AMOUNT ELSE 0 END ) AS RAMOUNT "
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SA' THEN PCS ELSE -1*PCS END ) AS TPCS"
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SA' THEN GRSWT ELSE -1*GRSWT END ) AS TGRSWT"
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SA' THEN NETWT ELSE -1*NETWT END ) AS TNETWT"
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SA' THEN DIAWT_C ELSE -1*DIAWT_C END ) AS TDIAWT_C"
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SA' THEN S.AMOUNT ELSE -1*S.AMOUNT END ) AS TAMOUNT "
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SR' AND COMMISION <> 0 THEN -1*COMMISION WHEN SEP = 'SA' AND COMMISION > 0 THEN COMMISION ELSE 0 END ) AS COMMISION"
            StrSql += vbCrLf + "    ,EMPNAME,2 RESULT,'' AS COLHEAD "
            StrSql += vbCrLf + "    INTO TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & "1 "
            StrSql += vbCrLf + "    FROM TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " S"
            StrSql += vbCrLf + "    GROUP BY EMPNAME"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

            StrSql = "    INSERT INTO TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & "1"
            StrSql += vbCrLf + "    (PARTICULAR,SPCS,SGRSWT,SNETWT,SDIAWT_C,SAMOUNT,"
            StrSql += vbCrLf + "    RPCS,RGRSWT,RNETWT,RDIAWT_C,RAMOUNT,"
            StrSql += vbCrLf + "    TPCS,TGRSWT,TNETWT,TDIAWT_C,TAMOUNT,COMMISION,EMPNAME,RESULT,COLHEAD)"
            StrSql += vbCrLf + "    SELECT CONVERT(VARCHAR(300),'GRAND TOTAL')PARTICULAR    "
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SA' THEN PCS ELSE 0 END ) AS SPCS"
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SA' THEN GRSWT ELSE 0 END ) AS SGRSWT"
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SA' THEN NETWT ELSE 0 END ) AS SNETWT"
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SA' THEN DIAWT_C ELSE 0 END ) AS SDIAWT_C "
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SA' THEN S.AMOUNT ELSE 0 END ) AS SAMOUNT"
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SR' THEN PCS ELSE 0 END ) AS RPCS"
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SR' THEN GRSWT ELSE 0 END ) AS RGRSWT"
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SR' THEN NETWT ELSE 0 END ) AS RNETWT"
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SR' THEN DIAWT_C ELSE 0 END ) AS RDIAWT_C"
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SR' THEN S.AMOUNT ELSE 0 END ) AS RAMOUNT "
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SA' THEN PCS ELSE -1*PCS END ) AS TPCS"
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SA' THEN GRSWT ELSE -1*GRSWT END ) AS TGRSWT"
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SA' THEN NETWT ELSE -1*NETWT END ) AS TNETWT"
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SA' THEN DIAWT_C ELSE -1*DIAWT_C END ) AS TDIAWT_C"
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SA' THEN S.AMOUNT ELSE -1*S.AMOUNT END ) AS TAMOUNT "
            StrSql += vbCrLf + "    ,SUM(CASE WHEN SEP = 'SR' AND COMMISION <> 0 THEN -1*COMMISION WHEN SEP = 'SA' AND COMMISION > 0 THEN COMMISION ELSE 0 END ) AS COMMISION"
            StrSql += vbCrLf + "    ,'ZZZZZZ',2 RESULT,'G' AS COLHEAD "
            StrSql += vbCrLf + "    FROM TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " S"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

            StrSql = " SELECT * FROM TEMPTABLEDB..TEMPSALESCOMMISION_RES" & Sysid & "1 ORDER BY EMPNAME"
            dtSource.Columns.Add("KEYNO", GetType(Integer))
            dtSource.Columns("KEYNO").AutoIncrement = True
            dtSource.Columns("KEYNO").AutoIncrementSeed = 0
            dtSource.Columns("KEYNO").AutoIncrementStep = 1
            Da = New OleDbDataAdapter(StrSql, cn)
            Da.Fill(dtSource)
        End If
        

        

        
        

        Dim objGridShower As New frmGridDispDia
        objGridShower.gridView.DataSource = Nothing
        objGridShower.gridView.DataSource = dtSource
        objGridShower.Name = Me.Name
        objGridShower.WindowState = FormWindowState.Maximized
        objGridShower.gridView.Name = Me.Name
        objGridShower.gridView.Columns("PARTICULAR").Visible = True
        If objGridShower.gridView.Columns.Contains("KEYNO") Then objGridShower.gridView.Columns("KEYNO").Visible = False
        If objGridShower.gridView.Columns.Contains("COLHEAD") Then objGridShower.gridView.Columns("COLHEAD").Visible = False
        If objGridShower.gridView.Columns.Contains("RESULT") Then objGridShower.gridView.Columns("RESULT").Visible = False
        If objGridShower.gridView.Columns.Contains("SUBITEMNAME") Then objGridShower.gridView.Columns("SUBITEMNAME").Visible = False
        If objGridShower.gridView.Columns.Contains("GROUPER") Then objGridShower.gridView.Columns("GROUPER").Visible = False
        If objGridShower.gridView.Columns.Contains("SEP") Then objGridShower.gridView.Columns("SEP").Visible = False
        If objGridShower.gridView.Columns.Contains("METAL") Then objGridShower.gridView.Columns("METAL").Visible = False
        If objGridShower.gridView.Columns.Contains("EMPID") Then objGridShower.gridView.Columns("EMPID").Visible = False

        If objGridShower.gridView.Columns.Contains("EMPNAME") Then objGridShower.gridView.Columns("EMPNAME").Visible = False
        If objGridShower.gridView.Columns.Contains("COSTID") Then objGridShower.gridView.Columns("COSTID").Visible = False

        If objGridShower.gridView.Columns.Contains("TRANNO") Then objGridShower.gridView.Columns("TRANNO").Visible = rbtDetailed.Checked
        If objGridShower.gridView.Columns.Contains("TRANDATE") Then objGridShower.gridView.Columns("TRANDATE").Visible = rbtDetailed.Checked
        If objGridShower.gridView.Columns.Contains("TRANDATE") Then objGridShower.gridView.Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
        If objGridShower.gridView.Columns.Contains("SPCS") Then objGridShower.gridView.Columns("SPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        If objGridShower.gridView.Columns.Contains("SGRSWT") Then objGridShower.gridView.Columns("SGRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        If objGridShower.gridView.Columns.Contains("SNETWT") Then objGridShower.gridView.Columns("SNETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        If objGridShower.gridView.Columns.Contains("SDIAWT_C") Then objGridShower.gridView.Columns("SDIAWT_C").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        If objGridShower.gridView.Columns.Contains("SAMOUNT") Then objGridShower.gridView.Columns("SAMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        If objGridShower.gridView.Columns.Contains("RPCS") Then objGridShower.gridView.Columns("RPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        If objGridShower.gridView.Columns.Contains("RGRSWT") Then objGridShower.gridView.Columns("RGRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        If objGridShower.gridView.Columns.Contains("RNETWT") Then objGridShower.gridView.Columns("RNETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        If objGridShower.gridView.Columns.Contains("RDIAWT_C") Then objGridShower.gridView.Columns("RDIAWT_C").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        If objGridShower.gridView.Columns.Contains("RAMOUNT") Then objGridShower.gridView.Columns("RAMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        If objGridShower.gridView.Columns.Contains("TPCS") Then objGridShower.gridView.Columns("TPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        If objGridShower.gridView.Columns.Contains("TGRSWT") Then objGridShower.gridView.Columns("TGRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        If objGridShower.gridView.Columns.Contains("TNETWT") Then objGridShower.gridView.Columns("TNETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        If objGridShower.gridView.Columns.Contains("TDIAWT_C") Then objGridShower.gridView.Columns("TDIAWT_C").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        If objGridShower.gridView.Columns.Contains("TAMOUNT") Then objGridShower.gridView.Columns("TAMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        If objGridShower.gridView.Columns.Contains("COMMISION") Then objGridShower.gridView.Columns("COMMISION").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        'objGridShower.gridView.Columns("SPCS").HeaderText = "SALES_PCS"
        'objGridShower.gridView.Columns("SGRSWT").HeaderText = "SALES_GRSWT"
        'objGridShower.gridView.Columns("SNETWT").HeaderText = "SALES_NETWT"
        'objGridShower.gridView.Columns("SAMOUNT").HeaderText = "SALES_AMOUNT"
        objGridShower.gridView.Columns("SDIAWT_C").HeaderText = "SDIAWT"
        objGridShower.gridView.Columns("RDIAWT_C").HeaderText = "RDIAWT"
        'objGridShower.gridView.Columns("RPCS").HeaderText = "RETURN_PCS"
        'objGridShower.gridView.Columns("RGRSWT").HeaderText = "RETURN_GRSWT"
        'objGridShower.gridView.Columns("RNETWT").HeaderText = "RETURN_NETWT"
        Dim tit As String
        tit = "EMPLOYEE"
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
        StrSql += vbCrLf + " WHERE ISNULL(S.COMMISION,0) <> 0"
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

    Private Sub chkmnthwise_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub btnEmptyComm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
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

    Private Sub GrpContainer_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GrpContainer.Enter

    End Sub
End Class

