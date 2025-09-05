Imports System.Data.OleDb
Public Class frmOtherCharges
    Dim da As OleDbDataAdapter
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim flagSave As Boolean = False
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Function funcCallGrid() As Integer
        strSql = " SELECT MISCID,MISCNAME,"
        strSql += " (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST AS M WHERE M.METALID = CH.METALID)AS METALNAME,"
        strSql += " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY AS C WHERE C.CATCODE = CH.CATCODE)AS CATNAME,"
        strSql += " (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD AS A WHERE ACCODE = ACCTID)AS ACGRPNAME,"
        strSql += " DEFAULTVALUE,COMMISSION,"
        strSql += " CASE WHEN ACTIVE = 'Y' THEN 'YES' ELSE 'NO' END AS ACTIVE"
        strSql += " FROM " & cnAdminDb & "..MISCCHARGES AS CH"
        funcOpenGrid(strSql, gridView)
        With gridView
            .Columns("MISCID").Visible = False
            .Columns("MISCNAME").Width = 200
            .Columns("METALNAME").Width = 100
            .Columns("CATNAME").Width = 200
            .Columns("ACGRPNAME").HeaderText = "ACCNAME"
            .Columns("ACGRPNAME").Width = 150
            .Columns("DEFAULTVALUE").HeaderText = "VALUE"
            .Columns("DEFAULTVALUE").Width = 80
            .Columns("DEFAULTVALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("COMMISSION").HeaderText = "COMM%"
            .Columns("COMMISSION").Width = 80
            .Columns("COMMISSION").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("ACTIVE").Width = 60
        End With
    End Function
    Function funcNew() As Integer
        Dim dt As New DataTable
        dt.Clear()
        objGPack.TextClear(Me)
        flagSave = False
        txtMiscId.Text = objGPack.GetMax("MiscId", "MiscCharges", cnAdminDb)
        funcLoadMetalName()
        cmbActive.Text = "YES"
        funcCallGrid()
        txtMiscName__Man.Select()
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        If objGPack.DupChecker(txtMiscName__Man, "SELECT 1 FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCNAME = '" & txtMiscName__Man.Text & "' AND MISCID <> '" & txtMiscId.Text & "'") Then
            Exit Function
        End If
        If flagSave = False Then
            funcAdd()
        ElseIf flagSave = True Then
            funcUpdate()
        End If
    End Function
    Function funcAdd() As Integer
        Dim ds As New Data.DataSet
        ds.Clear()
        Dim MetalId As String = Nothing
        Dim CatCode As String = Nothing
        Dim AcctId As String = Nothing

        strSql = " select MetalId from " & cnAdminDb & "..MetalMast where MetalName = '" & cmbMetalName_Man.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "MetalId")
        If ds.Tables("Metalid").Rows.Count > 0 Then
            MetalId = ds.Tables("Metalid").Rows(0).Item("Metalid")
        End If

        strSql = " select CatCode from " & cnAdminDb & "..Category where CatName = '" & cmbCategoryName_Man.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "CatCode")
        If ds.Tables("CatCode").Rows.Count > 0 Then
            CatCode = ds.Tables("CatCode").Rows(0).Item("CatCode")
        End If

        strSql = " select AcCode from " & cnAdminDb & "..AcHead where AcName = '" & cmbAcctName_Man.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "AcCode")
        If ds.Tables("AcCode").Rows.Count > 0 Then
            AcctId = ds.Tables("AcCode").Rows(0).Item("AcCode")
        Else
            AcctId = "MIS" + txtMiscId.Text
        End If
        strSql = " Insert into " & cnAdminDb & "..MiscCharges"
        strSql += " ("
        strSql += " MiscId,MiscName,Metalid,catcode,"
        strSql += " AcctId,DefaultValue,Commission,active,"
        strSql += " UserId,Updated,Uptime"
        strSql += " )Values("
        strSql += " '" & txtMiscId.Text & "'" 'MiscId
        strSql += " ,'" & txtMiscName__Man.Text & "'" 'MiscName
        strSql += " ,'" & MetalId & "'" 'Metalid
        strSql += " ,'" & CatCode & "'" 'catcode
        strSql += " ,'" & AcctId & "'" 'AcctId
        strSql += " ," & Val(txtDefaultValue_Amt.Text) & "" 'DefaultValue
        strSql += " ," & Val(txtComm_Per.Text) & "" 'Commission
        strSql += " ,'" & Mid(cmbActive.Text, 1, 1) & "'" 'active
        strSql += " ," & userId & "" 'UserId
        strSql += " ,'" & GetServerDate() & "'" 'Updated
        strSql += " ,'" & GetServerTime() & "'" 'Uptime
        strSql += " )"

        Try
            tran = cn.BeginTransaction
            ExecQuery(SyncMode.Master, strSql, cn, tran)

            If Not ds.Tables("AcCode").Rows.Count > 0 Then
                strSql = funcGetSql("MIS" + txtMiscId.Text, txtMiscName__Man.Text, 3)
                ExecQuery(SyncMode.Master, strSql, cn, tran)
            End If
            tran.Commit()
            funcNew()
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try

    End Function
    Function funcUpdate() As Integer
        Dim ds As New Data.DataSet
        ds.Clear()
        Dim MetalId As String = Nothing
        Dim CatCode As String = Nothing
        Dim AcctId As String = Nothing

        strSql = " select MetalId from " & cnAdminDb & "..MetalMast where MetalName = '" & cmbMetalName_Man.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "MetalId")
        If ds.Tables("Metalid").Rows.Count > 0 Then
            MetalId = ds.Tables("Metalid").Rows(0).Item("Metalid")
        End If

        strSql = " select CatCode from " & cnAdminDb & "..Category where CatName = '" & cmbCategoryName_Man.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "CatCode")
        If ds.Tables("CatCode").Rows.Count > 0 Then
            CatCode = ds.Tables("CatCode").Rows(0).Item("CatCode")
        End If

        strSql = " select AcCode from " & cnAdminDb & "..AcHead where AcName = '" & cmbAcctName_Man.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "AcCode")
        If ds.Tables("AcCode").Rows.Count > 0 Then
            AcctId = ds.Tables("AcCode").Rows(0).Item("AcCode")
        End If

        strSql = " Update " & cnAdminDb & "..MiscCharges Set"
        strSql += " MiscName='" & txtMiscName__Man.Text & "'"
        strSql += " ,Metalid='" & MetalId & "'"
        strSql += " ,catcode='" & CatCode & "'"
        strSql += " ,AcctId='" & AcctId & "'"
        strSql += " ,DefaultValue=" & Val(txtDefaultValue_Amt.Text) & ""
        strSql += " ,Commission=" & Val(txtComm_Per.Text) & ""
        strSql += " ,active='" & Mid(cmbActive.Text, 1, 1) & "'"
        strSql += " ,UserId=" & userId & ""
        strSql += " ,Updated='" & Today.Date.ToString("yyyy-MM-dd") & "'"
        strSql += " ,Uptime='" & Date.Now.ToLongTimeString & "'"
        strSql += " Where MiscId = '" & txtMiscId.Text & "'"
        Try

            ExecQuery(SyncMode.Master, strSql, cn)

            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcGetDetails(ByVal temp As Integer) As Integer
        strSql = " Select MiscId,MiscName,"
        strSql += " (select MetalName from " & cnAdminDb & "..MetalMast as m where m.Metalid = ch.MetalId)as MetalName,"
        strSql += " (select CatName from " & cnAdminDb & "..Category as c where c.Catcode = ch.CatCode)as CatName,"
        strSql += " (select AcName from " & cnAdminDb & "..AcHead as a where AcCode = AcctId)as AcGrpName,"
        strSql += " DefaultValue,Commission,"
        strSql += " case when active = 'Y' then 'YES' else 'NO' end as Active"
        strSql += " From " & cnAdminDb & "..MiscCharges as Ch"
        strSql += " where MiscId = '" & temp & "'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        With dt.Rows(0)
            txtMiscId.Text = .Item("MiscId").ToString
            txtMiscName__Man.Text = .Item("MiscName").ToString
            cmbMetalName_Man.Text = .Item("MetalName").ToString
            cmbCategoryName_Man.Text = .Item("catName").ToString

            strSql = " select AcName from " & cnAdminDb & "..AcHead"
            strSql += " where AcGrpCode = 3 or AcGrpCode = 4 or AcGrpCode = 11"
            objGPack.FillCombo(strSql, cmbAcctName_Man)
            cmbAcctName_Man.Text = .Item("AcGrpName").ToString
            txtDefaultValue_Amt.Text = .Item("DefaultValue").ToString
            txtComm_Per.Text = .Item("Commission").ToString
            cmbActive.Text = .Item("active").ToString
        End With
        flagSave = True

    End Function
    Function funcGetSql(ByVal AcCode As String, ByVal AcName As String, ByVal AcGrpCode As String) As String
        Dim str As String = Nothing

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
        Return str
    End Function
    Function funcLoadMetalName() As Integer
        strSql = " select MetalName from " & cnAdminDb & "..MetalMast order by displayorder"
        objGPack.FillCombo(strSql, cmbMetalName_Man)
    End Function
    Function funcLoadCategoryName() As Integer
        cmbCategoryName_Man.Text = ""
        strSql = " Select CatName from " & cnAdminDb & "..Category "
        strSql += " Where MetalId = "
        strSql += " (select MetalId from " & cnAdminDb & "..MetalMast where MetalName = '" & cmbMetalName_Man.Text & "')"
        strSql += " order by CatName"
        objGPack.FillCombo(strSql, cmbCategoryName_Man)
        If Not cmbCategoryName_Man.Items.Count > 0 Then cmbCategoryName_Man.Enabled = False Else cmbCategoryName_Man.Enabled = True
    End Function
    Function funcLoadAcctName() As Integer
        cmbAcctName_Man.Items.Clear()
        Dim st As String = txtMiscName__Man.Text
        cmbAcctName_Man.Items.Add(st)
        strSql = " select AcName from " & cnAdminDb & "..AcHead"
        strSql += " where AcGrpCode = 3 or AcGrpCode = 4 or AcGrpCode = 11"
        objGPack.FillCombo(strSql, cmbAcctName_Man, False)
        cmbAcctName_Man.Text = st
    End Function

    Private Sub frmOtherCharges_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtMiscName__Man.Focused Then
                Exit Sub
            End If
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmOtherCharges_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcGridStyle(gridView)
        txtMiscId.Enabled = False
        cmbActive.Items.Add("YES")
        cmbActive.Items.Add("NO")
        cmbActive.Text = "YES"
        funcNew()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
    End Sub
    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click, OpenToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        gridView.Focus()
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        funcSave()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        funcNew()
    End Sub
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        funcExit()
    End Sub
    Private Sub cmbMetalName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMetalName_Man.SelectedIndexChanged
        funcLoadCategoryName()
    End Sub
    Private Sub cmbAcctName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAcctName_Man.GotFocus
        If flagSave = True Then
            Exit Sub
        End If
        If txtMiscName__Man.Text <> "" Then
            funcLoadAcctName()
        Else
            cmbAcctName_Man.Items.Clear()
        End If
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblStatus.Visible = True
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            gridView.CurrentCell = gridView.CurrentCell
            If gridView.RowCount > 0 Then
                funcGetDetails(gridView.Item(0, gridView.CurrentRow.Index).Value)
                txtMiscName__Man.Focus()
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            Me.SelectNextControl(grpField, True, True, True, True)
        End If
    End Sub

    Private Sub txtMiscName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMiscName__Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If objGPack.DupChecker(txtMiscName__Man, "SELECT 1 FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCNAME = '" & txtMiscName__Man.Text & "' AND MISCID <> '" & txtMiscId.Text & "'") Then
                txtMiscName__Man.Focus()
                Exit Sub
            Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub cmbAcctName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbAcctName_Man.KeyPress
        e.KeyChar = UCase(e.KeyChar)
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
        Dim delKey As String = gridView.Rows(gridView.CurrentRow.Index).Cells("MISCID").Value.ToString

        strSql = " SELECT DBNAME FROM " & cnAdminDb & "..DBMASTER WHERE DBNAME IN(SELECT NAME FROM MASTER..SYSDATABASES)"
        Dim dtDb As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtDb)
        If dtDb.Rows.Count > 0 Then
            chkQry = " SELECT TOP 1 ACCODE FROM " & cnAdminDb & "..ACHEAD "
            chkQry += vbCrLf + " WHERE ACCODE = (SELECT ACCTID FROM " & cnAdminDb & "..MISCCHARGES"
            chkQry += vbCrLf + " WHERE MISCID = '" & delKey & "')"
            chkQry += vbCrLf + " UNION "
            chkQry += vbCrLf + " SELECT TOP 1 CONVERT(VARCHAR,MISCID) FROM " & cnAdminDb & "..ITEMTAGMISCCHAR WHERE MISCID = '" & delKey & "'"
            For cnt As Integer = 0 To dtDb.Rows.Count - 1
                With dtDb.Rows(cnt)
                    chkQry += vbCrLf + " UNION "
                    chkQry += vbCrLf + " SELECT TOP 1 CONVERT(VARCHAR,MISCID) FROM " & .Item("DBNAME").ToString & "..ISSMISC WHERE MISCID = '" & delKey & "'"
                    chkQry += vbCrLf + " UNION "
                    chkQry += vbCrLf + " SELECT TOP 1 CONVERT(VARCHAR,MISCID) FROM " & .Item("DBNAME").ToString & "..ESTISSMISC WHERE MISCID = '" & delKey & "'"
                End With
            Next
            DeleteItem(SyncMode.Master, chkQry, "DELETE FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCID = '" & delKey & "'")
            funcCallGrid()
        End If
    End Sub
End Class