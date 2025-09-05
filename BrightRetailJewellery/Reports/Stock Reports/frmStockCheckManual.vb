Imports System.Data.OleDb
Imports System.IO

Public Class frmStockCheckManual
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter

    Dim dtStockChecked As New DataTable("Checked")
    Dim defaultPic As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICPATH'")
    Dim dtStoneEmpty As New DataTable
    Dim cntTagNo As Integer = 0
    Dim cntPcs As Integer = 0
    Dim cntGrsWt As Double = 0
    Dim cntNetWt As Double = 0
    Private WithEvents btnExcelStockCheck As New Button
    Private WithEvents btnPrintStockCheck As New Button
    Private WithEvents btnExcelStone As New Button
    Private WithEvents btnPrintStone As New Button
    Dim PRODTAGSEP As String = GetAdmindbSoftValue("PRODTAGSEP")
    Dim BARCODE2DSEP As String = GetAdmindbSoftValue("BARCODE2DSEP")
    Dim chkwttotal As Boolean = IIf(GetAdmindbSoftValue("STKCHKREP_TOTWT", "N") = "Y", True, False)
    Dim Port_BaudRate As Integer = 9600
    Dim Port_DataBit As Integer = 8
    Dim Port_PortName As String = "COM1"
    Dim Port_Parity As String = "N"
    Dim objPropertyDia As New PropertyDia(SerialPort1)
    Dim dtCounter As New DataTable


    Function funcNew() As Integer
        ''Load ItemName
        lblProcess.Text = ""
        cmbItemName.Items.Clear()
        cmbItemName.Items.Add("ALL")
        strSql = " Select itemName from " & cnAdminDb & "..itemMast order by itemName"
        objGPack.FillCombo(strSql, cmbItemName, False)
        cmbItemName.Text = "ALL"

        ''Load Counter
        strSql = " SELECT 'ALL' ITEMCTRNAME,'ALL' ITEMCTRID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ITEMCTRNAME,CONVERT(vARCHAR,ITEMCTRID),2 RESULT FROM " & cnAdminDb & "..ITEMCOUNTER"
        strSql += " ORDER BY RESULT,ITEMCTRNAME"
        dtCounter = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCounter)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCounter, dtCounter, "ITEMCTRNAME", , "ALL")

        ''Load Designer
        cmbDesignerName.Items.Clear()
        cmbDesignerName.Items.Add("ALL")
        strSql = " Select DesignerName from " & cnAdminDb & "..Designer order by DesignerName"
        objGPack.FillCombo(strSql, cmbDesignerName, False)
        cmbDesignerName.Text = "ALL"

        dtStockChecked.Columns.Add("itemId", GetType(String))
        dtStockChecked.Columns.Add("TagNo", GetType(String))
        dtStockChecked.Columns.Add("Pcs", GetType(String))
        dtStockChecked.Columns.Add("GrsWt", GetType(String))
        dtStockChecked.Columns.Add("NetWt", GetType(String))
        dtStockChecked.Columns.Add("Rate", GetType(String))
        dtStockChecked.Columns.Add("Salvalue", GetType(String))
        dtStockChecked.Columns.Add("itemName", GetType(String))
        dtStockChecked.Columns.Add("subItemName", GetType(String))
        dtStockChecked.Columns.Add("CounterName", GetType(String))
        dtStockChecked.Columns.Add("Approval", GetType(String))
        dtStockChecked.Columns.Add("chkTray", GetType(String))
        dtStockChecked.Columns.Add("picFileName", GetType(String))

        strSql = " Select ''itemId,''TagNo,''Pcs,''GrsWt,''NetWt,''Rate,''Salvalue,''itemName,''subITemName,''CounterName,''Approval,''chkTray,''PicFileName"
        Dim dtEmpty As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtEmpty)
        gridStockChecked.DataSource = dtEmpty
        funcStockGridStyle()

        If Not chkwttotal Then pnlTotal.Visible = False : lblTotal.Visible = False

        strSql = " Select ''itemName,''subItemName,''stnPcs,''stnWt,''stnType"
        dtStoneEmpty.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtStoneEmpty)
        gridStone.DataSource = dtStoneEmpty
        funcStoneStyle()
        pnlStoneTotal.Visible = False
    End Function

    Function funcStockGridStyle() As Integer
        With gridStockChecked
            With .Columns("itemId")
                .HeaderText = "ITEM.ID"
                .Width = 60
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("TagNo")
                .HeaderText = "TAGNO"
                .Width = 60
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("Pcs")
                .HeaderText = "PCS"
                .Width = 60
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("GrsWt")
                .HeaderText = "GRS WT"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("NetWt")
                .HeaderText = "NET WT"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("Rate")
                .HeaderText = "Rate Id"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("Salvalue")
                .HeaderText = "FixRate"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With

            With .Columns("itemName")
                .HeaderText = "ITEM NAME"
                .Width = 150
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("subItemName")
                .HeaderText = "SUBITEM"
                .Width = 150
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("CounterName")
                .HeaderText = "COUNTER"
                .Width = 130
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("Approval")
                .Visible = False
            End With
            With .Columns("ChkTray")
                .Visible = False
            End With
            With .Columns("PicFileName")
                .Visible = False
            End With
        End With
        For Each gv As DataGridViewRow In gridStockChecked.Rows
            Select Case gv.Cells("ITEMID").Value.ToString
                Case "TOTAL"
                    If chkwttotal Then
                        gv.Visible = True
                        pnlTotal.Visible = True
                        lblTotal.Visible = True
                    Else
                        gv.Visible = False
                        pnlTotal.Visible = False
                        lblTotal.Visible = False
                    End If
            End Select
        Next
    End Function

    Function funcStoneStyle() As Integer
        With gridStone
            With .Columns("stnType")
                .Visible = False
            End With
            With .Columns("itemName")
                .HeaderText = "STONE"
                .Width = 150
            End With
            With .Columns("subItemName")
                .HeaderText = "SUB STONE"
                .Width = 150
            End With
            With .Columns("stnPcs")
                .HeaderText = "PCS"
                .Width = 60
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("stnWt")
                .HeaderText = "WEIGHT"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
        End With
    End Function

    Function funcLoadCheckTran() As Integer
        If txtTrayNo.Text = "" Then
            MsgBox("Tray No Should No Empty", MsgBoxStyle.Exclamation)
            txtTrayNo.Focus()
            Exit Function
        End If
        If chkAutoWeightTransfer.Checked And txtTransWt_WET.Text.Trim = "" Then
            MsgBox("Transfer Weight Should No Empty", MsgBoxStyle.Exclamation)
            txtTransWt_WET.Focus()
            Exit Function
        End If

        Dim dt As New DataTable
        dt.Clear()
        strSql = " DECLARE @DEFPATH VARCHAR(200)"
        strSql += " SELECT @DEFPATH = '" & defaultPic & "'"
        strSql += " select itemId"
        strSql += " ,TagNo"
        strSql += " ,Pcs,GrsWt,NetWt,CASE WHEN SALEMODE = 'R' THEN Rate ELSE 0 END RATE,CASE WHEN SALEMODE ='F' THEN Salvalue ELSE 0 END SALVALUE"
        strSql += " ,(select itemName from " & cnAdminDb & "..itemmast where itemId = t.itemId)as itemName"
        strSql += " ,isnull((select subItemName from " & cnAdminDb & "..subItemMast where subItemid = t.subItemId),'')as subItemName"
        strSql += " ,isnull((select itemCtrName from " & cnAdminDb & "..itemCounter where itemCtrId = t.itemCtrId),'')as CounterName"
        strSql += " ,Approval"
        strSql += " ,chkTray"
        strSql += " ,@DEFPATH + PCTFILE AS picFileName"
        strSql += " from " & cnAdminDb & "..ITEMTAG as t"
        strSql += " where itemId = " & Val(txtItemCode.Text) & ""
        strSql += " and TagNo = '" & txtTagNo.Text & "'"

        If chkCmbCounter.Text <> "ALL" And Trim(chkCmbCounter.Text) <> "" Then
            strSql += " and itemCtrId in (select itemCtrId from " & cnAdminDb & "..itemCounter "
            strSql += " where itemCtrName in (" & GetChecked_CheckedList(chkCmbCounter, True) & "))"
        End If
        'If cmbCounterName.Text <> "ALL" And cmbCounterName.Text <> "" Then
        '    strSql += " and itemCtrId = (select itemCtrId from " & cnAdminDb & "..itemCounter where itemCtrName = '" & cmbCounterName.Text & "')"
        'End If

        If cmbDesignerName.Text <> "ALL" And cmbDesignerName.Text <> "" Then
            strSql += " and DesignerId = (select DesignerId from " & cnAdminDb & "..Designer where DesignerName = '" & cmbDesignerName.Text & "')"
        End If
        If chkAsonDate.Checked = True Then
            strSql += " and RecDate <= '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "'"
        End If
        strSql += " AND ISSDATE IS NULL"
        If cmbCostCentre_MAN.Enabled Then
            strSql += " AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "')"
        End If
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            MsgBox("Invalid TagNo", MsgBoxStyle.Exclamation)
            If chkCheckingByScan.Checked Then
                txtItemCode.Clear()
            End If
            txtTagNo.Clear()
            If chkCheckingByScan.Checked Then txtItemCode.Focus() Else txtTagNo.Focus()
            Exit Function
        End If

        With dt.Rows(0)
            If .Item("chkTray").ToString <> "" Then
                MsgBox("Already Exist in Tray No : " + .Item("chkTray"), MsgBoxStyle.Exclamation)
                If chkCheckingByScan.Checked Then txtItemCode.Focus() Else txtTagNo.Focus()
                Exit Function
            End If
            If .Item("Approval").ToString = "A" Then
                MsgBox("Approval Marked..", MsgBoxStyle.Exclamation)
                If chkCheckingByScan.Checked Then txtItemCode.Focus() Else txtTagNo.Focus()
                Exit Function
            End If
            Dim ro As DataRow = Nothing
            ro = dtStockChecked.NewRow
            For cnt As Integer = 0 To dt.Columns.Count - 1
                ro(cnt) = dt.Rows(0).Item(cnt)
            Next
            strSql = " Update " & cnAdminDb & "..ITEMTAG Set chkTray = '" & txtTrayNo.Text & "',chkDate = '" & GetEntryDate(GetServerDate(tran), tran) & "'"
            If chkAutoWeightTransfer.Checked = True Then
                strSql += " ,TransferWt = '" & Val(txtTransWt_WET.Text) & "'"
            End If
            strSql += " where itemid = " & Val(txtItemCode.Text) & ""
            strSql += " and TagNo = '" & txtTagNo.Text & "'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            If dtStockChecked.Rows.Count > 0 Then
                dtStockChecked.Rows.RemoveAt(dtStockChecked.Rows.Count - 1)
            End If
            dtStockChecked.Rows.Add(ro)

            ''Grand Total
            cntPcs += dt.Rows(0).Item("Pcs")
            cntGrsWt += dt.Rows(0).Item("GrsWt")
            cntNetWt += dt.Rows(0).Item("NetWt")
            cntTagNo += 1
            txtttags.Text = cntTagNo
            txtpcs.Text = cntPcs
            txttgrwt.Text = cntGrsWt
            txttnetwt.Text = cntNetWt

            ro = dtStockChecked.NewRow
            ro("itemId") = "TOTAL"
            ro("tagNo") = cntTagNo
            ro("pcs") = cntPcs
            ro("GrsWt") = cntGrsWt
            ro("NetWt") = cntNetWt
            ro("itemName") = ""
            ro("subItemName") = ""
            ro("CounterName") = ""
            ro("Approval") = ""
            ro("chkTray") = ""
            ro("picFileName") = ""
            dtStockChecked.Rows.Add(ro)
        End With
        gridStockChecked.DataSource = dtStockChecked
        funcStockGridStyle()
        gridStockChecked.CurrentCell = gridStockChecked.Rows(gridStockChecked.RowCount - 2).Cells(0)
        gridStockChecked.Rows(gridStockChecked.RowCount - 1).DefaultCellStyle.BackColor = Color.LightBlue
        gridStockChecked.Rows(gridStockChecked.RowCount - 1).DefaultCellStyle.SelectionBackColor = Color.LightBlue
        Dim filePath As String = gridStockChecked.Item("picFileName", gridStockChecked.CurrentRow.Index).Value
        lblProcess.Text = "Getting Image " & filePath
        If ChkWithImage.Checked Then
            If File.Exists(filePath) = True Then
                Dim Finfo As IO.FileInfo
                Finfo = New IO.FileInfo(filePath)
                AutoImageSizer(filePath, picTagImage, PictureBoxSizeMode.CenterImage)
                ''Finfo.IsReadOnly = False
                'Dim fileStr As New IO.FileStream(filePath, IO.FileMode.Open, FileAccess.Read)
                'picTagImage.Image = Bitmap.FromStream(fileStr)
                'fileStr.Close()
                'picTagImage.Image = Image.FromFile(filePath)
            Else
                picTagImage.Image = Nothing
            End If
        End If

        lblProcess.Text = ""
        txtTagNo.Clear()
        txtTransWt_WET.Clear()
        If chkCheckingByScan.Checked Then
            txtItemCode.Clear()
            txtTransWt_WET.Clear()
            'Me.SelectNextControl(txtItemCode, False, True, True, True)
            txtItemCode.Focus()
            ''txtItemCode.Select()
        Else
            Me.SelectNextControl(txtItemCode, False, True, True, True)
            'txtItemCode.Select()
        End If
        'txtItemCode.Focus()
    End Function


    Private Sub frmStockCheckManual_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If chkCheckingByScan.Checked And txtItemCode.Focused Then Exit Sub
            If chkCheckingByScan.Checked And txtTransWt_WET.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmStockCheckManual_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        btnUnCheckAll.Enabled = BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Authorize, False)
        Me.pnlMain.Location = New System.Drawing.Point(0, 0)
        If Not defaultPic.EndsWith("\") Then defaultPic += "\"


        txtStPcs.BackColor = Color.White
        txtStWt.BackColor = Color.White
        txtDiaPcs.BackColor = Color.White
        txtDiaWt.BackColor = Color.White
        txtPrePcs.BackColor = Color.White
        txtPreWt.BackColor = Color.White

        Dim portSettings() As String
        Dim ctrnameportsetting As String = System.Net.Dns.GetHostName().ToUpper & "-PORTSETTINGS"
        Dim portSettingstr As String = GetAdmindbSoftValue(ctrnameportsetting, "")
        If portSettingstr <> "" Then
            portSettings = portSettingstr.Split("/")
        Else
            portSettings = GetAdmindbSoftValue("PORTSETTINGS", "9600/COM1/8/N").Split("/")
        End If

        Port_BaudRate = IIf(Val(portSettings(0)) <> 0, Val(portSettings(0)), 9600)
        Port_PortName = IIf(portSettings(1) <> "", portSettings(1), "COM1")
        Port_DataBit = IIf(Val(portSettings(2)) <> 0, Val(portSettings(2)), 8)
        If portSettings.Length - 1 >= 3 Then Port_Parity = IIf(portSettings(3) <> "", portSettings(3), "N")
        SerialPort1.DataBits = Port_DataBit
        SerialPort1.BaudRate = Port_BaudRate
        SerialPort1.PortName = Port_PortName
        SerialPort1.Parity = GetParity(Port_Parity)
        objPropertyDia = New PropertyDia(SerialPort1)

        funcNew()
        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Function GetParity(ByVal ParityName As String) As System.IO.Ports.Parity

        Select Case ParityName
            Case "N"
                Return Ports.Parity.None
            Case "E"
                Return Ports.Parity.Even
            Case "M"
                Return Ports.Parity.Mark
            Case "O"
                Return Ports.Parity.Odd
            Case "S"
                Return Ports.Parity.Space
            Case Else
                Return Ports.Parity.None
        End Select
    End Function

    Private Sub chkAsonDate_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAsonDate.CheckStateChanged
        If chkAsonDate.Checked = False Then
            dtpAsOnDate.Enabled = False
        Else
            dtpAsOnDate.Enabled = True
        End If
    End Sub

    Private Sub chkAutoWeightTransfer_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAutoWeightTransfer.CheckStateChanged
        If chkAutoWeightTransfer.Checked = True Then
            lblAutoWt.Visible = True
            txtTransWt_WET.Visible = True
        Else
            lblAutoWt.Visible = False
            txtTransWt_WET.Visible = False
        End If
    End Sub

    Private Sub cmbItemName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItemName.SelectedIndexChanged
        If cmbItemName.Text <> "ALL" And cmbItemName.Text <> "" Then
            Dim dt As New DataTable
            dt.Clear()
            strSql = " Select itemId from " & cnAdminDb & "..itemMast where itemName = '" & cmbItemName.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                txtItemCode.Text = dt.Rows(0).Item("itemId")
            Else
                txtItemCode.Clear()
            End If
        Else
            txtItemCode.Clear()
        End If
    End Sub

    Private Sub cmbItemName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItemName.TextChanged
        If cmbItemName.Text = "" Then
            txtItemCode.Clear()
        End If
    End Sub

    Private Sub txtTagNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTagNo.GotFocus
        txtTagNo.SelectAll()
    End Sub

    Private Sub txtTagNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTagNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtTrayNo.Text.Trim = "" Then
                MsgBox("Tray No Should Not Empty", MsgBoxStyle.Exclamation)
                txtTrayNo.Focus()
                Exit Sub
            End If
            If chkAutoWeightTransfer.Checked = False Then
                funcLoadCheckTran()
            End If
        End If
    End Sub

    Private Sub txtTagNo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTagNo.LostFocus
        If chkAutoWeightTransfer.Checked = False Then
            If cmbItemName.Text = "ALL" Or cmbItemName.Text = "" Then
                txtItemCode.Focus()
            Else
                txtTagNo.Focus()
            End If
        End If
    End Sub


    Private Sub txtItemCode_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItemCode.GotFocus
        txtItemCode.SelectAll()
    End Sub

    Private Sub txtTrayNo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTrayNo.LostFocus
        If txtTrayNo.Text = "" Then
            txtTrayNo.Focus()
        End If
    End Sub

    Private Sub gridStockChecked_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridStockChecked.SelectionChanged
        Dim dtStone As New DataTable
        dtStone.Clear()
        If Not gridStockChecked.RowCount > 0 Then
            Exit Sub
        End If
        If gridStockChecked.CurrentRow.Index = gridStockChecked.RowCount - 1 Then
            gridStone.DataSource = dtStoneEmpty
            txtStPcs.Clear()
            txtStWt.Clear()
            txtDiaPcs.Clear()
            txtDiaWt.Clear()
            txtPrePcs.Clear()
            txtPreWt.Clear()
            picTagImage.Image = Nothing
            Exit Sub
        End If
        Dim filePath As String = gridStockChecked.Item("picFileName", gridStockChecked.CurrentRow.Index).Value
        If File.Exists(filePath) = True Then
            Dim Finfo As IO.FileInfo
            Finfo = New IO.FileInfo(filePath)
            'Finfo.IsReadOnly = False
            Dim fileStr As New IO.FileStream(filePath, IO.FileMode.Open)
            picTagImage.Image = Bitmap.FromStream(fileStr)
            fileStr.Close()
            'picTagImage.Image = Image.FromFile(filePath)
        Else
            picTagImage.Image = Nothing
        End If

        strSql = " select "
        strSql += " (select itemName from " & cnAdminDb & "..itemMast where ItemId = s.stnItemId)as itemName"
        strSql += " ,(select subItemName from " & cnAdminDb & "..subitemMast where subItemId = s.stnsubItemId)as subItemName"
        strSql += " ,stnPcs"
        strSql += " ,stnWt"
        strSql += " ,(SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID)AS stnType"
        strSql += " from " & cnAdminDb & "..ITEMTAGSTONE as s"
        strSql += " where itemId = '" & gridStockChecked.Item("itemId", gridStockChecked.CurrentRow.Index).Value & "'"
        strSql += " and tagNo = '" & gridStockChecked.Item("TagNo", gridStockChecked.CurrentRow.Index).Value & "'"

        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtStone)
        gridStone.DataSource = dtStone
        If Not dtStone.Rows.Count > 0 Then
            pnlStoneTotal.Visible = False
            txtStPcs.Clear()
            txtStWt.Clear()
            txtDiaPcs.Clear()
            txtDiaWt.Clear()
            txtPrePcs.Clear()
            txtPreWt.Clear()
            Exit Sub
        End If
        funcStoneStyle()
        pnlStoneTotal.Visible = True
        For cnt As Integer = 0 To dtStone.Rows.Count - 1
            With dtStone.Rows(cnt)
                If .Item("stnType") = "D" Then
                    txtDiaPcs.Text = Val(txtDiaPcs.Text) + Val(.Item("stnPcs"))
                    txtDiaWt.Text = Val(txtDiaWt.Text) + Val(.Item("stnWt"))
                ElseIf .Item("stnType") = "S" Then
                    txtStPcs.Text = Val(txtStPcs.Text) + Val(.Item("stnPcs"))
                    txtStWt.Text = Val(txtStWt.Text) + Val(.Item("stnWt"))
                Else
                    txtPrePcs.Text = Val(txtPrePcs.Text) + Val(.Item("stnPcs"))
                    txtPreWt.Text = Val(txtPreWt.Text) + Val(.Item("stnWt"))
                End If
            End With
        Next
    End Sub

    Private Sub txtTransWt_WET_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTransWt_WET.GotFocus
        If chkAutoWeightTransfer.Checked Then
            GetGrsWeightFromPort()
        End If
    End Sub

    Private Sub txtTransWt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTransWt_WET.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            funcLoadCheckTran()
        ElseIf e.KeyChar = Chr(Keys.Space) Then
            GetGrsWeightFromPort()
            e.Handled = True
        End If
    End Sub

    Private Sub txtTransWt_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTransWt_WET.LostFocus
        'If Val(txtTransWt_WET.Text) = 0 Then txtTransWt_WET.Focus() : Exit Sub
        'If cmbItemName.Text = "ALL" Then
        '    txtItemCode.Focus()
        'Else
        '    txtTagNo.Focus()
        'End If
    End Sub
    Private Sub GetGrsWeightFromPort()
        Dim Wt_Balance_Sep As String = GetAdmindbSoftValue("WT_BALANCE_SEP", "")
        Dim Weight As Double = Nothing
        Try
            If SerialPort1.IsOpen Then SerialPort1.Close()
            SerialPort1.Open()
            If SerialPort1.IsOpen Then
                Dim readStr As String = Nothing
                If Wt_Balance_Sep <> "" Then
                    readStr = UCase(SerialPort1.ReadTo(Wt_Balance_Sep))
                    If IsNumeric(readStr.Substring(readStr.Length - 6)) Then
                        readStr = (Val(readStr.Substring(readStr.Length - 6)) / 1000).ToString()
                    End If
                    Weight = Val(Trim(readStr))
                Else
                    For cnt As Integer = 1 To 10
                        readStr = UCase(SerialPort1.ReadLine)
                        If readStr.Contains(".") Then
                            Exit For
                        End If
                    Next
                    Dim wt() As String = readStr.Split(Environment.NewLine)
                    Dim wet As String = ""
                    For Each c As Char In wt(0)
                        If c = "," Then Continue For
                        If Char.IsPunctuation(c) Then wet += c
                        If Char.IsNumber(c) Then wet += c
                    Next
                    Weight = Val(Trim(wet))
                    ReadData.Text = readStr
                    SplitData.Text = wt(0)
                    ModifyData.Text = wet
                End If
            End If
            If SerialPort1.IsOpen Then SerialPort1.Close()
        Catch ex As Exception
            txtTagNo.Focus()
            MsgBox("Please check com connection" + vbCrLf + ex.Message + vbCrLf + ex.StackTrace)
            If SerialPort1.IsOpen Then SerialPort1.Close()
        End Try
        If Weight = 0 Then txtTransWt_WET.Text = "" : Exit Sub
        Dim rndDigit As Integer = 3
        Weight = Math.Round(Weight, rndDigit)
        txtTransWt_WET.Text = IIf(Weight <> 0, Format(Weight, "0.000"), Nothing)
        txtTransWt_WET.SelectAll()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub gridStockChecked_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridStockChecked.KeyDown
        If e.KeyCode = Keys.Delete Then
            If gridStockChecked.CurrentRow.Index = gridStockChecked.RowCount - 1 Then
                e.Handled = True
            Else
                Dim itemId As String = gridStockChecked.Item("itemId", gridStockChecked.CurrentRow.Index).Value
                Dim TagNo As String = gridStockChecked.Item("TagNo", gridStockChecked.CurrentRow.Index).Value
                If MessageBox.Show("Do U want to Delete... " + vbCrLf + "Item Id : " + itemId + vbCrLf + "Tag No : " + TagNo, "Delete Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Cancel Then
                    e.Handled = True
                End If
            End If

        End If
    End Sub

    Private Sub gridStockChecked_UserDeletingRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowCancelEventArgs) Handles gridStockChecked.UserDeletingRow
        Dim index As Integer = gridStockChecked.CurrentRow.Index
        Dim last As Integer = gridStockChecked.RowCount - 1
        With gridStockChecked
            cntTagNo = .Item("TagNo", last).Value - 1
            cntPcs = .Item("Pcs", last).Value - .Item("Pcs", index).Value
            cntGrsWt = .Item("GrsWt", last).Value - .Item("GrsWt", index).Value
            cntNetWt = .Item("NetWt", last).Value - .Item("NetWt", index).Value
            .Item("TagNo", last).Value = cntTagNo
            .Item("Pcs", last).Value = cntPcs
            .Item("GrsWt", last).Value = cntGrsWt
            .Item("NetWt", last).Value = cntNetWt
            strSql = " Update " & cnAdminDb & "..ITEMTAG Set chkTray = ''"
            If chkAutoWeightTransfer.Checked = True Then
                strSql += " ,TransferWt = 0"
            End If
            strSql += " where itemid = '" & .Item("itemId", index).Value & "'"
            strSql += " and TagNo = '" & .Item("TagNo", index).Value & "'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End With
    End Sub

    Private Sub btnUnCheckAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUnCheckAll.Click
        ''Load ItemName in Uncheck
        strSql = " Select itemName from " & cnAdminDb & "..itemMast order by itemName"
        objGPack.FillCombo(strSql, cmbUncheckItem, True)
        cmbUncheckItem.Items.Add("ALL")
        cmbUncheckItem.Text = "ALL"

        ''Load Counter in Uncheck
        strSql = " Select ItemCtrName from " & cnAdminDb & "..itemCounter order by itemCtrName"
        objGPack.FillCombo(strSql, cmbUncheckCounter, True)
        cmbUncheckCounter.Items.Add("ALL")
        cmbUncheckCounter.Text = "ALL"

        ''Load ItemType in Uncheck
        strSql = " Select Name from " & cnAdminDb & "..itemType order by Name"
        objGPack.FillCombo(strSql, cmbUncheckItemType, True)
        cmbUncheckItemType.Items.Add("ALL")
        cmbUncheckItemType.Text = "ALL"

        pnlMain.Visible = False
        Me.Controls.Add(pnlUncheck)
        Me.pnlUncheck.Location = New System.Drawing.Point(300, 200)
        pnlUncheck.BringToFront()
        pnlUncheck.Visible = True
        'cmbUncheckItem.Focus()
        txtcmbItemid_NUM.Focus()
    End Sub

    Private Sub gridStone_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridStone.KeyPress
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExcelStone_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            btnPrintStone_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub gridStockChecked_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridStockChecked.KeyPress
        If e.KeyChar = "X" Or e.KeyChar = "x" Then
            Me.btnExcelStockCheck_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "P" Then
            btnPrintStockCheck_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub btnExcelStockCheck_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcelStockCheck.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridStockChecked.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "Stock check Manual", gridStockChecked, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Sub btnExcelStone_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcelStone.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridStone.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "Stock check Manual", gridStone, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrintStockCheck_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintStockCheck.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridStockChecked.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "Stock check Manual", gridStockChecked, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnPrintStone_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintStone.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridStone.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "Stock check Manual", gridStone, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        dtStockChecked.Clear()
        ' cntTagNo = 0
        'cntPcs = 0
        'cntGrsWt = 0
        'cntNetWt = 0
        'chkAsonDate.Checked = False
        'chkAutoWeightTransfer.Checked = False
        'chkCheckingByScan.Checked = False
        gridStockChecked.DataSource = Nothing
        ' cmbItemName.Text = "ALL"
        ' cmbCounterName.Text = "ALL"
        'cmbDesignerName.Text = "ALL"
        pnlMain.Dock = DockStyle.Fill
        ''lOAD cOSTCENTRE
        'cmbCostCentre_MAN.Text = ""
        cmbCostCentre_MAN.Items.Clear()
        If GetAdmindbSoftValue("COSTCENTRE", "N") = "Y" Then
            cmbCostCentre_MAN.Enabled = True
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            objGPack.FillCombo(strSql, cmbCostCentre_MAN, False)
            cmbCostCentre_MAN.Text = cnCostName
            If strUserCentrailsed <> "Y" Then cmbCostCentre_MAN.Enabled = False
        Else
            cmbCostCentre_MAN.Enabled = False
        End If
        If cmbCostCentre_MAN.Enabled Then
            cmbCostCentre_MAN.Select()
        Else
            Me.SelectNextControl(cmbCostCentre_MAN, True, True, True, True)
        End If
        cmbItemName.Focus()
        cntPcs = 0
        cntGrsWt = 0
        cntNetWt = 0
        cntTagNo = 0

    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnUncheckUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUncheckUpdate.Click
        Dim objSecret As New frmAdminPassword()
        If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then
            Exit Sub
        End If
        strSql = " Update " & cnAdminDb & "..ITEMTAG Set chkTray = '' WHERE 1=1 "
        If cmbUncheckCounter.Text <> "ALL" And cmbUncheckCounter.Text <> "" Then
            strSql += " AND ITEMCTRID = (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER "
            strSql += " WHERE ITEMCTRNAME = '" & cmbUncheckCounter.Text & "')"
        End If
        If cmbUncheckItemType.Text <> "ALL" And cmbUncheckItemType.Text <> "" Then
            strSql += " AND ITEMTYPEID = (SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE "
            strSql += " WHERE NAME = '" & cmbUncheckItemType.Text & "')"
        End If
        If cmbUncheckItem.Text <> "ALL" And cmbUncheckItem.Text <> "" Then
            strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST "
            strSql += " WHERE ITEMNAME = '" & cmbUncheckItem.Text & "')"
        End If
        If cmbCostCentre_MAN.Text.ToString.Trim <> "" And cmbCostCentre_MAN.Text.ToString.Trim <> "ALL" Then
            'strSql += " AND COSTID = (SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTID')"
            strSql += " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text.ToString & "')"
        End If
        If Not cnCentStock Then strSql += " AND COMPANYID = '" & GetStockCompId() & "'"
        Try
            tran = cn.BeginTransaction
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            tran.Commit()
        Catch ex As Exception
            tran.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
        MsgBox("Successfully Unchecked..")
        btnUncheckExit.Focus()
    End Sub

    Private Sub btnUncheckExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUncheckExit.Click
        pnlUncheck.Visible = False
        pnlMain.Visible = True
        pnlMain.Location = New System.Drawing.Point(0, 0)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub txtItemCode_NUM_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtItemCode.KeyDown
        If e.KeyCode = Keys.Insert Then
            strSql = "SELECT ITEMID,ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T' ORDER BY ITEMID"
            txtItemCode.Text = BrighttechPack.SearchDialog.Show("Find ItemId", strSql, cn)
        End If
    End Sub

    Private Sub txtItemCode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtItemCode.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtTrayNo.Text.Trim = "" Then
                MsgBox("Tray No Should Not Empty", MsgBoxStyle.Exclamation)
                txtTrayNo.Focus()
                Exit Sub
            End If

            Dim barcode2d() As String = txtItemCode.Text.Split(BARCODE2DSEP)
            If barcode2d.Length > 2 Then Call Barcode2ddetails(txtItemCode.Text) : Exit Sub

            Dim sp() As String = txtItemCode.Text.Split(PRODTAGSEP)
            Dim ScanStr As String = txtItemCode.Text
            If PRODTAGSEP <> "" And txtItemCode.Text <> "" Then
                sp = txtItemCode.Text.Split(PRODTAGSEP)
                txtItemCode.Text = Trim(sp(0))
            End If
            If txtItemCode.Text.StartsWith("#") Then txtItemCode.Text = txtItemCode.Text.Remove(0, 1)
