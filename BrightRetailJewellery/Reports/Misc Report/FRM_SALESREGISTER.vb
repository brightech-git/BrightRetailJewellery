Imports System.Data.OleDb
Public Class FRM_SALESREGISTER
    Dim objGridShower As frmGridDispDia
    Dim dtCompany As New DataTable
    Dim dtCostCentre As New DataTable
    Dim strSql As String = Nothing
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand

    Private Sub FRM_SALESREGISTER_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyCode = Keys.F3 Then
            btnNew_Click(Me, New EventArgs)
        ElseIf e.KeyCode = Keys.F12 Then
            btnExit_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub FRM_SALESREGISTER_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GrpContainer.Location = New Point((ScreenWid - GrpContainer.Width) / 2, ((ScreenHit - 128) - GrpContainer.Height) / 2)
        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY "
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        GiritechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        GiritechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , "ALL")
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Prop_Sets()
        strSql = vbCrLf + " IF OBJECT_ID('TEMPDB..#SA_REGISTER','U') IS NOT NULL DROP TABLE #SA_REGISTER"
        strSql += vbCrLf + " SELECT I.TRANDATE,I.TRANNO,CONVERT(VARCHAR(500),(SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnStockDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO)))PARTICULAR,SUM(I.GRSWT)GRSWT,SUM(I.AMOUNT)AMOUNT,SUM(I.TAX)VAT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL)CASH"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL)CHEQUE"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL)[Cr CARD]"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL)SCHEME"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL)DISCOUNT"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL)ADVANCE"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL)DUE"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL)OTHERS"
        strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL)[SALES RETURN]"
        strSql += vbCrLf + " ,CA.CATNAME,I.BATCHNO,CA.METALID"
        strSql += vbCrLf + " ,IDENTITY(INT,1,1)SNO"
        strSql += vbCrLf + " INTO #SA_REGISTER"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = I.CATCODE"
        strSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then
            strSql += vbCrLf + " AND I.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & "))"
        End If
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            strSql += vbCrLf + " AND I.COSTID IN"
            strSql += vbCrLf + "(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
        End If
        strSql += vbCrLf + " AND I.TRANTYPE IN ('SA','OD','RD')"
        strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
        strSql += vbCrLf + " AND I.GRSWT > 0"
        strSql += vbCrLf + " GROUP BY I.TRANDATE,I.TRANNO,CA.CATNAME,I.BATCHNO,CA.METALID"
        strSql += vbCrLf + " ORDER BY CA.CATNAME,I.TRANDATE,I.TRANNO"

        strSql += vbCrLf + " DECLARE @QRY VARCHAR(8000)"
        strSql += vbCrLf + " DECLARE @METALNAME VARCHAR(50)"
        strSql += vbCrLf + " DECLARE CUR CURSOR"
        strSql += vbCrLf + " FOR SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY DISPLAYORDER,METALNAME"
        strSql += vbCrLf + " OPEN CUR"
        strSql += vbCrLf + " WHILE 1=1"
        strSql += vbCrLf + " 	BEGIN"
        strSql += vbCrLf + " 	FETCH NEXT FROM CUR INTO @METALNAME"
        strSql += vbCrLf + " 	IF @@FETCH_STATUS = -1 BREAK"
        strSql += vbCrLf + " 	SET @QRY = ' ALTER TABLE #SA_REGISTER ADD ['+@METALNAME+' PUR] NUMERIC(15,2)'"
        strSql += vbCrLf + " 	EXEC (@QRY)"
        strSql += vbCrLf + " 	END"
        strSql += vbCrLf + " CLOSE CUR"
        strSql += vbCrLf + " DEALLOCATE CUR"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

        strSql = vbCrLf + " IF OBJECT_ID('TEMPDB..#PU_REGISTER','U') IS NOT NULL DROP TABLE #PU_REGISTER"
        strSql += vbCrLf + " SELECT I.TRANDATE,I.BATCHNO,SUM(I.AMOUNT+I.TAX)AS AMOUNT,I.TRANTYPE,ME.METALNAME "
        strSql += vbCrLf + " INTO #PU_REGISTER"
        strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS I"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = I.METALID"
        strSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then
            strSql += vbCrLf + " AND I.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & "))"
        End If
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            strSql += vbCrLf + " AND I.COSTID IN"
            strSql += vbCrLf + "(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
        End If
        strSql += vbCrLf + " AND I.TRANTYPE IN ('SR','PU')"
        strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
        strSql += vbCrLf + " GROUP BY I.TRANDATE,I.BATCHNO,I.TRANTYPE,ME.METALNAME"
        strSql += vbCrLf + " "
        strSql += vbCrLf + " IF OBJECT_ID('TEMPDB..#ACC_REGISTER','U') IS NOT NULL DROP TABLE #ACC_REGISTER"
        strSql += vbCrLf + " SELECT I.TRANDATE,I.BATCHNO"
        strSql += vbCrLf + " ,SUM(CASE WHEN PAYMODE = 'DI' AND TRANMODE = 'D' THEN AMOUNT WHEN PAYMODE = 'DI' AND TRANMODE = 'C' THEN -1*AMOUNT ELSE 0 END) AS DISCOUNT"
        strSql += vbCrLf + " ,SUM(CASE WHEN PAYMODE = 'CA' AND TRANMODE = 'D' THEN AMOUNT WHEN PAYMODE = 'CA' AND TRANMODE = 'C' THEN -1*AMOUNT ELSE 0 END) AS CASH"
        strSql += vbCrLf + " ,SUM(CASE WHEN PAYMODE = 'CC' AND TRANMODE = 'D' THEN AMOUNT WHEN PAYMODE = 'CC' AND TRANMODE = 'C' THEN -1*AMOUNT ELSE 0 END) AS [CR CARD]"
        strSql += vbCrLf + " ,SUM(CASE WHEN PAYMODE = 'CH' AND TRANMODE = 'D' THEN AMOUNT WHEN PAYMODE = 'CH' AND TRANMODE = 'C' THEN -1*AMOUNT ELSE 0 END) AS CHEQUE"
        strSql += vbCrLf + " ,SUM(CASE WHEN PAYMODE = 'SS' AND TRANMODE = 'D' THEN AMOUNT WHEN PAYMODE = 'SS' AND TRANMODE = 'C' THEN -1*AMOUNT ELSE 0 END) AS SCHEME"
        strSql += vbCrLf + " ,SUM(CASE WHEN PAYMODE = 'AA' AND TRANMODE = 'D' THEN AMOUNT WHEN PAYMODE = 'AA' AND TRANMODE = 'C' THEN -1*AMOUNT ELSE 0 END) AS ADVANCE"
        strSql += vbCrLf + " ,SUM(CASE WHEN PAYMODE = 'DU' AND TRANMODE = 'D' THEN AMOUNT WHEN PAYMODE = 'DU' AND TRANMODE = 'C' THEN -1*AMOUNT ELSE 0 END) AS DUE"
        strSql += vbCrLf + " ,SUM(CASE WHEN PAYMODE NOT IN ('SS','CH','CC','CA','DI','AA','DU') AND TRANMODE = 'D' THEN AMOUNT WHEN PAYMODE NOT IN ('SS','CH','CC','CA','DI','AA','DU') AND TRANMODE = 'C' THEN -1*AMOUNT ELSE 0 END) AS OTHERS"
        strSql += vbCrLf + " INTO #ACC_REGISTER"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN AS I"
        strSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' "
        If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then
            strSql += vbCrLf + " AND I.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & "))"
        End If
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            strSql += vbCrLf + " AND I.COSTID IN"
            strSql += vbCrLf + "(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
        End If
        strSql += vbCrLf + " AND I.PAYMODE IN ('AA','CA','CC','CH','DI','HC','RO','SS','DU')"
        strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
        strSql += vbCrLf + " GROUP BY I.TRANDATE,I.BATCHNO"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        strSql = " SELECT * FROM #SA_REGISTER"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        Dim DtPur As New DataTable
        strSql = " SELECT * FROM #PU_REGISTER"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(DtPur)
        Dim DtAcc As New DataTable
        strSql = " SELECT * FROM #ACC_REGISTER"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(DtAcc)

        Dim RowSales() As DataRow = Nothing
        Dim RowUpdate() As DataRow = Nothing
        Dim FStr As String = Nothing
        ''PURCHASE UPDATION
        RowUpdate = DtPur.Select("TRANTYPE = 'PU'")
        For Each RoUpd As DataRow In RowUpdate
            FStr = "TRANDATE = '" & RoUpd.Item("TRANDATE") & "' AND BATCHNO = '" & RoUpd.Item("BATCHNO").ToString & "'"
            RowSales = dtGrid.Select(FStr, "SNO")
            If RowSales IsNot Nothing Then
                If RowSales.Length > 0 Then
                    RowSales(0).Item("" & RoUpd.Item("METALNAME") & " PUR") = RoUpd.Item("AMOUNT")
                End If
            End If
        Next
        ''RETURN UPDATION
        RowUpdate = DtPur.Select("TRANTYPE = 'SR'")
        For Each RoUpd As DataRow In RowUpdate
            FStr = "TRANDATE = '" & RoUpd.Item("TRANDATE") & "' AND BATCHNO = '" & RoUpd.Item("BATCHNO").ToString & "'"
            RowSales = dtGrid.Select(FStr, "SNO")
            If RowSales IsNot Nothing Then
                If RowSales.Length > 0 Then
                    RowSales(0).Item("SALES RETURN") = RoUpd.Item("AMOUNT")
                End If
            End If
        Next
        ''ACC UPDATION
        For Each Ro As DataRow In dtGrid.Rows
            FStr = "TRANDATE = '" & Ro.Item("TRANDATE") & "' AND BATCHNO = '" & Ro.Item("BATCHNO").ToString & "'"
            If Ro.Item("BATCHNO").ToString = "CHRAS121199" Then
                Dim I As Integer = 0
            End If
            RowUpdate = DtAcc.Select(FStr, "TRANDATE,BATCHNO")
            If RowUpdate IsNot Nothing Then
                If RowUpdate.Length > 0 Then
                    Ro.Item("DISCOUNT") = IIf(Val(RowUpdate(0).Item("DISCOUNT").ToString) <> 0, RowUpdate(0).Item("DISCOUNT"), DBNull.Value)
                    Ro.Item("CASH") = IIf(Val(RowUpdate(0).Item("CASH").ToString) <> 0, RowUpdate(0).Item("CASH"), DBNull.Value)
                    Ro.Item("CHEQUE") = IIf(Val(RowUpdate(0).Item("CHEQUE").ToString) <> 0, RowUpdate(0).Item("CHEQUE"), DBNull.Value)
                    Ro.Item("CR CARD") = IIf(Val(RowUpdate(0).Item("CR CARD").ToString) <> 0, RowUpdate(0).Item("CR CARD"), DBNull.Value)
                    Ro.Item("SCHEME") = IIf(Val(RowUpdate(0).Item("SCHEME").ToString) <> 0, RowUpdate(0).Item("SCHEME"), DBNull.Value)
                    Ro.Item("ADVANCE") = IIf(Val(RowUpdate(0).Item("ADVANCE").ToString) <> 0, RowUpdate(0).Item("ADVANCE"), DBNull.Value)
                    Ro.Item("DUE") = IIf(Val(RowUpdate(0).Item("DUE").ToString) <> 0, RowUpdate(0).Item("DUE"), DBNull.Value)
                    Ro.Item("OTHERS") = IIf(Val(RowUpdate(0).Item("OTHERS").ToString) <> 0, RowUpdate(0).Item("OTHERS"), DBNull.Value)
                End If
            End If
        Next
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "SALES REGISTER"
        Dim tit As String = "SALES REGISTER (CATEGORY WISE)" + vbCrLf
        tit += " DATE FROM " & dtpFrom.Text & " TO " & dtpTo.Text
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        objGridShower.Show()
        Dim ObjGrouper As New GiritechPack.DataGridViewGrouper(objGridShower.gridView, dtGrid)
        ObjGrouper.pColumns_Group.Add("CATNAME")
        ObjGrouper.pColName_Particular = "PARTICULAR"
        ObjGrouper.pColName_ReplaceWithParticular = "PARTICULAR"
        ObjGrouper.pColumns_Sort = "CATNAME,TRANDATE,TRANNO"

        ObjGrouper.pColumns_Sum.Add("GRSWT")
        ObjGrouper.pColumns_Sum.Add("AMOUNT")
        ObjGrouper.pColumns_Sum.Add("VAT")
        ObjGrouper.pColumns_Sum.Add("CASH")
        ObjGrouper.pColumns_Sum.Add("CHEQUE")
        ObjGrouper.pColumns_Sum.Add("CR CARD")
        ObjGrouper.pColumns_Sum.Add("SCHEME")
        ObjGrouper.pColumns_Sum.Add("DISCOUNT")
        ObjGrouper.pColumns_Sum.Add("ADVANCE")
        ObjGrouper.pColumns_Sum.Add("OTHERS")
        ObjGrouper.pColumns_Sum.Add("DUE")
        ObjGrouper.pColumns_Sum.Add("SALES RETURN")
        For Each col As DataColumn In dtGrid.Columns
            If col.ColumnName.Contains(" PUR") Then
                ObjGrouper.pColumns_Sum.Add(col.ColumnName)
            End If
        Next

        ObjGrouper.GroupDgv()
        DataGridView_SummaryFormatting(objGridShower.gridView)
        FormatGridColumns(objGridShower.gridView, False, False, , False)
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        objGridShower.gridViewHeader.Visible = False
        'objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("PARTICULAR")))
    End Sub

    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            dgv.Columns("PARTICULAR").Visible = True

            dgv.Columns("BATCHNO").Visible = False
            dgv.Columns("METALID").Visible = False
            dgv.Columns("SNO").Visible = False
            dgv.Columns("KEYNO").Visible = False
            dgv.Columns("COLHEAD").Visible = False

            dgv.Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
        End With
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Prop_Gets()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New FRM_SALESREGISTER_Properties
        GetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany)
        GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        SetSettingsObj(obj, Me.Name, GetType(FRM_SALESREGISTER_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New FRM_SALESREGISTER_Properties
        GetSettingsObj(obj, Me.Name, GetType(FRM_SALESREGISTER_Properties))
        SetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany, strCompanyName)
        SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, "ALL")

    End Sub
End Class
Public Class FRM_SALESREGISTER_Properties

    Private chkCmbCompany As New List(Of String)
    Public Property p_chkCmbCompany() As List(Of String)
        Get
            Return chkCmbCompany
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCompany = value
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