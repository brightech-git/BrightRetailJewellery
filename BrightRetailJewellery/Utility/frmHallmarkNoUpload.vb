Imports System.Data.OleDb
Imports System.IO
Imports System.Globalization
Public Class frmHallmarkNoUpload
    Dim strConn As String
    Dim strSql As String
    Dim dtGridView As New DataTable
    Dim dsExceelDatas As New DataSet
    Dim myExcel As Excel.Application
    Dim ukCultureInfo As New CultureInfo("en-GB")

    Function funcBarStep() As Integer
        If pbar.Value > pbar.Maximum - 5 Then
            pbar.Value = 0
        Else
            pbar.Value = pbar.Value + 5
        End If
        Me.Refresh()
    End Function
    Function funcArrageExcellColumns() As Integer
        Dim dtTemp As New DataTable
        Dim code As Integer = 0
        Try
            Dim Temprow() As DataRow
            Dim colHuId As String = ""
            Dim colItemid As String = ""
            Dim colTagno As String = ""
            Dim colGrsWt As String = ""
            Dim TempTagSno As String = ""
            Dim HALLMARKSno As String = ""
            Dim TagSno As String = ""
            dtTemp = New DataTable
            dtTemp = dtGridView.Copy
            Temprow = Nothing
            Temprow = dtTemp.Select("HALLMARK_COLUMN='HUID'", Nothing)
            colHuId = IIf(Temprow(0).Item("EXCEL_COLUMN").ToString <> "", Temprow(0).Item("EXCEL_COLUMN").ToString, "")
            Temprow = Nothing
            Temprow = dtTemp.Select("HALLMARK_COLUMN='GRSWT'", Nothing)
            colGrsWt = IIf(Temprow(0).Item("EXCEL_COLUMN").ToString <> "", Temprow(0).Item("EXCEL_COLUMN").ToString, "")
            Temprow = Nothing
            Temprow = dtTemp.Select("HALLMARK_COLUMN='TAGNO'", Nothing)
            colTagno = IIf(Temprow(0).Item("EXCEL_COLUMN").ToString <> "", Temprow(0).Item("EXCEL_COLUMN").ToString, "")
            Temprow = Nothing
            Temprow = dtTemp.Select("HALLMARK_COLUMN='ITEMID'", Nothing)
            colItemid = IIf(Temprow(0).Item("EXCEL_COLUMN").ToString <> "", Temprow(0).Item("EXCEL_COLUMN").ToString, "")
            If colGrsWt = "" Or colTagno = "" Or colHuId = "" Or colItemid = "" Then
                MsgBox("Should match all columns", MsgBoxStyle.Information)
                Exit Function
            End If
            TempTagSno = ""
            tran = Nothing
            tran = cn.BeginTransaction
            pbar.Visible = True
            pbar.Value = 0
            For CNT As Integer = 0 To dsExceelDatas.Tables(0).Rows.Count - 1
                HALLMARKSno = ""
                TagSno = ""
                If dsExceelDatas.Tables(0).Columns.Contains("CHECK") Then
                    If dsExceelDatas.Tables(0).Rows(CNT).Item("CHECK").ToString <> "" Then Continue For
                End If
                HALLMARKSno = GetNewSno(TranSnoType.ITEMTAGHALLMARKCODE, tran, "GET_ADMINSNO_TRAN")

                strSql = " SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE "
                strSql += "ITEMID = '" & dsExceelDatas.Tables(0).Rows(CNT).Item(colItemid).ToString & "'"
                strSql += " AND TAGNO = '" & dsExceelDatas.Tables(0).Rows(CNT).Item(colTagno).ToString & "'"
                TagSno = GetSqlValue(strSql, cn, tran)

                If TempTagSno.ToString <> TagSno Then
                    TempTagSno = TagSno
                    ''DELETING ITEMTAGHALLMARK
                    strSql = " DELETE FROM " & cnAdminDb & "..ITEMTAGHALLMARK WHERE TAGSNO='" & TagSno.ToString & "'"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                End If
                ''INSERTING ITEMTAGHALLMARK
                With dsExceelDatas.Tables(0).Rows(CNT)
                    strSql = " INSERT INTO " & cnAdminDb & "..ITEMTAGHALLMARK"
                    strSql += " ("
                    strSql += " SNO,TAGSNO,GRSWT,HM_BILLNO,USERID,SYSTEMID,APPVER,UPDATED,UPTIME,COSTID,COMPANYID"
                    strSql += " )VALUES("
                    strSql += " '" & HALLMARKSno & "'" 'SNO
                    strSql += " ,'" & TagSno & "'" 'TAGSNO
                    strSql += " ,'" & Val(.Item(colGrsWt).ToString) & "'" 'GRSWT
                    strSql += " ,'" & .Item(colHuId).ToString & "'" 'HM_BILLNO
                    strSql += " ," & userId & "" 'USERID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                    strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                    strSql += " ,'" & cnCostId & "'" 'COSTID
                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    strSql += " )"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                    code += 1
