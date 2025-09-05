Imports System.Data.OleDb
Public Class frmItemHallmarkDetalisUpdate
    Dim dtTagDet As New DataTable
    Dim strSql As String
    Dim dtgridHallmarkDetails As New DataTable("Gridhallmark")
    Dim updIssSno As String
    Dim taggrswt As Decimal
    Dim tagpcs As Integer
    Dim HALLMARKVALID As Boolean = IIf(GetAdmindbSoftValue("HALLMARK_VALID", "N") = "Y", True, False)
    Dim HallmarkMinLen As Integer = Val(GetAdmindbSoftValue("HALLMARK_MINLEN", 0,, True))
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Public Sub New(ByVal ISSsno As String)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        updIssSno = ISSsno
        ''OtherCharges
        objGPack.TextClear(Me)
        ''Hallmark Details
        dtgridHallmarkDetails = New DataTable
        With dtgridHallmarkDetails.Columns
            .Add("GRSWT", GetType(Decimal))
            .Add("HM_BILLNO", GetType(String))
            .Add("PCS", GetType(Integer))
            .Add("KEYNO", GetType(Integer))
        End With
        dtgridHallmarkDetails.Columns("KEYNO").AutoIncrement = True
        dtgridHallmarkDetails.Columns("KEYNO").AutoIncrementStep = 1
        dtgridHallmarkDetails.Columns("KEYNO").AutoIncrementSeed = 1
        gridHallmarkDetails.DataSource = dtgridHallmarkDetails
        StylegridHallmarkDetails(gridHallmarkDetails)

        ''HALLDETAILS
        strSql = " SELECT GRSWT,HM_BILLNO,1 PCS "
        strSql += " FROM " & cnAdminDb & "..ITEMTAGHALLMARK AS T"
        strSql += " WHERE TAGSNO = '" & updIssSno & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtgridHallmarkDetails)
        StylegridHallmarkDetails(gridHallmarkDetails)
        dtgridHallmarkDetails.AcceptChanges()

        tagpcs = 0
        taggrswt = 0
        strSql = " SELECT"
        strSql += " PCS,GRSWT"
        strSql += " FROM " & cnAdminDb & "..ITEMTAG AS T"
        strSql += " WHERE SNO = '" & updIssSno & "'"
        Dim dtttag As DataTable
        dtttag = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtttag)
        If dtttag.Rows.Count > 0 Then
            tagpcs = Val(dtttag.Rows(0).Item("PCS").ToString)
            taggrswt = Val(dtttag.Rows(0).Item("GRSWT").ToString)
        End If
        If dtgridHallmarkDetails.Rows.Count = 0 Then
            txtTagWt_WET.Text = Format(Val(taggrswt.ToString), "0.000")
            txtTagWt_WET.SelectAll()
            lblTagWt.Select()
        End If
    End Sub
    Public Sub StylegridHallmarkDetails(ByVal gridHallmarkDetails As DataGridView)
        With gridHallmarkDetails
            .Columns("KEYNO").Visible = False
            .Columns("PCS").Visible = False
            .Columns("GRSWT").Width = txtTagWt_WET.Width + 1
            .Columns("HM_BILLNO").Width = txtHallmarkNo.Width + 1
        End With
    End Sub


    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        funcsave()
        Me.Close()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcsave()
    End Sub

    Private Sub funcsave()
        Try
            Dim TagNo As String = Nothing
            Dim COSTID As String = Nothing
            Dim dt As New DataTable
            strSql = "  SELECT TAGNO,COSTID"
            strSql += "  FROM " & cnAdminDb & "..ITEMTAG "
            strSql += " WHERE SNO = '" & updIssSno & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If Not dt.Rows.Count > 0 Then Exit Sub
            COSTID = dt.Rows(0).Item("COSTID").ToString()
            TagNo = dt.Rows(0).Item("TAGNO").ToString()

            tran = Nothing
            tran = cn.BeginTransaction()

            ''DELETING HallmarkDetail
            strSql = " DELETE FROM " & cnAdminDb & "..ITEMTAGHALLMARK"
            strSql += " WHERE TAGSNO = '" & updIssSno & "'"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

            ''INSERTING HallmarkDetail
            For cnt As Integer = 0 To dtgridHallmarkDetails.Rows.Count - 1
                Dim HALLMARKSno As String = ""
                HALLMARKSno = GetNewSno(TranSnoType.ITEMTAGHALLMARKCODE, tran, "GET_ADMINSNO_TRAN")
                With dtgridHallmarkDetails.Rows(cnt)
                    strSql = " INSERT INTO " & cnAdminDb & "..ITEMTAGHALLMARK"
                    strSql += " ("
                    strSql += " SNO,TAGSNO,GRSWT,HM_BILLNO,USERID,SYSTEMID,APPVER,UPDATED,UPTIME,COSTID,COMPANYID"
                    strSql += " )VALUES("
                    strSql += " '" & HALLMARKSno & "'" 'SNO
                    strSql += " ,'" & updIssSno & "'" 'TAGSNO
                    strSql += " ,'" & Val(.Item("GRSWT").ToString) & "'" 'GRSWT
                    strSql += " ,'" & .Item("HM_BILLNO").ToString & "'" 'HM_BILLNO
                    strSql += " ," & userId & "" 'USERID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                    strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                    strSql += " ,'" & cnCostId & "'" 'COSTID
                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    strSql += " )"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                End With
            Next
            tran.Commit()
            tran = Nothing
            MsgBox(TagNo + E0009, MsgBoxStyle.Exclamation)
            Me.Dispose()
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            If Not tran Is Nothing Then tran.Dispose()
        End Try
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub gridHallmarkDetails_KeyDown(sender As Object, e As KeyEventArgs) Handles gridHallmarkDetails.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub gridHallmarkDetails_KeyPress(sender As Object, e As KeyPressEventArgs) Handles gridHallmarkDetails.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txthallmarkRowIndex.Text = ""
            gridHallmarkDetails.CurrentCell = gridHallmarkDetails.Rows(gridHallmarkDetails.CurrentRow.Index).Cells("HM_BILLNO")
            With gridHallmarkDetails.Rows(gridHallmarkDetails.CurrentRow.Index)
                txtTagWt_WET.Text = .Cells("GRSWT").FormattedValue
                txtHallmarkNo.Text = .Cells("HM_BILLNO").FormattedValue
                txthallmarkRowIndex.Text = gridHallmarkDetails.CurrentRow.Index
                lblTagWt.Select()
            End With
        End If
    End Sub

    Private Sub txtHallmarkNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtHallmarkNo.KeyPress
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            If HALLMARKVALID = True And txtHallmarkNo.Text = "" Then MsgBox("Hallmark Billno Is empty") : txtHallmarkNo.Focus() : Exit Sub
            If Val(txtHallmarkNo.Text.Length) > 0 Then
                If HallmarkMinLen > 0 Then
                    If Val(HallmarkMinLen) <> Val(txtHallmarkNo.Text.Length) Then
                        MsgBox("HallmarkNo Length Less Than Minimum Length", MsgBoxStyle.Information)
                        txtHallmarkNo.Select()
                        Exit Sub
                    End If
                End If
                Dim Htagsno As String = ""
                ''strSql = "SELECT DISTINCT TAGSNO FROM (SELECT ISNULL(SNO,'')TAGSNO FROM " & cnAdminDb & "..ITEMTAG WHERE  HM_BILLNO = '" & txtHallmarkNo.Text & "'"
                ''strSql += " UNION ALL SELECT ISNULL(TAGSNO,'')TAGSNO FROM " & cnAdminDb & "..ITEMTAGHALLMARK WHERE  HM_BILLNO = '" & txtHallmarkNo.Text & "') X "

                'strSql = "SELECT DISTINCT TAGSNO FROM (SELECT ISNULL(SNO,'')TAGSNO FROM " & cnAdminDb & "..ITEMTAG WHERE  HM_BILLNO = '" & txtHallmarkNo.Text & "'"
                'strSql += " AND NOT EXISTS(SELECT 1 FROM " & cnStockDb & "..ISSUE WHERE SNO IN "
                'strSql += " (SELECT ISSSNO FROM " & cnStockDb & "..ISSHALLMARK WHERE HM_BILLNO ='" & txtHallmarkNo.Text & "') AND ISNULL(CANCEL,'')<>'Y' AND TRANTYPE='SA') "
                'strSql += " UNION ALL SELECT ISNULL(TAGSNO,'')TAGSNO FROM " & cnAdminDb & "..ITEMTAGHALLMARK WHERE  HM_BILLNO = '" & txtHallmarkNo.Text & "' "
                'strSql += " AND NOT EXISTS(SELECT 1 FROM " & cnStockDb & "..ISSUE WHERE SNO IN "
                'strSql += " (SELECT ISSSNO FROM " & cnStockDb & "..ISSHALLMARK WHERE HM_BILLNO ='" & txtHallmarkNo.Text & "') AND ISNULL(CANCEL,'')<>'Y' AND TRANTYPE='SA') "
                'strSql += " ) X"

                strSql = "SELECT DISTINCT TAGSNO FROM (SELECT ISNULL(SNO,'')TAGSNO FROM " & cnAdminDb & "..ITEMTAG WHERE  HM_BILLNO = '" & txtHallmarkNo.Text & "'"
                strSql += " AND SNO NOT IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE LTRIM(ITEMID)+'-'+TAGNO IN "
                strSql += " (SELECT LTRIM(ITEMID)+'-'+TAGNO FROM " & cnStockDb & "..ISSUE WHERE SNO IN "
                strSql += " (SELECT ISSSNO FROM " & cnStockDb & "..ISSHALLMARK WHERE HM_BILLNO ='" & txtHallmarkNo.Text & "') AND ISNULL(CANCEL,'')<>'Y' AND TRANTYPE='SA')) "
                strSql += " UNION ALL "
                strSql += " SELECT ISNULL(TAGSNO,'')TAGSNO FROM " & cnAdminDb & "..ITEMTAGHALLMARK WHERE  HM_BILLNO = '" & txtHallmarkNo.Text & "' "
                strSql += " AND TAGSNO NOT IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE LTRIM(ITEMID)+'-'+TAGNO IN "
                strSql += " (SELECT LTRIM(ITEMID)+'-'+TAGNO FROM " & cnStockDb & "..ISSUE WHERE SNO IN "
                strSql += " (SELECT ISSSNO FROM " & cnStockDb & "..ISSHALLMARK WHERE HM_BILLNO ='" & txtHallmarkNo.Text & "') AND ISNULL(CANCEL,'')<>'Y' AND TRANTYPE='SA')) "
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
                    strSql += " AND NOT EXISTS(SELECT 1 FROM " & cnStockDb & "..ISSHALLMARK WHERE HM_BILLNO IN("
                    strSql += " SELECT DISTINCT HM_BILLNO FROM " & cnAdminDb & "..ITEMTAGHALLMARK WHERE TAGSNO IN ('" & Htagsno.ToString & "')))"
                    strSql += " AND T.SNO<>'" & updIssSno & "'"
                    Htagrow = GetSqlRow(strSql, cn, Nothing)
                    If Not Htagrow Is Nothing Then
                        MsgBox("HallmarkNo Already Exist" _
                        & IIf(Htagrow!Costname.ToString <> "", vbCrLf & " Branch : " & Htagrow!Costname.ToString, "") _
                        & vbCrLf & " Itemname : " & Htagrow!ITEMNAME.ToString & vbCrLf & " Recdate : " & Htagrow!RECDATE.ToString _
                        & vbCrLf & " Itemid : " & Htagrow!ITEMID.ToString & vbCrLf & " Tagno : " & Htagrow!TAGNO.ToString _
                        , MsgBoxStyle.Information)
                        txtHallmarkNo.Select()
                        Exit Sub
                    End If
                End If
            End If

            If Val(txtHallmarkNo.Text.Length) > 0 Then
                If Not Val(txtTagWt_WET.Text) > 0 And Val(taggrswt.ToString) > 0 Then
                    MsgBox(Me.GetNextControl(txtTagWt_WET, False).Text + E0001, MsgBoxStyle.Information)
                    txtTagWt_WET.Select()
                    Exit Sub
                End If


                Dim CHKro() As DataRow
                CHKro = Nothing
                If dtgridHallmarkDetails.Rows.Count > 0 Then
                    If Val(txtTagWt_WET.Text) > 0 And Val(taggrswt) > 0 Then
                        Dim chkwt As Decimal = 0
                        If txthallmarkRowIndex.Text = "" Then
                            chkwt = Val(dtgridHallmarkDetails.Compute("SUM(GRSWT)", Nothing)) + Val(txtTagWt_WET.Text)
                        Else
                            chkwt = Val(dtgridHallmarkDetails.Compute("SUM(GRSWT)", "KEYNO <> '" & gridHallmarkDetails.Rows(Val(txthallmarkRowIndex.Text)).Cells("KEYNO").Value & "'").ToString) + Val(txtTagWt_WET.Text)
                        End If
                        ''chkwt = Val(dtHallmarkDetails.Compute("SUM(GRSWT)", "KEYNO <> '" & gridHallmarkDetails.Rows(Val(txthallmarkRowIndex.Text)).Cells("KEYNO").Value & "'").ToString) + Val(txtTagWt_WET.Text)
                        If Val(chkwt) > Val(taggrswt.ToString) Then
                            MsgBox("TagWeight Should Not Exceed GrossWeight", MsgBoxStyle.Information)
                            txtTagWt_WET.Clear()
                            txtHallmarkNo.Clear()
                            txtTagWt_WET.Focus()
                            Exit Sub
                        End If
                    ElseIf Val(tagpcs.ToString) > 0 And Val(taggrswt.ToString) = 0 And HALLMARKVALID Then
                        Dim chkpcs As Integer = 0
                        If txthallmarkRowIndex.Text = "" Then
                            chkpcs = Val(dtgridHallmarkDetails.Compute("SUM(PCS)", Nothing)) + 1
                        Else
                            chkpcs = Val(dtgridHallmarkDetails.Compute("SUM(PCS)", "KEYNO <> '" & gridHallmarkDetails.Rows(Val(txthallmarkRowIndex.Text)).Cells("KEYNO").Value & "'").ToString) + 1
                        End If
                        'chkpcs = Val(dtHallmarkDetails.Compute("SUM(PCS)", "KEYNO <> '" & gridHallmarkDetails.Rows(Val(txthallmarkRowIndex.Text)).Cells("KEYNO").Value & "'").ToString) + 1
                        If Val(chkpcs) > Val(tagpcs.ToString) Then
                            MsgBox("Pcs Should Not Exceed TagPcs", MsgBoxStyle.Information)
                            txtHallmarkNo.Clear()
                            txtHallmarkNo.Focus()
                            Exit Sub
                        End If
                    End If

                    If txthallmarkRowIndex.Text <> "" Then
                        With gridHallmarkDetails.Rows(Val(txthallmarkRowIndex.Text))
                            .Cells("GRSWT").Value = txtTagWt_WET.Text
                            .Cells("HM_BILLNO").Value = txtHallmarkNo.Text
                            dtgridHallmarkDetails.AcceptChanges()
                            GoTo AFTERINSERT
                        End With
                    End If

                    Dim dtchk As DataTable
                    dtchk = New DataTable
                    dtchk = dtgridHallmarkDetails.Copy
                    Dim chkhmno As String
                    chkhmno = "HM_BILLNO='" & txtHallmarkNo.Text & "'"
                    CHKro = dtchk.Select(chkhmno, Nothing)

                    If CHKro.Length = 0 Then
                        Dim ro As DataRow = Nothing
                        ro = dtgridHallmarkDetails.NewRow
                        ro("PCS") = 1
                        ro("GRSWT") = IIf(Val(txtTagWt_WET.Text) <> 0, Val(txtTagWt_WET.Text), DBNull.Value)
                        ro("HM_BILLNO") = txtHallmarkNo.Text
                        dtgridHallmarkDetails.Rows.Add(ro)
                        dtgridHallmarkDetails.AcceptChanges()
                    ElseIf CHKro Is Nothing Then
                        Dim ro As DataRow = Nothing
                        ro = dtgridHallmarkDetails.NewRow
                        ro("PCS") = 1
                        ro("GRSWT") = IIf(Val(txtTagWt_WET.Text) <> 0, Val(txtTagWt_WET.Text), DBNull.Value)
                        ro("HM_BILLNO") = txtHallmarkNo.Text
                        dtgridHallmarkDetails.Rows.Add(ro)
                        dtgridHallmarkDetails.AcceptChanges()
                    Else
                        MsgBox("Hallmark No Already In Grid", MsgBoxStyle.Information)
                        txtTagWt_WET.SelectAll()
                        Exit Sub
                    End If
                Else
                    Dim ro As DataRow = Nothing
                    ro = dtgridHallmarkDetails.NewRow
                    ro("PCS") = 1
                    ro("GRSWT") = IIf(Val(txtTagWt_WET.Text) <> 0, Val(txtTagWt_WET.Text), DBNull.Value)
                    ro("HM_BILLNO") = txtHallmarkNo.Text
                    dtgridHallmarkDetails.Rows.Add(ro)
                    dtgridHallmarkDetails.AcceptChanges()
                End If
