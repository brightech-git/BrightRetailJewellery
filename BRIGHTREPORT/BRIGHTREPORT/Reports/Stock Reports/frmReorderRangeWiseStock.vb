Imports System.Data.OleDb
Public Class frmReorderRangeWiseStock
    'CALNO 599 -VASANTHAN, CLIENT-Senthil Murugan
    Dim strSql As String = Nothing
    Dim da As OleDbDataAdapter
    Dim max As Double = 0
    Dim dt As New DataTable
    Dim cmd As OleDbCommand
    Dim verSubTotalIndex(0) As Integer
    Dim frdt As New DataTable()

    Private Sub frmReorderRangeWiseStock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) And tabMain.SelectedTab.Name = tabView.Name Then
            btnBack_Click(Me, New EventArgs)
        End If
    End Sub

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.

        ''Check CostCentre
        strSql = " select 1 from " & cnAdminDb & "..SoftControl where ctlId = 'COSTCENTRE' and ctlText = 'Y'"
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            cmbCostCentre.Enabled = True
        Else
            cmbCostCentre.Enabled = False
        End If

        ''Load CostCentre
        strSql = " Select CostName from " & cnAdminDb & "..CostCentre order by CostName"
        cmbCostCentre.Items.Clear()
        cmbCostCentre.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbCostCentre, False)
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then cmbCostCentre.Enabled = False

        ''Load itemName
        strSql = " select itemName from " & cnAdminDb & "..itemMast order by itemName "
        cmbItemName.Items.Clear()
        cmbItemName.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbItemName, False)
        cmbItemName.Focus()

        ''Load DesignerName
        strSql = " Select DesignerName from " & cnAdminDb & "..Designer Order by DesignerName"
        cmbDesigner.Items.Clear()
        cmbDesigner.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbDesigner, False)

        ''Load Counter
        strSql = " Select ItemCtrName from " & cnAdminDb & "..itemCounter order by ItemCtrName"
        cmbCounter.Items.Clear()
        cmbCounter.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbCounter, False)
        txtWeightDiff_WET.Text = "10"
    End Sub

    Private Sub frmReorderRangeWiseStock_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        btnNew_Click(Me, New EventArgs)
        'Me.WindowState = FormWindowState.Maximized
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        Me.tabMain.SelectedTab = tabGen
    End Sub

    Function SetRangeValues() As Integer
        Dim diff As Double = Val(txtWeightDiff_WET.Text)
        txtRange1From_WET.Text = "0.001"
        txtRange1To_WET.Text = Format(diff, "0.000")

        txtRange2From_WET.Text = Format(Val(txtRange1To_WET.Text) + Val(txtRange1From_WET.Text), "0.000")
        txtRange2To_WET.Text = Format(Val(txtRange1To_WET.Text) + diff, "0.000")

        txtRange3From_WET.Text = Format(Val(txtRange2To_WET.Text) + Val(txtRange1From_WET.Text), "0.000")
        txtRange3To_WET.Text = Format(Val(txtRange2To_WET.Text) + diff, "0.000")

        txtRange4From_WET.Text = Format(Val(txtRange3To_WET.Text) + Val(txtRange1From_WET.Text), "0.000")
        txtRange4To_WET.Text = Format(Val(txtRange3To_WET.Text) + diff, "0.000")

        txtRange5From_WET.Text = Format(Val(txtRange4To_WET.Text) + Val(txtRange1From_WET.Text), "0.000")
        txtRange5To_WET.Text = Format(Val(txtRange4To_WET.Text) + diff, "0.000")

        txtRange6From_WET.Text = Format(Val(txtRange5To_WET.Text) + Val(txtRange1From_WET.Text), "0.000")
        txtRange6To_WET.Text = Format(Val(txtRange5To_WET.Text) + diff, "0.000")

        txtRange7From_WET.Text = Format(Val(txtRange6To_WET.Text) + Val(txtRange1From_WET.Text), "0.000")
        txtRange7To_WET.Text = Format(Val(txtRange6To_WET.Text) + diff, "0.000")

        txtRange8From_WET.Text = Format(Val(txtRange7To_WET.Text) + Val(txtRange1From_WET.Text), "0.000")
        txtRange8To_WET.Text = Format(Val(txtRange7To_WET.Text) + diff, "0.000")

        txtRange9From_WET.Text = Format(Val(txtRange8To_WET.Text) + Val(txtRange1From_WET.Text), "0.000")
        txtRange9To_WET.Text = Format(Val(txtRange8To_WET.Text) + diff, "0.000")

        txtRange10From_WET.Text = Format(Val(txtRange9To_WET.Text) + Val(txtRange1From_WET.Text), "0.000")
        txtRange10To_WET.Text = Format(Val(txtRange9To_WET.Text) + diff, "0.000")

        txtRange11From_WET.Text = Format(Val(txtRange10To_WET.Text) + Val(txtRange1From_WET.Text), "0.000")
        txtRange11To_WET.Text = Format(Val(txtRange10To_WET.Text) + diff, "0.000")

        txtRange12From_WET.Text = Format(Val(txtRange11To_WET.Text) + Val(txtRange1From_WET.Text), "0.000")
        txtRange12To_WET.Text = Format(Val(txtRange11To_WET.Text) + diff, "0.000")

        txtRange13From_WET.Text = Format(Val(txtRange12To_WET.Text) + Val(txtRange1From_WET.Text), "0.000")
        txtRange13To_WET.Text = Format(Val(txtRange12To_WET.Text) + diff, "0.000")

        txtRange14From_WET.Text = Format(Val(txtRange13To_WET.Text) + Val(txtRange1From_WET.Text), "0.000")
        txtRange14To_WET.Text = Format(Val(txtRange13To_WET.Text) + diff, "0.000")

        txtRange15From_WET.Text = Format(Val(txtRange14To_WET.Text) + Val(txtRange1From_WET.Text), "0.000")
        txtRange15To_WET.Text = Format(Val(txtRange14To_WET.Text) + diff, "0.000")

        txtRange16From_WET.Text = Format(Val(txtRange15To_WET.Text) + Val(txtRange1From_WET.Text), "0.000")
        txtRange16To_WET.Text = Format(Val(txtRange15To_WET.Text) + diff, "0.000")
    End Function

    Function funcHeader1() As String
        strSql = " Select ''PARTICULAR"
        strSql += " ,'' 'PCS1~GRSWT1'"
        strSql += " ,'' 'PCS2~GRSWT2'"
        strSql += " ,'' 'PCS3~GRSWT3'"
        strSql += " ,'' 'PCS4~GRSWT4'"
        strSql += " ,'' 'PCS5~GRSWT5'"
        strSql += " ,'' 'PCS6~GRSWT6'"
        strSql += " ,'' 'PCS7~GRSWT7'"
        strSql += " ,'' 'PCS8~GRSWT8'"
        strSql += " ,'' 'PCS9~GRSWT9'"
        strSql += " ,'' 'PCS10~GRSWT10'"
        strSql += " ,'' 'PCS11~GRSWT11'"
        strSql += " ,'' 'PCS12~GRSWT12'"
        strSql += " ,'' 'PCS13~GRSWT13'"
        strSql += " ,'' 'PCS14~GRSWT14'"
        strSql += " ,'' 'PCS15~GRSWT15'"
        strSql += " ,'' 'PCS16~GRSWT16'"
        strSql += " ,'SCROLL'SCROLL WHERE 1=2"
        Return strSql
    End Function

    Function funcVerticalStyle(ByVal rangeFrom As Double, ByVal rangeTo As Double, ByVal Result As Integer, Optional ByVal rangefilter As String = Nothing, Optional ByVal outofrange As Integer = 0, Optional ByVal rangeCaption As String = Nothing) As String
        Dim str As String = Nothing
        str = " SELECT " & IIf(rbtRangeWise.Checked, "CONVERT(VARCHAR,' ')DESIGNERID", "T.DESIGNERID") & ""
        str += vbCrLf + ",(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID)AS ITEM "
        str += vbCrLf + "," & IIf(rbtRangeWise.Checked, "CONVERT(VARCHAR,' ')as DESIGNER", "ISNULL(D.DESIGNERNAME,'')AS DESIGNER") & "," & Result & " RESULT"
        str += vbCrLf + ",'" & IIf(outofrange <> 0, "OutOfRange", IIf(rangeCaption <> Nothing, rangeCaption, rangeFrom & "  -  " & rangeTo)) & "' AS RANGE,SUM(PCS)AS PCS"
        str += vbCrLf + ",SUM(GRSWT)AS WEIGHT,' 'COLHEAD FROM " & cnAdminDb & "..ITEMTAG AS T "
        If Not rbtRangeWise.Checked Then str += " LEFT JOIN " & cnAdminDb & "..DESIGNER AS D ON  ISNULL(T.DESIGNERID,'') = ISNULL(D.DESIGNERID,'')"
        str += vbCrLf + " WHERE RECDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND (ISSDATE IS NULL OR ISSDATE > '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "')    "
        If outofrange = 0 Then
            str += vbCrLf + " AND GRSWT BETWEEN " & rangeFrom & "  AND " & rangeTo & " "
        ElseIf outofrange = 1 Then
            str += vbCrLf + " AND GRSWT NOT BETWEEN " & Val(txtRange1From_WET.Text.ToString) & "  AND " & Val(txtRange1To_WET.Text.ToString) & " "
            str += vbCrLf + " AND GRSWT NOT BETWEEN " & Val(txtRange2From_WET.Text.ToString) & "  AND " & Val(txtRange2To_WET.Text.ToString) & " "
            str += vbCrLf + " AND GRSWT NOT BETWEEN " & Val(txtRange3From_WET.Text.ToString) & "  AND " & Val(txtRange3To_WET.Text.ToString) & " "
            str += vbCrLf + " AND GRSWT NOT BETWEEN " & Val(txtRange4From_WET.Text.ToString) & "  AND " & Val(txtRange4To_WET.Text.ToString) & " "
            str += vbCrLf + " AND GRSWT NOT BETWEEN " & Val(txtRange5From_WET.Text.ToString) & "  AND " & Val(txtRange5To_WET.Text.ToString) & " "
            str += vbCrLf + " AND GRSWT NOT BETWEEN " & Val(txtRange6From_WET.Text.ToString) & "  AND " & Val(txtRange6To_WET.Text.ToString) & " "
            str += vbCrLf + " AND GRSWT NOT BETWEEN " & Val(txtRange7From_WET.Text.ToString) & "  AND " & Val(txtRange7To_WET.Text.ToString) & " "
            str += vbCrLf + " AND GRSWT NOT BETWEEN " & Val(txtRange8From_WET.Text.ToString) & "  AND " & Val(txtRange8To_WET.Text.ToString) & " "
            str += vbCrLf + " AND GRSWT NOT BETWEEN " & Val(txtRange9From_WET.Text.ToString) & "  AND " & Val(txtRange9To_WET.Text.ToString) & " "

            str += vbCrLf + " AND GRSWT NOT BETWEEN " & Val(txtRange10From_WET.Text.ToString) & "  AND " & Val(txtRange10To_WET.Text.ToString) & " "
            str += vbCrLf + " AND GRSWT NOT BETWEEN " & Val(txtRange11From_WET.Text.ToString) & "  AND " & Val(txtRange11To_WET.Text.ToString) & " "
            str += vbCrLf + " AND GRSWT NOT BETWEEN " & Val(txtRange12From_WET.Text.ToString) & "  AND " & Val(txtRange12To_WET.Text.ToString) & " "
            str += vbCrLf + " AND GRSWT NOT BETWEEN " & Val(txtRange13From_WET.Text.ToString) & "  AND " & Val(txtRange13To_WET.Text.ToString) & " "
            str += vbCrLf + " AND GRSWT NOT BETWEEN " & Val(txtRange14From_WET.Text.ToString) & "  AND " & Val(txtRange14To_WET.Text.ToString) & " "
            str += vbCrLf + " AND GRSWT NOT BETWEEN " & Val(txtRange15From_WET.Text.ToString) & "  AND " & Val(txtRange15To_WET.Text.ToString) & " "
            str += vbCrLf + " AND GRSWT NOT BETWEEN " & Val(txtRange16From_WET.Text.ToString) & "  AND " & Val(txtRange16To_WET.Text.ToString) & " "
        ElseIf outofrange = 2 Then
            str += vbCrLf + rangefilter
        End If
        If Not cnCentStock Then str += " AND COMPANYID = '" & GetStockCompId() & "'"
        If cmbCostCentre.Text <> "ALL" And cmbCostCentre.Text <> "" Then
            str += vbCrLf + " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "'),'')"
        End If
        If cmbCounter.Text <> "ALL" And cmbCounter.Text <> "" Then
            str += vbCrLf + " AND ISNULL(ITEMCTRID,0) = ISNULL((SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbCounter.Text & "'),'')"
        End If
        If cmbDesigner.Text <> "ALL" And cmbDesigner.Text <> "" Then
            str += vbCrLf + " AND " & IIf(rbtRangeWise.Checked, "ISNULL(DESIGNERID,0)", "ISNULL(D.DESIGNERID,0)") & " = ISNULL((SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesigner.Text & "'),0)"
        End If
        If cmbItemName.Text <> "ALL" And cmbItemName.Text <> "" Then
            str += vbCrLf + " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName.Text & "')"
        End If
        If cmbSubItem.Text <> "ALL" And cmbSubItem.Text <> "" Then
            str += vbCrLf + " AND SUBITEMID = (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName.Text & "'))"
        End If
        If Not rbtRangeWise.Checked Then str += " GROUP BY T.DESIGNERID,ITEMID,DESIGNERNAME" Else str += " GROUP BY ITEMID"
        Return str
    End Function

    Function funcFindMaxMin() As Integer
        If txtRange1To_WET.Text > max Then max = txtRange1To_WET.Text
        If txtRange2To_WET.Text > max Then max = txtRange2To_WET.Text
        If txtRange3To_WET.Text > max Then max = txtRange3To_WET.Text
        If txtRange4To_WET.Text > max Then max = txtRange4To_WET.Text
        If txtRange5To_WET.Text > max Then max = txtRange5To_WET.Text
        If txtRange6To_WET.Text > max Then max = txtRange6To_WET.Text
        If txtRange7To_WET.Text > max Then max = txtRange7To_WET.Text
        If txtRange8To_WET.Text > max Then max = txtRange8To_WET.Text
        If txtRange9To_WET.Text > max Then max = txtRange9To_WET.Text
        If txtRange10To_WET.Text > max Then max = txtRange10To_WET.Text
        If txtRange11To_WET.Text > max Then max = txtRange11To_WET.Text
        If txtRange12To_WET.Text > max Then max = txtRange12To_WET.Text
        If txtRange13To_WET.Text > max Then max = txtRange13To_WET.Text
        If txtRange14To_WET.Text > max Then max = txtRange14To_WET.Text
        If txtRange15To_WET.Text > max Then max = txtRange15To_WET.Text
        If txtRange16To_WET.Text > max Then max = txtRange16To_WET.Text
    End Function

    Function funcVerticalStyle(ByVal Result As Integer) As String
        funcFindMaxMin()
        Dim str As String = Nothing
        If rbtRangeWise.Checked = True Then
            str = " Select ''DesignerId,''Designer,"
            str += " '" & Result & "' Result,'' as Range,sum(isnull(Pcs,0))as PCS,"
            str += " sum(isnull(GrsWt,0))as WEIGHT,'G'COLHEAD"
            str += " from " & cnAdminDb & "..ITEMTAG as t"
            str += " where GrsWt Between 0.001  and " & max & ""
        Else
            str = " select designerid,'ZZZZSUB TOTAL'As Designer,"
            str += " '" & Result & "'Result,'' as Range,sum(isnull(Pcs,0))as PCS,"
            str += " sum(isnull(GrsWt,0))as WEIGHT,'S'COLHEAD"
            str += " from " & cnAdminDb & "..ITEMTAG as t"
            str += " where GrsWt Between 0.001  and " & max & ""
            str += " group by DesignerId"
        End If
        Return str
    End Function

    Function funcVerticalReport() As Integer
        strSql = " IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND "
        strSql += " NAME = 'TEMP" & systemId & "RANGEREORD')"
        strSql += " DROP TABLE TEMPTABLEDB..TEMP" & systemId & "RANGEREORD"
        strSql += " CREATE TABLE TEMPTABLEDB..TEMP" & systemId & "RANGEREORD("
        strSql += " PARTICULAR VARCHAR(50),"
        strSql += " ITEM VARCHAR(50),"
        strSql += " RESULT VARCHAR(2),"
        strSql += " RANGE VARCHAR(50),"
        strSql += " PCS INT,"
        strSql += " WEIGHT NUMERIC(15,3),"
        strSql += " COLHEAD VARCHAR(1),"
        strSql += " SNO INT IDENTITY)"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        If chkmaster.Checked = False Then
            strSql = " IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND "
            strSql += " NAME = 'TEMP" & systemId & "RANGE')"
            strSql += " DROP TABLE TEMPTABLEDB..TEMP" & systemId & "RANGE"
            strSql += " Select * INTO TEMPTABLEDB..TEMP" & systemId & "RANGE from ("
            strSql += funcVerticalStyle(Val(txtRange1From_WET.Text), Val(txtRange1To_WET.Text), 1) + " Union All"
            strSql += funcVerticalStyle(Val(txtRange2From_WET.Text), Val(txtRange2To_WET.Text), 2) + " Union All"
            strSql += funcVerticalStyle(Val(txtRange3From_WET.Text), Val(txtRange3To_WET.Text), 3) + " Union All"
            strSql += funcVerticalStyle(Val(txtRange4From_WET.Text), Val(txtRange4To_WET.Text), 4) + " Union All"
            strSql += funcVerticalStyle(Val(txtRange5From_WET.Text), Val(txtRange5To_WET.Text), 5) + " Union All"
            strSql += funcVerticalStyle(Val(txtRange6From_WET.Text), Val(txtRange6To_WET.Text), 6) + " Union All"
            strSql += funcVerticalStyle(Val(txtRange7From_WET.Text), Val(txtRange7To_WET.Text), 7) + " Union All"
            strSql += funcVerticalStyle(Val(txtRange8From_WET.Text), Val(txtRange8To_WET.Text), 8) + " Union All"
            strSql += funcVerticalStyle(Val(txtRange9From_WET.Text), Val(txtRange9To_WET.Text), 9) + " Union All"
            strSql += funcVerticalStyle(Val(txtRange10From_WET.Text), Val(txtRange10To_WET.Text), 10) + " Union All"
            strSql += funcVerticalStyle(Val(txtRange11From_WET.Text), Val(txtRange11To_WET.Text), 11) + " Union All"
            strSql += funcVerticalStyle(Val(txtRange12From_WET.Text), Val(txtRange12To_WET.Text), 12) + " Union All"
            strSql += funcVerticalStyle(Val(txtRange13From_WET.Text), Val(txtRange13To_WET.Text), 13) + " Union All"
            strSql += funcVerticalStyle(Val(txtRange14From_WET.Text), Val(txtRange14To_WET.Text), 14) + " Union All"
            strSql += funcVerticalStyle(Val(txtRange15From_WET.Text), Val(txtRange15To_WET.Text), 15) + " Union All"
            strSql += funcVerticalStyle(Val(txtRange16From_WET.Text), Val(txtRange16To_WET.Text), 16) + " Union All"
            strSql += funcVerticalStyle(0, 0, 17, Nothing, 1)
            strSql += " )as xy order by result"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        Else

            strSql = "SELECT FROMWEIGHT,TOWEIGHT,WEIGHT,DESIGNID FROM " & cnAdminDb & "..STKREORDER WHERE 1=1 AND FROMDAY IS NULL"
            'If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
            If cmbCostCentre.Text <> "ALL" And cmbCostCentre.Text <> "" Then
                strSql += vbCrLf + " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre.Text & "'),'')"
            End If
            'If cmbDesigner.Text <> "ALL" And cmbDesigner.Text <> "" Then
            '    strSql += vbCrLf + " AND ISNULL(DESIGNID,0) = ISNULL((SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesigner.Text & "'),0)"
            'End If
            If cmbItemName.Text <> "ALL" And cmbItemName.Text <> "" Then
                strSql += vbCrLf + " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName.Text & "')"
            End If
            'If cmbSubItem.Text <> "ALL" And cmbSubItem.Text <> "" Then
            '    strSql += vbCrLf + " AND SUBITEMID = (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName.Text & "'))"
            'End If
            da = New OleDbDataAdapter(strSql, cn)
            frdt = New DataTable()
            da.Fill(frdt)
            If frdt.Rows.Count > 0 Then
                Dim rangefilter As String = ""
                Dim rangecaption As String = ""
                strSql = " IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND "
                strSql += " NAME = 'TEMP" & systemId & "RANGE')"
                strSql += " DROP TABLE TEMPTABLEDB..TEMP" & systemId & "RANGE"
                strSql += " Select * INTO TEMPTABLEDB..TEMP" & systemId & "RANGE from ("
                For i As Integer = 0 To frdt.Rows.Count - 1
                    '599
                    rangecaption = frdt.Rows(i).Item("WEIGHT").ToString()
                    strSql += funcVerticalStyle(Val(frdt.Rows(i).Item("FROMWEIGHT").ToString()), Val(frdt.Rows(i).Item("TOWEIGHT").ToString()), i, , , rangecaption)
                    If i <= frdt.Rows.Count - 1 Then strSql += " Union All" : rangefilter += " AND GRSWT NOT BETWEEN " & Val(frdt.Rows(i).Item("FROMWEIGHT").ToString()) & "  AND " & Val(frdt.Rows(i).Item("TOWEIGHT").ToString()) & " "
                    If i = frdt.Rows.Count - 1 Then
                        strSql += funcVerticalStyle(0, 0, i + 1, rangefilter, 2, )
                    End If
                Next
                strSql += " )as xy"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            Else
                MsgBox("Record Not Found", MsgBoxStyle.Exclamation)
                Exit Function
            End If
        End If
        strSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "RANGE)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "RANGE(DESIGNER,RESULT,RANGE,PCS,WEIGHT,COLHEAD)"
        strSql += " SELECT "
        strSql += " CONVERT(VARCHAR,'') PARTICULAR,(SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "RANGE) RESULT,'TOTAL' RANGE,SUM(PCS) PCS,SUM(WEIGHT) WEIGHT,'T' COLHEAD"
        strSql += " FROM TEMPTABLEDB..TEMP" & systemId & "RANGE "
        strSql += " END  "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "RANGE)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "RANGEREORD(PARTICULAR,ITEM,RESULT,RANGE,PCS,WEIGHT,COLHEAD)"
        strSql += " SELECT DESIGNER PARTICULAR,ITEM,RESULT,RANGE,PCS,WEIGHT,COLHEAD "
        strSql += " FROM TEMPTABLEDB..TEMP" & systemId & "RANGE "
        If rbtRangeWise.Checked = True Then
            strSql += " order by convert(int,Result)"
        Else
            strSql += " order by convert(int,Result)"
        End If
        strSql += " END  "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        If rbtRangeWise.Checked Then
            strSql = " SELECT PARTICULAR,RANGE,ITEM,RESULT,SUM(PCS)AS PCS,SUM(WEIGHT)AS WEIGHT,COLHEAD FROM TEMPTABLEDB..TEMP" & systemId & "RANGEREORD "
            strSql += vbCrLf + "GROUP BY PARTICULAR,RANGE,ITEM,RESULT,COLHEAD ORDER BY COLHEAD"
        Else
            '599
            strSql = " SELECT PARTICULAR,ITEM,RANGE,RESULT,SUM(PCS)AS PCS,SUM(WEIGHT)AS WEIGHT,COLHEAD FROM TEMPTABLEDB..TEMP" & systemId & "RANGEREORD "
            strSql += vbCrLf + "GROUP BY PARTICULAR,ITEM,RANGE,RESULT,COLHEAD ORDER BY COLHEAD,PARTICULAR,ITEM,RESULT"
        End If



        'SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "RANGEREORD ORDER BY SNO"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            MsgBox("Record Not Found", MsgBoxStyle.Exclamation)
            Exit Function
        End If
        'Dim index As Integer = 0
        'ReDim verSubTotalIndex(dt.Rows.Count - 1)
        'Dim grandPcs As Integer = 0
        'Dim grandWeight As Double = 0
        'If rbtRangeWise.Checked = False Then
        '    For cnt As Integer = 0 To dt.Rows.Count - 1
        '        If dt.Rows(cnt).Item("PARTICULAR").ToString = "ZZZZSUB TOTAL" Then
        '            dt.Rows(cnt).Item("PARTICULAR") = "SUB TOTAL"
        '            dt.Rows(cnt).Item("COLHEAD") = "S"
        '            verSubTotalIndex(index) = cnt
        '            index += 1
        '            grandPcs += Val(dt.Rows(cnt).Item("Pcs").ToString)
        '            grandWeight += Val(dt.Rows(cnt).Item("Weight").ToString)
        '        End If
        '    Next
        '    Dim ro As DataRow = Nothing
        '    ro = dt.NewRow
        '    ro("PARTICULAR") = "GRAND TOTAL"
        '    ro("COLHEAD") = "G"
        '    ro("Pcs") = grandPcs
        '    ro("Weight") = grandWeight
        '    dt.Rows.Add(ro)
        'Else
        '    dt.Rows(dt.Rows.Count - 1).Item("PARTICULAR") = "GRAND TOTAL"
        'End If
        With gridView
            .DataSource = dt
            tabView.Show()
            GridViewFormat()
            With .Columns("PARTICULAR")
                .HeaderText = "DESIGNER"
                .Width = 150
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            End With
            With .Columns("Result")
                .Visible = False
            End With
            With .Columns("Range")
                .Width = 150
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            End With
            For cnt As Integer = 4 To gridView.Columns.Count - 1
                With .Columns(cnt)
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Width = 70
                End With
            Next
            .ColumnHeadersVisible = True
            .Columns("COLHEAD").Visible = False
            '.Columns("SNO").Visible = False
            gridViewHead.Visible = False
            For Each dgvRow As DataGridViewRow In gridView.Rows
                With dgvRow
                    Select Case .Cells("COLHEAD").Value.ToString
                        Case "T"
                            .Cells("RANGE").Style.BackColor = Color.LightGreen
                            .Cells("RANGE").Style.ForeColor = Color.Red
                            .Cells("PCS").Style.BackColor = Color.Red
                            .Cells("WEIGHT").Style.BackColor = Color.Red
                            .Cells("PCS").Style.ForeColor = Color.White
                            .Cells("WEIGHT").Style.ForeColor = Color.White
                            .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    End Select
                End With
            Next
            tabMain.SelectedTab = tabView
            gridView.Focus()
        End With
    End Function

    Function funcHorizantalReport() As Integer
        Dim header1 As String = funcHeader1()
        Dim flrSql As String = funcFiltration()

        strSql = " IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND "
        strSql += " NAME = 'TEMP" & systemId & "RANGEREORD')"
        strSql += " DROP TABLE TEMP" & systemId & "RANGEREORD"
        strSql += " CREATE TABLE TEMP" & systemId & "RANGEREORD("
        strSql += " PARTICULAR VARCHAR(50),"
        strSql += " RESULT INT,"
        strSql += " PCS1 INT,"
        strSql += " GRSWT1 NUMERIC(15,3),"
        strSql += " PCS2 INT,"
        strSql += " GRSWT2 NUMERIC(15,3),"
        strSql += " PCS3 INT,"
        strSql += " GRSWT3 NUMERIC(15,3),"
        strSql += " PCS4 INT,"
        strSql += " GRSWT4 NUMERIC(15,3),"
        strSql += " PCS5 INT,"
        strSql += " GRSWT5 NUMERIC(15,3),"
        strSql += " PCS6 INT,"
        strSql += " GRSWT6 NUMERIC(15,3),"
        strSql += " PCS7 INT,"
        strSql += " GRSWT7 NUMERIC(15,3),"
        strSql += " PCS8 INT,"
        strSql += " GRSWT8 NUMERIC(15,3),"
        strSql += " PCS9 INT,"
        strSql += " GRSWT9 NUMERIC(15,3),"
        strSql += " PCS10 INT,"
        strSql += " GRSWT10 NUMERIC(15,3),"
        strSql += " PCS11 INT,"
        strSql += " GRSWT11 NUMERIC(15,3),"
        strSql += " PCS12 INT,"
        strSql += " GRSWT12 NUMERIC(15,3),"
        strSql += " PCS13 INT,"
        strSql += " GRSWT13 NUMERIC(15,3),"
        strSql += " PCS14 INT,"
        strSql += " GRSWT14 NUMERIC(15,3),"
        strSql += " PCS15 INT,"
        strSql += " GRSWT15 NUMERIC(15,3),"
        strSql += " PCS16 INT,"
        strSql += " GRSWT16 NUMERIC(15,3),"
        strSql += " COLHEAD VARCHAR(1),"
        strSql += " SNO INT IDENTITY)"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND "
        strSql += " NAME = 'TEMP" & systemId & "RANGE')"
        strSql += " DROP TABLE TEMP" & systemId & "RANGE"
        strSql += " Select "
        strSql += " Designer PARTICULAR"
        strSql += " ,Result"
        strSql += " ,r1Pcs,r1GrsWt"
        strSql += " ,r2Pcs,r2GrsWt"
        strSql += " ,r3Pcs,r3GrsWt"
        strSql += " ,r4Pcs,r4GrsWt"
        strSql += " ,r5Pcs,r5GrsWt"
        strSql += " ,r6Pcs,r6GrsWt"
        strSql += " ,r7Pcs,r7GrsWt"
        strSql += " ,r8Pcs,r8GrsWt"
        strSql += " ,r9Pcs,r9GrsWt"
        strSql += " ,r10Pcs,r10GrsWt"
        strSql += " ,r11Pcs,r11GrsWt"
        strSql += " ,r12Pcs,r12GrsWt"
        strSql += " ,r13Pcs,r13GrsWt"
        strSql += " ,r14Pcs,r14GrsWt"
        strSql += " ,r15Pcs,r15GrsWt"
        strSql += " ,r16Pcs,r16GrsWt,COLHEAD"
        strSql += " INTO  TEMP" & systemId & "RANGE from ("
        strSql += " select"
        strSql += " distinct"
        If rbtDesignerWise.Checked = True Then
            strSql += " (select designerName from " & cnAdminDb & "..Designer "
            strSql += " where DesignerId = t.DesignerId)as Designer"
        Else
            strSql += " ''Designer"
        End If
        strSql += " ,'1'Result"
        strSql += " ,sum(case when Pcs between " & Val(txtRange1From_WET.Text) & " and " & Val(txtRange1To_WET.Text) & " then pcs else 0 end)as r1Pcs"
        strSql += " ,sum(case when GrsWt between " & Val(txtRange1From_WET.Text) & " and " & Val(txtRange1To_WET.Text) & " then pcs else 0 end)as r1GrsWt"
        strSql += " ,sum(case when Pcs between " & Val(txtRange2From_WET.Text) & " and " & Val(txtRange2To_WET.Text) & " then pcs else 0 end)as r2Pcs"
        strSql += " ,sum(case when GrsWt between " & Val(txtRange2From_WET.Text) & " and " & Val(txtRange2To_WET.Text) & " then pcs else 0 end)as r2GrsWt"
        strSql += " ,sum(case when Pcs between " & Val(txtRange3From_WET.Text) & " and " & Val(txtRange3To_WET.Text) & " then pcs else 0 end)as r3Pcs"
        strSql += " ,sum(case when GrsWt between " & Val(txtRange3From_WET.Text) & " and " & Val(txtRange3To_WET.Text) & " then pcs else 0 end)as r3GrsWt"
        strSql += " ,sum(case when Pcs between " & Val(txtRange4From_WET.Text) & " and " & Val(txtRange4To_WET.Text) & " then pcs else 0 end)as r4Pcs"
        strSql += " ,sum(case when GrsWt between " & Val(txtRange4From_WET.Text) & " and " & Val(txtRange4To_WET.Text) & " then pcs else 0 end)as r4GrsWt"
        strSql += " ,sum(case when Pcs between " & Val(txtRange5From_WET.Text) & " and " & Val(txtRange5To_WET.Text) & " then pcs else 0 end)as r5Pcs"
        strSql += " ,sum(case when GrsWt between " & Val(txtRange5From_WET.Text) & " and " & Val(txtRange5To_WET.Text) & " then pcs else 0 end)as r5GrsWt"
        strSql += " ,sum(case when Pcs between " & Val(txtRange6From_WET.Text) & " and " & Val(txtRange6To_WET.Text) & " then pcs else 0 end)as r6Pcs"
        strSql += " ,sum(case when GrsWt between " & Val(txtRange6From_WET.Text) & " and " & Val(txtRange6To_WET.Text) & " then pcs else 0 end)as r6GrsWt"
        strSql += " ,sum(case when Pcs between " & Val(txtRange7From_WET.Text) & " and " & Val(txtRange7To_WET.Text) & " then pcs else 0 end)as r7Pcs"
        strSql += " ,sum(case when GrsWt between " & Val(txtRange7From_WET.Text) & " and " & Val(txtRange7To_WET.Text) & " then pcs else 0 end)as r7GrsWt"
        strSql += " ,sum(case when Pcs between " & Val(txtRange8From_WET.Text) & " and " & Val(txtRange8To_WET.Text) & " then pcs else 0 end)as r8Pcs"
        strSql += " ,sum(case when GrsWt between " & Val(txtRange8From_WET.Text) & " and " & Val(txtRange8To_WET.Text) & " then pcs else 0 end)as r8GrsWt"
        strSql += " ,sum(case when Pcs between " & Val(txtRange9From_WET.Text) & " and " & Val(txtRange9To_WET.Text) & " then pcs else 0 end)as r9Pcs"
        strSql += " ,sum(case when GrsWt between " & Val(txtRange9From_WET.Text) & " and " & Val(txtRange9To_WET.Text) & " then pcs else 0 end)as r9GrsWt"
        strSql += " ,sum(case when Pcs between " & Val(txtRange10From_WET.Text) & " and " & Val(txtRange10To_WET.Text) & " then pcs else 0 end)as r10Pcs"
        strSql += " ,sum(case when GrsWt between " & Val(txtRange10From_WET.Text) & " and " & Val(txtRange10To_WET.Text) & " then pcs else 0 end)as r10GrsWt"
        strSql += " ,sum(case when Pcs between " & Val(txtRange11From_WET.Text) & " and " & Val(txtRange11To_WET.Text) & " then pcs else 0 end)as r11Pcs"
        strSql += " ,sum(case when GrsWt between " & Val(txtRange11From_WET.Text) & " and " & Val(txtRange11To_WET.Text) & " then pcs else 0 end)as r11GrsWt"
        strSql += " ,sum(case when Pcs between " & Val(txtRange11To_WET.Text) & " and " & Val(txtRange12To_WET.Text) & " then pcs else 0 end)as r12Pcs"
        strSql += " ,sum(case when GrsWt between " & Val(txtRange11To_WET.Text) & " and " & Val(txtRange12To_WET.Text) & " then pcs else 0 end)as r12GrsWt"
        strSql += " ,sum(case when Pcs between " & Val(txtRange13From_WET.Text) & " and " & Val(txtRange13To_WET.Text) & " then pcs else 0 end)as r13Pcs"
        strSql += " ,sum(case when GrsWt between " & Val(txtRange13From_WET.Text) & " and " & Val(txtRange13To_WET.Text) & " then pcs else 0 end)as r13GrsWt"
        strSql += " ,sum(case when Pcs between " & Val(txtRange14From_WET.Text) & " and " & Val(txtRange14To_WET.Text) & " then pcs else 0 end)as r14Pcs"
        strSql += " ,sum(case when GrsWt between " & Val(txtRange14From_WET.Text) & " and " & Val(txtRange14To_WET.Text) & " then pcs else 0 end)as r14GrsWt"
        strSql += " ,sum(case when Pcs between " & Val(txtRange15From_WET.Text) & " and " & Val(txtRange15To_WET.Text) & " then pcs else 0 end)as r15Pcs"
        strSql += " ,sum(case when GrsWt between " & Val(txtRange15From_WET.Text) & " and " & Val(txtRange15To_WET.Text) & " then pcs else 0 end)as r15GrsWt"
        strSql += " ,sum(case when Pcs between " & Val(txtRange16From_WET.Text) & " and " & Val(txtRange16To_WET.Text) & " then pcs else 0 end)as r16Pcs"
        strSql += " ,sum(case when GrsWt between " & Val(txtRange16From_WET.Text) & " and " & Val(txtRange16To_WET.Text) & " then pcs else 0 end)as r16GrsWt"
        strSql += " ,CONVERT(VARCHAR(1),' ')COLHEAD"
        strSql += " from " & cnAdminDb & "..ITEMTAG as t"
        strSql += flrSql
        If rbtDesignerWise.Checked = True Then
            strSql += " group by "
            strSql += " designerid"
        End If
        ''-------------------------Grand Total------------------------------
        strSql += " Union All"
        strSql += " select"
        strSql += " distinct"
        strSql += " 'Grand Total'Designer"
        strSql += " ,'2'Result"
        strSql += " ,sum(case when Pcs between " & Val(txtRange1From_WET.Text) & " and " & Val(txtRange1To_WET.Text) & " then pcs else 0 end)as r1Pcs"
        strSql += " ,sum(case when GrsWt between " & Val(txtRange1From_WET.Text) & " and " & Val(txtRange1To_WET.Text) & " then pcs else 0 end)as r1GrsWt"
        strSql += " ,sum(case when Pcs between " & Val(txtRange2From_WET.Text) & " and " & Val(txtRange2To_WET.Text) & " then pcs else 0 end)as r2Pcs"
        strSql += " ,sum(case when GrsWt between " & Val(txtRange2From_WET.Text) & " and " & Val(txtRange2To_WET.Text) & " then pcs else 0 end)as r2GrsWt"
        strSql += " ,sum(case when Pcs between " & Val(txtRange3From_WET.Text) & " and " & Val(txtRange3To_WET.Text) & " then pcs else 0 end)as r3Pcs"
        strSql += " ,sum(case when GrsWt between " & Val(txtRange3From_WET.Text) & " and " & Val(txtRange3To_WET.Text) & " then pcs else 0 end)as r3GrsWt"
        strSql += " ,sum(case when Pcs between " & Val(txtRange4From_WET.Text) & " and " & Val(txtRange4To_WET.Text) & " then pcs else 0 end)as r4Pcs"
        strSql += " ,sum(case when GrsWt between " & Val(txtRange4From_WET.Text) & " and " & Val(txtRange4To_WET.Text) & " then pcs else 0 end)as r4GrsWt"
        strSql += " ,sum(case when Pcs between " & Val(txtRange5From_WET.Text) & " and " & Val(txtRange5To_WET.Text) & " then pcs else 0 end)as r5Pcs"
        strSql += " ,sum(case when GrsWt between " & Val(txtRange5From_WET.Text) & " and " & Val(txtRange5To_WET.Text) & " then pcs else 0 end)as r5GrsWt"
        strSql += " ,sum(case when Pcs between " & Val(txtRange6From_WET.Text) & " and " & Val(txtRange6To_WET.Text) & " then pcs else 0 end)as r6Pcs"
        strSql += " ,sum(case when GrsWt between " & Val(txtRange6From_WET.Text) & " and " & Val(txtRange6To_WET.Text) & " then pcs else 0 end)as r6GrsWt"
        strSql += " ,sum(case when Pcs between " & Val(txtRange7From_WET.Text) & " and " & Val(txtRange7To_WET.Text) & " then pcs else 0 end)as r7Pcs"
        strSql += " ,sum(case when GrsWt between " & Val(txtRange7From_WET.Text) & " and " & Val(txtRange7To_WET.Text) & " then pcs else 0 end)as r7GrsWt"
        strSql += " ,sum(case when Pcs between " & Val(txtRange8From_WET.Text) & " and " & Val(txtRange8To_WET.Text) & " then pcs else 0 end)as r8Pcs"
        strSql += " ,sum(case when GrsWt between " & Val(txtRange8From_WET.Text) & " and " & Val(txtRange8To_WET.Text) & " then pcs else 0 end)as r8GrsWt"
        strSql += " ,sum(case when Pcs between " & Val(txtRange9From_WET.Text) & " and " & Val(txtRange9To_WET.Text) & " then pcs else 0 end)as r9Pcs"
        strSql += " ,sum(case when GrsWt between " & Val(txtRange9From_WET.Text) & " and " & Val(txtRange9To_WET.Text) & " then pcs else 0 end)as r9GrsWt"
        strSql += " ,sum(case when Pcs between " & Val(txtRange10From_WET.Text) & " and " & Val(txtRange10To_WET.Text) & " then pcs else 0 end)as r10Pcs"
        strSql += " ,sum(case when GrsWt between " & Val(txtRange10From_WET.Text) & " and " & Val(txtRange10To_WET.Text) & " then pcs else 0 end)as r10GrsWt"
        strSql += " ,sum(case when Pcs between " & Val(txtRange11From_WET.Text) & " and " & Val(txtRange11To_WET.Text) & " then pcs else 0 end)as r11Pcs"
        strSql += " ,sum(case when GrsWt between " & Val(txtRange11From_WET.Text) & " and " & Val(txtRange11To_WET.Text) & " then pcs else 0 end)as r11GrsWt"
        strSql += " ,sum(case when Pcs between " & Val(txtRange11To_WET.Text) & " and " & Val(txtRange12To_WET.Text) & " then pcs else 0 end)as r12Pcs"
        strSql += " ,sum(case when GrsWt between " & Val(txtRange11To_WET.Text) & " and " & Val(txtRange12To_WET.Text) & " then pcs else 0 end)as r12GrsWt"
        strSql += " ,sum(case when Pcs between " & Val(txtRange13From_WET.Text) & " and " & Val(txtRange13To_WET.Text) & " then pcs else 0 end)as r13Pcs"
        strSql += " ,sum(case when GrsWt between " & Val(txtRange13From_WET.Text) & " and " & Val(txtRange13To_WET.Text) & " then pcs else 0 end)as r13GrsWt"
        strSql += " ,sum(case when Pcs between " & Val(txtRange14From_WET.Text) & " and " & Val(txtRange14To_WET.Text) & " then pcs else 0 end)as r14Pcs"
        strSql += " ,sum(case when GrsWt between " & Val(txtRange14From_WET.Text) & " and " & Val(txtRange14To_WET.Text) & " then pcs else 0 end)as r14GrsWt"
        strSql += " ,sum(case when Pcs between " & Val(txtRange15From_WET.Text) & " and " & Val(txtRange15To_WET.Text) & " then pcs else 0 end)as r15Pcs"
        strSql += " ,sum(case when GrsWt between " & Val(txtRange15From_WET.Text) & " and " & Val(txtRange15To_WET.Text) & " then pcs else 0 end)as r15GrsWt"
        strSql += " ,sum(case when Pcs between " & Val(txtRange16From_WET.Text) & " and " & Val(txtRange16To_WET.Text) & " then pcs else 0 end)as r16Pcs"
        strSql += " ,sum(case when GrsWt between " & Val(txtRange16From_WET.Text) & " and " & Val(txtRange16To_WET.Text) & " then pcs else 0 end)as r16GrsWt"
        strSql += " ,CONVERT(VARCHAR(1),'G')COLHEAD"
        strSql += " from " & cnAdminDb & "..ITEMTAG as t"
        strSql += flrSql
        strSql += " )XY"
        strSql += " where not (r1Pcs = 0 and r1GrsWt = 0 "
        strSql += " and r2Pcs = 0 and r2GrsWt = 0 "
        strSql += " and r3Pcs = 0 and r3GrsWt = 0 "
        strSql += " and r4Pcs = 0 and r4GrsWt = 0 "
        strSql += " and r5Pcs = 0 and r5GrsWt = 0 "
        strSql += " and r6Pcs = 0 and r6GrsWt = 0 "
        strSql += " and r7Pcs = 0 and r7GrsWt = 0 "
        strSql += " and r8Pcs = 0 and r8GrsWt = 0 "
        strSql += " and r9Pcs = 0 and r9GrsWt = 0 "
        strSql += " and r10Pcs = 0 and r10GrsWt = 0 "
        strSql += " and r11Pcs = 0 and r11GrsWt = 0 "
        strSql += " and r12Pcs = 0 and r12GrsWt = 0 "
        strSql += " and r13Pcs = 0 and r13GrsWt = 0 "
        strSql += " and r14Pcs = 0 and r14GrsWt = 0 "
        strSql += " and r15Pcs = 0 and r15GrsWt = 0 "
        strSql += " and r16Pcs = 0 and r16GrsWt = 0 )"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "RANGE)>0 "
        strSql += " BEGIN "
        strSql += " INSERT INTO TEMP" & systemId & "RANGEREORD(PARTICULAR,RESULT,PCS1,GRSWT1,"
        strSql += " PCS2,GRSWT2,PCS3,GRSWT3,PCS4,GRSWT4,PCS5,GRSWT5,PCS6,GRSWT6,PCS7,GRSWT7,"
        strSql += " PCS8,GRSWT8,PCS9,GRSWT9,PCS10,GRSWT10,PCS11,GRSWT11,PCS12,GRSWT12,PCS13,"
        strSql += " GRSWT13, PCS14, GRSWT14, PCS15, GRSWT15, PCS16, GRSWT16, COLHEAD)"
        strSql += " SELECT PARTICULAR,RESULT,r1Pcs,r1GrsWt,"
        strSql += " r2Pcs,r2GrsWt,r3Pcs,r3GrsWt,r4Pcs,r4GrsWt,r5Pcs,r5GrsWt,r6Pcs,r6GrsWt,"
        strSql += " r7Pcs,r7GrsWt,r8Pcs,r8GrsWt,r9Pcs,r9GrsWt,r10Pcs,r10GrsWt,r11Pcs,r11GrsWt,"
        strSql += " r12Pcs,r12GrsWt,r13Pcs,r13GrsWt,r14Pcs,r14GrsWt,r15Pcs,r15GrsWt,r16Pcs,r16GrsWt,"
        strSql += " COLHEAD FROM TEMP" & systemId & "RANGE ORDER BY RESULT"
        strSql += " END  "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " SELECT * FROM TEMP" & systemId & "RANGEREORD ORDER BY SNO"

        dt = New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            MsgBox("Record Not Found", MsgBoxStyle.Exclamation)
            Exit Function
        End If
        gridView.DataSource = dt
        tabView.Show()
        GridViewFormat()
        With gridView
            .Columns("COLHEAD").Visible = False
            .Columns("SNO").Visible = False

            With .Columns("PARTICULAR")
                .HeaderText = ""
                .Width = 150
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            End With
            With .Columns("Result")
                .Visible = False
            End With
            For cnt As Integer = 2 To gridView.Columns.Count - 1
                gridView.Columns(cnt).Width = 70
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            Next

            Dim dt1 As New DataTable
            da = New OleDbDataAdapter(header1, cn)
            da.Fill(dt1)
            gridViewHead.DataSource = dt1
            funcGridViewHeadWidth()
            With gridViewHead
                .Columns("PARTICULAR").HeaderText = "PARTICULAR"
                .Columns(1).HeaderText = txtRange1From_WET.Text & " - " & txtRange1To_WET.Text
                .Columns(2).HeaderText = txtRange2From_WET.Text & " - " & txtRange2To_WET.Text
                .Columns(3).HeaderText = txtRange3From_WET.Text & " - " & txtRange3To_WET.Text
                .Columns(4).HeaderText = txtRange4From_WET.Text & " - " & txtRange4To_WET.Text
                .Columns(5).HeaderText = txtRange5From_WET.Text & " - " & txtRange5To_WET.Text
                .Columns(6).HeaderText = txtRange6From_WET.Text & " - " & txtRange6To_WET.Text
                .Columns(7).HeaderText = txtRange7From_WET.Text & " - " & txtRange7To_WET.Text
                .Columns(8).HeaderText = txtRange8From_WET.Text & " - " & txtRange8To_WET.Text
                .Columns(9).HeaderText = txtRange9From_WET.Text & " - " & txtRange9To_WET.Text
                .Columns(10).HeaderText = txtRange10From_WET.Text & " - " & txtRange10To_WET.Text
                .Columns(11).HeaderText = txtRange11From_WET.Text & " - " & txtRange11To_WET.Text
                .Columns(12).HeaderText = txtRange12From_WET.Text & " - " & txtRange12To_WET.Text
                .Columns(13).HeaderText = txtRange13From_WET.Text & " - " & txtRange13To_WET.Text
                .Columns(14).HeaderText = txtRange14From_WET.Text & " - " & txtRange14To_WET.Text
                .Columns(15).HeaderText = txtRange15From_WET.Text & " - " & txtRange15To_WET.Text
                .Columns(16).HeaderText = txtRange16From_WET.Text & " - " & txtRange16To_WET.Text
                .Columns("SCROLL").HeaderText = "SCROLL"
            End With

            Dim colWid As Integer = 0
            For cnt As Integer = 0 To gridView.ColumnCount - 1
                If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
            Next
            If colWid >= gridView.Width Then
                gridViewHead.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                gridViewHead.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            Else
                gridViewHead.Columns("SCROLL").Visible = False
            End If

            tabMain.SelectedTab = tabView
            gridView.Focus()
        End With
    End Function

    Function funcGridViewHeadWidth() As Integer
        With gridView
            gridViewHead.Columns("SCROLL").HeaderText = ""
            gridViewHead.Columns("PARTICULAR").Width = .Columns("PARTICULAR").Width
            gridViewHead.Columns(1).Width = .Columns("PCS1").Width + .Columns("GRSWT1").Width
            gridViewHead.Columns(2).Width = .Columns("PCS2").Width + .Columns("GRSWT2").Width
            gridViewHead.Columns(3).Width = .Columns("PCS3").Width + .Columns("GRSWT3").Width
            gridViewHead.Columns(4).Width = .Columns("PCS4").Width + .Columns("GRSWT4").Width
            gridViewHead.Columns(5).Width = .Columns("PCS5").Width + .Columns("GRSWT5").Width
            gridViewHead.Columns(6).Width = .Columns("PCS6").Width + .Columns("GRSWT6").Width
            gridViewHead.Columns(7).Width = .Columns("PCS7").Width + .Columns("GRSWT7").Width
            gridViewHead.Columns(8).Width = .Columns("PCS8").Width + .Columns("GRSWT8").Width
            gridViewHead.Columns(9).Width = .Columns("PCS9").Width + .Columns("GRSWT9").Width
            gridViewHead.Columns(10).Width = .Columns("PCS10").Width + .Columns("GRSWT10").Width
            gridViewHead.Columns(11).Width = .Columns("PCS11").Width + .Columns("GRSWT11").Width
            gridViewHead.Columns(12).Width = .Columns("PCS12").Width + .Columns("GRSWT12").Width
            gridViewHead.Columns(13).Width = .Columns("PCS13").Width + .Columns("GRSWT13").Width
            gridViewHead.Columns(14).Width = .Columns("PCS14").Width + .Columns("GRSWT14").Width
            gridViewHead.Columns(15).Width = .Columns("PCS15").Width + .Columns("GRSWT15").Width
            gridViewHead.Columns(16).Width = .Columns("PCS16").Width + .Columns("GRSWT16").Width

            gridViewHead.Columns("SCROLL").Visible = CType(.Controls(0), HScrollBar).Visible
            gridViewHead.Columns("SCROLL").Width = CType(.Controls(1), VScrollBar).Width
        End With
    End Function

    Function funcFiltration() As String
        strSql = " Where 1=1 "
        If Not cnCentStock Then strSql += " AND T.COMPANYID = '" & GetStockCompId() & "'"
        If cmbItemName.Text <> "ALL" Then
            strSql += " and t.itemid  = (select itemId from " & cnAdminDb & "..itemMast "
            strSql += " where itemName = '" & cmbItemName.Text & "')"
        End If
        If cmbSubItem.Text <> "ALL" Then
            strSql += " and t.subItemId = (select subItemid from " & cnAdminDb & "..subItemMast "
            strSql += " where subItemName = '" & cmbSubItem.Text & "')"
        End If
        If cmbDesigner.Text <> "ALL" Then
            strSql += " and t.DesignerId = (Select DesignerId from " & cnAdminDb & "..Designer "
            strSql += " where DesignerName = '" & cmbDesigner.Text & "')"
        End If
        If cmbCostCentre.Enabled = True Then
            If cmbCostCentre.Text <> "ALL" Then
                strSql += " and t.COSTID = (Select CostId from " & cnAdminDb & "..CostCentre "
                strSql += " where CostName = '" & cmbCostCentre.Text & "')"
            End If
        End If
        Return strSql
    End Function

    Private Sub btnView_SearchClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        Try
            'Me.Cursor = Cursors.WaitCursor
            gridView.DataSource = Nothing
            gridViewHead.DataSource = Nothing
            If chkVertical.Checked = True Then
                funcVerticalReport()
            Else
                gridViewHead.Visible = True
                funcHorizantalReport()
                For cnt As Integer = 0 To gridViewHead.ColumnCount - 1
                    gridViewHead.Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
                    gridViewHead.Columns(cnt).Resizable = DataGridViewTriState.False
                Next
                gridView.ScrollBars = ScrollBars.Both
            End If
            If Not CheckBckDays(userId, Me.Name, dtpAsOnDate.Value) Then dtpAsOnDate.Focus() : Exit Sub
            lblTitle.Text = "RANGEWISE RE-ORDER REPORT AS ON " & dtpAsOnDate.Text
            If cmbCounter.Text <> "ALL" And cmbCounter.Text <> "" Then
                lblTitle.Text += vbCrLf + " COUNTER : " & cmbCounter.Text
            End If
        Catch ex As Exception
            MsgBox("ERROR:" + ex.Message + vbCrLf + vbCrLf + "POSITION:" + ex.StackTrace)
        Finally
            Me.Cursor = Cursors.Arrow
        End Try
        Prop_Sets()
    End Sub

    ''Private Sub gridView_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridView.CellFormatting
    ''    With gridView
    ''        Select Case .Rows(e.RowIndex).Cells("COLHEAD").Value.ToString
    ''            Case "G"
    ''                .Rows(e.RowIndex).DefaultCellStyle.BackColor = reportTotalStyle.BackColor
    ''                .Rows(e.RowIndex).DefaultCellStyle.Font = reportTotalStyle.Font
    ''            Case "S"
    ''                .Rows(e.RowIndex).DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
    ''                .Rows(e.RowIndex).DefaultCellStyle.Font = reportSubTotalStyle.Font
    ''        End Select
    ''    End With
    ''End Sub

    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "G"
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                    Case "S"
                        .DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
                        .DefaultCellStyle.Font = reportSubTotalStyle.Font
                End Select
            End With
        Next
    End Function
    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.Escape) Then
            cmbItemName.Focus()
        ElseIf UCase(e.KeyChar) = "X" Then
            Me.btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub cmbItemName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItemName.SelectedIndexChanged
        cmbSubItem.Items.Clear()
        strSql = GetSubItemQry(New String() {"SUBITEMNAME"}, Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName.Text & "'")))
        'strSql = " select subItemName from " & cnAdminDb & "..subItemMast where itemId = "
        'strSql += " (select itemId from " & cnAdminDb & "..itemMast "
        'strSql += " where itemName = '" & cmbItemName.Text & "')"
        'strSql += " order by SubItemName"
        cmbSubItem.Items.Add("ALL")
        objGPack.FillCombo(strSql, cmbSubItem, False)
        cmbSubItem.Text = "ALL"
    End Sub

    Private Sub txtWeightDiff_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWeightDiff_WET.TextChanged
        SetRangeValues()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, gridViewHead)
        End If
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        'rbtDesignerWise.Checked = True
        'txtWeightDiff_NUM.Text = 0
        'txtWeightDiff_NUM.Text = ""
        'chkVertical.Checked = False
        gridView.DataSource = Nothing
        gridViewHead.DataSource = Nothing
        'cmbItemName.Text = "ALL"
        'cmbSubItem.Text = "ALL"
        'cmbDesigner.Text = "ALL"
        cmbCostCentre.Text = IIf(cmbCostCentre.Enabled = True, IIf(cnDefaultCostId, "ALL", cnCostName), "")
        '  cmbCounter.Text = "ALL"
        cmbItemName.Select()
        dtpAsOnDate.Value = GetServerDate()
        Prop_Gets()
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, gridViewHead)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView.ColumnWidthChanged
        With gridViewHead
            If .ColumnCount > 0 Then
                funcGridViewHeadWidth()
            End If
        End With
    End Sub

    Private Sub gridView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView.Scroll
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridViewHead.HorizontalScrollingOffset = e.NewValue
        End If
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridViewHead.HorizontalScrollingOffset = e.NewValue
                gridViewHead.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                gridViewHead.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGen
        cmbItemName.Focus()
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmReorderRangeWiseStock_Properties
        obj.p_cmbItemName = cmbItemName.Text
        obj.p_cmbSubItem = cmbSubItem.Text
        obj.p_cmbDesigner = cmbDesigner.Text
        'obj.p_cmbCostCentre = cmbCostCentre.Text
        obj.p_cmbCounter = cmbCounter.Text
        obj.p_txtWeightDiff_NUM = txtWeightDiff_WET.Text
        obj.p_txtRange1From_WET = txtRange1From_WET.Text
        obj.p_txtRange1To_WET = txtRange1To_WET.Text
        obj.p_txtRange2From_WET = txtRange2From_WET.Text
        obj.p_txtRange2To_WET = txtRange2To_WET.Text
        obj.p_txtRange3From_WET = txtRange3From_WET.Text
        obj.p_txtRange3To_WET = txtRange3To_WET.Text
        obj.p_txtRange4From_WET = txtRange4From_WET.Text
        obj.p_txtRange4To_WET = txtRange4To_WET.Text
        obj.p_txtRange5From_WET = txtRange5From_WET.Text
        obj.p_txtRange5To_WET = txtRange5To_WET.Text
        obj.p_txtRange6From_WET = txtRange6From_WET.Text
        obj.p_txtRange6To_WET = txtRange6To_WET.Text
        obj.p_txtRange7From_WET = txtRange7From_WET.Text
        obj.p_txtRange7To_WET = txtRange7To_WET.Text
        obj.p_txtRange8From_WET = txtRange8From_WET.Text
        obj.p_txtRange8To_WET = txtRange8To_WET.Text
        obj.p_txtRange9From_WET = txtRange9From_WET.Text
        obj.p_txtRange9To_WET = txtRange9To_WET.Text
        obj.p_txtRange10From_WET = txtRange10From_WET.Text
        obj.p_txtRange10To_WET = txtRange10To_WET.Text
        obj.p_txtRange11From_WET = txtRange11From_WET.Text
        obj.p_txtRange11To_WET = txtRange11To_WET.Text
        obj.p_txtRange12From_WET = txtRange12From_WET.Text
        obj.p_txtRange12To_WET = txtRange12To_WET.Text
        obj.p_txtRange13From_WET = txtRange13From_WET.Text
        obj.p_txtRange13To_WET = txtRange13To_WET.Text
        obj.p_txtRange14From_WET = txtRange14From_WET.Text
        obj.p_txtRange14To_WET = txtRange14To_WET.Text
        obj.p_txtRange15From_WET = txtRange15From_WET.Text
        obj.p_txtRange15To_WET = txtRange15To_WET.Text
        obj.p_txtRange16From_WET = txtRange16From_WET.Text
        obj.p_txtRange16To_WET = txtRange16To_WET.Text
        obj.p_rbtDesignerWise = rbtDesignerWise.Checked
        obj.p_rbtRangeWise = rbtRangeWise.Checked
        obj.p_chkVertical = chkVertical.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmReorderRangeWiseStock_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmReorderRangeWiseStock_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmReorderRangeWiseStock_Properties))
        cmbItemName.Text = obj.p_cmbItemName
        cmbItemName.Text = obj.p_cmbItemName
        cmbDesigner.Text = obj.p_cmbDesigner
        'cmbCostCentre.Text = obj.p_cmbCostCentre
        cmbCounter.Text = obj.p_cmbCounter
        txtWeightDiff_WET.Text = obj.p_txtWeightDiff_NUM
        txtRange1From_WET.Text = obj.p_txtRange1From_WET
        txtRange1To_WET.Text = obj.p_txtRange1To_WET
        txtRange2From_WET.Text = obj.p_txtRange2From_WET
        txtRange2To_WET.Text = obj.p_txtRange2From_WET
        txtRange3From_WET.Text = obj.p_txtRange3From_WET
        txtRange3To_WET.Text = obj.p_txtRange3To_WET
        txtRange4From_WET.Text = obj.p_txtRange4From_WET
        txtRange4To_WET.Text = obj.p_txtRange4To_WET
        txtRange5From_WET.Text = obj.p_txtRange5From_WET
        txtRange5To_WET.Text = obj.p_txtRange5To_WET
        txtRange6From_WET.Text = obj.p_txtRange6From_WET
        txtRange6To_WET.Text = obj.p_txtRange6To_WET
        txtRange7From_WET.Text = obj.p_txtRange7From_WET
        txtRange7To_WET.Text = obj.p_txtRange7To_WET
        txtRange8From_WET.Text = obj.p_txtRange8From_WET
        txtRange8To_WET.Text = obj.p_txtRange8To_WET
        txtRange9From_WET.Text = obj.p_txtRange9From_WET
        txtRange9To_WET.Text = obj.p_txtRange9To_WET
        txtRange10From_WET.Text = obj.p_txtRange10From_WET
        txtRange10To_WET.Text = obj.p_txtRange10To_WET
        txtRange11From_WET.Text = obj.p_txtRange11From_WET
        txtRange11To_WET.Text = obj.p_txtRange11To_WET
        txtRange12From_WET.Text = obj.p_txtRange12From_WET
        txtRange12To_WET.Text = obj.p_txtRange12To_WET
        txtRange13From_WET.Text = obj.p_txtRange13From_WET
        txtRange13To_WET.Text = obj.p_txtRange13To_WET
        txtRange14From_WET.Text = obj.p_txtRange14From_WET
        txtRange14To_WET.Text = obj.p_txtRange14To_WET
        txtRange15From_WET.Text = obj.p_txtRange15From_WET
        txtRange15To_WET.Text = obj.p_txtRange15To_WET
        txtRange16From_WET.Text = obj.p_txtRange16From_WET
        txtRange16To_WET.Text = obj.p_txtRange16To_WET
        rbtDesignerWise.Checked = obj.p_rbtDesignerWise
        rbtRangeWise.Checked = obj.p_rbtRangeWise
        chkVertical.Checked = obj.p_chkVertical
    End Sub

    Private Sub chkmaster_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkmaster.CheckedChanged
        If chkmaster.Checked = True Then
            chkVertical.Checked = True
            txtRange1From_WET.ReadOnly = True
            txtRange2From_WET.ReadOnly = True
            txtRange3From_WET.ReadOnly = True
            txtRange4From_WET.ReadOnly = True
            txtRange5From_WET.ReadOnly = True
            txtRange6From_WET.ReadOnly = True
            txtRange7From_WET.ReadOnly = True
            txtRange8From_WET.ReadOnly = True
            txtRange9From_WET.ReadOnly = True
            txtRange10From_WET.ReadOnly = True
            txtRange11From_WET.ReadOnly = True
            txtRange12From_WET.ReadOnly = True
            txtRange13From_WET.ReadOnly = True
            txtRange14From_WET.ReadOnly = True
            txtRange15From_WET.ReadOnly = True
            txtRange16From_WET.ReadOnly = True
            txtRange1To_WET.ReadOnly = True
            txtRange2To_WET.ReadOnly = True
            txtRange3To_WET.ReadOnly = True
            txtRange4To_WET.ReadOnly = True
            txtRange5To_WET.ReadOnly = True
            txtRange6To_WET.ReadOnly = True
            txtRange7To_WET.ReadOnly = True
            txtRange8To_WET.ReadOnly = True
            txtRange9To_WET.ReadOnly = True
            txtRange10To_WET.ReadOnly = True
            txtRange11To_WET.ReadOnly = True
            txtRange12To_WET.ReadOnly = True
            txtRange13To_WET.ReadOnly = True
            txtRange14To_WET.ReadOnly = True
            txtRange15To_WET.ReadOnly = True
            txtRange16To_WET.ReadOnly = True

        Else
            chkVertical.Checked = True
            txtRange1From_WET.ReadOnly = False
            txtRange2From_WET.ReadOnly = False
            txtRange3From_WET.ReadOnly = False
            txtRange4From_WET.ReadOnly = False
            txtRange5From_WET.ReadOnly = False
            txtRange6From_WET.ReadOnly = False
            txtRange7From_WET.ReadOnly = False
            txtRange8From_WET.ReadOnly = False
            txtRange9From_WET.ReadOnly = False
            txtRange10From_WET.ReadOnly = False
            txtRange11From_WET.ReadOnly = False
            txtRange12From_WET.ReadOnly = False
            txtRange13From_WET.ReadOnly = False
            txtRange14From_WET.ReadOnly = False
            txtRange15From_WET.ReadOnly = False
            txtRange16From_WET.ReadOnly = False
            txtRange1To_WET.ReadOnly = False
            txtRange2To_WET.ReadOnly = False
            txtRange3To_WET.ReadOnly = False
            txtRange4To_WET.ReadOnly = False
            txtRange5To_WET.ReadOnly = False
            txtRange6To_WET.ReadOnly = False
            txtRange7To_WET.ReadOnly = False
            txtRange8To_WET.ReadOnly = False
            txtRange9To_WET.ReadOnly = False
            txtRange10To_WET.ReadOnly = False
            txtRange11To_WET.ReadOnly = False
            txtRange12To_WET.ReadOnly = False
            txtRange13To_WET.ReadOnly = False
            txtRange14To_WET.ReadOnly = False
            txtRange15To_WET.ReadOnly = False
            txtRange16To_WET.ReadOnly = False
        End If

    End Sub
