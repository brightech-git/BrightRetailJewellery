Imports System.Xml
Imports System.Data.OleDb
Public Class frmTallyExportxmlVoucher3
#Region "VARIABLE"
    Dim strsql As String
    Dim dt As DataTable
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim dtCostCentre As DataTable
    Dim dtCompany As DataTable
#End Region
#Region "FORM LOAD"
    Private Sub frmXmlGenerateTally_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnNew_Click(Me, New System.EventArgs)
    End Sub

    Private Sub frmXmlGenerateTally_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
#End Region
#Region "BUTTON EVENTS"
    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If gridView.Rows.Count = 0 Then
            MsgBox("No Record Load...", MsgBoxStyle.Information)
            dtpAsOnDate.Focus()
            Exit Sub
        End If
        If txtPath.Text.Trim = "" Then
            MsgBox("Enter the Correct Path", MsgBoxStyle.Information)
            txtPath.Focus()
            txtPath.SelectAll()
            Exit Sub
        End If
        'If My.Computer.Network.Ping(txtPath.Text.Trim) = True Then
        '    MsgBox("INVALID PATH...", MsgBoxStyle.Information)
        '    Exit Sub
        'End If
        Dim settings As XmlWriterSettings = New XmlWriterSettings()
        settings.Indent = True
        Dim dr As DataRow = Nothing
        Dim phone As String = ""
        strsql = "SELECT * FROM " & cnAdminDb & "..COMPANY WHERE COMPANYID = '" & strCompanyId & "'"
        dr = GetSqlRow(strsql, cn)
        If Not dr Is Nothing Then
            phone = dr.Item("PHONE").ToString
        End If

        Dim Path As String = txtPath.Text.Trim & "saveBill.xml"
        Try
            Using writer As XmlWriter = XmlWriter.Create(Path, settings) 'E:\employees.xml
                ' Begin writing.
                writer.WriteStartDocument() 'START ROOT
                writer.WriteStartElement("ENVELOPE") ' START ENVELOPE.

                'START HEADER
                writer.WriteStartElement("HEADER")
                writer.WriteElementString("TALLYREQUEST", "Import Data")
                writer.WriteEndElement()
                'END HEADER

                writer.WriteStartElement("BODY") 'START BODY
                writer.WriteStartElement("IMPORTDATA") 'START IMPORTDATA
                writer.WriteStartElement("REQUESTDESC") 'START REQUESTDESC
                writer.WriteElementString("REPORTNAME", "All Masters")
                writer.WriteStartElement("STATICVARIABLES") 'START REQUESTDESC
                writer.WriteElementString("SVCURRENTCOMPANY", "" & strCompanyName & cnTranFromDate.Year & "-" & cnTranToDate.Year)
                writer.WriteEndElement() 'END STATICVARIABLES
                writer.WriteEndElement() 'END REQUESTDESC

                writer.WriteStartElement("REQUESTDATA") 'START REQUESTDATA
                For i As Integer = 0 To gridView.Rows.Count - 1
                    If gridView.Rows(i).Cells("CHK").Value.ToString = "" Then Continue For
                    writer.WriteStartElement("TALLYMESSAGE") 'START TALLYMESSATE
                    writer.WriteAttributeString("xmlns", "UDF", Nothing, "TallyUDF") 'writer.WriteAttributeString("ISBN", "urn:book", "1-800-925")
                    writer.WriteStartElement("VOUCHER") 'START VOUCHER
                    writer.WriteAttributeString("REMOTEID", "REMOTEID", Nothing, "")
                    writer.WriteAttributeString("VCHKEY", "VCHKEY", Nothing, "") '81a991b8-2203-4ebf-b6bd-ee76150085dd-0000a913:00000088

                    If cnCostId = "BR" Then
                        writer.WriteAttributeString("VCHTYPE", "VCHTYPE", Nothing, "Gst Sale BO")
                    ElseIf cnCostId = "GV" Then
                        writer.WriteAttributeString("VCHTYPE", "VCHTYPE", Nothing, "GST Sales")
                    Else
                        writer.WriteAttributeString("VCHTYPE", "VCHTYPE", Nothing, "GST SALES " & Trim(cnCostId))
                    End If

                    writer.WriteAttributeString("ACTION", "ACTION", Nothing, "Create")
                    writer.WriteAttributeString("OBJVIEW", "OBJVIEW", Nothing, "Invoice Voucher View")


                    writer.WriteStartElement("ADDRESS.LIST") 'START ADDRESS.LIST
                    writer.WriteAttributeString("TYPE", "TYPE", Nothing, "String")
                    writer.WriteElementString("ADDRESS", "Phone No: " & phone & "")
                    writer.WriteEndElement() 'END ADDRESS.LIST

                    writer.WriteStartElement("BASICBUYERADDRESS.LIST") 'START BASICBUYERADDRESS.LIST
                    writer.WriteAttributeString("TYPE", "TYPE", Nothing, "String")
                    writer.WriteElementString("BASICBUYERADDRESS", gridView.Rows(i).Cells("PNAME").Value.ToString) 'NANI GARU CH
                    writer.WriteElementString("BASICBUYERADDRESS", gridView.Rows(i).Cells("AREA").Value.ToString)
                    writer.WriteEndElement() 'END BASICBUYERADDRESS.LIST


                    writer.WriteStartElement("OLDAUDITENTRYIDS.LIST") 'START OLDAUDITENTRYIDS.LIST
                    writer.WriteAttributeString("TYPE", "TYPE", Nothing, "Number")
                    writer.WriteElementString("OLDAUDITENTRYIDS", "-1")
                    writer.WriteEndElement() 'END OLDAUDITENTRYIDS.LIST



                    writer.WriteElementString("DATE", gridView.Rows(i).Cells("TRANDATE").Value.ToString) '"20180703"
                    writer.WriteElementString("GUID", " ")
                    writer.WriteElementString("STATENAME", gridView.Rows(i).Cells("STATE").Value.ToString)
                    writer.WriteElementString("GSTREGISTRATIONTYPE", "Consumer")
                    writer.WriteElementString("VATDEALERTYPE", "Unregistered")
                    writer.WriteElementString("NARRATION", "Being Cash Sales Vide B.No:  " & gridView.Rows(i).Cells("TRANNO").Value.ToString & "")
                    writer.WriteElementString("COUNTRYOFRESIDENCE", "India")
                    writer.WriteElementString("PARTYNAME", "Cash")
                    writer.WriteElementString("VOUCHERTYPENAME", "GST Sales")
                    writer.WriteElementString("VOUCHERNUMBER", "HO-" & gridView.Rows(i).Cells("TRANNO").Value.ToString & "")
                    writer.WriteElementString("PARTYLEDGERNAME", "Cash")
                    writer.WriteElementString("BASICBASEPARTYNAME", "Cash")
                    writer.WriteStartElement("CSTFORMISSUETYPE") 'START CSTFORMISSUETYPE
                    writer.WriteEndElement() 'END CSTFORMISSUETYPE
                    writer.WriteStartElement("CSTFORMRECVTYPE") 'START CSTFORMRECVTYPE
                    writer.WriteEndElement() 'END CSTFORMRECVTYPE
                    writer.WriteElementString("FBTPAYMENTTYPE", "Default")
                    writer.WriteElementString("PERSISTEDVIEW", "Invoice Voucher View")
                    writer.WriteElementString("PLACEOFSUPPLY", gridView.Rows(i).Cells("STATE").Value.ToString)
                    writer.WriteElementString("BASICBUYERNAME", "Cash")
                    writer.WriteElementString("BASICDATETIMEOFINVOICE", Now)
                    writer.WriteElementString("BASICDATETIMEOFREMOVAL", Now)
                    writer.WriteStartElement("VCHGSTCLASS") 'START VCHGSTCLASS
                    writer.WriteEndElement() 'END VCHGSTCLASS
                    writer.WriteElementString("CONSIGNEESTATENAME", gridView.Rows(i).Cells("STATE").Value.ToString)
                    'writer.WriteElementString("ENTEREDBY", cnUserName)
                    writer.WriteElementString("DIFFACTUALQTY", "No")
                    writer.WriteElementString("ISMSTFROMSYNC", "No")
                    writer.WriteElementString("ASORIGINAL", "No")
                    writer.WriteElementString("AUDITED", "No")
                    writer.WriteElementString("FORJOBCOSTING", "No")
                    writer.WriteElementString("ISOPTIONAL", "No")
                    writer.WriteElementString("EFFECTIVEDATE", "No")
                    writer.WriteElementString("USEFOREXCISE", "No")
                    writer.WriteElementString("ISFORJOBWORKIN", "No")
                    writer.WriteElementString("ALLOWCONSUMPTION", "No")
                    writer.WriteElementString("USEFORINTEREST", "No")
                    writer.WriteElementString("USEFORGAINLOSS", "No")
                    writer.WriteElementString("USEFORGODOWNTRANSFER", "No")
                    writer.WriteElementString("USEFORCOMPOUND", "No")
                    writer.WriteElementString("USEFORSERVICETAX", "No")
                    writer.WriteElementString("ISEXCISEVOUCHER", "No")
                    writer.WriteElementString("EXCISETAXOVERRIDE", "No")
                    writer.WriteElementString("USEFORTAXUNITTRANSFER", "No")
                    writer.WriteElementString("EXCISEOPENING", "No")
                    writer.WriteElementString("USEFORFINALPRODUCTION", "No")
                    writer.WriteElementString("ISTDSOVERRIDDEN", "No")
                    writer.WriteElementString("ISTCSOVERRIDDEN", "No")
                    writer.WriteElementString("ISTDSTCSCASHVCH", "No")
                    writer.WriteElementString("INCLUDEADVPYMTVCH", "No")
                    writer.WriteElementString("ISSUBWORKSCONTRACT", "No")
                    writer.WriteElementString("ISVATOVERRIDDEN", "No")
                    writer.WriteElementString("IGNOREORIGVCHDATE", "No")
                    writer.WriteElementString("ISVATPAIDATCUSTOMS", "No")
                    writer.WriteElementString("ISDECLAREDTOCUSTOMS", "No")
                    writer.WriteElementString("ISSERVICETAXOVERRIDDEN", "No")
                    writer.WriteElementString("ISISDVOUCHER", "No")
                    writer.WriteElementString("ISEXCISEOVERRIDDEN", "No")
                    writer.WriteElementString("ISEXCISESUPPLYVCH", "No")
                    writer.WriteElementString("ISGSTOVERRIDDEN", "No")
                    writer.WriteElementString("GSTNOTEXPORTED", "No")
                    writer.WriteElementString("IGNOREGSTINVALIDATION", "No")
                    writer.WriteElementString("ISVATPRINCIPALACCOUNT", "No")
                    writer.WriteElementString("ISBOENOTAPPLICABLE", "No")
                    writer.WriteElementString("ISSHIPPINGWITHINSTATE", "No")
                    writer.WriteElementString("ISOVERSEASTOURISTTRANS", "No")
                    writer.WriteElementString("ISDESIGNATEDZONEPARTY", "No")
                    writer.WriteElementString("ISCANCELLED", "No")
                    writer.WriteElementString("HASCASHFLOW", "No")
                    writer.WriteElementString("ISPOSTDATED", "No")
                    writer.WriteElementString("USETRACKINGNUMBER", "No")
                    writer.WriteElementString("ISINVOICE", "Yes")
                    writer.WriteElementString("MFGJOURNAL", "No")
                    writer.WriteElementString("HASDISCOUNTS", "No")
                    writer.WriteElementString("ASPAYSLIP", "No")
                    writer.WriteElementString("ISCOSTCENTRE", "No")
                    writer.WriteElementString("ISSTXNONREALIZEDVCH", "No")
                    writer.WriteElementString("ISEXCISEMANUFACTURERON", "No")
                    writer.WriteElementString("ISBLANKCHEQUE", "No")
                    writer.WriteElementString("ISVOID", "No")
                    writer.WriteElementString("ISONHOLD", "No")
                    writer.WriteElementString("ORDERLINESTATUS", "No")
                    writer.WriteElementString("VATISAGNSTCANCSALES", "No")
                    writer.WriteElementString("VATISPURCEXEMPTED", "No")
                    writer.WriteElementString("ISVATRESTAXINVOICE", "No")
                    writer.WriteElementString("VATISASSESABLECALCVCH", "No")
                    writer.WriteElementString("ISVATDUTYPAID", "Yes")
                    writer.WriteElementString("ISDELIVERYSAMEASCONSIGNEE", "No")
                    writer.WriteElementString("ISDISPATCHSAMEASCONSIGNOR", "No")
                    writer.WriteElementString("ISDELETED", "No")
                    writer.WriteElementString("CHANGEVCHMODE", "No")
                    writer.WriteElementString("ALTERID", "28410")
                    writer.WriteElementString("MASTERID", "19315")
                    writer.WriteElementString("VOUCHERKEY", "185899069472904")
                    writer.WriteElementString("EXCLUDEDTAXATIONS.LIST", "    ")
                    writer.WriteElementString("OLDAUDITENTRIES.LIST", "     ")
                    writer.WriteElementString("ACCOUNTAUDITENTRIES.LIST", "    ")
                    writer.WriteElementString("AUDITENTRIES.LIST", "    ")
                    writer.WriteElementString("DUTYHEADDETAILS.LIST", "   ")
                    writer.WriteElementString("SUPPLEMENTARYDUTYHEADDETAILS.LIST", "    ")
                    writer.WriteElementString("EWAYBILLDETAILS.LIST", "    ")
                    writer.WriteElementString("INVOICEDELNOTES.LIST", "    ")
                    writer.WriteElementString("INVOICEORDERLIST.LIST", "    ")
                    writer.WriteElementString("INVOICEINDENTLIST.LIST", "    ")
                    writer.WriteElementString("ATTENDANCEENTRIES.LIST", "   ")
                    writer.WriteElementString("ORIGINVOICEDETAILS.LIST", "    ")
                    writer.WriteElementString("INVOICEEXPORTLIST.LIST", "   ")

                    Dim dtPaymode As New DataTable
                    strsql = " SELECT AMOUNT FROM " & cnStockDb & "..ACCTRAN "
                    strsql += vbCrLf + " WHERE TRANDATE = '" & Format(gridView.Rows(i).Cells("BILLDATE").Value, "yyyy-MM-dd") & "' "
                    strsql += vbCrLf + " AND BATCHNO = '" & (gridView.Rows(i).Cells("BATCHNO").Value.ToString) & "' "
                    strsql += vbCrLf + " And ISNULL(CANCEL,'') = '' AND TRANMODE = 'D'"
                    da = New OleDbDataAdapter(strsql, cn)
                    dtPaymode = New DataTable
                    da.Fill(dtPaymode)
                    For J As Integer = 0 To dtPaymode.Rows.Count - 1
                        '/*WRITE LOOP*
                        writer.WriteStartElement("LEDGERENTRIES.LIST") 'START LEDGERENTRIES.LIST
                        writer.WriteStartElement("OLDAUDITENTRYIDS.LIST") 'START OLDAUDITENTRYIDS.LIST
                        writer.WriteAttributeString("TYPE", "TYPE", Nothing, "Number")
                        writer.WriteElementString("OLDAUDITENTRYIDS", "-1")
                        writer.WriteEndElement() 'END OLDAUDITENTRYIDS.LIST
                        writer.WriteElementString("LEDGERNAME", "Cash")
                        writer.WriteElementString("GSTCLASS", "")
                        writer.WriteElementString("ISDEEMEDPOSITIVE", "Yes")
                        writer.WriteElementString("LEDGERFROMITEM", "No")
                        writer.WriteElementString("REMOVEZEROENTRIES", "No")
                        writer.WriteElementString("ISPARTYLEDGER", "Yes")
                        writer.WriteElementString("ISLASTDEEMEDPOSITIVE", "Yes")
                        writer.WriteElementString("ISCAPVATTAXALTERED", "No")
                        writer.WriteElementString("ISCAPVATNOTCLAIMED", "No")
                        writer.WriteElementString("AMOUNT", "-" & dtPaymode.Rows(J).Item("amount").ToString & "")
                        writer.WriteElementString("SERVICETAXDETAILS.LIST", "    ")
                        writer.WriteElementString("BANKALLOCATIONS.LIST", "    ")
                        writer.WriteElementString("BILLALLOCATIONS.LIST", "     ")
                        writer.WriteElementString("INTERESTCOLLECTION.LIST", "     ")
                        writer.WriteElementString("OLDAUDITENTRIES.LIST", "    ")
                        writer.WriteElementString("ACCOUNTAUDITENTRIES.LIST", "     ")
                        writer.WriteElementString("AUDITENTRIES.LIST", "     ")
                        writer.WriteElementString("INPUTCRALLOCS.LIST", "     ")
                        writer.WriteElementString("DUTYHEADDETAILS.LIST", "     ")
                        writer.WriteElementString("EXCISEDUTYHEADDETAILS.LIST", "     ")
                        writer.WriteElementString("RATEDETAILS.LIST", "     ")
                        writer.WriteElementString("SUMMARYALLOCS.LIST", "    ")
                        writer.WriteElementString("STPYMTDETAILS.LIST", "    ")
                        writer.WriteElementString("EXCISEPAYMENTALLOCATIONS.LIST", "    ")
                        writer.WriteElementString("TAXBILLALLOCATIONS.LIST", "     ")
                        writer.WriteElementString("TAXOBJECTALLOCATIONS.LIST", "     ")
                        writer.WriteElementString("TDSEXPENSEALLOCATIONS.LIST", "     ")
                        writer.WriteElementString("VATSTATUTORYDETAILS.LIST", "     ")
                        writer.WriteElementString("COSTTRACKALLOCATIONS.LIST", "     ")
                        writer.WriteElementString("REFVOUCHERDETAILS.LIST", "    ")
                        writer.WriteElementString("INVOICEWISEDETAILS.LIST", "     ")
                        writer.WriteElementString("VATITCDETAILS.LIST", "    ")
                        writer.WriteElementString("ADVANCETAXDETAILS.LIST", "    ")
                        writer.WriteEndElement() 'END LEDGERENTRIES.LIST
                        '/*WRITE LOOP*
                    Next
                    strsql = " SELECT TAXAMOUNT AS AMOUNT,TAXID FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & Format(gridView.Rows(i).Cells("BILLDATE").Value, "yyyy-MM-dd") & "' "
                    strsql += vbCrLf + " AND BATCHNO = '" & (gridView.Rows(i).Cells("BATCHNO").Value.ToString) & "' "
                    strsql += vbCrLf + "AND ISNULL(TRANTYPE,'') = 'SA' AND ISNULL(STUDDED,'') = 'N'"
                    da = New OleDbDataAdapter(strsql, cn)
                    dtPaymode = New DataTable
                    da.Fill(dtPaymode)
                    For J As Integer = 0 To dtPaymode.Rows.Count - 1
                        '/*WRITE LOOP*
                        writer.WriteStartElement("LEDGERENTRIES.LIST") 'START LEDGERENTRIES.LIST
                        writer.WriteStartElement("OLDAUDITENTRYIDS.LIST") 'START OLDAUDITENTRYIDS.LIST
                        writer.WriteAttributeString("TYPE", "TYPE", Nothing, "Number")
                        writer.WriteElementString("OLDAUDITENTRYIDS", "-1")
                        writer.WriteEndElement() 'END OLDAUDITENTRYIDS.LIST
                        writer.WriteElementString("ROUNDTYPE", "Normal Rounding")
                        If dtPaymode.Rows(J).Item("TAXID").ToString = "SG" Then
                            writer.WriteElementString("LEDGERNAME", "SGST")
                        Else
                            writer.WriteElementString("LEDGERNAME", "CGST")
                        End If
                        writer.WriteElementString("GSTCLASS", "")
                        writer.WriteElementString("ISDEEMEDPOSITIVE", "No")
                        writer.WriteElementString("LEDGERFROMITEM", "No")
                        writer.WriteElementString("REMOVEZEROENTRIES", "No")
                        writer.WriteElementString("ISPARTYLEDGER", "Yes")
                        writer.WriteElementString("ISLASTDEEMEDPOSITIVE", "Yes")
                        writer.WriteElementString("ISCAPVATTAXALTERED", "No")
                        writer.WriteElementString("ISCAPVATNOTCLAIMED", "No")
                        If dtPaymode.Rows(J).Item("TAXID").ToString = "SG" Then
                            writer.WriteElementString("AMOUNT", dtPaymode.Rows(J).Item("amount").ToString)
                        Else
                            writer.WriteElementString("AMOUNT", dtPaymode.Rows(J).Item("amount").ToString)
                        End If
                        writer.WriteElementString("VATEXPAMOUNT", dtPaymode.Rows(J).Item("amount").ToString)
                        writer.WriteElementString("SERVICETAXDETAILS.LIST", "  ")
                        writer.WriteElementString("BANKALLOCATIONS.LIST", "    ")
                        writer.WriteElementString("BILLALLOCATIONS.LIST", "     ")
                        writer.WriteElementString("INTERESTCOLLECTION.LIST", "     ")
                        writer.WriteElementString("OLDAUDITENTRIES.LIST", "    ")
                        writer.WriteElementString("ACCOUNTAUDITENTRIES.LIST", "     ")
                        writer.WriteElementString("AUDITENTRIES.LIST", "     ")
                        writer.WriteElementString("INPUTCRALLOCS.LIST", "     ")
                        writer.WriteElementString("DUTYHEADDETAILS.LIST", "     ")
                        writer.WriteElementString("EXCISEDUTYHEADDETAILS.LIST", "     ")
                        writer.WriteElementString("RATEDETAILS.LIST", "     ")
                        writer.WriteElementString("SUMMARYALLOCS.LIST", "    ")
                        writer.WriteElementString("STPYMTDETAILS.LIST", "    ")
                        writer.WriteElementString("EXCISEPAYMENTALLOCATIONS.LIST", "    ")
                        writer.WriteElementString("TAXBILLALLOCATIONS.LIST", "     ")
                        writer.WriteElementString("TAXOBJECTALLOCATIONS.LIST", "     ")
                        writer.WriteElementString("TDSEXPENSEALLOCATIONS.LIST", "     ")
                        writer.WriteElementString("VATSTATUTORYDETAILS.LIST", "     ")
                        writer.WriteElementString("COSTTRACKALLOCATIONS.LIST", "     ")
                        writer.WriteElementString("REFVOUCHERDETAILS.LIST", "    ")
                        writer.WriteElementString("INVOICEWISEDETAILS.LIST", "     ")
                        writer.WriteElementString("VATITCDETAILS.LIST", "    ")
                        writer.WriteElementString("ADVANCETAXDETAILS.LIST", "    ")
                        writer.WriteEndElement() 'END LEDGERENTRIES.LIST
                        '/*WRITE LOOP*
                    Next
                    '/**ROUNDED OFF TYPE******/
                    writer.WriteStartElement("LEDGERENTRIES.LIST") 'START LEDGERENTRIES.LIST
                    writer.WriteStartElement("OLDAUDITENTRYIDS.LIST") 'START OLDAUDITENTRYIDS.LIST
                    writer.WriteAttributeString("TYPE", "TYPE", Nothing, "Number")
                    writer.WriteElementString("OLDAUDITENTRYIDS", "-1")
                    writer.WriteEndElement() 'END OLDAUDITENTRYIDS.LIST
                    writer.WriteElementString("ROUNDTYPE", "Normal Rounding")
                    writer.WriteElementString("LEDGERNAME", "Round Off(-/+)")
                    writer.WriteElementString("GSTCLASS", "")
                    writer.WriteElementString("ISDEEMEDPOSITIVE", "No")
                    writer.WriteElementString("LEDGERFROMITEM", "No")
                    writer.WriteElementString("REMOVEZEROENTRIES", "No")
                    writer.WriteElementString("ISPARTYLEDGER", "No")
                    writer.WriteElementString("ISLASTDEEMEDPOSITIVE", "No")
                    writer.WriteElementString("ISCAPVATTAXALTERED", "No")
                    writer.WriteElementString("ISCAPVATNOTCLAIMED", "No")
                    writer.WriteElementString("ROUNDLIMIT", 1)
                    writer.WriteElementString("SERVICETAXDETAILS.LIST", "  ")
                    writer.WriteElementString("BANKALLOCATIONS.LIST", "    ")
                    writer.WriteElementString("BILLALLOCATIONS.LIST", "     ")
                    writer.WriteElementString("INTERESTCOLLECTION.LIST", "     ")
                    writer.WriteElementString("OLDAUDITENTRIES.LIST", "    ")
                    writer.WriteElementString("ACCOUNTAUDITENTRIES.LIST", "     ")
                    writer.WriteElementString("AUDITENTRIES.LIST", "     ")
                    writer.WriteElementString("INPUTCRALLOCS.LIST", "     ")
                    writer.WriteElementString("DUTYHEADDETAILS.LIST", "     ")
                    writer.WriteElementString("EXCISEDUTYHEADDETAILS.LIST", "     ")
                    writer.WriteElementString("RATEDETAILS.LIST", "     ")
                    writer.WriteElementString("SUMMARYALLOCS.LIST", "    ")
                    writer.WriteElementString("STPYMTDETAILS.LIST", "    ")
                    writer.WriteElementString("EXCISEPAYMENTALLOCATIONS.LIST", "    ")
                    writer.WriteElementString("TAXBILLALLOCATIONS.LIST", "     ")
                    writer.WriteElementString("TAXOBJECTALLOCATIONS.LIST", "     ")
                    writer.WriteElementString("TDSEXPENSEALLOCATIONS.LIST", "     ")
                    writer.WriteElementString("VATSTATUTORYDETAILS.LIST", "     ")
                    writer.WriteElementString("COSTTRACKALLOCATIONS.LIST", "     ")
                    writer.WriteElementString("REFVOUCHERDETAILS.LIST", "    ")
                    writer.WriteElementString("INVOICEWISEDETAILS.LIST", "     ")
                    writer.WriteElementString("VATITCDETAILS.LIST", "    ")
                    writer.WriteElementString("ADVANCETAXDETAILS.LIST", "    ")
                    writer.WriteEndElement() 'END LEDGERENTRIES.LIST
                    '/********/
                    Dim dtBeforeGst As New DataTable
                    dtBeforeGst = gridView.DataSource
                    writer.WriteStartElement("ALLINVENTORYENTRIES.LIST") 'START ALLINVENTORYENTRIES.LIST
                    writer.WriteElementString("STOCKITEMNAME", "New Gold Ornaments")
                    writer.WriteElementString("ISDEEMEDPOSITIVE", "No")
                    writer.WriteElementString("ISLASTDEEMEDPOSITIVE", "No")
                    writer.WriteElementString("ISAUTONEGATE", "No")
                    writer.WriteElementString("ISCUSTOMSCLEARANCE", "No")
                    writer.WriteElementString("ISTRACKCOMPONENT", "No")
                    writer.WriteElementString("ISTRACKPRODUCTION", "No")
                    writer.WriteElementString("ISPRIMARYITEM", "No")
                    writer.WriteElementString("ISSCRAP", "No")
                    writer.WriteElementString("RATE", "" & gridView.Rows(i).Cells("RATE").Value.ToString & "/Gms") 'dtBeforeGst.Rows(0).Item("RATE").ToString
                    writer.WriteElementString("AMOUNT", dtBeforeGst.Compute("SUM(AMOUNT)", "BATCHNO = '" & gridView.Rows(i).Cells("BATCHNO").Value.ToString & "'"))
                    writer.WriteElementString("ACTUALQTY", "" & dtBeforeGst.Compute("SUM(NETWT)", "BATCHNO = '" & gridView.Rows(i).Cells("BATCHNO").Value.ToString & "'") & " Gms")
                    writer.WriteElementString("BILLEDQTY", "" & dtBeforeGst.Compute("SUM(NETWT)", "BATCHNO = '" & gridView.Rows(i).Cells("BATCHNO").Value.ToString & "'") & " Gms")
                    writer.WriteStartElement("BATCHALLOCATIONS.LIST") 'START BATCHALLOCATIONS.LIST
                    If cnCostId = "BR" Then
                        writer.WriteElementString("GODOWNNAME", "Branch (Bundar Road)")
                    ElseIf cnCostId = "GV" Then
                        writer.WriteElementString("GODOWNNAME", "Governorpet (HO)")
                    Else
                        writer.WriteElementString("GODOWNNAME", "Governorpet (" & cnCostId & ")")
                    End If
                    writer.WriteElementString("BATCHNAME", "Primary Batch")
                    If cnCostId = "BR" Then
                        writer.WriteElementString("DESTINATIONGODOWNNAME", "Branch (Bundar Road)")
                    ElseIf cnCostId = "GV" Then
                        writer.WriteElementString("DESTINATIONGODOWNNAME", "Governorpet (HO)")
                    Else
                        writer.WriteElementString("DESTINATIONGODOWNNAME", "Governorpet (" & cnCostId & ")")
                    End If
                    writer.WriteElementString("INDENTNO", "")
                    writer.WriteElementString("ORDERNO", "")
                    writer.WriteElementString("TRACKINGNUMBER", "")
                    writer.WriteElementString("DYNAMICCSTISCLEARED", "No")

                    writer.WriteElementString("AMOUNT", dtBeforeGst.Compute("SUM(AMOUNT)", "BATCHNO = '" & gridView.Rows(i).Cells("BATCHNO").Value.ToString & "'"))
                    writer.WriteElementString("ACTUALQTY", "" & dtBeforeGst.Compute("SUM(NETWT)", "BATCHNO = '" & gridView.Rows(i).Cells("BATCHNO").Value.ToString & "'") & " Gms")
                    writer.WriteElementString("BILLEDQTY", "" & dtBeforeGst.Compute("SUM(NETWT)", "BATCHNO = '" & gridView.Rows(i).Cells("BATCHNO").Value.ToString & "'") & " Gms")

                    writer.WriteElementString("ADDITIONALDETAILS.LIST", "")
                    writer.WriteElementString("VOUCHERCOMPONENTLIST.LIST", "")
                    writer.WriteEndElement() 'END BATCHALLOCATIONS.LIST


                    writer.WriteStartElement("ACCOUNTINGALLOCATIONS.LIST") 'START ACCOUNTINGALLOCATIONS.LIST
                    writer.WriteStartElement("OLDAUDITENTRYIDS.LIST") 'START OLDAUDITENTRYIDS.LIST
                    writer.WriteAttributeString("TYPE", "TYPE", Nothing, "Number")
                    writer.WriteElementString("OLDAUDITENTRYIDS", "-1")
                    writer.WriteEndElement() 'END OLDAUDITENTRYIDS.LIST
                    'same
                    If cnCostId = "BR" Then
                        writer.WriteElementString("LEDGERNAME", "GST SALES BR")
                    ElseIf cnCostId = "GV" Then
                        writer.WriteElementString("LEDGERNAME", "GST SALES HO")
                    Else
                        writer.WriteElementString("LEDGERNAME", "GST SALES " & cnCostId)
                    End If
                    writer.WriteElementString("GSTCLASS", "")
                    writer.WriteElementString("ISDEEMEDPOSITIVE", "Yes")
                    writer.WriteElementString("LEDGERFROMITEM", "No")
                    writer.WriteElementString("REMOVEZEROENTRIES", "No")
                    writer.WriteElementString("ISPARTYLEDGER", "Yes")
                    writer.WriteElementString("ISLASTDEEMEDPOSITIVE", "Yes")
                    writer.WriteElementString("ISCAPVATTAXALTERED", "No")
                    writer.WriteElementString("AMOUNT", "" & dtBeforeGst.Compute("SUM(AMOUNT)", "BATCHNO = '" & gridView.Rows(i).Cells("BATCHNO").Value.ToString & "'"))
                    writer.WriteElementString("SERVICETAXDETAILS.LIST", "    ")
                    writer.WriteElementString("BANKALLOCATIONS.LIST", "    ")
                    writer.WriteElementString("BILLALLOCATIONS.LIST", "     ")
                    writer.WriteElementString("INTERESTCOLLECTION.LIST", "     ")
                    writer.WriteElementString("OLDAUDITENTRIES.LIST", "    ")
                    writer.WriteElementString("ACCOUNTAUDITENTRIES.LIST", "     ")
                    writer.WriteElementString("AUDITENTRIES.LIST", "     ")
                    writer.WriteElementString("INPUTCRALLOCS.LIST", "     ")
                    writer.WriteElementString("DUTYHEADDETAILS.LIST", "     ")
                    writer.WriteElementString("EXCISEDUTYHEADDETAILS.LIST", "     ")
                    writer.WriteElementString("RATEDETAILS.LIST", "     ")
                    writer.WriteElementString("SUMMARYALLOCS.LIST", "    ")
                    writer.WriteElementString("STPYMTDETAILS.LIST", "    ")
                    writer.WriteElementString("EXCISEPAYMENTALLOCATIONS.LIST", "    ")
                    writer.WriteElementString("TAXBILLALLOCATIONS.LIST", "     ")
                    writer.WriteElementString("TAXOBJECTALLOCATIONS.LIST", "     ")
                    writer.WriteElementString("TDSEXPENSEALLOCATIONS.LIST", "     ")
                    writer.WriteElementString("VATSTATUTORYDETAILS.LIST", "     ")
                    writer.WriteElementString("COSTTRACKALLOCATIONS.LIST", "     ")
                    writer.WriteElementString("REFVOUCHERDETAILS.LIST", "    ")
                    writer.WriteElementString("INVOICEWISEDETAILS.LIST", "     ")
                    writer.WriteElementString("VATITCDETAILS.LIST", "    ")
                    writer.WriteElementString("ADVANCETAXDETAILS.LIST", "    ")
                    writer.WriteElementString("DUTYHEADDETAILS.LIST", "   ")
                    writer.WriteElementString("SUPPLEMENTARYDUTYHEADDETAILS.LIST", "    ")
                    writer.WriteElementString("REFVOUCHERDETAILS.LIST", "    ")
                    writer.WriteElementString("EXCISEALLOCATIONS.LIST", "    ")
                    writer.WriteElementString("EXPENSEALLOCATIONS.LIST", "    ")
                    writer.WriteEndElement() 'END ACCOUNTINGALLOCATIONS.LIST
                    writer.WriteEndElement() 'END ALLINVENTORYENTRIES.LIST
                    'same
                    writer.WriteElementString("PAYROLLMODEOFPAYMENT.LIST", "    ")
                    writer.WriteElementString("ATTDRECORDS.LIST", "    ")
                    writer.WriteElementString("GSTEWAYCONSIGNORADDRESS.LIST", "    ")
                    writer.WriteElementString("GSTEWAYCONSIGNEEADDRESS.LIST", "    ")
                    writer.WriteElementString("TEMPGSTRATEDETAILS.LIST", "    ")
                    writer.WriteEndElement() 'END VOUCHER
                    writer.WriteEndElement() 'END TALLYMESSAGE
                Next

                writer.WriteStartElement("TALLYMESSAGE") 'START TALLYMESSATE
                writer.WriteAttributeString("xmlns", "UDF", Nothing, "TallyUDF")
                writer.WriteStartElement("COMPANY") 'START COMPANY
                writer.WriteStartElement("REMOTECMPINFO.LIST") 'START REMOTECMPINFO.LIST
                writer.WriteAttributeString("MERGE", "MERGE", Nothing, "Yes")
                writer.WriteElementString("NAME", "81a991b8-2203-4ebf-b6bd-ee76150085dd")
                writer.WriteElementString("REMOTECMPNAME", "" & strCompanyName & cnTranFromDate.Year & "-" & cnTranToDate.Year)
                writer.WriteElementString("REMOTECMPSTATE", gridView.Rows(0).Cells("STATE").Value.ToString)
                writer.WriteEndElement() 'END REMOTECMPINFO.LIST
                writer.WriteEndElement() 'END COMPANY
                writer.WriteEndElement() 'END TALLYMESSAGE

                writer.WriteEndElement() 'END REQUESTDATA
                writer.WriteEndElement() 'END IMPORTDATA
                writer.WriteEndElement() 'END BODY

                writer.WriteEndElement() 'END ENVELOPE
                writer.WriteEndDocument() 'END ROOT
                MsgBox("Completed")
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
            Exit Sub
        End Try
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        gridView.DataSource = Nothing
        dtpAsOnDate.Value = GetServerDate()
        dtpTo.Value = GetServerDate()

        strsql = " SELECT 'ALL'COSTNAME, 'ALL' COSTID,1 RESULT UNION ALL "
        strsql += vbCrLf + " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strsql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkcmbcostcentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))

        strsql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strsql += " UNION ALL"
        strsql += " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        strsql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtCompany)
        If dtCompany.Rows.Count > 0 Then
            cmbCompany.Items.Clear()
            For i As Integer = 0 To dtCompany.Rows.Count - 1
                cmbCompany.Items.Add(dtCompany.Rows(i).Item("COMPANYNAME").ToString)
            Next
            cmbCompany.Text = strCompanyName
        End If
    End Sub
    Private Sub btnView_Click(sender As Object, e As EventArgs) Handles btnView.Click
        Dim chkCostId As String = ""
        If chkcmbcostcentre.Text <> "ALL" And chkcmbcostcentre.Text <> "" Then
            chkCostId = GetSelectedCostId(chkcmbcostcentre, True)
        End If
        gridView.DataSource = Nothing
        strsql = " SELECT I.TRANNO"
        strsql += vbCrLf + " ,I.TRANDATE BILLDATE"
        strsql += vbCrLf + " ,SUM(I.NETWT) NETWT"
        strsql += vbCrLf + " ,CONVERT(NUMERIC(15,2),ROUND(ROUND((SUM(AMOUNT+TAX))/1.03,3) / (Case When SUM(NETWT) = 0 Then 1 Else SUM(NETWT) End),2))  RATE"
        strsql += vbCrLf + " ,SUM(AMOUNT) AMOUNT"
        strsql += vbCrLf + " ,SUM(TAX) TAX"
        strsql += vbCrLf + " ,BOARDRATE As BOARDRATE"
        strsql += vbCrLf + " ,SUM(AMOUNT+TAX) TOTALAMOUNT"
        strsql += vbCrLf + " ,PE.PNAME,PE.AREA,PE.STATE"
        strsql += vbCrLf + " ,ISNULL(PE.GSTNO,'') GSTNO"
        strsql += vbCrLf + " ,ISNULL(I.COSTID,'') COSTID "
        strsql += vbCrLf + " ,I.COMPANYID"
        strsql += vbCrLf + " ,I.BATCHNO "
        strsql += vbCrLf + " ,REPLACE(CAST(I.TRANDATE  AS DATE),'-','') TRANDATE"
        strsql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I, " & cnAdminDb & "..CUSTOMERINFO AS CU"
        strsql += vbCrLf + " ," & cnAdminDb & "..PERSONALINFO AS PE"
        strsql += vbCrLf + " WHERE I.BATCHNO = CU.BATCHNO AND CU.PSNO= PE.SNO "
        strsql += vbCrLf + " AND I.TRANDATE BETWEEN '" & Format(dtpAsOnDate.Value.Date, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "' "
        strsql += vbCrLf + " AND ISNULL(I.CANCEL,'') = '' AND I.TRANTYPE IN('SA')  "
        If txtBillNo.Text.Trim <> "" Then
            strsql += vbCrLf + " AND I.TRANNO IN(" & txtBillNo.Text.Trim & ")"
        End If
        If chkCostId <> "" Then
            strsql += vbCrLf + " AND I.COSTID IN (" & chkCostId & ")"
        End If
        If cmbCompany.Text <> "ALL" Then
            strsql += vbCrLf + " AND  I.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN ('" & cmbCompany.Text & "') )"
        End If
        strsql += vbCrLf + " GROUP BY I.TRANNO,I.TRANDATE,I.BOARDRATE,I.BATCHNO,PE.PNAME,PE.AREA,PE.STATE,ISNULL(PE.GSTNO,''),ISNULL(I.COSTID,''),I.COMPANYID"
        dt = New DataTable
        dt.Columns.Add("CHK", GetType(Boolean))
        dt.Columns("CHK").DefaultValue = True
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            With gridView
                .DataSource = Nothing
                .DataSource = dt
                FormatGridColumns(gridView, False, True, True, True)
                .Columns("CHK").ReadOnly = False
                '.Columns("BATCHNO").Visible = False
                .Columns("TRANDATE").Visible = False
            End With
        Else
            MsgBox("No Record found", MsgBoxStyle.Information)
            gridView.DataSource = Nothing
            dtpAsOnDate.Focus()
            Exit Sub
        End If
    End Sub
    Private Sub chkSelect_CheckedChanged(sender As Object, e As EventArgs) Handles chkSelect.CheckedChanged
        If gridView.Rows.Count > 0 Then
            If chkSelect.Checked = True Then
                For i As Integer = 0 To gridView.Rows.Count - 1
                    gridView.Rows(i).Cells("CHK").Value = CheckState.Checked
                Next
            Else
                For i As Integer = 0 To gridView.Rows.Count - 1
                    gridView.Rows(i).Cells("CHK").Value = CheckState.Unchecked
                Next
            End If
        End If
    End Sub

    Private Sub btnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            Dim title As String = ""
            title = "TALLY EXPORT REPORT " & dtpAsOnDate.Text & " / " & dtpTo.Text
            BrightPosting.GExport.Post(Me.Name, strCompanyName, title, gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            Dim title As String = ""
            title = "TALLY EXPORT REPORT " & dtpAsOnDate.Text & " / " & dtpTo.Text
            BrightPosting.GExport.Post(Me.Name, strCompanyName, title, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

#End Region
End Class