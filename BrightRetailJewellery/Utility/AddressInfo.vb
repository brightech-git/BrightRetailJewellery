Imports System.Data.OleDb
Public Class AddressInfo
    Dim objGridShower As frmGridDispDia
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim dtCostCentre As New DataTable
    Dim dtArea As New DataTable
    Dim RELIGION As Boolean = IIf(GetAdmindbSoftValue("RELIGION", "N") = "Y", True, False)

    Private Sub PacketWiseStockView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub AddressInfo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , "ALL")

        strSql = " SELECT 'ALL' AREANAME,'ALL' AREAID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT AREANAME,CONVERT(vARCHAR,AREAID),2 RESULT FROM " & cnAdminDb & "..AREAMAST"
        strSql += " ORDER BY RESULT,AREANAME"
        dtArea = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtArea)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbArea, dtArea, "AREANAME", , "ALL")
        chkDate.Checked = False
        If RELIGION Then
            lblReligion.Visible = True
            chkCmbReligion.Visible = True
            chkCmbReligion.Items.Clear()
            chkCmbReligion.Items.Add("ALL")
            chkCmbReligion.Items.Add("Hindu")
            chkCmbReligion.Items.Add("Muslim")
            chkCmbReligion.Items.Add("Christian")
            chkCmbReligion.Items.Add("Sikh")
            chkCmbReligion.Items.Add("Others")
            'chkCmbReligion.SelectedIndex = 0
        Else
            lblReligion.Visible = False
            chkCmbReligion.Visible = False
        End If
        'GrpContainer.Location = New Point((ScreenWid - GrpContainer.Width) / 2, ((ScreenHit - 128) - GrpContainer.Height) / 2)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , "ALL")
        chkDate.Checked = False
        chkHeadInfo.Checked = True
        chkHasCustomer.Checked = True
        chkRegularCustomer.Checked = True
        chkCmbCostCentre.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If chkHeadInfo.Checked = False And chkRegularCustomer.Checked = False Then
            chkHeadInfo.Checked = True
            chkRegularCustomer.Checked = True
        End If
        If chkHeadInfo.Checked And chkHasCustomer.Checked = False And chkHasSmith.Checked = False And chkHasDealer.Checked = False Then
            chkHasCustomer.Checked = True
        End If
        Dim chkCostName As String = GetQryString(chkCmbCostCentre.Text)
        Dim AddressMode As String = ""
        Dim AcType As String = ""
        Dim DateFlag As Char = ""
        If chkHeadInfo.Checked Then
            AddressMode += "H,"
        End If
        If chkRegularCustomer.Checked Then
            AddressMode += "R,"
        End If
        If AddressMode <> "" Then
            AddressMode = Mid(AddressMode, 1, AddressMode.Length - 1)
        Else
            AddressMode = "H,R"
        End If
        If chkHasCustomer.Checked Then
            AcType += "C,"
        End If
        If chkHasSmith.Checked Then
            AcType += "G,"
        End If
        If chkHasDealer.Checked Then
            AcType = "D,"
        End If
        If AcType <> "" Then
            AcType = Mid(AcType, 1, AcType.Length - 1)
        Else
            AcType = "C,G,D"
        End If
        If chkDate.Checked Then
            DateFlag = "S"
        Else
            DateFlag = "N"
        End If
        strSql = vbCrLf + " EXEC " & cnStockDb & "..SP_RPT_ADDRESSINFO"
        strSql += vbCrLf + " @ADMINDB = '" & cnAdminDb & "'"
        strSql += vbCrLf + " ,@COSTNAME = " & chkCostName & ""
        strSql += vbCrLf + " ,@ADDRESSMODE = '" & AddressMode & "'"
        strSql += vbCrLf + " ,@ACTYPE = '" & AcType & "'"
        strSql += vbCrLf + " ,@AREA = '" & GetQryString(chkCmbArea.Text).Replace("'", "") & "'"
        strSql += vbCrLf + " ,@RELIGION = '" & GetReligion(chkCmbReligion.Text).Replace("'", "") & "'"
        strSql += vbCrLf + " ,@DATEFILTER = '" & DateFlag & "'"
        strSql += vbCrLf + " ,@FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        Dim dtGrid As New DataTable
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "ADDRESS INFO"
        Dim tit As String = "ADDRESS INFO" + vbCrLf
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        FormatGridColumns(objGridShower.gridView, False, False, , True)
        If Not RELIGION Then
            If objGridShower.gridView.Columns.Contains("RELIGION") = True Then objGridShower.gridView.Columns("RELIGION").Visible = False
        End If
        objGridShower.gridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        objGridShower.Show()
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns(0)))
    End Sub

    Public Function GetReligion(ByVal Source As String, Optional ByVal sep As String = ",") As String
        Dim ret As String = ""
        Dim sp() As String = Source.ToString.Split(sep)
        For Each s As String In sp
            ret += "'" & Mid(Trim(s), 1, 1) & "',"
        Next
        If ret <> "" Then
            ret = Mid(ret, 1, ret.Length - 1)
        End If
        Return ret
    End Function

    Private Sub chkHeadInfo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkHeadInfo.CheckedChanged
        chkHasCustomer.Checked = False
        chkHasSmith.Checked = False
        chkHasDealer.Checked = False
        chkHasCustomer.Enabled = chkHeadInfo.Checked
        chkHasSmith.Enabled = chkHeadInfo.Checked
        chkHasDealer.Enabled = chkHeadInfo.Checked
        If chkHeadInfo.Checked Then
            chkHasCustomer.Checked = True
        End If
    End Sub

    Private Sub chkDate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDate.CheckedChanged
        If chkDate.Checked Then
            dtpFrom.Enabled = True
            dtpTo.Enabled = True
            lblTo.Enabled = True
        Else
            dtpFrom.Enabled = False
            dtpTo.Enabled = False
            lblTo.Enabled = False
        End If
    End Sub
End Class