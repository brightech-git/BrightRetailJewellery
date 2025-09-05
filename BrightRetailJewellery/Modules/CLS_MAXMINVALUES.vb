Imports System.Data.OleDb
Public Class CLS_MAXMINVALUES
    Dim StrSql As String
    Private WastagePer As Decimal = 0
    Public Property pWastagePer() As Decimal
        Get
            Return WastagePer
        End Get
        Set(ByVal value As Decimal)
            WastagePer = value
        End Set
    End Property

    Private Wastage As Decimal = 0
    Public Property pWastage() As Decimal
        Get
            Return Wastage
        End Get
        Set(ByVal value As Decimal)
            Wastage = value
        End Set
    End Property

    Private McGrm As Decimal = 0
    Public Property pMcGrm() As Decimal
        Get
            Return McGrm
        End Get
        Set(ByVal value As Decimal)
            McGrm = value
        End Set
    End Property

    Private Mc As Decimal = 0
    Public Property pMc() As Decimal
        Get
            Return Mc
        End Get
        Set(ByVal value As Decimal)
            Mc = value
        End Set
    End Property

    Private WastagePer_Min As Decimal = 0
    Public Property pWastagePer_Min() As Decimal
        Get
            Return WastagePer_Min
        End Get
        Set(ByVal value As Decimal)
            WastagePer_Min = value
        End Set
    End Property

    Private Wastage_Min As Decimal = 0
    Public Property pWastage_Min() As Decimal
        Get
            Return Wastage_Min
        End Get
        Set(ByVal value As Decimal)
            Wastage_Min = value
        End Set
    End Property

    Private McGrm_Min As Decimal = 0
    Public Property pMcGrm_Min() As Decimal
        Get
            Return McGrm_Min
        End Get
        Set(ByVal value As Decimal)
            McGrm_Min = value
        End Set
    End Property

    Private Mc_Min As Decimal = 0
    Public Property pMc_Min() As Decimal
        Get
            Return Mc_Min
        End Get
        Set(ByVal value As Decimal)
            Mc_Min = value
        End Set
    End Property

    Private DiscountPer As Decimal = 0
    Public Property pDiscountPer() As Decimal
        Get
            Return DiscountPer
        End Get
        Set(ByVal value As Decimal)
            DiscountPer = value
        End Set
    End Property

    Private Touch As Decimal = 0
    Public Property pTouch() As Decimal
        Get
            Return Touch
        End Get
        Set(ByVal value As Decimal)
            Touch = value
        End Set
    End Property

    Private Pur_WastagePer As Decimal = 0
    Public Property pPur_WastagePer() As Decimal
        Get
            Return Pur_WastagePer
        End Get
        Set(ByVal value As Decimal)
            Pur_WastagePer = value
        End Set
    End Property

    Private Pur_Wastage As Decimal = 0
    Public Property pPur_Wastage() As Decimal
        Get
            Return Pur_Wastage
        End Get
        Set(ByVal value As Decimal)
            Pur_Wastage = value
        End Set
    End Property

    Private Pur_McGrm As Decimal = 0
    Public Property pPur_McGrm() As Decimal
        Get
            Return Pur_McGrm
        End Get
        Set(ByVal value As Decimal)
            Pur_McGrm = value
        End Set
    End Property

    Private Pur_Mc As Decimal = 0
    Public Property pPur_Mc() As Decimal
        Get
            Return Pur_Mc
        End Get
        Set(ByVal value As Decimal)
            Pur_Mc = value
        End Set
    End Property

    Private Pur_WastagePer_Min As Decimal = 0
    Public Property pPur_WastagePer_Min() As Decimal
        Get
            Return Pur_WastagePer_Min
        End Get
        Set(ByVal value As Decimal)
            Pur_WastagePer_Min = value
        End Set
    End Property

    Private Pur_Wastage_Min As Decimal = 0
    Public Property pPur_Wastage_Min() As Decimal
        Get
            Return Pur_Wastage_Min
        End Get
        Set(ByVal value As Decimal)
            Pur_Wastage_Min = value
        End Set
    End Property

    Private Pur_McGrm_Min As Decimal = 0
    Public Property pPur_McGrm_Min() As Decimal
        Get
            Return Pur_McGrm_Min
        End Get
        Set(ByVal value As Decimal)
            Pur_McGrm_Min = value
        End Set
    End Property

    Private Pur_Mc_Min As Decimal = 0
    Public Property pPur_Mc_Min() As Decimal
        Get
            Return Pur_Mc_Min
        End Get
        Set(ByVal value As Decimal)
            Pur_Mc_Min = value
        End Set
    End Property

    Private Pur_DiscountPer As Decimal = 0
    Public Property pPur_DiscountPer() As Decimal
        Get
            Return Pur_DiscountPer
        End Get
        Set(ByVal value As Decimal)
            Pur_DiscountPer = value
        End Set
    End Property

    Private Pur_Touch As Decimal = 0
    Public Property pPur_Touch() As Decimal
        Get
            Return Pur_Touch
        End Get
        Set(ByVal value As Decimal)
            Pur_Touch = value
        End Set
    End Property

    Private DtWmc As New DataTable
    Public Property pDtWmc() As DataTable
        Get
            Return DtWmc
        End Get
        Set(ByVal value As DataTable)
            DtWmc = value
        End Set
    End Property

    Public Sub New(ByVal ValueAddedType As String, _
    ByVal Weight As Decimal, _
    ByVal ItemId As Integer, ByVal SubItemId As Integer, _
    ByVal DesignerId As Integer, ByVal ItemTypeId As Integer, ByVal TableCode As String, _
    ByVal Accode As String, ByVal CostId As String)

        Select Case ValueAddedType
            Case "T"
                'StrSql = " DECLARE @WT FLOAT"
                'StrSql += " SET @WT = " & Weight & ""
                StrSql = vbCrLf + " SELECT *"
                StrSql += vbCrLf + " FROM " & cnAdminDb & "..WMCTABLE "
                StrSql += vbCrLf + " WHERE TABLECODE = '" & TableCode & "'"
                StrSql += vbCrLf + " AND TABLECODE <> ''"
                StrSql += vbCrLf + " AND " & Weight & " BETWEEN FROMWEIGHT AND TOWEIGHT"
                If _IsWholeSaleType Then
                    If Accode <> "" Then StrSql += vbCrLf + " AND ISNULL(ACCODE,'') = '" & Accode & "'"
                Else
                    StrSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
                End If
                If CostId <> "" Then StrSql += " AND ISNULL(COSTID,'') = '" & CostId & "'"
            Case "D"
                'StrSql = " DECLARE @WT FLOAT"
                'StrSql += " SET @WT = " & Weight & ""
                StrSql = vbCrLf + " SELECT *"
                StrSql += vbCrLf + " FROM " & cnAdminDb & "..WMCTABLE "
                StrSql += vbCrLf + " WHERE ITEMID = '" & ItemId & "'"
                StrSql += vbCrLf + " and (SUBITEMID = " & SubItemId & " OR SUBITEMID = 0)"
                StrSql += vbCrLf + " and (DESIGNERID= " & DesignerId & ")"
                StrSql += vbCrLf + " AND " & Weight & " BETWEEN FROMWEIGHT AND TOWEIGHT"
                If _IsWholeSaleType Then
                    If Accode <> "" Then StrSql += vbCrLf + " AND ISNULL(ACCODE,'') = '" & Accode & "'"
                Else
                    StrSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
                End If
                StrSql += vbCrLf + " AND ISNULL(TABLECODE,'') = ''"
                If CostId <> "" Then StrSql += " AND ISNULL(COSTID,'') = '" & CostId & "'"
                StrSql += vbCrLf + " ORDER BY ITEMID DESC,SUBITEMID DESC,ITEMTYPE DESC"
            Case "P"
                'StrSql = " DECLARE @WT FLOAT"
                'StrSql += " SET @WT = " & Weight & ""
                StrSql = vbCrLf + " SELECT *"
                StrSql += vbCrLf + " FROM " & cnAdminDb & "..WMCTABLE "
                StrSql += vbCrLf + " WHERE ITEMTYPE = " & ItemTypeId & ""
                StrSql += vbCrLf + " AND " & Weight & " BETWEEN FROMWEIGHT AND TOWEIGHT"
                If _IsWholeSaleType Then
                    If Accode <> "" Then StrSql += vbCrLf + " AND ISNULL(ACCODE,'') = '" & Accode & "'" Else StrSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
                Else
                    StrSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
                End If
                If CostId <> "" Then StrSql += " AND ISNULL(COSTID,'') = '" & CostId & "'"
            Case Else
                'StrSql = " DECLARE @WT FLOAT"
                'StrSql += " SET @WT = " & Weight & ""
                StrSql = vbCrLf + " SELECT *"
                StrSql += vbCrLf + " FROM " & cnAdminDb & "..WMCTABLE "
                StrSql += vbCrLf + " WHERE (ITEMID = " & ItemId & " OR ITEMID = 0)"
                StrSql += vbCrLf + " and (SUBITEMID = " & SubItemId & " OR SUBITEMID = 0)"
                StrSql += vbCrLf + " and (DESIGNERID= " & DesignerId & " OR DESIGNERID = 0)"
                StrSql += vbCrLf + " and (ITEMTYPE= " & ItemTypeId & " OR ITEMTYPE = 0)"
                StrSql += vbCrLf + " AND " & Weight & " BETWEEN FROMWEIGHT AND TOWEIGHT"
                StrSql += vbCrLf + " AND ISNULL(TABLECODE,'') = ''"
                If CostId <> "" Then StrSql += " AND ISNULL(COSTID,'') = '" & CostId & "'"
                If Accode <> "" Then StrSql += vbCrLf + " AND ISNULL(ACCODE,'') = '" & Accode & "'"
                StrSql += vbCrLf + " ORDER BY ITEMID DESC,SUBITEMID DESC,ITEMTYPE DESC"
        End Select
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(DtWmc)
        If DtWmc.Rows.Count > 0 Then
            With DtWmc.Rows(0)
                WastagePer = Val(.Item("MAXWASTPER").ToString)
                Wastage = Val(.Item("MAXWAST").ToString)
                McGrm = Val(.Item("MAXMCGRM").ToString)
                Mc = Val(.Item("MAXMC").ToString)
                DiscountPer = Val(.Item("DISCOUNTPER").ToString)
                Touch = Val(.Item("TOUCH").ToString)
                WastagePer_Min = Val(.Item("MINWASTPER").ToString)
                Wastage_Min = Val(.Item("MINWAST").ToString)
                McGrm_Min = Val(.Item("MINMCGRM").ToString)
                Mc_Min = Val(.Item("MINMC").ToString)

                Pur_WastagePer = Val(.Item("MAXWASTPER_PUR").ToString)
                Pur_Wastage = Val(.Item("MAXWAST_PUR").ToString)
                Pur_McGrm = Val(.Item("MAXMCGRM_PUR").ToString)
                Pur_Mc = Val(.Item("MAXMC_PUR").ToString)
                Pur_DiscountPer = Val(.Item("DISCOUNTPER_PUR").ToString)
                Pur_Touch = Val(.Item("TOUCH_PUR").ToString)
                Pur_WastagePer_Min = Val(.Item("MINWASTPER_PUR").ToString)
                Pur_Wastage_Min = Val(.Item("MINWAST_PUR").ToString)
                Pur_McGrm_Min = Val(.Item("MINMCGRM_PUR").ToString)
                Pur_Mc_Min = Val(.Item("MINMC_PUR").ToString)
            End With
        End If
    End Sub
    Public Sub New(ByVal ValueAddedType As String, _
    ByVal Weight As Decimal, _
    ByVal ItemId As Integer, ByVal SubItemId As Integer, _
    ByVal CostId As String, Optional ByVal TableCode As String = "" _
    , Optional ByVal Designerid As Integer = 0, Optional ByVal ItemTypeId As Integer = 0)

        StrSql = vbCrLf + " SELECT *"
        StrSql += vbCrLf + " FROM " & cnAdminDb & "..BRANCHVA "
        StrSql += vbCrLf + " WHERE (ITEMID = " & ItemId & " OR ITEMID = 0)"
        StrSql += vbCrLf + " AND (SUBITEMID = " & SubItemId & " OR SUBITEMID = 0)"
        StrSql += vbCrLf + " AND " & Weight & " BETWEEN FROMWEIGHT AND TOWEIGHT"
        If CostId <> "" Then StrSql += " AND ISNULL(COSTID,'') = '" & CostId & "'"
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(DtWmc)
        If DtWmc.Rows.Count > 0 Then
            With DtWmc.Rows(0)
                WastagePer = Val(.Item("MAXWASTPER").ToString)
                McGrm = Val(.Item("MAXMCGRM").ToString)
            End With
        End If
    End Sub
End Class
