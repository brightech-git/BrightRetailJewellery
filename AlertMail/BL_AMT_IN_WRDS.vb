Imports ADODB
Imports System.Configuration
Imports System.Collections

' Authors Kalaiarasan T

Public Class BL_AMT_IN_WRDS

    Dim a(10, 10) As String
    Dim b(10) As String
    Public Amt As String
    Dim last_rup As String = "Rupees  "
    Dim last_pais As String = " Paise "
    Public Rup_str As String

    Public Function Storage()
        Dim i, j, k
        i = 0
        k = 1
        For j = 0 To 9
            Select Case j
                Case 0
                    b(j) = ""           'b(0)
                Case 1
                    a(i, j) = "One "         'a(0)(1)
                    a(k, j) = "Eleven "  'a(1)(1)
                    a(j, i) = "Ten "
                    b(j) = ""         'b(1)
                Case 2
                    a(i, j) = "Two "         'a(0)(2)
                    a(k, j) = "Twelve "      'a(1)(2)
                    a(j, i) = "Twenty "     'a(2)(0)
                    b(j) = ""     'b(2)
                Case 3
                    a(i, j) = "Three "       'a(0)(3)
                    a(k, j) = "Thirteen "    'a(1)(3)
                    a(j, i) = "Thirty "      'a(3)(0)
                    b(j) = "Hundred "     'b(3)
                Case 4
                    a(i, j) = "Four "       'a(0)(4)
                    a(k, j) = "Fourteen "   'a(1)(4)
                    a(j, i) = "Fourty "      'a(4)(0)
                    b(j) = "Thousand "    'b(4)
                Case 5
                    a(i, j) = "Five "       'a(0)(5)
                    a(k, j) = "Fifteen "   'a(1)(5)
                    a(j, i) = "Fifty "      'a(5)(0)
                    b(j) = "Thousand "    'b(5)
                Case 6
                    a(i, j) = "Six "       'a(0)(6)
                    a(k, j) = "Sixteen "   'a(1)(6)
                    a(j, i) = "Sixty "      'a(6)(0)
                    b(j) = "Lakh "    'b(6)
                Case 7
                    a(i, j) = "Seven "       'a(0)(7)
                    a(k, j) = "Seventeen "   'a(1)(7)
                    a(j, i) = "Seventy "      'a(7)(0)
                    b(j) = "Lakhs "    'b(7)
                Case 8
                    a(i, j) = "Eight "       'a(0)(8)
                    a(k, j) = "Eighteen "   'a(1)(8)
                    a(j, i) = "Eighty "      'a(8)(0)
                    b(j) = "Crores "    'b(8)
                Case 9
                    a(i, j) = "Nine "       'a(0)(9)
                    a(k, j) = "Ninteen "   'a(1)(9)
                    a(j, i) = "Ninty "      'a(9)(0)
                    b(j) = "Crores "    'b(9)
                Case 10
                    b(j) = "Million "    'b(10)
                Case 11
                    b(j) = "billion "    'b(11)
            End Select
        Next
    End Function

    Public Function Rec_Loop_Amt(ByVal str As String, ByVal pos As String, ByVal no As String, Optional ByVal ind As String = "")
        Dim cur_no, cur_no1 As Integer
        cur_no = Mid$(str, pos, 1)
        Dim del_ind As Integer = 2
        If no = 1 Then                                  ' For Last Number 
            Rup_str = Rup_str & a(0, cur_no)
        ElseIf no = 3 Then                              ' Split the Hundred
            If cur_no <> 0 Then
                Rup_str = Rup_str & a(0, cur_no) & b(no)
                If Int((Val(Mid$(str, 2, 1)) + Val(Mid$(str, 3, 1)))) <> 0.0 Then Rup_str = Rup_str & "And "
            Else
                Rup_str = Rup_str & b(cur_no)
            End If
            del_ind = 1
        Else
            cur_no1 = Mid$(str, pos + 1, 1)
            If (no = 2) Or ((no - 3) Mod 2 = 0) Then    'For split to two digits 
                If cur_no = 1 Then                      ' For 11,12,13... series
                    Rup_str = Rup_str & a(1, cur_no1) & b(no)
                ElseIf cur_no = 0 And cur_no1 <> 0 Then                   ' For sense 0 digit
                    Rup_str = Rup_str & a(0, cur_no1) & b(no)
                Else                                    ' Take 20,21 ...
                    If cur_no <> 0 Then
                        Rup_str = Rup_str & a(cur_no, 0) & a(0, cur_no1) & b(no)
                    End If
                End If
            Else                                        ' For One Digit
                If cur_no = 0 Then
                    Rup_str = Rup_str & b(cur_no)
                Else
                    Rup_str = Rup_str & a(0, cur_no) & b(no)
                End If
                del_ind = 1
            End If
        End If
        If no = 1 Or no = 2 Then
            str = ""
        Else
            str = Mid$(str, pos + del_ind, no)
        End If
        no = Len(Trim(str))
        If pos <= no Then
            Rec_Loop_Amt(str, 1, no, 0)
        Else
            Exit Function
        End If
    End Function


    Public Function get_input(ByVal amtinw As String) As String
        If amtinw = 0.0 Then Return "" ' this line insert by Kalaiarasan T for value =0 
        Storage()
        'Getting input
        Amt = Trim(amtinw)
        Dim l = Len(Trim(Amt))
        Dim inc As Integer = 1
        Rup_str = ""

        Dim n As Integer
        n = InStr(1, Amt, ".", CompareMethod.Text)
        If n > 0 Then  'And Mid$(Amt, n + 1, 1) <> 0 And Mid$(Amt, n + 2, 1) <> 0 Then
            Dim pais As String
            Dim pais1 As Integer
            'Amt = RndInt(Amt))
            Dim Paise_flag As Boolean = False
            pais = Mid$(Amt, n + 1, l)
            If Len(pais) >= 2 Then
                pais = Mid$(pais, 1, 2)
                If Val((Mid$(pais, 1, 2)) + Val(Mid$(pais, 2, 1))) = 0 Then
                    Paise_flag = True
                End If
            ElseIf Len(pais) = 1 Then
                pais = pais & "0"
            End If
            Amt = Mid$(Amt, 1, n - 1)
            Dim R_str As String
            Rup_str = ""
            Rec_Loop_Amt(Amt, 1, Len(Amt), )
            If Not Paise_flag Then

                    R_str = last_rup & " " & Rup_str & "and"
                Rup_str = ""
                Rec_Loop_Amt(pais, 1, Len(pais), )
                Rup_str = R_str & last_pais & Rup_str & "Only." '& last_pais
            Else
                Return last_rup & " " & Rup_str & "Only." '& last_rup
            End If
        Else
            Rec_Loop_Amt(Amt, 1, l, )
            Rup_str = last_rup & Rup_str & "Only." '& last_rup
        End If
        Return Rup_str
    End Function



End Class