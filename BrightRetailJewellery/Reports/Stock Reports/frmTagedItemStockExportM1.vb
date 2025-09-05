Imports System.Data.OleDb
Public Class frmTagedItemStockExportM1
#Region " Variable"
    Dim strsql As String = ""
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim dt As New DataTable
    Dim _ItemId As String = ""
    Dim _SubItemId As String = ""
    Dim _Fromdate As String = ""
    Dim _Todate As String = ""
    Dim _costid As String = ""
    Dim _Type As String = ""
#End Region

#Region " Construcotr"
    Public Sub New(ByVal Itemid As String, ByVal SubitemId As String, ByVal fromdate As String, ByVal todate As String, ByVal costname As String, ByVal Type As String)
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        _Type = Type
        _ItemId = Itemid
        _SubItemId = SubitemId
        _Fromdate = fromdate
        _Todate = todate
        If costname = "ALL" Then
            _costid = ""
        Else
            strsql = " SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & costname & ") "
            da = New OleDbDataAdapter(strsql, cn)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    With dt.Rows(i)
                        _costid += "''" & .Item("COSTID").ToString & "''"
                        If i <> dt.Rows.Count - 1 Then
                            _costid += ","
                        End If
                    End With
                Next
            Else
                _costid = ""
            End If
        End If
        da = New OleDbDataAdapter
        dt = New DataTable
        strsql = ""
    End Sub
#End Region


#Region "Button Events"
    Private Sub btnView_Click(sender As Object, e As EventArgs) Handles btnView.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            gridView.DataSource = Nothing

            If rbtTagDetail.Checked = True Then
                strsql = ""
                strsql += vbCrLf + " EXEC " & cnAdminDb & "..[WEB_STKTAGVIEWDETAIL] "
                strsql += vbCrLf + " @RECDATE='RECDATE'"
                strsql += vbCrLf + " ,@FROMDATE='" & _Fromdate & "'"
                strsql += vbCrLf + " ,@TODATE='" & _Todate & "'"
                strsql += vbCrLf + " ,@CUMMULATIVE ='N'"
                strsql += vbCrLf + " ,@ITEMID='" & _ItemId & "'"
                strsql += vbCrLf + " ,@SUBITEMID=''"
                strsql += vbCrLf + " ,@TAGNO =''"
                strsql += vbCrLf + " ,@DESIGNERID=''"
                strsql += vbCrLf + " ,@COSTID='" & _costid & "'"
                strsql += vbCrLf + " ,@TYPE='" & _Type & "'"
            ElseIf rbtStoneDetail.Checked = True Then
                strsql = ""
                strsql += vbCrLf + " EXEC " & cnAdminDb & "..[WEB_STKTAGSTONEVIEWDETAIL] "
                strsql += vbCrLf + " @RECDATE='RECDATE'"
                strsql += vbCrLf + " ,@FROMDATE='" & _Fromdate & "'"
                strsql += vbCrLf + " ,@TODATE='" & _Todate & "'"
                strsql += vbCrLf + " ,@CUMMULATIVE ='N'"
                strsql += vbCrLf + " ,@ITEMID ='" & _ItemId & "'"
                strsql += vbCrLf + " ,@SUBITEMID =''"
                strsql += vbCrLf + " ,@TAGNO=''"
                strsql += vbCrLf + " ,@COSTID='" & _costid & "'"
                strsql += vbCrLf + " ,@TYPE='" & _Type & "'"
            End If
            cmd = New OleDbCommand(strsql, cn)
            cmd.CommandTimeout = 1000
            da = New OleDbDataAdapter(cmd)
            Dim ds As New DataSet
            da.Fill(ds)
            dt = ds.Tables(0)
            If dt.Rows.Count > 0 Then
                With gridView
                    .DataSource = Nothing
                    .DataSource = dt
                    FormatGridColumns(gridView, False, True, True, True)
                    autoresize()
                End With
            Else
                MsgBox("No Record found", MsgBoxStyle.Information)
                gridView.DataSource = Nothing
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub autoresize()
        If gridView.RowCount > 0 Then
            If True Then
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridView.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
        End If
    End Sub

    Private Sub btnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.Click
        If gridView.RowCount > 0 Then
            BrightPosting.GExport.Post(Me.Name, "", "Export TagedItemStock", gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
#End Region

End Class