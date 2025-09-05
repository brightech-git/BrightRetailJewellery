Public Class CLS_EXPORTPROPERTIES
#Region "General Properties"
    Private chkAllColumnsSelect As Boolean = True
    Public Property p_chkAllColumnsSelect() As Boolean
        Get
            Return chkAllColumnsSelect
        End Get
        Set(ByVal value As Boolean)
            chkAllColumnsSelect = value
        End Set
    End Property
    Private SelectedColumns As New List(Of String)
    Public Property p_SelectedColumns() As List(Of String)
        Get
            Return SelectedColumns
        End Get
        Set(ByVal value As List(Of String))
            SelectedColumns = value
        End Set
    End Property
#End Region
#Region "INkJet Properies"
    Private chkInkCellColor As Boolean = True
    Public Property p_chkInkCellColor() As Boolean
        Get
            Return chkInkCellColor
        End Get
        Set(ByVal value As Boolean)
            chkInkCellColor = value
        End Set
    End Property
    Private chkInkCellBorder As Boolean = True
    Public Property p_chkInkCellBorder() As Boolean
        Get
            Return chkInkCellBorder
        End Get
        Set(ByVal value As Boolean)
            chkInkCellBorder = value
        End Set
    End Property
    Private chkInkFitToPageWidth As Boolean = True
    Public Property p_chkInkFitToPageWidth() As Boolean
        Get
            Return chkInkFitToPageWidth
        End Get
        Set(ByVal value As Boolean)
            chkInkFitToPageWidth = value
        End Set
    End Property
    Private chkInkLandscape As Boolean = False
    Public Property p_chkInkLandscape() As Boolean
        Get
            Return chkInkLandscape
        End Get
        Set(ByVal value As Boolean)
            chkInkLandscape = value
        End Set
    End Property
    Private chkInkWithCellFont As Boolean = True
    Public Property p_chkInkWithCellFont() As Boolean
        Get
            Return chkInkWithCellFont
        End Get
        Set(ByVal value As Boolean)
            chkInkWithCellFont = value
        End Set
    End Property
    Private txtInkFontDetail As String = ""
    Public Property p_txtInkFontDetail() As String
        Get
            Return txtInkFontDetail
        End Get
        Set(ByVal value As String)
            txtInkFontDetail = value
        End Set
    End Property
    Private txtInkSplitBy As Integer = 1
    Public Property p_txtInkSplitBy() As Integer
        Get
            Return txtInkSplitBy
        End Get
        Set(ByVal value As Integer)
            If value = 0 Then
                value = 1
            End If
            txtInkSplitBy = value
        End Set
    End Property
    Private lblCellBorderColor As Integer = Color.LightGray.ToArgb
    Public Property p_lblCellBorderColor() As Integer
        Get
            Return lblCellBorderColor
        End Get
        Set(ByVal value As Integer)
            lblCellBorderColor = value
        End Set
    End Property
    Private chkHeaderLeftalign As Boolean = True
    Public Property p_chkHeaderLeftalign() As Boolean
        Get
            Return chkHeaderLeftalign
        End Get
        Set(ByVal value As Boolean)
            chkHeaderLeftalign = value
        End Set
    End Property
    Private chkPrintDateTime As Boolean = True
    Public Property p_chkPrintDateTime() As Boolean
        Get
            Return chkPrintDateTime
        End Get
        Set(ByVal value As Boolean)
            chkPrintDateTime = value
        End Set
    End Property
    Private chkstrContent As Boolean = True
    Public Property p_chkShrinkContent() As Boolean
        Get
            Return chkstrContent
        End Get
        Set(ByVal value As Boolean)
            chkstrContent = value
        End Set
    End Property
    Private chkPrintHeadBold As Boolean = True
    Public Property p_chkPrintHeadBold() As Boolean
        Get
            Return chkPrintHeadBold
        End Get
        Set(ByVal value As Boolean)
            chkPrintHeadBold = value
        End Set
    End Property
    Private chkPrintUser As Boolean = True
    Public Property p_chkPrintUser() As Boolean
        Get
            Return chkPrintUser
        End Get
        Set(ByVal value As Boolean)
            chkPrintUser = value
        End Set
    End Property
