Imports System.Data.OleDb

Public Class DailyActivityReport
    Dim objGridShower As frmGridDispDia
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim dtcostCentre As DataTable
    Dim dtcompany As DataTable

    Private Sub DailyActivityReport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.F12 Then
            FClose()
        ElseIf e.KeyCode = Keys.F3 Then
            FNew()
        ElseIf e.KeyCode = Keys.F2 Then
            FView()
        End If

    End Sub

    Private Sub DailyActivityReport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub DailyActivityReport_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        FNew()
    End Sub

    Private Sub btnView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnView.Click
        FView()
    End Sub
    Public Function GetSelectedComId(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
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

    Private Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        FClose()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        FNew()
    End Sub
    Function FClose() As Integer
        Me.Close()
    End Function
    Function FNew() As Integer

        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()

        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY where ISNULL(ACTIVE,'')<>'N'"
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtcompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtcompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtcompany, "COMPANYNAME", , strCompanyName)

        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtcostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtcostCentre)
        BrighttechPack.GlobalMethods.FillCombo(ChkCmbCostCenter, dtcostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then ChkCmbCostCenter.Enabled = False
    End Function
    Function FView() As Integer
        Dim TABLENAME As String = "TEMPDAILYREPORT"

        strSql = vbCrLf + "EXEC " & cnAdminDb & "..SP_RPT_SMITHDAYRPT"
        strSql += vbCrLf + "@DBNAME = '" & cnStockDb & "'"
        strSql += vbCrLf + ",@TEMPTAB = '" & TABLENAME & "'"
        strSql += vbCrLf + ",@FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + ",@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"

        If chkCmbCompany.Text = "ALL" Then
            strSql += vbCrLf + ",@COMPANYID = 'ALL'"
        Else
            strSql += vbCrLf + ",@COMPANYID = '" & GetSelectedComId(chkCmbCompany, False) & "'"
        End If

        If ChkCmbCostCenter.Text = "ALL" Then
            strSql += vbCrLf + ",@COSTID = 'ALL'"
        Else
            strSql += vbCrLf + ",@COSTID = '" & GetSelectedCostId(ChkCmbCostCenter, False) & "'"
        End If

        Dim dss As New DataSet

        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dss)
        Dim dtGrid As New DataTable
        'dtGrid.Columns.Add("KEYNO", GetType(String))
        'dtGrid.Columns("KEYNO").AutoIncrement = True
        'dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        'dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        If dss.Tables.Contains("TABLE") Then
            dtGrid = dss.Tables(0)
        Else
            dtGrid = dss.Tables(1)
        End If

        Dim dtGrid1 As New DataTable
        'dtGrid1.Columns.Add("KEYNO", GetType(String))
        'dtGrid1.Columns("KEYNO").AutoIncrement = True
        'dtGrid1.Columns("KEYNO").AutoIncrementSeed = 0
        'dtGrid1.Columns("KEYNO").AutoIncrementStep = 1
        dtGrid1 = dtGrid.Copy
        'dtGrid1.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        If Not dtGrid1.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Function
        End If

        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "DAILY REPORT"
        Dim tit As String = "DAILY ACTIVITY REPORT"
        tit = tit + IIf(ChkCmbCostCenter.Text <> "" And ChkCmbCostCenter.Text <> "ALL", " :" & ChkCmbCostCenter.Text, "")
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid1)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.gridView.Columns("ACNAME").Visible = False
        objGridShower.gridView.Columns("PARTICULAR").Width = 200
        objGridShower.gridView.Columns("COLHEAD").Visible = False
        objGridShower.gridView.Columns("ORDNO").Visible = False
        objGridShower.gridView.Columns("KEYNO").Visible = False
        objGridShower.gridView.Columns("CREDIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        objGridShower.gridView.Columns("DEBIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        objGridShower.gridView.Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        objGridShower.gridView.Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = False
        objGridShower.formuser = userId
        objGridShower.Show()
        objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("PARTICULAR")))
        For Each dgvRow As DataGridViewRow In objGridShower.gridView.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "T"
                        .Cells("PARTICULAR").Style = reportHeadStyle
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "C"
                        .Cells("PARTICULAR").Style.BackColor = Color.LightGreen
                        .Cells("PARTICULAR").Style.ForeColor = Color.Red
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "I"
                        .Cells("PARTICULAR").Style.ForeColor = Color.Red
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    Case "S"
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                        .DefaultCellStyle.ForeColor = Color.Blue
                    Case "G"
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                        .DefaultCellStyle.BackColor = Color.LightYellow
                        .DefaultCellStyle.ForeColor = Color.Black
                End Select
            End With
        Next


        strSql = " IF  EXISTS (SELECT * FROM TEMPTABLEDB..SYSOBJECTS WHERE XTYPE = 'U' AND NAME = '" & TABLENAME & "' ) DROP TABLE TEMPTABLEDB.." & TABLENAME & ""
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

    End Function
End Class