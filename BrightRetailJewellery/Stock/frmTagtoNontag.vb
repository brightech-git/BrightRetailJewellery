Imports System.Data.OleDb
Public Class frmTagtoNontag
    Dim StrSql As String
    Dim Cmd As OleDbCommand
    Dim Da As OleDbDataAdapter
#Region "USER DEFINED FUNCTION"
    Function funnew()
        fillcombo(3)
        StrSql = " SELECT 'ALL' ITEMNAME,'ALL' ITEMID,1 RESULT"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + "SELECT DISTINCT ITEMNAME,CONVERT(VARCHAR,ITEMID)ITEMID,2 RESULT FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE IN('T') "
        StrSql += vbCrLf + " AND ITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMTAG WHERE ISSDATE IS NULL)  ORDER BY RESULT,ITEMNAME"
        Dim dttagitem As New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dttagitem)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbitemname, dttagitem, "ITEMNAME", , "ALL")
        gridView.DataSource = Nothing
        txtTagNo.Text = ""
        chkSelectAll.Checked = False
        chkcmbitemname.Focus()
        'cmbnonsubitem_OWN.DataSource = Nothing
        'cmbNontag_OWN.DataSource = Nothing
        'cmbcountername_OWN.DataSource = Nothing
    End Function
    Function Search()
        gridView.DataSource = Nothing
        Me.Refresh()
        Try
            Dim tagnoSearch As String = ""
            If txtTagNo.Text.Trim <> "" Then
                If txtTagNo.Text.Contains(",") Then
                    tagnoSearch = GetQryString(txtTagNo.Text)
                Else
                    tagnoSearch = GetQryString(txtTagNo.Text)
                End If
            End If
            Dim Selecteditems As String = GetSelecteditemid(chkcmbitemname, True)
            StrSql = vbCrLf + " SELECT I.ITEMNAME ITEM,T.RECDATE,T.TAGNO,T.PCS,T.GRSWT,T.NETWT,T.LESSWT"
            StrSql += vbCrLf + ",T.SNO,T.COSTID"
            StrSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T "
            StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS I ON I.ITEMID = T.ITEMID"
            StrSql += vbCrLf + " AND T.ISSDATE IS NULL AND ISNULL(APPROVAL,'') = ''"
            If Not cnCentStock Then StrSql += " AND T.COMPANYID = '" & GetStockCompId() & "'"
            If chkcmbitemname.Text <> "ALL" And chkcmbitemname.Text <> "" Then StrSql += vbCrLf + " AND T.ITEMID IN (" & Selecteditems & ")"
            If txtTagNo.Text.Trim <> "" Then StrSql += vbCrLf + " AND TAGNO IN(" & tagnoSearch & ")"
            StrSql += vbCrLf + " ORDER BY I.ITEMNAME"
            Dim dtGrid As New DataTable
            Da = New OleDbDataAdapter(StrSql, cn)
            Da.Fill(dtGrid)
            dtGrid.Columns.Add("CHECK", GetType(Boolean))
            dtGrid.Columns("CHECK").DefaultValue = False
            If Not dtGrid.Rows.Count > 0 Then
                MsgBox("Record not found", MsgBoxStyle.Information)
                chkcmbitemname.Select()
                Exit Function
            End If
            gridView.DataSource = dtGrid
            With gridView

                For Each DgvCol As DataGridViewColumn In gridView.Columns
                    DgvCol.ReadOnly = True
                Next
                BrighttechPack.GlobalMethods.FormatGridColumns(gridView)
                gridView.Columns("CHECK").Width = 40
                gridView.Columns("ITEM").Width = 175
                gridView.Columns("TAGNO").Width = 80
                gridView.Columns("PCS").Width = 40
                gridView.Columns("GRSWT").Width = 80
                gridView.Columns("NETWT").Width = 80
                gridView.Columns("LESSWT").Width = 80
                gridView.Columns("RECDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
                gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
                gridView.Columns("CHECK").ReadOnly = False
                gridView.Columns("CHECK").HeaderText = ""
                gridView.Columns("SNO").Visible = False
                gridView.Columns("COSTID").Visible = False
                gridView.Select()
                gridView.CurrentCell = gridView.Rows(0).Cells("CHECK")
            End With
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Function
    Function Transfer()
        Dim tran As OleDbTransaction
        tran = cn.BeginTransaction()
        Dim pcs As Int32 = 0
        Dim grswt As Decimal = 0
        Dim netwt As Decimal = 0
        Dim lesswt As Decimal = 0
        Try
            For i As Integer = 0 To gridView.Rows.Count - 1
                Dim flag As Boolean
                flag = IIf(IsDBNull(gridView.Rows(i).Cells("CHECK").Value), False, True)
                If flag = True Then
                    pcs += Val(gridView.Rows(i).Cells("PCS").Value.ToString())
                    grswt += Val(gridView.Rows(i).Cells("GRSWT").Value.ToString())
                    lesswt += Val(gridView.Rows(i).Cells("LESSWT").Value.ToString())
                    netwt += Val(gridView.Rows(i).Cells("NETWT").Value.ToString())
                    StrSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET ISSDATE='" & Today.Date.ToString("yyyy-MM-dd") & "' "
                    StrSql += vbCrLf + " ,TOFLAG = 'NT' WHERE 1=1 "
                    StrSql += vbCrLf + " AND SNO ='" & gridView.Rows(i).Cells("SNO").Value.ToString() & "'"
                    ExecQuery(SyncMode.Stock, StrSql, cn, tran, cnCostId, , , "TITEMTAG", , False)
                End If
            Next
            Dim tagSno As String = GetNewSno(TranSnoType.ITEMNONTAGCODE, tran, "GET_ADMINSNO_TRAN")
            StrSql = " INSERT INTO " & cnAdminDb & "..ITEMNONTAG"
            StrSql += " ("
            StrSql += " SNO,ITEMID,SUBITEMID,COMPANYID,RECDATE,ITEMCTRID,DESIGNERID,"
            StrSql += " PCS,GRSWT,LESSWT,NETWT,"
            StrSql += " FINRATE,ISSTYPE,RECISS,POSTED,"
            StrSql += " LOTNO,PACKETNO,DREFNO,"
            StrSql += " ORDREPNO,ORSNO,NARRATION,PURWASTAGE,"
            StrSql += " PURRATE,PURMC,RATE,COSTID,TCOSTID,"
            StrSql += " CTGRM,ITEMTYPEID,"
            StrSql += " CARRYFLAG,REASON,BATCHNO,CANCEL,"
            StrSql += " USERID,UPDATED,UPTIME,SYSTEMID,APPVER"
            StrSql += " )VALUES("
            StrSql += " '" & tagSno & "'" 'SNO
            StrSql += " ," & Val("" & cmbNontag_OWN.SelectedValue) & "" 'ITEMID
            StrSql += " ," & Val("" & cmbnonsubitem_OWN.SelectedValue.ToString()) & "" 'SUBITEMID
            StrSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
            StrSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'RECDATE
            StrSql += " ,'" & "" & cmbcountername_OWN.SelectedValue.ToString() & "'" 'ITEMCTRID
            StrSql += " ,''" 'DESIGNERID
            StrSql += " ," & pcs & "" 'PCS
            StrSql += " ," & grswt & "" 'GRSWT
            StrSql += " ," & lesswt & "" 'LESSWT
            StrSql += " ," & netwt & "" 'NETWT
            StrSql += " ,0" 'FINRATE
            StrSql += " ,''" 'ISSTYPE
            StrSql += " ,'R'" 'RECISS
            StrSql += " ,''" 'POSTED
            StrSql += " ,0" 'LOTNO
            StrSql += " ,''" 'PACKETNO
            StrSql += " ,0" 'DREFNO

            StrSql += " ,''" 'ORDREPNO
            StrSql += " ,''" 'ORSNO
            StrSql += " ,''" 'NARRATION
            StrSql += " ,0" 'PURWASTAGE
            StrSql += " ,0" 'PURRATE
            StrSql += " ,0" 'PURMC
            StrSql += " ,0" 'RATE
            StrSql += " ,'" & cnCostId & "'" 'COSTID
            StrSql += " ,'" & cnCostId & "'" 'TCOSTID
            StrSql += " ,''"
            StrSql += " ,0" 'ITEMTYPEID
            StrSql += " ,''" 'CARRYFLAG
            StrSql += " ,'0'" 'REASON
            StrSql += " ,''" 'BATCHNO
            StrSql += " ,''" 'CANCEL
            StrSql += " ," & userId & "" 'USERID
            StrSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
            StrSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
            StrSql += " ,'" & systemId & "'" 'SYSTEMID
            StrSql += " ,'" & VERSION & "'" 'APPVER
            StrSql += " )"
            ExecQuery(SyncMode.Stock, StrSql, cn, tran, cnCostId, , , "TITEMNONTAG", , False)
            tran.Commit()
            MsgBox("Tag transfer successfully.", MsgBoxStyle.Information)
            funnew()
        Catch ex As Exception
            tran.Rollback()
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function fillcombo(ByVal type As Integer)
        If type = 1 Then
            cmbnonsubitem_OWN.DataSource = Nothing
            Dim dtnonsub As New DataTable
            StrSql = " SELECT '' SUBITEMNAME,'' SUBITEMID,1 RESULT"
            StrSql += " UNION ALL"
            StrSql += " SELECT DISTINCT SUBITEMNAME,CONVERT(vARCHAR,SUBITEMID)SUBITEMID,2 RESULT FROM " & cnAdminDb & "..SUBITEMMAST WHERE 1=1"
            If cmbNontag_OWN.Text <> "" Then StrSql += vbCrLf + " AND ITEMID IN('" & cmbNontag_OWN.SelectedValue.ToString() & "')"
            StrSql += " ORDER BY RESULT,SUBITEMNAME"
            dtnonsub = New DataTable
            Da = New OleDbDataAdapter(StrSql, cn)
            Da.Fill(dtnonsub)
            If dtnonsub.Rows.Count > 0 Then
                cmbnonsubitem_OWN.DataSource = dtnonsub
                cmbnonsubitem_OWN.ValueMember = "SUBITEMID"
                cmbnonsubitem_OWN.DisplayMember = "SUBITEMNAME"
                cmbnonsubitem_OWN.SelectedIndex = 0
            End If
        ElseIf type = 2 Then
            cmbcountername_OWN.DataSource = Nothing
            Dim dtcoun As New DataTable
            StrSql = " SELECT '' ITEMCTRNAME,'' ITEMCTRID,1 RESULT"
            StrSql += " UNION ALL"
            StrSql += " SELECT DISTINCT ITEMCTRNAME,CONVERT(vARCHAR,ITEMCTRID)ITEMCTRID,2 RESULT FROM " & cnAdminDb & "..ItemCounter"
            StrSql += " ORDER BY RESULT,ITEMCTRNAME"
            Da = New OleDbDataAdapter(StrSql, cn)
            Da.Fill(dtcoun)
            If dtcoun.Rows.Count > 0 Then
                cmbcountername_OWN.DataSource = dtcoun
                cmbcountername_OWN.ValueMember = "ITEMCTRID"
                cmbcountername_OWN.DisplayMember = "ITEMCTRNAME"
                cmbcountername_OWN.SelectedIndex = 0
            End If
        ElseIf type = 3 Then
            cmbNontag_OWN.DataSource = Nothing
            'StrSql = " SELECT 'ALL' ITEMNAME,'ALL' ITEMID,1 RESULT"
            'StrSql += vbCrLf + " UNION ALL"
            StrSql = vbCrLf + "SELECT DISTINCT ITEMNAME,CONVERT(VARCHAR,ITEMID)ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE='N' "
            StrSql += " ORDER BY ITEMNAME"
            Dim dtitemname As New DataTable
            Da = New OleDbDataAdapter(StrSql, cn)
            Da.Fill(dtitemname)
            If dtitemname.Rows.Count > 0 Then
                cmbNontag_OWN.DataSource = dtitemname
                cmbNontag_OWN.ValueMember = "ITEMID"
                cmbNontag_OWN.DisplayMember = "ITEMNAME"
                cmbNontag_OWN.SelectedIndex = 0
            End If

        End If
    End Function
#End Region
#Region "KEY EVENTS"
    Private Sub frmTagtoNontag_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyCode = Keys.Escape And gridView.Focused = True Then
            cmbNontag_OWN.Focus()
            cmbNontag_OWN.SelectAll()
        End If
    End Sub
    Private Sub frmTagtoNontag_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funnew()
    End Sub

    Private Sub chkSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSelectAll.CheckedChanged
        If gridView.Rows.Count > 0 Then
            For i As Integer = 0 To gridView.Rows.Count - 1
                gridView.Rows(i).Cells("CHECK").Value = chkSelectAll.Checked
            Next
        End If
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Search()
    End Sub
    Private Sub transferToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles transferToolStripMenuItem1.Click
        btnTransfer_Click(Me, New EventArgs)
    End Sub

    Private Sub exitToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles exitToolStripMenuItem2.Click
        Me.Close()
    End Sub

    Private Sub newToolStripMenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles newToolStripMenuItem3.Click
        funnew()
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Escape Then
            cmbNontag_OWN.Focus()
            cmbNontag_OWN.SelectAll()
            'cmbNontag_OWN.SelectedIndex = 0
        ElseIf e.KeyCode = Keys.Enter Then
            gridView.CurrentCell = gridView.CurrentRow.Cells("LESSWT")
        End If
    End Sub


    Private Sub btnTransfer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTransfer.Click
        If MsgBox("Do you want to Transfer?", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
            Transfer()
        Else
            Exit Sub
        End If
    End Sub
    Private Sub cmbNontag_OWN_SelectionChangeCommitted(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbNontag_OWN.SelectionChangeCommitted
        'fillcombo(1)
        'fillcombo(2)
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funnew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Private Sub cmbNontag_OWN_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbNontag_OWN.Leave
        fillcombo(1)
        fillcombo(2)
    End Sub
#End Region
#Region "CONSTRUCTOR"
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub
#End Region
End Class