#End Region
#Region "Pdf Properties"
    Private txtPdfMargTop As Integer = 50
    Public Property p_txtPdfMargTop() As Integer
        Get
            Return txtPdfMargTop
        End Get
        Set(ByVal value As Integer)
            txtPdfMargTop = value
        End Set
    End Property
    Private txtPdfMargBottom As Integer = 50
    Public Property p_txtPdfMargBottom() As Integer
        Get
            Return txtPdfMargBottom
        End Get
        Set(ByVal value As Integer)
            txtPdfMargBottom = value
        End Set
    End Property
    Private txtPdfMargLeft As Integer = 50
    Public Property p_txtPdfMargLeft() As Integer
        Get
            Return txtPdfMargLeft
        End Get
        Set(ByVal value As Integer)
            txtPdfMargLeft = value
        End Set
    End Property
    Private txtPdfMargRight As Integer = 50
    Public Property p_txtPdfMargRight() As Integer
        Get
            Return txtPdfMargRight
        End Get
        Set(ByVal value As Integer)
            txtPdfMargRight = value
        End Set
    End Property
    Private chkPdfCellColor As Boolean = True
    Public Property p_chkPdfCellColor() As Boolean
        Get
            Return chkPdfCellColor
        End Get
        Set(ByVal value As Boolean)
            chkPdfCellColor = value
        End Set
    End Property
    Private chkPdfHeaderForAllpages As Boolean = False
    Public Property p_chkPdfHeaderForAllpages() As Boolean
        Get
            Return chkPdfHeaderForAllpages
        End Get
        Set(ByVal value As Boolean)
            chkPdfHeaderForAllpages = value
        End Set
    End Property
    Private chkPdfFittoPageWid As Boolean = True
    Public Property p_chkPdfFittoPageWid() As Boolean
        Get
            Return chkPdfFittoPageWid
        End Get
        Set(ByVal value As Boolean)
            chkPdfFittoPageWid = value
        End Set
    End Property
    Private chkPdfLandscape As Boolean = False
    Public Property p_chkPdfLandscape() As Boolean
        Get
            Return chkPdfLandscape
        End Get
        Set(ByVal value As Boolean)
            chkPdfLandscape = value
        End Set
    End Property
    Private chkPdfBorder As Boolean = True
    Public Property p_chkPdfBorder() As Boolean
        Get
            Return chkPdfBorder
        End Get
        Set(ByVal value As Boolean)
            chkPdfBorder = value
        End Set
    End Property
    Private txtPdfSavePath As String = "C:\PdfGridView1.pdf"
    Public Property p_txtPdfSavePath() As String
        Get
            Return txtPdfSavePath
        End Get
        Set(ByVal value As String)
            txtPdfSavePath = value
        End Set
    End Property
