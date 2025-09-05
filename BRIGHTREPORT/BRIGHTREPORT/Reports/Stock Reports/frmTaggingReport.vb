Imports System.Data.OleDb
Public Class frmTaggingReport
    Dim strsql As String = String.Empty
    Dim title As String = String.Empty
    Dim dtOptions As New DataTable
    Dim dtCostCentre As New DataTable
    Dim dtmetalname As New DataTable
    Dim til As String = String.Empty
#Region "Common Function"
    Private Sub FuncCostCenterLoad()
        strsql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strsql += " UNION ALL"
        strsql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strsql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId Then chkCmbCostCentre.Enabled = False
    End Sub

    Private Sub FuncMetal()
        'ChkComboBoxMetal.Items.Clear()
        'ChkComboBoxMetal.Items.Add("ALL")
        'strsql = " SELECT 'ALL' Metalname,'ALL' COSTID,1 RESULT"
        'strsql += " UNION ALL"
        'strsql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE ISNULL(ACTIVE,'') <> 'N' ORDER BY DISPLAYORDER"
        'objGPack.FillCombo(strsql, ChkComboBoxMetal, False, False)
        'dtmetalname = New DataTable
        'da = New OleDbDataAdapter(strsql, cn)
        'da.Fill(dtmetalname)
        'BrighttechPack.GlobalMethods.FillCombo(ChkComboBoxMetal, dtmetalname, "METALNAME", , )


        strsql = " SELECT 'ALL' METALNAME,'ALL' METALID,1 RESULT"
        strsql += " UNION ALL"
        strsql += " SELECT METALNAME,METALID,2 RESULT FROM " & cnAdminDb & "..METALMAST "
        strsql += " WHERE TTYPE='M'"
        strsql += " ORDER BY RESULT,METALNAME"
        dtmetalname = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtmetalname)
        BrighttechPack.GlobalMethods.FillCombo(ChkComboBoxMetal, dtmetalname, "METALNAME", , "ALL")


        '  ChkComboBoxMetal.DropDownStyle = ComboBoxStyle.DropDownList
    End Sub



    Private Sub FuncNew()
        With Me
            .lblTitle.Text = ""
            .dtpFrmDate.Value = GetServerDate()
            .dtpToDate.Value = GetServerDate()
            .DGrid.DataSource = Nothing
            .dtpFrmDate.Focus()
        End With
    End Sub
    Private Sub Heading()
        til = Me.Text & " Date : " & Me.dtpFrmDate.Value.ToString("dd-MM-yyyy")
        til += " To " & Me.dtpToDate.Value.ToString("dd-MM-yyyy")
        til += "At " & Me.chkCmbCostCentre.Text
        Me.lblTitle.Font = New System.Drawing.Font("VERDANA", 9, FontStyle.Bold)
        Me.lblTitle.Text = til
    End Sub
