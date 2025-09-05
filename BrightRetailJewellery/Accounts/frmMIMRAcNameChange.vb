Imports System.Data.OleDb
Public Class frmMIMRAcNameChange

#Region " Variable"
    Dim strsql As String = ""
    Dim da As OleDbDataAdapter
    Dim dt As New DataTable
    Dim cmd As OleDbCommand
    Public InclCusttype As String = ""
    Dim dtAcnameOld As New DataTable
    Dim dtAcnameNew As New DataTable
#End Region

#Region "User Define Function"
    Private Function funcAcNameStatecode(ByVal funAccode As String) As String
        Dim AcStatecode As String = ""
        strsql = " SELECT ISNULL(STATECODE,'') STATECODE FROM " & cnAdminDb & "..STATEMAST WHERE  "
        strsql += " STATEID IN(SELECT STATEID FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & funAccode & "') "
        AcStatecode = objGPack.GetSqlValue(strsql).ToString
        Return AcStatecode
    End Function
    Public Function strLoadAcname() As String
        Dim Qry As String = ""
        Qry = " "
        Qry += vbCrLf + " SELECT ACCODE,ACNAME FROM ("
        Qry += vbCrLf + " SELECT '' ACCODE, '' ACNAME,0 RESULT"
        Qry += vbCrLf + " UNION ALL"
        Qry += vbCrLf + " SELECT ACCODE,ACNAME,1 RESULT FROM " & cnAdminDb & "..ACHEAD "
        Qry += vbCrLf + " WHERE ISNULL(ACTIVE,'') = 'Y' "
        If InclCusttype = "Y" Then
            Qry += " AND ACTYPE IN (SELECT DISTINCT TYPEID FROM " & cnAdminDb & "..ACCTYPE "
            Qry += " WHERE ACTYPE IN ('G','D','I','C'))"
        Else
            Qry += " AND ACTYPE IN (SELECT DISTINCT TYPEID FROM " & cnAdminDb & "..ACCTYPE "
            Qry += " WHERE ACTYPE IN ('G','D','I'))"
        End If
        Qry += vbCrLf + " )X"
        Qry += vbCrLf + " ORDER BY RESULT,ACNAME"
        Return Qry
    End Function

    Public Sub LoadAcname()
        strsql = strLoadAcname()
        dtAcnameOld = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtAcnameOld)
        If dtAcnameOld.Rows.Count > 0 Then
            cmbOldAcName.DataSource = Nothing
            cmbOldAcName.DataSource = dtAcnameOld
            cmbOldAcName.ValueMember = "ACCODE"
            cmbOldAcName.DisplayMember = "ACNAME"
            cmbOldAcName.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cmbOldAcName.AutoCompleteSource = AutoCompleteSource.ListItems
        End If

        strsql = strLoadAcname()
        dtAcnameNew = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtAcnameNew)
        If dtAcnameNew.Rows.Count > 0 Then
            cmbNewAcName.DataSource = Nothing
            cmbNewAcName.DataSource = dtAcnameNew
            cmbNewAcName.ValueMember = "ACCODE"
            cmbNewAcName.DisplayMember = "ACNAME"
            cmbNewAcName.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cmbNewAcName.AutoCompleteSource = AutoCompleteSource.ListItems
        End If
    End Sub
#End Region

#Region " Constructor"
    Public Sub New(ByVal InclCust As String)
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        InclCusttype = InclCust
        strsql = ""
        LoadAcname()
    End Sub
#End Region

#Region " Form Load"
    Private Sub frmMIMRAcNameChange_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
    Private Sub frmMIMRAcNameChange_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{tab}")
        End If
    End Sub
#End Region

#Region " Button Events"
    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        If cmbNewAcName.Text = "" Then
            MsgBox("New Acname should not empty", MsgBoxStyle.Information)
            cmbNewAcName.Focus()
            cmbNewAcName.SelectAll()
            Exit Sub
        End If
        If cmbOldAcName.Text = "" Then
            MsgBox("Old Acname should not empty", MsgBoxStyle.Information)
            cmbNewAcName.Focus()
            cmbNewAcName.SelectAll()
            Exit Sub
        End If

        If cmbOldAcName.Text = cmbNewAcName.Text Then
            MsgBox("Same AcName not allowed", MsgBoxStyle.Information)
            Exit Sub
        End If

        Dim OldStatecode As String = funcAcNameStatecode(cmbOldAcName.SelectedValue.ToString)
        Dim NewStatecode As String = funcAcNameStatecode(cmbOldAcName.SelectedValue.ToString)
        If OldStatecode = NewStatecode Then
        Else
            MsgBox("Statecode mismatch " & vbCrLf & " Old Statecode : " & OldStatecode & vbCrLf & " New Statecode :  " & NewStatecode & vbCrLf & "", MsgBoxStyle.Information)
            Exit Sub
        End If

        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
#End Region

End Class