#End Region
#Region "DotMatrix Properties"
    Private cmbDotPrinterNames As String = ""
    Public Property p_cmbDotPrinterNames() As String
        Get
            Return cmbDotPrinterNames
        End Get
        Set(ByVal value As String)
            cmbDotPrinterNames = value
        End Set
    End Property
    Private rbtDot80Column As Boolean = True
    Public Property p_rbtDot80Column() As Boolean
        Get
            Return rbtDot80Column
        End Get
        Set(ByVal value As Boolean)
            rbtDot80Column = value
        End Set
    End Property
    Private rbtDot40Column As Boolean = True
    Public Property p_rbtDot40Column() As Boolean
        Get
            Return rbtDot40Column
        End Get
        Set(ByVal value As Boolean)
            rbtDot40Column = value
        End Set
    End Property

    Private rbtDot132Column As Boolean = False
    Public Property p_rbtDot132Column() As Boolean
        Get
            Return rbtDot132Column
        End Get
        Set(ByVal value As Boolean)
            rbtDot132Column = value
        End Set
    End Property
    Private chkDotRowSeperation As Boolean = False
    Public Property p_chkDotRowSeperation() As Boolean
        Get
            Return chkDotRowSeperation
        End Get
        Set(ByVal value As Boolean)
            chkDotRowSeperation = value
        End Set
    End Property
    Private chkDotUnderLine As Boolean = False
    Public Property p_chkDotUnderLine() As Boolean
        Get
            Return chkDotUnderLine
        End Get
        Set(ByVal value As Boolean)
            chkDotUnderLine = value
        End Set
    End Property
    Private txtDotMultiCol As Integer = 1
    Public Property p_txtDotMultiCol() As Integer
        Get
            Return txtDotMultiCol
        End Get
        Set(ByVal value As Integer)
            If value = 0 Then value = 1
            txtDotMultiCol = value
        End Set
    End Property
    Private ChkDelHead As Boolean = False
    Public Property p_ChkDelHead() As Boolean
        Get
            Return ChkDelHead
        End Get
        Set(ByVal value As Boolean)
            ChkDelHead = value
        End Set
    End Property
    Private chkWithEject As Boolean = False
    Public Property p_chkWithEject() As Boolean
        Get
            Return chkWithEject
        End Get
        Set(ByVal value As Boolean)
            chkWithEject = value
        End Set
    End Property

    Private chkDotAutoFit As Boolean = True
    Public Property p_chkDotAutoFit() As Boolean
        Get
            Return chkDotAutoFit
        End Get
        Set(ByVal value As Boolean)
            chkDotAutoFit = value
        End Set
    End Property
    Private chkCompany As Boolean = True
    Public Property p_chkCompany() As Boolean
        Get
            Return chkCompany
        End Get
        Set(ByVal value As Boolean)
            chkCompany = value
        End Set
    End Property

    Private rbtDotAuto As Boolean = True
    Public Property p_rbtDotAuto() As Boolean
        Get
            Return rbtDotAuto
        End Get
        Set(ByVal value As Boolean)
            rbtDotAuto = value
        End Set
    End Property
    Private rbtDotMedium As Boolean = False
    Public Property p_rbtDotMedium() As Boolean
        Get
            Return rbtDotMedium
        End Get
        Set(ByVal value As Boolean)
            rbtDotMedium = value
        End Set
    End Property
    Private rbtDotCondensed As Boolean = False
    Public Property p_rbtDotCondensed() As Boolean
        Get
            Return rbtDotCondensed
        End Get
        Set(ByVal value As Boolean)
            rbtDotCondensed = value
        End Set
    End Property
    Private rbtDotMicro As Boolean = False
    Public Property p_rbtDotMicro() As Boolean
        Get
            Return rbtDotMicro
        End Get
        Set(ByVal value As Boolean)
            rbtDotMicro = value
        End Set
    End Property
    Private txtDotHeaderSeperator As String = "|"
    Public Property p_txtDotHeaderSeperator() As String
        Get
            Return txtDotHeaderSeperator
        End Get
        Set(ByVal value As String)
            txtDotHeaderSeperator = value
        End Set
    End Property
    Private txtDotLinesPerPage As Integer = 60
    Public Property p_txtDotLinesPerPage() As Integer
        Get
            Return txtDotLinesPerPage
        End Get
        Set(ByVal value As Integer)
            txtDotLinesPerPage = value
        End Set
    End Property
    Private chkDotHeadSep As Boolean = True
    Public Property p_chkDotHeadSep() As Boolean
        Get
            Return chkDotHeadSep
        End Get
        Set(ByVal value As Boolean)
            chkDotHeadSep = value
        End Set
    End Property
    Private chkDotHeadBold As Boolean = True
    Public Property p_chkDotHeadBold() As Boolean
        Get
            Return chkDotHeadBold
        End Get
        Set(ByVal value As Boolean)
            chkDotHeadBold = value
        End Set
    End Property
#End Region
#Region "Excel Properties"
    Private chkXLVisibleColOnly As Boolean = True
    Public Property p_chkXLVisibleColOnly() As Boolean
        Get
            Return chkXLVisibleColOnly
        End Get
        Set(ByVal value As Boolean)
            chkXLVisibleColOnly = value
        End Set
    End Property
    Private chkXLHighlightCellFont As Boolean = False
    Public Property p_chkXLHighlightCellFont() As Boolean
        Get
            Return chkXLHighlightCellFont
        End Get
        Set(ByVal value As Boolean)
            chkXLHighlightCellFont = value
        End Set
    End Property
    Private chkXLHighlightCellColor As Boolean = False
    Public Property p_chkXLHighlightCellColor() As Boolean
        Get
            Return chkXLHighlightCellColor
        End Get
        Set(ByVal value As Boolean)
            chkXLHighlightCellColor = value
        End Set
    End Property
#End Region
End Class
