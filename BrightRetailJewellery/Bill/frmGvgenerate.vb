Imports System.Data.OleDb
Public Class frmGvgenerate
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim dReader As OleDbDataReader
    Dim tran As OleDbTransaction

    Dim strSql As String

    Dim Sno As String = Nothing
    Dim EntryOrder As Integer = 0
    Dim multipleTagFlag As String = Nothing
    Dim pieceRate As Decimal = 0
    Dim pcs As Integer = 0
    Dim narration As String
    Dim BatchNo As String
    Dim purFineRate As Double
    Dim purTouch As Double


    Dim TagNoGen As String = Nothing
    Dim TagNoFrom As String = Nothing
    Dim Gvnumber As String = Nothing
    Dim GV_QRCODE As Boolean = IIf(GetAdmindbSoftValue("GV_QRCODE", "N") = "Y", True, False)
    Dim GIFTWT2WT As String = GetAdmindbSoftValue("GIFTWT2WT", "").ToUpper
    Dim GIFTWT2WT_RANGE() As String

    'Dim objTag As New TagGeneration

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Dim dt As New DataTable
        ''CostCentre Checking..
        strSql = " SELECT 1 FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE' AND CTLTEXT = 'Y'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            cmbCostCentre.Items.Clear()
            cmbCostCentre.Enabled = False
        End If

        If cmbCostCentre.Enabled = True Then
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE "
            strSql += " WHERE ISNULL(ACTIVE,'Y')<>'N'"
            strSql += " ORDER BY COSTNAME"
            objGPack.FillCombo(strSql, cmbCostCentre)
            cmbCostCentre.Text = cnCostName
        End If

        strSql = " SELECT NAME FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE = 'G' ORDER BY NAME"
        objGPack.FillCombo(strSql, cmbItem_MAN)
        If GV_QRCODE = True Then
            strSql = " ALTER TABLE " & cnStockDb & "..GVTRAN ALTER COLUMN QTY NUMERIC (15,3) "
            cmd = New OleDbCommand(strSql, cn) : cmd.ExecuteNonQuery()
            Dim str As String = txtDeno_Num.Name
            txtDeno_Num.Name = str.Replace("_Num", "_AMT")
            txtQty_NUM.Name = str.Replace("_Num", "_WET")
            LabelQty.Text = "Weight"
        Else
            LabelQty.Text = "Qty"
        End If
        If GIFTWT2WT.Contains(",") Then
            GIFTWT2WT_RANGE = GIFTWT2WT.Split(",")
        End If
        funcNew()
    End Sub
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcGenItemTagStrSql(ByVal TagSno As String, ByVal COSTID As String _
    , ByVal CARDID As Integer, ByVal GVNo As String, ByVal VALUE As Decimal)


        ''Inserting GVTRAN
        strSql = " INSERT INTO " & cnStockDb & "..GVTRAN"
        strSql += " ("
        strSql += " SNO,TRANDATE,COSTID,CARDID,QTY,AMOUNT,"
        strSql += " RUNNO,DUEDAYS,COMPANYID,BATCHNO,"
        strSql += " USERID,UPDATED,UPTIME,TCOSTID)VALUES("
        strSql += " '" & TagSno & "'" 'SNO
        strSql += " ,'" & GetEntryDate(dtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
        strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
        strSql += " ," & CARDID
        strSql += " ,1"
        strSql += " ," & Val(txtDeno_Num.Text)
        strSql += " ,'" & GVNo & "'"
        strSql += " ," & Val(txtDuedays.Text)
        strSql += " ,'" & GetStockCompId() & "'"
        strSql += " ,'" & BatchNo & "'"
        strSql += " ," & userId & "" 'USERID
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & COSTID & "'" 'TCOSTID
        strSql += " )"
        Return strSql
    End Function

    Function funcGenItemTagStrSqlNew(ByVal TagSno As String, ByVal COSTID As String _
    , ByVal CARDID As Integer, ByVal GVNo As String, ByVal VALUE As Decimal)
        ''Inserting GVTRAN
        strSql = " INSERT INTO " & cnStockDb & "..GVTRAN"
        strSql += " ("
        strSql += " SNO,TRANDATE,COSTID,CARDID,QTY,AMOUNT,"
        strSql += " RUNNO,DUEDAYS,COMPANYID,BATCHNO,"
        strSql += " USERID,UPDATED,UPTIME,TCOSTID)VALUES("
        strSql += " '" & TagSno & "'" 'SNO
        strSql += " ,'" & GetEntryDate(dtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
        strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
        strSql += " ," & CARDID 'cardid
        strSql += " ,'" & Val(txtQty_NUM.Text) & "'" 'qty
        strSql += " ,'" & Val(txtDeno_Num.Text) & "'"  'AMOUNT
        strSql += " ,'" & GVNo & "'"
        strSql += " ," & Val(txtDuedays.Text)
        strSql += " ,'" & GetStockCompId() & "'"
        strSql += " ,'" & BatchNo & "'"
        strSql += " ," & userId & "" 'USERID
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & COSTID & "'" 'TCOSTID
        strSql += " )"
        Return strSql
    End Function
    Function funcNew() As Integer
        objGPack.TextClear(Me)
        Sno = Nothing
        EntryOrder = 0
        multipleTagFlag = Nothing
        dtpRecieptDate.Focus()
        dtpRecieptDate.Value = GetEntryDate(GetServerDate)
        cmbCostCentre.Text = cnCostName
    End Function
    Function funcSave() As Integer
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If Not CheckDate(dtpRecieptDate.Value) Then Exit Function
        If CheckEntryDate(dtpRecieptDate.Value) Then Exit Function
        funcAdd()
    End Function

    Function funcAdd() As Integer
        Dim TagSno As String = Nothing
        Dim GiftNo As String = Nothing

        Dim COSTID As String = cnCostId
        Dim CARDID As Integer = Nothing

        Dim tagVal As Integer = 0
        tran = Nothing
        ''Find ItemId
        strSql = " SELECT CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME='" & cmbItem_MAN.Text & "'"
        CARDID = Val(objGPack.GetSqlValue(strSql, , , tran))

        If Val(txtDeno_Num.Text) = 0 Then
            MsgBox("Denomination is empty", MsgBoxStyle.Information)
        End If
        If Val(txtQty_NUM.Text) = 0 Then
            MsgBox("Denomination is empty", MsgBoxStyle.Information)
        End If

        Try
            tran = Nothing
            tran = cn.BeginTransaction()
            ''Find TagSno
            BatchNo = GetNewBatchno(cnCostId, dtpRecieptDate.Value, tran)

            If cmbCostCentre.Enabled = True Then
                ''Find COSTID
                strSql = " SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE "
                strSql += " WHERE COSTNAME  = '" & cmbCostCentre.Text & "' "
                strSql += " AND ISNULL(ACTIVE,'Y')<>'N'"
                COSTID = objGPack.GetSqlValue(strSql, , , tran)
            End If
            Dim dtTagPrint As New DataTable
            dtTagPrint.Columns.Add("CARDID", GetType(Integer))
            dtTagPrint.Columns.Add("BATCHNO", GetType(String))

            Dim rowTag As DataRow = Nothing
            Dim bulkPcs As Integer = pcs
            rowTag = dtTagPrint.NewRow
            rowTag!CARDID = CARDID
            rowTag!BATCHNO = BatchNo
            dtTagPrint.Rows.Add(rowTag)
            If multipleTagFlag = "N" Then
                'issSno = GetNewSno( TranSnoType.TISSUECODE, TranSnoType.ISSUECODE), tran)
                TagSno = GetNewSno(TranSnoType.GVTRANCODE, tran) ' GetTagSno()
                GiftNo = GetGvNo(tran)
                strSql = funcGenItemTagStrSql(TagSno, COSTID, CARDID, GiftNo, Val(txtDeno_Num.Text))
                cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                UpdateGvNo(GiftNo, COSTID, tran)
            Else
                If GV_QRCODE = True And GIFTWT2WT <> "" And GIFTWT2WT_RANGE(0) = "Y" Then
                    TagSno = GetNewSno(TranSnoType.GVTRANCODE, tran) ' GetTagSno()
                    Dim RegNo As Double = 0
                    If txtPrefix.Text = "" Then
                        GiftNo = GetGvNo(tran)
                    Else
                        strSql = "   SELECT ISNULL(REGNO,0)REGNO FROM  " & cnAdminDb & "..GVDENOMMAST "
                        strSql += vbCrLf + "  WHERE AMOUNT='" & Val(txtDeno_Num.Text.ToString) & "' AND PREFIX='" & txtPrefix.Text.ToString & "'"
                        strSql += vbCrLf + "  AND GVID IN (SELECT TOP 1 CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='G' AND NAME='" & cmbItem_MAN.Text.ToString & "')"
                        RegNo = Val(objGPack.GetSqlValue(strSql, , "", tran).ToString) + 1
                        GiftNo = GetCostId(cnCostId) & GetCompanyId(strCompanyId) & txtPrefix.Text.ToString + RegNo.ToString
                    End If

                    strSql = funcGenItemTagStrSqlNew(TagSno, COSTID, CARDID, GiftNo, 1)
                    cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()

                    If txtPrefix.Text = "" Then
                        UpdateGvNo(GiftNo, COSTID, tran)
                    Else
                        strSql = " UPDATE " & cnAdminDb & "..GVDENOMMAST SET REGNO  = '" & Val(RegNo) & "' "
                        strSql += vbCrLf + "  WHERE AMOUNT='" & Val(txtDeno_Num.Text.ToString) & "' AND PREFIX='" & txtPrefix.Text.ToString & "'"
                        strSql += vbCrLf + "  AND GVID IN (SELECT TOP 1 CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='G' AND NAME='" & cmbItem_MAN.Text.ToString & "')"
                        ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID)
                    End If
                Else
                    For cnt As Integer = 1 To Val(txtQty_NUM.Text)
                        TagSno = GetNewSno(TranSnoType.GVTRANCODE, tran) ' GetTagSno()
                        Dim RegNo As Double = 0
RunnoChk:
                        If txtPrefix.Text = "" Then
                            GiftNo = GetGvNo(tran)
                        Else
                            strSql = "   SELECT ISNULL(REGNO,0)REGNO FROM  " & cnAdminDb & "..GVDENOMMAST "
                            strSql += vbCrLf + "  WHERE AMOUNT='" & Val(txtDeno_Num.Text.ToString) & "' AND PREFIX='" & txtPrefix.Text.ToString & "'"
                            strSql += vbCrLf + "  AND GVID IN (SELECT TOP 1 CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='G' AND NAME='" & cmbItem_MAN.Text.ToString & "')"
                            RegNo = Val(objGPack.GetSqlValue(strSql, , "", tran).ToString) + 1
                            GiftNo = GetCostId(cnCostId) & GetCompanyId(strCompanyId) & txtPrefix.Text.ToString + RegNo.ToString
                        End If
                        'strSql = " SELECT 1 FROM " & cnStockDb & "..GVTRAN WHERE RUNNO='" & GiftNo & "' "
                        'If Val(objGPack.GetSqlValue(strSql, , "", tran)) Then GoTo RunnoChk
                        bulkPcs = cnt

                        strSql = funcGenItemTagStrSql(TagSno, COSTID, CARDID, GiftNo, 1)
                        cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                        'ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID)

                        If txtPrefix.Text = "" Then
                            UpdateGvNo(GiftNo, COSTID, tran)
                        Else
                            strSql = " UPDATE " & cnAdminDb & "..GVDENOMMAST SET REGNO  = '" & Val(RegNo) & "' "
                            strSql += vbCrLf + "  WHERE AMOUNT='" & Val(txtDeno_Num.Text.ToString) & "' AND PREFIX='" & txtPrefix.Text.ToString & "'"
                            strSql += vbCrLf + "  AND GVID IN (SELECT TOP 1 CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='G' AND NAME='" & cmbItem_MAN.Text.ToString & "')"
                            ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID)
                        End If
                    Next
                End If
            End If
            tran.Commit()
            tran = Nothing
            MsgBox(E0008, MsgBoxStyle.Exclamation)

            'gift voucher qrcode
            If GV_QRCODE = True Then
                strSql = "SELECT RUNNO,QTY GRSWT,AMOUNT RATE,TRANDATE FROM " & cnStockDb & "..GVTRAN WHERE BATCHNO ='" & BatchNo & "' "
                Dim DT As DataRow = Nothing
                DT = GetSqlRow(strSql, cn, Nothing)
                If Not DT Is Nothing Then
                    Dim tdate As Date = DT!TRANDATE
                    Dim strqrdata As String = DT!RUNNO.ToString & "-" & DT!RATE & "-" & DT!GRSWT & "-" & tdate.ToString("yyyy/MM/dd")
                    Dim objp As New BrighttechPack.Methods
                    objp.qrcode_image(strqrdata, "GV_" & BatchNo)
                End If
            End If
            ''Last Tag and Wt
            funcNew()
            Dim prnmemsuffix As String = ""
            Dim strDate As String = GetEntryDate(dtpRecieptDate.Value, tran).ToString("yyyy-MM-dd")
            If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
            If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                Dim write As IO.StreamWriter
                Dim memfile As String = "\BillPrint" & prnmemsuffix.Trim & ".mem"
                write = IO.File.CreateText(Application.StartupPath & memfile)
                write.WriteLine(LSet("TYPE", 15) & ":GVP")
                write.WriteLine(LSet("BATCHNO", 15) & ":" & BatchNo)
                write.WriteLine(LSet("TRANDATE", 15) & ":" & strDate)
                write.WriteLine(LSet("DUPLICATE", 15) & ":N")
                write.Flush()
                write.Close()
                If EXE_WITH_PARAM = False Then
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                Else
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe",
                    LSet("TYPE", 15) & ":GVP" & ";" &
                    LSet("BATCHNO", 15) & ":" & BatchNo & ";" &
                    LSet("TRANDATE", 15) & ":" & strDate & ";" &
                    LSet("DUPLICATE", 15) & ":N")
                End If
            Else
                MsgBox("Billprint exe not found", MsgBoxStyle.Information)
            End If

            ''Lot Pcs
            'Dim oldItem As Integer = Nothing
            'Dim write As IO.StreamWriter
            'write = IO.File.CreateText(Application.StartupPath & "\Gvcodeprint.mem")
            'For Each ro As DataRow In dtTagPrint.Rows

            '    write.WriteLine(LSet("CARD", 7) & ":" & ro!CARDID.ToString)
            '    write.WriteLine(LSet("BATCHNO", 7) & ":" & ro!BATCHNO.ToString)
            'Next
            'write.Flush()
            'write.Close()
            'If IO.File.Exists(Application.StartupPath & "\GvPrint.exe") Then
            '    System.Diagnostics.Process.Start(Application.StartupPath & "\GvPrint.exe")
            'Else
            '    MsgBox("GvPrint.exe not found", MsgBoxStyle.Information)
            'End If
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            If Not tran Is Nothing Then tran.Dispose()
        End Try
    End Function
    'Private Function GetGvVal(ByVal GvNo As String) As String
    '    Dim TagVal As Integer = Nothing
    '    ''Find TagVal
    '    If IsNumeric(GvNo) = True Then
    '        TagVal = Val(GvNo)
    '    Else
    '        Dim index As Integer = 0
    '        For Each c As Char In GvNo
    '            If Char.IsLetter(c) = True Then
    '                index += 1
    '            Else
    '                Exit For
    '            End If
    '        Next
    '        Dim fNo As String
    '        Dim sNo As String
    '        fNo = GvNo.Substring(0, index)
    '        sNo = GvNo.Substring(index, GvNo.Length - index)
    '        TagVal = (AscW(fNo) * 1000) + Val(sNo)
    '    End If
    '    Return TagVal.ToString
    'End Function


    'Function funcGenTagNo(ByVal lstTagNo As String) As String
    '    '
    '    If IsNumeric(lstTagNo) = True Then
    '        lstTagNo = Val(lstTagNo) + 1
    '    Else
    '        Dim index As Integer = 0
    '        For Each c As Char In lstTagNo
    '            If Char.IsLetter(c) = True Then
    '                index += 1
    '            Else
    '                Exit For
    '            End If
    '        Next
    '        Dim fNo As String
    '        Dim sNo As String
    '        fNo = lstTagNo.Substring(0, index)
    '        sNo = lstTagNo.Substring(index, lstTagNo.Length - index)
    '        sNo = Val(sNo) + 1
    '        lstTagNo = fNo + sNo
    '    End If
    '    Return lstTagNo
    'End Function



    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        btnSave_Click(Me, New EventArgs)
    End Sub
    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub

    Private Sub txtQty_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtQty_NUM.TextChanged
        If Val(txtQty_NUM.Text) <> 0 Then txtValue.Text = Format(Val(txtQty_NUM.Text) * Val(txtDeno_Num.Text), "0.00")
    End Sub

    Private Sub frmGvgenerate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtDeno_Num_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtDeno_Num.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            strSql = "   SELECT PREFIX FROM  " & cnAdminDb & "..GVDENOMMAST WHERE AMOUNT='" & Val(txtDeno_Num.Text.ToString) & "' "
            strSql += vbCrLf + "  AND GVID IN (SELECT TOP 1 CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='G' AND NAME='" & cmbItem_MAN.Text.ToString & "')"
            txtPrefix.Text = objGPack.GetSqlValue(strSql, , "")
        End If
    End Sub

    Private Sub frmGvgenerate_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class

