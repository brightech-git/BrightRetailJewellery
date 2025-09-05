Imports System.Data.OleDb
Imports System.Data
Public Class frmStockUpload
    Dim strsql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim dt As New DataTable
    Dim dtMergeHeader As New DataTable

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click, ViewToolStripMenuItem.Click

        strsql = " EXEC " & cnAdminDb & "..SP_STOCK_UPLOAD "
        strsql += vbCrLf + " @DBNAME='" & cnAdminDb & "'"
        strsql += vbCrLf + ",@FROMDATE='" & dtpdatefrom.Value.ToString("yyyy-MM-dd") & "' "
        strsql += vbCrLf + ",@TODATE='" & DateTo.Value.ToString("yyyy-MM-dd") & "' "
        cmd = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        For i As Integer = 1 To 10
            strsql = " update " & cnAdminDb & "..TEMPSTOCK set diasievesize" & i & "=NULL where diasievesize" & i & "=0  "
            strsql += vbCrLf + " update " & cnAdminDb & "..TEMPSTOCK set diapcs" & i & "=NULL where diapcs" & i & "=0  "
            strsql += vbCrLf + " update " & cnAdminDb & "..TEMPSTOCK set diacts" & i & "=NULL where diacts" & i & "=0  "
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
        Next
        For i As Integer = 1 To 5
            strsql = " update " & cnAdminDb & "..TEMPSTOCK set cstsievesize" & i & "=NULL where cstsievesize" & i & "=0  "
            strsql += vbCrLf + " update " & cnAdminDb & "..TEMPSTOCK set cstpcs" & i & "=NULL where cstpcs" & i & "=0  "
            strsql += vbCrLf + " update " & cnAdminDb & "..TEMPSTOCK set cstcts" & i & "=NULL where cstcts" & i & "=0  "
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
        Next
        strsql = "select * from " & cnAdminDb & "..TEMPSTOCK"
        dt = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            DataGridView.DataSource = Nothing
            DataGridView.DataSource = dt
            gridformat()
            Exit Sub
        Else
            MsgBox("No Record Found..!", MsgBoxStyle.Information)
            DataGridView.DataSource = Nothing
            lblTitle.Visible = False
            Exit Sub
        End If

    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

        Private Sub frmStockUpload_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            btnExport_Click(Me, New EventArgs)
        ElseIf e.KeyChar = "P" Or e.KeyChar = "p" Then
            btnPrint_Click(Me, New EventArgs)
        ElseIf e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            dtpdatefrom.Focus()
        End If
    End Sub

    Private Sub frmStockUpload_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        dtpdatefrom.Focus()
        lblTitle.Visible = False
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If DataGridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, DataGridView, BrightPosting.GExport.GExportType.Print, DataGridView1)
        End If
    End Sub

    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub gridView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles DataGridView.Scroll
        If DataGridView1 Is Nothing Then Exit Sub
        If Not DataGridView1.Columns.Count > 0 Then Exit Sub
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            DataGridView1.HorizontalScrollingOffset = e.NewValue
        End If
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                DataGridView1.HorizontalScrollingOffset = e.NewValue
                DataGridView1.Columns("SCROLL").Visible = CType(DataGridView.Controls(0), HScrollBar).Visible
                DataGridView1.Columns("SCROLL").Width = CType(DataGridView.Controls(1), VScrollBar).Width
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If DataGridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, DataGridView, BrightPosting.GExport.GExportType.Export, DataGridView1) ', DataGridView1
        End If

    End Sub
    Private Sub GridViewHeaderStyle()
        dtMergeHeader = New DataTable("~MERGEHEADER")
        With dtMergeHeader
            .Columns.Add("SLNO~STYLENO~KEYWORD~COLLECTION~CATEGORY~TYPE~PURITY~GRSWT~NETWT", GetType(String))
            .Columns.Add("diacut1~diaclarity1~diacolor1~diasievesize1~diapcs1~diacts1", GetType(String))
            .Columns.Add("DIACUT2~DIACLARITY2~DIACOLOR2~DIASIEVESIZE2~DIAPCS2~DIACTS2", GetType(String))
            .Columns.Add("DIACUT3~DIACLARITY3~DIACOLOR3~DIASIEVESIZE3~DIAPCS3~DIACTS3", GetType(String))
            .Columns.Add("DIACUT4~DIACLARITY4~DIACOLOR4~DIASIEVESIZE4~DIAPCS4~DIACTS4", GetType(String))
            .Columns.Add("DIACUT5~DIACLARITY5~DIACOLOR5~DIASIEVESIZE5~DIAPCS5~DIACTS5", GetType(String))
            .Columns.Add("DIACUT6~DIACLARITY6~DIACOLOR6~DIASIEVESIZE6~DIAPCS6~DIACTS6", GetType(String))
            .Columns.Add("DIACUT7~DIACLARITY7~DIACOLOR7~DIASIEVESIZE7~DIAPCS7~DIACTS7", GetType(String))
            .Columns.Add("DIACUT8~DIACLARITY8~DIACOLOR8~DIASIEVESIZE8~DIAPCS8~DIACTS8", GetType(String))
            .Columns.Add("DIACUT9~DIACLARITY9~DIACOLOR9~DIASIEVESIZE9~DIAPCS9~DIACTS9", GetType(String))
            .Columns.Add("DIACUT10~DIACLARITY10~DIACOLOR10~DIASIEVESIZE10~DIAPCS10~DIACTS10", GetType(String))
            .Columns.Add("CSTTYPE1~CSTCUT1~CSTCOLOR1~CSTSIEVESIZE1~CSTPCS1~CSTCTS1", GetType(String))
            .Columns.Add("CSTTYPE2~CSTCUT2~CSTCOLOR2~CSTSIEVESIZE2~CSTPCS2~CSTCTS2", GetType(String))
            .Columns.Add("CSTTYPE3~CSTCUT3~CSTCOLOR3~CSTSIEVESIZE3~CSTPCS3~CSTCTS3", GetType(String))
            .Columns.Add("CSTTYPE4~CSTCUT4~CSTCOLOR4~CSTSIEVESIZE4~CSTPCS4~CSTCTS4", GetType(String))
            .Columns.Add("CSTTYPE5~CSTCUT5~CSTCOLOR5~CSTSIEVESIZE5~CSTPCS5~CSTCTS5", GetType(String))
            .Columns.Add("TOT_DIAPCS~TOT_DIACTS~TOT_CSTPCS~TOT_CSTCTS~TOT_CTS~QTY", GetType(String))
            .Columns.Add("SCROLL", GetType(String))

            '.Columns("SLNO~STYLENO~KEYWORD~COLLECTION~CATEGORY~TYPE~PURITY~GRSWT~NETWT").Caption = "PARTICULARS"
            '.Columns("diacut1~diaclarity1~diacolor1~diasievesize1~diapcs1~diacts1").Caption = "DIAMOND 1"
            '.Columns("diacut2~diaclarity2~diacolor2~diasievesize2~diapcs2~diacts2").Caption = "DIAMOND 2"
            '.Columns("diacut3~diaclarity3~diacolor3~diasievesize3~diapcs3~diacts3").Caption = "DIAMOND 3"
            '.Columns("diacut4~diaclarity4~diacolor4~diasievesize4~diapcs4~diacts4").Caption = "DIAMOND 4"
            '.Columns("diacut5~diaclarity5~diacolor5~diasievesize5~diapcs5~diacts5").Caption = "DIAMOND 5"
            '.Columns("diacut6~diaclarity6~diacolor6~diasievesize6~diapcs6~diacts6").Caption = "DIAMOND 6"
            '.Columns("diacut7~diaclarity7~diacolor7~diasievesize7~diapcs7~diacts7").Caption = "DIAMOND 7"
            '.Columns("diacut8~diaclarity8~diacolor8~diasievesize8~diapcs8~diacts8").Caption = "DIAMOND 8"
            '.Columns("diacut9~diaclarity9~diacolor9~diasievesize9~diapcs9~diacts9").Caption = "DIAMOND 9"
            '.Columns("diacut10~diaclarity10~diacolor10~diasievesize10~diapcs10~diacts10").Caption = "DIAMOND 10"
            '.Columns("CSTTYPE1~CSTCUT1~CSTCOLOR1~CSTSIEVESIZE1~CSTPCS1~CSTCTS1").Caption = "STONE 1"
            '.Columns("CSTTYPE2~CSTCUT2~CSTCOLOR2~CSTSIEVESIZE2~CSTPCS2~CSTCTS2").Caption = "STONE 2"
            '.Columns("CSTTYPE3~CSTCUT3~CSTCOLOR3~CSTSIEVESIZE3~CSTPCS3~CSTCTS3").Caption = "STONE 3"
            '.Columns("CSTTYPE4~CSTCUT4~CSTCOLOR4~CSTSIEVESIZE4~CSTPCS4~CSTCTS4").Caption = "STONE 4"
            '.Columns("CSTTYPE5~CSTCUT5~CSTCOLOR5~CSTSIEVESIZE5~CSTPCS5~CSTCTS5").Caption = "STONE 5"
            '.Columns("TOT_DIAPCS~TOT_DIACTS~TOT_CSTPCS~TOT_CSTCTS~TOT_CTS~QTY").Caption = "TOTAL"
            '.Columns("SCROLL").Caption = ""
        End With
        With DataGridView1
            .DataSource = dtMergeHeader
            .Columns("SLNO~STYLENO~KEYWORD~COLLECTION~CATEGORY~TYPE~PURITY~GRSWT~NETWT").HeaderText = "PARTICULARS"
            .Columns("diacut1~diaclarity1~diacolor1~diasievesize1~diapcs1~diacts1").HeaderText = "DIAMOND 1"
            .Columns("diacut2~diaclarity2~diacolor2~diasievesize2~diapcs2~diacts2").HeaderText = "DIAMOND 2"
            .Columns("diacut3~diaclarity3~diacolor3~diasievesize3~diapcs3~diacts3").HeaderText = "DIAMOND 3"
            .Columns("diacut4~diaclarity4~diacolor4~diasievesize4~diapcs4~diacts4").HeaderText = "DIAMOND 4"
            .Columns("diacut5~diaclarity5~diacolor5~diasievesize5~diapcs5~diacts5").HeaderText = "DIAMOND 5"
            .Columns("diacut6~diaclarity6~diacolor6~diasievesize6~diapcs6~diacts6").HeaderText = "DIAMOND 6"
            .Columns("diacut7~diaclarity7~diacolor7~diasievesize7~diapcs7~diacts7").HeaderText = "DIAMOND 7"
            .Columns("diacut8~diaclarity8~diacolor8~diasievesize8~diapcs8~diacts8").HeaderText = "DIAMOND 8"
            .Columns("diacut9~diaclarity9~diacolor9~diasievesize9~diapcs9~diacts9").HeaderText = "DIAMOND 9"
            .Columns("diacut10~diaclarity10~diacolor10~diasievesize10~diapcs10~diacts10").HeaderText = "DIAMOND 10"
            .Columns("CSTTYPE1~CSTCUT1~CSTCOLOR1~CSTSIEVESIZE1~CSTPCS1~CSTCTS1").HeaderText = "STONE 1"
            .Columns("CSTTYPE2~CSTCUT2~CSTCOLOR2~CSTSIEVESIZE2~CSTPCS2~CSTCTS2").HeaderText = "STONE 2"
            .Columns("CSTTYPE3~CSTCUT3~CSTCOLOR3~CSTSIEVESIZE3~CSTPCS3~CSTCTS3").HeaderText = "STONE 3"
            .Columns("CSTTYPE4~CSTCUT4~CSTCOLOR4~CSTSIEVESIZE4~CSTPCS4~CSTCTS4").HeaderText = "STONE 4"
            .Columns("CSTTYPE5~CSTCUT5~CSTCOLOR5~CSTSIEVESIZE5~CSTPCS5~CSTCTS5").HeaderText = "STONE 5"
            .Columns("TOT_DIAPCS~TOT_DIACTS~TOT_CSTPCS~TOT_CSTCTS~TOT_CTS~QTY").HeaderText = "TOTAL"
            .Columns("SCROLL").HeaderText = ""
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            funcColWidth()
            DataGridView.Focus()
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To DataGridView.ColumnCount - 1
                If DataGridView.Columns(cnt).Visible Then colWid += DataGridView.Columns(cnt).Width
            Next
            If colWid >= DataGridView.Width Then
                .Columns("SCROLL").Visible = CType(DataGridView.Controls(0), HScrollBar).Visible
                .Columns("SCROLL").Width = CType(DataGridView.Controls(1), VScrollBar).Width
            Else
                .Columns("SCROLL").Visible = False
            End If
        End With
    End Sub

    Function funcColWidth() As Integer
        With DataGridView1
            .Columns("slno~styleno~keyword~collection~category~type~purity~grswt~netwt").Width = _
           IIf(DataGridView.Columns("slno").Visible, DataGridView.Columns("slno").Width, 0) _
          + IIf(DataGridView.Columns("styleno").Visible, DataGridView.Columns("styleno").Width, 0) _
          + IIf(DataGridView.Columns("keyword").Visible, DataGridView.Columns("keyword").Width, 0) _
          + IIf(DataGridView.Columns("collection").Visible, DataGridView.Columns("collection").Width, 0) _
          + IIf(DataGridView.Columns("category").Visible, DataGridView.Columns("category").Width, 0) _
          + IIf(DataGridView.Columns("type").Visible, DataGridView.Columns("type").Width, 0) _
          + IIf(DataGridView.Columns("purity").Visible, DataGridView.Columns("purity").Width, 0) _
          + IIf(DataGridView.Columns("grswt").Visible, DataGridView.Columns("grswt").Width, 0) _
          + IIf(DataGridView.Columns("netwt").Visible, DataGridView.Columns("netwt").Width, 0)
            .Columns("slno~styleno~keyword~collection~category~type~purity~grswt~netwt").HeaderText = "PARTICULARS"


            .Columns("diacut1~diaclarity1~diacolor1~diasievesize1~diapcs1~diacts1").Width = _
            IIf(DataGridView.Columns("diacut1").Visible, DataGridView.Columns("diacut1").Width, 0) _
            + IIf(DataGridView.Columns("diaclarity1").Visible, DataGridView.Columns("diaclarity1").Width, 0) _
            + IIf(DataGridView.Columns("diacolor1").Visible, DataGridView.Columns("diacolor1").Width, 0) _
            + IIf(DataGridView.Columns("diasievesize1").Visible, DataGridView.Columns("diasievesize1").Width, 0) _
            + IIf(DataGridView.Columns("diapcs1").Visible, DataGridView.Columns("diapcs1").Width, 0) _
            + IIf(DataGridView.Columns("diacts1").Visible, DataGridView.Columns("diacts1").Width, 0)
            .Columns("diacut1~diaclarity1~diacolor1~diasievesize1~diapcs1~diacts1").HeaderText = "DIAMOND 1"

            .Columns("DIACUT2~DIACLARITY2~DIACOLOR2~DIASIEVESIZE2~DIAPCS2~DIACTS2").Width = _
            IIf(DataGridView.Columns("diacut2").Visible, DataGridView.Columns("diacut2").Width, 0) _
            + IIf(DataGridView.Columns("diaclarity2").Visible, DataGridView.Columns("diaclarity2").Width, 0) _
            + IIf(DataGridView.Columns("diacolor2").Visible, DataGridView.Columns("diacolor2").Width, 0) _
            + IIf(DataGridView.Columns("diasievesize2").Visible, DataGridView.Columns("diasievesize2").Width, 0) _
            + IIf(DataGridView.Columns("diapcs2").Visible, DataGridView.Columns("diapcs2").Width, 0) _
            + IIf(DataGridView.Columns("diacts2").Visible, DataGridView.Columns("diacts2").Width, 0)
            .Columns("diacut2~diaclarity2~diacolor2~diasievesize2~diapcs2~diacts2").HeaderText = "DIAMOND 2"

            .Columns("diacut3~diaclarity3~diacolor3~diasievesize3~diapcs3~diacts3").Width = _
            IIf(DataGridView.Columns("diacut3").Visible, DataGridView.Columns("diacut3").Width, 0) _
            + IIf(DataGridView.Columns("diaclarity3").Visible, DataGridView.Columns("diaclarity3").Width, 0) _
            + IIf(DataGridView.Columns("diacolor3").Visible, DataGridView.Columns("diacolor3").Width, 0) _
            + IIf(DataGridView.Columns("diasievesize3").Visible, DataGridView.Columns("diasievesize3").Width, 0) _
            + IIf(DataGridView.Columns("diapcs3").Visible, DataGridView.Columns("diapcs3").Width, 0) _
            + IIf(DataGridView.Columns("diacts3").Visible, DataGridView.Columns("diacts3").Width, 0)
            .Columns("diacut3~diaclarity3~diacolor3~diasievesize3~diapcs3~diacts3").HeaderText = "DIAMOND 3"

            .Columns("diacut4~diaclarity4~diacolor4~diasievesize4~diapcs4~diacts4").Width = _
           IIf(DataGridView.Columns("diacut4").Visible, DataGridView.Columns("diacut4").Width, 0) _
           + IIf(DataGridView.Columns("diaclarity4").Visible, DataGridView.Columns("diaclarity4").Width, 0) _
           + IIf(DataGridView.Columns("diacolor4").Visible, DataGridView.Columns("diacolor4").Width, 0) _
           + IIf(DataGridView.Columns("diasievesize4").Visible, DataGridView.Columns("diasievesize4").Width, 0) _
           + IIf(DataGridView.Columns("diapcs4").Visible, DataGridView.Columns("diapcs4").Width, 0) _
           + IIf(DataGridView.Columns("diacts4").Visible, DataGridView.Columns("diacts4").Width, 0)
            .Columns("diacut4~diaclarity4~diacolor4~diasievesize4~diapcs4~diacts4").HeaderText = "DIAMOND 4"

            .Columns("diacut5~diaclarity5~diacolor5~diasievesize5~diapcs5~diacts5").Width = _
           IIf(DataGridView.Columns("diacut5").Visible, DataGridView.Columns("diacut5").Width, 0) _
           + IIf(DataGridView.Columns("diaclarity5").Visible, DataGridView.Columns("diaclarity5").Width, 0) _
           + IIf(DataGridView.Columns("diacolor5").Visible, DataGridView.Columns("diacolor5").Width, 0) _
           + IIf(DataGridView.Columns("diasievesize5").Visible, DataGridView.Columns("diasievesize5").Width, 0) _
           + IIf(DataGridView.Columns("diapcs5").Visible, DataGridView.Columns("diapcs5").Width, 0) _
           + IIf(DataGridView.Columns("diacts5").Visible, DataGridView.Columns("diacts5").Width, 0)
            .Columns("diacut5~diaclarity5~diacolor5~diasievesize5~diapcs5~diacts5").HeaderText = "DIAMOND 5"

            .Columns("diacut6~diaclarity6~diacolor6~diasievesize6~diapcs6~diacts6").Width = _
           IIf(DataGridView.Columns("diacut6").Visible, DataGridView.Columns("diacut6").Width, 0) _
           + IIf(DataGridView.Columns("diaclarity6").Visible, DataGridView.Columns("diaclarity6").Width, 0) _
           + IIf(DataGridView.Columns("diacolor6").Visible, DataGridView.Columns("diacolor6").Width, 0) _
           + IIf(DataGridView.Columns("diasievesize6").Visible, DataGridView.Columns("diasievesize6").Width, 0) _
           + IIf(DataGridView.Columns("diapcs6").Visible, DataGridView.Columns("diapcs6").Width, 0) _
           + IIf(DataGridView.Columns("diacts6").Visible, DataGridView.Columns("diacts6").Width, 0)
            .Columns("diacut6~diaclarity6~diacolor6~diasievesize6~diapcs6~diacts6").HeaderText = "DIAMOND 6"

            .Columns("diacut7~diaclarity7~diacolor7~diasievesize7~diapcs7~diacts7").Width = _
           IIf(DataGridView.Columns("diacut7").Visible, DataGridView.Columns("diacut7").Width, 0) _
           + IIf(DataGridView.Columns("diaclarity7").Visible, DataGridView.Columns("diaclarity7").Width, 0) _
           + IIf(DataGridView.Columns("diacolor7").Visible, DataGridView.Columns("diacolor7").Width, 0) _
           + IIf(DataGridView.Columns("diasievesize7").Visible, DataGridView.Columns("diasievesize7").Width, 0) _
           + IIf(DataGridView.Columns("diapcs7").Visible, DataGridView.Columns("diapcs7").Width, 0) _
           + IIf(DataGridView.Columns("diacts7").Visible, DataGridView.Columns("diacts7").Width, 0)
            .Columns("diacut7~diaclarity7~diacolor7~diasievesize7~diapcs7~diacts7").HeaderText = "DIAMOND 7"

            .Columns("diacut8~diaclarity8~diacolor8~diasievesize8~diapcs8~diacts8").Width = _
           IIf(DataGridView.Columns("diacut8").Visible, DataGridView.Columns("diacut8").Width, 0) _
           + IIf(DataGridView.Columns("diaclarity8").Visible, DataGridView.Columns("diaclarity8").Width, 0) _
           + IIf(DataGridView.Columns("diacolor8").Visible, DataGridView.Columns("diacolor8").Width, 0) _
           + IIf(DataGridView.Columns("diasievesize8").Visible, DataGridView.Columns("diasievesize8").Width, 0) _
           + IIf(DataGridView.Columns("diapcs8").Visible, DataGridView.Columns("diapcs8").Width, 0) _
           + IIf(DataGridView.Columns("diacts8").Visible, DataGridView.Columns("diacts8").Width, 0)
            .Columns("diacut8~diaclarity8~diacolor8~diasievesize8~diapcs8~diacts8").HeaderText = "DIAMOND 8"

            .Columns("diacut9~diaclarity9~diacolor9~diasievesize9~diapcs9~diacts9").Width = _
           IIf(DataGridView.Columns("diacut9").Visible, DataGridView.Columns("diacut9").Width, 0) _
           + IIf(DataGridView.Columns("diaclarity9").Visible, DataGridView.Columns("diaclarity9").Width, 0) _
           + IIf(DataGridView.Columns("diacolor9").Visible, DataGridView.Columns("diacolor9").Width, 0) _
           + IIf(DataGridView.Columns("diasievesize9").Visible, DataGridView.Columns("diasievesize9").Width, 0) _
           + IIf(DataGridView.Columns("diapcs9").Visible, DataGridView.Columns("diapcs9").Width, 0) _
           + IIf(DataGridView.Columns("diacts9").Visible, DataGridView.Columns("diacts9").Width, 0)
            .Columns("diacut9~diaclarity9~diacolor9~diasievesize9~diapcs9~diacts9").HeaderText = "DIAMOND 9"

            .Columns("diacut10~diaclarity10~diacolor10~diasievesize10~diapcs10~diacts10").Width = _
           IIf(DataGridView.Columns("diacut10").Visible, DataGridView.Columns("diacut10").Width, 0) _
           + IIf(DataGridView.Columns("diaclarity10").Visible, DataGridView.Columns("diaclarity10").Width, 0) _
           + IIf(DataGridView.Columns("diacolor10").Visible, DataGridView.Columns("diacolor10").Width, 0) _
           + IIf(DataGridView.Columns("diasievesize10").Visible, DataGridView.Columns("diasievesize10").Width, 0) _
           + IIf(DataGridView.Columns("diapcs10").Visible, DataGridView.Columns("diapcs10").Width, 0) _
           + IIf(DataGridView.Columns("diacts10").Visible, DataGridView.Columns("diacts10").Width, 0)
            .Columns("diacut10~diaclarity10~diacolor10~diasievesize10~diapcs10~diacts10").HeaderText = "DIAMOND 10"



            .Columns("csttype1~cstcut1~cstcolor1~cstsievesize1~cstpcs1~cstcts1").Width = _
            IIf(DataGridView.Columns("csttype1").Visible, DataGridView.Columns("csttype1").Width, 0) _
            + IIf(DataGridView.Columns("cstcut1").Visible, DataGridView.Columns("cstcut1").Width, 0) _
            + IIf(DataGridView.Columns("cstcolor1").Visible, DataGridView.Columns("cstcolor1").Width, 0) _
            + IIf(DataGridView.Columns("cstsievesize1").Visible, DataGridView.Columns("cstsievesize1").Width, 0) _
            + IIf(DataGridView.Columns("cstpcs1").Visible, DataGridView.Columns("cstpcs1").Width, 0) _
            + IIf(DataGridView.Columns("cstcts1").Visible, DataGridView.Columns("cstcts1").Width, 0)
            .Columns("csttype1~cstcut1~cstcolor1~cstsievesize1~cstpcs1~cstcts1").HeaderText = "STONE 1"


            .Columns("csttype2~cstcut2~cstcolor2~cstsievesize2~cstpcs2~cstcts2").Width = _
            IIf(DataGridView.Columns("csttype2").Visible, DataGridView.Columns("csttype2").Width, 0) _
            + IIf(DataGridView.Columns("cstcut2").Visible, DataGridView.Columns("cstcut2").Width, 0) _
            + IIf(DataGridView.Columns("cstcolor2").Visible, DataGridView.Columns("cstcolor2").Width, 0) _
            + IIf(DataGridView.Columns("cstsievesize2").Visible, DataGridView.Columns("cstsievesize2").Width, 0) _
            + IIf(DataGridView.Columns("cstpcs2").Visible, DataGridView.Columns("cstpcs2").Width, 0) _
            + IIf(DataGridView.Columns("cstcts2").Visible, DataGridView.Columns("cstcts2").Width, 0)
            .Columns("csttype2~cstcut2~cstcolor2~cstsievesize2~cstpcs2~cstcts2").HeaderText = "STONE 2"

            .Columns("csttype3~cstcut3~cstcolor3~cstsievesize3~cstpcs3~cstcts3").Width = _
            IIf(DataGridView.Columns("csttype3").Visible, DataGridView.Columns("csttype3").Width, 0) _
            + IIf(DataGridView.Columns("cstcut3").Visible, DataGridView.Columns("cstcut3").Width, 0) _
            + IIf(DataGridView.Columns("cstcolor3").Visible, DataGridView.Columns("cstcolor3").Width, 0) _
            + IIf(DataGridView.Columns("cstsievesize3").Visible, DataGridView.Columns("cstsievesize3").Width, 0) _
            + IIf(DataGridView.Columns("cstpcs3").Visible, DataGridView.Columns("cstpcs3").Width, 0) _
            + IIf(DataGridView.Columns("cstcts3").Visible, DataGridView.Columns("cstcts3").Width, 0)
            .Columns("csttype3~cstcut3~cstcolor3~cstsievesize3~cstpcs3~cstcts3").HeaderText = "STONE 3"

            .Columns("csttype4~cstcut4~cstcolor4~cstsievesize4~cstpcs4~cstcts4").Width = _
            IIf(DataGridView.Columns("csttype4").Visible, DataGridView.Columns("csttype4").Width, 0) _
            + IIf(DataGridView.Columns("cstcut4").Visible, DataGridView.Columns("cstcut4").Width, 0) _
            + IIf(DataGridView.Columns("cstcolor4").Visible, DataGridView.Columns("cstcolor4").Width, 0) _
            + IIf(DataGridView.Columns("cstsievesize4").Visible, DataGridView.Columns("cstsievesize4").Width, 0) _
            + IIf(DataGridView.Columns("cstpcs4").Visible, DataGridView.Columns("cstpcs4").Width, 0) _
            + IIf(DataGridView.Columns("cstcts4").Visible, DataGridView.Columns("cstcts4").Width, 0)
            .Columns("csttype4~cstcut4~cstcolor4~cstsievesize4~cstpcs4~cstcts4").HeaderText = "STONE 4"

            .Columns("csttype5~cstcut5~cstcolor5~cstsievesize5~cstpcs5~cstcts5").Width = _
            IIf(DataGridView.Columns("csttype5").Visible, DataGridView.Columns("csttype5").Width, 0) _
            + IIf(DataGridView.Columns("cstcut5").Visible, DataGridView.Columns("cstcut5").Width, 0) _
            + IIf(DataGridView.Columns("cstcolor5").Visible, DataGridView.Columns("cstcolor5").Width, 0) _
            + IIf(DataGridView.Columns("cstsievesize5").Visible, DataGridView.Columns("cstsievesize5").Width, 0) _
            + IIf(DataGridView.Columns("cstpcs5").Visible, DataGridView.Columns("cstpcs5").Width, 0) _
            + IIf(DataGridView.Columns("cstcts5").Visible, DataGridView.Columns("cstcts5").Width, 0)
            .Columns("csttype5~cstcut5~cstcolor5~cstsievesize5~cstpcs5~cstcts5").HeaderText = "STONE 5"

            .Columns("TOT_DIAPCS~TOT_DIACTS~TOT_CSTPCS~TOT_CSTCTS~TOT_CTS~QTY").Width = _
            IIf(DataGridView.Columns("TOT_DIAPCS").Visible, DataGridView.Columns("TOT_DIAPCS").Width, 0) _
            + IIf(DataGridView.Columns("TOT_DIACTS").Visible, DataGridView.Columns("TOT_DIACTS").Width, 0) _
            + IIf(DataGridView.Columns("TOT_CSTPCS").Visible, DataGridView.Columns("TOT_CSTPCS").Width, 0) _
            + IIf(DataGridView.Columns("TOT_CSTCTS").Visible, DataGridView.Columns("TOT_CSTCTS").Width, 0) _
            + IIf(DataGridView.Columns("TOT_CTS").Visible, DataGridView.Columns("TOT_CTS").Width, 0) _
            + IIf(DataGridView.Columns("QTY").Visible, DataGridView.Columns("QTY").Width, 0)
            .Columns("TOT_DIAPCS~TOT_DIACTS~TOT_CSTPCS~TOT_CSTCTS~TOT_CTS~QTY").HeaderText = "TOTAL"


        End With
    End Function

    Private Sub AutoReToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoReToolStripMenuItem.Click
        If DataGridView.RowCount > 0 Then
            AutoReToolStripMenuItem.Checked = True
            If AutoReToolStripMenuItem.Checked Then
                DataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                DataGridView.Invalidate()
                For Each dgvCol As DataGridViewColumn In DataGridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                DataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In DataGridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                DataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
            GridViewHeaderStyle()
        End If

        


    End Sub
    Function gridformat()
        With DataGridView
            .Columns("tagsno").Visible = False
            .Columns("purity").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            Dim str As String
            For k As Integer = 1 To 10
                str = "diacut" & k
                .Columns(str).HeaderText = "CUT"
                str = "diaclarity" & k
                .Columns(str).HeaderText = "CLARITY"
                str = "diacolor" & k
                .Columns(str).HeaderText = "COLOR"
                str = "diapcs" & k
                .Columns(str).HeaderText = "PCS"
                str = "diacts" & k
                .Columns(str).HeaderText = "CTS"
                str = "diaSIEVESIZE" & k
                .Columns(str).HeaderText = "SIEVESIZE"
            Next
            For l As Integer = 1 To 5
                str = "csttype" & l
                .Columns(str).HeaderText = "TYPE"
                str = "cstcut" & l
                .Columns(str).HeaderText = "CUT"
                str = "cstcolor" & l
                .Columns(str).HeaderText = "COLOR"
                str = "cstsievesize" & l
                .Columns(str).HeaderText = "SIEVESIZE"
                str = "cstpcs" & l
                .Columns(str).HeaderText = "PCS"
                str = "cstcts" & l
                .Columns(str).HeaderText = "CTS"
            Next
        End With
        FormatGridColumns(DataGridView, False, False, True, False)
        FormatGridColumns(DataGridView1, False, False, True, False)
        AutoReToolStripMenuItem_Click(Me, New EventArgs)
        Dim tit As String = " STOCK UPLOAD REPORT FROM " & dtpdatefrom.Value.ToString("dd-MM-yyyy") & " TO " & DateTo.Value.ToString("dd-MM-yyyy") & ""
        lblTitle.Text = tit
        lblTitle.Visible = True
    End Function

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpdatefrom.Text = Today.ToString("dd-MM-yyyy")
        DateTo.Text = Today.ToString("dd-MM-yyyy")
        DataGridView.DataSource = Nothing
        DataGridView1.DataSource = Nothing
        lblTitle.Text = ""
    End Sub

End Class