Imports System.Data.OleDb
''LAST MODIFIED 20-12-2014-BY RAJKUMAR(ADDED REMARKS COULMN FOR NSC)
Public Class frmPasswordMaster
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim dtOP, dt, dtcostcenter, dtcount, dtuser As New DataTable
    Dim strSql As String
    Dim updFlag As Boolean
    Dim pwdid As Int32
    Private _r As Random = New Random
    Private OTPGENOPTION As String = GetAdmindbSoftValue("OTPGENOPTION", "C")
    Function fillCostids()
        strSql = "SELECT COSTID,COSTNAME FROM " & cnAdminDb & "..COSTCENTRE"
        da = New OleDbDataAdapter(strSql, cn)
        dtcostcenter = New DataTable()
        da.Fill(dtcostcenter)
        If dtcostcenter.Rows.Count > 0 Then
            cmbcostcenter_OWN.DataSource = dtcostcenter
            cmbcostcenter_OWN.DisplayMember = "COSTNAME"
            cmbcostcenter_OWN.ValueMember = "COSTID"
            cmbcostcenter_OWN.SelectedIndex = 0
        End If
    End Function
    Function fillOptionids()
        strSql = "SELECT OPTIONID,OPTIONNAME FROM " & cnAdminDb & "..PRJPWDOPTION WHERE ACTIVE='Y'"
        da = New OleDbDataAdapter(strSql, cn)
        dtOP = New DataTable()
        da.Fill(dtOP)
        If dtOP.Rows.Count > 0 Then
            cmboptionname_OWN.DataSource = dtOP
            cmboptionname_OWN.DisplayMember = "OPTIONNAME"
            cmboptionname_OWN.ValueMember = "OPTIONID"
            cmboptionname_OWN.SelectedIndex = 0
        End If
    End Function
    Function funNew()
        If GetAdmindbSoftValue("COSTCENTRE", "N") = "Y" Then
            fillcostids()
            cmbcostcenter_OWN.Enabled = True
        Else
            cmbcostcenter_OWN.Enabled = False
        End If
        fillUserids()
        fillOptionids()
        cmbcostcenter_OWN.Enabled = True
        updFlag = False
        cmbcostcenter_OWN.Select()
        txtRemarks.Text = ""
    End Function
    Function funcSave()
        Dim noofOtp As Integer = Val(txtNoofTimes_NUM.Text)
        For i As Integer = 0 To noofOtp - 1
            strSql = "SELECT ISNULL(MAX(PWDID),0) FROM " & cnAdminDb & "..PWDMASTER"
            pwdid = Val(GetSqlValue(cn, strSql)) + 1
            Dim anew As Long
            For ii As Integer = 0 To Len(pwdid) - 1
                anew += _r.Next()
            Next
            Dim empy As Int32
            If lbloptype.Text.ToString() = "Day" Then
                empy = 1
            ElseIf lbloptype.Text.ToString() = "Week" Then
                empy = 7
            ElseIf lbloptype.Text.ToString() = "Month" Then
                empy = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)
            ElseIf lbloptype.Text.ToString() = "Year" Then
                empy = 365
            End If
            Dim cmbuserid As Integer
            Dim cmbcostid As String = ""
            Dim cmbcashid As String = ""
            If cmbuser_OWN.Text <> "" Then cmbuserid = Val(cmbuser_OWN.SelectedValue.ToString)
            If cmbcostcenter_OWN.Text <> "" Then cmbcostid = cmbcostcenter_OWN.SelectedValue.ToString
            If Not updFlag Then
                strSql = "INSERT INTO " & cnAdminDb & "..PWDMASTER(PWDID,COSTID,PWDDATE,PWDTIME,PWDUSERID,"
                strSql += vbCrLf + "PASSWORD,PWDOPTIONID,PWDEXPIRY,PWDSTATUS,PWDCLSDATE,PWDCLSTIME,CRUSERID,REMARKS)"
                strSql += vbCrLf + "VALUES("
                strSql += vbCrLf + "" & pwdid & "" 'PWDID
                strSql += vbCrLf + ",'" & cmbcostid & "'"
                'strSql += vbCrLf + ",'" & DateTime.Now.Date.ToString() & "'" 'PWDDATE
                'strSql += vbCrLf + ",'" & Date.Now.ToLongTimeString & "'" 'PWDTIME
                strSql += vbCrLf + ",'" & GetEntryDate(GetServerDate) & "'" 'PWDDATE
                strSql += vbCrLf + ",'" & GetServerTime() & "'" 'PWDTIME
                strSql += vbCrLf + ",'" & IIf(cmbuserid = 0, "''", cmbuserid) & "'" 'PWDUSERID
                strSql += vbCrLf + ",'" & BrighttechPack.Methods.Encrypt(anew) & "'" 'PASSWORD
                strSql += vbCrLf + ",'" & cmboptionname_OWN.SelectedValue.ToString & "'" 'PWDOPTIONID
                strSql += vbCrLf + "," & empy & "" 'PWDEXPIRY
                strSql += vbCrLf + ",'N'" 'PWDSTATUS
                strSql += vbCrLf + ",''" 'PWDCLSDATE
                strSql += vbCrLf + ",''" 'PWDCLSTIME
                strSql += vbCrLf + "," & userId & "" 'CRUSERID
                strSql += vbCrLf + ",'" & txtRemarks.Text & "')" 'REMARKS
                ExecQuery(SyncMode.Stock, strSql, cn, tran, cmbcostid)
            End If
        Next
        MsgBox("Password Generated Successfully.", MsgBoxStyle.Information)
        funNew()
    End Function

