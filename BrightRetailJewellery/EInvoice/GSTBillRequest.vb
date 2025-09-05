Imports System.Collections.Generic
Public Class GSTBillRequest
        Public Property Version As String
        Public Property TranDtls As EinvdirectTranDtlsResponse
        Public Property DocDtls As EinvdirectDocDtlsResponse
        Public Property SellerDtls As EinvdirectSellerDtlsResponse
        Public Property BuyerDtls As EinvdirectBuyerDtlsResponse
        Public Property DispDtls As EinvdirectDispDtlsResponse
        Public Property ShipDtls As EinvdirectShipDtlsResponse
        Public Property ItemList As List(Of ItemList)
        Public Property ValDtls As EinvdirectValDtlsResponse
    End Class

    Public Class EinvdirectBillResponse
        Public Property data As EinvdirectbillTokenData
        Public Property status_cd As String
        Public Property status_desc As String
    End Class

    Public Class EinvdirectbillTokenData
        Public Property UserName As String
        Public Property TokenExpiry As String
        Public Property Sek As String
        Public Property ClientId As String
        Public Property AuthToken As String
        Public Property Irn As String
        Public Property SignedQRCode As String
        Public Property Status As String
    End Class

    Public Class EinvdirectTranDtlsResponse
        Public Property TaxSch As String
        Public Property SupTyp As String
    End Class

    Public Class EinvdirectDocDtlsResponse
        Public Property Typ As String
        Public Property No As String
        Public Property Dt As String
    End Class

    Public Class EinvdirectSellerDtlsResponse
        Public Property Gstin As String
        Public Property LglNm As String
        Public Property Addr1 As String
        Public Property Loc As String
        Public Property Pin As Integer
        Public Property Stcd As String
    End Class

    Public Class EinvdirectBuyerDtlsResponse
        Public Property Gstin As String
        Public Property Addr1 As String
        Public Property LglNm As String
        Public Property Loc As String
        Public Property Pos As String
        Public Property Stcd As String
        Public Property Pin As Integer
    End Class

    Public Class EinvdirectDispDtlsResponse
        Public Property Nm As String
        Public Property Addr1 As String
        Public Property Loc As String
        Public Property Stcd As String
        Public Property Pin As Integer
    End Class

    Public Class EinvdirectShipDtlsResponse
        Public Property Gstin As String
        Public Property LglNm As String
        Public Property Addr1 As String
        Public Property Loc As String
        Public Property Stcd As String
        Public Property Pin As Integer
    End Class

    Public Class ItemList
        Public Property SlNo As String
        Public Property IsServc As String
        Public Property HsnCd As String
        Public Property Qty As Double
        Public Property Unit As String
        Public Property UnitPrice As Double
        Public Property TotAmt As Double
        Public Property Discount As Double
        Public Property AssAmt As Double
        Public Property GstRt As Double
        Public Property SgstAmt As Decimal
        Public Property CgstAmt As Decimal
        Public Property IgstAmt As Decimal
        Public Property TotItemVal As Decimal
    End Class

    Public Class EinvdirectValDtlsResponse
        Public Property AssVal As Decimal
        Public Property CgstVal As Decimal
        Public Property SgstVal As Decimal
        Public Property IgstVal As Decimal
        Public Property Discount As Decimal
        Public Property OthChrg As Decimal
        Public Property RndOffAmt As Decimal
        Public Property TotInvVal As Decimal
    End Class
