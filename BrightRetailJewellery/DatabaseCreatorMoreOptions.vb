Public Class DatabaseCreatorMoreOptions
    Public Sub New(ByVal selectedModules As List(Of String), ByVal userLimit As Integer)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()


        ' Add any initialization after the InitializeComponent() call.
        dudLimit.Items.Clear()
        For cnt As Integer = 1 To 100
            dudLimit.Items.Add(cnt.ToString)
        Next
        For cnt As Integer = 0 To chkLstModules.Items.Count - 1
            If selectedModules.Contains(chkLstModules.Items.Item(cnt).ToString.ToUpper) Then
                chkLstModules.SetItemChecked(cnt, True)
            End If
        Next
        dudLimit.Text = userLimit
    End Sub
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        dudLimit.Items.Clear()
        For cnt As Integer = 1 To 100
            dudLimit.Items.Add(cnt.ToString)
        Next
    End Sub

    Private Sub DatabaseCreatorMoreOptions_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub MoreOptions_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
    Private Function GetCheckedItems() As List(Of String)
        Dim lst As New List(Of String)
        For cnt As Integer = 0 To chkLstModules.CheckedItems.Count - 1
            lst.Add(chkLstModules.CheckedItems.Item(cnt).ToString)
        Next
        Return lst
    End Function

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If Not chkLstModules.CheckedItems.Count > 0 Then
            chkLstModules.Focus()
            Exit Sub
        End If
        If Val(dudLimit.Text) = 0 Then
            dudLimit.Focus()
            Exit Sub
        End If
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
End Class