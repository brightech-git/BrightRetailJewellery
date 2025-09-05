Imports System.Net
Imports RestSharp
Imports Newtonsoft.Json
Imports System.Linq

Public Class GSTBill
    Dim _token As String = ""
    Dim gstin As String = ""
    Dim username As String = ""
    Dim password As String = ""
    Dim ip_address As String = ""
    Dim client_id As String = ""
    Dim client_secret As String = ""
    Dim authtokenurl As String = ""
    Dim invoiceurl As String = ""
    Sub New(dt As DataTable)
        gstin = dt.Rows(0)("gstin").ToString
        username = dt.Rows(0)("username").ToString
        password = dt.Rows(0)("password").ToString
        ip_address = dt.Rows(0)("ip_address").ToString
        client_id = dt.Rows(0)("client_id").ToString
        client_secret = dt.Rows(0)("client_secret").ToString
        authtokenurl = dt.Rows(0)("authtokenurl").ToString
        invoiceurl = dt.Rows(0)("invoiceurl").ToString
    End Sub
    Private Function Createauthtoken() As EinvdirectBillResponse
        Dim ResponseMessage As New EinvdirectBillResponse()
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls

        Dim url As String = authtokenurl
        Dim client As New RestClient(url)
        Dim request As New RestRequest(Method.[GET])

        request.AddHeader("Accept", "application/json")
        request.AddHeader("username", username)
        request.AddHeader("password", password)
        request.AddHeader("ip_address", ip_address)
        request.AddHeader("client_id", client_id)
        request.AddHeader("client_secret", client_secret)
        request.AddHeader("gstin", gstin)

        Dim response As IRestResponse = client.Execute(request)
        ResponseMessage = JsonConvert.DeserializeObject(Of EinvdirectBillResponse)(response.Content)

        If ResponseMessage.status_cd = "Sucess" Then
            _token = ResponseMessage.data.AuthToken
        End If

        Return ResponseMessage
    End Function
    Public Function GenerateEinvoice(dtComp As DataTable, dtBuyer As DataTable, dtItem As DataTable) As EinvdirectBillResponse
        Dim ResponseMessage As New EinvdirectBillResponse()
        Dim einvoice As New GSTBillRequest()
        Dim TranDtls As New EinvdirectTranDtlsResponse()
        Dim DocDtls As New EinvdirectDocDtlsResponse()
        Dim SellerDtls As New EinvdirectSellerDtlsResponse()
        Dim BuyerDtls As New EinvdirectBuyerDtlsResponse()
        Dim DispDtls As New EinvdirectDispDtlsResponse()
        Dim ShipDtls As New EinvdirectShipDtlsResponse()
        Dim ValDtls As New EinvdirectValDtlsResponse()
        Dim ItemList As New List(Of ItemList)()
        Dim igstAmt As Double
        Dim cgstAmt As Double
        Dim sgstAmt As Double

        If dtItem.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("IGST")) <> 0 Then
            cgstAmt = 0
            sgstAmt = 0
            igstAmt = dtItem.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("IGST"))
        Else
            igstAmt = 0
            cgstAmt = dtItem.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("TAX")) / 2
            sgstAmt = dtItem.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("TAX")) / 2
        End If

        einvoice.Version = "1.1"

        TranDtls.TaxSch = "GST"
        TranDtls.SupTyp = "B2B"

        DocDtls.Typ = "INV"
        DocDtls.No = dtItem.Rows(0)("TRANNO").ToString
        DocDtls.Dt = dtItem.Rows(0)("TRANDATE").ToString

        SellerDtls.LglNm = dtComp.Rows(0)("COMPANYNAME").ToString
        SellerDtls.Gstin = dtComp.Rows(0)("GSTNO").ToString
        SellerDtls.Loc = dtComp.Rows(0)("ADDRESS3").ToString
        SellerDtls.Pin = dtComp.Rows(0)("AREACODE")
        SellerDtls.Addr1 = dtComp.Rows(0)("ADDRESS1").ToString
        SellerDtls.Stcd = dtComp.Rows(0)("STATECODE").ToString

        If dtBuyer.Rows(0)("PINCODE").ToString = "" Then
            Throw New Exception("Pincode not available for customer.")
        End If

        If dtBuyer.Rows(0)("PNAME").ToString = "" Then
            Throw New Exception("Name not available for customer.")
        End If

        If dtBuyer.Rows(0)("STATECODE").ToString = "" Then
            Throw New Exception("Statecode not available for customer.")
        End If

        If dtBuyer.Rows(0)("AREA").ToString = "" Then
            Throw New Exception("AREA not available for customer.")
        End If

        BuyerDtls.Gstin = dtBuyer.Rows(0)("GSTNO").ToString
        BuyerDtls.LglNm = dtBuyer.Rows(0)("PNAME").ToString
        BuyerDtls.Addr1 = dtBuyer.Rows(0)("ADDRESS1").ToString
        BuyerDtls.Pin = dtBuyer.Rows(0)("PINCODE")
        BuyerDtls.Pos = dtBuyer.Rows(0)("STATECODE").ToString
        BuyerDtls.Stcd = dtBuyer.Rows(0)("STATECODE").ToString
        BuyerDtls.Loc = dtBuyer.Rows(0)("AREA").ToString

        'DispDtls.Nm = ""
        'DispDtls.Loc = ""
        'DispDtls.Pin = 0
        'DispDtls.Stcd = ""
        'DispDtls.Addr1 = ""

        'ShipDtls.Gstin = ""
        'ShipDtls.LglNm = ""
        'ShipDtls.Addr1 = ""
        'ShipDtls.Loc = ""
        'ShipDtls.Pin = 0
        'ShipDtls.Stcd = ""

        ValDtls.AssVal = dtItem.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("AMOUNT"))
        ValDtls.CgstVal = cgstAmt
        ValDtls.SgstVal = sgstAmt
        ValDtls.IgstVal = igstAmt
        ValDtls.Discount = dtItem.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("DISCOUNT"))
        ValDtls.OthChrg = 0
        ValDtls.RndOffAmt = 0
        ValDtls.TotInvVal = dtItem.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("TOTALAMT"))

        Dim _slno As Integer = 0
        For Each row As DataRow In dtItem.Rows
            _slno = _slno + 1
            ItemList.Add(New ItemList With {
            .SlNo = _slno,
            .IsServc = "N",
            .HsnCd = row("HSN").ToString,
            .Qty = row("PCS").ToString,
            .Unit = "GMS",
            .UnitPrice = row("AMOUNT").ToString,
            .TotAmt = row("AMOUNT").ToString,
            .Discount = 0,'row("DISCOUNT").ToString, Suresh 15-07-25
            .AssAmt = row("AMOUNT").ToString,
            .GstRt = row("SALESTAX").ToString,
            .SgstAmt = sgstAmt,
            .CgstAmt = cgstAmt,
            .IgstAmt = igstAmt,
            .TotItemVal = row("TOTALAMT").ToString
            })
        Next

        einvoice.TranDtls = TranDtls
        einvoice.DocDtls = DocDtls
        einvoice.SellerDtls = SellerDtls
        einvoice.BuyerDtls = BuyerDtls
        'einvoice.DispDtls = DispDtls
        'einvoice.ShipDtls = ShipDtls
        einvoice.ItemList = ItemList
        einvoice.ValDtls = ValDtls

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls

        Createauthtoken()

        Dim url As String = invoiceurl
        Dim client As New RestClient(url)
        Dim request As New RestRequest(Method.POST)

        request.AddHeader("Accept", "application/json")
        request.AddHeader("Content-Type", "application/json")
        request.AddHeader("username", username)
        request.AddHeader("ip_address", ip_address)
        request.AddHeader("client_id", client_id)
        request.AddHeader("client_secret", client_secret)
        request.AddHeader("auth-token", _token)
        request.AddHeader("gstin", gstin)

        Dim jsonString As String = JsonConvert.SerializeObject(einvoice)
        request.AddParameter("application/json", jsonString, ParameterType.RequestBody)

        Dim response As IRestResponse = client.Execute(request)
        ResponseMessage = JsonConvert.DeserializeObject(Of EinvdirectBillResponse)(response.Content)
        Return ResponseMessage
    End Function
End Class
