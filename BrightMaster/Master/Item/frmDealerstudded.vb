Imports System.Data.OleDb
Public Class frmDealerstudded
#Region "Varialbe"
    Dim cmd As New OleDbCommand
    Dim da As New OleDbDataAdapter
    Dim dr As OleDbDataReader
    Dim strsql As String = ""
    Dim dt As New DataTable
    Dim flagupdate As Boolean = False
    Dim flagUROWID As Integer = 0
#End Region

#Region " Form Load"
    Private Sub frmDealerstudded_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        tabmain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabmain.Region = New Region(New RectangleF(Me.tabgenral.Left, Me.tabgenral.Top, Me.tabgenral.Width, Me.tabgenral.Height))
        btnNew_Click(Me, New System.EventArgs)
    End Sub
    Private Sub frmDealerstudded_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
#End Region

#Region "User Defined Function"
    Private Sub funcAcName()
        strsql = " SELECT * FROM ( SELECT ACNAME,ACCODE,1 RESULT "
        strsql += " FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE IN ('G','D') "
        strsql += " UNION ALL"
        strsql += " SELECT '' ACNAME,'' ACCODE,0 RESULT  "
        strsql += " )X ORDER BY RESULT,ACNAME"
        cmd = New OleDbCommand(strsql, cn)
        Dim dr As OleDbDataReader = cmd.ExecuteReader
        Dim dt As New DataTable
        dt.Load(dr)
        cmbobx_Acname_OWN.DataSource = Nothing
        cmbobxAcNametab2_OWN.DataSource = Nothing
        If dt.Rows.Count > 0 Then
            cmbobx_Acname_OWN.ValueMember = "ACCODE"
            cmbobx_Acname_OWN.DisplayMember = "ACNAME"
            cmbobx_Acname_OWN.DataSource = dt
            cmbobxAcNametab2_OWN.ValueMember = "ACCODE"
            cmbobxAcNametab2_OWN.DisplayMember = "ACNAME"
            cmbobxAcNametab2_OWN.DataSource = dt
        End If
    End Sub
    Private Sub functItemName()

        strsql = " SELECT * FROM ( SELECT ITEMNAME,ITEMID,1 RESULT "
        strsql += " FROM " & cnAdminDb & "..ITEMMAST "  'ORDER BY ITEMID "
        strsql += " UNION ALL "
        strsql += " Select '' ITEMNAME,'' ITEMID,0 RESULT  "
        strsql += " )X ORDER BY RESULT,ITEMID"
        cmd = New OleDbCommand(strsql, cn)
        Dim dr As OleDbDataReader = cmd.ExecuteReader
        Dim dt As New DataTable
        dt.Load(dr)
        cmbobx_item_OWN.DataSource = Nothing
        cmbobxItmNametab2_OWN.DataSource = Nothing
        If dt.Rows.Count > 0 Then
            cmbobx_item_OWN.ValueMember = "ITEMID"
            cmbobx_item_OWN.DisplayMember = "ITEMNAME"
            cmbobx_item_OWN.DataSource = dt
            cmbobxItmNametab2_OWN.ValueMember = "ITEMID"
            cmbobxItmNametab2_OWN.DisplayMember = "ITEMNAME"
            cmbobxItmNametab2_OWN.DataSource = dt
        End If
    End Sub
    Private Sub funcStnItemName()
        'strsql = "SELECT ITEMNAME,ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE  DIASTONE IN ('D','S') ORDER BY ITEMID "
        strsql = " SELECT * FROM ( SELECT ITEMNAME,ITEMID,1 RESULT "
        strsql += " FROM " & cnAdminDb & "..ITEMMAST WHERE  DIASTONE IN ('D','S')  "  'ORDER BY ITEMID "
        strsql += " UNION ALL "
        strsql += " Select '' ITEMNAME,'' ITEMID,0 RESULT  "
        strsql += " )X ORDER BY RESULT,ITEMID"
        cmd = New OleDbCommand(strsql, cn)
        Dim dr As OleDbDataReader = cmd.ExecuteReader
        Dim dt As New DataTable
        dt.Load(dr)
        cmbobx_stn_OWN.DataSource = Nothing
        If dt.Rows.Count > 0 Then
            cmbobx_stn_OWN.ValueMember = "ITEMID"
            cmbobx_stn_OWN.DisplayMember = "ITEMNAME"
            cmbobx_stn_OWN.DataSource = dt
        End If
    End Sub
    Function funcOpen()
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Function
        tabmain.SelectedTab = Tabview
        Return 0
    End Function
    Function callgridview() As Int16
        Dim dt As New DataTable
        dt.Clear()
        strsql = " SELECT  ROWID"
        strsql += vbCrLf + " ,(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=DS.ACCODE ) AS ACCODE "
        strsql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=DS.ACCODE ) AS ACNAME "
        strsql += vbCrLf + " ,DS.ITEMID, I.ITEMNAME, DS.SUBITEMID "
        strsql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = DS.SUBITEMID ) AS SUBITEMNAME "
        strsql += vbCrLf + " ,DS.STNITEMID "
        strsql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN ('D','S')  AND ITEMID= DS.STNITEMID) AS STONEIEMNAME"
        strsql += vbCrLf + " ,DS.STNSUBITEMID "
        strsql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID= DS.STNSUBITEMID AND ITEMID = DS.STNITEMID) AS SUBSTONEIEMNAME"
        strsql += vbCrLf + " ,CASE WHEN DS.CALCMODE = 'W' THEN 'WEIGHT' ELSE 'PIECE' END AS CALCMODE "
        strsql += vbCrLf + " ,CASE WHEN DS.STONEUNIT= 'C' THEN 'CARAT' ELSE 'GRAM' END AS STONEUNIT "
        strsql += vbCrLf + " ,DS.PSTNRATE "
        strsql += vbCrLf + " FROM " & cnAdminDb & "..DEALER_STUDDED AS DS "
        strsql += vbCrLf + " ," & cnAdminDb & "..ITEMMAST AS I "
        strsql += vbCrLf + " WHERE DS.ITEMID=I.ITEMID "
        If cmbobxAcNametab2_OWN.Text <> "" And cmbobxAcNametab2_OWN.Text <> "ALL" Then
            strsql += vbCrLf + " AND DS.ACCODE='" & cmbobxAcNametab2_OWN.SelectedValue & "' "
        End If
        If cmbobxItmNametab2_OWN.Text <> "" And cmbobxItmNametab2_OWN.Text <> "ALL" Then
            strsql += vbCrLf + " AND DS.ITEMID = '" & cmbobxItmNametab2_OWN.SelectedValue & "' "
        End If
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dt)
        gridview_detail.DataSource = Nothing
        If dt.Rows.Count > 0 Then
            With gridview_detail
                .DataSource = dt
                .Columns("ITEMID").Visible = False
                .Columns("SUBITEMID").Visible = False
                .Columns("STNITEMID").Visible = False
                .Columns("STNSUBITEMID").Visible = False
                FormatGridColumns(gridview_detail, False, True, True, True)
                autoresize()
                Return 1
            End With
        Else
            MsgBox("No Record found", MsgBoxStyle.Information)
            Exit Function
            Return 0
        End If
        Return 0
    End Function
    Function autoresize()
        If gridview_detail.RowCount > 0 Then
            If AutoReziseToolStripMenuItem.Checked Then
                gridview_detail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridview_detail.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridview_detail.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridview_detail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridview_detail.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridview_detail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
        End If
    End Function
    Function update()
        strsql = " EXEC " & cnAdminDb & "..INSERT_DEALER_STUDDED"
        strsql += vbCrLf + "@ADMINDB ='" & cnAdminDb & "'"
        strsql += vbCrLf + ",@ITEMID ='" & txtbx_itemid.Text & "'"
        strsql += vbCrLf + ",@SUBITEMID ='" & cmbobx_subitm_OWN.SelectedValue.ToString & "'"
        strsql += vbCrLf + ",@STNITEMID ='" & txtbx_stoneid.Text & "'"
        strsql += vbCrLf + ",@STNSUBITEMID ='" & cmbobx_stnsubitm_OWN.SelectedValue.ToString & "'"
        strsql += vbCrLf + ",@PSTNRATE ='" & txtbx_stnrate.Text & "'"
        strsql += vbCrLf + ",@CALCMODE ='" & Mid(cmbobx_calmode.Text, 1, 1) & "'"
        strsql += vbCrLf + ",@STONEUNIT ='" & Mid(cmbobx_unit.Text, 1, 1) & "'"
        strsql += vbCrLf + ",@ACCODE ='" & cmbobx_Acname_OWN.SelectedValue.ToString & "'"
        strsql += vbCrLf + ",@USERID ='" & userId & "'"
        strsql += vbCrLf + ",@COSTID ='" & cnCostId & "'"
        strsql += vbCrLf + ",@DEDUCT ='0' "
        strsql += vbCrLf + ",@FROM_WT ='0' "
        strsql += vbCrLf + ",@TO_WT ='0' "
        strsql += vbCrLf + ",@UROWID = " & flagUROWID & " "
        strsql += vbCrLf + ",@FLAG = " & flagupdate & " "
        cmd = New OleDbCommand(strsql, cn, tran)
        cmd.ExecuteNonQuery()
    End Function

