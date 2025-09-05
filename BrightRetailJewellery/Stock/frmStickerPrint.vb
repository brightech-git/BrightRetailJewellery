Imports System.Data
Imports System.Data.OleDb

Public Class frmStickerPrint
    Dim strSql As String = ""
    Dim cmd As OleDbCommand
    Private Sub frmStickerPrint_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ISNULL(STUDDED,'') <> 'S' AND ACTIVE = 'Y' AND STOCKTYPE='T' ORDER BY ITEMNAME"
        objGPack.FillCombo(strSql, cmbItem_MAN, , False)

        strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ISNULL(ACTIVE,'') <> 'N' ORDER BY DISPLAYORDER,ITEMCTRNAME"
        objGPack.FillCombo(strSql, cmbCounter_MAN, False)

        strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ISNULL(ACTIVE,'') <> 'N' ORDER BY DESIGNERNAME"
        objGPack.FillCombo(strSql, cmbDesigner_MAN)

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        Dim systemName As String = My.Computer.Name

        strSql = "IF OBJECT_ID('TEMP" & systemName & "STICKERPRINT') IS NOT NULL DROP TABLE TEMP" & systemName & "STICKERPRINT"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        Dim ItemId As String = ""
        Dim SubItemId As String = ""
        Dim ItemCtrId As String = ""
        Dim DesignerId As String = ""
        ItemId = objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItem_MAN.Text.ToString & "'")
        SubItemId = objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID=" & Val(ItemId).ToString & " AND SUBITEMNAME='" & cmbSubItem_Man.Text.ToString & "'")
        ItemCtrId = objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME='" & cmbCounter_MAN.Text.ToString & "'")
        DesignerId = objGPack.GetSqlValue("SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME='" & cmbDesigner_MAN.Text.ToString & "'")
        strSql = vbCrLf + " SELECT "
        strSql += vbCrLf + " " & Val(ItemId.ToString).ToString & " ITEMID,'" & cmbItem_MAN.Text.ToString & "' ITEMNAME"
        strSql += vbCrLf + " ," & Val(SubItemId.ToString).ToString & " SUBITEMID,'" & cmbSubItem_Man.Text.ToString & "' SUBITEMNAME"
        strSql += vbCrLf + " ," & Val(ItemCtrId.ToString).ToString & " ITEMCTRID,'" & cmbCounter_MAN.Text.ToString & "' ITEMCTRNAME"
        strSql += vbCrLf + " ," & Val(DesignerId.ToString).ToString & " DESIGNERID,'" & cmbDesigner_MAN.Text.ToString & "' DESIGNERNAME"
        strSql += vbCrLf + " ,'" & cmbItemSize.Text.ToString & "' ITEMSIZE"
        strSql += vbCrLf + " ,'" & Val(txtPieces_Num_MAN.Text.ToString).ToString & "' PCS"
        strSql += vbCrLf + " ," & Format(Val(txtGrossWt_Wet_MAN.Text.ToString), "0.000").ToString & " GRSWT," & Format(Val(txtNetWt_Wet.Text.ToString), "0.000").ToString & " NETWT," & Format(Val(txtLessWt_Wet.Text.ToString), "0.000").ToString & " LESSWT"
        strSql += vbCrLf + " ," & Format(Val(txtMaxWastage_Per.Text.ToString), "0.00").ToString & " MAXWASTPER," & Format(Val(txtMaxWastage_Wet.Text.ToString), "0.00").ToString & " MAXWAST"
        strSql += vbCrLf + " ," & Format(Val(txtMaxMcPerGrm_Amt.Text.ToString), "0.00").ToString & " MAXMCGRM," & Format(Val(txtMaxMkCharge_Amt.Text.ToString), "0.00").ToString & " MAXMC"
        strSql += vbCrLf + " ,'" & txtNarration.Text.ToString & "' NARR"
        strSql += vbCrLf + " INTO TEMP" & systemName & "STICKERPRINT"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        Dim objBar As New clsBarcodePrint
        objBar.FuncprintSticker()

        btnNew_Click(Me, e)
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        cmbDesigner_MAN.Select()
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub SaveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveToolStripMenuItem.Click

    End Sub

    Private Sub NewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewToolStripMenuItem.Click

    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click

    End Sub

    Private Sub cmbItem_MAN_Leave(sender As Object, e As EventArgs) Handles cmbItem_MAN.Leave
        strSql = GetSubItemQry(New String() {"SUBITEMNAME"}, Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'")))
        objGPack.FillCombo(strSql, cmbSubItem_Man, False)
        If cmbSubItem_Man.Items.Count > 0 Then cmbSubItem_Man.Enabled = True Else cmbSubItem_Man.Enabled = False

        cmbItemSize.Enabled = True
        strSql = " SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')"
        objGPack.FillCombo(strSql, cmbItemSize, , False)
        If cmbItemSize.Items.Count > 0 Then cmbItemSize.Enabled = True Else cmbItemSize.Enabled = False
    End Sub

    Private Sub frmStickerPrint_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtLessWt_Wet_TextChanged(sender As Object, e As EventArgs) Handles txtLessWt_Wet.TextChanged
        If Val(txtLessWt_Wet.Text) >= Val(txtGrossWt_Wet_MAN.Text) Then
            MsgBox("Less Weight should be less than Gross Weight.")
            txtLessWt_Wet.Text = "0.000"
            Exit Sub
        End If
        Dim wt As Double = Nothing
        wt = Val(txtGrossWt_Wet_MAN.Text) - Val(txtLessWt_Wet.Text)
        strSql = "SELECT ISNULL(NETWTPER,0) NETWTPER FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"
        Dim mnetwtper As Double = Val(objGPack.GetSqlValue(strSql).ToString)
        If mnetwtper <> 0 Then wt = wt * (mnetwtper / 100)
        txtNetWt_Wet.Text = IIf(wt <> 0, Format(wt, "0.000"), "")
    End Sub

    Private Sub txtMaxWastage_Per_TextChanged(sender As Object, e As EventArgs) Handles txtMaxWastage_Per.TextChanged
        Dim wt As Double = Nothing
        Dim mweight As Double = Val(txtGrossWt_Wet_MAN.Text.ToString)
        wt = mweight * (Val(txtMaxWastage_Per.Text) / 100)
        wt = Math.Round(wt, 3)
        txtMaxWastage_Wet.Text = IIf(wt <> 0, Format(wt, "0.000"), "")
    End Sub

    Private Sub txtMaxWastage_Wet_TextChanged(sender As Object, e As EventArgs) Handles txtMaxWastage_Wet.TextChanged

    End Sub

    Private Sub txtMaxMcPerGrm_Amt_TextChanged(sender As Object, e As EventArgs) Handles txtMaxMcPerGrm_Amt.TextChanged
        Dim mc As Double = Nothing
        Dim mweight As Double = 0
        mweight = Val(txtGrossWt_Wet_MAN.Text)
        mc = mweight * Val(txtMaxMcPerGrm_Amt.Text.ToString)
        mc = Math.Round(mc, 2)
        txtMaxMkCharge_Amt.Text = IIf(mc <> 0, Format(mc, "0.00"), "")
    End Sub

    Private Sub txtNetWt_Wet_TextChanged(sender As Object, e As EventArgs) Handles txtNetWt_Wet.TextChanged
        If Val(txtNetWt_Wet.Text) >= Val(txtGrossWt_Wet_MAN.Text) Then
            'MsgBox("Net Weight should not exceed Gross Weight.")

            Dim wt As Double = Nothing
            wt = Val(txtGrossWt_Wet_MAN.Text) - Val(txtLessWt_Wet.Text)
            strSql = "SELECT ISNULL(NETWTPER,0) NETWTPER FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"
            Dim mnetwtper As Double = Val(objGPack.GetSqlValue(strSql).ToString)
            If mnetwtper <> 0 Then wt = wt * (mnetwtper / 100)
            txtNetWt_Wet.Text = IIf(wt <> 0, Format(wt, "0.000"), "")


            Exit Sub
        End If
    End Sub

    Private Sub txtGrossWt_Wet_MAN_TextChanged(sender As Object, e As EventArgs) Handles txtGrossWt_Wet_MAN.TextChanged
        Dim wt As Double = Nothing
        wt = Val(txtGrossWt_Wet_MAN.Text) - Val(txtLessWt_Wet.Text)
        strSql = "SELECT ISNULL(NETWTPER,0) NETWTPER FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"
        Dim mnetwtper As Double = Val(objGPack.GetSqlValue(strSql).ToString)
        If mnetwtper <> 0 Then wt = wt * (mnetwtper / 100)
        txtNetWt_Wet.Text = IIf(wt <> 0, Format(wt, "0.000"), "")

    End Sub
End Class