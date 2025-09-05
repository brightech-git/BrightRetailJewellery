Imports System.Data.OleDb
Public Class frmEmployeeWiseValueAdded
    Dim strsql As String = ""
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim dt As New DataTable
    Dim dtCompany As New DataTable
    Dim dtCostCentre As New DataTable
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.Value = GetServerDate()
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        GridView.DataSource = Nothing
    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        Dim chkCompanyId As String = GetQryStringForSp(chkCmbCompany.Text, cnAdminDb & "..COMPANY", "COMPANYID", "COMPANYNAME")
        Dim chkCostId As String = GetQryStringForSp(chkCmbCostCentre.Text, cnAdminDb & "..COSTCENTRE", "COSTID", "COSTNAME")

        If chkCmbCostCentre.Text = "ALL" Then
            strsql = "SELECT TRANNO,CONVERT(VARCHAR,ITEMID) + '-' + TAGNO AS TAGNO, "
            strsql += "(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID) AS ITEMNAME, "
            strsql += "(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = I.SUBITEMID) AS SUBITEMNAME, "
            strsql += "PCS,GRSWT, NETWT, AMOUNT,  "
            strsql += "CONVERT(NUMERIC(10,2),(AMOUNT-(GRSWT*RATE))) AS VALUEADD,  "
            strsql += "CONVERT(NUMERIC(10,2),RATE) AS RATE, "
            strsql += " CONVERT(NUMERIC(10,2),(((AMOUNT-(GRSWT*RATE)))/(GRSWT*RATE))*100)  AS VAPER, "
            strsql += "CONVERT(VARCHAR(40),(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = I.EMPID)) AS EMPNAME "
            strsql += "FROM " & cnStockDb & "..ISSUE I WHERE TRANTYPE IN ('SA','OD') AND ISNULL(CANCEL,'') = '' AND  "
            strsql += "TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strsql += "AND COMPANYID = '" & chkCompanyId & "' ORDER BY EMPNAME,TRANNO  "
            cmd = New OleDbCommand(strsql, cn)
            dt = New DataTable()
            da = New OleDbDataAdapter(cmd)
            da.Fill(dt)
            GridView.DataSource = dt
        Else
            strsql = "SELECT TRANNO,CONVERT(VARCHAR,ITEMID) + '-' + TAGNO AS TAGNO, "
            strsql += "(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID) AS ITEMNAME, "
            strsql += "(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = I.SUBITEMID) AS SUBITEMNAME, "
            strsql += "PCS,GRSWT, NETWT, AMOUNT,  "
            strsql += "CONVERT(NUMERIC(10,2),(AMOUNT-(GRSWT*RATE))) AS VALUEADD,  "
            strsql += "CONVERT(NUMERIC(10,2),RATE) AS RATE, "
            strsql += " CONVERT(NUMERIC(10,2),(((AMOUNT-(GRSWT*RATE)))/(GRSWT*RATE))*100)  AS VAPER, "
            'strsql += "(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = I.EMPID) AS EMPNAME "
            strsql += "CONVERT(VARCHAR(40),(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = I.EMPID)) AS EMPNAME "
            strsql += "FROM " & cnStockDb & "..ISSUE I WHERE TRANTYPE IN ('SA','OD') AND ISNULL(CANCEL,'') = '' AND  "
            strsql += "TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strsql += "AND COSTID ='" & chkCostId & "' AND COMPANYID = '" & chkCompanyId & "' ORDER BY EMPNAME,TRANNO  "
            cmd = New OleDbCommand(strsql, cn)
            dt = New DataTable()
            da = New OleDbDataAdapter(cmd)
            da.Fill(dt)
            GridView.DataSource = dt
        End If

    End Sub

    Private Sub frmEmployeeWiseValueAdded_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'GrpContainer.Location = New Point((ScreenWid - GrpContainer.Width) / 2, ((ScreenHit - 128) - GrpContainer.Height) / 2)
        strsql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strsql += " UNION ALL"
        strsql += " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        strsql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)
        strsql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strsql += " UNION ALL"
        strsql += " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strsql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub frmEmployeeWiseValueAdded_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If GridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, "", "", GridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If GridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, "", "", GridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub
End Class