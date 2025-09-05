Public Class GDataColumn
    Inherits Data.DataColumn
    Private Alignment As StringAlignment = StringAlignment.Near
    Public Property pAlignment() As StringAlignment
        Get
            Return Alignment
        End Get
        Set(ByVal value As StringAlignment)
            Alignment = value
        End Set
    End Property
    Private BackColor As Color = Nothing
    Public Property pBackColor() As Color
        Get
            Return BackColor
        End Get
        Set(ByVal value As Color)
            BackColor = value
        End Set
    End Property
End Class
