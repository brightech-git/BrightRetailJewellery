Imports System.Data.OleDb
Public Class frmProcessType
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim tempProId As Integer = Nothing ''For Update Purpose
    Dim flagSave As Boolean = False
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Function funcCallGrid() As Integer
        Dim dt As New DataTable
        dt.Clear()
        strSql = " SELECT PROID,PRONAME,(SELECT TOP 1 PRONAME FROM " & cnAdminDb & "..PROCESSTYPE WHERE PROID=P.UPROID) AS UNDERPRONAME,"
        strSql += " CASE WHEN PROTYPE = 'I' THEN 'ISSUE' "
        strSql += " WHEN PROTYPE = 'R' THEN 'RECEIPT'"
        strSql += " WHEN PROTYPE = 'O' THEN 'OTHERS'"
        strSql += " WHEN PROTYPE = 'B' THEN 'BOTH' ELSE '' END AS PROTYPE,"
        strSql += " CASE WHEN PROMODULE = 'S' THEN 'STOCK' "
        strSql += " WHEN PROMODULE = 'O' THEN 'ORDER' "
        strSql += " WHEN PROMODULE = 'R' THEN 'REPAIR' "
        strSql += " WHEN PROMODULE = 'T' THEN 'OTHER' "
        strSql += " WHEN PROMODULE = 'P' THEN 'POINT OF SALE' "
        strSql += " WHEN PROMODULE = 'C' THEN 'CASH TRANSACTION' "
        strSql += " WHEN PROMODULE = 'M' THEN 'SMITH TRANSACTION' ELSE '' END AS PROMODULE,"
        'strSql += " CASE WHEN GENTRANNO = 'Y' THEN 'YES' ELSE 'NO' END AS GENTRANNO,"
        'strSql += " ISSUENO,RECEIPTNO,"
        strSql += " (SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=P.ACCODE) AS ACNAME,"
        strSql += " PENDINGTRANSFER"
        strSql += " FROM " & cnAdminDb & "..PROCESSTYPE P"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        gridView.DataSource = dt
        gridView.Columns("PROID").Visible = False
        With gridView
            .Columns("PROID").Width = 50
            .Columns("PROID").HeaderText = "ID"
            .Columns("PROID").HeaderText = "TRANID"
            .Columns("PRONAME").Width = 180
            .Columns("PRONAME").HeaderText = "TRANNAME"
            .Columns("UNDERPRONAME").Width = 180
            .Columns("UNDERPRONAME").HeaderText = "TRANSACTIONUNDER"
            .Columns("PROTYPE").Width = 80
            .Columns("PROMODULE").Width = 150
            '.Columns("GENTRANNO").Width = 80
            '.Columns("ISSUENO").Width = 80
            '.Columns("RECEIPTNO").Width = 80
            .Columns("PENDINGTRANSFER").Width = 80
            .Columns("ACNAME").Width = 80
            .Columns("PENDINGTRANSFER").HeaderText = "PEN_TRANS"
        End With
        Return 0
    End Function
    Function funcNew()
        funcClear()
        funcCallGrid()
        flagSave = False
        cmbProcessType.Text = "BOTH"
        cmbTranNoGen.Text = "NO"
        cmbProcessModule.Text = "STOCK"

        strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ISNULL(ACTIVE,'')<>'N' ORDER BY ACNAME"
        objGPack.FillCombo(strSql, CmbAcname_OWN, True, False)

        strSql = " SELECT PRONAME FROM " & cnAdminDb & "..PROCESSTYPE ORDER BY PRONAME"
        objGPack.FillCombo(strSql, cmbTranUnder, True, False)
        If Not cmbTranUnder.Items.Count > 0 Then cmbTranUnder.Enabled = False

        chkPendingTransfer.Checked = False
        SaveToolStripMenuItem.Enabled = True
        txtProcessName__Man.Select()
        Return 0
    End Function
    Function funcClear()
        txtProcessName__Man.Clear()
        txtIssueNo_NUM.Clear()
        txtReceiptNo_NUM.Clear()
        Return 0
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        If objGPack.DupChecker(txtProcessName__Man, "SELECT 1 FROM " & cnAdminDb & "..PROCESSTYPE WHERE PRONAME = '" & txtProcessName__Man.Text & "' AND PROID <> '" & tempProId & "'") Then
            Exit Function
        End If
        If flagSave = False Then
            funcAdd()
        ElseIf flagSave = True Then
            funcUpdate()
        End If
    End Function
    Function funcGetProModule(ByVal str As String) As Char
        If str = "STOCK" Then
            Return "S"
        ElseIf str = "ORDER" Then
            Return "O"
        ElseIf str = "REPAIR" Then
            Return "R"
        ElseIf str = "OTHER" Then
            Return "T"
        ElseIf str = "CASH TRANSACTION" Then
            Return "C"
        ElseIf str = "SMITH TRANSACTION" Then
            Return "M"
        Else
            Return "P"
        End If
    End Function
    Function funcAdd()
        Dim tran As OleDbTransaction = Nothing
        Dim ProId As Integer = Nothing
        Dim dt As New DataTable
        dt.Clear()
        Try
            tran = cn.BeginTransaction()
            strSql = " Select isnull(Max(ProId),0)+1 as ProId from " & cnAdminDb & "..ProcessType"
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(dt)
            ProId = dt.Rows(0).Item("ProId").ToString
            strSql = " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME ='" & CmbAcname_OWN.Text.Trim & "'"
            Dim ACCODE As String = objGPack.GetSqlValue(strSql, "", "", tran)
            Dim _uproId As String = ""
            If cmbTranUnder.Text.Trim <> "" Then
                strSql = " SELECT PROID FROM " & cnAdminDb & "..PROCESSTYPE WHERE PRONAME ='" & cmbTranUnder.Text.Trim & "'"
                _uproId = objGPack.GetSqlValue(strSql, "", "", tran)
            End If

            strSql = " insert into " & cnAdminDb & "..ProcessType"
            strSql += " ("
            strSql += " ProId,UPROID,ProName,ProType,GenTranNo,ProModule,"
            If cmbTranNoGen.Text = "YES" Then
                strSql += " ISSUENO,"
                strSql += " RECEIPTNO,"
            End If
            strSql += " UsrId,Updated,Uptime"
            If chkPendingTransfer.Visible Then
                If chkPendingTransfer.Checked Then
                    strSql += " ,PENDINGTRANSFER"
                End If
            End If
            strSql += " ,ACCODE "
            strSql += " )Values ("
            strSql += " " & ProId & ""
            strSql += " ," & IIf(_uproId <> "", _uproId, 0) & ""
            strSql += " ,'" & txtProcessName__Man.Text & "'"
            strSql += " ,'" & Mid(cmbProcessType.Text, 1, 1) & "'"
            strSql += " ,'" & Mid(cmbTranNoGen.Text, 1, 1) & "'"
            strSql += " ,'" & funcGetProModule(cmbProcessModule.Text) & "'"
            If cmbTranNoGen.Text = "YES" Then
                strSql += " ,'" & txtIssueNo_NUM.Text & "'"
                strSql += " ,'" & txtReceiptNo_NUM.Text & "'"
            End If
            strSql += " ," & userId & ""
            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'"
            strSql += " ,'" & Date.Now.ToLongTimeString & "'"
            If chkPendingTransfer.Visible Then
                If chkPendingTransfer.Checked Then
                    strSql += " ,'Y'"
                End If
            End If
            strSql += " ,'" & ACCODE & "'"
            strSql += " )"
            ExecQuery(SyncMode.Master, strSql, cn, tran)
            tran.Commit()
            funcNew()
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
        Return 0
    End Function
    Function funcUpdate()
        Dim dt As New DataTable
        dt.Clear()
        strSql = " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME ='" & CmbAcname_OWN.Text.Trim & "'"
        Dim ACCODE As String = objGPack.GetSqlValue(strSql, "", "", )
        Dim _uproId As String = ""
        If cmbTranUnder.Text.Trim <> "" Then
            strSql = " SELECT PROID FROM " & cnAdminDb & "..PROCESSTYPE WHERE PRONAME ='" & cmbTranUnder.Text.Trim & "'"
            _uproId = objGPack.GetSqlValue(strSql, "", "", tran)
        End If

        strSql = " Update " & cnAdminDb & "..ProcessType Set"
        strSql += " ProName = '" & txtProcessName__Man.Text & "'"
        strSql += " ,UPROID = " & IIf(_uproId <> "", _uproId, 0)
        strSql += " ,ProType = '" & Mid(cmbProcessType.Text, 1, 1) & "'"
        strSql += " ,GenTranNo = '" & Mid(cmbTranNoGen.Text, 1, 1) & "'"
        strSql += " ,ProModule = '" & funcGetProModule(cmbProcessModule.Text) & "'"
        strSql += " ,IssueNo = '" & txtIssueNo_NUM.Text & "'"
        strSql += " ,receiptNo = '" & txtReceiptNo_NUM.Text & "'"
        strSql += " ,usrid = " & userId & ""
        strSql += " ,Updated = '" & Today.Date.ToString("yyyy-MM-dd") & "'"
        strSql += " ,Uptime = '" & Date.Now.ToLongTimeString & "'"
        If chkPendingTransfer.Checked = True Then
            strSql += " ,PENDINGTRANSFER = 'Y'"
        Else
            strSql += " ,PENDINGTRANSFER = ''"
        End If
        strSql += " ,ACCODE = '" & ACCODE & "'"
        strSql += " where ProId = " & tempProId & ""
        ExecQuery(SyncMode.Master, strSql, cn)
        funcNew()
        Return 0
    End Function
    Function funcGetDetails(ByVal tempId As Integer)
        Dim dt As New DataTable
        dt.Clear()
        strSql = " Select "
        strSql += " ProName,(SELECT TOP 1 PRONAME FROM " & cnAdminDb & "..PROCESSTYPE WHERE PROID=P.UPROID) AS UNDERPRONAME,"
        strSql += " case when ProType = 'I' then 'ISSUE' "
        strSql += " when ProType = 'R' then 'RECEIPT' "
        strSql += " when ProType = 'O' then 'OTHERS' "
        strSql += " when ProType = 'B' then 'BOTH' else '' end as ProType,"
        strSql += " case when GenTranNo = 'Y' then 'YES' else 'NO' end as GenTranNo,"
        strSql += " case when ProModule = 'S' then 'STOCK' "
        strSql += " when ProModule = 'O' then 'ORDER' "
        strSql += " when ProModule = 'R' then 'REPAIR' "
        strSql += " when ProModule = 'T' then 'OTHER' "
        strSql += " when ProModule = 'P' then 'POINT OF SALE' "
        strSql += " when ProModule = 'C' then 'CASH TRANSACTION' "
        strSql += " WHEN PROMODULE = 'M' THEN 'SMITH TRANSACTION' else '' end as ProModule,"
        strSql += " isnull(ISSUENO,'')issueno,isnull(RECEIPTNO,'')receiptno,UsrId,Updated,Uptime"
        strSql += " ,ISNULL(PENDINGTRANSFER,'')AS PENDINGTRANSFER"
        strSql += " ,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=P.ACCODE) AS ACNAME"
        strSql += " from " & cnAdminDb & "..ProcessType p where ProId = " & tempId & ""
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        With dt.Rows(0)
            txtProcessName__Man.Text = .Item("ProName").ToString
            cmbTranUnder.Text = .Item("UNDERPRONAME").ToString
            cmbProcessType.Text = .Item("ProType").ToString
            cmbTranNoGen.Text = .Item("GenTranNo").ToString
            cmbProcessModule.Text = .Item("ProModule").ToString
            If txtIssueNo_NUM.Enabled = True Then
                txtIssueNo_NUM.Text = .Item("ISSUENO").ToString
                txtReceiptNo_NUM.Text = .Item("RECEIPTNO").ToString
            Else
                txtIssueNo_NUM.Clear()
                txtReceiptNo_NUM.Clear()
            End If
            If UCase(.Item("PENDINGTRANSFER").ToString) = "Y" Then
                chkPendingTransfer.Checked = True
            Else
                chkPendingTransfer.Checked = False
            End If
            CmbAcname_OWN.Text = .Item("ACNAME").ToString
        End With
        tempProId = tempId
        Return 0
    End Function

    Private Sub frmProcessType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtProcessName__Man.Focused Then
                Exit Sub
            End If
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmProcessType_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcGridStyle(gridView)

        cmbProcessType.Items.Add("BOTH")
        cmbProcessType.Items.Add("ISSUE")
        cmbProcessType.Items.Add("RECEIPT")
        cmbProcessType.Items.Add("OTHERS")
        cmbProcessType.Text = "BOTH"

        cmbProcessModule.Items.Add("STOCK")
        cmbProcessModule.Items.Add("ORDER")
        cmbProcessModule.Items.Add("REPAIR")
        cmbProcessModule.Items.Add("OTHER")
        cmbProcessModule.Items.Add("POINT OF SALE")
        cmbProcessModule.Items.Add("CASH TRANSACTION")
        cmbProcessModule.Items.Add("SMITH TRANSACTION")
        cmbProcessModule.Text = "STOCK"

        cmbTranNoGen.Items.Add("NO")
        cmbTranNoGen.Items.Add("YES")
        cmbTranNoGen.Text = "NO"

        funcNew()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
    End Sub
    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click, OpenToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        funcCallGrid()
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
    Private Sub cmbTranNoGen_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTranNoGen.SelectedIndexChanged
        If cmbTranNoGen.Text = "NO" Then
            txtIssueNo_NUM.Enabled = False
            txtReceiptNo_NUM.Enabled = False
        Else
            txtIssueNo_NUM.Enabled = True
            txtReceiptNo_NUM.Enabled = True
        End If
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblStatus.Visible = True
    End Sub
    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            If gridView.RowCount > 0 Then
                funcGetDetails(gridView.Item(0, gridView.CurrentRow.Index).Value.ToString)
                flagSave = True
                txtProcessName__Man.Focus()
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            txtProcessName__Man.Focus()
        End If
    End Sub

    Private Sub cmbProcessModule_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbProcessModule.SelectedIndexChanged
        If cmbProcessModule.Text = "SMITH TRANSACTION" Then
            chkPendingTransfer.Visible = True
        Else
            chkPendingTransfer.Visible = False
        End If
    End Sub

    Private Sub txtProcessName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtProcessName__Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If objGPack.DupChecker(txtProcessName__Man, "SELECT 1 FROM " & cnAdminDb & "..PROCESSTYPE WHERE PRONAME = '" & txtProcessName__Man.Text & "' and proID <> '" & tempProId & "'") Then
                Exit Sub
            Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        lblStatus.Visible = False
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        If Not gridView.RowCount > 0 Then Exit Sub
        Dim list As New List(Of String)
        list.Add("PROID")
        list.Add("CTRANCODE")
        Dim ProId As String = gridView.CurrentRow.Cells("PROID").Value.ToString
        If DeleteItem(SyncMode.Master, list, "DELETE FROM " & cnAdminDb & "..PROCESSTYPE WHERE PROID = '" & ProId & "'", ProId, "PROCESSTYPE") Then
            funcCallGrid()
            gridView.Focus()
        End If
    End Sub
End Class