Imports System.Data.OleDb
Imports System.Linq
Public Class FRM_CUSTOMER_RECORD
    Dim StrSql As String
    Dim Da As OleDbDataAdapter
    Dim Cmd As OleDbCommand
    Dim lstSource As New List(Of Object)
    Dim RowHash As BrightPosting.GDataRow = Nothing
    Dim PicPath As String = GetAdmindbSoftValue("PICPATH")
    Dim CataLogPath As String = GetAdmindbSoftValue("TAGCATALOGPATH")
    Dim dtCompany, dtCostCentre As New DataTable
    Dim Column_Count, Grid_Size As Integer
    Dim commonTranDb As New ArrayList()
    Private Sub FRM_CUSTOMER_RECORD_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyCode = Keys.F3 Then
            btnNew_Click(Me, New EventArgs)
        ElseIf e.KeyCode = Keys.F12 Then
            btnExit_Click(Me, New EventArgs)
        ElseIf e.KeyCode = Keys.Escape Then
            If dgv2.Focused Then Dgv.Focus() : Exit Sub
            If Dgv.Focused Then dgv2.Focus() : Exit Sub

        End If
    End Sub

    Private Sub FRM_CUSTOMER_RECORD_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If PicPath <> "" Then
            If PicPath.EndsWith("\") = False Then PicPath += "\"
        End If
        If CataLogPath <> "" Then
            If CataLogPath.EndsWith("\") = False Then CataLogPath += "\"
        End If
        grpFiltration.Location = New Point((ScreenWid - grpFiltration.Width) / 2, ((ScreenHit - 128) - grpFiltration.Height) / 2)
        btnNew_Click(Me, New EventArgs)
        StrSql = " SELECT DBNAME from " & cnAdminDb & "..DBMASTER WHERE DBNAME<>'" & cnStockDb & "' ORDER BY STARTDATE DESC"
        Dim dtName As New DataTable()
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtName)
        If dtName.Rows.Count > 0 Then
            For i As Integer = 0 To dtName.Rows.Count - 1
                If objGPack.GetSqlValue("SELECT NAME FROM SYSDATABASES WHERE NAME = '" & dtName.Rows(i).Item("DBNAME") & "'").Length > 0 Then
                    commonTranDb.Add(dtName.Rows(i).Item("DBNAME"))
                End If
            Next
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        cmbSearchKey.Text = ""
        CmbOption.Text = "ALL"
        txtSearch_txt.Clear()
        StrSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        StrSql += " UNION ALL"
        StrSql += " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY "
        StrSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)
        StrSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        StrSql += " UNION ALL"
        StrSql += " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        StrSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , "ALL")
        dtpFrom.Value = cnTranFromDate.Date.ToString("yyyy-MM-dd")
        dtpFrom.Select()
        Dgv.DataSource = Nothing
        dgv2.DataSource = Nothing

    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dgv.DataSource = Nothing
        Dgv.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        Me.Refresh()
        Dim companyid As String = GetSelectedComId(chkcmbCompany, True)
        Dim costid As String = GetSelectedCostId(chkCmbCostCentre, True)
        Dim Trantype As String = CmbOption.Text
        Dim temptable As String = "TEMPTABLEDB..TEMP" & systemId & "CUSTOMERINFO"

        StrSql = vbCrLf + "  IF OBJECT_ID('" & temptable & "') IS NOT NULL DROP TABLE " & temptable & ""
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        'StrSql = " SELECT DISTINCT C.BATCHNO,CONVERT(SMALLDATETIME,NULL)TRANDATE,CONVERT(INTEGER ,NULL)TRANNO,SNO,TITLE,PNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,STATE,COUNTRY,PINCODE,PHONERES,MOBILE,EMAIL,FAX "
        'StrSql += " INTO TEMPTABLEDB..TEMPCUSTOMERINFO FROM " & cnAdminDb & "..PERSONALINFO P"
        'StrSql += " LEFT JOIN " & cnAdminDb & "..CUSTOMERINFO C ON P.SNO=C.PSNO "
        'StrSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Value & "' AND '" & dtpTo.Value & "'"
        'If companyid.Trim <> "" And companyid.Trim <> "ALL" Then StrSql += " AND COMPANYID IN(" & companyid & ")"
        'If costid.Trim <> "''" And costid.Trim <> "ALL" Then StrSql += " AND P.COSTID IN(" & costid & ")"
        'If txtSearch_txt.Text <> "" Then
        '    StrSql += " AND " & cmbSearchKey.Text & " LIKE"
        '    StrSql += " '" & txtSearch_txt.Text & "%'"
        'End If
        'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        'StrSql = vbCrLf + "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPCUSTOMERINFO WHERE TRANDATE IS NULL)>0"
        'StrSql += vbCrLf + "  BEGIN"
        'StrSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMPCUSTOMERINFO SET TRANDATE = I.TRANDATE,TRANNO=I.TRANNO "
        'StrSql += vbCrLf + "  FROM TEMPTABLEDB..TEMPCUSTOMERINFO AS T INNER JOIN " & cnStockDb & "..ISSUE AS I ON I.BATCHNO = T.BATCHNO"
        'StrSql += vbCrLf + "  WHERE T.TRANDATE IS NULL"
        'StrSql += vbCrLf + "  END"
        'StrSql += vbCrLf + "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPCUSTOMERINFO WHERE TRANDATE IS NULL)>0"
        'StrSql += vbCrLf + "  BEGIN"
        'StrSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMPCUSTOMERINFO SET TRANDATE = I.TRANDATE,TRANNO=I.TRANNO "
        'StrSql += vbCrLf + "  FROM TEMPTABLEDB..TEMPCUSTOMERINFO AS T INNER JOIN " & cnStockDb & "..RECEIPT AS I ON I.BATCHNO = T.BATCHNO"
        'StrSql += vbCrLf + "  WHERE T.TRANDATE IS NULL"
        'StrSql += vbCrLf + "  END"
        'StrSql += vbCrLf + "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPCUSTOMERINFO WHERE TRANDATE IS NULL)>0"
        'StrSql += vbCrLf + "  BEGIN"
        'StrSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMPCUSTOMERINFO SET TRANDATE = I.TRANDATE,TRANNO=I.TRANNO "
        'StrSql += vbCrLf + "  FROM TEMPTABLEDB..TEMPCUSTOMERINFO AS T INNER JOIN " & cnStockDb & "..ACCTRAN AS I ON I.BATCHNO = T.BATCHNO"
        'StrSql += vbCrLf + "  WHERE T.TRANDATE IS NULL"
        'StrSql += vbCrLf + "  END"
        'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        StrSql = "  SELECT DISTINCT C.BATCHNO,I.TRANDATE,I.TRANNO,P.SNO,P.TITLE,I.TRANTYPE  "
        StrSql += vbCrLf + "  ,P.PNAME,P.DOORNO,P.ADDRESS1,P.ADDRESS2,P.ADDRESS3,AC.DOBIRTH,AC.ANNIVERSARY,P.AREA,P.CITY,P.STATE,P.COUNTRY,P.PINCODE,P.PHONERES,P.MOBILE,P.EMAIL,P.FAX,P.PAN,P.GSTNO "
        StrSql += vbCrLf + "  INTO " & temptable & " "
        StrSql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE I"
        StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CUSTOMERINFO C ON C.BATCHNO=I.BATCHNO"
        StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..PERSONALINFO P  ON P.SNO=C.PSNO "
        StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..ACHEAD AC ON AC.ACCODE=P.ACCODE "
        StrSql += " WHERE  I.TRANDATE BETWEEN '" & dtpFrom.Value & "' AND '" & dtpTo.Value & "' AND ISNULL(I.CANCEL,'')=''"
        If companyid.Trim <> "" And companyid.Trim <> "ALL" Then StrSql += " AND I.COMPANYID IN(" & companyid & ")"
        If costid.Trim <> "''" And costid.Trim <> "ALL" Then StrSql += " AND I.COSTID IN(" & costid & ")"
        If Trantype.Trim <> "" And Trantype.Trim <> "ALL" Then StrSql += " AND I.TRANTYPE IN ('SA','RD','OD')"
        If Trantype.Trim <> "" And Trantype.Trim <> "ALL" And Trantype.Trim = "RECEIPT" Then StrSql += " AND 1<>1 "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        StrSql = "  INSERT INTO " & temptable & " "
        StrSql += vbCrLf + "  SELECT DISTINCT C.BATCHNO,I.TRANDATE,I.TRANNO,P.SNO,P.TITLE,I.TRANTYPE "
        StrSql += vbCrLf + "  ,P.PNAME,P.DOORNO,P.ADDRESS1,P.ADDRESS2,P.ADDRESS3,AC.DOBIRTH,AC.ANNIVERSARY,P.AREA,P.CITY,P.STATE,P.COUNTRY,P.PINCODE,P.PHONERES,P.MOBILE,P.EMAIL,P.FAX,P.PAN,P.GSTNO "
        StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT I"
        StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CUSTOMERINFO C ON C.BATCHNO=I.BATCHNO"
        StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..PERSONALINFO P  ON P.SNO=C.PSNO "
        StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..ACHEAD AC ON AC.ACCODE=P.ACCODE "
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value & "' AND '" & dtpTo.Value & "' AND ISNULL(I.CANCEL,'')=''"
        StrSql += vbCrLf + "  AND I.BATCHNO NOT IN(SELECT BATCHNO FROM " & temptable & ")"
        If companyid.Trim <> "" And companyid.Trim <> "ALL" Then StrSql += " AND I.COMPANYID IN(" & companyid & ")"
        If costid.Trim <> "''" And costid.Trim <> "ALL" Then StrSql += " AND I.COSTID IN(" & costid & ")"
        If Trantype.Trim <> "" And Trantype.Trim <> "ALL" And Trantype.Trim = "RECEIPT" Then StrSql += " AND I.TRANTYPE IN ('SR','PU')"
        If Trantype.Trim <> "" And Trantype.Trim <> "ALL" And Trantype.Trim = "SALES" Then StrSql += " AND 1<>1 "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        If commonTranDb.Count > 0 Then
            For i As Integer = 0 To commonTranDb.Count - 1
                StrSql = "  INSERT INTO " & temptable & " "
                StrSql += vbCrLf + "  SELECT DISTINCT C.BATCHNO,I.TRANDATE,I.TRANNO,P.SNO,P.TITLE,I.TRANTYPE  "
                StrSql += vbCrLf + "  ,P.PNAME,P.DOORNO,P.ADDRESS1,P.ADDRESS2,P.ADDRESS3,AC.DOBIRTH,AC.ANNIVERSARY,P.AREA,P.CITY,P.STATE,P.COUNTRY,P.PINCODE,P.PHONERES,P.MOBILE,P.EMAIL,P.FAX,P.PAN,P.GSTNO "
                StrSql += vbCrLf + "  FROM " & commonTranDb(i).ToString & "..ISSUE I"
                StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CUSTOMERINFO C ON C.BATCHNO=I.BATCHNO"
                StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..PERSONALINFO P  ON P.SNO=C.PSNO "
                StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..ACHEAD AC ON AC.ACCODE=P.ACCODE "
                StrSql += vbCrLf + "  WHERE  I.TRANDATE BETWEEN '" & dtpFrom.Value & "' AND '" & dtpTo.Value & "' AND ISNULL(I.CANCEL,'')=''"
                If companyid.Trim <> "" And companyid.Trim <> "ALL" Then StrSql += " AND I.COMPANYID IN(" & companyid & ")"
                If costid.Trim <> "''" And costid.Trim <> "ALL" Then StrSql += " AND I.COSTID IN(" & costid & ")"
                If Trantype.Trim <> "" And Trantype.Trim <> "ALL" Then StrSql += " AND I.TRANTYPE IN ('SA','RD','OD')"
                If Trantype.Trim <> "" And Trantype.Trim <> "ALL" And Trantype.Trim = "RECEIPT" Then StrSql += " AND 1<>1 "
                Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
                StrSql = "  INSERT INTO " & temptable & " "
                StrSql += vbCrLf + "  SELECT DISTINCT C.BATCHNO,I.TRANDATE,I.TRANNO,P.SNO,P.TITLE,I.TRANTYPE "
                StrSql += vbCrLf + "  ,P.PNAME,P.DOORNO,P.ADDRESS1,P.ADDRESS2,P.ADDRESS3,AC.DOBIRTH,AC.ANNIVERSARY,P.AREA,P.CITY,P.STATE,P.COUNTRY,P.PINCODE,P.PHONERES,P.MOBILE,P.EMAIL,P.FAX,P.PAN,P.GSTNO "
                StrSql += vbCrLf + "  FROM " & commonTranDb(i).ToString & "..RECEIPT I"
                StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CUSTOMERINFO C ON C.BATCHNO=I.BATCHNO"
                StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..PERSONALINFO P  ON P.SNO=C.PSNO "
                StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..ACHEAD AC ON AC.ACCODE=P.ACCODE "
                StrSql += vbCrLf + "  WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value & "' AND '" & dtpTo.Value & "' AND ISNULL(I.CANCEL,'')=''"
                StrSql += vbCrLf + "  AND I.BATCHNO NOT IN(SELECT BATCHNO FROM " & temptable & ")"
                If companyid.Trim <> "" And companyid.Trim <> "ALL" Then StrSql += " AND I.COMPANYID IN(" & companyid & ")"
                If costid.Trim <> "''" And costid.Trim <> "ALL" Then StrSql += " AND I.COSTID IN(" & costid & ")"
                If Trantype.Trim <> "" And Trantype.Trim <> "ALL" And Trantype.Trim = "RECEIPT" Then StrSql += " AND I.TRANTYPE IN ('SR','PU')"
                If Trantype.Trim <> "" And Trantype.Trim <> "ALL" And Trantype.Trim = "SALES" Then StrSql += " AND 1<>1 "
                Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
            Next
        End If
        'If Trantype.Trim <> "" And Trantype.Trim <> "ALL" Then StrSql += " AND I.TRANTYPE IN ('SA','RD','OD')"
        'StrSql += vbCrLf + "  UNION ALL"
        'StrSql += vbCrLf + "  SELECT DISTINCT C.BATCHNO,I.TRANDATE,I.TRANNO,P.SNO,P.TITLE,I.TRANTYPE "
        'StrSql += vbCrLf + "  ,P.PNAME,P.DOORNO,P.ADDRESS1,P.ADDRESS2,P.ADDRESS3,AC.DOBIRTH,AC.ANNIVERSARY,P.AREA,P.CITY,P.STATE,P.COUNTRY,P.PINCODE,P.PHONERES,P.MOBILE,P.EMAIL,P.FAX "
        'StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT I"
        'StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CUSTOMERINFO C ON C.BATCHNO=I.BATCHNO"
        'StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..PERSONALINFO P  ON P.SNO=C.PSNO "
        'StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..ACHEAD AC ON AC.ACCODE=P.ACCODE "
        'StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value & "' AND '" & dtpTo.Value & "' AND ISNULL(I.CANCEL,'')=''"
        'StrSql += vbCrLf + "  AND I.BATCHNO NOT IN(SELECT BATCHNO FROM " & temptable & ")"
        'If companyid.Trim <> "" And companyid.Trim <> "ALL" Then StrSql += " AND I.COMPANYID IN(" & companyid & ")"
        'If costid.Trim <> "''" And costid.Trim <> "ALL" Then StrSql += " AND I.COSTID IN(" & costid & ")"
        'If Trantype.Trim <> "" And Trantype.Trim <> "ALL" Then StrSql += " AND I.TRANTYPE IN ('SA','RD','OD')"
        'Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        StrSql = "  INSERT INTO " & temptable & " "


        StrSql = vbCrLf + "  SELECT * FROM " & temptable & " WHERE 1=1"
        If txtSearch_txt.Text <> "" Then
            'StrSql += " AND " & cmbSearchKey.Text & " LIKE"
            'StrSql += " '" & txtSearch_txt.Text & "%'"
            StrSql += " AND PNAME+ADDRESS1+AREA+CITY+STATE+MOBILE+PHONERES LIKE"
            StrSql += " '%" & txtSearch_txt.Text & "%'"
        End If
        StrSql += " ORDER BY TRANDATE,TRANNO"
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("CHECK", GetType(Boolean))
        dtGrid.Columns("CHECK").DefaultValue = False
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        Dgv.DataSource = dtGrid
        For cnt As Integer = 0 To Dgv.Columns.Count - 1
            Dgv.Columns(cnt).Visible = True
            Dgv.Columns(cnt).ReadOnly = True
        Next
        Dgv.Columns("SNO").Visible = False
        Dgv.Columns("TITLE").Visible = False
        Dgv.Columns("BATCHNO").Visible = False
        Dgv.Columns("TRANTYPE").Visible = False
        Dgv.Columns("PNAME").HeaderText = "NAME"
        Dgv.Columns("CHECK").HeaderText = "CHECK"
        Dgv.Columns("CHECK").ReadOnly = False

        Dgv.Columns("CHECK").Width = 50
        Dgv.Columns("TRANDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
        Dgv.Columns("ANNIVERSARY").DefaultCellStyle.Format = "dd-MM-yyyy"
        Dgv.Columns("DOBIRTH").DefaultCellStyle.Format = "dd-MM-yyyy"
        Dgv.Focus()
    End Sub

    Public Function GetSelectedComId(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = "" & strCompanyId & ""
        End If
        Return retStr
    End Function

    Private Sub ResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResizeToolStripMenuItem.Click
        If Dgv.RowCount > 0 Then
            If ResizeToolStripMenuItem.Checked Then
                Dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                Dgv.Invalidate()
                For Each dgvCol As DataGridViewColumn In Dgv.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                Dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In Dgv.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                Dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
            'DGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End If
    End Sub

    Private Sub AddRow(ByRef DtSource As BrightPosting.GDatatable, ByVal Row As DataRow)
        DtSource.Rows.Add(Row)
    End Sub

    Private Sub btnPrint_Select_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint_Select.Click
        lstSource.Clear()

        Dim RowSelected() As DataRow = CType(Dgv.DataSource, DataTable).Select("CHECK = TRUE", "CHECK")
        Dim Cnt As Integer = 1
        Dim Str As String = Nothing
        Dim RowTemp As BrightPosting.GDataRow

        Dim picBox As New PictureBox
        picBox.Size = New Size(150, 150)
        For Each Ro As DataRow In RowSelected
            Dim vDt As New BrightPosting.GDatatable
            vDt.Columns.Add("CONTENT", GetType(String))
            vDt.Columns.Add("LOGO", GetType(PictureBox))
            vDt.pRowHeight = picBox.Height
            vDt.pColHeaderVisible = False
            vDt.pCellBorder = False
            vDt.pContentFont = New Font("TIMES NEW ROMAN", 8, FontStyle.Bold)
            vDt.Columns("LOGO").Caption = picBox.Size.Width
            vDt.Columns("CONTENT").Caption = 400


            Str = Ro.Item("PNAME").ToString.Trim
            If Ro.Item("DOORNO").ToString.Trim <> "" Then Str += vbCrLf + Ro.Item("DOORNO").ToString.Trim
            If Ro.Item("ADDRESS1").ToString.Trim <> "" Then Str += vbCrLf + Ro.Item("ADDRESS1").ToString
            If Ro.Item("ADDRESS2").ToString.Trim <> "" Then Str += vbCrLf + Ro.Item("ADDRESS2").ToString
            If Ro.Item("ADDRESS3").ToString.Trim <> "" Then Str += vbCrLf + Ro.Item("ADDRESS3").ToString
            If Ro.Item("AREA").ToString.Trim <> "" Then Str += vbCrLf + Ro.Item("AREA").ToString
            Dim o As String = Ro.Item("CITY").ToString.Trim & IIf(Ro.Item("PINCODE").ToString.Trim <> "", " - " & Ro.Item("PINCODE").ToString, "")
            If Ro.Item("CITY").ToString.Trim <> "" Then Str += vbCrLf + o
            If Ro.Item("PHONERES").ToString.Trim <> "" Then Str += vbCrLf + "Phone Res : " & Ro.Item("PHONERES").ToString
            If Ro.Item("MOBILE").ToString.Trim <> "" Then Str += vbCrLf + "Mobile : " & Ro.Item("MOBILE").ToString
            If Ro.Item("EMAIL").ToString.Trim <> "" Then Str += vbCrLf + "E-Mail : " & Ro.Item("EMAIL").ToString
            If Ro.Item("FAX").ToString.Trim <> "" Then Str += vbCrLf + "Fax : " & Ro.Item("FAX").ToString

            RowTemp = vDt.NewRow
            RowTemp.pRowHeight = 160
            RowTemp.Item("CONTENT") = Str
            AutoImageSizer(My.Resources.no_photo, picBox, PictureBoxSizeMode.CenterImage)
            RowTemp.Item("LOGO") = picBox
            vDt.Rows.Add(RowTemp)
            vDt.AcceptChanges()
            lstSource.Add(vDt)
            GetSnoInfo(Ro.Item("SNO").ToString)

            If Cnt <> RowSelected.Length Then
                lstSource.Add(Chr(13))
            End If
            Cnt += 1
        Next

        Dim obj As New BrightPosting.GListPrinter(lstSource)
        obj.pTitle = $"CUSTOMER TRANSACTION FLOW {vbCrLf} {chkcmbCompany.Text}"
        obj.Print()
    End Sub

    Private Sub GetSnoInfo(ByVal Sno As String)
        StrSql = vbCrLf + "  IF OBJECT_ID('TEMPTABLEDB..TEMPCUSTOMERINFO') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPCUSTOMERINFO"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        StrSql = vbCrLf + "  SELECT DISTINCT BATCHNO,CONVERT(SMALLDATETIME,NULL)TRANDATE "
        StrSql += vbCrLf + "  INTO TEMPTABLEDB..TEMPCUSTOMERINFO"
        StrSql += vbCrLf + "  FROM " & cnAdminDb & "..CUSTOMERINFO"
        StrSql += vbCrLf + "  WHERE PSNO = '" & Sno & "'"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        StrSql = vbCrLf + "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPCUSTOMERINFO WHERE TRANDATE IS NULL)>0"
        StrSql += vbCrLf + "  BEGIN"
        StrSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMPCUSTOMERINFO SET TRANDATE = I.TRANDATE"
        StrSql += vbCrLf + "  FROM TEMPTABLEDB..TEMPCUSTOMERINFO AS T INNER JOIN " & cnStockDb & "..ISSUE AS I ON I.BATCHNO = T.BATCHNO"
        StrSql += vbCrLf + "  WHERE T.TRANDATE IS NULL"
        StrSql += vbCrLf + "  END"
        StrSql += vbCrLf + "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPCUSTOMERINFO WHERE TRANDATE IS NULL)>0"
        StrSql += vbCrLf + "  BEGIN"
        StrSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMPCUSTOMERINFO SET TRANDATE = I.TRANDATE"
        StrSql += vbCrLf + "  FROM TEMPTABLEDB..TEMPCUSTOMERINFO AS T INNER JOIN " & cnStockDb & "..RECEIPT AS I ON I.BATCHNO = T.BATCHNO"
        StrSql += vbCrLf + "  WHERE T.TRANDATE IS NULL"
        StrSql += vbCrLf + "  END"
        StrSql += vbCrLf + "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPCUSTOMERINFO WHERE TRANDATE IS NULL)>0"
        StrSql += vbCrLf + "  BEGIN"
        StrSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMPCUSTOMERINFO SET TRANDATE = I.TRANDATE"
        StrSql += vbCrLf + "  FROM TEMPTABLEDB..TEMPCUSTOMERINFO AS T INNER JOIN " & cnStockDb & "..ACCTRAN AS I ON I.BATCHNO = T.BATCHNO"
        StrSql += vbCrLf + "  WHERE T.TRANDATE IS NULL"
        StrSql += vbCrLf + "  END"
        StrSql += vbCrLf + "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPCUSTOMERINFO WHERE TRANDATE IS NULL)>0"
        StrSql += vbCrLf + "  BEGIN"
        StrSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMPCUSTOMERINFO SET TRANDATE = I.ORDATE"
        StrSql += vbCrLf + "  FROM TEMPTABLEDB..TEMPCUSTOMERINFO AS T INNER JOIN " & cnAdminDb & "..ORMAST AS I ON I.BATCHNO = T.BATCHNO"
        StrSql += vbCrLf + "  WHERE T.TRANDATE IS NULL"
        StrSql += vbCrLf + "  END"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        StrSql = vbCrLf + "  SELECT * FROM TEMPTABLEDB..TEMPCUSTOMERINFO"
        Dim DtTemp As New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(DtTemp)
        If Not DtTemp.Rows.Count > 0 Then Exit Sub
        Dim vDt As New BrightPosting.GDatatable
        vDt.TableName = "TRANHEAD"
        vDt.Columns.Add("TAGIMAGE", GetType(PictureBox))
        vDt.Columns.Add("TRANNO", GetType(Integer))
        vDt.Columns.Add("TRANDATE", GetType(String))
        vDt.Columns.Add("DESCRIPTION", GetType(String))
        vDt.Columns.Add("TAGNO", GetType(String))
        vDt.Columns.Add("PCS", GetType(Integer))
        vDt.Columns.Add("GRSWT", GetType(Decimal))
        vDt.Columns.Add("AMOUNT", GetType(Decimal))

        vDt.Columns("TAGIMAGE").Caption = IIf(chkWithTagImage.Checked, 120, 0)
        vDt.Columns("TRANNO").Caption = 60
        vDt.Columns("TRANDATE").Caption = 80
        vDt.Columns("DESCRIPTION").Caption = 120
        vDt.Columns("TAGNO").Caption = 70
        vDt.Columns("PCS").Caption = 50
        vDt.Columns("GRSWT").Caption = 70
        vDt.Columns("AMOUNT").Caption = 80

        vDt.pColHeaderVisible = True
        vDt.AcceptChanges()
        lstSource.Add(vDt) ''Adding Table Head
        For cnt As Integer = 0 To DtTemp.Rows.Count - 1
            If DtTemp.Rows(cnt).Item("TRANDATE").ToString = "" Then Continue For
            AddHeadTable("#" & DtTemp.Rows(cnt).Item("BATCHNO").ToString, Nothing, False)
            GetSalesInfo(vDt.Clone, DtTemp.Rows(cnt).Item("BATCHNO").ToString, CType(DtTemp.Rows(cnt).Item("TRANDATE"), Date))
            GetReturnInfo(vDt.Clone, DtTemp.Rows(cnt).Item("BATCHNO").ToString, CType(DtTemp.Rows(cnt).Item("TRANDATE"), Date))
            GetPurchaseInfo(vDt.Clone, DtTemp.Rows(cnt).Item("BATCHNO").ToString, CType(DtTemp.Rows(cnt).Item("TRANDATE"), Date))
            GetReceiptInfo(vDt.Clone, DtTemp.Rows(cnt).Item("BATCHNO").ToString, CType(DtTemp.Rows(cnt).Item("TRANDATE"), Date))
            GetPaymentInfo(vDt.Clone, DtTemp.Rows(cnt).Item("BATCHNO").ToString, CType(DtTemp.Rows(cnt).Item("TRANDATE"), Date))
            GetOrderBookInfo(vDt.Clone, DtTemp.Rows(cnt).Item("BATCHNO").ToString, CType(DtTemp.Rows(cnt).Item("TRANDATE"), Date))
            GetCollectionInfo(vDt.Clone, DtTemp.Rows(cnt).Item("BATCHNO").ToString, CType(DtTemp.Rows(cnt).Item("TRANDATE"), Date))
            If cnt <> DtTemp.Rows.Count - 1 Then
                'AddHeadTable("", Nothing, False)
            End If
        Next
    End Sub
    Private Function GetTableColumnWidth(ByVal vDt As BrightPosting.GDatatable) As Decimal
        Dim wid As Decimal = Nothing
        For Each col As DataColumn In vDt.Columns
            wid += Val(col.Caption)
        Next
        Return wid
    End Function
    Private Sub AddHeadTable(ByVal HeadDescription As String, ByVal TableBackColor As Color, Optional ByVal CellBorder As Boolean = True)
        Dim DtHeadTable As New BrightPosting.GDatatable
        DtHeadTable.TableName = "HeadTable_" & HeadDescription
        DtHeadTable.pCellBorder = CellBorder
        DtHeadTable.pColHeaderVisible = False
        DtHeadTable.pTableBackColor = TableBackColor
        DtHeadTable.pContentFont = New Font(DtHeadTable.pContentFont.FontFamily.Name, DtHeadTable.pContentFont.Size, FontStyle.Bold)
        DtHeadTable.pTableContentAlignment = StringAlignment.Near
        DtHeadTable.Columns.Add("DESCRIPTION", GetType(String))
        DtHeadTable.Columns("DESCRIPTION").Caption = 10
        RowHash = DtHeadTable.NewRow
        RowHash.Item("DESCRIPTION") = HeadDescription
        DtHeadTable.Rows.Add(RowHash)
        lstSource.Add(DtHeadTable)
    End Sub
    Private Sub GetCollectionInfo(ByVal vDt As BrightPosting.GDatatable, ByVal Batchno As String, ByVal Trandate As Date)
        StrSql = vbCrLf + "  SELECT CONVERT(VARCHAR(200),DESCRIPTION)DESCRIPTION,SUM(AMOUNT)AS AMOUNT"
        StrSql += vbCrLf + "  	FROM"
        StrSql += vbCrLf + "  	("
        StrSql += vbCrLf + "  	SELECT "
        StrSql += vbCrLf + "  	CASE WHEN PAYMODE = 'AA' THEN 'ADVANCE ADJUSTED'"
        StrSql += vbCrLf + "  	WHEN PAYMODE = 'DU' THEN 'CREDIT'"
        StrSql += vbCrLf + "  	WHEN PAYMODE IN ('SS','CG','CZ','CB','CD') THEN 'SCHEME'"
        StrSql += vbCrLf + "  	WHEN PAYMODE = 'CH' THEN 'CHEQUE'"
        StrSql += vbCrLf + "  	WHEN PAYMODE = 'CC' THEN 'CREDIT CARD'"
        StrSql += vbCrLf + "  	WHEN PAYMODE = 'HC' THEN 'HANDLING'"
        StrSql += vbCrLf + "  	WHEN PAYMODE = 'DI' THEN 'DISCOUNT'"
        StrSql += vbCrLf + "  	WHEN PAYMODE = 'CA' THEN 'CASH'"
        StrSql += vbCrLf + "  	ELSE PAYMODE END AS DESCRIPTION"
        StrSql += vbCrLf + "  	,CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END AS AMOUNT"
        StrSql += vbCrLf + "  	FROM " & cnStockDb & "..ACCTRAN "
        StrSql += vbCrLf + "  	WHERE TRANDATE = '" & Trandate.ToString("yyyy-MM-dd") & "' and batchno = '" & Batchno & "'"
        StrSql += vbCrLf + "  	AND PAYMODE IN ('AA','DU','CH','CC','HC','DI','CA','SS','CG','CZ','CB','CD')"
        StrSql += vbCrLf + "  	AND ISNULL(CANCEL,'') = ''"
        StrSql += vbCrLf + "  	)X"
        StrSql += vbCrLf + "  GROUP BY DESCRIPTION"
        Dim DtCollection As New BrightPosting.GDatatable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(DtCollection)
        If DtCollection.Rows.Count > 0 Then
            AddHeadTable("COLLECTION", Color.Gainsboro)
            DtCollection.Columns("DESCRIPTION").Caption = GetTableColumnWidth(vDt) - Val(vDt.Columns("AMOUNT").Caption)
            DtCollection.Columns("DESCRIPTION").Caption = Val(vDt.Columns("AMOUNT").Caption)
            DtCollection.pColHeaderVisible = False
            lstSource.Add(DtCollection)
        End If
    End Sub
    Private Sub GetOrderBookInfo(ByVal vDt As BrightPosting.GDatatable, ByVal Batchno As String, ByVal Trandate As Date)
        StrSql = vbCrLf + " SELECT "
        StrSql += vbCrLf + " STYLENO,ORNO,CONVERT(VARCHAR,ORDATE,103)ORDATE"
        StrSql += vbCrLf + " ,CASE WHEN ISNULL(STYLENO,'') = '' THEN DESCRIPT ELSE STYLENO END AS [DESCRIPTION]"
        StrSql += vbCrLf + " ,PCS,GRSWT"
        StrSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO = O.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
        StrSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ORSTONE WHERE ORSNO = O.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
        StrSql += vbCrLf + " ,ORVALUE,PICTFILE PCTFILE"
        StrSql += vbCrLf + " FROM " & cnAdminDb & "..ORMAST AS O"
        StrSql += vbCrLf + " WHERE ORDATE = '" & Trandate.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " AND ISNULL(ORDCANCEL,'') = '' AND ISNULL(CANCEL,'') = ''"
        StrSql += vbCrLf + " AND BATCHNO = '" & Batchno & "'"
        Dim DtOrderBook As New BrightPosting.GDatatable
        If chkWithTagImage.Checked Then
            DtOrderBook.Columns.Add("TAGIMAGE", GetType(PictureBox))
        End If
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(DtOrderBook)
        Dim picBox As New PictureBox
        If DtOrderBook.Rows.Count > 0 Then
            AddHeadTable("ORDER BOOK", Color.Gainsboro)
            If chkWithTagImage.Checked Then
                DtOrderBook.Columns("TAGIMAGE").Caption = Val(vDt.Columns("TAGIMAGE").Caption)
            End If
            DtOrderBook.Columns("STYLENO").Caption = 80
            DtOrderBook.Columns("ORNO").Caption = Val(vDt.Columns("TRANNO").Caption)
            DtOrderBook.Columns("ORDATE").Caption = Val(vDt.Columns("TRANDATE").Caption)
            DtOrderBook.Columns("DESCRIPTION").Caption = Val(vDt.Columns("DESCRIPTION").Caption)
            DtOrderBook.Columns("PCS").Caption = Val(vDt.Columns("PCS").Caption)
            DtOrderBook.Columns("GRSWT").Caption = Val(vDt.Columns("GRSWT").Caption)
            DtOrderBook.Columns("DIAPCS").Caption = Val(vDt.Columns("PCS").Caption)
            DtOrderBook.Columns("DIAWT").Caption = Val(vDt.Columns("GRSWT").Caption)
            DtOrderBook.Columns("ORVALUE").Caption = Val(vDt.Columns("AMOUNT").Caption)
            DtOrderBook.pColHeaderVisible = True
            If chkWithTagImage.Checked Then
                For Each Row As BrightPosting.GDataRow In DtOrderBook.Rows
                    Row.pRowHeight = DtOrderBook.pRowHeight
                    If IO.File.Exists(IIf(Row.Item("STYLENO").ToString <> "", CataLogPath, PicPath) & Row.Item("PCTFILE").ToString) Then
                        picBox = New PictureBox
                        picBox.Size = New Size(100, 100)
                        AutoImageSizer(IIf(Row.Item("STYLENO").ToString <> "", CataLogPath, PicPath) & Row.Item("PCTFILE").ToString, picBox, PictureBoxSizeMode.CenterImage)
                        Row.Item("TAGIMAGE") = picBox
                        Row.pRowHeight = 100
                    End If
                Next
            End If
            DtOrderBook.Columns.Remove("STYLENO")
            DtOrderBook.Columns.Remove("PCTFILE")
            lstSource.Add(DtOrderBook)
        End If

        'Dim DtSales As New BrightPosting.GDatatable
        'If chkWithTagImage.Checked Then
        '    DtSales.Columns.Add("TAGIMAGE", GetType(PictureBox))
        'End If
        'Da = New OleDbDataAdapter(StrSql, cn)
        'Da.Fill(DtSales)
        'Dim picBox As New PictureBox
        'If DtSales.Rows.Count > 0 Then
        '    AddHeadTable("SALES", Color.Gainsboro)
        '    If chkWithTagImage.Checked Then
        '        For Each Row As BrightPosting.GDataRow In DtSales.Rows
        '            Row.pRowHeight = DtSales.pRowHeight
        '            If IO.File.Exists(PicPath & Row.Item("PCTFILE").ToString) Then
        '                picBox = New PictureBox
        '                picBox.Size = New Size(100, 100)
        '                AutoImageSizer(PicPath & Row.Item("PCTFILE").ToString, picBox, PictureBoxSizeMode.CenterImage)
        '                Row.Item("TAGIMAGE") = picBox
        '                Row.pRowHeight = 100
        '            End If
        '        Next
        '    End If
        '    For Each Col As DataColumn In DtSales.Columns
        '        If vDt.Columns.Contains(Col.ColumnName) = False Then Continue For
        '        DtSales.Columns(Col.ColumnName).Caption = vDt.Columns(Col.ColumnName).Caption
        '    Next
        '    DtSales.pColHeaderVisible = False
        '    lstSource.Add(DtSales)
        'End If

    End Sub
    Private Sub GetPaymentInfo(ByVal vDt As BrightPosting.GDatatable, ByVal Batchno As String, ByVal Trandate As Date)
        StrSql = vbCrLf + "  SELECT TRANNO,CONVERT(VARCHAR,TRANDATE,103)TRANDATE"
        StrSql += vbCrLf + "  ,CASE WHEN PAYMODE = 'AP' THEN 'ADVANCE REPAY'"
        StrSql += vbCrLf + "  WHEN PAYMODE = 'MP' THEN 'OTHER PAYMENT'"
        StrSql += vbCrLf + "  WHEN PAYMODE = 'DP' THEN 'CREDIT PAYMENT'"
        StrSql += vbCrLf + "  WHEN PAYMODE = 'HP' THEN 'SCHEME PAYMENT'"
        'StrSql += vbCrLf + "  WHEN PAYMODE = 'OR' THEN 'FURTHER ADVANCE'"
        StrSql += vbCrLf + "  ELSE PAYMODE END + ' - ' + substring(RUNNO,6,len(RUNNO)) AS DESCRIPTION"
        StrSql += vbCrLf + "  ,AMOUNT"
        StrSql += vbCrLf + "  FROM " & cnAdminDb & "..OUTSTANDING"
        StrSql += vbCrLf + "  WHERE TRANDATE = '" & Trandate.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + "  AND RECPAY = 'P'"
        StrSql += vbCrLf + "  AND FROMFLAG = 'P'"
        StrSql += vbCrLf + "  AND BATCHNO = '" & Batchno & "'"
        Dim DtPayment As New BrightPosting.GDatatable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(DtPayment)
        If DtPayment.Rows.Count > 0 Then
            AddHeadTable("PAYMENT", Color.Gainsboro)
            DtPayment.Columns("TRANNO").Caption = Val(vDt.Columns("TRANNO").Caption)
            DtPayment.Columns("TRANDATE").Caption = Val(vDt.Columns("TRANDATE").Caption)
            DtPayment.Columns("DESCRIPTION").Caption = GetTableColumnWidth(vDt) - Val(vDt.Columns("TRANNO").Caption) - Val(vDt.Columns("TRANDATE").Caption) - Val(vDt.Columns("AMOUNT").Caption)
            DtPayment.Columns("AMOUNT").Caption = Val(vDt.Columns("AMOUNT").Caption)
            DtPayment.pColHeaderVisible = False
            lstSource.Add(DtPayment)
        End If
    End Sub
    Private Sub GetReceiptInfo(ByVal vDt As BrightPosting.GDatatable, ByVal Batchno As String, ByVal Trandate As Date)
        StrSql = vbCrLf + "  SELECT TRANNO,CONVERT(VARCHAR,TRANDATE,103)TRANDATE"
        StrSql += vbCrLf + "  ,CASE WHEN PAYMODE = 'AR' THEN 'CUSTOMER ADVANCE'"
        StrSql += vbCrLf + "  WHEN PAYMODE = 'MR' THEN 'OTHER RECEIPT'"
        StrSql += vbCrLf + "  WHEN PAYMODE = 'DR' THEN 'CREDIT RECEIPT'"
        StrSql += vbCrLf + "  WHEN PAYMODE = 'HR' THEN 'SCHEME RECEIPT'"
        StrSql += vbCrLf + "  WHEN PAYMODE = 'OR' THEN 'FURTHER ADVANCE'"
        StrSql += vbCrLf + "  ELSE PAYMODE END + ' - ' + substring(RUNNO,6,len(RUNNO)) AS DESCRIPTION"
        StrSql += vbCrLf + "  ,AMOUNT"
        StrSql += vbCrLf + "  FROM " & cnAdminDb & "..OUTSTANDING"
        StrSql += vbCrLf + "  WHERE TRANDATE = '" & Trandate.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + "  AND RECPAY = 'R'"
        StrSql += vbCrLf + "  AND FROMFLAG = 'P'"
        StrSql += vbCrLf + "  AND BATCHNO = '" & Batchno & "'"
        Dim DtReceipt As New BrightPosting.GDatatable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(DtReceipt)
        If DtReceipt.Rows.Count > 0 Then
            AddHeadTable("RECEIPT", Color.Gainsboro)
            DtReceipt.Columns("TRANNO").Caption = Val(vDt.Columns("TRANNO").Caption)
            DtReceipt.Columns("TRANDATE").Caption = Val(vDt.Columns("TRANDATE").Caption)
            DtReceipt.Columns("DESCRIPTION").Caption = GetTableColumnWidth(vDt) - Val(vDt.Columns("TRANNO").Caption) - Val(vDt.Columns("TRANDATE").Caption) - Val(vDt.Columns("AMOUNT").Caption)
            DtReceipt.Columns("AMOUNT").Caption = Val(vDt.Columns("AMOUNT").Caption)
            DtReceipt.pColHeaderVisible = False
            lstSource.Add(DtReceipt)
        End If
    End Sub
    Private Sub GetPurchaseInfo(ByVal vDt As BrightPosting.GDatatable, ByVal Batchno As String, ByVal Trandate As Date)
        StrSql = " SELECT I.TRANNO,CONVERT(VARCHAR,I.TRANDATE,103) AS TRANDATE"
        StrSql += vbCrLf + " ,ISNULL(SM.SUBITEMNAME,ISNULL(IM.ITEMNAME,CA.CATNAME)) AS DESCRIPTION"
        StrSql += vbCrLf + " ,I.TAGNO,I.PCS,I.GRSWT,I.AMOUNT"
        StrSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS I"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = I.CATCODE"
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = I.ITEMID"
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = I.ITEMID AND SM.SUBITEMID = I.SUBITEMID"
        StrSql += vbCrLf + " WHERE I.TRANDATE = '" & Trandate.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " AND I.BATCHNO = '" & Batchno & "'"
        StrSql += vbCrLf + " AND I.TRANTYPE = 'PU'"
        StrSql += vbCrLf + " AND I.BATCHNO NOT IN (SELECT BATCHNO FROM " & cnAdminDb & "..ORMAST WHERE BATCHNO = '" & Batchno & "')"
        StrSql += vbCrLf + " UNION"
        StrSql += vbCrLf + " SELECT null,null,'TOTAL' DESCRIPTION,null,sum(I.PCS)PCS,sum(I.GRSWT)GRSWT,sum(I.AMOUNT) AMOUNT"
        StrSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS I"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = I.CATCODE"
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = I.ITEMID"
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = I.ITEMID AND SM.SUBITEMID = I.SUBITEMID"
        StrSql += vbCrLf + " WHERE I.TRANDATE = '" & Trandate.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " AND I.BATCHNO = '" & Batchno & "'"
        StrSql += vbCrLf + " AND I.TRANTYPE = 'PU'"
        StrSql += vbCrLf + " AND I.BATCHNO NOT IN (SELECT BATCHNO FROM " & cnAdminDb & "..ORMAST WHERE BATCHNO = '" & Batchno & "')"
        StrSql += vbCrLf + " order by TRANNO desc"
        Dim DtPurchase As New BrightPosting.GDatatable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(DtPurchase)
        If DtPurchase.Rows.Count > 0 AndAlso DtPurchase.Rows(0)(2).ToString() <> "TOTAL" Then
            AddHeadTable("PURCHASE", Color.Gainsboro)
            For Each Col As DataColumn In DtPurchase.Columns
                If vDt.Columns.Contains(Col.ColumnName) = False Then Continue For
                DtPurchase.Columns(Col.ColumnName).Caption = vDt.Columns(Col.ColumnName).Caption
            Next
            DtPurchase.pColHeaderVisible = False
            lstSource.Add(DtPurchase)
        End If
    End Sub
    Private Sub GetReturnInfo(ByVal vDt As BrightPosting.GDatatable, ByVal Batchno As String, ByVal Trandate As Date)
        StrSql = " SELECT I.TRANNO,CONVERT(VARCHAR,I.TRANDATE,103) AS TRANDATE"
        StrSql += vbCrLf + " ,ISNULL(SM.SUBITEMNAME,IM.ITEMNAME) AS DESCRIPTION"
        StrSql += vbCrLf + " ,I.TAGNO,I.PCS,I.GRSWT,I.AMOUNT"
        StrSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS I"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = I.ITEMID"
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = I.ITEMID AND SM.SUBITEMID = I.SUBITEMID"
        StrSql += vbCrLf + " WHERE I.TRANDATE = '" & Trandate.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " AND I.BATCHNO = '" & Batchno & "'"
        StrSql += vbCrLf + " AND I.TRANTYPE = 'SR'"
        StrSql += vbCrLf + " AND I.BATCHNO NOT IN (SELECT BATCHNO FROM " & cnAdminDb & "..ORMAST WHERE BATCHNO = '" & Batchno & "')"
        StrSql += vbCrLf + " UNION"
        StrSql += vbCrLf + " SELECT null,null,'TOTAL' DESCRIPTION,null,sum(I.PCS)PCS,sum(I.GRSWT)GRSWT,sum(I.AMOUNT) AMOUNT"
        StrSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS I"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = I.ITEMID"
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = I.ITEMID AND SM.SUBITEMID = I.SUBITEMID"
        StrSql += vbCrLf + " WHERE I.TRANDATE = '" & Trandate.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " AND I.BATCHNO = '" & Batchno & "'"
        StrSql += vbCrLf + " AND I.TRANTYPE = 'SR'"
        StrSql += vbCrLf + " AND I.BATCHNO NOT IN (SELECT BATCHNO FROM " & cnAdminDb & "..ORMAST WHERE BATCHNO = '" & Batchno & "')"
        StrSql += vbCrLf + " order by TRANNO desc"
        Dim DtReturn As New BrightPosting.GDatatable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(DtReturn)
        If DtReturn.Rows.Count > 0 AndAlso DtReturn.Rows(0)(2).ToString() <> "TOTAL" Then
            AddHeadTable("RETURN", Color.Gainsboro)
            For Each Col As DataColumn In DtReturn.Columns
                If vDt.Columns.Contains(Col.ColumnName) = False Then Continue For
                DtReturn.Columns(Col.ColumnName).Caption = vDt.Columns(Col.ColumnName).Caption
            Next
            DtReturn.pColHeaderVisible = False
            lstSource.Add(DtReturn)
        End If
    End Sub
    Private Sub GetSalesInfo(ByVal vDt As BrightPosting.GDatatable, ByVal Batchno As String, ByVal Trandate As Date)
        StrSql = " SELECT I.TRANNO,CONVERT(VARCHAR,I.TRANDATE,103) AS TRANDATE"
        StrSql += vbCrLf + " ,ISNULL(SM.SUBITEMNAME,IM.ITEMNAME) AS DESCRIPTION"
        StrSql += vbCrLf + " ,I.TAGNO,I.PCS,I.GRSWT,(I.AMOUNT+I.TAX) AMOUNT"
        StrSql += vbCrLf + " ,(SELECT PCTFILE FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID = I.ITEMID AND TAGNO = I.TAGNO)AS PCTFILE"
        StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = I.ITEMID"
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = I.ITEMID AND SM.SUBITEMID = I.SUBITEMID"
        StrSql += vbCrLf + " WHERE I.TRANDATE = '" & Trandate.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " AND I.BATCHNO = '" & Batchno & "'"
        StrSql += vbCrLf + " AND I.TRANTYPE = 'SA'"
        StrSql += vbCrLf + " AND I.BATCHNO NOT IN (SELECT BATCHNO FROM " & cnAdminDb & "..ORMAST WHERE BATCHNO = '" & Batchno & "')"
        StrSql += vbCrLf + " UNION"
        StrSql += vbCrLf + " SELECT null,null,'TOTAL' DESCRIPTION,null,sum(I.PCS)PCS,sum(I.GRSWT)GRSWT,sum((I.AMOUNT+I.TAX)) AMOUNT,null"
        StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = I.ITEMID"
        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = I.ITEMID AND SM.SUBITEMID = I.SUBITEMID"
        StrSql += vbCrLf + " WHERE I.TRANDATE = '" & Trandate.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " AND I.BATCHNO = '" & Batchno & "'"
        StrSql += vbCrLf + " AND I.TRANTYPE = 'SA'"
        StrSql += vbCrLf + " AND I.BATCHNO NOT IN (SELECT BATCHNO FROM " & cnAdminDb & "..ORMAST WHERE BATCHNO = '" & Batchno & "')"
        StrSql += vbCrLf + " order by TRANNO desc"
        Dim DtSales As New BrightPosting.GDatatable
        If chkWithTagImage.Checked Then
            DtSales.Columns.Add("TAGIMAGE", GetType(PictureBox))
        End If
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(DtSales)
        Dim picBox As New PictureBox
        If DtSales.Rows.Count > 0 AndAlso DtSales.Rows(0)(2).ToString() <> "TOTAL" Then
            AddHeadTable("SALES", Color.Gainsboro)
            If chkWithTagImage.Checked Then
                For Each Row As BrightPosting.GDataRow In DtSales.Rows
                    Row.pRowHeight = DtSales.pRowHeight
                    If IO.File.Exists(PicPath & Row.Item("PCTFILE").ToString) Then
                        picBox = New PictureBox
                        picBox.Size = New Size(100, 100)
                        AutoImageSizer(PicPath & Row.Item("PCTFILE").ToString, picBox, PictureBoxSizeMode.CenterImage)
                        Row.Item("TAGIMAGE") = picBox
                        Row.pRowHeight = 100
                    End If
                Next
            End If
            For Each Col As DataColumn In DtSales.Columns
                If vDt.Columns.Contains(Col.ColumnName) = False Then Continue For
                DtSales.Columns(Col.ColumnName).Caption = vDt.Columns(Col.ColumnName).Caption
            Next
            DtSales.pColHeaderVisible = False
            lstSource.Add(DtSales)
        End If
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If Dgv.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "CUSTOMER TRANSACTION FLOW", Dgv, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub
    Private Sub Dgv_CellEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Dgv.CellEnter
        dgv2.DataSource = Nothing
        dgv2.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        Me.Refresh()
        Dim dttGrid As New DataTable
        ' If Dgv.CurrentRow.Index = IsNothing() Then Exit SuB

        Dim batchno As String = Dgv.CurrentRow.Cells("BATCHNO").Value.ToString()
        Dim companyid As String = GetSelectedComId(chkcmbCompany, True)
        Dim costid As String = GetSelectedCostId(chkCmbCostCentre, True)
        Dim Trantype As String = CmbOption.Text
        Dim temptable As String = "TEMPTABLEDB..TEMP" & systemId & "CUSTOMERINFO1"

        StrSql = vbCrLf + "  IF OBJECT_ID('" & temptable & "') IS NOT NULL DROP TABLE " & temptable & ""
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()

        StrSql = " ;with cte1 as("
        StrSql += vbCrLf + "  SELECT  C.BATCHNO,I.TRANNO,I.TRANDATE,M.ITEMNAME ,S.SUBITEMNAME "
        StrSql += vbCrLf + "  ,I.PCS,I.GRSWT,I.NETWT,I.AMOUNT,I.TAX GST,(I.AMOUNT+I.TAX) TOTALAMOUNT"
        StrSql += vbCrLf + "  ,E.EMPNAME ,CC.COSTNAME,  "
        StrSql += vbCrLf + "  CASE WHEN TRANTYPE='SA'THEN 'SALES'"
        StrSql += vbCrLf + "  WHEN TRANTYPE ='OD' THEN 'ORDER DELIVERY'"
        StrSql += vbCrLf + "  WHEN TRANTYPE ='RD' THEN 'REPAIR DELIVERY' ELSE TRANTYPE END AS TRANTYPE,C.REMARK1 AS REMARK "
        StrSql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE I"
        StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CUSTOMERINFO C ON C.BATCHNO=I.BATCHNO"
        StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..COSTCENTRE  CC ON CC .COSTID  =I.COSTID "
        StrSql += vbCrLf + "   LEFT JOIN " & cnAdminDb & "..ITEMMAST M ON M.ITEMID =I.ITEMID "
        StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & " ..SUBITEMMAST S ON S.SUBITEMID =I.SUBITEMID "
        StrSql += vbCrLf + "  LEFT  JOIN " & cnAdminDb & "..EMPMASTER E ON I.EMPID =E.EMPID "
        StrSql += " WHERE I.BATCHNO='" & batchno & " '"
        If companyid.Trim <> "" And companyid.Trim <> "ALL" Then StrSql += " AND I.COMPANYID IN(" & companyid & ") "
        StrSql += vbCrLf + "  UNION ALL"
        StrSql += vbCrLf + "  SELECT  C.BATCHNO,I.TRANNO,I.TRANDATE,M.ITEMNAME ,S.SUBITEMNAME "
        StrSql += vbCrLf + "  ,I.PCS,I.GRSWT,I.NETWT,I.AMOUNT,I.TAX GST,(I.AMOUNT+I.TAX) TOTALAMOUNT"
        StrSql += vbCrLf + "  ,E.EMPNAME ,CC.COSTNAME,  "
        StrSql += vbCrLf + "  CASE WHEN TRANTYPE='SR'THEN 'SALES RETURN'"
        StrSql += vbCrLf + "  WHEN TRANTYPE ='PU' THEN 'PURCHASE'"
        StrSql += vbCrLf + "  ELSE TRANTYPE END AS TRANTYPE,C.REMARK1 AS REMARK "
        StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT I"
        StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CUSTOMERINFO C ON C.BATCHNO=I.BATCHNO"
        StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..COSTCENTRE  CC ON CC .COSTID  =I.COSTID "
        StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..ITEMMAST M ON M.ITEMID =I.ITEMID "
        StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & " ..SUBITEMMAST S ON S.SUBITEMID =I.SUBITEMID "
        StrSql += vbCrLf + "  LEFT  JOIN " & cnAdminDb & "..EMPMASTER E ON I.EMPID =E.EMPID "
        StrSql += " WHERE I.BATCHNO='" & batchno & " '"
        If companyid.Trim <> "" And companyid.Trim <> "ALL" Then StrSql += " AND I.COMPANYID IN(" & companyid & ") "
        If commonTranDb.Count > 0 Then
            For i As Integer = 0 To commonTranDb.Count - 1
                StrSql += vbCrLf + "  UNION ALL"
                StrSql += vbCrLf + "  SELECT  C.BATCHNO,I.TRANNO,I.TRANDATE,M.ITEMNAME ,S.SUBITEMNAME "
                StrSql += vbCrLf + "  ,I.PCS,I.GRSWT,I.NETWT,I.AMOUNT,I.TAX GST,(I.AMOUNT+I.TAX) TOTALAMOUNT"
                StrSql += vbCrLf + "  ,E.EMPNAME ,CC.COSTNAME,  "
                StrSql += vbCrLf + "  CASE WHEN TRANTYPE='SA'THEN 'SALES'"
                StrSql += vbCrLf + "  WHEN TRANTYPE ='OD' THEN 'ORDER DELIVERY'"
                StrSql += vbCrLf + "  WHEN TRANTYPE ='RD' THEN 'REPAIR DELIVERY' ELSE TRANTYPE END AS TRANTYPE,C.REMARK1 AS REMARK "
                StrSql += vbCrLf + "  FROM " & commonTranDb(i).ToString & "..ISSUE I"
                StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CUSTOMERINFO C ON C.BATCHNO=I.BATCHNO"
                StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..COSTCENTRE  CC ON CC .COSTID  =I.COSTID "
                StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..ITEMMAST M ON M.ITEMID =I.ITEMID "
                StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & " ..SUBITEMMAST S ON S.SUBITEMID =I.SUBITEMID "
                StrSql += vbCrLf + "  LEFT  JOIN " & cnAdminDb & "..EMPMASTER E ON I.EMPID =E.EMPID "
                StrSql += vbCrLf + "  WHERE I.BATCHNO='" & batchno & " '"
                If companyid.Trim <> "" And companyid.Trim <> "ALL" Then StrSql += " AND I.COMPANYID IN(" & companyid & ") "
                StrSql += vbCrLf + "  UNION ALL"
                StrSql += vbCrLf + "  SELECT  C.BATCHNO,I.TRANNO,I.TRANDATE,M.ITEMNAME ,S.SUBITEMNAME "
                StrSql += vbCrLf + "  ,I.PCS,I.GRSWT,I.NETWT,I.AMOUNT,I.TAX GST,(I.AMOUNT+I.TAX) TOTALAMOUNT"
                StrSql += vbCrLf + "  ,E.EMPNAME ,CC.COSTNAME,  "
                StrSql += vbCrLf + "  CASE WHEN TRANTYPE='SR'THEN 'SALES RETURN'"
                StrSql += vbCrLf + "  WHEN TRANTYPE ='PU' THEN 'PURCHASE'"
                StrSql += vbCrLf + "  ELSE TRANTYPE END AS TRANTYPE,C.REMARK1 AS REMARK "
                StrSql += vbCrLf + "  FROM " & commonTranDb(i).ToString & "..RECEIPT I"
                StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CUSTOMERINFO C ON C.BATCHNO=I.BATCHNO"
                StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..COSTCENTRE  CC ON CC .COSTID  =I.COSTID "
                StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..ITEMMAST M ON M.ITEMID =I.ITEMID "
                StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & " ..SUBITEMMAST S ON S.SUBITEMID =I.SUBITEMID "
                StrSql += vbCrLf + "  LEFT  JOIN " & cnAdminDb & "..EMPMASTER E ON I.EMPID =E.EMPID "
                StrSql += vbCrLf + "  WHERE I.BATCHNO='" & batchno & " '"
                If companyid.Trim <> "" And companyid.Trim <> "ALL" Then StrSql += " AND I.COMPANYID IN(" & companyid & ") "
            Next
        End If
        StrSql += vbCrLf + " ) "
        StrSql += vbCrLf + " ,cte2 as(select *,CASE WHEN TRANTYPE='SALES'THEN 1 WHEN TRANTYPE ='ORDER DELIVERY' THEN 3 WHEN TRANTYPE ='REPAIR DELIVERY' THEN 5 WHEN TRANTYPE='SALES RETURN'THEN 7 WHEN TRANTYPE ='PURCHASE' THEN 9 END RESULT from cte1) "
        StrSql += vbCrLf + " select * from cte2"
        StrSql += vbCrLf + " union "
        StrSql += vbCrLf + " select null,null,null,CASE WHEN RESULT=1 THEN 'TOTAL SALES' WHEN RESULT =3 THEN 'TOTAL ORDER DELIVERY' WHEN RESULT =5 THEN 'TOTAL REPAIR DELIVERY' WHEN RESULT=7 THEN 'TOTAL SALES RETURN' WHEN RESULT = 9 THEN 'TOTAL PURCHASE' end BATCHNO,'',sum(PCS)PCS, sum(GRSWT)GRSWT, sum(NETWT)NETWT, sum(AMOUNT)AMOUNT, sum(GST)GST,  sum(TOTALAMOUNT)TOTALAMOUNT, '', '', '', '', CASE WHEN RESULT=1 THEN 2 WHEN RESULT =3 THEN 4 WHEN RESULT =5 THEN 6 WHEN RESULT=7 THEN 8 WHEN RESULT =9 THEN 10 END RESULT from cte2 group by RESULT order by RESULT "

        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dttGrid)
        dgv2.DataSource = dttGrid
        dgv2.Columns("BATCHNO").Visible = False
        dgv2.Columns("RESULT").Visible = False
        dgv2.Columns("TRANDATE").DefaultCellStyle.Format = "dd-MM-yyyy"
        'dgv2.Columns("TRANNO").Width = 50
        'dgv2.Columns("TRANDATE").Width = 100
        'dgv2.Columns("ITEMNAME").Width = 200
        'dgv2.Columns("SUBITEMNAME").Width = 200
        'dgv2.Columns("EMPNAME").Width = 100
        'dgv2.Columns("COSTNAME").Width = 100
        'dgv2.Columns("PCS").Width = 50
        dgv2.Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgv2.Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgv2.Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgv2.Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgv2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
        dgv2.Invalidate()
        For Each dgvCol As DataGridViewColumn In dgv2.Columns
            dgvCol.Width = dgvCol.Width
        Next
        dgv2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        'GridColumnDivider()
    End Sub
    Private Sub dgv2_CellMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgv2.CellMouseClick
        Dim tempBatch As String = ""
        tempBatch = dgv2.CurrentRow.Cells("BATCHNO").Value.ToString()
        dgv2.DefaultCellStyle.BackColor = Color.WhiteSmoke
        dgv2.DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Regular)
        For Each GV As DataGridViewRow In Dgv.Rows
            With GV
                Select Case .Cells("BATCHNO").Value.ToString()
                    Case tempBatch
                        .DefaultCellStyle.BackColor = Color.Wheat
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Regular)
                    Case Else
                        .DefaultCellStyle.BackColor = Color.White
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Regular)
                End Select
            End With
        Next
    End Sub
    'Function GridColumnDivider()
    '    Grid_Size = 0
    '    Column_Count = 0
    '    With dgv2
    '        Dim Column_Size As Integer
    '        For i As Integer = 0 To .Columns.Count - 1
    '            If .Columns(i).Visible = True Then
    '                Column_Count = Column_Count + 1
    '            End If
    '        Next
    '        .ScrollBars = ScrollBars.None
    '        Grid_Size = .Width - 10
    '        .Columns("TRANNO").Width = Column_Size
    '        Column_Size = GridSizeCalculation(Column_Size, "TRANNO")
    '        .Columns("TRANDATE").Width = Column_Size
    '        Column_Size = GridSizeCalculation(Column_Size, "TRANDATE")
    '        .Columns("ITEMNAME").Width = Column_Size * 2.5
    '        Column_Size = GridSizeCalculation(Column_Size, "ITEMNAME")
    '        .Columns("SUBITEMNAME").Width = Column_Size / 1.25
    '        Column_Size = GridSizeCalculation(Column_Size, "SUBITEMNAME")
    '        .Columns("EMPNAME").Width = Column_Size / 1.25
    '        Column_Size = GridSizeCalculation(Column_Size, "EMPNAME")
    '        .Columns("COSTNAME").Width = Column_Size
    '        Column_Size = GridSizeCalculation(Column_Size, "COSTNAME")
    '    End With
    'End Function
    'Private Function GridSizeCalculation(ByVal size As Integer, ByVal cellname As String) As Integer
    '    With dgv2
    '        If .Columns(cellname).Visible = True Then
    '            Grid_Size = Grid_Size - .Columns(cellname).Width
    '            Column_Count = Column_Count - 1
    '            size = Val(Grid_Size / Column_Count)
    '        End If
    '        Return size
    '    End With
    'End Function

End Class