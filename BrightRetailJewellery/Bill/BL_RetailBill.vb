Imports System.Data.OleDb
Public Class BL_RetailBill
    Dim dt As New DataTable
    Dim cmd As OleDbCommand
    Dim strsql As String
    Dim HasSimilarBillNo As Boolean = False
    Public objSoftKeys As New SoftKeys

    Public Enum TranType
        Sales = 0
        CreditSales = 1
        SalesReturn = 2
        Purchase = 3
        ApprovalIssue = 4
        ApprovalReceipt = 5
        MiscIssue = 6
        GiftVoucher = 7
        Receipt = 8
        OtherReceipt = 9
        Payment = 10
        OtherPayment = 11
        Order = 12
        Repair = 13
        Customer_Advance = 14
        OrderRepayment = 15
    End Enum

    Public Sub funccalcPreviledge(ByVal priv_card As String, ByVal _tran As OleDbTransaction, ByVal _dtgrid As DataTable _
                               , ByVal Save_Previlige As Boolean _
                               , ByVal _BillDate As Date _
                               , ByVal _BillCostId As String _
                               , ByVal _BatchNo As String _
                               , ByVal _TranTypeCol As List(Of String), ByVal _TRANNO As String _
                               , ByVal previlId As String, ByVal redeemprevilId As String _
                               , ByVal redeemvalue As Double, ByVal redeempoint As Double, ByVal redeemAmount As Double)
        Dim pt_Points As Double = 0
        Dim pt_Value As Double = 0
        If priv_card = "Y" Then
            For pt As Integer = 0 To _dtgrid.Rows.Count - 1
                If _dtgrid.Rows(pt)("ENTFLAG").ToString = "" Then Exit For
                If Not (_dtgrid.Rows(pt)("TRANTYPE").ToString = "SA" Or _dtgrid.Rows(pt)("TRANTYPE").ToString = "SR") Then Continue For
                pt_Value = 0 '''''''''''''''''''''''''''''''''''''2020-02-22 For JMT SA and SR Seperate Record Insert
                pt_Points = 0
                Dim pr_Points As Double = 0
                Dim pr_Value As Double = 0
                Dim pr_Based As String = "A"
                Dim pr_FixedRange As String = "N"
                Dim pr_ValueIn As String = "A"
                strsql = vbCrLf + " SELECT PV.WEIGHTORAMOUNT,PV.POINTS ,PV.FIXEDRANGE,ISNULL(PV.VALUEINWTRAMT,'A')VALUEINWTRAMT "
                strsql += vbCrLf + "  FROM " & cnAdminDb & "..PRIVILIGE PV WHERE 1=1 "
                'strsql += vbCrLf + "  AND PV.COMPANYID='" & strCompanyId & "' "
                strsql += vbCrLf + "  AND PV.COSTID='" & cnCostId & "' "
                strsql += vbCrLf + "  AND PV.METALID IN (SELECT TOP 1 METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID='" & _dtgrid.Rows(pt)("ITEMID").ToString & "')"
                strsql += vbCrLf + "  AND PV.ITEMID='" & _dtgrid.Rows(pt)("ITEMID").ToString & "' "
                strsql += vbCrLf + "  AND ISNULL(PV.SUBITEMID,0)IN(" & Val(_dtgrid.Rows(pt)("SUBITEMID").ToString) & ")"
                Dim dtptHd As New DataTable
                cmd = New OleDbCommand(strsql, cn, _tran)
                da = New OleDbDataAdapter(cmd)
                da.Fill(dtptHd)
                If dtptHd.Rows.Count > 0 Then
                    pr_Based = dtptHd.Rows(0)("WEIGHTORAMOUNT").ToString
                    'pr_Points = dtptHd.Rows(0)("POINTS").ToString
                    pr_FixedRange = dtptHd.Rows(0)("FIXEDRANGE").ToString
                    pr_ValueIn = dtptHd.Rows(0)("VALUEINWTRAMT").ToString
                Else
                    strsql = vbCrLf + " SELECT PV.WEIGHTORAMOUNT,PV.POINTS ,PV.FIXEDRANGE,ISNULL(PV.VALUEINWTRAMT,'A')VALUEINWTRAMT "
                    strsql += vbCrLf + "  FROM " & cnAdminDb & "..PRIVILIGE PV WHERE 1=1"
                    'strsql += vbCrLf + "  AND PV.COMPANYID='" & strCompanyId & "' "
                    strsql += vbCrLf + "  AND PV.COSTID='" & cnCostId & "' "
                    strsql += vbCrLf + "  AND PV.METALID IN (SELECT TOP 1 METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID='" & _dtgrid.Rows(pt)("ITEMID").ToString & "')"
                    strsql += vbCrLf + "  AND PV.ITEMID='" & _dtgrid.Rows(pt)("ITEMID").ToString & "' "
                    dtptHd = New DataTable
                    cmd = New OleDbCommand(strsql, cn, _tran)
                    da = New OleDbDataAdapter(cmd)
                    da.Fill(dtptHd)
                    If dtptHd.Rows.Count > 0 Then
                        pr_Based = dtptHd.Rows(0)("WEIGHTORAMOUNT").ToString
                        pr_FixedRange = dtptHd.Rows(0)("FIXEDRANGE").ToString
                        pr_ValueIn = dtptHd.Rows(0)("VALUEINWTRAMT").ToString
                    Else
                        Continue For
                    End If
                End If
                strsql = vbCrLf + " SELECT ISNULL(PV.POINTS,0)POINTS,ISNULL(PV.VALUE,0)VALUE,ISNULL(PV.VALUEINWTRAMT,'A')VALUEINWTRAMT"
                strsql += vbCrLf + " FROM " & cnAdminDb & "..PRIVILIGE PV WHERE 1=1"
                'strsql += vbCrLf + " AND PV.COMPANYID='" & strCompanyId & "'"
                strsql += vbCrLf + " AND PV.COSTID='" & cnCostId & "' "
                strsql += vbCrLf + " AND PV.METALID IN (SELECT TOP 1 METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID='" & _dtgrid.Rows(pt)("ITEMID").ToString & "')"
                strsql += vbCrLf + " AND PV.ITEMID = '" & _dtgrid.Rows(pt)("ITEMID").ToString & "'"
                strsql += vbCrLf + " AND ISNULL(PV.SUBITEMID,0) = '" & _dtgrid.Rows(pt)("SUBITEMID").ToString & "'"
                strsql += vbCrLf + " AND '" & _dtgrid.Rows(pt)("NETWT").ToString & "' BETWEEN PV.FROMRANGE AND PV.TORANGE "
                strsql += vbCrLf + " AND '" & pr_Based & "' ='W'  AND '" & pr_FixedRange & "'<>'Y' AND ISNULL(PV.POINTMUL,'')<>'Y'"
                Dim dtptBd1 As New DataTable
                cmd = New OleDbCommand(strsql, cn, _tran)
                da = New OleDbDataAdapter(cmd)
                da.Fill(dtptBd1)
                If dtptBd1.Rows.Count > 0 Then
                    pr_Points = dtptBd1.Rows(0)("POINTS").ToString
                    pr_Value = dtptBd1.Rows(0)("VALUE").ToString
                    pr_ValueIn = dtptBd1.Rows(0)("VALUEINWTRAMT").ToString
                Else
                    strsql = vbCrLf + " SELECT ISNULL(PV.POINTS,0)POINTS,ISNULL(PV.VALUE,0)VALUE,ISNULL(PV.VALUEINWTRAMT,'A')VALUEINWTRAMT"
                    strsql += vbCrLf + " FROM " & cnAdminDb & "..PRIVILIGE PV WHERE 1=1"
                    'strsql += vbCrLf + " AND PV.COMPANYID='" & strCompanyId & "'"
                    strsql += vbCrLf + " AND PV.COSTID='" & cnCostId & "' "
                    strsql += vbCrLf + " AND PV.METALID IN (SELECT TOP 1 METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID='" & _dtgrid.Rows(pt)("ITEMID").ToString & "')"
                    strsql += vbCrLf + " AND PV.ITEMID = '" & _dtgrid.Rows(pt)("ITEMID").ToString & "'"
                    strsql += vbCrLf + " AND '" & _dtgrid.Rows(pt)("NETWT").ToString & "' BETWEEN PV.FROMRANGE AND PV.TORANGE "
                    strsql += vbCrLf + " AND '" & pr_Based & "' ='W'  AND '" & pr_FixedRange & "'<>'Y' AND ISNULL(PV.POINTMUL,'')<>'Y'"
                    dtptBd1 = New DataTable
                    cmd = New OleDbCommand(strsql, cn, _tran)
                    da = New OleDbDataAdapter(cmd)
                    da.Fill(dtptBd1)
                    If dtptBd1.Rows.Count > 0 Then
                        pr_Points = dtptBd1.Rows(0)("POINTS").ToString
                        pr_Value = dtptBd1.Rows(0)("VALUE").ToString
                        pr_ValueIn = dtptBd1.Rows(0)("VALUEINWTRAMT").ToString
                    End If
                End If

                strsql = vbCrLf + " SELECT POINTS = ISNULL(PV.POINTS,0),VALUE = ISNULL(PV.VALUE,0),VALUEINWTRAMT=ISNULL(PV.VALUEINWTRAMT,'A')"
                strsql += vbCrLf + " FROM " & cnAdminDb & "..PRIVILIGE PV WHERE 1=1"
                'strsql += vbCrLf + " AND PV.COMPANYID='" & strCompanyId & "' "
                strsql += vbCrLf + " AND PV.COSTID='" & cnCostId & "' "
                strsql += vbCrLf + " AND PV.METALID IN (SELECT TOP 1 METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID='" & _dtgrid.Rows(pt)("ITEMID").ToString & "')"
                strsql += vbCrLf + " AND PV.ITEMID = '" & _dtgrid.Rows(pt)("ITEMID").ToString & "'"
                strsql += vbCrLf + " AND ISNULL(PV.SUBITEMID,0) = '" & _dtgrid.Rows(pt)("SUBITEMID").ToString & "'"
                strsql += vbCrLf + " AND '" & Val(_dtgrid.Rows(pt)("GROSSAMT").ToString) & "' BETWEEN PV.FROMRANGE AND PV.TORANGE "
                strsql += vbCrLf + " AND '" & pr_Based & "' ='A'  AND '" & pr_FixedRange & "'<>'Y' AND ISNULL(PV.POINTMUL,'')<>'Y'"
                Dim dtptBd2 As New DataTable
                cmd = New OleDbCommand(strsql, cn, _tran)
                da = New OleDbDataAdapter(cmd)
                da.Fill(dtptBd2)
                If dtptBd2.Rows.Count > 0 Then
                    pr_Points = dtptBd2.Rows(0)("POINTS").ToString
                    pr_Value = dtptBd2.Rows(0)("VALUE").ToString
                    pr_ValueIn = dtptBd2.Rows(0)("VALUEINWTRAMT").ToString
                Else
                    strsql = vbCrLf + " SELECT POINTS = ISNULL(PV.POINTS,0),VALUE = ISNULL(PV.VALUE,0),VALUEINWTRAMT=ISNULL(PV.VALUEINWTRAMT,'A')"
                    strsql += vbCrLf + " FROM " & cnAdminDb & "..PRIVILIGE PV WHERE 1=1"
                    'strsql += vbCrLf + " AND PV.COMPANYID='" & strCompanyId & "' "
                    strsql += vbCrLf + " AND PV.COSTID='" & cnCostId & "' "
                    strsql += vbCrLf + " AND PV.METALID IN (SELECT TOP 1 METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID='" & _dtgrid.Rows(pt)("ITEMID").ToString & "')"
                    strsql += vbCrLf + " AND PV.ITEMID = '" & _dtgrid.Rows(pt)("ITEMID").ToString & "'"
                    strsql += vbCrLf + " AND '" & Val(_dtgrid.Rows(pt)("GROSSAMT").ToString) & "' BETWEEN PV.FROMRANGE AND PV.TORANGE "
                    strsql += vbCrLf + " AND '" & pr_Based & "' ='A'  AND '" & pr_FixedRange & "'<>'Y' AND ISNULL(PV.POINTMUL,'')<>'Y'"
                    dtptBd2 = New DataTable
                    cmd = New OleDbCommand(strsql, cn, _tran)
                    da = New OleDbDataAdapter(cmd)
                    da.Fill(dtptBd2)
                    If dtptBd2.Rows.Count > 0 Then
                        pr_Points = dtptBd2.Rows(0)("POINTS").ToString
                        pr_Value = dtptBd2.Rows(0)("VALUE").ToString
                        pr_ValueIn = dtptBd2.Rows(0)("VALUEINWTRAMT").ToString
                    End If
                End If

                strsql = vbCrLf + " SELECT (ROUND('" & Val(_dtgrid.Rows(pt)("NETWT").ToString) & "',0)) * ISNULL(PV.POINTS,0)POINTS,"
                strsql += vbCrLf + " (((ROUND('" & Val(_dtgrid.Rows(pt)("NETWT").ToString) & "',0)) * ISNULL(PV.POINTS,0))*ISNULL(PV.VALUE,0))VALUE,"
                strsql += vbCrLf + " ISNULL(PV.VALUEINWTRAMT,'A')VALUEINWTRAMT"
                strsql += vbCrLf + " FROM " & cnAdminDb & "..PRIVILIGE PV WHERE 1=1"
                'strsql += vbCrLf + " AND PV.COMPANYID='" & strCompanyId & "' "
                strsql += vbCrLf + " AND PV.COSTID='" & cnCostId & "' "
                strsql += vbCrLf + " AND PV.METALID IN (SELECT TOP 1 METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID='" & _dtgrid.Rows(pt)("ITEMID").ToString & "')"
                strsql += vbCrLf + " AND PV.ITEMID = '" & _dtgrid.Rows(pt)("ITEMID").ToString & "'"
                strsql += vbCrLf + " AND ISNULL(PV.SUBITEMID,0) = '" & _dtgrid.Rows(pt)("SUBITEMID").ToString & "'"
                strsql += vbCrLf + " AND '" & Val(_dtgrid.Rows(pt)("NETWT").ToString) & "' BETWEEN PV.FROMRANGE AND PV.TORANGE "
                strsql += vbCrLf + " AND '" & pr_Based & "' ='W' AND '" & pr_FixedRange & "' <> 'Y' AND ISNULL(PV.POINTMUL,'')='Y'"
                Dim dtptBd3 As New DataTable
                cmd = New OleDbCommand(strsql, cn, _tran)
                da = New OleDbDataAdapter(cmd)
                da.Fill(dtptBd3)
                If dtptBd3.Rows.Count > 0 Then
                    pr_Points = dtptBd3.Rows(0)("POINTS").ToString
                    pr_Value = dtptBd3.Rows(0)("VALUE").ToString
                    pr_ValueIn = dtptBd3.Rows(0)("VALUEINWTRAMT").ToString
                Else
                    strsql = vbCrLf + " SELECT (ROUND('" & Val(_dtgrid.Rows(pt)("NETWT").ToString) & "',0)) * ISNULL(PV.POINTS,0)POINTS,"
                    strsql += vbCrLf + " (((ROUND('" & Val(_dtgrid.Rows(pt)("NETWT").ToString) & "',0)) * ISNULL(PV.POINTS,0))*ISNULL(PV.VALUE,0))VALUE,"
                    strsql += vbCrLf + " ISNULL(PV.VALUEINWTRAMT,'A')VALUEINWTRAMT"
                    strsql += vbCrLf + " FROM " & cnAdminDb & "..PRIVILIGE PV WHERE 1=1"
                    'strsql += vbCrLf + " AND PV.COMPANYID='" & strCompanyId & "' "
                    strsql += vbCrLf + " AND PV.COSTID='" & cnCostId & "' "
                    strsql += vbCrLf + " AND PV.METALID IN (SELECT TOP 1 METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID='" & _dtgrid.Rows(pt)("ITEMID").ToString & "')"
                    strsql += vbCrLf + " AND PV.ITEMID = '" & _dtgrid.Rows(pt)("ITEMID").ToString & "'"
                    strsql += vbCrLf + " AND '" & Val(_dtgrid.Rows(pt)("NETWT").ToString) & "' BETWEEN PV.FROMRANGE AND PV.TORANGE "
                    strsql += vbCrLf + " AND '" & pr_Based & "' ='W' AND '" & pr_FixedRange & "' <> 'Y' AND ISNULL(PV.POINTMUL,'')='Y'"
                    dtptBd3 = New DataTable
                    cmd = New OleDbCommand(strsql, cn, _tran)
                    da = New OleDbDataAdapter(cmd)
                    da.Fill(dtptBd3)
                    If dtptBd3.Rows.Count > 0 Then
                        pr_Points = dtptBd3.Rows(0)("POINTS").ToString
                        pr_Value = dtptBd3.Rows(0)("VALUE").ToString
                        pr_ValueIn = dtptBd3.Rows(0)("VALUEINWTRAMT").ToString
                    End If
                End If
                strsql = vbCrLf + " SELECT (ROUND('" & Val(_dtgrid.Rows(pt)("GROSSAMT").ToString) & "',0)) * ISNULL(PV.POINTS,0)POINTS,"
                strsql += vbCrLf + " (((ROUND('" & Val(_dtgrid.Rows(pt)("GROSSAMT").ToString) & "',0)) * ISNULL(PV.POINTS,0))*ISNULL(PV.VALUE,0))VALUE,ISNULL(PV.VALUEINWTRAMT,'A')VALUEINWTRAMT"
                strsql += vbCrLf + " FROM " & cnAdminDb & "..PRIVILIGE PV WHERE 1=1"
                'strsql += vbCrLf + " AND PV.COMPANYID='" & strCompanyId & "' "
                strsql += vbCrLf + " AND PV.COSTID='" & cnCostId & "' "
                strsql += vbCrLf + " AND PV.METALID IN (SELECT TOP 1 METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID='" & _dtgrid.Rows(pt)("ITEMID").ToString & "')"
                strsql += vbCrLf + " AND PV.ITEMID = '" & _dtgrid.Rows(pt)("ITEMID").ToString & "'"
                strsql += vbCrLf + " AND ISNULL(PV.SUBITEMID,0) = '" & _dtgrid.Rows(pt)("SUBITEMID").ToString & "'"
                strsql += vbCrLf + " AND '" & Val(_dtgrid.Rows(pt)("GROSSAMT").ToString) & "' BETWEEN PV.FROMRANGE AND PV.TORANGE "
                strsql += vbCrLf + " AND '" & pr_Based & "' ='A'  AND '" & pr_FixedRange & "'<>'Y' AND ISNULL(PV.POINTMUL,'')='Y' "
                Dim dtptBd4 As New DataTable
                cmd = New OleDbCommand(strsql, cn, _tran)
                da = New OleDbDataAdapter(cmd)
                da.Fill(dtptBd4)
                If dtptBd4.Rows.Count > 0 Then
                    pr_Points = dtptBd4.Rows(0)("POINTS").ToString
                    pr_Value = dtptBd4.Rows(0)("VALUE").ToString
                    pr_ValueIn = dtptBd4.Rows(0)("VALUEINWTRAMT").ToString
                Else
                    strsql = vbCrLf + " SELECT (ROUND('" & Val(_dtgrid.Rows(pt)("GROSSAMT").ToString) & "',0)) * ISNULL(PV.POINTS,0)POINTS,"
                    strsql += vbCrLf + " (((ROUND('" & Val(_dtgrid.Rows(pt)("GROSSAMT").ToString) & "',0)) * ISNULL(PV.POINTS,0))*ISNULL(PV.VALUE,0))VALUE,ISNULL(PV.VALUEINWTRAMT,'A')VALUEINWTRAMT"
                    strsql += vbCrLf + " FROM " & cnAdminDb & "..PRIVILIGE PV WHERE 1=1"
                    'strsql += vbCrLf + " AND PV.COMPANYID='" & strCompanyId & "' "
                    strsql += vbCrLf + " AND PV.COSTID='" & cnCostId & "' "
                    strsql += vbCrLf + " AND PV.METALID IN (SELECT TOP 1 METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID='" & _dtgrid.Rows(pt)("ITEMID").ToString & "')"
                    strsql += vbCrLf + " AND PV.ITEMID = '" & _dtgrid.Rows(pt)("ITEMID").ToString & "'"
                    strsql += vbCrLf + " AND '" & Val(_dtgrid.Rows(pt)("GROSSAMT").ToString) & "' BETWEEN PV.FROMRANGE AND PV.TORANGE "
                    strsql += vbCrLf + " AND '" & pr_Based & "' ='A'  AND '" & pr_FixedRange & "'<>'Y' AND ISNULL(PV.POINTMUL,'')='Y' "
                    dtptBd4 = New DataTable
                    cmd = New OleDbCommand(strsql, cn, _tran)
                    da = New OleDbDataAdapter(cmd)
                    da.Fill(dtptBd4)
                    If dtptBd4.Rows.Count > 0 Then
                        pr_Points = dtptBd4.Rows(0)("POINTS").ToString
                        pr_Value = dtptBd4.Rows(0)("VALUE").ToString
                        pr_ValueIn = dtptBd4.Rows(0)("VALUEINWTRAMT").ToString
                    End If
                End If

                strsql = vbCrLf + " SELECT (('" & Val(_dtgrid.Rows(pt)("NETWT").ToString) & "')/ISNULL(PV.FROMRANGE,0)) * ISNULL(PV.POINTS,0)POINTS, "
                strsql += vbCrLf + " ISNULL(PV.VALUEINWTRAMT,'A') VALUEINWTRAMT"
                strsql += vbCrLf + " ,(('" & Val(_dtgrid.Rows(pt)("NETWT").ToString) & "')/ISNULL(PV.FROMRANGE,0)) * ISNULL(PV.VALUE,0)VALUE "
                strsql += vbCrLf + " FROM " & cnAdminDb & "..PRIVILIGE PV WHERE 1=1"
                'strsql += vbCrLf + " AND PV.COMPANYID='" & strCompanyId & "' "
                strsql += vbCrLf + " AND PV.COSTID='" & cnCostId & "' "
                strsql += vbCrLf + " AND PV.METALID IN (SELECT TOP 1 METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID='" & _dtgrid.Rows(pt)("ITEMID").ToString & "')"
                strsql += vbCrLf + " AND PV.ITEMID = '" & _dtgrid.Rows(pt)("ITEMID").ToString & "'"
                strsql += vbCrLf + " AND ISNULL(PV.SUBITEMID,0) = '" & _dtgrid.Rows(pt)("SUBITEMID").ToString & "'"
                strsql += vbCrLf + " AND '" & pr_Based & "' ='W' AND '" & pr_FixedRange & "' = 'Y' AND (ISNULL(PV.FROMRANGE,0) <> 0 OR ISNULL(PV.FROMRANGE,0) <> 0)"
                Dim dtptBd5 As New DataTable
                cmd = New OleDbCommand(strsql, cn, _tran)
                da = New OleDbDataAdapter(cmd)
                da.Fill(dtptBd5)
                If dtptBd5.Rows.Count > 0 Then
                    pr_Points = dtptBd5.Rows(0)("POINTS").ToString
                    pr_Value = dtptBd5.Rows(0)("VALUE").ToString
                    pr_ValueIn = dtptBd5.Rows(0)("VALUEINWTRAMT").ToString
                Else
                    strsql = vbCrLf + " SELECT (('" & Val(_dtgrid.Rows(pt)("NETWT").ToString) & "')/ISNULL(PV.FROMRANGE,0)) * ISNULL(PV.POINTS,0)POINTS, "
                    strsql += vbCrLf + " ISNULL(PV.VALUEINWTRAMT,'A') VALUEINWTRAMT"
                    strsql += vbCrLf + " ,(('" & Val(_dtgrid.Rows(pt)("NETWT").ToString) & "')/ISNULL(PV.FROMRANGE,0)) * ISNULL(PV.VALUE,0)VALUE "
                    strsql += vbCrLf + " FROM " & cnAdminDb & "..PRIVILIGE PV WHERE 1=1"
                    'strsql += vbCrLf + " AND PV.COMPANYID='" & strCompanyId & "' "
                    strsql += vbCrLf + " AND PV.COSTID='" & cnCostId & "' "
                    strsql += vbCrLf + " AND PV.METALID IN (SELECT TOP 1 METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID='" & _dtgrid.Rows(pt)("ITEMID").ToString & "')"
                    strsql += vbCrLf + " AND PV.ITEMID = '" & _dtgrid.Rows(pt)("ITEMID").ToString & "'"
                    strsql += vbCrLf + " AND '" & pr_Based & "' ='W' AND '" & pr_FixedRange & "' = 'Y' AND (ISNULL(PV.FROMRANGE,0) <> 0 OR ISNULL(PV.FROMRANGE,0) <> 0)"
                    dtptBd5 = New DataTable
                    cmd = New OleDbCommand(strsql, cn, _tran)
                    da = New OleDbDataAdapter(cmd)
                    da.Fill(dtptBd5)
                    If dtptBd5.Rows.Count > 0 Then
                        pr_Points = dtptBd5.Rows(0)("POINTS").ToString
                        pr_Value = dtptBd5.Rows(0)("VALUE").ToString
                        pr_ValueIn = dtptBd5.Rows(0)("VALUEINWTRAMT").ToString
                    End If
                End If

                strsql = vbCrLf + " SELECT ((ROUND('" & Val(_dtgrid.Rows(pt)("GROSSAMT").ToString) & "',0))/ISNULL(PV.FROMRANGE,0)) * ISNULL(PV.POINTS,0)POINTS,"
                strsql += vbCrLf + " ISNULL('" & pr_ValueIn & "','A')VALUEINWTRAMT,"
                strsql += vbCrLf + " (('" & Val(_dtgrid.Rows(pt)("GROSSAMT").ToString) & "')/ISNULL(PV.FROMRANGE,0)) * ISNULL(PV.VALUE,0)VALUE "
                strsql += vbCrLf + " FROM " & cnAdminDb & "..PRIVILIGE PV WHERE 1=1"
                'strsql += vbCrLf + " AND PV.COMPANYID='" & strCompanyId & "' "
                strsql += vbCrLf + " AND PV.COSTID='" & cnCostId & "' "
                strsql += vbCrLf + " AND PV.METALID IN (SELECT TOP 1 METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID='" & _dtgrid.Rows(pt)("ITEMID").ToString & "')"
                strsql += vbCrLf + " AND PV.ITEMID = '" & _dtgrid.Rows(pt)("ITEMID").ToString & "'"
                strsql += vbCrLf + " AND ISNULL(PV.SUBITEMID,0) = '" & _dtgrid.Rows(pt)("SUBITEMID").ToString & "'"
                strsql += vbCrLf + " AND '" & pr_Based & "'  ='A'  AND '" & pr_FixedRange & "'='Y' AND (ISNULL(PV.FROMRANGE,0) <> 0 OR ISNULL(PV.FROMRANGE,0) <> 0)"
                Dim dtptBd6 As New DataTable
                cmd = New OleDbCommand(strsql, cn, _tran)
                da = New OleDbDataAdapter(cmd)
                da.Fill(dtptBd6)
                If dtptBd6.Rows.Count > 0 Then
                    pr_Points = dtptBd6.Rows(0)("POINTS").ToString
                    pr_Value = dtptBd6.Rows(0)("VALUE").ToString
                    pr_ValueIn = dtptBd6.Rows(0)("VALUEINWTRAMT").ToString
                Else
                    strsql = vbCrLf + " SELECT ((ROUND('" & Val(_dtgrid.Rows(pt)("GROSSAMT").ToString) & "',0))/ISNULL(PV.FROMRANGE,0)) * ISNULL(PV.POINTS,0)POINTS,"
                    strsql += vbCrLf + " ISNULL('" & pr_ValueIn & "','A')VALUEINWTRAMT,"
                    strsql += vbCrLf + " (('" & Val(_dtgrid.Rows(pt)("GROSSAMT").ToString) & "')/ISNULL(PV.FROMRANGE,0)) * ISNULL(PV.VALUE,0)VALUE "
                    strsql += vbCrLf + " FROM " & cnAdminDb & "..PRIVILIGE PV WHERE 1=1"
                    'strsql += vbCrLf + " AND PV.COMPANYID='" & strCompanyId & "' "
                    strsql += vbCrLf + " AND PV.COSTID='" & cnCostId & "' "
                    strsql += vbCrLf + " AND PV.METALID IN (SELECT TOP 1 METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID='" & _dtgrid.Rows(pt)("ITEMID").ToString & "')"
                    strsql += vbCrLf + " AND PV.ITEMID = '" & _dtgrid.Rows(pt)("ITEMID").ToString & "'"
                    strsql += vbCrLf + " AND '" & pr_Based & "'  ='A'  AND '" & pr_FixedRange & "'='Y' AND (ISNULL(PV.FROMRANGE,0) <> 0 OR ISNULL(PV.FROMRANGE,0) <> 0)"
                    dtptBd6 = New DataTable
                    cmd = New OleDbCommand(strsql, cn, _tran)
                    da = New OleDbDataAdapter(cmd)
                    da.Fill(dtptBd6)
                    If dtptBd6.Rows.Count > 0 Then
                        pr_Points = dtptBd6.Rows(0)("POINTS").ToString
                        pr_Value = dtptBd6.Rows(0)("VALUE").ToString
                        pr_ValueIn = dtptBd6.Rows(0)("VALUEINWTRAMT").ToString
                    End If
                End If
                If _dtgrid.Rows(pt)("TRANTYPE").ToString = "SR" Then
                    pt_Points -= pr_Points
                    pt_Value -= pr_Value
                Else
                    pt_Points += pr_Points
                    pt_Value += pr_Value
                End If

                ''''''''''''''''''''''''''''''''''''' 2020-02-22 Added For JMT SA and SR Seperate Record Insert
                If previlId <> "" And Save_Previlige Then
                    strsql = "SELECT COUNT(*)COUNT FROM " & cnAdminDb & "..ACHEAD WHERE PREVILEGEID ='" & previlId & "'"
                    If Not (objGPack.GetSqlValue(strsql, "COUNT", "0", _tran)) > 0 Then Exit Sub
                    Dim strTrantype As String = "R"
                    If pt_Value < 0 Then
                        pt_Value = -1 * pt_Value
                        If pt_Points < 0 Then pt_Points = -1 * pt_Points
                        strTrantype = "I"
                    End If

                    If pt_Value <> 0 And pt_Points <> 0 Then
                        Dim ptsno As String = ""
                        ptsno = GetNewSno(TranSnoType.PRIVILEGETRANCODE, _tran, "GET_ADMINSNO_TRAN")
                        'ptsno = GetNewSno(TranSnoType.PRIVILEGECODE, _tran)
                        strsql = " INSERT INTO " & cnAdminDb & "..PRIVILEGETRAN"
                        strsql += " ("
                        strsql += " SNO,BATCHNO,PREVILEGEID,TRANTYPE,TRANDATE,TRANNO,POINTS,PVALUE,USERID,ENTRYTYPE)"
                        strsql += " VALUES('" & ptsno & "','" & _BatchNo & "','" & previlId & "','" & strTrantype & "'"
                        strsql += " ,'" & _BillDate & "'," & _TRANNO & ",'" & Math.Round(pt_Points, 2) & "','" & Math.Round(pt_Value, 2) & "'"
                        strsql += " ," & userId & ",'A')"
                        ExecQuery(SyncMode.Master, strsql, cn, _tran, _BillCostId, , , , , , , False) ''Sync to all Location

                        strsql = " UPDATE " & cnStockDb & "..ISSUE SET RESNO='" & ptsno & "' WHERE BATCHNO='" & _BatchNo & "' AND COMPANYID='" & strCompanyId & "' "
                        ExecQuery(SyncMode.Transaction, strsql, cn, _tran, _BillCostId)
                        strsql = " UPDATE " & cnStockDb & "..RECEIPT SET JOBISNO='" & ptsno & "' WHERE BATCHNO='" & _BatchNo & "' AND COMPANYID='" & strCompanyId & "' "
                        ExecQuery(SyncMode.Transaction, strsql, cn, _tran, _BillCostId)
                    End If
                    If _TranTypeCol.Contains("SA") And redeemprevilId <> "" And Val(redeemvalue) > 0 Then
                        Dim ptsno As String = ""
                        ptsno = GetNewSno(TranSnoType.PRIVILEGETRANCODE, _tran, "GET_ADMINSNO_TRAN")
                        Dim mredvalue As Decimal = 0
                        Dim mredpts As Decimal = 0
                        mredvalue = Val(redeempoint)
                        mredpts = Val(redeemAmount)
                        If _TranTypeCol.Contains("SR") = True And _TranTypeCol.Contains("SA") = False Then
                            mredvalue = mredvalue * (-1) : mredpts = mredpts * (-1)
                        End If
                        strsql = " INSERT INTO " & cnAdminDb & "..PRIVILEGETRAN"
                        strsql += " ("
                        strsql += " SNO,BATCHNO,PREVILEGEID,TRANTYPE,TRANDATE,TRANNO,POINTS,PVALUE,USERID,ENTRYTYPE)"
                        strsql += " VALUES('" & ptsno & "','" & _BatchNo & "','" & previlId & "','I'"
                        strsql += " ,'" & _BillDate & "'," & _TRANNO & ",'" & mredpts & "','" & mredvalue & "'"
                        strsql += " ," & userId & ",'A')"
                        ExecQuery(SyncMode.Master, strsql, cn, _tran, _BillCostId, , , , , , , False) ''Sync to all Location
                    End If
                End If
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Next

            'If previlId <> "" And Save_Previlige Then '''''''''''''''''''''''''''''''''''''2020-02-22 Comented For JMT SA and SR Seperate Record Insert
            '    strsql = "SELECT COUNT(*)COUNT FROM " & cnAdminDb & "..ACHEAD WHERE PREVILEGEID ='" & previlId & "'"
            '    If Not (objGPack.GetSqlValue(strsql, "COUNT", "0", _tran)) > 0 Then Exit Sub
            '    Dim strTrantype As String = "R"
            '    If pt_Value < 0 Then
            '        pt_Value = -1 * pt_Value
            '        If pt_Points < 0 Then pt_Points = -1 * pt_Points
            '        strTrantype = "I"
            '    End If

            '    If pt_Value <> 0 And pt_Points <> 0 Then
            '        Dim ptsno As String = ""
            '        ptsno = GetNewSno(TranSnoType.PRIVILEGETRANCODE, _tran, "GET_ADMINSNO_TRAN")
            '        'ptsno = GetNewSno(TranSnoType.PRIVILEGECODE, _tran)
            '        strsql = " INSERT INTO " & cnAdminDb & "..PRIVILEGETRAN"
            '        strsql += " ("
            '        strsql += " SNO,BATCHNO,PREVILEGEID,TRANTYPE,TRANDATE,TRANNO,POINTS,PVALUE,USERID,ENTRYTYPE)"
            '        strsql += " VALUES('" & ptsno & "','" & _BatchNo & "','" & previlId & "','" & strTrantype & "'"
            '        strsql += " ,'" & _BillDate & "'," & _TRANNO & ",'" & Math.Round(pt_Points, 2) & "','" & Math.Round(pt_Value, 2) & "'"
            '        strsql += " ," & userId & ",'A')"
            '        ExecQuery(SyncMode.Master, strsql, cn, _tran, _BillCostId, , , , , , , False) ''Sync to all Location

            '        strsql = " UPDATE " & cnStockDb & "..ISSUE SET RESNO='" & ptsno & "' WHERE BATCHNO='" & _BatchNo & "' AND COMPANYID='" & strCompanyId & "' "
            '        ExecQuery(SyncMode.Transaction, strsql, cn, _tran, _BillCostId)
            '        strsql = " UPDATE " & cnStockDb & "..RECEIPT SET JOBISNO='" & ptsno & "' WHERE BATCHNO='" & _BatchNo & "' AND COMPANYID='" & strCompanyId & "' "
            '        ExecQuery(SyncMode.Transaction, strsql, cn, _tran, _BillCostId)
            '    End If
            '    If _TranTypeCol.Contains("SA") And redeemprevilId <> "" And Val(redeemvalue) > 0 Then
            '        Dim ptsno As String = ""
            '        ptsno = GetNewSno(TranSnoType.PRIVILEGETRANCODE, _tran, "GET_ADMINSNO_TRAN")
            '        Dim mredvalue As Decimal = 0
            '        Dim mredpts As Decimal = 0
            '        mredvalue = Val(redeempoint)
            '        mredpts = Val(redeemAmount)
            '        If _TranTypeCol.Contains("SR") = True And _TranTypeCol.Contains("SA") = False Then
            '            mredvalue = mredvalue * (-1) : mredpts = mredpts * (-1)
            '        End If
            '        strsql = " INSERT INTO " & cnAdminDb & "..PRIVILEGETRAN"
            '        strsql += " ("
            '        strsql += " SNO,BATCHNO,PREVILEGEID,TRANTYPE,TRANDATE,TRANNO,POINTS,PVALUE,USERID,ENTRYTYPE)"
            '        strsql += " VALUES('" & ptsno & "','" & _BatchNo & "','" & previlId & "','I'"
            '        strsql += " ,'" & _BillDate & "'," & _TRANNO & ",'" & mredpts & "','" & mredvalue & "'"
            '        strsql += " ," & userId & ",'A')"
            '        ExecQuery(SyncMode.Master, strsql, cn, _tran, _BillCostId, , , , , , , False) ''Sync to all Location
            '    End If
            'End If

        End If
        '----#
        If redeemprevilId <> "" And Val(redeemvalue) > 0 Then
            Dim mredvalue As Decimal = 0
            Dim mredpts As Decimal = 0
            'Alter by vasanth for both weight and amount
            mredvalue = Val(redeempoint) 'Val(redeemvalue)
            mredpts = Val(redeemAmount)
            If _TranTypeCol.Contains("SR") = True And _TranTypeCol.Contains("SA") = False Then
                mredvalue = mredvalue * (-1) : mredpts = mredpts * (-1)
            End If
            strsql = " Insert into " & cnStockDb & "..PriviledgeTran"
            strsql += " ("
            strsql += " BATCHNO,PREVILEGEID,TRANDATE,POINTS,PVALUE)"
            strsql += " VALUES('" & _BatchNo & "','" & redeemprevilId & "'"
            strsql += " ,'" & _BillDate & "'," & mredpts & ""
            strsql += " ," & mredvalue & ")"
            ExecQuery(SyncMode.Transaction, strsql, cn, _tran, _BillCostId)
        End If

    End Sub

