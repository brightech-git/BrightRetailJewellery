Imports System.Data.OleDb
Imports System.Threading
Imports System.IO
Public Class frmGSTR3BNEW
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim dsGridView As New DataSet
    Dim dtCostCentre As New DataTable
    Dim ChitDb As String = GetAdmindbSoftValue("CHITDBPREFIX", "")
    Dim Chit As String = GetAdmindbSoftValue("CHITDB", "N")
    Dim GST3B_F1 As String = GetAdmindbSoftValue("GST3B_F1", "N")
    Private Sub SalesAbs()
        Try
            If rbtmulti.Checked = True Then
                If ChkcmbFilter.Text = "" Then
                    MsgBox("Select Any One Transaction Type", MsgBoxStyle.Information)
                    ChkcmbFilter.Focus()
                    Exit Sub
                End If
            End If
            Prop_Sets()
            gridView.DataSource = Nothing
            gridViewHead.DataSource = Nothing
            gridView.Refresh()
            gridViewHead.Refresh()
            Dim Sysid As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
            Me.Refresh()
            Dim GstCalc As String = "N"
            Dim Selectedfilter As String = ""
            If ChkcmbFilter.Text.Contains("PURCHASE RD") Then
                Selectedfilter += "PR,"
            End If
            If ChkcmbFilter.Text.Contains("PURCHASE URD") Then
                Selectedfilter += "PU,"
            End If
            If ChkcmbFilter.Text.Contains("PURCHASE RETURN") Then
                Selectedfilter += "PIR,"
            End If
            If ChkcmbFilter.Text.Contains("JOURNAL ENTRY") Then
                Selectedfilter += "JE,"
            End If
            If ChkcmbFilter.Text.Contains("CREDIT NOTE INPUT") Then
                Selectedfilter += "CRI,"
            End If
            If ChkcmbFilter.Text.Contains("CREDIT NOTE OUTPUT") Then
                Selectedfilter += "CRO,"
            End If
            If ChkcmbFilter.Text.Contains("DEBIT NOTE INPUT") Then
                Selectedfilter += "DNI,"
            End If
            If ChkcmbFilter.Text.Contains("DEBIT NOTE OUTPUT") Then
                Selectedfilter += "DNO,"
            End If
            If ChkcmbFilter.Text.Contains("JOB WORK RECEIPT") Then
                Selectedfilter += "RRE,"
            End If
            If ChkcmbFilter.Text.Contains(".JOB WORK ISSUE") Then
                Selectedfilter += "IIS,"
            End If
            If ChkcmbFilter.Text.Contains("SALES.") Then
                Selectedfilter += "SA,"
            End If
            If ChkcmbFilter.Text.Contains("SALES RETURN") Then
                Selectedfilter += "SR,"
            End If
            If ChkcmbFilter.Text.Contains("EXPORT SALES") Then
                Selectedfilter += "EX,"
            End If
            If ChkcmbFilter.Text.Contains("REPAIR DELIVERY") Then
                Selectedfilter += "RD,"
            End If
            If ChkcmbFilter.Text.Contains("ADVANCE ADJUSTED.") Then
                Selectedfilter += "AA,"
            End If
            If ChkcmbFilter.Text.Contains("ADVANCE RECEIVED.") Then
                Selectedfilter += "AR,"
            End If
            If ChkcmbFilter.Text.Contains("SAVINGS COLLECTION") Then
                Selectedfilter += "CC,"
            End If
            If ChkcmbFilter.Text.Contains("SAVINGS CLOSED") Then
                Selectedfilter += "SS,"
            End If
            If ChkcmbFilter.Text.Contains("ORDER ADJUSTED") Then
                Selectedfilter += "OP,"
            End If
            If ChkcmbFilter.Text.Contains("ORDER ADVANCE") Then
                Selectedfilter += "OR,"
            End If
            If ChkcmbFilter.Text.Contains("ORDER & ADVANCE ADJUSTED") Then
                Selectedfilter += "XXXX,"
            End If
            If ChkcmbFilter.Text.Contains("ORDER & ADVANCE RECEIVED") Then
                Selectedfilter += "ZZZZ,"
            End If
            If Selectedfilter <> "" Then
                Selectedfilter = Mid(Selectedfilter, 1, Selectedfilter.Length - 1)
                Selectedfilter += ""
            End If
            If ChitDb <> "" Then
                strSql = "SELECT CTLTEXT FROM " & ChitDb & "SAVINGS..SOFTCONTROL WHERE CTLID='GSTCALC'"
                GstCalc = objGPack.GetSqlValue(strSql, "CTLTEXT", "N").ToString
            End If
            strSql = " EXEC " & cnAdminDb & "..PROC_GSTR3B_MULTI"
            strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
            strSql += vbCrLf + " ,@FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@ADVDATE = '" & dtpAdv_OWN.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " ,@METALNAME = '" & cmbMetal.Text & "'"
            strSql += vbCrLf + " ,@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
            strSql += vbCrLf + " ,@ENTRYTYPE = '" & Selectedfilter & "'"
            strSql += vbCrLf + " ,@CHITDB = '" & ChitDb & "'"
            strSql += vbCrLf + " ,@GSTCALC = '" & GstCalc & "'"
            strSql += vbCrLf + " ,@FORMAT1 = '" & GST3B_F1.ToString & "'"
            strSql += vbCrLf + " ,@SALESTYPE = '" & cmbType.Text & "'"
            strSql += vbCrLf + " ,@RPTTYPE = 'M'"
            If chkcancel.Checked = True Then
                strSql += vbCrLf + " ,@WITHCANCEL = 'Y'"
            Else
                strSql += vbCrLf + " ,@WITHCANCEL = 'N'"
            End If
            If Chkonlycancel.Checked = True Then
                strSql += vbCrLf + " ,@ONLYCANCEL = 'Y'"
            Else
                strSql += vbCrLf + " ,@ONLYCANCEL = 'N'"
            End If
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            If Selectedfilter = "CC" Then
                strSql = "SELECT  ROW_NUMBER() OVER (PARTITION BY TRANNO ORDER BY TRANNO)SNO,[MON] MONTH,[COSTCODE]COSTID,[HSNCODE]HSN,[INVDATE],[TRANNO],[REFNO]REFNO,[REFDATE]REFDATE,[PNAME],[PLACE][PALCE OF SUPPLY],CNAME[NATURE OF EXPENSES],NULL [TYPE],NULL OTHERS,"
                strSql += vbCrLf + "[GSTNO],[AMOUNT],[RATE],[ITAX],[CTAX],[STAX],(ISNULL(ITAX,0)+ISNULL(CTAX,0)+ISNULL(STAX,0))[TOTAL GST],[GNETAMOUNT],NULL [CANCEL] "
            Else
                strSql = "SELECT  ROW_NUMBER() OVER (PARTITION BY S_TYPE ORDER BY S_TYPE)SNO,[MON] MONTH,[COSTCODE]COSTID,[HSNCODE]HSN,[INVDATE],[TRANNO],[REFNO]REFNO,[REFDATE]REFDATE,[PNAME],[PLACE][PALCE OF SUPPLY],CNAME[NATURE OF EXPENSES],S_TYPE [TYPE],OTHERS,"
                strSql += vbCrLf + "[GSTNO],[AMOUNT],[RATE],[ITAX],[CTAX],[STAX],(ISNULL(ITAX,0)+ISNULL(CTAX,0)+ISNULL(STAX,0))[TOTAL GST],[GNETAMOUNT],[CANCEL] "
            End If


            strSql += vbCrLf + ",[RUNNO],[RECDATE],[AMOUNTA],[AMOUNTB],[TCS],[GNETAMOUNT],[CRCMTAX],[SRCMTAX],[IRCMTAX],[RNETAMOUNT],[DOORNO],[ADDRESS1],[ADDRESS2],[ADDRESS3]  "
            strSql += vbCrLf + " ,[AREA],[CITY],[STATE],[COUNTRY],[PINCODE],[PHONE],[MOBILE],[EMAIL],[BATCHNO],[RESULT] ,[COLHEAD] FROM TEMPTABLEDB..TEMPGSTRPT_MULTI"
            strSql += vbCrLf + " ORDER BY TYPE,RESULT,INVDATE,TRANNO"

            Dim dtGrid As New DataTable
            dtGrid.Columns.Add("KEYNO", GetType(Integer))
            dtGrid.Columns("KEYNO").AutoIncrement = True
            dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
            dtGrid.Columns("KEYNO").AutoIncrementStep = 1
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGrid)
            If Not dtGrid.Rows.Count > 0 Then
                MsgBox("Record Not Found", MsgBoxStyle.Information)
                dtpFrom.Select()
                Exit Sub
            End If
            dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
            gridView.DataSource = dtGrid
            FormatGridColumns(gridView, False, False, True, False)

            Dim dt As New DataTable
            dt = gridView.DataSource
            dt.AcceptChanges()
            Dim ros() As DataRow = Nothing

            ros = dt.Select("COLHEAD = 'G'")
            For cnt As Integer = 0 To ros.Length - 1
                gridView.Rows(Val(ros(cnt).Item("KEYNO").ToString)).DefaultCellStyle = reportTotalStyle
            Next
            ros = dt.Select("COLHEAD = 'T'")
            For cnt As Integer = 0 To ros.Length - 1
                gridView.Rows(Val(ros(cnt).Item("KEYNO").ToString)).Cells("PNAME").Style = reportHeadStyle
            Next
            ros = dt.Select("COLHEAD = 'S'")
            For cnt As Integer = 0 To ros.Length - 1
                gridView.Rows(Val(ros(cnt).Item("KEYNO").ToString)).DefaultCellStyle = reportSubTotalStyle
            Next
            With gridView
                .Columns("BATCHNO").Visible = False
                .Columns("KEYNO").Visible = False
                .Columns("RESULT").Visible = False
                If .Columns.Contains("AMOUNT") Then .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("TRANNO1") Then .Columns("TRANNO1").Visible = False
                If .Columns.Contains("GSTNO1") Then .Columns("GSTNO1").Visible = False
                .Columns("COLHEAD").Visible = False
                .Columns("TRANNO").HeaderText = "TRANNO"
                .Columns("ITAX").HeaderText = "IGST"
                .Columns("CTAX").HeaderText = "CGST"
                .Columns("STAX").HeaderText = "SGST"
                .Columns("IRCMTAX").HeaderText = "RCM IGST"
                .Columns("CRCMTAX").HeaderText = "RCM CGST"
                .Columns("SRCMTAX").HeaderText = "RCM SGST"
                .Columns("PNAME").HeaderText = "AC NAME"
                .Columns("INVDATE").HeaderText = "TRANDATE"
                .Columns("AMOUNT").HeaderText = "TAXABLE VALUE"
                .Columns("GNETAMOUNT").HeaderText = "TOTALAMOUNT"
                .Columns("COSTID").HeaderText = "COSTID"
                If .Columns.Contains("MODEPAY") Then .Columns("MODEPAY").HeaderText = "PayMode"
                If .Columns.Contains("PLACE") Then .Columns("PLACE").HeaderText = "PlaceOfSupply"
                .Columns("INVDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
                .Columns("IRCMTAX").Visible = False
                .Columns("CRCMTAX").Visible = False
                .Columns("SRCMTAX").Visible = False
                .Columns("RNETAMOUNT").Visible = False
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                .Invalidate()
                For Each dgvCol As DataGridViewColumn In .Columns
                    dgvCol.Width = dgvCol.Width
                Next
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End With
            FormatGridColumns(gridViewHead, False, False, True, False)
            Dim TITLE As String = ""
            TITLE += " GST REPORT  FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
            TITLE += "  AT " & chkCmbCostCentre.Text & ""
            lblTitle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            lblTitle.Text = TITLE
            pnlHeading.Visible = True
            btnView_Search.Enabled = True
            gridView.Focus()
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & ex.StackTrace, MsgBoxStyle.Information) : Exit Sub
        End Try
    End Sub
    Function GetSelectedCompanyId(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "" & strCompanyId & ""
        End If
        Return retStr
    End Function

    Function funcColWidth() As Integer
        With gridViewHead
            Dim ColWidth As Integer
            Dim ColWidth1 As Integer
            If gridView.Columns.Contains("AMOUNTA") Then
                ColWidth = gridView.Columns("AMOUNTA").Width
                gridView.Columns("AMOUNTA").HeaderText = "AfterGST"
            End If
            If gridView.Columns.Contains("AMOUNTB") Then
                ColWidth += gridView.Columns("AMOUNTB").Width
                gridView.Columns("AMOUNTB").HeaderText = "BeforeGST"
            End If
            If gridView.Columns.Contains("CNAME") Then
                ColWidth += gridView.Columns("CNAME").Width
                gridView.Columns("CNAME").HeaderText = "Contra Name"
            End If
            If gridView.Columns.Contains("RATE") Then
                ColWidth += gridView.Columns("RATE").Width
                gridView.Columns("RATE").HeaderText = "RATE"
            End If
            If gridView.Columns.Contains("RECDATE") Then
                ColWidth += gridView.Columns("RECDATE").Width
                gridView.Columns("RECDATE").HeaderText = "Date"
            End If
            If gridView.Columns.Contains("RUNNO") Then
                ColWidth += gridView.Columns("RUNNO").Width
                gridView.Columns("RUNNO").HeaderText = "RunNo"
            End If
            If cmbFilter.Text = "ADVANCE RECEIVED" Then
                gridView.Columns("AMOUNTA").HeaderText = "Amount"
            ElseIf cmbFilter.Text = "ORDER ADVANCE" Then
                gridView.Columns("AMOUNTA").HeaderText = "Amount"
            ElseIf cmbFilter.Text = "ORDER & ADVANCE RECEIVED" Then
                gridView.Columns("AMOUNTA").HeaderText = "Amount"
            End If
            If gridView.Columns.Contains("MODEPAY") Then
                ColWidth1 = gridView.Columns("MODEPAY").Width
            End If
            If gridView.Columns.Contains("COSTID") Then
                ColWidth1 += gridView.Columns("COSTID").Width
            End If
            If gridView.Columns.Contains("INSTALLMENT") Then
                ColWidth1 += gridView.Columns("INSTALLMENT").Width
            End If
            If gridView.Columns.Contains("COSTCODE") Then
                ColWidth1 += gridView.Columns("COSTCODE").Width
            End If
            If gridView.Columns.Contains("RUNNO") Or gridView.Columns.Contains("RECDATE") Or gridView.Columns.Contains("AMOUNTA") Or gridView.Columns.Contains("AMOUNTB") Then
                .Columns("RUNNO~RECDATE~AMOUNTA~AMOUNTB").Width = ColWidth
            End If
            If gridView.Columns.Contains("TCS") Then
                .Columns("CTAX~STAX~ITAX~TCS").Width = gridView.Columns("CTAX").Width _
                + gridView.Columns("STAX").Width _
                + gridView.Columns("ITAX").Width _
                + gridView.Columns("TCS").Width
            Else
                .Columns("CTAX~STAX~ITAX").Width = gridView.Columns("CTAX").Width _
                + gridView.Columns("STAX").Width _
                + gridView.Columns("ITAX").Width
            End If
            .Columns("GNETAMOUNT").Width = gridView.Columns("GNETAMOUNT").Width + gridView.Columns("RATE").Width
            With .Columns(IIf(gridView.Columns.Contains("PLACE"), "PLACE~", "") + "DOORNO~ADDRESS1~ADDRESS2~ADDRESS3~AREA~CITY~STATE~COUNTRY~PINCODE~PHONE~MOBILE~EMAIL")
                .Width = gridView.Columns("DOORNO").Width
                .Width += gridView.Columns("ADDRESS1").Width + gridView.Columns("ADDRESS2").Width
                If gridView.Columns.Contains("PLACE") Then
                    .Width += gridView.Columns("PLACE").Width
                End If
                .Width += gridView.Columns("ADDRESS3").Width + gridView.Columns("AREA").Width + gridView.Columns("CITY").Width
                .Width += gridView.Columns("STATE").Width
                .Width += gridView.Columns("COUNTRY").Width + gridView.Columns("PINCODE").Width + gridView.Columns("PHONE").Width
                .Width += gridView.Columns("MOBILE").Width + +gridView.Columns("EMAIL").Width
            End With
            With .Columns("SCROLL")
                .HeaderText = ""
                .Width = 0
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            If gridView.Columns.Contains("RUNNO") Or gridView.Columns.Contains("RECDATE") Or gridView.Columns.Contains("AMOUNTA") Or gridView.Columns.Contains("AMOUNTB") Then
                With .Columns("RUNNO~RECDATE~AMOUNTA~AMOUNTB")
                    If ColWidth = 0 Then
                        .HeaderText = ""
                        .Width = 0
                        .SortMode = DataGridViewColumnSortMode.NotSortable
                    End If
                End With
            End If
        End With
    End Function
    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        lblStatus.Visible = False
        pnlHeading.Visible = False
        SalesAbs()
        Exit Sub
    End Sub
    Private Sub loadcombo(ByVal type As String, ByVal loadtype As String)
        cmbFilter.DataSource = Nothing
        cmbFilter.Items.Clear()
        If type = "I" Or type = "B" Then
            strSql = " SELECT 'PURCHASE RD' TITLE,'PR' ENTRYTYPE "
            strSql += " UNION ALL"
            strSql += " SELECT 'PURCHASE URD' TITLE,'PU' ENTRYTYPE "
            strSql += " UNION ALL"
            strSql += " SELECT 'CREDIT NOTE INPUT' TITLE,'CRI' ENTRYTYPE "
            strSql += " UNION ALL"
            strSql += " SELECT 'DEBIT NOTE INPUT' TITLE,'DNI' ENTRYTYPE "
            strSql += " UNION ALL"
            strSql += " SELECT 'JOB WORK RECEIPT' TITLE,'RRE' ENTRYTYPE "
            strSql += " UNION ALL"
            strSql += " SELECT '.JOB WORK ISSUE' TITLE,'IIS' ENTRYTYPE "
            strSql += " UNION ALL"
            strSql += " SELECT 'JOURNAL ENTRY' TITLE,'JE' ENTRYTYPE "
            strSql += " UNION ALL"
            strSql += " SELECT 'PURCHASE RETURN' TITLE,'PIR' ENTRYTYPE "
        End If
        If type = "O" Or type = "B" Then
            If type = "B" Then
                strSql += " UNION ALL"
                strSql += " SELECT 'SALES.' TITLE,'SA' ENTRYTYPE "
                strSql += " UNION ALL"
                strSql += " SELECT 'SALES RETURN' TITLE,'SR' ENTRYTYPE "
                strSql += " UNION ALL"
                strSql += " SELECT 'EXPORT SALES' TITLE,'EX' ENTRYTYPE "
            Else
                strSql = " SELECT 'SALES.' TITLE,'SA' ENTRYTYPE "
                strSql += " UNION ALL"
                strSql += " SELECT 'SALES RETURN' TITLE,'SR' ENTRYTYPE "
                strSql += " UNION ALL"
                strSql += " SELECT 'EXPORT SALES' TITLE,'EX' ENTRYTYPE "
            End If
            strSql += " UNION ALL"
            strSql += " SELECT 'CREDIT NOTE OUTPUT' TITLE,'CRO' ENTRYTYPE "
            strSql += " UNION ALL"
            strSql += " SELECT 'DEBIT NOTE OUTPUT' TITLE,'DNO' ENTRYTYPE "
            strSql += " UNION ALL"
            strSql += " SELECT 'REPAIR DELIVERY' TITLE,'RD' ENTRYTYPE "
            strSql += " UNION ALL"
            strSql += " SELECT 'ADVANCE ADJUSTED.' TITLE,'AA' ENTRYTYPE "
            strSql += " UNION ALL"
            strSql += " SELECT 'ADVANCE RECEIVED.' TITLE,'AR' ENTRYTYPE "
            If ChitDb <> "" Then
                strSql += " UNION ALL"
                strSql += " SELECT 'SAVINGS COLLECTION' TITLE,'CC' ENTRYTYPE "
                strSql += " UNION ALL"
                strSql += " SELECT 'SAVINGS CLOSED' TITLE,'SS' ENTRYTYPE "
            End If
            strSql += " UNION ALL"
            strSql += " SELECT 'ORDER ADJUSTED' TITLE,'OP' ENTRYTYPE "
            strSql += " UNION ALL"
            strSql += " SELECT 'ORDER ADVANCE' TITLE,'OR' ENTRYTYPE "
            strSql += " UNION ALL"
            strSql += " SELECT 'ORDER & ADVANCE ADJUSTED' TITLE,'AAAA' ENTRYTYPE "
            strSql += " UNION ALL"
            strSql += " SELECT 'ORDER & ADVANCE RECEIVED' TITLE,'ORAR' ENTRYTYPE "
        End If
        Dim dtFilter As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtFilter)
        BrighttechPack.GlobalMethods.FillCombo(ChkcmbFilter, dtFilter, "TITLE", , "")
        cmbFilter.Items.Clear()
        cmbFilter.DataSource = dtFilter
        cmbFilter.DisplayMember = "TITLE"
        cmbFilter.ValueMember = "ENTRYTYPE"
        cmbFilter.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbFilter.AutoCompleteSource = AutoCompleteSource.ListItems
        If loadtype = "M" Then
            cmbFilter.Enabled = False
        ElseIf loadtype = "S" Then
            ChkcmbFilter.Enabled = False
        End If
    End Sub
    Private Sub frmGSTR3B_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmSalesAbstract_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Chit <> "Y" Then
            ChitDb = ""
        End If
        rbtmulti.Checked = True
        rbtBothMU.Checked = True
        cmbMetal.Items.Clear()
        cmbMetal.Items.Add("ALL")
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY METALNAME"
        objGPack.FillCombo(strSql, cmbMetal, False)
        cmbMetal.Text = "ALL"
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE "
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        cmbFilter.DataSource = Nothing
        cmbFilter.Items.Clear()
        strSql = " SELECT 'JOURNAL ENTRY' TITLE,'JE' ENTRYTYPE "
        strSql += " UNION ALL"
        strSql += " SELECT 'CREDIT NOTE INPUT' TITLE,'CRI' ENTRYTYPE "
        strSql += " UNION ALL"
        strSql += " SELECT 'DEBIT NOTE INPUT' TITLE,'DNI' ENTRYTYPE "
        strSql += " UNION ALL"
        strSql += " SELECT 'CREDIT NOTE OUTPUT' TITLE,'CRO' ENTRYTYPE "
        strSql += " UNION ALL"
        strSql += " SELECT 'DEBIT NOTE OUTPUT' TITLE,'DNO' ENTRYTYPE "
        strSql += " UNION ALL"
        strSql += " SELECT 'SALES.' TITLE,'SA' ENTRYTYPE "
        strSql += " UNION ALL"
        strSql += " SELECT 'REPAIR DELIVERY' TITLE,'RD' ENTRYTYPE "
        strSql += " UNION ALL"
        strSql += " SELECT 'PURCHASE RD' TITLE,'PR' ENTRYTYPE "
        strSql += " UNION ALL"
        strSql += " SELECT 'PURCHASE URD' TITLE,'PU' ENTRYTYPE "
        strSql += " UNION ALL"
        strSql += " SELECT 'PURCHASE RETURN' TITLE,'PIR' ENTRYTYPE "
        strSql += " UNION ALL"
        strSql += " SELECT 'ADVANCE ADJUSTED' TITLE,'AA' ENTRYTYPE "
        strSql += " UNION ALL"
        strSql += " SELECT 'ADVANCE RECEIVED' TITLE,'AR' ENTRYTYPE "
        strSql += " UNION ALL"
        strSql += " SELECT 'ORDER ADJUSTED' TITLE,'OP' ENTRYTYPE "
        strSql += " UNION ALL"
        strSql += " SELECT 'ORDER ADVANCE' TITLE,'OR' ENTRYTYPE "
        strSql += " UNION ALL"
        strSql += " SELECT 'JOB WORK RECEIPT' TITLE,'RRE' ENTRYTYPE "
        strSql += " UNION ALL"
        strSql += " SELECT 'JOB WORK ISSUE' TITLE,'IIS' ENTRYTYPE "
        strSql += " UNION ALL"
        strSql += " SELECT 'SALES RETURN' TITLE,'SR' ENTRYTYPE "
        strSql += " UNION ALL"
        strSql += " SELECT 'EXPORT SALES' TITLE,'EX' ENTRYTYPE "
        If ChitDb <> "" Then
            strSql += " UNION ALL"
            strSql += " SELECT 'SAVINGS COLLECTION' TITLE,'CC' ENTRYTYPE "
            strSql += " UNION ALL"
            strSql += " SELECT 'SAVINGS CLOSED' TITLE,'SS' ENTRYTYPE "
        End If
        strSql += " UNION ALL"
        strSql += " SELECT 'ORDER & ADVANCE ADJUSTED' TITLE,'AAAA' ENTRYTYPE "
        strSql += " UNION ALL"
        strSql += " SELECT 'ORDER & ADVANCE RECEIVED' TITLE,'ORAR' ENTRYTYPE "
        strSql += " ORDER BY TITLE"
        Dim dtFilter As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtFilter)
        BrighttechPack.GlobalMethods.FillCombo(ChkcmbFilter, dtFilter, "TITLE", , "")
        cmbFilter.DataSource = dtFilter
        cmbFilter.DisplayMember = "TITLE"
        cmbFilter.ValueMember = "ENTRYTYPE"
        cmbFilter.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cmbFilter.AutoCompleteSource = AutoCompleteSource.ListItems
        cmbType.Items.Add("ALL")
        cmbType.Items.Add("B2B")
        cmbType.Items.Add("B2C")
        cmbType.Text = "ALL"
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

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Dim GstDate As Date = GetAdmindbSoftValue("GSTDATE", "")
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        dtpAdv_OWN.Value = GstDate
        pnlHeading.Visible = False
        gridView.DataSource = Nothing
        btnView_Search.Enabled = True
        lblStatus.Visible = False
        Prop_Gets()
        dtpAdv_OWN.Value = New Date(2017, 7, 1)
        dtpFrom.Focus()
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, gridViewHead)
        End If
    End Sub

    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView.ColumnWidthChanged
        If gridViewHead.ColumnCount > 0 Then
            funcColWidth()
        End If
    End Sub
    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExcel_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, gridViewHead)
        End If
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmGSTR3BNEW_Properties
        SetSettingsObj(obj, Me.Name, GetType(frmGSTR3BNEW_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmGSTR3BNEW_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmGSTR3BNEW_Properties))
    End Sub

    Private Sub AutoReziseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoReziseToolStripMenuItem.Click
        If gridView.RowCount > 0 Then
            If AutoReziseToolStripMenuItem.Checked Then
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridView.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
        End If
    End Sub

    Private Sub gridView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView.Scroll
        If gridViewHead Is Nothing Then Exit Sub
        If Not gridViewHead.Columns.Count > 0 Then Exit Sub
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridViewHead.HorizontalScrollingOffset = e.NewValue
        End If
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridViewHead.HorizontalScrollingOffset = e.NewValue
                gridViewHead.Columns("SCROLL").Visible = CType(gridViewHead.Controls(0), HScrollBar).Visible
                gridViewHead.Columns("SCROLL").Width = CType(gridViewHead.Controls(1), VScrollBar).Width
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub

    Private Sub cmbFilter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbFilter.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cmbFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbFilter.SelectedIndexChanged
        If cmbFilter.Text = "ADVANCE ADJUSTED" Then
            lblAdv.Text = "Advance Received After"
            PnlAdv.Visible = True
        ElseIf cmbFilter.Text = "ORDER ADJUSTED" Then
            lblAdv.Text = "Order Received After"
            PnlAdv.Visible = True
        ElseIf cmbFilter.Text = "ORDER & ADVANCE ADJUSTED" Then
            lblAdv.Text = "Ord && Adv Received After"
            PnlAdv.Visible = True
        ElseIf cmbFilter.Text = "SAVINGS CLOSED" Then
            lblAdv.Text = "Chit Received After"
            PnlAdv.Visible = True
        ElseIf cmbFilter.Text = "SALES" Then
            PnlSaleType.Visible = True
            PnlAdv.Visible = False
        Else
            PnlAdv.Visible = False
            PnlSaleType.Visible = False
        End If
    End Sub
    Private Sub rbtBothMU_CheckedChanged(sender As Object, e As EventArgs) Handles rbtBothMU.CheckedChanged
        If rbtBothMU.Checked = True Then
            rbtInput.Checked = False
            rbtOutput.Checked = False
            If rbtmulti.Checked = True Then
                loadcombo("B", "M")
            Else
                loadcombo("B", "S")
            End If
        End If
    End Sub

    Private Sub rbtInput_CheckedChanged(sender As Object, e As EventArgs) Handles rbtInput.CheckedChanged
        If rbtInput.Checked = True Then
            rbtBothMU.Checked = False
            rbtOutput.Checked = False
            If rbtmulti.Checked = True Then
                loadcombo("I", "M")
            Else
                loadcombo("I", "S")
            End If
        End If
    End Sub

    Private Sub rbtOutput_CheckedChanged(sender As Object, e As EventArgs) Handles rbtOutput.CheckedChanged
        If rbtOutput.Checked = True Then
            rbtBothMU.Checked = False
            rbtInput.Checked = False
            If rbtmulti.Checked = True Then
                loadcombo("O", "M")
            Else
                loadcombo("O", "S")
            End If
        End If
    End Sub

    Private Sub rbtsingle_CheckedChanged(sender As Object, e As EventArgs) Handles rbtsingle.CheckedChanged
        If rbtsingle.Checked = True Then
            rbtmulti.Checked = False
            ChkcmbFilter.Enabled = False
            cmbFilter.Enabled = True
        End If
    End Sub

    Private Sub rbtmulti_CheckedChanged(sender As Object, e As EventArgs) Handles rbtmulti.CheckedChanged
        If rbtmulti.Checked = True Then
            rbtsingle.Checked = False
            cmbFilter.Enabled = False
            ChkcmbFilter.Enabled = True
        End If
    End Sub

    Private Sub Chkonlycancel_CheckedChanged(sender As Object, e As EventArgs) Handles Chkonlycancel.CheckedChanged
        If Chkonlycancel.Checked = True Then
            chkcancel.Enabled = False
        Else
            chkcancel.Enabled = True
        End If
    End Sub
