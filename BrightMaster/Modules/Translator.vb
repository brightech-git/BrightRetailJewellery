Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Web.Script.Serialization
Public Class Translator
    ''' When you have your own Client ID and secret, specify them here:
    Private Const CLIENT_ID As String = "FREE_TRIAL_ACCOUNT"
    Private Const CLIENT_SECRET As String = "PUBLIC_SECRET"
    Private Const API_URL As String = "http://api.whatsmate.net/v1/translation/translate"

    Public Function funcTranslate(ByVal fromLang As String, ByVal toLang As String, ByVal text As String) As String
        'Dim webClient As New WebClient()
        'Dim response As String = ""
        'Try
        '    Dim payloadObj As New Payload(fromLang, toLang, text)
        '    Dim serializer As New JavaScriptSerializer()
        '    Dim postData As String = serializer.Serialize(payloadObj)

        '    webClient.Headers("content-type") = "application/json"
        '    webClient.Headers("X-WM-CLIENT-ID") = CLIENT_ID
        '    webClient.Headers("X-WM-CLIENT-SECRET") = CLIENT_SECRET
        '    webClient.Encoding = Encoding.UTF8

        '    response = webClient.UploadString(API_URL, postData)

        'Catch webEx As WebException
        '    Dim res As HttpWebResponse = DirectCast(webEx.Response, HttpWebResponse)
        '    Dim stream As Stream = res.GetResponseStream()
        '    Dim reader As New StreamReader(stream)
        '    Dim body As String = reader.ReadToEnd()
        'End Try
        'Return response
    End Function
    Private Class Payload
        Public fromLang As String
        Public toLang As String
        Public text As String

        Sub New(ByVal fromLangCode As String, ByVal toLangCode As String, ByVal originalText As String)
            fromLang = fromLangCode
            toLang = toLangCode
            text = originalText
        End Sub
    End Class
End Class
