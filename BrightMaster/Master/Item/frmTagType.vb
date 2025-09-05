Imports System.Data.OleDb
Public Class frmTagType
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim flagSave As Boolean = False
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        funcGridStyle(gridView)
        objGPack.TextClear(grpInfo)
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        tabGeneral.BackgroundImage = bakImage
        tabView.BackgroundImage = bakImage
        tabGeneral.BackgroundImageLayout = ImageLayout.Stretch
        tabGeneral.BackgroundImageLayout = ImageLayout.Stretch

        txtItemTypeId_Num_Man.Enabled = False
        pnlGrid.Dock = DockStyle.Fill


        cmbSoftModule.Items.Add("STOCK")
        cmbSoftModule.Items.Add("SALES")
        cmbSoftModule.Items.Add("ESTIMATION")
        cmbSoftModule.Text = "STOCK"

        cmbRateGet.Items.Add("YES")
        cmbRateGet.Items.Add("NO")
        cmbRateGet.Text = "NO"

        cmbAutoWtTransfer.Items.Add("YES")
        cmbAutoWtTransfer.Items.Add("NO")
        cmbAutoWtTransfer.Text = "NO"
        funcNew()
    End Sub

    Function funcNew() As Integer
        Dim dt As New DataTable
        dt.Clear()
        objGPack.TextClear(Me)
        cmbPurityName_Man.Text = ""
        funcLoadTableCode()
        txtItemTypeId_Num_Man.Text = objGPack.GetMax("ItemTypeId", "ItemType", cnAdminDb)
        cmbSoftModule.Text = "STOCK"
        cmbRateGet.Text = "NO"
        tabMain.SelectedTab = tabGeneral
        txtName__Man.Select()
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        If objGPack.DupChecker(txtName__Man, "SELECT 1 FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & txtName__Man.Text & "' AND ITEMTYPEID <> '" & txtItemTypeId_Num_Man.Text & "'") Then Exit Function
        If flagSave = False Then
            funcAdd()
        ElseIf flagSave = True Then
            funcUpdate()
        End If
    End Function
    Function funcOpen() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Function
        tabMain.SelectedTab = tabView
        strSql = " SELECT"
        strSql += " ITEMTYPEID,NAME,SNAME,"
        strSql += " DISPALYORDER,"
        strSql += " CASE WHEN SOFTMODULE = 'S' THEN 'STOCK'"
        strSql += " WHEN SOFTMODULE = 'A' THEN 'SALES' ELSE 'ESTIMATION' END SOFTMODULE,"
        strSql += " CASE WHEN RATEGET = 'Y' THEN 'YES' ELSE 'NO' END RATEGET,"
        strSql += " TABLECODE,"
        strSql += " RATEADDMCGRM,"
        strSql += " ISNULL((SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY AS C WHERE C.CATCODE = I.CATCODE),'')AS CATNAME, "
        strSql += " ISNULL((SELECT PURITYNAME FROM " & cnAdminDb & "..PURITYMAST AS P WHERE P.PURITYID = I.PURITYID),'')AS PURITYNAME,"
        strSql += " OWNPURLESSPER,OWNPURLESSAMT,OWNEXLESSPER,"
        strSql += " OWNEXLESSAMT,OTHPURLESSPER,OTHPURLESSAMT,"
        strSql += " OTHEXLESSPER,OTHEXLESSAMT, "
        strSql += " OWNWASTPER,OTHWASTPER, "
        strSql += " OWNWASTPERMIN,OTHWASTPERMIN "
        strSql += " ,OWNPURDIALESSPER,OWNPURDIALESSAMT,OWNEXDIALESSPER,OWNEXDIALESSAMT"
        strSql += " ,OTHPURDIALESSPER,OTHPURDIALESSAMT,OTHEXDIALESSPER,OTHEXDIALESSAMT"
        strSql += " FROM " & cnAdminDb & "..ITEMTYPE AS I"
        funcOpenGrid(strSql, gridView)
        gridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        With gridView
            .Columns("ITEMTYPEID").HeaderText = "ID"
            .Columns("ITEMTYPEID").Width = 50
            .Columns("NAME").Width = 300
            .Columns("SNAME").HeaderText = "SHORT NAME"
            .Columns("SNAME").Width = 150
            .Columns("DISPALYORDER").HeaderText = "ORDER"
            .Columns("DISPALYORDER").Width = 50
            .Columns("SOFTMODULE").Width = 90
            .Columns("RATEGET").Width = 60
            .Columns("TABLECODE").Width = 80
        End With
        For CNT As Integer = 7 To gridView.ColumnCount - 1
            gridView.Columns(CNT).Visible = False
        Next
        gridView.Select()
    End Function
    Function funcAdd() As Integer
        Dim CatCode As String = Nothing
        Dim PurityId As String = Nothing
        Dim softModule As String = Nothing

        If cmbSoftModule.Text = "STOCK" Then
            softModule = "S"
        ElseIf cmbSoftModule.Text = "SALES" Then
            softModule = "A"
        Else
            softModule = "E"
        End If

        Dim ds As New Data.DataSet
        ds.Clear()
        If cmbRateGet.Text = "YES" Then
            PurityId = objGPack.GetSqlValue("SELECT DISTINCT PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYNAME = '" & cmbPurityName_Man.Text & "'")
            CatCode = objGPack.GetSqlValue("SELECT TOP 1 CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE PURITYID = '" & PurityId & "'")
        Else
            If cmbCategory.Text <> "ALL" Then
                CatCode = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbCategory.Text & "'")
            End If
            PurityId = ""
        End If

        If pnlEstimation.Enabled = False Then
            txtOwnPurLess_Per.Clear()
            txtOwnPurLess_Amt.Clear()
            txtOwnExLess_Per.Clear()
            txtOwnExLess_Amt.Clear()

            txtOtherPurLess_Per.Clear()
            txtOtherPurLess_Amt.Clear()
            txtOtherExLess_Per.Clear()
            txtOtherExLess_Amt.Clear()
            txtOwnWast.Clear()
            txtOthWast.Clear()
        End If

        strSql = " Insert into " & cnAdminDb & "..ItemType"
        strSql += " ("
        strSql += " ItemTypeID,Name,SName,DispalyOrder,"
        strSql += " SoftModule,RateGet,Catcode,PurityId,"
        strSql += " RateAddMcGrm,TableCode,OwnPurLessPer,OwnPurLessAmt,"
        strSql += " OwnExLessPer,OwnExLessAmt,OthPurLessPer,"
        strSql += " OthPurLessAmt,OthExLessPer,OthexLessAmt,"
        strSql += " OWNWASTPER,OTHWASTPER, "
        strSql += " OWNWASTPERMIN,OTHWASTPERMIN, "
        strSql += " OWNPURDIALESSPER,OWNPURDIALESSAMT,OWNEXDIALESSPER,OWNEXDIALESSAMT, "
        strSql += " OTHPURDIALESSPER,OTHPURDIALESSAMT,OTHEXDIALESSPER,OTHEXDIALESSAMT, "
        strSql += " UserId,Updated,Uptime,AUTOWTTRF"
        strSql += " )Values ("
        strSql += " '" & txtItemTypeId_Num_Man.Text & "'" 'ItemTypeID
        strSql += " ,'" & txtName__Man.Text & "'" 'Name
        strSql += " ,'" & txtShortName.Text & "'" 'SName
        strSql += " ,'" & txtDisplayOrder_Num.Text & "'" 'DispalyOrder
        strSql += " ,'" & softModule & "'" 'SoftModule
        strSql += " ,'" & Mid(cmbRateGet.Text, 1, 1) & "'" 'RateGet
        strSql += " ,'" & CatCode & "'" 'Catcode
        strSql += " ,'" & PurityId & "'" 'PurityId
        strSql += " ," & Val(txtRateAddMcGrm_Num.Text) & "" 'RateAddMcGrm
        strSql += " ,'" & cmbTableCode.Text & "'" 'TableCode
        strSql += " ," & Val(txtOwnPurLess_Per.Text) & "" 'OwnPurLessPer
        strSql += " ," & Val(txtOwnPurLess_Amt.Text) & "" 'OwnPurLessAmt
        strSql += " ," & Val(txtOwnExLess_Per.Text) & "" 'OwnExLessPer
        strSql += " ," & Val(txtOwnExLess_Amt.Text) & "" 'OwnExLessAmt
        strSql += " ," & Val(txtOtherPurLess_Per.Text) & "" 'OthPurLessPer
        strSql += " ," & Val(txtOtherPurLess_Amt.Text) & "" 'OthPurLessAmt
        strSql += " ," & Val(txtOtherExLess_Per.Text) & "" 'OthExLessPer
        strSql += " ," & Val(txtOtherExLess_Amt.Text) & "" 'OthexLessAmt
        strSql += " ," & Val(txtOwnWast.Text) & "" 'OthExLessPer
        strSql += " ," & Val(txtOthWast.Text) & "" 'OthexLessAmt
        strSql += " ," & Val(txtOwnWast1.Text) & "" 'OthExLessPer
        strSql += " ," & Val(txtOthWast1.Text) & "" 'OthexLessAmt
        strSql += " ," & Val(txtOwnStudLess_Per.Text) & "" 'OWNPURDIALESSPER
        strSql += " ," & Val(txtOwnStudLess_Amt.Text) & "" 'OWNPURDIALESSAMT
        strSql += " ," & Val(txtOwnExStudLess_Per.Text) & "" 'OWNEXDIALESSPER
        strSql += " ," & Val(txtOwnExStudLess_Amt.Text) & "" 'OWNEXDIALESSAMT
        strSql += " ," & Val(txtOtherStudLess_Per.Text) & "" 'OTHPURDIALESSPER
        strSql += " ," & Val(txtOtherStudLess_Amt.Text) & "" 'OTHPURDIALESSAMT
        strSql += " ," & Val(txtOtherExStudLess_Per.Text) & "" 'OTHEXDIALESSPER
        strSql += " ," & Val(txtOtherExStudLess_Amt.Text) & "" 'OTHEXDIALESSAMT
        strSql += " ,'" & userId & "'" 'UserId
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Updated
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'Uptime
        strSql += " ,'" & Mid(cmbAutoWtTransfer.Text, 1, 1) & "'" 'Auto Weight Transfer
        strSql += " )"
        Try
            ExecQuery(SyncMode.Master, strSql, cn)
            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcUpdate() As Integer
        Dim CatCode As String = Nothing
        Dim PurityId As String = Nothing
        Dim softModule As String = Nothing

        If cmbSoftModule.Text = "STOCK" Then
            softModule = "S"
        ElseIf cmbSoftModule.Text = "SALES" Then
            softModule = "A"
        Else
            softModule = "E"
        End If

        Dim ds As New Data.DataSet
        ds.Clear()
        If cmbRateGet.Text = "YES" Then
            strSql = " Select PurityId from " & cnAdminDb & "..PurityMast where PurityName = '" & cmbPurityName_Man.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(ds, "PurityId")
            If ds.Tables("PurityId").Rows.Count > 0 Then
                PurityId = ds.Tables("PurityId").Rows(0).Item("PurityId")
            End If
        Else
            
            PurityId = ""
        End If
        If cmbCategory.Text <> "ALL" Then
            CatCode = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbCategory.Text & "'")
        End If
        If pnlEstimation.Enabled = False Then
            txtOwnPurLess_Per.Clear()
            txtOwnPurLess_Amt.Clear()
            txtOwnExLess_Per.Clear()
            txtOwnExLess_Amt.Clear()

            txtOtherPurLess_Per.Clear()
            txtOtherPurLess_Amt.Clear()
            txtOtherExLess_Per.Clear()
            txtOtherExLess_Amt.Clear()
            txtOwnWast.Clear()
            txtOthWast.Clear()
            txtOwnWast1.Clear()
            txtOthWast1.Clear()
        End If


        strSql = " Update " & cnAdminDb & "..ItemType Set"
        strSql += " Name='" & txtName__Man.Text & "'"
        strSql += " ,SName='" & txtShortName.Text & "'"
        strSql += " ,DispalyOrder='" & txtDisplayOrder_Num.Text & "'"
        strSql += " ,SoftModule='" & softModule & "'"
        strSql += " ,RateGet='" & Mid(cmbRateGet.Text, 1, 1) & "'"
        strSql += " ,Catcode='" & CatCode & "'"
        strSql += " ,PurityId='" & PurityId & "'"
        strSql += " ,RateAddMcGrm='" & Val(txtRateAddMcGrm_Num.Text) & "'"
        strSql += " ,TableCode='" & cmbTableCode.Text & "'"
        strSql += " ,OwnPurLessPer='" & Val(txtOwnPurLess_Per.Text) & "'"
        strSql += " ,OwnPurLessAmt='" & Val(txtOwnPurLess_Amt.Text) & "'"
        strSql += " ,OwnExLessPer='" & Val(txtOwnExLess_Per.Text) & "'"
        strSql += " ,OwnExLessAmt='" & Val(txtOwnExLess_Amt.Text) & "'"
        strSql += " ,OthPurLessPer='" & Val(txtOtherPurLess_Per.Text) & "'"
        strSql += " ,OthPurLessAmt='" & Val(txtOtherPurLess_Amt.Text) & "'"
        strSql += " ,OthExLessPer='" & Val(txtOtherExLess_Per.Text) & "'"
        strSql += " ,OthexLessAmt='" & Val(txtOtherExLess_Amt.Text) & "'"
        strSql += " ,OthwastPER='" & Val(txtOthWast.Text) & "'"
        strSql += " ,OwnwastPER='" & Val(txtOwnWast.Text) & "'"
        strSql += " ,OthwastPERMIN='" & Val(txtOthWast1.Text) & "'"
        strSql += " ,OwnwastPERMIN='" & Val(txtOwnWast1.Text) & "'"
        strSql += " ,UserId='" & userId & "'"
        strSql += " ,Updated='" & Today.Date.ToString("yyyy-MM-dd") & "'"
        strSql += " ,Uptime='" & Date.Now.ToLongTimeString & "'"
        strSql += " ,AUTOWTTRF='" & Mid(cmbAutoWtTransfer.Text, 1, 1) & "'"
        strSql += " ,OWNPURDIALESSPER='" & Val(txtOwnStudLess_Per.Text) & "'"
        strSql += " ,OWNPURDIALESSAMT='" & Val(txtOwnStudLess_Amt.Text) & "'"
        strSql += " ,OWNEXDIALESSPER='" & Val(txtOwnExStudLess_Per.Text) & "'"
        strSql += " ,OWNEXDIALESSAMT='" & Val(txtOwnExStudLess_Amt.Text) & "'"
        strSql += " ,OTHPURDIALESSPER='" & Val(txtOtherStudLess_Per.Text) & "'"
        strSql += " ,OTHPURDIALESSAMT='" & Val(txtOtherStudLess_Amt.Text) & "'"
        strSql += " ,OTHEXDIALESSPER='" & Val(txtOtherExStudLess_Per.Text) & "'"
        strSql += " ,OTHEXDIALESSAMT='" & Val(txtOtherExStudLess_Amt.Text) & "'"
        strSql += " Where ItemTypeId = '" & txtItemTypeId_Num_Man.Text & "'"
        Try
            ExecQuery(SyncMode.Master, strSql, cn)
            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcGetDetails(ByVal tempItemTypeId As String) As Integer
        Dim dt As New DataTable
        dt.Clear()
        strSql = " SELECT"
        strSql += " ITEMTYPEID,NAME,SNAME,"
        strSql += " DISPALYORDER,"
        strSql += " CASE WHEN SOFTMODULE = 'S' THEN 'STOCK'"
        strSql += " WHEN SOFTMODULE = 'A' THEN 'SALES' ELSE 'ESTIMATION' END SOFTMODULE,"
        strSql += " CASE WHEN RATEGET = 'Y' THEN 'YES' ELSE 'NO' END RATEGET,"
        strSql += " ISNULL((SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY AS C WHERE C.CATCODE = I.CATCODE),'')AS CATNAME, "
        strSql += " ISNULL((SELECT PURITYNAME FROM " & cnAdminDb & "..PURITYMAST AS P WHERE P.PURITYID = I.PURITYID),'')AS PURITYNAME,"
        strSql += " RATEADDMCGRM,"
        strSql += " TABLECODE,"
        strSql += " OWNPURLESSPER,OWNPURLESSAMT,OWNEXLESSPER,"
        strSql += " OWNEXLESSAMT,OTHPURLESSPER,OTHPURLESSAMT,"
        strSql += " OTHEXLESSPER,OTHEXLESSAMT, "
        strSql += " OTHWASTPER,OWNWASTPER, "
        strSql += " OTHWASTPERMIN,OWNWASTPERMIN "
        strSql += " ,CASE WHEN AUTOWTTRF = 'Y' THEN 'YES' ELSE 'NO' END AS AUTOWTTRF "
        strSql += " ,OWNPURDIALESSPER,OWNPURDIALESSAMT,OWNEXDIALESSPER,OWNEXDIALESSAMT "
        strSql += " ,OTHPURDIALESSPER,OTHPURDIALESSAMT,OTHEXDIALESSPER,OTHEXDIALESSAMT "
        strSql += " FROM " & cnAdminDb & "..ITEMTYPE AS I"
        strSql += " WHERE ITEMTYPEID = '" & tempItemTypeId & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        With dt.Rows(0)
            txtItemTypeId_Num_Man.Text = .Item("ItemTypeID").ToString
            txtName__Man.Text = .Item("Name").ToString
            txtShortName.Text = .Item("SName").ToString
            txtDisplayOrder_Num.Text = .Item("DispalyOrder").ToString
            cmbSoftModule.Text = .Item("SoftModule").ToString
            cmbRateGet.Text = .Item("RateGet").ToString
            cmbPurityName_Man.Text = .Item("PurityName").ToString
            txtRateAddMcGrm_Num.Text = .Item("RateAddMcGrm").ToString
            cmbTableCode.Text = .Item("TableCode").ToString
            txtOwnPurLess_Per.Text = .Item("OwnPurLessPer").ToString
            txtOwnPurLess_Amt.Text = .Item("OwnPurLessAmt").ToString
            txtOwnExLess_Per.Text = .Item("OwnExLessPer").ToString
            txtOwnExLess_Amt.Text = .Item("OwnExLessAmt").ToString
            txtOtherPurLess_Per.Text = .Item("OthPurLessPer").ToString
            txtOtherPurLess_Amt.Text = .Item("OthPurLessAmt").ToString
            txtOtherExLess_Per.Text = .Item("OthExLessPer").ToString
            txtOtherExLess_Amt.Text = .Item("OthexLessAmt").ToString
            txtOthWast.Text = .Item("OthWASTPER").ToString
            txtOwnWast.Text = .Item("OWNWASTPER").ToString
            txtOthWast1.Text = .Item("OthWASTPERMIN").ToString
            txtOwnWast1.Text = .Item("OWNWASTPERMIN").ToString
            cmbAutoWtTransfer.Text = .Item("AUTOWTTRF").ToString
            cmbCategory.Text = .Item("CATNAME").ToString
            txtOwnStudLess_Per.Text = .Item("OWNPURDIALESSPER").ToString
            txtOwnStudLess_Amt.Text = .Item("OWNPURDIALESSAMT").ToString
            txtOwnExStudLess_Per.Text = .Item("OWNEXDIALESSPER").ToString
            txtOwnExStudLess_Amt.Text = .Item("OWNEXDIALESSAMT").ToString
            txtOtherStudLess_Per.Text = .Item("OTHPURDIALESSPER").ToString
            txtOtherStudLess_Amt.Text = .Item("OTHPURDIALESSAMT").ToString
            txtOtherExStudLess_Per.Text = .Item("OTHEXDIALESSPER").ToString
            txtOtherExStudLess_Amt.Text = .Item("OTHEXDIALESSAMT").ToString
        End With
        flagSave = True
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcLoadTableCode() As Integer
        strSql = " select distinct(tablecode) from " & cnAdminDb & "..wmctable"
        objGPack.FillCombo(strSql, cmbTableCode)
    End Function

    Private Sub frmTagType_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Escape) Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                tabMain.SelectedTab = tabGeneral
                txtName__Man.Focus()
            End If
        ElseIf e.KeyChar = Chr(Keys.Enter) Then
            If txtName__Man.Focused Then
                Exit Sub
            End If
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cmbRateGet_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbRateGet.SelectedIndexChanged
        If cmbRateGet.Text = "YES" Then
            Dim dt As New DataTable
            dt.Clear()
            strSql = "select distinct purityname from " & cnAdminDb & "..puritymast order by purityname"
            objGPack.FillCombo(strSql, cmbPurityName_Man)
            cmbPurityName_Man.Enabled = True
        Else
            cmbPurityName_Man.Items.Clear()
            cmbPurityName_Man.Enabled = False
            strSql = " Select 'ALL' as Category Union all "
            strSql = "select distinct CATNAME as Category from " & cnAdminDb & "..Category order by catname"
            objGPack.FillCombo(strSql, cmbCategory)
            cmbCategory.Enabled = True

        End If
    End Sub


    Private Sub cmbSoftModule_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSoftModule.SelectedIndexChanged
        If cmbSoftModule.Text = "ESTIMATION" Then
            pnlEstimation.Enabled = True
            lblAutowttransfer.Enabled = True
            cmbAutoWtTransfer.Enabled = True
        Else
            pnlEstimation.Enabled = False
            lblAutowttransfer.Enabled = False
            cmbAutoWtTransfer.Enabled = False
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        funcOpen()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        funcSave()
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        funcOpen()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        funcNew()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        funcExit()
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblStatus.Visible = True
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            gridView.CurrentCell = gridView.CurrentCell
            If gridView.RowCount > 0 Then
                funcGetDetails(gridView.Item(0, gridView.CurrentRow.Index).Value.ToString)
                tabMain.SelectedTab = tabGeneral
                txtName__Man.Focus()
            End If
        End If
    End Sub

    Private Sub txtName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtName__Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If objGPack.DupChecker(txtName__Man, "SELECT 1 FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & txtName__Man.Text & "' AND ITEMTYPEID <> '" & txtItemTypeId_Num_Man.Text & "'") Then
                Exit Sub
            Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus

        lblStatus.Visible = False
    End Sub

    Private Sub gridView_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.RowEnter
        objGPack.TextClear(grpInfo)
        With gridView.Rows(e.RowIndex)
            detPurName.Text = .Cells("PurityName").Value.ToString
            detRateAddMcGrm.Text = .Cells("RateAddMcGrm").Value.ToString
            detOwnPurLessPer.Text = .Cells("OwnPurLessPer").Value.ToString
            detOwnPurLessAmt.Text = .Cells("OwnPurLessAmt").Value.ToString
            detOwnExLessPer.Text = .Cells("OwnExLessPer").Value.ToString
            detOwnExLessAmt.Text = .Cells("OwnExLessAmt").Value.ToString
            detOthPurLessPer.Text = .Cells("OthPurLessPer").Value.ToString
            detOthPurLessAmt.Text = .Cells("OthPurLessAmt").Value.ToString
            detOthExLessPer.Text = .Cells("OthExLessPer").Value.ToString
            detOthExLessAmt.Text = .Cells("OthexLessAmt").Value.ToString
            txtOthdetWast.Text = .Cells("othwastper").Value.ToString
            txtOwndetWast.Text = .Cells("Ownwastper").Value.ToString
            txtOthdetWast1.Text = .Cells("othwastperMIN").Value.ToString
            txtOwndetWast1.Text = .Cells("OwnwastperMIN").Value.ToString
        End With
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        Dim chkQry As String = Nothing
        Dim delKey As String = gridView.Rows(gridView.CurrentRow.Index).Cells("ITEMTYPEID").Value.ToString

        strSql = " SELECT DBNAME FROM " & cnAdminDb & "..DBMASTER WHERE DBNAME IN(SELECT NAME FROM MASTER..SYSDATABASES)"
        Dim dtDb As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtDb)
        If dtDb.Rows.Count > 0 Then
            chkQry = " SELECT TOP 1 ITEMTYPE FROM " & cnAdminDb & "..WMCTABLE WHERE ITEMTYPE = '" & delKey & "'"
            chkQry += " UNION "
            chkQry += " SELECT TOP 1 ITEMTYPEID FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMTYPEID = '" & delKey & "'"
            chkQry += " UNION "
            chkQry += " SELECT TOP 1 ITEMTYPEID FROM " & cnAdminDb & "..ITEMNONTAG WHERE ITEMTYPEID = '" & delKey & "'"
            For cnt As Integer = 0 To dtDb.Rows.Count - 1
                With dtDb.Rows(cnt)
                    chkQry += " UNION "
                    chkQry += " SELECT TOP 1 ITEMTYPEID FROM " & .Item("DBNAME").ToString & "..ISSUE WHERE ITEMTYPEID = '" & delKey & "'"
                    chkQry += " UNION "
                    chkQry += " SELECT TOP 1 ITEMTYPEID FROM " & .Item("DBNAME").ToString & "..RECEIPT WHERE ITEMTYPEID = '" & delKey & "'"
                    chkQry += " UNION "
                    chkQry += " SELECT TOP 1 ITEMTYPEID FROM " & .Item("DBNAME").ToString & "..OPENITEM	 WHERE ITEMTYPEID = '" & delKey & "'"
                    chkQry += " UNION "
                    chkQry += " SELECT TOP 1 ITEMTYPEID FROM " & .Item("DBNAME").ToString & "..ESTISSUE WHERE ITEMTYPEID = '" & delKey & "'"
                    chkQry += " UNION "
                    chkQry += " SELECT TOP 1 ITEMTYPEID FROM " & .Item("DBNAME").ToString & "..ESTRECEIPT WHERE ITEMTYPEID = '" & delKey & "'"
                    'If cnt <> dtDb.Rows.Count - 1 Then    chkQry += " UNION "
                End With
            Next
            DeleteItem(SyncMode.Master, chkQry, "DELETE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = '" & delKey & "'")
            funcOpen()
        End If
    End Sub
End Class