End Class

Public Class frmGSTR3BNEW_Properties
    Private chkPcs As Boolean = True
    Public Property p_chkPcs() As Boolean
        Get
            Return chkPcs
        End Get
        Set(ByVal value As Boolean)
            chkPcs = value
        End Set
    End Property
    Private chkGrsWt As Boolean = True
    Public Property p_chkGrsWt() As Boolean
        Get
            Return chkGrsWt
        End Get
        Set(ByVal value As Boolean)
            chkGrsWt = value
        End Set
    End Property
    Private chkNetWt As Boolean = True
    Public Property p_chkNetWt() As Boolean
        Get
            Return chkNetWt
        End Get
        Set(ByVal value As Boolean)
            chkNetWt = value
        End Set
    End Property
    Private chkWithSR As Boolean = False
    Public Property p_chkWithSR() As Boolean
        Get
            Return chkWithSR
        End Get
        Set(ByVal value As Boolean)
            chkWithSR = value
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
    Private chkVA As Boolean = False
    Public Property p_chkVA() As Boolean
        Get
            Return chkVA
        End Get
        Set(ByVal value As Boolean)
            chkVA = value
        End Set
    End Property
    Private cmbMetal As String = "ALL"
    Public Property p_cmbMetal() As String
        Get
            Return cmbMetal
        End Get
        Set(ByVal value As String)
            cmbMetal = value
        End Set
    End Property
    Private cmbCategory As String = "ALL"
    Public Property p_cmbCategory() As String
        Get
            Return cmbCategory
        End Get
        Set(ByVal value As String)
            cmbCategory = value
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
    Private rbtMonth As Boolean = False
    Public Property p_rbtMonth() As Boolean
        Get
            Return rbtMonth
        End Get
        Set(ByVal value As Boolean)
            rbtMonth = value
        End Set
    End Property
    Private rbtDate As Boolean = False
    Public Property p_rbtDate() As Boolean
        Get
            Return rbtDate
        End Get
        Set(ByVal value As Boolean)
            rbtDate = value
        End Set
    End Property
    Private rbtBillNo As Boolean = False
    Public Property p_rbtBillNo() As Boolean
        Get
            Return rbtBillNo
        End Get
        Set(ByVal value As Boolean)
            rbtBillNo = value
        End Set
    End Property
    Private chkBillPrefix As Boolean = False
    Public Property p_chkBillPrefix() As Boolean
        Get
            Return chkBillPrefix
        End Get
        Set(ByVal value As Boolean)
            chkBillPrefix = value
        End Set
    End Property
    Private cmbFilter As String = "ALL"
    Public Property p_cmbFilter() As String
        Get
            Return cmbFilter
        End Get
        Set(ByVal value As String)
            cmbFilter = value
        End Set
    End Property
End Class