#Region "OLD_INSERTACCTRAN"
    'Function Insertacctran _
    '(ByVal tran As OleDbTransaction, _
    'ByVal sno As String, ByVal tNo As Integer, ByVal billdate As Date, ByVal tranMode As String, ByVal accode As String, _
    'ByVal amount As Double, ByVal balance As Double, ByVal pcs As Integer, ByVal grsWT As Double, ByVal netWT As Double, _
    'ByVal payMode As String, ByVal contra As String, ByVal batchno As String, _
    'Optional ByVal BillCashCounterId As String = Nothing, _
    'Optional ByVal BillCostId As String = Nothing, _
    'Optional ByVal VATEXM As String = "N", _
    'Optional ByVal Disc_EmpId As String = "0", _
    'Optional ByVal refNo As String = Nothing, Optional ByVal refDate As String = Nothing, _
    'Optional ByVal chqCardNo As String = Nothing, _
    'Optional ByVal chqDate As String = Nothing, _
    'Optional ByVal chqCardId As Integer = Nothing, _
    'Optional ByVal chqCardRef As String = Nothing, _
    'Optional ByVal Remark1 As String = Nothing, _
    'Optional ByVal Remark2 As String = Nothing _
    ')
    '    If amount = 0 Then Exit Function

    '    strsql = "EXEC " & cnAdminDb & "..SP_INSERT_ACCTRAN @FROMID='" & cnCostId & "'"
    '    strsql += ", @TRANDBNAME='" & cnStockDb & "'"
    '    strsql += ", @SNO='" & sno & "'" 'SNO
    '    strsql += ", @TRANNO=" & tNo & "" 'TRANNO 
    '    strsql += ", @TRANDATE='" & billdate & "'" 'TRANDATE
    '    strsql += ", @TRANMODE='" & tranMode & "'"  'TRANMODE
    '    strsql += ", @ACCODE='" & accode & "'" 'ACCODE
    '    strsql += ", @AMOUNT= " & Math.Abs(amount) & "" 'AMOUNT
    '    strsql += ", @BALANCE= " & balance & "" 'AMOUNT
    '    strsql += ", @PCS=" & Math.Abs(pcs) & "" 'PCS
    '    strsql += ", @GRSWT=" & Math.Abs(grsWT) & ""  'GRSWT
    '    strsql += ", @NETWT=" & Math.Abs(netWT) & "" 'NETWT
    '    If refNo = Nothing Then
    '        strsql += ", @REFNO= ''"  'REFNO
    '    Else
    '        strsql += ", @REFNO='" & refNo & "'"  'REFNO
    '    End If
    '    If refDate = Nothing Then
    '        strsql += ", @REFDATE=''" 'REFDATE
    '    Else
    '        strsql += ", @REFDATE='" & refDate & "'" 'REFDATE
    '    End If
    '    strsql += ", @PAYMODE='" & payMode & "'" 'PAYMODE
    '    If chqCardNo = Nothing Then
    '        strsql += ", @CHQCARDNO= ''" 'CHQCARDNO
    '    Else
    '        strsql += ", @CHQCARDNO='" & chqCardNo & "'" 'CHQCARDNO
    '    End If
    '    strsql += ", @CARDID=" & chqCardId & "" 'CARDID
    '    If chqCardRef = Nothing Then
    '        strsql += ", @CHQCARDREF=''" 'CHQCARDREF
    '    Else
    '        strsql += ", @CHQCARDREF='" & chqCardRef & "'" 'CHQCARDREF
    '    End If
    '    If chqDate = Nothing Then
    '        strsql += ", @CHQDATE=''" 'CHQDATE
    '    Else
    '        strsql += ", @CHQDATE='" & chqDate & "'" 'CHQDATE
    '    End If
    '    strsql += ", @BRSFLAG=''" 'BRSFLAG
    '    strsql += ", @RELIASEDATE=''" 'RELIASEDATE
    '    strsql += ", @FROMFLAG='P'" 'FROMFLAG
    '    If Remark1 = Nothing Then
    '        strsql += ", @REMARK1=''"  'REMARK1
    '    Else
    '        strsql += ", @REMARK1='" & Remark1 & "'"  'REMARK1
    '    End If
    '    If Remark2 = Nothing Then
    '        strsql += ", @REMARK2= ''"  'REMARK2
    '    Else
    '        strsql += ", @REMARK2=' " & Remark2 & "'" 'REMARK2
    '    End If
    '    strsql += ", @CONTRA='" & contra & "'" 'CONTRA
    '    strsql += ", @BATCHNO='" & batchno & "'"  'BATCHNO
    '    strsql += ", @USERID= '" & userId & "'" 'USERID
    '    strsql += ", @UPDATED='" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
    '    strsql += ", @UPTIME='" & Date.Now.ToLongTimeString & "'" 'UPTIME
    '    strsql += ", @SYSTEMID='" & systemId & "'" 'SYSTEMID
    '    strsql += ", @CASHID='" & BillCashCounterId & "'" 'CASHID
    '    strsql += ", @COSTID='" & BillCostId & "'" 'COSTID
    '    strsql += ", @VATEXM='" & VATEXM & "'" 'VATEXM
    '    strsql += ", @APPVER='" & VERSION & "'" 'APPVER
    '    strsql += ", @COMPANYID='" & strCompanyId & "'" 'COMPANYID
    '    strsql += ", @DISC_EMPID=" & Disc_EmpId ' DISC_EMPID
    '    cmd = New OleDbCommand(strsql, cn, tran) : cmd.CommandTimeout = 1000
    '    cmd.ExecuteNonQuery()

    '    'cmd = New OleDbCommand(cnStockDb & "..SP_ACCTRAN_INSERT", cn, tran)
    '    'cmd.CommandType = CommandType.StoredProcedure
    '    'cmd.Parameters.Add("@DBNAME", OleDbType.VarChar).Value = cnStockDb
    '    'cmd.Parameters.Add("@SNO", OleDbType.VarChar).Value = GetNewSno(TranSnoType.ACCTRANCODE, tran) 'SNO
    '    'cmd.Parameters.Add("@TRANNO", OleDbType.VarChar).Value = tNo  'TRANNO 
    '    'cmd.Parameters.Add("@TRANDATE", OleDbType.VarChar).Value = billdate  'TRANDATE
    '    'cmd.Parameters.Add("@TRANMODE", OleDbType.VarChar).Value = tranMode   'TRANMODE
    '    'cmd.Parameters.Add("@ACCODE", OleDbType.VarChar).Value = accode  'ACCODE
    '    'cmd.Parameters.Add("@AMOUNT", OleDbType.VarChar).Value = Math.Abs(amount) 'AMOUNT
    '    'cmd.Parameters.Add("@PCS", OleDbType.VarChar).Value = Math.Abs(pcs) 'PCS
    '    'cmd.Parameters.Add("@GRSWT", OleDbType.VarChar).Value = Math.Abs(grsWT)  'GRSWT
    '    'cmd.Parameters.Add("@NETWT", OleDbType.VarChar).Value = Math.Abs(netWT) 'NETWT
    '    'If refNo = Nothing Then
    '    '    cmd.Parameters.Add("@REFNO", OleDbType.VarChar).Value = "''"  'REFNO
    '    'Else
    '    '    cmd.Parameters.Add("@REFNO", OleDbType.VarChar).Value = refNo  'REFNO
    '    'End If
    '    'If refDate = Nothing Then
    '    '    cmd.Parameters.Add("@REFDATE", OleDbType.VarChar).Value = "''" 'REFDATE
    '    'Else
    '    '    cmd.Parameters.Add("@REFDATE", OleDbType.VarChar).Value = refDate 'REFDATE
    '    'End If
    '    'cmd.Parameters.Add("@PAYMODE", OleDbType.VarChar).Value = payMode 'PAYMODE
    '    'If chqCardNo = Nothing Then
    '    '    cmd.Parameters.Add("@CHQCARDNO", OleDbType.VarChar).Value = "''" 'CHQCARDNO
    '    'Else
    '    '    cmd.Parameters.Add("@CHQCARDNO", OleDbType.VarChar).Value = chqCardNo 'CHQCARDNO
    '    'End If
    '    'cmd.Parameters.Add("@CARDID", OleDbType.VarChar).Value = chqCardId 'CARDID
    '    'If chqCardRef = Nothing Then
    '    '    cmd.Parameters.Add("@CHQCARDREF", OleDbType.VarChar).Value = "''" 'CHQCARDREF
    '    'Else
    '    '    cmd.Parameters.Add("@CHQCARDREF", OleDbType.VarChar).Value = chqCardRef 'CHQCARDREF
    '    'End If
    '    'If chqDate = Nothing Then
    '    '    cmd.Parameters.Add("@CHQDATE", OleDbType.VarChar).Value = "''" 'CHQDATE
    '    'Else
    '    '    cmd.Parameters.Add("@CHQDATE", OleDbType.VarChar).Value = chqDate 'CHQDATE
    '    'End If
    '    'cmd.Parameters.Add("@BRSFLAG", OleDbType.VarChar).Value = "''" 'BRSFLAG
    '    'cmd.Parameters.Add("@RELIASEDATE", OleDbType.VarChar).Value = "''" 'RELIASEDATE
    '    'cmd.Parameters.Add("@FROMFLAG", OleDbType.VarChar).Value = "P" 'FROMFLAG
    '    'If Remark1 = Nothing Then
    '    '    cmd.Parameters.Add("@REMARK1", OleDbType.VarChar).Value = "''"  'REMARK1
    '    'Else
    '    '    cmd.Parameters.Add("@REMARK1", OleDbType.VarChar).Value = Remark1  'REMARK1
    '    'End If
    '    'If Remark2 = Nothing Then
    '    '    cmd.Parameters.Add("@REMARK2", OleDbType.VarChar).Value = "''"  'REMARK2
    '    'Else
    '    '    cmd.Parameters.Add("@REMARK2", OleDbType.VarChar).Value = Remark2  'REMARK2
    '    'End If
    '    'cmd.Parameters.Add("@CONTRA", OleDbType.VarChar).Value = contra  'CONTRA
    '    'cmd.Parameters.Add("@BATCHNO", OleDbType.VarChar).Value = batchno  'BATCHNO
    '    'cmd.Parameters.Add("@USERID", OleDbType.VarChar).Value = userId  'USERID
    '    'cmd.Parameters.Add("@UPDATED", OleDbType.VarChar).Value = Today.Date.ToString("yyyy-MM-dd") 'UPDATED
    '    'cmd.Parameters.Add("@UPTIME", OleDbType.VarChar).Value = Date.Now.ToLongTimeString  'UPTIME
    '    'cmd.Parameters.Add("@SYSTEMID", OleDbType.VarChar).Value = systemId  'SYSTEMID
    '    'cmd.Parameters.Add("@CASHID", OleDbType.VarChar).Value = BillCashCounterId  'CASHID
    '    'cmd.Parameters.Add("@COSTID", OleDbType.VarChar).Value = BillCostId 'COSTID
    '    'cmd.Parameters.Add("@VATEXM", OleDbType.VarChar).Value = VATEXM  'VATEXM
    '    'cmd.Parameters.Add("@APPVER", OleDbType.VarChar).Value = VERSION 'APPVER
    '    'cmd.Parameters.Add("@COMPANYID", OleDbType.VarChar).Value = strCompanyId 'COMPANYID
    '    'cmd.Parameters.Add("@DISC_EMPID", OleDbType.VarChar).Value = Disc_EmpId ' DISC_EMPID
    '    'cmd.ExecuteNonQuery()

    'End Function
