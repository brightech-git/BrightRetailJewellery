Imports System.Data.OleDb
Public Class W_TagWiseProfitAndLoss
    Dim objGridShower As frmGridDispDia
    Dim Strsql As String
    Dim Da As OleDbDataAdapter
    Dim Cmd As OleDbCommand
    Private Function VIEW()
        Strsql = " SET NOCOUNT ON "
        Strsql += vbCrLf + "IF (SELECT COUNT(*) FROM MASTER..SYSOBJECTS WHERE XTYPE='U' AND NAME='TEMP_TAGPROFIT') > 0 DROP TABLE MASTER..TEMP_TAGPROFIT"
        Strsql += vbCrLf + "  SELECT CONVERT(VARCHAR(300),PARTICULAR,103) AS PARTICULAR,ITEMNAME,SUBITEMNAME,TAGNO,SUPPLIERNAME,PCS,GRSWT,NETWT,"
        Strsql += vbCrLf + "  PURCHASETOUCH,PURCHASEPUREWT,SALESTOUCH,SALESPUREWT,DIFFTOUCH,DIFFPUREWT,SALESMAN,CONVERT(INT,1) AS RESULT ,CONVERT(VARCHAR,NULL) AS COLHEAD   "
        Strsql += vbCrLf + "  INTO MASTER..TEMP_TAGPROFIT "
        Strsql += vbCrLf + "  FROM ("
        Strsql += vbCrLf + "  SELECT I.TRANDATE AS PARTICULAR,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID) AS ITEMNAME,  "
        Strsql += vbCrLf + " (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID=T.SUBITEMID) AS SUBITEMNAME,  "
        Strsql += vbCrLf + " I.TAGNO,"
        Strsql += vbCrLf + " (SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID=T.DESIGNERID) AS SUPPLIERNAME,  "
        Strsql += vbCrLf + " I.PCS,I.GRSWT,I.NETWT,"
        Strsql += vbCrLf + " P.PURTOUCH AS PURCHASETOUCH,"
        Strsql += vbCrLf + " CONVERT(NUMERIC(13,3),CASE WHEN I.GRSNET='N' THEN (ISNULL(I.NETWT,0)*ISNULL(P.PURTOUCH,0))/100 ELSE (ISNULL(I.GRSWT,0)*ISNULL(P.PURTOUCH,0))/100 END) AS PURCHASEPUREWT,"
        Strsql += vbCrLf + " I.TOUCH AS SALESTOUCH,I.PUREWT AS SALESPUREWT ,"
        Strsql += vbCrLf + " (ISNULL(I.TOUCH,0)-ISNULL(P.PURTOUCH,0)) AS DIFFTOUCH,  "
        Strsql += vbCrLf + " CONVERT(NUMERIC(13,3),I.PUREWT-CASE WHEN I.GRSNET='N' THEN (ISNULL(I.NETWT,0)*ISNULL(P.PURTOUCH,0))/100 ELSE (ISNULL(I.GRSWT,0)*ISNULL(P.PURTOUCH,0))/100 END) AS DIFFPUREWT,"
        Strsql += vbCrLf + " (SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID=I.EMPID) AS SALESMAN   "
        Strsql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I   "
        Strsql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMTAG  AS T ON  T.TAGNO=I.TAGNO AND T.ITEMID=I.ITEMID  "
        Strsql += vbCrLf + " INNER JOIN " & cnAdminDb & "..PURITEMTAG AS P ON P.TAGSNO=T.SNO  "
        Strsql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        If cmbSupplier.Text <> "" Then
            Strsql += vbCrLf + " AND I.TAGDESIGNER IN (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME='" & Trim(cmbSupplier.Text) & "')"
        End If
        If cmbItemName.Text <> "" Then
            Strsql += vbCrLf + " AND I.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & Trim(cmbItemName.Text) & "')"
        End If
        If cmbSubItemName.Text <> "" Then
            Strsql += vbCrLf + " AND I.SUBITEMID IN (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & Trim(cmbSubItemName.Text) & "')"
        End If

        Strsql += ") AS X "

        Strsql += vbCrLf + " INSERT INTO MASTER..TEMP_TAGPROFIT(PARTICULAR,GRSWT,PURCHASETOUCH,PURCHASEPUREWT,SALESTOUCH,SALESPUREWT,DIFFTOUCH,DIFFPUREWT,RESULT,COLHEAD)"
        Strsql += vbCrLf + " SELECT 'Grand Total',SUM(GRSWT),CONVERT(NUMERIC(13,2),SUM(PURCHASETOUCH)/COUNT(PURCHASETOUCH)),"
        Strsql += vbCrLf + " SUM(PURCHASEPUREWT),CONVERT(NUMERIC(13,2),SUM(SALESTOUCH)/COUNT(SALESTOUCH)),"
        Strsql += vbCrLf + " SUM(SALESPUREWT),CONVERT(NUMERIC(13,2),SUM(DIFFTOUCH)/COUNT(DIFFTOUCH)),"
        Strsql += vbCrLf + " SUM(DIFFPUREWT),2,'G' FROM MASTER..TEMP_TAGPROFIT "

        Strsql += vbCrLf + " SELECT * FROM MASTER..TEMP_TAGPROFIT ORDER BY RESULT "


        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1





        dtGrid.Columns.Add("SLNO", GetType(Integer))
        dtGrid.Columns("SLNO").AutoIncrement = True
        dtGrid.Columns("SLNO").AutoIncrementSeed = 1
        dtGrid.Columns("SLNO").AutoIncrementStep = 1



        Da = New OleDbDataAdapter(Strsql, cn)
        Da.Fill(dtGrid)
        dtGrid.Columns("SLNO").SetOrdinal(0)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        Return dtGrid
    End Function

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click
        Dim dtGrid As New DataTable
        dtGrid = VIEW()
        If dtGrid.Rows.Count = 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        ''AddHandler objGridShower.gridView.KeyDown, AddressOf DataGridView_KeyDown
        ''AddHandler objGridShower.gridView.KeyPress, AddressOf DataGridView_KeyPress
        Dim tit As String = "TAG NO WISE PROFIT " + vbCrLf
        tit += " DATE FROM " & dtpFrom.Text + " TO " + dtpTo.Text
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = Nothing
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = True
        objGridShower.gridViewHeader.Visible = True
        AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged
        objGridShower.lblStatus.Text = ""
        FormatGridColumns(objGridShower.gridView, False, False, , False)

        objGridShower.Show()
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        Call DataGridView_DetailViewFormatting(objGridShower.gridView)
        Call GridViewHeaderCreator(objGridShower.gridViewHeader)
        Call SetGridHeadColWid(objGridShower.gridViewHeader)
        objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("PARTICULAR")))
    End Sub
   
    Private Sub DataGridView_DetailViewFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            .Columns("ITEMNAME").HeaderText = "ITEM NAME"
            .Columns("ITEMNAME").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns("ITEMNAME").Width = 100
            .Columns("SUBITEMNAME").HeaderText = "SUBITEM NAME"
            .Columns("SUBITEMNAME").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns("SUBITEMNAME").Width = 100
            .Columns("PARTICULAR").HeaderText = "TRAN DATE"
            .Columns("PARTICULAR").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns("PARTICULAR").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("PARTICULAR").Width = 100
            .Columns("SUPPLIERNAME").HeaderText = "SUPPLIER NAME"
            .Columns("SUPPLIERNAME").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .Columns("SUPPLIERNAME").Width = 100
            .Columns("TAGNO").HeaderText = "TAGNO"
            .Columns("TAGNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TAGNO").Width = 60
            .Columns("PCS").HeaderText = "PCS"
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PCS").Width = 60
            .Columns("GRSWT").HeaderText = "GRS WT"
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").Width = 60
            .Columns("NETWT").HeaderText = "NET WT"
            .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETWT").Width = 60
            .Columns("SALESTOUCH").HeaderText = "TOUCH"
            .Columns("SALESTOUCH").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SALESTOUCH").Width = 60

            .Columns("SALESPUREWT").HeaderText = "PURE WT"
            .Columns("SALESPUREWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SALESPUREWT").Width = 60

            .Columns("PURCHASETOUCH").HeaderText = "TOUCH"
            .Columns("PURCHASETOUCH").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PURCHASETOUCH").Width = 60

            .Columns("PURCHASEPUREWT").HeaderText = "PURE WT"
            .Columns("PURCHASEPUREWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PURCHASEPUREWT").Width = 60

            .Columns("DIFFTOUCH").HeaderText = "TOUCH"
            .Columns("DIFFTOUCH").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIFFTOUCH").Width = 60


            .Columns("DIFFPUREWT").HeaderText = "PURE WT"
            .Columns("DIFFPUREWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIFFPUREWT").Width = 60

            .Columns("SALESMAN").HeaderText = "SALES MAN"
            .Columns("SALESMAN").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SALESMAN").Width = 60

         
            .Columns("COLHEAD").Width = 0
            .Columns("RESULT").Width = 0
            .Columns("COLHEAD").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("KEYNO").Visible = False
        End With
    End Sub
    Private Sub SetGridHeadColWid(ByVal gridViewHeader As DataGridView)
        Dim f As frmGridDispDia
        f = objGPack.GetParentControl(gridViewHeader)
        If Not f.gridViewHeader.Visible Then Exit Sub
        If f.gridViewHeader Is Nothing Then Exit Sub
        If Not f.gridView.ColumnCount > 0 Then Exit Sub
        If Not f.gridViewHeader.ColumnCount > 0 Then Exit Sub
        With f.gridViewHeader
            .Columns("KEYNO~PARTICULAR~ITEMNAME~SUBITEMNAME~TAGNO~SUPPLIERNAME~PCS~GRSWT~NETWT").Width = objGridShower.gridView.Columns("KEYNO").Width _
                    + objGridShower.gridView.Columns("PARTICULAR").Width + objGridShower.gridView.Columns("ITEMNAME").Width _
                            + objGridShower.gridView.Columns("SUBITEMNAME").Width + objGridShower.gridView.Columns("TAGNO").Width _
                                    + objGridShower.gridView.Columns("SUPPLIERNAME").Width + objGridShower.gridView.Columns("PCS").Width _
                                        + objGridShower.gridView.Columns("GRSWT").Width + objGridShower.gridView.Columns("NETWT").Width


            .Columns("PURCHASETOUCH~PURCHASEPUREWT").Width = objGridShower.gridView.Columns("PURCHASETOUCH").Width + objGridShower.gridView.Columns("PURCHASEPUREWT").Width
            .Columns("SALESTOUCH-SALESPUREWT").Width = objGridShower.gridView.Columns("SALESTOUCH").Width + objGridShower.gridView.Columns("SALESPUREWT").Width
            .Columns("DIFFTOUCH~DIFFPUREWT").Width = objGridShower.gridView.Columns("DIFFTOUCH").Width + objGridShower.gridView.Columns("DIFFPUREWT").Width
            .Columns("SALESMAN").Width = objGridShower.gridView.Columns("SALESMAN").Width
        End With
    End Sub
    Private Sub GridViewHeaderCreator(ByVal gridviewHead As DataGridView)
        Dim dtHead As New DataTable
        gridviewHead.Visible = True
        Strsql = "SELECT ''[KEYNO~PARTICULAR~ITEMNAME~SUBITEMNAME~TAGNO~SUPPLIERNAME~PCS~GRSWT~NETWT],"
        Strsql += " ''[PURCHASETOUCH~PURCHASEPUREWT],''[SALESTOUCH-SALESPUREWT],''[DIFFTOUCH~DIFFPUREWT],"
        Strsql += " ''[SALESMAN] "
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtHead)
        gridviewHead.DataSource = dtHead
        gridviewHead.Columns("KEYNO~PARTICULAR~ITEMNAME~SUBITEMNAME~TAGNO~SUPPLIERNAME~PCS~GRSWT~NETWT").HeaderText = ""
        gridviewHead.Columns("PURCHASETOUCH~PURCHASEPUREWT").HeaderText = "PURCHASE"
        gridviewHead.Columns("SALESTOUCH-SALESPUREWT").HeaderText = "SALES"
        gridviewHead.Columns("DIFFTOUCH~DIFFPUREWT").HeaderText = "DIFFERENCE"
        gridviewHead.Columns("SALESMAN").HeaderText = ""


    End Sub

    Private Sub Load_Supplier()
        Strsql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER  Order by DESIGNERNAME "
        Dim dt As New DataTable
        Da = New OleDbDataAdapter(Strsql, cn)
        Da.Fill(dt)
        cmbSupplier.Items.Clear()
        For Each ro As DataRow In dt.Rows
            cmbSupplier.Items.Add(ro.Item("DESIGNERNAME").ToString)
        Next
    End Sub
    Private Sub Load_Item()
        Strsql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST  Order by ITEMNAME "
        Dim dt As New DataTable
        Da = New OleDbDataAdapter(Strsql, cn)
        Da.Fill(dt)
        cmbItemName.Items.Clear()
        For Each ro As DataRow In dt.Rows
            cmbItemName.Items.Add(ro.Item("ITEMNAME").ToString)
        Next
    End Sub
    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs)
        SetGridHeadColWid(CType(sender, DataGridView))
    End Sub
    Private Sub Load_SubItem()
        Strsql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID IN "
        Strsql += " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST where ItemName='" & cmbItemName.Text & "') Order by SUBITEMNAME "
        Dim dt As New DataTable
        Da = New OleDbDataAdapter(Strsql, cn)
        Da.Fill(dt)
        cmbSubItemName.Items.Clear()
        For Each ro As DataRow In dt.Rows
            cmbSubItemName.Items.Add(ro.Item("SUBITEMNAME").ToString)
        Next
    End Sub
    Private Sub W_TagWiseProfitAndLoss_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub W_TagWiseProfitAndLoss_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call Load_Supplier()
        Call Load_Item()
        grpControls.Location = New Point((ScreenWid - grpControls.Width) / 2, ((ScreenHit - 128) - grpControls.Height) / 2)
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.Value = GetEntryDate(GetServerDate)
        dtpTo.Value = GetEntryDate(GetServerDate)
        dtpFrom.Select()
    End Sub
    Private Sub cmbSubItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubItemName.GotFocus
        Call Load_SubItem()
    End Sub

    Private Sub cmbSubItemName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSubItemName.SelectedIndexChanged

    End Sub
End Class