#End Region
    Private Sub frmTaggingReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        FuncCostCenterLoad()
        FuncMetal()
    End Sub
    Private Sub btnnew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnnew.Click
        FuncNew()
    End Sub
    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExcel.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If dGrid.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, dGrid, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If dGrid.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, dGrid, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub
    Private Sub btnexit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnexit.Click
        Me.Close()
    End Sub
    Private Sub frmTaggingReport_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
        Select Case e.KeyCode
            Case Keys.F3
                btnnew_Click(btnnew, Nothing)
                Exit Select
            Case Keys.F12
                btnexit_Click(btnexit, Nothing)
                Exit Select
            Case Keys.V
                btnview_Click(btnview, Nothing)
                Exit Select
            Case Keys.X
                btnExcel_Click(btnExcel, Nothing)
                Exit Select
            Case Keys.P
                btnPrint_Click(btnPrint, Nothing)
                Exit Select
        End Select
    End Sub
    Private Sub btnview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnview.Click
        Dim CHKCOSTID As String = GetQryStringForSp(chkCmbCostCentre.Text, cnAdminDb & "..COSTCENTRE", "COSTID", "COSTNAME", False)
        Dim CHKMETALID As String = GetQryStringForSp(ChkComboBoxMetal.Text, cnAdminDb & "..METALMAST", "METALID", "METALNAME", False)
        dGrid.DataSource = Nothing

        strsql = "select COSTID from " & cnAdminDb & "..COSTCENTRE where ACTIVE = 'Y' order by UPDATED"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dt)

        strsql = "SELECT IL.LOTDATE,ISNULL(IL.LOTNO,0) AS LOTNO ,ISNULL(IL.PCS,0) AS LOTPCS"
        strsql += ",ISNULL(IL.GRSWT,0) AS LOTWT,ISNULL(IL.CPCS,0) AS TAGPCS,ISNULL(IL.CGRSWT,0) AS TAGWT"
        For Each dr As DataRow In dt.Rows
            strsql += ",(select sum(PCS) as pcs from  " & cnAdminDb & "..itemtag where lotsno in (select sno from " & cnAdminDb & "..itemlot where lotno=il.LOTNO) and COSTID='" & dr("COSTID") & "' group by COSTID) as " & dr("COSTID") & "PCS"
            strsql += ",(select sum(GRSWT) as Grswt from " & cnAdminDb & "..itemtag where lotsno in (select sno from " & cnAdminDb & "..itemlot where lotno=il.LOTNO) and COSTID='" & dr("COSTID") & "' group by COSTID) as " & dr("COSTID") & "WT "
        Next
        'strsql += ",(select sum(PCS) as pcs from " & cnAdminDb & "..itemtag where lotsno in (select sno from " & cnAdminDb & "..itemlot where lotno=il.LOTNO) and COSTID='FT' group by COSTID) as FTPCS "
        'strsql += ",(select sum(GRSWT) as Grswt from " & cnAdminDb & "..itemtag where lotsno in (select sno from " & cnAdminDb & "..itemlot where lotno=il.LOTNO) and COSTID='FT' group by COSTID) as FTWT "
        'strsql += ",(select sum(PCS) as pcs from " & cnAdminDb & "..itemtag where lotsno in (select sno from " & cnAdminDb & "..itemlot where lotno=il.LOTNO) and COSTID='FH' group by COSTID) as FHPCS "
        'strsql += ",(select sum(GRSWT) as Grswt from " & cnAdminDb & "..itemtag where lotsno in (select sno from " & cnAdminDb & "..itemlot where lotno=il.LOTNO) and COSTID='FH' group by COSTID) as FHWT "
        strsql += ",(ISNULL(IL.PCS,0) - ISNULL(IL.CPCS,0))AS DIFFPCS"
        strsql += ",(ISNULL(IL.GRSWT,0) - ISNULL(IL.CGRSWT,0))AS DIFFWEIGHT "
        strsql += ",TRANINVNO,D.DESIGNERNAME ,CASE WHEN i.METALID='G' THEN 'GOLD' WHEN i.METALID='S' THEN 'SILVER' WHEN i.METALID='D' THEN 'DIAMOND' END AS METALNAME  "
        strsql += "FROM " & cnAdminDb & "..ITEMLOT AS IL "
        strsql += "inner join " & cnAdminDb & "..DESIGNER D ON IL.DESIGNERID = D.DESIGNERID "
        strsql += "inner join " & cnAdminDb & "..ITEMMAST I ON IL.ITEMID = I.ITEMID "
        strsql += " WHERE IL.LOTDATE BETWEEN  '" & dtpFrmDate.Value.ToString("yyyy/MM/dd") & "' "
        strsql += " AND '" & dtpToDate.Value.ToString("yyyy/MM/dd") & "'"
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strsql += " AND IL.COSTID ='" & CHKCOSTID & "'"
        If ChkComboBoxMetal.Text <> "ALL" And ChkComboBoxMetal.Text <> "" Then strsql += " AND I.METALID = '" & CHKMETALID & "'"
        Dim ds As New DataSet
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(ds)
        ' ---Total

        strsql = "SELECT SUM(ISNULL(IL.PCS,0)) AS LOTPCS"
        strsql += ",SUM(ISNULL(IL.GRSWT,0)) AS LOTWT,SUM(ISNULL(IL.CPCS,0)) AS TAGPCS,SUM(ISNULL(IL.CGRSWT,0)) AS TAGWT "
        'strsql += ",(ISNULL(IL.PCS,0) - ISNULL(IL.CPCS,0))AS DIFFPCS"
        'strsql += ",(ISNULL(IL.GRSWT,0) - ISNULL(IL.CGRSWT,0))AS DIFFWEIGHT "
        'strsql += ",TRANINVNO,D.DESIGNERNAME ,CASE WHEN i.METALID='G' THEN 'GOLD' WHEN i.METALID='S' THEN 'SILVER' WHEN i.METALID='D' THEN 'DIAMOND' END AS METALNAME  "
        strsql += "FROM " & cnAdminDb & "..ITEMLOT AS IL "
        strsql += "inner join " & cnAdminDb & "..DESIGNER D ON IL.DESIGNERID = D.DESIGNERID "
        strsql += "inner join " & cnAdminDb & "..ITEMMAST I ON IL.ITEMID = I.ITEMID "
        strsql += " WHERE IL.LOTDATE BETWEEN  '" & dtpFrmDate.Value.ToString("yyyy/MM/dd") & "' "
        strsql += " AND '" & dtpToDate.Value.ToString("yyyy/MM/dd") & "'"
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strsql += " AND IL.COSTID ='" & CHKCOSTID & "'"
        If ChkComboBoxMetal.Text <> "ALL" And ChkComboBoxMetal.Text <> "" Then strsql += " AND I.METALID = '" & CHKMETALID & "'"
        ' Dim ds As New DataSet
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(ds)

        For Each dr As DataRow In dt.Rows
            Dim totalFJPCS As Decimal = 0
            Dim totalFJWT As Decimal = 0
            Dim dtDetail As DataTable = ds.Tables(0)
            For i As Integer = 0 To ds.Tables(0).Rows.Count - 2
                If Not IsDBNull(dtDetail.Rows(i)(dr("COSTID") & "PCS")) AndAlso IsNumeric(dtDetail.Rows(i)(dr("COSTID") & "PCS")) Then
                    totalFJPCS += Convert.ToDecimal(dtDetail.Rows(i)(dr("COSTID") & "PCS"))
                End If
                If Not IsDBNull(dtDetail.Rows(i)(dr("COSTID") & "WT")) AndAlso IsNumeric(dtDetail.Rows(i)(dr("COSTID") & "WT")) Then
                    totalFJWT += Convert.ToDecimal(dtDetail.Rows(i)(dr("COSTID") & "WT"))
                End If
            Next
            dtDetail.Rows(dtDetail.Rows.Count - 1)(dr("COSTID") & "PCS") = totalFJPCS
            dtDetail.Rows(dtDetail.Rows.Count - 1)(dr("COSTID") & "WT") = totalFJWT
        Next

        If ds.Tables(0).Rows.Count > 0 Then
            dGrid.DataSource = ds.Tables(0)
            Heading()
            dGrid.Columns("LOTDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
            'dGrid.Columns("LOTPCS").Visible = False
            'dGrid.Columns("TAGPCS").Visible = False
            'dGrid.Columns("DIFFPCS").Visible = False
            'dGrid.Columns("DIFFPCS").Visible = False
            dGrid.Columns("LOTNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            dGrid.Columns("LOTPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dGrid.Columns("TAGPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            For Each dr As DataRow In dt.Rows
                dGrid.Columns(dr("COSTID") & "PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                dGrid.Columns(dr("COSTID") & "WT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            Next
            'dGrid.Columns("FTPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            'dGrid.Columns("FTWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            'dGrid.Columns("FHPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            'dGrid.Columns("FHWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight


            dGrid.Columns("DIFFPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dGrid.Columns("LOTWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dGrid.Columns("TAGWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dGrid.Columns("DIFFWEIGHT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dGrid.Columns("METALNAME").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            dGrid.Columns("LOTDATE").Width = 80
            dGrid.Columns("LOTNO").Width = 60

            dGrid.Columns("LOTPCS").Width = 60
            dGrid.Columns("TAGPCS").Width = 60
            dGrid.Columns("LOTWT").Width = 80
            dGrid.Columns("TAGWT").Width = 80
            dGrid.Columns("DIFFPCS").Width = 60
            For Each dr As DataRow In dt.Rows
                dGrid.Columns(dr("COSTID") & "PCS").Width = 60
                dGrid.Columns(dr("COSTID") & "WT").Width = 60
            Next
            'dGrid.Columns("FTPCS").Width = 60
            'dGrid.Columns("FTWT").Width = 60
            'dGrid.Columns("FHPCS").Width = 60
            'dGrid.Columns("FHWT").Width = 60

            dGrid.Columns("DIFFWEIGHT").Width = 60
            dGrid.Columns("DIFFWEIGHT").HeaderText = "DIFFWT"
            dGrid.Columns("DESIGNERNAME").Width = 150
            ' dGrid.Columns("METALNAME").Width = 150
        Else
            dGrid.DataSource = Nothing
            Me.lblTitle.Text = ""
            MsgBox("No Records Found.")
        End If
    End Sub
    Private Sub AutoResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoResizeToolStripMenuItem.Click
        AutoResizeToolStripMenuItem.Checked = True
        If dGrid.RowCount > 0 Then
            If AutoResizeToolStripMenuItem.Checked Then
                dGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                dGrid.Invalidate()
                For Each dgvCol As DataGridViewColumn In dGrid.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                dGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In dGrid.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                dGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
        End If
    End Sub
    Private Sub chkCmbCostCentre_SelectedIndexChanged(sender As Object, e As EventArgs) Handles chkCmbCostCentre.SelectedIndexChanged
    End Sub
End Class