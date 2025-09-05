Imports System.Data.OleDb
Public Class frmTDScategory
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim Ediordig As Integer
    Dim strSql As String
    Dim flagSave As Boolean = False
    Dim msinsert As Boolean = False
    Dim Tblinsert As Boolean = False
    Dim cardCode As Integer = Nothing
    Dim tempAcCode As String = Nothing ''For Update purpose
    Dim chitDbFound As Boolean = False
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Function funcCallGrid() As Integer
        Dim dt As New DataTable
        dt.Clear()
        strSql = " SELECT TDSCATNAME,TDSCATEGORY,ACCODE,"
        strSql += " (SELECT top 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=TC.ACCODE ) AS ACNAME"
        strSql += " ,TDSPER,DISPLAYORDER as [ORD.ID],TDSCATID "
        strSql += " FROM	" & cnAdminDb & "..TDSCATEGORY TC ORDER BY DISPLAYORDER,TDSCATNAME"
        Try
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            Return 0
        End Try
        gridView.DataSource = dt
        With gridView
            .Columns("ORD.ID").Width = 75
            .Columns("TDSCATID").Visible = False
            .Columns("TDSCATNAME").Width = 250
            .Columns("TDSCATEGORY").Width = 100
            .Columns("ACCODE").Visible = False
            .Columns("ACNAME").Width = 125
            .Columns("TDSPER").Width = 105
            .Columns("TDSPER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End With
        Return 0
    End Function
    Function funcNew()
        tabMain.SelectedTab = tabGeneral
        tempAcCode = Nothing
        cardCode = Nothing
        objGPack.TextClear(Me)
        'objGPack.TextClear(grpInfo)
        funcLoadDefaultAcCode()
        funcCallGrid()
        flagSave = False
        txtCardName__Man.Focus()
        Return 0
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcSave() As Integer
        '        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        '        If objGPack.Validator_Check(Me) Then Exit Function
        If txtCardName__Man.Text = "" Or txtShortName__Man.Text = "" Or cmbDefaultAcCode.Text = "" Then
            MsgBox("Please Enter Valid Input")
            Exit Function
        End If
        If flagSave = False Then
            If objGPack.DupChecker(txtCardName__Man, "SELECT 1 FROM " & cnAdminDb & "..achead WHERE Acname = '" & txtCardName__Man.Text & "' AND Accode <> '" & cardCode & "'") Then
                Exit Function
            End If
            funcAdd()
        ElseIf flagSave = True Then
            funcUpdate()
        End If
    End Function
    Function funcAdd() As Integer

        Dim ds As New Data.DataSet
        ds.Clear()
        Dim dr As OleDbDataReader = Nothing
        Dim tran As OleDbTransaction = Nothing
        Dim CardCode As String = Nothing
        Dim AcCode As String = Nothing
        Dim DefaultAcCode As String = Nothing
        Dim PrizeAc As String = Nothing
        Dim BonusAc As String = Nothing
        Dim DeductAc As String = Nothing
        Dim CompanyId As String = Nothing
        Dim SchemeId As String = Nothing
        Dim TdsCatid As Integer
        Try
            tran = cn.BeginTransaction()
            strSql = " select max(substring(accode,4,10)) as Accode from " & cnAdminDb & "..achead where Acgrpcode=11 and substring(accode,1,3) ='TDS' and accode <>'TDSIN'"
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(ds, "Accode")
            CardCode = Val(ds.Tables("Accode").Rows(0).Item("Accode").ToString) + 1

            AcCode = "TDS" + Replace(Space(4 - Len(CardCode)), " ", 0) & CardCode

            strSql = " select top 1 AcCode from " & cnAdminDb & "..achead where ACName = '" & cmbDefaultAcCode.Text & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(ds, "DefaultAcCode")
            If ds.Tables("DefaultAcCode").Rows.Count > 0 Then
                msinsert = False
                DefaultAcCode = ds.Tables("DefaultAccode").Rows(0).Item("AcCode").ToString
            Else
                msinsert = True
                DefaultAcCode = AcCode
            End If

            strSql = " select 1 as Accode from " & cnAdminDb & "..tdscategory where tdscatname='" & Trim(txtCardName__Man.Text) & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(ds, "Accode")
            If ds.Tables("Accode").Rows.Count > 0 Then
                Tblinsert = True
            Else
                Tblinsert = False
            End If

            strSql = " select isnull(max(Tdscatid),0) + 1 as Tdscatid from " & cnAdminDb & "..tdscategory "
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(ds, "Tdscat")
            If ds.Tables("Tdscat").Rows.Count > 0 Then
                TdsCatid = Val(ds.Tables("Tdscat").Rows(0).Item("Tdscatid").ToString)
            Else
                TdsCatid = 1
            End If

            ''Insert into AcHead
            If msinsert = True Then
                strSql = " insert into " & cnAdminDb & "..AcHead("
                strSql += " AcCode,AcName,ACGrpCode,ACSubGrpCode,"
                strSql += " AcType,DoorNo,Address1,Address2,"
                strSql += " Address3,Area,City,Pincode,"
                strSql += " PhoneNo,Mobile,"
                strSql += " Emailid,"
                strSql += " WebSite,Ledprint,TdsFlag,TdsPer,"
                strSql += " Depflag,Depper,Outstanding,AutoGen,"
                strSql += " VATEX,LocalOutst,LocalTaxNo,CentralTaxNo,"
                strSql += " Userid,CrDate,CrTime)values("
                strSql += " '" & DefaultAcCode & "','" & txtCardName__Man.Text & "','11','0',"
                strSql += " 'O','','','',"
                strSql += " '','','','',"
                strSql += " '','',"
                strSql += " '',"
                strSql += " '','','',0,"
                strSql += " '',0,'','',"
                strSql += " '','','','',"
                strSql += " " & userId & ",'" & Today.Date.ToString("yyyy-MM-dd") & "','" & Date.Now.ToLongTimeString & "')"
                ExecQuery(SyncMode.Master, strSql, cn, tran)
            End If
            If Tblinsert = True Then
                strSql = " insert into " & cnAdminDb & "..tdscategory( TDSCATID,TDSCATNAME,TDSCATEGORY,DISPLAYORDER,ACCODE,TDSPER )"
                strSql += " Values("
                strSql += "" & TdsCatid & ",'" & txtCardName__Man.Text & "','" & txtShortName__Man.Text & "',"
                strSql += " '" & txtorderid.Text & "','" & DefaultAcCode & "'," & Val(txtSurcharge.Text) & ")"
                ExecQuery(SyncMode.Master, strSql, cn, tran)
            End If
            tran.Commit()
            funcNew()
        Catch ex As Exception
            tran.Rollback()
            tran.Dispose()
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcUpdate()
        Dim ds As New Data.DataSet
        ds.Clear()
        Dim CompanyId As String = Nothing
        Dim SchemeId As String = Nothing
        Dim DefaultAcCode As String = Nothing

        Dim tran As OleDbTransaction = Nothing

        Try
            tran = cn.BeginTransaction()

            strSql = " select TDSCATNAME as AcCode from " & cnAdminDb & "..TDSCATEGORY where TDSCATNAME = '" & txtCardName__Man.Text & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(ds, "DefaultAcCode")
            If ds.Tables("DefaultAcCode").Rows.Count > 0 Then
                DefaultAcCode = ds.Tables("DefaultAcCode").Rows(0).Item("AcCode").ToString
            Else
                DefaultAcCode = tempAcCode
            End If

            '''''''''''''''''''''''
            strSql = " Update " & cnAdminDb & "..TDSCATEGORY Set "
            strSql += " TDSCATEGORY = '" & txtShortName__Man.Text & "'"
            strSql += " ,DISPLAYORDER = '" & txtorderid.Text & "'"
            strSql += " ,ACCODE = (SELECT ACCODE  FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbDefaultAcCode.Text & "')"
            strSql += " ,TDSPER = '" & Val(txtSurcharge.Text) & "'"
            strSql += " where TDSCATNAME ='" & txtCardName__Man.Text & "'"
            ExecQuery(SyncMode.Master, strSql, cn, tran)

            '     strSql = " Update " & cnAdminDb & "..ACHEAD SET ACNAME = '" & txtCardName__Man.Text & "'"
            '    strSql += " Where ACCODE = '" & objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..CREDITCARD WHERE CARDCODE = " & cardCode & "", , , tran) & "'"
            '   ExecQuery(SyncMode.Master, strSql, cn, tran)
            tran.Commit()
            funcNew()
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
        Return 0
    End Function
    Function funcCheckChitDb() As Boolean
        Dim dt As New DataTable
        dt.Clear()
        Dim QRY As String
        QRY = " Select CtlText from " & cnAdminDb & "..Softcontrol where ctlId = 'ChitDb' and ctlText = 'Y'"
        da = New OleDbDataAdapter(QRY, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return False
        End If
        Return True
    End Function
    Function funcCheckDefaultAcCode() As Boolean
        Dim dt As New DataTable
        dt.Clear()
        strSql = " Select Name from " & cnAdminDb & "..CreditCard where Name = '" & txtCardName__Man.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return False
        End If
        Return True
    End Function

    Function funcCheckOrder() As Boolean
    End Function

    Function funcLoadDefaultAcCode()
        Dim dt As New DataTable
        dt.Clear()
        cmbDefaultAcCode.Items.Clear()
        cmbDefaultAcCode.Text = ""
        'strSql = " select Acname as Name from " & cnAdminDb & "..achead where substring(accode,1,3) ='TDS' order by 1"
        strSql = " SELECT ACNAME AS NAME FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE='O' ORDER BY ACNAME"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        Dim cnt As Integer = Nothing
        For cnt = 0 To dt.Rows.Count - 1
            With dt.Rows(cnt)
                cmbDefaultAcCode.Items.Add(.Item("Name").ToString)
            End With
        Next
        cmbDefaultAcCode.Text = dt.Rows(0).Item("Name").ToString
        Return 0
    End Function

    Private Sub frmCreditCard_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                tabMain.SelectedTab = tabGeneral
            End If
        End If
    End Sub

    Private Sub frmCreditCard_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub GrpFieldPos()
        If funcCheckChitDb() = True Then
            'Available



            pnl1.Location = New Point(6, 16)

            pnl2.Location = New Point(6, 139)
            pnlButtons.Location = New System.Drawing.Point(6, 216)
            grpField.Size = New System.Drawing.Size(441, 252)
        Else
            pnl1.Location = New Point(6, 16)

            pnl2.Location = New Point(6, 91)
            pnlButtons.Location = New System.Drawing.Point(6, 166)
            grpField.Size = New System.Drawing.Size(441, 203)
        End If
    End Sub
    Private Sub frmCreditCard_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcGridStyle(gridView)
        tabMain.ItemSize = New Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))

        Dim dt As New DataTable
        dt.Clear()
        strSql = " Select ctlText from " & cnAdminDb & "..SoftControl where ctlId = 'ChitDbPrefix'"

        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            cnChitCompanyid = dt.Rows(0).Item("ctlText").ToString
        End If
        If funcCheckChitDb() = True Then
            'Available

            chitDbFound = True
        Else
            'Not Available
            chitDbFound = False
        End If
        'GrpFieldPos()
        '        funcLoadDefaultAcCode()
        funcNew()
        txtCardName__Man.Focus()
    End Sub

    Private Sub cmbCardType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)


        txtSurcharge.Enabled = False
        txtSurcharge.Enabled = False

        GrpFieldPos()

        funcLoadDefaultAcCode()
    End Sub


    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        funcSave()
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
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
    End Sub
    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        funcCallGrid()
        tabMain.SelectedTab = tabView
        gridView.Focus()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub
    Function funcGetDetails(ByVal tempCardCode As String)
        Dim dt As New DataTable
        dt.Clear()
        Try
            strSql = " SELECT DISPLAYORDER,TDSCATID,TDSCATNAME,TDSCATEGORY,ACCODE,"
            strSql += " (SELECT top 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=TC.ACCODE) AS ACNAME"
            strSql += " ,TDSPER,DISPLAYORDER "
            strSql += " FROM	" & cnAdminDb & "..TDSCATEGORY TC where TDSCATNAME = '" & tempCardCode & "'"

            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If Not dt.Rows.Count > 0 Then
                Return 0
            End If
            With dt.Rows(0)
                Ediordig = .Item("DISPLAYORDER").ToString
                txtCardName__Man.Text = .Item("TDSCATNAME").ToString
                txtShortName__Man.Text = .Item("TDSCATEGORY").ToString
                txtorderid.Text = .Item("DISPLAYORDER").ToString
                cmbDefaultAcCode.Text = .Item("ACNAME").ToString
                txtSurcharge.Text = .Item("TDSPER").ToString
                'Accode = tempCardCode'
                tempAcCode = .Item("AcCode").ToString
                tabMain.SelectedTab = tabGeneral
            End With
            'cmbCardType.Select()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        Return 0
    End Function

    Private Sub txtCommision_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
    End Sub

    Private Sub txtSurcharge_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        Dim keyChar As String
        keyChar = e.KeyChar
        If AscW(e.KeyChar) = 46 Then
            If txtSurcharge.Text.Contains(".") = True Then
                e.Handled = True
            End If
        End If
        If (Not Char.IsDigit(keyChar)) And AscW(keyChar) <> 8 And AscW(keyChar) <> 13 And AscW(keyChar) <> 46 Then
            e.Handled = True
            MsgBox("Digits only Allowed 1 to 9")
            txtSurcharge.Focus()
        End If
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblStatus.Visible = True
    End Sub
    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridView.RowCount > 0 Then
                funcGetDetails(gridView.Item(0, gridView.CurrentRow.Index).Value.ToString)
                flagSave = True
            End If
        ElseIf e.KeyCode = Keys.Escape Then
        End If
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus

        lblStatus.Visible = False
    End Sub




    'Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
    '    If Not gridView.RowCount > 0 Then
    '        Exit Sub
    '    End If
    '    '  If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
    '    Dim chkQry As String = Nothing
    '    Dim delKey As String = gridView.Rows(gridView.CurrentRow.Index).Cells("TDSCATNAME").Value.ToString

    '    strSql = " SELECT DBNAME FROM " & cnAdminDb & "..DBMASTER"
    '    Dim dtDb As New DataTable
    '    da = New OleDbDataAdapter(strSql, cn)
    '    da.Fill(dtDb)
    '    If dtDb.Rows.Count > 0 Then
    '        Dim delQry As String = Nothing
    '        delQry += " DELETE FROM " & cnAdminDb & "..tdscategory WHERE TDSCATNAME = '" & delKey & "'"
    '        DeleteItem(SyncMode.Master, chkQry, delQry)
    '        funcCallGrid()
    '    End If
    'End Sub

    Private Sub txtCardName__Man_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCardName__Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If objGPack.DupChecker(txtCardName__Man, "SELECT 1 FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & txtCardName__Man.Text & "' AND CARDCODE <> '" & cardCode & "'") Then
                Exit Sub
            End If
        End If
    End Sub

    Private Sub txtCardName__Man_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCardName__Man.LostFocus
        If txtCardName__Man.Text <> "" Then
            If funcCheckDefaultAcCode() = False Then
                cmbDefaultAcCode.Items.Clear()
                funcLoadDefaultAcCode()
                cmbDefaultAcCode.Items.Add(txtCardName__Man.Text)
                cmbDefaultAcCode.Text = txtCardName__Man.Text
            Else
                funcLoadDefaultAcCode()
                cmbDefaultAcCode.Text = txtCardName__Man.Text
            End If
        End If
    End Sub

    Private Sub gridView_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellContentClick

    End Sub

    Private Sub tabGeneral_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabGeneral.Click

    End Sub

    Private Sub txtCardName__Man_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCardName__Man.TextChanged

    End Sub

    Private Sub txtShortName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtShortName__Man.TextChanged

    End Sub

    Private Sub txtorderid_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtorderid.KeyPress
        funcCheckOrder()
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtorderid.Text <> "" Then
                If flagSave = True And Ediordig = txtorderid.Text Then
                Else
                    Dim dt As New DataTable
                    dt.Clear()
                    strSql = " Select DISPLAYORDER from " & cnAdminDb & "..TDSCATEGORY where DISPLAYORDER = '" & txtorderid.Text & "'"
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(dt)
                    If dt.Rows.Count > 0 Then
                        MsgBox("This Display Orderid Alredy Exist")
                        txtorderid.Text = ""
                        txtorderid.Focus()
                    Else
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub txtorderid_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtorderid.TextChanged
    End Sub
End Class