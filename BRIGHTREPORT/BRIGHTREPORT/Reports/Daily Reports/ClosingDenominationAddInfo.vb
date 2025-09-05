Imports System.Data.OleDb
Public Class ClosingDenominationAddInfo
    Dim strsql As String
    Dim cmd As New OleDbCommand
    Dim dt As New DataTable
    Dim fdt As New DataTable
    Dim StrFilter As String
    Dim da As New OleDbDataAdapter
    Dim batchno As String
    Dim tempdt As New DataTable
    Private Sub ClosingDenomination_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then 'And cmbtype.Focused <> True Then
            If cmbtype.Focused <> False Then Exit Sub
            SendKeys.Send("{TAb}")
        End If
    End Sub
    Private Sub ClosingDenomination_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        InitializeFunc()
        tempdt = New DataTable
        strsql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMPADD') DROP TABLE TEMPADD"
        strsql += " CREATE TABLE TEMPADD( MODE VARCHAR(25),TYPE VARCHAR(2),CATEGORY VARCHAR(75),CATCODE VARCHAR(15),PCS INT,GRSWT NUMERIC(15,3),NETWT NUMERIC(15,3)) "
        cmd = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strsql += " select * from TEMPADD "
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(tempdt)
    End Sub
#Region "TextBox Events"
    Private Sub txtden5_OWN_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtden5_OWN.TextChanged
        txtAmt5coin_OWN.Text = (Val(txtden5_OWN.Text) * 5).ToString
        AmtCalculation()
    End Sub

    Private Sub txtden1_OTHERS_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtden1_OTHERS.TextChanged
        txtAmtothers_OWN.Text = (Val(txtden1_OTHERS.Text) * 1).ToString
        AmtCalculation()
    End Sub

    Private Sub txtden1_OWN_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtden1_OWN.TextChanged
        txtAmt1_OWN.Text = (Val(txtden1_OWN.Text) * 1).ToString
        AmtCalculation()
    End Sub

    Private Sub txtden2_OWN_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtden2_OWN.TextChanged
        txtAmt2_OWN.Text = (Val(txtden2_OWN.Text) * 2).ToString
        AmtCalculation()
    End Sub

    Private Sub txtden10_OWN_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtden10_OWN.TextChanged
        txtAmt10_OWN.Text = (Val(txtden10_OWN.Text) * 10).ToString
        AmtCalculation()
    End Sub

    Private Sub txtden20_OWN_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtden20_OWN.TextChanged
        txtAmt20_OWN.Text = (Val(txtden20_OWN.Text) * 20).ToString
        AmtCalculation()
    End Sub

    Private Sub txtden50_OWN_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtden50.TextChanged
        txtAmt50_OWN.Text = (Val(txtden50.Text) * 50).ToString
        AmtCalculation()
    End Sub

    Private Sub txtden1hrd_OWN_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtden1hrd.TextChanged
        txtAmt1hrd_OWN.Text = (Val(txtden1hrd.Text) * 100).ToString
        AmtCalculation()
    End Sub

    Private Sub txtden5hrd_OWN_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtden5hrd.TextChanged
        txtAmt5hrd_OWN.Text = (Val(txtden5hrd.Text) * 200).ToString
        AmtCalculation()
    End Sub

    Private Sub txtDenthous_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtDenthous.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            'SendKeys.Send("{TAb}")
        End If
    End Sub

    Private Sub txtDenthous_OWN_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDenthous.TextChanged
        txtAmttho_OWN.Text = (Val(txtDenthous.Text) * 500).ToString
        AmtCalculation()
    End Sub
#End Region

#Region "Key Events"

    Private Sub cmbCntId_OWN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbCntId_OWN.KeyDown
        If e.KeyCode = Keys.Enter Then
            'FillData()
            tempdt = New DataTable
            strsql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMPADD') DROP TABLE TEMPADD"
            strsql += " CREATE TABLE TEMPADD( MODE VARCHAR(25),TYPE VARCHAR(2),CATEGORY VARCHAR(75),CATCODE VARCHAR(15),PCS INT,GRSWT NUMERIC(15,3),NETWT NUMERIC(15,3)) "
            cmd = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strsql += " select * from TEMPADD "
            da = New OleDbDataAdapter(strsql, cn)
            da.Fill(tempdt)
        End If
    End Sub
#End Region

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Funsave()
        InitializeFunc()
        tempdt = New DataTable
    End Sub

