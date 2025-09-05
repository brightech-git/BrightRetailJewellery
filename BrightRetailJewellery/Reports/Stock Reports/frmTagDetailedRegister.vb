Imports System.Data.OleDb
Public Class frmTagDetailedRegister
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim objGridShower As frmGridDispDia
    Dim defaultPic As String = GetAdmindbSoftValue("PICPATH")
    Private Sub chkSales_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSales.CheckedChanged
        lblTo.Visible = True
        dtpTo.Visible = True
        lblAsonDate.Text = "From Date"
    End Sub

    Private Sub chkPurchase_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPurchase.CheckedChanged
        lblTo.Visible = True
        dtpTo.Visible = True
        lblAsonDate.Text = "From Date"
    End Sub

    Private Sub chkStockOnly_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkStockOnly.CheckedChanged
        lblTo.Visible = False
        dtpTo.Visible = False
        lblAsonDate.Text = "As On Date"
    End Sub

    Private Sub frmTagDetailedRegister_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not defaultPic.EndsWith("\") Then defaultPic += "\"
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE ACTIVE = 'Y' ORDER BY DISPLAYORDER"
        FillCheckedListBox(strSql, chkLstMetal)
        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            chkLstCostCentre.Enabled = True
            chkLstCostCentre.Items.Clear()
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            'FillCheckedListBox(strSql, chkLstCostCentre)
            Dim dt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            For i As Integer = 0 To dt.Rows.Count - 1
                If cnCostName = dt.Rows(i).Item("COSTNAME").ToString And cnDefaultCostId = False Then
                    chkLstCostCentre.Items.Add(dt.Rows(i).Item("COSTNAME").ToString, True)
                Else
                    chkLstCostCentre.Items.Add(dt.Rows(i).Item("COSTNAME").ToString, False)
                End If
            Next
            If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkLstCostCentre.Enabled = False
        Else
            chkLstCostCentre.Enabled = False
            chkLstCostCentre.Items.Clear()
        End If
        strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACTIVE = 'Y' ORDER BY DESIGNERNAME"
        FillCheckedListBox(strSql, chkLstDesigner)
        strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ACTIVE = 'Y' ORDER BY ITEMCTRNAME"
        FillCheckedListBox(strSql, chkLstItemCounter)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub frmTagDetailedRegister_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        ' chkStockOnly.Checked = True
        'chkWithStudded.Checked = True
        dtpAsOnDate.Value = GetServerDate()
        dtpAsOnDate.Select()
        Prop_Gets()
    End Sub

    Private Sub chkMetalSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMetalSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstMetal, chkMetalSelectAll.Checked)
    End Sub

    Private Sub chkCostCentreSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCostCentreSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCostCentre, chkCostCentreSelectAll.Checked)
    End Sub

    Private Sub chkDesignerSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDesignerSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstDesigner, chkDesignerSelectAll.Checked)
    End Sub

    Private Sub chkItemCounterSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkItemCounterSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstItemCounter, chkItemCounterSelectAll.Checked)
    End Sub

    Private Sub chkItemSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkItemSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstItem, chkItemSelectAll.Checked)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        DataGridLoad()
    End Sub
    Private Sub DataGridLoad()
        Dim int As Integer
        Dim dtItem As New DataTable
        Dim dts As New DataTable
        Dim chkCostName As String = GetChecked_CheckedList(chkLstCostCentre)
        Dim chkItemCounter As String = GetChecked_CheckedList(chkLstItemCounter)
        Dim chkMetalName As String = GetChecked_CheckedList(chkLstMetal)
        Dim chkItemName As String = GetChecked_CheckedList(chkLstItem)
        Dim chkDesigner As String = GetChecked_CheckedList(chkLstDesigner)

        strSql = " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'Temp_Details')>0 DROP TABLE TEMPTABLEDB..Temp_Details"
        strSql += " select Itemid,Tagno,sum(Grswt) as Grswt,'B' TTYPE,(select Metalid from " & cnAdminDb & "..ITEMMAST  as I where I.ITEMID = T.itemid)as Metalid,"
        strSql += " sum(Grswt) as Netwt,'G' as stoneunit, (Select SUM(purvalue) from " & cnAdminDb & "..PURITEMTAG as P where P.ITEMID = t.itemid) as Purvalue "
        strSql += " Into TEMPTABLEDB..Temp_Details"
        strSql += " from " & cnAdminDb & "..ITEMTAG as T"
        strSql += " Group by ITEMID,tagno"
        strSql += " Union all"
        strSql += " select Itemid,Tagno,0 as grswt,'' TTYPE,(select Metalid from " & cnAdminDb & "..Category  as I where I.Catcode = T.catcode)as Metalid,"
        strSql += " sum(Grswt) as Netwt,'G' as stoneunit,0 as Purvalue "
        strSql += " from " & cnAdminDb & "..ITEMTAGMETAL as T"
        strSql += " Group by ITEMID,TAGNO,catcode"
        strSql += " Union all"
        strSql += " select Itemid,Tagno,0 as grswt,'' TTYPE,(select Metalid from " & cnAdminDb & "..itemmast  as I where I.ITEMID  = T.Stnitemid)as Metalid,"
        strSql += " sum((case when stoneunit = 'G' then stnwt  else Stnwt /5  end)) as Netwt,stoneunit,0 as Purvalue "
        strSql += " from " & cnAdminDb & "..ITEMTAGSTONE as T"
        strSql += " Group by ITEMID,TAGNO,STNITEMID ,STONEUNIT "
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        'strSql = vbCrLf + " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'Temp_Details')>0 DROP TABLE Temp_Details"
        'strSql += vbCrLf + " select Itemid,Tagno,sum(Grswt) as Grswt,'B' TTYPE,(select Metalid from " & cnAdminDb & "..ITEMMAST  as I where I.ITEMID = T.itemid)as Metalid,"
        'strSql += vbCrLf + " sum(NETWT) as Netwt,'G' as stoneunit, (Select SUM(purvalue) from " & cnAdminDb & "..PURITEMTAG as P where P.ITEMID = t.itemid) as Purvalue "
        'strSql += vbCrLf + " ,RECDATE,sum(LESSWT)as LESSWT,sum(SALVALUE) as SALEVALUE"
        'strSql += vbCrLf + " ,ISSDATE,ISSREFNO,ACTUALRECDATE"
        'strSql += vbCrLf + " Into Temp_Details"
        'strSql += vbCrLf + " from " & cnAdminDb & "..ITEMTAG as T"
        'strSql += vbCrLf + " Group by ITEMID,tagno,RECDATE,ISSDATE,ISSREFNO,ACTUALRECDATE"
        'strSql += vbCrLf + " Union all"
        'strSql += vbCrLf + " select Itemid,Tagno,sum(Grswt) as grswt,'' TTYPE,(select Metalid from " & cnAdminDb & "..Category  as I where I.Catcode = T.catcode)as Metalid,"
        'strSql += vbCrLf + " 0 as Netwt,'G' as stoneunit,0 as Purvalue "
        'strSql += vbCrLf + " ,RECDATE,0 as LESSWT,0 as SALEVALUE"
        'strSql += vbCrLf + " ,ISSDATE,'' as ISSREFNO,'' as ACTUALRECDATE"
        'strSql += vbCrLf + " from " & cnAdminDb & "..ITEMTAGMETAL as T"
        'strSql += vbCrLf + " Group by ITEMID,TAGNO,catcode,RECDATE,ISSDATE"
        'strSql += vbCrLf + " Union all"
        'strSql += vbCrLf + " select Itemid,Tagno,0 as grswt,'' TTYPE,(select Metalid from " & cnAdminDb & "..itemmast  as I where I.ITEMID  = T.Stnitemid)as Metalid,"
        'strSql += vbCrLf + " sum((case when stoneunit = 'G' then stnwt  else Stnwt /5  end)) as Netwt,stoneunit,0 as Purvalue "
        'strSql += vbCrLf + " ,RECDATE,0 as LESSWT,0 as SALEVALUE"
        'strSql += vbCrLf + " ,ISSDATE,'' as ISSREFNO,'' as ACTUALRECDATE"
        'strSql += vbCrLf + " from " & cnAdminDb & "..ITEMTAGSTONE as T"
        'strSql += vbCrLf + " Group by ITEMID,TAGNO,STNITEMID ,STONEUNIT,RECDATE,ISSDATE "
        'cmd = New OleDbCommand(strSql, cn)
        'cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()

        strSql = vbCrLf + " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPTAGPURDET')>0 DROP TABLE TEMPTABLEDB..TEMPTAGPURDET"
        strSql += vbCrLf + "  Select ITEM,TAGNO,GRSWT as GRSWT,"

        Dim sql As String
        If chkMetalName = "" Then
            sql = " SELECT METALNAME,METALID FROM " & cnAdminDb & "..METALMAST order by DISPLAYORDER"
        Else
            sql = " SELECT METALNAME,METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ") order by DISPLAYORDER"
        End If
        da = New OleDbDataAdapter(sql, cn)
        Dim dt As New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            For int = 0 To dt.Rows.Count - 1
                If dt.Rows(int).Item("METALID").ToString = "G" Then
                    strSql += " CONVERT(NUMERIC(15,3)," & dt.Rows(int).Item("METALNAME") & ")" & dt.Rows(int).Item("METALNAME") & ","
                End If
            Next
            For int = 0 To dt.Rows.Count - 1
                If dt.Rows(int).Item("METALID").ToString <> "G" Then
                    strSql += " CONVERT(NUMERIC(15,3)," & dt.Rows(int).Item("METALNAME") & ")" & dt.Rows(int).Item("METALNAME") & ","
                End If
            Next
        End If

        strSql += vbCrLf + " 1 RESULT,convert(varchar(2),'') COLHEAD,"
        strSql += vbCrLf + " CONVERT(varchar(60),ITEMID)ITEMID "
        'strSql += vbCrLf + " ,NETWT,LESSWT,SALEVALUE,recdate,ISSDATE,ISSREFNO "
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPTAGPURDET"
        strSql += vbCrLf + " FROM ("
        strSql += vbCrLf + " select (select ITEMNAME from " & cnAdminDb & "..ITEMMAST as a where a.itemid=T.ITEMID) as ITEM,TAGNO,sum(Grswt) as GRSWT,"

        If dt.Rows.Count > 0 Then
            For int = 0 To dt.Rows.Count - 1
                'If int = 0 Then
                If dt.Rows(int).Item("METALID").ToString = "G" Then
                    strSql = strSql + "(sum(case when Metalid = '" & dt.Rows(int).Item("METALID") & "' then Netwt else (CASE WHEN TTYPE = '' THEN Netwt *-1 ELSE 0 END) end)) as " & dt.Rows(int).Item("METALNAME") & ","
                    'strSql = strSql + ","
                End If
                'End If
            Next

            For int = 0 To dt.Rows.Count - 1
                If dt.Rows(int).Item("METALID").ToString <> "G" Then
                    strSql = strSql + "(sum(case when Metalid = '" & dt.Rows(int).Item("METALID") & "' then Netwt else 0 end)) as " & dt.Rows(int).Item("METALNAME") & ""
                    strSql = strSql + ","
                End If
            Next
        End If

        strSql = strSql + vbCrLf + " ITEMID "
        'strSql = strSql + vbCrLf + " ,sum(NETWT)as NETWT,sum(LESSWT)as LESSWT,sum(SALEVALUE)as SALEVALUE,recdate,ISSDATE,ISSREFNO"
        strSql = strSql + vbCrLf + " from TEMPTABLEDB..Temp_details as T "
        ' strSql = strSql + vbCrLf + " WHERE Recdate =ActualRecDate"
        strSql = strSql + vbCrLf + " Group by tagno,itemid)X"

        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + "IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPFINALREPORT')>0 DROP TABLE TEMPTABLEDB..TEMPFINALREPORT "
        strSql += vbCrLf + " Select CONVERT (INT,0)SNO,CONVERT(varchar(60),PARTICULAR)PARTICULAR,RECDATE"
        If chkSales.Checked Then
            strSql += vbCrLf + " ,ISSDATE,ISSREFNO"
        ElseIf chkPurchase.Checked Then
            strSql += vbCrLf + " ,ISSDATE"
        End If
        strSql += vbCrLf + " ,ITEMID,TAGNO,PCS,GRSWT,LESSWT,NETWT,STYLENO,CONVERT(NUMERIC(15,2),TOTAL)TOTAL"
        strSql += vbCrLf + " ,ISNULL(TOTAL,0)-ISNULL(STNAMT,0)-ISNULL(PURMC,0) AS NETAMT,"

        If dt.Rows.Count > 0 Then
            For int = 0 To dt.Rows.Count - 1
                If dt.Rows(int).Item("METALID").ToString = "G" Then
                    strSql += vbCrLf + " CONVERT(NUMERIC(15,3)," & dt.Rows(int).Item("METALNAME") & ")" & dt.Rows(int).Item("METALNAME") & ","
                End If
            Next

            For int = 0 To dt.Rows.Count - 1
                If dt.Rows(int).Item("METALID").ToString <> "G" Then
                    strSql += vbCrLf + " CONVERT(NUMERIC(15,3)," & dt.Rows(int).Item("METALNAME") & ")" & dt.Rows(int).Item("METALNAME") & ","
                End If
            Next
        End If
        strSql += vbCrLf + " CONVERT(NUMERIC(15,3),STNWT)STNWT ,CONVERT(NUMERIC(15,2),STNAMT)STNAMT, "
        strSql += vbCrLf + " CONVERT(NUMERIC(15,2),PURMC)PURMC,CONVERT(NUMERIC(15,2),TOTAL-ISNULL(PURTAX,0)) GRANDTOTAL, CONVERT(NUMERIC(15,2),PURTAX) PURTAX,"
        strSql += vbCrLf + " TRANINVNO,SUPBILLNO,ITEM,1 RESULT,convert(varchar(2),'') COLHEAD"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0)SALEVALUE,DESIGNER"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPFINALREPORT"
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " ("
        'strSql += vbCrLf + " select Distinct (Select itemname from " & cnAdminDb & "..itemmast as I where I.ITEMID=a.ITEMID) as PARTICULAR,Convert(varchar(20),a.recdate,103)recdate"
        strSql += vbCrLf + " select Distinct (Select itemname from " & cnAdminDb & "..itemmast as I where I.ITEMID=a.ITEMID) as PARTICULAR,a.recdate as recdate"
        If chkSales.Checked Then
            strSql += vbCrLf + " ,Convert(varchar(20),a.ISSDATE,103)ISSDATE,a.ISSREFNO"
        ElseIf chkPurchase.Checked Then
            strSql += vbCrLf + " ,Convert(varchar(20),a.ISSDATE,103)ISSDATE"
        End If
        strSql += vbCrLf + " ,a.ITEMID,a.TAGNO,PCS,e.GRSWT,a.LESSWT,a.NETWT,"
        strSql += vbCrLf + " STYLENO ,0 as TOTAL,"

        If dt.Rows.Count > 0 Then
            For int = 0 To dt.Rows.Count - 1
                If dt.Rows(int).Item("METALID").ToString = "G" Then
                    strSql += vbCrLf + dt.Rows(int).Item("METALNAME") & ","
                End If
            Next

            For int = 0 To dt.Rows.Count - 1
                If dt.Rows(int).Item("METALID").ToString <> "G" Then
                    strSql += vbCrLf + dt.Rows(int).Item("METALNAME") & ","
                End If
            Next
        End If
        strSql += vbCrLf + " sum (c.STNWT) as STNWT,0 STNAMT,0 as PURMC,0 as GRANDTOTAL,0 as PURTAX,TRANINVNO,SUPBILLNO,"
        'strSql += " CASE WHEN a.ISSDATE IS NOT NULL THEN 'DATE : ' + CONVERT(VARCHAR,a.ISSDATE,103),"
        strSql += vbCrLf + " ITEM,RESULT,COLHEAD "
        strSql += vbCrLf + " ,a.SALVALUE AS SALEVALUE,(Select DESIGNERNAME from " & cnAdminDb & "..Designer  where DESIGNERID=a.DESIGNERID ) as DESIGNER"
        strSql += vbCrLf + " from " & cnAdminDb & "..ITEMTAG a"
        strSql += vbCrLf + " left join " & cnAdminDb & "..ITEMTAGMETAL b on a.TAGNO=b.TAGNO"
        strSql += vbCrLf + " left join " & cnAdminDb & "..ITEMTAGSTONE c on a.TAGNO=c.TAGNO "
        'strSql += vbCrLf + " left join " & cnAdminDb & "..PURITEMTAG d on a.TAGNO=d.TAGNO"
        'strSql += vbCrLf + "  left join " & cnAdminDb & "..PURITEMTAGSTONE PS on a.TAGNO=PS.TAGNO"
        strSql += vbCrLf + " left join TEMPTABLEDB..TEMPTAGPURDET e on a.TAGNO=e.tagno"
        If chkStockOnly.Checked Then
            strSql += vbCrLf + " Where  a.RECDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
        ElseIf chkSales.Checked Then
            strSql += vbCrLf + " Where  a.ISSDATE BETWEEN '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        ElseIf chkPurchase.Checked Then
            strSql += vbCrLf + " Where  a.RECDATE BETWEEN '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        End If
        strSql += vbCrLf + " and RESULT=1"
        If chkCostName <> "" Then strSql += vbCrLf + " AND a.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If chkItemCounter <> "" Then strSql += vbCrLf + " AND a.ITEMCTRID IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & chkItemCounter & "))"
        If chkItemName <> "" Then strSql += vbCrLf + " AND a.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & chkItemName & "))"
        If chkMetalName <> "" Then strSql += vbCrLf + " AND a.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkDesigner <> "" Then strSql += vbCrLf + " AND DESIGNERID IN (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN (" & chkDesigner & "))"
        If txtTranInvNo.Text <> "" Then strSql += vbCrLf + " AND TRANINVNO = '" & txtTranInvNo.Text & "'"
        If chkStockOnly.Checked Then strSql += vbCrLf + " AND a.ISSDATE IS NULL"
        strSql += vbCrLf + " group by a.TAGNO,STYLENO,PCS,a.NETWT,"

        If dt.Rows.Count > 0 Then
            For int = 0 To dt.Rows.Count - 1
                If dt.Rows(int).Item("METALID").ToString = "G" Then
                    strSql += vbCrLf + dt.Rows(int).Item("METALNAME") & ","
                End If
            Next

            For int = 0 To dt.Rows.Count - 1
                If dt.Rows(int).Item("METALID").ToString <> "G" Then
                    strSql += vbCrLf + dt.Rows(int).Item("METALNAME") & ","
                End If
            Next
        End If

        strSql += vbCrLf + " TRANINVNO,SUPBILLNO,a.ITEMID,RESULT,COLHEAD,ITEM,DESIGNERID,e.GRSWT,a.NETWT,a.LESSWT,a.SALVALUE,a.recdate,a.ISSREFNO,a.ISSDATE)X"

        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPFINALREPORT)>0"
        strSql += vbCrLf + " BEGIN"
        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPFINALREPORT(ITEM,PARTICULAR,RESULT,COLHEAD)"
        strSql += vbCrLf + " SELECT DISTINCT ITEM,PARTICULAR,0 RESULT,convert(varchar(2),'T') COLHEAD FROM TEMPTABLEDB..TEMPFINALREPORT"
        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPFINALREPORT(ITEM,PARTICULAR,RESULT,COLHEAD,PCS,GRSWT,NETAMT,"

        If dt.Rows.Count > 0 Then
            For int = 0 To dt.Rows.Count - 1
                If dt.Rows(int).Item("METALID").ToString = "G" Then
                    strSql += vbCrLf + dt.Rows(int).Item("METALNAME") & ","
                End If
            Next

            For int = 0 To dt.Rows.Count - 1
                If dt.Rows(int).Item("METALID").ToString <> "G" Then
                    strSql += vbCrLf + dt.Rows(int).Item("METALNAME") & ","
                End If
            Next
        End If

        strSql += vbCrLf + " STNWT,STNAMT,PURMC,GRANDTOTAL,PURTAX,NETWT,LESSWT,SALEVALUE)"
        strSql += vbCrLf + " Select distinct ITEM,'SUB TOTAL',2 RESULT,'S' COLHEAD,SUM(PCS),SUM(GRSWT),SUM(NETAMT),"


        If dt.Rows.Count > 0 Then
            For int = 0 To dt.Rows.Count - 1
                If dt.Rows(int).Item("METALID").ToString = "G" Then
                    strSql += vbCrLf + " SUM(" & dt.Rows(int).Item("METALNAME") & "),"
                End If
            Next

            For int = 0 To dt.Rows.Count - 1
                If dt.Rows(int).Item("METALID").ToString <> "G" Then
                    strSql += vbCrLf + " SUM(" & dt.Rows(int).Item("METALNAME") & "),"
                End If
            Next
        End If

        strSql += vbCrLf + " SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(PURTAX),SUM(NETWT),SUM(LESSWT),SUM(SALEVALUE)"
        strSql += vbCrLf + " from TEMPTABLEDB..TEMPFINALREPORT Group by ITEM"

        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPFINALREPORT(ITEM,PARTICULAR,RESULT,COLHEAD,TOTAL,PCS,GRSWT,NETAMT,"

        If dt.Rows.Count > 0 Then
            For int = 0 To dt.Rows.Count - 1
                If dt.Rows(int).Item("METALID").ToString = "G" Then
                    strSql += vbCrLf + dt.Rows(int).Item("METALNAME") & ","
                End If
            Next

            For int = 0 To dt.Rows.Count - 1
                If dt.Rows(int).Item("METALID").ToString <> "G" Then
                    strSql += vbCrLf + dt.Rows(int).Item("METALNAME") & ","
                End If
            Next
        End If


        strSql += vbCrLf + " STNWT,STNAMT,PURMC,GRANDTOTAL ,PURTAX,NETWT,LESSWT,SALEVALUE)"
        strSql += vbCrLf + " Select DISTINCT 'ZZZZ','GRAND TOTAL',3 RESULT,convert(varchar(2),'G') COLHEAD,"
        strSql += vbCrLf + " SUM(TOTAL),SUM(PCS),SUM(GRSWT),SUM(NETAMT),"

        If dt.Rows.Count > 0 Then
            For int = 0 To dt.Rows.Count - 1
                If dt.Rows(int).Item("METALID").ToString = "G" Then
                    strSql += vbCrLf + " SUM(" & dt.Rows(int).Item("METALNAME") & "),"
                End If
            Next

            For int = 0 To dt.Rows.Count - 1
                If dt.Rows(int).Item("METALID").ToString <> "G" Then
                    strSql += vbCrLf + " SUM(" & dt.Rows(int).Item("METALNAME") & "),"
                End If
            Next
        End If


        strSql += vbCrLf + " SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(PURTAX),SUM(NETWT),SUM(LESSWT),SUM(SALEVALUE)"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPFINALREPORT where Result=1"
        strSql += vbCrLf + " End"

        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMPFINALREPORT ORDER BY ITEM,RESULT"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtItem)

        'dtItem.Columns.Add("Sno", Type.GetType("System.Int32"))
        Dim sNo As Integer = 0
        For i As Integer = 0 To dtItem.Rows.Count - 1
            If dtItem.Rows(i).Item("TAGNO").ToString <> "" Then
                sNo += 1
                dtItem.Rows(i).Item("SNO") = sNo
            End If
        Next

        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Size = New Size(778, 550)
        objGridShower.Text = "TAGED DETAILED REGISTER "
        Dim tit As String = "TAGED DETAILED REGISTER " + IIf(chkMetalName <> "", chkMetalName.Replace("'", ""), " ALL METAL") + vbCrLf
        If chkStockOnly.Checked Then
            tit += "AS ON DATE  " + dtpAsOnDate.Text
        ElseIf chkSales.Checked Then
            tit += "FROM " + dtpAsOnDate.Text + " TO " + dtpTo.Text
        ElseIf chkPurchase.Checked Then
            tit += "FROM " + dtpAsOnDate.Text + " TO " + dtpTo.Text
        End If
        Dim Cname As String = ""
        If chkLstCostCentre.Items.Count > 0 And chkLstCostCentre.CheckedItems.Count > 0 Then
            For CNT As Integer = 0 To chkLstCostCentre.Items.Count - 1
                If chkLstCostCentre.GetItemChecked(CNT) = True Then
                    Cname += "" & chkLstCostCentre.Items(CNT) + ","
                End If
            Next
            If Cname <> "" Then Cname = " :" + Mid(Cname, 1, Len(Cname) - 1)
        End If
        objGridShower.lblTitle.Text = tit & Cname
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtItem)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        'objGridShower.gridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        AddHandler objGridShower.gridView.KeyPress, AddressOf GridView_KeyPress

        'If objGridShower.gridView("COLHEAD", 0).Value = "T" Then
        '    objGridShower.gridView.BackgroundColor = Color.Gold
        'End If
        'If objGridShower.gridView.Columns("COLHEAD") = "S" Then
        '    objGridShower.gridView.ForeColor = Color.CadetBlue
        'End If
        'If objGridShower.gridView.Columns("COLHEAD") = "G" Then
        '    objGridShower.gridView.BackgroundColor = Color.LightYellow
        'End If

        objGridShower.pnlFooter.Visible = False
        objGridShower.FormReLocation = False
        objGridShower.FormReSize = False
        objGridShower.Show()
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        DataGridView_Detailed(objGridShower.gridView)
        GridViewFormat()
        objGridShower.FormReSize = True
        objGridShower.gridViewHeader.Visible = False
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle = objGridShower.gridView.ColumnHeadersDefaultCellStyle
        Prop_Sets()
    End Sub
    Private Sub DataGridView_Detailed(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            .Columns("SNO").Width = 60
            .Columns("PARTICULAR").Width = 200
            .Columns("TAGNO").Width = 75
            .Columns("STYLENO").Width = 60
            .Columns("TOTAL").Width = 100
            .Columns("PCS").Width = 70
            .Columns("NETWT").Width = 80
            .Columns("NETAMT").Width = 90
            '.Columns("DIAWT").Width = 80
            ' .Columns("DIAAMT").Width = 100
            .Columns("STNWT").Width = 80
            .Columns("STNAMT").Width = 100
            .Columns("PURMC").Width = 80
            .Columns("GRANDTOTAL").Width = 100
            .Columns("PURTAX").Width = 70
            .Columns("TRANINVNO").Width = 100
            .Columns("SUPBILLNO").Width = 100
            '.Columns("TAGIMAGE").Width = 100

            '    .Columns("DIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            '    If chkWithImage.Checked And chkWithImage.Visible Then
            '        .Columns("TAGIMAGE").Visible = True
            '    Else
            '        .Columns("TAGIMAGE").Visible = False
            '    End If
            .Columns("DESIGNER").Visible = rbtDetailed.Checked
            .Columns("STYLENO").Visible = rbtDetailed.Checked
            .Columns("TAGNO").Visible = Not rbtSummary.Checked
            .Columns("STYLENO").Visible = Not rbtSummary.Checked
            .Columns("TRANINVNO").Visible = Not rbtSummary.Checked
            .Columns("SUPBILLNO").Visible = Not rbtSummary.Checked

            '.Columns("PCTFILE").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("ITEM").Visible = False
            If chkSales.Checked Then .Columns("ISSREFNO").Visible = True
            '.Columns("KEYNO").Visible = False
            '.Columns("TAGVAL").Visible = False
            .Columns("ITEMID").Visible = True
            .Columns("STYLENO").Visible = False
            .Columns("TOTAL").Visible = False
            .Columns("NETAMT").Visible = False
            .Columns("STNAMT").Visible = False
            .Columns("PURMC").Visible = False
            .Columns("GRANDTOTAL").Visible = False
            .Columns("PURTAX").Visible = False
            .Columns("TRANINVNO").Visible = False
            .Columns("SUPBILLNO").Visible = False

            If chkSales.Checked Then
                .Columns("ISSDATE").HeaderText = "BILL DATE"
                .Columns("ISSREFNO").HeaderText = "BILLNO"
            ElseIf chkPurchase.Checked Then
                .Columns("ISSDATE").HeaderText = "SALE DATE"
            End If
            .Columns("ITEMID").HeaderText = "PRODUCT CODE"
            .Columns("RECDATE").HeaderText = "PLUS DATE"

            FormatGridColumns(dgv)
        End With
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
                    .Columns(i).DefaultCellStyle.Format = "dd/MM/yyyy"
                End If
                If Not sortColumns Then .Columns(i).SortMode = DataGridViewColumnSortMode.NotSortable
                '.Columns(i).Resizable = DataGridViewTriState.False 
            Next
        End With
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Private Sub chkLstItem_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstItem.GotFocus
        Dim chkMetalName As String = GetChecked_CheckedList(chkLstMetal)
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST "
        strSql += " WHERE ACTIVE = 'Y'"
        If chkMetalName <> "" Then strSql += " AND METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")) "
        strSql += " ORDER BY ITEMNAME"
        FillCheckedListBox(strSql, chkLstItem)
        SetChecked_CheckedList(chkLstItem, chkItemSelectAll.Checked)
    End Sub

    Private Sub rbtDetailed_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtDetailed.CheckedChanged
        chkWithImage.Visible = rbtDetailed.Checked
    End Sub
    Private Sub GridView_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs)
        If e.KeyChar = Chr(Keys.Enter) Then
            With objGridShower
                If Not .gridView.RowCount > 0 Then Exit Sub
                If .gridView.CurrentRow Is Nothing Then Exit Sub
                If .gridView.CurrentRow.Cells("ITEMID").Value.ToString = "" Then Exit Sub
                If .gridView.CurrentRow.Cells("TAGNO").Value.ToString = "" Then Exit Sub
                Dim objTagViewer As New frmTagImageViewer( _
                 .gridView.CurrentRow.Cells("TAGNO").Value.ToString, _
                 Val(.gridView.CurrentRow.Cells("ITEMID").Value.ToString), _
                 BrighttechPack.Methods.GetRights(_DtUserRights, frmTagCheck.Name, BrighttechPack.Methods.RightMode.Authorize, False))
                objTagViewer.ShowDialog()
            End With
        End If
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
    Private Sub Prop_Sets()
        Dim obj As New frmTagDetailedRegister_Properties
        obj.p_txtTranInvNo = txtTranInvNo.Text
        obj.p_chkDesignerSelectAll = chkDesignerSelectAll.Checked
        GetChecked_CheckedList(chkLstDesigner, obj.p_chkLstDesigner)
        'obj.p_chkCostCentreSelectAll = chkCostCentreSelectAll.Checked
        'GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        obj.p_chkMetalSelectAll = chkMetalSelectAll.Checked
        GetChecked_CheckedList(chkLstMetal, obj.p_chkLstMetal)
        obj.p_chkItemSelectAll = chkItemSelectAll.Checked
        GetChecked_CheckedList(chkLstItem, obj.p_chkLstItem)
        obj.p_chkItemCounterSelectAll = chkItemCounterSelectAll.Checked
        GetChecked_CheckedList(chkLstItemCounter, obj.p_chkLstItemCounter)
        obj.p_chkStockOnly = chkStockOnly.Checked
        obj.p_chkSalesOnly = chkSales.Checked
        obj.p_chkPurchaseOnly = chkPurchase.Checked
        obj.p_rbtDetailed = rbtDetailed.Checked
        obj.p_rbtSummary = rbtSummary.Checked
        obj.p_chkWithImage = chkWithImage.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmTagDetailedRegister_Properties))
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New frmTagDetailedRegister_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmTagDetailedRegister_Properties))
        txtTranInvNo.Text = obj.p_txtTranInvNo
        chkDesignerSelectAll.Checked = obj.p_chkDesignerSelectAll
        SetChecked_CheckedList(chkLstDesigner, obj.p_chkLstDesigner, Nothing)
        'chkCostCentreSelectAll.Checked = obj.p_chkCostCentreSelectAll
        'SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, cnCostName)
        chkMetalSelectAll.Checked = obj.p_chkMetalSelectAll
        SetChecked_CheckedList(chkLstMetal, obj.p_chkLstMetal, Nothing)
        chkItemSelectAll.Checked = obj.p_chkItemSelectAll
        SetChecked_CheckedList(chkLstItem, obj.p_chkLstItem, Nothing)
        chkItemCounterSelectAll.Checked = obj.p_chkItemCounterSelectAll
        SetChecked_CheckedList(chkLstItemCounter, obj.p_chkLstItemCounter, Nothing)
        chkStockOnly.Checked = obj.p_chkStockOnly
        chkSales.Checked = obj.p_chkSalesOnly
        chkPurchase.Checked = obj.p_chkPurchaseOnly
        rbtDetailed.Checked = obj.p_rbtDetailed
        rbtSummary.Checked = obj.p_rbtSummary
        chkWithImage.Checked = obj.p_chkWithImage
    End Sub
