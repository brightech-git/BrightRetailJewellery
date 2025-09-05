Imports System.Data.OleDb
Public Class frmTableBaseWastage
    Dim strsql, Type As String
    Dim da As New OleDbDataAdapter
    Dim cmd As New OleDbCommand
    Dim dtmetal, dtitem, dtsubitem, dttabcode, dt, dtcostname, dtCounter As New DataTable

    Private Sub frmTableBaseWastage_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funnew()
    End Sub

#Region "KeyDownEvents"
    Private Sub frmTableBaseWastage_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
#End Region

#Region "ToolStripRegion"
    Private Sub searchToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles searchToolStripMenuItem1.Click

    End Sub

    Private Sub newToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles newToolStripMenuItem2.Click

    End Sub

    Private Sub exitToolStripMenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles exitToolStripMenuItem3.Click
        Me.Close()
    End Sub

    Private Sub exportToolStripMenuItem4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles exportToolStripMenuItem4.Click

    End Sub

    Private Sub printToolStripMenuItem5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles printToolStripMenuItem5.Click

    End Sub
#End Region
#Region "ButtonClickEvents"
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridFlag.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridFlag, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridFlag.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridFlag, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funnew()
    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        Search()
    End Sub
#End Region
#Region "UserDefinedFunction"
    Function funnew()
        'Fillcombo(chkcmbitemname, "Y", 1)
        dtpFrom.Value = GetServerDate()
        Fillcombo(Chkcmbmetal, "", 3)
        Fillcombo(chkcmbitemname, "Y", 1)
        Fillcombo(ChkcmbCostcentre, "", 4)
        Fillcombo(chkCmbItemCounter, "", 5)
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then ChkcmbCostcentre.Enabled = False
        chklsttablecode.Enabled = True
        lblTitle.Visible = False
    End Function
    Public Function GetSelecteditemidD(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean, ByVal field As String _
, ByVal table As String, ByVal filter As String, ByVal dbname As String) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT " & field & " FROM " & dbname & ".." & table & " WHERE " & filter & "= '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "''"
        End If
        Return retStr
    End Function
    
    Function Search()
        Dim Selectedmetals As String = GetSelecteditemidD(Chkcmbmetal, True, "METALID", "METALMAST", "METALNAME", cnAdminDb)
        Dim Selecteditems As String = GetSelecteditemidD(chkcmbitemname, True, "ITEMID", "ITEMMAST", "ITEMNAME", cnAdminDb)
        Dim Selectedsubitems As String = GetSelecteditemidD(chklstsubitem, True, "SUBITEMID", "SUBITEMMAST", "SUBITEMNAME", cnAdminDb)
        Dim SelectedCostids As String = GetSelecteditemidD(ChkcmbCostcentre, True, "COSTID", "COSTCENTRE", "COSTNAME", cnAdminDb)
        Dim SelectedCounterids As String = GetSelecteditemidD(chkCmbItemCounter, True, "ITEMCTRID", "ITEMCOUNTER", "ITEMCTRNAME", cnAdminDb)
        Dim Selectedtabcode As String = ""
        Selectedtabcode = GetChecked_CheckedList(chklsttablecode, True) 'GetSelecteditemidD(chklsttablecode, True, "TABLECODE", "ITEMTAG", "ITEMNAME", cnStockDb)
        strsql = "IF EXISTS(SELECT * FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME='TEMPTAGITEM')  DROP TABLE TEMPTABLEDB..TEMPTAGITEM"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()
        If rbtSummary.Checked Then
            strsql = "SELECT * INTO TEMPTABLEDB..TEMPTAGITEM FROM("
            strsql += vbCrLf + " SELECT '('+CONVERT(VARCHAR(50),T.ITEMID)+') '+ITEMNAME PARTICULAR, I.ITEMNAME ITEM,NULL RECDATE,NULL TAGNO,SUM(T.PCS)PCS,SUM(T.GRSWT)GRSWT,SUM(T.NETWT)NETWT"
            strsql += vbCrLf + ",T.MAXWASTPER,NULL MAXWAST,T.MAXMCGRM,NULL MAXMC"
            strsql += vbCrLf + " ,'R'COLHEAD,'1'RESULT"
            strsql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T "
            strsql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS I ON I.ITEMID = T.ITEMID"
            strsql += vbCrLf + " WHERE T.RECDATE <='" & Format(dtpFrom.Value, "yyyy-MM-dd") & "'"
            strsql += vbCrLf + " AND (T.ISSDATE IS NULL OR T.ISSDATE >'" & Format(dtpFrom.Value, "yyyy-MM-dd") & "') " 'AND ISNULL(APPROVAL,'') = ''"
            If Selectedmetals <> "" And Selectedmetals <> "''" And Selectedmetals <> "'ALL'" Then strsql += vbCrLf + " AND T.ITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID IN (" & Selectedmetals & "))"
            If Selecteditems <> "" And Selecteditems <> "''" And Selecteditems <> "'ALL'" Then strsql += vbCrLf + " AND T.ITEMID IN(" & Selecteditems & ")"
            If Selectedsubitems <> "" And Selectedsubitems <> "''" And Selectedsubitems <> "'ALL'" Then strsql += vbCrLf + " AND  T.SUBITEMID IN(" & Selectedsubitems & ")"
            If Selectedtabcode <> "" And Selectedtabcode <> "''" And Selectedtabcode <> "'ALL'" Then strsql += vbCrLf + " AND T.TABLECODE IN(" & Selectedtabcode & ")"
            If SelectedCostids <> "" And SelectedCostids <> "''" And SelectedCostids <> "'ALL'" Then strsql += vbCrLf + " AND T.COSTID IN(" & SelectedCostids & ")"
            If SelectedCounterids <> "" And SelectedCounterids <> "''" And SelectedCounterids <> "'ALL'" Then strsql += vbCrLf + " AND T.ITEMCTRID IN(" & SelectedCounterids & ")"
            If Not cnCentStock Then strsql += " AND T.COMPANYID = '" & GetStockCompId() & "'"
            'If chkcmbitemname.Text <> "ALL" And chkcmbitemname.Text <> "" Then strsql += vbCrLf + " AND T.ITEMID IN (" & Selecteditems & ")"
            strsql += vbCrLf + " GROUP BY T.ITEMID,I.ITEMNAME,T.MAXWASTPER,T.MAXMCGRM"
            strsql += vbCrLf + ")X"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
            strsql = "INSERT INTO TEMPTABLEDB..TEMPTAGITEM(PARTICULAR,ITEM,RECDATE,PCS,GRSWT,NETWT,MAXWAST,MAXMC,COLHEAD,RESULT)"
            strsql += vbCrLf + " SELECT 'SUBTOTAL'PARTICULAR,ITEM,NULL RECDATE,SUM(PCS),SUM(GRSWT),SUM(NETWT) ,SUM(MAXWAST),SUM(MAXMC),'S',3 "
            strsql += vbCrLf + "  FROM TEMPTABLEDB..TEMPTAGITEM WHERE RESULT=1 GROUP BY ITEM"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
            strsql = "INSERT INTO TEMPTABLEDB..TEMPTAGITEM(PARTICULAR,ITEM,RECDATE,PCS,GRSWT,NETWT,MAXWAST,MAXMCGRM,COLHEAD,RESULT)"
            strsql += vbCrLf + " SELECT 'GRANDTOTAL'PARTICULAR,'ZZZ',NULL,SUM(PCS), SUM(GRSWT),SUM(NETWT),SUM(MAXWAST),SUM(MAXMC),'Z',4 "
            strsql += vbCrLf + "  FROM TEMPTABLEDB..TEMPTAGITEM WHERE RESULT=1"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
        Else
            strsql = "SELECT * INTO TEMPTABLEDB..TEMPTAGITEM FROM("
            strsql += vbCrLf + " SELECT CONVERT(VARCHAR(50),ITEMNAME) PARTICULAR, I.ITEMNAME ITEM,T.RECDATE,T.TAGNO,T.PCS,T.GRSWT,T.NETWT"
            strsql += vbCrLf + ",T.MAXWASTPER,T.MAXWAST,T.MAXMCGRM,T.MAXMC,'R'COLHEAD,'1'RESULT"
            strsql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T "
            strsql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS I ON I.ITEMID = T.ITEMID"
            strsql += vbCrLf + " WHERE T.RECDATE<='" & Format(dtpFrom.Value, "yyyy-MM-dd") & "'"
            strsql += vbCrLf + " AND (T.ISSDATE IS NULL OR T.ISSDATE >'" & Format(dtpFrom.Value, "yyyy-MM-dd") & "') " 'AND ISNULL(APPROVAL,'') = ''"
            If Selecteditems <> "" And Selecteditems <> "''" And Selecteditems <> "'ALL'" Then strsql += vbCrLf + " AND T.ITEMID IN(" & Selecteditems & ")"
            If Selectedsubitems <> "" And Selectedsubitems <> "''" And Selectedsubitems <> "'ALL'" Then strsql += vbCrLf + " AND  T.SUBITEMID IN(" & Selectedsubitems & ")"
            If Selectedtabcode <> "" And Selectedtabcode <> "''" And Selectedtabcode <> "'ALL'" Then strsql += vbCrLf + " AND T.TABLECODE IN(" & Selectedtabcode & ")"
            If SelectedCostids <> "" And SelectedCostids <> "''" And SelectedCostids <> "'ALL'" Then strsql += vbCrLf + " AND T.COSTID IN(" & SelectedCostids & ")"
            If SelectedCounterids <> "" And SelectedCounterids <> "''" And SelectedCounterids <> "'ALL'" Then strsql += vbCrLf + " AND T.ITEMCTRID IN(" & SelectedCounterids & ")"
            If Not cnCentStock Then strsql += " AND T.COMPANYID = '" & GetStockCompId() & "'"
            'If chkcmbitemname.Text <> "ALL" And chkcmbitemname.Text <> "" Then strsql += vbCrLf + " AND T.ITEMID IN (" & Selecteditems & ")"
            strsql += vbCrLf + ")X"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
            strsql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMPTAGITEM(PARTICULAR,RECDATE,COLHEAD,RESULT)"
            strsql += vbCrLf + "SELECT DISTINCT CONVERT(VARCHAR(13),RECDATE, 103)PARTICULAR,RECDATE,'T',0 FROM"
            strsql += vbCrLf + "TEMPTABLEDB..TEMPTAGITEM"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
            strsql = "INSERT INTO TEMPTABLEDB..TEMPTAGITEM(PARTICULAR,RECDATE,GRSWT,NETWT,MAXWAST,MAXMCGRM,MAXMC,COLHEAD,RESULT)"
            strsql += vbCrLf + " SELECT 'SUBTOTAL'PARTICULAR, RECDATE,SUM(GRSWT),SUM(NETWT) ,SUM(MAXWAST),SUM(MAXMCGRM),SUM(MAXMC),'S',3 "
            strsql += vbCrLf + "  FROM TEMPTABLEDB..TEMPTAGITEM WHERE RESULT=1 GROUP BY RECDATE "
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
            strsql = "INSERT INTO TEMPTABLEDB..TEMPTAGITEM(PARTICULAR,RECDATE,GRSWT,NETWT,MAXWAST,MAXMCGRM,MAXMC,COLHEAD,RESULT)"
            strsql += vbCrLf + " SELECT 'GRANDTOTAL'PARTICULAR,'9-12-31', SUM(GRSWT),SUM(NETWT),SUM(MAXWAST),SUM(MAXMCGRM),SUM(MAXMC),'Z',4 "
            strsql += vbCrLf + "  FROM TEMPTABLEDB..TEMPTAGITEM WHERE RESULT=1"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
        End If

        strsql = "SELECT * FROM TEMPTABLEDB..TEMPTAGITEM ORDER BY RECDATE,ITEM,RESULT,COLHEAD,PARTICULAR,MAXWASTPER,MAXMCGRM"
        Dim dtGrid As New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtGrid)
        If dtGrid.Rows.Count > 0 Then
            gridFlag.DataSource = dtGrid
            With gridFlag
                If rbtSummary.Checked Then
                    .Columns("MAXWAST").Visible = False
                    .Columns("MAXMC").Visible = False
                    .Columns("TAGNO").Visible = False
                Else
                    .Columns("MAXWAST").Visible = True
                    .Columns("MAXMC").Visible = True
                    .Columns("TAGNO").Visible = True
                End If
                .Columns("RECDATE").Visible = False
                .Columns("ITEM").Visible = False
                .Columns("RESULT").Visible = False
                .Columns("COLHEAD").Visible = False
                .Columns("PARTICULAR").Width = 200
                .Columns("PCS").Width = 40
                .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight
                .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight
                .Columns("MAXWAST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight
                .Columns("MAXMCGRM").DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight
                .Columns("MAXWASTPER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight
                .Columns("MAXMC").DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight
            End With
            Gridstyle()
            lblTitle.Text = "Tagged Item Wastage Report" & IIf(ChkcmbCostcentre.Text <> "" And ChkcmbCostcentre.Text <> "ALL", " :" & ChkcmbCostcentre.Text, "")
            lblTitle.Visible = True
        Else
            MsgBox("Records Not Found.")
            gridFlag.DataSource = Nothing
        End If

    End Function

    Function Gridstyle()
        For Each ROWS As DataGridViewRow In gridFlag.Rows
            With ROWS
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "T"
                        .Cells("PARTICULAR").Style.BackColor = Color.LightGreen
                        .Cells("PARTICULAR").Style.ForeColor = Color.Red
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "S"
                        .DefaultCellStyle.ForeColor = Color.RoyalBlue
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "Z"
                        .Cells("PARTICULAR").Style.BackColor = Color.LightGreen
                        .DefaultCellStyle.BackColor = Color.LightYellow
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                End Select
            End With

        Next
    End Function

    Function Fillcombo(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal typ As String, ByVal flag As Int32, Optional ByVal filt1 As String = "")
        If flag = 1 Then
            strsql = " SELECT 'ALL' ITEMNAME,'ALL' ITEMID,1 RESULT"
            strsql += " UNION ALL"
            strsql += " SELECT DISTINCT ITEMNAME,CONVERT(vARCHAR,ITEMID),2 RESULT FROM " & cnAdminDb & "..ITEMMAST WHERE TAGTYPE ='" & typ & "'"
            strsql += " AND ITEMID IN(SELECT DISTINCT ITEMID FROM " & cnAdminDb & "..ITEMTAG WHERE ISSDATE IS NULL)"
            If filt1 <> "" And filt1 <> "''" Then strsql += vbCrLf + " AND METALID IN (" & filt1 & ")"
            strsql += " ORDER BY RESULT,ITEMNAME"
            dtitem = New DataTable
            da = New OleDbDataAdapter(strsql, cn)
            da.Fill(dtitem)
            BrighttechPack.GlobalMethods.FillCombo(chkLst, dtitem, "ITEMNAME", True, "ALL")
        ElseIf flag = 2 Then
            strsql = " SELECT 'ALL' SUBITEMNAME,'ALL' SUBITEMID,1 RESULT"
            strsql += " UNION ALL"
            strsql += " SELECT DISTINCT SUBITEMNAME,CONVERT(vARCHAR,SUBITEMID),2 RESULT FROM " & cnAdminDb & "..SUBITEMMAST WHERE 1=1"
            If typ <> "''" And typ <> "" Then strsql += vbCrLf + " AND ITEMID IN(" & typ & ")"
            strsql += " ORDER BY RESULT,SUBITEMNAME"
            dtsubitem = New DataTable
            da = New OleDbDataAdapter(strsql, cn)
            da.Fill(dtsubitem)
            BrighttechPack.GlobalMethods.FillCombo(chkLst, dtsubitem, "SUBITEMNAME", , "ALL")
        ElseIf flag = 3 Then
            strsql = " SELECT 'ALL' METALNAME,'ALL' METALID,1 RESULT"
            strsql += " UNION ALL"
            strsql += " SELECT DISTINCT METALNAME,METALID,2 RESULT FROM " & cnAdminDb & "..METALMAST "
            strsql += " ORDER BY RESULT,METALNAME"
            dtmetal = New DataTable
            da = New OleDbDataAdapter(strsql, cn)
            da.Fill(dtmetal)
            BrighttechPack.GlobalMethods.FillCombo(chkLst, dtmetal, "METALNAME", , "ALL")
        ElseIf flag = 4 Then
            strsql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
            strsql += " UNION ALL"
            strsql += " SELECT DISTINCT COSTNAME,COSTID,2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
            strsql += " ORDER BY RESULT,COSTNAME"
            dtcostname = New DataTable
            da = New OleDbDataAdapter(strsql, cn)
            da.Fill(dtcostname)
            BrighttechPack.GlobalMethods.FillCombo(chkLst, dtcostname, "COSTNAME", True, IIf(cnDefaultCostId, "ALL", cnCostName))
        ElseIf flag = 5 Then
            strsql = " SELECT 'ALL' ITEMCTRNAME,'ALL' ITEMCTRID,1 RESULT"
            strsql += " UNION ALL"
            strsql += " SELECT DISTINCT ITEMCTRNAME,CONVERT(VARCHAR,ITEMCTRID),2 RESULT FROM " & cnAdminDb & "..ITEMCOUNTER WHERE 1=1"
            strsql += " ORDER BY RESULT,ITEMCTRNAME"
            dtCounter = New DataTable
            da = New OleDbDataAdapter(strsql, cn)
            da.Fill(dtCounter)
            BrighttechPack.GlobalMethods.FillCombo(chkLst, dtCounter, "ITEMCTRNAME", , "ALL")
        Else
            strsql = " SELECT 'ALL' TABLECODE,1 RESULT"
            strsql += " UNION ALL"
            strsql += " SELECT DISTINCT TABLECODE ,2 RESULT FROM " & cnAdminDb & "..ITEMTAG WHERE 1=1"
            If typ <> "" And typ <> "''" Then strsql += vbCrLf + " AND ITEMID IN(" & typ & ")"
            strsql += " ORDER BY RESULT,TABLECODE"
            dttabcode = New DataTable
            da = New OleDbDataAdapter(strsql, cn)
            da.Fill(dttabcode)
            BrighttechPack.GlobalMethods.FillCombo(chkLst, dttabcode, "TABLECODE", , "ALL")
        End If
    End Function

#End Region
    Private Sub chkcmbitemname_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkcmbitemname.Leave
        Type = GetSelecteditemid(chkcmbitemname, True)
        Fillcombo(chklstsubitem, Type, 2)
        Fillcombo(chklsttablecode, Type, 3)
    End Sub


   
    Private Sub Chkcmbmetal_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Chkcmbmetal.Leave
        Type = GetSelectedMetalid(Chkcmbmetal, True)
        Fillcombo(chkcmbitemname, "Y", 1, Type)
    End Sub

    Private Sub GroupBox1_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupBox1.Enter

    End Sub
End Class