#Region "Events"
    Private Sub frmPasswordMaster_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        funNew()
    End Sub
    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        funcSearch()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If Not OTPGENOPTION.Contains("S") Then If _SyncTo <> "" Then MsgBox("Master Entry cannot allow at Location", MsgBoxStyle.Information) : Exit Sub
        funcSave()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        funNew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGeneral
        funNew()
    End Sub

    Private Sub frmPasswordMaster_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
        If e.KeyCode = Keys.Escape Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                btnBack_Click(Me, New EventArgs)
            End If
        End If
    End Sub
#End Region

    Private Sub cmboptionname_OWN_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmboptionname_OWN.Leave
        cmboptionname_SelectedIndexChanged(sender, e)
    End Sub

    Private Sub cmboptionname_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmboptionname_OWN.SelectedIndexChanged
        strSql = "SELECT OPTIONTYPE FROM  " & cnAdminDb & "..PRJPWDOPTION WHERE OPTIONID=" & Val(cmboptionname_OWN.SelectedValue.ToString()) & " "
        Dim type As String = GetSqlValue(cn, strSql)
        If type = "OP" Then
            lbloptype.Text = "OneTime"
            txtNoofTimes_NUM.Enabled = False
        ElseIf type = "DP" Then
            lbloptype.Text = "Day"
            txtNoofTimes_NUM.Enabled = True
        ElseIf type = "WP" Then
            lbloptype.Text = "Week"
            txtNoofTimes_NUM.Enabled = True
        ElseIf type = "MP" Then
            lbloptype.Text = "Month"
            txtNoofTimes_NUM.Enabled = True
        ElseIf type = "YP" Then
            lbloptype.Text = "Year"
            txtNoofTimes_NUM.Enabled = True
        End If
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            If gridView.Rows.Count > 0 Then
                With gridView
                    pwdid = Val(.CurrentRow.Cells("PWDID").Value.ToString())
                    cmbcostcenter_OWN.Text = .CurrentRow.Cells("COSTNAME").Value.ToString()
                    cmboptionname_OWN.Text = .CurrentRow.Cells("OPTIONNAME").Value.ToString() 'GetSqlValue(cn, strSql)
                    cmbuser_OWN.Text = .CurrentRow.Cells("USERNAME").Value.ToString() 'GetSqlValue(cn, strSql)
                End With
                tabMain.SelectedTab = tabGeneral
                cmbcostcenter_OWN.Enabled = False
                cmbuser_OWN.Focus()
                cmbuser_OWN.SelectAll()
                updFlag = True
            End If
        End If
    End Sub
    Function funcSearch()
        strSql = "SELECT P.PWDID,O.OPTIONNAME,C.COSTNAME,U.USERNAME,P.COSTID,P.PASSWORD,P.PWDSTATUS STATUS FROM " & cnAdminDb & "..PWDMASTER P"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..COSTCENTRE AS C ON P.COSTID=C.COSTID "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..PRJPWDOPTION AS O ON P.PWDOPTIONID=O.OPTIONID "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..USERMASTER AS U ON P.PWDUSERID=U.USERID "
        strSql += vbCrLf + " WHERE 1=1 ORDER BY P.PWDID DESC"
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtgrid As New DataTable
        da.Fill(dtgrid)
        If dtgrid.Rows.Count > 0 Then
            gridView.DataSource = Nothing
            gridView.DataSource = dtgrid
            With gridView
                .Columns("COSTNAME").Width = 150
                .Columns("OPTIONNAME").Width = 150
                .Columns("USERNAME").Width = 100
                .Columns("PWDID").Width = 50
                .Columns("STATUS").Width = 75
                .Columns("OPTIONNAME").HeaderText = "OPTIONNAME"
                .Columns("COSTID").Visible = False
                .Columns("PASSWORD").Visible = False
            End With
            tabMain.SelectedTab = tabView
            gridView.Focus()
        Else
            gridView.DataSource = Nothing
            MsgBox("Record Not Found.", MsgBoxStyle.Information)
        End If
    End Function
    Private Sub cmbuser_OWN_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbuser_OWN.GotFocus
        fillUserids()
    End Sub
    Function fillUserids()
        strSql = "SELECT USERID,USERNAME  FROM " & cnAdminDb & "..USERMASTER WHERE  ISNULL(ACTIVE,'') <>'N'"
        If cmbcostcenter_OWN.Text <> "" Then strSql += " AND COSTID='" & cmbcostcenter_OWN.SelectedValue.ToString & "'"
        strSql += " UNION ALL"
        strSql += " SELECT USERID,USERNAME  FROM " & cnAdminDb & "..USERMASTER WHERE  ISNULL(ACTIVE,'') <>'N'"
        If cmbcostcenter_OWN.Text <> "" Then strSql += " AND USERCOSTID LIKE ('%" & cmbcostcenter_OWN.SelectedValue.ToString & "%')"
        da = New OleDbDataAdapter(strSql, cn)
        dtcount = New DataTable()
        da.Fill(dtcount)
        If dtcount.Rows.Count > 0 Then
            cmbuser_OWN.DataSource = dtcount
            cmbuser_OWN.DisplayMember = "USERNAME"
            cmbuser_OWN.ValueMember = "USERID"
            cmbuser_OWN.SelectedIndex = 0
        End If
    End Function

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        Dim costid As String = ""

        If Not gridView.RowCount > 0 Then Exit Sub
        Dim list As New List(Of String)
        list.Add("PWDID")
        Dim pwdId As String = gridView.CurrentRow.Cells("PWDID").Value.ToString
        strSql = " SELECT COSTID FROM " & cnAdminDb & "..PWDMASTER WHERE PWDID =" & pwdId
        costid = objGPack.GetSqlValue(strSql, , , tran)
        strSql = " UPDATE " & cnAdminDb & "..PWDMASTER SET PWDSTATUS='C' WHERE PWDID =" & pwdId
        ExecQuery(SyncMode.Stock, strSql, cn, tran, costid)
        MsgBox("Update Successfully.", MsgBoxStyle.Information)
        funcSearch()
        gridView.Focus()      
    End Sub
End Class