#End Region

#Region " Button Events"
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click
        Try
            If Val(txtbx_itemid.Text) <> cmbobx_item_OWN.SelectedValue.ToString Then
                MsgBox("")
                Exit Sub
            End If
            Dim chksaveitem As Integer = 0
            Dim chksubitem As String = ""
            strsql = " SELECT SUBITEM FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID='" & txtbx_itemid.Text & "' "
            chksubitem = objGPack.GetSqlValue(strsql).ToString
            strsql = "  SELECT COUNT(*)CNT FROM " & cnAdminDb & "..DEALER_STUDDED "
            strsql += " WHERE ITEMID= '" & txtbx_itemid.Text & "' "
            If cmbobx_subitm_OWN.SelectedValue Is Nothing Then
                strsql += " And SUBITEMID= '0'"
            Else
                strsql += " And SUBITEMID= '" & cmbobx_subitm_OWN.SelectedValue.ToString & "' "
            End If
            strsql += " And STNITEMID= '" & txtbx_stoneid.Text & "' "
            If cmbobx_stnsubitm_OWN.SelectedValue Is Nothing Then
                strsql += " And STNSUBITEMID= '0'"
            Else
                strsql += " And STNSUBITEMID= '" & cmbobx_stnsubitm_OWN.SelectedValue.ToString & "' "
            End If
            'strsql += " And CALCMODE= '" & Mid(cmbobx_calmode.Text, 1, 1) & "'  "
            'strsql += " And STONEUNIT='" & Mid(cmbobx_unit.Text, 1, 1) & "' "
            strsql += " AND ACCODE = '" & cmbobx_Acname_OWN.SelectedValue.ToString & "'"
            chksaveitem = Val(objGPack.GetSqlValue(strsql).ToString)

            If chksubitem = "Y" Then

                If chksaveitem > 0 Then
                    MsgBox("Already saved", MsgBoxStyle.Information)
                    cmbobx_item_OWN.Focus()
                    cmbobx_item_OWN.SelectAll()
                    Exit Sub
                Else
                    If cmbobx_Acname_OWN.Text.Trim = "" Then
                        MsgBox("Acname should not Empty", MsgBoxStyle.Information)
                        cmbobx_Acname_OWN.Focus()
                        cmbobx_Acname_OWN.SelectAll()
                        Exit Sub
                    End If
                    If cmbobx_unit.Text.Trim = "" Then
                        MsgBox("Unit should not Empty", MsgBoxStyle.Information)
                        cmbobx_Acname_OWN.Focus()
                        cmbobx_Acname_OWN.SelectAll()
                        Exit Sub
                    End If
                    If cmbobx_calmode.Text.Trim = "" Then
                        MsgBox("Calc Mode should not Empty", MsgBoxStyle.Information)
                        cmbobx_Acname_OWN.Focus()
                        cmbobx_Acname_OWN.SelectAll()
                        Exit Sub
                    End If
                    If cmbobx_item_OWN.Text.Trim = "" Then
                        MsgBox("Itemname should not Empty", MsgBoxStyle.Information)
                        cmbobx_item_OWN.Focus()
                        cmbobx_item_OWN.SelectAll()
                        Exit Sub
                    End If
                    If flagupdate = True Then
                        'If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..DEALER_STUDDED WHERE ROWID = " & flagUROWID & "").Length > 0 Then
                        '    MsgBox("Itemid Already Exist", MsgBoxStyle.Information)
                        '    'txtEmpId_Num_Man.Focus()
                        '    Exit Sub
                        'End If
                    End If
                    Me.Cursor = Cursors.WaitCursor
                    tran = Nothing
                    tran = cn.BeginTransaction
                    If flagupdate = True Then
                        update()
                    Else
                        strsql = " EXEC " & cnAdminDb & "..INSERT_DEALER_STUDDED"
                        strsql += vbCrLf + "@ADMINDB ='" & cnAdminDb & "'"
                        strsql += vbCrLf + ",@ITEMID ='" & txtbx_itemid.Text & "'"
                        strsql += vbCrLf + ",@SUBITEMID ='" & cmbobx_item_OWN.SelectedValue & "'"
                        strsql += vbCrLf + ",@STNITEMID ='" & txtbx_stoneid.Text & "'"
                        strsql += vbCrLf + ",@STNSUBITEMID ='" & cmbobx_stnsubitm_OWN.SelectedValue & "'"
                        strsql += vbCrLf + ",@PSTNRATE ='" & Val(txtbx_stnrate.Text) & "'"
                        strsql += vbCrLf + ",@CALCMODE ='" & Mid(cmbobx_calmode.Text, 1, 1) & "'"
                        strsql += vbCrLf + ",@STONEUNIT ='" & Mid(cmbobx_unit.Text, 1, 1) & "'"
                        strsql += vbCrLf + ",@ACCODE ='" & cmbobx_Acname_OWN.SelectedValue.ToString & "'"
                        strsql += vbCrLf + ",@USERID ='" & userId & "'"
                        strsql += vbCrLf + ",@COSTID ='" & cnCostId & "'"
                        strsql += vbCrLf + ",@DEDUCT ='0' "
                        strsql += vbCrLf + ",@FROM_WT ='0'"
                        strsql += vbCrLf + ",@TO_WT ='0' "
                        strsql += vbCrLf + ",@UROWID = " & flagUROWID & " "
                        strsql += vbCrLf + ",@FLAG = " & flagupdate & " "
                        cmd = New OleDbCommand(strsql, cn, tran)
                        cmd.ExecuteNonQuery()
                    End If
                    tran.Commit()
                    tran = Nothing
                    MsgBox("Saved Successfully..")
                    clearTextbox()
                    cmbobx_Acname_OWN.Focus()
                End If
            Else
                MsgBox("Checked Subitem", MsgBoxStyle.Information)
                cmbobx_item_OWN.Focus()
                cmbobx_item_OWN.SelectAll()
                Exit Sub
            End If
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message & vbCrLf & ex.StackTrace, MsgBoxStyle.Information)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
    Private Function clearTextbox()
        txtbx_itemid.Text = ""
        txtbx_stoneid.Text = ""
        txtbx_stnrate.Text = ""
        cmbobx_Acname_OWN.Text = ""
        cmbobx_calmode.Text = ""
        cmbobx_item_OWN.Text = ""
        cmbobx_stn_OWN.Text = ""
        cmbobx_stnsubitm_OWN.Text = ""
        cmbobx_subitm_OWN.Text = ""
        flagupdate = False
        flagUROWID = 0
        txtbx_itemid.ReadOnly = False
    End Function

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        clearTextbox()
        funcAcName()
        functItemName()
        funcStnItemName()
        callgridview()
        cmbobx_Acname_OWN.Focus()
    End Sub
