Imports System.Data.OleDb
Public Class frmTargetCounterReport
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim das As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim objGridShower As frmGridDispDia
    Dim defaultPic As String = GetAdmindbSoftValue("PICPATH")
    Dim dtCostCentre As DataTable

    Private Sub frmTargetCounterReport_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
        If e.KeyChar = Chr(Keys.Escape) Then
            objGridShower.gridView.Hide()
        End If
    End Sub

    Private Sub frmTargetCounterReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not defaultPic.EndsWith("\") Then defaultPic += "\"
        funcLoadCounterGroup()
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , cnCostName)
        If strUserCentrailsed <> "Y" Then chkCmbCostCentre.Enabled = False

        strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER "
        strSql += " WHERE ACTIVE = 'Y'"
        If cmbCounterGroup.Text <> "" Then strSql += " AND CTRGROUP ='" & cmbCounterGroup.Text & "'"
        strSql += " ORDER BY ITEMCTRNAME"
        FillCheckedListBox(strSql, chkLstCounter)
        SetChecked_CheckedList(chkLstCounter, chkCounterNameAll.Checked)
        btnNew_Click(Me, New EventArgs)
        FormControlRestrict("frmTargetCounterReport", userId, dtpFromDate, dtpToDate, chkLstCounter)
    End Sub
    Public Function FormControlRestrict(ByVal frm As String, ByVal uId As String, ByVal dtCtrl As BrighttechPack.DatePicker, ByVal dtCtrl1 As BrighttechPack.DatePicker, ByVal chkCtrl As CheckedListBox)
        chkCtrl.Items.Clear()
        Dim sql As String = "Select CTRLNAME,CTRLVALUE from " & cnAdminDb & "..USERFRMCTRL where FormName='" & frm & "' and UserId=" & uId
        da = New OleDbDataAdapter(sql, cn)
        Dim dtValue As New DataTable()
        da.Fill(dtValue)
        If dtValue.Rows.Count > 0 Then
            For i As Integer = 0 To dtValue.Rows.Count - 1
                If dtValue.Rows(i).Item("CTRLNAME").ToString = dtCtrl.Name.ToString Then
                    If dtValue.Rows(i).Item("CTRLVALUE").ToString = "MinDate" Then
                        dtCtrl.Value = System.DateTime.Now.Date.AddDays(-1)
                        dtCtrl.Enabled = True
                    ElseIf dtValue.Rows(i).Item("CTRLVALUE").ToString = "MaxDate" Then
                        dtCtrl.Value = System.DateTime.Now.Date
                        dtCtrl.Enabled = True
                    ElseIf dtValue.Rows(i).Item("CTRLVALUE").ToString = "GetDate" Then
                        dtCtrl.Value = System.DateTime.Now.Date
                        dtCtrl.Enabled = False
                        dtCtrl.BackColor = Color.White
                    End If
                End If
                If dtValue.Rows(i).Item("CTRLNAME").ToString = dtCtrl1.Name.ToString Then
                    If dtValue.Rows(i).Item("CTRLVALUE").ToString = "MinDate" Then
                        dtCtrl1.Value = System.DateTime.Now.Date.AddDays(-1)
                        dtCtrl1.Enabled = True
                    ElseIf dtValue.Rows(i).Item("CTRLVALUE").ToString = "MaxDate" Then
                        dtCtrl1.Value = System.DateTime.Now.Date.AddDays(1)
                        dtCtrl1.Enabled = True
                    ElseIf dtValue.Rows(i).Item("CTRLVALUE").ToString = "GetDate" Then
                        dtCtrl1.Value = System.DateTime.Now.Date
                        dtCtrl1.Enabled = False
                        dtCtrl1.BackColor = Color.White
                    End If
                End If
                If dtValue.Rows(i).Item("CTRLNAME").ToString = chkCtrl.Name.ToString Then
                    Dim ctrlValue As String = dtValue.Rows(i).Item("CTRLVALUE").ToString
                    If ctrlValue = dtValue.Rows(i).Item("CTRLVALUE").ToString Then
                        chkCtrl.Items.Add(ctrlValue)
                        chkCtrl.SetItemChecked(i, True)
                        chkCtrl.Enabled = False
                    End If
                End If
            Next
        End If
    End Function
    Function funcLoadCounterGroup() As Integer
        cmbCounterGroup.Items.Clear()
        cmbCounterGroup.Items.Add("")
        strSql = " SELECT CTRGROUP FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ACTIVE = 'Y' ORDER BY CTRGROUP"
        objGPack.FillCombo(strSql, cmbCounterGroup, False)
        cmbCounterGroup.Text = ""
    End Function

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub
    Private Sub funcNew()
        objGPack.TextClear(Me)
        dtpFromDate.Value = GetServerDate()
        dtpToDate.Value = GetServerDate()
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , cnCostName)
        dtpFromDate.Select()
        Prop_Gets()
    End Sub

    Private Sub chkCounterNameAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCounterNameAll.CheckedChanged
        SetChecked_CheckedList(chkLstCounter, chkCounterNameAll.Checked)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Private Sub chkLstCounter_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstCounter.GotFocus
    End Sub
    Private chkCounterSelectAll As Boolean = False
    Public Property p_chkCounterNameAll() As Boolean
        Get
            Return chkCounterSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkCounterSelectAll = value
        End Set
    End Property
    Private chkLstItemCounter As New List(Of String)
    Public Property p_chkLstCounter() As List(Of String)
        Get
            Return chkLstItemCounter
        End Get
        Set(ByVal value As List(Of String))
            chkLstItemCounter = value
        End Set
    End Property
    Private rbtDetaile As Boolean = True
    Public Property p_rbtDetailed() As Boolean
        Get
            Return rbtDetaile
        End Get
        Set(ByVal value As Boolean)
            rbtDetaile = value
        End Set
    End Property
    Private rbtSummarys As Boolean = False
    Public Property p_rbtSummary() As Boolean
        Get
            Return rbtSummarys
        End Get
        Set(ByVal value As Boolean)
            rbtSummarys = value
        End Set
    End Property
 

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim dtItem As New DataTable
        Dim isWtUnit As Boolean = False
        Dim chkItemCounter As String = GetChecked_CheckedList(chkLstCounter)
        Dim ChkCostids As String = GetSelectedCostId(chkCmbCostCentre, True)

        strSql = vbCrLf + " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPDETAIL')>0 DROP TABLE TEMPDETAIL"
        strSql += vbCrLf + " SELECT ITEMID,SUBITEMID,ITEMCTRID,SUM(TPCS) AS TPCS,SUM(TGRSWT) AS TGRSWT,"
        strSql += vbCrLf + " SUM(MPCS) AS MPCS, SUM(MGRSWT) AS MGRSWT,SUM(YPCS) AS YPCS, SUM(YGRSWT) AS YGRSWT "
        strSql += vbCrLf + " ,SUM(ISNULL(TPCS,0)-ISNULL(YPCS,0)) AS BPCS,SUM(ISNULL(TGRSWT,0)-ISNULL(YGRSWT,0)) AS BGRSWT,SUM(SPCS) SPCS,SUM(SGRSWT) SGRSWT "
        strSql += vbCrLf + " INTO TEMPDETAIL"
        strSql += vbCrLf + " FROM ("
        strSql += vbCrLf + " SELECT T.ITEMID,T.SUBITEMID,T.ITEMCTRID,SUM(PCS) AS TPCS,SUM(T.GRSWT) AS TGRSWT,0 AS MPCS,CONVERT(NUMERIC(13,2),0) AS MGRSWT"
        strSql += vbCrLf + " ,0 AS YPCS,CONVERT(NUMERIC(13,2),0) AS YGRSWT,0 AS SPCS,CONVERT(NUMERIC(13,2),0) AS SGRSWT FROM " & cnAdminDb & "..TARGETCOUNTER AS T , " & cnAdminDb & "..SUBITEMMAST AS S"
        strSql += vbCrLf + " WHERE S.ITEMID = T.ITEMID AND S.SUBITEMID = T.SUBITEMID AND"
        strSql += vbCrLf + " T.ITEMCTRID IN(SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE CTRGROUP = 0)"
        If rdbGram.Checked Then
            strSql += vbCrLf + " AND ISNULL(WEIGHTUNIT,'') <> 'C'"
        ElseIf rdbCarot.Checked Then
            strSql += vbCrLf + " AND ISNULL(WEIGHTUNIT,'') = 'C' "
        End If
        strSql += vbCrLf + " GROUP BY T.ITEMID,T.SUBITEMID,T.ITEMCTRID "
        strSql += vbCrLf + " UNION ALL"
        If rdbGram.Checked Then
            strSql += vbCrLf + " SELECT I.ITEMID,I.SUBITEMID,I.ITEMCTRID,0 AS TPCS,0 AS TGRSWT,SUM(I.PCS)AS MPCS,"
            strSql += vbCrLf + " SUM(I.GRSWT) AS MGRSWT "
            strSql += vbCrLf + " ,0 AS YPCS,CONVERT(NUMERIC(13,2),0) AS YGRSWT,0 AS SPCS,CONVERT(NUMERIC(13,2),0) AS SGRSWT"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
            strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..TARGETCOUNTER T ON T.ITEMID=I.ITEMID AND T.SUBITEMID=I.SUBITEMID AND T.ITEMCTRID=I.ITEMCTRID"
            strSql += vbCrLf + " WHERE I.TRANDATE BETWEEN"
            strSql += vbCrLf + " '" & dtpFromDate.Value.ToString("yyyy-MM-dd") & "' and '" & dtpToDate.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') <> 'Y' AND I.TRANTYPE IN ('SA','OD')"
            strSql += vbCrLf + " AND I.ITEMCTRID IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE CTRGROUP = 0)"
            strSql += vbCrLf + " AND I.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..TARGETCOUNTER WHERE ISNULL(WEIGHTUNIT,'') <> 'C')"
            If ChkCostids <> "''" Then strSql += vbCrLf + " AND I.COSTID IN (" & ChkCostids & ")"
            strSql += vbCrLf + " GROUP BY I.ITEMID,I.SUBITEMID,I.ITEMCTRID,T.WEIGHTUNIT,I.SNO"
        ElseIf rdbCarot.Checked Then
            strSql += vbCrLf + " SELECT I.ITEMID,I.SUBITEMID,I.ITEMCTRID,0 AS TPCS,0 AS TGRSWT,SUM(I.PCS) AS MPCS,"
            strSql += vbCrLf + " (SELECT SUM(ISIS.STNWT) FROM " & cnStockDb & "..ISSSTONE ISIS WHERE ISIS.ISSSNO=I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(DIASTONE,'') = 'D')) "
            strSql += vbCrLf + " AS MGRSWT "
            strSql += vbCrLf + " ,0 AS YPCS,CONVERT(NUMERIC(13,2),0) AS YGRSWT,0 AS SPCS,CONVERT(NUMERIC(13,2),0) AS SGRSWT"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
            strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..TARGETCOUNTER T ON T.ITEMID=I.ITEMID AND T.SUBITEMID=I.SUBITEMID AND T.ITEMCTRID=I.ITEMCTRID"
            strSql += vbCrLf + " WHERE I.TRANDATE BETWEEN"
            strSql += vbCrLf + " '" & dtpFromDate.Value.ToString("yyyy-MM-dd") & "' and '" & dtpToDate.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') <> 'Y' AND I.TRANTYPE IN ('SA','OD')"
            strSql += vbCrLf + " AND I.ITEMCTRID IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE CTRGROUP = 0)"
            If ChkCostids <> "''" Then strSql += vbCrLf + " AND I.COSTID IN (" & ChkCostids & ")"
            'STRSQL += VBCRLF + " AND I.ITEMID IN (SELECT ITEMID FROM " & CNADMINDB & "..TARGETCOUNTER WHERE ISNULL(WEIGHTUNIT,'') = 'C')"
            strSql += vbCrLf + " GROUP BY I.ITEMID,I.SUBITEMID,I.ITEMCTRID,T.WEIGHTUNIT,I.SNO"
        End If
        strSql += vbCrLf + " UNION ALL"
        If rdbGram.Checked Then
            strSql += vbCrLf + " SELECT I.ITEMID,I.SUBITEMID,I.ITEMCTRID,0 AS TPCS,0 AS TGRSWT,0 AS MPCS,0 AS MGRSWT"
            strSql += vbCrLf + " ,SUM(I.PCS)AS YPCS,"
            strSql += vbCrLf + " SUM(I.GRSWT) "
            strSql += vbCrLf + " AS YGRSWT ,0 AS SPCS,CONVERT(NUMERIC(13,2),0) AS SGRSWT "
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
            strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..TARGETCOUNTER T ON T.ITEMID=I.ITEMID AND T.SUBITEMID=I.SUBITEMID AND T.ITEMCTRID=I.ITEMCTRID"
            strSql += vbCrLf + " WHERE I.TRANDATE <= '" & dtpToDate.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') <> 'Y' AND I.TRANTYPE IN ('SA','OD')"
            strSql += vbCrLf + " AND I.ITEMCTRID IN(SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE CTRGROUP = 0)"
            strSql += vbCrLf + " AND I.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..TARGETCOUNTER WHERE ISNULL(WEIGHTUNIT,'') <> 'C')"
            If ChkCostids <> "''" Then strSql += vbCrLf + " AND I.COSTID IN (" & ChkCostids & ")"
            strSql += vbCrLf + " GROUP BY I.ITEMID,I.SUBITEMID,I.ITEMCTRID,T.WEIGHTUNIT,I.SNO"
        ElseIf rdbCarot.Checked Then
            strSql += vbCrLf + " SELECT I.ITEMID,I.SUBITEMID,I.ITEMCTRID,0 AS TPCS,0 AS TGRSWT,0 AS MPCS,0 AS MGRSWT"
            strSql += vbCrLf + " ,SUM(I.PCS)AS YPCS,"
            strSql += vbCrLf + " (SELECT SUM(ISIS.STNWT) FROM " & cnStockDb & "..ISSSTONE ISIS WHERE ISIS.ISSSNO=I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(DIASTONE,'') = 'D'))  "
            strSql += vbCrLf + " AS YGRSWT ,0 AS SPCS,CONVERT(NUMERIC(13,2),0) AS SGRSWT "
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
            strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..TARGETCOUNTER T ON T.ITEMID=I.ITEMID AND T.SUBITEMID=I.SUBITEMID AND T.ITEMCTRID=I.ITEMCTRID"
            strSql += vbCrLf + " WHERE I.TRANDATE <= '" & dtpToDate.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') <> 'Y' AND I.TRANTYPE IN ('SA','OD')"
            strSql += vbCrLf + " AND I.ITEMCTRID IN(SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE CTRGROUP = 0)"
            If ChkCostids <> "''" Then strSql += vbCrLf + " AND I.COSTID IN (" & ChkCostids & ")"
            strSql += vbCrLf + " GROUP BY I.ITEMID,I.SUBITEMID,I.ITEMCTRID,T.WEIGHTUNIT,I.SNO"
        End If
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT I.ITEMID,I.SUBITEMID,I.ITEMCTRID,0 AS TPCS,0 AS TGRSWT,0 AS MPCS,0 AS MGRSWT"
        strSql += vbCrLf + " ,0 AS YPCS,0 AS YGRSWT,SUM(I.PCS) AS SPCS,SUM(I.GRSWT) AS SGRSWT "
        strSql += vbCrLf + " from " & cnAdminDb & "..ITEMTAG I"
        strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..TARGETCOUNTER T ON T.ITEMID=I.ITEMID AND T.SUBITEMID=I.SUBITEMID AND T.ITEMCTRID=I.ITEMCTRID"
        strSql += vbCrLf + " WHERE I.RECDATE <= '" & dtpToDate.Value.ToString("yyyy-MM-dd") & "' AND (ISSDATE IS NULL OR ISSDATE >= '" & dtpToDate.Value.ToString("yyyy-MM-dd") & "')"
        strSql += vbCrLf + " AND I.ITEMCTRID IN(SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE CTRGROUP = 0)"
        strSql += vbCrLf + " AND I.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..TARGETCOUNTER WHERE 1=1 "
        If rdbGram.Checked Then
            strSql += vbCrLf + " AND ISNULL(WEIGHTUNIT,'') <> 'C')"
        ElseIf rdbCarot.Checked Then
            strSql += vbCrLf + " AND ISNULL(WEIGHTUNIT,'') = 'C')"
        End If
        If ChkCostids <> "''" Then strSql += vbCrLf + " AND I.COSTID IN (" & ChkCostids & ")"
        strSql += vbCrLf + " GROUP BY I.ITEMID,I.SUBITEMID,I.ITEMCTRID,T.WEIGHTUNIT"
        strSql += vbCrLf + " ) AS X"
        strSql += vbCrLf + " GROUP BY ITEMID,SUBITEMID,ITEMCTRID"

        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPCOUNTERDETAIL')>0 DROP TABLE TEMPCOUNTERDETAIL"
        If rbtSummary.Checked = True Then
            strSql += vbCrLf + " SELECT COUNTERNAME,ITEMNAME,SUBITEMNAME,SUM(TPCS)TPCS,SUM(TGRSWT)TGRSWT,SUM(MPCS)MPCS,"
            strSql += vbCrLf + " SUM(MGRSWT)MGRSWT,SUM(YPCS)YPCS,SUM(YGRSWT)YGRSWT,SUM(BPCS)BPCS,SUM(BGRSWT)BGRSWT,SUM(SPCS)SPCS,SUM(SGRSWT)SGRSWT"
            strSql += vbCrLf + " CONVERT (VARCHAR(60),ITEM)ITEM,CONVERT (VARCHAR(60),COUNTER)COUNTER,1 RESULT,CONVERT(VARCHAR(2),'') COLHEAD"
        Else
            strSql += vbCrLf + " SELECT COUNTERNAME,ITEMNAME,SUBITEMNAME,TPCS,TGRSWT,MPCS,MGRSWT,YPCS,YGRSWT,BPCS,"
            strSql += vbCrLf + " BGRSWT,SPCS,SGRSWT,CONVERT (VARCHAR(60),ITEM)ITEM,CONVERT (VARCHAR(60),COUNTER)COUNTER,1 RESULT,CONVERT(VARCHAR(2),'') COLHEAD"
        End If
        strSql += vbCrLf + " INTO TEMPCOUNTERDETAIL"
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT (SELECT  ITEMNAME FROM " & cnAdminDb & "..ITEMMAST IM WHERE IM.ITEMID=T.ITEMID)AS ITEMNAME,"
        strSql += vbCrLf + " (SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER IC WHERE IC.ITEMCTRID=T.ITEMCTRID)AS COUNTERNAME,"
        strSql += vbCrLf + " (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST SM WHERE SM.SUBITEMID=T.SUBITEMID ) AS SUBITEMNAME,"
        strSql += vbCrLf + " T.TPCS,T.TGRSWT,T.MPCS,T.MGRSWT,T.YPCS,T.YGRSWT,T.BPCS,T.BGRSWT,T.SPCS,T.SGRSWT"
        strSql += vbCrLf + " ,(SELECT  ITEMNAME FROM " & cnAdminDb & "..ITEMMAST IM WHERE IM.ITEMID=T.ITEMID)AS ITEM,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER IC WHERE IC.ITEMCTRID=T.ITEMCTRID)AS COUNTER FROM TEMPDETAIL T"
        If chkItemCounter <> "" Then strSql += vbCrLf + " WHERE T.ITEMCTRID IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & chkItemCounter & "))"
        strSql += vbCrLf + " )X"
        If rbtSummary.Checked = True Then
            strSql += vbCrLf + " GROUP BY COUNTER,ITEM,COUNTERNAME,ITEMNAME,SUBITEMNAME "
        End If
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + "   IF (SELECT COUNT(*) FROM TEMPCOUNTERDETAIL)>0"
        strSql += vbCrLf + " BEGIN"
        If rbtDetailed.Checked = True Then
            strSql += vbCrLf + " INSERT INTO TEMPCOUNTERDETAIL(COUNTER,COUNTERNAME,ITEMNAME,RESULT,COLHEAD) "
            strSql += vbCrLf + " SELECT DISTINCT COUNTER,COUNTER,'',0 RESULT,'T' COLHEAD"
            strSql += vbCrLf + " FROM TEMPCOUNTERDETAIL"
            strSql += vbCrLf + " INSERT INTO"
            strSql += vbCrLf + " TEMPCOUNTERDETAIL"
            strSql += vbCrLf + "(COUNTER,COUNTERNAME,ITEMNAME,RESULT,COLHEAD,TPCS,TGRSWT,MPCS,MGRSWT,YPCS,YGRSWT,BPCS,BGRSWT,SPCS,SGRSWT)"
            strSql += vbCrLf + " SELECT DISTINCT COUNTER,'SUB TOTAL','',2 RESULT,'S' COLHEAD,SUM(TPCS),SUM(TGRSWT),SUM(MPCS), SUM(MGRSWT), "
            strSql += vbCrLf + " SUM(YPCS), SUM(YGRSWT), SUM(BPCS), SUM(BGRSWT),SUM(SPCS),SUM(SGRSWT)"
            strSql += vbCrLf + " FROM TEMPCOUNTERDETAIL"
            strSql += vbCrLf + " GROUP BY COUNTER "
        End If
        strSql += vbCrLf + " INSERT INTO"
        strSql += vbCrLf + " TEMPCOUNTERDETAIL(COUNTER,COUNTERNAME,ITEMNAME,RESULT,COLHEAD,"
        strSql += vbCrLf + " TPCS,TGRSWT,MPCS,MGRSWT,YPCS,YGRSWT,BPCS,BGRSWT,SPCS,SGRSWT) "
        strSql += vbCrLf + " SELECT DISTINCT 'ZZZZ','GRAND TOTAL','',3 RESULT,'G' COLHEAD, SUM(TPCS),"
        strSql += vbCrLf + " SUM(TGRSWT),SUM(MPCS),SUM(MGRSWT), SUM(YPCS), SUM(YGRSWT), SUM(BPCS), SUM(BGRSWT),SUM(SPCS),SUM(SGRSWT)"
        strSql += vbCrLf + " FROM TEMPCOUNTERDETAIL"
        strSql += vbCrLf + " WHERE RESULT=1 "
        strSql += vbCrLf + " END"
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        With dtMergeHeader
            .Columns.Add("COUNTERNAME~ITEMNAME~SUBITEMNAME", GetType(String))
            .Columns.Add("TPCS~TGRSWT", GetType(String))
            .Columns.Add("MPCS~MGRSWT", GetType(String))
            .Columns.Add("YPCS~YGRSWT", GetType(String))
            .Columns.Add("BPCS~BGRSWT", GetType(String))
            .Columns.Add("SPCS~SGRSWT", GetType(String))
            .Columns("COUNTERNAME~ITEMNAME~SUBITEMNAME").Caption = "DESCRIPTION"
            .Columns("TPCS~TGRSWT").Caption = "TARGET"
            .Columns("MPCS~MGRSWT").Caption = "MONTH" & Month(Me.dtpFromDate.Value)
            .Columns("YPCS~YGRSWT").Caption = "YTD"
            .Columns("BPCS~BGRSWT").Caption = "BALANCE"
            .Columns("SPCS~SGRSWT").Caption = "AVAILABLE_STK"
        End With

        strSql = "SELECT * FROM TEMPCOUNTERDETAIL ORDER BY COUNTER,RESULT,ITEM,SUBITEMNAME"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtItem)
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Size = New Size(1032, 735)

        objGridShower.Text = "TARGET COUNTER SALES REPORT "
        Dim tit As String = "TARGET COUNTER SALES REPORT " + dtpFromDate.Text + "-" + dtpToDate.Text
        'Dim tit As String = "TARGET COUNTER SALES REPORT" + IIf(chkItemCounter <> "", chkItemCounter.Replace("'", ""), " ALL ITEMCOUNTER")
        'tit += "AS ON DATE  " + dtpToDate.Text
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtMergeHeader)
        objGridShower.dsGrid.Tables.Add(dtItem)
        objGridShower.gridViewHeader.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(1)
        AddHandler objGridShower.gridView.KeyPress, AddressOf GridView_KeyPress
        objGridShower.pnlFooter.Visible = False
        objGridShower.FormReLocation = False
        objGridShower.FormReSize = False
        objGridShower.Show()
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        DataGridView_Detailed(objGridShower.gridView)
        objGridShower.FormReSize = True
        objGridShower.gridViewHeader.Visible = True
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle = objGridShower.gridView.ColumnHeadersDefaultCellStyle
        GridViewFormat()
        GridHead()
        Prop_Sets()
    End Sub
    Private Sub GridHead()
        With objGridShower.gridView
            objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle = .ColumnHeadersDefaultCellStyle
            objGridShower.gridViewHeader.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised
            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken
            Dim TEMPCOLWIDTH As Integer = 0
            TEMPCOLWIDTH += .Columns("COUNTERNAME").Width + .Columns("ITEMNAME").Width + .Columns("SUBITEMNAME").Width
            objGridShower.gridViewHeader.Columns("COUNTERNAME~ITEMNAME~SUBITEMNAME").Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns("COUNTERNAME~ITEMNAME~SUBITEMNAME").HeaderText = "DESCRIPTION"
            TEMPCOLWIDTH = 0
            TEMPCOLWIDTH += .Columns("TPCS").Width + .Columns("TGRSWT").Width
            objGridShower.gridViewHeader.Columns("TPCS~TGRSWT").Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns("TPCS~TGRSWT").HeaderText = "TARGET"
            TEMPCOLWIDTH = 0
            TEMPCOLWIDTH += .Columns("MPCS").Width + .Columns("MGRSWT").Width
            objGridShower.gridViewHeader.Columns("MPCS~MGRSWT").Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns("MPCS~MGRSWT").HeaderText = "MONTH - " & MonthName(Month(Me.dtpToDate.Value)) & "'" & Year(dtpToDate.Value)
            TEMPCOLWIDTH = 0
            TEMPCOLWIDTH += .Columns("YPCS").Width + .Columns("YGRSWT").Width
            objGridShower.gridViewHeader.Columns("YPCS~YGRSWT").Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns("YPCS~YGRSWT").HeaderText = "YTD"
            TEMPCOLWIDTH = 0
            TEMPCOLWIDTH += .Columns("BPCS").Width + .Columns("BGRSWT").Width
            objGridShower.gridViewHeader.Columns("BPCS~BGRSWT").Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns("BPCS~BGRSWT").HeaderText = "BALANCE"
            TEMPCOLWIDTH = 0
            TEMPCOLWIDTH += .Columns("SPCS").Width + .Columns("SGRSWT").Width
            objGridShower.gridViewHeader.Columns("SPCS~SGRSWT").Width = TEMPCOLWIDTH
            objGridShower.gridViewHeader.Columns("SPCS~SGRSWT").HeaderText = "AVAILABLE STOCK"
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To .ColumnCount - 1
                If .Columns(cnt).Visible Then colWid += .Columns(cnt).Width
                If UCase(Mid(.Columns(cnt).Name, 2)) = "PCS" Then .Columns(cnt).HeaderText = "PCS"
                If UCase(Mid(.Columns(cnt).Name, 2)) = "GRSWT" Then .Columns(cnt).HeaderText = "GRS WT"
            Next
        End With
    End Sub

    Private Sub DataGridView_Detailed(ByVal dgv As DataGridView)
        With dgv
            If dgv.Rows.Count > 0 Then
                For Each dgvCol As DataGridViewColumn In dgv.Columns
                    dgvCol.Visible = True
                Next
                .Columns("COUNTERNAME").Width = 150
                .Columns("ITEMNAME").Width = 100
                .Columns("SUBITEMNAME").Width = 170
                .Columns("Tpcs").Width = 60
                .Columns("Tgrswt").Width = 90
                .Columns("Mpcs").Width = 60
                .Columns("Ypcs").Width = 60
                .Columns("MGrswt").Width = 80
                .Columns("YGrswt").Width = 80
                .Columns("Bpcs").Width = 60
                .Columns("Bgrswt").Width = 90
                .Columns("Spcs").Width = 60
                .Columns("Sgrswt").Width = 90
                .Columns("RESULT").Visible = False
                .Columns("COLHEAD").Visible = False
                .Columns("ITEM").Visible = False
                .Columns("COUNTER").Visible = False
                FormatGridColumns(dgv, False, False, , False)
            End If
        End With
    End Sub
    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In objGridShower.gridView.Rows
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
                End Select
            End With
        Next
    End Function
    Private Sub GridView_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs)
        If e.KeyChar = Chr(Keys.Enter) Then
            With objGridShower
                If Not .gridView.RowCount > 0 Then Exit Sub
                If .gridView.CurrentRow Is Nothing Then Exit Sub
                'If .gridView.CurrentRow.Cells("ITEMID").Value.ToString = "" Then Exit Sub
                'If .gridView.CurrentRow.Cells("TAGNO").Value.ToString = "" Then Exit Sub
                'Dim objTagViewer As New frmTagImageViewer( _
                ' .gridView.CurrentRow.Cells("TAGNO").Value.ToString, _
                ' Val(.gridView.CurrentRow.Cells("ITEMID").Value.ToString), _
                ' BrighttechPack.Methods.GetRights(_DtUserRights, frmTagCheck.Name, BrighttechPack.Methods.RightMode.Authorize, False))
                'objTagViewer.ShowDialog()
            End With
        End If
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New frmTagItemsPurchaseReport_Old_Properties
        obj.p_chkItemCounterSelectAll = chkCounterNameAll.Checked
        GetChecked_CheckedList(chkLstCounter, obj.p_chkLstItemCounter)
        obj.p_rbtDetailed = rbtDetailed.Checked
        obj.p_rbtSummary = rbtSummary.Checked

        SetSettingsObj(obj, Me.Name, GetType(frmTagItemsPurchaseReport_Old_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmTagItemsPurchaseReport_Old_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmTagItemsPurchaseReport_Old_Properties))
        chkCounterNameAll.Checked = obj.p_chkItemCounterSelectAll
        SetChecked_CheckedList(chkLstCounter, obj.p_chkLstItemCounter, Nothing)
        rbtDetailed.Checked = obj.p_rbtDetailed
        rbtSummary.Checked = obj.p_rbtSummary
    End Sub

    Private Sub cmbCounterGroup_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCounterGroup.SelectedIndexChanged
        Dim status As Boolean = False
        strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER "
        strSql += " WHERE ACTIVE = 'Y'"
        If cmbCounterGroup.Text <> "" Then strSql += " AND CTRGROUP ='" & cmbCounterGroup.Text & "'"
        strSql += " ORDER BY ITEMCTRNAME"
        FillChkedListBox(strSql, chkLstCounter)
    End Sub
    Private Sub FillChkedListBox(ByVal qry As String, ByVal chkLst As CheckedListBox, Optional ByVal clear As Boolean = True, Optional ByVal Check As Boolean = True)
        chkLst.Items.Clear()
        Dim dt As New DataTable
        da = New OleDbDataAdapter(qry, cn)
        da.Fill(dt)
        For Each ro As DataRow In dt.Rows
            chkLst.Items.Add(ro(0).ToString)
            chkLst.SetItemChecked(chkLst.Items.Count - 1, Check)
        Next
    End Sub

    Private Sub frmTargetCounterReport_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.F3 Then funcNew()
        If e.KeyCode = Keys.F12 Then Me.Close()
    End Sub
End Class