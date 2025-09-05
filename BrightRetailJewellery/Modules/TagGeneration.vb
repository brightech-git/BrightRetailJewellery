Imports System.Data.OleDb
Public Class TagGeneration
    Dim strSql As String
    Dim _TagNoFrom As String
    Public _TagNoGen As String ' Dim Replace Public
    Dim _TAG_RANDOM_ITEM As Boolean
    Dim TagPrefix As String = GetAdmindbSoftValue("TAGPREFIX", , tran)
    Dim IsMainCostCentre As Boolean
    Dim RANDOM_UNIQUE_TAG As String
    Dim RANDOM_UNIQUE_TAG_NEW As String
    Private Tag_random As Random = New Random
    Dim TagSystemPrefix As String
    Dim Comp_TagPrefix As String = ""
    Dim TagPrefix_Item As Boolean = IIf(GetAdmindbSoftValue("TAGPREFIX_ITEM", "N") = "Y", True, False)

    Public Sub New()
        Comp_TagPrefix = GetAdmindbSoftValue("TAGPREFIX_" & strCompanyId, , tran)
        If Comp_TagPrefix <> "" Then
            TagPrefix = Comp_TagPrefix
        End If
        _TagNoFrom = UCase(GetAdmindbSoftValue("TAGNOFROM"))
        _TagNoGen = UCase(GetAdmindbSoftValue("TAGNOGEN"))
        _TAG_RANDOM_ITEM = IIf(GetAdmindbSoftValue("TAG_RANDOM_ITEM", "N") = "Y", True, False)
        RANDOM_UNIQUE_TAG = GetAdmindbSoftValue("RANDOM_UNIQUE_TAG", "N")
        RANDOM_UNIQUE_TAG_NEW = GetAdmindbSoftValue("RANDOM_UNIQUE_TAG_NEW", "N")
        Try
            If Mid(RANDOM_UNIQUE_TAG, 1, 1) = "Y" Then
                Dim randomuniq() As String = RANDOM_UNIQUE_TAG.Split(",")
                If randomuniq.Length < 3 Then RANDOM_UNIQUE_TAG = "Y,1,99999999"
                GoTo NEXT1
            End If
            If Mid(RANDOM_UNIQUE_TAG_NEW, 1, 1) = "Y" Then
                Dim randomuniq() As String = RANDOM_UNIQUE_TAG_NEW.Split(",")
                If randomuniq.Length < 2 Then RANDOM_UNIQUE_TAG_NEW = "Y,8"
                GoTo NEXT1
                End If
                If _TagNoFrom = "I" Then
                If Not (_TagNoGen = "I" Or _TagNoGen = "L") Then
                    MsgBox("[TagNoGen] Settings not set properly", MsgBoxStyle.Information)
                    _TagNoFrom = Nothing
                    _TagNoGen = Nothing
                End If
            ElseIf _TagNoFrom = "U" And Not _TAG_RANDOM_ITEM Then  ''UNIQUE
                If Not (_TagNoGen = "Y" Or _TagNoGen = "N" Or _TagNoGen = "M" Or _TagNoGen = "T" Or _TagNoGen = "D" Or _TagNoGen = "W") Then ''Y-yEAR, N-Numeric, M-month, T-Time, D-Dayswise, W-weightWise
                    MsgBox("[TagNoGen] Settings not set properly", MsgBoxStyle.Information)
                    _TagNoFrom = Nothing
                    _TagNoGen = Nothing
                End If
            Else
                MsgBox("[TagNoFrom] Settings not set properly", MsgBoxStyle.Information)
                _TagNoFrom = Nothing
                _TagNoGen = Nothing
            End If
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
NEXT1:
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SYNC-TO'"
        If objGPack.GetSqlValue(strSql) = "" Then
            IsMainCostCentre = True
        Else
            IsMainCostCentre = False
        End If

        If _TagNoGen = "T" Or _TagNoGen = "D" Or _TagNoGen = "W" Then
            Dim dr As DataRow = Nothing
            strSql = " SELECT COUNT(*)CNT,SYSTEMPREFIX FROM " & cnAdminDb & "..TAGSYSTEM "
            strSql += vbCrLf + " WHERE SYSTEMNAME = '" & Environment.MachineName & "' "
            strSql += vbCrLf + " GROUP BY SYSTEMPREFIX "
            dr = GetSqlRow(strSql, cn)
            If Not dr Is Nothing Then
                TagSystemPrefix = dr.Item("SYSTEMPREFIX").ToString
            Else
                MsgBox("TagSystem Name not found in master", MsgBoxStyle.Information)
                _TagNoFrom = Nothing
                _TagNoGen = Nothing
                TagSystemPrefix = ""
            End If

            strSql = " select DATA_TYPE from " & cnAdminDb & ".INFORMATION_SCHEMA.COLUMNS IC "
            strSql += vbCrLf + " where TABLE_NAME = 'ITEMTAG' "
            strSql += vbCrLf + " and COLUMN_NAME = 'TAGVAL' "
            If GetSqlValue(cn, strSql).ToString = "int" Then
                MsgBox("TAGVAL column Should be BIGINT ", MsgBoxStyle.Information)
                _TagNoFrom = Nothing
                _TagNoGen = Nothing
            End If

            If _TagNoGen = "D" Or _TagNoGen = "W" Then
                strSql = vbCrLf + "   select COUNT(*) cnt from ("
                strSql += vbCrLf + "   select TABLE_NAME,COLUMN_NAME,CHARACTER_MAXIMUM_LENGTH as _len, "
                strSql += vbCrLf + "  'ALTER TABLE " & cnAdminDb & "..' + TABLE_NAME + ' ALTER COLUMN ' +COLUMN_NAME +' VARCHAR(15)' as VALUE"
                strSql += vbCrLf + "   from " & cnAdminDb & ".INFORMATION_SCHEMA.COLUMNS "
                strSql += vbCrLf + "   where COLUMN_NAME Like '%tagno%'"
                strSql += vbCrLf + "   union all"
                strSql += vbCrLf + "   select TABLE_NAME,COLUMN_NAME,CHARACTER_MAXIMUM_LENGTH as _len, "
                strSql += vbCrLf + "  'ALTER TABLE " & cnStockDb & "..' + TABLE_NAME + ' ALTER COLUMN ' +COLUMN_NAME +' VARCHAR(15)' AS VALUE"
                strSql += vbCrLf + "   from " & cnStockDb & ".INFORMATION_SCHEMA.COLUMNS "
                strSql += vbCrLf + "   where COLUMN_NAME Like '%tagno%'"
                strSql += vbCrLf + "   )x where _len <= 12 "
                If GetSqlValue(cn, strSql) > 0 Then
                    MsgBox("TAGNO column Should be VARCHAR(15) Must Admindb & Trandb ", MsgBoxStyle.Information)
                    _TagNoFrom = Nothing
                    _TagNoGen = Nothing
                End If
                strSql = vbCrLf + " SELECT COUNT(*) CNT FROM ("
                strSql += vbCrLf + "  SELECT DISTINCT TABLE_NAME,COLUMN_NAME,CHARACTER_MAXIMUM_LENGTH as _len, "
                strSql += vbCrLf + " 'ALTER TABLE " & cnAdminDb & "..'+ TABLE_NAME + ' ALTER COLUMN ' +COLUMN_NAME + ' VARCHAR(15) NOT NULL ' AS VALUE"
                strSql += vbCrLf + " FROM " & cnAdminDb & ".INFORMATION_SCHEMA.COLUMNS "
                strSql += vbCrLf + "  where TABLE_NAME Like '%ITEMTAG%' AND COLUMN_NAME IN ('SNO') "
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + "  SELECT DISTINCT TABLE_NAME,COLUMN_NAME,CHARACTER_MAXIMUM_LENGTH as _len, "
                strSql += vbCrLf + " 'ALTER TABLE " & cnAdminDb & "..'+ TABLE_NAME + ' ALTER COLUMN ' +COLUMN_NAME + ' VARCHAR(15) NOT NULL ' AS VALUE"
                strSql += vbCrLf + " FROM " & cnAdminDb & ".INFORMATION_SCHEMA.COLUMNS "
                strSql += vbCrLf + "  where TABLE_NAME Like '%ITEMTAG%' AND COLUMN_NAME IN ('TAGSNO') "
                strSql += vbCrLf + " )X WHERE _len < 15 "
                If GetSqlValue(cn, strSql) > 0 Then
                    MsgBox(" SNO column Should be VARCHAR(15) NOT NULL PRIMARY KEY Must Admindb " & vbCrLf & "TAGSNO column should be varchar(15)", MsgBoxStyle.Information)
                    _TagNoFrom = Nothing
                    _TagNoGen = Nothing
                End If
            End If
        End If
    End Sub
    Private Function GenTagNo(ByRef TagNo As String, Optional ByVal tran As OleDbTransaction = Nothing, Optional ByVal increament As Boolean = True) As String
        Dim str As String = Nothing
        If IsNumeric(TagNo) Then
            If increament Then TagNo = Val(TagNo) + 1 Else TagNo = Val(TagNo) - 1
        Else
            Dim fPart As String = Nothing
            Dim sPart As String = Nothing
            For Each c As Char In TagNo
                If IsNumeric(c) Then
                    sPart += c
                Else
                    fPart += c
                End If
            Next
            If increament Then TagNo = fPart + (Val(sPart) + 1).ToString Else TagNo = fPart + (IIf(Val(sPart) - 1 > 0, Val(sPart) - 1, 0)).ToString
        End If
        Return TagPrefix + TagNo
    End Function

    Private Function GenerateTagNo(ByVal RecDate As Date, ByVal itemName As String, ByVal lotSno As String, Optional ByVal tran As OleDbTransaction = Nothing) As String
        Dim tagNo As String = Nothing
        Dim str As String = Nothing
        If Mid(RANDOM_UNIQUE_TAG, 1, 1) = "Y" Then tagNo = RANDOM_TAGNO_UNIQUE(RANDOM_UNIQUE_TAG) : Return tagNo : Exit Function
        If Mid(RANDOM_UNIQUE_TAG_NEW, 1, 1) = "Y" Then tagNo = IIf(TagPrefix.ToString <> "", TagPrefix, "") & RANDOM_TAGNO_UNIQUE_NEW(RANDOM_UNIQUE_TAG_NEW) : Return tagNo : Exit Function
        If _TagNoFrom = "I" Then ''FROM ITEMMAST OR UNIQUE
            If _TagNoGen = "I" Then ''FROM ITEM
                If TagPrefix_Item = True Then
                    TagPrefix = ""
                    str = " SELECT SUBSTRING(SHORTNAME,1,5)SHORTNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = "
                    str += " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & itemName & "')"
                    TagPrefix = objGPack.GetSqlValue(str, , "", tran)
                End If
                If _TAG_RANDOM_ITEM Then
                    str = " SELECT STARTTAG LASTTAGNO FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = "
                    str += " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & itemName & "')"
                    tagNo = GenTagNo(objGPack.GetSqlValue(str, , "1", tran), tran, False)
                    tagNo = GenTagNo(tagNo, tran)
                Else
                    str = " SELECT CURRENTTAGNO LASTTAGNO FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = "
                    str += " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & itemName & "')"
                    tagNo = GenTagNo(objGPack.GetSqlValue(str, , "1", tran), tran)
                End If
            ElseIf _TagNoGen = "L" Then ''FROM LOT
                str = " SELECT CURTAGNO AS LASTTAGNO FROM " & cnAdminDb & "..ITEMLOT WHERE ITEMID = "
                str += " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & itemName & "')"
                str += " AND SNO = '" & lotSno & "'"
                tagNo = GenTagNo(objGPack.GetSqlValue(str, , "1", tran), tran)
            End If
        Else 'UNIQUE
            str = " SELECT CTLTEXT AS LASTTAGNO FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'LASTTAGNO'"
            Dim lastTagNo As String = objGPack.GetSqlValue(str, , , tran)
            If _TagNoGen = "Y" Then 'YEAR BASED
                If Not lastTagNo.Length > 0 Then lastTagNo = Mid(cnTranToDate.Date.ToString("yyyy-MM-dd"), 3, 2).ToString & "A" & " "
                tagNo = BasedOnUniqueYear(lastTagNo)
            ElseIf _TagNoGen = "N" Then 'Numeric Based
                If TagPrefix <> "" Then
                    If lastTagNo.ToUpper.Contains(TagPrefix.ToUpper) Then
                        lastTagNo = Mid(lastTagNo, TagPrefix.Length + 1, lastTagNo.Length)
                    End If
                End If
                tagNo = BasedOnUniqueNumeric(lastTagNo)
                tagNo = TagPrefix & tagNo
            ElseIf _TagNoGen = "M" Then 'Month and Year Based Like MM\YY\A\number
                tagNo = BasedOnUniqueMonthYear(lastTagNo, RecDate)
            ElseIf _TagNoGen = "T" Then
                tagNo = BasedOnUniqueTime(cnCostId, RecDate, TagSystemPrefix, tran)
            ElseIf _TagNoGen = "D" Then
                tagNo = BasedonUniqueYearDayMonthHrsMinSec(cnCostId, RecDate, TagSystemPrefix, tran)
            ElseIf _TagNoGen = "W" Then
                'No need
            End If
        End If
        Return tagNo
    End Function
    Private Function RANDOM_TAGNO_UNIQUE(ByVal tagstring As String) As String
        Dim minval As UInteger
        Dim maxval As UInteger
        Dim tagarray() As String = tagstring.Split(",")
        'If Len(tagarray(2).ToString) > 9 Then Return dblRnd() : Exit Function
        minval = tagarray(1) : maxval = tagarray(2)
        Dim Tagnew As UInteger
        'Tagnew = Tag_random.NextDouble(minval, maxval)
        Tagnew = Tag_random.Next(minval, maxval)
        Return Tagnew
    End Function

    Private Function RANDOM_TAGNO_UNIQUE_NEW(ByVal tagstring As String) As String
        Dim numbers As String = "1234567890"
        Dim Tagnew As String
        Dim characters As String = numbers
        Dim tagarray() As String = tagstring.Split(",")
        Dim length As Integer = Integer.Parse(tagarray(1))
        Dim otp As String = String.Empty
        For i As Integer = 0 To length - 1
            Dim character As String = String.Empty
            Do
                Dim index As Integer = New Random().Next(0, characters.Length)
                character = characters.ToCharArray()(index).ToString()
            Loop While otp.IndexOf(character) <> -1
            otp += character
        Next
        Tagnew = otp
        Return Tagnew
    End Function

    Public Function dblRnd() As Double

        Const Power15 As Long = 2 ^ 15
        Const Power16 As Long = 2 ^ 16
        Const LongMax As Long = 2 ^ 31 - 1
        Dim lngValHi As Long
        Dim lngValLo As Long
        lngValHi = Int(Rnd() * Power15)
        lngValLo = Int(Rnd() * Power16)
        dblRnd = CDbl(lngValHi * Power16 Or lngValLo) / LongMax
    End Function
    Private Function BasedOnUniqueNumeric(ByVal tagNo As String) As String
        Dim retStr As String = Nothing
        Dim numericPart As Integer = Val(tagNo)
        numericPart += 1
        retStr = numericPart.ToString '  Format(numericPart, "00000000")
        Return retStr
    End Function

    Private Function BasedOnUniqueMonthYear(ByVal LastTagNo As String, ByVal RecDate As Date) As String
        Dim retStr As String = Nothing
        Dim MonthPart As String = Mid(LastTagNo, 1, 2)
        Dim YearPart As String = Mid(LastTagNo, 3, 2)
        Dim AlphaPart As Char = Mid(LastTagNo, 5, 1)
        Dim Limit As String
        Dim NumericPart As Integer
        If Char.IsLetter(AlphaPart) Then 'Alpha Contains
            'Limit = "999"
            NumericPart = Mid(LastTagNo, 6, 3)
        Else
            'Limit = "9999"
            'AlphaPart = ""
            NumericPart = Val(Mid(LastTagNo, 5, 4))
        End If
        If Val(MonthPart) <> Val(RecDate.Month) Then
            NumericPart = 0
            MonthPart = Format(RecDate.Month, "00")
            AlphaPart = ""
        End If
        If Val(YearPart) <> Val(Mid(RecDate.Year, 3, 2)) Then
            NumericPart = 0
            YearPart = Format(Val(Mid(RecDate.Year, 3, 2)), "00")
            AlphaPart = ""
        End If
        If Char.IsLetter(AlphaPart) Then 'Alpha Contains
            Limit = "999"
        Else
            Limit = "9999"
        End If
        If NumericPart < Val(Limit) Then
            NumericPart += 1
        Else
            NumericPart = 1
            Limit = 999
            If AlphaPart = Nothing Then
                AlphaPart = "A"
            ElseIf AlphaPart <> "Z" Then
                AlphaPart = Chr(Asc(AlphaPart) + 1)
            ElseIf Val(MonthPart) <> 12 Then
                AlphaPart = "A"
                MonthPart = Format(Val(MonthPart) + 1, "00")
            Else
                AlphaPart = "A"
                MonthPart = "01"
            End If
        End If
        Dim num As String = FormatStringCustom(NumericPart, "0", Limit.Length)
        retStr = MonthPart & YearPart
        If AlphaPart <> Nothing And (num.Length <= 3) Then retStr += AlphaPart
        retStr += num
        Return retStr
    End Function

    Private Function BasedOnUniqueYear(ByVal tagNo As String) As String
        Dim retStr As String = Nothing
        Dim yearPart As String = Mid(tagNo, 1, 2)
        Dim alphaPart As String = Mid(tagNo, 3, 1)
        Dim numericPart As Integer = Nothing
        If IsMainCostCentre Then
            numericPart = Val(Mid(tagNo, 4))
        Else
            numericPart = Val(Mid(tagNo, 6))
        End If
        If numericPart < 99999 Then
            numericPart += 1
        Else
            numericPart = 1
            If alphaPart <> "Z" Then
                alphaPart = Chr(Asc(alphaPart) + 1)
            Else
                alphaPart = "A"
                yearPart = Format(Val(yearPart) + 1, "00")
            End If
        End If
        'retStr = yearPart & alphaPart & Format(numericPart, "00000")
        If IsMainCostCentre Then
            retStr = yearPart & alphaPart & FormatStringCustom(numericPart, "0", 5)
        Else
            retStr = yearPart & alphaPart & cnCostId & FormatStringCustom(numericPart, "0", 3)
        End If
        Return retStr
    End Function
    Public Function BasedOnUniqueTime(ByVal _costId As String, ByVal RecDate As Date, ByVal systemPrefix As String, ByVal _tran As OleDbTransaction) As String
        Dim tagNew As String = ""
        Dim Str As String = ""
        Dim dr As DataRow = Nothing
        Dim day As String = AlphbetGenerated(RecDate.Day)
        Dim month As String = AlphbetGenerated(RecDate.Month)
        Dim year As String = AlphbetGenerated(Right(RecDate.Year, 2))
        Dim hrs As String = ""
        Dim min As String = ""
        Dim sec As String = ""
        Str = " SELECT DATEPART(HOUR,GETDATE()) HOURS ,DATEPART(MI,GETDATE()) MINUTES ,DATEPART(SECOND,GETDATE()) SECONDS "
        dr = GetSqlRow(Str, cn, _tran)
        If Not dr Is Nothing Then
            hrs = AlphbetGenerated(Val(dr.Item("HOURS").ToString))
            min = Format(dr.Item("MINUTES"), "00")
            sec = Format(dr.Item("SECONDS"), "00")
        End If
        tagNew = TagPrefix & year & month & day & systemPrefix & hrs & min & sec
        Return Trim(tagNew)
    End Function

    Public Function BasedOnSNOGenerator(ByVal _costId As String, ByVal RecDate As Date, ByVal systemPrefix As String) As String
        Dim tagNew As String = ""
        Dim Str As String = ""
        Dim dr As DataRow = Nothing
        Dim day As String = AlphbetGenerated(RecDate.Day)
        Dim month As String = AlphbetGenerated(RecDate.Month)
        Dim year As String = AlphbetGenerated(Right(RecDate.Year, 2))
        Dim hrs As String = ""
        Dim min As String = ""
        Dim sec As String = ""
        Str = " SELECT DATEPART(HOUR,GETDATE()) HOURS ,DATEPART(MI,GETDATE()) MINUTES ,DATEPART(SECOND,GETDATE()) SECONDS "
        dr = GetSqlRow(Str, cn, tran)
        If Not dr Is Nothing Then
            hrs = AlphbetGenerated(Val(dr.Item("HOURS").ToString))
            min = Format(dr.Item("MINUTES"), "00")
            sec = Format(dr.Item("SECONDS"), "00")
        End If
        'tagNew = IIf(COSTCENTRE_SINGLE = False, cnCostId, "00") & strCompanyId & year & month & day & systemPrefix & hrs & min & sec
        tagNew = IIf(cnCostId <> "", cnCostId, "00") & strCompanyId & year & month & day & systemPrefix & hrs & min & sec
        Return Trim(tagNew)
    End Function

    Private Function AlphbetGenerated(ByVal value As Integer) As String
        Select Case value
            Case 1 : Return "A"
            Case 2 : Return "B"
            Case 3 : Return "C"
            Case 4 : Return "D"
            Case 5 : Return "E"
            Case 6 : Return "F"
            Case 7 : Return "G"
            Case 8 : Return "H"
            Case 9 : Return "I"
            Case 10 : Return "J"
            Case 11 : Return "K"
            Case 12 : Return "L"
            Case 13 : Return "M"
            Case 14 : Return "N"
            Case 15 : Return "O"
            Case 16 : Return "P"
            Case 17 : Return "Q"
            Case 18 : Return "R"
            Case 19 : Return "S"
            Case 20 : Return "T"
            Case 21 : Return "U"
            Case 22 : Return "V"
            Case 23 : Return "W"
            Case 24 : Return "X"
            Case 25 : Return "Y"
            Case 26 : Return "Z"
            Case 27 : Return "5"
            Case 28 : Return "6"
            Case 29 : Return "7"
            Case 30 : Return "8"
            Case 31 : Return "9"
        End Select
    End Function

    Private Function BasedonUniqueItemIdWeightHrsMinSec(ByVal Itemid As String, ByVal Value As String, ByVal _tran As OleDbTransaction) As String
        Dim tagNew As String = ""
        Dim Str As String = ""
        Dim dr As DataRow = Nothing
        Dim hrs As String = ""
        Dim min As String = ""
        Dim sec As String = ""
        Str = " SELECT DATEPART(HOUR,GETDATE()) HOURS ,DATEPART(MI,GETDATE()) MINUTES ,DATEPART(SECOND,GETDATE()) SECONDS "
        dr = GetSqlRow(Str, cn, _tran)
        If Not dr Is Nothing Then
            hrs = Format(dr.Item("HOURS"), "00")
            min = Format(dr.Item("MINUTES"), "00")
            sec = Format(dr.Item("SECONDS"), "00")
        End If
        tagNew = TagPrefix & Itemid & Value.Replace(".", "") & hrs & min & sec
        Return Trim(tagNew)
    End Function

    Private Function BasedonUniqueYearDayMonthHrsMinSec(ByVal _costId As String, ByVal RecDate As Date, ByVal systemPrefix As String, ByVal _tran As OleDbTransaction) As String
        Dim tagNew As String = ""
        Dim Str As String = ""
        Dim dr As DataRow = Nothing
        Dim day As String = ""
        Dim month As String = ""
        Dim year As String = ""
        year = Right(RecDate.Year, 2)
        day = Format(RecDate.Day, "00")
        month = Format(RecDate.Month, "00")
        Dim hrs As String = ""
        Dim min As String = ""
        Dim sec As String = ""
        Str = " SELECT DATEPART(HOUR,GETDATE()) HOURS ,DATEPART(MI,GETDATE()) MINUTES ,DATEPART(SECOND,GETDATE()) SECONDS "
        dr = GetSqlRow(Str, cn, _tran)
        If Not dr Is Nothing Then
            hrs = Format(dr.Item("HOURS"), "00")
            min = Format(dr.Item("MINUTES"), "00")
            sec = Format(dr.Item("SECONDS"), "00")
        End If
        tagNew = TagPrefix & year & month & day & systemPrefix & hrs & min & sec
        Return Trim(tagNew)
    End Function

    Public Function GetTagVal(ByVal tagNo As String, Optional ByVal _Tran As OleDbTransaction = Nothing _
    , Optional ByVal recdate As Date = Nothing) As Long 'Integer to Long
        Dim retVal As String = Nothing
        Dim i As Integer = 0
        Dim ascStr As String = Nothing
        Dim secStr As String = Nothing
        Dim firstNum As String = Nothing
        Dim secondNum As String = Nothing
        If _TagNoGen = "T" Or _TagNoGen = "D" Then
            Dim Str As String = ""
            Dim dr As DataRow = Nothing
            Str = " SELECT DATEPART(HOUR,GETDATE()) [HOURS] ,DATEPART(MI,GETDATE()) [MINUTES] ,DATEPART(SECOND,GETDATE()) [SECONDS] "
            If Not _Tran Is Nothing Then
                dr = GetSqlRow(Str, cn, _Tran)
            Else
                dr = GetSqlRow(Str, cn, _Tran)
            End If
            retVal = Right(recdate.Year, 2) & Format(recdate.Month, "00") & Format(recdate.Day, "00") & Format(dr.Item("HOURS"), "00") & Format(dr.Item("MINUTES"), "00") & Format(dr.Item("SECONDS"), "00")
            Return retVal
        End If
        For Each c As Char In tagNo
            If Char.IsLetter(c) Then
                ascStr = c
                Exit For
            End If
            i += 1
        Next
        If i > 0 Then firstNum = Mid(tagNo, 1, i)
        For Each c As Char In Mid(tagNo, i + 2, tagNo.Length)
            If Char.IsNumber(c) Then
                secondNum += c
            End If
        Next
        secondNum = Val(secondNum).ToString
        If ascStr <> Nothing Then
            retVal = (Asc(ascStr) * 10000) + Val(firstNum + secondNum)
            ' Val(Mid(tagNo, 1, i) & Asc(ascStr) & Val(Mid(tagNo, i + 2, tagNo.Length)))
        Else
            retVal = Val(tagNo)
        End If
        Return retVal
    End Function

    Public Function GetTagNo(ByVal RecDate As Date, ByVal itemName As String, ByVal lotSno As String, Optional ByVal tran As OleDbTransaction = Nothing _
                             , Optional ByVal itemid As Integer = 0 _
                             , Optional ByVal ValueAmt As String = "") As String
        If _TagNoFrom = "" Or _TagNoGen = "" Then
            Return Nothing
        End If
        Dim tagNo As String = GenerateTagNo(RecDate, itemName, lotSno, tran)
