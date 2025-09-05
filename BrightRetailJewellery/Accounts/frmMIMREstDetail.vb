Imports System.Data.OleDb
Public Class frmMIMREstDetail
#Region "Variable"
    Dim strsql As String = ""
    Dim da As OleDbDataAdapter
    Dim dt As New DataTable
    Dim cmd As OleDbCommand
    Public MIMRAccode As String = ""
#End Region

#Region " Constractor"
    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        dtpTrandate.Value = GetServerDate()
    End Sub
#End Region

#Region " Form Load"
    Private Sub frmMIMREstDetail_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
    Private Sub frmMIMREstDetail_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{tab}")
        ElseIf e.KeyCode = Keys.Escape Then
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()
        End If
    End Sub

#End Region

#Region " Textbox Events"
    Private Sub txtEstNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txtEstNo.KeyDown
        If e.KeyCode = Keys.Enter Then
            If txtEstNo.Text.Trim <> "" Then
                strsql = ""
                strsql += vbCrLf + " SELECT "
                strsql += vbCrLf + " A.SNO"
                strsql += vbCrLf + " ,A.TRANNO"
                strsql += vbCrLf + " ,A.TRANDATE"
                strsql += vbCrLf + " ,A.ITEMID"
                strsql += vbCrLf + " ,A.SUBITEMID "
                strsql += vbCrLf + " ,C.ITEMNAME"
                strsql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = A.ITEMID AND SUBITEMID = A.SUBITEMID) SUBITEMNAME"
                strsql += vbCrLf + " ,A.PCS"
                strsql += vbCrLf + " ,A.GRSWT"
                strsql += vbCrLf + " ,A.NETWT"
                strsql += vbCrLf + " ,A.LESSWT"
                strsql += vbCrLf + " ,A.TAGNO"
                strsql += vbCrLf + " ,B.TOUCH"
                strsql += vbCrLf + " ,B.PUREWT"
                strsql += vbCrLf + " ,B.MCGRM"
                strsql += vbCrLf + " ,CASE WHEN ISNULL(B.MCPER,0)>0 THEN B.MCPER ELSE 0 END MCPER "
                strsql += vbCrLf + " ,ISNULL(MCPIE,0) MCPIE"
                strsql += vbCrLf + " ,B.WASTPER "
                strsql += vbCrLf + " ,CASE WHEN A.GRSNET ='G' THEN 'GRS WT' ELSE 'NET WT' END GRSNET"
                strsql += vbCrLf + " ,A.ESTBATCHNO"
                strsql += vbCrLf + " ,A.PCS TAGPCS"
                strsql += vbCrLf + " ,A.TAGGRSWT"
                strsql += vbCrLf + " ,A.TAGNETWT"
                strsql += vbCrLf + " FROM " & cnStockDb & "..ESTISSUE AS A"
                strsql += vbCrLf + " ," & cnAdminDb & "..DEALER_WMCTABLE AS B"
                strsql += vbCrLf + " ," & cnAdminDb & "..ITEMMAST AS C"
                strsql += vbCrLf + " WHERE A.ITEMID=B.ITEMID  "
                strsql += vbCrLf + " And A.SUBITEMID = B.SUBITEMID "
                strsql += vbCrLf + " And A.ITEMID=C.ITEMID"
                strsql += vbCrLf + " And A.TRANDATE='" & Format(dtpTrandate.Value.Date, "yyyy-MM-dd") & "' "
                strsql += vbCrLf + " AND A.TRANNO = " & txtEstNo.Text.Trim & ""
                strsql += vbCrLf + " AND ISNULL(A.CANCEL,'')='' "
                strsql += vbCrLf + " AND ISNULL(A.BATCHNO,'')='' "
                'strsql += vbCrLf + " AND A.COMPANYID='" & cnCompanyId & "'"
                strsql += vbCrLf + " AND A.TRANTYPE ='SA' "
                strsql += vbCrLf + " AND B.ACCODE = '" & MIMRAccode & "'"
                da = New OleDbDataAdapter(strsql, cn)
                dt = New DataTable
                da.Fill(dt)
                gridViewMissing.DataSource = Nothing
                gridView.DataSource = Nothing
                If dt.Rows.Count > 0 Then
                    With gridView
                        Dim dr As DataRow = Nothing
                        dr = dt.NewRow
                        dr!PCS = Val(dt.Compute("SUM(PCS)", "").ToString)
                        dr!GRSWT = Val(dt.Compute("SUM(GRSWT)", "").ToString)
                        dr!NETWT = Val(dt.Compute("SUM(NETWT)", "").ToString)
                        dr!LESSWT = Val(dt.Compute("SUM(LESSWT)", "").ToString)
                        dt.Rows.Add(dr)
                        .DataSource = Nothing
                        .DataSource = dt
                        .Columns("SNO").Visible = False
                        .Columns("TRANNO").Visible = False
                        .Columns("TRANDATE").Visible = False
                        .Columns("SUBITEMID").Visible = False
                        .Columns("ESTBATCHNO").Visible = False
                        FormatGridColumns(gridView, False, True, True, False)
                        AutoResizeNewMethod(gridView)
                        gridView.Focus()
                        Dim missSno As String = ""
                        If dt.Rows.Count > 0 Then
                            missSno = ""
                            For i As Integer = 0 To dt.Rows.Count - 1
                                With dt.Rows(i)
                                    missSno += "'" & .Item("SNO").ToString() & "'"
                                    If i <> dt.Rows.Count - 1 Then
                                        missSno += ","
                                    End If
                                End With
                            Next
                            Dim dtEstIs As New DataTable
                            strsql = ""
                            strsql = " SELECT A.TAGNO"
                            strsql += vbCrLf + " ,A.ITEMID "
                            strsql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = A.ITEMID And SUBITEMID = A.SUBITEMID) SUBITEMNAME"
                            strsql += vbCrLf + " ,A.GRSWT "
                            strsql += vbCrLf + " ,A.NETWT "
                            strsql += vbCrLf + " ,A.LESSWT "
                            strsql += vbCrLf + " FROM " & cnStockDb & "..ESTISSUE AS A"
                            strsql += vbCrLf + " WHERE A.TRANDATE='" & Format(dtpTrandate.Value.Date, "yyyy-MM-dd") & "' "
                            strsql += vbCrLf + " AND A.TRANNO = " & txtEstNo.Text.Trim & ""
                            strsql += vbCrLf + " AND ISNULL(A.CANCEL,'')='' "
                            strsql += vbCrLf + " AND ISNULL(A.BATCHNO,'')='' "
                            strsql += vbCrLf + " AND A.SNO NOT IN (" & missSno & ")"
                            da = New OleDbDataAdapter(strsql, cn)
                            dtEstIs = New DataTable
                            da.Fill(dtEstIs)
                            If dtEstIs.Rows.Count > 0 Then
                                gridViewMissing.DataSource = Nothing
                                gridViewMissing.DataSource = dtEstIs
                                FormatGridColumns(gridViewMissing, False, True, True, False)
                                AutoResizeNewMethod(gridViewMissing)
                            End If
                        End If
                    End With
                Else
                    MsgBox("No Record found", MsgBoxStyle.Information)
                End If
            Else
                MsgBox("EstNo Should not Empty", MsgBoxStyle.Information)
                txtEstNo.Focus()
                txtEstNo.SelectAll()
            End If
        End If
    End Sub
#End Region


#Region " Button Events"
    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If gridViewMissing.RowCount > 0 Then
            BrightPosting.GExport.Post(Me.Name, "", "MIMR RETURN TAG CHECK", gridViewMissing, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub
#End Region
End Class