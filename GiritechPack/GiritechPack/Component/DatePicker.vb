Public Class DatePicker
    Inherits System.Windows.Forms.TextBox

    Dim MinDate As Date = CDate("1753-01-01")
    Dim MaxDate As Date = CDate("9998-12-31")

    Private sep As Char = CChar("-")
    Private mstrMask As String = "##" & Seperator & "##" & Seperator & "####"
    Private mParams() As Char = {CChar("#"), CChar("&"), CChar("!")}


    Public Property Mask() As String
        Get
            Return (mstrMask)
        End Get

        Set(ByVal Value As String)
            'Use # for Digit only
            'Use & for Letter only
            'Use ! for Letter or Digit
            mstrMask = Value
            Me.Text = mstrMask
            Me.Text = Format(Today.Day, "00") & Seperator & Format(Today.Month, "00") & Seperator & Today.Year
            'me.Text = me.Text.Replace("#", "_").Replace("&", "_").Replace("!", "_")
        End Set
    End Property

    Public Property Seperator() As Char
        Get
            If sep = "" Then sep = "-"
            Return sep
        End Get
        Set(ByVal value As Char)
            sep = value
            Mask = mstrMask
        End Set
    End Property

    Public Property MaximumDate() As Date
        Get
            Return MaxDate
        End Get
        Set(ByVal value As Date)
            MaxDate = value
        End Set
    End Property

    Public Property MinimumDate() As Date
        Get
            Return MinDate
        End Get
        Set(ByVal value As Date)
            MinDate = value
        End Set
    End Property

    Public Property Value() As Date
        Get
            Return GetDate()
        End Get
        Set(ByVal value As Date)
            Me.Text = Format(value.Day, "00") & Seperator & Format(value.Month, "00") & Seperator & Format(value.Year, "0000")
        End Set
    End Property

    Private Function GetDate() As Object
        Dim sp() As String = Me.Text.Split(Seperator)
        Dim dd As Integer = Val(sp(0))
        Dim mm As Integer
        Dim yy As Integer
        If Not sp.Length > 1 Then mm = 0 Else mm = Val(sp(1))
        If Not sp.Length > 2 Then yy = 0 Else yy = Val(sp(2))
        If Not (yy >= MinimumDate.Year And yy <= MaximumDate.Year) Then
            If mm < 4 Then
                yy = MaximumDate.Year
            Else
                yy = MinimumDate.Year
            End If
        End If
        If dd = 0 Then dd = Today.Day
        If mm = 0 Then mm = Today.Month
        Return (Format(yy, "0000") & Seperator & Format(mm, "00") & Seperator & Format(dd, "00"))
    End Function

