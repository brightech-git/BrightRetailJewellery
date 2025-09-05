Imports System.Data.OleDb
Public Class frmdocumentupdation
    Dim strSql As String
    Dim frmtype As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim dtcost, dtvcost, dtpre, dtchk, dtsentby, dtrecby, dtsentto, dtdocdes, dtvsentby As DataTable
    Dim gridobj As New DataGridView
    Dim serverdate As Date


    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Public Sub New(ByVal Load As String)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        frmtype = Load
        strSql = " SELECT USERNAME,USERID FROM " & cnAdminDb & "..USERMASTER WHERE ISNULL(ACTIVE,'')='Y' AND ISNULL(COSTID,'')='" & cnCostId & "'  ORDER BY USERNAME"
        dtpre = New DataTable
        dtpre = GetSqlTable(strSql, cn)
        gridobj = IIf(frmtype = "S", gridview, gridviewR)
        If Load = "S" Then
            TabControl1.SelectedTab = TabPage1
        Else
            cmbgrid_OWN.Visible = False
            TabControl1.SelectedTab = TabPage3
            Loadcostcentreandemp()
        End If
        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Private Function funcnew()

        serverdate = GetServerDate()
        gridview.DataSource = Nothing : gridviewR.DataSource = Nothing
        dtpdocdate.Value = Date.Now
        dtpdoctodate.Value = Date.Now
        dtpfrmdate.Value = Date.Now
        dtptodate.Value = Date.Now
        dtpRfrmdate.Value = Date.Now
        dtpRTodate.Value = Date.Now
        If frmtype = "S" Then
            objGPack.TextClear(Me)
            strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,0 RESULT UNION ALL "
            strSql += " SELECT COSTNAME,COSTID,1 RESULT FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID NOT IN('" & cnCostId & "')ORDER BY RESULT,COSTNAME"
            dtcost = New DataTable : dtchk = New DataTable : dtsentby = New DataTable : dtrecby = New DataTable : dtsentto = New DataTable : dtdocdes = New DataTable
            dtcost = GetSqlTable(strSql, cn)
            BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtcost, "COSTNAME", , "ALL")
            dtchk = dtpre.Copy() : dtsentby = dtpre.Copy() : dtrecby = dtpre.Copy() : dtsentto = dtpre.Copy()
            If dtpre.Rows.Count > 0 Then
                cmbpreperedby.DataSource = dtpre
                cmbpreperedby.DisplayMember = "USERNAME"
                cmbpreperedby.ValueMember = "USERID"
                cmbpreperedby.SelectedIndex = -1
                cmbpreperedby.Enabled = True

                cmbcheckedby.DataSource = dtchk
                cmbcheckedby.DisplayMember = "USERNAME"
                cmbcheckedby.ValueMember = "USERID"
                cmbcheckedby.SelectedIndex = -1
                cmbcheckedby.Enabled = True

                cmbsentby.DataSource = dtsentby
                cmbsentby.DisplayMember = "USERNAME"
                cmbsentby.ValueMember = "USERID"
                cmbsentby.SelectedIndex = -1
                cmbsentby.Enabled = True

            Else
                cmbpreperedby.DataSource = Nothing
                cmbpreperedby.Enabled = False

                cmbcheckedby.DataSource = Nothing
                cmbcheckedby.Enabled = False

                cmbsentby.DataSource = Nothing
                cmbsentby.Enabled = False
            End If

            strSql = "  SELECT MENUID,MENUTEXT FROM " & cnAdminDb & "..MENUMASTER WHERE MENUID IN("
            strSql += vbCrLf + "  SELECT MENUID FROM " & cnAdminDb & "..ROLETRAN WHERE ROLEID IN("
            strSql += vbCrLf + "  SELECT ROLEID FROM " & cnAdminDb & "..ROLEMASTER WHERE USERID=" & userId & " AND USERID NOT IN(999)) AND _PRINT='Y')"
            dtdocdes = GetSqlTable(strSql, cn)
            If dtdocdes.Rows.Count > 0 Then
                cmbdocdescription.DataSource = dtdocdes
                cmbdocdescription.DisplayMember = "MENUTEXT"
                cmbdocdescription.ValueMember = "MENUID"
                cmbdocdescription.Enabled = True
            Else
                cmbdocdescription.DataSource = Nothing
                cmbdocdescription.Enabled = False
            End If

            dtpdocdate.Focus()
        Else
            cmbRrpttype.Text = "All"
            cmbRrpttype.Focus()
            cmbRrpttype.SelectAll()
        End If

    End Function
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, btnRexit.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcnew()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnopen.Click
        Loadcostcentreandemp()
        TabControl1.SelectedTab = TabPage2
        dtpfrmdate.Focus()
        gridview.DataSource = Nothing
    End Sub

    Private Sub Loadcostcentreandemp()

        strSql = "SELECT 'ALL' COSTNAME,'ALL' COSTID ,0 RESULT UNION ALL "
        strSql += vbCrLf + " SELECT COSTNAME,COSTID,1 RESULT FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID NOT IN('" & cnCostId & "')ORDER BY RESULT,COSTNAME"
        dtvcost = New DataTable
        dtvcost = GetSqlTable(strSql, cn)
        If dtvcost.Rows.Count > 1 Then
            If frmtype = "S" Then
                cmbvtocostid.DataSource = dtvcost
                cmbvtocostid.DisplayMember = "COSTNAME"
                cmbvtocostid.ValueMember = "COSTID"
                cmbvtocostid.SelectedIndex = 0
                cmbvtocostid.Enabled = True
            Else
                cmbRfrmcostid.DataSource = dtvcost
                cmbRfrmcostid.DisplayMember = "COSTNAME"
                cmbRfrmcostid.ValueMember = "COSTID"
                cmbRfrmcostid.SelectedIndex = 0
                cmbRfrmcostid.Enabled = True
            End If
        Else
            cmbvtocostid.DataSource = Nothing
            cmbvtocostid.Enabled = False
            cmbRfrmcostid.DataSource = Nothing
            cmbRfrmcostid.Enabled = False
        End If

        strSql = "SELECT 'ALL' USERNAME,'ALL' USERID ,0 RESULT UNION ALL "
        strSql += " SELECT USERNAME,CONVERT(VARCHAR,USERID)USERID,1 RESULT FROM " & cnAdminDb & "..USERMASTER WHERE ISNULL(ACTIVE,'')='Y' AND ISNULL(COSTID,'')='" & cnCostId & "'   ORDER BY RESULT,USERNAME"
        dtvsentby = New DataTable
        dtvsentby = GetSqlTable(strSql, cn)
        If dtvsentby.Rows.Count > 0 Then
            If frmtype = "S" Then
                cmbvsentby.DataSource = dtvsentby
                cmbvsentby.DisplayMember = "USERNAME"
                cmbvsentby.ValueMember = "USERID"
                cmbvsentby.SelectedIndex = 0
                cmbvsentby.Enabled = True
            Else
                cmbRreceivedby.DataSource = dtvsentby
                cmbRreceivedby.DisplayMember = "USERNAME"
                cmbRreceivedby.ValueMember = "USERID"
                cmbRreceivedby.SelectedIndex = 0
                cmbRreceivedby.Enabled = True
            End If
        Else
            cmbvsentby.DataSource = Nothing
            cmbvsentby.Enabled = False
            cmbRreceivedby.DataSource = Nothing
            cmbRreceivedby.Enabled = False
        End If
        If dtpre.Rows.Count > 0 Then
            If frmtype = "R" Then
                cmbgrid_OWN.DataSource = dtpre
                cmbgrid_OWN.DisplayMember = "USERNAME"
                cmbgrid_OWN.ValueMember = "USERID"
                cmbgrid_OWN.SelectedIndex = 0
                cmbgrid_OWN.Enabled = True
            Else
                cmbgrid_OWN.DataSource = Nothing
                cmbgrid_OWN.Enabled = False
            End If
        End If
    End Sub
    Private Sub Funcview()
        If frmtype = "S" Then
            strSql = "SELECT SNO,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID=T.TOID)TOCOSTCENTRE"
            strSql += vbCrLf + ",CONVERT(VARCHAR,DOC_DATE,105)DOC_DATE,CONVERT(VARCHAR,DOC_TODATE,105)DOC_TODATE"
            strSql += vbCrLf + ",CONVERT(VARCHAR,SENTDATE,105)SENTDATE,DOC_DESC"
            strSql += vbCrLf + ",(SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID=T.PREPAREDBY)PREPAREDBY"
            strSql += vbCrLf + ",(SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID=T.CHECKEDBY)CHECKEDBY"
            strSql += vbCrLf + ",(SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID=T.SENTBY)SENTBY"
            strSql += vbCrLf + ",(SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID=T.SENTTO)SENTTO"
            strSql += vbCrLf + ",(SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID=T.RECEIVEDBY)RECEIVEDBY"
            strSql += vbCrLf + ",CONVERT(VARCHAR,RECEIVEDATE,105)RECEIVEDATE,REMARK,ISNULL(CANCEL,'')CANCEL,TOID COSTID"
            'strSql += vbCrLf + " FROM " & cnAdminDb & "..DOC_STATUS T WHERE  DOC_DATE = '" & dtpfrmdate.Value.ToString("yyyy-MM-dd") & "' AND DOC_TODATE='" & dtptodate.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..DOC_STATUS T WHERE  1=1 "
            strSql += vbCrLf + " AND DOC_DATE between '" & dtpfrmdate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtptodate.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND DOC_TODATE <= '" & dtptodate.Value.ToString("yyyy-MM-dd") & "'"
            'strSql += vbCrLf + " AND '" & dtptodate.Value.ToString("yyyy-MM-dd") & "' BETWEEN DOC_TODATE AND DOC_TODATE"

            strSql += vbCrLf + " AND FROMID='" & cnCostId & "' "
            If cmbvtocostid.Text <> "ALL" And cmbvtocostid.Text <> "" Then strSql += vbCrLf + " AND TOID='" & cmbvtocostid.SelectedValue.ToString & "'"
            If cmbvsentby.Text <> "ALL" And cmbvsentby.Text <> "" Then strSql += vbCrLf + " AND SENTBY='" & cmbvsentby.SelectedValue.ToString & "'"
            If Not chkwithcancel.Checked Then strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
            strSql += vbCrLf + " ORDER BY TOCOSTCENTRE,T.SENTDATE"
        Else
            strSql = "SELECT SNO,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID=T.FROMID)TOCOSTCENTRE,T.FROMID"
            strSql += vbCrLf + ",CONVERT(VARCHAR,DOC_DATE,105)DOC_DATE,CONVERT(VARCHAR,DOC_TODATE,105)DOC_TODATE"
            strSql += vbCrLf + ",CONVERT(VARCHAR,SENTDATE,105)SENTDATE,DOC_DESC,ISNULL(CANCEL,'')CANCEL"
            strSql += vbCrLf + ",(SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID=T.PREPAREDBY)PREPAREDBY"
            strSql += vbCrLf + ",(SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID=T.CHECKEDBY)CHECKEDBY"
            strSql += vbCrLf + ",(SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID=T.SENTBY)SENTBY"
            strSql += vbCrLf + ",(SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID=T.SENTTO)SENTTO"
            strSql += vbCrLf + ",(SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID=T.RECEIVEDBY)RECEIVEDBY"
            strSql += vbCrLf + ",CONVERT(VARCHAR,RECEIVEDATE,105)RECEIVEDATE,FROMID COSTID"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..DOC_STATUS T WHERE  1=1"
            strSql += vbCrLf + " AND TOID='" & cnCostId & "'"
            If cmbRrpttype.Text = "Pending" Then strSql += vbCrLf + " AND ISNULL(RECEIVEDBY,'')=''"
            If cmbRrpttype.Text = "Received" And cmbRreceivedby.Text <> "ALL" And cmbRreceivedby.Text <> "" Then strSql += vbCrLf + " AND ISNULL(RECEIVEDBY,'')='" & cmbRreceivedby.SelectedValue.ToString & "'"
            If cmbRrpttype.Text = "Received" Then strSql += vbCrLf + " AND RECEIVEDATE BETWEEN '" & dtpRfrmdate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpRTodate.Value.ToString("yyyy-MM-dd") & "'"
            If cmbRrpttype.Text = "All" And cmbRreceivedby.Text <> "ALL" And cmbRreceivedby.Text <> "" Then strSql += vbCrLf + " AND ISNULL(RECEIVEDBY,'')='" & cmbRreceivedby.SelectedValue.ToString & "'"
            If cmbRrpttype.Text = "All" Then strSql += vbCrLf + " AND DOC_DATE BETWEEN '" & dtpRfrmdate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpRTodate.Value.ToString("yyyy-MM-dd") & "'"
            If cmbRrpttype.Text = "All" Then strSql += vbCrLf + " AND DOC_TODATE <= '" & dtpRTodate.Value.ToString("yyyy-MM-dd") & "'"

            If cmbRfrmcostid.Text <> "ALL" And cmbRfrmcostid.Text <> "" Then strSql += vbCrLf + " AND FROMID='" & cmbRfrmcostid.SelectedValue.ToString & "'"
            If Not chkwithcancel.Checked Then strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
            strSql += vbCrLf + " ORDER BY T.FROMID,T.DOC_DATE"
        End If

        Dim dtgrid As New DataTable
        dtgrid = GetSqlTable(strSql, cn)
        If dtgrid.Rows.Count > 0 Then
            With gridobj
                .DataSource = dtgrid
                .Columns("SNO").Visible = False
                .Columns("TOCOSTCENTRE").Width = 120

                .Columns("SENTDATE").Width = 90
                .Columns("DOC_DATE").Width = 90
                .Columns("RECEIVEDATE").Width = 120
                .Columns("DOC_DESC").Width = 200

                .Columns("PREPAREDBY").Width = 150
                .Columns("SENTBY").Width = 150
                .Columns("SENTTO").Width = 150
                .Columns("RECEIVEDBY").Width = 150

                .Columns("TOCOSTCENTRE").HeaderText = "To Costcentre"
                .Columns("DOC_DATE").HeaderText = "FrmDate"
                .Columns("DOC_TODATE").HeaderText = "ToDate"
                .Columns("SENTDATE").HeaderText = "Sent Date"
                .Columns("RECEIVEDATE").HeaderText = "Received Date"
                .Columns("DOC_DESC").HeaderText = "Document Description"

                .Columns("PREPAREDBY").HeaderText = "Prepared BY"
                .Columns("CHECKEDBY").HeaderText = "Checked BY"
                .Columns("SENTBY").HeaderText = "Sent By"
                .Columns("SENTTO").HeaderText = "Sent To"
                .Columns("RECEIVEDBY").HeaderText = "Received By"
                .Columns("COSTID").Visible = False
                If frmtype = "R" Then
                    .Columns("TOCOSTCENTRE").HeaderText = "Frm Costcentre"
                    .Columns("FROMID").Visible = False
                End If
                For i As Integer = 0 To gridobj.Rows.Count - 1
                    If gridobj.Rows(i).Cells("CANCEL").Value.ToString = "Y" Then
                        gridobj.Rows(i).DefaultCellStyle.BackColor = Color.LightPink
                    End If
                Next
                .Focus()
            End With
        Else
            gridobj.DataSource = Nothing
            If frmtype = "R" Then cmbRrpttype.Focus()
            If frmtype = "S" Then dtpfrmdate.Focus()
            MsgBox("No Record found.")
        End If
    End Sub

    Private Sub Add()
        Try
            Dim costid As String = "ALL"
            If chkCmbCostCentre.Text <> "ALL" Then
                costid = GetSelectedCostId(chkCmbCostCentre, True)
            End If
            strSql = "SELECT  DISTINCT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE 1=1"
            If costid <> "ALL" Then strSql += " AND COSTID IN(" & costid & ")"
            Dim dtcost As New DataTable
            dtcost = GetSqlTable(strSql, cn)
            If dtcost.Rows.Count > 0 Then
                For i As Integer = 0 To dtcost.Rows.Count - 1
                    Dim sno As String = GetNewSno(TranSnoType.DOC_STATUS, tran, "GET_ADMINSNO_TRAN")
                    strSql = " INSERT INTO " & cnAdminDb & "..DOC_STATUS(SNO,FROMID,TOID,DOC_DATE,DOC_TODATE,DOC_TYPE,DOC_DESC,PREPAREDBY,CHECKEDBY,SENTBY,SENTDATE,SENTTO,REMARK)"
                    strSql += " VALUES"
                    strSql += " ("
                    strSql += " '" & sno & "'" 'SNO
                    strSql += " ,'" & cnCostId & "'" 'FROMID
                    strSql += " ,'" & dtcost.Rows(i).Item("COSTID").ToString & "'" 'TOID
                    strSql += " ,'" & dtpdocdate.Value.ToString("yyyy-MM-dd") & "'" 'DOC_DATE
                    strSql += " ,'" & dtpdoctodate.Value.ToString("yyyy-MM-dd") & "'" 'DOC_TODATE
                    strSql += " ,'G'" 'DOC_TYPE
                    strSql += " ,'" & txtdocdes.Text.ToString & "'" 'DOC_DESC
                    strSql += " ," & cmbpreperedby.SelectedValue & "" 'PREPARED BY
                    strSql += " ," & cmbcheckedby.SelectedValue & "" 'CHECKED BY
                    strSql += " ," & cmbsentby.SelectedValue & "" 'SENT BY
                    strSql += " ,'" & serverdate.ToString("yyyy-MM-dd") & "'" 'SENT DATE
                    strSql += " ," & IIf(cmbsentto.Enabled = True, cmbsentto.SelectedValue, 0) & "" 'SENT TO
                    strSql += " ,'" & txtremark.Text.ToString & "'" 'REMARK
                    strSql += " )"
                    ExecQuery(SyncMode.Transaction, strSql, cn, , dtcost.Rows(i).Item("COSTID").ToString)
                Next
            End If
            MsgBox("Inserted..")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        ' If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If txtdocdes.Text = "" Then
            MsgBox("Document Description should not empty", MsgBoxStyle.Information)
            txtdocdes.Focus()
            Exit Sub
        ElseIf cmbpreperedby.Text = "" Then
            MsgBox("Prepared By should not empty", MsgBoxStyle.Information)
            cmbpreperedby.Focus()
            Exit Sub
        ElseIf cmbcheckedby.Text = "" Then
            MsgBox("Checked By should not empty", MsgBoxStyle.Information)
            cmbcheckedby.Focus()
            Exit Sub
        ElseIf cmbsentby.Text = "" Then
            MsgBox("Sent By should not empty", MsgBoxStyle.Information)
            cmbsentby.Focus()
            Exit Sub
        ElseIf cmbsentto.Text = "" And cmbsentto.Enabled Then
            MsgBox("Sent To should not empty", MsgBoxStyle.Information)
            cmbsentto.Focus()
            Exit Sub
        End If
        If dtpdocdate.Value > serverdate Then
            MsgBox("Document date should by lessthan Server Date", MsgBoxStyle.Information)
            dtpdocdate.Focus()
            Exit Sub
        End If
        If dtpdoctodate.Value > serverdate Then
            MsgBox("Document date should by lessthan Server Date", MsgBoxStyle.Information)
            dtpdoctodate.Focus()
            Exit Sub
        End If

        Add()
        funcnew()
    End Sub

    Private Sub frmSyncCostcentre_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) And cmbgrid_OWN.Focused = False And gridviewR.Focused = False Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            btnback_Click(Me, New EventArgs())
        End If
    End Sub

    Private Sub frmSyncCostcentre_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        TabControl1.Appearance = TabAppearance.FlatButtons
        TabControl1.ItemSize = New Size(0, 1)
        TabControl1.SizeMode = TabSizeMode.Fixed
        funcnew()
        If (cmbtocostid.Enabled = False And cmbtocostid.Items.Count <= 0 And frmtype = "S") Or (cmbRfrmcostid.Enabled = False And cmbRfrmcostid.Items.Count <= 1 And frmtype = "R") Then
            MsgBox("Access Denied")
            Me.BeginInvoke(New MethodInvoker(AddressOf closeit))
        End If
    End Sub
    Private Sub closeit()
        Me.Close()
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        btnSave_Click(Me, New EventArgs)
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        btnOpen_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub cmbdoctype_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbdoctype.SelectedIndexChanged
        If cmbdoctype.Text = "GENERAL" Then
            txtdocdes.Text = ""
            txtdocdes.Visible = True
            cmbdocdescription.Visible = False
        Else
            txtdocdes.Text = ""
            txtdocdes.Visible = False
            cmbdocdescription.Visible = True
            cmbdocdescription.SelectedIndex = -1
        End If
    End Sub

    Private Sub btnback_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnback.Click
        If TabControl1.SelectedTab Is TabPage2 Then
            TabControl1.SelectedTab = TabPage1
            cmbtocostid.Focus()
        End If
    End Sub

    Private Sub btnview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnview.Click, btnRview.Click
        Funcview()
    End Sub

    Private Sub gridview_CellEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridviewR.CellEnter
        If Not TabControl1.SelectedTab Is TabPage3 Then Exit Sub
        If gridviewR.Columns(gridviewR.CurrentCell.ColumnIndex).Name = "RECEIVEDBY" Then
            If gridviewR.CurrentCell.Value Is Nothing Or IsDBNull(gridviewR.CurrentCell.Value) Then : cmbgrid_OWN.Text = String.Empty
            Else : cmbgrid_OWN.Text = gridviewR.CurrentCell.Value : End If
            Dim CurrentCellx As Integer = gridviewR.GetCellDisplayRectangle(gridviewR.CurrentCell.ColumnIndex, gridviewR.CurrentRow.Index, False).Left + gridviewR.Left
            Dim CurrentCelly As Integer = gridviewR.GetCellDisplayRectangle(gridviewR.CurrentCell.ColumnIndex, gridviewR.CurrentRow.Index, False).Top + gridviewR.Top
            cmbgrid_OWN.Location = New Point(CurrentCellx, CurrentCelly)
            cmbgrid_OWN.Size = New Size(gridviewR.CurrentCell.Size.Width - 1, gridviewR.CurrentCell.Size.Height - 2)
            cmbgrid_OWN.Show()
            cmbgrid_OWN.BringToFront()
            cmbgrid_OWN.Select()
            cmbgrid_OWN.Focus()
        Else
            cmbgrid_OWN.Visible = False
        End If
    End Sub

    Private Sub cmbgrid_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbgrid_OWN.KeyDown
        If Not TabControl1.SelectedTab Is TabPage3 Then Exit Sub
        If e.KeyCode = Keys.Return Then
            e.Handled = True
            If cmbgrid_OWN.Focused = False Then Exit Sub
            If gridviewR.Columns(gridviewR.CurrentCell.ColumnIndex).Name = "RECEIVEDBY" Then
                If cmbgrid_OWN.Text <> String.Empty Then
                    If gridviewR.CurrentRow.Cells("RECEIVEDBY").Value.ToString <> "" Then GoTo skip
                    strSql = "UPDATE T SET T.RECEIVEDBY='" & cmbgrid_OWN.SelectedValue.ToString & "',T.RECEIVEDATE='" & Date.Now.ToString("yyyy-MM-dd") & "'"
                    strSql += vbCrLf + " FROM " & cnAdminDb & "..DOC_STATUS T WHERE SNO='" & gridviewR.CurrentRow.Cells("SNO").Value.ToString & "'"
                    ExecQuery(SyncMode.Transaction, strSql, cn, , gridviewR.CurrentRow.Cells("FROMID").Value.ToString)
                    strSql = "SELECT isnull((SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID=T.RECEIVEDBY),'')RECEIVEDBY,convert(varchar,isnull(RECEIVEDATE,''),105)RECEIVEDATE  FROM " & cnAdminDb & "..DOC_STATUS T WHERE SNO='" & gridviewR.CurrentRow.Cells("SNO").Value.ToString & "'"
                    Dim dt As New DataTable : dt = GetSqlTable(strSql, cn)
                    If dt.Rows.Count > 0 Then
                        gridviewR.CurrentRow.Cells("RECEIVEDBY").Value = dt.Rows(0).Item("RECEIVEDBY").ToString
                        gridviewR.CurrentRow.Cells("RECEIVEDATE").Value = dt.Rows(0).Item("RECEIVEDATE").ToString
                    End If
