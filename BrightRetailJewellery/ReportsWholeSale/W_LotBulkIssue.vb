Imports System.Data.OleDb
Public Class W_LotBulkIssue
    Dim Da As OleDbDataAdapter
    Dim Cmd As OleDbCommand
    Dim StrSql As String

    Private Sub LotBulkIssue_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub LotBulkIssue_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        btnNew_Click(Me, New EventArgs)
        gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpAsOnDate.Value = BrighttechPack.GlobalMethods.GetServerDate(cn)
        gridView.DataSource = Nothing
        chkSelectAll.Checked = False
        btnLotIssue.Enabled = False
        dtpAsOnDate.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        gridView.DataSource = Nothing
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("CHECK", GetType(Boolean))
        dtGrid.Columns("CHECK").DefaultValue = chkSelectAll.Checked
        StrSql = vbCrLf + "  SELECT I.TRANNO,I.TRANDATE"
        StrSql += vbCrLf + "  ,(SELECT ACNAME FROM " & CNADMINDB & "..ACHEAD WHERE ACCODE = I.ACCODE)ACNAME"
        StrSql += vbCrLf + "  ,(SELECT ITEMNAME FROM " & CNADMINDB & "..ITEMMAST WHERE ITEMID = I.ITEMID)AS ITEM"
        StrSql += vbCrLf + "  ,(SELECT SUBITEMNAME FROM " & CNADMINDB & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID)AS SUBITEM"
        StrSql += vbCrLf + "  ,SUM(I.PCS - ISNULL(L.PCS,0))PCS"
        StrSql += vbCrLf + "  ,SUM(I.GRSWT - ISNULL(L.GRSWT,0))GRSWT"
        StrSql += vbCrLf + "  ,SUM(I.NETWT - ISNULL(L.NETWT,0))NETWT"
        StrSql += vbCrLf + "  ,I.TOUCH"
        StrSql += vbCrLf + "  ,I.PUREWT PUREWT"
        StrSql += vbCrLf + "  ,I.SNO,I.ITEMTYPEID"
        StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT I"
        StrSql += vbCrLf + "  LEFT OUTER JOIN " & cnStockDb & "..W_LOTISSUE AS L ON L.TRANNO = I.TRANNO AND L.TRANDATE = I.TRANDATE AND L.RECSNO = I.SNO"
        StrSql += vbCrLf + "  WHERE I.TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + "  AND ISNULL(I.CANCEL,'') = '' AND ISNULL(L.CANCEL,'') = ''"
        StrSql += vbCrLf + "  AND I.GRSWT <> 0 AND LEN(I.TRANTYPE)>2 AND ISNULL(I.ITEMID,0) <> 0"
        StrSql += vbCrLf + "  GROUP BY I.TRANNO,I.TRANDATE,I.ACCODE,I.ITEMID,I.SUBITEMID,I.TOUCH,I.PUREWT,I.SNO,I.ITEMTYPEID"
        StrSql += vbCrLf + "  HAVING "
        StrSql += vbCrLf + "  (SUM(I.PCS - ISNULL(L.PCS,0)) > 0"
        StrSql += vbCrLf + "  OR SUM(I.GRSWT - ISNULL(L.GRSWT,0)) > 0)"
        StrSql += vbCrLf + "  ORDER BY I.TRANDATE,I.TRANNO"
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        gridView.DataSource = dtGrid
        BrighttechPack.GlobalMethods.FormatGridColumns(gridView)

        gridView.Columns("CHECK").Width = 35
        gridView.Columns("TRANNO").Width = 60
        gridView.Columns("TRANDATE").Width = 80
        gridView.Columns("ACNAME").Width = 150
        gridView.Columns("ITEM").Width = 120
        gridView.Columns("SUBITEM").Width = 120
        gridView.Columns("PCS").Width = 70
        gridView.Columns("GRSWT").Width = 80
        gridView.Columns("NETWT").Width = 80
        gridView.Columns("TOUCH").Width = 80
        gridView.Columns("PUREWT").Width = 80

        gridView.Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
        gridView.Columns("GRSWT").DefaultCellStyle.Format = "0.000"
        gridView.Columns("NETWT").DefaultCellStyle.Format = "0.000"
        gridView.Columns("PUREWT").DefaultCellStyle.Format = "0.000"
        gridView.Columns("TOUCH").DefaultCellStyle.Format = "0.00"
        gridView.Columns("SNO").Visible = False
        gridView.Columns("CHECK").ReadOnly = False
        gridView.Columns("ITEMTYPEID").Visible = False
        gridView.Focus()
        btnLotIssue.Enabled = chkSelectAll.Checked
    End Sub

    Private Sub btnLotIssue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLotIssue.Click
        Dim Tran As OleDbTransaction = Nothing
        Dim DesignerId As String = Nothing
        Dim COSTID As String = Nothing
        Dim itemId As Integer = Nothing
        Dim stockType As String = Nothing
        Dim subItemId As Integer = Nothing
        Dim RoIssue As DataRow() = Nothing
        Dim noOfTag As Integer = Nothing
        Dim itemCounterId As Integer = Nothing
        Dim LotNo As Integer
        Dim entryType As String = Nothing
        Dim dtSelected As New DataTable
        dtSelected = CType(gridView.DataSource, DataTable)
        dtSelected.AcceptChanges()
        RoIssue = dtSelected.Select("CHECK = 'TRUE'", String.Empty)
        Dim objBulkIssue As New W_LotBulkIssueDia
        If objBulkIssue.ShowDialog <> Windows.Forms.DialogResult.OK Then Exit Sub
        Try

            Tran = CN.BeginTransaction()
            'Getting LotNo
