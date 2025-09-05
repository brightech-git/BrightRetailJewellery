Public Class BillPurchaseDetail
    Dim StrSql As String
    Dim MetalBaseItem As Boolean = IIf(GetAdmindbSoftValue("PURC_METALBASEITEM", "N") = "Y", True, False)
    Dim CatViewFilter As Boolean = IIf(GetAdmindbSoftValue("CAT_VIEW_FILTER", "N") = "Y", True, False)
    Dim categorytype As Boolean = IIf(GetAdmindbSoftValue("EST_CATITEMTYPE", "N") = "Y", True, False)
    Dim ESTPURSUBITEMEMPTY As Boolean = IIf(GetAdmindbSoftValue("ESTPURSUBITEMEMPTY", "N") = "Y", True, False)
    Dim mAlltype As Boolean = False
    Dim AllItemType1 As Boolean = False
    Dim Displtype1 As String = Nothing

    Public Sub New(Optional ByVal AllItemType As Boolean = False, Optional ByVal Displtype As String = Nothing)
        AllItemType1 = AllItemType
        Displtype1 = Displtype
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        StrSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY"
        StrSql += " WHERE CATMODE IN ('B','P')"
        If Displtype <> Nothing Then
            If Displtype = "E" Then StrSql += " AND ESTDISPLAY = 'Y' "
        End If
        If CatViewFilter Then StrSql += " AND VIEWFILTER LIKE'%PU%'"
        StrSql += " ORDER BY DISPLAYORDER,CATNAME"
        objGPack.FillCombo(StrSql, cmbCategory_MAN)

        StrSql = " SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE"
        If Not AllItemType Then
            StrSql += " WHERE SOFTMODULE = 'E' "
        End If
        StrSql += " ORDER BY DISPALYORDER"
        objGPack.FillCombo(StrSql, cmbItemType_MAN)
        If cmbItemType_MAN.Items.Count > 0 Then cmbItemType_MAN.Enabled = True Else cmbItemType_MAN.Enabled = False
        mAlltype = AllItemType
    End Sub

    Private Sub BillPurchaseDetail_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub BillPurchaseDetail_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtDescription.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cmbCategory_MAN_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCategory_MAN.SelectedIndexChanged
        Dim selectcategory As DataRow
        If ESTPURSUBITEMEMPTY Then cmbItemType_MAN.Enabled = True
        selectcategory = GetSqlRow("SELECT CATCODE,METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbCategory_MAN.Text & "' ", cn)
        Dim selecatcode As String = selectcategory.Item("catcode").ToString
        Dim selemetalcode As String = selectcategory.Item("METALID").ToString
        StrSql = " SELECT ITEMNAME"
        StrSql += " FROM " & cnAdminDb & "..ITEMMAST"
        '' as per magesh instruction to nsc
        If MetalBaseItem Then
            StrSql += " WHERE METALID= '" & selemetalcode & "'"
        Else
            StrSql += " WHERE CATCODE = '" & selecatcode & "'"
        End If
        StrSql += GetItemQryFilteration()
        objGPack.FillCombo(StrSql, cmbItem)
        If cmbItem.Items.Count > 0 Then cmbItem.Enabled = True Else cmbItem.Enabled = False : cmbSubItem_Own.Enabled = False
        cmbItem_SelectedIndexChanged(Me, New EventArgs)
        'If categorytype Then

    End Sub

    Private Sub cmbItem_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbItem.KeyPress
        e.KeyChar = UCase(e.KeyChar)
    End Sub

    Private Sub cmbItem_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItem.SelectedIndexChanged
        ' speed access if only
        If ESTPURSUBITEMEMPTY Then cmbSubItem_Own.Text = "" : Exit Sub
        If cmbItem.Text = "" Then cmbSubItem_Own.Text = "" : Exit Sub
        StrSql = GetSubItemQry(New String() {"SUBITEMNAME"}, Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "'")))
        'StrSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST "
        'StrSql += " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..itemmast where ITEMNAME = '" & cmbItem.Text & "' AND ACTIVE = 'Y')"
        objGPack.FillCombo(StrSql, cmbSubItem_Own)
        If cmbSubItem_Own.Items.Count > 0 Then cmbSubItem_Own.Enabled = True Else cmbSubItem_Own.Enabled = False
    End Sub

    Private Sub BillPurchaseDetail_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '' Add any initialization after the InitializeComponent() call.
        'StrSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY"
        'StrSql += " WHERE CATMODE IN ('B','P')"
        'If Displtype1 <> Nothing Then
        '    If Displtype1 = "E" Then StrSql += " AND ESTDISPLAY = 'Y' "
        'End If
        'If CatViewFilter Then StrSql += " AND VIEWFILTER LIKE'%PU%'"
        'StrSql += " ORDER BY DISPLAYORDER,CATNAME"
        'objGPack.FillCombo(StrSql, cmbCategory_MAN)

        'StrSql = " SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE"
        'If Not AllItemType1 Then
        '    StrSql += " WHERE SOFTMODULE = 'E' "
        'End If
        'StrSql += " ORDER BY DISPALYORDER"
        'objGPack.FillCombo(StrSql, cmbItemType_MAN)
        'If cmbItemType_MAN.Items.Count > 0 Then cmbItemType_MAN.Enabled = True Else cmbItemType_MAN.Enabled = False
        'mAlltype = AllItemType1
        cmbCategory_MAN.Select()
    End Sub

    Private Sub txtDescription_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDescription.GotFocus
        'If txtDescription.Text = "" Then
        txtDescription.Text = IIf(cmbSubItem_Own.Text <> "", cmbSubItem_Own.Text, cmbItem.Text)
        ' End If
    End Sub

    Private Sub txtDescription_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtDescription.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then

            Dim selectcategory As DataRow
            selectcategory = GetSqlRow("SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbCategory_MAN.Text & "' ", cn)
            Dim selemetalcode As String = selectcategory.Item("METALID").ToString
            If cmbCategory_MAN.Text <> "" Then
                Dim _CompMetalId As String = GetAdmindbSoftValue(strCompanyId & "_METALID", "")
                If _CompMetalId = "" Then GoTo ExitCompMetal
                If selemetalcode = "" Then GoTo ExitCompMetal
                Dim _CompMetalAr As String() = _CompMetalId.Split(",")
                If Not Array.IndexOf(_CompMetalAr, selemetalcode) >= 0 Then
                    MsgBox("Invalid Metal for this Company.", MsgBoxStyle.Information)
                    cmbCategory_MAN.Focus()
                    cmbCategory_MAN.SelectAll()
                    Exit Sub
                End If
            End If
ExitCompMetal:

            cmbCategory_MAN.Focus()
            If cmbCategory_MAN.Text = "" Or cmbCategory_MAN.Items.Contains(cmbCategory_MAN.Text) = False Then
                cmbCategory_MAN.Select()
                Exit Sub
            End If
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub cmbSubItem_Own_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubItem_Own.GotFocus
        If ESTPURSUBITEMEMPTY Then cmbSubItem_Own.Text = "" : SendKeys.Send("{TAB}")
    End Sub

    Private Sub cmbSubItem_Own_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbSubItem_Own.KeyDown
        If e.KeyCode = Keys.Enter Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub cmbSubItem_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbSubItem_Own.KeyPress
        e.KeyChar = UCase(e.KeyChar)
    End Sub

    Private Sub cmbItemType_MAN_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItemType_MAN.GotFocus
        Dim selecatcode As String = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbCategory_MAN.Text & "'").ToString
        StrSql = " SELECT TOP 1 NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE CATCODE = '" & selecatcode & "'"
        If Not mAlltype Then
            StrSql += " AND SOFTMODULE = 'E' "
        End If
        StrSql += " ORDER BY DISPALYORDER"

        Dim defitemtype As String = objGPack.GetSqlValue(StrSql) '.FillCombo(StrSql, cmbItemType_MAN)
        If cmbItemType_MAN.Items.Count > 0 And defitemtype <> "" Then cmbItemType_MAN.Text = defitemtype
        If ESTPURSUBITEMEMPTY Then cmbItemType_MAN.Enabled = False
    End Sub

        Private Sub cmb_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles _
    cmbCategory_MAN.KeyDown, cmbItem.KeyDown, cmbItemType_MAN.KeyDown
            If e.KeyCode = Keys.Enter Then
            If CType(sender, ComboBox).Text = "" Then
                MsgBox("Empty Values not Accept", MsgBoxStyle.Information)
                CType(sender, ComboBox).Select()
                Exit Sub
            End If
            If Not CType(sender, ComboBox).Items.Contains(CType(sender, ComboBox).Text) Then
                MsgBox("Invalid Selection", MsgBoxStyle.Information)
                CType(sender, ComboBox).Select()
                Exit Sub
            End If
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cmb_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles _
   cmbCategory_MAN.KeyPress, cmbItem.KeyPress, cmbSubItem_Own.KeyPress, cmbItemType_MAN.KeyPress
        e.KeyChar = UCase(e.KeyChar)
    End Sub

    Private Sub cmbSubItem_Own_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSubItem_Own.SelectedIndexChanged

    End Sub
End Class