End Class

Public Class frmReorderRangeWiseStock_Properties
#Region "General Properties"
    Private cmbItemName As String = "ALL"
    Public Property p_cmbItemName() As String
        Get
            Return cmbItemName
        End Get
        Set(ByVal value As String)
            cmbItemName = value
        End Set
    End Property
    Private cmbSubItem As String = "ALL"
    Public Property p_cmbSubItem() As String
        Get
            Return cmbSubItem
        End Get
        Set(ByVal value As String)
            cmbSubItem = value
        End Set
    End Property
    Private cmbDesigner As String = "ALL"
    Public Property p_cmbDesigner() As String
        Get
            Return cmbDesigner
        End Get
        Set(ByVal value As String)
            cmbDesigner = value
        End Set
    End Property
    Private cmbCostCentre As String = "ALL"
    Public Property p_cmbCostCentre() As String
        Get
            Return cmbCostCentre
        End Get
        Set(ByVal value As String)
            cmbCostCentre = value
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
    Private txtWeightDiff_NUM As String = ""
    Public Property p_txtWeightDiff_NUM() As String
        Get
            Return txtWeightDiff_NUM
        End Get
        Set(ByVal value As String)
            txtWeightDiff_NUM = value
        End Set
    End Property
    Private txtRange1From_WET As String = "0.001"
    Public Property p_txtRange1From_WET() As String
        Get
            Return txtRange1From_WET
        End Get
        Set(ByVal value As String)
            txtRange1From_WET = value
        End Set
    End Property
    Private txtRange1To_WET As String = "0.000"
    Public Property p_txtRange1To_WET() As String
        Get
            Return txtRange1To_WET
        End Get
        Set(ByVal value As String)
            txtRange1To_WET = value
        End Set
    End Property
    Private txtRange2From_WET As String = "0.001"
    Public Property p_txtRange2From_WET() As String
        Get
            Return txtRange2From_WET
        End Get
        Set(ByVal value As String)
            txtRange2From_WET = value
        End Set
    End Property
    Private txtRange2To_WET As String = "0.000"
    Public Property p_txtRange2To_WET() As String
        Get
            Return txtRange2To_WET
        End Get
        Set(ByVal value As String)
            txtRange2To_WET = value
        End Set
    End Property
    Private txtRange3From_WET As String = "0.001"
    Public Property p_txtRange3From_WET() As String
        Get
            Return txtRange3From_WET
        End Get
        Set(ByVal value As String)
            txtRange3From_WET = value
        End Set
    End Property
    Private txtRange3To_WET As String = "0.000"
    Public Property p_txtRange3To_WET() As String
        Get
            Return txtRange3To_WET
        End Get
        Set(ByVal value As String)
            txtRange3To_WET = value
        End Set
    End Property
    Private txtRange4From_WET As String = "0.001"
    Public Property p_txtRange4From_WET() As String
        Get
            Return txtRange4From_WET
        End Get
        Set(ByVal value As String)
            txtRange4From_WET = value
        End Set
    End Property
    Private txtRange4To_WET As String = "0.000"
    Public Property p_txtRange4To_WET() As String
        Get
            Return txtRange4To_WET
        End Get
        Set(ByVal value As String)
            txtRange4To_WET = value
        End Set
    End Property
    Private txtRange5From_WET As String = "0.001"
    Public Property p_txtRange5From_WET() As String
        Get
            Return txtRange5From_WET
        End Get
        Set(ByVal value As String)
            txtRange5From_WET = value
        End Set
    End Property
    Private txtRange5To_WET As String = "0.000"
    Public Property p_txtRange5To_WET() As String
        Get
            Return txtRange5To_WET
        End Get
        Set(ByVal value As String)
            txtRange5To_WET = value
        End Set
    End Property
    Private txtRange6From_WET As String = "0.001"
    Public Property p_txtRange6From_WET() As String
        Get
            Return txtRange6From_WET
        End Get
        Set(ByVal value As String)
            txtRange6From_WET = value
        End Set
    End Property
    Private txtRange6To_WET As String = "0.000"
    Public Property p_txtRange6To_WET() As String
        Get
            Return txtRange6To_WET
        End Get
        Set(ByVal value As String)
            txtRange6To_WET = value
        End Set
    End Property
    Private txtRange7From_WET As String = "0.001"
    Public Property p_txtRange7From_WET() As String
        Get
            Return txtRange7From_WET
        End Get
        Set(ByVal value As String)
            txtRange7From_WET = value
        End Set
    End Property
    Private txtRange7To_WET As String = "0.000"
    Public Property p_txtRange7To_WET() As String
        Get
            Return txtRange7To_WET
        End Get
        Set(ByVal value As String)
            txtRange7To_WET = value
        End Set
    End Property
    Private txtRange8From_WET As String = "0.001"
    Public Property p_txtRange8From_WET() As String
        Get
            Return txtRange8From_WET
        End Get
        Set(ByVal value As String)
            txtRange8From_WET = value
        End Set
    End Property
    Private txtRange8To_WET As String = "0.000"
    Public Property p_txtRange8To_WET() As String
        Get
            Return txtRange8To_WET
        End Get
        Set(ByVal value As String)
            txtRange8To_WET = value
        End Set
    End Property
    Private txtRange9From_WET As String = "0.001"
    Public Property p_txtRange9From_WET() As String
        Get
            Return txtRange9From_WET
        End Get
        Set(ByVal value As String)
            txtRange9From_WET = value
        End Set
    End Property
    Private txtRange9To_WET As String = "0.000"
    Public Property p_txtRange9To_WET() As String
        Get
            Return txtRange9To_WET
        End Get
        Set(ByVal value As String)
            txtRange9To_WET = value
        End Set
    End Property
    Private txtRange10From_WET As String = "0.001"
    Public Property p_txtRange10From_WET() As String
        Get
            Return txtRange10From_WET
        End Get
        Set(ByVal value As String)
            txtRange10From_WET = value
        End Set
    End Property
    Private txtRange10To_WET As String = "0.000"
    Public Property p_txtRange10To_WET() As String
        Get
            Return txtRange10To_WET
        End Get
        Set(ByVal value As String)
            txtRange10To_WET = value
        End Set
    End Property

    Private txtRange11From_WET As String = "0.001"
    Public Property p_txtRange11From_WET() As String
        Get
            Return txtRange11From_WET
        End Get
        Set(ByVal value As String)
            txtRange11From_WET = value
        End Set
    End Property
    Private txtRange11To_WET As String = "0.000"
    Public Property p_txtRange11To_WET() As String
        Get
            Return txtRange11To_WET
        End Get
        Set(ByVal value As String)
            txtRange11To_WET = value
        End Set
    End Property
    Private txtRange12From_WET As String = "0.001"
    Public Property p_txtRange12From_WET() As String
        Get
            Return txtRange12From_WET
        End Get
        Set(ByVal value As String)
            txtRange12From_WET = value
        End Set
    End Property
    Private txtRange12To_WET As String = "0.000"
    Public Property p_txtRange12To_WET() As String
        Get
            Return txtRange12To_WET
        End Get
        Set(ByVal value As String)
            txtRange12To_WET = value
        End Set
    End Property
    Private txtRange13From_WET As String = "0.001"
    Public Property p_txtRange13From_WET() As String
        Get
            Return txtRange13From_WET
        End Get
        Set(ByVal value As String)
            txtRange13From_WET = value
        End Set
    End Property
    Private txtRange13To_WET As String = "0.000"
    Public Property p_txtRange13To_WET() As String
        Get
            Return txtRange13To_WET
        End Get
        Set(ByVal value As String)
            txtRange13To_WET = value
        End Set
    End Property
    Private txtRange14From_WET As String = "0.001"
    Public Property p_txtRange14From_WET() As String
        Get
            Return txtRange14From_WET
        End Get
        Set(ByVal value As String)
            txtRange14From_WET = value
        End Set
    End Property
    Private txtRange14To_WET As String = "0.000"
    Public Property p_txtRange14To_WET() As String
        Get
            Return txtRange14To_WET
        End Get
        Set(ByVal value As String)
            txtRange14To_WET = value
        End Set
    End Property
    Private txtRange15From_WET As String = "0.001"
    Public Property p_txtRange15From_WET() As String
        Get
            Return txtRange15From_WET
        End Get
        Set(ByVal value As String)
            txtRange15From_WET = value
        End Set
    End Property
    Private txtRange15To_WET As String = "0.000"
    Public Property p_txtRange15To_WET() As String
        Get
            Return txtRange15To_WET
        End Get
        Set(ByVal value As String)
            txtRange15To_WET = value
        End Set
    End Property
    Private txtRange16From_WET As String = "0.001"
    Public Property p_txtRange16From_WET() As String
        Get
            Return txtRange16From_WET
        End Get
        Set(ByVal value As String)
            txtRange16From_WET = value
        End Set
    End Property
    Private txtRange16To_WET As String = "0.000"
    Public Property p_txtRange16To_WET() As String
        Get
            Return txtRange16To_WET
        End Get
        Set(ByVal value As String)
            txtRange16To_WET = value
        End Set
    End Property
    Private rbtDesignerWise As Boolean = True
    Public Property p_rbtDesignerWise() As Boolean
        Get
            Return rbtDesignerWise
        End Get
        Set(ByVal value As Boolean)
            rbtDesignerWise = value
        End Set
    End Property
    Private rbtRangeWise As Boolean = False
    Public Property p_rbtRangeWise() As Boolean
        Get
            Return rbtRangeWise
        End Get
        Set(ByVal value As Boolean)
            rbtRangeWise = value
        End Set
    End Property
    Private chkVertical As Boolean = False
    Public Property p_chkVertical() As Boolean
        Get
            Return chkVertical
        End Get
        Set(ByVal value As Boolean)
            chkVertical = value
        End Set
    End Property
#End Region
End Class