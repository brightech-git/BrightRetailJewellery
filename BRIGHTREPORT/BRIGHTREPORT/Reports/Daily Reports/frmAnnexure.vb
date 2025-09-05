Imports System.Data.OleDb

Public Class frmAnnexure
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim dsGridView As New DataSet
    Dim ftrStr As String
    Dim dtCostCentre As New DataTable
    Dim dtacname As New DataTable
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub
    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If gridView.DataSource IsNot Nothing Then CType(gridView.DataSource, DataTable).Rows.Clear()
        Me.Refresh()
        Dim mtemptable As String = "TEMP" & Trim(Guid.NewGuid.ToString.Substring(0, 5))
        Dim chkCostId As String = GetQryStringForSp(chkCmbCostCentre.Text, cnAdminDb & "..COSTCENTRE", "COSTID", "COSTNAME", True)
        Dim chkaccode As String = GetQryStringForSp(chkcmbacname_OWN.Text, cnAdminDb & "..ACHEAD", "ACCODE", "ACNAME", True)

        strSql = vbCrLf + " SELECT AA.TIN TINNO,''CIB,IDENTITY(INT,1,1)SLNO,ACNAME,'NG'GOVT,"
        strSql += vbCrLf + " AA.PHONENO PHONE,AA.PAN PANNO,DOBIRTH,''FATHERSNAME,DOORNO"
        strSql += vbCrLf + " ,AA.ADDRESS1,AA.ADDRESS2,AA.ADDRESS3,CITY,STATE"
        strSql += vbCrLf + " ,AA.PINCODE,MODEPAY,AMOUNT,TRANDATE,TRANNO,REFNO,TRANDATE1"
        strSql += vbCrLf + " INTO TEMPTABLEDB.." & mtemptable & " FROM ("
        strSql += vbCrLf + " SELECT A.ACCODE,"
        strSql += vbCrLf + " CASE WHEN PAYMODE='CA' THEN 'C' "
        strSql += vbCrLf + " WHEN PAYMODE='CC' THEN 'R' "
        strSql += vbCrLf + " WHEN PAYMODE='CH' THEN 'Q' "
        strSql += vbCrLf + " WHEN PAYMODE='CP' THEN 'Q' "
        strSql += vbCrLf + " WHEN PAYMODE='TR' THEN 'Q' "
        strSql += vbCrLf + " WHEN PAYMODE='JE' THEN 'Q' "
        strSql += vbCrLf + " WHEN PAYMODE='MR' THEN 'Q' "
        strSql += vbCrLf + " WHEN PAYMODE='CR' THEN 'Q' "
        strSql += vbCrLf + " WHEN PAYMODE='DR' THEN 'Q' "
        strSql += vbCrLf + " ELSE 'O' END MODEPAY,A.COMPANYID"
        strSql += vbCrLf + " ,SUM(AMOUNT)AMOUNT,CONVERT(VARCHAR,TRANDATE,105)TRANDATE,TRANDATE AS TRANDATE1, TRANNO,REFNO"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN A"
        If chkAsonDate.Checked Then
            strSql += vbCrLf + " WHERE TRANDATE <= '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' "
        Else
            strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "'"
        End If
        strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
        If chkaccode <> "" And chkaccode <> "ALL" Then
            strSql += vbCrLf + " AND A.ACCODE IN (" & chkaccode & ") "
        Else
            strSql += vbCrLf + " AND A.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ISNULL(ACTIVE,'')<>'N' AND ACTYPE='D') "
        End If
        If chkCostId <> "" And chkCostId <> "ALL" Then
            strSql += vbCrLf + " AND A.COSTID IN (" & chkCostId & ") "
        End If
        strSql += vbCrLf + " AND TRANMODE='C'"
        strSql += vbCrLf + " GROUP BY PAYMODE,TRANDATE,TRANNO,REFNO,A.ACCODE,A.COMPANYID ) X "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..COMPANY C ON C.COMPANYID=X.COMPANYID "
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ACHEAD AA ON AA.ACCODE=X.ACCODE    "
        If Val(txtFrmAmt_AMT.Text) <> 0 And Val(txtToAmt_AMT.Text) <> 0 Then
            strSql += vbCrLf + " WHERE AMOUNT BETWEEN " & Val(txtFrmAmt_AMT.Text) & " AND " & Val(txtToAmt_AMT.Text)
        End If
        strSql += vbCrLf + " ORDER BY TRANDATE1"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = "SELECT * FROM TEMPTABLEDB.." & mtemptable & " ORDER BY SLNO,TRANDATE1,TRANNO"
        Dim dtGrid As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        dsGridView = New DataSet
        da.Fill(dsGridView)
        dtGrid = dsGridView.Tables(0)
        If dtGrid.Rows.Count > 0 Then
            With gridView
                .DataSource = Nothing
                .DataSource = dtGrid
                .Columns("TINNO").HeaderText = "TAN"
                .Columns("CIB").HeaderText = "CODE"
                .Columns("SLNO").HeaderText = "SL NO"
                .Columns("ACNAME").HeaderText = "NAME"
                .Columns("GOVT").HeaderText = "GOVT/NONGOVT"
                .Columns("PHONE").HeaderText = "CON NO"
                .Columns("PANNO").HeaderText = "PAN"
                .Columns("DOBIRTH").HeaderText = "DOB"
                .Columns("FATHERSNAME").HeaderText = "F-NAME"
                .Columns("ADDRESS1").HeaderText = "ADD1"
                .Columns("ADDRESS2").HeaderText = "ADD2"
                .Columns("ADDRESS3").HeaderText = "CITY"
                .Columns("CITY").HeaderText = "DISTRICT"
                .Columns("STATE").HeaderText = "STATE"
                .Columns("PINCODE").HeaderText = "PIN"
                .Columns("MODEPAY").HeaderText = "PMODE"
                .Columns("AMOUNT").HeaderText = "TXN_AMOUNT"
                .Columns("TRANDATE").HeaderText = "TXN_DATE"
                .Columns("TRANNO").HeaderText = "BILL NO"
                .Columns("REFNO").HeaderText = "TXN-REF"
                .Columns("TRANDATE1").Visible = False
                .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
                .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("TRANNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            lblTitle.Text = "ANNEXURE (PART B)"
            If dtpTo.Enabled = False Then
                lblTitle.Text += " AS ON " & dtpFrom.Text & ""
            Else
                lblTitle.Text += " -DATE FROM " & dtpFrom.Text & " TO " & dtpTo.Text & ""
            End If
            lblTitle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            If chkCmbCostCentre.Visible Then lblTitle.Text = lblTitle.Text & " COST CENTRE : " & chkCmbCostCentre.Text
            gridView.Focus()

        Else
            gridView.DataSource = Nothing
            lblTitle.Text = ""
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        Prop_Sets()
        'For i As Integer = 0 To gridView.Rows.Count - 1
        '    If gridView.Rows(i).Cells("COLHEAD").Value = "G" Then
        '        gridView.Rows(i).DefaultCellStyle.BackColor = Color.LightYellow
        '        gridView.Rows(i).DefaultCellStyle.ForeColor = Color.Black
        '        gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        '    ElseIf gridView.Rows(i).Cells("COLHEAD").Value = "S" Or gridView.Rows(i).Cells("COLHEAD").Value = "S1" Then
        '        gridView.Rows(i).DefaultCellStyle.BackColor = Color.White
        '        gridView.Rows(i).DefaultCellStyle.ForeColor = Color.Blue
        '        gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        '    ElseIf gridView.Rows(i).Cells("COLHEAD").Value = "T" Then
        '        gridView.Rows(i).Cells("PARTICULAR").Style.BackColor = Color.LightGreen
        '        gridView.Rows(i).Cells("PARTICULAR").Style.ForeColor = Color.Red
        '        gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        '    ElseIf gridView.Rows(i).Cells("COLHEAD").Value = "C" Then
        '        gridView.Rows(i).Cells("PARTICULAR").Style.BackColor = Color.DarkGreen
        '        gridView.Rows(i).Cells("PARTICULAR").Style.ForeColor = Color.White
        '        gridView.Rows(i).DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        '    End If
        'Next

    End Sub

    Private Sub frmTrailBal_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub


    Private Sub frmTrailBal_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        pnlGridHeading.Visible = False
        chkAsonDate.Checked = False
        ''CostCentre
        strSql = " Select 1 from " & cnAdminDb & "..SoftControl where ctlId = 'CostCentre' and ctlText = 'Y'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            chkCmbCostCentre.Visible = True
        Else
            chkCmbCostCentre.Visible = False
        End If

        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        strSql = " SELECT 'ALL' ACNAME,'ALL' ACCODE,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ACNAME,CONVERT(vARCHAR,ACCODE),2 RESULT FROM " & cnAdminDb & "..ACHEAD WHERE ACTIVE='Y' AND ACTYPE='D'"
        strSql += " ORDER BY RESULT,ACNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtacname)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbacname_OWN, dtacname, "ACNAME", , "ALL")
        btnNew_Click(Me, New EventArgs)
        chkAsonDate.Select()
    End Sub

    Private Sub chkAsonDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAsonDate.CheckedChanged
        If chkAsonDate.Checked = True Then
            chkAsonDate.Text = "&As OnDate"
            dtpTo.Enabled = False
        Else
            chkAsonDate.Text = "&Date From"
            dtpTo.Enabled = True
        End If
    End Sub
    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, Nothing)
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        gridView.DataSource = Nothing
        btnView_Search.Enabled = True
        Prop_Gets()
        chkAsonDate.Select()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.X) Or e.KeyChar = "x" Then
            Me.btnExcel_Click(Me, New EventArgs)
            '=====================================================
            '170908 modified
        ElseIf e.KeyChar = Chr(Keys.P) Or e.KeyChar = "P" Then
            Me.btnPrint_Click(Me, New EventArgs)
            '=======================================================
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, Nothing)
        End If
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub

    Private Sub ResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResizeToolStripMenuItem.Click
        If gridView.RowCount > 0 Then
            If ResizeToolStripMenuItem.Checked Then
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
            'gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End If
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmAnnexure_Properties
        obj.p_chkAsonDate = chkAsonDate.Checked
        GetChecked_CheckedList(chkcmbacname_OWN, obj.p_chkCmbacname)
        SetSettingsObj(obj, Me.Name, GetType(frmAnnexure_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmAnnexure_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmAnnexure_Properties))
        chkAsonDate.Checked = obj.p_chkAsonDate
        SetChecked_CheckedList(chkcmbacname_OWN, obj.p_chkCmbacname, "ALL")
    End Sub
End Class

Public Class frmAnnexure_Properties
    Private chkAsonDate As Boolean = True
    Public Property p_chkAsonDate() As Boolean
        Get
            Return chkAsonDate
        End Get
        Set(ByVal value As Boolean)
            chkAsonDate = value
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
    Private chkCmbacname As New List(Of String)
    Public Property p_chkCmbacname() As List(Of String)
        Get
            Return chkCmbacname
        End Get
        Set(ByVal value As List(Of String))
            chkCmbacname = value
        End Set
    End Property
    Private chktotal As Boolean = False
    Public Property p_chktotal() As Boolean
        Get
            Return chktotal
        End Get
        Set(ByVal value As Boolean)
            chktotal = False
        End Set
    End Property
End Class