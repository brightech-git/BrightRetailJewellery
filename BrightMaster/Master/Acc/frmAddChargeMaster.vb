Imports System.Data.OleDb
Public Class frmAddChargeMaster
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim flagSave As Boolean = False
    Dim dtAcHead As New DataTable
    Dim cnt As Integer
    Dim tempAcCode As Integer = Nothing

        Private Sub frmAccEntryMaster_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                btnBack_Click(Me, New EventArgs)
            End If
        End If
    End Sub
    Private Sub frmAccEntryMaster_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView.Focused Then Exit Sub
                SendKeys.Send("{TAB}")
        End If
    End Sub

    'Private Sub frmAccEntryMaster_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    cmbChargModule.Items.Add("ACCOUNTS")

    '    cmbChargModule.SelectedIndex = 0
    '    'cmbType.Text = "RECEIPT"

    '    cmbActive.Items.Add("YES")
    '    cmbActive.Items.Add("NO")
    '    cmbActive.Text = "YES"
    '    'txtName_MAN.CharacterCasing = CharacterCasing.Upper

    '    tabMain.ItemSize = New System.Drawing.Size(1, 1)
    '    Me.tabMain.Region = New Region(New RectangleF(Me.CmbAccHead.Left, Me.CmbAccHead.Top, Me.CmbAccHead.Width, Me.CmbAccHead.Height))

    '    btnNew_Click(Me, New EventArgs)
    '    txtName_MAN.Focus()

    'End Sub
    Function funcLoadGroup()
        dtAcHead.Clear()
        CmbAccountHead.Items.Clear()
        strSql = "select ACNAME from " & cnAdminDb & "..ACHEAD order by ACNAME"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtAcHead)
        If dtAcHead.Rows.Count > 0 Then
            For cnt = 0 To dtAcHead.Rows.Count - 1
                CmbAccountHead.Items.Add(dtAcHead.Rows(cnt).Item("ACNAME").ToString)
            Next
        End If
        Return 0
    End Function

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        objGPack.TextClear(Me)
        EnableAutoGenerator(True)
        cmbChargModule.SelectedIndex = 0
        txtSno.Text = ""
        cmbActive.Text = "YES"
        txtDisplayOrder.Text = ""
        flagSave = False
        txtName_MAN.Focus()
        funcLoadGroup()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
    Function funcAdd()
        Dim dt As New DataTable
        dt.Clear()
        Dim ds As New Data.DataSet
        ds.Clear()
        Dim designerId As Integer = Nothing
        Dim AcCode As String = Nothing
        Dim tran As OleDbTransaction = Nothing


        If txtName_MAN.Text = "" Then
            MsgBox("Charges Name Should Not Empty", MsgBoxStyle.Information)
            txtName_MAN.Focus()
            Exit Function
        End If

        strSql = " SELECT * FROM  " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & CmbAccountHead.Text & "' "
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtchk As New DataTable
        da.Fill(dtchk)

        If dtchk.Rows.Count = 0 Then
            strSql = " SELECT CONVERT(INT,MAX(CTLTEXT))AS ACCODE FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'ACCODE'"
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(ds, "AcCode")
            tempAcCode = ds.Tables("AcCode").Rows(0).Item("AcCode").ToString
