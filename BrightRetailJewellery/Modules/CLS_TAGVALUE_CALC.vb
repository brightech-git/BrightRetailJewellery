Imports System.Data.OleDb
Public Class CLS_TAGVALUE_CALC
    Private TagTblPrefix As String
    Private TagSno As String
    Private TranDbName As String
    Private AdminDbName As String
    Private Cn As OleDbConnection
    Private Cmd As OleDbCommand
    Private Tran As OleDbTransaction
    Private Da As OleDbDataAdapter
    Private CostId As String
    Private RoundWastage As Integer = 3
    Private RouncMc As Integer = 2
    Private McWithWastage As Boolean = False
    Private McOnGrsNet As Boolean = True
    Private McCalcOn_Item_Grs As Boolean = False
    Private MultiMetalCalc As Boolean = False
    Private StrSql As String = ""
    Private DtTemp As New DataTable
    Public WmcValueUpdated As Boolean
    Public RowTagInfo As DataRow = Nothing

    Private Function GetSqlValue(ByVal Qry As String, Optional ByVal DefValue As String = "") As String
        Dim DtTemp As New DataTable
        Cmd = New OleDbCommand(StrSql, Cn, Tran)
        Da = New OleDbDataAdapter(Cmd)
        DtTemp = New DataTable
        Da.Fill(DtTemp)
        If DtTemp.Rows.Count > 0 Then
            DefValue = DtTemp.Rows(0).Item(0).ToString
        End If
        Return DefValue
    End Function

    Public Sub New(ByVal CostId As String, ByVal TagSno As String, ByVal TagTblPrefix As String _
    , ByVal TranDbName As String, ByVal AdminDbName As String _
    , ByVal Cn As OleDbConnection, ByVal Tran As OleDbTransaction _
    )
        Me.CostId = CostId
        Me.TagSno = TagSno
        Me.TagTblPrefix = TagTblPrefix
        Me.Cn = Cn
        Me.Tran = Tran
        Me.TranDbName = TranDbName
        Me.AdminDbName = AdminDbName
        StrSql = " SELECT * FROM " & AdminDbName & ".." & TagTblPrefix & "ITEMTAG"
        StrSql += " WHERE SNO = '" & TagSno & "'"
        Cmd = New OleDbCommand(StrSql, Cn, Tran)
        Da = New OleDbDataAdapter(Cmd)
        DtTemp = New DataTable
        Da.Fill(DtTemp)
        If Not DtTemp.Rows.Count > 0 Then
            Exit Sub
        End If
        RowTagInfo = DtTemp.Rows(0)

        StrSql = " SELECT CTLTEXT FROM " & AdminDbName & "..SOFTCONTROL WHERE CTLID = 'ROUNDOFF-WASTAGE' AND CTLTEXT <> ''"
        RoundWastage = Val(GetSqlValue(StrSql, "3"))

        StrSql = " SELECT CTLTEXT FROM " & AdminDbName & "..SOFTCONTROL WHERE CTLID = 'ROUNDOFF-MC' AND CTLTEXT <> ''"
        RouncMc = Val(GetSqlValue(StrSql, "2"))

        StrSql = " SELECT CTLTEXT FROM " & AdminDbName & "..SOFTCONTROL WHERE CTLID = 'MCWITHWASTAGE' AND CTLTEXT = 'Y'"
        McWithWastage = IIf(GetSqlValue(StrSql, "N") = "N", False, True)

        StrSql = " SELECT CTLTEXT FROM " & AdminDbName & "..SOFTCONTROL WHERE CTLID = 'MC_ON_GRSNET' AND CTLTEXT = 'N'"
        McOnGrsNet = IIf(GetSqlValue(StrSql, "Y") = "Y", True, False)

        StrSql = "SELECT CTLTEXT FROM " & AdminDbName & "..SOFTCONTROL WHERE CTLID = 'MULTIMETALCALC' AND CTLTEXT = 'Y'"
        McCalcOn_Item_Grs = IIf(GetSqlValue(StrSql, "N") = "N", False, True)

        ''Regenerate Wmc Values
        WmcValueUpdated = CalcWmcValues()
        If WmcValueUpdated = False Then Exit Sub

        StrSql = " SELECT SUM(STNAMT) FROM " & AdminDbName & ".." & TagTblPrefix & "ITEMTAGSTONE WHERE TAGSNO = '" & TagSno & "'"
        Dim StudAmt As Decimal = Val(GetSqlValue(StrSql))

        StrSql = " SELECT SUM(AMOUNT) FROM " & AdminDbName & ".." & TagTblPrefix & "ITEMTAGMISCCHAR WHERE TAGSNO = '" & TagSno & "'"
        Dim MiscAmt As Decimal = Val(GetSqlValue(StrSql))

        StrSql = " SELECT SUM(AMOUNT) FROM " & AdminDbName & ".." & TagTblPrefix & "ITEMTAGMETAL WHERE TAGSNO = '" & TagSno & "'"
        Dim MultiAmt As Decimal = Nothing


        Dim IsMultiMetal As Boolean = False
        StrSql = " SELECT MULTIMETAL FROM " & AdminDbName & "..ITEMMAST WHERE ITEMID = " & Val(RowTagInfo.Item("ITEMID").ToString) & ""
        IsMultiMetal = IIf(GetSqlValue(StrSql, "N") = "Y", True, False)

        Dim SaleValue As Decimal = Nothing
        Select Case RowTagInfo.Item("SALEMODE").ToString.ToUpper
            Case "F"
                SaleValue = RowTagInfo.Item("SALVALUE").ToString
            Case "R"
                SaleValue = Val(RowTagInfo.Item("PCS").ToString) * Val(RowTagInfo.Item("RATE").ToString)
            Case Else
                Dim Weight As Decimal = Nothing
                Dim Rate As Decimal = IIf(RowTagInfo.Item("SALEMODE").ToString.ToUpper = "M", Val(RowTagInfo.Item("RATE").ToString), Val(RowTagInfo.Item("BOARDRATE").ToString))
                If RowTagInfo.Item("GRSNET").ToString = "G" Then
                    Weight = Val(RowTagInfo.Item("GRSWT").ToString)
                Else ''NET WT
                    Weight = Val(RowTagInfo.Item("NETWT").ToString)
                End If
                If Val(RowTagInfo.Item("TOUCH").ToString) > 0 And Val(RowTagInfo.Item("MAXWAST").ToString) = 0 Then
                    Weight = (Weight * Val(RowTagInfo.Item("TOUCH").ToString)) / 100
                End If

                If IsMultiMetal And MultiMetalCalc Then
                    ''Multimetal Calc
                    SaleValue = MultiAmt + StudAmt + MiscAmt
                Else
                    SaleValue = ((Weight + Val(RowTagInfo.Item("MAXWAST").ToString)) * Rate) _
                    + Val(RowTagInfo.Item("MAXMC").ToString) + MultiAmt + StudAmt + MiscAmt
                End If
                RowTagInfo.Item("SALVALUE") = Math.Round(SaleValue)
        End Select
    End Sub
    Private Function CalcWmcValues() As Boolean
        Dim wt As Double = Nothing
        If RowTagInfo.Item("GRSNET").ToString = "G" Then
            wt = Val(RowTagInfo.Item("GRSWT").ToString)
        Else ''NET WT
            wt = Val(RowTagInfo.Item("NETWT").ToString)
        End If
        If RowTagInfo.Item("VALUEADDEDTYPE").ToString.ToUpper = "" Then
            Return False
        End If
        Select Case RowTagInfo.Item("VALUEADDEDTYPE").ToString.ToUpper
            Case "T"
                StrSql = " DECLARE @WT FLOAT"
                StrSql += vbCrLf + " SET @WT = " & wt & ""
                StrSql += vbCrLf + " SELECT TOUCH,MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC"
                StrSql += vbCrLf + " ,MINWASTPER,MINMCGRM,MINWAST,MINMC FROM " & AdminDbName & "..WMCTABLE "
                StrSql += vbCrLf + " WHERE TABLECODE = '" & RowTagInfo.Item("TABLECODE").ToString & "'"
                StrSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
                StrSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
                StrSql += vbCrLf + " AND ISNULL(COSTID,'') = '" & CostId & "'"
            Case "I"
                StrSql = " DECLARE @WT FLOAT"
                StrSql += vbCrLf + " SET @WT = " & wt & ""
                StrSql += vbCrLf + " SELECT TOUCH,MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC"
                StrSql += vbCrLf + " ,MINWASTPER,MINMCGRM,MINWAST,MINMC FROM " & AdminDbName & "..WMCTABLE "
                StrSql += vbCrLf + " WHERE ITEMID = " & Val(RowTagInfo.Item("ITEMID").ToString)
                StrSql += vbCrLf + " AND SUBITEMID = " & Val(RowTagInfo.Item("SUBITEMID").ToString)
                StrSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
                StrSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
                StrSql += vbCrLf + " AND ISNULL(TABLECODE,'') = ''"
                StrSql += vbCrLf + " AND ISNULL(COSTID,'') = '" & CostId & "'"
            Case "D"
                StrSql = " DECLARE @WT FLOAT"
                StrSql += vbCrLf + " SET @WT = " & wt & ""
                StrSql += vbCrLf + " SELECT TOUCH,MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC"
                StrSql += vbCrLf + " ,MINWASTPER,MINMCGRM,MINWAST,MINMC FROM " & AdminDbName & "..WMCTABLE "
                StrSql += vbCrLf + " WHERE ITEMID = " & Val(RowTagInfo.Item("ITEMID").ToString)
                StrSql += vbCrLf + " AND SUBITEMID = " & Val(RowTagInfo.Item("SUBITEMID").ToString)
                StrSql += vbCrLf + " AND DESIGNERID = " & Val(RowTagInfo.Item("DESIGNERID").ToString)
                StrSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
                StrSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
                StrSql += vbCrLf + " AND ISNULL(TABLECODE,'') = ''"
                StrSql += vbCrLf + " AND ISNULL(COSTID,'') = '" & CostId & "'"
            Case "P"
                StrSql = " DECLARE @WT FLOAT"
                StrSql += vbCrLf + " SET @WT = " & wt & ""
                StrSql += vbCrLf + " SELECT TOUCH,MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC"
                StrSql += vbCrLf + " ,MINWASTPER,MINMCGRM,MINWAST,MINMC FROM " & AdminDbName & "..WMCTABLE "
                StrSql += vbCrLf + " WHERE ITEMTYPE = " & Val(RowTagInfo.Item("ITEMTYPEID").ToString)
                StrSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
                StrSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
                StrSql += vbCrLf + " AND ISNULL(COSTID,'') = '" & CostId & "'"
            Case Else
                Return False
        End Select
        Dim DtWmc As New DataTable
        Cmd = New OleDbCommand(StrSql, Me.Cn, Me.Tran)
        Da = New OleDbDataAdapter(Cmd)
        Da.Fill(DtWmc)
        If Not DtWmc.Rows.Count > 0 Then
            Return False
        Else
            If Val(RowTagInfo.Item("SUBITEMID").ToString) = 0 Then
                StrSql = " SELECT ISNULL(MCCALC,'N') FROM " & AdminDbName & "..ITEMMAST WHERE ITEMID = " & Val(RowTagInfo.Item("ITEMID").ToString)
            Else
                StrSql = " SELECT ISNULL(MCCALC,'N') FROM " & AdminDbName & "..SUBITEMMAST WHERE ITEMID = " & Val(RowTagInfo.Item("SUBITEMID").ToString) & " AND SUBITEMID = " & Val(RowTagInfo.Item("SUBITEMID").ToString)
            End If
            McCalcOn_Item_Grs = IIf(GetSqlValue(StrSql, "G") = "G", True, False)
            With DtWmc.Rows(0)
                RowTagInfo("MAXWASTPER") = Val(.Item("MAXWASTPER").ToString)
                RowTagInfo("MAXWAST") = Val(.Item("MAXWAST").ToString)
                RowTagInfo("MAXMCGRM") = Val(.Item("MAXMCGRM").ToString)
                RowTagInfo("MAXMC") = Val(.Item("MAXMC").ToString)
                RowTagInfo("MINWASTPER") = Val(.Item("MINWASTPER").ToString)
                RowTagInfo("MINWAST") = Val(.Item("MINWAST").ToString)
                RowTagInfo("MINMCGRM") = Val(.Item("MINMCGRM").ToString)
                RowTagInfo("MINMC") = Val(.Item("MINMC").ToString)
                RowTagInfo("TOUCH") = Val(.Item("TOUCH").ToString)

                If Val(.Item("MAXWASTPER").ToString) <> 0 Then
                    RowTagInfo("MAXWASTPER") = Val(.Item("MAXWASTPER").ToString)
                    RowTagInfo("MAXWAST") = Math.Round((wt * Val(.Item("MAXWASTPER").ToString) / 100), RoundWastage)
                End If
                If Val(.Item("MAXMCGRM").ToString) <> 0 Then
                    Dim Mc As Decimal = 0
                    Dim was As Decimal = IIf(McWithWastage, Val(.Item("MAXWAST").ToString), 0)
                    If McOnGrsNet Then
                        Mc = (wt + was) * Val(.Item("MAXMCGRM").ToString)
                    Else
                        Mc = (IIf(McCalcOn_Item_Grs, Val(RowTagInfo.Item("GRSWT").ToString), Val(RowTagInfo.Item("NETWT").ToString)) + was) * Val(.Item("MAXMCGRM").ToString)
                    End If
                    RowTagInfo("MAXMCGRM") = Val(.Item("MAXMCGRM").ToString)
                    RowTagInfo("MAXMC") = Mc
                End If
            End With
            Return True
        End If

    End Function
End Class