TAGDUPCHECK:
        ''TAGNO
        If _TagNoFrom = "U" And _TagNoGen = "W" Then
            tagNo = "."
            If itemid > 0 Then
                tagNo = BasedonUniqueItemIdWeightHrsMinSec(itemid, ValueAmt, tran)
                'tagNo = "1177832141555"
                strSql = "SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAG"
                strSql += " WHERE TAGNO = '" & tagNo & "'"
                If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
                    tagNo = BasedonUniqueItemIdWeightHrsMinSec(itemid, ValueAmt, tran)
                    GoTo TAGDUPCHECK
                End If
            End If
            Return tagNo
        End If

        If Mid(RANDOM_UNIQUE_TAG, 1, 1) = "Y" Then
            strSql = "SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAG"
            strSql += " WHERE TAGNO = '" & tagNo & "'"
            If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
                tagNo = RANDOM_TAGNO_UNIQUE(RANDOM_UNIQUE_TAG)
                GoTo TAGDUPCHECK
            End If
            Return tagNo
        End If

        If Mid(RANDOM_UNIQUE_TAG_NEW, 1, 1) = "Y" Then
            strSql = "SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAG"
            strSql += " WHERE TAGNO = '" & tagNo & "'"
            If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
                tagNo = IIf(TagPrefix.ToString <> "", TagPrefix, "") & RANDOM_TAGNO_UNIQUE_NEW(RANDOM_UNIQUE_TAG_NEW)
                GoTo TAGDUPCHECK
            End If
            Return tagNo
        End If

        If _TagNoFrom = "I" Then
            strSql = "SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAG"
            strSql += " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & itemName & "') "
            strSql += " AND TAGNO = '" & tagNo & "'"
            If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
                If TagPrefix <> "" And TagPrefix_Item Then
                    If tagNo.ToUpper.Contains(TagPrefix.ToUpper) Then
                        tagNo = Mid(tagNo, TagPrefix.Length + 1, tagNo.Length)
                    End If
                End If
                tagNo = GenTagNo(tagNo, tran)
                GoTo TAGDUPCHECK
            End If
        ElseIf _TagNoFrom = "U" Then
            strSql = "SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAG"
            strSql += " WHERE TAGNO = '" & tagNo & "'"
            If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
                If _TagNoGen = "Y" Then
                    tagNo = BasedOnUniqueYear(tagNo)
                ElseIf _TagNoGen = "N" Then
                    If TagPrefix <> "" Then
                        If tagNo.ToUpper.Contains(TagPrefix.ToUpper) Then
                            tagNo = Mid(tagNo, TagPrefix.Length + 1, tagNo.Length)
                        End If
                    End If
                    tagNo = BasedOnUniqueNumeric(tagNo)
                    tagNo = TagPrefix & tagNo
                ElseIf _TagNoGen = "M" Then
                    tagNo = BasedOnUniqueMonthYear(tagNo, RecDate)
                ElseIf _TagNoGen = "T" Then
                    tagNo = BasedOnUniqueTime(cnCostId, RecDate, TagSystemPrefix, tran)
                ElseIf _TagNoGen = "D" Then
                    tagNo = BasedonUniqueYearDayMonthHrsMinSec(cnCostId, RecDate, TagSystemPrefix, tran)
                End If
                GoTo TAGDUPCHECK
            End If
        End If
        Return tagNo
    End Function
    Public Function GetWTagNo(ByVal RecDate As Date, ByVal itemName As String, ByVal lotSno As String, Optional ByVal tran As OleDbTransaction = Nothing) As String
        If _TagNoFrom = "" Or _TagNoGen = "" Then
            Return Nothing
        End If
        Dim tagNo As String = GenerateTagNo(RecDate, itemName, lotSno, tran)