#End Region

    Function Insertacctran _
    (ByVal tran As OleDbTransaction, ByVal editFlag As Boolean,
    ByVal TableName As String, ByVal sno As String, ByVal tNo As Integer, ByVal billdate As Date, ByVal tranMode As String, ByVal accode As String, ByVal Saccode As String,
    ByVal amount As Double, ByVal balance As Double, ByVal pcs As Integer, ByVal grsWT As Double, ByVal netWT As Double,
    ByVal payMode As String, ByVal contra As String, ByVal batchno As String,
    Optional ByVal BillCashCounterId As String = Nothing,
    Optional ByVal BillCostId As String = Nothing,
    Optional ByVal SBillCostId As String = Nothing,
    Optional ByVal VATEXM As String = "N",
    Optional ByVal Disc_EmpId As String = "0",
    Optional ByVal refNo As String = Nothing, Optional ByVal refDate As String = Nothing,
    Optional ByVal chqCardNo As String = Nothing,
    Optional ByVal chqDate As String = Nothing,
    Optional ByVal chqCardId As Integer = Nothing,
    Optional ByVal chqCardRef As String = Nothing,
    Optional ByVal Remark1 As String = Nothing,
    Optional ByVal Remark2 As String = Nothing,
    Optional ByVal WT_ENTORDER As String = "0",
    Optional ByVal TDSCATID As String = "0",
    Optional ByVal TDSPER As String = "0",
    Optional ByVal TDSAMOUNT As String = "0",
    Optional ByVal FLAG As String = Nothing,
    Optional ByVal PCODE As String = Nothing,
    Optional ByVal fromFlag As String = "A")
        If amount = 0 Then Exit Function
        If TableName = "" Then Exit Function


        strsql = "EXEC " & cnAdminDb & "..SP_INSERT_ACCTRAN @TABLENAME='" & TableName & "',@FROMID='" & cnCostId & "'"
        strsql += vbCrLf + " , @TRANDBNAME='" & cnStockDb & "'"
        strsql += vbCrLf + " , @SNO='" & sno & "'" 'SNO
        strsql += vbCrLf + " , @TRANNO=" & tNo & "" 'TRANNO 
        strsql += vbCrLf + " , @TRANDATE='" & billdate.Date.ToString("yyyy-MM-dd") & "'" 'TRANDATE
        strsql += vbCrLf + " , @TRANMODE='" & tranMode & "'"  'TRANMODE
        strsql += vbCrLf + " , @ACCODE='" & accode & "'" 'ACCODE
        strsql += vbCrLf + " , @SACCODE='" & Saccode & "'" 'ACCODE
        strsql += vbCrLf + " , @AMOUNT= " & Math.Abs(amount) & "" 'AMOUNT
        strsql += vbCrLf + " , @BALANCE= " & balance & "" 'AMOUNT
        strsql += vbCrLf + " , @PCS=" & Math.Abs(pcs) & "" 'PCS
        strsql += vbCrLf + " , @GRSWT=" & Math.Abs(grsWT) & ""  'GRSWT
        strsql += vbCrLf + " , @NETWT=" & Math.Abs(netWT) & "" 'NETWT
        If refNo = Nothing Then
            strsql += vbCrLf + " , @REFNO= ''"  'REFNO
        Else
            strsql += vbCrLf + " , @REFNO='" & refNo & "'"  'REFNO
        End If
        If refDate = Nothing And refDate = "NULL" Then
            strsql += vbCrLf + " , @REFDATE=NULL" 'REFDATE
        Else
            strsql += vbCrLf + " , @REFDATE='" & refDate & "'" 'REFDATE
        End If
        strsql += vbCrLf + " , @PAYMODE='" & payMode & "'" 'PAYMODE
        If chqCardNo = Nothing Then
            strsql += vbCrLf + " , @CHQCARDNO= ''" 'CHQCARDNO
        Else
            strsql += vbCrLf + " , @CHQCARDNO='" & chqCardNo & "'" 'CHQCARDNO
        End If
        strsql += vbCrLf + " , @CARDID=" & chqCardId & "" 'CARDID
        If chqCardRef = Nothing Then
            strsql += vbCrLf + " , @CHQCARDREF=''" 'CHQCARDREF
        Else
            strsql += vbCrLf + " , @CHQCARDREF='" & chqCardRef & "'" 'CHQCARDREF
        End If
        If chqDate = Nothing Then
            strsql += vbCrLf + " , @CHQDATE=''" 'CHQDATE
        Else
            strsql += vbCrLf + " , @CHQDATE='" & chqDate & "'" 'CHQDATE
        End If
        strsql += vbCrLf + " , @BRSFLAG=''" 'BRSFLAG
        strsql += vbCrLf + " , @RELIASEDATE=''" 'RELIASEDATE
        strsql += vbCrLf + " , @FROMFLAG='" & fromFlag & "'" 'FROMFLAG
        If Remark1 = Nothing Then
            strsql += vbCrLf + " , @REMARK1=''"  'REMARK1
        Else
            strsql += vbCrLf + " , @REMARK1='" & Remark1 & "'"  'REMARK1
        End If
        If Remark2 = Nothing Then
            strsql += vbCrLf + " , @REMARK2= ''"  'REMARK2
        Else
            strsql += vbCrLf + " , @REMARK2='" & Remark2 & "'" 'REMARK2
        End If
        strsql += vbCrLf + " , @CONTRA='" & contra & "'" 'CONTRA
        strsql += vbCrLf + " , @BATCHNO='" & batchno & "'"  'BATCHNO
        strsql += vbCrLf + " , @USERID= '" & userId & "'" 'USERID
        strsql += vbCrLf + " , @UPDATED='" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strsql += vbCrLf + " , @UPTIME='" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strsql += vbCrLf + " , @SYSTEMID='" & systemId & "'" 'SYSTEMID
        strsql += vbCrLf + " , @CASHID='" & BillCashCounterId & "'" 'CASHID
        strsql += vbCrLf + " , @COSTID='" & BillCostId & "'" 'COSTID
        strsql += vbCrLf + " , @SCOSTID='" & SBillCostId & "'" 'COSTID
        strsql += vbCrLf + " , @VATEXM='" & VATEXM & "'" 'VATEXM
        strsql += vbCrLf + " , @APPVER='" & VERSION & "'" 'APPVER
        strsql += vbCrLf + " , @COMPANYID='" & strCompanyId & "'" 'COMPANYID
        strsql += vbCrLf + " , @DISC_EMPID=" & Disc_EmpId ' DISC_EMPID
        strsql += vbCrLf + " , @WT_ENTORDER=" & WT_ENTORDER ' WT_ENTORDER
        strsql += vbCrLf + " , @TDSCATID=" & TDSCATID ' TDSCATID
        strsql += vbCrLf + " , @TDSPER=" & TDSPER ' TDSPER
        strsql += vbCrLf + " , @TDSAMOUNT=" & TDSAMOUNT ' TDSPER
        strsql += vbCrLf + " , @FLAG='" & FLAG & "'" ' FLAG
        strsql += vbCrLf + " , @PCODE='" & PCODE & "'" ' PCODE for  VBJ
        cmd = New OleDbCommand(strsql, cn, tran) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strsql = ""
        cmd = Nothing
    End Function
    Function InsertGstRegister(ByVal tran As OleDbTransaction, ByVal IssNo As String, ByVal Batchno As String, ByVal Costid As String, ByVal CompanyId As String _
                , ByVal Tranno As Integer, ByVal Trandate As Date, ByVal Trantype As String _
                , ByVal ItemName As String, ByVal Hsn As String, ByVal Pcs As Integer, ByVal Rate As Decimal _
                , ByVal Amount As Decimal, ByVal Wt As Decimal, ByVal SgstPer As Decimal, ByVal CgstPer As Decimal, ByVal IgstPer As Decimal _
                , ByVal Sgst As Decimal, ByVal Cgst As Decimal, ByVal Igst As Decimal, ByVal PartyName As String)
        strsql = " INSERT INTO " & cnStockDb & "..GSTREGISTER"
        strsql += " ("
        strsql += " SNO,ISSSNO,TRANNO,TRANDATE,TRANTYPE,PCS,WEIGHT,RATE,AMOUNT,TAX,TAXAMOUNT"
        strsql += " ,SGST_PER,CGST_PER,IGST_PER"
        strsql += " ,SGST,CGST,IGST"
        strsql += " ,BATCHNO,COSTID,COMPANYID,SUPNAME"
        strsql += " )"
        strsql += " VALUES"
        strsql += " ("
        strsql += " '" & GetNewSno(TranSnoType.GSTREGISTERCODE, tran) & "' " 'SNO
        strsql += " ,'" & IssNo & "' " 'ISSSNO
        strsql += " ," & Tranno & "" 'TRANNO
        strsql += " ,'" & Trandate & "'" 'TRANDATE  
        strsql += " ,'" & Trantype & "'" 'TRANTYPE  
        strsql += " ," & Pcs & "" 'PCS
        strsql += " ," & Wt & "" 'WEIGHT
        strsql += " ," & Rate & "" 'RATE
        strsql += " ," & Amount & "" 'AMOUNT
        strsql += " ," & (SgstPer + CgstPer + IgstPer) & "" 'TAX  
        strsql += " ," & (Sgst + Cgst + Igst) & "" 'TAXAMOUNT  
        strsql += " ," & SgstPer & "" 'SGST_PER  
        strsql += " ," & CgstPer & "" 'CGST_PER  
        strsql += " ," & IgstPer & "" 'IGST_PER  
        strsql += " ," & Sgst & "" 'SGST   
        strsql += " ," & Cgst & "" 'CGST 
        strsql += " ," & Igst & "" 'IGST 
        strsql += " ,'" & Batchno & "'" 'BATCHNO
        strsql += " ,'" & Costid & "'" 'COSTID  
        strsql += " ,'" & CompanyId & "'" 'COMPANYID  
        strsql += " ,'" & PartyName & "'" 'SUPNAME  
        strsql += " )"
        ExecQuery(SyncMode.Transaction, strsql, cn, tran, Costid)
    End Function
    Function UpdateGstRegister(ByVal tran As OleDbTransaction, ByVal IssNo As String, ByVal Batchno As String, ByVal Costid As String, ByVal CompanyId As String _
                , ByVal Tranno As Integer, ByVal Trandate As Date, ByVal Trantype As String _
                , ByVal ItemName As String, ByVal Hsn As String, ByVal Pcs As Integer, ByVal Rate As Decimal _
                , ByVal Amount As Decimal, ByVal Wt As Decimal, ByVal SgstPer As Decimal, ByVal CgstPer As Decimal, ByVal IgstPer As Decimal _
                , ByVal Sgst As Decimal, ByVal Cgst As Decimal, ByVal Igst As Decimal, ByVal PartyName As String)
        strsql = " UPDATE " & cnStockDb & "..GSTREGISTER"
        strsql += " SET"
        strsql += " PCS = " & Pcs & "" 'PCS
        strsql += " ,WEIGHT = " & Wt & "" 'WEIGHT
        strsql += " ,RATE = " & Rate & "" 'RATE
        strsql += " ,AMOUNT = " & Amount & "" 'AMOUNT
        strsql += " ,TAX = " & (SgstPer + CgstPer + IgstPer) & "" 'TAX  
        strsql += " ,TAXAMOUNT = " & (Sgst + Cgst + Igst) & "" 'TAXAMOUNT  
        strsql += " ,SGST_PER = " & SgstPer & "" 'SGST_PER  
        strsql += " ,CGST_PER = " & CgstPer & "" 'CGST_PER  
        strsql += " ,IGST_PER = " & IgstPer & "" 'IGST_PER  
        strsql += " ,SGST = " & Sgst & "" 'SGST   
        strsql += " ,CGST = " & Cgst & "" 'CGST 
        strsql += " ,IGST = " & Igst & "" 'IGST 
        strsql += " WHERE BATCHNO = '" & Batchno & "'"
        ExecQuery(SyncMode.Transaction, strsql, cn, tran, Costid)
    End Function
    Function InsertSASRDetails(
    ByVal tran As OleDbTransaction,
    ByVal issSno As String, ByVal TNO As String, ByVal billdate As Date, ByVal BatchNo As String, ByVal GoldRate As String, ByVal OrdAdvanceWeight As String, ByVal DIRECTREPAIR As String _
    , ByVal tranType As String, ByVal CATCODE As String, ByVal pcs As Integer, ByVal grsWt As Double, ByVal netWT As Double, ByVal LessWt As Double _
    , ByVal PuerWt As Double, ByVal Tagno As String, ByVal Itemid As Integer, ByVal SubItemid As Integer, ByVal WASTPER As Double, ByVal wast As Double _
    , ByVal MCGRM As Double, ByVal mc As Double, ByVal amount As Double, ByVal rate As Double, ByVal BOARDRATE As Double, ByVal SALEMODE As String, ByVal GRSNET As String _
    , ByVal REFNO As Integer, ByVal REFDATE As String, ByVal FLAG As String, ByVal stnAmt As Double, ByVal miscAmt As Double, ByVal empId As Integer _
    , ByVal TAGPCS As Double, ByVal TAGGRSWT As Double, ByVal TAGNETWT As Double, ByVal TAGRATE As Double, ByVal TAGSVALUE As Double _
    , ByVal TAGDESIGNER As Integer, ByVal ITEMCTRID As Integer, ByVal ITEMTYPEID As Integer, ByVal PURITY As Decimal, ByVal TABLECODE As String _
    , ByVal ACCODE As String, ByVal REMARK1 As String, ByVal REMARK2 As String, ByVal DISCOUNT As Double, ByVal RUNNO As String, ByVal vat As Double, ByVal SC As Double _
    , Optional ByVal TOUCH As Double = 0, Optional ByVal DISCOUNT_FINAL As Double = 0, Optional ByVal RATEID As Double = 0 _
    , Optional ByVal SETGRPID As String = Nothing _
    , Optional ByVal EDC1 As String = Nothing, Optional ByVal EDC2 As String = Nothing, Optional ByVal EDC3 As String = Nothing _
    , Optional ByVal BillCashCounterId As String = Nothing _
    , Optional ByVal BillCostId As String = Nothing _
    , Optional ByVal VATEXM As String = "N", Optional ByVal BALANCEINWEIGHT As Boolean = False, Optional ByVal isOrder As Boolean = False _
    , Optional ByVal tranNo As Integer = Nothing, Optional ByVal EstNo As Integer = Nothing _
    , Optional ByVal ORDSTATE_ID As Integer = Nothing, Optional ByVal ALLOYWT As Double = Nothing _
    , Optional ByVal ALLOYAMT As Double = Nothing, Optional ByVal INCENTIVE As String = Nothing _
    , Optional ByVal IS2D As String = Nothing, Optional ByVal AuthEmpid As Integer = Nothing _
    , Optional ByVal ED As String = Nothing, Optional ByVal CHIT_DISCOUNT As Double = 0 _
    , Optional ByVal SGSTPER As Double = 0, Optional ByVal CGSTPER As Double = 0, Optional ByVal IGSTPER As Double = 0 _
    , Optional ByVal SGST As Double = 0, Optional ByVal CGST As Double = 0, Optional ByVal IGST As Double = 0 _
    , Optional ByVal EDSNO1 As String = "", Optional ByVal EDSNO2 As String = "", Optional ByVal EDSNO3 As String = "" _
    , Optional ByVal SGSTSNO As String = "", Optional ByVal CGSTSNO As String = "", Optional ByVal IGSTSNO As String = "" _
    , Optional ByVal EntOrder As Integer = 0 _
    , Optional ByVal CESSPER As Double = 0, Optional ByVal CESS As Double = 0, Optional ByVal CESSSNO As String = "", Optional ByVal HSN As String = ""
    )
        If GetBillControlValue("GEN-SIMILARNO", tran) = "Y" Then
            HasSimilarBillNo = True
        Else
            HasSimilarBillNo = False
        End If
        'If tranType = "AI" Or tranType = "MI" Then
        '    SGST = 0 : CGST = 0 : IGST = 0
        '    EDC1 = Nothing : EDC2 = Nothing : EDC3 = Nothing
        'End If
        If tranType = "SR" Or tranType = "AR" Then
            strsql = " EXEC " & cnAdminDb & "..SP_INSERT_RECEIPT @FROMID='" & cnCostId & "'"
        Else
            strsql = " EXEC " & cnAdminDb & "..SP_INSERT_ISSUE @FROMID='" & cnCostId & "'"
        End If
        strsql += ", @DBNAME='" & cnStockDb & "'"
        strsql += ", @SNO='" & issSno & "'" 'SNO
        strsql += ", @TRANNO=" & TNO & "" 'TRANNO 
        strsql += ", @TRANDATE='" & billdate & "'" 'TRANDATE
        If Val(OrdAdvanceWeight) > 0 Then
            If isOrder Then strsql += ",  @TRANTYPE='OD'" Else strsql += ",  @TRANTYPE='RD'" 'TRANTYPE
        ElseIf DIRECTREPAIR = "Y" Then
            strsql += ", @TRANTYPE='RD'" 'TRANTYPE
        Else
            strsql += ", @TRANTYPE='" & tranType & "'" 'TRANTYPE
        End If
        strsql += ", @PCS=" & pcs & "" 'PCS
        strsql += ", @GRSWT=" & grsWt & "" 'GRSWT
        strsql += ", @NETWT=" & netWT & "" 'NETWT
        strsql += ", @LESSWT=" & LessWt & ""  'LESSWT
        strsql += ", @PUREWT=" & PuerWt & "" 'PUREWT '0
        strsql += ", @TAGNO='" & Tagno & "'" 'TAGNO
        strsql += ", @ITEMID=" & Itemid & "" 'ITEMID
        strsql += ", @SUBITEMID=" & SubItemid & "" 'SUBITEMID
        strsql += ", @WASTPER=" & WASTPER & "" 'WASTPER
        strsql += ", @WASTAGE=" & wast & "" 'WASTAGE
        strsql += ", @MCGRM=" & MCGRM & "" 'MCGRM
        strsql += ", @MCHARGE=" & mc & "" 'MCHARGE
        strsql += ", @AMOUNT=" & amount & "" 'AMOUNT
        strsql += ", @RATE=" & rate & "" 'RATE
        Dim mxrate As Decimal = BOARDRATE
        If mxrate = 0 Then mxrate = Val(GoldRate)
        strsql += ", @BOARDRATE=" & mxrate & "" 'BOARDRATE
        strsql += ", @SALEMODE='" & SALEMODE & "'" 'SALEMODE
        strsql += ", @GRSNET='" & GRSNET & "'" 'GRSNET
        'strsql += ", @TRANSTATUS='" & IIf(IS2D = "Y", "2", "") & "'" 'TRANSTATUS ''
        strsql += ", @TRANSTATUS=''" 'TRANSTATUS ''
        If REFNO <> 0 Then
            strsql += ", @REFNO='" & REFNO & "'" 'REFNO ''
        Else
            strsql += ", @REFNO=''" 'REFNO ''
        End If

        If REFDATE <> Nothing Then
            strsql += ", @REFDATE='" & REFDATE & "'" 'REFDATE NULL
        Else
            strsql += ", @REFDATE=''" 'REFDATE NULL
        End If
        strsql += ", @COSTID='" & BillCostId & "'" 'COSTID 
        strsql += ", @COMPANYID='" & strCompanyId & "'" 'COMPANYID
        strsql += ", @FLAG='" & FLAG & "'" 'FLAG 
        strsql += ", @EMPID=" & empId & "" 'EMPID
        strsql += ", @TAGPCS=" & TAGPCS & "" 'TAGPCS
        strsql += ", @TAGGRSWT= " & TAGGRSWT & "" 'TAGGRSWT
        strsql += ", @TAGNETWT=" & TAGNETWT & "" 'TAGNETWT
        strsql += ", @TAGRATEID=" & TAGRATE & "" 'TAGRATEID
        strsql += ", @TAGSVALUE=" & TAGSVALUE & "" 'TAGSVALUE
        strsql += ", @TAGDESIGNER='" & TAGDESIGNER & "'" 'TAGDESIGNER
        strsql += ", @ITEMCTRID=" & ITEMCTRID & "" 'ITEMCTRID
        strsql += ", @ITEMTYPEID=" & ITEMTYPEID & " " 'ITEMTYPEID
        strsql += ", @PURITY=" & PURITY & "" 'PURITY
        strsql += ", @TABLECODE='" & TABLECODE & "'" 'TABLECODE
        strsql += ", @INCENTIVE='" & INCENTIVE & "'" 'INCENTIVE
        strsql += ", @WEIGHTUNIT=''" 'WEIGHTUNIT
        strsql += ", @CATCODE='" & CATCODE & "'" 'CATCODE
        If tranType = "MI" Then
            strsql += ", @OCATCODE='" & CATCODE & "'" 'OCATCODE
        Else
            strsql += ", @OCATCODE=''" 'OCATCODE
        End If

        strsql += ", @ACCODE='" & ACCODE & "'" 'ACCODE
        strsql += ", @ALLOY=" & ALLOYWT & "" 'ALLOY
        strsql += ", @BATCHNO='" & BatchNo & "'" 'BATCHNO
        strsql += ", @REMARK1='" & REMARK1 & " '" 'REMARK1
        strsql += ", @REMARK2='" & REMARK2 & "'" 'REMARK2
        strsql += ", @USERID='" & userId & "'" 'USERID
        strsql += ", @UPDATED='" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strsql += ", @UPTIME='" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strsql += ", @SYSTEMID='" & systemId & "'" 'SYSTEMID
        strsql += ", @DISCOUNT=" & DISCOUNT & " " 'DISCOUNT
        strsql += ", @RUNNO='" & RUNNO & "'" 'RUNNO
        strsql += ", @CASHID='" & BillCashCounterId & "'" 'CASHID
        strsql += ", @VATEXM='" & VATEXM & "'" 'VATEXM
        strsql += ", @TAX=" & vat & "" 'TAX
        strsql += ", @SC=" & SC & "" 'SC
        If tranType = "SR" Or tranType = "AR" Then
            strsql += ", @DUSTWT=0" 'DUSTWT
            strsql += ", @MELTWT=0" 'DUSTWT
            strsql += ", @PUREXCH=''" 'PUREXCH
            strsql += ", @MAKE=''" 'MAKE
        End If
        If Val(OrdAdvanceWeight.ToString) > 0 Then
            strsql += ", @STNAMT=0" 'STONEAMT
            strsql += ", @MISCAMT=0" 'MISCAMT
        Else
            strsql += ", @STNAMT=" & stnAmt & "" 'STONEAMT
            strsql += ", @MISCAMT=" & miscAmt & "" 'MISCAMT
        End If
        strsql += ", @METALID='" & objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & CATCODE & "'", , , tran) & "'" 'MTALID
        strsql += ", @STONEUNIT='" & objGPack.GetSqlValue("SELECT STONEUNIT FROM " & cnAdminDb & "..ITEMMAST  WHERE ITEMID = " & Itemid, , , tran) & "'" 'STONEUNIT
        strsql += ", @APPVER='" & VERSION & "'" 'APPVER
        strsql += ", @TOUCH=" & TOUCH & "" 'TOUCH '
        If EstNo <> 0 Then
            strsql += ", @ESTSNO=" & EstNo & "" 'ESTSNO'
        Else
            strsql += ", @ESTSNO=''" 'ESTSNO'
        End If
        strsql += ", @OTHERAMT=" & Math.Round(ALLOYAMT, 2) & ""  'OTHERAMT
        strsql += ", @FIN_DISCOUNT=" & DISCOUNT_FINAL & "" 'DISCOUNT
        strsql += ", @RATEID=" & RATEID & "" 'RATEID
        strsql += ", @SETGRPID='" & SETGRPID & "'" 'SETGRPID
        strsql += ", @TRANFLAG='" & IIf(BALANCEINWEIGHT, "W", "") & "'" 'TRANFLAG
        strsql += ", @EXDUTY='" & objSoftKeys.EXDUTY & "'" 'EXDUTY
        strsql += ", @EDC1='" & Val(EDC1) & "'" 'EDC1
        strsql += ", @EDC2='" & Val(EDC2) & "'" 'EDC2
        strsql += ", @EDC3='" & Val(EDC3) & "'" 'EDC3
        strsql += ", @EDC1ID='" & objSoftKeys.EDC1ID & "'" 'EDC1ID
        strsql += ", @EDC2ID='" & objSoftKeys.EDC2ID & "'" 'EDC2ID
        strsql += ", @EDC3ID='" & objSoftKeys.EDC3ID & "'" 'EDC3ID
        strsql += ", @EDC1PER='" & objSoftKeys.EDC1PER & "'" 'EDC1PER
        strsql += ", @EDC2PER='" & objSoftKeys.EDC2PER & "'" 'EDC2PER
        strsql += ", @EDC3PER='" & objSoftKeys.EDC3PER & "'" 'EDC3PER
        strsql += ", @ORDSTATE_ID='" & ORDSTATE_ID & "'" 'TRANFLAG
        strsql += ", @DISC_EMPID='" & AuthEmpid & "'" 'TRANFLAG
        strsql += ", @STKTYPE='" & ED & "'" 'STKTYPE
        If tranType <> "SR" And tranType <> "AR" Then
            strsql += ", @CHIT_DISCOUNT=" & CHIT_DISCOUNT & "" 'DISCOUNT
        End If
        strsql += ", @SGST='" & Val(SGST) & "'" 'SGST
        strsql += ", @CGST='" & Val(CGST) & "'" 'CGST
        strsql += ", @IGST='" & Val(IGST) & "'" 'IGST
        strsql += ", @SGSTPER='" & SGSTPER & "'" 'SGSTPER
        strsql += ", @CGSTPER='" & CGSTPER & "'" 'CGSTPER
        strsql += ", @IGSTPER='" & IGSTPER & "'" 'IGSTPER
        strsql += ", @EDSNO1='" & EDSNO1 & "'" 'EDSNO1
        strsql += ", @EDSNO2='" & EDSNO2 & "'" 'EDSNO2
        strsql += ", @EDSNO3='" & EDSNO3 & "'" 'EDSNO3
        strsql += ", @SGSTSNO='" & SGSTSNO & "'" 'SGSTSNO
        strsql += ", @CGSTSNO='" & CGSTSNO & "'" 'CGSTSNO
        strsql += ", @IGSTSNO='" & IGSTSNO & "'" 'IGSTSNO
        strsql += ", @ENTORDER='" & EntOrder & "'" 'IGSTSNO
        strsql += ", @CESS='" & Val(CESS) & "'" 'CESS
        strsql += ", @CESSPER='" & CESSPER & "'" 'CESSPER
        strsql += ", @CESSSNO='" & CESSSNO & "'" 'CESSSNO
        strsql += ", @HSNCODE='" & HSN & "'" 'HSNCODE
        cmd = New OleDbCommand(strsql, cn, tran) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function
    Function UpdateSASRDetails(
    ByVal tran As OleDbTransaction,
    ByVal issSno As String, ByVal TNO As String, ByVal billdate As Date, ByVal BatchNo As String, ByVal GoldRate As String, ByVal OrdAdvanceWeight As String, ByVal DIRECTREPAIR As String _
    , ByVal tranType As String, ByVal CATCODE As String, ByVal pcs As Integer, ByVal grsWt As Double, ByVal netWT As Double, ByVal LessWt As Double _
    , ByVal PuerWt As Double, ByVal Tagno As String, ByVal Itemid As Integer, ByVal SubItemid As Integer, ByVal WASTPER As Double, ByVal wast As Double _
    , ByVal MCGRM As Double, ByVal mc As Double, ByVal amount As Double, ByVal rate As Double, ByVal BOARDRATE As Double, ByVal SALEMODE As String, ByVal GRSNET As String _
    , ByVal REFNO As Integer, ByVal REFDATE As String, ByVal FLAG As String, ByVal stnAmt As Double, ByVal miscAmt As Double, ByVal empId As Integer _
    , ByVal TAGPCS As Double, ByVal TAGGRSWT As Double, ByVal TAGNETWT As Double, ByVal TAGRATE As Double, ByVal TAGSVALUE As Double _
    , ByVal TAGDESIGNER As Integer, ByVal ITEMCTRID As Integer, ByVal ITEMTYPEID As Integer, ByVal PURITY As Decimal, ByVal TABLECODE As String _
    , ByVal ACCODE As String, ByVal REMARK1 As String, ByVal REMARK2 As String, ByVal DISCOUNT As Double, ByVal RUNNO As String, ByVal vat As Double, ByVal SC As Double _
    , Optional ByVal TOUCH As Double = 0, Optional ByVal DISCOUNT_FINAL As Double = 0, Optional ByVal RATEID As Double = 0 _
    , Optional ByVal SETGRPID As String = Nothing _
    , Optional ByVal EDC1 As String = Nothing, Optional ByVal EDC2 As String = Nothing, Optional ByVal EDC3 As String = Nothing _
    , Optional ByVal BillCashCounterId As String = Nothing _
    , Optional ByVal BillCostId As String = Nothing _
    , Optional ByVal VATEXM As String = "N", Optional ByVal BALANCEINWEIGHT As Boolean = False, Optional ByVal isOrder As Boolean = False _
    , Optional ByVal tranNo As Integer = Nothing, Optional ByVal EstNo As Integer = Nothing _
    , Optional ByVal ORDSTATE_ID As Integer = Nothing, Optional ByVal ALLOYWT As Double = Nothing _
    , Optional ByVal ALLOYAMT As Double = Nothing, Optional ByVal INCENTIVE As String = Nothing _
    , Optional ByVal IS2D As String = Nothing, Optional ByVal AuthEmpid As Integer = Nothing _
    , Optional ByVal ED As String = Nothing, Optional ByVal CHIT_DISCOUNT As Double = 0 _
    , Optional ByVal SGSTPER As Double = 0, Optional ByVal CGSTPER As Double = 0, Optional ByVal IGSTPER As Double = 0 _
    , Optional ByVal SGST As Double = 0, Optional ByVal CGST As Double = 0, Optional ByVal IGST As Double = 0 _
    , Optional ByVal EDSNO1 As String = "", Optional ByVal EDSNO2 As String = "", Optional ByVal EDSNO3 As String = "" _
    , Optional ByVal SGSTSNO As String = "", Optional ByVal CGSTSNO As String = "", Optional ByVal IGSTSNO As String = "" _
    , Optional ByVal EntOrder As Integer = 0 _
    , Optional ByVal CESSPER As Double = 0, Optional ByVal CESS As Double = 0, Optional ByVal CESSSNO As String = "", Optional ByVal HSN As String = "" _
    , Optional ByVal issNo As String = ""
    )
        If GetBillControlValue("GEN-SIMILARNO", tran) = "Y" Then
            HasSimilarBillNo = True
        Else
            HasSimilarBillNo = False
        End If
        'If tranType = "AI" Or tranType = "MI" Then
        '    SGST = 0 : CGST = 0 : IGST = 0
        '    EDC1 = Nothing : EDC2 = Nothing : EDC3 = Nothing
        'End If
        If tranType = "SR" Or tranType = "AR" Then
            strsql = " EXEC " & cnAdminDb & "..SP_INSERT_RECEIPT @FROMID='" & cnCostId & "'"
        Else
            strsql = " EXEC " & cnAdminDb & "..SP_UPDATE_ISSUE @FROMID='" & cnCostId & "'"
        End If
        strsql += ", @DBNAME='" & cnStockDb & "'"
        strsql += ", @SNO='" & issSno & "'" 'SNO
        strsql += ", @TRANNO=" & TNO & "" 'TRANNO 
        strsql += ", @TRANDATE='" & billdate & "'" 'TRANDATE
        If Val(OrdAdvanceWeight) > 0 Then
            If isOrder Then strsql += ",  @TRANTYPE='OD'" Else strsql += ",  @TRANTYPE='RD'" 'TRANTYPE
        ElseIf DIRECTREPAIR = "Y" Then
            strsql += ", @TRANTYPE='RD'" 'TRANTYPE
        Else
            strsql += ", @TRANTYPE='" & tranType & "'" 'TRANTYPE
        End If
        strsql += ", @PCS=" & pcs & "" 'PCS
        strsql += ", @GRSWT=" & grsWt & "" 'GRSWT
        strsql += ", @NETWT=" & netWT & "" 'NETWT
        strsql += ", @LESSWT=" & LessWt & ""  'LESSWT
        strsql += ", @PUREWT=" & PuerWt & "" 'PUREWT '0
        strsql += ", @TAGNO='" & Tagno & "'" 'TAGNO
        strsql += ", @ITEMID=" & Itemid & "" 'ITEMID
        strsql += ", @SUBITEMID=" & SubItemid & "" 'SUBITEMID
        strsql += ", @WASTPER=" & WASTPER & "" 'WASTPER
        strsql += ", @WASTAGE=" & wast & "" 'WASTAGE
        strsql += ", @MCGRM=" & MCGRM & "" 'MCGRM
        strsql += ", @MCHARGE=" & mc & "" 'MCHARGE
        strsql += ", @AMOUNT=" & amount & "" 'AMOUNT
        strsql += ", @RATE=" & rate & "" 'RATE
        Dim mxrate As Decimal = BOARDRATE
        If mxrate = 0 Then mxrate = Val(GoldRate)
        strsql += ", @BOARDRATE=" & mxrate & "" 'BOARDRATE
        strsql += ", @SALEMODE='" & SALEMODE & "'" 'SALEMODE
        strsql += ", @GRSNET='" & GRSNET & "'" 'GRSNET
        'strsql += ", @TRANSTATUS='" & IIf(IS2D = "Y", "2", "") & "'" 'TRANSTATUS ''
        strsql += ", @TRANSTATUS=''" 'TRANSTATUS ''
        If REFNO <> 0 Then
            strsql += ", @REFNO='" & REFNO & "'" 'REFNO ''
        Else
            strsql += ", @REFNO=''" 'REFNO ''
        End If

        If REFDATE <> Nothing Then
            strsql += ", @REFDATE='" & REFDATE & "'" 'REFDATE NULL
        Else
            strsql += ", @REFDATE=''" 'REFDATE NULL
        End If
        strsql += ", @COSTID='" & BillCostId & "'" 'COSTID 
        strsql += ", @COMPANYID='" & strCompanyId & "'" 'COMPANYID
        strsql += ", @FLAG='" & FLAG & "'" 'FLAG 
        strsql += ", @EMPID=" & empId & "" 'EMPID
        strsql += ", @TAGPCS=" & TAGPCS & "" 'TAGPCS
        strsql += ", @TAGGRSWT= " & TAGGRSWT & "" 'TAGGRSWT
        strsql += ", @TAGNETWT=" & TAGNETWT & "" 'TAGNETWT
        strsql += ", @TAGRATEID=" & TAGRATE & "" 'TAGRATEID
        strsql += ", @TAGSVALUE=" & TAGSVALUE & "" 'TAGSVALUE
        strsql += ", @TAGDESIGNER='" & TAGDESIGNER & "'" 'TAGDESIGNER
        strsql += ", @ITEMCTRID=" & ITEMCTRID & "" 'ITEMCTRID
        strsql += ", @ITEMTYPEID=" & ITEMTYPEID & " " 'ITEMTYPEID
        strsql += ", @PURITY=" & PURITY & "" 'PURITY
        strsql += ", @TABLECODE='" & TABLECODE & "'" 'TABLECODE
        strsql += ", @INCENTIVE='" & INCENTIVE & "'" 'INCENTIVE
        strsql += ", @WEIGHTUNIT=''" 'WEIGHTUNIT
        strsql += ", @CATCODE='" & CATCODE & "'" 'CATCODE
        If tranType = "MI" Then
            strsql += ", @OCATCODE='" & CATCODE & "'" 'OCATCODE
        Else
            strsql += ", @OCATCODE=''" 'OCATCODE
        End If

        strsql += ", @ACCODE='" & ACCODE & "'" 'ACCODE
        strsql += ", @ALLOY=" & ALLOYWT & "" 'ALLOY
        strsql += ", @BATCHNO='" & BatchNo & "'" 'BATCHNO
        strsql += ", @REMARK1='" & REMARK1 & " '" 'REMARK1
        strsql += ", @REMARK2='" & REMARK2 & "'" 'REMARK2
        strsql += ", @USERID='" & userId & "'" 'USERID
        strsql += ", @UPDATED='" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strsql += ", @UPTIME='" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strsql += ", @SYSTEMID='" & systemId & "'" 'SYSTEMID
        strsql += ", @DISCOUNT=" & DISCOUNT & " " 'DISCOUNT
        strsql += ", @RUNNO='" & RUNNO & "'" 'RUNNO
        strsql += ", @CASHID='" & BillCashCounterId & "'" 'CASHID
        strsql += ", @VATEXM='" & VATEXM & "'" 'VATEXM
        strsql += ", @TAX=" & vat & "" 'TAX
        strsql += ", @SC=" & SC & "" 'SC
        If tranType = "SR" Or tranType = "AR" Then
            strsql += ", @DUSTWT=0" 'DUSTWT
            strsql += ", @MELTWT=0" 'DUSTWT
            strsql += ", @PUREXCH=''" 'PUREXCH
            strsql += ", @MAKE=''" 'MAKE
        End If
        If Val(OrdAdvanceWeight.ToString) > 0 Then
            strsql += ", @STNAMT=0" 'STONEAMT
            strsql += ", @MISCAMT=0" 'MISCAMT
        Else
            strsql += ", @STNAMT=" & stnAmt & "" 'STONEAMT
            strsql += ", @MISCAMT=" & miscAmt & "" 'MISCAMT
        End If
        strsql += ", @METALID='" & objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & CATCODE & "'", , , tran) & "'" 'MTALID
        strsql += ", @STONEUNIT='" & objGPack.GetSqlValue("SELECT STONEUNIT FROM " & cnAdminDb & "..ITEMMAST  WHERE ITEMID = " & Itemid, , , tran) & "'" 'STONEUNIT
        strsql += ", @APPVER='" & VERSION & "'" 'APPVER
        strsql += ", @TOUCH=" & TOUCH & "" 'TOUCH '
        If EstNo <> 0 Then
            strsql += ", @ESTSNO=" & EstNo & "" 'ESTSNO'
        Else
            strsql += ", @ESTSNO=''" 'ESTSNO'
        End If
        strsql += ", @OTHERAMT=" & Math.Round(ALLOYAMT, 2) & ""  'OTHERAMT
        strsql += ", @FIN_DISCOUNT=" & DISCOUNT_FINAL & "" 'DISCOUNT
        strsql += ", @RATEID=" & RATEID & "" 'RATEID
        strsql += ", @SETGRPID='" & SETGRPID & "'" 'SETGRPID
        strsql += ", @TRANFLAG='" & IIf(BALANCEINWEIGHT, "W", "") & "'" 'TRANFLAG
        strsql += ", @EXDUTY='" & objSoftKeys.EXDUTY & "'" 'EXDUTY
        strsql += ", @EDC1='" & Val(EDC1) & "'" 'EDC1
        strsql += ", @EDC2='" & Val(EDC2) & "'" 'EDC2
        strsql += ", @EDC3='" & Val(EDC3) & "'" 'EDC3
        strsql += ", @EDC1ID='" & objSoftKeys.EDC1ID & "'" 'EDC1ID
        strsql += ", @EDC2ID='" & objSoftKeys.EDC2ID & "'" 'EDC2ID
        strsql += ", @EDC3ID='" & objSoftKeys.EDC3ID & "'" 'EDC3ID
        strsql += ", @EDC1PER='" & objSoftKeys.EDC1PER & "'" 'EDC1PER
        strsql += ", @EDC2PER='" & objSoftKeys.EDC2PER & "'" 'EDC2PER
        strsql += ", @EDC3PER='" & objSoftKeys.EDC3PER & "'" 'EDC3PER
        strsql += ", @ORDSTATE_ID='" & ORDSTATE_ID & "'" 'TRANFLAG
        strsql += ", @DISC_EMPID='" & AuthEmpid & "'" 'TRANFLAG
        strsql += ", @STKTYPE='" & ED & "'" 'STKTYPE
        If tranType <> "SR" And tranType <> "AR" Then
            strsql += ", @CHIT_DISCOUNT=" & CHIT_DISCOUNT & "" 'DISCOUNT
        End If
        strsql += ", @SGST='" & Val(SGST) & "'" 'SGST
        strsql += ", @CGST='" & Val(CGST) & "'" 'CGST
        strsql += ", @IGST='" & Val(IGST) & "'" 'IGST
        strsql += ", @SGSTPER='" & SGSTPER & "'" 'SGSTPER
        strsql += ", @CGSTPER='" & CGSTPER & "'" 'CGSTPER
        strsql += ", @IGSTPER='" & IGSTPER & "'" 'IGSTPER
        strsql += ", @EDSNO1='" & EDSNO1 & "'" 'EDSNO1
        strsql += ", @EDSNO2='" & EDSNO2 & "'" 'EDSNO2
        strsql += ", @EDSNO3='" & EDSNO3 & "'" 'EDSNO3
        strsql += ", @SGSTSNO='" & SGSTSNO & "'" 'SGSTSNO
        strsql += ", @CGSTSNO='" & CGSTSNO & "'" 'CGSTSNO
        strsql += ", @IGSTSNO='" & IGSTSNO & "'" 'IGSTSNO
        strsql += ", @ENTORDER='" & EntOrder & "'" 'IGSTSNO
        strsql += ", @CESS='" & Val(CESS) & "'" 'CESS
        strsql += ", @CESSPER='" & CESSPER & "'" 'CESSPER
        strsql += ", @CESSSNO='" & CESSSNO & "'" 'CESSSNO
        strsql += ", @HSNCODE='" & HSN & "'" 'HSNCODE
        strsql += ", @ISSNO='" & issNo & "'"
        cmd = New OleDbCommand(strsql, cn, tran) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function
    Function InsertPURDetails(
    ByVal tran As OleDbTransaction,
    ByVal issSno As String, ByVal TNO As String, ByVal billdate As Date, ByVal BatchNo As String _
    , ByVal tranType As String, ByVal CATCODE As String, ByVal pcs As Integer, ByVal grsWt As Double, ByVal netWT As Double, ByVal LessWt As Double _
    , ByVal PuerWt As Double, ByVal Itemid As Integer, ByVal SubItemid As Integer, ByVal WASTPER As Double, ByVal wast As Double _
    , ByVal amount As Double, ByVal rate As Double, ByVal BOARDRATE As Double, ByVal FLAG As String, ByVal stnAmt As Double _
    , ByVal miscAmt As Double, ByVal empId As Integer, ByVal ITEMTYPEID As Integer, ByVal PURITY As Decimal, ByVal ALLOY As Double _
    , ByVal REMARK1 As String, ByVal REMARK2 As String, ByVal DISCOUNT As Double, ByVal vat As Double, ByVal SC As Double _
    , ByVal DUSTWT As String, ByVal MELTWT As String, ByVal PUREXCH As String, ByVal MAKE As String, ByVal TOUCH As Double, ByVal OTHERAMT As Double _
    , Optional ByVal BillCashCounterId As String = Nothing _
    , Optional ByVal BillCostId As String = Nothing _
    , Optional ByVal VATEXM As String = "N" _
    , Optional ByVal tranNo As Integer = Nothing, Optional ByVal EstNo As Integer = Nothing, Optional ByVal DiscAuth As Integer = Nothing _
    , Optional ByVal EDC1 As String = Nothing, Optional ByVal EDC2 As String = Nothing, Optional ByVal EDC3 As String = Nothing _
    , Optional ByVal SGSTPER As Double = 0, Optional ByVal CGSTPER As Double = 0, Optional ByVal IGSTPER As Double = 0 _
    , Optional ByVal SGST As Double = 0, Optional ByVal CGST As Double = 0, Optional ByVal IGST As Double = 0 _
    , Optional ByVal EDSNO1 As String = "", Optional ByVal EDSNO2 As String = "", Optional ByVal EDSNO3 As String = "" _
    , Optional ByVal SGSTSNO As String = "", Optional ByVal CGSTSNO As String = "", Optional ByVal IGSTSNO As String = "" _
    , Optional ByVal EntOrder As Integer = 0
    )

        strsql = " EXEC " & cnAdminDb & "..SP_INSERT_RECEIPT @FROMID='" & cnCostId & "'"
        strsql += ", @DBNAME='" & cnStockDb & "'"
        strsql += ", @SNO='" & issSno & "'" 'SNO
        strsql += ", @TRANNO=" & TNO & "" 'TRANNO 
        strsql += ", @TRANDATE='" & billdate & "'" 'TRANDATE
        strsql += ", @TRANTYPE='" & tranType & "'" 'TRANTYPE
        strsql += ", @PCS=" & pcs & "" 'PCS
        strsql += ", @GRSWT=" & grsWt & "" 'GRSWT
        strsql += ", @NETWT=" & netWT & "" 'NETWT
        strsql += ", @LESSWT=" & LessWt & ""  'LESSWT
        strsql += ", @PUREWT=" & PuerWt & "" 'PUREWT 
        strsql += ", @TAGNO=''" 'TAGNO
        strsql += ", @ITEMID=" & Itemid & "" 'ITEMID
        strsql += ", @SUBITEMID=" & SubItemid & "" 'SUBITEMID
        strsql += ", @WASTPER=" & WASTPER & "" 'WASTPER
        strsql += ", @WASTAGE=" & wast & "" 'WASTAGE
        strsql += ", @MCGRM=0" 'MCGRM
        strsql += ", @MCHARGE=0" 'MCHARGE
        strsql += ", @AMOUNT=" & amount & "" 'AMOUNT
        strsql += ", @RATE=" & rate & "" 'RATE
        strsql += ", @BOARDRATE=" & BOARDRATE & "" 'BOARDRATE
        strsql += ", @SALEMODE=''" 'SALEMODE
        strsql += ", @GRSNET=''" 'GRSNET
        strsql += ", @TRANSTATUS=''" 'TRANSTATUS ''
        strsql += ", @REFNO=''" 'REFNO ''
        strsql += ", @REFDATE=''" 'REFDATE NULL
        strsql += ", @COSTID='" & BillCostId & "'" 'COSTID 
        strsql += ", @COMPANYID='" & strCompanyId & "'" 'COMPANYID
        strsql += ", @FLAG='" & FLAG & "'" 'FLAG 
        strsql += ", @EMPID=" & empId & "" 'EMPID
        strsql += ", @TAGPCS=''" 'TAGPCS
        strsql += ", @TAGGRSWT= 0" 'TAGGRSWT
        strsql += ", @TAGNETWT=0" 'TAGNETWT
        strsql += ", @TAGRATEID=0" 'TAGRATEID
        strsql += ", @TAGSVALUE=0" 'TAGSVALUE
        strsql += ", @TAGDESIGNER=''" 'TAGDESIGNER
        strsql += ", @ITEMCTRID=0" 'ITEMCTRID
        strsql += ", @ITEMTYPEID=" & ITEMTYPEID & " " 'ITEMTYPEID
        strsql += ", @PURITY=" & PURITY & "" 'PURITY
        strsql += ", @TABLECODE=''" 'TABLECODE
        strsql += ", @INCENTIVE=''" 'INCENTIVE
        strsql += ", @WEIGHTUNIT=''" 'WEIGHTUNIT
        strsql += ", @CATCODE='" & CATCODE & "'" 'CATCODE
        strsql += ", @OCATCODE=''" 'OCATCODE
        strsql += ", @ACCODE=''" 'ACCODE
        strsql += ", @ALLOY=" & ALLOY & "" 'ALLOY
        strsql += ", @BATCHNO='" & BatchNo & "'" 'BATCHNO
        strsql += ", @REMARK1='" & REMARK1 & " '" 'REMARK1
        strsql += ", @REMARK2='" & REMARK2 & "'" 'REMARK2
        strsql += ", @USERID='" & userId & "'" 'USERID
        strsql += ", @UPDATED='" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strsql += ", @UPTIME='" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strsql += ", @SYSTEMID='" & systemId & "'" 'SYSTEMID
        strsql += ", @DISCOUNT=" & DISCOUNT & " " 'DISCOUNT
        strsql += ", @RUNNO=''" 'RUNNO
        strsql += ", @CASHID='" & BillCashCounterId & "'" 'CASHID
        strsql += ", @VATEXM='" & VATEXM & "'" 'VATEXM
        strsql += ", @TAX=" & vat & "" 'TAX
        strsql += ", @SC=" & SC & "" 'SC
        strsql += ", @DUSTWT=" & DUSTWT & "" 'DUSTWT
        strsql += ", @MELTWT=" & MELTWT & "" 'DUSTWT
        strsql += ", @PUREXCH='" & PUREXCH & "'" 'PUREXCH
        strsql += ", @MAKE='" & MAKE & "'" 'MAKE
        strsql += ", @STNAMT=" & stnAmt & "" 'STONEAMT
        strsql += ", @MISCAMT=" & miscAmt & "" 'MISCAMT
        strsql += ", @METALID='" & objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & CATCODE & "'", , , tran) & "'" 'MTALID
        strsql += ", @STONEUNIT='" & objGPack.GetSqlValue("SELECT STONEUNIT FROM " & cnAdminDb & "..ITEMMAST  WHERE ITEMID = " & Itemid, , , tran) & "'" 'STONEUNIT
        strsql += ", @APPVER='" & VERSION & "'" 'APPVER
        strsql += ", @TOUCH=" & TOUCH & "" 'TOUCH '
        If EstNo <> 0 Then
            strsql += ", @ESTSNO=" & EstNo & "" 'ESTSNO'
        Else
            strsql += ", @ESTSNO=''" 'ESTSNO'
        End If
        strsql += ", @OTHERAMT=" & OTHERAMT & "" 'OTHERAMT
        strsql += ", @FIN_DISCOUNT=0" 'DISCOUNT
        strsql += ", @RATEID=0" 'RATEID
        strsql += ", @SETGRPID=''" 'SETGRPID
        strsql += ", @TRANFLAG=''" 'TRANFLAG
        strsql += ", @EXDUTY='" & objSoftKeys.EXDUTY & "'" 'EXDUTY
        strsql += ", @EDC1='" & Val(EDC1) & "'" 'EDC1
        strsql += ", @EDC2='" & Val(EDC2) & "'" 'EDC2
        strsql += ", @EDC3='" & Val(EDC3) & "'" 'EDC3
        strsql += ", @EDC1ID='" & objSoftKeys.EDC1ID & "'" 'EDC1ID
        strsql += ", @EDC2ID='" & objSoftKeys.EDC2ID & "'" 'EDC2ID
        strsql += ", @EDC3ID='" & objSoftKeys.EDC3ID & "'" 'EDC3ID
        strsql += ", @EDC1PER='" & objSoftKeys.EDC1PER & "'" 'EDC1PER
        strsql += ", @EDC2PER='" & objSoftKeys.EDC2PER & "'" 'EDC2PER
        strsql += ", @EDC3PER='" & objSoftKeys.EDC3PER & "'" 'EDC3PER
        strsql += ", @ORDSTATE_ID=0" 'TRANFLAG
        strsql += ", @DISC_EMPID=" & DiscAuth 'TRANFLAG
        strsql += ", @STKTYPE='" & IIf(Val(EDC1) > 0, "M", "T") & "'" 'STKTYPE
        strsql += ", @SGST='" & Val(SGST) & "'" 'SGST
        strsql += ", @CGST='" & Val(CGST) & "'" 'CGST
        strsql += ", @IGST='" & Val(IGST) & "'" 'IGST
        strsql += ", @SGSTPER='" & SGSTPER & "'" 'SGSTPER
        strsql += ", @CGSTPER='" & CGSTPER & "'" 'CGSTPER
        strsql += ", @IGSTPER='" & IGSTPER & "'" 'IGSTPER
        strsql += ", @EDSNO1='" & EDSNO1 & "'" 'EDSNO1
        strsql += ", @EDSNO2='" & EDSNO2 & "'" 'EDSNO2
        strsql += ", @EDSNO3='" & EDSNO3 & "'" 'EDSNO3
        strsql += ", @SGSTSNO='" & SGSTSNO & "'" 'SGSTSNO
        strsql += ", @CGSTSNO='" & CGSTSNO & "'" 'CGSTSNO
        strsql += ", @IGSTSNO='" & IGSTSNO & "'" 'IGSTSNO
        strsql += ", @ENTORDER='" & EntOrder & "'" 'IGSTSNO
        strsql += ", @CESS='0'" 'IGSTSNO
        strsql += ", @CESSPER='0'" 'IGSTSNO
        strsql += ", @CESSSNO=''" 'IGSTSNO
        strsql += ", @HSNCODE=''" 'HSNCODE
        cmd = New OleDbCommand(strsql, cn, tran) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function
    Function UpdatePURDetails(
    ByVal tran As OleDbTransaction,
    ByVal issSno As String, ByVal TNO As String, ByVal billdate As Date, ByVal BatchNo As String _
    , ByVal tranType As String, ByVal CATCODE As String, ByVal pcs As Integer, ByVal grsWt As Double, ByVal netWT As Double, ByVal LessWt As Double _
    , ByVal PuerWt As Double, ByVal Itemid As Integer, ByVal SubItemid As Integer, ByVal WASTPER As Double, ByVal wast As Double _
    , ByVal amount As Double, ByVal rate As Double, ByVal BOARDRATE As Double, ByVal FLAG As String, ByVal stnAmt As Double _
    , ByVal miscAmt As Double, ByVal empId As Integer, ByVal ITEMTYPEID As Integer, ByVal PURITY As Decimal, ByVal ALLOY As Double _
    , ByVal REMARK1 As String, ByVal REMARK2 As String, ByVal DISCOUNT As Double, ByVal vat As Double, ByVal SC As Double _
    , ByVal DUSTWT As String, ByVal MELTWT As String, ByVal PUREXCH As String, ByVal MAKE As String, ByVal TOUCH As Double, ByVal OTHERAMT As Double _
    , Optional ByVal BillCashCounterId As String = Nothing _
    , Optional ByVal BillCostId As String = Nothing _
    , Optional ByVal VATEXM As String = "N" _
    , Optional ByVal tranNo As Integer = Nothing, Optional ByVal EstNo As Integer = Nothing, Optional ByVal DiscAuth As Integer = Nothing _
    , Optional ByVal EDC1 As String = Nothing, Optional ByVal EDC2 As String = Nothing, Optional ByVal EDC3 As String = Nothing _
    , Optional ByVal SGSTPER As Double = 0, Optional ByVal CGSTPER As Double = 0, Optional ByVal IGSTPER As Double = 0 _
    , Optional ByVal SGST As Double = 0, Optional ByVal CGST As Double = 0, Optional ByVal IGST As Double = 0 _
    , Optional ByVal EDSNO1 As String = "", Optional ByVal EDSNO2 As String = "", Optional ByVal EDSNO3 As String = "" _
    , Optional ByVal SGSTSNO As String = "", Optional ByVal CGSTSNO As String = "", Optional ByVal IGSTSNO As String = "" _
    , Optional ByVal EntOrder As Integer = 0 _
    , Optional ByVal WT_ENTORDER As Integer = 0
    )
        strsql = " EXEC " & cnAdminDb & "..SP_UPDATE_RECEIPT @FROMID='" & cnCostId & "'"
        strsql += ", @DBNAME='" & cnStockDb & "'"
        strsql += ", @SNO='" & issSno & "'" 'SNO
        strsql += ", @TRANNO=" & TNO & "" 'TRANNO 
        strsql += ", @TRANDATE='" & billdate & "'" 'TRANDATE
        strsql += ", @TRANTYPE='" & tranType & "'" 'TRANTYPE
        strsql += ", @PCS=" & pcs & "" 'PCS
        strsql += ", @GRSWT=" & grsWt & "" 'GRSWT
        strsql += ", @NETWT=" & netWT & "" 'NETWT
        strsql += ", @LESSWT=" & LessWt & ""  'LESSWT
        strsql += ", @PUREWT=" & PuerWt & "" 'PUREWT 
        strsql += ", @TAGNO=''" 'TAGNO
        strsql += ", @ITEMID=" & Itemid & "" 'ITEMID
        strsql += ", @SUBITEMID=" & SubItemid & "" 'SUBITEMID
        strsql += ", @WASTPER=" & WASTPER & "" 'WASTPER
        strsql += ", @WASTAGE=" & wast & "" 'WASTAGE
        strsql += ", @MCGRM=0" 'MCGRM
        strsql += ", @MCHARGE=0" 'MCHARGE
        strsql += ", @AMOUNT=" & amount & "" 'AMOUNT
        strsql += ", @RATE=" & rate & "" 'RATE
        strsql += ", @BOARDRATE=" & BOARDRATE & "" 'BOARDRATE
        strsql += ", @SALEMODE=''" 'SALEMODE
        strsql += ", @GRSNET=''" 'GRSNET
        strsql += ", @TRANSTATUS=''" 'TRANSTATUS ''
        strsql += ", @REFNO=''" 'REFNO ''
        strsql += ", @REFDATE=''" 'REFDATE NULL
        strsql += ", @COSTID='" & BillCostId & "'" 'COSTID 
        strsql += ", @COMPANYID='" & strCompanyId & "'" 'COMPANYID
        strsql += ", @FLAG='" & FLAG & "'" 'FLAG 
        strsql += ", @EMPID=" & empId & "" 'EMPID
        strsql += ", @TAGPCS=''" 'TAGPCS
        strsql += ", @TAGGRSWT= 0" 'TAGGRSWT
        strsql += ", @TAGNETWT=0" 'TAGNETWT
        strsql += ", @TAGRATEID=0" 'TAGRATEID
        strsql += ", @TAGSVALUE=0" 'TAGSVALUE
        strsql += ", @TAGDESIGNER=''" 'TAGDESIGNER
        strsql += ", @ITEMCTRID=0" 'ITEMCTRID
        strsql += ", @ITEMTYPEID=" & ITEMTYPEID & " " 'ITEMTYPEID
        strsql += ", @PURITY=" & PURITY & "" 'PURITY
        strsql += ", @TABLECODE=''" 'TABLECODE
        strsql += ", @INCENTIVE=''" 'INCENTIVE
        strsql += ", @WEIGHTUNIT=''" 'WEIGHTUNIT
        strsql += ", @CATCODE='" & CATCODE & "'" 'CATCODE
        strsql += ", @OCATCODE=''" 'OCATCODE
        strsql += ", @ACCODE=''" 'ACCODE
        strsql += ", @ALLOY=" & ALLOY & "" 'ALLOY
        strsql += ", @BATCHNO='" & BatchNo & "'" 'BATCHNO
        strsql += ", @REMARK1='" & REMARK1 & " '" 'REMARK1
        strsql += ", @REMARK2='" & REMARK2 & "'" 'REMARK2
        strsql += ", @USERID='" & userId & "'" 'USERID
        strsql += ", @UPDATED='" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strsql += ", @UPTIME='" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strsql += ", @SYSTEMID='" & systemId & "'" 'SYSTEMID
        strsql += ", @DISCOUNT=" & DISCOUNT & " " 'DISCOUNT
        strsql += ", @RUNNO=''" 'RUNNO
        strsql += ", @CASHID='" & BillCashCounterId & "'" 'CASHID
        strsql += ", @VATEXM='" & VATEXM & "'" 'VATEXM
        strsql += ", @TAX=" & vat & "" 'TAX
        strsql += ", @SC=" & SC & "" 'SC
        strsql += ", @DUSTWT=" & DUSTWT & "" 'DUSTWT
        strsql += ", @MELTWT=" & MELTWT & "" 'DUSTWT
        strsql += ", @PUREXCH='" & PUREXCH & "'" 'PUREXCH
        strsql += ", @MAKE='" & MAKE & "'" 'MAKE
        strsql += ", @STNAMT=" & stnAmt & "" 'STONEAMT
        strsql += ", @MISCAMT=" & miscAmt & "" 'MISCAMT
        strsql += ", @METALID='" & objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & CATCODE & "'", , , tran) & "'" 'MTALID
        strsql += ", @STONEUNIT='" & objGPack.GetSqlValue("SELECT STONEUNIT FROM " & cnAdminDb & "..ITEMMAST  WHERE ITEMID = " & Itemid, , , tran) & "'" 'STONEUNIT
        strsql += ", @APPVER='" & VERSION & "'" 'APPVER
        strsql += ", @TOUCH=" & TOUCH & "" 'TOUCH '
        If EstNo <> 0 Then
            strsql += ", @ESTSNO=" & EstNo & "" 'ESTSNO'
        Else
            strsql += ", @ESTSNO=''" 'ESTSNO'
        End If
        strsql += ", @OTHERAMT=" & OTHERAMT & "" 'OTHERAMT
        strsql += ", @FIN_DISCOUNT=0" 'DISCOUNT
        strsql += ", @RATEID=0" 'RATEID
        strsql += ", @SETGRPID=''" 'SETGRPID
        strsql += ", @TRANFLAG=''" 'TRANFLAG
        strsql += ", @EXDUTY='" & objSoftKeys.EXDUTY & "'" 'EXDUTY
        strsql += ", @EDC1='" & Val(EDC1) & "'" 'EDC1
        strsql += ", @EDC2='" & Val(EDC2) & "'" 'EDC2
        strsql += ", @EDC3='" & Val(EDC3) & "'" 'EDC3
        strsql += ", @EDC1ID='" & objSoftKeys.EDC1ID & "'" 'EDC1ID
        strsql += ", @EDC2ID='" & objSoftKeys.EDC2ID & "'" 'EDC2ID
        strsql += ", @EDC3ID='" & objSoftKeys.EDC3ID & "'" 'EDC3ID
        strsql += ", @EDC1PER='" & objSoftKeys.EDC1PER & "'" 'EDC1PER
        strsql += ", @EDC2PER='" & objSoftKeys.EDC2PER & "'" 'EDC2PER
        strsql += ", @EDC3PER='" & objSoftKeys.EDC3PER & "'" 'EDC3PER
        strsql += ", @ORDSTATE_ID=0" 'TRANFLAG
        strsql += ", @DISC_EMPID=" & DiscAuth 'TRANFLAG
        strsql += ", @STKTYPE='" & IIf(Val(EDC1) > 0, "M", "T") & "'" 'STKTYPE
        strsql += ", @SGST='" & Val(SGST) & "'" 'SGST
        strsql += ", @CGST='" & Val(CGST) & "'" 'CGST
        strsql += ", @IGST='" & Val(IGST) & "'" 'IGST
        strsql += ", @SGSTPER='" & SGSTPER & "'" 'SGSTPER
        strsql += ", @CGSTPER='" & CGSTPER & "'" 'CGSTPER
        strsql += ", @IGSTPER='" & IGSTPER & "'" 'IGSTPER
        strsql += ", @EDSNO1='" & EDSNO1 & "'" 'EDSNO1
        strsql += ", @EDSNO2='" & EDSNO2 & "'" 'EDSNO2
        strsql += ", @EDSNO3='" & EDSNO3 & "'" 'EDSNO3
        strsql += ", @SGSTSNO='" & SGSTSNO & "'" 'SGSTSNO
        strsql += ", @CGSTSNO='" & CGSTSNO & "'" 'CGSTSNO
        strsql += ", @IGSTSNO='" & IGSTSNO & "'" 'IGSTSNO
        strsql += ", @ENTORDER='" & EntOrder & "'" 'IGSTSNO
        strsql += ", @CESS='0'" 'IGSTSNO
        strsql += ", @CESSPER='0'" 'IGSTSNO
        strsql += ", @CESSSNO=''" 'IGSTSNO
        strsql += ", @HSNCODE=''" 'HSNCODE
        strsql += ", @WT_ENTORDER=" & WT_ENTORDER 'HSNCODE
        cmd = New OleDbCommand(strsql, cn, tran) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function

    Function InsertEstIssueReceiptStone(
        ByVal tran As OleDbTransaction,
        ByVal Issno As String, ByVal TNo As Integer, ByVal billdate As Date, ByVal BatchNo As String, ByVal EstBatchNo As String _
       , ByVal tranType As String, ByVal STNCATCODE As String, ByVal stnpcs As Integer, ByVal stnnetWT As Double _
       , ByVal StnItemid As Integer, ByVal StnSubItemid As Integer, ByVal stnrate As Double, ByVal stnamount As Double _
       , ByVal CALCMODE As String, ByVal Stoneunit As String, ByVal vat As Double _
       , ByVal DISCOUNT As Double, ByVal TAGSTNPCS As Double, ByVal TAGSTNWT As Double _
       , ByVal TAGSNO As String _
       , Optional ByVal BillCostId As String = Nothing _
       , Optional ByVal VATEXM As String = "N" _
       , Optional ByVal tranNo As Integer = Nothing _
       , Optional ByVal CutId As Integer = 0 _
       , Optional ByVal ColorId As Integer = 0 _
       , Optional ByVal ClarityId As Integer = 0 _
       , Optional ByVal ShapeId As Integer = 0 _
       , Optional ByVal SetTypeId As Integer = 0 _
       , Optional ByVal Height As Decimal = 0 _
       , Optional ByVal Width As Decimal = 0 _
       , Optional ByVal SGST As Double = 0 _
       , Optional ByVal CGST As Double = 0 _
       , Optional ByVal IGST As Double = 0 _
       , Optional ByVal SGSTPER As Double = 0 _
       , Optional ByVal CGSTPER As Double = 0 _
       , Optional ByVal IGSTPER As Double = 0 _
       , Optional ByVal SGSTSNO As String = "" _
       , Optional ByVal CGSTSNO As String = "" _
       , Optional ByVal IGSTSNO As String = "" _
       , Optional ByVal CESS As Double = 0 _
       , Optional ByVal CESSPER As Double = 0 _
       , Optional ByVal CESSSNO As String = "" _
       , Optional ByVal ISMSNO As String = ""
       )
        Dim sno As String = Nothing
        If tranType = "SR" Or
           tranType = "AR" Or
           tranType = "PU" Then
            sno = GetNewSno(TranSnoType.ESTRECEIPTSTONECODE, tran)
        Else
            sno = GetNewSno(TranSnoType.ESTISSSTONECODE, tran)
        End If

        If tranType = "SR" Or tranType = "AR" Or
        tranType = "PU" Then
            strsql = " EXEC " & cnAdminDb & "..SP_INSERT_ESTRECEIPTSTONE @FROMID='" & cnCostId & "'"
        Else
            strsql = " EXEC " & cnAdminDb & "..SP_INSERT_ESTISSUESTONE @FROMID='" & cnCostId & "'"
        End If
        strsql += ", @DBNAME='" & cnStockDb & "'"
        strsql += ", @SNO='" & sno & "'" 'SNO
        strsql += " ,@ISSSNO='" & Issno & "'" 'ISSSNO
        strsql += ", @TRANNO=" & TNo & "" 'TRANNO 
        strsql += ", @TRANDATE='" & billdate & "'" 'TRANDATE
        strsql += ", @TRANTYPE='" & tranType & "'" 'TRANTYPE 
        strsql += ", @STNPCS=" & stnpcs & "" 'STNPCS
        strsql += ", @STNWT=" & stnnetWT & "" 'STNWT
        strsql += ", @STNRATE=" & stnrate & "" 'STNRATE
        strsql += ", @STNAMT=" & stnamount & "" 'STNAMT
        strsql += ", @STNITEMID=" & StnItemid & "" 'STNITEMID
        strsql += ", @STNSUBITEMID=" & StnSubItemid & "" 'STNSUBITEMID
        strsql += ", @CALCMODE='" & CALCMODE & "'" 'CALCMODE
        strsql += ", @STONEUNIT='" & Stoneunit & "'" 'STONEUNIT
        strsql += ", @STONEMODE=''" 'STONEMODE 
        strsql += ", @TRANSTATUS=''" 'TRANSTATUS
        strsql += ", @COSTID='" & BillCostId & "'" 'COSTID
        strsql += ", @COMPANYID='" & strCompanyId & "'" 'COMPANYID
        strsql += ", @BATCHNO='" & BatchNo & "'" 'BATCHNO
        strsql += ", @SYSTEMID='" & systemId & "'" 'SYSTEMID
        strsql += ", @VATEXM='" & VATEXM & "'" 'VATEXM
        strsql += ", @CATCODE='" & STNCATCODE & "'" 'CATCODE
        strsql += ", @TAX=" & vat & "" 'TAX
        strsql += ", @APPVER='" & VERSION & "'" 'APPVER
        strsql += ", @DISCOUNT=" & DISCOUNT & "" 'DISCOUNT
        strsql += ", @TAGSTNPCS=" & TAGSTNPCS & "" 'TAGSTNPCS
        strsql += ", @TAGSTNWT=" & TAGSTNWT & "" 'TAGSTNWT
        strsql += ", @TAGSNO='" & TAGSNO & "'" 'TAGSNO
        strsql += ", @CUTID=" & CutId & "" 'CUTID
        strsql += ", @COLORID=" & ColorId & "" 'COLORID
        strsql += ", @CLARITYID=" & ClarityId & "" 'CLARITYID
        strsql += ", @SHAPEID=" & ShapeId & "" 'SHAPEID
        strsql += ", @SETTYPEID=" & SetTypeId & "" 'SETTYPEID
        strsql += ", @HEIGHT=" & Height & "" 'HEIGHT
        strsql += ", @WIDTH=" & Width & "" 'WIDTH
        strsql += ", @ESTBATCHNO='" & EstBatchNo & "'" 'ESTBATCHNO
        strsql += ", @SGST='" & Val(SGST) & "'" 'SGST
        strsql += ", @CGST='" & Val(CGST) & "'" 'CGST
        strsql += ", @IGST='" & Val(IGST) & "'" 'IGST
        strsql += ", @SGSTPER='" & SGSTPER & "'" 'SGSTPER
        strsql += ", @CGSTPER='" & CGSTPER & "'" 'CGSTPER
        strsql += ", @IGSTPER='" & IGSTPER & "'" 'IGSTPER
        strsql += ", @SGSTSNO='" & SGSTSNO & "'" 'SGSTSNO
        strsql += ", @CGSTSNO='" & CGSTSNO & "'" 'CGSTSNO
        strsql += ", @IGSTSNO='" & IGSTSNO & "'" 'IGSTSNO
        strsql += ", @CESS='" & Val(IGST) & "'" 'IGST
        strsql += ", @CESSPER='" & IGSTPER & "'" 'IGSTPER
        strsql += ", @CESSSNO='" & IGSTSNO & "'" 'IGSTSNO
        strsql += ", @ISMSNO='" & ISMSNO & "'" 'MultimetalSno
        cmd = New OleDbCommand(strsql, cn, tran) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function
    Function InsertIssueReceiptStone(
    ByVal tran As OleDbTransaction,
    ByVal Issno As String, ByVal TNo As Integer, ByVal billdate As Date, ByVal BatchNo As String _
   , ByVal tranType As String, ByVal STNCATCODE As String, ByVal stnpcs As Integer, ByVal stnnetWT As Double _
   , ByVal StnItemid As Integer, ByVal StnSubItemid As Integer, ByVal stnrate As Double, ByVal stnamount As Double _
   , ByVal CALCMODE As String, ByVal Stoneunit As String, ByVal vat As Double _
   , ByVal DISCOUNT As Double, ByVal TAGSTNPCS As Double, ByVal TAGSTNWT As Double _
   , Optional ByVal BillCostId As String = Nothing _
   , Optional ByVal VATEXM As String = "N" _
   , Optional ByVal tranNo As Integer = Nothing _
   , Optional ByVal CutId As Integer = 0 _
   , Optional ByVal ColorId As Integer = 0 _
   , Optional ByVal ClarityId As Integer = 0 _
   , Optional ByVal ShapeId As Integer = 0 _
   , Optional ByVal SetTypeId As Integer = 0 _
   , Optional ByVal Height As Decimal = 0 _
   , Optional ByVal Width As Decimal = 0 _
   , Optional ByVal SGST As Double = 0 _
   , Optional ByVal CGST As Double = 0 _
   , Optional ByVal IGST As Double = 0 _
   , Optional ByVal SGSTPER As Double = 0 _
   , Optional ByVal CGSTPER As Double = 0 _
   , Optional ByVal IGSTPER As Double = 0 _
   , Optional ByVal SGSTSNO As String = "" _
   , Optional ByVal CGSTSNO As String = "" _
   , Optional ByVal IGSTSNO As String = "" _
   , Optional ByVal CESS As Double = 0 _
   , Optional ByVal CESSPER As Double = 0 _
   , Optional ByVal CESSSNO As String = "" _
   , Optional ByVal ISMSNO As String = ""
   )
        Dim sno As String = Nothing
        If tranType = "SR" Or
           tranType = "AR" Or
           tranType = "PU" Then
            sno = GetNewSno(TranSnoType.RECEIPTSTONECODE, tran)
        Else
            sno = GetNewSno(TranSnoType.ISSSTONECODE, tran)
        End If
        If vat = 0 Then
            SGST = 0 : CGST = 0 : IGST = 0
        End If
        If tranType = "SR" Or tranType = "AR" Or
        tranType = "PU" Then
            strsql = " EXEC " & cnAdminDb & "..SP_INSERT_RECEIPTSTONE @FROMID='" & cnCostId & "'"
        Else
            strsql = " EXEC " & cnAdminDb & "..SP_INSERT_ISSUESTONE @FROMID='" & cnCostId & "'"
        End If
        strsql += ", @DBNAME='" & cnStockDb & "'"
        strsql += ", @SNO='" & sno & "'" 'SNO
        strsql += " ,@ISSSNO='" & Issno & "'" 'ISSSNO
        strsql += ", @TRANNO=" & TNo & "" 'TRANNO 
        strsql += ", @TRANDATE='" & billdate & "'" 'TRANDATE
        strsql += ", @TRANTYPE='" & tranType & "'" 'TRANTYPE 
        strsql += ", @STNPCS=" & stnpcs & "" 'STNPCS
        strsql += ", @STNWT=" & stnnetWT & "" 'STNWT
        strsql += ", @STNRATE=" & stnrate & "" 'STNRATE
        strsql += ", @STNAMT=" & stnamount & "" 'STNAMT
        strsql += ", @STNITEMID=" & StnItemid & "" 'STNITEMID
        strsql += ", @STNSUBITEMID=" & StnSubItemid & "" 'STNSUBITEMID
        strsql += ", @CALCMODE='" & CALCMODE & "'" 'CALCMODE
        strsql += ", @STONEUNIT='" & Stoneunit & "'" 'STONEUNIT
        strsql += ", @STONEMODE=''" 'STONEMODE 
        strsql += ", @TRANSTATUS=''" 'TRANSTATUS
        strsql += ", @COSTID='" & BillCostId & "'" 'COSTID
        strsql += ", @COMPANYID='" & strCompanyId & "'" 'COMPANYID
        strsql += ", @BATCHNO='" & BatchNo & "'" 'BATCHNO
        strsql += ", @SYSTEMID='" & systemId & "'" 'SYSTEMID
        strsql += ", @VATEXM='" & VATEXM & "'" 'VATEXM
        strsql += ", @CATCODE='" & STNCATCODE & "'" 'CATCODE
        strsql += ", @TAX=" & vat & "" 'TAX
        strsql += ", @APPVER='" & VERSION & "'" 'APPVER
        strsql += ", @DISCOUNT=" & DISCOUNT & "" 'DISCOUNT
        strsql += ", @TAGSTNPCS=" & TAGSTNPCS & "" 'TAGSTNPCS
        strsql += ", @TAGSTNWT=" & TAGSTNWT & "" 'TAGSTNWT
        strsql += ", @CUTID=" & CutId & "" 'CUTID
        strsql += ", @COLORID=" & ColorId & "" 'COLORID
        strsql += ", @CLARITYID=" & ClarityId & "" 'CLARITYID
        strsql += ", @SHAPEID=" & ShapeId & "" 'SHAPEID
        strsql += ", @SETTYPEID=" & SetTypeId & "" 'SETTYPEID
        strsql += ", @HEIGHT=" & Height & "" 'HEIGHT
        strsql += ", @WIDTH=" & Width & "" 'WIDTH
        strsql += ", @SGST='" & Val(SGST) & "'" 'SGST
        strsql += ", @CGST='" & Val(CGST) & "'" 'CGST
        strsql += ", @IGST='" & Val(IGST) & "'" 'IGST
        strsql += ", @SGSTPER='" & SGSTPER & "'" 'SGSTPER
        strsql += ", @CGSTPER='" & CGSTPER & "'" 'CGSTPER
        strsql += ", @IGSTPER='" & IGSTPER & "'" 'IGSTPER
        strsql += ", @SGSTSNO='" & SGSTSNO & "'" 'SGSTSNO
        strsql += ", @CGSTSNO='" & CGSTSNO & "'" 'CGSTSNO
        strsql += ", @IGSTSNO='" & IGSTSNO & "'" 'IGSTSNO
        strsql += ", @CESS='" & Val(CESS) & "'" 'CESS
        strsql += ", @CESSPER='" & CESSPER & "'" 'CESSPER
        strsql += ", @CESSSNO='" & CESSSNO & "'" 'CESSSNO
        strsql += ", @ISMSNO='" & ISMSNO & "'" 'MULTIMETAL SNO
        cmd = New OleDbCommand(strsql, cn, tran) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function
    Function UpdateIssueReceiptStone(
    ByVal tran As OleDbTransaction,
    ByVal Issno As String, ByVal TNo As Integer, ByVal billdate As Date, ByVal BatchNo As String _
   , ByVal tranType As String, ByVal STNCATCODE As String, ByVal stnpcs As Integer, ByVal stnnetWT As Double _
   , ByVal StnItemid As Integer, ByVal StnSubItemid As Integer, ByVal stnrate As Double, ByVal stnamount As Double _
   , ByVal CALCMODE As String, ByVal Stoneunit As String, ByVal vat As Double _
   , ByVal DISCOUNT As Double, ByVal TAGSTNPCS As Double, ByVal TAGSTNWT As Double _
   , Optional ByVal BillCostId As String = Nothing _
   , Optional ByVal VATEXM As String = "N" _
   , Optional ByVal tranNo As Integer = Nothing _
   , Optional ByVal CutId As Integer = 0 _
   , Optional ByVal ColorId As Integer = 0 _
   , Optional ByVal ClarityId As Integer = 0 _
   , Optional ByVal ShapeId As Integer = 0 _
   , Optional ByVal SetTypeId As Integer = 0 _
   , Optional ByVal Height As Decimal = 0 _
   , Optional ByVal Width As Decimal = 0 _
   , Optional ByVal SGST As Double = 0 _
   , Optional ByVal CGST As Double = 0 _
   , Optional ByVal IGST As Double = 0 _
   , Optional ByVal SGSTPER As Double = 0 _
   , Optional ByVal CGSTPER As Double = 0 _
   , Optional ByVal IGSTPER As Double = 0 _
   , Optional ByVal SGSTSNO As String = "" _
   , Optional ByVal CGSTSNO As String = "" _
   , Optional ByVal IGSTSNO As String = "" _
   , Optional ByVal CESS As Double = 0 _
   , Optional ByVal CESSPER As Double = 0 _
   , Optional ByVal CESSSNO As String = "" _
   , Optional ByVal ISMSNO As String = "" _
   , Optional ByVal ISSSTNSNO As String = ""
   )
        Dim sno As String = Nothing
        If tranType = "SR" Or
           tranType = "AR" Or
           tranType = "PU" Then
            sno = GetNewSno(TranSnoType.RECEIPTSTONECODE, tran)
        Else
            sno = GetNewSno(TranSnoType.ISSSTONECODE, tran)
        End If
        If vat = 0 Then
            SGST = 0 : CGST = 0 : IGST = 0
        End If
        If tranType = "SR" Or tranType = "AR" Or
        tranType = "PU" Then
            strsql = " EXEC " & cnAdminDb & "..SP_INSERT_RECEIPTSTONE @FROMID='" & cnCostId & "'"
        Else
            strsql = " EXEC " & cnAdminDb & "..SP_UPDATE_ISSUESTONE @FROMID='" & cnCostId & "'"
        End If
        strsql += ", @DBNAME='" & cnStockDb & "'"
        strsql += ", @SNO='" & sno & "'" 'SNO
        strsql += " ,@ISSSNO='" & Issno & "'" 'ISSSNO
        strsql += ", @TRANNO=" & TNo & "" 'TRANNO 
        strsql += ", @TRANDATE='" & billdate & "'" 'TRANDATE
        strsql += ", @TRANTYPE='" & tranType & "'" 'TRANTYPE 
        strsql += ", @STNPCS=" & stnpcs & "" 'STNPCS
        strsql += ", @STNWT=" & stnnetWT & "" 'STNWT
        strsql += ", @STNRATE=" & stnrate & "" 'STNRATE
        strsql += ", @STNAMT=" & stnamount & "" 'STNAMT
        strsql += ", @STNITEMID=" & StnItemid & "" 'STNITEMID
        strsql += ", @STNSUBITEMID=" & StnSubItemid & "" 'STNSUBITEMID
        strsql += ", @CALCMODE='" & CALCMODE & "'" 'CALCMODE
        strsql += ", @STONEUNIT='" & Stoneunit & "'" 'STONEUNIT
        strsql += ", @STONEMODE=''" 'STONEMODE 
        strsql += ", @TRANSTATUS=''" 'TRANSTATUS
        strsql += ", @COSTID='" & BillCostId & "'" 'COSTID
        strsql += ", @COMPANYID='" & strCompanyId & "'" 'COMPANYID
        strsql += ", @BATCHNO='" & BatchNo & "'" 'BATCHNO
        strsql += ", @SYSTEMID='" & systemId & "'" 'SYSTEMID
        strsql += ", @VATEXM='" & VATEXM & "'" 'VATEXM
        strsql += ", @CATCODE='" & STNCATCODE & "'" 'CATCODE
        strsql += ", @TAX=" & vat & "" 'TAX
        strsql += ", @APPVER='" & VERSION & "'" 'APPVER
        strsql += ", @DISCOUNT=" & DISCOUNT & "" 'DISCOUNT
        strsql += ", @TAGSTNPCS=" & TAGSTNPCS & "" 'TAGSTNPCS
        strsql += ", @TAGSTNWT=" & TAGSTNWT & "" 'TAGSTNWT
        strsql += ", @CUTID=" & CutId & "" 'CUTID
        strsql += ", @COLORID=" & ColorId & "" 'COLORID
        strsql += ", @CLARITYID=" & ClarityId & "" 'CLARITYID
        strsql += ", @SHAPEID=" & ShapeId & "" 'SHAPEID
        strsql += ", @SETTYPEID=" & SetTypeId & "" 'SETTYPEID
        strsql += ", @HEIGHT=" & Height & "" 'HEIGHT
        strsql += ", @WIDTH=" & Width & "" 'WIDTH
        strsql += ", @SGST='" & Val(SGST) & "'" 'SGST
        strsql += ", @CGST='" & Val(CGST) & "'" 'CGST
        strsql += ", @IGST='" & Val(IGST) & "'" 'IGST
        strsql += ", @SGSTPER='" & SGSTPER & "'" 'SGSTPER
        strsql += ", @CGSTPER='" & CGSTPER & "'" 'CGSTPER
        strsql += ", @IGSTPER='" & IGSTPER & "'" 'IGSTPER
        strsql += ", @SGSTSNO='" & SGSTSNO & "'" 'SGSTSNO
        strsql += ", @CGSTSNO='" & CGSTSNO & "'" 'CGSTSNO
        strsql += ", @IGSTSNO='" & IGSTSNO & "'" 'IGSTSNO
        strsql += ", @CESS='" & Val(CESS) & "'" 'CESS
        strsql += ", @CESSPER='" & CESSPER & "'" 'CESSPER
        strsql += ", @CESSSNO='" & CESSSNO & "'" 'CESSSNO
        strsql += ", @ISMSNO='" & ISMSNO & "'" 'MULTIMETAL SNO
        strsql += " ,@ISSSTNSNO='" & ISSSTNSNO & "'"
        cmd = New OleDbCommand(strsql, cn, tran) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function
    Function InsertAdjTran(
    ByVal tran As OleDbTransaction,
    ByVal TNo As Integer, ByVal billdate As Date, ByVal BatchNo As String _
   , ByVal Recpay As String, ByVal Cash As Double _
   , ByVal Mobile As String, ByVal Pan As String, ByVal Accode As String _
   , ByVal CostId As String)
        Dim sno As String = Nothing
        sno = GetNewSno(TranSnoType.ADJTRANCODE, tran)
        strsql = " EXEC " & cnAdminDb & "..SP_INSERT_ADJTRAN "
        strsql += "  @FROMID='" & cnCostId & "'"
        strsql += ", @DBNAME='" & cnStockDb & "'"
        strsql += ", @SNO='" & sno & "'" 'SNO
        strsql += ", @TRANNO=" & TNo & "" 'TRANNO 
        strsql += ", @TRANDATE='" & billdate & "'" 'TRANDATE
        strsql += ", @RECPAY='" & Recpay & "'" 'RECPAY 
        strsql += ", @CASH='" & Cash & "'" 'CASH 
        strsql += ", @MOBILE='" & Mobile & "'" 'MOBILE 
        strsql += ", @PAN='" & Pan & "'" 'PAN 
        strsql += ", @ACCODE='" & Accode & "'" 'ACCODE 
        strsql += ", @COSTID='" & CostId & "'" 'COSTID
        strsql += ", @COMPANYID='" & strCompanyId & "'" 'COMPANYID
        strsql += ", @BATCHNO='" & BatchNo & "'" 'BATCHNO
        cmd = New OleDbCommand(strsql, cn, tran) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function
    Function UpdateAdjTran(
    ByVal tran As OleDbTransaction,
    ByVal TNo As Integer, ByVal billdate As Date, ByVal BatchNo As String _
   , ByVal Recpay As String, ByVal Cash As Double _
   , ByVal Mobile As String, ByVal Pan As String, ByVal Accode As String _
   , ByVal CostId As String)
        Dim sno As String = Nothing
        sno = GetNewSno(TranSnoType.ADJTRANCODE, tran)
        strsql = " EXEC " & cnAdminDb & "..SP_UPDATE_ADJTRAN "
        strsql += "  @FROMID='" & cnCostId & "'"
        strsql += ", @DBNAME='" & cnStockDb & "'"
        strsql += ", @SNO='" & sno & "'" 'SNO
        strsql += ", @TRANNO=" & TNo & "" 'TRANNO 
        strsql += ", @TRANDATE='" & billdate & "'" 'TRANDATE
        strsql += ", @RECPAY='" & Recpay & "'" 'RECPAY 
        strsql += ", @CASH='" & Cash & "'" 'CASH 
        strsql += ", @MOBILE='" & Mobile & "'" 'MOBILE 
        strsql += ", @PAN='" & Pan & "'" 'PAN 
        strsql += ", @ACCODE='" & Accode & "'" 'ACCODE 
        strsql += ", @COSTID='" & CostId & "'" 'COSTID
        strsql += ", @COMPANYID='" & strCompanyId & "'" 'COMPANYID
        strsql += ", @BATCHNO='" & BatchNo & "'" 'BATCHNO
        cmd = New OleDbCommand(strsql, cn, tran) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function

