Imports System.Data.OleDb
Imports System.Text.RegularExpressions
Public Class frmTradingAccount
    Dim strSql As String = ""
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim dt As DataTable
    Dim dtCostCentre As DataTable
    Dim dtCompany As DataTable
    Dim DtCostid As DataTable
    Dim Rempty As Boolean = False
    Dim CnStartMonth As String = "04"

    Private Sub frmTrading_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmTradingAccount_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
        dtpDate.Value = DateTime.Today.Date
        ''COSTCENTRE
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False

        strSql = " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N' "
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(cmbCompany, dtCompany, "COMPANYNAME", True)
        btnView.Focus()
        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click
        getreport()
        Prop_Sets()
    End Sub
    Private Sub getreport()
        gridView.DataSource = Nothing
        Dim dtdate As String = GetSqlValue(cn, "select startdate from " & cnAdminDb & "..DBMASTER where DBNAME ='" & cnStockDb & "'")
        strSql = " EXEC " & cnAdminDb & "..AGR_SP_VATFORM_ANNEXURE_8"
        strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
        strSql += vbCrLf + " ,@ADMINDB = '" & cnAdminDb & "'"
        strSql += vbCrLf + " ,@FROMDATE = '" & dtdate & "'"
        strSql += vbCrLf + " ,@TODATE = '" & Format(dtpDate.Value, "yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@METALNAME = 'ALL'"
        strSql += vbCrLf + " ,@COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " ,@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
        strSql += vbCrLf + " ,@SPLITPU = 'N'"
        'strSql += vbCrLf + " ,@GS11 = 'G'"
        'strSql += vbCrLf + " ,@GS12 = 'N'"
        'strSql += vbCrLf + " ,@GGS11RATE = 'Y'"
        'strSql += vbCrLf + " ,@SGS11RATE = 'Y'"
        strSql += vbCrLf + " ,@GS11 = '" & IIf(rbtGs11Grswt.Checked, "G", "N") & "'"
        strSql += vbCrLf + " ,@GS12 = '" & IIf(rbtGs12Grswt.Checked, "G", "N") & "'"
        strSql += vbCrLf + " ,@GGS11RATE = '" & IIf(ChkRate.Checked, "Y", "N") & "'"
        strSql += vbCrLf + " ,@SGS11RATE = '" & IIf(ChkSRate.Checked, "Y", "N") & "'"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = "IF OBJECT_ID('TEMPTABLEDB..TEMPTRADING','U')IS NOT NULL DROP TABLE  TEMPTABLEDB..TEMPTRADING"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = "SELECT SEP,LPARTICULAR,LAMT,RPARTICULAR,RAMT,RESULT "
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPTRADING"
        strSql += vbCrLf + " FROM ("
        ''GOLD
        strSql += vbCrLf + " SELECT 'G'SEP,'GOLD JEWELLERY & BULLION 'LPARTICULAR,'0.0'LAMT,''RPARTICULAR,'0.0'RAMT,0 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT 'G'SEP,'    TO OPENING STOCK'LPARTICULAR,"
        strSql += vbCrLf + " (SELECT SUM(AMOUNT)FROM  TEMPTABLEDB..TEMPANNEX8B  WHERE RESULT IN('1','11'))LAMT"
        strSql += vbCrLf + " ,'     BY SALES'RPARTICULAR,"
        strSql += vbCrLf + " (SELECT SUM(AMOUNT)FROM  TEMPTABLEDB..TEMPANNEX8B  WHERE RESULT IN('26'))SALES,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT 'G'SEP,'    TO PURCHASE'LPARTICULAR,"
        strSql += vbCrLf + " (SELECT SUM(AMOUNT)FROM  TEMPTABLEDB..TEMPANNEX8B  WHERE RESULT IN('2','18')) PURCHASE,"
        strSql += vbCrLf + " '     BY CLOSING STOCK 'RPARTICULAR,"
        strSql += vbCrLf + " (SELECT SUM(AMOUNT)FROM  TEMPTABLEDB..TEMPANNEX8B  WHERE RESULT IN('9','30'))CLOSEING,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT 'G'SEP,'    TO MAKING'LPARTICULAR,"
        strSql += vbCrLf + " (SELECT SUM(AMOUNT)FROM  TEMPTABLEDB..TEMPANNEX8B  WHERE RESULT IN('19'))MAKING,"
        strSql += vbCrLf + " ''RPARTICULAR,'0.0',1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT 'G'SEP,'    TO GROSS PROFIT'LPARTICULAR,'0.00'MAKING,''RPARTICULAR,'0.0',1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        ''SILVER
        strSql += vbCrLf + " SELECT 'S'SEP,'SILVER ARTICLES & JEWELLERY 'LPARTICULAR,'0.0'LAMT,''RPARTICULAR,'0.0'RAMT,0 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT 'S'SEP,'    TO OPEINIG STOCK 'LPARTICULAR,"
        strSql += vbCrLf + " (SELECT SUM(AMOUNT)FROM  TEMPTABLEDB..TEMPANNEX8B  WHERE RESULT IN('36','48'))OAMT"
        strSql += vbCrLf + " ,'     BY SALES'RPARTICULAR,"
        strSql += vbCrLf + " (SELECT SUM(AMOUNT)FROM  TEMPTABLEDB..TEMPANNEX8B  WHERE RESULT IN('59'))SALES,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT 'S'SEP,'    TO PURCHASE'LPARTICULAR,"
        strSql += vbCrLf + " (SELECT SUM(AMOUNT)FROM  TEMPTABLEDB..TEMPANNEX8B  WHERE RESULT IN('37','49'))PURCHASE,"
        strSql += vbCrLf + " '     BY CLOSING STOCK 'RPARTICULAR,"
        strSql += vbCrLf + " (SELECT SUM(AMOUNT)FROM  TEMPTABLEDB..TEMPANNEX8B  WHERE RESULT IN('45','60'))CLOSEING,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT 'S'SEP,'    TO MAKING'LPARTICULAR,"
        strSql += vbCrLf + " (SELECT SUM(AMOUNT)FROM  TEMPTABLEDB..TEMPANNEX8B  WHERE RESULT IN('57.5'))MAKING,''RPARTICULAR,'0.0',1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT 'S'SEP,'    TO GROSS PROFIT'LPARTICULAR,'0.00'MAKING,''RPARTICULAR,'0.0',1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        ''DIAMOND
        strSql += vbCrLf + " SELECT 'D'SEP,'DIAMOND'LPARTICULAR,'0.0'OAMT,''RPARTICULAR,'0.0'RAMT,0 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT 'D'SEP,'    TO OPEINIG STOCK'LPARTICULAR,"
        strSql += vbCrLf + " (SELECT SUM(AMOUNT)FROM  TEMPTABLEDB..TEMPANNEX8B  WHERE RESULT IN('63','67.2'))OAMT"
        strSql += vbCrLf + " ,'     BY SALES'RPARTICULAR,"
        strSql += vbCrLf + " (SELECT SUM(AMOUNT)FROM  TEMPTABLEDB..TEMPANNEX8B  WHERE RESULT IN('66','67.5'))SALES,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT 'D'SEP,'    TO PURCHASE'LPARTICULAR,"
        strSql += vbCrLf + " (SELECT SUM(AMOUNT)FROM  TEMPTABLEDB..TEMPANNEX8B  WHERE RESULT IN('64','67.3'))PURCHASE,"
        strSql += vbCrLf + " '     BY CLOSING STOCK 'RPARTICULAR,"
        strSql += vbCrLf + " (SELECT SUM(AMOUNT)FROM  TEMPTABLEDB..TEMPANNEX8B  WHERE RESULT IN('67','67.6'))CLOSEING,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT 'D'SEP,'    TO MAKING'LPARTICULAR,'0.00'MAKING,''RPARTICULAR,'0.0',1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT 'D'SEP,'    TO GROSS PROFIT'LPARTICULAR,'0.00'MAKING,''RPARTICULAR,'0.0',1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        ''PLATINUM
        strSql += vbCrLf + " SELECT 'P'SEP,'PLATINUM JEWELLERY ACCOUNT'LPARTICULAR,'0.0'OAMT,''RPARTICULAR,'0.0'RAMT,0 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT 'P'SEP,'    TO OPEINIG STOCK'LPARTICULAR,"
        strSql += vbCrLf + " (SELECT SUM(AMOUNT)FROM  TEMPTABLEDB..TEMPANNEX8B  WHERE RESULT IN('72','79'))OAMT"
        strSql += vbCrLf + " ,'     BY SALES'RPARTICULAR,"
        strSql += vbCrLf + " (SELECT SUM(AMOUNT)FROM  TEMPTABLEDB..TEMPANNEX8B  WHERE RESULT IN('75','83'))SALES,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT 'P'SEP,'    TO PURCHASE'LPARTICULAR,"
        strSql += vbCrLf + " (SELECT SUM(AMOUNT)FROM  TEMPTABLEDB..TEMPANNEX8B  WHERE RESULT IN('73','80'))PURCHASE,"
        strSql += vbCrLf + " '     BY CLOSING STOCK 'RPARTICULAR,(SELECT SUM(AMOUNT)FROM  TEMPTABLEDB..TEMPANNEX8B  WHERE RESULT IN('76','84'))CLOSEING,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT 'P'SEP,'    TO MAKING'LPARTICULAR,"
        strSql += vbCrLf + " (SELECT SUM(AMOUNT)FROM  TEMPTABLEDB..TEMPANNEX8B  WHERE RESULT IN('72.6'))MAKING,''RPARTICULAR,'0.0',1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT 'P'SEP,'    TO GROSS PROFIT'LPARTICULAR,'0.00'MAKING,''RPARTICULAR,'0.0',1 RESULT"
        strSql += vbCrLf + " )X"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = "ALTER TABLE TEMPTABLEDB..TEMPTRADING ADD SEP1 VARCHAR(10)"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = "UPDATE TEMPTABLEDB..TEMPTRADING SET SEP1='A' WHERE SEP='G'"
        strSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMPTRADING SET SEP1='B' WHERE SEP='S'"
        strSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMPTRADING SET SEP1='C' WHERE SEP='D'"
        strSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMPTRADING SET SEP1='D' WHERE SEP='P'"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        Dim t1 As String = GetSqlValue(cn, "SELECT SUM(LAMT-RAMT) FROM TEMPTABLEDB..TEMPTRADING WHERE RESULT='1' AND SEP='G'")
        Dim t2 As String = GetSqlValue(cn, "SELECT SUM(LAMT-RAMT) FROM TEMPTABLEDB..TEMPTRADING WHERE RESULT='1' AND SEP='S'")
        Dim t3 As String = GetSqlValue(cn, "SELECT SUM(LAMT-RAMT) FROM TEMPTABLEDB..TEMPTRADING WHERE RESULT='1' AND SEP='P'")
        Dim t4 As String = GetSqlValue(cn, "SELECT SUM(LAMT-RAMT) FROM TEMPTABLEDB..TEMPTRADING WHERE RESULT='1' AND SEP='D'")

        If t1 > 0 Then
            strSql = "UPDATE TEMPTABLEDB..TEMPTRADING SET RAMT='" & t1 & "' WHERE RESULT='1' AND SEP='G' AND LPARTICULAR LIKE'%TO GROSS PROFIT%'"
        Else
            t1 = t1 * -1
            strSql = "UPDATE TEMPTABLEDB..TEMPTRADING SET LAMT='" & t1 & "' WHERE RESULT='1' AND SEP='G' AND LPARTICULAR LIKE'%TO GROSS PROFIT%'"
        End If
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        If t2 > 0 Then
            strSql = "UPDATE TEMPTABLEDB..TEMPTRADING SET RAMT='" & t2 & "' WHERE RESULT='1' AND SEP='S' AND LPARTICULAR LIKE'%TO GROSS PROFIT%'"
        Else
            t2 = t2 * -1
            strSql = "UPDATE TEMPTABLEDB..TEMPTRADING SET LAMT='" & t2 & "' WHERE RESULT='1' AND SEP='S' AND LPARTICULAR LIKE'%TO GROSS PROFIT%'"
        End If
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        If t3 > 0 Then
            strSql = "UPDATE TEMPTABLEDB..TEMPTRADING SET RAMT='" & t3 & "' WHERE RESULT='1' AND SEP='P' AND LPARTICULAR LIKE'%TO GROSS PROFIT%'"
        Else
            t3 = t3 * -1
            strSql = "UPDATE TEMPTABLEDB..TEMPTRADING SET LAMT='" & t3 & "' WHERE RESULT='1' AND SEP='P' AND LPARTICULAR LIKE'%TO GROSS PROFIT%'"
        End If
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        If t4 > 0 Then
            strSql = "UPDATE TEMPTABLEDB..TEMPTRADING SET RAMT='" & t4 & "' WHERE RESULT='1' AND SEP='D' AND LPARTICULAR LIKE'%TO GROSS PROFIT%'"
        Else
            t4 = t4 * -1
            strSql = "UPDATE TEMPTABLEDB..TEMPTRADING SET LAMT='" & t4 & "' WHERE RESULT='1' AND SEP='D' AND LPARTICULAR LIKE'%TO GROSS PROFIT%'"
        End If
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        ''TOTAL
        strSql = "INSERT INTO  TEMPTABLEDB..TEMPTRADING(SEP,SEP1,LPARTICULAR,LAMT,RPARTICULAR,RAMT,RESULT)"
        strSql += vbCrLf + "SELECT 'G'SEP,'A','',SUM(LAMT),'',SUM(RAMT),2 AS RESULT FROM  TEMPTABLEDB..TEMPTRADING "
        strSql += vbCrLf + "WHERE RESULT='1' AND SEP='G'"
        strSql += vbCrLf + "INSERT INTO  TEMPTABLEDB..TEMPTRADING(SEP,SEP1,LPARTICULAR,LAMT,RPARTICULAR,RAMT,RESULT)"
        strSql += vbCrLf + "SELECT 'S'SEP,'B','',SUM(LAMT),'',SUM(RAMT),2 AS RESULT FROM  TEMPTABLEDB..TEMPTRADING "
        strSql += vbCrLf + "WHERE RESULT='1' AND SEP='S'"
        strSql += vbCrLf + "INSERT INTO  TEMPTABLEDB..TEMPTRADING(SEP,SEP1,LPARTICULAR,LAMT,RPARTICULAR,RAMT,RESULT)"
        strSql += vbCrLf + "SELECT 'D'SEP,'C','',SUM(LAMT),'',SUM(RAMT),2 AS RESULT FROM  TEMPTABLEDB..TEMPTRADING "
        strSql += vbCrLf + "WHERE RESULT='1' AND SEP='D'"
        strSql += vbCrLf + "INSERT INTO  TEMPTABLEDB..TEMPTRADING(SEP,SEP1,LPARTICULAR,LAMT,RPARTICULAR,RAMT,RESULT)"
        strSql += vbCrLf + "SELECT 'P'SEP,'D','',SUM(LAMT),'',SUM(RAMT),2 AS RESULT FROM  TEMPTABLEDB..TEMPTRADING "
        strSql += vbCrLf + "WHERE RESULT='1' AND SEP='P'"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = " UPDATE TEMPTABLEDB..TEMPTRADING SET LAMT=(CASE WHEN LAMT='0' THEN NULL ELSE LAMT END) "
        strSql += vbCrLf + "UPDATE TEMPTABLEDB..TEMPTRADING SET RAMT=(CASE WHEN RAMT='0' THEN NULL ELSE RAMT END)"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        If cmbMetal.Text = "ALL" Then
            strSql = " SELECT * FROM TEMPTABLEDB..TEMPTRADING ORDER BY SEP1,RESULT"
        ElseIf cmbMetal.Text = "GOLD" Then
            strSql = " SELECT * FROM TEMPTABLEDB..TEMPTRADING WHERE SEP='G' ORDER BY SEP1,RESULT"
        ElseIf cmbMetal.Text = "SILVER" Then
            strSql = " SELECT * FROM TEMPTABLEDB..TEMPTRADING WHERE SEP='S' ORDER BY SEP1,RESULT"
        ElseIf cmbMetal.Text = "DIAMOND" Then
            strSql = " SELECT * FROM TEMPTABLEDB..TEMPTRADING WHERE SEP='D' ORDER BY SEP1,RESULT"
        ElseIf cmbMetal.Text = "PLATINUM" Then
            strSql = " SELECT * FROM TEMPTABLEDB..TEMPTRADING WHERE SEP='P' ORDER BY SEP1,RESULT"
        End If
        da = New OleDbDataAdapter(strSql, cn)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            gridView.DataSource = Nothing
            gridView.DataSource = dt
            gridformat()
        Else
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            gridView.DataSource = Nothing
            dtpDate.Focus()
        End If
    End Sub
    Function gridformat()
        gridView.Columns("SEP").Visible = False
        gridView.Columns("SEP1").Visible = False
        gridView.Columns("RESULT").Visible = False
        gridView.Columns("LPARTICULAR").Width = 250
        gridView.Columns("LPARTICULAR").HeaderText = "PARTICULAR"
        gridView.Columns("LAMT").Width = 150
        gridView.Columns("LAMT").HeaderText = "AMOUNT"
        gridView.Columns("RPARTICULAR").Width = 250
        gridView.Columns("RPARTICULAR").HeaderText = "PARTICULAR"
        gridView.Columns("RAMT").Width = 120
        gridView.Columns("RAMT").HeaderText = "AMOUNT"
        gridView.Columns("LAMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("RAMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        For Each gv As DataGridViewRow In gridView.Rows
            With gv
                .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Regular)
                Select Case .Cells("RESULT").Value.ToString
                    Case "0"
                        .Cells("LPARTICULAR").Style.ForeColor = reportHeadStyle.ForeColor
                        .Cells("LPARTICULAR").Style.BackColor = reportHeadStyle.BackColor
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "2"
                        .Cells("LAMT").Style.ForeColor = reportHeadStyle1.ForeColor
                        .Cells("RAMT").Style.ForeColor = reportHeadStyle1.ForeColor
                        .Cells("LAMT").Style.BackColor = reportHeadStyle1.BackColor
                        .Cells("RAMT").Style.BackColor = reportHeadStyle1.BackColor
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                End Select
                Select Case .Cells("RPARTICULAR").Value.ToString.Trim
                    Case "BY CLOSING STOCK"
                        '.Cells("RPARTICULAR").Style.ForeColor = reportHeadStyle2.ForeColor
                        .Cells("RAMT").Style.ForeColor = reportHeadStyle2.ForeColor
                        '.Cells("RPARTICULAR").Style.BackColor = Color.White
                        .Cells("RAMT").Style.BackColor = Color.White
                        '.Cells("RPARTICULAR").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                        .Cells("RAMT").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                End Select
            End With
        Next
        Dim TITLE As String
        TITLE = " TRADING ACCOUNT FOR THE YEAR ENDED " & dtpDate.Value.ToString("dd-MM-yyyy") & ""
        If cmbMetal.Text <> "ALL" Then
            TITLE = TITLE + "METAL AT " & cmbMetal.Text & ""
        End If
        lblTitle.Text = TITLE
        lblTitle.Visible = True
        FormatGridColumns(gridView, False, False, True, False)
    End Function

    Function gridformatPrint()
        gridView.Columns("SEP").Visible = False
        gridView.Columns("SEP1").Visible = False
        gridView.Columns("RESULT").Visible = False
        gridView.Columns("LPARTICULAR").Width = 250
        gridView.Columns("LPARTICULAR").HeaderText = "PARTICULAR"
        gridView.Columns("LAMT").Width = 150
        gridView.Columns("LAMT").HeaderText = "AMOUNT"
        gridView.Columns("RPARTICULAR").Width = 250
        gridView.Columns("RPARTICULAR").HeaderText = "PARTICULAR"
        gridView.Columns("RAMT").Width = 120
        gridView.Columns("RAMT").HeaderText = "AMOUNT"
        gridView.Columns("LAMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        gridView.Columns("RAMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        For Each gv As DataGridViewRow In gridView.Rows
            With gv
                .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Regular)
                Select Case .Cells("RESULT").Value.ToString
                    Case "0"
                        .Cells("LPARTICULAR").Style.ForeColor = reportHeadStyle.ForeColor
                        .Cells("LPARTICULAR").Style.BackColor = reportHeadStyle.BackColor
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "2"
                        .Cells("LAMT").Style.ForeColor = Color.White
                        .Cells("RAMT").Style.ForeColor = Color.White
                        .Cells("LAMT").Style.BackColor = Color.DarkGreen
                        .Cells("RAMT").Style.BackColor = Color.DarkGreen
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                End Select
                Select Case .Cells("RPARTICULAR").Value.ToString.Trim
                    Case "BY CLOSING STOCK"
                        '.Cells("RPARTICULAR").Style.ForeColor = Color.White
                        .Cells("RAMT").Style.ForeColor = Color.White

                        '.Cells("RPARTICULAR").Style.BackColor = Color.DarkSeaGreen
                        .Cells("RAMT").Style.BackColor = Color.DarkGreen

                        '.Cells("RPARTICULAR").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                        .Cells("RAMT").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
                End Select
            End With
        Next
        Dim TITLE As String
        TITLE = " TRADING ACCOUNT FOR THE YEAR ENDED " & dtpDate.Value.ToString("dd-MM-yyyy") & ""
        If cmbMetal.Text <> "ALL" Then
            TITLE = TITLE + "METAL AT " & cmbMetal.Text & ""
        End If
        lblTitle.Text = TITLE
        lblTitle.Visible = True
        FormatGridColumns(gridView, False, False, True, False)
    End Function

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        If cmbCompany.Items.Count > 0 Then cmbCompany.SelectedIndex = 0
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        dtpDate.Value = DateTime.Today.Date
        gridView.DataSource = Nothing
        lblTitle.Text = ""
        lblTitle.Visible = False
        cmbMetal.Text = "All"
        CnStartMonth = Format(cnTranFromDate.Month, "00")
        Prop_Gets()
        cmbCompany.Focus()
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.RowCount > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.RowCount > 0 Then
            gridformatPrint()
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
            gridformat()
        End If
    End Sub
    Private Sub viewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles viewToolStripMenuItem.Click
        btnView_Click(Me, New EventArgs)
    End Sub
    Private Sub newToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles newToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub exitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles exitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New frmTradingAccount_Properties
        obj.p_cmbCompany = cmbCompany.Text
        SetSettingsObj(obj, Me.Name, GetType(frmTradingAccount_Properties))
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New frmTradingAccount_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmTradingAccount_Properties))
        cmbCompany.Text = obj.p_cmbCompany
    End Sub
End Class

Public Class frmTradingAccount_Properties
    Private cmbCompany As String = strCompanyName
    Public Property p_cmbCompany() As String
        Get
            Return cmbCompany
        End Get
        Set(ByVal value As String)
            cmbCompany = value
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
End Class