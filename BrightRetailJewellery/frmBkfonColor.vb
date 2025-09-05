Imports System.Data.OleDb
Imports System.Text.RegularExpressions
Public Class frmBkfonColor
#Region " Variable"
    Dim strsql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim dtGrid As New DataTable
    Dim _Tran As OleDbTransaction = Nothing
    Dim dt As New DataTable

    Dim FORMCOLOR As String = "FORMCOLOR"
    Dim TEXTBOXCOLOR As String = "TEXTBOXCOLOR"
    Dim COMBOBOXCOLOR As String = "COMBOBOXCOLOR"
    Dim GRIDCOLOR As String = "GRIDCOLOR"
    Dim RADIOCOLOR As String = "RADIOCOLOR"
    Dim BUTTONCOLOR As String = "BUTTONCOLOR"
#End Region

#Region " Constructor"
    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub
#End Region

#Region " Form Load"
    Private Sub frmBkfonColor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnNew_Click(Me, New System.EventArgs)
    End Sub
    Private Sub frmBkfonColor_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{{TAB}}")
        End If
    End Sub
#End Region

#Region " Userdefine Function"

    Function AutoResize()
        If gridview.RowCount > 0 Then
            If True Then
                gridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridview.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridview.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridview.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
            'gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End If
    End Function

    Function GetTable() As DataTable
        ' Create four typed columns in the DataTable.
        dtGrid.Columns.Add("IMAGENAME", GetType(String))
        dtGrid.Columns.Add("FONTCOLOR", GetType(String))
        dtGrid.Columns.Add("BGCOLOR", GetType(String))
    End Function
#End Region

#Region " Button Events"
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            If gridview.Rows.Count = 0 Then
                Exit Sub
            End If
            Dim dtSave As New DataTable
            dtSave = gridview.DataSource
            If dtSave.Rows.Count > 0 Then
                _Tran = Nothing
                _Tran = cn.BeginTransaction
                For i As Integer = 0 To dtSave.Rows.Count - 1


                    strsql = ""
                    strsql += vbCrLf + " DELETE " & cnAdminDb & "..STYLE"
                    strsql += vbCrLf + " WHERE IMGNAME = '" & dtSave.Rows(i).Item("IMAGENAME").ToString & "'"
                    strsql += vbCrLf + " AND  ISNULL(LOGO,'')= 'N'"
                    cmd = New OleDbCommand(strsql, cn, _Tran)
                    cmd.ExecuteNonQuery()

                    strsql = ""
                    strsql += vbCrLf + " INSERT INTO " & cnAdminDb & "..STYLE(BGCOLOR,IMGNAME,TRANDATE,USERID,LOGO)"
                    strsql += vbCrLf + " VALUES "
                    strsql += vbCrLf + " ( "
                    strsql += vbCrLf + " '" & dtSave.Rows(i).Item("BGCOLOR").ToString & "' "
                    strsql += vbCrLf + " ,'" & dtSave.Rows(i).Item("IMAGENAME").ToString & "' "
                    strsql += vbCrLf + " ,GETDATE() "
                    strsql += vbCrLf + " ," & userId & " "
                    strsql += vbCrLf + " ,'N'"
                    strsql += vbCrLf + " )"
                    cmd = New OleDbCommand(strsql, cn, _Tran)
                    cmd.ExecuteNonQuery()
                Next
                _Tran.Commit()
                _Tran = Nothing
            End If
            btnNew_Click(Me, New System.EventArgs)
            MsgBox("saved successfull once again restart application")
        Catch ex As Exception
            If Not _Tran Is Nothing Then
                _Tran.Rollback()
                _Tran = Nothing
                MessageBox.Show(ex.ToString)
            Else
                MessageBox.Show(ex.ToString)
            End If
        End Try
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        dtGrid = New DataTable
        GetTable()
        gridview.DataSource = Nothing
        strsql = "SELECT IMGNAME,BGCOLOR,FONTCOLOR FROM " & cnAdminDb & "..STYLE WHERE ISNULL(LOGO,'')= 'N'"
        da = New OleDbDataAdapter(strsql, cn)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            For i As Integer = 0 To dt.Rows.Count - 1
                AddGridview(dt.Rows(i).Item("IMGNAME").ToString, dt.Rows(i).Item("FONTCOLOR").ToString, dt.Rows(i).Item("BGCOLOR").ToString)
                SurroundingSub(dt.Rows(i).Item("BGCOLOR").ToString, dt.Rows(i).Item("IMGNAME").ToString)
            Next
        End If
    End Sub
    Private Sub SurroundingSub(ByVal bgcolor As String, ByVal _imageName As String)
        'Dim bgcolor As String = "Color [A=255, R=128, G=0, B=255]"
        Dim c As Color
        Dim m As Match = Regex.Match(bgcolor, "A=(?<Alpha>\d+),\s*R=(?<Red>\d+),\s*G=(?<Green>\d+),\s*B=(?<Blue>\d+)")
        If m.Success Then
            Dim alpha As Integer = Integer.Parse(m.Groups("Alpha").Value)
            Dim red As Integer = Integer.Parse(m.Groups("Red").Value)
            Dim green As Integer = Integer.Parse(m.Groups("Green").Value)
            Dim blue As Integer = Integer.Parse(m.Groups("Blue").Value)
            c = Color.FromArgb(alpha, red, green, blue)
        Else
            Dim cName As String = bgcolor.Replace("Color [", "").Replace("]", "")
            c = System.Drawing.Color.FromName(bgcolor.Replace("Color [", "").Replace("]", ""))
        End If
        If _imageName = FORMCOLOR Then
            Panel1.BackColor = c
        ElseIf _imageName = TEXTBOXCOLOR Then
            txtFontColor.BackColor = c
        ElseIf _imageName = COMBOBOXCOLOR Then
            cmbFontColor.BackColor = c
        ElseIf _imageName = GRIDCOLOR Then
            GridFontColor_OWN.GridColor = c
        ElseIf _imageName = RADIOCOLOR Then
            rbtFontColor.BackColor = c
        ElseIf _imageName = BUTTONCOLOR Then
            btnFontColor.BackColor = c
        End If

    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub AddGridview(ByVal _imageName As String, ByVal _fontColor As String, ByVal _bgcolorname As String)
        With gridview
            For i As Integer = 0 To dtGrid.Rows.Count - 1
                If dtGrid.Rows(i).Item("IMAGENAME").ToString = _imageName Then
                    dtGrid.Rows.Remove(dtGrid.Rows(i))
                    Exit For
                End If
            Next
            Dim dr As DataRow = Nothing
            dr = dtGrid.NewRow
            dr!IMAGENAME = _imageName
            dr!FONTCOLOR = _fontColor
            dr!BGCOLOR = _bgcolorname
            dtGrid.Rows.Add(dr)
            .DataSource = Nothing
            .DataSource = dtGrid
            gridview.Columns("FONTCOLOR").Visible = False
            FormatGridColumns(gridview, False, True, True, True)
            AutoResize()
        End With

    End Sub

