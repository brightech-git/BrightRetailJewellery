Imports System.Data.OleDb
Public Class frmOrder
    Public Enum OrderType
        OrderEntry = 0
        OrderEdit = 1
        OrderUpdate = 2
    End Enum
    Dim QuickOrder As Boolean = False
    Public UpdateFileImagePath As String = ""
    Dim dtSoftKeys As DataTable = GetAdmindbSoftValueAll()
    Dim defaultPic As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "TAGCATALOGPATH", Application.ExecutablePath)
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim Addressflag As Boolean = False
    Dim searchSender As Control = Nothing
    Dim LockOrderDate As Boolean = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "LOCK_ORDERDATE", "Y") = "Y", True, False)
    Dim Sms_Masking As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "SMS_MASKING", "GIRITECH")
    Dim Sms_Send As Boolean = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "SMS_SEND", "N") = "Y", True, False)
    Dim Sms_Name As Boolean = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "SMS_NAME", "N") = "Y", True, False)
    Dim Sms_Msg As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "SMS_MSG_ORDBOOK", "Thanks for Ordered")
    Dim GLEDGERCODE As String
    Dim RndGrossAmt As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "ROUNDOFF-GROSS", "N")
    Dim RndFinalAmt As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "ROUNDOFF-FINAL", "N")
    Dim DiscBTorAT As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "ORDDISC_VAT_ADJ", "A")
    Dim RndVat As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "ROUNDOFF-VAT", "N")
    Dim WastageRound As Integer = Val(GetAdmindbSoftValuefromDt(dtSoftKeys, "ROUNDOFF-WASTAGE", "3"))
    Dim McRound As Integer = Val(GetAdmindbSoftValuefromDt(dtSoftKeys, "ROUNDOFF-MC", "2"))
    Dim ManualSize As Boolean = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "MANUALITEMSIZE", "N") = "Y", True, False)
    Dim Rm_Item_Dly As Boolean = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "RM_ITEM_DLY", "N") = "Y", True, False)
    Dim REORDERCHK As Boolean = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "REORDER_CHKQTY", "N") = "Y", True, False)
    Dim McWithWast As Boolean = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "MCWITHWASTAGE", "N") = "Y", True, False)
    Dim LockSalesEst As Boolean = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "LOCK_ORD_EST", "N") = "Y", True, False)
    Dim DRSMAINTAIN As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "DRSMAINTAIN", "N")
    Dim ORD_DISC_LOCK As Boolean = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "ORD_DISC_LOCK", "N") = "Y", True, False)
    Dim chitMainDb As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "CHITDBPREFIX", "") + "SAVINGS"
    Public OrdType As OrderType = OrderType.OrderEntry
    Dim OrdRepManDueDate As Double = Val(GetAdmindbSoftValuefromDt(dtSoftKeys, "ORDREPMANDUEDATE", "1").ToString)
    Public Ispurbillauto As Boolean = True
    Dim dtGridOrder As New DataTable
    'Dim dtGridStone As New DataTable
    Dim dtStoneDetails As New DataTable

    Dim dtGridSample As New DataTable
    Dim dtSampleDetails As New DataTable

    Public OrderUpdSno As String
    Public Batchno As String
    Public OrderNo As String
    Dim TranNo As Integer
    Public BillCostId As String
    Public BillDate As Date
    Public BillCashCounterId As String
    Dim CASHID As String
    Dim VATEXM As String = "Y"
    Public MANBILLNO As Boolean = False
    Dim objManualBill As frmManualBillNoGen
    Dim ObjTagsearch As TagSearch


    ''OrderDet
    Dim calcType As String = Nothing
    Public subItemName As String = Nothing
    Public itemTypeName As String = Nothing
    Public Itemid As Integer = 0
    Public subItemid As Integer = 0
    Public isStone As Boolean = False
    Public picPath As String = Nothing
    Dim CartId As Integer = Nothing
    Dim Oitemname As String = Nothing

    Dim objCreditCard As New frmCreditCardAdj
    Dim objChitCard As New frmChitAdj
    Dim objAdvance As New frmAdvanceAdj(BillCostId)
    Dim objCheaque As New frmChequeAdj
    Public objStone As New frmStoneDia
    Dim objAddressDia As New frmAddressDia(True, True)
    Dim defalutDestination As String
    Dim objOrderAdvance As frmOrderAdvance
    Dim dtoredt As New DataTable
    Dim orderestimate As Boolean = False
    Dim Readytag As String = ""
    Dim READYITEMBOOKING As Boolean = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "READYITEMBOOKING", "N") = "Y", True, False)
    Dim VALUECALC_GRNETOPT As Boolean = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "VALUECALC_GRNETOPT", "N") = "Y", True, False)
    Dim _MCONGRSNET As Boolean = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "MC_ON_GRSNET", "Y") = "Y", True, False)
    Dim _WASTONGRSNET As Boolean = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "WAST_ON_GRSNET", "Y") = "Y", True, False)
    Dim WASTAGECEILING_FLOOR As String = GetAdmindbSoftValue("WASTRND_CEIL_FLR", "")
    Dim _MCCALCON_ITEM_GRS As Boolean = False
    Dim _WASTCALCON_ITEM_GRS As Boolean = False
    Public MANBILLNO_FORREADYITEM As Boolean = False
    Dim SMS_MSG_ORDBOOK As String = objGPack.GetSqlValue("SELECT ISNULL(TEMPLATE_DESC,'') AS TEMPLATE_DESC FROM " & cnAdminDb & "..SMSTEMPLATE WHERE TEMPLATE_NAME='SMS_MSG_ORDBOOK' AND ISNULL(ACTIVE,'Y')<>'N'", "TEMPLATE_DESC").ToString
    Dim ORDDATE_EDIT As Boolean = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "ORDDATE_EDIT", "Y") = "Y", True, False)
    Dim ORD_OFF_VRATE As Boolean = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "ORD_OFF_VRATE", "N") = "Y", True, False)
    Dim ORD_SCHEMEBOOK As Boolean = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "ORD_SCHEMEBOOK", "N") = "Y", True, False)
    Dim GstPer As Decimal
    Dim GstRecCode As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "GSTACCODE", "")
    Dim GstRecAcc() As String
    Dim GSTADVCALC_INCL As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "GSTADVCALC", "N")
    Dim SCode As String
    Dim CCode As String
    Dim ICode As String
    Dim InterStateBill As Boolean
    Dim _Vbc As Boolean = False
    Dim RndGst As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "ROUNDOFF-GST", "N")
    Public objAddtionalDetails As New frmOrAdditionalDetails
    Dim ORADDITIONALDETAIL As Boolean = IIf(GetAdmindbSoftValue("ORADDITIONALDETAIL", "N") = "Y", True, False)
    Public objSoftKeys As New SoftKeys
    Dim dtOrderAdditionalDetails As New DataTable
    Dim ORDER_LOCK_PAYMODE As String = GetAdmindbSoftValue("ORDER_LOCK_PAYMODE", "").ToString
    Dim CashReceivedValidate As String = GetAdmindbSoftValue("CASHRECEIVEDVALIDATE", "D")
    Dim MetalBasedStone As Boolean = IIf(GetAdmindbSoftValue("MULTIMETALBASEDSTONE", "N") = "Y", True, False)

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        objGPack.Validator_Object(objCreditCard)
        objGPack.Validator_Object(objAdvance)
        objGPack.Validator_Object(objCheaque)

        CASHID = GetAdmindbSoftValuefromDt(dtSoftKeys, "CASH", "CASH")
        ' Add any initialization after the InitializeComponent() call.
        Dim pt As Point = New Point((Me.Width - grpDescription.Width) / 2, (Me.Height - grpDescription.Height) / 2)
        Me.Controls.Add(grpDescription)
        grpDescription.Location = pt
        grpDescription.BringToFront()
        grpDescription.Visible = False

        Me.Controls.Add(pnlOrderExtraDet)
        pnlOrderExtraDet.Visible = False
        pnlOrderExtraDet.BringToFront()

        tabAddress.Controls.Add(grpImage)
        grpImage.Location = grpAdj.Location
        grpImage.Size = grpAdj.Size
        AutoImageSizer(My.Resources.no_photo, picImage, PictureBoxSizeMode.StretchImage)
        'picImage.SizeMode = PictureBoxSizeMode.StretchImage
        grpImage.BringToFront()
        grpImage.Visible = False

        ''Order
        With dtGridOrder
            .Columns.Add("ITEMID", GetType(String))
            .Columns.Add("PARTICULAR", GetType(String))
            .Columns.Add("PCS", GetType(Integer))
            .Columns.Add("GRSWT", GetType(Decimal))
            .Columns.Add("NETWT", GetType(Decimal))
            .Columns.Add("SIZENO", GetType(String))
            .Columns.Add("RATE", GetType(Double))
            .Columns.Add("WASTAGE", GetType(Decimal))
            .Columns.Add("MC", GetType(Double))
            .Columns.Add("COMM", GetType(Double))
            .Columns.Add("DIAPCSWT", GetType(String))
            .Columns.Add("OTHERAMT", GetType(Double))
            .Columns.Add("GROSSAMT", GetType(Double))
            .Columns.Add("VAT", GetType(Double))
            .Columns.Add("AMOUNT", GetType(Double))
            .Columns.Add("SAMPLEANDIMAGE", GetType(String))
            Dim col As New DataColumn("KEYNO", GetType(Integer))
            col.AutoIncrement = True
            col.AutoIncrementSeed = 1
            col.AutoIncrementStep = 1
            .Columns.Add(col)
            .Columns.Add("SAMPLE", GetType(String))
            .Columns.Add("IMAGE", GetType(String))
            .Columns.Add("ENTFLAG", GetType(String))
            .Columns.Add("GRSNET", GetType(String))
            .Columns.Add("LESSWT", GetType(Decimal))
            .Columns.Add("STONEAMT", GetType(Double))
            .Columns.Add("MISCAMT", GetType(Double))
            .Columns.Add("VATPER", GetType(Double))
            .Columns.Add("WASTAGEPER", GetType(Decimal))
            .Columns.Add("MCGRM", GetType(Double))
            .Columns.Add("COMMPER", GetType(Double))
            .Columns.Add("DESCRIPTION", GetType(String))
            .Columns.Add("SUBITEMNAME", GetType(String))
            .Columns.Add("SIZENAME", GetType(String))
            .Columns.Add("PICTPATH", GetType(String))
            .Columns.Add("ITEMTYPE", GetType(String))
            .Columns.Add("STYLENO", GetType(String))
            .Columns.Add("ADDCARTID", GetType(Integer))
            .Columns.Add("ITEMNAME", GetType(String))
            .Columns.Add("SUBITEMID", GetType(Integer))
            .Columns.Add("TAGNO", GetType(String))
            .Columns.Add("ESTNO", GetType(Integer))
            .Columns.Add("METALID", GetType(String))
            .Columns.Add("DISCOUNT", GetType(Double))
        End With
        dtGridOrder.AcceptChanges()
        gridOrder.DataSource = dtGridOrder
        gridOrder.ColumnHeadersVisible = False
        FormatGridColumns(gridOrder)
        ClearDtGrid(dtGridOrder)
        StyleGridOrder(gridOrder)
        ''Order Total
        Dim dtGridOrderTotal As New DataTable
        dtGridOrderTotal = dtGridOrder.Copy
        dtGridOrderTotal.Rows.Clear()
        dtGridOrderTotal.Rows.Add()
        dtGridOrderTotal.Rows(0).Item(0) = "TOTAL"
        gridOrderTotal.ColumnHeadersVisible = False
        gridOrderTotal.Font = New Font("VERDANA", 8, FontStyle.Bold)
        gridOrderTotal.DataSource = dtGridOrderTotal
        For Each col As DataGridViewColumn In gridOrder.Columns
            With gridOrderTotal.Columns(col.Name)
                .Visible = col.Visible
                .Width = col.Width
                .DefaultCellStyle = col.DefaultCellStyle
            End With
        Next
        FormatGridColumns(gridOrderTotal)
        StyleGridOrder(gridOrderTotal)
        CalcGridOrderTotal()

        ''Stone Group
        cmbStUnit.Items.Add("C")
        cmbStUnit.Items.Add("G")
        cmbStUnit.Text = "C"
        cmbStCalc.Items.Add("W")
        cmbStCalc.Items.Add("P")
        cmbStCalc.Text = "W"

        ''Stone
        With dtStoneDetails.Columns
            .Add("KEYNO", GetType(Integer))
            .Add("TRANTYPE", GetType(String))
            .Add("ITEM", GetType(String))
            .Add("SUBITEM", GetType(String))
            .Add("PCS", GetType(Int32))
            .Add("WEIGHT", GetType(Decimal))
            .Add("UNIT", GetType(String))
            .Add("CALC", GetType(String))
            .Add("RATE", GetType(Double))
            .Add("AMOUNT", GetType(Double))
            .Add("METALID", GetType(String))
            .Add("DISCOUNT", GetType(Double))
            .Add("TAGSTNPCS", GetType(Integer))
            .Add("TAGSTNWT", GetType(Decimal))
            .Add("TAGSNO", GetType(String))
            .Add("R_VAT", GetType(Decimal))
            .Add("ISSSNO", GetType(String))
            .Add("CUTID", GetType(Integer))
            .Add("COLORID", GetType(Integer))
            .Add("CLARITYID", GetType(Integer))
            .Add("SHAPEID", GetType(Integer))
            .Add("SETTYPEID", GetType(Integer))
            .Add("HEIGHT", GetType(Decimal))
            .Add("WIDTH", GetType(Decimal))
            .Add("SGST", GetType(Decimal))
            .Add("CGST", GetType(Decimal))
            .Add("IGST", GetType(Decimal))
            .Add("CESS", GetType(Decimal))
            If MetalBasedStone Then
                .Add("TAGMSNO", GetType(String))
            End If
            .Add("STNGRPID", GetType(Integer))
            .Add("ORGAMOUNT", GetType(Double))
        End With

        ''Sample/Attachment
        With dtSampleDetails
            .Columns.Add("KEYNO", GetType(Integer))
            .Columns.Add("TYPE", GetType(String))
            .Columns.Add("FROM", GetType(String))
            .Columns.Add("ITEM", GetType(String))
            .Columns.Add("DESCRIPTION", GetType(String))
            .Columns.Add("TAGNO", GetType(String))
            .Columns.Add("PCS", GetType(Integer))
            .Columns.Add("GRSWT", GetType(Decimal))
            .Columns.Add("NETWT", GetType(Decimal))
        End With
        'ORADDITIONAL DETAILS
        With dtOrderAdditionalDetails.Columns
            .Add("KEYNO", GetType(Integer))
            .Add("TYPENAME", GetType(String))
            .Add("VALUENAME", GetType(String))
        End With
        dtGridSample = dtSampleDetails.Copy
        dtGridSample.Rows.Clear()
        dtGridSample.AcceptChanges()
        gridSample.DataSource = dtGridSample
        gridSample.ColumnHeadersVisible = False
        FormatGridColumns(gridSample)
        StyleGridSample()
        cmbSamType.Items.Add("SAMPLE")
        cmbSamType.Items.Add("ATTACHMENT")
        cmbSamFrom.Items.Add("CUSTOMER")
        cmbSamFrom.Items.Add("COMPANY")
    End Sub
    Private Function GetControlLocation(ByVal ctrl As Control, ByRef pt As Point) As Point
        If TypeOf ctrl Is Form Then
            Return pt
        Else
            pt += ctrl.Location
            Return GetControlLocation(ctrl.Parent, pt)
        End If
        Return pt
    End Function


    Private Sub ShowOrderExtraDetails(ByVal sender As Control)
        Dim pt As New Point((pnlContainer_OWN.Width - pnlOrderExtraDet.Width) / 2, (pnlContainer_OWN.Height - pnlOrderExtraDet.Height) / 2)
        pnlOrderExtraDet.Location = pt
        pnlOrderExtraDet.Visible = True
        pnlOrderExtraDet.BringToFront()
        pnlOrderExtraDet.BackColor = pnlContainer_OWN.BackColor
        If mcasvaper() = True Then
            lblMcPerGm.Text = "Mc Percent"
        Else
            lblMcPerGm.Text = "Mc/Grm"
        End If
        txtOWastagePer_Per.Focus()
        txtOWastagePer_Per.SelectAll()
        searchSender = sender
    End Sub

    Private Sub ShowOrderAdditionalDetails()
        If ORADDITIONALDETAIL = False Then
            Exit Sub
        End If
        If objAddtionalDetails.Visible Then Exit Sub
        objAddtionalDetails.BackColor = pnlContainer_OWN.BackColor
        objAddtionalDetails.StartPosition = FormStartPosition.CenterScreen
        objAddtionalDetails.MaximizeBox = False
        objAddtionalDetails.cmbType.Select()
        objAddtionalDetails.ShowDialog()
        Me.SelectNextControl(txtOGrsWt_WET, True, True, True, True)
    End Sub

    Private Sub CalcStoneAmount()
        Dim amt As Double = Nothing
        If cmbStCalc.Text = "P" Then
            amt = Val(txtStRate_AMT.Text) * Val(txtStPcs_NUM.Text)
        Else
            amt = Val(txtStRate_AMT.Text) * Val(txtStWeight_WET.Text)

        End If
        txtStAmount_AMT.Text = IIf(amt <> 0, Format(amt, "0.00"), "")
    End Sub

    Function chkreorderstk() As Boolean
        Dim chkstkqty As Decimal = 0
        Dim chkmaxqty As Decimal = 0
        Dim stkdt As New DataTable
        strSql += " SELECT sum(pcs) CLPCS from " & cnAdminDb & "..ITEMTAG T"
        strSql += " WHERE ITEMID = " & Val(Itemid)
        If Val(subItemid) <> 0 Then strSql += " AND SUBITEMID = " & Val(subItemid)
        strSql += " AND RECDATE <= '" & dtpOrderDate.Value & "' AND ISSDATE IS NULL"
        strSql += " AND ISNULL(COSTID,'') = '" & BillCostId & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(stkdt)
        If stkdt.Rows.Count > 0 Then chkstkqty = Val("" & stkdt.Rows(0).Item(0).ToString)
        Dim rstkdt As New DataTable
        strSql = " SELECT sum(piece) Maxpcs from " & cnAdminDb & "..STKREORDER"
        strSql += " WHERE ITEMID = " & Val(Itemid)
        If Val(subItemid) <> 0 Then strSql += " AND SUBITEMID = " & Val(subItemid)
        strSql += " AND ISNULL(COSTID,'') = '" & BillCostId & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(rstkdt)
        If rstkdt.Rows.Count > 0 Then chkmaxqty = Val("" & rstkdt.Rows(0).Item(0).ToString)
        If chkmaxqty <= chkstkqty + Val(txtOPcs_NUM.Text) Then Return False Else Return True

    End Function

    Private Sub CalcMc(Optional ByVal FetchVa As Boolean = True)
        Dim mc As Double = Nothing
        Dim vatype As String = objGPack.GetSqlValue("SELECT VALUEADDEDTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(Itemid) & " ", , , )
        If FetchVa Then
            Dim dtMctab As New DataTable
            strSql = " DECLARE @WT FLOAT"
            strSql += " SET @WT = " & Val(txtONetWt_WET.Text) & ""
            strSql += " SELECT MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC"
            strSql += " ,MINWASTPER,MINMCGRM,MINWAST,MINMC FROM " & cnAdminDb & "..WMCTABLE "
            strSql += " WHERE 1=1 "
            If vatype <> "P" Then
                strSql += " AND ITEMID = " & Val(Itemid)
                strSql += " AND SUBITEMID = " & Val(subItemid)
            End If
            strSql += " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
            strSql += " AND ISNULL(COSTID,'') = '" & BillCostId & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtMctab)

            If dtMctab.Rows.Count > 0 Then
                With dtMctab.Rows(0)
                    If Val("" & .Item("MAXMC").ToString) <> 0 Then
                        mc = Val("" & .Item("MAXMC").ToString)
                    Else
                        txtOMcPerGrm_AMT.Text = Val("" & .Item("MAXMCGRM").ToString)
                    End If
                End With
            End If
        End If
        If mc = 0 Then
            ''mc = (Val(txtONetWt_WET.Text) + IIf(McWithWast = True, Val(txtOWastage_WET.Text), 0)) * Val(txtOMcPerGrm_AMT.Text)
            If mcasvaper() = True Then
                If Not Val(txtOMcPerGrm_AMT.Text) > 0 Then Exit Sub
                mc = (Val(txtONetWt_WET.Text) + IIf(McWithWast = True, Val(txtOWastage_WET.Text), 0)) * Val(txtORate_AMT.Text)
                mc = (mc * Val(txtOMcPerGrm_AMT.Text)) / 100
            Else

                mc = (Val(txtONetWt_WET.Text) + IIf(McWithWast = True, Val(txtOWastage_WET.Text), 0)) * Val(txtOMcPerGrm_AMT.Text)
            End If
        End If
        mc = Math.Round(mc, McRound)
        txtOMc_AMT.Text = IIf(mc <> 0, Format(mc, "0.00"), Nothing)
    End Sub


    Private Function mcasvaper() As Boolean
        Dim mcasva As String
        mcasva = UCase(objGPack.GetSqlValue("Select MCASVAPER FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID =" & Val(Itemid) & "", , "N"))
        If Val(subItemid) > 0 Then
            mcasva = UCase(objGPack.GetSqlValue("SELECT MCASVAPER FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = " & Val(Itemid) & " AND SUBITEMID = '" & Val(subItemid) & "'"))
        End If
        If mcasva = "Y" Then Return True Else Return False
    End Function

    Private Sub CalcWastage(Optional ByVal FetchVa As Boolean = True)
        Dim wast As Double = Nothing
        Dim vatype As String = objGPack.GetSqlValue("SELECT VALUEADDEDTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(Itemid) & " ", , , )
        Dim mtablcode As String = objGPack.GetSqlValue("SELECT TABLECODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(Itemid) & " ", , , )
        If FetchVa Then
            Dim dtMctab As New DataTable
            strSql = " DECLARE @WT FLOAT"
            strSql += " SET @WT = " & Val(txtONetWt_WET.Text) & ""
            strSql += " SELECT MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC"
            strSql += " ,MINWASTPER,MINMCGRM,MINWAST,MINMC FROM " & cnAdminDb & "..WMCTABLE "
            strSql += " WHERE 1=1 "
            If vatype = "I" Then
                strSql += " AND ITEMID = " & Val(Itemid)
                strSql += " AND SUBITEMID = " & Val(subItemid)
            Else
                strSql += " AND ITEMID = 0"
                strSql += " AND SUBITEMID =0 "
                strSql += " and tablecode='" & mtablcode.ToString & "' and isnull(tablecode,'') <> '' "
            End If
            strSql += " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
            strSql += " AND ISNULL(COSTID,'') = '" & BillCostId & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtMctab)

            If dtMctab.Rows.Count > 0 Then
                With dtMctab.Rows(0)
                    If Val("" & .Item("MAXWAST").ToString) <> 0 Then
                        wast = Val("" & .Item("MAXWAST").ToString)
                    Else
                        txtOWastagePer_Per.Text = Val("" & .Item("MAXWASTPER").ToString)
                    End If
                End With
            End If
        End If

        If wast = 0 Then
            wast = Val(txtONetWt_WET.Text) * (Val(txtOWastagePer_Per.Text) / 100)
        End If
        wast = Math.Round(wast, WastageRound)
        txtOWastage_WET.Text = IIf(wast <> 0, Format(wast, "0.000"), Nothing)
    End Sub

    Private Sub CalcCommision()
        Dim cmm As Double = Nothing
        cmm = Val(txtONetWt_WET.Text) * Val(txtOCommGrm_AMT.Text)
        txtOCommision_AMT.Text = IIf(cmm <> 0, Format(cmm, "0.00"), Nothing)
    End Sub


    Private Sub CalcAmount()
        Dim amt As Double = Nothing
        amt = Val(txtOGrossAmount_AMT.Text) + Val(txtOVat_AMT.Text)
        txtOAmount_AMT.Text = IIf(amt <> 0, Format(amt, "0.00"), Nothing)
    End Sub


    Private Sub CalcGrossAmount()
        Dim amt As Double = Nothing
        Dim wt As Double = Val(txtONetWt_WET.Text)
        Select Case calcType
            Case "M"
                amt = Val(txtORate_AMT.Text)
            Case "R"
                amt = Val(txtOPcs_NUM.Text) * Val(txtORate_AMT.Text)
            Case Else
                amt = (wt + Val(txtOWastage_WET.Text)) * Val(txtORate_AMT.Text) + Val(txtOMc_AMT.Text) _
                + Val(txtOOtherAmt_AMT.Text) + Val(txtOCommision_AMT.Text)
        End Select
        amt = CalcRoundoffAmt(amt, RndGrossAmt)
        txtOGrossAmount_AMT.Text = IIf(amt <> 0, Format(amt, "0.00"), Nothing)
    End Sub

    Private Sub CalcVat()
        If GetAdmindbSoftValue("VBC_" & strCompanyId, "N").ToString = "Y" Then
            Exit Sub
        End If
        strSql = " SELECT SALESTAX,S_SGSTTAX,S_CGSTTAX,S_IGSTTAX FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = "
        strSql += " (SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtOItem.Text & "')"
        Dim vatPer As Double = Nothing
        Dim dr As DataRow = GetSqlRow(strSql, cn)
        If Not dr Is Nothing Then
            If GST Then
                vatPer = Val(dr("S_SGSTTAX").ToString) + Val(dr("S_CGSTTAX").ToString)
            Else
                vatPer = dr("SALESTAX").ToString
            End If
        End If
        Dim vat As Double = Val(txtOGrossAmount_AMT.Text) * (vatPer / 100)
        vat = CalcRoundoffAmt(vat, RndVat)
        txtOVat_AMT.Text = IIf(vat <> 0, Format(vat, "0.00"), Nothing)
    End Sub

    Private Sub CalcGridOrderTotal()
        Dim pcs As Integer = Nothing
        Dim grsWt As Decimal = Nothing
        Dim netWt As Decimal = Nothing
        Dim wastage As Decimal = Nothing
        Dim mc As Double = Nothing
        Dim mcCharge As Double = Nothing
        Dim grossAmt As Double = Nothing
        Dim vat As Double = Nothing
        Dim amount As Double = Nothing
        dtGridOrder.AcceptChanges()
        For Each dgvRow As DataGridViewRow In gridOrder.Rows
            If dgvRow.Cells("ENTFLAG").Value.ToString = "" Then Continue For
            pcs += Val(dgvRow.Cells("PCS").Value.ToString)
            grsWt += Val(dgvRow.Cells("GRSWT").Value.ToString)
            netWt += Val(dgvRow.Cells("NETWT").Value.ToString)
            wastage += Val(dgvRow.Cells("WASTAGE").Value.ToString)
            mc += Val(dgvRow.Cells("MC").Value.ToString)
            'mcCharge +=
            grossAmt += Val(dgvRow.Cells("GROSSAMT").Value.ToString)
            vat += Val(dgvRow.Cells("VAT").Value.ToString)
            amount += Val(dgvRow.Cells("AMOUNT").Value.ToString)
        Next
        With gridOrderTotal.Rows(0)
            .Cells("PCS").Value = IIf(pcs <> 0, pcs, DBNull.Value)
            .Cells("GRSWT").Value = IIf(grsWt <> 0, grsWt, DBNull.Value)
            .Cells("NETWT").Value = IIf(netWt <> 0, netWt, DBNull.Value)
            .Cells("WASTAGE").Value = IIf(wastage <> 0, wastage, DBNull.Value)
            .Cells("MC").Value = IIf(mc <> 0, mc, DBNull.Value)
            .Cells("GROSSAMT").Value = IIf(grossAmt <> 0, grossAmt, DBNull.Value)
            .Cells("VAT").Value = IIf(vat <> 0, vat, DBNull.Value)
            .Cells("AMOUNT").Value = IIf(amount <> 0, amount, DBNull.Value)
        End With
        CalcBalanceAmount()
    End Sub

    Private Sub CalcStoneWtAmount()
        Dim diaCaratWt As Double = 0
        Dim diaGramWt As Double = 0
        Dim diaPcs As Integer = 0
        Dim diaAmt As Double = 0

        Dim preCaratWt As Double = 0
        Dim preGramWt As Double = 0
        Dim prePcs As Integer = 0
        Dim preAmt As Double = 0

        Dim stoCaratWt As Double = 0
        Dim stoGramWt As Double = 0
        Dim stoPcs As Integer = 0
        Dim stoAmt As Double = 0

        For cnt As Integer = 0 To gridStone.RowCount - 1
            With gridStone.Rows(cnt)
                Select Case .Cells("METALID").Value.ToString
                    Case "D"
                        diaPcs += Val(.Cells("PCS").Value.ToString)
                        diaAmt += Val(.Cells("AMOUNT").Value.ToString)
                    Case "S"
                        stoPcs += Val(.Cells("PCS").Value.ToString)
                        stoAmt += Val(.Cells("AMOUNT").Value.ToString)
                    Case "P"
                        prePcs += Val(.Cells("PCS").Value.ToString)
                        preAmt += Val(.Cells("AMOUNT").Value.ToString)
                End Select
                Select Case .Cells("UNIT").Value.ToString
                    Case "G"
                        If .Cells("METALID").Value.ToString = "S" Then
                            stoGramWt += Val(.Cells("WEIGHT").Value.ToString)
                        ElseIf .Cells("METALID").Value.ToString = "P" Then
                            preGramWt += Val(.Cells("WEIGHT").Value.ToString)
                        ElseIf .Cells("METALID").Value.ToString = "D" Then
                            diaGramWt += Val(.Cells("WEIGHT").Value.ToString)
                        End If
                    Case "C"
                        If .Cells("METALID").Value.ToString = "S" Then
                            stoCaratWt += Val(.Cells("WEIGHT").Value.ToString)
                        ElseIf .Cells("METALID").Value.ToString = "P" Then
                            preCaratWt += Val(.Cells("WEIGHT").Value.ToString)
                        ElseIf .Cells("METALID").Value.ToString = "D" Then
                            diaCaratWt += Val(.Cells("WEIGHT").Value.ToString)
                        End If
                End Select
            End With
        Next

        Dim lessWt As Double = (diaCaratWt + stoCaratWt + preCaratWt) / 5 + (diaGramWt + stoGramWt + preGramWt)
        Dim stnAmt As Double = diaAmt + stoAmt + preAmt
        If gridStoneTotal.RowCount > 0 Then
            gridStoneTotal.Rows(0).Cells("WEIGHT").Value = IIf(lessWt <> 0, lessWt, DBNull.Value)
            gridStoneTotal.Rows(0).Cells("AMOUNT").Value = IIf(stnAmt <> 0, stnAmt, DBNull.Value)
        End If
        Dim ntWt As Double = Val(txtOGrsWt_WET.Text) - lessWt
        txtONetWt_WET.Text = IIf(ntWt <> 0, Format(ntWt, "0.000"), Nothing)
        txtOOtherAmt_AMT.Text = IIf(stnAmt <> 0, Format(stnAmt, "0.00"), Nothing)
    End Sub

    Private Sub ClearDtGrid(ByVal dt As DataTable) ''Only For gridOrder
        dt.Columns("KeyNo").AutoIncrementSeed = 1
        dt.Rows.Clear()
        For i As Integer = 1 To 1000
            dt.Rows.Add()
        Next
        dt.AcceptChanges()
    End Sub

    Private Sub StyleGridSample()
        With gridSample
            .Columns("KEYNO").Visible = False
            .Columns("TYPE").Width = cmbSamType.Width + 1
            .Columns("FROM").Width = cmbSamFrom.Width + 1
            .Columns("ITEM").Width = txtSamItem.Width + 1
            .Columns("DESCRIPTION").Width = txtSamDescription.Width + 1
            .Columns("TAGNO").Width = txtSamTagNo.Width + 1
            .Columns("PCS").Width = txtSamPcs_NUM.Width + 1
            .Columns("GRSWT").Width = txtSamGrsWt_WET.Width + 1
            .Columns("NETWT").Width = txtSamNetWt_WET.Width + 1
        End With
    End Sub

    Private Sub StyleGridStone(ByVal gridStone As DataGridView)
        With gridStone
            .Columns("KEYNO").Visible = False
            .Columns("METALID").Visible = False
            .Columns("ITEM").Width = txtStItem.Width + 1
            .Columns("SUBITEM").Width = txtStSubItem.Width + 1
            .Columns("PCS").Width = txtStPcs_NUM.Width + 1
            .Columns("UNIT").Width = cmbStUnit.Width + 1
            .Columns("CALC").Width = cmbStCalc.Width + 1
            .Columns("WEIGHT").Width = txtStWeight_WET.Width + 1
            .Columns("RATE").Width = txtStRate_AMT.Width + 1
            .Columns("AMOUNT").Width = txtStAmount_AMT.Width + 1
            .Columns("DISCOUNT").Visible = False
        End With
    End Sub

    Private Sub StyleGridOrder(ByVal grid As DataGridView)
        With grid
            .Columns("ITEMID").Width = txtOItemId.Width + 1
            .Columns("PARTICULAR").Width = txtOParticular.Width + 1
            .Columns("PCS").Width = txtOPcs_NUM.Width + 1
            .Columns("GRSWT").Width = txtOGrsWt_WET.Width + 1
            .Columns("NETWT").Width = txtONetWt_WET.Width + 1
            .Columns("SIZENO").Width = txtOSize.Width + 1
            .Columns("RATE").Width = txtORate_AMT.Width + 1
            .Columns("WASTAGE").Width = txtOWastage_WET.Width + 1
            .Columns("MC").Width = txtOMc_AMT.Width + 1
            .Columns("COMM").Width = txtOCommision_AMT.Width + 1
            .Columns("DIAPCSWT").Width = txtODiaPcsWt.Width + 1
            .Columns("OTHERAMT").Width = txtOOtherAmt_AMT.Width + 1
            .Columns("GROSSAMT").Width = txtOGrossAmount_AMT.Width + 1
            .Columns("VAT").Width = txtOVat_AMT.Width + 1
            .Columns("AMOUNT").Width = txtOAmount_AMT.Width + 1
            .Columns("SAMPLEANDIMAGE").Width = chkOSample.Width + chkOImage.Width
            .Columns("SAMPLE").Width = chkOSample.Width + 1
            .Columns("IMAGE").Width = chkOImage.Width + 1
            For cnt As Integer = 16 To gridOrder.ColumnCount - 1
                .Columns(cnt).Visible = False
            Next
            .Columns("SAMPLEANDIMAGE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("SAMPLE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("IMAGE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        End With
    End Sub

    Private Sub frmOrder_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If pnlOrderExtraDet.Visible Then
                If Not searchSender Is Nothing Then
                    searchSender.Select()
                End If
                pnlOrderExtraDet.Visible = False
                Exit Sub
            End If
            If grpDescription.Visible Then
                If txtODescription.Text = "" Then
                    GoTo nextGroup
                End If
                grpDescription.Visible = False
                Me.SelectNextControl(txtOParticular, True, True, True, True)
                Exit Sub
            End If
nextGroup:
            If grpImage.Visible And btnBrowse.Visible Then
                LoadOrderDetail()
                tabOtherOptions.SelectedTab = tabAddress
                'txtOParticular.Focus()
                txtOItemId.Focus()
                grpImage.Visible = False
            ElseIf objGPack.IsActive(tabStone) Then
                tabOtherOptions.SelectedTab = tabAddress
                Me.SelectNextControl(txtOGrsWt_WET, True, True, True, True)
            ElseIf objGPack.IsActive(grpSample) Then
                tabOtherOptions.SelectedTab = tabAddress
                Me.SelectNextControl(chkOSample, True, True, True, True)
            ElseIf txtOItemId.Focused Then
                If GetEntFlag() Then
                    'If _IsWholeSaleType Then
                    '    txtAdjCash_AMT.Focus()
                    'Else
                    '    ShowAddressDia()
                    'End If
                    txtAdjCash_AMT.Focus()
                End If
            End If
        ElseIf e.Control = True And e.KeyCode = Keys.M Then
            chkGenMultiRows.Checked = Not chkGenMultiRows.Checked
        End If
    End Sub

    Private Function ShowAddressDia() As Boolean
        If objAddressDia.Visible Then Exit Function
        objAddressDia.BackColor = pnlContainer_OWN.BackColor
        objAddressDia.StartPosition = FormStartPosition.CenterScreen
        'objAddressDia.StartPosition = FormStartPosition.Manual
        'objAddressDia.Location = New Point(75, 181)
        objAddressDia.MaximizeBox = False
        objAddressDia.grpAddress.BackgroundColor = grpHeader.BackgroundColor
        'objAddressDia.dtpAddressDueDate.Select()
        objAddressDia._DUEAMOUNT = 0
        objAddressDia.InterBill = InterStateBill
        'objAddressDia._IS_USERLEVELPWD = False
        objAddressDia.dtBalance.DataSource = Nothing
        'If Mid(DRSMAINTAIN, 1, 1) <> "N" Then objAddressDia._DRSMAINTAIN = DRSMAINTAIN

        objAddressDia.txtMobile.Select()
        If _IsWholeSaleType Then
            If objAddressDia.ShowDialog() = Windows.Forms.DialogResult.OK Then
                Return True
            End If
        Else
            If objAddressDia.ShowDialog() = Windows.Forms.DialogResult.OK Then
                If grpAdj.Enabled Then
                    txtAdjCash_AMT.Focus()
                Else
                    btnSave.Select()
                End If
            End If
        End If
    End Function


    Private Sub frmOrder_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtSalesMan_NUM.Focused Then Exit Sub
            If txtOParticular.Focused Then Exit Sub
            If txtOItemId.Focused Then Exit Sub
            If txtODescription.Focused Then Exit Sub
            If txtOGrsWt_WET.Focused Then Exit Sub
            If chkOSample.Focused Then Exit Sub
            If chkOImage.Focused Then Exit Sub
            ''orextra det
            If txtOCommGrm_AMT.Focused Then Exit Sub
            ''stone
            If txtStItem.Focused Then Exit Sub
            If txtStAmount_AMT.Focused Then Exit Sub
            ''SAMPLE
            If txtSamItem.Focused Then Exit Sub
            If txtSamTagNo.Focused Then Exit Sub
            If txtSamNetWt_WET.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub FurtherAdvanceInitialize()
        'pnlContainer_OWN.Size = New Size(1024, 274)
        grpHeader.Location = New Point(8, -4)
        grpHeader.Size = New Size(1006, 74)
        tabOtherOptions.Location = New Point(1, 57)
        tabOtherOptions.Size = New Size(1021, 213)
        grpOrderDetails.Visible = False
        pnlFurtherAdvAddress.Visible = True
    End Sub



    Private Sub frmOrder_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If LockOrderDate Then
            lblOrderDate.Visible = False
            dtpOrderDate.Visible = False
        End If
        If GST Then
            lblVat.Text = lblVat.Text.Replace("Vat", "GST")
            GstPer = Val(objGPack.GetSqlValue("SELECT STAX FROM " & cnAdminDb & "..TAXMAST WHERE TAXCODE='GT'", , "0").ToString)
            If GstRecCode.Contains(",") Then
                GstRecAcc = GstRecCode.Split(",")
                If GstRecAcc.Length <> 3 Then
                    MsgBox("GST Advance Receive Account not set Properly.", MsgBoxStyle.Information)
                Else
                    SCode = GstRecAcc(0).ToString
                    CCode = GstRecAcc(1).ToString
                    ICode = GstRecAcc(2).ToString
                End If
            End If
        End If
        If ORD_OFF_VRATE Then rbtVariableRate.Visible = False
        If ORD_SCHEMEBOOK Then
            rbtSchemeOrder.Visible = True
            lblOrdRate.Visible = False
        Else
            rbtSchemeOrder.Visible = False
            lblOrdRate.Visible = True
        End If
        If Not defaultPic.EndsWith("\") Then defaultPic += "\"
        If _CounterWiseCashMaintanance Then
            CASHID = objGPack.GetSqlValue("SELECT CASHACCODE FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHID = '" & BillCashCounterId & "' AND ISNULL(CASHACCODE,'') <> ''", , GetAdmindbSoftValue("CASH", "CASH"))
        End If
        Dim dt As New DateTimePicker
        dtpOrderDate.Value = BillDate
        dtpDueDate.MinimumDate = BillDate
        dtpDueDate.MaximumDate = dt.MaxDate
        dtpRemDate.MinimumDate = BillDate
        dtpRemDate.MaximumDate = dt.MaxDate



        pnlOrderRate.BackColor = Color.Transparent
        pnlOrderType.BackColor = Color.Transparent
        txtAdjDiscount_AMT.ReadOnly = ORD_DISC_LOCK
        Style5ToolStripMenuItem_Click(Me, New EventArgs)
        pnlContainer_OWN.BorderStyle = BorderStyle.Fixed3D
        pnlContainer_OWN.Location = New Point((ScreenWid - 10 - pnlContainer_OWN.Width) / 2, ((ScreenHit - 136) - pnlContainer_OWN.Height) / 2)

        defalutDestination = GetAdmindbSoftValue("OR_PICPATH")
        If defalutDestination = "" Then defalutDestination = GetAdmindbSoftValue("PICPATH")
        If Not defalutDestination.EndsWith("\") And defalutDestination <> "" Then defalutDestination += "\"

        tabOtherOptions.Region = New Region(New RectangleF(tabAddress.Left, tabAddress.Top, tabAddress.Width, tabAddress.Height))
        btnNew_Click(Me, New EventArgs)
        QuickOrder = IIf(GetAdmindbSoftValue("QORDERBOOK", "N") = "N", False, True)
        If LockSalesEst Then
            Label2.Visible = False
        End If
        If QuickOrder Then
            LoadQkOrderDetails()
        End If
        If OrdType = OrderType.OrderEdit Or OrdType = OrderType.OrderUpdate Then
            'Me.StartPosition = FormStartPosition.Manual
            'Me.Location = New Point(0, 300)
            LoadDefaultValues()
        End If

        If GSTADVCALC_INCL = "E" Then
            GSTADVCALC_INCL = "I"
        End If
        If GetAdmindbSoftValue("VBC_" & strCompanyId, "N").ToString = "Y" Then
            _Vbc = True
        End If
        If ORDER_LOCK_PAYMODE <> "" Then
            If ORDER_LOCK_PAYMODE.Contains("AW") Then
                txtAdjAdvanceWt.ReadOnly = True
                AdvanceWeightToolStripMenuItem.ShortcutKeys = Keys.None
            End If
            If ORDER_LOCK_PAYMODE.Contains("AA") Then
                txtAdjAdvance_AMT.ReadOnly = True
                AdvanceToolStripMenuItem.ShortcutKeys = Keys.None
            End If
            If ORDER_LOCK_PAYMODE.Contains("AC") Then
                txtAdjChitCard_AMT.ReadOnly = True
                ChitCardToolStripMenuItem.ShortcutKeys = Keys.None
            End If
            If ORDER_LOCK_PAYMODE.Contains("CH") Then
                txtAdjCheque_AMT.ReadOnly = True
                ChequeToolStripMenuItem.ShortcutKeys = Keys.None
            End If
            If ORDER_LOCK_PAYMODE.Contains("CC") Then
                txtAdjCreditCard_AMT.ReadOnly = True
                CreditCardToolStripMenuItem.ShortcutKeys = Keys.None
            End If
            If ORDER_LOCK_PAYMODE.Contains("DI") Then
                txtAdjDiscount_AMT.ReadOnly = True
                DiscountToolStripMenuItem.ShortcutKeys = Keys.None
            End If
            If ORDER_LOCK_PAYMODE.Contains("CA") Then
                txtAdjCash_AMT.ReadOnly = True
                CashToolStripMenuItem.ShortcutKeys = Keys.None
            End If
        End If
    End Sub

    Private Sub LoadQkOrderDetails()
        Dim ItemId As Integer = Val(GetAdmindbSoftValue("QORDERITEM", "0"))
        If ItemId = 0 Then
            MsgBox("Quick Order Book Itemd Id doesnot Set in Softcontrol" + vbCrLf + "Please set value to [QORDERITEM]", MsgBoxStyle.Information)
            Me.Close()
            Exit Sub
        End If
        Dim ItemName As String = objGPack.GetSqlValue("SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & ItemId & "")
        Dim rate As Double = Val(GetRate(BillDate, objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & ItemName & "'")))
        For i As Integer = 0 To dtGridOrder.Rows.Count - 1
            With dtGridOrder.Rows(i)
                If .Item("ENTFLAG").ToString <> "Y" Then
                    .Item("PARTICULAR") = ItemName
                    .Item("ITEMNAME") = ItemName
                    '.Item("PCS") = 1
                    '.Item("GRSWT") = 0.01
                    '.Item("NETWT") = 0.01
                    .Item("RATE") = IIf(rate <> 0, Format(rate, "0.00"), DBNull.Value)
                    '.Item("WASTAGE") = IIf(Val(txtOWastage_WET.Text) > 0, txtOWastage_WET.Text, DBNull.Value)
                    '.Item("MC") = IIf(Val(txtOMc_AMT.Text) > 0, txtOMc_AMT.Text, DBNull.Value)
                    '.Item("COMM") = IIf(Val(txtOCommision_AMT.Text) <> 0, txtOCommision_AMT.Text, DBNull.Value)
                    '.Item("DIAPCSWT") = txtODiaPcsWt.Text
                    '.Item("OTHERAMT") = IIf(Val(txtOOtherAmt_AMT.Text) > 0, txtOOtherAmt_AMT.Text, DBNull.Value)
                    '.Item("GROSSAMT") = IIf(Val(txtOGrossAmount_AMT.Text) > 0, txtOGrossAmount_AMT.Text, DBNull.Value)
                    '.Item("VAT") = IIf(Val(txtOVat_AMT.Text) > 0, txtOVat_AMT.Text, DBNull.Value)
                    '.Item("AMOUNT") = IIf(Val(txtOAmount_AMT.Text) > 0, txtOAmount_AMT.Text, DBNull.Value)
                    '.Item("SAMPLE") = IIf(chkOSample.Checked, "Y", "N")
                    '.Item("IMAGE") = IIf(chkOImage.Checked, "Y", "N")
                    '.Item("SAMPLEANDIMAGE") = IIf(chkOSample.Checked, "Y", "N") & IIf(chkOImage.Checked, "Y", "N")
                    .Item("ENTFLAG") = "Y"
                    '.Item("SUBITEMNAME") = subItemName
                    .Item("DESCRIPTION") = ItemName
                    '.Item("COMMPER") = Val(txtOCommGrm_AMT.Text)
                    '.Item("WASTAGEPER") = Val(txtOWastagePer_Per.Text)
                    '.Item("MCGRM") = Val(txtOMcPerGrm_AMT.Text)
                    '.Item("STONEAMT") = Val(objStone.gridStoneTotal.Rows(0).Cells("AMOUNT").Value.ToString)
                    '.Item("LESSWT") = Val(txtOGrsWt_WET.Text) - Val(txtONetWt_WET.Text)
                    '.Item("PICTPATH") = picPath
                    '.Item("ITEMTYPE") = itemTypeName
                    '.Item("STYLENO") = txtOStyleNo.Text
                    '.Item("ADDCARTID") = CartId
                    gridOrder.CurrentCell = gridOrder.Rows(i).Cells("PARTICULAR")
                    dtGridOrder.Rows.Add()
                    Exit For
                End If
            End With
        Next
        dtGridOrder.AcceptChanges()
        CalcGridOrderTotal()
        grpOrderDetails.Enabled = False
    End Sub

    Private Sub LoadDefaultValues()
        strSql = vbCrLf + " SELECT TOP 1 ORRATE,ORMODE,CONVERT(VARCHAR,DUEDATE,103)DUEDATE,CONVERT(VARCHAR,REMDATE,103)REMDATE,ORDATE,EMPID"
        strSql += " ,DATEDIFF(dd,remdate,duedate)REMDAYS,DATEDIFF(dd,ORDATE,duedate)DUEDAYS"
        strSql += vbCrLf + " FROM " & cnadmindb & "..ORMAST"
        strSql += vbCrLf + " WHERE ORNO = '" & OrderNo & "' AND BATCHNO = '" & Batchno & "'"
        Dim dtGrid As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Order info not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        With dtGrid.Rows(0)
            If .Item("ORMODE").ToString = "C" Then
                rbtCustomerOrderType.Checked = True
            ElseIf .Item("ORMODE").ToString = "O" Then
                rbtCompanyOrderType.Checked = True
            Else
                rbtSchemeOrder.Checked = True
            End If
            If .Item("ORRATE").ToString = "D" Then
                rbtDeliveryRate.Checked = True
            ElseIf .Item("ORRATE").ToString = "V" Then
                rbtVariableRate.Checked = True
            Else
                rbtCurrentRate.Checked = True
            End If
            txtDueDays_NUM.Text = Val(.Item("DUEDAYS").ToString)
            txtRemDays_NUM.Text = Val(.Item("REMDAYS").ToString)

            'dtpDueDate.Text = .Item("DUEDATE").ToString
            'dtpRemDate.Text = .Item("REMDATE").ToString
        End With
        If Not ORDDATE_EDIT Then grpHeader.Enabled = False
        grpAdj.Enabled = False
        strSql = " SELECT SNO,PREVILEGEID,ACCODE,TRANDATE,TITLE"
        strSql += " ,INITIAL,PNAME,DOORNO,ADDRESS1"
        strSql += " ,ADDRESS2,ADDRESS3,AREA,CITY"
        strSql += " ,STATE,COUNTRY,PINCODE,PHONERES"
        strSql += " ,MOBILE,EMAIL,FAX"
        strSql += " FROM " & cnAdminDb & "..PERSONALINFO"
        strSql += " WHERE SNO = (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = '" & Batchno & "')"
        Dim dtAddress As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtAddress)
        If dtAddress.Rows.Count > 0 Then
            With dtAddress.Rows(0)
                objAddressDia.AddressLock = True
                'objAddressDia.chkRegularCustomer.Checked = True
                'objAddressDia.chkRegularCustomer.Enabled = False
                objAddressDia.txtAddressPrevilegeId.Text = .Item("PREVILEGEID").ToString
                objAddressDia.txtAddressPartyCode.Text = .Item("ACCODE").ToString
                objAddressDia.cmbAddressTitle_OWN.Text = .Item("TITLE").ToString
                objAddressDia.txtAddressInitial.Text = .Item("INITIAL").ToString
                objAddressDia.txtAddressName.Text = .Item("PNAME").ToString
                objAddressDia.txtAddressDoorNo.Text = .Item("DOORNO").ToString
                objAddressDia.txtAddress1.Text = .Item("ADDRESS1").ToString
                objAddressDia.txtAddress2.Text = .Item("ADDRESS2").ToString
                objAddressDia.txtAddress3.Text = .Item("ADDRESS3").ToString
                objAddressDia.cmbAddressArea_OWN.Text = .Item("AREA").ToString
                objAddressDia.cmbAddressCity_OWN.Text = .Item("CITY").ToString
                objAddressDia.cmbAddressState.Text = .Item("STATE").ToString
                objAddressDia.cmbAddressCountry_OWN.Text = .Item("COUNTRY").ToString
                objAddressDia.txtAddressPincode_NUM.Text = .Item("PINCODE").ToString
                objAddressDia.txtAddressPhoneRes.Text = .Item("PHONERES").ToString
                objAddressDia.txtAddressMobile.Text = .Item("MOBILE").ToString
                objAddressDia.txtAddressEmailId_OWN.Text = .Item("EMAIL").ToString
                objAddressDia.txtAddressFax.Text = .Item("FAX").ToString
                objAddressDia.txtAddressRegularSno.Text = .Item("SNO").ToString
            End With
        End If
        btnNew.Enabled = False
        txtOItemId.Select()
    End Sub

    Private Sub Style1ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Style1ToolStripMenuItem.Click
        pnlContainer_OWN.BackColor = Color.RosyBrown
        ColorChange(pnlContainer_OWN, Color.MistyRose, pnlContainer_OWN.BackColor)
        pnlOrderRate.BackColor = Color.Transparent
        pnlOrderType.BackColor = Color.Transparent
    End Sub

    Private Sub Style2ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Style2ToolStripMenuItem.Click
        pnlContainer_OWN.BackColor = Color.SlateGray
        ColorChange(pnlContainer_OWN, Color.MintCream, pnlContainer_OWN.BackColor)
        pnlOrderRate.BackColor = Color.Transparent
        pnlOrderType.BackColor = Color.Transparent
    End Sub

    Private Sub Style3ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Style3ToolStripMenuItem.Click
        pnlContainer_OWN.BackColor = Color.PaleVioletRed
        ColorChange(pnlContainer_OWN, Color.MistyRose, pnlContainer_OWN.BackColor)
        pnlOrderRate.BackColor = Color.Transparent
        pnlOrderType.BackColor = Color.Transparent
    End Sub

    Private Sub Style4ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Style4ToolStripMenuItem.Click
        pnlContainer_OWN.BackColor = SystemColors.AppWorkspace
        ColorChange(pnlContainer_OWN, SystemColors.Control, pnlContainer_OWN.BackColor)
        pnlOrderRate.BackColor = Color.Transparent
        pnlOrderType.BackColor = Color.Transparent
    End Sub

    Private Sub Style5ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Style5ToolStripMenuItem.Click
        pnlContainer_OWN.BackColor = SystemColors.InactiveCaption
        ColorChange(pnlContainer_OWN, Color.Lavender, pnlContainer_OWN.BackColor)
        pnlOrderRate.BackColor = Color.Transparent
        pnlOrderType.BackColor = Color.Transparent
    End Sub

    Private Sub Style6ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Style6ToolStripMenuItem.Click
        pnlContainer_OWN.BackColor = SystemColors.InactiveCaption
        ColorChange(pnlContainer_OWN, Color.Lavender, pnlContainer_OWN.BackColor)
        pnlOrderRate.BackColor = Color.Transparent
        pnlOrderType.BackColor = Color.Transparent
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        If OrdType = OrderType.OrderEntry Then
            If MessageBox.Show("Close Alert", "Do you want to Close?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.OK Then
                Me.Close()
            End If
        Else
            Me.Close()
        End If
    End Sub

    Private Sub ShowStoneDia()
        If objStone.Visible Then Exit Sub
        If Not isStone Then Exit Sub : 
        objStone.grsWt = Val(txtOGrsWt_WET.Text)
        objStone.BackColor = pnlContainer_OWN.BackColor
        objStone.StartPosition = FormStartPosition.CenterScreen
        objStone.MaximizeBox = False
        objStone.grpStone.BackgroundColor = grpHeader.BackgroundColor
        objStone.StyleGridStone(objStone.gridStone)
        objStone.txtStItem.Select()
        objStone.ShowDialog()
        Dim stnWt As Double = Val(objStone.gridStoneTotal.Rows(0).Cells("WEIGHT").Value.ToString)
        Dim stnAmt As Double = Val(objStone.gridStoneTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        Dim ntWt As Double = Val(txtOGrsWt_WET.Text) - stnWt
        txtONetWt_WET.Text = IIf(ntWt <> 0, Format(ntWt, "0.000"), Nothing)
        txtODiaPcsWt.Text = objStone._DiaPcs.ToString & " / " & Format(objStone._DiaWt, "0.000")
        txtOOtherAmt_AMT.Text = IIf(stnAmt <> 0, Format(stnAmt, "0.00"), Nothing)
        Me.SelectNextControl(txtOGrsWt_WET, True, True, True, True)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        chkGenMultiRows.Checked = False
        objGPack.TextClear(Me)
        Set916Rate(BillDate)
        rbtCustomerOrderType.Checked = True
        rbtCurrentRate.Checked = True
        dtpDueDate.Value = BillDate 'GetEntryDate(BillDate)
        dtpRemDate.Value = BillDate 'GetEntryDate(BillDate)
        'BillDate = GetEntryDate(GetServerDate)
        'lblBillDate.Text = GetEntryDate(BillDate).ToString("dd/MM/yyyy")
        ClearDtGrid(dtGridOrder)
        CalcGridOrderTotal()
        ClearItemDetails()
        objStone = New frmStoneDia
        objCreditCard = New frmCreditCardAdj
        objCheaque = New frmChequeAdj
        objAdvance = New frmAdvanceAdj(BillCostId)
        objChitCard = New frmChitAdj
        objAddressDia = New frmAddressDia(True, True)
        objOrderAdvance = New frmOrderAdvance(CASHID)
        objAddtionalDetails = New frmOrAdditionalDetails
        objAddressDia.chkSendSms.Visible = Sms_Send
        objAddressDia.chkSendSms.Checked = Sms_Send
        If OrdRepManDueDate <> 0 Then
            txtDueDays_NUM.Text = OrdRepManDueDate
        Else
            txtDueDays_NUM.Text = "0"
        End If
        txtRemDays_NUM.Text = 1
        dtoredt.Rows.Clear()
        dtSampleDetails.Rows.Clear()
        dtSampleDetails.AcceptChanges()
        dtStoneDetails.Rows.Clear()
        dtStoneDetails.AcceptChanges()
        dtGridSample.Rows.Clear()
        dtGridSample.AcceptChanges()
        dtOrderAdditionalDetails.Rows.Clear()
        dtOrderAdditionalDetails.AcceptChanges()
        AutoImageSizer(My.Resources.no_photo, picImage, PictureBoxSizeMode.StretchImage)
        'picImage.Image = My.Resources.no_photo
        ''SAMPLE
        cmbSamType.Text = "SAMPLE"
        cmbSamFrom.Text = "CUSTOMER"
        rbtCustomerOrderType.Focus()
        If READYITEMBOOKING Then ChkReadyOrder.Visible = True Else ChkReadyOrder.Visible = False
        ChkReadyOrder.Checked = False
        Dim ctlId As String = "ORDERNO"
        Dim orType As String = "O"
        If Not MANBILLNO_FORREADYITEM And READYITEMBOOKING And ChkReadyOrder.Checked Then
            orType = "R"
            ctlId = "READYORDERNO"
        End If
        strSql = " SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = '" & ctlId & "' AND COMPANYID = '" & strCompanyId & "'"
        TranNo = Val(objGPack.GetSqlValue(strSql, , , tran)) + 1
        lblNextNo.Text = orType & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString & TranNo
        If MANBILLNO Then
            lblNx.Visible = False
            lblNext.Visible = False
            lblNextNo.Visible = False
        End If
        lblMcPerGm.Text = "Mc/Grm"
        InterStateBill = False
    End Sub

    Function GetTranControlValues(ByVal ctlId As String, Optional ByVal ddefault As Integer = 1, Optional ByVal dbName As String = Nothing) As Integer
        Dim cmd As OleDbCommand = Nothing
        If dbName = Nothing Then
            strSql = " SELECT ISNULL(MAX(CONVERT(INT,CTLTEXT))+1,1) CTLTEXT FROM " & cnStockDb & "..SOFTCONTROLTRAN"
            strSql += " WHERE CTLID = '" & ctlId & "'"
        Else
            strSql = " SELECT ISNULL(MAX(CONVERT(INT,CTLTEXT))+1,1) CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL"
            strSql += " WHERE CTLID = '" & ctlId & "'"
        End If
        Return Val(objGPack.GetSqlValue(strSql, , ddefault, tran))
    End Function

    Private Sub GenBatchNo()
        Batchno = GetNewBatchno(cnCostId, BillDate, tran)
    End Sub

    Private Sub InsertOrderDetail(ByVal index As Integer)
        With gridOrder.Rows(index)
            ''image ref
            Dim iio As IO.FileInfo = Nothing
            Dim extension As String = Nothing
            Dim picFileName As String = Nothing
            Dim destFilePath As String = Nothing
            If .Cells("PICTPATH").Value.ToString <> "" And defalutDestination <> "" Then
                If IO.Directory.Exists(defalutDestination) Then
                    iio = New IO.FileInfo(.Cells("PICTPATH").Value.ToString)
                    extension = iio.Extension
                    picFileName = iio.Name
                    If .Cells("STYLENO").Value.ToString = "" Then
                        picFileName = OrderNo & "_" & (index + 1).ToString + extension
                        destFilePath = defalutDestination & picFileName
                    End If
                End If
            End If
            Dim orSno As String = GetNewSno(TranSnoType.ORMASTCODE, tran, "GET_ADMINSNO_TRAN")
            strSql = " INSERT INTO " & cnadmindb & "..ORMAST"
            strSql += " ("
            strSql += " SNO,ORNO,ORDATE,REMDATE,DUEDATE,ORTYPE,COMPANYID,ORRATE"
            strSql += " ,ORMODE,ITEMID,SUBITEMID,DESCRIPT,PCS,GRSWT,NETWT"
            strSql += " ,SIZEID,RATE,NATURE,MCGRM,MC,WASTPER,WAST"
            strSql += " ,COMMPER,COMM,OTHERAMT,CANCEL,ORVALUE,COSTID,BATCHNO"
            strSql += " ,CORNO,PROPSMITH,PICTFILE,EMPID"
            strSql += " ,USERID,UPDATED,UPTIME,APPVER"
            strSql += " ,TAX,SC,ADSC,ITEMTYPEID,SIZENO,STYLENO,DISCOUNT"
            strSql += " ,SYSTEMID,CASHID"
            strSql += " )"
            strSql += " VALUES"
            strSql += " ("
            strSql += " '" & orSno & "'" 'SNO
            strSql += " ,'" & OrderNo & "'" 'ORNO
            strSql += " ,'" & BillDate.ToString("yyyy-MM-dd") & "'" 'GetEntryDate(BillDate, tran).ToString("yyyy-MM-dd") & "'" 'ORDATE
            strSql += " ,'" & dtpRemDate.Value.Date.ToString("yyyy-MM-dd") & "'" 'REMDATE
            strSql += " ,'" & dtpDueDate.Value.Date.ToString("yyyy-MM-dd") & "'" 'DUEDATE
            If READYITEMBOOKING And ChkReadyOrder.Checked Then
                strSql += " ,'B'" 'ORTYPE
            ElseIf rbtSchemeOrder.Checked Then
                strSql += " ,'B'" 'ORTYPE
            Else
                strSql += " ,'O'" 'ORTYPE
            End If
            strSql += " ,'" & strCompanyId & "'" 'COMPANYID
            Dim Ratemode As String = "D"
            If rbtCurrentRate.Checked = True Then Ratemode = "C"
            If rbtDeliveryRate.Checked = True Then Ratemode = "D"
            If rbtVariableRate.Checked = True Then Ratemode = "V"
            Dim OrMode As String
            If rbtCustomerOrderType.Checked Then OrMode = "C"
            If rbtCompanyOrderType.Checked Then OrMode = "O"
            If rbtSchemeOrder.Checked Then OrMode = "S"
            strSql += " ,'" & Ratemode & "'" 'ORRATE
            strSql += " ,'" & OrMode & "'" 'ORMODE
            strSql += " ," & IIf(Val("" & .Cells("Itemid").Value) <> 0, .Cells("Itemid").Value, Val(objGPack.GetSqlValue(" SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEMNAME").Value.ToString & "'", , , tran)) & "")
            strSql += " ," & IIf(Val("" & .Cells("SUBItemid").Value) <> 0, .Cells("SUBItemid").Value, Val(objGPack.GetSqlValue(" SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & .Cells("SUBITEMNAME").Value.ToString & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEMNAME").Value.ToString & "')", , , tran)) & "") 'SUBITEMID
            strSql += " ,'" & .Cells("DESCRIPTION").Value.ToString & "'" 'DESCRIPT
            strSql += " ," & Val(.Cells("PCS").Value.ToString) & "" 'PCS
            strSql += " ," & Val(.Cells("GRSWT").Value.ToString) & "" 'GRSWT
            strSql += " ," & Val(.Cells("NETWT").Value.ToString) & "" 'NETWT
            strSql += " ," & Val(objGPack.GetSqlValue("SELECT SIZEID FROM " & cnAdminDb & "..ItemSize WHERE SIZENAME = '" & .Cells("SIZENAME").Value.ToString & "'", , , tran)) & "" 'SIZEID
            strSql += " ," & Val(.Cells("RATE").Value.ToString) & "" 'RATE
            strSql += " ,''" 'NATURE
            strSql += " ," & Val(.Cells("MCGRM").Value.ToString) & "" 'MCGRM
            strSql += " ," & Val(.Cells("MC").Value.ToString) & "" 'MC
            strSql += " ," & Val(.Cells("WASTAGEPER").Value.ToString) & "" 'WASTPER
            strSql += " ," & Val(.Cells("WASTAGE").Value.ToString) & "" 'WAST
            strSql += " ," & Val(.Cells("COMMPER").Value.ToString) & "" 'COMMPER
            strSql += " ," & Val(.Cells("COMM").Value.ToString) & "" 'COMM
            strSql += " ," & Val(.Cells("OTHERAMT").Value.ToString) & "" 'OTHERAMT
            strSql += " ,''" 'CANCEL
            strSql += " ," & Val(.Cells("GROSSAMT").Value.ToString) & "" 'ORVALUE
            strSql += " ,'" & BillCostId & "'" 'COSTID
            strSql += " ,'" & Batchno & "'" 'BATCHNO
            strSql += " ,0" 'CORNO
            strSql += " ,''" 'PROPSMITH

            strSql += " ,'" & picFileName & "'" 'PICTFILE

            strSql += " ," & Val(txtSalesMan_NUM.Text) & "" 'eMPID
            strSql += " ," & userId & ""
            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'"
            strSql += " ,'" & Date.Now.ToLongTimeString & "'"
            strSql += " ,'" & VERSION & "'"
            strSql += " ," & Val(.Cells("VAT").Value.ToString) & "" 'TAX
            strSql += " ,0" 'SC
            strSql += " ,0" 'ADSC
            strSql += " ," & Val(objGPack.GetSqlValue("SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & .Cells("ITEMTYPE").Value.ToString & "'", , , tran)) & "" 'ITEMTYPEID
            strSql += " ,'" & .Cells("SIZENO").Value.ToString & "'" 'SIZENO
            If READYITEMBOOKING And ChkReadyOrder.Checked Then
                strSql += " ,'" & .Cells("TAGNO").Value.ToString & "'" 'STYLENO
            Else
                strSql += " ,'" & .Cells("STYLENO").Value.ToString & "'" 'STYLENO
            End If
            If Val(.Cells("DISCOUNT").Value.ToString) <> 0 Then
                strSql += " ," & Val(.Cells("DISCOUNT").Value.ToString) & "" 'DISCOUNT
            Else
                strSql += " ," & Val(txtAdjDiscount_AMT.Text) & "" 'DISCOUNT
            End If
            strSql += " ,'" & systemId & "'" 'SYSTEMID
            strSql += " ,'" & BillCashCounterId & "'" 'CASHID
            strSql += " )"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId, , destFilePath)
            txtAdjDiscount_AMT.Clear()
            If Val("" & .Cells("ESTNO").Value) <> 0 Or .Cells("TAGNO").Value.ToString <> "" Then
                If Rm_Item_Dly Then

                    Dim Isno As String = GetNewSno(TranSnoType.ORIRDETAILCODE, tran, "GET_ADMINSNO_TRAN")
                    Dim tranno As Integer = 0
                    Dim EntryOrder As Integer = 0
                    strSql = " SELECT ISNULL(MAX(CTLTEXT),0)+1 AS TRANNO FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID  = 'ORDIRBILLNO' AND COMPANYID = '" & strCompanyId & "'"
                    tranno = Val(objGPack.GetSqlValue(strSql, , , tran))

                    strSql = " SELECT ISNULL(MAX(ENTRYORDER),0)+1 AS ENTRYORDER FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORNO = '" & OrderNo & "'"
                    EntryOrder = Val(objGPack.GetSqlValue(strSql, , , tran))

                    Dim DESIGNERID As Integer = 0
                    strSql = " INSERT INTO " & cnAdminDb & "..ORIRDETAIL"
                    strSql += " ("
                    strSql += " SNO,ORSNO,TRANNO,TRANDATE,DESIGNERID,ORNO,COMPANYID"
                    strSql += " ,TAGNO,PROID,DESCRIPT,PCS,GRSWT,NETWT"
                    strSql += " ,MC,WASTAGE,COSTID,BATCHNO"
                    strSql += " ,ENTRYORDER,ORDSTATE_ID"
                    strSql += " ,USERID,UPDATED,UPTIME,APPVER,ORSTATUS"
                    strSql += " )"
                    strSql += " VALUES"
                    strSql += " ("
                    strSql += " '" & Isno & "'" 'SNO
                    strSql += " ,'" & orSno & "'" 'ORsNO
                    strSql += " ,'" & tranno & "'" 'ORNO
                    strSql += " ,'" & BillDate.ToString("yyyy-MM-dd") & "'" 'GetEntryDate(BillDate, tran).ToString("yyyy-MM-dd") & "'" 'ORDATE
                    strSql += " ," & DESIGNERID & ""
                    strSql += " ,'" & OrderNo & "'" 'ORNO
                    strSql += " ,'" & strCompanyId & "'" 'COMPANYID
                    strSql += " ,'" & .Cells("TAGNO").Value.ToString & "'"
                    strSql += " ," & IIf(.Cells("Itemid").Value <> 0, .Cells("Itemid").Value, Val(objGPack.GetSqlValue(" SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEMNAME").Value.ToString & "'", , , tran)) & "")
                    strSql += " ,'" & .Cells("DESCRIPTION").Value.ToString & "'" 'DESCRIPT
                    strSql += " ," & Val(.Cells("PCS").Value.ToString) & "" 'PCS
                    strSql += " ," & Val(.Cells("GRSWT").Value.ToString) & "" 'GRSWT
                    strSql += " ," & Val(.Cells("NETWT").Value.ToString) & "" 'NETWT
                    strSql += " ," & Val(.Cells("MC").Value.ToString) & "" 'MC
                    strSql += " ," & Val(.Cells("WASTAGE").Value.ToString) & "" 'WAST
                    strSql += " ,'" & BillCostId & "'" 'COSTID
                    strSql += " ,'" & Batchno & "'" 'BATCHNO
                    strSql += " ," & EntryOrder & ",2" 'CORNO
                    strSql += " ," & userId & ""
                    strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'"
                    strSql += " ,'" & Date.Now.ToLongTimeString & "'"
                    strSql += " ,'" & VERSION & "'"
                    strSql += " ,'I'" 'ORTYPE
                    strSql += " )"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
                    Dim Rsno As String = GetNewSno(TranSnoType.ORIRDETAILCODE, tran, "GET_ADMINSNO_TRAN")
                    strSql = Replace(strSql, "'" + Isno + "'", "'" + Rsno + "'")
                    strSql = Replace(strSql, ",'I' )", ",'R' )")

                    Dim Qry As String = " SELECT ISNULL(MAX(ENTRYORDER),0)+1 AS ENTRYORDER FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORNO = '" & OrderNo & "'"
                    Dim EntryOrderR As Integer = 0
                    EntryOrderR = Val(objGPack.GetSqlValue(Qry, , , tran))

                    strSql = Replace(strSql, "," & EntryOrder & ",2", "," & EntryOrderR & ",4")
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
                    strSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET ORDREPNO='" & OrderNo & "',ORSNO='" & orSno & "'"
                    strSql += " WHERE ITEMID = " & .Cells("Itemid").Value & " AND TAGNO = '" & .Cells("TAGNO").Value.ToString & "'"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
                End If
                If READYITEMBOOKING And ChkReadyOrder.Checked Then
                    strSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET ORDREPNO='" & OrderNo & "',ORSNO='" & orSno & "'"
                    strSql += " WHERE ITEMID = " & .Cells("Itemid").Value & " AND TAGNO = '" & .Cells("TAGNO").Value.ToString & "'"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
                    strSql = " UPDATE " & cnStockDb & "..ESTISSUE SET ORDERNO = '" & OrderNo & "' ,ORSNO='" & orSno & "'"
                    strSql += " WHERE TRANNO = " & .Cells("ESTNO").Value & ""
                    strSql += " AND TAGNO='" & .Cells("TAGNO").Value & "'"
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()
                End If
                If orderestimate = True Then
                    strSql = " UPDATE " & cnStockDb & "..ESTORMAST SET ODBATCHNO = '" & Batchno & "' "
                    strSql += " WHERE ORNO = '" & Val(.Cells("ESTNO").Value) & "' AND ORTYPE = 'O' "
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()
                    orderestimate = False
                End If
            End If

            If Val(.Cells("ADDCARTID").Value.ToString) > 0 Then
                strSql = " DELETE FROM " & cnStockDb & "..STYLECARD WHERE SID = " & Val(.Cells("ADDCARTID").Value.ToString) & ""
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
            End If

            ''Stone
            For Each stRow As DataRow In dtStoneDetails.Rows
                If .Cells("KEYNO").Value = stRow("KEYNO") Then
                    InsertStoneDetails(stRow, orSno)
                End If
            Next

            ''Sample
            For Each samRow As DataRow In dtSampleDetails.Rows
                If .Cells("KEYNO").Value = samRow("KEYNO") Then
                    InsertSampleDetails(samRow, orSno)
                End If
            Next
            'OR ADDTIONAL DETAILS
            For Each stRow As DataRow In dtOrderAdditionalDetails.Rows
                If .Cells("KEYNO").Value = stRow("KEYNO") Then
                    InsertOrderAdditionalDetails(TranNo, orSno, "OR", BillCostId, strCompanyId, stRow, Batchno, tran)
                End If
            Next

        End With
    End Sub

    Private Sub InsertSampleDetails(ByVal samRow As DataRow, ByVal orSno As String)
        With samRow
            strSql = " INSERT INTO " & cnAdminDb & "..ORSAMPLE "
            strSql += " ("
            strSql += " SNO,ORSNO,ITEMID,DESCRIP,TAGNO,GRSWT,NETWT,SAMPLETYPE"
            strSql += " ,ISSUSEDFOR,CANCEL,COSTID,BATCHNO,APPVER,COMPANYID"
            strSql += " )"
            strSql += " VALUES"
            strSql += " ("
            strSql += " '" & GetNewSno(TranSnoType.ORSAMPLECODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO
            strSql += " ,'" & orSno & "'" 'ORSNO
            strSql += " ," & Val(objGPack.GetSqlValue(" SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Item("ITEM").ToString & "'", , , tran)) & "" 'ITEMID
            strSql += " ,'" & .Item("DESCRIPTION").ToString & "'" 'DESCRIP
            strSql += " ,'" & .Item("TAGNO").ToString & "'" 'TAGNO
            strSql += " ," & Val(.Item("GRSWT").ToString) & "" 'GRSWT
            strSql += " ," & Val(.Item("NETWT").ToString) & "" 'NETWT
            strSql += " ,'" & Mid(.Item("TYPE").ToString, 1, 1) & "'" 'SAMPLETYPE
            strSql += " ,'" & IIf(UCase(.Item("FROM").ToString) = "CUSTOMER", "C", "O") & "'" 'ISSUSEDFOR
            strSql += " ,''" 'CANCEL
            strSql += " ,'" & BillCostId & "'" 'COSTID
            strSql += " ,'" & Batchno & "'" 'BATCHNO
            strSql += " ,'" & VERSION & "'" 'VERSION
            strSql += " ,'" & strCompanyId & "'" 'COMPANYID
            strSql += " )"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
        End With
    End Sub

    Private Sub InsertStoneDetails(ByVal stnRow As DataRow, ByVal orSno As String)
        ''Insert Stone
        With stnRow
            strSql = " INSERT INTO " & cnadmindb & "..ORSTONE"
            strSql += " ("
            strSql += " SNO,ORSNO,STNITEMID,STNSUBITEMID,STNPCS,STNWT,STNRATE,STNAMT"
            strSql += " ,BATCHNO,CANCEL,COSTID,APPVER,STNUNIT,CALCMODE,COMPANYID"
            strSql += " ,CUTID,COLORID,CLARITYID,SETTYPEID,SHAPEID,WIDTH,HEIGHT"
            strSql += " )"
            strSql += " VALUES"
            strSql += " ("
            strSql += " '" & GetNewSno(TranSnoType.ORSTONECODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO
            strSql += " ,'" & orSno & "'" 'ORSNO
            strSql += " ," & Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Item("ITEM").ToString & "'", , , tran)) & "" 'STNITEMID
            strSql += " ," & Val(objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & .Item("SUBITEM").ToString & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Item("ITEM").ToString & "')", , , tran)) & "" 'STNSUBITEMID
            strSql += " ," & Val(.Item("PCS").ToString) & "" 'STNPCS
            strSql += " ," & Val(.Item("WEIGHT").ToString) & "" 'STNWT
            strSql += " ," & Val(.Item("RATE").ToString) & "" 'STNRATE
            strSql += " ," & Val(.Item("AMOUNT").ToString) & "" 'STNAMOUNT
            strSql += " ,'" & Batchno & "'" 'BATCHNO
            strSql += " ,''" 'CANCEL
            strSql += " ,'" & BillCostId & "'" 'COSTID
            strSql += " ,'" & VERSION & "'" 'VERSION
            strSql += " ,'" & .Item("UNIT").ToString & "'" 'STNUNIT
            strSql += " ,'" & .Item("CALC").ToString & "'" 'STNCALC
            strSql += " ,'" & strCompanyId & "'" 'COMPANYID
            strSql += " ,'" & Val(.Item("CUTID").ToString) & "'" 'CUTID
            strSql += " ,'" & Val(.Item("COLORID").ToString) & "'" 'COLORID            
            strSql += " ,'" & Val(.Item("CLARITYID").ToString) & "'" 'SETTYPEID
            strSql += " ,'" & Val(.Item("SETTYPEID").ToString) & "'" 'CLARITYID
            strSql += " ,'" & Val(.Item("SETTYPEID").ToString) & "'" 'SHAPEID
            strSql += " ,'" & Val(.Item("WIDTH").ToString) & "'" 'WIDTH
            strSql += " ,'" & Val(.Item("HEIGHT").ToString) & "'" 'HEIGHT
            strSql += " )"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
        End With
    End Sub

    Private Function GetEntFlag() As Boolean
        Dim entFlag As Boolean = False
        For Each ro As DataRow In dtGridOrder.Rows
            If ro!ENTFLAG.ToString = "" Then Exit For
            Return True
        Next
        Return entFlag
    End Function

    Private Sub SendSms()
        Dim obj As New GiriTechSMS.SMSService
        obj.ClientSendSMS(Sms_Masking, objAddressDia.txtAddressMobile.Text, IIf(Sms_Name, IIf(objAddressDia.txtAddressName.Text <> "", "Dear " & objAddressDia.cmbAddressTitle_OWN.Text & " ", "") & objAddressDia.txtAddressName.Text & " ", "") & Sms_Msg)
        'MsgBox("Sms Sended..")
    End Sub

    Private Sub UpdateOrder()
        With gridOrder.Rows(0)
            ''image ref
            Try
                strSql = " SELECT BATCHNO,ORDATE FROM " & cnAdminDb & "..ORMAST WHERE SNO = '" & OrderUpdSno & "'"
                Batchno = objGPack.GetSqlValue(strSql, "BATCHNO")
                Dim objAddressDia As New frmAddressDia(BillDate, BillCostId, Batchno, True)
                objAddressDia.BackColor = SystemColors.InactiveCaption
                objAddressDia.MaximizeBox = False
                objAddressDia.grpAddress.BackgroundColor = Color.Lavender
                objAddressDia.ShowDialog()
                tran = Nothing
                tran = cn.BeginTransaction
                Dim iio As IO.FileInfo = Nothing
                Dim extension As String = Nothing
                Dim picFileName As String = Nothing
                Dim destFilePath As String = Nothing
                If defalutDestination <> Nothing Then
                    If IO.Directory.Exists(defalutDestination) Then
                        Try
                            For cnt As Integer = 0 To gridOrder.RowCount - 1
                                With gridOrder.Rows(cnt)
                                    If .Cells("ENTFLAG").Value.ToString = "" Then Continue For
                                    If .Cells("STYLENO").Value.ToString <> "" Then Continue For
                                    If IO.File.Exists(.Cells("PICTPATH").Value.ToString) Then
                                        iio = New IO.FileInfo(.Cells("PICTPATH").Value.ToString)
                                        extension = iio.Extension
                                        destFilePath = defalutDestination & OrderNo & "_" & OrderUpdSno + extension
                                        IO.File.Copy(.Cells("PICTPATH").Value.ToString, destFilePath, True)
                                    End If
                                End With
                            Next
                        Catch ex As Exception
                            tran.Rollback()
                            tran.Dispose()
                            tran = Nothing
                            MsgBox(ex.Message)
                            Exit Sub
                        End Try
                    End If
                End If
                If .Cells("PICTPATH").Value.ToString <> "" And defalutDestination <> "" Then
                    If IO.Directory.Exists(defalutDestination) Then
                        iio = New IO.FileInfo(.Cells("PICTPATH").Value.ToString)
                        extension = iio.Extension
                        picFileName = iio.Name
                        If .Cells("STYLENO").Value.ToString = "" Then
                            picFileName = OrderNo & "_" & OrderUpdSno + extension
                            destFilePath = defalutDestination & picFileName
                        End If
                    End If
                End If
                strSql = vbCrLf + " UPDATE " & cnAdminDb & "..ORMAST SET"
                strSql += vbCrLf + " ITEMID = " & Val(objGPack.GetSqlValue(" SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEMNAME").Value.ToString & "'", , , tran)) & "" 'ITEMID
                strSql += vbCrLf + " ,SUBITEMID = " & Val(objGPack.GetSqlValue(" SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & .Cells("SUBITEMNAME").Value.ToString & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEMNAME").Value.ToString & "')", , , tran)) & "" 'SUBITEMID
                If ORDDATE_EDIT Then
                    strSql += vbCrLf + " ,ORDATE='" & BillDate.ToString("yyyy-MM-dd") & "'" 'ORDATE
                    strSql += vbCrLf + " ,REMDATE='" & dtpRemDate.Value.Date.ToString("yyyy-MM-dd") & "'" 'REMDATE
                    strSql += vbCrLf + " ,DUEDATE='" & dtpDueDate.Value.Date.ToString("yyyy-MM-dd") & "'" 'DUEDATE
                End If
                strSql += vbCrLf + " ,DESCRIPT = '" & .Cells("DESCRIPTION").Value.ToString & "'" 'DESCRIPT
                strSql += vbCrLf + " ,PCS = " & Val(.Cells("PCS").Value.ToString) & "" 'PCS
                strSql += vbCrLf + " ,GRSWT = " & Val(.Cells("GRSWT").Value.ToString) & "" 'GRSWT
                strSql += vbCrLf + " ,NETWT = " & Val(.Cells("NETWT").Value.ToString) & "" 'NETWT
                strSql += vbCrLf + " ,SIZEID = " & Val(objGPack.GetSqlValue("SELECT SIZEID FROM " & cnAdminDb & "..ItemSize WHERE SIZENAME = '" & .Cells("SIZENAME").Value.ToString & "'", , , tran)) & "" 'SIZEID
                strSql += vbCrLf + " ,RATE = " & Val(.Cells("RATE").Value.ToString) & "" 'RATE
                strSql += vbCrLf + " ,MCGRM = " & Val(.Cells("MCGRM").Value.ToString) & "" 'MCGRM
                strSql += vbCrLf + " ,MC = " & Val(.Cells("MC").Value.ToString) & "" 'MC
                strSql += vbCrLf + " ,WASTPER = " & Val(.Cells("WASTAGEPER").Value.ToString) & "" 'WASTPER
                strSql += vbCrLf + " ,WAST = " & Val(.Cells("WASTAGE").Value.ToString) & "" 'WAST
                strSql += vbCrLf + " ,COMMPER = " & Val(.Cells("COMMPER").Value.ToString) & "" 'COMMPER
                strSql += vbCrLf + " ,COMM = " & Val(.Cells("COMM").Value.ToString) & "" 'COMM
                strSql += vbCrLf + " ,OTHERAMT = " & Val(.Cells("OTHERAMT").Value.ToString) & "" 'OTHERAMT
                strSql += vbCrLf + " ,ORVALUE = " & Val(.Cells("GROSSAMT").Value.ToString) & "" 'ORVALUE
                strSql += vbCrLf + " ,PICTFILE = '" & picFileName & "'" 'PICTFILE
                strSql += vbCrLf + " ,TAX = " & Val(.Cells("VAT").Value.ToString) & "" 'TAX
                strSql += vbCrLf + " ,ITEMTYPEID = " & Val(objGPack.GetSqlValue("SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & .Cells("ITEMTYPE").Value.ToString & "'", , , tran)) & "" 'ITEMTYPEID
                strSql += vbCrLf + " ,SIZENO = '" & .Cells("SIZENO").Value.ToString & "'" 'SIZENO
                strSql += vbCrLf + " ,STYLENO = '" & .Cells("STYLENO").Value.ToString & "'" 'STYLENO
                strSql += vbCrLf + " WHERE SNO = '" & OrderUpdSno & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId, , destFilePath)

                ''Stone
                strSql = " DELETE FROM " & cnAdminDb & "..ORSTONE"
                strSql += " WHERE ORSNO = '" & OrderUpdSno & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
                For Each stRow As DataRow In dtStoneDetails.Rows
                    If .Cells("KEYNO").Value = stRow("KEYNO") Then
                        InsertStoneDetails(stRow, OrderUpdSno)
                    End If
                Next
                ''Sample
                strSql = " DELETE FROM " & cnAdminDb & "..ORSAMPLE"
                strSql += " WHERE ORSNO = '" & OrderUpdSno & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
                For Each samRow As DataRow In dtSampleDetails.Rows
                    If .Cells("KEYNO").Value = samRow("KEYNO") Then
                        InsertSampleDetails(samRow, OrderUpdSno)
                    End If
                Next

                ''Additional Details
                strSql = " DELETE FROM " & cnAdminDb & "..ORADTRAN"
                strSql += " WHERE ORSNO = '" & OrderUpdSno & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
                For Each stRow As DataRow In dtOrderAdditionalDetails.Rows
                    If .Cells("KEYNO").Value = stRow("KEYNO") Then
                        InsertOrderAdditionalDetails(TranNo, OrderUpdSno, "OR", BillCostId, strCompanyId, stRow, Batchno, tran)
                    End If
                Next

                tran.Commit()
                tran = Nothing
            Catch ex As Exception
                If tran IsNot Nothing Then tran.Rollback()
                MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            End Try
        End With
        MsgBox("Updated")
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        ''VALIDATION
        If CheckTrialDate(BillDate) = False Then Exit Sub
        If GetAdmindbSoftValue("ORD_" & strCompanyId, "N") = "Y" Then MsgBox("Order entry locken on " & strCompanyName & " Company, Change the Company. ", MsgBoxStyle.Information) : Exit Sub
        If OrdType = OrderType.OrderEntry Then
            If Not MANBILLNO Then
                If Not CheckDate(BillDate) Then Exit Sub
                If CheckEntryDate(BillDate) Then Exit Sub
            End If
        End If
        'Dim Amt As Double = Val(txtAdjRec_AMT.Text)
        'If GSTADVCALC_INCL = False Then
        '    Amt += Val(txtAdjGst_AMT.Text)
        'End If
        'If Amt > 0 Then
        '    If (Val(txtAdjAdvance_AMT.Text) + Val(txtAdjCash_AMT.Text) + Val(txtAdjChitCard_AMT.Text) + Val(txtAdjCreditCard_AMT.Text) + Val(txtAdjCheque_AMT.Text)) <> Amt Then
        '        MsgBox("Amount not Tally...", MsgBoxStyle.Information)
        '        Exit Sub
        '    End If
        'End If
        Dim SEPPURBILL As Boolean = IIf(GetAdmindbSoftValuefromDt(dtSoftKeys, "SEPORDPURBILLNO", "Y") = "Y", True, False)
        Dim entFlag As Boolean = GetEntFlag()
        If Not entFlag Then
            MsgBox("There is no Record", MsgBoxStyle.Information)
            txtOItemId.Focus()
            Exit Sub
        End If
        If Addressflag = False Then
            If objAddressDia.txtAddressName.Text = "" Then
                MsgBox("Party Name Should Not Empty", MsgBoxStyle.Information)
                ShowAddressDia()
                objAddressDia.txtAddressName.Select()
                Addressflag = True
                Exit Sub
            Else
                MsgBox("Any Changes In Party Name", MsgBoxStyle.Information)
                ShowAddressDia()
                Addressflag = True
                objAddressDia.txtAddressName.Select()
                Exit Sub
            End If

        End If

        Dim _ChkBy As String = ""
        Dim _SearchCol As String = ""
        Dim _SearchVal As String = ""
        If objSoftKeys.POS_PANLOCK = "M" Then _ChkBy = "M"
        If objSoftKeys.POS_PANLOCK = "A" Then _ChkBy = "A"
        If objSoftKeys.POS_PANLOCK = "P" Then _ChkBy = "P"
        If _ChkBy = "M" Then
            _SearchVal = objAddressDia.txtAddressMobile.Text
            _SearchCol = "MOBILE"
        End If
        If _ChkBy = "P" Then
            _SearchVal = objAddressDia.txtAddressPan.Text
            _SearchCol = "PAN"
        End If
        If _ChkBy = "A" Then
            _SearchVal = objAddressDia.txtAddressPartyCode.Text
            _SearchCol = "ACCODE"
        End If
        If _ChkBy <> "" And _SearchVal <> "" Then
            Dim CPurchAmt As Double = Val(txtAdjCash_AMT.Text)
            'If objSoftKeys.POS_PANLOCK_MODE.Contains("AA") = True Then CPurchAmt += Val(txtAdjAdvance_AMT.Text)
            'If objSoftKeys.POS_PANLOCK_MODE.Contains("DU") = True Then CPurchAmt += Val(txtAdjCheque_AMT.Text)
            'If objSoftKeys.POS_PANLOCK_MODE.Contains("PU") = True Then CPurchAmt += Val(gridPurTotal.Rows(0).Cells("AMOUNT").Value.ToString)
            'If objSoftKeys.POS_PANLOCK_MODE.Contains("CH") = True And Val(gridSASRTotal.Rows(0).Cells("AMOUNT").Value.ToString) > 0 Then CPurchAmt += Val(txtAdjCheque_AMT.Text)
            'If objSoftKeys.POS_PANLOCK_MODE.Contains("SS") = True Then CPurchAmt += Val(txtAdjChitCard_AMT.Text)
            If objSoftKeys.POS_PANLOCK_MODE.Contains("CC") = True Then CPurchAmt += Val(txtAdjCreditCard_AMT.Text)
            If objSoftKeys.POS_PANLOCK_MODE.Contains("DI") = True Then CPurchAmt += Val(txtAdjDiscount_AMT.Text)
            'If objSoftKeys.POS_PANLOCK_MODE.Contains("HC") = True Then CPurchAmt += Val(txtAdjHandlingCharge_AMT.Text)
            'If objSoftKeys.POS_PANLOCK_MODE.Contains("GV") = True Then CPurchAmt += Val(txtAdjGiftVoucher_AMT.Text)
            Dim AdjAmt As Double
            strSql = " SELECT SUM(CASH)CASH "
            strSql += " FROM " & cnStockDb & "..ADJTRAN WHERE 1=1 "
            If CashReceivedValidate = "Y" Then
                strSql += " AND YEAR(TRANDATE)='" & BillDate.Year & "'"
            ElseIf CashReceivedValidate = "M" Then
                strSql += " AND MONTH(TRANDATE)='" & BillDate.Month & "'"
            ElseIf CashReceivedValidate = "D" Or CashReceivedValidate = "" Then
                strSql += " AND TRANDATE='" & BillDate & "'"
            Else
                strSql += " AND TRANDATE='" & BillDate & "'"
            End If
            strSql += " AND RECPAY='R'"
            strSql += " AND ISNULL(CANCEL,'')=''"
            strSql += " AND " & _SearchCol & "='" & _SearchVal & "'"
            AdjAmt = Val(objGPack.GetSqlValue(strSql, "CASH").ToString)
            If AdjAmt = 0 Then
                strSql = " SELECT SUM(AMOUNT)AMOUNT "
                strSql += " FROM " & cnStockDb & "..ACCTRAN WHERE 1=1 "
                If CashReceivedValidate = "Y" Then
                    strSql += " AND YEAR(TRANDATE)='" & BillDate.Year & "'"
                ElseIf CashReceivedValidate = "M" Then
                    strSql += " AND MONTH(TRANDATE)='" & BillDate.Month & "'"
                ElseIf CashReceivedValidate = "D" Or CashReceivedValidate = "" Then
                    strSql += " AND TRANDATE='" & BillDate & "'"
                Else
                    strSql += " AND TRANDATE='" & BillDate & "'"
                End If
                strSql += " AND TRANMODE='D' "
                If objSoftKeys.POS_PANLOCK_MODE <> "" Then
                    strSql += " AND PAYMODE IN('" & objSoftKeys.POS_PANLOCK_MODE.Replace(",", "','") & "') "
                End If
                strSql += " AND FROMFLAG='P' "
                strSql += " AND ISNULL(CANCEL,'')=''"
                If _ChkBy = "M" Then
                    strSql += " AND BATCHNO IN(SELECT DISTINCT BATCHNO FROM " & cnAdminDb & "..CUSTOMERINFO"
                    strSql += " WHERE PSNO IN(SELECT SNO FROM " & cnAdminDb & "..PERSONALINFO WHERE " & _SearchCol & "='" & _SearchVal & "'))"
                End If
                If _ChkBy = "P" Then
                    strSql += " AND BATCHNO IN(SELECT DISTINCT BATCHNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE " & _SearchCol & "='" & _SearchVal & "')"
                End If
                If _ChkBy = "A" Then
                    strSql += " AND BATCHNO IN(SELECT DISTINCT BATCHNO FROM " & cnStockDb & "..ISSUE WHERE " & _SearchCol & "='" & _SearchVal & "')"
                End If
                CPurchAmt += Val(objGPack.GetSqlValue(strSql, "AMOUNT").ToString)
            Else
                CPurchAmt += AdjAmt
            End If
            If (CPurchAmt > objSoftKeys.CashReceived And objSoftKeys.CashReceived <> 0) Then
                If objSoftKeys.CASHRECEIVEDLOCK = "R" Then
                    MsgBox("Cash Received Restricted Upto " & Val(objSoftKeys.CashReceived), MsgBoxStyle.Information)
                    txtAdjCash_AMT.Select()
                    Exit Sub
                Else
                    If MessageBox.Show("Cash Received Should Not Exceed " & objSoftKeys.CashReceived & vbCrLf & "Do you wish to Continue?", "CashReceived Exceed", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                        GoTo ExCash
                    End If
                End If
                txtAdjCash_AMT.Select()
                Exit Sub
            End If
        End If
        If GetAdmindbSoftValuefromDt(dtSoftKeys, "VBC_" & strCompanyId, "N") = "N" Then
            If Val(txtAdjCash_AMT.Text) > 0 And Math.Abs(Val(txtAdjCash_AMT.Text)) > Val(objSoftKeys.CashReceived) Then
                If objSoftKeys.CASHRECEIVEDLOCK = "R" Then
                    MsgBox("Cash Received Restricted Upto " + objSoftKeys.CashReceived, MsgBoxStyle.Information)
                Else
                    If MessageBox.Show("Cash Received Should Not Exceed " & objSoftKeys.CashReceived & vbCrLf & "Do you wish to Continue?", "CashReceived Exceed", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                        GoTo ExCash
                    End If
                End If
                txtAdjCash_AMT.Select()
                Exit Sub
            End If
        End If
ExCash:

        Dim MINADVANYRATE As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "ORDMINADVAMTLOCK", "N").ToString
        If rbtCurrentRate.Checked = True Or (MINADVANYRATE = "Y" Or MINADVANYRATE = "R") Then
            Dim ORDMINADVRATEPER As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "ORDMINADVRATE", "0")
            If ORDMINADVRATEPER <> "0" Then
                Dim mgoldamt As Decimal = Val("" & dtGridOrder.Compute("SUM(AMOUNT)", "METALID='G'"))
                Dim msilvamt As Decimal = Val("" & dtGridOrder.Compute("SUM(AMOUNT)", "METALID='S'"))
                Dim mdiaamt As Decimal = Val("" & dtGridOrder.Compute("SUM(AMOUNT)", "METALID='D'"))
                Dim mgoldper As Decimal = 0, msilvper As Decimal = 0, mdiaper As Decimal = 0
                Dim minadvperarry() As String = Split(ORDMINADVRATEPER, ",")
                If minadvperarry.Length > 1 Then
                    mgoldper = Val(minadvperarry(0)) : msilvper = Val(minadvperarry(1)) : If minadvperarry.Length > 2 Then mdiaper = Val(minadvperarry(2))
                Else
                    mgoldper = msilvper = mdiaper = Val(minadvperarry(0))
                End If
                Dim mgoldadvamt As Decimal = 0, msilvadvamt As Decimal = 0, mdiaadvamt As Decimal = 0
                If mgoldamt > 0 And mgoldper > 0 Then mgoldadvamt = mgoldamt * (mgoldper / 100)
                If msilvamt > 0 And msilvper > 0 Then msilvadvamt = msilvamt * (msilvper / 100)
                If mdiaamt > 0 And mdiaper > 0 Then mdiaadvamt = mdiaamt * (mdiaper / 100)
                If mgoldadvamt + msilvadvamt + mdiaadvamt > Val(Mid(lblnetamount.Text, InStr(lblnetamount.Text, ":") + 1)) - Val(Mid(lblBalance.Text, InStr(lblBalance.Text, ":") + 1)) Then
                    If MINADVANYRATE = "N" Then
                        rbtDeliveryRate.Checked = True
                        rbtCurrentRate.Checked = False
                        If MsgBox("Advance Payment(" & mgoldadvamt + msilvadvamt + mdiaadvamt & "/-) too less for RATE FIX" & vbCrLf & "Can you change advance? ", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                            txtAdjCash_AMT.Focus()
                            Exit Sub
                        End If
                    Else
                        If MINADVANYRATE = "Y" Then
                            If MsgBox("Advance Payment(" & mgoldadvamt + msilvadvamt + mdiaadvamt & "/-) too less than Minimum Advance Amount" & vbCrLf & "Can you change advance? ", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                                txtAdjCash_AMT.Focus()
                                Exit Sub
                            End If
                        Else
                            MsgBox("Advance Payment(" & mgoldadvamt + msilvadvamt + mdiaadvamt & "/-) too less than Minimum Advance Amount", MsgBoxStyle.Information)
                            txtAdjCash_AMT.Focus()
                            Exit Sub
                        End If
                    End If

                End If
            End If
        End If
        Dim objDenom As Denomination = Nothing
        If GetAdmindbSoftValuefromDt(dtSoftKeys, "DENOMINATION", "N") = "Y" Then
            objDenom = New Denomination
            objDenom.txtBillAmount.Text = Format(Val(txtAdjCash_AMT.Text), "0.00")
            If objDenom.ShowDialog <> Windows.Forms.DialogResult.OK Then
                Exit Sub
            End If
        End If
        Dim Ispurchase As Boolean = False
        Dim Ptranno As Long
        For Each ro As DataRow In objOrderAdvance.dtGridPur.Rows
            If ro!ENTFLAG.ToString = "" Then Continue For
            If ro.Item("MODE").ToString = "PURCHASE" Then Ispurchase = True : Exit For
        Next
        For Each ro As DataRow In objOrderAdvance.dtGridPur.Rows
            If ro!ENTFLAG.ToString = "" Then Continue For
            If ro!EMPID.ToString = "" Then
                ro!EMPID = Val(txtSalesMan_NUM.Text.ToString)
            End If
        Next

        If OrdType = OrderType.OrderEntry Then
            If MANBILLNO And ChkReadyOrder.Checked = False Then
                objManualBill = New frmManualBillNoGen
                frmManualBillNoGen.Text = "Manual Order No"
                objGPack.Validator_Object(objManualBill)
ReEnterBillNO:
                If objManualBill.ShowDialog = Windows.Forms.DialogResult.OK Then
                    TranNo = Val(objManualBill.txtBillNo_NUM.Text)
                    strSql = " SELECT DISTINCT ORNO FROM " & cnAdminDb & "..ORMAST"
                    strSql += " WHERE ORNO = '" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & "O" & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString & TranNo.ToString & "'"
                    strSql += " AND COMPANYID = '" & strCompanyId & "'"
                    strSql += " UNION"
                    strSql += " SELECT DISTINCT RUNNO FROM " & cnAdminDb & "..OUTSTANDING"
                    strSql += " WHERE RUNNO = '" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & "O" & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString & TranNo.ToString & "'"
                    strSql += " AND COMPANYID = '" & strCompanyId & "'"
                    If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
                        MsgBox("Order BillNo Already Exist", MsgBoxStyle.Information)
                        GoTo ReEnterBillNO
                    End If
                    If Ispurchase And SEPPURBILL Then
                        If Ispurbillauto Then GoTo vvvpurauto
ReEnterBillNOP:
                        frmManualBillNoGen.Text = "Manual Purchase No"
                        If objManualBill.ShowDialog = Windows.Forms.DialogResult.OK Then
                            Ptranno = Val(objManualBill.txtBillNo_NUM.Text)
                            strSql = " SELECT DISTINCT TRANNO FROM " & cnStockDb & "..RECEIPT"
                            strSql += " WHERE TRANNO = '" & Ptranno.ToString & "'"
                            strSql += " AND COMPANYID = '" & strCompanyId & "'"
                            If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
                                MsgBox("Purchase BillNo Already Exist", MsgBoxStyle.Information)
                                GoTo ReEnterBillNOP
                            End If
                        End If
vvvpurauto:
                    Else
                        If Ispurchase Then Ptranno = TranNo
                    End If
                Else
                    btnSave.Focus()
                    Me.Close()
                    Exit Sub
                End If
            End If
            If MANBILLNO_FORREADYITEM And ChkReadyOrder.Checked And READYITEMBOOKING Then
                objManualBill = New frmManualBillNoGen
                frmManualBillNoGen.Text = "Manual Order No"
                objGPack.Validator_Object(objManualBill)
ReEnterBillNO1:
                If objManualBill.ShowDialog = Windows.Forms.DialogResult.OK Then
                    TranNo = Val(objManualBill.txtBillNo_NUM.Text)
                    strSql = " SELECT DISTINCT ORNO FROM " & cnAdminDb & "..ORMAST"
                    strSql += " WHERE ORNO = '" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & "B" & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString & TranNo.ToString & "'"
                    strSql += " AND COMPANYID = '" & strCompanyId & "'"
                    strSql += " UNION"
                    strSql += " SELECT DISTINCT RUNNO FROM " & cnAdminDb & "..OUTSTANDING"
                    strSql += " WHERE RUNNO = '" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & "B" & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString & TranNo.ToString & "'"
                    strSql += " AND COMPANYID = '" & strCompanyId & "'"
                    If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
                        MsgBox("Order BillNo Already Exist", MsgBoxStyle.Information)
                        GoTo ReEnterBillNO1
                    End If
                    If Ispurchase And SEPPURBILL Then
                        If Ispurbillauto Then GoTo vvvpurauto1
ReEnterBillNOP1:
                        frmManualBillNoGen.Text = "Manual Purchase No"
                        If objManualBill.ShowDialog = Windows.Forms.DialogResult.OK Then
                            Ptranno = Val(objManualBill.txtBillNo_NUM.Text)
                            strSql = " SELECT DISTINCT TRANNO FROM " & cnStockDb & "..RECEIPT"
                            strSql += " WHERE TRANNO = '" & Ptranno.ToString & "'"
                            strSql += " AND COMPANYID = '" & strCompanyId & "'"
                            If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
                                MsgBox("Purchase BillNo Already Exist", MsgBoxStyle.Information)
                                GoTo ReEnterBillNOP1
                            End If
                        End If
vvvpurauto1:
                    Else
                        If Ispurchase Then Ptranno = TranNo
                    End If
                Else
                    btnSave.Focus()
                    Me.Close()
                    Exit Sub
                End If
            End If
        ElseIf OrdType = OrderType.OrderUpdate Then
            UpdateOrder()
            strSql = " SELECT BATCHNO,ORDATE FROM " & cnAdminDb & "..ORMAST WHERE SNO = '" & OrderUpdSno & "'"
            Batchno = objGPack.GetSqlValue(strSql, "BATCHNO")
            BillDate = objGPack.GetSqlValue(strSql, "ORDATE")
            CallBillPrint(Batchno, BillDate)
            Me.Close()
            Exit Sub
        End If
        Me.Refresh()
        Try
            tran = Nothing
            tran = cn.BeginTransaction()

            If OrdType = OrderType.OrderEntry Then
                GenBatchNo()
                If GetAdmindbSoftValuefromDt(dtSoftKeys, "DENOMINATION", "N") = "Y" Then
                    objDenom.InsertDenomTran(Batchno, BillDate, BillCostId, tran)
                End If
                Dim ControlId As String = "ORDERNO"
                Dim OrType As String = "O"
GETORDERNO:
                If Not MANBILLNO And ChkReadyOrder.Checked = False Then
                    If rbtSchemeOrder.Checked Then
                        ControlId = "READYORDERNO"
                        OrType = "B"
                    Else
                        ControlId = "ORDERNO"
                        OrType = "O"
                    End If

                    strSql = " SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = '" & ControlId & "' AND COMPANYID = '" & strCompanyId & "'"
                    TranNo = Val(objGPack.GetSqlValue(strSql, , , tran))

                    strSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & TranNo + 1 & "' "
                    strSql += " WHERE CTLID = '" & ControlId & "' AND COMPANYID = '" & strCompanyId & "'"
                    strSql += " AND CONVERT(INT,CTLTEXT) = " & TranNo & ""
                    cmd = New OleDbCommand(strSql, cn, tran)
                    If cmd.ExecuteNonQuery() = 0 Then
                        GoTo GETORDERNO
                    End If
                    strSql = " SELECT DISTINCT ORNO FROM " & cnAdminDb & "..ORMAST"
                    strSql += " WHERE ORNO = '" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & OrType & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString & Val(TranNo + 1).ToString & "'"
                    strSql += " AND COMPANYID = '" & strCompanyId & "'"
                    If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
                        GoTo GETORDERNO
                    End If
                    TranNo = TranNo + 1
                    Ispurbillauto = True
                End If
                If Not MANBILLNO_FORREADYITEM And READYITEMBOOKING And ChkReadyOrder.Checked Then
                    ControlId = "READYORDERNO"
                    OrType = "B"

                    strSql = " SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = '" & ControlId & "' AND COMPANYID = '" & strCompanyId & "'"
                    TranNo = Val(objGPack.GetSqlValue(strSql, , , tran))

                    strSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & TranNo + 1 & "' "
                    strSql += " WHERE CTLID = '" & ControlId & "' AND COMPANYID = '" & strCompanyId & "'"
                    strSql += " AND CONVERT(INT,CTLTEXT) = " & TranNo & ""
                    cmd = New OleDbCommand(strSql, cn, tran)
                    If cmd.ExecuteNonQuery() = 0 Then
                        GoTo GETORDERNO
                    End If
                    strSql = " SELECT DISTINCT ORNO FROM " & cnAdminDb & "..ORMAST"
                    strSql += " WHERE ORNO = '" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & OrType & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString & Val(TranNo + 1).ToString & "'"
                    strSql += " AND COMPANYID = '" & strCompanyId & "'"
                    If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
                        GoTo GETORDERNO
                    End If
                    TranNo = TranNo + 1
                    Ispurbillauto = True
                End If
                If Ispurchase And SEPPURBILL And Ispurbillauto Then
GETPURNO:
                    Ptranno = getPurTranNo()
                    If Ptranno = 0 Then
                        strSql = " SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = 'GEN-PURBILLNO' AND COMPANYID = '" & strCompanyId & "'"
                        Ptranno = Val(objGPack.GetSqlValue(strSql, , , tran))

                        strSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & (Ptranno + 1).ToString & "' "
                        strSql += " WHERE CTLID = 'GEN-PURBILLNO' AND COMPANYID = '" & strCompanyId & "'"
                        strSql += " AND CONVERT(INT,CTLTEXT) = " & Ptranno & ""
                        cmd = New OleDbCommand(strSql, cn, tran)
                        If cmd.ExecuteNonQuery() = 0 Then
                            GoTo GETPURNO
                        End If
                        Ptranno = Ptranno + 1
                        strSql = " SELECT DISTINCT TRANNO FROM " & cnStockDb & "..RECEIPT"
                        strSql += " WHERE TRANTYPE = 'PU' AND TRANNO = '" & Ptranno.ToString & "'"
                        strSql += " AND COMPANYID = '" & strCompanyId & "'"
                        If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then GoTo GETPURNO
                    End If
                Else
                    If Ispurchase Then Ptranno = TranNo
                End If
                If READYITEMBOOKING And ChkReadyOrder.Checked Then OrType = "B"
                If rbtSchemeOrder.Checked Then OrType = "B"
                OrderNo = GetCostId(BillCostId) & GetCompanyId(strCompanyId) & OrType & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString & TranNo
            End If

            If defalutDestination <> Nothing Then
                If IO.Directory.Exists(defalutDestination) Then
                    Try
                        For cnt As Integer = 0 To gridOrder.RowCount - 1
                            With gridOrder.Rows(cnt)
                                If .Cells("ENTFLAG").Value.ToString = "" Then Continue For
                                If .Cells("STYLENO").Value.ToString <> "" Then Continue For
                                If IO.File.Exists(.Cells("PICTPATH").Value.ToString) Then
                                    Dim iio As New IO.FileInfo(.Cells("PICTPATH").Value.ToString)
                                    Dim extension As String = iio.Extension
                                    Dim destFilePath As String = defalutDestination & OrderNo & "_" & (cnt + 1).ToString + extension
                                    IO.File.Copy(.Cells("PICTPATH").Value.ToString, destFilePath, True)
                                End If
                            End With
                        Next
                    Catch ex As Exception
                        tran.Rollback()
                        tran.Dispose()
                        tran = Nothing
                        MsgBox(ex.Message)
                        Exit Sub
                    End Try
                End If
            End If
            If Mid(DRSMAINTAIN, 1, 1) <> "N" Then objAddressDia.InsertintoAchead(tran)
            GLEDGERCODE = objAddressDia.GLEDGERCODE
            If OrdType = OrderType.OrderEntry Then
                objAddressDia.InsertIntoPersonalInfo(BillDate, BillCostId, Batchno, tran)
            End If


            For index As Integer = 0 To gridOrder.RowCount - 1
                With gridOrder.Rows(index)
                    If .Cells("ENTFLAG").Value.ToString = "" Then Exit For
                    InsertOrderDetail(index)
                End With
            Next
            If OrdType = OrderType.OrderEntry Then
                If InsertAccountsDetail(Ptranno) Then Exit Sub
            End If


            Dim balAmt As Double = Val(objGPack.GetSqlValue("SELECT SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE -1*AMOUNT END) AS BALANCE FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = '" & Batchno & "'", , "0", tran))
            If balAmt <> 0 Then
                tran.Rollback()
                tran.Dispose()
                tran = Nothing
                MsgBox("Credit Debit Not Tally : " + balAmt.ToString, MsgBoxStyle.Information)
                txtAdjCash_AMT.Focus()
                Exit Sub
            End If
            tran.Commit()
            tran = Nothing
            Dim msgb As String = "Order No : " & Mid(OrderNo, 6, 20)
            If Ptranno <> 0 Then msgb = msgb & vbCrLf & "Purchase No." & Ptranno.ToString
            MsgBox(msgb)
            Dim pBatchno As String = Batchno
            Dim pBilldate As Date = BillDate.ToString("yyyy-MM-dd")
            If OrdType = OrderType.OrderEntry Then
                If objAddressDia.chkSendSms.Checked And objAddressDia.txtAddressMobile.Text <> "" Then
                    Dim t As New Threading.Thread(AddressOf SendSms)
                    t.Start()
                End If
            End If
            If SMS_MSG_ORDBOOK <> "" And OrdType = OrderType.OrderEntry Then
                Dim TempMsg As String = ""
                TempMsg = SMS_MSG_ORDBOOK
                TempMsg = Replace(SMS_MSG_ORDBOOK, vbCrLf, "")
                TempMsg = Replace(TempMsg, "<NAME>", IIf(objAddressDia.cmbAddressTitle_OWN.Text <> "", objAddressDia.cmbAddressTitle_OWN.Text, "") & " " & objAddressDia.txtAddressName.Text)
                TempMsg = Replace(TempMsg, "<ORDERNO>", Mid(OrderNo, 6, 20))
                TempMsg = Replace(TempMsg, "<ORDERDATE>", BillDate.ToString("dd.MM.yyyy"))
                SmsSend(TempMsg, objAddressDia.txtAddressMobile.Text)
            End If
            btnNew_Click(Me, New EventArgs)
            CallBillPrint(pBatchno, pBilldate)
            If OrdType = OrderType.OrderEdit Then
                Me.Close()
            End If
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            If Not tran Is Nothing Then tran.Dispose()
        End Try
    End Sub

    Private Function getPurTranNo() As Long
        strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BILLNO'"
        strSql = " IF (SELECT 1 FROM " & cnAdminDb & "..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "BILLNO') > 0"
        strSql += vbCrLf + "  DROP TABLE " & cnAdminDb & "..TEMP" & systemId & "BILLNO"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()

        For Each ro As DataRow In objOrderAdvance.dtGridPur.Rows
            If ro!ENTFLAG.ToString = "" Then Continue For
            If ro.Item("MODE").ToString = "PURCHASE" Then


                Dim PuTranNo As Long = GetBillNo("PU" _
                , objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & ro!CATNAME.ToString & "'", , , tran)
                )
                Return PuTranNo
            End If
        Next
    End Function



    Public Function GetBillNo(ByVal type As String, Optional ByVal CategoryId As String = Nothing) As Long
        Dim billNo As Long = Nothing

SALESPURCHASE:
        Dim issType As String = "PUR"
        Dim metId As String = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & CategoryId & "'", , , tran)
        Dim taxId As String = objGPack.GetSqlValue("SELECT TAXID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & CategoryId & "'", , , tran)

        ''CASH COUNTER
        strSql = "SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL"
        strSql += " WHERE CTLID = 'COU-" & BillCashCounterId & "-" & issType & "'"
        strSql += " AND COMPANYID = '" & strCompanyId & "'"
        If strBCostid <> Nothing Then strSql += " AND COSTID = '" & strBCostid & "'"
        If UCase(objGPack.GetSqlValue(strSql, , , tran)) = "Y" Then
            Return GetBillNoValue("COU-" & BillCashCounterId & "-" & issType & "", tran)
        End If
        ''METALID
        strSql = "SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL"
        strSql += " WHERE CTLID = 'MET-" & metId & "-" & issType & "'"
        strSql += " AND COMPANYID = '" & strCompanyId & "'"
        If strBCostid <> Nothing Then strSql += " AND COSTID = '" & strBCostid & "'"
        If UCase(objGPack.GetSqlValue(strSql, , , tran)) = "Y" Then
            Return GetBillNoValue("MET-" & metId & "-" & issType & "", tran)
        End If
        ''TAXTYPE
        strSql = "SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL"
        strSql += " WHERE CTLID = 'TAX-" & taxId & "-" & issType & "'"
        strSql += " AND COMPANYID = '" & strCompanyId & "'"
        If strBCostid <> Nothing Then strSql += " AND COSTID = '" & strBCostid & "'"
        If UCase(objGPack.GetSqlValue(strSql, , , tran)) = "Y" Then
            Return GetBillNoValue("TAX-" & taxId & "-" & issType & "", tran)
        End If
        ''CATTYPE

        strSql = "SELECT CTLMODE FROM " & cnStockDb & "..BILLCONTROL"
        strSql += " WHERE CTLID = 'CAT-" & CategoryId & "-" & issType & "'"
        strSql += " AND COMPANYID = '" & strCompanyId & "'"
        If strBCostid <> Nothing Then strSql += " AND COSTID = '" & strBCostid & "'"
        If UCase(objGPack.GetSqlValue(strSql, , , tran)) = "Y" Then
            Return GetBillNoValue("CAT-" & CategoryId & "-" & issType & "", tran)
        Else
            Return GetBillNoValue("GEN-PURBILLNO", tran)
        End If
    End Function


    Private Sub CallBillPrint(ByVal pBatchno As String, ByVal pBilldate As Date)
        Dim BillPrintExe As Boolean = IIf(GetAdmindbSoftValue("CALLBILLPRINTEXE", "Y") = "Y", True, False)
        Dim BillPrint_Format As String = GetAdmindbSoftValue("BILLPRINT_FORMAT", "")
        If GST And BillPrint_Format = "M1" Then
            Dim obj As New BrighttechREPORT.frmBillPrintDocA4N("POS", pBatchno.ToString, pBilldate.ToString("yyyy-MM-dd"), "")
            Me.Refresh()
        ElseIf GST And BillPrint_Format = "M2" Then
            Dim obj As New BrighttechREPORT.frmBillPrintDocB5("POS", pBatchno.ToString, pBilldate.ToString("yyyy-MM-dd"), "")
            Me.Refresh()
        ElseIf GST And BillPrint_Format = "M3" Then
            Dim obj As New BrighttechREPORT.frmBillPrintDocA5("POS", pBatchno.ToString, pBilldate.ToString("yyyy-MM-dd"), "")
            Me.Refresh()
        ElseIf GST And BillPrint_Format = "M4" Then
            Dim obj As New BrighttechREPORT.frmBillPrintDocB52cpy("POS", pBatchno.ToString, pBilldate.ToString("yyyy-MM-dd"), "")
            Me.Refresh()
        ElseIf GST And BillPrintExe = False Then
            Dim billDoc As New frmBillPrintDoc("POS", pBatchno.ToString, pBilldate.ToString("yyyy-MM-dd"), "")
            Me.Refresh()
        Else
            If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                Dim write As IO.StreamWriter
                Dim prnmemsuffix As String = ""
                If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
                Dim memfile As String = "\BillPrint" + prnmemsuffix.Trim & ".mem"
                write = IO.File.CreateText(Application.StartupPath & memfile)
                write.WriteLine(LSet("TYPE", 15) & ":ORD")
                write.WriteLine(LSet("BATCHNO", 15) & ":" & pBatchno)
                write.WriteLine(LSet("TRANDATE", 15) & ":" & pBilldate.ToString("yyyy-MM-dd"))
                write.WriteLine(LSet("DUPLICATE", 15) & ":N")
                write.Flush()
                write.Close()
                If EXE_WITH_PARAM = False Then
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                Else
                    System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe",
                    LSet("TYPE", 15) & ":ORD" & ";" &
                    LSet("BATCHNO", 15) & ":" & pBatchno & ";" &
                    LSet("TRANDATE", 15) & ":" & pBilldate.ToString("yyyy-MM-dd") & ";" &
                    LSet("DUPLICATE", 15) & ":N")
                End If

            Else
                MsgBox("Billprint exe not found", MsgBoxStyle.Information)
            End If
        End If
    End Sub

    Private Sub txtSalesMan_NUM_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSalesMan_NUM.GotFocus
        Main.ShowHelpText("Press Insert Key to Help")
    End Sub

    Private Sub txtSalesMan_NUM_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSalesMan_NUM.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadSalesMan()
        End If
    End Sub

    Private Sub LoadSalesMan()
        strSql = " SELECT EMPID,EMPNAME FROM " & cnAdminDb & "..EMPMASTER "
        strSql += " WHERE ACTIVE = 'Y'"
        Dim row As DataRow = Nothing
        row = BrighttechPack.SearchDialog.Show_R("Select Employee Name", strSql, cn, 1)
        If Not row Is Nothing Then
            txtSalesMan_NUM.Text = row!EMPID.ToString
            txtSalesManName.Text = row!EMPNAME.ToString
            If QuickOrder Then
                txtAdjCash_AMT.Select()
            Else
                If READYITEMBOOKING And ChkReadyOrder.Checked Then
                    EstimateToolStripMenuItem_Click(Me, New EventArgs)
                Else
                    Me.SelectNextControl(txtSalesMan_NUM, True, True, True, True)
                End If
            End If
        End If
    End Sub

    Private Sub ClearItemDetails()
        If txtOItemId.Text = "" Then
            txtOItem.Clear()
        End If
        calcType = Nothing
        subItemName = Nothing
        itemTypeName = Nothing
        CartId = Nothing
        isStone = False
        txtOParticular.Clear()
        txtOStyleNo.Clear()
        txtOPcs_NUM.Clear()
        txtOGrsWt_WET.Clear()
        txtONetWt_WET.Clear()
        txtOSize.Clear()
        txtORate_AMT.Clear()
        txtOWastage_WET.Clear()
        txtOMc_AMT.Clear()
        txtOCommision_AMT.Clear()
        txtODiaPcsWt.Clear()
        txtOOtherAmt_AMT.Clear()
        txtOGrossAmount_AMT.Clear()
        txtOVat_AMT.Clear()
        txtOAmount_AMT.Clear()
        txtODescription.Clear()
        txtOWastagePer_Per.Clear()
        txtOMcPerGrm_AMT.Clear()
        txtOCommGrm_AMT.Clear()
        picPath = Nothing
        AutoImageSizer(My.Resources.no_photo, picImage, PictureBoxSizeMode.StretchImage)
        'picImage.Image = My.Resources.no_photo

        objStone = New frmStoneDia


        'dtGridStone.Rows.Clear()
        'CalcStoneWtAmount()
        dtGridSample.Rows.Clear()

        chkOSample.Checked = False
        chkOImage.Checked = False
        btnBrowse.Visible = True
        grpImage.Visible = False
    End Sub

    Private Function GetOrderRate() As String
        Dim rate As Double = Val(GetRate(BillDate, objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtOItem.Text & "'")))
        If itemTypeName <> Nothing Then
            Dim PURITYID As String = objGPack.GetSqlValue("SELECT PURITYID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & itemTypeName & "' AND RATEGET = 'Y'", , )
            If PURITYID <> "" Then rate = Val(GetRate_Purity(BillDate, PURITYID))
        End If
        Return IIf(rate <> 0, Format(rate, "0.00"), Nothing)
    End Function

    Private Sub LoadStoneItemName()
        strSql = " SELECT"
        strSql += " ITEMID AS ITEMID,ITEMNAME AS ITEMNAME"
        strSql += " FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ISNULL(STUDDED,'') = 'S' AND ACTIVE = 'Y'"
        strSql += GetItemQryFilteration()
        Dim itemName As String = BrighttechPack.SearchDialog.Show("Find ItemName", strSql, cn, 1, 1, , txtStItem.Text)
        If itemName <> "" Then
            txtStItem.Text = itemName
            LoadStoneitemDetails()
        Else
            txtStItem.Focus()
            txtStItem.SelectAll()
        End If
    End Sub

    Private Sub LoadStoneitemDetails()
        If objGPack.GetSqlValue("SELECT SUBITEM FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "'" & GetItemQryFilteration()) = "Y" Then
            Dim DefItem As String = txtStSubItem.Text
            Dim iId As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "'"))
            If Not _SubItemOrderByName Then
                DefItem = objGPack.GetSqlValue("SELECT DISPLAYORDER FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtStSubItem.Text & "' AND ITEMID = " & iId & "")
            End If
            strSql = GetSubItemQry(New String() {"SUBITEMID ID", "DISPLAYORDER DISP", "SUBITEMNAME SUBITEM"}, iId)
            txtStSubItem.Text = BrighttechPack.SearchDialog.Show("Search SubItem", strSql, cn, IIf(_SubItemOrderByName, 2, 1), 2, , DefItem, , False, True)
            'Dim qry As String = "SELECT SUBITEMID ID,SUBITEMNAME SUBITEM FROM " & cnAdminDb & "..SUBITEMMAST "
            'qry += " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')"
            'qry += " AND ACTIVE = 'Y'"
            'txtStSubItem.Text = BrighttechPack.SearchDialog.Show("Search SubItem", qry, cn, 1, 1, , txtStSubItem.Text, , False, True)
        End If

        If txtStSubItem.Text <> "" Then
            strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtStSubItem.Text & "' AND ITEMID = (sELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')"
        Else
            strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "'"
        End If
        Dim calType As String = objGPack.GetSqlValue(strSql, , , tran)
        cmbStCalc.Text = IIf(calType = "R", "P", "W")

        strSql = " SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "'"
        txtStMetalCode.Text = objGPack.GetSqlValue(strSql, "DIASTONE", , tran)
        If txtStMetalCode.Text = "T" Then cmbStUnit.Text = "G" Else cmbStUnit.Text = "C"
        Me.SelectNextControl(txtStItem, True, True, True, True)
    End Sub

    Private Sub LoadTagdetails(ByVal Rmtagno As String)
        Dim dtTagdet As New DataTable
        strSql = " DECLARE @DEFPATH VARCHAR(200) "
        strSql += " SELECT @DEFPATH = '" & defaultPic & "'"
        strSql += "SELECT T.SNO, IT.ITEMNAME AS ITEMNAME,SIT.SUBITEMNAME,T.SUBITEMID,PCS, T.GRSWT, NETWT,LESSWT, RATE  ,SALVALUE,ISNULL(T.TOUCH,0) SATOUCH, "
        strSql += "DESIGNERID, ITEMCTRID, ITEMTYPEID ,PURITY,ISNULL(P.PURTOUCH,0) TOUCH, "
        strSql += "CASE WHEN T.TABLECODE = '' THEN '0' ELSE T.TABLECODE END AS TABLECODE, GRSNET, SALEMODE as CALCTYPE  ,"
        strSql += "T.SUBITEMID,ISNULL(MAXWASTPER,0) MAXWASTPER,isnull(MAXWAST,0) MAXWAST,isnull(MAXMCGRM,0) MAXMCGRM ,isnull(MAXMC,0) MAXMC,"
        strSql += " CASE WHEN T.PCTFILE <> '' THEN @DEFPATH + T.PCTFILE END AS PCTFILE,T.SUBITEMID,MINMC,MINWAST ,"
        strSql += "ISNULL(ST.SALESTAX,0) SALESTAX,"
        strSql += "MINWASTPER,MINWAST,MINMCGRM,MINMC ,"
        strSql += "SUM(ISNULL(TS.STNAMT,0)) AS STNAMT ,"
        strSql += "SUM(ISNULL(MC.AMOUNT,0)) AS MISCAMT ,"
        strSql += "CASE WHEN DIT.DIASTONE='D' THEN SUM(ISNULL(TS.STNPCS,0)) END AS DIAPCS,"
        strSql += "CASE WHEN DIT.DIASTONE='D' THEN SUM(ISNULL(TS.STNWT,0)) END AS DIAWT"
        strSql += " FROM " & cnAdminDb & "..ITEMTAG AS T "
        strSql += " LEFT JOIN " & cnAdminDb & "..PURITEMTAG P ON P.TAGSNO = T.SNO"
        strSql += " LEFT JOIN " & cnAdminDb & "..ITEMMAST IT ON IT.ITEMID = T.ITEMID"
        strSql += " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SIT ON SIT.SUBITEMID = T.SUBITEMID"
        strSql += " LEFT JOIN " & cnAdminDb & "..CATEGORY ST ON IT.CATCODE = ST.CATCODE "
        strSql += " LEFT JOIN " & cnAdminDb & "..ITEMTAGSTONE TS ON TS.TAGSNO = T.SNO "
        strSql += " LEFT JOIN " & cnAdminDb & "..ITEMMAST DIT ON DIT.ITEMID = TS.STNITEMID"

        strSql += " LEFT JOIN " & cnAdminDb & "..ITEMTAGMISCCHAR MC ON MC.TAGSNO = T.SNO"
        strSql += " WHERE T.ITEMID = " & Val(txtOItemId.Text) & " AND T.TAGNO = '" & Rmtagno & "'"
        strSql += vbCrLf + " AND T.COSTID = '" & BillCostId & "'"
        'If COMPANYTAG Then strSql += " AND T.COMPANYID = '" & GetStockCompId() & "'"
        strSql += " GROUP BY IT.ITEMNAME,SIT.SUBITEMNAME,T.SUBITEMID,PCS, T.GRSWT, NETWT,LESSWT, RATE  ,SALVALUE,T.TOUCH,DESIGNERID, ITEMCTRID, ITEMTYPEID "
        strSql += ",PURITY,P.PURTOUCH,T.TABLECODE, GRSNET,SALEMODE,MAXWASTPER,MAXWAST,MAXMCGRM ,MAXMC,T.PCTFILE,MINMC,MINWAST"
        strSql += ",SALESTAX,MINWASTPER,MINWAST,MINMCGRM,MINMC,T.SNO,DIT.DIASTONE  "

        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTagdet)
        If Not dtTagdet.Rows.Count > 0 Then
            MsgBox(E0004, MsgBoxStyle.Information)
            Exit Sub
        End If
        With dtTagdet.Rows(0)
            '.Cells("SUBITEMID").Value = roEst.Item("SUBITEMID")
            '.Cells("TAGNO").Value = roEst.Item("TAGNO").ToString
            txtOItem.Text = .Item("ITEMNAME").ToString
            txtOParticular.Text = .Item("SUBITEMNAME").ToString
            txtODescription.Text = .Item("SUBITEMNAME").ToString
            txtOPcs_NUM.Text = Val(.Item("PCS").ToString)
            txtOGrsWt_WET.Text = Format(.Item("GRSWT"), "0.000")
            txtONetWt_WET.Text = Format(.Item("NETWT"), "0.000")

            If .Item("SUBITEMNAME").ToString = "" Then
                strSql = " SELECT ISNULL(MCCALC,'N') FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Item("ITEMNAME").ToString.Trim & "'"
            Else
                strSql = " SELECT ISNULL(MCCALC,'N') FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & .Item("SUBITEMNAME").ToString.Trim & "' AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Item("ITEMNAME").ToString.Trim & "')"
            End If
            If objGPack.GetSqlValue(strSql, , "N") = "G" Then
                _MCCALCON_ITEM_GRS = True
            Else
                _MCCALCON_ITEM_GRS = False
            End If
            If _WASTONGRSNET = False Then
                Dim was As Double = Nothing
                Dim wasPer As Double = (Val(.Item("MAXWASTPER").ToString) / 100)
                txtOWastagePer_Per.Text = IIf(Val(.Item("MAXWASTPER").ToString) > 0, Val(.Item("MAXWASTPER").ToString), "")
                was = IIf(_MCCALCON_ITEM_GRS, Val(.Item("GRSWT").ToString), Val(.Item("NETWT").ToString)) * wasPer
                If WASTAGECEILING_FLOOR = "C" Then was = WTCEILINGCALC(was, WastageRound) Else was = Math.Round(was, WastageRound)
                txtOWastage_WET.Text = was.ToString
            Else
                txtOWastage_WET.Text = IIf(Val(.Item("MAXWAST").ToString) > 0, Val(.Item("MAXWAST").ToString), "")
                txtOWastagePer_Per.Text = IIf(Val(.Item("MAXWASTPER").ToString) > 0, Val(.Item("MAXWASTPER").ToString), "")
            End If
            '.Cells("RATE").Value = IIf(Format(roEst.Item("RATE"), "0.000") <> 0, Format(roEst.Item("RATE"), "0.00"), DBNull.Value)
            If _MCONGRSNET = False Then
                Dim wast As Decimal = Val(txtOWastage_WET.Text.ToString & "")
                Dim mc As Decimal = 0
                mc = IIf(_MCCALCON_ITEM_GRS, (Val(dtTagdet.Rows(0).Item("GRSWT").ToString) + wast) * Val(dtTagdet.Rows(0).Item("MAXMCGRM").ToString),
                   (Val(dtTagdet.Rows(0).Item("NETWT").ToString) + wast) * Val(dtTagdet.Rows(0).Item("MAXMCGRM").ToString))
                mc = Math.Round(mc, McRound)
                txtOMc_AMT.Text = mc.ToString
            Else
                txtOMcPerGrm_AMT.Text = IIf(Val(.Item("MAXMCGRM").ToString) > 0, Val(.Item("MAXMCGRM").ToString), "")
                txtOMc_AMT.Text = IIf(Val(.Item("MAXMC").ToString) > 0, Val(.Item("MAXMC").ToString), "")
            End If


            '   txtSalesMan_NUM.Text = Val(roEst.Item("empid"))
            txtORate_AMT.Text = GetOrderRate()

            strSql = " SELECT 0 KEYNO"
            strSql += " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = STNITEMID)AS ITEM"
            strSql += " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = STNSUBITEMID)AS SUBITEM"
            strSql += " ,STNPCS PCS,STNWT WEIGHT,STONEUNIT UNIT,CALCMODE CALC,STNRATE RATE,STNAMT AMOUNT,''METALID,STNPCS TAGSTNPCS,STNWT TAGSTNWT,TAGSNO"
            strSql += " ,CUTID,COLORID,CLARITYID,SHAPEID,SETTYPEID,HEIGHT,WIDTH"
            strSql += " FROM " & cnAdminDb & "..ITEMTAGSTONE"
            strSql += " WHERE TAGSNO = '" & .Item("SNO").ToString & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(objStone.dtGridStone)
            objStone.CalcStoneWtAmount()

            txtOOtherAmt_AMT.Text = Val(objStone.dtGridStone.Compute("sum(AMOUNT)", Nothing).ToString)
            ''Stone
            'Dim index As Integer = 0
            'For rwIndex As Integer = 0 To objStone.dtGridStone.Rows.Count - 1
            '    Dim ro As DataRow = dtStoneDetails.NewRow
            '    ro("KEYNO") = dtGridOrder.Rows(index).Item("KEYNO").ToString
            '    For colIndex As Integer = 2 To objStone.dtGridStone.Columns.Count - 1
            '        ro(colIndex) = objStone.dtGridStone.Rows(rwIndex).Item(colIndex)
            '    Next
            '    dtStoneDetails.Rows.Add(ro)
            'Next
            'dtStoneDetails.AcceptChanges()

            'txtOrderRepNo_KeyPress(Me, New KeyPressEventArgs(Chr(Keys.Enter)))

        End With

    End Sub

    Private Sub LoadItemDetails()
        Dim iId As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtOItem.Text & "'" & GetItemQryFilteration()))
        Itemid = iId
        If UCase(objGPack.GetSqlValue("SELECT SUBITEM FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtOItem.Text & "'" & GetItemQryFilteration())) = "Y" Then
            Dim DefItem As String = subItemName

            If Not _SubItemOrderByName Then
                DefItem = objGPack.GetSqlValue("SELECT DISPLAYORDER FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & subItemName & "' AND ITEMID = " & iId & "")
            End If
            strSql = GetSubItemQry(New String() {"SUBITEMID ID", "DISPLAYORDER DISP", "SUBITEMNAME SUBITEM"}, iId)
            subItemName = BrighttechPack.SearchDialog.Show("Search SubItem", strSql, cn, IIf(_SubItemOrderByName, 2, 1), 2, , DefItem, , False, True)
            'Dim qry As String = "SELECT SUBITEMID ID,SUBITEMNAME SUBITEM FROM " & cnAdminDb & "..SUBITEMMAST "
            'qry += " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtOItem.Text & "')"
            'qry += " AND ACTIVE = 'Y'"
            'subItemName = BrighttechPack.SearchDialog.Show("Search SubItem", qry, cn, 1, 1, , subItemName, , False, True)
            subItemid = Val(objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & subItemName & "' and Itemid = " & Itemid))
        End If

        If UCase(objGPack.GetSqlValue("SELECT TAGTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtOItem.Text & "'")) = "Y" Then
            itemTypeName = BrighttechPack.SearchDialog.Show("Search TagType", "SELECT NAME TAGTYPE FROM " & cnAdminDb & "..ITEMTYPE ORDER BY NAME", cn, 0, 0, , itemTypeName, , False, True)
        End If

        If ManualSize Then
            strSql = " SELECT SIZESTOCK FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtOItem.Text & "'"
            If objGPack.GetSqlValue(strSql) = "Y" Then
                strSql = " SELECT SIZEID,SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtOItem.Text & "')"
                txtOSize.Text = BrighttechPack.SearchDialog.Show("Search SizeName", strSql, cn, 1, 1, , txtOSize.Text, , False, True)
            End If
        End If

        txtORate_AMT.Text = GetOrderRate()

        If subItemName <> Nothing Then
            strSql = " SELECT STUDDEDSTONE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & subItemName & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtOItem.Text & "')"
        Else
            strSql = " SELECT STUDDEDSTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtOItem.Text & "'" & GetItemQryFilteration()
        End If
        If UCase(objGPack.GetSqlValue(strSql)) = "Y" Then
            isStone = True
        Else
            isStone = False
        End If
        grpDescription.Visible = True
        If txtODescription.Text = "" Then
            txtODescription.Text = IIf(subItemName <> "", subItemName, txtOItem.Text)
        End If
        txtODescription.Focus()
        'Me.SelectNextControl(txtOItemName, True, True, True, True)
    End Sub

    Private Sub LoadSamItemDetails()
        If txtSamDescription.Text = "" Then
            txtSamDescription.Text = txtSamItem.Text
        End If
        Me.SelectNextControl(txtSamItem, True, True, True, True)
    End Sub

    Private Sub LoadSamItemName()
        strSql = " SELECT"
        strSql += " ITEMID AS ITEMID,ITEMNAME AS ITEMNAME"
        strSql += " FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ACTIVE = 'Y'"
        strSql += GetItemQryFilteration()
        'strsql += " AND ISNULL(STUDDED,'') <> 'S'
        Dim itemName As String = BrighttechPack.SearchDialog.Show("Find ItemName", strSql, cn, 1, 1, , txtSamItem.Text)
        If itemName <> "" Then
            txtSamItem.Text = itemName
            LoadSamItemDetails()
        Else
            txtSamItem.Focus()
            txtSamItem.SelectAll()
        End If
    End Sub

    Private Function LoadItemName()
        If txtOItemId.Text <> String.Empty Then
            strSql = "SELECT * FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=" & Val(txtOItemId.Text)
            If objGPack.DupCheck(strSql) = False Then
                MsgBox("Item Not Found", , "BrighttechGold")
                txtOItemId.Focus()
                txtOItemId.Clear()
                Return False
                Exit Function
            Else
                strSql = "SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=" & Val(txtOItemId.Text)
                Oitemname = GetSqlValue(cn, strSql)
                txtOItem.Text = Oitemname
                txtOParticular.Text = Oitemname
                LoadItemDetails()
                Return True
                Exit Function
            End If
        End If
        strSql = " SELECT"
        strSql += " ITEMID AS ITEMID,ITEMNAME AS ITEMNAME,"
        strSql += " CASE WHEN STOCKTYPE = 'T' THEN 'TAGGED' "
        strSql += " WHEN STOCKTYPE = 'N' THEN 'NON TAGGED' ELSE 'POCKET BASED' END AS STOCK_TYPE,"
        strSql += " CASE WHEN CALTYPE = 'W' THEN 'WEIGHT'"
        strSql += " WHEN CALTYPE = 'W' THEN 'WEIGHT'"
        strSql += " WHEN CALTYPE = 'R' THEN 'RATE'"
        strSql += " WHEN CALTYPE = 'F' THEN 'FIXED'"
        strSql += " WHEN CALTYPE = 'B' THEN 'BOTH' ELSE 'METAL RATE' END AS CAL_TYPE"
        strSql += " FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ISNULL(STUDDED,'') <> 'S' AND ACTIVE = 'Y'"
        strSql += GetItemQryFilteration()
        Dim itemId As String = BrighttechPack.SearchDialog.Show("Find ItemId", strSql, cn, 1, 0, , txtOItemId.Text)
        If itemId <> "" Then
            strSql = "SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID='" & itemId & "'"
            Oitemname = GetSqlValue(cn, strSql)
            txtOItem.Text = Oitemname
            txtOParticular.Text = Oitemname
            txtOItemId.Text = itemId
            If Rm_Item_Dly And Readytag = "" Then
                If Gettagno() Then Return True : Exit Function
            End If
            LoadItemDetails()
            Return True
            Exit Function
        Else
            txtOItemId.Focus()
            txtOItemId.SelectAll()
        End If
    End Function

    Private Sub txtSalesMan_NUM_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSalesMan_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtSalesMan_NUM.Text = "" Then
                LoadSalesMan()
            ElseIf txtSalesMan_NUM.Text <> "" Then
                txtSalesManName.Text = objGPack.GetSqlValue("SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = " & Val(txtSalesMan_NUM.Text) & "")
                If txtSalesManName.Text = Nothing Then
                    LoadSalesMan()
                Else
                    If READYITEMBOOKING And ChkReadyOrder.Checked Then
                        EstimateToolStripMenuItem_Click(Me, New EventArgs)
                    Else
                        Me.SelectNextControl(txtSalesMan_NUM, True, True, True, True)
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub txtSalesMan_NUM_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSalesMan_NUM.LostFocus
        Main.HideHelpText()
    End Sub

    Private Sub txtSalesManName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSalesManName.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Function LoadStyleNo() As Boolean
        Dim fileDestPath As String = Nothing
        Dim picBox As New PictureBox
        Dim StyleNoSno As String
        Dim CartSelectedRow As DataRow = Nothing
        If txtOParticular.Text <> "" Then
            ''StyleNo Check
            StyleNoSno = objGPack.GetSqlValue("SELECT SNO FROM " & cnAdminDb & "..TAGCATALOG WHERE STYLENO = '" & txtOParticular.Text & "'", , "-1")
            If StyleNoSno <> "-1" Then
                GoTo LoadStyleDetail
            Else
                StyleNoSno = ""
            End If

        End If
        Dim LoadedCart As String = Nothing
        For Each ro As DataRow In dtGridOrder.Rows
            If Val(ro.Item("ADDCARTID").ToString) > 0 Then
                LoadedCart += Val(ro.Item("ADDCARTID").ToString).ToString & ","
            End If
        Next
        If LoadedCart <> "" Then
            LoadedCart = Mid(LoadedCart, 1, LoadedCart.Length - 1)
        End If
        strSql = vbCrLf + " SELECT  TC.STYLENO"
        strSql += vbCrLf + " ,ST.CARTNAME,CASE WHEN ISNULL(S.SUBITEMNAME,'') = '' THEN I.ITEMNAME ELSE S.SUBITEMNAME END AS DESCRIPTION,TC.PCS,TC.GRSWT,TC.NETWT"
        strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..TAGCATALOGSTONE WHERE TAGSNO = TC.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAPCS"
        strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..TAGCATALOGSTONE WHERE TAGSNO = TC.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAWT"
        strSql += vbCrLf + " ,TC.PCTFILE AS PCTFILE_HIDE,ST.SID SID_HIDE,ST.CATSNO CATSNO_HIDE"
        strSql += vbCrLf + " FROM " & cnStockDb & "..STYLECARD AS ST"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..TAGCATALOG AS TC ON ST.CATSNO = TC.SNO"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS I ON I.ITEMID = TC.ITEMID"
        strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..SUBITEMMAST AS S ON S.SUBITEMID = TC.SUBITEMID AND S.ITEMID = TC.ITEMID"
        If LoadedCart <> "" Then
            strSql += vbCrLf + " WHERE ST.SID NOT IN (" & LoadedCart & ")"
        End If
        strSql += " ORDER BY ST.CARTNAME,ST.SID"
        Dim dtGrid As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If dtGrid.Rows.Count > 0 Then
            Select Case MessageBox.Show("Do you want to load Selected Catalog Info", "Catalog Cart Selection", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
                Case Windows.Forms.DialogResult.Yes
                    dtGrid.Columns.Add("SAMPLE", GetType(Image))
                    picBox.Size = New Size(100, 100)
                    For Each ro As DataRow In dtGrid.Rows
                        If ro("PCTFILE_HIDE").ToString = "" Then
                            ro("SAMPLE").Value = My.Resources.EmptyImage
                            Continue For
                        End If
                        fileDestPath = defaultPic & ro("PCTFILE_HIDE").ToString
                        If IO.File.Exists(fileDestPath) Then
                            AutoImageSizer(fileDestPath, picBox, PictureBoxSizeMode.CenterImage)
                            ro("SAMPLE") = picBox.Image
                        Else
                            ro("SAMPLE") = My.Resources.EmptyImage
                        End If
                    Next
                    Dim objCart As New CatalogCart(dtGrid)
                    If objCart.ShowDialog = Windows.Forms.DialogResult.OK Then
                        CartSelectedRow = objCart.dtGrid.Rows(objCart.DataGridView1.CurrentRow.Index) ' BrighttechPack.SearchDialog.Show_R("Select Cart Details", strSql, cn, 0, 3, , , , , True, True)
                        If CartSelectedRow IsNot Nothing Then
                            StyleNoSno = CartSelectedRow.Item("CATSNO_HIDE").ToString
                            GoTo LoadStyleDetail
                        End If
                    End If
                Case Windows.Forms.DialogResult.Ignore
                    Return True
            End Select
        End If

        strSql = vbCrLf + "  SELECT T.STYLENO,"
        strSql += vbCrLf + "  I.ITEMNAME,S.SUBITEMNAME,T.PCS,T.GRSWT,T.NETWT"
        strSql += vbCrLf + "  ,(sELECT sum(STNWT) FROM " & cnAdminDb & "..TAGCATALOGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAWT"
        strSql += vbCrLf + "  ,T.NARRATION,T.PCTFILE,t.sno as sno_HIDE"
        strSql += vbCrLf + "  FROM " & cnAdminDb & "..TAGCATALOG AS T"
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS I ON I.ITEMID = T.ITEMID"
        strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS S ON S.ITEMID = T.ITEMID AND S.SUBITEMID = T.SUBITEMID"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Exit Function
        End If


        StyleNoSno = BrighttechPack.SearchDialog.Show("Select StyleNo", dt, cn, 0, 9, , txtOStyleNo.Text, , , , , True)
        If StyleNoSno <> "" Then
LoadStyleDetail:
            strSql = " SELECT C.STYLENO,I.ITEMNAME,S.SUBITEMNAME,T.NAME AS ITEMTYPE,C.* "
            strSql += " FROM " & cnAdminDb & "..TAGCATALOG AS C "
            strSql += " INNER JOIN " & cnAdminDb & "..ITEMMAST AS I ON I.ITEMID = C.ITEMID"
            strSql += " LEFT OUTER JOIN " & cnAdminDb & "..SUBITEMMAST AS S ON S.ITEMID = C.ITEMID AND S.SUBITEMID = C.SUBITEMID"
            strSql += " LEFT OUTER JOIN " & cnAdminDb & "..ITEMTYPE AS T ON T.ITEMTYPEID = C.ITEMTYPEID"
            strSql += " WHERE C.SNO = '" & StyleNoSno & "'"
            Dim dtTagCatalogDet As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTagCatalogDet)
            If dtTagCatalogDet.Rows.Count > 0 Then
                ClearItemDetails()
                Dim row As DataRow = dtTagCatalogDet.Rows(0)
                txtOParticular.Text = row.Item("STYLENO").ToString
                txtOItem.Text = row.Item("ITEMNAME").ToString
                If CartSelectedRow IsNot Nothing Then CartId = CartSelectedRow.Item("SID_HIDE").ToString
                txtOStyleNo.Text = row.Item("STYLENO").ToString
                subItemName = row.Item("SUBITEMNAME").ToString
                itemTypeName = row.Item("ITEMTYPE").ToString
                txtOSize.Text = row.Item("SIZEDESC").ToString
                txtOPcs_NUM.Text = row.Item("PCS").ToString
                txtOGrsWt_WET.Text = row.Item("GRSWT").ToString
                txtONetWt_WET.Text = row.Item("NETWT").ToString
                txtORate_AMT.Text = GetOrderRate()
                Dim ItemId As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtOItem.Text & "'"))
                Dim SubItemId As Integer = Nothing
                If subItemName <> Nothing Then
                    SubItemId = Val(objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & subItemName & "' AND ITEMID = " & ItemId & ""))
                    strSql = " SELECT STUDDEDSTONE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & subItemName & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtOItem.Text & "')"
                Else
                    strSql = " SELECT STUDDEDSTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtOItem.Text & "'"
                End If
                If UCase(objGPack.GetSqlValue(strSql)) = "Y" Then
                    isStone = True
                Else
                    isStone = False
                End If
                If isStone Then
                    strSql = " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPCATSTONE')>0 DROP TABLE TEMPCATSTONE"
                    strSql += vbCrLf + " SELECT IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM,TS.STNPCS PCS,TS.STNWT WEIGHT,TS.STONEUNIT UNIT,TS.CALCMODE CALC"
                    strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL)RATE,CONVERT(NUMERIC(15,2),NULL)AMOUNT,IM.DIASTONE AS METALID,STNITEMID AS ITEMID,STNSUBITEMID AS SUBITEMID"
                    strSql += vbCrLf + " INTO TEMPCATSTONE"
                    strSql += vbCrLf + " FROM " & cnAdminDb & "..TAGCATALOGSTONE AS TS"
                    strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = TS.STNITEMID"
                    strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = TS.STNITEMID AND SM.SUBITEMID = TS.STNSUBITEMID"
                    strSql += vbCrLf + " WHERE TS.TAGSNO = '" & StyleNoSno & "'"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                    strSql = " UPDATE TEMPCATSTONE SET RATE = C.MAXRATE"
                    strSql += vbCrLf + " FROM TEMPCATSTONE AS T," & cnAdminDb & "..CENTRATE AS C"
                    strSql += vbCrLf + " WHERE (CASE WHEN T.PCS > 0 THEN T.WEIGHT/T.PCS ELSE T.WEIGHT END)*100 BETWEEN C.FROMCENT AND C.TOCENT"
                    strSql += vbCrLf + " AND C.ITEMID = T.ITEMID AND C.SUBITEMID = T.SUBITEMID"
                    strSql += vbCrLf + " AND ISNULL(C.ACCODE,'') = '" & objAddressDia.txtAddressPartyCode.Text & "'"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()

                    strSql = " UPDATE TEMPCATSTONE SET AMOUNT = CASE WHEN CALC = 'W' THEN WEIGHT * RATE ELSE PCS * RATE END"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()

                    strSql = " SELECT ITEM,SUBITEM,PCS,WEIGHT,UNIT,CALC,RATE,AMOUNT,METALID"
                    strSql += vbCrLf + " FROM TEMPCATSTONE"
                    Dim dtSt As New DataTable
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(dtSt)
                    Dim RoStDia As DataRow = Nothing
                    For Each RoSt As DataRow In dtSt.Rows
                        RoStDia = objStone.dtGridStone.NewRow
                        For Each col As DataColumn In dtSt.Columns
                            RoStDia.Item(col.ColumnName) = RoSt.Item(col)
                        Next
                        objStone.dtGridStone.Rows.Add(RoStDia)
                    Next
                End If

                strSql = " DECLARE @WT FLOAT"
                strSql += " SET @WT = " & Val(txtONetWt_WET.Text) & ""
                strSql += " SELECT MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC"
                strSql += " ,MINWASTPER,MINMCGRM,MINWAST,MINMC FROM " & cnAdminDb & "..WMCTABLE "
                strSql += " WHERE ITEMID = " & ItemId & ""
                strSql += " AND SUBITEMID = " & SubItemId & ""
                strSql += " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
                strSql += " AND ISNULL(ACCODE,'') = '" & objAddressDia.txtAddressPartyCode.Text & "'"
                strSql += " AND ISNULL(COSTID,'') = '" & BillCostId & "'"
                Dim dtWastMc As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtWastMc)
                If dtWastMc.Rows.Count > 0 Then
                    With dtWastMc.Rows(0)
                        txtOWastagePer_Per.Clear()
                        txtOMcPerGrm_AMT.Clear()
                        txtOWastage_WET.Text = IIf(Val(.Item("MAXWAST").ToString) <> 0, Format(Val(.Item("MAXWAST").ToString), "0.000"), Nothing)
                        txtOMc_AMT.Text = IIf(Val(.Item("MAXMC").ToString) <> 0, Format(Val(.Item("MAXMC").ToString), "0.00"), Nothing)
                        txtOWastagePer_Per.Text = IIf(Val(.Item("MAXWASTPER").ToString) <> 0, Format(Val(.Item("MAXWASTPER").ToString), "0.000"), Nothing)
                        txtOMcPerGrm_AMT.Text = IIf(Val(.Item("MAXMCGRM").ToString) <> 0, Format(Val(.Item("MAXMCGRM").ToString), "0.00"), Nothing)
                    End With
                End If
                Dim defPic As String = GetAdmindbSoftValuefromDt(dtSoftKeys, "TAGCATALOGPATH", Application.ExecutablePath)

                If Not defPic.EndsWith("\") And defPic <> "" Then defPic += "\"
                picPath = defPic & row.Item("PCTFILE").ToString
                AutoImageSizer(picPath, picImage, PictureBoxSizeMode.CenterImage)
                chkOImage.Checked = True
                grpDescription.Visible = True
                grpImage.Visible = True
                btnBrowse.Visible = False
                If txtODescription.Text = "" Then
                    txtODescription.Text = IIf(subItemName <> "", subItemName, txtOItem.Text)
                End If
                txtODescription.Focus()
                Return True
            End If
        End If
    End Function

    Private Sub txtOItemId_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOItemId.LostFocus
        Main.HideHelpText()
    End Sub

    Private Sub txtOItemId_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOItemId.TextChanged
        ClearItemDetails()
    End Sub
    Private Sub txtOItemId_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOItemId.GotFocus
        Main.ShowHelpText("Press Insert Key to Help")
    End Sub
    Private Sub txtOItemId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtOItemId.KeyDown
        If e.KeyCode = Keys.Insert Then
            If Not GetEntFlag() Then
                If _IsWholeSaleType Then
                    If objAddressDia.txtAddressName.Text = "" Then
                        If ShowAddressDia() = False Then Exit Sub
                    End If
                End If
            End If
            If _IsWholeSaleType Then
                If LoadStyleNo() Then Exit Sub
            End If

            If LoadItemName() = False Then Exit Sub
            txtOParticular.Text = Oitemname
        End If
    End Sub

    Private Sub txtOItemId_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtOItemId.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Not GetEntFlag() Then
                If _IsWholeSaleType Then
                    If objAddressDia.txtAddressName.Text = "" Then
                        If ShowAddressDia() = False Then Exit Sub
                    End If
                End If
            End If



            If _IsWholeSaleType Then
                If LoadStyleNo() Then Exit Sub
            End If
            If txtOItemId.Text <> "" And Rm_Item_Dly Then
                If Gettagno() Then Exit Sub
            End If

            If txtOItem.Text = "" Then
                If LoadItemName() = False Then Exit Sub
                txtOParticular.Text = Oitemname
            ElseIf txtOItem.Text <> "" And objGPack.DupCheck("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtOItem.Text & "'") = False Then
                LoadItemName()
            Else
                LoadItemDetails()
            End If

        End If
    End Sub
    Private Function Gettagno() As Boolean
        Dim RmTagno As String
        ObjTagsearch = New TagSearch
        ObjTagsearch.OrdItemid = Val(txtOItemId.Text)
        ObjTagsearch.dtOrdtag = dtGridOrder
        If ObjTagsearch.ShowDialog = Windows.Forms.DialogResult.OK Then
            RmTagno = ObjTagsearch.txtTagNo.Text
            If RmTagno <> "" Then LoadTagdetails(RmTagno) : chkOImage.Focus() : Readytag = RmTagno : Return True
        End If
        Return False
    End Function
    '#Region "Comment by Rajkumar"


    '    Private Sub txtOItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOParticular.GotFocus
    '        Main.ShowHelpText("Press Insert Key to Help")
    '    End Sub
    '    Private Sub txtOItemName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtOParticular.KeyDown
    '        If e.KeyCode = Keys.Insert Then
    '            If Not GetEntFlag() Then
    '                If _IsWholeSaleType Then
    '                    If objAddressDia.txtAddressName.Text = "" Then
    '                        If ShowAddressDia() = False Then Exit Sub
    '                    End If
    '                End If
    '            End If
    '            If _IsWholeSaleType Then
    '                If LoadStyleNo() Then Exit Sub
    '            End If
    '            LoadItemName()
    '        End If
    '    End Sub

    '    Private Sub txtOItemName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtOParticular.KeyPress
    '        If e.KeyChar = Chr(Keys.Enter) Then
    '            If Not GetEntFlag() Then
    '                If _IsWholeSaleType Then
    '                    If objAddressDia.txtAddressName.Text = "" Then
    '                        If ShowAddressDia() = False Then Exit Sub
    '                    End If
    '                End If
    '            End If
    '            If _IsWholeSaleType Then
    '                If LoadStyleNo() Then Exit Sub
    '            End If
    '            If txtOItem.Text = "" Then
    '                LoadItemName()
    '            ElseIf txtOItem.Text <> "" And objGPack.DupCheck("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtOItem.Text & "'") = False Then
    '                LoadItemName()
    '            Else
    '                LoadItemDetails()
    '            End If
    '        End If
    '    End Sub

    '    Private Sub txtOItemName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOParticular.LostFocus
    '        Main.HideHelpText()
    '    End Sub

    '    Private Sub txtOItemName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOParticular.TextChanged
    '        ClearItemDetails()
    '    End Sub
    '#End Region

    Private Sub txtORate_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtORate_AMT.GotFocus
        txtORate_AMT.Text = GetOrderRate()
    End Sub

    Private Sub txtOPcs_NUM_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOPcs_NUM.Leave
        If rbtCompanyOrderType.Checked = False Then Exit Sub
        If Val(txtOPcs_NUM.Text) <> 0 And REORDERCHK = True Then
            If chkreorderstk() = False Then
                If MsgBox("STOCK QTY. EXCEED MAX. QTY. YOU NEED MORE QTY TO PLACE ORDER", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    Dim objSecret As New frmAdminPassword()
                    If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then
                        txtOPcs_NUM.SelectAll()
                        txtOPcs_NUM.Focus()
                    End If
                Else
                    txtOPcs_NUM.Focus()
                    Exit Sub
                End If
            End If
        End If
    End Sub


    Private Sub txtOPcs_NUM_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOPcs_NUM.TextChanged
        CalcGrossAmount()
    End Sub

    Private Sub txtOGrsWt_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtOGrsWt_WET.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            ShowStoneDia()
            ShowOrderAdditionalDetails()
            Me.SelectNextControl(txtOGrsWt_WET, True, True, True, True)
        End If
    End Sub

    Private Sub txtOGrsWt_WET_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOGrsWt_WET.Leave
        If objGPack.GetSqlValue("Select caltype from " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(txtOItemId.Text)).ToString <> "R" Then
            If Val(txtOGrsWt_WET.Text) = 0 Then MsgBox("Please check..Order weight is zero", MsgBoxStyle.Question) : txtOGrsWt_WET.Focus() : Exit Sub
        End If
    End Sub

    Private Sub txtOGrsWt_WET_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOGrsWt_WET.TextChanged
        CalcMc()
        CalcWastage()
        CalcCommision()
        CalcStoneWtAmount()
        CalcGrossAmount()
    End Sub

    Private Sub txtONetWt_WET_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtONetWt_WET.TextChanged
        CalcMc()
        CalcWastage()
        CalcCommision()
        CalcGrossAmount()
    End Sub

    Private Sub txtORate_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtORate_AMT.TextChanged
        CalcGrossAmount()
    End Sub

    Private Sub txtOWastage_WET_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOWastage_WET.GotFocus
        Main.ShowHelpText("Press Insert to Set Wastage Per")
    End Sub

    Private Sub txtOWastage_WET_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtOWastage_WET.KeyDown
        If e.KeyCode = Keys.Insert Then
            ShowOrderExtraDetails(CType(sender, Control))
        End If
    End Sub

    Private Sub txtOWastage_WET_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOWastage_WET.LostFocus
        Main.HideHelpText()
    End Sub



    Private Sub txtOWastage_WET_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOWastage_WET.TextChanged
        CalcGrossAmount()
    End Sub

    Private Sub txtOMc_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOMc_AMT.GotFocus
        Main.ShowHelpText("Press Insert to Set McPerGram Value")
    End Sub

    Private Sub txtOMc_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtOMc_AMT.KeyDown
        If e.KeyCode = Keys.Insert Then
            ShowOrderExtraDetails(CType(sender, Control))
        End If

    End Sub

    Private Sub txtOMc_AMT_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOMc_AMT.LostFocus
        Main.HideHelpText()
    End Sub

    Private Sub txtOMc_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOMc_AMT.TextChanged
        CalcGrossAmount()
    End Sub

    Private Sub txtODiaPcsWt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtODiaPcsWt.GotFocus
        SendKeys.Send("{TAB}")
    End Sub
    Private Sub txtOOtherAmt_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOOtherAmt_AMT.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtOOtherAmt_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOOtherAmt_AMT.TextChanged
        CalcGrossAmount()
    End Sub

    Private Sub txtOGrossAmount_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOGrossAmount_AMT.TextChanged
        CalcVat()
        CalcAmount()
    End Sub

    Private Sub txtOVat_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOVat_AMT.TextChanged
        CalcAmount()
    End Sub

    Private Sub LoadOrderDetail()
        If objGPack.GetSqlValue("SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtOItem.Text & "'") = "" Then
            MsgBox("Invalid ItemName", MsgBoxStyle.Information)
            txtOParticular.Focus()
            Exit Sub
        End If
        If objGPack.GetSqlValue("SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtOItem.Text & "'") <> "R" Then
            If Not Val(txtOGrsWt_WET.Text) > 0 Then
                MsgBox("Grs Weight Should Not Emtpy", MsgBoxStyle.Information)
                txtOGrsWt_WET.Focus()
                Exit Sub
            End If
            If Not Val(txtONetWt_WET.Text) > 0 Then
                MsgBox("NetWeight Should Not Empty", MsgBoxStyle.Information)
                txtONetWt_WET.Focus()
                Exit Sub
            End If
        Else
            If Not Val(txtOPcs_NUM.Text) > 0 Then
                MsgBox("Pcs Should Not Empty", MsgBoxStyle.Information)
                txtOPcs_NUM.Focus()
                Exit Sub
            End If
        End If
        If Not Val(txtORate_AMT.Text) > 0 Then
            MsgBox("Rate Should Not Empty", MsgBoxStyle.Information)
            txtORate_AMT.Focus()
            Exit Sub
        End If
        If txtODescription.Text = "" Then
            MsgBox("Description Should Not Empty", MsgBoxStyle.Information)
            grpDescription.Visible = True
            txtODescription.Focus()
            Exit Sub
        End If
        Dim NoOfRows As Integer = 1
        If chkGenMultiRows.Checked Then
            Dim objMultipleNo As New frmB4Discount
            objMultipleNo.Text = "No Of Entries"
            objMultipleNo.LBLAmountLabel.Text = "No :"
            objMultipleNo.txtSADiscount_AMT.Text = 1
            objMultipleNo.StartPosition = FormStartPosition.CenterScreen
            objMultipleNo.BackColor = Me.BackColor
            objMultipleNo.grpDiscount.BackgroundColor = grpHeader.BackgroundColor
            objMultipleNo.txtSADiscount_AMT.Select()
            If objMultipleNo.ShowDialog() = Windows.Forms.DialogResult.OK Then
                NoOfRows = IIf(Val(objMultipleNo.txtSADiscount_AMT.Text) = 0, 1, Val(objMultipleNo.txtSADiscount_AMT.Text))
            End If
        End If

        For Cnt As Integer = 1 To NoOfRows
            Dim index As Integer = 0
            If txtOrderRowIndex.Text = "" Then
                For i As Integer = 0 To dtGridOrder.Rows.Count - 1
                    With dtGridOrder.Rows(i)
                        If .Item("ENTFLAG").ToString <> "Y" Then
                            .Item("PARTICULAR") = IIf(txtOStyleNo.Text <> "", txtOStyleNo.Text, txtOItem.Text)
                            .Item("ITEMNAME") = txtOItem.Text
                            .Item("ITEMID") = Val(txtOItemId.Text)
                            .Item("PCS") = IIf(Val(txtOPcs_NUM.Text) > 0, txtOPcs_NUM.Text, DBNull.Value)
                            .Item("GRSWT") = IIf(Val(txtOGrsWt_WET.Text) > 0, txtOGrsWt_WET.Text, DBNull.Value)
                            .Item("NETWT") = IIf(Val(txtONetWt_WET.Text) > 0, txtONetWt_WET.Text, DBNull.Value)
                            .Item("SIZENO") = txtOSize.Text
                            .Item("RATE") = IIf(Val(txtORate_AMT.Text) > 0, txtORate_AMT.Text, DBNull.Value)
                            .Item("WASTAGE") = IIf(Val(txtOWastage_WET.Text) > 0, txtOWastage_WET.Text, DBNull.Value)
                            .Item("MC") = IIf(Val(txtOMc_AMT.Text) > 0, txtOMc_AMT.Text, DBNull.Value)
                            .Item("COMM") = IIf(Val(txtOCommision_AMT.Text) <> 0, txtOCommision_AMT.Text, DBNull.Value)
                            .Item("DIAPCSWT") = txtODiaPcsWt.Text
                            .Item("OTHERAMT") = IIf(Val(txtOOtherAmt_AMT.Text) > 0, txtOOtherAmt_AMT.Text, DBNull.Value)
                            .Item("GROSSAMT") = IIf(Val(txtOGrossAmount_AMT.Text) > 0, txtOGrossAmount_AMT.Text, DBNull.Value)
                            .Item("VAT") = IIf(Val(txtOVat_AMT.Text) > 0, txtOVat_AMT.Text, DBNull.Value)
                            .Item("AMOUNT") = IIf(Val(txtOAmount_AMT.Text) > 0, txtOAmount_AMT.Text, DBNull.Value)
                            .Item("SAMPLE") = IIf(chkOSample.Checked, "Y", "N")
                            .Item("IMAGE") = IIf(chkOImage.Checked, "Y", "N")
                            .Item("SAMPLEANDIMAGE") = IIf(chkOSample.Checked, "Y", "N") & IIf(chkOImage.Checked, "Y", "N")
                            .Item("ENTFLAG") = "Y"
                            .Item("TAGNO") = Readytag
                            .Item("SUBITEMNAME") = subItemName
                            .Item("DESCRIPTION") = txtODescription.Text
                            If ManualSize Then
                                .Item("SIZENAME") = txtOSize.Text
                            End If
                            .Item("COMMPER") = Val(txtOCommGrm_AMT.Text)
                            .Item("WASTAGEPER") = Val(txtOWastagePer_Per.Text)
                            .Item("MCGRM") = Val(txtOMcPerGrm_AMT.Text)
                            .Item("STONEAMT") = Val(objStone.gridStoneTotal.Rows(0).Cells("AMOUNT").Value.ToString)
                            .Item("LESSWT") = Val(txtOGrsWt_WET.Text) - Val(txtONetWt_WET.Text)
                            .Item("PICTPATH") = picPath
                            .Item("ITEMTYPE") = itemTypeName
                            .Item("STYLENO") = txtOStyleNo.Text
                            .Item("ADDCARTID") = CartId
                            .Item("METALID") = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Itemid)
                            gridOrder.CurrentCell = gridOrder.Rows(i).Cells("PARTICULAR")
                            index = i
                            dtGridOrder.Rows.Add()
                            Readytag = ""
                            Exit For
                        End If
                    End With
                Next
            Else
                With dtGridOrder.Rows(Val(txtOrderRowIndex.Text))
                    .Item("PARTICULAR") = IIf(txtOStyleNo.Text <> "", txtOStyleNo.Text, txtOItem.Text)
                    .Item("ITEMNAME") = txtOItem.Text
                    .Item("ITEMID") = Val(txtOItemId.Text)
                    .Item("PCS") = IIf(Val(txtOPcs_NUM.Text) > 0, txtOPcs_NUM.Text, DBNull.Value)
                    .Item("GRSWT") = IIf(Val(txtOGrsWt_WET.Text) > 0, txtOGrsWt_WET.Text, DBNull.Value)
                    .Item("NETWT") = IIf(Val(txtONetWt_WET.Text) > 0, txtONetWt_WET.Text, DBNull.Value)
                    .Item("SIZENO") = txtOSize.Text
                    .Item("RATE") = IIf(Val(txtORate_AMT.Text) > 0, txtORate_AMT.Text, DBNull.Value)
                    .Item("WASTAGE") = IIf(Val(txtOWastage_WET.Text) > 0, txtOWastage_WET.Text, DBNull.Value)
                    .Item("MC") = IIf(Val(txtOMc_AMT.Text) > 0, txtOMc_AMT.Text, DBNull.Value)
                    .Item("COMM") = IIf(Val(txtOCommision_AMT.Text) <> 0, txtOCommision_AMT.Text, DBNull.Value)
                    .Item("DIAPCSWT") = txtODiaPcsWt.Text
                    .Item("OTHERAMT") = IIf(Val(txtOOtherAmt_AMT.Text) > 0, txtOOtherAmt_AMT.Text, DBNull.Value)
                    .Item("GROSSAMT") = IIf(Val(txtOGrossAmount_AMT.Text) > 0, txtOGrossAmount_AMT.Text, DBNull.Value)
                    .Item("VAT") = IIf(Val(txtOVat_AMT.Text) > 0, txtOVat_AMT.Text, DBNull.Value)
                    .Item("AMOUNT") = IIf(Val(txtOAmount_AMT.Text) > 0, txtOAmount_AMT.Text, DBNull.Value)
                    .Item("SAMPLE") = IIf(chkOSample.Checked, "Y", "N")
                    .Item("IMAGE") = IIf(chkOImage.Checked, "Y", "N")
                    .Item("SAMPLEANDIMAGE") = IIf(chkOSample.Checked, "Y", "N") & IIf(chkOImage.Checked, "Y", "N")
                    .Item("ENTFLAG") = "Y"
                    .Item("TAGNO") = Readytag
                    .Item("SUBITEMNAME") = subItemName
                    .Item("DESCRIPTION") = txtODescription.Text
                    If ManualSize Then
                        .Item("SIZENAME") = txtOSize.Text
                    End If
                    .Item("COMMPER") = Val(txtOCommGrm_AMT.Text)
                    .Item("WASTAGEPER") = Val(txtOWastagePer_Per.Text)
                    .Item("MCGRM") = Val(txtOMcPerGrm_AMT.Text)
                    .Item("STONEAMT") = Val(objStone.gridStoneTotal.Rows(0).Cells("AMOUNT").Value.ToString)
                    .Item("LESSWT") = Val(txtOGrsWt_WET.Text) - Val(txtONetWt_WET.Text)
                    .Item("PICTPATH") = picPath
                    .Item("ITEMTYPE") = itemTypeName
                    .Item("STYLENO") = txtOStyleNo.Text
                    .Item("ADDCARTID") = CartId
                    .Item("METALID") = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Itemid)
                    gridOrder.CurrentCell = gridOrder.Rows(Val(txtOrderRowIndex.Text)).Cells("PARTICULAR")
                    index = Val(txtOrderRowIndex.Text)
                    dtStoneDetails.AcceptChanges()
                    For Each ro As DataRow In dtStoneDetails.Rows
                        If ro!KEYNO.ToString = .Item("KEYNO").ToString Then
                            ro.Delete()
                        End If
                    Next
                    dtStoneDetails.AcceptChanges()

                    dtSampleDetails.AcceptChanges()
                    For Each ro As DataRow In dtSampleDetails.Rows
                        If ro!KEYNO.ToString = .Item("KEYNO").ToString Then
                            ro.Delete()
                        End If
                    Next
                    dtSampleDetails.AcceptChanges()

                    dtOrderAdditionalDetails.AcceptChanges()
                    For Each ro As DataRow In dtOrderAdditionalDetails.Rows
                        If ro!KEYNO.ToString = .Item("KEYNO").ToString Then
                            ro.Delete()
                        End If
                    Next
                    dtOrderAdditionalDetails.AcceptChanges()
                    Readytag = ""
                End With
            End If
            dtGridOrder.AcceptChanges()
            CalcGridOrderTotal()

            ''Stone
            For rwIndex As Integer = 0 To objStone.dtGridStone.Rows.Count - 1
                Dim ro As DataRow = dtStoneDetails.NewRow
                ro("KEYNO") = dtGridOrder.Rows(index).Item("KEYNO").ToString
                For colIndex As Integer = 2 To objStone.dtGridStone.Columns.Count - 1
                    ro(colIndex) = objStone.dtGridStone.Rows(rwIndex).Item(colIndex)
                Next
                dtStoneDetails.Rows.Add(ro)
            Next
            dtStoneDetails.AcceptChanges()

            ''SAmple/Attachment
            For rwIndex As Integer = 0 To dtGridSample.Rows.Count - 1
                Dim ro As DataRow = dtSampleDetails.NewRow
                ro("KEYNO") = dtGridOrder.Rows(index).Item("KEYNO").ToString
                For colIndex As Integer = 1 To dtGridSample.Columns.Count - 1
                    ro(colIndex) = dtGridSample.Rows(rwIndex).Item(colIndex)
                Next
                dtSampleDetails.Rows.Add(ro)
            Next
            dtSampleDetails.AcceptChanges()
            ''order Additional Details
            For rwIndex As Integer = 0 To objAddtionalDetails.gridView.Rows.Count - 1
                Dim ro As DataRow = dtOrderAdditionalDetails.NewRow
                ro("KEYNO") = dtGridOrder.Rows(index).Item("KEYNO").ToString
                For colIndex As Integer = 1 To objAddtionalDetails.gridView.Columns.Count - 1
                    ro(colIndex) = objAddtionalDetails.DtView.Rows(rwIndex).Item(colIndex)
                Next
                dtOrderAdditionalDetails.Rows.Add(ro)
            Next
            dtOrderAdditionalDetails.AcceptChanges()
        Next Cnt
        ''Clear
        Readytag = ""
        objAddtionalDetails.DtView.Clear()
        txtOItem.Clear()
        'txtOParticular.Clear()
        'txtOParticular.Focus()
        txtOItemId.Clear()
        txtOItemId.Focus()
        CartId = Nothing
        chkGenMultiRows.Checked = False
        If OrdType = OrderType.OrderUpdate Then
            btnSave_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub chkOImage_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chkOImage.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If chkOImage.Checked And defalutDestination <> "" And txtOStyleNo.Text = "" Then
                If Not IO.Directory.Exists(defalutDestination) Then
                    MsgBox(defalutDestination & vbCrLf & " Destination Path not found" & vbCrLf & "Please make the correct path", MsgBoxStyle.Information)
                    chkOImage.Checked = False
                    chkOImage.Select()
                    Exit Sub
                End If
                grpImage.Visible = True
                grpImage.BringToFront()
                btnBrowse.Focus()
            Else
                LoadOrderDetail()
            End If
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub txtODescription_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtODescription.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtODescription.Text = "" Then
                Exit Sub
            End If
            grpDescription.Visible = False
            Me.SelectNextControl(txtOParticular, True, True, True, True)
        End If
    End Sub

    Private Sub gridOrder_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridOrder.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If Not gridOrder.RowCount > 0 Then Exit Sub
            gridOrder.CurrentCell = gridOrder.CurrentRow.Cells("ITEMID")
            With gridOrder.Rows(gridOrder.CurrentRow.Index)
                If .Cells("ENTFLAG").Value.ToString <> "Y" Then Exit Sub
                txtOStyleNo.Text = .Cells("PARTICULAR").FormattedValue
                txtOItem.Text = .Cells("ITEMNAME").FormattedValue
                txtOItemId.Text = .Cells("ITEMID").FormattedValue
                txtOPcs_NUM.Text = .Cells("PCS").FormattedValue
                txtOGrsWt_WET.Text = .Cells("GRSWT").FormattedValue
                txtONetWt_WET.Text = .Cells("NETWT").FormattedValue
                txtOSize.Text = .Cells("SIZENO").FormattedValue
                txtORate_AMT.Text = .Cells("RATE").FormattedValue
                txtOWastage_WET.Text = .Cells("WASTAGE").FormattedValue
                txtOMc_AMT.Text = .Cells("MC").FormattedValue
                txtOCommision_AMT.Text = .Cells("COMM").FormattedValue
                txtODiaPcsWt.Text = .Cells("DIAPCSWT").FormattedValue
                txtOOtherAmt_AMT.Text = .Cells("OTHERAMT").FormattedValue
                txtOGrossAmount_AMT.Text = .Cells("GROSSAMT").FormattedValue
                txtOVat_AMT.Text = .Cells("VAT").FormattedValue
                txtOAmount_AMT.Text = .Cells("AMOUNT").FormattedValue
                chkOSample.Checked = IIf(.Cells("SAMPLE").FormattedValue = "Y", True, False)
                chkOImage.Checked = IIf(.Cells("IMAGE").FormattedValue = "Y", True, False)
                subItemName = .Cells("SUBITEMNAME").FormattedValue
                txtODescription.Text = .Cells("DESCRIPTION").FormattedValue
                txtOSize.Text = .Cells("SIZENAME").FormattedValue
                txtOCommGrm_AMT.Text = .Cells("COMMPER").FormattedValue
                txtOWastagePer_Per.Text = .Cells("WASTAGEPER").FormattedValue
                txtOMcPerGrm_AMT.Text = .Cells("MCGRM").FormattedValue
                picPath = .Cells("PICTPATH").FormattedValue
                itemTypeName = .Cells("ITEMTYPE").FormattedValue
                txtOStyleNo.Text = .Cells("STYLENO").FormattedValue
                CartId = Val(.Cells("ADDCARTID").FormattedValue.ToString)
                For Each roStn As DataRow In dtStoneDetails.Select("KEYNO = " & .Cells("KEYNO").FormattedValue.ToString & "", "TRANTYPE")
                    objStone.dtGridStone.ImportRow(roStn)
                Next
                For Each roStn As DataRow In dtOrderAdditionalDetails.Select("KEYNO = " & .Cells("KEYNO").FormattedValue.ToString & "", "TYPENAME")
                    objAddtionalDetails.DtView.ImportRow(roStn)
                Next
                txtOrderRowIndex.Text = gridOrder.CurrentRow.Index
                txtOItemId.Focus()
                'Me.SelectNextControl(txtOItemId, True, True, False, False)
            End With
        End If
    End Sub

    Private Sub gridOrder_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridOrder.UserDeletedRow
        dtGridOrder.AcceptChanges()
        CalcGridOrderTotal()
    End Sub

    Private Sub gridOrder_UserDeletingRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowCancelEventArgs) Handles gridOrder.UserDeletingRow
        If gridOrder.Rows(e.Row.Index).Cells("EntFlag").Value.ToString = "Y" Then

        End If
        dtGridOrder.Rows.Add()
        dtGridOrder.AcceptChanges()
    End Sub

    Private Sub txtStItem_MAN_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStItem.GotFocus
        Main.ShowHelpText("Press Insert Key to Help")
    End Sub

    Private Sub txtStItem_MAN_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStItem.LostFocus
        Main.HideHelpText()
    End Sub

    Private Sub txtStItem_MAN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtStItem.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadStoneItemName()
        End If
    End Sub

    Private Sub txtStItem_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStItem.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtStItem.Text = "" Then
                LoadStoneItemName()
            ElseIf txtStItem.Text <> "" And objGPack.DupCheck("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "'") = False Then
                LoadStoneItemName()
            Else
                LoadStoneitemDetails()
            End If
        End If
    End Sub

    Private Sub txtStSubItem_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStSubItem.GotFocus
        'If objGPack.DupCheck("SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem_MAN.Text & "')") = False Then
        SendKeys.Send("{TAB}")
        'End If
    End Sub

    Private Sub txtStSubItem_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtStSubItem.KeyDown
        'If e.KeyCode = Keys.Insert Then
        '    strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST"
        '    strSql += " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem_MAN.Text & "')"
        '    strSql += " AND ACTIVE = 'Y' ORDER BY SUBITEMNAME "
        '    showSearch(txtStSubItem, strSql)
        'ElseIf e.KeyCode = Keys.Down Then
        '    If lstSearch.Visible Then
        '        lstSearch.Select()
        '    Else
        '        gridStone.Select()
        '    End If
        'ElseIf e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Tab Then
        '    lstSearch.Visible = False
        'End If
    End Sub

    Private Sub txtStSubItem_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStSubItem.KeyPress
        'If e.KeyChar = Chr(Keys.Enter) Then
        '    If txtStSubItem.Text = "" Then
        '        MsgBox("SubItem Should Not Empty", MsgBoxStyle.Information)
        '        txtStSubItem.Focus()
        '        Exit Sub
        '    End If
        '    If objGPack.DupCheck("SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem_MAN.Text & "') AND SUBITEMNAME = '" & txtStSubItem.Text & "'") = False Then
        '        MsgBox("Invalid SubItem", MsgBoxStyle.Information)
        '        txtStSubItem.Focus()
        '    End If
        'End If
    End Sub


    Private Sub txtStPcs_NUM_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStPcs_NUM.TextChanged
        CalcStoneAmount()
    End Sub

    Private Sub txtStWeight_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStWeight_WET.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim stWeight As Double = Val(txtStWeight_WET.Text)
            For cnt As Integer = 0 To gridStone.RowCount - 1
                With gridStone.Rows(cnt)
                    If .Cells("UNIT").Value.ToString = "C" Then
                        stWeight += Val(.Cells("WEIGHT").Value.ToString) / 5
                    ElseIf .Cells("UNIT").Value.ToString = "G" Then
                        stWeight += Val(.Cells("WEIGHT").Value.ToString)
                    End If
                End With
            Next
            If stWeight > Val(txtOGrsWt_WET.Text) Then
                MsgBox(Me.GetNextControl(txtStWeight_WET, False).Text + E0015 + Me.GetNextControl(txtOGrsWt_WET, False).Text, MsgBoxStyle.Exclamation)
                txtStWeight_WET.Focus()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub txtStWeight_WET_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStWeight_WET.LostFocus
        cmbStCalc.Text = IIf(Val(txtStWeight_WET.Text) <> 0, "W", "P")
    End Sub

    Private Sub txtStRate_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStRate_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim cent As Double = 0
            If cmbStUnit.Text = "C" Then cent = (Val(txtStWeight_WET.Text) / IIf(Val(txtStPcs_NUM.Text) = 0, 1, Val(txtStPcs_NUM.Text))) Else cent = Val(txtStWeight_WET.Text) / IIf(Val(txtStPcs_NUM.Text) = 0, 1, Val(txtStPcs_NUM.Text))
            cent *= 100
            strSql = " DECLARE @CENT FLOAT"
            strSql += " SET @CENT = " & cent & ""
            strSql += " SELECT MINRATE FROM " & cnAdminDb & "..CENTRATE WHERE"
            strSql += " @CENT BETWEEN FROMCENT AND TOCENT "
            strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')"
            strSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')),0)"
            Dim rate As Double = Val(objGPack.GetSqlValue(strSql, , , tran))
            If Val(txtStRate_AMT.Text) < rate Then
                MsgBox(Me.GetNextControl(txtStRate_AMT, False).Text + E0020 + rate.ToString)
                txtStRate_AMT.Focus()
            End If
        End If
    End Sub

    Private Sub cmbStCalc_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStCalc.SelectedIndexChanged
        CalcStoneAmount()
    End Sub


    Private Sub pnlOrderExtraDet_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles pnlOrderExtraDet.VisibleChanged
        If pnlOrderExtraDet.Visible = False Then
            searchSender = Nothing
        End If
    End Sub

    Private Sub txtOMcPerGrm_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOMcPerGrm_AMT.TextChanged
        CalcMc(False)
    End Sub

    Private Sub txtOWastagePer_Per_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOWastagePer_Per.TextChanged
        CalcWastage(False)
    End Sub

    Private Sub txtOCommGrm_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtOCommGrm_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txtOWastage_WET.Focus()
            pnlOrderExtraDet.Visible = False
        End If
    End Sub

    Private Sub txtOCommGrm_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOCommGrm_AMT.TextChanged
        CalcCommision()
    End Sub

    Private Sub ShowOrderAdvance()
        If objOrderAdvance.Visible Then Exit Sub
        objOrderAdvance.BackColor = pnlContainer_OWN.BackColor
        objOrderAdvance.StartPosition = FormStartPosition.CenterScreen
        objOrderAdvance.grpPur.BackgroundColor = grpHeader.BackgroundColor
        objOrderAdvance.pMetalId = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & dtGridOrder.Rows(0).Item("ITEMNAME").ToString & "'")
        objOrderAdvance.ShowDialog()
        Dim totWt As Double = Val(objOrderAdvance.txtPurTotalWeight.Text)
        Dim totAmt As Double = Nothing
        totAmt = Val(objOrderAdvance.txtPurTotalAmount.Text) + Val(objAdvance.gridAdvanceTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        totAmt = CalcRoundoffAmt(totAmt, RndFinalAmt)
        txtAdjAdvance_AMT.Text = IIf(totAmt <> 0, Format(totAmt, "0.00"), Nothing)
        txtAdjAdvanceWt.Text = IIf(totWt <> 0, Format(totWt, "0.000"), Nothing)
        If GSTADVCALC_INCL = "E" Then
            CalcBalAmt()
        End If
        CalcBalanceAmount()
    End Sub

    Private Sub txtAdjAdvanceWt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdjAdvanceWt.GotFocus
        ShowOrderAdvance()
    End Sub

    Private Sub txtAdjAdvance_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdjAdvance_AMT.GotFocus
        If objAdvance.Visible Then Exit Sub
        objAdvance.BackColor = pnlContainer_OWN.BackColor
        objAdvance.StartPosition = FormStartPosition.CenterScreen
        objAdvance.grpAdvance.BackgroundColor = grpHeader.BackgroundColor
        If objAdvance.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If objAddressDia.txtAddressRegularSno.Text <> "" Then '  objAdvance.dtGridAdvance.Rows.Count > 0 Then
                GetAdvanceAddress()
            End If
            Dim advAmt As Double = Val(objAdvance.gridAdvanceTotal.Rows(0).Cells("AMOUNT").Value.ToString) + Val(objOrderAdvance.txtPurTotalAmount.Text)
            advAmt = CalcRoundoffAmt(advAmt, RndFinalAmt)
            txtAdjAdvance_AMT.Text = IIf(advAmt <> 0, Format(advAmt, "0.00"), Nothing)
            If GSTADVCALC_INCL = "I" Then
                txtAdjCash_AMT.Focus()
            Else
                CalcBalAmt()
            End If
        End If
    End Sub

    Private Sub txtAdjCheque_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdjCheque_AMT.GotFocus
        If objCheaque.Visible Then Exit Sub
        objCheaque.BackColor = pnlContainer_OWN.BackColor
        objCheaque.StartPosition = FormStartPosition.CenterScreen
        objCheaque.grpCheque.BackgroundColor = grpHeader.BackgroundColor
        objCheaque.ShowDialog()
        Dim chqAmt As Double = Val(objCheaque.gridChequeTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        txtAdjCheque_AMT.Text = IIf(chqAmt <> 0, Format(chqAmt, "0.00"), Nothing)
        If GSTADVCALC_INCL = "I" Then
            txtAdjCash_AMT.Focus()
        Else
            CalcBalAmt()
        End If
    End Sub

    Private Sub txtAdjCreditCard_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdjCreditCard_AMT.GotFocus
        If objCreditCard.Visible Then Exit Sub
        objCreditCard.BackColor = pnlContainer_OWN.BackColor
        objCreditCard.StartPosition = FormStartPosition.CenterScreen
        objCreditCard.grpCreditCard.BackgroundColor = grpHeader.BackgroundColor
        objCreditCard.ShowDialog()
        Dim cardAmt As Double = Val(objCreditCard.gridCreditCardTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        txtAdjCreditCard_AMT.Text = IIf(cardAmt <> 0, Format(cardAmt, "0.00"), Nothing)
        If GSTADVCALC_INCL = "I" Then
            txtAdjCash_AMT.Focus()
        Else
            CalcBalAmt()
        End If
    End Sub
    Private Sub CalcGst()
        Dim amt As Double = Nothing
        Dim Totamt As Double = Nothing
        Dim SGstAmt As Double = Nothing
        Dim CGstAmt As Double = Nothing
        Dim GstAmt As Double = Nothing
        If _Vbc Then
            Exit Sub
        End If
        If GSTADVCALC_INCL = "N" Then Exit Sub
        amt = (Val(txtAdjCreditCard_AMT.Text) + Val(txtAdjChitCard_AMT.Text) + Val(txtAdjCheque_AMT.Text) + Val(txtAdjCash_AMT.Text) + Val(txtAdjAdvance_AMT.Text))
        Totamt = (Val(txtAdjCreditCard_AMT.Text) + Val(txtAdjChitCard_AMT.Text) + Val(txtAdjCheque_AMT.Text) + Val(txtAdjCash_AMT.Text) + Val(txtAdjAdvance_AMT.Text))
        amt -= Val(objOrderAdvance.dtGridPur.Compute("SUM(GROSSAMT)", Nothing).ToString)
        If Val(objOrderAdvance.dtGridPur.Compute("SUM(VAT)", Nothing).ToString) > 0 Then
            amt -= Val(objOrderAdvance.dtGridPur.Compute("SUM(GST)", Nothing).ToString)
        End If
        If amt < 0 Then Exit Sub
        If GST Then
            If GSTADVCALC_INCL = "I" Then
                SGstAmt = (amt * (GstPer / 2)) / (100 + GstPer)
                SGstAmt = Math.Round(SGstAmt, 2)
                SGstAmt = CalcRoundoffAmt(SGstAmt, RndGst)
                CGstAmt = (amt * (GstPer / 2)) / (100 + GstPer)
                CGstAmt = Math.Round(CGstAmt, 2)
                CGstAmt = CalcRoundoffAmt(CGstAmt, RndGst)

                GstAmt = CGstAmt + SGstAmt
                GstAmt += Val(objOrderAdvance.dtGridPur.Compute("SUM(GST)", Nothing).ToString)
                txtAdjGst_AMT.Text = IIf(GstAmt <> 0, Format(GstAmt, "0.00"), Nothing)
            Else
                SGstAmt = (amt * (GstPer / 2)) / 100
                SGstAmt = Math.Round(SGstAmt, 2)
                SGstAmt = CalcRoundoffAmt(SGstAmt, RndGst)
                CGstAmt = (amt * (GstPer / 2)) / 100
                CGstAmt = Math.Round(CGstAmt, 2)
                CGstAmt = CalcRoundoffAmt(CGstAmt, RndGst)

                GstAmt = CGstAmt + SGstAmt
                GstAmt += Val(objOrderAdvance.dtGridPur.Compute("SUM(GST)", Nothing).ToString)
                txtAdjGst_AMT.Text = IIf(GstAmt <> 0, Format(GstAmt, "0.00"), Nothing)
                amt += GstAmt
            End If
        End If
        txtAdjRec_AMT.Text = IIf(Totamt <> 0, Format(Totamt, "0.00"), Nothing)
    End Sub
    Private Sub CalcBalAmt()
        Dim TotAmt As Decimal = Val(txtAdjRec_AMT.Text)
        'TotAmt += Val(txtAdjGst_AMT.Text)
        Dim Cash As Decimal = TotAmt - (Val(txtAdjCreditCard_AMT.Text) + Val(txtAdjChitCard_AMT.Text) + Val(txtAdjCheque_AMT.Text) + Val(txtAdjAdvance_AMT.Text))
        'txtAdjCash_AMT.Text = IIf(Cash <> 0, Format(Cash, "0.00"), Nothing)
        'txtAdjCash_AMT.Focus()
    End Sub
    Private Sub CalcBalanceAmount()
        lblBalance.Text = ""
        lblnetamount.Text = ""
        If OrdType <> OrderType.OrderEntry Then Exit Sub
        If QuickOrder Then Exit Sub
        If Not gridOrderTotal.RowCount > 0 Then Exit Sub
        If objOrderAdvance Is Nothing Then Exit Sub
        Dim TotAmt As Decimal = Val(gridOrderTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        lblnetamount.Text = "Total  : " & Format(TotAmt, "0.00")
        For Each ro As DataRow In objOrderAdvance.dtGridPur.Rows
            If ro.Item("MODE").ToString = "WEIGHT" Then
                TotAmt -= Val(ro.Item("AMOUNT").ToString)
            End If
        Next
        'TotAmt -= (Val(txtAdjAdvanceWt.Text) * Val(Main.tStripGoldRate.Text))
        TotAmt -= Val(txtAdjAdvance_AMT.Text)
        TotAmt -= Val(txtAdjCreditCard_AMT.Text)
        TotAmt -= Val(txtAdjCheque_AMT.Text)
        TotAmt -= Val(txtAdjChitCard_AMT.Text)
        TotAmt -= Val(txtAdjDiscount_AMT.Text)
        TotAmt -= Val(txtAdjCash_AMT.Text)
        If TotAmt <> 0 Then
            lblBalance.Text = "Balance : " & Format(TotAmt, "0.00")
        End If
        CalcGst()
    End Sub

    Private Sub txtAddressFax_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = Chr(Keys.Enter) Then
            txtAdjCash_AMT.Focus()
        End If
    End Sub

    Private Sub InsertOrderAdditionalDetails(ByVal tNo As Integer,
          ByVal Orsno As String,
          ByVal Trantype As String,
          ByVal Costid As String,
          ByVal Companyid As String,
          ByVal ro As DataRow,
          ByVal Batchno As String _
          , ByVal tran As OleDbTransaction)
        Dim Typeid As Integer = 0
        Dim Valueid As Integer = 0
        With ro
            Typeid = GetSqlValue("SELECT TYPEID FROM " & cnAdminDb & "..ORADMAST WHERE TYPENAME = '" & .Item("TYPENAME").ToString & "'", cn, tran)
            Valueid = GetSqlValue("SELECT VALUEID FROM " & cnAdminDb & "..ORADVALUEMAST WHERE TYPEID = '" & Typeid & "' AND VALUENAME = '" & .Item("VALUENAME").ToString & "'", cn, tran)
            strSql = " INSERT INTO " & cnAdminDb & "..ORADTRAN"
            strSql += " ("
            strSql += " SNO,TYPEID,VALUENAME,ORSNO,TRANNO,ORNO,TRANDATE"
            strSql += " ,TRANTYPE,COSTID,COMPANYID,BATCHNO,USERID,UPDATED,UPTIME,VALUEID"
            strSql += " )"
            strSql += " VALUES"
            strSql += " ("
            strSql += " '" & GetNewSno(TranSnoType.ORADTRAN, tran, "GET_ADMINSNO_TRAN") & "'" ''SNO
            strSql += " ,'" & Typeid & "'" 'typeid
            strSql += " ,'" & .Item("VALUENAME").ToString & "'" 'VALUENAME
            strSql += " ,'" & Orsno & "'" 'TRANNO 
            strSql += " ," & tNo & "" 'TRANNO 
            strSql += " ,'" & OrderNo & "'" 'TRANNO 
            strSql += " ,'" & GetEntryDate(BillDate, tran).ToString("yyyy-MM-dd") & "'" 'TRANDATE
            strSql += " ,'" & Trantype & "'" 'Trantype
            strSql += " ,'" & BillCostId & "'" 'COSTID
            strSql += " ,'" & strCompanyId & "'" 'COMPANYI
            strSql += " ,'" & Batchno & "'" 'BATCHNO
            strSql += " ,'" & userId & "'" 'USERID
            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
            strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIMe                    
            strSql += " ,'" & Valueid & "'"
            strSql += " )"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
        End With
        strSql = ""
    End Sub

    Private Function InsertAccountsDetail(Optional ByVal ptranno As Long = 0) As Boolean
        Dim ContraCode As String = Nothing 'objGPack.GetSqlValue("SELECT TOP 1 ACCODE FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = '" & Batchno & "' AND TRANMODE = '" & IIf(Val(txtAdjReceive_AMT.Text) > 0, "C", "D") & "' ORDER BY substring(SNO,6,10)", , , tran)
        Dim amount As Double
        Dim mrem1 As String = "Against Order No:" & TranNo.ToString
        Dim mrem2 As String = "Customer : " & objAddressDia.txtAddressName.Text
        Dim xaccode As String = IIf(objAddressDia.txtAddressPartyCode.Text <> "", objAddressDia.txtAddressPartyCode.Text, "ADVORD")
        Dim mslcode As String = ""
        If GLEDGERCODE <> "" Then mslcode = xaccode : xaccode = GLEDGERCODE
        amount = Val(txtAdjAdvance_AMT.Text) - Val(objOrderAdvance.txtPurTotalAmount.Text)
        amount += Val(txtAdjChitCard_AMT.Text)
        amount += Val(txtAdjCreditCard_AMT.Text)
        amount += Val(txtAdjCheque_AMT.Text)
        amount += Val(txtAdjCash_AMT.Text)
        Dim TotRecAmt As Double = amount 'Val(txtAdjRec_AMT.Text)
        Dim TotGstAmt As Double = Val(txtAdjGst_AMT.Text)
        Dim OrRate As Double = Val(lblGoldRate.Text)
        If rbtCurrentRate.Checked Then
            For Each _dr As DataRow In dtGridOrder.Select("ISNULL(RATE,0)<>0")
                OrRate = _dr!RATE.ToString
            Next
        End If
        Dim AdvGstAmt As Double = 0
        If GST Then
            For Each ro As DataRow In objOrderAdvance.dtGridPur.Rows
                If ro!ENTFLAG.ToString = "" Then Exit For
                If ro.Item("MODE").ToString = "PURCHASE" Or ro.Item("MODE").ToString = "EXCHANGE" Then
                    AdvGstAmt += Val(ro!GST.ToString)
                End If
            Next
        End If
        If GST And TotGstAmt > 0 Then
            If InterStateBill = False Then
                InsertIntoAccTran(IIf(ptranno <> 0, ptranno, TranNo), IIf(TotGstAmt > 0, "C", "D"),
                            SCode, Math.Round(TotGstAmt / 2, 2), 0, 0, 0, "SV", ContraCode, , , , , , , mrem1, mrem2, mslcode)
                InsertIntoAccTran(IIf(ptranno <> 0, ptranno, TranNo), IIf(TotGstAmt > 0, "C", "D"),
                            CCode, Math.Round(TotGstAmt / 2, 2), 0, 0, 0, "SV", ContraCode, , , , , , , mrem1, mrem2, mslcode)
            Else
                InsertIntoAccTran(IIf(ptranno <> 0, ptranno, TranNo), IIf(TotGstAmt > 0, "C", "D"),
                            ICode, TotGstAmt, 0, 0, 0, "SV", ContraCode, , , , , , , mrem1, mrem2, mslcode)
            End If
        End If
        If TotRecAmt <> 0 Then
            InsertIntoAccTran(TranNo, IIf(TotRecAmt > 0, "C", "D"),
            xaccode, (TotRecAmt - (TotGstAmt - AdvGstAmt)), 0, 0, 0, "OR", ContraCode, , , , , , , mrem1, mrem2, mslcode)
            InsertIntoOustanding("A", OrderNo, (TotRecAmt - (TotGstAmt - AdvGstAmt)), "R", "OR", , , , OrRate, , , , , , , xaccode, mslcode, (TotGstAmt - AdvGstAmt))
        End If

        ''Cash Transaction
        InsertIntoAccTran(TranNo, IIf(Val(txtAdjCash_AMT.Text) > 0, "D", "C"),
        CASHID, Val(txtAdjCash_AMT.Text), 0, 0, 0, "CA", ContraCode, , , , , , , mrem1, mrem2)
        ''SCHEME Trans
        If Val(txtAdjChitCard_AMT.Text) <> 0 Then
            If objChitCard.InsertChitCardDetail(Batchno, TranNo, BillDate, BillCashCounterId, BillCostId, VATEXM, tran, "D", False) Then Return True
        End If

        objOrderAdvance.InsertOrderAdvanceDetail(Batchno, IIf(ptranno <> 0, ptranno, TranNo), BillDate,
        BillCashCounterId, BillCostId, VATEXM, tran, OrderNo,
        xaccode, "D", , , , , , OrRate, , AdvGstAmt)

        ''Advance Trans
        For Each ro As DataRow In objAdvance.dtGridAdvance.Rows
            xaccode = IIf(ro!ACCODE.ToString <> "", ro!ACCODE.ToString, "ADVANCE")
            If GLEDGERCODE <> "" Then mslcode = xaccode : xaccode = GLEDGERCODE

            InsertIntoAccTran(TranNo, IIf(Val(ro!AMOUNT.ToString) > 0, "D", "C"),
            xaccode, Val(ro!AMOUNT.ToString), 0, 0, 0, "AA", ContraCode, , , , ro!DATE.ToString, , , mrem1, mrem2, mslcode)
            InsertIntoOustanding("A", GetCostId(BillCostId) & GetCompanyId(strCompanyId) & ro!RUNNO.ToString, Val(ro!AMOUNT.ToString), "P", "AA", , , , , , ro!REFNO.ToString, ro!DATE.ToString, , , , xaccode, mslcode)
        Next

        ''CreditCard Trans
        For Each ro As DataRow In objCreditCard.dtGridCreditCard.Rows
            InsertIntoAccTran(TranNo, IIf(Val(ro!AMOUNT.ToString) > 0, "D", "C"),
            objGPack.GetSqlValue(" SELECT ACCODE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran) _
            , Val(ro!AMOUNT.ToString), 0, 0, 0, "CC", ContraCode,
            , , ro!CARDNO.ToString, ro!DATE.ToString,
             objGPack.GetSqlValue(" SELECT CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran),
            ro!APPNO.ToString, mrem1, mrem2)

            Dim commision As Double = Format(Val(ro!AMOUNT.ToString) * (Val(objGPack.GetSqlValue(" SELECT COMMISSION FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran)) / 100), "0.00")
            If commision <> 0 Then
                InsertIntoAccTran(TranNo, IIf(Val(ro!AMOUNT.ToString) > 0, "C", "D"),
                objGPack.GetSqlValue(" SELECT ACCODE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran) _
                , commision, 0, 0, 0, "BC", "BANKC",
                , , , , , , mrem1, mrem2)

                InsertIntoAccTran(TranNo, IIf(Val(ro!AMOUNT.ToString) > 0, "D", "C"),
                "BANKC" _
                , commision, 0, 0, 0, "BC", objGPack.GetSqlValue(" SELECT ACCODE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran),
                , , , , , , mrem1, mrem2)

                Dim sCharge As Double = Format(commision * (Val(objGPack.GetSqlValue(" SELECT SURCHARGE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran)) / 100), "0.00")
                If sCharge <> 0 Then
                    InsertIntoAccTran(TranNo, IIf(Val(ro!AMOUNT.ToString) > 0, "C", "D"),
                    objGPack.GetSqlValue(" SELECT ACCODE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran) _
                    , sCharge, 0, 0, 0, "BS", "BANKS",
                    , , , , , , mrem1, mrem2)

                    InsertIntoAccTran(TranNo, IIf(Val(ro!AMOUNT.ToString) > 0, "D", "C"),
                    "BANKS" _
                    , sCharge, 0, 0, 0, "BS", objGPack.GetSqlValue(" SELECT ACCODE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & ro!CARDTYPE.ToString & "'", , , tran),
                    , , , , , , mrem1, mrem2)
                End If
            End If
        Next
        ''Cheque Trans
        For Each ro As DataRow In objCheaque.dtGridCheque.Rows
            InsertIntoAccTran(TranNo, IIf(Val(ro!AMOUNT.ToString) > 0, "D", "C"),
            objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & ro!DEFAULTBANK.ToString & "'", , , tran),
            Val(ro!AMOUNT.ToString), 0, 0, 0, "CH", ContraCode,
            , , ro!CHEQUENO.ToString, ro!DATE.ToString, , ro!BANKNAME.ToString, mrem1, mrem2)
        Next

        ''UPDATE CONTRA
        strSql = " UPDATE " & cnStockDb & "..ACCTRAN SET CONTRA = "
        strSql += " (SELECT TOP 1 ACCODE FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = T.BATCHNO AND TRANMODE = 'C' AND PAYMODE<>'SV' AND ACCODE <> T.ACCODE ORDER BY SNO)"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T"
        strSql += " WHERE TRANDATE = '" & GetEntryDate(BillDate, tran).ToString("yyyy-MM-dd") & "' AND BATCHNO = '" & Batchno & "' AND TRANMODE = 'D'"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
        strSql = " UPDATE " & cnStockDb & "..ACCTRAN SET CONTRA = "
        strSql += " (SELECT TOP 1 ACCODE FROM " & cnStockDb & "..ACCTRAN WHERE BATCHNO = T.BATCHNO AND TRANMODE = 'D' AND PAYMODE<>'SV' AND ACCODE <> T.ACCODE ORDER BY SNO)"
        strSql += " FROM " & cnStockDb & "..ACCTRAN AS T"
        strSql += " WHERE TRANDATE = '" & GetEntryDate(BillDate, tran).ToString("yyyy-MM-dd") & "' AND BATCHNO = '" & Batchno & "' AND TRANMODE = 'C'"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
    End Function
    Private Sub InsertIntoAccTran _
  (ByVal tNo As Integer,
  ByVal tranMode As String,
  ByVal accode As String,
  ByVal amount As Double,
  ByVal pcs As Integer,
  ByVal grsWT As Double,
  ByVal netWT As Double,
  ByVal payMode As String,
  ByVal contra As String,
  Optional ByVal refNo As String = Nothing, Optional ByVal refDate As String = Nothing,
  Optional ByVal chqCardNo As String = Nothing,
  Optional ByVal chqDate As String = Nothing,
  Optional ByVal chqCardId As Integer = Nothing,
  Optional ByVal chqCardRef As String = Nothing,
  Optional ByVal Remark1 As String = Nothing,
  Optional ByVal Remark2 As String = Nothing,
Optional ByVal Slcode As String = Nothing)
        If Slcode Is Nothing Then Slcode = ""
        If amount = 0 Then Exit Sub
        strSql = " INSERT INTO " & cnStockDb & "..ACCTRAN"
        strSql += " ("
        strSql += " SNO,TRANNO,TRANDATE,TRANMODE,ACCODE,SACCODE"
        strSql += " ,AMOUNT,PCS,GRSWT,NETWT"
        strSql += " ,REFNO,REFDATE,PAYMODE,CHQCARDNO"
        strSql += " ,CARDID,CHQCARDREF,CHQDATE,BRSFLAG"
        strSql += " ,RELIASEDATE,FROMFLAG,REMARK1,REMARK2"
        strSql += " ,CONTRA,BATCHNO,USERID,UPDATED"
        strSql += " ,UPTIME,SYSTEMID,CASHID,COSTID,APPVER,COMPANYID"
        strSql += " )"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & GetNewSno(TranSnoType.ACCTRANCODE, tran) & "'" ''SNO
        strSql += " ," & tNo & "" 'TRANNO 
        strSql += " ,'" & GetEntryDate(BillDate, tran).ToString("yyyy-MM-dd") & "'" 'TRANDATE
        strSql += " ,'" & tranMode & "'" 'TRANMODE
        strSql += " ,'" & accode & "'" 'ACCODE
        strSql += " ,'" & Slcode & "'" 'ACCODE
        strSql += " ," & Math.Abs(amount) & "" 'AMOUNT
        strSql += " ," & Math.Abs(pcs) & "" 'PCS
        strSql += " ," & Math.Abs(grsWT) & "" 'GRSWT
        strSql += " ," & Math.Abs(netWT) & "" 'NETWT
        strSql += " ,'" & refNo & "'" 'REFNO
        If refDate = Nothing Then
            strSql += " ,NULL" 'REFDATE
        Else
            strSql += " ,'" & refDate & "'" 'REFDATE
        End If
        strSql += " ,'" & payMode & "'" 'PAYMODE
        strSql += " ,'" & chqCardNo & "'" 'CHQCARDNO
        strSql += " ," & chqCardId & "" 'CARDID
        strSql += " ,'" & chqCardRef & "'" 'CHQCARDREF
        If chqDate = Nothing Then
            strSql += " ,NULL" 'CHQDATE
        Else
            strSql += " ,'" & chqDate & "'" 'CHQDATE
        End If
        strSql += " ,''" 'BRSFLAG
        strSql += " ,NULL" 'RELIASEDATE
        strSql += " ,'D'" 'FROMFLAG
        strSql += " ,'" & Remark1 & "'" 'REMARK1
        strSql += " ,'" & Remark2 & "'" 'REMARK2
        strSql += " ,'" & contra & "'" 'CONTRA
        strSql += " ,'" & Batchno & "'" 'BATCHNO
        strSql += " ,'" & userId & "'" 'USERID
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & systemId & "'" 'SYSTEMID
        strSql += " ,'" & BillCashCounterId & "'" 'CASHID
        strSql += " ,'" & BillCostId & "'" 'COSTID
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,'" & strCompanyId & "'" 'COMPANYID
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
        strSql = ""
        cmd = Nothing
    End Sub
    Private Sub InsertIntoOustanding _
   (
   ByVal tType As String, ByVal RunNo As String, ByVal Amount As Double,
   ByVal RecPay As String,
   ByVal Paymode As String,
   Optional ByVal GrsWt As Double = 0,
   Optional ByVal NetWt As Double = 0,
   Optional ByVal CatCode As String = Nothing,
   Optional ByVal Rate As Double = Nothing,
   Optional ByVal Value As Double = Nothing,
   Optional ByVal refNo As String = Nothing,
   Optional ByVal refDate As String = Nothing,
   Optional ByVal purity As Double = Nothing,
   Optional ByVal proId As Integer = Nothing,
   Optional ByVal dueDate As String = Nothing,
   Optional ByVal aCCode As String = Nothing,
   Optional ByVal SlCode As String = Nothing,
    Optional ByVal GSTVAL As Double = Nothing)
        If Amount = 0 And GrsWt = 0 Then Exit Sub
        If SlCode Is Nothing Then SlCode = ""
        Dim Sno As String = GetNewSno(TranSnoType.OUTSTANDINGCODE, tran, "GET_ADMINSNO_TRAN")
        strSql = " INSERT INTO " & cnAdminDb & "..OUTSTANDING"
        strSql += " ("
        strSql += " SNO,TRANNO,TRANDATE,TRANTYPE,RUNNO"
        strSql += " ,AMOUNT,GRSWT,NETWT,RECPAY"
        strSql += " ,REFNO,REFDATE,EMPID"
        strSql += " ,TRANSTATUS,PURITY,CATCODE,BATCHNO"
        strSql += " ,USERID,UPDATED,UPTIME,SYSTEMID"
        strSql += " ,RATE,VALUE,CASHID,REMARK1,REMARK2,CTRANCODE,DUEDATE,APPVER"
        strSql += " ,COMPANYID,ACCODE,COSTID,PAYMODE,FROMFLAG,SACCODE,GSTVAL)"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & Sno & "'" ''SNO
        strSql += " ," & TranNo & "" 'TRANNO
        strSql += " ,'" & GetEntryDate(BillDate, tran).ToString("yyyy-MM-dd") & "'" 'TRANDATE
        strSql += " ,'" & tType & "'" 'TRANTYPE
        strSql += " ,'" & RunNo & "'" 'RUNNO
        strSql += " ," & Math.Abs(Amount) & "" 'AMOUNT
        strSql += " ," & Math.Abs(GrsWt) & "" 'GRSWT
        strSql += " ," & Math.Abs(NetWt) & "" 'NETWT
        strSql += " ,'" & RecPay & "'" 'RECPAY
        strSql += " ,'" & refNo & "'" 'REFNO
        If refDate <> Nothing Then
            strSql += " ,'" & refDate & "'" 'REFDATE
        Else
            strSql += " ,NULL" 'REFDATE
        End If

        strSql += " ,0" 'EMPID
        strSql += " ,''" 'TRANSTATUS
        strSql += " ," & purity & "" 'PURITY
        strSql += " ,'" & CatCode & "'" 'CATCODE
        strSql += " ,'" & Batchno & "'" 'BATCHNO
        strSql += " ," & userId & "" 'USERID

        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & systemId & "'" 'SYSTEMID
        strSql += " ," & Rate & "" 'RATE
        strSql += " ," & Value & "" 'VALUE
        strSql += " ,'" & BillCashCounterId & "'" 'CASHID
        strSql += " ,''" 'REMARK1
        strSql += " ,''" 'REMARK1
        strSql += " ," & proId & "" 'CTRANCODE
        If dueDate <> Nothing Then
            strSql += " ,'" & dueDate & "'" 'DUEDATE
        Else
            strSql += " ,NULL" 'DUEDATE
        End If
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,'" & strCompanyId & "'" 'COMPANYID
        strSql += " ,'" & aCCode & "'" 'Accode
        strSql += " ,'" & BillCostId & "'" 'COSTID
        strSql += " ,'" & Paymode & "'" 'PAYMODE
        strSql += " ,'D'" 'FROMFLAG
        strSql += " ,'" & SlCode & "'" 'Accode
        strSql += " ," & GSTVAL & "" 'GSTVAL
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
        If GSTVAL > 0 Then
            strSql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
            strSql += " ("
            strSql += " SNO,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
            strSql += " ,AMOUNT,TAXPER,TAXAMOUNT,TSNO,COSTID,COMPANYID,ISSSNO"
            strSql += " )"
            strSql += " VALUES("
            strSql += " '" & GetNewSno(TranSnoType.TAXTRANCODE, tran) & "'" ''SNO
            strSql += " ," & TranNo & "" 'TRANNO
            strSql += " ,'" & GetEntryDate(BillDate, tran).ToString("yyyy-MM-dd") & "'" 'TRANDATE 'BillDate
            strSql += " ,'" & Paymode & "'" 'TRANTYPE
            strSql += " ,'" & Batchno & "'" 'BATCHNO
            strSql += " ,'SG'" 'TAXID
            strSql += " ," & Amount & "" 'AMOUNT
            strSql += " ," & GstPer / 2 & "" 'TAXPER
            strSql += " ," & Math.Abs(GSTVAL / 2) & "" 'TAXAMOUNT
            strSql += " ,1" 'TSNO
            strSql += " ,'" & BillCostId & "'" 'COSTID 
            strSql += " ,'" & strCompanyId & "'" 'COMPANYID
            strSql += " ,'" & Sno & "'" 'COMPANYID
            strSql += " )"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
            strSql = " INSERT INTO " & cnStockDb & "..TAXTRAN"
            strSql += " ("
            strSql += " SNO,TRANNO,TRANDATE,TRANTYPE,BATCHNO,TAXID"
            strSql += " ,AMOUNT,TAXPER,TAXAMOUNT,TSNO,COSTID,COMPANYID,ISSSNO"
            strSql += " )"
            strSql += " VALUES("
            strSql += " '" & GetNewSno(TranSnoType.TAXTRANCODE, tran) & "'" ''SNO
            strSql += " ," & TranNo & "" 'TRANNO
            strSql += " ,'" & GetEntryDate(BillDate, tran).ToString("yyyy-MM-dd") & "'" 'TRANDATE 'BillDate
            strSql += " ,'" & Paymode & "'" 'TRANTYPE
            strSql += " ,'" & Batchno & "'" 'BATCHNO
            strSql += " ,'CG'" 'TAXID
            strSql += " ," & Amount & "" 'AMOUNT
            strSql += " ," & GstPer / 2 & "" 'TAXPER
            strSql += " ," & Math.Abs(GSTVAL / 2) & "" 'TAXAMOUNT
            strSql += " ,2" 'TSNO
            strSql += " ,'" & BillCostId & "'" 'COSTID 
            strSql += " ,'" & strCompanyId & "'" 'COMPANYID
            strSql += " ,'" & Sno & "'" 'COMPANYID
            strSql += " )"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, BillCostId)
        End If
    End Sub

    Private Sub txtDueDays_NUM_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDueDays_NUM.Leave
        If Val(txtDueDays_NUM.Text) > 0 Then
            dtpDueDate.Value = BillDate.AddDays(Val(txtDueDays_NUM.Text)) '(BillDate, tran).AddDays(Val(txtDueDays_NUM.Text))
            txtRemDays_NUM.Text = 1
            'txtRemDays_TextChanged(Me, New EventArgs)
        Else
            MsgBox("Due days can't be zero")
            txtDueDays_NUM.Focus()
            Exit Sub
        End If
    End Sub

    Private Sub txtDueDays_NUM_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDueDays_NUM.TextChanged
        'If Val(txtDueDays_NUM.Text) > 0 Then
        '    dtpDueDate.Value = GetEntryDate(BillDate, tran).AddDays(Val(txtDueDays_NUM.Text))
        '    txtRemDays_NUM.Text = 1 'Val(txtDueDays_NUM.Text) - 1
        '    txtRemDays_TextChanged(Me, New EventArgs)
        '    If txtRemDays_NUM.Text = 0 Then txtRemDays_NUM.Clear()
        'Else
        '    MsgBox("Due days can't be zero")
        '    txtDueDays_NUM.Focus()
        '    Exit Sub
        '    'dtpDueDate.Value = GetEntryDate(BillDate, tran).ToString("yyyy-MM-dd")
        '    'txtRemDays_NUM.Text = Val(txtDueDays_NUM.Text)
        'End If
    End Sub

    Private Sub txtRemDays_NUM_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRemDays_NUM.Leave
        If Val(txtRemDays_NUM.Text) > 0 Then
            dtpRemDate.Value = dtpDueDate.Value.AddDays(-1 * Val(txtRemDays_NUM.Text))
        Else
            MsgBox("Reminder days can't be zero")
            txtRemDays_NUM.Focus()
            Exit Sub
        End If
    End Sub

    Private Sub dtpRemDate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        If dtpRemDate.Value > dtpDueDate.Value Then
            MsgBox("Reminder Date does not exceed DueDate", MsgBoxStyle.Information)
            dtpRemDate.Focus()
            Exit Sub
        End If
        'txtRemDays.Text = (dtpDueDate.Value - dtpRemDate.Value).Days
    End Sub

    Private Sub dtpDueDate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        If dtpDueDate.Value < BillDate.Date Then '  GetEntryDate(BillDate, tran).Date Then
            MsgBox("Due Date must exceed OrderDate", MsgBoxStyle.Information)
            dtpDueDate.Focus()
            Exit Sub
        End If
        txtDueDays_NUM.Text = DateDiff(DateInterval.Day, dtpDueDate.Value.Date, BillDate.Date)        'GetEntryDate(BillDate, tran).Date).Days
    End Sub



    Private Sub chkOSample_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles chkOSample.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If chkOSample.Checked Then
                tabOtherOptions.SelectedTab = tabSample
                cmbSamType.Focus()
            Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub


    Private Sub txtSamItem_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSamItem.GotFocus
        Main.ShowHelpText("Press Insert Key to Help")
    End Sub

    Private Sub txtSamItem_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSamItem.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadSamItemName()
        End If
    End Sub

    Private Sub txtSamItem_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSamItem.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtSamItem.Text = "" Then
                LoadSamItemName()
            ElseIf txtSamItem.Text <> "" And objGPack.DupCheck("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtSamItem.Text & "'") = False Then
                LoadSamItemName()
            Else
                LoadSamItemDetails()
            End If
        End If
    End Sub

    Private Sub txtSamItem_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSamItem.LostFocus
        Main.HideHelpText()
    End Sub

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        Try
            Dim openDia As New OpenFileDialog
            Dim str As String
            str = "JPEG(*.jpg)|*.jpg"
            str += "|Bitmap(*.bmp)|*.bmp"
            str += "|GIF(*.gif)|*.gif"
            str += "|All Files(*.*)|*.*"
            openDia.Filter = str
            If openDia.ShowDialog = Windows.Forms.DialogResult.OK Then
                AutoImageSizer(openDia.FileName, picImage, PictureBoxSizeMode.StretchImage)
                picPath = openDia.FileName
            End If
        Catch ex As Exception
            MsgBox(E0016, MsgBoxStyle.Exclamation)
        End Try
    End Sub

    Private Sub LoadSampleDetails()
        If txtSamTagNo.Text <> "" Then
            strSql = " SELECT * FROM " & cnAdminDb & "..ITEMTAG"
            strSql += " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtSamItem.Text & "')"
            strSql += " AND  TAGNO = '" & txtSamTagNo.Text & "'"
            strSql += " AND ISSDATE IS NULL"
            Dim dtTagCheck As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTagCheck)
            If Not dtTagCheck.Rows.Count > 0 Then
                MsgBox("Invalid TagNo", MsgBoxStyle.Information)
                Exit Sub
            End If
        End If
        If Val(txtSamGrsWt_WET.Text) = 0 Then
            MsgBox("GrsWt Should Not Empty", MsgBoxStyle.Information)
            txtSamGrsWt_WET.Focus()
            Exit Sub
        End If
        If Val(txtSamNetWt_WET.Text) = 0 Then
            If Val(txtSamNetWt_WET.Text) = 0 Then
                MsgBox("NetWt Should Not Empty", MsgBoxStyle.Information)
                txtSamNetWt_WET.Focus()
                Exit Sub
            ElseIf Val(txtSamNetWt_WET.Text) > Val(txtSamGrsWt_WET.Text) Then
                MsgBox("Invalid NetWeight", MsgBoxStyle.Information)
                txtSamNetWt_WET.Focus()
                Exit Sub
            End If
        End If
        Dim ro As DataRow = dtGridSample.NewRow
        ro!TYPE = cmbSamType.Text
        ro!FROM = cmbSamFrom.Text
        ro!ITEM = txtSamItem.Text
        ro!DESCRIPTION = txtSamDescription.Text
        ro!TAGNO = txtSamTagNo.Text
        ro!PCS = IIf(Val(txtSamPcs_NUM.Text) <> 0, txtSamPcs_NUM.Text, DBNull.Value)
        ro!GRSWT = IIf(Val(txtSamGrsWt_WET.Text) <> 0, txtSamGrsWt_WET.Text, DBNull.Value)
        ro!NETWT = IIf(Val(txtSamNetWt_WET.Text) <> 0, txtSamNetWt_WET.Text, DBNull.Value)
        dtGridSample.Rows.Add(ro)
        dtGridSample.AcceptChanges()

        objGPack.TextClear(grpSample)
        cmbSamType.Text = "SAMPLE"
        cmbSamFrom.Text = "CUSTOMER"
        cmbSamType.Focus()
    End Sub

    Private Sub txtSamNetWt_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSamNetWt_WET.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            LoadSampleDetails()
        ElseIf Val(txtSamNetWt_WET.Text + e.KeyChar) > Val(txtSamGrsWt_WET.Text) Then
            e.Handled = True
        End If
    End Sub

    Private Sub cmbSamFrom_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSamFrom.SelectedIndexChanged
        If cmbSamFrom.Text = "CUSTOMER" Then
            txtSamTagNo.Clear()
        End If
    End Sub

    Private Sub txtSamTagNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSamTagNo.GotFocus
        If cmbSamFrom.Text = "CUSTOMER" Then
            SendKeys.Send("{TAB}")
        End If
        Main.ShowHelpText("Press Insert Key to Help")
    End Sub


    Private Sub txtOCommision_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOCommision_AMT.GotFocus
        Main.ShowHelpText("Press Insert to Set Commision Per Grm Value")
    End Sub

    Private Sub txtOCommision_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtOCommision_AMT.KeyDown
        If e.KeyCode = Keys.Insert Then
            ShowOrderExtraDetails(CType(sender, Control))
        End If
    End Sub

    Private Sub txtOCommision_AMT_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOCommision_AMT.LostFocus
        Main.HideHelpText()
    End Sub

    Private Sub txtOCommision_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOCommision_AMT.TextChanged
        CalcGrossAmount()
    End Sub

    Private Sub AdvanceWeightToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AdvanceWeightToolStripMenuItem.Click
        txtAdjAdvanceWt_GotFocus(Me, New EventArgs)
    End Sub

    Private Sub AdvanceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AdvanceToolStripMenuItem.Click
        txtAdjAdvance_AMT_GotFocus(Me, New EventArgs)
    End Sub

    Private Sub ChitCardToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChitCardToolStripMenuItem.Click
        txtAdjChitCard_AMT_GotFocus(Me, New EventArgs)
    End Sub

    Private Sub ChequeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChequeToolStripMenuItem.Click
        txtAdjCheque_AMT_GotFocus(Me, New EventArgs)
    End Sub

    Private Sub CreditCardToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CreditCardToolStripMenuItem.Click
        txtAdjCreditCard_AMT_GotFocus(Me, New EventArgs)
    End Sub

    Private Sub CashToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CashToolStripMenuItem.Click
        txtAdjCash_AMT.Focus()
    End Sub

    Private Sub WastageMcToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WastageMcToolStripMenuItem.Click
        txtOWastage_WET_KeyDown(Me, New KeyEventArgs(Keys.Insert))
    End Sub

    Private Sub txtAdjChitCard_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdjChitCard_AMT.GotFocus
        If objChitCard.Visible Then Exit Sub
        objChitCard.BackColor = pnlContainer_OWN.BackColor
        objChitCard.StartPosition = FormStartPosition.CenterScreen
        objChitCard.grpCHIT.BackgroundColor = grpHeader.BackgroundColor
        Select Case objChitCard.ShowDialog
            Case Windows.Forms.DialogResult.OK
                Dim chitAmt As Double = Val(objChitCard.gridCHITCardTotal.Rows(0).Cells("TOTAL").Value.ToString)
                txtAdjChitCard_AMT.Text = IIf(chitAmt <> 0, Format(chitAmt, "0.00"), Nothing)
                If chitAmt > 0 And objChitCard.gridCHITCard.RowCount > 0 Then
                    If objAddressDia.txtAddressName.Text = "" Then
                        strSql = " SELECT P.TITLE,P.INITIAL,P.PNAME,P.DOORNO,P.ADDRESS1,P.ADDRESS2,P.AREA"
                        strSql += " ,P.CITY,P.STATE,P.COUNTRY,P.PINCODE,P.EMAIL,P.FAX,P.PHONERES,P.MOBILE"
                        strSql += " FROM " & chitMainDb & "..PERSONALINFO AS P," & chitMainDb & "..SCHEMEMAST AS S"
                        strSql += " WHERE S.GROUPCODE = '" & objChitCard.gridCHITCard.Rows(0).Cells("GRPCODE").Value.ToString & "'"
                        strSql += " AND S.REGNO = '" & objChitCard.gridCHITCard.Rows(0).Cells("REGNO").Value.ToString & "'"
                        strSql += " AND P.PERSONALID = S.SNO"
                        Dim dtChitInfo As New DataTable
                        da = New OleDbDataAdapter(strSql, cn)
                        da.Fill(dtChitInfo)
                        If dtChitInfo.Rows.Count > 0 Then
                            With dtChitInfo.Rows(0)
                                objAddressDia.cmbAddressTitle_OWN.Text = .Item("TITLE").ToString
                                objAddressDia.txtAddressInitial.Text = .Item("INITIAL").ToString
                                objAddressDia.txtAddressName.Text = .Item("PNAME").ToString
                                objAddressDia.txtAddressDoorNo.Text = .Item("DOORNO").ToString
                                objAddressDia.txtAddress1.Text = .Item("ADDRESS1").ToString
                                objAddressDia.txtAddress2.Text = .Item("ADDRESS2").ToString
                                objAddressDia.cmbAddressArea_OWN.Text = .Item("AREA").ToString
                                objAddressDia.cmbAddressCity_OWN.Text = .Item("CITY").ToString
                                objAddressDia.cmbAddressState.Text = .Item("STATE").ToString
                                objAddressDia.cmbAddressCountry_OWN.Text = .Item("COUNTRY").ToString
                                objAddressDia.txtAddressPincode_NUM.Text = .Item("PINCODE").ToString
                                objAddressDia.txtAddressEmailId_OWN.Text = .Item("EMAIL").ToString
                                objAddressDia.txtAddressFax.Text = .Item("FAX").ToString
                                objAddressDia.txtAddressPhoneRes.Text = .Item("PHONERES").ToString
                                objAddressDia.txtAddressMobile.Text = .Item("MOBILE").ToString
                            End With
                        End If

                    End If
                End If

                txtAdjCash_AMT.Focus()
            Case Windows.Forms.DialogResult.Abort
                objChitCard = New frmChitAdj
                txtAdjCash_AMT.Focus()
        End Select
    End Sub

    Private Sub txtAdjChitCard_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAdjChitCard_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txtAdjCash_AMT.Focus()
        End If
    End Sub

    Private Sub txtSamTagNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSamTagNo.KeyDown
        If e.KeyCode = Keys.Insert Then
            If objGPack.GetSqlValue("SELECT STOCKTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtSamItem.Text & "'") = "T" Then
                strSql = " SELECT"
                strSql += " TAGNO AS TAGNO,ITEMID AS ITEMID,"
                strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEMNAME,"
                strSql += " PCS AS PCS,"
                strSql += " GRSWT AS GRS_WT,NETWT AS NET_WT,RATE AS RATE,"
                strSql += " SALVALUE AS SALVALUE,TAGVAL AS TAGVAL,"
                strSql += " ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID  = T.SUBITEMID),'')AS SUBITEM,"
                strSql += " CONVERT(VARCHAR,RECDATE,103) AS RECDATE,"
                strSql += " CASE WHEN APPROVAL = 'A' THEN 'APPROVAL' WHEN APPROVAL = 'R' THEN 'RESERVED' ELSE NULL END AS STATUS,"
                strSql += " CASE WHEN APPROVAL = 'A' THEN '" & Color.MistyRose.Name & "' WHEN APPROVAL = 'R' THEN '" & Color.PaleTurquoise.Name & "' ELSE NULL END AS COLOR_HIDE"
                strSql += " FROM"
                strSql += " " & cnAdminDb & "..ITEMTAG AS T"
                strSql += " WHERE T.ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtSamItem.Text & "')"
                strSql += " AND ISSDATE IS NULL"
                strSql += " ORDER BY TAGNO"
                txtSamTagNo.Text = BrighttechPack.SearchDialog.Show("Find TagNo", strSql, cn)
                txtSamTagNo.SelectAll()
            End If
        End If
    End Sub

    Private Sub txtSamTagNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSamTagNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtSamTagNo.Text = "" Then
                txtSamPcs_NUM.Focus()
                Exit Sub
            End If
            strSql = " SELECT * FROM " & cnAdminDb & "..ITEMTAG"
            strSql += " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtSamItem.Text & "')"
            strSql += " AND  TAGNO = '" & txtSamTagNo.Text & "'"
            strSql += " AND ISSDATE IS NULL"
            Dim dtTagCheck As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTagCheck)
            If dtTagCheck.Rows.Count > 0 Then
                txtSamGrsWt_WET.Text = dtTagCheck.Rows(0).Item("GRSWT").ToString
                txtSamNetWt_WET.Text = dtTagCheck.Rows(0).Item("NETWT").ToString
                LoadSampleDetails()
            Else
                MsgBox("Invalid TagNo", MsgBoxStyle.Information)
                txtSamTagNo.Focus()
            End If
        End If
    End Sub

    Private Sub txtSamTagNo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSamTagNo.LostFocus
        Main.HideHelpText()
    End Sub
    Public Sub Set916Rate(ByVal ddate As Date)
        strSql = " SELECT METALID,SRATE,PURITY FROM " & cnAdminDb & "..RATEMAST R"
        strSql += " WHERE RDATE = '" & ddate.ToString("yyyy-MM-dd") & "'"
        strSql += " AND RATEGROUP = (SELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST  WHERE RDATE = R.RDATE)"
        strSql += " AND PURITY BETWEEN 91.6 AND 92"
        If RATE_BRANCHWISE Then
            strSql += " AND COSTID='" & cnCostId & "'"
        End If
        strSql += " ORDER BY METALID,PURITY"
        Dim dtRate As New DataTable
        cmd = New OleDbCommand(strSql, cn, tran)
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtRate)
        Dim gRate As Double = Nothing
        Dim sRate As Double = Nothing
        For Each ro As DataRow In dtRate.Rows
            If ro!METALID.ToString = "G" Then
                gRate = Val(ro!SRATE.ToString)
            ElseIf ro!METALID.ToString = "S" Then
                sRate = Val(ro!SRATE.ToString)
            End If
        Next
        lblGoldRate.Text = IIf(gRate <> 0, Format(gRate, "0.00"), Nothing)
        lblSilverRate.Text = IIf(sRate <> 0, Format(sRate, "0.00"), Nothing)
    End Sub
    Private Sub GetAdvanceAddress()
        With objAdvance
            objAddressDia.AddressLock = True
            'objAddressDia.chkRegularCustomer.Enabled = False
            'objAddressDia.chkRegularCustomer.Checked = True
            objAddressDia.txtAddressPrevilegeId.Text = .txtAddressPrevilegeId.Text
            objAddressDia.txtAddressPartyCode.Text = .txtAddressPartyCode.Text
            objAddressDia.cmbAddressTitle_OWN.Text = .cmbAddressTitle_OWN.Text
            objAddressDia.txtAddressInitial.Text = .txtAddressInitial.Text
            objAddressDia.txtAddressName.Text = .txtAddressName.Text
            objAddressDia.txtAddressDoorNo.Text = .txtAddressDoorNo.Text
            objAddressDia.txtAddress1.Text = .txtAddress1.Text
            objAddressDia.txtAddress2.Text = .txtAddress2.Text
            objAddressDia.txtAddress3.Text = .txtAddress3.Text
            objAddressDia.cmbAddressArea_OWN.Text = .cmbAddressArea_OWN.Text
            objAddressDia.cmbAddressCity_OWN.Text = .cmbAddressCity_OWN.Text
            objAddressDia.cmbAddressState.Text = .cmbAddressState_OWN.Text
            objAddressDia.cmbAddressCountry_OWN.Text = .cmbAddressCountry_OWN.Text
            objAddressDia.txtAddressPincode_NUM.Text = .txtAddressPincode_NUM.Text
            objAddressDia.txtAddressEmailId_OWN.Text = .txtAddressEmailId_OWN.Text
            objAddressDia.txtAddressFax.Text = .txtAddressFax.Text
            objAddressDia.txtAddressPhoneRes.Text = .txtAddressPhoneRes.Text
            objAddressDia.txtAddressMobile.Text = .txtAddressMobile.Text
            objAddressDia.txtAddressRegularSno.Text = .txtAddressRegularSno.Text
        End With
    End Sub

    Private Sub txtSamGrsWt_WET_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSamGrsWt_WET.TextChanged
        txtSamNetWt_WET.Text = IIf(Val(txtSamGrsWt_WET.Text) <> 0, txtSamGrsWt_WET.Text, Nothing)
    End Sub

    Private Sub gridSample_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridSample.UserDeletedRow
        dtSampleDetails.AcceptChanges()
        If Not gridSample.RowCount > 0 Then cmbSamFrom.Focus()
    End Sub

    Private Sub txtOSize_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOSize.GotFocus
        If ManualSize Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub DiscountToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DiscountToolStripMenuItem.Click
        txtAdjDiscount_AMT.Select()
    End Sub

    Private Sub dtpOrderDate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpOrderDate.LostFocus
        Try
            BillDate = dtpOrderDate.Value
            Dim dt As New DateTimePicker
            dtpDueDate.MinimumDate = BillDate
            dtpDueDate.MaximumDate = dt.MaxDate
            dtpRemDate.MinimumDate = BillDate
            dtpRemDate.MaximumDate = dt.MaxDate
            txtDueDays_NUM_TextChanged(Me, New EventArgs)
            'txtRemDays_TextChanged(Me, New EventArgs)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtAdjDiscount_AMT_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdjDiscount_AMT.Leave
        If Val(txtAdjDiscount_AMT.Text) <> 0 Then
            If DiscBTorAT = "B" Then
                Dim avgdiscount As Double = Val(txtAdjDiscount_AMT.Text) / Val(dtGridOrder.Compute("SUM(GROSSAMT)", Nothing).ToString)
                For Each xRows As DataRow In dtGridOrder.Rows
                    xRows.Item("DISCOUNT") = Val(xRows.Item("GROSSAMT").ToString) * avgdiscount
                Next
            End If
        End If
    End Sub

    Private Sub txtAdjCash_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtAdjCash_AMT.KeyDown
        If e.KeyCode = Keys.Enter Then
            If objAddressDia.txtAddressName.Text = "" Then
                ShowAddressDia()
                objAddressDia.txtAddressName.Select()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub txtAdjAdvance_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdjAdvance_AMT.TextChanged _
    , txtAdjAdvanceWt.TextChanged, txtAdjCash_AMT.TextChanged _
    , txtAdjCheque_AMT.TextChanged, txtAdjChitCard_AMT.TextChanged _
    , txtAdjCreditCard_AMT.TextChanged _
    , txtAdjDiscount_AMT.TextChanged
        CalcBalanceAmount()
    End Sub

    Private Sub EstimateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EstimateToolStripMenuItem.Click
        'LoadOrderDetail()
        If LockSalesEst Then
            MsgBox("Sales Estimation is Locked.", MsgBoxStyle.Information)
            Exit Sub
        End If
        Dim Objestcall As New FRM_ESTIMATEISSNO

        If Objestcall.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If Objestcall.txtEstNo_NUM.Text = "" Then
                MsgBox("EstNo Should not Empty", MsgBoxStyle.Information)
                Exit Sub
            End If

            'For Each ro As DataRow In dtGridPur.Rows
            '    If ro!ENTFLAG.ToString = "" Then Exit For
            '    If ro!ESTNO.ToString = Objestcall.txtEstNo_NUM.Text Then
            '        MsgBox("This EstNo Already Loaded", MsgBoxStyle.Information)
            '        Exit Sub
            '    End If
            'Next


            Dim index As Integer = 0
            strSql = " SELECT *"
            strSql += " ,(SELECT CATNAME FROM  " & cnAdminDb & "..CATEGORY WHERE CATCODE = E.CATCODE)AS CATNAME"
            strSql += " FROM " & cnStockDb & "..ESTISSUE AS E"
            strSql += " WHERE TRANNO = " & Val(Objestcall.txtEstNo_NUM.Text) & ""
            strSql += " AND TRANTYPE = 'SA' AND ISNULL(CANCEL,'')=''"
            strSql += " AND TRANDATE = '" & BillDate.Date & "'"
            If READYITEMBOOKING And ChkReadyOrder.Checked Then
                strSql += " AND ISNULL(ORDERNO,'')=''"
                strSql += " AND ISNULL(ORSNO,'')=''"
            End If
            Dim dtEstDet As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtEstDet)
            If Not dtEstDet.Rows.Count > 0 Then
                MsgBox(Me.GetNextControl(Objestcall.txtEstNo_NUM, False).Text + E0022, MsgBoxStyle.Information)
                Exit Sub
            End If
            If dtEstDet.Rows(0).Item("BATCHNO").ToString <> "" Then
                MsgBox("Already Billed", MsgBoxStyle.Information)
                Exit Sub
            End If

            If dtEstDet.Rows.Count > 1000 Then MsgBox("Estimate Rows Should less than 1000", MsgBoxStyle.Information) : Exit Sub
            Ordedit(Val(Objestcall.txtEstNo_NUM.Text))
            StyleGridOrder(gridOrder)
            CalcGridOrderTotal()
            If READYITEMBOOKING And ChkReadyOrder.Checked Then
                txtAdjCash_AMT.Focus()
            End If
        End If
    End Sub

    Private Sub ContextMenuStrip1_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs)
        'LoadOrderDetail()
        Dim Objestcall As New FRM_ESTIMATEORDNO

        If Objestcall.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If Objestcall.txtEstNo_NUM.Text = "" Then
                MsgBox("EstNo Should not Empty", MsgBoxStyle.Information)
                Exit Sub
            End If

            'For Each ro As DataRow In dtGridPur.Rows
            '    If ro!ENTFLAG.ToString = "" Then Exit For
            '    If ro!ESTNO.ToString = Objestcall.txtEstNo_NUM.Text Then
            '        MsgBox("This EstNo Already Loaded", MsgBoxStyle.Information)
            '        Exit Sub
            '    End If
            'Next


            Dim index As Integer = 0
            strSql = " SELECT *"
            'strSql += " ,(SELECT CATNAME FROM  " & cnAdminDb & "..CATEGORY WHERE CATCODE = E.CATCODE)AS CATNAME"
            strSql += " FROM " & cnStockDb & "..ESTORMAST AS E"
            strSql += " WHERE ORNO = " & Val(Objestcall.txtEstNo_NUM.Text) & ""
            strSql += " AND ORTYPE = 'O' AND ISNULL(CANCEL,'')=''"
            strSql += " AND ORDATE = '" & BillDate.Date & "'"
            Dim dtEstDet As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtEstDet)
            If Not dtEstDet.Rows.Count > 0 Then
                MsgBox(Me.GetNextControl(Objestcall.txtEstNo_NUM, False).Text + E0022, MsgBoxStyle.Information)
                Exit Sub
            End If
            If dtEstDet.Rows(0).Item("ODBATCHNO").ToString <> "" Then
                MsgBox("Already Billed", MsgBoxStyle.Information)
                Exit Sub
            End If

            'OrderEstimateno = Val(Objestcall.txtEstNo_NUM.Text)

            Ordedit(Val(Objestcall.txtEstNo_NUM.Text))
            StyleGridOrder(gridOrder)
            CalcGridOrderTotal()
        End If
    End Sub


    Private Sub Ordedit(ByVal ESTNO As Int64)
        Dim dtEstoredt As New DataTable
        strSql = " SELECT TRANNO,SNO,ITEMID,SUBITEMID,TAGNO,(SELECT ITEMNAME FROM  " & cnAdminDb & "..ITEMMAST WHERE ITEMID = E.ITEMID)AS ITEMNAME ,"
        strSql += " PCS,GRSWT,NETWT,'' AS SIZE,RATE,WASTAGE,MCGRM,MCHARGE ,0 AS COM,"
        strSql += " CONVERT(NVARCHAR,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..ESTISSSTONE WHERE ISSSNO = E.SNO)) "
        strSql += " + '/' + "
        strSql += " CONVERT(NVARCHAR,(SELECT SUM(STNWT) FROM " & cnStockDb & "..ESTISSSTONE WHERE ISSSNO = E.SNO)) AS PCSAWT,"
        strSql += " (SELECT SUM(STNAMT) FROM " & cnStockDb & "..ESTISSSTONE WHERE ISSSNO = E.SNO)AS STNAMT ,"
        strSql += " AMOUNT,TAX"
        If READYITEMBOOKING And ChkReadyOrder.Checked Then
            strSql += " ,AMOUNT + TAX AS NETAMOUNT,"
        Else
            strSql += " ,AMOUNT - TAX AS NETAMOUNT,"
        End If
        strSql += " (SELECT TOP 1 EMPNAME FROM  " & cnAdminDb & "..EMPMASTER  WHERE empid = E.empid)AS EmpNAME,empid  "
        strSql += " FROM"
        strSql += " " & cnStockDb & "..ESTISSUE AS E "
        strSql += " WHERE"
        strSql += " TRANNO = " & ESTNO & " AND TRANTYPE = 'SA' "
        strSql += " AND ISNULL(BATCHNO,'') = '' " 'AND TRANDATE = GETDATE()"
        cmd = New OleDbCommand(strSql, cn, tran)
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtEstoredt)
        Dim i As Long = 0
        For Each ro As DataRow In dtGridOrder.Rows
            If ro!ENTFLAG.ToString = "" Then Exit For
            If ro!ESTNO.ToString = ESTNO.ToString Then
                MsgBox("Already Loaded", MsgBoxStyle.Information)
                txtOParticular.Focus()
                Exit Sub
            End If
            i += 1
        Next
        'gridOrder.DataSource = dtoredt



        For Each roEst As DataRow In dtEstoredt.Rows
            With gridOrder.Rows(i)
                If .Cells("ENTFLAG").ToString <> "Y" Then
                    .Cells("ESTNO").Value = ESTNO
                    .Cells("ITEMID").Value = roEst.Item("ITEMID")
                    .Cells("SUBITEMID").Value = roEst.Item("SUBITEMID")
                    .Cells("TAGNO").Value = roEst.Item("TAGNO").ToString
                    .Cells("PARTICULAR").Value = roEst.Item("ITEMNAME").ToString
                    .Cells("PCS").Value = Val(roEst.Item("PCS"))
                    .Cells("GRSWT").Value = Format(roEst.Item("GRSWT"), "0.000")
                    .Cells("NETWT").Value = Format(roEst.Item("NETWT"), "0.000")
                    '.Cells("COMM").Value = ""
                    .Cells("RATE").Value = IIf(Format(roEst.Item("RATE"), "0.000") <> 0, Format(roEst.Item("RATE"), "0.00"), DBNull.Value)
                    .Cells("WASTAGE").Value = IIf(Val(roEst.Item("WASTAGE")) > 0, Val(roEst.Item("WASTAGE")), DBNull.Value)
                    .Cells("MC").Value = IIf(Val(roEst.Item("MCGRM")) > 0, Val(roEst.Item("MCGRM").ToString), Val(roEst.Item("MCHARGE").ToString))
                    .Cells("COMM").Value = IIf(Val(roEst.Item("COM")) > 0, Val(roEst.Item("COM")), DBNull.Value)
                    .Cells("DIAPCSWT").Value = roEst.Item("PCSAWT").ToString
                    .Cells("OTHERAMT").Value = Val(roEst.Item("STNAMT") & "")
                    .Cells("GROSSAMT").Value = Val(roEst.Item("AMOUNT") & "")
                    .Cells("VAT").Value = IIf(Val(roEst.Item("TAX")) > 0, Val(roEst.Item("TAX")), DBNull.Value)
                    .Cells("AMOUNT").Value = Val(roEst.Item("NETAMOUNT") & "")
                    .Cells("SAMPLE").Value = IIf(chkOSample.Checked, "Y", "N")
                    .Cells("IMAGE").Value = IIf(chkOImage.Checked, "Y", "N")
                    .Cells("SAMPLEANDIMAGE").Value = IIf(chkOSample.Checked, "Y", "N") & IIf(chkOImage.Checked, "Y", "N")
                    .Cells("ENTFLAG").Value = "Y"
                    If Val(txtSalesMan_NUM.Text) < 1 Then
                        txtSalesMan_NUM.Text = Val(roEst.Item("empid"))
                        txtSalesManName.Text = roEst.Item("EmpNAME").ToString
                    End If

                    strSql = " SELECT 0 KEYNO,TRANTYPE"
                    strSql += " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = STNITEMID)AS ITEM"
                    strSql += " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = STNSUBITEMID)AS SUBITEM"
                    strSql += " ,STNPCS PCS,STNWT WEIGHT,STONEUNIT UNIT,CALCMODE CALC"
                    strSql += " ,STNRATE RATE,STNAMT AMOUNT,''METALID,0 DISCOUNT,STNPCS TAGSTNPCS,STNWT TAGSTNWT,TAGSNO"
                    strSql += " ,0 AS R_VAT,''ISSSNO"
                    strSql += " ,ISNULL(CUTID,0)CUTID,ISNULL(COLORID,0)COLORID,ISNULL(CLARITYID,0)CLARITYID"
                    strSql += " ,ISNULL(SHAPEID,0)SHAPEID,ISNULL(SETTYPEID,0)SETTYPEID,ISNULL(HEIGHT,0)HEIGHT ,ISNULL(WIDTH,0)WIDTH"
                    strSql += " ,0 SGST"
                    strSql += " ,0 CGST"
                    strSql += " ,0 IGST"
                    strSql += " FROM " & cnStockDb & "..ESTISSSTONE"
                    strSql += " WHERE ISSSNO = '" & roEst.Item("SNO").ToString & "'"
                    da = New OleDbDataAdapter(strSql, cn)
                    Dim st As New DataTable
                    da.Fill(st)
                    da.Fill(objStone.dtGridStone)
                    objStone.CalcStoneWtAmount()


                    dtStoneDetails.AcceptChanges()
                    For Each ro As DataRow In dtStoneDetails.Rows
                        If ro!KEYNO.ToString = .Cells("KEYNO").Value.ToString Then
                            ro.Delete()
                        End If
                    Next
                    dtStoneDetails.AcceptChanges()

                    ''Stone
                    Dim index As Integer = 0
                    For rwIndex As Integer = 0 To st.Rows.Count - 1
                        Dim ro As DataRow = dtStoneDetails.NewRow
                        ro("KEYNO") = .Cells("KEYNO").Value.ToString
                        For colIndex As Integer = 2 To objStone.dtGridStone.Columns.Count - 1
                            ro(colIndex) = st.Rows(rwIndex).Item(colIndex)
                        Next
                        dtStoneDetails.Rows.Add(ro)
                    Next
                    dtStoneDetails.AcceptChanges()


                    If objAddressDia.txtAddressRegularSno.Text <> "" Then
                        strSql = " SELECT SNO,PREVILEGEID,ACCODE,TRANDATE,TITLE"
                        strSql += " ,INITIAL,PNAME,DOORNO,ADDRESS1"
                        strSql += " ,ADDRESS2,ADDRESS3,AREA,CITY"
                        strSql += " ,STATE,COUNTRY,PINCODE,PHONERES"
                        strSql += " ,MOBILE,EMAIL,FAX"
                        strSql += " FROM " & cnAdminDb & "..PERSONALINFO"
                        strSql += " WHERE SNO = '" & roEst.Item("PSNO").ToString & "'"
                        Dim dtAddress As New DataTable
                        da = New OleDbDataAdapter(strSql, cn)
                        da.Fill(dtAddress)
                        If dtAddress.Rows.Count > 0 Then
                            With dtAddress.Rows(0)
                                objAddressDia.txtAddressPrevilegeId.Text = .Item("PREVILEGEID").ToString
                                objAddressDia.txtAddressPartyCode.Text = .Item("ACCODE").ToString
                                objAddressDia.cmbAddressTitle_OWN.Text = .Item("TITLE").ToString
                                objAddressDia.txtAddressInitial.Text = .Item("INITIAL").ToString
                                objAddressDia.txtAddressName.Text = .Item("PNAME").ToString
                                objAddressDia.txtAddressDoorNo.Text = .Item("DOORNO").ToString
                                objAddressDia.txtAddress1.Text = .Item("ADDRESS1").ToString
                                objAddressDia.txtAddress2.Text = .Item("ADDRESS2").ToString
                                objAddressDia.txtAddress3.Text = .Item("ADDRESS3").ToString
                                objAddressDia.cmbAddressArea_OWN.Text = .Item("AREA").ToString
                                objAddressDia.cmbAddressCity_OWN.Text = .Item("CITY").ToString
                                objAddressDia.cmbAddressState.Text = .Item("STATE").ToString
                                objAddressDia.cmbAddressCountry_OWN.Text = .Item("COUNTRY").ToString
                                objAddressDia.txtAddressPincode_NUM.Text = .Item("PINCODE").ToString
                                objAddressDia.txtAddressPhoneRes.Text = .Item("PHONERES").ToString
                                objAddressDia.txtAddressMobile.Text = .Item("MOBILE").ToString
                                objAddressDia.txtAddressEmailId_OWN.Text = .Item("EMAIL").ToString
                                objAddressDia.txtAddressFax.Text = .Item("FAX").ToString
                                objAddressDia.EstSno = .Item("SNO").ToString()
                            End With
                        End If
                    End If
                End If
            End With
            i = i + 1
            'If gridOrder.Rows.Count - 1 < i Then gridOrder.Rows.Add()
        Next
    End Sub

    Private Sub OrderEstimateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OrderEstimateToolStripMenuItem.Click
        'LoadOrderDetail()
        Dim Objestcall As New FRM_ESTIMATEORDNO

        If Objestcall.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If Objestcall.txtEstNo_NUM.Text = "" Then
                MsgBox("EstNo Should not Empty", MsgBoxStyle.Information)
                Exit Sub
            End If

            'For Each ro As DataRow In dtGridPur.Rows
            '    If ro!ENTFLAG.ToString = "" Then Exit For
            '    If ro!ESTNO.ToString = Objestcall.txtEstNo_NUM.Text Then
            '        MsgBox("This EstNo Already Loaded", MsgBoxStyle.Information)
            '        Exit Sub
            '    End If
            'Next


            Dim index As Integer = 0
            strSql = " SELECT *"
            'strSql += " ,(SELECT CATNAME FROM  " & cnAdminDb & "..CATEGORY WHERE CATCODE = E.CATCODE)AS CATNAME"
            strSql += " FROM " & cnStockDb & "..ESTORMAST AS E"
            strSql += " WHERE ORNO = " & Val(Objestcall.txtEstNo_NUM.Text) & ""
            strSql += " AND ORTYPE = 'O' AND ISNULL(CANCEL,'')=''"
            strSql += " AND ORDATE = '" & BillDate.Date & "'"
            Dim dtEstDet As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtEstDet)
            If Not dtEstDet.Rows.Count > 0 Then
                MsgBox(Me.GetNextControl(Objestcall.txtEstNo_NUM, False).Text + E0022, MsgBoxStyle.Information)
                Exit Sub
            End If
            If dtEstDet.Rows(0).Item("ODBATCHNO").ToString <> "" Then
                MsgBox("Already Billed", MsgBoxStyle.Information)
                Exit Sub
            End If

            If dtEstDet.Rows.Count > 1000 Then MsgBox("Estimate Rows Should less than 1000", MsgBoxStyle.Information) : Exit Sub
            EstimateOrdedit(Val(Objestcall.txtEstNo_NUM.Text))
            StyleGridOrder(gridOrder)
            CalcGridOrderTotal()
            txtOItemId.Focus()
        End If
    End Sub


    Private Sub EstimateOrdedit(ByVal ESTNO As Int64)
        Dim dtEstoredt As New DataTable
        strSql = " SELECT ORNO,ORRATE,ORMODE,DUEDATE,REMDATE,SIZEID,SIZENO,SNO,ITEMID,SUBITEMID,DESCRIPT,''TAGNO,(SELECT ITEMNAME FROM  " & cnAdminDb & "..ITEMMAST WHERE ITEMID = E.ITEMID)AS ITEMNAME ,(SELECT NAME FROM  " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = E.ITEMTYPEID)AS ITEMTYPE ,"
        strSql += " PCS,GRSWT,NETWT,'' AS SIZE,RATE,WAST,MC ,WASTPER,MCGRM,0 AS COM,"
        strSql += " CONVERT(NVARCHAR,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..ESTORSTONE WHERE ORSNO = E.SNO)) "
        strSql += " + '/' + "
        strSql += " CONVERT(NVARCHAR,(SELECT SUM(STNWT) FROM " & cnStockDb & "..ESTORSTONE WHERE ORSNO = E.SNO)) AS PCSAWT,"
        strSql += " (SELECT SUM(STNAMT) FROM " & cnStockDb & "..ESTORSTONE WHERE ORSNO = E.SNO)AS STNAMT ,"
        strSql += " ORVALUE AMOUNT,TAX,ORVALUE + TAX AS NETAMOUNT,DUEDATE,REMDATE,DATEDIFF(DAY,ORDATE,DUEDATE) DUEDAYS,DATEDIFF(DAY,REMDATE,DUEDATE) REMDAYS,"
        strSql += " (SELECT TOP 1 EMPNAME FROM  " & cnAdminDb & "..EMPMASTER  WHERE EMPID = E.EMPID)AS EMPNAME,EMPID,PSNO "
        strSql += " FROM"
        strSql += " " & cnStockDb & "..ESTORMAST AS E "
        strSql += " WHERE"
        strSql += " ORNO = " & ESTNO & " AND ORTYPE = 'O' "
        'strSql += " AND ISNULL(BATCHNO,'') = '' AND TRANDATE = GETDATE()"
        strSql += " AND ISNULL(ODBATCHNO,'') = '' "
        cmd = New OleDbCommand(strSql, cn, tran)
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtEstoredt)

        For Each ro As DataRow In dtoredt.Rows
            ' If ro!ENTFLAG.ToString = "" Then Exit For
            If ro!ORNO.ToString = ESTNO.ToString Then
                MsgBox("Already Loaded", MsgBoxStyle.Information)
                txtOParticular.Focus()
                Exit Sub
            End If
        Next
        'gridOrder.DataSource = dtoredt
        dtoredt.Merge(dtEstoredt)
        Dim i As Int16 = 0

        For Each roEst As DataRow In dtoredt.Rows
            With gridOrder.Rows(i)
                If .Cells("ENTFLAG").ToString <> "Y" Then
                    If roEst.Item("ORRATE").ToString = "D" Then rbtDeliveryRate.Checked = True Else rbtCurrentRate.Checked = True
                    If roEst.Item("ORMODE").ToString = "C" Then rbtCustomerOrderType.Checked = True
                    If roEst.Item("ORMODE").ToString = "O" Then rbtCompanyOrderType.Checked = True
                    If roEst.Item("ORMODE").ToString = "S" Then rbtSchemeOrder.Checked = True
                    dtpDueDate.Value = roEst.Item("DUEDATE")
                    dtpRemDate.Value = roEst.Item("REMDATE")
                    .Cells("SIZENO").Value = roEst.Item("SIZENO").ToString
                    .Cells("ESTNO").Value = ESTNO
                    .Cells("ITEMID").Value = roEst.Item("ITEMID")
                    .Cells("SUBITEMID").Value = roEst.Item("SUBITEMID")
                    .Cells("TAGNO").Value = roEst.Item("TAGNO").ToString
                    .Cells("PARTICULAR").Value = roEst.Item("ITEMNAME").ToString
                    .Cells("DESCRIPTION").Value = roEst.Item("DESCRIPT").ToString
                    .Cells("ITEMTYPE").Value = roEst.Item("ITEMTYPE").ToString
                    .Cells("PCS").Value = Val(roEst.Item("PCS"))
                    .Cells("GRSWT").Value = Format(roEst.Item("GRSWT"), "0.000")
                    .Cells("NETWT").Value = Format(roEst.Item("NETWT"), "0.000")
                    '.Cells("COMM").Value = ""
                    .Cells("RATE").Value = IIf(Format(roEst.Item("RATE"), "0.000") <> 0, Format(roEst.Item("RATE"), "0.00"), DBNull.Value)
                    .Cells("WASTAGE").Value = IIf(Val(roEst.Item("WAST")) > 0, Val(roEst.Item("WAST")), DBNull.Value)
                    .Cells("MC").Value = IIf(Val(roEst.Item("MC")) > 0, Val(roEst.Item("MC")), DBNull.Value)

                    .Cells("WASTAGEPER").Value = IIf(Val(roEst.Item("WASTPER")) > 0, Val(roEst.Item("WASTPER")), DBNull.Value)
                    .Cells("MCGRM").Value = IIf(Val(roEst.Item("MCGRM")) > 0, Val(roEst.Item("MCGRM")), DBNull.Value)

                    .Cells("COMM").Value = IIf(Val(roEst.Item("COM")) > 0, Val(roEst.Item("COM")), DBNull.Value)
                    .Cells("DIAPCSWT").Value = roEst.Item("PCSAWT").ToString
                    .Cells("OTHERAMT").Value = Val(roEst.Item("STNAMT") & "")
                    .Cells("GROSSAMT").Value = Val(roEst.Item("AMOUNT") & "")
                    .Cells("VAT").Value = IIf(Val(roEst.Item("TAX")) > 0, Val(roEst.Item("TAX")), DBNull.Value)
                    .Cells("AMOUNT").Value = Val(roEst.Item("NETAMOUNT") & "")
                    .Cells("SAMPLE").Value = IIf(chkOSample.Checked, "Y", "N")
                    .Cells("IMAGE").Value = IIf(chkOImage.Checked, "Y", "N")
                    .Cells("SAMPLEANDIMAGE").Value = IIf(chkOSample.Checked, "Y", "N") & IIf(chkOImage.Checked, "Y", "N")
                    .Cells("ENTFLAG").Value = "Y"
                    If Val(txtSalesMan_NUM.Text) < 1 Then
                        txtSalesMan_NUM.Text = Val(roEst.Item("empid"))
                        txtSalesManName.Text = roEst.Item("EmpNAME").ToString
                    End If
                    dtpDueDate.Value = roEst.Item("DUEDATE")
                    dtpRemDate.Value = roEst.Item("REMDATE")
                    txtDueDays_NUM.Text = Val(roEst.Item("DUEDAYS"))
                    txtRemDays_NUM.Text = Val(roEst.Item("REMDAYS"))
                    strSql = " SELECT 0 KEYNO,TRANTYPE"
                    strSql += " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = STNITEMID)AS ITEM"
                    strSql += " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = STNSUBITEMID)AS SUBITEM"
                    strSql += " ,STNPCS PCS,STNWT WEIGHT,STONEUNIT UNIT,CALCMODE CALC,STNRATE RATE,STNAMT AMOUNT,''METALID,STNPCS TAGSTNPCS,STNWT TAGSTNWT,TAGSNO"
                    strSql += " FROM " & cnStockDb & "..ESTISSSTONE"
                    strSql += " WHERE ISSSNO = '" & roEst.Item("SNO").ToString & "'"
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(objStone.dtGridStone)
                    objStone.CalcStoneWtAmount()


                    ''Stone
                    Dim index As Integer = 0
                    For rwIndex As Integer = 0 To objStone.dtGridStone.Rows.Count - 1
                        Dim ro As DataRow = dtStoneDetails.NewRow
                        ro("KEYNO") = dtGridOrder.Rows(index).Item("KEYNO").ToString
                        For colIndex As Integer = 2 To objStone.dtGridStone.Columns.Count - 1
                            ro(colIndex) = objStone.dtGridStone.Rows(rwIndex).Item(colIndex)
                        Next
                        dtStoneDetails.Rows.Add(ro)
                    Next
                    dtStoneDetails.AcceptChanges()


                    If roEst.Item("PSNO").ToString <> "" Then
                        strSql = " SELECT SNO,PREVILEGEID,ACCODE,TRANDATE,TITLE"
                        strSql += " ,INITIAL,PNAME,DOORNO,ADDRESS1"
                        strSql += " ,ADDRESS2,ADDRESS3,AREA,CITY"
                        strSql += " ,STATE,COUNTRY,PINCODE,PHONERES"
                        strSql += " ,MOBILE,EMAIL,FAX"
                        strSql += " FROM " & cnAdminDb & "..PERSONALINFO"
                        strSql += " WHERE SNO = '" & roEst.Item("PSNO").ToString & "'"
                        Dim dtAddress As New DataTable
                        da = New OleDbDataAdapter(strSql, cn)
                        da.Fill(dtAddress)
                        If dtAddress.Rows.Count > 0 Then
                            With dtAddress.Rows(0)
                                objAddressDia.txtAddressPrevilegeId.Text = .Item("PREVILEGEID").ToString
                                objAddressDia.txtAddressPartyCode.Text = .Item("ACCODE").ToString
                                objAddressDia.cmbAddressTitle_OWN.Text = .Item("TITLE").ToString
                                objAddressDia.txtAddressInitial.Text = .Item("INITIAL").ToString
                                objAddressDia.txtAddressName.Text = .Item("PNAME").ToString
                                objAddressDia.txtAddressDoorNo.Text = .Item("DOORNO").ToString
                                objAddressDia.txtAddress1.Text = .Item("ADDRESS1").ToString
                                objAddressDia.txtAddress2.Text = .Item("ADDRESS2").ToString
                                objAddressDia.txtAddress3.Text = .Item("ADDRESS3").ToString
                                objAddressDia.cmbAddressArea_OWN.Text = .Item("AREA").ToString
                                objAddressDia.cmbAddressCity_OWN.Text = .Item("CITY").ToString
                                objAddressDia.cmbAddressState.Text = .Item("STATE").ToString
                                objAddressDia.cmbAddressCountry_OWN.Text = .Item("COUNTRY").ToString
                                objAddressDia.txtAddressPincode_NUM.Text = .Item("PINCODE").ToString
                                objAddressDia.txtAddressPhoneRes.Text = .Item("PHONERES").ToString
                                objAddressDia.txtAddressMobile.Text = .Item("MOBILE").ToString
                                objAddressDia.txtAddressEmailId_OWN.Text = .Item("EMAIL").ToString
                                objAddressDia.txtAddressFax.Text = .Item("FAX").ToString
                                objAddressDia.EstSno = .Item("SNO").ToString()
                            End With
                        End If
                    End If
                End If
            End With
            i = i + 1
        Next
        orderestimate = True
    End Sub

    Private Sub dtpDueDate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpDueDate.GotFocus
        If Val(txtDueDays_NUM.Text) <> 0 Then dtpDueDate.ReadOnly = True Else dtpDueDate.ReadOnly = False
    End Sub

    Private Sub dtpRemDate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpRemDate.GotFocus
        If Val(txtRemDays_NUM.Text) <> 0 Then dtpRemDate.ReadOnly = True Else dtpRemDate.ReadOnly = False
    End Sub

    Private Sub BillTypeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BillTypeToolStripMenuItem.Click
        If GST Then
            Dim f As New frmState
            If f.ShowDialog() = Windows.Forms.DialogResult.OK Then
                If f.rbtOwn.Checked Then
                    InterStateBill = False
                Else
                    InterStateBill = True
                End If
            End If
        End If
    End Sub

    Private Sub txtRec_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        CalcGst()
    End Sub

    Private Sub AddressToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddressToolStripMenuItem.Click
        ShowAddressDia()
    End Sub
End Class