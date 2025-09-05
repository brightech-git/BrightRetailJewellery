Imports System.Data.OleDb
Public Class frmGSTcategory
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
        strSql = " SELECT GSTCATNAME,GSTCATEGORY,ACCODE,"
        strSql += " (SELECT top 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=TC.CGSTAC) AS CGSTAC,"
        strSql += " (SELECT top 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=TC.SGSTAC) AS SGSTAC,"
        strSql += " (SELECT top 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=TC.IGSTAC) AS IGSTAC,"
        strSql += " (SELECT top 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=TC.RCGSTAC) AS RCGSTAC,"
        strSql += " (SELECT top 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=TC.RSGSTAC) AS RSGSTAC,"
        strSql += " (SELECT top 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=TC.RIGSTAC) AS RIGSTAC,"
        strSql += " CASE WHEN RD='Y' THEN 'YES' ELSE 'NO' END RD,"
        strSql += " GSTPER,DISPLAYORDER as [OID],GSTCATID "
        strSql += " FROM	" & cnAdminDb & "..GSTCATEGORY TC ORDER BY DISPLAYORDER,GSTCATNAME"
        Try
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            Return 0
        End Try
        gridView.DataSource = dt
        With gridView
            .Columns("OID").Width = 40
            .Columns("GSTCATID").Visible = False
            .Columns("GSTCATNAME").Width = 250
            .Columns("GSTCATEGORY").Width = 100
            .Columns("ACCODE").Visible = False
            .Columns("CGSTAC").Width = 175
            .Columns("SGSTAC").Width = 175
            .Columns("IGSTAC").Width = 175
            .Columns("RCGSTAC").Width = 150
            .Columns("RSGSTAC").Width = 150
            .Columns("RIGSTAC").Width = 150
            .Columns("GSTPER").Width = 105
            .Columns("RD").Width = 50
            .Columns("GSTPER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RD").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("CGSTAC").HeaderText = "CGST ACNAME"
            .Columns("SGSTAC").HeaderText = "SGST ACNAME"
            .Columns("IGSTAC").HeaderText = "IGST ACNAME"
            .Columns("RCGSTAC").HeaderText = "RCM CGST ACNAME"
            .Columns("RSGSTAC").HeaderText = "RCM SGST ACNAME"
            .Columns("RIGSTAC").HeaderText = "RCM IGST ACNAME"
            .Columns("GSTCATNAME").HeaderText = "GST CATNAME"
            .Columns("GSTCATEGORY").HeaderText = "GSTCAT"
            .Columns("RD").HeaderText = "RDLR"
        End With
        Return 0
    End Function
    Function funcNew()
        tabMain.SelectedTab = tabGeneral
        tempAcCode = Nothing
        cardCode = Nothing
        objGPack.TextClear(Me)
        'objGPack.TextClear(grpInfo)
        funcLoadCGSTDefaultAcCode()
        funcLoadSGSTDefaultAcCode()
        funcLoadIGSTDefaultAcCode()
        funcLoadRCGSTDefaultAcCode()
        funcLoadRIGSTDefaultAcCode()
        funcLoadRSGSTDefaultAcCode()
        funcCallGrid()
        flagSave = False
        txtShortName__Man.Focus()
        cmbActive.Text = "YES"
        Return 0
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcSave() As Integer
        '        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        '        If objGPack.Validator_Check(Me) Then Exit Function
        If txtCardName__Man.Text = "" Or txtShortName__Man.Text = "" Or cmbCGSTDefaultAcCode.Text = "" Or cmbIGSTDefaultAcCode.Text = "" Or cmbSGSTDefaultAcCode.Text = "" Then
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
        'Dim DefaultCGSTAcCode As String = Nothing
        Dim DefaultCGSTAcCode As String = Nothing
        Dim DefaultSGSTAcCode As String = Nothing
        Dim DefaultIGSTAcCode As String = Nothing
        Dim DefaultRCGSTAcCode As String = Nothing
        Dim DefaultRSGSTAcCode As String = Nothing
        Dim DefaultRIGSTAcCode As String = Nothing
        Dim PrizeAc As String = Nothing
        Dim BonusAc As String = Nothing
        Dim DeductAc As String = Nothing
        Dim CompanyId As String = Nothing
        Dim SchemeId As String = Nothing
        Dim GSTCatid As Integer
        Dim rdActive As String
        Try
            tran = cn.BeginTransaction()
            strSql = " select max(substring(accode,4,10)) as Accode from " & cnAdminDb & "..achead where Acgrpcode=12 and substring(accode,1,3) ='GST' and accode <>'GSTIN'"
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(ds, "Accode")
            CardCode = Val(ds.Tables("Accode").Rows(0).Item("Accode").ToString) + 1
            AcCode = "GST" + Replace(Space(4 - Len(CardCode)), " ", 0) & CardCode

            strSql = " select top 1 AcCode from " & cnAdminDb & "..achead where ACName = '" & txtCardName__Man.Text & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(ds, "AcCodeExist")
            If ds.Tables("AcCodeExist").Rows.Count > 0 Then
                DefaultCGSTAcCode = ds.Tables("DefaultCgstAcCode").Rows(0).Item("AcCode").ToString
                msinsert = False
            Else
                msinsert = True
            End If

            'CGST ACCODE
            strSql = " select top 1 AcCode from " & cnAdminDb & "..achead where ACName = '" & cmbCGSTDefaultAcCode.Text & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(ds, "DefaultCgstAcCode")
            If ds.Tables("DefaultCgstAcCode").Rows.Count > 0 Then                
                DefaultCGSTAcCode = ds.Tables("DefaultCgstAcCode").Rows(0).Item("AcCode").ToString                
            End If
            'SGST ACCODE
            strSql = " select top 1 AcCode from " & cnAdminDb & "..achead where ACName = '" & cmbSGSTDefaultAcCode.Text & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(ds, "DefaultSgstAcCode")
            If ds.Tables("DefaultSgstAcCode").Rows.Count > 0 Then                
                DefaultSGSTAcCode = ds.Tables("DefaultSgstAcCode").Rows(0).Item("AcCode").ToString            
            End If
            'IGST ACCODE
            strSql = " select top 1 AcCode from " & cnAdminDb & "..achead where ACName = '" & cmbIGSTDefaultAcCode.Text & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(ds, "DefaultIgstAcCode")
            If ds.Tables("DefaultIgstAcCode").Rows.Count > 0 Then                
                DefaultIGSTAcCode = ds.Tables("DefaultIgstAcCode").Rows(0).Item("AcCode").ToString                
            End If

            'RCM CGST ACCODE
            strSql = " select top 1 AcCode from " & cnAdminDb & "..achead where ACName = '" & cmbRCGSTDefaultAcCode.Text & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(ds, "DefaultRCgstAcCode")
            If ds.Tables("DefaultRCgstAcCode").Rows.Count > 0 Then
                DefaultRCGSTAcCode = ds.Tables("DefaultRCgstAcCode").Rows(0).Item("AcCode").ToString
            End If
            'RCM SGST ACCODE
            strSql = " select top 1 AcCode from " & cnAdminDb & "..achead where ACName = '" & cmbRSGSTDefaultAcCode.Text & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(ds, "DefaultRSgstAcCode")
            If ds.Tables("DefaultRSgstAcCode").Rows.Count > 0 Then
                DefaultRSGSTAcCode = ds.Tables("DefaultRSgstAcCode").Rows(0).Item("AcCode").ToString
            End If
            'RCM IGST ACCODE
            strSql = " select top 1 AcCode from " & cnAdminDb & "..achead where ACName = '" & cmbRGSTDefaultAcCode.Text & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(ds, "DefaultRIgstAcCode")
            If ds.Tables("DefaultRIgstAcCode").Rows.Count > 0 Then
                DefaultRIGSTAcCode = ds.Tables("DefaultRIgstAcCode").Rows(0).Item("AcCode").ToString
            End If



            strSql = " select 1 as Accode from " & cnAdminDb & "..GSTcategory where GSTcatname='" & Trim(txtCardName__Man.Text) & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(ds, "Accode")
            If ds.Tables("Accode").Rows.Count > 0 Then
                Tblinsert = True
            Else
                Tblinsert = False
            End If

            strSql = " select isnull(max(GSTcatid),0) + 1 as GSTcatid from " & cnAdminDb & "..GSTcategory "
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(ds, "GSTcat")
            If ds.Tables("GSTcat").Rows.Count > 0 Then
                GSTCatid = Val(ds.Tables("GSTcat").Rows(0).Item("GSTcatid").ToString)
            Else
                GSTCatid = 1
            End If
            If cmbActive.Text = "YES" Then
                rdActive = "Y"
            Else
                rdActive = "N"
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
                strSql += " '" & AcCode & "','" & txtCardName__Man.Text & "','12','0',"
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
                strSql = " insert into " & cnAdminDb & "..GSTCATEGORY( GSTCATID,GSTCATNAME,GSTCATEGORY,CGSTAC,SGSTAC,IGSTAC,"
                If rdActive = "N" Then strSql += " RCGSTAC,RSGSTAC,RIGSTAC,"
                strSql += " DISPLAYORDER,ACCODE,GSTPER,RD)"
                strSql += " Values("
                strSql += "" & GSTCatid & ",'" & txtCardName__Man.Text & "','" & txtShortName__Man.Text & "','" & DefaultCGSTAcCode & "','" & DefaultSGSTAcCode & "','" & DefaultIGSTAcCode & "',"
                If rdActive = "N" Then strSql += " '" & DefaultRCGSTAcCode & "','" & DefaultRSGSTAcCode & "','" & DefaultRIGSTAcCode & "',"
                strSql += " '" & txtorderid.Text & "','" & AcCode & "'," & Val(txtGSTCharge.Text) & ",'" & rdActive & "')"
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
        Dim rdActive As String
        Dim tran As OleDbTransaction = Nothing

        Try
            tran = cn.BeginTransaction()

            strSql = " select GSTCATNAME as AcCode from " & cnAdminDb & "..GSTCATEGORY where GSTCATNAME = '" & txtCardName__Man.Text & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(ds, "DefaultAcCode")
            If ds.Tables("DefaultAcCode").Rows.Count > 0 Then
                DefaultAcCode = ds.Tables("DefaultAcCode").Rows(0).Item("AcCode").ToString
            Else
                DefaultAcCode = tempAcCode
            End If
            If cmbActive.Text = "YES" Then
                rdActive = "Y"
            Else
                rdActive = "N"
            End If
            '''''''''''''''''''''''
            strSql = " Update " & cnAdminDb & "..GSTCATEGORY Set "
            strSql += " GSTCATEGORY = '" & txtShortName__Man.Text & "'"
            strSql += " ,DISPLAYORDER = '" & txtorderid.Text & "'"
            strSql += " ,CGSTAC = (SELECT ACCODE  FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbCGSTDefaultAcCode.Text & "')"
            strSql += " ,SGSTAC = (SELECT ACCODE  FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbSGSTDefaultAcCode.Text & "')"
            strSql += " ,IGSTAC = (SELECT ACCODE  FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbIGSTDefaultAcCode.Text & "')"
            If rdActive = "N" Then
                strSql += " ,RCGSTAC = (SELECT ACCODE  FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbRCGSTDefaultAcCode.Text & "')"
                strSql += " ,RSGSTAC = (SELECT ACCODE  FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbRSGSTDefaultAcCode.Text & "')"
                strSql += " ,RIGSTAC = (SELECT ACCODE  FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & cmbRGSTDefaultAcCode.Text & "')"
            End If            
            strSql += " ,GSTPER = '" & Val(txtGSTCharge.Text) & "'"
            strSql += " ,RD = '" & rdActive & "'"
            strSql += " where GSTCATNAME ='" & txtCardName__Man.Text & "'"
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

    Function funcLoadCGSTDefaultAcCode()
        Dim dt As New DataTable
        dt.Clear()
        cmbCGSTDefaultAcCode.Items.Clear()
        cmbCGSTDefaultAcCode.Text = ""
        strSql = " SELECT ACNAME AS NAME FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE='O'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        Dim cnt As Integer = Nothing
        For cnt = 0 To dt.Rows.Count - 1
            With dt.Rows(cnt)
                cmbCGSTDefaultAcCode.Items.Add(.Item("Name").ToString)
            End With
        Next
        cmbCGSTDefaultAcCode.Text = dt.Rows(0).Item("Name").ToString
        Return 0
    End Function


    Function funcLoadSGSTDefaultAcCode()
        Dim dt As New DataTable
        dt.Clear()
        cmbSGSTDefaultAcCode.Items.Clear()
        cmbSGSTDefaultAcCode.Text = ""
        strSql = " SELECT ACNAME AS NAME FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE='O'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        Dim cnt As Integer = Nothing
        For cnt = 0 To dt.Rows.Count - 1
            With dt.Rows(cnt)
                cmbSGSTDefaultAcCode.Items.Add(.Item("Name").ToString)
            End With
        Next
        cmbSGSTDefaultAcCode.Text = dt.Rows(0).Item("Name").ToString
        Return 0
    End Function

    Function funcLoadIGSTDefaultAcCode()
        Dim dt As New DataTable
        dt.Clear()
        cmbIGSTDefaultAcCode.Items.Clear()
        cmbIGSTDefaultAcCode.Text = ""
        strSql = " SELECT ACNAME AS NAME FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE='O'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        Dim cnt As Integer = Nothing
        For cnt = 0 To dt.Rows.Count - 1
            With dt.Rows(cnt)
                cmbIGSTDefaultAcCode.Items.Add(.Item("Name").ToString)
            End With
        Next
        cmbIGSTDefaultAcCode.Text = dt.Rows(0).Item("Name").ToString
        Return 0
    End Function

    Function funcLoadRCGSTDefaultAcCode()
        Dim dt As New DataTable
        dt.Clear()
        cmbRCGSTDefaultAcCode.Items.Clear()
        cmbRCGSTDefaultAcCode.Text = ""
        strSql = " SELECT ACNAME AS NAME FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE='O'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        Dim cnt As Integer = Nothing
        For cnt = 0 To dt.Rows.Count - 1
            With dt.Rows(cnt)
                cmbRCGSTDefaultAcCode.Items.Add(.Item("Name").ToString)
            End With
        Next
        cmbRCGSTDefaultAcCode.Text = dt.Rows(0).Item("Name").ToString
        Return 0
    End Function


    Function funcLoadRSGSTDefaultAcCode()
        Dim dt As New DataTable
        dt.Clear()
        cmbRSGSTDefaultAcCode.Items.Clear()
        cmbRSGSTDefaultAcCode.Text = ""
        strSql = " SELECT ACNAME AS NAME FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE='O'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        Dim cnt As Integer = Nothing
        For cnt = 0 To dt.Rows.Count - 1
            With dt.Rows(cnt)
                cmbRSGSTDefaultAcCode.Items.Add(.Item("Name").ToString)
            End With
        Next
        cmbRSGSTDefaultAcCode.Text = dt.Rows(0).Item("Name").ToString
        Return 0
    End Function

    Function funcLoadRIGSTDefaultAcCode()
        Dim dt As New DataTable
        dt.Clear()
        cmbRGSTDefaultAcCode.Items.Clear()
        cmbRGSTDefaultAcCode.Text = ""
        strSql = " SELECT ACNAME AS NAME FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE='O'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        Dim cnt As Integer = Nothing
        For cnt = 0 To dt.Rows.Count - 1
            With dt.Rows(cnt)
                cmbRGSTDefaultAcCode.Items.Add(.Item("Name").ToString)
            End With
        Next
        cmbRGSTDefaultAcCode.Text = dt.Rows(0).Item("Name").ToString
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


        txtGSTCharge.Enabled = False
        txtGSTCharge.Enabled = False

        GrpFieldPos()
        funcLoadCGSTDefaultAcCode()
        funcLoadIGSTDefaultAcCode()
        funcLoadSGSTDefaultAcCode()
        funcLoadRCGSTDefaultAcCode()
        funcLoadRIGSTDefaultAcCode()
        funcLoadRSGSTDefaultAcCode()
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
            strSql = " SELECT DISPLAYORDER,GSTCATID,GSTCATNAME,GSTCATEGORY,ACCODE,"
            strSql += " (SELECT top 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=TC.CGSTAC) AS CGSTAC,"
            strSql += " (SELECT top 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=TC.SGSTAC) AS SGSTAC,"
            strSql += " (SELECT top 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=TC.IGSTAC) AS IGSTAC,"
            strSql += " (SELECT top 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=TC.RCGSTAC) AS RCGSTAC,"
            strSql += " (SELECT top 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=TC.RSGSTAC) AS RSGSTAC,"
            strSql += " (SELECT top 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=TC.RIGSTAC) AS RIGSTAC,"
            strSql += " CASE WHEN RD='Y' THEN 'YES' ELSE 'NO' END RD,"
            strSql += " GSTPER,DISPLAYORDER "
            strSql += " FROM	" & cnAdminDb & "..GSTCATEGORY TC where GSTCATNAME = '" & tempCardCode & "'"

            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If Not dt.Rows.Count > 0 Then
                Return 0
            End If
            With dt.Rows(0)
                Ediordig = .Item("DISPLAYORDER").ToString
                txtCardName__Man.Text = .Item("GSTCATNAME").ToString
                txtShortName__Man.Text = .Item("GSTCATEGORY").ToString
                txtorderid.Text = .Item("DISPLAYORDER").ToString
                cmbCGSTDefaultAcCode.Text = .Item("CGSTAC").ToString
                cmbSGSTDefaultAcCode.Text = .Item("SGSTAC").ToString
                cmbIGSTDefaultAcCode.Text = .Item("IGSTAC").ToString
                cmbRCGSTDefaultAcCode.Text = .Item("RCGSTAC").ToString
                cmbRSGSTDefaultAcCode.Text = .Item("RSGSTAC").ToString
                cmbRGSTDefaultAcCode.Text = .Item("RIGSTAC").ToString
                txtGSTCharge.Text = .Item("GSTPER").ToString
                cmbActive.Text = .Item("RD").ToString
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

    Private Sub txtSurcharge_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        Dim keyChar As String
        keyChar = e.KeyChar
        If AscW(e.KeyChar) = 46 Then
            If txtGSTCharge.Text.Contains(".") = True Then
                e.Handled = True
            End If
        End If
        If (Not Char.IsDigit(keyChar)) And AscW(keyChar) <> 8 And AscW(keyChar) <> 13 And AscW(keyChar) <> 46 Then
            e.Handled = True
            MsgBox("Digits only Allowed 1 to 9")
            txtGSTCharge.Focus()
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
                cmbCGSTDefaultAcCode.Items.Clear()
                cmbSGSTDefaultAcCode.Items.Clear()
                cmbIGSTDefaultAcCode.Items.Clear()
                funcLoadCGSTDefaultAcCode()
                funcLoadSGSTDefaultAcCode()
                funcLoadIGSTDefaultAcCode()
                funcLoadRCGSTDefaultAcCode()
                funcLoadRSGSTDefaultAcCode()
                funcLoadRIGSTDefaultAcCode()
                'cmbCGSTDefaultAcCode.Items.Add(txtCardName__Man.Text)
                'cmbCGSTDefaultAcCode.Text = txtCardName__Man.Text
            Else
                funcLoadCGSTDefaultAcCode()
                funcLoadIGSTDefaultAcCode()
                funcLoadSGSTDefaultAcCode()
                funcLoadRCGSTDefaultAcCode()
                funcLoadRSGSTDefaultAcCode()
                funcLoadRIGSTDefaultAcCode()
                'cmbCGSTDefaultAcCode.Text = txtCardName__Man.Text
            End If
        End If
    End Sub
    Private Sub txtorderid_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtorderid.KeyPress        
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtorderid.Text <> "" Then
                If flagSave = True And Ediordig = txtorderid.Text Then
                Else
                    Dim dt As New DataTable
                    dt.Clear()
                    strSql = " Select DISPLAYORDER from " & cnAdminDb & "..GSTCATEGORY where DISPLAYORDER = '" & txtorderid.Text & "'"
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
End Class