#Region "Defined Function"

    Function InitializeFunc()
        CmbLoad()
        txtDen2thous.Text = "0.00"
        txtDenthous.Text = "0.00"
        txtden5hrd.Text = "0.00"
        txtden2hrd.Text = "0.00"
        txtden1hrd.Text = "0.00"
        txtden50.Text = "0.00"
        txtden20_OWN.Text = "0.00"
        txtden10_OWN.Text = "0.00"
        txtden5_OWN.Text = "0.00"
        txtden2_OWN.Text = "0.00"
        txtden1_OWN.Text = "0.00"
        txtden1_OTHERS.Text = "0.00"
        txtAmt2tho_OWN.Text = "0.00"
        txtAmttho_OWN.Text = "0.00"
        txtAmt5hrd_OWN.Text = "0.00"
        txtAmt2hrd_OWN.Text = "0.00"
        txtAmt1hrd_OWN.Text = "0.00"
        txtAmt50_OWN.Text = "0.00"
        txtAmt20_OWN.Text = "0.00"
        txtAmt10_OWN.Text = "0.00"
        txtAmt5coin_OWN.Text = "0.00"
        txtAmt2_OWN.Text = "0.00"
        txtAmt1_OWN.Text = "0.00"
        txtAmtothers_OWN.Text = "0.00"
        txtTotNetAmt_OWN.Text = "0.00"
        txtNetBal_OWN.Text = "0.00"
        cmbtype.Text = ""
        txtpcs.Text = ""
        txtgrswt.Text = ""
        txtNewt.Text = ""
        gridSummary.DataSource = Nothing
        DatePicker1.Focus()
        grid.Visible = False
    End Function

    Function CmbLoad()
        strsql = "SELECT CASHID,CASHNAME FROM " & cnAdminDb & "..CASHCOUNTER"
        DT = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(DT)
        If DT.Rows.Count > 0 Then
            cmbCntId_OWN.DataSource = Nothing
            cmbCntId_OWN.DataSource = DT
            cmbCntId_OWN.DisplayMember = "CASHNAME"
            cmbCntId_OWN.ValueMember = "CASHID"
        End If
        strsql = "SELECT CATCODE,CATNAME FROM " & cnAdminDb & "..CATEGORY ORDER BY CATNAME"
        dt = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            cmbcategory_OWN.DataSource = Nothing
            cmbcategory_OWN.DataSource = dt
            cmbcategory_OWN.DisplayMember = "CATNAME"
            cmbcategory_OWN.ValueMember = "CATCODE"
        End If
        cmbnodeid_OWN.Items.Clear()
        strsql = "SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..ACCTRAN ORDER BY SYSTEMID"
        objGPack.FillCombo(strsql, cmbnodeid_OWN, False)

    End Function

    Function GetCashDetails(ByVal cntid As String, ByVal gdate As DateTime) As DataTable
        Dim gdt As New DataTable
        strsql = "SELECT (SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE='" & Format(gdate, "yyyy-MM-dd") & "' AND CASHID ='" & cntid & "' AND TRANMODE='D' AND PAYMODE='CH')AS CHEQUEDR"
        strsql += vbCrLf + ",(SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE='" & Format(gdate, "yyyy-MM-dd") & "' AND CASHID ='" & cntid & "' AND TRANMODE='C' AND PAYMODE='CH')AS CHEQUECR"
        strsql += vbCrLf + ",(SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE='" & Format(gdate, "yyyy-MM-dd") & "' AND CASHID ='" & cntid & "' AND TRANMODE='D' AND PAYMODE='CC')AS CARDDR"
        strsql += vbCrLf + ",(SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE='" & Format(gdate, "yyyy-MM-dd") & "' AND CASHID ='" & cntid & "' AND TRANMODE='C' AND PAYMODE='CC')AS CARDCR"
        strsql += vbCrLf + ",(SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE='" & Format(gdate, "yyyy-MM-dd") & "' AND CASHID ='" & cntid & "' AND TRANMODE='C' AND PAYMODE='CA')AS CASHCR"
        strsql += vbCrLf + ",(SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE='" & Format(gdate, "yyyy-MM-dd") & "' AND CASHID ='" & cntid & "' AND TRANMODE='D' AND PAYMODE='CA')AS CASHDR"
        strsql += vbCrLf + ",(SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE='" & Format(gdate, "yyyy-MM-dd") & "' AND CASHID ='" & cntid & "' AND TRANMODE='C' AND PAYMODE='GV')AS GVCR"
        strsql += vbCrLf + ",(SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE='" & Format(gdate, "yyyy-MM-dd") & "' AND CASHID ='" & cntid & "' AND TRANMODE='D' AND PAYMODE='GV')AS GVDR"
        strsql += vbCrLf + ",(SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE='" & Format(gdate, "yyyy-MM-dd") & "' AND CASHID ='" & cntid & "' AND TRANMODE='D' AND PAYMODE='DU')AS CREDIT "
        gdt = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(gdt)
        If gdt.Rows.Count > 0 Then
            Return gdt
        End If
    End Function
    Function FillData()
        txtDen2thous.Text = "0.00"
        txtDenthous.Text = "0.00"
        txtden5hrd.Text = "0.00"
        txtden2hrd.Text = "0.00"
        txtden1hrd.Text = "0.00"
        txtden50.Text = "0.00"
        txtden20_OWN.Text = "0.00"
        txtden10_OWN.Text = "0.00"
        txtden5_OWN.Text = "0.00"
        txtden2_OWN.Text = "0.00"
        txtden1_OWN.Text = "0.00"
        txtden1_OTHERS.Text = "0.00"
        txtAmt2tho_OWN.Text = "0.00"
        txtAmttho_OWN.Text = "0.00"
        txtAmt5hrd_OWN.Text = "0.00"
        txtAmt2hrd_OWN.Text = "0.00"
        txtAmt1hrd_OWN.Text = "0.00"
        txtAmt50_OWN.Text = "0.00"
        txtAmt20_OWN.Text = "0.00"
        txtAmt10_OWN.Text = "0.00"
        txtAmt5coin_OWN.Text = "0.00"
        txtAmt2_OWN.Text = "0.00"
        txtAmt1_OWN.Text = "0.00"
        txtAmtothers_OWN.Text = "0.00"
        txtTotNetAmt_OWN.Text = "0.00"
        txtNetBal_OWN.Text = "0.00"
        txtpcs.Text = ""
        txtNewt.Text = ""
        txtgrswt.Text = ""
        gridSummary.DataSource = Nothing
        fdt = New DataTable
        fdt = GetCashDetails(cmbCntId_OWN.SelectedValue.ToString, DatePicker1.Value)
        If fdt.Rows.Count > 0 Then
            'If Val(fdt.Rows(0).Item("CHEQUEDR").ToString) <> 0 Then
            '    txtDDebit.Text = fdt.Rows(0).Item("CHEQUEDR").ToString
            'End If
            'If Val(fdt.Rows(0).Item("CHEQUECR").ToString) <> 0 Then
            '    txtCCredit.Text = fdt.Rows(0).Item("CHEQUECR").ToString
            'End If
            'If Val(fdt.Rows(0).Item("CHEQUEDR").ToString) <> 0 Then
            '    txtCtotal.Text = (Val(fdt.Rows(0).Item("CHEQUEDR").ToString) - Val(fdt.Rows(0).Item("CHEQUECR").ToString)).ToString
            'End If
            'If Val(fdt.Rows(0).Item("CARDCR").ToString) <> 0 Then
            '    txtCCardCredit_OWN.Text = IIf(IsDBNull(fdt.Rows(0).Item("CARDCR").ToString), "0.00", fdt.Rows(0).Item("CARDCR").ToString)
            'End If
            'If Val(fdt.Rows(0).Item("CARDDR").ToString) <> 0 Then
            '    txtCCrdDebit_OWN.Text = IIf(IsDBNull(fdt.Rows(0).Item("CARDDR").ToString), "0.00", fdt.Rows(0).Item("CARDDR").ToString)
            'End If
            'txtCCardTotal_OWN.Text = (Val(fdt.Rows(0).Item("CARDDR").ToString) - Val(fdt.Rows(0).Item("CARDCR").ToString)).ToString
            'If Val(fdt.Rows(0).Item("CASHDR").ToString) <> 0 Then
            '    TextBox1.Text = IIf(IsDBNull(fdt.Rows(0).Item("CASHDR").ToString), "0.00", fdt.Rows(0).Item("CASHDR").ToString)
            'End If
            'If Val(fdt.Rows(0).Item("CASHCR").ToString) <> 0 Then
            '    txtCrCredit_OWN.Text = IIf(IsDBNull(fdt.Rows(0).Item("CASHCR").ToString), "0.00", fdt.Rows(0).Item("CASHCR").ToString)
            'End If

            'If Val(fdt.Rows(0).Item("GVDR").ToString) <> 0 Then
            '    txtGvdebit.Text = IIf(IsDBNull(fdt.Rows(0).Item("GVDR").ToString), "0.00", fdt.Rows(0).Item("GVDR").ToString)
            'End If
            'If Val(fdt.Rows(0).Item("GVCR").ToString) <> 0 Then
            '    txtGvcredit.Text = IIf(IsDBNull(fdt.Rows(0).Item("GVCR").ToString), "0.00", fdt.Rows(0).Item("GVCR").ToString)
            'End If

            'txtCrdTotal_OWN.Text = Val(TextBox1.Text.ToString) - Val(txtCrCredit_OWN.Text.ToString)
            'txtDNetAmt_OWN.Text = Val(txtCrdTotal_OWN.Text.ToString)
        End If
        'cmbtype.Focus()
    End Function

    Function ViewDenom(ByVal CASID As String, ByVal TRANDATE As DateTime)
        Dim dtk As New DataTable
        strsql = "SELECT * FROM " & cnStockDb & "..DENOMTRAN WHERE 1=1"
        strsql += vbCrLf + " AND SYSTEMID ='" & CASID & "' AND TRANDATE ='" & Format(TRANDATE, "yyyy-MM-dd") & "' "
        strsql += vbCrLf + " AND DEN_ID <>'0' "
        dtk = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtk)
        If dtk.Rows.Count > 0 Then
            For i As Integer = 0 To dtk.Rows.Count - 1
                If dtk.Rows(i).Item("DEN_ID") = "11" Then
                    txtAmt2tho_OWN.Text = dtk.Rows(i).Item("DEN_AMOUNT").ToString
                    txtDen2thous.Text = dtk.Rows(i).Item("DEN_QTY").ToString
                ElseIf dtk.Rows(i).Item("DEN_ID") = "12" Then
                    txtAmt2hrd_OWN.Text = dtk.Rows(i).Item("DEN_AMOUNT").ToString
                    txtden2hrd.Text = dtk.Rows(i).Item("DEN_QTY").ToString

                ElseIf dtk.Rows(i).Item("DEN_ID") = "1" Then
                    txtAmttho_OWN.Text = dtk.Rows(i).Item("DEN_AMOUNT").ToString
                    txtDenthous.Text = dtk.Rows(i).Item("DEN_QTY").ToString
                ElseIf dtk.Rows(i).Item("DEN_ID").ToString = "2" Then
                    txtAmt5hrd_OWN.Text = dtk.Rows(i).Item("DEN_AMOUNT").ToString
                    txtden5hrd.Text = dtk.Rows(i).Item("DEN_QTY").ToString
                ElseIf dtk.Rows(i).Item("DEN_ID").ToString = "3" Then
                    txtAmt1hrd_OWN.Text = dtk.Rows(i).Item("DEN_AMOUNT").ToString
                    txtden1hrd.Text = dtk.Rows(i).Item("DEN_QTY").ToString
                ElseIf dtk.Rows(i).Item("DEN_ID").ToString = "4" Then
                    txtAmt50_OWN.Text = dtk.Rows(i).Item("DEN_AMOUNT").ToString
                    txtden50.Text = dtk.Rows(i).Item("DEN_QTY").ToString
                ElseIf dtk.Rows(i).Item("DEN_ID").ToString = "5" Then
                    txtAmt20_OWN.Text = dtk.Rows(i).Item("DEN_AMOUNT").ToString
                    txtden20_OWN.Text = dtk.Rows(i).Item("DEN_QTY").ToString
                ElseIf dtk.Rows(i).Item("DEN_ID").ToString = "6" Then
                    txtAmt10_OWN.Text = dtk.Rows(i).Item("DEN_AMOUNT").ToString
                    txtden10_OWN.Text = dtk.Rows(i).Item("DEN_QTY").ToString
                ElseIf dtk.Rows(i).Item("DEN_ID").ToString = "7" Then
                    txtAmt5coin_OWN.Text = dtk.Rows(i).Item("DEN_AMOUNT").ToString
                    txtden5_OWN.Text = dtk.Rows(i).Item("DEN_QTY").ToString
                ElseIf dtk.Rows(i).Item("DEN_ID").ToString = "8" Then
                    txtAmt2_OWN.Text = dtk.Rows(i).Item("DEN_AMOUNT").ToString
                    txtden2_OWN.Text = dtk.Rows(i).Item("DEN_QTY").ToString
                ElseIf dtk.Rows(i).Item("DEN_ID").ToString = "9" Then
                    txtAmt1_OWN.Text = dtk.Rows(i).Item("DEN_AMOUNT").ToString
                    txtden1_OWN.Text = dtk.Rows(i).Item("DEN_QTY").ToString
                ElseIf dtk.Rows(i).Item("DEN_ID").ToString = "10" Then
                    txtAmtothers_OWN.Text = dtk.Rows(i).Item("DEN_AMOUNT").ToString
                    txtden1_OTHERS.Text = dtk.Rows(i).Item("DEN_QTY").ToString
                End If
            Next


            'strsql = "SELECT (CASE WHEN TYPE='PU' THEN 'Purchase' "
            'strsql += vbCrLf + "WHEN TYPE='SR' THEN 'SalesReturn' "
            'strsql += vbCrLf + "Else 'PartlySales' END  ) TYPE"
            'strsql += vbCrLf + ",(SELECT CATNAME FROM " & cnAdminDb & " ..CATEGORY WHERE CATCODE =D.CATCODE)CATEGORY,SYSTEMID,PCS,GRSWT,NETWT  FROM " & cnStockDb & "..DENOMTRAN  D WHERE 1=1"
            'strsql += vbCrLf + " AND CASHID ='" & CASID & "' AND TRANDATE ='" & Format(TRANDATE, "yyyy-MM-dd") & "' AND CATCODE='" & catcode & "'"
            'strsql += vbCrLf + " AND DEN_ID='0'"
            'dtk = New DataTable
            'da = New OleDbDataAdapter(strsql, cn)
            'da.Fill(dtk)
            'If dtk.Rows.Count > 0 Then
            '    txtpcs.Text = dtk.Rows(0).Item("PCS").ToString
            '    txtgrswt.Text = dtk.Rows(0).Item("GRSWT").ToString
            '    txtNewt.Text = dtk.Rows(0).Item("NETWT").ToString
            '    cmbtype.Text = dtk.Rows(0).Item("TYPE").ToString
            '    cmbnodeid_OWN.Text = dtk.Rows(0).Item("SYSTEMID").ToString
            '    cmbcategory.Text = dtk.Rows(0).Item("CATEGORY").ToString
            'End If
        End If
    End Function

    Private Sub Filteration()
        Dim tempchkitem As String = Nothing
        StrFilter = ""
        StrFilter += " AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN"
        StrFilter += " ('" & cmbCntId_OWN.Text.ToString.Trim & "'))"
        tempchkitem = ""
    End Sub

    Private Function TableCreation()
        strsql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = "
        strsql += " 'TEMP" & systemId & "SASRPU') "
        strsql += " DROP TABLE TEMP" & systemId & "SASRPU "
        strsql += " CREATE TABLE TEMP" & systemId & "SASRPU( "
        strsql += " DESCRIPTION VARCHAR(100),"
        strsql += " RECPCS VARCHAR(20),"
        strsql += " RECGRSWT VARCHAR(20),"
        strsql += " RECNETWT VARCHAR(20),"
        strsql += " RECEIPT NUMERIC(20,2),"
        strsql += " PAYMENT NUMERIC(20,2),"
        strsql += " AVERAGE VARCHAR(50),"
        strsql += " COLHEAD VARCHAR(1),"
        strsql += " RESULT1 INT,"
        strsql += " SNO INT IDENTITY(1,1))"
        Cmd = New OleDbCommand(strsql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Function

    Private Sub ProcPartlySales()
        strsql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSPARTLYSALE') DROP TABLE TEMP" & systemId & "ABSPARTLYSALE "
        strSql += vbcrlf + " SELECT CATNAME, SUM(TAGPCS)-SUM(ISSPCS)ISSPCS ,SUM(TAGGRSWT)-SUM(ISSGRSWT)ISSGRSWT ,SUM(TAGNETWT)-SUM(ISSNETWT)ISSNETWT"
        strSql += vbcrlf + " ,SUM(STNPCS)STNPCS, SUM(STNWT)STNWT, 1 AS RESULT1, ' ' AS COLHEAD "
        strSql += vbcrlf + " INTO TEMP" & systemId & "ABSPARTLYSALE FROM ("
        strSql += vbcrlf + " SELECT (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
        strSql += vbcrlf + " WHERE CATCODE = I.CATCODE)AS CATNAME   "
        strSql += vbcrlf + " ,I.PCS AS ISSPCS, I.GRSWT AS ISSGRSWT, I.NETWT AS ISSNETWT "
        strSql += vbcrlf + " ,(SELECT ISNULL(SUM(STNPCS),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)STNPCS "
        strSql += vbcrlf + " ,(SELECT ISNULL(SUM(TAGSTNPCS),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)TAGSTNPCS "
        strSql += vbcrlf + " ,(SELECT ISNULL(SUM(STNWT),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO )STNWT "
        strSql += vbcrlf + " ,(SELECT ISNULL(SUM(TAGSTNWT),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)TAGSTNWT "
        strSql += vbcrlf + " ,TAGPCS,TAGGRSWT,TAGNETWT FROM " & cnStockDb & "..ISSUE I "
        strSql += vbcrlf + " WHERE I.TRANDATE BETWEEN '" & DatePicker1.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbcrlf + " AND '" & DatePicker1.Value.ToString("yyyy-MM-dd") & "' "
        strSql += vbcrlf + " AND I.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T')"
        strSql += vbcrlf + " AND I.TAGNO <> ''"
        strSql += vbcrlf + " AND ISNULL(I.CANCEL,'') = ''   "
        strSql += vbcrlf + " AND I.COMPANYID IN ('" & cnCompanyId & "')"
        strsql += StrFilter
        strSql += vbcrlf + " ) X "
        strSql += vbcrlf + " WHERE ((TAGPCS)<>ISSPCS OR (TAGGRSWT)<> ISSGRSWT OR (TAGNETWT)<>ISSNETWT "
        strSql += vbcrlf + " ) GROUP BY CATNAME"
        strSql += vbcrlf + " HAVING SUM(TAGPCS)-SUM(ISSPCS) <> 0 OR SUM(TAGGRSWT)-SUM(ISSGRSWT) <> 0"
        'If RPT_SEPVAT_DABS Then
        strSql += vbcrlf + " UNION ALL"
        strSql += vbcrlf + " SELECT CATNAME, SUM(TAGPCS)-SUM(ISSPCS)ISSPCS ,SUM(TAGGRSWT)-SUM(ISSGRSWT)ISSGRSWT ,SUM(TAGNETWT)-SUM(ISSNETWT)ISSNETWT"
        strSql += vbcrlf + " ,SUM(STNPCS)STNPCS, SUM(STNWT)STNWT, 1 AS RESULT1, ' ' AS COLHEAD "
        strSql += vbcrlf + " FROM ("
        strSql += vbcrlf + " SELECT (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
        strSql += vbcrlf + " WHERE CATCODE = ISS.CATCODE)AS CATNAME   "
        strSql += vbcrlf + " ,ISS.STNPCS AS ISSPCS, ISS.STNWT AS ISSGRSWT, ISS.STNWT AS ISSNETWT "
        strSql += vbcrlf + " ,STNPCS "
        strSql += vbcrlf + " ,TAGSTNPCS "
        strSql += vbcrlf + " ,STNWT "
        strSql += vbcrlf + " ,TAGSTNWT "
        strSql += vbcrlf + " ,TAGSTNPCS AS TAGPCS,TAGSTNWT AS TAGGRSWT,TAGSTNWT AS TAGNETWT FROM " & cnStockDb & "..ISSSTONE ISS "
        strSql += vbcrlf + " WHERE ISS.TRANDATE BETWEEN '" & DatePicker1.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbcrlf + " AND '" & DatePicker1.Value.ToString("yyyy-MM-dd") & "' "
        strSql += vbcrlf + " AND ISSSNO IN(SELECT SNO FROM  " & cnStockDb & "..ISSUE I  "
        strSql += vbcrlf + " WHERE I.TRANDATE BETWEEN '" & DatePicker1.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbcrlf + " AND '" & DatePicker1.Value.ToString("yyyy-MM-dd") & "' "
        strSql += vbcrlf + " AND I.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T')"
        strSql += vbcrlf + " AND I.TAGNO <> ''"
        strSql += vbcrlf + " AND ISNULL(I.CANCEL,'') = ''   "
        strSql += vbcrlf + " AND I.COMPANYID IN ('" & cnCompanyId & "')"
        strsql += StrFilter
        strSql += vbcrlf + " ) ) X "
        strSql += vbcrlf + " WHERE ((TAGPCS)<>ISSPCS OR (TAGGRSWT)<> ISSGRSWT OR (TAGNETWT)<>ISSNETWT "
        strSql += vbcrlf + " ) GROUP BY CATNAME"
        strSql += vbcrlf + " HAVING SUM(TAGPCS)-SUM(ISSPCS) <> 0 OR SUM(TAGGRSWT)-SUM(ISSGRSWT) <> 0"
        'End If
        Cmd = New OleDbCommand(strsql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()


        strsql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSPARTLYSALE)>0 "
        strsql += vbCrLf + " BEGIN "
        strsql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
        strsql += vbCrLf + " (DESCRIPTION,RESULT1,COLHEAD) VALUES('PARTLY SALES',0,'T') "
        strsql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
        'strsql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RESULT1,COLHEAD) "
        strsql += vbCrLf + " (DESCRIPTION,RECPCS,RECGRSWT,RECNETWT,RESULT1,COLHEAD) "
        strsql += vbCrLf + " SELECT CATNAME CATNAME, "
        strsql += vbCrLf + " CASE WHEN ISSPCS    <> 0 THEN ISSPCS    ELSE NULL END ISSPCS, "
        strsql += vbCrLf + " CASE WHEN ISSGRSWT  <> 0 THEN ISSGRSWT  ELSE NULL END ISSGRSWT, "
        strsql += vbCrLf + " CASE WHEN ISSNETWT  <> 0 THEN ISSNETWT  ELSE NULL END ISSNETWT, "
        strsql += vbCrLf + " 1 RESULT1, COLHEAD  "
        strsql += vbCrLf + " FROM TEMP" & systemId & "ABSPARTLYSALE ORDER BY RESULT1 "


        strsql += vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
        strsql += vbCrLf + " (DESCRIPTION,RECPCS,RECGRSWT,RECNETWT,RESULT1,COLHEAD) "
        strsql += vbCrLf + " SELECT 'PARTLY SALES TOTAL'CATNAME, "
        strsql += vbCrLf + " SUM(CASE WHEN ISSPCS    <> 0 THEN ISSPCS    ELSE NULL END) ISSPCS, "
        strsql += vbCrLf + " SUM(CASE WHEN ISSGRSWT  <> 0 THEN ISSGRSWT  ELSE NULL END) ISSGRSWT, "
        strsql += vbCrLf + " SUM(CASE WHEN ISSNETWT  <> 0 THEN ISSNETWT  ELSE NULL END) ISSNETWT, "
        strsql += vbCrLf + " 1 RESULT1,'S'COLHEAD  "
        strsql += vbCrLf + " FROM TEMP" & systemId & "ABSPARTLYSALE ORDER BY RESULT1 "
        strsql += vbCrLf + " END "
        Cmd = New OleDbCommand(strsql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub ProcPurchase()
        strsql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSPURCHASE') DROP TABLE TEMP" & systemId & "ABSPURCHASE"
        strSql += vbcrlf + " SELECT CATCODE, "
        strSql += vbcrlf + " CATNAME, ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, "
        strSql += vbcrlf + " RECGRSWT, RECNETWT, RECEIPT, "
        strSql += vbcrlf + " PAYMENT, AVERAGE, RATE, COLHEAD, RESULT,  RESULT1  INTO TEMP" & systemId & "ABSPURCHASE FROM ("
        strSql += vbcrlf + " SELECT R.CATCODE, "
        strSql += vbcrlf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE)AS CATNAME, "
        strSql += vbcrlf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
        strSql += vbcrlf + " SUM(PCS) AS RECPCS, SUM(GRSWT) AS RECGRSWT, SUM(NETWT) AS RECNETWT, "
        strsql += vbCrLf + " 0 AS RECEIPT, "
        strsql += vbCrLf + " SUM(AMOUNT) AS PAYMENT, 1 AS RESULT,1 AS RESULT1, "
        strsql += vbCrLf + " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(CASE WHEN SUM(GRSWT)<>0 THEN "
        strsql += vbCrLf + " (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(GRSWT,0))=0 THEN 1 ELSE SUM(GRSWT) END)) "
        strsql += vbCrLf + " ELSE "
        strsql += vbCrLf + " (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(PCS,0))=0 THEN 1 ELSE SUM(PCS) END)) END))) AS AVERAGE, "
        strsql += vbCrLf + " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(SUM(RATE)/COUNT(RATE)))) AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
        strsql += vbCrLf + " FROM "
        strsql += vbCrLf + " ("
        strsql += vbCrLf + " SELECT PCS,GRSWT,NETWT"
        strsql += vbCrLf + " ,AMOUNT+ISNULL(TAX,0)+ISNULL(SC,0)+ISNULL(ADSC,0)"
        strsql += vbCrLf + " -ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = R.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)"
        strsql += vbCrLf + " AS AMOUNT,RATE,CATCODE,METALID"
        strSql += vbcrlf + " FROM " & cnStockDb & "..RECEIPT R"
        strSql += vbcrlf + " WHERE R.TRANDATE BETWEEN '" & DatePicker1.Value.ToString("yyyy-MM-dd") & "' "
        strSql += vbcrlf + " AND '" & DatePicker1.Value.ToString("yyyy-MM-dd") & "' AND "
        strSql += vbcrlf + " TRANTYPE = 'PU' AND ISNULL(R.CANCEL,'') = ''  AND COMPANYID IN ('" & cnCompanyId & "')"
        strsql += Replace(StrFilter, "SYSTEMID", "R.SYSTEMID")
        strsql += vbCrLf + " )AS R"
        strsql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
        strsql += vbCrLf + " GROUP BY R.CATCODE,C.METALID "
        strSql += vbcrlf + " UNION ALL"
        strSql += vbcrlf + " SELECT R.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
        strSql += vbcrlf + " WHERE ACCODE = C.PTAXID) AS CATNAME, "
        strSql += vbcrlf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
        strSql += vbcrlf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
        strSql += vbcrlf + " SUM(TAX) AS RECEIPT,0 PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
        strSql += vbcrlf + " FROM " & cnStockDb & "..RECEIPT R"
        strSql += vbcrlf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
        strSql += vbcrlf + " WHERE R.TRANDATE BETWEEN '" & DatePicker1.Value.ToString("yyyy-MM-dd") & "' "
        strSql += vbcrlf + " AND '" & DatePicker1.Value.ToString("yyyy-MM-dd") & "' AND "
        strSql += vbcrlf + " R.TRANTYPE = 'PU' AND ISNULL(R.CANCEL,'') = ''  AND R.COMPANYID IN ('" & cnCompanyId & "')"
        strsql += Replace(StrFilter, "SYSTEMID", "R.SYSTEMID")
        strSql += vbcrlf + " GROUP BY R.CATCODE, C.PTAXID "
        strSql += vbcrlf + " HAVING SUM(TAX) > 0"
        strSql += vbcrlf + " UNION ALL"
        strSql += vbcrlf + " SELECT CATCODE,CATNAME, SUM(ISSPCS) AS ISSPCS, "
        strSql += vbcrlf + " SUM(ISSGRSWT) AS ISSGRSWT, SUM(ISSNETWT) AS ISSNETWT,"
        strSql += vbcrlf + " SUM(RECPCS) AS RECPCS, SUM(RECGRSWT) AS RECGRSWT, SUM(RECNETWT) AS RECNETWT, "
        strSql += vbcrlf + " SUM(RECEIPT) RECEIPT, SUM(PAYMENT) PAYMENT, "
        strSql += vbcrlf + " RESULT, 1 AS RESULT1, AVERAGE, RATE, COLHEAD FROM ("
        strSql += vbcrlf + " SELECT (SELECT DISTINCT CATCODE FROM " & cnStockDb & "..RECEIPT "
        strSql += vbcrlf + " WHERE BATCHNO = S.BATCHNO AND COMPANYID IN ('" & cnCompanyId & "') AND CATCODE = S.CATCODE) CATCODE,"
        strSql += vbcrlf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY  "
        strSql += vbcrlf + " WHERE CATCODE = S.CATCODE) CATNAME,  "
        strSql += vbcrlf + " 0 AS ISSPCS,  0 AS ISSGRSWT, 0 AS ISSNETWT,  "
        strSql += vbcrlf + " SUM(S.STNPCS) AS RECPCS,  SUM(S.STNWT)  AS RECGRSWT,  SUM(S.STNWT)  AS RECNETWT,  "
        strSql += vbcrlf + " 0 AS RECEIPT,  SUM(S.STNAMT) AS PAYMENT,  "
        strSql += vbcrlf + " 2 AS RESULT, '' AS AVERAGE, '' AS RATE, "
        strSql += vbcrlf + " 'D' COLHEAD  FROM " & cnStockDb & "..RECEIPTSTONE S "
        strSql += vbcrlf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON S.STNITEMID =I.ITEMID "
        strSql += vbcrlf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON I.METALID=M.METALID"
        strSql += vbcrlf + " WHERE S.TRANTYPE='PU' AND S.BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & "..RECEIPT I "
        strSql += vbcrlf + " WHERE I.TRANDATE BETWEEN '" & DatePicker1.Value.ToString("yyyy-MM-dd") & "' "
        strSql += vbcrlf + " AND  '" & DatePicker1.Value.ToString("yyyy-MM-dd") & "' "
        strSql += vbcrlf + " " & Replace(StrFilter, "SYSTEMID", "S.SYSTEMID") & " AND ISNULL(CANCEL,'') = ''  AND TRANTYPE = 'PU' AND COMPANYID IN ('" & cnCompanyId & "'))"
        strSql += vbcrlf + " AND  (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) = 'D'  "
        strSql += vbcrlf + " AND S.COMPANYID IN ('" & cnCompanyId & "')"
        strSql += vbcrlf + " GROUP BY S.CATCODE,S.BATCHNO  "
        strSql += vbcrlf + " ) Y GROUP BY CATCODE, CATNAME, AVERAGE, RATE, COLHEAD, RESULT"
        strSql += vbcrlf + " ) X "
        Cmd = New OleDbCommand(strsql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()


        strsql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSPURCHASE)>0 "
        strSql += vbcrlf + " BEGIN "
        strSql += vbcrlf + " INSERT INTO TEMP" & systemId & "ABSPURCHASE(CATNAME, "
        strSql += vbcrlf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        strSql += vbcrlf + " RESULT, RESULT1, COLHEAD) "
        strSql += vbcrlf + " SELECT 'PURCHASE'CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT, "
        strSql += vbcrlf + " 0 RECPCS, 0 RECGRSWT, 0 RECNETWT, 0 RECEIPT, 0 PAYMENT, 0 RESULT, 0 AS RESULT1, 'T' COLHEAD "
        strSql += vbcrlf + " END "
        Cmd = New OleDbCommand(strsql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()


        strsql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSPURCHASE)>0 "
        strSql += vbcrlf + " BEGIN "
        strSql += vbcrlf + " INSERT INTO TEMP" & systemId & "ABSPURCHASE(CATCODE, CATNAME,ISSPCS, ISSGRSWT, ISSNETWT, "
        strSql += vbcrlf + " RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT, RESULT, RESULT1, COLHEAD) "
        strsql += vbCrLf + " SELECT 'Z' AS CATCODE, 'PURCHASE TOTAL'CATNAME,SUM(ISSPCS),SUM(ISSGRSWT),SUM(ISSNETWT), "
        strSql += vbcrlf + " SUM(RECPCS),SUM(RECGRSWT),SUM(RECNETWT), SUM(RECEIPT), "
        strSql += vbcrlf + " SUM(PAYMENT),4 RESULT, 2 AS RESULT1, 'S' COLHEAD "
        strSql += vbcrlf + " FROM TEMP" & systemId & "ABSPURCHASE WHERE RESULT = 1 "
        strSql += vbcrlf + " END "
        Cmd = New OleDbCommand(strsql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()


        strsql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSPURCHASE)>0"
        strSql += vbcrlf + " BEGIN "
        'strsql += "INSERT INTO TEMP" & systemId & "SASRPU"
        'strSql += vbcrlf + " (DESCRIPTION, COLHEAD) "
        'strSql += vbcrlf + " SELECT 'PURCHASE ['+ CONVERT(VARCHAR,SUM(PAYMENT))+']' ,'T' "
        'strSql += vbcrlf + " FROM TEMP" & systemId & "ABSPURCHASE WHERE RESULT1 = 1"
        strSql += vbcrlf + " INSERT INTO TEMP" & systemId & "SASRPU"
        strsql += vbCrLf + " (DESCRIPTION,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,COLHEAD,RESULT1) "
        strsql += vbCrLf + " SELECT CATNAME, "
        strSql += vbcrlf + " CASE WHEN RECPCS   <> 0 THEN RECPCS   ELSE NULL END RECPCS, "
        strSql += vbcrlf + " CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END RECGRSWT, "
        strSql += vbcrlf + " CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END RECNETWT, "
        strSql += vbcrlf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
        strSql += vbcrlf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
        strSql += vbcrlf + " CASE WHEN AVERAGE <> '' THEN (AVERAGE + ' -- ' + RATE) ELSE '' END, COLHEAD, RESULT1  "
        strSql += vbcrlf + " FROM TEMP" & systemId & "ABSPURCHASE "
        strsql += vbCrLf + " ORDER BY CATCODE, RESULT"
        strsql += vbCrLf + " END "
        Cmd = New OleDbCommand(strsql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub Procsales()
        'strsql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "TEMPCOUNTER') DROP TABLE TEMP" & systemId & "TEMPCOUNTER"
        'strsql += vbCrLf + "SELECT "
        'strsql += vbCrLf + "(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=D.CATCODE)PARTICULAR,"
        'strsql += vbCrLf + "(SELECT CASHNAME FROM " & cnAdminDb & " ..CASHCOUNTER WHERE CASHID =D.CASHID )COUNTER,"
        'strsql += vbCrLf + "(CASE WHEN TYPE='PU' THEN 'PURCHASE'"
        'strsql += vbCrLf + "WHEN TYPE='SR' THEN 'SALES RETURN' ELSE 'PARTLY SALES' END"
        'strsql += vbCrLf + ")TYPE ,"
        'strsql += vbCrLf + "(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=D.CATCODE)CATEGORY"
        'strsql += vbCrLf + ",SYSTEMID,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT, 1 AS RESULT,NULL AS COLHEAD"
        'strsql += vbCrLf + "INTO TEMP" & systemId & "TEMPCOUNTER"
        'strsql += vbCrLf + "FROM "
        'strsql += vbCrLf + " " & cnStockDb & "..DENOMTRAN D WHERE 1=1"
        'strsql += vbCrLf + " AND TRANDATE BETWEEN '" & DatePicker1.Value.ToString("yyyy-MM-dd") & "' "
        'strsql += vbCrLf + " AND '" & DatePicker1.Value.ToString("yyyy-MM-dd") & "' AND  DEN_ID ='0' "
        'strsql += vbCrLf + " AND  SYSTEMID in('" & cmbnodeid_OWN.Text & "')"
        'strsql += vbCrLf + "GROUP BY CASHID ,CATCODE,SYSTEMID,TYPE"

        ''
        strsql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "TEMPCOUNTER') DROP TABLE TEMP" & systemId & "TEMPCOUNTER"
        strsql += vbCrLf + "SELECT PARTICULAR,COUNTER,TYPE,CATEGORY,SYSTEMID,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(GRSWT-NETWT)DIFF,1 AS RESULT,NULL AS COLHEAD"
        strsql += vbCrLf + "INTO TEMP" & systemId & "TEMPCOUNTER"
        strsql += vbCrLf + "FROM ("
        strsql += vbCrLf + "SELECT "
        strsql += vbCrLf + "(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=D.CATCODE)PARTICULAR,"
        strsql += vbCrLf + "(SELECT CASHNAME FROM " & cnAdminDb & " ..CASHCOUNTER WHERE CASHID =D.CASHID )COUNTER,"
        strsql += vbCrLf + "(CASE WHEN TYPE='PU' THEN 'PURCHASE'"
        strsql += vbCrLf + "WHEN TYPE='SR' THEN 'SALES RETURN' ELSE 'PARTLY SALES' END"
        strsql += vbCrLf + ")TYPE ,"
        strsql += vbCrLf + "(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=D.CATCODE)CATEGORY"
        strsql += vbCrLf + ",SYSTEMID,SUM(PCS)PCS,SUM(GRSWT)GRSWT,0 NETWT"
        strsql += vbCrLf + " FROM "
        strsql += vbCrLf + " " & cnStockDb & "..DENOMTRAN D WHERE 1=1"
        strsql += vbCrLf + "AND TRANDATE BETWEEN '" & DatePicker1.Value.ToString("yyyy-MM-dd") & "' "
        strsql += vbCrLf + "AND '" & DatePicker1.Value.ToString("yyyy-MM-dd") & "' AND  DEN_ID ='0' "
        strsql += vbCrLf + " AND  SYSTEMID in('" & cmbnodeid_OWN.Text & "')"
        strsql += vbCrLf + "GROUP BY CASHID ,CATCODE,SYSTEMID,TYPE"
        strsql += vbCrLf + "UNION ALL"
        strsql += vbCrLf + "SELECT "
        strsql += vbCrLf + "(SELECT CATNAME FROM " & cnAdminDb & " ..CATEGORY WHERE CATCODE =R.CATCODE )PARTICULAR,"
        strsql += vbCrLf + "(SELECT CASHNAME FROM " & cnAdminDb & " ..CASHCOUNTER WHERE CASHID =R.CASHID )COUNTER,"
        strsql += vbCrLf + "(CASE WHEN TRANTYPE='SR' THEN 'SALES RETURN'END)TYPE ,"
        strsql += vbCrLf + "(SELECT CATNAME FROM " & cnAdminDb & " ..CATEGORY WHERE CATCODE =R.CATCODE )CATEGORY,"
        strsql += vbCrLf + "SYSTEMID,SUM(PCS)PCS,0 GRSWT,SUM(GRSWT)NETWT"
        strsql += vbCrLf + "FROM " & cnStockDb & " ..RECEIPT R "
        strsql += vbCrLf + "WHERE TRANDATE = '" & DatePicker1.Value.ToString("yyyy-MM-dd") & "' "
        strsql += vbCrLf + "AND TRANTYPE ='SR' "
        strsql += vbCrLf + " AND  SYSTEMID in('" & cmbnodeid_OWN.Text & "') AND ISNULL(CANCEL,'') <>'Y'"
        strsql += vbCrLf + "GROUP BY CATCODE ,CASHID,TRANTYPE ,SYSTEMID"
        strsql += vbCrLf + "UNION ALL"
        strsql += vbCrLf + "SELECT "
        strsql += vbCrLf + "(SELECT CATNAME FROM " & cnAdminDb & " ..CATEGORY WHERE CATCODE =R.CATCODE )PARTICULAR,"
        strsql += vbCrLf + "(SELECT CASHNAME FROM " & cnAdminDb & " ..CASHCOUNTER WHERE CASHID =R.CASHID )COUNTER,"
        strsql += vbCrLf + "(CASE WHEN TRANTYPE='PU' THEN 'PURCHASE'END)TYPE ,"
        strsql += vbCrLf + "(SELECT CATNAME FROM " & cnAdminDb & " ..CATEGORY WHERE CATCODE =R.CATCODE )CATEGORY,"
        strsql += vbCrLf + "SYSTEMID,SUM(PCS)PCS,0 GRSWT,SUM(GRSWT)NETWT"
        strsql += vbCrLf + "FROM " & cnStockDb & " ..RECEIPT R "
        strsql += vbCrLf + "WHERE TRANDATE ='" & DatePicker1.Value.ToString("yyyy-MM-dd") & "' "
        strsql += vbCrLf + "AND TRANTYPE ='PU' "
        strsql += vbCrLf + " AND  SYSTEMID in('" & cmbnodeid_OWN.Text & "') AND ISNULL(CANCEL,'') <>'Y'"
        strsql += vbCrLf + "GROUP BY CATCODE ,CASHID,TRANTYPE ,SYSTEMID"
        strsql += vbCrLf + "UNION ALL"
        strsql += vbCrLf + "SELECT CATNAME AS PARTICULAR,COUNTER ,'PARTLY SALES' AS TYPE ,CATNAME AS CATEGORY,SYSTEMID,SUM(ISSPCS)PCS,0 AS GRSWT,SUM(ISSGRSWT)NETWT"
        strsql += vbCrLf + "FROM"
        strsql += vbCrLf + "("
        strsql += vbCrLf + "SELECT CATNAME, SUM(TAGPCS)-SUM(ISSPCS)ISSPCS ,SUM(TAGGRSWT)-SUM(ISSGRSWT)ISSGRSWT ,SUM(TAGNETWT)-SUM(ISSNETWT)ISSNETWT"
        strsql += vbCrLf + ",SUM(STNPCS)STNPCS, SUM(STNWT)STNWT,SYSTEMID,"
        strsql += vbCrLf + "COUNTER,1 AS RESULT1, ' ' AS COLHEAD "
        strsql += vbCrLf + "FROM ("
        strsql += vbCrLf + "SELECT (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
        strsql += vbCrLf + "WHERE CATCODE = I.CATCODE)AS CATNAME   "
        strsql += vbCrLf + ",I.PCS AS ISSPCS, I.GRSWT AS ISSGRSWT, I.NETWT AS ISSNETWT "
        strsql += vbCrLf + ",(SELECT ISNULL(SUM(STNPCS),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)STNPCS "
        strsql += vbCrLf + ",(SELECT ISNULL(SUM(TAGSTNPCS),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)TAGSTNPCS "
        strsql += vbCrLf + ",(SELECT ISNULL(SUM(STNWT),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO )STNWT "
        strsql += vbCrLf + ",(SELECT ISNULL(SUM(TAGSTNWT),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)TAGSTNWT "
        strsql += vbCrLf + ",TAGPCS,TAGGRSWT,TAGNETWT,SYSTEMID "
        strsql += vbCrLf + ",(SELECT CASHNAME FROM " & cnAdminDb & " ..CASHCOUNTER WHERE CASHID =I.CASHID )COUNTER"
        strsql += vbCrLf + "FROM " & cnStockDb & "..ISSUE I "
        strsql += vbCrLf + "WHERE I.TRANDATE BETWEEN '" & DatePicker1.Value.ToString("yyyy-MM-dd") & "'"
        strsql += vbCrLf + "AND '" & DatePicker1.Value.ToString("yyyy-MM-dd") & "' "
        strsql += vbCrLf + "AND I.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T')"
        strsql += vbCrLf + "AND I.TAGNO <> ''"
        strsql += vbCrLf + "AND ISNULL(I.CANCEL,'') = ''   "
        strsql += vbCrLf + " AND  SYSTEMID in('" & cmbnodeid_OWN.Text & "')"
        strsql += vbCrLf + ") X "
        strsql += vbCrLf + "WHERE ((TAGPCS)<>ISSPCS OR (TAGGRSWT)<> ISSGRSWT OR (TAGNETWT)<>ISSNETWT "
        strsql += vbCrLf + ") GROUP BY CATNAME,SYSTEMID,COUNTER"
        strsql += vbCrLf + "HAVING SUM(TAGPCS)-SUM(ISSPCS) <> 0 OR SUM(TAGGRSWT)-SUM(ISSGRSWT) <> 0"
        strsql += vbCrLf + "UNION ALL"
        strsql += vbCrLf + "SELECT CATNAME, SUM(TAGPCS)-SUM(ISSPCS)ISSPCS ,SUM(TAGGRSWT)-SUM(ISSGRSWT)ISSGRSWT ,SUM(TAGNETWT)-SUM(ISSNETWT)ISSNETWT"
        strsql += vbCrLf + ",SUM(STNPCS)STNPCS, SUM(STNWT)STNWT,SYSTEMID, "
        strsql += vbCrLf + "(SELECT CASHNAME FROM " & cnAdminDb & " ..CASHCOUNTER WHERE CASHID =x.cashid )COUNTER"
        strsql += vbCrLf + ",1 AS RESULT1, ' ' AS COLHEAD "
        strsql += vbCrLf + "FROM ("
        strsql += vbCrLf + "SELECT (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
        strsql += vbCrLf + "WHERE CATCODE = ISS.CATCODE)AS CATNAME   "
        strsql += vbCrLf + ",ISS.STNPCS AS ISSPCS, ISS.STNWT AS ISSGRSWT, ISS.STNWT AS ISSNETWT "
        strsql += vbCrLf + ",STNPCS "
        strsql += vbCrLf + ",TAGSTNPCS "
        strsql += vbCrLf + ",STNWT "
        strsql += vbCrLf + ",TAGSTNWT "
        strsql += vbCrLf + ",TAGSTNPCS AS TAGPCS,TAGSTNWT AS TAGGRSWT,TAGSTNWT AS TAGNETWT,SYSTEMID "
        strsql += vbCrLf + ",(SELECT CASHID FROM " & cnStockDb & "..issue WHERE SNO  =ISS.ISSSNO )cashid"
        strsql += vbCrLf + "FROM " & cnStockDb & "..ISSSTONE ISS "
        strsql += vbCrLf + "WHERE ISS.TRANDATE BETWEEN '" & DatePicker1.Value.ToString("yyyy-MM-dd") & "'"
        strsql += vbCrLf + "AND '" & DatePicker1.Value.ToString("yyyy-MM-dd") & "' "
        strsql += vbCrLf + "AND ISSSNO IN(SELECT SNO FROM  " & cnStockDb & "..ISSUE I  "
        strsql += vbCrLf + "WHERE I.TRANDATE BETWEEN '" & DatePicker1.Value.ToString("yyyy-MM-dd") & "'"
        strsql += vbCrLf + "AND '" & DatePicker1.Value.ToString("yyyy-MM-dd") & "' "
        strsql += vbCrLf + "AND I.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T')"
        strsql += vbCrLf + "AND I.TAGNO <> ''"
        strsql += vbCrLf + "AND ISNULL(I.CANCEL,'') = ''   "
        strsql += vbCrLf + " AND  SYSTEMID in('" & cmbnodeid_OWN.Text & "')"
        strsql += vbCrLf + ") ) X "
        strsql += vbCrLf + "WHERE ((TAGPCS)<>ISSPCS OR (TAGGRSWT)<> ISSGRSWT OR (TAGNETWT)<>ISSNETWT "
        strsql += vbCrLf + ") GROUP BY CATNAME,SYSTEMID,cashid "
        strsql += vbCrLf + "HAVING SUM(TAGPCS)-SUM(ISSPCS) <> 0 OR SUM(TAGGRSWT)-SUM(ISSGRSWT) <> 0"
        strsql += vbCrLf + ")Y GROUP BY CATNAME,SYSTEMID,COUNTER"
        strsql += vbCrLf + ")Z GROUP BY PARTICULAR ,COUNTER,TYPE,CATEGORY ,SYSTEMID  "


        cmd = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strsql = " alter table TEMP" & systemId & "TEMPCOUNTER alter column COLHEAD varchar(1) "
        cmd = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strsql = "  INSERT INTO TEMP" & systemId & "TEMPCOUNTER (PARTICULAR ,TYPE,RESULT,COLHEAD )"
        strsql += vbCrLf + "SELECT DISTINCT TYPE,TYPE,0,'T' FROM  TEMP" & systemId & "TEMPCOUNTER WHERE RESULT='1'"
        cmd = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strsql = " INSERT INTO TEMP" & systemId & "TEMPCOUNTER(PARTICULAR,TYPE,PCS,GRSWT,NETWT,RESULT,COLHEAD)"
        strsql += vbCrLf + "SELECT TYPE+'--TOTAL',TYPE,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,2,'S' FROM TEMP" & systemId & "TEMPCOUNTER WHERE RESULT='1'"
        strsql += vbCrLf + "GROUP BY TYPE "
        cmd = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strsql = "ALTER TABLE TEMP" & systemId & "TEMPCOUNTER ALTER COLUMN PCS  VARCHAR(15)"
        strsql += vbCrLf + "ALTER TABLE TEMP" & systemId & "TEMPCOUNTER ALTER COLUMN GRSWT VARCHAR(15)"
        cmd = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strsql = " alter table TEMP" & systemId & "TEMPCOUNTER add STATUS varchar(15) "
        cmd = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Sub

    Private Sub Procreportvalues()
        ''PARTLY SALES
        strsql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSPARTLYSALE') DROP TABLE TEMP" & systemId & "ABSPARTLYSALE "
        strsql += vbCrLf + " SELECT CATNAME, SUM(TAGPCS)-SUM(ISSPCS)ISSPCS ,SUM(TAGGRSWT)-SUM(ISSGRSWT)ISSGRSWT ,SUM(TAGNETWT)-SUM(ISSNETWT)ISSNETWT"
        strsql += vbCrLf + " ,SUM(STNPCS)STNPCS, SUM(STNWT)STNWT, 1 AS RESULT1, ' ' AS COLHEAD "
        strsql += vbCrLf + " INTO TEMP" & systemId & "ABSPARTLYSALE FROM ("
        strsql += vbCrLf + " SELECT (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
        strsql += vbCrLf + " WHERE CATCODE = I.CATCODE)AS CATNAME   "
        strsql += vbCrLf + " ,I.PCS AS ISSPCS, I.GRSWT AS ISSGRSWT, I.NETWT AS ISSNETWT "
        strsql += vbCrLf + " ,(SELECT ISNULL(SUM(STNPCS),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)STNPCS "
        strsql += vbCrLf + " ,(SELECT ISNULL(SUM(TAGSTNPCS),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)TAGSTNPCS "
        strsql += vbCrLf + " ,(SELECT ISNULL(SUM(STNWT),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO )STNWT "
        strsql += vbCrLf + " ,(SELECT ISNULL(SUM(TAGSTNWT),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)TAGSTNWT "
        strsql += vbCrLf + " ,TAGPCS,TAGGRSWT,TAGNETWT FROM " & cnStockDb & "..ISSUE I "
        strsql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & DatePicker1.Value.ToString("yyyy-MM-dd") & "'"
        strsql += vbCrLf + " AND '" & DatePicker1.Value.ToString("yyyy-MM-dd") & "' "
        strsql += vbCrLf + " AND I.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T')"
        strsql += vbCrLf + " AND I.TAGNO <> ''"
        strsql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''   "
        strsql += vbCrLf + " AND SYSTEMID ='" & cmbnodeid_OWN.Text & "' "
        strsql += vbCrLf + " AND I.COMPANYID IN ('" & cnCompanyId & "')"
        strsql += StrFilter
        strsql += vbCrLf + " ) X "
        strsql += vbCrLf + " WHERE ((TAGPCS)<>ISSPCS OR (TAGGRSWT)<> ISSGRSWT OR (TAGNETWT)<>ISSNETWT "
        strsql += vbCrLf + " ) GROUP BY CATNAME"
        strsql += vbCrLf + " HAVING SUM(TAGPCS)-SUM(ISSPCS) <> 0 OR SUM(TAGGRSWT)-SUM(ISSGRSWT) <> 0"
        'If RPT_SEPVAT_DABS Then
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT CATNAME, SUM(TAGPCS)-SUM(ISSPCS)ISSPCS ,SUM(TAGGRSWT)-SUM(ISSGRSWT)ISSGRSWT ,SUM(TAGNETWT)-SUM(ISSNETWT)ISSNETWT"
        strsql += vbCrLf + " ,SUM(STNPCS)STNPCS, SUM(STNWT)STNWT, 1 AS RESULT1, ' ' AS COLHEAD "
        strsql += vbCrLf + " FROM ("
        strsql += vbCrLf + " SELECT (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
        strsql += vbCrLf + " WHERE CATCODE = ISS.CATCODE)AS CATNAME   "
        strsql += vbCrLf + " ,ISS.STNPCS AS ISSPCS, ISS.STNWT AS ISSGRSWT, ISS.STNWT AS ISSNETWT "
        strsql += vbCrLf + " ,STNPCS "
        strsql += vbCrLf + " ,TAGSTNPCS "
        strsql += vbCrLf + " ,STNWT "
        strsql += vbCrLf + " ,TAGSTNWT "
        strsql += vbCrLf + " ,TAGSTNPCS AS TAGPCS,TAGSTNWT AS TAGGRSWT,TAGSTNWT AS TAGNETWT FROM " & cnStockDb & "..ISSSTONE ISS "
        strsql += vbCrLf + " WHERE ISS.TRANDATE BETWEEN '" & DatePicker1.Value.ToString("yyyy-MM-dd") & "'"
        strsql += vbCrLf + " AND '" & DatePicker1.Value.ToString("yyyy-MM-dd") & "' "
        strsql += vbCrLf + " AND ISSSNO IN(SELECT SNO FROM  " & cnStockDb & "..ISSUE I  "
        strsql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & DatePicker1.Value.ToString("yyyy-MM-dd") & "'"
        strsql += vbCrLf + " AND '" & DatePicker1.Value.ToString("yyyy-MM-dd") & "' "
        strsql += vbCrLf + " AND I.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T')"
        strsql += vbCrLf + " AND I.TAGNO <> ''"
        strsql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''   "
        strsql += vbCrLf + " AND SYSTEMID ='" & cmbnodeid_OWN.Text & "' "
        strsql += vbCrLf + " AND I.COMPANYID IN ('" & cnCompanyId & "')"
        strsql += StrFilter
        strsql += vbCrLf + " ) ) X "
        strsql += vbCrLf + " WHERE ((TAGPCS)<>ISSPCS OR (TAGGRSWT)<> ISSGRSWT OR (TAGNETWT)<>ISSNETWT "
        strsql += vbCrLf + " ) GROUP BY CATNAME"
        strsql += vbCrLf + " HAVING SUM(TAGPCS)-SUM(ISSPCS) <> 0 OR SUM(TAGGRSWT)-SUM(ISSGRSWT) <> 0"
        'End If
        cmd = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        ''PURCHASE
        strsql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMPPURCHASE') DROP TABLE TEMPPURCHASE"
        strsql += vbCrLf + "SELECT SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,(SELECT CATNAME FROM " & cnAdminDb & " ..CATEGORY WHERE CATCODE =R.CATCODE )CATEGORY"
        strsql += vbCrLf + "  INTO TEMPPURCHASE FROM " & cnStockDb & " ..RECEIPT R WHERE TRANDATE ='" & DatePicker1.Value.ToString("yyyy-MM-dd") & "' AND TRANTYPE ='PU' AND SYSTEMID ='" & cmbnodeid_OWN.Text & "' AND ISNULL(CANCEL,'') <>'Y'"
        strsql += vbCrLf + "GROUP BY CATCODE "
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()
        ''SALESRETURN

        strsql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMPSALESRETURN') DROP TABLE TEMPSALESRETURN"
        strsql += vbCrLf + "SELECT SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,(SELECT CATNAME FROM " & cnAdminDb & " ..CATEGORY WHERE CATCODE =R.CATCODE )CATEGORY"
        strsql += vbCrLf + "  INTO TEMPSALESRETURN FROM " & cnStockDb & " ..RECEIPT R WHERE TRANDATE ='" & DatePicker1.Value.ToString("yyyy-MM-dd") & "' AND TRANTYPE ='SR' AND SYSTEMID ='" & cmbnodeid_OWN.Text & "' AND ISNULL(CANCEL,'') <>'Y'"
        strsql += vbCrLf + "GROUP BY CATCODE "
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()
        ''CASH DETAILS
        strsql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMPCASH') DROP TABLE TEMPCASH"
        strsql += vbCrLf + " SELECT SUM((CASE WHEN A.TRANMODE = 'd' THEN AMOUNT ELSE -AMOUNT END)) AS AMOUNT "
        strsql += vbCrLf + " INTO TEMPCASH  from " & cnStockDb & " ..ACCTRAN a where TRANDATE ='" & DatePicker1.Value.ToString("yyyy-MM-dd") & "'AND SYSTEMID ='" & cmbnodeid_OWN.Text & "'"
        strsql += vbCrLf + "and PAYMODE='CA'  AND ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACGRPCODE = '1')"
        strsql += vbCrLf + "and isnull(CANCEL,'')<>'Y'AND FROMFLAG NOT IN ('','S','O','A')"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()
    End Sub

    Function AmtCalculation()
        txtTotNetAmt_OWN.Text = (Val(txtAmt2tho_OWN.Text) + Val(txtAmttho_OWN.Text) + Val(txtAmt5hrd_OWN.Text) + Val(txtAmt2hrd_OWN.Text) + Val(txtAmt1hrd_OWN.Text) _
        + Val(txtAmt50_OWN.Text) + Val(txtAmt20_OWN.Text) + Val(txtAmt10_OWN.Text) + Val(txtAmt5coin_OWN.Text) _
        + Val(txtAmt2_OWN.Text) + Val(txtAmt1_OWN.Text) + Val(txtAmtothers_OWN.Text))
        txtNetBal_OWN.Text = Val(txtTotNetAmt_OWN.Text)
        If Val(txtTotNetAmt_OWN.Text) > 0 Then
            txtTotNetAmt_OWN.ForeColor = Color.Black
        Else
            txtTotNetAmt_OWN.ForeColor = Color.LightBlue
        End If

        If Val(txtNetBal_OWN.Text) > 0 Then
            txtNetBal_OWN.ForeColor = Color.LightBlue
        Else
            txtNetBal_OWN.ForeColor = Color.Red
        End If
    End Function

    Function Funsave()
        Dim DENID As String

        BatchNo = GetNewBatchno(cnCostId, DatePicker1.Value, tran)
        If BatchNo = "" Then
            MsgBox("Batchno is Empty")
            If Not tran Is Nothing Then tran.Rollback() : tran = Nothing
            Exit Function
        End If
        Dim tempprintcount As Integer = GetSqlValue(cn, "SELECT DISTINCT ISNULL(PRINTCOUNT,0)PRINTCOUNT FROM " & cnStockDb & "..DENOMTRAN WHERE 1=1  AND TRANDATE='" & Format(DatePicker1.Value, "yyyy-MM-dd") & "' AND SYSTEMID='" & cmbnodeid_OWN.Text & "'")
        If tempprintcount = 0 Then
            tempprintcount = 1
        Else
            tempprintcount = tempprintcount + 1
        End If
        If Val(txtTotNetAmt_OWN.Text) <> 0 Then
            strsql = "DELETE FROM " & cnStockDb & "..DENOMTRAN WHERE 1=1 " 'CASHID='" & cmbCntId_OWN.SelectedValue.ToString & "'"
            strsql += vbCrLf + " AND TRANDATE='" & Format(DatePicker1.Value, "yyyy-MM-dd") & "' AND SYSTEMID='" & cmbnodeid_OWN.Text & "'  "
            Dim cmd As New OleDbCommand
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
            If Val(txtDen2thous.Text) <> 0 Then
                DENID = GetDenid("2000")
                Insertdenom(batchno, DENID, DatePicker1.Value, cmbCntId_OWN.SelectedValue, Val(txtDen2thous.Text.ToString), Val(txtAmt2tho_OWN.Text.ToString), cmbnodeid_OWN.Text, tempprintcount)
            End If
            If Val(txtDenthous.Text) <> 0 Then
                DENID = GetDenid("1000")
                Insertdenom(batchno, DENID, DatePicker1.Value, cmbCntId_OWN.SelectedValue, Val(txtDenthous.Text.ToString), Val(txtAmttho_OWN.Text.ToString), cmbnodeid_OWN.Text, tempprintcount)
            End If
            If Val(txtden5hrd.Text) <> 0 Then
                DENID = GetDenid("500")
                Insertdenom(batchno, DENID, DatePicker1.Value, cmbCntId_OWN.SelectedValue, Val(txtden5hrd.Text.ToString), Val(txtAmt5hrd_OWN.Text.ToString), cmbnodeid_OWN.Text, tempprintcount)
            End If
            If Val(txtden2hrd.Text) <> 0 Then
                DENID = GetDenid("200")
                Insertdenom(batchno, DENID, DatePicker1.Value, cmbCntId_OWN.SelectedValue, Val(txtden2hrd.Text.ToString), Val(txtAmt2hrd_OWN.Text.ToString), cmbnodeid_OWN.Text, tempprintcount)
            End If
            If Val(txtden1hrd.Text) <> 0 Then
                DENID = GetDenid("100")
                Insertdenom(batchno, DENID, DatePicker1.Value, cmbCntId_OWN.SelectedValue, Val(txtden1hrd.Text.ToString), Val(txtAmt1hrd_OWN.Text.ToString), cmbnodeid_OWN.Text, tempprintcount)
            End If
            If Val(txtden50.Text) <> 0 Then
                DENID = GetDenid("50")
                Insertdenom(batchno, DENID, DatePicker1.Value, cmbCntId_OWN.SelectedValue, Val(txtden50.Text.ToString), Val(txtAmt50_OWN.Text.ToString), cmbnodeid_OWN.Text, tempprintcount)
            End If
            If Val(txtden20_OWN.Text) <> 0 Then
                DENID = GetDenid("20")
                Insertdenom(batchno, DENID, DatePicker1.Value, cmbCntId_OWN.SelectedValue, Val(txtden20_OWN.Text.ToString), Val(txtAmt20_OWN.Text.ToString), cmbnodeid_OWN.Text, tempprintcount)
            End If
            If Val(txtden10_OWN.Text) <> 0 Then
                DENID = GetDenid("10")
                Insertdenom(batchno, DENID, DatePicker1.Value, cmbCntId_OWN.SelectedValue, Val(txtden10_OWN.Text.ToString), Val(txtAmt10_OWN.Text.ToString), cmbnodeid_OWN.Text, tempprintcount)
            End If
            If Val(txtden5_OWN.Text) <> 0 Then
                DENID = GetDenid("5")
                Insertdenom(batchno, DENID, DatePicker1.Value, cmbCntId_OWN.SelectedValue, Val(txtden5_OWN.Text.ToString), Val(txtAmt5coin_OWN.Text.ToString), cmbnodeid_OWN.Text, tempprintcount)
            End If
            If Val(txtden2_OWN.Text) <> 0 Then
                DENID = GetDenid("2")
                Insertdenom(batchno, DENID, DatePicker1.Value, cmbCntId_OWN.SelectedValue, Val(txtden2_OWN.Text.ToString), Val(txtAmt2_OWN.Text.ToString), cmbnodeid_OWN.Text, tempprintcount)
            End If
            If Val(txtden1_OWN.Text) <> 0 Then
                DENID = GetDenid("1")
                Insertdenom(batchno, DENID, DatePicker1.Value, cmbCntId_OWN.SelectedValue, Val(txtden1_OWN.Text.ToString), Val(txtAmt1_OWN.Text.ToString), cmbnodeid_OWN.Text, tempprintcount)
            End If
            If Val(txtden1_OTHERS.Text) <> 0 Then
                DENID = GetDenid("1")
                Insertdenom(batchno, DENID, DatePicker1.Value, cmbCntId_OWN.SelectedValue, Val(txtden1_OTHERS.Text.ToString), Val(txtAmtothers_OWN.Text.ToString), cmbnodeid_OWN.Text, tempprintcount)
            End If
            For i As Integer = 0 To tempdt.Rows.Count - 1
                'BATCHNO,TRANDATE,CASHID,DEN_ID,DEN_QTY,DEN_AMOUNT,TYPE,CATCODE,SYSTEMID,PCS,GRSWT,NETWT
                InsertdenomWT(batchno, "0", DatePicker1.Value, cmbCntId_OWN.SelectedValue, "0", "0", tempdt.Rows(i).Item("TYPE").ToString, tempdt.Rows(i).Item("CATCODE").ToString, cmbnodeid_OWN.Text, tempdt.Rows(i).Item("PCS").ToString, tempdt.Rows(i).Item("GRSWT").ToString, tempdt.Rows(i).Item("NETWT").ToString, tempprintcount)
                'If MsgBox("Do You Want Print....!", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                '    PrintFunc()
                'End If
            Next
            InitializeFunc()
        Else
            MsgBox("Denomination Should not be Empty...!")
            DatePicker1.Focus()
        End If
    End Function

    Function PrintFunc()
        Dim dttt As New DataTable
        If cmbCntId_OWN.Text.ToString <> "" And cmbCntId_OWN.Text.ToString <> "ALL" Then
            Filteration()
            TableCreation()
            ' ProcPurchase()
            'ProcPartlySales()
            Procreportvalues()
            Procsales()

            strsql = " SELECT  "
            strsql += vbCrLf + " DESCRIPTION,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,COLHEAD"
            strsql += vbCrLf + " FROM TEMP" & systemId & "SASRPU"
            strsql += vbCrLf + " ORDER BY SNO"
            cmd = New OleDbCommand(strsql, cn)
            da = New OleDbDataAdapter(cmd)
            dt = New DataTable
            da.Fill(dttt)

            strsql = " SELECT PARTICULAR AS DESCRIPTION,'' AS RECPCS,  "
            strsql += vbCrLf + " GRSWT AS RECGRSWT,NETWT AS RECNETWT,TYPE,CATEGORY,COLHEAD,STATUS,DIFF "
            strsql += vbCrLf + " FROM TEMP" & systemId & "TEMPCOUNTER"
            strsql += vbCrLf + " ORDER BY TYPE,RESULT"
            cmd = New OleDbCommand(strsql, cn)
            da = New OleDbDataAdapter(cmd)
            dt = New DataTable
            da.Fill(dt)

          'DENOMINATION            

            Dim _Tally As Boolean = False
            For Each kro As DataRow In dt.Rows
                'Dim _CATCode As String = objGPack.GetSqlValue("SELECT TOP 1 CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME ='" & kro!CATEGORY.ToString & "'", "")
                If kro!COLHEAD.ToString <> "" Then Continue For
                If kro!TYPE = "PARTLY SALES" Then
                    strsql = "SELECT SUM(ISSGRSWT)GRSWT FROM TEMP" & systemId & "ABSPARTLYSALE WHERE RESULT1=1 AND CATNAME='" & kro!CATEGORY.ToString & "'"
                    If Val(objGPack.GetSqlValue(strsql, , "")) < Val(kro!RECGRSWT.ToString) Then
                        kro!STATUS = "EXCESS"
                    ElseIf Val(objGPack.GetSqlValue(strsql, , "")) = Val(kro!RECGRSWT.ToString) Then
                        kro!STATUS = "TALLY"
                    Else
                        kro!STATUS = "SORT"
                    End If
                ElseIf kro!TYPE = "PURCHASE" Then
                    strsql = "SELECT SUM(GRSWT)GRSWT FROM TEMPPURCHASE WHERE  CATEGORY='" & kro!CATEGORY.ToString & "'"
                    If Val(objGPack.GetSqlValue(strsql, , "")) < Val(kro!RECGRSWT.ToString) Then
                        kro!STATUS = "EXCESS"
                    ElseIf Val(objGPack.GetSqlValue(strsql, , "")) = Val(kro!RECGRSWT.ToString) Then
                        kro!STATUS = "TALLY"
                    Else
                        kro!STATUS = "NOT-TALLY"
                    End If
                ElseIf kro!TYPE = "SALES RETURN" Then
                    strsql = "SELECT SUM(GRSWT)GRSWT FROM TEMPSALESRETURN WHERE  CATEGORY='" & kro!CATEGORY.ToString & "'"
                    If Val(objGPack.GetSqlValue(strsql, , "")) < Val(kro!RECGRSWT.ToString) Then
                        kro!STATUS = "EXCESS"
                    ElseIf Val(objGPack.GetSqlValue(strsql, , "")) = Val(kro!RECGRSWT.ToString) Then
                        kro!STATUS = "TALLY"
                    Else
                        kro!STATUS = "NOT-TALLY"
                    End If
                Else
                    Continue For
                End If
            Next
            If dt.Rows.Count > 0 Then dt.AcceptChanges()

            Dim ro As DataRow
            ro = dt.NewRow
            dt.Rows.Add(ro)
            ro = dt.NewRow
            ro = dt.NewRow
            dt.Rows.Add(ro)
            ro = dt.NewRow
            ro("DESCRIPTION") = "DENOMINATION"
            ro("RECPCS") = "QTY"
            ro("RECGRSWT") = "AMOUNT"
            ro("COLHEAD") = "T"
            dt.Rows.Add(ro)
            ro = dt.NewRow
            ro("DESCRIPTION") = "2000 X"
            ro("RECPCS") = Val(txtDen2thous.Text).ToString
            ro("RECGRSWT") = Val(txtAmt2tho_OWN.Text).ToString
            dt.Rows.Add(ro)
            ro = dt.NewRow
            ro("DESCRIPTION") = "1000 X"
            ro("RECPCS") = Val(txtDenthous.Text).ToString
            ro("RECGRSWT") = Val(txtAmttho_OWN.Text).ToString
            dt.Rows.Add(ro)
            ro = dt.NewRow
            ro("DESCRIPTION") = "500 X"
            ro("RECPCS") = Val(txtden5hrd.Text).ToString
            ro("RECGRSWT") = Val(txtAmt5hrd_OWN.Text).ToString
            dt.Rows.Add(ro)
            ro = dt.NewRow
            ro("DESCRIPTION") = "200 X"
            ro("RECPCS") = Val(txtden2hrd.Text).ToString
            ro("RECGRSWT") = Val(txtAmt2hrd_OWN.Text).ToString
            dt.Rows.Add(ro)
            ro = dt.NewRow
            ro("DESCRIPTION") = "100 X"
            ro("RECPCS") = Val(txtden1hrd.Text).ToString
            ro("RECGRSWT") = Val(txtAmt1hrd_OWN.Text).ToString
            dt.Rows.Add(ro)
            ro = dt.NewRow
            ro("DESCRIPTION") = "50 X"
            ro("RECPCS") = Val(txtden50.Text).ToString
            ro("RECGRSWT") = Val(txtAmt50_OWN.Text).ToString
            dt.Rows.Add(ro)
            ro = dt.NewRow
            ro("DESCRIPTION") = "20 X"
            ro("RECPCS") = Val(txtden20_OWN.Text).ToString
            ro("RECGRSWT") = Val(txtAmt20_OWN.Text).ToString
            dt.Rows.Add(ro)
            ro = dt.NewRow
            ro("DESCRIPTION") = "10 X"
            ro("RECPCS") = Val(txtden10_OWN.Text).ToString
            ro("RECGRSWT") = Val(txtAmt10_OWN.Text).ToString
            dt.Rows.Add(ro)
            ro = dt.NewRow
            ro("DESCRIPTION") = "5 X"
            ro("RECPCS") = Val(txtden5_OWN.Text).ToString
            ro("RECGRSWT") = Val(txtAmt5coin_OWN.Text).ToString
            dt.Rows.Add(ro)
            ro = dt.NewRow
            ro("DESCRIPTION") = "2 X"
            ro("RECPCS") = Val(txtden2_OWN.Text).ToString
            ro("RECGRSWT") = Val(txtAmt2_OWN.Text).ToString
            dt.Rows.Add(ro)
            ro = dt.NewRow
            ro("DESCRIPTION") = "1 X"
            ro("RECPCS") = Val(txtden1_OWN.Text).ToString
            ro("RECGRSWT") = Val(txtAmt1_OWN.Text).ToString
            dt.Rows.Add(ro)
            ro = dt.NewRow
            ro("DESCRIPTION") = "OTHERS X"
            ro("RECPCS") = Val(txtden1_OTHERS.Text).ToString
            ro("RECGRSWT") = Val(txtAmtothers_OWN.Text).ToString
            dt.Rows.Add(ro)
            ro = dt.NewRow
            dt.Rows.Add(ro)
            ro = dt.NewRow
            ro("DESCRIPTION") = "TOTAL"
            ro("RECGRSWT") = Val(txtTotNetAmt_OWN.Text).ToString
            dt.Rows.Add(ro)
            ro = dt.NewRow
            ro("DESCRIPTION") = "BALANCE"
            ro("RECGRSWT") = Val(txtNetBal_OWN.Text).ToString
            strsql = "SELECT ISNULL(SUM(AMOUNT),0)AMOUNT  FROM TEMPCASH "
            Dim tempamt As Decimal = GetSqlValue(cn, "SELECT ISNULL(SUM(AMOUNT),0)AMOUNT  FROM TEMPCASH ")
            ro("RECNETWT") = tempamt
            ro("DIFF") = Val(txtNetBal_OWN.Text) - tempamt
            If Val(objGPack.GetSqlValue(strsql, , "")) < Val(txtNetBal_OWN.Text) Then
                ro("STATUS") = "EXCESS"
            ElseIf Val(objGPack.GetSqlValue(strsql, , "")) = Val(txtNetBal_OWN.Text) Then
                ro("STATUS") = "TALLY"
            Else
                ro("STATUS") = "SORT"
            End If
            dt.Rows.Add(ro)
            ''
            ro = dt.NewRow
            dt.Rows.Add(ro)
            ro = dt.NewRow
            ro = dt.NewRow
            dt.Rows.Add(ro)
            ro = dt.NewRow
            ro("DESCRIPTION") = "NUMBER OF TIME PRINTING :"
            Dim tempprintcount As Integer = GetSqlValue(cn, "SELECT DISTINCT ISNULL(PRINTCOUNT,0)PRINTCOUNT FROM " & cnStockDb & "..DENOMTRAN WHERE 1=1  AND TRANDATE='" & Format(DatePicker1.Value, "yyyy-MM-dd") & "' AND SYSTEMID='" & cmbnodeid_OWN.Text & "'")
            ro("RECPCS") = tempprintcount
            dt.Rows.Add(ro)
            If dt.Rows.Count > 0 Then
                gridSummary.DataSource = Nothing
                gridSummary.DataSource = dt
                For Each gv As DataGridViewRow In gridSummary.Rows
                    With gv
                        Select Case .Cells("COLHEAD").Value.ToString
                            Case "T"
                                .DefaultCellStyle.BackColor = reportHeadStyle.BackColor
                                .DefaultCellStyle.Font = reportHeadStyle.Font
                                .DefaultCellStyle.ForeColor = reportHeadStyle.ForeColor
                            Case "S"
                                .DefaultCellStyle.BackColor = reportHeadStyle2.BackColor
                                .DefaultCellStyle.Font = reportHeadStyle2.Font
                                .DefaultCellStyle.ForeColor = reportHeadStyle2.ForeColor
                        End Select
                        Select Case .Cells("DESCRIPTION").Value.ToString
                            Case "NUMBER OF TIME PRINTING :"
                                .Visible = False
                        End Select
                    End With
                Next
                gridSummary.ColumnHeadersDefaultCellStyle.Font = reportHeadStyle.Font
                If gridSummary.Columns.Contains("CATEGORY") Then gridSummary.Columns("CATEGORY").Visible = False
                If gridSummary.Columns.Contains("TYPE") Then gridSummary.Columns("TYPE").Visible = False
                If gridSummary.Columns.Contains("AVERAGE") Then gridSummary.Columns("AVERAGE").Visible = False
                If gridSummary.Columns.Contains("COLHEAD") Then gridSummary.Columns("COLHEAD").Visible = False
                If gridSummary.Columns.Contains("RECEIPT") Then gridSummary.Columns("RECEIPT").Visible = False
                If gridSummary.Columns.Contains("PAYMENT") Then gridSummary.Columns("PAYMENT").Visible = False

                'If dt.Select("NUMBER OF TIME PRINTING :") Then gridSummary.Rows("NUMBER OF TIME PRINTING :").Visible = False
                If gridSummary.Columns.Contains("DIFF") Then
                    gridSummary.Columns("DIFF").Visible = False
                    gridSummary.Columns("DIFF").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End If
                If gridSummary.Columns.Contains("RECPCS") Then
                    gridSummary.Columns("RECPCS").HeaderText = "PCS"
                    gridSummary.Columns("RECPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End If
                If gridSummary.Columns.Contains("RECGRSWT") Then
                    gridSummary.Columns("RECGRSWT").HeaderText = "GRSWT"
                    gridSummary.Columns("RECGRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End If
                If gridSummary.Columns.Contains("RECNETWT") Then
                    gridSummary.Columns("RECNETWT").HeaderText = "ACTUAL GRSWT"
                    gridSummary.Columns("RECNETWT").Visible = False
                    gridSummary.Columns("RECNETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End If
                gridSummary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridSummary.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridSummary.Columns
                    dgvCol.Width = dgvCol.Width
                    dgvCol.SortMode = DataGridViewColumnSortMode.NotSortable
                Next
                gridSummary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                gridSummary.DataSource = Nothing
            End If
        End If
    End Function
    Function GetDenid(ByVal denval As String) As String
        strsql = "SELECT DEN_ID FROM " & cnAdminDb & "..DENOMMAST WHERE DEN_VALUE='" & denval & "'"
        DT = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(DT)
        If DT.Rows.Count > 0 Then
            Return DT.Rows(0).Item("DEN_ID").ToString
        Else
            Return Nothing
        End If
    End Function
    Function Insertdenom(ByVal batchno As String, ByVal DEN_ID As String, ByVal gdate As DateTime, ByVal cashid As String, ByVal denqty As Double, ByVal denamt As Double, ByVal systemid As String, ByVal printcount As Integer) As Boolean
        strsql = "INSERT INTO " & cnStockDb & "..DENOMTRAN (BATCHNO,TRANDATE,CASHID,DEN_ID,DEN_QTY,DEN_AMOUNT,SYSTEMID,PRINTCOUNT)"
        strsql += vbCrLf + " VALUES ('" & batchno & "' ,'" & Format(gdate, "yyyy-MM-dd") & "' , '" & cashid & "' , " & DEN_ID & ""
        strsql += vbCrLf + "," & denqty & "," & denamt & ",'" & systemid & "','" & printcount & "')"
        Dim cmd As New OleDbCommand
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()
    End Function
    Function InsertdenomWT(ByVal batchno As String, ByVal DEN_ID As String, ByVal gdate As DateTime, ByVal cashid As String, ByVal denqty As Double, ByVal denamt As Double, ByVal type As String, ByVal catcode As String, ByVal systemid As String, ByVal pcs As Integer, ByVal grswt As Decimal, ByVal NETwt As Decimal, ByVal printcount As Integer) As Boolean
        strsql = "INSERT INTO " & cnStockDb & "..DENOMTRAN (BATCHNO,TRANDATE,CASHID,DEN_ID,DEN_QTY,DEN_AMOUNT,TYPE,CATCODE,SYSTEMID,PCS,GRSWT,NETWT,PRINTCOUNT)"
        strsql += vbCrLf + " VALUES ( '" & batchno & "','" & Format(gdate, "yyyy-MM-dd") & "' , '" & cashid & "' , " & DEN_ID & ""
        strsql += vbCrLf + "," & denqty & "," & denamt & ",'" & type & "','" & catcode & "','" & systemid & "'," & pcs & "," & grswt & "," & NETwt & ",'" & printcount & "')"
        Dim cmd As New OleDbCommand
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()
    End Function
#End Region
    Private Sub SaveToolStripMenuItem1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem1.Click
        btnSave_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub ExitToolStripMenuItem3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem3.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnView.Click
        FillData()
        ViewDenom(cmbnodeid_OWN.Text.ToString, DatePicker1.Value)
        PrintFunc()
    End Sub

    Private Sub ViewToolStripMenuItem2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ViewToolStripMenuItem2.Click
        btnView_Click(Me, New EventArgs)
    End Sub

    Private Sub ToolStripMenuItem1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem1.Click
        Button1_Click(Me, New EventArgs)
    End Sub

    Private Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        InitializeFunc()
        tempdt = New DataTable
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If gridSummary.Rows.Count > 0 Then
            Dim strLabel As String = ""
            strLabel = "CASH COUNTER : " + cmbCntId_OWN.Text.ToString + "(" & cmbnodeid_OWN.Text & ")" + Environment.NewLine
            strLabel += "DATE : " + DatePicker1.Text.ToString
            gridSummary.Columns("RECNETWT").Visible = True
            gridSummary.Columns("DIFF").Visible = True
            gridSummary.Columns("RECNETWT").HeaderText = "ACTUAL"
            For Each gv As DataGridViewRow In gridSummary.Rows
                With gv
                    Select Case .Cells("DESCRIPTION").Value.ToString
                        Case "NUMBER OF TIME PRINTING :"
                            .Visible = True
                    End Select
                End With
            Next
            BrightPosting.GExport.Post(Me.Name, strCompanyName, strLabel, gridSummary, BrightPosting.GExport.GExportType.Print)
            gridSummary.Columns("RECNETWT").Visible = False
            gridSummary.Columns("DIFF").Visible = False
            For Each gv As DataGridViewRow In gridSummary.Rows
                With gv
                    Select Case .Cells("DESCRIPTION").Value.ToString
                        Case "NUMBER OF TIME PRINTING :"
                            .Visible = False
                    End Select
                End With
            Next
        End If
    End Sub

    Private Sub Label4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label4.Click

    End Sub
    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbtype.SelectedIndexChanged

    End Sub
    Private Sub txtNewt_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNewt.KeyDown
        If e.KeyCode = Keys.Enter Then
            If txtNewt.Text = "" Then
                MsgBox("Netwt Should not Empty.", MsgBoxStyle.Information)
                Exit Sub
            End If
            Dim type As String
            Dim ro As DataRow
            ro = tempdt.NewRow
            tempdt.Rows.Add(ro)

            If cmbtype.Text = "Purchase" Then
                type = "PU"
            ElseIf cmbtype.Text = "PartlySales" Then
                type = "PS"
            ElseIf cmbtype.Text = "SalesReturn" Then
                type = "SR"
            End If
            ro("MODE") = cmbtype.Text
            ro("TYPE") = type
            ro("CATEGORY") = cmbcategory_OWN.Text
            ro("catcode") = cmbcategory_OWN.SelectedValue.ToString
            ro("PCS") = Val(txtpcs.Text)
            ro("GRSWT") = txtgrswt.Text
            If txtNewt.Text = "" And txtNewt.Text = 0 Then
                ro("NETWT") = "0"
            Else
                ro("NETWT") = txtNewt.Text
            End If
            grid.DataSource = Nothing
            grid.DataSource = tempdt
            grid.Visible = True
            grid.Columns("TYPE").Visible = False
            grid.Columns("CATCODE").Visible = False
            grid.Columns("MODE").HeaderText = "TYPE"
            grid.Columns("PCS").Width = 50
            grid.Columns("GRSWT").Width = 75
            grid.Columns("NETWT").Width = 75
            ' tempdt.Rows.Add(ro)
            FillData()
            cmbtype.Focus()
        End If
    End Sub

    Private Sub cmbnodeid_OWN_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbnodeid_OWN.SelectedIndexChanged

    End Sub

    Private Sub txtpcs_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtpcs.TextChanged

    End Sub

    Private Sub cmbtype_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbtype.KeyDown
        If e.KeyCode = Keys.Escape Then
            grid.Visible = False
            txtDen2thous.Focus()
        ElseIf e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAb}")
        End If
    End Sub

    Private Sub txtgrswt_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtgrswt.KeyDown
        If e.KeyCode = Keys.Enter Then
            If txtgrswt.Text = "" Then
                MsgBox("Grswt Should not Empty.", MsgBoxStyle.Information)
                Exit Sub
            End If
            Dim type As String

            For Each rk As DataRow In tempdt.Select("mode='" & cmbtype.Text.ToString & "' and CATEGORY='" & cmbcategory_OWN.Text.ToString() & "'")
                rk.Delete()
                tempdt.AcceptChanges()
            Next

            Dim ro As DataRow
            ro = tempdt.NewRow
            tempdt.Rows.Add(ro)
            If cmbtype.Text = "Purchase" Then
                type = "PU"
            ElseIf cmbtype.Text = "PartlySales" Then
                type = "PS"
            ElseIf cmbtype.Text = "SalesReturn" Then
                type = "SR"
            End If
            ro("MODE") = cmbtype.Text
            ro("TYPE") = type
            ro("CATEGORY") = cmbcategory_OWN.Text
            ro("catcode") = cmbcategory_OWN.SelectedValue.ToString
            ro("PCS") = Val(txtpcs.Text)
            ro("GRSWT") = txtgrswt.Text
            ro("NETWT") = Val(txtNewt.Text)
            
            

            ''tempdt = DirectCast(ViewState("tempdt"), DataTable)
            ''dt = dt.DefaultView.ToTable(True, "MODE", "CATEGORY")

            grid.DataSource = Nothing
            grid.DataSource = tempdt
            grid.Visible = True
            grid.Columns("TYPE").Visible = False
            grid.Columns("CATCODE").Visible = False
            grid.Columns("MODE").HeaderText = "TYPE"
            grid.Columns("PCS").Width = 50
            grid.Columns("GRSWT").Width = 75
            grid.Columns("NETWT").Visible = False
            'grid.Columns("NETWT").Width = 75
            ' tempdt.Rows.Add(ro)

            'tempdt = DeleteDuplicateFromDataTable(tempdt, "MODE", "CATEGORY")
            'tempdt.AcceptChanges()
            ' ''Dim r As Integer = 1
            ' ''Do
            ' ''    Dim dr As DataRow = dt.Rows(r - 1)
            ' ''    Do While dt.Rows(r).Item(columnIndex).ToString = dr.Item(columnIndex).ToString
            ' ''        dt.Rows.RemoveAt(r)
            ' ''        If r = dt.Rows.Count Then Exit Do
            ' ''    Loop
            ' ''    r += 1
            ' ''Loop Until r >= dt.Rows.Count

            FillData()
            cmbtype.Focus()
        End If
    End Sub
    Protected Function DeleteDuplicateFromDataTable(ByVal dtDuplicate As DataTable, ByVal columnName As String, ByVal columnName1 As String) As DataTable
        Dim hashT As New Hashtable()
        Dim arrDuplicate As New ArrayList()
        For Each row As DataRow In dtDuplicate.Rows
            If hashT.Contains(row(columnName)) And hashT.Contains(row(columnName1)) Then
                arrDuplicate.Add(row)
            Else
                hashT.Add(row(columnName), String.Empty)
            End If
        Next
        For Each row As DataRow In arrDuplicate
            dtDuplicate.Rows.Remove(row)
        Next

        Return dtDuplicate
    End Function

    Private Sub txtDen2thous_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDen2thous.TextChanged
        txtAmt2tho_OWN.Text = (Val(txtDen2thous.Text) * 2000).ToString
        AmtCalculation()
    End Sub

    Private Sub txtden2hrd_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtden2hrd.TextChanged
        txtAmt2hrd_OWN.Text = (Val(txtden2hrd.Text) * 200).ToString
        AmtCalculation()
    End Sub
End Class