#Region "OLDoUTS"
    'Function InsertOutstandingDetails( _
    'ByVal tran As OleDbTransaction, _
    'ByVal Sno As String, ByVal TNO As String, ByVal billdate As Date, ByVal BatchNo As String, _
    'ByVal tType As String, ByVal RunNo As String, ByVal Amount As Double, _
    'ByVal RecPay As String, ByVal Paymode As String, _
    'Optional ByVal GrsWt As Double = 0, Optional ByVal NetWt As Double = 0, _
    'Optional ByVal CatCode As String = Nothing, Optional ByVal Rate As Double = Nothing, Optional ByVal Value As Double = Nothing, _
    'Optional ByVal refNo As String = Nothing, Optional ByVal refDate As String = Nothing, Optional ByVal purity As Decimal = Nothing, _
    'Optional ByVal proId As Integer = Nothing, Optional ByVal dueDate As String = Nothing, Optional ByVal Remark1 As String = Nothing, _
    'Optional ByVal Remark2 As String = Nothing, Optional ByVal Accode As String = Nothing, Optional ByVal Flag As String = Nothing, _
    'Optional ByVal EmpId As Integer = Nothing, Optional ByVal sasrEmpId As Integer = Nothing, Optional ByVal PuEmpId As Integer = Nothing, _
    'Optional ByVal PureWt As Double = Nothing, Optional ByVal BillCashCounterId As String = Nothing, _
    ' Optional ByVal VATEXM As String = "N", Optional ByVal BillCostId As String = Nothing _
    '    )

    '    strsql = " EXEC " & cnAdminDb & "..SP_INSERT_OUTSTANDING @FROMID='" & cnCostId & "'"
    '    strsql += ", @DBNAME='" & cnAdminDb & "'"
    '    strsql += ", @SNO='" & Sno & "'" 'SNO
    '    strsql += ", @TRANNO=" & TNO & "" 'TRANNO 
    '    strsql += ", @TRANDATE='" & billdate & "'" 'TRANDATE
    '    strsql += ", @TRANTYPE='" & tType & "'" 'TRANTYPE
    '    strsql += ", @RUNNO='" & RunNo & "'" 'RUNNO
    '    strsql += ", @AMOUNT=" & Amount & "" 'AMOUNT
    '    strsql += ", @GRSWT=" & GrsWt & "" 'GRSWT
    '    strsql += ", @NETWT=" & NetWt & "" 'NETWT
    '    strsql += ", @PUREWT=" & PureWt & "" 'PUREWT
    '    strsql += ", @RECPAY='" & RecPay & "'" 'RECPAY
    '    If refNo <> Nothing Then
    '        strsql += ", @REFNO='" & refNo & "'" 'REFNO
    '    Else
    '        strsql += ", @REFNO=''" 'REFNO
    '    End If

    '    If refDate <> Nothing Then
    '        strsql += ", @REFDATE='" & refDate & "'" 'REFDATE
    '    Else
    '        strsql += ", @REFDATE=''" 'REFDATE
    '    End If
    '    If EmpId <> 0 Then
    '        strsql += ", @EMPID=" & EmpId & "" 'EMPID
    '    ElseIf sasrEmpId <> 0 Then
    '        strsql += ", @EMPID=" & sasrEmpId & "" 'EMPID
    '    ElseIf PuEmpId <> 0 Then
    '        strsql += ", @EMPID=" & PuEmpId & "" 'EMPID
    '    Else
    '        strsql += ", @EMPID=0" 'EMPID
    '    End If
    '    strsql += ", @TRANSTATUS=''" 'TRANSTATUS
    '    strsql += ", @PURITY=" & purity & "" 'PURITY
    '    strsql += ", @CATCODE='" & CatCode & "'" 'CATCODE
    '    strsql += ", @BATCHNO='" & BatchNo & "'" 'BATCHNO
    '    strsql += ", @USERID=" & userId & "" 'USERID
    '    strsql += ", @UPDATED='" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
    '    strsql += ", @UPTIME='" & Date.Now.ToLongTimeString & "'" 'UPTIME
    '    strsql += ", @SYSTEMID='" & systemId & "'" 'SYSTEMID
    '    strsql += ", @RATE=" & Rate & "" 'RATE
    '    strsql += ", @VALUE=" & Value & "" 'VALUE
    '    strsql += ", @CASHID='" & BillCashCounterId & "'" 'CASHID
    '    strsql += ", @VATEXM='" & VATEXM & "'" 'VATEXM
    '    strsql += ", @REMARK1='" & Remark1 & "'" 'REMARK1
    '    strsql += ", @REMARK2='" & Remark2 & "'" 'REMARK1
    '    strsql += ", @ACCODE='" & Accode & "'" 'ACCODE
    '    strsql += ", @CTRANCODE=" & proId & "" 'CTRANCODE
    '    If dueDate <> Nothing Then
    '        strsql += ", @DUEDATE='" & dueDate & "'" 'DUEDATE
    '    Else
    '        strsql += ", @DUEDATE=''" 'DUEDATE
    '    End If
    '    strsql += ", @APPVER='" & VERSION & "'" 'APPVER
    '    strsql += ", @COMPANYID='" & strCompanyId & "'" 'COMPANYID
    '    strsql += ", @COSTID='" & BillCostId & "'" 'COSTID
    '    strsql += ", @FROMFLAG='P'" 'FROMFLAG
    '    strsql += ", @FLAG='" & Flag & "'" 'FLAG FOR ORDER ADVANCE REPAY
    '    strsql += ", @PAYMODE='" & Paymode & "'"

    '    cmd = New OleDbCommand(strsql, cn, tran) : cmd.CommandTimeout = 1000
    '    cmd.ExecuteNonQuery()
    'End Function
