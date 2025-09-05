Imports System.Data.OleDb
Public Class FRM_SALES_COMMISION_COM
    Dim StrSql As String
    Dim Da As OleDbDataAdapter
    Dim Cmd As OleDbCommand
    Dim objGridShower As frmGridDispDia

    Private Sub FRM_SALES_COMMISION_COM_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            If DgvMetal.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        ElseIf e.KeyCode = Keys.F12 Then
            btnExit_Click(Me, New EventArgs)
        ElseIf e.KeyCode = Keys.F3 Then
            btnNew_Click(Me, New EventArgs)
        End If
    End Sub
    Private Sub FRM_SALES_COMMISION_COM_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GrpContainer.Location = New Point((ScreenWid - GrpContainer.Width) / 2, ((ScreenHit - 128) - GrpContainer.Height) / 2)

        'LOAD COMPANY
        StrSql = vbCrLf + " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT COMPANYNAME,CONVERT(VARCHAR,COMPANYID),2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        StrSql += vbCrLf + " ORDER BY RESULT,COMPANYNAME"
        Da = New OleDbDataAdapter(StrSql, cn)
        Dim dtCompany As New DataTable()
        Da.Fill(dtCompany)
        ChkcmbCompany.Items.Clear()
        BrighttechPack.GlobalMethods.FillCombo(ChkcmbCompany, dtCompany, "COMPANYNAME", , "ALL")

        'For Metal Commision
        StrSql = " SELECT METALID,METALNAME,CONVERT(NUMERIC(15,2),1000.00) AS COMM_BAS,CONVERT(NUMERIC(15,2),NULL)COMM_AMT FROM " & cnAdminDb & "..METALMAST ORDER BY DISPLAYORDER,METALNAME"
        Dim DtMetal As New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(DtMetal)
        DgvMetal.DataSource = DtMetal

        DgvMetal.Columns("METALID").Visible = False
        DgvMetal.Columns("METALNAME").Width = 159
        DgvMetal.Columns("COMM_BAS").Width = 120
        DgvMetal.Columns("COMM_AMT").Width = 120
        DgvMetal.Columns("METALNAME").ReadOnly = True
        DgvMetal.Columns("COMM_BAS").ReadOnly = False
        DgvMetal.Columns("COMM_AMT").ReadOnly = False
        DgvMetal.Columns("COMM_BAS").DefaultCellStyle.Format = "0.00"
        DgvMetal.Columns("COMM_AMT").DefaultCellStyle.Format = "0.00"
        DgvMetal.Columns("COMM_BAS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DgvMetal.Columns("COMM_AMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DgvMetal.Columns("METALNAME").HeaderText = "METAL"
        DgvMetal.Columns("COMM_BAS").HeaderText = "COMMISION ON"
        DgvMetal.Columns("COMM_AMT").HeaderText = "COMMISION AMT"

        DgvMetal.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        If DgvMetal.RowCount > 0 Then
            DgvMetal.CurrentCell = DgvMetal.Rows(0).Cells("COMM_AMT")
        End If
        'For Category Commision
        StrSql = " SELECT CGROUPID,CGROUPNAME AS CATGROUP"
        StrSql += " ,CONVERT (VARCHAR(20),'')AS COMM_BASTYPE ,CONVERT(NUMERIC(15,2),1000.00) AS COMM_BAS"
        StrSql += " ,CONVERT (VARCHAR(20),'')AS COMM_AMTTYPE ,CONVERT(NUMERIC(15,2),NULL)COMM_AMT "
        StrSql += " FROM " & cnAdminDb & "..CATEGORYGROUP ORDER BY CGROUPNAME"
        Dim DtCategory As New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(DtCategory)
        DgvCategory.DataSource = DtCategory

        DgvCategory.Columns("CGROUPID").Visible = False
        DgvCategory.Columns("CATGROUP").Width = 150
        DgvCategory.Columns("COMM_BASTYPE").Width = 80
        DgvCategory.Columns("COMM_AMTTYPE").Width = 80
        DgvCategory.Columns("COMM_BAS").Width = 100
        DgvCategory.Columns("COMM_AMT").Width = 100
        DgvCategory.Columns("CATGROUP").ReadOnly = True
        DgvCategory.Columns("COMM_BASTYPE").ReadOnly = False
        DgvCategory.Columns("COMM_AMTTYPE").ReadOnly = False
        DgvCategory.Columns("COMM_BAS").ReadOnly = False
        DgvCategory.Columns("COMM_AMT").ReadOnly = False
        DgvCategory.Columns("COMM_BAS").DefaultCellStyle.Format = "0.00"
        DgvCategory.Columns("COMM_AMT").DefaultCellStyle.Format = "0.00"
        DgvCategory.Columns("COMM_BAS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DgvCategory.Columns("COMM_AMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        DgvCategory.Columns("CATGROUP").HeaderText = "CATEGORY"
        DgvCategory.Columns("COMM_BASTYPE").HeaderText = "COMMISION ON TYPE"
        DgvCategory.Columns("COMM_AMTTYPE").HeaderText = "COMMISION AMT TYPE"
        DgvCategory.Columns("COMM_BAS").HeaderText = "COMMISION ON"
        DgvCategory.Columns("COMM_AMT").HeaderText = "COMMISION AMT"

        DgvCategory.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        'If DgvCategory.RowCount > 0 Then
        '    DgvCategory.CurrentCell = DgvCategory.Rows(0).Cells("COMM_AMT")
        'End If
        btnNew_Click(Me, New EventArgs)
        DgvCategory.Columns("CGROUPID").Visible = False
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Prop_Sets()
        Dim selCompany As String = Nothing
        If ChkcmbCompany.Text = "ALL" Then
            'selCompany = "ALL"
            Dim sql As String = "SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY  WHERE ISNULL(ACTIVE,'')<>'N'"
            Dim dtCompany As New DataTable()
            Da = New OleDbDataAdapter(sql, cn)
            Da.Fill(dtCompany)
            If dtCompany.Rows.Count > 0 Then
                'selItemId = "'"
                For i As Integer = 0 To dtCompany.Rows.Count - 1
                    selCompany += dtCompany.Rows(i).Item("COMPANYID").ToString + ","
                Next
                If selCompany <> "" Then
                    selCompany = Mid(selCompany, 1, selCompany.Length - 1)
                End If
                'selItemId += "'"
            End If
        ElseIf ChkcmbCompany.Text <> "" Then
            Dim sql As String = "SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(ChkcmbCompany.Text) & ")"
            Dim dtCompany As New DataTable()
            Da = New OleDbDataAdapter(sql, cn)
            Da.Fill(dtCompany)
            If dtCompany.Rows.Count > 0 Then
                'selItemId = "'"
                For i As Integer = 0 To dtCompany.Rows.Count - 1
                    selCompany += dtCompany.Rows(i).Item("COMPANYID").ToString + ","
                Next
                If selCompany <> "" Then
                    selCompany = Mid(selCompany, 1, selCompany.Length - 1)
                End If
                'selItemId += "'"
            End If
        End If
        If rbtMetalWise.Checked Then
            StrSql = " IF OBJECT_ID('TEMPTABLEDB..TEMP" & systemId & "METALMAST') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & systemId & "METALMAST"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

            StrSql = " CREATE TABLE TEMPTABLEDB..TEMP" & systemId & "METALMAST"
            StrSql += " ("
            StrSql += " METALNAME VARCHAR(50)"
            StrSql += " ,COMM_BAS NUMERIC(15,2)"
            StrSql += " ,COMM_AMT NUMERIC(15,2)"
            StrSql += " )"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

            For cnt As Integer = 0 To DgvMetal.RowCount - 1
                StrSql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "METALMAST"
                StrSql += " (METALNAME,COMM_BAS,COMM_AMT)"
                StrSql += " SELECT"
                StrSql += " '" & DgvMetal.Rows(cnt).Cells("METALNAME").Value.ToString & "'"
                StrSql += " ," & Val(DgvMetal.Rows(cnt).Cells("COMM_BAS").Value.ToString) & ""
                StrSql += " ," & Val(DgvMetal.Rows(cnt).Cells("COMM_AMT").Value.ToString) & ""
                Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
            Next
            StrSql = " EXEC " & cnStockDb & "..SP_RPT_SALESCOMMISION_COM"
            StrSql += " @COMM_TBLNAME = 'TEMPTABLEDB..TEMP" & systemId & "METALMAST'"
            StrSql += " ,@FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += " ,@DETAILED = '" & IIf(rbtDetailed.Checked, "Y", "N") & "'"
            StrSql += " ,@ORDER_EMPID = '" & IIf(chkOrderbyEmpId.Checked, "Y", "N") & "'"
            StrSql += " ,@COMPANYID=""" & selCompany & """"
            StrSql += " ,@DBNAME='" & cnAdminDb & "'"
            StrSql += " ,@TYPE='M'"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.CommandTimeout = 1000

        End If
        If rbtCategorywise.Checked Then
            StrSql = " IF OBJECT_ID('TEMPTABLEDB..TEMP" & systemId & "CATGROUP') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & systemId & "CATGROUP"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

            StrSql = " CREATE TABLE TEMPTABLEDB..TEMP" & systemId & "CATGROUP"
            StrSql += " ("
            StrSql += " CATGROUP VARCHAR(50)"
            StrSql += " ,COMM_BASTYPE VARCHAR(50)"
            StrSql += " ,COMM_BAS NUMERIC(15,2)"
            StrSql += " ,COMM_AMTTYPE VARCHAR(50)"
            StrSql += " ,COMM_AMT NUMERIC(15,2)"
            StrSql += " )"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

            For cnt As Integer = 0 To DgvCategory.RowCount - 1
                StrSql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "CATGROUP"
                StrSql += " (CATGROUP,COMM_BASTYPE,COMM_BAS,COMM_AMTTYPE,COMM_AMT)"
                StrSql += " SELECT"
                StrSql += " '" & DgvCategory.Rows(cnt).Cells("CATGROUP").Value.ToString & "'"
                StrSql += " ,'" & DgvCategory.Rows(cnt).Cells("COMM_BASTYPE").Value.ToString & "'"
                StrSql += " ," & Val(DgvCategory.Rows(cnt).Cells("COMM_BAS").Value.ToString) & ""
                StrSql += " ,'" & DgvCategory.Rows(cnt).Cells("COMM_AMTTYPE").Value.ToString & "'"
                StrSql += " ," & Val(DgvCategory.Rows(cnt).Cells("COMM_AMT").Value.ToString) & ""
                Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
            Next
            StrSql = " EXEC " & cnStockDb & "..SP_RPT_SALESCOMMISION_COM"
            StrSql += " @COMM_TBLNAME = 'TEMPTABLEDB..TEMP" & systemId & "CATGROUP'"
            StrSql += " ,@FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += " ,@DETAILED = '" & IIf(rbtDetailed.Checked, "Y", "N") & "'"
            StrSql += " ,@ORDER_EMPID = '" & IIf(chkOrderbyEmpId.Checked, "Y", "N") & "'"
            StrSql += " ,@COMPANYID=""" & selCompany & """"
            StrSql += " ,@DBNAME='" & cnAdminDb & "'"
            StrSql += " ,@TYPE='C'"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.CommandTimeout = 1000
        End If


        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        If Not Cmd Is Nothing Then
            Da = New OleDbDataAdapter(Cmd)
            Da.Fill(dtGrid)
            dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
            'Prop_Sets()
            If Not dtGrid.Rows.Count > 1 Then
                MsgBox("Record not found", MsgBoxStyle.Information)
                Exit Sub
            End If
            objGridShower = New frmGridDispDia
            objGridShower.Name = Me.Name
            objGridShower.gridView.RowTemplate.Height = 21
            objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
            objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            objGridShower.Text = "SALES PERSON COMMISION REPORT"
            Dim tit As String = "SALES PERSON COMMISION REPORT" + vbCrLf
            tit += " DATE FROM " & dtpFrom.Text + " TO " + dtpTo.Text
            objGridShower.lblTitle.Text = tit
            AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged
            objGridShower.StartPosition = FormStartPosition.CenterScreen
            objGridShower.dsGrid.DataSetName = objGridShower.Name
            objGridShower.dsGrid.Tables.Add(dtGrid)
            objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
            objGridShower.FormReSize = False
            objGridShower.FormReLocation = False
            objGridShower.pnlFooter.Visible = False
            objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            DataGridView_SummaryFormatting(objGridShower.gridView)
            FormatGridColumns(objGridShower.gridView, False, False, , False)

            objGridShower.Show()
            objGridShower.FormReSize = True
            objGridShower.FormReLocation = True
            objGridShower.gridViewHeader.Visible = True
            GridViewHeaderCreator(objGridShower.gridViewHeader)
            FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
            objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("PARTICULAR")))
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        Prop_Gets()
        If rbtMetalWise.Checked = False And rbtCategorywise.Checked = False Then
            rbtMetalWise.Checked = True
        End If
        dtpFrom.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub Dgv_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DgvMetal.KeyDown
        If e.KeyCode = Keys.Enter Then
            If DgvMetal.CurrentRow Is Nothing Then
                Exit Sub
            End If
            If DgvMetal.CurrentRow.Index = DgvMetal.RowCount - 1 Then
                SendKeys.Send("{TAB}")
            Else
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub Dgv_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DgvMetal.SelectionChanged
        If DgvMetal.CurrentRow Is Nothing Then Exit Sub
        If DgvMetal.CurrentCell.ColumnIndex = DgvMetal.Columns("METALNAME").Index Then
            DgvMetal.CurrentCell = DgvMetal.CurrentRow.Cells("COMM_BAS")
        End If
    End Sub

    Private Sub Dgv_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles DgvMetal.EditingControlShowing
        'This event is fired when the user tries to edit the content of a cell: 
        If (Me.DgvMetal.CurrentCell.ColumnIndex = DgvMetal.Columns("COMM_AMT").Index Or Me.DgvMetal.CurrentCell.ColumnIndex = DgvMetal.Columns("COMM_BAS").Index) And Not e.Control Is Nothing Then
            Dim tb As TextBox = CType(e.Control, TextBox)
            '---add an event handler to the TextBox control---
            AddHandler tb.KeyPress, AddressOf TextBox_KeyPress
        End If
        'If Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("ProducedQty").Index And Not e.Control Is Nothing Then
        '    Dim tb As TextBox = CType(e.Control, TextBox)
        '    '---add an event handler to the TextBox control---
        '    AddHandler tb.KeyPress, AddressOf TextBox_KeyPress
        'End If
    End Sub

    Private Sub TextBox_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = "." And CType(sender, TextBox).Text.Contains(".") Then
            e.Handled = True
            Return
            Exit Sub
        End If
        Select Case e.KeyChar
            Case "+", "-", "0" To "9", ChrW(Keys.Back), ".", _
            ChrW(Keys.Enter), ChrW(Keys.Escape)
            Case Else
                e.Handled = True
                MsgBox("Digits only Allowed 0 to 9", MsgBoxStyle.Information)
                CType(sender, TextBox).Focus()
                Return
        End Select
        If CType(sender, TextBox).Text.Contains(".") Then
            Dim dotPos As Integer = InStr(CType(sender, TextBox).Text, ".", CompareMethod.Text)
            Dim sp() As String = CType(sender, TextBox).Text.Split(".")
            Dim curPos As Integer = CType(sender, TextBox).SelectionStart
            If sp.Length >= 2 Then
                If curPos >= dotPos Then
                    If sp(1).Length > 1 Then
                        e.Handled = True
                        Return
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs)
        SetGridHeadColWidth(CType(sender, DataGridView))
    End Sub

    Private Sub GridViewHeaderCreator(ByVal gridviewHead As DataGridView)
        Dim dtHead As New DataTable
        StrSql = "SELECT ''[PARTICULAR]"
        StrSql += " ,''[EMPID]"
        StrSql += " ,''[METALNAME]"
        StrSql += " ,''[SPCS~SNETWT~SAMOUNT]"
        StrSql += " ,''[RPCS~RNETWT~RAMOUNT]"
        StrSql += " ,''[PCS~NETWT~AMOUNT]"
        StrSql += " ,''[INCENTIVE],''SCROLL"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtHead)

        gridviewHead.DataSource = dtHead
        gridviewHead.Columns("PARTICULAR").HeaderText = ""
        gridviewHead.Columns("EMPID").HeaderText = ""
        gridviewHead.Columns("METALNAME").HeaderText = ""
        gridviewHead.Columns("SPCS~SNETWT~SAMOUNT").HeaderText = "SALES"
        gridviewHead.Columns("RPCS~RNETWT~RAMOUNT").HeaderText = "RETURN"
        gridviewHead.Columns("PCS~NETWT~AMOUNT").HeaderText = "ACTUAL"
        gridviewHead.Columns("INCENTIVE").HeaderText = ""
        gridviewHead.Columns("SCROLL").HeaderText = ""
        gridviewHead.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        SetGridHeadColWidth(gridviewHead)
    End Sub
    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = False
            Next
            .Columns("PARTICULAR").Width = 200
            .Columns("EMPID").Width = 60
            .Columns("METALNAME").Width = 100
            .Columns("SPCS").Width = 60
            .Columns("SNETWT").Width = 80
            .Columns("SAMOUNT").Width = 100
            .Columns("RPCS").Width = 60
            .Columns("RNETWT").Width = 80
            .Columns("RAMOUNT").Width = 100
            .Columns("PCS").Width = 60
            .Columns("NETWT").Width = 80
            .Columns("AMOUNT").Width = 100
            .Columns("INCENTIVE").Width = 100

            .Columns("PARTICULAR").Visible = True
            .Columns("EMPID").Visible = True
            .Columns("METALNAME").Visible = True
            .Columns("SPCS").Visible = True
            .Columns("SNETWT").Visible = True
            .Columns("SAMOUNT").Visible = True
            .Columns("RPCS").Visible = True
            .Columns("RNETWT").Visible = True
            .Columns("RAMOUNT").Visible = True
            .Columns("PCS").Visible = True
            .Columns("NETWT").Visible = True
            .Columns("AMOUNT").Visible = True
            .Columns("INCENTIVE").Visible = True
            If rbtMetalWise.Checked Then
                .Columns("METALNAME").HeaderText = "METAL"
            Else
                .Columns("METALNAME").HeaderText = "CATEGROY"
            End If
            .Columns("SPCS").HeaderText = "PCS"
            .Columns("SNETWT").HeaderText = "NETWT"
            .Columns("SAMOUNT").HeaderText = "AMOUNT"
            .Columns("RPCS").HeaderText = "PCS"
            .Columns("RNETWT").HeaderText = "NETWT"
            .Columns("RAMOUNT").HeaderText = "AMOUNT"
            .Columns("PCS").HeaderText = "PCS"
            .Columns("NETWT").HeaderText = "NETWT"
            .Columns("AMOUNT").HeaderText = "AMOUNT"
            FormatGridColumns(dgv, False, False, , False)
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub


    Private Sub rbtMetalWise_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtMetalWise.CheckedChanged
        If rbtMetalWise.Checked Then
            DgvMetal.Visible = True
            DgvCategory.Visible = False
        End If
    End Sub
    Private Sub rbtCategorywise_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtCategorywise.CheckedChanged
        If rbtCategorywise.Checked Then
            DgvMetal.Visible = False
            DgvCategory.Visible = True
        End If
    End Sub

    Private Sub DgvCategory_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DgvCategory.CellEnter
        Dim pt As Point = Panel2.Location  'DgvCategory.Location
        Select Case DgvCategory.Columns(e.ColumnIndex).Name
            Case "COMM_BASTYPE"
                CmbSearch.Items.Clear()
                If DgvCategory.CurrentRow.Cells("COMM_BASTYPE").Value.ToString <> "" Then
                    CmbSearch.Size = New Size(DgvCategory.Columns(e.ColumnIndex).Width, CmbSearch.Height)
                    CmbSearch.Location = pt + DgvCategory.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Location
                    loadLstSearch()
                    CmbSearch.Text = DgvCategory.CurrentCell.FormattedValue
                    CmbSearch.Visible = True
                    CmbSearch.Focus()
                Else
                    CmbSearch.Items.Clear()
                    CmbSearch.Size = New Size(DgvCategory.Columns(e.ColumnIndex).Width, CmbSearch.Height)
                    CmbSearch.Location = pt + DgvCategory.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Location
                    loadLstSearch()
                    CmbSearch.Text = DgvCategory.CurrentCell.FormattedValue
                    CmbSearch.Visible = True
                    CmbSearch.Focus()
                    CmbSearch.BackColor = Color.White
                End If
            Case "COMM_BAS"
                txtSearch.Clear()
                txtSearch.Size = New Size(DgvCategory.Columns(e.ColumnIndex).Width, txtSearch.Height)
                txtSearch.Location = pt + DgvCategory.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Location
                txtSearch.Text = DgvCategory.CurrentCell.FormattedValue
                txtSearch.Visible = True
                txtSearch.Focus()
            Case "COMM_AMTTYPE"
                CmbSearch.Items.Clear()
                If DgvCategory.CurrentRow.Cells("COMM_AMTTYPE").Value.ToString <> "" Then
                    CmbSearch.Size = New Size(DgvCategory.Columns(e.ColumnIndex).Width, CmbSearch.Height)
                    CmbSearch.Location = pt + DgvCategory.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Location
                    loadLstSearch()
                    CmbSearch.Text = DgvCategory.CurrentCell.FormattedValue
                    CmbSearch.Visible = True
                    CmbSearch.Focus()
                Else
                    CmbSearch.Items.Clear()
                    CmbSearch.Size = New Size(DgvCategory.Columns(e.ColumnIndex).Width, CmbSearch.Height)
                    CmbSearch.Location = pt + DgvCategory.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Location
                    loadLstSearch()
                    CmbSearch.Text = DgvCategory.CurrentCell.FormattedValue
                    CmbSearch.Visible = True
                    CmbSearch.Focus()
                    CmbSearch.BackColor = Color.White
                End If
            Case "COMM_AMT"
                txtSearch.Clear()
                txtSearch.Size = New Size(DgvCategory.Columns(e.ColumnIndex).Width, txtSearch.Height)
                txtSearch.Location = pt + DgvCategory.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Location
                txtSearch.Text = DgvCategory.CurrentCell.FormattedValue
                txtSearch.Visible = True
                txtSearch.Focus()
        End Select
    End Sub

    Private Sub DgvCategory_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DgvCategory.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        ElseIf e.KeyCode = Keys.Escape Then
            btnSearch.Focus()
        End If
    End Sub

    Private Sub DgvCategory_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles DgvCategory.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            DgvCategory.CurrentCell = DgvCategory.Rows(DgvCategory.CurrentRow.Index).Cells(DgvCategory.CurrentCell.ColumnIndex)
        End If
    End Sub


    Private Sub CmbSearch_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles CmbSearch.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            With DgvCategory
                Select Case .Columns(.CurrentCell.ColumnIndex).Name
                    Case "COMM_BASTYPE"
                        DgvCategory.CurrentCell.Value = CmbSearch.Text
                        CmbSearch.Text = ""
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells("COMM_BAS")
                        .CurrentCell.Selected = True
                    Case "COMM_AMTTYPE"
                        DgvCategory.CurrentCell.Value = CmbSearch.Text
                        CmbSearch.Text = ""
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells("COMM_AMT")
                        .CurrentCell.Selected = True
                End Select
                CmbSearch.Visible = False
            End With
        End If
    End Sub
    Private Sub loadLstSearch()
        CmbSearch.Items.Clear()
        With DgvCategory
            Select Case .Columns(.CurrentCell.ColumnIndex).Name
                Case "COMM_BASTYPE"
                    CmbSearch.Items.Add("WT(GM)")
                    CmbSearch.Items.Add("AMOUNT")
                    CmbSearch.Items.Add("PCS")
                Case "COMM_AMTTYPE"
                    CmbSearch.Items.Add("AMOUNT")
                    If .CurrentRow.Cells("COMM_BASTYPE").Value.ToString = "AMOUNT" Then
                        CmbSearch.Items.Add("PER(%)")
                    End If
            End Select
        End With
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New FRM_SALES_COMMISION_COM_Properties
        Dim dtMetal As New DataTable
        dtMetal = CType(DgvMetal.DataSource, DataTable)
        dtMetal.TableName = "METALCOMMISION"
        obj.p_DtMetalComm = dtMetal
        obj.p_rbtMetalwise = rbtMetalWise.Checked
        Dim dtCategory As New DataTable
        dtCategory = CType(DgvCategory.DataSource, DataTable)
        dtCategory.TableName = "CATEGORYCOMMISION"
        obj.p_DtCategoryComm = dtCategory
        obj.p_rbtCategorywise = rbtCategorywise.Checked
        obj.p_chkOrderbyEmpId = chkOrderbyEmpId.Checked
        obj.p_rbtSummary = rbtSummary.Checked
        obj.p_rbtDetailed = rbtDetailed.Checked
        SetSettingsObj(obj, Me.Name, GetType(FRM_SALES_COMMISION_COM_Properties))
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New FRM_SALES_COMMISION_COM_Properties
        GetSettingsObj(obj, Me.Name, GetType(FRM_SALES_COMMISION_COM_Properties))
        If obj.p_DtMetalComm IsNot Nothing Then
            If obj.p_DtMetalComm.Rows.Count > 0 Then
                For i As Integer = 0 To DgvMetal.RowCount - 1
                    Dim Ro() As DataRow = obj.p_DtMetalComm.Select("METALID = '" & DgvMetal.Rows(i).Cells("METALID").Value.ToString & "'", "METALID")
                    If Ro IsNot Nothing Then
                        If Ro.Length > 0 Then
                            DgvMetal.Rows(i).Cells("COMM_BAS").Value = Val(Ro(0).Item("COMM_BAS").ToString)
                            DgvMetal.Rows(i).Cells("COMM_AMT").Value = Val(Ro(0).Item("COMM_AMT").ToString)
                        End If
                    End If
                Next
            End If
        End If
        If obj.p_DtCategoryComm IsNot Nothing Then
            If obj.p_DtCategoryComm.Rows.Count > 0 Then
                For i As Integer = 0 To DgvCategory.RowCount - 1
                    Dim Ro() As DataRow = obj.p_DtCategoryComm.Select("CGROUPID = '" & DgvCategory.Rows(i).Cells("CGROUPID").Value.ToString & "'", "CGROUPID")
                    If Ro IsNot Nothing Then
                        If Ro.Length > 0 Then
                            DgvCategory.Rows(i).Cells("COMM_BASTYPE").Value = Ro(0).Item("COMM_BASTYPE").ToString
                            DgvCategory.Rows(i).Cells("COMM_BAS").Value = Val(Ro(0).Item("COMM_BAS").ToString)
                            DgvCategory.Rows(i).Cells("COMM_AMTTYPE").Value = Ro(0).Item("COMM_AMTTYPE").ToString
                            DgvCategory.Rows(i).Cells("COMM_AMT").Value = Val(Ro(0).Item("COMM_AMT").ToString)
                        End If
                    End If
                Next
            End If
        End If
        rbtMetalWise.Checked = obj.p_rbtMetalwise
        rbtCategorywise.Checked = obj.p_rbtCategorywise
        chkOrderbyEmpId.Checked = obj.p_chkOrderbyEmpId
        rbtSummary.Checked = obj.p_rbtSummary
        rbtDetailed.Checked = obj.p_rbtDetailed
    End Sub

    Private Sub txtSearch_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSearch.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            With DgvCategory
                Select Case .Columns(.CurrentCell.ColumnIndex).Name
                    Case "COMM_BAS"
                        DgvCategory.CurrentCell.Value = txtSearch.Text
                        .CurrentCell = .Rows(.CurrentCell.RowIndex).Cells("COMM_AMTTYPE")
                        .CurrentCell.Selected = True
                    Case "COMM_AMT"
                        DgvCategory.CurrentCell.Value = txtSearch.Text
                        If .CurrentCell.RowIndex = .RowCount - 1 Then
                            btnSearch.Focus()
                        Else
                            .CurrentCell = .Rows(.CurrentCell.RowIndex + 1).Cells("COMM_BASTYPE")
                            .CurrentCell.Selected = True
                        End If

                End Select
                txtSearch.Visible = False
            End With
        End If
    End Sub
End Class
Public Class FRM_SALES_COMMISION_COM_Properties
    Private DtMetalComm As New DataTable("METALCOMMISION")
    Public Property p_DtMetalComm() As DataTable
        Get
            Return DtMetalComm
        End Get
        Set(ByVal value As DataTable)
            DtMetalComm = value
        End Set
    End Property
    Private DtCategoryComm As New DataTable("CATEGORYCOMMISION")
    Public Property p_DtCategoryComm() As DataTable
        Get
            Return DtCategoryComm
        End Get
        Set(ByVal value As DataTable)
            DtCategoryComm = value
        End Set
    End Property
    Private chkOrderbyEmpId As Boolean = True
    Public Property p_chkOrderbyEmpId() As Boolean
        Get
            Return chkOrderbyEmpId
        End Get
        Set(ByVal value As Boolean)
            chkOrderbyEmpId = value
        End Set
    End Property
    Private rbtDetailed As Boolean = False
    Public Property p_rbtDetailed() As Boolean
        Get
            Return rbtDetailed
        End Get
        Set(ByVal value As Boolean)
            rbtDetailed = value
        End Set
    End Property
    Private rbtSummary As Boolean = True
    Public Property p_rbtSummary() As Boolean
        Get
            Return rbtSummary
        End Get
        Set(ByVal value As Boolean)
            rbtSummary = value
        End Set
    End Property
    Private rbtMetalwise As Boolean = False
    Public Property p_rbtMetalwise() As Boolean
        Get
            Return rbtMetalwise
        End Get
        Set(ByVal value As Boolean)
            rbtMetalwise = value
        End Set
    End Property
    Private rbtCategorywise As Boolean = False
    Public Property p_rbtCategorywise() As Boolean
        Get
            Return rbtCategorywise
        End Get
        Set(ByVal value As Boolean)
            rbtCategorywise = value
        End Set
    End Property
End Class