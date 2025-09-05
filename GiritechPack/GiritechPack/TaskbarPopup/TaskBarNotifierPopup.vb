Public Class TaskBarNotifierPopup
    Private Shared WithEvents taskbarNotifier1 As TaskBarNotifier
    Public Shared Sub Show()
        taskbarNotifier1 = New TaskBarNotifier()
        'taskbarNotifier1.SetBackgroundBitmap(My.Resources.msnPopUp, Color.FromArgb(255, 0, 255))
        taskbarNotifier1.SetCloseBitmap(My.Resources.close, Color.FromArgb(255, 0, 255), New Point(127, 8))
        taskbarNotifier1.TitleRectangle = New Rectangle(40, 9, 70, 25)
        taskbarNotifier1.TextRectangle = New Rectangle(8, 41, 133, 68)
        With taskbarNotifier1
            .CloseButtonClickEnabled = True
            .TitleClickEnabled = False
            .TextClickEnabled = True
            .DrawTextFocusRect = False
            .KeepVisibleOnMouseOver = True
            .ReShowOnMouseOver = False
            .Show("Alert", "Transfered", 500, 3000, 500)
        End With
    End Sub
End Class