SkipData:           funcBarStep()
                End With
            Next
            tran.Commit()
            pbar.Value = pbar.Maximum
            pbar.Visible = False
            MsgBox("Imported ...", MsgBoxStyle.Information)
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            tran = Nothing
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Function
    Private Sub btnPath_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPath.Click
        Dim openDia As New OpenFileDialog
        Dim loadstr As String = ""
        loadstr = "(*.xlsx) | *.xlsx"
        loadstr += "|(*.xls) | *.xls"
        openDia.Filter = loadstr.ToString


        If openDia.ShowDialog = Windows.Forms.DialogResult.OK Then
            txtDataPath.Text = openDia.FileName
            pbar.Visible = True
            Me.Refresh()
            Dim myExcel As Excel.Application
            Dim myWorkBook As Excel.Workbook
            Dim tworksheet As Excel.Worksheet
            myExcel = CreateObject("Excel.Application")
            myWorkBook = myExcel.Workbooks.Open(txtDataPath.Text)
            cmbSheetName.Items.Clear()
            For Each tworksheet In myWorkBook.Worksheets
                Me.cmbSheetName.Items.Add(UCase(tworksheet.Name))
            Next
            If cmbSheetName.Items.Count > 0 Then
                cmbSheetName.Text = cmbSheetName.Items.Item(0)
            End If
            Me.SelectNextControl(btnPath, True, True, True, True)
            funcBarStep()
            pbar.Value = pbar.Maximum
            pbar.Value = 0
            pbar.Visible = False
        End If
    End Sub

    Private Sub xmlConvet_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Me.Dispose()
    End Sub

    Private Sub xmlConvet_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Not myExcel Is Nothing Then myExcel.Workbooks.Close()
    End Sub
    Function funcLoadABookcolumns() As Integer
        dtGridView.Rows.Clear()
        Dim BFields() As String = {"SNO", "ITEMID", "TAGNO", "PCS", "GRSWT", "HUID"}
        cmbGridExcelCol.Items.Clear()
        Dim row As DataRow = Nothing

        For Each Field As String In BFields
            row = dtGridView.NewRow
            row("HALLMARK_COLUMN") = Field.ToString
            dtGridView.Rows.Add(row)
        Next Field
        dtGridView.AcceptChanges()
    End Function

    Private Sub xmlConvet_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        pbar.Visible = False

        strSql = " SELECT ''HALLMARK_COLUMN "
        da = New OleDbDataAdapter(strSql, cn)
        dtGridView = New DataTable
        da.Fill(dtGridView)
        gridView.DataSource = dtGridView
        gridView.Columns(0).Width = 260
        gridView.Columns(0).HeaderText = "HALLMARK COLUMN"
        gridView.DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.Window
        gridView.DefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.ControlText
        gridView.Columns(0).SortMode = DataGridViewColumnSortMode.NotSortable
        gridView.Focus()
        cmbGridExcelCol.Visible = False
        funcLoadABookcolumns()
        btnSave.Enabled = False
    End Sub

    Private Sub txtDataPath_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDataPath.GotFocus
        Me.SelectNextControl(sender, True, True, True, True)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        KillExcel()
        Me.Dispose()
    End Sub

    Private Sub gridView_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellEnter
        Dim gridViewPt As Point = gridView.Location
        Select Case e.ColumnIndex
            Case 1
                cmbGridExcelCol.Visible = True
                cmbGridExcelCol.Size = gridView.GetCellDisplayRectangle(gridView.CurrentCell.ColumnIndex, gridView.CurrentCell.RowIndex, True).Size
                cmbGridExcelCol.Location = gridView.GetCellDisplayRectangle(gridView.CurrentCell.ColumnIndex, gridView.CurrentCell.RowIndex, True).Location + gridViewPt
                cmbGridExcelCol.Text = gridView.CurrentCell.Value.ToString
                cmbGridExcelCol.SelectAll()
                cmbGridExcelCol.Focus()
        End Select
    End Sub

    Private Sub gridView_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
        cmbGridExcelCol.Text = ""
        cmbGridExcelCol.Visible = False
    End Sub

    Private Sub cmbGridAddCol_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbGridExcelCol.KeyDown
        If e.Shift Then
            If e.KeyCode = Keys.Down Then
                If gridView.CurrentRow.Index <> gridView.RowCount - 1 Then
                    gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index + 1).Cells(1)
                End If
            ElseIf e.KeyCode = Keys.Up Then
                If gridView.CurrentRow.Index <> 0 Then
                    gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index - 1).Cells(1)
                End If
            ElseIf e.KeyCode = Keys.Left Then
                If gridView.CurrentCell.ColumnIndex <> 0 Then
                    gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells(1)
                End If
            ElseIf e.KeyCode = Keys.Right Then
                If gridView.CurrentCell.ColumnIndex <> gridView.ColumnCount - 1 Then
                    gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells(1)
                End If
            End If
        End If
    End Sub

    Private Sub cmbGridAddCol_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbGridExcelCol.KeyPress
        e.KeyChar = UCase(e.KeyChar)
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbGridExcelCol.Text <> "" Then
                If cmbGridExcelCol.Items.Contains(cmbGridExcelCol.Text) = False Then
                    MsgBox("Invalid Item", MsgBoxStyle.Information)
                    cmbGridExcelCol.Focus()
                    Exit Sub
                End If
            End If
            gridView.CurrentCell.Value = cmbGridExcelCol.Text
            Select Case gridView.CurrentCell.ColumnIndex
                Case 1
                    If gridView.CurrentRow.Index = gridView.RowCount - 1 Then
                        gridView.CurrentCell = gridView.Rows(0).Cells(1)
                    Else
                        gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index + 1).Cells(1)
                    End If
            End Select
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            btnValidate.Focus()
        End If

    End Sub
    Private Sub btnConvert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If txtDataPath.Text.Length = 0 Then
            MsgBox("Data Path Should not Empty", MsgBoxStyle.Information)
            btnPath.Focus()
            btnPath.Select()
            Exit Sub
        End If
        btnSave.Enabled = False
        funcArrageExcellColumns()
        KillExcel()
    End Sub

    Private Sub gridView_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If gridView.Columns.Count = 2 Then
            If gridView.CurrentCell.ColumnIndex = 0 Then
                gridView.CurrentCell = gridView.Rows(gridView.CurrentCell.RowIndex).Cells(1)
            End If
        End If
    End Sub
    Private Sub cmbSheetName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbSheetName.KeyPress
        Try
            If e.KeyChar = Chr(Keys.Enter) Then
                If dtGridView.Columns.Contains("EXCEL_COLUMN") Then
                    dtGridView.Columns.Remove("EXCEL_COLUMN")
                    dtGridView.AcceptChanges()
                End If
                dtGridView.Columns.Add("EXCEL_COLUMN")
                dtGridView.AcceptChanges()
                gridView.Columns(1).Width = 260
                gridView.Columns(1).HeaderText = "Excel column"
                gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                gridView.ColumnHeadersDefaultCellStyle.Font = New Font("Verdana", 8, FontStyle.Bold)
                dsExceelDatas.Clear()

                strConn = "Provider=Microsoft.Ace.OLEDB.12.0;Data Source='" & txtDataPath.Text & "';Extended Properties=""Excel 12.0 Xml;HDR=YES;"""
                strSql = "SELECT * FROM [" & cmbSheetName.Text & "$]"
                da = New OleDbDataAdapter(strSql, strConn)
                da.Fill(dsExceelDatas, "ExcelData")
                For i As Integer = 0 To dsExceelDatas.Tables(0).Rows.Count - 1
                    If dsExceelDatas.Tables(0).Rows(i).Item(0).ToString = "" Then
                        dsExceelDatas.Tables(0).Rows(i).Delete()
                    End If
                Next
                dsExceelDatas.Tables(0).AcceptChanges()
                DataGridView1.DataSource = Nothing
                DataGridView1.DataSource = dsExceelDatas.Tables(0)
                For cnt As Integer = 0 To 0
                    For i As Integer = 0 To dsExceelDatas.Tables(cnt).Columns.Count - 1
                        cmbGridExcelCol.Items.Add(dsExceelDatas.Tables(cnt).Columns(i).ColumnName.ToString)
                    Next
                Next
                pbar.Maximum = dsExceelDatas.Tables(0).Rows.Count - 1
                Me.SelectNextControl(sender, True, True, True, True)
                btnSave.Enabled = False
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Sub
    Sub KillExcel()
        '---Kill excel
        Dim procs As Process() = Process.GetProcessesByName("EXCEL")
        Dim proc As Process
        For Each proc In procs
            If proc.Responding Then
                If proc.MainWindowTitle = "" Then
                    proc.Kill()
                End If
            Else
                proc.Kill()
            End If
        Next proc
    End Sub

    Private Sub btnValidate_Click(sender As Object, e As EventArgs) Handles btnValidate.Click
        Dim tempcolHM As String = ""
        Dim tempcolGrsWt As String = ""
        Dim tempcolItemid As String = ""
        Dim tempcolTagNo As String = ""
        Dim dttt As New DataTable
        dttt = dsExceelDatas.Tables(0).Copy
        For CNT As Integer = 0 To dtGridView.Rows.Count - 1
            If dtGridView.Rows(CNT).Item("HALLMARK_COLUMN").ToString = "HUID" Then
                tempcolHM = dtGridView.Rows(CNT).Item("EXCEL_COLUMN").ToString
            ElseIf dtGridView.Rows(CNT).Item("HALLMARK_COLUMN").ToString = "GRSWT" Then
                tempcolGrsWt = dtGridView.Rows(CNT).Item("EXCEL_COLUMN").ToString
            ElseIf dtGridView.Rows(CNT).Item("HALLMARK_COLUMN").ToString = "ITEMID" Then
                tempcolItemid = dtGridView.Rows(CNT).Item("EXCEL_COLUMN").ToString
            ElseIf dtGridView.Rows(CNT).Item("HALLMARK_COLUMN").ToString = "TAGNO" Then
                tempcolTagNo = dtGridView.Rows(CNT).Item("EXCEL_COLUMN").ToString
            End If
        Next
        If tempcolHM = "" Then Exit Sub
        If dsExceelDatas.Tables(0).Columns.Contains("CHECK") Then dsExceelDatas.Tables(0).Columns.Remove("CHECK")
        dsExceelDatas.Tables(0).Columns.Add("CHECK", GetType(String))
        dsExceelDatas.Tables(0).AcceptChanges()
        For CNT As Integer = 0 To dsExceelDatas.Tables(0).Rows.Count - 1
            Dim Htagsno As String = ""
            Dim chkhmno As String = ""
            chkhmno = dsExceelDatas.Tables(0).Rows(CNT).Item(tempcolHM).ToString
            If chkhmno.ToString = "" Or chkhmno.ToString = "0" Then
                dsExceelDatas.Tables(0).Rows(CNT).Item("CHECK") = "Hallmark No is Empty"
                Continue For
            End If
            'strSql = "SELECT DISTINCT TAGSNO FROM (SELECT ISNULL(SNO,'')TAGSNO FROM " & cnAdminDb & "..ITEMTAG WHERE  HM_BILLNO = '" & chkhmno.ToString & "'"
            'strSql += " AND NOT EXISTS(SELECT 1 FROM " & cnStockDb & "..RECEIPT WHERE SNO IN "
            'strSql += " (SELECT ISSSNO FROM " & cnStockDb & "..RECEIPTHALLMARK WHERE HM_BILLNO ='" & chkhmno.ToString & "') AND ISNULL(CANCEL,'')<>'Y' AND TRANTYPE='SR') "
            'strSql += " UNION ALL SELECT ISNULL(TAGSNO,'')TAGSNO FROM " & cnAdminDb & "..ITEMTAGHALLMARK WHERE  HM_BILLNO = '" & chkhmno.ToString & "' "
            'strSql += " AND NOT EXISTS(SELECT 1 FROM " & cnStockDb & "..RECEIPT WHERE SNO IN "
            'strSql += " (SELECT ISSSNO FROM " & cnStockDb & "..RECEIPTHALLMARK WHERE HM_BILLNO ='" & chkhmno.ToString & "') AND ISNULL(CANCEL,'')<>'Y' AND TRANTYPE='SR') "
            'strSql += " ) X"

            strSql = "SELECT DISTINCT TAGSNO FROM (SELECT ISNULL(SNO,'')TAGSNO FROM " & cnAdminDb & "..ITEMTAG WHERE  HM_BILLNO = '" & chkhmno.ToString & "'"
            strSql += " AND SNO NOT IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE LTRIM(ITEMID)+'-'+TAGNO IN "
            strSql += " (SELECT LTRIM(ITEMID)+'-'+TAGNO FROM " & cnStockDb & "..ISSUE WHERE SNO IN "
            strSql += " (SELECT ISSSNO FROM " & cnStockDb & "..ISSHALLMARK WHERE HM_BILLNO ='" & chkhmno.ToString & "') AND ISNULL(CANCEL,'')<>'Y' AND TRANTYPE='SA')) "
            strSql += " UNION ALL "
            strSql += " SELECT ISNULL(TAGSNO,'')TAGSNO FROM " & cnAdminDb & "..ITEMTAGHALLMARK WHERE  HM_BILLNO = '" & chkhmno.ToString & "' "
            strSql += " AND TAGSNO NOT IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE LTRIM(ITEMID)+'-'+TAGNO IN "
            strSql += " (SELECT LTRIM(ITEMID)+'-'+TAGNO FROM " & cnStockDb & "..ISSUE WHERE SNO IN "
            strSql += " (SELECT ISSSNO FROM " & cnStockDb & "..ISSHALLMARK WHERE HM_BILLNO ='" & chkhmno.ToString & "') AND ISNULL(CANCEL,'')<>'Y' AND TRANTYPE='SA')) "
            strSql += " ) X"
            Htagsno = GetSqlValue(cn, strSql)
            If Not Htagsno Is Nothing Then
                Dim Htagrow As DataRow
                Htagrow = Nothing
                strSql = " SELECT ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID=T.COSTID),'') COSTNAME "
                strSql += " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID)ITEMNAME "
                strSql += " ,ITEMID,TAGNO,CONVERT(VARCHAR(12),RECDATE,103)RECDATE "
                strSql += " FROM " & cnAdminDb & "..ITEMTAG T WHERE "
                strSql += " SNO='" & Htagsno.ToString & "'"
                Htagrow = GetSqlRow(strSql, cn, Nothing)
                If Not Htagrow Is Nothing Then
                    Dim tempstr As String = ""
                    tempstr = "HallmarkNo Already Exist" _
                        & IIf(Htagrow!Costname.ToString <> "", " Branch : " & Htagrow!Costname.ToString & ",", "") _
                        & " Itemname : " & Htagrow!ITEMNAME.ToString & ", Recdate : " & Htagrow!RECDATE.ToString _
                        & ", Itemid : " & Htagrow!ITEMID.ToString & ", Tagno : " & Htagrow!TAGNO.ToString
                    dsExceelDatas.Tables(0).Rows(CNT).Item("CHECK") = tempstr
                    Continue For
                End If
            End If
            strSql = " SELECT COUNT(*)CNT FROM " & cnAdminDb & "..ITEMTAG WHERE "
            strSql += " ITEMID = '" & dsExceelDatas.Tables(0).Rows(CNT).Item(tempcolItemid).ToString & "'"
            strSql += " AND TAGNO = '" & dsExceelDatas.Tables(0).Rows(CNT).Item(tempcolTagNo).ToString & "'"
            strSql += " AND ISNULL(ISSDATE,'')<>'' "
            If Val(GetSqlValue(cn, strSql)) <> 0 Then
                dsExceelDatas.Tables(0).Rows(CNT).Item("CHECK") = "Tag Already Sold."
            End If

            strSql = " SELECT SUM(GRSWT) GRSWT FROM " & cnAdminDb & "..ITEMTAG WHERE "
            strSql += " ITEMID = '" & dsExceelDatas.Tables(0).Rows(CNT).Item(tempcolItemid).ToString & "'"
            strSql += " AND TAGNO = '" & dsExceelDatas.Tables(0).Rows(CNT).Item(tempcolTagNo).ToString & "'"
            Dim chkgrswt As Double = Val(GetSqlValue(cn, strSql).ToString)
            If chkgrswt > 0 And Htagsno Is Nothing Then
                Dim chkstrIT As String = ""
                chkstrIT = "[" & tempcolItemid & "]='" & dsExceelDatas.Tables(0).Rows(CNT).Item(tempcolItemid).ToString _
                    & "' AND [" & tempcolTagNo & "] ='" & dsExceelDatas.Tables(0).Rows(CNT).Item(tempcolTagNo).ToString & "'"
                If chkgrswt <> Val(dttt.Compute("SUM([" & tempcolGrsWt & "])", chkstrIT.ToString)) Then
                    dsExceelDatas.Tables(0).Rows(CNT).Item("CHECK") = "TagWt Mismatch With Given Hallmark Detail. Taggrswt : " & chkgrswt.ToString & "."
                End If
            End If
        Next
        dsExceelDatas.Tables(0).AcceptChanges()
        dttt = New DataTable
        dttt = dsExceelDatas.Tables(0).Copy
        Dim drttt() As DataRow
        drttt = dttt.Select("ISNULL(CHECK,'')=''")
        If drttt.Length > 0 Then btnSave.Enabled = True : btnSave.Focus()
        If drttt.Length = dttt.Rows.Count Then
            dsExceelDatas.Tables(0).Columns.Remove("CHECK")
            dsExceelDatas.Tables(0).AcceptChanges()
        Else
            If DataGridView1.Columns.Contains("CHECK") Then DataGridView1.Columns("CHECK").Width = 200
        End If
    End Sub
End Class
