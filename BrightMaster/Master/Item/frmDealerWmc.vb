Imports System.Data.OleDb
Public Class frmDealerWmc

#Region "Variables"
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim dt As New DataTable
    Dim dtGridView As New DataTable
    Dim flagSave As Boolean
    Dim FLAG As Integer = 0
    Dim cmd As OleDbCommand
    Dim sid As String = Nothing
    Dim dtTemp As New DataTable
#End Region

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Try
            ' Add any initialization after the InitializeComponent() call.
            txtSno.Visible = False
            tabGeneral.BackgroundImage = bakImage
            tabGeneral.BackgroundImageLayout = ImageLayout.Stretch
            tabView.BackgroundImage = bakImage
            tabView.BackgroundImageLayout = ImageLayout.Stretch
            tabMain.ItemSize = New System.Drawing.Size(1, 1)
            'AddHandler txtTouch.KeyPress, AddressOf percentage_Keypress

            rbtDealer.Checked = True
            rbtDealer.Focus()

            cmbOpenParty.Items.Add("ALL")
            cmbOpenParty.Text = "ALL"
            objGPack.FillCombo(strSql, cmbOpenParty, , False)

            strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST ORDER BY ITEMNAME"
            objGPack.FillCombo(strSql, cmbDItem)

            cmbOpenItem.Items.Add("ALL")
            cmbOpenItem.Text = "ALL"
            objGPack.FillCombo(strSql, cmbOpenItem, , False)

            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            gridView.BorderStyle = BorderStyle.None
            With dtGridView
                .Columns.Add("SNO", GetType(Integer))
                .Columns.Add("ACNAME", GetType(String))
                .Columns.Add("ITEM", GetType(String))
                .Columns.Add("SUBITEM", GetType(String))
                .Columns.Add("TOUCH", GetType(Double))
                .Columns.Add("MAXMC", GetType(Double))
            End With
            gridView.DataSource = dtGridView
            gridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            With gridView
                .Columns("SNO").Visible = False
                .Columns("ITEM").Visible = False
                .Columns("SUBITEM").Visible = False
                .Columns("TOUCH").Visible = False
                .Columns("MAXMC").Visible = False
                .Columns("TOUCH").Width = 50
                .Columns("TOUCH").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("MAXMC").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("MAXMC").HeaderText = "MC"
            End With
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
#Region "userdefined Function"
    'Function clear()
    '    sid = ""
    '    'dt = New DataTable
    '    'dtTemp = New DataTable
    '    strSql = ""
    '    'dtGridView = New DataTable
    '    da = New OleDbDataAdapter
    '    flagSave = False
    '    cmd = New OleDbCommand

    'End Function
    Function chkcmbosubitemnload(ByVal ItemName As String)
        Try
            strSql = " SELECT SUBITEMNAME ,CONVERT(VARCHAR,SUBITEMID) SUBITEMID,2 RESULT FROM " & cnAdminDb & "..SUBITEMMAST WHERE"
            strSql += vbCrLf + " ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST "
            strSql += vbCrLf + " WHERE ITEMNAME = '" & ItemName & "') "
            strSql += vbCrLf + " ORDER BY SUBITEMNAME"
            dt = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            chkcmbx_Ositemname.Items.Clear()
            BrighttechPack.GlobalMethods.FillCombo(chkcmbx_Ositemname, dt, "SUBITEMNAME", False)
            'objGPack.FillCombo(strSql, cmbx_Ositemname, , False)
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Function

    Function funcAdd(ByVal Accode As String, ByVal ItemId As Integer, ByVal subItemId As Integer, ByVal sid As String) As Integer
        Dim dmgrsnet As String, mcalcmode As String, mccalcmode As String
        Dim omgrsnet As String
        dmgrsnet = "B"
        omgrsnet = "c"
        'If Me.optDGrs.Checked = True Then mgrsnet = "G"
        'If Me.optDNet.Checked = True Then mgrsnet = "N"
        'If Me.opt_Ogrswt.Checked = True Then mgrsnet = "G"
        'If Me.opt_Ontwt.Checked = True Then mgrsnet = "N"
        If Me.optDGrs.Checked = True Then dmgrsnet = "G"
        If Me.optDNet.Checked = True Then dmgrsnet = "N"
        If Me.opt_Ogrswt.Checked = True Then omgrsnet = "G"
        If Me.opt_Ontwt.Checked = True Then omgrsnet = "N"
        mcalcmode = "O"
        If Me.optTouch.Checked = True Then mcalcmode = "T"
        If Me.OptWast.Checked = True Then mcalcmode = "W"
        mccalcmode = "O"
        If Me.optMcgm.Checked = True Then mccalcmode = "W"
        If Me.optMcPie.Checked = True Then mccalcmode = "P"

        'strSql = " INSERT INTO " & cnAdminDb & "..DEALER_WMCTABLE"
        'strSql += vbCrLf + " (SNO,"
        'strSql += vbCrLf + " ACCODE,ITEMID,SUBITEMID,GRSNET,CALCMODE,MCCALC"
        'strSql += vbCrLf + " ,TOUCH,ALLOY,WASTPER,WAST,WASTPIE,MCGRM,MC"
        'strSql += vbCrLf + " ,FROM_WT,TO_WT"
        'strSql += vbCrLf + " ,USERID,UPDATED,UPTIME,COSTID,MCPER,PUREWT)"
        'strSql += vbCrLf + " VALUES"
        'strSql += vbCrLf + " ( "
        'strSql += vbCrLf + " " & Val(txtSno.Text) & " " 'SNO
        'strSql += vbCrLf + " ,'" & Accode & "'" 'ACCODE
        'strSql += vbCrLf + " ," & ItemId & "" 'ITEMID
        'strSql += vbCrLf + " ," & subItemId & "" 'SUBITEMID
        'strSql += vbCrLf + " ,'" & mgrsnet & "' " 'GRSNET
        'strSql += vbCrLf + " ,'" & mcalcmode & "' " 'CALCMODE
        'strSql += vbCrLf + " ,'" & mccalcmode & "'" 'MCCALC
        'strSql += vbCrLf + " ," & Val(Me.txtDTouch_Amt.Text) & "" 'TOUCH
        'strSql += vbCrLf + " ," & Val(Me.txtAlloy_Amt.Text) & "" 'ALLOY
        'strSql += vbCrLf + " ," & Val(txtDWastPer.Text) & "" 'WASTPER
        'strSql += vbCrLf + " ," & Val(txtWastgm.Text) & "" 'WASTGRAM
        'strSql += vbCrLf + " ," & Val(txtWastperPie.Text) & "" 'WASTPER
        'strSql += vbCrLf + " ," & Val(txtDMcgm.Text) & "" 'MCGRMGRAM
        'strSql += vbCrLf + " ," & Val(txtMcPie.Text) & "" 'MCGRMPIECE
        'strSql += vbCrLf + " ," & Val(txtFromWt.Text) & " " 'FROM WEIGHT
        'strSql += vbCrLf + " ," & Val(txtTowt.Text) & "" 'TOWEIGHT
        'strSql += vbCrLf + " ," & userId & "" 'USERID
        'strSql += vbCrLf + " ,'" & Format(Now.Date, "yyyy-MM-dd") & "'" 'UPDATED
        'strSql += vbCrLf + " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        'strSql += vbCrLf + " ,'" & cnCostId & "'" 'COSTID
        'strSql += vbCrLf + " ," & Val(txtDmcPer_PER.Text) & "" 'MCPER
        'strSql += vbCrLf + " ," & Val(txtDPurePer_PER.Text) & "" 'PUREWT
        'strSql += vbCrLf + " )"
        strSql = ""
        strSql += vbCrLf + "  EXEC " & cnAdminDb & "..[INSERT_DEALERVA]  "
        strSql += vbCrLf + "  @AdminDb ='" & cnAdminDb & "' "
        strSql += vbCrLf + " ,@SNO = " & Val(txtSno.Text) & "  "
        strSql += vbCrLf + " ,@ITEMID = " & ItemId & " "
        strSql += vbCrLf + " ,@SUBITEMID = " & subItemId & " "
        strSql += vbCrLf + " ,@ACCODE = '" & Accode & "'"
        strSql += vbCrLf + " ,@CALCMODE = '" & mcalcmode & "'"
        strSql += vbCrLf + " ,@GRSNET = '" & dmgrsnet & "'"
        strSql += vbCrLf + " ,@MCCALC= '" & mccalcmode & "'"
        strSql += vbCrLf + " ,@WASTPER=" & Val(txtbx_OwstgePER.Text) & ""
        strSql += vbCrLf + " ,@WAST=" & Val(txtbx_Owstge.Text) & ""
        strSql += vbCrLf + " ,@WASTPIE=" & Val(txtWastperPie.Text) & ""
        strSql += vbCrLf + " ,@MCGRM=" & Val(txtbx_Omcgm.Text) & ""
        strSql += vbCrLf + " ,@MC=" & Val(txtbx_Omc.Text) & ""
        strSql += vbCrLf + " ,@FROM_WT=" & Val(txtFromWt.Text) & ""
        strSql += vbCrLf + " ,@TO_WT =" & Val(txtTowt.Text) & ""
        strSql += vbCrLf + " ,@TOUCH=  '0' "
        strSql += vbCrLf + " ,@ALLOY=" & Val(Me.txtAlloy_Amt.Text) & " "
        strSql += vbCrLf + " ,@USERID= " & userId & ""
        strSql += vbCrLf + " ,@UPDATED='" & Format(Now.Date, "yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@UPTIME='" & Date.Now.ToLongTimeString & "'"
        strSql += vbCrLf + " ,@COSTID='" & cnCostId & "'"
        strSql += vbCrLf + " ,@MCPER=" & Val(txtbx_Omcp.Text) & ""
        strSql += vbCrLf + " ,@PUREWT=" & Val(txtDPurePer_PER.Text) & ""
        strSql += vbCrLf + " ,@TAXINCLUCIVE= '0' "
        strSql += vbCrLf + " ,@PRATE= '0' "
        strSql += vbCrLf + " ,@MCPIE= '0' "
        strSql += vbCrLf + " ,@EMCPERG='0'"
        strSql += vbCrLf + " ,@DSUBITEMNAME= '' "
        strSql += vbCrLf + " ,@D_ITEMID= " & ItemId & " "
        strSql += vbCrLf + " ,@D_SUBITEMID= " & subItemId & " "
        strSql += vbCrLf + " ,@D_TOUCH=" & Val(Me.txtDTouch_Amt.Text) & ""
        strSql += vbCrLf + " ,@D_PURE= " & Val(txtDPurePer_PER.Text) & ""
        strSql += vbCrLf + " ,@D_WAST=" & Val(txtDWastPer.Text) & ""
        strSql += vbCrLf + " ,@D_MC=" & Val(txtDmcPer_PER.Text) & ""
        strSql += vbCrLf + " ,@D_MCGRAM=" & Val(txtDMcgm.Text) & ""
        strSql += vbCrLf + " ,@DGRSNET = '" & dmgrsnet & "'"
        strSql += vbCrLf + " ,@O_ITEMID='" & Val(txtbxOitmid_NUM.Text) & "' "
        strSql += vbCrLf + " ,@O_SUBITEMID= '" & sid & "' "
        strSql += vbCrLf + " ,@OGRSNET = '" & omgrsnet & "'"
        strSql += vbCrLf + " ,@SID= '" & sid & "' "
        strSql += vbCrLf + " ,@flagSave= " & flagSave & " "
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        MsgBox("Saved Successfully", MsgBoxStyle.Information)
    End Function
    Function funcUpdate(ByVal Accode As String, ByVal ItemId As Integer, ByVal subItemId As Integer, ByVal sid As String) As Integer
        Dim dmgrsnet As String, mcalcmode As String, mccalcmode As String
        Dim omgrsnet As String
        dmgrsnet = "B"
        omgrsnet = "c"
        If Me.optDGrs.Checked = True Then dmgrsnet = "G"
        If Me.optDNet.Checked = True Then dmgrsnet = "N"
        If Me.opt_Ogrswt.Checked = True Then omgrsnet = "G"
        If Me.opt_Ontwt.Checked = True Then omgrsnet = "N"

        mcalcmode = "O"
        If Me.optTouch.Checked = True Then mcalcmode = "T"
        If Me.OptWast.Checked = True Then mcalcmode = "W"
        mccalcmode = "O"
        If Me.optMcgm.Checked = True Then mccalcmode = "W"
        If Me.optMcPie.Checked = True Then mccalcmode = "P"

        strSql = ""
        strSql += vbCrLf + "  EXEC " & cnAdminDb & "..[INSERT_DEALERVA]  "
        strSql += vbCrLf + "  @AdminDb ='" & cnAdminDb & "' "
        strSql += vbCrLf + " ,@SNO = " & Val(txtSno.Text) & "  "
        strSql += vbCrLf + " ,@ITEMID = " & ItemId & " "
        strSql += vbCrLf + " ,@SUBITEMID = " & subItemId & " "
        strSql += vbCrLf + " ,@ACCODE = '" & Accode & "'"
        strSql += vbCrLf + " ,@CALCMODE = '" & mcalcmode & "'"
        strSql += vbCrLf + " ,@GRSNET = '" & dmgrsnet & "'"
        strSql += vbCrLf + " ,@MCCALC= '" & mccalcmode & "'"
        strSql += vbCrLf + " ,@WASTPER=" & Val(txtbx_OwstgePER.Text) & ""
        strSql += vbCrLf + " ,@WAST=" & Val(txtbx_Owstge.Text) & ""
        strSql += vbCrLf + " ,@WASTPIE=" & Val(txtWastperPie.Text) & ""
        strSql += vbCrLf + " ,@MCGRM=" & Val(txtbx_Omcgm.Text) & ""
        strSql += vbCrLf + " ,@MC=" & Val(txtbx_Omc.Text) & ""
        strSql += vbCrLf + " ,@FROM_WT=" & Val(txtFromWt.Text) & ""
        strSql += vbCrLf + " ,@TO_WT =" & Val(txtTowt.Text) & ""
        strSql += vbCrLf + " ,@TOUCH=  '0' "
        strSql += vbCrLf + " ,@ALLOY=" & Val(Me.txtAlloy_Amt.Text) & " "
        strSql += vbCrLf + " ,@USERID= " & userId & ""
        strSql += vbCrLf + " ,@UPDATED='" & Format(Now.Date, "yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@UPTIME='" & Date.Now.ToLongTimeString & "'"
        strSql += vbCrLf + " ,@COSTID='" & cnCostId & "'"
        strSql += vbCrLf + " ,@MCPER=" & Val(txtbx_Omcp.Text) & ""
        strSql += vbCrLf + " ,@PUREWT=" & Val(txtDPurePer_PER.Text) & ""
        strSql += vbCrLf + " ,@TAXINCLUCIVE= '0' "
        strSql += vbCrLf + " ,@PRATE= '0' "
        strSql += vbCrLf + " ,@MCPIE= '0' "
        strSql += vbCrLf + " ,@EMCPERG='0'"
        strSql += vbCrLf + " ,@DSUBITEMNAME= '' "
        strSql += vbCrLf + " ,@D_ITEMID= " & ItemId & " "
        strSql += vbCrLf + " ,@D_SUBITEMID= " & subItemId & " "
        strSql += vbCrLf + " ,@D_TOUCH=" & Val(Me.txtDTouch_Amt.Text) & ""
        strSql += vbCrLf + " ,@D_PURE= " & Val(txtDPurePer_PER.Text) & ""
        strSql += vbCrLf + " ,@D_WAST=" & Val(txtDWastPer.Text) & ""
        strSql += vbCrLf + " ,@D_MC=" & Val(txtDmcPer_PER.Text) & ""
        strSql += vbCrLf + " ,@D_MCGRAM=" & Val(txtDMcgm.Text) & ""
        strSql += vbCrLf + " ,@DGRSNET = '" & dmgrsnet & "'"
        strSql += vbCrLf + " ,@O_ITEMID='" & Val(txtbxOitmid_NUM.Text) & "' "
        strSql += vbCrLf + " ,@O_SUBITEMID= '" & sid & "' "
        strSql += vbCrLf + " ,@OGRSNET = '" & omgrsnet & "'"
        strSql += vbCrLf + " ,@SID= '" & sid & "' "
        strSql += vbCrLf + " ,@flagSave= " & flagSave & " "
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        'strSql = " UPDATE " & cnAdminDb & "..DEALER_WMCTABLE SET"  
        'strSql += " ACCODE = '" & Accode & "'"
        'strSql += " ,ITEMID = " & ItemId & ""
        'strSql += " ,SUBITEMID = " & subItemId & ""
        'strSql += " ,TOUCH = " & Val(txtDTouch_Amt.Text) & ""
        'strSql += " ,ALLOY= " & Val(txtAlloy_Amt.Text) & ""
        'strSql += " ,GRSNET = '" & mgrsnet & "'"
        'strSql += " ,CALCMODE = '" & mcalcmode & "'"
        'strSql += " ,MCCALC = '" & mccalcmode & "'"
        'strSql += " ,WASTPER = " & Val(Me.txtDWastPer.Text) & ""
        'strSql += " ,WAST = " & Val(Me.txtWastgm.Text) & ""
        'strSql += " ,WASTPIE = " & Val(Me.txtWastperPie.Text) & ""
        'strSql += " ,MCGRM = " & Val(Me.txtDMcgm.Text) & ""
        'strSql += " ,MC = " & Val(Me.txtMcPie.Text) & ""
        'strSql += " ,MCPER = " & Val(Me.txtDmcPer_PER.Text) & ""
        'strSql += " ,PUREWT = " & Val(Me.txtDPurePer_PER.Text) & ""
        'strSql += " WHERE SNO = " & Val(txtSno.Text) & ""
        'cmd = New OleDbCommand(strSql, cn)
        'cmd.ExecuteNonQuery()
        MsgBox("Updated  Successfully", MsgBoxStyle.Information)

    End Function
#End Region
#Region "Form Function"
    Private Sub frmValueAdded_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Me.WindowState = FormWindowState.Maximized
            txtDsubItemId_NUM.Visible = False
            Cmbobxitemnameload()
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Private Sub frmValueAdded_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        Try
            If e.KeyChar = Chr(Keys.Enter) Then
                If cmbPartyName.Focused Then
                    Exit Sub
                End If
                If cmbDItem.Focused Then
                    Exit Sub
                End If
                If cmbDSubItem.Focused Then
                    Exit Sub
                End If
                If txtDTouch_Amt.Focused Then
                    Exit Sub
                End If
                SendKeys.Send("{TAB}")
            ElseIf e.KeyChar = Chr(Keys.Escape) Then
                If tabMain.SelectedTab.Name = tabView.Name Then
                    tabMain.SelectedTab = tabGeneral
                    rbtDealer.Focus()
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub
    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        btnOpen_Click(Me, New EventArgs)
    End Sub
    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        btnSave_Click(Me, New EventArgs)
    End Sub
    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub
#End Region

#Region " Button Events"
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If funcComboChecker("Party Name", cmbPartyName) Then Exit Sub
            If funcComboChecker("Item", cmbDItem, False) Then Exit Sub
            If funcComboChecker("SubItem", cmbDSubItem, False) Then Exit Sub
            If cmbDItem.Text <> cmbx_Oitmname.Text Then
                MsgBox("DEALER ITEM NAME AND OUR ITEM MISMATCH", MsgBoxStyle.Information, "Message")
                cmbx_Oitmname.Focus()
                Exit Sub
            End If
            If cmbDItem.Text = "" Then
                MsgBox("DEALER ITEM NAME EMPTY", MsgBoxStyle.Information, "Message")
                cmbDItem.Focus()
                Exit Sub
            End If
            If cmbx_Oitmname.Text = "" Then
                MsgBox(" OUR ITEM NAME EMPTY", MsgBoxStyle.Information, "Message")
                cmbx_Oitmname.Focus()
                Exit Sub
            End If
            If Val(txtDTouch_Amt.Text) > 112 Then
                MsgBox("Touch Range Should be in 0 to 112", MsgBoxStyle.Information, "Message")
                txtDTouch_Amt.SelectAll()
                Exit Sub
            End If
            'If Val(txtDWastPer.Text) <> 0 Then 'And Val(txtWastperPie.Text) <> 0 Then
            '    MsgBox("Wastage combination more than one,It may be conflict", MsgBoxStyle.Information)
            '    Exit Sub
            'End If
            If Val(txtFromWt.Text) > Val(txtTowt.Text) Then
                MsgBox("To Weight in Wt range is less than from weightt", MsgBoxStyle.Information, "Message")
                txtTowt.SelectAll()
                txtTowt.Focus()
                Exit Sub
            End If
            If True Then
            Else
                If Val(txtFromWt.Text) = 0 Or Val(txtTowt.Text) = 0 Then
                    MsgBox("From Weight & To Weight in Wt range should not be zero", MsgBoxStyle.Information, "Message")
                    txtFromWt.SelectAll()
                    txtFromWt.Focus()
                    Exit Sub
                End If
            End If
            If Val(txtDTouch_Amt.Text) <> Val(txtDPurePer_PER.Text) + Val(txtDWastPer.Text) + Val(txtDmcPer_PER.Text) Then
                If MsgBox("Touch per mismatch", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, "Message") = MsgBoxResult.No Then
                    txtDTouch_Amt.SelectAll()
                    txtDTouch_Amt.Focus()
                    Exit Sub
                End If
            End If

            strSql = "select max(sno) from " & cnAdminDb & "..DEALER_WMCTABLE"
            If Not flagSave Then txtSno.Text = Val(objGPack.GetSqlValue(strSql).ToString) + 1


            strSql = " Declare @wtFrom as float,@wtTo as Float"
            strSql += vbCrLf + " Set @wtFrom = " & Val(txtFromWt.Text) & ""
            strSql += vbCrLf + " Set @wtTo = " & Val(txtTowt.Text) & ""

            strSql += vbCrLf + " SELECT 1 FROM " & cnAdminDb & "..DEALER_WMCTABLE"
            strSql += vbCrLf + " WHERE ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbPartyName.Text & "')"
            If cmbDItem.Text <> "" Then strSql += vbCrLf + " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbDItem.Text & "')"
            If cmbDSubItem.Text <> "" Then
                strSql += vbCrLf + " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbDSubItem.Text & "' AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbDItem.Text & "') ),0)"
            End If
            If flagSave Then
                strSql += vbCrLf + " AND SNO <> " & Val(txtSno.Text) & ""
                strSql += vbCrLf + " and touch = '" & Val(txtDTouch_Amt.Text) & "'"
            Else
                strSql += vbCrLf + " and ((from_Wt between @wtFrom and @wtTo) OR (To_Wt between @wtFrom and @wtTo))"
                strSql += vbCrLf + " and touch = '" & Val(txtDTouch_Amt.Text) & "'"
            End If
            Dim dtTemp As New DataTable
            dtTemp.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTemp)
            If dtTemp.Rows.Count > 0 Then
                MsgBox("Touch/Mc/Wastage Already Exist in this Party", MsgBoxStyle.Information)
                If Not cmbDSubItem.Items.Count > 0 Then
                    cmbDSubItem.Focus()
                Else
                    cmbDItem.Focus()
                End If
                Exit Sub
            End If
            'Try
            Dim accode As String = Nothing
            Dim itemId As Integer = Nothing
            Dim subItemId As Integer = Nothing
            strSql = " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbPartyName.Text & "'"
            dtTemp.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTemp)
            If dtTemp.Rows.Count > 0 Then
                accode = dtTemp.Rows(0).Item("ACCODE").ToString
            End If

            strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbDItem.Text & "'"
            dtTemp.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTemp)
            If dtTemp.Rows.Count > 0 Then
                itemId = Val(dtTemp.Rows(0).Item("ITEMID").ToString)
            End If

            strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbDSubItem.Text & "' AND ITEMID = '" & itemId & "'"
            dtTemp.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTemp)
            If dtTemp.Rows.Count > 0 Then
                subItemId = Val(dtTemp.Rows(0).Item("SUBITEMID").ToString)
            End If
            '***************************************************

            Dim SItemId As String = ""
            strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbx_Oitmname.Text & "'"
            SItemId = Val(objGPack.GetSqlValue(strSql).ToString)
            If SItemId > 0 Then
                strSql = " SELECT SUBITEMNAME,SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE itemid='" & SItemId & "' AND SUBITEMNAME IN (" & GetQryString(chkcmbx_Ositemname.Text, ",") & ") "
                dt = New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    sid = ""
                    For i As Integer = 0 To dt.Rows.Count - 1
                        sid += dt.Rows(i).Item("SUBITEMID").ToString
                        If i <> dt.Rows.Count - 1 Then
                            sid += ","
                        End If
                    Next
                End If

            End If
            '***********************************************
            If flagSave = False Then
                funcAdd(accode, itemId, subItemId, sid)
            Else
                funcUpdate(accode, itemId, subItemId, sid)
            End If
            'btnNew_Click(Me, New EventArgs)
            objGPack.TextClear(Me)
            flagSave = False
            txtSno.Text = ""
            chkcmbx_Ositemname.Text = ""
            If rbtDealer.Checked Then
                rbtDealer.Focus()
            Else
                rbtSmith.Focus()
            End If
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        Try
            tabMain.SelectedTab = tabView
            If rbtDealer.Checked = True Then
                rbtOpenCustomer.Checked = True
                rbtOpenCustomer.Focus()
            ElseIf rbtSmith.Checked = True Then
                rbtOpenSmith.Checked = True
                rbtOpenSmith.Focus()
            End If
            btnOpenView_Click(Me, New EventArgs)

        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Try
            objGPack.TextClear(Me)
            flagSave = False
            FLAG = 0
            cmbPartyName.Text = ""
            cmbDItem.Text = ""
            cmbDSubItem.Text = ""
            txtDItemid_NUM.Text = ""
            rbtDealer.Focus()
            txtbxOitmid_NUM.Text = ""
            txtbxOsitmid.Text = ""
            cmbx_Oitmname.Text = ""
            chkcmbx_Ositemname.Items.Clear()
            txtbx_Owstge.Text = ""
            txtbx_OwstgePER.Text = ""
            txtbx_Omc.Text = ""
            txtbx_Omcgm.Text = ""
            txtbx_Omcp.Text = ""
            chkcmbosubitemnload("")
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Private Sub btnOpenView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenView.Click
        Try
            'strSql = " SELECT SNO,H.ACNAME,P.ITEMNAME,SP.SUBITEMNAME AS SUBITEM,TOUCH,ALLOY,MC,MCGRM,WASTPER,WAST,WASTPIE,FROM_WT,TO_WT"
            'strSql += " ,MCPER,PUREWT"
            'strSql += " FROM " & cnAdminDb & "..DEALER_WMCTABLE AS W "
            'strSql += " LEFT JOIN  " & cnAdminDb & "..ACHEAD AS H ON  H.ACCODE = W.ACCODE"
            'strSql += " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS P ON  P.ITEMID = W.ITEMID"
            'strSql += " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SP ON  SP.SUBITEMID = W.SUBITEMID"

            strSql = " SELECT  SNO ,S_ID"
            strSql += vbCrLf + ",(SELECT A.ACNAME FROM " & cnAdminDb & "..ACHEAD AS A WHERE A.ACCODE=W.ACCODE) ACNAME "
            strSql += vbCrLf + ",(SELECT Q.ITEMNAME  FROM " & cnAdminDb & "..ITEMMAST AS Q WHERE  Q.ITEMID =W.D_ITEMID  ) DITEMANAME "
            strSql += vbCrLf + ",(SELECT SQ.SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS SQ WHERE SQ.SUBITEMID=W.D_SUBITEMID ) DSUBITEMNAME "
            strSql += vbCrLf + ",(SELECT R.ITEMNAME  FROM " & cnAdminDb & "..ITEMMAST AS R WHERE  R.ITEMID =W.O_ITEMID  ) OITEMNAME "
            strSql += vbCrLf + ",W.O_SUBITEMID OSUBITEMID "
            strSql += vbCrLf + ",CONVERT(VARCHAR(8000),NULL) OSUBITEMNAME "
            ''strSql += vbCrLf + ",(SELECT SR.SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS SR WHERE SR.SUBITEMID=W.O_SUBITEMID ) OSUBITEM "
            ''strSql += vbCrLf + ",(SELECT SR.SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST AS SR WHERE SR.SUBITEMID=W.O_SUBITEMID ) OSUBITEM "
            strSql += vbCrLf + ",D_TOUCH,ALLOY AS DALLOY,D_MC  "
            strSql += vbCrLf + " ,D_MCGRAM,D_WAST,D_PURE,FROM_WT,TO_WT ,MCPER,PUREWT "
            strSql += vbCrLf + ",WASTPER AS OWASTPER, WAST AS OWAST "
            strSql += vbCrLf + ",MCGRM AS OMCGRAM, MC AS OMC, MCPER AS OMCPER "
            strSql += vbCrLf + "FROM " & cnAdminDb & "..DEALER_WMCTABLE AS W  "
            If rbtOpenCustomer.Checked Then '
                strSql += vbCrLf + " WHERE w.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE='C') "
            ElseIf rbtOpenDealer.Checked Then
                strSql += vbCrLf + " WHERE w.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE IN ('D')) "
            Else
                strSql += vbCrLf + " WHERE w.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE IN ('G')) "
            End If
            If UCase(cmbOpenParty.Text) <> "ALL" And cmbOpenParty.Text <> "" Then
                strSql += vbCrLf + " AND w.ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbOpenParty.Text & "')"
            End If
            If UCase(cmbOpenItem.Text) <> "ALL" And cmbOpenItem.Text <> "" Then
                strSql += vbCrLf + " AND w.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbOpenItem.Text & "')"
            End If
            If UCase(cmbOpenSubItem.Text) <> "ALL" And cmbOpenSubItem.Text <> "" Then
                strSql += vbCrLf + " AND w.SUBITEMID IN (ISNULL((SELECT TOP 1 SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbOpenSubItem.Text & "'),0))"
            End If
            dtGridView.Rows.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGridView)
            Dim SNAME As String = Nothing
            'Dim aresult As String = ""
            For i As Integer = 0 To dtGridView.Rows.Count - 1
                Dim aresult As String = ""
                If dtGridView.Rows(i).Item("S_ID").ToString <> "" Then
                    Dim a() As String = dtGridView.Rows(i).Item("S_ID").ToString.Split(",")
                    If a.Length > 0 Then
                        For j As Integer = 0 To a.Length - 1
                            If dtGridView.Rows.Count > 0 Then
                                strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID='" & a(j).ToString & "' "
                                dt = New DataTable
                                da = New OleDbDataAdapter(strSql, cn)
                                da.Fill(dt)
                                If dt.Rows.Count > 0 Then
                                    SNAME = ""
                                    For k As Integer = 0 To dt.Rows.Count - 1
                                        SNAME += dt.Rows(k).Item("SUBITEMNAME").ToString
                                        If i <> dt.Rows.Count - 1 Then
                                            SNAME += ","
                                        End If
                                    Next
                                End If
                                '  aresult += SNAME
                            End If
                            aresult += SNAME
                        Next
                    End If
                End If
                dtGridView.Rows(i).Item("OSUBITEMNAME") = aresult
            Next
            If dtGridView.Rows.Count > 0 Then
                gridView.Focus()
            Else
                MsgBox("Record Not Found", MsgBoxStyle.Information)
                rbtOpenCustomer.Focus()
            End If
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Private Sub rbtDealer_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtDealer.CheckedChanged
        Try
            If rbtDealer.Checked Then
                strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE IN('D','G') ORDER BY ACNAME"
                objGPack.FillCombo(strSql, cmbPartyName, , False)
                rbtDealer.Focus()
            End If
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Private Sub rbtSmith_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtSmith.CheckedChanged
        Try
            If rbtSmith.Checked Then
                If flagSave = False Then
                    cmbPartyName.Text = ""
                End If
                strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE in ('G') ORDER BY ACNAME"
                objGPack.FillCombo(strSql, cmbPartyName, , False)
                rbtSmith.Focus()
            End If
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Private Sub rbtOpenCustomer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtOpenCustomer.CheckedChanged
        Try
            If rbtOpenCustomer.Checked Then
                cmbOpenParty.Text = ""
                strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE ='C' ORDER BY ACNAME"
                objGPack.FillCombo(strSql, cmbOpenParty, , False)
            End If
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Private Sub rbtOpenSmith_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtOpenSmith.CheckedChanged
        Try
            If rbtOpenSmith.Checked Then
                cmbOpenParty.Text = ""
                strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE in ('G')"
                objGPack.FillCombo(strSql, cmbOpenParty, , False)
            End If
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Private Sub rbtCust_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtCust.CheckedChanged
        Try
            If rbtCust.Checked = True Then
                strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE = 'C'"
                objGPack.FillCombo(strSql, cmbPartyName, , False)

                rbtCust.Focus()
            End If
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Private Sub rbtOpenDealer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtOpenDealer.CheckedChanged
        Try
            If rbtOpenDealer.Checked = True Then
                strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE = 'D'"
                objGPack.FillCombo(strSql, cmbOpenParty, , False)
                rbtOpenDealer.Focus()
            End If
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
#End Region

