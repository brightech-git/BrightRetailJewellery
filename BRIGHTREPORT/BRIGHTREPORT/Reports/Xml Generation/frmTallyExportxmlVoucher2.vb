Imports System.Xml
Imports System.Data.OleDb
Public Class frmTallyExportxmlVoucher2
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


    Private Sub CURRENCY(ByVal writer As XmlWriter)
        'FIRST STEP 1
        writer.WriteStartElement("CURRENCY") 'START CURRENCY
        writer.WriteAttributeString("NAME", "NAME", Nothing, "₹")
        writer.WriteAttributeString("RESERVEDNAME", "RESERVEDNAME", Nothing, "")
        writer.WriteElementString("GUID", "0073cba0-58fe-4056-9e37-4c8fee3ec61d-0000001d") 'START GUID
        writer.WriteElementString("MAILINGNAME", "INR") 'START MAILINGNAME
        writer.WriteElementString("ORIGINALNAME", "₹") 'START ORIGINALNAME
        writer.WriteElementString("EXPANDEDSYMBOL", "INR") 'START EXPANDEDSYMBOL
        writer.WriteElementString("DECIMALSYMBOL", "paise") 'START DECIMALSYMBOL
        writer.WriteElementString("ISUPDATINGTARGETID", "No") 'START ISUPDATINGTARGETID
        writer.WriteElementString("ASORIGINAL", "No") 'START ASORIGINAL
        writer.WriteElementString("ISSUFFIX", "No") 'START ISSUFFIX
        writer.WriteElementString("HASSPACE", "Yes") 'START HASSPACE
        writer.WriteElementString("INMILLIONS", "No") 'START INMILLIONS
        writer.WriteElementString("ALTERID", "825") 'START ALTERID
        writer.WriteElementString("DECIMALPLACES", " 2") 'START DECIMALPLACES
        writer.WriteElementString("DECIMALPLACESFORPRINTING", " 2") 'START DECIMALPLACESFORPRINTING
        writer.WriteElementString("DAILYSTDRATES.LIST", " ") 'START DAILYSTDRATES
        writer.WriteElementString("DAILYBUYINGRATES.LIST", " ") 'START DAILYBUYINGRATES
        writer.WriteElementString("DAILYSELLINGRATES.LIST", " ") 'START DAILYSELLINGRATES
        writer.WriteEndElement() 'END CURRENCY
    End Sub

    Private Sub GROUP(ByVal writer As XmlWriter, ByVal name As String, ByVal name2 As String)
        'FIRST STEP 1
        writer.WriteStartElement("GROUP") 'START GROUP
        writer.WriteAttributeString("NAME", "NAME", Nothing, name) '
        writer.WriteAttributeString("RESERVEDNAME", "RESERVEDNAME", Nothing, name)
        writer.WriteElementString("GUID", "0073cba0-58fe-4056-9e37-4c8fee3ec61d-0000001b") 'START GUID
        writer.WriteElementString("PARENT", "") '
        writer.WriteElementString("GRPDEBITPARENT", "") '
        writer.WriteElementString("GRPCREDITPARENT", "") '
        writer.WriteElementString("ISBILLWISEON", "No") '
        writer.WriteElementString("ISCOSTCENTRESON", "No") '
        writer.WriteElementString("ISADDABLE", "No") '
        writer.WriteElementString("ISUPDATINGTARGETID", "No") '
        writer.WriteElementString("ASORIGINAL", "No") '
        writer.WriteElementString("ISSUBLEDGER", "No") '
        writer.WriteElementString("ISREVENUE", "Yes") '
        writer.WriteElementString("AFFECTSGROSSPROFIT", "No") '
        writer.WriteElementString("ISDEEMEDPOSITIVE", "No") '
        writer.WriteElementString("TRACKNEGATIVEBALANCES", "No") '
        writer.WriteElementString("ISCONDENSED", "No") '
        writer.WriteElementString("AFFECTSSTOCK", "No") '
        writer.WriteElementString("ISGROUPFORLOANRCPT", "No") '
        writer.WriteElementString("ISGROUPFORLOANPYMNT", "No") '
        writer.WriteElementString("ISRATEINCLUSIVEVAT", "No") '
        writer.WriteElementString("ISINVDETAILSENABLE", "No") '
        writer.WriteElementString("SORTPOSITION", "270") '
        writer.WriteElementString("SERVICETAXDETAILS.LIST", "") '
        writer.WriteElementString("VATDETAILS.LIST", "") '
        writer.WriteElementString("SALESTAXCESSDETAILS.LIST", "") '
        writer.WriteElementString("GSTDETAILS.LIST", "") '
        '''''
        writer.WriteStartElement("LANGUAGENAME.LIST") 'START LANGUAGENAME
        writer.WriteStartElement("NAME.LIST") 'START NAME.LIST
        writer.WriteAttributeString("NAME", "NAME", Nothing, "String")
        writer.WriteElementString("NAME", name) '
        If name2 <> "" Then
            writer.WriteElementString("NAME", name2) ' 'name2->Income (Indirect)
        End If
        writer.WriteEndElement() 'END NAME.LIST
        writer.WriteElementString("LANGUAGEID", "1033") '
        writer.WriteEndElement() 'END LANGUAGENAME
        '''''
        writer.WriteElementString("XBRLDETAIL.LIST", " ") '
        writer.WriteElementString("AUDITDETAILS.LIST", " ") '
        writer.WriteElementString("SCHVIDETAILS.LIST", " ") '
        writer.WriteElementString("EXCISETARIFFDETAILS.LIST", " ") '
        writer.WriteElementString("TCSCATEGORYDETAILS.LIST", " ") '
        writer.WriteElementString("TDSCATEGORYDETAILS.LIST", " ") '
        writer.WriteElementString("GSTCLASSFNIGSTRATES.LIST", " ") '
        writer.WriteElementString("EXTARIFFDUTYHEADDETAILS.LIST", " ") '
        writer.WriteEndElement() 'END GROUP
    End Sub


    Private Sub LEDGER(ByVal writer As XmlWriter, ByVal name As String, ByVal type As String _
                       , ByVal GSTTYPEOFSUPPLY As String, ByVal ISCOSTCENTRESON As String _
                       , ByVal AFFECTSSTOCK As String, ByVal gstper As Double _
                       , ByVal RESERVEDNAME As String, ByVal type2 As String, ByVal PARENT As String)
        'FIRST STEP 1
        writer.WriteStartElement("LEDGER") 'START LEDGER
        writer.WriteAttributeString("NAME", "NAME", Nothing, name)
        writer.WriteAttributeString("RESERVEDNAME", "RESERVEDNAME", Nothing, RESERVEDNAME)

        writer.WriteStartElement("MAILINGNAME.LIST") 'START MAILINGNAME
        writer.WriteAttributeString("TYPE", "TYPE", Nothing, type) '
        writer.WriteElementString("MAILINGNAME", name) '
        writer.WriteEndElement() 'END MAILINGNAME

        writer.WriteStartElement("OLDAUDITENTRYIDS.LIST") 'START OLDAUDITENTRYIDS
        writer.WriteAttributeString("TYPE", "TYPE", Nothing, type2)
        writer.WriteElementString("OLDAUDITENTRYIDS", "-1")
        writer.WriteEndElement() 'END OLDAUDITENTRYIDS

        writer.WriteElementString("STARTINGFROM", gridView.Rows(0).Cells("TRANDATE").Value.ToString)
        writer.WriteElementString("GUID", "0073cba0-58fe-4056-9e37-4c8fee3ec61d-000000ac")
        writer.WriteElementString("CURRENCYNAME", "₹")
        writer.WriteElementString("PARENT", PARENT)
        writer.WriteElementString("TAXCLASSIFICATIONNAME", "")
        writer.WriteElementString("TAXTYPE", "Others")
        writer.WriteElementString("GSTTYPE", "")
        writer.WriteElementString("APPROPRIATEFOR", "")
        writer.WriteElementString("GSTDUTYHEAD", "State Tax")
        writer.WriteElementString("GSTTYPEOFSUPPLY", GSTTYPEOFSUPPLY)
        writer.WriteElementString("EXCISELEDGERCLASSIFICATION", "")
        writer.WriteElementString("EXCISEDUTYTYPE", "")
        writer.WriteElementString("EXCISENATUREOFPURCHASE", "")
        writer.WriteElementString("LEDGERFBTCATEGORY", "")
        writer.WriteElementString("ISBILLWISEON", "No")
        writer.WriteElementString("ISCOSTCENTRESON", ISCOSTCENTRESON)

        writer.WriteElementString("ISINTERESTON", "No")
        writer.WriteElementString("ALLOWINMOBILE", "No")
        writer.WriteElementString("ISCOSTTRACKINGON", "No")
        writer.WriteElementString("ISBENEFICIARYCODEON", "No")
        writer.WriteElementString("ISUPDATINGTARGETID", "No")
        writer.WriteElementString("ASORIGINAL", "No")
        writer.WriteElementString("ISCONDENSED", "No")
        writer.WriteElementString("AFFECTSSTOCK", AFFECTSSTOCK)
        writer.WriteElementString("ISRATEINCLUSIVEVAT", "No")
        writer.WriteElementString("FORPAYROLL", "No")
        writer.WriteElementString("ISABCENABLED", "No")
        writer.WriteElementString("ISCREDITDAYSCHKON", "No")

        writer.WriteElementString("INTERESTONBILLWISE", "No")
        writer.WriteElementString("OVERRIDEINTEREST", "No")
        writer.WriteElementString("OVERRIDEADVINTEREST", "No")
        writer.WriteElementString("USEFORVAT", "No")
        writer.WriteElementString("IGNORETDSEXEMPT", "No")
        writer.WriteElementString("ISTCSAPPLICABLE", "No")
        writer.WriteElementString("ISTDSAPPLICABLE", "No")
        writer.WriteElementString("ISFBTAPPLICABLE", "No")
        writer.WriteElementString("ISGSTAPPLICABLE", "No")
        writer.WriteElementString("ISEXCISEAPPLICABLE", "No")
        writer.WriteElementString("ISTDSEXPENSE", "No")
        writer.WriteElementString("ISEDLIAPPLICABLE", "No")
        writer.WriteElementString("ISRELATEDPARTY", "No")
        writer.WriteElementString("USEFORESIELIGIBILITY", "No")
        writer.WriteElementString("ISINTERESTINCLLASTDAY", "No")
        writer.WriteElementString("APPROPRIATETAXVALUE", "No")
        writer.WriteElementString("ISBEHAVEASDUTY", "No")
        writer.WriteElementString("INTERESTINCLDAYOFADDITION", "No")
        writer.WriteElementString("INTERESTINCLDAYOFDEDUCTION", "No")
        writer.WriteElementString("ISOTHTERRITORYASSESSEE", "No")
        writer.WriteElementString("OVERRIDECREDITLIMIT", "No")
        writer.WriteElementString("ISAGAINSTFORMC", "No")
        writer.WriteElementString("ISCHEQUEPRINTINGENABLED", "Yes")
        writer.WriteElementString("ISPAYUPLOAD", "No")
        writer.WriteElementString("ISPAYBATCHONLYSAL", "No")
        writer.WriteElementString("ISBNFCODESUPPORTED", "No")
        writer.WriteElementString("ALLOWEXPORTWITHERRORS", "No")
        writer.WriteElementString("CONSIDERPURCHASEFOREXPORT", "No")
        writer.WriteElementString("ISTRANSPORTER", "No")
        writer.WriteElementString("USEFORNOTIONALITC", "No")
        writer.WriteElementString("ISECOMMOPERATOR", "No")
        writer.WriteElementString("SHOWINPAYSLIP", "No")
        writer.WriteElementString("USEFORGRATUITY", "No")
        writer.WriteElementString("ISTDSPROJECTED", "No")
        writer.WriteElementString("FORSERVICETAX", "No")
        writer.WriteElementString("ISINPUTCREDIT", "No")
        writer.WriteElementString("ISEXEMPTED", "No")
        writer.WriteElementString("ISABATEMENTAPPLICABLE", "No")
        writer.WriteElementString("ISSTXPARTY", "No")
        writer.WriteElementString("ISSTXNONREALIZEDTYPE", "No")
        writer.WriteElementString("ISUSEDFORCVD", "No")
        writer.WriteElementString("LEDBELONGSTONONTAXABLE", "No")
        writer.WriteElementString("ISEXCISEMERCHANTEXPORTER", "No")
        writer.WriteElementString("ISPARTYEXEMPTED", "No")
        writer.WriteElementString("ISSEZPARTY", "No")
        writer.WriteElementString("TDSDEDUCTEEISSPECIALRATE", "No")
        writer.WriteElementString("ISECHEQUESUPPORTED", "No")
        writer.WriteElementString("ISEDDSUPPORTED", "No")
        writer.WriteElementString("HASECHEQUEDELIVERYMODE", "No")
        writer.WriteElementString("HASECHEQUEDELIVERYTO", "No")
        writer.WriteElementString("HASECHEQUEPRINTLOCATION", "No")
        writer.WriteElementString("HASECHEQUEPAYABLELOCATION", "No")
        writer.WriteElementString("HASECHEQUEBANKLOCATION", "No")
        writer.WriteElementString("HASEDDDELIVERYMODE", "No")
        writer.WriteElementString("HASEDDDELIVERYTO", "No")
        writer.WriteElementString("HASEDDPRINTLOCATION", "No")
        writer.WriteElementString("HASEDDPAYABLELOCATION", "No")
        writer.WriteElementString("HASEDDBANKLOCATION", "No")
        writer.WriteElementString("ISEBANKINGENABLED", "No")
        writer.WriteElementString("ISEXPORTFILEENCRYPTED", "No")
        writer.WriteElementString("ISBATCHENABLED", "No")
        writer.WriteElementString("ISPRODUCTCODEBASED", "No")
        writer.WriteElementString("HASEDDCITY", "No")
        writer.WriteElementString("HASECHEQUECITY", "No")
        writer.WriteElementString("ISFILENAMEFORMATSUPPORTED", "No")
        writer.WriteElementString("HASCLIENTCODE", "No")
        writer.WriteElementString("PAYINSISBATCHAPPLICABLE", "No")
        writer.WriteElementString("PAYINSISFILENUMAPP", "No")
        writer.WriteElementString("ISSALARYTRANSGROUPEDFORBRS", "No")
        writer.WriteElementString("ISEBANKINGSUPPORTED", "No")
        writer.WriteElementString("ISSCBUAE", "No")
        writer.WriteElementString("ISBANKSTATUSAPP", "No")
        writer.WriteElementString("ISSALARYGROUPED", "No")
        writer.WriteElementString("USEFORPURCHASETAX", "No")
        writer.WriteElementString("AUDITED", "No")
        writer.WriteElementString("SORTPOSITION", "1000")
        writer.WriteElementString("ALTERID", "799")
        writer.WriteElementString("SERVICETAXDETAILS.LIST", "")
        writer.WriteElementString("LBTREGNDETAILS.LIST", "")

        If gstper > 0 Then
            writer.WriteStartElement("VATDETAILS.LIST", "") 'START
            writer.WriteElementString("FROMDATE", gridView.Rows(0).Cells("TRANDATE").Value.ToString)
            writer.WriteElementString("FRMDATE", gridView.Rows(0).Cells("TRANDATE").Value.ToString)
            writer.WriteElementString("TAXTYPE", "Exempt")
            writer.WriteElementString("STATNATURENAME", "Sales Exempt")
            writer.WriteElementString("VATCOMMODITYNAME", name)
            writer.WriteElementString("SCHEDULE", "Fourth Schedule")
            writer.WriteElementString("VATCOMMODITYCODE", 755)
            writer.WriteElementString("ISINZRBASICSERVICES", "No")
            writer.WriteElementString("ISINVDETAILSENABLE", "No")
            writer.WriteElementString("ISCALCONACTUALQTY", "No")
            writer.WriteElementString("RATEOFVAT", "0")
            writer.WriteElementString("VATITEMSLABRATES.LIST", "")
            writer.WriteEndElement() 'END
        Else
            writer.WriteElementString("VATDETAILS.LIST", "")
        End If
        writer.WriteElementString("SALESTAXCESSDETAILS.LIST", "")
        If gstper > 0 Then
            writer.WriteStartElement("GSTDETAILS.LIST", "") 'START GSTDETAILS

            writer.WriteElementString("APPLICABLEFROM", gridView.Rows(0).Cells("TRANDATE").Value.ToString)
            writer.WriteElementString("HSNCODE", "7113")
            writer.WriteElementString("HSN", "Silver Articles")
            writer.WriteElementString("TAXABILITY", "Taxable")
            writer.WriteElementString("GSTNATUREOFTRANSACTION", "Sales Taxable")
            writer.WriteElementString("ISREVERSECHARGEAPPLICABLE", "No")
            writer.WriteElementString("ISNONGSTGOODS", "No")
            writer.WriteElementString("GSTINELIGIBLEITC", "No")
            writer.WriteElementString("INCLUDEEXPFORSLABCALC", "No")


            writer.WriteStartElement("STATEWISEDETAILS.LIST", "") ' START STATEWISEDETAILS.LIST

            writer.WriteStartElement("RATEDETAILS.LIST", "") ' START RATEDETAILS.LIST
            writer.WriteElementString("GSTRATEDUTYHEAD", "Central Tax")
            writer.WriteElementString("GSTRATEVALUATIONTYPE", "Based on Value")
            writer.WriteElementString("GSTRATE", "1.50")
            writer.WriteEndElement() 'END RATEDETAILS.LIST

            writer.WriteStartElement("RATEDETAILS.LIST", "") ' START RATEDETAILS.LIST
            writer.WriteElementString("GSTRATEDUTYHEAD", "State Tax")
            writer.WriteElementString("GSTRATEVALUATIONTYPE", "Based on Value")
            writer.WriteElementString("GSTRATE", "1.50")
            writer.WriteEndElement() 'END RATEDETAILS.LIST

            writer.WriteStartElement("RATEDETAILS.LIST", "") ' START RATEDETAILS.LIST
            writer.WriteElementString("GSTRATEDUTYHEAD", "Integrated Tax")
            writer.WriteElementString("GSTRATEVALUATIONTYPE", "Based on Value")
            writer.WriteElementString("GSTRATE", "3")
            writer.WriteEndElement() 'END RATEDETAILS.LIST

            writer.WriteStartElement("RATEDETAILS.LIST", "") ' START RATEDETAILS.LIST
            writer.WriteElementString("GSTRATEDUTYHEAD", "Cess")
            writer.WriteElementString("GSTRATEVALUATIONTYPE", "Based on Value")
            writer.WriteEndElement() 'END RATEDETAILS.LIST

            writer.WriteStartElement("RATEDETAILS.LIST", "") ' START RATEDETAILS.LIST
            writer.WriteElementString("GSTRATEDUTYHEAD", "Cess on Qty")
            writer.WriteElementString("GSTRATEVALUATIONTYPE", "Based on Quantity")
            writer.WriteEndElement() 'END RATEDETAILS.LIST

            writer.WriteElementString("GSTSLABRATES.LIST", "")

            writer.WriteEndElement() 'END STATEWISEDETAILS.LIST
            writer.WriteElementString("TEMPGSTDETAILSLABRATES.LIST", "")

            writer.WriteEndElement() 'END GSTDETAILS
        Else
            writer.WriteElementString("GSTDETAILS.LIST", "")
        End If

        writer.WriteStartElement("LANGUAGENAME.LIST") 'START LANGUAGENAME
        writer.WriteStartElement("NAME.LIST") 'START NAME.LIST
        writer.WriteAttributeString("NAME", "NAME", Nothing, "STRING")
        writer.WriteElementString("NAME", name) '
        writer.WriteEndElement() 'END NAME.LIST
        writer.WriteElementString("LANGUAGEID", "1033") '
        writer.WriteEndElement() 'END LANGUAGENAME

        writer.WriteElementString("XBRLDETAIL.LIST", "")
        writer.WriteElementString("AUDITDETAILS.LIST", "")
        writer.WriteElementString("SCHVIDETAILS.LIST", "")
        writer.WriteElementString("EXCISETARIFFDETAILS.LIST", "")

        writer.WriteElementString("TCSCATEGORYDETAILS.LIST", "")
        writer.WriteElementString("TDSCATEGORYDETAILS.LIST", "")
        writer.WriteElementString("SLABPERIOD.LIST", "")
        writer.WriteElementString("GRATUITYPERIOD.LIST", "")
        writer.WriteElementString("ADDITIONALCOMPUTATIONS.LIST", "")
        writer.WriteElementString("EXCISEJURISDICTIONDETAILS.LIST", "")
        writer.WriteElementString("EXCLUDEDTAXATIONS.LIST", "")
        writer.WriteElementString("BANKALLOCATIONS.LIST", "")
        writer.WriteElementString("PAYMENTDETAILS.LIST", "")
        writer.WriteElementString("BANKEXPORTFORMATS.LIST", "")
        writer.WriteElementString("BILLALLOCATIONS.LIST", "")
        writer.WriteElementString("INTERESTCOLLECTION.LIST", "")

        writer.WriteElementString("LEDGERCLOSINGVALUES.LIST", "")
        writer.WriteElementString("LEDGERAUDITCLASS.LIST", "")
        writer.WriteElementString("OLDAUDITENTRIES.LIST", "")
        writer.WriteElementString("TDSEXEMPTIONRULES.LIST", "")
        writer.WriteElementString("DEDUCTINSAMEVCHRULES.LIST", "")

        writer.WriteElementString("LOWERDEDUCTION.LIST", "")
        writer.WriteElementString("STXABATEMENTDETAILS.LIST", "")
        writer.WriteElementString("LEDMULTIADDRESSLIST.LIST", "")
        writer.WriteElementString("STXTAXDETAILS.LIST", "")
        writer.WriteElementString("CHEQUERANGE.LIST", "")
        writer.WriteElementString("DEFAULTVCHCHEQUEDETAILS.LIST", "")
        writer.WriteElementString("ACCOUNTAUDITENTRIES.LIST", "")
        writer.WriteElementString("AUDITENTRIES.LIST", "")
        writer.WriteElementString("BRSIMPORTEDINFO.LIST", "")
        writer.WriteElementString("AUTOBRSCONFIGS.LIST", "")
        writer.WriteElementString("BANKURENTRIES.LIST", "")
        writer.WriteElementString("DEFAULTCHEQUEDETAILS.LIST", "")

        writer.WriteElementString("DEFAULTOPENINGCHEQUEDETAILS.LIST", "")
        writer.WriteElementString("CANCELLEDPAYALLOCATIONS.LIST", "")
        writer.WriteElementString("ECHEQUEPRINTLOCATION.LIST", "")
        writer.WriteElementString("ECHEQUEPAYABLELOCATION.LIST", "")
        writer.WriteElementString("EDDPRINTLOCATION.LIST", "")
        writer.WriteElementString("EDDPAYABLELOCATION.LIST", "")
        writer.WriteElementString("AVAILABLETRANSACTIONTYPES.LIST", "")
        writer.WriteElementString("LEDPAYINSCONFIGS.LIST", "")
        writer.WriteElementString("TYPECODEDETAILS.LIST", "")
        writer.WriteElementString("FIELDVALIDATIONDETAILS.LIST", "")
        writer.WriteElementString("INPUTCRALLOCS.LIST", "")
        writer.WriteElementString("GSTCLASSFNIGSTRATES.LIST", "")
        writer.WriteElementString("EXTARIFFDUTYHEADDETAILS.LIST", "")
        writer.WriteElementString("VOUCHERTYPEPRODUCTCODES.LIST", "")

        writer.WriteEndElement() 'END LEDGER
    End Sub

    Private Sub voucherTypereserver(ByVal writer As XmlWriter)
        writer.WriteStartElement("VOUCHERTYPE") 'START VOUCHERTYPE
        writer.WriteAttributeString("NAME", "NAME", Nothing, "Sales")
        writer.WriteAttributeString("RESERVEDNAME", "RESERVEDNAME", Nothing, "Sales")
        writer.WriteElementString("GUID", "0073cba0-58fe-4056-9e37-4c8fee3ec61d-00000026") 'START GUID
        writer.WriteElementString("PARENT", "Sales") '
        writer.WriteElementString("MAILINGNAME", "Sale") '
        writer.WriteElementString("NUMBERINGMETHOD", "Automatic (Manual Override)")

        writer.WriteElementString("ISUPDATINGTARGETID", "No")
        writer.WriteElementString("ASORIGINAL", "No")
        writer.WriteElementString("ISDEEMEDPOSITIVE", "Yes")
        writer.WriteElementString("AFFECTSSTOCK", "No")
        writer.WriteElementString("PREVENTDUPLICATES", "Yes")
        writer.WriteElementString("PREFILLZERO", "No")
        writer.WriteElementString("PRINTAFTERSAVE", "No")
        writer.WriteElementString("FORMALRECEIPT", "No")
        writer.WriteElementString("ISOPTIONAL", "No")
        writer.WriteElementString("ASMFGJRNL", "No")
        writer.WriteElementString("EFFECTIVEDATE", "No")

        writer.WriteElementString("COMMONNARRATION", "Yes")

        writer.WriteElementString("MULTINARRATION", "No")
        writer.WriteElementString("ISTAXINVOICE", "No")
        writer.WriteElementString("USEFORPOSINVOICE", "No")
        writer.WriteElementString("USEFOREXCISETRADERINVOICE", "No")
        writer.WriteElementString("USEFOREXCISE", "No")
        writer.WriteElementString("USEFORJOBWORK", "No")
        writer.WriteElementString("ISFORJOBWORKIN", "No")
        writer.WriteElementString("ALLOWCONSUMPTION", "No")
        writer.WriteElementString("USEFOREXCISEGOODS", "No")
        writer.WriteElementString("USEFOREXCISESUPPLEMENTARY", "No")
        writer.WriteElementString("ISDEFAULTALLOCENABLED", "No")
        writer.WriteElementString("SORTPOSITION", "70")
        writer.WriteElementString("ALTERID", "807")
        writer.WriteElementString("BEGINNINGNUMBER", "1")

        writer.WriteStartElement("LANGUAGENAME.LIST") 'START LANGUAGENAME
        writer.WriteStartElement("NAME.LIST") 'START NAME.LIST
        writer.WriteAttributeString("NAME", "NAME", Nothing, "STRING")
        writer.WriteElementString("NAME", "Sales") '
        writer.WriteEndElement() 'END NAME.LIST
        writer.WriteElementString("LANGUAGEID", "1033") '
        writer.WriteEndElement() 'END LANGUAGENAME

        writer.WriteElementString("AUDITDETAILS.LIST", "")
        writer.WriteElementString("EXCISETARIFFDETAILS.LIST", "")
        writer.WriteElementString("PREFIXLIST.LIST", "")
        writer.WriteElementString("SUFFIXLIST.LIST", "")


        writer.WriteStartElement("RESTARTFROMLIST.LIST") 'START RESTARTFROMLIST
        writer.WriteElementString("DATE", gridView.Rows(0).Cells("TRANDATE").Value.ToString)
        writer.WriteElementString("RESTARTFROM", "Yearly")
        writer.WriteElementString("PERIODBEGINNIGNUM", "1")
        writer.WriteStartElement("LASTNUMBERLIST.LIST") 'START LASTNUMBERLIST
        writer.WriteElementString("DATE", gridView.Rows(0).Cells("TRANDATE").Value.ToString)
        writer.WriteElementString("LASTNUMBER", "15")
        writer.WriteEndElement() 'END LASTNUMBERLIST
        writer.WriteEndElement() 'END RESTARTFROMLIST


        writer.WriteStartElement("RESTARTFROMLIST.LIST") 'START RESTARTFROMLIST
        writer.WriteElementString("DATE", gridView.Rows(0).Cells("TRANDATE").Value.ToString)
        writer.WriteElementString("RESTARTFROM", "Yearly")
        writer.WriteElementString("PERIODBEGINNIGNUM", "1")
        writer.WriteElementString("LASTNUMBERLIST", "")
        writer.WriteEndElement() 'END RESTARTFROMLIST


        writer.WriteElementString("VOUCHERCLASSLIST.LIST", "")
        writer.WriteElementString("PRODUCTCODEDETAILS.LIST", "")

        writer.WriteRaw("<UDF:ISUSEFOREXCISE.LIST TYPE = 'String' INDEX = '15124' ISLIST = 'Yes' DESC = 'IsUseforExcise'>  <UDF:ISUSEFOREXCISE DESC='IsUseforExcise'>No</UDF:ISUSEFOREXCISE> </UDF:ISUSEFOREXCISE.LIST>")
        writer.WriteEndElement() 'END VOUCHERTYPE
    End Sub

    Private Sub voucherObjectview(ByVal writer As XmlWriter)

        writer.WriteAttributeString("OBJVIEW", "OBJVIEW", Nothing, "Invoice Voucher View")
        writer.WriteAttributeString("REMOTEID", "REMOTEID", Nothing, "0073cba0-58fe-4056-9e37-4c8fee3ec61d-00000005")
        writer.WriteAttributeString("VCHKEY", "VCHKEY", Nothing, "0073cba0-58fe-4056-9e37-4c8fee3ec61d-0000a7e1:00000008")
        writer.WriteAttributeString("VCHTYPE", "VCHTYPE", Nothing, "Sales")
        writer.WriteAttributeString("ACTION", "ACTION", Nothing, "Create")


        'prblm
        writer.WriteStartElement("OLDAUDITENTRYIDS.LIST") 'START OLDAUDITENTRYIDS.LIST
        writer.WriteAttributeString("TYPE", "TYPE", Nothing, "Number")
        'writer.WriteStartElement("OLDAUDITENTRYIDS") 'START OLDAUDITENTRYIDS
        writer.WriteElementString("OLDAUDITENTRYIDS", 1)
        'writer.WriteEndElement() 'END OLDAUDITENTRYIDS
        writer.WriteEndElement() 'END OLDAUDITENTRYIDS.LIST
        'prblm


        writer.WriteElementString("DATE", "20170831")
        writer.WriteElementString("GUID", "0073cba0-58fe-4056-9e37-4c8fee3ec61d-00000005")
        writer.WriteElementString("STATENAME", gridView.Rows(0).Cells("STATE").Value.ToString)
        writer.WriteElementString("GSTREGISTRATIONTYPE", "Unregistered")
        writer.WriteElementString("VATDEALERTYPE", "Unregistered")

        writer.WriteElementString("COUNTRYOFRESIDENCE", "India")
        writer.WriteElementString("PARTYNAME", "Cash")

        writer.WriteElementString("VOUCHERTYPENAME", "Sales")
        writer.WriteElementString("REFERENCE", "5")
        writer.WriteElementString("VOUCHERNUMBER", "5")
        writer.WriteElementString("PARTYLEDGERNAME", "Cash")
        writer.WriteElementString("BASICBASEPARTYNAME", "Cash")
        writer.WriteElementString("CSTFORMISSUETYPE", "")
        writer.WriteElementString("CSTFORMRECVTYPE", "")
        writer.WriteElementString("FBTPAYMENTTYPE", "Default")
        writer.WriteElementString("PERSISTEDVIEW", "Invoice Voucher View")

        writer.WriteElementString("PLACEOFSUPPLY", gridView.Rows(0).Cells("STATE").Value.ToString)
        writer.WriteElementString("CONSIGNEEGSTIN", "33AAJFN9935A1ZB")

        writer.WriteElementString("BASICBUYERNAME", strCompanyName)
        writer.WriteElementString("BASICDATETIMEOFINVOICE", "30-Apr-2018 at 13:01")
        writer.WriteElementString("BASICDATETIMEOFREMOVAL", "30-Apr-2018 at 13:01")
        writer.WriteElementString("VCHGSTCLASS", "")
        writer.WriteElementString("CONSIGNEESTATENAME", gridView.Rows(0).Cells("STATE").Value.ToString)

        writer.WriteElementString("DIFFACTUALQTY", "No")
        writer.WriteElementString("ISMSTFROMSYNC", "No")
        writer.WriteElementString("ASORIGINAL", "No")
        writer.WriteElementString("AUDITED", "No")
        writer.WriteElementString("FORJOBCOSTING", "No")
        writer.WriteElementString("ISOPTIONAL", "No")
        writer.WriteElementString("EFFECTIVEDATE", "20170831")
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
        writer.WriteElementString("IGNOREPOSVALIDATION", "No")
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
        writer.WriteElementString("ISINVOICE", "No")
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
        writer.WriteElementString("ISVATDUTYPAID", "No")
        writer.WriteElementString("ISDELIVERYSAMEASCONSIGNEE", "No")
        writer.WriteElementString("ISDISPATCHSAMEASCONSIGNOR", "No")
        writer.WriteElementString("ISDELETED", "No")
        writer.WriteElementString("CHANGEVCHMODE", "No")
        writer.WriteElementString("ALTERID", "56")
        writer.WriteElementString("MASTERID", "5")
        writer.WriteElementString("VOUCHERKEY", "184584809480200")
        writer.WriteElementString("ASDF", "No")
        writer.WriteElementString("ASDF", "No")

        writer.WriteElementString("EXCLUDEDTAXATIONS.LIST", "")
        writer.WriteElementString("OLDAUDITENTRIES.LIST", "")
        writer.WriteElementString("ACCOUNTAUDITENTRIES.LIST", "")
        writer.WriteElementString("AUDITENTRIES.LIST", "")
        writer.WriteElementString("DUTYHEADDETAILS.LIST", "")
        writer.WriteElementString("SUPPLEMENTARYDUTYHEADDETAILS.LIST", "")
        writer.WriteElementString("EWAYBILLDETAILS.LIST", "")
        writer.WriteElementString("INVOICEDELNOTES.LIST", "")
        writer.WriteElementString("INVOICEORDERLIST.LIST", "")
        writer.WriteElementString("INVOICEINDENTLIST.LIST", "")
        writer.WriteElementString("ATTENDANCEENTRIES.LIST", "")
        writer.WriteElementString("ORIGINVOICEDETAILS.LIST", "")
        writer.WriteElementString("INVOICEEXPORTLIST.LIST", "")

    End Sub

    Private Sub Amout(ByVal writer As XmlWriter, ByVal amount As Double)
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
        If amount < 0 Then
            writer.WriteElementString("AMOUNT", "-" & amount & "")
        Else
            writer.WriteElementString("AMOUNT", amount)
        End If
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
    End Sub



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
                writer.WriteElementString("REPORTNAME", "Vouchers")
                writer.WriteStartElement("STATICVARIABLES") 'START REQUESTDESC
                writer.WriteElementString("SVCURRENTCOMPANY", "" & strCompanyName & " " & cnTranFromDate.Year & "-" & cnTranToDate.Year) '
                writer.WriteEndElement() 'END STATICVARIABLES
                writer.WriteEndElement() 'END REQUESTDESC

                writer.WriteStartElement("REQUESTDATA") 'START REQUESTDATA
                'For i As Integer = 0 To gridView.Rows.Count - 1
                'If gridView.Rows(i).Cells("CHK").Value.ToString = "" Then Continue For
                writer.WriteStartElement("TALLYMESSAGE") 'START TALLYMESSATE
                writer.WriteAttributeString("xmlns", "UDF", Nothing, "TallyUDF")
                CURRENCY(writer)
                CURRENCY(writer)
                GROUP(writer, "Indirect Incomes", "Income (Indirect)")
                LEDGER(writer, "OTC SGST", "String", "Services", "No", "No", 0, "", "Number", "Indirect Incomes")
                LEDGER(writer, "OTC CGST", "String", "Services", "No", "No", 0, "", "Number", "Indirect Incomes")
                GROUP(writer, "Sales Accounts", "")
                LEDGER(writer, "Sale Silver Articles Anklet Metti", "Number", "Goods", "Yes", "Yes", 1, "", "Number", "Indirect Incomes")
                LEDGER(writer, "Sale Gold Jewells", "Number", "Goods", "Yes", "Yes", 1, "", "Number", "Indirect Incomes")
                GROUP(writer, "Current Assets", "")
                GROUP(writer, "Cash-in-Hand", "Income (Indirect)")
                LEDGER(writer, "Cash", "Number", "Goods", "Yes", "Yes", 0, "", "Number", "Indirect Incomes")
                voucherTypereserver(writer)
                writer.WriteStartElement("VOUCHERTYPE") 'START VOUCHERTYPE
                voucherObjectview(writer)
                For i As Integer = 0 To gridView.Rows.Count - 1
                    If gridView.Rows(i).Cells("CHK").Value.ToString = "False" Then Continue For
                    'writer.WriteStartElement("VOUCHERTYPE") 'START VOUCHERTYPE
                    'voucherObjectview(writer)
                    Dim dtPaymode As New DataTable
                    strsql = " SELECT AMOUNT FROM " & cnStockDb & "..ACCTRAN "
                    strsql += vbCrLf + " WHERE TRANDATE = '" & Format(gridView.Rows(i).Cells("BILLDATE").Value, "yyyy-MM-dd") & "' "
                    strsql += vbCrLf + " AND BATCHNO = '" & (gridView.Rows(i).Cells("BATCHNO").Value.ToString) & "' "
                    strsql += vbCrLf + " And ISNULL(CANCEL,'') = '' AND TRANMODE = 'D'"
                    da = New OleDbDataAdapter(strsql, cn)
                    dtPaymode = New DataTable
                    da.Fill(dtPaymode)
                    For J As Integer = 0 To dtPaymode.Rows.Count - 1
                        Amout(writer, dtPaymode.Rows(i).Item("AMOUNT").ToString)
                    Next
                    strsql = " SELECT TAXAMOUNT AS AMOUNT,TAXID FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & Format(gridView.Rows(i).Cells("BILLDATE").Value, "yyyy-MM-dd") & "' "
                    strsql += vbCrLf + " AND BATCHNO = '" & (gridView.Rows(i).Cells("BATCHNO").Value.ToString) & "' "
                    strsql += vbCrLf + "AND ISNULL(TRANTYPE,'') = 'SA' AND ISNULL(STUDDED,'') = 'N'"
                    da = New OleDbDataAdapter(strsql, cn)
                    dtPaymode = New DataTable
                    da.Fill(dtPaymode)
                    For J As Integer = 0 To dtPaymode.Rows.Count - 1
                        Amout(writer, dtPaymode.Rows(i).Item("AMOUNT").ToString)
                    Next
                    writer.WriteElementString("ALLINVENTORYENTRIES.LIST", " ")
                    writer.WriteElementString("PAYROLLMODEOFPAYMENT.LIST", " ")
                    writer.WriteElementString("ATTDRECORDS.LIST", " ")
                    writer.WriteElementString("GSTEWAYCONSIGNORADDRESS.LIST", " ")
                    writer.WriteElementString("GSTEWAYCONSIGNEEADDRESS.LIST", " ")
                    writer.WriteElementString("TEMPGSTRATEDETAILS.LIST", " ")
                    '/// IMPORTANT
                    writer.WriteRaw("<UDF:TRADERCONSVATTINNO.LIST DESC = 'TraderConsVATTINNo' ISLIST = 'Yes' TYPE = 'String' INDEX = '10015'>  <UDF:ISUSEFOREXCISE DESC='`TraderConsVATTINNo`'>33283142301</UDF:ISUSEFOREXCISE> </UDF:TRADERCONSVATTINNO.LIST>")
                    '///
                    'writer.WriteEndElement() 'END TALLYMESSAGE
                    'writer.WriteEndElement() 'END VOUCHERTYPE
                Next
                writer.WriteEndElement() 'END TALLYMESSAGE
                writer.WriteEndElement() 'END VOUCHERTYPE

                writer.WriteStartElement("TALLYMESSAGE") 'START TALLYMESSATE
                writer.WriteAttributeString("xmlns", "UDF", Nothing, "TallyUDF")
                writer.WriteStartElement("COMPANY") 'START COMPANY
                writer.WriteStartElement("REMOTECMPINFO.LIST") 'START REMOTECMPINFO.LIST
                writer.WriteAttributeString("MERGE", "MERGE", Nothing, "Yes")
                writer.WriteElementString("NAME", "0073cba0-58fe-4056-9e37-4c8fee3ec61d")
                writer.WriteElementString("REMOTECMPNAME", "" & strCompanyName & "  " & cnTranFromDate.Year & "-" & cnTranToDate.Year & "")
                writer.WriteElementString("REMOTECMPSTATE", gridView.Rows(0).Cells("STATE").Value.ToString)
                writer.WriteEndElement() 'END REMOTECMPINFO.LIST
                writer.WriteEndElement() 'END COMPANY
                writer.WriteEndElement() 'END TALLYMESSAGE


                writer.WriteStartElement("TALLYMESSAGE") 'START TALLYMESSATE2
                writer.WriteAttributeString("xmlns", "UDF", Nothing, "TallyUDF")
                writer.WriteStartElement("COMPANY") 'START COMPANY
                writer.WriteStartElement("REMOTECMPINFO.LIST") 'START REMOTECMPINFO.LIST
                writer.WriteAttributeString("MERGE", "MERGE", Nothing, "Yes")
                writer.WriteElementString("NAME", "0073cba0-58fe-4056-9e37-4c8fee3ec61d")
                writer.WriteElementString("REMOTECMPNAME", "" & strCompanyName & " " & cnTranFromDate.Year & " -" & cnTranToDate.Year & "")
                writer.WriteElementString("REMOTECMPSTATE", gridView.Rows(0).Cells("STATE").Value.ToString)
                writer.WriteEndElement() 'END REMOTECMPINFO.LIST
                writer.WriteEndElement() 'END COMPANY
                writer.WriteEndElement() 'END TALLYMESSAGE2


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

        If txtBillNo.Text.Trim = "" Then
            MsgBox("Enter the Bill No..", MsgBoxStyle.Information)
            txtBillNo.Focus()

            Exit Sub
        End If

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
        strsql += vbCrLf + " ,I.BATCHNO"
        'strsql += vbCrLf + " ,I.SNO "
        'strsql += vbCrLf + " ,(SELECT TAXPER FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO = I.BATCHNO And TAXID = 'SG' AND ISSSNO = I.SNO) CGSTPER"
        'strsql += vbCrLf + " ,(SELECT TAXPER FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO = I.BATCHNO AND TAXID = 'CG' AND ISSSNO = I.SNO) SGSTPER"
        strsql += vbCrLf + " , '1.50' CGSTPER"
        strsql += vbCrLf + " , '1.50' SGSTPER"
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
        strsql += vbCrLf + " GROUP BY I.TRANNO,I.TRANDATE,I.BOARDRATE,I.BATCHNO,PE.PNAME,PE.AREA,PE.STATE,ISNULL(PE.GSTNO,''),ISNULL(I.COSTID,''),I.COMPANYID" ',I.SNO
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
