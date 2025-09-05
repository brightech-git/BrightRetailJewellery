Public Class GDataRow
    Inherits Data.DataRow
    Private _Datatable As BrightPosting.GDatatable
    Private RowHeight As Integer = 21
    Public Property pRowHeight() As Integer
        Get
            Return RowHeight
        End Get
        Set(ByVal value As Integer)
            RowHeight = value
        End Set
    End Property
    Friend Sub New(ByVal rb As DataRowBuilder)
        MyBase.New(rb)
        Me._Datatable = CType(Me.Table, BrightPosting.GDatatable)
    End Sub
End Class