#Region "Events"

    Protected Overrides Sub OnGotFocus(ByVal e As System.EventArgs)
        Me.SelectionStart = 0
    End Sub

    Protected Overrides Sub OnKeyDown(ByVal e As System.Windows.Forms.KeyEventArgs)

        'Disables Delete Key
        If e.KeyCode = Keys.Delete Then
            e.Handled = True
        End If

    End Sub

    Protected Overrides Sub OnLeave(ByVal e As System.EventArgs)
        Dim dat As Object = GetDate()
        Try
            If Not IsDate(Convert.ToDateTime(dat).ToString("yyyy-MM-dd")) Then
                MsgBox("Invalid Date", MsgBoxStyle.Information)
                Me.Focus()
                Exit Sub
            End If
        Catch ex As Exception
            MsgBox("Invalid Date", MsgBoxStyle.Information)
            Me.Focus()
            Exit Sub
        End Try
        'Dim ddate As Date = CDate(dat)
        'Me.Text = Format(ddate.Day, "00") & Seperator & Format(ddate.Month, "00") & Seperator & Format(ddate.Year, "0000")
        'Dim tDate As Date = GetDate()
        'If Not (tDate >= MinimumDate And tDate <= MaximumDate) Then
        '    Dim msg As String = "Date Between "
        '    msg += Format(MinimumDate.Day, "00") & Seperator & Format(MinimumDate.Month, "00") & Seperator & Format(MinimumDate.Year, "0000")
        '    msg += " And "
        '    msg += Format(MaximumDate.Day, "00") & Seperator & Format(MaximumDate.Month, "00") & Seperator & Format(MaximumDate.Year, "0000")
        '    msg += " Only Allowed"
        '    MsgBox(msg, MsgBoxStyle.Information)
        '    Me.Focus()
        '    Exit Sub
        'End If
    End Sub

    Protected Overrides Sub OnKeyPress(ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim dat As Object = GetDate()
            Try
                If Not IsDate(Convert.ToDateTime(dat).ToString("yyyy-MM-dd")) Then
                    MsgBox("Invalid Date", MsgBoxStyle.Information)
                    Me.Focus()
                    Exit Sub
                End If
            Catch ex As Exception
                MsgBox("Invalid Date", MsgBoxStyle.Information)
                Me.Focus()
                Exit Sub
            End Try
            'If Not IsDate(dat) Then
            '    MsgBox("Invalid Date", MsgBoxStyle.Information)
            '    Me.Focus()
            '    Exit Sub
            'End If
            Dim ddate As Date = CDate(dat)
            Me.Text = Format(ddate.Day, "00") & Seperator & Format(ddate.Month, "00") & Seperator & Format(ddate.Year, "0000")
            Dim tDate As Date = GetDate()
            If Not (tDate >= MinimumDate And tDate <= MaximumDate) Then
                Dim msg As String = "Date Between "
                msg += Format(MinimumDate.Day, "00") & Seperator & Format(MinimumDate.Month, "00") & Seperator & Format(MinimumDate.Year, "0000")
                msg += " And "
                msg += Format(MaximumDate.Day, "00") & Seperator & Format(MaximumDate.Month, "00") & Seperator & Format(MaximumDate.Year, "0000")
                msg += " Only Allowed"
                MsgBox(msg, MsgBoxStyle.Information)
                Me.Focus()
                Exit Sub
            End If
            Exit Sub
        End If
        'key pressed
        Dim chrKeyPressed As Char = e.KeyChar
        'Original cursor position
        Dim intSelStart As Integer = Me.SelectionStart
        'In case of a selection, delete text to this position
        Dim intDelTo As Integer = Me.SelectionStart + Me.SelectionLength - 1

        Dim strText As String = Me.Text
        'Used to avoid deletion of the selection when an invalid key is pressed
        Dim bolDelete As Boolean = False

        Dim i As Integer

        e.Handled = True

        If chrKeyPressed = ControlChars.Back Then
            bolDelete = True
            If intSelStart > 0 And intDelTo < intSelStart Then
                intSelStart -= 1
            End If
        End If

        If intSelStart = 0 Then 'd1
            If Val(chrKeyPressed) > 3 Then Exit Sub
        ElseIf intSelStart = 1 Then 'd2
            If Val(Mid(Me.Text, 1, 1)) & Val(chrKeyPressed) > 31 Then Exit Sub
        ElseIf intSelStart = 3 Then 'm1
            If Val(chrKeyPressed) > 1 Then Exit Sub
        ElseIf intSelStart = 4 Then 'm2
            If Val(Mid(Me.Text, 4, 1)) & Val(chrKeyPressed) > 12 Then Exit Sub
        End If

        'Find the Next Insertion point
        For i = Me.SelectionStart To mstrMask.Length - 1

            'Test for # or &
            If mstrMask.Chars(i) = "#" AndAlso Char.IsDigit(chrKeyPressed) _
                OrElse mstrMask.Chars(i) = "&" AndAlso Char.IsLetter(chrKeyPressed) _
                OrElse mstrMask.Chars(i) = "!" AndAlso Char.IsLetterOrDigit(chrKeyPressed) Then

                strText = strText.Remove(i, 1).Insert(i, chrKeyPressed)
                intSelStart = i + 1
                bolDelete = True

            End If

            'Prevent looping unitl the next available match when mixing # & ! on the same mask
            If Array.IndexOf(mParams, mstrMask.Chars(i)) > -1 Then
                Exit For
            End If

        Next

        'Delete remaining chars from selection or previous char if backspace
        If bolDelete Then
            For i = intSelStart To intDelTo
                If Array.IndexOf(mParams, mstrMask.Chars(i)) > -1 Then
                    strText = strText.Remove(i, 1).Insert(i, "_")
                End If
            Next
            Me.Text = strText
            Me.SelectionStart = intSelStart
            Me.SelectionLength = 0
        End If
    End Sub
#End Region

End Class