#Region "Textbox and Combobx Events"
    Private Sub cmbPartyName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbPartyName.KeyPress
        e.KeyChar = UCase(e.KeyChar)
        If e.KeyChar = Chr(Keys.Enter) Then
            If funcComboChecker("Party Name", cmbPartyName) Then
                Exit Sub
            End If
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub cmbItem_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbDItem.KeyPress
        Try
            e.KeyChar = UCase(e.KeyChar)
            If e.KeyChar = Chr(Keys.Enter) Then
                If funcComboChecker("Item", cmbDItem) Then
                    Exit Sub
                End If
                strSql = " SELECT 1 FROM " & cnAdminDb & "..DEALER_WMCTABLE"
                strSql += " WHERE ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbPartyName.Text & "')"
                strSql += " AND ITEMID = (SELECT ITEMID FROM  " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbDItem.Text & "')"
                strSql += " AND SUBITEMID = 0"
                If flagSave Then
                    strSql += " AND SNO <> " & Val(txtSno.Text) & ""
                End If
                Dim dtTemp As New DataTable
                dtTemp.Clear()
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtTemp)
                If dtTemp.Rows.Count > 0 Then
                    MsgBox("Touch/Mc/Wastage Already Exist in this Party", MsgBoxStyle.Information)
                    cmbDItem.Focus()
                    Exit Sub
                End If
                SendKeys.Send("{TAB}")
            End If
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Private Sub cmbItem_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbDItem.LostFocus
        Try
            strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE"
            strSql += " ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME"
            strSql += " = '" & cmbDItem.Text & "') ORDER BY SUBITEMNAME"
            objGPack.FillCombo(strSql, cmbDSubItem, , False)
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Private Sub cmbSubItem_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbDSubItem.GotFocus
        If Not cmbDSubItem.Items.Count > 0 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub cmbSubItem_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbDSubItem.KeyPress
        Try
            e.KeyChar = UCase(e.KeyChar)
            If e.KeyChar = Chr(Keys.Enter) Then
                If funcComboChecker("SubItem", cmbDSubItem) Then
                    Exit Sub
                End If
                strSql = " Declare @wtFrom as float,@wtTo as Float"
                strSql += " Set @wtFrom = " & Val(txtFromWt.Text) & ""
                strSql += " Set @wtTo = " & Val(txtTowt.Text) & ""

                strSql += " SELECT 1 FROM " & cnAdminDb & "..DEALER_WMCTABLE"
                strSql += " WHERE ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbPartyName.Text & "')"
                strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbDItem.Text & "')"
                strSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE  SUBITEMNAME = '" & cmbDSubItem.Text & "' AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbDItem.Text & "')),0)"
                If flagSave Then
                    strSql += " AND SNO <> " & Val(txtSno.Text) & ""
                Else
                    strSql += " and ((from_Wt between @wtFrom and @wtTo) OR (To_Wt between @wtFrom and @wtTo))"
                End If
                Dim dtTemp As New DataTable
                dtTemp.Clear()
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtTemp)
                If dtTemp.Rows.Count > 0 Then
                    MsgBox("Touch/Mc/Wastage Already Exist in this Party", MsgBoxStyle.Information)
                    cmbDSubItem.Focus()
                    Exit Sub
                End If
                SendKeys.Send("{TAB}")
            End If
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Private Sub cmbOpenItem_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbOpenItem.LostFocus
        Try
            strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE"
            strSql += " ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME"
            strSql += " = '" & cmbOpenItem.Text & "') ORDER BY SUBITEMNAME"
            cmbOpenSubItem.Items.Clear()
            cmbOpenSubItem.Items.Add("ALL")
            cmbOpenSubItem.Text = "ALL"
            objGPack.FillCombo(strSql, cmbOpenSubItem, , False)
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Private Sub cmbOpenSubItem_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbOpenSubItem.GotFocus
        If Not cmbOpenSubItem.Items.Count > 0 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub txtTouch_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtDTouch_Amt.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub cmbItem_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbDItem.SelectedIndexChanged
        cmbDSubItem.Text = ""
    End Sub
    Private Sub cmbSubItem_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDSubItem.SelectedIndexChanged
        If cmbDSubItem.SelectedValue Is Nothing Then
            Exit Sub
        End If
        txtDsubItemId_NUM.Text = cmbDSubItem.SelectedValue.ToString
    End Sub

    Private Sub optMcPie_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optMcPie.CheckedChanged
        If optMcPie.Checked Then lblMcpergm.Text = "M-Charge/Pie"
    End Sub
    Private Sub optMcgm_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optMcgm.CheckedChanged
        If optMcgm.Checked Then lblMcpergm.Text = "M-Charge/Gm"
    End Sub
    Private Sub txtItemid_NUM_KeyDown(sender As Object, e As KeyEventArgs) Handles txtDItemid_NUM.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Val(txtDItemid_NUM.Text) > 0 Then
                strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = '" & Val(txtDItemid_NUM.Text) & "'"
                cmbDItem.Text = GetSqlValue(cn, strSql).ToString()
            End If
        End If

    End Sub
    Private Sub txtOpenItemId_KeyDown(sender As Object, e As KeyEventArgs) Handles txtOpenItemId.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Val(txtOpenItemId.Text) > 0 Then
                strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = '" & Val(txtOpenItemId.Text) & "'"
                cmbOpenItem.Text = GetSqlValue(cn, strSql).ToString()
            End If
        End If
    End Sub
    'NEW    '
    Private Sub txtbx_Oitmid_NUM_KeyDown(sender As Object, e As KeyEventArgs) Handles txtbxOitmid_NUM.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Val(txtbxOitmid_NUM.Text) > 0 Then
                strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = '" & Val(txtbxOitmid_NUM.Text) & "'"
                cmbx_Oitmname.Text = GetSqlValue(cn, strSql).ToString()
            End If
        End If
    End Sub
    Function Cmbobxitemnameload()
        Try
            strSql = "SELECT ITEMNAME,ITEMID FROM " & cnAdminDb & "..ITEMMAST"
            objGPack.FillCombo(strSql, cmbx_Oitmname, , False)
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Function
    Private Sub cmbx_Oitmname_KeyPress(sender As Object, e As KeyPressEventArgs) Handles cmbx_Oitmname.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbx_Oitmname.Text.Trim = "" Then
            Else
                chkcmbosubitemnload(cmbx_Oitmname.Text)
            End If
        End If
    End Sub
    Private Sub cmbDItem_KeyDown(sender As Object, e As KeyEventArgs) Handles cmbDItem.KeyDown
        If e.KeyCode = Keys.Enter Then
            If cmbDItem.Text.Trim = "" Then
            Else
                strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbDItem.Text.Trim & "'"
                txtDItemid_NUM.Text = GetSqlValue(cn, strSql).ToString()
            End If
        End If
    End Sub
    Private Sub cmbx_Oitmname_KeyDown(sender As Object, e As KeyEventArgs) Handles cmbx_Oitmname.KeyDown
        If e.KeyCode = Keys.Enter Then
            If cmbx_Oitmname.Text.Trim = "" Then
            Else
                strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbx_Oitmname.Text.Trim & "'"
                txtbxOitmid_NUM.Text = GetSqlValue(cn, strSql).ToString()
            End If
        End If
    End Sub

