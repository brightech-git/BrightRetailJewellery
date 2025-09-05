Imports System.Data.OleDb
Imports System.Threading
Imports System.IO
Public Class frmGSTR3B

    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim dsGridView As New DataSet
    Dim dtCostCentre As New DataTable
    Dim ChitDb As String = GetAdmindbSoftValue("CHITDBPREFIX", "")
    Dim Chit As String = GetAdmindbSoftValue("CHITDB", "N")
    Dim GST3B_F1 As String = GetAdmindbSoftValue("GST3B_F1", "N")
    Private Sub SalesAbs()
        Try
            Prop_Sets()
            gridView.DataSource = Nothing
            gridViewHead.DataSource = Nothing
            gridView.Refresh()
            gridViewHead.Refresh()
            Dim Sysid As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
            Me.Refresh()
            Dim GstCalc As String = "N"
            If ChitDb <> "" Then
                strSql = "SELECT CTLTEXT FROM " & ChitDb & "SAVINGS..SOFTCONTROL WHERE CTLID='GSTCALC'"
                GstCalc = objGPack.GetSqlValue(strSql, "CTLTEXT", "N").ToString
            End If
            strSql = " EXEC " & cnAdminDb & "..PROC_GSTR3B"
            strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
            strSql += vbCrLf + " ,@FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@ADVDATE = '" & dtpAdv_OWN.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,@COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " ,@METALNAME = '" & cmbMetal.Text & "'"
            strSql += vbCrLf + " ,@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
            strSql += vbCrLf + " ,@ENTRYTYPE = '" & cmbFilter.SelectedValue.ToString() & "'"
            strSql += vbCrLf + " ,@CHITDB = '" & ChitDb & "'"
            strSql += vbCrLf + " ,@GSTCALC = '" & GstCalc & "'"
            strSql += vbCrLf + " ,@FORMAT1 = '" & GST3B_F1.ToString & "'"
            strSql += vbCrLf + " ,@SALESTYPE = '" & cmbType.Text & "'"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = "SELECT * FROM TEMPTABLEDB..TEMPGSTRPT "
            If cmbFilter.Text = "ADVANCE ADJUSTED" Or cmbFilter.Text = "ORDER ADJUSTED" Or cmbFilter.Text = "ORDER & ADVANCE ADJUSTED" Then
                strSql += vbCrLf + " ORDER BY HSNCODE,RESULT,INVDATE,TRANNO"
            ElseIf cmbFilter.Text = "SAVINGS COLLECTION" Then
                strSql += vbCrLf + " ORDER BY GSTNO1,RESULT,INVDATE,TRANNO1"
            ElseIf cmbFilter.Text = "SAVINGS CLOSED" Then
                strSql += vbCrLf + " ORDER BY BATCHNO,RESULT,INVDATE,TRANNO"
            Else
                strSql += vbCrLf + " ORDER BY RESULT,INVDATE,TRANNO"
            End If
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
                '.Columns("SNO").Visible = False
                .Columns("COSTID").Visible = False
                .Columns("KEYNO").Visible = False
                .Columns("RESULT").Visible = False
                If .Columns.Contains("ACCODE") Then .Columns("ACCODE").Visible = False
                If .Columns.Contains("AMOUNT") Then .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
                If .Columns.Contains("TRANNO1") Then .Columns("TRANNO1").Visible = False
                If .Columns.Contains("GSTNO1") Then .Columns("GSTNO1").Visible = False
                .Columns("COLHEAD").Visible = False
                .Columns("TRANNO").HeaderText = "INVNO"
                .Columns("ITAX").HeaderText = "IGST"
                .Columns("CTAX").HeaderText = "CGST"
                .Columns("STAX").HeaderText = "SGST"
                .Columns("IRCMTAX").HeaderText = "RCM IGST"
                .Columns("CRCMTAX").HeaderText = "RCM CGST"
                .Columns("SRCMTAX").HeaderText = "RCM SGST"
                .Columns("PNAME").HeaderText = "Customer Name"
                If cmbFilter.SelectedValue.ToString() = "JE" Then .Columns("CNAME").HeaderText = "Contra Name"
                .Columns("INVDATE").HeaderText = "Date"
                .Columns("AMOUNT").HeaderText = "Amount"
                .Columns("GNETAMOUNT").HeaderText = "Amount"
                .Columns("COSTID").HeaderText = "CostId"
                If .Columns.Contains("MODEPAY") Then .Columns("MODEPAY").HeaderText = "PayMode"
                If .Columns.Contains("PLACE") Then .Columns("PLACE").HeaderText = "PlaceOfSupply"
                '.Columns("RNETAMOUNT").HeaderText = "Net Amount"
                .Columns("INVDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
                .Columns("IRCMTAX").Visible = False
                .Columns("CRCMTAX").Visible = False
                .Columns("SRCMTAX").Visible = False
                .Columns("RNETAMOUNT").Visible = False
                If cmbFilter.Text = "SAVINGS COLLECTION" Then
                    .Columns("COSTID").Visible = True
                    .Columns("HSNCODE").HeaderText = "RegNo"
                    .Columns("GSTNO").HeaderText = "GroupCode"
                    .Columns("TRANNO").HeaderText = "ReceiptNo"
                    .Columns("INSTALLMENT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                ElseIf cmbFilter.Text = "SAVINGS CLOSED" Then
                    .Columns("HSNCODE").HeaderText = "RegNo"
                    .Columns("GSTNO").HeaderText = "GroupCode"
                    .Columns("AMOUNTA").DefaultCellStyle.BackColor = Color.Lavender
                    .Columns("AMOUNTB").DefaultCellStyle.BackColor = Color.Lavender
                ElseIf cmbFilter.Text = "ADVANCE RECEIVED" Or cmbFilter.Text = "ADVANCE ADJUSTED" Or cmbFilter.Text = "ORDER & ADVANCE ADJUSTED" Then
                    .Columns("HSNCODE").Visible = False
                    .Columns("GSTNO").Visible = False
                ElseIf cmbFilter.Text = "ORDER ADVANCE" Or cmbFilter.Text = "ORDER ADJUSTED" Or cmbFilter.Text = "ORDER & ADVANCE RECEIVED" Then
                    .Columns("HSNCODE").Visible = False
                    .Columns("GSTNO").Visible = False
                End If
                If cmbFilter.Text = "ADVANCE ADJUSTED" Or cmbFilter.Text = "ORDER ADJUSTED" Or cmbFilter.Text = "ORDER & ADVANCE ADJUSTED" Then
                    .Columns("RECDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
                    .Columns("RECDATE").DefaultCellStyle.BackColor = Color.Lavender
                    .Columns("AMOUNTA").DefaultCellStyle.BackColor = Color.Lavender
                    .Columns("AMOUNTB").DefaultCellStyle.BackColor = Color.Lavender
                    .Columns("RUNNO").DefaultCellStyle.BackColor = Color.Lavender
                ElseIf cmbFilter.Text = "ADVANCE RECEIVED" Or cmbFilter.Text = "ORDER ADVANCE" Or cmbFilter.Text = "ORDER & ADVANCE RECEIVED" Then
                    .Columns("RECDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
                    .Columns("RECDATE").DefaultCellStyle.BackColor = Color.Lavender
                    .Columns("AMOUNTA").DefaultCellStyle.BackColor = Color.Lavender
                    .Columns("RUNNO").DefaultCellStyle.BackColor = Color.Lavender
                End If
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                .Invalidate()
                For Each dgvCol As DataGridViewColumn In .Columns
                    dgvCol.Width = dgvCol.Width
                Next
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End With
            funcHeaderNew()
            FormatGridColumns(gridViewHead, False, False, True, False)
            Dim TITLE As String = ""

            TITLE += " GST REPORT FOR " & cmbFilter.Text & " FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
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
    Public Function funcHeaderNew() As Integer
        Dim REFSTRING As String
        Dim dtheader As New DataTable
        dtheader.Clear()
        If cmbFilter.SelectedValue.ToString() = "JE" Then
            REFSTRING = "~REFDATE~REFNO"
        Else
            REFSTRING = ""
        End If
        Try
            Dim dtMergeHeader As New DataTable("~MERGEHEADER")
            With dtMergeHeader
                If cmbFilter.SelectedValue.ToString() = "JE" Then
                    .Columns.Add("GSTNO~HSNCODE~PNAME~CNAME~INVDATE~TRANNO~INSTALLMENT~MODEPAY~COSTCODE~AMOUNT" & REFSTRING, GetType(String))
                ElseIf cmbFilter.SelectedValue.ToString() = "SA" Or cmbFilter.SelectedValue.ToString() = "RD" Then
                    .Columns.Add("GSTNO~HSNCODE~PNAME~INVDATE~TRANNO~INSTALLMENT~MODEPAY~COSTCODE~AMOUNT~RATE~MON~REFNO~REFDATE", GetType(String))
                Else
                    .Columns.Add("GSTNO~HSNCODE~PNAME~INVDATE~TRANNO~INSTALLMENT~MODEPAY~COSTCODE~AMOUNT~MON~REFNO~REFDATE", GetType(String))
                End If
                If gridView.Columns.Contains("RUNNO") Or gridView.Columns.Contains("RECDATE") Or gridView.Columns.Contains("AMOUNTA") Or gridView.Columns.Contains("AMOUNTB") Then
                    .Columns.Add("RUNNO~RECDATE~AMOUNTA~AMOUNTB", GetType(String))
                End If

                If gridView.Columns.Contains("TCS") Then
                    .Columns.Add("CTAX~STAX~ITAX~TCS", GetType(String))
                Else
                    .Columns.Add("CTAX~STAX~ITAX", GetType(String))
                End If

                '.Columns.Add("GNETAMOUNT", GetType(String))
                If cmbFilter.SelectedValue.ToString() = "JE" Then
                    .Columns.Add("GNETAMOUNT~RATE", GetType(String))
                Else
                    .Columns.Add("GNETAMOUNT", GetType(String))
                End If
                '.Columns.Add("CRCMTAX~SRCMTAX~IRCMTAX", GetType(String))
                '.Columns.Add("RNETAMOUNT", GetType(String))
                If gridView.Columns.Contains("PLACE") Then
                    .Columns.Add("PLACE~DOORNO~ADDRESS1~ADDRESS2~ADDRESS3~AREA~CITY~STATE~COUNTRY~PINCODE~PHONE~MOBILE~EMAIL", GetType(String))
                Else
                    .Columns.Add("DOORNO~ADDRESS1~ADDRESS2~ADDRESS3~AREA~CITY~STATE~COUNTRY~PINCODE~PHONE~MOBILE~EMAIL", GetType(String))
                End If
                If gridView.Columns.Contains("IRN") Then
                    .Columns.Add("IRN", GetType(String))
                End If
                If gridView.Columns.Contains("ACKNO") Then
                    .Columns.Add("ACKNO", GetType(String))
                End If
                .Columns.Add("SCROLL", GetType(String))
                If cmbFilter.SelectedValue.ToString() = "JE" Then
                    .Columns("GSTNO~HSNCODE~PNAME~CNAME~INVDATE~TRANNO~INSTALLMENT~MODEPAY~COSTCODE~AMOUNT" & REFSTRING).Caption = "PARTICULAR"
                ElseIf cmbFilter.SelectedValue.ToString() = "SA" Or cmbFilter.SelectedValue.ToString() = "RD" Then
                    .Columns("GSTNO~HSNCODE~PNAME~INVDATE~TRANNO~INSTALLMENT~MODEPAY~COSTCODE~AMOUNT~RATE~MON~REFNO~REFDATE").Caption = "PARTICULAR"
                Else
                    .Columns("GSTNO~HSNCODE~PNAME~INVDATE~TRANNO~INSTALLMENT~MODEPAY~COSTCODE~AMOUNT~MON~REFNO~REFDATE").Caption = "PARTICULAR"
                End If
                If gridView.Columns.Contains("TCS") Then
                    .Columns("CTAX~STAX~ITAX~TCS").Caption = "TAX"
                Else
                    .Columns("CTAX~STAX~ITAX").Caption = "TAX"
                End If

                '.Columns("GNETAMOUNT").Caption = "NETAMOUNT"
                If cmbFilter.SelectedValue.ToString() = "JE" Then
                    .Columns("GNETAMOUNT~RATE").Caption = "NETAMOUNT"
                Else
                    .Columns("GNETAMOUNT").Caption = "NETAMOUNT"
                End If
                '.Columns("CRCMTAX~SRCMTAX~IRCMTAX").Caption = "RCMGST"
                '.Columns("RNETAMOUNT").Caption = "NETAMOUNT"
                If gridView.Columns.Contains("PLACE") Then
                    .Columns("PLACE~DOORNO~ADDRESS1~ADDRESS2~ADDRESS3~AREA~CITY~STATE~COUNTRY~PINCODE~PHONE~MOBILE~EMAIL").Caption = "ADDRESS INFORMATION"
                Else
                    .Columns("DOORNO~ADDRESS1~ADDRESS2~ADDRESS3~AREA~CITY~STATE~COUNTRY~PINCODE~PHONE~MOBILE~EMAIL").Caption = "ADDRESS INFORMATION"
                End If
                If gridView.Columns.Contains("IRN") Then
                    .Columns("IRN").Caption = ""
                End If
                If gridView.Columns.Contains("ACKNO") Then
                    .Columns("ACKNO").Caption = ""
                End If

            End With
            With gridViewHead
                .DataSource = dtMergeHeader
                If cmbFilter.SelectedValue.ToString() = "JE" Then
                    .Columns("GSTNO~HSNCODE~PNAME~CNAME~INVDATE~TRANNO~INSTALLMENT~MODEPAY~COSTCODE~AMOUNT" & REFSTRING).HeaderText = "PARTICULAR"
                ElseIf cmbFilter.SelectedValue.ToString() = "SA" Or cmbFilter.SelectedValue.ToString() = "RD" Then
                    .Columns("GSTNO~HSNCODE~PNAME~INVDATE~TRANNO~INSTALLMENT~MODEPAY~COSTCODE~AMOUNT~RATE~MON~REFNO~REFDATE").HeaderText = "PARTICULAR"
                Else
                    .Columns("GSTNO~HSNCODE~PNAME~INVDATE~TRANNO~INSTALLMENT~MODEPAY~COSTCODE~AMOUNT~MON~REFNO~REFDATE").HeaderText = "PARTICULAR"
                End If
                If gridView.Columns.Contains("RUNNO") Or gridView.Columns.Contains("RECDATE") Or gridView.Columns.Contains("AMOUNTA") Or gridView.Columns.Contains("AMOUNTB") Then
                    .Columns("RUNNO~RECDATE~AMOUNTA~AMOUNTB").HeaderText = "ADVANCERECEIVED"
                End If
                If cmbFilter.Text = "SAVINGS CLOSED" Then
                    .Columns("RUNNO~RECDATE~AMOUNTA~AMOUNTB").HeaderText = "SAVINGSRECEIVED"
                ElseIf cmbFilter.Text = "ORDER ADJUSTED" Then
                    .Columns("RUNNO~RECDATE~AMOUNTA~AMOUNTB").HeaderText = "ORDERRECEIVED"
                ElseIf cmbFilter.Text = "ADVANCE RECEIVED" Then
                    .Columns("RUNNO~RECDATE~AMOUNTA~AMOUNTB").HeaderText = "ADVANCEADJUSTED"
                ElseIf cmbFilter.Text = "ORDER ADVANCE" Then
                    .Columns("RUNNO~RECDATE~AMOUNTA~AMOUNTB").HeaderText = "ORDERADJUSTED"
                ElseIf cmbFilter.Text = "ORDER & ADVANCE RECEIVED" Then
                    .Columns("RUNNO~RECDATE~AMOUNTA~AMOUNTB").HeaderText = "ADJUSTED"
                ElseIf cmbFilter.Text = "ORDER & ADVANCE ADJUSTED" Then
                    .Columns("RUNNO~RECDATE~AMOUNTA~AMOUNTB").HeaderText = "RECEIVED"
                End If
                If gridView.Columns.Contains("TCS") Then
                    .Columns("CTAX~STAX~ITAX~TCS").HeaderText = "TAX"
                Else
                    .Columns("CTAX~STAX~ITAX").HeaderText = "TAX"
                End If

                '.Columns("GNETAMOUNT").HeaderText = "AMOUNT"
                If cmbFilter.SelectedValue.ToString() = "JE" Then
                    .Columns("GNETAMOUNT~RATE").HeaderText = "AMOUNT"
                Else
                    .Columns("GNETAMOUNT").HeaderText = "AMOUNT"
                End If
                '.Columns("CRCMTAX~SRCMTAX~IRCMTAX").HeaderText = "RCMGST"
                '.Columns("RNETAMOUNT").HeaderText = "NETAMOUNT"
                If gridView.Columns.Contains("PLACE") Then
                    .Columns("PLACE~DOORNO~ADDRESS1~ADDRESS2~ADDRESS3~AREA~CITY~STATE~COUNTRY~PINCODE~PHONE~MOBILE~EMAIL").HeaderText = "ADDRESS INFORMATION"
                Else
                    .Columns("DOORNO~ADDRESS1~ADDRESS2~ADDRESS3~AREA~CITY~STATE~COUNTRY~PINCODE~PHONE~MOBILE~EMAIL").HeaderText = "ADDRESS INFORMATION"
                End If
                If gridView.Columns.Contains("IRN") Then
                    .Columns("IRN").HeaderText = ""
                End If
                If gridView.Columns.Contains("ACKNO") Then
                    .Columns("ACKNO").HeaderText = ""
                End If
                .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                funcColWidth()
                gridView.Focus()
                Dim colWid As Integer = 0
                For cnt As Integer = 0 To gridView.ColumnCount - 1
                    If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
                Next
                .Columns("SCROLL").Visible = False
            End With
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & ex.StackTrace, MsgBoxStyle.Information)
        End Try
    End Function
    Function funcColWidth() As Integer
        Dim REFSTRING As String
        If cmbFilter.SelectedValue.ToString() = "JE" Then
            REFSTRING = "~REFDATE~REFNO"
        Else
            REFSTRING = ""
        End If
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
            If cmbFilter.Text = "SAVINGS COLLECTION" Then
                If gridView.Columns.Contains("MODEPAY") Then
                    ColWidth1 = gridView.Columns("MODEPAY").Width
                End If
                If gridView.Columns.Contains("COSTID") Then
                    ColWidth1 += gridView.Columns("COSTID").Width
                End If
                If gridView.Columns.Contains("INSTALLMENT") Then
                    ColWidth1 += gridView.Columns("INSTALLMENT").Width
                End If
            End If
            If gridView.Columns.Contains("COSTCODE") Then
                ColWidth1 += gridView.Columns("COSTCODE").Width
            End If
            If cmbFilter.SelectedValue.ToString() = "JE" Then
                .Columns("GSTNO~HSNCODE~PNAME~CNAME~INVDATE~TRANNO~INSTALLMENT~MODEPAY~COSTCODE~AMOUNT" & REFSTRING).Width = gridView.Columns("PNAME").Width _
                + gridView.Columns("INVDATE").Width + gridView.Columns("AMOUNT").Width _
                + gridView.Columns("TRANNO").Width + gridView.Columns("CNAME").Width _
                + gridView.Columns("TRANNO").Width _
                + IIf(gridView.Columns("HSNCODE").Visible, gridView.Columns("HSNCODE").Width, 0) _
                + IIf(gridView.Columns("GSTNO").Visible, gridView.Columns("GSTNO").Width, 0) _
                + IIf(gridView.Columns.Contains("REFDATE"), gridView.Columns("REFDATE").Width, 0) _
                + IIf(gridView.Columns.Contains("REFNO"), gridView.Columns("REFNO").Width, 0) _
                + ColWidth1

            ElseIf cmbFilter.SelectedValue.ToString() = "SA" Or cmbFilter.SelectedValue.ToString() = "RD" Then
                .Columns("GSTNO~HSNCODE~PNAME~INVDATE~TRANNO~INSTALLMENT~MODEPAY~COSTCODE~AMOUNT~RATE~MON~REFNO~REFDATE").Width = gridView.Columns("PNAME").Width _
                + gridView.Columns("INVDATE").Width + gridView.Columns("RATE").Width + gridView.Columns("AMOUNT").Width _
                + gridView.Columns("TRANNO").Width _
                + IIf(gridView.Columns("HSNCODE").Visible, gridView.Columns("HSNCODE").Width, 0) _
                + IIf(gridView.Columns("GSTNO").Visible, gridView.Columns("GSTNO").Width, 0) _
                + IIf(gridView.Columns("MON").Visible, gridView.Columns("MON").Width, 0) _
                + IIf(gridView.Columns("REFNO").Visible, gridView.Columns("REFNO").Width, 0) _
                + IIf(gridView.Columns("REFDATE").Visible, gridView.Columns("REFDATE").Width, 0) _
                + ColWidth1
            Else
                .Columns("GSTNO~HSNCODE~PNAME~INVDATE~TRANNO~INSTALLMENT~MODEPAY~COSTCODE~AMOUNT~MON~REFNO~REFDATE").Width = gridView.Columns("PNAME").Width _
                + gridView.Columns("INVDATE").Width + gridView.Columns("AMOUNT").Width _
                + gridView.Columns("TRANNO").Width _
                + IIf(gridView.Columns("HSNCODE").Visible, gridView.Columns("HSNCODE").Width, 0) _
                + IIf(gridView.Columns("GSTNO").Visible, gridView.Columns("GSTNO").Width, 0) _
                + IIf(gridView.Columns("MON").Visible, gridView.Columns("MON").Width, 0) _
                + IIf(gridView.Columns("REFNO").Visible, gridView.Columns("REFNO").Width, 0) _
                + IIf(gridView.Columns("REFDATE").Visible, gridView.Columns("REFDATE").Width, 0) _
                + ColWidth1
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

            '.Columns("GNETAMOUNT").Width = gridView.Columns("GNETAMOUNT").Width
            If cmbFilter.SelectedValue.ToString() = "JE" Then
                .Columns("GNETAMOUNT~RATE").Width = gridView.Columns("GNETAMOUNT").Width + gridView.Columns("RATE").Width
            Else
                .Columns("GNETAMOUNT").Width = gridView.Columns("GNETAMOUNT").Width
            End If

            '.Columns("CRCMTAX~SRCMTAX~IRCMTAX").Width = gridView.Columns("CRCMTAX").Width _
            '    + gridView.Columns("SRCMTAX").Width _
            '    + gridView.Columns("IRCMTAX").Width            
            '.Columns("RNETAMOUNT").Width = gridView.Columns("RNETAMOUNT").Width

            With .Columns(IIf(gridView.Columns.Contains("PLACE"), "PLACE~", "") + "DOORNO~ADDRESS1~ADDRESS2~ADDRESS3~AREA~CITY~STATE~COUNTRY~PINCODE~PHONE~MOBILE~EMAIL")
                .Width = gridView.Columns("DOORNO").Width
                .Width += gridView.Columns("ADDRESS1").Width + gridView.Columns("ADDRESS2").Width
                '.Width += IIf(gridView.Columns.Contains("PLACE"), Val(gridView.Columns("PLACE").Width), 0)
                If gridView.Columns.Contains("PLACE") Then
                    .Width += gridView.Columns("PLACE").Width
                End If
                .Width += gridView.Columns("ADDRESS3").Width + gridView.Columns("AREA").Width + gridView.Columns("CITY").Width
                .Width += gridView.Columns("STATE").Width
                .Width += gridView.Columns("COUNTRY").Width + gridView.Columns("PINCODE").Width + gridView.Columns("PHONE").Width
                .Width += gridView.Columns("MOBILE").Width + +gridView.Columns("EMAIL").Width
            End With

            If .Columns.Contains("IRN") Then
                With .Columns("IRN")
                    .Width = gridView.Columns("IRN").Width
                End With
            End If
            If .Columns.Contains("ACKNO") Then
                With .Columns("ACKNO")
                    .Width = gridView.Columns("ACKNO").Width
                End With
            End If

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

    Private Sub frmGSTR3B_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmSalesAbstract_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Chit <> "Y" Then
            ChitDb = ""
        End If
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
        cmbFilter.Items.Clear()
        'cmbFilter.Items.Add("ALL")
        strSql = " SELECT 'JOURNAL ENTRY' TITLE,'JE' ENTRYTYPE "
        strSql += " UNION ALL"
        strSql += " SELECT 'CREDIT NOTE' TITLE,'CN' ENTRYTYPE "
        strSql += " UNION ALL"
        strSql += " SELECT 'DEBIT NOTE' TITLE,'DN' ENTRYTYPE "
        strSql += " UNION ALL"
        strSql += " SELECT 'SALES' TITLE,'SA' ENTRYTYPE "
        strSql += " UNION ALL"
        strSql += " SELECT 'REPAIR DELIVERY' TITLE,'RD' ENTRYTYPE "
        strSql += " UNION ALL"
        strSql += " SELECT 'PURCHASE RD' TITLE,'PR' ENTRYTYPE "
        strSql += " UNION ALL"
        strSql += " SELECT 'PURCHASE URD' TITLE,'PU' ENTRYTYPE "
        strSql += " UNION ALL"
        strSql += " SELECT 'ADVANCE ADJUSTED' TITLE,'AA' ENTRYTYPE "
        strSql += " UNION ALL"
        strSql += " SELECT 'ADVANCE RECEIVED' TITLE,'AR' ENTRYTYPE "
        strSql += " UNION ALL"
        strSql += " SELECT 'ORDER ADJUSTED' TITLE,'OP' ENTRYTYPE "
        strSql += " UNION ALL"
        strSql += " SELECT 'ORDER ADVANCE' TITLE,'OR' ENTRYTYPE "
        strSql += " UNION ALL"
        strSql += " SELECT 'JOB WORK' TITLE,'JW' ENTRYTYPE "
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

    Private Sub gridView_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridView.CellFormatting
        'Try
        '    Select Case e.ColumnIndex
        '        Case 5
        '            If e.RowIndex < 3 Then
        '                If Val(e.Value.ToString) = 0 Then
        '                    'gridSummary.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = ""
        '                End If
        '                If Val(e.Value.ToString) <> 0 Then
        '                    'gridSummary.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.BackColor = Color.Lavender
        '                    e.CellStyle.BackColor = Color.Lavender
        '                End If
        '            End If
        '        Case 6
        '            If e.RowIndex < 3 Then
        '                If Val(e.Value.ToString) = 0 Then
        '                    'gridSummary.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = ""
        '                End If
        '                If Val(e.Value.ToString) <> 0 Then
        '                    'gridSummary.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.BackColor = Color.Lavender
        '                    e.CellStyle.BackColor = Color.LavenderBlush
        '                End If
        '            End If
        '    End Select
        '    e.CellStyle.SelectionBackColor = e.CellStyle.BackColor
        '    e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor
        'Catch ex As Exception
        '    MsgBox("ERROR: " + ex.Message + vbCrLf + vbCrLf + "POSITION: " + ex.StackTrace, MsgBoxStyle.Critical, "ERROR MESSAGE")
        'End Try
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
        ElseIf UCase(e.KeyChar) = "G" Then
            If gridView.Item("GSTNO", gridView.CurrentRow.Index).Value.ToString = "" Then Exit Sub
            If GetAdmindbSoftValue("GST_EINV_OFFLINE_JSON", "N") = "Y" And GetAdmindbSoftValue("AUTOGSTDNCN", "N") = "Y" _
                And (cmbFilter.SelectedValue.ToString() = "DN" Or cmbFilter.SelectedValue.ToString() = "CN") Then
                If gridView.Item("IRN", gridView.CurrentRow.Index).Value.ToString <> "" Then
                    MsgBox("IRN already generated.")
                    Exit Sub
                End If
                strSql = " SELECT 1 CNT FROM " & cnStockDb & "..EINVTRAN WHERE BATCHNO ='" & gridView.Item("BatchNo", gridView.CurrentRow.Index).Value.ToString & "'"
                If objGPack.GetSqlValue(strSql, "CNT", "").ToString <> "" Then
                    MsgBox("IRN already generated.")
                    Exit Sub
                End If
                If B2B_Address_Validator(gridView.Item("BatchNo", gridView.CurrentRow.Index).Value.ToString, gridView.Item("BatchNo", gridView.CurrentRow.Index).Value.ToString) = False Then
                    Exit Sub
                End If
                If MessageBox.Show("Do you want upload Bill to Portal", "GST", MessageBoxButtons.YesNo) = DialogResult.No Then
                    Exit Sub
                End If
                Dim arp As New ClearTaxInv
                If objGPack.GetSqlValue("SELECT 1 CNT FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & gridView.Item("BatchNo", gridView.CurrentRow.Index).Value.ToString & "'", "CNT", "") <> "" Then
                    arp.B2B_Upload(gridView.Item("BatchNo", gridView.CurrentRow.Index).Value.ToString, cmbFilter.SelectedValue.ToString())
                Else
                    arp.B2B_Upload(gridView.Item("BatchNo", gridView.CurrentRow.Index).Value.ToString, cmbFilter.SelectedValue.ToString(), gridView.Item("ACCODE", gridView.CurrentRow.Index).Value.ToString)
                End If

            End If
        End If
    End Sub


    Function B2B_Address_Validator(ByVal btchNo As String, ByVal Billno As String) As Boolean
        Dim dtBuyer As New DataTable
        If objGPack.GetSqlValue("SELECT 1 CNT FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & btchNo & "'", "CNT", "") <> "" Then
            strSql = vbCrLf + " SELECT ISNULL(ACCODE,'') ACCODE,PNAME,ADDRESS1,ADDRESS2,ADDRESS3,AREA,(CASE WHEN ISNULL(PHONERES,'')='' THEN ISNULL(MOBILE,'') ELSE PHONERES END)PHONERES,EMAIL,GSTNO,ISNULL(PINCODE,0)PINCODE,CONVERT(VARCHAR,ISNULL((SELECT STATECODE FROM " + cnAdminDb + "..STATEMAST WHERE STATENAME=A.STATE),0))STATECODE "
            strSql += vbCrLf + " FROM " & cnAdminDb & "..PERSONALINFO AS A WHERE SNO IN (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & btchNo & "') AND ISNULL(ACCODE,'')='' "
            da = New OleDbDataAdapter(strSql, cn)
            dtBuyer = New DataTable
            da.Fill(dtBuyer)
            If dtBuyer.Rows.Count > 0 Then
                If dtBuyer.Rows(0)("ACCODE").ToString <> "" Then GoTo ACCODECHK
                Dim str As String = ""
                For k As Integer = 0 To dtBuyer.Columns.Count - 1
                    If dtBuyer.Columns(k).ColumnName.ToString = "ACCODE" And dtBuyer.Rows(0)("ACCODE").ToString = "" Then Continue For
                    If dtBuyer.Rows(0)(dtBuyer.Columns(k).ColumnName.ToString).ToString = "" Then
                        str += vbCrLf + dtBuyer.Columns(k).ColumnName.ToString + " is Required"
                    End If
                Next
                If str.ToString.Trim <> "" Then
                    MsgBox("Bill No - " + Billno + vbCrLf + str)
                    Return False
                End If
                Return True
            End If
ACCODECHK:
            strSql = vbCrLf + " SELECT ACNAME PNAME,ADDRESS1,ADDRESS2,ADDRESS3,AREA,(CASE WHEN ISNULL(PHONENO,'')='' THEN ISNULL(MOBILE,'') ELSE PHONENO END)PHONERES,EMAILID EMAIL,GSTNO,ISNULL(PINCODE,0)PINCODE,CONVERT(VARCHAR,ISNULL((SELECT STATECODE FROM " & cnAdminDb & "..STATEMAST WHERE STATENAME=A.STATE),0))STATECODE  "
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ACHEAD AS A WHERE ACCODE IN (SELECT ACCODE FROM " & cnStockDb & "..ISSUE WHERE BATCHNO='" & btchNo & "' UNION ALL SELECT ACCODE FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO='" & btchNo & "')"
            da = New OleDbDataAdapter(strSql, cn)
            dtBuyer = New DataTable
            da.Fill(dtBuyer)
            If dtBuyer.Rows.Count > 0 Then
                Dim str As String = ""
                For k As Integer = 0 To dtBuyer.Columns.Count - 1
                    If dtBuyer.Rows(0)(dtBuyer.Columns(k).ColumnName.ToString).ToString = "" Then
                        str += vbCrLf + dtBuyer.Columns(k).ColumnName.ToString + " is Required"
                    End If
                Next
                If str.ToString.Trim <> "" Then
                    MsgBox("Bill No - " + Billno + vbCrLf + str)
                    Return False
                End If
                Return True
            End If
        Else
            strSql = vbCrLf + " SELECT ACNAME PNAME,ADDRESS1,ADDRESS2,ADDRESS3,AREA,(CASE WHEN ISNULL(PHONENO,'')='' THEN ISNULL(MOBILE,'') ELSE PHONENO END)PHONERES,EMAILID EMAIL,GSTNO,ISNULL(PINCODE,0)PINCODE,CONVERT(VARCHAR,ISNULL((SELECT STATECODE FROM " & cnAdminDb & "..STATEMAST WHERE STATENAME=A.STATE),0))STATECODE  "
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ACHEAD AS A WHERE ACCODE ='" & gridView.Item("ACCODE", gridView.CurrentRow.Index).Value.ToString & "'"
            da = New OleDbDataAdapter(strSql, cn)
            dtBuyer = New DataTable
            da.Fill(dtBuyer)
            If dtBuyer.Rows.Count > 0 Then
                Dim str As String = ""
                For k As Integer = 0 To dtBuyer.Columns.Count - 1
                    If dtBuyer.Rows(0)(dtBuyer.Columns(k).ColumnName.ToString).ToString = "" Then
                        str += vbCrLf + dtBuyer.Columns(k).ColumnName.ToString + " is Required"
                    End If
                Next
                If str.ToString.Trim <> "" Then
                    MsgBox("Bill No - " + Billno + vbCrLf + str)
                    Return False
                End If
                Return True
            End If
        End If
        Return False
    End Function

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
        Dim obj As New frmGSTR3B_Properties
        SetSettingsObj(obj, Me.Name, GetType(frmGSTR3B_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmGSTR3B_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmGSTR3B_Properties))
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
End Class

Public Class frmGSTR3B_Properties
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