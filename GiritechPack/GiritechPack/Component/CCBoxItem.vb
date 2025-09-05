Imports System.Collections.Generic
Imports System.Text
Public Class CCBoxItem
    Private val As Integer
    Public Property Value() As Integer
        Get
            Return val
        End Get
        Set(ByVal value As Integer)
            val = value
        End Set
    End Property

    Private m_name As String
    Public Property Name() As String
        Get
            Return m_name
        End Get
        Set(ByVal value As String)
            m_name = value
        End Set
    End Property

    Public Sub New()
    End Sub

    Public Sub New(ByVal name As String, ByVal val As Integer)
        Me.m_name = name
        Me.val = val
    End Sub

    Public Overrides Function ToString() As String
        Return String.Format("name: '{0}', value: {1}", m_name, val)
    End Function
End Class