skip:
                    If gridviewR.CurrentRow.Index < gridviewR.RowCount - 1 Then
                        gridviewR.CurrentCell = gridviewR.Item("RECEIVEDBY", gridviewR.CurrentRow.Index + 1)
                        gridviewR.Item("RECEIVEDBY", gridviewR.CurrentRow.Index).Selected = True
                    Else
                        cmbgrid_OWN.Visible = False
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub gridview_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridviewR.Scroll
        cmbgrid_OWN.Visible = False
    End Sub

    Private Sub cmbRrpttype_SelectedValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbRrpttype.SelectedValueChanged
        If cmbRrpttype.Text = "Pending" Then
            pnldatefilt.Enabled = False
            cmbRreceivedby.Text = "ALL"
            cmbRreceivedby.Enabled = False
        Else
            pnldatefilt.Enabled = True
            cmbRreceivedby.Text = "ALL"
            cmbRreceivedby.Enabled = True
        End If
    End Sub

    Private Sub btnRexport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRexport.Click, btnexport.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridobj.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, "", "Document Maintanence", gridobj, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub chkCmbCostCentre_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCmbCostCentre.Leave
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text.Contains(",") = False Then
            strSql = "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & chkCmbCostCentre.Text.ToString & "'"
            Dim Costid As String = objGPack.GetSqlValue(strSql, "COSTID", "")
            strSql = " SELECT USERNAME,USERID FROM " & cnAdminDb & "..USERMASTER WHERE ISNULL(ACTIVE,'')='Y' AND "
            strSql += vbCrLf + " ISNULL(COSTID,'')IN ('" & Costid & "')  "
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT USERNAME,USERID FROM " & cnAdminDb & "..USERMASTER WHERE ISNULL(ACTIVE,'')='Y' AND "
            strSql += vbCrLf + " ISNULL(USERCOSTID,'')LIKE('%" & Costid & "%')   "
            dtsentto = New DataTable
            dtsentto = GetSqlTable(strSql, cn)
            If dtsentto.Rows.Count > 0 Then
                cmbsentto.DataSource = dtsentto
                cmbsentto.DisplayMember = "USERNAME"
                cmbsentto.ValueMember = "USERID"
                cmbsentto.SelectedIndex = -1
                cmbsentto.Enabled = True
            Else
                cmbsentto.DataSource = Nothing
                cmbsentto.Enabled = False
            End If
        Else
            cmbsentto.DataSource = Nothing
            cmbsentto.Enabled = False
        End If
    End Sub

    Private Sub dtpdoctodate_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtpdoctodate.Enter
        dtpdoctodate.Value = dtpdocdate.Value
    End Sub

    Private Sub cmbpreperedby_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbpreperedby.Enter
        'cmbpreperedby.DroppedDown = True
        cmbpreperedby.DropDownHeight = 100

    End Sub

    Private Sub cmbcheckedby_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbcheckedby.Enter
        'cmbcheckedby.DroppedDown = True
        cmbcheckedby.DropDownHeight = 100
    End Sub


    Private Sub cmbsentby_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbsentby.Enter
        'cmbsentby.DroppedDown = True
        cmbsentby.DropDownHeight = 100
    End Sub

    Private Sub cmbsentto_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbsentto.Enter
        'cmbsentto.DroppedDown = True
        cmbsentto.DropDownHeight = 100
    End Sub

    Private Sub cmbpreperedby_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbpreperedby.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    
    Private Sub cmbcheckedby_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbcheckedby.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cmbsentby_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbsentby.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cmbsentto_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbsentto.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub gridview_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridview.KeyDown
        If gridview.RowCount <= 0 Then Exit Sub
        If e.KeyCode = Keys.C Then
            If gridview.CurrentRow.Cells("CANCEL").Value <> "Y" Then
                MsgBox("Are you sure to Cancel.")
                Dim objSecret As New frmAdminPassword()
                If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then : Exit Sub : End If
                strSql = " UPDATE " & cnAdminDb & "..DOC_STATUS SET CANCEL='Y' WHERE SNO='" & gridview.CurrentRow.Cells("SNO").Value.ToString & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, , gridview.CurrentRow.Cells("COSTID").Value.ToString)
                Funcview()
                MsgBox("Successfully Cancelled.")
            Else
                MsgBox("Already Cancelled.")
            End If
        End If
    End Sub
End Class

