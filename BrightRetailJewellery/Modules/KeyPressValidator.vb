Module KeyPressValidator
    Public Enum Datatype
        Number = 0
        Weight = 1
        Amount = 2
        Percentage = 3
    End Enum
    Public Sub textBoxKeyPressValidator(ByVal txt As TextBox, ByVal e As KeyPressEventArgs, ByVal type As Datatype)
        Select Case type
            Case Datatype.Number
                If Not (AscW(e.KeyChar) >= 48 And AscW(e.KeyChar) <= 57) _
                And Not (AscW(e.KeyChar) = 45) _
                And Not (e.KeyChar = Chr(Keys.Enter)) _
                    And Not (e.KeyChar = Chr(Keys.Back)) _
                    And Not (e.KeyChar = Chr(Keys.Escape)) _
                    And Not (e.KeyChar = Chr(Keys.Delete)) Then
                    e.Handled = True
                End If
            Case Datatype.Amount
                If e.KeyChar = "." And txt.Text.Contains(".") Then
                    e.Handled = True
                    Exit Sub
                End If
                If e.KeyChar = "-" And txt.Text.Contains("-") Then
                    e.Handled = True
                    Exit Sub
                End If

                If Not (AscW(e.KeyChar) >= 48 And AscW(e.KeyChar) <= 57) _
                And Not (AscW(e.KeyChar) = 45) _
                And Not (e.KeyChar = Chr(Keys.Enter)) _
                    And Not (e.KeyChar = Chr(Keys.Back)) _
                    And Not (e.KeyChar = Chr(Keys.Escape)) _
                    And Not (e.KeyChar = Chr(Keys.Delete)) Then
                    e.Handled = True
                End If
                If txt.Text.Contains(".") Then
                    Dim dotPos As Integer = InStr(txt.Text, ".", CompareMethod.Text)
                    Dim sp() As String = txt.Text.Split(".")
                    Dim curPos As Integer = txt.SelectionStart
                    If sp.Length >= 2 Then
                        If curPos >= dotPos Then
                            If sp(1).Length > 1 Then
                                e.Handled = True
                            End If
                        End If
                    End If
                End If
            Case Datatype.Weight
                If e.KeyChar = "." And txt.Text.Contains(".") Then
                    e.Handled = True
                    Exit Sub
                End If
                If e.KeyChar = "-" And txt.Text.Contains("-") Then
                    e.Handled = True
                    Exit Sub
                End If
                If Not (AscW(e.KeyChar) >= 48 And AscW(e.KeyChar) <= 57) _
                And Not (AscW(e.KeyChar) = 45) _
               And Not (e.KeyChar = Chr(Keys.Enter)) _
                   And Not (e.KeyChar = Chr(Keys.Back)) _
                   And Not (e.KeyChar = Chr(Keys.Escape)) _
                   And Not (e.KeyChar = Chr(Keys.Delete)) Then
                    e.Handled = True
                End If
                If txt.Text.Contains(".") Then
                    Dim dotPos As Integer = InStr(txt.Text, ".", CompareMethod.Text)
                    Dim sp() As String = txt.Text.Split(".")
                    Dim curPos As Integer = txt.SelectionStart
                    If sp.Length >= 2 Then
                        If curPos >= dotPos Then
                            If sp(1).Length > 2 Then
                                e.Handled = True
                            End If
                        End If
                    End If
                End If
            Case Datatype.Percentage
                Dim value As String = txt.Text
                If e.KeyChar = "." And txt.Text.Contains(".") Or Val(txt.Text + e.KeyChar) > 100 Then
                    e.Handled = True
                    Exit Sub
                End If
                If e.KeyChar = "-" And txt.Text.Contains("-") Then
                    e.Handled = True
                    Exit Sub
                End If

                Select Case e.KeyChar
                    Case "0" To "9", ".", ChrW(Keys.Back), ChrW(Keys.Escape), "-"
                        If txt.Text.Contains(".") Then
                            Dim sp() As String = txt.Text.Split(".")
                            If sp.Length >= 2 Then
                                If txt.SelectionStart >= InStr(txt.Text, ".", CompareMethod.Text) Then
                                    If sp(1).Length > 1 Then
                                        e.Handled = True
                                        Exit Sub
                                    End If
                                End If
                            End If
                        End If
                        value += e.KeyChar
                        If Val(value) > 100 Then
                            e.Handled = True
                        End If
                    Case Else
                        e.Handled = True
                End Select
        End Select
    End Sub
End Module
