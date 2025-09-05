Imports System.IO
Imports System.Data.OleDb
Imports VB = Microsoft.VisualBasic
Imports System.Text.RegularExpressions
Imports System.Net
Imports System.Net.Security
Imports System.Web.UI.HtmlControls
Imports System.Globalization
Imports System.Math
Imports System.Drawing

Public Class frmWebTag
    '#    For Image Capture          #
    ' 328 kalaiarasan .
    '01 SHERIFF 25-10-12
    'CALNO 386 ALTER BY VASANTH FOR PRINCE- CALCMODE LODE FROM SOFTCONTROL ONLY
    'CALNO 280913 ALTER BY VASANTH FOR PRINCE- Order tag can allow when smith record will found
    Dim oldpath, oldpath1, newpath, newpath1 As String
    Dim flagDeviceMode As Boolean = False
    Dim iDevice As Integer = 0
    Const CAP As Short = &H400S
    Const CAP_DRIVER_CONNECT As Integer = CAP + 10
    Const CAP_DRIVER_DISCONNECT As Integer = CAP + 11
    Const CAP_EDIT_COPY As Integer = CAP + 30
    Const CAP_SET_PREVIEW As Integer = CAP + 50
    Const CAP_SET_PREVIEWRATE As Integer = CAP + 52
    Const CAP_SET_SCALE As Integer = CAP + 53
    Const WS_CHILD As Integer = &H40000000
    Const WS_VISIBLE As Integer = &H10000000
    Const SWP_NOMOVE As Short = &H2S
    Const SWP_NOSIZE As Short = 1
    Const SWP_NOZORDER As Short = &H4S
    Const HWND_BOTTOM As Short = 1
    Dim lotprintack As Boolean = IIf(GetAdmindbSoftValue("LOTACKPRINT", "N") = "Y", True, False)
    Dim Lotautopost As Boolean = IIf(GetAdmindbSoftValue("ACC_LOTAUTOPOST", "N") = "Y", True, False)
    Dim LOTNARRATION As Boolean = IIf(GetAdmindbSoftValue("LOTNARRATION", "N") = "Y", True, False)
    Dim STKAFINDATE As Boolean = IIf(GetAdmindbSoftValue("STKAFINDATE", "N") = "Y", True, False)
    Dim ORDERLOTITEM As Boolean = IIf(GetAdmindbSoftValue("ORDERLOT_ITEM", "N") = "Y", True, False)
    Dim CHKSMITHREC As Boolean = IIf(GetAdmindbSoftValue("CHK_SMITH_REC", "N") = "Y", True, False)
    Dim TAGEDITDISABLE As String = GetAdmindbSoftValue("TAGEDITDISABLE", "")
    Dim CHKBOOKSTOCK As Boolean = IIf(GetAdmindbSoftValue("CHK_BOOK_STK", "N") = "Y", True, False)
    Dim strprint As String
    Dim Ratevaluezero As Boolean = False
    Dim FileWrite As StreamWriter
    Dim hHwnd As Integer  ' Handle value to preview window
    Declare Function SendMessage Lib "user32" Alias "SendMessageA" _
        (ByVal hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, _
         ByVal lParam As Object) As Integer
    Declare Function SetWindowPos Lib "user32" Alias "SetWindowPos" (ByVal hwnd As Integer, _
        ByVal hWndInsertAfter As Integer, ByVal x As Integer, ByVal y As Integer, _
        ByVal cx As Integer, ByVal cy As Integer, ByVal wFlags As Integer) As Integer

    Declare Function DestroyWindow Lib "user32" (ByVal hndw As Integer) As Boolean
    Declare Function capCreateCaptureWindowA Lib "avicap32.dll" _
            (ByVal lpszWindowName As String, ByVal dwStyle As Integer, _
            ByVal x As Integer, ByVal y As Integer, ByVal nWidth As Integer, _
            ByVal nHeight As Short, ByVal hWndParent As Integer, _
            ByVal nID As Integer) As Integer

    '#    For Image Capture          #


    Dim objPropertyDia As New PropertyDia(wSerialPort1)
    Dim ItemLock As Boolean
    Dim updIssSno As String
    Dim WupdIssSno As String
    Dim dtTagDetails As New DataTable
    Dim dtWTagDetails As New DataTable
    Dim lotPcs As Integer
    Dim lotGrsWt As Double
    Dim lotNetWt As Double
    Dim tagEdit As Boolean
    Dim multiMetalCalc As Boolean = False
    Dim BlockSv As Boolean = False
    Dim isStyleCode As Boolean
    Dim dtGridView As New DataTable
    Dim dtStoneDetails As New DataTable("gridStone")
    Dim dtMultiMetalDetails As New DataTable("gridMultimetal")
    Dim dtMiscDetails As New DataTable("gridMisc")

    Dim dtWGridView As New DataTable
    Dim dtWStoneDetails As New DataTable("WgridStone")
    Dim dtWMultiMetalDetails As New DataTable("WgridMultimetal")
    Dim dtWMiscDetails As New DataTable("WgridMisc")

    Dim METALID As String
    Dim OthCharge As String
    Dim sizeStock As String
    Dim studdedStone As String
    Dim grossnetdiff As String
    Dim multiMetal As String
    Dim calType As String
    Dim noOfPiece As Integer
    Dim pieceRate As Double
    Dim Recsno As String
    Dim mfromItemid As Integer = 0
    Dim SNO As String
    Dim strSql As String
    Dim cmd As OleDbCommand
    'Dim tran As OleDbTransaction

    Dim dReader As OleDbDataReader
    Dim gridStoneCurrentRow As Integer

    Dim TagNoGen As String = Nothing
    Dim TagNoFrom As String = Nothing
    Dim LastTagNo As String = Nothing
    Dim currentTagNo As String = Nothing
    Dim entryOrder As Integer
    Dim lotRecieptDate As Date = Nothing
    Dim purRate As Double = Nothing

    Dim purVal As Double = 0
    ''Picture
    Dim defalutDestination As String = Nothing
    Dim picExtension As String = Nothing
    Dim picPath As String = Nothing
    Dim defalutSourcePath As String = Nothing

    ''Calculation Option
    Dim CalMode As String
    Dim FixedVa As Boolean = False

    Dim flagPurValSet As Boolean = False

    Dim WastageRound As Integer = Val(GetAdmindbSoftValue("ROUNDOFF-WASTAGE", "3"))
    Dim McRnd As Integer = Val(GetAdmindbSoftValue("ROUNDOFF-MC", "2"))
    Dim tabCheckBy As String = GetAdmindbSoftValue("LOTCHECKBY", "P")
    Dim Lotchkdate As Boolean = IIf(GetAdmindbSoftValue("LOTCHKDATE", "N") = "Y", True, False)
    Dim OrderRow As DataRow = Nothing
    Dim LockLessWt As Boolean = IIf(GetAdmindbSoftValue("LOCKLESSWT", "Y") = "Y", True, False)
    Dim objTag As New TagGeneration
    Dim costLock As Boolean = IIf(GetAdmindbSoftValue("TAGCOSTLOCK") = "Y", True, False)
    Dim TOUCH_LOCK_TAG As Boolean = IIf(GetAdmindbSoftValue("TOUCH_LOCK_TAG", "N") = "Y", True, False)

    Dim StudWtDedut As String = GetAdmindbSoftValue("STUDWTDEDUCT", "DSP")
    Dim McWithWastage As Boolean = IIf(GetAdmindbSoftValue("MCWITHWASTAGE", "N") = "Y", True, False)

    Dim checkAutoWeight As Boolean = IIf(GetAdmindbSoftValue("AUTOWEIGHT", "N") = "Y", True, False)
    Dim checkBarCode As Boolean = IIf(GetAdmindbSoftValue("BARCODE", "N") = "Y", True, False)
    Dim checkGuaranteeCard As Boolean = IIf(GetAdmindbSoftValue("GURANTEECARD", "N") = "Y", True, False)
    Dim SalVal_Lock As Boolean = IIf(GetAdmindbSoftValue("SALVAL_LOCK_TAG", "N") = "Y", True, False)

    Dim Tag_Tolerance As Decimal = Val(GetAdmindbSoftValue("TAGTOLERANCE", "0"))
    Dim NEEDUS As Boolean = IIf(GetAdmindbSoftValue("NEEDUS", "N") = "Y", True, False)
    Dim HALLMARKVALID As Boolean = IIf(GetAdmindbSoftValue("HALLMARK_VALID", "N") = "Y", True, False)
    Dim SALEVALUEPLUS As Decimal = Val(GetAdmindbSoftValue("SALEVALUEPLUS", "0"))
    Dim SALVALUEROUND As Decimal = Val(GetAdmindbSoftValue("STKSALVALUEROUND", "0"))
    Dim TagDlrSealGet As Boolean = IIf(GetAdmindbSoftValue("TAGDLRSEALGET", "N") = "Y", True, False)
    Dim PacketNoEnable As String = GetAdmindbSoftValue("PACKETNO_ENABLE", "N")
    Dim TAGENTRY_FOCUS As String = GetAdmindbSoftValue("TAG_ENTRY_FOCUS", "P")
    Dim CENTRATE_DESIGNER As Boolean = IIf(GetAdmindbSoftValue("CENTRATE_DES", "Y") = "Y", True, False)
    Dim CHKCOST_ORD As Boolean = IIf(GetAdmindbSoftValue("ORD_CHK_COST", "N") = "Y", True, False)
    Dim ALLOYGOLD_SALEMODE As Boolean = IIf(GetAdmindbSoftValue("ALLOYGOLD_SALEMODE", "N") = "Y", True, False)
    Dim VALUECALC_GRNETOPT As Boolean = IIf(GetAdmindbSoftValue("VALUECALC_GRNETOPT", "N") = "Y", True, False)
    Dim Port_BaudRate As Integer = 9600
    Dim Port_DataBit As Integer = 8
    Dim Port_PortName As String = "COM1"
    Dim Port_Parity As String = "N"
    Dim ObjOrderTagInfo As New TagOrderInfo
    Dim ObjMinValue As TagMinValues
    Dim ObjPurDetail As New TagPurchaseDetailEntry
    Dim ObjExtraWt As New frmExtaWt
    Dim ObjRsUs As New frmRsUs
    Dim PurDetailSize As Size = ObjPurDetail.Size
    Dim _HasHallMark As Boolean = IIf(GetAdmindbSoftValue("HALLMARK_LOCK", "N") = "N", False, True)
    Dim _HasMinMc As Boolean = IIf(GetAdmindbSoftValue("MINMCTAB", "N") = "N", False, True)
    Dim _HasPurchase As Boolean = IIf(GetAdmindbSoftValue("PURTAB", "N") = "N", False, True)
    Dim PUR_AUTOCALC As Boolean = IIf(GetAdmindbSoftValue("PUR_AUTOCALC", "N") = "N", False, True)
    Dim _HasRfId As Boolean = IIf(GetAdmindbSoftValue("RFID_LOCK", "N") = "N", False, True)
    Dim _TagImageMust As Boolean = IIf(GetAdmindbSoftValue("ATTACHIMAGE_MUST", "N") = "Y", True, False)
    Dim _CheckOrderInfo As Boolean = IIf(GetAdmindbSoftValue("CHECKORDERINFO", "Y") = "Y", True, False)
    Dim _MCONGRSNET As Boolean = IIf(GetAdmindbSoftValue("MC_ON_GRSNET", "Y") = "Y", True, False)
    Dim _WASTONGRSNET As Boolean = IIf(GetAdmindbSoftValue("WAST_ON_GRSNET", "Y") = "Y", True, False)
    Dim _MCCALCON_ITEM_GRS As Boolean = False
    Dim _VALUECALCON_ITEM_GRS As Boolean = False

    Dim lotPurRate As Decimal = Nothing
    Dim lotPurTouch As Decimal = Nothing
    Dim lotPurWastagePer As Decimal = Nothing
    Dim lotPurMcPerGrm As Decimal = Nothing

    Dim EditOrNo As String = Nothing
    Dim EditEmpId As String = Nothing

    Dim purSalevalue As Decimal

    Dim Studded_Loose As String = Nothing
    Dim objdatescr As New frmDateInput
    Dim RFID_PORT As String = GetAdmindbSoftValue("RFID_PORT", "38400/1/8E1")
    Dim STK_REORD_VALID As Boolean = IIf(GetAdmindbSoftValue("STK_REORD_VALID", "Y") = "Y", True, False)
    Dim _FourCMaintain As Boolean = IIf(GetAdmindbSoftValue("4CMAINTAIN", "Y") = "Y", True, False)
    Dim ObjDiaDetails As New frmDiamondDetails
    Dim Cut As String = ""
    Dim Color As String = ""
    Dim Clarity As String = ""
    Dim Shape As String = ""
    Dim SetType As String = ""
    Dim Width As Decimal = 0
    Dim Height As Decimal = 0
    Dim TagCutId As Integer = 0
    Dim TagColorId As Integer = 0
    Dim TagClarityId As Integer = 0
    Dim TagShapeId As Integer = 0
    Dim TagSetTypeId As Integer = 0
    Dim TagWidth As Decimal = 0
    Dim TagHeight As Decimal = 0
    Dim WTagCutId As Integer = 0
    Dim WTagColorId As Integer = 0
    Dim WTagClarityId As Integer = 0
    Dim WTagShapeId As Integer = 0
    Dim WTagSetTypeId As Integer = 0
    Dim WTagWidth As Decimal = 0
    Dim WTagHeight As Decimal = 0
    Dim TChkbStk As Boolean = True
    Dim DtAddStockEntry As New DataTable
    Dim AddStockEntry As Boolean = True
    Dim SubItemPic As Boolean = False
    Dim STUDDEDWTPER As String = GetAdmindbSoftValue("STUDDEDWTPER", "N")
    Dim objstudPer As New StuddedDeDuctPer
    Dim Tagsave As Boolean = False

    Dim tagWebtagDiff As Boolean = False
    Dim MLasttagno As Integer
    Dim MTagprefix As String

    Private Property pFixedVa() As Boolean
        Get
            Return FixedVa
        End Get
        Set(ByVal value As Boolean)
            FixedVa = value
        End Set
    End Property


    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub


    Public Sub New(ByVal WISSsno As String)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.BackColor = frmBackColor
        Me.BackgroundImage = bakImage
        Me.BackgroundImageLayout = ImageLayout.Stretch
        Me.StartPosition = FormStartPosition.CenterScreen
        BrighttechPack.LanguageChange.Set_Language_Form(Me, LangId)
        objGPack.Validator_Object(Me)
        Me.MdiParent = Main

        If objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'MULTIMETALCALC'") = "Y" Then
            multiMetalCalc = True
        End If
        tagEdit = True
        WupdIssSno = WISSsno
        'updIssSno = ISSsno
        ' Add any initialization after the InitializeComponent() call.
        pnlSearch.Location = New Point(804, 16)
        pnlSearch.Size = New Size(209, 394)
        Me.Controls.Add(pnlSearch)
        pnlSearch.BringToFront()
        txtNarration.CharacterCasing = CharacterCasing.Normal
        'Me.WindowState = FormWindowState.Maximized
        pnlMain.BorderStyle = BorderStyle.None
        pnlMultiMetal.BackColor = Drawing.Color.LightGoldenrodYellow
        pnlStoneGrid.BackColor = Drawing.Color.LightGoldenrodYellow
        pnlMisc.BackColor = Drawing.Color.LightGoldenrodYellow

        'TabControl1.SelectTab(tabTag)
        TabGeneral.SelectTab(tabWebTag)
        Me.TabControl1_SelectedIndexChanged(Me, New EventArgs)
        'TabControl1.SelectedTab.Name = tabTag.Name

        ''dtGridView
        gridView.BorderStyle = BorderStyle.None

        'for Web tag
        With dtWGridView.Columns
            .Add("ITEM", GetType(String))
            .Add("SUBITEM", GetType(String))
            .Add("TAGNO", GetType(String))
            .Add("PCS", GetType(Int16))
            .Add("GRSWT", GetType(Decimal))
            .Add("LESSWT", GetType(Decimal))
            .Add("NETWT", GetType(Decimal))
            .Add("RATE", GetType(Decimal))
            .Add("SALEVALUE", GetType(Decimal))
            .Add("SIZE", GetType(String))
        End With
        wgridView.DataSource = dtWGridView
        With wgridView
            .Columns("ITEM").Width = 150
            .Columns("SUBITEM").Width = 120
            .Columns("TAGNO").Width = 60
            .Columns("PCS").Width = 60
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").Width = 70
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("LESSWT").Width = 70
            .Columns("LESSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETWT").Width = 70
            .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RATE").Width = 70
            .Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SALEVALUE").Width = 100
            .Columns("SALEVALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SALEVALUE").HeaderText = "SALE VALUE"
            .Columns("SIZE").Width = 200
        End With
        wgridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells

        'for tag
        With dtGridView.Columns
            ''SUBITEM,ITEMSIZE,TAGNO,PIECES,GRSWEIGHT,LESSWEIGHT,NETWT,RATE,CALCMODE,TABLECODE
            .Add("ITEM", GetType(String))
            .Add("SUBITEM", GetType(String))
            .Add("TAGNO", GetType(String))
            .Add("PCS", GetType(Int16))
            .Add("GRSWT", GetType(Decimal))
            .Add("LESSWT", GetType(Decimal))
            .Add("NETWT", GetType(Decimal))
            .Add("RATE", GetType(Decimal))
            .Add("SALEVALUE", GetType(Decimal))
            .Add("SIZE", GetType(String))
        End With
        gridView.DataSource = dtGridView
        With gridView
            .Columns("ITEM").Width = 150
            .Columns("SUBITEM").Width = 120
            .Columns("TAGNO").Width = 60
            .Columns("PCS").Width = 60
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").Width = 70
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("LESSWT").Width = 70
            .Columns("LESSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETWT").Width = 70
            .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RATE").Width = 70
            .Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SALEVALUE").Width = 100
            .Columns("SALEVALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SALEVALUE").HeaderText = "SALE VALUE"
            .Columns("SIZE").Width = 200
        End With
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells

        ''Web MultiMetal
        With dtWMultiMetalDetails.Columns
            .Add("CATEGORY", GetType(String))
            .Add("RATE", GetType(Double))
            .Add("WEIGHT", GetType(Double))
            .Add("WASTAGEPER", GetType(Double))
            .Add("WASTAGE", GetType(Double))
            .Add("MCPERGRM", GetType(Double))
            .Add("MC", GetType(Double))
            .Add("AMOUNT", GetType(Double))
            .Add("PURWASTAGEPER", GetType(Double))
            .Add("PURWASTAGE", GetType(Double))
            .Add("PURMCPERGRM", GetType(Double))
            .Add("PURMC", GetType(Double))
            .Add("PURRATE", GetType(Double))
            .Add("PURAMOUNT", GetType(Double))
            .Add("STNSNO", GetType(String))
            .Add("KEYNO", GetType(Integer))
        End With
        dtWMultiMetalDetails.Columns("KEYNO").AutoIncrement = True
        dtWMultiMetalDetails.Columns("KEYNO").AutoIncrementStep = 1
        dtWMultiMetalDetails.Columns("KEYNO").AutoIncrementSeed = 1
        wgridMultimetal.DataSource = dtWMultiMetalDetails
        WStyleGridMultiMetal()

        Dim dtWMultiMetalTotal As New DataTable
        dtWMultiMetalTotal = dtWMultiMetalDetails.Copy
        dtWMultiMetalTotal.Rows.Add()
        dtWMultiMetalTotal.Rows(0).Item("CATEGORY") = "TOTAL"
        dtWMultiMetalTotal.AcceptChanges()
        With wgridMultiMetalTotal
            .DataSource = dtWMultiMetalTotal
            .Columns("CATEGORY").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            For cnt As Integer = 0 To wgridMultimetal.ColumnCount - 1
                .Columns(cnt).Width = wgridMultimetal.Columns(cnt).Width
                .Columns(cnt).Visible = wgridMultimetal.Columns(cnt).Visible
                .Columns(cnt).DefaultCellStyle = wgridMultimetal.Columns(cnt).DefaultCellStyle
                .Columns(cnt).ReadOnly = True
            Next
        End With

        ''MultiMetal
        With dtMultiMetalDetails.Columns
            .Add("CATEGORY", GetType(String))
            .Add("RATE", GetType(Double))
            .Add("WEIGHT", GetType(Double))
            .Add("WASTAGEPER", GetType(Double))
            .Add("WASTAGE", GetType(Double))
            .Add("MCPERGRM", GetType(Double))
            .Add("MC", GetType(Double))
            .Add("AMOUNT", GetType(Double))
            .Add("PURWASTAGEPER", GetType(Double))
            .Add("PURWASTAGE", GetType(Double))
            .Add("PURMCPERGRM", GetType(Double))
            .Add("PURMC", GetType(Double))
            .Add("PURRATE", GetType(Double))
            .Add("PURAMOUNT", GetType(Double))
            .Add("STNSNO", GetType(String))
            .Add("KEYNO", GetType(Integer))
        End With
        dtMultiMetalDetails.Columns("KEYNO").AutoIncrement = True
        dtMultiMetalDetails.Columns("KEYNO").AutoIncrementStep = 1
        dtMultiMetalDetails.Columns("KEYNO").AutoIncrementSeed = 1
        gridMultimetal.DataSource = dtMultiMetalDetails
        StyleGridMultiMetal()

        Dim dtMultiMetalTotal As New DataTable
        dtMultiMetalTotal = dtMultiMetalDetails.Copy
        dtMultiMetalTotal.Rows.Add()
        dtMultiMetalTotal.Rows(0).Item("CATEGORY") = "TOTAL"
        dtMultiMetalTotal.AcceptChanges()
        With gridMultiMetalTotal
            .DataSource = dtMultiMetalTotal
            .Columns("CATEGORY").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            For cnt As Integer = 0 To gridMultimetal.ColumnCount - 1
                .Columns(cnt).Width = gridMultimetal.Columns(cnt).Width
                .Columns(cnt).Visible = gridMultimetal.Columns(cnt).Visible
                .Columns(cnt).DefaultCellStyle = gridMultimetal.Columns(cnt).DefaultCellStyle
                .Columns(cnt).ReadOnly = True
            Next
        End With

        ''Web Stone
        With dtWStoneDetails.Columns
            .Add("PACKETNO", GetType(String))
            .Add("ITEM", GetType(String))
            .Add("SUBITEM", GetType(String))
            .Add("PCS", GetType(Int32))
            .Add("WEIGHT", GetType(Decimal))
            .Add("UNIT", GetType(String))
            .Add("CALC", GetType(String))
            .Add("RATE", GetType(Decimal))
            .Add("AMOUNT", GetType(Decimal))
            .Add("METALID", GetType(String))
            .Add("PURRATE", GetType(Decimal))
            .Add("PURVALUE", GetType(Decimal))
            .Add("KEYNO", GetType(Integer))
            .Add("STNSNO", GetType(String))
            .Add("USRATE", GetType(Decimal))
            .Add("INDRS", GetType(Decimal))
            .Add("CUT", GetType(String))
            .Add("COLOR", GetType(String))
            .Add("CLARITY", GetType(String))
            .Add("SHAPE", GetType(String))
            .Add("SETTYPE", GetType(String))
            .Add("HEIGHT", GetType(Decimal))
            .Add("WIDTH", GetType(Decimal))
        End With
        dtWStoneDetails.Columns("KEYNO").AutoIncrement = True
        dtWStoneDetails.Columns("KEYNO").AutoIncrementStep = 1
        dtWStoneDetails.Columns("KEYNO").AutoIncrementSeed = 1
        wgridStone.DataSource = dtWStoneDetails
        wgridStone.Columns("USRATE").Visible = False
        wgridStone.Columns("INDRS").Visible = False
        wgridStone.Columns("CUT").Visible = False
        wgridStone.Columns("COLOR").Visible = False
        wgridStone.Columns("CLARITY").Visible = False
        wgridStone.Columns("SHAPE").Visible = False
        wgridStone.Columns("SETTYPE").Visible = False
        wgridStone.Columns("HEIGHT").Visible = False
        wgridStone.Columns("WIDTH").Visible = False
        WStyleGridStone()
        dtWStoneDetails.AcceptChanges()
        Dim dtWStoneFooter As New DataTable
        dtWStoneFooter = dtWStoneDetails.Copy
        dtWStoneFooter.Rows.Clear()
        dtWStoneFooter.Rows.Add()
        dtWStoneFooter.AcceptChanges()
        wgridStoneFooter.DataSource = dtWStoneFooter
        With wgridStoneFooter
            .DefaultCellStyle.BackColor = Drawing.Color.Gainsboro
            .DefaultCellStyle.SelectionBackColor = Drawing.Color.Gainsboro
            .Rows(0).Cells("SUBITEM").Value = "TOTAL"
            For cnt As Integer = 0 To wgridStone.ColumnCount - 1
                .Columns(cnt).Width = wgridStone.Columns(cnt).Width
                .Columns(cnt).Visible = wgridStone.Columns(cnt).Visible
                .Columns(cnt).DefaultCellStyle = wgridStone.Columns(cnt).DefaultCellStyle
            Next
        End With

        ''Stone
        With dtStoneDetails.Columns
            .Add("PACKETNO", GetType(String))
            .Add("ITEM", GetType(String))
            .Add("SUBITEM", GetType(String))
            .Add("PCS", GetType(Int32))
            .Add("WEIGHT", GetType(Decimal))
            .Add("UNIT", GetType(String))
            .Add("CALC", GetType(String))
            .Add("RATE", GetType(Decimal))
            .Add("AMOUNT", GetType(Decimal))
            .Add("METALID", GetType(String))
            .Add("PURRATE", GetType(Decimal))
            .Add("PURVALUE", GetType(Decimal))
            .Add("KEYNO", GetType(Integer))
            .Add("STNSNO", GetType(String))
            .Add("USRATE", GetType(Decimal))
            .Add("INDRS", GetType(Decimal))
            .Add("CUT", GetType(String))
            .Add("COLOR", GetType(String))
            .Add("CLARITY", GetType(String))
            .Add("SHAPE", GetType(String))
            .Add("SETTYPE", GetType(String))
            .Add("HEIGHT", GetType(Decimal))
            .Add("WIDTH", GetType(Decimal))
        End With
        dtStoneDetails.Columns("KEYNO").AutoIncrement = True
        dtStoneDetails.Columns("KEYNO").AutoIncrementStep = 1
        dtStoneDetails.Columns("KEYNO").AutoIncrementSeed = 1
        gridStone.DataSource = dtStoneDetails
        gridStone.Columns("USRATE").Visible = False
        gridStone.Columns("INDRS").Visible = False
        gridStone.Columns("CUT").Visible = False
        gridStone.Columns("COLOR").Visible = False
        gridStone.Columns("CLARITY").Visible = False
        gridStone.Columns("SHAPE").Visible = False
        gridStone.Columns("SETTYPE").Visible = False
        gridStone.Columns("HEIGHT").Visible = False
        gridStone.Columns("WIDTH").Visible = False
        StyleGridStone()
        dtStoneDetails.AcceptChanges()
        Dim dtStoneFooter As New DataTable
        dtStoneFooter = dtStoneDetails.Copy
        dtStoneFooter.Rows.Clear()
        dtStoneFooter.Rows.Add()
        dtStoneDetails.AcceptChanges()
        gridStoneFooter.DataSource = dtStoneFooter
        With gridStoneFooter
            .DefaultCellStyle.BackColor = Drawing.Color.Gainsboro
            .DefaultCellStyle.SelectionBackColor = Drawing.Color.Gainsboro
            .Rows(0).Cells("SUBITEM").Value = "TOTAL"
            For cnt As Integer = 0 To gridStone.ColumnCount - 1
                .Columns(cnt).Width = gridStone.Columns(cnt).Width
                .Columns(cnt).Visible = gridStone.Columns(cnt).Visible
                .Columns(cnt).DefaultCellStyle = gridStone.Columns(cnt).DefaultCellStyle
            Next
        End With


        ''Web OtherCharges
        With dtWMiscDetails.Columns
            .Add("MISC", GetType(String))
            .Add("AMOUNT", GetType(Double))
            .Add("PURAMOUNT", GetType(Double))
            .Add("KEYNO", GetType(Integer))
        End With
        dtWMiscDetails.Columns("KEYNO").AutoIncrement = True
        dtWMiscDetails.Columns("KEYNO").AutoIncrementStep = 1
        dtWMiscDetails.Columns("KEYNO").AutoIncrementSeed = 1
        wgridMisc.DataSource = dtWMiscDetails
        WStyleGridMisc()
        Dim dtWMiscFooter As New DataTable
        dtWMiscFooter = dtWMiscDetails.Copy
        dtWMiscFooter.Rows.Clear()
        dtWMiscFooter.Rows.Add()
        dtWMiscFooter.Rows(0).Item("MISC") = "TOTAL"
        dtWMiscFooter.AcceptChanges()
        wgridMiscFooter.DataSource = dtWMiscFooter
        With wgridMiscFooter
            For cnt As Integer = 0 To wgridMisc.ColumnCount - 1
                .Columns(cnt).Width = wgridMisc.Columns(cnt).Width
                .Columns(cnt).Visible = wgridMisc.Columns(cnt).Visible
                .Columns(cnt).DefaultCellStyle = wgridMisc.Columns(cnt).DefaultCellStyle
            Next
            .DefaultCellStyle.BackColor = Drawing.Color.Gainsboro
            .DefaultCellStyle.SelectionBackColor = Drawing.Color.Gainsboro
        End With


        ''OtherCharges
        With dtMiscDetails.Columns
            .Add("MISC", GetType(String))
            .Add("AMOUNT", GetType(Double))
            .Add("PURAMOUNT", GetType(Double))
            .Add("KEYNO", GetType(Integer))
        End With
        dtMiscDetails.Columns("KEYNO").AutoIncrement = True
        dtMiscDetails.Columns("KEYNO").AutoIncrementStep = 1
        dtMiscDetails.Columns("KEYNO").AutoIncrementSeed = 1
        gridMisc.DataSource = dtMiscDetails
        StyleGridMisc()
        Dim dtMiscFooter As New DataTable
        dtMiscFooter = dtMiscDetails.Copy
        dtMiscFooter.Rows.Clear()
        dtMiscFooter.Rows.Add()
        dtMiscFooter.Rows(0).Item("MISC") = "TOTAL"
        dtMiscDetails.AcceptChanges()
        gridMiscFooter.DataSource = dtMiscFooter
        With gridMiscFooter
            For cnt As Integer = 0 To gridMisc.ColumnCount - 1
                .Columns(cnt).Width = gridMisc.Columns(cnt).Width
                .Columns(cnt).Visible = gridMisc.Columns(cnt).Visible
                .Columns(cnt).DefaultCellStyle = gridMisc.Columns(cnt).DefaultCellStyle
            Next
            .DefaultCellStyle.BackColor = Drawing.Color.Gainsboro
            .DefaultCellStyle.SelectionBackColor = Drawing.Color.Gainsboro
        End With

        'Additional Entry
        With DtAddStockEntry.Columns
            .Add("OTHID", GetType(Integer))
            .Add("OTHNAME", GetType(String))
            .Add("MISCID", GetType(String))
            .Add("KEYNO", GetType(Integer))
        End With
        DtAddStockEntry.Columns("KEYNO").AutoIncrement = True
        DtAddStockEntry.Columns("KEYNO").AutoIncrementStep = 1
        DtAddStockEntry.Columns("KEYNO").AutoIncrementSeed = 1
        AddStockEntry = True


        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ISNULL(STUDDED,'') <> 'S' AND ACTIVE = 'Y' AND STOCKTYPE = 'Y'"
        strSql += GetItemQryFilteration("S")
        strSql += " order by ITEMNAME"
        objGPack.FillCombo(strSql, cmbItem_MAN, , False)
        objGPack.FillCombo(strSql, wcmbItem_MAN, , False)

        'for webtag 
        wcmbStUnit.Items.Add("C")
        wcmbStUnit.Items.Add("G")
        wcmbStUnit.Text = "C"
        wcmbStCalc.Items.Add("W")
        wcmbStCalc.Items.Add("P")
        wcmbStCalc.Text = "W"

        'for tag 
        cmbStUnit.Items.Add("C")
        cmbStUnit.Items.Add("G")
        cmbStUnit.Text = "C"
        cmbStCalc.Items.Add("W")
        cmbStCalc.Items.Add("P")
        cmbStCalc.Text = "W"


        ''CostCentre Checking..
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE'"
        If UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", "Y", tran)) = "N" Then
            cmbCostCentre_Man.Items.Clear()
            cmbCostCentre_Man.Text = ""
            cmbCostCentre_Man.Enabled = False
        End If

        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'ATTACHIMAGE'"
        If UCase(objGPack.GetSqlValue(strSql, , , tran)) = "Y" Then
            btnAttachImage.Enabled = True
        Else
            btnAttachImage.Enabled = False
        End If

        ' ''Min McWastage Visibility
        'strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'MINMCTAB'"
        'If UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", "Y", tran)) = "Y" Then pnlMin.Enabled = True Else pnlMin.Enabled = False

        ' ''Set MaxWastage Focus
        'strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'MAXMCFOCUS'"
        'If UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", "Y", tran)) = "Y" Then pnlValueAdded.Enabled = True Else pnlValueAdded.Enabled = False

        ''Set AttachImag btn Visible
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'ATTACHIMAGE'"
        If UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", "Y", tran)) = "Y" Then btnAttachImage.Enabled = True Else btnAttachImage.Enabled = False

        ''Sets default Paths
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICPATH'"
        defalutDestination = UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", , tran))
        If Not defalutDestination.EndsWith("\") And defalutDestination <> "" Then defalutDestination += "\"
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICSOURCEPATH'"
        defalutSourcePath = UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", , tran))

        ''TagNo Gen
        Dim ds As New Data.DataSet
        ds.Clear()
        strSql = " SELECT CTLTEXT  FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'TAGNOGEN'"
        TagNoGen = objGPack.GetSqlValue(strSql, "CTLTEXT", , tran)

        ''CALMODE //WHEATHER GRSWT BASE OR NETWT BASE
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'CALCMODE'"
        CalMode = objGPack.GetSqlValue(strSql, "CTLTEXT", , tran)

        cmbCalcMode.Items.Add("GRS WT")
        cmbCalcMode.Items.Add("NET WT")

        If funcNew() = 0 Then Exit Sub
        If WfuncNew() = 0 Then Exit Sub
        'for webtag edit
        strSql = " SELECT (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEMNAME"
        strSql += " ,DESCRIP,(SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = T.ITEMTYPEID)AS ITEMTYPE"
        strSql += " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)AS DESIGNER"
        strSql += " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = T.COSTID)AS COSTCENTRE"
        strSql += " ,T.RECDATE,T.PURITY"
        strSql += " ,ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID),'')AS SUBITEM"
        strSql += " ,ISNULL((SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = T.ITEMTYPEID),'')AS ITEMTYPE"
        strSql += " ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID)AS ITEMCOUNTER"
        strSql += " ,(SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZEID = T.SIZEID)AS ITEMSIZE"
        strSql += " ,T.TAGNO,T.PCS,T.GRSWT,T.LESSWT,T.NETWT,T.RATE,T.GRSNET,T.TABLECODE,T.STYLENO"
        strSql += " ,T.MAXWASTPER,T.MAXWAST,T.MAXMCGRM,T.MAXMC,T.MINWASTPER,T.MINWAST,T.MINMCGRM,T.MINMC"
        strSql += " ,T.SALVALUE,T.NARRATION,T.PCTFILE"
        strSql += " ,T.BOARDRATE,T.ORSNO,T.RFID,T.SALEMODE,T.TOUCH"
        strSql += " ,T.HM_BILLNO,T.HM_CENTER,T.ADD_VA_PER,T.REFVALUE,T.ORDREPNO,T.EXTRAWT,T.USRATE,T.INDRS"
        If _FourCMaintain Then
            strSql += " ,T.CUTID,T.COLORID,T.CLARITYID,T.SHAPEID,T.SETTYPEID"
        End If
        strSql += " ,T.HEIGHT,T.WIDTH"
        strSql += "  ," & cnAdminDb & ".DBO.FUNCSPECIFIC(T.SNO,CASE WHEN ISNULL(T.REASON,'')='' THEN CAST(T.SUBITEMID AS VARCHAR)+',' ELSE T.REASON END,'M') AS REASON"
        strSql += " FROM " & cnAdminDb & "..WITEMTAG AS T "
        strSql += " WHERE T.SNO = '" & WupdIssSno & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtWTagDetails)
        If Not dtWTagDetails.Rows.Count > 0 Then Me.Dispose()
        With dtWTagDetails.Rows(0)

            updIssSno = objGPack.GetSqlValue("SELECT ISNULL(SNO,'') FROM " & cnAdminDb & "..ITEMTAG WHERE RECSNO = '" & WupdIssSno & "'", , "")
            wcmbItem_MAN.Text = .Item("ITEMNAME").ToString
            pFixedVa = IIf(objGPack.GetSqlValue("SELECT ISNULL(FIXEDVA,'N') FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wcmbItem_MAN.Text & "'", , "N") = "N", False, True)
            Dim xItemid As Integer
            strSql = " SELECT ITEMID,METALID,STUDDEDSTONE,SIZESTOCK,MULTIMETAL,OTHCHARGE,CALTYPE,NOOFPIECE,PIECERATE,VALUEADDEDTYPE,GROSSNETWTDIFF"
            strSql += " ,STUDDED,STOCKTYPE,TableCode,MCASVAPER FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Item("ITEMNAME").ToString & "'"
            Dim dtWItemDetail As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtWItemDetail)
            If dtWItemDetail.Rows.Count > 0 Then
                With dtWItemDetail.Rows(0)
                    xItemid = Val(.Item("ITEMID").ToString)
                    METALID = .Item("METALID").ToString
                    studdedStone = .Item("STUDDEDSTONE").ToString
                    grossnetdiff = .Item("GROSSNETWTDIFF").ToString
                    sizeStock = .Item("SIZESTOCK").ToString
                    multiMetal = .Item("MULTIMETAL").ToString
                    OthCharge = .Item("OTHCHARGE").ToString
                    noOfPiece = Val(.Item("NOOFPIECE").ToString)
                    pieceRate = Val(.Item("PIECERATE").ToString)
                    calType = .Item("CALTYPE").ToString
                    'If .Item("MCASVAPER").ToString = "Y" Then Label44.Text = "MC PERCENT" Else Label44.Text = "Max Mc Per Gram"
                    WcmbSubItem_OWN.Enabled = True
                    strSql = GetSubItemQry(New String() {"SUBITEMNAME"}, xItemid)
                    objGPack.FillCombo(strSql, WcmbSubItem_OWN)
                    Dim dtSubItem As New DataTable
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(dtSubItem)
                    BrighttechPack.GlobalMethods.FillCombo(WchkcmbSubItem, dtSubItem, "SUBITEMNAME")
                    WchkcmbSubItem.Enabled = True
                    If Not WcmbSubItem_OWN.Items.Count > 0 Then
                        WcmbSubItem_OWN.Enabled = False
                    Else
                        WcmbSubItem_OWN.Enabled = True
                        WcmbSubItem_OWN.Text = ""
                    End If
                    If WcmbSubItem_OWN.Text <> "" Then
                        Dim subdr As DataRow = GetSqlRow("SELECT ISNULL(FIXEDVA,'N'),CALTYPE,NOOFPIECE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & WcmbSubItem_OWN.Text & "' AND ITEMID =" & xItemid & "", cn, tran)
                        pFixedVa = subdr.Item(0).ToString
                        calType = subdr.Item(1).ToString
                        noOfPiece = Val(subdr.Item(2).ToString)
                    End If
                    wfuncAssignTabControls()
                End With
            End If
            Dim lstSubItem As New List(Of String)
            Dim strsub() As String
            strsub = dtWTagDetails.Rows(0).Item("REASON").ToString.Split(":")
            If strsub.Length > 0 Then
                For I As Integer = 0 To strsub.Length - 1
                    lstSubItem.Add(strsub(I).ToString)
                Next
                SetChecked_CheckedList(WchkcmbSubItem, lstSubItem, "")
            End If

            wdtpRecieptDate.MinimumDate = (New DateTimePicker).MinDate
            wdtpRecieptDate.MaximumDate = (New DateTimePicker).MaxDate

            'txtLotNo_Num_Man.Enabled = False
            wcmbItem_MAN.Text = .Item("ITEMNAME").ToString
            wtxtNameOfProduct_OWN.Text = .Item("DESCRIP").ToString
            pFixedVa = IIf(objGPack.GetSqlValue("SELECT ISNULL(FIXEDVA,'N') FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wcmbItem_MAN.Text & "'", , "N") = "N", False, True)
            wcmbItem_MAN.Enabled = False
            wcmbPurity.Text = .Item("ITEMTYPE").ToString
            wdtpRecieptDate.Value = .Item("RECDATE")
            wtxtMetalRate_Amt.Text = .Item("BOARDRATE").ToString ' GetMetalRate()
            WcmbSubItem_OWN.Text = .Item("SUBITEM").ToString
            wtxtTagNo__Man.Text = .Item("TAGNO").ToString
            wtxtTagNo__Man.Enabled = False
            wtxtPieces_Num_Man.Text = .Item("PCS").ToString
            '328
            noOfPiece = Val(.Item("PCS").ToString)
            '328
            WtxtGrossWt_Wet.Text = .Item("GRSWT").ToString
            WtxtLessWt_Wet.Text = .Item("LESSWT").ToString
            WtxtNetWt_Wet.Text = .Item("NETWT").ToString
            wtxtMcPerGrm_Amt.Text = IIf(Val(.Item("MAXMCGRM").ToString) <> 0, .Item("MAXMCGRM").ToString, Nothing)
            wtxtMkCharge_Amt.Text = IIf(Val(.Item("MAXMC").ToString) <> 0, .Item("MAXMC").ToString, Nothing)
            wtxtNarration.Text = .Item("NARRATION").ToString
            wtxtWidth_Wet.Text = Val(.Item("WIDTH").ToString)
            wtxtHght_Wet.Text = Val(.Item("HEIGHT").ToString)


            ''STONEDETAILS
            strSql = " SELECT ISNULL(PACKETNO,'') PACKETNO,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = STNITEMID)AS ITEM"
            strSql += " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = STNSUBITEMID)AS SUBITEM"
            strSql += " ,STNPCS PCS,STNWT WEIGHT"
            strSql += " ,STONEUNIT UNIT,CALCMODE CALC,STNRATE RATE,STNAMT AMOUNT"
            strSql += " ,(SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) AS METALID"
            strSql += " ,NULL PURRATE"
            strSql += " ,NULL PURVALUE,SNO STNSNO,USRATE,INDRS"
            If _FourCMaintain Then
                strSql += " ,(SELECT CUTNAME FROM " & cnAdminDb & "..STNCUT WHERE CUTID = S.CUTID)AS CUT"
                strSql += " ,(SELECT COLORNAME FROM " & cnAdminDb & "..STNCOLOR WHERE COLORID = S.COLORID)AS COLOR"
                strSql += " ,(SELECT CLARITYNAME FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYID = S.CLARITYID)AS CLARITY"
                strSql += " ,(SELECT SHAPENAME FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPEID = S.SHAPEID)AS SHAPE"
                strSql += " ,(SELECT SETTYPENAME FROM " & cnAdminDb & "..STNSETTYPE WHERE SETTYPEID = S.SETTYPEID)AS SETTYPE"
                strSql += " ,S.HEIGHT,S.WIDTH"
            End If
            strSql += " FROM " & cnAdminDb & "..WITEMTAGSTONE S"
            strSql += " WHERE TAGSNO = '" & WupdIssSno & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtWStoneDetails)
            dtWStoneDetails.AcceptChanges()
            WStyleGridStone()
            WCalcLessWt()
            WCalcFinalTotal()

            'Additional stock Entry
            strSql = "SELECT OTHID,O.NAME AS OTHNAME,O.MISCID FROM " & cnAdminDb & "..WADDINFOITEMTAG A"
            strSql += " LEFT JOIN " & cnAdminDb & "..OTHERMASTER O ON A.OTHID=O.ID"
            strSql += " WHERE A.TAGSNO = '" & WupdIssSno & "'"
            Dim WDtAddStockEntry As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(WDtAddStockEntry)
            Dim LstAdd1 As New List(Of String)
            Dim LstAdd2 As New List(Of String)
            Dim LstAdd3 As New List(Of String)
            Dim LstAdd4 As New List(Of String)
            Dim LstAdd5 As New List(Of String)
            Dim LstAdd6 As New List(Of String)
            Dim LstAdd7 As New List(Of String)
            Dim LstAdd8 As New List(Of String)
            Dim LstAdd9 As New List(Of String)
            Dim LstAdd10 As New List(Of String)
            Dim LstAdd11 As New List(Of String)
            Dim LstAdd12 As New List(Of String)
            For V As Integer = 0 To WDtAddStockEntry.Rows.Count - 1
                If V = 0 Then wcmbAddM1_OWN.Text = WDtAddStockEntry.Rows(V).Item("OTHNAME").ToString
                If V = 1 Then wcmbAddM2_OWN.Text = WDtAddStockEntry.Rows(V).Item("OTHNAME").ToString
                If Val(WDtAddStockEntry.Rows(V).Item("MISCID").ToString) = 1 Then
                    LstAdd1.Add(WDtAddStockEntry.Rows(V).Item("OTHNAME").ToString)
                End If
                If Val(WDtAddStockEntry.Rows(V).Item("MISCID").ToString) = 2 Then
                    LstAdd2.Add(WDtAddStockEntry.Rows(V).Item("OTHNAME").ToString)
                End If
                If Val(WDtAddStockEntry.Rows(V).Item("MISCID").ToString) = 3 Then
                    LstAdd3.Add(WDtAddStockEntry.Rows(V).Item("OTHNAME").ToString)
                End If
                If Val(WDtAddStockEntry.Rows(V).Item("MISCID").ToString) = 4 Then
                    LstAdd4.Add(WDtAddStockEntry.Rows(V).Item("OTHNAME").ToString)
                End If
                If Val(WDtAddStockEntry.Rows(V).Item("MISCID").ToString) = 5 Then
                    LstAdd5.Add(WDtAddStockEntry.Rows(V).Item("OTHNAME").ToString)
                End If
                If Val(WDtAddStockEntry.Rows(V).Item("MISCID").ToString) = 6 Then
                    LstAdd6.Add(WDtAddStockEntry.Rows(V).Item("OTHNAME").ToString)
                End If
                If Val(WDtAddStockEntry.Rows(V).Item("MISCID").ToString) = 7 Then
                    LstAdd7.Add(WDtAddStockEntry.Rows(V).Item("OTHNAME").ToString)
                End If
                If Val(WDtAddStockEntry.Rows(V).Item("MISCID").ToString) = 8 Then
                    LstAdd8.Add(WDtAddStockEntry.Rows(V).Item("OTHNAME").ToString)
                End If
                If Val(WDtAddStockEntry.Rows(V).Item("MISCID").ToString) = 9 Then
                    LstAdd9.Add(WDtAddStockEntry.Rows(V).Item("OTHNAME").ToString)
                End If
                If Val(WDtAddStockEntry.Rows(V).Item("MISCID").ToString) = 10 Then
                    LstAdd10.Add(WDtAddStockEntry.Rows(V).Item("OTHNAME").ToString)
                End If
                If Val(WDtAddStockEntry.Rows(V).Item("MISCID").ToString) = 11 Then
                    LstAdd11.Add(WDtAddStockEntry.Rows(V).Item("OTHNAME").ToString)
                End If
                If Val(WDtAddStockEntry.Rows(V).Item("MISCID").ToString) = 12 Then
                    LstAdd12.Add(WDtAddStockEntry.Rows(V).Item("OTHNAME").ToString)
                End If
            Next
            SetChecked_CheckedList(wChkcmbAddM1, LstAdd1, "")
            SetChecked_CheckedList(wChkcmbAddM2, LstAdd2, "")
            SetChecked_CheckedList(wChkcmbAddM3, LstAdd3, "")
            SetChecked_CheckedList(wChkcmbAddM4, LstAdd4, "")
            SetChecked_CheckedList(wChkcmbAddM5, LstAdd5, "")
            SetChecked_CheckedList(wChkcmbAddM6, LstAdd6, "")
            SetChecked_CheckedList(wChkcmbAddM7, LstAdd7, "")
            SetChecked_CheckedList(wChkcmbAddM8, LstAdd8, "")
            SetChecked_CheckedList(wChkcmbAddM9, LstAdd9, "")
            SetChecked_CheckedList(wChkcmbAddM10, LstAdd10, "")
            SetChecked_CheckedList(wChkcmbAddM11, LstAdd11, "")
            SetChecked_CheckedList(wChkcmbAddM12, LstAdd12, "")

            wtxtSalValue_Amt_Man.Text = IIf(Val(.Item("SALVALUE").ToString) <> 0, SALEVALUE_ROUND(Val(.Item("SALVALUE").ToString)), Nothing)
            If SALEVALUEPLUS <> 0 Then wtxtSalValue_Amt_Man.Text = Val(wtxtSalValue_Amt_Man.Text) * SALEVALUEPLUS

            If UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'TAGEDITB4DATE'", , "N")) = "N" Then
                wdtpRecieptDate.Enabled = False
            End If
            If UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'TAGEDITPCSWT'", , "N")) = "N" Then
                wtxtPieces_Num_Man.ReadOnly = True
                WtxtGrossWt_Wet.ReadOnly = True
                WtxtNetWt_Wet.ReadOnly = True
            End If
            If TAGEDITDISABLE <> "" Then
                If TAGEDITDISABLE.Contains("SI") Then WcmbSubItem_OWN.Enabled = False
                If TAGEDITDISABLE.Contains("IT") Then wcmbPurity.Enabled = False
                If TAGEDITDISABLE.Contains("SD") Then wgridStone.Enabled = False : wpnlStDet.Enabled = False
                If TAGEDITDISABLE.Contains("SV") Then wtxtSalValue_Amt_Man.Enabled = False
                If TAGEDITDISABLE.Contains("MC") Then wtxtMcPerGrm_Amt.ReadOnly = True : wtxtMkCharge_Amt.ReadOnly = True
                If TAGEDITDISABLE.Contains("RT") Then wtxtMetalRate_Amt.ReadOnly = True
                If TAGEDITDISABLE.Contains("DT") Then wdtpRecieptDate.Enabled = False
                If TAGEDITDISABLE.Contains("NR") Then wtxtNarration.ReadOnly = True
            End If
            If objGPack.DupCheck("SELECT 1 FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wcmbItem_MAN.Text & "' AND STYLENO = 'Y'") Then
                isStyleCode = True
            End If
            WbtnNew.Enabled = False
            If _FourCMaintain Then
                strSql = "SELECT ISNULL(MAINTAIN4C,'N') FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "' AND METALID='D'"
                If objGPack.GetSqlValue(strSql).ToString.ToUpper = "N" Then Exit Sub
                strSql = "SELECT STUDDED FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "' AND METALID='D'"
                If objGPack.GetSqlValue(strSql).ToString.ToUpper = "L" Then
                    WTagCutId = Val(dtTagDetails.Rows(0).Item("CUTID").ToString)
                    WTagColorId = Val(dtTagDetails.Rows(0).Item("COLORID").ToString)
                    WTagClarityId = Val(dtTagDetails.Rows(0).Item("CLARITYID").ToString)
                    WTagShapeId = Val(dtTagDetails.Rows(0).Item("SHAPEID").ToString)
                    WTagSetTypeId = Val(dtTagDetails.Rows(0).Item("SETTYPEID").ToString)
                    ObjDiaDetails = New frmDiamondDetails
                    ObjDiaDetails.CmbCut.Text = objGPack.GetSqlValue(" SELECT CUTNAME FROM " & cnAdminDb & "..STNCUT WHERE CUTID = '" & WTagCutId & "'", "CUTNAME", , tran)
                    ObjDiaDetails.CmbColor.Text = objGPack.GetSqlValue(" SELECT COLORNAME FROM " & cnAdminDb & "..STNCOLOR WHERE COLORID = '" & WTagColorId & "'", "COLORNAME", , tran)
                    ObjDiaDetails.CmbClarity.Text = objGPack.GetSqlValue(" SELECT CLARITYNAME FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYID = '" & WTagClarityId & "'", "CLARITYNAME", , tran)
                    ObjDiaDetails.cmbShape.Text = objGPack.GetSqlValue(" SELECT SHAPENAME FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPEID = '" & WTagShapeId & "'", "SHAPENAME", , tran)
                    ObjDiaDetails.cmbSetType.Text = objGPack.GetSqlValue(" SELECT SETTYPENAME FROM " & cnAdminDb & "..STNSETTYPE WHERE SETTYPEID = '" & WTagSetTypeId & "'", "SETTYPENAME", , tran)
                    ObjDiaDetails.txtWidth_WET.Text = Val(dtTagDetails.Rows(0).Item("WIDTH").ToString)
                    ObjDiaDetails.txtHeight_WET.Text = Val(dtTagDetails.Rows(0).Item("HEIGHT").ToString)
                End If
            End If
        End With

        'for tag edit
        strSql = " SELECT (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEMNAME"
        strSql += " ,(SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = T.ITEMTYPEID)AS ITEMTYPE"
        strSql += " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)AS DESIGNER"
        strSql += " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = T.COSTID)AS COSTCENTRE"
        strSql += " ,T.RECDATE,T.PURITY"
        strSql += " ,ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID),'')AS SUBITEM"
        strSql += " ,ISNULL((SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = T.ITEMTYPEID),'')AS ITEMTYPE"
        strSql += " ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID)AS ITEMCOUNTER"
        strSql += " ,(SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZEID = T.SIZEID)AS ITEMSIZE"
        strSql += " ,T.TAGNO,T.PCS,T.GRSWT,T.LESSWT,T.NETWT,T.RATE,T.GRSNET,T.TABLECODE,T.STYLENO"
        strSql += " ,T.MAXWASTPER,T.MAXWAST,T.MAXMCGRM,T.MAXMC,T.MINWASTPER,T.MINWAST,T.MINMCGRM,T.MINMC"
        strSql += " ,T.SALVALUE,T.NARRATION,T.PCTFILE"
        strSql += " ,P.PURLESSWT,P.PURNETWT,P.PURRATE,P.PURGRSNET,P.PURTOUCH,P.PURWASTAGE,P.PURMC,P.PURVALUE"
        strSql += " ,T.BOARDRATE,T.ORSNO,T.RFID,P.PURTAX,P.RECDATE AS PURDATE,T.SALEMODE,T.TOUCH"
        strSql += " ,T.HM_BILLNO,T.HM_CENTER,T.ADD_VA_PER,T.REFVALUE,T.ORDREPNO,T.EXTRAWT,T.USRATE,T.INDRS"
        If _FourCMaintain Then
            strSql += " ,T.CUTID,T.COLORID,T.CLARITYID,T.SHAPEID,T.SETTYPEID,T.HEIGHT,T.WIDTH"
        End If
        strSql += "  FROM " & cnAdminDb & "..ITEMTAG AS T "
        strSql += "  LEFT OUTER JOIN " & cnAdminDb & "..PURITEMTAG AS P ON P.TAGSNO = T.SNO"
        strSql += " WHERE T.RECSNO = '" & WupdIssSno & "' AND T.SNO='" & updIssSno & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTagDetails)
        If Not dtTagDetails.Rows.Count > 0 Then Exit Sub 'Me.Dispose()
        With dtTagDetails.Rows(0)
            'SNO = dtTagDetails.Rows(0).Item("LOTSNO").ToString
            'strSql = " SELECT LOTNO,PCS,GRSWT,CPCS,CGRSWT,PCS-CPCS BALPCS,GRSWT-CGRSWT BALGRSWT,WMCTYPE "
            'strSql += " FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = '" & SNO & "'"
            'Dim dtLotDet As New DataTable
            'da = New OleDbDataAdapter(strSql, cn)
            'da.Fill(dtLotDet)
            'If dtLotDet.Rows.Count > 0 Then
            '    With dtLotDet.Rows(0)
            '        'txtLotNo_Num_Man.Text = .Item("LOTNO").ToString
            '        cmbCostCentre_Man.Text = dtTagDetails.Rows(0).Item("COSTCENTRE").ToString
            '        lblPLot.Text = IIf(Val(.Item("PCS").ToString) <> 0, Val(.Item("PCS").ToString), "")
            '        lblPCompled.Text = IIf(Val(.Item("CPCS").ToString) <> 0, Val(.Item("CPCS").ToString), "")
            '        lblPBalance.Text = IIf(Val(.Item("BALPCS").ToString) <> 0, Val(.Item("BALPCS").ToString), "")

            '        lblWLot.Text = IIf(Val(.Item("GRSWT").ToString) <> 0, Val(.Item("GRSWT").ToString).ToString("0.000"), "")
            '        lblWCompleted.Text = IIf(Val(.Item("CGRSWT").ToString) <> 0, Val(.Item("CGRSWT")).ToString("0.000"), "")
            '        lblWBalance.Text = IIf(Val(.Item("BALGRSWT").ToString) <> 0, Val(.Item("BALGRSWT")).ToString("0.000"), "")

            '    End With
            'End If
            cmbCostCentre_Man.Text = dtTagDetails.Rows(0).Item("COSTCENTRE").ToString
            If .Item("TABLECODE") <> "" Then
                cmbTableCode.Enabled = True
                strSql = " SELECT DISTINCT TABLECODE FROM " & cnAdminDb & "..WMCTABLE WHERE TABLECODE <> '' "
                If cmbCostCentre_Man.Text <> "" Then strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_Man.Text & "'),'')"
                strSql += " ORDER BY TABLECODE"
                objGPack.FillCombo(strSql, cmbTableCode)
            Else
                cmbTableCode.Items.Clear()
                cmbTableCode.Enabled = False
            End If

            cmbItem_MAN.Text = .Item("ITEMNAME").ToString

            pFixedVa = IIf(objGPack.GetSqlValue("SELECT ISNULL(FIXEDVA,'N') FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'", , "N") = "N", False, True)
            Dim xItemid As Integer
            strSql = " SELECT ITEMID,METALID,STUDDEDSTONE,SIZESTOCK,MULTIMETAL,OTHCHARGE,CALTYPE,NOOFPIECE,PIECERATE,VALUEADDEDTYPE,GROSSNETWTDIFF"
            strSql += " ,STUDDED,STOCKTYPE,TableCode,MCASVAPER FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Item("ITEMNAME").ToString & "'"
            Dim dtItemDetail As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtItemDetail)
            If dtItemDetail.Rows.Count > 0 Then
                With dtItemDetail.Rows(0)
                    xItemid = Val(.Item("ITEMID").ToString)
                    METALID = .Item("METALID").ToString
                    studdedStone = .Item("STUDDEDSTONE").ToString
                    grossnetdiff = .Item("GROSSNETWTDIFF").ToString
                    sizeStock = .Item("SIZESTOCK").ToString
                    multiMetal = .Item("MULTIMETAL").ToString
                    OthCharge = .Item("OTHCHARGE").ToString
                    noOfPiece = Val(.Item("NOOFPIECE").ToString)
                    pieceRate = Val(.Item("PIECERATE").ToString)
                    calType = .Item("CALTYPE").ToString
                    If .Item("MCASVAPER").ToString = "Y" Then Label44.Text = "MC PERCENT" Else Label44.Text = "Max Mc Per Gram"
                    cmbSubItem_Man.Enabled = True
                    strSql = GetSubItemQry(New String() {"SUBITEMNAME"}, xItemid)
                    objGPack.FillCombo(strSql, cmbSubItem_Man)
                    If Not cmbSubItem_Man.Items.Count > 0 Then
                        cmbSubItem_Man.Enabled = False
                    Else
                        cmbSubItem_Man.Enabled = True
                        cmbSubItem_Man.Text = ""
                    End If
                    If cmbSubItem_Man.Text <> "" Then
                        Dim subdr As DataRow = GetSqlRow("SELECT ISNULL(FIXEDVA,'N'),CALTYPE,NOOFPIECE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "' AND ITEMID =" & xItemid & "", cn, tran)

                        pFixedVa = subdr.Item(0).ToString
                        calType = subdr.Item(1).ToString
                        noOfPiece = Val(subdr.Item(2).ToString)
                        'pFixedVa = IIf(objGPack.GetSqlValue("SELECT ISNULL(FIXEDVA,'N') FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "'", , "N") = "N", False, True)
                    End If
                    Dim mtablecode As String = dtTagDetails.Rows(0).Item("TABLECODE").ToString
                    If mtablecode = "" Then
                        strSql = " select TableCode from " & cnAdminDb & "..itemMast where ItemName = '" & dtTagDetails.Rows(0).Item("ITEMNAME").ToString & "' and TableCode <> ''"
                        mtablecode = objGPack.GetSqlValue(strSql)
                    End If
                    If cmbTableCode.Items.Contains(mtablecode) = False Then cmbTableCode.Items.Add(mtablecode)
                    cmbTableCode.Text = mtablecode
                    funcAssignTabControls()
                End With
            End If


            If .Item("ORSNO").ToString <> "" Then
                strSql = " SELECT * FROM " & cnAdminDb & "..ORMAST"
                strSql += " WHERE SNO = '" & .Item("ORSNO").ToString & "'"
                Dim dtOrdDet As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtOrdDet)
                If dtOrdDet.Rows.Count > 0 Then
                    OrderRow = dtOrdDet.Rows(0)
                End If
            End If
            dtpRecieptDate.MinimumDate = (New DateTimePicker).MinDate
            dtpRecieptDate.MaximumDate = (New DateTimePicker).MaxDate

            'txtLotNo_Num_Man.Enabled = False
            cmbItem_MAN.Text = .Item("ITEMNAME").ToString
            pFixedVa = IIf(objGPack.GetSqlValue("SELECT ISNULL(FIXEDVA,'N') FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'", , "N") = "N", False, True)
            cmbItem_MAN.Enabled = False
            cmbItemType_MAN.Text = .Item("ITEMTYPE").ToString
            cmbDesigner_MAN.Text = .Item("DESIGNER").ToString
            cmbCounter_MAN.Text = .Item("ITEMCOUNTER").ToString
            dtpRecieptDate.Value = .Item("RECDATE")
            txtPurity_Per.Text = .Item("PURITY").ToString
            txtMetalRate_Amt.Text = .Item("BOARDRATE").ToString ' GetMetalRate()
            cmbSubItem_Man.Text = .Item("SUBITEM").ToString
            ObjRsUs.txtUSDollar_Amt.Text = .Item("USRATE").ToString
            ObjRsUs.txtIndRs_Amt.Text = .Item("INDRS").ToString
            Me.cmbSubItem_SelectedIndexChanged(Me, New EventArgs)
            cmbItemType_MAN.Text = .Item("ITEMTYPE").ToString
            If sizeStock = "Y" Then
                cmbItemSize.Enabled = True
                strSql = " SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')"
                objGPack.FillCombo(strSql, cmbItemSize, , False)
                If cmbItemSize.Items.Count > 0 Then cmbItemSize.Enabled = True Else cmbItemSize.Enabled = False
                cmbItemSize.Text = .Item("ITEMSIZE").ToString
            Else
                cmbItemSize.Enabled = False
            End If
            txtTagNo__Man.Text = .Item("TAGNO").ToString
            txtTagNo__Man.Enabled = False
            txtPieces_Num_Man.Text = .Item("PCS").ToString
            '328
            noOfPiece = Val(.Item("PCS").ToString)
            '328
            txtGrossWt_Wet.Text = .Item("GRSWT").ToString
            ObjExtraWt.txtExtraWt_WET.Text = .Item("EXTRAWT").ToString
            txtLessWt_Wet.Text = .Item("LESSWT").ToString
            txtNetWt_Wet.Text = .Item("NETWT").ToString
            txtRate_Amt.Text = IIf(Val(.Item("RATE").ToString) <> 0, .Item("RATE").ToString, Nothing)
            cmbCalcMode.Text = IIf(.Item("GRSNET").ToString = "G", "GRS WT", "NET WT")
            cmbTableCode.Text = .Item("TABLECODE").ToString
            txtStyleCode.Text = .Item("STYLENO").ToString
            txtMaxWastage_Per.Text = IIf(Val(.Item("MAXWASTPER").ToString) <> 0, .Item("MAXWASTPER").ToString, Nothing)
            txtMaxWastage_Wet.Text = IIf(Val(.Item("MAXWAST").ToString) <> 0, .Item("MAXWAST").ToString, Nothing)
            txtMaxMcPerGrm_Amt.Text = IIf(Val(.Item("MAXMCGRM").ToString) <> 0, .Item("MAXMCGRM").ToString, Nothing)
            txtMaxMkCharge_Amt.Text = IIf(Val(.Item("MAXMC").ToString) <> 0, .Item("MAXMC").ToString, Nothing)
            ObjMinValue.txtMinWastage_Per.Text = IIf(Val(.Item("MINWASTPER").ToString) <> 0, .Item("MINWASTPER").ToString, Nothing)
            ObjMinValue.txtMinWastage_Wet.Text = IIf(Val(.Item("MINWAST").ToString) <> 0, .Item("MINWAST").ToString, Nothing)
            ObjMinValue.txtMinMcPerGram_Amt.Text = IIf(Val(.Item("MINMCGRM").ToString) <> 0, .Item("MINMCGRM").ToString, Nothing)
            ObjMinValue.txtMinMkCharge_Amt.Text = IIf(Val(.Item("MINMC").ToString) <> 0, .Item("MINMC").ToString, Nothing)
            txtTouch_AMT.Text = .Item("TOUCH").ToString
            txtRfId.Text = .Item("RFID").ToString
            txtHmBillNo.Text = .Item("HM_BILLNO").ToString
            txtHmCentre.Text = .Item("HM_CENTER").ToString
            txtNarration.Text = .Item("NARRATION").ToString
            txtOrderNo.Text = .Item("ORDREPNO").ToString
            txtRefVal_AMT.Text = IIf(Val(.Item("REFVALUE").ToString) <> 0, Format(Val(.Item("REFVALUE").ToString), "0.00"), "")
            If txtOrderNo.Text <> "" Then lblOrder.Visible = True : txtOrderNo.Visible = True

            If .Item("PCTFILE").ToString <> "" Then
                picPath = .Item("PCTFILE").ToString
                Dim serverPath As String = Nothing
                Dim fileDestPath As String = defalutDestination & .Item("PCTFILE").ToString
                AutoImageSizer(fileDestPath, picModel, PictureBoxSizeMode.CenterImage)
                picModel.BringToFront()
                Dim Finfo As FileInfo
                Finfo = New FileInfo(fileDestPath)
                'Finfo.IsReadOnly = False
                picExtension = Finfo.Extension

            End If
            ''MULTIMETAL
            strSql = " SELECT"
            strSql += " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = T.CATCODE)AS CATEGORY"
            strSql += " ,GRSWT WEIGHT,WASTPER WASTAGEPER,WAST WASTAGE,MCGRM MCPERGRM,MC MC,AMOUNT,NULL PURRATE"
            strSql += " ,(SELECT PURWASTAGE FROM " & cnAdminDb & "..PURITEMTAGMETAL WHERE METSNO = T.SNO)AS PURWASTAGE"
            strSql += " ,(SELECT PURMC FROM " & cnAdminDb & "..PURITEMTAGMETAL WHERE METSNO = T.SNO)AS PURMC"
            strSql += " ,(SELECT PURVALUE FROM " & cnAdminDb & "..PURITEMTAGMETAL WHERE METSNO = T.SNO)AS PURAMOUNT"

            strSql += " FROM " & cnAdminDb & "..ITEMTAGMETAL AS T"
            strSql += " WHERE TAGSNO = '" & updIssSno & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtMultiMetalDetails)
            StyleGridMultiMetal()
            CalcMultiMetalTotal()

            ''STONEDETAILS
            strSql = " SELECT "
            strSql += " ISNULL(PACKETNO,'') PACKETNO,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = STNITEMID)AS ITEM"
            strSql += " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = STNSUBITEMID)AS SUBITEM"
            strSql += " ,STNPCS PCS,STNWT WEIGHT"
            strSql += " ,STONEUNIT UNIT,CALCMODE CALC,STNRATE RATE,STNAMT AMOUNT"
            strSql += " ,(SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) AS METALID"
            strSql += " ,NULL PURRATE"
            strSql += " ,NULL PURVALUE,SNO STNSNO,USRATE,INDRS"
            If _FourCMaintain Then
                strSql += " ,(SELECT CUTNAME FROM " & cnAdminDb & "..STNCUT WHERE CUTID = S.CUTID)AS CUT"
                strSql += " ,(SELECT COLORNAME FROM " & cnAdminDb & "..STNCOLOR WHERE COLORID = S.COLORID)AS COLOR"
                strSql += " ,(SELECT CLARITYNAME FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYID = S.CLARITYID)AS CLARITY"
                strSql += " ,(SELECT SHAPENAME FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPEID = S.SHAPEID)AS SHAPE"
                strSql += " ,(SELECT SETTYPENAME FROM " & cnAdminDb & "..STNSETTYPE WHERE SETTYPEID = S.SETTYPEID)AS SETTYPE"
                strSql += " ,S.HEIGHT,S.WIDTH"
            End If
            'strSql += " ,PURRATE,PURAMT PURVALUE"
            strSql += " FROM " & cnAdminDb & "..ITEMTAGSTONE S"
            strSql += " WHERE TAGSNO = '" & updIssSno & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtStoneDetails)
            dtStoneDetails.AcceptChanges()
            StyleGridStone()
            CalcLessWt()
            CalcFinalTotal()

            If _HasPurchase Or pFixedVa Or PUR_AUTOCALC Then
                strSql = " SELECT "
                strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = STNITEMID)AS ITEM"
                strSql += " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = STNSUBITEMID)AS SUBITEM"
                strSql += " ,STNPCS PCS,STNWT WEIGHT"
                strSql += " ,STONEUNIT UNIT,CALCMODE CALC,STNRATE RATE,STNAMT AMOUNT"
                strSql += " ,(SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) AS METALID"
                strSql += " ,PURRATE,PURAMT PURVALUE,STNSNO"
                strSql += " FROM " & cnAdminDb & "..PURITEMTAGSTONE S"
                strSql += " WHERE TAGSNO = '" & updIssSno & "'"
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(ObjPurDetail.dtGridStone)
                ObjPurDetail.dtGridStone.AcceptChanges()
            End If
            For Each RoPur As DataRow In ObjPurDetail.dtGridStone.Rows
                If RoPur.Item("STNSNO").ToString = "" Then Continue For
                For Each Ro_S As DataRow In dtStoneDetails.Rows
                    If Ro_S.Item("STNSNO") = RoPur.Item("STNSNO") Then
                        RoPur.Item("KEYNO") = Ro_S.Item("KEYNO")
                        Exit For
                    End If
                Next
            Next
            ''MISCCHARGE
            strSql = " SELECT"
            strSql += " (SELECT MISCNAME FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCID = T.MISCID)MISC"
            strSql += " ,AMOUNT"
            strSql += " ,(SELECT PURAMOUNT FROM " & cnAdminDb & "..PURITEMTAGMISCCHAR WHERE MISSNO = T.SNO)PURAMOUNT "
            strSql += " FROM " & cnAdminDb & "..ITEMTAGMISCCHAR AS T"
            strSql += " WHERE TAGSNO = '" & updIssSno & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtMiscDetails)
            StyleGridMisc()
            dtMiscDetails.AcceptChanges()
            CalcFinalTotal()
            'Additional stock Entry
            strSql = "SELECT OTHID,O.NAME AS OTHNAME,O.MISCID FROM " & cnAdminDb & "..ADDINFOITEMTAG A"
            strSql += " LEFT JOIN " & cnAdminDb & "..OTHERMASTER O ON A.OTHID=O.ID"
            strSql += " WHERE A.TAGSNO = '" & updIssSno & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(DtAddStockEntry)
            DtAddStockEntry.AcceptChanges()

            lotRecieptDate = objGPack.GetSqlValue("SELECT LOTDATE FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = '" & updIssSno & "'", , dtpRecieptDate.Value.Date.ToString("yyyy-MM-dd")) 'LOTNO = " & Val(txtLotNo_Num_Man.Text) & "
            txtSalValue_Amt_Man.Text = IIf(Val(.Item("SALVALUE").ToString) <> 0, SALEVALUE_ROUND(Val(.Item("SALVALUE").ToString)), Nothing)
            If SALEVALUEPLUS <> 0 Then txtSalValue_Amt_Man.Text = Val(txtSalValue_Amt_Man.Text) * SALEVALUEPLUS
            SyncStoneMiscToPurStoneMisc()
            'PURRATE, PURGRSNET, PURTOUCH, PURWASTAGE, PURMC
            If .Item("PURDATE").ToString <> "" Then
                ObjPurDetail.dtpPurchaseDate.Value = .Item("PURDATE")
            End If
            ObjPurDetail.txtpURFixedValueVa_AMT.Text = IIf(Val(.Item("ADD_VA_PER").ToString) <> 0, Format(Val(.Item("ADD_VA_PER").ToString), "0.00"), "")
            ObjPurDetail.salePcs = Val(txtPieces_Num_Man.Text)
            ObjPurDetail.CalcMode = calType
            ObjPurDetail.txtPurGrossWt_Wet.Text = IIf(Val(.Item("GRSWT").ToString) <> 0, .Item("GRSWT").ToString, Nothing)
            ObjPurDetail.txtPurLessWt_Wet.Text = IIf(Val(.Item("PURLESSWT").ToString) <> 0, .Item("PURLESSWT").ToString, Nothing)
            ObjPurDetail.txtPurNetWt_Wet.Text = IIf(Val(.Item("PURNETWT").ToString) <> 0, .Item("PURNETWT").ToString, Nothing)
            purRate = Val(.Item("PURRATE").ToString)
            ObjPurDetail.txtPurRate_Amt.Text = IIf(Val(.Item("PURRATE").ToString) <> 0, .Item("PURRATE").ToString, Nothing)
            'ObjPurDetail.cmbPurCalMode.Text = IIf(.Item("PURGRSNET").ToString = "G", ObjPurDetail.cmbPurCalMode.Text = "GRS WT", ObjPurDetail.cmbPurCalMode.Text = "NET WT")
            If .Item("PURGRSNET").ToString = "G" Then
                ObjPurDetail.cmbPurCalMode.Text = "GRS WT"
            Else
                ObjPurDetail.cmbPurCalMode.Text = "NET WT"
            End If
            ObjPurDetail.txtPurTouch_Amt.Text = IIf(Val(.Item("PURTOUCH").ToString) <> 0, .Item("PURTOUCH").ToString, Nothing)
            ObjPurDetail.txtPurWastage_Wet.Text = IIf(Val(.Item("PURWASTAGE").ToString) <> 0, .Item("PURWASTAGE").ToString, Nothing)
            ObjPurDetail.txtPurMakingChrg_Amt.Text = IIf(Val(.Item("PURMC").ToString) <> 0, .Item("PURMC").ToString, Nothing)
            ObjPurDetail.txtPurTax_AMT.Text = IIf(Val(.Item("PURTAX").ToString) <> 0, .Item("PURTAX").ToString, Nothing)
            ObjPurDetail.txtPurPurchaseVal_Amt.Text = IIf(Val(.Item("PURVALUE").ToString) <> 0, .Item("PURVALUE").ToString, Nothing)
            txtPurchaseValue_Amt.Text = ObjPurDetail.txtPurPurchaseVal_Amt.Text
            ObjPurDetail.CalcPurchaseGrossValue()
            ObjPurDetail.CalcPurchaseValue()

            If UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'TAGEDITB4DATE'", , "N")) = "N" Then
                dtpRecieptDate.Enabled = False
            End If
            If UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'TAGEDITPCSWT'", , "N")) = "N" Then
                txtPieces_Num_Man.ReadOnly = True
                txtGrossWt_Wet.ReadOnly = True
                txtNetWt_Wet.ReadOnly = True
            End If
            If TAGEDITDISABLE <> "" Then
                If TAGEDITDISABLE.Contains("SI") Then cmbSubItem_Man.Enabled = False
                If TAGEDITDISABLE.Contains("IT") Then cmbItemType_MAN.Enabled = False
                If TAGEDITDISABLE.Contains("TC") Then cmbTableCode.Enabled = False
                If TAGEDITDISABLE.Contains("SD") Then gridStone.Enabled = False : pnlStDet.Enabled = False
                If TAGEDITDISABLE.Contains("SV") Then txtSalValue_Amt_Man.Enabled = False
                If TAGEDITDISABLE.Contains("PV") Then txtPurchaseValue_Amt.Enabled = False
                If TAGEDITDISABLE.Contains("RI") Then txtRate_Amt.ReadOnly = True
                If TAGEDITDISABLE.Contains("WS") Then txtMaxWastage_Per.ReadOnly = True : txtMaxWastage_Wet.ReadOnly = True
                If TAGEDITDISABLE.Contains("MC") Then txtMaxMcPerGrm_Amt.ReadOnly = True : txtMaxMkCharge_Amt.ReadOnly = True
                If TAGEDITDISABLE.Contains("DG") Then cmbDesigner_MAN.Enabled = False
                If TAGEDITDISABLE.Contains("RT") Then txtMetalRate_Amt.ReadOnly = True
                If TAGEDITDISABLE.Contains("DT") Then dtpRecieptDate.Enabled = False
                If TAGEDITDISABLE.Contains("CT") Then cmbCounter_MAN.Enabled = False
                If TAGEDITDISABLE.Contains("SZ") Then cmbItemSize.Enabled = False
                If TAGEDITDISABLE.Contains("NR") Then txtNarration.ReadOnly = True

            End If
            lotPcs = Val(txtPieces_Num_Man.Text)
            lotGrsWt = Val(txtGrossWt_Wet.Text)
            lotNetWt = Val(txtNetWt_Wet.Text)
            If objGPack.DupCheck("SELECT 1 FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "' AND STYLENO = 'Y'") Then
                isStyleCode = True
            End If

            'ORDER NO
            'ElseIf _CheckOrderInfo = False And ObjOrderTagInfo.txtOrderNo.Text <> "" Then
            If _CheckOrderInfo = False And dtTagDetails.Rows(0).Item("ORDREPNO").ToString <> "0" And dtTagDetails.Rows(0).Item("ORDREPNO").ToString <> "" Then                ' 
                ObjOrderTagInfo = New TagOrderInfo
                strSql = " SELECT EMPID FROM " & cnAdminDb & "..ORMAST WHERE ISNULL(ORNO,'') = '" & dtTagDetails.Rows(0).Item("ORDREPNO").ToString & "'"
                ObjOrderTagInfo.txtOrderNo.Text = dtTagDetails.Rows(0).Item("ORDREPNO").ToString.Substring(5)
                Dim dtemp As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtemp)
                If dtemp.Rows.Count > 0 Then ObjOrderTagInfo.txtEmpNo_NUM.Text = Val(dtemp.Rows(0).Item(0).ToString)
                EditOrNo = dtTagDetails.Rows(0).Item("ORDREPNO").ToString 'ObjOrderTagInfo.txtOrderNo.Text
                EditEmpId = ObjOrderTagInfo.txtEmpNo_NUM.Text
            End If

            btnNew.Enabled = False

            If _FourCMaintain Then
                strSql = "SELECT ISNULL(MAINTAIN4C,'N') FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "' AND METALID='D'"
                If objGPack.GetSqlValue(strSql).ToString.ToUpper = "N" Then Exit Sub
                strSql = "SELECT STUDDED FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "' AND METALID='D'"
                If objGPack.GetSqlValue(strSql).ToString.ToUpper = "L" Then
                    TagCutId = Val(dtTagDetails.Rows(0).Item("CUTID").ToString)
                    TagColorId = Val(dtTagDetails.Rows(0).Item("COLORID").ToString)
                    TagClarityId = Val(dtTagDetails.Rows(0).Item("CLARITYID").ToString)
                    TagShapeId = Val(dtTagDetails.Rows(0).Item("SHAPEID").ToString)
                    TagSetTypeId = Val(dtTagDetails.Rows(0).Item("SETTYPEID").ToString)
                    ObjDiaDetails = New frmDiamondDetails
                    ObjDiaDetails.CmbCut.Text = objGPack.GetSqlValue(" SELECT CUTNAME FROM " & cnAdminDb & "..STNCUT WHERE CUTID = '" & TagCutId & "'", "CUTNAME", , tran)
                    ObjDiaDetails.CmbColor.Text = objGPack.GetSqlValue(" SELECT COLORNAME FROM " & cnAdminDb & "..STNCOLOR WHERE COLORID = '" & TagColorId & "'", "COLORNAME", , tran)
                    ObjDiaDetails.CmbClarity.Text = objGPack.GetSqlValue(" SELECT CLARITYNAME FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYID = '" & TagClarityId & "'", "CLARITYNAME", , tran)
                    ObjDiaDetails.cmbShape.Text = objGPack.GetSqlValue(" SELECT SHAPENAME FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPEID = '" & TagShapeId & "'", "SHAPENAME", , tran)
                    ObjDiaDetails.cmbSetType.Text = objGPack.GetSqlValue(" SELECT SETTYPENAME FROM " & cnAdminDb & "..STNSETTYPE WHERE SETTYPEID = '" & TagSetTypeId & "'", "SETTYPENAME", , tran)
                    ObjDiaDetails.txtWidth_WET.Text = Val(dtTagDetails.Rows(0).Item("WIDTH").ToString)
                    ObjDiaDetails.txtHeight_WET.Text = Val(dtTagDetails.Rows(0).Item("HEIGHT").ToString)
                End If
            End If
        End With
    End Sub
    Private Sub frmItemTag_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ItemLock = False
    End Sub

    Private Sub frmItemTag_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            'WEB STONE TAB FOCUS

            Select Case WTabControl1.SelectedTab.Name
                Case WtabMultiMetal.Name
                    If IsActive(wgridMultimetal) Then
                        wtxtMMCategory.Select()
                    ElseIf IsActive(wgrpMultiMetal) Then
                        If WTabControl1.TabPages.Contains(WtabStone) Then
                            WTabControl1.SelectTab(WtabStone)
                            wtxtStItem.Select()
                        ElseIf WTabControl1.TabPages.Contains(WtabOtherCharges) Then
                            WTabControl1.SelectTab(WtabOtherCharges)
                            wtxtMiscMisc.Select()
                        Else
                            WTabControl1.SelectTab(tabTagDet)
                            Me.SelectNextControl(WtxtGrossWt_Wet, True, True, True, True)
                        End If
                    End If
                Case WtabStone.Name
                    If IsActive(wgridStone) Then
                        wtxtStItem.Select()
                    ElseIf (IsActive(wgrpStoneDetails) Or IsActive(wpnlStDet)) Then
                        If WTabControl1.TabPages.Contains(WtabOtherCharges) Then
                            WTabControl1.SelectTab(WtabOtherCharges)
                            wtxtMiscMisc.Select()
                        Else
                            WTabControl1.SelectTab(tabTagDet)
                            Me.SelectNextControl(WtxtGrossWt_Wet, True, True, True, True)
                        End If
                    End If
                Case WtabOtherCharges.Name
                    If IsActive(wgridMisc) Then
                        wtxtMiscMisc.Select()
                    ElseIf IsActive(wgrpOtherCharges) Then
                        WTabControl1.SelectTab(tabTagDet)
                        Me.SelectNextControl(WtxtGrossWt_Wet, True, True, True, True)
                    End If
                    'Case tabPurchase.Name
                    '    TabControl1.SelectedTab = tabTag
                    '    Me.SelectNextControl(txtSalValue_Amt_Man, True, True, True, True)
            End Select
            Select Case TabControl1.SelectedTab.Name
                Case tabMultiMetal.Name
                    If IsActive(gridMultimetal) Then
                        txtMMCategory.Select()
                    ElseIf IsActive(grpMultiMetal) Then
                        If TabControl1.TabPages.Contains(tabStone) Then
                            TabControl1.SelectTab(tabStone)
                            'txtStItemSelect()
                        ElseIf TabControl1.TabPages.Contains(tabOtherCharges) Then
                            TabControl1.SelectTab(tabOtherCharges)
                            txtMiscMisc.Select()
                        Else
                            TabControl1.SelectTab(TabPage1)
                            Me.SelectNextControl(txtGrossWt_Wet, True, True, True, True)
                        End If
                    End If
                Case tabStone.Name
                    If IsActive(gridStone) Then
                        txtStItem.Select()
                        Exit Sub
                    ElseIf (IsActive(grpStoneDetails) Or IsActive(pnlStDet)) Then
                        If TabControl1.TabPages.Contains(tabOtherCharges) Then
                            TabControl1.SelectTab(tabOtherCharges)
                            txtMiscMisc.Select()
                            Exit Sub
                        Else
                            TabControl1.SelectTab(TabPage1)
                            Me.SelectNextControl(txtGrossWt_Wet, True, True, True, True)
                            Exit Sub
                        End If
                    End If
                Case tabOtherCharges.Name
                    If IsActive(gridMisc) Then
                        txtMiscMisc.Select()
                        Exit Sub
                    ElseIf IsActive(grpOtherCharges) Then
                        TabControl1.SelectTab(TabPage1)
                        Me.SelectNextControl(txtGrossWt_Wet, True, True, True, True)
                        Exit Sub
                    End If
                Case tabPurchase.Name
                    TabControl1.SelectedTab = TabPage1
                    Me.SelectNextControl(txtSalValue_Amt_Man, True, True, True, True)
                    Exit Sub
            End Select
            Select Case TabGeneral.SelectedTab.Name
                Case tabTag.Name
                    Tagsave = False
                    TabGeneral.SelectedTab = tabWebTag
                    WbtnSave.Focus()
            End Select
        End If
    End Sub

    Private Sub frmItemTag_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            'For Web tag
            If wcmbItem_MAN.Focused Then Exit Sub
            If wgridStone.Focused Then Exit Sub
            If wgridMisc.Focused Then Exit Sub
            If wgridMultimetal.Focused Then Exit Sub

            If WtxtGrossWt_Wet.Focused Then Exit Sub
            If wtxtMMAmount_AMT.Focused Then Exit Sub
            If wtxtStAmount_Amt.Focused Then Exit Sub
            If wtxtstPackettno.Focused Then Exit Sub
            If wtxtStItem.Focused Then Exit Sub
            If wtxtStSubItem.Focused Then Exit Sub
            If wtxtMkCharge_Amt.Focused Then Exit Sub
            If wtxtMiscAmount_Amt.Focused Then Exit Sub
            '' If txtSalValue_Amt_Man.Focused Then Exit Sub
            ''MULTIMETAL    
            If wtxtMMCategory.Focused Then Exit Sub
            ''misc
            If wtxtMiscMisc.Focused Then Exit Sub
            'If txtRate_Amt.Enabled = True And NEEDUS = True And WtxtNetWt_Wet.Focused Then Exit Sub
            'If WtxtStRate_Amt.Enabled = True And NEEDUS = True And cmbStCalc.Focused Then Exit Sub
            'If NEEDUS = True And Studded_Loose = "L" And txtMetalRate_Amt.Focused And Val(txtMetalRate_Amt.Text) = 0 And Not (calType = "R" Or calType = "M") Then
            '    e.Handled = True
            '    'WtxtGrossWt_Wet.Focus()
            '    Me.SelectNextControl(txtMetalRate_Amt, True, True, True, True)
            '    Exit Sub
            'End If

            If NEEDUS = True And wtxtMMRate.Focused And calType = "M" Then
                strSql = " SELECT ISNULL(STUDDED,'') STUDDED FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') ='" & wcmbItem_MAN.Text & "' AND METALID = 'D'"
                Studded_Loose = objGPack.GetSqlValue(strSql, "STUDDED", "")
                If Studded_Loose = "L" Then
                    ObjRsUs.Label1.Focus()
                    Exit Sub
                End If
            End If

            If NEEDUS = True And wtxtStRate_Amt.Focused And Val(wtxtStRate_Amt.Text) = 0 Then
                strSql = " SELECT ISNULL(STUDDED,'') STUDDED FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') ='" & wtxtStItem.Text & "' AND METALID = 'D'"
                Studded_Loose = objGPack.GetSqlValue(strSql, "STUDDED", "")
                If Studded_Loose <> "" Then
                    ObjRsUs.Label1.Focus()
                    Exit Sub
                End If
            End If

            'For Tag
            If cmbItem_MAN.Focused Then Exit Sub
            If gridStone.Focused Then Exit Sub
            If gridMisc.Focused Then Exit Sub
            If gridMultimetal.Focused Then Exit Sub

            If txtGrossWt_Wet.Focused Then Exit Sub
            If txtMMAmount_AMT.Focused Then Exit Sub
            If txtStAmount_Amt.Focused Then Exit Sub
            If txtstPackettno.Focused Then Exit Sub
            If txtStItem.Focused Then Exit Sub
            If txtStSubItem.Focused Then Exit Sub
            If txtMiscAmount_Amt.Focused Then Exit Sub
            If txtSalValue_Amt_Man.Focused Then Exit Sub
            If txtNarration.Focused Then Exit Sub
            ''MULTIMETAL    
            If txtMMCategory.Focused Then Exit Sub
            ''misc
            If txtMiscMisc.Focused Then Exit Sub
            If txtRate_Amt.Enabled = True And NEEDUS = True And txtNetWt_Wet.Focused Then Exit Sub
            If txtStRate_Amt.Enabled = True And NEEDUS = True And cmbStCalc.Focused Then Exit Sub
            If NEEDUS = True And Studded_Loose = "L" And txtMetalRate_Amt.Focused And Val(txtMetalRate_Amt.Text) = 0 And Not (calType = "R" Or calType = "M") Then
                e.Handled = True
                'txtGrossWt_Wet.Focus()
                Me.SelectNextControl(txtMetalRate_Amt, True, True, True, True)
                Exit Sub
            End If

            If NEEDUS = True And txtMMRate.Focused And calType = "M" Then
                strSql = " SELECT ISNULL(STUDDED,'') STUDDED FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') ='" & cmbItem_MAN.Text & "' AND METALID = 'D'"
                Studded_Loose = objGPack.GetSqlValue(strSql, "STUDDED", "")
                If Studded_Loose = "L" Then
                    ObjRsUs.Label1.Focus()
                    Exit Sub
                End If
            End If

            If NEEDUS = True And txtStRate_Amt.Focused And Val(txtStRate_Amt.Text) = 0 Then
                strSql = " SELECT ISNULL(STUDDED,'') STUDDED FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') ='" & txtStItem.Text & "' AND METALID = 'D'"
                Studded_Loose = objGPack.GetSqlValue(strSql, "STUDDED", "")
                If Studded_Loose <> "" Then
                    ObjRsUs.Label1.Focus()
                    Exit Sub
                End If
            End If

            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.F2) Then
            If IsActive(wgrpStoneDetails) Then
                wgridStone.Select()
            End If
            If IsActive(grpStoneDetails) Then
                gridStone.Select()
            End If
        End If
    End Sub
    Private Sub frmItemTag_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'TabGeneral.ItemSize = New System.Drawing.Size(1, 1)
        'Me.TabGeneral.Region = New Region(New RectangleF(Me.tabMain.Left, Me.tabMain.Top, Me.tabMain.Width, Me.tabMain.Height))
        TabGeneral.SelectedTab = tabWebTag
        If GetAdmindbSoftValue("HIDE-WTMACHINEDET", "Y") = "Y" Then
            wgrpWtMachingDet.Visible = False
            grpWtMachingDet.Visible = False
        Else
            wgrpWtMachingDet.Visible = True
            grpWtMachingDet.Visible = True
        End If

        Dim portSettings() As String
        Dim ctrnameportsetting As String = System.Net.Dns.GetHostName().ToUpper & "-PORTSETTINGS"
        Dim portSettingstr As String = GetAdmindbSoftValue(ctrnameportsetting, "")
        If portSettingstr <> "" Then
            portSettings = portSettingstr.Split("/")
        Else
            portSettings = GetAdmindbSoftValue("PORTSETTINGS", "9600/COM1/8/N").Split("/")
        End If


        Port_BaudRate = IIf(Val(portSettings(0)) <> 0, Val(portSettings(0)), 9600)
        Port_PortName = IIf(portSettings(1) <> "", portSettings(1), "COM1")
        Port_DataBit = IIf(Val(portSettings(2)) <> 0, Val(portSettings(2)), 8)
        If portSettings.Length - 1 >= 3 Then Port_Parity = IIf(portSettings(3) <> "", portSettings(3), "N")
        wSerialPort1.DataBits = Port_DataBit
        wSerialPort1.BaudRate = Port_BaudRate
        wSerialPort1.PortName = Port_PortName
        wSerialPort1.Parity = WGetParity(Port_Parity)
        objPropertyDia = New PropertyDia(wSerialPort1)
        If tagEdit Then Exit Sub


        wtxtNarration.CharacterCasing = CharacterCasing.Normal
        wpnlMain.BorderStyle = BorderStyle.None
        wpnlMultiMetal.BackColor = Drawing.Color.LightGoldenrodYellow
        wpnlStoneGrid.BackColor = Drawing.Color.LightGoldenrodYellow
        wpnlMisc.BackColor = Drawing.Color.LightGoldenrodYellow

        txtNarration.CharacterCasing = CharacterCasing.Normal
        pnlMain.BorderStyle = BorderStyle.None
        pnlMultiMetal.BackColor = Drawing.Color.LightGoldenrodYellow
        pnlStoneGrid.BackColor = Drawing.Color.LightGoldenrodYellow
        pnlMisc.BackColor = Drawing.Color.LightGoldenrodYellow


        WTabControl1.SelectTab(tabTagDet)
        Me.TabControl1_SelectedIndexChanged(Me, New EventArgs)
        'TabControl1.SelectedTab.Name = tabTag.Name
        If objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'MULTIMETALCALC'") = "Y" Then
            multiMetalCalc = True
        End If
        If Lotautopost Then costLock = False
        ''dtGridView
        wgridView.BorderStyle = BorderStyle.None
        gridView.BorderStyle = BorderStyle.None
        'For Webtag
        With dtWGridView.Columns
            ''SUBITEM,ITEMSIZE,TAGNO,PIECES,GRSWEIGHT,LESSWEIGHT,NETWT,RATE,CALCMODE,TABLECODE
            .Add("ITEM", GetType(String))
            .Add("SUBITEM", GetType(String))
            .Add("TAGNO", GetType(String))
            .Add("PCS", GetType(Int16))
            .Add("GRSWT", GetType(Decimal))
            .Add("LESSWT", GetType(Decimal))
            .Add("NETWT", GetType(Decimal))
            .Add("RATE", GetType(Decimal))
            .Add("SALEVALUE", GetType(Decimal))
            .Add("SIZE", GetType(String))
        End With
        wgridView.DataSource = dtWGridView
        With wgridView
            .Columns("ITEM").Width = 150
            .Columns("SUBITEM").Width = 120
            .Columns("TAGNO").Width = 60
            .Columns("PCS").Width = 60
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").Width = 70
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("LESSWT").Width = 70
            .Columns("LESSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETWT").Width = 70
            .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RATE").Width = 70
            .Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SALEVALUE").Width = 100
            .Columns("SALEVALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SALEVALUE").HeaderText = "SALE VALUE"
            .Columns("SIZE").Width = 200
        End With
        wgridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells

        'For tag
        With dtGridView.Columns
            ''SUBITEM,ITEMSIZE,TAGNO,PIECES,GRSWEIGHT,LESSWEIGHT,NETWT,RATE,CALCMODE,TABLECODE
            .Add("ITEM", GetType(String))
            .Add("SUBITEM", GetType(String))
            .Add("TAGNO", GetType(String))
            .Add("PCS", GetType(Int16))
            .Add("GRSWT", GetType(Decimal))
            .Add("LESSWT", GetType(Decimal))
            .Add("NETWT", GetType(Decimal))
            .Add("RATE", GetType(Decimal))
            .Add("SALEVALUE", GetType(Decimal))
            .Add("SIZE", GetType(String))
        End With
        gridView.DataSource = dtGridView
        With gridView
            .Columns("ITEM").Width = 150
            .Columns("SUBITEM").Width = 120
            .Columns("TAGNO").Width = 60
            .Columns("PCS").Width = 60
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").Width = 70
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("LESSWT").Width = 70
            .Columns("LESSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETWT").Width = 70
            .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RATE").Width = 70
            .Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SALEVALUE").Width = 100
            .Columns("SALEVALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SALEVALUE").HeaderText = "SALE VALUE"
            .Columns("SIZE").Width = 200
        End With
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells


        'Web Tag MultiMetal
        With dtWMultiMetalDetails.Columns
            .Add("CATEGORY", GetType(String))
            .Add("WEIGHT", GetType(Double))
            .Add("RATE", GetType(Double))
            .Add("WASTAGEPER", GetType(Double))
            .Add("WASTAGE", GetType(Double))
            .Add("MCPERGRM", GetType(Double))
            .Add("MC", GetType(Double))
            .Add("AMOUNT", GetType(Double))
            .Add("PURWASTAGEPER", GetType(Double))
            .Add("PURWASTAGE", GetType(Double))
            .Add("PURMCPERGRM", GetType(Double))
            .Add("PURMC", GetType(Double))
            .Add("PURRATE", GetType(Double))
            .Add("PURAMOUNT", GetType(Double))
            .Add("STNSNO", GetType(String))
            .Add("KEYNO", GetType(Integer))
            dtWMultiMetalDetails.Columns("KEYNO").AutoIncrement = True
            dtWMultiMetalDetails.Columns("KEYNO").AutoIncrementStep = 1
            dtWMultiMetalDetails.Columns("KEYNO").AutoIncrementSeed = 1
        End With
        wgridMultimetal.DataSource = dtWMultiMetalDetails
        WStyleGridMultiMetal()

        Dim dtWMultiMetalTotal As New DataTable
        dtWMultiMetalTotal = dtWMultiMetalDetails.Copy
        dtWMultiMetalTotal.Rows.Add()
        dtWMultiMetalTotal.Rows(0).Item("CATEGORY") = "TOTAL"
        dtWMultiMetalTotal.AcceptChanges()
        With wgridMultiMetalTotal
            .DataSource = dtWMultiMetalTotal
            .Columns("CATEGORY").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            For cnt As Integer = 0 To wgridMultimetal.ColumnCount - 1
                .Columns(cnt).Width = wgridMultimetal.Columns(cnt).Width
                .Columns(cnt).Visible = wgridMultimetal.Columns(cnt).Visible
                .Columns(cnt).DefaultCellStyle = wgridMultimetal.Columns(cnt).DefaultCellStyle
                .Columns(cnt).ReadOnly = True
            Next
        End With

        'Tag MultiMetal
        With dtMultiMetalDetails.Columns
            .Add("CATEGORY", GetType(String))
            .Add("WEIGHT", GetType(Double))
            .Add("RATE", GetType(Double))
            .Add("WASTAGEPER", GetType(Double))
            .Add("WASTAGE", GetType(Double))
            .Add("MCPERGRM", GetType(Double))
            .Add("MC", GetType(Double))
            .Add("AMOUNT", GetType(Double))
            .Add("PURWASTAGEPER", GetType(Double))
            .Add("PURWASTAGE", GetType(Double))
            .Add("PURMCPERGRM", GetType(Double))
            .Add("PURMC", GetType(Double))
            .Add("PURRATE", GetType(Double))
            .Add("PURAMOUNT", GetType(Double))
            .Add("STNSNO", GetType(String))
            .Add("KEYNO", GetType(Integer))
            dtMultiMetalDetails.Columns("KEYNO").AutoIncrement = True
            dtMultiMetalDetails.Columns("KEYNO").AutoIncrementStep = 1
            dtMultiMetalDetails.Columns("KEYNO").AutoIncrementSeed = 1
        End With
        gridMultimetal.DataSource = dtMultiMetalDetails
        StyleGridMultiMetal()

        Dim dtMultiMetalTotal As New DataTable
        dtMultiMetalTotal = dtMultiMetalDetails.Copy
        dtMultiMetalTotal.Rows.Add()
        dtMultiMetalTotal.Rows(0).Item("CATEGORY") = "TOTAL"
        dtMultiMetalTotal.AcceptChanges()
        With gridMultiMetalTotal
            .DataSource = dtMultiMetalTotal
            .Columns("CATEGORY").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            For cnt As Integer = 0 To gridMultimetal.ColumnCount - 1
                .Columns(cnt).Width = gridMultimetal.Columns(cnt).Width
                .Columns(cnt).Visible = gridMultimetal.Columns(cnt).Visible
                .Columns(cnt).DefaultCellStyle = gridMultimetal.Columns(cnt).DefaultCellStyle
                .Columns(cnt).ReadOnly = True
            Next
        End With

        'Web Tag Stone
        With dtWStoneDetails.Columns
            .Add("PACKETNO", GetType(String))
            .Add("ITEM", GetType(String))
            .Add("SUBITEM", GetType(String))
            .Add("PCS", GetType(Int32))
            .Add("WEIGHT", GetType(Decimal))
            .Add("UNIT", GetType(String))
            .Add("CALC", GetType(String))
            .Add("RATE", GetType(Decimal))
            .Add("AMOUNT", GetType(Decimal))
            .Add("METALID", GetType(String))
            .Add("PURRATE", GetType(Decimal))
            .Add("PURVALUE", GetType(Decimal))
            .Add("KEYNO", GetType(Integer))
            .Add("STNSNO", GetType(String))
            .Add("USRATE", GetType(Decimal))
            .Add("INDRS", GetType(Decimal))
            .Add("CUT", GetType(String))
            .Add("COLOR", GetType(String))
            .Add("CLARITY", GetType(String))
            .Add("SHAPE", GetType(String))
            .Add("SETTYPE", GetType(String))
            .Add("HEIGHT", GetType(Decimal))
            .Add("WIDTH", GetType(Decimal))
        End With
        dtWStoneDetails.Columns("KEYNO").AutoIncrement = True
        dtWStoneDetails.Columns("KEYNO").AutoIncrementStep = 1
        dtWStoneDetails.Columns("KEYNO").AutoIncrementSeed = 1
        wgridStone.DataSource = dtWStoneDetails
        wgridStone.Columns("USRATE").Visible = False
        wgridStone.Columns("INDRS").Visible = False
        wgridStone.Columns("CUT").Visible = False
        wgridStone.Columns("COLOR").Visible = False
        wgridStone.Columns("CLARITY").Visible = False
        wgridStone.Columns("SHAPE").Visible = False
        wgridStone.Columns("SETTYPE").Visible = False
        wgridStone.Columns("HEIGHT").Visible = False
        wgridStone.Columns("WIDTH").Visible = False
        WStyleGridStone()
        dtWStoneDetails.AcceptChanges()
        Dim dtWStoneFooter As New DataTable
        dtWStoneFooter = dtWStoneDetails.Copy
        dtWStoneFooter.Rows.Clear()
        dtWStoneFooter.Rows.Add()
        dtwStoneDetails.AcceptChanges()
        wgridStoneFooter.DataSource = dtWStoneFooter

        With wgridStoneFooter
            .Rows(0).Cells("SUBITEM").Value = "TOTAL"
            For cnt As Integer = 0 To wgridStone.ColumnCount - 1
                .Columns(cnt).Width = wgridStone.Columns(cnt).Width
                .Columns(cnt).Visible = wgridStone.Columns(cnt).Visible
                .Columns(cnt).DefaultCellStyle = wgridStone.Columns(cnt).DefaultCellStyle
            Next
        End With

        'Tag Stone
        With dtStoneDetails.Columns
            .Add("PACKETNO", GetType(String))
            .Add("ITEM", GetType(String))
            .Add("SUBITEM", GetType(String))
            .Add("PCS", GetType(Int32))
            .Add("WEIGHT", GetType(Decimal))
            .Add("UNIT", GetType(String))
            .Add("CALC", GetType(String))
            .Add("RATE", GetType(Decimal))
            .Add("AMOUNT", GetType(Decimal))
            .Add("METALID", GetType(String))
            .Add("PURRATE", GetType(Decimal))
            .Add("PURVALUE", GetType(Decimal))
            .Add("KEYNO", GetType(Integer))
            .Add("STNSNO", GetType(String))
            .Add("USRATE", GetType(Decimal))
            .Add("INDRS", GetType(Decimal))
            .Add("CUT", GetType(String))
            .Add("COLOR", GetType(String))
            .Add("CLARITY", GetType(String))
            .Add("SHAPE", GetType(String))
            .Add("SETTYPE", GetType(String))
            .Add("HEIGHT", GetType(Decimal))
            .Add("WIDTH", GetType(Decimal))
        End With
        dtStoneDetails.Columns("KEYNO").AutoIncrement = True
        dtStoneDetails.Columns("KEYNO").AutoIncrementStep = 1
        dtStoneDetails.Columns("KEYNO").AutoIncrementSeed = 1
        gridStone.DataSource = dtStoneDetails
        gridStone.Columns("USRATE").Visible = False
        gridStone.Columns("INDRS").Visible = False
        gridStone.Columns("CUT").Visible = False
        gridStone.Columns("COLOR").Visible = False
        gridStone.Columns("CLARITY").Visible = False
        gridStone.Columns("SHAPE").Visible = False
        gridStone.Columns("SETTYPE").Visible = False
        gridStone.Columns("HEIGHT").Visible = False
        gridStone.Columns("WIDTH").Visible = False
        StyleGridStone()
        dtStoneDetails.AcceptChanges()
        Dim dtStoneFooter As New DataTable
        dtStoneFooter = dtStoneDetails.Copy
        dtStoneFooter.Rows.Clear()
        dtStoneFooter.Rows.Add()
        dtStoneDetails.AcceptChanges()
        gridStoneFooter.DataSource = dtStoneFooter

        With gridStoneFooter
            .Rows(0).Cells("SUBITEM").Value = "TOTAL"
            For cnt As Integer = 0 To gridStone.ColumnCount - 1
                .Columns(cnt).Width = gridStone.Columns(cnt).Width
                .Columns(cnt).Visible = gridStone.Columns(cnt).Visible
                .Columns(cnt).DefaultCellStyle = gridStone.Columns(cnt).DefaultCellStyle
            Next
        End With

        'Web Tag OtherCharges
        With dtWMiscDetails.Columns
            .Add("MISC", GetType(String))
            .Add("AMOUNT", GetType(Double))
            .Add("PURAMOUNT", GetType(Double))
            .Add("KEYNO", GetType(Integer))
            .Add("STNSNO", GetType(String))
        End With
        dtWMiscDetails.Columns("KEYNO").AutoIncrement = True
        dtwMiscDetails.Columns("KEYNO").AutoIncrementStep = 1
        dtwMiscDetails.Columns("KEYNO").AutoIncrementSeed = 1
        wgridMisc.DataSource = dtWMiscDetails
        WStyleGridMisc()

        Dim dtWMiscFooter As New DataTable
        dtWMiscFooter = dtWMiscDetails.Copy
        dtWMiscFooter.Rows.Clear()
        dtwMiscFooter.Rows.Add()
        dtWMiscFooter.Rows(0).Item("MISC") = "TOTAL"
        dtwMiscDetails.AcceptChanges()
        wgridMiscFooter.DataSource = dtWMiscFooter
        With wgridMiscFooter
            For cnt As Integer = 0 To wgridMisc.ColumnCount - 1
                .Columns(cnt).Width = wgridMisc.Columns(cnt).Width
                .Columns(cnt).Visible = wgridMisc.Columns(cnt).Visible
                .Columns(cnt).DefaultCellStyle = wgridMisc.Columns(cnt).DefaultCellStyle
            Next
        End With

        'Tag OtherCharges
        With dtMiscDetails.Columns
            .Add("MISC", GetType(String))
            .Add("AMOUNT", GetType(Double))
            .Add("PURAMOUNT", GetType(Double))
            .Add("KEYNO", GetType(Integer))
            .Add("STNSNO", GetType(String))
        End With
        dtMiscDetails.Columns("KEYNO").AutoIncrement = True
        dtMiscDetails.Columns("KEYNO").AutoIncrementStep = 1
        dtMiscDetails.Columns("KEYNO").AutoIncrementSeed = 1
        gridMisc.DataSource = dtMiscDetails
        StyleGridMisc()

        Dim dtMiscFooter As New DataTable
        dtMiscFooter = dtMiscDetails.Copy
        dtMiscFooter.Rows.Clear()
        dtMiscFooter.Rows.Add()
        dtMiscFooter.Rows(0).Item("MISC") = "TOTAL"
        dtMiscDetails.AcceptChanges()
        gridMiscFooter.DataSource = dtMiscFooter
        With gridMiscFooter
            For cnt As Integer = 0 To gridMisc.ColumnCount - 1
                .Columns(cnt).Width = gridMisc.Columns(cnt).Width
                .Columns(cnt).Visible = gridMisc.Columns(cnt).Visible
                .Columns(cnt).DefaultCellStyle = gridMisc.Columns(cnt).DefaultCellStyle
            Next
        End With

        'Additional Entry
        With DtAddStockEntry.Columns
            .Add("OTHID", GetType(Integer))
            .Add("OTHNAME", GetType(String))
            .Add("MISCID", GetType(Integer))
            .Add("KEYNO", GetType(Integer))

        End With
        DtAddStockEntry.Columns("KEYNO").AutoIncrement = True
        DtAddStockEntry.Columns("KEYNO").AutoIncrementStep = 1
        DtAddStockEntry.Columns("KEYNO").AutoIncrementSeed = 1

        'For Web tag
        wcmbStUnit.Items.Add("C")
        wcmbStUnit.Items.Add("G")
        wcmbStUnit.Text = "C"
        wcmbStCalc.Items.Add("W")
        wcmbStCalc.Items.Add("P")
        wcmbStCalc.Text = "W"
        'For Tag
        cmbStUnit.Items.Add("C")
        cmbStUnit.Items.Add("G")
        cmbStUnit.Text = "C"
        cmbStCalc.Items.Add("W")
        cmbStCalc.Items.Add("P")
        cmbStCalc.Text = "W"

        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ISNULL(STUDDED,'') <> 'S' AND ACTIVE = 'Y' AND STOCKTYPE='T' ORDER BY ITEMNAME"
        objGPack.FillCombo(strSql, wcmbItem_MAN, , False)
        objGPack.FillCombo(strSql, cmbItem_MAN, , False)

        ''CostCentre Checking..
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE'"
        If UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", "Y", tran)) = "N" Then
            cmbCostCentre_Man.Items.Clear()
            cmbCostCentre_Man.Text = ""
            cmbCostCentre_Man.Enabled = False
        End If

        ' ''Min McWastage Visibility
        'strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'MINMCTAB'"
        'If UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", "Y", tran)) = "Y" Then pnlMin.Enabled = True Else pnlMin.Enabled = False

        ''Set MaxWastage Focus
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'MAXMCFOCUS'"
        If UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", "Y", tran)) = "Y" Then pnlMax.Enabled = True Else pnlMax.Enabled = False

        'Set AttachImag btn Visible
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'ATTACHIMAGE'"
        If UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", "Y", tran)) = "Y" Then btnAttachImage.Enabled = True Else btnAttachImage.Enabled = False

        ''Sets default Paths
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICPATH'"
        defalutDestination = UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", , tran))
        If Not defalutDestination.EndsWith("\") And defalutDestination <> "" Then defalutDestination += "\"
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICSOURCEPATH'"
        defalutSourcePath = UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", , tran))

        ''TagNo Gen
        Dim ds As New Data.DataSet
        ds.Clear()
        strSql = " SELECT CTLTEXT  FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'TAGNOGEN'"
        TagNoGen = objGPack.GetSqlValue(strSql, "CTLTEXT", , tran)

        ''CALMODE //WHEATHER GRSWT BASE OR NETWT BASE
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'CALCMODE'"
        CalMode = objGPack.GetSqlValue(strSql, "CTLTEXT", , tran)

        funcNew()
        If WfuncNew() = 0 Then Exit Sub
    End Sub
#Region "WebTag Function"
#Region "Caluculation Procedures"

    Private Sub WCalcLessWt()
        'funcStoneDetailsClear()
        'funcDiamondDetailsClear()
        'funcPreciousDetailsClear()
        If studdedStone <> "Y" Then Exit Sub
        Dim diaCaratWt As Double = 0
        Dim diaGramWt As Double = 0
        Dim diaPcs As Integer = 0
        Dim diaAmt As Double = 0

        Dim preCaratWt As Double = 0
        Dim preGramWt As Double = 0
        Dim prePcs As Integer = 0
        Dim preAmt As Double = 0

        Dim stoCaratWt As Double = 0
        Dim stoGramWt As Double = 0
        Dim stoPcs As Integer = 0
        Dim stoAmt As Double = 0

        If WTabControl1.TabPages.Contains(WtabStone) Then
            For cnt As Integer = 0 To wgridStone.RowCount - 1
                With wgridStone.Rows(cnt)
                    Select Case .Cells("METALID").Value.ToString
                        Case "D"
                            diaPcs += Val(.Cells("PCS").Value.ToString)
                            diaAmt += Val(.Cells("AMOUNT").Value.ToString)
                        Case "S"
                            stoPcs += Val(.Cells("PCS").Value.ToString)
                            stoAmt += Val(.Cells("AMOUNT").Value.ToString)
                        Case "P"
                            prePcs += Val(.Cells("PCS").Value.ToString)
                            preAmt += Val(.Cells("AMOUNT").Value.ToString)
                    End Select
                    Select Case .Cells("UNIT").Value.ToString
                        Case "G"
                            If .Cells("METALID").Value.ToString = "S" Then
                                stoGramWt += Val(.Cells("WEIGHT").Value.ToString)
                            ElseIf .Cells("METALID").Value.ToString = "P" Then
                                preGramWt += Val(.Cells("WEIGHT").Value.ToString)
                            ElseIf .Cells("METALID").Value.ToString = "D" Then
                                diaGramWt += Val(.Cells("WEIGHT").Value.ToString)
                            End If
                        Case "C"
                            If .Cells("METALID").Value.ToString = "S" Then
                                stoCaratWt += Val(.Cells("WEIGHT").Value.ToString)
                            ElseIf .Cells("METALID").Value.ToString = "P" Then
                                preCaratWt += Val(.Cells("WEIGHT").Value.ToString)
                            ElseIf .Cells("METALID").Value.ToString = "D" Then
                                diaCaratWt += Val(.Cells("WEIGHT").Value.ToString)
                            End If
                    End Select
                End With
            Next
        End If
        Dim mStudWtDedut As String
        If StudWtDedut = "I" Then
            If cmbSubItem_Man.Text <> "" Then
                strSql = " SELECT STUDDEDUCT FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')"
            Else
                strSql = " SELECT STUDDEDUCT  FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"
            End If
            mStudWtDedut = objGPack.GetSqlValue(strSql).ToString.ToUpper
        Else
            mStudWtDedut = StudWtDedut
        End If

        Dim lessWt As Double = Nothing
        If mStudWtDedut.Contains("D") Then
            lessWt += (diaCaratWt / 5) + diaGramWt
        End If
        If mStudWtDedut.Contains("S") Then
            lessWt += (stoCaratWt / 5) + stoGramWt
        End If
        If mStudWtDedut.Contains("P") Then
            lessWt += (preCaratWt / 5) + preGramWt
        End If
        If mStudWtDedut.Contains("N") Then
            lessWt = 0
        End If
        'lessWt = (diaCaratWt + stoCaratWt + preCaratWt) / 5 + (diaGramWt + stoGramWt + preGramWt)
        WtxtLessWt_Wet.Text = IIf(lessWt <> 0, Format(lessWt, "0.000"), "")
    End Sub

    Private Sub WCalcNetWt()
        Dim wt As Double = Nothing
        wt = Val(WtxtGrossWt_Wet.Text) - Val(WtxtLessWt_Wet.Text)
        strSql = "SELECT ISNULL(NETWTPER,0) NETWTPER FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wcmbItem_MAN.Text & "'"
        Dim mnetwtper As Double = Val(objGPack.GetSqlValue(strSql).ToString)
        If mnetwtper <> 0 Then wt = wt * (mnetwtper / 100)
        WtxtNetWt_Wet.Text = IIf(wt <> 0, Format(wt, "0.000"), "")
    End Sub

    Private Sub WCalcFinalTotal(Optional ByVal salevalueblock As Boolean = False)
        WCalcNetWt()
        WCalcStoneTotals()

        WCalcMiscTotalAmount()

        If Not salevalueblock Then WCalcSaleValue()

    End Sub

    Private Sub WCalcSaleValue()
        Dim amt As Double = Nothing
        'If calType = "F" Then Exit Sub
        If calType = "R" Then
            amt = Val(wtxtPieces_Num_Man.Text) * Val(wtxtMetalRate_Amt.Text)
        Else
            Dim wt As Double = 0
            Dim rate As Double = Val(wtxtMetalRate_Amt.Text) 'IIf(calType = "M", Val(txtRate_Amt.Text), )

            wt = Val(WtxtNetWt_Wet.Text)


            If WTabControl1.Contains(WtabMultiMetal) And multiMetalCalc Then
                amt = 0
                If GetSoftValue("MULTIMETALCALC") = "N" Then
                    GoTo WegithCalc
                End If
                For Each ro As DataRow In dtWMultiMetalDetails.Rows
                    If Not Val(ro!AMOUNT.ToString) > 0 Then
                        amt += (Val(ro!WEIGHT.ToString) + Val(ro!WASTAGE.ToString)) * Val(ro!RATE.ToString)
                        amt += Val(ro!MC.ToString)
                    End If
                    amt += Val(ro!AMOUNT.ToString)
                Next

            Else
WegithCalc:
                If VALUECALC_GRNETOPT Then
                    If _VALUECALCON_ITEM_GRS Then wt = Val(WtxtGrossWt_Wet.Text) Else wt = Val(WtxtNetWt_Wet.Text)
                End If
                amt = ((wt * rate) _
                + Val(wtxtMkCharge_Amt.Text) _
                + Val(wgridStoneFooter.Rows(0).Cells("AMOUNT").Value.ToString))

            End If
        End If
        amt = Math.Round(amt)
        wtxtSalValue_Amt_Man.Text = IIf(amt <> 0, Format(WSALEVALUE_ROUND(amt), "0.00"), "")
        If SALEVALUEPLUS <> 0 Then wtxtSalValue_Amt_Man.Text = Val(wtxtSalValue_Amt_Man.Text) * SALEVALUEPLUS
        ObjPurDetail.CalcPurchaseValue()

    End Sub

#End Region

    Private Function WGetMetalRate() As Double
        Dim purityId As String = Nothing
        ''objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = " & saItemTypeId & " AND RATEGET = 'Y'", , )
        If wcmbPurity.Text <> "" Then
            purityId = objGPack.GetSqlValue("SELECT PURITYID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & wcmbPurity.Text & "' AND RATEGET = 'Y' AND SOFTMODULE = 'S'", , )
        End If
        If Not Trim(purityId).Length > 0 Then
            purityId = objGPack.GetSqlValue("SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wcmbItem_MAN.Text & "')")
        End If
        If purityId = "" Then Return 0
        Dim metalId As String = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = '" & purityId & "'")

        Dim rate As Double = Nothing
        strSql = " SELECT TOP 1 SRATE FROM " & cnAdminDb & "..RATEMAST AS M"
        strSql += " WHERE RDATE = '" & wdtpRecieptDate.Value & "'"
        strSql += " AND RATEGROUP = (SELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = M.RDATE)"
        strSql += " AND METALID = '" & metalId & "'"
        strSql += " AND PURITY = (SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = '" & purityId & "')"
        strSql += " ORDER BY SNO DESC"
        rate = Val(objGPack.GetSqlValue(strSql, , , tran))
        If IsDate(wdtpRecieptDate.Value) Then
            Return rate
        Else
            Return 0
        End If
    End Function

    Function WfuncNew() As Integer
        If GetAdmindbSoftValue("SYNC-TO", , tran) <> "" Then
            If GetAdmindbSoftValue("BRANCHTAG", "Y", tran) = "N" Then
                MsgBox("Taged entry cannot allow at location", MsgBoxStyle.Information)

                If Me.Focused = True Then Me.Close()
                ItemLock = False
                Return 0

                Exit Function
            End If
        End If
        TabGeneral.SelectedTab = tabWebTag
        Ratevaluezero = False
        ObjRsUs.txtIndRs_Amt.Clear()
        ObjRsUs.txtUSDollar_Amt.Clear()
        ObjOrderTagInfo = New TagOrderInfo
        ObjPurDetail = New TagPurchaseDetailEntry
        ObjMinValue = New TagMinValues
        ObjExtraWt = New frmExtaWt

        'txtRate_Amt.ReadOnly = False
        ItemLock = False
        objGPack.TextClear(Me)
        isStyleCode = False
        flagPurValSet = False
        SNO = Nothing
        entryOrder = Nothing
        METALID = Nothing
        studdedStone = Nothing
        grossnetdiff = Nothing
        sizeStock = Nothing
        multiMetal = Nothing
        OthCharge = Nothing
        calType = Nothing
        noOfPiece = Nothing
        pieceRate = Nothing

        SubItemPic = False
        mfromItemid = 0
        wtxtMcPerGrm_Amt.Text = ""
        wtxtMkCharge_Amt.Text = ""

        Studded_Loose = ""
        dtWMultiMetalDetails.Rows.Clear()
        dtWStoneDetails.Rows.Clear()
        dtWMiscDetails.Rows.Clear()
        With wgridStoneFooter.Rows(0)
            .Cells("PCS").Value = DBNull.Value
            .Cells("WEIGHT").Value = DBNull.Value
            .Cells("AMOUNT").Value = DBNull.Value
        End With
        wgridMiscFooter.Rows(0).Cells("AMOUNT").Value = DBNull.Value
        ''Pieces

        WbtnSave.Enabled = True
        TChkbStk = True
        AddStockEntry = True

        wGridDia.DataSource = Nothing
        wGridStn.DataSource = Nothing
        wGridSummary.DataSource = Nothing
        WFuncLoadAddMaster()

        Tagsave = False
        tagWebtagDiff = False
        strSql = " SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE ORDER BY dispalyorder,NAME"
        objGPack.FillCombo(strSql, wcmbPurity, , False)
        If wcmbPurity.Items.Count > 0 Then wcmbPurity.Enabled = True Else wcmbPurity.Enabled = False

        WHideSearch()

        WTabControl1.TabPages.Remove(WtabMultiMetal)
        WTabControl1.TabPages.Remove(WtabStone)
        WTabControl1.TabPages.Remove(WtabOtherCharges)
        WTabControl1.TabPages.Remove(WtabPurchase)
        wtxtSalValue_Amt_Man.Clear()
        If STKAFINDATE = False Then
            wdtpRecieptDate.Value = GetEntryDate(GetServerDate)
        Else
            wdtpRecieptDate.MinimumDate = (New DateTimePicker).MinDate
        End If
        MLasttagno = 0
        MTagprefix = ""
        wdtpRecieptDate.Select()
        Return 1
    End Function
    Function WFuncLoadAddMaster()
        If Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE MISCID=1 ", "", "", )) > 0 Then
            strSql = " SELECT NAME FROM " & cnAdminDb & "..OTHERMASTER WHERE MISCID=1"
            Dim dtAdd As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtAdd)
            objGPack.FillCombo(strSql, wcmbAddM1_OWN, True, False)
            BrighttechPack.GlobalMethods.FillCombo(wChkcmbAddM1, dtAdd, "NAME")
            wlblAddM1.Text = objGPack.GetSqlValue("SELECT MISCNAME FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE MISCID=1 ", "", "", )
        Else
            wcmbAddM1_OWN.Enabled = False
            wChkcmbAddM1.Enabled = False
        End If
        If Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE MISCID=2 ", "", "", )) > 0 Then
            strSql = " SELECT NAME FROM " & cnAdminDb & "..OTHERMASTER WHERE MISCID=2"
            objGPack.FillCombo(strSql, wcmbAddM2_OWN, True, False)
            Dim dtAdd As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtAdd)
            objGPack.FillCombo(strSql, wcmbAddM2_OWN, True, False)
            BrighttechPack.GlobalMethods.FillCombo(wChkcmbAddM2, dtAdd, "NAME")
            wlblAddM2.Text = objGPack.GetSqlValue("SELECT MISCNAME FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE MISCID=2 ", "", "", )
        Else
            wcmbAddM2_OWN.Enabled = False
            wChkcmbAddM2.Enabled = False
        End If
        If Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE MISCID=3 ", "", "", )) > 0 Then
            strSql = " SELECT NAME FROM " & cnAdminDb & "..OTHERMASTER WHERE MISCID=3"
            Dim dtAdd As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtAdd)
            'objGPack.FillCombo(strSql, wcmbAddM3, True, False)
            BrighttechPack.GlobalMethods.FillCombo(wChkcmbAddM3, dtAdd, "NAME")
            wLblAddM3.Text = objGPack.GetSqlValue("SELECT MISCNAME FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE MISCID=3 ", "", "", )
        Else
            wChkcmbAddM3.Enabled = False
        End If
        If Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE MISCID=4 ", "", "", )) > 0 Then
            strSql = " SELECT NAME FROM " & cnAdminDb & "..OTHERMASTER WHERE MISCID=4"
            Dim dtAdd As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtAdd)
            'objGPack.FillCombo(strSql, wcmbAddM4, True, False)
            BrighttechPack.GlobalMethods.FillCombo(wChkcmbAddM4, dtAdd, "NAME")
            wLblAddM4.Text = objGPack.GetSqlValue("SELECT MISCNAME FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE MISCID=4 ", "", "", )
        Else
            wChkcmbAddM4.Enabled = False
        End If
        If Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE MISCID=5 ", "", "", )) > 0 Then
            strSql = " SELECT NAME FROM " & cnAdminDb & "..OTHERMASTER WHERE MISCID=5"
            Dim dtAdd As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtAdd)
            'objGPack.FillCombo(strSql, wcmbAddM5, True, False)
            BrighttechPack.GlobalMethods.FillCombo(wChkcmbAddM5, dtAdd, "NAME")
            wlblAddM5.Text = objGPack.GetSqlValue("SELECT MISCNAME FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE MISCID=5 ", "", "", )
        Else
            wChkcmbAddM5.Enabled = False
        End If
        If Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE MISCID=6 ", "", "", )) > 0 Then
            strSql = " SELECT NAME FROM " & cnAdminDb & "..OTHERMASTER WHERE MISCID=6"
            Dim dtAdd As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtAdd)
            'objGPack.FillCombo(strSql, wcmbAddM6, True, False)
            BrighttechPack.GlobalMethods.FillCombo(wChkcmbAddM6, dtAdd, "NAME")
            wlblAddM6.Text = objGPack.GetSqlValue("SELECT MISCNAME FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE MISCID=6 ", "", "", )
        Else
            wChkcmbAddM6.Enabled = False
        End If
        If Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE MISCID=7 ", "", "", )) > 0 Then
            strSql = " SELECT NAME FROM " & cnAdminDb & "..OTHERMASTER WHERE MISCID=7"
            Dim dtAdd As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtAdd)
            'objGPack.FillCombo(strSql, wcmbAddM7, True, False)
            BrighttechPack.GlobalMethods.FillCombo(wChkcmbAddM7, dtAdd, "NAME")
            wLblAddM7.Text = objGPack.GetSqlValue("SELECT MISCNAME FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE MISCID=7 ", "", "", )
        Else
            wChkcmbAddM7.Enabled = False
        End If
        If Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE MISCID=8 ", "", "", )) > 0 Then
            strSql = " SELECT NAME FROM " & cnAdminDb & "..OTHERMASTER WHERE MISCID=8"
            Dim dtAdd As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtAdd)
            'objGPack.FillCombo(strSql, wcmbAddM8, True, False)
            BrighttechPack.GlobalMethods.FillCombo(wChkcmbAddM8, dtAdd, "NAME")
            wlblAddM8.Text = objGPack.GetSqlValue("SELECT MISCNAME FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE MISCID=8 ", "", "", )
        Else
            wChkcmbAddM8.Enabled = False
        End If
        If Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE MISCID=9 ", "", "", )) > 0 Then
            strSql = " SELECT NAME FROM " & cnAdminDb & "..OTHERMASTER WHERE MISCID=9"
            Dim dtAdd As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtAdd)
            'objGPack.FillCombo(strSql, wcmbAddM9, True, False)
            BrighttechPack.GlobalMethods.FillCombo(wChkcmbAddM9, dtAdd, "NAME")
            wlblAddM9.Text = objGPack.GetSqlValue("SELECT MISCNAME FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE MISCID=9 ", "", "", )
        Else
            wChkcmbAddM9.Enabled = False
        End If
        If Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE MISCID=10 ", "", "", )) > 0 Then
            strSql = " SELECT NAME FROM " & cnAdminDb & "..OTHERMASTER WHERE MISCID=10"
            Dim dtAdd As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtAdd)
            'objGPack.FillCombo(strSql, wcmbAddM10, True, False)
            BrighttechPack.GlobalMethods.FillCombo(wChkcmbAddM10, dtAdd, "NAME")
            wlblAddM10.Text = objGPack.GetSqlValue("SELECT MISCNAME FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE MISCID=10 ", "", "", )
        Else
            wChkcmbAddM10.Enabled = False
        End If
        If Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE MISCID=11 ", "", "", )) > 0 Then
            strSql = " SELECT NAME FROM " & cnAdminDb & "..OTHERMASTER WHERE MISCID=11"
            Dim dtAdd As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtAdd)
            'objGPack.FillCombo(strSql, wcmbAddM11, True, False)
            BrighttechPack.GlobalMethods.FillCombo(wChkcmbAddM11, dtAdd, "NAME")
            wlblAddM11.Text = objGPack.GetSqlValue("SELECT MISCNAME FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE MISCID=11 ", "", "", )
        Else
            wChkcmbAddM11.Enabled = False
        End If
        If Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE MISCID=12 ", "", "", )) > 0 Then
            strSql = " SELECT NAME FROM " & cnAdminDb & "..OTHERMASTER WHERE MISCID=12"
            Dim dtAdd As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtAdd)
            'objGPack.FillCombo(strSql, wcmbAddM12, True, False)
            BrighttechPack.GlobalMethods.FillCombo(wChkcmbAddM12, dtAdd, "NAME")
            wlblAddM12.Text = objGPack.GetSqlValue("SELECT MISCNAME FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE MISCID=12 ", "", "", )
        Else
            wChkcmbAddM12.Enabled = False
        End If
    End Function
    Private Function WGetidwithlen(ByVal CItemid As String, ByVal len As Integer) As String
        If CItemid = Nothing Then CItemid = ""
        Dim RetStr As String = ""
        If len = 0 Then len = 1
        For cnt As Integer = 1 To len - CItemid.ToString.Length
            RetStr += "0"
        Next
        RetStr += CItemid
        Return RetStr
    End Function
    Function wfuncAdd() As String
        Dim RowFiter() As DataRow = Nothing
        Dim TagSno As String = Nothing
        Dim TagNo As String = Nothing
        Dim COSTID As String = Nothing
        Dim itemId As Integer = Nothing
        Dim subitemId As Integer = Nothing
        Dim sizeId As Integer = Nothing
        Dim itemCtrId As Integer = Nothing
        Dim designerId As Integer = Nothing
        Dim tagVal As Integer = 0
        'Dim saleMode As String = Nothing
        Dim itemTypeId As Integer = Nothing
        Dim tranInvNo As String = Nothing
        Dim supBillno As String = Nothing

        Dim stlPcs As Integer = 0
        Dim stlWt As Double = 0
        Dim stlType As String = Nothing

        Dim dialPcs As Integer = 0
        Dim dialWt As Double = 0
        Try
            Dim XToCostid As String = GetAdmindbSoftValue("SYNC-TO", "", tran)
            Dim XBranchtag As Boolean = IIf(GetAdmindbSoftValue("BRANCHTAG", "", tran) = "Y", True, False)

            ''Find ItemId
            strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & wcmbItem_MAN.Text & "'"
            itemId = Val(objGPack.GetSqlValue(strSql, "ITEMID", , tran))

            ''Find itemTypeId
            strSql = " SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & wcmbPurity.Text & "'"
            itemTypeId = Val(objGPack.GetSqlValue(strSql, "ITEMTYPEID", , tran))

            If WcmbSubItem_OWN.Enabled = True Then
                ''Find SubItemId
                strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & WcmbSubItem_OWN.Text & "' AND ITEMID = " & itemId & ""
                subitemId = Val(objGPack.GetSqlValue(strSql, "SUBITEMID", , tran))
            End If
            If WchkcmbSubItem.Enabled = True Then
                ''Find SubItemId
                Dim SubItem() As String = WchkcmbSubItem.Text.Split(",")
                strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & SubItem(0) & "' AND ITEMID = " & itemId & ""
                subitemId = Val(objGPack.GetSqlValue(strSql, "SUBITEMID", , tran))
            End If
            Dim strSubitemids As String = GetSelectedSubitemid(WchkcmbSubItem, False)
            Dim mlwmctype As String = objGPack.GetSqlValue("SELECT WMCTYPE FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = '" & SNO & "'", , , tran)
            tran = Nothing
            tran = cn.BeginTransaction()
            ' ''Find TagSno
            'GETNTAGSNO:
            '            'TagSno = GetNewSno(TranSnoType.ITEMTAGCODE, tran, "GET_TRANSNO_ADMIN")
            TagSno = GetNewSno(TranSnoType.WITEMTAGCODE, tran, "GET_ADMINSNO_TRAN")
            'TagDupGen:
            '            Dim tagPrefix As String
            '            Dim SEAL As String = ""
            '            Dim ItemSname As String = ""
            '            Dim Styleno As String = ""
            '            Dim uniqueid As String = ""
            '            SEAL = objGPack.GetSqlValue("SELECT SUBSTRING(SEAL,1,2) SEAL FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID=" & designerId & "", , "00", tran)
            '            ItemSname = objGPack.GetSqlValue("SELECT SUBSTRING(SHORTNAME,1,2) SNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=" & itemId & "", , "00", tran)
            '            Styleno = "0" '.Cells("STYLENO").Value.ToString

            '            Dim stnSHAPEid, stnColorid, stnClarityid As Integer
            '            stnSHAPEid = 0 : stnColorid = 0 : stnClarityid = 0
            '            Dim drsstone() As DataRow = dtWStoneDetails.Select("", "")
            '            If drsstone.Length > 0 Then
            '                stnSHAPEid = Val(objGPack.GetSqlValue("SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME='" & drsstone(0).Item("SHAPE").ToString & "' ", , 0, tran).ToString)
            '                stnColorid = Val(objGPack.GetSqlValue("SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME='" & drsstone(0).Item("COLOR").ToString & "' ", , 0, tran).ToString)
            '                stnClarityid = Val(objGPack.GetSqlValue("SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME='" & drsstone(0).Item("CLARITY").ToString & "' ", , 0, tran).ToString)
            '            End If
            '            strSql = "SELECT TOP 1 UNIQUEID FROM " & cnAdminDb & "..DIASTYLE "
            '            strSql += " WHERE SHAPEID = " & stnSHAPEid & ""
            '            strSql += " AND COLORID = " & stnColorid & ""
            '            strSql += " AND CLARITYID = " & stnClarityid & ""
            '            uniqueid = objGPack.GetSqlValue(strSql, , "0", tran)

            '            tagPrefix = SEAL & WGetidwithlen(Styleno, 6) & uniqueid
            '            strSql = " SELECT LASTNO FROM " & cnAdminDb & "..DIAUNIQUETAG WHERE UNQDESC='" & tagPrefix & "'"
            '            LastTagNo = objGPack.GetSqlValue(strSql, , "0", tran)
            '            If LastTagNo = 0 Then
            '                strSql += " INSERT INTO " & cnAdminDb & "..DIAUNIQUETAG(UNQDESC,LASTNO) "
            '                strSql += " VALUES('" & tagPrefix & "',1)"
            '                'ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostCenterId)
            '                cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
            '                LastTagNo = LastTagNo + 1
            '            Else
            '                LastTagNo = LastTagNo + 1
            '                strSql = " UPDATE " & cnAdminDb & "..DIAUNIQUETAG SET LASTNO =" & LastTagNo & " WHERE UNQDESC='" & tagPrefix & "'"
            '                'ExecQuery(SyncMode.Transaction, strSql, cn, tran, CostCenterId)
            '                cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
            '            End If
            '            WtxtTagNo__Man.Text = tagPrefix & WGetidwithlen(LastTagNo.ToString, 3)

            TagNo = wtxtTagNo__Man.Text

            ''Find Stone & Diamond
            If WTabControl1.TabPages.Contains(WtabStone) = True Then
                'stlPcs += Val(lblStnPcs.Text)
                'stlWt += (Val(lblStnCaratWt.Text) / 5) + Val(lblStnGramWt.Text)

                'stlPcs += Val(lblPrePcs.Text)
                'stlWt += (Val(lblPreCarat.Text) / 5) + Val(lblPreGram.Text)

                'dialPcs += Val(lblDiaPcs.Text)
                'dialWt += (Val(lblDiaCarat.Text) / 5) + Val(lblDiaGram.Text)

                stlType = "G"
                stlWt = Math.Round(stlWt, 3)
                dialWt = Math.Round(dialWt, 3)
            Else
                stlPcs = 0
                stlWt = 0
                stlType = "G"
            End If



            ''Find TagVal
            tagVal = objTag.GetTagVal(TagNo)



            Dim purStoneValue As Double
            For Each roStn As DataRow In dtWStoneDetails.Rows
                purStoneValue += Val(roStn!PURVALUE.ToString)
            Next

            Dim orderRepNo As String = Nothing
            Dim orderRepSno As String = Nothing
            If Not OrderRow Is Nothing Then
                orderRepNo = OrderRow.Item("ORNO").ToString
                orderRepSno = OrderRow.Item("SNO").ToString

            ElseIf _CheckOrderInfo = False And ObjOrderTagInfo.txtOrderNo.Text <> "" Then
                Dim OrdBatchno As String = GetNewBatchno(cnCostId, wdtpRecieptDate.Value, tran)
                orderRepNo = GetCostId(COSTID) & GetCompanyId(GetStockCompId()) & ObjOrderTagInfo.txtOrderNo.Text
                orderRepSno = GetNewSno(TranSnoType.ORMASTCODE, tran, "GET_ADMINSNO_TRAN")

                ''Auto Generation in Ormast
                Dim DtWOrderMast As New DataTable
                DtWOrderMast = BrighttechPack.GlobalMethods.GetTableStructure(cnAdminDb, "ORMAST", cn, tran)
                Dim RowOrder As DataRow = DtWOrderMast.NewRow
                RowOrder.Item("SNO") = orderRepSno
                RowOrder.Item("ORNO") = orderRepNo
                RowOrder.Item("ORDATE") = wdtpRecieptDate.Value.ToString("yyyy-MM-dd")
                RowOrder.Item("REMDATE") = wdtpRecieptDate.Value.ToString("yyyy-MM-dd")
                RowOrder.Item("DUEDATE") = wdtpRecieptDate.Value.ToString("yyyy-MM-dd")
                RowOrder.Item("ORTYPE") = "O"
                RowOrder.Item("COMPANYID") = GetStockCompId()
                RowOrder.Item("ORRATE") = "C"
                RowOrder.Item("ORMODE") = "C"
                RowOrder.Item("ITEMID") = itemId
                RowOrder.Item("SUBITEMID") = subitemId
                RowOrder.Item("DESCRIPT") = wcmbItem_MAN.Text
                RowOrder.Item("PCS") = Val(wtxtPieces_Num_Man.Text)
                RowOrder.Item("GRSWT") = Val(WtxtGrossWt_Wet.Text)
                'RowOrder.Item("EXTRAWT") = Val(ObjExtraWt.txtExtraWt_WET.Text)
                RowOrder.Item("NETWT") = Val(WtxtNetWt_Wet.Text)
                RowOrder.Item("SIZEID") = sizeId
                RowOrder.Item("RATE") = Val(wtxtMetalRate_Amt.Text)
                RowOrder.Item("NATURE") = DBNull.Value
                RowOrder.Item("MCGRM") = Val(wtxtMcPerGrm_Amt.Text)
                RowOrder.Item("MC") = Val(wtxtMkCharge_Amt.Text)
                RowOrder.Item("WASTPER") = 0
                RowOrder.Item("WAST") = 0
                RowOrder.Item("COMMPER") = DBNull.Value
                RowOrder.Item("COMM") = DBNull.Value
                RowOrder.Item("OTHERAMT") = DBNull.Value
                RowOrder.Item("CANCEL") = DBNull.Value
                RowOrder.Item("ORVALUE") = Val(wtxtSalValue_Amt_Man.Text)
                RowOrder.Item("COSTID") = COSTID
                RowOrder.Item("BATCHNO") = OrdBatchno
                RowOrder.Item("CORNO") = DBNull.Value
                RowOrder.Item("PROPSMITH") = DBNull.Value
                RowOrder.Item("PICTFILE") = DBNull.Value

                RowOrder.Item("EMPID") = Val(ObjOrderTagInfo.txtEmpNo_NUM.Text)
                RowOrder.Item("UPDATED") = Today.Date.Now.ToString("yyyy-MM-dd")
                RowOrder.Item("UPTIME") = Date.Now.ToLongTimeString
                RowOrder.Item("USERID") = userId
                RowOrder.Item("ODBATCHNO") = DBNull.Value
                RowOrder.Item("ODSNO") = DBNull.Value
                RowOrder.Item("APPVER") = VERSION
                RowOrder.Item("REASON") = DBNull.Value
                RowOrder.Item("TAX") = DBNull.Value
                RowOrder.Item("SC") = DBNull.Value
                RowOrder.Item("ADSC") = DBNull.Value
                RowOrder.Item("ITEMTYPEID") = itemTypeId
                RowOrder.Item("SIZENO") = DBNull.Value
                RowOrder.Item("TRANSFERED") = DBNull.Value
                RowOrder.Item("STYLENO") = DBNull.Value
                RowOrder.Item("DISCOUNT") = DBNull.Value
                RowOrder.Item("ORDCANCEL") = DBNull.Value
                DtWOrderMast.Rows.Add(RowOrder)
                InsertData(SyncMode.Stock, DtWOrderMast, cn, tran, COSTID)

                Dim DtWOrIrDetail As New DataTable
                DtWOrIrDetail = BrighttechPack.GlobalMethods.GetTableStructure(cnAdminDb, "ORIRDETAIL", cn, tran)

                strSql = " SELECT 1 CNT FROM " & cnAdminDb & "..ORIRDETAIL WHERE ISNULL(ORNO,'') = '" & orderRepNo & "' AND ISNULL(ORSNO,'') = '" & orderRepSno & "' AND ISNULL(ORSTATUS,'') = 'I' AND ISNULL(CANCEL,'') = ''"
                Dim dtChk As New DataTable
                cmd = New OleDbCommand(strSql, cn, tran)
                da = New OleDbDataAdapter(cmd)
                da.Fill(dtChk)
                If dtChk.Rows.Count = 0 Then
                    RowOrder = Nothing
                    RowOrder = DtWOrIrDetail.NewRow
                    ''INSERTING IRDETAIL
                    RowOrder.Item("SNO") = GetNewSno(TranSnoType.ORIRDETAILCODE, tran, "GET_ADMINSNO_TRAN")
                    RowOrder.Item("ORSNO") = orderRepSno
                    RowOrder.Item("TRANNO") = GetBillNoValue("ORDIRBILLNO", tran)
                    RowOrder.Item("TRANDATE") = wdtpRecieptDate.Value
                    RowOrder.Item("DESIGNERID") = designerId
                    RowOrder.Item("PCS") = Val(wtxtPieces_Num_Man.Text)
                    RowOrder.Item("GRSWT") = Val(WtxtGrossWt_Wet.Text)
                    RowOrder.Item("NETWT") = Val(WtxtNetWt_Wet.Text)
                    RowOrder.Item("WASTAGE") = 0
                    RowOrder.Item("MC") = Val(wtxtMkCharge_Amt.Text)
                    RowOrder.Item("TAGNO") = wtxtTagNo__Man.Text
                    RowOrder.Item("ORSTATUS") = "I"
                    RowOrder.Item("CANCEL") = DBNull.Value
                    RowOrder.Item("COSTID") = COSTID
                    RowOrder.Item("DESCRIPT") = wcmbItem_MAN.Text
                    RowOrder.Item("ORNO") = orderRepNo
                    RowOrder.Item("BATCHNO") = OrdBatchno
                    RowOrder.Item("USERID") = userId
                    RowOrder.Item("UPDATED") = Today.Date.Now.ToString("yyyy-MM-dd")
                    RowOrder.Item("UPTIME") = Date.Now.ToLongTimeString
                    RowOrder.Item("APPVER") = VERSION
                    RowOrder.Item("COMPANYID") = GetStockCompId()
                    RowOrder.Item("TRANSFERED") = DBNull.Value
                    DtWOrIrDetail.Rows.Add(RowOrder)
                    InsertData(SyncMode.Stock, DtWOrIrDetail, cn, tran, COSTID)
                End If

                RowOrder = Nothing
                If DtWOrIrDetail.Rows.Count > 0 Then DtWOrIrDetail.Rows.Clear()
                RowOrder = DtWOrIrDetail.NewRow
                ''INSERTING IRDETAIL
                RowOrder.Item("SNO") = GetNewSno(TranSnoType.ORIRDETAILCODE, tran, "GET_ADMINSNO_TRAN")
                RowOrder.Item("ORSNO") = orderRepSno
                RowOrder.Item("TRANNO") = GetBillNoValue("ORDIRBILLNO", tran)
                RowOrder.Item("TRANDATE") = wdtpRecieptDate.Value
                RowOrder.Item("DESIGNERID") = designerId
                RowOrder.Item("PCS") = Val(wtxtPieces_Num_Man.Text)
                RowOrder.Item("GRSWT") = Val(WtxtGrossWt_Wet.Text)
                RowOrder.Item("NETWT") = Val(WtxtNetWt_Wet.Text)
                RowOrder.Item("WASTAGE") = 0
                RowOrder.Item("MC") = Val(wtxtMkCharge_Amt.Text)
                RowOrder.Item("TAGNO") = wtxtTagNo__Man.Text
                RowOrder.Item("ORSTATUS") = "R"
                RowOrder.Item("CANCEL") = DBNull.Value
                RowOrder.Item("COSTID") = COSTID
                RowOrder.Item("DESCRIPT") = wcmbItem_MAN.Text
                RowOrder.Item("ORNO") = orderRepNo
                RowOrder.Item("BATCHNO") = OrdBatchno
                RowOrder.Item("USERID") = userId
                RowOrder.Item("UPDATED") = Today.Date.Now.ToString("yyyy-MM-dd")
                RowOrder.Item("UPTIME") = Date.Now.ToLongTimeString
                RowOrder.Item("APPVER") = VERSION
                RowOrder.Item("COMPANYID") = GetStockCompId()
                RowOrder.Item("TRANSFERED") = DBNull.Value
                DtWOrIrDetail.Rows.Add(RowOrder)
                InsertData(SyncMode.Stock, DtWOrIrDetail, cn, tran, COSTID)
            End If



            ''INSERTING ITEMTAG
            strSql = " INSERT INTO " & cnAdminDb & "..WITEMTAG"
            strSql += " ("
            strSql += " SNO,RECDATE,COSTID,ITEMID,ORDREPNO,ORSNO,"
            strSql += " ORDSALMANCODE,SUBITEMID,SIZEID,ITEMCTRID,TABLECODE,DESIGNERID,"
            strSql += " TAGNO,PCS,GRSWT,"
            strSql += " LESSWT,NETWT,RATE,FINERATE,MAXWASTPER,MAXMCGRM,"
            strSql += " MAXWAST,MAXMC,MINWASTPER,MINMCGRM,MINWAST,MINMC,"
            strSql += " TAGKEY,TAGVAL,LOTSNO,COMPANYID,SALVALUE,PURITY,NARRATION,DESCRIP,"
            strSql += " REASON,ENTRYMODE,GRSNET,"
            strSql += " ISSDATE,ISSREFNO,ISSPCS,ISSWT,FROMFLAG,TOFLAG,APPROVAL,SALEMODE,"
            strSql += " BATCHNO,MARK,"
            strSql += " PCTPATH,PCTFILE,OLDTAGNO,ITEMTYPEID,ACTUALRECDATE,WEIGHTUNIT,"
            strSql += " TRANSFERWT,CHKDATE,CHKTRAY,CARRYFLAG,BRANDID,PRNFLAG,"
            strSql += " MCDISCPER,WASTDISCPER,RESDATE,TRANINVNO,SUPBILLNO,WORKDAYS,"
            strSql += " USERID,UPDATED,UPTIME,SYSTEMID,STYLENO,APPVER,TRANSFERDATE,"
            strSql += " BOARDRATE,RFID,TOUCH,HM_BILLNO,HM_CENTER,ADD_VA_PER,REFVALUE,VALUEADDEDTYPE,TCOSTID,EXTRAWT,"
            strSql += " USRATE,INDRS,"
            strSql += " RECSNO,FROMITEMID"
            If _FourCMaintain Then
                strSql += " ,CUTID,COLORID,CLARITYID,SHAPEID,SETTYPEID"
            End If
            strSql += ",HEIGHT,WIDTH ) VALUES("
            strSql += " '" & TagSno & "'" 'SNO
            strSql += " ,'" & GetEntryDate(wdtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE 'dtpRecieptDate.Value.Date.ToString("yyyy-MM-dd")
            strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
            strSql += " ," & itemId & "" 'ITEMID
            strSql += " ,'" & orderRepNo & "'" 'ORDREPNO
            strSql += " ,'" & orderRepSno & "'" 'ORsno
            strSql += " ,''" 'ORDSALMANCODE
            strSql += " ," & subitemId & "" 'SUBITEMID
            strSql += " ," & sizeId & "" 'SIZEID
            strSql += " ," & itemCtrId & "" 'ITEMCTRID
            strSql += " ,''"
            'If cmbTableCode.Enabled = True Then 'TABLECODE

            'Else
            '    strSql += " ,''"
            'End If
            strSql += " ," & Val(designerId) & "" 'DESIGNERID
            strSql += " ,'" & TagNo & "'" 'TAGNO
            strSql += " ," & Val(wtxtPieces_Num_Man.Text) & "" 'PCS
            strSql += " ," & Val(WtxtGrossWt_Wet.Text) & "" 'GRSWT
            strSql += " ," & Val(WtxtLessWt_Wet.Text) & "" 'LESSWT
            strSql += " ," & Val(WtxtNetWt_Wet.Text) & "" 'NETWT
            strSql += " ," & Val(wtxtMetalRate_Amt.Text) & "" 'RATE

            strSql += ",0" 'FINERATE
            strSql += " ,0" ' & Val(txtMaxWastage_Per.Text) & "" 'MAXWASTPER
            strSql += " ," & Val(wtxtMcPerGrm_Amt.Text) & "" 'MAXMCGRM
            strSql += " ,0" ' & Val(txtMaxWastage_Wet.Text) & "" 'MAXWAST
            strSql += " ," & Val(wtxtMkCharge_Amt.Text) & "" 'MAXMC
            strSql += " ," & Val(ObjMinValue.txtMinWastage_Per.Text) & "" 'MINWASTPER
            strSql += " ," & Val(ObjMinValue.txtMinMcPerGram_Amt.Text) & "" 'MINMCGRM
            strSql += " ," & Val(ObjMinValue.txtMinWastage_Wet.Text) & "" 'MINWAST
            strSql += " ," & Val(ObjMinValue.txtMinMkCharge_Amt.Text) & "" 'MINMC
            strSql += " ,'" & itemId.ToString & "" & TagNo & "'" 'TAGKEY
            strSql += " ," & tagVal & "" 'TAGVAL
            strSql += " ,'" & SNO & "'" 'LOTSNO
            strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
            strSql += " ," & Val(wtxtSalValue_Amt_Man.Text) & "" 'SALVALUE
            strSql += " ,0" 'PURITY
            strSql += " ,'" & wtxtNarration.Text & "'" 'NARRATION
            strSql += " ,'" & wtxtNameOfProduct_OWN.Text.Replace("'", "''") & "'" 'DESCRIP

            strSql += " ,'" & strSubitemids & "'" 'REASON

            strSql += " ,'M'"


            strSql += " ,'N'"

            strSql += " ,NULL" 'ISSDATE
            strSql += " ,0" 'ISSREFNO
            strSql += " ,0" 'ISSPCS
            strSql += " ,0" 'ISSWT
            strSql += " ,''" 'FROMFLAG
            strSql += " ,''" 'TOFLAG
            strSql += " ,''" 'APPROVAL
            strSql += " ,'" & calType & "'" 'SALEMODE
            strSql += " ,''" 'BATCHNO
            strSql += " ,0" 'MARK

            strSql += " ,'',''" ' pctfile

            'strSql += " ,'" & IIf(picPath <> Nothing, "L" + txtLotNo_Num_Man.Text + "I" + itemId.ToString + "T" + TagNo.Replace(":", "") + "." + picExtension.ToString, picPath) & "'" 'PCTFILE
            strSql += " ,''" 'OLDTAGNO
            strSql += " ," & Val(itemTypeId) & "" 'ITEMTYPEID
            strSql += " ,'" & GetEntryDate(wdtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'ACTUALRECDATE
            strSql += " ,''" 'WEIGHTUNIT
            strSql += " ,0" 'TRANSFERWT
            strSql += " ,NULL" 'CHKDATE
            strSql += " ,''" 'CHKTRAY
            strSql += " ,''" 'CARRYFLAG
            strSql += " ,''" 'BRANDID
            strSql += " ,''" 'PRNFLAG
            strSql += " ,0" 'MCDISCPER
            strSql += " ,0" 'WASTDISCPER
            strSql += " ,NULL" 'RESDATE
            strSql += " ,'" & tranInvNo & "'" 'TRANINVNO
            strSql += " ,'" & supBillno & "'" 'SUPBILLNO
            strSql += " ,''" 'WORKDAYS
            strSql += " ," & userId & "" 'USERID
            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
            strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
            strSql += " ,'" & systemId & "'" 'SYSTEMID
            strSql += " ,''" 'STYLENO
            strSql += " ,'" & VERSION & "'" 'APPVER
            strSql += " ,'" & GetEntryDate(wdtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'TRANSFERDATE
            strSql += " ," & Val(wtxtMetalRate_Amt.Text) & ""
            strSql += " ,''"
            strSql += " ,0"
            strSql += " ,''" 'HM_BILLNO
            strSql += " ,''" 'HM_CENTER
            strSql += " ,0" 'ADD_VA_PER
            strSql += " ,0" 'REFVALUE
            strSql += " ,'" & mlwmctype & "'"
            strSql += " ,'" & COSTID & "'" 'TCOSTID
            strSql += " ,'" & Val(ObjExtraWt.txtExtraWt_WET.Text) & "'" 'EXTRAWT
            strSql += " ," & IIf(NEEDUS = True And Studded_Loose = "L", Val(ObjRsUs.txtUSDollar_Amt.Text), 0) & ""
            strSql += " ," & IIf(NEEDUS = True And Studded_Loose = "L", Val(ObjRsUs.txtIndRs_Amt.Text), 0) & ""
            strSql += " ,'" & Recsno & "'" 'EXTRAWT
            strSql += " ," & mfromItemid & "" 'EXTRAWT
            If _FourCMaintain Then
                strSql += " ," & TagCutId & "" 'CUTID
                strSql += " ," & TagColorId & "" 'COLORID
                strSql += " ," & TagClarityId & "" 'CLARITYID
                strSql += " ," & TagShapeId & "" 'SHAPEID
                strSql += " ," & TagSetTypeId & "" 'SETTYPEID
            End If
            strSql += " ," & Val(wtxtHght_Wet.Text) & "," & Val(wtxtWidth_Wet.Text)
            strSql += " )"
            'If File.Exists(picPath) = True And SubItemPic = False Then
            '    Dim serverPath As String = Nothing
            '    Dim fileDestPath As String = (defalutDestination + "L" + txtLotNo_Num_Man.Text + "I" + itemId.ToString + "T" + TagNo.Replace(":", "") + IIf(picExtension.ToString.StartsWith("."), picExtension.ToString, "." + picExtension.ToString))

            '    Dim Finfo As FileInfo
            '    Finfo = New FileInfo(fileDestPath)
            '    If IO.Directory.Exists(Finfo.Directory.FullName) = False Then
            '        MsgBox(Finfo.Directory.FullName & " Does not exist. Please make a corresponding path", MsgBoxStyle.Information)
            '        tran.Rollback()
            '        Exit Function
            '    End If
            '    If UCase(picPath) <> fileDestPath.ToUpper Then
            '        Dim cFile As New FileInfo(picPath)
            '        cFile.CopyTo(fileDestPath, True)
            '    End If
            'End If

            'If XToCostid <> "" And XBranchtag Then
            '    ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid, , IIf(picPath <> Nothing, (defalutDestination + "L" + txtLotNo_Num_Man.Text + "I" + itemId.ToString + "T" + WtxtTagNo__Man.Text.Replace(":", "") + "." + picExtension), Nothing), "TITEMTAG", , True)
            'Else
            cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
            'End If
            TagCutId = 0 : TagColorId = 0 : TagClarityId = 0 : TagShapeId = 0 : TagSetTypeId = 0 : TagHeight = 0 : TagWidth = 0
            'ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID, , IIf(picPath <> Nothing, (defalutDestination + "L" + txtLotNo_Num_Man.Text + "I" + itemId.ToString + "T" + WtxtTagNo__Man.Text.Replace(":", "") + "." + picExtension), Nothing), "TITEMTAG", , False)


            ''Inserting StoneDetail
            For cnt As Integer = 0 To dtWStoneDetails.Rows.Count - 1
                With dtWStoneDetails.Rows(cnt)
                    Dim CutId As Integer = 0
                    Dim ColorId As Integer = 0
                    Dim ClarityId As Integer = 0
                    Dim ShapeId As Integer = 0
                    Dim SetTypeId As Integer = 0
                    Dim stnItemId As Integer = 0
                    Dim stnSubItemId As Integer = 0
                    Dim stnSno As String = GetNewSno(TranSnoType.WITEMTAGSTONECODE, tran, "GET_ADMINSNO_TRAN")
                    'Dim caType As String = Nothing
                    strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Item("ITEM").ToString & "'"
                    stnItemId = Val(objGPack.GetSqlValue(strSql, "ITEMID", , tran))
                    strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & .Item("SUBITEM").ToString & "' AND ITEMID = " & stnItemId & ""
                    stnSubItemId = Val(objGPack.GetSqlValue(strSql, , , tran))
                    .Item("STNSNO") = stnSno
                    ''Inserting itemTagStone
                    strSql = " INSERT INTO " & cnAdminDb & "..WITEMTAGSTONE("
                    strSql += " SNO,TAGSNO,ITEMID,COMPANYID,STNITEMID,"
                    strSql += " STNSUBITEMID,TAGNO,STNPCS,STNWT,"
                    strSql += " STNRATE,STNAMT,DESCRIP,"
                    strSql += " RECDATE,CALCMODE,"
                    strSql += " MINRATE,SIZECODE,STONEUNIT,ISSDATE,"
                    strSql += " OLDTAGNO,CARRYFLAG,COSTID,SYSTEMID,APPVER,"
                    strSql += " USRATE,INDRS,PACKETNO"
                    If _FourCMaintain Then
                        strSql += " ,CUTID,COLORID,CLARITYID,SHAPEID,SETTYPEID,HEIGHT,WIDTH"
                        CutId = Val(objGPack.GetSqlValue(" SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & .Item("CUT").ToString & "'", "CUTID", , tran))
                        ColorId = Val(objGPack.GetSqlValue(" SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME = '" & .Item("COLOR").ToString & "'", "COLORID", , tran))
                        ClarityId = Val(objGPack.GetSqlValue(" SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & .Item("CLARITY").ToString & "'", "CLARITYID", , tran))
                        ShapeId = Val(objGPack.GetSqlValue(" SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & .Item("SHAPE").ToString & "'", "SHAPEID", , tran))
                        SetTypeId = Val(objGPack.GetSqlValue(" SELECT SETTYPEID FROM " & cnAdminDb & "..STNSETTYPE WHERE SETTYPENAME = '" & .Item("SETTYPE").ToString & "'", "SETTYPEID", , tran))
                    End If
                    strSql += " )VALUES("
                    strSql += " '" & stnSno & "'" ''SNO
                    strSql += " ,'" & TagSno & "'" 'TAGSNO
                    strSql += " ,'" & itemId & "'" 'ITEMID
                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    strSql += " ," & stnItemId & "" 'STNITEMID
                    strSql += " ," & stnSubItemId & "" 'STNSUBITEMID
                    strSql += " ,'" & TagNo & "'" 'TAGNO
                    strSql += " ," & Val(.Item("PCS").ToString) & "" 'STNPCS
                    strSql += " ," & Val(.Item("WEIGHT").ToString) & "" 'STNWT
                    strSql += " ," & Val(.Item("RATE").ToString) & "" 'STNRATE
                    strSql += " ," & Val(.Item("AMOUNT").ToString) & "" 'STNAMT
                    If stnSubItemId <> 0 Then 'DESCRIP
                        strSql += " ,'" & .Item("SUBITEM").ToString & "'"
                    Else
                        strSql += " ,'" & .Item("ITEM").ToString & "'"
                    End If
                    strSql += " ,'" & GetEntryDate(wdtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                    strSql += " ,'" & .Item("CALC").ToString & "'" 'CALCMODE
                    strSql += " ,0" 'MINRATE
                    strSql += " ,0" 'SIZECODE
                    strSql += " ,'" & .Item("UNIT").ToString & "'" 'STONEUNIT
                    strSql += " ,NULL" 'ISSDATE
                    strSql += " ,''" 'OLDTAGNO
                    strSql += " ,''" 'CARRYFLAG
                    strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " ," & Val(.Item("USRATE").ToString) & "" 'USRATE
                    strSql += " ," & Val(.Item("INDRS").ToString) & "" 'INDRS
                    strSql += " ,'" & .Item("PACKETNO").ToString & "'" 'PACKETNO
                    If _FourCMaintain Then
                        strSql += " ," & CutId & "" 'CUTID
                        strSql += " ," & ColorId & "" 'COLORID
                        strSql += " ,'" & ClarityId & "'" 'CLARITYID
                        strSql += " ,'" & ShapeId & "'" 'SHAPEID
                        strSql += " ,'" & SetTypeId & "'" 'SETTYPEID
                        strSql += " ,'" & Val(.Item("HEIGHT").ToString) & "'" 'HEIGHT
                        strSql += " ,'" & Val(.Item("WIDTH").ToString) & "'" 'WIDTH
                    End If
                    strSql += " )"
                    If XToCostid <> "" And XBranchtag Then
                        ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid, , , "TITEMTAGSTONE", , True)
                    Else
                        cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                    End If
                    'ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID, , , "TITEMTAGSTONE", , False)
                End With
            Next

            ''INSERTING ADDITIONAL STOCK ENTRY

            If wChkcmbAddM1.Enabled And wChkcmbAddM1.Text <> "" Then
                Dim OthIds() As String = wChkcmbAddM1.Text.ToString.Split(",")
                For i As Integer = 0 To OthIds.Length - 1
                    Dim Othid As String = Val(objGPack.GetSqlValue("SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE NAME='" & OthIds(i).Trim & "' AND MISCID=1", , , tran))
                    strSql = " INSERT INTO " & cnAdminDb & "..WADDINFOITEMTAG("
                    strSql += " SNO,TAGSNO,OTHID,ITEMID,TAGNO,RECDATE,COMPANYID"
                    strSql += " ,COSTID,SYSTEMID,APPVER"
                    strSql += " )VALUES("
                    strSql += " '" & GetNewSno(TranSnoType.ADDINFOITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO  
                    strSql += " ,'" & TagSno & "'" 'TAGSNO
                    strSql += " ,'" & Othid & "'" 'OTHID
                    strSql += " ,'" & itemId & "'" 'ITEMID
                    strSql += " ,'" & TagNo & "'" 'TAGNO
                    strSql += " ,'" & GetEntryDate(wdtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " )"
                    If XToCostid <> "" And XBranchtag Then
                        ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid)
                    Else
                        cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                    End If
                Next
            End If
            If wChkcmbAddM2.Enabled And wChkcmbAddM2.Text <> "" Then
                Dim OthIds() As String = wChkcmbAddM2.Text.ToString.Split(",")
                For i As Integer = 0 To OthIds.Length - 1
                    Dim Othid As String = Val(objGPack.GetSqlValue("SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE NAME='" & OthIds(i).Trim & "' AND MISCID=2", , , tran))
                    strSql = " INSERT INTO " & cnAdminDb & "..WADDINFOITEMTAG("
                    strSql += " SNO,TAGSNO,OTHID,ITEMID,TAGNO,RECDATE,COMPANYID"
                    strSql += " ,COSTID,SYSTEMID,APPVER"
                    strSql += " )VALUES("
                    strSql += " '" & GetNewSno(TranSnoType.ADDINFOITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO  
                    strSql += " ,'" & TagSno & "'" 'TAGSNO
                    strSql += " ,'" & Othid & "'" 'OTHID
                    strSql += " ,'" & itemId & "'" 'ITEMID
                    strSql += " ,'" & TagNo & "'" 'TAGNO
                    strSql += " ,'" & GetEntryDate(wdtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " )"
                    If XToCostid <> "" And XBranchtag Then
                        ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid)
                    Else
                        cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                    End If
                Next
            End If
            If wChkcmbAddM3.Enabled And wChkcmbAddM3.Text <> "" Then
                Dim OthIds() As String = wChkcmbAddM3.Text.ToString.Split(",")
                For i As Integer = 0 To OthIds.Length - 1
                    Dim Othid As String = Val(objGPack.GetSqlValue("SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE NAME='" & OthIds(i).Trim & "' AND MISCID=3", , , tran))
                    strSql = " INSERT INTO " & cnAdminDb & "..WADDINFOITEMTAG("
                    strSql += " SNO,TAGSNO,OTHID,ITEMID,TAGNO,RECDATE,COMPANYID"
                    strSql += " ,COSTID,SYSTEMID,APPVER"
                    strSql += " )VALUES("
                    strSql += " '" & GetNewSno(TranSnoType.ADDINFOITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO  
                    strSql += " ,'" & TagSno & "'" 'TAGSNO
                    strSql += " ,'" & Othid & "'" 'OTHID
                    strSql += " ,'" & itemId & "'" 'ITEMID
                    strSql += " ,'" & TagNo & "'" 'TAGNO
                    strSql += " ,'" & GetEntryDate(wdtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " )"
                    If XToCostid <> "" And XBranchtag Then
                        ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid)
                    Else
                        cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                    End If
                Next
            End If
            If wChkcmbAddM4.Enabled And wChkcmbAddM4.Text <> "" Then
                Dim OthIds() As String = wChkcmbAddM4.Text.ToString.Split(",")
                For i As Integer = 0 To OthIds.Length - 1
                    Dim Othid As String = Val(objGPack.GetSqlValue("SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE NAME='" & OthIds(i).Trim & "' AND MISCID=4", , , tran))
                    strSql = " INSERT INTO " & cnAdminDb & "..WADDINFOITEMTAG("
                    strSql += " SNO,TAGSNO,OTHID,ITEMID,TAGNO,RECDATE,COMPANYID"
                    strSql += " ,COSTID,SYSTEMID,APPVER"
                    strSql += " )VALUES("
                    strSql += " '" & GetNewSno(TranSnoType.ADDINFOITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO  
                    strSql += " ,'" & TagSno & "'" 'TAGSNO
                    strSql += " ,'" & Othid & "'" 'OTHID
                    strSql += " ,'" & itemId & "'" 'ITEMID
                    strSql += " ,'" & TagNo & "'" 'TAGNO
                    strSql += " ,'" & GetEntryDate(wdtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " )"
                    If XToCostid <> "" And XBranchtag Then
                        ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid)
                    Else
                        cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                    End If
                Next
            End If
            If wChkcmbAddM5.Enabled And wChkcmbAddM5.Text <> "" Then
                Dim OthIds() As String = wChkcmbAddM5.Text.ToString.Split(",")
                For i As Integer = 0 To OthIds.Length - 1
                    Dim Othid As String = Val(objGPack.GetSqlValue("SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE NAME='" & OthIds(i).Trim & "' AND MISCID=5", , , tran))
                    strSql = " INSERT INTO " & cnAdminDb & "..WADDINFOITEMTAG("
                    strSql += " SNO,TAGSNO,OTHID,ITEMID,TAGNO,RECDATE,COMPANYID"
                    strSql += " ,COSTID,SYSTEMID,APPVER"
                    strSql += " )VALUES("
                    strSql += " '" & GetNewSno(TranSnoType.ADDINFOITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO  
                    strSql += " ,'" & TagSno & "'" 'TAGSNO
                    strSql += " ,'" & Othid & "'" 'OTHID
                    strSql += " ,'" & itemId & "'" 'ITEMID
                    strSql += " ,'" & TagNo & "'" 'TAGNO
                    strSql += " ,'" & GetEntryDate(wdtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " )"
                    If XToCostid <> "" And XBranchtag Then
                        ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid)
                    Else
                        cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                    End If
                Next
            End If
            If wChkcmbAddM6.Enabled And wChkcmbAddM6.Text <> "" Then
                Dim OthIds() As String = wChkcmbAddM6.Text.ToString.Split(",")
                For i As Integer = 0 To OthIds.Length - 1
                    Dim Othid As String = Val(objGPack.GetSqlValue("SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE NAME='" & OthIds(i).Trim & "' AND MISCID=6", , , tran))
                    strSql = " INSERT INTO " & cnAdminDb & "..WADDINFOITEMTAG("
                    strSql += " SNO,TAGSNO,OTHID,ITEMID,TAGNO,RECDATE,COMPANYID"
                    strSql += " ,COSTID,SYSTEMID,APPVER"
                    strSql += " )VALUES("
                    strSql += " '" & GetNewSno(TranSnoType.ADDINFOITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO  
                    strSql += " ,'" & TagSno & "'" 'TAGSNO
                    strSql += " ,'" & Othid & "'" 'OTHID
                    strSql += " ,'" & itemId & "'" 'ITEMID
                    strSql += " ,'" & TagNo & "'" 'TAGNO
                    strSql += " ,'" & GetEntryDate(wdtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " )"
                    If XToCostid <> "" And XBranchtag Then
                        ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid)
                    Else
                        cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                    End If
                Next
            End If

            If wChkcmbAddM7.Enabled And wChkcmbAddM7.Text <> "" Then
                Dim OthIds() As String = wChkcmbAddM7.Text.ToString.Split(",")
                For i As Integer = 0 To OthIds.Length - 1
                    Dim Othid As String = Val(objGPack.GetSqlValue("SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE NAME='" & OthIds(i).Trim & "' AND MISCID=7", , , tran))
                    strSql = " INSERT INTO " & cnAdminDb & "..WADDINFOITEMTAG("
                    strSql += " SNO,TAGSNO,OTHID,ITEMID,TAGNO,RECDATE,COMPANYID"
                    strSql += " ,COSTID,SYSTEMID,APPVER"
                    strSql += " )VALUES("
                    strSql += " '" & GetNewSno(TranSnoType.ADDINFOITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO  
                    strSql += " ,'" & TagSno & "'" 'TAGSNO
                    strSql += " ,'" & Othid & "'" 'OTHID
                    strSql += " ,'" & itemId & "'" 'ITEMID
                    strSql += " ,'" & TagNo & "'" 'TAGNO
                    strSql += " ,'" & GetEntryDate(wdtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " )"
                    If XToCostid <> "" And XBranchtag Then
                        ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid)
                    Else
                        cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                    End If
                Next
            End If

            If wChkcmbAddM8.Enabled And wChkcmbAddM8.Text <> "" Then
                Dim OthIds() As String = wChkcmbAddM8.Text.ToString.Split(",")
                For i As Integer = 0 To OthIds.Length - 1
                    Dim Othid As String = Val(objGPack.GetSqlValue("SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE NAME='" & OthIds(i).Trim & "' AND MISCID=8", , , tran))
                    strSql = " INSERT INTO " & cnAdminDb & "..WADDINFOITEMTAG("
                    strSql += " SNO,TAGSNO,OTHID,ITEMID,TAGNO,RECDATE,COMPANYID"
                    strSql += " ,COSTID,SYSTEMID,APPVER"
                    strSql += " )VALUES("
                    strSql += " '" & GetNewSno(TranSnoType.ADDINFOITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO  
                    strSql += " ,'" & TagSno & "'" 'TAGSNO
                    strSql += " ,'" & Othid & "'" 'OTHID
                    strSql += " ,'" & itemId & "'" 'ITEMID
                    strSql += " ,'" & TagNo & "'" 'TAGNO
                    strSql += " ,'" & GetEntryDate(wdtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " )"
                    If XToCostid <> "" And XBranchtag Then
                        ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid)
                    Else
                        cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                    End If
                Next
            End If

            If wChkcmbAddM9.Enabled And wChkcmbAddM9.Text <> "" Then
                Dim OthIds() As String = wChkcmbAddM9.Text.ToString.Split(",")
                For i As Integer = 0 To OthIds.Length - 1
                    Dim Othid As String = Val(objGPack.GetSqlValue("SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE NAME='" & OthIds(i).Trim & "' AND MISCID=9", , , tran))
                    strSql = " INSERT INTO " & cnAdminDb & "..WADDINFOITEMTAG("
                    strSql += " SNO,TAGSNO,OTHID,ITEMID,TAGNO,RECDATE,COMPANYID"
                    strSql += " ,COSTID,SYSTEMID,APPVER"
                    strSql += " )VALUES("
                    strSql += " '" & GetNewSno(TranSnoType.ADDINFOITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO  
                    strSql += " ,'" & TagSno & "'" 'TAGSNO
                    strSql += " ,'" & Othid & "'" 'OTHID
                    strSql += " ,'" & itemId & "'" 'ITEMID
                    strSql += " ,'" & TagNo & "'" 'TAGNO
                    strSql += " ,'" & GetEntryDate(wdtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " )"
                    If XToCostid <> "" And XBranchtag Then
                        ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid)
                    Else
                        cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                    End If
                Next
            End If

            If wChkcmbAddM10.Enabled And wChkcmbAddM10.Text <> "" Then
                Dim OthIds() As String = wChkcmbAddM10.Text.ToString.Split(",")
                For i As Integer = 0 To OthIds.Length - 1
                    Dim Othid As String = Val(objGPack.GetSqlValue("SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE NAME='" & OthIds(i).Trim & "' AND MISCID=10", , , tran))
                    strSql = " INSERT INTO " & cnAdminDb & "..WADDINFOITEMTAG("
                    strSql += " SNO,TAGSNO,OTHID,ITEMID,TAGNO,RECDATE,COMPANYID"
                    strSql += " ,COSTID,SYSTEMID,APPVER"
                    strSql += " )VALUES("
                    strSql += " '" & GetNewSno(TranSnoType.ADDINFOITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO  
                    strSql += " ,'" & TagSno & "'" 'TAGSNO
                    strSql += " ,'" & Othid & "'" 'OTHID
                    strSql += " ,'" & itemId & "'" 'ITEMID
                    strSql += " ,'" & TagNo & "'" 'TAGNO
                    strSql += " ,'" & GetEntryDate(wdtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " )"
                    If XToCostid <> "" And XBranchtag Then
                        ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid)
                    Else
                        cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                    End If
                Next
            End If

            If wChkcmbAddM11.Enabled And wChkcmbAddM11.Text <> "" Then
                Dim OthIds() As String = wChkcmbAddM11.Text.ToString.Split(",")
                For i As Integer = 0 To OthIds.Length - 1
                    Dim Othid As String = Val(objGPack.GetSqlValue("SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE NAME='" & OthIds(i).Trim & "' AND MISCID=11", , , tran))
                    strSql = " INSERT INTO " & cnAdminDb & "..WADDINFOITEMTAG("
                    strSql += " SNO,TAGSNO,OTHID,ITEMID,TAGNO,RECDATE,COMPANYID"
                    strSql += " ,COSTID,SYSTEMID,APPVER"
                    strSql += " )VALUES("
                    strSql += " '" & GetNewSno(TranSnoType.ADDINFOITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO  
                    strSql += " ,'" & TagSno & "'" 'TAGSNO
                    strSql += " ,'" & Othid & "'" 'OTHID
                    strSql += " ,'" & itemId & "'" 'ITEMID
                    strSql += " ,'" & TagNo & "'" 'TAGNO
                    strSql += " ,'" & GetEntryDate(wdtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " )"
                    If XToCostid <> "" And XBranchtag Then
                        ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid)
                    Else
                        cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                    End If
                Next
            End If

            If wChkcmbAddM12.Enabled And wChkcmbAddM12.Text <> "" Then
                Dim OthIds() As String = wChkcmbAddM12.Text.ToString.Split(",")
                For i As Integer = 0 To OthIds.Length - 1
                    Dim Othid As String = Val(objGPack.GetSqlValue("SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE NAME='" & OthIds(i).Trim & "' AND MISCID=12", , , tran))
                    strSql = " INSERT INTO " & cnAdminDb & "..WADDINFOITEMTAG("
                    strSql += " SNO,TAGSNO,OTHID,ITEMID,TAGNO,RECDATE,COMPANYID"
                    strSql += " ,COSTID,SYSTEMID,APPVER"
                    strSql += " )VALUES("
                    strSql += " '" & GetNewSno(TranSnoType.ADDINFOITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO  
                    strSql += " ,'" & TagSno & "'" 'TAGSNO
                    strSql += " ,'" & Othid & "'" 'OTHID
                    strSql += " ,'" & itemId & "'" 'ITEMID
                    strSql += " ,'" & TagNo & "'" 'TAGNO
                    strSql += " ,'" & GetEntryDate(wdtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " )"
                    If XToCostid <> "" And XBranchtag Then
                        ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid)
                    Else
                        cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                    End If
                Next
            End If

            ''ORDER IRDETAIL
            If Not OrderRow Is Nothing Then
                ''BATCHNO
                'strSql = " SELECT MAX(CTLTEXT)+1 BATCHNO FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'BATCHNO'"
                'Dim batchNo As String = objGPack.GetSqlValue(strSql, , , tran)
                'batchNo = COSTID + Mid(Today.Year, 3, 2).ToString + batchNo
                Dim ordCompanyId As String = objGPack.GetSqlValue("SELECT COMPANYID FROM " & cnAdminDb & "..ORMAST WHERE SNO = '" & OrderRow.Item("SNO").ToString & "'", , , tran)
                Dim batchno As String = GetNewBatchno(cnCostId, GetEntryDate(wdtpRecieptDate.Value, tran).ToString("yyyy-MM-dd"), tran)
                Dim Ord_TranNo As String

                strSql = " SELECT 1 CNT FROM " & cnAdminDb & "..ORIRDETAIL WHERE ISNULL(ORNO,'') = '" & OrderRow.Item("ORNO").ToString & "' AND ISNULL(ORSNO,'') = '" & OrderRow.Item("SNO").ToString & "' AND ISNULL(ORSTATUS,'') = 'I' AND ISNULL(CANCEL,'') = ''"
                Dim dtChk As New DataTable
                cmd = New OleDbCommand(strSql, cn, tran)
                da = New OleDbDataAdapter(cmd)
                da.Fill(dtChk)
                If dtChk.Rows.Count = 0 Then
                    Ord_TranNo = objGPack.GetSqlValue(" SELECT ISNULL(MAX(CTLTEXT),0)+1 AS TRANNO FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID  = 'ORDIRBILLNO' AND COMPANYID = '" & strCompanyId & "'", , , tran)
                    strSql = " INSERT INTO " & cnAdminDb & "..ORIRDETAIL"
                    strSql += " ("
                    strSql += " SNO,ORSNO,TRANNO,TRANDATE,DESIGNERID,PCS,GRSWT,NETWT,TAGNO,ORSTATUS,CANCEL,COSTID,DESCRIPT,ORNO"
                    strSql += " ,BATCHNO,USERID,UPDATED,UPTIME,APPVER,COMPANYID,ENTRYORDER,ORDSTATE_ID,CATCODE"
                    strSql += " )"
                    strSql += " VALUES"
                    strSql += " ("
                    strSql += " '" & GetNewSno(TranSnoType.ORIRDETAILCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO
                    strSql += " ,'" & OrderRow.Item("SNO").ToString & "'" 'ORSNO
                    'strSql += " ," & Val(objGPack.GetSqlValue(" SELECT ISNULL(MAX(CTLTEXT),0)+1 AS TRANNO FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID  = 'ORDIRBILLNO' AND COMPANYID = '" & strCompanyId & "'", , , tran)) & "" 'TRANNO
                    strSql += " ," & Val(Ord_TranNo) & "" 'TRANNO
                    strSql += " ,'" & GetEntryDate(wdtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'TRANDATE
                    strSql += " ," & designerId & "" 'DESIGNERID
                    strSql += " ," & Val(wtxtPieces_Num_Man.Text) & "" 'PCS
                    strSql += " ," & Val(WtxtGrossWt_Wet.Text) & "" 'GRSWT
                    strSql += " ," & Val(WtxtNetWt_Wet.Text) & "" 'NETWT
                    strSql += " ,'" & wtxtTagNo__Man.Text & "'" 'TAGNO
                    strSql += " ,'I'" 'ORSTATUS
                    strSql += " ,''" 'CANCEL
                    strSql += " ,'" & COSTID & "'" 'COSTID
                    strSql += " ,'" & wtxtNarration.Text & "'" 'DESCRIPT
                    strSql += " ,'" & OrderRow.Item("ORNO").ToString & "'" 'ORNO
                    strSql += " ,'" & batchno & "'" 'BATCHNO
                    strSql += " ," & userId & "" 'USERID
                    strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                    strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " ,'" & ordCompanyId & "'" 'COMPANYID
                    strSql += " ," & objGPack.GetSqlValue("SELECT ISNULL(MAX(ENTRYORDER),0)+1 FROM " & cnAdminDb & "..ORIRDETAIL", , , tran) & "" 'ENTRYORDER
                    strSql += " ,2" 'ORDSTATE_ID
                    strSql += " ,'" & objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(itemId) & "", , , tran) & "'" 'CATCODE
                    strSql += " )"
                    If XToCostid <> "" And XBranchtag Then
                        ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid, , , , , True)
                    Else
                        cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                    End If

                    strSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & Ord_TranNo & "' WHERE CTLID = 'ORDIRBILLNO' AND COMPANYID = '" & strCompanyId & "'"
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()
                End If


                Ord_TranNo = objGPack.GetSqlValue(" SELECT ISNULL(MAX(CTLTEXT),0)+1 AS TRANNO FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID  = 'ORDIRBILLNO' AND COMPANYID = '" & strCompanyId & "'", , , tran)

                strSql = " INSERT INTO " & cnAdminDb & "..ORIRDETAIL"
                strSql += " ("
                strSql += " SNO,ORSNO,TRANNO,TRANDATE,DESIGNERID,PCS,GRSWT,NETWT,TAGNO,ORSTATUS,CANCEL,COSTID,DESCRIPT,ORNO"
                strSql += " ,BATCHNO,USERID,UPDATED,UPTIME,APPVER,COMPANYID,ENTRYORDER,ORDSTATE_ID,CATCODE"
                strSql += " )"
                strSql += " VALUES"
                strSql += " ("
                strSql += " '" & GetNewSno(TranSnoType.ORIRDETAILCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO
                strSql += " ,'" & OrderRow.Item("SNO").ToString & "'" 'ORSNO
                'strSql += " ," & Val(objGPack.GetSqlValue(" SELECT ISNULL(MAX(CTLTEXT),0)+1 AS TRANNO FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID  = 'ORDIRBILLNO' AND COMPANYID = '" & strCompanyId & "'", , , tran)) & "" 'TRANNO
                strSql += " ," & Val(Ord_TranNo) & "" 'TRANNO
                strSql += " ,'" & GetEntryDate(wdtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'TRANDATE
                strSql += " ," & designerId & "" 'DESIGNERID
                strSql += " ," & Val(wtxtPieces_Num_Man.Text) & "" 'PCS
                strSql += " ," & Val(WtxtGrossWt_Wet.Text) & "" 'GRSWT
                strSql += " ," & Val(WtxtNetWt_Wet.Text) & "" 'NETWT
                strSql += " ,'" & wtxtTagNo__Man.Text & "'" 'TAGNO
                strSql += " ,'R'" 'ORSTATUS
                strSql += " ,''" 'CANCEL
                strSql += " ,'" & COSTID & "'" 'COSTID
                strSql += " ,'" & wtxtNarration.Text & "'" 'DESCRIPT
                strSql += " ,'" & OrderRow.Item("ORNO").ToString & "'" 'ORNO
                strSql += " ,'" & batchno & "'" 'BATCHNO
                strSql += " ," & userId & "" 'USERID
                strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                strSql += " ,'" & VERSION & "'" 'APPVER
                strSql += " ,'" & ordCompanyId & "'" 'COMPANYID
                strSql += " ," & objGPack.GetSqlValue("SELECT ISNULL(MAX(ENTRYORDER),0)+1 FROM " & cnAdminDb & "..ORIRDETAIL", , , tran) & "" 'ENTRYORDER
                strSql += " ,4" 'ORDSTATE_ID
                strSql += " ,'" & objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(itemId) & "", , , tran) & "'" 'CATCODE
                strSql += " )"
                If XToCostid <> "" And XBranchtag Then
                    ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid, , , , , True)
                Else
                    cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                End If
                'ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID, , , , , False)
                strSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & Ord_TranNo & "' WHERE CTLID = 'ORDIRBILLNO' AND COMPANYID = '" & strCompanyId & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
            End If



            ''INSERT INTO GRIDVIEW
            Dim ro As DataRow = Nothing
            ro = dtWGridView.NewRow
            ro("ITEM") = wcmbItem_MAN.Text
            ro("TAGNO") = TagNo
            ro("PCS") = IIf(Val(wtxtPieces_Num_Man.Text) <> 0, Val(wtxtPieces_Num_Man.Text), DBNull.Value)
            ro("GRSWT") = IIf(Val(WtxtGrossWt_Wet.Text) <> 0, Val(WtxtGrossWt_Wet.Text), DBNull.Value)
            ro("LESSWT") = IIf(Val(WtxtLessWt_Wet.Text) <> 0, Val(WtxtLessWt_Wet.Text), DBNull.Value)
            ro("NETWT") = IIf(Val(WtxtNetWt_Wet.Text) <> 0, Val(WtxtNetWt_Wet.Text), DBNull.Value)
            ro("RATE") = Val(wtxtMMRate.Text)
            ro("SALEVALUE") = wtxtSalValue_Amt_Man.Text
            ro("SIZE") = DBNull.Value
            dtWGridView.Rows.Add(ro)
            dtWGridView.AcceptChanges()
            wgridView.CurrentCell = wgridView.Rows(wgridView.RowCount - 1).Cells("ITEM")

            If Tagsave = True Then funcAdd(TagSno)

            tran.Commit()
            tran = Nothing
            MsgBox(TagNo + E0012, MsgBoxStyle.Exclamation)
            If Tagsave = True Then funcNew()
            WfuncNew()
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            If Not tran Is Nothing Then tran.Dispose()
        End Try
    End Function

    Private Sub WStyleGridMultiMetal()

        With wgridMultimetal
            .Columns("CATEGORY").Width = wtxtMMCategory.Width + 1
            .Columns("RATE").Visible = False
            With .Columns("WEIGHT")
                .HeaderText = "WEIGHT"
                .Width = wtxtMMWeight_Wet.Width + 1
                .DefaultCellStyle.Format = "0.000"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            .Columns("PURWASTAGEPER").Visible = False
            .Columns("PURWASTAGE").Visible = False
            .Columns("PURMCPERGRM").Visible = False
            .Columns("PURMC").Visible = False
            With .Columns("WASTAGEPER")
                .HeaderText = "WAST%"
                .Width = wtxtMMWastagePer_PER.Width + 1
                .DefaultCellStyle.Format = "0.000"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("WASTAGE")
                .HeaderText = "WAST"
                .Width = wtxtMMWastage_WET.Width + 1
                .DefaultCellStyle.Format = "0.000"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("MCPERGRM")
                .HeaderText = "MC/GRM"
                .Width = wtxtMMMcPerGRm_AMT.Width + 1
                .DefaultCellStyle.Format = "0.000"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("MC")
                .HeaderText = "MC"
                .Width = wtxtMMMc_AMT.Width + 1
                .DefaultCellStyle.Format = "0.000"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("AMOUNT")
                .HeaderText = "AMOUNT"
                .Width = wtxtMMAmount_AMT.Width + 1
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "0.00"
            End With
            With .Columns("PURRATE")
                .Width = 80
                .HeaderText = "PURRATE"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "0.00"
                .Visible = False
            End With
            With .Columns("PURAMOUNT")
                .Width = 99
                .HeaderText = "PURAMOUNT"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "0.00"
                .Visible = False
            End With
            .Columns("KEYNO").Visible = False

            .Columns("STNSNO").Visible = False
        End With
    End Sub

    Private Sub WStyleGridStone()
        With wgridStone
            .Columns("PACKETNO").Width = 85
            If PacketNoEnable = "N" Then
                .Columns("PACKETNO").Visible = False
                wtxtstPackettno.Visible = False
                wlblstPacketno.Visible = False
                wpnlStDet.AutoSize = True
                wpnlStDet.Location = New Point(12, 11)
                wpnlStoneGrid.Size = New Size(773, 155)
            Else
                .Columns("PACKETNO").Visible = True
                wtxtstPackettno.Visible = True
                wlblstPacketno.Visible = True
            End If
            .Columns("ITEM").Width = 211
            .Columns("SUBITEM").Width = 186
            .Columns("UNIT").Width = 40
            .Columns("CALC").Width = 40
            .Columns("PCS").Width = 39
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("WEIGHT").Width = 74
            .Columns("WEIGHT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RATE").Width = 80
            .Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").Width = 99
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("METALID").Visible = False
            .Columns("KEYNO").Visible = False
            With .Columns("PURRATE")
                .Width = 80
                .Visible = False
                .DefaultCellStyle.Format = "0.00"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("PURVALUE")
                .Width = 99
                .Visible = False
                .DefaultCellStyle.Format = "0.00"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            .Columns("STNSNO").Visible = False
        End With
    End Sub
    Private Sub WStyleGridMisc()
        With wgridMisc
            With .Columns("MISC")
                .HeaderText = "MISCELLANEOUS"
                .Width = 298
            End With
            With .Columns("AMOUNT")
                .HeaderText = "AMOUNT"
                .Width = 99
                .DefaultCellStyle.Format = "0.00"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("PURAMOUNT")
                .Visible = False
                .Width = 99
                .DefaultCellStyle.Format = "0.00"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            .Columns("KEYNO").Visible = False
        End With
    End Sub

    Private Function WGetParity(ByVal ParityName As String) As System.IO.Ports.Parity

        Select Case ParityName
            Case "N"
                Return Ports.Parity.None
            Case "E"
                Return Ports.Parity.Even
            Case "M"
                Return Ports.Parity.Mark
            Case "O"
                Return Ports.Parity.Odd
            Case "S"
                Return Ports.Parity.Space
            Case Else
                Return Ports.Parity.None
        End Select
    End Function

    Function wfuncAssignTabControls() As Integer
        WTabControl1.TabPages.Remove(WtabMultiMetal)
        WTabControl1.TabPages.Remove(WtabStone)
        WTabControl1.TabPages.Remove(WtabOtherCharges)

        Dim ds As New DataSet
        ds.Clear()
        'If multiMetal = "Y" Then
        '    TabControl1.TabPages.Add(tabMultiMetal)
        'End If
        If studdedStone = "Y" Then
            WTabControl1.TabPages.Add(WtabStone)
        End If
        'If OthCharge = "Y" Then
        '    TabControl1.TabPages.Add(tabOtherCharges)
        'End If

        If UCase(objGPack.GetSqlValue("SELECT TAGTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wcmbItem_MAN.Text & "'")) = "Y" Then
            wcmbPurity.Enabled = True
        Else
            wcmbPurity.Enabled = False
        End If


        ''Set PurchaseTab
        Dim dt As New DataTable
        dt.Clear()
        strSql = " select 1 from " & cnAdminDb & "..SoftControl where ctlId = 'PURTAB' and ctlText = 'Y'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            'TabControl1.TabPages.Add(tabPurchase)
        End If
        wpnlMax.Enabled = False
        If calType = "R" Then
            WtxtGrossWt_Wet.Enabled = False
            WtxtLessWt_Wet.Enabled = False
            WtxtNetWt_Wet.Enabled = False

        ElseIf calType = "M" Then
            WtxtGrossWt_Wet.Enabled = True
            WtxtLessWt_Wet.Enabled = True
            WtxtNetWt_Wet.Enabled = True


            wtxtMMRate.Text = pieceRate
            If GetSoftValue("MAXMCFOCUS") = "Y" Then wpnlMax.Enabled = True
        ElseIf calType = "F" Then
            WtxtGrossWt_Wet.Enabled = True
            WtxtLessWt_Wet.Enabled = True
            WtxtNetWt_Wet.Enabled = True

            If GetSoftValue("MAXMCFOCUS") = "Y" Then wpnlMax.Enabled = True
        ElseIf calType = "B" Then
            WtxtGrossWt_Wet.Enabled = True
            WtxtLessWt_Wet.Enabled = True
            WtxtNetWt_Wet.Enabled = True
            wtxtMMRate.Enabled = True
            If GetSoftValue("MAXMCFOCUS") = "Y" Then wpnlMax.Enabled = True
        Else
            WtxtGrossWt_Wet.Enabled = True
            WtxtLessWt_Wet.Enabled = True
            WtxtNetWt_Wet.Enabled = True


            If GetSoftValue("MAXMCFOCUS") = "Y" Then wpnlMax.Enabled = True
        End If
        wtxtMetalRate_Amt.Text = Format(WGetMetalRate(), "0.00")
    End Function

    Private Sub WtxtTagNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles wtxtTagNo__Man.GotFocus
        wtxtTagNo__Man.Text = objTag.GetWTagNo(wdtpRecieptDate.Value.ToString("yyyy-MM-dd"), wcmbItem_MAN.Text, SNO)
    End Sub

    Private Sub wTabControl1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles WTabControl1.SelectedIndexChanged
        If WTabControl1.SelectedTab.Name = WtabPurchase.Name Then
            'WgrpSaveControls.Size = New Size(972, 290)
        Else
            wgrpSaveControls.Size = New Size(972, 435)
        End If
    End Sub

    Private Sub WtxtMaxMcPerGrm_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles wtxtMcPerGrm_Amt.TextChanged
        Dim mc As Double = Nothing
        Dim wast As Double = 0 ' IIf(McWithWastage, Val(txtMaxWastage_Wet.Text), 0)
        Dim mweight As Double = 0
        If _MCONGRSNET Then
            mweight = (Val(WtxtNetWt_Wet.Text) + wast)
        Else
            mweight = IIf(_MCCALCON_ITEM_GRS, Val(WtxtGrossWt_Wet.Text), Val(WtxtNetWt_Wet.Text)) + wast
        End If
        strSql = " SELECT MCASVAPER FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wcmbItem_MAN.Text & "'"
        If objGPack.GetSqlValue(strSql).ToString = "Y" Then wLabel44.Text = "MC PERCENT" Else wLabel44.Text = "Max Mc Per Gram"
        'If cmbSubItem_Man.Text <> "" Then
        '    strSql = " SELECT MCASVAPER FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & cmbSubItem_Man.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')"
        '    If objGPack.GetSqlValue(strSql).ToString = "Y" Then Label44.Text = "MC PERCENT" Else Label44.Text = "Max Mc Per Gram"
        'End If

        If wLabel44.Text = "MC PERCENT" Then mc = mweight * (Val(wtxtMetalRate_Amt.Text) * (Val(wtxtMcPerGrm_Amt.Text) / 100)) Else mc = mweight * Val(wtxtMcPerGrm_Amt.Text)
        '        mc = Math.Round(mc, McRnd)
        mc = Math.Round(mc)

        wtxtMkCharge_Amt.Text = IIf(mc <> 0, Format(mc, "0.00"), "")
    End Sub

    Private Sub WtxtSalValue_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles wtxtSalValue_Amt_Man.GotFocus
        'If Val(txtRate_Amt.Text) = 0 Then Ratevaluezero = True
        If purSalevalue > 0 Then
            If wtxtSalValue_Amt_Man.Text < purSalevalue Then
                wtxtSalValue_Amt_Man.Text = WSALEVALUE_ROUND(purSalevalue)
            End If
        Else
            WCalcFinalTotal(BlockSv)
            strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wcmbItem_MAN.Text & "'"
            wtxtSalValue_Amt_Man.ReadOnly = False
            Dim calcType As String = objGPack.GetSqlValue(strSql)
            If calcType <> "F" Then
                If SalVal_Lock = True Then
                    wtxtSalValue_Amt_Man.ReadOnly = True
                Else
                    wtxtSalValue_Amt_Man.ReadOnly = False
                End If
            End If
        End If

    End Sub
    Private Function WSALEVALUE_ROUND(ByVal svalue As Decimal) As Decimal
        If SALVALUEROUND <> 0 Then
            If svalue <> 0 Then
                Dim wholepart As Decimal = Val(svalue) / SALVALUEROUND
                Dim intpart As Decimal = Int(wholepart)
                Dim decpart As Decimal = Round(wholepart - intpart)
                svalue = (intpart + decpart) * SALVALUEROUND
            End If
        End If
        Return svalue
    End Function
    Private Sub WTagEditSave()
        Dim RowFiter() As DataRow = Nothing
        Dim COSTID As String = Nothing
        Dim itemId As Integer = Nothing
        Dim subitemId As Integer = Nothing
        Dim sizeId As Integer = Nothing
        Dim itemCtrId As Integer = Nothing
        Dim designerId As Integer = Nothing
        'Dim saleMode As String = Nothing
        Dim itemTypeId As Integer = Nothing
        Dim tranInvNo As String = Nothing
        Dim supBillno As String = Nothing

        Dim stlPcs As Integer = 0
        Dim stlWt As Double = 0
        Dim stlType As String = Nothing

        Dim dialPcs As Integer = 0
        Dim dialWt As Double = 0

        Try
            Dim strSubitemids As String = GetSelectedSubitemid(WchkcmbSubItem, False)
            tran = Nothing
            tran = cn.BeginTransaction()

            ''Find ItemId
            strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & wcmbItem_MAN.Text & "'"
            itemId = Val(objGPack.GetSqlValue(strSql, "ITEMID", , tran))

            strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & wcmbItem_MAN.Text & "' AND ITEMID='" & itemId & "'"
            subitemId = Val(objGPack.GetSqlValue(strSql, "SUBITEMID", , tran))

            ''Find Stone & Diamond
            If WTabControl1.TabPages.Contains(WtabStone) = True Then
                stlType = "G"
                stlWt = Math.Round(stlWt, 3)
                dialWt = Math.Round(dialWt, 3)
            Else
                stlPcs = 0
                stlWt = 0
                stlType = "G"
            End If
            ''Find itemTypeId
            strSql = " SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & wcmbPurity.Text & "'"
            itemTypeId = Val(objGPack.GetSqlValue(strSql, "ITEMTYPEID", , tran))

            ''FIND TRANINVNO AND SUPBILLNO
            strSql = " SELECT TRANINVNO FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = '" & SNO & "'"
            tranInvNo = objGPack.GetSqlValue(strSql, "TRANINVNO", , tran)
            If WcmbSubItem_OWN.Enabled = True Then
                ''Find SubItemId
                strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & WcmbSubItem_OWN.Text & "' AND ITEMID = " & itemId & ""
                subitemId = Val(objGPack.GetSqlValue(strSql, "SUBITEMID", , tran))
            End If

            If WchkcmbSubItem.Enabled = True Then
                ''Find SubItemId
                Dim SubItem() As String = WchkcmbSubItem.Text.Split(",")
                strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & SubItem(0) & "' AND ITEMID = " & itemId & ""
                subitemId = Val(objGPack.GetSqlValue(strSql, "SUBITEMID", , tran))
            End If


            Dim purStnValue As Double
            'For Each roStn As DataRow In dtWStoneDetails.Rows
            '    purStnValue += Val(roStn!PURVALUE.ToString)
            'Next

            strSql = " UPDATE " & cnAdminDb & "..WITEMTAG SET "
            strSql += " RECDATE = '" & wdtpRecieptDate.Value.Date.ToString("yyyy-MM-dd") & "'" 'RECDATE
            strSql += " ,COSTID = '" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
            strSql += " ,ITEMID = " & itemId & "" 'ITEMID
            strSql += " ,SUBITEMID = " & subitemId & "" 'SUBITEMID
            strSql += " ,REASON = '" & strSubitemids & "'" 'SUBITEMID
            strSql += " ,SIZEID = " & Val(sizeId) & "" 'SIZEID
            strSql += " ,ITEMCTRID = " & Val(itemCtrId) & "" 'ITEMCTRID
            strSql += " ,TABLECODE = ''"
            strSql += " ,DESIGNERID = " & Val(designerId) & "" 'DESIGNERID
            strSql += " ,PCS = " & Val(wtxtPieces_Num_Man.Text) & "" 'PCS
            strSql += " ,GRSWT = " & Val(WtxtGrossWt_Wet.Text) & "" 'GRSWT
            strSql += " ,LESSWT = " & Val(WtxtLessWt_Wet.Text) & "" 'LESSWT
            strSql += " ,NETWT = " & Val(WtxtNetWt_Wet.Text) & "" 'NETWT
            strSql += " ,RATE = " & Val(wtxtMetalRate_Amt.Text) & ""
            strSql += " ,MAXWASTPER = " & 0 & "" 'MAXWASTPER
            strSql += " ,MAXMCGRM = " & Val(wtxtMcPerGrm_Amt.Text) & "" 'MAXMCGRM
            strSql += " ,MAXWAST = " & 0 & "" 'MAXWAST
            strSql += " ,MAXMC = " & Val(wtxtMkCharge_Amt.Text) & "" 'MAXMC
            strSql += " ,MINWASTPER = " & Val(ObjMinValue.txtMinWastage_Per.Text) & "" 'MINWASTPER
            strSql += " ,MINMCGRM = " & Val(ObjMinValue.txtMinMcPerGram_Amt.Text) & "" 'MINMCGRM
            strSql += " ,MINWAST = " & Val(ObjMinValue.txtMinWastage_Wet.Text) & "" 'MINWAST
            strSql += " ,MINMC = " & Val(ObjMinValue.txtMinMkCharge_Amt.Text) & "" 'MINMC
            strSql += " ,COMPANYID =  '" & GetStockCompId() & "'" 'COMPANYID
            strSql += " ,SALVALUE = " & Val(wtxtSalValue_Amt_Man.Text) & "" 'SALVALUE
            strSql += " ,PURITY = 0" 'PURITY
            strSql += " ,NARRATION = '" & wtxtNarration.Text & "'" 'NARRATION
            strSql += " ,DESCRIP = '" & wtxtNameOfProduct_OWN.Text.Replace("'", "''") & "'" 'DESCRIP
            strSql += " ,ENTRYMODE = 'M'" 'ENTRYMODE
            strSql += " ,GRSNET = 'N'" 'GRSNET
            strSql += " ,SALEMODE = '" & calType & "'" 'SALEMODE
            strSql += " ,PCTFILE = ''"
            strSql += " ,ITEMTYPEID = " & Val(itemTypeId) & ""
            strSql += " ,TRANINVNO = '" & tranInvNo & "'"
            strSql += " ,SUPBILLNO = '" & supBillno & "'"
            strSql += " ,USERID = " & userId & ""
            strSql += " ,UPDATED = '" & Today.Date.ToString("yyyy-MM-dd") & "'"
            strSql += " ,UPTIME = '" & Date.Now.ToLongTimeString & "'"
            strSql += " ,SYSTEMID = '" & systemId & "'"
            '--LOTENTRYORDER
            strSql += " ,STYLENO = ''"
            strSql += " ,BOARDRATE = " & Val(wtxtMetalRate_Amt.Text) & ""
            strSql += " ,TRANSFERDATE = '" & wdtpRecieptDate.Value.Date.ToString("yyyy-MM-dd") & "'" 'RECDATE
            strSql += " ,RFID = ''" 'RFID
            strSql += " ,TOUCH = 0" 'TOUCH
            strSql += " ,HM_BILLNO = ''" 'HM_BILLNO
            strSql += " ,HM_CENTER = ''" 'HM_CENTER
            strSql += " ,ADD_VA_PER = " & Val(ObjPurDetail.txtpURFixedValueVa_AMT.Text) & "" 'ADD_VA_PER
            strSql += " ,REFVALUE = " & 0 & "" 'REFVALUE
            strSql += " ,EXTRAWT = " & Val(ObjExtraWt.txtExtraWt_WET.Text) & "" 'EXTRAWT
            strSql += " ,USRATE = " & IIf(NEEDUS = True And Studded_Loose = "L", Val(ObjRsUs.txtUSDollar_Amt.Text), 0) & "" 'USRATE
            strSql += " ,INDRS = " & IIf(NEEDUS = True And Studded_Loose = "L", Val(ObjRsUs.txtIndRs_Amt.Text), 0) & "" 'INDRS
            If _FourCMaintain Then
                strSql += " ,CUTID=" & TagCutId
                strSql += " ,COLORID=" & TagColorId
                strSql += " ,CLARITYID=" & TagClarityId
                strSql += " ,SHAPEID=" & TagShapeId
                strSql += " ,SETTYPEID=" & TagSetTypeId
            End If
            strSql += " ,HEIGHT=" & Val(wtxtHght_Wet.Text)
            strSql += " ,WIDTH=" & Val(wtxtWidth_Wet.Text)
            strSql += " WHERE SNO = '" & WupdIssSno & "'"
            'cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId, , )
            TagCutId = 0 : TagColorId = 0 : TagClarityId = 0 : TagShapeId = 0 : TagSetTypeId = 0 : TagHeight = 0 : TagWidth = 0
            ''DELETING STONEDETAIL
            strSql = " DELETE FROM " & cnAdminDb & "..WITEMTAGSTONE"
            strSql += " WHERE TAGSNO = '" & WupdIssSno & "'"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

            ''Inserting StoneDetail
            If WTabControl1.TabPages.Contains(WtabStone) Then
                For cnt As Integer = 0 To dtWStoneDetails.Rows.Count - 1
                    With dtWStoneDetails.Rows(cnt)
                        Dim CutId As Integer = 0
                        Dim ColorId As Integer = 0
                        Dim ClarityId As Integer = 0
                        Dim ShapeId As Integer = 0
                        Dim SetTypeId As Integer = 0
                        Dim stnItemId As Integer = 0
                        Dim stnSubItemId As Integer = 0
                        Dim stnSno As String = GetNewSno(TranSnoType.WITEMTAGSTONECODE, tran, "GET_ADMINSNO_TRAN")
                        .Item("STNSNO") = stnSno
                        'Dim caType As String = Nothing
                        strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Item("ITEM").ToString & "'"
                        stnItemId = Val(objGPack.GetSqlValue(strSql, "ITEMID", , tran))
                        strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & .Item("SUBITEM").ToString & "' AND ITEMID = " & stnItemId & ""
                        stnSubItemId = Val(objGPack.GetSqlValue(strSql, , , tran))
                        ''Inserting itemTagStone
                        strSql = " INSERT INTO " & cnAdminDb & "..WITEMTAGSTONE("
                        strSql += " SNO,TAGSNO,ITEMID,COMPANYID,STNITEMID,"
                        strSql += " STNSUBITEMID,TAGNO,STNPCS,STNWT,"
                        strSql += " STNRATE,STNAMT,DESCRIP,"
                        strSql += " RECDATE,CALCMODE,"
                        strSql += " MINRATE,SIZECODE,STONEUNIT,ISSDATE,"
                        strSql += " OLDTAGNO,CARRYFLAG,COSTID,SYSTEMID,APPVER,USRATE,INDRS,PACKETNO"
                        If _FourCMaintain Then
                            strSql += " ,CUTID,COLORID,CLARITYID,SHAPEID,SETTYPEID,HEIGHT,WIDTH"
                            CutId = Val(objGPack.GetSqlValue(" SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & .Item("CUT").ToString & "'", "CUTID", , tran))
                            ColorId = Val(objGPack.GetSqlValue(" SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME = '" & .Item("COLOR").ToString & "'", "COLORID", , tran))
                            ClarityId = Val(objGPack.GetSqlValue(" SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & .Item("CLARITY").ToString & "'", "CLARITYID", , tran))
                            ShapeId = Val(objGPack.GetSqlValue(" SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & .Item("SHAPE").ToString & "'", "SHAPEID", , tran))
                            SetTypeId = Val(objGPack.GetSqlValue(" SELECT SETTYPEID FROM " & cnAdminDb & "..STNSETTYPE WHERE SETTYPENAME = '" & .Item("SETTYPE").ToString & "'", "SETTYPEID", , tran))
                        End If
                        strSql += " )VALUES("
                        strSql += " '" & stnSno & "'" ''SNO
                        strSql += " ,'" & WupdIssSno & "'" 'TAGSNO
                        strSql += " ,'" & itemId & "'" 'ITEMID
                        strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                        strSql += " ," & stnItemId & "" 'STNITEMID
                        strSql += " ," & stnSubItemId & "" 'STNSUBITEMID
                        strSql += " ,'" & wtxtTagNo__Man.Text & "'" 'TAGNO
                        strSql += " ," & Val(.Item("PCS").ToString) & "" 'STNPCS
                        strSql += " ," & Val(.Item("WEIGHT").ToString) & "" 'STNWT
                        strSql += " ," & Val(.Item("RATE").ToString) & "" 'STNRATE
                        strSql += " ," & Val(.Item("AMOUNT").ToString) & "" 'STNAMT
                        If stnSubItemId <> 0 Then 'DESCRIP
                            strSql += " ,'" & .Item("SUBITEM").ToString & "'"
                        Else
                            strSql += " ,'" & .Item("ITEM").ToString & "'"
                        End If
                        strSql += " ,'" & wdtpRecieptDate.Value.Date.ToString("yyyy-MM-dd") & "'" 'RECDATE
                        strSql += " ,'" & .Item("CALC").ToString & "'" 'CALCMODE
                        strSql += " ,0" 'MINRATE
                        strSql += " ,0" 'SIZECODE
                        strSql += " ,'" & .Item("UNIT").ToString & "'" 'STONEUNIT
                        strSql += " ,NULL" 'ISSDATE
                        strSql += " ,''" 'OLDTAGNO
                        strSql += " ,''" 'CARRYFLAG
                        strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                        strSql += " ,'" & systemId & "'" 'SYSTEMID
                        strSql += " ,'" & VERSION & "'" 'APPVER
                        strSql += " ," & Val(.Item("USRATE").ToString) & "" 'STNRATE
                        strSql += " ," & Val(.Item("INDRS").ToString) & "" 'STNAMT
                        strSql += " ,'" & .Item("PACKETNO").ToString & "'" 'PACKETNO
                        If _FourCMaintain Then
                            strSql += " ,'" & CutId & "'" 'CUTID
                            strSql += " ,'" & ColorId & "'" 'COLORID
                            strSql += " ,'" & ClarityId & "'" 'CLARITYID
                            strSql += " ,'" & ShapeId & "'" 'SHAPEID
                            strSql += " ,'" & SetTypeId & "'" 'SETTYPEID
                            strSql += " ,'" & Val(.Item("HEIGHT").ToString) & "'" 'HEIGHT
                            strSql += " ,'" & Val(.Item("WIDTH").ToString) & "'" 'WIDTH
                        End If
                        strSql += " )"
                        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                    End With
                Next
            End If
            ''DELETING ADDITIONAL STOCK ENTRY
            strSql = " DELETE FROM " & cnAdminDb & "..WADDINFOITEMTAG"
            strSql += " WHERE TAGSNO = '" & WupdIssSno & "'"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

            ''INSERTING ADDITIONAL STOCK ENTRY

            'If wcmbAddM1.Enabled And wcmbAddM1.Text <> "" Then
            '    Dim Othid As String = Val(objGPack.GetSqlValue("SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE NAME='" & wcmbAddM1.Text & "' AND MISCID=1", , , tran))
            '    strSql = " INSERT INTO " & cnAdminDb & "..WADDINFOITEMTAG("
            '    strSql += " SNO,TAGSNO,OTHID,ITEMID,TAGNO,RECDATE,COMPANYID"
            '    strSql += " ,COSTID,SYSTEMID,APPVER"
            '    strSql += " )VALUES("
            '    strSql += " '" & GetNewSno(TranSnoType.ADDINFOITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO  
            '    strSql += " ,'" & WupdIssSno & "'" 'TAGSNO
            '    strSql += " ,'" & Othid & "'" 'OTHID
            '    strSql += " ,'" & itemId & "'" 'ITEMID
            '    strSql += " ,'" & wtxtTagNo__Man.Text & "'" 'TAGNO
            '    strSql += " ,'" & GetEntryDate(wdtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
            '    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
            '    strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
            '    strSql += " ,'" & systemId & "'" 'SYSTEMID
            '    strSql += " ,'" & VERSION & "'" 'APPVER
            '    strSql += " )"
            '    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            'End If
            'If wcmbAddM2.Enabled And wcmbAddM2.Text <> "" Then
            '    Dim Othid As String = Val(objGPack.GetSqlValue("SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE NAME='" & wcmbAddM2.Text & "' AND MISCID=2", , , tran))
            '    strSql = " INSERT INTO " & cnAdminDb & "..WADDINFOITEMTAG("
            '    strSql += " SNO,TAGSNO,OTHID,ITEMID,TAGNO,RECDATE,COMPANYID"
            '    strSql += " ,COSTID,SYSTEMID,APPVER"
            '    strSql += " )VALUES("
            '    strSql += " '" & GetNewSno(TranSnoType.ADDINFOITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO  
            '    strSql += " ,'" & WupdIssSno & "'" 'TAGSNO
            '    strSql += " ,'" & Othid & "'" 'OTHID
            '    strSql += " ,'" & itemId & "'" 'ITEMID
            '    strSql += " ,'" & wtxtTagNo__Man.Text & "'" 'TAGNO
            '    strSql += " ,'" & GetEntryDate(wdtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
            '    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
            '    strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
            '    strSql += " ,'" & systemId & "'" 'SYSTEMID
            '    strSql += " ,'" & VERSION & "'" 'APPVER
            '    strSql += " )"
            '    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            'End If
            ''INSERTING ADDITIONAL STOCK ENTRY

            If wChkcmbAddM1.Enabled And wChkcmbAddM1.Text <> "" Then
                Dim OthIds() As String = wChkcmbAddM1.Text.ToString.Split(",")
                For i As Integer = 0 To OthIds.Length - 1
                    Dim Othid As String = Val(objGPack.GetSqlValue("SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE NAME='" & OthIds(i).Trim & "' AND MISCID=1", , , tran))
                    strSql = " INSERT INTO " & cnAdminDb & "..WADDINFOITEMTAG("
                    strSql += " SNO,TAGSNO,OTHID,ITEMID,TAGNO,RECDATE,COMPANYID"
                    strSql += " ,COSTID,SYSTEMID,APPVER"
                    strSql += " )VALUES("
                    strSql += " '" & GetNewSno(TranSnoType.ADDINFOITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO  
                    strSql += " ,'" & WupdIssSno & "'" 'TAGSNO
                    strSql += " ,'" & Othid & "'" 'OTHID
                    strSql += " ,'" & itemId & "'" 'ITEMID
                    strSql += " ,'" & wtxtTagNo__Man.Text & "'" 'TAGNO
                    strSql += " ,'" & GetEntryDate(wdtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " )"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                Next
            End If
            If wChkcmbAddM2.Enabled And wChkcmbAddM2.Text <> "" Then
                Dim OthIds() As String = wChkcmbAddM2.Text.ToString.Split(",")
                For i As Integer = 0 To OthIds.Length - 1
                    Dim Othid As String = Val(objGPack.GetSqlValue("SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE NAME='" & OthIds(i).Trim & "' AND MISCID=2", , , tran))
                    strSql = " INSERT INTO " & cnAdminDb & "..WADDINFOITEMTAG("
                    strSql += " SNO,TAGSNO,OTHID,ITEMID,TAGNO,RECDATE,COMPANYID"
                    strSql += " ,COSTID,SYSTEMID,APPVER"
                    strSql += " )VALUES("
                    strSql += " '" & GetNewSno(TranSnoType.ADDINFOITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO  
                    strSql += " ,'" & WupdIssSno & "'" 'TAGSNO
                    strSql += " ,'" & Othid & "'" 'OTHID
                    strSql += " ,'" & itemId & "'" 'ITEMID
                    strSql += " ,'" & wtxtTagNo__Man.Text & "'" 'TAGNO
                    strSql += " ,'" & GetEntryDate(wdtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " )"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                Next
            End If
            If wChkcmbAddM3.Enabled And wChkcmbAddM3.Text <> "" Then
                Dim OthIds() As String = wChkcmbAddM3.Text.ToString.Split(",")
                For i As Integer = 0 To OthIds.Length - 1
                    Dim Othid As String = Val(objGPack.GetSqlValue("SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE NAME='" & OthIds(i).Trim & "' AND MISCID=3", , , tran))
                    strSql = " INSERT INTO " & cnAdminDb & "..WADDINFOITEMTAG("
                    strSql += " SNO,TAGSNO,OTHID,ITEMID,TAGNO,RECDATE,COMPANYID"
                    strSql += " ,COSTID,SYSTEMID,APPVER"
                    strSql += " )VALUES("
                    strSql += " '" & GetNewSno(TranSnoType.ADDINFOITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO  
                    strSql += " ,'" & WupdIssSno & "'" 'TAGSNO
                    strSql += " ,'" & Othid & "'" 'OTHID
                    strSql += " ,'" & itemId & "'" 'ITEMID
                    strSql += " ,'" & wtxtTagNo__Man.Text & "'" 'TAGNO
                    strSql += " ,'" & GetEntryDate(wdtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " )"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                Next
            End If
            If wChkcmbAddM4.Enabled And wChkcmbAddM4.Text <> "" Then
                Dim OthIds() As String = wChkcmbAddM4.Text.ToString.Split(",")
                For i As Integer = 0 To OthIds.Length - 1
                    Dim Othid As String = Val(objGPack.GetSqlValue("SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE NAME='" & OthIds(i).Trim & "' AND MISCID=4", , , tran))
                    strSql = " INSERT INTO " & cnAdminDb & "..WADDINFOITEMTAG("
                    strSql += " SNO,TAGSNO,OTHID,ITEMID,TAGNO,RECDATE,COMPANYID"
                    strSql += " ,COSTID,SYSTEMID,APPVER"
                    strSql += " )VALUES("
                    strSql += " '" & GetNewSno(TranSnoType.ADDINFOITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO  
                    strSql += " ,'" & WupdIssSno & "'" 'TAGSNO
                    strSql += " ,'" & Othid & "'" 'OTHID
                    strSql += " ,'" & itemId & "'" 'ITEMID
                    strSql += " ,'" & wtxtTagNo__Man.Text & "'" 'TAGNO
                    strSql += " ,'" & GetEntryDate(wdtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " )"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                Next
            End If
            If wChkcmbAddM5.Enabled And wChkcmbAddM5.Text <> "" Then
                Dim OthIds() As String = wChkcmbAddM5.Text.ToString.Split(",")
                For i As Integer = 0 To OthIds.Length - 1
                    Dim Othid As String = Val(objGPack.GetSqlValue("SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE NAME='" & OthIds(i).Trim & "' AND MISCID=5", , , tran))
                    strSql = " INSERT INTO " & cnAdminDb & "..WADDINFOITEMTAG("
                    strSql += " SNO,TAGSNO,OTHID,ITEMID,TAGNO,RECDATE,COMPANYID"
                    strSql += " ,COSTID,SYSTEMID,APPVER"
                    strSql += " )VALUES("
                    strSql += " '" & GetNewSno(TranSnoType.ADDINFOITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO  
                    strSql += " ,'" & WupdIssSno & "'" 'TAGSNO
                    strSql += " ,'" & Othid & "'" 'OTHID
                    strSql += " ,'" & itemId & "'" 'ITEMID
                    strSql += " ,'" & wtxtTagNo__Man.Text & "'" 'TAGNO
                    strSql += " ,'" & GetEntryDate(wdtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " )"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                Next
            End If
            If wChkcmbAddM6.Enabled And wChkcmbAddM6.Text <> "" Then
                Dim OthIds() As String = wChkcmbAddM6.Text.ToString.Split(",")
                For i As Integer = 0 To OthIds.Length - 1
                    Dim Othid As String = Val(objGPack.GetSqlValue("SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE NAME='" & OthIds(i).Trim & "' AND MISCID=6", , , tran))
                    strSql = " INSERT INTO " & cnAdminDb & "..WADDINFOITEMTAG("
                    strSql += " SNO,TAGSNO,OTHID,ITEMID,TAGNO,RECDATE,COMPANYID"
                    strSql += " ,COSTID,SYSTEMID,APPVER"
                    strSql += " )VALUES("
                    strSql += " '" & GetNewSno(TranSnoType.ADDINFOITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO  
                    strSql += " ,'" & WupdIssSno & "'" 'TAGSNO
                    strSql += " ,'" & Othid & "'" 'OTHID
                    strSql += " ,'" & itemId & "'" 'ITEMID
                    strSql += " ,'" & wtxtTagNo__Man.Text & "'" 'TAGNO
                    strSql += " ,'" & GetEntryDate(wdtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " )"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                Next
            End If
            If wChkcmbAddM7.Enabled And wChkcmbAddM7.Text <> "" Then
                Dim OthIds() As String = wChkcmbAddM7.Text.ToString.Split(",")
                For i As Integer = 0 To OthIds.Length - 1
                    Dim Othid As String = Val(objGPack.GetSqlValue("SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE NAME='" & OthIds(i).Trim & "' AND MISCID=7", , , tran))
                    strSql = " INSERT INTO " & cnAdminDb & "..WADDINFOITEMTAG("
                    strSql += " SNO,TAGSNO,OTHID,ITEMID,TAGNO,RECDATE,COMPANYID"
                    strSql += " ,COSTID,SYSTEMID,APPVER"
                    strSql += " )VALUES("
                    strSql += " '" & GetNewSno(TranSnoType.ADDINFOITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO  
                    strSql += " ,'" & WupdIssSno & "'" 'TAGSNO
                    strSql += " ,'" & Othid & "'" 'OTHID
                    strSql += " ,'" & itemId & "'" 'ITEMID
                    strSql += " ,'" & wtxtTagNo__Man.Text & "'" 'TAGNO
                    strSql += " ,'" & GetEntryDate(wdtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " )"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                Next
            End If
            If wChkcmbAddM8.Enabled And wChkcmbAddM8.Text <> "" Then
                Dim OthIds() As String = wChkcmbAddM8.Text.ToString.Split(",")
                For i As Integer = 0 To OthIds.Length - 1
                    Dim Othid As String = Val(objGPack.GetSqlValue("SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE NAME='" & OthIds(i).Trim & "' AND MISCID=8", , , tran))
                    strSql = " INSERT INTO " & cnAdminDb & "..WADDINFOITEMTAG("
                    strSql += " SNO,TAGSNO,OTHID,ITEMID,TAGNO,RECDATE,COMPANYID"
                    strSql += " ,COSTID,SYSTEMID,APPVER"
                    strSql += " )VALUES("
                    strSql += " '" & GetNewSno(TranSnoType.ADDINFOITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO  
                    strSql += " ,'" & WupdIssSno & "'" 'TAGSNO
                    strSql += " ,'" & Othid & "'" 'OTHID
                    strSql += " ,'" & itemId & "'" 'ITEMID
                    strSql += " ,'" & wtxtTagNo__Man.Text & "'" 'TAGNO
                    strSql += " ,'" & GetEntryDate(wdtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " )"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                Next
            End If
            If wChkcmbAddM9.Enabled And wChkcmbAddM9.Text <> "" Then
                Dim OthIds() As String = wChkcmbAddM9.Text.ToString.Split(",")
                For i As Integer = 0 To OthIds.Length - 1
                    Dim Othid As String = Val(objGPack.GetSqlValue("SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE NAME='" & OthIds(i).Trim & "' AND MISCID=9", , , tran))
                    strSql = " INSERT INTO " & cnAdminDb & "..WADDINFOITEMTAG("
                    strSql += " SNO,TAGSNO,OTHID,ITEMID,TAGNO,RECDATE,COMPANYID"
                    strSql += " ,COSTID,SYSTEMID,APPVER"
                    strSql += " )VALUES("
                    strSql += " '" & GetNewSno(TranSnoType.ADDINFOITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO  
                    strSql += " ,'" & WupdIssSno & "'" 'TAGSNO
                    strSql += " ,'" & Othid & "'" 'OTHID
                    strSql += " ,'" & itemId & "'" 'ITEMID
                    strSql += " ,'" & wtxtTagNo__Man.Text & "'" 'TAGNO
                    strSql += " ,'" & GetEntryDate(wdtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " )"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                Next
            End If
            If wChkcmbAddM10.Enabled And wChkcmbAddM10.Text <> "" Then
                Dim OthIds() As String = wChkcmbAddM10.Text.ToString.Split(",")
                For i As Integer = 0 To OthIds.Length - 1
                    Dim Othid As String = Val(objGPack.GetSqlValue("SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE NAME='" & OthIds(i).Trim & "' AND MISCID=10", , , tran))
                    strSql = " INSERT INTO " & cnAdminDb & "..WADDINFOITEMTAG("
                    strSql += " SNO,TAGSNO,OTHID,ITEMID,TAGNO,RECDATE,COMPANYID"
                    strSql += " ,COSTID,SYSTEMID,APPVER"
                    strSql += " )VALUES("
                    strSql += " '" & GetNewSno(TranSnoType.ADDINFOITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO  
                    strSql += " ,'" & WupdIssSno & "'" 'TAGSNO
                    strSql += " ,'" & Othid & "'" 'OTHID
                    strSql += " ,'" & itemId & "'" 'ITEMID
                    strSql += " ,'" & wtxtTagNo__Man.Text & "'" 'TAGNO
                    strSql += " ,'" & GetEntryDate(wdtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " )"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                Next
            End If
            If wChkcmbAddM11.Enabled And wChkcmbAddM11.Text <> "" Then
                Dim OthIds() As String = wChkcmbAddM11.Text.ToString.Split(",")
                For i As Integer = 0 To OthIds.Length - 1
                    Dim Othid As String = Val(objGPack.GetSqlValue("SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE NAME='" & OthIds(i).Trim & "' AND MISCID=11", , , tran))
                    strSql = " INSERT INTO " & cnAdminDb & "..WADDINFOITEMTAG("
                    strSql += " SNO,TAGSNO,OTHID,ITEMID,TAGNO,RECDATE,COMPANYID"
                    strSql += " ,COSTID,SYSTEMID,APPVER"
                    strSql += " )VALUES("
                    strSql += " '" & GetNewSno(TranSnoType.ADDINFOITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO  
                    strSql += " ,'" & WupdIssSno & "'" 'TAGSNO
                    strSql += " ,'" & Othid & "'" 'OTHID
                    strSql += " ,'" & itemId & "'" 'ITEMID
                    strSql += " ,'" & wtxtTagNo__Man.Text & "'" 'TAGNO
                    strSql += " ,'" & GetEntryDate(wdtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " )"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                Next
            End If
            If wChkcmbAddM12.Enabled And wChkcmbAddM12.Text <> "" Then
                Dim OthIds() As String = wChkcmbAddM12.Text.ToString.Split(",")
                For i As Integer = 0 To OthIds.Length - 1
                    Dim Othid As String = Val(objGPack.GetSqlValue("SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE NAME='" & OthIds(i).Trim & "' AND MISCID=12", , , tran))
                    strSql = " INSERT INTO " & cnAdminDb & "..WADDINFOITEMTAG("
                    strSql += " SNO,TAGSNO,OTHID,ITEMID,TAGNO,RECDATE,COMPANYID"
                    strSql += " ,COSTID,SYSTEMID,APPVER"
                    strSql += " )VALUES("
                    strSql += " '" & GetNewSno(TranSnoType.ADDINFOITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO  
                    strSql += " ,'" & WupdIssSno & "'" 'TAGSNO
                    strSql += " ,'" & Othid & "'" 'OTHID
                    strSql += " ,'" & itemId & "'" 'ITEMID
                    strSql += " ,'" & wtxtTagNo__Man.Text & "'" 'TAGNO
                    strSql += " ,'" & GetEntryDate(wdtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " )"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                Next
            End If
            'If wChkcmbAddM10.Enabled And wChkcmbAddM10.Text <> "" Then
            '    Dim OthIds() As String = wChkcmbAddM10.Text.ToString.Split(",")
            '    For i As Integer = 0 To OthIds.Length - 1
            '        Dim Othid As String = Val(objGPack.GetSqlValue("SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE NAME='" & OthIds(i).Trim & "' AND MISCID=10", , , tran))
            '        strSql = " INSERT INTO " & cnAdminDb & "..WADDINFOITEMTAG("
            '        strSql += " SNO,TAGSNO,OTHID,ITEMID,TAGNO,RECDATE,COMPANYID"
            '        strSql += " ,COSTID,SYSTEMID,APPVER"
            '        strSql += " )VALUES("
            '        strSql += " '" & GetNewSno(TranSnoType.ADDINFOITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO  
            '        strSql += " ,'" & WupdIssSno & "'" 'TAGSNO
            '        strSql += " ,'" & Othid & "'" 'OTHID
            '        strSql += " ,'" & itemId & "'" 'ITEMID
            '        strSql += " ,'" & wtxtTagNo__Man.Text & "'" 'TAGNO
            '        strSql += " ,'" & GetEntryDate(wdtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
            '        strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
            '        strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
            '        strSql += " ,'" & systemId & "'" 'SYSTEMID
            '        strSql += " ,'" & VERSION & "'" 'APPVER
            '        strSql += " )"
            '        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            '    Next
            'End If
            If Tagsave = True Then TagEditSave()

            tran.Commit()
            tran = Nothing
            Dim prnmemsuffix As String = ""
            If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId

            MsgBox(wtxtTagNo__Man.Text + E0009, MsgBoxStyle.Exclamation)
            Me.Dispose()
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            If Not tran Is Nothing Then tran.Dispose()
        End Try

    End Sub

    Private Sub WUpdateTag()
        If objGPack.Validator_Check(wgrpSaveControls) Then Exit Sub

        Dim ds As New DataSet
        ds.Clear()
        If WtxtLessWt_Wet.Enabled = True Then
            'If Val(WtxtGrossWt_Wet.Text) <> 0 And Val(WtxtLessWt_Wet.Text) >= Val(WtxtGrossWt_Wet.Text) Then
            If Val(WtxtGrossWt_Wet.Text) <> 0 And Val(WtxtLessWt_Wet.Text) > Val(WtxtGrossWt_Wet.Text) Then
                MsgBox(E0004 + Me.GetNextControl(WtxtLessWt_Wet, False).Text, MsgBoxStyle.Exclamation)
                WtxtLessWt_Wet.Focus()
                Exit Sub
            End If
        End If
        If WtxtNetWt_Wet.Enabled = True Then
            If Val(WtxtGrossWt_Wet.Text) <> 0 And Val(WtxtNetWt_Wet.Text) < 0 Then
                MsgBox(Me.GetNextControl(WtxtNetWt_Wet, False).Text + E0001, MsgBoxStyle.Exclamation)
                WtxtNetWt_Wet.Focus()
                Exit Sub
            End If
            If Val(WtxtNetWt_Wet.Text) > Val(WtxtGrossWt_Wet.Text) Then
                MsgBox(Me.GetNextControl(WtxtNetWt_Wet, False).Text + E0015 + Me.GetNextControl(WtxtGrossWt_Wet, False).Text, MsgBoxStyle.Exclamation)
                WtxtNetWt_Wet.Focus()
                Exit Sub
            End If
        End If
        WTagEditSave()
        If GetAdmindbSoftValue("USERLEVELPWD", "N") = "Y" Then
            Dim Sqlqry As String = "Select Optionid from " & cnAdminDb & "..PRJPWDOPTION where OPTIONNAME ='TAGEDIT' AND active = 'Y'"
            Dim Optionid As Integer = Val("" & objGPack.GetSqlValue(Sqlqry, , , tran))
            If Optionid <> 0 Then
                strSql = " INSERT INTO " & cnAdminDb & "..EXEMPTION"
                strSql += " ("
                strSql += " Exempid,OPTIONID,BATCHNO,COSTID,EXEMPDATE,EXEMPTIME,EXEMPUSER,EXEMPOPEN,DESCRIPTION"
                strSql += " )"
                strSql += " VALUES"
                strSql += " ("
                strSql += " " & Val(objGPack.GetMax("Exempid", "EXEMPTION", cnAdminDb, tran).ToString)
                strSql += " ," & Optionid
                strSql += " ,'" & updIssSno & "'" 'ISSSNO
                strSql += " ,'" & cnCostId & "'"
                strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'"
                strSql += " ,'" & Date.Now.ToLongTimeString & "'"
                strSql += " ," & userId
                strSql += " ,'OTP'"
                strSql += " ,'Tag edit allowed'"
                strSql += " )"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            End If
        End If

    End Sub

    Private Sub WbtnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WbtnSave.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If Not tagEdit Then
            If STKAFINDATE = False Then
                If Not CheckDate(wdtpRecieptDate.Value) Then Exit Sub
                If CheckEntryDate(wdtpRecieptDate.Value) Then Exit Sub
            End If
        End If
        strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wcmbItem_MAN.Text & "'"
        Dim calcType As String = objGPack.GetSqlValue(strSql)
        ''Weight Rate Validation
        Select Case calcType.ToUpper
            Case "W"
                If Val(WtxtGrossWt_Wet.Text) = 0 Then
                    MsgBox("Grs Weight should not empty", MsgBoxStyle.Information)
                    WtxtGrossWt_Wet.Focus()
                    Exit Sub
                End If
            Case "R"
                'If Val(txtRate_Amt.Text) = 0 Then
                '    MsgBox("Rate should not empty", MsgBoxStyle.Information)
                '    txtRate_Amt.Focus()
                '    Exit Sub
                'End If
            Case "B"
                If Val(WtxtGrossWt_Wet.Text) = 0 Then
                    MsgBox("Grs Weight should not empty", MsgBoxStyle.Information)
                    WtxtGrossWt_Wet.Focus()
                    Exit Sub
                End If
                'If Val(txtRate_Amt.Text) = 0 Then
                '    MsgBox("Rate should not empty", MsgBoxStyle.Information)
                '    txtRate_Amt.Focus()
                '    Exit Sub
                'End If
            Case "F"
                If Val(WtxtGrossWt_Wet.Text) = 0 Then
                    MsgBox("Weight should not empty", MsgBoxStyle.Information)
                    WtxtGrossWt_Wet.Focus()
                    Exit Sub
                End If
        End Select

nnnext:
        If picPath <> Nothing Then
            If Not IO.Directory.Exists(defalutDestination) Then
                MsgBox(defalutDestination & vbCrLf & " Destination Path not found" & vbCrLf & "Please make the correct path", MsgBoxStyle.Information)
                Exit Sub
            End If
        End If
        If tagEdit Then
            WUpdateTag()
            Exit Sub
        End If
        If objGPack.Validator_Check(wgrpSaveControls) Then Exit Sub
        Dim ds As New DataSet
        ds.Clear()
        Dim tDd As Date = GetServerDate()
        If GetAdmindbSoftValue("GLOBALDATE").ToUpper = "Y" Then
            tDd = GetEntryDate(tDd)
        End If
        If Not (wdtpRecieptDate.Value.Date.ToString("yyyy-MM-dd") >= lotRecieptDate.Date.ToString("yyyy-MM-dd")) Then
            Dim errStr As String
            errStr = "Reciept Date Should not allow before LotDate"
            'errStr += " And Receipt Date Should not Exceed Today Date"
            MsgBox(errStr, MsgBoxStyle.Exclamation)
            wdtpRecieptDate.Focus()
            Exit Sub
        End If
        If Not tagEdit Then
            If Not wdtpRecieptDate.Value.Date.ToString("yyyy-MM-dd") <= tDd.ToString("yyyy-MM-dd") Then
                Dim errStr As String
                errStr = "Receipt Date Should not Exceed Today Date"
                MsgBox(errStr, MsgBoxStyle.Exclamation)
                wdtpRecieptDate.Focus()
                Exit Sub
            End If
        End If

        If WtxtLessWt_Wet.Enabled = True Then
            If Val(WtxtGrossWt_Wet.Text) <> 0 And Val(WtxtLessWt_Wet.Text) > Val(WtxtGrossWt_Wet.Text) Then
                'If Val(WtxtGrossWt_Wet.Text) <> 0 And Val(WtxtLessWt_Wet.Text) >= Val(WtxtGrossWt_Wet.Text) Then
                MsgBox(E0004 + Me.GetNextControl(WtxtLessWt_Wet, False).Text, MsgBoxStyle.Exclamation)
                WtxtLessWt_Wet.Focus()
                Exit Sub
            End If
        End If
        If WtxtNetWt_Wet.Enabled = True Then
            If Val(WtxtGrossWt_Wet.Text) <> 0 And Val(WtxtNetWt_Wet.Text) <= 0 And Val(WtxtGrossWt_Wet.Text) <> Val(WtxtLessWt_Wet.Text) Then
                MsgBox("Net Weight Should not Empty", MsgBoxStyle.Exclamation)
                WtxtNetWt_Wet.Focus()
                Exit Sub
            End If
            If Val(WtxtNetWt_Wet.Text) > Val(WtxtGrossWt_Wet.Text) Then
                MsgBox(Me.GetNextControl(WtxtNetWt_Wet, False).Text + E0015 + Me.GetNextControl(WtxtGrossWt_Wet, False).Text, MsgBoxStyle.Exclamation)
                WtxtNetWt_Wet.Focus()
                Exit Sub
            End If
        End If
        If Val(ObjMinValue.txtMinMcPerGram_Amt.Text) > Val(wtxtMcPerGrm_Amt.Text) Then
            MsgBox("Min Mc Per Grm Should not Exceed Max Mc Per Grm", MsgBoxStyle.Information)
            ObjMinValue.txtMinMcPerGram_Amt.Focus()
            Exit Sub
        End If
        If Val(ObjMinValue.txtMinMkCharge_Amt.Text) > Val(wtxtMkCharge_Amt.Text) Then
            MsgBox("Min Making Charge Should not Exceed Max Making Charge", MsgBoxStyle.Information)
            ObjMinValue.txtMinMkCharge_Amt.Focus()
            Exit Sub
        End If
        Try
            wfuncAdd()
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub WbtnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WbtnNew.Click
        WfuncNew()
    End Sub

    Private Sub WbtnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WbtnExit.Click
        ItemLock = False
        Me.Close()
    End Sub

    Private Sub WSaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles wSaveToolStripMenuItem.Click
        WbtnSave_Click(Me, New EventArgs)
    End Sub

    Private Sub WNewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles wNewToolStripMenuItem.Click
        WbtnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub WExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles wExitToolStripMenuItem.Click
        WbtnExit_Click(Me, New EventArgs)
    End Sub


    Private Sub WEditGridToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles wEditGridToolStripMenuItem.Click
        If WTabControl1.SelectedTab.Name = WtabStone.Name Then
            wgridStone.Focus()
        ElseIf WTabControl1.SelectedTab.Name = WtabMultiMetal.Name Then
            wgridMultimetal.Focus()
        ElseIf WTabControl1.SelectedTab.Name = WtabOtherCharges.Name Then
            wgridMisc.Focus()
        End If
    End Sub


    Private Sub WCalcMaxMinValues()
        strSql = Nothing
        Dim type As String
        If Not OrderRow Is Nothing Then
            'objGPack.TextClear(pnlValueAdded)
            objGPack.TextClear(wpnlMax)
            objGPack.TextClear(ObjMinValue)

            If Val(OrderRow!MCGRM.ToString) = 0 Then
                wtxtMkCharge_Amt.Text = Math.Round(IIf(Val(OrderRow!MC.ToString) <> 0, Format(Val(OrderRow!MC.ToString), "0.00"), ""), 2)
            Else
                wtxtMcPerGrm_Amt.Text = Math.Round(IIf(Val(OrderRow!MCGRM.ToString) <> 0, Format(Val(OrderRow!MCGRM.ToString), "0.00"), ""), 2)
            End If

        End If

        If Val(WtxtGrossWt_Wet.Text) = 0 Then Exit Sub
        If Val(WtxtNetWt_Wet.Text) = 0 Then Exit Sub
        type = objGPack.GetSqlValue(" SELECT VALUEADDEDTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wcmbItem_MAN.Text & "'") 'LOTNO = " & Val(txtLotNo_Num_Man.Text) & "")
        Select Case type
            Case "T"
                strSql = " DECLARE @WT FLOAT"
                strSql += vbCrLf + " SET @WT = " & Val(WtxtNetWt_Wet.Text) & ""

                strSql += vbCrLf + " SELECT TOUCH,MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC,TOUCH_PUR,MAXWASTPER_PUR,MAXMCGRM_PUR,MAXWAST_PUR,MAXMC_PUR"
                strSql += vbCrLf + " ,MINWASTPER,MINMCGRM,MINWAST,MINMC FROM " & cnAdminDb & "..WMCTABLE "
                strSql += vbCrLf + " WHERE TABLECODE = ''" ' & cmbTableCode.Text & "'"
                strSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
                strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
                'strSql += vbCrLf + " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_Man.Text & "'),'')"
            Case "I"
                Dim dpcs As Integer = Val(dtWStoneDetails.Compute("SUM(PCS)", "METALID='D'").ToString)

                strSql = "select count(*) FROM " & cnAdminDb & "..WMCTABLE "
                strSql += vbCrLf + " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wcmbItem_MAN.Text & "') and isnull(diafrompcs,0) <> 0 and isnull(diatopcs,0) <> 0 "
                If Val(objGPack.GetSqlValue(strSql).ToString) = 0 Then dpcs = 0

                strSql = " DECLARE @WT FLOAT,@DIAPCS INT"
                strSql += vbCrLf + " SET @WT = " & Val(WtxtNetWt_Wet.Text) & ""

                strSql += vbCrLf + " SET @DIAPCS = " & dpcs & ""
                strSql += vbCrLf + " SELECT TOUCH,MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC,TOUCH_PUR,MAXWASTPER_PUR,MAXMCGRM_PUR,MAXWAST_PUR,MAXMC_PUR"
                strSql += vbCrLf + " ,MINWASTPER,MINMCGRM,MINWAST,MINMC FROM " & cnAdminDb & "..WMCTABLE "
                strSql += vbCrLf + " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wcmbItem_MAN.Text & "')"
                'strSql += vbCrLf + " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "' AND ITEMID = ( SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')),0)"
                strSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
                If dpcs <> 0 Then strSql += vbCrLf + " AND @DIAPCS BETWEEN DIAFROMPCS AND DIATOPCS "
                strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
                strSql += vbCrLf + " AND ISNULL(TABLECODE,'') = ''"
                'strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_Man.Text & "'),'')"
            Case "D"
                strSql = " DECLARE @WT FLOAT"
                strSql += vbCrLf + " SET @WT = " & Val(WtxtNetWt_Wet.Text) & ""

                strSql += vbCrLf + " SELECT TOUCH,MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC,TOUCH_PUR,MAXWASTPER_PUR,MAXMCGRM_PUR,MAXWAST_PUR,MAXMC_PUR"
                strSql += vbCrLf + " ,MINWASTPER,MINMCGRM,MINWAST,MINMC FROM " & cnAdminDb & "..WMCTABLE "
                strSql += vbCrLf + " WHERE ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wcmbItem_MAN.Text & "' UNION SELECT 0)"
                'strSql += vbCrLf + " AND SUBITEMID IN (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')  UNION SELECT 0)"
                'strSql += vbCrLf + " AND DESIGNERID = ISNULL((SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesigner_MAN.Text & "'),0)"
                strSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
                strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
                strSql += vbCrLf + " AND ISNULL(TABLECODE,'') = ''"
                'strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_Man.Text & "'),'')"
            Case "P"
                strSql = " DECLARE @WT FLOAT"
                strSql += vbCrLf + " SET @WT = " & Val(WtxtNetWt_Wet.Text) & ""

                strSql += vbCrLf + " SELECT TOUCH,MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC,TOUCH_PUR,MAXWASTPER_PUR,MAXMCGRM_PUR,MAXWAST_PUR,MAXMC_PUR"
                strSql += vbCrLf + " ,MINWASTPER,MINMCGRM,MINWAST,MINMC FROM " & cnAdminDb & "..WMCTABLE "
                strSql += vbCrLf + " WHERE ITEMTYPE = (SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & wcmbPurity.Text & "')"
                strSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
                strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
                'strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_Man.Text & "'),'')"
        End Select
        If type = Nothing Then
            Exit Sub
        End If
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            If Not tagEdit Then
                If PUR_AUTOCALC Then
                    ObjPurDetail.txtPurTouch_Amt.Clear()
                    ObjPurDetail.txtPurMakingChrg_Amt.Clear()
                    ObjPurDetail.txtPurWastage_Wet.Clear()
                    ObjPurDetail.txtPurMcPerGrm_Amt.Text = lotPurMcPerGrm
                    ObjPurDetail.txtPurWastage_Per.Text = lotPurWastagePer
                    ObjPurDetail.txtPurTouch_Amt.Text = lotPurTouch
                End If
                ' txtTouch_AMT.Clear()
                'objGPack.TextClear(pnlValueAdded)
                objGPack.TextClear(wpnlMax)
                objGPack.TextClear(ObjMinValue)
                If type = "I" Then
                    If GetSoftValue("MAXMCFOCUS") = "N" Then wpnlMax.Enabled = True
                End If
                Exit Sub
            End If
        Else
            'txtTouch_AMT.Clear()
            objGPack.TextClear(wpnlMax)
            objGPack.TextClear(ObjMinValue)
            'objGPack.TextClear(pnlValueAdded)
            With dt.Rows(0)

                Dim wmcWastPer As Double = Val(.Item("MAXWASTPER").ToString)
                Dim wmcWast As Double = Val(.Item("MAXWAST").ToString)
                Dim wmcMcGrm As Double = Val(.Item("MAXMCGRM").ToString)
                Dim wmcMc As Double = Val(.Item("MAXMC").ToString)
                If type = "I" Then
                    If GetSoftValue("MAXMCFOCUS") = "N" Then
                        If wmcWastPer = 0 And wmcWast = 0 And wmcMcGrm = 0 And wmcMc = 0 Then wpnlMax.Enabled = True Else wpnlMax.Enabled = False
                    End If
                    If dt.Rows.Count > 1 Then wmcMcGrm = Val(dt.Rows(1).Item("Maxmcgrm").ToString) : wmcMc = Val(dt.Rows(1).Item("Maxmc").ToString)
                End If

                If wmcMcGrm = 0 Then
                    wtxtMkCharge_Amt.Text = IIf(Val(.Item("MAXMC").ToString) <> 0, Format(Val(.Item("MAXMC").ToString), "0.00"), "")
                Else
                    wtxtMcPerGrm_Amt.Text = IIf(Val(.Item("MAXMCGRM").ToString) <> 0, Format(Val(.Item("MAXMCGRM").ToString), "0.00"), "")
                End If
                wmcWastPer = Val(.Item("MINWASTPER").ToString)
                wmcWast = Val(.Item("MINWAST").ToString)
                wmcMcGrm = Val(.Item("MINMCGRM").ToString)
                wmcMc = Val(.Item("MINMC").ToString)
                '  txtTouch_AMT.Text = IIf(Val(.Item("TOUCH").ToString) <> 0, Format(Val(.Item("TOUCH").ToString), "0.00"), "")
                If wmcWastPer = 0 Then
                    ObjMinValue.txtMinWastage_Wet.Text = IIf(Val(.Item("MINWAST").ToString) <> 0, Format(Val(.Item("MINWAST").ToString), "0.000"), "")
                Else
                    ObjMinValue.txtMinWastage_Per.Text = IIf(Val(.Item("MINWASTPER").ToString) <> 0, Format(Val(.Item("MINWASTPER").ToString), "0.00"), "")
                End If
                If wmcMcGrm = 0 Then
                    ObjMinValue.txtMinMkCharge_Amt.Text = IIf(Val(.Item("MINMC").ToString) <> 0, Format(Val(.Item("MINMC").ToString), "0.00"), "")
                Else
                    ObjMinValue.txtMinMcPerGram_Amt.Text = IIf(Val(.Item("MINMCGRM").ToString) <> 0, Format(Val(.Item("MINMCGRM").ToString), "0.00"), "")
                End If


            End With
        End If

    End Sub


    Private Sub WGetGrsWeightFromPort()

        Dim Wt_Balance_Sep As String = GetAdmindbSoftValue("WT_BALANCE_SEP", "")
        Dim Weight As Double = Nothing
        Try
            If wSerialPort1.IsOpen Then wSerialPort1.Close()
            wSerialPort1.Open()
            If wSerialPort1.IsOpen Then
                Dim readStr As String = Nothing
                If Wt_Balance_Sep <> "" Then
                    readStr = UCase(wSerialPort1.ReadTo(Wt_Balance_Sep))
                    If IsNumeric(readStr.Substring(readStr.Length - 6)) Then
                        readStr = (Val(readStr.Substring(readStr.Length - 6)) / 1000).ToString()
                    End If
                    Weight = Val(Trim(readStr))
                Else
                    For cnt As Integer = 1 To 10
                        readStr = UCase(wSerialPort1.ReadLine)
                        If readStr.Contains(".") Then
                            Exit For
                        End If
                    Next
                    Dim wt() As String = readStr.Split(Environment.NewLine)
                    Dim wet As String = ""
                    For Each c As Char In wt(0)
                        If c = "," Then Continue For
                        If Char.IsPunctuation(c) Then wet += c
                        If Char.IsNumber(c) Then wet += c
                    Next
                    Weight = Val(Trim(wet))
                    wReadData.Text = readStr
                    wSplitData.Text = wt(0)
                    wModifyData.Text = wet
                End If
            End If
            If wSerialPort1.IsOpen Then wSerialPort1.Close()
        Catch ex As Exception
            wtxtTagNo__Man.Focus()
            MsgBox("Please check com connection" + vbCrLf + ex.Message + vbCrLf + ex.StackTrace)
            If wSerialPort1.IsOpen Then wSerialPort1.Close()
        End Try
        If Weight = 0 Then WtxtGrossWt_Wet.Text = "" : Exit Sub
        Dim rndDigit As Integer = 0
        Dim METALID As String = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wcmbItem_MAN.Text & "'")
        If METALID = "S" Then
            rndDigit = Val(GetSoftValue("ROUNDOFF-SILVER"))
        ElseIf METALID = "G" Then
            rndDigit = Val(GetSoftValue("ROUNDOFF-GOLD"))
        Else
            rndDigit = 3
        End If
        Weight = Math.Round(Weight, rndDigit)
        WtxtGrossWt_Wet.Text = IIf(Weight <> 0, Format(Weight, "0.000"), Nothing)
        WtxtGrossWt_Wet.SelectAll()
    End Sub


    Private Sub WtxtGrossWt_Wet_Man_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles WtxtGrossWt_Wet.GotFocus
        If tagEdit Then Exit Sub
        'If chkAutomaticWt.Checked = True Then
        '    WtxtGrossWt_Wet.ReadOnly = True
        '    WGetGrsWeightFromPort()
        'Else
        WtxtGrossWt_Wet.ReadOnly = False
        'End If
    End Sub



    Private Sub WtxtGrossWt_Wet_Man_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles WtxtGrossWt_Wet.KeyPress
        If e.KeyChar = Chr(Keys.Enter) And Val(WtxtGrossWt_Wet.Text) = 0 Then

            strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wcmbItem_MAN.Text & "'"

            Dim calcType As String = objGPack.GetSqlValue(strSql)
            ''Weight Rate Validation
            Select Case calcType.ToUpper
                Case "W"
                    If Val(WtxtGrossWt_Wet.Text) = 0 Then
                        MsgBox("Grs Weight should not empty", MsgBoxStyle.Information)
                        WtxtGrossWt_Wet.Focus()
                        Exit Sub
                    End If
                Case "B"
                    If Val(WtxtGrossWt_Wet.Text) = 0 Then
                        MsgBox("Grs Weight should not empty", MsgBoxStyle.Information)
                        WtxtGrossWt_Wet.Focus()
                        Exit Sub
                    End If
                    If Val(wtxtMMRate.Text) = 0 Then
                        MsgBox("Rate should not empty", MsgBoxStyle.Information)
                        wtxtMMRate.Focus()
                        Exit Sub
                    End If
                Case "F"
                    SendKeys.Send("{TAB}")
                    Exit Sub
            End Select
            Exit Sub
        End If
        If e.KeyChar = Chr(Keys.Enter) And Val(WtxtGrossWt_Wet.Text) > 0 Then


            Dim tempitemid As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') = '" & wcmbItem_MAN.Text & "'").ToString)
            Dim tempSitemid As Integer

            If CHKBOOKSTOCK = True Then
                'Check Bookstock
                Dim tempcatcode As String = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') = '" & wcmbItem_MAN.Text & "'").ToString
                TChkbStk = False
                If wCheckBookstk(tempcatcode) < 0 Then
                    MsgBox("Closing Book stock weight is less than zero.")
                    WtxtGrossWt_Wet.Text = ""
                    WbtnSave.Enabled = False
                    WtxtGrossWt_Wet.Focus()
                    Exit Sub
                End If
            End If

            If Mid(STUDDEDWTPER, 1, 1) <> "N" And objGPack.GetSqlValue("SELECT ISNULL(STUDDEDSTONE,'')STUDDEDSTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wcmbItem_MAN.Text & "'", , "N", ) = "Y" Then
                objstudPer = New StuddedDeDuctPer
                If objstudPer.ShowDialog = Windows.Forms.DialogResult.OK Then
                    Dim StudDet() As String = STUDDEDWTPER.Split(",")
                    Dim Stnitemid As Integer = 0
                    If StudDet.Length > 1 Then Stnitemid = Val(StudDet(1).ToString) 'Val(dtdesgstnrow(2).ToString)
                    Dim StudRate As Decimal = 0
                    If StudDet.Length > 2 Then StudRate = Val(StudDet(2).ToString)
                    If Stnitemid = 0 Then Stnitemid = 9999
                    Dim StudDeductPer As Decimal = Val(objstudPer.txtStuddedPer_PER.Text)


                    Dim StnSitemid As Integer = 0 'Val(dtdesgstnrow(3).ToString)
                    Dim dtstnitemdr As DataRow
                    Dim dtstnsitemdr As DataRow
                    Dim tempwtunit As String
                    Dim tempcalmode As String
                    dtstnitemdr = GetSqlRow("SELECT ITEMNAME,CALTYPE,STONEUNIT FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Stnitemid, cn)
                    tempwtunit = dtstnitemdr(2).ToString
                    tempcalmode = dtstnitemdr(1).ToString
                    If StnSitemid <> 0 Then
                        dtstnsitemdr = GetSqlRow("SELECT SUBITEMNAME,CALTYPE,STONEUNIT FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = " & StnSitemid, cn)
                        tempwtunit = dtstnitemdr(2).ToString
                        tempcalmode = dtstnitemdr(1).ToString
                    End If

                    Dim stnwt As Decimal = (StudDeductPer / 100) * Val(WtxtGrossWt_Wet.Text)
                    Dim drstn As DataRow
                    dtWStoneDetails.Rows.Clear()
                    drstn = dtWStoneDetails.NewRow
                    drstn("ITEM") = dtstnitemdr(0)
                    drstn("METALID") = "S"
                    drstn("SUBITEM") = StnSitemid
                    drstn("PCS") = 0
                    drstn("WEIGHT") = stnwt
                    drstn("UNIT") = tempwtunit
                    drstn("CALC") = tempcalmode
                    drstn("RATE") = StudRate 'dtdesgstnrow(1)
                    drstn("AMOUNT") = stnwt * StudRate ' Val(dtdesgstnrow(1).ToString)
                    drstn("STNSNO") = 1
                    dtWStoneDetails.Rows.Add(drstn)
                    WStyleGridStone()
                    WCalcLessWt()
                    WCalcFinalTotal()
                    'WtxtNetWt_Wet.Focus()
                    GoTo StudContinue
                End If
            End If
            Dim dtdesgstnrow As DataRow
            'Dim tempDesignerid As Integer = objGPack.GetSqlValue("SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE ISNULL(DESIGNERNAME,'') = '" & cmbDesigner_MAN.Text & "'")
            'dtdesgstnrow = GetSqlRow("SELECT STNDEDPER,STN_RATE,STNITEMID,STNSUBITEMID FROM " & cnAdminDb & "..DESIGNERSTONE WHERE ACTIVE <> 'N' AND DESIGNERID = " & tempDesignerid & " AND ITEMID = " & tempitemid & " AND SUBITEMID = " & tempSitemid, cn)
            If Not dtdesgstnrow Is Nothing Then
                Dim Stnitemid As Integer = Val(dtdesgstnrow(2).ToString)
                If Stnitemid = 0 Then Stnitemid = 9999

                Dim StnSitemid As Integer = Val(dtdesgstnrow(3).ToString)
                Dim dtstnitemdr As DataRow
                Dim dtstnsitemdr As DataRow
                Dim tempwtunit As String
                Dim tempcalmode As String
                dtstnitemdr = GetSqlRow("SELECT ITEMNAME,CALTYPE,STONEUNIT FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Stnitemid, cn)
                tempwtunit = dtstnitemdr(2).ToString
                tempcalmode = dtstnitemdr(1).ToString
                If StnSitemid <> 0 Then
                    dtstnsitemdr = GetSqlRow("SELECT SUBITEMNAME,CALTYPE,STONEUNIT FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = " & StnSitemid, cn)
                    tempwtunit = dtstnitemdr(2).ToString
                    tempcalmode = dtstnitemdr(1).ToString
                End If

                Dim stnwt As Decimal = (Val(dtdesgstnrow(0).ToString) / 100) * Val(WtxtGrossWt_Wet.Text)
                Dim drstn As DataRow
                dtWStoneDetails.Rows.Clear()
                drstn = dtWStoneDetails.NewRow
                drstn("ITEM") = dtstnitemdr(0)
                drstn("METALID") = "S"
                drstn("SUBITEM") = StnSitemid
                drstn("PCS") = 0
                drstn("WEIGHT") = stnwt
                drstn("UNIT") = tempwtunit
                drstn("CALC") = tempcalmode
                drstn("RATE") = dtdesgstnrow(1)
                drstn("AMOUNT") = Math.Round(stnwt * Val(dtdesgstnrow(1).ToString), 0)
                drstn("STNSNO") = 1
                dtWStoneDetails.Rows.Add(drstn)
                WCalcLessWt()
                WCalcFinalTotal()
                WtxtNetWt_Wet.Focus()
                Exit Sub
            End If
StudContinue:
            If _FourCMaintain Then
                strSql = "SELECT STUDDED FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & tempitemid & "  AND METALID='D'"
                If objGPack.GetSqlValue(strSql).ToString.ToUpper = "L" Then
                    If Not tagEdit Then
                        ObjDiaDetails = New frmDiamondDetails
                    End If
                    ObjDiaDetails.CmbCut.Focus()
                    ObjDiaDetails.Location = New Drawing.Point(481, 200)
                    ObjDiaDetails.Size = New Drawing.Size(261, 175)
                    Dim view4c As String = ""
                    If wcmbItem_MAN.Text <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & wcmbItem_MAN.Text & "'", , , )
                    If Not view4c.Contains("CU") Then ObjDiaDetails.CmbCut.Enabled = False
                    If Not view4c.Contains("CO") Then ObjDiaDetails.CmbColor.Enabled = False
                    If Not view4c.Contains("CL") Then ObjDiaDetails.CmbClarity.Enabled = False
                    If Not view4c.Contains("SH") Then ObjDiaDetails.cmbShape.Enabled = False
                    If Not view4c.Contains("SE") Then ObjDiaDetails.cmbSetType.Enabled = False
                    If Not view4c.Contains("HE") Then ObjDiaDetails.txtHeight_WET.Enabled = False
                    If Not view4c.Contains("WI") Then ObjDiaDetails.txtWidth_WET.Enabled = False
                    ObjDiaDetails.ShowDialog()
                    TagCutId = Val(objGPack.GetSqlValue(" SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & ObjDiaDetails.CmbCut.Text & "'", "CUTID", , tran))
                    TagColorId = Val(objGPack.GetSqlValue(" SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME = '" & ObjDiaDetails.CmbColor.Text & "'", "COLORID", , tran))
                    TagClarityId = Val(objGPack.GetSqlValue(" SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & ObjDiaDetails.CmbClarity.Text & "'", "CLARITYID", , tran))
                    TagShapeId = Val(objGPack.GetSqlValue(" SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & ObjDiaDetails.cmbShape.Text & "'", "SHAPEID", , tran))
                    TagSetTypeId = Val(objGPack.GetSqlValue(" SELECT SETTYPEID FROM " & cnAdminDb & "..STNSETTYPE WHERE SETTYPENAME = '" & ObjDiaDetails.cmbSetType.Text & "'", "SETTYPEID", , tran))
                    TagHeight = Val(ObjDiaDetails.txtHeight_WET.Text.ToString)
                    TagWidth = Val(ObjDiaDetails.txtWidth_WET.Text.ToString)
                End If
            End If
            If objGPack.GetSqlValue("SELECT EXTRAWT FROM " & cnAdminDb & "..ITEMMAST WHERE itemid = " & tempitemid & "", "EXTRAWT", "N") = "Y" Then
                'If Me.Controls.Contains(grpExtraWt) = False Then
                '    Me.Controls.Add(grpExtraWt)
                'End If
                'grpExtraWt.Location = New Point(480, 175)
                'grpExtraWt.Visible = True
                'txtExtraWt_WET.Focus()
                'grpExtraWt.BringToFront()
                'ObjExtraWt.Visible = True
                'ObjExtraWt.StartPosition = FormStartPosition.CenterScreen
                'ObjExtraWt.BringToFront()
                ObjExtraWt.txtExtraWt_WET.Focus()
                ObjExtraWt.ShowDialog()
            Else
                'ObjExtraWt.Visible = False
            End If
            strSql = " SELECT mcasvaper FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & tempitemid & ""
            If objGPack.GetSqlValue(strSql).ToString = "Y" Then wLabel44.Text = "MC PERCENT" Else wLabel44.Text = "Max Mc Per Gram"

            With WTabControl1
                If .TabPages.Contains(WtabMultiMetal) Then
                    .SelectedTab = WtabMultiMetal
                    Me.SelectNextControl(WtabMultiMetal, True, True, True, True)
                ElseIf .TabPages.Contains(WtabStone) And wpnlStDet.Enabled = True Then
                    .SelectedTab = WtabStone
                    If PacketNoEnable <> "N" Then
                        wtxtstPackettno.Focus()
                    Else
                        Me.SelectNextControl(WtabStone, True, True, True, True)
                    End If

                ElseIf .TabPages.Contains(WtabOtherCharges) Then
                    .SelectedTab = WtabOtherCharges
                    Me.SelectNextControl(WtabOtherCharges, True, True, True, True)
                Else
                    SendKeys.Send("{TAB}")
                End If
            End With
        ElseIf e.KeyChar = Chr(Keys.Space) Then
            WGetGrsWeightFromPort()
            e.Handled = True
        End If
    End Sub

    Function wCheckBookstk(ByVal Mcatcode As String) As Decimal
        Dim Mcostid As String = ""
        Dim Mgrswt As Decimal = 0
        'If cmbCostCentre_Man.Text <> "" Then Mcostid = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre_Man.Text & "'")
        strSql = " SELECT SUM(ISNULL(GRSWT,0)) GRSWT FROM ("
        strSql += vbCrLf + " SELECT SUM(GRSWT) AS GRSWT FROM " & cnStockDb & "..OPENWEIGHT WHERE COMPANYID='" & GetStockCompId() & "' AND CATCODE='" & Mcatcode & "' AND STOCKTYPE='C' AND TRANTYPE='R' AND ISNULL(COSTID,'')='" & Mcostid & "' "
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT SUM(GRSWT)*(-1) AS GRSWT FROM " & cnStockDb & "..OPENWEIGHT WHERE COMPANYID='" & GetStockCompId() & "' AND CATCODE='" & Mcatcode & "' AND STOCKTYPE='C' AND TRANTYPE='I' AND ISNULL(COSTID,'')='" & Mcostid & "' "
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT SUM(GRSWT)*(-1) AS GRSWT FROM " & cnStockDb & "..ISSUE WHERE COMPANYID='" & GetStockCompId() & "' AND CATCODE='" & Mcatcode & "' AND ISNULL(COSTID,'')='" & Mcostid & "'  AND ISNULL(CANCEL,'')=''"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT SUM(GRSWT)GRSWT FROM " & cnStockDb & "..RECEIPT WHERE COMPANYID='" & GetStockCompId() & "' AND CATCODE='" & Mcatcode & "' AND ISNULL(COSTID,'')='" & Mcostid & "' AND ISNULL(CANCEL,'')='' "
        strSql += vbCrLf + " )X "
        Mgrswt = Val(objGPack.GetSqlValue(strSql))
        Return Mgrswt
    End Function


    Private Sub WbtnAttachImage_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles wbtnGenerateTag.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.SelectNextControl(wbtnGenerateTag, True, True, True, True)
        End If
    End Sub

#Region "TextChangedEvents"
    Private Sub WtxtPieces_Num_Man_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles wtxtPieces_Num_Man.TextChanged
        WCalcFinalTotal()
    End Sub

    Private Sub WtxtNetWt_Wet_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles WtxtNetWt_Wet.TextChanged
        WCalcMaxMinValues()
        WCalcFinalTotal()
    End Sub

    Private Sub wtxtMetalRate_Amt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles wtxtMetalRate_Amt.KeyPress
        If e.KeyChar = Chr(Keys.Enter) And Val(wtxtMMRate.Text) = 0 Then
            strSql = " SELECT ISNULL(STUDDED,'') STUDDED FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') ='" & wcmbItem_MAN.Text & "' AND METALID = 'D'"
            Studded_Loose = objGPack.GetSqlValue(strSql, "STUDDED", "")
            If NEEDUS = True And Studded_Loose = "L" And Val(wtxtMMRate.Text) = 0 Then
                If calType = "M" Then
                    If ObjRsUs.ShowDialog = Windows.Forms.DialogResult.OK Then
                        wtxtMMRate.Text = Convert.ToDouble(Val(ObjRsUs.txtUSDollar_Amt.Text) * Val(ObjRsUs.txtIndRs_Amt.Text)).ToString("0.00")
                        'Me.SelectNextControl(txtRate_Amt, True, True, True, True)
                    End If
                End If
            End If


            strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wcmbItem_MAN.Text & "'"

            Dim calcType As String = objGPack.GetSqlValue(strSql)
            ''Weight Rate Validation
            Select Case calcType.ToUpper
                Case "R"
                    If Val(wtxtMMRate.Text) = 0 Then
                        MsgBox("Rate should not empty", MsgBoxStyle.Information)
                        wtxtMMRate.Focus()
                        Exit Sub
                    End If
                Case "B"
                    If Val(WtxtGrossWt_Wet.Text) = 0 Then
                        MsgBox("Grs Weight should not empty", MsgBoxStyle.Information)
                        WtxtGrossWt_Wet.Focus()
                        Exit Sub
                    End If

                Case "F"
                    If Val(WtxtGrossWt_Wet.Text) = 0 And Val(wtxtMMRate.Text) = 0 Then
                        MsgBox("Weight and Rate should not empty", MsgBoxStyle.Information)
                        WtxtGrossWt_Wet.Focus()
                        Exit Sub
                    End If
            End Select
        End If
    End Sub

    Private Sub WtxtMaxWastage_Wet_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles wtxtMMWastage_WET.TextChanged
        Me.WtxtMaxMcPerGrm_TextChanged(Me, New EventArgs)
        WCalcFinalTotal()
    End Sub

    Private Sub WtxtMaxMkCharge_Amt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles wtxtMkCharge_Amt.KeyPress
        If Val(wtxtMcPerGrm_Amt.Text) > 0 Then
            e.Handled = True
        End If
        If e.KeyChar = Chr(Keys.Enter) Then
            Me.SelectNextControl(wtxtMkCharge_Amt, True, True, True, True)
        End If
    End Sub

    Private Sub WtxtMaxMkCharge_Amt_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles wtxtMkCharge_Amt.TextChanged
        WCalcFinalTotal()
    End Sub

#End Region

#Region "MultiMetal Procedures"
    Private Sub WgridMultimetal_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles wgridMultimetal.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If

    End Sub

    Private Sub WgridMultimetal_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles wgridMultimetal.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            wgridMultimetal.CurrentCell = wgridMultimetal.Rows(wgridMultimetal.CurrentRow.Index).Cells(0)
            With wgridMultimetal.Rows(wgridMultimetal.CurrentRow.Index)
                wtxtMMCategory.Text = .Cells("CATEGORY").FormattedValue
                wtxtMMWeight_Wet.Text = .Cells("WEIGHT").FormattedValue
                wtxtMMWastagePer_PER.Text = .Cells("WASTAGEPER").FormattedValue
                wtxtMMWastage_WET.Text = .Cells("WASTAGE").FormattedValue
                wtxtMMMcPerGRm_AMT.Text = .Cells("MCPERGRM").FormattedValue
                wtxtMMMc_AMT.Text = .Cells("MC").FormattedValue
                wtxtMMAmount_AMT.Text = .Cells("AMOUNT").FormattedValue
                wtxtMMRowIndex.Text = wgridMultimetal.CurrentRow.Index
                wtxtMMCategory.Focus()
                wtxtMMCategory.SelectAll()
                '.Item("CATEGORY") = ""
                '.Item("WEIGHT") = IIf(Val(txtMMWeight_Wet.Text) <> 0, Val(txtMMWeight_Wet.Text), DBNull.Value)
                '.Item("RATE") = IIf(Val(txtMMRate_Amt.Text) <> 0, Val(txtMMRate_Amt.Text), DBNull.Value)
                '.Item("AMOUNT")
            End With
        End If
    End Sub

    Private Sub WgridMultimetal_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles wgridMultimetal.UserDeletedRow
        dtWMultiMetalDetails.AcceptChanges()
        WCalcFinalTotal()
        If Not wgridMultimetal.RowCount > 0 Then
            wtxtMMCategory.Focus()
        End If
    End Sub

#End Region

#Region "Stone Procedures"
    Private Sub WCalcStoneTotals()
        ''Calc Total
        Dim totPcs As Integer = Nothing
        Dim totWt As Double = Nothing
        Dim totAmt As Double = Nothing
        If WTabControl1.TabPages.Contains(WtabStone) Then
            For cnt As Integer = 0 To wgridStone.RowCount - 1
                With wgridStone.Rows(cnt)
                    totPcs += Val(.Cells("PCS").Value.ToString)
                    If .Cells("UNIT").Value.ToString = "C" Then
                        totWt += Val(.Cells("WEIGHT").Value.ToString) / 5
                    ElseIf .Cells("UNIT").Value.ToString = "G" Then
                        totWt += Val(.Cells("WEIGHT").Value.ToString)
                    End If
                    totAmt += Val(.Cells("AMOUNT").Value.ToString)
                End With
            Next
        End If
        With wgridStoneFooter.Rows(0)
            .Cells("PCS").Value = IIf(totPcs <> 0, totPcs, DBNull.Value)
            .Cells("WEIGHT").Value = IIf(totWt <> 0, Format(totWt, FormatNumberStyle(DiaRnd)), DBNull.Value)
            .Cells("AMOUNT").Value = IIf(totAmt <> 0, Format(totAmt, "0.000"), DBNull.Value)
        End With
    End Sub

    Private Sub WCalcStoneAmount()
        Dim amt As Double = Nothing
        If wcmbStCalc.Text = "P" Then
            amt = Val(wtxtStRate_Amt.Text) * Val(wtxtStPcs_Num.Text)
        Else
            amt = Val(wtxtStRate_Amt.Text) * Val(wtxtStWeight.Text)
        End If
        amt = Math.Round(amt)
        wtxtStAmount_Amt.Text = IIf(amt <> 0, Format(amt, "0.00"), "")
    End Sub

    Private Sub WtxtStRate_Amt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles wtxtStRate_Amt.GotFocus
        'If NEEDUS = True And Studded_Loose <> "L" Then
        '    SendKeys.Send("{TAB}")
        'End If
    End Sub

    Private Sub WtxtStRate_Amt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles wtxtStRate_Amt.KeyDown
        If e.KeyCode = Keys.Down Then
            wgridStone.Select()
        End If
    End Sub

    Private Sub WtxtStRate_Amt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles wtxtStRate_Amt.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            strSql = " SELECT ISNULL(STUDDED,'') STUDDED FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') ='" & wtxtStItem.Text & "' AND METALID = 'D'"
            Studded_Loose = objGPack.GetSqlValue(strSql, "STUDDED", "")
            If NEEDUS = True And Studded_Loose <> "" And Val(wtxtStRate_Amt.Text) = 0 Then
                'If calType = "M" Then
                If ObjRsUs.ShowDialog = Windows.Forms.DialogResult.OK Then
                    wtxtStRate_Amt.Text = Convert.ToDouble(Val(ObjRsUs.txtUSDollar_Amt.Text) * Val(ObjRsUs.txtIndRs_Amt.Text)).ToString("0.00")
                End If
                'End If
            End If

            Dim cent As Double = 0
            strSql = " SELECT ISNULL(CALTYPE,'') CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') ='" & wtxtStItem.Text & "'"
            Dim mCaltype As String = objGPack.GetSqlValue(strSql, "CALTYPE", "")
            strSql = " SELECT ISNULL(CALTYPE,'') CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE ISNULL(SUBITEMNAME,'') ='" & wtxtStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wtxtStItem.Text & "')"
            mCaltype = objGPack.GetSqlValue(strSql, "CALTYPE", "")
            If mCaltype = "D" Then
                cent = Val(wtxtStWeight.Text)
            Else
                cent = (Val(wtxtStWeight.Text) / IIf(Val(wtxtStPcs_Num.Text) = 0, 1, Val(wtxtStPcs_Num.Text)))
            End If

            cent *= 100
            strSql = " DECLARE @CENT FLOAT"
            strSql += " SET @CENT = " & cent & ""
            strSql += " SELECT MINRATE FROM " & cnAdminDb & "..CENTRATE WHERE"
            strSql += " @CENT BETWEEN FROMCENT AND TOCENT "
            strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wtxtStItem.Text & "')"
            strSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & wtxtStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wtxtStItem.Text & "')),0)"

            'If cmbCostCentre_Man.Text.Trim <> "" Then strSql += " AND COSTID IN(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre_Man.Text & "')"
            Dim rate As Double = Val(objGPack.GetSqlValue(strSql, , , tran))
            If Val(wtxtStRate_Amt.Text) < rate Then
                MsgBox(Me.GetNextControl(wtxtStRate_Amt, False).Text + E0020 + rate.ToString)
                wtxtStRate_Amt.Focus()
            End If
        End If
    End Sub

    Private Sub WtxtStRate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles wtxtStRate_Amt.TextChanged
        WCalcStoneAmount()
    End Sub

    Private Sub WtxtStAmount_Amt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles wtxtStAmount_Amt.KeyDown
        If e.KeyCode = Keys.Down Then
            wgridStone.Select()
        End If
    End Sub

    Private Sub WtxtStAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles wtxtStAmount_Amt.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            ''Validation
            If objGPack.Validator_Check(wgrpStoneDetails) Then Exit Sub

            If objGPack.DupCheck("SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wtxtStItem.Text & "' AND  STUDDED = 'S' AND ACTIVE = 'Y'") = False Then
                MsgBox("Invalid Item", MsgBoxStyle.Information)
                wtxtStItem.Focus()
                Exit Sub
            End If
            If UCase(objGPack.GetSqlValue("SELECT SUBITEM FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wtxtStItem.Text & "' AND STUDDED = 'S' AND ACTIVE = 'Y'")) = "Y" Then
                If wtxtStSubItem.Text = "" Then
                    MsgBox("SubItem Should Not Empty", MsgBoxStyle.Information)
                    wtxtStSubItem.Focus()
                    Exit Sub
                End If
                If objGPack.DupCheck("SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wtxtStItem.Text & "') AND SUBITEMNAME = '" & wtxtStSubItem.Text & "'") = False Then
                    MsgBox("Invalid SubItem", MsgBoxStyle.Information)
                    wtxtStSubItem.Focus()
                    Exit Sub
                End If
            Else
                wtxtStSubItem.Clear()
            End If
            If Val(wtxtStPcs_Num.Text) = 0 And Val(wtxtStWeight.Text) = 0 And Val(wtxtStAmount_Amt.Text) = 0 Then
                MsgBox(Me.GetNextControl(wtxtStPcs_Num, False).Text + "," + Me.GetNextControl(wtxtStWeight, False).Text + E0001, MsgBoxStyle.Information)
                wtxtStItem.Focus()
                Exit Sub
            End If
            Dim cent As Double = Nothing
            strSql = " SELECT ISNULL(CALTYPE,'') CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') ='" & wtxtStItem.Text & "'"
            Dim mCaltype As String = objGPack.GetSqlValue(strSql, "CALTYPE", "")
            strSql = " SELECT ISNULL(CALTYPE,'') CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE ISNULL(SUBITEMNAME,'') ='" & wtxtStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wtxtStItem.Text & "')"
            mCaltype = objGPack.GetSqlValue(strSql, "CALTYPE", "")
            If mCaltype = "D" Then
                cent = Val(wtxtStWeight.Text)
            Else
                cent = (Val(wtxtStWeight.Text) / IIf(Val(wtxtStPcs_Num.Text) = 0, 1, Val(wtxtStPcs_Num.Text)))
            End If

            'If wcmbStUnit.Text = "C" Then cent = (Val(wtxtStWeight.Text) / IIf(Val(wtxtStPcs_Num.Text) = 0, 1, Val(wtxtStPcs_Num.Text))) Else cent = Val(wtxtStWeight.Text) / IIf(Val(wtxtStPcs_Num.Text) = 0, 1, Val(wtxtStPcs_Num.Text))
            cent *= 100
            strSql = " DECLARE @CENT FLOAT"
            strSql += " SET @CENT = " & cent & ""
            strSql += " SELECT MINRATE FROM " & cnAdminDb & "..CENTRATE WHERE"
            strSql += " @CENT BETWEEN FROMCENT AND TOCENT "
            strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wtxtStItem.Text & "')"
            strSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & wtxtStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wtxtStItem.Text & "')),0)"

            'If cmbCostCentre_Man.Text.Trim <> "" Then strSql += " AND COSTID IN(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre_Man.Text & "')"
            Dim rate As Double = Val(objGPack.GetSqlValue(strSql, , , tran))
            If Val(wtxtStRate_Amt.Text) < rate Then
                MsgBox(Me.GetNextControl(wtxtStRate_Amt, False).Text + E0020 + rate.ToString)
                wtxtStRate_Amt.Focus()
                Exit Sub
            End If
            Dim stWeight As Double = IIf(wcmbStUnit.Text = "C", Val(wtxtStWeight.Text) / 5, Val(wtxtStWeight.Text))
            For cnt As Integer = 0 To wgridStone.RowCount - 1
                If wtxtStRowIndex.Text <> "" Then If Val(wtxtStRowIndex.Text) = cnt Then Continue For
                With wgridStone.Rows(cnt)
                    If .Cells("UNIT").Value.ToString = "C" Then
                        stWeight += Val(.Cells("WEIGHT").Value.ToString) / 5
                    ElseIf .Cells("UNIT").Value.ToString = "G" Then
                        stWeight += Val(.Cells("WEIGHT").Value.ToString)
                    End If
                End With
            Next
            If stWeight > Val(WtxtGrossWt_Wet.Text) Then
                MsgBox(Me.GetNextControl(wtxtStWeight, False).Text + E0015 + Me.GetNextControl(WtxtGrossWt_Wet, False).Text, MsgBoxStyle.Exclamation)
                wtxtStWeight.Focus()
                Exit Sub
            End If
            If wtxtStRowIndex.Text <> "" Then
                'If MessageBox.Show("Would you like to update this Entry", "Update Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.OK Then
                With wgridStone.Rows(Val(wtxtStRowIndex.Text))
                    .Cells("PACKETNO").Value = wtxtstPackettno.Text
                    .Cells("ITEM").Value = wtxtStItem.Text
                    .Cells("SUBITEM").Value = wtxtStSubItem.Text
                    .Cells("UNIT").Value = wcmbStUnit.Text
                    .Cells("CALC").Value = wcmbStCalc.Text
                    .Cells("PCS").Value = IIf(Val(wtxtStPcs_Num.Text) <> 0, Val(wtxtStPcs_Num.Text), DBNull.Value)
                    .Cells("WEIGHT").Value = IIf(Val(wtxtStWeight.Text) <> 0, Format(Val(wtxtStWeight.Text), FormatNumberStyle(DiaRnd)), DBNull.Value)
                    .Cells("RATE").Value = IIf(Val(wtxtStRate_Amt.Text) <> 0, Format(Val(wtxtStRate_Amt.Text), "0.00"), DBNull.Value)
                    .Cells("AMOUNT").Value = IIf(Val(wtxtStAmount_Amt.Text) <> 0, Format(Val(wtxtStAmount_Amt.Text), "0.00"), DBNull.Value)
                    .Cells("METALID").Value = wtxtStMetalCode.Text
                    .Cells("USRATE").Value = IIf(NEEDUS = True And Studded_Loose <> "L", Val(ObjRsUs.txtUSDollar_Amt.Text), 0)
                    .Cells("INDRS").Value = IIf(NEEDUS = True And Studded_Loose <> "L", Val(ObjRsUs.txtIndRs_Amt.Text), 0)
                    If _FourCMaintain Then
                        .Cells("CUT").Value = ObjDiaDetails.CmbCut.Text
                        .Cells("COLOR").Value = ObjDiaDetails.CmbColor.Text
                        .Cells("CLARITY").Value = ObjDiaDetails.CmbClarity.Text
                        .Cells("SHAPE").Value = ObjDiaDetails.cmbShape.Text
                        .Cells("SETTYPE").Value = ObjDiaDetails.cmbSetType.Text
                        .Cells("HEIGHT").Value = Val(ObjDiaDetails.txtHeight_WET.Text)
                        .Cells("WIDTH").Value = Val(ObjDiaDetails.txtWidth_WET.Text)
                    End If
                    dtWStoneDetails.AcceptChanges()
                    GoTo AFTERINSERT
                End With
                'End If
            End If
            ''Insertion
            Dim ro As DataRow = Nothing
            ro = dtWStoneDetails.NewRow
            ro("PACKETNO") = wtxtstPackettno.Text
            ro("ITEM") = wtxtStItem.Text
            ro("SUBITEM") = wtxtStSubItem.Text
            ro("PCS") = IIf(Val(wtxtStPcs_Num.Text) <> 0, Val(wtxtStPcs_Num.Text), DBNull.Value)
            ro("UNIT") = wcmbStUnit.Text
            ro("CALC") = wcmbStCalc.Text
            ro("WEIGHT") = IIf(Val(wtxtStWeight.Text) <> 0, Format(Val(wtxtStWeight.Text), FormatNumberStyle(DiaRnd)), DBNull.Value)
            ro("RATE") = IIf(Val(wtxtStRate_Amt.Text) <> 0, Format(Val(wtxtStRate_Amt.Text), "0.00"), DBNull.Value)
            ro("AMOUNT") = IIf(Val(wtxtStAmount_Amt.Text) <> 0, Format(Val(wtxtStAmount_Amt.Text), "0.00"), DBNull.Value)
            ro("METALID") = wtxtStMetalCode.Text
            ro("USRATE") = IIf(NEEDUS = True And Studded_Loose <> "L", Val(ObjRsUs.txtUSDollar_Amt.Text), 0)
            ro("INDRS") = IIf(NEEDUS = True And Studded_Loose <> "L", Val(ObjRsUs.txtIndRs_Amt.Text), 0)
            If _FourCMaintain Then
                ro("CUT") = ObjDiaDetails.CmbCut.Text
                ro("COLOR") = ObjDiaDetails.CmbColor.Text
                ro("CLARITY") = ObjDiaDetails.CmbClarity.Text
                ro("SHAPE") = ObjDiaDetails.cmbShape.Text
                ro("SETTYPE") = ObjDiaDetails.cmbSetType.Text
                ro("HEIGHT") = Val(ObjDiaDetails.txtHeight_WET.Text)
                ro("WIDTH") = Val(ObjDiaDetails.txtWidth_WET.Text)
            End If
            dtWStoneDetails.Rows.Add(ro)
            dtWStoneDetails.AcceptChanges()
            wgridStone.CurrentCell = wgridStone.Rows(wgridStone.RowCount - 1).Cells(1)
AFTERINSERT:
            WCalcLessWt()
            WfuncSplitDiaStn()
            WCalcSummary()
            WCalcFinalTotal()

            ''CLEAR
            'cmbStItem_Man.Text = ""
            'cmbStSubItem_Man.Text = ""
            wtxtStSubItem.Clear()
            wtxtStPcs_Num.Clear()
            wtxtStWeight.Clear()
            wtxtStRate_Amt.Clear()
            wtxtStAmount_Amt.Clear()
            wtxtStMetalCode.Clear()
            wtxtstPackettno.Clear()
            wtxtStRowIndex.Clear()
            ObjRsUs.txtIndRs_Amt.Clear()
            ObjRsUs.txtUSDollar_Amt.Clear()
            ObjDiaDetails = New frmDiamondDetails
            If PacketNoEnable <> "N" Then
                wtxtstPackettno.Focus()
            Else
                wtxtStItem.Focus()
            End If

        End If
    End Sub

    Private Sub WtxtStWeight_Wet_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles wtxtStWeight.KeyDown
        If e.KeyCode = Keys.Down Then
            wgridStone.Select()
        End If
    End Sub

    Private Sub WtxtStWeight_Wet_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles wtxtStWeight.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim stWeight As Double = IIf(wcmbStUnit.Text = "C", Val(wtxtStWeight.Text) / 5, Val(wtxtStWeight.Text))
            For cnt As Integer = 0 To wgridStone.RowCount - 1
                If wtxtStRowIndex.Text <> "" Then If Val(wtxtStRowIndex.Text) = cnt Then Continue For
                With wgridStone.Rows(cnt)
                    If .Cells("UNIT").Value.ToString = "C" Then
                        stWeight += Val(.Cells("WEIGHT").Value.ToString) / 5
                    ElseIf .Cells("UNIT").Value.ToString = "G" Then
                        stWeight += Val(.Cells("WEIGHT").Value.ToString)
                    End If
                End With
            Next
            If stWeight > Val(WtxtGrossWt_Wet.Text) Then
                MsgBox(Me.GetNextControl(wtxtStWeight, False).Text + E0015 + Me.GetNextControl(WtxtGrossWt_Wet, False).Text, MsgBoxStyle.Exclamation)
                wtxtStWeight.Focus()
                Exit Sub
            End If
        Else
            WeightValidation(wtxtStWeight, e, DiaRnd)
        End If
    End Sub

    Private Sub WtxtStWeight_Wet_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles wtxtStWeight.LostFocus
        wcmbStCalc.Text = IIf(Val(wtxtStWeight.Text) > 0, "W", "P")
        wtxtStWeight.Text = IIf(Val(wtxtStWeight.Text) <> 0, Format(Val(wtxtStWeight.Text), FormatNumberStyle(DiaRnd)), wtxtStWeight.Text)
    End Sub

    Private Sub WtxtStWeight_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles wtxtStWeight.TextChanged
        Dim cent As Double = 0
        strSql = " SELECT ISNULL(CALTYPE,'') CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') ='" & wtxtStItem.Text & "'"
        Dim mCaltype As String = objGPack.GetSqlValue(strSql, "CALTYPE", "")
        strSql = " SELECT ISNULL(CALTYPE,'') CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE ISNULL(SUBITEMNAME,'') ='" & wtxtStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wtxtStItem.Text & "')"
        mCaltype = objGPack.GetSqlValue(strSql, "CALTYPE", "")
        If mCaltype = "D" Then
            cent = Val(wtxtStWeight.Text)
        Else
            cent = (Val(wtxtStWeight.Text) / IIf(Val(wtxtStPcs_Num.Text) = 0, 1, Val(wtxtStPcs_Num.Text)))
        End If
        'If wcmbStUnit.Text = "C" Then cent = (Val(wtxtStWeight.Text) / IIf(Val(wtxtStPcs_Num.Text) = 0, 1, Val(wtxtStPcs_Num.Text))) Else cent = Val(wtxtStWeight.Text) / IIf(Val(wtxtStPcs_Num.Text) = 0, 1, Val(wtxtStPcs_Num.Text))
        cent *= 100
        strSql = " DECLARE @CENT FLOAT"
        strSql += vbCrLf + " SET @CENT = " & cent & ""
        strSql += vbCrLf + " SELECT MAXRATE FROM " & cnAdminDb & "..CENTRATE WHERE"
        strSql += vbCrLf + " @CENT BETWEEN FROMCENT AND TOCENT "
        strSql += vbCrLf + " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wtxtStItem.Text & "')"
        strSql += vbCrLf + " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & wtxtStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wtxtStItem.Text & "')),0)"
        'If CENTRATE_DESIGNER Then strSql += " AND ISNULL(DESIGNERID,0) =ISNULL((SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME='" & cmbDesigner_MAN.Text & "'),0)"
        'strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ISNULL((SELECT ACCODE FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesigner_MAN.Text & "'),'')"
        'If cmbCostCentre_Man.Text.Trim <> "" Then strSql += " AND COSTID IN(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre_Man.Text & "')"
        If _FourCMaintain Then

            Dim ColorId As Integer = 0
            Dim CutId As Integer = 0
            Dim ClarityId As Integer = 0
            Dim Shapeid As Integer = 0
            ColorId = objGPack.GetSqlValue("SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME = '" & ObjDiaDetails.CmbColor.Text & "'", "COLORID", 0)
            'CutId = objGPack.GetSqlValue("SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & ObjDiaDetails.CmbCut.Text & "'", "CUTID", 0)
            ClarityId = objGPack.GetSqlValue("SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & ObjDiaDetails.CmbClarity.Text & "'", "CLARITYID", 0)
            Shapeid = objGPack.GetSqlValue("SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & ObjDiaDetails.cmbShape.Text & "'", "SHAPEID", 0)
            strSql += vbCrLf + " AND COLORID=" & ColorId
            strSql += vbCrLf + " AND SHAPEID=" & Shapeid
            'strSql += vbCrLf + " AND CUTID=" & CutId
            strSql += vbCrLf + " AND CLARITYID=" & ClarityId
        End If
        Dim rate As Double = Val(objGPack.GetSqlValue(strSql, "MAXRATE", "", tran))
        If rate <> 0 Then
            wtxtStRate_Amt.Text = IIf(rate <> 0, Format(rate, "0.00"), "")
        Else
            Dim XpurRate As Double = Val(objGPack.GetSqlValue(Replace(strSql, "MAXRATE", "PURRATE"), "PURRATE", "", tran).ToString)
            Dim SaleRateper As Double = Val(objGPack.GetSqlValue(Replace(strSql, "MAXRATE", "SALESPER"), "SALESPER", "", tran).ToString)
            If SaleRateper <> 0 And XpurRate <> 0 Then rate = XpurRate + (XpurRate * (SaleRateper / 100))
            wtxtStRate_Amt.Text = IIf(rate <> 0, Format(rate, "0.00"), "")
            'WtxtStRate_Amt.Text = "0.00"
        End If
        WCalcStoneAmount()
    End Sub


    Private Sub WcmbStCalc_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles wcmbStCalc.SelectedIndexChanged
        WCalcStoneAmount()
    End Sub

    Private Sub WtxtStPcs_Num_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles wtxtStPcs_Num.KeyDown
        If e.KeyCode = Keys.Down Then
            wgridStone.Select()
        End If
    End Sub

    Private Sub WtxtStPcs_Num_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles wtxtStPcs_Num.Leave
        Dim view4c As String = ""
        Dim maintain4c As Boolean
        strSql = "SELECT ISNULL(MAINTAIN4C,'N') FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wtxtStItem.Text & "' AND METALID='D'"
        If objGPack.GetSqlValue(strSql).ToString.ToUpper = "N" Then maintain4c = False Else maintain4c = True
        strSql = "SELECT ISNULL(MAINTAIN4C,'N') FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & wtxtStSubItem.Text & "' AND ITEMID=(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & wtxtStItem.Text & "')"
        If objGPack.GetSqlValue(strSql).ToString.ToUpper = "N" Then maintain4c = False Else maintain4c = True
        If _FourCMaintain And Not tagEdit And maintain4c Then
            ObjDiaDetails = New frmDiamondDetails
            If wtxtStItem.Text <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & wtxtStItem.Text & "'", , , )
            If wtxtStSubItem.Text <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & wtxtStSubItem.Text & "' AND ITEMID=(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & wtxtStItem.Text & "')", , , )
            If Not view4c.Contains("CU") Then ObjDiaDetails.CmbCut.Enabled = False
            If Not view4c.Contains("CO") Then ObjDiaDetails.CmbColor.Enabled = False
            If Not view4c.Contains("CL") Then ObjDiaDetails.CmbClarity.Enabled = False
            If Not view4c.Contains("SH") Then ObjDiaDetails.cmbShape.Enabled = False
            If Not view4c.Contains("SE") Then ObjDiaDetails.cmbSetType.Enabled = False
            If Not view4c.Contains("HE") Then ObjDiaDetails.txtHeight_WET.Enabled = False
            If Not view4c.Contains("WI") Then ObjDiaDetails.txtWidth_WET.Enabled = False

            ObjDiaDetails.CmbCut.Focus()
            ObjDiaDetails.ShowDialog()
        ElseIf _FourCMaintain And tagEdit And maintain4c Then
            ObjDiaDetails = New frmDiamondDetails
            ObjDiaDetails.CmbCut.Text = Cut
            ObjDiaDetails.CmbColor.Text = Color
            ObjDiaDetails.CmbClarity.Text = Clarity
            ObjDiaDetails.cmbShape.Text = Shape
            ObjDiaDetails.cmbSetType.Text = SetType
            ObjDiaDetails.txtWidth_WET.Text = Width
            ObjDiaDetails.txtHeight_WET.Text = Height
            If wtxtStItem.Text <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & wtxtStItem.Text & "'", , , )
            If wtxtStSubItem.Text <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & wtxtStSubItem.Text & "' AND ITEMID=(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & wtxtStItem.Text & "')", , , )
            If Not view4c.Contains("CU") Then ObjDiaDetails.CmbCut.Enabled = False
            If Not view4c.Contains("CO") Then ObjDiaDetails.CmbColor.Enabled = False
            If Not view4c.Contains("CL") Then ObjDiaDetails.CmbClarity.Enabled = False
            If Not view4c.Contains("SH") Then ObjDiaDetails.cmbShape.Enabled = False
            If Not view4c.Contains("SE") Then ObjDiaDetails.cmbSetType.Enabled = False
            If Not view4c.Contains("HE") Then ObjDiaDetails.txtHeight_WET.Enabled = False
            If Not view4c.Contains("WI") Then ObjDiaDetails.txtWidth_WET.Enabled = False
            ObjDiaDetails.CmbCut.Focus()
            ObjDiaDetails.ShowDialog()
        End If
    End Sub

    Private Sub WtxtStPcs_Num_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles wtxtStPcs_Num.TextChanged
        WCalcStoneAmount()
    End Sub

    Private Sub WgridStone_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles wgridStone.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            wgridStone.CurrentCell = wgridStone.Rows(wgridStone.CurrentRow.Index).Cells(1)
        End If
    End Sub

    Private Sub WgridStone_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles wgridStone.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            With wgridStone.Rows(wgridStone.CurrentRow.Index)
                wtxtstPackettno.Text = .Cells("PACKETNO").FormattedValue
                wtxtStItem.Text = .Cells("ITEM").FormattedValue
                wtxtStSubItem.Text = .Cells("SUBITEM").FormattedValue
                wtxtStPcs_Num.Text = .Cells("PCS").FormattedValue
                wtxtStWeight.Text = .Cells("WEIGHT").FormattedValue
                wcmbStUnit.Text = .Cells("UNIT").FormattedValue
                wcmbStCalc.Text = .Cells("CALC").FormattedValue
                wtxtStRate_Amt.Text = .Cells("RATE").FormattedValue
                wtxtStAmount_Amt.Text = .Cells("AMOUNT").FormattedValue
                ObjRsUs.txtUSDollar_Amt.Text = .Cells("USRATE").FormattedValue
                ObjRsUs.txtIndRs_Amt.Text = .Cells("INDRS").FormattedValue
                If _FourCMaintain Then
                    Cut = IIf(IsDBNull(.Cells("CUT").Value), "", .Cells("CUT").Value)
                    Color = IIf(IsDBNull(.Cells("COLOR").Value), "", .Cells("COLOR").Value)
                    Clarity = IIf(IsDBNull(.Cells("CLARITY").Value), "", .Cells("CLARITY").Value)
                    Shape = IIf(IsDBNull(.Cells("SHAPE").Value), "", .Cells("SHAPE").Value)
                    SetType = IIf(IsDBNull(.Cells("SETTYPE").Value), "", .Cells("SETTYPE").Value)
                    Height = IIf(IsDBNull(.Cells("HEIGHT").Value), 0, .Cells("HEIGHT").Value)
                    Width = IIf(IsDBNull(.Cells("WIDTH").Value), 0, .Cells("WIDTH").Value)
                    ObjDiaDetails.CmbCut.Text = Cut
                    ObjDiaDetails.CmbColor.Text = Color
                    ObjDiaDetails.CmbClarity.Text = Clarity
                    ObjDiaDetails.cmbShape.Text = Shape
                    ObjDiaDetails.cmbSetType.Text = SetType
                    ObjDiaDetails.txtWidth_WET.Text = Width
                    ObjDiaDetails.txtHeight_WET.Text = Height
                End If
                wtxtStRowIndex.Text = wgridStone.CurrentRow.Index
                If PacketNoEnable <> "N" Then
                    wtxtstPackettno.Focus()
                    wtxtstPackettno.SelectAll()
                Else
                    wtxtStItem.Focus()
                    wtxtStItem.SelectAll()
                End If
            End With
        End If
    End Sub

    Private Sub WgridStone_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles wgridStone.UserDeletedRow
        dtWStoneDetails.AcceptChanges()

        Dim RowCheck() As DataRow = Nothing
        With ObjPurDetail
StDel:
            For Each ro As DataRow In .dtGridStone.Rows
                RowCheck = dtWStoneDetails.Select("KEYNO = " & Val(ro.Item("KEYNO").ToString) & "")
                If RowCheck.Length = 0 Then
                    ro.Delete()
                    .dtGridStone.AcceptChanges()
                    GoTo StDel
                End If
            Next
            .dtGridStone.AcceptChanges()
            .CalcPurchaseGrossValue()
            .CalcPurchaseValue()
        End With

        WCalcLessWt()
        WfuncSplitDiaStn()
        WCalcSummary()
        WCalcFinalTotal()
        objGPack.TextClear(wgrpStoneDetails)

        If Not wgridStone.RowCount > 0 Then
            wtxtStItem.Focus()
        End If
    End Sub

#End Region


#Region "Miscellaneous Procedures"
    Private Sub WCalcMiscTotalAmount()
        Dim miscTot As Double = Nothing
        For cnt As Integer = 0 To wgridMisc.Rows.Count - 1
            miscTot += Val(wgridMisc.Rows(cnt).Cells("AMOUNT").Value.ToString)
        Next
        wgridMiscFooter.Rows(0).Cells("AMOUNT").Value = IIf(miscTot <> 0, Format(miscTot, "0.00"), DBNull.Value)
        'txtMiscAmt.Text = IIf(miscTot <> 0, Format(miscTot, "0.00"), "")
    End Sub

    Private Sub WtxtMiscAmount_Amt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles wtxtMiscAmount_Amt.KeyDown
        If e.KeyCode = Keys.Down Then
            wgridMisc.Select()
        End If
    End Sub

    Private Sub WtxtMiscAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles wtxtMiscAmount_Amt.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If wtxtMiscMisc.Text = "" Then
                MsgBox(Me.GetNextControl(wtxtMiscMisc, False).Text + E0001, MsgBoxStyle.Information)
                wtxtMiscMisc.Select()
                Exit Sub
            End If
            If objGPack.DupCheck("SELECT MISCNAME FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCNAME = '" & wtxtMiscMisc.Text & "' AND  ACTIVE = 'Y'") = False Then
                MsgBox("Invalid Misc", MsgBoxStyle.Information)
                wtxtMiscMisc.Focus()
                Exit Sub
            End If
            If Not Val(wtxtMiscAmount_Amt.Text) > 0 Then
                MsgBox(Me.GetNextControl(wtxtMiscAmount_Amt, False).Text + E0001, MsgBoxStyle.Information)
                wtxtMiscAmount_Amt.Select()
                Exit Sub
            End If
            If wtxtMiscRowIndex.Text <> "" Then
                With wgridMisc.Rows(Val(wtxtMiscRowIndex.Text))
                    .Cells("MISC").Value = wtxtMiscMisc.Text
                    .Cells("AMOUNT").Value = IIf(Val(wtxtMiscAmount_Amt.Text) <> 0, Val(wtxtMiscAmount_Amt.Text), DBNull.Value)
                    dtWMiscDetails.AcceptChanges()
                    GoTo AFTERINSERT
                End With
            End If
            Dim ro As DataRow = Nothing
            ro = dtWMiscDetails.NewRow
            ro("MISC") = wtxtMiscMisc.Text
            ro("AMOUNT") = IIf(Val(wtxtMiscAmount_Amt.Text) <> 0, Val(wtxtMiscAmount_Amt.Text), DBNull.Value)
            dtWMiscDetails.Rows.Add(ro)
            dtWMiscDetails.AcceptChanges()
AFTERINSERT:
            WCalcFinalTotal()
            wgridMisc.CurrentCell = wgridMisc.Rows(wgridMisc.RowCount - 1).Cells(0)

            wtxtMiscMisc.Clear()
            wtxtMiscAmount_Amt.Clear()
            wtxtMiscMisc.Select()
            wtxtMiscRowIndex.Clear()
        End If
    End Sub

    Private Sub WgridMisc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles wgridMisc.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub WgridMisc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles wgridMisc.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            wgridMisc.CurrentCell = wgridMisc.Rows(wgridMisc.CurrentRow.Index).Cells(0)
            With wgridMisc.Rows(wgridMisc.CurrentRow.Index)
                wtxtMiscMisc.Text = .Cells("MISC").FormattedValue
                wtxtMiscAmount_Amt.Text = .Cells("AMOUNT").FormattedValue
                wtxtMiscRowIndex.Text = wgridMisc.CurrentRow.Index
                wtxtMiscMisc.Focus()
                wtxtMiscMisc.SelectAll()
            End With
        End If
    End Sub

    Private Sub WgridMisc_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles wgridMisc.UserDeletedRow
        dtWMiscDetails.AcceptChanges()
        WCalcFinalTotal()
        If Not wgridMisc.RowCount > 0 Then
            wtxtMiscMisc.Select()
        End If

        Dim RowCheck() As DataRow = Nothing
        With ObjPurDetail
StDel:
            For Each ro As DataRow In .dtGridMisc.Rows
                RowCheck = dtWMiscDetails.Select("KEYNO = " & Val(ro.Item("KEYNO").ToString) & "")
                If RowCheck.Length = 0 Then
                    ro.Delete()
                    .dtGridMisc.AcceptChanges()
                    GoTo StDel
                End If
            Next
            .dtGridMisc.AcceptChanges()
            .CalcPurchaseGrossValue()
            .CalcPurchaseValue()
        End With


    End Sub
#End Region



    Private Sub WtxtLessWt_Wet_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles WtxtLessWt_Wet.GotFocus
        If Val(WtxtGrossWt_Wet.Text) = 0 Then
            SendKeys.Send("{TAB}")
            Exit Sub
        End If
        If LockLessWt = False Then Exit Sub
        If LockLessWt = True Then
            SendKeys.Send("{TAB}")
            Exit Sub
        End If
        If studdedStone = "Y" And Val(WtxtGrossWt_Wet.Text) <> Val(WtxtNetWt_Wet.Text) Then
            SendKeys.Send("{TAB}")
        ElseIf studdedStone = "N" And grossnetdiff = "N" Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub WtxtLessWt_Wet_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles WtxtLessWt_Wet.LostFocus
        WCalcFinalTotal()
    End Sub

    Private Sub WListSearch(ByVal cmb As ComboBox)
        If Not cmb.Items.Count > 0 Then Exit Sub
        wcmbSearch.Items.Clear()
        For Each obj As Object In cmb.Items
            wcmbSearch.Items.Add(obj)
        Next
        wpnlSearch.Visible = True
    End Sub

    Private Sub WHideSearch()
        wpnlSearch.Visible = False
        wcmbSearch.Items.Clear()
    End Sub

    Private Sub WcmbItemType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles wcmbPurity.GotFocus
        WListSearch(CType(sender, ComboBox))
    End Sub

    Private Sub WcmbItemType_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles wcmbPurity.LostFocus
        WHideSearch()
        wtxtMetalRate_Amt.Text = Format(WGetMetalRate(), "0.00")
        WCalcMaxMinValues()
    End Sub

    Private Sub WcmbItemType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles wcmbPurity.TextChanged
        wcmbSearch.Text = CType(sender, ComboBox).Text
    End Sub

    Private Sub WtxtMetalRate_Amt_Man_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles wtxtMetalRate_Amt.GotFocus
        wtxtMetalRate_Amt.Text = Format(WGetMetalRate(), "0.00")
    End Sub

    Private Sub WtxtStItem_MAN_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles wtxtStItem.GotFocus
        Main.ShowHelpText("Press Insert Key to Help")
    End Sub

    Private Sub WtxtStItem_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles wtxtStItem.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If wtxtStItem.Text = "" Then
                WLoadStoneItemName()
            ElseIf wtxtStItem.Text <> "" And objGPack.DupCheck("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wtxtStItem.Text & "'") = False Then
                WLoadStoneItemName()
            Else
                WLoadStoneitemDetails()
            End If
        End If
    End Sub

    Private Sub WtxtStItem_Man_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles wtxtStItem.LostFocus
        Main.HideHelpText()
    End Sub

    Private Sub WtxtStSubItem_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles wtxtStSubItem.GotFocus
        'If objGPack.DupCheck("SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem_MAN.Text & "')") = False Then
        SendKeys.Send("{TAB}")
        'End If
    End Sub

    Private Sub WtxtStItem_MAN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles wtxtStItem.KeyDown
        If e.KeyCode = Keys.Insert Then
            WLoadStoneItemName()
        ElseIf e.KeyCode = Keys.Down Then
            If wgridStone.RowCount > 0 Then
                wgridStone.Focus()
            End If
        End If
    End Sub

    Private Sub WLoadStoneItemName()
        strSql = " SELECT"
        strSql += " ITEMID AS ITEMID,ITEMNAME AS ITEMNAME"
        strSql += " FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ISNULL(STUDDED,'') = 'S' AND ACTIVE = 'Y'"
        Dim itemName As String = BrighttechPack.SearchDialog.Show("Find ItemName", strSql, cn, 1, 1, , wtxtStItem.Text)
        If itemName <> "" Then
            wtxtStItem.Text = itemName
            WLoadStoneitemDetails()
        Else
            wtxtStItem.Focus()
            wtxtStItem.SelectAll()
        End If
    End Sub

    Private Sub WLoadStoneitemDetails()

        If objGPack.GetSqlValue("SELECT SUBITEM FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wtxtStItem.Text & "'") = "Y" Then
            Dim DefItem As String = wtxtStSubItem.Text
            Dim itemId As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wtxtStItem.Text & "'"))
            If Not _SubItemOrderByName Then
                DefItem = objGPack.GetSqlValue("SELECT DISPLAYORDER FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & wtxtStSubItem.Text & "' AND ITEMID = " & itemId & "")
            End If
            strSql = GetSubItemQry(New String() {"SUBITEMID ID", "DISPLAYORDER DISP", "SUBITEMNAME SUBITEM"}, itemId)
            wtxtStSubItem.Text = BrighttechPack.SearchDialog.Show("Search SubItem", strSql, cn, IIf(_SubItemOrderByName, 2, 1), 2, , DefItem, , False, True)
        Else
            wtxtStSubItem.Clear()
        End If

        If wtxtStSubItem.Text <> "" Then
            strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & wtxtStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wtxtStItem.Text & "')"
        Else
            strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wtxtStItem.Text & "'"
        End If
        Dim calType As String = objGPack.GetSqlValue(strSql, , , tran)
        wcmbStCalc.Text = IIf(calType = "R", "P", "W")

        If wtxtStSubItem.Text <> "" Then
            strSql = " SELECT STONEUNIT FROM " & cnAdminDb & "..SUBITEMMAST  WHERE SUBITEMNAME = '" & wtxtStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wtxtStItem.Text & "')"
        Else
            strSql = " SELECT STONEUNIT FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wtxtStItem.Text & "'"
        End If
        wcmbStUnit.Text = objGPack.GetSqlValue(strSql, , , tran)

        strSql = " SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wtxtStItem.Text & "'"
        wtxtStMetalCode.Text = objGPack.GetSqlValue(strSql, "DIASTONE", , tran)
        'If WtxtStMetalCode.Text = "S" Then WcmbStUnit.Text = "G" Else WcmbStUnit.Text = "C"
        Me.SelectNextControl(wtxtStItem, True, True, True, True)
    End Sub


    Private Sub WtxtMiscMisc_Man_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles wtxtMiscMisc.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If wtxtMiscMisc.Text = "" Then Exit Sub
            If objGPack.DupCheck("SELECT MISCNAME FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCNAME = '" & wtxtMiscMisc.Text & "' AND  ACTIVE = 'Y'") = False Then
                MsgBox("Invalid Misc", MsgBoxStyle.Information)
                wtxtMiscMisc.Focus()
            End If
        End If
    End Sub

    Private Sub WtxtMiscMisc_Man_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles wtxtMiscMisc.LostFocus
        If wtxtMiscMisc.Text = "" Then Exit Sub
        strSql = " SELECT DEFAULTVALUE FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCNAME = '" & wtxtMiscMisc.Text & "'"
        Dim amt As Double = Val(objGPack.GetSqlValue(strSql, "DEFAULTVALUE", , tran))
        wtxtMiscAmount_Amt.Text = IIf(amt <> 0, Format(amt, "0.00"), "")
    End Sub

    Private Sub WtxtMMCategory_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles wtxtMMCategory.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If wtxtMMCategory.Text = "" Then Exit Sub
            If objGPack.DupCheck("SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & wtxtMMCategory.Text & "'") = False Then
                MsgBox("Invalid Category", MsgBoxStyle.Information)
                wtxtMMCategory.Focus()
            End If
        End If
    End Sub

    Private Sub WtxtMetalRate_Amt_Man_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles wtxtMetalRate_Amt.TextChanged
        WCalcFinalTotal()
    End Sub

    Private Sub WtxtMMAmount_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles wtxtMMAmount_AMT.KeyDown
        If e.KeyCode = Keys.Down Then
            wgridMultimetal.Select()
        End If
    End Sub

    Private Sub WtxtMMAmount_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles wtxtMMAmount_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If wtxtMMCategory.Text = "" Then
                MsgBox("Category Should Not Empty", MsgBoxStyle.Information)
                wtxtMMCategory.Focus()
                Exit Sub
            ElseIf objGPack.DupCheck("SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & wtxtMMCategory.Text & "'") = False Then
                MsgBox("Invalid Category", MsgBoxStyle.Information)
                wtxtMMCategory.Focus()
                Exit Sub
            ElseIf (Not Val(wtxtMMWeight_Wet.Text) > 0) And (Not Val(wtxtMMAmount_AMT.Text) > 0) Then
                MsgBox("Weight,Rate,Amount Should not Empty", MsgBoxStyle.Information)
                wtxtMMWeight_Wet.Focus()
                Exit Sub
            End If
            If wtxtMMRowIndex.Text <> "" Then
                With dtWMultiMetalDetails.Rows(Val(wtxtMMRowIndex.Text))
                    .Item("CATEGORY") = wtxtMMCategory.Text
                    .Item("WEIGHT") = IIf(Val(wtxtMMWeight_Wet.Text) <> 0, Val(wtxtMMWeight_Wet.Text), DBNull.Value)
                    .Item("WASTAGEPER") = IIf(Val(wtxtMMWastagePer_PER.Text) <> 0, Val(wtxtMMWastagePer_PER.Text), DBNull.Value)
                    .Item("WASTAGE") = IIf(Val(wtxtMMWastage_WET.Text) <> 0, Val(wtxtMMWastage_WET.Text), DBNull.Value)
                    .Item("MCPERGRM") = IIf(Val(wtxtMMMcPerGRm_AMT.Text) <> 0, Val(wtxtMMMcPerGRm_AMT.Text), DBNull.Value)
                    .Item("MC") = IIf(Val(wtxtMMMc_AMT.Text) <> 0, Val(wtxtMMMc_AMT.Text), DBNull.Value)
                    .Item("AMOUNT") = IIf(Val(wtxtMMAmount_AMT.Text) <> 0, Val(wtxtMMAmount_AMT.Text), DBNull.Value)
                    .Item("RATE") = IIf(Val(wtxtMMRate.Text) <> 0, Val(wtxtMMRate.Text), DBNull.Value)
                    dtWMultiMetalDetails.AcceptChanges()
                    GoTo AFTERINSERT
                End With
            End If
            Dim ro As DataRow = Nothing
            ro = dtWMultiMetalDetails.NewRow
            ro("CATEGORY") = wtxtMMCategory.Text
            ro("WEIGHT") = IIf(Val(wtxtMMWeight_Wet.Text) <> 0, Val(wtxtMMWeight_Wet.Text), DBNull.Value)
            ro("WASTAGEPER") = IIf(Val(wtxtMMWastagePer_PER.Text) <> 0, Val(wtxtMMWastagePer_PER.Text), DBNull.Value)
            ro("WASTAGE") = IIf(Val(wtxtMMWastage_WET.Text) <> 0, Val(wtxtMMWastage_WET.Text), DBNull.Value)
            ro("MCPERGRM") = IIf(Val(wtxtMMMcPerGRm_AMT.Text) <> 0, Val(wtxtMMMcPerGRm_AMT.Text), DBNull.Value)
            ro("MC") = IIf(Val(wtxtMMMc_AMT.Text) <> 0, Val(wtxtMMMc_AMT.Text), DBNull.Value)
            ro("AMOUNT") = IIf(Val(wtxtMMAmount_AMT.Text) <> 0, Val(wtxtMMAmount_AMT.Text), DBNull.Value)
            ro("RATE") = IIf(Val(wtxtMMRate.Text) <> 0, Val(wtxtMMRate.Text), DBNull.Value)
            dtWMultiMetalDetails.Rows.Add(ro)
AFTERINSERT:
            WCalcFinalTotal()

            wtxtMMWeight_Wet.Clear()
            wtxtMMWastagePer_PER.Clear()
            wtxtMMWastage_WET.Clear()
            wtxtMMMcPerGRm_AMT.Clear()
            wtxtMMMc_AMT.Clear()
            wtxtMMAmount_AMT.Clear()
            wtxtMMRowIndex.Clear()
            wtxtMMCategory.Select()
            wtxtMMRate.Clear()
        End If
    End Sub

    Private Sub WtxtMMAmount_AMT_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles wtxtMMAmount_AMT.LostFocus
        wtxtMMCategory.Focus()
    End Sub

    Private Sub WtxtMMWastage_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles wtxtMMWastage_WET.KeyPress
        If Val(wtxtMMWastagePer_PER.Text) > 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub WtxtMMWastagePer_PER_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles wtxtMMWastagePer_PER.TextChanged
        Dim wt As Double = Val(wtxtMMWeight_Wet.Text) * (Val(wtxtMMWastagePer_PER.Text) / 100)
        wt = Math.Round(wt, WastageRound)
        wtxtMMWastage_WET.Text = IIf(wt <> 0, Format(wt, "0.000"), "")
    End Sub

    Private Sub WtxtMMWastagePer_PER_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles wtxtMMWastagePer_PER.KeyDown
        If e.KeyCode = Keys.Down Then
            If wgridMultimetal.RowCount > 0 Then
                wgridMultimetal.Focus()
            End If
        End If
    End Sub

    Private Sub WtxtMMWastage_WET_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles wtxtMMWastage_WET.GotFocus
        If Val(wtxtMMWastagePer_PER.Text) > 0 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub WtxtMMWastage_WET_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles wtxtMMWastage_WET.KeyDown
        If e.KeyCode = Keys.Down Then
            If wgridMultimetal.RowCount > 0 Then
                wgridMultimetal.Focus()
            End If
        End If
    End Sub

    Private Sub WtxtMMMcPerGRm_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles wtxtMMMcPerGRm_AMT.KeyDown
        If e.KeyCode = Keys.Down Then
            If wgridMultimetal.RowCount > 0 Then
                wgridMultimetal.Focus()
            End If
        End If
    End Sub
    Private Sub WtxtMMMcPerGRm_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles wtxtMMMcPerGRm_AMT.TextChanged
        Dim mc As Double = Val(wtxtMMWeight_Wet.Text) * Val(wtxtMMMcPerGRm_AMT.Text)
        wtxtMMMc_AMT.Text = IIf(mc <> 0, Format(mc, "0.00"), "")
    End Sub

    Private Sub WtxtMMMc_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles wtxtMMMc_AMT.GotFocus
        If Val(wtxtMMMcPerGRm_AMT.Text) > 0 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub WtxtMMMc_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles wtxtMMMc_AMT.KeyDown
        If e.KeyCode = Keys.Down Then
            If wgridMultimetal.RowCount > 0 Then
                wgridMultimetal.Focus()
            End If
        End If
    End Sub
    Private Sub WtxtMMMc_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles wtxtMMMc_AMT.KeyPress
        If Val(wtxtMMMcPerGRm_AMT.Text) > 0 Then
            e.Handled = True
        End If
    End Sub
    Private Sub WtxtMMCategory_MAN_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles wtxtMMCategory.GotFocus
        If ALLOYGOLD_SALEMODE Then
            If Val(wgridMultiMetalTotal.Rows(0).Cells("WEIGHT").Value.ToString) = 0 Then
                Dim basecatcode As String
                strSql = "SELECT I.CATCODE,C.CATNAME,NETWTPER FROM " & cnAdminDb & "..ITEMMAST I INNER JOIN " & cnAdminDb & "..CATEGORY C ON I.CATCODE = C.CATCODE WHERE I.ITEMNAME = '" & wcmbItem_MAN.Text & "'"
                Dim alloydr As DataRow
                alloydr = GetSqlRow(strSql, cn)
                If Not alloydr Is Nothing Then
                    If Val(alloydr(2).ToString) <> 0 Then
                        wtxtMMCategory.Text = alloydr(1).ToString
                        wtxtMMWeight_Wet.Text = Val(WtxtGrossWt_Wet.Text) * ((100 - Val(alloydr(2).ToString)) / 100)

                    End If
                End If
            Else
                wtxtMMWeight_Wet.Text = Val(WtxtGrossWt_Wet.Text) - Val(wgridMultiMetalTotal.Rows(0).Cells("WEIGHT").Value.ToString)
            End If
        End If
        Main.ShowHelpText("Press Insert Key to Help")
    End Sub

    Private Sub WtxtMMCategory_MAN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles wtxtMMCategory.KeyDown
        If e.KeyCode = Keys.Insert Then
            WLoadCatName()
        ElseIf e.KeyCode = Keys.Down Then
            If wgridMultimetal.RowCount > 0 Then
                wgridMultimetal.Focus()
            End If
        End If
    End Sub

    Private Sub WtxtMMCategory_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles wtxtMMCategory.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If wtxtMMCategory.Text = "" Then
                WLoadCatName()
            ElseIf wtxtMMCategory.Text <> "" And objGPack.DupCheck("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & wtxtMMCategory.Text & "'") = False Then
                WLoadCatName()
            Else
                WLoadCatDetails()
            End If
        End If
    End Sub

    Private Sub WtxtMMCategory_MAN_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles wtxtMMCategory.LostFocus
        Main.HideHelpText()
    End Sub
    Private Sub WLoadCatName()
        strSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
        strSql += " WHERE PURITYID IN (SELECT PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE METALTYPE = 'O')"
        strSql += " ORDER BY CATNAME"
        Dim catName As String = BrighttechPack.SearchDialog.Show("Find CatName", strSql, cn, , , , wtxtMMCategory.Text)
        If catName <> "" Then
            wtxtMMCategory.Text = catName
            WLoadCatDetails()
        Else
            wtxtMMCategory.Select()
            wtxtMMCategory.SelectAll()
        End If
    End Sub
    Private Sub WLoadCatDetails()
        If wtxtMMCategory.Text <> "" Then
            wtxtMMRate.Text = Val(GetRate(wdtpRecieptDate.Value, objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & wtxtMMCategory.Text & "'")))
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub WtxtMMRate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles wtxtMMRate.KeyDown
        If e.KeyCode = Keys.Down Then
            If wgridMultimetal.RowCount > 0 Then
                wgridMultimetal.Focus()
            End If
        End If
    End Sub

    Private Sub WtxtMMWeight_Wet_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles wtxtMMWeight_Wet.KeyDown
        If e.KeyCode = Keys.Down Then
            If wgridMultimetal.RowCount > 0 Then
                wgridMultimetal.Focus()
            End If
        End If
    End Sub
    Private Sub WtxtMiscMisc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles wtxtMiscMisc.GotFocus
        Main.ShowHelpText("Press Insert Key to Help")
    End Sub

    Private Sub WtxtMiscMisc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles wtxtMiscMisc.KeyDown
        If e.KeyCode = Keys.Insert Then
            WLoadMiscName()
        ElseIf e.KeyCode = Keys.Down Then
            If wgridMisc.RowCount > 0 Then wgridMisc.Focus()
        End If
    End Sub

    Private Sub WtxtMiscMisc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles wtxtMiscMisc.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If wtxtMiscMisc.Text = "" Then
                WLoadMiscName()
            ElseIf wtxtMiscMisc.Text <> "" And objGPack.DupCheck("SELECT MISCNAME FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCNAME = '" & wtxtMiscMisc.Text & "'") = False Then
                WLoadMiscName()
            Else
                WLoadMiscDetails()
            End If
        End If
    End Sub

    Private Sub WtxtMiscMisc_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles wtxtMiscMisc.LostFocus
        Main.HideHelpText()
    End Sub
    Private Sub WLoadMiscName()
        strSql = " SELECT MISCNAME FROM " & cnAdminDb & "..MISCCHARGES WHERE ACTIVE = 'Y'"
        Dim Name As String = BrighttechPack.SearchDialog.Show("Find MiscName", strSql, cn, , , , wtxtMiscMisc.Text)
        If Name <> "" Then
            wtxtMiscMisc.Text = Name
            WLoadMiscDetails()
        Else
            wtxtMiscMisc.Focus()
            wtxtMiscMisc.SelectAll()
        End If
    End Sub
    Private Sub WLoadMiscDetails()
        If wtxtMiscMisc.Text <> "" Then
            strSql = " SELECT DEFAULTVALUE FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCNAME = '" & wtxtMiscMisc.Text & "'"
            Dim amt As Double = Val(objGPack.GetSqlValue(strSql, "DEFAULTVALUE", , tran))
            wtxtMiscAmount_Amt.Text = IIf(amt <> 0, Format(amt, "0.00"), "")
            Me.SelectNextControl(wtxtMiscMisc, True, True, True, True)
        End If
    End Sub

    Private Sub WtxtTagNo__Man_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles wtxtTagNo__Man.KeyPress
        Select Case e.KeyChar
            Case "\", "/", "*", """", "<", ">", "|", ":", "."
                e.Handled = True
        End Select
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim _TagNoFrom = GetSoftValue("TAGNOFROM")
            If _TagNoFrom = "I" Then
                strSql = "SELECT TAGNO FROM " & cnAdminDb & "..WITEMTAG"
                strSql += " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wcmbItem_MAN.Text & "') "
                strSql += " AND TAGNO = '" & wtxtTagNo__Man.Text & "'"
                If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
                    MsgBox("TagNo Already Exist", MsgBoxStyle.Information)
                    wtxtTagNo__Man.Focus()
                End If
            ElseIf _TagNoFrom = "U" Then
                strSql = "SELECT TAGNO FROM " & cnAdminDb & "..WITEMTAG"
                strSql += " WHERE TAGNO = '" & wtxtTagNo__Man.Text & "'"
                If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
                    MsgBox("TagNo Already Exist", MsgBoxStyle.Information)
                    wtxtTagNo__Man.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub WcmbItem_Man_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles wcmbItem_MAN.GotFocus
        ItemLock = True
        WListSearch(CType(sender, ComboBox))
    End Sub

    Private Sub WcmbItem_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles wcmbItem_MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If wcmbItem_MAN.Text = "" Then Exit Sub
            Dim AA As Integer = Val(wcmbItem_MAN.Text)
            If Val(wcmbItem_MAN.Text) <> 0 And wcmbItem_MAN.Text = AA.ToString() Then
                ''entrered itemid 
                Dim itemName As String = objGPack.GetSqlValue("SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & wcmbItem_MAN.Text & " AND STOCKTYPE = 'T' AND ACTIVE = 'Y' AND ISNULL(STUDDED,'') <> 'Y'")
                If itemName <> "" Then wcmbItem_MAN.Text = itemName
            End If
            If objGPack.DupCheck("SELECT 1 FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wcmbItem_MAN.Text & "' AND STYLENO = 'Y'") Then
                isStyleCode = True
            End If
            Dim dt As New DataTable
            wfuncAssignTabControls()
            funcAssignTabControls()
            'txtLotNo_Num_Man.Enabled = False
            ItemLock = False
            Me.SelectNextControl(wcmbItem_MAN, True, True, True, True)
        End If
    End Sub

    Private Sub WcmbItem_MAN_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles wcmbItem_MAN.Leave
        If wcmbItem_MAN.Text <> "" And WcmbSubItem_OWN.Enabled = False Then
            WcmbSubItem_OWN.Items.Clear()
            If objGPack.GetSqlValue("SELECT SUBITEM FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wcmbItem_MAN.Text & "'") = "Y" Then
                strSql = GetSubItemQry(New String() {"SUBITEMNAME"}, Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wcmbItem_MAN.Text & "'")))
                objGPack.FillCombo(strSql, WcmbSubItem_OWN, False)
                WcmbSubItem_OWN.Enabled = True
                WcmbSubItem_OWN.Focus()
                Dim dtSubItem As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtSubItem)
                BrighttechPack.GlobalMethods.FillCombo(WchkcmbSubItem, dtSubItem, "SUBITEMNAME")
                WchkcmbSubItem.Enabled = True
            Else
                WcmbSubItem_OWN.Enabled = False
                WcmbSubItem_OWN.Text = ""
                WchkcmbSubItem.Enabled = False
                WchkcmbSubItem.Text = ""
            End If
        End If
        If ItemLock Then
            wcmbItem_MAN.Select()
            Exit Sub
        End If
    End Sub

    Private Sub WcmbItem_Man_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles wcmbItem_MAN.LostFocus
        WHideSearch()
        'If cmbItem_MAN.Text = "" Or cmbItem_MAN.Items.Contains(cmbItem_MAN.Text) = False Then
        '    cmbItem_MAN.Focus()
        '    Exit Sub
        'End If
        'Me.SelectNextControl(txtLotNo_Num_Man, True, True, True, True)
    End Sub

    Private Sub WcmbItem_MAN_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles wcmbItem_MAN.SelectedIndexChanged

        If wcmbItem_MAN.Text.Trim = "" Then Exit Sub
        strSql = " SELECT METALID,STUDDEDSTONE,SUBITEM"
        strSql += " FROM " & cnAdminDb & "..ITEMMAST as i WHERE ITEMNAME = '" & wcmbItem_MAN.Text & "'"
        Dim dtItemDet As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtItemDet)
        If dtItemDet.Rows.Count > 0 Then
            studdedStone = dtItemDet.Rows(0).Item("STUDDEDSTONE")
            WcmbSubItem_OWN.Items.Clear()
            If dtItemDet.Rows(0).Item("SUBITEM").ToString = "Y" Then
                strSql = GetSubItemQry(New String() {"SUBITEMNAME"}, Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wcmbItem_MAN.Text & "'")))
                objGPack.FillCombo(strSql, WcmbSubItem_OWN, False)
                WcmbSubItem_OWN.Enabled = True
                Dim dtSubItem As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtSubItem)
                BrighttechPack.GlobalMethods.FillCombo(WchkcmbSubItem, dtSubItem, "SUBITEMNAME")
                WchkcmbSubItem.Enabled = True
            Else
                WcmbSubItem_OWN.Enabled = False
                WcmbSubItem_OWN.Text = ""
                WchkcmbSubItem.Enabled = False
                WchkcmbSubItem.Text = ""
            End If
        End If
        wfuncAssignTabControls()
        funcAssignTabControls()
    End Sub

    Private Sub WWeighingScalePropertyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WeighingScalePropertyToolStripMenuItem.Click
        objPropertyDia = New PropertyDia(wSerialPort1)
        objPropertyDia.ShowDialog()
    End Sub


    Private Sub WtxtGrossWt_Wet_MAN_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles WtxtGrossWt_Wet.TextChanged
        WCalcMaxMinValues()
        WCalcFinalTotal()
    End Sub

    Private Sub WcmbStCalc_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles wcmbStCalc.KeyPress
        '        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
        '            If WtxtStRate_Amt.Enabled = True And NEEDUS = True Then
        'RsUSView:
        '                e.Handled = True
        '                If ObjRsUs.ShowDialog = Windows.Forms.DialogResult.OK Then
        '                    txtRate_Amt.Text = Val(ObjRsUs.txtUSDollar_Amt.Text) * Val(ObjRsUs.txtIndRs_Amt.Text)
        '                    If Val(txtRate_Amt.Text) = 0 Then
        '                        MsgBox("Rate should not empty", MsgBoxStyle.Information)
        '                        ObjRsUs.txtUSDollar_Amt.Focus()
        '                        GoTo RsUSView
        '                    End If
        '                End If
        '            End If
        '        End If
    End Sub


    Private Sub WtxtstPackettno_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles wtxtstPackettno.KeyDown
        If e.KeyCode = Keys.Down Then
            If wgridStone.RowCount > 0 Then
                wgridStone.Focus()
            End If
        End If
    End Sub

    Private Sub WtxtstPackettno_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles wtxtstPackettno.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If PacketNoEnable = "R" Then
                If wtxtstPackettno.Text = "" Then
                    MsgBox("Packet No. must enter", MsgBoxStyle.Information)
                    wtxtstPackettno.Focus()
                    Exit Sub
                End If
            End If
            Me.SelectNextControl(WtabStone, True, True, True, True)
        End If
    End Sub
    Private Sub WcmbAddM1_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles wcmbAddM1_OWN.GotFocus
        wcmbSearch.Text = ""
        WListSearch(CType(sender, ComboBox))
    End Sub

    Private Sub WcmbAddM1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles wcmbAddM1_OWN.LostFocus
        WHideSearch()
    End Sub

    Private Sub WcmbAddM2_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles wcmbAddM2_OWN.GotFocus
        wcmbSearch.Text = ""
        WListSearch(CType(sender, ComboBox))
    End Sub

    Private Sub WcmbAddM2_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles wcmbAddM2_OWN.LostFocus
        WHideSearch()
    End Sub

    Private Sub WcmbAddM1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles wcmbAddM1_OWN.TextChanged
        wcmbSearch.Text = CType(sender, ComboBox).Text
    End Sub

    Private Sub WcmbAddM2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles wcmbAddM2_OWN.TextChanged
        wcmbSearch.Text = CType(sender, ComboBox).Text
    End Sub
    Private Function WfuncSplitDiaStn()
        If dtWStoneDetails.Rows.Count > 0 Then
            wGridDia.DataSource = Nothing
            wGridStn.DataSource = Nothing
            Dim DtTem As New DataTable
            Dim DtDia As New DataTable
            Dim DtStn As New DataTable
            Dim DView As New DataView
            Dim SView As New DataView
            DtTem = dtWStoneDetails.Copy
            DView = DtTem.DefaultView
            With DView
                .RowFilter = "METALID='D'"
            End With
            DtDia = DView.ToTable(True, "ITEM", "SUBITEM", "SHAPE", "COLOR", "CLARITY", "PCS", "WEIGHT", "RATE", "AMOUNT")
            SView = DtTem.DefaultView
            With SView
                .RowFilter = "METALID<>'D'"
            End With
            DtStn = SView.ToTable(True, "ITEM", "SUBITEM", "SHAPE", "COLOR", "CLARITY", "PCS", "WEIGHT", "RATE", "AMOUNT")
            For v As Integer = 0 To DtDia.Rows.Count - 1
                DtDia.Rows(v).Item("ITEM") = IIf(DtDia.Rows(v).Item("SUBITEM").ToString.Trim = "", DtDia.Rows(v).Item("ITEM").ToString, DtDia.Rows(v).Item("SUBITEM").ToString) & " " & DtDia.Rows(v).Item("SHAPE").ToString & " " & DtDia.Rows(v).Item("COLOR").ToString & " " & DtDia.Rows(v).Item("CLARITY").ToString
            Next
            wGridDia.DataSource = DtDia

            wGridDia.Columns("ITEM").Width = 200
            wGridDia.Columns("SUBITEM").Visible = False
            wGridDia.Columns("SHAPE").Visible = False
            wGridDia.Columns("COLOR").Visible = False
            wGridDia.Columns("CLARITY").Visible = False
            wGridDia.Columns("PCS").Width = 50
            wGridDia.Columns("WEIGHT").Width = 70
            wGridDia.Columns("RATE").Width = 70
            wGridDia.Columns("AMOUNT").Width = 80
            For v As Integer = 0 To DtStn.Rows.Count - 1
                DtStn.Rows(v).Item("ITEM") = IIf(DtStn.Rows(v).Item("SUBITEM").ToString.Trim = "", DtStn.Rows(v).Item("ITEM").ToString, DtStn.Rows(v).Item("SUBITEM").ToString) & " " & DtStn.Rows(v).Item("SHAPE").ToString & " " & DtStn.Rows(v).Item("COLOR").ToString & " " & DtStn.Rows(v).Item("CLARITY").ToString
            Next
            wGridStn.DataSource = DtStn
            wGridStn.Columns("ITEM").Width = 200
            wGridStn.Columns("SUBITEM").Visible = False
            wGridStn.Columns("SHAPE").Visible = False
            wGridStn.Columns("COLOR").Visible = False
            wGridStn.Columns("CLARITY").Visible = False
            wGridStn.Columns("PCS").Width = 50
            wGridStn.Columns("WEIGHT").Width = 70
            wGridStn.Columns("RATE").Width = 70
            wGridStn.Columns("AMOUNT").Width = 80
        End If
    End Function

    Private Sub WcmbItem_MAN_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles wcmbItem_MAN.TextChanged
        wcmbSearch.Text = CType(sender, ComboBox).Text
    End Sub

    Private Sub wtxtNameOfProduct_MAN_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles wtxtNameOfProduct_OWN.Leave
        If wtxtNameOfProduct_OWN.Text.Trim <> "" Then
            If tagEdit = False And Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnAdminDb & "..WITEMTAG WHERE DESCRIP='" & wtxtNameOfProduct_OWN.Text.Replace("'", "''") & "'", , "", )) > 0 Then MsgBox("This name already exists.", MsgBoxStyle.Information) : wtxtNameOfProduct_OWN.Focus() : Exit Sub
        End If
    End Sub
    Function WCalcSummary()
        wGridSummary.DataSource = Nothing
        Dim Grate As Double = WGetMetalRate()
        Dim gamt As Decimal = Math.Round(Grate * Val(WtxtNetWt_Wet.Text), 2)
        Dim Diaamt As Decimal
        Dim Stnamt As Decimal
        Dim Mc As Decimal = Val(wtxtMkCharge_Amt.Text)
        Dim Vat As Decimal
        Dim Total As Decimal
        Dim Dtsummary As New DataTable
        With Dtsummary.Columns
            .Add("TYPE", GetType(String))
            .Add("GOLD", GetType(Double))
            .Add("DIAMOND", GetType(Double))
            .Add("GEMSSTONE", GetType(Double))
            .Add("MC", GetType(Double))
            .Add("VAT", GetType(Double))
            .Add("AMOUNT", GetType(Double))
        End With
        Dim vatper As Decimal = Val(objGPack.GetSqlValue("SELECT SALESTAX FROM " & cnAdminDb & "..CATEGORY WHERE ITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & wcmbItem_MAN.Text & "' )", , , ))
        If vatper = 0 Then vatper = 1
        strSql = "SELECT DISTINCT UNIQUECODE,MAX(COLORID)COLORID,MAX(CLARITYID)CLARITYID FROM " & cnAdminDb & "..DIASTYLE GROUP BY UNIQUECODE "
        Dim DtDiastyle As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(DtDiastyle)
        Dim dtWstud As New DataTable
        dtWstud = dtWStoneDetails.Copy
        Dim dtDia As New DataTable
        Dim DView As New DataView
        DView = dtWstud.DefaultView
        With DView
            .RowFilter = "METALID='D'"
        End With
        dtDia = DView.ToTable(True, "SHAPE", "PCS", "WEIGHT", "ITEM", "SUBITEM", "RATE")
        Dim Dtdiashape As New DataTable
        Dtdiashape = dtDia.DefaultView.ToTable(True, "SHAPE", "ITEM", "SUBITEM", "RATE")
        Dim dtstn As New DataTable
        Dim SView As New DataView
        SView = dtWstud.DefaultView
        With SView
            .RowFilter = "METALID<>'D'"
        End With
        dtstn = SView.ToTable(True, "SHAPE", "PCS", "WEIGHT", "AMOUNT")
        If dtstn.Rows.Count > 0 Then
            Stnamt = dtstn.Compute("SUM(AMOUNT)", "")
        End If

        If dtWStoneDetails.Rows.Count > 0 And DtDiastyle.Rows.Count > 0 And Dtdiashape.Rows.Count > 0 Then
            For v As Integer = 0 To DtDiastyle.Rows.Count - 1
                Diaamt = 0
                For m As Integer = 0 To Dtdiashape.Rows.Count - 1
                    'Dim shapename As String = objGPack.GetSqlValue("SELECT SHAPENAME FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPEID='" & Val(DtDiastyle.Rows(v).Item("SHAPEID").ToString) & "'", , , )
                    Dim shapeid As Integer = objGPack.GetSqlValue("SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME='" & Dtdiashape.Rows(m).Item("SHAPE").ToString & "'", , , )
                    ' If shapename = Dtdiashape.Rows(m).Item("SHAPE").ToString Then
                    Dim Diawt As Decimal = 0
                    Dim Diapcs As Integer = 0
                    Diawt = Val(dtDia.Compute("SUM(WEIGHT)", "SHAPE='" & Dtdiashape.Rows(m).Item("SHAPE").ToString & "' AND ITEM='" & Dtdiashape.Rows(m).Item("ITEM").ToString & "' AND SUBITEM='" & Dtdiashape.Rows(m).Item("SUBITEM").ToString & "' AND RATE='" & Dtdiashape.Rows(m).Item("RATE").ToString & "'").ToString)
                    Diapcs = Val(dtDia.Compute("SUM(PCS)", "SHAPE='" & Dtdiashape.Rows(m).Item("SHAPE").ToString & "' AND ITEM='" & Dtdiashape.Rows(m).Item("ITEM").ToString & "' AND SUBITEM='" & Dtdiashape.Rows(m).Item("SUBITEM").ToString & "' AND RATE='" & Dtdiashape.Rows(m).Item("RATE").ToString & "'").ToString)
                    Dim cent As Double = 0
                    If Diapcs = 0 Then Diapcs = 1
                    strSql = " SELECT ISNULL(CALTYPE,'') CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') ='" & Dtdiashape.Rows(m).Item("ITEM").ToString & "'"
                    Dim mCaltype As String = objGPack.GetSqlValue(strSql, "CALTYPE", "")
                    strSql = " SELECT ISNULL(CALTYPE,'') CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE ISNULL(SUBITEMNAME,'') ='" & Dtdiashape.Rows(m).Item("SUBITEM").ToString & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & Dtdiashape.Rows(m).Item("ITEM").ToString & "')"
                    mCaltype = objGPack.GetSqlValue(strSql, "CALTYPE", "")
                    If mCaltype = "D" Then
                        cent = Diawt
                    Else
                        cent = Diawt / Diapcs
                    End If

                    'cent = Diawt / Diapcs
                    cent *= 100
                    strSql = " DECLARE @CENT FLOAT"
                    strSql += vbCrLf + " SET @CENT = " & cent & ""
                    strSql += vbCrLf + " SELECT MAXRATE FROM " & cnAdminDb & "..CENTRATE WHERE"
                    strSql += vbCrLf + " @CENT BETWEEN FROMCENT AND TOCENT "
                    strSql += vbCrLf + " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & Dtdiashape.Rows(m).Item("ITEM").ToString & "')"
                    strSql += vbCrLf + " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & Dtdiashape.Rows(m).Item("SUBITEM").ToString & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & Dtdiashape.Rows(m).Item("ITEM").ToString & "')),0)"
                    strSql += vbCrLf + " AND COLORID=" & Val(DtDiastyle.Rows(v).Item("COLORID").ToString)
                    strSql += vbCrLf + " AND CLARITYID=" & Val(DtDiastyle.Rows(v).Item("CLARITYID").ToString)
                    strSql += vbCrLf + " AND SHAPEID=" & shapeid
                    Dim rate As Double = Val(objGPack.GetSqlValue(strSql, "MAXRATE", "", tran))
                    If rate <> 0 Then
                        Diaamt = Diaamt + Math.Round(Diawt * rate, 2)
                    Else
                        Diaamt = Diaamt + Math.Round(Diawt * Val(Dtdiashape.Rows(m).Item("RATE").ToString), 2)
                    End If
                    'End If
                Next
                Vat = Math.Round(((gamt + Diaamt + Stnamt + Mc) / 100) * vatper, 2)
                Dim ro As DataRow = Nothing
                ro = Dtsummary.NewRow
                ro("TYPE") = DtDiastyle.Rows(v).Item("UNIQUECODE").ToString
                ro("GOLD") = gamt
                ro("DIAMOND") = Diaamt
                ro("GEMSSTONE") = Stnamt
                ro("MC") = Mc
                ro("VAT") = Vat
                ro("AMOUNT") = (gamt + Diaamt + Stnamt + Mc + Vat)
                Dtsummary.Rows.Add(ro)
                Dtsummary.AcceptChanges()
            Next
        End If
        wGridSummary.DataSource = Dtsummary
        wGridSummary.Columns("TYPE").Width = 70
        wGridSummary.Columns("GOLD").Width = 70
        wGridSummary.Columns("DIAMOND").Width = 70
        wGridSummary.Columns("GEMSSTONE").Width = 70
        wGridSummary.Columns("MC").Width = 70
        wGridSummary.Columns("VAT").Width = 70
        wGridSummary.Columns("AMOUNT").Width = 70
    End Function

    Private Sub WtxtSalValue_Amt_Man_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles wtxtSalValue_Amt_Man.GotFocus
        WCalcSummary()
    End Sub
    Private Sub wbtnGenerateTag_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles wbtnGenerateTag.Click

        If Not tagEdit Then
            If STKAFINDATE = False Then
                If Not CheckDate(wdtpRecieptDate.Value) Then Exit Sub
                If CheckEntryDate(wdtpRecieptDate.Value) Then Exit Sub
            End If
        End If
        strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wcmbItem_MAN.Text & "'"
        Dim calcType As String = objGPack.GetSqlValue(strSql)
        ''Weight Rate Validation
        Select Case calcType.ToUpper
            Case "W"
                If Val(WtxtGrossWt_Wet.Text) = 0 Then
                    MsgBox("Grs Weight should not empty", MsgBoxStyle.Information)
                    WtxtGrossWt_Wet.Focus()
                    Exit Sub
                End If
            Case "R"
                'If Val(txtRate_Amt.Text) = 0 Then
                '    MsgBox("Rate should not empty", MsgBoxStyle.Information)
                '    txtRate_Amt.Focus()
                '    Exit Sub
                'End If
            Case "B"
                If Val(WtxtGrossWt_Wet.Text) = 0 Then
                    MsgBox("Grs Weight should not empty", MsgBoxStyle.Information)
                    WtxtGrossWt_Wet.Focus()
                    Exit Sub
                End If
                'If Val(txtRate_Amt.Text) = 0 Then
                '    MsgBox("Rate should not empty", MsgBoxStyle.Information)
                '    txtRate_Amt.Focus()
                '    Exit Sub
                'End If
            Case "F"
                If Val(WtxtGrossWt_Wet.Text) = 0 Then
                    MsgBox("Weight should not empty", MsgBoxStyle.Information)
                    WtxtGrossWt_Wet.Focus()
                    Exit Sub
                End If
        End Select

nnnext:
        If picPath <> Nothing Then
            If Not IO.Directory.Exists(defalutDestination) Then
                MsgBox(defalutDestination & vbCrLf & " Destination Path not found" & vbCrLf & "Please make the correct path", MsgBoxStyle.Information)
                Exit Sub
            End If
        End If
        If objGPack.Validator_Check(wgrpSaveControls) Then Exit Sub
        Dim ds As New DataSet
        ds.Clear()
        Dim tDd As Date = GetServerDate()
        If GetAdmindbSoftValue("GLOBALDATE").ToUpper = "Y" Then
            tDd = GetEntryDate(tDd)
        End If
        If Not (wdtpRecieptDate.Value.Date.ToString("yyyy-MM-dd") >= lotRecieptDate.Date.ToString("yyyy-MM-dd")) Then
            Dim errStr As String
            errStr = "Reciept Date Should not allow before LotDate"
            'errStr += " And Receipt Date Should not Exceed Today Date"
            MsgBox(errStr, MsgBoxStyle.Exclamation)
            wdtpRecieptDate.Focus()
            Exit Sub
        End If
        If Not tagEdit Then
            If Not wdtpRecieptDate.Value.Date.ToString("yyyy-MM-dd") <= tDd.ToString("yyyy-MM-dd") Then
                Dim errStr As String
                errStr = "Receipt Date Should not Exceed Today Date"
                MsgBox(errStr, MsgBoxStyle.Exclamation)
                wdtpRecieptDate.Focus()
                Exit Sub
            End If
        End If

        If WtxtLessWt_Wet.Enabled = True Then
            If Val(WtxtGrossWt_Wet.Text) <> 0 And Val(WtxtLessWt_Wet.Text) > Val(WtxtGrossWt_Wet.Text) Then
                'If Val(WtxtGrossWt_Wet.Text) <> 0 And Val(WtxtLessWt_Wet.Text) >= Val(WtxtGrossWt_Wet.Text) Then
                MsgBox(E0004 + Me.GetNextControl(WtxtLessWt_Wet, False).Text, MsgBoxStyle.Exclamation)
                WtxtLessWt_Wet.Focus()
                Exit Sub
            End If
        End If
        If WtxtNetWt_Wet.Enabled = True Then
            If Val(WtxtGrossWt_Wet.Text) <> 0 And Val(WtxtNetWt_Wet.Text) <= 0 And Val(WtxtGrossWt_Wet.Text) <> Val(WtxtLessWt_Wet.Text) Then
                MsgBox("Net Weight Should not Empty", MsgBoxStyle.Exclamation)
                WtxtNetWt_Wet.Focus()
                Exit Sub
            End If
            If Val(WtxtNetWt_Wet.Text) > Val(WtxtGrossWt_Wet.Text) Then
                MsgBox(Me.GetNextControl(WtxtNetWt_Wet, False).Text + E0015 + Me.GetNextControl(WtxtGrossWt_Wet, False).Text, MsgBoxStyle.Exclamation)
                WtxtNetWt_Wet.Focus()
                Exit Sub
            End If
        End If
        If Val(ObjMinValue.txtMinMcPerGram_Amt.Text) > Val(wtxtMcPerGrm_Amt.Text) Then
            MsgBox("Min Mc Per Grm Should not Exceed Max Mc Per Grm", MsgBoxStyle.Information)
            ObjMinValue.txtMinMcPerGram_Amt.Focus()
            Exit Sub
        End If
        If Val(ObjMinValue.txtMinMkCharge_Amt.Text) > Val(wtxtMkCharge_Amt.Text) Then
            MsgBox("Min Making Charge Should not Exceed Max Making Charge", MsgBoxStyle.Information)
            ObjMinValue.txtMinMkCharge_Amt.Focus()
            Exit Sub
        End If


        dtpRecieptDate.Value = wdtpRecieptDate.Value
        cmbItem_MAN.Text = wcmbItem_MAN.Text
        If cmbItem_MAN.Text <> "" Then
            cmbSubItem_Man.Items.Clear()
            If objGPack.GetSqlValue("SELECT SUBITEM FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'") = "Y" Then
                strSql = GetSubItemQry(New String() {"SUBITEMNAME"}, Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'")))
                objGPack.FillCombo(strSql, cmbSubItem_Man, False)
                cmbSubItem_Man.Enabled = True
            Else
                cmbSubItem_Man.Enabled = False
                cmbSubItem_Man.Text = ""
            End If
        End If
        cmbSubItem_Man.Text = WcmbSubItem_OWN.Text

        'txtTagNo__Man.Text = wtxtTagNo__Man.Text
        cmbItemType_MAN.Text = wcmbPurity.Text
        txtPieces_Num_Man.Text = wtxtPieces_Num_Man.Text
        txtGrossWt_Wet.Text = WtxtGrossWt_Wet.Text
        txtNetWt_Wet.Text = WtxtNetWt_Wet.Text
        txtLessWt_Wet.Text = WtxtLessWt_Wet.Text
        txtMaxMcPerGrm_Amt.Text = wtxtMcPerGrm_Amt.Text
        txtMaxMkCharge_Amt.Text = wtxtMkCharge_Amt.Text
        txtMetalRate_Amt.Text = wtxtMetalRate_Amt.Text

        dtStoneDetails = dtWStoneDetails
        dtStoneDetails.AcceptChanges()
        gridStone.DataSource = dtStoneDetails
        StyleGridStone()
        txtSalValue_Amt_Man.Text = wtxtSalValue_Amt_Man.Text

        TabGeneral.SelectedTab = tabTag
        cmbItem_MAN.Focus()
    End Sub

    Private Sub WcmbSubItem_MAN_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles WcmbSubItem_OWN.GotFocus, WcmbSubItem_OWN.GotFocus
        WListSearch(CType(sender, ComboBox))
    End Sub

    Private Sub WcmbSubItem_MAN_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles WcmbSubItem_OWN.LostFocus, WcmbSubItem_OWN.LostFocus
        If WcmbSubItem_OWN.Text <> "" Then
            Dim subdr As DataRow = GetSqlRow("SELECT ISNULL(FIXEDVA,'N'),CALTYPE,NOOFPIECE,ISNULL(TABLECODE,''),ISNULL(MCCALC,''),ISNULL(VALUECALC,'') FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & WcmbSubItem_OWN.Text & "' and ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & wcmbItem_MAN.Text & "')", cn, tran)
            If Not subdr Is Nothing Then
                If Val("" & subdr.Item(2).ToString) <> 0 Then noOfPiece = Val("" & subdr.Item(2).ToString)
                If (subdr.Item(3).ToString) <> "''" Then cmbTableCode.Text = subdr.Item(3).ToString
                _MCCALCON_ITEM_GRS = IIf(subdr.Item(4).ToString = "G", True, False)
                _VALUECALCON_ITEM_GRS = IIf(subdr.Item(5).ToString = "G", True, False)
            End If
        End If
        WHideSearch()
        WCalcLessWt()
        WCalcNetWt()
        WCalcMaxMinValues()
        WCalcSaleValue()
    End Sub

    Private Sub WcmbSubItem_MAN_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles WcmbSubItem_OWN.TextChanged, WcmbSubItem_OWN.TextChanged
        wcmbSearch.Text = CType(sender, ComboBox).Text
    End Sub

#End Region

#Region "Tag Function"

#Region "Caluculation Procedures"

    Private Sub CalcLessWt()
        funcStoneDetailsClear()
        funcDiamondDetailsClear()
        funcPreciousDetailsClear()
        If studdedStone <> "Y" Then Exit Sub
        Dim diaCaratWt As Double = 0
        Dim diaGramWt As Double = 0
        Dim diaPcs As Integer = 0
        Dim diaAmt As Double = 0

        Dim preCaratWt As Double = 0
        Dim preGramWt As Double = 0
        Dim prePcs As Integer = 0
        Dim preAmt As Double = 0

        Dim stoCaratWt As Double = 0
        Dim stoGramWt As Double = 0
        Dim stoPcs As Integer = 0
        Dim stoAmt As Double = 0

        If TabControl1.TabPages.Contains(tabStone) Then
            For cnt As Integer = 0 To gridStone.RowCount - 1
                With gridStone.Rows(cnt)
                    Select Case .Cells("METALID").Value.ToString
                        Case "D"
                            diaPcs += Val(.Cells("PCS").Value.ToString)
                            diaAmt += Val(.Cells("AMOUNT").Value.ToString)
                        Case "S"
                            stoPcs += Val(.Cells("PCS").Value.ToString)
                            stoAmt += Val(.Cells("AMOUNT").Value.ToString)
                        Case "P"
                            prePcs += Val(.Cells("PCS").Value.ToString)
                            preAmt += Val(.Cells("AMOUNT").Value.ToString)
                    End Select
                    Select Case .Cells("UNIT").Value.ToString
                        Case "G"
                            If .Cells("METALID").Value.ToString = "S" Then
                                stoGramWt += Val(.Cells("WEIGHT").Value.ToString)
                            ElseIf .Cells("METALID").Value.ToString = "P" Then
                                preGramWt += Val(.Cells("WEIGHT").Value.ToString)
                            ElseIf .Cells("METALID").Value.ToString = "D" Then
                                diaGramWt += Val(.Cells("WEIGHT").Value.ToString)
                            End If
                        Case "C"
                            If .Cells("METALID").Value.ToString = "S" Then
                                stoCaratWt += Val(.Cells("WEIGHT").Value.ToString)
                            ElseIf .Cells("METALID").Value.ToString = "P" Then
                                preCaratWt += Val(.Cells("WEIGHT").Value.ToString)
                            ElseIf .Cells("METALID").Value.ToString = "D" Then
                                diaCaratWt += Val(.Cells("WEIGHT").Value.ToString)
                            End If
                    End Select
                End With
            Next
        End If

        lblStnPcs.Text = IIf(stoPcs <> 0, stoPcs, "")
        lblStnCaratWt.Text = IIf(stoCaratWt, Format(stoCaratWt, FormatNumberStyle(DiaRnd)), "")
        lblStnGramWt.Text = IIf(stoGramWt, Format(stoGramWt, FormatNumberStyle(DiaRnd)), "")
        lblStnAmount.Text = IIf(stoAmt, Format(stoAmt, "0.00"), "")

        lblPrePcs.Text = IIf(prePcs <> 0, prePcs, "")
        lblPreCarat.Text = IIf(preCaratWt, Format(preCaratWt, "0.000"), "")
        lblPreGram.Text = IIf(preGramWt, Format(preGramWt, "0.000"), "")
        lblPreAmount.Text = IIf(preAmt, Format(preAmt, "0.00"), "")

        lblDiaPcs.Text = IIf(diaPcs <> 0, diaPcs, "")
        lblDiaCarat.Text = IIf(diaCaratWt <> 0, Format(diaCaratWt, FormatNumberStyle(DiaRnd)), "")
        lblDiaGram.Text = IIf(diaGramWt <> 0, Format(diaGramWt, FormatNumberStyle(DiaRnd)), "")
        lblDiaAmount.Text = IIf(diaAmt <> 0, Format(diaAmt, "0.00"), "")

        Dim mStudWtDedut As String
        If StudWtDedut = "I" Then
            If cmbSubItem_Man.Text <> "" Then
                strSql = " SELECT STUDDEDUCT FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')"
            Else
                strSql = " SELECT STUDDEDUCT  FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"
            End If
            mStudWtDedut = objGPack.GetSqlValue(strSql).ToString.ToUpper
        Else
            mStudWtDedut = StudWtDedut
        End If

        Dim lessWt As Double = Nothing
        If mStudWtDedut.Contains("D") Then
            lessWt += (diaCaratWt / 5) + diaGramWt
        End If
        If mStudWtDedut.Contains("S") Then
            lessWt += (stoCaratWt / 5) + stoGramWt
        End If
        If mStudWtDedut.Contains("P") Then
            lessWt += (preCaratWt / 5) + preGramWt
        End If
        If mStudWtDedut.Contains("N") Then
            lessWt = 0
        End If
        'lessWt = (diaCaratWt + stoCaratWt + preCaratWt) / 5 + (diaGramWt + stoGramWt + preGramWt)
        txtLessWt_Wet.Text = IIf(lessWt <> 0, Format(lessWt, "0.000"), "")
    End Sub

    Private Sub CalcNetWt()
        Dim wt As Double = Nothing
        wt = Val(txtGrossWt_Wet.Text) - Val(txtLessWt_Wet.Text)
        strSql = "SELECT ISNULL(NETWTPER,0) NETWTPER FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"
        Dim mnetwtper As Double = Val(objGPack.GetSqlValue(strSql).ToString)
        If mnetwtper <> 0 Then wt = wt * (mnetwtper / 100)
        txtNetWt_Wet.Text = IIf(wt <> 0, Format(wt, "0.000"), "")
    End Sub

    Private Sub CalcFinalTotal(Optional ByVal salevalueblock As Boolean = False)
        CalcLessWt()
        CalcNetWt()
        CalcStoneTotals()
        CalcMultiMetalTotal()
        CalcMiscTotalAmount()
        'Calcsubitems()

        If Not salevalueblock Then CalcSaleValue()
        'CalcPurchaseValue()
    End Sub

    Private Sub Calcsubitems()
        Dim dt As New DataTable
        strSql = " SELECT * FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            With dt.Rows(0)
                pieceRate = Val(dt.Rows(0).Item("PieceRate").ToString)
                lotPurRate = Val(dt.Rows(0).Item("PIECERATE_PUR").ToString)
                If pieceRate <> 0 Then txtRate_Amt.Text = pieceRate.ToString
                txtStyleCode.Text = "" & .Item("STYLECODE").ToString
                If .Item("PCTFILE").ToString <> "" Then
                    picPath = defalutDestination + .Item("PCTFILE").ToString
                    SubItemPic = True
                    Dim Finfo As FileInfo
                    Finfo = New FileInfo(picPath)
                    'Finfo.IsReadOnly = False
                    AutoImageSizer(picPath, picModel)
                    'Dim fStream As New FileStream(openDia.FileName, FileMode.Open)
                    'picModel.Image = Image.FromStream(fStream)
                    'fStream.Close()
                    picExtension = Finfo.Extension
                End If
            End With
        End If
    End Sub

    Private Sub CalcSaleValue()
        Dim amt As Double = Nothing
        'If calType = "F" Then Exit Sub
        If calType = "R" Then
            amt = Val(txtPieces_Num_Man.Text) * Val(txtRate_Amt.Text)
        Else
            Dim wt As Double = 0
            Dim rate As Double = IIf(calType = "M", Val(txtRate_Amt.Text), Val(txtMetalRate_Amt.Text))
            If cmbCalcMode.SelectedIndex = 0 Then ''GRS WT
                wt = Val(txtGrossWt_Wet.Text)
                If _MCONGRSNET Then
                    wt = Val(txtGrossWt_Wet.Text)
                Else
                    wt = IIf(_MCCALCON_ITEM_GRS, Val(txtGrossWt_Wet.Text), Val(txtNetWt_Wet.Text))
                End If

            Else ''NET WT
                wt = Val(txtNetWt_Wet.Text)
            End If
            If Val(txtTouch_AMT.Text) > 0 And Val(txtMaxWastage_Wet.Text) = 0 Then
                wt = (wt * Val(txtTouch_AMT.Text)) / 100
            End If
            If TabControl1.Contains(tabMultiMetal) And multiMetalCalc Then
                amt = 0
                If GetSoftValue("MULTIMETALCALC") = "N" Then
                    GoTo WegithCalc
                End If
                For Each ro As DataRow In dtMultiMetalDetails.Rows
                    If Not Val(ro!AMOUNT.ToString) > 0 Then
                        amt += (Val(ro!WEIGHT.ToString) + Val(ro!WASTAGE.ToString)) * Val(ro!RATE.ToString)
                        amt += Val(ro!MC.ToString)
                    End If
                    amt += Val(ro!AMOUNT.ToString)
                Next
                amt += Val(lblDiaAmount.Text) + Val(lblStnAmount.Text) + Val(lblPreAmount.Text) _
                + Val(txtMiscAmt.Text)
            Else
WegithCalc:
                If VALUECALC_GRNETOPT Then
                    If _VALUECALCON_ITEM_GRS Then wt = Val(txtGrossWt_Wet.Text) Else wt = Val(txtNetWt_Wet.Text)
                End If
                amt = ((wt + Val(txtMaxWastage_Wet.Text)) * rate) _
                + Val(txtMaxMkCharge_Amt.Text) _
                + Val(txtMultiAmt.Text) _
                + Val(lblDiaAmount.Text) + Val(lblStnAmount.Text) + Val(lblPreAmount.Text) _
                + Val(txtMiscAmt.Text)
                amt += IIf(calType = "B", Val(txtRate_Amt.Text), 0)
                amt += IIf(calType = "F", Val(txtRate_Amt.Text), 0)
            End If
        End If
        amt = Math.Round(amt)
        txtSalValue_Amt_Man.Text = IIf(amt <> 0, Format(SALEVALUE_ROUND(amt), "0.00"), "")
        If SALEVALUEPLUS <> 0 Then txtSalValue_Amt_Man.Text = Val(txtSalValue_Amt_Man.Text) * SALEVALUEPLUS
        ObjPurDetail.CalcPurchaseValue()
        txtPurchaseValue_Amt.Text = ObjPurDetail.txtPurPurchaseVal_Amt.Text
    End Sub

#End Region

    Private Function GetMetalRate() As Double
        Dim purityId As String = Nothing
        ''objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = " & saItemTypeId & " AND RATEGET = 'Y'", , )
        If cmbItemType_MAN.Text <> "" Then
            purityId = objGPack.GetSqlValue("SELECT PURITYID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType_MAN.Text & "' AND RATEGET = 'Y' AND SOFTMODULE = 'S'", , )
        End If
        If Not Trim(purityId).Length > 0 Then
            purityId = objGPack.GetSqlValue("SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')")
        End If
        If purityId = "" Then Return 0
        Dim metalId As String = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = " & purityId & "")

        Dim rate As Double = Nothing
        strSql = " SELECT TOP 1 SRATE FROM " & cnAdminDb & "..RATEMAST AS M"
        strSql += " WHERE RDATE = '" & dtpRecieptDate.Value & "'"
        strSql += " AND RATEGROUP = (SELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = M.RDATE)"
        strSql += " AND METALID = '" & metalId & "'"
        strSql += " AND PURITY = (SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = '" & purityId & "')"
        strSql += " ORDER BY SNO DESC"
        rate = Val(objGPack.GetSqlValue(strSql, , , tran))
        If IsDate(dtpRecieptDate.Value) Then
            Return rate
        Else
            Return 0
        End If
    End Function
    Function funcStoneDetailsClear() As Integer
        lblStnPcs.Text = ""
        lblStnCaratWt.Text = ""
        lblStnGramWt.Text = ""
        lblStnAmount.Text = ""
    End Function

    Function funcDiamondDetailsClear() As Integer
        lblDiaPcs.Text = ""
        lblDiaCarat.Text = ""
        lblDiaGram.Text = ""
        lblDiaAmount.Text = ""
    End Function

    Function funcPreciousDetailsClear() As Integer
        lblPrePcs.Text = ""
        lblPreCarat.Text = ""
        lblPreGram.Text = ""
        lblPreAmount.Text = ""
    End Function

    Function funcNew() As Integer
        txtLotNo_Num.Enabled = False
        If GetAdmindbSoftValue("SYNC-TO", , tran) <> "" Then
            If GetAdmindbSoftValue("BRANCHTAG", "Y", tran) = "N" Then
                MsgBox("Taged entry cannot allow at location", MsgBoxStyle.Information)
                txtLotNo_Num.Enabled = False
                If Me.Focused = True Then Me.Close()
                ItemLock = False
                Return 0

                Exit Function
            End If
        End If
        Ratevaluezero = False
        ObjRsUs.txtIndRs_Amt.Clear()
        ObjRsUs.txtUSDollar_Amt.Clear()
        ObjOrderTagInfo = New TagOrderInfo
        ObjPurDetail = New TagPurchaseDetailEntry
        ObjMinValue = New TagMinValues
        ObjExtraWt = New frmExtaWt
        AddHandler ObjMinValue.txtMinWastage_Per.KeyPress, AddressOf ObjMinValues_txtMinWastage_Per_KeyPress
        AddHandler ObjMinValue.txtMinWastage_Per.TextChanged, AddressOf ObjMinValues_txtMinWastage_Per_TextChanged
        AddHandler ObjMinValue.txtMinMcPerGram_Amt.KeyPress, AddressOf ObjMinValues_txtMinMcPerGram_Amt_KeyPress
        AddHandler ObjMinValue.txtMinMcPerGram_Amt.TextChanged, AddressOf ObjMinValues_txtMinMcPerGram_Amt_TextChanged
        AddHandler ObjMinValue.txtMinWastage_Wet.GotFocus, AddressOf ObjMinValue_txtMinWastage_Wet_GotFocus
        AddHandler ObjMinValue.txtMinWastage_Wet.KeyPress, AddressOf ObjMinValue_txtMinWastage_Wet_KeyPress
        AddHandler ObjMinValue.txtMinMkCharge_Amt.GotFocus, AddressOf ObjMinValue_txtMinMkCharge_Amt_GotFocus
        AddHandler ObjMinValue.txtMinMkCharge_Amt.KeyPress, AddressOf ObjMinValue_txtMinMkCharge_Amt_KeyPress
        AddHandler ObjMinValue.txtMinWastage_Wet.TextChanged, AddressOf ObjMinValue_txtMinWastage_Wet_TextChanged

        txtRate_Amt.ReadOnly = False
        ItemLock = False
        objGPack.TextClear(Me)
        isStyleCode = False
        flagPurValSet = False
        SNO = Nothing
        entryOrder = Nothing
        METALID = Nothing
        studdedStone = Nothing
        grossnetdiff = Nothing
        sizeStock = Nothing
        multiMetal = Nothing
        OthCharge = Nothing
        calType = Nothing
        noOfPiece = Nothing
        pieceRate = Nothing
        OrderRow = Nothing
        lblOrder.Visible = False
        txtOrderNo.Visible = False
        txtOrderNo.Text = ""

        SubItemPic = False
        mfromItemid = 0
        txtMaxWastage_Per.Text = ""
        txtMaxWastage_Wet.Text = ""
        txtMaxMcPerGrm_Amt.Text = ""
        txtMaxMkCharge_Amt.Text = ""
        lblItemChange.Visible = False
        If TagDlrSealGet Then txt_seal.Enabled = True Else txt_seal.Enabled = False
        Studded_Loose = ""
        dtMultiMetalDetails.Rows.Clear()
        dtStoneDetails.Rows.Clear()
        dtMiscDetails.Rows.Clear()
        With gridStoneFooter.Rows(0)
            .Cells("PCS").Value = DBNull.Value
            .Cells("WEIGHT").Value = DBNull.Value
            .Cells("AMOUNT").Value = DBNull.Value
        End With
        gridMiscFooter.Rows(0).Cells("AMOUNT").Value = DBNull.Value

        cmbSubItem_Man.Text = ""
        cmbItemSize.Text = ""
        ''CLEAR LABEL BOX VALUES
        funcStoneDetailsClear()
        funcDiamondDetailsClear()
        funcPreciousDetailsClear()
        lblLotType.Text = ""
        lblFrom.Text = ""
        lblTo.Text = ""
        lblLastTagNo.Text = ""
        lblLastTagWt.Text = ""
        ''Pieces
        lblPLot.Text = ""
        lblPCompled.Text = ""
        lblPBalance.Text = ""
        ''Weight
        lblWLot.Text = ""
        lblWCompleted.Text = ""
        lblWBalance.Text = ""
        txtMiscAmt.Text = ""
        txtMultiWt.Text = ""
        txtMultiAmt.Text = ""
        btnSave.Enabled = True
        TChkbStk = True
        AddStockEntry = True

        txtLotNo_Num.Enabled = True

        funcCostCentre()

        strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ISNULL(ACTIVE,'') <> 'N' ORDER BY DESIGNERNAME"
        objGPack.FillCombo(strSql, cmbDesigner_MAN, , False)

        If UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'ITEMCOUNTER'")) = "Y" Then
            strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER ORDER BY DISPLAYORDER,ITEMCTRNAME"
            objGPack.FillCombo(strSql, cmbCounter_MAN, , False)
            cmbCounter_MAN.Enabled = True
        Else
            cmbCounter_MAN.Enabled = False
        End If

        strSql = " SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE ORDER BY dispalyorder,NAME"
        objGPack.FillCombo(strSql, cmbItemType_MAN, , False)
        If cmbItemType_MAN.Items.Count > 0 Then cmbItemType_MAN.Enabled = True Else cmbItemType_MAN.Enabled = False

        cmbTableCode.Enabled = True

        If CalMode = "O" Then
            cmbCalcMode.Text = "GRS WT"
            cmbCalcMode.Enabled = True
        ElseIf CalMode = "G" Then
            cmbCalcMode.Text = "GRS WT"
            cmbCalcMode.Enabled = False
        ElseIf CalMode = "N" Then
            cmbCalcMode.Text = "NET WT"
            cmbCalcMode.Enabled = False
        End If
        If flagDeviceMode = True Then picCapture.Visible = True
        TabControl1.TabPages.Remove(tabMultiMetal)
        TabControl1.TabPages.Remove(tabStone)
        TabControl1.TabPages.Remove(tabOtherCharges)
        TabControl1.TabPages.Remove(tabPurchase)
        txtSalValue_Amt_Man.Clear()
        If STKAFINDATE = False Then
            dtpRecieptDate.Value = GetEntryDate(GetServerDate)
        Else
            dtpRecieptDate.MinimumDate = (New DateTimePicker).MinDate
        End If
        MLasttagno = 0
        MTagprefix = ""

        txtLotNo_Num.Select()
        Return 1
    End Function

    Function funcMultyNew() As Integer
        'picModel.Visible = False
        ObjRsUs.txtIndRs_Amt.Clear()
        ObjRsUs.txtUSDollar_Amt.Clear()
        Dim purTouch As Decimal = Nothing
        Dim purWastPer As Decimal = Nothing
        Dim purWastage As Decimal = Nothing
        Dim purMcGrm As Decimal = Nothing
        Dim purMc As Decimal = Nothing
        purTouch = Val(ObjPurDetail.txtPurTouch_Amt.Text)
        purWastPer = Val(ObjPurDetail.txtPurWastage_Per.Text)
        purWastage = Val(ObjPurDetail.txtPurWastage_Wet.Text)
        purMcGrm = Val(ObjPurDetail.txtPurMcPerGrm_Amt.Text)
        purMc = Val(ObjPurDetail.txtPurMakingChrg_Amt.Text)
        Dim salrateper As Decimal = Val(ObjPurDetail.txtSaleRate_PER.Text)
        ObjPurDetail = New TagPurchaseDetailEntry
        ObjPurDetail.txtPurTouch_Amt.Text = purTouch ' lotPurTouch
        ObjPurDetail.txtPurWastage_Per.Text = purWastPer
        ObjPurDetail.txtPurWastage_Wet.Text = purWastage
        ObjPurDetail.txtPurMcPerGrm_Amt.Text = purMcGrm ' lotPurMcPerGrm
        ObjPurDetail.txtPurMakingChrg_Amt.Text = purMc
        ObjPurDetail.txtPurRate_Amt.Text = purRate
        ObjPurDetail.txtSaleRate_PER.Text = salrateper
        ObjPurDetail.Loadd = False
        If SubItemPic = False Then
            AutoImageSizer(My.Resources.no_photo, picModel, PictureBoxSizeMode.StretchImage)
            'Me.picModel.Image = My.Resources.Resources.no_photo
            picPath = Nothing
            picExtension = Nothing
        End If
        flagPurValSet = False

        ObjExtraWt = New frmExtaWt
        Studded_Loose = ""
        dtStoneDetails.Rows.Clear()
        dtMiscDetails.Rows.Clear()
        dtMultiMetalDetails.Rows.Clear()
        With gridStoneFooter.Rows(0)
            .Cells("PCS").Value = DBNull.Value
            .Cells("WEIGHT").Value = DBNull.Value
            .Cells("AMOUNT").Value = DBNull.Value
        End With
        gridMiscFooter.Rows(0).Cells("AMOUNT").Value = DBNull.Value


        funcStoneDetailsClear()
        funcDiamondDetailsClear()
        funcPreciousDetailsClear()

        txtMiscAmt.Text = ""
        txtMultiWt.Text = ""

        ''GridMultiMetal
        txtMMWeight_Wet.Clear()
        txtMMWastagePer_PER.Clear()
        txtMMWastage_WET.Clear()
        txtMMMcPerGRm_AMT.Clear()
        txtMMMc_AMT.Clear()
        txtMMAmount_AMT.Clear()
        txtMMRate.Clear()
        txtMMTotWeight.Text = ""
        ''Grid Stone
        txtStPcs_Num.Clear()
        txtStWeight.Clear()
        txtStRate_Amt.Clear()
        txtStAmount_Amt.Clear()
        txtStTotPcs.Text = ""
        txtStTotWeight.Text = ""
        txtStTotAmount.Text = ""
        ''Grid Misc
        txtMiscAmount_Amt.Clear()
        txtMiscTotAmt.Text = ""

        txtTagNo__Man.Clear()
        txtPieces_Num_Man.Clear()
        ' txtRate_Amt.Clear()
        txtGrossWt_Wet.Clear()
        txtLessWt_Wet.Clear()
        txtNetWt_Wet.Clear()
        If CalMode = "O" Then
            cmbCalcMode.Text = "GRS WT"
            cmbCalcMode.Enabled = True
        ElseIf CalMode = "G" Then
            cmbCalcMode.Text = "GRS WT"
            cmbCalcMode.Enabled = False
        ElseIf CalMode = "N" Then
            cmbCalcMode.Text = "NET WT"
            cmbCalcMode.Enabled = False
        End If
        txtStyleCode.Clear()
        txtSalValue_Amt_Man.Clear()
        txtRefVal_AMT.Clear()
        txtRfId.Clear()
        If Not LOTNARRATION Then txtNarration.Clear()
        If flagDeviceMode = True Then picCapture.Visible = True
        'gridPurMisc.DataSource = Nothing
        'gridPurStone.DataSource = Nothing
        'gridPurMisc.DataSource = Nothing
        If TAGENTRY_FOCUS = "P" Then
            txtTagNo__Man.Focus() : Exit Function
        End If
        Me.SelectNextControl(chkBarcodePrint, True, True, True, True)
    End Function

    Function funcSetMultiMetalGridStyle() As Integer
        With gridMultimetal
            With .Columns("CATEGORY")
                .HeaderText = "CATEGORY NAME"
                .Width = 343
            End With
            With .Columns("WEIGHT")
                .HeaderText = "WEIGHT"
                .Width = 74
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("RATE")
                .HeaderText = "RATE"
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("AMOUNT")
                .HeaderText = "AMOUNT"
                .Width = 99
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
        End With
    End Function

    Function funcAdd(ByVal Witemtagsno As String) As Integer
        Dim RowFiter() As DataRow = Nothing
        Dim TagSno As String = Nothing
        Dim TagNo As String = Nothing
        Dim COSTID As String = Nothing
        Dim itemId As Integer = Nothing
        Dim subitemId As Integer = Nothing
        Dim sizeId As Integer = Nothing
        Dim itemCtrId As Integer = Nothing
        Dim designerId As Integer = Nothing
        Dim tagVal As Integer = 0
        'Dim saleMode As String = Nothing
        Dim itemTypeId As Integer = Nothing
        Dim tranInvNo As String = Nothing
        Dim supBillno As String = Nothing

        Dim stlPcs As Integer = 0
        Dim stlWt As Double = 0
        Dim stlType As String = Nothing

        Dim dialPcs As Integer = 0
        Dim dialWt As Double = 0


        'Try


        Dim XToCostid As String = GetAdmindbSoftValue("SYNC-TO", "", tran)
        Dim XBranchtag As Boolean = IIf(GetAdmindbSoftValue("BRANCHTAG", "", tran) = "Y", True, False)

        ''Find COSTID
        strSql = " SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME  = '" & cmbCostCentre_Man.Text & "'"
        COSTID = objGPack.GetSqlValue(strSql, "COSTID", , tran)

        ''Find ItemId
        strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItem_MAN.Text & "'"
        itemId = Val(objGPack.GetSqlValue(strSql, "ITEMID", , tran))

        If cmbSubItem_Man.Enabled = True Then
            ''Find SubItemId
            strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "' AND ITEMID = " & itemId & ""
            subitemId = Val(objGPack.GetSqlValue(strSql, "SUBITEMID", , tran))
        End If

        If cmbItemSize.Enabled = True Then
            ''Find ItemSize
            strSql = " SELECT SIZEID FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZENAME = '" & cmbItemSize.Text & "'"
            strSql += " AND ITEMID IN (SELECT  ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItem_MAN.Text & "')"
            sizeId = Val(objGPack.GetSqlValue(strSql, "SIZEID", , tran))
        End If

        If cmbCounter_MAN.Enabled = True Then
            ''Find ItemCounter
            strSql = " SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbCounter_MAN.Text & "'"
            itemCtrId = Val(objGPack.GetSqlValue(strSql, "ITEMCTRID", , tran))
        End If

        ''Find DesignerId
        strSql = " SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesigner_MAN.Text & "'"
        designerId = Val(objGPack.GetSqlValue(strSql, "DESIGNERID", "", tran))
        ''Find itemTypeId
        strSql = " SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType_MAN.Text & "'"
        itemTypeId = Val(objGPack.GetSqlValue(strSql, "ITEMTYPEID", , tran))

        ''FIND TRANINVNO AND SUPBILLNO
        strSql = " SELECT TRANINVNO,BILLNO FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = '" & SNO & "'"
        tranInvNo = objGPack.GetSqlValue(strSql, "TRANINVNO", , tran)
        supBillno = objGPack.GetSqlValue(strSql, "BILLNO", , tran)

        Dim mlwmctype As String = objGPack.GetSqlValue("SELECT WMCTYPE FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = '" & SNO & "'", , , tran)
        'Dim randomuniqtagno = GetAdmindbSoftValue("RANDOM_UNIQUE_TAG", "N").ToString
        'tran = Nothing
        'tran = cn.BeginTransaction()
        ''Find TagSno
GETNTAGSNO:
        'TagSno = GetNewSno(TranSnoType.ITEMTAGCODE, tran, "GET_TRANSNO_ADMIN")
        TagSno = GetNewSno(TranSnoType.ITEMTAGCODE, tran, "GET_ADMINSNO_TRAN")
        'TagDupGen:

        '            If Mid(randomuniqtagno, 1, 1) = "Y" Then
        '                txtTagNo__Man.Text = objTag.GetTagNo(dtpRecieptDate.Value.ToString("yyyy-MM-dd"), cmbItem_MAN.Text, SNO, tran)
        '                GoTo Continuetagging
        '            End If

        '            If GetSoftValue("TAGNOFROM") = "I" Then
        '                strSql = "SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAG"
        '                strSql += " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "') "
        '                strSql += " AND TAGNO = '" & txtTagNo__Man.Text & "'"
        '                If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
        '                    txtTagNo__Man.Text = objTag.GetTagNo(dtpRecieptDate.Value.ToString("yyyy-MM-dd"), cmbItem_MAN.Text, SNO, tran)
        '                    GoTo TagDupGen
        '                End If
        '            ElseIf GetSoftValue("TAGNOFROM") = "U" Then
        '                txtTagNo__Man.Text = objTag.GetTagNo(dtpRecieptDate.Value.ToString("yyyy-MM-dd"), cmbItem_MAN.Text, SNO, tran)
        '            End If
        'Continuetagging:
        TagNo = txtTagNo__Man.Text
        ''Find Stone & Diamond
        If TabControl1.TabPages.Contains(tabStone) = True Then
            stlPcs += Val(lblStnPcs.Text)
            stlWt += (Val(lblStnCaratWt.Text) / 5) + Val(lblStnGramWt.Text)

            stlPcs += Val(lblPrePcs.Text)
            stlWt += (Val(lblPreCarat.Text) / 5) + Val(lblPreGram.Text)

            dialPcs += Val(lblDiaPcs.Text)
            dialWt += (Val(lblDiaCarat.Text) / 5) + Val(lblDiaGram.Text)

            stlType = "G"
            stlWt = Math.Round(stlWt, 3)
            dialWt = Math.Round(dialWt, 3)
        Else
            stlPcs = 0
            stlWt = 0
            stlType = "G"
        End If



        ''Find TagVal
        tagVal = objTag.GetTagVal(TagNo)



        Dim purStoneValue As Double
        For Each roStn As DataRow In dtStoneDetails.Rows
            purStoneValue += Val(roStn!PURVALUE.ToString)
        Next

        Dim orderRepNo As String = Nothing
        Dim orderRepSno As String = Nothing
        If Not OrderRow Is Nothing Then
            orderRepNo = OrderRow.Item("ORNO").ToString
            orderRepSno = OrderRow.Item("SNO").ToString

        ElseIf _CheckOrderInfo = False And ObjOrderTagInfo.txtOrderNo.Text <> "" Then
            Dim OrdBatchno As String = GetNewBatchno(cnCostId, dtpRecieptDate.Value, tran)
            orderRepNo = GetCostId(COSTID) & GetCompanyId(GetStockCompId()) & ObjOrderTagInfo.txtOrderNo.Text
            orderRepSno = GetNewSno(TranSnoType.ORMASTCODE, tran, "GET_ADMINSNO_TRAN")

            ''Auto Generation in Ormast
            Dim DtOrderMast As New DataTable
            DtOrderMast = BrighttechPack.GlobalMethods.GetTableStructure(cnAdminDb, "ORMAST", cn, tran)
            Dim RowOrder As DataRow = DtOrderMast.NewRow
            RowOrder.Item("SNO") = orderRepSno
            RowOrder.Item("ORNO") = orderRepNo
            RowOrder.Item("ORDATE") = dtpRecieptDate.Value.ToString("yyyy-MM-dd")
            RowOrder.Item("REMDATE") = dtpRecieptDate.Value.ToString("yyyy-MM-dd")
            RowOrder.Item("DUEDATE") = dtpRecieptDate.Value.ToString("yyyy-MM-dd")
            RowOrder.Item("ORTYPE") = "O"
            RowOrder.Item("COMPANYID") = GetStockCompId()
            RowOrder.Item("ORRATE") = "C"
            RowOrder.Item("ORMODE") = "C"
            RowOrder.Item("ITEMID") = itemId
            RowOrder.Item("SUBITEMID") = subitemId
            RowOrder.Item("DESCRIPT") = cmbItem_MAN.Text
            RowOrder.Item("PCS") = Val(txtPieces_Num_Man.Text)
            RowOrder.Item("GRSWT") = Val(txtGrossWt_Wet.Text)
            'RowOrder.Item("EXTRAWT") = Val(ObjExtraWt.txtExtraWt_WET.Text)
            RowOrder.Item("NETWT") = Val(txtNetWt_Wet.Text)
            RowOrder.Item("SIZEID") = sizeId
            RowOrder.Item("RATE") = IIf(Val(txtRate_Amt.Text) <> 0, Val(txtRate_Amt.Text), Val(txtMetalRate_Amt.Text))
            RowOrder.Item("NATURE") = DBNull.Value
            RowOrder.Item("MCGRM") = Val(txtMaxMcPerGrm_Amt.Text)
            RowOrder.Item("MC") = Val(txtMaxMkCharge_Amt.Text)
            RowOrder.Item("WASTPER") = Val(txtMaxWastage_Per.Text)
            RowOrder.Item("WAST") = Val(txtMaxWastage_Wet.Text)
            RowOrder.Item("COMMPER") = DBNull.Value
            RowOrder.Item("COMM") = DBNull.Value
            RowOrder.Item("OTHERAMT") = DBNull.Value
            RowOrder.Item("CANCEL") = DBNull.Value
            RowOrder.Item("ORVALUE") = Val(txtSalValue_Amt_Man.Text)
            RowOrder.Item("COSTID") = COSTID
            RowOrder.Item("BATCHNO") = OrdBatchno
            RowOrder.Item("CORNO") = DBNull.Value
            RowOrder.Item("PROPSMITH") = DBNull.Value
            RowOrder.Item("PICTFILE") = DBNull.Value

            RowOrder.Item("EMPID") = Val(ObjOrderTagInfo.txtEmpNo_NUM.Text)
            RowOrder.Item("UPDATED") = Today.Date.Now.ToString("yyyy-MM-dd")
            RowOrder.Item("UPTIME") = Date.Now.ToLongTimeString
            RowOrder.Item("USERID") = userId
            RowOrder.Item("ODBATCHNO") = DBNull.Value
            RowOrder.Item("ODSNO") = DBNull.Value
            RowOrder.Item("APPVER") = VERSION
            RowOrder.Item("REASON") = DBNull.Value
            RowOrder.Item("TAX") = DBNull.Value
            RowOrder.Item("SC") = DBNull.Value
            RowOrder.Item("ADSC") = DBNull.Value
            RowOrder.Item("ITEMTYPEID") = itemTypeId
            RowOrder.Item("SIZENO") = DBNull.Value
            RowOrder.Item("TRANSFERED") = DBNull.Value
            RowOrder.Item("STYLENO") = DBNull.Value
            RowOrder.Item("DISCOUNT") = DBNull.Value
            RowOrder.Item("ORDCANCEL") = DBNull.Value
            DtOrderMast.Rows.Add(RowOrder)
            InsertData(SyncMode.Stock, DtOrderMast, cn, tran, COSTID)

            Dim DtOrIrDetail As New DataTable
            DtOrIrDetail = BrighttechPack.GlobalMethods.GetTableStructure(cnAdminDb, "ORIRDETAIL", cn, tran)

            strSql = " SELECT 1 CNT FROM " & cnAdminDb & "..ORIRDETAIL WHERE ISNULL(ORNO,'') = '" & orderRepNo & "' AND ISNULL(ORSNO,'') = '" & orderRepSno & "' AND ISNULL(ORSTATUS,'') = 'I' AND ISNULL(CANCEL,'') = ''"
            Dim dtChk As New DataTable
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(dtChk)
            If dtChk.Rows.Count = 0 Then
                RowOrder = Nothing
                RowOrder = DtOrIrDetail.NewRow
                ''INSERTING IRDETAIL
                RowOrder.Item("SNO") = GetNewSno(TranSnoType.ORIRDETAILCODE, tran, "GET_ADMINSNO_TRAN")
                RowOrder.Item("ORSNO") = orderRepSno
                RowOrder.Item("TRANNO") = GetBillNoValue("ORDIRBILLNO", tran)
                RowOrder.Item("TRANDATE") = dtpRecieptDate.Value
                RowOrder.Item("DESIGNERID") = designerId
                RowOrder.Item("PCS") = Val(txtPieces_Num_Man.Text)
                RowOrder.Item("GRSWT") = Val(txtGrossWt_Wet.Text)
                RowOrder.Item("NETWT") = Val(txtNetWt_Wet.Text)
                RowOrder.Item("WASTAGE") = Val(txtMaxWastage_Wet.Text)
                RowOrder.Item("MC") = Val(txtMaxMkCharge_Amt.Text)
                RowOrder.Item("TAGNO") = txtTagNo__Man.Text
                RowOrder.Item("ORSTATUS") = "I"
                RowOrder.Item("CANCEL") = DBNull.Value
                RowOrder.Item("COSTID") = COSTID
                RowOrder.Item("DESCRIPT") = cmbItem_MAN.Text
                RowOrder.Item("ORNO") = orderRepNo
                RowOrder.Item("BATCHNO") = OrdBatchno
                RowOrder.Item("USERID") = userId
                RowOrder.Item("UPDATED") = Today.Date.Now.ToString("yyyy-MM-dd")
                RowOrder.Item("UPTIME") = Date.Now.ToLongTimeString
                RowOrder.Item("APPVER") = VERSION
                RowOrder.Item("COMPANYID") = GetStockCompId()
                RowOrder.Item("TRANSFERED") = DBNull.Value
                DtOrIrDetail.Rows.Add(RowOrder)
                InsertData(SyncMode.Stock, DtOrIrDetail, cn, tran, COSTID)
            End If

            RowOrder = Nothing
            If DtOrIrDetail.Rows.Count > 0 Then DtOrIrDetail.Rows.Clear()
            RowOrder = DtOrIrDetail.NewRow
            ''INSERTING IRDETAIL
            RowOrder.Item("SNO") = GetNewSno(TranSnoType.ORIRDETAILCODE, tran, "GET_ADMINSNO_TRAN")
            RowOrder.Item("ORSNO") = orderRepSno
            RowOrder.Item("TRANNO") = GetBillNoValue("ORDIRBILLNO", tran)
            RowOrder.Item("TRANDATE") = dtpRecieptDate.Value
            RowOrder.Item("DESIGNERID") = designerId
            RowOrder.Item("PCS") = Val(txtPieces_Num_Man.Text)
            RowOrder.Item("GRSWT") = Val(txtGrossWt_Wet.Text)
            RowOrder.Item("NETWT") = Val(txtNetWt_Wet.Text)
            RowOrder.Item("WASTAGE") = Val(txtMaxWastage_Wet.Text)
            RowOrder.Item("MC") = Val(txtMaxMkCharge_Amt.Text)
            RowOrder.Item("TAGNO") = txtTagNo__Man.Text
            RowOrder.Item("ORSTATUS") = "R"
            RowOrder.Item("CANCEL") = DBNull.Value
            RowOrder.Item("COSTID") = COSTID
            RowOrder.Item("DESCRIPT") = cmbItem_MAN.Text
            RowOrder.Item("ORNO") = orderRepNo
            RowOrder.Item("BATCHNO") = OrdBatchno
            RowOrder.Item("USERID") = userId
            RowOrder.Item("UPDATED") = Today.Date.Now.ToString("yyyy-MM-dd")
            RowOrder.Item("UPTIME") = Date.Now.ToLongTimeString
            RowOrder.Item("APPVER") = VERSION
            RowOrder.Item("COMPANYID") = GetStockCompId()
            RowOrder.Item("TRANSFERED") = DBNull.Value
            DtOrIrDetail.Rows.Add(RowOrder)
            InsertData(SyncMode.Stock, DtOrIrDetail, cn, tran, COSTID)
        End If



        ''INSERTING ITEMTAG
        strSql = " INSERT INTO " & cnAdminDb & "..ITEMTAG"
        strSql += " ("
        strSql += " SNO,RECDATE,COSTID,ITEMID,ORDREPNO,ORSNO,"
        strSql += " ORDSALMANCODE,SUBITEMID,SIZEID,ITEMCTRID,TABLECODE,DESIGNERID,"
        strSql += " TAGNO,PCS,GRSWT,"
        strSql += " LESSWT,NETWT,RATE,FINERATE,MAXWASTPER,MAXMCGRM,"
        strSql += " MAXWAST,MAXMC,MINWASTPER,MINMCGRM,MINWAST,MINMC,"
        strSql += " TAGKEY,TAGVAL,LOTSNO,COMPANYID,SALVALUE,PURITY,NARRATION,DESCRIP,"
        strSql += " REASON,ENTRYMODE,GRSNET,"
        strSql += " ISSDATE,ISSREFNO,ISSPCS,ISSWT,FROMFLAG,TOFLAG,APPROVAL,SALEMODE,"
        strSql += " BATCHNO,MARK,"
        strSql += " PCTPATH,PCTFILE,OLDTAGNO,ITEMTYPEID,ACTUALRECDATE,WEIGHTUNIT,"
        strSql += " TRANSFERWT,CHKDATE,CHKTRAY,CARRYFLAG,BRANDID,PRNFLAG,"
        strSql += " MCDISCPER,WASTDISCPER,RESDATE,TRANINVNO,SUPBILLNO,WORKDAYS,"
        strSql += " USERID,UPDATED,UPTIME,SYSTEMID,STYLENO,APPVER,TRANSFERDATE,"
        strSql += " BOARDRATE,RFID,TOUCH,HM_BILLNO,HM_CENTER,ADD_VA_PER,REFVALUE,VALUEADDEDTYPE,TCOSTID,EXTRAWT,"
        strSql += " USRATE,INDRS,"
        strSql += " RECSNO,FROMITEMID"
        If _FourCMaintain Then
            strSql += " ,CUTID,COLORID,CLARITYID,SHAPEID,SETTYPEID,HEIGHT,WIDTH"
        End If
        strSql += " ) VALUES("
        strSql += " '" & TagSno & "'" 'SNO
        strSql += " ,'" & GetEntryDate(dtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE 'dtpRecieptDate.Value.Date.ToString("yyyy-MM-dd")
        strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
        strSql += " ," & itemId & "" 'ITEMID
        strSql += " ,'" & orderRepNo & "'" 'ORDREPNO
        strSql += " ,'" & orderRepSno & "'" 'ORsno
        strSql += " ,''" 'ORDSALMANCODE
        strSql += " ," & subitemId & "" 'SUBITEMID
        strSql += " ," & sizeId & "" 'SIZEID
        strSql += " ," & itemCtrId & "" 'ITEMCTRID
        strSql += " ,'" & cmbTableCode.Text & "'"
        'If cmbTableCode.Enabled = True Then 'TABLECODE

        'Else
        '    strSql += " ,''"
        'End If
        strSql += " ," & Val(designerId) & "" 'DESIGNERID
        strSql += " ,'" & TagNo & "'" 'TAGNO
        strSql += " ," & Val(txtPieces_Num_Man.Text) & "" 'PCS
        strSql += " ," & Val(txtGrossWt_Wet.Text) & "" 'GRSWT
        strSql += " ," & Val(txtLessWt_Wet.Text) & "" 'LESSWT
        strSql += " ," & Val(txtNetWt_Wet.Text) & "" 'NETWT
        If txtRate_Amt.Enabled = True Then
            strSql += " ," & Val(txtRate_Amt.Text) & "" 'RATE
        Else
            strSql += ",0"
        End If
        strSql += ",0" 'FINERATE
        strSql += " ," & Val(txtMaxWastage_Per.Text) & "" 'MAXWASTPER
        strSql += " ," & Val(txtMaxMcPerGrm_Amt.Text) & "" 'MAXMCGRM
        strSql += " ," & Val(txtMaxWastage_Wet.Text) & "" 'MAXWAST
        strSql += " ," & Val(txtMaxMkCharge_Amt.Text) & "" 'MAXMC
        strSql += " ," & Val(ObjMinValue.txtMinWastage_Per.Text) & "" 'MINWASTPER
        strSql += " ," & Val(ObjMinValue.txtMinMcPerGram_Amt.Text) & "" 'MINMCGRM
        strSql += " ," & Val(ObjMinValue.txtMinWastage_Wet.Text) & "" 'MINWAST
        strSql += " ," & Val(ObjMinValue.txtMinMkCharge_Amt.Text) & "" 'MINMC
        strSql += " ,'" & itemId.ToString & "" & TagNo & "'" 'TAGKEY
        strSql += " ," & tagVal & "" 'TAGVAL
        strSql += " ,'" & SNO & "'" 'LOTSNO
        strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
        strSql += " ," & Val(txtSalValue_Amt_Man.Text) & "" 'SALVALUE
        strSql += " ," & Val(txtPurity_Per.Text) & "" 'PURITY
        strSql += " ,'" & txtNarration.Text & "'" 'NARRATION
        If cmbSubItem_Man.Enabled = True Then 'DESCRIP
            strSql += " ,'" & cmbSubItem_Man.Text & "'"
        Else
            strSql += " ,'" & cmbItem_MAN.Text & "'"
        End If
        strSql += " ,''" 'REASON
        If chkAutomaticWt.Checked = True Then 'ENTRYMODE
            strSql += " ,'A'"
        Else
            strSql += " ,'M'"
        End If
        If cmbCalcMode.Enabled = True Then
            strSql += " ,'" & Mid(cmbCalcMode.Text, 1, 1) & "'" 'GRSNET
        Else
            strSql += " ,'" & Mid(cmbCalcMode.Text, 1, 1) & "'" 'GRSNET
            'strSql += " ,''" 'GRSNET
        End If
        strSql += " ,NULL" 'ISSDATE
        strSql += " ,0" 'ISSREFNO
        strSql += " ,0" 'ISSPCS
        strSql += " ,0" 'ISSWT
        strSql += " ,''" 'FROMFLAG
        strSql += " ,''" 'TOFLAG
        strSql += " ,''" 'APPROVAL
        strSql += " ,'" & calType & "'" 'SALEMODE
        strSql += " ,''" 'BATCHNO
        strSql += " ,0" 'MARK

        If picPath <> Nothing Then
            strSql += " ,'" & defalutDestination & "'"
            If SubItemPic = True Then
                strSql += " ,'" & picPath.Replace(defalutDestination, "") & "'" 'pctfile
            Else
                strSql += " ,'" & "L" + txtLotNo_Num.Text + "I" + itemId.ToString + "T" + TagNo.Replace(":", "") + IIf(picExtension.ToString.StartsWith("."), picExtension.ToString, "." + picExtension.ToString) & "'" 'pctfile
            End If
        Else
            strSql += " ,'',''" ' pctfile
        End If
        'strSql += " ,'" & IIf(picPath <> Nothing, "L" + txtLotNo_Num_Man.Text + "I" + itemId.ToString + "T" + TagNo.Replace(":", "") + "." + picExtension.ToString, picPath) & "'" 'PCTFILE
        strSql += " ,''" 'OLDTAGNO
        strSql += " ," & Val(itemTypeId) & "" 'ITEMTYPEID
        strSql += " ,'" & GetEntryDate(dtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'ACTUALRECDATE
        strSql += " ,''" 'WEIGHTUNIT
        strSql += " ,0" 'TRANSFERWT
        strSql += " ,NULL" 'CHKDATE
        strSql += " ,''" 'CHKTRAY
        strSql += " ,''" 'CARRYFLAG
        strSql += " ,''" 'BRANDID
        strSql += " ,''" 'PRNFLAG
        strSql += " ,0" 'MCDISCPER
        strSql += " ,0" 'WASTDISCPER
        strSql += " ,NULL" 'RESDATE
        strSql += " ,'" & tranInvNo & "'" 'TRANINVNO
        strSql += " ,'" & supBillno & "'" 'SUPBILLNO
        strSql += " ,''" 'WORKDAYS
        strSql += " ," & userId & "" 'USERID
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & systemId & "'" 'SYSTEMID
        strSql += " ,'" & txtStyleCode.Text & "'" 'STYLENO
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,'" & GetEntryDate(dtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'TRANSFERDATE
        strSql += " ," & Val(txtMetalRate_Amt.Text) & ""
        strSql += " ,'" & txtRfId.Text & "'"
        strSql += " ," & Val(txtTouch_AMT.Text) & ""
        strSql += " ,'" & txtHmBillNo.Text & "'" 'HM_BILLNO
        strSql += " ,'" & txtHmCentre.Text & "'" 'HM_CENTER
        strSql += " ," & Val(ObjPurDetail.txtpURFixedValueVa_AMT.Text) & "" 'ADD_VA_PER
        strSql += " ," & Val(txtRefVal_AMT.Text) & "" 'REFVALUE
        strSql += " ,'" & mlwmctype & "'"
        strSql += " ,'" & COSTID & "'" 'TCOSTID
        strSql += " ,'" & Val(ObjExtraWt.txtExtraWt_WET.Text) & "'" 'EXTRAWT
        strSql += " ," & IIf(NEEDUS = True And Studded_Loose = "L", Val(ObjRsUs.txtUSDollar_Amt.Text), 0) & ""
        strSql += " ," & IIf(NEEDUS = True And Studded_Loose = "L", Val(ObjRsUs.txtIndRs_Amt.Text), 0) & ""
        'strSql += " ,'" & Recsno & "'" 'RECSNO
        strSql += " ,'" & WitemtagSno & "'" 'RECSNO
        strSql += " ," & mfromItemid & "" 'EXTRAWT
        If _FourCMaintain Then
            strSql += " ," & TagCutId & "" 'CUTID
            strSql += " ," & TagColorId & "" 'COLORID
            strSql += " ," & TagClarityId & "" 'CLARITYID
            strSql += " ," & TagShapeId & "" 'SHAPEID
            strSql += " ," & TagSetTypeId & "" 'SETTYPEID
            strSql += " ," & TagHeight & "" 'HEIGHT
            strSql += " ," & TagWidth & "" 'WIDTH
        End If
        strSql += " )"
        If File.Exists(picPath) = True And SubItemPic = False Then
            Dim serverPath As String = Nothing
            Dim fileDestPath As String = (defalutDestination + "L" + txtLotNo_Num.Text + "I" + itemId.ToString + "T" + TagNo.Replace(":", "") + IIf(picExtension.ToString.StartsWith("."), picExtension.ToString, "." + picExtension.ToString))

            Dim Finfo As FileInfo
            Finfo = New FileInfo(fileDestPath)
            If IO.Directory.Exists(Finfo.Directory.FullName) = False Then
                MsgBox(Finfo.Directory.FullName & " Does not exist. Please make a corresponding path", MsgBoxStyle.Information)
                tran.Rollback()
                Exit Function
            End If
            If UCase(picPath) <> fileDestPath.ToUpper Then
                Dim cFile As New FileInfo(picPath)
                cFile.CopyTo(fileDestPath, True)
            End If
        End If

        If XToCostid <> "" And XBranchtag Then
            ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid, , IIf(picPath <> Nothing, (defalutDestination + "L" + txtLotNo_Num.Text + "I" + itemId.ToString + "T" + txtTagNo__Man.Text.Replace(":", "") + "." + picExtension), Nothing), "TITEMTAG", , True)
        Else
            cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
        End If
        TagCutId = 0 : TagColorId = 0 : TagClarityId = 0 : TagShapeId = 0 : TagSetTypeId = 0 : TagHeight = 0 : TagWidth = 0
        'ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID, , IIf(picPath <> Nothing, (defalutDestination + "L" + txtLotNo_Num_Man.Text + "I" + itemId.ToString + "T" + txtTagNo__Man.Text.Replace(":", "") + "." + picExtension), Nothing), "TITEMTAG", , False)

        If _HasPurchase Or pFixedVa Or PUR_AUTOCALC Then
            ''ITEM PUR DETAIL
            strSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..PURITEMTAG"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " TAGSNO"
            strSql += vbCrLf + " ,ITEMID"
            strSql += vbCrLf + " ,TAGNO"
            strSql += vbCrLf + " ,PURLESSWT"
            strSql += vbCrLf + " ,PURNETWT"
            strSql += vbCrLf + " ,PURRATE"
            strSql += vbCrLf + " ,PURGRSNET"
            strSql += vbCrLf + " ,PURWASTAGE"
            strSql += vbCrLf + " ,PURTOUCH"
            strSql += vbCrLf + " ,PURMC"
            strSql += vbCrLf + " ,PURVALUE"
            strSql += vbCrLf + " ,PURTAX"
            strSql += vbCrLf + " ,RECDATE"
            strSql += vbCrLf + " ,COMPANYID,COSTID"
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " VALUES"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " '" & TagSno & "'" 'TAGSNO
            strSql += vbCrLf + " ," & itemId & "" 'ITEMID
            strSql += vbCrLf + " ,'" & TagNo & "'" 'TAGNO
            strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurLessWt_Wet.Text) & "" ' PURLESSWT
            strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurNetWt_Wet.Text) & "" ' PURNETWT"
            strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurRate_Amt.Text) & "" ' PURRATE"
            strSql += vbCrLf + " ,'" & Mid(ObjPurDetail.cmbPurCalMode.Text, 1, 1) & "'" ' PURGRSNET"
            strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurWastage_Wet.Text) & "" ' PURWASTAGE"
            strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurTouch_Amt.Text) & "" ' PURTOUCH"
            strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurMakingChrg_Amt.Text) & "" ' PURMC"
            strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurPurchaseVal_Amt.Text) & "" ' PURVALUE"
            strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurTax_AMT.Text) & ""
            strSql += vbCrLf + " ,'" & ObjPurDetail.dtpPurchaseDate.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,'" & GetStockCompId() & "'"
            strSql += vbCrLf + " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'"
            strSql += vbCrLf + " )"
            If XToCostid <> "" And XBranchtag Then
                ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid, , , "TPURITEMTAG", , True)
            Else
                cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
            End If
            'ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID, , , "TPURITEMTAG", , False)
        End If
        ''Inserting StoneDetail
        For cnt As Integer = 0 To dtStoneDetails.Rows.Count - 1
            With dtStoneDetails.Rows(cnt)
                Dim CutId As Integer = 0
                Dim ColorId As Integer = 0
                Dim ClarityId As Integer = 0
                Dim ShapeId As Integer = 0
                Dim SetTypeId As Integer = 0
                Dim stnItemId As Integer = 0
                Dim stnSubItemId As Integer = 0
                Dim stnSno As String = GetNewSno(TranSnoType.ITEMTAGSTONECODE, tran, "GET_ADMINSNO_TRAN")
                'Dim caType As String = Nothing
                strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Item("ITEM").ToString & "'"
                stnItemId = Val(objGPack.GetSqlValue(strSql, "ITEMID", , tran))
                strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & .Item("SUBITEM").ToString & "' AND ITEMID = " & stnItemId & ""
                stnSubItemId = Val(objGPack.GetSqlValue(strSql, , , tran))
                .Item("STNSNO") = stnSno
                ''Inserting itemTagStone
                strSql = " INSERT INTO " & cnAdminDb & "..ITEMTAGSTONE("
                strSql += " SNO,TAGSNO,ITEMID,COMPANYID,STNITEMID,"
                strSql += " STNSUBITEMID,TAGNO,STNPCS,STNWT,"
                strSql += " STNRATE,STNAMT,DESCRIP,"
                strSql += " RECDATE,CALCMODE,"
                strSql += " MINRATE,SIZECODE,STONEUNIT,ISSDATE,"
                strSql += " OLDTAGNO,CARRYFLAG,COSTID,SYSTEMID,APPVER,"
                strSql += " USRATE,INDRS,PACKETNO"
                If _FourCMaintain Then
                    strSql += " ,CUTID,COLORID,CLARITYID,SHAPEID,SETTYPEID,HEIGHT,WIDTH"
                    CutId = Val(objGPack.GetSqlValue(" SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & .Item("CUT").ToString & "'", "CUTID", , tran))
                    ColorId = Val(objGPack.GetSqlValue(" SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME = '" & .Item("COLOR").ToString & "'", "COLORID", , tran))
                    ClarityId = Val(objGPack.GetSqlValue(" SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & .Item("CLARITY").ToString & "'", "CLARITYID", , tran))
                    ShapeId = Val(objGPack.GetSqlValue(" SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & .Item("SHAPE").ToString & "'", "SHAPEID", , tran))
                    SetTypeId = Val(objGPack.GetSqlValue(" SELECT SETTYPEID FROM " & cnAdminDb & "..STNSETTYPE WHERE SETTYPENAME = '" & .Item("SETTYPE").ToString & "'", "SETTYPEID", , tran))
                End If
                strSql += " )VALUES("
                strSql += " '" & stnSno & "'" ''SNO
                strSql += " ,'" & TagSno & "'" 'TAGSNO
                strSql += " ,'" & itemId & "'" 'ITEMID
                strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                strSql += " ," & stnItemId & "" 'STNITEMID
                strSql += " ," & stnSubItemId & "" 'STNSUBITEMID
                strSql += " ,'" & TagNo & "'" 'TAGNO
                strSql += " ," & Val(.Item("PCS").ToString) & "" 'STNPCS
                strSql += " ," & Val(.Item("WEIGHT").ToString) & "" 'STNWT
                strSql += " ," & Val(.Item("RATE").ToString) & "" 'STNRATE
                strSql += " ," & Val(.Item("AMOUNT").ToString) & "" 'STNAMT
                If stnSubItemId <> 0 Then 'DESCRIP
                    strSql += " ,'" & .Item("SUBITEM").ToString & "'"
                Else
                    strSql += " ,'" & .Item("ITEM").ToString & "'"
                End If
                strSql += " ,'" & GetEntryDate(dtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                strSql += " ,'" & .Item("CALC").ToString & "'" 'CALCMODE
                strSql += " ,0" 'MINRATE
                strSql += " ,0" 'SIZECODE
                strSql += " ,'" & .Item("UNIT").ToString & "'" 'STONEUNIT
                strSql += " ,NULL" 'ISSDATE
                strSql += " ,''" 'OLDTAGNO
                strSql += " ,''" 'CARRYFLAG
                strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                strSql += " ,'" & systemId & "'" 'SYSTEMID
                strSql += " ,'" & VERSION & "'" 'APPVER
                strSql += " ," & Val(.Item("USRATE").ToString) & "" 'USRATE
                strSql += " ," & Val(.Item("INDRS").ToString) & "" 'INDRS
                strSql += " ,'" & .Item("PACKETNO").ToString & "'" 'PACKETNO
                If _FourCMaintain Then
                    strSql += " ," & CutId & "" 'CUTID
                    strSql += " ," & ColorId & "" 'COLORID
                    strSql += " ,'" & ClarityId & "'" 'CLARITYID
                    strSql += " ,'" & ShapeId & "'" 'SHAPEID
                    strSql += " ,'" & SetTypeId & "'" 'SETTYPEID
                    strSql += " ,'" & Val(.Item("HEIGHT").ToString) & "'" 'HEIGHT
                    strSql += " ,'" & Val(.Item("WIDTH").ToString) & "'" 'WIDTH
                End If
                strSql += " )"
                If XToCostid <> "" And XBranchtag Then
                    ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid, , , "TITEMTAGSTONE", , True)
                Else
                    cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                End If
                'ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID, , , "TITEMTAGSTONE", , False)
            End With
        Next
        If _HasPurchase Or pFixedVa Or PUR_AUTOCALC Then
            For Each RoPur As DataRow In ObjPurDetail.dtGridStone.Rows
                If RoPur.Item("KEYNO").ToString = "" Then Continue For
                For Each Ro_S As DataRow In dtStoneDetails.Rows
                    If Ro_S.Item("KEYNO") = RoPur.Item("KEYNO") Then
                        RoPur.Item("STNSNO") = Ro_S.Item("STNSNO")
                        Exit For
                    End If
                Next
            Next

            Dim stnItemId As Integer = 0
            Dim stnSubItemId As Integer = 0
            For Each RoPur As DataRow In ObjPurDetail.dtGridStone.Rows
                'Dim caType As String = Nothing
                strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & RoPur.Item("ITEM").ToString & "'"
                stnItemId = Val(objGPack.GetSqlValue(strSql, "ITEMID", , tran))
                strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & RoPur.Item("SUBITEM").ToString & "' AND ITEMID = " & stnItemId & ""
                stnSubItemId = Val(objGPack.GetSqlValue(strSql, , , tran))
                strSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..PURITEMTAGSTONE"
                strSql += vbCrLf + " ("
                strSql += vbCrLf + " TAGSNO"
                strSql += vbCrLf + " ,ITEMID"
                strSql += vbCrLf + " ,TAGNO"
                strSql += vbCrLf + " ,STNITEMID,STNSUBITEMID,STNPCS,STNWT,STNRATE,STNAMT,STONEUNIT,CALCMODE"
                strSql += vbCrLf + " ,PURRATE"
                strSql += vbCrLf + " ,PURAMT,COMPANYID,COSTID"
                strSql += vbCrLf + " ,STNSNO"
                strSql += vbCrLf + " )"
                strSql += vbCrLf + " VALUES"
                strSql += vbCrLf + " ("
                strSql += vbCrLf + " '" & TagSno & "'" 'TAGSNO
                strSql += vbCrLf + " ," & itemId & "" 'ITEMID
                strSql += vbCrLf + " ,'" & TagNo & "'" 'TAGNO
                strSql += " ," & stnItemId & "" 'STNITEMID
                strSql += " ," & stnSubItemId & "" 'STNSUBITEMID
                strSql += " ," & Val(RoPur.Item("PCS").ToString) & "" 'STNPCS
                strSql += " ," & Val(RoPur.Item("WEIGHT").ToString) & "" 'STNWT
                strSql += " ," & Val(RoPur.Item("RATE").ToString) & "" 'STNRATE
                strSql += " ," & Val(RoPur.Item("AMOUNT").ToString) & "" 'STNAMT
                strSql += " ,'" & RoPur.Item("UNIT").ToString & "'" 'STONEUNIT
                strSql += " ,'" & RoPur.Item("CALC").ToString & "'" 'CALCMODE
                strSql += vbCrLf + " ," & Val(RoPur.Item("PURRATE").ToString) & "" 'PURRATE
                strSql += vbCrLf + " ," & Val(RoPur.Item("PURVALUE").ToString) & "" 'PURAMT
                strSql += vbCrLf + " ,'" & GetStockCompId() & "'"
                strSql += vbCrLf + " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'"
                strSql += vbCrLf + " ,'" & RoPur.Item("STNSNO").ToString & "'"
                strSql += vbCrLf + " )"
                If XToCostid <> "" And XBranchtag Then
                    ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid, , , "TPURITEMTAGSTONE", , True)
                Else
                    cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                End If
                'ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID, , , "TPURITEMTAGSTONE", , False)
            Next
        End If
        ''iNSERTING mISC
        For cnt As Integer = 0 To dtMiscDetails.Rows.Count - 1
            Dim miscSno As String = GetNewSno(TranSnoType.ITEMTAGMISCCHARCODE, tran, "GET_ADMINSNO_TRAN")
            With dtMiscDetails.Rows(cnt)
                Dim miscId As String = Nothing
                strSql = " SELECT MISCID FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCNAME = '" & .Item("MISC").ToString & "'"
                miscId = Val(objGPack.GetSqlValue(strSql, "MISCID", , tran))
                strSql = " INSERT INTO " & cnAdminDb & "..ITEMTAGMISCCHAR"
                strSql += " ("
                strSql += " SNO,ITEMID,TAGNO,MISCID,AMOUNT,"
                strSql += " TAGSNO,COSTID,SYSTEMID,APPVER,COMPANYID)VALUES("
                strSql += " '" & miscSno & "'" 'SNO
                strSql += " ," & itemId & "" 'ITEMID
                strSql += " ,'" & TagNo & "'" 'TAGNO
                strSql += " ," & miscId & "" 'MISCID
                strSql += " ," & Val(.Item("AMOUNT").ToString) & "" 'AMOUNT
                strSql += " ,'" & TagSno & "'" 'TAGSNO
                strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                strSql += " ,'" & systemId & "'" 'SYSTEMID
                strSql += " ,'" & VERSION & "'" 'APPVER
                strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                strSql += " )"
                If XToCostid <> "" And XBranchtag Then
                    ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid, , , "TITEMTAGMISCCHAR", , True)
                Else
                    cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                End If


            End With
            If _HasPurchase Or pFixedVa Or PUR_AUTOCALC Then
                RowFiter = ObjPurDetail.dtGridMisc.Select("KEYNO = " & Val(dtMiscDetails.Rows(cnt).Item("KEYNO").ToString))
                If RowFiter.Length > 0 Then
                    strSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..PURITEMTAGMISCCHAR"
                    strSql += vbCrLf + " ("
                    strSql += vbCrLf + " MISSNO,TAGSNO"
                    strSql += vbCrLf + " ,ITEMID"
                    strSql += vbCrLf + " ,TAGNO"
                    strSql += vbCrLf + " ,PURAMOUNT,COMPANYID,COSTID,STNSNO"
                    strSql += vbCrLf + " )"
                    strSql += vbCrLf + " VALUES"
                    strSql += vbCrLf + " ("
                    strSql += vbCrLf + " '" & miscSno & "'" 'MISSNO
                    strSql += vbCrLf + " ,'" & TagSno & "'" 'TAGSNO
                    strSql += " ," & itemId & "" 'ITEMID
                    strSql += " ,'" & TagNo & "'" 'TAGNO
                    strSql += " ," & Val(RowFiter(0).Item("PURAMOUNT").ToString) & "" 'PURAMOUNT
                    strSql += vbCrLf + " ,'" & GetStockCompId() & "'"
                    strSql += vbCrLf + " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'"
                    strSql += vbCrLf + " ,'" & miscSno & "'" 'STNSNO
                    strSql += vbCrLf + " )"
                    If XToCostid <> "" And XBranchtag Then
                        ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid, , , "TPURITEMTAGMISCCHAR", , True)
                    Else
                        cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                    End If


                End If
            End If
        Next

        ''INSERTING MULTIMETAL
        For cnt As Integer = 0 To dtMultiMetalDetails.Rows.Count - 1
            Dim metalSno As String = GetNewSno(TranSnoType.ITEMTAGMETALCODE, tran, "GET_ADMINSNO_TRAN")
            Dim multiMetalId As String = Nothing
            Dim multiCatCode As String = Nothing
            With dtMultiMetalDetails.Rows(cnt)
                strSql = " SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME='" & .Item("CATEGORY").ToString & "'"
                multiMetalId = objGPack.GetSqlValue(strSql, "METALID", , tran)
                strSql = " SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME='" & .Item("CATEGORY").ToString & "'"
                multiCatCode = objGPack.GetSqlValue(strSql, "CATCODE", , tran)
                ''INSERTING ITEMTAGMETAL
                strSql = " INSERT INTO " & cnAdminDb & "..ITEMTAGMETAL("
                strSql += " METALID,COMPANYID,ITEMID,RECDATE,"
                strSql += " CATCODE,TAGNO,GRSWT,RATE,"
                strSql += " AMOUNT,TAGSNO,ISSDATE,CARRYFLAG,"
                strSql += " COSTID,SYSTEMID,APPVER,"
                strSql += " WASTPER,WAST,MCGRM,MC,SNO"
                strSql += " )VALUES("
                strSql += " '" & multiMetalId & "'" 'METALID
                strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                strSql += " ,'" & itemId & "'" 'ITEMID
                strSql += " ,'" & GetEntryDate(dtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                strSql += " ,'" & multiCatCode & "'" 'CATCODE
                strSql += " ,'" & TagNo & "'" 'TAGNO
                strSql += " ," & Val(.Item("WEIGHT").ToString) & "" 'GRSWT
                strSql += " ," & Val(.Item("RATE").ToString) & "" 'RATE
                strSql += " ," & Val(.Item("AMOUNT").ToString) & "" 'AMOUNT
                strSql += " ,'" & TagSno & "'" 'TAGSNO
                strSql += " ,NULL" 'ISSDATE
                strSql += " ,''" 'CARRYFLAG
                strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                strSql += " ,'" & systemId & "'" 'SYSTEMID
                strSql += " ,'" & VERSION & "'" 'APPVER
                strSql += " ," & Val(.Item("WASTAGEPER").ToString) & "" 'WASTPER
                strSql += " ," & Val(.Item("WASTAGE").ToString) & "" 'WASTPER
                strSql += " ," & Val(.Item("MCPERGRM").ToString) & "" 'WASTPER
                strSql += " ," & Val(.Item("MC").ToString) & "" 'WASTPER
                strSql += " ,'" & metalSno & "'" 'metaalsno
                strSql += " )"
                If XToCostid <> "" And XBranchtag Then
                    ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid, , , "TITEMTAGMETAL", , True)
                Else
                    cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                End If


            End With
            If _HasPurchase Or pFixedVa Or PUR_AUTOCALC Then
                RowFiter = ObjPurDetail.dtGridMultiMetal.Select("KEYNO = " & Val(dtMultiMetalDetails.Rows(cnt).Item("KEYNO").ToString))
                If RowFiter.Length > 0 Then
                    strSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..PURITEMTAGMETAL"
                    strSql += vbCrLf + " ("
                    strSql += vbCrLf + " METSNO,TAGSNO"
                    strSql += vbCrLf + " ,ITEMID"
                    strSql += vbCrLf + " ,TAGNO,PURRATE,PURWASTAGE,PURMC,PURTOUCH,PURVALUE"
                    strSql += vbCrLf + " ,COMPANYID,COSTID"
                    strSql += vbCrLf + " )"
                    strSql += vbCrLf + " VALUES"
                    strSql += vbCrLf + " ("
                    strSql += vbCrLf + " '" & metalSno & "'" 'MISSNO
                    strSql += vbCrLf + " ,'" & TagSno & "'" 'TAGSNO
                    strSql += " ," & itemId & "" 'ITEMID
                    strSql += " ,'" & TagNo & "'" 'TAGNO
                    strSql += ",0" 'PURRATE
                    strSql += "," & Val(RowFiter(0).Item("PURWASTAGE").ToString) & "" 'PURWASTAGE
                    strSql += "," & Val(RowFiter(0).Item("PURMC").ToString) & "" 'PURMC
                    strSql += ",0" 'PURTOUCH
                    strSql += "," & Val(RowFiter(0).Item("PURAMOUNT").ToString) & "" 'PURVALUE
                    strSql += vbCrLf + " ,'" & GetStockCompId() & "'"
                    strSql += vbCrLf + " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'"
                    strSql += vbCrLf + " )"
                    If XToCostid <> "" And XBranchtag Then
                        ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid, , , "TPURITEMTAGMETAL", , True)
                    Else
                        cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                    End If
                End If

            End If
        Next

        ''INSERTING ADDITIONAL STOCK ENTRY
        For cnt As Integer = 0 To DtAddStockEntry.Rows.Count - 1
            With DtAddStockEntry.Rows(cnt)
                ''INSERTING ADDINFOITEMTAG
                strSql = " INSERT INTO " & cnAdminDb & "..ADDINFOITEMTAG("
                strSql += " SNO,TAGSNO,OTHID,ITEMID,TAGNO,RECDATE,COMPANYID"
                strSql += " ,COSTID,SYSTEMID,APPVER"
                strSql += " )VALUES("
                strSql += " '" & GetNewSno(TranSnoType.ADDINFOITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO  
                strSql += " ,'" & TagSno & "'" 'TAGSNO
                strSql += " ,'" & Val(.Item("OTHID").ToString) & "'" 'OTHID
                strSql += " ,'" & itemId & "'" 'ITEMID
                strSql += " ,'" & TagNo & "'" 'TAGNO
                strSql += " ,'" & GetEntryDate(dtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                strSql += " ,'" & systemId & "'" 'SYSTEMID
                strSql += " ,'" & VERSION & "'" 'APPVER
                strSql += " )"
                If XToCostid <> "" And XBranchtag Then
                    ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid)
                Else
                    cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                End If
            End With
        Next



        ''ORDER IRDETAIL
        If Not OrderRow Is Nothing Then
            ''BATCHNO
            'strSql = " SELECT MAX(CTLTEXT)+1 BATCHNO FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'BATCHNO'"
            'Dim batchNo As String = objGPack.GetSqlValue(strSql, , , tran)
            'batchNo = COSTID + Mid(Today.Year, 3, 2).ToString + batchNo
            Dim ordCompanyId As String = objGPack.GetSqlValue("SELECT COMPANYID FROM " & cnAdminDb & "..ORMAST WHERE SNO = '" & OrderRow.Item("SNO").ToString & "'", , , tran)
            Dim batchno As String = GetNewBatchno(cnCostId, GetEntryDate(dtpRecieptDate.Value, tran).ToString("yyyy-MM-dd"), tran)
            Dim Ord_TranNo As String

            strSql = " SELECT 1 CNT FROM " & cnAdminDb & "..ORIRDETAIL WHERE ISNULL(ORNO,'') = '" & OrderRow.Item("ORNO").ToString & "' AND ISNULL(ORSNO,'') = '" & OrderRow.Item("SNO").ToString & "' AND ISNULL(ORSTATUS,'') = 'I' AND ISNULL(CANCEL,'') = ''"
            Dim dtChk As New DataTable
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(dtChk)
            If dtChk.Rows.Count = 0 Then
                Ord_TranNo = objGPack.GetSqlValue(" SELECT ISNULL(MAX(CTLTEXT),0)+1 AS TRANNO FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID  = 'ORDIRBILLNO' AND COMPANYID = '" & strCompanyId & "'", , , tran)
                strSql = " INSERT INTO " & cnAdminDb & "..ORIRDETAIL"
                strSql += " ("
                strSql += " SNO,ORSNO,TRANNO,TRANDATE,DESIGNERID,PCS,GRSWT,NETWT,TAGNO,ORSTATUS,CANCEL,COSTID,DESCRIPT,ORNO"
                strSql += " ,BATCHNO,USERID,UPDATED,UPTIME,APPVER,COMPANYID,ENTRYORDER,ORDSTATE_ID,CATCODE"
                strSql += " )"
                strSql += " VALUES"
                strSql += " ("
                strSql += " '" & GetNewSno(TranSnoType.ORIRDETAILCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO
                strSql += " ,'" & OrderRow.Item("SNO").ToString & "'" 'ORSNO
                'strSql += " ," & Val(objGPack.GetSqlValue(" SELECT ISNULL(MAX(CTLTEXT),0)+1 AS TRANNO FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID  = 'ORDIRBILLNO' AND COMPANYID = '" & strCompanyId & "'", , , tran)) & "" 'TRANNO
                strSql += " ," & Val(Ord_TranNo) & "" 'TRANNO
                strSql += " ,'" & GetEntryDate(dtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'TRANDATE
                strSql += " ," & designerId & "" 'DESIGNERID
                strSql += " ," & Val(txtPieces_Num_Man.Text) & "" 'PCS
                strSql += " ," & Val(txtGrossWt_Wet.Text) & "" 'GRSWT
                strSql += " ," & Val(txtNetWt_Wet.Text) & "" 'NETWT
                strSql += " ,'" & txtTagNo__Man.Text & "'" 'TAGNO
                strSql += " ,'I'" 'ORSTATUS
                strSql += " ,''" 'CANCEL
                strSql += " ,'" & COSTID & "'" 'COSTID
                strSql += " ,'" & txtNarration.Text & "'" 'DESCRIPT
                strSql += " ,'" & OrderRow.Item("ORNO").ToString & "'" 'ORNO
                strSql += " ,'" & batchno & "'" 'BATCHNO
                strSql += " ," & userId & "" 'USERID
                strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                strSql += " ,'" & VERSION & "'" 'APPVER
                strSql += " ,'" & ordCompanyId & "'" 'COMPANYID
                strSql += " ," & objGPack.GetSqlValue("SELECT ISNULL(MAX(ENTRYORDER),0)+1 FROM " & cnAdminDb & "..ORIRDETAIL", , , tran) & "" 'ENTRYORDER
                strSql += " ,2" 'ORDSTATE_ID
                strSql += " ,'" & objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(itemId) & "", , , tran) & "'" 'CATCODE
                strSql += " )"
                If XToCostid <> "" And XBranchtag Then
                    ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid, , , , , True)
                Else
                    cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                End If

                strSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & Ord_TranNo & "' WHERE CTLID = 'ORDIRBILLNO' AND COMPANYID = '" & strCompanyId & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
            End If


            Ord_TranNo = objGPack.GetSqlValue(" SELECT ISNULL(MAX(CTLTEXT),0)+1 AS TRANNO FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID  = 'ORDIRBILLNO' AND COMPANYID = '" & strCompanyId & "'", , , tran)

            strSql = " INSERT INTO " & cnAdminDb & "..ORIRDETAIL"
            strSql += " ("
            strSql += " SNO,ORSNO,TRANNO,TRANDATE,DESIGNERID,PCS,GRSWT,NETWT,TAGNO,ORSTATUS,CANCEL,COSTID,DESCRIPT,ORNO"
            strSql += " ,BATCHNO,USERID,UPDATED,UPTIME,APPVER,COMPANYID,ENTRYORDER,ORDSTATE_ID,CATCODE"
            strSql += " )"
            strSql += " VALUES"
            strSql += " ("
            strSql += " '" & GetNewSno(TranSnoType.ORIRDETAILCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO
            strSql += " ,'" & OrderRow.Item("SNO").ToString & "'" 'ORSNO
            'strSql += " ," & Val(objGPack.GetSqlValue(" SELECT ISNULL(MAX(CTLTEXT),0)+1 AS TRANNO FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID  = 'ORDIRBILLNO' AND COMPANYID = '" & strCompanyId & "'", , , tran)) & "" 'TRANNO
            strSql += " ," & Val(Ord_TranNo) & "" 'TRANNO
            strSql += " ,'" & GetEntryDate(dtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'TRANDATE
            strSql += " ," & designerId & "" 'DESIGNERID
            strSql += " ," & Val(txtPieces_Num_Man.Text) & "" 'PCS
            strSql += " ," & Val(txtGrossWt_Wet.Text) & "" 'GRSWT
            strSql += " ," & Val(txtNetWt_Wet.Text) & "" 'NETWT
            strSql += " ,'" & txtTagNo__Man.Text & "'" 'TAGNO
            strSql += " ,'R'" 'ORSTATUS
            strSql += " ,''" 'CANCEL
            strSql += " ,'" & COSTID & "'" 'COSTID
            strSql += " ,'" & txtNarration.Text & "'" 'DESCRIPT
            strSql += " ,'" & OrderRow.Item("ORNO").ToString & "'" 'ORNO
            strSql += " ,'" & batchno & "'" 'BATCHNO
            strSql += " ," & userId & "" 'USERID
            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
            strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
            strSql += " ,'" & VERSION & "'" 'APPVER
            strSql += " ,'" & ordCompanyId & "'" 'COMPANYID
            strSql += " ," & objGPack.GetSqlValue("SELECT ISNULL(MAX(ENTRYORDER),0)+1 FROM " & cnAdminDb & "..ORIRDETAIL", , , tran) & "" 'ENTRYORDER
            strSql += " ,4" 'ORDSTATE_ID
            strSql += " ,'" & objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(itemId) & "", , , tran) & "'" 'CATCODE
            strSql += " )"
            If XToCostid <> "" And XBranchtag Then
                ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid, , , , , True)
            Else
                cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
            End If
            'ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID, , , , , False)
            strSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & Ord_TranNo & "' WHERE CTLID = 'ORDIRBILLNO' AND COMPANYID = '" & strCompanyId & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()
        End If


        If Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnAdminDb & "..DIAUNIQUETAG WHERE UNQDESC='" & MTagprefix & "'", "", "", tran)) > 0 Then
            strSql = " UPDATE " & cnAdminDb & "..DIAUNIQUETAG SET LASTNO =" & MLasttagno & " WHERE UNQDESC='" & MTagprefix & "'"
            cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
        Else
            strSql += " INSERT INTO " & cnAdminDb & "..DIAUNIQUETAG(UNQDESC,LASTNO) "
            strSql += " VALUES('" & MTagprefix & "',1)"
            cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
        End If



        'Dim tagPrefix As String = GetSoftValue("TAGPREFIX")
        'Dim updTagNo As String = Nothing
        'If tagPrefix.Length > 0 Then
        '    updTagNo = txtTagNo__Man.Text.Replace(tagPrefix, "")
        'Else
        '    updTagNo = txtTagNo__Man.Text
        'End If

        'If GetSoftValue("TAGNOFROM") = "I" Then ''FROM ITEMMAST OR UNIQUE
        '    If GetSoftValue("TAGNOGEN") = "I" Then ''FROM ITEM
        '        strSql = " UPDATE " & cnAdminDb & "..ITEMMAST SET CURRENTTAGNO = '" & updTagNo & "' WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"
        '        cmd = New OleDbCommand(strSql, cn, tran)
        '        cmd.ExecuteNonQuery()
        '        'ExecQuery(strSql, cn, tran, COSTID)
        '    Else ''FROM LOT
        '        strSql = " UPDATE " & cnAdminDb & "..ITEMLOT SET CURTAGNO = '" & updTagNo & "'"
        '        cmd = New OleDbCommand(strSql, cn, tran)
        '        cmd.ExecuteNonQuery()
        '        'ExecQuery(strSql, cn, tran, COSTID)
        '    End If
        'ElseIf GetSoftValue("TAGNOFROM") = "U" Then  'UNIQUE
        '    strSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT  = '" & updTagNo & "' WHERE CTLID = 'LASTTAGNO'"
        '    cmd = New OleDbCommand(strSql, cn, tran)
        '    cmd.ExecuteNonQuery()
        '    'ExecQuery(strSql, cn, tran, COSTID)
        'End If

        'If cnCostId.ToUpper <> COSTID.ToUpper Then
        '    Dim obj As New TrasitIssRec(cnCostId, COSTID, "I", GetEntryDate(GetServerDate(tran), tran), TagSno, cn, tran)
        '    If obj.InsertTagIssue(False) Then
        '        strSql = " INSERT INTO " & cnStockDb & "..TRANSIT_AUDIT"
        '        strSql += " (FROMID,TOID,TRANDATE,TAGSNO,ISSREC,STOCKTYPE)"
        '        strSql += " SELECT '" & cnCostId & "','" & COSTID & "','" & dtpRecieptDate.Value.ToString("yyyy-MM-dd") & "','" & TagSno & "','I','T'"
        '        cmd = New OleDbCommand(strSql, cn, tran)
        '        cmd.ExecuteNonQuery()
        '    End If
        'End If


        'CPIECES AND CWT
        'strSql = " UPDATE " & cnAdminDb & "..ITEMLOT SET CPCS = CPCS + " & Val(txtPieces_Num_Man.Text) & ""
        'strSql += " ,CGRSWT = CGRSWT + " & Val(txtGrossWt_Wet.Text) & ""
        'strSql += " ,CNETWT = ISNULL(CNETWT,0) + " & Val(txtNetWt_Wet.Text) & ""
        'strSql += " WHERE SNO = '" & SNO & "'"
        'cmd = New OleDbCommand(strSql, cn, tran)
        'cmd.ExecuteNonQuery()
        'ExecQuery(strSql, cn, tran, COSTID)


        ''INSERT INTO GRIDVIEW
        Dim ro As DataRow = Nothing
        ro = dtGridView.NewRow
        ro("ITEM") = cmbItem_MAN.Text
        ro("SUBITEM") = cmbSubItem_Man.Text
        ro("TAGNO") = TagNo
        ro("PCS") = IIf(Val(txtPieces_Num_Man.Text) <> 0, Val(txtPieces_Num_Man.Text), DBNull.Value)
        ro("GRSWT") = IIf(Val(txtGrossWt_Wet.Text) <> 0, Val(txtGrossWt_Wet.Text), DBNull.Value)
        ro("LESSWT") = IIf(Val(txtLessWt_Wet.Text) <> 0, Val(txtLessWt_Wet.Text), DBNull.Value)
        ro("NETWT") = IIf(Val(txtNetWt_Wet.Text) <> 0, Val(txtNetWt_Wet.Text), DBNull.Value)
        ro("RATE") = IIf(Val(txtRate_Amt.Text) <> 0, Val(txtRate_Amt.Text), DBNull.Value)
        ro("SALEVALUE") = txtSalValue_Amt_Man.Text
        ro("SIZE") = cmbItemSize.Text
        dtGridView.Rows.Add(ro)
        dtGridView.AcceptChanges()
        gridView.CurrentCell = gridView.Rows(gridView.RowCount - 1).Cells("ITEM")

        Exit Function
        'tran.Commit()
        'tran = Nothing

        'MsgBox(TagNo + E0012, MsgBoxStyle.Exclamation)
        ' ''Lot Pcs
        'lblPCompled.Text = Val(lblPCompled.Text) + Val(txtPieces_Num_Man.Text)
        'lblPBalance.Text = Val(lblPLot.Text) - Val(lblPCompled.Text)
        'mfromItemid = 0
        ' ''Lot Wt
        'lblWCompleted.Text = IIf(Val(lblWCompleted.Text) + Val(txtGrossWt_Wet.Text) <> 0, _
        'Format(Val(lblWCompleted.Text) + Val(txtGrossWt_Wet.Text), "0.000"), Nothing)
        'lblWBalance.Text = IIf(Val(lblWLot.Text) - Val(lblWCompleted.Text) <> 0, _
        'Format(Val(lblWLot.Text) - Val(lblWCompleted.Text), "0.000"), Nothing)

        ''Last Tag and Wt
        lblLastTagNo.Text = TagNo
        lblLastTagWt.Text = txtGrossWt_Wet.Text
        Dim prnmemsuffix As String = ""
        If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
        If chkBarcodePrint.Checked = True Then
            If GetAdmindbSoftValue("SING-TRANDB", "N") = "Y" Then
                ''for kanaga laxmi
                If cmbSubItem_Man.Enabled = True Then 'DESCRIP
                    BarcodeDescrip = cmbSubItem_Man.Text
                Else
                    BarcodeDescrip = cmbItem_MAN.Text
                End If
                BarcodeTagNo = TagNo
                BarcodeSno = TagSno
                FRM_PRINTDIA.ShowDialog()
            Else
                ''190609 modified
                Dim memfile As String = "\Barcodeprint" & prnmemsuffix.Trim & ".mem"
                Dim write As StreamWriter
                write = IO.File.CreateText(Application.StartupPath & memfile)
                write.WriteLine(LSet("PROC", 7) & ":" & itemId)
                write.WriteLine(LSet("TAGNO", 7) & ":" & TagNo)
                write.Flush()
                write.Close()

                If EXE_WITH_PARAM = False Then
                    If IO.File.Exists(Application.StartupPath & "\PrjBarcodePrint.exe") Then
                        System.Diagnostics.Process.Start(Application.StartupPath & "\PrjBarcodePrint.exe")
                    Else
                        MsgBox("Barcode print exe not found", MsgBoxStyle.Information)
                    End If
                Else
                    Dim itDesc As String = LSet("PROC", 7) & ":" & itemId
                    Dim tagDesc As String = LSet("TAGNO", 7) & ":" & TagNo
                    If IO.File.Exists(Application.StartupPath & "\PrjBarcodePrint.exe") Then
                        System.Diagnostics.Process.Start(Application.StartupPath & "\PrjBarcodePrint.exe", itDesc & ";" & tagDesc)
                    Else
                        MsgBox("Barcode print exe not found", MsgBoxStyle.Information)
                    End If
                End If
            End If
        End If

        If chkGuaranteeCard.Checked = True Then
            '290609 modified
            frmCardDetails.cardImagePath = picPath
            If IO.File.Exists(picPath) Then frmCardDetails.picTag.Image = Image.FromFile(picPath)
            frmCardDetails.TagNoEnb = False
            frmCardDetails.cmbTagNo.Text = TagNo
            frmCardDetails.cmbTagNo.Enabled = False
            frmCardDetails.ShowDialog()
        End If
        Dim flagComplete As Boolean = False
        'If tabCheckBy = "P" Then
        '    If Not Val(lblPBalance.Text) > 0 Then flagComplete = True
        'Else
        '    If Not Val(lblWBalance.Text) > 0 Then flagComplete = True
        'End If

        'If flagComplete Then
        'MsgBox("Lot Completed..", MsgBoxStyle.Information)
        'If lotprintack = True Then
        '    If MsgBox("Do you print acknowledgement?", MsgBoxStyle.YesNo, "Brighttech Message") = MsgBoxResult.Yes Then
        '        DetailPrint()
        '    End If
        'End If
        funcNew()
        'Else
        '    funcMultyNew()
        '    If Not OrderRow Is Nothing Then
        '        LoadOrderDetails(OrderRow.Item("ORNO").ToString)
        '    ElseIf _CheckOrderInfo = False And ObjOrderTagInfo.txtOrderNo.Text <> "" Then
        '        If ObjOrderTagInfo.ShowDialog() <> Windows.Forms.DialogResult.OK Then
        '            btnNew_Click(Me, New EventArgs)
        '            txtLotNo_Num.Focus()
        '            Exit Function
        '        End If
        '    End If
        'End If

        'Catch ex As Exception
        '    If Not tran Is Nothing Then tran.Rollback()
        '    MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        'Finally
        '    If Not tran Is Nothing Then tran.Dispose()
        'End Try
    End Function

    Public Function funcOrIrDetail_Save(ByVal OrdNo As String, ByVal OrdSno As String, ByVal TagNo As String, ByVal BatchNO As String, ByVal ItemId As String, ByVal DesignerId As String, ByVal CostId As String)
        ''Find TranNo
        strSql = " SELECT ISNULL(MAX(CTLTEXT),0)+1 AS TRANNO FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID  = 'ORDIRBILLNO' AND COMPANYID = '" & strCompanyId & "'"
        Dim tranNo As String = Val(objGPack.GetSqlValue(strSql, , , tran))

        strSql = " INSERT INTO " & cnAdminDb & "..ORIRDETAIL"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SNO,ORSNO,TRANNO,TRANDATE,DESIGNERID,PCS,GRSWT,NETWT,WASTAGE,MC,TAGNO,ORSTATUS,CANCEL,COSTID,DESCRIPT,ORNO"
        strSql += vbCrLf + " ,BATCHNO,USERID,UPDATED,UPTIME,APPVER,COMPANYID,ENTRYORDER,ORDSTATE_ID,CATCODE"
        strSql += vbCrLf + " )"
        strSql += vbCrLf + " SELECT '" & GetNewSno(TranSnoType.ORIRDETAILCODE, tran, "GET_ADMINSNO_TRAN") & "' SNO,SNO ORSNO," & tranNo & " TRANNO,'" & GetEntryDate(GetServerDate(tran), tran) & "' TRANDATE"
        strSql += vbCrLf + " ," & DesignerId & " DESIGNERID,PCS,GRSWT,NETWT,WAST WASTAGE,MC"
        strSql += vbCrLf + " ,'" & TagNo & "' TAGNO,'R' ORSTATUS,'' CANCEL,'" & CostId & "' COSTID,'' DESCRIPT"
        strSql += vbCrLf + " ,ORNO,'" & BatchNO & "' BATCHNO,USERID,UPDATED,UPTIME"
        strSql += vbCrLf + " ,APPVER,COMPANYID,'" & objGPack.GetSqlValue("SELECT ISNULL(MAX(ENTRYORDER),0)+1 FROM " & cnAdminDb & "..ORIRDETAIL", , , tran) & "' ENTRYORDER,4 ORDSTATE_ID"
        strSql += vbCrLf + " ,'" & objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(ItemId) & "", , , tran) & "' CATCODE"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ORMAST WHERE ISNULL(ORNO,'') = '" & OrdNo & "' AND ISNULL(SNO,'') = '" & OrdSno & "'"
        ExecQuery(SyncMode.Stock, strSql, cn, tran, CostId)
        strSql = "SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE MAIN = 'Y'"
        Dim HeadCostId As String = BrighttechPack.GetSqlValue(cn, strSql, "COSTID", "", tran)
        If CostId <> HeadCostId Then
            ExecQuery(SyncMode.Stock, strSql, cn, tran, HeadCostId, , , , False)
        End If

        ''UPDATING
        strSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT = '" & tranNo & "' WHERE CTLID = 'ORDIRBILLNO' AND COMPANYID = '" & strCompanyId & "'"
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        tran.Commit()
        tran = Nothing
    End Function

    Function funcCostCentre(Optional ByVal defaultItem As String = "") As Integer
        cmbCostCentre_Man.Items.Clear()
        cmbCostCentre_Man.Items.Add("")
        strSql = " select CostName from " & cnAdminDb & "..CostCentre order by CostName"
        objGPack.FillCombo(strSql, cmbCostCentre_Man, False)
        cmbCostCentre_Man.Text = ""
        If defaultItem <> "" Then
            cmbCostCentre_Man.Text = defaultItem
        End If
    End Function

    Private Function GetSoftValue(ByVal id As String) As String
        Return UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = '" & id & "'", , "", tran))
    End Function

    Private Function GetTagNo() As String
        Dim tagNo As String = Nothing
        Dim str As String = Nothing
        If GetSoftValue("TAGNOFROM") = "I" Then ''FROM ITEMMAST OR UNIQUE
            If GetSoftValue("TAGNOGEN") = "I" Then ''FROM ITEM
                str = " SELECT CURRENTTAGNO LASTTAGNO FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = "
                str += " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')"
            Else ''FROM LOT
                str = " SELECT CURTAGNO AS LASTTAGNO FROM " & cnAdminDb & "..ITEMLOT WHERE ITEMID = "
                str += " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')"
                str += " AND SNO = '" & SNO & "'"
            End If
        Else 'UNIQUE
            str = " SELECT CTLTEXT AS LASTTAGNO FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'LASTTAGNO'"
        End If
        tagNo = objGPack.GetSqlValue(str, , "1", tran)
        Return GenTagNo(tagNo)
    End Function

    Private Function GenTagNo(ByRef TagNo As String) As String
        Dim str As String = Nothing
        If IsNumeric(TagNo) Then
            TagNo = Val(TagNo) + 1
        Else
            Dim fPart As String = Nothing
            Dim sPart As String = Nothing
            For Each c As Char In TagNo
                If IsNumeric(c) Then
                    sPart += c
                Else
                    fPart += c
                End If
            Next
            TagNo = fPart + (Val(sPart) + 1).ToString
        End If
        Dim tagPrefix As String = GetSoftValue("TAGPREFIX")
        Return tagPrefix + TagNo
    End Function

    Private Sub StyleGridMultiMetal()

        With gridMultimetal
            .Columns("CATEGORY").Width = txtMMCategory.Width + 1
            .Columns("RATE").Visible = False
            With .Columns("WEIGHT")
                .HeaderText = "WEIGHT"
                .Width = txtMMWeight_Wet.Width + 1
                .DefaultCellStyle.Format = "0.000"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            .Columns("PURWASTAGEPER").Visible = False
            .Columns("PURWASTAGE").Visible = False
            .Columns("PURMCPERGRM").Visible = False
            .Columns("PURMC").Visible = False
            With .Columns("WASTAGEPER")
                .HeaderText = "WAST%"
                .Width = txtMMWastagePer_PER.Width + 1
                .DefaultCellStyle.Format = "0.000"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("WASTAGE")
                .HeaderText = "WAST"
                .Width = txtMMWastage_WET.Width + 1
                .DefaultCellStyle.Format = "0.000"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("MCPERGRM")
                .HeaderText = "MC/GRM"
                .Width = txtMMMcPerGRm_AMT.Width + 1
                .DefaultCellStyle.Format = "0.000"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("MC")
                .HeaderText = "MC"
                .Width = txtMMMc_AMT.Width + 1
                .DefaultCellStyle.Format = "0.000"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("AMOUNT")
                .HeaderText = "AMOUNT"
                .Width = txtMMAmount_AMT.Width + 1
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "0.00"
            End With
            With .Columns("PURRATE")
                .Width = 80
                .HeaderText = "PURRATE"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "0.00"
                .Visible = False
            End With
            With .Columns("PURAMOUNT")
                .Width = 99
                .HeaderText = "PURAMOUNT"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .DefaultCellStyle.Format = "0.00"
                .Visible = False
            End With
            .Columns("KEYNO").Visible = False

            .Columns("STNSNO").Visible = False
        End With
    End Sub

    Private Sub StyleGridStone()
        With gridStone
            .Columns("PACKETNO").Width = 85
            If PacketNoEnable = "N" Then
                .Columns("PACKETNO").Visible = False
                txtstPackettno.Visible = False
                lblstPacketno.Visible = False
                pnlStDet.AutoSize = True
                pnlStDet.Location = New Point(12, 11)
                pnlStoneGrid.Size = New Size(773, 155)
            Else
                .Columns("PACKETNO").Visible = True
                txtstPackettno.Visible = True
                lblstPacketno.Visible = True
            End If
            .Columns("ITEM").Width = 211
            .Columns("SUBITEM").Width = 186
            .Columns("UNIT").Width = 40
            .Columns("CALC").Width = 40
            .Columns("PCS").Width = 39
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("WEIGHT").Width = 74
            .Columns("WEIGHT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RATE").Width = 80
            .Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").Width = 99
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("METALID").Visible = False
            .Columns("KEYNO").Visible = False
            With .Columns("PURRATE")
                .Width = 80
                .Visible = False
                .DefaultCellStyle.Format = "0.00"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("PURVALUE")
                .Width = 99
                .Visible = False
                .DefaultCellStyle.Format = "0.00"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            .Columns("STNSNO").Visible = False
        End With
    End Sub
    Private Sub StyleGridMisc()
        With gridMisc
            With .Columns("MISC")
                .HeaderText = "MISCELLANEOUS"
                .Width = 298
            End With
            With .Columns("AMOUNT")
                .HeaderText = "AMOUNT"
                .Width = 99
                .DefaultCellStyle.Format = "0.00"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("PURAMOUNT")
                .Visible = False
                .Width = 99
                .DefaultCellStyle.Format = "0.00"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            .Columns("KEYNO").Visible = False
        End With
    End Sub

    Private Function GetParity(ByVal ParityName As String) As System.IO.Ports.Parity

        Select Case ParityName
            Case "N"
                Return Ports.Parity.None
            Case "E"
                Return Ports.Parity.Even
            Case "M"
                Return Ports.Parity.Mark
            Case "O"
                Return Ports.Parity.Odd
            Case "S"
                Return Ports.Parity.Space
            Case Else
                Return Ports.Parity.None
        End Select
    End Function

    Private Sub chkAutosnap()

        If GetSoftValue("AUTOSNAP") = "Y" Then
            flagDeviceMode = True
            btnAttachImage.Text = "&Capture Image"
        Else
            btnAttachImage.Text = "&Attach Image"
            flagDeviceMode = False
        End If

        If flagDeviceMode = True Then

            picCapture.Visible = True
            PictureBox1.Visible = True
            'Panel2.Location = New Point(192, 3)
            OpenForm()
        Else
            picCapture.Visible = False
            PictureBox1.Visible = False
        End If
    End Sub


    Function funcAssignTabControls() As Integer
        TabControl1.TabPages.Remove(tabMultiMetal)
        TabControl1.TabPages.Remove(tabStone)
        TabControl1.TabPages.Remove(tabOtherCharges)

        Dim ds As New DataSet
        ds.Clear()
        If multiMetal = "Y" Then
            TabControl1.TabPages.Add(tabMultiMetal)
        End If
        If studdedStone = "Y" Then
            TabControl1.TabPages.Add(tabStone)
        End If
        If OthCharge = "Y" Then
            TabControl1.TabPages.Add(tabOtherCharges)
        End If

        If UCase(objGPack.GetSqlValue("SELECT TAGTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'")) = "Y" Then
            cmbItemType_MAN.Enabled = True
        Else
            cmbItemType_MAN.Enabled = False
        End If


        ''Set PurchaseTab
        Dim dt As New DataTable
        dt.Clear()
        strSql = " select 1 from " & cnAdminDb & "..SoftControl where ctlId = 'PURTAB' and ctlText = 'Y'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            'TabControl1.TabPages.Add(tabPurchase)
        End If
        pnlMax.Enabled = False
        If calType = "R" Then
            txtGrossWt_Wet.Enabled = False
            txtLessWt_Wet.Enabled = False
            txtNetWt_Wet.Enabled = False
            txtRate_Amt.Enabled = True
            cmbCalcMode.Enabled = False
            txtRate_Amt.Text = pieceRate
        ElseIf calType = "M" Then
            txtGrossWt_Wet.Enabled = True
            txtLessWt_Wet.Enabled = True
            txtNetWt_Wet.Enabled = True
            txtRate_Amt.Enabled = True
            If CalMode = "O" Then cmbCalcMode.Enabled = True
            txtRate_Amt.Text = pieceRate
            If GetSoftValue("MAXMCFOCUS") = "Y" Then pnlMax.Enabled = True
        ElseIf calType = "F" Then
            txtGrossWt_Wet.Enabled = True
            txtLessWt_Wet.Enabled = True
            txtNetWt_Wet.Enabled = True
            txtRate_Amt.Enabled = True
            If CalMode = "O" Then cmbCalcMode.Enabled = True
            If GetSoftValue("MAXMCFOCUS") = "Y" Then pnlMax.Enabled = True
        ElseIf calType = "B" Then
            txtGrossWt_Wet.Enabled = True
            txtLessWt_Wet.Enabled = True
            txtNetWt_Wet.Enabled = True
            txtRate_Amt.Enabled = True
            If CalMode = "O" Then cmbCalcMode.Enabled = True
            If GetSoftValue("MAXMCFOCUS") = "Y" Then pnlMax.Enabled = True
        Else
            txtGrossWt_Wet.Enabled = True
            txtLessWt_Wet.Enabled = True
            txtNetWt_Wet.Enabled = True
            txtRate_Amt.Enabled = False
            If CalMode = "O" Then cmbCalcMode.Enabled = True
            If GetSoftValue("MAXMCFOCUS") = "Y" Then pnlMax.Enabled = True
        End If
        txtMetalRate_Amt.Text = Format(GetMetalRate(), "0.00")
    End Function

    Private Sub LoadOrderDetails(ByVal ORDERNO As String, Optional ByVal mcostid As String = "")
        Dim dt As DataTable
        strSql = " SELECT "
        strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = O.ITEMID) AS ITEM"
        strSql += " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = O.SUBITEMID) AS SUBITEM"
        strSql += " ,(SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZEID = O.SIZEID)AS SIZENAME"
        strSql += " ,O.*"
        strSql += " FROM " & cnAdminDb & "..ORMAST O"
        strSql += " WHERE ISNULL(ORDCANCEL,'') <> 'Y' "
        If mcostid <> "" And CHKCOST_ORD = True Then strSql += " AND COSTID='" & mcostid & "'"
        If Mid(ORDERNO, 6, 1) = "O" Then
            strSql += " AND isnull(ORNO,'') ='" & ORDERNO & "'"
        Else
            strSql += " AND SUBSTRING(ORNO, 6, 20) ='" & ORDERNO & "'"
        End If

        'AND SUBSTRING(ORNO,6,20) = '" & ORDERNO & "'"
        strSql += " AND "
        strSql += " NOT EXISTS (sELECT 1 FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSTATUS = 'R' AND ORSNO = O.SNO AND ORDSTATE_ID = 4)"
        strSql += " AND ISNULL(ODBATCHNO,'') = ''  AND ISNULL(CANCEL,'') = ''"
        strSql += " ORDER BY ORNO,SNO"
        Dim dtOrdDet As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtOrdDet)
        If dtOrdDet.Rows.Count = 1 Then
            OrderRow = dtOrdDet.Rows(0)
        Else
            OrderRow = BrighttechPack.SearchDialog.Show_R("Select Order ", strSql, cn, , , , , , False)
        End If

        If OrderRow Is Nothing Then
            btnNew_Click(Me, New EventArgs)
            Exit Sub
        End If
        strSql = " SELECT * FROM " & cnAdminDb & "..ORMAST"
        strSql += " WHERE ISNULL(ORDCANCEL,'') <> 'Y' "
        If Mid(ORDERNO, 6, 1) = "O" Then
            strSql += " AND isnull(ORNO,'') ='" & ORDERNO & "'"
        Else
            strSql += " AND SUBSTRING(ORNO, 6, 20) ='" & ORDERNO & "'"
        End If

        'SUBSTRING(ORNO,6,20) = '" & ORDERNO & "'"
        strSql += " AND SNO = '" & OrderRow.Item("SNO").ToString & "'"
        dt = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then Exit Sub
        With dt.Rows(0)
            cmbItem_MAN.Text = OrderRow.Item("ITEM").ToString
            pFixedVa = IIf(objGPack.GetSqlValue("SELECT ISNULL(FIXEDVA,'N') FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'", , "N") = "N", False, True)
            cmbSubItem_Man.Items.Clear()
            If objGPack.GetSqlValue("SELECT SUBITEM FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'") = "Y" Then
                strSql = GetSubItemQry(New String() {"SUBITEMNAME"}, Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'")))
                objGPack.FillCombo(strSql, cmbSubItem_Man, False)
                If Val(.Item("SUBITEMID").ToString) <> 0 Then cmbSubItem_Man.Text = objGPack.GetSqlValue("SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = " & Val(.Item("SUBITEMID").ToString) & "")
                cmbSubItem_Man.Enabled = True
            Else
                cmbSubItem_Man.Enabled = False
                cmbSubItem_Man.Text = ""
            End If
            cmbSubItem_Man.Text = OrderRow.Item("SUBITEM").ToString
            cmbItemSize.Text = OrderRow.Item("SIZENAME").ToString
            OrderRow = dt.Rows(0)
            txtPieces_Num_Man.Text = IIf(Val(OrderRow.Item("PCS").ToString) > 0, Val(OrderRow.Item("PCS").ToString), "")
            txtGrossWt_Wet.Text = IIf(Val(OrderRow.Item("GRSWT").ToString) > 0, Format(Val(OrderRow.Item("GRSWT").ToString), "0.000"), "")
            txtNetWt_Wet.Text = IIf(Val(OrderRow.Item("NETWT").ToString) > 0, Format(Val(OrderRow.Item("NETWT").ToString), "0.000"), "")
            strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = "
            strSql += " (SELECT TOP 1 DESIGNERID FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = '" & OrderRow.Item("SNO").ToString & "' AND ISNULL(CANCEL,'') = '' ORDER BY ENTRYORDER DESC)"
            cmbDesigner_MAN.Text = objGPack.GetSqlValue(strSql)
            strSql = " SELECT"
            strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = STNITEMID)AS ITEM"
            strSql += " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = STNSUBITEMID)AS SUBITEM"
            strSql += " ,STNPCS PCS,STNWT WEIGHT,STNUNIT UNIT,CALCMODE CALC,STNRATE RATE,STNAMT AMOUNT"
            strSql += " ,(SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = STNITEMID)METALID"
            strSql += " ,NULL PURRATE,NULL PURVALUE,0 USRATE,0 INDRS"
            strSql += " FROM " & cnAdminDb & "..ORSTONE"
            strSql += " WHERE BATCHNO = '" & .Item("BATCHNO").ToString & "'"
            strSql += " AND ORSNO = '" & .Item("SNO").ToString & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtStoneDetails)
            CalcLessWt()
            CalcFinalTotal()
            'cmbItem_MAN.Focus()
        End With
    End Sub



    Function funcAssignTagDefaultVal() As String

        strSql = " SELECT  LOT.LOTNO,LOT.ENTRYORDER,LOT.LOTDATE,LOT.RATE AS LOTRATE ,IT.ITEMNAME AS ITEMNAME,"
        strSql += " ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS IM WHERE IM.SUBITEMID = LOT.SUBITEMID),'')AS SUBITEMNAME ,"
        strSql += " CASE WHEN ISNULL(LC.ITEMCTRNAME,'') <>'' THEN LC.ITEMCTRNAME ELSE IC.ITEMCTRNAME END AS DEFAULTCOUNTER ,"
        strSql += " ISNULL(DS.DESIGNERNAME,'') AS DEFAULTDESIGNERNAME ,"
        strSql += " ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = LOT.COSTID),'')AS DEFAULTCOSTCENTRE ,"
        strSql += " CASE WHEN ENTRYTYPE = 'R' THEN 'REGULAR' WHEN ENTRYTYPE = 'OR' THEN 'ORDER' WHEN ENTRYTYPE = 'RE' THEN 'REPAIR' WHEN ENTRYTYPE = 'SR' THEN 'SALES RETURN' WHEN ENTRYTYPE = 'NT' THEN 'NONTAG TO TAG' ELSE 'LOT' END AS ENTRYTYPE ,"
        strSql += " LOT.STARTTAGNO,LOT.ENDTAGNO,LOT.CURTAGNO,LOT.PCS,LOT.GRSWT,LOT.CPCS,LOT.CGRSWT,(LOT.PCS-LOT.CPCS)AS BALPCS,(LOT.GRSWT-LOT.CGRSWT)AS BALGRSWT ,"
        strSql += " ISNULL(IT.OTHCHARGE ,'')AS OTHCHARGE ,"
        strSql += " IT.METALID AS METALID ,IT.STUDDEDSTONE AS STUDDEDSTONE , IT.GROSSNETWTDIFF AS GROSSNETWTDIFF ,"
        strSql += " IT.SIZESTOCK AS SIZESTOCK , IT.MULTIMETAL AS MULTIMETAL , IT.CALTYPE AS CALTYPE ,"
        strSql += " (SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID =  (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE =  IT.CATCODE ))AS PURITY ,"
        strSql += " IT.NOOFPIECE AS NOOFPIECE , IT.PIECERATE AS PIECERATE , IT.STOCKTYPE AS STOCKTYPE ,"
        strSql += " WMCTYPE AS VALUEADDEDTYPE ,FINERATE,LOT.SALEPER,TUCH,WASTPER,MCGRM,LOT.OTHCHARGE ,"
        strSql += " ITT.NAME AS ITEMTYPE ,"
        strSql += "CASE WHEN ISNULL(IT.MCCALC,'N') = 'G' THEN 'GRS WT' ELSE 'NET WT' END AS MCCALC "
        strSql += " FROM " & cnAdminDb & "..ITEMLOT AS LOT LEFT JOIN " & cnAdminDb & "..ITEMMAST IT ON LOT.ITEMID = IT.ITEMID "
        strSql += " LEFT JOIN " & cnAdminDb & "..ITEMCOUNTER LC ON LC.ITEMCTRID = LOT.ITEMCTRID"
        strSql += " LEFT JOIN  " & cnAdminDb & "..ITEMCOUNTER IC ON IC.ITEMCTRID = IT.DEFAULTCOUNTER"
        strSql += " LEFT JOIN " & cnAdminDb & "..DESIGNER AS DS ON DS.DESIGNERID = LOT.DESIGNERID"
        strSql += " LEFT JOIN " & cnAdminDb & "..ITEMTYPE ITT ON ITT.ITEMTYPEID = LOT.ITEMTYPEID"
        strSql += " WHERE SNO = '" & SNO & "'"
        Return strSql
    End Function

    Function funcAssignVal(ByVal dt As DataTable, Optional ByVal isOrder As Boolean = False) As Integer
        If dt.Rows.Count > 0 Then
            Dim Assorted As Boolean = False
            With dt.Rows(0)
                If objGPack.GetSqlValue("SELECT ISNULL(ASSORTED,'') FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Item("ItemName").ToString & "'") = "Y" Then
                    Assorted = True
                End If
                'lblItemChange.Visible = Assorted
                If Assorted Then
                    strSql = " SELECT OTHCHARGE,METALID,STUDDEDSTONE,SIZESTOCK,MULTIMETAL,CALTYPE"
                    strSql += " ,(SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = "
                    strSql += " (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE))AS PURITY"
                    strSql += " ,NOOFPIECE,PIECERATE,STOCKTYPE"
                    strSql += " FROM " & cnAdminDb & "..ITEMMAST as i WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"
                    Dim dtItemDet As New DataTable
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(dtItemDet)
                    If dtItemDet.Rows.Count > 0 Then
                        .Item("OTHCHARGE") = dtItemDet.Rows(0).Item("OTHCHARGE")
                        .Item("METALID") = dtItemDet.Rows(0).Item("METALID")
                        .Item("STUDDEDSTONE") = dtItemDet.Rows(0).Item("STUDDEDSTONE")
                        .Item("SIZESTOCK") = dtItemDet.Rows(0).Item("SIZESTOCK")
                        .Item("MULTIMETAL") = dtItemDet.Rows(0).Item("MULTIMETAL")
                        .Item("CALTYPE") = dtItemDet.Rows(0).Item("CALTYPE")
                        .Item("PURITY") = dtItemDet.Rows(0).Item("PURITY")
                        .Item("NOOFPIECE") = dtItemDet.Rows(0).Item("NOOFPIECE")
                        .Item("PIECERATE") = dtItemDet.Rows(0).Item("PIECERATE")
                        .Item("STOCKTYPE") = dtItemDet.Rows(0).Item("STOCKTYPE")
                    End If
                Else
                    If Not isOrder Then
                        cmbItem_MAN.Text = .Item("ItemName").ToString
                    End If
                End If
                pFixedVa = IIf(objGPack.GetSqlValue("SELECT ISNULL(FIXEDVA,'N') FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'", , "N") = "N", False, True)
                If .Item("SizeStock") = "Y" Then
                    cmbItemSize.Enabled = True
                    strSql = " SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')"
                    objGPack.FillCombo(strSql, cmbItemSize, , False)
                    If cmbItemSize.Items.Count > 0 Then cmbItemSize.Enabled = True Else cmbItemSize.Enabled = False
                Else
                    cmbItemSize.Text = ""
                    cmbItemSize.Enabled = False
                End If
                If .Item("StockType") = "N" Then
                    MsgBox(E0013, MsgBoxStyle.Exclamation)
                    cmbItem_MAN.Text = ""
                    txtLotNo_Num.Focus()
                    Exit Function
                ElseIf .Item("StockType") = "P" Then
                    MsgBox(E0014, MsgBoxStyle.Exclamation)
                    Return 0
                    Exit Function
                End If
                cmbCostCentre_Man.Text = .Item("DefaultCostCentre").ToString
                If .Item("ValueAddedType") = "T" Then
                    cmbTableCode.Enabled = True
                    strSql = " SELECT DISTINCT TABLECODE FROM " & cnAdminDb & "..WMCTABLE WHERE TABLECODE <> '' "
                    strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_Man.Text & "'),'')"
                    strSql += " ORDER BY TABLECODE"
                    objGPack.FillCombo(strSql, cmbTableCode)
                Else
                    cmbTableCode.Items.Clear()
                    cmbTableCode.Enabled = False
                End If

                If Not isOrder Then cmbDesigner_MAN.Text = .Item("DefaultDesignerName").ToString
                cmbCostCentre_Man.Text = .Item("DefaultCostCentre").ToString
                cmbCounter_MAN.Text = .Item("DefaultCounter").ToString
                cmbItemType_MAN.Text = .Item("ITEMTYPE").ToString

                txtLotNo_Num.Text = .Item("LotNo").ToString
                entryOrder = .Item("EntryOrder")

                dtpRecieptDate.Value = GetEntryDate(GetServerDate)
                lotRecieptDate = .Item("LotDate")

                If Not isOrder Then
                    cmbSubItem_Man.Items.Clear()
                    If objGPack.GetSqlValue("SELECT SUBITEM FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'") = "Y" Then
                        strSql = GetSubItemQry(New String() {"SUBITEMNAME"}, Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'")))
                        objGPack.FillCombo(strSql, cmbSubItem_Man, False)
                        If .Item("SUBITEMNAME").ToString <> "" Then cmbSubItem_Man.Text = .Item("SUBITEMNAME").ToString
                        cmbSubItem_Man.Enabled = True
                    Else
                        cmbSubItem_Man.Enabled = False
                        cmbSubItem_Man.Text = ""
                    End If
                End If
                If cmbSubItem_Man.Text <> "" Then
                    pFixedVa = IIf(objGPack.GetSqlValue("SELECT ISNULL(FIXEDVA,'N') FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "'", , "N") = "N", False, True)
                End If
                If cmbSubItem_Man.Text = "" Then
                    strSql = " SELECT ISNULL(MCCALC,'N') FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"
                Else
                    strSql = " SELECT ISNULL(MCCALC,'N') FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "'"
                End If
                If objGPack.GetSqlValue(strSql, , "N") = "G" Then
                    _MCCALCON_ITEM_GRS = True
                Else
                    _MCCALCON_ITEM_GRS = False
                End If

                If cmbSubItem_Man.Text = "" Then
                    strSql = " SELECT ISNULL(VALUECALC,'N') FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"
                Else
                    strSql = " SELECT ISNULL(VALUECALC,'N') FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "'"
                End If
                If objGPack.GetSqlValue(strSql, , "N") = "G" Then _VALUECALCON_ITEM_GRS = True Else _VALUECALCON_ITEM_GRS = False

                If isOrder Then
                    lblOrder.Visible = True
                    txtOrderNo.Visible = True
                    Dim siz As String = objGPack.GetSqlValue("SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZEID = (SELECT SIZEID FROM " & cnAdminDb & "..ORMAST WHERE SNO = '" & OrderRow.Item("SNO").ToString & "')")
                    If siz <> "" Then cmbItemSize.Text = siz
                    txtOrderNo.Text = objGPack.GetSqlValue("SELECT SUBSTRING(ORNO,6,LEN(ORNO)) FROM " & cnAdminDb & "..ORMAST WHERE SNO = '" & OrderRow.Item("SNO").ToString & "'")
                End If
                txtPurity_Per.Text = Format(Val(.Item("Purity").ToString), "0.00")

                strSql = " select TableCode from " & cnAdminDb & "..itemMast where ItemName = '" & cmbItem_MAN.Text & "' and TableCode <> ''"
                Dim dtTableCode As New DataTable
                dtTableCode.Clear()
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtTableCode)
                If dtTableCode.Rows.Count > 0 Then
                    cmbTableCode.Text = dtTableCode.Rows(0).Item("TableCode").ToString
                End If

                strSql = " select TableCode from " & cnAdminDb & "..ITEMLOT where sno = '" & SNO & "' and ISNULL(TableCode,'') <> ''"
                dtTableCode.Clear()
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtTableCode)
                If dtTableCode.Rows.Count > 0 Then
                    If cmbTableCode.Text <> "" Then
                        cmbTableCode.Text = dtTableCode.Rows(0).Item("TableCode").ToString
                        cmbTableCode.Enabled = False
                    End If
                End If

                lblLotType.Text = .Item("ENTRYTYPE").ToString
                lblFrom.Text = .Item("STARTTAGNO").ToString
                lblTo.Text = .Item("ENDTAGNO").ToString
                lblLastTagNo.Text = .Item("CURTAGNO").ToString

                ''PIECES
                lblPLot.Text = IIf(Val(.Item("PCS").ToString) <> 0, Val(.Item("PCS").ToString), "")
                lblPCompled.Text = IIf(Val(.Item("CPCS").ToString) <> 0, Val(.Item("CPCS").ToString), "")
                lblPBalance.Text = IIf(Val(.Item("BALPCS").ToString) <> 0, Val(.Item("BALPCS").ToString), "")

                ''WEIGHT
                lblWLot.Text = IIf(Val(.Item("GRSWT").ToString) <> 0, Format(Val(.Item("GRSWT").ToString), "0.000"), "")
                lblWCompleted.Text = IIf(Val(.Item("CGRSWT").ToString) <> 0, Format(Val(.Item("CGRSWT").ToString), "0.000"), "")
                lblWBalance.Text = IIf(Val(.Item("BALGRSWT").ToString) <> 0, Format(Val(.Item("BALGRSWT").ToString), "0.000"), "")

                ''
                METALID = .Item("METALID").ToString
                studdedStone = .Item("StuddedStone").ToString
                grossnetdiff = .Item("GROSSNETWTDIFF").ToString
                sizeStock = .Item("SizeStock").ToString
                multiMetal = .Item("Multimetal").ToString
                OthCharge = .Item("OthCharge").ToString
                calType = .Item("CalType").ToString
                noOfPiece = Val(.Item("noOfPiece").ToString)
                If Val(.Item("LotRate").ToString) Then pieceRate = Val(.Item("LotRate").ToString) Else pieceRate = Val(.Item("PieceRate").ToString)
                'CALNO 386
                'cmbCalcMode.Text = .Item("MCCALC").ToString()
                'CALNO 386

                ''PURDETAIL
                lotPurRate = Val(.Item("FINERATE").ToString)
                lotPurTouch = Val(.Item("TUCH").ToString)
                lotPurWastagePer = Val(.Item("WASTPER").ToString)
                lotPurMcPerGrm = Val(.Item("MCGRM").ToString)
                purRate = Val(.Item("FINERATE").ToString)
                ObjPurDetail.txtSaleRate_PER.Text = Val(.Item("SALEPER").ToString)
                ObjPurDetail.txtPurTouch_Amt.Text = IIf(Val(.Item("TUCH").ToString) <> 0, Format(Val(.Item("TUCH").ToString), "0.00"), Nothing)
                ObjPurDetail.txtPurWastage_Per.Text = IIf(Val(.Item("WASTPER").ToString) <> 0, Format(Val(.Item("WASTPER").ToString), "0.00"), Nothing)
                ObjPurDetail.txtPurMcPerGrm_Amt.Text = IIf(Val(.Item("MCGRM").ToString) <> 0, Format(Val(.Item("MCGRM").ToString), "0.00"), Nothing)
                If calType = "R" And purRate <> 0 And Val(.Item("SALEPER").ToString) <> 0 Then txtRate_Amt.Text = purRate + (purRate * (Val(ObjPurDetail.txtSaleRate_PER.Text) / 100))
            End With
        Else
            cmbItem_MAN.Text = ""
            pFixedVa = False
            cmbSubItem_Man.Items.Clear()
            cmbSubItem_Man.Text = ""
            cmbSubItem_Man.Enabled = False

            lblLotType.Text = ""
            lblFrom.Text = ""
            lblTo.Text = ""
            lblLastTagNo.Text = ""
            ''Pieces
            lblPLot.Text = ""
            lblPCompled.Text = ""
            lblPBalance.Text = ""

            ''Weight
            lblWLot.Text = ""
            lblWCompleted.Text = ""
            lblWBalance.Text = ""

            ''PUR DETAIL
            ObjPurDetail.txtPurTouch_Amt.Clear()
            ObjPurDetail.txtPurWastage_Per.Clear()
            ObjPurDetail.txtPurMcPerGrm_Amt.Clear()

        End If
        Return 1
    End Function

    Private Sub txtLotNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLotNo_Num.GotFocus
        TabControl1.SelectedTab = tabTagDet
    End Sub

    Private Sub txtLotNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtLotNo_Num.KeyDown
        Dim mLotchk As String = ""
        If e.KeyCode = Keys.Insert Then

            If Lotchkdate = True Then

                objdatescr.Label1.Text = "Lot Receipt Date"
                objdatescr.ShowDialog()
                mLotchk = " AND LOTDATE ='" & objdatescr.dtpBillDatef.Value.ToString("yyyy-MM-dd") & "' "
            End If

            strSql = " SELECT "
            strSql += vbCrLf + " LOTNO,LOTDATE"
            strSql += vbCrLf + " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = LOT.DESIGNERID)AS DESIGNER"
            strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS IM WHERE IM.ITEMID = LOT.ITEMID)AS ITEM"
            strSql += vbCrLf + " ,ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS SM WHERE SM.SUBITEMID = LOT.SUBITEMID),'')AS SUBITEM"
            strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = LOT.COSTID)AS COSTCENTRE"
            strSql += vbCrLf + " ,PCS,GRSWT,CPCS AS CPCS,CGRSWT AS CGRSWT,(PCS-CPCS)AS BALPCS,(GRSWT-CGRSWT)AS BALGRSWT"
            strSql += vbCrLf + " ,(SELECT (CASE WHEN STOCKTYPE = 'T' THEN 'TAGED' WHEN STOCKTYPE = 'N' THEN 'NON TAGED' ELSE 'PACKET BASED' END) FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = LOT.ITEMID)AS STOCKTYPE"
            strSql += vbCrLf + " ,CASE ENTRYTYPE WHEN 'R' THEN 'REGULAR' WHEN 'OR' THEN 'ORDER' WHEN 'RE' THEN 'ORDER' ELSE ENTRYTYPE END AS LOTTYPE"
            strSql += vbCrLf + " ,SNO,FROMITEMID"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMLOT AS LOT"
            strSql += vbCrLf + " WHERE "

            If tabCheckBy = "P" Then
                strSql += vbCrLf + " PCS > CPCS "
            Else
                strSql += vbCrLf + " ((GRSWT > CGRSWT) or (rate <> 0 and pcs > cpcs))"
            End If
            If mLotchk <> "" Then strSql += vbCrLf + mLotchk
            strSql += vbCrLf + " AND ISNULL(COMPLETED,'') <> 'Y'"
            strSql += vbCrLf + " AND (SELECT STOCKTYPE FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = LOT.ITEMID) = 'T'"
            strSql += vbCrLf + " AND LOTNO LIKE '" & txtLotNo_Num.Text & "%'"
            strSql += vbCrLf + " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T')"
            strSql += vbCrLf + " AND ISNULL(BULKLOT,'') <> 'Y'"
            strSql += vbCrLf + " ORDER BY LOTDATE,LOTNO "
            SNO = BrighttechPack.SearchDialog.Show("Searching LotNo", strSql, cn, 1, 14)
            strSql = " SELECT LOTNO FROM " & cnAdminDb & "..ITEMLOT"
            strSql += vbCrLf + " WHERE SNO = '" & SNO & "'"
            Dim dt As DataTable
            dt = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                txtLotNo_Num.Text = dt.Rows(0).Item("LotNo").ToString
            Else
                Exit Sub
            End If
            If LoadLotDetails() = False Then Exit Sub
            txtLotNo_Num.Enabled = False
            'strSql = funcAssignTagDefaultVal()
            'dt = New DataTable
            'da = New OleDbDataAdapter(strSql, cn)
            'da.Fill(dt)
            'funcAssignVal(dt)
            'If cmbSubItem_Man.Enabled Then cmbSubItem_SelectedIndexChanged(Me, New EventArgs)
            'funcAssignTabControls()
            'ORDetail()
            'txtLotNo_Num_Man.Enabled = False
        End If
    End Sub

    Private Function ORDetail() As Boolean
        Dim entryType As String = UCase(objGPack.GetSqlValue("SELECT ENTRYTYPE FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = '" & SNO & "'")) '' LOTNO = " & Val(txtLotNo_Num_Man.Text) & "
        Dim dr_Ord As DataRow
        If entryType = "OR" Or entryType = "RE" Then
            Dim mcostid As String = UCase(objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = '" & SNO & "'"))
            Dim OrderNo As String = Nothing
            strSql = " SELECT ORDREPNO FROM " & cnAdminDb & "..ITEMLOT WHERE ISNULL(LOTNO,'') = '" & txtLotNo_Num.Text & "'"
            strSql += " AND SNO = '" & SNO & "'"
            If mcostid <> "" Then strSql += " AND COSTID='" & mcostid & "'"
            Dim dtOrdLot As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtOrdLot)
            If dtOrdLot.Rows.Count > 0 Then
                If entryType = "OR" Then
                    Dim orNo As String() = dtOrdLot.Rows(0).Item("ORDREPNO").ToString.Split("O")
                    If orNo.Length > 1 Then OrderNo = "O" + orNo(1).ToString
                    If orNo.Length > 2 Then OrderNo = "O" + orNo(2).ToString
                Else
                    Dim orNo1 As String() = dtOrdLot.Rows(0).Item("ORDREPNO").ToString.Split("R")
                    If orNo1.Length > 1 Then OrderNo = "R" + orNo1(1).ToString
                    If orNo1.Length > 2 Then OrderNo = "R" + orNo1(2).ToString
                End If
            End If
            If OrderNo Is Nothing Or OrderNo = "" Then
                strSql = " SELECT DISTINCT SUBSTRING(ORNO,6,20) ORNO,ORDATE TRANDATE,SUBSTRING(ORNO,1,2) COSTID ,SUBSTRING(ORNO,3,3) COMPANYID FROM " & cnAdminDb & "..ORMAST AS I"
                strSql += " WHERE ISNULL(ORDCANCEL,'') <> 'Y' AND NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSTATUS = 'R' AND ORSNO = I.SNO  AND ISNULL(CANCEL,'') = '' AND ORDSTATE_ID = 4)"
                strSql += " AND I.COMPANYID = '" & strCompanyId & "'"
                If mcostid <> "" And CHKCOST_ORD = True Then strSql += " AND I.COSTID='" & mcostid & "'"
                If entryType = "OR" Then
                    strSql += " AND SUBSTRING(ORNO,6,1) = 'O'"
                    dr_Ord = BrighttechPack.SearchDialog.Show_R("Select Order No", strSql, cn, , , , , , False)
                Else
                    strSql += " AND SUBSTRING(ORNO,6,1) = 'R'"
                    dr_Ord = BrighttechPack.SearchDialog.Show_R("Select Repair No", strSql, cn, , , , , , False)
                End If
                If dr_Ord Is Nothing Then
                    MsgBox("Invalid Order Selection", MsgBoxStyle.Information)
                    btnNew_Click(Me, New EventArgs)
                    grpSaveControls.Select()
                    Return False
                End If
                OrderNo = dr_Ord.Item("ORNO").ToString 'dr_Ord.Item("COSTID").ToString + dr_Ord.Item("COMPANYID").ToString + dr_Ord.Item("ORNO").ToString
            Else
                'strSql = " SELECT TOP 1 ISNULL(ORNO,'') ORNO FROM " & cnadmindb & "..ORIRDETAIL AS I"
                'strSql += " WHERE NOT EXISTS (sELECT 1 FROM " & cnadmindb & "..ORIRDETAIL WHERE ORSTATUS = 'R' AND ORSNO = I.ORSNO  AND ISNULL(CANCEL,'') = '' AND ORDSTATE_ID = 4)"
                strSql = " SELECT DISTINCT SUBSTRING(ORNO,6,20) ORNO,ORDATE TRANDATE,SUBSTRING(ORNO,1,2) COSTID ,SUBSTRING(ORNO,3,3) COMPANYID FROM " & cnAdminDb & "..ORMAST AS I"
                strSql += " WHERE NOT EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSTATUS = 'R' AND ORSNO = I.SNO  AND ISNULL(CANCEL,'') = '' AND ORDSTATE_ID = 4)"
                strSql += " AND I.COMPANYID = '" & strCompanyId & "'"
                If mcostid <> "" And CHKCOST_ORD = True Then strSql += " AND I.COSTID='" & mcostid & "'"
                If entryType = "OR" Then
                    strSql += " AND SUBSTRING(ORNO,6,1) = 'O'"
                Else
                    strSql += " AND SUBSTRING(ORNO,6,1) = 'R'"
                End If
                If Mid(OrderNo, 6, 1) = "O" Or Mid(OrderNo, 6, 1) = "R" Then
                    strSql += " AND isnull(ORNO,'') ='" & OrderNo & "'"
                Else
                    strSql += " AND SUBSTRING(ORNO, 6, 20) ='" & OrderNo & "'"
                End If
                ''strSql += " AND ISNULL(ORNO,'') = '" & OrderNo & "'"
                If BrighttechPack.GetSqlValue(cn, strSql, "ORNO", "") = "" Then
                    MsgBox("Invalid Order or Order Already Alotted", MsgBoxStyle.Information)
                    btnNew_Click(Me, New EventArgs)
                    grpSaveControls.Select()
                    Return False
                End If
            End If
            'CALNO 280913
            If CHKSMITHREC = True Then
                strSql = " SELECT DISTINCT SUBSTRING(ORNO,6,20) ORNO,ORDATE TRANDATE,SUBSTRING(ORNO,1,2) COSTID ,SUBSTRING(ORNO,3,3) COMPANYID FROM " & cnAdminDb & "..ORMAST AS I"
                strSql += " WHERE EXISTS (SELECT 1 FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSTATUS = 'R' AND ORSNO = I.SNO  AND ISNULL(CANCEL,'') = '')"
                strSql += " AND I.COMPANYID = '" & strCompanyId & "'"
                If entryType = "OR" Then
                    strSql += " AND SUBSTRING(ORNO,6,1) = 'O'"
                Else
                    strSql += " AND SUBSTRING(ORNO,6,1) = 'R'"
                End If
                If Mid(OrderNo, 6, 1) = "O" Or Mid(OrderNo, 6, 1) = "R" Then
                    strSql += " AND isnull(ORNO,'') ='" & OrderNo & "'"
                Else
                    strSql += " AND SUBSTRING(ORNO, 6, 20) ='" & OrderNo & "'"
                End If
                If mcostid <> "" And CHKCOST_ORD = True Then strSql += " AND COSTID='" & mcostid & "'"
                ''strSql += " AND ISNULL(ORNO,'') = '" & OrderNo & "'"
                If BrighttechPack.GetSqlValue(cn, strSql, "ORNO", "") = "" Then
                    MsgBox("Cannot tag this Order, because Smith Receipt Record Not Found", MsgBoxStyle.Information)
                    btnNew_Click(Me, New EventArgs)
                    grpSaveControls.Select()
                    Return False
                End If
            End If

            If OrderNo.Length > 0 Then
                LoadOrderDetails(OrderNo, mcostid)
            Else
                MsgBox("Smith Issued Record Not Found", MsgBoxStyle.Information)
                btnNew_Click(Me, New EventArgs)
                grpSaveControls.Select()
            End If
            Return True
        End If
        Return False
    End Function

    Private Sub txtTagNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTagNo__Man.GotFocus
        Dim tagPrefix As String
        Dim SEAL As String = wtxtTagNo__Man.Text
        Dim ItemSname As String = ""
        Dim Styleno As String = ""
        Dim uniqueid As String = ""
        'SEAL = objGPack.GetSqlValue("SELECT SUBSTRING(SEAL,1,2) SEAL FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID=" & designerId & "", , "00", tran)
        'ItemSname = objGPack.GetSqlValue("SELECT SUBSTRING(SHORTNAME,1,2) SNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItem_MAN.Text & "'", , "00", tran)
        Styleno = "0" '.Cells("STYLENO").Value.ToString

        Dim stnSHAPEid, stnColorid, stnClarityid As Integer
        stnSHAPEid = 0 : stnColorid = 0 : stnClarityid = 0
        Dim drsstone() As DataRow = dtWStoneDetails.Select("", "")
        If drsstone.Length > 0 Then
            stnSHAPEid = Val(objGPack.GetSqlValue("SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME='" & drsstone(0).Item("SHAPE").ToString & "' ", , 0, tran).ToString)
            stnColorid = Val(objGPack.GetSqlValue("SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME='" & drsstone(0).Item("COLOR").ToString & "' ", , 0, tran).ToString)
            stnClarityid = Val(objGPack.GetSqlValue("SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME='" & drsstone(0).Item("CLARITY").ToString & "' ", , 0, tran).ToString)
        End If
        strSql = "SELECT TOP 1 UNIQUEID FROM " & cnAdminDb & "..DIASTYLE "
        strSql += " WHERE SHAPEID = " & stnSHAPEid & ""
        strSql += " AND COLORID = " & stnColorid & ""
        strSql += " AND CLARITYID = " & stnClarityid & ""
        uniqueid = objGPack.GetSqlValue(strSql, , "0", tran)

        'tagPrefix = SEAL & WGetidwithlen(Styleno, 6) & uniqueid
        tagPrefix = SEAL & uniqueid
        strSql = " SELECT LASTNO FROM " & cnAdminDb & "..DIAUNIQUETAG WHERE UNQDESC='" & tagPrefix & "'"
        LastTagNo = objGPack.GetSqlValue(strSql, , "0", tran)

        LastTagNo = (Val(LastTagNo) + 1)
        'for update
        MLasttagno = LastTagNo
        MTagprefix = tagPrefix

        txtTagNo__Man.Text = tagPrefix & WGetidwithlen(LastTagNo.ToString, 4)

        If GetAdmindbSoftValue("TAGGENLOCK") = "Y" Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cmbSubItem_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubItem_Man.SelectedIndexChanged
        funcStoneDetailsClear()
        funcDiamondDetailsClear()
        funcPreciousDetailsClear()
        txtMiscAmt.Text = ""
        txtMultiWt.Text = ""

        strSql = " select CalType,StuddedStone,OthCharge,PieceRate,isnull(fixedva,'N') as fixedva,mcasvaper from " & cnAdminDb & "..Subitemmast"
        strSql += " where subitemname = '" & cmbSubItem_Man.Text & "'"
        strSql += " and itemid in(select itemid from " & cnAdminDb & "..Itemmast where Itemname ='" & cmbItem_MAN.Text & "')"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            With dt.Rows(0)
                calType = .Item("Caltype")
                studdedStone = .Item("StuddedStone")
                OthCharge = .Item("OthCharge")
                pFixedVa = IIf(.Item("FIXEDVA").ToString = "N", False, True)
                If .Item("mcasvaper").ToString = "Y" Then Label44.Text = "MC PERCENT" Else Label44.Text = "Max Mc Per Gram"
            End With
            'funcAssignTabControls()
            CalcMaxMinValues()
        End If
    End Sub

    Private Sub txtPieces_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPieces_Num_Man.GotFocus
        If AddStockEntry = True And tagEdit = True Then AddStockEntry = False : LoadAdditionalStockEntry()
        If noOfPiece <> 0 Then
            txtPieces_Num_Man.Text = noOfPiece.ToString
        Else
            txtPieces_Num_Man.Text = "1"
        End If
    End Sub

    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
        If TabControl1.SelectedTab.Name = tabPurchase.Name Then
            'grpSaveControls.Size = New Size(972, 290)
        Else
            grpSaveControls.Size = New Size(972, 435)
        End If
    End Sub

    Private Sub cmbCalcMode_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCalcMode.GotFocus
        'If tagEdit = False Then Me.cmbCalcMode_SelectedIndexChanged(Me, New System.EventArgs)
        Me.cmbCalcMode_SelectedIndexChanged(Me, New System.EventArgs)
        If ALLOYGOLD_SALEMODE Then SendKeys.Send("{TAB}") : Exit Sub
        'CALNO 386
        If Val(txtGrossWt_Wet.Text) = Val(txtNetWt_Wet.Text) Then
            If cmbCalcMode.Text = "NET WT" Then cmbCalcMode.Text = "GRS WT"
            SendKeys.Send("{TAB}") ' Me.SelectNextControl(cmbCalcMode, True, True, True, True)
        ElseIf Val(txtGrossWt_Wet.Text) > Val(txtNetWt_Wet.Text) Then
            cmbCalcMode.Text = "NET WT"
        End If

    End Sub

    Private Sub cmbCalcMode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCalcMode.SelectedIndexChanged
        If cmbTableCode.Enabled = True Then
            Exit Sub
        End If
        CalcMaxMinValues()
        CalcFinalTotal()
    End Sub

    Private Sub txtMaxWastagePer_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMaxWastage_Per.TextChanged
        Dim wt As Double = Nothing
        Dim mweight As Double
        If cmbCalcMode.SelectedIndex = 0 Then ''GRS WT
            mweight = Val(txtGrossWt_Wet.Text)
        Else ''NET WT
            mweight = Val(txtNetWt_Wet.Text)
        End If
        If Not _WASTONGRSNET Then
            mweight = IIf(_MCCALCON_ITEM_GRS, Val(txtGrossWt_Wet.Text), Val(txtNetWt_Wet.Text))
        End If
        wt = mweight * (Val(txtMaxWastage_Per.Text) / 100)
        wt = Math.Round(wt, WastageRound)
        txtMaxWastage_Wet.Text = IIf(wt <> 0, Format(wt, "0.000"), "")
    End Sub

    Private Sub txtMaxMcPerGrm_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMaxMcPerGrm_Amt.TextChanged
        Dim mc As Double = Nothing
        Dim wast As Double = IIf(McWithWastage, Val(txtMaxWastage_Wet.Text), 0)
        Dim mweight As Double = 0
        If _MCONGRSNET Then
            If cmbCalcMode.SelectedIndex = 0 Then ''GRS WT
                mweight = (Val(txtGrossWt_Wet.Text) + wast)
            Else ''NET WT
                mweight = (Val(txtNetWt_Wet.Text) + wast)
            End If
        Else
            mweight = IIf(_MCCALCON_ITEM_GRS, Val(txtGrossWt_Wet.Text), Val(txtNetWt_Wet.Text)) + wast
        End If
        strSql = " SELECT mcasvaper FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"
        If objGPack.GetSqlValue(strSql).ToString = "Y" Then Label44.Text = "MC PERCENT" Else Label44.Text = "Max Mc Per Gram"
        If cmbSubItem_Man.Text <> "" Then
            strSql = " SELECT mcasvaper FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & cmbSubItem_Man.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')"
            If objGPack.GetSqlValue(strSql).ToString = "Y" Then Label44.Text = "MC PERCENT" Else Label44.Text = "Max Mc Per Gram"
        End If
        If Label44.Text = "MC PERCENT" Then mc = mweight * (Val(txtMetalRate_Amt.Text) * (Val(txtMaxMcPerGrm_Amt.Text) / 100)) Else mc = mweight * Val(txtMaxMcPerGrm_Amt.Text)
        mc = Math.Round(mc, McRnd)
        txtMaxMkCharge_Amt.Text = IIf(mc <> 0, Format(mc, "0.00"), "")
    End Sub

    Private Sub txtSalValue_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSalValue_Amt_Man.GotFocus
        If Val(txtRate_Amt.Text) = 0 Then Ratevaluezero = True
        If purSalevalue > 0 Then
            If txtSalValue_Amt_Man.Text < purSalevalue Then
                txtSalValue_Amt_Man.Text = SALEVALUE_ROUND(purSalevalue)
            End If
        Else
            CalcFinalTotal(BlockSv)
            If cmbSubItem_Man.Text <> "" Then
                strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')"
            Else
                strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"
            End If
            txtSalValue_Amt_Man.ReadOnly = False
            Dim calcType As String = objGPack.GetSqlValue(strSql)
            If calcType <> "F" Then
                If SalVal_Lock = True Then
                    txtSalValue_Amt_Man.ReadOnly = True
                Else
                    txtSalValue_Amt_Man.ReadOnly = False
                End If
            End If
        End If
    End Sub
    Private Function SALEVALUE_ROUND(ByVal svalue As Decimal) As Decimal
        If SALVALUEROUND <> 0 Then
            If svalue <> 0 Then
                Dim wholepart As Decimal = Val(svalue) / SALVALUEROUND
                Dim intpart As Decimal = Int(wholepart)
                Dim decpart As Decimal = Round(wholepart - intpart)
                svalue = (intpart + decpart) * SALVALUEROUND
            End If
        End If
        Return svalue
    End Function
    Private Sub TagEditSave()
        'Dim TagSno As Integer = 0
        'Dim TagNo As String = Nothing
        'Dim tagVal As Integer = 0
        Dim RowFiter() As DataRow = Nothing
        Dim COSTID As String = Nothing
        Dim itemId As Integer = Nothing
        Dim subitemId As Integer = Nothing
        Dim sizeId As Integer = Nothing
        Dim itemCtrId As Integer = Nothing
        Dim designerId As Integer = Nothing
        'Dim saleMode As String = Nothing
        Dim itemTypeId As Integer = Nothing
        Dim tranInvNo As String = Nothing
        Dim supBillno As String = Nothing

        Dim stlPcs As Integer = 0
        Dim stlWt As Double = 0
        Dim stlType As String = Nothing

        Dim dialPcs As Integer = 0
        Dim dialWt As Double = 0

        'Try
        'tran = Nothing
        'tran = cn.BeginTransaction()
        ' ''Find TagSno
        'strSql = " SELECT CONVERT(INT,CTLTEXT)+1 AS TAGSNO FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'TAGSNO'"
        'TagSno = objGPack.GetSqlValue(strSql, "TAGSNO", , tran)

        ''Find COSTID
        strSql = " SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME  = '" & cmbCostCentre_Man.Text & "'"
        COSTID = objGPack.GetSqlValue(strSql, "COSTID", , tran)

        ''Find ItemId
        strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItem_MAN.Text & "'"
        itemId = Val(objGPack.GetSqlValue(strSql, "ITEMID", , tran))

        If cmbSubItem_Man.Enabled = True Then
            ''Find SubItemId
            strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "' AND ITEMID = " & itemId & ""
            subitemId = Val(objGPack.GetSqlValue(strSql, "SUBITEMID", , tran))
        End If

        If cmbItemSize.Enabled = True Then
            ''Find ItemSize
            strSql = " SELECT SIZEID FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZENAME = '" & cmbItemSize.Text & "'"
            sizeId = Val(objGPack.GetSqlValue(strSql, "SIZEID", , tran))
        End If

        If cmbCounter_MAN.Enabled = True Then
            ''Find ItemCounter
            strSql = " SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbCounter_MAN.Text & "'"
            itemCtrId = Val(objGPack.GetSqlValue(strSql, "ITEMCTRID", "", tran))
        End If

        ''Find DesignerId
        strSql = " SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesigner_MAN.Text & "'"
        designerId = Val(objGPack.GetSqlValue(strSql, "DESIGNERID", "", tran))

        ''Find Stone & Diamond
        If TabControl1.TabPages.Contains(tabStone) = True Then
            stlPcs += Val(lblStnPcs.Text)
            stlWt += (Val(lblStnCaratWt.Text) / 5) + Val(lblStnGramWt.Text)

            stlPcs += Val(lblPrePcs.Text)
            stlWt += (Val(lblPreCarat.Text) / 5) + Val(lblPreGram.Text)

            dialPcs += Val(lblDiaPcs.Text)
            dialWt += (Val(lblDiaCarat.Text) / 5) + Val(lblDiaGram.Text)

            stlType = "G"
            stlWt = Math.Round(stlWt, 3)
            dialWt = Math.Round(dialWt, 3)
        Else
            stlPcs = 0
            stlWt = 0
            stlType = "G"
        End If
        ''Find itemTypeId
        strSql = " SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType_MAN.Text & "'"
        itemTypeId = Val(objGPack.GetSqlValue(strSql, "ITEMTYPEID", , tran))

        ''FIND TRANINVNO AND SUPBILLNO
        strSql = " SELECT TRANINVNO FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = '" & SNO & "'"
        tranInvNo = objGPack.GetSqlValue(strSql, "TRANINVNO", , tran)
        strSql = " SELECT BILLNO FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = '" & SNO & "'"
        supBillno = objGPack.GetSqlValue(strSql, "BILLNO", , tran)

        Dim purStnValue As Double
        For Each roStn As DataRow In dtStoneDetails.Rows
            purStnValue += Val(roStn!PURVALUE.ToString)
        Next

        strSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET "
        strSql += " RECDATE = '" & dtpRecieptDate.Value.Date.ToString("yyyy-MM-dd") & "'" 'RECDATE
        strSql += " ,COSTID = '" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
        strSql += " ,SUBITEMID = " & subitemId & "" 'SUBITEMID
        strSql += " ,SIZEID = " & sizeId & "" 'SIZEID
        strSql += " ,ITEMCTRID = " & itemCtrId & "" 'ITEMCTRID
        strSql += " ,TABLECODE = '" & cmbTableCode.Text & "'"
        'If cmbTableCode.Enabled = True Then 'TABLECODE
        'Else
        '    strSql += " ,TABLECODE = ''"
        'End If
        strSql += " ,DESIGNERID = " & Val(designerId) & "" 'DESIGNERID
        strSql += " ,PCS = " & Val(txtPieces_Num_Man.Text) & "" 'PCS
        strSql += " ,GRSWT = " & Val(txtGrossWt_Wet.Text) & "" 'GRSWT
        strSql += " ,LESSWT = " & Val(txtLessWt_Wet.Text) & "" 'LESSWT
        strSql += " ,NETWT = " & Val(txtNetWt_Wet.Text) & "" 'NETWT
        If txtRate_Amt.Enabled = True Then
            strSql += " ,RATE = " & Val(txtRate_Amt.Text) & "" 'RATE
        Else
            strSql += ",RATE = 0"
        End If
        strSql += " ,MAXWASTPER = " & Val(txtMaxWastage_Per.Text) & "" 'MAXWASTPER
        strSql += " ,MAXMCGRM = " & Val(txtMaxMcPerGrm_Amt.Text) & "" 'MAXMCGRM
        strSql += " ,MAXWAST = " & Val(txtMaxWastage_Wet.Text) & "" 'MAXWAST
        strSql += " ,MAXMC = " & Val(txtMaxMkCharge_Amt.Text) & "" 'MAXMC
        strSql += " ,MINWASTPER = " & Val(ObjMinValue.txtMinWastage_Per.Text) & "" 'MINWASTPER
        strSql += " ,MINMCGRM = " & Val(ObjMinValue.txtMinMcPerGram_Amt.Text) & "" 'MINMCGRM
        strSql += " ,MINWAST = " & Val(ObjMinValue.txtMinWastage_Wet.Text) & "" 'MINWAST
        strSql += " ,MINMC = " & Val(ObjMinValue.txtMinMkCharge_Amt.Text) & "" 'MINMC
        strSql += " ,COMPANYID =  '" & GetStockCompId() & "'" 'COMPANYID
        strSql += " ,SALVALUE = " & Val(txtSalValue_Amt_Man.Text) & "" 'SALVALUE
        strSql += " ,PURITY = " & Val(txtPurity_Per.Text) & "" 'PURITY
        strSql += " ,NARRATION = '" & txtNarration.Text & "'" 'NARRATION
        If cmbSubItem_Man.Enabled = True Then 'DESCRIP
            strSql += " ,DESCRIP = '" & cmbSubItem_Man.Text & "'"
        Else
            strSql += " ,DESCRIP = '" & cmbItem_MAN.Text & "'"
        End If
        If chkAutomaticWt.Checked = True Then 'ENTRYMODE
            strSql += " ,ENTRYMODE = 'A'"
        Else
            strSql += " ,ENTRYMODE = 'M'"
        End If
        If cmbCalcMode.Enabled = True Then
            strSql += " ,GRSNET = '" & Mid(cmbCalcMode.Text, 1, 1) & "'" 'GRSNET
        Else
            strSql += " ,GRSNET = '" & Mid(cmbCalcMode.Text, 1, 1) & "'" 'GRSNET
            'strSql += " ,GRSNET = ''" 'GRSNET
        End If
        strSql += " ,SALEMODE = '" & calType & "'"

        If picPath Is Nothing Or picPath = "" Then
            strSql += " ,PCTFILE = ''"
        Else
            strSql += " ,PCTFILE = '" & IIf(picPath <> "", "L" + txtLotNo_Num.Text + "I" + itemId.ToString + "T" + txtTagNo__Man.Text.Replace(":", "") + IIf(picExtension.ToString.StartsWith("."), picExtension.ToString, "." + picExtension.ToString), picPath) & "'"
        End If

        strSql += " ,ITEMTYPEID = " & Val(itemTypeId) & ""
        '24-07-13 Alter by vasanth
        'strSql += " ,ACTUALRECDATE = '" & dtpRecieptDate.Value.Date.ToString("yyyy-MM-dd") & "'"
        strSql += " ,TRANINVNO = '" & tranInvNo & "'"
        strSql += " ,SUPBILLNO = '" & supBillno & "'"
        strSql += " ,USERID = " & userId & ""
        strSql += " ,UPDATED = '" & Today.Date.ToString("yyyy-MM-dd") & "'"
        strSql += " ,UPTIME = '" & Date.Now.ToLongTimeString & "'"
        strSql += " ,SYSTEMID = '" & systemId & "'"
        '--LOTENTRYORDER
        strSql += " ,STYLENO = '" & txtStyleCode.Text & "'"
        strSql += " ,BOARDRATE = " & Val(txtMetalRate_Amt.Text) & ""
        strSql += " ,TRANSFERDATE = '" & dtpRecieptDate.Value.Date.ToString("yyyy-MM-dd") & "'" 'RECDATE
        strSql += " ,RFID = '" & txtRfId.Text & "'" 'RFID
        strSql += " ,TOUCH = " & Val(txtTouch_AMT.Text) & "" 'TOUCH
        strSql += " ,HM_BILLNO = '" & txtHmBillNo.Text & "'" 'HM_BILLNO
        strSql += " ,HM_CENTER = '" & txtHmCentre.Text & "'" 'HM_CENTER
        strSql += " ,ADD_VA_PER = " & Val(ObjPurDetail.txtpURFixedValueVa_AMT.Text) & "" 'ADD_VA_PER
        strSql += " ,REFVALUE = " & Val(txtRefVal_AMT.Text) & "" 'REFVALUE
        strSql += " ,EXTRAWT = " & Val(ObjExtraWt.txtExtraWt_WET.Text) & "" 'EXTRAWT
        strSql += " ,USRATE = " & IIf(NEEDUS = True And Studded_Loose = "L", Val(ObjRsUs.txtUSDollar_Amt.Text), 0) & "" 'USRATE
        strSql += " ,INDRS = " & IIf(NEEDUS = True And Studded_Loose = "L", Val(ObjRsUs.txtIndRs_Amt.Text), 0) & "" 'INDRS
        If _FourCMaintain Then
            strSql += " ,CUTID=" & TagCutId
            strSql += " ,COLORID=" & TagColorId
            strSql += " ,CLARITYID=" & TagClarityId
            strSql += " ,SHAPEID=" & TagShapeId
            strSql += " ,SETTYPEID=" & TagSetTypeId
            strSql += " ,HEIGHT=" & TagHeight
            strSql += " ,WIDTH=" & TagWidth
        End If
        strSql += " WHERE SNO = '" & updIssSno & "'"
        Dim fileDestPath As String = Nothing
        If File.Exists(defalutDestination + picPath) = True Then
            Dim serverPath As String = Nothing
            fileDestPath = (defalutDestination + "L" + txtLotNo_Num.Text + "I" + itemId.ToString + "T" + txtTagNo__Man.Text.Replace(":", "") + IIf(picExtension.ToString.StartsWith("."), picExtension.ToString, "." + picExtension.ToString))
            Dim Finfo As FileInfo
            Finfo = New FileInfo(fileDestPath)
            If IO.Directory.Exists(Finfo.Directory.FullName) = False Then
                MsgBox(Finfo.Directory.FullName & " Does not exist. Please make a corresponding path", MsgBoxStyle.Information)
                tran.Rollback()
                Exit Sub
            End If
            If UCase(defalutDestination + picPath) <> fileDestPath.ToUpper Then
                Dim cFile As New FileInfo(defalutDestination + picPath)
                cFile.CopyTo(fileDestPath, True)
                'File.Copy(picPath, fileDestPath)
            End If
        End If
        'cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId, , fileDestPath)
        TagCutId = 0 : TagColorId = 0 : TagClarityId = 0 : TagShapeId = 0 : TagSetTypeId = 0 : TagHeight = 0 : TagWidth = 0
        If _HasPurchase Or pFixedVa Or PUR_AUTOCALC Then
            ''DELETING PURSTONEDETAIL
            strSql = " DELETE FROM " & cnAdminDb & "..PURITEMTAG"
            strSql += " WHERE TAGSNO = '" & updIssSno & "'"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
        End If
        If _HasPurchase Or pFixedVa Or PUR_AUTOCALC Then
            ''ITEM PUR DETAIL
            strSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..PURITEMTAG"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " TAGSNO,ITEMID,TAGNO,PURLESSWT,PURNETWT,PURRATE,PURGRSNET"
            strSql += vbCrLf + " ,PURWASTAGE,PURTOUCH,PURMC,PURVALUE,PURTAX,RECDATE,COMPANYID,COSTID"
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " VALUES"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " '" & updIssSno & "'" 'TAGSNO
            strSql += vbCrLf + " ," & itemId & "" 'ITEMID
            strSql += vbCrLf + " ,'" & txtTagNo__Man.Text & "'" 'TAGNO
            strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurLessWt_Wet.Text) & "" ' PURLESSWT
            strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurNetWt_Wet.Text) & "" ' PURNETWT"
            strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurRate_Amt.Text) & "" ' PURRATE"
            strSql += vbCrLf + " ,'" & Mid(ObjPurDetail.cmbPurCalMode.Text, 1, 1) & "'" ' PURGRSNET"
            strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurWastage_Wet.Text) & "" ' PURWASTAGE"
            strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurTouch_Amt.Text) & "" ' PURTOUCH"
            strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurMakingChrg_Amt.Text) & "" ' PURMC"
            strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurPurchaseVal_Amt.Text) & "" ' PURVALUE"
            strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurTax_AMT.Text) & ""
            strSql += vbCrLf + " ,'" & ObjPurDetail.dtpPurchaseDate.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " ,'" & GetStockCompId() & "'"
            strSql += vbCrLf + " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'"
            strSql += vbCrLf + " )"
            'cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
        End If

        If _HasPurchase Or pFixedVa Or PUR_AUTOCALC Then
            ''DELETING PURSTONEDETAIL
            strSql = " DELETE FROM " & cnAdminDb & "..PURITEMTAGSTONE"
            strSql += " WHERE TAGSNO = '" & updIssSno & "'"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            'cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
        End If
        ''DELETING STONEDETAIL
        strSql = " DELETE FROM " & cnAdminDb & "..ITEMTAGSTONE"
        strSql += " WHERE TAGSNO = '" & updIssSno & "'"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

        ''Inserting StoneDetail
        If TabControl1.TabPages.Contains(tabStone) Then
            For cnt As Integer = 0 To dtStoneDetails.Rows.Count - 1
                With dtStoneDetails.Rows(cnt)
                    Dim CutId As Integer = 0
                    Dim ColorId As Integer = 0
                    Dim ClarityId As Integer = 0
                    Dim ShapeId As Integer = 0
                    Dim SetTypeId As Integer = 0
                    Dim stnItemId As Integer = 0
                    Dim stnSubItemId As Integer = 0
                    Dim stnSno As String = GetNewSno(TranSnoType.ITEMTAGSTONECODE, tran, "GET_ADMINSNO_TRAN")
                    .Item("STNSNO") = stnSno
                    'Dim caType As String = Nothing
                    strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Item("ITEM").ToString & "'"
                    stnItemId = Val(objGPack.GetSqlValue(strSql, "ITEMID", , tran))
                    strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & .Item("SUBITEM").ToString & "' AND ITEMID = " & stnItemId & ""
                    stnSubItemId = Val(objGPack.GetSqlValue(strSql, , , tran))
                    ''Inserting itemTagStone
                    strSql = " INSERT INTO " & cnAdminDb & "..ITEMTAGSTONE("
                    strSql += " SNO,TAGSNO,ITEMID,COMPANYID,STNITEMID,STNSUBITEMID,TAGNO,STNPCS,STNWT,"
                    strSql += " STNRATE,STNAMT,DESCRIP,RECDATE,CALCMODE,MINRATE,SIZECODE,STONEUNIT,ISSDATE,"
                    strSql += " OLDTAGNO,CARRYFLAG,COSTID,SYSTEMID,APPVER,USRATE,INDRS,PACKETNO"
                    If _FourCMaintain Then
                        strSql += " ,CUTID,COLORID,CLARITYID,SHAPEID,SETTYPEID,HEIGHT,WIDTH"
                        CutId = Val(objGPack.GetSqlValue(" SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & .Item("CUT").ToString & "'", "CUTID", , tran))
                        ColorId = Val(objGPack.GetSqlValue(" SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME = '" & .Item("COLOR").ToString & "'", "COLORID", , tran))
                        ClarityId = Val(objGPack.GetSqlValue(" SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & .Item("CLARITY").ToString & "'", "CLARITYID", , tran))
                        ShapeId = Val(objGPack.GetSqlValue(" SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & .Item("SHAPE").ToString & "'", "SHAPEID", , tran))
                        SetTypeId = Val(objGPack.GetSqlValue(" SELECT SETTYPEID FROM " & cnAdminDb & "..STNSETTYPE WHERE SETTYPENAME = '" & .Item("SETTYPE").ToString & "'", "SETTYPEID", , tran))
                    End If
                    strSql += " )VALUES("
                    strSql += " '" & stnSno & "'" ''SNO
                    strSql += " ,'" & updIssSno & "'" 'TAGSNO
                    strSql += " ,'" & itemId & "'" 'ITEMID
                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    strSql += " ," & stnItemId & "" 'STNITEMID
                    strSql += " ," & stnSubItemId & "" 'STNSUBITEMID
                    strSql += " ,'" & txtTagNo__Man.Text & "'" 'TAGNO
                    strSql += " ," & Val(.Item("PCS").ToString) & "" 'STNPCS
                    strSql += " ," & Val(.Item("WEIGHT").ToString) & "" 'STNWT
                    strSql += " ," & Val(.Item("RATE").ToString) & "" 'STNRATE
                    strSql += " ," & Val(.Item("AMOUNT").ToString) & "" 'STNAMT
                    If stnSubItemId <> 0 Then 'DESCRIP
                        strSql += " ,'" & .Item("SUBITEM").ToString & "'"
                    Else
                        strSql += " ,'" & .Item("ITEM").ToString & "'"
                    End If
                    strSql += " ,'" & dtpRecieptDate.Value.Date.ToString("yyyy-MM-dd") & "'" 'RECDATE
                    strSql += " ,'" & .Item("CALC").ToString & "'" 'CALCMODE
                    strSql += " ,0" 'MINRATE
                    strSql += " ,0" 'SIZECODE
                    strSql += " ,'" & .Item("UNIT").ToString & "'" 'STONEUNIT
                    strSql += " ,NULL" 'ISSDATE
                    strSql += " ,''" 'OLDTAGNO
                    strSql += " ,''" 'CARRYFLAG
                    strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " ," & Val(.Item("USRATE").ToString) & "" 'STNRATE
                    strSql += " ," & Val(.Item("INDRS").ToString) & "" 'STNAMT
                    strSql += " ,'" & .Item("PACKETNO").ToString & "'" 'PACKETNO
                    If _FourCMaintain Then
                        strSql += " ,'" & CutId & "'" 'CUTID
                        strSql += " ,'" & ColorId & "'" 'COLORID
                        strSql += " ,'" & ClarityId & "'" 'CLARITYID
                        strSql += " ,'" & ShapeId & "'" 'SHAPEID
                        strSql += " ,'" & SetTypeId & "'" 'SETTYPEID
                        strSql += " ,'" & Val(.Item("HEIGHT").ToString) & "'" 'HEIGHT
                        strSql += " ,'" & Val(.Item("WIDTH").ToString) & "'" 'WIDTH
                    End If
                    strSql += " )"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                End With
            Next
            If _HasPurchase Or pFixedVa Or PUR_AUTOCALC Then
                For Each RoPur As DataRow In ObjPurDetail.dtGridStone.Rows
                    If RoPur.Item("KEYNO").ToString = "" Then Continue For
                    For Each Ro_S As DataRow In dtStoneDetails.Rows
                        If Ro_S.Item("KEYNO") = RoPur.Item("KEYNO") Then
                            RoPur.Item("STNSNO") = Ro_S.Item("STNSNO")
                            Exit For
                        End If
                    Next
                Next
                Dim stnItemId As Integer = 0
                Dim stnSubItemId As Integer = 0
                For Each RoPur As DataRow In ObjPurDetail.dtGridStone.Rows
                    'Dim caType As String = Nothing
                    strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & RoPur.Item("ITEM").ToString & "'"
                    stnItemId = Val(objGPack.GetSqlValue(strSql, "ITEMID", , tran))
                    strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & RoPur.Item("SUBITEM").ToString & "' AND ITEMID = " & stnItemId & ""
                    stnSubItemId = Val(objGPack.GetSqlValue(strSql, , , tran))
                    strSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..PURITEMTAGSTONE"
                    strSql += vbCrLf + " ("
                    strSql += vbCrLf + " TAGSNO,ITEMID,TAGNO,STNITEMID,STNSUBITEMID,STNPCS,STNWT,STNRATE,STNAMT"
                    strSql += vbCrLf + " ,STONEUNIT,CALCMODE,PURRATE,PURAMT,COMPANYID,COSTID,STNSNO"
                    strSql += vbCrLf + " )VALUES("
                    strSql += vbCrLf + " '" & updIssSno & "'" 'TAGSNO
                    strSql += vbCrLf + " ," & itemId & "" 'ITEMID
                    strSql += vbCrLf + " ,'" & txtTagNo__Man.Text & "'" 'TAGNO
                    strSql += " ," & stnItemId & "" 'STNITEMID
                    strSql += " ," & stnSubItemId & "" 'STNSUBITEMID
                    strSql += " ," & Val(RoPur.Item("PCS").ToString) & "" 'STNPCS
                    strSql += " ," & Val(RoPur.Item("WEIGHT").ToString) & "" 'STNWT
                    strSql += " ," & Val(RoPur.Item("RATE").ToString) & "" 'STNRATE
                    strSql += " ," & Val(RoPur.Item("AMOUNT").ToString) & "" 'STNAMT
                    strSql += " ,'" & RoPur.Item("UNIT").ToString & "'" 'STONEUNIT
                    strSql += " ,'" & RoPur.Item("CALC").ToString & "'" 'CALCMODE
                    strSql += vbCrLf + " ," & Val(RoPur.Item("PURRATE").ToString) & "" 'PURRATE
                    strSql += vbCrLf + " ," & Val(RoPur.Item("PURVALUE").ToString) & "" 'PURAMT
                    strSql += vbCrLf + " ,'" & GetStockCompId() & "'"
                    strSql += vbCrLf + " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'"
                    strSql += vbCrLf + " ,'" & RoPur.Item("STNSNO").ToString & "'" 'STNSNO
                    strSql += vbCrLf + " )"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                Next
            End If
        End If
        If _HasPurchase Or pFixedVa Or PUR_AUTOCALC Then
            ''DELETING PURMISCDETAIL
            strSql = " DELETE FROM " & cnAdminDb & "..PURITEMTAGMISCCHAR"
            strSql += " WHERE TAGSNO = '" & updIssSno & "'"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
        End If
        ''DELETING MISCDETAIL
        strSql = " DELETE FROM " & cnAdminDb & "..ITEMTAGMISCCHAR"
        strSql += " WHERE TAGSNO = '" & updIssSno & "'"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
        ''iNSERTING mISC
        For cnt As Integer = 0 To dtMiscDetails.Rows.Count - 1
            Dim miscSno As String = GetNewSno(TranSnoType.ITEMTAGMISCCHARCODE, tran, "GET_ADMINSNO_TRAN")
            With dtMiscDetails.Rows(cnt)
                Dim miscId As String = Nothing
                strSql = " SELECT MISCID FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCNAME = '" & .Item("MISC").ToString & "'"
                miscId = Val(objGPack.GetSqlValue(strSql, "MISCID", , tran))
                strSql = " INSERT INTO " & cnAdminDb & "..ITEMTAGMISCCHAR"
                strSql += " ("
                strSql += " SNO,ITEMID,TAGNO,MISCID,AMOUNT,TAGSNO,COSTID,SYSTEMID,APPVER,COMPANYID)VALUES("
                strSql += " '" & miscSno & "'" 'SNO
                strSql += " ," & itemId & "" 'ITEMID
                strSql += " ,'" & txtTagNo__Man.Text & "'" 'TAGNO
                strSql += " ," & miscId & "" 'MISCID
                strSql += " ," & Val(.Item("AMOUNT").ToString) & "" 'AMOUNT
                strSql += " ,'" & updIssSno & "'" 'TAGSNO
                strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                strSql += " ,'" & systemId & "'" 'SYSTEMID
                strSql += " ,'" & VERSION & "'" 'APPVER
                strSql += " ,'" & GetStockCompId() & "'" ' COMPANYID
                strSql += " )"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            End With
            If _HasPurchase Or pFixedVa Or PUR_AUTOCALC Then
                RowFiter = ObjPurDetail.dtGridMisc.Select("KEYNO = " & Val(dtMiscDetails.Rows(cnt).Item("KEYNO").ToString))
                If RowFiter.Length > 0 Then
                    strSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..PURITEMTAGMISCCHAR"
                    strSql += vbCrLf + " ("
                    strSql += vbCrLf + " MISSNO,TAGSNO,ITEMID,TAGNO,PURAMOUNT,COMPANYID,COSTID"
                    strSql += vbCrLf + " )"
                    strSql += vbCrLf + " VALUES"
                    strSql += vbCrLf + " ("
                    strSql += vbCrLf + " '" & miscSno & "'" 'MISSNO
                    strSql += vbCrLf + " ,'" & updIssSno & "'" 'TAGSNO
                    strSql += " ," & itemId & "" 'ITEMID
                    strSql += " ,'" & txtTagNo__Man.Text & "'" 'TAGNO
                    strSql += " ," & Val(RowFiter(0).Item("PURAMOUNT").ToString) & "" 'PURAMOUNT
                    strSql += " ,'" & strCompanyId & "'"
                    strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'"
                    strSql += vbCrLf + " )"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                End If
            End If
        Next

        ''DELETING MULTIMETALDETAIL
        strSql = " DELETE FROM " & cnAdminDb & "..ITEMTAGMETAL"
        strSql += " WHERE TAGSNO = '" & updIssSno & "'"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
        ''INSERTING MULTIMETAL
        For cnt As Integer = 0 To dtMultiMetalDetails.Rows.Count - 1
            Dim metalSno As String = GetNewSno(TranSnoType.ITEMTAGMETALCODE, tran, "GET_ADMINSNO_TRAN")
            Dim multiMetalId As String = Nothing
            Dim multiCatCode As String = Nothing
            With dtMultiMetalDetails.Rows(cnt)
                strSql = " SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME='" & .Item("CATEGORY").ToString & "'"
                multiMetalId = objGPack.GetSqlValue(strSql, , , tran)
                strSql = " SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME='" & .Item("CATEGORY").ToString & "'"
                multiCatCode = objGPack.GetSqlValue(strSql, , , tran)
                ''INSERTING ITEMTAGMETAL
                strSql = " INSERT INTO " & cnAdminDb & "..ITEMTAGMETAL("
                strSql += " METALID,COMPANYID,ITEMID,RECDATE,"
                strSql += " CATCODE,TAGNO,GRSWT,RATE,"
                strSql += " AMOUNT,TAGSNO,ISSDATE,CARRYFLAG,"
                strSql += " COSTID,SYSTEMID,APPVER,"
                strSql += " WASTPER,WAST,MCGRM,MC,SNO"
                strSql += " )VALUES("
                strSql += " '" & multiMetalId & "'" 'METALID
                strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                strSql += " ,'" & itemId & "'" 'ITEMID
                strSql += " ,'" & dtpRecieptDate.Value.Date.ToString("yyyy-MM-dd") & "'" 'RECDATE
                strSql += " ,'" & multiCatCode & "'" 'CATCODE
                strSql += " ,'" & txtTagNo__Man.Text & "'" 'TAGNO
                strSql += " ," & Val(.Item("WEIGHT").ToString) & "" 'GRSWT
                strSql += " ," & Val(.Item("RATE").ToString) & "" 'RATE
                strSql += " ," & Val(.Item("AMOUNT").ToString) & "" 'AMOUNT
                strSql += " ,'" & updIssSno & "'" 'TAGSNO
                strSql += " ,NULL" 'ISSDATE
                strSql += " ,''" 'CARRYFLAG
                strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                strSql += " ,'" & systemId & "'" 'SYSTEMID
                strSql += " ,'" & VERSION & "'" 'APPVER
                strSql += " ," & Val(.Item("WASTAGEPER").ToString) & "" 'WASTPER
                strSql += " ," & Val(.Item("WASTAGE").ToString) & "" 'WASTPER
                strSql += " ," & Val(.Item("MCPERGRM").ToString) & "" 'WASTPER
                strSql += " ," & Val(.Item("MC").ToString) & "" 'WASTPER
                strSql += " ,'" & metalSno & "'" 'metaalsno
                strSql += " )"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            End With
            If _HasPurchase Or pFixedVa Or PUR_AUTOCALC Then
                RowFiter = ObjPurDetail.dtGridMultiMetal.Select("KEYNO = " & Val(dtMultiMetalDetails.Rows(cnt).Item("KEYNO").ToString))
                If RowFiter.Length > 0 Then
                    strSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..PURITEMTAGMETAL"
                    strSql += vbCrLf + " ("
                    strSql += vbCrLf + " METSNO,TAGSNO"
                    strSql += vbCrLf + " ,ITEMID"
                    strSql += vbCrLf + " ,TAGNO,PURRATE,PURWASTAGE,PURMC,PURTOUCH,PURVALUE"
                    strSql += vbCrLf + " ,COMPANYID,COSTID"
                    strSql += vbCrLf + " )"
                    strSql += vbCrLf + " VALUES"
                    strSql += vbCrLf + " ("
                    strSql += vbCrLf + " '" & metalSno & "'" 'MISSNO
                    strSql += vbCrLf + " ,'" & updIssSno & "'" 'TAGSNO
                    strSql += " ," & itemId & "" 'ITEMID
                    strSql += " ,'" & txtTagNo__Man.Text & "'" 'TAGNO
                    strSql += ",0" 'PURRATE
                    strSql += "," & Val(RowFiter(0).Item("PURWASTAGE").ToString) & "" 'PURWASTAGE
                    strSql += "," & Val(RowFiter(0).Item("PURMC").ToString) & "" 'PURMC
                    strSql += ",0" 'PURTOUCH
                    strSql += "," & Val(RowFiter(0).Item("PURAMOUNT").ToString) & "" 'PURVALUE
                    strSql += vbCrLf + " ,'" & GetStockCompId() & "'"
                    strSql += vbCrLf + " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'"
                    strSql += vbCrLf + " )"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                End If
            End If
        Next

        ''DELETING ADDITIONAL STOCK ENTRY
        strSql = " DELETE FROM " & cnAdminDb & "..ADDINFOITEMTAG"
        strSql += " WHERE TAGSNO = '" & updIssSno & "'"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
        ''INSERTING ADDITIONAL STOCK ENTRY
        For cnt As Integer = 0 To DtAddStockEntry.Rows.Count - 1
            With DtAddStockEntry.Rows(cnt)
                ''INSERTING ADDINFOITEMTAG
                strSql = " INSERT INTO " & cnAdminDb & "..ADDINFOITEMTAG("
                strSql += " SNO,TAGSNO,OTHID,ITEMID,TAGNO,RECDATE,COMPANYID"
                strSql += " ,COSTID,SYSTEMID,APPVER"
                strSql += " )VALUES("
                strSql += " '" & GetNewSno(TranSnoType.ADDINFOITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO  
                strSql += " ,'" & updIssSno & "'" 'TAGSNO
                strSql += " ,'" & Val(.Item("OTHID").ToString) & "'" 'OTHID
                strSql += " ,'" & itemId & "'" 'ITEMID
                strSql += " ,'" & txtTagNo__Man.Text & "'" 'TAGNO
                strSql += " ,'" & GetEntryDate(dtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                strSql += " ,'" & systemId & "'" 'SYSTEMID
                strSql += " ,'" & VERSION & "'" 'APPVER
                strSql += " )"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            End With
        Next

        ''CPIECES AND CWT
        'strSql = " UPDATE " & cnAdminDb & "..ITEMLOT SET CPCS = CPCS + " & (Val(txtPieces_Num_Man.Text) - lotPcs) & ""
        'strSql += " ,CGRSWT = CGRSWT + " & (Val(txtGrossWt_Wet.Text) - lotGrsWt) & ""
        'strSql += " ,CNETWT = ISNULL(CNETWT,0) + " & (Val(txtNetWt_Wet.Text) - lotNetWt) & ""
        'strSql += " WHERE SNO = '" & SNO & "'"
        'cmd = New OleDbCommand(strSql, cn, tran)
        'cmd.ExecuteNonQuery()

        If _CheckOrderInfo = False And ObjOrderTagInfo.txtOrderNo.Text <> "" Then
            strSql = vbCrLf + " UPDATE " & cnAdminDb & "..ITEMTAG SET ORDREPNO = '" & GetCostId(COSTID) & GetCompanyId(GetStockCompId()) & ObjOrderTagInfo.txtOrderNo.Text & "'"
            strSql += vbCrLf + " WHERE TAGNO = '" & txtTagNo__Man.Text & "' AND ORDREPNO = '" & EditOrNo & "'"
            strSql += vbCrLf + " UPDATE " & cnAdminDb & "..ORIRDETAIL SET ORNO = '" & GetCostId(COSTID) & GetCompanyId(GetStockCompId()) & ObjOrderTagInfo.txtOrderNo.Text & "'"
            strSql += vbCrLf + " WHERE TAGNO = '" & txtTagNo__Man.Text & "' AND ORNO = '" & EditOrNo & "'"
            strSql += vbCrLf + " UPDATE " & cnAdminDb & "..ORMAST SET ORNO = '" & GetCostId(COSTID) & GetCompanyId(GetStockCompId()) & ObjOrderTagInfo.txtOrderNo.Text & "' ,EMPID = '" & ObjOrderTagInfo.txtEmpNo_NUM.Text & "'"
            strSql += vbCrLf + " WHERE ORNO = '" & EditOrNo & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()
        End If
    End Sub


    Private Sub UpdateTag()
        If objGPack.Validator_Check(grpSaveControls) Then Exit Sub
        Dim ds As New DataSet
        ds.Clear()
        If txtLessWt_Wet.Enabled = True Then
            'If Val(txtGrossWt_Wet.Text) <> 0 And Val(txtLessWt_Wet.Text) >= Val(txtGrossWt_Wet.Text) Then
            If Val(txtGrossWt_Wet.Text) <> 0 And Val(txtLessWt_Wet.Text) > Val(txtGrossWt_Wet.Text) Then
                MsgBox(E0004 + Me.GetNextControl(txtLessWt_Wet, False).Text, MsgBoxStyle.Exclamation)
                txtLessWt_Wet.Focus()
                Exit Sub
            End If
        End If
        If txtNetWt_Wet.Enabled = True Then
            If Val(txtGrossWt_Wet.Text) <> 0 And Val(txtNetWt_Wet.Text) < 0 Then
                MsgBox(Me.GetNextControl(txtNetWt_Wet, False).Text + E0001, MsgBoxStyle.Exclamation)
                txtNetWt_Wet.Focus()
                Exit Sub
            End If
            If Val(txtNetWt_Wet.Text) > Val(txtGrossWt_Wet.Text) Then
                MsgBox(Me.GetNextControl(txtNetWt_Wet, False).Text + E0015 + Me.GetNextControl(txtGrossWt_Wet, False).Text, MsgBoxStyle.Exclamation)
                txtNetWt_Wet.Focus()
                Exit Sub
            End If
        End If
        TagEditSave()
        If GetAdmindbSoftValue("USERLEVELPWD", "N") = "Y" Then
            Dim Sqlqry As String = "Select Optionid from " & cnAdminDb & "..PRJPWDOPTION where OPTIONNAME ='TAGEDIT' AND active = 'Y'"
            Dim Optionid As Integer = Val("" & objGPack.GetSqlValue(Sqlqry, , , tran))
            If Optionid <> 0 Then
                strSql = " INSERT INTO " & cnAdminDb & "..EXEMPTION"
                strSql += " ("
                strSql += " Exempid,OPTIONID,BATCHNO,COSTID,EXEMPDATE,EXEMPTIME,EXEMPUSER,EXEMPOPEN,DESCRIPTION"
                strSql += " )"
                strSql += " VALUES"
                strSql += " ("
                strSql += " " & Val(objGPack.GetMax("Exempid", "EXEMPTION", cnAdminDb, tran).ToString)
                strSql += " ," & Optionid
                strSql += " ,'" & updIssSno & "'" 'ISSSNO
                strSql += " ,'" & cnCostId & "'"
                strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'"
                strSql += " ,'" & Date.Now.ToLongTimeString & "'"
                strSql += " ," & userId
                strSql += " ,'OTP'"
                strSql += " ,'Tag edit allowed'"
                strSql += " )"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            End If
        End If

    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If Not tagEdit Then
            If STKAFINDATE = False Then
                If Not CheckDate(dtpRecieptDate.Value) Then Exit Sub
                If CheckEntryDate(dtpRecieptDate.Value) Then Exit Sub
            End If
        End If
        If cmbSubItem_Man.Text <> "" Then
            strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')"
        Else
            strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"
        End If
        Dim calcType As String = objGPack.GetSqlValue(strSql)
        ''Weight Rate Validation
        Select Case calcType.ToUpper
            Case "W"
                If Val(txtGrossWt_Wet.Text) = 0 Then
                    MsgBox("Grs Weight should not empty", MsgBoxStyle.Information)
                    txtGrossWt_Wet.Focus()
                    Exit Sub
                End If
            Case "R"
                If Val(txtRate_Amt.Text) = 0 Then
                    MsgBox("Rate should not empty", MsgBoxStyle.Information)
                    txtRate_Amt.Focus()
                    Exit Sub
                End If
            Case "B"
                If Val(txtGrossWt_Wet.Text) = 0 Then
                    MsgBox("Grs Weight should not empty", MsgBoxStyle.Information)
                    txtGrossWt_Wet.Focus()
                    Exit Sub
                End If
                If Val(txtRate_Amt.Text) = 0 Then
                    MsgBox("Rate should not empty", MsgBoxStyle.Information)
                    txtRate_Amt.Focus()
                    Exit Sub
                End If
            Case "F"
                If Val(txtGrossWt_Wet.Text) = 0 And Val(txtRate_Amt.Text) = 0 Then
                    MsgBox("Weight and Rate should not empty", MsgBoxStyle.Information)
                    txtGrossWt_Wet.Focus()
                    Exit Sub
                End If
        End Select

        If (_HasPurchase Or pFixedVa) Then
            If ObjPurDetail.Loadd = False Then
                If tagEdit And TAGEDITDISABLE.Contains("PV") Then GoTo nnnext
                txtSalValue_KeyPress(Me, New KeyPressEventArgs(Chr(Keys.Enter)))
                Exit Sub
            End If
        End If

        If PUR_AUTOCALC = True And Not (_HasPurchase Or pFixedVa) Then
            ShowPurDetails()
        End If
nnnext:
        If picPath <> Nothing Then
            If Not IO.Directory.Exists(defalutDestination) Then
                MsgBox(defalutDestination & vbCrLf & " Destination Path not found" & vbCrLf & "Please make the correct path", MsgBoxStyle.Information)
                Exit Sub
            End If
        End If
        If _TagImageMust Then
            If picPath = Nothing Then
                MsgBox("Please Attach Image", MsgBoxStyle.Information)
                btnAttachImage.Select()
                Exit Sub
            End If
        End If
        'If tagEdit Then
        '    If _CheckOrderInfo = False And ObjOrderTagInfo.txtOrderNo.Text <> "" Then
        '        ObjOrderTagInfo.ShowDialog()
        '    End If
        '    UpdateTag()
        '    Exit Sub
        'End If
        If objGPack.Validator_Check(grpSaveControls) Then Exit Sub
        Dim ds As New DataSet
        ds.Clear()
        Dim tDd As Date = GetServerDate()
        If GetAdmindbSoftValue("GLOBALDATE").ToUpper = "Y" Then
            tDd = GetEntryDate(tDd)
        End If
        If Not (dtpRecieptDate.Value.Date.ToString("yyyy-MM-dd") >= lotRecieptDate.Date.ToString("yyyy-MM-dd")) Then
            Dim errStr As String
            errStr = "Reciept Date Should not allow before LotDate"
            'errStr += " And Receipt Date Should not Exceed Today Date"
            MsgBox(errStr, MsgBoxStyle.Exclamation)
            dtpRecieptDate.Focus()
            Exit Sub
        End If
        If Not tagEdit Then
            If Not dtpRecieptDate.Value.Date.ToString("yyyy-MM-dd") <= tDd.ToString("yyyy-MM-dd") Then
                Dim errStr As String
                errStr = "Receipt Date Should not Exceed Today Date"
                MsgBox(errStr, MsgBoxStyle.Exclamation)
                dtpRecieptDate.Focus()
                Exit Sub
            End If
        End If

        'If tabCheckBy = "P" Then
        '    If Not Val(txtPieces_Num_Man.Text) <= Val(lblPBalance.Text) + Val(Tag_Tolerance) Then
        '        MsgBox(Me.GetNextControl(txtPieces_Num_Man, False).Text + E0006 + lblPBalance.Text, MsgBoxStyle.Exclamation)
        '        txtPieces_Num_Man.Focus()
        '        Exit Sub
        '    End If
        'Else
        '    'If Not Math.Abs(Val(lblWBalance.Text) - Val(txtGrossWt_Wet.Text)) <= Val(Tag_Tolerance) Then
        '    If Not Val(txtGrossWt_Wet.Text) <= Val(lblWBalance.Text) + Val(Tag_Tolerance) Then
        '        MsgBox(Me.GetNextControl(txtPieces_Num_Man, False).Text + E0006 + lblWBalance.Text, MsgBoxStyle.Exclamation)
        '        txtGrossWt_Wet.Focus()
        '        Exit Sub
        '    End If
        'End If
        '' JOS 1
        If Tag_Tolerance <> 0 Then
            If Val(lblPBalance.Text) - Val(txtPieces_Num_Man.Text) = 0 Then
                If Math.Round(Math.Abs(Val(lblWBalance.Text) - Val(txtGrossWt_Wet.Text)), 3) > Math.Round(Val(Tag_Tolerance), 3) Then
                    MsgBox(Me.GetNextControl(txtPieces_Num_Man, False).Text + E0024 + lblWBalance.Text, MsgBoxStyle.Exclamation)
                    txtGrossWt_Wet.Focus()
                    Exit Sub
                End If
            End If
        End If
        'JOS 1

        'If txtRate_Amt.Enabled = True Then
        '    If Val(txtRate_Amt.Text) <= 0 Then
        '        MsgBox("Rate should not Empty", MsgBoxStyle.Exclamation)
        '        txtRate_Amt.Focus()
        '        Exit Sub
        '    End If
        'End If
        If txtLessWt_Wet.Enabled = True Then
            If Val(txtGrossWt_Wet.Text) <> 0 And Val(txtLessWt_Wet.Text) > Val(txtGrossWt_Wet.Text) Then
                'If Val(txtGrossWt_Wet.Text) <> 0 And Val(txtLessWt_Wet.Text) >= Val(txtGrossWt_Wet.Text) Then
                MsgBox(E0004 + Me.GetNextControl(txtLessWt_Wet, False).Text, MsgBoxStyle.Exclamation)
                txtLessWt_Wet.Focus()
                Exit Sub
            End If
        End If
        If txtNetWt_Wet.Enabled = True Then
            If Val(txtGrossWt_Wet.Text) <> 0 And Val(txtNetWt_Wet.Text) <= 0 And Val(txtGrossWt_Wet.Text) <> Val(txtLessWt_Wet.Text) Then
                MsgBox("Net Weight Should not Empty", MsgBoxStyle.Exclamation)
                txtNetWt_Wet.Focus()
                Exit Sub
            End If
            If Val(txtNetWt_Wet.Text) > Val(txtGrossWt_Wet.Text) Then
                MsgBox(Me.GetNextControl(txtNetWt_Wet, False).Text + E0015 + Me.GetNextControl(txtGrossWt_Wet, False).Text, MsgBoxStyle.Exclamation)
                txtNetWt_Wet.Focus()
                Exit Sub
            End If
        End If
        If Val(ObjMinValue.txtMinWastage_Per.Text) > Val(txtMaxWastage_Per.Text) Then
            MsgBox("MinWastage Per Should not Exceed MaxWastage Per", MsgBoxStyle.Information)
            ObjMinValue.txtMinWastage_Per.Focus()
            Exit Sub
        End If
        If Val(ObjMinValue.txtMinWastage_Wet.Text) > Val(txtMaxWastage_Wet.Text) Then
            MsgBox("MinWastage Should not Exceed MaxWastage ", MsgBoxStyle.Information)
            ObjMinValue.txtMinWastage_Wet.Focus()
            Exit Sub
        End If
        If Val(ObjMinValue.txtMinMcPerGram_Amt.Text) > Val(txtMaxMcPerGrm_Amt.Text) Then
            MsgBox("Min Mc Per Grm Should not Exceed Max Mc Per Grm", MsgBoxStyle.Information)
            ObjMinValue.txtMinMcPerGram_Amt.Focus()
            Exit Sub
        End If
        If Val(ObjMinValue.txtMinMkCharge_Amt.Text) > Val(txtMaxMkCharge_Amt.Text) Then
            MsgBox("Min Making Charge Should not Exceed Max Making Charge", MsgBoxStyle.Information)
            ObjMinValue.txtMinMkCharge_Amt.Focus()
            Exit Sub
        End If
        Try
            'funcAdd()
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub txtLotNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtLotNo_Num.KeyPress
        Dim dt As DataTable
        If txtLotNo_Num.Text = "" Then
            Exit Sub
        End If
        If e.KeyChar = Chr(Keys.Enter) Then
            strSql = " SELECT lot.SNO,r.Recsno FROM " & cnAdminDb & "..ITEMLOT lot left join " & cnStockDb & "..LOTISSUE R ON lot.Sno = r.Lotsno"
            strSql += " WHERE lot.LOTNO = '" & txtLotNo_Num.Text & "'"
            If tabCheckBy = "P" Then
                strSql += " AND lot.PCS > lot.CPCS "
            Else
                strSql += vbCrLf + " AND ((lot.GRSWT > lot.CGRSWT) or (lot.rate <> 0 and lot.pcs > lot.cpcs))"
                '                strSql += " AND GRSWT > CGRSWT"
            End If
            strSql += " AND ISNULL(COMPLETED,'') <> 'Y'"
            ' strSql += " AND (SELECT STOCKTYPE FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = LOT.ITEMID) = 'T'"
            'strSql += " AND LOTNO = '" & txtLotNo_Num_Man.Text & "'"
            strSql += " AND lot.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T')"
            strSql += " AND ISNULL(BULKLOT,'') <> 'Y'"
            da = New OleDbDataAdapter(strSql, cn)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If dt.Rows.Count = 1 Then
                    SNO = dt.Rows(0).Item("Sno").ToString
                    Recsno = dt.Rows(0).Item("recsno").ToString
                    If LoadLotDetails() = False Then Exit Sub
                    txtLotNo_Num.Enabled = False
                Else
                    strSql = " SELECT "
                    strSql += " LOTNO"
                    strSql += vbCrLf + " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = LOT.DESIGNERID)AS DESIGNER"
                    strSql += " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS IM WHERE IM.ITEMID = LOT.ITEMID)AS ITEMNAME"
                    strSql += " ,ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS SM WHERE SM.SUBITEMID = LOT.SUBITEMID),'')AS SUBITEMNAME"
                    strSql += " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = LOT.COSTID)AS COSTCENTRE"
                    strSql += " ,PCS,GRSWT,CPCS AS CPCS,CGRSWT AS CGRSWT,(PCS-CPCS) AS BALPCS,(GRSWT-CGRSWT)AS BALGRSWT"
                    strSql += " ,(SELECT (CASE WHEN STOCKTYPE = 'T' THEN 'TAGED' WHEN STOCKTYPE = 'N' THEN 'NON TAGED' ELSE 'PACKET BASED' END) FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = LOT.ITEMID)AS STOCKTYPE"
                    strSql += vbCrLf + " ,CASE ENTRYTYPE WHEN 'R' THEN 'REGULAR' WHEN 'OR' THEN 'ORDER' WHEN 'RE' THEN 'ORDER' ELSE ENTRYTYPE END AS LOTTYPE,SNO,FROMITEMID"
                    strSql += " FROM " & cnAdminDb & "..ITEMLOT AS LOT"
                    strSql += " WHERE PCS > CPCS AND ISNULL(COMPLETED,'') <> 'Y'"
                    strSql += " AND (SELECT STOCKTYPE FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = LOT.ITEMID) = 'T'"
                    strSql += " AND LOTNO = '" & txtLotNo_Num.Text & "'"
                    strSql += " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T')"
                    strSql += " AND ISNULL(BULKLOT,'') <> 'Y'"
                    strSql += " ORDER BY LOTNO "
                    SNO = BrighttechPack.SearchDialog.Show("Searching LotNo", strSql, cn, 0, 13)

                    strSql = " SELECT A.LOTNO,LI.RECSNO FROM " & cnAdminDb & "..ITEMLOT A LEFT JOIN " & cnStockDb & "..LOTISSUE LI ON A.SNO=LI.LOTSNO"
                    strSql += " WHERE SNO = '" & SNO & "'"
                    dt.Clear()
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(dt)
                    If dt.Rows.Count > 0 Then
                        txtLotNo_Num.Text = dt.Rows(0).Item("LotNo").ToString
                        Recsno = dt.Rows(0).Item("RECSNO").ToString
                    Else
                        Exit Sub
                    End If

                    If LoadLotDetails() = False Then Exit Sub
                    txtLotNo_Num.Enabled = False
                End If
            Else
                MsgBox(E0004 + Me.GetNextControl(txtLotNo_Num, False).Text, MsgBoxStyle.Exclamation)
                txtLotNo_Num.Focus()
            End If
        Else
            Exit Sub
        End If
        'funcAssignTabControls()
    End Sub

    Private Function LoadLotDetails() As Boolean
        Dim itemName As String = ""
        'strSql = " SELECT (sELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = L.ITEMID)AS ITEMNAME FROM " & cnAdminDb & "..ITEMLOT as L WHERE SNO = '" & SNO & "'"
        strSql = " SELECT L.ENTRYTYPE ENTRYTYPE,IT.ITEMNAME ITEMNAME,IT.ITEMID ITEMID,ISNULL(IT.ASSORTED,'') AS ASSORTED,fromitemid,IT.MCASVAPER FROM " & cnAdminDb & "..ITEMLOT L LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IT ON L.ITEMID = IT.ITEMID WHERE SNO = '" & SNO & "' "
        Dim DR As DataRow
        DR = GetSqlRow(strSql, cn)
        'itemName = objGPack.GetSqlValue(strSql)
        itemName = DR.Item("ITEMNAME").ToString
        Dim itemID As String
        itemID = DR.Item("ITEMID").ToString
        Dim entryType As String = DR.Item("ENTRYTYPE").ToString
        Dim mASSORTED As String = DR.Item("ASSORTED").ToString
        mfromItemid = Val(DR.Item("FROMITEMID").ToString)
        If DR.Item("Mcasvaper").ToString = "Y" Then Label44.Text = "MC PERCENT" Else Label44.Text = "Max Mc Per Gram"
        If entryType = "OR" Or entryType = "RE" Then
            If _CheckOrderInfo Then
                If Not ORDetail() Then
                    btnNew_Click(Me, New EventArgs)
                    txtLotNo_Num.Focus()
                    Return False
                    Exit Function
                End If
            Else
                If ObjOrderTagInfo.ShowDialog() <> Windows.Forms.DialogResult.OK Then
                    btnNew_Click(Me, New EventArgs)
                    txtLotNo_Num.Focus()
                    Return False
                    Exit Function
                End If
            End If
            If ORDERLOTITEM = True And itemID <> GetSqlValue(cn, "SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItem_MAN.Text.Trim & "'") Then
                MsgBox("Order item and lot item should be same", MsgBoxStyle.Information)
                btnNew_Click(Me, New EventArgs)
                txtLotNo_Num.Focus()
                Return False
                Exit Function
            End If
            Dim dt As New DataTable
            strSql = funcAssignTagDefaultVal()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            funcAssignVal(dt, _CheckOrderInfo)
            If cmbSubItem_Man.Enabled Then cmbSubItem_SelectedIndexChanged(Me, New EventArgs)
            'funcAssignTabControls()
            lblItemChange.Visible = False
            ItemLock = False
            Me.SelectNextControl(cmbItem_MAN, True, True, True, True)
            Return True
            Exit Function
        End If
        'If objGPack.GetSqlValue("SELECT ISNULL(ASSORTED,'') FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & itemName & "'") = "Y" Then
        If mASSORTED = "Y" Then
            lblItemChange.Visible = True
            'cmbItem_MAN.Text = ""
        Else
            cmbItem_MAN.Text = itemName
        End If
        cmbItem_MAN.Select()
        Return True
    End Function

    Private Sub txtSalValue_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSalValue_Amt_Man.KeyPress
        If Not (e.KeyChar = Chr(Keys.Enter)) Then
            Exit Sub
        End If
        If calType = "M" And Ratevaluezero Then txtRate_Amt.Text = Math.Round(Val(txtSalValue_Amt_Man.Text) / Val(txtGrossWt_Wet.Text), 2)
        ShowPurDetails()
        BlockSv = True
        'If txtRefVal_AMT.Visible Then
        '    txtRefVal_AMT.Select()
        'Else
        '    Me.SelectNextControl(txtPurchaseValue_Amt, True, True, True, True)
        'End If
        Me.SelectNextControl(txtSalValue_Amt_Man, True, True, True, True)
        'If Not TabControl1.TabPages.Contains(tabPurchase) Then Exit Sub
        'If flagPurValSet Then Exit Sub
        'cmbPurCalMode.Text = cmbCalcMode.Text
        'txtPurGrossWt_Wet.Text = txtGrossWt_Wet_MAN.Text
        'txtPurLessWt_Wet.Text = txtLessWt_Wet.Text
        'txtPurNetWt_Wet.Text = txtNetWt_Wet.Text
        'If purRate = 0 Then
        '    txtPurRate_Amt.Text = IIf(calType = "M", txtRate_Amt_Man.Text, txtMetalRate_Amt_Man.Text)
        'Else
        '    txtPurRate_Amt.Text = purRate
        'End If
        'TabControl1.SelectedTab = tabPurchase
    End Sub

    Private Sub ShowPurDetails()

        If _HasPurchase = False And pFixedVa = False And PUR_AUTOCALC = False Then Exit Sub
        Dim RowCheck() As DataRow = Nothing
        If tagEdit And TAGEDITDISABLE.Contains("PV") Then Exit Sub
        With ObjPurDetail
            .CalcMode = calType
            If pFixedVa Then
                .lblFixedValVa.Visible = True
                .txtpURFixedValueVa_AMT.Visible = True
            Else
                .lblFixedValVa.Visible = False
                .txtpURFixedValueVa_AMT.Visible = False
            End If
            .salePcs = Val(txtPieces_Num_Man.Text)
            If Not flagPurValSet Then
                If Not tagEdit Then
                    .cmbPurCalMode.Text = cmbCalcMode.Text
                    .txtPurGrossWt_Wet.Text = txtGrossWt_Wet.Text
                    .txtPurLessWt_Wet.Text = txtLessWt_Wet.Text
                    .txtPurNetWt_Wet.Text = txtNetWt_Wet.Text
                    .dtpPurchaseDate.Value = dtpRecieptDate.Value
                    .txtPurMcPerGrm_TextChanged(Me, New EventArgs)
                    If purRate = 0 Then
                        .txtPurRate_Amt.Text = IIf(calType <> "W" And calType <> "B", txtRate_Amt.Text, txtMetalRate_Amt.Text)
                        purRate = Val(.txtPurRate_Amt.Text)
                    Else
                        .txtPurRate_Amt.Text = purRate
                    End If
                End If
                SyncStoneMiscToPurStoneMisc()
            End If
            For cnt As Integer = 0 To dtStoneDetails.Rows.Count - 1
                RowCheck = .dtGridStone.Select("KEYNO = " & Val(dtStoneDetails.Rows(cnt).Item("KEYNO").ToString) & "")
                If RowCheck.Length = 0 Then
                    .dtGridStone.ImportRow(dtStoneDetails.Rows(cnt))
                End If
            Next
            .dtGridStone.AcceptChanges()
            For cnt As Integer = 0 To dtStoneDetails.Rows.Count - 1
                RowCheck = .dtGridStone.Select("KEYNO = " & Val(dtStoneDetails.Rows(cnt).Item("KEYNO").ToString) & "")
                If RowCheck.Length = 0 Then
                    .dtGridStone.ImportRow(dtStoneDetails.Rows(cnt))
                End If
            Next
            .dtGridStone.AcceptChanges()

            If Not flagPurValSet Then
                If Not tagEdit Then
                    If Val(.txtPurTaxPer_PER.Text) = 0 And Val(.txtPurTax_AMT.Text) = 0 Then
                        Dim pTax As Decimal = Val(objGPack.GetSqlValue("SELECT PTAX FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = (sELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')"))
                        .txtPurTaxPer_PER.Text = IIf(pTax <> 0, Format(pTax, "0.00"), "")
                    End If
                End If
            End If
            If Not (_HasPurchase = False And pFixedVa = False) Then
                .ShowDialog()
            End If
            '01
            'If .saleValue > 0 Then
            '    'If txtSalValue_Amt_Man.Text < .saleValue Then
            '    purSalevalue = .saleValue
            '    txtSalValue_Amt_Man.Text = SALEVALUE_ROUND(.saleValue)
            '    'End If
            'End If
            '01
            'If Val(.txtPurRate_Amt.Text) <> 0 Then
            '    purRate = Val(.txtPurRate_Amt.Text)
            'End If
            For cnt As Integer = 0 To .dtGridStone.Rows.Count - 1
                For Each col As DataColumn In dtStoneDetails.Columns
                    If col.ColumnName.ToUpper <> "PURRATE" Then Continue For
                    If col.ColumnName.ToUpper <> "PURVALUE" Then Continue For
                    dtStoneDetails.Rows(cnt).Item(col.ColumnName) = .dtGridStone.Rows(cnt).Item(col.ColumnName)
                Next
            Next
            For cnt As Integer = 0 To .dtGridMisc.Rows.Count - 1
                For Each col As DataColumn In dtMiscDetails.Columns
                    If col.ColumnName.ToUpper <> "PURAMOUNT" Then Continue For
                    dtMiscDetails.Rows(cnt).Item(col.ColumnName) = .dtGridMisc.Rows(cnt).Item(col.ColumnName)
                Next
            Next
            txtPurchaseValue_Amt.Text = .txtPurPurchaseVal_Amt.Text
            If calType = "R" And Val(.txtSaleRate_PER.Text) <> 0 Then txtRate_Amt.Text = Val(.txtPurPurchaseVal_Amt.Text) + (Val(.txtPurPurchaseVal_Amt.Text) * (Val(.txtSaleRate_PER.Text) / 100))
            If calType = "M" And Val(.txtSaleRate_PER.Text) <> 0 Then txtRate_Amt.Text = Val(.txtPurRate_Amt.Text) + (Val(.txtPurRate_Amt.Text) * (Val(.txtSaleRate_PER.Text) / 100))
            flagPurValSet = False
        End With
    End Sub

    Private Sub SyncStoneMiscToPurStoneMisc()
        Dim RowCheck() As DataRow = Nothing
        ''Set Stone Value
        With ObjPurDetail
            If Not .dtGridStone.Rows.Count > 0 Then
StDel:
                For Each ro As DataRow In .dtGridStone.Rows
                    RowCheck = dtStoneDetails.Select("KEYNO = " & Val(ro.Item("KEYNO").ToString) & "")
                    If RowCheck.Length = 0 Then
                        ro.Delete()
                        .dtGridStone.AcceptChanges()
                        GoTo StDel
                    End If
                Next
                .dtGridStone.AcceptChanges()
                For cnt As Integer = 0 To dtStoneDetails.Rows.Count - 1
                    RowCheck = .dtGridStone.Select("KEYNO = " & Val(dtStoneDetails.Rows(cnt).Item("KEYNO").ToString) & "")
                    If RowCheck.Length = 0 Then
                        .dtGridStone.ImportRow(dtStoneDetails.Rows(cnt))
                    End If
                Next
                .dtGridStone.AcceptChanges()

                Dim DtTemp As New DataTable
                Dim purRate As Decimal = Nothing
                For Each Row As DataRow In .dtGridStone.Rows
                    Dim stItemId As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & Row.Item("ITEM").ToString & "'"))
                    Dim cent As Double = 0
                    strSql = " SELECT ISNULL(CALTYPE,'') CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') ='" & Row.Item("ITEM").ToString & "'"
                    Dim mCaltype As String = objGPack.GetSqlValue(strSql, "CALTYPE", "")
                    strSql = " SELECT ISNULL(CALTYPE,'') CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE ISNULL(SUBITEMNAME,'') ='" & Row.Item("SUBITEM").ToString & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & Row.Item("ITEM").ToString & "')"
                    mCaltype = objGPack.GetSqlValue(strSql, "CALTYPE", "")
                    If mCaltype = "D" Then
                        cent = Val(Row.Item("WEIGHT").ToString)
                    Else
                        cent = (Val(Row.Item("WEIGHT").ToString) / IIf(Val(Row.Item("PCS").ToString) <> 0, Val(Row.Item("PCS").ToString), 1))
                    End If

                    Dim wt As Decimal = cent * 100
                    strSql = vbCrLf + " SELECT C.PURRATE FROM " & cnAdminDb & "..CENTRATE AS C"
                    strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = C.ITEMID AND IM.ITEMNAME = '" & Row.Item("ITEM").ToString & "'"
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.SUBITEMID = C.SUBITEMID AND SM.ITEMID = C.ITEMID AND SM.SUBITEMNAME = '" & Row.Item("SUBITEM").ToString & "'"
                    strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..DESIGNER AS DE ON DE.ACCODE = C.ACCODE AND DE.DESIGNERNAME = '" & cmbDesigner_MAN.Text & "'"
                    strSql += vbCrLf + " WHERE " & wt & " BETWEEN FROMCENT AND TOCENT"
                    strSql += vbCrLf + " AND ISNULL(C.SUBITEMID,0) = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & Row.Item("SUBITEM").ToString & "' AND ITEMID = " & stItemId & "),0)"
                    If CENTRATE_DESIGNER Then strSql += vbCrLf + " AND ISNULL(DE.DESIGNERID,0)=ISNULL(C.DESIGNERID,0)"
                    If cmbCostCentre_Man.Text.Trim <> "" Then strSql += " AND COSTID IN(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre_Man.Text & "')"
                    'strSql += vbCrLf + " WHERE ISNULL(CONVERT(NUMERIC(15,4),(" & Val(Row.Item("WEIGHT").ToString) & " /CASE WHEN " & Val(Row.Item("PCS").ToString) & "  = 0 THEN 1 ELSE " & Val(Row.Item("PCS").ToString) & " END)*100),0) BETWEEN FROMCENT AND TOCENT"
                    strSql += vbCrLf + " ORDER BY C.ACCODE desc,C.SUBITEMID desc,C.ITEMID"
                    DtTemp = New DataTable
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(DtTemp)
                    If DtTemp.Rows.Count > 0 Then
                        Row.Item("PURRATE") = IIf(Val(DtTemp.Rows(0).Item("PURRATE").ToString) <> 0, Val(DtTemp.Rows(0).Item("PURRATE").ToString), DBNull.Value)
                        If Val(Row.Item("PURRATE").ToString) <> 0 Then
                            Row.Item("PURVALUE") = IIf(Row.Item("CALC").ToString = "P", Val(Row.Item("PCS").ToString), Val(Row.Item("WEIGHT").ToString)) * Val(Row.Item("PURRATE").ToString)
                        End If
                    End If
                Next
            End If
            .CalcStoneWtAmount()
MiscDel:
            ''Misc Value
            For Each ro As DataRow In .dtGridMisc.Rows
                RowCheck = dtMiscDetails.Select("KEYNO = " & Val(ro.Item("KEYNO").ToString) & "")
                If RowCheck.Length = 0 Then
                    ro.Delete()
                    .dtGridMisc.AcceptChanges()
                    GoTo MiscDel
                End If
            Next
            .dtGridMisc.AcceptChanges()
            For cnt As Integer = 0 To dtMiscDetails.Rows.Count - 1
                RowCheck = .dtGridMisc.Select("KEYNO = " & Val(dtMiscDetails.Rows(cnt).Item("KEYNO").ToString) & "")
                If RowCheck.Length = 0 Then
                    .dtGridMisc.ImportRow(dtMiscDetails.Rows(cnt))
                End If
            Next
            .dtGridMisc.AcceptChanges()
            .CalcMiscTotalAmount()
MetalDel:
            ''MultiMetal Value
            For Each ro As DataRow In .dtGridMultiMetal.Rows
                RowCheck = dtMultiMetalDetails.Select("KEYNO = " & Val(ro.Item("KEYNO").ToString) & "")
                If RowCheck.Length = 0 Then
                    ro.Delete()
                    .dtGridMultiMetal.AcceptChanges()
                    GoTo MetalDel
                End If
            Next
            .dtGridMultiMetal.AcceptChanges()
            For cnt As Integer = 0 To dtMultiMetalDetails.Rows.Count - 1
                RowCheck = .dtGridMultiMetal.Select("KEYNO = " & Val(dtMultiMetalDetails.Rows(cnt).Item("KEYNO").ToString) & "")
                If RowCheck.Length = 0 Then
                    .dtGridMultiMetal.ImportRow(dtMultiMetalDetails.Rows(cnt))
                End If
            Next
            .dtGridMultiMetal.AcceptChanges()
        End With
    End Sub


    Private Sub DeleteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteToolStripMenuItem.Click
        AutoImageSizer(My.Resources.no_photo, picModel, PictureBoxSizeMode.StretchImage)
        'Me.picModel.Image = My.Resources.Resources.no_photo
        ''picModel.Image = Nothing
        picPath = Nothing
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
        WfuncNew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        ItemLock = False
        Me.Close()
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        btnSave_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub dtpRecieptDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim tDd As Date = GetServerDate()
            If GetAdmindbSoftValue("GLOBALDATE").ToUpper = "Y" Then
                tDd = GetEntryDate(tDd)
            End If
            If Not (dtpRecieptDate.Value.Date.ToString("yyyy-MM-dd") >= lotRecieptDate.Date.ToString("yyyy-MM-dd") _
            And dtpRecieptDate.Value.Date.ToString("yyyy-MM-dd") <= tDd.ToString("yyyy-MM-dd")) Then
                Dim errStr As String
                errStr = "Reciept Date Should not allow before LotDate" + vbCrLf
                errStr += " And Receipt Date Should not Exceed Today Date"
                MsgBox(errStr, MsgBoxStyle.Exclamation)
                dtpRecieptDate.Focus()
            End If
        End If
    End Sub


    Private Sub EditGridToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditGridToolStripMenuItem.Click
        If TabControl1.SelectedTab.Name = tabStone.Name Then
            gridStone.Focus()
        ElseIf TabControl1.SelectedTab.Name = tabMultiMetal.Name Then
            gridMultimetal.Focus()
        ElseIf TabControl1.SelectedTab.Name = tabOtherCharges.Name Then
            gridMisc.Focus()
        End If
    End Sub


    Private Sub cmbTableCode_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTableCode.GotFocus
        If tagEdit Then Exit Sub
        Me.cmbTableCode_SelectedIndexChanged(Me, New System.EventArgs)
    End Sub

    Private Sub CalcMaxMinValues()
        strSql = Nothing
        Dim type As String
        If Not OrderRow Is Nothing Then
            'objGPack.TextClear(pnlValueAdded)
            objGPack.TextClear(pnlMax)
            objGPack.TextClear(ObjMinValue)
            If Val(OrderRow!WASTPER.ToString) = 0 Then
                txtMaxWastage_Wet.Text = IIf(Val(OrderRow!WAST.ToString) <> 0, Format(Val(OrderRow!WAST.ToString), "0.000"), "")
            Else
                txtMaxWastage_Per.Text = IIf(Val(OrderRow!WASTPER.ToString) <> 0, Format(Val(OrderRow!WASTPER.ToString), "0.00"), "")
            End If
            If Val(OrderRow!MCGRM.ToString) = 0 Then
                txtMaxMkCharge_Amt.Text = IIf(Val(OrderRow!MC.ToString) <> 0, Format(Val(OrderRow!MC.ToString), "0.00"), "")
            Else
                txtMaxMcPerGrm_Amt.Text = IIf(Val(OrderRow!MCGRM.ToString) <> 0, Format(Val(OrderRow!MCGRM.ToString), "0.00"), "")
            End If
            If (Val(txtMaxWastage_Per.Text) <> 0 And Val(txtMaxWastage_Wet.Text) <> 0) Or (Val(txtMaxMkCharge_Amt.Text) <> 0 And Val(txtMaxWastage_Per.Text) <> 0) Then
                Exit Sub
            End If
        End If

        If cmbCalcMode.SelectedIndex = 0 And Val(txtGrossWt_Wet.Text) = 0 Then Exit Sub
        If cmbCalcMode.SelectedIndex <> 0 And Val(txtNetWt_Wet.Text) = 0 Then Exit Sub
        type = objGPack.GetSqlValue(" SELECT WMCTYPE FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = '" & SNO & "'") 'LOTNO = " & Val(txtLotNo_Num_Man.Text) & "")
        Select Case type
            Case "T"
                strSql = " DECLARE @WT FLOAT"
                If cmbCalcMode.SelectedIndex = 0 Then ''GRS WT
                    strSql += vbCrLf + " SET @WT = " & Val(txtGrossWt_Wet.Text) & ""
                Else ''NET WT
                    strSql += vbCrLf + " SET @WT = " & Val(txtNetWt_Wet.Text) & ""
                End If
                strSql += vbCrLf + " SELECT TOUCH,MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC,TOUCH_PUR,MAXWASTPER_PUR,MAXMCGRM_PUR,MAXWAST_PUR,MAXMC_PUR"
                strSql += vbCrLf + " ,MINWASTPER,MINMCGRM,MINWAST,MINMC FROM " & cnAdminDb & "..WMCTABLE "
                strSql += vbCrLf + " WHERE TABLECODE = '" & cmbTableCode.Text & "'"
                strSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
                strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
                strSql += vbCrLf + " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_Man.Text & "'),'')"
            Case "I"
                Dim dpcs As Integer = Val(dtStoneDetails.Compute("SUM(PCS)", "METALID='D'").ToString)

                strSql = "select count(*) FROM " & cnAdminDb & "..WMCTABLE "
                strSql += vbCrLf + " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "') and isnull(diafrompcs,0) <> 0 and isnull(diatopcs,0) <> 0 "
                If Val(objGPack.GetSqlValue(strSql).ToString) = 0 Then dpcs = 0

                strSql = " DECLARE @WT FLOAT,@DIAPCS INT"
                If cmbCalcMode.SelectedIndex = 0 Then ''GRS WT
                    strSql += vbCrLf + " SET @WT = " & Val(txtGrossWt_Wet.Text) & ""
                Else ''NET WT
                    strSql += vbCrLf + " SET @WT = " & Val(txtNetWt_Wet.Text) & ""
                End If
                strSql += vbCrLf + " SET @DIAPCS = " & dpcs & ""
                strSql += vbCrLf + " SELECT TOUCH,MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC,TOUCH_PUR,MAXWASTPER_PUR,MAXMCGRM_PUR,MAXWAST_PUR,MAXMC_PUR"
                strSql += vbCrLf + " ,MINWASTPER,MINMCGRM,MINWAST,MINMC FROM " & cnAdminDb & "..WMCTABLE "
                strSql += vbCrLf + " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')"
                strSql += vbCrLf + " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "' AND ITEMID = ( SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')),0)"
                strSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
                If dpcs <> 0 Then strSql += vbCrLf + " AND @DIAPCS BETWEEN DIAFROMPCS AND DIATOPCS "
                strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
                strSql += vbCrLf + " AND ISNULL(TABLECODE,'') = ''"
                strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_Man.Text & "'),'')"
            Case "D"
                strSql = " DECLARE @WT FLOAT"
                If cmbCalcMode.SelectedIndex = 0 Then ''GRS WT
                    strSql += vbCrLf + " SET @WT = " & Val(txtGrossWt_Wet.Text) & ""
                Else ''NET WT
                    strSql += vbCrLf + " SET @WT = " & Val(txtNetWt_Wet.Text) & ""
                End If
                strSql += vbCrLf + " SELECT TOUCH,MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC,TOUCH_PUR,MAXWASTPER_PUR,MAXMCGRM_PUR,MAXWAST_PUR,MAXMC_PUR"
                strSql += vbCrLf + " ,MINWASTPER,MINMCGRM,MINWAST,MINMC FROM " & cnAdminDb & "..WMCTABLE "
                strSql += vbCrLf + " WHERE ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "' UNION SELECT 0)"
                strSql += vbCrLf + " AND SUBITEMID IN (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')  UNION SELECT 0)"
                strSql += vbCrLf + " AND DESIGNERID = ISNULL((SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesigner_MAN.Text & "'),0)"
                strSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
                strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
                strSql += vbCrLf + " AND ISNULL(TABLECODE,'') = ''"
                strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_Man.Text & "'),'')"
            Case "P"
                strSql = " DECLARE @WT FLOAT"
                If cmbCalcMode.SelectedIndex = 0 Then ''GRS WT
                    strSql += vbCrLf + " SET @WT = " & Val(txtGrossWt_Wet.Text) & ""
                Else ''NET WT
                    strSql += vbCrLf + " SET @WT = " & Val(txtNetWt_Wet.Text) & ""
                End If
                strSql += vbCrLf + " SELECT TOUCH,MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC,TOUCH_PUR,MAXWASTPER_PUR,MAXMCGRM_PUR,MAXWAST_PUR,MAXMC_PUR"
                strSql += vbCrLf + " ,MINWASTPER,MINMCGRM,MINWAST,MINMC FROM " & cnAdminDb & "..WMCTABLE "
                strSql += vbCrLf + " WHERE ITEMTYPE = (SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType_MAN.Text & "')"
                strSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
                strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
                strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_Man.Text & "'),'')"
        End Select
        If type = Nothing Then
            Exit Sub
        End If
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            If Not tagEdit Then
                If PUR_AUTOCALC Then
                    ObjPurDetail.txtPurTouch_Amt.Clear()
                    ObjPurDetail.txtPurMakingChrg_Amt.Clear()
                    ObjPurDetail.txtPurWastage_Wet.Clear()
                    ObjPurDetail.txtPurMcPerGrm_Amt.Text = lotPurMcPerGrm
                    ObjPurDetail.txtPurWastage_Per.Text = lotPurWastagePer
                    ObjPurDetail.txtPurTouch_Amt.Text = lotPurTouch
                End If
                txtTouch_AMT.Clear()
                'objGPack.TextClear(pnlValueAdded)
                objGPack.TextClear(pnlMax)
                objGPack.TextClear(ObjMinValue)
                If type = "I" Then
                    If GetSoftValue("MAXMCFOCUS") = "N" Then pnlMax.Enabled = True
                End If
                Exit Sub
            End If
        Else
            txtTouch_AMT.Clear()
            objGPack.TextClear(pnlMax)
            objGPack.TextClear(ObjMinValue)
            'objGPack.TextClear(pnlValueAdded)
            With dt.Rows(0)

                Dim wmcWastPer As Double = Val(.Item("MAXWASTPER").ToString)
                Dim wmcWast As Double = Val(.Item("MAXWAST").ToString)
                Dim wmcMcGrm As Double = Val(.Item("MAXMCGRM").ToString)
                Dim wmcMc As Double = Val(.Item("MAXMC").ToString)
                If type = "I" Then
                    If GetSoftValue("MAXMCFOCUS") = "N" Then
                        If wmcWastPer = 0 And wmcWast = 0 And wmcMcGrm = 0 And wmcMc = 0 Then pnlMax.Enabled = True Else pnlMax.Enabled = False
                    End If
                    '  If dt.Rows.Count > 1 Then wmcMcGrm = Val(dt.Rows(1).Item("Maxmcgrm").ToString) : wmcMc = Val(dt.Rows(1).Item("Maxmc").ToString)
                End If
                If wmcWastPer = 0 Then
                    txtMaxWastage_Wet.Text = IIf(Val(.Item("MAXWAST").ToString) <> 0, Format(Val(.Item("MAXWAST").ToString), "0.000"), "")
                Else
                    txtMaxWastage_Per.Text = IIf(Val(.Item("MAXWASTPER").ToString) <> 0, Format(Val(.Item("MAXWASTPER").ToString), "0.00"), "")
                End If
                If wmcMcGrm = 0 Then
                    txtMaxMkCharge_Amt.Text = IIf(Val(.Item("MAXMC").ToString) <> 0, Format(Val(.Item("MAXMC").ToString), "0.00"), "")
                Else
                    txtMaxMcPerGrm_Amt.Text = IIf(Val(.Item("MAXMCGRM").ToString) <> 0, Format(Val(.Item("MAXMCGRM").ToString), "0.00"), "")
                End If
                wmcWastPer = Val(.Item("MINWASTPER").ToString)
                wmcWast = Val(.Item("MINWAST").ToString)
                wmcMcGrm = Val(.Item("MINMCGRM").ToString)
                wmcMc = Val(.Item("MINMC").ToString)
                txtTouch_AMT.Text = IIf(Val(.Item("TOUCH").ToString) <> 0, Format(Val(.Item("TOUCH").ToString), "0.00"), "")
                If wmcWastPer = 0 Then
                    ObjMinValue.txtMinWastage_Wet.Text = IIf(Val(.Item("MINWAST").ToString) <> 0, Format(Val(.Item("MINWAST").ToString), "0.000"), "")
                Else
                    ObjMinValue.txtMinWastage_Per.Text = IIf(Val(.Item("MINWASTPER").ToString) <> 0, Format(Val(.Item("MINWASTPER").ToString), "0.00"), "")
                End If
                If wmcMcGrm = 0 Then
                    ObjMinValue.txtMinMkCharge_Amt.Text = IIf(Val(.Item("MINMC").ToString) <> 0, Format(Val(.Item("MINMC").ToString), "0.00"), "")
                Else
                    ObjMinValue.txtMinMcPerGram_Amt.Text = IIf(Val(.Item("MINMCGRM").ToString) <> 0, Format(Val(.Item("MINMCGRM").ToString), "0.00"), "")
                End If

                If PUR_AUTOCALC Then
                    '',TOUCH_PUR,MAXWASTPER_PUR,MAXMCGRM_PUR,MAXWAST_PUR,MAXMC_PUR"
                    ObjPurDetail.txtPurTouch_Amt.Text = IIf(Val(.Item("TOUCH_PUR").ToString) <> 0, Val(.Item("TOUCH_PUR").ToString), "")
                    ObjPurDetail.txtPurWastage_Per.Text = IIf(Val(.Item("MAXWASTPER_PUR").ToString) <> 0, Val(.Item("MAXWASTPER_PUR").ToString), "")
                    ObjPurDetail.txtPurWastage_Wet.Text = IIf(Val(.Item("MAXWAST_PUR").ToString) <> 0, Val(.Item("MAXWAST_PUR").ToString), "")
                    ObjPurDetail.txtPurMcPerGrm_Amt.Text = IIf(Val(.Item("MAXMCGRM_PUR").ToString) <> 0, Val(.Item("MAXMCGRM_PUR").ToString), "")
                    ObjPurDetail.txtPurMakingChrg_Amt.Text = IIf(Val(.Item("MAXMC_PUR").ToString) <> 0, Val(.Item("MAXMC_PUR").ToString), "")
                    ObjPurDetail.txtPurTouch_TextChanged(Me, New EventArgs)
                    ObjPurDetail.txtPurWastagePer_TextChanged(Me, New EventArgs)
                    ObjPurDetail.txtPurMcPerGrm_TextChanged(Me, New EventArgs)
                End If
            End With
        End If

    End Sub

    Private Sub cmbTableCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTableCode.SelectedIndexChanged
        CalcMaxMinValues()
    End Sub

    Private Sub gridOtherDetail_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs)
        Select Case e.RowIndex
            Case 0, 1, 2, 4, 9, 10, 11, 12
                e.CellStyle.BackColor = Drawing.Color.White
            Case 3, 5, 6, 7, 8, 13, 14, 15, 16
                e.CellStyle.BackColor = Drawing.Color.WhiteSmoke
        End Select
        e.CellStyle.SelectionBackColor = e.CellStyle.BackColor
        e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor
    End Sub

    Private Sub GetGrsWeightFromPort()

        Dim Wt_Balance_Sep As String = GetAdmindbSoftValue("WT_BALANCE_SEP", "")
        Dim Weight As Double = Nothing
        Try
            If wSerialPort1.IsOpen Then wSerialPort1.Close()
            wSerialPort1.Open()
            If wSerialPort1.IsOpen Then
                Dim readStr As String = Nothing
                If Wt_Balance_Sep <> "" Then
                    readStr = UCase(wSerialPort1.ReadTo(Wt_Balance_Sep))
                    If IsNumeric(readStr.Substring(readStr.Length - 6)) Then
                        readStr = (Val(readStr.Substring(readStr.Length - 6)) / 1000).ToString()
                    End If
                    Weight = Val(Trim(readStr))
                Else
                    For cnt As Integer = 1 To 10
                        readStr = UCase(wSerialPort1.ReadLine)
                        If readStr.Contains(".") Then
                            Exit For
                        End If
                    Next
                    Dim wt() As String = readStr.Split(Environment.NewLine)
                    Dim wet As String = ""
                    For Each c As Char In wt(0)
                        If c = "," Then Continue For
                        If Char.IsPunctuation(c) Then wet += c
                        If Char.IsNumber(c) Then wet += c
                    Next
                    Weight = Val(Trim(wet))
                    ReadData.Text = readStr
                    SplitData.Text = wt(0)
                    ModifyData.Text = wet
                End If
            End If
            If wSerialPort1.IsOpen Then wSerialPort1.Close()
        Catch ex As Exception
            txtTagNo__Man.Focus()
            MsgBox("Please check com connection" + vbCrLf + ex.Message + vbCrLf + ex.StackTrace)
            If wSerialPort1.IsOpen Then wSerialPort1.Close()
        End Try
        If Weight = 0 Then txtGrossWt_Wet.Text = "" : Exit Sub
        Dim rndDigit As Integer = 0
        Dim METALID As String = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'")
        If METALID = "S" Then
            rndDigit = Val(GetSoftValue("ROUNDOFF-SILVER"))
        ElseIf METALID = "G" Then
            rndDigit = Val(GetSoftValue("ROUNDOFF-GOLD"))
        Else
            rndDigit = 3
        End If
        Weight = Math.Round(Weight, rndDigit)
        txtGrossWt_Wet.Text = IIf(Weight <> 0, Format(Weight, "0.000"), Nothing)
        txtGrossWt_Wet.SelectAll()
    End Sub


    Private Sub txtGrossWt_Wet_Man_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGrossWt_Wet.GotFocus
        If tagEdit Then Exit Sub
        If chkAutomaticWt.Checked = True Then
            txtGrossWt_Wet.ReadOnly = True
            GetGrsWeightFromPort()
        Else
            txtGrossWt_Wet.ReadOnly = False
        End If
    End Sub



    Private Sub txtGrossWt_Wet_Man_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtGrossWt_Wet.KeyPress
        If e.KeyChar = Chr(Keys.Enter) And Val(txtGrossWt_Wet.Text) = 0 Then

            If cmbSubItem_Man.Text <> "" Then
                strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')"
            Else
                strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"
            End If
            Dim calcType As String = objGPack.GetSqlValue(strSql)
            ''Weight Rate Validation
            Select Case calcType.ToUpper
                Case "W"
                    If Val(txtGrossWt_Wet.Text) = 0 Then
                        MsgBox("Grs Weight should not empty", MsgBoxStyle.Information)
                        txtGrossWt_Wet.Focus()
                        Exit Sub
                    End If
                Case "B"
                    If Val(txtGrossWt_Wet.Text) = 0 Then
                        MsgBox("Grs Weight should not empty", MsgBoxStyle.Information)
                        txtGrossWt_Wet.Focus()
                        Exit Sub
                    End If
                    If Val(txtRate_Amt.Text) = 0 Then
                        MsgBox("Rate should not empty", MsgBoxStyle.Information)
                        txtRate_Amt.Focus()
                        Exit Sub
                    End If
                Case "F"
                    SendKeys.Send("{TAB}")
                    Exit Sub
            End Select
            Exit Sub
        End If
        If e.KeyChar = Chr(Keys.Enter) And Val(txtGrossWt_Wet.Text) > 0 Then


            Dim tempitemid As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') = '" & cmbItem_MAN.Text & "'").ToString)
            Dim tempSitemid As Integer
            If cmbSubItem_Man.Text <> "" Then
                tempSitemid = objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE ISNULL(SUBITEMNAME,'') = '" & cmbSubItem_Man.Text & "'")
            End If
            If CHKBOOKSTOCK = True Then
                'Check Bookstock
                Dim tempcatcode As String = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') = '" & cmbItem_MAN.Text & "'").ToString
                TChkbStk = False
                If CheckBookstk(tempcatcode) < 0 Then
                    MsgBox("Closing Book stock weight is less than zero.")
                    txtGrossWt_Wet.Text = ""
                    btnSave.Enabled = False
                    txtGrossWt_Wet.Focus()
                    Exit Sub
                End If
            End If

            If Mid(STUDDEDWTPER, 1, 1) <> "N" And objGPack.GetSqlValue("SELECT ISNULL(STUDDEDSTONE,'')STUDDEDSTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'", , "N", ) = "Y" Then
                objstudPer = New StuddedDeDuctPer
                If objstudPer.ShowDialog = Windows.Forms.DialogResult.OK Then
                    Dim StudDet() As String = STUDDEDWTPER.Split(",")
                    Dim Stnitemid As Integer = 0
                    If StudDet.Length > 1 Then Stnitemid = Val(StudDet(1).ToString) 'Val(dtdesgstnrow(2).ToString)
                    Dim StudRate As Decimal = 0
                    If StudDet.Length > 2 Then StudRate = Val(StudDet(2).ToString)
                    If Stnitemid = 0 Then Stnitemid = 9999
                    Dim StudDeductPer As Decimal = Val(objstudPer.txtStuddedPer_PER.Text)


                    Dim StnSitemid As Integer = 0 'Val(dtdesgstnrow(3).ToString)
                    Dim dtstnitemdr As DataRow
                    Dim dtstnsitemdr As DataRow
                    Dim tempwtunit As String
                    Dim tempcalmode As String
                    dtstnitemdr = GetSqlRow("SELECT ITEMNAME,CALTYPE,STONEUNIT FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Stnitemid, cn)
                    tempwtunit = dtstnitemdr(2).ToString
                    tempcalmode = dtstnitemdr(1).ToString
                    If StnSitemid <> 0 Then
                        dtstnsitemdr = GetSqlRow("SELECT SUBITEMNAME,CALTYPE,STONEUNIT FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = " & StnSitemid, cn)
                        tempwtunit = dtstnitemdr(2).ToString
                        tempcalmode = dtstnitemdr(1).ToString
                    End If

                    Dim stnwt As Decimal = (StudDeductPer / 100) * Val(txtGrossWt_Wet.Text)
                    Dim drstn As DataRow
                    dtStoneDetails.Rows.Clear()
                    drstn = dtStoneDetails.NewRow
                    drstn("ITEM") = dtstnitemdr(0)
                    drstn("METALID") = "S"
                    drstn("SUBITEM") = StnSitemid
                    drstn("PCS") = 0
                    drstn("WEIGHT") = stnwt
                    drstn("UNIT") = tempwtunit
                    drstn("CALC") = tempcalmode
                    drstn("RATE") = StudRate 'dtdesgstnrow(1)
                    drstn("AMOUNT") = stnwt * StudRate ' Val(dtdesgstnrow(1).ToString)
                    drstn("STNSNO") = 1
                    dtStoneDetails.Rows.Add(drstn)
                    StyleGridStone()
                    CalcLessWt()
                    CalcFinalTotal()
                    'txtNetWt_Wet.Focus()
                    GoTo StudContinue
                End If
            End If
            Dim dtdesgstnrow As DataRow
            Dim tempDesignerid As Integer = objGPack.GetSqlValue("SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE ISNULL(DESIGNERNAME,'') = '" & cmbDesigner_MAN.Text & "'")
            dtdesgstnrow = GetSqlRow("SELECT STNDEDPER,STN_RATE,STNITEMID,STNSUBITEMID FROM " & cnAdminDb & "..DESIGNERSTONE WHERE ACTIVE <> 'N' AND DESIGNERID = " & tempDesignerid & " AND ITEMID = " & tempitemid & " AND SUBITEMID = " & tempSitemid, cn)
            If Not dtdesgstnrow Is Nothing Then
                Dim Stnitemid As Integer = Val(dtdesgstnrow(2).ToString)
                If Stnitemid = 0 Then Stnitemid = 9999

                Dim StnSitemid As Integer = Val(dtdesgstnrow(3).ToString)
                Dim dtstnitemdr As DataRow
                Dim dtstnsitemdr As DataRow
                Dim tempwtunit As String
                Dim tempcalmode As String
                dtstnitemdr = GetSqlRow("SELECT ITEMNAME,CALTYPE,STONEUNIT FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Stnitemid, cn)
                tempwtunit = dtstnitemdr(2).ToString
                tempcalmode = dtstnitemdr(1).ToString
                If StnSitemid <> 0 Then
                    dtstnsitemdr = GetSqlRow("SELECT SUBITEMNAME,CALTYPE,STONEUNIT FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = " & StnSitemid, cn)
                    tempwtunit = dtstnitemdr(2).ToString
                    tempcalmode = dtstnitemdr(1).ToString
                End If

                Dim stnwt As Decimal = (Val(dtdesgstnrow(0).ToString) / 100) * Val(txtGrossWt_Wet.Text)
                Dim drstn As DataRow
                dtStoneDetails.Rows.Clear()
                drstn = dtStoneDetails.NewRow
                drstn("ITEM") = dtstnitemdr(0)
                drstn("METALID") = "S"
                drstn("SUBITEM") = StnSitemid
                drstn("PCS") = 0
                drstn("WEIGHT") = stnwt
                drstn("UNIT") = tempwtunit
                drstn("CALC") = tempcalmode
                drstn("RATE") = dtdesgstnrow(1)
                drstn("AMOUNT") = stnwt * Val(dtdesgstnrow(1).ToString)
                drstn("STNSNO") = 1
                dtStoneDetails.Rows.Add(drstn)
                CalcLessWt()
                CalcFinalTotal()
                txtNetWt_Wet.Focus()
                Exit Sub
            End If
StudContinue:
            If _FourCMaintain Then
                strSql = "SELECT STUDDED FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & tempitemid & "  AND METALID='D'"
                If objGPack.GetSqlValue(strSql).ToString.ToUpper = "L" Then
                    If Not tagEdit Then
                        ObjDiaDetails = New frmDiamondDetails
                    End If
                    ObjDiaDetails.CmbCut.Focus()
                    ObjDiaDetails.Location = New Drawing.Point(481, 200)
                    ObjDiaDetails.Size = New Drawing.Size(261, 175)
                    Dim view4c As String = ""
                    If cmbItem_MAN.Text <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItem_MAN.Text & "'", , , )
                    If cmbSubItem_Man.Text <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & cmbSubItem_Man.Text & "' AND ITEMID=(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItem_MAN.Text & "')", , , )
                    If Not view4c.Contains("CU") Then ObjDiaDetails.CmbCut.Enabled = False
                    If Not view4c.Contains("CO") Then ObjDiaDetails.CmbColor.Enabled = False
                    If Not view4c.Contains("CL") Then ObjDiaDetails.CmbClarity.Enabled = False
                    If Not view4c.Contains("SH") Then ObjDiaDetails.cmbShape.Enabled = False
                    If Not view4c.Contains("SE") Then ObjDiaDetails.cmbSetType.Enabled = False
                    If Not view4c.Contains("HE") Then ObjDiaDetails.txtHeight_WET.Enabled = False
                    If Not view4c.Contains("WI") Then ObjDiaDetails.txtWidth_WET.Enabled = False
                    ObjDiaDetails.ShowDialog()
                    TagCutId = Val(objGPack.GetSqlValue(" SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & ObjDiaDetails.CmbCut.Text & "'", "CUTID", , tran))
                    TagColorId = Val(objGPack.GetSqlValue(" SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME = '" & ObjDiaDetails.CmbColor.Text & "'", "COLORID", , tran))
                    TagClarityId = Val(objGPack.GetSqlValue(" SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & ObjDiaDetails.CmbClarity.Text & "'", "CLARITYID", , tran))
                    TagShapeId = Val(objGPack.GetSqlValue(" SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & ObjDiaDetails.cmbShape.Text & "'", "SHAPEID", , tran))
                    TagSetTypeId = Val(objGPack.GetSqlValue(" SELECT SETTYPEID FROM " & cnAdminDb & "..STNSETTYPE WHERE SETTYPENAME = '" & ObjDiaDetails.cmbSetType.Text & "'", "SETTYPEID", , tran))
                    TagHeight = Val(ObjDiaDetails.txtHeight_WET.Text.ToString)
                    TagWidth = Val(ObjDiaDetails.txtWidth_WET.Text.ToString)
                End If
            End If
            If objGPack.GetSqlValue("SELECT EXTRAWT FROM " & cnAdminDb & "..ITEMMAST WHERE itemid = " & tempitemid & "", "EXTRAWT", "N") = "Y" Then
                'If Me.Controls.Contains(grpExtraWt) = False Then
                '    Me.Controls.Add(grpExtraWt)
                'End If
                'grpExtraWt.Location = New Point(480, 175)
                'grpExtraWt.Visible = True
                'txtExtraWt_WET.Focus()
                'grpExtraWt.BringToFront()
                'ObjExtraWt.Visible = True
                'ObjExtraWt.StartPosition = FormStartPosition.CenterScreen
                'ObjExtraWt.BringToFront()
                ObjExtraWt.txtExtraWt_WET.Focus()
                ObjExtraWt.ShowDialog()
            Else
                'ObjExtraWt.Visible = False
            End If
            strSql = " SELECT mcasvaper FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & tempitemid & ""
            If objGPack.GetSqlValue(strSql).ToString = "Y" Then Label44.Text = "MC PERCENT" Else Label44.Text = "Max Mc Per Gram"
            If cmbSubItem_Man.Text <> "" Then
                strSql = " SELECT mcasvaper FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = " & tempitemid & " AND SUBITEMID=" & tempSitemid & ""
                If objGPack.GetSqlValue(strSql).ToString = "Y" Then Label44.Text = "MC PERCENT" Else Label44.Text = "Max Mc Per Gram"
            End If

            With TabControl1
                If .TabPages.Contains(tabMultiMetal) Then
                    .SelectedTab = tabMultiMetal
                    Me.SelectNextControl(tabMultiMetal, True, True, True, True)
                ElseIf .TabPages.Contains(tabStone) And pnlStDet.Enabled = True Then
                    .SelectedTab = tabStone
                    If PacketNoEnable <> "N" Then
                        txtstPackettno.Focus()
                    Else
                        Me.SelectNextControl(tabStone, True, True, True, True)
                    End If

                ElseIf .TabPages.Contains(tabOtherCharges) Then
                    .SelectedTab = tabOtherCharges
                    Me.SelectNextControl(tabOtherCharges, True, True, True, True)
                Else
                    SendKeys.Send("{TAB}")
                End If
            End With
        ElseIf e.KeyChar = Chr(Keys.Space) Then
            GetGrsWeightFromPort()
            e.Handled = True
        End If
    End Sub

    Function CheckBookstk(ByVal Mcatcode As String) As Decimal
        Dim Mcostid As String = ""
        Dim Mgrswt As Decimal = 0
        If cmbCostCentre_Man.Text <> "" Then Mcostid = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre_Man.Text & "'")
        strSql = " SELECT SUM(ISNULL(GRSWT,0)) GRSWT FROM ("
        strSql += vbCrLf + " SELECT SUM(GRSWT) AS GRSWT FROM " & cnStockDb & "..OPENWEIGHT WHERE COMPANYID='" & GetStockCompId() & "' AND CATCODE='" & Mcatcode & "' AND STOCKTYPE='C' AND TRANTYPE='R' AND ISNULL(COSTID,'')='" & Mcostid & "' "
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT SUM(GRSWT)*(-1) AS GRSWT FROM " & cnStockDb & "..OPENWEIGHT WHERE COMPANYID='" & GetStockCompId() & "' AND CATCODE='" & Mcatcode & "' AND STOCKTYPE='C' AND TRANTYPE='I' AND ISNULL(COSTID,'')='" & Mcostid & "' "
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT SUM(GRSWT)*(-1) AS GRSWT FROM " & cnStockDb & "..ISSUE WHERE COMPANYID='" & GetStockCompId() & "' AND CATCODE='" & Mcatcode & "' AND ISNULL(COSTID,'')='" & Mcostid & "'  AND ISNULL(CANCEL,'')=''"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT SUM(GRSWT)GRSWT FROM " & cnStockDb & "..RECEIPT WHERE COMPANYID='" & GetStockCompId() & "' AND CATCODE='" & Mcatcode & "' AND ISNULL(COSTID,'')='" & Mcostid & "' AND ISNULL(CANCEL,'')='' "
        strSql += vbCrLf + " )X "
        Mgrswt = Val(objGPack.GetSqlValue(strSql))
        Return Mgrswt
    End Function

    Private Sub btnAttachImage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAttachImage.Click
        If Not IO.Directory.Exists(defalutDestination) Then
            MsgBox(defalutDestination & vbCrLf & " Destination Path not found" & vbCrLf & "Please make the correct path", MsgBoxStyle.Information)
            Exit Sub
        End If

        If flagDeviceMode = True Then
            piccap()
            SubItemPic = False
            btnSave.Focus()
            Exit Sub
        End If
        Try
            Dim openDia As New OpenFileDialog
            Dim str As String
            If IO.File.Exists(defalutSourcePath) Then openDia.InitialDirectory = defalutSourcePath
            str = "JPEG(*.jpg)|*.jpg"
            str += "|Bitmap(*.bmp)|*.bmp"
            str += "|GIF(*.gif)|*.gif"
            str += "|All Files(*.*)|*.*"
            openDia.Filter = str
            If openDia.ShowDialog = Windows.Forms.DialogResult.OK Then
                SubItemPic = False
                'picModel.Visible = True
                Dim Finfo As FileInfo
                Finfo = New FileInfo(openDia.FileName)
                'Finfo.IsReadOnly = False
                AutoImageSizer(openDia.FileName, picModel)
                'Dim fStream As New FileStream(openDia.FileName, FileMode.Open)
                'picModel.Image = Image.FromStream(fStream)
                'fStream.Close()
                picPath = openDia.FileName
                picExtension = Finfo.Extension
                'If openDia.FilterIndex = 1 Then
                '    picExtension = "JPG"
                'ElseIf openDia.FilterIndex = 2 Then
                '    picExtension = "BMP"
                'ElseIf openDia.FilterIndex = 2 Then
                '    picExtension = "GIF"
                'End If
                Me.SelectNextControl(btnAttachImage, True, True, True, True)
            Else
                Me.SelectNextControl(btnAttachImage, True, True, True, True)
            End If
        Catch ex As Exception
            MsgBox(E0016, MsgBoxStyle.Exclamation)
        End Try
    End Sub
    Private Sub piccap()
        Dim data As IDataObject
        Dim bmap As Image
        If File.Exists(Application.StartupPath & "\tst.jpg") Then File.Delete(Application.StartupPath & "\tst.jpg")
        ' Copy image to clipboard 
        SendMessage(hHwnd, CAP_EDIT_COPY, 0, 0)
        ' Get image from clipboard and convert it to a bitmap 
        Data = Clipboard.GetDataObject()
        If Data.GetDataPresent(GetType(System.Drawing.Bitmap)) Then
            bmap = CType(Data.GetData(GetType(System.Drawing.Bitmap)), Image)
            picModel.Image = bmap

            picPath = Application.StartupPath & "\tst.jpg"
            picExtension = "Jpg"
            picModel.Image.Save(picPath, System.Drawing.Imaging.ImageFormat.Jpeg)
            picCapture.Visible = False
        End If

    End Sub
    Private Sub OpenForm()
        Dim iHeight As Integer = picCapture.Height
        Dim iWidth As Integer = picCapture.Width
        ' Open Preview window in picturebox .
        ' Create a child window with capCreateCaptureWindowA so you can display it in a picturebox.

        hHwnd = capCreateCaptureWindowA(iDevice, WS_VISIBLE Or WS_CHILD, 0, 0, 640, _
            480, picCapture.Handle.ToInt32, 0)
        ' Connect to device
        If SendMessage(hHwnd, CAP_DRIVER_CONNECT, iDevice, 0) Then

            ' Set the preview scale
            SendMessage(hHwnd, CAP_SET_SCALE, True, 0)

            ' Set the preview rate in milliseconds
            SendMessage(hHwnd, CAP_SET_PREVIEWRATE, 66, 0)

            ' Start previewing the image from the camera 
            SendMessage(hHwnd, CAP_SET_PREVIEW, True, 0)

            ' Resize window to fit in picturebox 
            SetWindowPos(hHwnd, HWND_BOTTOM, 0, 0, picCapture.Width, picCapture.Height, _
                                   SWP_NOMOVE Or SWP_NOZORDER)

        Else
            ' Error connecting to device close window 
            DestroyWindow(hHwnd)

        End If
    End Sub


    Private Sub CloseCam()
        If flagDeviceMode = True Then
            ' Disconnect from device
            SendMessage(hHwnd, CAP_DRIVER_DISCONNECT, iDevice, 0)
            ' close window 
            DestroyWindow(hHwnd)
        End If
    End Sub

    Private Sub MemberLedgerForm_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Leave
        Call CloseCam()
    End Sub

    Private Sub btnAttachImage_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btnAttachImage.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.SelectNextControl(btnAttachImage, True, True, True, True)
        End If
    End Sub

#Region "TextChangedEvents"

    Private Sub txtPieces_Num_Man_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPieces_Num_Man.Leave
        If tabCheckBy = "P" Then
            'If Val(lblPBalance.Text) + Val(Tag_Tolerance) < Val(txtPieces_Num_Man.Text) - lotPcs Then
            '    MsgBox("Pcs should not exceed lot balance pcs", MsgBoxStyle.Information)
            '    txtPieces_Num_Man.Focus()
            'End If
            If STK_REORD_VALID = True Then StockChecking()
        End If
    End Sub

    Private Sub txtPieces_Num_Man_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPieces_Num_Man.TextChanged
        CalcFinalTotal()
    End Sub

    Private Sub txtGrossWt_Wet_Man_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGrossWt_Wet.Leave
        'If tabCheckBy <> "P" Then
        '    If Val(lblWBalance.Text) + Val(Tag_Tolerance) < Val(txtGrossWt_Wet.Text) - lotGrsWt Then
        '        MsgBox("Weight should not exceed lot balance weight", MsgBoxStyle.Information)
        '        txtGrossWt_Wet.Text = 0
        '        txtGrossWt_Wet.Focus()
        '    End If
        'End If
        If CHKBOOKSTOCK = True And TChkbStk = True Then
            'Check Bookstock
            Dim tempcatcode As String = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') = '" & cmbItem_MAN.Text & "'").ToString
            TChkbStk = False
            If CheckBookstk(tempcatcode) < 0 Then
                MsgBox("Closing Book stock weight is less than zero.")
                txtGrossWt_Wet.Text = ""
                btnSave.Enabled = False
                txtGrossWt_Wet.Focus()
                Exit Sub
            End If
        End If

    End Sub

    Private Sub txtNetWt_Wet_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNetWt_Wet.GotFocus
        If Val(txtGrossWt_Wet.Text) = 0 Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtNetWt_Wet_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNetWt_Wet.KeyPress

        '        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
        '            If txtRate_Amt.Enabled = True And NEEDUS = True Then
        'RsUSView:
        '                e.Handled = True
        '                If ObjRsUs.ShowDialog = Windows.Forms.DialogResult.OK Then
        '                    txtRate_Amt.Text = Val(ObjRsUs.txtUSDollar_Amt.Text) * Val(ObjRsUs.txtIndRs_Amt.Text)
        '                    If Val(txtRate_Amt.Text) = 0 Then
        '                        MsgBox("Rate should not empty", MsgBoxStyle.Information)
        '                        ObjRsUs.txtUSDollar_Amt.Focus()
        '                        GoTo RsUSView
        '                    End If

        '                    If cmbSubItem_Man.Text <> "" Then
        '                        strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')"
        '                    Else
        '                        strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"
        '                    End If
        '                    Dim calcType As String = objGPack.GetSqlValue(strSql)
        '                    ''Weight Rate Validation
        '                    Select Case calcType.ToUpper
        '                        Case "R"
        '                            If Val(txtRate_Amt.Text) = 0 Then
        '                                MsgBox("Rate should not empty", MsgBoxStyle.Information)
        '                                txtRate_Amt.Focus()
        '                                Exit Sub
        '                            End If
        '                        Case "B"
        '                            If Val(txtGrossWt_Wet.Text) = 0 Then
        '                                MsgBox("Grs Weight should not empty", MsgBoxStyle.Information)
        '                                txtGrossWt_Wet.Focus()
        '                                Exit Sub
        '                            End If
        '                            If Val(txtRate_Amt.Text) = 0 Then
        '                                MsgBox("Rate should not empty", MsgBoxStyle.Information)
        '                                txtRate_Amt.Focus()
        '                                Exit Sub
        '                            End If
        '                        Case "F"
        '                            If Val(txtGrossWt_Wet.Text) = 0 And Val(txtRate_Amt.Text) = 0 Then
        '                                MsgBox("Weight and Rate should not empty", MsgBoxStyle.Information)
        '                                txtGrossWt_Wet.Focus()
        '                                Exit Sub
        '                            End If
        '                    End Select
        '                End If
        '            End If
        '        End If
    End Sub


    Private Sub txtNetWt_Wet_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNetWt_Wet.TextChanged
        CalcMaxMinValues()
        CalcFinalTotal()
    End Sub

    Private Sub txtRate_Amt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRate_Amt.GotFocus
        'If txtRate_Amt.Enabled = True And NEEDUS = True Then
        '    'SendKeys.Send("{TAB}")S
        '    txtRate_Amt.ReadOnly = True
        'End If
        'If NEEDUS = True And (calType = "R" Or calType = "M") And Studded_Loose = "L" Then
        '    SendKeys.Send("{TAB}")
        'End If
    End Sub

    Private Sub txtRate_Amt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtRate_Amt.KeyPress
        If e.KeyChar = Chr(Keys.Enter) And Val(txtRate_Amt.Text) = 0 Then
            strSql = " SELECT ISNULL(STUDDED,'') STUDDED FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') ='" & cmbItem_MAN.Text & "' AND METALID = 'D'"
            Studded_Loose = objGPack.GetSqlValue(strSql, "STUDDED", "")
            If NEEDUS = True And Studded_Loose = "L" And Val(txtRate_Amt.Text) = 0 Then
                If calType = "M" Then
                    If ObjRsUs.ShowDialog = Windows.Forms.DialogResult.OK Then
                        txtRate_Amt.Text = Convert.ToDouble(Val(ObjRsUs.txtUSDollar_Amt.Text) * Val(ObjRsUs.txtIndRs_Amt.Text)).ToString("0.00")
                        'Me.SelectNextControl(txtRate_Amt, True, True, True, True)
                    End If
                End If
            End If

            If cmbSubItem_Man.Text <> "" Then
                strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')"
            Else
                strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"
            End If
            Dim calcType As String = objGPack.GetSqlValue(strSql)
            ''Weight Rate Validation
            Select Case calcType.ToUpper
                Case "R"
                    If Val(txtRate_Amt.Text) = 0 Then
                        MsgBox("Rate should not empty", MsgBoxStyle.Information)
                        txtRate_Amt.Focus()
                        Exit Sub
                    End If
                Case "B"
                    If Val(txtGrossWt_Wet.Text) = 0 Then
                        MsgBox("Grs Weight should not empty", MsgBoxStyle.Information)
                        txtGrossWt_Wet.Focus()
                        Exit Sub
                    End If
                    If Val(txtRate_Amt.Text) = 0 Then
                        MsgBox("Rate should not empty", MsgBoxStyle.Information)
                        txtRate_Amt.Focus()
                        Exit Sub
                    End If
                Case "F"
                    If Val(txtGrossWt_Wet.Text) = 0 And Val(txtRate_Amt.Text) = 0 Then
                        MsgBox("Weight and Rate should not empty", MsgBoxStyle.Information)
                        txtGrossWt_Wet.Focus()
                        Exit Sub
                    End If
            End Select
        End If
    End Sub

    Private Sub txtRate_Amt_Man_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRate_Amt.TextChanged
        CalcFinalTotal()
    End Sub

    Private Sub txtMaxWastage_Wet_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMaxWastage_Wet.GotFocus
        If Val(txtMaxWastage_Per.Text) > 0 Then txtMaxWastage_Wet.ReadOnly = True Else txtMaxWastage_Wet.ReadOnly = False
        If tagEdit And TAGEDITDISABLE.Contains("WS") Then txtMaxWastage_Wet.ReadOnly = True
    End Sub

    Private Sub txtMaxWastage_Wet_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMaxWastage_Wet.TextChanged
        Me.txtMaxMcPerGrm_TextChanged(Me, New EventArgs)
        CalcFinalTotal()
    End Sub

    Private Sub txtMaxMkCharge_Amt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMaxMkCharge_Amt.KeyPress
        If Val(txtMaxMcPerGrm_Amt.Text) > 0 Then
            e.Handled = True
        End If
        If e.KeyChar = Chr(Keys.Enter) Then
            If _HasMinMc Then

                ObjMinValue = New TagMinValues
                AddHandler ObjMinValue.txtMinWastage_Per.TextChanged, AddressOf ObjMinValues_txtMinWastage_Per_TextChanged
                AddHandler ObjMinValue.txtMinMcPerGram_Amt.TextChanged, AddressOf ObjMinValues_txtMinMcPerGram_Amt_TextChanged
                If tagEdit And TAGEDITDISABLE.Contains("MC") Then ObjMinValue.txtMinMcPerGram_Amt.ReadOnly = True : ObjMinValue.txtMinMkCharge_Amt.ReadOnly = True
                If tagEdit And TAGEDITDISABLE.Contains("WS") Then ObjMinValue.txtMinWastage_Per.ReadOnly = True : ObjMinValue.txtMinWastage_Wet.ReadOnly = True
                ObjMinValue.txtMinWastage_Per.Focus()
                ObjMinValue.ShowDialog()
            End If
            Me.SelectNextControl(txtMaxMkCharge_Amt, True, True, True, True)
        End If
    End Sub

    Private Sub txtMaxMkCharge_Amt_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMaxMkCharge_Amt.TextChanged
        CalcFinalTotal()
    End Sub

#End Region

#Region "MultiMetal Procedures"
    Private Sub CalcMultiMetalTotal()
        Dim wt As Double = Nothing
        Dim amt As Double = Nothing
        For cnt As Integer = 0 To gridMultimetal.Rows.Count - 1
            With gridMultimetal.Rows(cnt)
                wt += Val(.Cells("WEIGHT").Value.ToString)
                amt += Val(.Cells("AMOUNT").Value.ToString)
            End With
        Next
        gridMultiMetalTotal.Rows(0).Cells("WEIGHT").Value = IIf(wt <> 0, Format(wt, "0.000"), DBNull.Value)
        gridMultiMetalTotal.Rows(0).Cells("AMOUNT").Value = IIf(amt <> 0, Format(amt, "0.00"), DBNull.Value)
        txtMultiWt.Text = IIf(wt <> 0, Format(wt, "0.000"), "")
        txtMultiAmt.Text = IIf(amt <> 0, Format(amt, "0.00"), "")
    End Sub




    Private Sub gridMultimetal_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridMultimetal.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If

    End Sub

    Private Sub gridMultimetal_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridMultimetal.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            gridMultimetal.CurrentCell = gridMultimetal.Rows(gridMultimetal.CurrentRow.Index).Cells(0)
            With gridMultimetal.Rows(gridMultimetal.CurrentRow.Index)
                txtMMCategory.Text = .Cells("CATEGORY").FormattedValue
                txtMMWeight_Wet.Text = .Cells("WEIGHT").FormattedValue
                txtMMWastagePer_PER.Text = .Cells("WASTAGEPER").FormattedValue
                txtMMWastage_WET.Text = .Cells("WASTAGE").FormattedValue
                txtMMMcPerGRm_AMT.Text = .Cells("MCPERGRM").FormattedValue
                txtMMMc_AMT.Text = .Cells("MC").FormattedValue
                txtMMAmount_AMT.Text = .Cells("AMOUNT").FormattedValue
                txtMMRowIndex.Text = gridMultimetal.CurrentRow.Index
                txtMMCategory.Focus()
                txtMMCategory.SelectAll()
                '.Item("CATEGORY") = ""
                '.Item("WEIGHT") = IIf(Val(txtMMWeight_Wet.Text) <> 0, Val(txtMMWeight_Wet.Text), DBNull.Value)
                '.Item("RATE") = IIf(Val(txtMMRate_Amt.Text) <> 0, Val(txtMMRate_Amt.Text), DBNull.Value)
                '.Item("AMOUNT")
            End With
        End If
    End Sub

    Private Sub gridMultimetal_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridMultimetal.UserDeletedRow
        dtMultiMetalDetails.AcceptChanges()
        CalcFinalTotal()
        If Not gridMultimetal.RowCount > 0 Then
            txtMMCategory.Focus()
        End If
    End Sub

#End Region

#Region "Stone Procedures"
    Private Sub CalcStoneTotals()
        ''Calc Total
        Dim totPcs As Integer = Nothing
        Dim totWt As Double = Nothing
        Dim totAmt As Double = Nothing
        If TabControl1.TabPages.Contains(tabStone) Then
            For cnt As Integer = 0 To gridStone.RowCount - 1
                With gridStone.Rows(cnt)
                    totPcs += Val(.Cells("PCS").Value.ToString)
                    If .Cells("UNIT").Value.ToString = "C" Then
                        totWt += Val(.Cells("WEIGHT").Value.ToString) / 5
                    ElseIf .Cells("UNIT").Value.ToString = "G" Then
                        totWt += Val(.Cells("WEIGHT").Value.ToString)
                    End If
                    totAmt += Val(.Cells("AMOUNT").Value.ToString)
                End With
            Next
        End If
        With gridStoneFooter.Rows(0)
            .Cells("PCS").Value = IIf(totPcs <> 0, totPcs, DBNull.Value)
            .Cells("WEIGHT").Value = IIf(totWt <> 0, Format(totWt, FormatNumberStyle(DiaRnd)), DBNull.Value)
            .Cells("AMOUNT").Value = IIf(totAmt <> 0, Format(totAmt, "0.000"), DBNull.Value)
        End With
    End Sub

    Private Sub CalcStoneAmount()
        Dim amt As Double = Nothing
        If cmbStCalc.Text = "P" Then
            amt = Val(txtStRate_Amt.Text) * Val(txtStPcs_Num.Text)
        Else
            amt = Val(txtStRate_Amt.Text) * Val(txtStWeight.Text)
        End If
        amt = Math.Round(amt)
        txtStAmount_Amt.Text = IIf(amt <> 0, Format(amt, "0.00"), "")
    End Sub

    Private Sub txtStRate_Amt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStRate_Amt.GotFocus
        'If NEEDUS = True And Studded_Loose <> "L" Then
        '    SendKeys.Send("{TAB}")
        'End If
    End Sub

    Private Sub txtStRate_Amt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtStRate_Amt.KeyDown
        If e.KeyCode = Keys.Down Then
            gridStone.Select()
        End If
    End Sub

    Private Sub txtStRate_Amt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStRate_Amt.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            strSql = " SELECT ISNULL(STUDDED,'') STUDDED FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') ='" & txtStItem.Text & "' AND METALID = 'D'"
            Studded_Loose = objGPack.GetSqlValue(strSql, "STUDDED", "")
            If NEEDUS = True And Studded_Loose <> "" And Val(txtStRate_Amt.Text) = 0 Then
                'If calType = "M" Then
                If ObjRsUs.ShowDialog = Windows.Forms.DialogResult.OK Then
                    txtStRate_Amt.Text = Convert.ToDouble(Val(ObjRsUs.txtUSDollar_Amt.Text) * Val(ObjRsUs.txtIndRs_Amt.Text)).ToString("0.00")
                End If
                'End If
            End If

            Dim cent As Double = 0
            strSql = " SELECT ISNULL(CALTYPE,'') CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') ='" & txtStItem.Text & "'"
            Dim mCaltype As String = objGPack.GetSqlValue(strSql, "CALTYPE", "")
            strSql = " SELECT ISNULL(CALTYPE,'') CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE ISNULL(SUBITEMNAME,'') ='" & txtStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')"
            mCaltype = objGPack.GetSqlValue(strSql, "CALTYPE", "")
            If mCaltype = "D" Then
                cent = Val(txtStWeight.Text)
            Else
                cent = (Val(txtStWeight.Text) / IIf(Val(txtStPcs_Num.Text) = 0, 1, Val(txtStPcs_Num.Text)))
            End If
            'If cmbStUnit.Text = "C" Then cent = (Val(txtStWeight.Text) / IIf(Val(txtStPcs_Num.Text) = 0, 1, Val(txtStPcs_Num.Text))) Else cent = Val(txtStWeight.Text) / IIf(Val(txtStPcs_Num.Text) = 0, 1, Val(txtStPcs_Num.Text))
            cent *= 100
            strSql = " DECLARE @CENT FLOAT"
            strSql += " SET @CENT = " & cent & ""
            strSql += " SELECT MINRATE FROM " & cnAdminDb & "..CENTRATE WHERE"
            strSql += " @CENT BETWEEN FROMCENT AND TOCENT "
            strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')"
            strSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')),0)"
            If CENTRATE_DESIGNER Then strSql += " AND ISNULL(DESIGNERID,0) =ISNULL((SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME='" & cmbDesigner_MAN.Text & "'),0)"
            If cmbCostCentre_Man.Text.Trim <> "" Then strSql += " AND COSTID IN(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre_Man.Text & "')"
            Dim rate As Double = Val(objGPack.GetSqlValue(strSql, , , tran))
            If Val(txtStRate_Amt.Text) < rate Then
                MsgBox(Me.GetNextControl(txtStRate_Amt, False).Text + E0020 + rate.ToString)
                txtStRate_Amt.Focus()
            End If
        End If
    End Sub

    Private Sub txtStRate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStRate_Amt.TextChanged
        CalcStoneAmount()
    End Sub

    Private Sub txtStAmount_Amt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtStAmount_Amt.KeyDown
        If e.KeyCode = Keys.Down Then
            gridStone.Select()
        End If
    End Sub

    Private Sub txtStAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStAmount_Amt.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            ''Validation
            If objGPack.Validator_Check(grpStoneDetails) Then Exit Sub

            If objGPack.DupCheck("SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "' AND  STUDDED = 'S' AND ACTIVE = 'Y'") = False Then
                MsgBox("Invalid Item", MsgBoxStyle.Information)
                txtStItem.Focus()
                Exit Sub
            End If
            If UCase(objGPack.GetSqlValue("SELECT SUBITEM FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "' AND STUDDED = 'S' AND ACTIVE = 'Y'")) = "Y" Then
                If txtStSubItem.Text = "" Then
                    MsgBox("SubItem Should Not Empty", MsgBoxStyle.Information)
                    txtStSubItem.Focus()
                    Exit Sub
                End If
                If objGPack.DupCheck("SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "') AND SUBITEMNAME = '" & txtStSubItem.Text & "'") = False Then
                    MsgBox("Invalid SubItem", MsgBoxStyle.Information)
                    txtStSubItem.Focus()
                    Exit Sub
                End If
            Else
                txtStSubItem.Clear()
            End If
            If Val(txtStPcs_Num.Text) = 0 And Val(txtStWeight.Text) = 0 And Val(txtStAmount_Amt.Text) = 0 Then
                MsgBox(Me.GetNextControl(txtStPcs_Num, False).Text + "," + Me.GetNextControl(txtStWeight, False).Text + E0001, MsgBoxStyle.Information)
                txtStItem.Focus()
                Exit Sub
            End If
            Dim cent As Double = Nothing

            strSql = " SELECT ISNULL(CALTYPE,'') CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') ='" & txtStItem.Text & "'"
            Dim mCaltype As String = objGPack.GetSqlValue(strSql, "CALTYPE", "")
            strSql = " SELECT ISNULL(CALTYPE,'') CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE ISNULL(SUBITEMNAME,'') ='" & txtStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')"
            mCaltype = objGPack.GetSqlValue(strSql, "CALTYPE", "")
            If mCaltype = "D" Then
                cent = Val(txtStWeight.Text)
            Else
                cent = (Val(txtStWeight.Text) / IIf(Val(txtStPcs_Num.Text) = 0, 1, Val(txtStPcs_Num.Text)))
            End If

            'If cmbStUnit.Text = "C" Then cent = (Val(txtStWeight.Text) / IIf(Val(txtStPcs_Num.Text) = 0, 1, Val(txtStPcs_Num.Text))) Else cent = Val(txtStWeight.Text) / IIf(Val(txtStPcs_Num.Text) = 0, 1, Val(txtStPcs_Num.Text))
            cent *= 100
            strSql = " DECLARE @CENT FLOAT"
            strSql += " SET @CENT = " & cent & ""
            strSql += " SELECT MINRATE FROM " & cnAdminDb & "..CENTRATE WHERE"
            strSql += " @CENT BETWEEN FROMCENT AND TOCENT "
            strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')"
            strSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')),0)"
            If CENTRATE_DESIGNER Then strSql += " AND ISNULL(DESIGNERID,0) =ISNULL((SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME='" & cmbDesigner_MAN.Text & "'),0)"
            If cmbCostCentre_Man.Text.Trim <> "" Then strSql += " AND COSTID IN(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre_Man.Text & "')"
            Dim rate As Double = Val(objGPack.GetSqlValue(strSql, , , tran))
            If Val(txtStRate_Amt.Text) < rate Then
                MsgBox(Me.GetNextControl(txtStRate_Amt, False).Text + E0020 + rate.ToString)
                txtStRate_Amt.Focus()
                Exit Sub
            End If
            Dim stWeight As Double = IIf(cmbStUnit.Text = "C", Val(txtStWeight.Text) / 5, Val(txtStWeight.Text))
            For cnt As Integer = 0 To gridStone.RowCount - 1
                If txtStRowIndex.Text <> "" Then If Val(txtStRowIndex.Text) = cnt Then Continue For
                With gridStone.Rows(cnt)
                    If .Cells("UNIT").Value.ToString = "C" Then
                        stWeight += Val(.Cells("WEIGHT").Value.ToString) / 5
                    ElseIf .Cells("UNIT").Value.ToString = "G" Then
                        stWeight += Val(.Cells("WEIGHT").Value.ToString)
                    End If
                End With
            Next
            If stWeight > Val(txtGrossWt_Wet.Text) Then
                MsgBox(Me.GetNextControl(txtStWeight, False).Text + E0015 + Me.GetNextControl(txtGrossWt_Wet, False).Text, MsgBoxStyle.Exclamation)
                txtStWeight.Focus()
                Exit Sub
            End If
            If txtStRowIndex.Text <> "" Then
                'If MessageBox.Show("Would you like to update this Entry", "Update Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.OK Then
                With gridStone.Rows(Val(txtStRowIndex.Text))
                    .Cells("PACKETNO").Value = txtstPackettno.Text
                    .Cells("ITEM").Value = txtStItem.Text
                    .Cells("SUBITEM").Value = txtStSubItem.Text
                    .Cells("UNIT").Value = cmbStUnit.Text
                    .Cells("CALC").Value = cmbStCalc.Text
                    .Cells("PCS").Value = IIf(Val(txtStPcs_Num.Text) <> 0, Val(txtStPcs_Num.Text), DBNull.Value)
                    .Cells("WEIGHT").Value = IIf(Val(txtStWeight.Text) <> 0, Format(Val(txtStWeight.Text), FormatNumberStyle(DiaRnd)), DBNull.Value)
                    .Cells("RATE").Value = IIf(Val(txtStRate_Amt.Text) <> 0, Format(Val(txtStRate_Amt.Text), "0.00"), DBNull.Value)
                    .Cells("AMOUNT").Value = IIf(Val(txtStAmount_Amt.Text) <> 0, Format(Val(txtStAmount_Amt.Text), "0.00"), DBNull.Value)
                    .Cells("METALID").Value = txtStMetalCode.Text
                    .Cells("USRATE").Value = IIf(NEEDUS = True And Studded_Loose <> "L", Val(ObjRsUs.txtUSDollar_Amt.Text), 0)
                    .Cells("INDRS").Value = IIf(NEEDUS = True And Studded_Loose <> "L", Val(ObjRsUs.txtIndRs_Amt.Text), 0)
                    If _FourCMaintain Then
                        .Cells("CUT").Value = ObjDiaDetails.CmbCut.Text
                        .Cells("COLOR").Value = ObjDiaDetails.CmbColor.Text
                        .Cells("CLARITY").Value = ObjDiaDetails.CmbClarity.Text
                        .Cells("SHAPE").Value = ObjDiaDetails.cmbShape.Text
                        .Cells("SETTYPE").Value = ObjDiaDetails.cmbSetType.Text
                        .Cells("HEIGHT").Value = Val(ObjDiaDetails.txtHeight_WET.Text)
                        .Cells("WIDTH").Value = Val(ObjDiaDetails.txtWidth_WET.Text)
                    End If
                    dtStoneDetails.AcceptChanges()
                    GoTo AFTERINSERT
                End With
                'End If
            End If
            ''Insertion
            Dim ro As DataRow = Nothing
            ro = dtStoneDetails.NewRow
            ro("PACKETNO") = txtstPackettno.Text
            ro("ITEM") = txtStItem.Text
            ro("SUBITEM") = txtStSubItem.Text
            ro("PCS") = IIf(Val(txtStPcs_Num.Text) <> 0, Val(txtStPcs_Num.Text), DBNull.Value)
            ro("UNIT") = cmbStUnit.Text
            ro("CALC") = cmbStCalc.Text
            ro("WEIGHT") = IIf(Val(txtStWeight.Text) <> 0, Format(Val(txtStWeight.Text), FormatNumberStyle(DiaRnd)), DBNull.Value)
            ro("RATE") = IIf(Val(txtStRate_Amt.Text) <> 0, Format(Val(txtStRate_Amt.Text), "0.00"), DBNull.Value)
            ro("AMOUNT") = IIf(Val(txtStAmount_Amt.Text) <> 0, Format(Val(txtStAmount_Amt.Text), "0.00"), DBNull.Value)
            ro("METALID") = txtStMetalCode.Text
            ro("USRATE") = IIf(NEEDUS = True And Studded_Loose <> "L", Val(ObjRsUs.txtUSDollar_Amt.Text), 0)
            ro("INDRS") = IIf(NEEDUS = True And Studded_Loose <> "L", Val(ObjRsUs.txtIndRs_Amt.Text), 0)
            If _FourCMaintain Then
                ro("CUT") = ObjDiaDetails.CmbCut.Text
                ro("COLOR") = ObjDiaDetails.CmbColor.Text
                ro("CLARITY") = ObjDiaDetails.CmbClarity.Text
                ro("SHAPE") = ObjDiaDetails.cmbShape.Text
                ro("SETTYPE") = ObjDiaDetails.cmbSetType.Text
                ro("HEIGHT") = Val(ObjDiaDetails.txtHeight_WET.Text)
                ro("WIDTH") = Val(ObjDiaDetails.txtWidth_WET.Text)
            End If
            dtStoneDetails.Rows.Add(ro)
            dtStoneDetails.AcceptChanges()
            gridStone.CurrentCell = gridStone.Rows(gridStone.RowCount - 1).Cells(1)
AFTERINSERT:
            tagWebtagDiff = True
            CalcLessWt()
            CalcFinalTotal()

            ''CLEAR
            'cmbStItem_Man.Text = ""
            'cmbStSubItem_Man.Text = ""
            txtStSubItem.Clear()
            txtStPcs_Num.Clear()
            txtStWeight.Clear()
            txtStRate_Amt.Clear()
            txtStAmount_Amt.Clear()
            txtStMetalCode.Clear()
            txtstPackettno.Clear()
            txtStRowIndex.Clear()
            ObjRsUs.txtIndRs_Amt.Clear()
            ObjRsUs.txtUSDollar_Amt.Clear()
            If PacketNoEnable <> "N" Then
                txtstPackettno.Focus()
            Else
                txtStItem.Focus()
            End If

        End If
    End Sub

    Private Sub txtStWeight_Wet_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtStWeight.KeyDown
        If e.KeyCode = Keys.Down Then
            gridStone.Select()
        End If
    End Sub

    Private Sub txtStWeight_Wet_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStWeight.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim stWeight As Double = IIf(cmbStUnit.Text = "C", Val(txtStWeight.Text) / 5, Val(txtStWeight.Text))
            For cnt As Integer = 0 To gridStone.RowCount - 1
                If txtStRowIndex.Text <> "" Then If Val(txtStRowIndex.Text) = cnt Then Continue For
                With gridStone.Rows(cnt)
                    If .Cells("UNIT").Value.ToString = "C" Then
                        stWeight += Val(.Cells("WEIGHT").Value.ToString) / 5
                    ElseIf .Cells("UNIT").Value.ToString = "G" Then
                        stWeight += Val(.Cells("WEIGHT").Value.ToString)
                    End If
                End With
            Next
            If stWeight > Val(txtGrossWt_Wet.Text) Then
                MsgBox(Me.GetNextControl(txtStWeight, False).Text + E0015 + Me.GetNextControl(txtGrossWt_Wet, False).Text, MsgBoxStyle.Exclamation)
                txtStWeight.Focus()
                Exit Sub
            End If
        Else
            WeightValidation(txtStWeight, e, DiaRnd)
        End If
    End Sub

    Private Sub txtStWeight_Wet_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStWeight.LostFocus
        cmbStCalc.Text = IIf(Val(txtStWeight.Text) > 0, "W", "P")
        txtStWeight.Text = IIf(Val(txtStWeight.Text) <> 0, Format(Val(txtStWeight.Text), FormatNumberStyle(DiaRnd)), txtStWeight.Text)
    End Sub


    Private Sub txtStWeight_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStWeight.TextChanged
        Dim cent As Double = 0
        strSql = " SELECT ISNULL(CALTYPE,'') CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') ='" & txtStItem.Text & "'"
        Dim mCaltype As String = objGPack.GetSqlValue(strSql, "CALTYPE", "")
        strSql = " SELECT ISNULL(CALTYPE,'') CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE ISNULL(SUBITEMNAME,'') ='" & txtStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')"
        mCaltype = objGPack.GetSqlValue(strSql, "CALTYPE", "")
        If mCaltype = "D" Then
            cent = Val(txtStWeight.Text)
        Else
            cent = (Val(txtStWeight.Text) / IIf(Val(txtStPcs_Num.Text) = 0, 1, Val(txtStPcs_Num.Text)))
        End If
        ' If cmbStUnit.Text = "C" Then cent = (Val(txtStWeight.Text) / IIf(Val(txtStPcs_Num.Text) = 0, 1, Val(txtStPcs_Num.Text))) Else cent = Val(txtStWeight.Text) / IIf(Val(txtStPcs_Num.Text) = 0, 1, Val(txtStPcs_Num.Text))

        cent *= 100
        strSql = " DECLARE @CENT FLOAT"
        strSql += vbCrLf + " SET @CENT = " & cent & ""
        strSql += vbCrLf + " SELECT MAXRATE FROM " & cnAdminDb & "..CENTRATE WHERE"
        strSql += vbCrLf + " @CENT BETWEEN FROMCENT AND TOCENT "
        strSql += vbCrLf + " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')"
        strSql += vbCrLf + " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')),0)"
        If CENTRATE_DESIGNER Then strSql += " AND ISNULL(DESIGNERID,0) =ISNULL((SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME='" & cmbDesigner_MAN.Text & "'),0)"
        'strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ISNULL((SELECT ACCODE FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesigner_MAN.Text & "'),'')"
        If cmbCostCentre_Man.Text.Trim <> "" Then strSql += " AND COSTID IN(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre_Man.Text & "')"
        If _FourCMaintain Then

            Dim ColorId As Integer = 0
            Dim CutId As Integer = 0
            Dim ClarityId As Integer = 0
            Dim Shapeid As Integer = 0
            ColorId = objGPack.GetSqlValue("SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME = '" & ObjDiaDetails.CmbColor.Text & "'", "COLORID", 0)
            'CutId = objGPack.GetSqlValue("SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & ObjDiaDetails.CmbCut.Text & "'", "CUTID", 0)
            ClarityId = objGPack.GetSqlValue("SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & ObjDiaDetails.CmbClarity.Text & "'", "CLARITYID", 0)
            Shapeid = objGPack.GetSqlValue("SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & ObjDiaDetails.cmbShape.Text & "'", "SHAPEID", 0)
            strSql += vbCrLf + " AND COLORID=" & ColorId
            strSql += vbCrLf + " AND SHAPEID=" & Shapeid
            'strSql += vbCrLf + " AND CUTID=" & CutId
            strSql += vbCrLf + " AND CLARITYID=" & ClarityId
        End If
        Dim rate As Double = Val(objGPack.GetSqlValue(strSql, "MAXRATE", "", tran))
        If rate <> 0 Then
            txtStRate_Amt.Text = IIf(rate <> 0, Format(rate, "0.00"), "")
        Else
            Dim XpurRate As Double = Val(objGPack.GetSqlValue(Replace(strSql, "MAXRATE", "PURRATE"), "PURRATE", "", tran).ToString)
            Dim SaleRateper As Double = Val(objGPack.GetSqlValue(Replace(strSql, "MAXRATE", "SALESPER"), "SALESPER", "", tran).ToString)
            If SaleRateper <> 0 And XpurRate <> 0 Then rate = XpurRate + (XpurRate * (SaleRateper / 100))
            txtStRate_Amt.Text = IIf(rate <> 0, Format(rate, "0.00"), "")
            'txtStRate_Amt.Text = "0.00"
        End If
        CalcStoneAmount()
    End Sub

    Private Sub cmbStCalc_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStCalc.Leave

    End Sub

    Private Sub cmbStCalc_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStCalc.SelectedIndexChanged
        CalcStoneAmount()
    End Sub

    Private Sub txtStPcs_Num_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtStPcs_Num.KeyDown
        If e.KeyCode = Keys.Down Then
            gridStone.Select()
        End If
    End Sub

    Private Sub txtStPcs_Num_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStPcs_Num.Leave
        Dim view4c As String = ""
        Dim maintain4c As Boolean
        strSql = "SELECT ISNULL(MAINTAIN4C,'N') FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "' AND METALID='D'"
        If objGPack.GetSqlValue(strSql).ToString.ToUpper = "N" Then maintain4c = False Else maintain4c = True
        strSql = "SELECT ISNULL(MAINTAIN4C,'N') FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtStSubItem.Text & "' AND ITEMID=(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & txtStItem.Text & "')"
        If objGPack.GetSqlValue(strSql).ToString.ToUpper = "N" Then maintain4c = False Else maintain4c = True
        If _FourCMaintain And Not tagEdit And maintain4c Then
            ObjDiaDetails = New frmDiamondDetails
            If txtStItem.Text <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & txtStItem.Text & "'", , , )
            If txtStSubItem.Text <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & txtStSubItem.Text & "' AND ITEMID=(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & txtStItem.Text & "')", , , )
            If Not view4c.Contains("CU") Then ObjDiaDetails.CmbCut.Enabled = False
            If Not view4c.Contains("CO") Then ObjDiaDetails.CmbColor.Enabled = False
            If Not view4c.Contains("CL") Then ObjDiaDetails.CmbClarity.Enabled = False
            If Not view4c.Contains("SH") Then ObjDiaDetails.cmbShape.Enabled = False
            If Not view4c.Contains("SE") Then ObjDiaDetails.cmbSetType.Enabled = False
            If Not view4c.Contains("HE") Then ObjDiaDetails.txtHeight_WET.Enabled = False
            If Not view4c.Contains("WI") Then ObjDiaDetails.txtWidth_WET.Enabled = False

            ObjDiaDetails.CmbCut.Focus()
            ObjDiaDetails.ShowDialog()
        ElseIf _FourCMaintain And tagEdit And maintain4c Then
            ObjDiaDetails = New frmDiamondDetails
            ObjDiaDetails.CmbCut.Text = Cut
            ObjDiaDetails.CmbColor.Text = Color
            ObjDiaDetails.CmbClarity.Text = Clarity
            ObjDiaDetails.cmbShape.Text = Shape
            ObjDiaDetails.cmbSetType.Text = SetType
            ObjDiaDetails.txtWidth_WET.Text = Width
            ObjDiaDetails.txtHeight_WET.Text = Height
            If txtStItem.Text <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & txtStItem.Text & "'", , , )
            If txtStSubItem.Text <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & txtStSubItem.Text & "' AND ITEMID=(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & txtStItem.Text & "')", , , )
            If Not view4c.Contains("CU") Then ObjDiaDetails.CmbCut.Enabled = False
            If Not view4c.Contains("CO") Then ObjDiaDetails.CmbColor.Enabled = False
            If Not view4c.Contains("CL") Then ObjDiaDetails.CmbClarity.Enabled = False
            If Not view4c.Contains("SH") Then ObjDiaDetails.cmbShape.Enabled = False
            If Not view4c.Contains("SE") Then ObjDiaDetails.cmbSetType.Enabled = False
            If Not view4c.Contains("HE") Then ObjDiaDetails.txtHeight_WET.Enabled = False
            If Not view4c.Contains("WI") Then ObjDiaDetails.txtWidth_WET.Enabled = False
            ObjDiaDetails.CmbCut.Focus()
            ObjDiaDetails.ShowDialog()
        End If
    End Sub

    Private Sub txtStPcs_Num_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStPcs_Num.TextChanged
        CalcStoneAmount()
    End Sub

    Private Sub gridStone_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridStone.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            gridStone.CurrentCell = gridStone.Rows(gridStone.CurrentRow.Index).Cells(1)
        End If
    End Sub

    Private Sub gridStone_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridStone.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            With gridStone.Rows(gridStone.CurrentRow.Index)
                txtstPackettno.Text = .Cells("PACKETNO").FormattedValue
                txtStItem.Text = .Cells("ITEM").FormattedValue
                txtStSubItem.Text = .Cells("SUBITEM").FormattedValue
                txtStPcs_Num.Text = .Cells("PCS").FormattedValue
                txtStWeight.Text = .Cells("WEIGHT").FormattedValue
                cmbStUnit.Text = .Cells("UNIT").FormattedValue
                cmbStCalc.Text = .Cells("CALC").FormattedValue
                txtStRate_Amt.Text = .Cells("RATE").FormattedValue
                txtStAmount_Amt.Text = .Cells("AMOUNT").FormattedValue
                ObjRsUs.txtUSDollar_Amt.Text = .Cells("USRATE").FormattedValue
                ObjRsUs.txtIndRs_Amt.Text = .Cells("INDRS").FormattedValue
                If _FourCMaintain Then
                    Cut = IIf(IsDBNull(.Cells("CUT").Value), "", .Cells("CUT").Value)
                    Color = IIf(IsDBNull(.Cells("COLOR").Value), "", .Cells("COLOR").Value)
                    Clarity = IIf(IsDBNull(.Cells("CLARITY").Value), "", .Cells("CLARITY").Value)
                    Shape = IIf(IsDBNull(.Cells("SHAPE").Value), "", .Cells("SHAPE").Value)
                    SetType = IIf(IsDBNull(.Cells("SETTYPE").Value), "", .Cells("SETTYPE").Value)
                    Height = IIf(IsDBNull(.Cells("HEIGHT").Value), 0, .Cells("HEIGHT").Value)
                    Width = IIf(IsDBNull(.Cells("WIDTH").Value), 0, .Cells("WIDTH").Value)
                    ObjDiaDetails.CmbCut.Text = Cut
                    ObjDiaDetails.CmbColor.Text = Color
                    ObjDiaDetails.CmbClarity.Text = Clarity
                    ObjDiaDetails.cmbShape.Text = Shape
                    ObjDiaDetails.cmbSetType.Text = SetType
                    ObjDiaDetails.txtWidth_WET.Text = Width
                    ObjDiaDetails.txtHeight_WET.Text = Height
                End If
                txtStRowIndex.Text = gridStone.CurrentRow.Index
                If PacketNoEnable <> "N" Then
                    txtstPackettno.Focus()
                    txtstPackettno.SelectAll()
                Else
                    txtStItem.Focus()
                    txtStItem.SelectAll()
                End If
            End With
        End If
    End Sub

    Private Sub gridStone_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridStone.UserDeletedRow
        dtStoneDetails.AcceptChanges()

        Dim RowCheck() As DataRow = Nothing
        With ObjPurDetail
StDel:
            For Each ro As DataRow In .dtGridStone.Rows
                RowCheck = dtStoneDetails.Select("KEYNO = " & Val(ro.Item("KEYNO").ToString) & "")
                If RowCheck.Length = 0 Then
                    ro.Delete()
                    .dtGridStone.AcceptChanges()
                    GoTo StDel
                End If
            Next
            .dtGridStone.AcceptChanges()
            .CalcPurchaseGrossValue()
            .CalcPurchaseValue()
        End With

        CalcLessWt()
        CalcFinalTotal()
        objGPack.TextClear(grpStoneDetails)

        If Not gridStone.RowCount > 0 Then
            txtStItem.Focus()
        End If
    End Sub

#End Region


#Region "Miscellaneous Procedures"
    Private Sub CalcMiscTotalAmount()
        Dim miscTot As Double = Nothing
        For cnt As Integer = 0 To gridMisc.Rows.Count - 1
            miscTot += Val(gridMisc.Rows(cnt).Cells("AMOUNT").Value.ToString)
        Next
        gridMiscFooter.Rows(0).Cells("AMOUNT").Value = IIf(miscTot <> 0, Format(miscTot, "0.00"), DBNull.Value)
        txtMiscAmt.Text = IIf(miscTot <> 0, Format(miscTot, "0.00"), "")
    End Sub

    Private Sub txtMiscAmount_Amt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMiscAmount_Amt.KeyDown
        If e.KeyCode = Keys.Down Then
            gridMisc.Select()
        End If
    End Sub

    Private Sub txtMiscAmount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMiscAmount_Amt.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtMiscMisc.Text = "" Then
                MsgBox(Me.GetNextControl(txtMiscMisc, False).Text + E0001, MsgBoxStyle.Information)
                txtMiscMisc.Select()
                Exit Sub
            End If
            If objGPack.DupCheck("SELECT MISCNAME FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCNAME = '" & txtMiscMisc.Text & "' AND  ACTIVE = 'Y'") = False Then
                MsgBox("Invalid Misc", MsgBoxStyle.Information)
                txtMiscMisc.Focus()
                Exit Sub
            End If
            If Not Val(txtMiscAmount_Amt.Text) > 0 Then
                MsgBox(Me.GetNextControl(txtMiscAmount_Amt, False).Text + E0001, MsgBoxStyle.Information)
                txtMiscAmount_Amt.Select()
                Exit Sub
            End If
            If txtMiscRowIndex.Text <> "" Then
                With gridMisc.Rows(Val(txtMiscRowIndex.Text))
                    .Cells("MISC").Value = txtMiscMisc.Text
                    .Cells("AMOUNT").Value = IIf(Val(txtMiscAmount_Amt.Text) <> 0, Val(txtMiscAmount_Amt.Text), DBNull.Value)
                    dtMiscDetails.AcceptChanges()
                    GoTo AFTERINSERT
                End With
            End If
            Dim ro As DataRow = Nothing
            ro = dtMiscDetails.NewRow
            ro("MISC") = txtMiscMisc.Text
            ro("AMOUNT") = IIf(Val(txtMiscAmount_Amt.Text) <> 0, Val(txtMiscAmount_Amt.Text), DBNull.Value)
            dtMiscDetails.Rows.Add(ro)
            dtMiscDetails.AcceptChanges()
AFTERINSERT:
            CalcFinalTotal()
            gridMisc.CurrentCell = gridMisc.Rows(gridMisc.RowCount - 1).Cells(0)

            txtMiscMisc.Clear()
            txtMiscAmount_Amt.Clear()
            txtMiscMisc.Select()
            txtMiscRowIndex.Clear()
        End If
    End Sub

    Private Sub gridMisc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridMisc.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub gridMisc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridMisc.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            gridMisc.CurrentCell = gridMisc.Rows(gridMisc.CurrentRow.Index).Cells(0)
            With gridMisc.Rows(gridMisc.CurrentRow.Index)
                txtMiscMisc.Text = .Cells("MISC").FormattedValue
                txtMiscAmount_Amt.Text = .Cells("AMOUNT").FormattedValue
                txtMiscRowIndex.Text = gridMisc.CurrentRow.Index
                txtMiscMisc.Focus()
                txtMiscMisc.SelectAll()
            End With
        End If
    End Sub

    Private Sub gridMisc_UserDeletedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles gridMisc.UserDeletedRow
        dtMiscDetails.AcceptChanges()
        CalcFinalTotal()
        If Not gridMisc.RowCount > 0 Then
            txtMiscMisc.Select()
        End If

        Dim RowCheck() As DataRow = Nothing
        With ObjPurDetail
StDel:
            For Each ro As DataRow In .dtGridMisc.Rows
                RowCheck = dtMiscDetails.Select("KEYNO = " & Val(ro.Item("KEYNO").ToString) & "")
                If RowCheck.Length = 0 Then
                    ro.Delete()
                    .dtGridMisc.AcceptChanges()
                    GoTo StDel
                End If
            Next
            .dtGridMisc.AcceptChanges()
            .CalcPurchaseGrossValue()
            .CalcPurchaseValue()
        End With


    End Sub
#End Region

    Private Sub txtPurPurchaseVal_Amt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        txtPurchaseValue_Amt.Text = ObjPurDetail.txtPurPurchaseVal_Amt.Text
    End Sub

    Private Sub txtLessWt_Wet_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLessWt_Wet.GotFocus
        If Val(txtGrossWt_Wet.Text) = 0 Then
            SendKeys.Send("{TAB}")
            Exit Sub
        End If
        If LockLessWt = False Then Exit Sub
        If LockLessWt = True Then
            SendKeys.Send("{TAB}")
            Exit Sub
        End If
        If studdedStone = "Y" And Val(txtGrossWt_Wet.Text) <> Val(txtNetWt_Wet.Text) Then
            SendKeys.Send("{TAB}")
        ElseIf studdedStone = "N" And grossnetdiff = "N" Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtLessWt_Wet_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLessWt_Wet.LostFocus
        CalcFinalTotal()
    End Sub

    Private Sub ObjMinValues_txtMinWastage_Per_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Not Val(ObjMinValue.txtMinWastage_Per.Text + e.KeyChar) <= Val(txtMaxWastage_Per.Text) Then
            e.Handled = True
        End If
    End Sub


    Private Sub ObjMinValues_txtMinWastage_Per_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim wt As Double = Nothing
        If cmbCalcMode.SelectedIndex = 0 Then ''GRS WT
            wt = (Val(txtGrossWt_Wet.Text) * Val(ObjMinValue.txtMinWastage_Per.Text) / 100)
        Else ''NET WT
            wt = (Val(txtNetWt_Wet.Text) * Val(ObjMinValue.txtMinWastage_Per.Text) / 100)
        End If
        wt = Math.Round(wt, WastageRound)
        ObjMinValue.txtMinWastage_Wet.Text = IIf(wt <> 0, Format(wt, "0.000"), "")
    End Sub

    Private Sub ObjMinValues_txtMinMcPerGram_Amt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Not Val(txtMaxMcPerGrm_Amt.Text) >= Val(ObjMinValue.txtMinMcPerGram_Amt.Text + e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub ObjMinValues_txtMinMcPerGram_Amt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim mc As Double = Nothing
        Dim wast As Double = IIf(McWithWastage, Val(ObjMinValue.txtMinWastage_Wet.Text), 0)
        If cmbCalcMode.SelectedIndex = 0 Then ''GRS WT
            mc = (Val(txtGrossWt_Wet.Text) + wast) * Val(ObjMinValue.txtMinMcPerGram_Amt.Text)
        Else ''NET WT
            mc = (Val(txtNetWt_Wet.Text) + wast) * Val(ObjMinValue.txtMinMcPerGram_Amt.Text)
        End If
        ObjMinValue.txtMinMkCharge_Amt.Text = IIf(mc <> 0, Format(mc, "0.00"), "")
    End Sub

    Private Sub txtLotNo_Num_Man_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLotNo_Num.LostFocus
        isStyleCode = False
        ''txtMetalRate_Amt_Man.Text = FORMAT(GetMetalRate(),"0.00")
    End Sub

    Private Sub txtStyleCode_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStyleCode.GotFocus
        If Not isStyleCode Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub ListSearch(ByVal cmb As ComboBox)
        If Not cmb.Items.Count > 0 Then Exit Sub
        cmbSearch.Items.Clear()
        For Each obj As Object In cmb.Items
            cmbSearch.Items.Add(obj)
        Next
        pnlSearch.Visible = True
    End Sub

    Private Sub HideSearch()
        pnlSearch.Visible = False
        cmbSearch.Items.Clear()
    End Sub

    Private Sub cmbItemType_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItemType_MAN.GotFocus
        ListSearch(CType(sender, ComboBox))
    End Sub

    Private Sub cmbItemType_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItemType_MAN.LostFocus
        HideSearch()
        txtMetalRate_Amt.Text = Format(GetMetalRate(), "0.00")
        CalcMaxMinValues()
    End Sub

    Private Sub cmbItemType_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItemType_MAN.TextChanged
        cmbSearch.Text = CType(sender, ComboBox).Text
    End Sub

    Private Sub cmbDesigner_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbDesigner_MAN.GotFocus
        ListSearch(CType(sender, ComboBox))
    End Sub

    Private Sub cmbDesigner_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbDesigner_MAN.LostFocus
        HideSearch()
        CalcMaxMinValues()
    End Sub

    Private Sub cmbDesigner_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbDesigner_MAN.TextChanged
        cmbSearch.Text = CType(sender, ComboBox).Text
    End Sub

    Private Sub cmbCounter_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCounter_MAN.GotFocus
        If Not cmbCounter_MAN.Items.Count > 0 Then SendKeys.Send("{TAB}")
        ListSearch(CType(sender, ComboBox))
    End Sub

    Private Sub cmbCounter_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCounter_MAN.LostFocus
        HideSearch()
    End Sub

    Private Sub cmbCounter_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCounter_MAN.TextChanged
        cmbSearch.Text = CType(sender, ComboBox).Text
    End Sub

    Private Sub cmbSubItem_Man_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubItem_Man.GotFocus
        'strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')"
        'objGPack.FillCombo(strSql, cmbSubItem_Man)
        ListSearch(CType(sender, ComboBox))
    End Sub

    Private Sub cmbSubItem_Man_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubItem_Man.LostFocus
        If cmbSubItem_Man.Text <> "" Then
            Dim subdr As DataRow = GetSqlRow("SELECT ISNULL(FIXEDVA,'N'),CALTYPE,NOOFPIECE,ISNULL(TABLECODE,''),ISNULL(MCCALC,''),ISNULL(VALUECALC,'') FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "' and ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')", cn, tran)
            'pFixedVa = subdr.Item(0).ToString
            'calType = subdr.Item(1).ToString
            If Not subdr Is Nothing Then
                If Val("" & subdr.Item(2).ToString) <> 0 Then noOfPiece = Val("" & subdr.Item(2).ToString)
                If (subdr.Item(3).ToString) <> "''" Then cmbTableCode.Text = subdr.Item(3).ToString
                _MCCALCON_ITEM_GRS = IIf(subdr.Item(4).ToString = "G", True, False)
                _VALUECALCON_ITEM_GRS = IIf(subdr.Item(5).ToString = "G", True, False)
            End If
            'CALNO 386
            'If (subdr.Item(4).ToString) <> "''" Then cmbCalcMode.Text = 
            'CALNO 386
        End If
        HideSearch()
        CalcLessWt()
        CalcNetWt()
        CalcMaxMinValues()
        Calcsubitems()
        CalcSaleValue()
    End Sub

    Private Sub cmbSubItem_Man_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubItem_Man.TextChanged
        cmbSearch.Text = CType(sender, ComboBox).Text
    End Sub

    Private Sub cmbItemSize_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItemSize.GotFocus
        ListSearch(CType(sender, ComboBox))
    End Sub

    Private Sub cmbItemSize_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItemSize.LostFocus
        HideSearch()
    End Sub

    Private Sub cmbItemSize_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItemSize.TextChanged
        cmbSearch.Text = CType(sender, ComboBox).Text
    End Sub

    Private Sub ObjMinValue_txtMinWastage_Wet_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        If Val(ObjMinValue.txtMinWastage_Per.Text) > 0 Then ObjMinValue.txtMinWastage_Wet.ReadOnly = True Else ObjMinValue.txtMinWastage_Wet.ReadOnly = False
    End Sub

    Private Sub ObjMinValue_txtMinWastage_Wet_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Not Val(txtMaxWastage_Wet.Text) >= Val(ObjMinValue.txtMinWastage_Wet.Text + e.KeyChar) Then
            e.Handled = True
            ' Else
            MsgBox("Check Maximum value")
        End If
    End Sub

    Private Sub ObjMinValue_txtMinMkCharge_Amt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        If Val(ObjMinValue.txtMinMcPerGram_Amt.Text) > 0 Then ObjMinValue.txtMinMcPerGram_Amt.ReadOnly = True Else ObjMinValue.txtMinMcPerGram_Amt.ReadOnly = False
    End Sub

    Private Sub ObjMinValue_txtMinMkCharge_Amt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Not Val(txtMaxMkCharge_Amt.Text) >= Val(ObjMinValue.txtMinMkCharge_Amt.Text + e.KeyChar) Then
            e.Handled = True
            MsgBox("Check Maximum value")
        End If
    End Sub

    Private Sub txtMetalRate_Amt_Man_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMetalRate_Amt.GotFocus
        txtMetalRate_Amt.Text = Format(GetMetalRate(), "0.00")
    End Sub

    Private Sub txtStItem_MAN_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStItem.GotFocus
        Main.ShowHelpText("Press Insert Key to Help")
    End Sub

    Private Sub txtStItem_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStItem.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtStItem.Text = "" Then
                LoadStoneItemName()
            ElseIf txtStItem.Text <> "" And objGPack.DupCheck("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "'") = False Then
                LoadStoneItemName()
            Else
                LoadStoneitemDetails()
            End If
        End If
    End Sub

    Private Sub txtStItem_Man_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStItem.LostFocus
        Main.HideHelpText()
    End Sub

    Private Sub txtStSubItem_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStSubItem.GotFocus
        'If objGPack.DupCheck("SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem_MAN.Text & "')") = False Then
        SendKeys.Send("{TAB}")
        'End If
    End Sub

    Private Sub txtStItem_MAN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtStItem.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadStoneItemName()
        ElseIf e.KeyCode = Keys.Down Then
            If gridStone.RowCount > 0 Then
                gridStone.Focus()
            End If
        End If
    End Sub

    Private Sub LoadStoneItemName()
        strSql = " SELECT"
        strSql += " ITEMID AS ITEMID,ITEMNAME AS ITEMNAME"
        strSql += " FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ISNULL(STUDDED,'') = 'S' AND ACTIVE = 'Y'"
        Dim itemName As String = BrighttechPack.SearchDialog.Show("Find ItemName", strSql, cn, 1, 1, , txtStItem.Text)
        If itemName <> "" Then
            txtStItem.Text = itemName
            LoadStoneitemDetails()
        Else
            txtStItem.Focus()
            txtStItem.SelectAll()
        End If
    End Sub

    Private Sub LoadStoneitemDetails()
        'txtStSubItem.Clear()
        'txtStPcs_Num.Clear()
        'txtStWeight.Clear()
        'txtStRate_Amt.Clear()
        'txtStAmount_Amt.Clear()
        If objGPack.GetSqlValue("SELECT SUBITEM FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "'") = "Y" Then
            Dim DefItem As String = txtStSubItem.Text
            Dim itemId As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "'"))
            If Not _SubItemOrderByName Then
                DefItem = objGPack.GetSqlValue("SELECT DISPLAYORDER FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtStSubItem.Text & "' AND ITEMID = " & itemId & "")
            End If
            strSql = GetSubItemQry(New String() {"SUBITEMID ID", "DISPLAYORDER DISP", "SUBITEMNAME SUBITEM"}, itemId)
            txtStSubItem.Text = BrighttechPack.SearchDialog.Show("Search SubItem", strSql, cn, IIf(_SubItemOrderByName, 2, 1), 2, , DefItem, , False, True)
        Else
            txtStSubItem.Clear()
        End If

        If txtStSubItem.Text <> "" Then
            strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & txtStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')"
        Else
            strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "'"
        End If
        Dim calType As String = objGPack.GetSqlValue(strSql, , , tran)
        cmbStCalc.Text = IIf(calType = "R", "P", "W")

        If txtStSubItem.Text <> "" Then
            strSql = " SELECT STONEUNIT FROM " & cnAdminDb & "..SUBITEMMAST  WHERE SUBITEMNAME = '" & txtStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "')"
        Else
            strSql = " SELECT STONEUNIT FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "'"
        End If
        cmbStUnit.Text = objGPack.GetSqlValue(strSql, , , tran)

        strSql = " SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtStItem.Text & "'"
        txtStMetalCode.Text = objGPack.GetSqlValue(strSql, "DIASTONE", , tran)
        'If txtStMetalCode.Text = "S" Then cmbStUnit.Text = "G" Else cmbStUnit.Text = "C"
        Me.SelectNextControl(txtStItem, True, True, True, True)
    End Sub


    Private Function GetControlLocation(ByVal ctrl As Control, ByRef pt As Point) As Point
        If TypeOf ctrl Is Form Then
            Return pt
        Else
            pt += ctrl.Location
            Return GetControlLocation(ctrl.Parent, pt)
        End If
        Return pt
    End Function


    Private Sub txtMiscMisc_Man_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMiscMisc.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtMiscMisc.Text = "" Then Exit Sub
            If objGPack.DupCheck("SELECT MISCNAME FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCNAME = '" & txtMiscMisc.Text & "' AND  ACTIVE = 'Y'") = False Then
                MsgBox("Invalid Misc", MsgBoxStyle.Information)
                txtMiscMisc.Focus()
            End If
        End If
    End Sub

    Private Sub txtMiscMisc_Man_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMiscMisc.LostFocus
        If txtMiscMisc.Text = "" Then Exit Sub
        strSql = " SELECT DEFAULTVALUE FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCNAME = '" & txtMiscMisc.Text & "'"
        Dim amt As Double = Val(objGPack.GetSqlValue(strSql, "DEFAULTVALUE", , tran))
        txtMiscAmount_Amt.Text = IIf(amt <> 0, Format(amt, "0.00"), "")
    End Sub

    Private Sub txtMMCategory_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMMCategory.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtMMCategory.Text = "" Then Exit Sub
            If objGPack.DupCheck("SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & txtMMCategory.Text & "'") = False Then
                MsgBox("Invalid Category", MsgBoxStyle.Information)
                txtMMCategory.Focus()
            End If
        End If
    End Sub

    Private Sub txtMetalRate_Amt_Man_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMetalRate_Amt.TextChanged
        CalcFinalTotal()
    End Sub

    Private Sub txtMMAmount_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMMAmount_AMT.KeyDown
        If e.KeyCode = Keys.Down Then
            gridMultimetal.Select()
        End If
    End Sub

    Private Sub txtMMAmount_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMMAmount_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtMMCategory.Text = "" Then
                MsgBox("Category Should Not Empty", MsgBoxStyle.Information)
                txtMMCategory.Focus()
                Exit Sub
            ElseIf objGPack.DupCheck("SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & txtMMCategory.Text & "'") = False Then
                MsgBox("Invalid Category", MsgBoxStyle.Information)
                txtMMCategory.Focus()
                Exit Sub
            ElseIf (Not Val(txtMMWeight_Wet.Text) > 0) And (Not Val(txtMMAmount_AMT.Text) > 0) Then
                MsgBox("Weight,Rate,Amount Should not Empty", MsgBoxStyle.Information)
                txtMMWeight_Wet.Focus()
                Exit Sub
            End If
            If txtMMRowIndex.Text <> "" Then
                With dtMultiMetalDetails.Rows(Val(txtMMRowIndex.Text))
                    .Item("CATEGORY") = txtMMCategory.Text
                    .Item("WEIGHT") = IIf(Val(txtMMWeight_Wet.Text) <> 0, Val(txtMMWeight_Wet.Text), DBNull.Value)
                    .Item("WASTAGEPER") = IIf(Val(txtMMWastagePer_PER.Text) <> 0, Val(txtMMWastagePer_PER.Text), DBNull.Value)
                    .Item("WASTAGE") = IIf(Val(txtMMWastage_WET.Text) <> 0, Val(txtMMWastage_WET.Text), DBNull.Value)
                    .Item("MCPERGRM") = IIf(Val(txtMMMcPerGRm_AMT.Text) <> 0, Val(txtMMMcPerGRm_AMT.Text), DBNull.Value)
                    .Item("MC") = IIf(Val(txtMMMc_AMT.Text) <> 0, Val(txtMMMc_AMT.Text), DBNull.Value)
                    .Item("AMOUNT") = IIf(Val(txtMMAmount_AMT.Text) <> 0, Val(txtMMAmount_AMT.Text), DBNull.Value)
                    .Item("RATE") = IIf(Val(txtMMRate.Text) <> 0, Val(txtMMRate.Text), DBNull.Value)
                    dtMultiMetalDetails.AcceptChanges()
                    GoTo AFTERINSERT
                End With
            End If
            Dim ro As DataRow = Nothing
            ro = dtMultiMetalDetails.NewRow
            ro("CATEGORY") = txtMMCategory.Text
            ro("WEIGHT") = IIf(Val(txtMMWeight_Wet.Text) <> 0, Val(txtMMWeight_Wet.Text), DBNull.Value)
            ro("WASTAGEPER") = IIf(Val(txtMMWastagePer_PER.Text) <> 0, Val(txtMMWastagePer_PER.Text), DBNull.Value)
            ro("WASTAGE") = IIf(Val(txtMMWastage_WET.Text) <> 0, Val(txtMMWastage_WET.Text), DBNull.Value)
            ro("MCPERGRM") = IIf(Val(txtMMMcPerGRm_AMT.Text) <> 0, Val(txtMMMcPerGRm_AMT.Text), DBNull.Value)
            ro("MC") = IIf(Val(txtMMMc_AMT.Text) <> 0, Val(txtMMMc_AMT.Text), DBNull.Value)
            ro("AMOUNT") = IIf(Val(txtMMAmount_AMT.Text) <> 0, Val(txtMMAmount_AMT.Text), DBNull.Value)
            ro("RATE") = IIf(Val(txtMMRate.Text) <> 0, Val(txtMMRate.Text), DBNull.Value)
            dtMultiMetalDetails.Rows.Add(ro)
AFTERINSERT:
            CalcFinalTotal()

            txtMMWeight_Wet.Clear()
            txtMMWastagePer_PER.Clear()
            txtMMWastage_WET.Clear()
            txtMMMcPerGRm_AMT.Clear()
            txtMMMc_AMT.Clear()
            txtMMAmount_AMT.Clear()
            txtMMRowIndex.Clear()
            txtMMCategory.Select()
            txtMMRate.Clear()
        End If
    End Sub

    Private Sub txtMMAmount_AMT_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMMAmount_AMT.LostFocus
        txtMMCategory.Focus()
    End Sub

    Private Sub txtMMWastage_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMMWastage_WET.KeyPress
        If Val(txtMMWastagePer_PER.Text) > 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtMMWastagePer_PER_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMMWastagePer_PER.TextChanged
        Dim wt As Double = Val(txtMMWeight_Wet.Text) * (Val(txtMMWastagePer_PER.Text) / 100)
        wt = Math.Round(wt, WastageRound)
        txtMMWastage_WET.Text = IIf(wt <> 0, Format(wt, "0.000"), "")
    End Sub

    Private Sub txtMMWastagePer_PER_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMMWastagePer_PER.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridMultimetal.RowCount > 0 Then
                gridMultimetal.Focus()
            End If
        End If
    End Sub

    Private Sub txtMMWastage_WET_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMMWastage_WET.GotFocus
        If Val(txtMMWastagePer_PER.Text) > 0 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtMMWastage_WET_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMMWastage_WET.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridMultimetal.RowCount > 0 Then
                gridMultimetal.Focus()
            End If
        End If
    End Sub

    Private Sub txtMMMcPerGRm_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMMMcPerGRm_AMT.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridMultimetal.RowCount > 0 Then
                gridMultimetal.Focus()
            End If
        End If
    End Sub
    Private Sub txtMMMcPerGRm_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMMMcPerGRm_AMT.TextChanged
        Dim mc As Double = Val(txtMMWeight_Wet.Text) * Val(txtMMMcPerGRm_AMT.Text)
        txtMMMc_AMT.Text = IIf(mc <> 0, Format(mc, "0.00"), "")
    End Sub

    Private Sub txtMMMc_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMMMc_AMT.GotFocus
        If Val(txtMMMcPerGRm_AMT.Text) > 0 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtMMMc_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMMMc_AMT.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridMultimetal.RowCount > 0 Then
                gridMultimetal.Focus()
            End If
        End If
    End Sub
    Private Sub txtMMMc_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMMMc_AMT.KeyPress
        If Val(txtMMMcPerGRm_AMT.Text) > 0 Then
            e.Handled = True
        End If
    End Sub
    Private Sub txtMMCategory_MAN_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMMCategory.GotFocus
        If ALLOYGOLD_SALEMODE Then
            If Val(gridMultiMetalTotal.Rows(0).Cells("WEIGHT").Value.ToString) = 0 Then
                Dim basecatcode As String
                strSql = "SELECT I.CATCODE,C.CATNAME,NETWTPER FROM " & cnAdminDb & "..ITEMMAST I INNER JOIN " & cnAdminDb & "..CATEGORY C ON I.CATCODE = C.CATCODE WHERE I.ITEMNAME = '" & cmbItem_MAN.Text & "'"
                Dim alloydr As DataRow
                alloydr = GetSqlRow(strSql, cn)
                If Not alloydr Is Nothing Then
                    If Val(alloydr(2).ToString) <> 0 Then
                        txtMMCategory.Text = alloydr(1).ToString
                        txtMMWeight_Wet.Text = Val(txtGrossWt_Wet.Text) * ((100 - Val(alloydr(2).ToString)) / 100)

                    End If
                End If
            Else
                txtMMWeight_Wet.Text = Val(txtGrossWt_Wet.Text) - Val(gridMultiMetalTotal.Rows(0).Cells("WEIGHT").Value.ToString)
            End If
        End If
        Main.ShowHelpText("Press Insert Key to Help")
    End Sub

    Private Sub txtMMCategory_MAN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMMCategory.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadCatName()
        ElseIf e.KeyCode = Keys.Down Then
            If gridMultimetal.RowCount > 0 Then
                gridMultimetal.Focus()
            End If
        End If
    End Sub

    Private Sub txtMMCategory_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMMCategory.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtMMCategory.Text = "" Then
                LoadCatName()
            ElseIf txtMMCategory.Text <> "" And objGPack.DupCheck("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & txtMMCategory.Text & "'") = False Then
                LoadCatName()
            Else
                LoadCatDetails()
            End If
        End If
    End Sub

    Private Sub txtMMCategory_MAN_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMMCategory.LostFocus
        Main.HideHelpText()
    End Sub
    Private Sub LoadCatName()
        strSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
        strSql += " WHERE PURITYID IN (SELECT PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE METALTYPE = 'O')"
        strSql += " ORDER BY CATNAME"
        Dim catName As String = BrighttechPack.SearchDialog.Show("Find CatName", strSql, cn, , , , txtMMCategory.Text)
        If catName <> "" Then
            txtMMCategory.Text = catName
            LoadCatDetails()
        Else
            txtMMCategory.Select()
            txtMMCategory.SelectAll()
        End If
    End Sub
    Private Sub LoadCatDetails()
        If txtMMCategory.Text <> "" Then
            txtMMRate.Text = Val(GetRate(dtpRecieptDate.Value, objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & txtMMCategory.Text & "'")))
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtMMRate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMMRate.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridMultimetal.RowCount > 0 Then
                gridMultimetal.Focus()
            End If
        End If
    End Sub

    Private Sub txtMMWeight_Wet_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMMWeight_Wet.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridMultimetal.RowCount > 0 Then
                gridMultimetal.Focus()
            End If
        End If
    End Sub
    Private Sub txtMiscMisc_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMiscMisc.GotFocus
        Main.ShowHelpText("Press Insert Key to Help")
    End Sub

    Private Sub txtMiscMisc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMiscMisc.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadMiscName()
        ElseIf e.KeyCode = Keys.Down Then
            If gridMisc.RowCount > 0 Then gridMisc.Focus()
        End If
    End Sub

    Private Sub txtMiscMisc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMiscMisc.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtMiscMisc.Text = "" Then
                LoadMiscName()
            ElseIf txtMiscMisc.Text <> "" And objGPack.DupCheck("SELECT MISCNAME FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCNAME = '" & txtMiscMisc.Text & "'") = False Then
                LoadMiscName()
            Else
                LoadMiscDetails()
            End If
        End If
    End Sub

    Private Sub txtMiscMisc_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMiscMisc.LostFocus
        Main.HideHelpText()
    End Sub
    Private Sub LoadMiscName()
        strSql = " SELECT MISCNAME FROM " & cnAdminDb & "..MISCCHARGES WHERE ACTIVE = 'Y'"
        Dim Name As String = BrighttechPack.SearchDialog.Show("Find MiscName", strSql, cn, , , , txtMiscMisc.Text)
        If Name <> "" Then
            txtMiscMisc.Text = Name
            LoadMiscDetails()
        Else
            txtMiscMisc.Focus()
            txtMiscMisc.SelectAll()
        End If
    End Sub
    Private Sub LoadMiscDetails()
        If txtMiscMisc.Text <> "" Then
            strSql = " SELECT DEFAULTVALUE FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCNAME = '" & txtMiscMisc.Text & "'"
            Dim amt As Double = Val(objGPack.GetSqlValue(strSql, "DEFAULTVALUE", , tran))
            txtMiscAmount_Amt.Text = IIf(amt <> 0, Format(amt, "0.00"), "")
            Me.SelectNextControl(txtMiscMisc, True, True, True, True)
        End If
    End Sub

    Private Sub txtTagNo__Man_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTagNo__Man.KeyPress
        Select Case e.KeyChar
            Case "\", "/", "*", """", "<", ">", "|", ":", "."
                e.Handled = True
        End Select
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim _TagNoFrom = GetSoftValue("TAGNOFROM")
            If _TagNoFrom = "I" Then
                strSql = "SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAG"
                strSql += " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "') "
                strSql += " AND TAGNO = '" & txtTagNo__Man.Text & "'"
                If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
                    MsgBox("TagNo Already Exist", MsgBoxStyle.Information)
                    txtTagNo__Man.Focus()
                End If
            ElseIf _TagNoFrom = "U" Then
                strSql = "SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAG"
                strSql += " WHERE TAGNO = '" & txtTagNo__Man.Text & "'"
                If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
                    MsgBox("TagNo Already Exist", MsgBoxStyle.Information)
                    txtTagNo__Man.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub txtPurity_Per_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPurity_Per.GotFocus
        'SendKeys.Send("{TAB}")
    End Sub

    Private Sub cmbCostCentre_Man_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCostCentre_Man.GotFocus
        If costLock Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub ItemChangeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemChangeToolStripMenuItem.Click
        If Not lblItemChange.Visible Then Exit Sub
        LoadLotDetails()
    End Sub

    Private Sub cmbItem_Man_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItem_MAN.GotFocus
        ItemLock = True
        If Not lblItemChange.Visible Then
            ItemLock = False
            Dim dt As New DataTable
            'strSql = funcAssignTagDefaultVal()
            'da = New OleDbDataAdapter(strSql, cn)
            'da.Fill(dt)
            'funcAssignVal(dt)
            If cmbSubItem_Man.Enabled Then cmbSubItem_SelectedIndexChanged(Me, New EventArgs)
            'funcAssignTabControls()
            ItemLock = False
            If objGPack.DupCheck("SELECT 1 FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "' AND STYLENO = 'Y'") Then
                isStyleCode = True
            End If
            '         SendKeys.Send("{TAB}")
            '   ORDetail()
            Exit Sub
        End If
        ListSearch(CType(sender, ComboBox))
    End Sub

    Private Sub cmbItem_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbItem_MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbItem_MAN.Text = "" Then Exit Sub
            Dim AA As Integer = Val(cmbItem_MAN.Text)
            If Val(cmbItem_MAN.Text) <> 0 And cmbItem_MAN.Text = AA.ToString() Then
                ''entrered itemid 
                Dim itemName As String = objGPack.GetSqlValue("SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & cmbItem_MAN.Text & " AND STOCKTYPE = 'T' AND ACTIVE = 'Y' AND ISNULL(STUDDED,'') <> 'Y'")
                If itemName <> "" Then cmbItem_MAN.Text = itemName
            End If
            If objGPack.DupCheck("SELECT 1 FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "' AND STYLENO = 'Y'") Then
                isStyleCode = True
            End If
            'Dim dt As New DataTable
            'strSql = funcAssignTagDefaultVal()
            'da = New OleDbDataAdapter(strSql, cn)
            'da.Fill(dt)
            'funcAssignVal(dt)
            If cmbSubItem_Man.Enabled Then cmbSubItem_SelectedIndexChanged(Me, New EventArgs)
            'funcAssignTabControls()
            'txtLotNo_Num_Man.Enabled = False
            ItemLock = False
            Me.SelectNextControl(cmbItem_MAN, True, True, True, True)
        End If
    End Sub

    Private Sub cmbItem_MAN_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItem_MAN.Leave
        If cmbItem_MAN.Text <> "" And cmbSubItem_Man.Text.Trim = "" Then
            cmbSubItem_Man.Items.Clear()
            If objGPack.GetSqlValue("SELECT SUBITEM FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'") = "Y" Then
                strSql = GetSubItemQry(New String() {"SUBITEMNAME"}, Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'")))
                objGPack.FillCombo(strSql, cmbSubItem_Man, False)
                cmbSubItem_Man.Enabled = True
            Else
                cmbSubItem_Man.Enabled = False
                cmbSubItem_Man.Text = ""
            End If
        End If

        If ItemLock Then
            cmbItem_MAN.Select()
            Exit Sub
        End If
    End Sub

    Private Sub cmbItem_Man_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItem_MAN.LostFocus
        HideSearch()
    End Sub

    Private Sub cmbItem_Man_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItem_MAN.TextChanged
        cmbSearch.Text = CType(sender, ComboBox).Text
    End Sub

    Private Sub ObjMinValue_txtMinWastage_Wet_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.ObjMinValues_txtMinMcPerGram_Amt_TextChanged(Me, New EventArgs)
    End Sub

    Private Sub WeighingScalePropertyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WeighingScalePropertyToolStripMenuItem.Click
        objPropertyDia = New PropertyDia(wSerialPort1)
        objPropertyDia.ShowDialog()
    End Sub

    Private Sub txtGrossWt_Wet_Man_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGrossWt_Wet.LostFocus
        ObjPurDetail.txtPurGrossWt_Wet.Text = txtGrossWt_Wet.Text
    End Sub

    Private Sub txtGrossWt_Wet_MAN_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGrossWt_Wet.TextChanged
        CalcMaxMinValues()
        CalcFinalTotal()
    End Sub

    Private Sub txtTouch_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTouch_AMT.GotFocus
        If TOUCH_LOCK_TAG Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtTouch_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTouch_AMT.TextChanged
        CalcFinalTotal()
    End Sub

    Private Sub txtHmBillNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtHmBillNo.GotFocus
        If Not _HasHallMark Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtHmCentre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtHmCentre.GotFocus
        If Not _HasHallMark Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtRfId_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRfId.GotFocus
        If Not _HasRfId Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtRfId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtRfId.KeyDown
        If Not e.KeyCode = Keys.Insert Then Exit Sub
        Dim obj As CLS_RFID_READER
        obj = New CLS_RFID_READER(RfIdReader.TagReader)
        Dim RFTID_SET() As String = RFID_PORT.Split("/")
        If RFTID_SET.Length >= 1 Then
            obj.pBaudRate = Val(RFTID_SET(0))
        End If
        If RFTID_SET.Length >= 2 Then
            obj.pPortNumber = Val(RFTID_SET(1))
        End If
        If RFTID_SET.Length >= 3 Then
            obj.pFlag = Val(RFTID_SET(2))
        End If
        Dim lst As New List(Of String)
        lst = obj.Read()
        txtRfId.Text = obj.pReadedData.Item(obj.pReadedData.Count - 1)
    End Sub

    Private Sub txtHmBillNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtHmBillNo.KeyPress
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            If HALLMARKVALID = True And txtHmBillNo.Text = "" Then MsgBox("Hallmark Billno Is empty") : txtHmBillNo.Focus() : Exit Sub
        End If
    End Sub

    Private Sub txtHmCentre_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtHmCentre.KeyPress
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            If HALLMARKVALID = True And txtHmCentre.Text = "" Then MsgBox("Hallmark Centre Is empty") : txtHmCentre.Focus() : Exit Sub
        End If
    End Sub

    Private Function StockChecking()
        Dim blnPiece As Boolean = False
        Dim sql As String
        If cmbItem_MAN.Text <> "" Then
            sql = vbCrLf + "Select isnull(Sum(Piece),0)as Piece from " & cnAdminDb & "..StkReorder where ItemId=(Select ItemId from " & cnAdminDb & "..ItemMast where ItemName='" & cmbItem_MAN.Text & "')"
            If cmbSubItem_Man.Text <> "" Then
                sql += vbCrLf + " and SubItemId=(Select SubItemId from " & cnAdminDb & "..SubItemMast where SubItemName='" & cmbSubItem_Man.Text & "')"
                If cmbItemSize.Text <> "" Then
                    sql += vbCrLf + " and SizeId =(Select SizeId from " & cnAdminDb & "..ItemSize where SizeName='" & cmbItemSize.Text & "' and ItemId=(Select ItemId from " & cnAdminDb & "..ItemMast where ItemName='" & cmbItem_MAN.Text & "'))"
                End If
            End If
        End If
        Dim dts As New DataTable()
        da = New OleDbDataAdapter(sql, cn)
        da.Fill(dts)

        If dts.Rows.Count >= 0 Then
            For i As Integer = 0 To dts.Rows.Count - 1
                If Val(dts.Rows(i).Item("Piece").ToString) <> 0 Then
                    If Not txtPieces_Num_Man.Text <= Val(dts.Rows(i).Item("Piece").ToString) Then
                        blnPiece = True
                    End If
                End If
            Next
            If blnPiece = True Then MsgBox("Stock Exceeded...") : txtPieces_Num_Man.Focus() : Exit Function
        End If
    End Function
    Function DetailPrint()
        Dim dtprint As New DataTable
        Dim i, PgNo, line As Integer
        Dim dt As New DataTable
        FileWrite = File.CreateText(Application.StartupPath + "\FilePrint.txt")
        PgNo = 0
        line = 0
        strprint = Chr(27) + "M"
        FileWrite.WriteLine(strprint)
        Dim Date1 As String = Space(14) : Dim date2 As String = Space(10) : Dim Time As String = Space(14)
        Dim Time1 As String = Space(12) : Dim LotNo As String = Space(14) : Dim LotNo1 As String = Space(10)
        Dim User As String = Space(14) : Dim User1 As String = Space(20) : Dim LotPcs As String = Space(14)
        Dim LotPcs1 As String = Space(12) : Dim LGrsWt As String = Space(10) : Dim LGrsWt1 As String = Space(12)
        Dim LNetwt As String = Space(10) : Dim LNetwt1 As String = Space(12) : Dim CompPcs As String = Space(14)
        Dim CompPcs1 As String = Space(12) : Dim CNetwt As String = Space(10) : Dim CNetwt1 As String = Space(12)
        Dim CGrsWt As String = Space(10) : Dim CGrsWt1 As String = Space(12) : Dim Verfyed As String = Space(10)
        Dim Checkedby As String = Space(10) : Dim DiffCompcs As String = Space(14) : Dim DiffCompcs1 As String = Space(10)
        Dim DiffComWt As String = Space(14) : Dim DiffComWt1 As String = Space(10)
        Dim username As String
        strSql = "Select username from " & cnAdminDb & "..usermaster where userid='" & userId & "'"
        username = globalMethods.GetSqlValue(cn, strSql)
        Date1 = LSet("Date         :", 14)
        date2 = LSet(dtpRecieptDate.Value.ToString("dd/MM/yyyy"), 10)
        Time = LSet("Time :", 8)
        Time1 = LSet(Now.ToString(), 8)
        LotNo = LSet("LotNo        :", 14)
        LotNo1 = LSet(txtLotNo_Num.Text.ToString(), 8)
        User = LSet("User :", 8)
        User1 = LSet(username, 15)
        LotPcs = LSet("Lot Pcs      :", 14)
        LotPcs1 = LSet(lblPLot.Text.ToString(), 8)
        LGrsWt = LSet("GrsWt:", 8)
        LGrsWt1 = LSet(lblWLot.Text.ToString(), 8)
        LNetwt = LSet("NetWt:", 8)
        LNetwt1 = LSet(lblWLot.Text.ToString(), 8)
        CompPcs = LSet("Completed Pcs:", 14)
        CompPcs1 = LSet(lblPCompled.Text.ToString(), 8)
        CGrsWt = LSet("GrsWt:", 8)
        CGrsWt1 = LSet(lblWCompleted.Text.ToString(), 8)
        CNetwt = LSet("NetWt:", 8)
        CNetwt1 = LSet(lblWCompleted.Text.ToString(), 8)
        DiffCompcs = LSet("Diff Pcs:", 14)
        DiffCompcs1 = LSet(lblPBalance.Text.ToString(), 8)
        DiffComWt = LSet("Diff Wt:", 8)
        DiffComWt1 = LSet(lblWBalance.Text.ToString(), 8)

        Verfyed = LSet("Verified By:", 8)
        Checkedby = LSet("Checked By:", 8)


        strprint = Date1 + Space(1) + date2 + Space(10) + Time + Space(1) + Time1
        FileWrite.WriteLine(strprint)
        strprint = LotNo + Space(1) + LotNo1 + Space(12) + User + Space(1) + User1
        FileWrite.WriteLine(strprint)
        strprint = "--------------------------------------------------------------"
        FileWrite.WriteLine(strprint)
        strprint = LotPcs + Space(1) + LotPcs1 + Space(12) + LGrsWt + Space(1) + LGrsWt1
        FileWrite.WriteLine(strprint)
        strprint = "" 'Space(35) + LNetwt + Space(1) + LNetwt1
        FileWrite.WriteLine(strprint)
        strprint = CompPcs + Space(1) + CompPcs1 + Space(12) + CGrsWt + Space(1) + CGrsWt1
        FileWrite.WriteLine(strprint)
        strprint = "" 'Space(35) + LNetwt + Space(1) + LNetwt1
        FileWrite.WriteLine(strprint)
        strprint = DiffCompcs + Space(1) + DiffCompcs1 + Space(12) + DiffComWt + Space(1) + DiffComWt1
        FileWrite.WriteLine(strprint)
        strprint = "--------------------------------------------------------------"
        FileWrite.WriteLine(strprint)
        strprint = Verfyed + Space(27) + Checkedby
        FileWrite.WriteLine(strprint)
        strprint = ""
        FileWrite.WriteLine(strprint)
        FileWrite.WriteLine(strprint)
        FileWrite.Close()
        line += 1
        frmPrinterSelect.Show()
    End Function

    Private Sub txt_seal_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_seal.GotFocus
        Dim dtseal As New DataTable
        strSql = " SELECT SEAL FROM " & cnAdminDb & "..DESIGNER WHERE ISNULL(ACTIVE,'') <> 'N' ORDER BY DESIGNERNAME"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtseal)

        If Not dtseal.Rows.Count > 0 Then Exit Sub
        cmbSearch.Items.Clear()
        For i As Integer = 0 To dtseal.Rows.Count - 1
            cmbSearch.Items.Add(dtseal.Rows(i).Item(0).ToString)
        Next
        pnlSearch.Visible = True
    End Sub

    Private Sub txt_seal_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_seal.Leave
        If txt_seal.Text <> "" Then
            strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ISNULL(ACTIVE,'') <> 'N' AND SEAL='" & txt_seal.Text & "'"
            Dim dlrname As String = GetSqlValue(cn, strSql)
            If dlrname = "" Then MsgBox("Given Seal Not Found") : txt_seal.Text = "" : txt_seal.Focus() : Exit Sub
            cmbDesigner_MAN.Text = dlrname
        End If
    End Sub

    Private Sub txt_seal_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_seal.LostFocus
        HideSearch()
    End Sub

    Private Sub txt_seal_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_seal.TextChanged
        cmbSearch.Text = CType(sender, TextBox).Text
    End Sub

    Private Sub txtstPackettno_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtstPackettno.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridStone.RowCount > 0 Then
                gridStone.Focus()
            End If
        End If
    End Sub

    Private Sub txtstPackettno_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtstPackettno.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If PacketNoEnable = "R" Then
                If txtstPackettno.Text = "" Then
                    MsgBox("Packet No. must enter", MsgBoxStyle.Information)
                    txtstPackettno.Focus()
                    Exit Sub
                End If
            End If
            Me.SelectNextControl(tabStone, True, True, True, True)
        End If
    End Sub
    Private Function LoadAdditionalStockEntry()
        'DtAddStockEntry.Rows.Clear()
        Dim miscid As Integer = 1
        Dim OItemid As Integer
        strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItem_MAN.Text & "'"
        OItemid = Val(objGPack.GetSqlValue(strSql, "ITEMID", , tran))

        strSql = "SELECT * FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE ISNULL(ACTIVE,'')<>'N' ORDER BY MISCID"
        Dim DtOth As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(DtOth)
        For k As Integer = 0 To DtOth.Rows.Count - 1
            If miscid > 2 Then Exit For
            Dim Rindex As Integer = 0
            Dim MNAME As String = ""
            If DtAddStockEntry.Rows.Count > 0 Then
                Dim roow() As DataRow = Nothing
                roow = DtAddStockEntry.Select("MISCID='" & Val(DtOth.Rows(k).Item("MISCID").ToString) & "'", "")
                If roow.Length > 0 Then
                    Rindex = Val(roow(0).Item("KEYNO").ToString)  'Val(DtAddStockEntry.Compute("MAX(KEYNO)", "MISCID='" & Val(DtOth.Rows(k).Item("MISCID").ToString) & "'"))
                    MNAME = DtAddStockEntry.Rows(Rindex - 1).Item("OTHNAME").ToString
                End If
            End If
            strSql = " SELECT ID,NAME,MISCID FROM " & cnAdminDb & "..OTHERMASTER WHERE ISNULL(ACTIVE,'')<>'N' "
            strSql += " "
            strSql += " AND (ITEMID='" & OItemid & "' OR ITEMID=0)"
            If wcmbAddM1_OWN.Text <> "" And miscid = 1 Then strSql += " AND MISCID='" & miscid & "' AND NAME='" & wcmbAddM1_OWN.Text & "'"
            If wcmbAddM2_OWN.Text <> "" And miscid = 2 Then strSql += " AND MISCID='" & miscid & "' AND NAME='" & wcmbAddM2_OWN.Text & "'"
            cmd = New OleDbCommand(strSql, cn)
            Dim dtAddmaster As New DataTable
            da = New OleDbDataAdapter(cmd)
            da.Fill(dtAddmaster)

            If dtAddmaster.Rows.Count > 0 Then
                If Rindex > 0 Then
                    With DtAddStockEntry.Rows(Rindex - 1)
                        .Item("OTHID") = Val(dtAddmaster.Rows(0).Item("ID").ToString)
                        .Item("OTHNAME") = dtAddmaster.Rows(0).Item("NAME").ToString
                        .Item("MISCID") = Val(dtAddmaster.Rows(0).Item("MISCID").ToString)
                    End With
                Else
                    Dim ro As DataRow = Nothing
                    ro = DtAddStockEntry.NewRow
                    ro("OTHID") = Val(dtAddmaster.Rows(0).Item("ID").ToString)
                    ro("OTHNAME") = dtAddmaster.Rows(0).Item("NAME").ToString
                    ro("MISCID") = Val(dtAddmaster.Rows(0).Item("MISCID").ToString)
                    DtAddStockEntry.Rows.Add(ro)
                End If
            End If
        Next
    End Function

    Private Sub txtTagNo__Man_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTagNo__Man.LostFocus
        LoadAdditionalStockEntry()
    End Sub
    Private Sub txtNarration_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNarration.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
            If Not tagEdit Then
                If STKAFINDATE = False Then
                    If Not CheckDate(dtpRecieptDate.Value) Then Exit Sub
                    If CheckEntryDate(dtpRecieptDate.Value) Then Exit Sub
                End If
            End If
            If cmbSubItem_Man.Text <> "" Then
                strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')"
            Else
                strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"
            End If
            Dim calcType As String = objGPack.GetSqlValue(strSql)
            ''Weight Rate Validation
            Select Case calcType.ToUpper
                Case "W"
                    If Val(txtGrossWt_Wet.Text) = 0 Then
                        MsgBox("Grs Weight should not empty", MsgBoxStyle.Information)
                        txtGrossWt_Wet.Focus()
                        Exit Sub
                    End If
                Case "R"
                    If Val(txtRate_Amt.Text) = 0 Then
                        MsgBox("Rate should not empty", MsgBoxStyle.Information)
                        txtRate_Amt.Focus()
                        Exit Sub
                    End If
                Case "B"
                    If Val(txtGrossWt_Wet.Text) = 0 Then
                        MsgBox("Grs Weight should not empty", MsgBoxStyle.Information)
                        txtGrossWt_Wet.Focus()
                        Exit Sub
                    End If
                    If Val(txtRate_Amt.Text) = 0 Then
                        MsgBox("Rate should not empty", MsgBoxStyle.Information)
                        txtRate_Amt.Focus()
                        Exit Sub
                    End If
                Case "F"
                    If Val(txtGrossWt_Wet.Text) = 0 And Val(txtRate_Amt.Text) = 0 Then
                        MsgBox("Weight and Rate should not empty", MsgBoxStyle.Information)
                        txtGrossWt_Wet.Focus()
                        Exit Sub
                    End If
            End Select

            If (_HasPurchase Or pFixedVa) Then
                If ObjPurDetail.Loadd = False Then
                    If tagEdit And TAGEDITDISABLE.Contains("PV") Then GoTo nnnext
                    txtSalValue_KeyPress(Me, New KeyPressEventArgs(Chr(Keys.Enter)))
                    Exit Sub
                End If
            End If

            If PUR_AUTOCALC = True And Not (_HasPurchase Or pFixedVa) Then
                ShowPurDetails()
            End If
nnnext:
            If picPath <> Nothing Then
                If Not IO.Directory.Exists(defalutDestination) Then
                    MsgBox(defalutDestination & vbCrLf & " Destination Path not found" & vbCrLf & "Please make the correct path", MsgBoxStyle.Information)
                    Exit Sub
                End If
            End If
            If _TagImageMust Then
                If picPath = Nothing Then
                    MsgBox("Please Attach Image", MsgBoxStyle.Information)
                    btnAttachImage.Select()
                    Exit Sub
                End If
            End If
           
            If objGPack.Validator_Check(grpSaveControls) Then Exit Sub
            Dim ds As New DataSet
            ds.Clear()
            Dim tDd As Date = GetServerDate()
            If GetAdmindbSoftValue("GLOBALDATE").ToUpper = "Y" Then
                tDd = GetEntryDate(tDd)
            End If
            If Not (dtpRecieptDate.Value.Date.ToString("yyyy-MM-dd") >= lotRecieptDate.Date.ToString("yyyy-MM-dd")) Then
                Dim errStr As String
                errStr = "Reciept Date Should not allow before LotDate"
                'errStr += " And Receipt Date Should not Exceed Today Date"
                MsgBox(errStr, MsgBoxStyle.Exclamation)
                dtpRecieptDate.Focus()
                Exit Sub
            End If
            If Not tagEdit Then
                If Not dtpRecieptDate.Value.Date.ToString("yyyy-MM-dd") <= tDd.ToString("yyyy-MM-dd") Then
                    Dim errStr As String
                    errStr = "Receipt Date Should not Exceed Today Date"
                    MsgBox(errStr, MsgBoxStyle.Exclamation)
                    dtpRecieptDate.Focus()
                    Exit Sub
                End If
            End If
            If Tag_Tolerance <> 0 Then
                If Val(lblPBalance.Text) - Val(txtPieces_Num_Man.Text) = 0 Then
                    If Math.Round(Math.Abs(Val(lblWBalance.Text) - Val(txtGrossWt_Wet.Text)), 3) > Math.Round(Val(Tag_Tolerance), 3) Then
                        MsgBox(Me.GetNextControl(txtPieces_Num_Man, False).Text + E0024 + lblWBalance.Text, MsgBoxStyle.Exclamation)
                        txtGrossWt_Wet.Focus()
                        Exit Sub
                    End If
                End If
            End If
           
            If txtLessWt_Wet.Enabled = True Then
                If Val(txtGrossWt_Wet.Text) <> 0 And Val(txtLessWt_Wet.Text) > Val(txtGrossWt_Wet.Text) Then
                    'If Val(txtGrossWt_Wet.Text) <> 0 And Val(txtLessWt_Wet.Text) >= Val(txtGrossWt_Wet.Text) Then
                    MsgBox(E0004 + Me.GetNextControl(txtLessWt_Wet, False).Text, MsgBoxStyle.Exclamation)
                    txtLessWt_Wet.Focus()
                    Exit Sub
                End If
            End If
            If txtNetWt_Wet.Enabled = True Then
                If Val(txtGrossWt_Wet.Text) <> 0 And Val(txtNetWt_Wet.Text) <= 0 And Val(txtGrossWt_Wet.Text) <> Val(txtLessWt_Wet.Text) Then
                    MsgBox("Net Weight Should not Empty", MsgBoxStyle.Exclamation)
                    txtNetWt_Wet.Focus()
                    Exit Sub
                End If
                If Val(txtNetWt_Wet.Text) > Val(txtGrossWt_Wet.Text) Then
                    MsgBox(Me.GetNextControl(txtNetWt_Wet, False).Text + E0015 + Me.GetNextControl(txtGrossWt_Wet, False).Text, MsgBoxStyle.Exclamation)
                    txtNetWt_Wet.Focus()
                    Exit Sub
                End If
            End If
            If Val(ObjMinValue.txtMinWastage_Per.Text) > Val(txtMaxWastage_Per.Text) Then
                MsgBox("MinWastage Per Should not Exceed MaxWastage Per", MsgBoxStyle.Information)
                ObjMinValue.txtMinWastage_Per.Focus()
                Exit Sub
            End If
            If Val(ObjMinValue.txtMinWastage_Wet.Text) > Val(txtMaxWastage_Wet.Text) Then
                MsgBox("MinWastage Should not Exceed MaxWastage ", MsgBoxStyle.Information)
                ObjMinValue.txtMinWastage_Wet.Focus()
                Exit Sub
            End If
            If Val(ObjMinValue.txtMinMcPerGram_Amt.Text) > Val(txtMaxMcPerGrm_Amt.Text) Then
                MsgBox("Min Mc Per Grm Should not Exceed Max Mc Per Grm", MsgBoxStyle.Information)
                ObjMinValue.txtMinMcPerGram_Amt.Focus()
                Exit Sub
            End If
            If Val(ObjMinValue.txtMinMkCharge_Amt.Text) > Val(txtMaxMkCharge_Amt.Text) Then
                MsgBox("Min Making Charge Should not Exceed Max Making Charge", MsgBoxStyle.Information)
                ObjMinValue.txtMinMkCharge_Amt.Focus()
                Exit Sub
            End If
            If dtpRecieptDate.Value <> wdtpRecieptDate.Value Then tagWebtagDiff = True
            If cmbItem_MAN.Text <> wcmbItem_MAN.Text Then tagWebtagDiff = True
            If cmbSubItem_Man.Text <> WcmbSubItem_OWN.Text And cmbSubItem_Man.Enabled = True Then tagWebtagDiff = True
            'If txtTagNo__Man.Text <> wtxtTagNo__Man.Text Then tagWebtagDiff = True
            If cmbItemType_MAN.Text <> wcmbPurity.Text Then tagWebtagDiff = True
            If txtPieces_Num_Man.Text <> wtxtPieces_Num_Man.Text Then tagWebtagDiff = True
            If txtGrossWt_Wet.Text <> WtxtGrossWt_Wet.Text Then tagWebtagDiff = True
            If txtNetWt_Wet.Text <> WtxtNetWt_Wet.Text Then tagWebtagDiff = True
            If txtLessWt_Wet.Text <> WtxtLessWt_Wet.Text Then tagWebtagDiff = True
            If txtMaxMcPerGrm_Amt.Text <> wtxtMcPerGrm_Amt.Text Then tagWebtagDiff = True
            If txtMaxMkCharge_Amt.Text <> wtxtMkCharge_Amt.Text Then tagWebtagDiff = True
            If txtMetalRate_Amt.Text <> wtxtMetalRate_Amt.Text Then tagWebtagDiff = True
            If txtSalValue_Amt_Man.Text <> wtxtSalValue_Amt_Man.Text Then tagWebtagDiff = True
            If tagWebtagDiff = True Then
                If MessageBox.Show("Some value can be different from WebTag.Do you want to update Webtag?", "Update Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.OK Then

                    wdtpRecieptDate.Value = dtpRecieptDate.Value
                    wcmbItem_MAN.Text = cmbItem_MAN.Text
                    WcmbSubItem_OWN.Text = cmbSubItem_Man.Text
                    'wtxtTagNo__Man.Text = txtTagNo__Man.Text
                    wcmbPurity.Text = cmbItemType_MAN.Text
                    wtxtPieces_Num_Man.Text = txtPieces_Num_Man.Text
                    WtxtGrossWt_Wet.Text = txtGrossWt_Wet.Text
                    WtxtNetWt_Wet.Text = txtNetWt_Wet.Text
                    WtxtLessWt_Wet.Text = txtLessWt_Wet.Text
                    wtxtMcPerGrm_Amt.Text = txtMaxMcPerGrm_Amt.Text
                    wtxtMkCharge_Amt.Text = txtMaxMkCharge_Amt.Text
                    wtxtMMRate.Text = txtMMRate.Text
                    dtWStoneDetails = dtStoneDetails
                    dtWStoneDetails.AcceptChanges()
                    wgridStone.DataSource = dtWStoneDetails
                    wtxtSalValue_Amt_Man.Text = txtSalValue_Amt_Man.Text
                End If
            End If

            Tagsave = True
            TabGeneral.SelectedTab = tabWebTag
            WbtnSave.Focus()
        End If
    End Sub
#End Region
End Class