Imports System.Data.OleDb
Imports System.Data
Public Class frmStockReorderBulk
    Dim subitemid_code As String
    Dim sizeid_code As String

#Region "USER DEFINE"

    Private Sub AutoResize(ByVal gridview As DataGridView)
        If gridview.RowCount > 0 Then
            gridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            gridview.Invalidate()
            For Each dgvCol As DataGridViewColumn In gridview.Columns
                dgvCol.Width = dgvCol.Width
            Next
            gridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End If
    End Sub

    Function funcLoadDesigner() As Integer
        strsql = "select 0 DESIGNERID, 'ALL' DESIGNERNAME"
        strsql += " UNION ALL"
        strsql += " select DESIGNERID,DESIGNERNAME from " & cnAdminDb & "..DESIGNER WHERE ISNULL(ACTIVE , '') ='Y'  "
        strsql += " order by DESIGNERID"
        dt = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dt)
        cmbDesignerName_OWN.DataSource = Nothing
        cmbDesignerName_OWN.DataSource = dt
        cmbDesignerName_OWN.DisplayMember = "DESIGNERNAME"
        cmbDesignerName_OWN.ValueMember = "DESIGNERID"
    End Function
    Function funcLoadSizeName() As Integer
        strsql = "select 0 sizeid, 'ALL' SIZENAME"
        strsql += " UNION ALL"
        strsql += " select SIZEID,SIZENAME from " & cnAdminDb & "..ITEMSIZE "
        strsql += " order by sizeid"
        dt = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dt)
        cmbItemSize_OWN.DataSource = Nothing
        cmbItemSize_OWN.DataSource = dt
        cmbItemSize_OWN.DisplayMember = "SIZENAME"
        cmbItemSize_OWN.ValueMember = "SIZEID"
    End Function
    Function funcLoadItemName(ByVal MetalId As String) As Integer
        strsql = " select ITEMID,ITEMNAME from " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ACTIVE , '') ='Y'"
        If MetalId <> "" And MetalId <> "System.Data.DataRowView" Then strsql += " AND METALID = '" & MetalId & "'"
        strsql += " order by ITEMNAME"
        dt = New DataTable
        da = New OleDbDataAdapter(strsql, cn)


        da.Fill(dt)
        cmbItemName_OWN.DataSource = Nothing
        cmbItemName_OWN.DataSource = dt
        cmbItemName_OWN.DisplayMember = "ITEMNAME"
        cmbItemName_OWN.ValueMember = "ITEMID"
    End Function
    Function funcLoadSubItemName(ByVal ItemId As String) As Integer
        strsql = "select 0 SUBITEMID, 'ALL' SUBITEMNAME"
        strsql += " UNION ALL"
        strsql += " SELECT SUBITEMID,SUBITEMNAME from " & cnAdminDb & "..SUBITEMMAST WHERE ISNULL(ACTIVE , '') ='Y' "
        If ItemId <> "" And ItemId <> "System.Data.DataRowView" Then strsql += " AND ITEMID in ( " & ItemId & ")"
        strsql += " ORDER BY SUBITEMNAME"
        dt = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dt)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbsubitem, dt, "SUBITEMNAME", , "")
        chkcmbsubitem.Text = "ALL"
    End Function
    Function funcLoadMetal() As Integer
        strsql = " select METALNAME,METALID from " & cnAdminDb & "..METALMAST  WHERE ISNULL(ACTIVE , '') ='Y' order by DISPLAYORDER"
        dt = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dt)
        txtItemId.Text = ""
        If cmbMetal_Own.Items.Count = 0 Then
            cmbMetal_Own.Items.Clear()
            cmbMetal_Own.DataSource = Nothing
        End If
        cmbMetal_Own.DataSource = dt


        cmbMetal_Own.DisplayMember = "METALNAME"
        cmbMetal_Own.ValueMember = "METALID"
        txtItemId.Text = ""
    End Function
    Function funcLoadCostCentre() As Integer
        If GetAdmindbSoftValue("COSTCENTRE", "N") = "Y" Then
            cmbCostCentre_own.Enabled = True
            strsql = " select COSTID,CostName from " & cnAdminDb & "..CostCentre  WHERE ISNULL(ACTIVE , '') ='Y' order by CostName"
        Else
            strsql = " select '' COSTID,'ALL' CostName "
        End If
        dt = New DataTable
        da = New OleDbDataAdapter(strsql, cn)

        da.Fill(dt)
        cmbCostCentre_own.DataSource = Nothing
        cmbCostCentre_own.DataSource = dt
        cmbCostCentre_own.DisplayMember = "CostName"
        cmbCostCentre_own.ValueMember = "COSTID"
        If GetAdmindbSoftValue("COSTCENTRE", "N") = "Y" Then
            cmbCostCentre_own.Text = GetSqlValue(cn, "SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & cnCostId & "'").ToString
        End If
    End Function
#End Region
#Region "VARIABLE"
    Dim strsql As String = ""
    Dim dt As DataTable
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim _tran As OleDbTransaction = Nothing
#End Region
#Region "FORM LOAD EVENTS"
    Private Sub frmStockReorderBulk_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        funcLoadSizeName()
        funcLoadItemName("")
        funcLoadMetal()
        funcLoadCostCentre()
        funcLoadDesigner()
        cmbOrderBy.Items.Clear()
        cmbOrderBy.Items.Add("NONE")
        cmbOrderBy.Items.Add("SUBITEMID")
        cmbOrderBy.Items.Add("SUBITEMNAME")
        cmbOrderBy.Text = "SUBITEMNAME"
        cmbMetal_Own.Focus()
        cmbMetal_Own.Select()
        btnNew_Click(Me, New System.EventArgs)
    End Sub
    Private Sub frmStockReorderBulk_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
#End Region
#Region "EVENTS "
    Private Sub txtItemId_Leave(sender As Object, e As EventArgs) Handles txtItemId.Leave
        If txtItemId.Text <> "" Or Val(txtItemId.Text) > 0 Then

            Dim dr As DataRow = Nothing
            strsql = " SELECT ITEMNAME"
            strsql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = I.METALID) METALNAME"
            strsql += vbCrLf + " ,ISNULL(SIZESTOCK,'') SIZESTOCK "
            strsql += vbCrLf + " FROM " & cnAdminDb & "..ITEMMAST AS I"
            strsql += vbCrLf + " WHERE ITEMID = '" & txtItemId.Text & "'"
            dr = GetSqlRow(strsql, cn)
            If Not dr Is Nothing Then
                cmbItemName_OWN.Text = dr.Item("ITEMNAME").ToString
                cmbMetal_Own.Text = dr.Item("METALNAME").ToString
                cmbItemSize_OWN.DataSource = Nothing
                If dr.Item("SIZESTOCK") = "Y" Then
                    Dim dtItemsize As New DataTable
                    strsql = " SELECT 0 SIZEID, 'ALL' SIZENAME"
                    strsql += " UNION ALL "
                    strsql += " SELECT SIZEID,SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE ITEMID = " & Val(txtItemId.Text) & " "
                    strsql += " ORDER BY SIZEID"
                    da = New OleDbDataAdapter(strsql, cn)
                    da.Fill(dtItemsize)
                    If dtItemsize.Rows.Count > 0 Then
                        cmbItemSize_OWN.DataSource = Nothing
                        cmbItemSize_OWN.DataSource = dtItemsize
                        cmbItemSize_OWN.ValueMember = "SIZEID"
                        cmbItemSize_OWN.DisplayMember = "SIZENAME"
                    End If
                End If
                funcLoadSubItemName(Val(txtItemId.Text))
            Else
                MsgBox("Item Id not found in master", MsgBoxStyle.Information)
                txtItemId.Focus()
                txtItemId.SelectAll()
                Exit Sub
            End If
        End If
    End Sub
    Function Getsubitemid()
        subitemid_code = ""
        If chkcmbsubitem.Text <> "ALL" And chkcmbsubitem.Text <> "" Then
            Dim sql As String = "SELECT SUBITEMID,SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME IN (" & GetQryString(chkcmbsubitem.Text) & ") And ISNULL(ACTIVE,'')= 'Y'   ORDER BY SUBITEMID"
            Dim dtsub As New DataTable()
            da = New OleDbDataAdapter(sql, cn)
            da.Fill(dtsub)
            If dtsub.Rows.Count > 0 Then
                For i As Integer = 0 To dtsub.Rows.Count - 1
                    subitemid_code += dtsub.Rows(i).Item("SUBITEMID").ToString + ","
                Next
                If subitemid_code <> "" Then
                    subitemid_code = Mid(subitemid_code, 1, subitemid_code.Length - 1)
                End If
            End If
        ElseIf chkcmbsubitem.Text = "ALL" Then
            Dim sql As String = "SELECT SUBITEMID,SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = '" & txtItemId.Text & "' And ISNULL(ACTIVE,'')= 'Y'   ORDER BY SUBITEMID"
            Dim dtsub As New DataTable()
            da = New OleDbDataAdapter(sql, cn)
            da.Fill(dtsub)
            If dtsub.Rows.Count > 0 Then
                For i As Integer = 0 To dtsub.Rows.Count - 1
                    subitemid_code += dtsub.Rows(i).Item("SUBITEMID").ToString + ","
                Next
                If subitemid_code <> "" Then
                    subitemid_code = Mid(subitemid_code, 1, subitemid_code.Length - 1)
                End If
            End If
        Else
            subitemid_code = 0
        End If
    End Function
    Function GetSizeid()
        sizeid_code = ""
        If cmbItemSize_OWN.Text <> "ALL" And cmbItemSize_OWN.Text <> "" Then
            Dim sql As String = "SELECT SIZEID,SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZENAME IN ('" & cmbItemSize_OWN.Text & "') AND ITEMID = '" & txtItemId.Text & "' "
            sizeid_code = Val(GetSqlValue(cn, sql))
        ElseIf cmbItemSize_OWN.Text = "ALL" Then
            Dim sql As String = "SELECT SIZEID,SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE ITEMID = '" & txtItemId.Text & "' ORDER BY SIZENAME"
            Dim dtsub As New DataTable()
            da = New OleDbDataAdapter(sql, cn)
            da.Fill(dtsub)
            If dtsub.Rows.Count > 0 Then
                For i As Integer = 0 To dtsub.Rows.Count - 1
                    sizeid_code += dtsub.Rows(i).Item("SIZEID").ToString + ","
                Next
                If sizeid_code <> "" Then
                    sizeid_code = Mid(sizeid_code, 1, sizeid_code.Length - 1)
                End If
            Else
                sizeid_code = 0
            End If
        Else
            sizeid_code = 0
        End If
    End Function
    Private Sub btnView_Click(sender As Object, e As EventArgs) Handles btnView.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        If txtItemId.Text.Trim = "" Then MsgBox("Enter the ItemId ", MsgBoxStyle.Information) : txtItemId.Focus() : gridView_OWN.DataSource = Nothing : Exit Sub
        If chkcmbsubitem.Text.Trim = "" Then MsgBox("Check the SubItemName ", MsgBoxStyle.Information) : txtItemId.Focus() : gridView_OWN.DataSource = Nothing : Exit Sub
        gridView_OWN.DataSource = Nothing
        If cmbCostCentre_own.Text = "" Then
        End If
        strsql = "SELECT COUNT(*) CNT FROM " & cnAdminDb & "..RANGEMAST WHERE COSTID = '" & cmbCostCentre_own.SelectedValue.ToString & "' AND ITEMID = " & txtItemId.Text.Trim & ""
        If Val(GetSqlValue(cn, strsql).ToString) = 0 Then
            MsgBox("Range Master Not found in master", MsgBoxStyle.Information)
            gridView_OWN.DataSource = Nothing
            Exit Sub
        End If
        Getsubitemid()
        GetSizeid()
        Dim flagupdate As Boolean = False
        If subitemid_code <> "0" Then
            strsql = "select count(*) cnt from " & cnAdminDb & "..STKREORDER where ITEMID = " & txtItemId.Text.Trim & " AND  SUBITEMID IN ( " & subitemid_code & ") AND COSTID = '" & cmbCostCentre_own.SelectedValue.ToString & "'"
        Else
            strsql = "select count(*) cnt from " & cnAdminDb & "..STKREORDER where ITEMID = " & txtItemId.Text.Trim & "  AND COSTID = '" & cmbCostCentre_own.SelectedValue.ToString & "'"
        End If
        If chkwithoutsize.Checked = False Then
            If cmbItemSize_OWN.Text <> "ALL" Then
                strsql += vbCrLf + " AND SIZEID='" & cmbItemSize_OWN.SelectedValue & "'"
            Else
                strsql += vbCrLf + " AND SIZEID IN (" & sizeid_code.ToString & ")"
            End If
        End If
        Dim _chk As Double = Val(GetSqlValue(cn, strsql).ToString)
        If _chk > 0 Then
            flagupdate = True
        End If
        Dim tableInsert As String = "" & tempdbname & "..STKRANGEMAST"
        strsql = vbCrLf + "IF  OBJECT_ID('" & tableInsert & "','U') IS NOT NULL DROP TABLE " & tableInsert & ""
        strsql += vbCrLf + "CREATE TABLE " & tableInsert & "(ITEMID INT,SUBITEMID INT,SUBITEMNAME VARCHAR(50),COSTID VARCHAR(2),SIZEID INT,SIZENAME VARCHAR(50))"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()
        If flagupdate = True Then
            If subitemid_code <> "0" Then
                strsql = vbCrLf + " DECLARE db_cursor CURSOR FOR SELECT RANGECAPTION,RANGECAPTION,TOWEIGHT FROM " & cnAdminDb & "..STKREORDER WHERE COSTID = '" & cmbCostCentre_own.SelectedValue.ToString & "' AND ITEMID = " & txtItemId.Text & "  And SUBITEMID In ( " & subitemid_code & ")  GROUP BY RANGECAPTION,TOWEIGHT ORDER BY TOWEIGHT ; "
            Else
                strsql = vbCrLf + " Declare db_cursor CURSOR For Select RANGECAPTION,RANGECAPTION,TOWEIGHT FROM " & cnAdminDb & "..STKREORDER WHERE COSTID = '" & cmbCostCentre_own.SelectedValue.ToString & "' AND ITEMID = " & txtItemId.Text & " GROUP BY RANGECAPTION,TOWEIGHT ORDER BY TOWEIGHT ; "
            End If
            strsql += vbCrLf + "DECLARE @MYNAME VARCHAR(256);"
            strsql += vbCrLf + "DECLARE @MYAGE VARCHAR(256);"
            strsql += vbCrLf + "DECLARE @MYFAVORITECOLOR VARCHAR(40);"
            strsql += vbCrLf + "DECLARE @TEMPCAPTION VARCHAR(40);"
            strsql += vbCrLf + "DECLARE @QRY VARCHAR(8000);"
            strsql += vbCrLf + "OPEN db_cursor;"
            strsql += vbCrLf + "FETCH NEXT FROM db_cursor INTO @myName, @myAge, @myFavoriteColor;"
            strsql += vbCrLf + "WHILE @@FETCH_STATUS = 0  "
            strsql += vbCrLf + "BEGIN  "
            strsql += vbCrLf + "	print @myname"
            strsql += vbCrLf + "	IF @TEMPCAPTION = @MYNAME"
            strsql += vbCrLf + "	BEGIN"
            strsql += vbCrLf + "	    GOTO ZZZ"
            strsql += vbCrLf + "	END"
            strsql += vbCrLf + "	SET @TEMPCAPTION = @MYNAME"
            strsql += vbCrLf + "	SET @QRY = 'ALTER TABLE " & tableInsert & " ADD [' + @myName + '] VARCHAR(4)'"
            strsql += vbCrLf + "	PRINT @QRY"
            strsql += vbCrLf + "	EXEC (@QRY)"
            strsql += vbCrLf + "ZZZ:	"
            strsql += vbCrLf + "       FETCH NEXT FROM db_cursor INTO @myName, @myAge, @myFavoriteColor;"
            strsql += vbCrLf + "END;"
            strsql += vbCrLf + "CLOSE db_cursor;"
            strsql += vbCrLf + "DEALLOCATE db_cursor;"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()

            strsql = vbCrLf + "INSERT INTO " & tableInsert & "(ITEMID,SUBITEMID,SUBITEMNAME,COSTID,SIZEID,SIZENAME)"
            strsql += vbCrLf + " SELECT DISTINCT A.ITEMID,SUBITEMID"
            'strsql += vbCrLf + " ,(select SUBITEMNAME from " & cnAdminDb & "..SUBITEMMAST where ITEMID = a.ITEMID and SUBITEMID = a.SUBITEMID) SUBITEMNAME"
            strsql += vbCrLf + " ,CASE WHEN ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = A.ITEMID AND SUBITEMID = A.SUBITEMID),'') = '' THEN (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = A.ITEMID ) ELSE (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = A.ITEMID AND SUBITEMID = A.SUBITEMID) END SUBITEMNAME"
            strsql += vbCrLf + " , '" & cmbCostCentre_own.SelectedValue.ToString & "' "
            If subitemid_code <> "0" Then
                If chkwithoutsize.Checked = False Then
                    strsql += vbCrLf + " COSTID,ISNULL(B.SIZEID,0) SIZEID,'' SIZENAME from " & cnAdminDb & "..STKREORDER as a "
                    strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMSIZE  AS B ON B.ITEMID=A.ITEMID "
                Else
                    strsql += vbCrLf + " COSTID,0 SIZEID,'' SIZENAME from " & cnAdminDb & "..STKREORDER as a "
                End If

                strsql += vbCrLf + " where A.ITEMID = " & txtItemId.Text & "  And SUBITEMID In ( " & subitemid_code & ") "
            Else
                If chkwithoutsize.Checked = False Then
                    strsql += vbCrLf + " COSTID,ISNULL(B.SIZEID,0) SIZEID,'' SIZENAME from " & cnAdminDb & "..STKREORDER as a "
                    strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMSIZE  AS B ON B.ITEMID=A.ITEMID "
                Else
                    strsql += vbCrLf + " COSTID,0 SIZEID,'' SIZENAME from " & cnAdminDb & "..STKREORDER as a "
                End If
                strsql += vbCrLf + " where A.ITEMID = " & txtItemId.Text & "  "
            End If
            If chkwithoutsize.Checked = False Then
                If cmbItemSize_OWN.Text <> "ALL" And cmbItemSize_OWN.Text <> "" Then
                    strsql += vbCrLf + " And A.SIZEID='" & cmbItemSize_OWN.SelectedValue & "' And B.SIZEID='" & cmbItemSize_OWN.SelectedValue & "'"
                End If
            End If
            strsql += vbCrLf + " UNION ALL"
            If chkwithoutsize.Checked = False Then
                strsql += vbCrLf + " SELECT A.ITEMID,A.SUBITEMID,SUBITEMNAME,'" & cmbCostCentre_own.SelectedValue.ToString & "' COSTID,ISNULL(B.SIZEID,0) SIZEID,''SIZENAME "
                strsql += vbCrLf + " FROM " & cnAdminDb & "..SUBITEMMAST  AS A "
                strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMSIZE  AS B ON B.ITEMID=A.ITEMID "
            Else
                strsql += vbCrLf + " SELECT A.ITEMID,A.SUBITEMID,SUBITEMNAME,'" & cmbCostCentre_own.SelectedValue.ToString & "' COSTID,0 SIZEID,''SIZENAME "
                strsql += vbCrLf + " FROM " & cnAdminDb & "..SUBITEMMAST  AS A "
            End If

            If subitemid_code <> "0" Then
                strsql += vbCrLf + " WHERE A.ITEMID = " & txtItemId.Text & "  And A.SUBITEMID In ( " & subitemid_code & ") "
            Else
                strsql += vbCrLf + " WHERE A.ITEMID = " & txtItemId.Text & " "
            End If

            strsql += vbCrLf + " And SUBITEMID Not IN (SELECT SUBITEMID FROM " & cnAdminDb & "..STKREORDER "
            strsql += vbCrLf + " WHERE ITEMID =A.ITEMID And SUBITEMID = A.SUBITEMID )"
            If chkwithoutsize.Checked = False Then
                If cmbItemSize_OWN.Text <> "ALL" And cmbItemSize_OWN.Text <> "" Then
                    strsql += vbCrLf + " And B.SIZEID='" & cmbItemSize_OWN.SelectedValue & "'"
                End If
            End If

            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
            If chkwithoutsize.Checked = False Then
                strsql = vbCrLf + "UPDATE  A SET A.SIZENAME = B.SIZENAME FROM " & tableInsert & " A INNER JOIN " & cnAdminDb & ".. ITEMSIZE B ON A.ITEMID =B.ITEMID AND  A.SIZEID =B.SIZEID"
                cmd = New OleDbCommand(strsql, cn)
                cmd.ExecuteNonQuery()
            End If

        Else
            If subitemid_code <> "0" Then
                strsql = vbCrLf + " Declare DB_CURSOR CURSOR For Select DISTINCT CAPTION,CAPTION,TOWEIGHT FROM " & cnAdminDb & "..RANGEMAST WHERE COSTID = '" & cmbCostCentre_own.SelectedValue.ToString & "' AND ITEMID = " & txtItemId.Text & " AND SUBITEMID In ( " & subitemid_code & ")   GROUP BY CAPTION,TOWEIGHT ORDER BY TOWEIGHT ; "
            Else
                strsql = vbCrLf + " DECLARE DB_CURSOR CURSOR FOR SELECT DISTINCT CAPTION,CAPTION,TOWEIGHT FROM " & cnAdminDb & "..RANGEMAST WHERE COSTID = '" & cmbCostCentre_own.SelectedValue.ToString & "' AND ITEMID = " & txtItemId.Text & "  GROUP BY CAPTION,TOWEIGHT ORDER BY TOWEIGHT ; "
            End If

            strsql += vbCrLf + "DECLARE @MYNAME VARCHAR(256);"
            strsql += vbCrLf + "DECLARE @MYAGE VARCHAR(256);"
            strsql += vbCrLf + "DECLARE @MYFAVORITECOLOR VARCHAR(40);"
            strsql += vbCrLf + "DECLARE @TEMPCAPTION VARCHAR(40);"
            strsql += vbCrLf + "DECLARE @QRY VARCHAR(8000);"
            strsql += vbCrLf + "OPEN DB_CURSOR;"
            strsql += vbCrLf + "FETCH NEXT FROM DB_CURSOR INTO @MYNAME, @MYAGE, @MYFAVORITECOLOR;"
            strsql += vbCrLf + "WHILE @@FETCH_STATUS = 0  "
            strsql += vbCrLf + "BEGIN  "
            strsql += vbCrLf + "	PRINT @MYNAME"
            strsql += vbCrLf + "	IF @TEMPCAPTION = @MYNAME"
            strsql += vbCrLf + "	BEGIN"
            strsql += vbCrLf + "	    GOTO ZZZ"
            strsql += vbCrLf + "	END"
            strsql += vbCrLf + "	SET @TEMPCAPTION = @MYNAME"
            strsql += vbCrLf + "	SET @QRY = 'ALTER TABLE " & tableInsert & " ADD [' + @MYNAME + '] VARCHAR(4)'"
            strsql += vbCrLf + "	PRINT @QRY"
            strsql += vbCrLf + "	EXEC (@QRY)"
            strsql += vbCrLf + "ZZZ:	"
            strsql += vbCrLf + "       FETCH NEXT FROM DB_CURSOR INTO @MYNAME, @MYAGE, @MYFAVORITECOLOR;"
            strsql += vbCrLf + "END;"
            strsql += vbCrLf + "CLOSE DB_CURSOR;"
            strsql += vbCrLf + "DEALLOCATE DB_CURSOR;"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()

            'strsql = vbCrLf + "INSERT INTO " & tableInsert & "(ITEMID,SUBITEMID,SUBITEMNAME,COSTID)"
            'strsql += vbCrLf + " select distinct ITEMID,SUBITEMID,SUBITEMNAME, '" & cmbCostCentre_own.SelectedValue.ToString & "' COSTID "
            'strsql += vbCrLf + " from " & cnAdminDb & "..SUBITEMMAST where ITEMID = " & txtItemId.Text & ""
            strsql = vbCrLf + " INSERT INTO " & tableInsert & "(ITEMID,SUBITEMID,SUBITEMNAME,COSTID,SIZEID,SIZENAME)"
            strsql += vbCrLf + " SELECT DISTINCT A.ITEMID,A.SUBITEMID"
            strsql += vbCrLf + " ,CASE WHEN ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = A.ITEMID And SUBITEMID = A.SUBITEMID),'') = '' THEN (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = A.ITEMID ) ELSE (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = A.ITEMID AND SUBITEMID = A.SUBITEMID) END SUBITEMNAME"
            strsql += vbCrLf + " , '" & cmbCostCentre_own.SelectedValue.ToString & "' COSTID "
            If sizeid_code <> "0" Then
                If chkwithoutsize.Checked = False Then
                    strsql += vbCrLf + " , ISNULL(SZ.SIZEID,0)SIZEID,'' SIZENAME"
                Else
                    strsql += vbCrLf + " , 0 SIZEID,'' SIZENAME"
                End If
            Else
                strsql += vbCrLf + " , 0 SIZEID,'' SIZENAME"
            End If
            If subitemid_code <> "0" Then
                strsql += vbCrLf + " FROM " & cnAdminDb & "..RANGEMAST AS A LEFT JOIN " & cnAdminDb & "..STKREORDER S ON S.ITEMID = A.ITEMID AND S.SUBITEMID = A.SUBITEMID  "
                If chkwithoutsize.Checked = False Then
                    If sizeid_code <> "0" Then
                        strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMSIZE SZ ON SZ.ITEMID = A.ITEMID AND SZ.SIZEID IN(" & sizeid_code.ToString & ")"
                    End If
                End If

                strsql += vbCrLf + "WHERE A.ITEMID = " & txtItemId.Text & " AND A.SUBITEMID IN ( " & subitemid_code & ")  "
            Else
                strsql += vbCrLf + " FROM " & cnAdminDb & "..RANGEMAST AS A LEFT JOIN " & cnAdminDb & "..STKREORDER S ON S.ITEMID = A.ITEMID AND S.SUBITEMID = A.SUBITEMID  "
                If chkwithoutsize.Checked = False Then
                    If sizeid_code <> "0" Then
                        strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMSIZE SZ ON SZ.ITEMID = A.ITEMID AND SZ.SIZEID IN(" & sizeid_code.ToString & ")"
                    End If
                End If
                strsql += vbCrLf + "WHERE A.ITEMID = " & txtItemId.Text & "  "
            End If
            If chkwithoutsize.Checked = False Then
                If cmbItemSize_OWN.Text <> "ALL" And cmbItemSize_OWN.Text <> "" Then
                    strsql += vbCrLf + " AND SZ.SIZEID='" & cmbItemSize_OWN.SelectedValue & "'"
                End If
            End If

            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
            If chkwithoutsize.Checked = False Then
                strsql = vbCrLf + "UPDATE  A SET A.SIZENAME = B.SIZENAME FROM " & tableInsert & " A INNER JOIN " & cnAdminDb & ".. ITEMSIZE B ON A.ITEMID =B.ITEMID And  A.SIZEID =B.SIZEID"
                cmd = New OleDbCommand(strsql, cn)
                cmd.ExecuteNonQuery()
            End If
        End If
        If flagupdate = True Then
            strsql = " SELECT DISTINCT RANGECAPTION,ITEMID,SUBITEMID,COSTID,PIECE,SIZEID FROM " & cnAdminDb & "..STKREORDER "
            If subitemid_code <> "0" Then
                strsql += vbCrLf + " WHERE ITEMID = " & txtItemId.Text.ToString & " And SUBITEMID In ( " & subitemid_code & ")  "
            Else
                strsql += vbCrLf + " WHERE ITEMID = " & txtItemId.Text.ToString & "  "
            End If
            strsql += vbCrLf + " And COSTID = '" & cmbCostCentre_own.SelectedValue.ToString & "' "
            If chkwithoutsize.Checked = False Then
                If cmbItemSize_OWN.Text = "ALL" Or cmbItemSize_OWN.Text = "" Then
                    strsql += vbCrLf + " AND SIZEID in (" & sizeid_code.ToString & ")"
                ElseIf cmbItemSize_OWN.Text <> "ALL" Or cmbItemSize_OWN.Text <> "" Then
                    strsql += vbCrLf + " AND SIZEID='" & cmbItemSize_OWN.SelectedValue & "'"
                End If
            End If
            strsql += vbCrLf + " ORDER BY ITEMID,SUBITEMID"
            dt = New DataTable
            da = New OleDbDataAdapter(strsql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    'SELECT * FROM
                    strsql = " UPDATE " & tableInsert & " "
                    strsql += " SET [" & dt.Rows(i).Item("RANGECAPTION").ToString & "] = CASE WHEN '" & dt.Rows(i).Item("PIECE").ToString & "' ='0' THEN NULL ELSE '" & dt.Rows(i).Item("PIECE").ToString & "' END"
                    strsql += " WHERE ITEMID = '" & dt.Rows(i).Item("ITEMID").ToString & "'"
                    strsql += " AND SUBITEMID = '" & dt.Rows(i).Item("SUBITEMID").ToString & "'"
                    strsql += " AND COSTID= '" & dt.Rows(i).Item("COSTID").ToString & "' "
                    If chkwithoutsize.Checked = False Then
                        strsql += " AND SIZEID= '" & dt.Rows(i).Item("SIZEID").ToString & "' "
                    End If
                    cmd = New OleDbCommand(strsql, cn)
                    cmd.ExecuteNonQuery()
                Next
            End If
            strsql = vbCrLf + " SELECT * FROM " & tableInsert & " AS A"
            strsql += vbCrLf + " WHERE EXISTS (SELECT * FROM " & cnAdminDb & "..RANGEMAST WHERE ITEMID = A.ITEMID AND SUBITEMID = A.SUBITEMID AND COSTID = A.COSTID)"
            'STRSQL += VBCRLF + " ORDER BY ITEMID,SUBITEMID,COSTID"
            If cmbOrderBy.Text = "NONE" Then
                strsql += vbCrLf + " ORDER BY ITEMID,COSTID,SIZENAME"
            ElseIf cmbOrderBy.Text = "SUBITEMID" Then
                strsql += vbCrLf + " ORDER BY ITEMID,SUBITEMID,COSTID,SIZENAME"
            ElseIf cmbOrderBy.Text = "SUBITEMNAME" Then
                strsql += vbCrLf + " ORDER BY ITEMID,SUBITEMNAME,COSTID,SIZENAME"
            End If
        Else
            ' strsql = vbCrLf + "select * from " & tableInsert & " order by itemid,subitemid,costid"
            strsql = vbCrLf + " select * from " & tableInsert & " as a"
            strsql += vbCrLf + " WHERE EXISTS (SELECT * FROM " & cnAdminDb & "..RANGEMAST WHERE ITEMID = A.ITEMID And SUBITEMID = A.SUBITEMID And COSTID = A.COSTID)"
            If subitemid_code <> "0" Then
                strsql += vbCrLf + " AND ITEMID = " & txtItemId.Text.ToString & " And SUBITEMID In ( " & subitemid_code & ")  "
            Else
                strsql += vbCrLf + " AND ITEMID = " & txtItemId.Text.ToString & "  "
            End If
            If chkwithoutsize.Checked = False Then
                If cmbItemSize_OWN.Text = "ALL" Or cmbItemSize_OWN.Text = "" Then
                    strsql += vbCrLf + " AND SIZEID in (" & sizeid_code.ToString & ")"
                ElseIf cmbItemSize_OWN.Text <> "ALL" Or cmbItemSize_OWN.Text <> "" Then
                    strsql += vbCrLf + " AND SIZEID='" & cmbItemSize_OWN.SelectedValue & "'"
                End If
            End If

            If CMBORDERBY.TEXT = "NONE" Then
                STRSQL += VBCRLF + " ORDER BY ITEMID,COSTID,SIZENAME"
            ElseIf CMBORDERBY.TEXT = "SUBITEMID" Then
                STRSQL += VBCRLF + " ORDER BY ITEMID,SUBITEMID,COSTID,SIZENAME"
            ElseIf CMBORDERBY.TEXT = "SUBITEMNAME" Then
                STRSQL += VBCRLF + " ORDER BY ITEMID,SUBITEMNAME,COSTID,SIZENAME"
            End If
        End If

        dt = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            With gridView_OWN
                .DataSource = Nothing
                .DataSource = dt
                FormatGridColumns(gridView_OWN, False, True, False, False)
                'AutoResize(gridView_OWN)
                For i As Integer = 0 To .ColumnCount - 1
                    If gridView_OWN.Columns(i).Name <> "SIZENAME" Then
                        If gridView_OWN.Columns(i).Name <> "SUBITEMNAME" Then
                            'If .Columns(i).ValueType.Name Is GetType(Integer).Name Then
                            .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                            'End If
                        End If
                    End If
                Next
                .Columns("SUBITEMNAME").Width = 250
                .Columns("SUBITEMNAME").Frozen = True
                .Columns("SIZENAME").Frozen = True

                .Columns("SUBITEMNAME").ReadOnly = True
                .Columns("SIZENAME").ReadOnly = True
                If chkwithoutsize.Checked = True Then
                    .Columns("SIZENAME").Visible = False
                End If
                .Columns("SUBITEMID").Visible = False
                .Columns("ITEMID").Visible = False
                .Columns("COSTID").Visible = False
                .Columns("SIZEID").Visible = False
                gridView_OWN.Focus()
            End With
        Else
            MsgBox("Range Master Not found in master", MsgBoxStyle.Information)
            gridView_OWN.DataSource = Nothing
            Exit Sub
        End If
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click, NEwToolStripMenuItem.Click
        gridView_OWN.DataSource = Nothing
        btnUpdate.Enabled = True
        funcLoadSizeName()
        funcLoadItemName("")
        funcLoadSubItemName(0)
        funcLoadCostCentre()
        funcLoadMetal()
        funcLoadDesigner()
        cmbOrderBy.Items.Clear()
        cmbOrderBy.Items.Add("NONE")
        cmbOrderBy.Items.Add("SUBITEMID")
        cmbOrderBy.Items.Add("SUBITEMNAME")
        cmbOrderBy.Text = "SUBITEMNAME"
        cmbMetal_Own.Focus()
        cmbMetal_Own.Select()
        txtItemId.Text = ""
        cmbItemName_OWN.Text = ""
        chkcmbsubitem.Text = "ALL"
        subitemid_code = 0
    End Sub
    Private Sub btnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.Click
        If gridView_OWN.Rows.Count > 0 Then
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "STOCK REORDER MASTER", gridView_OWN, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub
    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If gridView_OWN.Rows.Count > 0 Then
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "STOCK REORDER MASTER", gridView_OWN, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
    Private Sub cmbItemName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbItemName_OWN.SelectedIndexChanged
        If Not cmbItemName_OWN.SelectedValue Is Nothing Then
            If cmbItemName_OWN.SelectedValue.ToString <> "System.Data.DataRowView" Then
                txtItemId.Text = cmbItemName_OWN.SelectedValue.ToString
            End If
        End If
    End Sub
    Private Sub cmbMetal_Man_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbMetal_Own.SelectedIndexChanged
        If cmbMetal_Own.SelectedValue.ToString <> "System.Data.DataRowView" And txtItemId.Text = "" Then
            funcLoadItemName(cmbMetal_Own.SelectedValue.ToString)
        Else
            funcLoadItemName(cmbMetal_Own.SelectedValue.ToString)
        End If
        txtItemId.Text = ""
        cmbItemName_OWN.Text = ""
    End Sub
    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If gridView_OWN.Rows.Count > 0 Then
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
            If chkcmbsubitem.Text.Trim = "" Then MsgBox("Check the SubItemName ", MsgBoxStyle.Information) : txtItemId.Focus() : gridView_OWN.DataSource = Nothing : Exit Sub
            If MsgBox("Do You Want Update", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2) = MsgBoxResult.Yes Then
                Dim dtUpdate As New DataTable
                Dim Qry As String = ""
                dtUpdate = gridView_OWN.DataSource
                If dtUpdate.Rows.Count > 0 Then
                    btnUpdate.Enabled = False
                    strsql = " DELETE " & cnAdminDb & "..STKREORDER "
                    strsql += vbCrLf + " WHERE ITEMID = '" & dtUpdate.Rows(0).Item("itemid").ToString & "' "
                    If chkcmbsubitem.Text <> "ALL" Then
                        strsql += vbCrLf + " AND  SUBITEMID IN ( " & subitemid_code & ")"
                    End If
                    strsql += vbCrLf + " AND COSTID = '" & cmbCostCentre_own.SelectedValue.ToString & "'"
                    If chkwithoutsize.Checked = False Then
                        If cmbItemSize_OWN.Text <> "ALL" Then
                            strsql += vbCrLf + " AND SIZEID='" & cmbItemSize_OWN.SelectedValue & "'"
                        ElseIf sizeid_code.ToString <> "0" Then
                            strsql += vbCrLf + " AND SIZEID in (" & sizeid_code & ")"
                        End If
                    End If
                    cmd = New OleDbCommand(strsql, cn)
                    cmd.ExecuteNonQuery()

                    For r As Integer = 0 To dtUpdate.Rows.Count - 1
                        Dim currentRow As Integer = r
                        Dim displayorder As Integer = 0
                        For c As Integer = 0 To dtUpdate.Columns.Count - 1
                            If dtUpdate.Columns(c).ColumnName.ToString = "ITEMID" Then Continue For
                            If dtUpdate.Columns(c).ColumnName.ToString = "SUBITEMID" Then Continue For
                            If dtUpdate.Columns(c).ColumnName.ToString = "SUBITEMNAME" Then Continue For
                            If dtUpdate.Columns(c).ColumnName.ToString = "COSTID" Then Continue For
                            Dim dr As DataRow = Nothing
                            Dim Fromweight As Decimal = 0
                            Dim Toweight As Decimal = 0
                            displayorder = displayorder + 1
                            Dim currentColumnName As String = dtUpdate.Columns(c).ColumnName.ToString
                            strsql = " SELECT FROMWEIGHT,TOWEIGHT FROM " & cnAdminDb & "..RANGEMAST"
                            strsql += vbCrLf + " WHERE ITEMID = '" & dtUpdate.Rows(r).Item("ITEMID").ToString & "'"
                            strsql += vbCrLf + " AND SUBITEMID = '" & dtUpdate.Rows(r).Item("SUBITEMID").ToString & "'"
                            strsql += vbCrLf + " AND CAPTION  = '" & currentColumnName & "'"
                            dr = GetSqlRow(strsql, cn)
                            If Not dr Is Nothing Then
                                Fromweight = dr.Item("FROMWEIGHT").ToString
                                Toweight = dr.Item("TOWEIGHT").ToString
                            Else
                                'InsertStkReorder(dtUpdate.Rows(r).Item("ITEMID").ToString, dtUpdate.Rows(r).Item("SUBITEMID").ToString _
                                '                 , dtUpdate.Rows(r).Item("COSTID").ToString _
                                '                 , c, r, Fromweight, Toweight _
                                '                 , dtUpdate.Rows(r).Item(currentColumnName).ToString)
                                Continue For
                            End If

                            Dim dtSizeName As DataTable
                            strsql = " SELECT SIZEID FROM " & cnAdminDb & "..ITEMSIZE WHERE ITEMID = " & dtUpdate.Rows(r).Item("ITEMID").ToString & " "
                            da = New OleDbDataAdapter(strsql, cn)
                            dtSizeName = New DataTable
                            da.Fill(dtSizeName)

                            If chkwithoutsize.Checked = False Then
                                If dtSizeName.Rows.Count = 0 Then
                                    InsertStkReorder(dtUpdate.Rows(r).Item("ITEMID").ToString, dtUpdate.Rows(r).Item("SUBITEMID").ToString _
                                        , dtUpdate.Rows(r).Item("COSTID").ToString _
                                        , c, r, Fromweight, Toweight _
                                        , dtUpdate.Rows(r).Item(currentColumnName).ToString, currentColumnName.ToString, displayorder, 0)
                                Else
                                    If cmbItemSize_OWN.Text <> "ALL" Then
                                        InsertStkReorder(dtUpdate.Rows(r).Item("ITEMID").ToString, dtUpdate.Rows(r).Item("SUBITEMID").ToString _
                                        , dtUpdate.Rows(r).Item("COSTID").ToString _
                                        , c, r, Fromweight, Toweight _
                                        , dtUpdate.Rows(r).Item(currentColumnName).ToString, currentColumnName.ToString, displayorder, Val(cmbItemSize_OWN.SelectedValue).ToString)
                                    Else
                                        ''For i As Integer = 0 To dtSizeName.Rows.Count - 1
                                        ''    InsertStkReorder(dtUpdate.Rows(r).Item("ITEMID").ToString, dtUpdate.Rows(r).Item("SUBITEMID").ToString _
                                        ''    , dtUpdate.Rows(r).Item("COSTID").ToString _
                                        ''    , c, r, Fromweight, Toweight _
                                        ''    , dtUpdate.Rows(r).Item(currentColumnName).ToString, currentColumnName.ToString, displayorder, dtSizeName.Rows(i).Item("SIZEID").ToString)
                                        ''Next
                                        InsertStkReorder(dtUpdate.Rows(r).Item("ITEMID").ToString, dtUpdate.Rows(r).Item("SUBITEMID").ToString _
                                        , dtUpdate.Rows(r).Item("COSTID").ToString _
                                        , c, r, Fromweight, Toweight _
                                        , dtUpdate.Rows(r).Item(currentColumnName).ToString, currentColumnName.ToString, displayorder, dtUpdate.Rows(r).Item("SIZEID").ToString)
                                    End If

                                End If
                            Else
                                InsertStkReorder(dtUpdate.Rows(r).Item("ITEMID").ToString, dtUpdate.Rows(r).Item("SUBITEMID").ToString _
                                , dtUpdate.Rows(r).Item("COSTID").ToString _
                                , c, r, Fromweight, Toweight _
                                , dtUpdate.Rows(r).Item(currentColumnName).ToString, currentColumnName.ToString, displayorder, 0)
                            End If
                        Next
                    Next
                End If
                MsgBox("Updated successfully", MsgBoxStyle.Information)
                btnNew_Click(Me, New System.EventArgs)
            End If
        End If
    End Sub

    Private Sub InsertStkReorder(ByVal itemid As String, ByVal subitemid As String, ByVal costid As String _
                                 , ByVal columnIndex As Integer, ByVal rowIndex As Integer _
                                 , ByVal fromweight As Double, ByVal toweight As Double _
                                 , ByVal currentColumnPcs As String, ByVal _rangeCaption As String, ByVal _displayorder As Integer, ByVal _sizeId As Integer)
        Dim Qry As String
        Qry = " INSERT INTO " & cnAdminDb & "..STKREORDER "
        Qry += vbCrLf + " ("
        Qry += vbCrLf + " SNO"
        Qry += vbCrLf + " ,ITEMID,SUBITEMID"
        Qry += vbCrLf + " ,COSTID,DESIGNID"
        Qry += vbCrLf + " ,FROMWEIGHT,TOWEIGHT"
        Qry += vbCrLf + " ,PIECE,WEIGHT"
        Qry += vbCrLf + " ,SIZEID"
        Qry += vbCrLf + " ,USERID,UPDATED,UPTIME"
        Qry += vbCrLf + " ,RANGEMODE,RANGECAPTION,DISPLAYORDER ) "
        Qry += vbCrLf + " VALUES ( "
        Qry += vbCrLf + " '" & itemid & subitemid & costid & columnIndex & rowIndex & "'"
        Qry += vbCrLf + " ,'" & itemid & "' " 'ITEMID
        Qry += vbCrLf + " ,'" & subitemid & "' " 'SUBITEMID
        Qry += vbCrLf + " ,'" & costid & "' " 'COSTID
        Qry += vbCrLf + " ,'0' " 'DESIGNID
        Qry += vbCrLf + " ,'" & fromweight & "' " 'FROMWEIGHT
        Qry += vbCrLf + " ,'" & toweight & "' " 'TOWEIGHT
        Qry += vbCrLf + " ,'" & currentColumnPcs & "' " 'PIECE
        Qry += vbCrLf + " ,'0'" 'WEIGHT
        Qry += vbCrLf + " ,'" & _sizeId & "'" 'SIZEID
        Qry += vbCrLf + " ,'" & userId & "'" 'USERID
        Qry += vbCrLf + " ,'" & Format(Now.Date, "yyyy-MM-dd") & "' " 'UPDATED
        Qry += vbCrLf + " ,'" & Now.ToShortTimeString & "'" 'UPTIME
        Qry += vbCrLf + " ,'P'" 'RANGEMODE
        Qry += vbCrLf + " , '" & _rangeCaption & "'"
        Qry += vbCrLf + " , '" & _displayorder & "'"
        Qry += vbCrLf + " )"
        cmd = New OleDbCommand(Qry, cn)
        cmd.ExecuteNonQuery()
    End Sub
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        If gridView_OWN.Rows.Count > 0 Then
            Try
                Dim Chk As String = gridView_OWN.CurrentRow.Index
                _tran = Nothing
                _tran = cn.BeginTransaction
                strsql = " DELETE FROM " & cnAdminDb & "..STKREORDER "
                strsql += vbCrLf + " WHERE ITEMID = " & gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("ITEMID").Value.ToString & " "
                strsql += vbCrLf + " "
                cmd = New OleDbCommand(strsql, cn, _tran)
                cmd.ExecuteNonQuery()
                _tran.Commit()
                _tran = Nothing
                MsgBox("Deleted")
                gridView_OWN.DataSource = Nothing
            Catch ex As Exception
                If Not _tran Is Nothing Then
                    _tran.Rollback()
                    _tran = Nothing
                    MessageBox.Show(ex.ToString)
                    Exit Sub
                ElseIf ex.ToString.Contains("Object reference not set to an instance of an object.") Then
                    gridView_OWN.Focus()
                    Exit Sub
                Else
                    MessageBox.Show(ex.ToString)
                    Exit Sub
                End If
            End Try
        End If
    End Sub
    Private Sub cmbItemSize_OWN_GotFocus(sender As Object, e As EventArgs) Handles cmbItemSize_OWN.GotFocus
        If Val(txtItemId.Text) > 0 Then
            Dim dtItemsize As New DataTable
            strsql = " SELECT 0 SIZEID, 'ALL' SIZENAME"
            strsql += " UNION ALL "
            strsql += " SELECT SIZEID,SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE ITEMID = " & Val(txtItemId.Text) & " "
            strsql += " ORDER BY SIZEID"
            da = New OleDbDataAdapter(strsql, cn)
            da.Fill(dtItemsize)
            If dtItemsize.Rows.Count > 0 Then
                cmbItemSize_OWN.DataSource = Nothing
                cmbItemSize_OWN.DataSource = dtItemsize
                cmbItemSize_OWN.ValueMember = "SIZEID"
                cmbItemSize_OWN.DisplayMember = "SIZENAME"
            End If
        End If
    End Sub

    Private Sub cmbItemName_OWN_Leave(sender As Object, e As EventArgs) Handles cmbItemName_OWN.Leave
        txtItemId_Leave(e, New System.EventArgs)
        funcLoadSubItemName(Val(txtItemId.Text))
    End Sub

    Private Sub AutoReziseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AutoReziseToolStripMenuItem.Click
        If gridView_OWN.RowCount > 0 Then
            If AutoReziseToolStripMenuItem.Checked Then
                gridView_OWN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridView_OWN.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridView_OWN.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView_OWN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridView_OWN.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView_OWN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
        End If
    End Sub

    Private Sub gridView_OWN_KeyDown(sender As Object, e As KeyEventArgs) Handles gridView_OWN.KeyDown
        If e.KeyCode = Keys.D Then
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
            If gridView_OWN.Rows.Count > 0 Then
                If MsgBox("Do You Want Delete This Row", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2) = MsgBoxResult.Yes Then
                    Try
                        Dim Chk As String = gridView_OWN.CurrentRow.Index
                        _tran = Nothing
                        _tran = cn.BeginTransaction
                        strsql = " DELETE FROM " & cnAdminDb & "..STKREORDER "
                        strsql += vbCrLf + " WHERE ITEMID = " & gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("ITEMID").Value.ToString & " And SUBITEMID = " & gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("SUBITEMID").Value.ToString & "   "
                        If Val(gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("SIZEID").Value.ToString) > 0 Then
                            strsql += vbCrLf + "  And SIZEID = '" & gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("SIZEID").Value.ToString & "' "
                        End If
                        cmd = New OleDbCommand(strsql, cn, _tran)
                        cmd.ExecuteNonQuery()
                        _tran.Commit()
                        _tran = Nothing
                        MsgBox("Deleted")
                        gridView_OWN.DataSource = Nothing
                    Catch ex As Exception
                        If Not _tran Is Nothing Then
                            _tran.Rollback()
                            _tran = Nothing
                            MessageBox.Show(ex.ToString)
                            Exit Sub
                        ElseIf ex.ToString.Contains("Object reference not set to an instance of an object.") Then
                            gridView_OWN.Focus()
                            Exit Sub
                        Else
                            MessageBox.Show(ex.ToString)
                            Exit Sub
                        End If
                    End Try
                End If

            End If
        Else
            If Not Char.IsDigit(gridView_OWN.CurrentCell.Value.ToString) Then
                If gridView_OWN.CurrentCell.Value.ToString <> "" Then
                    If gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name <> "SIZENAME" Then
                        If gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name <> "SUBITEMNAME" Then
                            MsgBox("Please Enter numeric Value", MsgBoxStyle.Information)
                            gridView_OWN.Focus()
                        End If
                    End If
                End If
            End If
        End If

    End Sub
    Private Sub gridView_OWN_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles gridView_OWN.CellEnter
        If Not Char.IsDigit(gridView_OWN.CurrentCell.Value.ToString) Then
            If gridView_OWN.CurrentCell.Value.ToString <> "" Then
                If gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name <> "SIZENAME" Then
                    If gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name <> "SUBITEMNAME" Then
                        MsgBox("Please Enter numeric Value", MsgBoxStyle.Information)
                        gridView_OWN.Focus()
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub gridView_OWN_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles gridView_OWN.CellEndEdit
        If Not Char.IsDigit(gridView_OWN.CurrentCell.Value.ToString) Then
            If gridView_OWN.CurrentCell.Value.ToString <> "" Then
                If gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name <> "SIZENAME" Then
                    If gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name <> "SUBITEMNAME" Then
                        MsgBox("Please Enter numeric Value", MsgBoxStyle.Information)
                        gridView_OWN.CurrentCell.Value = ""
                        gridView_OWN.Focus()
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub gridView_OWN_KeyPress(sender As Object, e As KeyPressEventArgs) Handles gridView_OWN.KeyPress
        If Not Char.IsDigit(gridView_OWN.CurrentCell.Value.ToString) Then
            If gridView_OWN.CurrentCell.Value.ToString <> "" Then
                If gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name <> "SIZENAME" Then
                    If gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name <> "SUBITEMNAME" Then
                        MsgBox("Please Enter numeric Value", MsgBoxStyle.Information)
                        gridView_OWN.Focus()
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub txtItemId_KeyDown(sender As Object, e As KeyEventArgs) Handles txtItemId.KeyDown
        If e.KeyCode = Keys.Insert Then
            strsql = "SELECT ITEMID, ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T' ORDER BY ITEMID"
            txtItemId.Text = BrighttechPack.SearchDialog.Show("Find Itemid", strsql, cn)
        End If
    End Sub

    Private Sub chkwithoutsize_CheckedChanged(sender As Object, e As EventArgs) Handles chkwithoutsize.CheckedChanged
        If chkwithoutsize.Checked = True Then
            cmbItemSize_OWN.Enabled = False
            cmbItemSize_OWN.Text = ""
        Else
            cmbItemSize_OWN.Enabled = True
        End If
    End Sub
#End Region
End Class
