Imports System.Data.OleDb
Public Class frmReqItem
    Dim strSql As String
    Dim dtTemp As New DataTable
    Dim dtReq As New DataTable
    Dim dtReqStone As New DataTable
    Dim dtStone As New DataTable

    Dim editFlag As Boolean = False

    Dim editGeneral As Boolean = False
    Dim editRw As Integer = Nothing
    Dim cmd As OleDbCommand
    Dim dr As OleDbDataReader
    Dim tran As OleDbTransaction
    Function funcGetOpenViewDetails(ByVal ReqNo As Integer) As Integer
        funcNew()
        strSql = " SELECT "
        strSql += " (SELECT ITEMNAME FROM " & CNADMINDB & "..ITEMMAST WHERE ITEMID = R.ITEMID)AS ITEM"
        strSql += " ,ISNULL((SELECT SUBITEMNAME FROM " & CNADMINDB & "..SUBITEMMAST WHERE SUBITEMID = R.SUBITEMID),'')SUBITEM"
        strSql += " ,PCS,GRSWT,NETWT,ISSUESTATUS,REMARK1,REQNO,ITEMID,SUBITEMID,SNO"
        strSql += " FROM " & CNSTOCKDB & "..REQITEM R"
        strSql += " WHERE REQNO = " & ReqNo & ""
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtReq)

        strSql = " SELECT "
        strSql += " (SELECT ITEMNAME FROM " & CNADMINDB & "..ITEMMAST WHERE ITEMID = R.STNITEMID)AS STNITEM"
        strSql += " ,STNPCS"
        strSql += " ,STNWT,STONEUNIT,REQNO,STNITEMID,STNSUBITEMID,(SELECT DIASTONE FROM " & CNADMINDB & "..ITEMMAST WHERE ITEMID = R.STNITEMID)DIASTONE,ISSSNO SNO "
        strSql += " FROM " & CNSTOCKDB & "..REQITEMSTONE R"
        strSql += " WHERE REQNO = " & ReqNo & ""
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtReqStone)
    End Function

    Function funcInsertIntoReqItem(ByVal rwIndex As Integer, ByVal ReqNo As Integer) As Integer
        strSql = " INSERT INTO " & CNSTOCKDB & "..REQITEM"
        strSql += " ("
        strSql += " SNO,REQNO,ITEMID,SUBITEMID,GRSWT,NETWT"
        strSql += " ,ISSUESTATUS,USERID,UPDATED,UPTIME"
        strSql += " ,PCS,REMARK1,APPVER"
        strSql += " )"
        strSql += " VALUES"
        strSql += " ("
        With gridView.Rows(rwIndex)
            strSql += " '" & GetNewSno(TranSnoType.REQITEMCODE, tran) & "'" 'SNO
            strSql += " ," & ReqNo & "" 'REQNO
            strSql += " ," & Val(.Cells("ITEMID").Value.ToString) & "" 'ITEMID
            strSql += " ," & Val(.Cells("SUBITEMID").Value.ToString) & "" 'SUBITEMID
            strSql += " ," & Val(.Cells("GRSWT").Value.ToString) & "" 'GRSWT
            strSql += " ," & Val(.Cells("NETWT").Value.ToString) & "" 'NETWT
            strSql += " ,''" 'ISSUESTATUS
            strSql += " ," & UserId & "" 'USERID
            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
            strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
            strSql += " ," & Val(.Cells("PCS").Value.ToString) & "" 'PCS
            strSql += " ,'" & .Cells("REMARK1").Value.ToString & "'" 'REMARK1
            strSql += " ,'" & VERSION & "'" 'APPVER
        End With
        strSql += " )"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
    End Function

    Function funcInserIntoReqItemStone(ByVal RwIndex As Integer, ByVal ReqNo As Integer) As Integer
        Dim issSno As String = Nothing
        ''Find IssSno
        strSql = " Select COUNT(*) as Sno from " & cnStockDb & "..reqItem"
        cmd = New OleDbCommand(strSql, cn, tran)
        dr = cmd.ExecuteReader
        If dr.Read = True Then
            issSno = cnCostId & dr.Item("Sno").ToString
        End If
        dr.Close()

        Dim dv As DataView
        Dim dt As New DataTable
        dv = dtReqStone.DefaultView
        dv.RowFilter = "SNO = " + gridView.Rows(RwIndex).Cells("SNO").Value.ToString
        dt = dv.ToTable
        For cnt As Integer = 0 To dt.Rows.Count - 1
            With dt.Rows(cnt)
                strSql = " INSERT INTO " & cnStockDb & "..REQITEMSTONE"
                strSql += " ("
                strSql += " ISSSNO,REQNO,STNITEMID,STNSUBITEMID,STNWT,USERID"
                strSql += " ,UPDATED,UPTIME,STNPCS,STONEUNIT,APPVER"
                strSql += " )"
                strSql += " VALUES"
                strSql += " ("
                strSql += " '" & issSno & "'" 'ISSSNO
                strSql += " ," & ReqNo & "" 'REQNO
                strSql += " ," & Val(.Item("STNITEMID").ToString) & "" 'STNITEMID
                strSql += " ," & Val(.Item("STNSUBITEMID").ToString) & "" 'STNSUBITEMID
                strSql += " ," & Val(.Item("STNWT").ToString) & "" 'STNWT
                strSql += " ," & userId & "" 'USERID
                strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                strSql += " ," & Val(.Item("STNPCS").ToString) & "" 'STNPCS
                strSql += " ,'" & .Item("STONEUNIT").ToString & "'" 'STONEUNIT
                strSql += " ,'" & VERSION & "'" 'APPVER
                strSql += " )"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
            End With
        Next
    End Function


    Function funcSave() As Integer
        Dim reqNo As Integer = Nothing
        Try
            tran = cn.BeginTransaction
            If editFlag = False Then
                ''FIND REQNO
                strSql = " SELECT ISNULL(MAX(REQNO),0)+1 AS REQNO FROM " & cnStockDb & "..REQITEM"
                reqNo = Val(objGPack.GetSqlValue(strSql, , , tran))
            Else
                reqNo = Val(dtReq.Rows(0).Item("REQNO").ToString)
                ''DELETING OLD RECORDS
                strSql = " DELETE FROM " & cnStockDb & "..REQITEM WHERE REQNO = " & reqNo & ""
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()

                strSql = " DELETE FROM " & cnStockDb & "..REQITEMSTONE WHERE REQNO = " & reqNo & ""
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
            End If
            For rwIndex As Integer = 0 To gridView.RowCount - 1
                funcInsertIntoReqItem(rwIndex, reqNo)
                funcInserIntoReqItemStone(rwIndex, reqNo)
            Next
            tran.Commit()
            funcNew()
            MsgBox(reqNo.ToString + E0012)
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox("MESSAGE :" + ex.Message + vbCrLf + "STACKTRACE :" + ex.StackTrace)
        End Try
    End Function

    Function funcRemovingStnDetails(ByVal sno As String) As Integer
        Dim dv As DataView
        dv = dtReqStone.DefaultView
        dv.RowFilter = "SNO <> " + sno
        dtReqStone = dv.ToTable
    End Function

    Function funcGetDetails(ByVal curRow As Integer) As Integer
        If Not gridView.RowCount > 0 Then
            Exit Function
        End If
        With gridView.Rows(curRow)
            cmbItem_Man.Text = .Cells("ITEM").Value.ToString
            If .Cells("SUBITEM").Value.ToString <> "" Then
                cmbSubItem_Man.Enabled = True
                cmbSubItem_Man.Text = .Cells("SUBITEM").Value.ToString
            Else
                cmbSubItem_Man.Enabled = False
                cmbSubItem_Man.Text = ""
            End If
            txtPcs_Num.Text = .Cells("PCS").Value.ToString
            txtGrsWt_Wet.Text = .Cells("GRSWT").Value.ToString
            txtNetWt_Wet.Text = .Cells("NETWT").Value.ToString
            txtRemark.Text = .Cells("REMARK1").Value.ToString

            dtStone.Rows.Clear()
            Dim dv As DataView
            Dim dtTempStone As New DataTable
            dtTempStone = dtReqStone.Copy
            dv = dtTempStone.DefaultView
            dv.RowFilter = "Sno = " + .Cells("SNO").Value.ToString
            dtStone = dv.ToTable
            gridStnView.DataSource = dtStone
            If gridStnView.RowCount > 0 Then
                pnlStoneDetails.Visible = True
            Else
                pnlStoneDetails.Visible = False
            End If
            btnAdd.Text = "&Update"
            cmbItem_Man.Focus()
        End With
    End Function

    Function funcNew() As Integer
        objGPack.TextClear(Me)
        editGeneral = False
        editRw = Nothing
        dtReq.Rows.Clear()
        dtReqStone.Rows.Clear()
        dtStone.Rows.Clear()
        cmbItem_Man.Text = ""
        cmbSubItem_Man.Enabled = True
        cmbSubItem_Man.Text = ""
        cmbStnUnit.Text = "GRAM"
        pnlStoneDetails.Visible = False
        btnAdd.Text = "&Add"
        cmbItem_Man.Focus()
    End Function

    Function funcClear() As Integer
        btnAdd.Text = "&Add"
        pnlStoneDetails.Visible = False
        editGeneral = False
        editRw = Nothing
        cmbItem_Man.Text = ""
        cmbSubItem_Man.Enabled = True
        cmbSubItem_Man.Text = ""
        txtPcs_Num.Clear()
        txtGrsWt_Wet.Clear()
        txtNetWt_Wet.Clear()
        txtRemark.Clear()
        dtStone.Clear()
        cmbItem_Man.Focus()
    End Function

    Function funcStnCalcWeight() As String
        Dim wt As Double
        For cnt As Integer = 0 To gridStnView.Rows.Count - 1
            If gridStnView.Rows(cnt).Cells("STONEUNIT").Value.ToString = "C" Then
                wt += Val(gridStnView.Rows(cnt).Cells("STNWT").Value.ToString) / 5
            Else
                wt += Val(gridStnView.Rows(cnt).Cells("STNWT").Value.ToString)
            End If
        Next
        If wt <> 0 Then
            Return Format(wt, "0.000")
        End If
        Return ""
    End Function

    Function funcStdClear() As Integer
        cmbStnItem_Man.Text = ""
        txtStnSubItem.Clear()
        txtStnPcs_Num.Clear()
        txtStnWeight_Wet.Clear()
        cmbStnUnit.Text = "GRAM"
    End Function

    Private Sub frmReqItem_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Escape) Then
            If tabMain.SelectedTab.Name = tabGeneral.Name Then
                Dim netWt As Double = Nothing
                netWt = Val(txtGrsWt_Wet.Text) - Val(funcStnCalcWeight)
                If netWt <> 0 Then
                    txtNetWt_Wet.Text = Format(netWt, "0.000")
                    txtRemark.Focus()
                Else
                    txtNetWt_Wet.Clear()
                    txtNetWt_Wet.Focus()
                End If
            Else
                tabMain.SelectedTab = tabGeneral
                btnNew.Focus()
            End If
        ElseIf e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmReqItem_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabMain.ItemSize = New System.Drawing.Size(60, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))

        strSql = " SELECT ''ITEM,''SUBITEM,''PCS,''GRSWT,''NETWT,''ISSUESTATUS,''REMARK1,''REQNO,''ITEMID,''SUBITEMID,''SNO"
        strSql += " WHERE 1<>1"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtReq)
        gridView.DataSource = dtReq
        With gridView
            With .Columns("ITEM")
                .Width = 300
            End With
            With .Columns("SUBITEM")
                .Width = 250
            End With
            With .Columns("PCS")
                .Width = 60
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("GRSWT")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("NETWT")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            For CNT As Integer = 5 To gridView.Columns.Count - 1
                .Columns(CNT).Visible = False
            Next
        End With

        strSql = " SELECT ''STNITEM,''STNPCS,''STNWT,''STONEUNIT,''REQNO,''STNITEMID,''STNSUBITEMID,''DIASTONE,''SNO "
        strSql += " WHERE 1<>1"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtStone)
        dtReqStone = dtStone.Copy
        gridStnView.DataSource = dtStone
        With gridStnView
            With .Columns("STNITEM")
                .Width = 242
            End With
            With .Columns("STNPCS")
                .Width = 52
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("STNWT")
                .Width = 90
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("STONEUNIT")
                .Width = 90
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            For CNT As Integer = 4 To gridStnView.Columns.Count - 1
                .Columns(CNT).Visible = False
            Next
        End With

        ''load itemname
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST ORDER BY ITEMNAME"
        objGPack.FillCombo(strSql, cmbItem_Man)

        ''load itemname
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE METALID IN ('D','T') ORDER BY ITEMNAME"
        objGPack.FillCombo(strSql, cmbStnItem_Man)

        cmbStnUnit.Items.Add("CARAT")
        cmbStnUnit.Items.Add("GRAM")

        funcNew()
    End Sub


    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If cmbItem_Man.Text = "" Then
            MsgBox(Me.GetNextControl(cmbItem_Man, False).Text + E0001, MsgBoxStyle.Information)
            cmbItem_Man.Select()
            Exit Sub
        End If
        If Not cmbItem_Man.Items.Contains(cmbItem_Man.Text) Then
            MsgBox(E0004 + Me.GetNextControl(cmbItem_Man, False).Text, MsgBoxStyle.Information)
            cmbItem_Man.Select()
            Exit Sub
        End If
        If cmbSubItem_Man.Enabled Then
            If cmbSubItem_Man.Text = "" Then
                MsgBox(Me.GetNextControl(cmbSubItem_Man, False).Text + E0001, MsgBoxStyle.Information)
                cmbSubItem_Man.Select()
                Exit Sub
            End If
            If Not cmbSubItem_Man.Items.Contains(cmbSubItem_Man.Text) Then
                MsgBox(E0004 + Me.GetNextControl(cmbSubItem_Man, False).Text, MsgBoxStyle.Information)
                cmbSubItem_Man.Select()
                Exit Sub
            End If
        End If
        If Val(txtPcs_Num.Text) = 0 And Val(txtGrsWt_Wet.Text) = 0 Then
            MsgBox(Me.GetNextControl(txtPcs_Num, False).Text + _
            "," + Me.GetNextControl(txtGrsWt_Wet, False).Text + E0001, MsgBoxStyle.Information)
            txtPcs_Num.Focus()
            Exit Sub
        End If

        Dim ro As DataRow = Nothing
        Dim itemId As Integer = Nothing
        Dim subItemId As Integer = Nothing

        strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_Man.Text & "'"
        itemId = Val(objGPack.GetSqlValue(strSql))
        strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "' and itemid = " & itemId & ""
        subItemId = Val(objGPack.GetSqlValue(strSql))

        If editGeneral = False Then
            ro = dtReq.NewRow
            ro("ITEM") = cmbItem_Man.Text
            ro("SUBITEM") = cmbSubItem_Man.Text
            ro("PCS") = txtPcs_Num.Text
            ro("GRSWT") = txtGrsWt_Wet.Text
            ro("NETWT") = txtNetWt_Wet.Text
            ro("ISSUESTATUS") = ""
            ro("REMARK1") = txtRemark.Text
            ro("REQNO") = ""
            ro("ITEMID") = itemId
            ro("SUBITEMID") = subItemId
            ro("SNO") = dtReq.Rows.Count + 1
            dtReq.Rows.Add(ro)
        ElseIf editRw <> Nothing Then
            With dtReq.Rows(editRw - 1)
                .Item("ITEM") = cmbItem_Man.Text
                .Item("SUBITEM") = cmbSubItem_Man.Text
                .Item("PCS") = txtPcs_Num.Text
                .Item("GRSWT") = txtGrsWt_Wet.Text
                .Item("NETWT") = txtNetWt_Wet.Text
                .Item("ISSUESTATUS") = ""
                .Item("REMARK1") = txtRemark.Text
                .Item("ITEMID") = itemId
                .Item("SUBITEMID") = subItemId
                funcRemovingStnDetails(.Item("SNO"))
            End With
        End If

        ''Adding Stone Details
        For cnt As Integer = 0 To dtStone.Rows.Count - 1
            dtReqStone.ImportRow(dtStone.Rows(cnt))
        Next
        dtReqStone.AcceptChanges()
        funcClear()
    End Sub

    Private Sub cmbStnUnit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbStnUnit.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If objGPack.Validator_Check(pnlStoneDetails) Then Exit Sub
            If Not (Val(txtStnPcs_Num.Text) > 0 And Val(txtStnWeight_Wet.Text) > 0) Then
                MsgBox(Me.GetNextControl(txtStnPcs_Num, False).Text + "," + _
                Me.GetNextControl(txtStnWeight_Wet, False).Text + E0001, MsgBoxStyle.Information)
                txtStnPcs_Num.Focus()
                Exit Sub
            End If
            Dim totWt As Double = Val(txtStnWeight_Wet.Text)
            For cnt As Integer = 0 To gridStnView.RowCount - 1
                With gridStnView.Rows(cnt)
                    If .Cells("STONEUNIT").Value.ToString = "C" Then
                        totWt += Val(.Cells("STNWT").Value.ToString) / 5
                    ElseIf .Cells("STONEUNIT").Value.ToString = "G" Then
                        totWt += Val(.Cells("STNWT").Value.ToString)
                    End If
                End With
            Next
            If totWt > Val(txtGrsWt_Wet.Text) Then
                MsgBox(Me.GetNextControl(txtStnWeight_Wet, False).Text + E0010 + Val(txtGrsWt_Wet.Text), MsgBoxStyle.Information)
                txtStnWeight_Wet.Select()
                Exit Sub
            End If


            Dim ro As DataRow = Nothing
            Dim itemId As Integer = Nothing
            Dim subItemId As Integer = Nothing
            Dim diaStone As String = Nothing


            strSql = " SELECT ITEMID,DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbStnItem_Man.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            dtTemp.Clear()
            da.Fill(dtTemp)
            If dtTemp.Rows.Count > 0 Then
                itemId = Val(dtTemp.Rows(0).Item("ITEMID").ToString)
                diaStone = dtTemp.Rows(0).Item("DIASTONE").ToString
            End If

            strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtStnSubItem.Text & "' and itemid = " & itemId & ""
            dtTemp.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTemp)
            If dtTemp.Rows.Count > 0 Then
                subItemId = Val(dtTemp.Rows(0).Item("SUBITEMID").ToString)
            End If
            ro = dtStone.NewRow
            If txtStnSubItem.Text = "" Then
                ro("STNITEM") = cmbStnItem_Man.Text
            Else
                ro("STNITEM") = txtStnSubItem.Text
            End If
            ro("STNPCS") = txtStnPcs_Num.Text
            ro("STNWT") = txtStnWeight_Wet.Text
            ro("STONEUNIT") = Mid(cmbStnUnit.Text, 1, 1)
            ro("REQNO") = ""
            ro("STNITEMID") = itemId
            ro("STNSUBITEMID") = subItemId
            ro("DIASTONE") = diaStone
            ro("SNO") = dtReq.Rows.Count + 1
            dtStone.Rows.Add(ro)
            funcStdClear()
            pnlStoneDetails.Focus()
        End If
    End Sub


    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        funcClear()
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblStatus1.Visible = True
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridView.RowCount > 0 Then
                gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells(0)
            End If
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            funcGetDetails(gridView.CurrentRow.Index)
            editGeneral = True
            editRw = gridView.CurrentRow.Index + 1
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        strSql = " SELECT REQNO"
        strSql += " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = R.ITEMID)AS ITEM"
        strSql += " ,ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = R.SUBITEMID),'')SUBITEM"
        strSql += " ,PCS,GRSWT,NETWT,ISSUESTATUS,REMARK1,ITEMID,SUBITEMID,SNO"
        strSql += " FROM " & CNSTOCKDB & "..REQITEM AS R"
        strSql += " WHERE ISSUESTATUS <> 'Y'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        gridOpenView.DataSource = dt
        If dt.Rows.Count > 0 Then
            With gridOpenView
                With .Columns("REQNO")
                    .Width = 60
                End With
                With .Columns("ITEM")
                    .Width = 275
                End With
                With .Columns("SUBITEM")
                    .Width = 250
                End With
                With .Columns("PCS")
                    .Width = 60
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                With .Columns("GRSWT")
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                With .Columns("NETWT")
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
                .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                For CNT As Integer = 6 To gridOpenView.Columns.Count - 1
                    .Columns(CNT).Visible = False
                Next
            End With
        End If
        tabMain.SelectedTab = tabView
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        btnSave_Click(Me, New EventArgs)
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

    Private Sub gridOpenView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridOpenView.GotFocus
        lblStatus2.Visible = True
    End Sub

    Private Sub gridOpenView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridOpenView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridOpenView.RowCount > 0 Then
                gridOpenView.CurrentCell = gridOpenView.Rows(gridOpenView.CurrentRow.Index).Cells(0)
            End If
        End If
    End Sub

    Private Sub gridOpenView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridOpenView.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            funcGetOpenViewDetails(gridOpenView.Rows(gridOpenView.CurrentRow.Index).Cells("REQNO").Value.ToString)
            editFlag = True
            tabMain.SelectedTab = tabGeneral
            gridView.Focus()
        End If
    End Sub

    Private Sub cmbItem_Man_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItem_Man.SelectedIndexChanged
        strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = "
        strSql += " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_Man.Text & "')"
        objGPack.FillCombo(strSql, cmbSubItem_Man)
        If Not cmbSubItem_Man.Items.Count > 0 Then
            cmbSubItem_Man.Text = ""
            cmbSubItem_Man.Enabled = False
        Else
            cmbSubItem_Man.Enabled = True
        End If

        strSql = " SELECT 1 FROM " & cnAdminDb & "..ITEMMAST WHERE STUDDEDSTONE = 'Y' AND ITEMNAME = '" & cmbItem_Man.Text & "'"
        dtTemp = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTemp)
        If dtTemp.Rows.Count > 0 Then
            pnlStoneDetails.Visible = True
        Else
            pnlStoneDetails.Visible = False
        End If
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        lblStatus1.Visible = False
    End Sub

    Private Sub gridOpenView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridOpenView.LostFocus
        lblStatus2.Visible = False
    End Sub

    Private Sub gridView_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridView.UserDeletedRow
        dtReq.AcceptChanges()
        If Not gridView.RowCount > 0 Then btnNew.Select()
    End Sub

    Private Sub txtStnWeight_Wet_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStnWeight_Wet.KeyPress
        Dim totWt As Double = Val(txtStnWeight_Wet.Text)
        For cnt As Integer = 0 To gridStnView.RowCount - 1
            With gridStnView.Rows(cnt)
                If .Cells("STONEUNIT").Value.ToString = "C" Then
                    totWt += Val(.Cells("STNWT").Value.ToString) / 5
                ElseIf .Cells("STONEUNIT").Value.ToString = "G" Then
                    totWt += Val(.Cells("STNWT").Value.ToString)
                End If
            End With
        Next
        If totWt > Val(txtGrsWt_Wet.Text) Then
            MsgBox(Me.GetNextControl(txtStnWeight_Wet, False).Text + E0010 + Val(txtGrsWt_Wet.Text), MsgBoxStyle.Information)
            txtStnWeight_Wet.Select()
        End If
    End Sub
End Class