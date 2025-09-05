Imports System
Imports System.Data.OleDb

Public Class frmDiamondDetails
    Dim strsql As String
    Dim da As New OleDbDataAdapter
    Dim DtStnGrp As New DataTable
    Dim ORDER_MULTI_MIMR As Boolean = IIf(GetAdmindbSoftValue("ORDER_MULTI_MIMR", "N") = "Y", True, False)
    Public ForSales As Boolean = False
    Dim POS_ENABLE_STNGRP As Boolean = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "POS_ENABLE_STNGRP", "N") = "Y", True, False)

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        LoadStnGrp()
        strsql = " SELECT COLORNAME FROM " & cnAdminDb & "..STNCOLOR WHERE ISNULL(ACTIVE,'')='Y' ORDER BY COLORNAME"
        objGPack.FillCombo(strsql, CmbColor, True, False)
        strsql = " SELECT CUTNAME FROM " & cnAdminDb & "..STNCUT WHERE ISNULL(ACTIVE,'')='Y' ORDER BY CUTNAME"
        objGPack.FillCombo(strsql, CmbCut, True, False)
        strsql = " SELECT CLARITYNAME FROM " & cnAdminDb & "..STNCLARITY WHERE ISNULL(ACTIVE,'')='Y' ORDER BY CLARITYNAME"
        objGPack.FillCombo(strsql, CmbClarity, True, False)
        strsql = " SELECT SHAPENAME FROM " & cnAdminDb & "..STNSHAPE WHERE ISNULL(ACTIVE,'')='Y' ORDER BY SHAPENAME"
        objGPack.FillCombo(strsql, cmbShape, True, False)
        strsql = " SELECT SETTYPENAME FROM " & cnAdminDb & "..STNSETTYPE WHERE ISNULL(ACTIVE,'')='Y' ORDER BY SETTYPENAME"
        objGPack.FillCombo(strsql, cmbSetType, True, False)
    End Sub

    Public Sub New(ByVal stnGrpId As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        LoadStnGrp()
        strsql = " SELECT COLORNAME FROM " & cnAdminDb & "..STNCOLOR WHERE ISNULL(ACTIVE,'')='Y' ORDER BY COLORNAME"
        objGPack.FillCombo(strsql, CmbColor, True, False)
        strsql = " SELECT CUTNAME FROM " & cnAdminDb & "..STNCUT WHERE ISNULL(ACTIVE,'')='Y' ORDER BY CUTNAME"
        objGPack.FillCombo(strsql, CmbCut, True, False)
        strsql = " SELECT CLARITYNAME FROM " & cnAdminDb & "..STNCLARITY WHERE ISNULL(ACTIVE,'')='Y' ORDER BY CLARITYNAME"
        objGPack.FillCombo(strsql, CmbClarity, True, False)
        strsql = " SELECT SHAPENAME FROM " & cnAdminDb & "..STNSHAPE WHERE ISNULL(ACTIVE,'')='Y' ORDER BY SHAPENAME"
        objGPack.FillCombo(strsql, cmbShape, True, False)
        strsql = " SELECT SETTYPENAME FROM " & cnAdminDb & "..STNSETTYPE WHERE ISNULL(ACTIVE,'')='Y' ORDER BY SETTYPENAME"
        objGPack.FillCombo(strsql, cmbSetType, True, False)
        If stnGrpId <> "" Then
            cmbStnGrp.Text = GetSqlValue(cn, "SELECT GROUPNAME FROM " & cnAdminDb & "..STONEGROUP WHERE GROUPID = '" & stnGrpId & "'")
            SetDefaultValues(cmbStnGrp.Text)
        End If
    End Sub
    Private Sub LoadStnGrp(Optional ByVal StnGrpid As String = "")
        strsql = "SELECT '' GROUPNAME,0 GROUPID,0 DISPLAYORDER "
        strsql += "UNION ALL"
        strsql += " SELECT DISTINCT GROUPNAME,GROUPID,DISPLAYORDER FROM " & cnAdminDb & "..STONEGROUP WHERE ISNULL(ACTIVE,'') <> 'N' "
        If StnGrpid <> "" Then
            strsql += "  WHERE GROUPID = '" & StnGrpid & "'"
        End If
        strsql += "ORDER BY DISPLAYORDER,GROUPNAME "
        objGPack.FillCombo(strsql, cmbStnGrp, True, False)
    End Sub
    Private Sub frmDiamondDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub frmDiamondDetails_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Me.Close()
    End Sub

    Private Sub cmbStnGrp_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbStnGrp.SelectedIndexChanged
        SetDefaultValues(cmbStnGrp.Text)
    End Sub
    Public Function SetDefaultValues(ByVal StnGroup As String)
        If StnGroup <> "" Then
            strsql = vbCrLf + "  SELECT "
            strsql += vbCrLf + "(SELECT CUTNAME FROM " & cnAdminDb & "..STNCUT WHERE CUTID = S.CUTID) CUTNAME"
            strsql += vbCrLf + "  ,(SELECT COLORNAME FROM " & cnAdminDb & "..STNCOLOR WHERE COLORID = S.COLORID) COLORNAME"
            strsql += vbCrLf + "  ,(SELECT CLARITYNAME FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYID = S.CLARITYID) CLARITY"
            strsql += vbCrLf + "  ,(SELECT SHAPENAME FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPEID = S.SHAPEID) SHAPE"
            strsql += vbCrLf + "  ,(SELECT SETTYPENAME FROM " & cnAdminDb & "..STNSETTYPE WHERE SETTYPEID = S.SETTYPEID) SETTYPENAME"
            strsql += vbCrLf + "  ,GROUPID FROM " & cnAdminDb & "..STONEGROUP S WHERE GROUPNAME = '" & cmbStnGrp.Text & "'"
            DtStnGrp = New DataTable
            da = New OleDbDataAdapter(strsql, cn)
            da.Fill(DtStnGrp)
            If DtStnGrp.Rows.Count > 0 Then
                If POS_ENABLE_STNGRP And ForSales Then cmbStnGrp.Enabled = False
                CmbCut.Text = DtStnGrp.Rows(0).Item("CUTNAME").ToString : CmbCut.Enabled = False
                CmbColor.Text = DtStnGrp.Rows(0).Item("COLORNAME").ToString : CmbColor.Enabled = False
                CmbClarity.Text = DtStnGrp.Rows(0).Item("CLARITY").ToString : CmbClarity.Enabled = False
                cmbShape.Text = DtStnGrp.Rows(0).Item("SHAPE").ToString : cmbShape.Enabled = False
                cmbSetType.Text = DtStnGrp.Rows(0).Item("SETTYPENAME").ToString : cmbSetType.Enabled = False
            End If
        End If
    End Function

    Private Sub frmDiamondDetails_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If ORDER_MULTI_MIMR = True Then
            cmbStnGrp.Visible = True
            lblStnGrp.Visible = True
            CmbCut.Enabled = False
            CmbColor.Enabled = False
            CmbClarity.Enabled = False
            cmbSetType.Enabled = False
            cmbShape.Enabled = False
        Else
            cmbStnGrp.Visible = False
            lblStnGrp.Visible = False
            CmbCut.Enabled = True
            CmbColor.Enabled = True
            CmbClarity.Enabled = True
            cmbSetType.Enabled = True
            cmbShape.Enabled = True
        End If
    End Sub
End Class