#End Region
#Region "Grid function"
    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        Try
            If e.KeyCode = Keys.Enter Then
                e.Handled = True
                If gridView.RowCount > 0 Then

                    strSql = " SELECT  SNO,S_ID "
                    strSql += vbCrLf + ",(SELECT A.ACNAME FROM " & cnAdminDb & "..ACHEAD AS A WHERE A.ACCODE=W.ACCODE) ACNAME "
                    strSql += vbCrLf + ",(SELECT Q.ITEMNAME  FROM " & cnAdminDb & "..ITEMMAST AS Q WHERE  Q.ITEMID =W.D_ITEMID  ) DITEMANAME "
                    strSql += vbCrLf + ",(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS SQ WHERE SQ.SUBITEMID=W.D_SUBITEMID ) DSUBITEMNAME "
                    strSql += vbCrLf + ",(SELECT R.ITEMNAME  FROM " & cnAdminDb & "..ITEMMAST AS R WHERE  R.ITEMID =W.O_ITEMID  ) OITEMNAME "
                    strSql += vbCrLf + ",W.O_SUBITEMID OSUBITEMID "
                    strSql += vbCrLf + ",CONVERT(VARCHAR(8000),NULL) OSUBITEMNAME "
                    'strSql += vbCrLf + ",(SELECT SR.SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS SR WHERE SR.SUBITEMID=W.O_SUBITEMID ) OSUBITEMNAME "
                    strSql += vbCrLf + ",D_TOUCH,ALLOY AS DALLOY,D_MC  "
                    strSql += vbCrLf + " ,D_MCGRAM,D_WAST,D_PURE,DGRSNET "
                    strSql += vbCrLf + ",FROM_WT,TO_WT ,MCPER,PUREWT ,CALCMODE"
                    strSql += vbCrLf + ",WASTPER AS OWASTPER, WAST AS OWAST "
                    strSql += vbCrLf + ",OGRSNET "
                    strSql += vbCrLf + ",MCGRM AS OMCGRAM, MC AS OMC, MCPER AS OMCPER ,W.GRSNET,O_ITEMID,D_ITEMID"
                    strSql += vbCrLf + "FROM " & cnAdminDb & "..DEALER_WMCTABLE AS W  "
                    strSql += vbCrLf + " WHERE SNO = " & Val(gridView.Rows(gridView.CurrentRow.Index).Cells("SNO").Value.ToString) & ""

                    da = New OleDbDataAdapter(strSql, cn)
                    dtTemp = New DataTable
                    da.Fill(dtTemp)

                    Dim SNAME As String = Nothing
                    'Dim aresult As String = ""
                    For i As Integer = 0 To dtTemp.Rows.Count - 1
                        Dim aresult As String = ""
                        If dtTemp.Rows(i).Item("S_ID").ToString <> "" Then
                            Dim a() As String = dtTemp.Rows(i).Item("S_ID").ToString.Split(",")
                            If a.Length > 0 Then
                                For j As Integer = 0 To a.Length - 1
                                    If dtTemp.Rows.Count > 0 Then
                                        strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID='" & a(j).ToString & "' "
                                        dt = New DataTable
                                        da = New OleDbDataAdapter(strSql, cn)
                                        da.Fill(dt)
                                        If dt.Rows.Count > 0 Then
                                            SNAME = ""
                                            For k As Integer = 0 To dt.Rows.Count - 1
                                                SNAME += dt.Rows(k).Item("SUBITEMNAME").ToString
                                                If i <> dt.Rows.Count Then
                                                    SNAME += ","
                                                End If
                                            Next
                                        End If
                                        'aresult += SNAME
                                    End If
                                    aresult += SNAME
                                Next
                            End If
                        End If
                        dtTemp.Rows(i).Item("OSUBITEMNAME") = aresult
                    Next


                    If dtTemp.Rows.Count > 0 Then
                        With dtTemp.Rows(0)

                            cmbPartyName.Text = .Item("ACNAME").ToString
                            cmbDItem.Text = .Item("DITEMANAME").ToString
                            cmbDSubItem.Text = .Item("DSUBITEMNAME").ToString
                            txtDTouch_Amt.Text = .Item("D_TOUCH").ToString
                            txtAlloy_Amt.Text = .Item("DALLOY").ToString
                            txtDWastPer.Text = .Item("D_WAST").ToString
                            'txtWastgm.Text = .Item("WAST").ToString
                            txtWastperPie.Text = .Item("D_PURE").ToString
                            txtDMcgm.Text = .Item("D_MCGRAM").ToString
                            txtMcPie.Text = .Item("D_MC").ToString
                            txtSno.Text = .Item("SNO").ToString
                            'cmbOpenItem.Text = .Item("OITEMNAME").ToString
                            'cmbOpenSubItem.Text = .Item("OSUBITEM").ToString   
                            cmbx_Oitmname.Text = .Item("OITEMNAME").ToString
                            'cmbx_Ositemname.Text = .Item("OSUBITEM").ToString
                            'chkcmbx_Ositemname.Text = .Item("OSUBITEMNAME").ToString   
                            Dim Subname As New List(Of String)
                            'Dim Subname() As String
                            'Dim dtsname As New DataTable
                            'dtsname = New DataTable
                            For Each s As String In .Item("OSUBITEMNAME").ToString().Split(",")
                                Subname.Add(s.Replace("'", ""))
                            Next
                            If .Item("OSUBITEMNAME").ToString() = "" Then
                                chkcmbx_Ositemname.SetItemChecked(0, True)
                            Else
                                chkcmbosubitemnload(cmbx_Oitmname.Text)
                                If Subname.Count > 0 Then
                                    For i As Integer = 0 To Subname.Count - 1
                                        For j As Integer = 0 To chkcmbx_Ositemname.Items.Count - 1
                                            If chkcmbx_Ositemname.Items(j).ToString = Subname(i).ToString Then
                                                chkcmbx_Ositemname.SetItemChecked(j, True)
                                            End If
                                        Next
                                    Next
                                End If
                                'For cnt As Integer = 0 To dtTemp.Rows.Count - 1
                                '    If Subname.Contains(dtTemp.Rows(cnt).Item("OSUBITEMNAME").ToString) Then
                                '        chkcmbx_Ositemname.SetItemChecked(cnt, True)
                                '    Else
                                '        chkcmbx_Ositemname.SetItemChecked(cnt, False)
                                '    End If
                                'Next
                            End If
                            txtbxOitmid_NUM.Text = .Item("O_ITEMID").ToString
                            txtDItemid_NUM.Text = .Item("D_ITEMID").ToString
                            txtbx_OwstgePER.Text = .Item("OWASTPER").ToString
                            txtbx_Owstge.Text = .Item("OWAST").ToString
                            txtbx_Omc.Text = .Item("OMC").ToString
                            txtbx_Omcgm.Text = .Item("OMCGRAM").ToString
                            txtbx_Omcp.Text = .Item("OMCPER").ToString
                            If .Item("DGRSNET").ToString = "G" Then Me.optDGrs.Checked = True
                            If .Item("OGRSNET").ToString = "G" Then Me.opt_Ogrswt.Checked = True
                            If Val(.Item("FROM_WT").ToString) <> 0 Then txtFromWt.Text = .Item("FROM_WT").ToString
                            If Val(.Item("TO_WT").ToString) <> 0 Then txtTowt.Text = .Item("TO_WT").ToString
                            If .Item("DGRSNET").ToString = "N" Then Me.optDNet.Checked = True
                            If .Item("OGRSNET").ToString = "N" Then Me.opt_Ontwt.Checked = True
                            If .Item("CALCMODE").ToString = "T" Then Me.optTouch.Checked = True
                            If .Item("CALCMODE").ToString = "W" Then Me.OptWast.Checked = True
                            'If .Item("MCCALC").ToString = "W" Then Me.optMcgm.Checked = True
                            'If .Item("MCCALC").ToString = "P" Then Me.optMcPie.Checked = True
                            If Val(.Item("MCPER").ToString) <> 0 Then txtDmcPer_PER.Text = .Item("D_MC").ToString
                            If Val(.Item("PUREWT").ToString) <> 0 Then txtDPurePer_PER.Text = .Item("D_PURE").ToString
                            flagSave = True
                            tabMain.SelectedTab = tabGeneral
                            If Val(objGPack.GetSqlValue("SELECT 1 CUS FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & .Item("ACNAME").ToString & "' AND ACTYPE='D'")) > 0 Then
                                rbtDealer.Focus()
                            Else
                                rbtSmith.Focus()
                            End If
                        End With
                    End If
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
#End Region


End Class