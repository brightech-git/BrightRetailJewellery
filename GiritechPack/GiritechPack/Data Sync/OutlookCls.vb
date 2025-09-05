Imports Microsoft.Office.Interop
Public Class OutlookCls
    Public Shared Function SyncOutlook() As Boolean
        Dim oApp As New Outlook.Application

        Dim oNS As Outlook._NameSpace = oApp.GetNamespace("mapi")
        Dim oSyncs As Outlook.SyncObjects
        Dim oSync As Outlook.SyncObject
        Try
            ' Reference SyncObjects.
            oSyncs = oNS.SyncObjects
            oSync = oSyncs.Item("All Accounts")
            ' Send and receive.
            oSync.Start()
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
        ' Clean up.
        oSync = Nothing
        oSyncs = Nothing
        oNS = Nothing
        oApp = Nothing
        Return True
    End Function

End Class
