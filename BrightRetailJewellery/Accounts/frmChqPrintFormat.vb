Imports System.Data.OleDb
Imports System.Drawing.Text
Imports System.Drawing

Public Class frmChqPrintFormat
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim tran As OleDbTransaction
    Dim dr As OleDbDataReader
    Dim dt As New DataTable
    Dim grddt As New DataTable
    Dim upflag As Boolean = False
    Dim snonum As String = ""
    Dim intfuns As New InstalledFontCollection
    Dim fontfamilies() As FontFamily = intfuns.Families()

    Private Sub frmChqPrintFormat_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcNew()
    End Sub
    Function funcNew() As Integer
        objGPack.TextClear(Me)
        cmbformattype.Items.Clear()
        strSql = "SELECT count(*) FROM " & cnStockDb & "..CHEQUEBOOK"
        If GetSqlValue(cn, strSql) > 0 Then
            strSql = "SELECT DISTINCT CHQFORMAT FROM " & cnStockDb & "..CHEQUEBOOK"
        Else
            strSql = " SELECT 1 as CHQFORMAT"
        End If
        da = New OleDbDataAdapter(strSql, cn)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            For i As Integer = 0 To dt.Rows.Count - 1
                cmbformattype.Items.Add(dt.Rows(i).Item("CHQFORMAT").ToString())
            Next
        End If

        strSql = "SELECT 'LEFT' NAME,0 VALUE UNION ALL SELECT 'RIGHT' NAME,1 VALUE UNION ALL SELECT 'MIDDLE' NAME,2 VALUE "
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtt As New DataTable
        dtt = New DataTable
        da.Fill(dtt)
        If dtt.Rows.Count > 0 Then
            cmballig_OWN.DataSource = Nothing
            cmballig_OWN.DataSource = dtt
            cmballig_OWN.DisplayMember = "NAME"
            cmballig_OWN.ValueMember = "VALUE"
            cmballig_OWN.SelectedIndex = 0
        End If
        cmbformattype.Focus()
        cmbformattype.SelectAll()
        cmbformattype.SelectedText = dt.Rows(0).Item("CHQFORMAT").ToString()
        chkisbold.Checked = False
        chkdouble.Checked = False
        chkactive.Checked = False
        chkIsCentre.Checked = False
        chkIsCondenses.Checked = False
        chkisItalic.Checked = False
        chkIsMedium.Checked = False
        chkIsUnderline.Checked = False
        chklblprint.Checked = False
        For Each FontFamily As FontFamily In fontfamilies
            cmbfont.Items.Add(FontFamily.Name)
        Next
        snonum = ""
        upflag = False
        btnsave1.Text = "Save[F1]"
        GetDetails()
    End Function
    Private Sub frmChqPrintFormat_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub FuncSave()

        If upflag = False Then
            strSql = " INSERT INTO " & cnAdminDb & "..CHQPRINT_FORMAT"
            strSql += " ("
            strSql += " SNO,COMPANYID,COSTID,FORMATTYPE,AREA,COLNAME"
            strSql += " ,PRINTROW,PRINTCOL,COLWIDTH,LABELDESC,ISLABELPRINT,ISBOLD,ISDOUBLE,ISCONDENSE"
            strSql += vbCrLf + ",ISUNDERLINE,USERID,LUPDATE,ACTIVE,ISMEDIUM,ISCENTRE,FONTNAME,CUSTOMFORMAT"
            strSql += vbCrLf + ",ISITALIC,ALIGNMENT"
            strSql += " )"
            strSql += " VALUES"
            strSql += " ("
            strSql += " '" & GetNewSno(TranSnoType.CHQPRINT_FORMAT, tran) & "'" ''SNO
            strSql += " ,'" & cnCompanyId & "'" 'COMPANYID
            strSql += " ,'" & cnCostId & "'" 'COSTID
            strSql += " ," & Val(cmbformattype.Text.ToString()) & "" 'FORMATTYPE
            strSql += " ,''" 'AREA
            strSql += " ,'" & txtcolname.Text.ToString() & "'" 'COLNAME
            strSql += " ," & Val(txtPrintRow_DEC.Text.ToString()) & "" 'PRINTROW"
            strSql += " ," & Val(txtPrintcol_NUM.Text.ToString()) & "" 'PRINTCOL
            strSql += " ," & Val(txtcolwidth_NUM.Text.ToString()) & "" 'COLWIDTH
            strSql += " ,'" & txtlbldesc.Text.ToString() & "'" 'LABELDESC
            strSql += " ," & IIf(chklblprint.Checked, 1, 0) & "" 'ISLBLPRINT
            strSql += " ," & IIf(chkisbold.Checked, 1, 0) & "" 'ISBOLD
            strSql += " ," & IIf(chkdouble.Checked, 1, 0) & "" 'ISDOUBLE
            strSql += " ," & IIf(chkIsCondenses.Checked, 1, 0) & "" 'ISCONDENSE
            strSql += " ," & IIf(chkIsUnderline.Checked, 1, 0) & "" 'ISUNDERLINE
            strSql += " ,'" & userId & "'" 'USERID
            strSql += " ,'" & Today.Date & "'" 'LUPDATED
            strSql += " ," & IIf(chkactive.Checked, 1, 0) & "" 'ACTIVE
            strSql += " ," & IIf(chkIsMedium.Checked, 1, 0) & "" 'ISMEDIUM
            strSql += " ," & IIf(chkIsCentre.Checked, 1, 0) & "" 'ISCENTER
            strSql += " ,'" & cmbfont.Text.ToString() & "'" 'FONTNAME
            strSql += " ,''" 'CUSTOMFORMAT
            strSql += " ," & IIf(chkisItalic.Checked, 1, 0) & "" 'ISITALIC
            strSql += " ," & Val(cmballig_OWN.SelectedValue.ToString()) & "" 'ISITALIC
            strSql += " )"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()
        End If
    End Sub
    Private Sub GetDetails()
        strSql = "SELECT * FROM " & cnAdminDb & "..CHQPRINT_FORMAT WHERE FORMATTYPE='" & cmbformattype.Text.ToString() & "' ORDER BY PRINTROW "
        da = New OleDbDataAdapter(strSql, cn)

        grddt = New DataTable
        da.Fill(grddt)
        If dt.Rows.Count > 0 Then
            GridView.DataSource = Nothing
            GridView.DataSource = grddt
            For i As Integer = 0 To GridView.ColumnCount - 1
                With GridView
                    .Columns(i).Visible = False
                End With
            Next
            With GridView
                .Columns("FORMATTYPE").Visible = True
                .Columns("COLNAME").Visible = True
                .Columns("PRINTROW").Visible = True
                .Columns("PRINTCOL").Visible = True
                .Columns("COLWIDTH").Visible = True
                .Columns("LABELDESC").Visible = True
                .Columns("ISLABELPRINT").Visible = True
                .Columns("ISBOLD").Visible = True
                .Columns("ISDOUBLE").Visible = True
                .Columns("ISCONDENSE").Visible = True
                .Columns("ISUNDERLINE").Visible = True
                .Columns("ACTIVE").Visible = True
                .Columns("ISMEDIUM").Visible = True
                .Columns("ISCENTRE").Visible = True
                .Columns("FONTNAME").Visible = True
                .Columns("ALIGNMENT").Visible = True
                .Columns("PRINTROW").Width = 60
                .Columns("PRINTCOL").Width = 60
                .Columns("COLWIDTH").Width = 60
                .Columns("ISLABELPRINT").Width = 70
                .Columns("ISBOLD").Width = 50
                .Columns("ISDOUBLE").Width = 50
                .Columns("ISCONDENSE").Width = 50
                .Columns("ISUNDERLINE").Width = 60
                .Columns("ACTIVE").width = 40
                .Columns("ISMEDIUM").Width = 40
                .Columns("ISCENTRE").Width = 40
                .Columns("FONTNAME").Width = 130
                .Columns("ALIGNMENT").Width = 40

                .Columns("FORMATTYPE").HeaderText = "FormatType"
                .Columns("COLNAME").HeaderText = "ColName"
                .Columns("LABELDESC").HeaderText = "LblDescp"
                .Columns("FONTNAME").HeaderText = "FontName"
                .Columns("PRINTROW").HeaderText = "PrntRow"
                .Columns("PRINTCOL").HeaderText = "PrntCol"
                .Columns("COLWIDTH").HeaderText = "ColWidth"
                .Columns("ISLABELPRINT").HeaderText = "lblprnt"
                .Columns("ISBOLD").HeaderText = "Bold"
                .Columns("ISDOUBLE").HeaderText = "Double"
                .Columns("ISCONDENSE").HeaderText = "Condense"
                .Columns("ISUNDERLINE").HeaderText = "Underline"
                .Columns("ACTIVE").HeaderText = "Active"
                .Columns("ISMEDIUM").HeaderText = "Medium"
                .Columns("ISCENTRE").HeaderText = "Center"
                .Columns("FONTNAME").HeaderText = "FontName"
                .Columns("ALIGNMENT").HeaderText = "Align"

                .Columns("PRINTROW").DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight
                .Columns("PRINTCOL").DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight
                .Columns("COLWIDTH").DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight
                .Columns("ALIGNMENT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight
            End With
        End If
    End Sub
    Private Sub FncUpdate(ByVal sno As String)
        If upflag = True Then
            strSql = "UPDATE " & cnAdminDb & "..CHQPRINT_FORMAT SET "
            strSql += vbCrLf + " COLNAME ='" & txtcolname.Text.ToString() & "'" 'COLNAME
            strSql += vbCrLf + " ,PRINTROW= " & Val(txtPrintRow_DEC.Text.ToString()) & "" 'PRINTROW"
            strSql += vbCrLf + " ,PRINTCOL= " & Val(txtPrintcol_NUM.Text.ToString()) & "" 'PRINTCOL
            strSql += vbCrLf + " ,COLWIDTH= " & Val(txtcolwidth_NUM.Text.ToString()) & "" 'COLWIDTH
            strSql += vbCrLf + " ,LABELDESC= '" & txtlbldesc.Text.ToString() & "'" 'LABELDESC
            strSql += vbCrLf + " ,ISLABELPRINT= " & IIf(chklblprint.Checked, 1, 0) & "" 'ISLBLPRINT
            strSql += vbCrLf + " ,ISBOLD= " & IIf(chkisbold.Checked, 1, 0) & "" 'ISBOLD
            strSql += vbCrLf + " ,ISDOUBLE= " & IIf(chkdouble.Checked, 1, 0) & "" 'ISDOUBLE
            strSql += vbCrLf + " ,ISCONDENSE= " & IIf(chkIsCondenses.Checked, 1, 0) & "" 'ISCONDENSE
            strSql += vbCrLf + " ,ISUNDERLINE= " & IIf(chkIsUnderline.Checked, 1, 0) & "" 'ISUNDERLINE
            strSql += vbCrLf + " ,ACTIVE= " & IIf(chkactive.Checked, 1, 0) & "" 'ACTIVE
            strSql += vbCrLf + " ,ISMEDIUM= " & IIf(chkIsMedium.Checked, 1, 0) & "" 'ISMEDIUM
            strSql += vbCrLf + " ,ISCENTRE= " & IIf(chkIsCentre.Checked, 1, 0) & "" 'ISCENTER
            strSql += vbCrLf + " ,ISITALIC= " & IIf(chkisItalic.Checked, 1, 0) & "" 'ISITALIC
            strSql += vbCrLf + " ,FONTNAME= '" & cmbfont.Text.ToString() & "'" 'FONTNAME
            strSql += vbCrLf + " ,ALIGNMENT= " & Val(cmballig_OWN.SelectedValue.ToString()) & "" 'FONTNAME
            strSql += vbCrLf + " WHERE SNO='" & sno & "' AND FORMATTYPE=" & Val(cmbformattype.Text.ToString()) & ""
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()
        End If
    End Sub

    Private Sub btnsave1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnsave1.Click
        If upflag = False Then
            If txtPrintRow_DEC.Text = "" Then
                MsgBox("Print Row should not be empty.", MsgBoxStyle.Information)
                cmbformattype.Focus()
                Exit Sub
            End If
            If txtcolname.Text = "" Then
                MsgBox("Column name should not be empty.", MsgBoxStyle.Information)
                txtcolname.Focus()
                Exit Sub
            End If
            If txtcolwidth_NUM.Text = "" Then
                MsgBox("Column width should not be empty.", MsgBoxStyle.Information)
                txtcolwidth_NUM.Focus()
                Exit Sub
            End If
            strSql = "SELECT COUNT(*) FROM " & cnAdminDb & "..CHQPRINT_FORMAT WHERE FORMATTYPE=" & Val(cmbformattype.Text.ToString()) & ""
            strSql += " AND COLNAME='" & txtcolname.Text.ToString() & "' "
            If GetSqlValue(cn, strSql) > 0 And upflag = False Then
                MsgBox("This Column name is already belong to this format.", MsgBoxStyle.Information)
                cmbformattype.Focus()
                Exit Sub
            End If
            FuncSave()
            MsgBox("Record added Successfully.")
            funcNew()
        Else
            FncUpdate(snonum)
            MsgBox("Update Successfully.")
            funcNew()
        End If

    End Sub

    Private Sub btnnew1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnnew1.Click
        funcNew()
    End Sub

    Private Sub btnexit12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnexit12.Click
        Me.Close()
    End Sub

    Private Sub ExitToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem1.Click
        Me.Close()
    End Sub

    Private Sub saveToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles saveToolStripMenuItem2.Click
        btnsave1_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem3.Click
        btnnew1_Click(Me, New EventArgs)
    End Sub

    Private Sub txtfortype_NUM_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub
    Private Sub GridView_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles GridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            If GridView.RowCount > 0 Then
                ' cmbformattype.Text = GridView.CurrentRow.Cells("FORMATTYPE").Value.ToString()
                txtcolname.Text = GridView.CurrentRow.Cells("COLNAME").Value.ToString()
                txtPrintRow_DEC.Text = GridView.CurrentRow.Cells("PRINTROW").Value.ToString()
                txtPrintcol_NUM.Text = Val(GridView.CurrentRow.Cells("PRINTCOL").Value.ToString())
                txtcolwidth_NUM.Text = Val(GridView.CurrentRow.Cells("COLWIDTH").Value.ToString())
                txtlbldesc.Text = GridView.CurrentRow.Cells("LABELDESC").Value.ToString()
                chklblprint.Checked = IIf(GridView.CurrentRow.Cells("ISLABELPRINT").Value = True, True, False)
                chkisbold.Checked = IIf(GridView.CurrentRow.Cells("ISBOLD").Value = True, True, False)
                chkdouble.Checked = IIf(GridView.CurrentRow.Cells("ISDOUBLE").Value = True, True, False)
                chkIsCondenses.Checked = IIf(GridView.CurrentRow.Cells("ISCONDENSE").Value = True, True, False)
                chkIsUnderline.Checked = IIf(GridView.CurrentRow.Cells("ISUNDERLINE").Value = True, True, False)
                chkactive.Checked = IIf(GridView.CurrentRow.Cells("ACTIVE").Value = True, True, False)
                chkIsMedium.Checked = IIf(GridView.CurrentRow.Cells("ISMEDIUM").Value = True, True, False)
                chkIsCentre.Checked = IIf(GridView.CurrentRow.Cells("ISCENTRE").Value = True, True, False)
                chkisItalic.Checked = IIf(GridView.CurrentRow.Cells("ISITALIC").Value = True, True, False)
                cmbfont.Text = GridView.CurrentRow.Cells("FONTNAME").Value.ToString()
                cmballig_OWN.SelectedIndex = Val(GridView.CurrentRow.Cells("ALIGNMENT").Value.ToString())
                snonum = GridView.CurrentRow.Cells("SNO").Value.ToString()
                upflag = True
                btnsave1.Text = "Update[F1]"
                '  cmbformattype.Enabled = False
                cmbformattype.Focus()
                cmbformattype.SelectAll()
            End If
        End If
    End Sub

    Private Sub EditToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditToolStripMenuItem1.Click
        btnEdit1_Click(Me, New EventArgs)
    End Sub

    Private Sub btnEdit1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit1.Click
        GridView.Focus()
    End Sub

    Private Sub cmbformattype_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbformattype.SelectedIndexChanged
        GetDetails()
    End Sub
End Class