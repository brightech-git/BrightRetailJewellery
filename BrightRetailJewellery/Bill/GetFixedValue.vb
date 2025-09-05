Public Class GetFixedValue
    Public GrossAmt As Double
    Public Vat As Double
    Public NetAmt As Double
    Public Sub CalcGrossNetAmount(ByVal itemId As Integer, ByVal subItemId As Integer, ByVal tagNo As String _
    , ByVal B4Discount As Double, ByVal RoundOff_Gross As String _
    , ByVal VatPer As Double, ByVal RoundOff_Vat As String, ByVal OrdAdvanceWeight As Double)
        Dim tgAmt As Double = CalcRoundoffAmt(Val(objGPack.GetSqlValue("SELECT SALVALUE FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID = " & itemId & " AND TAGNO = '" & tagNo & "'")) _
- Val(B4Discount), RoundOff_Gross)

        If Val(OrdAdvanceWeight) > 0 Then Exit Sub
        Dim taxInclusive As String = ""
        If subItemId > 0 Then
            taxInclusive = UCase(objGPack.GetSqlValue("SELECT TAXINCLUCIVE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = " & subItemId & " AND ITEMID = " & itemId & ""))
        Else
            taxInclusive = UCase(objGPack.GetSqlValue("SELECT TAXINCLUCIVE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & itemId & ""))
        End If
        Dim vat As Double = Nothing
        Dim sur As Double = Nothing
        vat = Format(tgAmt * (VatPer / 100), "0.00")
        vat = CalcRoundoffAmt(vat, RoundOff_Vat)
        Dim txAmt As Double = vat
        If taxInclusive = "N" Then
            '    txtSAGrossAmount_AMT.Text = IIf(tgAmt <> 0, Format(tgAmt, "0.00"), Nothing)
            '    txtSAVat_AMT.Text = IIf(txAmt <> 0, Format(txAmt, "0.00"), Nothing)
            'Else
            '    txtSAGrossAmount_AMT.Text = IIf(tgAmt - txAmt <> 0, Format(tgAmt - txAmt, "0.00"), Nothing)
            '    txtSAVat_AMT.Text = IIf(txAmt <> 0, Format(txAmt, "0.00"), Nothing)
        End If
    End Sub
End Class