Imports System.IO
Imports System.Data.OleDb

Public Class frmItemMastePosting
    Dim StrSql As String
    Dim Da As OleDbDataAdapter
    Dim Cmd As OleDbCommand
    Dim dt As DataTable
    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click
        StrSql = "  SELECT METALNAME,CATNAME AS CATEGORY,ITEMNAME,SUBITEMNAME "
        strsql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMMAST AS I"
        strsql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CATEGORY AS C ON I.CATCODE=C.CATCODE"
        strsql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..METALMAST AS M ON I.METALID=M.METALID "
        StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..SUBITEMMAST AS S ON I.ITEMID=S.ITEMID "
        StrSql += vbCrLf + "  WHERE 1=1 "
        If chkcmbMetal.Text <> "ALL" And chkcmbMetal.Text <> "" Then
            StrSql += " AND M.METALNAME IN (" & GetQryString(chkcmbMetal.Text) & ")"
        End If
        If chkcmbCategory.Text <> "ALL" And chkcmbCategory.Text <> "" Then
            StrSql += " AND  C.CATNAME IN (" & GetQryString(chkcmbCategory.Text) & ")"
        End If
        If chkcmbItemName.Text <> "ALL" And chkcmbItemName.Text <> "" Then
            StrSql += " AND  I.ITEMNAME IN (" & GetQryString(chkcmbItemName.Text) & ")"
        End If
        If chkcmbSubItemName.Text <> "ALL" And chkcmbSubItemName.Text <> "" Then
            StrSql += " AND  S.SUBITEMNAME IN (" & GetQryString(chkcmbSubItemName.Text) & ")"
        End If
        strsql += vbCrLf + "  ORDER BY METALNAME,CATNAME,ITEMNAME,SUBITEMNAME "
        cmd = New OleDbCommand(strsql, cn)
        da = New OleDbDataAdapter(cmd)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Dim strMetal As String = ""
            Dim strCat As String = ""
            Dim strItem As String = ""
            Dim strSubItem As String = ""
            For i As Integer = 0 To dt.Rows.Count - 1
                If strMetal <> dt.Rows(i)("METALNAME").ToString Then
                    strMetal = dt.Rows(i)("METALNAME").ToString
                Else
                    dt.Rows(i)("METALNAME") = ""
                End If
                If strCat <> dt.Rows(i)("CATEGORY").ToString Then
                    strCat = dt.Rows(i)("CATEGORY").ToString
                Else
                    dt.Rows(i)("CATEGORY") = ""
                End If
                If strItem <> dt.Rows(i)("ITEMNAME").ToString Then
                    strItem = dt.Rows(i)("ITEMNAME").ToString
                Else
                    dt.Rows(i)("ITEMNAME") = ""
                End If
                If strSubItem <> dt.Rows(i)("SUBITEMNAME").ToString Then
                    strSubItem = dt.Rows(i)("SUBITEMNAME").ToString
                Else
                    dt.Rows(i)("SUBITEMNAME") = ""
                End If
            Next
            dgv.DataSource = Nothing
            dgv.DataSource = dt
            For Each dgvRow As DataGridViewRow In dgv.Rows
                With dgvRow
                    Select Case .Cells("METALNAME").Value.ToString
                        Case Is <> ""
                            .Cells("METALNAME").Style.BackColor = Color.Wheat
                    End Select
                    Select Case .Cells("CATEGORY").Value.ToString
                        Case Is <> ""
                            .Cells("CATEGORY").Style.BackColor = Color.MistyRose
                    End Select
                    Select Case .Cells("ITEMNAME").Value.ToString
                        Case Is <> ""
                            .Cells("ITEMNAME").Style.BackColor = Color.LightGreen
                    End Select
                End With
            Next
            For j As Integer = 0 To dgv.Columns.Count - 1
                dgv.Columns(j).SortMode = DataGridViewColumnSortMode.NotSortable
                dgv.Columns(j).Width = 150
            Next
        Else
            dgv.DataSource = Nothing
            MsgBox("Record not found...")
            chkcmbMetal.Focus()
        End If
    End Sub
    Function funcLoadCategory()

        chkcmbCategory.Items.Clear()
        chkcmbCategory.Items.Add("ALL")
        StrSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
        If chkcmbMetal.Text <> "ALL" And chkcmbMetal.Text <> "" Then
            StrSql += " WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(chkcmbMetal.Text) & "))"
        End If
        StrSql += "  ORDER BY CATNAME"
        Dim dt As New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dt)
        For i As Integer = 0 To dt.Rows.Count - 1
            chkcmbCategory.Items.Add(dt.Rows(i).Item("CATNAME").ToString)
        Next
    End Function

    Function funcLoadItemName()
        chkcmbItemName.Items.Clear()
        chkcmbItemName.Items.Add("ALL")
        StrSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ACTIVE,'') = 'Y'"
        StrSql += " AND ISNULL(STOCKREPORT,'') = 'Y'"
        If chkcmbMetal.Text <> "ALL" And chkcmbMetal.Text <> "" Then
            StrSql += " AND  METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetQryString(chkcmbMetal.Text) & "))"
        End If
        If chkcmbCategory.Text <> "ALL" And chkcmbCategory.Text <> "" Then
            StrSql += " AND  CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & GetQryString(chkcmbCategory.Text) & "))"
        End If
        StrSql += "  ORDER BY ITEMID"
        Dim dt As New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dt)
        For i As Integer = 0 To dt.Rows.Count - 1
            chkcmbItemName.Items.Add(dt.Rows(i).Item("ITEMNAME").ToString)
        Next
    End Function

    Function funcLoadSubItemName()
        chkcmbSubItemName.Items.Clear()
        chkcmbSubItemName.Items.Add("ALL")
        StrSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ISNULL(ACTIVE,'') = 'Y'"
        If chkcmbItemName.Text <> "ALL" And chkcmbItemName.Text <> "" Then
            StrSql += " AND  ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkcmbItemName.Text) & "))"
        End If
        StrSql += "  ORDER BY SUBITEMID"
        Dim dt As New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dt)
        For i As Integer = 0 To dt.Rows.Count - 1
            chkcmbSubItemName.Items.Add(dt.Rows(i).Item("SUBITEMNAME").ToString)
        Next
    End Function

    Function funcLoadMetal()
        chkcmbMetal.Items.Clear()
        chkcmbMetal.Items.Add("ALL")
        StrSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY METALNAME"
        Dim dt As New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dt)
        For i As Integer = 0 To dt.Rows.Count - 1
            chkcmbMetal.Items.Add(dt.Rows(i).Item("METALNAME").ToString)
        Next
    End Function
    Private Sub frmItemMastePosting_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmItemMastePosting_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        chkcmbCategory.Items.Clear()
        chkcmbMetal.Items.Clear()
        chkcmbItemName.Items.Clear()
        chkcmbSubItemName.Items.Clear()
        dgv.DataSource = Nothing
        funcLoadMetal()
        chkcmbMetal.Focus()
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        BrightPosting.GExport.Post(Me.Name, strCompanyName, "ITEM MASTER POSTING", dgv, BrightPosting.GExport.GExportType.Export, gridViewHead)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub


    Private Sub chkcmbCategory_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkcmbCategory.KeyDown
        If e.KeyCode = Keys.Enter Then
            funcLoadItemName()
            If chkcmbCategory.Text = "" Then
                chkcmbCategory.SelectedText = "ALL"
            End If
        End If
    End Sub

    Private Sub chkcmbMetal_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkcmbMetal.KeyDown
        If e.KeyCode = Keys.Enter Then
            funcLoadCategory()
            If chkcmbMetal.Text = "" Then
                chkcmbMetal.SelectedText = "ALL"
            End If
        End If
    End Sub

    Private Sub chkcmbItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkcmbItemName.KeyDown
        If e.KeyCode = Keys.Enter Then
            funcLoadSubItemName()
            If chkcmbItemName.Text = "" Then
                chkcmbItemName.SelectedText = "ALL"
            End If
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        BrightPosting.GExport.Post(Me.Name, strCompanyName, "ITEM MASTER POSTING", dgv, BrightPosting.GExport.GExportType.Print, gridViewHead)
    End Sub

    Private Sub chkcmbSubItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkcmbSubItemName.KeyDown
        If e.KeyCode = Keys.Enter Then
            If chkcmbSubItemName.Text = "" Then
                chkcmbSubItemName.SelectedText = "ALL"
            End If
        End If
    End Sub

 
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        frmItemMastePosting_Load(Me, e)
    End Sub

    Private Sub GroupBox1_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupBox1.Enter

    End Sub
End Class