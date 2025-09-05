Public Class frmHallmarkInfo
    Dim strSql As String
    Public dtgridHallmarkDetails As DataTable
    Public taggrswt As Decimal
    Public tagpcs As Integer
    Dim HALLMARKVALID As Boolean = IIf(GetAdmindbSoftValue("HALLMARK_VALID", "N") = "Y", True, False)
    Dim HallmarkMinLen As Integer = Val(GetAdmindbSoftValue("HALLMARK_MINLEN", 0,, True))
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.TextClear(Me)
        ''Hallmark Details
        dtgridHallmarkDetails = New DataTable
        With dtgridHallmarkDetails.Columns
            .Add("KEYNO", GetType(Integer))
            .Add("TRANTYPE", GetType(String))
            .Add("GRSWT", GetType(Decimal))
            .Add("HM_BILLNO", GetType(String))
            .Add("PCS", GetType(Integer))
            .Add("EKEYNO", GetType(Integer))

        End With
        dtgridHallmarkDetails.Columns("EKEYNO").AutoIncrement = True
        dtgridHallmarkDetails.Columns("EKEYNO").AutoIncrementStep = 1
        dtgridHallmarkDetails.Columns("EKEYNO").AutoIncrementSeed = 1
        gridHallmarkDetails.DataSource = dtgridHallmarkDetails
        StylegridHallmarkDetails(gridHallmarkDetails)
    End Sub
    Public Sub StylegridHallmarkDetails(ByVal gridHallmarkDetails As DataGridView)
        With gridHallmarkDetails
            .Columns("KEYNO").Visible = False
            .Columns("PCS").Visible = False
            .Columns("TRANTYPE").Visible = False
            .Columns("GRSWT").Width = txtTagWt_WET.Width + 1
            .Columns("HM_BILLNO").Width = txtHallmarkNo.Width + 1
        End With
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
                strSql = "SELECT DISTINCT TAGSNO FROM (SELECT ISNULL(SNO,'')TAGSNO FROM " & cnAdminDb & "..ITEMTAG WHERE  HM_BILLNO = '" & txtHallmarkNo.Text & "'"
                strSql += " UNION ALL SELECT ISNULL(TAGSNO,'')TAGSNO FROM " & cnAdminDb & "..ITEMTAGHALLMARK WHERE  HM_BILLNO = '" & txtHallmarkNo.Text & "') X "
                Htagsno = GetSqlValue(cn, strSql)
                If Not Htagsno Is Nothing Then
                    Dim gridhmbillno As String = ""
                    If txthallmarkRowIndex.Text <> "" Then
                        gridhmbillno = gridHallmarkDetails.Rows(Val(txthallmarkRowIndex.Text)).Cells("HM_BILLNO").Value
                    End If
                    Dim Htagrow As DataRow
                    Htagrow = Nothing
                    strSql = " SELECT ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID=T.COSTID),'') COSTNAME "
                    strSql += " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID)ITEMNAME "
                    strSql += " ,ITEMID,TAGNO,CONVERT(VARCHAR(12),RECDATE,103)RECDATE "
                    strSql += " FROM " & cnAdminDb & "..ITEMTAG T WHERE "
                    strSql += " SNO='" & Htagsno.ToString & "'"
                    Htagrow = GetSqlRow(strSql, cn, Nothing)
                    If Not Htagrow Is Nothing And txtHallmarkNo.Text <> gridhmbillno.ToString Then
                        'MsgBox("HallmarkNo Already Exist" _
                        '& IIf(Htagrow!Costname.ToString <> "", vbCrLf & " Branch : " & Htagrow!Costname.ToString, "") _
                        '& vbCrLf & " Itemname : " & Htagrow!ITEMNAME.ToString & vbCrLf & " Recdate : " & Htagrow!RECDATE.ToString _
                        '& vbCrLf & " Itemid : " & Htagrow!ITEMID.ToString & vbCrLf & " Tagno : " & Htagrow!TAGNO.ToString _
                        ', MsgBoxStyle.Information)
                        'txtHallmarkNo.Select()
                        'Exit Sub
                    End If
                End If
            End If

            If Val(txtHallmarkNo.Text.Length) > 0 Then
                If Not Val(txtTagWt_WET.Text) > 0 And Val(taggrswt) > 0 Then
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
                            chkwt = Val(dtgridHallmarkDetails.Compute("SUM(GRSWT)", "EKEYNO <> '" & gridHallmarkDetails.Rows(Val(txthallmarkRowIndex.Text)).Cells("EKEYNO").Value & "'").ToString) + Val(txtTagWt_WET.Text)
                        End If
                        'chkwt = Val(dtgridHallmarkDetails.Compute("SUM(GRSWT)", "EKEYNO <> '" & gridHallmarkDetails.Rows(Val(txthallmarkRowIndex.Text)).Cells("EKEYNO").Value & "'").ToString) + Val(txtTagWt_WET.Text)
                        If Val(chkwt) > Val(taggrswt) Then
                            'MsgBox("Hallmark Weight Should Not Exceed GrossWeight", MsgBoxStyle.Information)
                            'txtTagWt_WET.Clear()
                            'txtHallmarkNo.Clear()
                            'txtTagWt_WET.Focus()
                            'Exit Sub
                        End If
                    ElseIf Val(tagpcs) > 0 Then
                        Dim chkpcs As Integer = 0
                        If txthallmarkRowIndex.Text = "" Then
                            chkpcs = Val(dtgridHallmarkDetails.Compute("SUM(PCS)", Nothing)) + 1
                        Else
                            chkpcs = Val(dtgridHallmarkDetails.Compute("SUM(PCS)", "EKEYNO <> '" & gridHallmarkDetails.Rows(Val(txthallmarkRowIndex.Text)).Cells("EKEYNO").Value & "'").ToString) + 1
                        End If
                        'chkpcs = Val(dtgridHallmarkDetails.Compute("SUM(PCS)", "KEYNO <> '" & gridHallmarkDetails.Rows(Val(txthallmarkRowIndex.Text)).Cells("KEYNO").Value & "'").ToString) + 1
                        If Val(chkpcs) > Val(tagpcs) Then
                            MsgBox("Hallmark Pcs Should Not Exceed TagPcs", MsgBoxStyle.Information)
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
                    gridHallmarkDetails.CurrentCell = gridHallmarkDetails.Rows(gridHallmarkDetails.RowCount - 1).Cells(2)
                End If

                txtHallmarkNo.Clear()
                txtTagWt_WET.Clear()
                txthallmarkRowIndex.Clear()
                lblTagWt.Select()
            End If
        End If
    End Sub

    Private Sub frmHallmarkInfo_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown

        If e.KeyCode = Keys.Escape Then
            dtgridHallmarkDetails.AcceptChanges()
            lblTagWt.Select()
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub gridHallmarkDetails_KeyDown(sender As Object, e As KeyEventArgs) Handles gridHallmarkDetails.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub gridHallmarkDetails_KeyPress(sender As Object, e As KeyPressEventArgs) Handles gridHallmarkDetails.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txthallmarkRowIndex.Text = ""
            gridHallmarkDetails.CurrentCell = gridHallmarkDetails.Rows(gridHallmarkDetails.CurrentRow.Index).Cells(2)
            With gridHallmarkDetails.Rows(gridHallmarkDetails.CurrentRow.Index)
                txtTagWt_WET.Text = .Cells("GRSWT").FormattedValue
                txtHallmarkNo.Text = .Cells("HM_BILLNO").FormattedValue
                txthallmarkRowIndex.Text = gridHallmarkDetails.CurrentRow.Index
                lblTagWt.Select()
            End With
        End If
    End Sub

    Private Sub frmHallmarkInfo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub gridHallmarkDetails_UserDeletedRow(sender As Object, e As DataGridViewRowEventArgs) Handles gridHallmarkDetails.UserDeletedRow
        dtgridHallmarkDetails.AcceptChanges()
        If Not gridHallmarkDetails.RowCount > 0 Then
            txtTagWt_WET.Focus()
        End If
    End Sub
End Class