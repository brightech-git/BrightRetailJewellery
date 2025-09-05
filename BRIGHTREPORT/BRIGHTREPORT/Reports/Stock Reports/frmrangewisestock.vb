Imports System.Data.OleDb
Imports System.Threading
Imports System.IO
Public Class frmrangewisestock
    Dim strSql As String
    Dim strQry As String
    Dim dsGridView As New DataSet
    Dim dtcompany, dtCostCentre, dtmetal, dtitem, dtdesign, dtsubitem, dtrange, dtcounter As New DataTable
    Dim OLtran As OleDbTransaction
    Dim companyid As String = "ALL"
    Dim costids As String = "ALL"
    Dim metalids As String = "ALL"
    Dim itemid As String = "ALL"
    Dim subitemid As String = "ALL"
    Dim designerid As String = "ALL"
    Dim Counterid As String = "ALL"
    Dim stktype As String = "T"


    Dim cmd As OleDbCommand
    Dim SPECIFICFORMAT As String = GetAdmindbSoftValue("SPECIFICFORMAT", "0")
    Private Sub Generateselectedragetable()

        If Trim(chkCmbCostCentre.Text.ToString) = "ALL" Or Trim(chkCmbCostCentre.Text.ToString) = "" Then costids = "ALL" Else costids = GetSelectedCostId(chkCmbCostCentre, True)
        If Trim(chkcmbitem.Text.ToString) = "ALL" Or Trim(chkcmbitem.Text.ToString) = "" Then itemid = "ALL" Else itemid = GetSelecteditemid(chkcmbitem, True)
        If Trim(chkcmbsutitem.Text.ToString) = "ALL" Or Trim(chkcmbsutitem.Text.ToString) = "" Then subitemid = "ALL" Else subitemid = GetSelectedSubitemid(chkcmbsutitem, True)

        strSql = " IF OBJECT_ID('TEMPTABLEDB..TEMPCHECKEDITEM') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPCHECKEDITEM"
        If rbtDetailed.Checked Then
            strSql += " SELECT DISTINCT  CONVERT(VARCHAR,FROMWEIGHT)+'-'+CONVERT(VARCHAR,TOWEIGHT)RANGE,FROMWEIGHT,TOWEIGHT,CAPTION,ITEMID,SUBITEMID,COSTID,1 RESULT INTO TEMPTABLEDB..TEMPCHECKEDITEM FROM " & cnAdminDb & "..RANGEMAST WHERE 1=1 " ',ITEMID,SUBITEMID,COSTID 
        Else
            strSql += " SELECT DISTINCT  CONVERT(VARCHAR,FROMWEIGHT)+'-'+CONVERT(VARCHAR,TOWEIGHT)RANGE,FROMWEIGHT,TOWEIGHT,CAPTION,ITEMID,0 SUBITEMID,COSTID,1 RESULT INTO TEMPTABLEDB..TEMPCHECKEDITEM FROM " & cnAdminDb & "..RANGEMAST WHERE 1=1 " ',ITEMID,SUBITEMID,COSTID 
        End If

        If costids <> "ALL" Then strSql += vbCrLf + " AND COSTID IN(" & costids & ")"
        If itemid <> "ALL" Then
            strSql += vbCrLf + " AND ITEMID IN(" & itemid & ")"
        End If
        If subitemid <> "ALL" And rbtDetailed.Checked Then
            strSql += vbCrLf + " AND SUBITEMID IN(" & subitemid & ")"
        End If
        strSql += vbCrLf + " ORDER BY RESULT,FROMWEIGHT"
        cmd = New OleDbCommand(strSql, cn) : cmd.ExecuteNonQuery()


        strSql = " IF OBJECT_ID('TEMPTABLEDB..TEMPSELECTRANGE') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPSELECTRANGE"
        If rbtDetailed.Checked Then
            strSql += " SELECT FROMWEIGHT,TOWEIGHT,CAPTION,ITEMID,SUBITEMID,COSTID INTO TEMPTABLEDB..TEMPSELECTRANGE FROM " & cnAdminDb & "..RANGEMAST WHERE 1<>1 " ',ITEMID,SUBITEMID,COSTID 
        Else
            strSql += " SELECT DISTINCT FROMWEIGHT,TOWEIGHT,CAPTION,ITEMID,0 SUBITEMID,COSTID INTO TEMPTABLEDB..TEMPSELECTRANGE FROM " & cnAdminDb & "..RANGEMAST WHERE 1<>1 " ',ITEMID,SUBITEMID,COSTID 
        End If

        If costids <> "ALL" Then strSql += vbCrLf + " AND COSTID IN(" & costids & ")"
        If itemid <> "ALL" Then
            strSql += vbCrLf + " AND ITEMID IN(" & itemid & ")"
        End If
        If subitemid <> "ALL" And rbtDetailed.Checked Then
            strSql += vbCrLf + " AND SUBITEMID IN(" & subitemid & ")"
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.ExecuteNonQuery()

        ''newly added
        strSql = " "
        ''strSql = " SELECT 'ALL' RANGE,0 FROMWEIGHT,0 TOWEIGHT,'ALL' CAPTION,0 ITEMID,0 SUBITEMID,'ALL' COSTID "
        ''strSql += " ,0 RESULT UNION ALL"
        If rbtDetailed.Checked Then
            strSql += " SELECT DISTINCT CONVERT(VARCHAR,FROMWEIGHT)+'-'+CONVERT(VARCHAR,TOWEIGHT)RANGE,FROMWEIGHT,TOWEIGHT,CAPTION,ITEMID,SUBITEMID,COSTID "
        Else
            strSql += " SELECT DISTINCT CONVERT(VARCHAR,FROMWEIGHT)+'-'+CONVERT(VARCHAR,TOWEIGHT)RANGE,FROMWEIGHT,TOWEIGHT,CAPTION,ITEMID,0 SUBITEMID,COSTID "
        End If
        strSql += " ,1 RESULT FROM " & cnAdminDb & "..RANGEMAST WHERE 1=1 "
        If costids <> "ALL" Then strSql += vbCrLf + " AND COSTID IN(" & costids & ")"
        If itemid <> "ALL" Then strSql += vbCrLf + " AND ITEMID IN(" & itemid & ")"
        If subitemid <> "ALL" And rbtDetailed.Checked Then strSql += vbCrLf + " AND SUBITEMID IN(" & subitemid & ")"
        strSql += vbCrLf + " ORDER BY RESULT,FROMWEIGHT"
        dtrange = New DataTable
        dtrange = GetSqlTable(strSql, cn)
        If dtrange.Rows.Count = 0 Then : MsgBox("No Ranges available.") : Exit Sub : End If

        Dim res As String
        If Trim(chkcmbrange.Text.ToString) = "ALL" Or Trim(chkcmbrange.Text.ToString) = "" Then
            'For i As Integer = 0 To dtrange.Rows.Count - 1
            '    res = dtrange.Rows(i).Item("Caption").ToString
            '    If res <> "ALL" Then
            '        InsertRange(dtrange.Rows(i).Item("FROMWEIGHT").ToString, dtrange.Rows(i).Item("TOWEIGHT").ToString, dtrange.Rows(i).Item("CAPTION").ToString, dtrange.Rows(i).Item("ITEMID").ToString, dtrange.Rows(i).Item("SUBITEMID").ToString, dtrange.Rows(i).Item("COSTID").ToString)
            '    End If
            'Next
            For i As Integer = 0 To chkcmbrange.Items.Count - 1
                Dim range() As String
                range = chkcmbrange.Items.Item(i).ToString.Split("-")
                If range.Length = 1 Then Continue For
                strSql = vbCrLf + "  SELECT CAPTION FROM TEMPTABLEDB..TEMPCHECKEDITEM WHERE FROMWEIGHT = '" & range(0) & "' AND TOWEIGHT = '" & range(1) & "'"
                Dim strcap As String = objGPack.GetSqlValue(strSql)
                InsertRange(range(0), range(1), strcap, "", "", "")
            Next
        Else
            For i As Integer = 0 To chkcmbrange.CheckedItems.Count - 1
                Dim range() As String
                'Dim strcap As String
                'res = dtrange.Rows(i).Item("CAPTION").ToString
                ''If res <> "ALL" Then
                'strcap = dtrange.Rows(i).Item("CAPTION").ToString
                range = chkcmbrange.CheckedItems.Item(i).ToString.Split("-")
                strSql = vbCrLf + "  SELECT CAPTION FROM TEMPTABLEDB..TEMPCHECKEDITEM WHERE FROMWEIGHT = '" & range(0) & "' AND TOWEIGHT = '" & range(1) & "'"
                Dim strcap As String = objGPack.GetSqlValue(strSql)
                InsertRange(range(0), range(1), strcap, "", "", "")
                'End If
            Next
        End If
    End Sub
    Private Sub InsertRange(ByVal frwt As String, ByVal towt As String, ByVal Cap As String, ByVal itemid As String, ByVal subid As String, ByVal cost As String)
        strSql = "INSERT INTO TEMPTABLEDB..TEMPSELECTRANGE"
        strSql += " SELECT '" & frwt & "','" & towt & "','" & Cap & "','" & itemid & "','" & subid & "','" & cost & "'"
        cmd = New OleDbCommand(strSql, cn) : cmd.ExecuteNonQuery()
    End Sub
    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Trim(chkcmbcompany.Text.ToString) = "ALL" Or Trim(chkcmbcompany.Text.ToString) = "" Then companyid = "ALL" Else companyid = GetSelectedCompanyId(chkcmbcompany, True)
        If Trim(chkCmbCostCentre.Text.ToString) = "ALL" Or Trim(chkCmbCostCentre.Text.ToString) = "" Then costids = "ALL" Else costids = GetSelectedCostId(chkCmbCostCentre, True)
        If Trim(chkcmbmetal.Text.ToString) = "ALL" Or Trim(chkcmbmetal.Text.ToString) = "" Then metalids = "ALL" Else metalids = GetSelectedMetalid(chkcmbmetal, True)
        If Trim(chkcmbitem.Text.ToString) = "ALL" Or Trim(chkcmbitem.Text.ToString) = "" Then itemid = "ALL" Else itemid = GetSelecteditemid(chkcmbitem, True)
        If Trim(chkcmbsutitem.Text.ToString) = "ALL" Or Trim(chkcmbsutitem.Text.ToString) = "" Then subitemid = "ALL" Else subitemid = GetSelectedSubitemid(chkcmbsutitem, True)
        If Trim(chkcmbdesigner.Text.ToString) = "ALL" Or Trim(chkcmbdesigner.Text.ToString) = "" Then designerid = "ALL" Else designerid = GetSelectedDesignerid(chkcmbdesigner, True)
        If Trim(chkCmbCounter.Text.ToString) = "ALL" Or Trim(chkCmbCounter.Text.ToString) = "" Then Counterid = "ALL" Else Counterid = GetSelectedCounderid(chkCmbCounter, True)
        Generateselectedragetable()
        If rbtboth.Checked Then : stktype = "B" : ElseIf rbttag.Checked Then : stktype = "T" : Else : stktype = "N" : End If
        HorizontalView()
    End Sub

    Private Function GetSelectedCounderid(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME= '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
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
    Private Sub HorizontalView()
        gridview.DataSource = Nothing
        gridheader1.DataSource = Nothing
        strSql = "  IF OBJECT_ID('TEMPTABLEDB..TEMPRANGEWISE','U')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPRANGEWISE "
        strSql += vbCrLf + "  IF OBJECT_ID('TEMPTABLEDB..TEMPRANGEWISE_RES','U')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPRANGEWISE_RES"
        cmd = New OleDbCommand(strSql, cn) : cmd.ExecuteNonQuery()

        strSql = " EXECUTE " & cnAdminDb & "..SP_RPT_RANGEWISESTOCK"
        strSql += vbCrLf + "@TEMPTABLE='TEMPTABLEDB..TEMPRANGEWISE'"
        strSql += vbCrLf + ",@ASONDATE ='" & dtpFrom.Value.ToString("yyyy/MM/dd") & "'"
        strSql += vbCrLf + ",@COMPANYID=""" & companyid.ToString & """"
        strSql += vbCrLf + ",@COSTID=""" & costids.ToString & """"
        strSql += vbCrLf + ",@METALID=""" & metalids.ToString & """"
        strSql += vbCrLf + ",@ITEMID=""" & itemid.ToString & """"
        strSql += vbCrLf + ",@SUBITEMID=""" & subitemid.ToString & """"
        strSql += vbCrLf + ",@DESIGNERID=""" & designerid.ToString & """"
        strSql += vbCrLf + ",@WITHCUM='" & IIf(chkwithcum.Checked, "Y", "N") & "'"
        strSql += vbCrLf + ",@STKTYPE='" & stktype & "'"
        strSql += vbCrLf + ",@BASEDON='" & IIf(rbtitem.Checked, "I", "D") & "'"
        strSql += vbCrLf + ",@DISPTYPE='" & IIf(rbthorizontal.Checked, "H", "V") & "'"
        strSql += vbCrLf + ",@COUNTERIDS = """ & Counterid.ToString & """"
        strSql += vbCrLf + ",@RPTTYPE='" & IIf(rbtDetailed.Checked, "D", "S") & "'"
        strSql += vbCrLf + ",@WITHOUTESTK='" & IIf(chkWithoutEmptyStock.Checked, "Y", "N") & "'"
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        Dim da As New OleDbDataAdapter(cmd)
        Dim dss As New DataSet
        da.Fill(dss)
        Dim dt As New DataTable
        Dim tempdt As New DataTable
        dt = dss.Tables(0)
        If dt.Rows.Count > 0 Then
            gridview.DataSource = Nothing
            gridview.DataSource = dt
        Else
            gridview.DataSource = Nothing
            gridheader1.DataSource = Nothing
            MsgBox("No Records found..")
            Exit Sub
        End If
        TabControl1.SelectedTab = TabPage2
        With gridview
            For i As Integer = 0 To gridview.Columns.Count - 1
                If .Columns(i).HeaderText.Contains("PCS") Then .Columns(i).Width = 90
                If .Columns(i).HeaderText.Contains("GRSWT") Then .Columns(i).Width = 120
                If .Columns(i).HeaderText.Contains("PARTICULAR") Then .Columns(i).Width = 250
                If .Columns(i).HeaderText.Contains("COSTNAME") Then .Columns(i).Width = 150
            Next
            .Columns("COLHEAD").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("METALNAME").Visible = False
            If .Columns.Contains("SUBITEMNAME") Then .Columns("SUBITEMNAME").Visible = False
            If .Columns.Contains("SUBITEMID") Then .Columns("SUBITEMID").Visible = False
            .Columns("METALID").Visible = False
            If SPECIFICFORMAT = "1" And rbthorizontal.Checked Then
                .Columns("COSTNAME").Visible = True
            Else
                .Columns("COSTNAME").Visible = False
            End If
            .Columns("COSTID").Visible = False
            If rbtitem.Checked Then .Columns("ITEMID").Visible = False : .Columns("ITEMNAME").Visible = False
            If rbtdesigner.Checked Then .Columns("DESIGNERID").Visible = False : .Columns("DESIGNERNAME").Visible = False
            If Not rbthorizontal.Checked Then
                .Columns("FROMWEIGHT").Visible = False
                .Columns("TOWEIGHT").Visible = False
                .Columns("DISPORDER").Visible = False
            End If
        End With
        GridViewHeaderStyle()
        lblTitle.Text = ""
        lblTitle.Text += " AS ON " & dtpFrom.Value.ToString("dd-MM-yyyy")
        If chkcmbmetal.Text <> "ALL" Then lblTitle.Text += " FOR " & chkcmbmetal.Text
        lblTitle.Text += IIf(chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "", " :" & chkCmbCostCentre.Text, "")
        lblTitle.Text += IIf(chkCmbCounter.Text <> "ALL" And chkCmbCounter.Text <> "", " :" & chkCmbCounter.Text, "")
    End Sub
    Private Sub GridViewHeaderStyle()
        Dim dtrange As New DataTable("COST")
        If rbthorizontal.Checked Then
            Dim hro() As String
            Dim range As String = "ALL"
            With dtrange
                .Columns.Add("PARTICULAR", GetType(String))
                If SPECIFICFORMAT = "1" Then
                    .Columns.Add("COSTNAME", GetType(String))
                End If
                Dim cdt As New DataTable
                strSql = " select 'TOTAL'range,0 TOWEIGHT,0 RESULT  union all "
                strSql += " select 'OTHERS'range,0 TOWEIGHT,2 RESULT  union all "
                'strSql += "SELECT  convert(varchar,FROMWEIGHT) +'-'+ convert(varchar,TOWEIGHT)RANGE ,TOWEIGHT,1 RESULT FROM TEMPTABLEDB..TEMPRANGEWISE WHERE ISNULL(FROMWEIGHT,0)<>0 AND ISNULL(TOWEIGHT,0)<>0   group by TOWEIGHT,FROMWEIGHT order by RESULT,TOWEIGHT  "
                strSql += "SELECT  CONVERT(VARCHAR,FROMWEIGHT) +'-'+ CONVERT(VARCHAR,TOWEIGHT)RANGE,TOWEIGHT,1 RESULT FROM TEMPTABLEDB..TEMPRANGEWISE WHERE ISNULL(FROMWEIGHT,0)<>0 AND ISNULL(TOWEIGHT,0)<>0  AND IITEMID =(SELECT TOP 1 IITEMID FROM TEMPTABLEDB..TEMPRANGEWISE_1 ORDER BY CNT  DESC ) GROUP BY TOWEIGHT,FROMWEIGHT ORDER BY RESULT,TOWEIGHT  "
                cdt = GetSqlTable(strSql, cn)
                Dim count As Integer = 1
                If cdt.Rows.Count > 0 Then
                    range = ""
                    For i As Integer = 0 To cdt.Rows.Count - 1
                        If cdt.Rows(i).Item("RANGE").ToString <> "OTHERS" And cdt.Rows(i).Item("RANGE").ToString <> "TOTAL" Then
                            range += count.ToString
                            count = count + 1
                        Else
                            range += cdt.Rows(i).Item("RANGE").ToString
                        End If

                        If i < cdt.Rows.Count - 1 Then range += ","
                    Next
                End If
                If range <> "ALL" Then
                    range = range.Replace("'", "")
                    hro = range.Split(",")
                End If
                For i As Integer = 0 To hro.Length - 1
                    Dim txt As String = ""
                    Dim type As String = ""
                    For j As Integer = 2 To gridview.Columns.Count - 1
                        If gridview.Columns(j).HeaderText.Contains(hro(i).ToString) Then
                            txt += gridview.Columns(j).HeaderText
                            If j <= gridview.Columns.Count Then txt += "~"
                        End If
                    Next
                    If txt <> "" Then .Columns.Add(txt, GetType(String)) : .Columns(txt).Caption = "" 'hro(i).ToString
                Next
                dtrange.Columns.Add("SCROLL", GetType(String))
                dtrange.Columns("PARTICULAR").Caption = ""
                If SPECIFICFORMAT = "1" Then
                    dtrange.Columns("COSTNAME").Caption = ""
                End If
                dtrange.Columns("SCROLL").Caption = ""
            End With
            With gridheader1
                .DataSource = dtrange
                For i As Integer = 0 To hro.Length - 1
                    Dim txt As String = ""
                    For j As Integer = 2 To gridview.Columns.Count - 1
                        If gridview.Columns(j).HeaderText.Contains(hro(i).ToString) Then
                            txt += gridview.Columns(j).HeaderText
                            If j <= gridview.Columns.Count Then txt += "~"
                        End If
                    Next
                    If txt <> "" Then If (txt.Contains("TOTAL") Or txt.Contains("OTHERS")) Then .Columns(txt).HeaderText = hro(i).ToString Else .Columns(txt).HeaderText = ""
                Next
                .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Columns("PARTICULAR").Width = gridview.Columns("PARTICULAR").Width
                If SPECIFICFORMAT = "1" Then
                    .Columns("COSTNAME").Width = gridview.Columns("COSTNAME").Width
                End If
                For i As Integer = 0 To hro.Length - 1
                    Dim txt As String = ""
                    Dim width As Integer = 0
                    Dim type As String = ""
                    Dim typwidth As Integer = 0
                    Dim metalwidth As Integer = 0
                    For j As Integer = 2 To gridview.Columns.Count - 1
                        If gridview.Columns(j).HeaderText.Contains(hro(i).ToString) Then
                            txt += gridview.Columns(j).HeaderText
                            width += gridview.Columns(j).Width
                            If j <= gridview.Columns.Count Then txt += "~"
                        End If
                    Next
                    If txt <> "" Then .Columns(txt).Width = width
                Next
                gridview.Focus()
                Dim colWid As Integer = 0
                For cnt As Integer = 0 To gridview.ColumnCount - 1
                    If gridview.Columns(cnt).Visible Then colWid += gridview.Columns(cnt).Width
                Next
                gridheader1.Columns("PARTICULAR").Frozen = True
                gridview.Columns("PARTICULAR").Frozen = True
                If SPECIFICFORMAT = "1" Then
                    gridheader1.Columns("COSTNAME").Frozen = True
                    gridview.Columns("COSTNAME").Frozen = True
                End If

                If colWid >= gridview.Width Then
                    gridheader1.Columns("SCROLL").Visible = CType(gridview.Controls(0), HScrollBar).Visible
                    gridheader1.Columns("SCROLL").Width = CType(gridview.Controls(1), VScrollBar).Width
                Else
                    gridheader1.Columns("SCROLL").Visible = False
                End If
            End With
            For i As Integer = 0 To gridview.Columns.Count - 1
                If gridview.Columns(i).HeaderText.Contains("PCS") Then gridview.Columns(i).HeaderText = "PCS"
                If gridview.Columns(i).HeaderText.Contains("GRSWT") Then gridview.Columns(i).HeaderText = "GRSWT"
                If gridview.Columns(i).HeaderText.Contains("AMOUNT") Then gridview.Columns(i).HeaderText = "AMOUNT"
                gridview.Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight
            Next

            gridview.Columns("PARTICULAR").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            gridview.Columns("PARTICULAR").HeaderText = ""
            If SPECIFICFORMAT = "1" Then
                gridview.Columns("COSTNAME").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                gridview.Columns("COSTNAME").HeaderText = ""
            End If
            Dim dt As DataTable = CType(gridview.DataSource, DataTable)
            Dim dt1 As DataTable = CType(gridheader1.DataSource, DataTable)
            gridheader1.Columns("SCROLL").Visible = False
            gridheader1.Columns("SCROLL").HeaderText = ""
            gridview.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            gridview.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Else
            'With gridheader1
            '    .DataSource = dtrange
            '    .Columns("PCS~GRSWT~RANGE").HeaderText = "WEIGHT"
            '    .Columns(" ").HeaderText = "PARTICULARS"

            '    .Columns("PCS~GRSWT~RANGE").Width = _
            '    IIf(gridview.Columns("PCS").Visible, gridview.Columns("PCS").Width, 0) _
            '    + IIf(gridview.Columns("GRSWT").Visible, gridview.Columns("GRSWT").Width, 0) _
            '    + IIf(gridview.Columns("RANGE").Visible, gridview.Columns("RANGE").Width, 0)

            '    .Columns("PCS~GRSWT~RANGE").HeaderText = "WEIGHT"
            'End With

            'dtrange.Columns("PARTICULARS").HE = "PARTICULARS"
            'dtrange.Columns("PCS~GRSWT~RANGE").Caption = "WEIGHT"
            dtrange.Columns.Add("PARTICULAR", GetType(String))
            dtrange.Columns.Add("WEIGHT~PCS,WEIGHT~GRSWT,WEIGHT~RANGE", GetType(String))
            With dtrange
                .Columns("PARTICULAR").Caption = "PARTICULARS"
                .Columns("WEIGHT~PCS,WEIGHT~GRSWT,WEIGHT~RANGE").Caption = "WEIGHT"
                '.Columns("WEIGHT~PCS,WEIGHT~GRSWT,WEIGHT~RANGE"). = "WEIGHT"
            End With

            gridheader1.DataSource = dtrange
            gridheader1.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            gridheader1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            gridheader1.Columns("WEIGHT~PCS,WEIGHT~GRSWT,WEIGHT~RANGE").HeaderText = "WEIGHT"
            gridheader1.Columns("PARTICULAR").Width = gridview.Columns("PARTICULAR").Width
            gridheader1.Columns("WEIGHT~PCS,WEIGHT~GRSWT,WEIGHT~RANGE").Width = gridview.Columns("PCS").Width + gridview.Columns("GRSWT").Width + gridview.Columns("RANGE").Width
        End If

        For i As Integer = 0 To gridview.Rows.Count - 1

            If gridview.Rows(i).Cells("COLHEAD").Value = "G" Then
                gridview.Rows(i).DefaultCellStyle.BackColor = Color.LightYellow
                gridview.Rows(i).DefaultCellStyle.ForeColor = Color.Black
                gridview.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf gridview.Rows(i).Cells("COLHEAD").Value = "T" Then
                gridview.Rows(i).DefaultCellStyle.BackColor = Color.Green
                gridview.Rows(i).DefaultCellStyle.ForeColor = Color.White
                gridview.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf gridview.Rows(i).Cells("COLHEAD").Value = "T1" Then
                gridview.Rows(i).DefaultCellStyle.BackColor = Color.LightGreen
                gridview.Rows(i).DefaultCellStyle.ForeColor = Color.Red
                gridview.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf gridview.Rows(i).Cells("COLHEAD").Value = "T2" Then
                gridview.Rows(i).DefaultCellStyle.BackColor = Color.LightBlue
                gridview.Rows(i).DefaultCellStyle.ForeColor = Color.Red
                gridview.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            ElseIf gridview.Rows(i).Cells("COLHEAD").Value = "S" Or gridview.Rows(i).Cells("COLHEAD").Value = "S1" Or gridview.Rows(i).Cells("COLHEAD").Value = "S2" Then
                gridview.Rows(i).DefaultCellStyle.BackColor = Color.White
                gridview.Rows(i).DefaultCellStyle.ForeColor = Color.Blue
                gridview.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            End If
        Next
        gridview.Columns("PARTICULAR").HeaderText = "PARTICULAR"
        If SPECIFICFORMAT = "1" And rbthorizontal.Checked Then
            gridview.Columns("COSTNAME").HeaderText = "COSTNAME"
        End If
        'gridview.Columns("WEIGHT~PCS,WEIGHT~GRSWT,WEIGHT~RANGE").HeaderText = "WEIGHT"
    End Sub

    Private Sub frmSalesAbstract_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            If TabControl1.SelectedTab Is TabPage1 = False Then
                TabControl1.SelectedTab = TabPage1
            End If
        End If
    End Sub

    Private Sub frmSalesAbstract_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        TabControl1.ItemSize = New Size(0, 1)
        TabControl1.SizeMode = TabSizeMode.Fixed

        strSql = " SELECT 'ALL'COSTNAME, 'ALL' COSTID,1 RESULT UNION ALL SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))

        strSql = " SELECT 'ALL'COMPANYNAME, 'ALL' COMPANYID,1 RESULT UNION ALL SELECT COMPANYNAME,CONVERT(vARCHAR,COMPANYID),2 RESULT FROM " & cnAdminDb & "..COMPANY"
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtcompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtcompany)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbcompany, dtcompany, "COMPANYNAME", , "ALL")

        strSql = " SELECT 'ALL'METALNAME, 'ALL' METALID,1 RESULT UNION ALL SELECT METALNAME,CONVERT(vARCHAR,METALID),2 RESULT FROM " & cnAdminDb & "..METALMAST"
        strSql += " ORDER BY RESULT,METALNAME"
        dtmetal = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtmetal)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbmetal, dtmetal, "METALNAME", , "ALL")
        strSql = " SELECT 'ALL'DESIGNERNAME, 'ALL' DESIGNERID,1 RESULT UNION ALL SELECT DESIGNERNAME,CONVERT(vARCHAR,DESIGNERID),2 RESULT FROM " & cnAdminDb & "..DESIGNER"
        strSql += " ORDER BY RESULT,DESIGNERNAME"
        dtdesign = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtdesign)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbdesigner, dtdesign, "DESIGNERNAME", , "ALL")


        TabControl1.SelectedTab = TabPage1
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExtToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Function Funcfillitemname(ByVal defvalue As String) As Integer
        strSql = " SELECT 'ALL'ITEMNAME, 'ALL' ITEMID,'' METALID,1 RESULT UNION ALL SELECT ITEMNAME,CONVERT(VARCHAR,ITEMID),METALID,2 RESULT FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " ORDER BY RESULT,ITEMNAME"
        dtitem = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtitem)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbitem, dtitem, "ITEMNAME", , defvalue.ToString)
    End Function
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.Value = System.DateTime.Now
        Panel3.Visible = False
        txtItemCode_NUM.Clear()
        'strSql = " SELECT 'ALL'ITEMNAME, 'ALL' ITEMID,'' METALID,1 RESULT UNION ALL SELECT ITEMNAME,CONVERT(vARCHAR,ITEMID),METALID,2 RESULT FROM " & cnAdminDb & "..ITEMMAST"
        'strSql += " ORDER BY RESULT,ITEMNAME"
        'dtitem = New DataTable
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dtitem)
        'BrighttechPack.GlobalMethods.FillCombo(chkcmbitem, dtitem, "ITEMNAME", , "ALL")
        Funcfillitemname("ALL")

        strSql = " SELECT 'ALL'SUBITEMNAME, 'ALL' SUBITEMID,'' ITEMID,1 RESULT UNION ALL SELECT SUBITEMNAME,CONVERT(vARCHAR,SUBITEMID),ITEMID,2 RESULT FROM " & cnAdminDb & "..SUBITEMMAST"
        strSql += " ORDER BY RESULT,SUBITEMNAME"
        dtsubitem = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtsubitem)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbsutitem, dtsubitem, "SUBITEMNAME", , "ALL")

        If Trim(chkCmbCostCentre.Text.ToString) = "ALL" Or Trim(chkCmbCostCentre.Text.ToString) = "" Then costids = "ALL" Else costids = GetSelectedCostId(chkCmbCostCentre, True)
        If Trim(chkcmbitem.Text.ToString) = "ALL" Or Trim(chkcmbitem.Text.ToString) = "" Then itemid = "ALL" Else itemid = GetSelecteditemid(chkcmbitem, True)
        If Trim(chkcmbsutitem.Text.ToString) = "ALL" Or Trim(chkcmbsutitem.Text.ToString) = "" Then subitemid = "ALL" Else subitemid = GetSelectedSubitemid(chkcmbsutitem, True)

        strSql = " "
        ''strSql = " SELECT 'ALL' RANGE,0 FROMWEIGHT,0 TOWEIGHT,'ALL' CAPTION,0 ITEMID,0 SUBITEMID,'ALL' COSTID "
        ''strSql += " ,0 RESULT UNION ALL"
        strSql += " SELECT DISTINCT   CONVERT(VARCHAR,FROMWEIGHT)+'-'+CONVERT(VARCHAR,TOWEIGHT)RANGE,FROMWEIGHT,TOWEIGHT,CAPTION,ITEMID,SUBITEMID,COSTID "
        strSql += " ,1 RESULT FROM " & cnAdminDb & "..RANGEMAST WHERE 1=1 "
        If costids <> "ALL" Then strSql += vbCrLf + " AND COSTID IN(" & costids & ")"
        If itemid <> "ALL" Then strSql += vbCrLf + " AND ITEMID IN(" & itemid & ")"
        If subitemid <> "ALL" Then strSql += vbCrLf + " AND SUBITEMID IN(" & subitemid & ")"
        strSql += vbCrLf + " ORDER BY RESULT,FROMWEIGHT"
        dtrange = New DataTable
        dtrange = GetSqlTable(strSql, cn)
        If dtrange.Rows.Count = 1 Then : MsgBox("No Ranges available.") : Exit Sub : End If

        Dim dtrange1 As DataTable
        strSql = " SELECT 'ALL' RANGE,0 FROMWEIGHT,0 TOWEIGHT,'ALL' CAPTION "
        strSql += " ,0 RESULT UNION ALL"
        strSql += " SELECT DISTINCT   CONVERT(VARCHAR,FROMWEIGHT)+'-'+CONVERT(VARCHAR,TOWEIGHT)RANGE,FROMWEIGHT,TOWEIGHT,CAPTION "
        strSql += " ,1 RESULT FROM " & cnAdminDb & "..RANGEMAST WHERE 1=1 "
        If costids <> "ALL" Then strSql += vbCrLf + " AND COSTID IN(" & costids & ")"
        If itemid <> "ALL" Then strSql += vbCrLf + " AND ITEMID IN(" & itemid & ")"
        If subitemid <> "ALL" Then strSql += vbCrLf + " AND SUBITEMID IN(" & subitemid & ")"
        strSql += vbCrLf + " ORDER BY RESULT,FROMWEIGHT"
        dtrange1 = New DataTable
        dtrange1 = GetSqlTable(strSql, cn)
        If dtrange1.Rows.Count = 1 Then : MsgBox("No Ranges available.") : Exit Sub : End If
        BrighttechPack.GlobalMethods.FillCombo(chkcmbrange, dtrange1, "RANGE", , "ALL")

        strSql = " SELECT 'ALL' ITEMCTRNAME,'ALL' ITEMCTRID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ITEMCTRNAME,CONVERT(vARCHAR,ITEMCTRID),2 RESULT FROM " & cnAdminDb & "..ITEMCOUNTER"
        strSql += " ORDER BY RESULT,ITEMCTRNAME"
        dtcounter = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtcounter)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCounter, dtcounter, "ITEMCTRNAME", , "ALL")

        gridview.DataSource = Nothing
        btnView_Search.Enabled = True
        chkCmbCostCentre.Text = "ALL"
        If chkCmbCostCentre.Items.Count > 1 And SPECIFICFORMAT = "1" Then
            For cnt As Integer = 0 To chkCmbCostCentre.Items.Count - 1
                If chkCmbCostCentre.Items(cnt).ToString = "ALL" Then
                    chkCmbCostCentre.SetItemChecked(cnt, True)
                Else
                    chkCmbCostCentre.SetItemChecked(cnt, False)
                End If
            Next
        End If
        chkcmbmetal.Text = "ALL"
        'chkcmbrange.Text = "ALL"
        dtpFrom.Focus()
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If gridview.Rows.Count > 0 Then
            Dim rpttitle As String
            rpttitle = "RANGEWISE STOCK"
            BrightPosting.GExport.Post(Me.Name, rpttitle, lblTitle.Text, gridview, BrightPosting.GExport.GExportType.Export, IIf(rbtvertical.Checked = False, gridheader1, Nothing), Nothing, Nothing)
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridview.KeyPress
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then

        End If
    End Sub

    Private Sub ResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResizeToolStripMenuItem.Click
        If gridview.RowCount > 0 Then
            If ResizeToolStripMenuItem.Checked Then
                gridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridview.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridview.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridview.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
            'gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End If
    End Sub

    Private Sub gridView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridview.Scroll
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridheader1.HorizontalScrollingOffset = e.NewValue
        End If
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then

                gridheader1.HorizontalScrollingOffset = e.NewValue
                gridheader1.Columns("SCROLL").Visible = CType(gridview.Controls(0), HScrollBar).Visible
                gridheader1.Columns("SCROLL").Width = CType(gridview.Controls(1), VScrollBar).Width
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub

    Private Sub chkcmbmetal_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkcmbmetal.Leave
        'If chkcmbmetal.Text.Contains("ALL") Or chkcmbmetal.Text = "" Then chkcmbmetal.Text = "ALL"
        If chkcmbmetal.CheckedItems.Contains("ALL") Or chkcmbmetal.Text = "" Then
            chkcmbmetal.Text = "ALL"
            chkcmbmetal.SetItemCheckState(0, CheckState.Checked)
            For i As Integer = 1 To chkcmbmetal.Items.Count - 1
                If chkcmbmetal.GetItemCheckState(i) = CheckState.Checked Then
                    chkcmbmetal.SetItemCheckState(i, CheckState.Unchecked)
                End If
            Next
        End If

    End Sub

    Private Sub chkCmbCostCentre_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCmbCostCentre.Leave
        If chkCmbCostCentre.CheckedItems.Contains("ALL") Or chkCmbCostCentre.Text = "" Then
            chkCmbCostCentre.Text = "ALL"
            chkCmbCostCentre.SetItemCheckState(0, CheckState.Checked)
            For i As Integer = 1 To chkCmbCostCentre.Items.Count - 1
                If chkCmbCostCentre.GetItemCheckState(i) = CheckState.Checked Then
                    chkCmbCostCentre.SetItemCheckState(i, CheckState.Unchecked)
                End If
            Next
        End If
    End Sub

    Private Sub txtItemCode_NUM_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtItemCode_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Val(txtItemCode_NUM.Text) > 0 Then
                Dim tempitem As String = GetSqlValue(cn, "SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID='" & Val(txtItemCode_NUM.Text) & "'")
                If tempitem Is Nothing Then tempitem = "ALL" : txtItemCode_NUM.Text = ""
                Funcfillitemname(tempitem.ToString)
            End If
        End If
    End Sub

    Private Sub rbtvertical_CheckedChanged(sender As Object, e As EventArgs) Handles rbtvertical.CheckedChanged
        If rbtvertical.Checked Then
            rbtSummary.Visible = True
            chkWithoutEmptyStock.Enabled = True
        Else
            rbtSummary.Visible = False
            rbtSummary.Checked = False
            chkWithoutEmptyStock.Enabled = False
            chkWithoutEmptyStock.Checked = False
        End If
    End Sub

    Private Sub chkcmbdesigner_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub rbthorizontal_CheckedChanged(sender As Object, e As EventArgs) Handles rbthorizontal.CheckedChanged
        If rbthorizontal.Checked Then
            rbtSummary.Visible = False
            rbtSummary.Checked = False
            rbtDetailed.Checked = True
            chkWithoutEmptyStock.Enabled = False
            chkWithoutEmptyStock.Checked = False
        Else
            rbtSummary.Visible = True
            chkWithoutEmptyStock.Enabled = True
        End If
    End Sub

    Private Sub chkcmbrange_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkcmbrange.Enter
        'If Trim(chkCmbCostCentre.Text.ToString) = "ALL" Or Trim(chkCmbCostCentre.Text.ToString) = "" Then costids = "ALL" Else costids = GetSelectedCostId(chkCmbCostCentre, True)
        'If Trim(chkcmbitem.Text.ToString) = "ALL" Or Trim(chkcmbitem.Text.ToString) = "" Then itemid = "ALL" Else itemid = GetSelecteditemid(chkcmbitem, True)
        'If Trim(chkcmbsutitem.Text.ToString) = "ALL" Or Trim(chkcmbsutitem.Text.ToString) = "" Then subitemid = "ALL" Else subitemid = GetSelectedsubitemid(chkcmbsutitem, True)
        'strSql = " SELECT 'ALL' RANGE,0 FROMWEIGHT,0 TOWEIGHT,'ALL' CAPTION,0 RESULT UNION ALL " ',0 ITEMID,0 SUBITEMID,'' COSTID
        'strSql += "SELECT DISTINCT  CONVERT(VARCHAR,FROMWEIGHT)+'-'+CONVERT(VARCHAR,TOWEIGHT)RANGE,FROMWEIGHT,TOWEIGHT,CAPTION,1 RESULT FROM " & cnAdminDb & "..RANGEMAST WHERE 1=1 " ',ITEMID,SUBITEMID,COSTID 
        'If costids <> "ALL" Then strSql += vbCrLf + " AND COSTID IN(" & costids & ")"
        'If itemid <> "ALL" Then strSql += vbCrLf + " AND ITEMID IN(" & itemid & ")"
        'If subitemid <> "ALL" Then strSql += vbCrLf + " AND SUBITEMID IN(" & subitemid & ")"
        'strSql += vbCrLf + " ORDER BY RESULT,FROMWEIGHT"
        'dtrange = New DataTable
        'dtrange = GetSqlTable(strSql, cn)
        'If dtrange.Rows.Count = 1 Then : MsgBox("No Ranges available.") : Exit Sub : End If
        'BrighttechPack.GlobalMethods.FillCombo(chkcmbrange, dtrange, "RANGE", , "ALL")
        ''strSql = " SELECT 'ALL' RANGE,0 FROMWEIGHT,0 TOWEIGHT,'ALL' CAPTION,0 RESULT UNION ALL " ',0 ITEMID,0 SUBITEMID,'' COSTID
        'strSql = "SELECT DISTINCT  CONVERT(VARCHAR,FROMWEIGHT)+'-'+CONVERT(VARCHAR,TOWEIGHT)RANGE,FROMWEIGHT,TOWEIGHT,CAPTION,1 RESULT into TEMPTABLEDB..TEMPCHECKEDRANGE FROM " & cnAdminDb & "..RANGEMAST WHERE 1=1 " ',ITEMID,SUBITEMID,COSTID 
        'If costids <> "ALL" Then strSql += vbCrLf + " AND COSTID IN(" & costids & ")"
        'If itemid <> "ALL" Then strSql += vbCrLf + " AND ITEMID IN(" & itemid & ")"
        'If subitemid <> "ALL" Then strSql += vbCrLf + " AND SUBITEMID IN(" & subitemid & ")"
        'strSql += vbCrLf + " ORDER BY RESULT,FROMWEIGHT"
        'cmd = New OleDbCommand(strSql, cn) : cmd.ExecuteNonQuery()
    End Sub

    Private Sub chkcmbitem_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkcmbitem.Enter
        If Trim(chkcmbmetal.Text.ToString) <> "ALL" And Trim(chkcmbmetal.Text.ToString) <> "" Then
            If Trim(chkcmbmetal.Text.ToString) = "ALL" Or Trim(chkcmbmetal.Text.ToString) = "" Then metalids = "ALL" Else metalids = GetSelectedMetalid(chkcmbmetal, True)
            Dim dttemp As New DataTable
            Dim dv As New DataView
            dttemp = dtitem.Copy()
            dv = dttemp.DefaultView
            dv.RowFilter = "(METALID IN(" & metalids & "))"
            dttemp = dv.ToTable()
            dtitem.Rows.Clear()
            Dim ro As DataRow
            ro = dtitem.NewRow()
            ro("ITEMNAME") = "ALL"
            ro("ITEMID") = "0"
            ro("METALID") = "ALL"
            ro("RESULT") = "0"
            dtitem.Rows.Add(ro)
            dtitem.Merge(dttemp)
            BrighttechPack.GlobalMethods.FillCombo(chkcmbitem, dtitem, "ITEMNAME", , "ALL")
        End If
    End Sub

    Private Sub chkcmbsutitem_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkcmbsutitem.Enter
        If Trim(chkcmbitem.Text.ToString) <> "ALL" And Trim(chkcmbitem.Text.ToString) <> "" Then
            If Trim(chkcmbitem.Text.ToString) = "ALL" Or Trim(chkcmbitem.Text.ToString) = "" Then itemid = "ALL" Else itemid = GetSelecteditemid(chkcmbitem, True)
            Dim dttemp As New DataTable
            Dim dv As New DataView
            dttemp = dtsubitem.Copy()
            dv = dttemp.DefaultView
            dv.RowFilter = "(ITEMID IN(" & itemid & "))"
            dttemp = dv.ToTable()
            dtsubitem.Rows.Clear()
            Dim ro As DataRow
            ro = dtsubitem.NewRow()
            ro("SUBITEMNAME") = "ALL"
            ro("SUBITEMID") = "0"
            ro("ITEMID") = "0"
            ro("RESULT") = "0"
            dtsubitem.Rows.Add(ro)
            dtsubitem.Merge(dttemp)
            BrighttechPack.GlobalMethods.FillCombo(chkcmbsutitem, dtsubitem, "SUBITEMNAME", , "ALL")
        End If
    End Sub

    Private Sub chkcmbrange_GotFocus(sender As Object, e As EventArgs) Handles chkcmbrange.GotFocus
        If chkcmbrange.Text = "ALL" Then
            If Trim(chkCmbCostCentre.Text.ToString) = "ALL" Or Trim(chkCmbCostCentre.Text.ToString) = "" Then costids = "ALL" Else costids = GetSelectedCostId(chkCmbCostCentre, True)
            If Trim(chkcmbitem.Text.ToString) = "ALL" Or Trim(chkcmbitem.Text.ToString) = "" Then itemid = "ALL" Else itemid = GetSelecteditemid(chkcmbitem, True)
            If Trim(chkcmbsutitem.Text.ToString) = "ALL" Or Trim(chkcmbsutitem.Text.ToString) = "" Then subitemid = "ALL" Else subitemid = GetSelectedSubitemid(chkcmbsutitem, True)
            Dim dtrange1 As DataTable
            strSql = " SELECT 'ALL' RANGE,0 FROMWEIGHT,0 TOWEIGHT,'ALL' CAPTION "
            strSql += " ,0 RESULT UNION ALL"
            strSql += " SELECT DISTINCT   CONVERT(VARCHAR,FROMWEIGHT)+'-'+CONVERT(VARCHAR,TOWEIGHT)RANGE,FROMWEIGHT,TOWEIGHT,CAPTION "
            strSql += " ,1 RESULT FROM " & cnAdminDb & "..RANGEMAST WHERE 1=1 "
            If costids <> "ALL" Then strSql += vbCrLf + " AND COSTID IN(" & costids & ")"
            If itemid <> "ALL" Then strSql += vbCrLf + " AND ITEMID IN(" & itemid & ")"
            If subitemid <> "ALL" Then strSql += vbCrLf + " AND SUBITEMID IN(" & subitemid & ")"
            strSql += vbCrLf + " ORDER BY RESULT,FROMWEIGHT"
            dtrange1 = New DataTable
            dtrange1 = GetSqlTable(strSql, cn)
            If dtrange1.Rows.Count = 1 Then : MsgBox("No Ranges available.") : Exit Sub : End If
            BrighttechPack.GlobalMethods.FillCombo(chkcmbrange, dtrange1, "RANGE", , "ALL")
        End If
    End Sub
    Private Sub btnback_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnback.Click
        If Not TabControl1.SelectedTab Is TabPage1 Then TabControl1.SelectedTab = TabPage1
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If gridview.Rows.Count > 0 Then
            Dim rpttitle As String
            rpttitle = "RANGEWISE STOCK"
            BrightPosting.GExport.Post(Me.Name, rpttitle, lblTitle.Text, gridview, BrightPosting.GExport.GExportType.Print, IIf(rbtvertical.Checked = False, gridheader1, Nothing), Nothing, Nothing)
        End If
    End Sub
End Class