Public Class GDatatable
    Inherits DataTable
    Private ColHeaderVisible As Boolean = True
    Private ColHeaderFont As New Font("Verdana", 9, FontStyle.Bold)
    Private ContentFont As New Font("Verdana", 9, FontStyle.Regular)
    Private CellBorder As Boolean = True
    Private CellBorderColor As Pen = Pens.Gray
    Private RowHeight As Integer = 21
    Private TablePadding As Integer = 0

    Public Property pTablePadding() As Integer
        Get
            Return TablePadding
        End Get
        Set(ByVal value As Integer)
            TablePadding = value
        End Set
    End Property

    Public Property pCellBorderColor() As Pen
        Get
            Return CellBorderColor
        End Get
        Set(ByVal value As Pen)
            CellBorderColor = value
        End Set
    End Property

    Public Property pColHeaderFont() As Font
        Get
            Return ColHeaderFont
        End Get
        Set(ByVal value As Font)
            ColHeaderFont = value
        End Set
    End Property
    Public Property pContentFont() As Font
        Get
            Return ContentFont
        End Get
        Set(ByVal value As Font)
            ContentFont = value
        End Set
    End Property

    Public Property pCellBorder() As Boolean
        Get
            Return CellBorder
        End Get
        Set(ByVal value As Boolean)
            CellBorder = value
        End Set
    End Property

    Public Property pColHeaderVisible() As Boolean
        Get
            Return ColHeaderVisible
        End Get
        Set(ByVal value As Boolean)
            ColHeaderVisible = value
        End Set
    End Property
    Public Property pRowHeight() As Integer
        Get
            Return RowHeight
        End Get
        Set(ByVal value As Integer)
            RowHeight = value
        End Set
    End Property

    Private TableContentAlignment As StringAlignment = Nothing
    Public Property pTableContentAlignment() As StringAlignment
        Get
            Return TableContentAlignment
        End Get
        Set(ByVal value As StringAlignment)
            TableContentAlignment = value
        End Set
    End Property

    Private TableBackColor As Color = Nothing
    Public Property pTableBackColor() As Color
        Get
            Return TableBackColor
        End Get
        Set(ByVal value As Color)
            TableBackColor = value
        End Set
    End Property

    Protected Overrides Function NewRowFromBuilder(ByVal builder As DataRowBuilder) As DataRow
        Return New GDataRow(builder)
    End Function

    Default Public ReadOnly Property GRows(ByVal index As Integer) As GDataRow
        Get
            Return CType(Me.Rows(index), GDataRow)
        End Get
    End Property

End Class
