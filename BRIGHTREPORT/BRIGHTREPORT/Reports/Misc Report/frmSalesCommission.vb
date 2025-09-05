Imports System.Data.OleDb

Public Class frmSalesCommission
    Dim StrSql As String
    Dim Da As OleDbDataAdapter
    Dim Cmd As OleDbCommand
    Dim objGridShower As frmGridDispDia
    Dim dtCostCentre As New DataTable
    Dim dtEMP, dtmetal, dtsubITEM As New DataTable
    Dim dtITEM As New DataTable
    Dim dtCompany As New DataTable
    Dim itemfilter As String = ""
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
        StrSql += " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'Y')<>'N'"
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

        funcNew()

        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub funcNew()
        StrSql = " SELECT ITEMNAME,CONVERT(vARCHAR,ITEMID),2 RESULT FROM " & cnAdminDb & "..ITEMMAST"
        StrSql += " WHERE ISNULL(ACTIVE,'Y')<>'N'"
        StrSql += " ORDER BY RESULT,ITEMNAME"
        FillCheckedListBox(StrSql, chkLstItem, , chkItemAll.Checked)

        StrSql = " SELECT SUBITEMNAME,CONVERT(vARCHAR,SUBITEMID),2 RESULT FROM " & cnAdminDb & "..SUBITEMMAST"
        StrSql += " WHERE ISNULL(ACTIVE,'Y')<>'N'"
        StrSql += " ORDER BY RESULT,SUBITEMNAME"
        FillCheckedListBox(StrSql, chkLstSubItem, , chkSubItemAll.Checked)

        StrSql = " SELECT EMPNAME,CONVERT(vARCHAR,EMPID),2 RESULT FROM " & cnAdminDb & "..EMPMASTER"
        StrSql += " WHERE ISNULL(ACTIVE,'Y')<>'N'"
        StrSql += " ORDER BY RESULT,EMPNAME"
        FillCheckedListBox(StrSql, chkLstEmployee, chkEmpAll.Checked)
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        ' chkWithItem.Checked = False
        rbtGrpEmployee.Checked = True
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , "ALL")
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        BrighttechPack.GlobalMethods.FillCombo(chkcmbmetal, dtmetal, "METALNAME", , "ALL")
        funcNew()
        Prop_Gets()
        dtpFrom.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Prop_Sets()
        'Sysid = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
        Sysid = systemId + userId.ToString
        itemfilter = ""
        Dim chkMetalid As String = GetSelectedMetalid(chkcmbmetal, True)
        Dim chkCompId As String = GetQryStringForSp(chkCmbCompany.Text, cnAdminDb & "..COMPANY", "COMPANYID", "COMPANYNAME", True)
        Dim chkCostId As String = GetQryStringForSp(chkCmbCostCentre.Text, cnAdminDb & "..COSTCENTRE", "COSTID", "COSTNAME", True)
        Dim chkempid As String = GetChecked_CheckedList(chkLstEmployee)
        Dim chkitemid As String = GetChecked_CheckedList(chkLstItem)
        Dim chksubitemid As String = GetChecked_CheckedList(chkLstSubItem)
        If chkLstSubItem.CheckedItems.Count = chkLstSubItem.Items.Count Then
            chksubitemid = "ALL"
        End If
        If chkLstEmployee.Items.Count = chkLstEmployee.CheckedItems.Count Then
            chkempid = "ALL"
        End If
        'Dim chkempid As String = GetQryStringForSp(chkcmbemployee.Text, cnAdminDb & "..EMPMASTER", "EMPID", "EMPNAME", True)
        'Dim chkitemid As String = GetQryStringForSp(chkcmbitemname.Text, cnAdminDb & "..ITEMMAST", "ITEMID", "ITEMNAME", True)
        'Dim chksubitemid As String = GetQryStringForSp(chkcmbsubitem.Text, cnAdminDb & "..SUBITEMMAST", "SUBITEMID", "SUBITEMNAME", True)

        'StrSql = "SELECT DISTINCT ITEMID FROM " & cnStockDb & "..ISSUE I"
        'StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND I.TRANTYPE = 'SA' AND ISNULL(I.CANCEL,'') = ''"
        'If chkempid <> "ALL" And chkempid <> "" Then
        '    StrSql += vbCrLf + " AND EMPID IN(" & chkempid & ")"
        'End If
        'If chkCostId <> "ALL" And chkCostId <> "" Then
        '    StrSql += vbCrLf + " AND COSTID IN(" & chkCostId & ")"
        'End If
        'If chkitemid <> "ALL" And chkitemid <> "" Then
        '    StrSql += vbCrLf + " AND ITEMID IN(" & chkitemid & ")"
        'End If
        'StrSql += vbCrLf + " AND ITEMID NOT IN(SELECT ITEMID FROM " & cnAdminDb & "..SALES_COMMISION UNION SELECT 0)"
        'Dim da As New OleDbDataAdapter
        'da = New OleDbDataAdapter(StrSql, cn)
        'Dim dt As New DataTable
        'da.Fill(dt)
        'If dt.Rows.Count > 0 Then
        '    Dim Itemid As String = ""
        '    For i As Integer = 0 To dt.Rows.Count - 1
        '        If i = dt.Rows.Count - 1 Then
        '            Itemid += dt.Rows(i).Item("ITEMID").ToString
        '        Else
        '            Itemid += dt.Rows(i).Item("ITEMID").ToString + ","
        '        End If
        '    Next
        '    MsgBox("Commission not Set for Items" & vbCrLf & Itemid, MsgBoxStyle.Information) : Exit Sub
        'End If
        'StrSql = "SELECT DISTINCT  SUBITEMID FROM " & cnStockDb & "..ISSUE I"
        'StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND I.TRANTYPE = 'SA' AND ISNULL(I.CANCEL,'') = ''"
        'If chkempid <> "ALL" And chkempid <> "" Then
        '    StrSql += vbCrLf + " AND EMPID IN(" & chkempid & ")"
        'End If
        'If chkCostId <> "ALL" And chkCostId <> "" Then
        '    StrSql += vbCrLf + " AND COSTID IN(" & chkCostId & ")"
        'End If
        'If chksubitemid <> "ALL" And chksubitemid <> "" Then
        '    StrSql += vbCrLf + " AND SUBITEMID IN(" & chkempid & ")"
        'End If
        'StrSql += vbCrLf + " AND SUBITEMID NOT IN(SELECT SUBITEMID FROM " & cnAdminDb & "..SALES_COMMISION UNION SELECT 0)"
        'dt = New DataTable
        'da.Fill(dt)
        'If dt.Rows.Count > 0 Then
        '    Dim Itemid As String = ""
        '    For i As Integer = 0 To dt.Rows.Count - 1
        '        If i = dt.Rows.Count - 1 Then
        '            Itemid += dt.Rows(i).Item("SUBITEMID").ToString
        '        Else
        '            Itemid += dt.Rows(i).Item("SUBITEMID").ToString + ","
        '        End If
        '    Next
        '    MsgBox("Commission not Set for Subitems" & vbCrLf & Itemid, MsgBoxStyle.Information) : Exit Sub
        'End If

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
        If chkempid <> "" And chkempid <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(I.EMPID,'')IN(SELECT EMPID FROM " & cnAdminDb & "..EMPMASTER WHERE EMPNAME IN(" & chkempid & ")) "
        If chkitemid <> "" And chkitemid <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(I.ITEMID,'')IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN(" & chkitemid & ")) "
        If chksubitemid <> "" And chksubitemid <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(I.SUBITEMID,'')IN(SELECT 0 UNION ALL SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME IN(" & chksubitemid & ")) "
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
        If chkempid <> "" And chkempid <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(I.EMPID,'')IN(SELECT EMPID FROM " & cnAdminDb & "..EMPMASTER WHERE EMPNAME IN(" & chkempid & ")) "
        If chkitemid <> "" And chkitemid <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(ISS.STNITEMID,'')IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN(" & chkitemid & ")) "
        If chksubitemid <> "" And chksubitemid <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(ISS.STNSUBITEMID,'')IN(SELECT 0 UNION ALL SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME IN(" & chksubitemid & ")) "
        If chkCostId <> "" And chkCostId <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(ISS.COSTID,'') IN (" & chkCostId & ")"
        If chkCompId <> "" And chkCompId <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(ISS.COMPANYID,'') IN (" & chkCompId & ")"
        If itemfilter <> "" Then StrSql += " AND ISS.STNITEMID NOT IN(" & itemfilter & ")"
        StrSql += vbCrLf + " )X "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        StrSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & ""

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
        If chkempid <> "" And chkempid <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(I.EMPID,'')IN(SELECT EMPID FROM " & cnAdminDb & "..EMPMASTER WHERE EMPNAME IN(" & chkempid & ")) "
        If chkitemid <> "" And chkitemid <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(I.ITEMID,'')IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN(" & chkitemid & ")) "
        If chksubitemid <> "" And chksubitemid <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(I.SUBITEMID,'')IN(SELECT 0 UNION ALL SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME IN(" & chksubitemid & ")) "
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
        If chkempid <> "" And chkempid <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(I.EMPID,'')IN(SELECT EMPID FROM " & cnAdminDb & "..EMPMASTER WHERE EMPNAME IN(" & chkempid & ")) "
        If chkitemid <> "" And chkitemid <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(IR.STNITEMID,'')IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN(" & chkitemid & ")) "
        If chksubitemid <> "" And chksubitemid <> "ALL" Then StrSql += vbCrLf + " AND ISNULL(IR.STNSUBITEMID,'')IN(SELECT 0 UNION ALL SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME IN(" & chksubitemid & ")) "
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
            'If chksubitemid <> "" And chksubitemid <> "ALL" Then StrSql += vbCrLf + " AND SC.SUBITEMID = TS.SUBITEMID "
            StrSql += vbCrLf + " AND (CASE WHEN TS.CALTYPE IN ('R','F') THEN TS.AMOUNT ELSE TS.NETWT END) BETWEEN SC.FROM_VAL AND SC.TO_VAL"
            StrSql += vbCrLf + " WHERE ISNULL(TS.UPDATED,'') = ''"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        End If


        StrSql = vbCrLf + " UPDATE TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " SET COMM_FLAT = (SC.COMMISION*ISNULL(TS.PCS,1))"
        'StrSql += vbCrLf + " CASE WHEN SC.BASEDON IN('V') THEN  (SC.COMMISION*ISNULL(TS.PCS,1))ELSE SC.COMMISION END"
        StrSql += vbCrLf + ",COMM_GRM = SC.COMMISIONGRM,COMM_PER = SC.COMMISIONPER,UPDATED = 'Y'"
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " AS TS"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..SALES_COMMISION  AS SC ON (SC.ITEMCTRID = TS.ITEMCTRID OR ISNULL(SC.ITEMCTRID,0) = 0) AND SC.ITEMID = TS.ITEMID --AND SC.SUBITEMID = TS.SUBITEMID "
        StrSql += vbCrLf + " AND (CASE WHEN TS.CALTYPE IN ('R','F') THEN TS.AMOUNT ELSE TS.NETWT END) BETWEEN SC.FROM_VAL AND SC.TO_VAL"
        StrSql += vbCrLf + " WHERE ISNULL(TS.UPDATED,'') = ''"

        StrSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " SET COMM_FLAT = (SC.COMMISION*ISNULL(TS.PCS,1))"
        'StrSql += vbCrLf + " CASE WHEN SC.BASEDON IN('V') THEN  (SC.COMMISION*ISNULL(TS.PCS,1))ELSE SC.COMMISION END"
        StrSql += vbCrLf + ",COMM_GRM = SC.COMMISIONGRM,COMM_PER = SC.COMMISIONPER,UPDATED = 'Y'"
        StrSql += vbCrLf + " FROM TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " AS TS"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..SALES_COMMISION  AS SC ON (SC.ITEMCTRID = TS.ITEMCTRID OR ISNULL(SC.ITEMCTRID,0) = 0) AND SC.ITEMID = TS.ITEMID --AND SC.SUBITEMID = 0 "
        StrSql += vbCrLf + " AND (CASE WHEN TS.CALTYPE IN ('R','F') THEN TS.AMOUNT ELSE TS.NETWT END) BETWEEN SC.FROM_VAL AND SC.TO_VAL"
        StrSql += vbCrLf + " WHERE ISNULL(TS.UPDATED,'') = ''"

        StrSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " SET COMMISION =  "
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
        If rbtSummary.Checked Then
            If insper = 0 Then MsgBox("Incentive percentage should not be Zero." & vbCrLf & "Pls update softcontrol Id is INCENTIVEPERCENTAGE")
            GroupingGridbymonthwise()
        ElseIf rbtDetailed.Checked Then
            GroupingGridbydefault()
        ElseIf rbtTarget.Checked Then
            funcTargetVsSales()
        End If
    End Sub
    Private Sub funcTargetVsSales()
        StrSql = "IF OBJECT_ID('TEMPTABLEDB..TEMPTTT" & Sysid & "','U')  IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPTTT" & Sysid & ""
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.ExecuteNonQuery()
        StrSql = "IF OBJECT_ID('TEMPTABLEDB..TEMPTTTTT" & Sysid & "','U')  IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPTTTTT" & Sysid & ""
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.ExecuteNonQuery()
        StrSql += vbCrLf + "SELECT "
        StrSql += vbCrLf + " CONVERT(NUMERIC(20,3),SUM(SGRSWT))SALESWT"
        StrSql += vbCrLf + " ,METAL,SUM(EI.WEIGHT)TARGET"
        StrSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID=X.COSTID)COSTNAME"
        StrSql += vbCrLf + " ,CONVERT(VARCHAR(3),SUBSTRING(DATENAME(MONTH,MNTH),0,4)) TMONTH, MNTHID"
        StrSql += vbCrLf + " INTO TEMPTABLEDB..TEMPTTT" & Sysid & ""
        StrSql += vbCrLf + " FROM ("
        StrSql += vbCrLf + " SELECT "
        StrSql += vbCrLf + " SUM(CASE WHEN SEP = 'SA' THEN GRSWT ELSE -1*GRSWT END) AS SGRSWT"
        StrSql += vbCrLf + " ,METAL,COSTID"
        StrSql += vbCrLf + " ,CONVERT(VARCHAR(15),'2013-'+ SUBSTRING(DATENAME(MONTH,TRANDATE),0,4)+'-01')MNTH"
        StrSql += vbCrLf + " ,DATEPART(mm,TRANDATE)  MNTHID"
        StrSql += vbCrLf + "FROM TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " S GROUP BY METAL,COSTID "
        StrSql += vbCrLf + ",CONVERT(VARCHAR(15),'2013-'+ SUBSTRING(DATENAME(MONTH,TRANDATE),0,4)+'-01'), TRANDATE"
        StrSql += vbCrLf + ")X "
        StrSql += vbCrLf + "LEFT OUTER JOIN " & cnAdminDb & "..EMPWISEINCENTIVE AS EI ON EI.COSTID=X.COSTID "
        StrSql += vbCrLf + "AND EI.METALID=(SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME=X.METAL) "
        StrSql += vbCrLf + "AND EI.MONTH=DATENAME(MONTH,X.MNTH)"
        StrSql += vbCrLf + "GROUP BY X.COSTID,METAL, MNTH, MNTHID"
        StrSql += vbCrLf + "ORDER BY COSTNAME"
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.ExecuteNonQuery()

        StrSql = vbCrLf + "DECLARE @QRY VARCHAR(1000)"
        StrSql += vbCrLf + "DECLARE @MONTH VARCHAR(50)"
        StrSql += vbCrLf + "DECLARE @MONTHID VARCHAR(50)"
        StrSql += vbCrLf + "DECLARE CUR CURSOR FOR"
        'StrSql += vbCrLf + "SELECT DISTINCT METAL  FROM TEMPTABLEDB..TEMPTTT" & Sysid & ""
        StrSql += vbCrLf + "SELECT DISTINCT TMONTH, MNTHID FROM TEMPTABLEDB..TEMPTTT" & Sysid & " ORDER BY MNTHID"
        StrSql += vbCrLf + "OPEN CUR"
        StrSql += vbCrLf + "FETCH NEXT FROM CUR INTO @MONTH , @MONTHID"
        StrSql += vbCrLf + "SELECT @QRY='CREATE TABLE TEMPTABLEDB..TEMPTTTTT" & Sysid & "(PARTICULAR VARCHAR(100),BRANCH VARCHAR(100),COLHEAD VARCHAR(2), METAL VARCHAR(30), RESULT INT'"
        StrSql += vbCrLf + "WHILE @@FETCH_STATUS<>-1"
        StrSql += vbCrLf + "BEGIN"
        StrSql += vbCrLf + "	SELECT @QRY=@QRY+CHAR(13)+ ','+@MONTH +'_TARGET NUMERIC(20,3)'"
        StrSql += vbCrLf + "	SELECT @QRY=@QRY+CHAR(13)+ ','+@MONTH +'_SALES NUMERIC(20,3)'"
        'StrSql += vbCrLf + "	SELECT @QRY=@QRY+CHAR(13)+ ','+@METAL +'_DIFF NUMERIC(20,3)'"
        StrSql += vbCrLf + "	FETCH NEXT FROM CUR INTO @MONTH , @MONTHID"
        StrSql += vbCrLf + "END"
        StrSql += vbCrLf + "SELECT @QRY=@QRY+CHAR(13)+ ')'"
        StrSql += vbCrLf + "PRINT(@QRY)"
        StrSql += vbCrLf + "EXEC(@QRY)"
        StrSql += vbCrLf + "CLOSE CUR"
        StrSql += vbCrLf + "DEALLOCATE CUR"
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.ExecuteNonQuery()
        StrSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMPTTTTT" & Sysid & "(PARTICULAR,BRANCH, METAL, RESULT, COLHEAD)"
        StrSql += vbCrLf + " SELECT DISTINCT COSTNAME,COSTNAME, 'ZZ', 0, 'T' FROM TEMPTABLEDB..TEMPTTT" & Sysid & ""
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.ExecuteNonQuery()

        StrSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPTTTTT" & Sysid & "(PARTICULAR, METAL, BRANCH, RESULT, COLHEAD)"
        StrSql += vbCrLf + " SELECT METAL, METAL, COSTNAME,  1, 'D'  FROM TEMPTABLEDB..TEMPTTT00853"
        StrSql += vbCrLf + " GROUP BY METAL, COSTNAME"
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.ExecuteNonQuery()

        'StrSql = vbCrLf + "DECLARE @QRY VARCHAR(1000)"
        'StrSql += vbCrLf + "DECLARE @COSTNAME VARCHAR(50)"
        'StrSql += vbCrLf + "DECLARE @METAL VARCHAR(50)"
        'StrSql += vbCrLf + "DECLARE CUR CURSOR FOR"
        'StrSql += vbCrLf + "SELECT DISTINCT COSTNAME,METAL  FROM TEMPTABLEDB..TEMPTTT" & Sysid & ""
        'StrSql += vbCrLf + "OPEN CUR"
        'StrSql += vbCrLf + "FETCH NEXT FROM CUR INTO @COSTNAME,@METAL"
        'StrSql += vbCrLf + "WHILE @@FETCH_STATUS<>-1"
        'StrSql += vbCrLf + "BEGIN"
        'StrSql += vbCrLf + "	SELECT @QRY=CHAR(13)+ 'UPDATE TEMPTABLEDB..TEMPTTTTT" & Sysid & " SET '+@METAL+'_TARGET='"
        'StrSql += vbCrLf + "	SELECT @QRY=@QRY+CHAR(13)+ '(SELECT TARGET FROM TEMPTABLEDB..TEMPTTT" & Sysid & " WHERE COSTNAME='''+@COSTNAME+''' AND METAL='''+@METAL+''')'"
        'StrSql += vbCrLf + "	SELECT @QRY=@QRY+CHAR(13)+ 'WHERE BRANCH='''+@COSTNAME+'''' "
        'StrSql += vbCrLf + "	PRINT(@QRY)"
        'StrSql += vbCrLf + "	EXEC(@QRY)"
        'StrSql += vbCrLf + "	SELECT @QRY=CHAR(13)+ 'UPDATE TEMPTABLEDB..TEMPTTTTT" & Sysid & " SET '+@METAL+'_SALES='"
        'StrSql += vbCrLf + "	SELECT @QRY=@QRY+CHAR(13)+ '(SELECT SALESWT FROM TEMPTABLEDB..TEMPTTT" & Sysid & " WHERE COSTNAME='''+@COSTNAME+''' AND METAL='''+@METAL+''')'"
        'StrSql += vbCrLf + "	SELECT @QRY=@QRY+CHAR(13)+ 'WHERE BRANCH='''+@COSTNAME+''''"
        'StrSql += vbCrLf + "	PRINT(@QRY)"
        'StrSql += vbCrLf + "	EXEC(@QRY)"
        'StrSql += vbCrLf + "	FETCH NEXT FROM CUR INTO @COSTNAME,@METAL"
        'StrSql += vbCrLf + "END"
        'StrSql += vbCrLf + "CLOSE CUR"
        'StrSql += vbCrLf + "DEALLOCATE CUR"
        'Cmd = New OleDbCommand(StrSql, cn)
        'Cmd.ExecuteNonQuery()

        StrSql = " DECLARE @QRY VARCHAR(8000)"
        StrSql += vbCrLf + " DECLARE @COSTNAME VARCHAR(50)"

        StrSql += vbCrLf + " DECLARE CURCOSTNAME CURSOR FOR"
        StrSql += vbCrLf + " SELECT DISTINCT COSTNAME  FROM TEMPTABLEDB..TEMPTTT00853 "
        StrSql += vbCrLf + " OPEN CURCOSTNAME"
        StrSql += vbCrLf + " FETCH NEXT FROM CURCOSTNAME INTO @COSTNAME"
        StrSql += vbCrLf + " WHILE @@FETCH_STATUS<>-1"
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " PRINT 	@COSTNAME"
        StrSql += vbCrLf + " /*CURSOR FOR FETCH MONTH*/		"
        StrSql += vbCrLf + " DECLARE @MONTH VARCHAR(10)	"
        StrSql += vbCrLf + " DECLARE CURMONTH CURSOR FOR"
        StrSql += vbCrLf + " SELECT DISTINCT TMONTH  FROM TEMPTABLEDB..TEMPTTT00853 WHERE COSTNAME = @COSTNAME		"
        StrSql += vbCrLf + " OPEN CURMONTH "
        StrSql += vbCrLf + " FETCH NEXT FROM CURMONTH INTO @MONTH"
        StrSql += vbCrLf + " WHILE @@FETCH_STATUS <> -1"
        StrSql += vbCrLf + " BEGIN "

        StrSql += vbCrLf + " /*CURSOR FOR FETCH METAL*/		"
        StrSql += vbCrLf + " DECLARE @METAL VARCHAR(50)"
        StrSql += vbCrLf + " DECLARE CURMETAL CURSOR FOR"
        StrSql += vbCrLf + " SELECT DISTINCT METAL  FROM TEMPTABLEDB..TEMPTTT00853 WHERE TMONTH = @MONTH AND METAL NOT LIKE 'ZZZ%'"
        StrSql += vbCrLf + " OPEN CURMETAL "
        StrSql += vbCrLf + " FETCH NEXT FROM CURMETAL INTO @METAL				"
        StrSql += vbCrLf + " WHILE @@FETCH_STATUS <> -1"
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " SELECT @QRY= 'UPDATE TEMPTABLEDB..TEMPTTTTT00853 SET '+@MONTH+'_TARGET='		"
        StrSql += vbCrLf + " SELECT @QRY=@QRY+CHAR(13)+ ' (SELECT TARGET FROM TEMPTABLEDB..TEMPTTT00853 WHERE COSTNAME='''+@COSTNAME+''' AND METAL='''+@METAL+''' AND TMONTH = '''+@MONTH+''')'		"
        StrSql += vbCrLf + " SELECT @QRY=@QRY+CHAR(13)+ ' WHERE BRANCH='''+@COSTNAME+''' AND METAL = '''+@METAL+''' AND METAL NOT LIKE ''ZZZ'' ' 		"
        StrSql += vbCrLf + " PRINT @QRY"
        StrSql += vbCrLf + " EXEC(@QRY)	"

        StrSql += vbCrLf + " SELECT @QRY=' UPDATE TEMPTABLEDB..TEMPTTTTT00853 SET '+@MONTH+'_SALES='"
        StrSql += vbCrLf + " SELECT @QRY=@QRY+CHAR(13)+ '(SELECT SALESWT FROM TEMPTABLEDB..TEMPTTT00853 WHERE COSTNAME='''+@COSTNAME+''' AND METAL='''+@METAL+''' AND TMONTH = '''+@MONTH+''')'"
        StrSql += vbCrLf + " SELECT @QRY=@QRY+CHAR(13)+ 'WHERE BRANCH='''+@COSTNAME+''' AND METAL = '''+@METAL+''' AND METAL NOT LIKE ''ZZZ'' '"
        StrSql += vbCrLf + " PRINT @QRY"
        StrSql += vbCrLf + " EXEC(@QRY)	"
        StrSql += vbCrLf + " FETCH NEXT FROM CURMETAL INTO @METAL"
        StrSql += vbCrLf + " End"
        StrSql += vbCrLf + " Close  CURMETAL "
        StrSql += vbCrLf + " DEALLOCATE CURMETAL "

        StrSql += vbCrLf + " FETCH NEXT FROM CURMONTH INTO @MONTH"
        StrSql += vbCrLf + " End"
        StrSql += vbCrLf + " Close CURMONTH "
        StrSql += vbCrLf + " DEALLOCATE CURMONTH "

        StrSql += vbCrLf + " FETCH NEXT FROM CURCOSTNAME INTO @COSTNAME"
        StrSql += vbCrLf + " End"
        StrSql += vbCrLf + " Close CURCOSTNAME "
        StrSql += vbCrLf + " DEALLOCATE CURCOSTNAME "

        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.ExecuteNonQuery()

        StrSql = vbCrLf + "ALTER TABLE TEMPTABLEDB..TEMPTTTTT" & Sysid & " ADD DISPORDER INT"
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.ExecuteNonQuery()
        StrSql = vbCrLf + "UPDATE TT SET DISPORDER=CC.DISPORDER FROM TEMPTABLEDB..TEMPTTTTT" & Sysid & " TT ," & cnAdminDb & "..COSTCENTRE CC WHERE CC.COSTNAME=TT.BRANCH   "
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.ExecuteNonQuery()
        StrSql = vbCrLf + "DECLARE @QRY VARCHAR(1000)"
        StrSql += vbCrLf + "DECLARE @QRY1 VARCHAR(4000)"
        StrSql += vbCrLf + "DECLARE @QRY2 VARCHAR(4000)"
        StrSql += vbCrLf + "DECLARE @QRY3 VARCHAR(4000)"
        StrSql += vbCrLf + "DECLARE @QRY4 VARCHAR(4000)"
        StrSql += vbCrLf + "DECLARE @MONTH VARCHAR(50)"
        StrSql += vbCrLf + "DECLARE CUR CURSOR FOR"
        StrSql += vbCrLf + "SELECT DISTINCT TMONTH  FROM TEMPTABLEDB..TEMPTTT" & Sysid & ""
        StrSql += vbCrLf + "OPEN CUR"
        StrSql += vbCrLf + "FETCH NEXT FROM CUR INTO @MONTH"
        StrSql += vbCrLf + "SELECT @QRY1='INSERT INTO TEMPTABLEDB..TEMPTTTTT" & Sysid & "(PARTICULAR,BRANCH,DISPORDER,COLHEAD'"
        StrSql += vbCrLf + "SELECT @QRY2=''"
        StrSql += vbCrLf + "SELECT @QRY3=''"
        StrSql += vbCrLf + "SELECT @QRY4='INSERT INTO TEMPTABLEDB..TEMPTTTTT" & Sysid & "(PARTICULAR,BRANCH,METAL,RESULT, DISPORDER,COLHEAD'"
        StrSql += vbCrLf + "WHILE @@FETCH_STATUS<>-1"
        StrSql += vbCrLf + "BEGIN"
        'StrSql += vbCrLf + "	SELECT @QRY=CHAR(13)+ 'UPDATE TEMPTABLEDB..TEMPTTTTT" & Sysid & " SET '+@MONTH +'_DIFF=ISNULL('+@MONTH +'_TARGET,0)-ISNULL('+@MONTH +'_SALES,0) '"
        'StrSql += vbCrLf + "	EXEC(@QRY)"
        'StrSql += vbCrLf + "	SELECT @QRY=CHAR(13)+ 'UPDATE TEMPTABLEDB..TEMPTTTTT" & Sysid & " SET '+@MONTH +'_DIFF=NULL WHERE ISNULL('+@MONTH +'_TARGET,0)=0'"
        'StrSql += vbCrLf + "	EXEC(@QRY)"
        StrSql += vbCrLf + "	SELECT @QRY=CHAR(13)+ 'UPDATE TEMPTABLEDB..TEMPTTTTT" & Sysid & " SET '+@MONTH +'_TARGET=NULL WHERE ISNULL('+@MONTH +'_TARGET,0)=0'"
        StrSql += vbCrLf + "	EXEC(@QRY)"
        StrSql += vbCrLf + "	SELECT @QRY=CHAR(13)+ 'UPDATE TEMPTABLEDB..TEMPTTTTT" & Sysid & " SET '+@MONTH +'_SALES=NULL WHERE ISNULL('+@MONTH +'_SALES,0)=0'"
        StrSql += vbCrLf + "	EXEC(@QRY)"
        'StrSql += vbCrLf + "	SELECT @QRY2=@QRY2+','+@MONTH+'_TARGET,'+@MONTH+'_SALES,'+@MONTH+'_DIFF'"
        StrSql += vbCrLf + "	SELECT @QRY2=@QRY2+','+@MONTH+'_TARGET,'+@MONTH+'_SALES'"

        'StrSql += vbCrLf + "	SELECT @QRY3=@QRY3+',SUM(ISNULL('+@MONTH+'_TARGET,0)),SUM(ISNULL('+@MONTH+'_SALES,0)),SUM(ISNULL('+@MONTH+'_DIFF,0))'"
        StrSql += vbCrLf + "	SELECT @QRY3=@QRY3+',SUM(ISNULL('+@MONTH+'_TARGET,0)),SUM(ISNULL('+@MONTH+'_SALES,0))'"
        StrSql += vbCrLf + "	FETCH NEXT FROM CUR INTO @MONTH"
        StrSql += vbCrLf + "END"
        StrSql += vbCrLf + "CLOSE CUR"
        StrSql += vbCrLf + "DEALLOCATE CUR"
        StrSql += vbCrLf + "SELECT @QRY1=@QRY1+@QRY2+')'"
        StrSql += vbCrLf + "SELECT @QRY1=@QRY1+' SELECT ''GRAND TOTAL'',''ZZZZ'',999,''G'''"
        StrSql += vbCrLf + "SELECT @QRY1=@QRY1+' '+@QRY3"
        StrSql += vbCrLf + "SELECT @QRY1=@QRY1+' FROM TEMPTABLEDB..TEMPTTTTT" & Sysid & "'"
        StrSql += vbCrLf + "PRINT (@QRY1)"
        StrSql += vbCrLf + "EXEC(@QRY1)"

        StrSql += vbCrLf + " SELECT @QRY4=@QRY4+ @QRY2+')'"
        StrSql += vbCrLf + " SELECT @QRY4=@QRY4+' SELECT ''SUB TOTAL'', BRANCH, ''ZZZZ'', 2, 99,''S1'''"
        StrSql += vbCrLf + " SELECT @QRY4=@QRY4+' '+@QRY3"
        StrSql += vbCrLf + " SELECT @QRY4=@QRY4+' FROM TEMPTABLEDB..TEMPTTTTT00853 WHERE BRANCH NOT LIKE ''Z%'' GROUP BY BRANCH'"
        StrSql += vbCrLf + " PRINT @QRY4"
        StrSql += vbCrLf + " EXEC (@QRY4)"

        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.ExecuteNonQuery()

        'StrSql = vbCrLf + "SELECT * FROM TEMPTABLEDB..TEMPTTTTT" & Sysid & " ORDER BY DISPORDER"
        StrSql = vbCrLf + "SELECT * FROM TEMPTABLEDB..TEMPTTTTT" & Sysid & " "
        StrSql += vbCrLf + "ORDER BY BRANCH, RESULT, PARTICULAR, METAL"
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
        objGridShower.gridView.Columns("BRANCH").Visible = False
        objGridShower.gridView.Columns("KEYNO").Visible = False
        objGridShower.gridView.Columns("DISPORDER").Visible = False
        objGridShower.gridView.Columns("COLHEAD").Visible = False
        objGridShower.gridView.Columns("RESULT").Visible = False
        objGridShower.gridView.Columns("METAL").Visible = False
        FormatGridColumns(objGridShower.gridView, False, False, True, False)
        objGridShower.Text = "TARGET VS SALES  "
        Dim tit As String = "TARGET VS SALES" + vbCrLf
        tit += " DATE FROM " & dtpFrom.Text + " TO " + dtpTo.Text
        tit += IIf(chkCmbCostCentre.Text <> "" And chkCmbCostCentre.Text <> "ALL", " :" & chkCmbCostCentre.Text, "")
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridViewHeader.Visible = True
        objGridShower.Show()
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView, "PARTICULAR")
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        funcGridHeaderNew(objGridShower.gridView, objGridShower.gridViewHeader)
        objGridShower.ResizeToolStripMenuItem.Checked = True
        objGridShower.ResizeToolStripMenuItem_Click(Me, New EventArgs)
        StrSql = vbCrLf + "SELECT DISTINCT TMONTH AS MONTH FROM TEMPTABLEDB..TEMPTTT" & Sysid & " "
        Dim dtMetal As New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtMetal)
        funcGridHeaderStyle(objGridShower.gridView, objGridShower.gridViewHeader, dtMetal)
    End Sub
    Function funcGridHeaderNew(ByVal Grid As DataGridView, ByVal GridHead As DataGridView) As Integer
        Try
            StrSql = vbCrLf + "SELECT DISTINCT TMONTH MONTH, MNTHID FROM TEMPTABLEDB..TEMPTTT" & Sysid & " ORDER BY MNTHID"
            Dim dtMetal As New DataTable
            Da = New OleDbDataAdapter(StrSql, cn)
            Da.Fill(dtMetal)
            Dim dtMergeHeader As New DataTable("~MERGEHEADER")
            With dtMergeHeader
                .Columns.Add("PARTICULAR", GetType(String))
                For i As Integer = 0 To dtMetal.Rows.Count - 1
                    '.Columns.Add(dtMetal.Rows(i).Item("MONTH") & "_TARGET~" & dtMetal.Rows(i).Item("MONTH") & "_SALES~" & dtMetal.Rows(i).Item("MONTH") & "_DIFF", GetType(Decimal))
                    .Columns.Add(dtMetal.Rows(i).Item("MONTH") & "_TARGET~" & dtMetal.Rows(i).Item("MONTH") & "_SALES~", GetType(Decimal))
                Next
                .Columns.Add("SCROLL", GetType(String))
                .Columns("PARTICULAR").Caption = ""
                .Columns("SCROLL").Caption = ""
            End With
            GridHead.DataSource = dtMergeHeader
            funcGridHeaderStyle(Grid, GridHead, dtMetal)
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Function
    Function funcGridHeaderStyle(ByVal gridView As DataGridView, ByVal gridViewHead As DataGridView, ByVal dtMetal As DataTable) As Integer
        With gridViewHead
            .Columns("SCROLL").HeaderText = ""
            With .Columns("PARTICULAR")
                .Visible = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Width = gridView.Columns("PARTICULAR").Width
                .HeaderText = " "
            End With
            For i As Integer = 0 To dtMetal.Rows.Count - 1
                'With .Columns(dtMetal.Rows(i).Item("MONTH") & "_TARGET~" & dtMetal.Rows(i).Item("MONTH") & "_SALES~" & dtMetal.Rows(i).Item("MONTH") & "_DIFF")
                With .Columns(dtMetal.Rows(i).Item("MONTH") & "_TARGET~" & dtMetal.Rows(i).Item("MONTH") & "_SALES~")
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    '.Width = gridView.Columns(dtMetal.Rows(i).Item("MONTH") & "_TARGET").Width + gridView.Columns(dtMetal.Rows(i).Item("MONTH") & "_SALES").Width + gridView.Columns(dtMetal.Rows(i).Item("MONTH") & "_DIFF").Width
                    .Width = gridView.Columns(dtMetal.Rows(i).Item("MONTH") & "_TARGET").Width + gridView.Columns(dtMetal.Rows(i).Item("MONTH") & "_SALES").Width
                    .HeaderText = dtMetal.Rows(i).Item("MONTH")
                    gridView.Columns(dtMetal.Rows(i).Item("MONTH") & "_TARGET").HeaderText = "TARGET"
                    If dtMetal.Rows(i).Item("MONTH").ToString = "DIAMOND" Then
                        gridView.Columns(dtMetal.Rows(i).Item("MONTH") & "_TARGET").DefaultCellStyle.BackColor = Color.Beige
                        gridView.Columns(dtMetal.Rows(i).Item("MONTH") & "_SALES").DefaultCellStyle.BackColor = Color.Beige
                        'gridView.Columns(dtMetal.Rows(i).Item("MONTH") & "_DIFF").DefaultCellStyle.BackColor = Color.Beige
                    ElseIf dtMetal.Rows(i).Item("MONTH").ToString = "SILVER" Then
                        gridView.Columns(dtMetal.Rows(i).Item("MONTH") & "_TARGET").DefaultCellStyle.BackColor = Color.Lavender
                        gridView.Columns(dtMetal.Rows(i).Item("MONTH") & "_SALES").DefaultCellStyle.BackColor = Color.Lavender
                        '.Columns(dtMetal.Rows(i).Item("MONTH") & "_DIFF").DefaultCellStyle.BackColor = Color.Lavender
                    ElseIf dtMetal.Rows(i).Item("MONTH").ToString = "GOLD" Then
                        gridView.Columns(dtMetal.Rows(i).Item("MONTH") & "_TARGET").DefaultCellStyle.BackColor = Color.LavenderBlush
                        gridView.Columns(dtMetal.Rows(i).Item("MONTH") & "_SALES").DefaultCellStyle.BackColor = Color.LavenderBlush
                        'gridView.Columns(dtMetal.Rows(i).Item("MONTH") & "_DIFF").DefaultCellStyle.BackColor = Color.LavenderBlush
                    End If
                End With
                gridView.Columns(dtMetal.Rows(i).Item("MONTH") & "_SALES").HeaderText = "SALES"
                'gridView.Columns(dtMetal.Rows(i).Item("MONTH") & "_DIFF").HeaderText = "DIFF"
            Next
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        End With
    End Function

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

        StrSql = "IF OBJECT_ID('TEMPTABLEDB..TEMPSALESCOMM" & Sysid & "','U')  IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPSALESCOMM" & Sysid & ""
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.ExecuteNonQuery()
        StrSql = vbCrLf + "SELECT PARTICULAR"
        StrSql += vbCrLf + ",SUM(SALESCOMM)SALESCOMM,SUM(OTHERCOMM)OTHERCOMM"
        StrSql += vbCrLf + ",SUM((CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(SPCS,0) ELSE 0 END))SPCS"
        StrSql += vbCrLf + ",CONVERT(NUMERIC(20,3),SUM(SGRSWT))SGRSWT,CONVERT(NUMERIC(20,3),SUM(SNETWT))SNETWT"
        StrSql += vbCrLf + ",SUM((CASE WHEN ISNULL(METAL,'') NOT IN('OTHERS')THEN ISNULL(SAMOUNT,0)ELSE 0 END))SAMOUNT"
        StrSql += vbCrLf + ",SUM((CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM')THEN ISNULL(RPCS,0) ELSE 0 END))RPCS"
        StrSql += vbCrLf + ",SUM(RGRSWT)RGRSWT"
        StrSql += vbCrLf + ",CONVERT(NUMERIC(20,3),SUM(RNETWT))RNETWT"
        StrSql += vbCrLf + ",SUM((CASE WHEN ISNULL(METAL,'') NOT IN('OTHERS')THEN ISNULL(RAMOUNT,0)ELSE 0 END))RAMOUNT"
        StrSql += vbCrLf + ",CONVERT(NUMERIC(20,3),ISNULL(SUM(SNETWT),0)-ISNULL(SUM(RNETWT),0))NETWT"
        StrSql += vbCrLf + ",CONVERT(NUMERIC(15,2),ROUND(SUM(COMMISION)/CASE WHEN ISNULL(SUM(SNETWT),0)-ISNULL(SUM(RNETWT),0)=0 THEN 1 ELSE ISNULL(SUM(SNETWT),0)-ISNULL(SUM(RNETWT),0) END,2),2) SALESINC"
        StrSql += vbCrLf + ",SUM(COMMISION)COMMISSION"
        StrSql += vbCrLf + ",AMOUNT BACKINC"
        StrSql += vbCrLf + ",CASE WHEN ISNULL(METAL,'') IN('OTHERS','PLATINUM') THEN CONVERT(NUMERIC(20,2),(ISNULL(SUM(SPCS),0)-ISNULL(SUM(RPCS),0))*AMOUNT)"
        StrSql += vbCrLf + "ELSE CONVERT(NUMERIC(20,2),(ISNULL(SUM(SNETWT),0)-ISNULL(SUM(RNETWT),0))*AMOUNT) END BACKCOMM"
        StrSql += vbCrLf + ",CONVERT(NUMERIC(20,2),NULL)TOTALAMT"
        StrSql += vbCrLf + ",EMPNAME"
        StrSql += vbCrLf + ",CONVERT (VARCHAR(2),'') COLHEAD,2 RESULT"
        StrSql += vbCrLf + "," & grouper & " AS GROUPER"
        StrSql += vbCrLf + ",ITEM"
        StrSql += vbCrLf + ",METAL"
        StrSql += vbCrLf + ",X.EMPID"
        StrSql += vbCrLf + ",(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID=X.COSTID)COSTNAME"
        StrSql += vbCrLf + "INTO TEMPTABLEDB..TEMPSALESCOMM" & Sysid & ""
        StrSql += vbCrLf + "FROM ("
        StrSql += vbCrLf + "SELECT CONVERT(VARCHAR(300)," & particular & ")PARTICULAR"
        StrSql += vbCrLf + ",CASE WHEN SEP = 'SR' AND COMMISION <> 0 THEN -1*COMMISION WHEN SEP = 'SA' AND COMMISION > 0 THEN COMMISION ELSE NULL END AS COMMISION"
        StrSql += vbCrLf + ",CASE WHEN SEP = 'SR' AND SALESCOMM <> 0 THEN -1*SALESCOMM WHEN SEP = 'SA' AND SALESCOMM > 0 THEN SALESCOMM ELSE NULL END AS SALESCOMM"
        StrSql += vbCrLf + ",CASE WHEN SEP = 'SR' AND OTHERCOMM <> 0 THEN -1*OTHERCOMM WHEN SEP = 'SA' AND OTHERCOMM > 0 THEN OTHERCOMM ELSE NULL END AS OTHERCOMM"
        'StrSql += vbCrLf + ",CASE WHEN SEP = 'SA' AND COMMISION > 0 THEN COMMISION ELSE NULL END AS COMMISION"
        'StrSql += vbCrLf + ",CASE WHEN SEP = 'SA' AND SALESCOMM > 0 THEN SALESCOMM ELSE NULL END AS SALESCOMM"
        'StrSql += vbCrLf + ",CASE WHEN SEP = 'SA' AND OTHERCOMM > 0 THEN OTHERCOMM ELSE NULL END AS OTHERCOMM"
        StrSql += vbCrLf + ",CASE WHEN SEP = 'SA' THEN PCS ELSE NULL END AS SPCS"
        StrSql += vbCrLf + ",CASE WHEN SEP = 'SA' THEN GRSWT ELSE NULL END AS SGRSWT"
        StrSql += vbCrLf + ",CASE WHEN SEP = 'SA' THEN NETWT ELSE NULL END AS SNETWT"
        StrSql += vbCrLf + ",CASE WHEN SEP = 'SA' THEN S.AMOUNT ELSE NULL END AS SAMOUNT"
        StrSql += vbCrLf + ",CASE WHEN SEP = 'SR' THEN PCS ELSE NULL END AS RPCS"
        StrSql += vbCrLf + ",CASE WHEN SEP = 'SR' THEN GRSWT ELSE NULL END AS RGRSWT"
        StrSql += vbCrLf + ",CASE WHEN SEP = 'SR' THEN NETWT ELSE NULL END AS RNETWT"
        StrSql += vbCrLf + ",CASE WHEN SEP = 'SR' THEN S.AMOUNT ELSE NULL END AS RAMOUNT"
        StrSql += vbCrLf + ",CASE WHEN SEP='SA' THEN (ISNULL(BI.AMOUNT,0)*(CASE WHEN ISNULL(S.METAL,'') IN('OTHERS','PLATINUM') THEN S.PCS ELSE S.NETWT END)) ELSE 0 END SABACKAMT "
        StrSql += vbCrLf + ",CASE WHEN SEP='SR' THEN (ISNULL(BI.AMOUNT,0)*(CASE WHEN ISNULL(S.METAL,'') IN('OTHERS','PLATINUM') THEN S.PCS ELSE S.NETWT END)) ELSE 0 END SRBACKAMT"
        StrSql += vbCrLf + ",CONVERT(VARCHAR(100),EMPNAME)AS EMPNAME"
        StrSql += vbCrLf + ",CONVERT(VARCHAR(2),SEP)SEP,CONVERT(VARCHAR(200)," & grouper & ")AS GROUPER"
        StrSql += vbCrLf + ",ITEM,METAL,EMPID,S.COSTID,BI.AMOUNT"
        StrSql += vbCrLf + ",CONVERT(VARCHAR(15),'2013-'+ SUBSTRING(DATENAME(MONTH,TRANDATE),0,4)+'-01')MNTH"
        StrSql += vbCrLf + "FROM TEMPTABLEDB..TEMPSALESCOMMISION" & Sysid & " S"
        StrSql += vbCrLf + "LEFT JOIN " & cnAdminDb & "..BACKENDINCENTIVE AS BI ON BI.ITEMID=S.ITEMID AND DATENAME(MONTH,S.TRANDATE)=BI.MONTH "
        StrSql += vbCrLf + ")X "
        StrSql += vbCrLf + "LEFT JOIN " & cnAdminDb & "..EMPWISEINCENTIVE AS EI ON EI.EMPID=X.EMPID AND EI.COSTID=X.COSTID "
        StrSql += vbCrLf + "AND EI.METALID=(SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME=X.METAL) "
        StrSql += vbCrLf + "AND EI.MONTH=DATENAME(MONTH,X.MNTH)"
        'If chkWithEmptyCommision.Checked = False Then StrSql += vbCrLf + " WHERE ISNULL(COMMISION,0) <> 0"
        StrSql += vbCrLf + "GROUP BY PARTICULAR,X.EMPID,EMPNAME," & grouper & ",ITEM,METAL,X.COSTID,AMOUNT"
        If chkWithEmptyCommision.Checked = False Then StrSql += vbCrLf + " HAVING SUM(COMMISION)<>0"
        'End If
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        StrSql = " UPDATE TEMPTABLEDB..TEMPSALESCOMM" & Sysid & " SET BACKINC = "
        StrSql += vbCrLf + " CASE WHEN CONVERT(NUMERIC, SALESINC) BETWEEN 2 AND 2.5 THEN '0.25' "
        StrSql += vbCrLf + " WHEN CONVERT(NUMERIC, SALESINC) >= 2.5 THEN '0.50' END "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()


        StrSql = vbCrLf + "UPDATE TEMPTABLEDB..TEMPSALESCOMM" & Sysid & " SET BACKCOMM=NULL WHERE BACKCOMM=0 OR METAL='STONE'"
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.ExecuteNonQuery()
        StrSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMPSALESCOMM" & Sysid & "(PARTICULAR,GROUPER,COSTNAME,RESULT,COLHEAD)"
        If grouper = "EMPNAME" Then
            StrSql += vbCrLf + "SELECT DISTINCT GROUPER+' ['+CAST(EMPID AS VARCHAR)+']',GROUPER,COSTNAME,1,'T' FROM TEMPTABLEDB..TEMPSALESCOMM" & Sysid & ""
        Else
            StrSql += vbCrLf + "SELECT DISTINCT GROUPER,GROUPER,COSTNAME,1,'T' FROM TEMPTABLEDB..TEMPSALESCOMM" & Sysid & ""
        End If
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.ExecuteNonQuery()

        StrSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMPSALESCOMM" & Sysid & "(PARTICULAR,COSTNAME,RESULT,COLHEAD)"
        StrSql += vbCrLf + "SELECT DISTINCT COSTNAME,COSTNAME,0,'S1' FROM TEMPTABLEDB..TEMPSALESCOMM" & Sysid & ""
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.ExecuteNonQuery()


        StrSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMPSALESCOMM" & Sysid & ""
        StrSql += vbCrLf + "(PARTICULAR,GROUPER,COSTNAME,RESULT,COLHEAD,COMMISSION,BACKCOMM"
        StrSql += vbCrLf + ",SPCS,SGRSWT,SNETWT,SAMOUNT,RPCS,RGRSWT,RNETWT,RAMOUNT,NETWT)"
        StrSql += vbCrLf + "SELECT 'TOTAL',GROUPER,COSTNAME,5,'S' "
        StrSql += vbCrLf + ",ISNULL(SUM(COMMISSION),0),ISNULL(SUM(BACKCOMM),0)"
        StrSql += vbCrLf + ",ISNULL(SUM(SPCS),0),ISNULL(SUM(SGRSWT),0),ISNULL(SUM(SNETWT),0),ISNULL(SUM(SAMOUNT),0)"
        StrSql += vbCrLf + ",ISNULL(SUM(RPCS),0),ISNULL(SUM(RGRSWT),0),ISNULL(SUM(RNETWT),0),ISNULL(SUM(RAMOUNT),0)"
        StrSql += vbCrLf + ",ISNULL(SUM(NETWT),0)"
        StrSql += vbCrLf + "FROM TEMPTABLEDB..TEMPSALESCOMM" & Sysid & " WHERE COSTNAME IS NOT NULL AND GROUPER IS NOT NULL"
        StrSql += vbCrLf + "AND RESULT=2"
        StrSql += vbCrLf + "GROUP BY GROUPER,COSTNAME"
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.ExecuteNonQuery()
        StrSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMPSALESCOMM" & Sysid & ""
        StrSql += vbCrLf + "(PARTICULAR,GROUPER,COSTNAME,RESULT,COLHEAD,COMMISSION,BACKCOMM"
        StrSql += vbCrLf + ",SPCS,SGRSWT,SNETWT,SAMOUNT,RPCS,RGRSWT,RNETWT,RAMOUNT,NETWT)"
        StrSql += vbCrLf + "SELECT 'GRAND TOTAL','ZZZZZZZZ',COSTNAME,6,'G' "
        StrSql += vbCrLf + ",ISNULL(SUM(COMMISSION),0),ISNULL(SUM(BACKCOMM),0)"
        StrSql += vbCrLf + ",ISNULL(SUM(SPCS),0),ISNULL(SUM(SGRSWT),0),ISNULL(SUM(SNETWT),0),ISNULL(SUM(SAMOUNT),0)"
        StrSql += vbCrLf + ",ISNULL(SUM(RPCS),0),ISNULL(SUM(RGRSWT),0),ISNULL(SUM(RNETWT),0),ISNULL(SUM(RAMOUNT),0)"
        StrSql += vbCrLf + ",ISNULL(SUM(NETWT),0)"
        StrSql += vbCrLf + "FROM TEMPTABLEDB..TEMPSALESCOMM" & Sysid & " WHERE COSTNAME IS NOT NULL AND GROUPER IS NOT NULL"
        StrSql += vbCrLf + "AND RESULT=2"
        StrSql += vbCrLf + "GROUP BY COSTNAME  "
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.ExecuteNonQuery()
        StrSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMPSALESCOMM" & Sysid & ""
        StrSql += vbCrLf + "(PARTICULAR,GROUPER,COSTNAME,RESULT,COLHEAD)"
        StrSql += vbCrLf + "SELECT NULL,'ZZZZZZZZ',COSTNAME,7,'' "
        StrSql += vbCrLf + "FROM TEMPTABLEDB..TEMPSALESCOMM" & Sysid & " WHERE COSTNAME IS NOT NULL AND GROUPER IS NOT NULL"
        StrSql += vbCrLf + "GROUP BY COSTNAME"
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.ExecuteNonQuery()
        StrSql = vbCrLf + "UPDATE TEMPTABLEDB..TEMPSALESCOMM" & Sysid & " SET COMMISSION=NULL WHERE COMMISSION=0"
        StrSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMPSALESCOMM" & Sysid & " SET SPCS=NULL WHERE SPCS=0"
        StrSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMPSALESCOMM" & Sysid & " SET SGRSWT=NULL WHERE SGRSWT=0"
        StrSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMPSALESCOMM" & Sysid & " SET SNETWT=NULL WHERE SNETWT=0"
        StrSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMPSALESCOMM" & Sysid & " SET SAMOUNT=NULL WHERE SAMOUNT=0"
        StrSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMPSALESCOMM" & Sysid & " SET RPCS=NULL WHERE RPCS=0"
        StrSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMPSALESCOMM" & Sysid & " SET RGRSWT=NULL WHERE RGRSWT=0"
        StrSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMPSALESCOMM" & Sysid & " SET RNETWT=NULL WHERE RNETWT=0"
        StrSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMPSALESCOMM" & Sysid & " SET BACKINC=NULL WHERE ISNULL(NETWT,0)=0 OR ISNULL(BACKCOMM,0)=0"
        StrSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMPSALESCOMM" & Sysid & " SET SALESINC=NULL WHERE NETWT=0"
        StrSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMPSALESCOMM" & Sysid & " SET NETWT=NULL WHERE NETWT=0"
        StrSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMPSALESCOMM" & Sysid & " SET RAMOUNT=NULL WHERE RAMOUNT=0"
        StrSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMPSALESCOMM" & Sysid & " SET TOTALAMT=ISNULL(COMMISSION,0)+ISNULL(BACKCOMM,0)"
        StrSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMPSALESCOMM" & Sysid & " SET TOTALAMT=NULL WHERE TOTALAMT=0"
        Cmd = New OleDbCommand(StrSql, cn)
        Cmd.ExecuteNonQuery()
        StrSql = vbCrLf + "SELECT * FROM TEMPTABLEDB..TEMPSALESCOMM" & Sysid & " ORDER BY COSTNAME,GROUPER,RESULT"

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
        If objGridShower.gridView.Columns.Contains("COUNTER") Then objGridShower.gridView.Columns("COUNTER").Visible = Not rbtGrpCounter.Checked
        objGridShower.gridView.Columns("KEYNO").Visible = False
        objGridShower.gridView.Columns("COLHEAD").Visible = False
        objGridShower.gridView.Columns("KEYNO").Visible = False
        objGridShower.gridView.Columns("RESULT").Visible = False
        If objGridShower.gridView.Columns.Contains("ITEM") Then objGridShower.gridView.Columns("ITEM").Visible = False
        If objGridShower.gridView.Columns.Contains("GROUPER") Then objGridShower.gridView.Columns("GROUPER").Visible = False
        If objGridShower.gridView.Columns.Contains("SEP") Then objGridShower.gridView.Columns("SEP").Visible = False
        If objGridShower.gridView.Columns.Contains("RESULT") Then objGridShower.gridView.Columns("RESULT").Visible = False
        If objGridShower.gridView.Columns.Contains("EMPID") Then objGridShower.gridView.Columns("EMPID").Visible = False
        If Val(txtCommPercentage_AMT.Text) <> 0 Then
            objGridShower.gridView.Columns("SALESCOMM").Visible = True
            objGridShower.gridView.Columns("OTHERCOMM").Visible = True
            objGridShower.gridView.Columns("SALESCOMM").HeaderText = "SALES COUNTER COMM"
            objGridShower.gridView.Columns("OTHERCOMM").HeaderText = "OTHER COUNTER COMM"
        Else
            If objGridShower.gridView.Columns.Contains("SALESCOMM") Then objGridShower.gridView.Columns("SALESCOMM").Visible = False
            If objGridShower.gridView.Columns.Contains("OTHERCOMM") Then objGridShower.gridView.Columns("OTHERCOMM").Visible = False
        End If
        If objGridShower.gridView.Columns.Contains("COUNTER") Then objGridShower.gridView.Columns(IIf(rbtGrpCounter.Checked, "COUNTER", grouper)).Visible = False

        objGridShower.gridView.Columns("EMPNAME").Visible = False
        objGridShower.gridView.Columns("SPCS").Visible = False
        objGridShower.gridView.Columns("RPCS").Visible = False
        objGridShower.gridView.Columns("SGRSWT").Visible = False
        objGridShower.gridView.Columns("RGRSWT").Visible = False
        objGridShower.gridView.Columns("SAMOUNT").Visible = False
        objGridShower.gridView.Columns("RAMOUNT").Visible = False
        If objGridShower.gridView.Columns.Contains("COUNTER") Then objGridShower.gridView.Columns("COUNTER").Visible = False
        If objGridShower.gridView.Columns.Contains("METAL") Then objGridShower.gridView.Columns("METAL").Visible = False
        If objGridShower.gridView.Columns.Contains("COSTNAME") Then objGridShower.gridView.Columns("COSTNAME").Visible = False
        FormatGridColumns(objGridShower.gridView, False, False, True, False)
        objGridShower.gridView.Columns("SPCS").HeaderText = "SALES PCS"
        objGridShower.gridView.Columns("SGRSWT").HeaderText = "SALES GRSWT"
        objGridShower.gridView.Columns("SNETWT").HeaderText = "SALES NETWT"
        objGridShower.gridView.Columns("SAMOUNT").HeaderText = "SALES AMOUNT"
        objGridShower.gridView.Columns("RPCS").HeaderText = "RETURN PCS"
        objGridShower.gridView.Columns("RGRSWT").HeaderText = "RETURN GRSWT"
        objGridShower.gridView.Columns("RNETWT").HeaderText = "RETURN NETWT"
        objGridShower.gridView.Columns("RAMOUNT").HeaderText = "RETURN AMOUNT"
        objGridShower.gridView.Columns("COMMISSION").HeaderText = "SALES COMM"
        objGridShower.gridView.Columns("BACKCOMM").HeaderText = "BACKEND COMM"
        objGridShower.gridView.Columns("BACKINC").HeaderText = "BACKEND INC"
        objGridShower.gridView.Columns("SALESINC").HeaderText = "SALES INC"
        objGridShower.gridView.Columns("NETWT").HeaderText = "TOTAL NETWT"
        objGridShower.gridView.Columns("TOTALAMT").HeaderText = "TOTAL INCAMT"
        objGridShower.Text = "EMPLOYEE WISE SALES COMMISION "
        Dim tit As String = "EMPLOYEE WISE SALES COMMISION" + vbCrLf
        If rbtMetal.Checked Then
            tit += " GROUPING BY METAL "
        ElseIf rbtproduct.Checked Then
            tit += " GROUPING BY PRODUCT "
        ElseIf rbtGrpEmployee.Checked Then
            tit += " GROUPING BY EMPLOYEE "
        End If
        tit += " DATE FROM " & dtpFrom.Text + " TO " + dtpTo.Text
        tit += IIf(chkCmbCostCentre.Text <> "" And chkCmbCostCentre.Text <> "ALL", " :" & chkCmbCostCentre.Text, "")
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
        objGridShower.ResizeToolStripMenuItem.Checked = True
        objGridShower.ResizeToolStripMenuItem_Click(Me, New EventArgs)
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
        StrSql += vbCrLf + "  ,1 AS RESULT"
        StrSql += vbCrLf + "  ,'D'AS COLHEAD,EI.WEIGHT"
        If rbtGrpEmployee.Checked Then
            StrSql += vbCrLf + "  ,X.COSTID"
        End If
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
        If rbtGrpEmployee.Checked Then
            StrSql += vbCrLf + "  ,X.COSTID"
        End If
        If chkWithEmptyCommision.Checked = False Then StrSql += vbCrLf + "  HAVING SUM(ISNULL(SACOMMISION,0))<>0 "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        If rbtGrpEmployee.Checked Then
            StrSql = "  IF OBJECT_ID('TEMPTABLEDB..TEMP_OUT" & Sysid & "')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_OUT" & Sysid & ""
            StrSql += vbCrLf + "  SELECT CONVERT(VARCHAR(20),COSTID)PARTICULARS,CONVERT(VARCHAR(20),COSTID)COSTID,EMPID,EMPNAME"
            'StrSql += vbCrLf + "  ,CONVERT(NUMERIC(20,2),ISNULL(SUM(SACOMMISION),0)-ISNULL(SUM(SRCOMMISION),0))SALESINC"
            'StrSql += vbCrLf + "  ,CONVERT(NUMERIC(20,2),ISNULL(SUM(SABACKAMT),0)-ISNULL(SUM(SRBACKAMT),0))BACKINC"
            StrSql += " ,CONVERT(NUMERIC(20,2)"
            StrSql += " ,CASE WHEN PER BETWEEN 90 AND 99 THEN (SUM(SACOMMISION)-SUM(SRCOMMISION))/100*" & insper & ""
            StrSql += " WHEN PER > 99 THEN ISNULL(SUM(SACOMMISION),0)-ISNULL(SUM(SRCOMMISION),0) END) SALESINC"
            StrSql += " ,CONVERT(NUMERIC(20,2)"
            StrSql += " ,CASE WHEN PER BETWEEN 90 AND 99 THEN (SUM(SABACKAMT)-SUM(SRBACKAMT))/100*" & insper & ""
            StrSql += " WHEN PER > 99 THEN ISNULL(SUM(SABACKAMT),0)-ISNULL(SUM(SRBACKAMT),0) END) BACKINC"
            StrSql += vbCrLf + "  ,CONVERT(NUMERIC(20,2),NULL)TOTALINC"
            StrSql += vbCrLf + "  ,1 AS RESULT,' ' COLHEAD"
            StrSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP_OUT" & Sysid & " FROM TEMPTABLEDB..SA_COMMISION" & Sysid & " "
            StrSql += vbCrLf + "  GROUP BY COSTID,EMPID,EMPNAME,PER ORDER BY COSTID,EMPID,EMPNAME"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
            StrSql = " UPDATE TEMPTABLEDB..TEMP_OUT" & Sysid & " SET TOTALINC=SALESINC+BACKINC"
            Cmd = New OleDbCommand(StrSql, cn)
            Cmd.ExecuteNonQuery()
            StrSql = " DELETE FROM  TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE ISNULL(TOTALINC,0)=0"
            Cmd = New OleDbCommand(StrSql, cn)
            Cmd.ExecuteNonQuery()
        Else
            StrSql = "  IF OBJECT_ID('TEMPTABLEDB..TEMP_OUT" & Sysid & "')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_OUT" & Sysid & ""
            StrSql += vbCrLf + "  SELECT CONVERT(VARCHAR(200),EMPNAME + '['+ CONVERT(VARCHAR(4),EMPID) +']')PARTICULAR,*"
            StrSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP_OUT" & Sysid & " FROM TEMPTABLEDB..SA_COMMISION" & Sysid & " "
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        End If

        StrSql = "SELECT COUNT(*) FROM TEMPTABLEDB..TEMP_OUT" & Sysid & ""
        Dim CNT As Integer = Val(GetSqlValue(cn, StrSql))
        If CNT > 0 Then
            If rbtGrpEmployee.Checked Then
                StrSql = "INSERT INTO TEMPTABLEDB..TEMP_OUT" & Sysid & "(PARTICULARS,COSTID,COLHEAD,RESULT,SALESINC,BACKINC,TOTALINC)"
                StrSql += "SELECT 'SUB TOTAL',COSTID,'G',3 "
                StrSql += ",SUM(SALESINC),SUM(BACKINC),SUM(TOTALINC)"
                StrSql += "FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " GROUP BY COSTID"
                Cmd = New OleDbCommand(StrSql, cn)
                Cmd.ExecuteNonQuery()
                StrSql = "INSERT INTO TEMPTABLEDB..TEMP_OUT" & Sysid & "(PARTICULARS,COSTID,COLHEAD,RESULT,SALESINC,BACKINC,TOTALINC)"
                StrSql += "SELECT 'GRAND TOTAL','ZZZZZ','G',4 "
                StrSql += ",SUM(SALESINC),SUM(BACKINC),SUM(TOTALINC)"
                StrSql += "FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " WHERE RESULT=3"
                Cmd = New OleDbCommand(StrSql, cn)
                Cmd.ExecuteNonQuery()
                StrSql = "UPDATE TEMPTABLEDB..TEMP_OUT" & Sysid & " SET SALESINC=NULL WHERE SALESINC=0"
                StrSql += "UPDATE TEMPTABLEDB..TEMP_OUT" & Sysid & " SET BACKINC=NULL WHERE BACKINC=0"
                StrSql += "UPDATE TEMPTABLEDB..TEMP_OUT" & Sysid & " SET TOTALINC=NULL WHERE TOTALINC=0"
                Cmd = New OleDbCommand(StrSql, cn)
                Cmd.ExecuteNonQuery()
                StrSql = "SELECT * FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " ORDER BY COSTID,RESULT,EMPID,EMPNAME"
            Else
                StrSql = "  IF OBJECT_ID('TEMPTABLEDB..TEMP_OUTFINAL" & Sysid & "')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_OUTFINAL" & Sysid & ""
                Cmd = New OleDbCommand(StrSql, cn)
                Cmd.ExecuteNonQuery()
                'StrSql = "SELECT Particular,METAL,RESULT,COLHEAD,EmpId "
                'StrSql += " ,Target,SAPCS SalesPcs"
                'StrSql += " ,CONVERT(NUMERIC(20,3),SASALES) SalesWt"
                'StrSql += " ,SRPCS SalesRetPcs"
                'StrSql += " ,CONVERT(NUMERIC(20,3),SRSALES) SalesRetWt"
                'StrSql += " ,CONVERT(NUMERIC(20,3),(SASALES-SRSALES))TotalWt"
                'StrSql += " ,Per as [Inc%]"
                'StrSql += " ,CONVERT(NUMERIC(20,2)"
                'StrSql += " ,CASE WHEN PER BETWEEN 90 AND 99 THEN ((SACOMMISION-SRCOMMISION)/100*" & insper & ")"
                'StrSql += " WHEN PER > 99 THEN (SACOMMISION-SRCOMMISION) END) SalesInc"
                'StrSql += " ,CONVERT(NUMERIC(20,2)"
                'StrSql += " ,CASE WHEN PER BETWEEN 90 AND 99 THEN ((SABACKAMT-SRBACKAMT)/100*" & insper & ")"
                'StrSql += " WHEN PER > 99 THEN (SABACKAMT-SRBACKAMT) END) BackEndInc"
                ''StrSql += " ,CONVERT(NUMERIC(20,2),(SABACKAMT-SRBACKAMT))BackEndInc"
                'StrSql += " ,CONVERT(NUMERIC(20,2),NULL)TotalInc"
                'StrSql += " INTO TEMPTABLEDB..TEMP_OUTFINAL" & Sysid & ""
                'StrSql += " FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " ORDER BY METAL,RESULT"
                'Cmd = New OleDbCommand(StrSql, cn)
                'Cmd.ExecuteNonQuery()

                StrSql = "SELECT EMPID, PARTICULAR, CONVERT(NUMERIC(20,3),SASALES) SALESWT,  "
                StrSql += " CONVERT(NUMERIC(20,2) ,CASE WHEN PER BETWEEN 90 AND 99 THEN ((SACOMMISION-SRCOMMISION)/100*50) "
                StrSql += " WHEN PER > 99 THEN (SACOMMISION-SRCOMMISION) END) SALESINC, SAPCS SALESPCS,"
                StrSql += " SRPCS SALESRETPCS, CONVERT(NUMERIC(20,3),SRSALES) SALESRETWT, CONVERT(NUMERIC(20,3),(SASALES-SRSALES))TOTALWT,"
                StrSql += " TARGET, PER AS [INC%] , METAL,  CONVERT(NUMERIC(20,2) "
                StrSql += " ,CASE WHEN PER BETWEEN 90 AND 99 THEN ((SABACKAMT-SRBACKAMT)/100*50) WHEN PER > 99 THEN (SABACKAMT-SRBACKAMT) END) "
                StrSql += " BACKENDINC, CONVERT(NUMERIC(20,2),NULL)TOTALINC , RESULT, COLHEAD "
                StrSql += " INTO TEMPTABLEDB..TEMP_OUTFINAL" & Sysid & ""
                StrSql += " FROM TEMPTABLEDB..TEMP_OUT" & Sysid & " ORDER BY METAL,RESULT"
                Cmd = New OleDbCommand(StrSql, cn)
                Cmd.ExecuteNonQuery()

                StrSql = " UPDATE TEMPTABLEDB..TEMP_OUTFINAL" & Sysid & " SET TotalInc=SalesInc+BackEndInc"
                Cmd = New OleDbCommand(StrSql, cn)
                Cmd.ExecuteNonQuery()
                StrSql = "INSERT INTO TEMPTABLEDB..TEMP_OUTFINAL" & Sysid & "(PARTICULAR,METAL,RESULT,COLHEAD)"
                StrSql += "SELECT DISTINCT METAL,METAL,0,'T' FROM TEMPTABLEDB..TEMP_OUT" & Sysid & ""
                Cmd = New OleDbCommand(StrSql, cn)
                Cmd.ExecuteNonQuery()
                StrSql = "INSERT INTO TEMPTABLEDB..TEMP_OUTFINAL" & Sysid & "(PARTICULAR,METAL,RESULT,COLHEAD"
                StrSql += ",SalesPcs,SalesWt,SalesRetPcs,SalesRetWt,TotalWt,SalesInc,BackEndInc,TotalInc"
                StrSql += ")"
                StrSql += "SELECT DISTINCT 'SUB TOTAL',METAL,3,'S' "
                StrSql += ",SUM(SalesPcs),SUM(SalesWt),SUM(SalesRetPcs)"
                StrSql += ",SUM(SalesRetWt),SUM(TotalWt),SUM(SalesInc),SUM(BackEndInc),SUM(TotalInc)"
                StrSql += "FROM TEMPTABLEDB..TEMP_OUTFINAL" & Sysid & " GROUP BY METAL"
                Cmd = New OleDbCommand(StrSql, cn)
                Cmd.ExecuteNonQuery()
                StrSql = "INSERT INTO TEMPTABLEDB..TEMP_OUTFINAL" & Sysid & "(PARTICULAR,METAL,RESULT,COLHEAD"
                StrSql += ",SalesPcs,SalesWt,SalesRetPcs,SalesRetWt,TotalWt,SalesInc,BackEndInc,TotalInc"
                StrSql += ")"
                StrSql += "SELECT DISTINCT 'GRAND TOTAL','ZZZZZZZ',4,'G' "
                StrSql += ",SUM(SalesPcs),SUM(SalesWt),SUM(SalesRetPcs)"
                StrSql += ",SUM(SalesRetWt),SUM(TotalWt),SUM(SalesInc),SUM(BackEndInc),SUM(TotalInc)"
                StrSql += "FROM TEMPTABLEDB..TEMP_OUTFINAL" & Sysid & " WHERE RESULT=3"
                Cmd = New OleDbCommand(StrSql, cn)
                Cmd.ExecuteNonQuery()
                StrSql = "UPDATE TEMPTABLEDB..TEMP_OUTFINAL" & Sysid & " SET SalesPcs=NULL WHERE SalesPcs=0"
                StrSql += " UPDATE TEMPTABLEDB..TEMP_OUTFINAL" & Sysid & " SET SALESWT=NULL WHERE SALESWT=0"
                StrSql += " UPDATE TEMPTABLEDB..TEMP_OUTFINAL" & Sysid & " SET SalesRetPcs=NULL WHERE SalesRetPcs=0"
                StrSql += " UPDATE TEMPTABLEDB..TEMP_OUTFINAL" & Sysid & " SET SalesRetWt=NULL WHERE SalesRetWt=0"
                StrSql += " UPDATE TEMPTABLEDB..TEMP_OUTFINAL" & Sysid & " SET TotalWt=NULL WHERE TotalWt=0"
                StrSql += " UPDATE TEMPTABLEDB..TEMP_OUTFINAL" & Sysid & " SET SalesInc=NULL WHERE SalesInc=0"
                StrSql += " UPDATE TEMPTABLEDB..TEMP_OUTFINAL" & Sysid & " SET BackEndInc=NULL WHERE BackEndInc=0"
                StrSql += " UPDATE TEMPTABLEDB..TEMP_OUTFINAL" & Sysid & " SET TotalInc=NULL WHERE TotalInc=0"
                Cmd = New OleDbCommand(StrSql, cn)
                Cmd.ExecuteNonQuery()
                StrSql = "SELECT * FROM TEMPTABLEDB..TEMP_OUTFINAL" & Sysid & " ORDER BY METAL,RESULT"
            End If
            Dim mna As New System.Globalization.DateTimeFormatInfo
            Dim mnth As String = mna.GetMonthName(Val(dtpFrom.Value.ToString.Substring(5, 2))).ToString
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
            If rbtGrpEmployee.Checked = False Then
                objGridShower.gridView.Columns("PARTICULAR").Visible = True
                objGridShower.gridView.Columns("PARTICULAR").Width = 250
                objGridShower.gridView.Columns("METAL").Visible = False
            Else
                objGridShower.gridView.Columns("COSTID").Visible = False
            End If
            objGridShower.gridView.Columns("KEYNO").Visible = False
            objGridShower.gridView.Columns("RESULT").Visible = False
            objGridShower.gridView.Columns("COLHEAD").Visible = False
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
            objGridShower.ResizeToolStripMenuItem.Checked = True
            objGridShower.ResizeToolStripMenuItem_Click(Me, New EventArgs)
        Else
            MsgBox("No Records found.", MsgBoxStyle.Information)
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
        Dim obj As New frmSalesCommission_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmSalesCommission_Properties))
        rbtSummary.Checked = obj.p_rbtSummary
        rbtDetailed.Checked = obj.p_rbtDetailed
        rbtGrpCounter.Checked = obj.p_rbtGrpCounter
        rbtGrpEmployee.Checked = obj.p_rbtGrpEmployee
        rbtproduct.Checked = obj.p_rbtproduct
        chkWithEmptyCommision.Checked = obj.p_chkWithEmptyCommision
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New frmSalesCommission_Properties
        obj.p_rbtSummary = rbtSummary.Checked
        obj.p_rbtDetailed = rbtDetailed.Checked
        obj.p_chkWithEmptyCommision = chkWithEmptyCommision.Checked
        obj.p_rbtGrpCounter = rbtGrpCounter.Checked
        obj.p_rbtGrpEmployee = rbtGrpEmployee.Checked
        obj.p_rbtproduct = rbtproduct.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmSalesCommission_Properties))
    End Sub

    Private Sub chkcmbmetal_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkcmbmetal.Leave
        Call chkcmbmetal_SelectionChangeCommitted(sender, e)
    End Sub

    Private Sub chkcmbmetal_SelectionChangeCommitted(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkcmbmetal.SelectionChangeCommitted
        Dim metalid As String = GetSelectedMetalid(chkcmbmetal, True)
        StrSql = " SELECT ITEMNAME,CONVERT(vARCHAR,ITEMID),2 RESULT FROM " & cnAdminDb & "..ITEMMAST WHERE 1=1"
        If chkcmbmetal.Text <> "ALL" Then StrSql += " AND METALID IN(" & metalid & ")"
        StrSql += " AND ISNULL(ACTIVE,'Y')<>'N'"
        StrSql += " ORDER BY RESULT,ITEMNAME"
        FillCheckedListBox(StrSql, chkLstItem, , chkItemAll.Checked)
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

    Private Sub chkEmpAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkEmpAll.CheckedChanged
        SetChecked_CheckedList(chkLstEmployee, chkEmpAll.Checked)
    End Sub

    Private Sub chkItemAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkItemAll.CheckedChanged
        SetChecked_CheckedList(chkLstItem, chkItemAll.Checked)
    End Sub

    Private Sub chkSubItemAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSubItemAll.CheckedChanged
        SetChecked_CheckedList(chkLstSubItem, chkSubItemAll.Checked)
    End Sub

    Private Sub chkLstItem_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstItem.Leave
        If Not chkLstItem.CheckedItems.Count > 0 Then
            chkItemAll.Checked = True
        End If
        funcLoadSubItem()
    End Sub

    Private Sub chkLstSubItem_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstSubItem.Leave
        If Not chkLstSubItem.CheckedItems.Count > 0 Then
            chkSubItemAll.Checked = True
        End If
    End Sub

    Private Sub chkLstEmployee_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstEmployee.Leave
        If Not chkLstEmployee.CheckedItems.Count > 0 Then
            chkEmpAll.Checked = True
        End If
    End Sub
    Private Sub funcLoadSubItem()
        Dim chkitemid As String = GetChecked_CheckedList(chkLstItem)
        chkLstSubItem.Items.Clear()
        StrSql = " SELECT SUBITEMNAME,CONVERT(VARCHAR,SUBITEMID),2 RESULT FROM " & cnAdminDb & "..SUBITEMMAST WHERE "
        StrSql += " ITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN(" & chkitemid & "))"
        StrSql += " AND ISNULL(ACTIVE,'Y')<>'N'"
        StrSql += " ORDER BY RESULT,SUBITEMNAME"
        FillCheckedListBox(StrSql, chkLstSubItem, , chkSubItemAll.Checked)
    End Sub
End Class

Public Class frmSalesCommission_Properties
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