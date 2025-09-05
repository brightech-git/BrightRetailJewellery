Imports System.Data.OleDb
Public Class frmrptTagEdit
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim dtGridView As New DataTable
    Dim cmd As New OleDbCommand
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Dim dt As New DataTable
        strSql = " SELECT 1 FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE' AND CTLTEXT = 'Y'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            cmbCostCentre_MAN.Text = ""
            cmbCostCentre_MAN.Items.Clear()
            cmbCostCentre_MAN.Enabled = False
        End If
        If cmbCostCentre_MAN.Enabled Then
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            objGPack.FillCombo(strSql, cmbCostCentre_MAN)
        End If
        'strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST"
        'strSql += " WHERE ISNULL(STUDDED,'') <> 'S' AND ACTIVE = 'Y' AND STOCKTYPE='T' ORDER BY ITEMNAME"
        'objGPack.FillCombo(strSql, cmbItem_MAN, , False)
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub frmrptTagEdit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmrptTagEdit_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        btnNew_Click(Me, e)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        dtpFrom.Value = GetEntryDate(GetServerDate(tran), tran)
        dtpTo.Value = GetEntryDate(GetServerDate(tran), tran)
        dtGridView = New DataTable
        gridETag.DataSource = Nothing
        gridTag.DataSource = Nothing
        cmbCostCentre_MAN.Select()
        'Panel1.Visible = False
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Function funcColWidth() As Integer
        With gridViewHeader
            If gridViewHeader.DataSource Is Nothing Then Exit Function
            Dim StrHead As String = "OITEMNAME~OSUBITEMNAME~OITEMID~OSUBITEMID~OGRSWT~OLESSWT~ONETWT~OSTNWT~OSTNAMT~OMAXWASTPER~OMAXWAST~OMAXMCGRM~OMAXMC~OSALVALUE"
            Dim StrHead1 As String = "CITEMNAME~CSUBITEMNAME~CITEMID~CSUBITEMID~CGRSWT~STONEWT~CNETWT~CSTNWT~CSTNAMT~CMAXWASTPER~CMAXWAST~CMAXMCGRM~CMAXMC~CSALVALUE"
            .Columns(StrHead).HeaderText = "OLD TAG"
            .Columns(StrHead).Width = gridView.Columns("OITEMNAME").Width
            .Columns(StrHead).Width += gridView.Columns("OSUBITEMNAME").Width
            '.Columns(StrHead).Width += gridView.Columns("OITEMID").Width
            '.Columns(StrHead).Width += gridView.Columns("OSUBITEMID").Width
            .Columns(StrHead).Width += gridView.Columns("OGRSWT").Width
            .Columns(StrHead).Width += gridView.Columns("OLESSWT").Width
            .Columns(StrHead).Width += gridView.Columns("ONETWT").Width
            .Columns(StrHead).Width += gridView.Columns("OSTNWT").Width
            .Columns(StrHead).Width += gridView.Columns("OSTNAMT").Width
            .Columns(StrHead).Width += gridView.Columns("OMAXWASTPER").Width
            .Columns(StrHead).Width += gridView.Columns("OMAXWAST").Width
            .Columns(StrHead).Width += gridView.Columns("OMAXMCGRM").Width
            .Columns(StrHead).Width += gridView.Columns("OMAXMC").Width
            .Columns(StrHead).Width += gridView.Columns("OSALVALUE").Width

            .Columns(StrHead1).HeaderText = "NEW TAG"
            .Columns(StrHead1).Width = gridView.Columns("CITEMNAME").Width
            .Columns(StrHead1).Width += gridView.Columns("CSUBITEMNAME").Width
            '.Columns(StrHead1).Width += gridView.Columns("CITEMID").Width
            '.Columns(StrHead1).Width += gridView.Columns("CSUBITEMID").Width
            .Columns(StrHead1).Width += gridView.Columns("CGRSWT").Width
            .Columns(StrHead1).Width += gridView.Columns("STONEWT").Width
            .Columns(StrHead1).Width += gridView.Columns("CNETWT").Width
            .Columns(StrHead1).Width += gridView.Columns("CSTNWT").Width
            .Columns(StrHead1).Width += gridView.Columns("CSTNAMT").Width
            .Columns(StrHead1).Width += gridView.Columns("CMAXWASTPER").Width
            .Columns(StrHead1).Width += gridView.Columns("CMAXWAST").Width
            .Columns(StrHead1).Width += gridView.Columns("CMAXMCGRM").Width
            .Columns(StrHead1).Width += gridView.Columns("CMAXMC").Width
            .Columns(StrHead1).Width += gridView.Columns("CSALVALUE").Width
        End With
    End Function
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            strSql = "  SELECT "
            strSql += vbCrLf + "  (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=E.ITEMID)OITEMNAME,"
            strSql += vbCrLf + "  (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID=E.SUBITEMID)OSUBITEMNAME,"
            strSql += vbCrLf + "  E.ITEMID AS OITEMID,E.SUBITEMID AS OSUBITEMID,E.GRSWT OGRSWT,E.LESSWT AS OLESSWT,E.NETWT ONETWT,"
            strSql += vbCrLf + "  (SELECT SUM(CASE WHEN STONEUNIT='C' THEN STNWT/5 ELSE STNWT END) FROM " & cnAdminDb & "..EITEMTAGSTONE "
            strSql += vbCrLf + "  WHERE TAGSNO=E.SNO AND RECDATE=E.RECDATE) AS OSTNWT,"
            strSql += vbCrLf + "  (SELECT SUM(STNAMT) FROM " & cnAdminDb & "..EITEMTAGSTONE WHERE TAGSNO=E.SNO AND RECDATE=E.RECDATE) AS OSTNAMT,"
            strSql += vbCrLf + "  E.MAXWASTPER OMAXWASTPER,E.MAXWAST OMAXWAST,E.MAXMCGRM OMAXMCGRM,E.MAXMC OMAXMC,E.SALVALUE OSALVALUE,"
            strSql += vbCrLf + "  (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=I.ITEMID)CITEMNAME,"
            strSql += vbCrLf + "  (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID=I.SUBITEMID)CSUBITEMNAME,"
            strSql += vbCrLf + "  I.ITEMID AS CITEMID,I.SUBITEMID AS CSUBITEMID,I.GRSWT AS CGRSWT,I.LESSWT AS STONEWT,I.NETWT AS CNETWT,"
            strSql += vbCrLf + "  (SELECT SUM(CASE WHEN STONEUNIT='C' THEN STNWT/5 ELSE STNWT END) FROM " & cnAdminDb & "..EITEMTAGSTONE "
            strSql += vbCrLf + "  WHERE TAGSNO=E.SNO AND RECDATE=E.RECDATE) AS CSTNWT,"
            strSql += vbCrLf + "  (SELECT SUM(STNAMT) FROM " & cnAdminDb & "..EITEMTAGSTONE WHERE TAGSNO=E.SNO AND RECDATE=E.RECDATE) AS CSTNAMT,"
            strSql += vbCrLf + "  I.MAXWASTPER AS CMAXWASTPER,I.MAXWAST AS CMAXWAST,I.MAXMCGRM AS CMAXMCGRM,I.MAXMC AS CMAXMC,I.SALVALUE AS CSALVALUE"
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..EITEMTAG AS E"
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMTAG AS I ON E.TAGNO=I.TAGNO"
            strSql += vbCrLf + "  WHERE ISNULL(E.ISSDATE,'')='' "
            strSql += vbCrLf + "  AND E.RECDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
            If txtTagNo.Text <> "" Then
                strSql += " AND ISNULL(E.TAGNO,'') = '" & txtTagNo.Text.ToString & "'"
            End If
            If cmbCostCentre_MAN.Text <> "" Then
                strSql += " AND ISNULL(E.COSTID,'') IN ( SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "')"
            End If
            Dim dts As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dts)
            If dts.Rows.Count > 0 Then
                gridView.DataSource = Nothing
                gridView.DataSource = dts
                If gridView.Rows.Count > 0 Then
                    With gridView
                        .Columns("CITEMID").Visible = False
                        .Columns("CSUBITEMID").Visible = False
                        .Columns("OITEMID").Visible = False
                        .Columns("OSUBITEMID").Visible = False
                        .Columns("CITEMNAME").HeaderText = "ITEMNAME"
                        .Columns("CSUBITEMNAME").HeaderText = "SUBITEMNAME"
                        .Columns("CGRSWT").HeaderText = "GRSWT"
                        .Columns("CNETWT").HeaderText = "NETWT"
                        .Columns("CSTNAMT").HeaderText = "STNAMT"
                        .Columns("CMAXWASTPER").HeaderText = "MAXWASTPER"
                        .Columns("CMAXWAST").HeaderText = "MAXWAST"
                        .Columns("CMAXMCGRM").HeaderText = "MAXMCGRM"
                        .Columns("CMAXMC").HeaderText = "MAXMC"
                        .Columns("CSALVALUE").HeaderText = "SALVALUE"
                        .Columns("OITEMNAME").HeaderText = "ITEMNAME"
                        .Columns("OSUBITEMNAME").HeaderText = "SUBITEMNAME"
                        .Columns("OGRSWT").HeaderText = "GRSWT"
                        .Columns("ONETWT").HeaderText = "NETWT"
                        .Columns("OSTNAMT").HeaderText = "STNAMT"
                        .Columns("OMAXWASTPER").HeaderText = "MAXWASTPER"
                        .Columns("OMAXWAST").HeaderText = "MAXWAST"
                        .Columns("OMAXMCGRM").HeaderText = "MAXMCGRM"
                        .Columns("OMAXMC").HeaderText = "MAXMC"
                        .Columns("OSALVALUE").HeaderText = "SALVALUE"
                    End With
                    Dim dtMergeHeader As DataTable
                    dtMergeHeader = New DataTable
                    With dtMergeHeader
                        .Columns.Add("OITEMNAME~OSUBITEMNAME~OITEMID~OSUBITEMID~OGRSWT~OLESSWT~ONETWT~OSTNWT~OSTNAMT~OMAXWASTPER~OMAXWAST~OMAXMCGRM~OMAXMC~OSALVALUE", GetType(String))
                        .Columns.Add("CITEMNAME~CSUBITEMNAME~CITEMID~CSUBITEMID~CGRSWT~STONEWT~CNETWT~CSTNWT~CSTNAMT~CMAXWASTPER~CMAXWAST~CMAXMCGRM~CMAXMC~CSALVALUE", GetType(String))
                    End With
                    With gridViewHeader
                        .DataSource = Nothing
                        .DataSource = dtMergeHeader
                        .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                        .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                        funcColWidth()
                        gridView.Focus()
                        'Dim colWid As Integer = 0
                        'For cnt As Integer = 0 To gridView.ColumnCount - 1
                        '    If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
                        'Next
                    End With
                End If
            End If


            dtGridView = New DataTable
            Dim condition As String = Nothing
            strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPEDITTAGS') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPEDITTAGS"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            strSql = vbCrLf + " SELECT *,0 AS RESULT2"
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPEDITTAGS"
            strSql += vbCrLf + " FROM ("
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " M.ITEMNAME,SM.SUBITEMNAME,T.TAGNO,T.PCS,T.GRSWT,T.LESSWT,T.NETWT,'' STONEUNIT,T.MAXWASTPER,T.MAXWAST,T.MAXMC,T.SALVALUE,T.SNO,T.UPDATED,T.UPTIME,0 AS RESULT"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..EITEMTAG AS T"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS M ON T.ITEMID=M.ITEMID"
            strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON T.SUBITEMID=SM.SUBITEMID"
            strSql += vbCrLf + " WHERE 1=1 "
            If txtTagNo.Text <> "" Then
                strSql += " AND ISNULL(T.TAGNO,'') = '" & txtTagNo.Text.ToString & "'"
            End If
            strSql += vbCrLf + " AND T.RECDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "' "
            If COSTCENTRE_SINGLE = False Then
                strSql += " AND ISNULL(T.COSTID,'') = '" & cnCostId & "'"
            End If
            If cmbCostCentre_MAN.Text <> "" Then
                strSql += " AND ISNULL(T.COSTID,'') in ( select costid from " & cnAdminDb & "..costcentre where costname = '" & cmbCostCentre_MAN.Text & "')"
            End If
            strSql += vbCrLf + " AND ISNULL(T.ISSDATE,'')='' "
            strSql += vbCrLf + " AND T.COSTID='" & cnCostId & "'"
            strSql += vbCrLf + " AND T.COMPANYID = '" & strCompanyId & "' "
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " M.ITEMNAME,SM.SUBITEMNAME,T.TAGNO,T.STNPCS,T.STNWT GRSWT,0 LESSWT,T.STNWT NETWT,T.STONEUNIT,0 AS MAXWASTPER,0 AS MAXWAST,0 AS MAXMC,T.STNAMT AS SALVALUE,T.TAGSNO,NULL,NULL,1 AS RESULT"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..EITEMTAGSTONE AS T"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS M ON T.STNITEMID=M.ITEMID"
            strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON T.STNSUBITEMID=SM.SUBITEMID"
            strSql += vbCrLf + " AND ISNULL(T.ISSDATE,'')='' "
            strSql += vbCrLf + " WHERE 1=1 "
            If txtTagNo.Text <> "" Then
                strSql += " AND ISNULL(T.TAGNO,'') = '" & txtTagNo.Text.ToString & "'"
            End If
            strSql += " AND T.RECDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "' "
            If COSTCENTRE_SINGLE = False Then
                strSql += " AND ISNULL(T.COSTID,'') = '" & cnCostId & "'"
            End If
            If cmbCostCentre_MAN.Text <> "" Then
                strSql += " AND ISNULL(T.COSTID,'') in ( select costid from " & cnAdminDb & "..costcentre where costname = '" & cmbCostCentre_MAN.Text & "')"
            End If
            strSql += vbCrLf + " )X ORDER BY TAGNO,SNO,RESULT"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            strSql = " SELECT * FROM TEMPTABLEDB..TEMPEDITTAGS"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGridView)
            If Not dtGridView.Rows.Count > 0 Then
                gridETag.DataSource = Nothing
                gridTag.DataSource = Nothing
                MsgBox("Record Not Found", MsgBoxStyle.Information)
                Exit Sub
            Else
                gridETag.DataSource = Nothing
                gridTag.DataSource = Nothing
                gridETag.DataSource = dtGridView
                GridStyle(gridETag)
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            btnSearch.Enabled = True
        End Try
    End Sub

    Private Function GridStyle(ByVal dgv As DataGridView)
        If dgv.Rows.Count > 0 Then
            With dgv
                .Columns("SNO").Visible = False
                .Columns("UPDATED").Visible = False
                .Columns("UPTIME").Visible = False
                .Columns("RESULT").Visible = False
                .Columns("RESULT2").Visible = False
                .Columns("TAGNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("LESSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("MAXWASTPER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("MAXWAST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("MAXMC").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("SALVALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            For Each gvE As DataGridViewRow In dgv.Rows
                With gvE
                    Select Case .Cells("RESULT").Value.ToString
                        Case "0"
                            .DefaultCellStyle.BackColor = Color.LightGray
                            .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                        Case "1"
                            .DefaultCellStyle.BackColor = Color.Ivory
                    End Select
                End With
            Next
            'dgv.Select()
        End If
    End Function

    Private Sub gridETag_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridETag.KeyDown
        If e.KeyCode = Keys.Enter Then
            If gridETag.Rows.Count = 0 Then Exit Sub
            With gridETag.CurrentRow
                If .Cells("RESULT").Value.ToString() = "0" Then
                    Try
                        dtGridView = New DataTable
                        Dim condition As String = Nothing
                        strSql = vbCrLf + " SELECT *,0 AS RESULT2"
                        strSql += vbCrLf + " FROM ("
                        strSql += vbCrLf + " SELECT "
                        strSql += vbCrLf + " M.ITEMNAME,SM.SUBITEMNAME,T.TAGNO,T.PCS,T.GRSWT,T.LESSWT,T.NETWT,'' STONEUNIT,T.MAXWASTPER,T.MAXWAST,T.MAXMC,T.SALVALUE,T.SNO,T.UPDATED,T.UPTIME,0 AS RESULT"
                        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
                        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS M ON T.ITEMID=M.ITEMID"
                        strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON T.SUBITEMID=SM.SUBITEMID"
                        strSql += vbCrLf + " WHERE T.SNO='" & .Cells("SNO").Value.ToString() & "' "
                        If COSTCENTRE_SINGLE = False Then
                            strSql += " AND ISNULL(T.COSTID,'') = '" & cnCostId & "'"
                        End If
                        If cmbCostCentre_MAN.Text <> "" Then
                            strSql += " AND ISNULL(T.COSTID,'') in ( select costid from " & cnAdminDb & "..costcentre where costname = '" & cmbCostCentre_MAN.Text & "')"
                        End If
                        strSql += vbCrLf + " AND ISNULL(T.ISSDATE,'')='' "
                        'strSql += vbCrLf + " AND T.COSTID='" & cnCostId & "'"
                        strSql += vbCrLf + " AND T.COMPANYID = '" & strCompanyId & "' "
                        strSql += vbCrLf + " UNION ALL"
                        strSql += vbCrLf + " SELECT "
                        strSql += vbCrLf + " M.ITEMNAME,SM.SUBITEMNAME,T.TAGNO,T.STNPCS,T.STNWT GRSWT,0 LESSWT,T.STNWT NETWT,T.STONEUNIT,0 AS MAXWASTPER,0 AS MAXWAST,0 AS MAXMC,T.STNAMT AS SALVALUE,T.TAGSNO,NULL,NULL,1 AS RESULT"
                        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE AS T"
                        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS M ON T.STNITEMID=M.ITEMID"
                        strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON T.STNSUBITEMID=SM.SUBITEMID"
                        strSql += vbCrLf + " AND ISNULL(T.ISSDATE,'')='' "
                        strSql += vbCrLf + " WHERE T.TAGSNO='" & .Cells("SNO").Value.ToString() & "' "
                        If COSTCENTRE_SINGLE = False Then
                            strSql += " AND ISNULL(T.COSTID,'') = '" & cnCostId & "'"
                        End If
                        If cmbCostCentre_MAN.Text <> "" Then
                            strSql += " AND ISNULL(T.COSTID,'') in ( select costid from " & cnAdminDb & "..costcentre where costname = '" & cmbCostCentre_MAN.Text & "')"
                        End If
                        strSql += vbCrLf + " )X ORDER BY TAGNO,SNO,RESULT"
                        da = New OleDbDataAdapter(strSql, cn)
                        da.Fill(dtGridView)
                        If Not dtGridView.Rows.Count > 0 Then
                            MsgBox("Record Not Found", MsgBoxStyle.Information)
                            Exit Sub
                        Else
                            gridTag.DataSource = Nothing
                            gridTag.DataSource = dtGridView
                                GridStyle(gridTag)
                        End If
                    Catch ex As Exception

                        MsgBox(ex.Message)
                    End Try
                Else
                    gridTag.DataSource = Nothing
                End If
            End With
        End If
    End Sub

    Private Sub AutoResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoResizeToolStripMenuItem.Click
        If gridETag.RowCount > 0 Then
            If AutoResizeToolStripMenuItem.Checked Then
                gridETag.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridETag.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridETag.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridETag.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridETag.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridETag.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
            'gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End If
    End Sub

    Private Sub AutoResizeToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoResizeToolStripMenuItem1.Click
        If gridTag.RowCount > 0 Then
            If AutoResizeToolStripMenuItem1.Checked Then
                gridTag.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridTag.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridTag.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridTag.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridTag.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridTag.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
            'gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End If
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, Me.Text, gridView, BrightPosting.GExport.GExportType.Export, gridViewHeader)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, Me.Text, gridView, BrightPosting.GExport.GExportType.Print, gridViewHeader)
        End If
    End Sub

End Class