TAGDUPCHECK:
        ''TAGNO
        If Mid(RANDOM_UNIQUE_TAG, 1, 1) = "Y" Then
            strSql = "SELECT TAGNO FROM " & cnAdminDb & "..WITEMTAG"
            strSql += " WHERE TAGNO = '" & tagNo & "'"
            If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
                tagNo = RANDOM_TAGNO_UNIQUE(RANDOM_UNIQUE_TAG)
                GoTo TAGDUPCHECK
            End If
            Return tagNo
        End If
        If Mid(RANDOM_UNIQUE_TAG_NEW, 1, 1) = "Y" Then
            strSql = "SELECT TAGNO FROM " & cnAdminDb & "..WITEMTAG"
            strSql += " WHERE TAGNO = '" & tagNo & "'"
            If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
                tagNo = IIf(TagPrefix.ToString <> "", TagPrefix, "") & RANDOM_TAGNO_UNIQUE_NEW(RANDOM_UNIQUE_TAG_NEW)
                GoTo TAGDUPCHECK
            End If
            Return tagNo
        End If
        If _TagNoFrom = "I" Then
            strSql = "SELECT TAGNO FROM " & cnAdminDb & "..WITEMTAG"
            strSql += " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & itemName & "') "
            strSql += " AND TAGNO = '" & tagNo & "'"
            If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
                tagNo = GenTagNo(tagNo, tran)
                GoTo TAGDUPCHECK
            End If
        ElseIf _TagNoFrom = "U" Then
            strSql = "SELECT TAGNO FROM " & cnAdminDb & "..WITEMTAG"
            strSql += " WHERE TAGNO = '" & tagNo & "'"
            If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
                If _TagNoGen = "Y" Then
                    tagNo = BasedOnUniqueYear(tagNo)
                ElseIf _TagNoGen = "N" Then
                    If TagPrefix <> "" Then
                        If tagNo.ToUpper.Contains(TagPrefix.ToUpper) Then
                            tagNo = Mid(tagNo, TagPrefix.Length + 1, tagNo.Length)
                        End If
                    End If
                    tagNo = BasedOnUniqueNumeric(tagNo)
                    tagNo = TagPrefix & tagNo
                ElseIf _TagNoGen = "M" Then
                    tagNo = BasedOnUniqueMonthYear(tagNo, RecDate)
                End If
                GoTo TAGDUPCHECK
            End If
        End If
        Return tagNo
    End Function
End Class
