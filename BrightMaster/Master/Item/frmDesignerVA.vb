Imports System.Data.OleDb
Public Class frmDesignerVA
    Dim strSql As String
    Dim flagSave As Boolean
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim id, TEMP As Integer
    Dim cefval, act As String
    Dim dt As New DataTable
    Dim designerid, itemid, ssubitem, sitem As String
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Function funcAdd() As Integer
        id = objGPack.GetSqlValue("SELECT ISNULL(MAX(ID),0)+1 FROM " & cnAdminDb & "..DESIGNERVA ")
        designerid = objGPack.GetSqlValue("SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER  WHERE  DESIGNERNAME='" & CmbName_MAN.Text & "' ")
        itemid = objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST  WHERE  ITEMNAME='" & Cmbitemname_MAN.Text & "' ")
        sitem = objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST  WHERE  ITEMNAME='" & CmbStnitem_MAN.Text & "' ")
        ssubitem = objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST  WHERE  SUBITEMNAME='" & CmbStudsubitem.Text & "' ")
        If ssubitem = "" Then
            ssubitem = 0
        End If
        strSql = " INSERT INTO " & cnAdminDb & "..DESIGNERVA "
        strSql += " (ID,DESIGNERID,STNITEMID,STNRATEPER"
        strSql += " ,WASTPER,WASTAGE,MCGRM,MC"
        strSql += " ,CERCHARGES,ACTIVE,USERID,UPDATED,UPTIME"
        strSql += " ,ITEMID,STNSUBITEMID) VALUES "
        strSql += " (" & id & ",'" & designerid & "'," & sitem & ",'" & Val(txtRate_NUM.Text) & "'"
        strSql += " ,'" & Val(txtWastPer_PER.Text) & "'"
        strSql += " ,'" & Val(txtWast_NUM.Text) & "'"
        strSql += " ,'" & Val(txtMcper_NUM.Text) & "'"
        strSql += " ,'" & Val(txtMcGrm_NUM.Text) & "'"
        strSql += " ,'" & Mid(Cmbcerfichrge.Text, 1, 1) & "','" & Mid(cmbActive.Text, 1, 1) & "','" & userId & "', "
        strSql += " '" & Today.Date.ToString("yyyy-MM-dd") & "','" & Date.Now.ToLongTimeString & "','" & itemid & "','" & ssubitem & "')"
        ExecQuery(SyncMode.Master, strSql, cn)
        MsgBox("Save Successfully..", MsgBoxStyle.Information, "Message")
    End Function
    Function funcUpdate() As Integer
        designerid = objGPack.GetSqlValue("SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER  WHERE  DESIGNERNAME='" & CmbName_MAN.Text & "' ")
        itemid = objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST  WHERE  ITEMNAME='" & Cmbitemname_MAN.Text & "' ")
        sitem = objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST  WHERE  ITEMNAME='" & CmbStnitem_MAN.Text & "' ")
        ssubitem = objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST  WHERE  SUBITEMNAME='" & CmbStudsubitem.Text & "' ")
        If ssubitem = "" Then
            ssubitem = 0
        End If
        strSql = " UPDATE " & cnAdminDb & "..DESIGNERVA SET DESIGNERID = '" & designerid & "'"
        strSql += " ,STNITEMID = '" & sitem & "' "
        strSql += " ,ITEMID = '" & itemid & "' "
        strSql += " ,STNSUBITEMID = '" & ssubitem & "' "
        strSql += " ,STNRATEPER = '" & Val(txtRate_NUM.Text) & "' "
        strSql += " ,WASTPER = '" & Val(txtWastPer_PER.Text) & "' "
        strSql += " ,WASTAGE = '" & Val(txtWast_NUM.Text) & "' "
        strSql += " ,MCGRM = '" & Val(txtMcper_NUM.Text) & "' "
        strSql += " ,MC = '" & Val(txtMcGrm_NUM.Text) & "' "
        strSql += " ,CERCHARGES = '" & Mid(Cmbcerfichrge.Text, 1, 1) & "'"
        strSql += " ,ACTIVE = '" & Mid(cmbActive.Text, 1, 1) & "'"
        strSql += " WHERE ID = '" & TEMP & "'"
        ExecQuery(SyncMode.Master, strSql, cn)
        MsgBox("Update Successfully..", MsgBoxStyle.Information, "Message")
    End Function
    Function funcNew() As Integer
        objGPack.TextClear(Me)
        cmbActive.Text = "YES"
        Cmbcerfichrge.Text = "YES"
        flagSave = False

        CmbName_MAN.Items.Clear()
        strSql = " SELECT DESIGNERNAME FROM  " & cnAdminDb & "..DESIGNER WHERE ACTIVE='Y' ORDER BY DESIGNERNAME"
        objGPack.FillCombo(strSql, CmbName_MAN, False)

        CmbStnitem_MAN.Items.Clear()
        strSql = " SELECT ITEMNAME FROM  " & cnAdminDb & "..ITEMMAST WHERE METALID IN('T','D') AND ACTIVE='Y' ORDER BY ITEMNAME"
        objGPack.FillCombo(strSql, CmbStnitem_MAN, False)


        Cmbitemname_MAN.Items.Clear()
        strSql = " SELECT ITEMNAME FROM  " & cnAdminDb & "..ITEMMAST WHERE METALID NOT IN('T','D') AND ACTIVE='Y' ORDER BY ITEMNAME"
        objGPack.FillCombo(strSql, Cmbitemname_MAN, False)

        txtRate_NUM.Text = ""
        gridView.DataSource = Nothing
        btnOpen_Click(Me, New EventArgs)
        CmbName_MAN.Select()
        CmbName_MAN.Focus()
    End Function
    Private Sub frmDesignerVA_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmDesignerVA_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcGridStyle(gridView)
        cmbActive.Items.Add("YES")
        cmbActive.Items.Add("NO")
        Cmbcerfichrge.Items.Add("YES")
        Cmbcerfichrge.Items.Add("NO")
        funcNew()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        strSql = ""
        strSql += "SELECT A.ID AS ID,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID=A.DESIGNERID)NAME,"
        strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=A.ITEMID)AS ITEMNAME,"
        strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=A.STNITEMID)STUDDEDITEM,"
        strSql += " (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID=A.STNSUBITEMID)STUDDEDSUBITEM,CONVERT(NUMERIC(15,2),A.STNRATEPER) AS STONERATE,"
        strSql += " WASTPER,WASTAGE,MCGRM,MC,"
        strSql += " (CASE WHEN CERCHARGES ='Y' THEN 'YES' ELSE 'NO' END )AS CERCHARGE,(CASE WHEN ACTIVE='Y' THEN 'YES'ELSE 'NO' END )AS ACTIVE"
        strSql += " FROM("
        strSql += " SELECT ID,DESIGNERID,ITEMID,STNSUBITEMID,STNITEMID,STNRATEPER,CERCHARGES ,ACTIVE"
        strSql += " ,WASTPER,WASTAGE,MCGRM,MC"
        strSql += " FROM  " & cnAdminDb & "..DESIGNERVA "
        strSql += " )A"
        da = New OleDbDataAdapter(strSql, cn)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count < 0 Then
            'MsgBox("Records not found..", MsgBoxStyle.Information, "Message")
            Exit Sub
        Else
            gridView.DataSource = Nothing
            gridView.DataSource = dt
            gridView.Columns("NAME").Width = 170
            gridView.Columns("NAME").HeaderText = "DESIGNER"
            gridView.Columns("ITEMNAME").Width = 150
            gridView.Columns("ITEMNAME").HeaderText = "ITEM"
            gridView.Columns("STUDDEDITEM").Width = 150
            gridView.Columns("STUDDEDSUBITEM").Width = 150
            gridView.Columns("WASTPER").Width = 75
            gridView.Columns("WASTAGE").Width = 75
            gridView.Columns("MCGRM").Width = 75
            gridView.Columns("MC").Width = 75
            gridView.Columns("ACTIVE").Visible = False
            gridView.Columns("ID").Visible = False
            gridView.Columns("CERCHARGE").Visible = False
            gridView.Columns("WASTPER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            gridView.Columns("WASTAGE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            gridView.Columns("MCGRM").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            gridView.Columns("MC").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            gridView.Columns("STONERATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            gridView.Focus()
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        btnOpen_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If objGPack.Validator_Check(Me) Then Exit Sub
        Try
            If flagSave = False Then
                funcAdd()
                funcNew()
            Else
                funcUpdate()
            End If
            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblStatus.Visible = True
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Escape Then
            CmbName_MAN.Focus()
        ElseIf e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridView.RowCount > 0 Then
                gridView.CurrentCell = gridView.CurrentCell
                CmbName_MAN.Text = gridView.CurrentRow.Cells("NAME").Value.ToString
                Cmbitemname_MAN.Text = gridView.CurrentRow.Cells("ITEMNAME").Value.ToString
                CmbStnitem_MAN.Text = gridView.CurrentRow.Cells("STUDDEDITEM").Value.ToString
                CmbStudsubitem.Text = gridView.CurrentRow.Cells("STUDDEDSUBITEM").Value.ToString
                txtRate_NUM.Text = Val(gridView.CurrentRow.Cells("STONERATE").Value.ToString)
                txtWastPer_PER.Text = Val(gridView.CurrentRow.Cells("WASTPER").Value.ToString)
                txtWast_NUM.Text = Val(gridView.CurrentRow.Cells("WASTAGE").Value.ToString)
                txtMcper_NUM.Text = Val(gridView.CurrentRow.Cells("MCGRM").Value.ToString)
                txtMcGrm_NUM.Text = Val(gridView.CurrentRow.Cells("MC").Value.ToString)
                Cmbcerfichrge.Text = gridView.CurrentRow.Cells("CERCHARGE").Value.ToString
                cmbActive.Text = gridView.CurrentRow.Cells("ACTIVE").Value.ToString
                TEMP = gridView.CurrentRow.Cells("ID").Value.ToString
                flagSave = True
                CmbName_MAN.Select()
                CmbName_MAN.Focus()
            End If
        End If
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        btnSave_Click(Me, New EventArgs)
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        lblStatus.Visible = False
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        Dim chkQry As String = Nothing
        Dim delKey As String = gridView.Rows(gridView.CurrentRow.Index).Cells("ID").Value.ToString
        strSql = "DELETE FROM " & cnAdminDb & "..DESIGNERVA WHERE ID= '" & delKey & "'"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        btnOpen_Click(Me, New EventArgs)
    End Sub

    Private Sub CmbStudsubitem_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmbStudsubitem.GotFocus
        If flagSave = False Then
            CmbStudsubitem.Items.Clear()
            itemid = objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST  WHERE  ITEMNAME='" & CmbStnitem_MAN.Text & "' ")
            strSql = "SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID='" & itemid & "'  "
            objGPack.FillCombo(strSql, CmbStudsubitem, False)
        Else
            CmbStudsubitem.Items.Clear()
            itemid = objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST  WHERE  ITEMNAME='" & CmbStnitem_MAN.Text & "' ")
            strSql = "SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID='" & itemid & "'  "
            objGPack.FillCombo(strSql, CmbStudsubitem, False)
            CmbStudsubitem.Text = gridView.CurrentRow.Cells("STUDDEDSUBITEM").Value.ToString
        End If
        
    End Sub
End Class