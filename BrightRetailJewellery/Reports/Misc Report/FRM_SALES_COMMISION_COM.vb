Imports System.Data.OleDb
Public Class FRM_SALES_COMMISION_COM
    Dim StrSql As String
    Dim Da As OleDbDataAdapter
    Dim Cmd As OleDbCommand
    Dim objGridShower As frmGridDispDia

    Private Sub FRM_SALES_COMMISION_COM_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Dgv.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        ElseIf e.KeyCode = Keys.F12 Then
            btnExit_Click(Me, New EventArgs)
        ElseIf e.KeyCode = Keys.F3 Then
            btnNew_Click(Me, New EventArgs)
        End If
    End Sub
    Private Sub FRM_SALES_COMMISION_COM_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GrpContainer.Location = New Point((ScreenWid - GrpContainer.Width) / 2, ((ScreenHit - 128) - GrpContainer.Height) / 2)
        StrSql = " SELECT METALID,METALNAME,CONVERT(NUMERIC(15,2),1000.00) AS COMM_BAS,CONVERT(NUMERIC(15,2),NULL)COMM_AMT FROM " & cnAdminDb & "..METALMAST ORDER BY DISPLAYORDER,METALNAME"
        Dim DtMetal As New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(DtMetal)
        Dgv.DataSource = DtMetal

        Dgv.Columns("METALID").Visible = False
        Dgv.Columns("METALNAME").Width = 159
        Dgv.Columns("COMM_BAS").Width = 120
        Dgv.Columns("COMM_AMT").Width = 120
        Dgv.Columns("METALNAME").ReadOnly = True
        Dgv.Columns("COMM_BAS").ReadOnly = False
        Dgv.Columns("COMM_AMT").ReadOnly = False

        Dgv.Columns("COMM_BAS").DefaultCellStyle.Format = "0.00"
        Dgv.Columns("COMM_AMT").DefaultCellStyle.Format = "0.00"

        Dgv.Columns("COMM_BAS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Dgv.Columns("COMM_AMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight


        Dgv.Columns("METALNAME").HeaderText = "METAL"
        Dgv.Columns("COMM_BAS").HeaderText = "COMMISION ON"
        Dgv.Columns("COMM_AMT").HeaderText = "COMMISION AMT"

        Dgv.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        If Dgv.RowCount > 0 Then
            Dgv.CurrentCell = Dgv.Rows(0).Cells("COMM_AMT")
        End If
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Prop_Sets()
        StrSql = " IF OBJECT_ID('MASTER..TEMP" & systemId & "METALMAST') IS NOT NULL DROP TABLE MASTER..TEMP" & systemId & "METALMAST"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        StrSql = " CREATE TABLE MASTER..TEMP" & systemId & "METALMAST"
        StrSql += " ("
        StrSql += " METALNAME VARCHAR(50)"
        StrSql += " ,COMM_BAS NUMERIC(15,2)"
        StrSql += " ,COMM_AMT NUMERIC(15,2)"
        StrSql += " )"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        For cnt As Integer = 0 To Dgv.RowCount - 1
            StrSql = " INSERT INTO MASTER..TEMP" & systemId & "METALMAST"
            StrSql += " (METALNAME,COMM_BAS,COMM_AMT)"
            StrSql += " SELECT"
            StrSql += " '" & Dgv.Rows(cnt).Cells("METALNAME").Value.ToString & "'"
            StrSql += " ," & Val(Dgv.Rows(cnt).Cells("COMM_BAS").Value.ToString) & ""
            StrSql += " ," & Val(Dgv.Rows(cnt).Cells("COMM_AMT").Value.ToString) & ""
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        Next

        StrSql = " EXEC " & cnStockDb & "..SP_RPT_SALESCOMMISION_COM"
        StrSql += " @COMM_TBLNAME = 'MASTER..TEMP" & systemId & "METALMAST'"
        StrSql += " ,@FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += " ,@DETAILED = '" & IIf(rbtDetailed.Checked, "Y", "N") & "'"
        StrSql += " ,@ORDER_EMPID = '" & IIf(chkOrderbyEmpId.Checked, "Y", "N") & "'"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.CommandTimeout = 1000

        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        Da = New OleDbDataAdapter(Cmd)
        Da.Fill(dtGrid)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        'Prop_Sets()
        If Not dtGrid.Rows.Count > 0 Then
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
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        Prop_Gets()
        dtpFrom.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub Dgv_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Dgv.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Dgv.CurrentRow Is Nothing Then
                Exit Sub
            End If
            If Dgv.CurrentRow.Index = Dgv.RowCount - 1 Then
                SendKeys.Send("{TAB}")
            Else
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub Dgv_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Dgv.SelectionChanged
        If Dgv.CurrentRow Is Nothing Then Exit Sub
        If Dgv.CurrentCell.ColumnIndex = Dgv.Columns("METALNAME").Index Then
            Dgv.CurrentCell = Dgv.CurrentRow.Cells("COMM_BAS")
        End If
    End Sub

    Private Sub Dgv_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Dgv.EditingControlShowing
        'This event is fired when the user tries to edit the content of a cell: 
        If (Me.Dgv.CurrentCell.ColumnIndex = Dgv.Columns("COMM_AMT").Index Or Me.Dgv.CurrentCell.ColumnIndex = Dgv.Columns("COMM_BAS").Index) And Not e.Control Is Nothing Then
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

            '.Columns("SPCS").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            '.Columns("SNETWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            '.Columns("SAMOUNT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            '.Columns("RPCS").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            '.Columns("RNETWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            '.Columns("RAMOUNT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            '.Columns("PCS").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            '.Columns("NETWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            '.Columns("AMOUNT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            '.Columns("INCENTIVE").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)

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

            .Columns("METALNAME").HeaderText = "METAL"
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
    Private Sub Prop_Gets()
        Dim obj As New FRM_SALES_COMMISION_COM_Properties
        GetSettingsObj(obj, Me.Name, GetType(FRM_SALES_COMMISION_COM_Properties))
        If obj.p_DtMetalComm IsNot Nothing Then
            If obj.p_DtMetalComm.Rows.Count > 0 Then
                For i As Integer = 0 To Dgv.RowCount - 1
                    Dim Ro() As DataRow = obj.p_DtMetalComm.Select("METALID = '" & Dgv.Rows(i).Cells("METALID").Value.ToString & "'", "METALID")
                    If Ro IsNot Nothing Then
                        If Ro.Length > 0 Then
                            Dgv.Rows(i).Cells("COMM_BAS").Value = Val(Ro(0).Item("COMM_BAS").ToString)
                            Dgv.Rows(i).Cells("COMM_AMT").Value = Val(Ro(0).Item("COMM_AMT").ToString)
                        End If
                    End If
                Next
            End If
        End If
        chkOrderbyEmpId.Checked = obj.p_chkOrderbyEmpId
        rbtSummary.Checked = obj.p_rbtSummary
        rbtDetailed.Checked = obj.p_rbtDetailed
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New FRM_SALES_COMMISION_COM_Properties
        Dim dt As New DataTable
        dt = CType(Dgv.DataSource, DataTable)
        dt.TableName = "METALCOMMISION"
        obj.p_DtMetalComm = dt
        obj.p_chkOrderbyEmpId = chkOrderbyEmpId.Checked
        obj.p_rbtSummary = rbtSummary.Checked
        obj.p_rbtDetailed = rbtDetailed.Checked
        SetSettingsObj(obj, Me.Name, GetType(FRM_SALES_COMMISION_COM_Properties))
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
End Class