Imports System.Data.OleDb
Imports System.IO
Imports System.Resources
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports CrystalDecisions.ReportSource
Imports System.Data
Public Class frmCustomerTransactionDetail_New
#Region "Variable"
    Dim strsql As String = Nothing
    Dim da As OleDbDataAdapter
    Dim dt As DataTable
    Dim dt_dbmaster As DataTable
    Dim cmd As OleDbCommand
    Dim temptableName As String = "TEMPTABLEDB..TEMP" & systemId & "CUSTOMERINFO"
    Dim ds As dsCustomerTransactionDetails
    Dim obj_CrystalReport As CR_CustomerTransationDetails
    Dim cnStockDb As String
    Dim cnStockDbYear As String

#End Region

#Region "Form"
    Private Sub frmCustomerTransactionDetail_New_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Customer Name
        cmbName.Items.Clear()
        strsql = "SELECT DISTINCT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE PNAME IS NOT NULL  ORDER BY PNAME"
        objGPack.FillCombo(strsql, cmbName, False)

        'Mobile
        cmbMobile.Items.Clear()
        strsql = "SELECT DISTINCT MOBILE FROM " & cnAdminDb & "..PERSONALINFO WHERE MOBILE IS NOT NULL  ORDER BY MOBILE"
        objGPack.FillCombo(strsql, cmbMobile, False)
    End Sub

    Private Sub frmCustomerTransactionDetail_New_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

#End Region