#End Region

#Region "Combox Event"
    Private Sub cmbobx_subitm_KeyDown(sender As Object, e As KeyEventArgs) Handles cmbobx_subitm_OWN.KeyDown
        If e.KeyCode = Keys.Enter Then
            Dim chksubitem As String = ""
            strsql = " SELECT SUBITEM FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID='" & txtbx_itemid.Text & "' "
            chksubitem = objGPack.GetSqlValue(strsql).ToString
            If chksubitem = "Y" Then

                If cmbobx_subitm_OWN.SelectedValue Is Nothing Then
                    MsgBox("Subitem Name not found in Master", MsgBoxStyle.Information)
                    cmbobx_subitm_OWN.Focus()
                    cmbobx_subitm_OWN.SelectAll()
                    Exit Sub
                Else
                    'txtbx_subitmid.Text = cmbobx_subitm_OWN.SelectedValue.ToString
                End If
            End If
        End If

    End Sub

    'Private Sub cmbobx_stnsubitm_KeyDown(sender As Object, e As KeyEventArgs) Handles cmbobx_stnsubitm_OWN.KeyDown
    '    If e.KeyCode = Keys.Enter Then
    '        strsql = "SELECT SUBITEMNAME,SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST AS S"
    '        strsql += " INNER JOIN " & cnAdminDb & "..ITEMMAST AS I "
    '        strsql += " ON S.ITEMID = I.ITEMID "
    '        strsql += " WHERE I.ITEMNAME ='" & cmbobx_stn_OWN.Text & "' "
    '        cmd = New OleDbCommand(strsql, cn)
    '        Dim dr As OleDbDataReader = cmd.ExecuteReader
    '        Dim dt As New DataTable
    '        dt.Load(dr)
    '        cmbobx_stnsubitm_OWN.DataSource = Nothing
    '        If dt.Rows.Count > 0 Then
    '            'cmbobx_stnsubitm_OWN.Text = dt.Rows(0).Item("SUBITEMNAME").ToString
    '            cmbobx_stnsubitm_OWN.ValueMember = "SUBITEMID"
    '            cmbobx_stnsubitm_OWN.DisplayMember = "SUBITEMNAME"
    '            cmbobx_stnsubitm_OWN.DataSource = dt
    '        End If
    '    End If
    'End Sub

    Private Sub cmbobx_item_OWN_KeyDown(sender As Object, e As KeyEventArgs) Handles cmbobx_item_OWN.KeyDown
        If e.KeyCode = Keys.Enter Then

            If cmbobx_item_OWN.SelectedValue Is Nothing Then
                MsgBox("ItemName not found in master", MsgBoxStyle.Information)
                cmbobx_item_OWN.Focus()
                cmbobx_item_OWN.SelectAll()
            Else
                txtbx_itemid.Text = cmbobx_item_OWN.SelectedValue.ToString
            End If
            strsql = "SELECT SUBITEMNAME,SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST AS S"
            strsql += " INNER JOIN " & cnAdminDb & "..ITEMMAST AS I "
            strsql += " ON S.ITEMID = I.ITEMID "
            strsql += " WHERE I.ITEMNAME ='" & cmbobx_item_OWN.Text & "' "
            cmd = New OleDbCommand(strsql, cn)
            Dim dr As OleDbDataReader = cmd.ExecuteReader
            Dim dt As New DataTable
            dt.Load(dr)
            cmbobx_subitm_OWN.DataSource = Nothing
            If dt.Rows.Count > 0 Then
                'cmbobx_subitm_OWN.Text = dt.Rows(0).Item("subitemname").ToString
                cmbobx_subitm_OWN.ValueMember = "SUBITEMID"
                cmbobx_subitm_OWN.DisplayMember = "SUBITEMNAME"
                cmbobx_subitm_OWN.DataSource = dt
            End If

        End If
    End Sub
    Private Sub cmbobx_stn_OWN_KeyDown(sender As Object, e As KeyEventArgs) Handles cmbobx_stn_OWN.KeyDown
        If e.KeyCode = Keys.Enter Then
            If cmbobx_stn_OWN.SelectedValue Is Nothing Then
                MsgBox("Stone Name not found in Master", MsgBoxStyle.Information)
                cmbobx_stn_OWN.Focus()
                cmbobx_stn_OWN.SelectAll()
            Else
                txtbx_stoneid.Text = cmbobx_stn_OWN.SelectedValue.ToString
            End If
            strsql = "SELECT SUBITEMNAME,SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST AS S"
            strsql += " INNER JOIN " & cnAdminDb & "..ITEMMAST AS I "
            strsql += " ON S.ITEMID = I.ITEMID "
            strsql += " WHERE I.ITEMNAME ='" & cmbobx_stn_OWN.Text & "' "
            cmd = New OleDbCommand(strsql, cn)
            Dim dr As OleDbDataReader = cmd.ExecuteReader
            Dim dt As New DataTable
            dt.Load(dr)
            cmbobx_stnsubitm_OWN.DataSource = Nothing
            If dt.Rows.Count > 0 Then
                'cmbobx_stnsubitm_OWN.Text = dt.Rows(0).Item("SUBITEMNAME").ToString
                cmbobx_stnsubitm_OWN.ValueMember = "SUBITEMID"
                cmbobx_stnsubitm_OWN.DisplayMember = "SUBITEMNAME"
                cmbobx_stnsubitm_OWN.DataSource = dt
            End If
        End If
    End Sub
    Private Sub cmbobx_subitm_OWN_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cmbobx_subitm_OWN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            'If cmbobx_subitm_OWN.SelectedValue Is Nothing Then
            '    MsgBox("Subitem Name not found in Master", MsgBoxStyle.Information)
            '    cmbobx_subitm_OWN.Focus()
            '    cmbobx_subitm_OWN.SelectAll()
            '    Exit Sub
            'Else
            '    txtbx_subitmid.Text = cmbobx_subitm_OWN.SelectedValue.ToString
            'End If
        End If
    End Sub
    Private Sub cmbobx_stn_OWN_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cmbobx_stn_OWN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            'If cmbobx_stn_OWN.SelectedValue Is Nothing Then
            '    MsgBox("Stone Name not found in Master", MsgBoxStyle.Information)
            '    cmbobx_stn_OWN.Focus()
            '    cmbobx_stn_OWN.SelectAll()
            'Else
            '    txtbx_stoneid.Text = cmbobx_stn_OWN.SelectedValue.ToString
            'End If
            'strsql = "SELECT SUBITEMNAME,SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST AS S"
            'strsql += " INNER JOIN " & cnAdminDb & "..ITEMMAST AS I "
            'strsql += " ON S.ITEMID = I.ITEMID "
            'strsql += " WHERE I.ITEMNAME ='" & cmbobx_stn_OWN.Text & "' "
            'cmd = New OleDbCommand(strsql, cn)
            'Dim dr As OleDbDataReader = cmd.ExecuteReader
            'Dim dt As New DataTable
            'dt.Load(dr)
            'cmbobx_stnsubitm_OWN.DataSource = Nothing
            'If dt.Rows.Count > 0 Then
            '    'cmbobx_stnsubitm_OWN.Text = dt.Rows(0).Item("SUBITEMNAME").ToString
            '    cmbobx_stnsubitm_OWN.ValueMember = "SUBITEMID"
            '    cmbobx_stnsubitm_OWN.DisplayMember = "SUBITEMNAME"
            '    cmbobx_stnsubitm_OWN.DataSource = dt
            'End If

        End If
    End Sub

    Private Sub cmbobx_stnsubitm_OWN_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cmbobx_stnsubitm_OWN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            'If cmbobx_stnsubitm_OWN.SelectedValue Is Nothing Then
            '    MsgBox("StonesubItem Name not Found in Master", MsgBoxStyle.Information)
            '    cmbobx_stnsubitm_OWN.Focus()
            '    cmbobx_stnsubitm_OWN.SelectAll()
            '    Exit Sub
            'Else
            '    txtbx_stnsubitmid.Text = cmbobx_stnsubitm_OWN.SelectedValue.ToString
            'End If
        End If
    End Sub