CheckItem:
            If txtItemCode.Text = "" Then
                MsgBox("Item Id should not empty", MsgBoxStyle.Information)
                Exit Sub
            End If
            If IsNumeric(ScanStr) = True And ScanStr.Contains(PRODTAGSEP) = False And objGPack.DupCheck("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE CONVERT(VARCHAR,ITEMID) = '" & Val(ScanStr) & "'" & GetItemQryFilteration()) = True Then
                'SendKeys.Send("{TAB}")
                Exit Sub
            ElseIf PRODTAGSEP <> "" And PRODTAGSEP <> Nothing And ScanStr.Contains(PRODTAGSEP) Then
                txtItemCode.Text = Trim(sp(0))
                txtTagNo.Text = Trim(sp(1))
                GoTo LoadItemInfo
            ElseIf PRODTAGSEP <> "" Then
                strSql = " SELECT ITEMID,TAGNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGKEY = '" & Trim(ScanStr).Replace(PRODTAGSEP, "") & "'"
                Dim dtItemDet As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtItemDet)
                If dtItemDet.Rows.Count > 0 Then
                    txtItemCode.Text = dtItemDet.Rows(0).Item("ITEMID").ToString
                    txtTagNo.Text = dtItemDet.Rows(0).Item("TAGNO").ToString
                    GoTo LoadItemInfo
                End If
            ElseIf txtItemCode.Text <> "" And objGPack.DupCheck("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE CONVERT(VARCHAR,ITEMID) = '" & Trim(txtItemCode.Text) & "'") = False Then
                strSql = " SELECT ITEMID,TAGNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGKEY = '" & Trim(txtItemCode.Text) & "'"
                Dim dtItemDet As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtItemDet)
                If dtItemDet.Rows.Count > 0 Then
                    txtItemCode.Text = dtItemDet.Rows(0).Item("ITEMID").ToString
                    txtTagNo.Text = dtItemDet.Rows(0).Item("TAGNO").ToString
                    txtTagNo_KeyPress(Me, New KeyPressEventArgs(Chr(Keys.Enter)))
                    Exit Sub
                    GoTo CheckItem
                End If
            End If