#End Region

    Function InsertOutstandingDetails( _
       ByVal tran As OleDbTransaction, ByVal TableName As String, _
       ByVal Sno As String, ByVal TNO As String, ByVal billdate As Date, ByVal BatchNo As String, _
       ByVal tType As String, ByVal RunNo As String, ByVal Amount As Double, _
       ByVal RecPay As String, ByVal Paymode As String, _
       Optional ByVal GrsWt As Double = 0, Optional ByVal NetWt As Double = 0, _
       Optional ByVal CatCode As String = Nothing, Optional ByVal Rate As Double = Nothing, Optional ByVal Value As Double = Nothing, _
       Optional ByVal refNo As String = Nothing, Optional ByVal refDate As String = Nothing, Optional ByVal purity As Decimal = Nothing, _
       Optional ByVal CTranCode As Integer = Nothing, Optional ByVal dueDate As String = Nothing, Optional ByVal Remark1 As String = Nothing, _
       Optional ByVal Remark2 As String = Nothing, Optional ByVal Accode As String = Nothing, Optional ByVal Flag As String = Nothing, _
       Optional ByVal EmpId As Integer = Nothing, Optional ByVal sasrEmpId As Integer = Nothing, Optional ByVal PuEmpId As Integer = Nothing, _
       Optional ByVal PureWt As Double = Nothing, Optional ByVal BillCashCounterId As String = Nothing, _
       Optional ByVal VATEXM As String = "N", Optional ByVal BillCostId As String = Nothing _
       , Optional ByVal FromFlag As String = "P")

        strsql = " EXEC " & cnAdminDb & "..SP_INSERT_OUTSTANDING @FROMID='" & cnCostId & "'"
        strsql += ", @DBNAME='" & cnAdminDb & "'"
        strsql += ", @TABLENAME='" & TableName & "'"
        strsql += ", @SNO='" & Sno & "'" 'SNO
        strsql += ", @TRANNO=" & TNO & "" 'TRANNO 
        strsql += ", @TRANDATE='" & billdate & "'" 'TRANDATE
        strsql += ", @TRANTYPE='" & tType & "'" 'TRANTYPE
        strsql += ", @RUNNO='" & RunNo & "'" 'RUNNO
        strsql += ", @AMOUNT=" & Amount & "" 'AMOUNT
        strsql += ", @GRSWT=" & GrsWt & "" 'GRSWT
        strsql += ", @NETWT=" & NetWt & "" 'NETWT
        strsql += ", @PUREWT=" & PureWt & "" 'PUREWT
        strsql += ", @RECPAY='" & RecPay & "'" 'RECPAY
        If refNo <> Nothing Then
            strsql += ", @REFNO='" & refNo & "'" 'REFNO
        Else
            strsql += ", @REFNO=''" 'REFNO
        End If

        If refDate <> Nothing Then
            strsql += ", @REFDATE='" & refDate & "'" 'REFDATE
        Else
            strsql += ", @REFDATE=''" 'REFDATE
        End If
        If EmpId <> 0 Then
            strsql += ", @EMPID=" & EmpId & "" 'EMPID
        ElseIf sasrEmpId <> 0 Then
            strsql += ", @EMPID=" & sasrEmpId & "" 'EMPID
        ElseIf PuEmpId <> 0 Then
            strsql += ", @EMPID=" & PuEmpId & "" 'EMPID
        Else
            strsql += ", @EMPID=0" 'EMPID
        End If
        strsql += ", @TRANSTATUS=''" 'TRANSTATUS
        strsql += ", @PURITY=" & purity & "" 'PURITY
        strsql += ", @CATCODE='" & CatCode & "'" 'CATCODE
        strsql += ", @BATCHNO='" & BatchNo & "'" 'BATCHNO
        strsql += ", @USERID=" & userId & "" 'USERID
        strsql += ", @UPDATED='" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strsql += ", @UPTIME='" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strsql += ", @SYSTEMID='" & systemId & "'" 'SYSTEMID
        strsql += ", @RATE=" & Rate & "" 'RATE
        strsql += ", @VALUE=" & Value & "" 'VALUE
        strsql += ", @CASHID='" & BillCashCounterId & "'" 'CASHID
        strsql += ", @VATEXM='" & VATEXM & "'" 'VATEXM
        strsql += ", @REMARK1='" & Remark1 & "'" 'REMARK1
        strsql += ", @REMARK2='" & Remark2 & "'" 'REMARK1
        strsql += ", @ACCODE='" & Accode & "'" 'ACCODE
        strsql += ", @CTRANCODE=" & CTranCode & "" 'CTRANCODE
        If dueDate <> Nothing Then
            strsql += ", @DUEDATE='" & dueDate & "'" 'DUEDATE
        Else
            strsql += ", @DUEDATE=''" 'DUEDATE
        End If
        strsql += ", @APPVER='" & VERSION & "'" 'APPVER
        strsql += ", @COMPANYID='" & strCompanyId & "'" 'COMPANYID
        strsql += ", @COSTID='" & BillCostId & "'" 'COSTID
        strsql += ", @FROMFLAG='" & FromFlag & "'" 'FROMFLAG
        strsql += ", @FLAG='" & Flag & "'" 'FLAG FOR ORDER ADVANCE REPAY
        strsql += ", @PAYMODE='" & Paymode & "'"

        cmd = New OleDbCommand(strsql, cn, tran) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function


    Function InsertNonTag( _
   ByVal tran As OleDbTransaction, _
    ByVal nonTagSno As String, ByVal ITEMID As Integer, ByVal SUBITEMID As Integer, ByVal billdate As Date, ByVal BatchNo As String _
   , ByVal PCS As Integer, ByVal GRSWT As Double, ByVal NETWT As Double _
   , ByVal LESSWT As Double, ByVal Rate As Double, ByVal RecPay As String, ByVal Isstype As String, ByVal PURRATE As Double _
   , ByVal Tagno As String, ByVal ItemCtrId As Integer, ByVal DesignerId As Integer, ByVal ITEMTYPEID As Integer _
   , ByVal Approval As String, ByVal FLAG As String, ByVal RECSNO As String _
   , Optional ByVal BillCostId As String = Nothing _
   , Optional ByVal VATEXM As String = "N" _
   , Optional ByVal tranNo As Integer = Nothing, Optional ByVal TCostid As String = "")

        strsql = " EXEC " & cnAdminDb & "..SP_INSERT_ITEMNONTAG @FROMID='" & cnCostId & "'"
        strsql += ", @DBNAME='" & cnAdminDb & "'"
        strsql += ", @SNO='" & nonTagSno & "'" 'SNO
        strsql += ", @ITEMID=" & ITEMID & "" 'ItemId
        strsql += ", @SUBITEMID=" & SUBITEMID & "" 'SubItemId
        strsql += ", @COMPANYID='" & strCompanyId & "'" 'CompanyId
        strsql += ", @RECDATE='" & billdate & "'" ''Recdate
        strsql += ", @PCS=" & PCS & "" 'Pcs
        strsql += ", @GRSWT=" & GRSWT & "" 'GrsWt
        strsql += ", @LESSWT=" & LESSWT & "" 'LessWt
        strsql += ", @NETWT=" & NETWT & "" 'NetWt
        strsql += ", @FINRATE=" & Rate & "" 'FineRate
        strsql += ", @ISSTYPE='" & Isstype & "'" 'Isstype
        strsql += ", @RECISS='" & RecPay & "'" 'RecIss
        strsql += ", @POSTED=''" 'Posted
        strsql += ", @LOTNO=0" 'Lotno
        strsql += ", @PACKETNO='" & Tagno & "'" 'Packetno
        strsql += ", @DREFNO=0" 'DRefno
        strsql += ", @ITEMCTRID=" & ItemCtrId & "" 'ItemCtrId
        strsql += ", @ORDREPNO=''" 'OrdRepNo
        strsql += ", @ORSNO=''" 'ORSNO
        strsql += ", @NARRATION=''" 'Narration
        strsql += ", @PURWASTAGE=0" 'PurWastage
        strsql += ", @PURRATE=" & PURRATE & "" 'PurRate 
        strsql += ", @PURMC=0" 'PurMC
        strsql += ", @RATE=" & Rate & "" 'Rate
        strsql += ", @COSTID='" & BillCostId & "'" 'COSTID
        strsql += ", @CTGRM=''" 'CtGrm
        strsql += ", @DESIGNERID=" & DesignerId & "" 'DesignerId
        strsql += ", @VATEXM='" & VATEXM & "'" 'VATEXM
        strsql += ", @ITEMTYPEID=" & ITEMTYPEID & "" 'ItemTypeId
        strsql += ", @CARRYFLAG=''" 'Carryflag
        strsql += ", @REASON=0" 'Reason
        strsql += ", @BATCHNO='" & BatchNo & "'" 'BatchNo
        strsql += ", @CANCEL=''" 'Cancel
        strsql += ", @USERID='" & userId & "'" 'UserId
        strsql += ", @UPDATED='" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Updated
        strsql += ", @UPTIME='" & Date.Now.ToLongTimeString & "'" 'Uptime
        strsql += ", @SYSTEMID='" & systemId & "'" 'SystemId
        strsql += ", @APPVER='" & VERSION & "'" 'APPVER
        strsql += ", @APPROVAL='" & Approval & "'" 'APPROVAL
        strsql += ", @FLAG='" & FLAG & "'" 'FLAG 'COUNTER/BACKOFFICE
        strsql += ", @RECSNO='" & RECSNO & "'" 'RECSNO
        strsql += ", @TCOSTID='" & TCostid & "'" 'TCOSTID
        cmd = New OleDbCommand(strsql, cn, tran) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

    End Function

    Function InsertNonTagStone( _
   ByVal tran As OleDbTransaction, _
    ByVal Sno As String, ByVal stnItemId As Integer, ByVal stnSubItemId As Integer, ByVal billdate As Date, ByVal BatchNo As String _
   , ByVal tagsno As String, ByVal PCS As Integer, ByVal STNWT As Double, ByVal AMOUNT As Double, ByVal Rate As Double _
   , ByVal RecPay As String, ByVal ITEM As String, ByVal SUBITEM As String _
   , ByVal CALC As String, ByVal UNIT As String _
   , Optional ByVal BillCostId As String = Nothing _
   , Optional ByVal VATEXM As String = "N" _
   , Optional ByVal tranNo As Integer = Nothing)

        strsql = " EXEC " & cnAdminDb & "..SP_INSERT_ITEMNONTAGSTONE @FROMID='" & cnCostId & "'"
        strsql += ", @DBNAME='" & cnAdminDb & "'"
        strsql += ", @SNO='" & Sno & "'" ''SNO
        strsql += ", @RECISS='" & RecPay & "'" ' RECISS
        strsql += ", @TAGSNO='" & tagsno & "'" 'TAGSNO
        strsql += ", @ITEMID='" & stnItemId & "'" 'ITEMID
        strsql += ", @COMPANYID='" & strCompanyId & "'" 'COMPANYID
        strsql += ", @STNITEMID=" & stnItemId & "" 'STNITEMID
        strsql += ", @STNSUBITEMID=" & stnSubItemId & "" 'STNSUBITEMID
        strsql += ", @STNPCS=" & PCS & "" 'STNPCS
        strsql += ", @STNWT=" & STNWT & "" 'STNWT
        strsql += ", @STNRATE=" & Rate & "" 'STNRATE
        strsql += ", @STNAMT=" & AMOUNT & "" 'STNAMT
        If stnSubItemId <> 0 Then 'DESCRIP
            strsql += ", @DESCRIP='" & SUBITEM & "'"
        Else
            strsql += ", @DESCRIP='" & ITEM & "'"
        End If
        strsql += ", @RECDATE='" & billdate & "'" 'RECDATE
        strsql += ", @PURRATE=0" 'PURRATE
        strsql += ", @PURAMT=0" 'PURAMT
        strsql += ", @CALCMODE='" & CALC & "'" 'CALCMODE
        strsql += ", @MINRATE=0" 'MINRATE
        strsql += ", @SIZECODE=0" 'SIZECODE
        strsql += ", @STONEUNIT='" & UNIT & "'" 'STONEUNIT
        strsql += ", @ISSDATE=''" 'ISSDATE
        strsql += ", @VATEXM=''" 'VATEXM
        strsql += ", @CARRYFLAG=''" 'CARRYFLAG
        strsql += ", @COSTID='" & BillCostId & "'" 'COSTID
        strsql += ", @SYSTEMID='" & systemId & "'" 'SYSTEMID
        strsql += ", @APPVER='" & VERSION & "'" 'APPVER

        cmd = New OleDbCommand(strsql, cn, tran) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

    End Function


End Class