#End Region

#Region "Button Event"


    Private Sub btnFormColor_Click(sender As Object, e As EventArgs) Handles btnFormColor.Click
        If ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            Panel1.BackColor = ColorDialog1.Color

            'Dim colorFromArg As String = "Color [A=255, R=255, G=128, B=128]"

            If ColorDialog1.Color.IsKnownColor = False Then
                AddGridview(FORMCOLOR, "", ColorDialog1.Color.ToString)
            Else
                AddGridview(FORMCOLOR, "", ColorDialog1.Color.ToString)
            End If


        End If
    End Sub

    Private Sub btntxtColor_Click(sender As Object, e As EventArgs) Handles btntxtColor.Click
        If ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            txtFontColor.BackColor = ColorDialog1.Color
            AddGridview(TEXTBOXCOLOR, "", ColorDialog1.Color.ToString())
        End If
    End Sub

    Private Sub btnComboxColor_Click(sender As Object, e As EventArgs) Handles btnComboxColor.Click
        If ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            cmbFontColor.BackColor = ColorDialog1.Color
            AddGridview(COMBOBOXCOLOR, "", ColorDialog1.Color.ToString())
        End If
    End Sub

    Private Sub btnGridviewColor_Click(sender As Object, e As EventArgs) Handles btnGridviewColor.Click
        If ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            GridFontColor_OWN.GridColor = ColorDialog1.Color
            AddGridview(GRIDCOLOR, "", ColorDialog1.Color.ToString())
        End If
    End Sub

    Private Sub btnRadioColor_Click(sender As Object, e As EventArgs) Handles btnRadioColor.Click
        If ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            rbtFontColor.BackColor = ColorDialog1.Color
            AddGridview(RADIOCOLOR, "", ColorDialog1.Color.ToString())
        End If
    End Sub

    Private Sub btnButtoncolor_Click(sender As Object, e As EventArgs) Handles btnButtoncolor.Click
        If ColorDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            btnFontColor.BackColor = ColorDialog1.Color
            AddGridview(BUTTONCOLOR, "", ColorDialog1.Color.ToString())
        End If
    End Sub
#End Region

End Class