LoadItemInfo:
            If sp.Length > 1 And PRODTAGSEP <> "" And txtItemCode.Text <> "" Then
                txtTagNo.Text = Trim(sp(1))
            End If
            If txtTagNo.Text <> "" And chkAutoWeightTransfer.Checked = False Then
                txtTagNo_KeyPress(Me, New KeyPressEventArgs(Chr(Keys.Enter)))
            ElseIf txtTagNo.Text <> "" And chkAutoWeightTransfer.Checked = True Then
                Me.SelectNextControl(txtTagNo, True, True, True, True)
            End If
        End If
        '        If e.KeyChar = Chr(Keys.Enter) Then
        '            Dim sp() As String = txtItemCode.Text.Split(PRODTAGSEP)
        '            If PRODTAGSEP <> "" And txtItemCode.Text <> "" Then
        '                sp = txtItemCode.Text.Split(PRODTAGSEP)
        '                txtItemCode.Text = Trim(sp(0))
        '            End If
        '            'Dim sep As String = Nothing
        '            'For Each c As Char In txtItemCode.Text
        '            '    If Not Char.IsNumber(c) Then sep += c & ","
        '            'Next
        '            'If sep <> Nothing Then
        '            '    sep.Remove(sep.Length - 1, 1)
        '            '    Dim s() As String = txtItemCode.Text.Split(sep)
        '            '    txtItemCode.Text = s(0)
        '            'End If
        'CheckItem:
        '            If txtItemCode.Text = "" Then
        '                MsgBox("Item Id should not empty", MsgBoxStyle.Information)
        '                Exit Sub
        '            End If
        '            If txtItemCode.Text.StartsWith("#") Then txtItemCode.Text = txtItemCode.Text.Remove(0, 1)
        '            If txtItemCode.Text <> "" And objGPack.DupCheck("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE CONVERT(VARCHAR,ITEMID) = '" & Trim(txtItemCode.Text) & "'") = False Then
        '                strSql = " SELECT ITEMID,TAGNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGKEY = '" & Trim(txtItemCode.Text) & "'"
        '                Dim dtItemDet As New DataTable
        '                da = New OleDbDataAdapter(strSql, cn)
        '                da.Fill(dtItemDet)
        '                If dtItemDet.Rows.Count > 0 Then
        '                    txtItemCode.Text = dtItemDet.Rows(0).Item("ITEMID").ToString
        '                    txtTagNo.Text = dtItemDet.Rows(0).Item("TAGNO").ToString
        '                    txtTagNo_KeyPress(Me, New KeyPressEventArgs(Chr(Keys.Enter)))
        '                    Exit Sub
        '                    GoTo CheckItem
        '                End If
        '            End If
        '            If sp.Length > 1 And PRODTAGSEP <> "" And txtItemCode.Text <> "" Then
        '                txtTagNo.Text = Trim(sp(1))
        '                txtTagNo_KeyPress(Me, New KeyPressEventArgs(Chr(Keys.Enter)))
        '            End If
        '            'Dim sp() As String = txtItemCode.Text.Split(PRODTAGSEP)
        '            'If PRODTAGSEP <> "" And txtItemCode.Text <> "" Then
        '            '    sp = txtItemCode.Text.Split(PRODTAGSEP)
        '            '    txtItemCode.Text = sp(0)
        '            'End If
        '            'Dim sep As String = Nothing
        '            'For Each c As Char In txtItemCode.Text
        '            '    If Not Char.IsNumber(c) Then sep += c & ","
        '            'Next
        '            'If sep <> Nothing Then
        '            '    sep.Remove(sep.Length - 1, 1)
        '            '    Dim s() As String = txtItemCode.Text.Split(sep)
        '            '    txtItemCode.Text = s(0)
        '            'End If
        '            'If sp.Length > 1 And PRODTAGSEP <> "" And txtItemCode.Text <> "" Then
        '            '    txtTagNo.Text = sp(1)
        '            '    txtTagNo_KeyPress(Me, New KeyPressEventArgs(Chr(Keys.Enter)))
        '            'End If
        'End If
    End Sub

    Private Function Barcode2ddetails(ByVal barcode2dstring As String)
        Dim barcode2darray1() As String = barcode2dstring.Split(BARCODE2DSEP)
        txtItemCode.Text = barcode2darray1(0).ToString
        txtTagNo.Text = barcode2darray1(2).ToString
        If txtTagNo.Text <> "" Then txtTagNo_KeyPress(Me, New KeyPressEventArgs(Chr(Keys.Enter)))
    End Function
    
    Private Sub WeighingScalePropertyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WeighingScalePropertyToolStripMenuItem.Click
        objPropertyDia = New PropertyDia(SerialPort1)
        objPropertyDia.ShowDialog()
    End Sub

    Private Sub txtItemCode_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtItemCode.TextChanged

    End Sub

    Private Sub txtcmbItemid_NUM_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtcmbItemid_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtcmbItemid_NUM.Text = "" Then Exit Sub
            strSql = " Select itemName from " & cnAdminDb & "..itemMast where itemId = '" & txtcmbItemid_NUM.Text & "'"
            Dim dt As New DataTable
            dt.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                cmbUncheckItem.Text = dt.Rows(0).Item("itemName")
            End If
        End If
    End Sub
End Class

