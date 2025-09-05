Imports System.Data.OleDb
Imports System.IO

Public Class frmSyncStatusChecking
    Dim Cmd As New OleDbCommand
    Dim da As New OleDbDataAdapter
    Dim dt As New DataTable
    Dim Strsql As String
    Dim ToCostId, maincostid As String
    Dim SelCostid As String
    Dim ds As New DataSet
    Dim dbPath As String
    Dim Password As String
    Dim _Cn As New OleDbConnection


    Private Sub frmSyncStatusChecking_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyCode = Keys.Escape Then
            btnback_Click(Me, New EventArgs)
        End If
    End Sub
    Private Sub frmSyncStatusChecking_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Loadcombo()
        LoadSoftcontrol()
        tabmain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabmain.Region = New Region(New RectangleF(Me.GeneralTabPage.Left, Me.GeneralTabPage.Top, Me.GeneralTabPage.Width, Me.GeneralTabPage.Height))
        If maincostid = cnCostId Then
            dtpChkdate.Focus()
        Else
            dtpChkdate.Enabled = False
            dtpChkdate1.Enabled = False
            cmbCostname.Enabled = False
            chkonlydiff.Enabled = False
            btnView.Enabled = False
            btnSend.Enabled = False
            btnupdate.Enabled = False
            clrtable.Enabled = False
        End If
        Strsql = "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostname.Text & "'"
        SelCostid = objGPack.GetSqlValue(Strsql, "COSTID")
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Private Sub LoadSoftcontrol()
        Strsql = "SELECT COSTID  FROM " & cnadmindb & "..SYNCCOSTCENTRE S  WHERE MAIN='Y'"
        dt = New DataTable
        dt = Getdata(Strsql, Cn)
        If dt.Rows.Count > 0 Then
            maincostid = dt.Rows(0).Item("COSTID").ToString
        End If
    End Sub
    Private Sub Loadcombo()
        Strsql = "SELECT (SELECT COSTNAME FROM " & cnadmindb & "..COSTCENTRE WHERE COSTID=S.COSTID)COSTNAME,S.COSTID"
        Strsql += " FROM " & cnAdminDb & "..SYNCCOSTCENTRE S  WHERE MAIN<>'Y'"
        objGPack.FillCombo(Strsql, cmbCostname, True)
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click
        GridaccView.DataSource = Nothing
        If Validation() = "C" Then
            MsgBox("Sorry request was sent to above branch." & vbCrLf & "Pls wait..") : Exit Sub
        ElseIf Validation() = "" Then
            If MsgBox("Data is not available." & vbCrLf & "Are you like to send request..", MsgBoxStyle.OkCancel) = MsgBoxResult.Cancel Then
                Exit Sub
            Else
                btnSend_Click(Me, New EventArgs())
                Exit Sub
            End If
        End If

        GridaccView.DataSource = Nothing
        Dim resdt, adt, rdt, idt As New DataTable
        Dim ro As DataRow
        resdt = New DataTable
        adt = GetaccountEntry()
        rdt = GetReceiptEntry()
        idt = GetIssueEntry()
        resdt.Columns.Add("PARTICULARS", Type.GetType("System.String"))
        resdt.Columns.Add("COSTID", Type.GetType("System.String"))
        resdt.Columns.Add("TRANDATE", Type.GetType("System.String"))
        resdt.Columns.Add("NAME", Type.GetType("System.String"))
        resdt.Columns.Add("BRANCH_CHK_FIELD", Type.GetType("System.Decimal"))
        resdt.Columns.Add("CORP_CHK_FIELD", Type.GetType("System.Decimal"))
        resdt.Columns.Add("DIFF", Type.GetType("System.Decimal"))
        resdt.Columns.Add("COLHEAD", Type.GetType("System.String"))
        resdt.Columns.Add("ACCODE", Type.GetType("System.String"))

        If adt.Rows.Count > 0 Then
            ro = resdt.NewRow()
            ro("PARTICULARS") = "ACCOUNTS"
            ro("NAME") = "ACCOUNT_NAME"
            ro("COLHEAD") = "AH"
            resdt.Rows.Add(ro)
            resdt.Merge(adt)

            ro = resdt.NewRow()
            ro("PARTICULARS") = "ACCOUNT-TOTAL"
            ro("BRANCH_CHK_FIELD") = DBNull.Value  ' adt.Compute("SUM(BRANCH_CHK_FIELD)", "COLHEAD='A'")
            ro("CORP_CHK_FIELD") = DBNull.Value  'adt.Compute("SUM(CORP_CHK_FIELD)", "COLHEAD='A'")
            ro("DIFF") = adt.Compute("SUM(DIFF)", "COLHEAD='A'")
            ro("COLHEAD") = "AT"
            resdt.Rows.Add(ro)
        End If
        If rdt.Rows.Count > 0 Then
            ro = resdt.NewRow()
            ro("PARTICULARS") = "RECEIPT"
            ro("NAME") = "CATEGORY_NAME"
            ro("COLHEAD") = "RH"
            resdt.Rows.Add(ro)
            resdt.Merge(rdt)

            ro = resdt.NewRow()
            ro("PARTICULARS") = "RECEIPT-TOTAL"
            ro("BRANCH_CHK_FIELD") = rdt.Compute("SUM(BRANCH_CHK_FIELD)", "COLHEAD='R'")
            ro("CORP_CHK_FIELD") = rdt.Compute("SUM(CORP_CHK_FIELD)", "COLHEAD='R'")
            ro("DIFF") = rdt.Compute("SUM(DIFF)", "COLHEAD='R'")
            ro("COLHEAD") = "RT"
            resdt.Rows.Add(ro)
        End If
        If idt.Rows.Count > 0 Then
            ro = resdt.NewRow()
            ro("PARTICULARS") = "ISSUE"
            ro("NAME") = "CATEGORY_NAME"
            ro("COLHEAD") = "IH"
            resdt.Rows.Add(ro)
            resdt.Merge(idt)

            ro = resdt.NewRow()
            ro("PARTICULARS") = "ISSUE-TOTAL"
            ro("BRANCH_CHK_FIELD") = idt.Compute("SUM(BRANCH_CHK_FIELD)", "COLHEAD='I'")
            ro("CORP_CHK_FIELD") = idt.Compute("SUM(CORP_CHK_FIELD)", "COLHEAD='I'")
            ro("DIFF") = idt.Compute("SUM(DIFF)", "COLHEAD='I'")
            ro("COLHEAD") = "IT"
            resdt.Rows.Add(ro)
        End If
        If resdt.Rows.Count > 0 Then
            GridaccView.DataSource = resdt
            GridaccView.Columns("COLHEAD").Visible = False
            GridaccView.Columns("ACCODE").Visible = False
            GridaccView.Columns("TRANDATE1").Visible = False
            GridaccView.Columns("PARTICULARS").Width = 125
            GridaccView.Columns("NAME").Width = 400
            GridaccView.Columns("COSTID").Width = 60
            GridaccView.Columns("TRANDATE").Width = 80
            GridaccView.Columns("BRANCH_CHK_FIELD").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            GridaccView.Columns("CORP_CHK_FIELD").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            GridaccView.Columns("DIFF").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            GridaccView.Columns("BRANCH_CHK_FIELD").HeaderText = "BRANCH"
            GridaccView.Columns("CORP_CHK_FIELD").HeaderText = "CORPRATE"
            For i As Integer = 0 To GridaccView.Rows.Count - 1
                With GridaccView.Rows(i)
                    If .Cells("COLHEAD").Value.ToString = "AH" Or .Cells("COLHEAD").Value.ToString = "RH" Or .Cells("COLHEAD").Value.ToString = "IH" Then
                        .DefaultCellStyle.BackColor = Color.LightGreen
                        .DefaultCellStyle.ForeColor = Color.Red
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    End If
                    If .Cells("COLHEAD").Value.ToString = "AT" Or .Cells("COLHEAD").Value.ToString = "RT" Or .Cells("COLHEAD").Value.ToString = "IT" Then
                        .DefaultCellStyle.BackColor = Color.White
                        .DefaultCellStyle.ForeColor = Color.Blue
                        .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    End If
                End With
            Next
            GridaccView.Focus()
        End If
    End Sub
    Function GetaccountEntry() As DataTable
        Strsql = " SELECT CONVERT(VARCHAR(50),'')PARTICULARS,COSTID,CONVERT(VARCHAR(15),TRANDATE,105)TRANDATE,TRANDATE TRANDATE1,A.ACNAME NAME"
        Strsql += vbCrLf + " ,CONVERT(DECIMAL(15,3),SUM(BRANCHAMT))BRANCH_CHK_FIELD,CONVERT(DECIMAL(15,3),SUM(CORPAMT))CORP_CHK_FIELD "
        Strsql += vbCrLf + " ,SUM(ISNULL(BRANCHAMT,0)-ISNULL(CORPAMT,0))DIFF,'A'COLHEAD,X.ACCODE"
        Strsql += vbCrLf + " FROM ("
        Strsql += vbCrLf + " SELECT  COSTID,TRANDATE,SUM(ISNULL(ACCAMT,0))BRANCHAMT,ISNULL(ACCCODE,'')ACCODE"
        Strsql += vbCrLf + " ,0.00 CORPAMT"
        Strsql += vbCrLf + " FROM " & cnStockDb & "..CHECKTABLE_JEW  WHERE ISNULL(FLAG,'')='A' AND COSTID='" & SelCostid & "' AND TRANDATE BETWEEN '" & Format(dtpChkdate.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpChkdate1.Value, "yyyy-MM-dd") & "'"
        Strsql += vbCrLf + " GROUP BY COSTID,TRANDATE,ACCCODE"
        Strsql += vbCrLf + " UNION ALL"
        Strsql += vbCrLf + " SELECT  COSTID,TRANDATE,0 BRANCHAMT,ACCODE"
        Strsql += vbCrLf + " ,SUM(CASE WHEN TRANMODE='D' THEN AMOUNT ELSE (AMOUNT)*-1 END)CORPAMT"
        Strsql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN  WHERE  COSTID='" & SelCostid & "' AND TRANDATE BETWEEN '" & Format(dtpChkdate.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpChkdate1.Value, "yyyy-MM-dd") & "'"
        Strsql += vbCrLf + " GROUP BY COSTID,TRANDATE,ACCODE"
        Strsql += vbCrLf + " )X "
        Strsql += vbCrLf + " LEFT JOIN " & cnadmindb & "..ACHEAD A ON X.ACCODE=A.ACCODE"
        Strsql += vbCrLf + " GROUP BY COSTID,TRANDATE,ACNAME,X.ACCODE"
        If chkonlydiff.Checked Then Strsql += vbCrLf + " HAVING SUM(ISNULL(BRANCHAMT,0)-ISNULL(CORPAMT,0))<>0 "
        Strsql += vbCrLf + " ORDER BY COSTID,TRANDATE1,ACNAME"
        dt = Getdata(Strsql, Cn)
        Return dt
    End Function
    Function GetIssueEntry() As DataTable
        Strsql = "  SELECT COSTID,CONVERT(VARCHAR(15),TRANDATE,105)TRANDATE,TRANDATE TRANDATE1,C.CATNAME NAME"
        Strsql += vbCrLf + "  ,CONVERT(DECIMAL(15,3),SUM(BRANCH_WT))BRANCH_CHK_FIELD,CONVERT(DECIMAL(15,3),SUM(CORP_WT))CORP_CHK_FIELD "
        Strsql += vbCrLf + "  ,SUM(ISNULL(BRANCH_WT,0)-ISNULL(CORP_WT,0))DIFF,'I'COLHEAD,X.ACCODE"
        Strsql += vbCrLf + "  FROM ("
        Strsql += vbCrLf + "  SELECT  COSTID,TRANDATE,SUM(ISNULL(ISSGRSWT,0))BRANCH_WT,ISNULL(ISSCATCODE,'')ACCODE"
        Strsql += vbCrLf + "  ,0.00 CORP_WT"
        Strsql += vbCrLf + "  FROM " & cnStockDb & "..CHECKTABLE_JEW  WHERE ISNULL(FLAG,'')='I' AND COSTID='" & SelCostid & "' AND TRANDATE BETWEEN '" & Format(dtpChkdate.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpChkdate1.Value, "yyyy-MM-dd") & "'"
        Strsql += vbCrLf + "  GROUP BY COSTID,TRANDATE,ISSCATCODE"
        Strsql += vbCrLf + "  UNION ALL"
        Strsql += vbCrLf + "  SELECT  COSTID,TRANDATE,0 BRANCH_WT,CATCODE ACCODE"
        Strsql += vbCrLf + "  ,SUM(ISNULL(GRSWT,0))CORP_WT"
        Strsql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE  WHERE  COSTID='" & SelCostid & "' AND TRANDATE BETWEEN '" & Format(dtpChkdate.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpChkdate1.Value, "yyyy-MM-dd") & "'"
        Strsql += vbCrLf + "  GROUP BY COSTID,TRANDATE,CATCODE"
        Strsql += vbCrLf + "  )X "
        Strsql += vbCrLf + "  LEFT JOIN " & cnadmindb & "..CATEGORY C ON X.ACCODE=C.CATCODE"
        Strsql += vbCrLf + "  GROUP BY COSTID,TRANDATE,CATNAME,X.ACCODE"
        If chkonlydiff.Checked Then Strsql += vbCrLf + " HAVING SUM(ISNULL(BRANCH_WT,0)-ISNULL(CORP_WT,0))<>0 "
        Strsql += vbCrLf + " ORDER BY COSTID,TRANDATE1,NAME"
        dt = Getdata(Strsql, Cn)
        Return dt
    End Function
    Function GetReceiptEntry() As DataTable
        Strsql = "  SELECT COSTID,CONVERT(VARCHAR(15),TRANDATE,105)TRANDATE,TRANDATE TRANDATE1,C.CATNAME NAME"
        Strsql += vbCrLf + "  ,CONVERT(DECIMAL(15,3),SUM(BRANCH_WT))BRANCH_CHK_FIELD,CONVERT(DECIMAL(15,3),SUM(CORP_WT))CORP_CHK_FIELD "
        Strsql += vbCrLf + "  ,SUM(ISNULL(BRANCH_WT,0)-ISNULL(CORP_WT,0))DIFF,'R'COLHEAD,X.ACCODE"
        Strsql += vbCrLf + "  FROM ("
        Strsql += vbCrLf + "  SELECT  COSTID,TRANDATE,SUM(ISNULL(RECGRSWT,0))BRANCH_WT,ISNULL(RECCATCODE,'')ACCODE"
        Strsql += vbCrLf + "  ,0.00 CORP_WT"
        Strsql += vbCrLf + "  FROM " & cnStockDb & "..CHECKTABLE_JEW  WHERE ISNULL(FLAG,'')='R' AND COSTID='" & SelCostid & "' AND TRANDATE BETWEEN '" & Format(dtpChkdate.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpChkdate1.Value, "yyyy-MM-dd") & "'"
        Strsql += vbCrLf + "  GROUP BY COSTID,TRANDATE,RECCATCODE"
        Strsql += vbCrLf + "  UNION ALL"
        Strsql += vbCrLf + "  SELECT  COSTID,TRANDATE,0 BRANCH_WT,CATCODE ACCODE"
        Strsql += vbCrLf + "  ,SUM(ISNULL(GRSWT,0))CORP_WT"
        Strsql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT  WHERE  COSTID='" & SelCostid & "' AND TRANDATE BETWEEN '" & Format(dtpChkdate.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpChkdate1.Value, "yyyy-MM-dd") & "'"
        Strsql += vbCrLf + "  GROUP BY COSTID,TRANDATE,CATCODE"
        Strsql += vbCrLf + "  )X "
        Strsql += vbCrLf + "  LEFT JOIN " & cnadmindb & "..CATEGORY C ON X.ACCODE=C.CATCODE"
        Strsql += vbCrLf + "  GROUP BY COSTID,TRANDATE,CATNAME,X.ACCODE"
        If chkonlydiff.Checked Then Strsql += vbCrLf + " HAVING SUM(ISNULL(BRANCH_WT,0)-ISNULL(CORP_WT,0))<>0 "
        Strsql += vbCrLf + " ORDER BY COSTID,TRANDATE1,NAME"
        dt = Getdata(Strsql, Cn)
        Return dt
    End Function

    Function Getdata(ByVal qry As String, ByVal cn As OleDbConnection) As DataTable
        dt = New DataTable
        Dim dtt As New DataTable
        da = New OleDbDataAdapter(qry, cn)
        da.Fill(dtt)
        Return dtt
    End Function
    Function Validation() As String
        Dim vdt As New DataTable
        Strsql = "SELECT ISNULL(FLAG,'')FLAG FROM " & cnStockDb & "..CHECKTRAN WHERE CHKDATE='" & Format(dtpChkdate.Value, "yyyy-MM-dd") & "' AND COSTID='" & SelCostid & "'"
        vdt = Getdata(Strsql, Cn)
        If vdt.Rows.Count > 0 Then
            Return vdt.Rows(0).Item(0).ToString
        Else
            Return ""
        End If
    End Function
    Private Sub btnSend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSend.Click
        If cmbCostname.Text <> "" And cmbCostname.Text <> "ALL" Then
            If Validation() = "C" Then MsgBox("Already send the request to the selected costcentre.") : Exit Sub
            If Validation() = "F" Then
                If MsgBox("Already received data from above costcentre." & vbCrLf & "Do you like to send request again?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Exit Sub
                Strsql = "DELETE FROM " & cnStockDb & "..CHECKTRAN WHERE CHKDATE='" & Format(dtpChkdate.Value, "yyyy-MM-dd") & "' AND CHKDATE1='" & Format(dtpChkdate1.Value, "yyyy-MM-dd") & "'"
                Strsql += " AND COSTID='" & SelCostid & "'"
                Cmd = New OleDbCommand(Strsql, Cn)
                Cmd.ExecuteNonQuery()
            End If
            If maincostid = cnCostId Then
                Strsql = "INSERT INTO " & cnStockDb & "..CHECKTRAN(CHKDATE,CHKDATE1,FLAG,COSTID)"
                Strsql += " VALUES('" & Format(dtpChkdate.Value, "yyyy-MM-dd") & "','" & Format(dtpChkdate1.Value, "yyyy-MM-dd") & "','C','" & SelCostid & "')"
                Cmd = New OleDbCommand(Strsql, Cn)
                Cmd.ExecuteNonQuery()
            End If
            Strsql = "INSERT INTO " & Mid(cnstockdb, 1, 3) & "UTILDB..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS)"
            Strsql += "SELECT '" & cnCostId & "','" & SelCostid & "','INSERT INTO " & cnStockDb & "..CHECKTRAN(CHKDATE,CHKDATE1,FLAG,COSTID) VALUES (''" & Format(dtpChkdate.Value, "yyyy-MM-dd") & "'',''" & Format(dtpChkdate1.Value, "yyyy-MM-dd") & "'',''C'',''" & SelCostid & "'')','N'"
            Cmd = New OleDbCommand(Strsql, Cn)
            Cmd.ExecuteNonQuery()
            MsgBox("Request Sending... ", MsgBoxStyle.Information)

        ElseIf cmbCostname.Text = "ALL" Then

            Strsql = "SELECT COSTID FROM " & cnadmindb & "..SYNCCOSTCENTRE WHERE MAIN<>'Y'"
            da = New OleDbDataAdapter(Strsql, Cn)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    With dt.Rows(i)
                        If Validation() = "C" Then Continue For
                        Strsql = "INSERT INTO " & Mid(cnstockdb, 1, 3) & "UTILDB..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS)"
                        Strsql += "SELECT '" & cnCostId & "','" & .Item("COSTID").ToString & "','INSERT INTO " & cnstockdb & "..CHECKTRAN VALUES (''" & Format(dtpChkdate.Value, "yyyy-MM-dd") & "'')','N'"
                        Cmd = New OleDbCommand(Strsql, Cn)
                        Cmd.ExecuteNonQuery()
                    End With
                Next
                MsgBox("Request Sending...", MsgBoxStyle.Information)
            End If
        End If
    End Sub

    Private Sub btnupdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnupdate.Click
        If GridaccView.Rows.Count <= 0 Then Exit Sub
        If GridaccView.CurrentRow Is Nothing Then Exit Sub
        'GridaccView.DataSource = Nothing
        If Validation() = "CC" Or Validation() = "C" Then
            MsgBox("Already send the request to the selected costcentre.") : Exit Sub
        ElseIf Validation() = "" Then
            If MsgBox("Initial Data is not available." & vbCrLf & "Are you like to send Initial request..", MsgBoxStyle.OkCancel) = MsgBoxResult.Cancel Then Exit Sub Else btnSend_Click(Me, New EventArgs())
        End If
        Strsql = " UPDATE " & cnStockDb & "..CHECKTRAN SET FLAG='CC' WHERE CHKDATE='" & Format(dtpChkdate.Value, "yyyy-MM-dd") & "' AND CHKDATE1='" & Format(dtpChkdate1.Value, "yyyy-MM-dd") & "' AND COSTID='" & SelCostid & "' AND ISNULL(FLAG,'')='C'"
        Cmd = New OleDbCommand(Strsql, Cn) : Cmd.ExecuteNonQuery()
        Dim gdt As New DataTable
        Dim dv As New DataView
        dt = New DataTable
        dt = CType(GridaccView.DataSource, DataTable)
        dv = dt.DefaultView
        If GridaccView.CurrentRow.Cells("COLHEAD").Value.ToString = "A" Then
            dv.RowFilter = "COLHEAD ='A' AND ISNULL(DIFF,0)<>0 AND ISNULL(ACCODE,'')<>'' AND ISNULL(ACCODE,'')='" & GridaccView.CurrentRow.Cells("ACCODE").Value.ToString & "' AND ISNULL(TRANDATE1,'')='" & GridaccView.CurrentRow.Cells("TRANDATE1").Value.ToString & "'"
        ElseIf GridaccView.CurrentRow.Cells("COLHEAD").Value.ToString = "R" Then
            dv.RowFilter = "COLHEAD ='R'AND ISNULL(DIFF,0)<>0 AND ISNULL(ACCODE,'')<>'' AND ISNULL(ACCODE,'')='" & GridaccView.CurrentRow.Cells("ACCODE").Value.ToString & "' AND ISNULL(TRANDATE1,'')='" & GridaccView.CurrentRow.Cells("TRANDATE1").Value.ToString & "'"
        ElseIf GridaccView.CurrentRow.Cells("COLHEAD").Value.ToString = "I" Then
            dv.RowFilter = "COLHEAD ='I' AND ISNULL(DIFF,0)<>0 AND ISNULL(ACCODE,'')<>'' AND ISNULL(ACCODE,'')='" & GridaccView.CurrentRow.Cells("ACCODE").Value.ToString & "' AND ISNULL(TRANDATE1,'')='" & GridaccView.CurrentRow.Cells("TRANDATE1").Value.ToString & "'"
        Else
            Exit Sub
        End If
        gdt = dv.ToTable
        If gdt.Rows.Count > 0 Then
            For i As Integer = 0 To gdt.Rows.Count - 1
                With gdt.Rows(i)
                    Strsql = " INSERT INTO " & Mid(cnStockDb, 1, 3) & "UTILDB..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS)"
                    Strsql += " SELECT '" & cnCostId & "','" & .Item("COSTID").ToString & "','INSERT INTO " & cnStockDb & "..CHECKTRAN(CHKDATE,CHKDATE1,FLAG,CHK_DATA) VALUES (''" & .Item("TRANDATE1").ToString & "'',''" & .Item("TRANDATE1").ToString & "'',''" & .Item("COLHEAD").ToString & "'',''" & .Item("ACCODE").ToString & "'')','N'"
                    Cmd = New OleDbCommand(Strsql, cn) : Cmd.ExecuteNonQuery()
                End With
            Next
        End If
    End Sub

    Private Sub btntriggercreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btntriggercreate.Click
        If MsgBox("Are you sure to Create tables..", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Exit Sub
        Dim _ConInfo As BrighttechPack.Coninfo
        _ConInfo = New BrighttechPack.Coninfo(Application.StartupPath + "\ConInfo.ini")
        Password = _ConInfo.lDbPwd
        If passWord <> "" Then passWord = BrighttechPack.Methods.Decrypt(passWord)
        If _ConInfo.lDbLoginType.ToUpper = "W" Then
            _Cn = New OleDbConnection("Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=" & cnStockDb & ";Data Source=" & _ConInfo.lServerName & "")
        Else
            _Cn = New OleDbConnection(String.Format("PROVIDER = SQLOLEDB.1;Persist Security Info=False; Initial Catalog=" & cnStockDb & ";Data Source={0};User Id=" & IIf(_ConInfo.lDbUserId <> "", _ConInfo.lDbUserId, "SA") & ";password=" & Password & ";", _ConInfo.lServerName))
        End If

        Strsql = " IF OBJECT_ID('" & cnStockDb & "..CHECKTRAN')IS NOT NULL DROP TABLE " & cnStockDb & "..CHECKTRAN"
        Strsql += vbCrLf + "CREATE TABLE " & cnStockDb & "..CHECKTRAN (COSTID VARCHAR(2),CHKDATE SMALLDATETIME,CHKDATE1 SMALLDATETIME ,CHK_DATA VARCHAR(15),FLAG VARCHAR(3)) "
        Cmd = New OleDbCommand(Strsql, Cn)
        Cmd.ExecuteNonQuery()

        If maincostid = cnCostId Then
            Strsql = " IF OBJECT_ID('" & cnstockdb & "..CHK_ISSUE')IS NOT NULL DROP TABLE " & cnstockdb & "..CHK_ISSUE"
            Strsql += vbCrLf + " IF OBJECT_ID('" & cnstockdb & "..CHK_RECEIPT')IS NOT NULL DROP TABLE " & cnstockdb & "..CHK_RECEIPT"
            Strsql += vbCrLf + " IF OBJECT_ID('" & cnstockdb & "..CHK_ACCTRAN')IS NOT NULL DROP TABLE " & cnstockdb & "..CHK_ACCTRAN"
            Strsql += vbCrLf + " SELECT * INTO " & cnstockdb & "..CHK_ISSUE FROM " & cnstockdb & "..ISSUE WHERE 1<>1 "
            Strsql += vbCrLf + " SELECT * INTO " & cnstockdb & "..CHK_RECEIPT FROM " & cnstockdb & "..RECEIPT WHERE 1<>1 "
            Strsql += vbCrLf + " SELECT * INTO " & cnstockdb & "..CHK_ACCTRAN FROM " & cnstockdb & "..ACCTRAN WHERE 1<>1 "

            Cmd = New OleDbCommand(Strsql, Cn)
            Cmd.ExecuteNonQuery()

            Strsql = " IF OBJECT_ID('" & cnStockDb & "..CHECKTABLE_JEW')IS NOT NULL DROP TABLE " & cnStockDb & "..CHECKTABLE_JEW"
            Strsql += vbCrLf + " SELECT COSTID,TRANDATE,GRSWT RECGRSWT,CATCODE RECCATCODE,GRSWT ISSGRSWT,CATCODE ISSCATCODE"
            Strsql += vbCrLf + " ,AMOUNT ACCAMT,ACCODE ACCCODE,AMOUNT OUTAMT,ACCODE OUTACCODE,''FLAG "
            Strsql += vbCrLf + " INTO " & cnstockdb & "..CHECKTABLE_JEW FROM " & cnstockdb & "..RECEIPT WHERE 1<>1"

            Cmd = New OleDbCommand(Strsql, Cn)
            Cmd.ExecuteNonQuery()
        Else
            If _Cn.State = ConnectionState.Closed Then _Cn.Open()
            Strsql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME='TRIG_CHECKTRAN')>0"
            Strsql += vbCrLf + "  	DROP TRIGGER TRIG_CHECKTRAN"
            Cmd = New OleDbCommand(Strsql, _Cn)
            Cmd.ExecuteNonQuery()
            Strsql = vbCrLf + " CREATE TRIGGER TRIG_CHECKTRAN ON  CHECKTRAN FOR INSERT            "
            Strsql += vbCrLf + " AS            "
            Strsql += vbCrLf + " BEGIN            "
            Strsql += vbCrLf + "  DECLARE @QRY NVARCHAR(4000)            "
            Strsql += vbCrLf + "  DECLARE @FLAG VARCHAR(3)            "
            Strsql += vbCrLf + "  DECLARE @CHKDATE SMALLDATETIME            "
            Strsql += vbCrLf + "  DECLARE @CHKDATE1 SMALLDATETIME"
            Strsql += vbCrLf + "  DECLARE @TRANDATE SMALLDATETIME"
            Strsql += vbCrLf + "  DECLARE @CHKDATA VARCHAR(20)        "
            Strsql += vbCrLf + "  DECLARE @COSTID VARCHAR(2)            "
            Strsql += vbCrLf + "  DECLARE @TOCOSTID VARCHAR(2)            "
            Strsql += vbCrLf + "  DECLARE @MAINCOSTID VARCHAR(2)  "
            Strsql += vbCrLf + "  DECLARE @RECGRSWT VARCHAR(20)        "
            Strsql += vbCrLf + "  DECLARE @RECCATCODE VARCHAR(20)        "
            Strsql += vbCrLf + "  DECLARE @ISSGRSWT VARCHAR(20)        "
            Strsql += vbCrLf + "  DECLARE @ISSCATCODE VARCHAR(20)        "
            Strsql += vbCrLf + "  DECLARE @ACCAMT VARCHAR(20)          "
            Strsql += vbCrLf + "  DECLARE @ACCCODE VARCHAR(15)           "
            Strsql += vbCrLf + "  DECLARE @OUTAMT VARCHAR(20)          "
            Strsql += vbCrLf + "  DECLARE @OUTACCODE VARCHAR(15)             "
            Strsql += vbCrLf + "  SELECT @COSTID =CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID ='COSTID'            "
            Strsql += vbCrLf + "  SELECT @TOCOSTID =CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID ='SYNC-TO'            "
            Strsql += vbCrLf + "  SELECT @MAINCOSTID=COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE MAIN='Y'  "
            Strsql += vbCrLf + "  SELECT @CHKDATE=ISNULL(CHKDATE,'') FROM INSERTED             "
            Strsql += vbCrLf + "  SELECT @CHKDATE1=ISNULL(CHKDATE1,'') FROM INSERTED             "
            Strsql += vbCrLf + "  SELECT @CHKDATA=ISNULL(CHK_DATA,'') FROM INSERTED           "
            Strsql += vbCrLf + "  SELECT @FLAG=ISNULL(FLAG,'') FROM INSERTED          "
            Strsql += vbCrLf + "  SET @QRY=''    "

            Strsql += vbCrLf + "  IF @TOCOSTID<>'' AND @COSTID <>'' AND (@MAINCOSTID<>@COSTID)  "
            Strsql += vbCrLf + "  BEGIN            "
            Strsql += vbCrLf + "   IF @FLAG='C'       "
            Strsql += vbCrLf + "    BEGIN      "
            Strsql += vbCrLf + "     INSERT INTO  " & Mid(cnStockDb, 1, 3) & "UTILDB..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS)        "
            Strsql += vbCrLf + "     SELECT ''+ @COSTID +'',''+ @TOCOSTID +'','DELETE FROM " & cnStockDb & "..CHECKTABLE_JEW WHERE COSTID='''+ @COSTID +''' ' ,'N'         "
            Strsql += vbCrLf + "     /**RECEIPT**/"
            Strsql += vbCrLf + "     DECLARE CUR CURSOR FOR        "
            Strsql += vbCrLf + "     SELECT SUM(GRSWT)GRSWT,CATCODE,TRANDATE FROM RECEIPT WHERE TRANDATE BETWEEN @CHKDATE AND @CHKDATE1 AND COSTID=@COSTID GROUP BY CATCODE,TRANDATE        "
            Strsql += vbCrLf + "     OPEN CUR FETCH NEXT FROM CUR INTO @RECGRSWT,@RECCATCODE,@TRANDATE        "
            Strsql += vbCrLf + "     WHILE @@FETCH_STATUS=0  "
            Strsql += vbCrLf + "     BEGIN         "
            Strsql += vbCrLf + "     INSERT INTO  " & Mid(cnStockDb, 1, 3) & "UTILDB..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS)        "
            Strsql += vbCrLf + "     SELECT ''+ @COSTID +'',''+ @TOCOSTID +'','INSERT INTO " & cnStockDb & "..CHECKTABLE_JEW(COSTID,TRANDATE,RECGRSWT,RECCATCODE,FLAG)         "
            Strsql += vbCrLf + "     SELECT '''+ @COSTID +''','''+ CONVERT(NVARCHAR,@TRANDATE,111) +'''        "
            Strsql += vbCrLf + "     ,'''+ @RECGRSWT  +''','''+ @RECCATCODE +''',''R''' ,'N'        "
            Strsql += vbCrLf + "     FETCH NEXT FROM CUR INTO @RECGRSWT,@RECCATCODE,@TRANDATE "
            Strsql += vbCrLf + "     END "
            Strsql += vbCrLf + "     CLOSE CUR  "
            Strsql += vbCrLf + "     DEALLOCATE CUR         "
            Strsql += vbCrLf + "     /**ISSUE**/"
            Strsql += vbCrLf + "     DECLARE CUR CURSOR FOR        "
            Strsql += vbCrLf + "     SELECT SUM(GRSWT)GRSWT,CATCODE,TRANDATE FROM ISSUE WHERE TRANDATE BETWEEN @CHKDATE AND @CHKDATE1 AND COSTID=@COSTID GROUP BY TRANDATE,CATCODE,TRANDATE        "
            Strsql += vbCrLf + "     OPEN CUR FETCH NEXT FROM CUR INTO @ISSGRSWT,@ISSCATCODE,@TRANDATE        "
            Strsql += vbCrLf + "     WHILE @@FETCH_STATUS=0  "
            Strsql += vbCrLf + "     BEGIN         "
            Strsql += vbCrLf + "     INSERT INTO  " & Mid(cnStockDb, 1, 3) & "UTILDB..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS)        "
            Strsql += vbCrLf + "     SELECT ''+ @COSTID +'',''+ @TOCOSTID +'','INSERT INTO " & cnStockDb & "..CHECKTABLE_JEW(COSTID,TRANDATE,ISSGRSWT,ISSCATCODE,FLAG)         "
            Strsql += vbCrLf + "     SELECT '''+ @COSTID +''','''+ CONVERT(NVARCHAR,@TRANDATE,111) +'''        "
            Strsql += vbCrLf + "     ,'''+ @ISSGRSWT  +''','''+ @ISSCATCODE +''',''I''' ,'N'        "
            Strsql += vbCrLf + "     FETCH NEXT FROM CUR INTO @ISSGRSWT,@ISSCATCODE,@TRANDATE "
            Strsql += vbCrLf + "     END "
            Strsql += vbCrLf + "     CLOSE CUR  "
            Strsql += vbCrLf + "     DEALLOCATE CUR         "
            Strsql += vbCrLf + "     /**ACCTRAN**/"
            Strsql += vbCrLf + "     DECLARE CUR1 CURSOR FOR        "
            Strsql += vbCrLf + "     SELECT SUM(CASE WHEN TRANMODE='D' THEN AMOUNT ELSE (AMOUNT)*-1 END)AMOUNT,ACCODE,TRANDATE FROM ACCTRAN WHERE         "
            Strsql += vbCrLf + "     TRANDATE BETWEEN @CHKDATE AND @CHKDATE1 AND COSTID=@COSTID  GROUP BY TRANDATE,ACCODE        "
            Strsql += vbCrLf + "     OPEN CUR1 FETCH NEXT FROM CUR1 INTO @ACCAMT,@ACCCODE,@TRANDATE        "
            Strsql += vbCrLf + "     WHILE @@FETCH_STATUS=0  "
            Strsql += vbCrLf + "     BEGIN         "
            Strsql += vbCrLf + "     INSERT INTO  " & Mid(cnStockDb, 1, 3) & "UTILDB..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS)        "
            Strsql += vbCrLf + "     SELECT ''+ @COSTID +'',''+ @TOCOSTID +'','INSERT INTO " & cnStockDb & "..CHECKTABLE_JEW(COSTID,TRANDATE,ACCAMT,ACCCODE,FLAG)         "
            Strsql += vbCrLf + "     SELECT '''+ @COSTID +''','''+ CONVERT(NVARCHAR,@TRANDATE,111) +'''        "
            Strsql += vbCrLf + "     ,'''+ @ACCAMT  +''','''+ @ACCCODE +''',''A''' ,'N'       "
            Strsql += vbCrLf + "     FETCH NEXT FROM CUR1 INTO @ACCAMT,@ACCCODE,@TRANDATE "
            Strsql += vbCrLf + "     END CLOSE CUR1  "
            Strsql += vbCrLf + "     DEALLOCATE CUR1         "
            Strsql += vbCrLf + "     INSERT INTO  " & Mid(cnStockDb, 1, 3) & "UTILDB..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS)        "
            Strsql += vbCrLf + "     SELECT ''+ @COSTID +'',''+ @TOCOSTID +'','UPDATE " & cnStockDb & "..CHECKTRAN SET FLAG=''F'' WHERE COSTID='''+ @COSTID +'''  AND CHKDATE='''+ CONVERT(NVARCHAR,@CHKDATE,111) +''' AND CHKDATE1='''+ CONVERT(NVARCHAR,@CHKDATE1,111) +'''' ,'N'         "
            Strsql += vbCrLf + "    END      "
            Strsql += vbCrLf + "    IF @FLAG='A'      "
            Strsql += vbCrLf + "    BEGIN      "
            Strsql += vbCrLf + "     INSERT INTO  " & Mid(cnStockDb, 1, 3) & "UTILDB..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS)        "
            Strsql += vbCrLf + "     SELECT ''+ @COSTID +'',''+ @TOCOSTID +'','DELETE FROM " & cnStockDb & "..CHK_ACCTRAN WHERE COSTID='''+ @COSTID +'''  AND TRANDATE BETWEEN '''+ CONVERT(NVARCHAR,@CHKDATE,111) +''' AND '''+ CONVERT(NVARCHAR,@CHKDATE1,111) +''' AND ACCODE='''+  @CHKDATA +''' ' ,'N'             "
            Strsql += vbCrLf + "     IF OBJECT_ID('TEMP_CHECKTRAN_ACCTRAN') IS NOT NULL DROP TABLE TEMP_CHECKTRAN_ACCTRAN      "
            Strsql += vbCrLf + "     SELECT * INTO TEMP_CHECKTRAN_ACCTRAN FROM ACCTRAN WHERE TRANDATE BETWEEN ''+ CONVERT(NVARCHAR,@CHKDATE,111) +'' AND ''+ CONVERT(NVARCHAR,@CHKDATE1,111) +'' AND COSTID=''+ @COSTID +'' AND  ACCODE IN(''+ @CHKDATA +'')      "
            Strsql += vbCrLf + "     EXEC INSERTQRYGENERATOR_TABLENEW @DBNAME='" & cnStockDb & "',@TABLENAME='TEMP_CHECKTRAN_ACCTRAN',@MASK_TABLENAME='CHK_ACCTRAN' ,@TEMPTABLE='TEMP_CHECKTRAN_ACCTRAN_RES'       "
            Strsql += vbCrLf + "     DECLARE CUR1 CURSOR FOR        "
            Strsql += vbCrLf + "     SELECT SQLTEXT FROM TEMP_CHECKTRAN_ACCTRAN_RES      "
            Strsql += vbCrLf + "     OPEN CUR1 FETCH NEXT FROM CUR1 INTO @QRY      "
            Strsql += vbCrLf + "     WHILE @@FETCH_STATUS=0  BEGIN         "

            Strsql += vbCrLf + "     INSERT INTO  " & Mid(cnStockDb, 1, 3) & "UTILDB..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS)        "
            Strsql += vbCrLf + "     SELECT ''+ @COSTID +'',''+ @TOCOSTID +'',''+ @QRY +'','N'       "
            Strsql += vbCrLf + "     print @QRY    "
            Strsql += vbCrLf + "     FETCH NEXT FROM CUR1 INTO @QRY END CLOSE CUR1  DEALLOCATE CUR1         "
            Strsql += vbCrLf + "     IF OBJECT_ID('TEMP_CHECKTRAN_ACCTRAN') IS NOT NULL DROP TABLE TEMP_CHECKTRAN_ACCTRAN      "
            Strsql += vbCrLf + "     IF OBJECT_ID('TEMP_CHECKTRAN_ACCTRAN_RES') IS NOT NULL DROP TABLE TEMP_CHECKTRAN_ACCTRAN_RES      "
            Strsql += vbCrLf + "    END      "
            Strsql += vbCrLf + "    IF @FLAG='R'      "
            Strsql += vbCrLf + "    BEGIN      "
            Strsql += vbCrLf + "     INSERT INTO  " & Mid(cnStockDb, 1, 3) & "UTILDB..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS)        "
            Strsql += vbCrLf + "     SELECT ''+ @COSTID +'',''+ @TOCOSTID +'','DELETE FROM " & cnStockDb & "..CHK_RECEIPT WHERE COSTID='''+ @COSTID +'''  AND TRANDATE BETWEEN '''+ CONVERT(NVARCHAR,@CHKDATE,111) +''' AND '''+ CONVERT(NVARCHAR,@CHKDATE1,111) +'''AND CATCODE='''+  @CHKDATA +''' ' ,'N'             "
            Strsql += vbCrLf + "     IF OBJECT_ID('TEMP_CHECKTRAN_RECEIPT') IS NOT NULL DROP TABLE TEMP_CHECKTRAN_RECEIPT      "
            Strsql += vbCrLf + "     SELECT * INTO TEMP_CHECKTRAN_RECEIPT FROM RECEIPT WHERE TRANDATE BETWEEN ''+ CONVERT(NVARCHAR,@CHKDATE,111) +'' AND ''+ CONVERT(NVARCHAR,@CHKDATE1,111) +'' AND COSTID=''+ @COSTID +'' AND  CATCODE IN(''+ @CHKDATA +'')      "
            Strsql += vbCrLf + "     EXEC INSERTQRYGENERATOR_TABLENEW @DBNAME='" & cnStockDb & "',@TABLENAME='TEMP_CHECKTRAN_RECEIPT',@MASK_TABLENAME='CHK_RECEIPT' ,@TEMPTABLE='TEMP_CHECKTRAN_RECEIPT_RES'       "
            Strsql += vbCrLf + "     DECLARE CUR1 CURSOR FOR        "
            Strsql += vbCrLf + "     SELECT SQLTEXT FROM TEMP_CHECKTRAN_RECEIPT_RES      "
            Strsql += vbCrLf + "     OPEN CUR1 FETCH NEXT FROM CUR1 INTO @QRY      "
            Strsql += vbCrLf + "     WHILE @@FETCH_STATUS=0  BEGIN         "
            Strsql += vbCrLf + "     INSERT INTO  " & Mid(cnStockDb, 1, 3) & "UTILDB..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS)        "
            Strsql += vbCrLf + "     SELECT ''+ @COSTID +'',''+ @TOCOSTID +'',''+ @QRY +'','N'       "
            Strsql += vbCrLf + "     FETCH NEXT FROM CUR1 INTO @QRY END CLOSE CUR1  DEALLOCATE CUR1         "
            Strsql += vbCrLf + "     IF OBJECT_ID('TEMP_CHECKTRAN_RECEIPT') IS NOT NULL DROP TABLE TEMP_CHECKTRAN_RECEIPT      "
            Strsql += vbCrLf + "     IF OBJECT_ID('TEMP_CHECKTRAN_RECEIPT_RES') IS NOT NULL DROP TABLE TEMP_CHECKTRAN_RECEIPT_RES      "
            Strsql += vbCrLf + "    END      "

            Strsql += vbCrLf + "   IF @FLAG='I'      "
            Strsql += vbCrLf + "    BEGIN      "
            Strsql += vbCrLf + "     INSERT INTO  " & Mid(cnStockDb, 1, 3) & "UTILDB..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS)        "
            Strsql += vbCrLf + "     SELECT ''+ @COSTID +'',''+ @TOCOSTID +'','DELETE FROM " & cnStockDb & "..CHK_ISSUE WHERE COSTID='''+ @COSTID +'''  AND TRANDATE BETWEEN '''+ CONVERT(NVARCHAR,@CHKDATE,111) +''' AND '''+ CONVERT(NVARCHAR,@CHKDATE1,111) +''' AND CATCODE='''+  @CHKDATA +''' ' ,'N'             "
            Strsql += vbCrLf + "     IF OBJECT_ID('TEMP_CHECKTRAN_ISSUE') IS NOT NULL DROP TABLE TEMP_CHECKTRAN_ISSUE      "
            Strsql += vbCrLf + "     SELECT * INTO TEMP_CHECKTRAN_ISSUE FROM ISSUE WHERE TRANDATE BETWEEN ''+ CONVERT(NVARCHAR,@CHKDATE,111) +'' AND ''+ CONVERT(NVARCHAR,@CHKDATE1,111) +'' AND COSTID=''+ @COSTID +'' AND  CATCODE IN(''+ @CHKDATA +'')      "
            Strsql += vbCrLf + "     EXEC INSERTQRYGENERATOR_TABLENEW @DBNAME='" & cnStockDb & "',@TABLENAME='TEMP_CHECKTRAN_ISSUE',@MASK_TABLENAME='CHK_ISSUE' ,@TEMPTABLE='TEMP_CHECKTRAN_ISSUE_RES'       "
            Strsql += vbCrLf + "     DECLARE CUR1 CURSOR FOR        "
            Strsql += vbCrLf + "     SELECT SQLTEXT FROM TEMP_CHECKTRAN_ISSUE_RES      "
            Strsql += vbCrLf + "     OPEN CUR1 FETCH NEXT FROM CUR1 INTO @QRY      "
            Strsql += vbCrLf + "     WHILE @@FETCH_STATUS=0  BEGIN         "
            Strsql += vbCrLf + "     INSERT INTO  " & Mid(cnStockDb, 1, 3) & "UTILDB..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS)        "
            Strsql += vbCrLf + "     SELECT ''+ @COSTID +'',''+ @TOCOSTID +'',''+ @QRY +'','N'       "
            Strsql += vbCrLf + "     FETCH NEXT FROM CUR1 INTO @QRY END CLOSE CUR1  DEALLOCATE CUR1         "
            Strsql += vbCrLf + "     IF OBJECT_ID('TEMP_CHECKTRAN_ISSUE') IS NOT NULL DROP TABLE TEMP_CHECKTRAN_ISSUE      "
            Strsql += vbCrLf + "     IF OBJECT_ID('TEMP_CHECKTRAN_ISSUE_RES') IS NOT NULL DROP TABLE TEMP_CHECKTRAN_ISSUE_RES      "
            Strsql += vbCrLf + "    END      "
            Strsql += vbCrLf + "  END      "
            Strsql += vbCrLf + "  IF @MAINCOSTID<>@COSTID DELETE FROM CHECKTRAN"
            Strsql += vbCrLf + " END    "
            Cmd = New OleDbCommand(Strsql, _Cn)
            Cmd.ExecuteNonQuery()
            If _Cn.State = ConnectionState.Open Then _Cn.Close()
        End If
        MsgBox("Check table was created...")
    End Sub

    Private Sub clrtable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clrtable.Click
        Strsql = " DELETE FROM " & cnstockdb & "..CHK_RECEIPT"
        Strsql += vbCrLf + " DELETE FROM " & cnstockdb & "..CHK_ISSUE"
        Strsql += vbCrLf + " DELETE FROM " & cnstockdb & "..CHK_ACCTRAN"
        Cmd = New OleDbCommand(Strsql, Cn)
        Cmd.ExecuteNonQuery()
        MsgBox("Check tables was deleted...")
    End Sub

    Private Sub GridaccView_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles GridaccView.KeyDown
        If GridaccView.Rows.Count <= 0 Then Exit Sub
        If e.KeyCode = Keys.D Then
            If GridaccView.CurrentRow.Cells("COLHEAD").Value.ToString = "A" Then
                ds.Clear()
                ds = AccountsDetail()
                If ds.Tables("CORP").Rows.Count > 0 Then
                    Gridcorp.DataSource = ds.Tables("CORP")
                    Gridcorp.Columns("CANCEL").Visible = False
                    Gridcorp.Columns("TRANNO").Width = 50
                    Gridcorp.Columns("TRANMODE").Width = 50
                    Gridcorp.Columns("TRANDATE").Width = 75
                    Gridcorp.Columns("ACNAME").Width = 150
                    Gridcorp.Columns("TRANMODE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    Gridcorp.Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                Else
                    Gridcorp.DataSource = Nothing
                End If
                If ds.Tables("BRANCH").Rows.Count > 0 Then
                    GridBranch.DataSource = ds.Tables("BRANCH")
                    GridBranch.Columns("CANCEL").Visible = False
                    GridBranch.Columns("TRANNO").Width = 50
                    GridBranch.Columns("ACNAME").Width = 150
                    GridBranch.Columns("TRANMODE").Width = 50
                    GridBranch.Columns("TRANDATE").Width = 75
                    GridBranch.Columns("TRANMODE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    GridBranch.Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                Else : GridBranch.DataSource = Nothing
                    MsgBox("No Records to View.." & vbCrLf & "Pls do Synchronization..")
                    Exit Sub
                End If
                lblcorp.Text = "ACCOUNTS DETAILS OF CORP"
                lblbranch.Text = "ACCOUNTS DETAILS OF " & UCase(GridaccView.CurrentRow.Cells("COSTID").Value.ToString) & " BRANCH"
                tabmain.SelectedTab = DetailTabPage
            ElseIf GridaccView.CurrentRow.Cells("COLHEAD").Value.ToString = "I" Then
                ds.Clear()
                ds = IssueDetail()
                If ds.Tables("CORP").Rows.Count > 0 Then
                    Gridcorp.DataSource = ds.Tables("CORP")
                    Gridcorp.Columns("CANCEL").Visible = False
                    Gridcorp.Columns("TRANNO").Width = 50
                    Gridcorp.Columns("TRANDATE").Width = 75
                    Gridcorp.Columns("CATNAME").Width = 150
                    Gridcorp.Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                Else
                    Gridcorp.DataSource = Nothing
                End If
                If ds.Tables("BRANCH").Rows.Count > 0 Then
                    GridBranch.DataSource = ds.Tables("BRANCH")
                    GridBranch.Columns("CANCEL").Visible = False
                    GridBranch.Columns("TRANNO").Width = 50
                    GridBranch.Columns("CATNAME").Width = 150
                    GridBranch.Columns("TRANDATE").Width = 75
                    GridBranch.Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                Else : GridBranch.DataSource = Nothing
                    MsgBox("No Records to View.." & vbCrLf & "Pls do Synchronization..")
                    Exit Sub
                End If
                lblcorp.Text = "ISSUE DETAILS OF CORP"
                lblbranch.Text = "ISSUE DETAILS OF " & UCase(GridaccView.CurrentRow.Cells("COSTID").Value.ToString) & " BRANCH"
                tabmain.SelectedTab = DetailTabPage
            ElseIf GridaccView.CurrentRow.Cells("COLHEAD").Value.ToString = "R" Then
                ds.Clear()
                ds = ReceiptDetail()
                If ds.Tables("CORP").Rows.Count > 0 Then
                    Gridcorp.DataSource = ds.Tables("CORP")
                    Gridcorp.Columns("CANCEL").Visible = False
                    Gridcorp.Columns("TRANNO").Width = 50
                    Gridcorp.Columns("TRANDATE").Width = 75
                    Gridcorp.Columns("CATNAME").Width = 150
                    Gridcorp.Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                Else
                    Gridcorp.DataSource = Nothing
                End If
                If ds.Tables("BRANCH").Rows.Count > 0 Then
                    GridBranch.DataSource = ds.Tables("BRANCH")
                    GridBranch.Columns("CANCEL").Visible = False
                    GridBranch.Columns("TRANNO").Width = 50
                    GridBranch.Columns("CATNAME").Width = 150
                    GridBranch.Columns("TRANDATE").Width = 75
                    GridBranch.Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                Else : GridBranch.DataSource = Nothing
                    MsgBox("No Records to View.." & vbCrLf & "Pls do Synchronization..")
                    Exit Sub
                End If
                lblcorp.Text = "RECEIPT DETAILS OF CORP"
                lblbranch.Text = "RECEIPT DETAILS OF " & UCase(GridaccView.CurrentRow.Cells("COSTID").Value.ToString) & " BRANCH"
                tabmain.SelectedTab = DetailTabPage
            End If
        End If
    End Sub
    Function AccountsDetail() As DataSet
        Dim dss As New DataSet
        Strsql = "SELECT BATCHNO,AA.ACNAME,CONVERT(VARCHAR(13),TRANDATE,105)TRANDATE,TRANNO,TRANMODE,SUM(AMOUNT)AMOUNT,CANCEL FROM " & cnstockdb & "..ACCTRAN A"
        Strsql += vbCrLf + " LEFT JOIN " & cnadmindb & "..ACHEAD AS AA ON AA.ACCODE=A.ACCODE WHERE 1=1 "
        Strsql += vbCrLf + " AND A.ACCODE IN(SELECT ACCODE FROM " & cnstockdb & "..CHK_ACCTRAN WHERE "
        Strsql += vbCrLf + " TRANDATE='" & Format(GridaccView.CurrentRow.Cells("TRANDATE1").Value, "yyyy/MM/dd") & "'"
        Strsql += vbCrLf + " AND COSTID='" & GridaccView.CurrentRow.Cells("COSTID").Value.ToString & "')"
        Strsql += vbCrLf + " AND TRANDATE='" & Format(GridaccView.CurrentRow.Cells("TRANDATE1").Value, "yyyy/MM/dd") & "'"
        Strsql += vbCrLf + " AND COSTID='" & GridaccView.CurrentRow.Cells("COSTID").Value.ToString & "'"
        Strsql += vbCrLf + " GROUP BY TRANDATE,AA.ACNAME,TRANNO,BATCHNO,TRANMODE,CANCEL"
        Strsql += vbCrLf + " ORDER BY TRANDATE,TRANNO,BATCHNO,AA.ACNAME,TRANMODE,CANCEL"
        da = New OleDbDataAdapter(Strsql, Cn)
        da.Fill(dss, "CORP")
        Strsql = "SELECT BATCHNO,AA.ACNAME,CONVERT(VARCHAR(13),TRANDATE,105)TRANDATE,TRANNO,TRANMODE,SUM(AMOUNT)AMOUNT,CANCEL FROM " & cnstockdb & "..CHK_ACCTRAN A"
        Strsql += vbCrLf + " LEFT JOIN " & cnadmindb & "..ACHEAD AS AA ON AA.ACCODE=A.ACCODE WHERE 1=1 "
        Strsql += vbCrLf + " AND TRANDATE='" & Format(GridaccView.CurrentRow.Cells("TRANDATE1").Value, "yyyy/MM/dd") & "'"
        Strsql += vbCrLf + " AND COSTID='" & GridaccView.CurrentRow.Cells("COSTID").Value.ToString & "'"
        Strsql += vbCrLf + " GROUP BY TRANDATE,AA.ACNAME,TRANNO,BATCHNO,TRANMODE,CANCEL"
        Strsql += vbCrLf + " ORDER BY TRANDATE,TRANNO,BATCHNO,AA.ACNAME,TRANMODE,CANCEL"
        da = New OleDbDataAdapter(Strsql, Cn)
        ds = New DataSet
        da.Fill(dss, "BRANCH")
        Return dss
    End Function
    Function IssueDetail() As DataSet
        Dim dss As New DataSet
        Strsql = "SELECT BATCHNO,AA.CATNAME,CONVERT(VARCHAR(13),TRANDATE,105)TRANDATE,TRANNO,SUM(GRSWT)AMOUNT,CANCEL FROM " & cnStockDb & "..ISSUE A"
        Strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY AS AA ON AA.CATCODE=A.CATCODE WHERE 1=1 "
        Strsql += vbCrLf + " AND A.CATCODE IN(SELECT CATCODE FROM " & cnStockDb & "..CHK_ISSUE WHERE "
        Strsql += vbCrLf + " TRANDATE='" & Format(GridaccView.CurrentRow.Cells("TRANDATE1").Value, "yyyy/MM/dd") & "'"
        Strsql += vbCrLf + " AND COSTID='" & GridaccView.CurrentRow.Cells("COSTID").Value.ToString & "')"
        Strsql += vbCrLf + " AND TRANDATE='" & Format(GridaccView.CurrentRow.Cells("TRANDATE1").Value, "yyyy/MM/dd") & "'"
        Strsql += vbCrLf + " AND COSTID='" & GridaccView.CurrentRow.Cells("COSTID").Value.ToString & "'"
        Strsql += vbCrLf + " GROUP BY TRANDATE,AA.CATNAME,TRANNO,BATCHNO,CANCEL"
        Strsql += vbCrLf + " ORDER BY TRANDATE,TRANNO,BATCHNO,AA.CATNAME,CANCEL"
        da = New OleDbDataAdapter(Strsql, cn)
        da.Fill(dss, "CORP")
        Strsql = "SELECT BATCHNO,AA.CATNAME,CONVERT(VARCHAR(13),TRANDATE,105)TRANDATE,TRANNO,SUM(GRSWT)AMOUNT,CANCEL FROM " & cnStockDb & "..CHK_ISSUE A"
        Strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY AS AA ON AA.CATCODE=A.CATCODE WHERE 1=1 "
        Strsql += vbCrLf + " AND TRANDATE='" & Format(GridaccView.CurrentRow.Cells("TRANDATE1").Value, "yyyy/MM/dd") & "'"
        Strsql += vbCrLf + " AND COSTID='" & GridaccView.CurrentRow.Cells("COSTID").Value.ToString & "'"
        Strsql += vbCrLf + " GROUP BY TRANDATE,AA.CATNAME,TRANNO,BATCHNO,CANCEL"
        Strsql += vbCrLf + " ORDER BY TRANDATE,TRANNO,BATCHNO,AA.CATNAME,CANCEL"
        da = New OleDbDataAdapter(Strsql, cn)
        ds = New DataSet
        da.Fill(dss, "BRANCH")
        Return dss
    End Function
    Function ReceiptDetail() As DataSet
        Dim dss As New DataSet
        Strsql = "SELECT BATCHNO,AA.CATNAME,CONVERT(VARCHAR(13),TRANDATE,105)TRANDATE,TRANNO,SUM(GRSWT)AMOUNT,CANCEL FROM " & cnStockDb & "..RECEIPT A"
        Strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY AS AA ON AA.CATCODE=A.CATCODE WHERE 1=1 "
        Strsql += vbCrLf + " AND A.CATCODE IN(SELECT CATCODE FROM " & cnStockDb & "..CHK_RECEIPT WHERE "
        Strsql += vbCrLf + " TRANDATE='" & Format(GridaccView.CurrentRow.Cells("TRANDATE1").Value, "yyyy/MM/dd") & "'"
        Strsql += vbCrLf + " AND COSTID='" & GridaccView.CurrentRow.Cells("COSTID").Value.ToString & "')"
        Strsql += vbCrLf + " AND TRANDATE='" & Format(GridaccView.CurrentRow.Cells("TRANDATE1").Value, "yyyy/MM/dd") & "'"
        Strsql += vbCrLf + " AND COSTID='" & GridaccView.CurrentRow.Cells("COSTID").Value.ToString & "'"
        Strsql += vbCrLf + " GROUP BY TRANDATE,AA.CATNAME,TRANNO,BATCHNO,CANCEL"
        Strsql += vbCrLf + " ORDER BY TRANDATE,TRANNO,BATCHNO,AA.CATNAME,CANCEL"
        da = New OleDbDataAdapter(Strsql, cn)
        da.Fill(dss, "CORP")
        Strsql = "SELECT BATCHNO,AA.CATNAME,CONVERT(VARCHAR(13),TRANDATE,105)TRANDATE,TRANNO,SUM(GRSWT)AMOUNT,CANCEL FROM " & cnStockDb & "..CHK_RECEIPT A"
        Strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY AS AA ON AA.CATCODE=A.CATCODE WHERE 1=1 "
        Strsql += vbCrLf + " AND TRANDATE='" & Format(GridaccView.CurrentRow.Cells("TRANDATE1").Value, "yyyy/MM/dd") & "'"
        Strsql += vbCrLf + " AND COSTID='" & GridaccView.CurrentRow.Cells("COSTID").Value.ToString & "'"
        Strsql += vbCrLf + " GROUP BY TRANDATE,AA.CATNAME,TRANNO,BATCHNO,CANCEL"
        Strsql += vbCrLf + " ORDER BY TRANDATE,TRANNO,BATCHNO,AA.CATNAME,CANCEL"
        da = New OleDbDataAdapter(Strsql, cn)
        ds = New DataSet
        da.Fill(dss, "BRANCH")
        Return dss
    End Function

    Private Sub btnfinupdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnfinupdate.Click
        If MsgBox("Are you sure to update..", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Exit Sub
        If GridBranch.Rows.Count > 0 Then
            Try
                tran = Nothing
                tran = cn.BeginTransaction
                If lblcorp.Text.StartsWith("ACCOUNTS") Then
                    Strsql = " DELETE FROM " & cnStockDb & "..ACCTRAN "
                    Strsql += vbCrLf + " WHERE ACCODE IN(SELECT ACCODE FROM " & cnStockDb & "..CHK_ACCTRAN WHERE "
                    Strsql += vbCrLf + " TRANDATE='" & Format(GridaccView.CurrentRow.Cells("TRANDATE1").Value, "yyyy/MM/dd") & "'"
                    Strsql += vbCrLf + " AND COSTID='" & GridaccView.CurrentRow.Cells("COSTID").Value.ToString & "')"
                    Strsql += vbCrLf + " AND TRANDATE='" & Format(GridaccView.CurrentRow.Cells("TRANDATE1").Value, "yyyy/MM/dd") & "'"
                    Strsql += vbCrLf + " AND COSTID='" & GridaccView.CurrentRow.Cells("COSTID").Value.ToString & "'"
                    Strsql += vbCrLf + " AND TRANNO<>'9999'"
                    Cmd = New OleDbCommand(Strsql, cn, tran)
                    Cmd.ExecuteNonQuery()

                    Strsql = " INSERT INTO " & cnStockDb & "..ACCTRAN "
                    Strsql += vbCrLf + "SELECT * FROM " & cnStockDb & "..CHK_ACCTRAN "
                    Strsql += vbCrLf + " TRANDATE='" & Format(GridaccView.CurrentRow.Cells("TRANDATE1").Value, "yyyy/MM/dd") & "'"
                    Strsql += vbCrLf + " AND COSTID='" & GridaccView.CurrentRow.Cells("COSTID").Value.ToString & "')"
                    Strsql += vbCrLf + " AND TRANDATE='" & Format(GridaccView.CurrentRow.Cells("TRANDATE1").Value, "yyyy/MM/dd") & "'"
                    Strsql += vbCrLf + " AND COSTID='" & GridaccView.CurrentRow.Cells("COSTID").Value.ToString & "'"
                    Strsql += vbCrLf + " AND TRANNO<>'9999'"
                    Cmd = New OleDbCommand(Strsql, cn, tran)
                    Cmd.ExecuteNonQuery()
                ElseIf lblcorp.Text.StartsWith("ISSUE") Then
                    Strsql = " DELETE FROM " & cnStockDb & "..ISSUE "
                    Strsql += vbCrLf + " WHERE CATCODE IN(SELECT CATCODE FROM " & cnStockDb & "..CHK_ISSUE WHERE "
                    Strsql += vbCrLf + " TRANDATE='" & Format(GridaccView.CurrentRow.Cells("TRANDATE1").Value, "yyyy/MM/dd") & "'"
                    Strsql += vbCrLf + " AND COSTID='" & GridaccView.CurrentRow.Cells("COSTID").Value.ToString & "')"
                    Strsql += vbCrLf + " AND TRANDATE='" & Format(GridaccView.CurrentRow.Cells("TRANDATE1").Value, "yyyy/MM/dd") & "'"
                    Strsql += vbCrLf + " AND COSTID='" & GridaccView.CurrentRow.Cells("COSTID").Value.ToString & "'"
                    Cmd = New OleDbCommand(Strsql, cn, tran)
                    Cmd.ExecuteNonQuery()

                    Strsql = " INSERT INTO " & cnStockDb & "..ISSUE "
                    Strsql += vbCrLf + "SELECT * FROM " & cnStockDb & "..CHK_ISSUE "
                    Strsql += vbCrLf + " WHERE CATCODE IN(SELECT CATCODE FROM " & cnStockDb & "..CHK_ISSUE WHERE "
                    Strsql += vbCrLf + " TRANDATE='" & Format(GridaccView.CurrentRow.Cells("TRANDATE1").Value, "yyyy/MM/dd") & "'"
                    Strsql += vbCrLf + " AND COSTID='" & GridaccView.CurrentRow.Cells("COSTID").Value.ToString & "')"
                    Strsql += vbCrLf + " AND TRANDATE='" & Format(GridaccView.CurrentRow.Cells("TRANDATE1").Value, "yyyy/MM/dd") & "'"
                    Strsql += vbCrLf + " AND COSTID='" & GridaccView.CurrentRow.Cells("COSTID").Value.ToString & "'"
                    Cmd = New OleDbCommand(Strsql, cn, tran)
                    Cmd.ExecuteNonQuery()
                ElseIf lblcorp.Text.StartsWith("RECEIPT") Then
                    Strsql = " DELETE FROM " & cnStockDb & "..RECEIPT "
                    Strsql += vbCrLf + " WHERE CATCODE IN(SELECT CATCODE FROM " & cnStockDb & "..CHK_RECEIPT  "
                    Strsql += vbCrLf + " WHERE TRANDATE='" & Format(GridaccView.CurrentRow.Cells("TRANDATE1").Value, "yyyy/MM/dd") & "'"
                    Strsql += vbCrLf + " AND COSTID='" & GridaccView.CurrentRow.Cells("COSTID").Value.ToString & "')"
                    Strsql += vbCrLf + " AND TRANDATE='" & Format(GridaccView.CurrentRow.Cells("TRANDATE1").Value, "yyyy/MM/dd") & "'"
                    Strsql += vbCrLf + " AND COSTID='" & GridaccView.CurrentRow.Cells("COSTID").Value.ToString & "'"
                    Cmd = New OleDbCommand(Strsql, cn, tran)
                    Cmd.ExecuteNonQuery()

                    Strsql = " INSERT INTO " & cnStockDb & "..RECEIPT "
                    Strsql += vbCrLf + "SELECT * FROM " & cnStockDb & "..CHK_RECEIPT "
                    Strsql += vbCrLf + " WHERE CATCODE IN(SELECT CATCODE FROM " & cnStockDb & "..CHK_RECEIPT  "
                    Strsql += vbCrLf + " WHERE TRANDATE='" & Format(GridaccView.CurrentRow.Cells("TRANDATE1").Value, "yyyy/MM/dd") & "'"
                    Strsql += vbCrLf + " AND COSTID='" & GridaccView.CurrentRow.Cells("COSTID").Value.ToString & "')"
                    Strsql += vbCrLf + " AND TRANDATE='" & Format(GridaccView.CurrentRow.Cells("TRANDATE1").Value, "yyyy/MM/dd") & "'"
                    Strsql += vbCrLf + " AND COSTID='" & GridaccView.CurrentRow.Cells("COSTID").Value.ToString & "'"
                    Cmd = New OleDbCommand(Strsql, cn, tran)
                    Cmd.ExecuteNonQuery()
                End If
            Catch ex As Exception
                If Not tran Is Nothing Then
                    tran.Rollback()
                    tran = Nothing
                End If
            End Try
            If Not tran Is Nothing Then tran.Commit()
            tran = Nothing
        End If
    End Sub

    Private Sub btnback_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnback.Click
        tabmain.SelectedTab = GeneralTabPage
    End Sub

    Private Sub cmbCostname_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCostname.Leave
        If cmbCostname.Text <> "" Then
            Strsql = "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostname.Text & "'"
            SelCostid = objGPack.GetSqlValue(Strsql, "COSTID")
        End If
    End Sub
End Class


