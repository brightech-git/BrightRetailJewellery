Imports System.Data.OleDb
Public Class frmPMIssue
    Dim MBillno As Integer
    Dim MBatchno As String = ""
    Dim Strsql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim _tran As OleDbTransaction = Nothing
    Public DtPmtran As New DataTable
    Dim dtPm As DataTable
    Dim FromBill As Boolean = False
    Dim COMPL_ISS_MODE As String = GetAdmindbSoftValue("COMPL_ISS_MODE", "N")
    Dim COMPLIMENT_NONTAG As String = GetAdmindbSoftValue("COMPLIMENT_NONTAG", "N")



    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Public Sub New(ByVal DtTable As DataTable)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        GridView.DataSource = Nothing
    End Sub


    Private Sub frmPMGenerateIssue_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            'SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            Me.Close()
        End If
    End Sub

    Private Sub PMGenerate_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        dtPm = New DataTable
        dtPm.Columns.Add("ITEMNAME", GetType(String))
        dtPm.Columns.Add("SUBITEMNAME", GetType(String))
        dtPm.Columns.Add("PCS", GetType(String))
        dtPm.Columns.Add("REMARK", GetType(String))
        GridView.DataSource = Nothing
        txtPcs.Text = "1"
        txtRemark.Text = ""
        txtCRowIndex.Text = ""
        LoadCombo()
        FuncGetRange()
        If cmbPMType.Text.ToString <> "ALL" And cmbPMType.Text.ToString <> "" Then
            Dim dr As DataRow
            dr = dtPm.NewRow
            dr!ITEMNAME = cmbPMType.Text.ToString
            If cmbPMSubType.Text.ToString <> "ALL" And cmbPMSubType.Text.ToString <> "" Then
                dr!SUBITEMNAME = cmbPMSubType.Text.ToString
            End If
            dr!PCS = txtPcs.Text.ToString
            dr!REMARK = txtRemark.Text.ToString
            dtPm.Rows.Add(dr)
            dtPm.AcceptChanges()
            txtPcs.Text = "1"
            txtRemark.Text = ""
            If dtPm.Rows.Count > 0 Then
                GridView.DataSource = Nothing
                GridView.DataSource = dtPm
            Else
                GridView.DataSource = Nothing
            End If
        End If
        cmbPMType.Focus()
    End Sub

    Function gridStyle()
        GridView.SelectionMode = DataGridViewSelectionMode.CellSelect
        GridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Function
    Private Function LoadCombo()
        cmbPMType.Items.Clear()
        If COMPLIMENT_NONTAG = "Y" Then
            Strsql = "SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ACTIVE,'')<>'N' AND ISNULL(COMPLIMENTS,'')='Y'"
            Strsql += " ORDER BY ITEMNAME"
        Else
            Strsql = "SELECT PMNAME FROM " & cnAdminDb & "..PMMAST WHERE ISNULL(ACTIVE,'')<>'N'"
            Strsql += " ORDER BY PMNAME"
        End If
        objGPack.FillCombo(Strsql, cmbPMType, True, False)
    End Function

    Private Sub GridView_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GridView.CellEnter

    End Sub


    Private Sub GridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles GridView.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        ElseIf e.KeyCode = Keys.Delete Then
            With GridView
                If .Rows.Count > 0 Then
                    .Rows.RemoveAt(.CurrentRow.Index)
                    .Refresh()
                    dtPm.AcceptChanges()
                    cmbPMType.Focus()
                End If
            End With
        End If
    End Sub

    Private Sub cmbPMType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbPMType.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbPMType.Text <> "" And cmbPMType.Text <> "ALL" Then
                If COMPLIMENT_NONTAG = "Y" Then
                    Strsql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID IN (SELECT ITEMID FROM  " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbPMType.Text.ToString & "') "
                    Strsql += " ORDER BY SUBITEMNAME"
                Else
                    Strsql = " SELECT PMSUBNAME FROM " & cnAdminDb & "..PMSUBMAST WHERE PMID IN (SELECT PMID FROM  " & cnAdminDb & "..PMMAST WHERE PMNAME='" & cmbPMType.Text.ToString & "') "
                    Strsql += " ORDER BY PMSUBNAME"
                End If

                objGPack.FillCombo(Strsql, cmbPMSubType, True, False)
                cmbPMSubType.Focus()
            Else
                cmbPMSubType.Items.Clear()
                cmbPMSubType.Items.Add("All")
            End If
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            Me.Close()
        End If
    End Sub



    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If GridView.RowCount = 0 Then Me.DialogResult = Windows.Forms.DialogResult.Cancel : Exit Sub
        If FromBill = True Then Me.DialogResult = Windows.Forms.DialogResult.OK : Exit Sub
        If MessageBox.Show("Do you want to save following packing material.", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
            InsertintoPmtran()
        End If
        Me.Close()
    End Sub
    Public Function InsertintoPmtran()

        If MBatchno = "" Then MBatchno = GridView.Rows(0).Cells("BATCHNO").Value.ToString
        If Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnStockDb & "..PMTRAN WHERE BATCHNO='" & MBatchno & "' AND TRANNO='" & Val(GridView.Rows(0).Cells("TRANNO").Value.ToString) & "'", , "", _tran)) > 0 Then
            Strsql = "DELETE FROM " & cnStockDb & "..PMTRAN WHERE BATCHNO='" & MBatchno & "' AND TRANNO='" & Val(GridView.Rows(0).Cells("TRANNO").Value.ToString) & "'"
            ExecQuery(SyncMode.Transaction, Strsql, cn, _tran, cnCostId)
        End If

        For m As Integer = 0 To GridView.RowCount - 1
            Dim Pmid As Integer = 0
            Dim Pmsubid As Integer = 0
            Pmid = Val(objGPack.GetSqlValue("SELECT PMID FROM " & cnAdminDb & "..PMMAST WHERE PMNAME ='" & GridView.Rows(m).Cells("PMNAME").Value.ToString & "'", , "", _tran))
            Pmsubid = Val(objGPack.GetSqlValue("SELECT PMSUBID FROM " & cnAdminDb & "..PMSUBMAST WHERE PMSUBNAME ='" & GridView.Rows(m).Cells("PMSUBNAME").Value.ToString & "' AND PMID=" & Pmid & "", , "", _tran))
            Strsql = " INSERT INTO " & cnStockDb & "..PMTRAN "
            Strsql += vbCrLf + " (SNO,PMID,PMSUBID,TRANDATE,TRANNO,TRANTYPE,PCS,AMOUNT,BATCHNO,COSTID,REMARK1,REMARK2"
            Strsql += vbCrLf + " ,CANCEL,FROMFLAG,USERID,UPDATED,UPTIME) VALUES("
            Strsql += vbCrLf + " '" & GetNewSno(TranSnoType.PMTRANCODE, tran) & "'" 'SNO
            Strsql += vbCrLf + " ," & Pmid & "" 'PMID
            Strsql += vbCrLf + " ," & Pmsubid & "" 'PMSUBID
            Strsql += vbCrLf + " ,'" & Format(GridView.Rows(m).Cells("TRANDATE").Value, "yyyy-MM-dd") & "'" 'TRANDATE
            Strsql += vbCrLf + " ,'" & Val(GridView.Rows(m).Cells("TRANNO").Value.ToString) & "'" 'TRANNO
            Strsql += vbCrLf + " ,''" 'TRANTYPE
            Strsql += vbCrLf + " ," & Val(GridView.Rows(m).Cells("QTY").Value.ToString) & "" 'PCS
            Strsql += vbCrLf + " ," & Val(GridView.Rows(m).Cells("AMOUNT").Value.ToString) & "" 'AMOUNT
            Strsql += vbCrLf + " ,'" & MBatchno & "'" 'BATCHNO
            Strsql += vbCrLf + " ,'" & cnCostId & "'" 'COSTID
            Strsql += vbCrLf + " ,''" 'REMARK1
            Strsql += vbCrLf + " ,''" 'REMARK2
            Strsql += vbCrLf + " ,NULL" 'CANCEL"
            Strsql += vbCrLf + " ,'P'" 'FROMFLAG
            Strsql += vbCrLf + " ,'" & userId & "'" 'USERID
            Strsql += vbCrLf + " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
            Strsql += vbCrLf + " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
            Strsql += vbCrLf + " )"
            ExecQuery(SyncMode.Transaction, Strsql, cn, _tran, cnCostId)
        Next
    End Function

    Private Sub cmbPMSubType_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbPMSubType.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txtPcs.Focus()
        End If
    End Sub

    Private Sub txtPcs_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPcs.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Val(txtPcs.Text.ToString) = 0 Then
                txtPcs.Focus()
                Exit Sub
            End If
            txtRemark.Focus()
        End If
    End Sub

    Private Sub txtRemark_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtRemark.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If COMPLIMENT_NONTAG = "Y" Then
                Strsql = "SELECT 1 FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbPMType.Text.ToString & "'"
            Else
                Strsql = "SELECT 1 FROM " & cnAdminDb & "..PMMAST WHERE PMNAME='" & cmbPMType.Text.ToString & "'"
            End If

            If Val(objGPack.GetSqlValue(Strsql, , "")) <= 0 Then
                MsgBox("Item not found...", MsgBoxStyle.Information)
                cmbPMType.Focus()
                Exit Sub
            End If
            If Val(txtPcs.Text.ToString) = 0 Then
                MsgBox("Please check Pcs", MsgBoxStyle.Information)
                txtPcs.Focus()
                Exit Sub
            End If
            If COMPLIMENT_NONTAG = "Y" Then
                Strsql = " SELECT 1 FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID IN  (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbPMType.Text.ToString & "')"
                If cmbPMSubType.Text.ToString = "ALL" Or cmbPMSubType.Text.ToString = "" Then
                    If Val(objGPack.GetSqlValue(Strsql, , "")) > 0 Then
                        MsgBox("Please Select Subitem", MsgBoxStyle.Information)
                        cmbPMSubType.Focus()
                        Exit Sub
                    End If
                End If
            End If
            If FuncCheckRange() = False Then Exit Sub
            If txtCRowIndex.Text.ToString = "" Then
                Dim dr As DataRow
                dr = dtPm.NewRow
                dr!ITEMNAME = cmbPMType.Text.ToString
                If cmbPMSubType.Text.ToString <> "ALL" And cmbPMSubType.Text.ToString <> "" Then
                    dr!SUBITEMNAME = cmbPMSubType.Text.ToString
                End If
                dr!PCS = txtPcs.Text.ToString
                dr!REMARK = txtRemark.Text.ToString
                dtPm.Rows.Add(dr)
                dtPm.AcceptChanges()
                txtPcs.Text = "1"
                txtRemark.Text = ""
                If dtPm.Rows.Count > 0 Then
                    GridView.DataSource = Nothing
                    GridView.DataSource = dtPm
                Else
                    GridView.DataSource = Nothing
                End If
            Else
                If GridView.Rows.Count > 0 Then
                    With GridView.Rows(txtCRowIndex.Text.ToString)
                        .Cells("ITEMNAME").Value = cmbPMType.Text.ToString
                        If cmbPMSubType.Text.ToString <> "ALL" And cmbPMSubType.Text.ToString <> "" Then
                            .Cells("SUBITEMNAME").Value = cmbPMSubType.Text.ToString
                        End If
                        .Cells("PCS").Value = txtPcs.Text.ToString
                        .Cells("REMARK").Value = txtRemark.Text.ToString
                    End With
                    txtCRowIndex.Text = ""
                    GridView.Refresh()
                End If
            End If
            cmbPMType.Focus()
        End If
    End Sub

    Private Sub GridView_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles GridView.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Not GridView.Rows.Count > 0 Then Exit Sub
            With GridView
                cmbPMType.Text = .CurrentRow.Cells("ITEMNAME").Value.ToString
                cmbPMSubType.Text = .CurrentRow.Cells("SUBITEMNAME").Value.ToString
                txtPcs.Text = .CurrentRow.Cells("PCS").Value.ToString
                txtRemark.Text = .CurrentRow.Cells("REMARK").Value.ToString
                txtCRowIndex.Text = .CurrentRow.Index.ToString
                cmbPMType.Focus()
            End With
        ElseIf e.KeyChar = Chr(Keys.Delete) Then
            With GridView
                If .Rows.Count > 0 Then
                    .Rows.RemoveAt(.CurrentRow.Index)
                    .Refresh()
                    dtPm.AcceptChanges()
                End If
            End With
        End If
    End Sub

    Private Function FuncGetRange()
        Strsql = "SELECT TOP 1"
        Strsql += vbCrLf + "(SELECT TOP 1 PMNAME FROM " & cnAdminDb & "..PMMAST WHERE PMID=V.PMID)PMNAME,"
        Strsql += vbCrLf + "(SELECT TOP 1 PMSUBNAME FROM " & cnAdminDb & "..PMSUBMAST WHERE PMSUBID=V.PMSUBID)PMSUBNAME"
        Strsql += vbCrLf + "FROM " & cnAdminDb & "..PMVALIDATE AS V WHERE '" & Val(txtAmount.Text.ToString) & "' BETWEEN FROM_AMT AND TO_AMT"
        DtPmtran = New DataTable
        cmd = New OleDbCommand(Strsql, cn)
        da = New OleDbDataAdapter(cmd)
        da.Fill(DtPmtran)
        If DtPmtran.Rows.Count > 0 Then
            cmbPMType.Text = DtPmtran.Rows(0)("PMNAME").ToString
            cmbPMSubType.Text = DtPmtran.Rows(0)("PMSUBNAME").ToString
        End If
    End Function

    Private Function FuncCheckRange()
        If COMPLIMENT_NONTAG = "Y" Then Return True : Exit Function
        If DtPmtran.Rows.Count > 0 Then
            If cmbPMType.Text.ToString <> DtPmtran.Rows(0)("PMNAME").ToString Then
                If txtRemark.Text.ToString.Length >= 5 Then Return True : Exit Function
                MsgBox("Complement Item Mismatched. Please type Remark...", MsgBoxStyle.Information)
                txtRemark.Focus()
                Return False
                Exit Function
            Else
                Return True
                Exit Function
            End If
        ElseIf DtPmtran.Rows.Count = 0 And txtRemark.Text.ToString.Length < 5 Then
            MsgBox("Complement Item not found. Please type remarks...", MsgBoxStyle.Information)
            txtRemark.Focus()
            Return False
            Exit Function
        End If
        Return True
    End Function

End Class