End Class
Public Class frmTagDetailedRegister_Properties
    Private txtTranInvNo As String = ""
    Public Property p_txtTranInvNo() As String
        Get
            Return txtTranInvNo
        End Get
        Set(ByVal value As String)
            txtTranInvNo = value
        End Set
    End Property
    Private chkDesignerSelectAll As Boolean = False
    Public Property p_chkDesignerSelectAll() As Boolean
        Get
            Return chkDesignerSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkDesignerSelectAll = value
        End Set
    End Property
    Private chkLstDesigner As New List(Of String)
    Public Property p_chkLstDesigner() As List(Of String)
        Get
            Return chkLstDesigner
        End Get
        Set(ByVal value As List(Of String))
            chkLstDesigner = value
        End Set
    End Property
    Private chkCostCentreSelectAll As Boolean = False
    Public Property p_chkCostCentreSelectAll() As Boolean
        Get
            Return chkCostCentreSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkCostCentreSelectAll = value
        End Set
    End Property
    Private chkLstCostCentre As New List(Of String)
    Public Property p_chkLstCostCentre() As List(Of String)
        Get
            Return chkLstCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkLstCostCentre = value
        End Set
    End Property
    Private chkMetalSelectAll As Boolean = False
    Public Property p_chkMetalSelectAll() As Boolean
        Get
            Return chkMetalSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkMetalSelectAll = value
        End Set
    End Property
    Private chkLstMetal As New List(Of String)
    Public Property p_chkLstMetal() As List(Of String)
        Get
            Return chkLstMetal
        End Get
        Set(ByVal value As List(Of String))
            chkLstMetal = value
        End Set
    End Property

    Private chkItemSelectAll As Boolean = False
    Public Property p_chkItemSelectAll() As Boolean
        Get
            Return chkItemSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkItemSelectAll = value
        End Set
    End Property
    Private chkLstItem As New List(Of String)
    Public Property p_chkLstItem() As List(Of String)
        Get
            Return chkLstItem
        End Get
        Set(ByVal value As List(Of String))
            chkLstItem = value
        End Set
    End Property
    Private chkItemCounterSelectAll As Boolean = False
    Public Property p_chkItemCounterSelectAll() As Boolean
        Get
            Return chkItemCounterSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkItemCounterSelectAll = value
        End Set
    End Property
    Private chkLstItemCounter As New List(Of String)
    Public Property p_chkLstItemCounter() As List(Of String)
        Get
            Return chkLstItemCounter
        End Get
        Set(ByVal value As List(Of String))
            chkLstItemCounter = value
        End Set
    End Property

    Private chkStockOnly As Boolean = False
    Public Property p_chkStockOnly() As Boolean
        Get
            Return chkStockOnly
        End Get
        Set(ByVal value As Boolean)
            chkStockOnly = value
        End Set
    End Property
    Private chkSalesOnly As Boolean = False
    Public Property p_chkSalesOnly() As Boolean
        Get
            Return chkSalesOnly
        End Get
        Set(ByVal value As Boolean)
            chkSalesOnly = value
        End Set
    End Property
    Private chkPurchaseOnly As Boolean = False
    Public Property p_chkPurchaseOnly() As Boolean
        Get
            Return chkPurchaseOnly
        End Get
        Set(ByVal value As Boolean)
            chkPurchaseOnly = value
        End Set
    End Property
    Private rbtDetailed As Boolean = True
    Public Property p_rbtDetailed() As Boolean
        Get
            Return rbtDetailed
        End Get
        Set(ByVal value As Boolean)
            rbtDetailed = value
        End Set
    End Property
    Private rbtSummary As Boolean = False
    Public Property p_rbtSummary() As Boolean
        Get
            Return rbtSummary
        End Get
        Set(ByVal value As Boolean)
            rbtSummary = value
        End Set
    End Property
    Private chkWithImage As Boolean = False
    Public Property p_chkWithImage() As Boolean
        Get
            Return chkWithImage
        End Get
        Set(ByVal value As Boolean)
            chkWithImage = value
        End Set
    End Property
End Class