Imports System.Data.OleDb
Imports System.Drawing.Text
Public Class LanguageChange
    Private Shared G_cmbFonts As New ComboBox
    Shared strSql As String = Nothing
#Region "Manages Control Captions"
    Public Shared Sub Set_Language_Form(ByVal f As Form, ByVal languageId As String)
        If languageId = "ENG" Or languageId = "" Then Exit Sub
        G_cmbFonts.Items.Clear()
        Dim installed_fonts As New InstalledFontCollection
        Dim fontName As String = Nothing
        G_DTable = New DataTable
        G_DAdapter = New OleDbDataAdapter(" SELECT FONT FROM " & G_CnAdmindb & "..LANGMASTER WHERE LANGID = '" & languageId & "'", G_Cn)
        G_DAdapter.Fill(G_DTable)
        If G_DTable.Rows.Count > 0 Then fontName = G_DTable.Rows(0).Item(0).ToString

        If fontName = Nothing Then Exit Sub

        'Get an array of the system's font familiies.
        Dim font_families() As FontFamily = installed_fonts.Families()
        'Display the font families.
        For Each font_family As FontFamily In font_families
            G_cmbFonts.Items.Add(UCase(font_family.Name))
        Next font_family

        If Not G_cmbFonts.Items.Contains(fontName) Then Exit Sub
        Set_LanguageControls(f, f, fontName, languageId) ''dt.Rows(0).Item("FONT").ToString
    End Sub

    Public Shared Function Get_LanguageCaption(ByVal languageId As String, ByVal frm As Form, ByVal ctr As Control) As String
        If languageId = Nothing Then Return ctr.Text
        strSql = " SELECT CTRLCAPTION FROM " & G_CnAdmindb & "..CAPTIONMASTER WHERE"
        strSql += " LANGID = '" & languageId & "'"
        strSql += " AND FRMID = '" & frm.Name & "'"
        strSql += " AND CTRLID = '" & ctr.Name & "'"
        G_DAdapter = New OleDbDataAdapter(strSql, G_Cn)
        G_DTable = New DataTable
        G_DAdapter.Fill(G_DTable)
        If G_DTable.Rows.Count > 0 Then Return G_DTable.Rows(0).Item("CTRLCAPTION").ToString
        Return Nothing
    End Function

    Private Shared Sub Set_LanguageControls(ByVal frm As Form, ByVal f As Object, ByVal ffont As String, ByVal languageId As String)
        For Each obj As Object In CType(f, Control).Controls
            If TypeOf obj Is Label Then
                Dim lblText As String = Get_LanguageCaption(languageId, frm, CType(obj, Label))
                If lblText <> Nothing Then
                    CType(obj, Label).Font = New Font(ffont, 12, FontStyle.Regular)
                    CType(obj, Label).Text = lblText
                End If
            Else
                Dim dt As New DataTable
                Set_LanguageControls(frm, obj, ffont, languageId)
            End If
        Next
    End Sub
#End Region

End Class