GENLOTNO:
            StrSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'LOTNO'"
            LotNo = Val(ObjGPack.GetSqlValue(StrSql, , , Tran))
            StrSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT = '" & LotNo + 1 & "' "
            StrSql += " WHERE CTLID = 'LOTNO' AND CTLTEXT = " & LotNo & ""
            Cmd = New OleDbCommand(StrSql, CN, Tran)
            If Cmd.ExecuteNonQuery() = 0 Then
                GoTo GENLOTNO
            End If
            LotNo += 1
            For cnt As Integer = 0 To RoIssue.Length - 1
                Dim lotSno As String = GetNewSno(TranSnoType.ITEMLOTCODE, Tran, "GET_ADMINSNO_TRAN") '  GetWSno(TranSnoType.ITEMLOTCODE, Tran, CNSTOCKDB)
                ''Find ItemId
                StrSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & RoIssue(cnt).Item("ITEM") & "'"
                itemId = Val(objGPack.GetSqlValue(StrSql, , , Tran))
                StrSql = " SELECT STOCKTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & RoIssue(cnt).Item("ITEM") & "'"
                stockType = objGPack.GetSqlValue(StrSql, , , Tran)
                ''Find DesignerId
                StrSql = " SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & objBulkIssue.cmbDesigner_MAN.Text & "'"
                DesignerId = objGPack.GetSqlValue(StrSql, , , Tran)
                ''Find COSTID
                StrSql = " SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & objBulkIssue.cmbCostCentre_Man.Text & "'"
                COSTID = objGPack.GetSqlValue(StrSql, , , Tran)
                ''Find SubItem Id
                StrSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST "
                StrSql += " WHERE SUBITEMNAME = '" & RoIssue(cnt).Item("SUBITEM") & "'"
                subItemId = Val(objGPack.GetSqlValue(StrSql, , , Tran))
                ''Find ItemTypeId

                StrSql = " SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & objBulkIssue.cmbItemCounter_MAN.Text & "'"
                itemCounterId = Val(objGPack.GetSqlValue(StrSql, , , Tran))

                StrSql = " SELECT NOOFPIECE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & RoIssue(cnt).Item("ITEM") & "'"
                noOfTag = Val(objGPack.GetSqlValue(StrSql, , , Tran))

                Select Case objBulkIssue.cmbEntryType.Text.ToUpper
                    Case "REGULAR"
                        entryType = "R"
                    Case "ORDER"
                        entryType = "OR"
                    Case "REPAIR"
                        entryType = "RE"
                    Case Else 'NONTAG TO TAG
                        entryType = "NT"
                End Select
                StrSql = " INSERT INTO " & cnAdminDb & "..ITEMLOT "
                StrSql += " ("
                StrSql += " SNO,ENTRYTYPE,LOTDATE,LOTNO,DESIGNERID,TRANINVNO,"
                StrSql += " BILLNO,COSTID,ENTRYORDER,ORDREPNO,ORDENTRYORDER,"
                StrSql += " ITEMID,SUBITEMID,PCS,GRSWT,STNPCS,STNWT,STNUNIT,"
                StrSql += " DIAPCS,DIAWT,NETWT,NOOFTAG,RATE,ITEMCTRID,WMCTYPE,"
                StrSql += " BULKLOT,MULTIPLETAGS,NARRATION,FINERATE,TUCH,"
                StrSql += " WASTPER,MCGRM,OTHCHARGE,STARTTAGNO,ENDTAGNO,CURTAGNO,"
                StrSql += " COMPANYID,CPCS,CGRSWT,COMPLETED,CANCEL,VATEXM,"
                StrSql += " ACCESSING,USERID,UPDATED,"
                StrSql += " UPTIME,SYSTEMID,APPVER,ITEMTYPEID)VALUES("
                StrSql += " '" & lotSno & "'" 'SNO
                StrSql += " ,'" & entryType & "'" 'ENTRYTYPE
                StrSql += " ,'" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'" 'LOTDATE 'DateTime.Parse(RoIssue(CNT).ITEM("LOTDATE"), ukCultureInfo.DateTimeFormat).ToString("yyyy-MM-dd")
                StrSql += " ," & LotNo & "" 'LOTNO
                StrSql += " ," & DesignerId & "" 'DESIGNERID
                StrSql += " ,''" 'TRANINVNO
                StrSql += " ,''" 'BILLNO
                StrSql += " ,'" & COSTID & "'" 'COSTID
                StrSql += " ," & cnt + 1 & "" 'ENTRYORDER
                StrSql += " ,''" 'ORDREPNO
                StrSql += " ,0" 'ORDENTRYORDER
                StrSql += " ," & Val(itemId) & "" 'ITEMID
                StrSql += " ," & Val(subItemId) & "" 'SUBITEMID
                StrSql += " ," & Val(RoIssue(cnt).Item("PCS").ToString) & "" 'PCS
                StrSql += " ," & Val(RoIssue(cnt).Item("GRSWT").ToString) & "" 'GRSWT
                StrSql += " ,0" 'STNPCS
                StrSql += " ,0" 'STNWT
                StrSql += " ,'G'" 'STNUNIT
                StrSql += " ,0" 'DIAPCS
                StrSql += " ,0" 'DIAWT
                StrSql += " ," & Val(RoIssue(cnt).Item("NETWT").ToString) & "" 'NETWT
                StrSql += " ," & IIf(noOfTag = 0, 1, noOfTag) & "" 'NOOFTAG
                StrSql += " ,0" 'RATE
                StrSql += " ," & Val(itemCounterId) & "" 'ITEMCTRID
                StrSql += " ,'" & objGPack.GetSqlValue("SELECT VALUEADDEDTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & RoIssue(cnt).Item("ITEM").ToString & "'", , , Tran) & "'" 'WMCTYPE
                StrSql += " ,'N'" 'BULKLOT
                StrSql += " ,'N'" 'MULTIPLETAGS
                StrSql += " ,''" 'NARRATION
                StrSql += " ,0" 'FINERATE
                StrSql += " ," & Val(RoIssue(cnt).Item("TOUCH").ToString) & "" 'TUCH
                StrSql += " ,0" 'WASTPER
                StrSql += " ,0" 'MCGRM
                StrSql += " ,0" 'OTHCHARGE
                StrSql += " ,''" 'STARTTAGNO
                StrSql += " ,''" 'ENDTAGNO
                StrSql += " ,''" 'CURTAGNO
                StrSql += " ,'" & cnCompanyId & "'" 'COMPANYID
                StrSql += " ,0" 'CPIECE
                StrSql += " ,0" 'CWEIGHT
                StrSql += " ,''" 'COMPLETED
                StrSql += " ,''" 'CANCEL
                StrSql += " ,''" 'VATEXM
                StrSql += " ,''" 'ACCESSING
                StrSql += " ," & userId & "" 'USERID
                StrSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                StrSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                StrSql += " ,'" & systemId & "'" 'SYSTEMID
                StrSql += " ,'" & VERSION & "'" 'APPVER
                StrSql += " ," & Val(RoIssue(cnt).Item("ITEMTYPEID").ToString) & "" 'ITEMTYPEID
                StrSql += " )"
                ExecQuery(SyncMode.Stock, StrSql, cn, Tran, COSTID)

                StrSql = "  INSERT INTO " & cnStockDb & "..W_LOTISSUE"
                StrSql += "  ("
                StrSql += "  TRANNO"
                StrSql += "  ,TRANDATE"
                StrSql += "  ,GRSWT"
                StrSql += "  ,NETWT"
                StrSql += "  ,CANCEL"
                StrSql += "  ,BATCHNO"
                StrSql += "  ,USERID"
                StrSql += "  ,UPDATED"
                StrSql += "  ,APPVER"
                StrSql += "  ,COMPANYID"
                StrSql += "  ,PCS"
                StrSql += "  ,LOTSNO"
                StrSql += "  ,ITEMID"
                StrSql += "  ,SUBITEMID"
                StrSql += "  ,RECSNO"
                StrSql += "  )"
                StrSql += "  SELECT"
                StrSql += "  " & RoIssue(cnt).Item("TRANNO") & ""
                StrSql += "  ,'" & RoIssue(cnt).Item("TRANDATE") & "'"
                StrSql += "  ," & Val(RoIssue(cnt).Item("GRSWT").ToString) & "" 'GRSWT
                StrSql += "  ," & Val(RoIssue(cnt).Item("NETWT").ToString) & "" 'NETWT
                StrSql += "  ,''" 'CANCEL
                StrSql += "  ,''" 'BATCHNO
                StrSql += "  ," & userId & "" 'USERID
                StrSql += "  ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                StrSql += "  ,'" & VERSION & "'" 'APPVER
                StrSql += "  ,'" & cnCompanyId & "'" 'COMPANYID
                StrSql += "  ," & Val(RoIssue(cnt).Item("PCS").ToString) & "" 'PCS
                StrSql += "  ,'" & lotSno & "'" 'LOTSNO
                StrSql += "  ," & itemId & "" 'ITEMID
                StrSql += "  ," & subItemId & "" 'SUBITEMID
                StrSql += "  ,'" & RoIssue(cnt).Item("SNO").ToString & "'" 'SNO
                ExecQuery(SyncMode.Stock, StrSql, cn, Tran, COSTID)
            Next
            Tran.Commit()
            Tran = Nothing
            MsgBox(LotNo.ToString + " Generated...", MsgBoxStyle.Exclamation)
        Catch ex As Exception
            If Not Tran Is Nothing Then Tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            If Not Tran Is Nothing Then Tran.Dispose()
        End Try
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub gridView_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellValueChanged
        Dim dt As New DataTable
        dt = CType(gridView.DataSource, DataTable)
        dt.AcceptChanges()
        Dim ro() As DataRow = dt.Select("CHECK = 'TRUE'", String.Empty)
        If ro.Length > 0 Then
            btnLotIssue.Enabled = True
        Else
            btnLotIssue.Enabled = False
        End If
    End Sub

    Private Sub gridView_CurrentCellDirtyStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.CurrentCellDirtyStateChanged
        gridView.CommitEdit(DataGridViewDataErrorContexts.Commit)
    End Sub
End Class