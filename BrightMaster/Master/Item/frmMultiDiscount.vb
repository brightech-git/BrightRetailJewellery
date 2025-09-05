Imports System.Data.OleDb
Public Class frmMultiDiscount
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim flagUpdateId As String = Nothing

    Private Sub frmMultiDiscount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub LoadGrid()
        strSql = "SELECT DISCNAME,DISCAUTHCODE,DISCAUTHAMT,DISCID FROM " & cnAdminDb & "..MULTIDISCOUNT ORDER BY DISCNAME"
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtGrid As New DataTable
        da.Fill(dtGrid)
        gridView.DataSource = dtGrid
        gridView.Columns("DISCID").Visible = False
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub

    Private Sub txtDiscount_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtDiscount_MAN.KeyPress
        If txtDiscount_MAN.Text = "" Then Exit Sub
        If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..MULTIDISCOUNT WHERE DISCNAME = '" & txtDiscount_MAN.Text & "' AND DISCID <> '" & flagUpdateId & "'").Length > 0 Then
            MsgBox("Already exist", MsgBoxStyle.Information)
            txtDiscount_MAN.Focus()
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If objGPack.Validator_Check(Me) Then Exit Sub
        If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..MULTIDISCOUNT WHERE DISCNAME = '" & txtDiscount_MAN.Text & "' AND DISCID <> '" & flagUpdateId & "'").Length > 0 Then
            MsgBox("Already exist", MsgBoxStyle.Information)
            txtDiscount_MAN.Focus()
            Exit Sub
        End If
        If flagUpdateId <> Nothing Then 'UPDATE
            strSql = "UPDATE " & cnAdminDb & "..MULTIDISCOUNT"
            strSql += " SET DISCNAME = '" & txtDiscount_MAN.Text & "'"
            strSql += " , DISCAUTHCODE = '" & txtDiscAuthCode.Text & "'"
            strSql += " , DISCAUTHAMT = " & Val(txtDiscAuthAmt.Text) & ""
            strSql += " WHERE DISCID = '" & flagUpdateId & "'"
            ExecQuery(SyncMode.Master, strSql, cn)
            InsertIntoAcHead(flagUpdateId, txtDiscount_MAN.Text, "11")
            MsgBox("Successfully Updated")
            btnNew_Click(Me, New EventArgs)
            Exit Sub
        End If
        Dim cnt As Integer = objGPack.GetSqlValue("SELECT COUNT(*)+1 FROM " & cnAdminDb & "..MULTIDISCOUNT")
        Dim discId As String = Nothing
genDis:
        discId = "DISC" & Format(cnt, "00")
        If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & discId & "'").Length > 0 Then
            cnt += 1
            GoTo genDis
        End If
        If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..MULTIDISCOUNT WHERE DISCID = '" & discId & "'").Length > 0 Then
            cnt += 1
            GoTo genDis
        End If
        strSql = " INSERT INTO " & cnAdminDb & "..MULTIDISCOUNT"
        strSql += " (DISCID,DISCNAME,DISCAUTHCODE,DISCAUTHAMT)VALUES"
        strSql += " ('" & discId & "','" & txtDiscount_MAN.Text & "','" & txtDiscauthcode.Text & "'," & Val(txtDiscauthamt.Text) & ")"
        ExecQuery(SyncMode.Master, strSql, cn)
        InsertIntoAcHead(discId, txtDiscount_MAN.Text, "11")
        MsgBox("Successfully Inserted")
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        txtDiscount_MAN.Clear()
        txtDiscAuthAmt.Clear()
        txtDiscAuthCode.Clear()
        LoadGrid()
        txtDiscount_MAN.Focus()
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        btnSave_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub frmMultiDiscount_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblStatus.Visible = True
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then e.Handled = True
    End Sub

    Private Sub InsertIntoAcHead(ByVal AcCode As String, ByVal AcName As String, ByVal AcGrpCode As String)
        Dim str As String = Nothing
        If AcCode = Nothing Then Exit Sub
        If objGPack.DupCheck("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & AcCode & "'", tran) = False Then
            str = " insert into " & cnAdminDb & "..AcHead("
            str += " AcCode,AcName,ACGrpCode,ACSubGrpCode,"
            str += " AcType,DoorNo,Address1,Address2,"
            str += " Address3,Area,City,Pincode,"
            str += " PhoneNo,Mobile,"
            str += " Emailid,"
            str += " WebSite,Ledprint,TdsFlag,TdsPer,"
            str += " Depflag,Depper,Outstanding,AutoGen,"
            str += " VATEX,LocalOutst,LocalTaxNo,CentralTaxNo,"
            str += " Userid,CrDate,CrTime)values("
            str += " '" & AcCode & "','" & AcName & "','" & AcGrpCode & "','0',"
            str += " 'O','','','',"
            str += " '','','','',"
            str += " '','',"
            str += " '',"
            str += " '','','',0,"
            str += " '',0,'','',"
            str += " '','','','',"
            str += " " & userId & ",'" & Today.Date.ToString("yyyy-MM-dd") & "','" & Date.Now.ToLongTimeString & "')"
        Else
            str = " UPDATE " & cnAdminDb & "..ACHEAD"
            str += " SET ACNAME = '" & AcName & "'"
            str += " WHERE ACCODE = '" & AcCode & "'"
        End If
        ExecQuery(SyncMode.Master, str, cn)
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            e.Handled = True
            If gridView.RowCount > 0 Then
                gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells(0)
                txtDiscount_MAN.Text = gridView.CurrentRow.Cells("DISCNAME").Value
                txtDiscAuthCode.Text = gridView.CurrentRow.Cells("DISCAUTHCODE").Value
                txtDiscAuthAmt.Text = gridView.CurrentRow.Cells("DISCAUTHAMT").Value
                flagUpdateId = gridView.CurrentRow.Cells("DISCID").Value
                txtDiscount_MAN.Focus()
            End If
        End If
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        lblStatus.Visible = False
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
End Class