#End Region

#Region " Textbox Events"
    Private Sub txtbx_itemid_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtbx_itemid.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            strsql = "SELECT ITEMNAME,ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID= '" & txtbx_itemid.Text & "' "
            cmd = New OleDbCommand(strsql, cn)
            Dim dr As OleDbDataReader = cmd.ExecuteReader
            Dim dt As New DataTable
            dt.Load(dr)
            'cmbobx_item_OWN.DataSource = Nothing
            If dt.Rows.Count > 0 Then
                cmbobx_item_OWN.Text = dt.Rows(0).Item("ITEMNAME").ToString
                'cmbobx_item_OWN.ValueMember = "ITEMID"
                'cmbobx_item_OWN.DisplayMember = "ITEMNAME"
                'cmbobx_item_OWN.DataSource = dt
            End If
        End If
        'funcharlock(Me, New System.EventArgs)
        If (Not Char.IsControl(e.KeyChar) _
           AndAlso (Not Char.IsDigit(e.KeyChar) _
           AndAlso (e.KeyChar <> Microsoft.VisualBasic.ChrW(46)))) Then
            e.Handled = True
        End If
    End Sub

    'Private Sub txtbx_subitmid_KeyPress(sender As Object, e As KeyPressEventArgs)
    '    If e.KeyChar = Chr(Keys.Enter) Then
    '        strsql = "SELECT SUBITEMNAME,SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID= '" & txtbx_subitmid.Text & "' "
    '        cmd = New OleDbCommand(strsql, cn)
    '        Dim dr As OleDbDataReader = cmd.ExecuteReader
    '        Dim dt As New DataTable
    '        dt.Load(dr)
    '        'cmbobx_stnsubitm_OWN.DataSource = Nothing
    '        If dt.Rows.Count > 0 Then
    '            cmbobx_subitm_OWN.Text = dt.Rows(0).Item("SUBITEMNAME").ToString
    '            'cmbobx_stnsubitm_OWN.ValueMember = "SUBITEMID"
    '            'cmbobx_stnsubitm_OWN.DisplayMember = "SUBITEMNAME"
    '            'cmbobx_stnsubitm_OWN.DataSource = dt
    '        End If
    '    End If
    '    If (Not Char.IsControl(e.KeyChar) _
    '        AndAlso (Not Char.IsDigit(e.KeyChar) _
    '        AndAlso (e.KeyChar <> Microsoft.VisualBasic.ChrW(46)))) Then
    '        e.Handled = True
    '    End If
    'End Sub

    Private Sub txtbx_stoneid_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtbx_stoneid.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            strsql = "SELECT ITEMNAME,ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE  DIASTONE IN ('D','S') AND ITEMID = '" & txtbx_stoneid.Text & "' ORDER BY ITEMID "
            cmd = New OleDbCommand(strsql, cn)
            Dim dr As OleDbDataReader = cmd.ExecuteReader
            Dim dt As New DataTable
            dt.Load(dr)
            If dt.Rows.Count > 0 Then
                cmbobx_stn_OWN.Text = dt.Rows(0).Item("ITEMNAME").ToString
                'cmbobx_stn_OWN.ValueMember = "ITEMID"
                'cmbobx_stn_OWN.DisplayMember = "ITEMNAME"
                'cmbobx_stn_OWN.DataSource = Nothing
                'cmbobx_stn_OWN.DataSource = dt
            End If
        End If
        If (Not Char.IsControl(e.KeyChar) _
            AndAlso (Not Char.IsDigit(e.KeyChar) _
            AndAlso (e.KeyChar <> Microsoft.VisualBasic.ChrW(46)))) Then
            e.Handled = True
        End If
    End Sub

    'Private Sub txtbx_stnsubitmid_KeyPress(sender As Object, e As KeyPressEventArgs)
    '    If e.KeyChar = Chr(Keys.Enter) Then
    '        strsql = "SELECT SUBITEMNAME,SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST AS S"
    '        strsql += " INNER JOIN " & cnAdminDb & "..ITEMMAST AS I "
    '        strsql += " ON S.ITEMID = I.ITEMID "
    '        strsql += " WHERE I.ITEMID ='" & txtbx_stnsubitmid.Text & "' "
    '        cmd = New OleDbCommand(strsql, cn)
    '        Dim dr As OleDbDataReader = cmd.ExecuteReader
    '        Dim dt As New DataTable
    '        dt.Load(dr)
    '        If dt.Rows.Count > 0 Then
    '            cmbobx_stnsubitm_OWN.Text = dt.Rows(0).Item("SUBITEMNAME").ToString
    '            'cmbobx_stnsubitm_OWN.ValueMember = "SUBITEMID"
    '            'cmbobx_stnsubitm_OWN.DisplayMember = "SUBITEMNAME"
    '            ''cmbobx_stnsubitm_OWN.DataSource = Nothing
    '            'cmbobx_stnsubitm_OWN.DataSource = dt
    '        End If
    '    End If
    '    If (Not Char.IsControl(e.KeyChar) _
    '        AndAlso (Not Char.IsDigit(e.KeyChar) _
    '        AndAlso (e.KeyChar <> Microsoft.VisualBasic.ChrW(46)))) Then
    '        e.Handled = True
    '    End If
    'End Sub
    Private Sub btnOpen_Click(sender As Object, e As EventArgs) Handles btnOpen.Click, OpenToolStripMenuItem.Click
        Try
            funcOpen()
            cmbobx_Acname_OWN.Focus()
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        tabmain.SelectedTab = tabgenral
    End Sub
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        callgridview()
    End Sub
    Private Sub gridview_detail_KeyDown(sender As Object, e As KeyEventArgs) Handles gridview_detail.KeyDown
        If e.KeyCode = Keys.Enter Then
            Dim index As Integer
            Dim selectedrow As DataGridViewRow
            selectedrow = gridview_detail.Rows(index)
            cmbobx_Acname_OWN.Text = gridview_detail.Rows(gridview_detail.CurrentRow.Index).Cells("ACNAME").Value.ToString
            txtbx_itemid.Text = gridview_detail.Rows(gridview_detail.CurrentRow.Index).Cells("ITEMID").Value.ToString
            cmbobx_item_OWN.Text = gridview_detail.Rows(gridview_detail.CurrentRow.Index).Cells("ITEMNAME").Value.ToString
            'txtbx_subitmid.Text = gridview_detail.Rows(gridview_detail.CurrentRow.Index).Cells("SUBITEMID").Value.ToString
            cmbobx_stnsubitm_OWN.Text = gridview_detail.Rows(gridview_detail.CurrentRow.Index).Cells("SUBITEMID").Value.ToString
            txtbx_stoneid.Text = gridview_detail.Rows(gridview_detail.CurrentRow.Index).Cells("STNITEMID").Value.ToString
            cmbobx_stn_OWN.Text = gridview_detail.Rows(gridview_detail.CurrentRow.Index).Cells("STONEIEMNAME").Value.ToString
            'txtbx_stnsubitmid.Text = gridview_detail.Rows(gridview_detail.CurrentRow.Index).Cells("SUBSTONEIEMNAME").Value.ToString
            cmbobx_stnsubitm_OWN.Text = gridview_detail.Rows(gridview_detail.CurrentRow.Index).Cells("SUBSTONEIEMNAME").Value.ToString
            cmbobx_calmode.Text = gridview_detail.Rows(gridview_detail.CurrentRow.Index).Cells("CALCMODE").Value.ToString
            cmbobx_unit.Text = gridview_detail.Rows(gridview_detail.CurrentRow.Index).Cells("STONEUNIT").Value.ToString
            txtbx_stnrate.Text = gridview_detail.Rows(gridview_detail.CurrentRow.Index).Cells("PSTNRATE").Value.ToString
            flagupdate = True
            flagUROWID = gridview_detail.Rows(gridview_detail.CurrentRow.Index).Cells("ROWID").Value.ToString
            tabmain.SelectedTab = tabgenral
            txtbx_itemid.ReadOnly = True
        End If
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If gridview_detail.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, Label1.Text, gridview_detail, BrightPosting.GExport.GExportType.Export, , , , , , True)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If gridview_detail.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, Label1.Text, gridview_detail, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    'Private Sub cmbobx_stnsubitm_OWN_KeyDown(sender As Object, e As KeyEventArgs) Handles cmbobx_stnsubitm_OWN.KeyDown
    '    If e.KeyCode = Keys.Enter Then

    '        Dim chkstnsubitem As String = ""
    '        strsql = " SELECT SUBITEM FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID='" & txtbx_stnsubitmid.Text & "' "
    '        chkstnsubitem = objGPack.GetSqlValue(strsql).ToString
    '        If chkstnsubitem = "Y" Then
    '            If cmbobx_stnsubitm_OWN.SelectedValue Is Nothing Then
    '                MsgBox("StonesubItem Name not Found in Master", MsgBoxStyle.Information)
    '                cmbobx_stnsubitm_OWN.Focus()
    '                cmbobx_stnsubitm_OWN.SelectAll()
    '                Exit Sub
    '            Else
    '                txtbx_stnsubitmid.Text = cmbobx_stnsubitm_OWN.SelectedValue.ToString
    '            End If
    '        End If
    '    End If
    'End Sub

    Private Sub txtbx_stnrate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtbx_stnrate.KeyPress
        If (Not Char.IsControl(e.KeyChar) _
           AndAlso (Not Char.IsDigit(e.KeyChar) _
           AndAlso (e.KeyChar <> Microsoft.VisualBasic.ChrW(46)))) Then
            e.Handled = True
        End If
    End Sub

    Private Sub cmbobx_Acname_OWN_KeyDown(sender As Object, e As KeyEventArgs) Handles cmbobx_Acname_OWN.KeyDown
        If e.KeyCode = Keys.Enter Then
            If cmbobx_Acname_OWN.SelectedValue Is Nothing Then
                MsgBox("AC Name not Found in Master", MsgBoxStyle.Information)
                cmbobx_Acname_OWN.Focus()
                cmbobx_Acname_OWN.SelectAll()
                Exit Sub

            End If
        End If
    End Sub

    'Private Sub cmbobx_item_OWN_KeyDown(sender As Object, e As KeyEventArgs) Handles cmbobx_item_OWN.KeyDown
    '    If e.KeyCode = Keys.Enter Then
    '        strsql = "SELECT SUBITEMNAME,SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST AS S"
    '            strsql += " INNER JOIN " & cnAdminDb & "..ITEMMAST AS I "
    '            strsql += " ON S.ITEMID = I.ITEMID "
    '            strsql += " WHERE I.ITEMNAME ='" & cmbobx_stn_OWN.Text & "' "
    '            cmd = New OleDbCommand(strsql, cn)
    '            Dim dr As OleDbDataReader = cmd.ExecuteReader
    '            Dim dt As New DataTable
    '            dt.Load(dr)
    '            cmbobx_stnsubitm_OWN.DataSource = Nothing
    '            If dt.Rows.Count > 0 Then
    '                'cmbobx_stnsubitm_OWN.Text = dt.Rows(0).Item("SUBITEMNAME").ToString
    '                cmbobx_stnsubitm_OWN.ValueMember = "SUBITEMID"
    '                cmbobx_stnsubitm_OWN.DisplayMember = "SUBITEMNAME"
    '                cmbobx_stnsubitm_OWN.DataSource = dt
    '            End If
    '        End If
    'End Sub

#End Region

End Class