ACCODE_GEN: tempAcCode += 1
            AcCode = funcSetNumberStyle(tempAcCode.ToString, 7)

            If objGPack.DupCheck("SELECT 1 FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & AcCode & "'", tran) Then
                GoTo ACCODE_GEN
            End If

            strSql = " Update " & cnAdminDb & "..SoftControl "
            strSql += " Set CtlText = '" & tempAcCode & "' "
            strSql += " where CtlId = 'ACCODE' "
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()

            strSql = " INSERT INTO " & cnAdminDb & "..ACHEAD("
            strSql += " ACCODE,ACNAME,ACGRPCODE,ACSUBGRPCODE"
            strSql += " )VALUES("
            strSql += " '" & AcCode & "','" & CmbAccountHead.Text & "'"
            strSql += " ,'11','0')"

            ExecQuery(SyncMode.Master, strSql, cn, tran)
        End If


        Dim dt1 As New DataTable
        dt1.Clear()
        strSql = " Select ACCODE from " & cnAdminDb & "..ACHEAD "
        strSql += " Where ACNAME = '" & CmbAccountHead.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            AcCode = dt1.Rows(0).Item("ACCODE").ToString
        End If

        Try
            Dim ChargeId As Integer = Nothing
            tran = Nothing
            tran = cn.BeginTransaction
            'If txtSno.Text = "" Then

            strSql = " SELECT ISNULL(MAX(CHARGEID),0)+1 AS CHARGEID FROM "
            strSql += " " & cnAdminDb & "..ADDCHARGE"
            ChargeId = Val(objGPack.GetSqlValue(strSql, , , tran))

            'AcCode = funcFindAccCode(CmbAccountHead.Text, tran)

            strSql = "  INSERT INTO " & cnAdminDb & "..ADDCHARGE"
            strSql += "  (CHARGEID,CHARGENAME,DISPLAYORDER,CHARGEMODULE,ACCODE,ACTIVE"
            strSql += " )"
            strSql += "  VALUES"
            strSql += "  ("
            strSql += " '" & ChargeId & "'" 'SNO
            strSql += " ,'" & txtName_MAN.Text & "'" 'CAPTION
            strSql += " ," & Val(txtDisplayOrder.Text) & "" 'DISPLAYORDER
            strSql += " ,'" & Mid(cmbChargModule.Text, 1, 1) & "'" 'TYPE
            strSql += " ,'" & AcCode & "'" 'PAYMODE
            strSql += " ,'" & Mid(cmbActive.Text, 1, 1) & "'" 'ACTIVE
            'strSql += " ,'" & txtDisplayText.Text & "'" 'DISPLAYTEXT
            'strSql += " ,'" & cmbDrCaption.Text & "'" 'DrCaption
            'strSql += " ,'" & cmbCrCaption.Text & "'" 'CrCaption
            strSql += ")"
            ExecQuery(SyncMode.Master, strSql, cn, tran)
            tran.Commit()
            MsgBox("Saved Successfully..", MsgBoxStyle.Information)
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
        Return 0
    End Function
    Function funcUpdate() As Integer

        Dim ds As New Data.DataSet
        ds.Clear()
        Dim designerId As Integer = Nothing
        Dim AcCode As String = Nothing
        Dim tran As OleDbTransaction = Nothing

        strSql = " SELECT * FROM  " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & CmbAccountHead.Text & "' "
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtchk As New DataTable
        da.Fill(dtchk)

        If dtchk.Rows.Count = 0 Then
            strSql = " SELECT CONVERT(INT,MAX(CTLTEXT))AS ACCODE FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'ACCODE'"
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(ds, "AcCode")
            tempAcCode = ds.Tables("AcCode").Rows(0).Item("AcCode").ToString
ACCODE_GEN: tempAcCode += 1
            AcCode = funcSetNumberStyle(tempAcCode.ToString, 7)

            If objGPack.DupCheck("SELECT 1 FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & AcCode & "'", tran) Then
                GoTo ACCODE_GEN
            End If

            strSql = " Update " & cnAdminDb & "..SoftControl "
            strSql += " Set CtlText = '" & tempAcCode & "' "
            strSql += " where CtlId = 'ACCODE' "
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()

            strSql = " INSERT INTO " & cnAdminDb & "..ACHEAD("
            strSql += " ACCODE,ACNAME,ACGRPCODE,ACSUBGRPCODE"
            strSql += " )VALUES("
            strSql += " '" & AcCode & "','" & CmbAccountHead.Text & "'"
            strSql += " ,'11','0')"

            ExecQuery(SyncMode.Master, strSql, cn, tran)
        End If


        Dim UpAcCode As String = Nothing
        Dim dt1 As New DataTable
        dt1.Clear()
        strSql = " Select ACCODE from " & cnAdminDb & "..ACHEAD "
        strSql += " Where ACNAME = '" & CmbAccountHead.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt1)
        If dt1.Rows.Count > 0 Then
            UpAcCode = dt1.Rows(0).Item("ACCODE").ToString
        End If

        'Dim tran As OleDbTransaction = Nothing
        Try
            tran = Nothing
            tran = cn.BeginTransaction

            strSql = " UPDATE " & cnAdminDb & "..ADDCHARGE"
            strSql += " SET CHARGENAME = '" & txtName_MAN.Text & "'"
            strSql += " ,CHARGEMODULE = '" & Mid(cmbChargModule.Text, 1, 1) & "'"
            strSql += " ,ACTIVE = '" & Mid(cmbActive.Text, 1, 1) & "'"
            strSql += " ,DISPLAYORDER = " & Val(txtDisplayOrder.Text) & ""
            strSql += " ,ACCODE = '" & UpAcCode & "' "
            strSql += " WHERE CHARGEID = '" & txtSno.Text & "'"
            ExecQuery(SyncMode.Master, strSql, cn, tran)
            tran.Commit()
            MsgBox("Updated Successfully..", MsgBoxStyle.Information)
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click

        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If objGPack.Validator_Check(Me) Then Exit Sub
        If flagSave = False Then
            funcAdd()
        ElseIf flagSave = True Then
            funcUpdate()
        End If

        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        'If objGPack.Validator_Check(Me) Then Exit Sub
        'If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..ACCENTRYMASTER WHERE SNO <> '" & txtSno.Text & "' AND CAPTION = '" & txtName_MAN.Text & "'").Length > 0 Then
        '    MsgBox("Name Already Exist")
        '    txtName_MAN.Focus()
        '    Exit Sub
        'End If
        ''If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..ACCENTRYMASTER WHERE SNO <> '" & txtSno.Text & "' AND PAYMODE = '" & txtPaymode_MAN.Text & "'").Length > 0 Then
        ''    MsgBox("Paymode Already Exist")
        ''    'txtPaymode_MAN.Focus()
        ''    Exit Sub
        ''End If

        'Try
        '    Dim ChargeId As Integer = Nothing
        '    tran = Nothing
        '    tran = cn.BeginTransaction
        '    'If txtSno.Text = "" Then

        '    strSql = " SELECT ISNULL(MAX(CHARGEID),0)+1 AS CHARGEID FROM "
        '    strSql += " " & cnAdminDb & "..ADDCHARGE"
        '    ChargeId = Val(objGPack.GetSqlValue(strSql, , , tran))


        '    strSql = "  INSERT INTO " & cnAdminDb & "..ACCENTRYMASTER"
        '    strSql += "  (SNO,CAPTION,TYPE,PAYMODE,ACTIVE,DISPLAYORDER,DISPLAYTEXT "
        '    strSql += " ,DRCAPTION,CRCAPTION"
        '    strSql += " )"
        '    strSql += "  VALUES"
        '    strSql += "  ("
        '    strSql += " '" & ChargeId & "'" 'SNO
        '    strSql += " ,'" & txtName_MAN.Text & "'" 'CAPTION
        '    strSql += " ,'" & Mid(cmbChargModule.Text, 1, 1) & "'" 'TYPE
        '    'strSql += " ,'" & txtPaymode_MAN.Text & "'" 'PAYMODE
        '    strSql += " ,'" & Mid(cmbActive.Text, 1, 1) & "'" 'ACTIVE
        '    strSql += " ," & Val(txtDisplayOrder.Text) & "" 'DISPLAYORDER
        '    'strSql += " ,'" & txtDisplayText.Text & "'" 'DISPLAYTEXT
        '    'strSql += " ,'" & cmbDrCaption.Text & "'" 'DrCaption
        '    'strSql += " ,'" & cmbCrCaption.Text & "'" 'CrCaption
        '    strSql += ")"
        '    ExecQuery(SyncMode.Master, strSql, cn, tran)
        '    'InsertIntoBillControl("GEN-" + txtPaymode_MAN.Text, txtName_MAN.Text + " ACCOUNTS ENTRY", "N", "N", "", "A", tran)
        '    'Else
        '    '    strSql = " UPDATE " & cnAdminDb & "..ACCENTRYMASTER"
        '    '    strSql += " SET CAPTION = '" & txtName_MAN.Text & "'"
        '    '    strSql += " ,TYPE = '" & Mid(cmbChargModule.Text, 1, 1) & "'"
        '    '    'strSql += " ,PAYMODE = '" & txtPaymode_MAN.Text & "'"
        '    '    strSql += " ,ACTIVE = '" & Mid(cmbActive.Text, 1, 1) & "'"
        '    '    strSql += " ,DISPLAYORDER = " & Val(txtDisplayOrder.Text) & ""
        '    '    'strSql += " ,DISPLAYTEXT = '" & txtDisplayText.Text & "'"
        '    '    'strSql += " ,DRCAPTION = '" & cmbDrCaption.Text & "'"
        '    '    'strSql += " ,CRCAPTION = '" & cmbCrCaption.Text & "'"
        '    '    strSql += " WHERE SNO = '" & txtSno.Text & "'"
        '    '    ExecQuery(SyncMode.Master, strSql, cn, tran)
        '    'End If
        '    tran.Commit()
        '    tran = Nothing
        '    Dim str As String = Nothing
        '    If txtSno.Text = "" Then str = "Saved Successfully.." Else str = "Updated Successfully.."
        '    str += vbCrLf + "You must restart your application for some of the change"
        '    str += vbCrLf + "made by menu creation to take effect."
        '    str += vbCrLf + "Do you want to restart your application now?"
        '    If MessageBox.Show(str, "Restart Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
        '        Application.Restart()
        '    End If
        '    btnNew_Click(Me, New EventArgs)
        'Catch ex As Exception
        '    If tran IsNot Nothing Then tran.Rollback()
        '    MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        'End Try
    End Sub
    Private Sub CallGrid()
        strSql = "  SELECT CHARGEID,CHARGENAME,DISPLAYORDER"
        strSql += " ,CASE WHEN CHARGEMODULE = 'A' THEN 'ACCOUNTS' ELSE '' END AS CHARGEMODULE"
        strSql += " ,ACNAME,CASE WHEN C.ACTIVE = 'Y' THEN 'YES' ELSE 'NO' END AS ACTIVE "
        strSql += " FROM " & cnAdminDb & "..ADDCHARGE AS C"
        strSql += " INNER JOIN " & cnAdminDb & "..ACHEAD AS H ON H.ACCODE = C.ACCODE"

        'strSql = " SELECT CHARGEID,CHARGENAME,DISPLAYORDER,CHARGEMODULE,ACCODE,CASE WHEN ACTIVE = 'Y' THEN 'YES' ELSE 'NO' END AS ACTIVE"
        'strSql += " FROM " & cnAdminDb & "..ADDCHARGE"
        Dim dtGrid As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        gridView.DataSource = dtGrid
        'gridView.Columns("AUTOGENERATOR").Visible = False
        'gridView.Columns("SNO").Visible = False
        'gridView.Columns("DISPLAYORDER").HeaderText = "ORDER"
        'gridView.Columns("DRCAPTION").HeaderText = "DR CAPTION"
        'gridView.Columns("CRCAPTION").HeaderText = "CR CAPTION"
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
    End Sub
    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click, OpenToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        tabView.Show()
        CallGrid()
        If Not gridView.RowCount > 0 Then
            MsgBox("Record not Found", MsgBoxStyle.Information)
            Exit Sub
        End If
        tabMain.SelectedTab = tabView
        gridView.Focus()
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = CmbAccHead
        txtName_MAN.Select()
    End Sub

    Private Sub txtName_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtName_MAN.KeyPress
        'If e.KeyChar = Chr(Keys.Enter) Then
        '    If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..ACCENTRYMASTER WHERE SNO <> '" & txtSno.Text & "' AND CAPTION = '" & txtName_MAN.Text & "'").Length > 0 Then
        '        MsgBox("Name Already Exist")
        '        txtName_MAN.Focus()
        '        Exit Sub
        '    End If
        'End If
        If e.KeyChar = Chr(Keys.Enter) Then
            CmbAccountHead.Items.Add(txtName_MAN.Text)
            'CmbAccountHead.Text = txtName_MAN.Text
        End If
    End Sub

    'Private Sub txtPaymode_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    '    If e.KeyChar = Chr(Keys.Enter) Then
    '        If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..ACCENTRYMASTER WHERE SNO <> '" & txtSno.Text & "' AND PAYMODE = '" & txtPaymode_MAN.Text & "'").Length > 0 Then
    '            MsgBox("Paymode Already Exist")
    '            txtPaymode_MAN.Focus()
    '            Exit Sub
    '        End If
    '    End If
    'End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblStatus.Visible = True
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub EnableAutoGenerator(ByVal status As Boolean)
        cmbChargModule.Enabled = status
        cmbActive.Enabled = status
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            e.Handled = True
            If Not gridView.RowCount > 0 Then Exit Sub
            gridView.CurrentCell = gridView.CurrentRow.Cells("CHARGEID")
            With gridView.CurrentRow
                txtSno.Text = .Cells("CHARGEID").Value.ToString
                txtName_MAN.Text = .Cells("CHARGENAME").Value.ToString
                cmbChargModule.Text = .Cells("CHARGEMODULE").Value.ToString
                CmbAccountHead.Text = .Cells("ACNAME").Value.ToString
                cmbActive.Text = .Cells("ACTIVE").Value.ToString
                txtDisplayOrder.Text = .Cells("DISPLAYORDER").Value.ToString
                flagSave = True
                btnBack_Click(Me, New EventArgs)
            End With
        End If
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        lblStatus.Visible = False
    End Sub
    'Private Sub cmbType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbChargModule.SelectedIndexChanged
    '    cmbDrCaption.Items.Clear()
    '    cmbCrCaption.Items.Clear()
    '    cmbDrCaption.Text = ""
    '    cmbCrCaption.Text = ""
    '    Select Case cmbChargModule.Text.ToUpper
    '        Case "RECEIPT"
    '            cmbDrCaption.Items.Add("Received To")
    '            cmbDrCaption.Items.Add("Dr")
    '            cmbDrCaption.Text = "Received To"
    '            cmbCrCaption.Items.Add("Received From")
    '            cmbCrCaption.Items.Add("Cr")
    '            cmbCrCaption.Text = "Received From"
    '        Case "PAYMENT"
    '            cmbDrCaption.Items.Add("Paid To")
    '            cmbDrCaption.Items.Add("Dr")
    '            cmbDrCaption.Text = "Paid To"
    '            cmbCrCaption.Items.Add("Paid From")
    '            cmbCrCaption.Items.Add("Cr")
    '            cmbCrCaption.Text = "Paid From"
    '        Case "JOURNAL", "DEBIT NOTE", "CREDIT NOTE"
    '            cmbDrCaption.Items.Add("Dr")
    '            cmbDrCaption.Text = "Dr"
    '            cmbCrCaption.Items.Add("Cr")
    '            cmbCrCaption.Text = "Cr"
    '    End Select
    'End Sub

   
   
    
    Private Sub txtName_MAN_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtName_MAN.LostFocus
        CmbAccountHead.Text = txtName_MAN.Text
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub frmAddChargeMaster_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        cmbChargModule.Items.Add("ACCOUNTS")

        cmbChargModule.SelectedIndex = 0
        'cmbType.Text = "RECEIPT"

        cmbActive.Items.Clear()
        cmbActive.Items.Add("YES")
        cmbActive.Items.Add("NO")
        cmbActive.Text = "YES"


        'txtName_MAN.CharacterCasing = CharacterCasing.Upper

        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.CmbAccHead.Left, Me.CmbAccHead.Top, Me.CmbAccHead.Width, Me.CmbAccHead.Height))
        btnNew_Click(Me, New EventArgs)
        txtName_MAN.Select()

    End Sub
End Class