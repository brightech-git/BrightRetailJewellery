Imports System.Data.OleDb
Public Class W_PendingBills
    Public Enum PendingMode
        Weight = 0
        Amount = 1
    End Enum
    Dim StrSql As String
    Dim Da As OleDbDataAdapter
    Dim Cmd As OleDbCommand
    Dim DtGrid As DataTable
    Dim ModeOfPending As PendingMode
    Dim strDiscTabName As String
    Public Sub New(ByVal Billdate As Date, ByVal Accode As String, ByVal modeOfPending As PendingMode, Optional ByVal strTabName As String = "")
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        strDiscTabName = strTabName
        gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        gridView.RowHeadersVisible = False
        gridView.BorderStyle = BorderStyle.Fixed3D
        gridView.BackgroundColor = Color.Lavender

        ' Add any initialization after the InitializeComponent() call.
        Me.ModeOfPending = modeOfPending
        StrSql = vbCrLf + " DECLARE @DATE SMALLDATETIME"
        StrSql += vbCrLf + " SELECT @DATE = '" & Billdate.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPISSUE')>0 DROP TABLE TEMPISSUE"
        StrSql += vbCrLf + " SELECT TRANDATE,TRANNO," & IIf(modeOfPending = PendingMode.Weight, "PUREWT", "AMOUNT") & " AS VALUE"
        StrSql += vbCrLf + " ,ACCODE,SETTLED,SNO"
        StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL)RECVALUE"
        StrSql += vbCrLf + " ,CONVERT(SMALLDATETIME,NULL)RECDATE"
        StrSql += vbCrLf + " INTO TEMPISSUE"
        StrSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING "
        StrSql += vbCrLf + " WHERE TRANDATE <= @DATE AND RECPAY = 'P' AND ISNULL(SETTLED,'') <> 'Y'"
        StrSql += vbCrLf + " AND ACCODE = '" & Accode & "' AND ISNULL(CANCEL,'') = ''"
        StrSql += vbCrLf + " AND " & IIf(modeOfPending = PendingMode.Weight, "ISNULL(PUREWT,0)", "ISNULL(AMOUNT,0)") & " <> 0"
        StrSql += vbCrLf + " "
        StrSql += vbCrLf + " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPRECEIPT')>0 DROP TABLE TEMPRECEIPT"
        StrSql += vbCrLf + " SELECT TRANDATE,TRANNO"
        StrSql += vbCrLf + " ," & IIf(modeOfPending = PendingMode.Weight, "PUREWT", "AMOUNT") & " AS VALUE"
        StrSql += vbCrLf + " ,ACCODE,SETTLED,SNO "
        StrSql += vbCrLf + " INTO TEMPRECEIPT"
        StrSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING "
        StrSql += vbCrLf + " WHERE TRANDATE <= @DATE AND RECPAY = 'R'"
        StrSql += vbCrLf + " AND ACCODE = '" & Accode & "' AND ISNULL(CANCEL,'') = '' AND ISNULL(SETTLED,'') <> 'Y' "
        StrSql += vbCrLf + " AND " & IIf(modeOfPending = PendingMode.Weight, "ISNULL(PUREWT,0)", "ISNULL(AMOUNT,0)") & " <> 0"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        GenBalance()
        StrSql = vbCrLf + " SELECT TRANDATE,TRANNO,SUM(ISNULL(VALUE,0)-ISNULL(RECVALUE,0))VALUE,SNO"
        StrSql += vbCrLf + " FROM TEMPISSUE "
        StrSql += vbCrLf + " GROUP BY TRANDATE,TRANNO,SNO"
        StrSql += vbCrLf + " HAVING SUM(ISNULL(VALUE,0)-ISNULL(RECVALUE,0)) <> 0"
        StrSql += vbCrLf + " ORDER BY TRANDATE,TRANNO"
        DtGrid = New DataTable
        DtGrid.Columns.Add("CHECK", GetType(Boolean))
        DtGrid.Columns("CHECK").DefaultValue = CType(False, Boolean)
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(DtGrid)
        gridView.DataSource = DtGrid
        With gridView
            .Columns("CHECK").Width = 40
            .Columns("CHECK").HeaderText = ""

            .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("TRANDATE").Width = 100
            .Columns("TRANDATE").SortMode = DataGridViewColumnSortMode.NotSortable

            .Columns("TRANNO").Width = 70
            .Columns("TRANNO").SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns("TRANNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            .Columns("VALUE").Width = 100
            .Columns("VALUE").SortMode = DataGridViewColumnSortMode.NotSortable
            .Columns("VALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SNO").Visible = False
        End With
        If gridView.RowCount > 0 Then
            gridView.Select()
            gridView.CurrentCell = gridView.FirstDisplayedCell
        End If

        ' ''Values insert to Temp Disc Table
        'For Each dgvRow As DataGridViewRow In gridView.Rows
        '    StrSql = vbCrLf + " INSERT INTO " & cnStockDb & ".." & strTabName & " (TranNo ,TranDate,ItemId  ,ItemName ,TagNo,Pcs,GrsWt,NetWt,PureWt,Touch,Rate,Amount,DisPer,DisPure,NetPure,DisVal,Mode) "
        '    StrSql += vbCrLf + " SELECT TranNo,TranDate,ItemId,(Select ItemName from " & cnAdminDb & "..ITEMMAST where ItemId=T.ItemId) as ItemName, "
        '    StrSql += vbCrLf + " TAGNO,PCS,GRSWT,NETWT,PUREWT,TOUCH,RATE,AMOUNT,NULL,NULL,NULL,NULL,NULL "
        '    StrSql += vbCrLf + " from " & cnStockDb & "..ISSUE as T WHERE TranNo=" & dgvRow.Cells("TranNo").Value
        '    StrSql += vbCrLf + " and TranDate='" & Format(dgvRow.Cells("TranDate").Value, "yyyy-MM-dd") & "'"
        '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '    Cmd.ExecuteNonQuery()
        'Next

    End Sub

    Private Sub GenBalance()
        StrSql = vbCrLf + " DECLARE @I_VALUE NUMERIC(15,3)        "
        StrSql += vbCrLf + " DECLARE @I_TRANDATE SMALLDATETIME"
        StrSql += vbCrLf + " DECLARE @I_TRANNO INT"
        StrSql += vbCrLf + " DECLARE @I_SNO VARCHAR(12)"
        StrSql += vbCrLf + " DECLARE @TEMPVALUE NUMERIC(15,3)    "
        StrSql += vbCrLf + " DECLARE @R_ACCODE VARCHAR(7)"
        StrSql += vbCrLf + " DECLARE @R_TRANNO INT"
        StrSql += vbCrLf + " DECLARE @R_SNO VARCHAR(12)"
        StrSql += vbCrLf + " DECLARE @R_VALUE NUMERIC(15,3)"
        StrSql += vbCrLf + " DECLARE @R_TRANDATE SMALLDATETIME"
        StrSql += vbCrLf + " DECLARE R_CUR CURSOR FOR SELECT TRANDATE,ACCODE,VALUE,TRANNO,SNO FROM TEMPRECEIPT ORDER BY TRANDATE,ACCODE"
        StrSql += vbCrLf + " OPEN R_CUR"
        StrSql += vbCrLf + " WHILE 1=1 BEGIN"
        StrSql += vbCrLf + " FETCH NEXT FROM R_CUR INTO @R_TRANDATE,@R_ACCODE,@R_VALUE,@R_TRANNO,@R_SNO"
        StrSql += vbCrLf + " 	IF @@FETCH_STATUS = -1 BREAK"
        StrSql += vbCrLf + " 	DECLARE I_CUR CURSOR FOR SELECT VALUE - ISNULL(RECVALUE,0) AS VALUE,TRANDATE,TRANNO,SNO FROM TEMPISSUE WHERE VALUE - ISNULL(RECVALUE,0) > 0 AND ACCODE = @R_ACCODE AND ISNULL(SETTLED,'') = 'Y'"
        StrSql += vbCrLf + "  	OPEN I_CUR"
        StrSql += vbCrLf + " 	WHILE 1 = 1"
        StrSql += vbCrLf + " 	BEGIN"
        StrSql += vbCrLf + " 		FETCH NEXT FROM I_CUR INTO @I_VALUE,@I_TRANDATE,@I_TRANNO,@I_SNO"
        StrSql += vbCrLf + " 		IF @@FETCH_STATUS = -1 BREAK"
        StrSql += vbCrLf + " 		SELECT @TEMPVALUE = 0"
        StrSql += vbCrLf + " 		IF @R_VALUE <> 0"
        StrSql += vbCrLf + " 		BEGIN"
        StrSql += vbCrLf + "  			IF @I_VALUE > @R_VALUE"
        StrSql += vbCrLf + " 				BEGIN"
        StrSql += vbCrLf + " 				SELECT @TEMPVALUE = @R_VALUE"
        StrSql += vbCrLf + " 				SELECT @R_VALUE = 0"
        StrSql += vbCrLf + " 				END"
        StrSql += vbCrLf + " 			ELSE"
        StrSql += vbCrLf + "   				BEGIN"
        StrSql += vbCrLf + " 				SELECT @TEMPVALUE = @I_VALUE"
        StrSql += vbCrLf + " 				SELECT @R_VALUE = @R_VALUE - @I_VALUE"
        StrSql += vbCrLf + " 				END"
        StrSql += vbCrLf + " 			UPDATE TEMPISSUE SET RECVALUE = isnull(RECVALUE,0) + @TEMPVALUE,RECDATE = @R_TRANDATE  "
        StrSql += vbCrLf + "   			WHERE CURRENT OF I_CUR  			"
        StrSql += vbCrLf + " 			IF @R_VALUE = 0 BREAK"
        StrSql += vbCrLf + " 		END"
        StrSql += vbCrLf + " 	END"
        StrSql += vbCrLf + "  	IF @R_VALUE <> 0 "
        StrSql += vbCrLf + "   	     BEGIN"
        StrSql += vbCrLf + "   	     INSERT INTO TEMPISSUE(ACCODE,VALUE,RECVALUE,RECDATE,TRANDATE,TRANNO,SNO)VALUES(@R_ACCODE,0,@R_VALUE,@R_TRANDATE,@R_TRANDATE,@R_TRANNO,@R_SNO)"
        StrSql += vbCrLf + "   	     END"
        StrSql += vbCrLf + " 	CLOSE I_CUR"
        StrSql += vbCrLf + " 	DEALLOCATE I_CUR"
        StrSql += vbCrLf + " END"
        StrSql += vbCrLf + " CLOSE R_CUR"
        StrSql += vbCrLf + " DEALLOCATE R_CUR"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
    Private Sub frmBillReceitBalView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView.Focused Then
                Exit Sub
            End If
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub gridView_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellValueChanged
        Try
            If gridView.CurrentRow Is Nothing Then Exit Sub
            Dim rwIndex As Integer = gridView.CurrentRow.Index
            With gridView.Rows(rwIndex)
                If CType(.Cells("CHECK").Value, Boolean) Then
                    If ModeOfPending = PendingMode.Weight Then
                        txtDetWeight.Text = Format(Val(txtDetWeight.Text) + Val(gridView.Rows(rwIndex).Cells("VALUE").Value.ToString), "0.000")
                    Else
                        txtDetAmount.Text = Format(Val(txtDetAmount.Text) + Val(gridView.Rows(rwIndex).Cells("VALUE").Value.ToString), "0.00")
                    End If
                    'If strDiscTabName <> "" Then
                    '    Call SHOW_DISCOUNTFORM(Val(.Cells("TRANNO").Value.ToString), .Cells("TRANDATE").Value, True)
                    'End If
                Else
                    If ModeOfPending = PendingMode.Weight Then
                        txtDetWeight.Text = Format(Val(txtDetWeight.Text) - Val(gridView.Rows(rwIndex).Cells("VALUE").Value.ToString), "0.000")
                    Else
                        txtDetAmount.Text = Format(Val(txtDetAmount.Text) - Val(gridView.Rows(rwIndex).Cells("VALUE").Value.ToString), "0.00")
                    End If
                    'If strDiscTabName <> "" Then
                    '    Call SHOW_DISCOUNTFORM(Val(.Cells("TranNo").Value.ToString), .Cells("TranDate").Value, False)
                    'End If
                End If
            End With
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub SHOW_DISCOUNTFORM(ByVal billno As Integer, ByVal billdate As Date, ByVal checkBox As Boolean)
        Dim objDiscount As W_PendingBillDiscount
        If checkBox = True Then
            objDiscount = New W_PendingBillDiscount(billno, billdate, strDiscTabName)
            objDiscount.ShowDialog()
        Else
            StrSql = " UPDATE " & cnStockDb & ".." & strDiscTabName & " SET DISVAL=NULL WHERE    TRANNO=" & billno & " AND TRANDATE='" & Format(billdate, "yyyy-MM-dd") & "'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If
    End Sub
    Private Sub gridView_CurrentCellDirtyStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.CurrentCellDirtyStateChanged
        If gridView.IsCurrentCellDirty Then
            gridView.CommitEdit(DataGridViewDataErrorContexts.Commit)
        End If
    End Sub
    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            If ModeOfPending = PendingMode.Weight Then txtEditTotWt_WET.Select() Else txtEditTotAMT_AMT.Select()
        End If
    End Sub
    Private Sub gridView_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.SelectionChanged
        If gridView.CurrentCell.ColumnIndex <> 0 Then
            gridView.CurrentCell = gridView.Rows(gridView.CurrentCell.RowIndex).Cells(0)
        End If
    End Sub
    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        'For cnt As Integer = 0 To gridView.RowCount - 1
        '    If CType(gridView.Rows(cnt).Cells(0).Value, Boolean) Then
        '        _DtBalanceViewPending.Rows(cnt).Item("FLAG") = "Y"
        '    Else
        '        _DtBalanceViewPending.Rows(cnt).Item("FLAG") = " "
        '    End If
        'Next
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
    Private Sub frmBillReceitBalView_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not gridView.RowCount > 0 Then Me.Close()
    End Sub
    Private Sub txtEditTotWt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtEditTotWt_WET.GotFocus
        If Val(txtDetWeight.Text) = 0 Or Val(txtEditTotAMT_AMT.Text) <> 0 Then
            txtEditTotWt_WET.Clear()
            SendKeys.Send("{TAB}")
        Else
            txtEditTotWt_WET.Text = txtDetWeight.Text
        End If
    End Sub
    Private Sub txtEditTotAmt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtEditTotAMT_AMT.GotFocus
        If Val(txtDetAmount.Text) = 0 Or Val(txtEditTotWt_WET.Text) <> 0 Then
            txtEditTotAMT_AMT.Clear()
            SendKeys.Send("{TAB}")
        Else
            txtEditTotAMT_AMT.Text = txtDetAmount.Text
        End If
    End Sub

    Private Sub txtEditTotWt_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtEditTotWt_WET.KeyPress
        WeightValidation(txtEditTotWt_WET, e)
    End Sub

    Private Sub txtEditTotAMT_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtEditTotAMT_AMT.KeyPress
        AmountValidation(txtEditTotAMT_AMT, e)
    End Sub

    Private Sub GotFocusColor(ByVal sender As Object, ByVal e As EventArgs) Handles txtEditTotAMT_AMT.GotFocus, txtEditTotWt_WET.GotFocus
        CType(sender, TextBox).BackColor = GlobalVariables.focusColor
    End Sub
    Private Sub LostFocusColor(ByVal sender As Object, ByVal e As EventArgs) Handles txtEditTotAMT_AMT.LostFocus, txtEditTotWt_WET.LostFocus
        CType(sender, TextBox).BackColor = GlobalVariables.lostFocusColor
    End Sub

    Private Sub gridView_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellContentClick

    End Sub
End Class