#Region "Button"
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click, SearchToolStripMenuItem.Click


        dt_dbmaster = New DataTable
        'strsql = "DELETE FROM " & temptableName & ""
        'cmd = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        Try

            strsql = vbCrLf + "  IF OBJECT_ID('" & temptableName & "') IS NOT NULL DROP TABLE " & temptableName & ""
            cmd = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

            'Create Table Manully
            'CINFO INT IDENTITY(1,1) NOT NULL PROBLEM, SMALLDATETIME

            strsql = "CREATE TABLE  " & temptableName & " "
            strsql += vbCrLf + "(CUSTNAME VARCHAR(200), MOBILE VARCHAR(15), DATE VARCHAR(150)"
            strsql += vbCrLf + ",ITEMNAME VARCHAR(150),SUBITEMNAME VARCHAR(100), METALNAME VARCHAR(100), AMOUNT VARCHAR(100) "
            strsql += vbCrLf + ",BATCHNO VARCHAR(100)"
            strsql += vbCrLf + ",TRANTYPE VARCHAR(100), TRANNO VARCHAR(100), REMARKS VARCHAR(5000),"
            strsql += vbCrLf + "RESULT VARCHAR(1), COLHEAD VARCHAR(3))"
            cmd = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

            'CrystalReportViewer1.Refresh()

            If cmbMobile.Text = "" And cmbName.Text = "" Then
                MsgBox("Select Customer Name or Mobile", MsgBoxStyle.Information)
                cmbName.Focus()
                Exit Sub
            End If

            Dim i As Integer

            'strsql = "SELECT DBNAME FROM " & cnAdminDb & "..DBMASTER"
            strsql = " SELECT NAME FROM MASTER.DBO.SYSDATABASES WHERE '[' + NAME + ']'  LIKE ('" & cnCompanyId & "T%') OR NAME  LIKE ('" & cnCompanyId & "T%') "
            strsql += vbCrLf + "  ORDER BY NAME DESC"
            da = New OleDbDataAdapter(strsql, cn)
            da.Fill(dt_dbmaster)

            If dt_dbmaster.Rows.Count > 0 Then

                For Each row As DataRow In dt_dbmaster.Rows
                    cnStockDbYear = row.Item("NAME")
                    cnStockDb = cnStockDbYear

                    'ISSUE
                    strsql = "INSERT INTO " & temptableName & ""
                    strsql += vbCrLf + "SELECT P.PNAME AS CUSTNAME, P.MOBILE, CONVERT(VARCHAR(15),I.TRANDATE,103) AS DATE"
                    strsql += vbCrLf + ",(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)ITEMNAME"
                    strsql += vbCrLf + ",(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = I.SUBITEMID)SUBITEMNAME"
                    strsql += vbCrLf + ",(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = I.METALID)METALNAME"
                    strsql += vbCrLf + ",I.AMOUNT AS AMOUNT"
                    strsql += vbCrLf + ",I.BATCHNO"
                    strsql += vbCrLf + ",CASE WHEN I.TRANTYPE = 'SA' THEN 'SALES' END AS TRANTYPE"
                    strsql += vbCrLf + ",I.TRANNO,'' AS REMARKS"
                    strsql += vbCrLf + ",'1' AS RESULT, 'T' AS COLHEAD"
                    strsql += vbCrLf + "FROM " & cnStockDb & "..ISSUE AS I"
                    strsql += vbCrLf + "INNER JOIN " & cnAdminDb & "..CUSTOMERINFO AS C ON C.BATCHNO = I.BATCHNO"
                    strsql += vbCrLf + "LEFT JOIN " & cnAdminDb & "..PERSONALINFO AS P ON P.SNO = C.PSNO"
                    strsql += vbCrLf + "WHERE ISNULL (I.CANCEL, '') = ''"
                    If cmbMobile.Text <> "" Then strsql += vbCrLf + " AND P.MOBILE = '" & cmbMobile.Text & "'"
                    If cmbName.Text <> "" Then strsql += vbCrLf + " AND P.PNAME = '" & cmbName.Text & "'"
                    strsql += vbCrLf + " ORDER BY I.TRANDATE DESC"
                    cmd = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()


                    'RECEIPT
                    strsql = "INSERT INTO " & temptableName & ""
                    strsql += vbCrLf + "SELECT P.PNAME AS CUSTNAME, P.MOBILE, CONVERT(VARCHAR(15), R.TRANDATE,103) AS DATE"
                    strsql += vbCrLf + ",(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = R.ITEMID)ITEMNAME"
                    strsql += vbCrLf + ",(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = R.SUBITEMID)SUBITEMNAME"
                    strsql += vbCrLf + ",(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = R.METALID)METALNAME"
                    strsql += vbCrLf + ",R.AMOUNT AS AMOUNT"
                    strsql += vbCrLf + ",R.BATCHNO"
                    strsql += vbCrLf + ",CASE WHEN R.TRANTYPE = 'PU' THEN 'PURCHASE' END AS TRANTYPE"
                    strsql += vbCrLf + ",R.TRANNO, '' AS REMARKS"
                    strsql += vbCrLf + ",'1' AS RESULT, 'T' AS COLHEAD"
                    strsql += vbCrLf + "FROM " & cnStockDb & "..RECEIPT AS R"
                    strsql += vbCrLf + "INNER JOIN " & cnAdminDb & "..CUSTOMERINFO AS C ON C.BATCHNO = R.BATCHNO"
                    strsql += vbCrLf + "LEFT JOIN " & cnAdminDb & "..PERSONALINFO AS P ON P.SNO = C.PSNO"
                    strsql += vbCrLf + "WHERE ISNULL (R.CANCEL, '') = ''"
                    If cmbMobile.Text <> "" Then strsql += vbCrLf + " AND P.MOBILE = '" & cmbMobile.Text & "'"
                    If cmbName.Text <> "" Then strsql += vbCrLf + "AND P.PNAME = '" & cmbName.Text & "'"
                    strsql += vbCrLf + " ORDER BY R.TRANDATE DESC"
                    cmd = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

                Next row


                'ORDERMASTER
                strsql = "INSERT INTO " & temptableName & ""
                strsql += vbCrLf + "SELECT P.PNAME AS CUSTNAME,"
                strsql += vbCrLf + "P.MOBILE, CONVERT(VARCHAR(15), O.ORDATE,103) AS DATE"
                strsql += vbCrLf + ",(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = O.ITEMID)ITEMNAME"
                strsql += vbCrLf + ",(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = O.SUBITEMID)SUBITEMNAME"
                strsql += vbCrLf + ",MM.METALNAME"
                strsql += vbCrLf + ",O.ORVALUE AS AMOUNT"
                strsql += vbCrLf + ",O.BATCHNO"
                strsql += vbCrLf + ",CASE WHEN O.ORTYPE = 'O' THEN 'ORDER' ELSE 'REPAIR' END AS TRANTYPE"
                strsql += vbCrLf + ",SUBSTRING(O.ORNO,5,20) AS TRANNO, '' AS REMARKS"
                strsql += vbCrLf + ",'1' AS RESULT, 'T' AS COLHEAD"
                strsql += vbCrLf + "FROM " & cnAdminDb & "..ORMAST AS O"
                strsql += vbCrLf + "INNER JOIN " & cnAdminDb & "..CUSTOMERINFO AS C ON C.BATCHNO = O.BATCHNO"
                strsql += vbCrLf + "LEFT JOIN " & cnAdminDb & "..PERSONALINFO AS P ON P.SNO = C.PSNO"
                strsql += vbCrLf + "LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = O.ITEMID"
                strsql += vbCrLf + "LEFT JOIN " & cnAdminDb & "..METALMAST AS MM ON MM.METALID = IM.METALID"
                strsql += vbCrLf + "WHERE ISNULL (O.CANCEL, '') = ''"
                If cmbMobile.Text <> "" Then strsql += vbCrLf + "AND P.MOBILE = '" & cmbMobile.Text & " '"
                If cmbName.Text <> "" Then strsql += vbCrLf + "AND P.PNAME = '" & cmbName.Text & "'"
                strsql += vbCrLf + " ORDER BY O.ORDATE DESC"
                cmd = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

                'OUTSTANDING
                strsql = "INSERT INTO " & temptableName & ""
                strsql += vbCrLf + "SELECT "
                strsql += vbCrLf + "P.PNAME AS CUSTNAME"
                strsql += vbCrLf + ",P.MOBILE, CONVERT(VARCHAR(15), OT.TRANDATE,103) AS DATE"
                strsql += vbCrLf + ",'' AS ITEMNAME,'' AS SUBITEMNAME, '' AS METALNAME"
                strsql += vbCrLf + ", OT.AMOUNT AS AMOUNT"
                strsql += vbCrLf + ",OT.BATCHNO"
                strsql += vbCrLf + ",CASE WHEN OT.TRANTYPE = 'A' AND OT.PAYMODE = 'AR'  THEN 'ADVANCE' "
                strsql += vbCrLf + "WHEN OT.TRANTYPE = 'A' AND OT.PAYMODE = 'OR'  THEN 'ORDER' "
                strsql += vbCrLf + "WHEN OT.TRANTYPE = 'A' AND OT.PAYMODE = 'AA'  THEN 'ADVANCE ADJUSTED' "
                strsql += vbCrLf + "WHEN OT.TRANTYPE = 'GV' THEN 'GIFT VOCHER'"
                strsql += vbCrLf + "WHEN OT.TRANTYPE = 'D' THEN 'DUE'"
                strsql += vbCrLf + "WHEN OT.TRANTYPE = 'T' THEN  'OTHER'"
                strsql += vbCrLf + "END AS TRANTYPE"
                strsql += vbCrLf + ",OT.TRANNO,'' AS REMARKS"
                strsql += vbCrLf + ",'1' AS RESULT, 'T' AS COLHEAD "
                strsql += vbCrLf + "FROM " & cnAdminDb & "..OUTSTANDING AS OT"
                strsql += vbCrLf + "INNER JOIN " & cnAdminDb & "..CUSTOMERINFO AS C ON C.BATCHNO = OT.BATCHNO"
                strsql += vbCrLf + "LEFT JOIN " & cnAdminDb & "..PERSONALINFO AS P ON P.SNO = C.PSNO"
                strsql += vbCrLf + "WHERE ISNULL(OT.CANCEL, '') = '' "
                If cmbMobile.Text <> "" Then strsql += vbCrLf + " AND P.MOBILE = '" & cmbMobile.Text & "'"
                If cmbName.Text <> "" Then strsql += vbCrLf + "AND P.PNAME = '" & cmbName.Text & "'"
                strsql += vbCrLf + " ORDER BY OT.TRANDATE DESC"
                cmd = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

                'CUSTOMERQUERY
                strsql = "INSERT INTO " & temptableName & ""
                strsql += vbCrLf + "SELECT P.PNAME AS CUSTNAME"
                strsql += vbCrLf + ",P.MOBILE, CONVERT(VARCHAR(15),CQ.TRANDATE,103) AS DATE,'' AS ITEMNAME"
                strsql += vbCrLf + ",'' AS SUBITEMNAME, '' AS METALNAME, '' AS AMOUNT"
                strsql += vbCrLf + ",'' AS BATCHNO, 'QUERY' AS TRANTYPE, '' AS TRANNO, "
                strsql += vbCrLf + "CQ.QUERY AS REMARKS,"
                strsql += vbCrLf + "'5' AS RESULT, 'T' AS COLHEAD"
                strsql += vbCrLf + "FROM " & cnAdminDb & "..CUSTOMERQUERY AS CQ"
                strsql += vbCrLf + "INNER JOIN " & cnAdminDb & "..PERSONALINFO AS P ON P.SNO = CQ.PSNO"
                strsql += vbCrLf + "WHERE 1 = 1"
                If cmbMobile.Text <> "" Then strsql += vbCrLf + " AND P.MOBILE = '" & cmbMobile.Text & "'"
                If cmbName.Text <> "" Then strsql += vbCrLf + "AND P.PNAME = '" & cmbName.Text & "'"
                strsql += vbCrLf + " ORDER BY CQ.TRANDATE DESC"
                cmd = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()


            End If

            strsql = "SELECT * FROM " & temptableName & ""
            strsql += vbCrLf + " ORDER BY DATE"
            da = New OleDbDataAdapter(strsql, cn)
            dt = New DataTable
            ds = New dsCustomerTransactionDetails
            obj_CrystalReport = New CR_CustomerTransationDetails
            dt.Clear()
            ds.Clear()
            da.Fill(dt)
            da.Fill(ds, "CustomerTransaction")
            If dt.Rows.Count > 0 Then
                obj_CrystalReport.SetDataSource(ds)
                CrystalReportViewer1.ReportSource = obj_CrystalReport
                CrystalReportViewer1.DisplayToolbar = True
                CrystalReportViewer1.Refresh()
            Else
                'NULL
                obj_CrystalReport.SetDataSource(ds)
                CrystalReportViewer1.ReportSource = obj_CrystalReport
                CrystalReportViewer1.DisplayToolbar = True
                CrystalReportViewer1.Refresh()
                MsgBox("No record Found", MsgBoxStyle.Information)
            End If
            cmbName.Focus()
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        cmbMobile.Items.Clear()
        cmbName.Items.Clear()
        'CrystalReportViewer1.ReportSource = Nothing
        'CrystalReportViewer1.Refresh()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            If PrintDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                PrintDialog1.AllowSomePages = True
                PrintDialog1.AllowSelection = True
                PrintDialog1.AllowSomePages = True
                PrintDialog1.AllowCurrentPage = True
                obj_CrystalReport.PrintOptions.PrinterName = PrintDialog1.PrinterSettings.PrinterName
                obj_CrystalReport.PrintToPrinter(PrintDialog1.PrinterSettings.Copies, False, PrintDialog1.PrinterSettings.FromPage, PrintDialog1.PrinterSettings.ToPage)
                'obj_CrystalReport.ExportToDisk(ExportFormatType.ExcelRecord, "")
            End If
        Catch ex As Exception
        End Try
    End Sub
#End Region

#Region "ToolStrip"
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub SearchToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SearchToolStripMenuItem.Click
        'btnSearch_Click(sender, System.EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        'btnNew_Click(sender, System.EventArgs)
    End Sub
#End Region
End Class