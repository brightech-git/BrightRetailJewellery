Imports System.Data.OleDb
Public Class frmTrialBalanceDetail
    Dim strSql As String
    Dim openCheck As Boolean
    Dim transCheck As Boolean
    Dim WithEvents btnExcel As New Button

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Sub New(ByVal acGrpName As String, ByVal strLblTitleDet As String, ByVal boolOpenCheck As Boolean, ByVal boolTransCheck As Boolean)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        openCheck = boolOpenCheck
        transCheck = boolTransCheck
        Dim dtDetail As New DataTable
        strSql = "SELECT "
        strSql += " ACNAME,"
        strSql += " CASE WHEN ODEBIT=0 THEN NULL ELSE ODEBIT END ODEBIT,"
        strSql += " CASE WHEN OCREDIT=0 THEN NULL ELSE OCREDIT END OCREDIT,"
        strSql += " CASE WHEN TDEBIT=0 THEN NULL ELSE TDEBIT END TDEBIT,"
        strSql += " CASE WHEN TCREDIT=0 THEN NULL ELSE TCREDIT END TCREDIT,"
        strSql += " CASE WHEN CDEBIT=0 THEN NULL ELSE CDEBIT END CDEBIT,"
        strSql += " CASE WHEN CCREDIT=0 THEN NULL ELSE CCREDIT END CCREDIT,"
        strSql += " RESULT"
        strSql += " FROM TEMP" & systemId & "TRAILBAL "
        strSql += " WHERE AcGrpName = '" & acGrpName & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtDetail)
        gridDetailView.DataSource = Nothing
        If dtDetail.Rows.Count > 0 Then
            gridDetailView.DataSource = dtDetail
            headGrid()
            funcDetailViewStyle()
            funcDetailGridHeadStyle()
            Me.StartPosition = FormStartPosition.CenterScreen
            lbltitledet.Text = strLblTitleDet
            lbltitledet.Visible = True
            lbltitledet.Font = New Font("VERDANA", 8, FontStyle.Bold)
            grpDetailView.Visible = True
            grpDetailView.BringToFront()
            For cnt As Integer = 0 To gridDetailViewHead.ColumnCount - 1
                gridDetailViewHead.Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
                gridDetailViewHead.Columns(cnt).Resizable = DataGridViewTriState.False
            Next
            gridDetailView.Focus()
        End If
    End Sub

    Function funcDetailViewStyle() As Integer
        With gridDetailView
            'With .Columns("AcCode")
            '    .Visible = False
            'End With
            'With .Columns("AcGrpCode")
            '    .Visible = False
            'End With
            'With .Columns("AcGrpName")
            '    .Visible = False
            'End With
            'With .Columns("TempAcGrpName")
            '    .HeaderText = "A/C NAME"
            '    .Width = 250
            '    .Visible = False
            'End With
            With .Columns("ACNAME")
                .Visible = True
                .Width = 250
            End With
            With .Columns("ODEBIT")
                .HeaderText = "DEBIT"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("OCREDIT")
                .HeaderText = "CREDIT"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            If openCheck = True Then
                .Columns("ODEBIT").Visible = True
                .Columns("OCREDIT").Visible = True
            Else
                .Columns("ODEBIT").Visible = False
                .Columns("OCREDIT").Visible = False
            End If
            With .Columns("TDEBIT")
                .HeaderText = "DEBIT"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("TCREDIT")
                .HeaderText = "CREDIT"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            If transCheck = True Then
                .Columns("TDEBIT").Visible = True
                .Columns("TCREDIT").Visible = True
            Else
                .Columns("TDEBIT").Visible = False
                .Columns("TCREDIT").Visible = False
            End If
            With .Columns("CDEBIT")
                .HeaderText = "DEBIT"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("CCREDIT")
                .HeaderText = "CREDIT"
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("Result")
                .Visible = False
            End With
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        End With
    End Function

    Function funcDetailGridHeadStyle() As Integer
        With gridDetailViewHead
            With .Columns("ACNAME")
                .Width = gridDetailView.Columns("ACNAME").Width
                .HeaderText = ""
            End With
            If Not openCheck Then .Columns("OPENING").Visible = False
            With .Columns("OPENING")
                If openCheck = True Then
                    .Visible = True
                Else
                    .Visible = False
                End If
                .HeaderText = "OPENING"
                .Width = gridDetailView.Columns("ODEBIT").Width + gridDetailView.Columns("OCREDIT").Width
            End With
            If Not transCheck Then .Columns("TRANSACT").Visible = False
            With .Columns("TRANSACT")
                If transCheck = True Then
                    .Visible = True
                Else
                    .Visible = False
                End If
                .HeaderText = "TRANSACT"
                .Width = gridDetailView.Columns("TDEBIT").Width + gridDetailView.Columns("TCREDIT").Width
            End With

            With .Columns("CLOSING")
                .HeaderText = "CLOSING"
                .Width = gridDetailView.Columns("CDEBIT").Width + gridDetailView.Columns("CCREDIT").Width
                .Visible = True
            End With
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8.25!, FontStyle.Bold)
        End With
    End Function

    Function headGrid() As Integer
        ''DETIAL Grid heading 
        strSql = " SELECT 'ACNAME'ACNAME,'OPENING'OPENING,'TRANSACT'TRANSACT,'CLOSING'CLOSING where 1<>1"
        Dim dtHead As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtHead)
        With gridDetailViewHead
            .DataSource = dtHead
            With gridDetailView
                If .ColumnCount > 0 Then
                    gridDetailViewHead.Columns("ACNAME").Width = .Columns("ACNAME").Width
                    gridDetailViewHead.Columns("OPENING").Width = .Columns("ODEBIT").Width + .Columns("OCREDIT").Width
                    gridDetailViewHead.Columns("TRANSACT").Width = .Columns("TDEBIT").Width + .Columns("TCREDIT").Width
                    gridDetailViewHead.Columns("CLOSING").Width = .Columns("CDEBIT").Width + .Columns("CCREDIT").Width
                End If
            End With
            '.Columns("ACNAME").Width = 400
            '.Columns("OPENING").Width = 200
            '.Columns("TRANSACT").Width = 200
            '.Columns("CLOSING").Width = 200
        End With
    End Function

    Private Sub frmTrialBalanceDetail_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        ElseIf UCase(e.KeyCode) = Keys.P Then
            If gridDetailView.Rows.Count > 0 Then
                Dim title As String
                title = Trim(lbltitledet.Text)
                title += Environment.NewLine
                If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
                BrightPosting.GExport.Post(Me.Name, strCompanyName, title, gridDetailView, BrightPosting.GExport.GExportType.Print)
            End If
        ElseIf UCase(e.KeyCode) = Keys.X Then
            Dim title As String
            title = Trim(lbltitledet.Text)
            title += Environment.NewLine
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
            If gridDetailView.Rows.Count > 0 Then
                BrightPosting.GExport.Post(Me.Name, strCompanyName, title, gridDetailView, BrightPosting.GExport.GExportType.Export)
            End If
        End If
    End Sub

    Private Sub gridDetailView_ColumnWidthChanged1(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridDetailView.ColumnWidthChanged
        With gridDetailView
            If .ColumnCount > 0 Then
                gridDetailViewHead.Columns("ACNAME").Width = .Columns("ACNAME").Width
                gridDetailViewHead.Columns("OPENING").Width = .Columns("ODEBIT").Width + .Columns("OCREDIT").Width
                gridDetailViewHead.Columns("TRANSACT").Width = .Columns("TDEBIT").Width + .Columns("TCREDIT").Width
                gridDetailViewHead.Columns("CLOSING").Width = .Columns("CDEBIT").Width + .Columns("CCREDIT").Width
            End If
        End With
    End Sub

    Private Sub gridDetailView_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridDetailView.CellContentClick

    End Sub
End Class