AFTERINSERT:
                If txthallmarkRowIndex.Text <> "" Then
                    gridHallmarkDetails.CurrentCell = gridHallmarkDetails.Rows(gridHallmarkDetails.RowCount - 1).Cells(0)
                End If

                txtHallmarkNo.Clear()
                txtTagWt_WET.Clear()
                txthallmarkRowIndex.Clear()
                lblTagWt.Select()
            End If
        End If
    End Sub
    Private Sub txtTagWt_WET_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtTagWt_WET.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtTagWt_WET_KeyDown(sender As Object, e As KeyEventArgs) Handles txtTagWt_WET.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridHallmarkDetails.RowCount > 0 Then gridHallmarkDetails.Focus()
        End If
    End Sub

    Private Sub txtHallmarkNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txtHallmarkNo.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridHallmarkDetails.RowCount > 0 Then gridHallmarkDetails.Focus()
        End If
    End Sub

    Private Sub gridHallmarkDetails_UserDeletedRow(sender As Object, e As DataGridViewRowEventArgs) Handles gridHallmarkDetails.UserDeletedRow
        dtgridHallmarkDetails.AcceptChanges()
        If Not gridHallmarkDetails.RowCount > 0 Then
            txtTagWt_WET.Focus()
        End If
    End Sub

    Private Sub frmItemHallmarkDetalisUpdate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Escape) Then
            If gridHallmarkDetails.RowCount > 0 Then
                btnSave.Focus()
            End If
        End If
    End Sub
End Class