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
Imports System.Threading

Public Class frmItemTag
    '#    For Image Capture          #
    ' 328 kalaiarasan .
    '01 SHERIFF 25-10-12
    'CALNO 386 ALTER BY VASANTH FOR PRINCE- CALCMODE LODE FROM SOFTCONTROL ONLY
    'CALNO 280913 ALTER BY VASANTH FOR PRINCE- Order tag can allow when smith record will found
    Dim oldpath, oldpath1, newpath, newpath1 As String
    Dim flagDeviceMode As Boolean = False
    Dim setItem As Boolean = False
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
    Dim Tagdesignedit As Boolean = IIf(GetAdmindbSoftValue("TAGDESIGNERLOTONLY", "N") = "Y", True, False)
    Dim CHKBOOKSTOCK As Boolean = IIf(GetAdmindbSoftValue("CHK_BOOK_STK", "N") = "Y", True, False)
    Dim CHK_BOOK_CTR_STOCK As Boolean = IIf(GetAdmindbSoftValue("CHK_BOOK_CTR_STOCK", "N") = "Y", True, False)
    Dim AUTOWTSKIPEDIT As Boolean = IIf(GetAdmindbSoftValue("AUTOWTSKIPEDIT", "N") = "Y", True, False)
    Dim NARRATION_VIEW_IN_ITEMTAG As Boolean = IIf(GetAdmindbSoftValue("NARRATION_VIEW_IN_ITEMTAG", "N") = "Y", True, False)
    Dim RepWastMcExOnly As Boolean = IIf(GetAdmindbSoftValue("REPWASTMCEXONLY", "N") = "Y", True, False)
    Dim TagLotCheck As Boolean = IIf(GetAdmindbSoftValue("TAGLOTCHECK", "N") = "Y", True, False)
    Dim OrdVA_Chg As Boolean = IIf(GetAdmindbSoftValue("ORDERVACHANGE", "N") = "Y", True, False)
    Dim VAL_ITEMTYPE_TABLE As Boolean = IIf(GetAdmindbSoftValue("VALIDATE_ITEMTYPE_TABLE", "N") = "Y", True, False)
    Dim SetGrpIdVis As Boolean = IIf(GetAdmindbSoftValue("SETITEMVIS", "N") = "Y", True, False)
    Dim objSetItem As New frmSetItemWastMc
    Dim strprint As String
    Dim Ratevaluezero As Boolean = False
    Dim FileWrite As StreamWriter
    Dim hHwnd As Integer  ' Handle value to preview window
    Declare Function SendMessage Lib "user32" Alias "SendMessageA" _
        (ByVal hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer,
         ByVal lParam As Object) As Integer
    Declare Function SetWindowPos Lib "user32" Alias "SetWindowPos" (ByVal hwnd As Integer,
        ByVal hWndInsertAfter As Integer, ByVal x As Integer, ByVal y As Integer,
        ByVal cx As Integer, ByVal cy As Integer, ByVal wFlags As Integer) As Integer

    Declare Function DestroyWindow Lib "user32" (ByVal hndw As Integer) As Boolean
    Declare Function capCreateCaptureWindowA Lib "avicap32.dll" _
            (ByVal lpszWindowName As String, ByVal dwStyle As Integer,
            ByVal x As Integer, ByVal y As Integer, ByVal nWidth As Integer,
            ByVal nHeight As Short, ByVal hWndParent As Integer,
            ByVal nID As Integer) As Integer

    '#    For Image Capture          #

    Dim Stylecode As String
    Dim objPropertyDia As New PropertyDia(SerialPort1)
    Dim ItemLock As Boolean
    Dim updIssSno As String
    Dim dtTagDetails As New DataTable
    Dim lotPcs As Integer
    Dim lotGrsWt As Double
    Dim lotNetWt As Double
    Dim tagEdit As Boolean
    Dim RepairLot As Boolean = False
    Dim tagwastEditotp As Boolean = False
    Dim multiMetalCalc As Boolean = False
    Dim BlockSv As Boolean = False
    Dim isStyleCode As Boolean
    Dim dtGridView As New DataTable
    Dim dtStoneDetails As New DataTable("GridStone")
    Dim dtMultiMetalDetails As New DataTable("GridMultiMetal")
    Dim dtMiscDetails As New DataTable("GridMisc")
    Dim dtHallmarkDetails As New DataTable("gridHallmarkDetails")

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

    Dim CatalogDestination As String = Nothing
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
    Dim PurVal_Disable As Boolean = IIf(GetAdmindbSoftValue("PURVAL_DISABLE", "N") = "Y", True, False)
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
    Dim ObjTagWt As New frmTagWt
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
    Dim TAG_TBLCODESEARCHOPT As Boolean = IIf(GetAdmindbSoftValue("TAG_TBLCODESEARCHOPT", "N") = "Y", True, False)
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
    Dim ObjDiaDetails As New frm4C
    Dim ObjDiamondDetails As New frmDiamondDetails
    Dim Cut As String = ""
    Dim Color As String = ""
    Dim Clarity As String = ""
    Dim Shape As String = ""
    Dim SetType As String = ""
    Dim Width As Decimal = 0
    Dim Height As Decimal = 0
    Dim StnGrpId As Integer = 0
    Dim TagCutId As Integer = 0
    Dim TagColorId As Integer = 0
    Dim TagClarityId As Integer = 0
    Dim TagShapeId As Integer = 0
    Dim TagSetTypeId As Integer = 0
    Dim TagWidth As Decimal = 0
    Dim TagHeight As Decimal = 0
    Dim TagStnGrpId As Integer = 0
    Dim TagStnGroup As String = ""
    Dim TChkbStk As Boolean = True
    Dim DtAddStockEntry As New DataTable
    Dim AddStockEntry As Boolean = True
    Dim SubItemPic As Boolean = False
    Dim STUDDEDWTPER As String = GetAdmindbSoftValue("STUDDEDWTPER", "N")
    Dim objstudPer As New StuddedDeDuctPer
    Dim OrderDetail As String = ""
    Dim TAG_STONEAUTOLOAD_MR As Boolean = IIf(GetAdmindbSoftValue("TAG_STONEAUTOLOAD_MR", "N") = "Y", True, False)
    Dim TAG_STONECALTYPE As Boolean = IIf(GetAdmindbSoftValue("TAG_STONECALTYPE", "N") = "Y", True, False)
    Dim ACC_STUDITEM_POPUP As Boolean = IIf(GetAdmindbSoftValue("ACC_STUDITEM_POPUP", "Y") = "Y", True, False)
    Dim WT_BALANCE_FORMAT As Integer = GetAdmindbSoftValue("WT_BALANCE_FORMAT", 1)
    Dim SMS_MSG_ORDTAG As String = objGPack.GetSqlValue("SELECT ISNULL(TEMPLATE_DESC,'') AS TEMPLATE_DESC FROM " & cnAdminDb & "..SMSTEMPLATE WHERE TEMPLATE_NAME='SMS_MSG_ORDTAG' AND ISNULL(ACTIVE,'Y')<>'N'", "TEMPLATE_DESC").ToString
    Dim TAGTOBRANCH As Boolean = IIf(GetAdmindbSoftValue("TAGTOBRANCH", "N") = "Y", True, False)
    Dim cnHoCostname As String = ""
    Dim PUR_LANDCOST As Boolean = IIf(GetAdmindbSoftValue("PUR_LANDCOST", "N").ToUpper = "Y", True, False)
    Dim TAGEDITPCSWT As String = GetAdmindbSoftValue("TAGEDITPCSWT", "N")
    Dim Authorize As Boolean = False
    Dim TAGDETDISABLE As String = GetAdmindbSoftValue("TAGDETDISABLE", "N").ToString
    Dim SMS_TAG_UPDATE As String = objGPack.GetSqlValue("SELECT ISNULL(TEMPLATE_DESC,'') AS TEMPLATE_DESC FROM " & cnAdminDb & "..SMSTEMPLATE WHERE TEMPLATE_NAME='SMS_TAG_UPDATE' AND ISNULL(ACTIVE,'Y')<>'N'", "TEMPLATE_DESC").ToString
    Dim issOrder As Boolean = False
    Dim DesignerStone As Boolean = False
    Dim _4C As Boolean = False
    'FOR TAGNO FROM RANGE MASTER
    Dim TagNo_Range As Boolean = IIf(GetAdmindbSoftValue("TAGNOFROMRANGEMAST", "N") = "Y", True, False)
    Dim TagNo_RangeBase As Boolean = IIf(GetAdmindbSoftValue("TAGNO_RANGEBASED", "N") = "Y", True, False)
    Dim StrTagNo As String = ""
    Dim StrSrtName As String = ""
    Dim StrCaption As String = ""
    Dim RangeSno As String = ""
    Dim TAG_LOT_ALLOW_COSTID As String = GetAdmindbSoftValue("TAG_ALLOW_COSTID", "")
    Dim STUD_STNWTPER As Boolean = IIf(GetAdmindbSoftValue("STUD_STNWTPER", "N") = "Y", True, False)
    Dim SalePer As Double = 0
    Dim FIXEDITEM_SALEPER As Boolean = IIf(GetAdmindbSoftValue("FIXEDITEM_SALEPER", "N") = "Y", True, False)
    Dim CallBarcodeExe As Boolean = IIf(GetAdmindbSoftValue("CALLBARCODEEXE", "N") = "Y", True, False)
    Dim REPEAT_TAGPCS As Boolean = IIf(GetAdmindbSoftValue("REPEAT_TAGPCS", "N") = "Y", True, False)
    Dim TAGWOLOT As Boolean = IIf(GetAdmindbSoftValue("TAGWOLOT", "N") = "Y", True, False)
    Dim ORDMULTIPCSCHK As Boolean = IIf(GetAdmindbSoftValue("ORDMULTIPCSCHK", "Y") = "Y", True, False)
    Dim ITEMTAGNARRATION As Boolean = IIf(GetAdmindbSoftValue("ITEMTAGNARRATION", "N") = "Y", True, False)
    Dim ragshow As Boolean = IIf(GetAdmindbSoftValue("RANGEINLOT", "N") = "Y", True, False)
    Dim EXCLUDE_RANGE As String = GetAdmindbSoftValue("EXCLUDE_RANGE", "").ToString
    Dim ORDWTINTAG As Boolean = IIf(GetAdmindbSoftValue("ORDWTINTAG", "Y") = "Y", True, False)
    Dim COPYIMAGETOCATALOGPATH As Boolean = IIf(GetAdmindbSoftValue("COPYIMAGETOCATALOGPATH", "N") = "Y", True, False)
    Dim _AuthPurchase As Boolean = IIf(GetAdmindbSoftValue("PURTAB_AUTH", "N") = "N", False, True)
    Dim TAGWISE_DISCOUNT As Boolean = IIf(GetAdmindbSoftValue("TAGWISE_DISCOUNT", "N") = "Y", True, False)
    Dim PurTab_GrsNet As Boolean = IIf(GetAdmindbSoftValue("PURTAB_GRSNET", "N") = "Y", True, False)
    Dim PurGrsNet As String = ""
    Dim Tag_ManualDate As Boolean = IIf(GetAdmindbSoftValue("TAGMANUALDATE", "N") = "Y", True, False)
    Dim TAGNO_SIZEBASED As Boolean = IIf(GetAdmindbSoftValue("TAGNO_SIZEBASED", "N") = "Y", True, False)
    Dim MANUAL_TAGNO As Boolean = IIf(GetAdmindbSoftValue("MANUAL_TAGNO", "N") = "Y", True, False)
    Dim W_Sale As Boolean = IIf(GetAdmindbSoftValue("ISWHOLESALE", "N") = "Y", True, False)
    Dim PURSTNRATE_AMOUNT As Boolean = IIf(GetAdmindbSoftValue("PURSTNRATE_AMOUNT", "N") = "Y", True, False)
    Dim STUDDIA_MAND As Boolean = IIf(GetAdmindbSoftValue("STUDDIA_PCS_MAND", "N") = "Y", True, False)
    Dim TAGTYPE_CATEGORY As Boolean = IIf(GetAdmindbSoftValue("TAGTYPE_CATEGORY", "N") = "Y", True, False)
    Dim LOCK_VA_ITEMTAG As Boolean = IIf(GetAdmindbSoftValue("LOCK_VA_ITEMTAG", "N") = "Y", True, False)
    Dim LOCK_MAXWASTAGEPER_ITEMTAG As Boolean = IIf(GetAdmindbSoftValue("LOCK_MAXWASTAGEPER_ITEMTAG", "N") = "Y", True, False)
    Dim RATE_FROM_WMCTABLE As Boolean = IIf(GetAdmindbSoftValue("RATE_FROM_WMCTABLE", "N") = "Y", True, False)
    Dim PURTAB_LOCK As Boolean = IIf(GetAdmindbSoftValue("PURTAB_LOCK", "N") = "N", False, True)
    Dim LOCK_TAGEDIT_VA As String = GetAdmindbSoftValue("LOCK_TAGEDIT_VA", "").ToUpper
    Dim ORDER_MULTI_MIMR As Boolean = IIf(GetAdmindbSoftValue("ORDER_MULTI_MIMR", "N") = "Y", True, False)
    Dim STOCK_VALIDATION As Boolean = IIf(GetAdmindbSoftValue("STOCK_VALIDATION", "N") = "Y", True, False)
    Dim EDITTAGPCS As Boolean = IIf(GetAdmindbSoftValue("EDITTAGPCS", "Y") = "Y", True, False)
    Dim TAGIMAGE_ITEMMAST As Boolean = IIf(GetAdmindbSoftValue("TAGIMAGE_ITEMMAST", "Y") = "Y", True, False)
    Dim TAGIMAGE As Boolean = False
    Dim SPECIFICFORMAT As String = GetAdmindbSoftValue("SPECIFICFORMAT", "")
    Dim TagPrefix_Item As Boolean = IIf(GetAdmindbSoftValue("TAGPREFIX_ITEM", "N") = "Y", True, False)
    Dim HallmarkMinLen As Integer = Val(GetAdmindbSoftValue("HALLMARK_MINLEN", 0,, True))
    Dim HallmarknoAsTagno As Boolean = IIf(GetAdmindbSoftValue("HALLMARKNOASTAGNO", "Y") = "Y", True, False)
    Dim NeedHallmark As Boolean = False
    Dim UHallmarknoAsTagno As Boolean = False
    Dim COPYIMAGEFROMCATALOGPATH As Boolean = IIf(GetAdmindbSoftValue("COPYIMAGEFROMCATALOGPATH", "N") = "Y", True, False)
    Dim TAGEDITCOSTCENTRE As Boolean = IIf(GetAdmindbSoftValue("TAGEDITCOSTCENTRE", "N") = "Y", True, False)
    Dim MetalBasedStone As Boolean = IIf(GetAdmindbSoftValue("MULTIMETALBASEDSTONE", "N") = "Y", True, False)
    Dim MultimetalValidWt As Boolean = IIf(GetAdmindbSoftValue("MULTIMETAL_VALIDATE_WT", "N") = "Y", True, False)
    Dim Itemmast_PctPath As Boolean = IIf(GetAdmindbSoftValue("PICPATHFROM", "S") = "I", True, False)
    Dim NeedTag_Hsncode As Boolean = IIf(GetAdmindbSoftValue("TAG_ENTRY_FOCUS_HSNCODE", "N") = "Y", True, False)
    Dim DefaultPctFile As String = GetAdmindbSoftValue("DEFAULT_PCTFILE", "")
    Dim NeedTag_CalcType As Boolean = IIf(GetAdmindbSoftValue("TAGENTRY_FOCUS_CALCTYPE", "N") = "Y", True, False)
    Dim NeedTag_SetItem As Boolean = IIf(GetAdmindbSoftValue("TAGENTRY_FOCUS_SETITEM", "N") = "Y", True, False)
    Dim NeedTag_UpdImg As Boolean = IIf(GetAdmindbSoftValue("TAGEDIT_UPDATEIMAGE", "Y") = "Y", True, False)
    Dim _LEntryType As String = ""
    Dim NeedOldTag_Recdate As Boolean = IIf(GetAdmindbSoftValue("TAG_ENTRY_FOCUS_RRECDATE", "N") = "Y", True, False)
    Dim PoNumber As String = String.Empty
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

    Public Sub New(ByVal ISSsno As String)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Authorize = BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Authorize, False)
        Me.BackColor = frmBackColor
        Me.BackgroundImage = bakImage
        Me.BackgroundImageLayout = ImageLayout.Stretch
        Me.StartPosition = FormStartPosition.CenterScreen
        BrighttechPack.LanguageChange.Set_Language_Form(Me, LangId)
        objGPack.Validator_Object(Me)
        Me.MdiParent = Main
        If TAGWISE_DISCOUNT Then txtWDisc_Per.Visible = True
        If objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'MULTIMETALCALC'") = "Y" Then
            multiMetalCalc = True
        End If
        tagEdit = True
        tagwastEditotp = False
        updIssSno = ISSsno
        ' Add any initialization after the InitializeComponent() call.
        pnlSearch.Location = New Point(783, 16)
        pnlSearch.Size = New Size(209, 394)
        Me.Controls.Add(pnlSearch)
        pnlSearch.BringToFront()
        txtNarration.CharacterCasing = CharacterCasing.Normal
        'Me.WindowState = FormWindowState.Maximized
        pnlMain.BorderStyle = BorderStyle.None
        pnlMultiMetal.BackColor = Drawing.Color.LightGoldenrodYellow
        pnlStoneGrid.BackColor = Drawing.Color.LightGoldenrodYellow
        pnlMisc.BackColor = Drawing.Color.LightGoldenrodYellow

        TabControl1.SelectTab(tabTag)
        Me.TabControl1_SelectedIndexChanged(Me, New EventArgs)
        'TabControl1.SelectedTab.Name = tabTag.Name

        ''dtGridView
        gridView.BorderStyle = BorderStyle.None

        With dtGridView.Columns
            ''SUBITEM,ITEMSIZE,TAGNO,PIECES,GRSWEIGHT,LESSWEIGHT,NETWT,RATE,CALCMODE,TABLECODE
            .Add("ITEM", GetType(String))
            .Add("SUBITEM", GetType(String))
            .Add("TAGNO", GetType(String))
            .Add("PCS", GetType(Int16))
            .Add("GRSWT", GetType(Decimal))
            .Add("LESSWT", GetType(Decimal))
            .Add("NETWT", GetType(Decimal))
            .Add("WASTAGE", GetType(Decimal))
            .Add("MC", GetType(Decimal))
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
            .Columns("WASTAGE").Width = 70
            .Columns("WASTAGE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("MC").Width = 70
            .Columns("MC").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RATE").Width = 70
            .Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SALEVALUE").Width = 100
            .Columns("SALEVALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SALEVALUE").HeaderText = "SALE VALUE"
            .Columns("SIZE").Width = 200
        End With
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        ''MultiMetal
        With dtMultiMetalDetails.Columns
            .Add("CATEGORY", GetType(String))
            .Add("RATE", GetType(Double))
            .Add("WEIGHT", GetType(Double))
            If MetalBasedStone = True Then
                .Add("NETWT", GetType(Double))
            End If
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

        ''Stone
        With dtStoneDetails.Columns
            .Add("PACKETNO", GetType(String))
            .Add("ITEM", GetType(String))
            .Add("SUBITEM", GetType(String))
            .Add("COLOR", GetType(String))
            .Add("CLARITY", GetType(String))
            .Add("SHAPE", GetType(String))
            .Add("STNSIZE", GetType(String))
            .Add("WPER", GetType(Decimal))
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
            .Add("SETTYPE", GetType(String))
            .Add("HEIGHT", GetType(Decimal))
            .Add("WIDTH", GetType(Decimal))
            .Add("STNGRPID", GetType(Integer))
            .Add("MKEYNO", GetType(Integer))
            .Add("MSNO", GetType(String))
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
        gridStone.Columns("MKEYNO").Visible = False
        gridStone.Columns("MSNO").Visible = False
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

        ''HALLMARK DETAILS
        With dtHallmarkDetails.Columns
            .Add("GRSWT", GetType(Decimal))
            .Add("HM_BILLNO", GetType(String))
            .Add("PCS", GetType(Integer))
            .Add("KEYNO", GetType(Integer))
        End With
        dtHallmarkDetails.Columns("KEYNO").AutoIncrement = True
        dtHallmarkDetails.Columns("KEYNO").AutoIncrementStep = 1
        dtHallmarkDetails.Columns("KEYNO").AutoIncrementSeed = 1
        gridHallmarkDetails.DataSource = dtHallmarkDetails
        StyleHallmarkDet()


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

        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'TAGCATALOGPATH'"
        CatalogDestination = UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", , tran))
        If Not CatalogDestination.EndsWith("\") And CatalogDestination <> "" Then CatalogDestination += "\"

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
        funcDiaDetails()
        strSql = " SELECT T.LOTSNO,"
        strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEMNAME"
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
        strSql += " ,T.HM_BILLNO,T.HM_CENTER,T.ADD_VA_PER,T.REFVALUE,T.ORDREPNO,T.EXTRAWT,T.TAGWT,T.COVERWT"
        strSql += " ,T.USRATE,T.INDRS"
        If _FourCMaintain Then
            strSql += " ,T.STNGRPID,T.CUTID,T.COLORID,T.CLARITYID,T.SHAPEID,T.SETTYPEID,T.HEIGHT,T.WIDTH"
        End If
        strSql += " ,ISNULL(TAGFIXEDVA,'N') TAGFIXEDVA,P.DESCRIPT  "
        strSql += " ,P.PURLCOSTPER,P.PURLCOST,P.PURSCOSTPER"
        strSql += " ,ISNULL(SETTAGNO,'')SETTAGNO,T.WEIGHTUNIT,ISNULL(T.WASTDISCPER,0)WASTDISCPER"
        strSql += " ,ISNULL(HSN,'')HSN"
        strSql += " ,CASE WHEN SALEMODE = 'B' THEN 'BOTH' "
        strSql += " WHEN SALEMODE = 'R' THEN 'RATE'"
        strSql += " WHEN SALEMODE = 'F' THEN 'FIXED'"
        strSql += " WHEN SALEMODE = 'D' THEN 'DIRECT'"
        strSql += " WHEN SALEMODE = 'M' THEN 'METAL RATE'"
        strSql += " WHEN SALEMODE = 'P' THEN 'PIECES'"
        strSql += " ELSE 'WEIGHT' END CALTYPE,T.ENTRYTYPE"
        strSql += " ,T.RRECDATE"
        strSql += " FROM " & cnAdminDb & "..ITEMTAG AS T "
        strSql += " LEFT OUTER JOIN " & cnAdminDb & "..PURITEMTAG AS P ON P.TAGSNO = T.SNO"
        strSql += " WHERE T.SNO = '" & updIssSno & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTagDetails)
        If Not dtTagDetails.Rows.Count > 0 Then Me.Dispose()
        With dtTagDetails.Rows(0)
            SNO = dtTagDetails.Rows(0).Item("LOTSNO").ToString
            strSql = " SELECT LOTNO,PCS,GRSWT,CPCS,CGRSWT,PCS-CPCS BALPCS,GRSWT-CGRSWT BALGRSWT,WMCTYPE "
            strSql += " FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = '" & SNO & "'"
            Dim dtLotDet As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtLotDet)
            If dtLotDet.Rows.Count > 0 Then
                With dtLotDet.Rows(0)
                    txtLotNo_Num_Man.Text = .Item("LOTNO").ToString
                    cmbCostCentre_Man.Text = dtTagDetails.Rows(0).Item("COSTCENTRE").ToString
                    lblPLot.Text = IIf(Val(.Item("PCS").ToString) <> 0, Val(.Item("PCS").ToString), "")
                    lblPCompled.Text = IIf(Val(.Item("CPCS").ToString) <> 0, Val(.Item("CPCS").ToString), "")
                    lblPBalance.Text = IIf(Val(.Item("BALPCS").ToString) <> 0, Val(.Item("BALPCS").ToString), "")

                    lblWLot.Text = IIf(Val(.Item("GRSWT").ToString) <> 0, Val(.Item("GRSWT").ToString).ToString("0.000"), "")
                    lblWCompleted.Text = IIf(Val(.Item("CGRSWT").ToString) <> 0, Val(.Item("CGRSWT")).ToString("0.000"), "")
                    lblWBalance.Text = IIf(Val(.Item("BALGRSWT").ToString) <> 0, Val(.Item("BALGRSWT")).ToString("0.000"), "")

                End With
            End If
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
            Dim _LooseItem As Boolean
            pFixedVa = IIf(objGPack.GetSqlValue("SELECT ISNULL(FIXEDVA,'N') FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'", , "N") = "N", False, True)
            Dim xItemid As Integer
            strSql = " SELECT ITEMID,METALID,STUDDEDSTONE,SIZESTOCK,MULTIMETAL,OTHCHARGE,CALTYPE,NOOFPIECE,PIECERATE,VALUEADDEDTYPE,GROSSNETWTDIFF"
            strSql += " ,STUDDED,STOCKTYPE,TableCode,MCASVAPER FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Item("ITEMNAME").ToString & "'"
            Dim dtItemDetail As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtItemDetail)
            If dtItemDetail.Rows.Count > 0 Then
                With dtItemDetail.Rows(0)
                    If .Item("STUDDED").ToString = "L" Or .Item("STUDDED").ToString = "B" Then _LooseItem = True
                    xItemid = Val(.Item("ITEMID").ToString)
                    METALID = .Item("METALID").ToString
                    studdedStone = .Item("STUDDEDSTONE").ToString
                    grossnetdiff = .Item("GROSSNETWTDIFF").ToString
                    sizeStock = .Item("SIZESTOCK").ToString
                    multiMetal = .Item("MULTIMETAL").ToString
                    OthCharge = .Item("OTHCHARGE").ToString
                    noOfPiece = Val(.Item("NOOFPIECE").ToString)
                    pieceRate = Val(.Item("PIECERATE").ToString)

                    If .Item("MCASVAPER").ToString = "Y" Then Label44.Text = "MC PERCENT" Else Label44.Text = "Max Mc Per Grm"
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
                    'If cmbSubItem_Man.Text <> "" Then
                    '    strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')"
                    'Else
                    '    strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"
                    'End If
                    ''calType = objGPack.GetSqlValue(strSql)
                    Dim mtablecode As String = dtTagDetails.Rows(0).Item("TABLECODE").ToString
                    If mtablecode = "" Then
                        strSql = " select TableCode from " & cnAdminDb & "..itemMast where ItemName = '" & dtTagDetails.Rows(0).Item("ITEMNAME").ToString & "' and TableCode <> ''"
                        mtablecode = objGPack.GetSqlValue(strSql)
                    End If
                    If cmbTableCode.Items.Contains(mtablecode) = False Then cmbTableCode.Items.Add(mtablecode)
                    cmbTableCode.Text = mtablecode
                    calType = .Item("CALTYPE").ToString
                    funcAssignTabControls()
                End With
            End If

            If _LooseItem Then
                CmbUnit.Text = .Item("WEIGHTUNIT").ToString
                CmbUnit.Visible = True
                txtGrossWt_Wet.Size = New Size(71, 21)
            Else
                CmbUnit.Visible = False
                txtGrossWt_Wet.Size = New Size(104, 21)
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

            txtLotNo_Num_Man.Enabled = False
            cmbItem_MAN.Text = .Item("ITEMNAME").ToString
            pFixedVa = IIf(objGPack.GetSqlValue("SELECT ISNULL(FIXEDVA,'N') FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'", , "N") = "N", False, True)
            cmbItem_MAN.Enabled = False
            cmbItemType_MAN.Text = .Item("ITEMTYPE").ToString
            cmbDesigner_MAN.Text = .Item("DESIGNER").ToString
            cmbCounter_MAN.Text = .Item("ITEMCOUNTER").ToString
            dtpRecieptDate.Value = .Item("RECDATE")
            If .Item("RRECDATE").ToString = "" Then
                dtpOldTagRecDate_OWN.Value = GetEntryDate(GetServerDate)
                chkOldTagRecdate.Checked = False
            Else
                chkOldTagRecdate.Checked = True
                dtpOldTagRecDate_OWN.Value = .Item("RRECDATE")
            End If
            txtPurity_Per.Text = .Item("PURITY").ToString
            txtSetTagno.Text = .Item("SETTAGNO").ToString 'SETTAGNO
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
            If cmbSubItem_Man.Text <> "" Then
                strSql = "SELECT SETITEM FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbSubItem_Man.Text & "' "
                strSql += " AND ITEMID IN (SELECT TOP 1 ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItem_MAN.Text & "')"
                If objGPack.GetSqlValue(strSql) = "Y" Then
                    txtSetTagno.Enabled = True
                    CmbSetItem.Text = "YES"
                End If
            Else
                If objGPack.GetSqlValue("SELECT SETITEM FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "' ") = "Y" Or CmbSetItem.Text = "YES" Then
                    txtSetTagno.Enabled = True
                    CmbSetItem.Text = "YES"
                End If
            End If
            If .Item("SETTAGNO").ToString <> "" Then CmbSetItem.Text = "YES" Else CmbSetItem.Text = "NO"
            txtTagNo__Man.Text = .Item("TAGNO").ToString
            txtTagNo__Man.Enabled = False
            txtPieces_Num_Man.Text = .Item("PCS").ToString
            '328
            noOfPiece = Val(.Item("PCS").ToString)
            '328
            txtGrossWt_Wet.Text = .Item("GRSWT").ToString

            ObjExtraWt.txtExtraWt_WET.Text = .Item("EXTRAWT").ToString
            ObjTagWt.txtTagWt_WET.Text = .Item("TAGWT").ToString
            ObjTagWt.txtCoverWt_WET.Text = .Item("COVERWT").ToString
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
            _LEntryType = .Item("ENTRYTYPE").ToString
            If .Item("ENTRYTYPE").ToString = "WO" And .Item("ORSNO").ToString = "" Then
                txtWorkOrderNo.Text = .Item("ORDREPNO").ToString
                txtWorkOrderNo.Enabled = False
            Else
                txtOrderNo.Text = .Item("ORDREPNO").ToString
                txtWorkOrderNo.Enabled = False
                txtWorkOrderNo.Clear()
            End If


            If cmbCalType.Enabled = True And NeedTag_CalcType Then
                calType = .Item("SALEMODE").ToString
            End If
            cmbCalType.Text = .Item("CALTYPE").ToString
            calType = .Item("SALEMODE").ToString
            If NeedTag_Hsncode Then
                txtHSN.Text = IIf(.Item("HSN").ToString <> "", .Item("HSN").ToString, "")
            Else
                txtHSN.Text = ""
            End If
            txtWDisc_Per.Text = .Item("WASTDISCPER").ToString 'WASTDISCPER
            txtRefVal_AMT.Text = IIf(Val(.Item("REFVALUE").ToString) <> 0, Format(Val(.Item("REFVALUE").ToString), "0.00"), "")
            If txtOrderNo.Text <> "" Then lblOrder.Visible = True : txtOrderNo.Visible = True
            If .Item("TAGFIXEDVA").ToString = "Y" Then chkFixedVa.Checked = True Else chkFixedVa.Checked = False
            If .Item("PCTFILE").ToString <> "" And defalutDestination <> "" Then
                picPath = .Item("PCTFILE").ToString
                Dim serverPath As String = Nothing
                Dim fileDestPath As String = defalutDestination & .Item("PCTFILE").ToString
                AutoImageSizer(fileDestPath, picModel, PictureBoxSizeMode.CenterImage)
                picModel.BringToFront()
                Dim Finfo As FileInfo
                Finfo = New FileInfo(fileDestPath)
                'Finfo.IsReadOnly = False
                picExtension = Finfo.Extension

                'If IO.File.Exists(fileDestPath) Then
                '    Dim Finfo As IO.FileInfo
                '    Finfo = New IO.FileInfo(fileDestPath)
                '    Finfo.IsReadOnly = False
                '    picExtension = Finfo.Extension.Replace(".", "")
                '    If IO.Directory.Exists(Finfo.Directory.FullName) = False Then
                '        picModel.Image = My.Resources.no_photo
                '    Else
                '        Dim fileStr As New IO.FileStream(fileDestPath, IO.FileMode.Open)
                '        picModel.Image = Image.FromStream(fileStr)
                '        fileStr.Close()
                '    End If
                'Else
                '    picModel.Image = My.Resources.no_photo
                'End If
            End If

            ''HALLDETAILS
            strSql = " SELECT 1 PCS,GRSWT,HM_BILLNO "
            strSql += " FROM " & cnAdminDb & "..ITEMTAGHALLMARK AS T"
            strSql += " WHERE TAGSNO = '" & updIssSno & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtHallmarkDetails)
            StyleHallmarkDet()
            If dtHallmarkDetails.Rows.Count > 0 Then
                NeedHallmark = True
            Else
                NeedHallmark = False
            End If

            ''MULTIMETAL
            strSql = " SELECT"
            strSql += " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = T.CATCODE)AS CATEGORY"
            If MetalBasedStone Then
                strSql += " ,GRSWT WEIGHT,NETWT,WASTPER WASTAGEPER,WAST WASTAGE,MCGRM MCPERGRM,MC MC,AMOUNT,NULL PURRATE"
            Else
                strSql += " ,GRSWT WEIGHT,WASTPER WASTAGEPER,WAST WASTAGE,MCGRM MCPERGRM,MC MC,AMOUNT,NULL PURRATE"
            End If
            strSql += " ,(SELECT PURWASTAGE FROM " & cnAdminDb & "..PURITEMTAGMETAL WHERE METSNO = T.SNO)AS PURWASTAGE"
            strSql += " ,(SELECT PURMC FROM " & cnAdminDb & "..PURITEMTAGMETAL WHERE METSNO = T.SNO)AS PURMC"
            strSql += " ,(SELECT PURVALUE FROM " & cnAdminDb & "..PURITEMTAGMETAL WHERE METSNO = T.SNO)AS PURAMOUNT"
            If MetalBasedStone Then
                strSql += " ,SNO STNSNO"
            End If
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
            strSql += " ,(SELECT CUTNAME FROM " & cnAdminDb & "..STNCUT WHERE CUTID = S.CUTID)AS CUT"
            strSql += " ,(SELECT COLORNAME FROM " & cnAdminDb & "..STNCOLOR WHERE COLORID = S.COLORID)AS COLOR"
            strSql += " ,(SELECT CLARITYNAME FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYID = S.CLARITYID)AS CLARITY"
            strSql += " ,(SELECT SHAPENAME FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPEID = S.SHAPEID)AS SHAPE"
            strSql += " ,(SELECT SIZENAME FROM " & cnAdminDb & "..STNSIZE WHERE STNSIZEID = S.SIZECODE)AS STNSIZE"
            strSql += " ,(SELECT SETTYPENAME FROM " & cnAdminDb & "..STNSETTYPE WHERE SETTYPEID = S.SETTYPEID)AS SETTYPE"
            strSql += " ,(SELECT GROUPNAME FROM " & cnAdminDb & "..STONEGROUP WHERE GROUPID = S.STNGRPID)AS STNGROUP"
            strSql += " ,STNGRPID "
            strSql += " ,S.HEIGHT,S.WIDTH"
            If MetalBasedStone Then
                strSql += " ,NULL MKEYNO,TAGMSNO MSNO"
            End If
            'strSql += " ,PURRATE,PURAMT PURVALUE"
            strSql += " FROM " & cnAdminDb & "..ITEMTAGSTONE S"
            strSql += " WHERE TAGSNO = '" & updIssSno & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtStoneDetails)
            dtStoneDetails.AcceptChanges()

            If dtMultiMetalDetails.Rows.Count > 0 And dtStoneDetails.Rows.Count > 0 And MetalBasedStone Then
                For Each Rmm As DataRow In dtMultiMetalDetails.Rows
                    Dim _tempMMkeyno As Integer = Val(Rmm("KEYNO").ToString)
                    For Each Rss As DataRow In dtStoneDetails.Rows
                        If Rss("MSNO").ToString = Rmm("STNSNO").ToString Then
                            Rss("MKEYNO") = _tempMMkeyno
                        End If
                    Next
                Next
                dtStoneDetails.AcceptChanges()
            End If

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
            ObjPurDetail.txtNarration.Text = .Item("DESCRIPT").ToString
            txtPurchaseValue_Amt.Text = ObjPurDetail.txtPurPurchaseVal_Amt.Text
            ObjPurDetail.txtPurLCostper_NUM.Text = IIf(Val(.Item("PURLCOSTPER").ToString) <> 0, .Item("PURLCOSTPER").ToString, Nothing)
            ObjPurDetail.txtPurLCost_NUM.Text = IIf(Val(.Item("PURLCOST").ToString) <> 0, .Item("PURLCOST").ToString, Nothing)
            ObjPurDetail.txtSaleRate_PER.Text = IIf(Val(.Item("PURSCOSTPER").ToString) <> 0, .Item("PURSCOSTPER").ToString, Nothing)
            ObjPurDetail.CalcPurchaseGrossValue()
            ObjPurDetail.CalcPurchaseValue()

            If MetalBasedStone = True Then
                lblMMweight.Text = "GRSWT"
                lblMMNetwt.Visible = True
                txtMMNetwt_Wet.Visible = True
            Else
                lblMMweight.Text = "Weight"
                lblMMNetwt.Visible = False
                txtMMNetwt_Wet.Visible = False
            End If

            If UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'TAGEDITB4DATE'", , "N")) = "N" Then
                dtpRecieptDate.Enabled = False
            End If
            If TAGEDITPCSWT = "N" Then
                txtPieces_Num_Man.ReadOnly = True
                txtGrossWt_Wet.ReadOnly = True
                txtNetWt_Wet.ReadOnly = True
            ElseIf TAGEDITPCSWT = "A" And Authorize Then
                txtPieces_Num_Man.ReadOnly = False
                txtGrossWt_Wet.ReadOnly = False
                txtNetWt_Wet.ReadOnly = False
            ElseIf TAGEDITPCSWT = "A" And Not Authorize Then
                txtPieces_Num_Man.ReadOnly = True
                txtGrossWt_Wet.ReadOnly = True
                txtNetWt_Wet.ReadOnly = True
            End If
            If dtItemDetail.Rows.Count > 0 Then
                Dim itemid As Integer = Val(dtItemDetail.Rows(0).Item("ITEMID").ToString)
                If FuncCheckTransactionDetails(itemid, txtTagNo__Man.Text) Then
                    txtPieces_Num_Man.ReadOnly = True
                    txtGrossWt_Wet.ReadOnly = True
                    txtNetWt_Wet.ReadOnly = True
                End If
            End If

            If TAGEDITDISABLE <> "" Then
                If TAGEDITDISABLE.Contains("SI") Then cmbSubItem_Man.Enabled = False
                If TAGEDITDISABLE.Contains("IT") Then cmbItemType_MAN.Enabled = False
                If TAGEDITDISABLE.Contains("TC") Then cmbTableCode.Enabled = False
                If TAGEDITDISABLE.Contains("SD") Then gridStone.Enabled = False
                If TAGEDITDISABLE.Contains("SV") Then txtSalValue_Amt_Man.ReadOnly = True : BlockSv = True
                If TAGEDITDISABLE.Contains("PV") Then txtPurchaseValue_Amt.Enabled = False
                If TAGEDITDISABLE.Contains("RI") Then txtRate_Amt.ReadOnly = True
                If TAGEDITDISABLE.Contains("WS") Then txtMaxWastage_Per.ReadOnly = True : txtMaxWastage_Wet.ReadOnly = True
                If TAGEDITDISABLE.Contains("MC") Then txtMaxMcPerGrm_Amt.ReadOnly = True : txtMaxMkCharge_Amt.ReadOnly = True
                If TAGEDITDISABLE.Contains("DG") Then cmbDesigner_MAN.Enabled = False
                If TAGEDITDISABLE.Contains("RT") Then txtMetalRate_Amt.ReadOnly = True
                If TAGEDITDISABLE.Contains("DT") Then dtpRecieptDate.Enabled = False
                If TAGEDITDISABLE.Contains("RDT") Or NeedOldTag_Recdate = False Then dtpOldTagRecDate_OWN.Enabled = False
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
            If _CheckOrderInfo = False And dtTagDetails.Rows(0).Item("ORDREPNO").ToString <> "0" And dtTagDetails.Rows(0).Item("ORDREPNO").ToString <> "" And .Item("ENTRYTYPE").ToString <> "WO" Then                ' 
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
                    TagStnGrpId = Val(dtTagDetails.Rows(0).Item("STNGRPID").ToString)
                    TagCutId = Val(dtTagDetails.Rows(0).Item("CUTID").ToString)
                    TagColorId = Val(dtTagDetails.Rows(0).Item("COLORID").ToString)
                    TagClarityId = Val(dtTagDetails.Rows(0).Item("CLARITYID").ToString)
                    TagShapeId = Val(dtTagDetails.Rows(0).Item("SHAPEID").ToString)
                    TagSetTypeId = Val(dtTagDetails.Rows(0).Item("SETTYPEID").ToString)
                    If ORDER_MULTI_MIMR Then
                        ObjDiamondDetails = New frmDiamondDetails
                        ObjDiamondDetails.cmbStnGrp.Text = objGPack.GetSqlValue(" SELECT GROUPNAME FROM " & cnAdminDb & "..STONEGROUP WHERE GROUPID = '" & TagStnGrpId & "'", "GROUPNAME", , tran)
                        ObjDiamondDetails.CmbCut.Text = objGPack.GetSqlValue(" SELECT CUTNAME FROM " & cnAdminDb & "..STNCUT WHERE CUTID = '" & TagCutId & "'", "CUTNAME", , tran)
                        ObjDiamondDetails.CmbColor.Text = objGPack.GetSqlValue(" SELECT COLORNAME FROM " & cnAdminDb & "..STNCOLOR WHERE COLORID = '" & TagColorId & "'", "COLORNAME", , tran)
                        ObjDiamondDetails.CmbClarity.Text = objGPack.GetSqlValue(" SELECT CLARITYNAME FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYID = '" & TagClarityId & "'", "CLARITYNAME", , tran)
                        ObjDiamondDetails.cmbShape.Text = objGPack.GetSqlValue(" SELECT SHAPENAME FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPEID = '" & TagShapeId & "'", "SHAPENAME", , tran)
                        ObjDiamondDetails.cmbSetType.Text = objGPack.GetSqlValue(" SELECT SETTYPENAME FROM " & cnAdminDb & "..STNSETTYPE WHERE SETTYPEID = '" & TagSetTypeId & "'", "SETTYPENAME", , tran)
                        ObjDiamondDetails.txtWidth_WET.Text = Val(dtTagDetails.Rows(0).Item("WIDTH").ToString)
                        ObjDiamondDetails.txtHeight_WET.Text = Val(dtTagDetails.Rows(0).Item("HEIGHT").ToString)
                    Else
                        ObjDiaDetails = New frm4C
                        ObjDiaDetails.CmbCut.Text = objGPack.GetSqlValue(" SELECT CUTNAME FROM " & cnAdminDb & "..STNCUT WHERE CUTID = '" & TagCutId & "'", "CUTNAME", , tran)
                        ObjDiaDetails.CmbColor.Text = objGPack.GetSqlValue(" SELECT COLORNAME FROM " & cnAdminDb & "..STNCOLOR WHERE COLORID = '" & TagColorId & "'", "COLORNAME", , tran)
                        ObjDiaDetails.CmbClarity.Text = objGPack.GetSqlValue(" SELECT CLARITYNAME FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYID = '" & TagClarityId & "'", "CLARITYNAME", , tran)
                        ObjDiaDetails.cmbShape.Text = objGPack.GetSqlValue(" SELECT SHAPENAME FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPEID = '" & TagShapeId & "'", "SHAPENAME", , tran)
                        ObjDiaDetails.cmbSetType.Text = objGPack.GetSqlValue(" SELECT SETTYPENAME FROM " & cnAdminDb & "..STNSETTYPE WHERE SETTYPEID = '" & TagSetTypeId & "'", "SETTYPENAME", , tran)
                        ObjDiaDetails.txtWidth_WET.Text = Val(dtTagDetails.Rows(0).Item("WIDTH").ToString)
                        ObjDiaDetails.txtHeight_WET.Text = Val(dtTagDetails.Rows(0).Item("HEIGHT").ToString)
                    End If


                End If
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
            If TAGDETDISABLE <> "" Then
                If TAGDETDISABLE.Contains("D") And cmbDesigner_MAN.Text <> "" Then cmbDesigner_MAN.Enabled = False
                If TAGDETDISABLE.Contains("C") And cmbCounter_MAN.Text <> "" Then cmbCounter_MAN.Enabled = False
                If TAGDETDISABLE.Contains("I") And cmbItem_MAN.Text <> "" Then cmbItem_MAN.Enabled = False
                If TAGDETDISABLE.Contains("S") And cmbSubItem_Man.Text <> "" Then cmbSubItem_Man.Enabled = False
                If TAGDETDISABLE.Contains("T") And cmbItemType_MAN.Text <> "" Then cmbItemType_MAN.Enabled = False
            End If
            If LOCK_TAGEDIT_VA <> "" Then
                If LOCK_TAGEDIT_VA.Contains("W") And txtMaxWastage_Per.Text <> "" Then txtMaxWastage_Per.Enabled = False
                If LOCK_TAGEDIT_VA.Contains("W") And txtMaxWastage_Wet.Text <> "" Then txtMaxWastage_Wet.Enabled = False
                If LOCK_TAGEDIT_VA.Contains("M") And txtMaxMcPerGrm_Amt.Text <> "" Then txtMaxMcPerGrm_Amt.Enabled = False
                If LOCK_TAGEDIT_VA.Contains("M") And txtMaxMkCharge_Amt.Text <> "" Then txtMaxMkCharge_Amt.Enabled = False
            End If
        End With
    End Sub
    Function FuncCheckTransactionDetails(ByVal itemid As Integer, ByVal TagNo As String, Optional ShowMaxDate As Boolean = False) As Boolean
        If tagEdit = False Then Return False : Exit Function
        If STOCK_VALIDATION = False Then Return False : Exit Function
        strSql = "SELECT MAX(TRANDATE) TRANDATE  FROM ("
        strSql += vbCrLf + " SELECT TRANDATE FROM " & cnStockDb & "..ISSUE WHERE ITEMID = '" & itemid & "' AND TAGNO = '" & TagNo & "' AND ISNULL(CANCEL,'') = ''  AND TRANTYPE NOT IN ('IIN','RIN')"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT TRANDATE FROM " & cnStockDb & "..RECEIPT WHERE ITEMID = '" & itemid & "' AND TAGNO = '" & TagNo & "' AND ISNULL(CANCEL,'') = ''  AND TRANTYPE NOT IN ('IIN','RIN'))X"
        da = New OleDbDataAdapter(strSql, cn)
        Dim dt As New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            If ShowMaxDate Then
                MsgBox("Last Transaction Made On " & dt.Rows(0).Item("TRANDATE").ToString)
            End If
            Return True : Exit Function
        End If
        Return False
    End Function
    Private Sub funcDiaDetails()
        If PacketNoEnable = "N" Then stHide()
        strSql = "SELECT COUNT(*) FROM " & cnAdminDb & "..ITEMMAST WHERE MAINTAIN4C='Y'"
        If Val(objGPack.GetSqlValue(strSql)) > 0 Then
            strSql = "SELECT TOP 1 VIEW4C FROM " & cnAdminDb & "..ITEMMAST WHERE VIEW4C <> '' ORDER BY LEN(VIEW4C) DESC"
            Dim View4C As String = objGPack.GetSqlValue(strSql)
            If View4C.Contains("SH") Then CmbStShape.Enabled = True Else CmbStShape.Enabled = False
            If View4C.Contains("CO") Then CmbStColor.Enabled = True Else CmbStColor.Enabled = False
            If View4C.Contains("CL") Then CmbStClarity.Enabled = True Else CmbStClarity.Enabled = False
            If View4C.Contains("SI") Then CmbStSize.Enabled = True Else CmbStSize.Enabled = False
            _4C = True


            Dim txtWid As Integer = 0
            If Not View4C.Contains("SI") Then txtWid += (CmbStSize.Width) : CmbStSize.Visible = False
            If Not View4C.Contains("CL") Then txtWid += (CmbStClarity.Width) : CmbStClarity.Visible = False
            If Not View4C.Contains("CO") Then txtWid += (CmbStColor.Width) : CmbStColor.Visible = False
            If Not View4C.Contains("SH") Then txtWid += (CmbStShape.Width) : CmbStShape.Visible = False
            If Not STUD_STNWTPER Then txtWid += (lblWPer.Width)
            If STUD_STNWTPER Then lblWPer.Left -= txtWid
            lblPcs.Left -= txtWid
            lblWt.Left -= txtWid
            lblUnit.Left -= txtWid
            lblCalcType.Left -= txtWid
            lblRate.Left -= txtWid
            lblAmt.Left -= txtWid
            If STUD_STNWTPER Then txtStWPer_AMT.Left -= txtWid
            txtStPcs_Num.Left -= txtWid
            txtStWeight.Left -= txtWid
            cmbStCalc.Left -= txtWid
            cmbStUnit.Left -= txtWid
            txtStRate_Amt.Left -= txtWid
            txtStAmount_Amt.Left -= txtWid
            pnlStoneGrid.Size = New Drawing.Size(pnlStoneGrid.Width - txtWid, pnlStoneGrid.Height)


        Else
            CmbStShape.Visible = False : lblShape.Visible = False
            CmbStColor.Visible = False : lblColor.Visible = False
            CmbStClarity.Visible = False : lblClarity.Visible = False
            CmbStSize.Visible = False : lblSize.Visible = False
            Dim txtWid As Integer = (CmbStSize.Width) _
                   + (CmbStClarity.Width) _
                   + (CmbStColor.Width) _
                   + (CmbStShape.Width)
            If Not STUD_STNWTPER Then txtWid += (lblWPer.Width)
            If STUD_STNWTPER Then lblWPer.Left -= txtWid
            lblPcs.Left -= txtWid
            lblWt.Left -= txtWid
            lblUnit.Left -= txtWid
            lblCalcType.Left -= txtWid
            lblRate.Left -= txtWid
            lblAmt.Left -= txtWid
            If STUD_STNWTPER Then txtStWPer_AMT.Left -= txtWid
            txtStPcs_Num.Left -= txtWid
            txtStWeight.Left -= txtWid
            cmbStCalc.Left -= txtWid
            cmbStUnit.Left -= txtWid
            txtStRate_Amt.Left -= txtWid
            txtStAmount_Amt.Left -= txtWid
            pnlStoneGrid.Size = New Drawing.Size(pnlStoneGrid.Width - txtWid, pnlStoneGrid.Height)
        End If
    End Sub

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
        Dim Isdeductstud As Boolean = False
        Dim dirlesswt As Double
        If TabControl1.TabPages.Contains(tabStone) Then
            For cnt As Integer = 0 To gridStone.RowCount - 1
                With gridStone.Rows(cnt)
                    If StudWtDedut = "I" Then
                        If .Cells("SUBITEM").Value.ToString <> "" Then
                            strSql = " SELECT STUDDEDUCT FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & .Cells("SUBITEM").Value.ToString & "'"
                            strSql += " AND ITEMID IN (SELECT TOP 1 ITEMID  FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEM").Value.ToString & "')"
                        Else
                            strSql = " SELECT STUDDEDUCT  FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Cells("ITEM").Value.ToString & "'"
                        End If
                        Isdeductstud = IIf(objGPack.GetSqlValue(strSql).ToString.ToUpper = "Y", True, False)
                    End If
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
                            If Isdeductstud Then dirlesswt += Val(.Cells("weight").Value.ToString)
                        Case "C"
                            If .Cells("METALID").Value.ToString = "S" Then
                                stoCaratWt += Val(.Cells("WEIGHT").Value.ToString)
                            ElseIf .Cells("METALID").Value.ToString = "P" Then
                                preCaratWt += Val(.Cells("WEIGHT").Value.ToString)
                            ElseIf .Cells("METALID").Value.ToString = "D" Then
                                diaCaratWt += Val(.Cells("WEIGHT").Value.ToString)
                            End If
                            If Isdeductstud Then dirlesswt += (Val(.Cells("weight").Value.ToString) / 5)
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

        Dim lessWt As Double = Nothing
        If StudWtDedut.Contains("D") Then
            lessWt += (diaCaratWt / 5) + diaGramWt
        End If
        If StudWtDedut.Contains("S") Then
            lessWt += (stoCaratWt / 5) + stoGramWt
        End If
        If StudWtDedut.Contains("P") Then
            lessWt += (preCaratWt / 5) + preGramWt
        End If
        If StudWtDedut.Contains("N") Then
            lessWt = 0
        End If
        lessWt += dirlesswt
        'lessWt = (diaCaratWt + stoCaratWt + preCaratWt) / 5 + (diaGramWt + stoGramWt + preGramWt)
        txtLessWt_Wet.Text = IIf(lessWt <> 0, Format(lessWt, "0.000"), "")
    End Sub

    Private Sub CalcNetWt()
        If TAGEDITPCSWT = "A" Then Exit Sub
        Dim wt As Double = Nothing
        wt = Val(txtGrossWt_Wet.Text) - Val(txtLessWt_Wet.Text)
        strSql = "SELECT ISNULL(NETWTPER,0) NETWTPER FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"
        Dim mnetwtper As Double = Val(objGPack.GetSqlValue(strSql).ToString)
        If mnetwtper <> 0 Then wt = wt * (mnetwtper / 100)
        txtNetWt_Wet.Text = IIf(wt <> 0, Format(wt, "0.000"), "")
    End Sub

    Private Sub CalcFinalTotal(Optional ByVal salevalueblock As Boolean = False)
        CalcNetWt()
        CalcStoneTotals()
        CalcMultiMetalTotal()
        CalcMiscTotalAmount()
        '        Calcsubitems()
        If issOrder Then
            If chkFixedVa.Visible And (Val(txtMaxMkCharge_Org.Text) <> 0 Or Val(txtMaxWastage_Org.Text) <> 0) Then
                If Val(txtMaxMkCharge_Amt.Text) <> Val(txtMaxMkCharge_Org.Text) Or Val(txtMaxWastage_Wet.Text) <> Val(txtMaxWastage_Org.Text) Then chkFixedVa.Checked = True Else chkFixedVa.Checked = False
            End If
        End If
        If Not salevalueblock Then CalcSaleValue()
        'CalcPurchaseValue()
    End Sub

    Private Sub Calcsubitems()
        Dim dt As New DataTable
        strSql = "SELECT ISNULL(STYLENO,'')STYLENO FROM " & cnAdminDb & "..ITEMLOT WHERE LOTNO =  '" & txtLotNo_Num_Man.Text & "' AND SNO = '" & SNO & "'"
        Stylecode = GetSqlValue(cn, strSql)
        strSql = " SELECT * FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            With dt.Rows(0)
                pieceRate = Val(dt.Rows(0).Item("PieceRate").ToString)
                lotPurRate = Val(dt.Rows(0).Item("PIECERATE_PUR").ToString)
                If pieceRate <> 0 Then txtRate_Amt.Text = pieceRate.ToString
                If tagEdit = False Then txtStyleCode.Text = Stylecode ' "" & .Item("STYLECODE").ToString
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
        If TAGEDITDISABLE.Contains("SV") And tagEdit Then Exit Sub
        Dim amt As Double = Nothing
        Dim Itemid As String = GetSqlValue(cn, "SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "' ")
        Dim Subitemid As String = GetSqlValue(cn, "SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "' ")
        Dim Itemtypeid As String = GetSqlValue(cn, "SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType_MAN.Text & "' ")
        Dim ValueAddedType As String = GetSqlValue(cn, "SELECT VALUEADDEDTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "' ")
        Dim DesignerId As String = GetSqlValue(cn, "SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesigner_MAN.Text & "' ")
        Dim TCostid As String = GetSqlValue(cn, "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_Man.Text & "'")
        If calType = "F" And tagEdit = True Then Exit Sub
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
                If dtMultiMetalDetails.Rows.Count = 0 Then
                    If VALUECALC_GRNETOPT Then
                        If _VALUECALCON_ITEM_GRS Then wt = Val(txtGrossWt_Wet.Text) Else wt = Val(txtNetWt_Wet.Text)
                    End If
                    amt = ((wt + Val(txtMaxWastage_Wet.Text)) * rate) _
                    + Val(txtMaxMkCharge_Amt.Text) _
                    + Val(txtMultiAmt.Text) _
                    + Val(lblDiaAmount.Text) + Val(lblStnAmount.Text) + Val(lblPreAmount.Text) _
                    + Val(txtMiscAmt.Text)
                End If
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
        If calType = "F" And FIXEDITEM_SALEPER Then
            amt = Format(Math.Round(amt + (amt * SalePer / 100), 2), "0.00")
        End If
        If calType = "F" Then
            strSql = " DECLARE @WT FLOAT,@DIAPCS INT"
            If cmbCalcMode.SelectedIndex = 0 Then ''GRS WT
                strSql += vbCrLf + " SET @WT = " & Val(txtGrossWt_Wet.Text) & ""
            Else ''NET WT
                strSql += vbCrLf + " SET @WT = " & Val(txtNetWt_Wet.Text) & ""
            End If
            strSql += vbCrLf + " SELECT SALVALUE"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..WMCTABLE "
            strSql += vbCrLf + " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')"
            strSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
            If Subitemid IsNot Nothing And CheckValueWmcTable(ValueAddedType, Itemid, Subitemid) Then
                strSql += vbCrLf + " AND SUBITEMID = " & Subitemid
            Else
                strSql += vbCrLf + " AND ISNULL(SUBITEMID,0) = 0"
            End If
            If DesignerId IsNot Nothing And ValueAddedType = "D" And CheckValueWmcTable(ValueAddedType, Itemid, Subitemid, DesignerId) Then
                strSql += vbCrLf + " AND DESIGNERID = " & DesignerId
            Else
                strSql += vbCrLf + " AND DESIGNERID = 0"
            End If
            If Itemtypeid IsNot Nothing And CheckValueWmcTable(ValueAddedType, Itemid, Subitemid, DesignerId, Itemtypeid) Then
                strSql += vbCrLf + " AND ITEMTYPE = " & Itemtypeid
            Else
                strSql += vbCrLf + " AND ITEMTYPE = 0 "
            End If
            'strSql += vbCrLf + " AND SUBITEMID = (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "')"
            strSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
            If cmbCostCentre_Man.Text <> "" Then
                strSql += vbCrLf + " AND COSTID = '" & TCostid & "'"
            End If
            Dim _Amount As Double = Val(objGPack.GetSqlValue(strSql, "SALVALUE", 0).ToString)
            If _Amount <> 0 Then
                amt = _Amount
                txtRate_Amt.Text = IIf(Math.Round(amt) <> 0, Format(Math.Round(amt), "0.00"), "")

            Else
                If RATE_FROM_WMCTABLE = True Then
                    txtRate_Amt.Text = ""
                End If
            End If
        End If
        amt = Math.Round(amt)
        txtSalValue_Amt_Man.Text = IIf(amt <> 0, Format(SALEVALUE_ROUND(amt), "0.00"), "")
        If SALEVALUEPLUS <> 0 Then txtSalValue_Amt_Man.Text = Val(txtSalValue_Amt_Man.Text) * SALEVALUEPLUS
        ObjPurDetail.CalcPurchaseValue()
        txtPurchaseValue_Amt.Text = ObjPurDetail.txtPurPurchaseVal_Amt.Text
        If PUR_LANDCOST And calType = "F" Then
            txtSalValue_Amt_Man.Text = Format(Val(txtPurchaseValue_Amt.Text) * Val(ObjPurDetail.txtSaleRate_PER.Text), "0.00")
        End If
    End Sub

#End Region
    Private Function GetRateFromWmcTable(ByVal Purrate As Boolean) As Double
        Dim Itemid As String = GetSqlValue(cn, "SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "' ")
        Dim Subitemid As String = GetSqlValue(cn, "SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "' ")
        Dim Itemtypeid As String = GetSqlValue(cn, "SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType_MAN.Text & "' ")
        Dim ValueAddedType As String = GetSqlValue(cn, "SELECT VALUEADDEDTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "' ")
        Dim DesignerId As String = GetSqlValue(cn, "SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesigner_MAN.Text & "' ")
        Dim TCostid As String = GetSqlValue(cn, "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_Man.Text & "'")
        Dim rate As Double = Nothing
        If calType = "F" Or calType = "R" Then
            strSql = "SELECT TOP 1 " & IIf(Purrate = True, "RATE_PUR", "RATE") & " FROM " & cnAdminDb & "..WMCTABLE WHERE ITEMID = " & Itemid & ""
            If Subitemid IsNot Nothing And CheckValueWmcTable(ValueAddedType, Itemid, Subitemid) Then
                strSql += vbCrLf + " AND SUBITEMID = " & Subitemid
            Else
                strSql += vbCrLf + " AND ISNULL(SUBITEMID,0) = 0"
            End If
            If DesignerId IsNot Nothing And CheckValueWmcTable(ValueAddedType, Itemid, Subitemid, DesignerId) Then
                strSql += vbCrLf + " AND DESIGNERID = " & DesignerId
            Else
                strSql += vbCrLf + " AND DESIGNERID = 0"
            End If
            If Itemtypeid IsNot Nothing And CheckValueWmcTable(ValueAddedType, Itemid, Subitemid, DesignerId, Itemtypeid) Then
                strSql += vbCrLf + " AND ITEMTYPE = " & Itemtypeid
            Else
                strSql += vbCrLf + " AND ITEMTYPE = 0 "
            End If
            If CheckValueWmcTable(ValueAddedType, Itemid, Subitemid, DesignerId, Itemtypeid, Val(txtGrossWt_Wet.Text)) Then
                strSql += vbCrLf + " AND " & Val(txtGrossWt_Wet.Text) & " BETWEEN FROMWEIGHT AND TOWEIGHT"
            End If
            If cmbCostCentre_Man.Text <> "" Then
                strSql += vbCrLf + " AND COSTID = '" & TCostid & "'"
            End If
            Return GetSqlValue(cn, strSql)
        Else
            Return 0
        End If
    End Function
    Private Function CheckValueWmcTable(ByVal ValueAddedType As String, ByVal Itemid As String _
            , Optional Subitemid As String = Nothing, Optional DesignerId As String = Nothing _
            , Optional ItemTypeId As String = Nothing, Optional Weight As Double = 0
            ) As Boolean
        Dim StrSqlTemp As String
        StrSqlTemp = "SELECT 1 FROM " & cnAdminDb & "..WMCTABLE WHERE ITEMID = '" & Itemid & "'"
        If Subitemid IsNot Nothing Then
            StrSqlTemp += vbCrLf + " AND SUBITEMID = " & Subitemid & ""
        End If
        If ValueAddedType = "D" And DesignerId IsNot Nothing Then
            StrSqlTemp += vbCrLf + " AND DESIGNERID = " & DesignerId & ""
        End If
        If ItemTypeId IsNot Nothing Then
            StrSqlTemp += vbCrLf + " AND ITEMTYPE = " & ItemTypeId & ""
        End If
        If Weight <> 0 Then
            StrSqlTemp += vbCrLf + " AND " & Val(Weight) & " BETWEEN FROMWEIGHT AND TOWEIGHT"
        End If
        If GetSqlValue(cn, StrSqlTemp) = 0 Then Return False : Exit Function
        Return True
    End Function
    Private Function GetMetalRate() As Double
        Dim purityId As String = Nothing
        ''objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = " & saItemTypeId & " AND RATEGET = 'Y'", , )
        Dim rate As Double = Nothing
        If RATE_FROM_WMCTABLE And (calType = "F") Then
            rate = GetRateFromWmcTable(False)
            If rate <> 0 Then Return rate : Exit Function
        End If
        If cmbItemType_MAN.Text <> "" Then
            purityId = objGPack.GetSqlValue("SELECT PURITYID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType_MAN.Text & "' AND RATEGET = 'Y' AND SOFTMODULE = 'S'", , )
        End If
        If Not Trim(purityId).Length > 0 Then
            purityId = objGPack.GetSqlValue("SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')")
        End If
        If purityId = "" Then Return 0
        Dim metalId As String = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = '" & purityId & "'")


        strSql = " SELECT TOP 1 SRATE FROM " & cnAdminDb & "..RATEMAST AS M"
        strSql += " WHERE RDATE = '" & dtpRecieptDate.Value & "'"
        strSql += " AND RATEGROUP = (SELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = M.RDATE)"
        strSql += " AND METALID = '" & metalId & "'"
        strSql += " AND PURITY = (SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = '" & purityId & "')"
        If RATE_BRANCHWISE Then
            strSql += " AND COSTID='" & cnCostId & "'"
        End If
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
        If GetAdmindbSoftValue("SYNC-TO", , tran) <> "" Then
            If GetAdmindbSoftValue("BRANCHTAG", "Y", tran) = "N" Then
                MsgBox("Taged entry cannot allow at location", MsgBoxStyle.Information)
                txtLotNo_Num_Man.Enabled = False
                If Me.Focused = True Then Me.Close()
                ItemLock = False
                Return 0
                Exit Function
            End If
        End If
        DesignerStone = False
        issOrder = False
        setItem = False
        NeedHallmark = False
        RepairLot = False
        chkFixedVa.Checked = False
        txtMaxMkCharge_Org.Text = ""
        txtMaxWastage_Org.Text = ""
        Ratevaluezero = False
        ObjRsUs.txtIndRs_Amt.Clear()
        ObjRsUs.txtUSDollar_Amt.Clear()
        ObjOrderTagInfo = New TagOrderInfo
        ObjPurDetail = New TagPurchaseDetailEntry
        ObjMinValue = New TagMinValues
        ObjExtraWt = New frmExtaWt
        ObjTagWt = New frmTagWt
        AddHandler ObjMinValue.txtMinWastage_Per.KeyPress, AddressOf ObjMinValues_txtMinWastage_Per_KeyPress
        AddHandler ObjMinValue.txtMinWastage_Per.TextChanged, AddressOf ObjMinValues_txtMinWastage_Per_TextChanged
        AddHandler ObjMinValue.txtMinMcPerGram_Amt.KeyPress, AddressOf ObjMinValues_txtMinMcPerGram_Amt_KeyPress
        AddHandler ObjMinValue.txtMinMcPerGram_Amt.TextChanged, AddressOf ObjMinValues_txtMinMcPerGram_Amt_TextChanged
        AddHandler ObjMinValue.txtMinWastage_Wet.GotFocus, AddressOf ObjMinValue_txtMinWastage_Wet_GotFocus
        AddHandler ObjMinValue.txtMinWastage_Wet.KeyPress, AddressOf ObjMinValue_txtMinWastage_Wet_KeyPress
        AddHandler ObjMinValue.txtMinMkCharge_Amt.GotFocus, AddressOf ObjMinValue_txtMinMkCharge_Amt_GotFocus
        AddHandler ObjMinValue.txtMinMkCharge_Amt.KeyPress, AddressOf ObjMinValue_txtMinMkCharge_Amt_KeyPress
        AddHandler ObjMinValue.txtMinWastage_Wet.TextChanged, AddressOf ObjMinValue_txtMinWastage_Wet_TextChanged

        AddHandler objSetItem.txtSetWastagePer_Per.TextChanged, AddressOf objSetItem_txtSetWastagePer_Per_TextChanged
        AddHandler objSetItem.txtSetWastage_WET.TextChanged, AddressOf objSetItem_txtSetWastage_WET_TextChanged
        AddHandler objSetItem.txtSetMcPerGrm_AMT.TextChanged, AddressOf objSetItem_txtSetMcPerGrm_AMT_TextChanged
        AddHandler objSetItem.txtSetMc_AMT.TextChanged, AddressOf objSetItem_txtSetMc_AMT_TextChanged


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
        _LEntryType = ""
        lblOrder.Visible = False
        txtOrderNo.Visible = False
        txtOrderNo.Text = ""
        txtSetTagno.Enabled = False
        SubItemPic = False
        mfromItemid = 0
        txtMaxWastage_Per.Text = ""
        txtMaxWastage_Wet.Text = ""
        txtMaxMcPerGrm_Amt.Text = ""
        txtMaxMkCharge_Amt.Text = ""
        txtWDisc_Per.Text = ""
        lblItemChange.Visible = False
        If TagDlrSealGet Then txt_seal.Enabled = True Else txt_seal.Enabled = False
        Studded_Loose = ""
        dtMultiMetalDetails.Rows.Clear()
        dtStoneDetails.Rows.Clear()
        dtMiscDetails.Rows.Clear()
        dtHallmarkDetails.Rows.Clear()
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
        lblStkType.Text = ""
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
        ''Net Weight
        lblNWLot.Text = ""
        lblNWCompleted.Text = ""
        lblNWBalance.Text = ""
        txtSetTagno.Text = ""
        txtMiscAmt.Text = ""
        txtMultiWt.Text = ""
        txtMultiAmt.Text = ""
        txtStyleCode.Clear()
        btnSave.Enabled = True
        TChkbStk = True
        AddStockEntry = True

        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'ATTACHIMAGE'"
        If UCase(objGPack.GetSqlValue(strSql, , , tran)) = "Y" Then
            btnAttachImage.Enabled = True
        Else
            btnAttachImage.Enabled = False
        End If

        StrTagNo = ""
        StrSrtName = ""
        StrCaption = ""
        RangeSno = ""
        txtHmBillNo.Text = ""

        txtLotNo_Num_Man.Enabled = True

        funcCostCentre()

        strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ISNULL(ACTIVE,'') <> 'N' ORDER BY DESIGNERNAME"
        objGPack.FillCombo(strSql, cmbDesigner_MAN, , False)

        If UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'ITEMCOUNTER'")) = "Y" Then
            strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ISNULL(ACTIVE,'') <> 'N' ORDER BY DISPLAYORDER,ITEMCTRNAME"
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
        dtpOldTagRecDate_OWN.Enabled = True
        dtpOldTagRecDate_OWN.Value = GetEntryDate(GetServerDate)
        dtpOldTagRecDate_OWN.Enabled = False
        chkOldTagRecdate.Checked = False
        If ACC_STUDITEM_POPUP Then
            cmbStItem.Visible = False
            CmbStSubItem.Visible = False
            cmbStItem.Enabled = False
            CmbStSubItem.Enabled = False
            txtStItem.Enabled = True
            txtStSubItem.Enabled = True
            txtStItem.Visible = True
            txtStSubItem.Visible = True
        Else
            LoadItem(cmbStItem)
            LoadSubItem(CmbStSubItem)
            cmbStItem.Visible = True
            CmbStSubItem.Visible = True
            cmbStItem.Enabled = True
            CmbStSubItem.Enabled = True
            txtStItem.Enabled = False
            txtStSubItem.Enabled = False
            txtStItem.Visible = False
            txtStSubItem.Visible = False
        End If
        LoadDiaDetails()

        tagwastEditotp = False
        txtPurchaseValue_Amt_Hide.Visible = False

        txtWorkOrderNo.Clear()
        txtWorkOrderNo.Enabled = False

        CmbSetItem.Items.Clear()
        CmbSetItem.Items.Add("YES")
        CmbSetItem.Items.Add("NO")
        CmbSetItem.Text = "NO"

        cmbCalType.Items.Clear()
        cmbCalType.Items.Add("WEIGHT")
        cmbCalType.Items.Add("RATE")
        cmbCalType.Items.Add("BOTH")
        cmbCalType.Items.Add("FIXED")
        cmbCalType.Items.Add("METAL RATE")
        cmbCalType.Items.Add("PIECES")
        cmbCalType.Text = "WEIGHT"

        If NeedTag_CalcType Then
            cmbCalType.Enabled = True
        Else
            cmbCalType.Enabled = False
        End If
        If NeedTag_SetItem Then
            CmbSetItem.Enabled = True
        Else
            CmbSetItem.Enabled = False
        End If

        If TAGWOLOT Then
            cmbItem_MAN.Select()
        Else
            txtLotNo_Num_Man.Select()
        End If
        Return 1
    End Function

    Private Sub LoadItem(ByVal Cmb As ComboBox)
        strSql = " SELECT"
        strSql += " ITEMNAME AS ITEMNAME"
        strSql += " FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ISNULL(STUDDED,'') IN('S','B') AND ACTIVE = 'Y'"
        strSql += GetItemQryFilteration()
        objGPack.FillCombo(strSql, Cmb, True, False)
        Cmb.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        Cmb.AutoCompleteSource = AutoCompleteSource.ListItems
    End Sub

    Private Sub LoadSubItem(ByVal Cmb As ComboBox)
        If cmbStItem.Text = "" Then Exit Sub
        Dim iId As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbStItem.Text & "'"))
        strSql = GetSubItemQry(New String() {"SUBITEMNAME SUBITEM"}, iId)
        objGPack.FillCombo(strSql, Cmb, True, False)
        If Cmb.Items.Count = 0 Then
            Cmb.Enabled = False
            Me.SelectNextControl(cmbStItem, True, True, True, True)
        Else
            Cmb.Enabled = True
            Cmb.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            Cmb.AutoCompleteSource = AutoCompleteSource.ListItems
        End If
    End Sub

    Private Sub LoadDiaDetails()
        strSql = " SELECT COLORNAME FROM " & cnAdminDb & "..STNCOLOR WHERE ISNULL(ACTIVE,'Y')<>'N' ORDER BY DISPORDER"
        objGPack.FillCombo(strSql, CmbStColor, True, False)
        strSql = " SELECT CLARITYNAME FROM " & cnAdminDb & "..STNCLARITY WHERE ISNULL(ACTIVE,'Y')<>'N' ORDER BY DISPORDER"
        objGPack.FillCombo(strSql, CmbStClarity, True, False)
        strSql = " SELECT SHAPENAME FROM " & cnAdminDb & "..STNSHAPE WHERE ISNULL(ACTIVE,'Y')<>'N' ORDER BY DISPORDER"
        objGPack.FillCombo(strSql, CmbStShape, True, False)
        strSql = " SELECT SIZENAME FROM " & cnAdminDb & "..STNSIZE WHERE ISNULL(ACTIVE,'Y')<>'N' ORDER BY DISPORDER"
        objGPack.FillCombo(strSql, CmbStSize, True, False)
        TagStnGrpId = 0
        TagCutId = 0
        TagColorId = 0
        TagClarityId = 0
        TagShapeId = 0
        TagSetTypeId = 0
    End Sub

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
        ObjTagWt = New frmTagWt
        Studded_Loose = ""
        dtStoneDetails.Rows.Clear()
        dtMiscDetails.Rows.Clear()
        dtMultiMetalDetails.Rows.Clear()
        dtHallmarkDetails.Rows.Clear()
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
        chkFixedVa.Checked = False
        txtMaxMkCharge_Org.Text = ""
        txtMaxWastage_Org.Text = ""
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
        txtStWPer_AMT.Clear()
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
        If REPEAT_TAGPCS = False Then txtPieces_Num_Man.Clear()
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

        If COPYIMAGEFROMCATALOGPATH = False Then txtStyleCode.Clear()
        If COPYIMAGEFROMCATALOGPATH Then
            strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'ATTACHIMAGE'"
            If UCase(objGPack.GetSqlValue(strSql, , , tran)) = "Y" Then
                btnAttachImage.Enabled = True
            Else
                btnAttachImage.Enabled = False
            End If
        End If

        txtSalValue_Amt_Man.Clear()
        txtRefVal_AMT.Clear()
        txtRfId.Clear()
        txtHmBillNo.Clear()
        txtWorkOrderNo.Clear()
        dtpOldTagRecDate_OWN.Enabled = True
        dtpOldTagRecDate_OWN.Value = GetEntryDate(GetServerDate)
        dtpOldTagRecDate_OWN.Enabled = False
        chkOldTagRecdate.Checked = False
        If Not LOTNARRATION And Not ITEMTAGNARRATION Then txtNarration.Clear()
        If flagDeviceMode = True Then picCapture.Visible = True
        'gridPurMisc.DataSource = Nothing       
        'gridPurStone.DataSource = Nothing
        'gridPurMisc.DataSource = Nothing
        If TAGENTRY_FOCUS = "P" Then
            txtTagNo__Man.Focus() : Exit Function
        ElseIf TAGENTRY_FOCUS = "S" Then
            If cmbSubItem_Man.Enabled = False Then txtTagNo__Man.Focus() : Exit Function
            cmbSubItem_Man.Focus() : Exit Function
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

    Function funcAdd() As Integer
        Dim RowFiter() As DataRow = Nothing
        Dim TagSno As String = Nothing
        Dim TagNo As String = Nothing
        Dim COSTID As String = Nothing
        Dim itemId As Integer = Nothing
        Dim subitemId As Integer = Nothing
        Dim sizeId As Integer = Nothing
        Dim itemCtrId As Integer = Nothing
        Dim designerId As Integer = Nothing
        Dim tagVal As Long = 0
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
            Dim XToCostid As String = GetAdmindbSoftValue("SYNC-TO", "")
            Dim XBranchtag As Boolean = IIf(GetAdmindbSoftValue("BRANCHTAG", "") = "Y", True, False)

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
            Dim randomuniqtagno = GetAdmindbSoftValue("RANDOM_UNIQUE_TAG", "N").ToString
            tran = Nothing
            tran = cn.BeginTransaction(IsolationLevel.Serializable)

            ''Find TagSno
GETNTAGSNO:
            'TagSno = GetNewSno(TranSnoType.ITEMTAGCODE, tran, "GET_TRANSNO_ADMIN")
            'TagSno = GetNewSno(TranSnoType.ITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") 'Same Line Below comment on 19-jan-2018
TagDupGen:
            If Mid(randomuniqtagno, 1, 1) = "Y" Then
                txtTagNo__Man.Text = objTag.GetTagNo(dtpRecieptDate.Value.ToString("yyyy-MM-dd"), cmbItem_MAN.Text, SNO, tran)
                GoTo Continuetagging
            End If
            If MANUAL_TAGNO = False Then
                If GetSoftValue("TAGNOFROM") = "I" Then
                    strSql = "SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAG"
                    strSql += " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "') "
                    strSql += " AND TAGNO = '" & txtTagNo__Man.Text & "'"
                    If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
                        txtTagNo__Man.Text = objTag.GetTagNo(dtpRecieptDate.Value.ToString("yyyy-MM-dd"), cmbItem_MAN.Text, SNO, tran)
                        GoTo TagDupGen
                    End If
                ElseIf GetSoftValue("TAGNOFROM") = "U" Then
                    Dim RateValue As String = ""
                    Dim a() As String = Nothing
                    Dim Result As String = ""
                    If Val(txtNetWt_Wet.Text) > 0 Then
                        a = txtNetWt_Wet.Text.Split(".")
                        Result = Trim(a(0)) & IIf(Val(a(1)) > 0, Val(a(1).TrimEnd("0")), "")
                        RateValue = IIf(Val(txtNetWt_Wet.Text) = 0, 0, Result)
                    Else
                        If Val(txtRate_Amt.Text.ToString) = 0 Then
                            'txtRate_Amt.Text = "0.00"
                            a = "0.00".ToString.Split(".")
                        Else
                            a = Val(txtRate_Amt.Text.ToString).ToString.Split(".")
                        End If
                        Dim i As Integer = a.Length
                        If Not i - 1 > 0 Then
                            Array.Resize(a, 2)
                            a(1) = Val("0")
                        End If
                        Result = Trim(a(0)) & IIf(Val(a(1)) > 0, Val(a(1).TrimEnd("0")), "")
                        RateValue = IIf(Val(txtRate_Amt.Text) = 0, 0, Result)
                    End If
                    txtTagNo__Man.Text = objTag.GetTagNo(dtpRecieptDate.Value.ToString("yyyy-MM-dd"), cmbItem_MAN.Text, SNO, tran, itemId, RateValue)
                End If
            End If

            strSql = " SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"
            Dim MetalId As String = objGPack.GetSqlValue(strSql, "METALID", "G", tran)
            Dim RangeExclude As Boolean = False
            Dim ITEMIDS() As String
            ITEMIDS = EXCLUDE_RANGE.Split(",")
            If EXCLUDE_RANGE <> "" Then
                For I As Integer = 0 To ITEMIDS.Length - 1
                    If itemId = ITEMIDS(I) Then
                        RangeExclude = True
                        Exit For
                    End If
                Next
            End If

            If TagNo_RangeBase And MetalId <> "S" Then
                strSql = " SELECT TOP 1 CAPTION FROM " & cnAdminDb & "..RANGEMAST WHERE ITEMID=" & itemId & " AND " & Val(txtGrossWt_Wet.Text) & " BETWEEN FROMWEIGHT AND TOWEIGHT  "
                StrCaption = objGPack.GetSqlValue(strSql, , "", tran)
                If StrCaption = "" Then
                    If Not tran Is Nothing Then tran.Rollback()
                    MsgBox("Please set Range...", MsgBoxStyle.Information) : txtGrossWt_Wet.Focus() : Exit Function
                End If
                strSql = " SELECT TOP 1 TAGNO FROM " & cnAdminDb & "..RANGEMAST WHERE ITEMID=" & itemId & " AND " & Val(txtGrossWt_Wet.Text) & " BETWEEN FROMWEIGHT AND TOWEIGHT  "
                StrTagNo = (Val(objGPack.GetSqlValue(strSql, , "", tran)) + 1).ToString
ReChk:
                strSql = " SELECT 1 FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO='" & StrCaption.ToString.Trim & "-" & StrTagNo.ToString.Trim & "' "
                If Val(objGPack.GetSqlValue(strSql, , "", tran)) > 0 Then
                    StrTagNo = (Val(StrTagNo.ToString) + 1).ToString
                    GoTo ReChk
                End If
                txtTagNo__Man.Text = StrCaption.ToString.Trim & "-" & StrTagNo.ToString.Trim
            ElseIf TAGNO_SIZEBASED And MetalId = "S" And cmbItemSize.Text <> "" Then
                strSql = " SELECT TOP 1 SHORTNAME FROM " & cnAdminDb & "..ITEMSIZE WHERE ITEMID=" & itemId & " AND SIZENAME='" & cmbItemSize.Text & "'"
                StrCaption = objGPack.GetSqlValue(strSql, , "", tran)
                If StrCaption = "" Then
                    If Not tran Is Nothing Then tran.Rollback()
                    MsgBox("Please set Range...", MsgBoxStyle.Information) : txtGrossWt_Wet.Focus() : Exit Function
                End If
                strSql = " SELECT TOP 1 TAGNO FROM " & cnAdminDb & "..ITEMSIZE WHERE ITEMID=" & itemId & " AND SIZENAME='" & cmbItemSize.Text & "'"
                StrTagNo = (Val(objGPack.GetSqlValue(strSql, , "", tran)) + 1).ToString
ReCheck:
                strSql = " SELECT 1 FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO='" & StrCaption.ToString.Trim & "-" & StrTagNo.ToString.Trim & "' "
                If Val(objGPack.GetSqlValue(strSql, , "", tran)) > 0 Then
                    StrTagNo = (Val(StrTagNo.ToString) + 1).ToString
                    GoTo ReCheck
                End If
                txtTagNo__Man.Text = StrCaption.ToString.Trim & "-" & StrTagNo.ToString.Trim
            Else
                If TagNo_Range And MetalId <> "S" And RangeExclude = False Then
                    strSql = " SELECT TOP 1 CAPTION FROM " & cnAdminDb & "..RANGEMAST WHERE ITEMID=" & itemId & " AND SUBITEMID=" & subitemId & " AND " & Val(txtGrossWt_Wet.Text) & " BETWEEN FROMWEIGHT AND TOWEIGHT  "
                    StrCaption = objGPack.GetSqlValue(strSql, , "", tran)
                    StrCaption = StrCaption.Replace("G", "")
                    If StrCaption = "" Then
                        If Not tran Is Nothing Then tran.Rollback()
                        MsgBox("Please set Range...", MsgBoxStyle.Information) : txtGrossWt_Wet.Focus() : Exit Function
                    End If
                    strSql = " SELECT TOP 1 CURRENTTAGNO FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=" & itemId
                    StrTagNo = (Val(objGPack.GetSqlValue(strSql, , "", tran)) + 1).ToString
                    'GET RANGE SNO
                    strSql = " SELECT TOP 1 SNO FROM " & cnAdminDb & "..RANGEMAST WHERE ITEMID=" & itemId & " AND SUBITEMID=" & subitemId & " AND " & Val(txtGrossWt_Wet.Text) & " BETWEEN FROMWEIGHT AND TOWEIGHT  "
                    RangeSno = objGPack.GetSqlValue(strSql, , "", tran)
ReCheckTag:
                    strSql = " SELECT 1 FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO='" & StrCaption.ToString.Trim & "-" & StrTagNo.ToString.Trim & "' "
                    If Val(objGPack.GetSqlValue(strSql, , "", tran)) > 0 Then
                        StrTagNo = (Val(StrTagNo.ToString) + 1).ToString
                        GoTo ReCheckTag
                    End If

                    'txtTagNo__Man.Text = StrSrtName.ToString.Trim & StrCaption.ToString.Trim & "-" & StrTagNo.ToString.Trim
                    txtTagNo__Man.Text = StrCaption.ToString.Trim & "-" & StrTagNo.ToString.Trim

                End If
            End If
            'ReCheckTag1:
            '            If GetSoftValue("TAGNOFROM") = "U" And TagNo_Range = False Then
            '                txtTagNo__Man.Text = objTag.GetTagNo(dtpRecieptDate.Value.ToString("yyyy-MM-dd"), cmbItem_MAN.Text, SNO, tran)
            '            End If
            '            '********
            '            strSql = " SELECT 1 FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO='" & txtTagNo__Man.Text & "' "
            '            If Val(objGPack.GetSqlValue(strSql, , "", tran)) > 0 Then
            '                'If GetSoftValue("TAGNOFROM") = "U" Then  'UNIQUE
            '                '    strSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT  = '" & Val(txtTagNo__Man.Text.ToString.Replace(GetSoftValue("TAGPREFIX"), "")) + 1 & "' WHERE CTLID = 'LASTTAGNO'"
            '                '    cmd = New OleDbCommand(strSql, cn, tran)
            '                '    cmd.ExecuteNonQuery()
            '                '    'ExecQuery(strSql, cn, tran, COSTID)
            '                'End If
            '                GoTo ReCheckTag1
            '            End If
Continuetagging:
            TagNo = txtTagNo__Man.Text
            If objTag._TagNoGen = "T" Or objTag._TagNoGen = "D" Or objTag._TagNoGen = "W" Then
                ' TagSno = IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & strCompanyId & TagNo
                TagSno = objTag.BasedOnSNOGenerator("", dtpRecieptDate.Value.Date, "")
            Else
                TagSno = GetNewSno(TranSnoType.ITEMTAGCODE, tran, "GET_ADMINSNO_TRAN")
            End If

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
            tagVal = objTag.GetTagVal(TagNo, tran, dtpRecieptDate.Value.ToString("yyyy-MM-dd"))
            '
            Dim purStoneValue As Double
            For Each roStn As DataRow In dtStoneDetails.Rows
                purStoneValue += Val(roStn!PURVALUE.ToString)
            Next
            UHallmarknoAsTagno = False
            If HallmarknoAsTagno Then
                If dtHallmarkDetails.Rows.Count > 0 Then
                    TagNo = dtHallmarkDetails.Rows(0).Item("HM_BILLNO").ToString
                    txtTagNo__Man.Text = dtHallmarkDetails.Rows(0).Item("HM_BILLNO").ToString
                    UHallmarknoAsTagno = True
                End If
            End If

            Dim orderRepNo As String = Nothing
            Dim orderRepSno As String = Nothing
            If Not OrderRow Is Nothing Then
                orderRepNo = OrderRow.Item("ORNO").ToString
                orderRepSno = OrderRow.Item("SNO").ToString

            ElseIf _CheckOrderInfo = False And ObjOrderTagInfo.txtOrderNo.Text <> "" Then
                Dim OrdBatchno As String = GetNewBatchno(cnCostId, dtpRecieptDate.Value, tran)
                orderRepNo = GetCostId(COSTID) & GetCompanyId(GetStockCompId()) & ObjOrderTagInfo.txtOrderNo.Text
                If objTag._TagNoGen = "T" Or objTag._TagNoGen = "D" Or objTag._TagNoGen = "W" Then
                    'orderRepSno = IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & TagNo
                    orderRepSno = objTag.BasedOnSNOGenerator("", dtpRecieptDate.Value.Date, "")
                Else
                    orderRepSno = GetNewSno(TranSnoType.ORMASTCODE, tran, "GET_ADMINSNO_TRAN")
                End If
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
                    If objTag._TagNoGen = "T" Or objTag._TagNoGen = "D" Or objTag._TagNoGen = "W" Then
                        'RowOrder.Item("SNO") = IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & TagNo
                        RowOrder.Item("SNO") = GetNewSno(TranSnoType.ORIRDETAILCODE, tran, "GET_ADMINSNO_TRAN")
                    Else
                        RowOrder.Item("SNO") = GetNewSno(TranSnoType.ORIRDETAILCODE, tran, "GET_ADMINSNO_TRAN")
                    End If
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
                If objTag._TagNoGen = "T" Or objTag._TagNoGen = "D" Or objTag._TagNoGen = "W" Then
                    'RowOrder.Item("SNO") = IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & TagNo
                    RowOrder.Item("SNO") = GetNewSno(TranSnoType.ORIRDETAILCODE, tran, "GET_ADMINSNO_TRAN")
                Else
                    RowOrder.Item("SNO") = GetNewSno(TranSnoType.ORIRDETAILCODE, tran, "GET_ADMINSNO_TRAN")
                End If
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

            If TAGWISE_DISCOUNT = False Then txtWDisc_Per.Text = 0

            ''INSERTING ITEMTAG

            'WRONGLY TAGSNO GENEREATE
            'strSql = "SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE NEWSNO='" & SNO & "'"
            'Dim NewSno As String = objGPack.GetSqlValue(strSql, "SNO", "", tran)
            'Dim StkType As String = ""
            'strSql = " SELECT TOP 1 STKTYPE FROM " & cnAdminDb & "..ITEMLOT WHERE SNO='" & IIf(NewSno = "", SNO, NewSno) & "'"
            'StkType = objGPack.GetSqlValue(strSql, "STKTYPE", "", tran)

            Dim NewSno As String = ""
            Dim getNwsno As String = ""
            strSql = "SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE NEWSNO='" & SNO & "'"
            getNwsno = objGPack.GetSqlValue(strSql, "SNO", "", tran)
            Dim StkType As String = ""
            strSql = " SELECT TOP 1 STKTYPE FROM " & cnAdminDb & "..ITEMLOT WHERE SNO='" & IIf(getNwsno = "", SNO, getNwsno) & "'"
            StkType = objGPack.GetSqlValue(strSql, "STKTYPE", "", tran)

            Dim LotEntryType As String = ""
            strSql = " SELECT TOP 1 ENTRYTYPE FROM " & cnAdminDb & "..ITEMLOT WHERE SNO='" & IIf(getNwsno = "", SNO, getNwsno) & "'"
            LotEntryType = objGPack.GetSqlValue(strSql, "ENTRYTYPE", "", tran)


            strSql = " INSERT INTO " & cnAdminDb & "..ITEMTAG"
            strSql += " ("
            strSql += " SNO,RECDATE,COSTID,ITEMID,ORDREPNO,ORSNO,"
            strSql += " ORDSALMANCODE,SUBITEMID,SIZEID,ITEMCTRID,TABLECODE,DESIGNERID,"
            strSql += " TAGNO"
            If txtSetTagno.Enabled Then
                strSql += " ,SETTAGNO"
            End If
            strSql += " ,PCS,GRSWT,"
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
            strSql += " BOARDRATE,RFID,TOUCH,HM_BILLNO,HM_CENTER,ADD_VA_PER,REFVALUE,VALUEADDEDTYPE,TCOSTID,"
            strSql += " EXTRAWT,TAGWT,COVERWT,"
            strSql += " USRATE,INDRS,"
            strSql += " RECSNO,FROMITEMID"
            If _FourCMaintain Then
                strSql += " ,STNGRPID,CUTID,COLORID,CLARITYID,SHAPEID,SETTYPEID,HEIGHT,WIDTH"
            End If
            If NeedTag_Hsncode Then
                strSql += ",HSN"
            End If
            strSql += ",TAGFIXEDVA,STKTYPE,OREXCESSWT,ENTRYTYPE,RRECDATE ) VALUES("
            strSql += " '" & TagSno & "'" 'SNO
            If Tag_ManualDate Then
                strSql += " ,'" & dtpRecieptDate.Value.Date.ToString("yyyy-MM-dd") & "'" 'RECDATE 
            Else
                strSql += " ,'" & GetEntryDate(dtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE 'dtpRecieptDate.Value.Date.ToString("yyyy-MM-dd")
            End If
            strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
            strSql += " ," & itemId & "" 'ITEMID
            ''strSql += " ,'" & orderRepNo & "'" 'ORDREPNO
            ''strSql += " ,'" & orderRepSno & "'" 'ORsno
            If LotEntryType = "WO" Then
                strSql += " ,'" & txtWorkOrderNo.Text & "'" 'ORDREPNO
                strSql += " ,''" 'ORsno
            Else
                strSql += " ,'" & orderRepNo & "'" 'ORDREPNO
                strSql += " ,'" & orderRepSno & "'" 'ORsno
            End If
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
            If txtSetTagno.Enabled Then
                If txtSetTagno.Text = "" Then
                    strSql += " ,'" & itemId.ToString & TagNo.ToString & "'" 'SETTAGNO
                Else
                    strSql += " ,'" & txtSetTagno.Text.ToString.Trim & "'" 'SETTAGNO
                End If
            End If
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
            If objTag._TagNoGen = "D" Or objTag._TagNoGen = "T" Or objTag._TagNoGen = "W" Then
                strSql += " ,'" & TagNo & "'" 'TAGKEY
            Else
                strSql += " ,'" & itemId.ToString & "" & TagNo & "'" 'TAGKEY
            End If
            strSql += " ," & tagVal & "" 'TAGVAL
            strSql += " ,'" & IIf(NewSno = "", SNO, NewSno) & "'" 'LOTSNO
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
                    If DefaultPctFile.ToString <> "" Then
                        Dim _tempdefpctfile As String = ""
                        _tempdefpctfile = DefaultPctFile.ToString
                        _tempdefpctfile = _tempdefpctfile.Replace("<ITEMID>", itemId.ToString)
                        _tempdefpctfile = _tempdefpctfile.Replace("<TAGNO>", TagNo.Replace(":", "").ToString)
                        strSql += " ,'" & _tempdefpctfile.ToString & "'" 'pctfile
                    Else
                        strSql += " ,'" & GetStockCompId() & "L" + txtLotNo_Num_Man.Text + "I" + itemId.ToString + "T" + TagNo.Replace(":", "") + IIf(picExtension.ToString.StartsWith("."), picExtension.ToString, "." + picExtension.ToString) & "'" 'pctfile
                    End If

                End If
            Else
                strSql += " ,'' " ' pctfile
                If DefaultPctFile.ToString <> "" Then
                    Dim _tempdefpctfile As String = ""
                    _tempdefpctfile = DefaultPctFile.ToString
                    _tempdefpctfile = _tempdefpctfile.Replace("<ITEMID>", itemId.ToString)
                    _tempdefpctfile = _tempdefpctfile.Replace("<TAGNO>", TagNo.Replace(":", "").ToString)
                    strSql += " ,'" & _tempdefpctfile.ToString & "'" 'pctfile
                Else
                    strSql += " ,''" ' pctfile
                End If
            End If
            'strSql += " ,'" & IIf(picPath <> Nothing, "L" + txtLotNo_Num_Man.Text + "I" + itemId.ToString + "T" + TagNo.Replace(":", "") + "." + picExtension.ToString, picPath) & "'" 'PCTFILE
            strSql += " ,''" 'OLDTAGNO
            strSql += " ," & Val(itemTypeId) & "" 'ITEMTYPEID
            If Tag_ManualDate Then
                strSql += " ,'" & dtpRecieptDate.Value.Date.ToString("yyyy-MM-dd") & "'" 'ACTUALRECDATE 
            Else
                strSql += " ,'" & GetEntryDate(dtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'ACTUALRECDATE
            End If
            strSql += " ,'" & Mid(CmbUnit.Text, 1, 1) & "'" 'WEIGHTUNIT
            strSql += " ,0" 'TRANSFERWT
            strSql += " ,NULL" 'CHKDATE
            strSql += " ,''" 'CHKTRAY
            strSql += " ,''" 'CARRYFLAG
            strSql += " ,''" 'BRANDID
            strSql += " ,''" 'PRNFLAG
            strSql += " ,0" 'MCDISCPER
            strSql += " ," & Val(txtWDisc_Per.Text) & "" 'WASTDISCPER
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
            If Tag_ManualDate Then
                strSql += " ,'" & dtpRecieptDate.Value.Date.ToString("yyyy-MM-dd") & "'" 'TRANSFERDATE 
            Else
                strSql += " ,'" & GetEntryDate(dtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'TRANSFERDATE
            End If
            strSql += " ," & Val(txtMetalRate_Amt.Text) & ""
            strSql += " ,'" & txtRfId.Text & "'"
            strSql += " ," & Val(txtTouch_AMT.Text) & ""
            strSql += " ,'" & txtHmBillNo.Text & "'" 'HM_BILLNO
            strSql += " ,'" & txtHmCentre.Text & "'" 'HM_CENTER
            strSql += " ," & Val(ObjPurDetail.txtpURFixedValueVa_AMT.Text) & "" 'ADD_VA_PER
            strSql += " ," & Val(txtRefVal_AMT.Text) & "" 'REFVALUE
            strSql += " ,'" & mlwmctype & "'"
            strSql += " ,'" & COSTID & "'" 'TCOSTID
            If RepairLot = False Then
                strSql += " ,'" & Val(ObjExtraWt.txtExtraWt_WET.Text) & "'" 'EXTRAWT
            Else
                strSql += " ,0" 'EXTRAWT
            End If

            strSql += " ,'" & Val(ObjTagWt.txtTagWt_WET.Text) & "'" 'TAGWT
            strSql += " ,'" & Val(ObjTagWt.txtCoverWt_WET.Text) & "'" 'COVERWT
            strSql += " ," & IIf(NEEDUS = True And Studded_Loose = "L", Val(ObjRsUs.txtUSDollar_Amt.Text), 0) & ""
            strSql += " ," & IIf(NEEDUS = True And Studded_Loose = "L", Val(ObjRsUs.txtIndRs_Amt.Text), 0) & ""
            strSql += " ,'" & Recsno & "'" 'RECSNO
            strSql += " ," & mfromItemid & "" 'FROMID
            If _FourCMaintain Then
                strSql += " ," & TagStnGrpId & "" 'STNGRPID
                strSql += " ," & TagCutId & "" 'CUTID
                strSql += " ," & TagColorId & "" 'COLORID
                strSql += " ," & TagClarityId & "" 'CLARITYID
                strSql += " ," & TagShapeId & "" 'SHAPEID
                strSql += " ," & TagSetTypeId & "" 'SETTYPEID
                strSql += " ," & TagHeight & "" 'HEIGHT
                strSql += " ," & TagWidth & "" 'WIDTH
            End If
            If NeedTag_Hsncode Then
                strSql += ",'" & txtHSN.Text & "'" 'HSNCODE
            End If
            strSql += " ,'" & IIf(chkFixedVa.Checked, "Y", "N") & "'"
            strSql += " ,'" & StkType & "'" 'STKTYPE
            If RepairLot = True Then
                strSql += " ," & Val(ObjExtraWt.txtExtraWt_WET.Text) & "" 'OREXCESSWT
            Else
                strSql += " ,0" 'OREXCESSWT
            End If
            strSql += " ,'" & LotEntryType & "'" 'ENTRYTYPE
            If NeedOldTag_Recdate And chkOldTagRecdate.Checked Then
                strSql += " ,'" & dtpOldTagRecDate_OWN.Value.Date.ToString("yyyy-MM-dd") & "'" 'RRECDATE
            Else
                strSql += " ,NULL" 'RRECDATE
            End If
            strSql += " )"
            If File.Exists(picPath) = True And SubItemPic = False Then
                Dim serverPath As String = Nothing
                Dim fileDestPath As String = ""
                If DefaultPctFile <> "" Then
                    Dim _tempdefpctfile As String = ""
                    _tempdefpctfile = DefaultPctFile.ToString
                    _tempdefpctfile = _tempdefpctfile.Replace("<ITEMID>", itemId.ToString)
                    _tempdefpctfile = _tempdefpctfile.Replace("<TAGNO>", txtTagNo__Man.Text.Replace(":", "").ToString)
                    fileDestPath = (defalutDestination & _tempdefpctfile.ToString)
                Else
                    fileDestPath = (defalutDestination & GetStockCompId().ToString & "L" + txtLotNo_Num_Man.Text + "I" + itemId.ToString + "T" + TagNo.Replace(":", "") + IIf(picExtension.ToString.StartsWith("."), picExtension.ToString, "." + picExtension.ToString))
                End If


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
                If COPYIMAGETOCATALOGPATH Then
                    Dim fileDestPath_Catalog As String = ""
                    If DefaultPctFile <> "" Then
                        Dim _tempdefpctfile As String = ""
                        _tempdefpctfile = DefaultPctFile.ToString
                        _tempdefpctfile = _tempdefpctfile.Replace("<ITEMID>", itemId.ToString)
                        _tempdefpctfile = _tempdefpctfile.Replace("<TAGNO>", txtTagNo__Man.Text.Replace(":", "").ToString)
                        fileDestPath_Catalog = (CatalogDestination & _tempdefpctfile.ToString)
                    Else
                        fileDestPath_Catalog = (CatalogDestination & GetStockCompId().ToString + "L" + txtLotNo_Num_Man.Text + "I" + itemId.ToString + "T" + TagNo.Replace(":", "") + IIf(picExtension.ToString.StartsWith("."), picExtension.ToString, "." + picExtension.ToString))
                    End If
                    If UCase(picPath) <> fileDestPath_Catalog.ToUpper Then
                        Dim cFile As New FileInfo(picPath)
                        cFile.CopyTo(fileDestPath_Catalog, True)
                    End If
                End If
            End If

            If XToCostid <> "" And XBranchtag Then
                ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid, , IIf(picPath <> Nothing, (defalutDestination + "L" + txtLotNo_Num_Man.Text + "I" + itemId.ToString + "T" + txtTagNo__Man.Text.Replace(":", "") + "." + picExtension), Nothing)) ', "TITEMTAG", , True)
            Else
                cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
            End If
            TagStnGrpId = 0 : TagCutId = 0 : TagColorId = 0 : TagClarityId = 0 : TagShapeId = 0 : TagSetTypeId = 0 : TagHeight = 0 : TagWidth = 0
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
                strSql += vbCrLf + " ,PURTAXPER"
                strSql += vbCrLf + " ,PURTAX"
                strSql += vbCrLf + " ,RECDATE"
                strSql += vbCrLf + " ,COMPANYID,COSTID"
                strSql += vbCrLf + " ,DESCRIPT"
                strSql += vbCrLf + " ,PURLCOSTPER"
                strSql += vbCrLf + " ,PURLCOST"
                strSql += vbCrLf + " ,PURSCOSTPER"
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
                strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurTaxPer_PER.Text) & ""
                strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurTax_AMT.Text) & ""
                strSql += vbCrLf + " ,'" & ObjPurDetail.dtpPurchaseDate.Value.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " ,'" & GetStockCompId() & "'"
                strSql += vbCrLf + " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'"
                strSql += vbCrLf + " ,'" & ObjPurDetail.txtNarration.Text & "'" 'DESCRIPT
                strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurLCostper_NUM.Text) & "" ' PURLCOSTPER"
                strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurLCost_NUM.Text) & "" ' PURLCOST"
                strSql += vbCrLf + " ," & Val(ObjPurDetail.txtSaleRate_PER.Text) & "" ' PURSCOSTPER"
                strSql += vbCrLf + " )"
                If XToCostid <> "" And XBranchtag Then
                    ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid) ', , , "TPURITEMTAG", , True)
                Else
                    cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                End If
                'ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID, , , "TPURITEMTAG", , False)
            End If

            ''INSERTING MULTIMETAL
            If MetalBasedStone And dtMultiMetalDetails.Rows.Count > 0 Then
                For cnt As Integer = 0 To dtMultiMetalDetails.Rows.Count - 1
                    Dim metalSno As String = ""
                    If objTag._TagNoGen = "T" Or objTag._TagNoGen = "D" Or objTag._TagNoGen = "W" Then
                        'metalSno = IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & TagNo & cnt
                        metalSno = objTag.BasedOnSNOGenerator("", dtpRecieptDate.Value.Date, "") & cnt
                    Else
                        metalSno = GetNewSno(TranSnoType.ITEMTAGMETALCODE, tran, "GET_ADMINSNO_TRAN")
                    End If

                    Dim multiMetalId As String = Nothing
                    Dim multiCatCode As String = Nothing
                    Dim multikeyno As Integer = 0
                    With dtMultiMetalDetails.Rows(cnt)
                        multikeyno = Val(.Item("KEYNO").ToString)
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
                        strSql += " ,NETWT"
                        strSql += " )VALUES("
                        strSql += " '" & multiMetalId & "'" 'METALID
                        strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                        strSql += " ,'" & itemId & "'" 'ITEMID
                        If Tag_ManualDate Then
                            strSql += " ,'" & dtpRecieptDate.Value.Date.ToString("yyyy-MM-dd") & "'" 'RECDATE 
                        Else
                            strSql += " ,'" & GetEntryDate(dtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                        End If
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
                        strSql += " ," & Val(.Item("NETWT").ToString) & "" 'NETWT
                        strSql += " )"
                        If XToCostid <> "" And XBranchtag Then
                            ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid) ', , , "TITEMTAGMETAL", , True)
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
                                ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid) ', , , "TPURITEMTAGMETAL", , True)
                            Else
                                cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                            End If
                        End If

                    End If

                    ''Inserting StoneDetail
                    If dtStoneDetails.Rows.Count > 0 Then
                        For cnt1 As Integer = 0 To dtStoneDetails.Rows.Count - 1
                            If Val(dtStoneDetails.Rows(cnt1).Item("MKEYNO").ToString) <> multikeyno Then Continue For
                            With dtStoneDetails.Rows(cnt1)
                                Dim CutId As Integer = 0
                                Dim ColorId As Integer = 0
                                Dim ClarityId As Integer = 0
                                Dim ShapeId As Integer = 0
                                Dim SizeCode As Integer = 0
                                Dim SetTypeId As Integer = 0
                                Dim stnItemId As Integer = 0
                                Dim stnSubItemId As Integer = 0
                                Dim stnSno As String = ""
                                If objTag._TagNoGen = "T" Or objTag._TagNoGen = "D" Or objTag._TagNoGen = "W" Then
                                    'stnSno = IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & TagNo & cnt1
                                    stnSno = objTag.BasedOnSNOGenerator("", dtpRecieptDate.Value.Date, "") & cnt1
                                Else
                                    stnSno = GetNewSno(TranSnoType.ITEMTAGSTONECODE, tran, "GET_ADMINSNO_TRAN")
                                End If
                                'Dim caType As String = Nothing
                                strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Item("ITEM").ToString & "'"
                                stnItemId = Val(objGPack.GetSqlValue(strSql, "ITEMID", , tran))
                                strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & .Item("SUBITEM").ToString & "' AND ITEMID = " & stnItemId & ""
                                stnSubItemId = Val(objGPack.GetSqlValue(strSql, , , tran))
                                CutId = Val(objGPack.GetSqlValue(" SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & .Item("CUT").ToString & "'", "CUTID", , tran))
                                ColorId = Val(objGPack.GetSqlValue(" SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME = '" & .Item("COLOR").ToString & "'", "COLORID", , tran))
                                ClarityId = Val(objGPack.GetSqlValue(" SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & .Item("CLARITY").ToString & "'", "CLARITYID", , tran))
                                ShapeId = Val(objGPack.GetSqlValue(" SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & .Item("SHAPE").ToString & "'", "SHAPEID", , tran))
                                SizeCode = Val(objGPack.GetSqlValue(" SELECT STNSIZEID FROM " & cnAdminDb & "..STNSIZE WHERE SIZENAME = '" & .Item("STNSIZE").ToString & "'", "STNSIZEID", , tran))
                                SetTypeId = Val(objGPack.GetSqlValue(" SELECT SETTYPEID FROM " & cnAdminDb & "..STNSETTYPE WHERE SETTYPENAME = '" & .Item("SETTYPE").ToString & "'", "SETTYPEID", , tran))

                                .Item("STNSNO") = stnSno
                                ''Inserting itemTagStone
                                strSql = " INSERT INTO " & cnAdminDb & "..ITEMTAGSTONE("
                                strSql += " SNO,TAGSNO,ITEMID,COMPANYID,STNITEMID,"
                                strSql += " STNSUBITEMID,TAGNO,STNPCS,STNWT,"
                                strSql += " STNRATE,STNAMT,DESCRIP,"
                                strSql += " RECDATE,CALCMODE,"
                                strSql += " MINRATE,STONEUNIT,ISSDATE,"
                                strSql += " OLDTAGNO,CARRYFLAG,COSTID,SYSTEMID,APPVER,"
                                strSql += " USRATE,INDRS,PACKETNO"
                                strSql += " ,STNGRPID,CUTID,COLORID,CLARITYID,SHAPEID,SIZECODE,SETTYPEID,HEIGHT,WIDTH"
                                strSql += " ,TAGMSNO"
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
                                If Tag_ManualDate Then
                                    strSql += " ,'" & dtpRecieptDate.Value.Date.ToString("yyyy-MM-dd") & "'" 'RECDATE 
                                Else
                                    strSql += " ,'" & GetEntryDate(dtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                                End If
                                strSql += " ,'" & .Item("CALC").ToString & "'" 'CALCMODE
                                strSql += " ,0" 'MINRATE
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
                                strSql += " ,'" & Val(.Item("STNGRPID").ToString) & "'" 'STNGRPID
                                strSql += " ," & CutId & "" 'CUTID
                                strSql += " ," & ColorId & "" 'COLORID
                                strSql += " ,'" & ClarityId & "'" 'CLARITYID
                                strSql += " ,'" & ShapeId & "'" 'SHAPEID
                                strSql += " ,'" & SizeCode & "'" 'SIZECODE
                                strSql += " ,'" & SetTypeId & "'" 'SETTYPEID
                                strSql += " ,'" & Val(.Item("HEIGHT").ToString) & "'" 'HEIGHT
                                strSql += " ,'" & Val(.Item("WIDTH").ToString) & "'" 'WIDTH
                                strSql += " ,'" & metalSno & "'" 'TAGMETALSNO
                                strSql += " )"
                                If XToCostid <> "" And XBranchtag Then
                                    ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid) ', , , "TITEMTAGSTONE", , True)
                                Else
                                    cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                                End If
                                'ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID, , , "TITEMTAGSTONE", , False)
                            End With
                        Next
                    End If
                Next
            Else
                For cnt As Integer = 0 To dtMultiMetalDetails.Rows.Count - 1
                    Dim metalSno As String = ""
                    If objTag._TagNoGen = "T" Or objTag._TagNoGen = "D" Or objTag._TagNoGen = "W" Then
                        'metalSno = IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & TagNo & cnt
                        metalSno = objTag.BasedOnSNOGenerator("", dtpRecieptDate.Value.Date, "") & cnt
                    Else
                        metalSno = GetNewSno(TranSnoType.ITEMTAGMETALCODE, tran, "GET_ADMINSNO_TRAN")
                    End If

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
                        If Tag_ManualDate Then
                            strSql += " ,'" & dtpRecieptDate.Value.Date.ToString("yyyy-MM-dd") & "'" 'RECDATE 
                        Else
                            strSql += " ,'" & GetEntryDate(dtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                        End If
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
                            ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid) ', , , "TITEMTAGMETAL", , True)
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
                                ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid) ', , , "TPURITEMTAGMETAL", , True)
                            Else
                                cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                            End If
                        End If

                    End If
                Next
                ''Inserting StoneDetail
                For cnt As Integer = 0 To dtStoneDetails.Rows.Count - 1
                    With dtStoneDetails.Rows(cnt)
                        Dim CutId As Integer = 0
                        Dim ColorId As Integer = 0
                        Dim ClarityId As Integer = 0
                        Dim ShapeId As Integer = 0
                        Dim SizeCode As Integer = 0
                        Dim SetTypeId As Integer = 0
                        Dim stnItemId As Integer = 0
                        Dim stnSubItemId As Integer = 0
                        Dim stnSno As String = ""
                        If objTag._TagNoGen = "T" Or objTag._TagNoGen = "D" Or objTag._TagNoGen = "W" Then
                            'stnSno = IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & TagNo & cnt
                            stnSno = objTag.BasedOnSNOGenerator("", dtpRecieptDate.Value.Date, "") & cnt
                        Else
                            stnSno = GetNewSno(TranSnoType.ITEMTAGSTONECODE, tran, "GET_ADMINSNO_TRAN")
                        End If
                        'Dim caType As String = Nothing
                        strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Item("ITEM").ToString & "'"
                        stnItemId = Val(objGPack.GetSqlValue(strSql, "ITEMID", , tran))
                        strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & .Item("SUBITEM").ToString & "' AND ITEMID = " & stnItemId & ""
                        stnSubItemId = Val(objGPack.GetSqlValue(strSql, , , tran))
                        CutId = Val(objGPack.GetSqlValue(" SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & .Item("CUT").ToString & "'", "CUTID", , tran))
                        ColorId = Val(objGPack.GetSqlValue(" SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME = '" & .Item("COLOR").ToString & "'", "COLORID", , tran))
                        ClarityId = Val(objGPack.GetSqlValue(" SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & .Item("CLARITY").ToString & "'", "CLARITYID", , tran))
                        ShapeId = Val(objGPack.GetSqlValue(" SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & .Item("SHAPE").ToString & "'", "SHAPEID", , tran))
                        SizeCode = Val(objGPack.GetSqlValue(" SELECT STNSIZEID FROM " & cnAdminDb & "..STNSIZE WHERE SIZENAME = '" & .Item("STNSIZE").ToString & "'", "STNSIZEID", , tran))
                        SetTypeId = Val(objGPack.GetSqlValue(" SELECT SETTYPEID FROM " & cnAdminDb & "..STNSETTYPE WHERE SETTYPENAME = '" & .Item("SETTYPE").ToString & "'", "SETTYPEID", , tran))

                        .Item("STNSNO") = stnSno
                        ''Inserting itemTagStone
                        strSql = " INSERT INTO " & cnAdminDb & "..ITEMTAGSTONE("
                        strSql += " SNO,TAGSNO,ITEMID,COMPANYID,STNITEMID,"
                        strSql += " STNSUBITEMID,TAGNO,STNPCS,STNWT,"
                        strSql += " STNRATE,STNAMT,DESCRIP,"
                        strSql += " RECDATE,CALCMODE,"
                        strSql += " MINRATE,STONEUNIT,ISSDATE,"
                        strSql += " OLDTAGNO,CARRYFLAG,COSTID,SYSTEMID,APPVER,"
                        strSql += " USRATE,INDRS,PACKETNO"
                        strSql += " ,STNGRPID,CUTID,COLORID,CLARITYID,SHAPEID,SIZECODE,SETTYPEID,HEIGHT,WIDTH"
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
                        If Tag_ManualDate Then
                            strSql += " ,'" & dtpRecieptDate.Value.Date.ToString("yyyy-MM-dd") & "'" 'RECDATE 
                        Else
                            strSql += " ,'" & GetEntryDate(dtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                        End If
                        strSql += " ,'" & .Item("CALC").ToString & "'" 'CALCMODE
                        strSql += " ,0" 'MINRATE
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
                        strSql += " ,'" & Val(.Item("STNGRPID").ToString) & "'" 'STNGRPID
                        strSql += " ," & CutId & "" 'CUTID
                        strSql += " ," & ColorId & "" 'COLORID
                        strSql += " ,'" & ClarityId & "'" 'CLARITYID
                        strSql += " ,'" & ShapeId & "'" 'SHAPEID
                        strSql += " ,'" & SizeCode & "'" 'SIZECODE
                        strSql += " ,'" & SetTypeId & "'" 'SETTYPEID
                        strSql += " ,'" & Val(.Item("HEIGHT").ToString) & "'" 'HEIGHT
                        strSql += " ,'" & Val(.Item("WIDTH").ToString) & "'" 'WIDTH
                        strSql += " )"
                        If XToCostid <> "" And XBranchtag Then
                            ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid) ', , , "TITEMTAGSTONE", , True)
                        Else
                            cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                        End If
                        'ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID, , , "TITEMTAGSTONE", , False)
                    End With
                Next
            End If



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
                        ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid) ', , , "TPURITEMTAGSTONE", , True)
                    Else
                        cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                    End If
                    'ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID, , , "TPURITEMTAGSTONE", , False)
                Next
            End If
            ''iNSERTING MISC
            For cnt As Integer = 0 To dtMiscDetails.Rows.Count - 1
                Dim miscSno As String = ""
                If objTag._TagNoGen = "T" Or objTag._TagNoGen = "D" Or objTag._TagNoGen = "W" Then
                    'miscSno = IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & TagNo & cnt
                    miscSno = objTag.BasedOnSNOGenerator("", dtpRecieptDate.Value.Date, "") & cnt
                Else
                    miscSno = GetNewSno(TranSnoType.ITEMTAGMISCCHARCODE, tran, "GET_ADMINSNO_TRAN")
                End If
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
                        ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid) ', , , "TITEMTAGMISCCHAR", , True)
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
                            ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid) ', , , "TPURITEMTAGMISCCHAR", , True)
                        Else
                            cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                        End If


                    End If
                End If
            Next


            ''INSERTING HALLMARK DETAILS
            For cnt As Integer = 0 To dtHallmarkDetails.Rows.Count - 1
                Dim HALLMARKSno As String = ""
                If objTag._TagNoGen = "T" Or objTag._TagNoGen = "D" Or objTag._TagNoGen = "W" Then
                    HALLMARKSno = objTag.BasedOnSNOGenerator("", dtpRecieptDate.Value.Date, "") & cnt
                Else
                    HALLMARKSno = GetNewSno(TranSnoType.ITEMTAGHALLMARKCODE, tran, "GET_ADMINSNO_TRAN")
                End If
                With dtHallmarkDetails.Rows(cnt)
                    strSql = " INSERT INTO " & cnAdminDb & "..ITEMTAGHALLMARK"
                    strSql += " ("
                    strSql += " SNO,TAGSNO,GRSWT,HM_BILLNO,USERID,SYSTEMID,APPVER,UPDATED,UPTIME,COSTID,COMPANYID"
                    strSql += " )VALUES("
                    strSql += " '" & HALLMARKSno & "'" 'SNO
                    strSql += " ,'" & TagSno & "'" 'TAGSNO
                    strSql += " ,'" & Val(.Item("GRSWT").ToString) & "'" 'GRSWT
                    strSql += " ,'" & .Item("HM_BILLNO").ToString & "'" 'HM_BILLNO
                    strSql += " ," & userId & "" 'USERID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                    strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                    strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    strSql += " )"
                    If XToCostid <> "" And XBranchtag Then
                        ExecQuery(SyncMode.Stock, strSql, cn, tran, XToCostid)
                    Else
                        cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                    End If
                End With
            Next



            ''INSERTING ADDITIONAL STOCK ENTRY
            For cnt As Integer = 0 To DtAddStockEntry.Rows.Count - 1
                With DtAddStockEntry.Rows(cnt)
                    ''INSERTING ADDINFOITEMTAG
                    strSql = " INSERT INTO " & cnAdminDb & "..ADDINFOITEMTAG("
                    strSql += " SNO,TAGSNO,OTHID,ITEMID,TAGNO,RECDATE,COMPANYID"
                    strSql += " ,COSTID,SYSTEMID,APPVER"
                    strSql += " )VALUES("
                    If objTag._TagNoGen = "T" Or objTag._TagNoGen = "D" Or objTag._TagNoGen = "W" Then
                        'strSql += " '" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & TagNo & cnt & "' "
                        strSql += " '" & objTag.BasedOnSNOGenerator("", dtpRecieptDate.Value.Date, "") & cnt & "' "
                    Else
                        strSql += " '" & GetNewSno(TranSnoType.ADDINFOITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO  
                    End If
                    strSql += " ,'" & TagSno & "'" 'TAGSNO
                    strSql += " ,'" & Val(.Item("OTHID").ToString) & "'" 'OTHID
                    strSql += " ,'" & itemId & "'" 'ITEMID
                    strSql += " ,'" & TagNo & "'" 'TAGNO
                    If Tag_ManualDate Then
                        strSql += " ,'" & dtpRecieptDate.Value.Date.ToString("yyyy-MM-dd") & "'" 'RECDATE 
                    Else
                        strSql += " ,'" & GetEntryDate(dtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
                    End If
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
                    If objTag._TagNoGen = "T" Or objTag._TagNoGen = "D" Or objTag._TagNoGen = "W" Then
                        'strSql += " '" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & TagNo & "'"
                        strSql += " '" & GetNewSno(TranSnoType.ORIRDETAILCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO
                    Else
                        strSql += " '" & GetNewSno(TranSnoType.ORIRDETAILCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO
                    End If
                    strSql += " ,'" & OrderRow.Item("SNO").ToString & "'" 'ORSNO
                    'strSql += " ," & Val(objGPack.GetSqlValue(" SELECT ISNULL(MAX(CTLTEXT),0)+1 AS TRANNO FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID  = 'ORDIRBILLNO' AND COMPANYID = '" & strCompanyId & "'", , , tran)) & "" 'TRANNO
                    strSql += " ," & Val(Ord_TranNo) & "" 'TRANNO
                    If Tag_ManualDate Then
                        strSql += " ,'" & dtpRecieptDate.Value.Date.ToString("yyyy-MM-dd") & "'" 'RECDATE 
                    Else
                        strSql += " ,'" & GetEntryDate(dtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'TRANDATE
                    End If
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
                If objTag._TagNoGen = "T" Or objTag._TagNoGen = "D" Or objTag._TagNoGen = "W" Then
                    'strSql += " '" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & TagNo & "'"
                    strSql += " '" & GetNewSno(TranSnoType.ORIRDETAILCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO
                Else
                    strSql += " '" & GetNewSno(TranSnoType.ORIRDETAILCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO
                End If
                strSql += " ,'" & OrderRow.Item("SNO").ToString & "'" 'ORSNO
                'strSql += " ," & Val(objGPack.GetSqlValue(" SELECT ISNULL(MAX(CTLTEXT),0)+1 AS TRANNO FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID  = 'ORDIRBILLNO' AND COMPANYID = '" & strCompanyId & "'", , , tran)) & "" 'TRANNO
                strSql += " ," & Val(Ord_TranNo) & "" 'TRANNO
                If Tag_ManualDate Then
                    strSql += " ,'" & dtpRecieptDate.Value.Date.ToString("yyyy-MM-dd") & "'" 'TRANDATE 
                Else
                    strSql += " ,'" & GetEntryDate(dtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'TRANDATE
                End If
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

            Dim tagPrefix As String = GetSoftValue("TAGPREFIX")
            Dim Comp_TagPrefix As String = ""
            Comp_TagPrefix = GetAdmindbSoftValue("TAGPREFIX_" & strCompanyId, , tran)
            If Comp_TagPrefix <> "" Then
                tagPrefix = Comp_TagPrefix
            End If
            'FOR TAGNO FROM RANGE MASTER
            If UHallmarknoAsTagno = False Then
                If TagNo_RangeBase And MetalId <> "S" Then
                    tagPrefix = StrSrtName + StrCaption
                    strSql = " UPDATE " & cnAdminDb & "..RANGEMAST SET TAGNO  = '" & StrTagNo & "' WHERE ITEMID=" & itemId & " AND " & Val(txtGrossWt_Wet.Text) & " BETWEEN FROMWEIGHT AND TOWEIGHT  "
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()
                ElseIf TAGNO_SIZEBASED And cmbItemSize.Text <> "" And MetalId = "S" Then
                    tagPrefix = StrSrtName + StrCaption
                    strSql = " UPDATE " & cnAdminDb & "..ITEMSIZE SET TAGNO  = '" & StrTagNo & "' WHERE ITEMID=" & itemId & " AND SIZENAME='" & cmbItemSize.Text & "'"
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()
                ElseIf TagNo_Range Then
                    tagPrefix = StrSrtName + StrCaption
                    strSql = " UPDATE " & cnAdminDb & "..ITEMMAST SET CURRENTTAGNO  = '" & StrTagNo & "' WHERE ITEMID=" & itemId
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()
                End If
                Dim updTagNo As String = Nothing
                If tagPrefix.Length > 0 Then
                    updTagNo = txtTagNo__Man.Text.Replace(tagPrefix, "")
                Else
                    updTagNo = txtTagNo__Man.Text
                End If
                If TagNo_Range = False And TagNo_RangeBase = False Then
                    If GetSoftValue("TAGNOFROM") = "I" Then ''FROM ITEMMAST OR UNIQUE
                        If GetSoftValue("TAGNOGEN") = "I" Then ''FROM ITEM
                            If TagPrefix_Item = True Then
                                tagPrefix = ""
                                strSql = " SELECT SUBSTRING(SHORTNAME,1,5)SHORTNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = "
                                strSql += " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')"
                                tagPrefix = objGPack.GetSqlValue(strSql, , "", tran)
                                If tagPrefix.Length > 0 Then
                                    updTagNo = txtTagNo__Man.Text.Replace(tagPrefix, "")
                                Else
                                    updTagNo = txtTagNo__Man.Text
                                End If
                            End If
                            strSql = " UPDATE " & cnAdminDb & "..ITEMMAST SET CURRENTTAGNO = '" & updTagNo & "' WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"
                            cmd = New OleDbCommand(strSql, cn, tran)
                            cmd.ExecuteNonQuery()
                            'ExecQuery(strSql, cn, tran, COSTID)
                        Else ''FROM LOT
                            strSql = " UPDATE " & cnAdminDb & "..ITEMLOT SET CURTAGNO = '" & updTagNo & "'"
                            cmd = New OleDbCommand(strSql, cn, tran)
                            cmd.ExecuteNonQuery()
                            'ExecQuery(strSql, cn, tran, COSTID)
                        End If
                    ElseIf GetSoftValue("TAGNOFROM") = "U" Then  'UNIQUE
                        strSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT  = '" & updTagNo & "' WHERE CTLID = 'LASTTAGNO'"
                        cmd = New OleDbCommand(strSql, cn, tran)
                        cmd.ExecuteNonQuery()
                        'ExecQuery(strSql, cn, tran, COSTID)
                    End If
                End If

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
                strSql = " UPDATE " & cnAdminDb & "..ITEMLOT SET CPCS = CPCS + " & Val(txtPieces_Num_Man.Text) & ""
                strSql += " ,CLOSETIME=GETDATE(),CGRSWT = CGRSWT + " & Val(txtGrossWt_Wet.Text) & ""
                strSql += " ,CNETWT = ISNULL(CNETWT,0) + " & Val(txtNetWt_Wet.Text) & ""
                strSql += " WHERE SNO = '" & SNO & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
                'ExecQuery(strSql, cn, tran, COSTID)

            Else
                'CPIECES AND CWT for hallmark no as tagno
                strSql = " UPDATE " & cnAdminDb & "..ITEMLOT SET CPCS = CPCS + " & Val(txtPieces_Num_Man.Text) & ""
                strSql += " ,CLOSETIME=GETDATE(),CGRSWT = CGRSWT + " & Val(txtGrossWt_Wet.Text) & ""
                strSql += " ,CNETWT = ISNULL(CNETWT,0) + " & Val(txtNetWt_Wet.Text) & ""
                strSql += " WHERE SNO = '" & SNO & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
            End If
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
            ro("WASTAGE") = IIf(Val(txtMaxWastage_Wet.Text) <> 0, Val(txtMaxWastage_Wet.Text), DBNull.Value)
            ro("MC") = IIf(Val(txtMaxMkCharge_Amt.Text) <> 0, Val(txtMaxMkCharge_Amt.Text), DBNull.Value)
            ro("RATE") = IIf(Val(txtRate_Amt.Text) <> 0, Val(txtRate_Amt.Text), DBNull.Value)
            ro("SALEVALUE") = txtSalValue_Amt_Man.Text
            ro("SIZE") = cmbItemSize.Text
            dtGridView.Rows.Add(ro)
            dtGridView.AcceptChanges()
            gridView.CurrentCell = gridView.Rows(gridView.RowCount - 1).Cells("ITEM")


            tran.Commit()
            tran = Nothing

            If SMS_MSG_ORDTAG <> "" And Not OrderRow Is Nothing Then
                strSql = "SELECT COUNT(*)CNT FROM " & cnAdminDb & "..ORMAST WHERE ORNO='" & OrderRow.Item("ORNO").ToString & "'"
                Dim NoofOrdItem As Integer = Val(objGPack.GetSqlValue(strSql, "CNT", 0).ToString)
                Dim NoofOrdTag As Integer = 0
                Dim SalVal As Double = 0

                strSql = " SELECT COUNT(*)CNT,SUM(SALVALUE)SALVALUE FROM " & cnAdminDb & "..ITEMTAG WHERE ORDREPNO='" & OrderRow.Item("ORNO").ToString & "'"
                Dim dr As DataRow
                dr = GetSqlRow(strSql, cn, tran)
                If Not dr Is Nothing Then
                    NoofOrdTag = Val(dr("CNT").ToString)
                    SalVal = Val(dr("SALVALUE").ToString)
                End If
                If NoofOrdTag = NoofOrdItem Then
                    strSql = "SELECT PNAME,MOBILE FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO="
                    strSql += " (SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & OrderRow.Item("BATCHNO").ToString & "')"
                    Dim OrdAddr As DataRow = GetSqlRow(strSql, cn, tran)
                    If Not OrdAddr Is Nothing Then
                        strSql = "SELECT SUM(CASE WHEN RECPAY='R' THEN AMOUNT ELSE -1*AMOUNT END)BALANCE"
                        strSql += " FROM " & cnAdminDb & "..OUTSTANDING WHERE ISNULL(CANCEL,'')=''"
                        strSql += " AND RUNNO='" & OrderRow.Item("ORNO").ToString & "'"
                        Dim TempMsg As String = ""
                        Dim BalAmt As Decimal = Val(objGPack.GetSqlValue(strSql, "BALANCE", 0, tran))
                        BalAmt = SalVal - BalAmt
                        TempMsg = SMS_MSG_ORDTAG
                        TempMsg = Replace(SMS_MSG_ORDTAG, vbCrLf, "")
                        TempMsg = Replace(TempMsg, "<NAME>", OrdAddr.Item("PNAME").ToString)
                        TempMsg = Replace(TempMsg, "<ORDERNO>", Mid(OrderRow.Item("ORNO").ToString, 6, 20))
                        TempMsg = Replace(TempMsg, "<AMOUNT>", BalAmt.ToString)
                        TempMsg = Replace(TempMsg, "<ITEMNAME>", cmbItem_MAN.Text.ToString)
                        TempMsg = Replace(TempMsg, "<GRSWT>", txtGrossWt_Wet.Text.ToString)
                        TempMsg = Replace(TempMsg, "<NETWT>", txtNetWt_Wet.Text.ToString)
                        If Mid(OrderRow.Item("ORNO").ToString, 6, 1) = "O" Then
                            TempMsg = Replace(TempMsg, "<BILLTYPE>", "Order")
                        ElseIf Mid(OrderRow.Item("ORNO").ToString, 6, 1) = "R" Then
                            TempMsg = Replace(TempMsg, "<BILLTYPE>", "Repair")
                        End If
                        SmsSend(TempMsg, OrdAddr.Item("MOBILE").ToString)
                    End If
                End If
            End If

            MsgBox(TagNo + E0012, MsgBoxStyle.Exclamation)
            If TAGWOLOT = False Then
                ''Lot Pcs
                lblPCompled.Text = Val(lblPCompled.Text) + Val(txtPieces_Num_Man.Text)
                lblPBalance.Text = Val(lblPLot.Text) - Val(lblPCompled.Text)
                mfromItemid = 0
                ''Lot Wt
                lblWCompleted.Text = IIf(Val(lblWCompleted.Text) + Val(txtGrossWt_Wet.Text) <> 0,
                Format(Val(lblWCompleted.Text) + Val(txtGrossWt_Wet.Text), "0.000"), Nothing)
                lblWBalance.Text = IIf(Val(lblWLot.Text) - Val(lblWCompleted.Text) <> 0,
                Format(Val(lblWLot.Text) - Val(lblWCompleted.Text), "0.000"), Nothing)

                lblNWCompleted.Text = IIf(Val(lblNWCompleted.Text) + Val(txtNetWt_Wet.Text) <> 0,
                Format(Val(lblNWCompleted.Text) + Val(txtNetWt_Wet.Text), "0.000"), Nothing)
                lblNWBalance.Text = IIf(Val(lblNWLot.Text) - Val(lblNWCompleted.Text) <> 0,
                Format(Val(lblNWLot.Text) - Val(lblNWCompleted.Text), "0.000"), Nothing)
            Else
                txtLotNo_Num_Man.Text = ""
            End If

            ''Last Tag and Wt
            lblLastTagNo.Text = TagNo
            lblLastTagWt.Text = txtGrossWt_Wet.Text
            Dim prnmemsuffix As String = ""
            Dim objBar As New clsBarcodePrint
            If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId
            If chkBarcodePrint.Checked = True Then
                If CallBarcodeExe = False Then
                    If MetalId = "G" Then
                        objBar.FuncprintBarcode_Single(itemId, TagNo)
                    Else
                        If Val(GetAdmindbSoftValue("BARCODE_FORMAT", "1")) <= "1" Then
                            objBar.FuncprintBarcode_Single(itemId, TagNo)
                        Else
                            FuncprintBarcode_Multi(itemId, TagNo, Val(GetAdmindbSoftValue("BARCODE_FORMAT", "1")), MetalId)
                        End If
                    End If
                Else
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
            If tabCheckBy = "P" Then
                If Not Val(lblPBalance.Text) > 0 Then flagComplete = True
            ElseIf tabCheckBy = "E" Then
                If Not Val(lblPBalance.Text) > 0 And Not Val(lblWBalance.Text) > 0 Then flagComplete = True
            Else
                If Not Val(lblWBalance.Text) > 0 Then flagComplete = True
            End If
            If flagComplete And TAGWOLOT = False Then
                MsgBox("Lot Completed..", MsgBoxStyle.Information)
                If lotprintack = True Then
                    If MsgBox("Do you print acknowledgement?", MsgBoxStyle.YesNo, "Brighttech Message") = MsgBoxResult.Yes Then
                        DetailPrint()
                    End If
                End If
                funcNew()
            Else
                funcMultyNew()
                If Not OrderRow Is Nothing Then
                    LoadOrderDetails(OrderRow.Item("ORNO").ToString)
                ElseIf _CheckOrderInfo = False And ObjOrderTagInfo.txtOrderNo.Text <> "" Then
                    If ObjOrderTagInfo.ShowDialog() <> Windows.Forms.DialogResult.OK Then
                        btnNew_Click(Me, New EventArgs)
                        txtLotNo_Num_Man.Focus()
                        Exit Function
                    End If
                End If
            End If
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            If Not tran Is Nothing Then tran.Dispose()
        End Try
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
        strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE 1=1 "
        If TAG_LOT_ALLOW_COSTID <> "" Then
            strSql += " AND COSTID IN('" & Replace(TAG_LOT_ALLOW_COSTID, ",", "','") & "')"
        End If
        If TAGTOBRANCH Then
            strSql += " AND COSTID NOT IN (SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE MAIN='Y')"
            strSql += " AND COSTID<>'EX'"
        End If
        strSql += " ORDER BY COSTNAME"
        objGPack.FillCombo(strSql, cmbCostCentre_Man, False)
        cmbCostCentre_Man.Text = ""
        If defaultItem <> "" Then
            cmbCostCentre_Man.Text = defaultItem
        End If
        If TAGTOBRANCH Then
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE "
            strSql += " WHERE COSTID=(SELECT TOP 1 COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE MAIN='Y')"
            cnHoCostname = objGPack.GetSqlValue(strSql, "COSTNAME", "")
        End If
    End Function

    'Function funcTagNoGen() As String
    '    Dim str As String
    '    Dim ds As New DataSet
    '    ds.Clear()
    '    ''Find LastTagNo
    '    If TagNoGen = "I" Then
    '        str = " select ctlText as TagNoFrom from " & cnAdminDb & "..SoftControl where ctlId = 'TagNoFrom'"
    '        da = New OleDbDataAdapter(str, cn)
    '        da.Fill(ds, "TagNoFrom")
    '        If ds.Tables("TagNoFrom").Rows.Count > 0 Then
    '            TagNoFrom = ds.Tables("TagNoFrom").Rows(0).Item("TagNoFrom")
    '        End If
    '        If TagNoFrom = "I" Then
    '            str = " select CurrentTagNo from " & cnAdminDb & "..itemMast where itemid = "
    '            str += " (select itemid from " & cnAdminDb & "..itemMast where itemName = '" & cmbItem_MAN.Text & "')"
    '            da = New OleDbDataAdapter(str, cn)
    '            da.Fill(ds, "LastTagNo")
    '            If ds.Tables("LastTagNo").Rows.Count > 0 Then
    '                LastTagNo = ds.Tables("LastTagNo").Rows(0).Item("CurrentTagNo")
    '                If LastTagNo = "" Then
    '                    LastTagNo = 0
    '                End If
    '            End If
    '        Else
    '            str = " Select ctlText as LastTagNo from " & cnAdminDb & "..SoftControl where ctlId = 'LastTagNo'"
    '            da = New OleDbDataAdapter(str, cn)
    '            da.Fill(ds, "LastTagNo")
    '            If ds.Tables("LastTagNo").Rows.Count > 0 Then
    '                LastTagNo = ds.Tables("LastTagNo").Rows(0).Item("LastTagNo")
    '            End If
    '        End If
    '    Else ''L--Lot Item Entry
    '        str = " select CurTagNo from " & cnAdminDb & "..ITEMLOT where itemid = "
    '        str += " (select itemid from " & cnAdminDb & "..itemMast where itemname = '" & cmbItem_MAN.Text & "')"
    '        str += " and lotno = '" & txtLotNo_Num_Man.Text & "' and entryorder = '" & entryOrder & "'"
    '        da = New OleDbDataAdapter(str, cn)
    '        da.Fill(ds, "LastTagNo")
    '        If ds.Tables("LastTagNo").Rows.Count > 0 Then
    '            LastTagNo = Val(ds.Tables("LastTagNo").Rows(0).Item("CurTagNo"))
    '        End If
    '    End If
    '    'If LastTagNo = "0" Then
    '    '    LastTagNo = 1
    '    'End If
    '    If IsNumeric(LastTagNo) = True Then
    '        LastTagNo = Val(LastTagNo) + 1
    '    Else
    '        Dim index As Integer = 0
    '        For Each c As Char In LastTagNo
    '            If Char.IsLetter(c) = True Then
    '                index += 1
    '            Else
    '                Exit For
    '            End If
    '        Next
    '        Dim fNo As String
    '        Dim sNo As String
    '        fNo = LastTagNo.Substring(0, index)
    '        sNo = LastTagNo.Substring(index, LastTagNo.Length - index)
    '        sNo = Val(sNo) + 1
    '        LastTagNo = fNo + sNo
    '    End If
    '    Return LastTagNo
    'End Function

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

    Private Sub frmItemTag_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ItemLock = False
    End Sub

    'Function funcGetTagNo() As String
    '    Dim str As String
    '    Dim ds As New DataSet
    '    ds.Clear()
    '    ''Find LastTagNo
    '    If TagNoGen = "I" Then
    '        str = " SELECT CTLTEXT AS TAGNOFROM FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'TAGNOFROM'"
    '        TagNoFrom = objGPack.GetSqlValue(str, "CTLTEXT", "", tran)
    '        If TagNoFrom = "I" Then
    '            str = " SELECT CURRENTTAGNO LASTTAGNO FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = "
    '            str += " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')"
    '        Else
    '            str = " SELECT CTLTEXT AS LASTTAGNO FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'LASTTAGNO'"
    '        End If
    '        LastTagNo = objGPack.GetSqlValue(str, "LASTTAGNO", "0", tran)
    '    Else ''L--Lot Item Entry
    '        str = " SELECT CURTAGNO AS LASTTAGNO FROM " & cnAdminDb & "..ITEMLOT WHERE ITEMID = "
    '        str += " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')"
    '        str += " AND LOTNO = '" & txtLotNo_Num_Man.Text & "' AND ENTRYORDER = '" & entryOrder & "'"
    '        LastTagNo = objGPack.GetSqlValue(str, "LASTTAGNO", "0", tran)
    '    End If
    '    If IsNumeric(LastTagNo) = True Then
    '        LastTagNo = Val(LastTagNo) + 1
    '    Else
    '        Dim index As Integer = 0
    '        For Each c As Char In LastTagNo
    '            If Char.IsLetter(c) = True Then
    '                index += 1
    '            Else
    '                Exit For
    '            End If
    '        Next
    '        Dim fNo As String
    '        Dim sNo As String
    '        fNo = LastTagNo.Substring(0, index)
    '        sNo = LastTagNo.Substring(index, LastTagNo.Length - index)
    '        sNo = Val(sNo) + 1
    '        LastTagNo = fNo + sNo
    '    End If
    '    Return LastTagNo
    'End Function

    Private Sub frmItemTag_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            ''STONE TAB FOCUS
            Select Case TabControl1.SelectedTab.Name
                Case tabMultiMetal.Name
                    If IsActive(gridMultimetal) Then
                        txtMMCategory.Select()
                    ElseIf IsActive(grpMultiMetal) Then
                        If dtMultiMetalDetails.Rows.Count > 0 And MultimetalValidWt Then
                            If Val(txtGrossWt_Wet.Text) <> Val(dtMultiMetalDetails.Compute("SUM(WEIGHT)", "").ToString) And Val(txtGrossWt_Wet.Text) > 0 Then
                                MsgBox("MultiMetal Weight Should Not Exceed Tag GrossWeight", MsgBoxStyle.Information)
                                txtMMCategory.Select()
                                Exit Select
                            End If
                        End If
                        If TabControl1.TabPages.Contains(tabStone) And (Not MetalBasedStone Or dtMultiMetalDetails.Rows.Count = 0) Then
                            TabControl1.SelectTab(tabStone)
                            If ACC_STUDITEM_POPUP Then txtStItem.Select() Else cmbStItem.Select()
                        ElseIf TabControl1.TabPages.Contains(tabOtherCharges) Then
                            TabControl1.SelectTab(tabOtherCharges)
                            txtMiscMisc.Select()
                        Else
                            TabControl1.SelectTab(tabTag)
                            Me.SelectNextControl(txtGrossWt_Wet, True, True, True, True)
                        End If
                    End If
                Case tabStone.Name
                    If IsActive(gridStone) Then
                        If ACC_STUDITEM_POPUP Then txtStItem.Select() Else cmbStItem.Select()
                    ElseIf (IsActive(grpStoneDetails)) Then
                        If MetalBasedStone And Val(txtMMWeight_Wet.Text) > 0 Then
                            TabControl1.SelectTab(tabMultiMetal)
                            If dtStoneDetails.Rows.Count > 0 Then
                                Dim _tempstnwt As Decimal = 0
                                For Each rsm As DataRow In dtStoneDetails.Rows
                                    If rsm("MKEYNO").ToString <> "" And txtMMRowIndex.Text = "" Then Continue For
                                    Dim _tmmkeyno As Integer = 0
                                    If txtMMRowIndex.Text <> "" Then
                                        _tmmkeyno = dtMultiMetalDetails.Rows(Val(txtMMRowIndex.Text)).Item("KEYNO").ToString
                                    End If
                                    If Val(rsm("MKEYNO").ToString) <> Val(_tmmkeyno) And rsm("MKEYNO").ToString <> "" Then Continue For
                                    If rsm("UNIT").ToString = "C" Then
                                        _tempstnwt += Math.Round(Val(Val(rsm("WEIGHT").ToString) / 5), DiaRnd)
                                    Else
                                        _tempstnwt += Val(rsm("WEIGHT").ToString)
                                    End If
                                Next
                                txtMMNetwt_Wet.Text = Val(txtMMWeight_Wet.Text) - Val(_tempstnwt)
                            Else
                                txtMMNetwt_Wet.Text = Val(txtMMWeight_Wet.Text)
                            End If
                            txtMMNetwt_Wet.Select()
                            Exit Select
                        ElseIf MetalBasedStone And Val(txtMMWeight_Wet.Text) > 0 Then
                            txtMMNetwt_Wet.Text = Val(txtMMWeight_Wet.Text)
                            txtMMNetwt_Wet.Select()
                            Exit Select
                        End If
                        If TabControl1.TabPages.Contains(tabOtherCharges) Then
                            TabControl1.SelectTab(tabOtherCharges)
                            txtMiscMisc.Select()
                        Else
                            TabControl1.SelectTab(tabTag)
                            Me.SelectNextControl(txtGrossWt_Wet, True, True, True, True)
                        End If
                    End If
                Case tabOtherCharges.Name
                    If IsActive(gridMisc) Then
                        txtMiscMisc.Select()
                    ElseIf IsActive(grpOtherCharges) Then
                        TabControl1.SelectTab(tabTag)
                        Me.SelectNextControl(txtGrossWt_Wet, True, True, True, True)
                    End If
                Case TabHallmark.Name
                    If IsActive(gridHallmarkDetails) Then
                        txtTagWt_WET.Focus()
                        txtTagWt_WET.SelectAll()
                    ElseIf IsActive(grpHallmarkDetails) Then
                        TabControl1.SelectTab(tabTag)
                        Me.SelectNextControl(txtHmCentre, True, True, True, True)
                    End If
                    'Case tabPurchase.Name
                    '    TabControl1.SelectedTab = tabTag
                    '    Me.SelectNextControl(txtSalValue_Amt_Man, True, True, True, True)
            End Select
        End If
    End Sub

    Private Sub frmItemTag_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If CmbStColor.Focused Then Exit Sub
            If CmbStClarity.Focused Then Exit Sub
            If CmbStShape.Focused Then Exit Sub
            If CmbStSize.Focused Then Exit Sub
            If cmbItem_MAN.Focused Then Exit Sub
            If gridStone.Focused Then Exit Sub
            If gridMisc.Focused Then Exit Sub
            If gridMultimetal.Focused Then Exit Sub
            If txtLotNo_Num_Man.Focused Then Exit Sub
            If txtGrossWt_Wet.Focused Then Exit Sub
            If txtMMAmount_AMT.Focused Then Exit Sub
            If txtStAmount_Amt.Focused Then Exit Sub
            If txtstPackettno.Focused Then Exit Sub
            If txtStItem.Focused Then Exit Sub
            If txtStSubItem.Focused Then Exit Sub
            If txtMaxMkCharge_Amt.Focused Then Exit Sub
            If txtMiscAmount_Amt.Focused Then Exit Sub
            If txtSalValue_Amt_Man.Focused Then Exit Sub
            ''MULTIMETAL    
            If txtMMCategory.Focused Then Exit Sub
            ''misc
            If txtMiscMisc.Focused Then Exit Sub
            If cmbStItem.Focused Then Exit Sub
            If CmbStSubItem.Focused Then Exit Sub
            'If txtRate_Amt.Enabled = True And NEEDUS = True And txtNetWt_Wet.Focused Then Exit Sub
            'If txtStRate_Amt.Enabled = True And NEEDUS = True And cmbStCalc.Focused Then Exit Sub
            'If NEEDUS = True And Studded_Loose = "L" And txtMetalRate_Amt.Focused And Val(txtMetalRate_Amt.Text) = 0 And Not (calType = "R" Or calType = "M") Then
            '    e.Handled = True
            '    'txtGrossWt_Wet.Focus()
            '    Me.SelectNextControl(txtMetalRate_Amt, True, True, True, True)
            '    Exit Sub
            'End If

            If NEEDUS = True And txtRate_Amt.Focused And Val(txtRate_Amt.Text) = 0 And calType = "M" Then
                strSql = " SELECT ISNULL(STUDDED,'') STUDDED FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') ='" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "' AND METALID = 'D'"
                Studded_Loose = objGPack.GetSqlValue(strSql, "STUDDED", "")
                If Studded_Loose = "L" Then
                    ObjRsUs.Label1.Focus()
                    Exit Sub
                End If
            End If

            If NEEDUS = True And txtStRate_Amt.Focused And Val(txtStRate_Amt.Text) = 0 Then
                strSql = " SELECT ISNULL(STUDDED,'') STUDDED FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') ='" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "' AND METALID = 'D'"
                Studded_Loose = objGPack.GetSqlValue(strSql, "STUDDED", "")
                If Studded_Loose <> "" Then
                    ObjRsUs.Label1.Focus()
                    Exit Sub
                End If
            End If

            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.F2) Then
            If IsActive(grpStoneDetails) Then
                gridStone.Select()
            End If
        End If
    End Sub

    Private Sub StyleGridMultiMetal()
        Dim _temploc As Integer = 0
        With gridMultimetal
            .Columns("CATEGORY").Width = txtMMCategory.Width + 1
            .Columns("RATE").Visible = False
            With .Columns("WEIGHT")
                .HeaderText = "WEIGHT"
                .Width = txtMMWeight_Wet.Width + 1
                .DefaultCellStyle.Format = "0.000"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With

            If MetalBasedStone Then
                With .Columns("NETWT")
                    .HeaderText = "NETWT"
                    .Width = txtMMNetwt_Wet.Width + 1
                    .DefaultCellStyle.Format = "0.000"
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End With
            Else
                _temploc = Val(txtMMWastagePer_PER.Location.X) - Val(txtMMNetwt_Wet.Location.X)
                lblMMWastPer.Left = Val(lblMMWastPer.Location.X) - Val(_temploc)
                txtMMWastagePer_PER.Left = Val(txtMMWastagePer_PER.Location.X) - Val(_temploc)
                lblMMwastage.Left = Val(lblMMwastage.Location.X) - Val(_temploc)
                txtMMWastage_WET.Left = Val(txtMMWastage_WET.Location.X) - Val(_temploc)
                lblMMMcGrm.Left = Val(lblMMMcGrm.Location.X) - Val(_temploc)
                txtMMMcPerGRm_AMT.Left = Val(txtMMMcPerGRm_AMT.Location.X) - Val(_temploc)
                lblMMMc.Left = Val(lblMMMc.Location.X) - Val(_temploc)
                txtMMMc_AMT.Left = Val(txtMMMc_AMT.Location.X) - Val(_temploc)
                lblMMFixed.Left = Val(lblMMFixed.Location.X) - Val(_temploc)
                txtMMAmount_AMT.Left = Val(txtMMAmount_AMT.Location.X) - Val(_temploc)
            End If
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
    Private Sub stHide()
        Dim txtWid As Integer = 60
        lblItem.Left -= txtWid
        lblSubItem.Left -= txtWid
        lblColor.Left -= txtWid
        lblClarity.Left -= txtWid
        lblShape.Left -= txtWid
        lblSize.Left -= txtWid
        If STUD_STNWTPER Then lblWPer.Left -= txtWid 'Else lblWPer.Left += txtWid If STUD_STNWTPER Then 
        lblPcs.Left -= txtWid
        lblWt.Left -= txtWid
        lblUnit.Left -= txtWid
        lblCalcType.Left -= txtWid
        lblRate.Left -= txtWid
        lblAmt.Left -= txtWid
        txtStItem.Left -= txtWid
        txtStSubItem.Left -= txtWid
        cmbStItem.Left -= txtWid
        CmbStSubItem.Left -= txtWid
        CmbStColor.Left -= txtWid
        CmbStClarity.Left -= txtWid
        CmbStShape.Left -= txtWid
        CmbStSize.Left -= txtWid
        If STUD_STNWTPER Then txtStWPer_AMT.Left -= txtWid 'Else txtStWPer_AMT.Left += txtWid If STUD_STNWTPER Then 
        txtStPcs_Num.Left -= txtWid
        txtStWeight.Left -= txtWid
        cmbStCalc.Left -= txtWid
        cmbStUnit.Left -= txtWid
        txtStRate_Amt.Left -= txtWid
        txtStAmount_Amt.Left -= txtWid
    End Sub
    Private Sub StyleGridStone()
        With gridStone
            If STUD_STNWTPER Then
                lblWPer.Visible = True
                txtStWPer_AMT.Visible = True
                txtStWeight.Enabled = False
            Else
                lblWPer.Visible = False
                txtStWPer_AMT.Visible = False
            End If
            .Columns("PACKETNO").Width = txtstPackettno.Width
            If PacketNoEnable = "N" Then
                .Columns("PACKETNO").Visible = False
                txtstPackettno.Visible = False
                lblstPacketno.Visible = False
                'pnlStDet.AutoSize = True
                'pnlStDet.Location = New Point(12, 11)
                'pnlStoneGrid.Size = New Size(944, 155)
            Else
                .Columns("PACKETNO").Visible = True
                txtstPackettno.Visible = True
                lblstPacketno.Visible = True
            End If
            .Columns("ITEM").Width = txtStItem.Width
            .Columns("SUBITEM").Width = txtStSubItem.Width
            .Columns("UNIT").Width = cmbStUnit.Width
            .Columns("CALC").Width = cmbStCalc.Width
            .Columns("WPER").Visible = STUD_STNWTPER
            If STUD_STNWTPER Then .Columns("WPER").Width = txtStWPer_AMT.Width
            If STUD_STNWTPER Then .Columns("WPER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PCS").Width = txtStPcs_Num.Width
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("WEIGHT").Width = txtStWeight.Width
            .Columns("WEIGHT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RATE").Width = txtStRate_Amt.Width
            .Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("AMOUNT").Width = txtStAmount_Amt.Width
            .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("METALID").Visible = False
            .Columns("KEYNO").Visible = False
            .Columns("MKEYNO").Visible = False
            .Columns("MSNO").Visible = False
            If _4C Then
                .Columns("COLOR").Width = CmbStColor.Width
                .Columns("COLOR").Visible = True
                .Columns("CLARITY").Width = CmbStClarity.Width
                .Columns("CLARITY").Visible = True
                .Columns("SHAPE").Width = CmbStShape.Width
                .Columns("SHAPE").Visible = True
                .Columns("STNSIZE").Width = CmbStSize.Width
                .Columns("STNSIZE").Visible = True
                strSql = "SELECT COUNT(*) FROM " & cnAdminDb & "..ITEMMAST WHERE MAINTAIN4C='Y'"
                If Val(objGPack.GetSqlValue(strSql)) > 0 Then
                    strSql = "SELECT TOP 1 VIEW4C FROM " & cnAdminDb & "..ITEMMAST WHERE VIEW4C <> '' ORDER BY LEN(VIEW4C) DESC"
                    Dim View4C As String = objGPack.GetSqlValue(strSql)
                    If Not View4C.Contains("SI") Then .Columns("STNSIZE").Visible = False
                    If Not View4C.Contains("CL") Then .Columns("CLARITY").Visible = False
                    If Not View4C.Contains("CO") Then .Columns("COLOR").Visible = False
                    If Not View4C.Contains("SH") Then .Columns("SHAPE").Visible = False
                End If
            Else
                .Columns("STNSIZE").Visible = False
                .Columns("CLARITY").Visible = False
                .Columns("COLOR").Visible = False
                .Columns("SHAPE").Visible = False
            End If
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
            .Columns("MKEYNO").Visible = False
            .Columns("MSNO").Visible = False
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
    Private Sub StyleHallmarkDet()
        With gridHallmarkDetails
            With .Columns("GRSWT")
                .HeaderText = "GRSWT"
                .Width = txtTagWt_WET.Width
                .DefaultCellStyle.Format = "0.000"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("HM_BILLNO")
                .HeaderText = "HALLMARK"
                .Width = txtHallmarkNo.Width
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            .Columns("PCS").Visible = False
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
    Private Sub frmItemTag_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        _IsWholeSaleType = W_Sale
        chkAutomaticWt.Checked = checkAutoWeight
        chkBarcodePrint.Checked = checkBarCode
        chkGuaranteeCard.Checked = checkGuaranteeCard
        If GetAdmindbSoftValue("HIDE-REF_VALUE", "Y") = "Y" Then
            'Label48.Size = New Size(1, Label48.Size.Height)
            'Label48.BackColor = Drawing.Color.Transparent
            txtRefVal_AMT.Visible = False
            'Label62.Text = " Purchase Value"
            txtSalValue_Amt_Man.Size = New Size(104, 21)
        Else
            'Label48.BackColor = Drawing.Color.Transparent
            'Label48.Size = New Size(1, Label48.Size.Height)
            txtRefVal_AMT.Visible = True
            'Label62.Text = "Pur Val"
            txtSalValue_Amt_Man.Size = New Size(66, 21)
        End If
        If NeedTag_Hsncode Then
            txtHSN.Enabled = True
        Else
            txtHSN.Enabled = False
        End If

        If NeedOldTag_Recdate Then
            chkOldTagRecdate.Visible = True
            dtpOldTagRecDate_OWN.Visible = True
        Else
            chkOldTagRecdate.Visible = False
            dtpOldTagRecDate_OWN.Visible = False
        End If

        If GetAdmindbSoftValue("HIDE-WTMACHINEDET", "Y") = "Y" Then
            grpWtMachingDet.Visible = False
        Else
            grpWtMachingDet.Visible = True
        End If
        If PurVal_Disable Then txtPurchaseValue_Amt.Visible = False : Label62.Visible = False
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
        SerialPort1.DataBits = Port_DataBit
        SerialPort1.BaudRate = Port_BaudRate
        SerialPort1.PortName = Port_PortName
        SerialPort1.Parity = GetParity(Port_Parity)
        objPropertyDia = New PropertyDia(SerialPort1)
        If tagEdit Then Exit Sub

        pnlSearch.Location = New Point(783, 16)
        pnlSearch.Size = New Size(209, 394)
        Me.Controls.Add(pnlSearch)
        pnlSearch.BringToFront()
        txtNarration.CharacterCasing = CharacterCasing.Normal
        'Me.WindowState = FormWindowState.Maximized
        pnlMain.BorderStyle = BorderStyle.None
        pnlMultiMetal.BackColor = Drawing.Color.LightGoldenrodYellow
        pnlStoneGrid.BackColor = Drawing.Color.LightGoldenrodYellow
        pnlMisc.BackColor = Drawing.Color.LightGoldenrodYellow

        TabControl1.SelectTab(tabTag)
        Me.TabControl1_SelectedIndexChanged(Me, New EventArgs)
        'TabControl1.SelectedTab.Name = tabTag.Name
        If objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'MULTIMETALCALC'") = "Y" Then
            multiMetalCalc = True
        End If

        If Lotautopost Then costLock = False
        ''dtGridView
        gridView.BorderStyle = BorderStyle.None

        With dtGridView.Columns
            ''SUBITEM,ITEMSIZE,TAGNO,PIECES,GRSWEIGHT,LESSWEIGHT,NETWT,RATE,CALCMODE,TABLECODE
            .Add("ITEM", GetType(String))
            .Add("SUBITEM", GetType(String))
            .Add("TAGNO", GetType(String))
            .Add("PCS", GetType(Int16))
            .Add("GRSWT", GetType(Decimal))
            .Add("LESSWT", GetType(Decimal))
            .Add("NETWT", GetType(Decimal))
            .Add("WASTAGE", GetType(Decimal))
            .Add("MC", GetType(Decimal))
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
            .Columns("WASTAGE").Width = 70
            .Columns("WASTAGE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("MC").Width = 70
            .Columns("MC").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RATE").Width = 70
            .Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SALEVALUE").Width = 100
            .Columns("SALEVALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SALEVALUE").HeaderText = "SALE VALUE"
            .Columns("SIZE").Width = 200
        End With
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        If STUD_STNWTPER Then
            lblWPer.Visible = True
            txtStWPer_AMT.Visible = True
            txtStWeight.Enabled = False
        Else
            lblWPer.Visible = False
            txtStWPer_AMT.Visible = False
        End If
        ''MultiMetal
        With dtMultiMetalDetails.Columns
            .Add("CATEGORY", GetType(String))
            .Add("WEIGHT", GetType(Double))
            If MetalBasedStone = True Then
                .Add("NETWT", GetType(Double))
            End If
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

        ''Stone
        With dtStoneDetails.Columns
            .Add("PACKETNO", GetType(String))
            .Add("ITEM", GetType(String))
            .Add("SUBITEM", GetType(String))
            .Add("COLOR", GetType(String))
            .Add("CLARITY", GetType(String))
            .Add("SHAPE", GetType(String))
            .Add("STNSIZE", GetType(String))
            .Add("WPER", GetType(Decimal))
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
            .Add("SETTYPE", GetType(String))
            .Add("HEIGHT", GetType(Decimal))
            .Add("WIDTH", GetType(Decimal))
            .Add("STNGRPID", GetType(Decimal))
            .Add("MKEYNO", GetType(Integer))
            .Add("MSNO", GetType(String))
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
        gridStone.Columns("STNSIZE").Visible = False
        gridStone.Columns("WPER").Visible = False
        gridStone.Columns("MKEYNO").Visible = False
        gridStone.Columns("MSNO").Visible = False
        StyleGridStone()
        dtStoneDetails.AcceptChanges()
        Dim dtStoneFooter As New DataTable
        dtStoneFooter = dtStoneDetails.Copy
        dtStoneFooter.Rows.Clear()
        dtStoneFooter.Rows.Add()
        dtStoneDetails.AcceptChanges()
        gridStoneFooter.DataSource = dtStoneFooter

        With gridStoneFooter
            '.DefaultCellStyle.BackColor = Color.Gainsboro
            '.DefaultCellStyle.SelectionBackColor = Color.Gainsboro
            .Rows(0).Cells("SUBITEM").Value = "TOTAL"
            For cnt As Integer = 0 To gridStone.ColumnCount - 1
                .Columns(cnt).Width = gridStone.Columns(cnt).Width
                .Columns(cnt).Visible = gridStone.Columns(cnt).Visible
                .Columns(cnt).DefaultCellStyle = gridStone.Columns(cnt).DefaultCellStyle
            Next
        End With
        ''OtherCharges
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
            '.DefaultCellStyle.BackColor = Color.Gainsboro
            '.DefaultCellStyle.SelectionBackColor = Color.Gainsboro
        End With

        ''HALLMARK DETAILS
        With dtHallmarkDetails.Columns
            .Add("GRSWT", GetType(Decimal))
            .Add("HM_BILLNO", GetType(String))
            .Add("PCS", GetType(Integer))
            .Add("KEYNO", GetType(Integer))
        End With
        dtHallmarkDetails.Columns("KEYNO").AutoIncrement = True
        dtHallmarkDetails.Columns("KEYNO").AutoIncrementStep = 1
        dtHallmarkDetails.Columns("KEYNO").AutoIncrementSeed = 1
        gridHallmarkDetails.DataSource = dtHallmarkDetails
        StyleHallmarkDet()

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


        cmbStUnit.Items.Add("C")
        cmbStUnit.Items.Add("G")
        cmbStUnit.Text = "C"
        cmbStCalc.Items.Add("W")
        cmbStCalc.Items.Add("P")
        cmbStCalc.Text = "W"

        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ISNULL(STUDDED,'') <> 'S' AND ACTIVE = 'Y' AND STOCKTYPE='T' ORDER BY ITEMNAME"
        objGPack.FillCombo(strSql, cmbItem_MAN, , False)


        ''CostCentre Checking..
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE'"
        If UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", "Y", tran)) = "N" Then
            cmbCostCentre_Man.Items.Clear()
            cmbCostCentre_Man.Text = ""
            cmbCostCentre_Man.Enabled = False
            chkFixedVa.Visible = False
        End If

        ' ''Min McWastage Visibility
        'strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'MINMCTAB'"
        'If UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", "Y", tran)) = "Y" Then pnlMin.Enabled = True Else pnlMin.Enabled = False

        ' ''Set MaxWastage Focus
        'strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'MAXMCFOCUS'"
        'If UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", "Y", tran)) = "Y" Then pnlMax.Enabled = True Else pnlMax.Enabled = False

        ''Set AttachImag btn Visible
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'ATTACHIMAGE'"
        If UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", "Y", tran)) = "Y" Then btnAttachImage.Enabled = True Else btnAttachImage.Enabled = False

        ''Sets default Paths
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICPATH'"
        defalutDestination = UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", , tran))
        If Not defalutDestination.EndsWith("\") And defalutDestination <> "" Then defalutDestination += "\"

        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'TAGCATALOGPATH'"
        CatalogDestination = UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", , tran))
        If Not CatalogDestination.EndsWith("\") And CatalogDestination <> "" Then CatalogDestination += "\"

        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICSOURCEPATH'"
        defalutSourcePath = UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", , tran))

        If cmbCounter_MAN.Enabled = True Then
            Dim dt As New DataTable
            dt.Clear()
            cmbCounter_MAN.Items.Clear()
            cmbCounter_MAN.Items.Add(" ")
            strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ISNULL(ACTIVE,'') <> 'N' ORDER BY DISPLAYORDER,ITEMCTRNAME"
            objGPack.FillCombo(strSql, cmbCounter_MAN, False)
            cmbCounter_MAN.Enabled = True
            cmbCounter_MAN.Text = ""
        Else
            cmbCounter_MAN.Enabled = False
            cmbCounter_MAN.Text = ""
        End If

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
        funcDiaDetails()
        If MetalBasedStone = True Then
            lblMMweight.Text = "GRSWT"
            lblMMNetwt.Visible = True
            txtMMNetwt_Wet.Visible = True
        Else
            lblMMweight.Text = "Weight"
            lblMMNetwt.Visible = False
            txtMMNetwt_Wet.Visible = False
        End If
        If funcNew() = 0 Then Exit Sub
        chkAutosnap()
        If TAGWISE_DISCOUNT Then
            lblDiscPer.Visible = True
            txtWDisc_Per.Visible = True
        Else
            txtNarration.Size = New Size(297, 21)
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
            StyleGridStone()
            With gridStoneFooter
                .Rows(0).Cells("SUBITEM").Value = "TOTAL"
                For cnt As Integer = 0 To gridStone.ColumnCount - 1
                    If .ColumnCount <= cnt And tagEdit Then
                        Continue For
                    End If
                    .Columns(cnt).Width = gridStone.Columns(cnt).Width
                    .Columns(cnt).Visible = gridStone.Columns(cnt).Visible
                    .Columns(cnt).DefaultCellStyle = gridStone.Columns(cnt).DefaultCellStyle
                Next
            End With
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
    Private Function GetPurchaseRate() As Double
        Dim Rate As Double = 0
        If _HasPurchase Then
            If RATE_FROM_WMCTABLE And (calType = "F" Or calType = "R") Then
                Rate = GetRateFromWmcTable(True)
            End If
        End If
        Return Rate
    End Function
    Private Sub LoadOrderDetails(ByVal ORDERNO As String, Optional ByVal mcostid As String = "")
        Dim dt As DataTable
        'strSql = " SELECT "
        'strSql += VBCRLF + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = O.ITEMID) AS ITEM"
        'strSql += VBCRLF + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = O.SUBITEMID) AS SUBITEM"
        'strSql += VBCRLF + " ,(SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZEID = O.SIZEID)AS SIZENAME"
        'strSql += VBCRLF + " ,PCS,GRSWT,NETWT,ORVALUE VALUE,SNO"
        'strSql += VBCRLF + " FROM " & cnadmindb & "..ORMAST O"
        'strSql += VBCRLF + " WHERE ORNO = '" & ORDERNO & "'"
        'strSql += VBCRLF + " AND SNO IN "
        'strSql += VBCRLF + " (SELECT ORSNO FROM " & cnadmindb & "..ORIRDETAIL AS I"
        'strSql += VBCRLF + " WHERE ORNO = '" & ORDERNO & "' AND NOT EXISTS (sELECT 1 FROM " & cnadmindb & "..ORIRDETAIL WHERE ORSTATUS = 'R' AND ORSNO = O.SNO AND ORDSTATE_ID = 4))"
        'strSql += VBCRLF + " AND ISNULL(ODBATCHNO,'') = ''  AND ISNULL(CANCEL,'') = ''"
        'strSql += VBCRLF + " ORDER BY ORNO,SNO"



        strSql = " SELECT "
        strSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = O.ITEMID) AS ITEM"
        strSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = O.SUBITEMID) AS SUBITEM"
        strSql += vbCrLf + " ,(SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZEID = O.SIZEID)AS SIZENAME"
        strSql += vbCrLf + " ,O.*"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ORMAST O"
        strSql += vbCrLf + " WHERE ISNULL(ORDCANCEL,'') = '' "
        If mcostid <> "" And CHKCOST_ORD = True Then strSql += vbCrLf + " AND COSTID='" & mcostid & "'"
        If Mid(ORDERNO, 6, 1) = "O" Then
            strSql += vbCrLf + " AND isnull(ORNO,'') ='" & ORDERNO & "'"
        Else
            strSql += vbCrLf + " AND SUBSTRING(ORNO, 6, 20) ='" & ORDERNO & "'"
        End If

        'AND SUBSTRING(ORNO,6,20) = '" & ORDERNO & "'"
        If ORDMULTIPCSCHK Then strSql += vbCrLf + " AND NOT EXISTS (sELECT 1 FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSTATUS = 'R' AND ORSNO = O.SNO AND ORDSTATE_ID = 4)"
        strSql += vbCrLf + " AND ISNULL(ODBATCHNO,'') = ''  "
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " ORDER BY ORNO,SNO"
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
        strSql += vbCrLf + " WHERE ISNULL(ORDCANCEL,'') <> 'Y' "
        If Mid(ORDERNO, 6, 1) = "O" Then
            strSql += vbCrLf + " AND isnull(ORNO,'') ='" & ORDERNO & "'"
        Else
            strSql += vbCrLf + " AND SUBSTRING(ORNO, 6, 20) ='" & ORDERNO & "'"
        End If

        'SUBSTRING(ORNO,6,20) = '" & ORDERNO & "'"
        strSql += vbCrLf + " AND SNO = '" & OrderRow.Item("SNO").ToString & "'"
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
            If ORDWTINTAG Then
                txtPieces_Num_Man.Text = IIf(Val(OrderRow.Item("PCS").ToString) > 0, Val(OrderRow.Item("PCS").ToString), "")
                txtGrossWt_Wet.Text = IIf(Val(OrderRow.Item("GRSWT").ToString) > 0, Format(Val(OrderRow.Item("GRSWT").ToString), "0.000"), "")
                txtNetWt_Wet.Text = IIf(Val(OrderRow.Item("NETWT").ToString) > 0, Format(Val(OrderRow.Item("NETWT").ToString), "0.000"), "")
            End If
            strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = "
            strSql += vbCrLf + " (SELECT TOP 1 DESIGNERID FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = '" & OrderRow.Item("SNO").ToString & "' AND ISNULL(CANCEL,'') = '' ORDER BY ENTRYORDER DESC)"
            cmbDesigner_MAN.Text = objGPack.GetSqlValue(strSql)
            If cmbDesigner_MAN.Text = "" Then
                strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = "
                strSql += vbCrLf + " (SELECT TOP 1 DESIGNERID FROM " & cnAdminDb & "..ITEMLOT WHERE LOTNO='" & txtLotNo_Num_Man.Text & "' AND ENTRYTYPE='OR' AND SNO = '" & SNO & "' )"
                cmbDesigner_MAN.Text = objGPack.GetSqlValue(strSql)
            End If
            strSql = " SELECT"
            strSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = STNITEMID)AS ITEM"
            strSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = STNSUBITEMID)AS SUBITEM"
            strSql += vbCrLf + " ,STNPCS PCS,STNWT WEIGHT,STNUNIT UNIT,CALCMODE CALC,STNRATE RATE,STNAMT AMOUNT"
            strSql += vbCrLf + " ,(SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = STNITEMID)METALID"
            strSql += vbCrLf + " ,NULL PURRATE,NULL PURVALUE,0 USRATE,0 INDRS"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ORSTONE"
            strSql += vbCrLf + " WHERE BATCHNO = '" & .Item("BATCHNO").ToString & "'"
            strSql += vbCrLf + " AND ORSNO = '" & .Item("SNO").ToString & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtStoneDetails)
            OrderDetail = txtGrossWt_Wet.Text & "," & txtNetWt_Wet.Text
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
        strSql += " CASE WHEN ENTRYTYPE = 'R' THEN 'REGULAR' WHEN ENTRYTYPE = 'OR' THEN 'ORDER' WHEN ENTRYTYPE = 'RE' THEN 'REPAIR' WHEN ENTRYTYPE = 'SR' THEN 'SALES RETURN' "
        strSql += " WHEN ENTRYTYPE = 'OO' THEN 'OLD' WHEN ENTRYTYPE = 'PS' THEN 'PARTLY SALE' WHEN ENTRYTYPE = 'AL' THEN 'ALTERATION' WHEN ENTRYTYPE = 'WO' THEN 'WORK ORDER' "
        strSql += " WHEN ENTRYTYPE = 'NT' THEN 'NONTAG TO TAG' ELSE 'LOT' END AS ENTRYTYPE ,"
        strSql += " LOT.STARTTAGNO,LOT.ENDTAGNO,LOT.CURTAGNO,LOT.PCS,LOT.GRSWT,LOT.NETWT,LOT.CPCS,LOT.CGRSWT,LOT.CNETWT"
        strSql += " ,(LOT.PCS-LOT.CPCS)AS BALPCS,(LOT.GRSWT-LOT.CGRSWT)AS BALGRSWT,(LOT.NETWT-LOT.CNETWT)AS BALNETWT ,"
        strSql += " ISNULL(IT.OTHCHARGE ,'')AS OTHCHARGE ,"
        strSql += " IT.METALID AS METALID ,IT.STUDDEDSTONE AS STUDDEDSTONE , IT.GROSSNETWTDIFF AS GROSSNETWTDIFF ,"
        strSql += " IT.SIZESTOCK AS SIZESTOCK , IT.MULTIMETAL AS MULTIMETAL , IT.CALTYPE AS CALTYPE ,"
        strSql += " (SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID =  (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE =  IT.CATCODE ))AS PURITY ,"
        strSql += " IT.NOOFPIECE AS NOOFPIECE , IT.PIECERATE AS PIECERATE , IT.STOCKTYPE AS STOCKTYPE ,"
        strSql += " WMCTYPE AS VALUEADDEDTYPE ,FINERATE,LOT.SALEPER,TUCH,WASTPER,MCGRM,LOT.OTHCHARGE ,"
        strSql += " ITT.NAME AS ITEMTYPE ,"
        strSql += " CASE WHEN ISNULL(IT.MCCALC,'N') = 'G' THEN 'GRS WT' ELSE 'NET WT' END AS MCCALC "
        strSql += " ,LOT.WASTDISCPER"
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
            issOrder = isOrder
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
                    strSql += " ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = I.DEFAULTCOUNTER)AS DEFAULTCOUNTER"
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
                        'If cmbCounter_MAN.Text = "" Then cmbCounter_MAN.Text = dtItemDet.Rows(0).Item("DEFAULTCOUNTER").ToString
                        If dtItemDet.Rows(0).Item("DEFAULTCOUNTER").ToString <> "" Then cmbCounter_MAN.Text = dtItemDet.Rows(0).Item("DEFAULTCOUNTER").ToString
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
                    txtLotNo_Num_Man.Focus()
                    Exit Function
                ElseIf .Item("StockType") = "P" Then
                    MsgBox(E0014, MsgBoxStyle.Exclamation)
                    Return 0
                    Exit Function
                End If
                If isOrder Then txtGrossWt_Wet.Text = .Item("GRSWT").ToString
                txtWDisc_Per.Text = Val(.Item("WASTDISCPER").ToString)
                cmbCostCentre_Man.Text = .Item("DefaultCostCentre").ToString
                If .Item("ValueAddedType") = "T" Or Assorted Then
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
                If Tagdesignedit And cmbDesigner_MAN.Text <> "" Then cmbDesigner_MAN.Enabled = False
                cmbCostCentre_Man.Text = .Item("DefaultCostCentre").ToString
                If Not Assorted Or cmbCounter_MAN.Text = "" Then cmbCounter_MAN.Text = .Item("DefaultCounter").ToString
                cmbItemType_MAN.Text = .Item("ITEMTYPE").ToString

                txtLotNo_Num_Man.Text = .Item("LotNo").ToString
                entryOrder = .Item("EntryOrder")

                dtpRecieptDate.Value = GetEntryDate(GetServerDate)
                dtpOldTagRecDate_OWN.Value = GetEntryDate(GetServerDate)

                lotRecieptDate = .Item("LotDate")
                If TAGTOBRANCH Then
                    If cmbCostCentre_Man.Text = cnHoCostname Then
                        cmbCostCentre_Man.Text = ""
                    End If
                End If
                If Not isOrder Then
                    cmbSubItem_Man.Items.Clear()
                    If objGPack.GetSqlValue("SELECT SUBITEM FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'") = "Y" Then
                        If PoNumber.Trim = "" Then
                            strSql = GetSubItemQry(New String() {"SUBITEMNAME"}, Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'")))
                        Else
                            strSql = "Select distinct SUBITEMNAME from " & cnAdminDb & "..SUBITEMMAST a"
                            strSql += vbCrLf + "Join " & cnAdminDb & "..PURCHASEORDER b on a.SUBITEMID = b.subitemid"
                            strSql += vbCrLf + "where PoNumber = '" & PoNumber & "'"
                        End If
                        objGPack.FillCombo(strSql, cmbSubItem_Man, False)
                        If .Item("SUBITEMNAME").ToString <> "" Then cmbSubItem_Man.Text = .Item("SUBITEMNAME").ToString
                        cmbSubItem_Man.Enabled = True
                    Else
                        If objGPack.GetSqlValue("SELECT SETITEM FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "' ") = "Y" Then
                            txtSetTagno.Enabled = True
                            CmbSetItem.Text = "YES"
                        End If
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
                    chkFixedVa.Checked = True
                    If objGPack.GetSqlValue("SELECT SETITEM FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "' ") = "Y" Then
                        txtSetTagno.Enabled = True
                        CmbSetItem.Text = "YES"
                    End If
                Else
                    chkFixedVa.Checked = False
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
                strSql = "SELECT ISNULL(STYLENO,'')STYLENO FROM " & cnAdminDb & "..ITEMLOT WHERE LOTNO =  '" & txtLotNo_Num_Man.Text & "' AND SNO = '" & SNO & "'"
                Stylecode = GetSqlValue(cn, strSql)
                If Stylecode <> "" Then
                    txtStyleCode.Text = Stylecode.ToString
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
                If PurTab_GrsNet Then
                    strSql = " SELECT GRSNET FROM " & cnStockDb & "..RECEIPT "
                    strSql += " WHERE SNO IN(SELECT RECSNO FROM " & cnStockDb & "..LOTISSUE "
                    strSql += " WHERE LOTSNO IN(SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE LOTNO='" & .Item("LotNo").ToString() & "'))"
                    PurGrsNet = objGPack.GetSqlValue(strSql, "GRSNET", "G").ToString
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

                ''NETWEIGHT
                lblNWLot.Text = IIf(Val(.Item("NETWT").ToString) <> 0, Format(Val(.Item("NETWT").ToString), "0.000"), "")
                lblNWCompleted.Text = IIf(Val(.Item("CNETWT").ToString) <> 0, Format(Val(.Item("CNETWT").ToString), "0.000"), "")
                lblNWBalance.Text = IIf(Val(.Item("BALNETWT").ToString) <> 0, Format(Val(.Item("BALNETWT").ToString), "0.000"), "")

                ''
                METALID = .Item("METALID").ToString
                studdedStone = .Item("StuddedStone").ToString
                grossnetdiff = .Item("GROSSNETWTDIFF").ToString
                sizeStock = .Item("SizeStock").ToString
                multiMetal = .Item("Multimetal").ToString
                OthCharge = .Item("OthCharge").ToString
                calType = .Item("CalType").ToString
                If cmbCalType.Enabled = True And NeedTag_CalcType Then
                    calType = Mid(cmbCalType.Text, 1, 1)
                Else
                    For cnt As Integer = 0 To cmbCalType.Items.Count - 1
                        If Mid(cmbCalType.Items(cnt).ToString, 1, 1) = .Item("Caltype").ToString Then
                            cmbCalType.Text = cmbCalType.Items(cnt).ToString
                            Exit For
                        End If
                    Next
                End If
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
                SalePer = Val(.Item("SALEPER").ToString)
                ObjPurDetail.txtSaleRate_PER.Text = Val(.Item("SALEPER").ToString)
                ObjPurDetail.txtPurTouch_Amt.Text = IIf(Val(.Item("TUCH").ToString) <> 0, Format(Val(.Item("TUCH").ToString), "0.00"), Nothing)
                ObjPurDetail.txtPurWastage_Per.Text = IIf(Val(.Item("WASTPER").ToString) <> 0, Format(Val(.Item("WASTPER").ToString), "0.00"), Nothing)
                ObjPurDetail.txtPurMcPerGrm_Amt.Text = IIf(Val(.Item("MCGRM").ToString) <> 0, Format(Val(.Item("MCGRM").ToString), "0.00"), Nothing)
                If calType = "R" And purRate <> 0 And Val(.Item("SALEPER").ToString) <> 0 Then txtRate_Amt.Text = purRate + (purRate * (Val(ObjPurDetail.txtSaleRate_PER.Text) / 100))

                strSql = " SELECT STONEUNIT FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "' AND STUDDED IN('L','B')"
                Dim Unit As String = objGPack.GetSqlValue(strSql, "STONEUNIT", "-1")
                If Unit = "-1" Then
                    CmbUnit.Text = ""
                    CmbUnit.Visible = False
                    txtGrossWt_Wet.Size = New Size(104, 21)
                Else
                    CmbUnit.Text = Unit
                    CmbUnit.Visible = True
                    txtGrossWt_Wet.Size = New Size(71, 21)
                End If

                'If PUR_LANDCOST Then
                '    If calType = "R" And purRate <> 0 And Val(.Item("SALEPER").ToString) <> 0 Then txtRate_Amt.Text = purRate + (purRate * (Val(ObjPurDetail.txtSaleRate_PER.Text) / 100))
                'End If


            End With
            If TAGDETDISABLE <> "" Then
                If TAGDETDISABLE.Contains("D") And cmbDesigner_MAN.Text <> "" Then cmbDesigner_MAN.Enabled = False
                If TAGDETDISABLE.Contains("C") And cmbCounter_MAN.Text <> "" Then cmbCounter_MAN.Enabled = False
                If TAGDETDISABLE.Contains("I") And cmbItem_MAN.Text <> "" Then cmbItem_MAN.Enabled = False
                If TAGDETDISABLE.Contains("S") And cmbSubItem_Man.Text <> "" Then cmbSubItem_Man.Enabled = False
                If TAGDETDISABLE.Contains("T") And cmbItemType_MAN.Text <> "" Then cmbItemType_MAN.Enabled = False
            End If
        Else
            If TAGWOLOT = False Then cmbItem_MAN.Text = ""
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
        If TAGEDITCOSTCENTRE = False Then
            cmbCostCentre_Man.Enabled = False
        Else
            cmbCostCentre_Man.Enabled = True
        End If
        Return 1
    End Function

    Private Sub txtLotNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLotNo_Num_Man.GotFocus
        setItem = False
        TabControl1.SelectedTab = tabTag
    End Sub

    Private Sub txtLotNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtLotNo_Num_Man.KeyDown
        Dim mLotchk As String = ""
        If e.KeyCode = Keys.Insert Then

            If Lotchkdate = True Then

                objdatescr.Label1.Text = "Lot Receipt Date"
                objdatescr.ShowDialog()
                mLotchk = " AND LOTDATE ='" & objdatescr.dtpBillDatef.Value.ToString("yyyy-MM-dd") & "' "
            End If

            strSql = " SELECT "
            strSql += vbCrLf + " LOTNO,LOTDATE"
            If ragshow = True Then strSql += vbCrLf + " ,(SELECT CAPTION FROM " & cnAdminDb & "..RANGEMAST WHERE SNO=LOT.RANGESNO)RANGE"
            strSql += vbCrLf + " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = LOT.DESIGNERID)AS DESIGNER"
            strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS IM WHERE IM.ITEMID = LOT.ITEMID)AS ITEM"
            strSql += vbCrLf + " ,ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS SM WHERE SM.SUBITEMID = LOT.SUBITEMID),'')AS SUBITEM"
            strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = LOT.COSTID)AS COSTCENTRE"
            strSql += vbCrLf + " ,PCS,GRSWT,CPCS AS CPCS,CGRSWT AS CGRSWT,(PCS-CPCS)AS BALPCS,(GRSWT-CGRSWT)AS BALGRSWT"
            strSql += vbCrLf + " ,(SELECT (CASE WHEN STOCKTYPE = 'T' THEN 'TAGED' WHEN STOCKTYPE = 'N' THEN 'NON TAGED' ELSE 'PACKET BASED' END) FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = LOT.ITEMID)AS STOCKTYPE"
            strSql += vbCrLf + " ,CASE ENTRYTYPE WHEN 'R' THEN 'REGULAR' WHEN 'OR' THEN 'ORDER' WHEN 'RE' THEN 'ORDER' ELSE ENTRYTYPE END AS LOTTYPE"
            strSql += vbCrLf + " ,SNO"
            strSql += vbCrLf + " ,CASE WHEN ISNULL(STKTYPE,'T')='M' THEN 'MANUFACTURING' WHEN ISNULL(STKTYPE,'T')='E' THEN 'EXEMPTED' ELSE 'TRADING' END AS STKTYPE"
            strSql += vbCrLf + " ,FROMITEMID"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMLOT AS LOT"
            strSql += vbCrLf + " WHERE "

            If tabCheckBy = "P" Then
                strSql += vbCrLf + " PCS > CPCS "
            ElseIf tabCheckBy = "E" Then
                strSql += vbCrLf + " ((GRSWT > CGRSWT) OR ( PCS > CPCS))"
            Else
                strSql += vbCrLf + " ((GRSWT > CGRSWT) or (rate <> 0 and pcs > cpcs))"
            End If
            If mLotchk <> "" Then strSql += vbCrLf + mLotchk
            strSql += vbCrLf + " AND ISNULL(COMPLETED,'') <> 'Y'"
            strSql += vbCrLf + " AND (SELECT STOCKTYPE FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = LOT.ITEMID) = 'T'"
            strSql += vbCrLf + " AND LOTNO LIKE '" & txtLotNo_Num_Man.Text & "%'"
            strSql += vbCrLf + " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T')"
            strSql += vbCrLf + " AND ISNULL(BULKLOT,'') <> 'Y'"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') <> 'Y'"
            If SPECIFICFORMAT.ToString = "1" Then
                strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COMPANYID LIKE '%" & strCompanyId & "%')"
                strSql += vbCrLf + " AND LOT.COMPANYID='" & strCompanyId & "'"
            End If
            strSql += vbCrLf + " ORDER BY LOTDATE,LOTNO "
            SNO = BrighttechPack.SearchDialog.Show("Searching LotNo", strSql, cn, 1, IIf(ragshow, 15, 14))
            strSql = " SELECT LOTNO,STKTYPE FROM " & cnAdminDb & "..ITEMLOT"
            strSql += vbCrLf + " WHERE SNO = '" & SNO & "'"
            Dim dt As DataTable
            dt = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                txtLotNo_Num_Man.Text = dt.Rows(0).Item("LotNo").ToString
                If dt.Rows(0).Item("STKTYPE").ToString = "M" Then
                    lblStkType.Text = "Manufacturing"
                ElseIf dt.Rows(0).Item("STKTYPE").ToString = "E" Then
                    lblStkType.Text = "Exempted"
                Else
                    lblStkType.Text = "Trading"
                End If
                lblStkType.Visible = True
            Else
                Exit Sub
            End If
            If LoadLotDetails() = False Then Exit Sub
            txtLotNo_Num_Man.Enabled = False
            strSql = "SELECT RECSNO FROM " & cnStockDb & "..LOTISSUE WHERE LOTSNO='" & SNO & "'"
            da = New OleDbDataAdapter(strSql, cn)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                Recsno = dt.Rows(0).Item("RECSNO").ToString
                If TAG_STONEAUTOLOAD_MR Then
                    funcReceiptStoneDetailAutoLoad(Recsno)
                End If
            End If
            funcReceiptMisCharges(Recsno)
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
            strSql = " SELECT ORDREPNO FROM " & cnAdminDb & "..ITEMLOT WHERE ISNULL(LOTNO,'') = '" & txtLotNo_Num_Man.Text & "'"
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

                If BrighttechPack.GetSqlValue(cn, strSql, "ORNO", "") = "" And ORDMULTIPCSCHK Then
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
        If MANUAL_TAGNO Then Exit Sub
        'txtTagNo__Man.Text = funcGetTagNo()
        strSql = " SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"
        Dim MetalId As String = objGPack.GetSqlValue(strSql)

        If MetalId = "S" And TAGNO_SIZEBASED And cmbItemSize.Text <> "" Then
            strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"
            Dim ItemId As String = objGPack.GetSqlValue(strSql)
            strSql = " SELECT TOP 1 SHORTNAME FROM " & cnAdminDb & "..ITEMSIZE WHERE ITEMID=" & ItemId & " AND SIZENAME='" & cmbItemSize.Text & "'"
            StrCaption = objGPack.GetSqlValue(strSql, , "", tran)
            If StrCaption = "" Then
                If Not tran Is Nothing Then tran.Rollback()
                MsgBox("Please set Range...", MsgBoxStyle.Information) : txtGrossWt_Wet.Focus() : Exit Sub
            End If
            strSql = " SELECT TOP 1 TAGNO FROM " & cnAdminDb & "..ITEMSIZE WHERE ITEMID=" & ItemId & " AND SIZENAME='" & cmbItemSize.Text & "'"
            StrTagNo = (Val(objGPack.GetSqlValue(strSql, , "", tran)) + 1).ToString
ReChk:
            strSql = " SELECT 1 FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO='" & StrCaption.ToString.Trim & "-" & StrTagNo.ToString.Trim & "' "
            If Val(objGPack.GetSqlValue(strSql, , "", tran)) > 0 Then
                StrTagNo = (Val(StrTagNo.ToString) + 1).ToString
                GoTo ReChk
            End If
            txtTagNo__Man.Text = StrCaption.ToString.Trim & "-" & StrTagNo.ToString.Trim
        Else
            txtTagNo__Man.Text = objTag.GetTagNo(dtpRecieptDate.Value.ToString("yyyy-MM-dd"), cmbItem_MAN.Text, SNO)
        End If
        '        txtTagNo__Man.Text = GetTagNo()
        'TAGDUPCHECK:
        '        ''TAGNO
        '        If GetSoftValue("TAGNOFROM") = "I" Then
        '            strSql = "SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAG"
        '            strSql += " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "') "
        '            strSql += " AND TAGNO = '" & txtTagNo__Man.Text & "'"
        '        Else 'UNIQUE
        '            strSql = "SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAG"
        '            strSql += " WHERE TAGNO = '" & txtTagNo__Man.Text & "'"
        '        End If
        '        If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
        '            Dim prefix As String = GetSoftValue("TAGPREFIX")
        '            If prefix.Length > 0 Then
        '                txtTagNo__Man.Text = GenTagNo(txtTagNo__Man.Text.Replace(prefix, ""))
        '            Else
        '                txtTagNo__Man.Text = GenTagNo(txtTagNo__Man.Text)
        '            End If

        '            GoTo TAGDUPCHECK
        '        End If
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
                If .Item("mcasvaper").ToString = "Y" Then Label44.Text = "MC PERCENT" Else Label44.Text = "Max Mc Per Grm"
                If tagEdit = False Then
                    For cnt As Integer = 0 To cmbCalType.Items.Count - 1
                        If Mid(cmbCalType.Items(cnt).ToString, 1, 1) = .Item("Caltype").ToString Then
                            cmbCalType.Text = cmbCalType.Items(cnt).ToString
                            Exit For
                        End If
                    Next
                ElseIf cmbCalType.Enabled = True Then
                    calType = Mid(cmbCalType.Text, 1, 1)
                End If

            End With
            funcAssignTabControls()
            CalcMaxMinValues()
        End If
        strSql = " SELECT STONEUNIT FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "' AND STUDDED IN('L','B')"
        Dim Unit As String = objGPack.GetSqlValue(strSql, "STONEUNIT", "-1")
        If Unit = "-1" Then
            CmbUnit.Text = ""
            CmbUnit.Visible = False
            txtGrossWt_Wet.Size = New Size(104, 21)
        Else
            strSql = " SELECT STONEUNIT FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "' "
            strSql += " AND ITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME ='" & cmbItem_MAN.Text & "')"
            Unit = objGPack.GetSqlValue(strSql, "STONEUNIT")
            CmbUnit.Visible = True
            CmbUnit.Text = Unit
            txtGrossWt_Wet.Size = New Size(71, 21)
        End If
        If RATE_FROM_WMCTABLE And (calType = "F") Then txtMetalRate_Amt.Text = Format(GetMetalRate(), "0.00")
        If RATE_FROM_WMCTABLE And (calType = "R") Then
            txtRate_Amt.Text = GetRateFromWmcTable(False)
        Else
            txtRate_Amt.Text = ""
        End If
    End Sub

    Private Sub txtPieces_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPieces_Num_Man.GotFocus
        If AddStockEntry = True And tagEdit = True Then AddStockEntry = False : LoadAdditionalStockEntry()
        If EDITTAGPCS = True Then txtPieces_Num_Man.ReadOnly = False Else txtPieces_Num_Man.ReadOnly = True
        If REPEAT_TAGPCS And txtPieces_Num_Man.Text = "" Then
            If noOfPiece <> 0 Then
                txtPieces_Num_Man.Text = noOfPiece.ToString
            Else
                txtPieces_Num_Man.Text = "1"
            End If
        End If
        If REPEAT_TAGPCS = False Then
            If noOfPiece <> 0 Then
                txtPieces_Num_Man.Text = noOfPiece.ToString
            Else
                txtPieces_Num_Man.Text = "1"
            End If
        End If

        If TAGEDITPCSWT = "N" Then txtGrossWt_Wet.ReadOnly = True Else txtGrossWt_Wet.ReadOnly = False
        If TAGEDITPCSWT = "A" And Authorize Then txtGrossWt_Wet.ReadOnly = False
        If TAGEDITPCSWT = "A" And Not Authorize Then txtGrossWt_Wet.ReadOnly = True
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
        Dim gorn As String
        If cmbCalcMode.SelectedIndex = 0 Then ''GRS WT
            mweight = Val(txtGrossWt_Wet.Text)
            gorn = "G"
        Else ''NET WT
            mweight = Val(txtNetWt_Wet.Text)
            gorn = "N"
        End If
        If Not _WASTONGRSNET Then
            mweight = IIf(_MCCALCON_ITEM_GRS, Val(txtGrossWt_Wet.Text), Val(txtNetWt_Wet.Text))
            If _MCCALCON_ITEM_GRS Then gorn = "G"
        End If
        If Not OrderRow Is Nothing Then
            If Mid(OrderRow(1).ToString, 6, 1) = "R" And RepWastMcExOnly And OrderDetail <> "" Then
                Dim ordetailarr() As String = OrderDetail.Split(",")
                mweight = mweight - IIf(gorn = "G", Val(ordetailarr(0).ToString), Val(ordetailarr(1).ToString))
            End If
        End If
        wt = mweight * (Val(txtMaxWastage_Per.Text) / 100)
        wt = Math.Round(wt, WastageRound)
        If tagEdit And wt = 0 Then Exit Sub
        txtMaxWastage_Wet.Text = IIf(wt <> 0, Format(wt, "0.000"), "")
    End Sub

    Private Sub txtMaxMcPerGrm_Amt_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtMaxMcPerGrm_Amt.KeyPress
        If tagEdit Then txtMaxMcPerGrm_TextChanged(Me, New System.EventArgs)
    End Sub

    Private Sub txtMaxMcPerGrm_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMaxMcPerGrm_Amt.TextChanged
        Dim mc As Double = Nothing
        Dim wast As Double = IIf(McWithWastage, Val(txtMaxWastage_Wet.Text), 0)
        Dim mweight As Double = 0
        Dim gorn As String
        If _MCONGRSNET Then
            If cmbCalcMode.SelectedIndex = 0 Then ''GRS WT
                mweight = (Val(txtGrossWt_Wet.Text) + wast)
                gorn = "G"
            Else ''NET WT
                mweight = (Val(txtNetWt_Wet.Text) + wast)
                gorn = "N"
            End If
        Else
            mweight = IIf(_MCCALCON_ITEM_GRS, Val(txtGrossWt_Wet.Text), Val(txtNetWt_Wet.Text)) + wast
            If _MCCALCON_ITEM_GRS Then gorn = "G"
        End If
        strSql = " SELECT mcasvaper FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"
        If objGPack.GetSqlValue(strSql).ToString = "Y" Then Label44.Text = "MC PERCENT" Else Label44.Text = "Max Mc Per Grm"
        If cmbSubItem_Man.Text <> "" Then
            strSql = " SELECT mcasvaper FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & cmbSubItem_Man.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')"
            If objGPack.GetSqlValue(strSql).ToString = "Y" Then Label44.Text = "MC PERCENT" Else Label44.Text = "Max Mc Per Grm"
        End If

        If Not OrderRow Is Nothing Then
            If Mid(OrderRow(1).ToString, 6, 1) = "R" And RepWastMcExOnly And OrderDetail <> "" Then
                Dim ordetailarr() As String = OrderDetail.Split(",")
                mweight = mweight - IIf(gorn = "G", Val(ordetailarr(0).ToString), Val(ordetailarr(1).ToString))
            End If
        End If

        If Label44.Text = "MC PERCENT" Then mc = mweight * (Val(txtMetalRate_Amt.Text) * (Val(txtMaxMcPerGrm_Amt.Text) / 100)) Else mc = mweight * Val(txtMaxMcPerGrm_Amt.Text)
        mc = Math.Round(mc, McRnd)

        If tagEdit And mc = 0 Then Exit Sub
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
            If cmbCalType.Enabled Then
                calcType = Mid(cmbCalType.Text, 1, 1)
            End If
            If calcType <> "F" Then
                If SalVal_Lock = True Then
                    txtSalValue_Amt_Man.ReadOnly = True
                Else
                    txtSalValue_Amt_Man.ReadOnly = False
                End If
            End If
        End If
        If TAGEDITDISABLE.Contains("SV") And tagEdit Then txtSalValue_Amt_Man.ReadOnly = True
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

        Try
            tran = Nothing
            tran = cn.BeginTransaction()
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
                strSql = " SELECT SIZEID FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZENAME = '" & cmbItemSize.Text & "' AND ITEMID = " & itemId & ""
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
            ' ''Find SaleMode
            'strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID='" & itemId & "'"
            'cmd = New OleDbCommand(strSql, cn, tran)
            'dReader = cmd.ExecuteReader
            'If dReader.Read = True Then
            '    saleMode = dReader.Item("CalType")
            'End If
            'dReader.Close()
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
            strSql = "  SELECT * "
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMTAG WHERE SNO='" & updIssSno & "'"
            Dim rowEdit As DataRow = GetSqlRow(strSql, cn, tran)
            Dim OldGrsWt, OldMcGrm, oldMc, OldWastper, OldWast As Decimal
            Dim _EditOrNo As String = ""
            Dim _EditOrSNo As String = ""
            Dim _EditEntryType As String = ""
            If Not rowEdit Is Nothing Then
                OldGrsWt = Val(rowEdit("GRSWT").ToString)
                OldMcGrm = Val(rowEdit("MAXMCGRM").ToString)
                OldWastper = Val(rowEdit("MAXWASTPER").ToString)
                oldMc = Val(rowEdit("MAXMC").ToString)
                OldWast = Val(rowEdit("MAXWAST").ToString)
                _EditOrNo = rowEdit("ORDREPNO").ToString
                _EditOrSNo = rowEdit("ORSNO").ToString
                _EditEntryType = rowEdit("ENTRYTYPE").ToString
            End If

            strSql = " UPDATE " & cnAdminDb & "..EITEMTAG SET ISSDATE='" & Today.Date.ToString("yyyy-MM-dd") & "' WHERE SNO = '" & updIssSno & "' AND ISSDATE IS NULL"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            strSql = " UPDATE " & cnAdminDb & "..EITEMTAGSTONE SET ISSDATE='" & Today.Date.ToString("yyyy-MM-dd") & "' WHERE TAGSNO = '" & updIssSno & "' AND ISSDATE IS NULL"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

            strSql = "  INSERT INTO " & cnAdminDb & "..EITEMTAG"
            strSql += vbCrLf + " (SNO,RECDATE,COSTID,ITEMID,ORDREPNO,ORSNO,ORDSALMANCODE,SUBITEMID,SIZEID,ITEMCTRID,TABLECODE,DESIGNERID,TAGNO,SETTAGNO,PCS,GRSWT,LESSWT,NETWT,RATE,FINERATE"
            strSql += vbCrLf + " ,MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC,MINWASTPER,MINMCGRM,MINWAST,MINMC,TAGKEY,TAGVAL,LOTSNO,COMPANYID,SALVALUE,PURITY,NARRATION,DESCRIP,REASON"
            strSql += vbCrLf + " ,ENTRYMODE,GRSNET,ISSDATE,ISSREFNO,ISSPCS,ISSWT,FROMFLAG,TOFLAG,APPROVAL,SALEMODE,BATCHNO,MARK,VATEXM,PCTPATH,PCTFILE,OLDTAGNO,ITEMTYPEID,ACTUALRECDATE"
            strSql += vbCrLf + " ,WEIGHTUNIT,TRANSFERWT,CHKDATE,CHKTRAY,CARRYFLAG,BRANDID,PRNFLAG,MCDISCPER,WASTDISCPER,RESDATE,TRANINVNO,SUPBILLNO,WORKDAYS"
            strSql += vbCrLf + " ,USERID,UPDATED,UPTIME" & IIf(NeedTag_Hsncode = True, ",HSN", "")
            strSql += vbCrLf + " ,SYSTEMID,STYLENO,APPVER,TRANSFERDATE,TRANSFERED,BOARDRATE,RFID,CERTIFICATENO,TOUCH,HM_BILLNO,HM_CENTER,ADD_VA_PER,REFVALUE,VALUEADDEDTYPE,REFNO"
            strSql += vbCrLf + " ,REFDATE,TCOSTID,RECSNO,EXTRAWT,TAGWT,COVERWT,USRATE,INDRS,FROMITEMID,CUTID,COLORID,CLARITYID,SETTYPEID,SHAPEID,HEIGHT,WIDTH,TRFVALUE,TAGFIXEDVA,STKTYPE,ENTRYTYPE,RRECDATE)"
            strSql += vbCrLf + "  SELECT SNO,'" & Today.Date.ToString("yyyy-MM-dd") & "',COSTID,ITEMID,ORDREPNO,ORSNO,ORDSALMANCODE,SUBITEMID,SIZEID,ITEMCTRID,TABLECODE,DESIGNERID,TAGNO,SETTAGNO,PCS,GRSWT,LESSWT,NETWT,RATE,FINERATE"
            strSql += vbCrLf + " ,MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC,MINWASTPER,MINMCGRM,MINWAST,MINMC,TAGKEY,TAGVAL,LOTSNO,COMPANYID,SALVALUE,PURITY,NARRATION,DESCRIP,REASON"
            strSql += vbCrLf + " ,ENTRYMODE,GRSNET,ISSDATE,ISSREFNO,ISSPCS,ISSWT,FROMFLAG,TOFLAG,APPROVAL,SALEMODE,BATCHNO,MARK,VATEXM,PCTPATH,PCTFILE,OLDTAGNO,ITEMTYPEID,ACTUALRECDATE"
            strSql += vbCrLf + " ,WEIGHTUNIT,TRANSFERWT,CHKDATE,CHKTRAY,CARRYFLAG,BRANDID,PRNFLAG,MCDISCPER,WASTDISCPER,RESDATE,TRANINVNO,SUPBILLNO,WORKDAYS"
            strSql += vbCrLf + " ,'" & userId & "','" & Today.Date.ToString("yyyy-MM-dd") & "','" & Date.Now.ToLongTimeString & "'" & IIf(NeedTag_Hsncode = True, ",HSN", "")
            strSql += vbCrLf + " ,'" & systemId & "',STYLENO,APPVER,TRANSFERDATE,TRANSFERED,BOARDRATE,RFID,CERTIFICATENO,TOUCH,HM_BILLNO,HM_CENTER,ADD_VA_PER,REFVALUE,VALUEADDEDTYPE,REFNO"
            strSql += vbCrLf + " ,REFDATE,TCOSTID,RECSNO,EXTRAWT,TAGWT,COVERWT,USRATE,INDRS,FROMITEMID,CUTID,COLORID,CLARITYID,SETTYPEID,SHAPEID,HEIGHT,WIDTH,TRFVALUE,TAGFIXEDVA,STKTYPE,ENTRYTYPE,RRECDATE"
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMTAG WHERE SNO='" & updIssSno & "'"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

            strSql = " INSERT INTO " & cnAdminDb & "..EITEMTAGSTONE"
            strSql += vbCrLf + " (SNO,TAGSNO,ITEMID,COMPANYID,STNITEMID,STNSUBITEMID,TAGNO,STNPCS,STNWT,STNRATE,STNAMT"
            strSql += vbCrLf + " ,DESCRIP,RECDATE,CALCMODE,MINRATE,SIZECODE,STONEUNIT,ISSDATE,OLDTAGNO,VATEXM,CARRYFLAG"
            strSql += vbCrLf + " ,COSTID,SYSTEMID,APPVER,TRANSFERED,USRATE,INDRS,PACKETNO,CUTID,COLORID,CLARITYID,SETTYPEID"
            strSql += vbCrLf + " ,SHAPEID,HEIGHT,WIDTH,TRFVALUE"
            strSql += vbCrLf + " ,USERID)"
            strSql += vbCrLf + " SELECT SNO,TAGSNO,ITEMID,COMPANYID,STNITEMID,STNSUBITEMID,TAGNO,STNPCS,STNWT,STNRATE,STNAMT"
            strSql += vbCrLf + " ,DESCRIP,'" & Today.Date.ToString("yyyy-MM-dd") & "',CALCMODE,MINRATE,SIZECODE,STONEUNIT,ISSDATE,OLDTAGNO,VATEXM,CARRYFLAG"
            strSql += vbCrLf + " ,COSTID,SYSTEMID,APPVER,TRANSFERED,USRATE,INDRS,PACKETNO,CUTID,COLORID,CLARITYID,SETTYPEID"
            strSql += vbCrLf + " ,SHAPEID,HEIGHT,WIDTH,TRFVALUE"
            strSql += vbCrLf + " ,'" & userId & "' FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE  TAGSNO = '" & updIssSno & "'"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            '*******************INSERTING INTO E ITEMTAG

            If TAGWISE_DISCOUNT = False Then txtWDisc_Per.Text = 0

            strSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET "
            strSql += " RECDATE = '" & dtpRecieptDate.Value.Date.ToString("yyyy-MM-dd") & "'" 'RECDATE
            strSql += " ,COSTID = '" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
            If cmbSubItem_Man.Enabled = True Then
                strSql += " ,SUBITEMID = " & subitemId & "" 'SUBITEMID
            End If
            If cmbItemSize.Enabled = True Then
                strSql += " ,SIZEID = " & sizeId & "" 'SIZEID
            End If
            If cmbCounter_MAN.Enabled = True Then
                strSql += " ,ITEMCTRID = " & itemCtrId & "" 'ITEMCTRID
            End If
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
            strSql += " ,WASTDISCPER=" & Val(txtWDisc_Per.Text) & "" 'WASTDISCPER
            If txtRate_Amt.Enabled = True Then
                strSql += " ,RATE = " & Val(txtRate_Amt.Text) & "" 'RATE
            Else
                strSql += ",RATE = 0"
            End If
            If txtSetTagno.Enabled = True Then
                strSql += " ,SETTAGNO =  '" & txtSetTagno.Text.ToString.Trim & "'" 'SETTAGNO
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


            If NeedTag_UpdImg = True Then
                If picPath Is Nothing Or picPath = "" Then
                    If DefaultPctFile.ToString <> "" Then
                        Dim _tempdefpctfile As String = ""
                        _tempdefpctfile = DefaultPctFile.ToString
                        _tempdefpctfile = _tempdefpctfile.Replace("<ITEMID>", itemId.ToString)
                        _tempdefpctfile = _tempdefpctfile.Replace("<TAGNO>", txtTagNo__Man.Text.Replace(":", "").ToString)
                        strSql += " ,PCTFILE ='" & _tempdefpctfile.ToString & "'" 'pctfile
                    Else
                        strSql += " ,PCTFILE = ''"
                    End If
                Else
                    If DefaultPctFile.ToString <> "" Then
                        Dim _tempdefpctfile As String = ""
                        _tempdefpctfile = DefaultPctFile.ToString
                        _tempdefpctfile = _tempdefpctfile.Replace("<ITEMID>", itemId.ToString)
                        _tempdefpctfile = _tempdefpctfile.Replace("<TAGNO>", txtTagNo__Man.Text.Replace(":", "").ToString)
                        strSql += " ,PCTFILE ='" & _tempdefpctfile.ToString & "'" 'pctfile
                    Else
                        strSql += " ,PCTFILE = '" & IIf(picPath <> "", GetStockCompId() & "L" + txtLotNo_Num_Man.Text + "I" + itemId.ToString + "T" + txtTagNo__Man.Text.Replace(":", "") + IIf(picExtension.ToString.StartsWith("."), picExtension.ToString, "." + picExtension.ToString), picPath) & "'"
                    End If
                End If
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
            If RepairLot = True Then
                strSql += " ,OREXCESSWT = " & Val(ObjExtraWt.txtExtraWt_WET.Text) & "" 'OREXCESSWT
            Else
                strSql += " ,EXTRAWT = " & Val(ObjExtraWt.txtExtraWt_WET.Text) & "" 'EXTRAWT
            End If
            strSql += " ,TAGWT = " & Val(ObjTagWt.txtTagWt_WET.Text) & "" 'TAGWT
            strSql += " ,COVERWT = " & Val(ObjTagWt.txtCoverWt_WET.Text) & "" 'COVERWT
            strSql += " ,USRATE = " & IIf(NEEDUS = True And Studded_Loose = "L", Val(ObjRsUs.txtUSDollar_Amt.Text), 0) & "" 'USRATE
            strSql += " ,INDRS = " & IIf(NEEDUS = True And Studded_Loose = "L", Val(ObjRsUs.txtIndRs_Amt.Text), 0) & "" 'INDRS
            strSql += " ,WEIGHTUNIT = '" & Mid(CmbUnit.Text, 1, 1) & "'"
            If _FourCMaintain Then
                strSql += " ,STNGRPID=" & TagStnGrpId
                strSql += " ,CUTID=" & TagCutId
                strSql += " ,COLORID=" & TagColorId
                strSql += " ,CLARITYID=" & TagClarityId
                strSql += " ,SHAPEID=" & TagShapeId
                strSql += " ,SETTYPEID=" & TagSetTypeId
                strSql += " ,HEIGHT=" & TagHeight
                strSql += " ,WIDTH=" & TagWidth
            End If
            If NeedTag_Hsncode Then
                strSql += " ,HSN = '" & txtHSN.Text & "'" 'HSNCODE
            End If
            If NeedOldTag_Recdate And chkOldTagRecdate.Checked Then
                strSql += " RRECDATE = '" & dtpOldTagRecDate_OWN.Value.Date.ToString("yyyy-MM-dd") & "'" 'RRECDATE
            End If
            strSql += " WHERE SNO = '" & updIssSno & "'"

            Dim fileDestPath As String = Nothing
            If File.Exists(picPath) = True Then
                Dim serverPath As String = Nothing
                If DefaultPctFile.ToString <> "" Then
                    Dim _tempdefpctfile As String = ""
                    _tempdefpctfile = DefaultPctFile.ToString
                    _tempdefpctfile = _tempdefpctfile.Replace("<ITEMID>", itemId.ToString)
                    _tempdefpctfile = _tempdefpctfile.Replace("<TAGNO>", txtTagNo__Man.Text.Replace(":", "").ToString)
                    fileDestPath = (defalutDestination + _tempdefpctfile.ToString)
                Else
                    fileDestPath = (defalutDestination + GetStockCompId() & "L" + txtLotNo_Num_Man.Text + "I" + itemId.ToString + "T" + txtTagNo__Man.Text.Replace(":", "") + IIf(picExtension.ToString.StartsWith("."), picExtension.ToString, "." + picExtension.ToString))
                End If

                Dim Finfo As FileInfo
                Finfo = New FileInfo(fileDestPath)
                If IO.Directory.Exists(Finfo.Directory.FullName) = False And NeedTag_UpdImg Then
                    MsgBox(Finfo.Directory.FullName & " Does not exist. Please make a corresponding path", MsgBoxStyle.Information)
                    tran.Rollback()
                    Exit Sub
                End If
                If UCase(picPath) <> fileDestPath.ToUpper And NeedTag_UpdImg Then
                    Dim cFile As New FileInfo(picPath)
                    cFile.CopyTo(fileDestPath, True)
                    'File.Copy(picPath, fileDestPath)
                End If
                If COPYIMAGETOCATALOGPATH And NeedTag_UpdImg Then
                    If DefaultPctFile.ToString <> "" Then
                        Dim _tempdefpctfile As String = ""
                        _tempdefpctfile = DefaultPctFile.ToString
                        _tempdefpctfile = _tempdefpctfile.Replace("<ITEMID>", itemId.ToString)
                        _tempdefpctfile = _tempdefpctfile.Replace("<TAGNO>", txtTagNo__Man.Text.Replace(":", "").ToString)
                        fileDestPath = (CatalogDestination + _tempdefpctfile.ToString)
                    Else
                        fileDestPath = (CatalogDestination + GetStockCompId() & "L" + txtLotNo_Num_Man.Text + "I" + itemId.ToString + "T" + txtTagNo__Man.Text.Replace(":", "") + IIf(picExtension.ToString.StartsWith("."), picExtension.ToString, "." + picExtension.ToString))
                    End If
                    If UCase(CatalogDestination + picPath) <> fileDestPath.ToUpper Then
                        Dim cFile As New FileInfo(picPath)
                        cFile.CopyTo(fileDestPath, True)
                    End If
                End If
            End If
            'cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId, , fileDestPath)
            TagCutId = 0 : TagColorId = 0 : TagClarityId = 0 : TagShapeId = 0 : TagSetTypeId = 0 : TagHeight = 0 : TagWidth = 0
            If _HasPurchase Or pFixedVa Or PUR_AUTOCALC Then
                ''INSERTING EPURITEMTAG
                strSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..EPURITEMTAG"
                strSql += vbCrLf + " ("
                strSql += vbCrLf + " TAGSNO,ITEMID,TAGNO,PURLESSWT,PURNETWT,PURRATE,PURGRSNET,PURWASTAGE,PURTOUCH"
                strSql += vbCrLf + " ,PURMC,PURVALUE,PURTAX,RECDATE,COMPANYID,COSTID,DESCRIPT,PURLCOSTPER"
                strSql += vbCrLf + " ,PURLCOST,PURSCOSTPER,UPDATED,UPTIME)"
                strSql += vbCrLf + " SELECT TAGSNO,ITEMID,TAGNO,PURLESSWT,PURNETWT,PURRATE,PURGRSNET,PURWASTAGE,PURTOUCH"
                strSql += vbCrLf + " ,PURMC,PURVALUE,PURTAX,RECDATE,COMPANYID,COSTID,DESCRIPT,PURLCOSTPER"
                strSql += vbCrLf + " ,PURLCOST,PURSCOSTPER"
                strSql += vbCrLf + " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + "  ,'" & Date.Now.ToLongTimeString & "' "
                strSql += vbCrLf + " FROM " & cnAdminDb & "..PURITEMTAG "
                strSql += vbCrLf + " WHERE TAGSNO = '" & updIssSno & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                ''DELETING PURITEMTAG
                strSql = " DELETE FROM " & cnAdminDb & "..PURITEMTAG"
                strSql += " WHERE TAGSNO = '" & updIssSno & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            End If
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
                strSql += vbCrLf + " ,DESCRIPT"
                strSql += vbCrLf + " ,PURLCOSTPER"
                strSql += vbCrLf + " ,PURLCOST"
                strSql += vbCrLf + " ,PURSCOSTPER"
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
                strSql += vbCrLf + " ,'" & ObjPurDetail.txtNarration.Text & "'"
                strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurLCostper_NUM.Text) & "" ' PURLCOSTPER"
                strSql += vbCrLf + " ," & Val(ObjPurDetail.txtPurLCost_NUM.Text) & "" ' PURLCOST"
                strSql += vbCrLf + " ," & Val(ObjPurDetail.txtSaleRate_PER.Text) & "" ' PURSCOSTPER"
                strSql += vbCrLf + " )"
                'cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            End If

            If _HasPurchase Or pFixedVa Or PUR_AUTOCALC Then
                ''INSERTING EPURSTONEDETAIL
                strSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..EPURITEMTAGSTONE"
                strSql += vbCrLf + " ("
                strSql += vbCrLf + " TAGSNO"
                strSql += vbCrLf + " ,ITEMID"
                strSql += vbCrLf + " ,TAGNO"
                strSql += vbCrLf + " ,STNITEMID,STNSUBITEMID,STNPCS,STNWT,STNRATE,STNAMT,STONEUNIT,CALCMODE"
                strSql += vbCrLf + " ,PURRATE"
                strSql += vbCrLf + " ,PURAMT,COMPANYID,COSTID,STNSNO,UPDATED,UPTIME"
                strSql += vbCrLf + " )"
                strSql += vbCrLf + " SELECT TAGSNO,ITEMID,TAGNO,STNITEMID,STNSUBITEMID,STNPCS,STNWT,STNRATE,STNAMT,STONEUNIT,CALCMODE"
                strSql += vbCrLf + " ,PURRATE,PURAMT,COMPANYID,COSTID,STNSNO"
                strSql += vbCrLf + " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " ,'" & Date.Now.ToLongTimeString & "' "
                strSql += vbCrLf + " FROM " & cnAdminDb & "..PURITEMTAGSTONE"
                strSql += " WHERE TAGSNO = '" & updIssSno & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

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


            If MetalBasedStone And dtMultiMetalDetails.Rows.Count > 0 Then

                ''DELETING MULTIMETALDETAIL
                strSql = " DELETE FROM " & cnAdminDb & "..ITEMTAGMETAL"
                strSql += " WHERE TAGSNO = '" & updIssSno & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                ''INSERTING MULTIMETAL
                For cnt As Integer = 0 To dtMultiMetalDetails.Rows.Count - 1
                    Dim metalSno As String = GetNewSno(TranSnoType.ITEMTAGMETALCODE, tran, "GET_ADMINSNO_TRAN")
                    Dim multiMetalId As String = Nothing
                    Dim multiCatCode As String = Nothing
                    Dim multiSno As String = Nothing
                    With dtMultiMetalDetails.Rows(cnt)
                        multiSno = .Item("STNSNO").ToString
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
                        If MetalBasedStone Then
                            strSql += " ,NETWT"
                        End If
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
                        If MetalBasedStone Then
                            strSql += " ," & Val(.Item("NETWT").ToString) & "" 'NETWT
                        End If
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

                    ''Inserting StoneDetail
                    If TabControl1.TabPages.Contains(tabStone) Then
                        For cnt1 As Integer = 0 To dtStoneDetails.Rows.Count - 1
                            If dtStoneDetails.Rows(cnt1).Item("MSNO").ToString <> multiSno Then Continue For
                            With dtStoneDetails.Rows(cnt1)
                                Dim CutId As Integer = 0
                                Dim ColorId As Integer = 0
                                Dim ClarityId As Integer = 0
                                Dim ShapeId As Integer = 0
                                Dim SizeCode As Integer = 0
                                Dim SetTypeId As Integer = 0
                                Dim stnItemId As Integer = 0
                                Dim stnSubItemId As Integer = 0
                                Dim stnSno As String = GetNewSno(TranSnoType.ITEMTAGSTONECODE, tran, "GET_ADMINSNO_TRAN")
                                StnGrpId = Val(objGPack.GetSqlValue(" SELECT GROUPID FROM " & cnAdminDb & "..STONEGROUP WHERE GROUPNAME = '" & .Item("STNGRPID").ToString & "'", "GROUPID", , tran))
                                CutId = Val(objGPack.GetSqlValue(" SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & .Item("CUT").ToString & "'", "CUTID", , tran))
                                ColorId = Val(objGPack.GetSqlValue(" SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME = '" & .Item("COLOR").ToString & "'", "COLORID", , tran))
                                ClarityId = Val(objGPack.GetSqlValue(" SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & .Item("CLARITY").ToString & "'", "CLARITYID", , tran))
                                ShapeId = Val(objGPack.GetSqlValue(" SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & .Item("SHAPE").ToString & "'", "SHAPEID", , tran))
                                SizeCode = Val(objGPack.GetSqlValue(" SELECT STNSIZEID FROM " & cnAdminDb & "..STNSIZE WHERE SIZENAME = '" & .Item("STNSIZE").ToString & "'", "STNSIZEID", , tran))
                                SetTypeId = Val(objGPack.GetSqlValue(" SELECT SETTYPEID FROM " & cnAdminDb & "..STNSETTYPE WHERE SETTYPENAME = '" & .Item("SETTYPE").ToString & "'", "SETTYPEID", , tran))
                                .Item("STNSNO") = stnSno
                                'Dim caType As String = Nothing
                                strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Item("ITEM").ToString & "'"
                                stnItemId = Val(objGPack.GetSqlValue(strSql, "ITEMID", , tran))
                                strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & .Item("SUBITEM").ToString & "' AND ITEMID = " & stnItemId & ""
                                stnSubItemId = Val(objGPack.GetSqlValue(strSql, , , tran))
                                ''Inserting itemTagStone
                                strSql = " INSERT INTO " & cnAdminDb & "..ITEMTAGSTONE("
                                strSql += " SNO,TAGSNO,ITEMID,COMPANYID,STNITEMID,"
                                strSql += " STNSUBITEMID,TAGNO,STNPCS,STNWT,"
                                strSql += " STNRATE,STNAMT,DESCRIP,"
                                strSql += " RECDATE,CALCMODE,"
                                strSql += " MINRATE,STONEUNIT,ISSDATE,"
                                strSql += " OLDTAGNO,CARRYFLAG,COSTID,SYSTEMID,APPVER,USRATE,INDRS,PACKETNO"
                                strSql += " ,STNGRPID,CUTID,COLORID,CLARITYID,SHAPEID,SIZECODE,SETTYPEID,HEIGHT,WIDTH"
                                If MetalBasedStone Then
                                    strSql += vbCrLf + " ,TAGMSNO"
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
                                strSql += " ,'" & StnGrpId & "'" 'STNGRPID
                                strSql += " ,'" & CutId & "'" 'CUTID
                                strSql += " ,'" & ColorId & "'" 'COLORID
                                strSql += " ,'" & ClarityId & "'" 'CLARITYID
                                strSql += " ,'" & ShapeId & "'" 'SHAPEID
                                strSql += " ,'" & SizeCode & "'" 'SIZECODE
                                strSql += " ,'" & SetTypeId & "'" 'SETTYPEID
                                strSql += " ,'" & Val(.Item("HEIGHT").ToString) & "'" 'HEIGHT
                                strSql += " ,'" & Val(.Item("WIDTH").ToString) & "'" 'WIDTH
                                If MetalBasedStone Then
                                    strSql += " ,'" & metalSno.ToString & "'" ''TAGMSNO
                                End If
                                strSql += " )"
                                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                            End With
                        Next

                    End If
                Next

                If TabControl1.TabPages.Contains(tabStone) Then
                    For cnt1 As Integer = 0 To dtStoneDetails.Rows.Count - 1
                        If dtStoneDetails.Rows(cnt1).Item("MSNO").ToString <> "" Then Continue For
                        With dtStoneDetails.Rows(cnt1)
                            Dim CutId As Integer = 0
                            Dim ColorId As Integer = 0
                            Dim ClarityId As Integer = 0
                            Dim ShapeId As Integer = 0
                            Dim SizeCode As Integer = 0
                            Dim SetTypeId As Integer = 0
                            Dim stnItemId As Integer = 0
                            Dim stnSubItemId As Integer = 0
                            Dim stnSno As String = GetNewSno(TranSnoType.ITEMTAGSTONECODE, tran, "GET_ADMINSNO_TRAN")
                            StnGrpId = Val(objGPack.GetSqlValue(" SELECT GROUPID FROM " & cnAdminDb & "..STONEGROUP WHERE GROUPNAME = '" & .Item("STNGRPID").ToString & "'", "GROUPID", , tran))
                            CutId = Val(objGPack.GetSqlValue(" SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & .Item("CUT").ToString & "'", "CUTID", , tran))
                            ColorId = Val(objGPack.GetSqlValue(" SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME = '" & .Item("COLOR").ToString & "'", "COLORID", , tran))
                            ClarityId = Val(objGPack.GetSqlValue(" SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & .Item("CLARITY").ToString & "'", "CLARITYID", , tran))
                            ShapeId = Val(objGPack.GetSqlValue(" SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & .Item("SHAPE").ToString & "'", "SHAPEID", , tran))
                            SizeCode = Val(objGPack.GetSqlValue(" SELECT STNSIZEID FROM " & cnAdminDb & "..STNSIZE WHERE SIZENAME = '" & .Item("STNSIZE").ToString & "'", "STNSIZEID", , tran))
                            SetTypeId = Val(objGPack.GetSqlValue(" SELECT SETTYPEID FROM " & cnAdminDb & "..STNSETTYPE WHERE SETTYPENAME = '" & .Item("SETTYPE").ToString & "'", "SETTYPEID", , tran))
                            .Item("STNSNO") = stnSno
                            'Dim caType As String = Nothing
                            strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Item("ITEM").ToString & "'"
                            stnItemId = Val(objGPack.GetSqlValue(strSql, "ITEMID", , tran))
                            strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & .Item("SUBITEM").ToString & "' AND ITEMID = " & stnItemId & ""
                            stnSubItemId = Val(objGPack.GetSqlValue(strSql, , , tran))
                            ''Inserting itemTagStone
                            strSql = " INSERT INTO " & cnAdminDb & "..ITEMTAGSTONE("
                            strSql += " SNO,TAGSNO,ITEMID,COMPANYID,STNITEMID,"
                            strSql += " STNSUBITEMID,TAGNO,STNPCS,STNWT,"
                            strSql += " STNRATE,STNAMT,DESCRIP,"
                            strSql += " RECDATE,CALCMODE,"
                            strSql += " MINRATE,STONEUNIT,ISSDATE,"
                            strSql += " OLDTAGNO,CARRYFLAG,COSTID,SYSTEMID,APPVER,USRATE,INDRS,PACKETNO"
                            strSql += " ,STNGRPID,CUTID,COLORID,CLARITYID,SHAPEID,SIZECODE,SETTYPEID,HEIGHT,WIDTH"
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
                            strSql += " ,'" & StnGrpId & "'" 'STNGRPID
                            strSql += " ,'" & CutId & "'" 'CUTID
                            strSql += " ,'" & ColorId & "'" 'COLORID
                            strSql += " ,'" & ClarityId & "'" 'CLARITYID
                            strSql += " ,'" & ShapeId & "'" 'SHAPEID
                            strSql += " ,'" & SizeCode & "'" 'SIZECODE
                            strSql += " ,'" & SetTypeId & "'" 'SETTYPEID
                            strSql += " ,'" & Val(.Item("HEIGHT").ToString) & "'" 'HEIGHT
                            strSql += " ,'" & Val(.Item("WIDTH").ToString) & "'" 'WIDTH
                            strSql += " )"
                            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                        End With
                    Next
                End If
            Else
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

                ''Inserting StoneDetail
                If TabControl1.TabPages.Contains(tabStone) Then
                    For cnt As Integer = 0 To dtStoneDetails.Rows.Count - 1
                        With dtStoneDetails.Rows(cnt)
                            Dim CutId As Integer = 0
                            Dim ColorId As Integer = 0
                            Dim ClarityId As Integer = 0
                            Dim ShapeId As Integer = 0
                            Dim SizeCode As Integer = 0
                            Dim SetTypeId As Integer = 0
                            Dim stnItemId As Integer = 0
                            Dim stnSubItemId As Integer = 0
                            Dim stnSno As String = GetNewSno(TranSnoType.ITEMTAGSTONECODE, tran, "GET_ADMINSNO_TRAN")
                            StnGrpId = Val(objGPack.GetSqlValue(" SELECT GROUPID FROM " & cnAdminDb & "..STONEGROUP WHERE GROUPNAME = '" & .Item("STNGRPID").ToString & "'", "GROUPID", , tran))
                            CutId = Val(objGPack.GetSqlValue(" SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & .Item("CUT").ToString & "'", "CUTID", , tran))
                            ColorId = Val(objGPack.GetSqlValue(" SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME = '" & .Item("COLOR").ToString & "'", "COLORID", , tran))
                            ClarityId = Val(objGPack.GetSqlValue(" SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & .Item("CLARITY").ToString & "'", "CLARITYID", , tran))
                            ShapeId = Val(objGPack.GetSqlValue(" SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & .Item("SHAPE").ToString & "'", "SHAPEID", , tran))
                            SizeCode = Val(objGPack.GetSqlValue(" SELECT STNSIZEID FROM " & cnAdminDb & "..STNSIZE WHERE SIZENAME = '" & .Item("STNSIZE").ToString & "'", "STNSIZEID", , tran))
                            SetTypeId = Val(objGPack.GetSqlValue(" SELECT SETTYPEID FROM " & cnAdminDb & "..STNSETTYPE WHERE SETTYPENAME = '" & .Item("SETTYPE").ToString & "'", "SETTYPEID", , tran))
                            .Item("STNSNO") = stnSno
                            'Dim caType As String = Nothing
                            strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Item("ITEM").ToString & "'"
                            stnItemId = Val(objGPack.GetSqlValue(strSql, "ITEMID", , tran))
                            strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & .Item("SUBITEM").ToString & "' AND ITEMID = " & stnItemId & ""
                            stnSubItemId = Val(objGPack.GetSqlValue(strSql, , , tran))
                            ''Inserting itemTagStone
                            strSql = " INSERT INTO " & cnAdminDb & "..ITEMTAGSTONE("
                            strSql += " SNO,TAGSNO,ITEMID,COMPANYID,STNITEMID,"
                            strSql += " STNSUBITEMID,TAGNO,STNPCS,STNWT,"
                            strSql += " STNRATE,STNAMT,DESCRIP,"
                            strSql += " RECDATE,CALCMODE,"
                            strSql += " MINRATE,STONEUNIT,ISSDATE,"
                            strSql += " OLDTAGNO,CARRYFLAG,COSTID,SYSTEMID,APPVER,USRATE,INDRS,PACKETNO"
                            strSql += " ,STNGRPID,CUTID,COLORID,CLARITYID,SHAPEID,SIZECODE,SETTYPEID,HEIGHT,WIDTH"
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
                            strSql += " ,'" & StnGrpId & "'" 'STNGRPID
                            strSql += " ,'" & CutId & "'" 'CUTID
                            strSql += " ,'" & ColorId & "'" 'COLORID
                            strSql += " ,'" & ClarityId & "'" 'CLARITYID
                            strSql += " ,'" & ShapeId & "'" 'SHAPEID
                            strSql += " ,'" & SizeCode & "'" 'SIZECODE
                            strSql += " ,'" & SetTypeId & "'" 'SETTYPEID
                            strSql += " ,'" & Val(.Item("HEIGHT").ToString) & "'" 'HEIGHT
                            strSql += " ,'" & Val(.Item("WIDTH").ToString) & "'" 'WIDTH
                            strSql += " )"
                            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                        End With
                    Next
                End If
            End If

            ''Inserting PURStoneDetail
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
                    strSql += vbCrLf + " ,PURAMT,COMPANYID,COSTID,STNSNO"
                    strSql += vbCrLf + " )"
                    strSql += vbCrLf + " VALUES"
                    strSql += vbCrLf + " ("
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
                    strSql += " SNO,ITEMID,TAGNO,MISCID,AMOUNT,"
                    strSql += " TAGSNO,COSTID,SYSTEMID,APPVER,COMPANYID)VALUES("
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
                        strSql += vbCrLf + " MISSNO,TAGSNO"
                        strSql += vbCrLf + " ,ITEMID"
                        strSql += vbCrLf + " ,TAGNO"
                        strSql += vbCrLf + " ,PURAMOUNT"
                        strSql += vbCrLf + " ,COMPANYID,COSTID"
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

            ''DELETING hallmarkdetails
            strSql = " DELETE FROM " & cnAdminDb & "..ITEMTAGHALLMARK"
            strSql += " WHERE TAGSNO = '" & updIssSno & "'"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            ''INSERTING HALLMARK DETAILS
            For cnt As Integer = 0 To dtHallmarkDetails.Rows.Count - 1
                Dim HALLMARKSno As String = ""
                HALLMARKSno = GetNewSno(TranSnoType.ITEMTAGHALLMARKCODE, tran, "GET_ADMINSNO_TRAN")
                With dtHallmarkDetails.Rows(cnt)
                    strSql = " INSERT INTO " & cnAdminDb & "..ITEMTAGHALLMARK"
                    strSql += " ("
                    strSql += " SNO,TAGSNO,GRSWT,HM_BILLNO,USERID,SYSTEMID,APPVER,UPDATED,UPTIME,COSTID,COMPANYID"
                    strSql += " )VALUES("
                    strSql += " '" & HALLMARKSno & "'" 'SNO
                    strSql += " ,'" & updIssSno & "'" 'TAGSNO
                    strSql += " ,'" & Val(.Item("GRSWT").ToString) & "'" 'GRSWT
                    strSql += " ,'" & .Item("HM_BILLNO").ToString & "'" 'HM_BILLNO
                    strSql += " ," & userId & "" 'USERID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                    strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                    strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    strSql += " )"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                End With
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

            ' ''Updating TagNo
            'If TagNoGen = "I" Then
            '    If TagNoFrom = "I" Then
            '        strSql = " UPDATE " & cnAdminDb & "..ITEMMAST SET CURRENTTAGNO = '" & TagNo & "' WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"
            '    Else 'SOFTCONTROL
            '        strSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT  = '" & TagNo & "' WHERE CTLID = 'LASTTAGNO'"
            '    End If
            'Else ''L
            '    strSql = " UPDATE " & cnAdminDb & "..ITEMLOT SET CURTAGNO = '" & TagNo & "'"
            'End If
            'cmd = New OleDbCommand(strSql, cn, tran)
            'cmd.ExecuteNonQuery()

            ' ''UPDATING 
            ' ''TAGNO INTO SOFTCONTROL
            'strSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT = '" & TagSno & "' WHERE CTLID = 'TAGSNO'"
            'cmd = New OleDbCommand(strSql, cn, tran)
            'cmd.ExecuteNonQuery()

            'CPIECES AND CWT
            strSql = " UPDATE " & cnAdminDb & "..ITEMLOT SET CPCS = CPCS + " & (Val(txtPieces_Num_Man.Text) - lotPcs) & ""
            strSql += " ,CLOSETIME=GETDATE(),CGRSWT = CGRSWT + " & (Val(txtGrossWt_Wet.Text) - lotGrsWt) & ""
            strSql += " ,CNETWT = ISNULL(CNETWT,0) + " & (Val(txtNetWt_Wet.Text) - lotNetWt) & ""
            strSql += " WHERE SNO = '" & SNO & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()

            If _CheckOrderInfo = False And ObjOrderTagInfo.txtOrderNo.Text <> "" And _EditEntryType.ToString <> "WO" Then
                strSql = vbCrLf + " UPDATE " & cnAdminDb & "..ITEMTAG SET ORDREPNO = '" & GetCostId(COSTID) & GetCompanyId(GetStockCompId()) & ObjOrderTagInfo.txtOrderNo.Text & "'"
                strSql += vbCrLf + " WHERE TAGNO = '" & txtTagNo__Man.Text & "' AND ORDREPNO = '" & EditOrNo & "'"
                strSql += vbCrLf + " UPDATE " & cnAdminDb & "..ORIRDETAIL SET ORNO = '" & GetCostId(COSTID) & GetCompanyId(GetStockCompId()) & ObjOrderTagInfo.txtOrderNo.Text & "'"
                strSql += vbCrLf + " WHERE TAGNO = '" & txtTagNo__Man.Text & "' AND ORNO = '" & EditOrNo & "'"
                strSql += vbCrLf + " UPDATE " & cnAdminDb & "..ORMAST SET ORNO = '" & GetCostId(COSTID) & GetCompanyId(GetStockCompId()) & ObjOrderTagInfo.txtOrderNo.Text & "' ,EMPID = '" & ObjOrderTagInfo.txtEmpNo_NUM.Text & "'"
                strSql += vbCrLf + " WHERE ORNO = '" & EditOrNo & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
            End If
            If _EditOrNo <> "" And _EditOrSNo <> "" And _EditEntryType.ToString <> "WO" Then
                strSql = vbCrLf + " UPDATE " & cnAdminDb & "..ORIRDETAIL SET GRSWT = " & Val(txtGrossWt_Wet.Text)
                strSql += vbCrLf + " , NETWT = " & Val(txtNetWt_Wet.Text)
                strSql += vbCrLf + " WHERE ORSNO = '" & _EditOrSNo & "' AND ORNO = '" & _EditOrNo & "'"
                strSql += vbCrLf + " AND TAGNO='" & txtTagNo__Man.Text & "' AND ORSTATUS='R' "
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
            End If

            ' ''Lot Pcs
            ''lblPCompled.Text = Val(lblPCompled.Text) + Val(txtPieces_Num_Man.Text)
            ''lblPBalance.Text = Val(lblPLot.Text) - Val(lblPCompled.Text)

            '' ''Lot Wt
            ''lblWCompleted.Text = Val(lblWCompleted.Text) + Val(txtGrossWt_Wet_Man.Text)
            ''lblWBalance.Text = Val(lblWLot.Text) - Val(lblWCompleted.Text)

            ' ''INSERT INTO GRIDVIEW
            ''Dim ro As DataRow = Nothing
            ''ro = dtGridView.NewRow
            ''ro("ITEM") = cmbItem_MAN.Text
            ''ro("SUBITEM") = cmbSubItem_Man.Text
            ''ro("TAGNO") = TagNo
            ''ro("PCS") = IIf(Val(txtPieces_Num_Man.Text) <> 0, Val(txtPieces_Num_Man.Text), DBNull.Value)
            ''ro("GRSWT") = IIf(Val(txtGrossWt_Wet_Man.Text) <> 0, Val(txtGrossWt_Wet_Man.Text), DBNull.Value)
            ''ro("LESSWT") = IIf(Val(txtLessWt_Wet.Text) <> 0, Val(txtLessWt_Wet.Text), DBNull.Value)
            ''ro("NETWT") = IIf(Val(txtNetWt_Wet.Text) <> 0, Val(txtNetWt_Wet.Text), DBNull.Value)
            ''ro("RATE") = IIf(Val(txtRate_Amt_Man.Text) <> 0, Val(txtRate_Amt_Man.Text), DBNull.Value)
            ''ro("SALEVALUE") = txtSalValue_Amt_Man.Text
            ''dtGridView.Rows.Add(ro)
            ''dtGridView.AcceptChanges()

            tran.Commit()
            tran = Nothing
            Dim prnmemsuffix As String = ""
            If GetAdmindbSoftValue("PRINTMEMWITHNODE", "N") = "Y" Then prnmemsuffix = systemId

            Dim objBar As New clsBarcodePrint
            If chkBarcodePrint.Checked = True Then
                If CallBarcodeExe = False Then
                    If METALID = "G" Then
                        objBar.FuncprintBarcode_Single(itemId, txtTagNo__Man.Text.ToString)
                    Else
                        If Val(GetAdmindbSoftValue("BARCODE_FORMAT", "1")) <= "1" Then
                            objBar.FuncprintBarcode_Single(itemId, txtTagNo__Man.Text.ToString)
                        Else
                            FuncprintBarcode_Multi(itemId, txtTagNo__Man.Text, Val(GetAdmindbSoftValue("BARCODE_FORMAT", "1")), METALID)
                        End If
                    End If
                Else
                    If GetAdmindbSoftValue("SING-TRANDB", "N") = "Y" Then
                        ''for kanaga durga
                        If cmbSubItem_Man.Enabled = True Then 'DESCRIP
                            BarcodeDescrip = cmbSubItem_Man.Text
                        Else
                            BarcodeDescrip = cmbItem_MAN.Text
                        End If
                        BarcodeTagNo = txtTagNo__Man.Text
                        BarcodeSno = updIssSno
                        FRM_PRINTDIA.ShowDialog()
                    Else
                        Dim write As StreamWriter
                        Dim memfile As String = "\Barcodeprint" & prnmemsuffix & ".mem"
                        write = IO.File.CreateText(Application.StartupPath & memfile)
                        write.WriteLine(LSet("PROC", 7) & ":" & itemId)
                        write.WriteLine(LSet("TAGNO", 7) & ":" & txtTagNo__Man.Text)
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
                            Dim tagDesc As String = LSet("TAGNO", 7) & ":" & txtTagNo__Man.Text
                            If IO.File.Exists(Application.StartupPath & "\PrjBarcodePrint.exe") Then
                                System.Diagnostics.Process.Start(Application.StartupPath & "\PrjBarcodePrint.exe", itDesc & ";" & tagDesc)
                            Else
                                MsgBox("Barcode print exe not found", MsgBoxStyle.Information)
                            End If
                        End If
                    End If
                End If
            End If
            If chkGuaranteeCard.Checked = True Then
                '290609 modified
                frmCardDetails.cardImagePath = picPath
                If IO.File.Exists(picPath) Then frmCardDetails.picTag.Image = Image.FromFile(picPath)
                frmCardDetails.TagNoEnb = False
                frmCardDetails.cmbTagNo.Text = txtTagNo__Man.Text
                frmCardDetails.cmbTagNo.Enabled = False
                frmCardDetails.ShowDialog()
            End If
            MsgBox(txtTagNo__Man.Text + E0009, MsgBoxStyle.Exclamation)
            If SMS_TAG_UPDATE <> "" Then
                funcSendAlertSms(OldGrsWt, Val(txtGrossWt_Wet.Text), txtTagNo__Man.Text, itemId _
                                            , OldWastper, OldWast, OldMcGrm, oldMc _
                                            , Val(txtMaxWastage_Per.Text), Val(txtMaxWastage_Wet.Text) _
                                            , Val(txtMaxMcPerGrm_Amt.Text), Val(txtMaxMkCharge_Amt.Text))
            End If
            Me.Dispose()
            ' ''Last Tag and Wt
            'lblLastTagNo.Text = TagNo
            'lblLastTagWt.Text = txtGrossWt_Wet_Man.Text

            'If Val(lblPBalance.Text) > 0 Then
            '    funcMultyNew()
            'Else
            '    funcNew()
            'End If
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            If Not tran Is Nothing Then tran.Dispose()
        End Try

    End Sub
    Private Sub funcSendAlertSms(ByVal OldGrsWt As Decimal, ByVal Grswt As Decimal _
                                , ByVal Tagno As String, ByVal Itemid As Integer _
                                , ByVal OldWastper As Decimal, ByVal OldWast As Decimal _
                                , ByVal OldMcGrm As Decimal, ByVal oldMc As Decimal _
                                , ByVal Wastper As Decimal, ByVal Wast As Decimal _
                                , ByVal McGrm As Decimal, ByVal Mc As Decimal)
        If ALERTBASE_MENU Then
            strSql = "SELECT TOP 1 1 FROM " & cnAdminDb & "..ALERTTRAN WHERE MENUID IN("
            strSql += vbCrLf + " SELECT MENUID FROM " & cnAdminDb & "..PRJMENUS WHERE ACCESSID LIKE '" & Me.Name & "%')"
            strSql += vbCrLf + " AND _EDIT='Y'"
        Else
            strSql = "SELECT TOP 1 1 FROM " & cnAdminDb & "..ALERTTRAN WHERE TABLENAME='ITEMTAG'"
            strSql += vbCrLf + " AND _EDIT='Y'"
        End If
        If objGPack.GetSqlValue(strSql, , "-1") = "-1" Then Exit Sub
        Dim TempMsg As String = ""
        TempMsg = SMS_TAG_UPDATE
        TempMsg = Replace(SMS_TAG_UPDATE, vbCrLf, "")
        TempMsg = Replace(TempMsg, "<USERNAME>", cnUserName)
        TempMsg = Replace(TempMsg, "<ITEMID>", Itemid.ToString)
        TempMsg = Replace(TempMsg, "<TAGNO>", Tagno.ToString)
        TempMsg = Replace(TempMsg, "<OLDGRSWT>", OldGrsWt.ToString)
        TempMsg = Replace(TempMsg, "<GRSWT>", Grswt.ToString)
        TempMsg = Replace(TempMsg, "<OLDMCGRM>", OldMcGrm.ToString)
        TempMsg = Replace(TempMsg, "<MCGRM>", McGrm.ToString)
        TempMsg = Replace(TempMsg, "<OLDMC>", oldMc.ToString)
        TempMsg = Replace(TempMsg, "<MC>", Mc.ToString)
        TempMsg = Replace(TempMsg, "<OLDWASTPER>", OldWastper.ToString)
        TempMsg = Replace(TempMsg, "<WASTPER>", Wastper.ToString)
        TempMsg = Replace(TempMsg, "<OLDWAST>", OldWast.ToString)
        TempMsg = Replace(TempMsg, "<WAST>", Wast.ToString)
        strSql = " SELECT  DISTINCT B.GROUPMOBILES FROM " & cnAdminDb & "..ALERTTRAN A "
        strSql += " LEFT JOIN " & cnAdminDb & "..ALERTGROUP B ON A.GROUPID=B.GROUPID "
        Dim dtAlert As New DataTable
        cmd = New OleDbCommand(strSql, cn, tran)
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtAlert)
        If dtAlert.Rows.Count > 0 Then
            For I As Integer = 0 To dtAlert.Rows.Count - 1
                Dim Mobiles() As String = dtAlert.Rows(I).Item("GROUPMOBILES").ToString.Split(",")
                If Not Mobiles Is Nothing Then
                    For J As Integer = 0 To Mobiles.Length - 1
                        If SmsSend(TempMsg, Mobiles(J).ToString) = False Then Exit For
                    Next
                End If
            Next
        End If
    End Sub

    Private Sub UpdateTag()
        If objGPack.Validator_Check(grpSaveControls) Then Exit Sub

        Dim ds As New DataSet
        ds.Clear()
        'If Not (dtpRecieptDate.Value.Date.ToString("yyyy-MM-dd") >= lotRecieptDate.Date.ToString("yyyy-MM-dd")) Then
        '    'And dtpRecieptDate.Value.Date.ToString("yyyy-MM-dd") <= GetServerDate(tran))
        '    Dim errStr As String
        '    errStr = "Reciept Date Should not allow before LotDate" + vbCrLf
        '    errStr += " And Receipt Date Should not Exceed Today Date"
        '    MsgBox(errStr, MsgBoxStyle.Exclamation)
        '    dtpRecieptDate.Focus()
        '    Exit Sub
        'End If


        'If Not Val(txtPieces_Num_Man.Text) <= Val(lblPBalance.Text) Then
        '    MsgBox(E0006 + lblPBalance.Text, MsgBoxStyle.Exclamation)
        '    txtPieces_Num_Man.Focus()
        '    Exit Sub
        'End If
        'If txtRate_Amt.Enabled = True Then
        '    If Val(txtRate_Amt.Text) <= 0 Then
        '        MsgBox(Me.GetNextControl(txtRate_Amt, False).Text + E0001, MsgBoxStyle.Exclamation)
        '        txtRate_Amt.Focus()
        '        Exit Sub
        '    End If
        'End If
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

        If Val(txtPieces_Num_Man.Text.ToString) = 0 And cmbItem_MAN.Text.ToString <> "" Then
            strSql = " SELECT 1 FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItem_MAN.Text.ToString & "' AND ISNULL(ALLOWZEROPCS,'')='Y' "
            If Val(objGPack.GetSqlValue(strSql, , "")) = 0 Then
                MsgBox("Pcs should not be zero", MsgBoxStyle.Information)
                txtPieces_Num_Man.Focus()
                Exit Sub
            End If
        End If
        If MANUAL_TAGNO = True Then
            Dim _TagNoFrom = GetSoftValue("TAGNOFROM")
            If _TagNoFrom = "I" Then
                strSql = "SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAG"
                strSql += " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "') "
                strSql += " AND TAGNO = '" & txtTagNo__Man.Text & "'"
                If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
                    MsgBox("TagNo Already Exist", MsgBoxStyle.Information)
                    txtTagNo__Man.Focus()
                    Exit Sub
                End If
            ElseIf _TagNoFrom = "U" Then
                strSql = "SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAG"
                strSql += " WHERE TAGNO = '" & txtTagNo__Man.Text & "'"
                If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
                    MsgBox("TagNo Already Exist", MsgBoxStyle.Information)
                    txtTagNo__Man.Focus()
                    Exit Sub
                End If
            End If
        End If

        If txtSetTagno.Enabled Then
            Dim dtSet As DataTable
            strSql = " SELECT "
            strSql += " ITEMID,SUBITEMID"
            strSql += " ,CASE WHEN ISSDATE IS NULL THEN 'N' ELSE 'Y' END ISSUED"
            strSql += " ,SETTAGNO "
            strSql += " FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO='" & txtSetTagno.Text.ToString & "'"

            dtSet = New DataTable
            cmd = New OleDbCommand(strSql, cn)
            da = New OleDbDataAdapter(cmd)
            da.Fill(dtSet)
            If dtSet.Rows.Count = 0 Then
                strSql = " SELECT "
                strSql += " ITEMID,SUBITEMID"
                strSql += " ,CASE WHEN ISSDATE IS NULL THEN 'N' ELSE 'Y' END ISSUED"
                strSql += " ,SETTAGNO "
                strSql += " FROM " & cnAdminDb & "..ITEMTAG WHERE LTRIM(ITEMID)+TAGNO='" & txtSetTagno.Text.ToString & "'"
                dtSet = New DataTable
                cmd = New OleDbCommand(strSql, cn)
                da = New OleDbDataAdapter(cmd)
                da.Fill(dtSet)
            End If
            If dtSet.Rows.Count > 0 Then
                With dtSet
                    If .Rows(0)("ISSUED").ToString = "Y" Then
                        MsgBox("SET TAGNO ALREDY ISSUED") : txtSetTagno.Focus() : Exit Sub
                    End If
                    If .Rows(0)("SETTAGNO").ToString <> "" And .Rows(0)("SETTAGNO").ToString <> txtSetTagno.Text.ToString Then
                        MsgBox("SET TAGNO ALREADY IN ANOTHER SET." + Environment.NewLine + "SET NO : " + .Rows(0)("SETTAGNO").ToString)
                        txtSetTagno.Focus()
                        Exit Sub
                    End If
                    If .Rows(0)("SUBITEMID").ToString = "0" Then
                        strSql = "SELECT SETITEM FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID='" & .Rows(0)("ITEMID").ToString & "' "
                        If objGPack.GetSqlValue(strSql) <> "Y" And CmbSetItem.Text = "NO" Then
                            MsgBox("SELECTED SET TAGNO IS NOT A SET ITEM")
                            txtSetTagno.Focus()
                            Exit Sub
                        End If
                    End If
                    If .Rows(0)("SUBITEMID").ToString <> "0" And .Rows(0)("ITEMID").ToString <> "0" Then
                        strSql = "SELECT SETITEM FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID='" & .Rows(0)("ITEMID").ToString & "' AND SUBITEMID='" & .Rows(0)("SUBITEMID").ToString & "' "
                        If objGPack.GetSqlValue(strSql) <> "Y" And CmbSetItem.Text = "NO" Then
                            MsgBox("SELECTED SET TAGNO IS NOT A SET ITEM")
                            txtSetTagno.Focus()
                            Exit Sub
                        End If
                    End If
                End With
            Else
                If MsgBox("SET TAGNO NOT FOUND" + vbCrLf + "Do you want to Proceed", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                    txtSetTagno.Focus() : Exit Sub
                End If
            End If
        End If
        If cmbSubItem_Man.Text <> "" Then
            strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')"
        Else
            strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"
        End If
        Dim calcType As String = objGPack.GetSqlValue(strSql)
        If cmbCalType.Enabled Then
            calcType = Mid(cmbCalType.Text, 1, 1)
        End If
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
            If ObjPurDetail.Loadd = False And _AuthPurchase = False Then
                If tagEdit And TAGEDITDISABLE.Contains("PV") Then GoTo nnnext
                txtSalValue_KeyPress(Me, New KeyPressEventArgs(Chr(Keys.Enter)))
                Exit Sub
            End If
        End If

        If PUR_AUTOCALC = True And Not (_HasPurchase Or pFixedVa) Then
            ShowPurDetails()
        End If


        If dtHallmarkDetails.Rows.Count > 0 Then
            For cnt As Integer = 0 To dtHallmarkDetails.Rows.Count - 1
                Dim chkHallmarkNo As String = ""
                chkHallmarkNo = dtHallmarkDetails.Rows(cnt).Item("HM_BILLNO").ToString
                If HallmarkMinLen > 0 And Not chkHallmarkNo Is Nothing Then
                    If Val(HallmarkMinLen) <> Val(chkHallmarkNo.ToString.Length) Then
                        MsgBox("HallmarkNo Length Less Than Minimum Length", MsgBoxStyle.Information)
                        TabControl1.SelectedTab = TabHallmark
                        Me.SelectNextControl(TabHallmark, True, True, True, True)
                        txtTagWt_WET.Focus()
                        Exit Sub
                    End If
                End If
                Dim Htagsno As String = ""
                ''strSql = "SELECT DISTINCT TAGSNO FROM (SELECT ISNULL(SNO,'')TAGSNO FROM " & cnAdminDb & "..ITEMTAG WHERE  HM_BILLNO = '" & chkHallmarkNo & "'"
                ''strSql += " UNION ALL SELECT ISNULL(TAGSNO,'')TAGSNO FROM " & cnAdminDb & "..ITEMTAGHALLMARK WHERE  HM_BILLNO = '" & chkHallmarkNo & "') X "

                'strSql = "SELECT DISTINCT TAGSNO FROM (SELECT ISNULL(SNO,'')TAGSNO FROM " & cnAdminDb & "..ITEMTAG WHERE  HM_BILLNO = '" & chkHallmarkNo & "'"
                'strSql += " AND NOT EXISTS(SELECT 1 FROM " & cnStockDb & "..ISSUE WHERE SNO IN "
                'strSql += " (SELECT ISSSNO FROM " & cnStockDb & "..ISSHALLMARK WHERE HM_BILLNO ='" & chkHallmarkNo & "') AND ISNULL(CANCEL,'')<>'Y' AND TRANTYPE='SA') "
                'strSql += " UNION ALL SELECT ISNULL(TAGSNO,'')TAGSNO FROM " & cnAdminDb & "..ITEMTAGHALLMARK WHERE  HM_BILLNO = '" & chkHallmarkNo & "' "
                'strSql += " AND NOT EXISTS(SELECT 1 FROM " & cnStockDb & "..ISSUE WHERE SNO IN "
                'strSql += " (SELECT ISSSNO FROM " & cnStockDb & "..ISSHALLMARK WHERE HM_BILLNO ='" & chkHallmarkNo & "') AND ISNULL(CANCEL,'')<>'Y' AND TRANTYPE='SA') "
                'strSql += " ) X"

                strSql = "SELECT DISTINCT TAGSNO FROM (SELECT ISNULL(SNO,'')TAGSNO FROM " & cnAdminDb & "..ITEMTAG WHERE  HM_BILLNO = '" & chkHallmarkNo & "'"
                strSql += " AND SNO NOT IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE LTRIM(ITEMID)+'-'+TAGNO IN "
                strSql += " (SELECT LTRIM(ITEMID)+'-'+TAGNO FROM " & cnStockDb & "..ISSUE WHERE SNO IN "
                strSql += " (SELECT ISSSNO FROM " & cnStockDb & "..ISSHALLMARK WHERE HM_BILLNO ='" & chkHallmarkNo & "') AND ISNULL(CANCEL,'')<>'Y' AND TRANTYPE='SA')) "
                strSql += " UNION ALL "
                strSql += " SELECT ISNULL(TAGSNO,'')TAGSNO FROM " & cnAdminDb & "..ITEMTAGHALLMARK WHERE  HM_BILLNO = '" & chkHallmarkNo & "' "
                strSql += " AND TAGSNO NOT IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE LTRIM(ITEMID)+'-'+TAGNO IN "
                strSql += " (SELECT LTRIM(ITEMID)+'-'+TAGNO FROM " & cnStockDb & "..ISSUE WHERE SNO IN "
                strSql += " (SELECT ISSSNO FROM " & cnStockDb & "..ISSHALLMARK WHERE HM_BILLNO ='" & chkHallmarkNo & "') AND ISNULL(CANCEL,'')<>'Y' AND TRANTYPE='SA')) "
                strSql += " ) X"
                Htagsno = GetSqlValue(cn, strSql)
                If Not Htagsno Is Nothing Then
                    Dim Htagrow As DataRow
                    strSql = " SELECT ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID=T.COSTID),'') COSTNAME "
                    strSql += " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID)ITEMNAME "
                    strSql += " ,ITEMID,TAGNO,CONVERT(VARCHAR(12),RECDATE,103)RECDATE "
                    strSql += " FROM " & cnAdminDb & "..ITEMTAG T WHERE "
                    strSql += " SNO='" & Htagsno.ToString & "'"
                    strSql += " AND NOT EXISTS(SELECT 1 FROM " & cnStockDb & "..ISSHALLMARK WHERE HM_BILLNO IN("
                    strSql += " SELECT DISTINCT HM_BILLNO FROM " & cnAdminDb & "..ITEMTAGHALLMARK WHERE TAGSNO IN ('" & Htagsno.ToString & "')))"
                    If tagEdit Then
                        strSql += " AND T.SNO<>'" & updIssSno & "'"
                    End If
                    Htagrow = GetSqlRow(strSql, cn, Nothing)
                    If Not Htagrow Is Nothing Then
                        MsgBox("HallmarkNo Already Exist" _
                            & IIf(Htagrow!Costname.ToString <> "", vbCrLf & " Branch : " & Htagrow!Costname.ToString, "") _
                            & vbCrLf & " Itemname : " & Htagrow!ITEMNAME.ToString & vbCrLf & " Recdate : " & Htagrow!RECDATE.ToString _
                            & vbCrLf & " Itemid : " & Htagrow!ITEMID.ToString & vbCrLf & " Tagno : " & Htagrow!TAGNO.ToString _
                            , MsgBoxStyle.Information)
                        TabControl1.SelectedTab = TabHallmark
                        Me.SelectNextControl(TabHallmark, True, True, True, True)
                        txtTagWt_WET.Focus()
                        Exit Sub
                    End If
                End If
            Next
            If Val(txtGrossWt_Wet.Text) > 0 Then
                Dim chkwt As Decimal = 0
                chkwt = Val(dtHallmarkDetails.Compute("SUM(GRSWT)", Nothing).ToString)
                If Val(chkwt) <> Val(txtGrossWt_Wet.Text) Then
                    MsgBox("Hallmark Weight Should Not Exceed GrossWeight", MsgBoxStyle.Information)
                    TabControl1.SelectedTab = TabHallmark
                    Me.SelectNextControl(TabHallmark, True, True, True, True)
                    txtTagWt_WET.Focus()
                    Exit Sub
                End If
            ElseIf Val(txtPieces_Num_Man.Text) > 0 And Val(txtGrossWt_Wet.Text) = 0 And HALLMARKVALID Then
                Dim chkpcs As Integer = 0
                chkpcs = Val(dtHallmarkDetails.Compute("SUM(PCS)", Nothing).ToString)
                If Val(chkpcs) <> Val(txtPieces_Num_Man.Text) Then
                    MsgBox("Hallmark Pcs Should Not Exceed TagPcs", MsgBoxStyle.Information)
                    TabControl1.SelectedTab = TabHallmark
                    Me.SelectNextControl(TabHallmark, True, True, True, True)
                    txtTagWt_WET.Focus()
                    Exit Sub
                End If
            End If
            ''If Val(txtPieces_Num_Man.Text) <> Val(dtHallmarkDetails.Compute("SUM(PCS)", Nothing)) Then
            ''    MsgBox("Hallmark Pcs Should Not Mismatch TagPcs", MsgBoxStyle.Information)
            ''    TabControl1.SelectedTab = TabHallmark
            ''    Me.SelectNextControl(TabHallmark, True, True, True, True)
            ''    txtTagWt_WET.Focus()
            ''    Exit Sub
            ''End If
        End If

        If HALLMARKVALID Then
            If NeedHallmark = True Then
                If dtHallmarkDetails.Rows.Count = 0 Then
                    MsgBox("Hallmark Details Should be Given", MsgBoxStyle.Information)
                    TabControl1.SelectedTab = TabHallmark
                    Me.SelectNextControl(TabHallmark, True, True, True, True)
                    txtTagWt_WET.Focus()
                    Exit Sub
                End If
                If txtHmBillNo.Text = "" And dtHallmarkDetails.Rows.Count = 0 Then
                    MsgBox("Hallmark Details Should be Given", MsgBoxStyle.Information)
                    TabControl1.SelectedTab = TabHallmark
                    Me.SelectNextControl(TabHallmark, True, True, True, True)
                    txtTagWt_WET.Focus()
                    Exit Sub
                End If
                ''If Val(txtPieces_Num_Man.Text) <> Val(dtHallmarkDetails.Compute("SUM(PCS)", Nothing)) Then
                ''    MsgBox("Hallmark Pcs Should Not Mismatch TagPcs", MsgBoxStyle.Information)
                ''    TabControl1.SelectedTab = TabHallmark
                ''    Me.SelectNextControl(TabHallmark, True, True, True, True)
                ''    txtTagWt_WET.Focus()
                ''    Exit Sub
                ''End If
                If Val(txtGrossWt_Wet.Text) <> Val(dtHallmarkDetails.Compute("SUM(GRSWT)", Nothing).ToString) And Val(txtGrossWt_Wet.Text) > 0 Then
                    MsgBox("Hallmark Weight Should Not Mismatch GrossWeight", MsgBoxStyle.Information)
                    TabControl1.SelectedTab = TabHallmark
                    Me.SelectNextControl(TabHallmark, True, True, True, True)
                    txtTagWt_WET.Focus()
                    Exit Sub
                ElseIf Val(txtPieces_Num_Man.Text) <> Val(dtHallmarkDetails.Compute("SUM(PCS)", Nothing).ToString) And Val(txtPieces_Num_Man.Text) > 0 And Val(txtGrossWt_Wet.Text) = 0 Then
                    MsgBox("Hallmark Pcs Should Not Exceed TagPcs", MsgBoxStyle.Information)
                    TabControl1.SelectedTab = TabHallmark
                    Me.SelectNextControl(TabHallmark, True, True, True, True)
                    txtTagWt_WET.Focus()
                    Exit Sub
                End If
            End If
        End If

        If dtMultiMetalDetails.Rows.Count > 0 And MultimetalValidWt Then
            If Val(txtGrossWt_Wet.Text) <> Val(dtMultiMetalDetails.Compute("SUM(WEIGHT)", "").ToString) And Val(txtGrossWt_Wet.Text) > 0 Then
                MsgBox("MultiMetal Weight Should Not Exceed Tag GrossWeight", MsgBoxStyle.Information)
                txtMMCategory.Select()
                Exit Sub
            End If
        End If

nnnext:
        If picPath <> Nothing And DefaultPctFile.ToString = "" Then
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

        If txtHSN.Text <> "" And NeedTag_Hsncode Then
            strSql = " SELECT COUNT(*)CNT FROM " & cnAdminDb & "..HSNMASTER WHERE HSNCODE = '" & txtHSN.Text & "'"
            If Val(GetSqlValue(cn, strSql).ToString) <= 0 Then
                MsgBox("Invalid Hsncode", MsgBoxStyle.Information)
                txtHSN.SelectAll()
                Exit Sub
            End If
        End If

        If GetAdmindbSoftValue("SPECIFIED_STYLENO", "N") = "Y" Then
            txtStyleCode.Text = funcspecifiedstyleno()
        End If

        If tagEdit Then txtMaxWastagePer_TextChanged(Me, New System.EventArgs)
        If tagEdit Then txtMaxMcPerGrm_TextChanged(Me, New System.EventArgs)

        If _LEntryType.ToString = "WO" Then
            If CheckWorkorder() = False Then
                Exit Sub
            End If
        End If

        If tagEdit Then
            If _CheckOrderInfo = False And ObjOrderTagInfo.txtOrderNo.Text <> "" Then
                ObjOrderTagInfo.ShowDialog()
            End If
            UpdateTag()
            Exit Sub
        End If
        If TAGWOLOT Then txtLotNo_Num_Man.Text = "XX"
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

        If tabCheckBy = "P" Then
            If Not Val(txtPieces_Num_Man.Text) <= Val(lblPBalance.Text) + Val(Tag_Tolerance) And TAGWOLOT = False Then
                MsgBox(Me.GetNextControl(txtPieces_Num_Man, False).Text + E0006 + lblPBalance.Text, MsgBoxStyle.Exclamation)
                txtPieces_Num_Man.Focus()
                Exit Sub
            End If
        Else
            'If Not Math.Abs(Val(lblWBalance.Text) - Val(txtGrossWt_Wet.Text)) <= Val(Tag_Tolerance) Then
            If Not Val(txtGrossWt_Wet.Text) <= Val(lblWBalance.Text) + Val(Tag_Tolerance) And TAGWOLOT = False Then
                MsgBox(Me.GetNextControl(txtPieces_Num_Man, False).Text + E0006 + lblWBalance.Text, MsgBoxStyle.Exclamation)
                txtGrossWt_Wet.Focus()
                Exit Sub
            End If
        End If
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
        If Val(ObjMinValue.txtMinWastage_Per.Text) <> 0 And Val(ObjMinValue.txtMinWastage_Per.Text) > Val(txtMaxWastage_Per.Text) Then
            MsgBox("MinWastage Per Should not Exceed MaxWastage Per", MsgBoxStyle.Information)
            ObjMinValue.txtMinWastage_Per.Focus()
            Exit Sub
        End If
        If Val(ObjMinValue.txtMinWastage_Wet.Text) <> 0 And Val(ObjMinValue.txtMinWastage_Wet.Text) > Val(txtMaxWastage_Wet.Text) Then
            MsgBox("MinWastage Should not Exceed MaxWastage ", MsgBoxStyle.Information)
            ObjMinValue.txtMinWastage_Wet.Focus()
            Exit Sub
        End If
        If Val(ObjMinValue.txtMinMcPerGram_Amt.Text) <> 0 And Val(ObjMinValue.txtMinMcPerGram_Amt.Text) > Val(txtMaxMcPerGrm_Amt.Text) Then
            MsgBox("Min Mc Per Grm Should not Exceed Max Mc Per Grm", MsgBoxStyle.Information)
            ObjMinValue.txtMinMcPerGram_Amt.Focus()
            Exit Sub
        End If
        If Val(ObjMinValue.txtMinMkCharge_Amt.Text) <> 0 And Val(ObjMinValue.txtMinMkCharge_Amt.Text) > Val(txtMaxMkCharge_Amt.Text) Then
            MsgBox("Min Making Charge Should not Exceed Max Making Charge", MsgBoxStyle.Information)
            ObjMinValue.txtMinMkCharge_Amt.Focus()
            Exit Sub
        End If
        If LOCK_VA_ITEMTAG Then
            If Val(txtMaxWastage_Wet.Text) = 0 And Val(txtMaxMkCharge_Amt.Text) = 0 Then
                MsgBox("VA Should not be empty", MsgBoxStyle.Information)
                txtMaxWastage_Per.Focus()
                Exit Sub
            End If
        End If
        If True Then
            strSql = " SELECT COUNT(*) CNT FROM " & cnAdminDb & "..ITEMLOT LOT "
            strSql += " LEFT JOIN " & cnStockDb & "..LOTISSUE R ON LOT.SNO = R.LOTSNO"
            strSql += " WHERE lot.LOTNO = '" & txtLotNo_Num_Man.Text & "'"
            If tabCheckBy = "P" Then
                strSql += " AND LOT.PCS > LOT.CPCS "
            ElseIf tabCheckBy = "E" Then
                strSql += vbCrLf + " AND ((LOT.GRSWT > LOT.CGRSWT) OR (LOT.PCS > LOT.CPCS))"
            Else
                strSql += vbCrLf + " AND ((LOT.GRSWT > LOT.CGRSWT) OR (LOT.RATE <> 0 AND LOT.PCS > LOT.CPCS))"
            End If
            strSql += " AND ISNULL(COMPLETED,'') <> 'Y'"
            strSql += " AND lot.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T')"
            strSql += " AND ISNULL(BULKLOT,'') <> 'Y'"
            Dim dtPcsGrswt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtPcsGrswt)
            If Val(dtPcsGrswt.Rows(0).Item("CNT").ToString) = 0 Then
                MsgBox("should not exceed lot", MsgBoxStyle.Information)
                txtPieces_Num_Man.Focus()
                Exit Sub
            End If
        End If

        If CHK_BOOK_CTR_STOCK And Val(txtGrossWt_Wet.Text.ToString) > 0 Then
            'Check Book stock and counter stock
            Dim tempcatcode As String = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') = '" & cmbItem_MAN.Text & "'").ToString
            TChkbStk = False
            'objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') = '" & cmbItem_MAN.Text & "'").ToString) + Val(txtGrossWt_Wet.Text.ToString
            If CheckBookstk(tempcatcode) < CheckCounterstk(tempcatcode) + Val(txtGrossWt_Wet.Text.ToString) Then
                MsgBox("Book stock weight is less than counter stock.")
                txtGrossWt_Wet.Text = ""
                btnSave.Enabled = False
                'txtGrossWt_Wet.Focus()
                txtPieces_Num_Man.Focus()
                Exit Sub
            End If
        End If
        Try
            funcAdd()
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Function funcspecifiedstyleno() As String
        Dim _tempstyleno As String = ""
        If cmbCounter_MAN.Enabled = True Then
            ''Find ItemCounter
            strSql = " SELECT ITEMCTRSHNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbCounter_MAN.Text & "'"
            _tempstyleno = objGPack.GetSqlValue(strSql, "ITEMCTRSHNAME", "", Nothing).ToString
        End If
        ''If DtAddStockEntry.Select("MISCID='6'", Nothing).Length > 0 Then
        ''    Dim drr() As DataRow
        ''    drr = DtAddStockEntry.Select("MISCID='6'", Nothing)
        ''    _tempstyleno = _tempstyleno & funcgetaddinfoitemtagShname(drr(0))
        ''End If
        ''If DtAddStockEntry.Select("MISCID='7'", Nothing).Length > 0 Then
        ''    Dim drr() As DataRow
        ''    drr = DtAddStockEntry.Select("MISCID='7'", Nothing)
        ''    _tempstyleno = _tempstyleno & funcgetaddinfoitemtagShname(drr(0))
        ''End If
        strSql = " SELECT SHORTNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItem_MAN.Text & "'"
        _tempstyleno = _tempstyleno & objGPack.GetSqlValue(strSql, "SHORTNAME", "", Nothing)

        If cmbSubItem_Man.Enabled = True Then
            ''Find SubItemId
            strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItem_MAN.Text & "'"
            Dim _itemid As Integer = Val(objGPack.GetSqlValue(strSql, "ITEMID", "", Nothing))
            strSql = " SELECT SHORTNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "' AND ITEMID = " & _itemid & ""
            _tempstyleno = _tempstyleno & objGPack.GetSqlValue(strSql, "SHORTNAME", "", Nothing).ToString
        End If

        strSql = " SELECT SNAME FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType_MAN.Text & "'"
        _tempstyleno = _tempstyleno & objGPack.GetSqlValue(strSql, "SNAME", "", Nothing).ToString
        If DtAddStockEntry.Select("MISCID='1'", Nothing).Length > 0 Then
            Dim drr() As DataRow
            drr = DtAddStockEntry.Select("MISCID='1'", Nothing)
            _tempstyleno = _tempstyleno & funcgetaddinfoitemtagShname(drr(0))
        End If
        If DtAddStockEntry.Select("MISCID='2'", Nothing).Length > 0 Then
            Dim drr() As DataRow
            drr = DtAddStockEntry.Select("MISCID='2'", Nothing)
            _tempstyleno = _tempstyleno & funcgetaddinfoitemtagShname(drr(0))
        End If
        If DtAddStockEntry.Select("MISCID='3'", Nothing).Length > 0 Then
            Dim drr() As DataRow
            drr = DtAddStockEntry.Select("MISCID='3'", Nothing)
            _tempstyleno = _tempstyleno & funcgetaddinfoitemtagShname(drr(0))
        End If
        If DtAddStockEntry.Select("MISCID='4'", Nothing).Length > 0 Then
            Dim drr() As DataRow
            drr = DtAddStockEntry.Select("MISCID='4'", Nothing)
            _tempstyleno = _tempstyleno & funcgetaddinfoitemtagShname(drr(0))
        End If
        ''If DtAddStockEntry.Select("MISCID='5'", Nothing).Length > 0 Then
        ''    Dim drr() As DataRow
        ''    drr = DtAddStockEntry.Select("MISCID='5'", Nothing)
        ''    _tempstyleno = _tempstyleno & funcgetaddinfoitemtagShname(drr(0))
        ''End If
        Return _tempstyleno
    End Function

    Function funcgetaddinfoitemtagShname(ByVal OTHID As DataRow) As String
        Dim _tempstr As String = ""
        strSql = " SELECT SHORTNAME FROM " & cnAdminDb & "..OTHERMASTER WHERE ID = '" & OTHID("OTHID").ToString & "'"
        _tempstr = objGPack.GetSqlValue(strSql, "SHORTNAME", "", Nothing).ToString
        Return _tempstr
    End Function

    Private Sub txtLotNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtLotNo_Num_Man.KeyPress
        Dim dt As DataTable
        If txtLotNo_Num_Man.Text = "" Then
            Exit Sub
        End If
        If e.KeyChar = Chr(Keys.Enter) Then
            strSql = " SELECT lot.SNO,r.Recsno,LOT.STKTYPE FROM " & cnAdminDb & "..ITEMLOT LOT "
            strSql += " LEFT JOIN " & cnStockDb & "..LOTISSUE R ON LOT.SNO = R.LOTSNO"
            strSql += " WHERE lot.LOTNO = '" & txtLotNo_Num_Man.Text & "'"
            If tabCheckBy = "P" Then
                strSql += " AND LOT.PCS > LOT.CPCS "
            ElseIf tabCheckBy = "E" Then
                strSql += vbCrLf + " AND ((LOT.GRSWT > LOT.CGRSWT) OR (LOT.PCS > LOT.CPCS))"
            Else
                strSql += vbCrLf + " AND ((LOT.GRSWT > LOT.CGRSWT) OR (LOT.RATE <> 0 AND LOT.PCS > LOT.CPCS))"
                '                strSql += " AND GRSWT > CGRSWT"
            End If
            strSql += " AND ISNULL(COMPLETED,'') <> 'Y'"
            ' strSql += " AND (SELECT STOCKTYPE FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = LOT.ITEMID) = 'T'"
            'strSql += " AND LOTNO = '" & txtLotNo_Num_Man.Text & "'"
            strSql += " AND lot.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T')"
            strSql += " AND ISNULL(BULKLOT,'') <> 'Y'"
            strSql += " AND ISNULL(LOT.CANCEL,'') <> 'Y'"
            If SPECIFICFORMAT.ToString = "1" Then
                strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COMPANYID LIKE '%" & strCompanyId & "%')"
                strSql += vbCrLf + " AND LOT.COMPANYID='" & strCompanyId & "'"
            End If
            da = New OleDbDataAdapter(strSql, cn)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If dt.Rows.Count = 1 Then
                    SNO = dt.Rows(0).Item("Sno").ToString
                    Recsno = dt.Rows(0).Item("recsno").ToString
                    If dt.Rows(0).Item("STKTYPE").ToString = "M" Then
                        lblStkType.Text = "Manufacturing"
                    ElseIf dt.Rows(0).Item("STKTYPE").ToString = "E" Then
                        lblStkType.Text = "Exempted"
                    Else
                        lblStkType.Text = "Trading"
                    End If
                    lblStkType.Visible = True
                    If LoadLotDetails() = False Then Exit Sub
                    txtLotNo_Num_Man.Enabled = False
                    If TAG_STONEAUTOLOAD_MR Then
                        funcReceiptStoneDetailAutoLoad(Recsno)
                    End If
                    funcReceiptMisCharges(Recsno)
                Else
                    strSql = " SELECT "
                    strSql += " LOTNO"
                    strSql += vbCrLf + " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = LOT.DESIGNERID)AS DESIGNER"
                    strSql += " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST AS IM WHERE IM.ITEMID = LOT.ITEMID)AS ITEMNAME"
                    strSql += " ,ISNULL((SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST AS SM WHERE SM.SUBITEMID = LOT.SUBITEMID),'')AS SUBITEMNAME"
                    strSql += " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = LOT.COSTID)AS COSTCENTRE"
                    strSql += " ,PCS,GRSWT,CPCS AS CPCS,CGRSWT AS CGRSWT,(PCS-CPCS) AS BALPCS,(GRSWT-CGRSWT)AS BALGRSWT"
                    strSql += " ,(SELECT (CASE WHEN STOCKTYPE = 'T' THEN 'TAGED' WHEN STOCKTYPE = 'N' THEN 'NON TAGED' ELSE 'PACKET BASED' END) FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = LOT.ITEMID)AS STOCKTYPE"
                    strSql += vbCrLf + " ,CASE ENTRYTYPE WHEN 'R' THEN 'REGULAR' WHEN 'OR' THEN 'ORDER' WHEN 'RE' THEN 'REPAIR' ELSE ENTRYTYPE END AS LOTTYPE,SNO,FROMITEMID"
                    strSql += " FROM " & cnAdminDb & "..ITEMLOT AS LOT"
                    strSql += " WHERE PCS > CPCS AND ISNULL(COMPLETED,'') <> 'Y'"
                    strSql += " AND (SELECT STOCKTYPE FROM " & cnAdminDb & "..ITEMMAST AS I WHERE I.ITEMID = LOT.ITEMID) = 'T'"
                    strSql += " AND LOTNO = '" & txtLotNo_Num_Man.Text & "'"
                    strSql += " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T')"
                    strSql += " AND ISNULL(BULKLOT,'') <> 'Y'"
                    If SPECIFICFORMAT.ToString = "1" Then
                        strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COMPANYID LIKE '%" & strCompanyId & "%')"
                        strSql += vbCrLf + " AND LOT.COMPANYID='" & strCompanyId & "'"
                    End If
                    strSql += " ORDER BY LOTNO "
                    SNO = BrighttechPack.SearchDialog.Show("Searching LotNo", strSql, cn, 0, 13)

                    strSql = " SELECT A.LOTNO,LI.RECSNO,A.STKTYPE FROM " & cnAdminDb & "..ITEMLOT A LEFT JOIN " & cnStockDb & "..LOTISSUE LI ON A.SNO=LI.LOTSNO"
                    strSql += " WHERE SNO = '" & SNO & "'"
                    dt.Clear()
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(dt)
                    If dt.Rows.Count > 0 Then
                        txtLotNo_Num_Man.Text = dt.Rows(0).Item("LotNo").ToString
                        Recsno = dt.Rows(0).Item("RECSNO").ToString
                        If dt.Rows(0).Item("STKTYPE").ToString = "M" Then
                            lblStkType.Text = "Manufacturing"
                        ElseIf dt.Rows(0).Item("STKTYPE").ToString = "E" Then
                            lblStkType.Text = "Exempted"
                        Else
                            lblStkType.Text = "Trading"
                        End If
                        funcReceiptMisCharges(Recsno)
                    Else
                        Exit Sub
                    End If

                    If LoadLotDetails() = False Then Exit Sub
                    txtLotNo_Num_Man.Enabled = False
                End If
            Else
                MsgBox(E0004 + Me.GetNextControl(txtLotNo_Num_Man, False).Text, MsgBoxStyle.Exclamation)
                txtLotNo_Num_Man.Focus()
            End If
        Else
            Exit Sub
        End If
        'funcAssignTabControls()
    End Sub

    Private Function LoadLotDetails() As Boolean
        Dim itemName As String = ""
        NeedHallmark = False
        'strSql = " SELECT (sELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = L.ITEMID)AS ITEMNAME FROM " & cnAdminDb & "..ITEMLOT as L WHERE SNO = '" & SNO & "'"
        strSql = " SELECT L.ENTRYTYPE ENTRYTYPE,IT.ITEMNAME ITEMNAME,IT.ITEMID ITEMID,ISNULL(IT.ASSORTED,'') AS ASSORTED,fromitemid,IT.MCASVAPER,L.STYLENO,L.NARRATION,ISNULL(L.HALLMARK,'Y') HALLMARK,ISNULL(PONUMBER,'') PONUMBER FROM " & cnAdminDb & "..ITEMLOT L LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IT ON L.ITEMID = IT.ITEMID WHERE SNO = '" & SNO & "' "
        Dim DR As DataRow
        DR = GetSqlRow(strSql, cn)
        'itemName = objGPack.GetSqlValue(strSql)
        itemName = DR.Item("ITEMNAME").ToString
        PoNumber = DR.Item("PONUMBER").ToString
        lblPoNumber.Text = PoNumber
        If DR.Item("HALLMARK").ToString = "Y" Then
            NeedHallmark = True
        Else
            NeedHallmark = False
        End If
        Dim itemID As String
        itemID = DR.Item("ITEMID").ToString
        If NARRATION_VIEW_IN_ITEMTAG Then
            txtNarration.Text = DR.Item("NARRATION").ToString
        End If

        Dim entryType As String = DR.Item("ENTRYTYPE").ToString
        _LEntryType = DR.Item("ENTRYTYPE").ToString
        Dim mASSORTED As String = DR.Item("ASSORTED").ToString
        mfromItemid = Val(DR.Item("FROMITEMID").ToString)

        If entryType.ToString = "WO" Then
            txtWorkOrderNo.Clear()
            txtWorkOrderNo.Enabled = True
        Else
            txtWorkOrderNo.Clear()
            txtWorkOrderNo.Enabled = False
        End If

        If DR.Item("Mcasvaper").ToString = "Y" Then Label44.Text = "MC PERCENT" Else Label44.Text = "Max Mc Per Grm"
        If entryType = "OR" Or entryType = "RE" Then
            If _CheckOrderInfo Then
                If Not ORDetail() Then
                    btnNew_Click(Me, New EventArgs)
                    txtLotNo_Num_Man.Focus()
                    Return False
                    Exit Function
                End If
            Else
                If ObjOrderTagInfo.ShowDialog() <> Windows.Forms.DialogResult.OK Then
                    btnNew_Click(Me, New EventArgs)
                    txtLotNo_Num_Man.Focus()
                    Return False
                    Exit Function
                End If
            End If
            If ORDERLOTITEM = True And itemID <> GetSqlValue(cn, "SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItem_MAN.Text.Trim & "'") Then
                MsgBox("Order item and lot item should be same", MsgBoxStyle.Information)
                btnNew_Click(Me, New EventArgs)
                txtLotNo_Num_Man.Focus()
                Return False
                Exit Function
            End If
            Dim dt As New DataTable
            strSql = funcAssignTagDefaultVal()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            funcAssignVal(dt, _CheckOrderInfo)
            If cmbSubItem_Man.Enabled Then cmbSubItem_SelectedIndexChanged(Me, New EventArgs)
            funcAssignTabControls()
            lblItemChange.Visible = False
            ItemLock = False
            Me.SelectNextControl(cmbItem_MAN, True, True, True, True)
            Return True
            Exit Function
        End If
        'If objGPack.GetSqlValue("SELECT ISNULL(ASSORTED,'') FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & itemName & "'") = "Y" Then
        If mASSORTED = "Y" Then
            lblItemChange.Visible = True
            lblStkType.Visible = False
            'cmbItem_MAN.Text = ""
        Else
            lblStkType.Visible = True
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
                    If PurTab_GrsNet Then
                        If PurGrsNet = "G" Then
                            .cmbPurCalMode.Text = "GRS WT"
                            .purGrsNet = "GRS WT"
                        Else
                            .cmbPurCalMode.Text = "NET WT"
                            .purGrsNet = "NET WT"
                        End If
                    Else
                        .cmbPurCalMode.Text = cmbCalcMode.Text
                    End If
                    .txtPurGrossWt_Wet.Text = txtGrossWt_Wet.Text
                    .txtPurLessWt_Wet.Text = txtLessWt_Wet.Text
                    .txtPurNetWt_Wet.Text = txtNetWt_Wet.Text
                    .dtpPurchaseDate.Value = dtpRecieptDate.Value
                    .txtPurMcPerGrm_TextChanged(Me, New EventArgs)
                    If purRate = 0 Then
                        purRate = GetPurchaseRate()
                        .txtPurRate_Amt.Text = purRate
                    End If
                    If purRate = 0 Then
                        .txtPurRate_Amt.Text = IIf(calType <> "W" And calType <> "B", txtRate_Amt.Text, txtMetalRate_Amt.Text)
                        If Val(.txtPurRate_Amt.Text) = 0 And PUR_LANDCOST And calType = "F" Then
                            .txtPurRate_Amt.Text = txtMetalRate_Amt.Text
                        End If
                        purRate = Val(.txtPurRate_Amt.Text)
                    Else
                        .txtPurRate_Amt.Text = purRate
                    End If
                End If
                SyncStoneMiscToPurStoneMisc()
            End If
            If purRate = 0 Then
                purRate = GetPurchaseRate()
                .txtPurRate_Amt.Text = purRate

                If PURTAB_LOCK = True Then
                    If purRate = 0 Then
                        .txtPurRate_Amt.Text = IIf(calType <> "W" And calType <> "B", txtRate_Amt.Text, txtMetalRate_Amt.Text)
                        If Val(.txtPurRate_Amt.Text) = 0 And PUR_LANDCOST And calType = "F" Then
                            .txtPurRate_Amt.Text = txtMetalRate_Amt.Text
                        End If
                        purRate = Val(.txtPurRate_Amt.Text)
                    Else
                        .txtPurRate_Amt.Text = purRate
                    End If
                End If
                If tagEdit Then
                    purRate = Val(objGPack.GetSqlValue("SELECT PURRATE FROM " + cnAdminDb + "..PURITEMTAG WHERE TAGSNO = '" + updIssSno + "'", "", "0"))
                    .txtPurRate_Amt.Text = purRate
                End If
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


            If PURTAB_LOCK = True Then
                If Not (_HasPurchase = False And pFixedVa = False) Then
                    If _AuthPurchase Then
                        .TagPurchaseDetail_Load(Me, New System.EventArgs)
                        ' IN FUTURE ADMIN VIEW PURCHASE VALUE JUST COMMENT THIS LINE
                        txtPurchaseValue_Amt_Hide.Location = txtPurchaseValue_Amt.Location
                        txtPurchaseValue_Amt_Hide.Size = txtPurchaseValue_Amt.Size
                        txtPurchaseValue_Amt_Hide.Visible = True
                        txtPurchaseValue_Amt_Hide.BringToFront()
                    End If
                End If
                '.TagPurchaseDetailEntry_KeyDown(Me, New System.EventArgs) '.btnSave_Click(Me, New System.EventArgs)
            Else
                Authorize = BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name.ToString, BrighttechPack.Methods.RightMode.Authorize, False)
                If Not (_HasPurchase = False And pFixedVa = False) Then
                    If _AuthPurchase Then
                        If Authorize Then .ShowDialog()
                    Else
                        .ShowDialog()
                    End If
                End If
            End If

            '01
            If PUR_LANDCOST Then
                If .saleValue > 0 Then
                    txtSalValue_Amt_Man.Text = SALEVALUE_ROUND(.saleValue)
                End If
            End If

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
            If calType = "R" And Val(.txtSaleRate_PER.Text) <> 0 Then txtRate_Amt.Text = Format(Val(.txtPurPurchaseVal_Amt.Text) + (Val(.txtPurPurchaseVal_Amt.Text) * (Val(.txtSaleRate_PER.Text) / 100)), "0.00")
            If calType = "M" And Val(.txtSaleRate_PER.Text) <> 0 Then txtRate_Amt.Text = Format(Val(.txtPurRate_Amt.Text) + (Val(.txtPurRate_Amt.Text) * (Val(.txtSaleRate_PER.Text) / 100)), "0.00")
            If PUR_LANDCOST Then
                If calType = "F" And Val(.txtSaleRate_PER.Text) <> 0 And Val(.txtPurPurchaseVal_Amt.Text) <> 0 Then
                    txtSalValue_Amt_Man.Text = Val(.txtPurPurchaseVal_Amt.Text) * Val(.txtSaleRate_PER.Text)
                End If
            End If
            flagPurValSet = True
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
                    Dim wt As Decimal = (Val(Row.Item("WEIGHT").ToString) / IIf(Val(Row.Item("PCS").ToString) <> 0, Val(Row.Item("PCS").ToString), 1)) * 100
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
        If TAG_TBLCODESEARCHOPT Then
            cmbTableCode.DropDownStyle = ComboBoxStyle.Simple
            cmbTableCode.Size = New System.Drawing.Size(87, 63)
            cmbTableCode.BringToFront()
        End If
        Me.cmbTableCode_SelectedIndexChanged(Me, New System.EventArgs)
    End Sub

    Private Sub CalcMaxMinValues()

        strSql = Nothing
        Dim type As String
        If Not OrderRow Is Nothing And OrdVA_Chg = False Then
            If tagEdit And chkFixedVa.Checked Then Exit Sub
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
            If (Val(txtMaxWastage_Per.Text) <> 0 Or Val(txtMaxWastage_Wet.Text) <> 0) Or (Val(txtMaxMkCharge_Amt.Text) <> 0 Or Val(txtMaxWastage_Per.Text) <> 0) Then
                txtMaxMkCharge_Org.Text = txtMaxMkCharge_Amt.Text : txtMaxWastage_Org.Text = txtMaxWastage_Wet.Text
                Exit Sub
            End If
        End If

        If cmbCalcMode.SelectedIndex = 0 And Val(txtGrossWt_Wet.Text) = 0 Then Exit Sub
        If cmbCalcMode.SelectedIndex <> 0 And Val(txtNetWt_Wet.Text) = 0 Then Exit Sub
        'type = objGPack.GetSqlValue(" SELECT WMCTYPE FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = '" & SNO & "'") 'LOTNO = " & Val(txtLotNo_Num_Man.Text) & "")
        If tagEdit Then
            type = objGPack.GetSqlValue(" SELECT VALUEADDEDTYPE FROM " & cnAdminDb & "..ITEMTAG WHERE SNO='" & updIssSno & "'") 'LOTNO = " & Val(txtLotNo_Num_Man.Text) & "")
        Else
            type = objGPack.GetSqlValue(" SELECT WMCTYPE FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = '" & SNO & "'") 'LOTNO = " & Val(txtLotNo_Num_Man.Text) & "")
        End If

        If objGPack.GetSqlValue("SELECT ISNULL(ASSORTED,'') FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = '" & SNO & "')") = "Y" Then
            strSql = "SELECT VALUEADDEDTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItem_MAN.Text & "'"
            type = objGPack.GetSqlValue(strSql, "VALUEADDEDTYPE", type)
        End If
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
                If VAL_ITEMTYPE_TABLE Then
                    strSql += vbCrLf + " AND ISNULL(ITEMTYPE,'') = ISNULL((SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType_MAN.Text & "'),'')"
                End If
                ''FOR SAYAR JEWELS
                If VAL_ITEMTYPE_TABLE And GetSqlRow(strSql, cn) Is Nothing Then
                    strSql = " DECLARE @WT FLOAT,@DIAPCS INT"
                    If cmbCalcMode.SelectedIndex = 0 Then ''GRS WT
                        strSql += vbCrLf + " SET @WT = " & Val(txtGrossWt_Wet.Text) & ""
                    Else ''NET WT
                        strSql += vbCrLf + " SET @WT = " & Val(txtNetWt_Wet.Text) & ""
                    End If
                    strSql += vbCrLf + " SELECT TOUCH,MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC,TOUCH_PUR,MAXWASTPER_PUR,MAXMCGRM_PUR,MAXWAST_PUR,MAXMC_PUR"
                    strSql += vbCrLf + " ,MINWASTPER,MINMCGRM,MINWAST,MINMC FROM " & cnAdminDb & "..WMCTABLE "
                    strSql += vbCrLf + " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')"
                    If cmbSubItem_Man.Enabled = True And cmbSubItem_Man.Text.Trim <> "" Then
                        strSql += vbCrLf + " AND SUBITEMID IN(SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "' AND ITEMID = ( SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'))"
                    End If
                    If cmbItemType_MAN.Enabled = True And cmbItemType_MAN.Text.Trim <> "" Then
                        strSql += vbCrLf + " AND ITEMTYPE IN((SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType_MAN.Text & "') UNION ALL SELECT 0 ) "
                    End If
                    strSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
                    strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
                    strSql += vbCrLf + " AND ISNULL(TABLECODE,'') = ''"
                    strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_Man.Text & "'),'')"
                    If GetSqlRow(strSql, cn) Is Nothing Then
                        strSql = " DECLARE @WT FLOAT,@DIAPCS INT"
                        If cmbCalcMode.SelectedIndex = 0 Then ''GRS WT
                            strSql += vbCrLf + " SET @WT = " & Val(txtGrossWt_Wet.Text) & ""
                        Else ''NET WT
                            strSql += vbCrLf + " SET @WT = " & Val(txtNetWt_Wet.Text) & ""
                        End If
                        strSql += vbCrLf + " SELECT TOUCH,MAXWASTPER,MAXMCGRM,MAXWAST,MAXMC,TOUCH_PUR,MAXWASTPER_PUR,MAXMCGRM_PUR,MAXWAST_PUR,MAXMC_PUR"
                        strSql += vbCrLf + " ,MINWASTPER,MINMCGRM,MINWAST,MINMC FROM " & cnAdminDb & "..WMCTABLE "
                        strSql += vbCrLf + " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')"
                        If cmbSubItem_Man.Enabled = True And cmbSubItem_Man.Text.Trim <> "" Then
                            strSql += vbCrLf + " AND SUBITEMID =0"
                        End If
                        strSql += vbCrLf + " AND ITEMTYPE IN((SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType_MAN.Text & "') UNION ALL SELECT 0 ) "
                        strSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
                        strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
                        strSql += vbCrLf + " AND ISNULL(TABLECODE,'') = ''"
                        strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_Man.Text & "'),'')"
                    End If
                End If
            Case "I"
                Dim dpcs As Integer = Val(dtStoneDetails.Compute("SUM(PCS)", "METALID='D'").ToString)

                strSql = "SELECT COUNT(*) FROM " & cnAdminDb & "..WMCTABLE "
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
                If cmbSubItem_Man.Enabled = True And cmbSubItem_Man.Text.Trim <> "" Then
                    strSql += vbCrLf + " AND SUBITEMID IN(SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "' AND ITEMID = ( SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'))"
                End If
                If cmbItemType_MAN.Enabled = True And cmbItemType_MAN.Text.Trim <> "" Then
                    strSql += vbCrLf + " AND ITEMTYPE IN((SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType_MAN.Text & "') UNION ALL SELECT 0 ) "
                End If
                strSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
                If dpcs <> 0 Then strSql += vbCrLf + " AND @DIAPCS BETWEEN DIAFROMPCS AND DIATOPCS "
                strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
                strSql += vbCrLf + " AND ISNULL(TABLECODE,'') = ''"
                strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_Man.Text & "'),'')"
                strSql += vbCrLf + " AND DESIGNERID = ISNULL((SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesigner_MAN.Text & "'),0)"

                If GetSqlRow(strSql, cn) Is Nothing Then
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
                    If cmbSubItem_Man.Enabled = True And cmbSubItem_Man.Text.Trim <> "" Then
                        strSql += vbCrLf + " AND SUBITEMID IN(SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "' AND ITEMID = ( SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'))"
                    End If
                    If cmbItemType_MAN.Enabled = True And cmbItemType_MAN.Text.Trim <> "" Then
                        strSql += vbCrLf + " AND ITEMTYPE IN((SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType_MAN.Text & "') UNION ALL SELECT 0 ) "
                    End If
                    strSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
                    If dpcs <> 0 Then strSql += vbCrLf + " AND @DIAPCS BETWEEN DIAFROMPCS AND DIATOPCS "
                    strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
                    strSql += vbCrLf + " AND ISNULL(TABLECODE,'') = ''"
                    strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_Man.Text & "'),'')"
                End If

                If GetSqlRow(strSql, cn) Is Nothing Then
                    If cmbItemType_MAN.Enabled = True And cmbItemType_MAN.Text.Trim <> "" And cmbItemType_MAN.Text.Trim <> "ALL" Then
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
                        If cmbSubItem_Man.Enabled = True And cmbSubItem_Man.Text.Trim <> "" Then
                            strSql += vbCrLf + " AND SUBITEMID =0"
                        End If
                        strSql += vbCrLf + " AND ITEMTYPE IN((SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType_MAN.Text & "') UNION ALL SELECT 0 ) "
                        strSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
                        If dpcs <> 0 Then strSql += vbCrLf + " AND @DIAPCS BETWEEN DIAFROMPCS AND DIATOPCS "
                        strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
                        strSql += vbCrLf + " AND ISNULL(TABLECODE,'') = ''"
                        strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_Man.Text & "'),'')"
                    End If
                    If GetSqlRow(strSql, cn) Is Nothing Then
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
                        If cmbSubItem_Man.Enabled = True And cmbSubItem_Man.Text.Trim <> "" Then
                            strSql += vbCrLf + " AND SUBITEMID IN(SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "' AND ITEMID = ( SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'))"
                        End If
                        strSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
                        If dpcs <> 0 Then strSql += vbCrLf + " AND @DIAPCS BETWEEN DIAFROMPCS AND DIATOPCS "
                        strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
                        strSql += vbCrLf + " AND ISNULL(TABLECODE,'') = ''"
                        strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_Man.Text & "'),'')"

                        If GetSqlRow(strSql, cn) Is Nothing Then
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
                            If cmbSubItem_Man.Enabled = True And cmbSubItem_Man.Text.Trim <> "" Then
                                strSql += vbCrLf + " AND SUBITEMID =0"
                            End If
                            strSql += vbCrLf + " AND @WT BETWEEN FROMWEIGHT AND TOWEIGHT"
                            If dpcs <> 0 Then strSql += vbCrLf + " AND @DIAPCS BETWEEN DIAFROMPCS AND DIATOPCS "
                            strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ''"
                            strSql += vbCrLf + " AND ISNULL(TABLECODE,'') = ''"
                            strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_Man.Text & "'),'')"
                        End If
                    End If
                End If
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
        If type = Nothing Or type.Trim = "" Then
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
                    If GetSoftValue("MAXMCFOCUS") = "Y" Then pnlMax.Enabled = True
                End If
                If Not setItem Then Exit Sub
            End If
        Else

            If tagEdit And dt.Rows.Count = 0 Then Exit Sub
            If tagEdit And chkFixedVa.Checked Then Exit Sub
            If tagEdit Then Exit Sub
            txtTouch_AMT.Clear()

            If LOCK_TAGEDIT_VA <> "" And tagEdit Then
                If Not LOCK_TAGEDIT_VA.Contains("W") Then objGPack.TextClear(txtMaxWastage_Per)
                If Not LOCK_TAGEDIT_VA.Contains("W") Then objGPack.TextClear(txtMaxWastage_Wet)
                If Not LOCK_TAGEDIT_VA.Contains("W") Then objGPack.TextClear(ObjMinValue.txtMinWastage_Per)
                If Not LOCK_TAGEDIT_VA.Contains("W") Then objGPack.TextClear(ObjMinValue.txtMinWastage_Wet)

                If Not LOCK_TAGEDIT_VA.Contains("M") Then objGPack.TextClear(txtMaxMcPerGrm_Amt)
                If Not LOCK_TAGEDIT_VA.Contains("M") Then objGPack.TextClear(txtMaxMkCharge_Amt)
                If Not LOCK_TAGEDIT_VA.Contains("M") Then objGPack.TextClear(ObjMinValue.txtMinMcPerGram_Amt)
                If Not LOCK_TAGEDIT_VA.Contains("M") Then objGPack.TextClear(ObjMinValue.txtMinMkCharge_Amt)
            Else
                objGPack.TextClear(pnlMax)
                objGPack.TextClear(ObjMinValue)
            End If
            'objGPack.TextClear(pnlValueAdded)
            With dt.Rows(0)

                Dim wmcWastPer As Double = Val(.Item("MAXWASTPER").ToString)
                Dim wmcWast As Double = Val(.Item("MAXWAST").ToString)
                Dim wmcMcGrm As Double = Val(.Item("MAXMCGRM").ToString)
                Dim wmcMc As Double = Val(.Item("MAXMC").ToString)
                If type = "I" Then
                    'If GetSoftValue("MAXMCFOCUS") = "N" Then
                    '    If wmcWastPer = 0 And wmcWast = 0 And wmcMcGrm = 0 And wmcMc = 0 Then pnlMax.Enabled = True Else pnlMax.Enabled = False
                    'End If
                    '  If dt.Rows.Count > 1 Then wmcMcGrm = Val(dt.Rows(1).Item("Maxmcgrm").ToString) : wmcMc = Val(dt.Rows(1).Item("Maxmc").ToString)
                End If
                If tagEdit Then
                    If Not LOCK_TAGEDIT_VA.Contains("W") Then
                        If wmcWastPer = 0 Then
                            txtMaxWastage_Wet.Text = IIf(Val(.Item("MAXWAST").ToString) <> 0, Format(Val(.Item("MAXWAST").ToString), "0.000"), "")
                        Else
                            txtMaxWastage_Per.Text = IIf(Val(.Item("MAXWASTPER").ToString) <> 0, Format(Val(.Item("MAXWASTPER").ToString), "0.00"), "")
                        End If
                    End If
                    If Not LOCK_TAGEDIT_VA.Contains("M") Then
                        If wmcMcGrm = 0 Then
                            txtMaxMkCharge_Amt.Text = IIf(Val(.Item("MAXMC").ToString) <> 0, Format(Val(.Item("MAXMC").ToString), "0.00"), "")
                        Else
                            txtMaxMcPerGrm_Amt.Text = IIf(Val(.Item("MAXMCGRM").ToString) <> 0, Format(Val(.Item("MAXMCGRM").ToString), "0.00"), "")
                        End If
                    End If
                Else
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
                End If


                wmcWastPer = Val(.Item("MINWASTPER").ToString)
                wmcWast = Val(.Item("MINWAST").ToString)
                wmcMcGrm = Val(.Item("MINMCGRM").ToString)
                wmcMc = Val(.Item("MINMC").ToString)
                txtTouch_AMT.Text = IIf(Val(.Item("TOUCH").ToString) <> 0, Format(Val(.Item("TOUCH").ToString), "0.00"), "")
                If tagEdit Then
                    If Not LOCK_TAGEDIT_VA.Contains("W") Then
                        If wmcWastPer = 0 Then
                            ObjMinValue.txtMinWastage_Wet.Text = IIf(Val(.Item("MINWAST").ToString) <> 0, Format(Val(.Item("MINWAST").ToString), "0.000"), "")
                        Else
                            ObjMinValue.txtMinWastage_Per.Text = IIf(Val(.Item("MINWASTPER").ToString) <> 0, Format(Val(.Item("MINWASTPER").ToString), "0.00"), "")
                        End If
                    End If
                    If Not LOCK_TAGEDIT_VA.Contains("M") Then
                        If wmcMcGrm = 0 Then
                            ObjMinValue.txtMinMkCharge_Amt.Text = IIf(Val(.Item("MINMC").ToString) <> 0, Format(Val(.Item("MINMC").ToString), "0.00"), "")
                        Else
                            ObjMinValue.txtMinMcPerGram_Amt.Text = IIf(Val(.Item("MINMCGRM").ToString) <> 0, Format(Val(.Item("MINMCGRM").ToString), "0.00"), "")
                        End If
                    End If
                Else
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
                End If
                txtMaxMkCharge_Org.Text = txtMaxMkCharge_Amt.Text
                txtMaxWastage_Org.Text = txtMaxWastage_Wet.Text
                If PUR_AUTOCALC Then
                    '',TOUCH_PUR,MAXWASTPER_PUR,MAXMCGRM_PUR,MAXWAST_PUR,MAXMC_PUR"
                    If lotPurTouch = 0 Then
                        ObjPurDetail.txtPurTouch_Amt.Text = IIf(Val(.Item("TOUCH_PUR").ToString) <> 0, Val(.Item("TOUCH_PUR").ToString), "")
                        ObjPurDetail.txtPurTouch_TextChanged(Me, New EventArgs)
                    End If
                    If lotPurWastagePer = 0 Then
                        ObjPurDetail.txtPurWastage_Per.Text = IIf(Val(.Item("MAXWASTPER_PUR").ToString) <> 0, Val(.Item("MAXWASTPER_PUR").ToString), "")
                        ObjPurDetail.txtPurWastage_Wet.Text = IIf(Val(.Item("MAXWAST_PUR").ToString) <> 0, Val(.Item("MAXWAST_PUR").ToString), "")
                        ObjPurDetail.txtPurWastagePer_TextChanged(Me, New EventArgs)
                    End If
                    If lotPurMcPerGrm = 0 Then
                        ObjPurDetail.txtPurMcPerGrm_Amt.Text = IIf(Val(.Item("MAXMCGRM_PUR").ToString) <> 0, Val(.Item("MAXMCGRM_PUR").ToString), "")
                        ObjPurDetail.txtPurMakingChrg_Amt.Text = IIf(Val(.Item("MAXMC_PUR").ToString) <> 0, Val(.Item("MAXMC_PUR").ToString), "")
                        ObjPurDetail.txtPurMcPerGrm_TextChanged(Me, New EventArgs)
                    End If
                End If

            End With
        End If
        If setItem = True Then
            ObjMinValue.txtMinWastage_Per.Text = "0"
            ObjMinValue.txtMinWastage_Wet.Text = "0"
            ObjMinValue.txtMinMkCharge_Amt.Text = "0"
            ObjMinValue.txtMinMcPerGram_Amt.Text = "0"
            txtMaxWastage_Per.Text = objSetItem.txtSetWastagePer_Per.Text.ToString
            If Val(txtMaxWastage_Per.Text.ToString) = 0 Then
                txtMaxWastage_Wet.Text = objSetItem.txtSetWastage_WET.Text.ToString
            End If
            txtMaxMcPerGrm_Amt.Text = objSetItem.txtSetMcPerGrm_AMT.Text.ToString
            If Val(txtMaxMcPerGrm_Amt.Text.ToString) = 0 Then
                txtMaxMkCharge_Amt.Text = objSetItem.txtSetMc_AMT.Text.ToString
            End If
            Exit Sub
        End If
    End Sub

    Private Sub cmbTableCode_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTableCode.LostFocus
        If TAG_TBLCODESEARCHOPT Then cmbTableCode.Size = New System.Drawing.Size(87, 21)
    End Sub

    Private Sub cmbTableCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTableCode.SelectedIndexChanged
        CalcMaxMinValues()
        CalcFinalTotal()
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
    Private Sub GetGrsWeightFromPort_Format3()
        ''Dim Wt_Balance_Sep As String = GetAdmindbSoftValue("WT_BALANCE_SEP", "")
        Dim Wt_Balance_Sep As String = ""
        Wt_Balance_Sep = GetAdmindbSoftValue(System.Net.Dns.GetHostName().ToUpper & "-WT_BALANCE_SEP", "", Nothing, True).ToString
        If Wt_Balance_Sep = "" Then
            Wt_Balance_Sep = GetAdmindbSoftValue("WT_BALANCE_SEP", "", Nothing, True).ToString
        End If
Getnext:
        Dim Weight As Double = Nothing
        Try
            Dim objwt As New WtTran.clsGetWt()
            Weight = Val(objwt.GetWt(Port_BaudRate, Port_PortName).ToString)
        Catch ex As Exception

        End Try
        If Weight = 0 Then txtGrossWt_Wet.Text = "" : GoTo Getnext : Exit Sub
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
        txtGrossWt_Wet.ReadOnly = True
        If AUTOWTSKIPEDIT Then
            Me.SelectNextControl(txtGrossWt_Wet, True, True, False, False)
        Else
            txtGrossWt_Wet.SelectAll()
        End If
    End Sub
    Private Sub GetGrsWeightFromPort_Format2()
        ''Dim Wt_Balance_Sep As String = GetAdmindbSoftValue("WT_BALANCE_SEP", "")
        Dim Wt_Balance_Sep As String = ""
        Wt_Balance_Sep = GetAdmindbSoftValue(System.Net.Dns.GetHostName().ToUpper & "-WT_BALANCE_SEP", "", Nothing, True).ToString
        If Wt_Balance_Sep = "" Then
            Wt_Balance_Sep = GetAdmindbSoftValue("WT_BALANCE_SEP", "", Nothing, True).ToString
        End If
Getnext:
        Dim Weight As Double = Nothing
        Try
            If Not SerialPort1.IsOpen Then SerialPort1.Open()
            If SerialPort1.IsOpen Then
                Dim readStr As String = Nothing
                If Wt_Balance_Sep <> "" Then
                    readStr = UCase(SerialPort1.ReadTo(Wt_Balance_Sep))
                    If IsNumeric(readStr.Substring(readStr.Length - 6)) Then
                        readStr = (Val(readStr.Substring(readStr.Length - 6)) / 1000).ToString()
                    End If
                    Weight = Val(Trim(readStr))
                Else
                    For cnt As Integer = 1 To 10
                        Thread.Sleep(500)
                        readStr = UCase(SerialPort1.ReadExisting)
                        If readStr.Contains(".") Then
                            Exit For
                        End If
                    Next
                    Dim wt() As String = readStr.Split(Environment.NewLine)
                    Dim wet As String = ""
                    Dim Index As Integer = wt.Length - 1
                    If Not wt(Index).Contains(".") Then Index -= 2
                    For Each c As Char In wt(Index)
                        If c = "," Or c = "?" Then Continue For
                        If Char.IsPunctuation(c) Then wet += c
                        If Char.IsNumber(c) Then wet += c
                    Next
                    Weight = Val(Trim(wet))
                    ReadData.Text = readStr
                    SplitData.Text = wt(0)
                    ModifyData.Text = wet
                End If
            End If
            If SerialPort1.IsOpen Then SerialPort1.Close()
        Catch ex As Exception

        End Try
        If Weight = 0 Then txtGrossWt_Wet.Text = "" : GoTo Getnext : Exit Sub
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
        txtGrossWt_Wet.ReadOnly = True
        If AUTOWTSKIPEDIT Then
            Me.SelectNextControl(txtGrossWt_Wet, True, True, False, False)
        Else
            txtGrossWt_Wet.SelectAll()
        End If
    End Sub
    Private Sub GetGrsWeightFromPort()

        ''Dim Wt_Balance_Sep As String = GetAdmindbSoftValue("WT_BALANCE_SEP", "")
        Dim Wt_Balance_Sep As String = ""
        Wt_Balance_Sep = GetAdmindbSoftValue(System.Net.Dns.GetHostName().ToUpper & "-WT_BALANCE_SEP", "", Nothing, True).ToString
        If Wt_Balance_Sep = "" Then
            Wt_Balance_Sep = GetAdmindbSoftValue("WT_BALANCE_SEP", "", Nothing, True).ToString
        End If

Getnext:
        Dim Weight As Double = Nothing
        Try
            If SerialPort1.IsOpen Then SerialPort1.Close()
            SerialPort1.Open()
            If SerialPort1.IsOpen Then
                Dim readStr As String = Nothing
                If Wt_Balance_Sep <> "" Then
                    readStr = UCase(SerialPort1.ReadTo(Wt_Balance_Sep))
                    If IsNumeric(readStr.Substring(readStr.Length - 6)) Then
                        readStr = (Val(readStr.Substring(readStr.Length - 6)) / 1000).ToString()
                    End If
                    Weight = Val(Trim(readStr))
                Else
                    For cnt As Integer = 1 To 10
                        readStr = UCase(SerialPort1.ReadLine)
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
            If SerialPort1.IsOpen Then SerialPort1.Close()

        Catch ex As Exception
            txtTagNo__Man.Focus()
            MsgBox("Please check com connection" + vbCrLf + ex.Message + vbCrLf + ex.StackTrace)
            If SerialPort1.IsOpen Then SerialPort1.Close()
        End Try
        If Weight = 0 Then txtGrossWt_Wet.Text = "" : GoTo Getnext : Exit Sub
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
        If AUTOWTSKIPEDIT Then
            Me.SelectNextControl(txtGrossWt_Wet, True, True, False, False)
        Else
            txtGrossWt_Wet.SelectAll()
        End If
    End Sub


    Private Sub txtGrossWt_Wet_Man_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGrossWt_Wet.GotFocus
        Dim Itemid As Integer = Val(GetSqlValue(cn, "SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"))
        If tagEdit Then
            If FuncCheckTransactionDetails(Itemid, txtTagNo__Man.Text) Then
                txtPieces_Num_Man.ReadOnly = True
                txtGrossWt_Wet.ReadOnly = True
                txtNetWt_Wet.ReadOnly = True
            End If
        End If
        If chkAutomaticWt.Checked = True Then
            If WT_BALANCE_FORMAT = 2 Then
                If txtGrossWt_Wet.ReadOnly = False Then
                    'GetGrsWeightFromPort_Format2()
                    GetGrsWeightFromPort_Format3()
                End If
            Else
                txtGrossWt_Wet.ReadOnly = True
                GetGrsWeightFromPort()
            End If
        Else
            txtGrossWt_Wet.ReadOnly = False
            If TAGEDITPCSWT = "N" And tagEdit Then txtGrossWt_Wet.ReadOnly = True Else txtGrossWt_Wet.ReadOnly = False
            If TAGEDITPCSWT = "A" And tagEdit And Authorize Then txtGrossWt_Wet.ReadOnly = False
            If TAGEDITPCSWT = "A" And tagEdit And Not Authorize Then txtGrossWt_Wet.ReadOnly = True
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
            If cmbCalType.Enabled Then
                calcType = Mid(cmbCalType.Text, 1, 1)
            End If
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
            Dim tempSitemid As Integer = 0
            If cmbSubItem_Man.Text <> "" Then
                tempSitemid = objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE ISNULL(SUBITEMNAME,'') = '" & cmbSubItem_Man.Text & "'")
            End If
            Dim RangeExclude As Boolean = False
            Dim ITEMIDS() As String
            ITEMIDS = EXCLUDE_RANGE.Split(",")
            If EXCLUDE_RANGE <> "" Then
                For I As Integer = 0 To ITEMIDS.Length - 1
                    If tempitemid = ITEMIDS(I) Then
                        RangeExclude = True
                        Exit For
                    End If
                Next
            End If
            strSql = " SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"
            Dim MetalId As String = objGPack.GetSqlValue(strSql)
            If TagNo_Range And MetalId <> "S" And RangeExclude = False Then
                strSql = " SELECT TOP 1 CAPTION FROM " & cnAdminDb & "..RANGEMAST WHERE ITEMID=" & tempitemid & " AND SUBITEMID=" & tempSitemid & " AND " & Val(txtGrossWt_Wet.Text) & " BETWEEN FROMWEIGHT AND TOWEIGHT "
                If objGPack.GetSqlValue(strSql, , "") = "" Then MsgBox("Please set Range...", MsgBoxStyle.Information) : txtGrossWt_Wet.Focus() : Exit Sub
            End If
            If TagNo_RangeBase And MetalId <> "S" And tagEdit = False Then
                strSql = " SELECT TOP 1 CAPTION FROM " & cnAdminDb & "..RANGEMAST WHERE ITEMID=" & tempitemid & "   AND " & Val(txtGrossWt_Wet.Text) & " BETWEEN FROMWEIGHT AND TOWEIGHT "
                StrCaption = objGPack.GetSqlValue(strSql, , "")
                If StrCaption = "" Then MsgBox("Please set Range...", MsgBoxStyle.Information) : txtGrossWt_Wet.Focus() : Exit Sub
                strSql = " SELECT TOP 1 TAGNO FROM " & cnAdminDb & "..RANGEMAST WHERE ITEMID=" & tempitemid & " AND " & Val(txtGrossWt_Wet.Text) & " BETWEEN FROMWEIGHT AND TOWEIGHT  "
                StrTagNo = (Val(objGPack.GetSqlValue(strSql, , "", tran)) + 1).ToString
ReChk:
                strSql = " SELECT 1 FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO='" & StrCaption.ToString.Trim & "-" & StrTagNo.ToString.Trim & "' "
                If Val(objGPack.GetSqlValue(strSql, , "", tran)) > 0 Then
                    StrTagNo = (Val(StrTagNo.ToString) + 1).ToString
                    GoTo ReChk
                End If
                txtTagNo__Man.Text = StrCaption.ToString.Trim & "-" & StrTagNo.ToString.Trim
            End If

            If FuncCheckTransactionDetails(tempitemid, txtTagNo__Man.Text) Then
                txtPieces_Num_Man.ReadOnly = True
                txtGrossWt_Wet.ReadOnly = True
                txtNetWt_Wet.ReadOnly = True
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
                    If tempwtunit = "C" Then stnwt = stnwt * 5
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
            Dim tempDesignerid As Integer
            If cmbDesigner_MAN.Text <> "" Then tempDesignerid = objGPack.GetSqlValue("SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE ISNULL(DESIGNERNAME,'') = '" & cmbDesigner_MAN.Text & "'")
            dtdesgstnrow = GetSqlRow("SELECT STNDEDPER,STN_RATE,STNITEMID,STNSUBITEMID,CALTYPE,STONEUNIT FROM " & cnAdminDb & "..DESIGNERSTONE WHERE ACTIVE <> 'N' AND DESIGNERID = " & tempDesignerid & " AND ITEMID = " & tempitemid & " AND SUBITEMID = " & tempSitemid, cn)
            If dtdesgstnrow Is Nothing Then
                dtdesgstnrow = GetSqlRow("SELECT STNDEDPER,STN_RATE,STNITEMID,STNSUBITEMID,CALTYPE,STONEUNIT FROM " & cnAdminDb & "..DESIGNERSTONE WHERE ACTIVE <> 'N' AND DESIGNERID = 0 AND ITEMID = " & tempitemid & " AND SUBITEMID = " & tempSitemid, cn)
            End If
            If Not dtdesgstnrow Is Nothing And tagEdit = False Then
                Dim Stnitemid As Integer = Val(dtdesgstnrow(2).ToString)
                If Stnitemid = 0 Then Stnitemid = 9999

                Dim StnSitemid As Integer = Val(dtdesgstnrow(3).ToString)
                Dim dtstnitemdr As DataRow
                Dim dtstnsitemdr As DataRow
                Dim tempcalmode As String = dtdesgstnrow(4).ToString
                Dim tempwtunit As String = dtdesgstnrow(5).ToString
                dtstnitemdr = GetSqlRow("SELECT ITEMNAME,CALTYPE,STONEUNIT FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Stnitemid, cn)
                If tempwtunit = "" Then tempwtunit = dtstnitemdr(2).ToString
                If tempcalmode = "" Then tempcalmode = dtstnitemdr(1).ToString
                If StnSitemid <> 0 Then
                    dtstnsitemdr = GetSqlRow("SELECT SUBITEMNAME,CALTYPE,STONEUNIT FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = " & StnSitemid, cn)
                    If tempwtunit = "" Then tempwtunit = dtstnsitemdr(2).ToString
                    If tempcalmode = "" Then tempcalmode = dtstnsitemdr(1).ToString
                End If

                Dim stnwt As Decimal = (Val(dtdesgstnrow(0).ToString) / 100) * Val(txtGrossWt_Wet.Text)
                If tempwtunit = "C" Then stnwt = stnwt * 5
                Dim drstn As DataRow

                dtStoneDetails.Rows.Clear()
                drstn = dtStoneDetails.NewRow
                drstn("ITEM") = dtstnitemdr(0).ToString
                drstn("METALID") = "S"
                drstn("SUBITEM") = dtstnsitemdr(0).ToString
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
                StyleGridStone()
                If TabControl1.TabPages.Contains(tabStone) Then
                    TabControl1.SelectedTab = tabStone
                    If PacketNoEnable <> "N" Then
                        txtstPackettno.Focus()
                    Else
                        Me.SelectNextControl(tabStone, True, True, True, True)
                    End If
                End If
                DesignerStone = True
                Exit Sub
            End If
StudContinue:
            If _FourCMaintain Then
                strSql = "SELECT STUDDED FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & tempitemid & "  AND METALID='D'"
                If objGPack.GetSqlValue(strSql).ToString.ToUpper = "L" Then
                    If Not tagEdit Then
                        ' ObjDiaDetails = New frm4C Reason comment on 18-08-2018
                    End If
                    If ORDER_MULTI_MIMR Then
                        ObjDiamondDetails.cmbStnGrp.Focus()
                        ObjDiamondDetails.Location = New Drawing.Point(481, 200)
                        ObjDiamondDetails.Size = New Drawing.Size(261, 235) ' 175
                        Dim view4c As String = ""
                        If cmbItem_MAN.Text <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItem_MAN.Text & "'", , , )
                        If cmbSubItem_Man.Text <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & cmbSubItem_Man.Text & "' AND ITEMID=(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItem_MAN.Text & "')", , , )
                        If Not view4c.Contains("CU") Then ObjDiamondDetails.CmbCut.Enabled = False
                        If Not view4c.Contains("CO") Then ObjDiamondDetails.CmbColor.Enabled = False
                        If Not view4c.Contains("CL") Then ObjDiamondDetails.CmbClarity.Enabled = False
                        If Not view4c.Contains("SH") Then ObjDiamondDetails.cmbShape.Enabled = False
                        If Not view4c.Contains("SI") Then ObjDiamondDetails.cmbShape.Enabled = False ' NOT WORKING
                        If Not view4c.Contains("SE") Then ObjDiamondDetails.cmbSetType.Enabled = False
                        If Not view4c.Contains("HE") Then ObjDiamondDetails.txtHeight_WET.Enabled = False
                        If Not view4c.Contains("WI") Then ObjDiamondDetails.txtWidth_WET.Enabled = False
                    Else
                        ObjDiaDetails.CmbCut.Focus()
                        ObjDiaDetails.Location = New Drawing.Point(481, 200)
                        ObjDiaDetails.Size = New Drawing.Size(261, 195) ' 175
                        Dim view4c As String = ""
                        If cmbItem_MAN.Text <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItem_MAN.Text & "'", , , )
                        If cmbSubItem_Man.Text <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & cmbSubItem_Man.Text & "' AND ITEMID=(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItem_MAN.Text & "')", , , )
                        If Not view4c.Contains("CU") Then ObjDiaDetails.CmbCut.Enabled = False
                        If Not view4c.Contains("CO") Then ObjDiaDetails.CmbColor.Enabled = False
                        If Not view4c.Contains("CL") Then ObjDiaDetails.CmbClarity.Enabled = False
                        If Not view4c.Contains("SH") Then ObjDiaDetails.cmbShape.Enabled = False
                        If Not view4c.Contains("SI") Then ObjDiaDetails.cmbShape.Enabled = False ' NOT WORKING
                        If Not view4c.Contains("SE") Then ObjDiaDetails.cmbSetType.Enabled = False
                        If Not view4c.Contains("HE") Then ObjDiaDetails.txtHeight_WET.Enabled = False
                        If Not view4c.Contains("WI") Then ObjDiaDetails.txtWidth_WET.Enabled = False
                    End If
                    Dim _dt4c As New DataTable

                    strSql = vbCrLf + " SELECT "
                    strSql += vbCrLf + " (SELECT GROUPNAME FROM " & cnAdminDb & "..STONEGROUP WHERE GROUPID=L.STNGRPID)STNGROUP,"
                    strSql += vbCrLf + " (SELECT CUTNAME FROM " & cnAdminDb & "..STNCUT WHERE CUTID=L.CUTID)CUT,"
                    strSql += vbCrLf + " (SELECT COLORNAME FROM " & cnAdminDb & "..STNCOLOR WHERE COLORID=L.COLORID)COLOR,"
                    strSql += vbCrLf + " (SELECT SHAPENAME FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPEID=L.SHAPEID)SHAPE,"
                    strSql += vbCrLf + " (SELECT CLARITYNAME FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYID=L.CLARITYID)CLARITY,"
                    strSql += vbCrLf + " (SELECT SETTYPENAME FROM " & cnAdminDb & "..STNSETTYPE WHERE SETTYPEID=L.SETTYPEID)SETTYPE,HEIGHT,WIDTH "
                    If tagEdit = True Then
                        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS L WHERE LOTSNO='" & SNO & "' AND SNO = '" & updIssSno & "'"
                    Else
                        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMLOT AS L WHERE SNO='" & SNO & "'"
                    End If
                    cmd = New OleDbCommand(strSql, cn)
                    da = New OleDbDataAdapter(cmd)
                    da.Fill(_dt4c)
                    If _dt4c.Rows.Count > 0 Then
                        If ORDER_MULTI_MIMR Then
                            ObjDiamondDetails.cmbStnGrp.Text = _dt4c.Rows(0)("STNGROUP").ToString
                            ObjDiamondDetails.CmbCut.Text = _dt4c.Rows(0)("CUT").ToString
                            ObjDiamondDetails.CmbColor.Text = _dt4c.Rows(0)("COLOR").ToString
                            ObjDiamondDetails.cmbShape.Text = _dt4c.Rows(0)("SHAPE").ToString
                            ObjDiamondDetails.CmbClarity.Text = _dt4c.Rows(0)("CLARITY").ToString
                            ObjDiamondDetails.cmbSetType.Text = _dt4c.Rows(0)("SETTYPE").ToString
                            ObjDiamondDetails.txtHeight_WET.Text = _dt4c.Rows(0)("HEIGHT").ToString
                            ObjDiamondDetails.txtWidth_WET.Text = _dt4c.Rows(0)("WIDTH").ToString
                        Else
                            ObjDiaDetails.CmbCut.Text = _dt4c.Rows(0)("CUT").ToString
                            ObjDiaDetails.CmbColor.Text = _dt4c.Rows(0)("COLOR").ToString
                            ObjDiaDetails.cmbShape.Text = _dt4c.Rows(0)("SHAPE").ToString
                            ObjDiaDetails.CmbClarity.Text = _dt4c.Rows(0)("CLARITY").ToString
                            ObjDiaDetails.cmbSetType.Text = _dt4c.Rows(0)("SETTYPE").ToString
                            ObjDiaDetails.txtHeight_WET.Text = _dt4c.Rows(0)("HEIGHT").ToString
                            ObjDiaDetails.txtWidth_WET.Text = _dt4c.Rows(0)("WIDTH").ToString
                        End If
                    End If
                    If ORDER_MULTI_MIMR Then
                        ObjDiamondDetails.ShowDialog()
                        TagStnGrpId = Val(objGPack.GetSqlValue(" SELECT GROUPID FROM " & cnAdminDb & "..STONEGROUP WHERE GROUPNAME = '" & ObjDiamondDetails.cmbStnGrp.Text & "'", "GROUPID", , tran))
                        TagCutId = Val(objGPack.GetSqlValue(" SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & ObjDiamondDetails.CmbCut.Text & "'", "CUTID", , tran))
                        TagColorId = Val(objGPack.GetSqlValue(" SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME = '" & ObjDiamondDetails.CmbColor.Text & "'", "COLORID", , tran))
                        TagShapeId = Val(objGPack.GetSqlValue(" SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & ObjDiamondDetails.cmbShape.Text & "'", "SHAPEID", , tran))
                        TagClarityId = Val(objGPack.GetSqlValue(" SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & ObjDiamondDetails.CmbClarity.Text & "'", "CLARITYID", , tran))
                        TagSetTypeId = Val(objGPack.GetSqlValue(" SELECT SETTYPEID FROM " & cnAdminDb & "..STNSETTYPE WHERE SETTYPENAME = '" & ObjDiamondDetails.cmbSetType.Text & "'", "SETTYPEID", , tran))
                        TagHeight = Val(ObjDiamondDetails.txtHeight_WET.Text.ToString)
                        TagWidth = Val(ObjDiamondDetails.txtWidth_WET.Text.ToString)
                    Else
                        ObjDiaDetails.ShowDialog()
                        TagCutId = Val(objGPack.GetSqlValue(" SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & ObjDiaDetails.CmbCut.Text & "'", "CUTID", , tran))
                        TagColorId = Val(objGPack.GetSqlValue(" SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME = '" & ObjDiaDetails.CmbColor.Text & "'", "COLORID", , tran))
                        TagShapeId = Val(objGPack.GetSqlValue(" SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & ObjDiaDetails.cmbShape.Text & "'", "SHAPEID", , tran))
                        TagClarityId = Val(objGPack.GetSqlValue(" SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & ObjDiaDetails.CmbClarity.Text & "'", "CLARITYID", , tran))
                        TagSetTypeId = Val(objGPack.GetSqlValue(" SELECT SETTYPEID FROM " & cnAdminDb & "..STNSETTYPE WHERE SETTYPENAME = '" & ObjDiaDetails.cmbSetType.Text & "'", "SETTYPEID", , tran))
                        TagHeight = Val(ObjDiaDetails.txtHeight_WET.Text.ToString)
                        TagWidth = Val(ObjDiaDetails.txtWidth_WET.Text.ToString)
                    End If
                End If
            End If
            Dim ORDREPNO As String = ""
            If Not OrderRow Is Nothing Then
                ORDREPNO = objGPack.GetSqlValue("SELECT ORDREPNO  FROM   " & cnAdminDb & "..ITEMLOT WHERE LOTNO = '" & txtLotNo_Num_Man.Text.ToString & "' And ISNULL(ORDREPNO,'') <> '' AND ISNULL(ENTRYTYPE,'') ='RE' And ISNULL(ORDREPNO,'') = '" & OrderRow.Item("ORNO").ToString & "'", "ORDREPNO", "")
            End If
            If ORDREPNO <> "" Then
                RepairLot = True
                Dim OREXTRAWT As Double
                If tagEdit Then
                    OREXTRAWT = Val(objGPack.GetSqlValue("SELECT TOP 1 OREXCESSWT FROM " & cnAdminDb & ".. ITEMTAG WHERE ISNULL(SNO,'') = '" & updIssSno.ToString & "' ").ToString)
                Else
                    OREXTRAWT = Val(objGPack.GetSqlValue("SELECT EXCESSWT FROM " & cnAdminDb & ".. ORIRDETAIL WHERE ISNULL(ORNO,'') = '" & OrderRow.Item("ORNO").ToString & "' AND ISNULL(ORSNO,'') = '" & OrderRow.Item("SNO").ToString & "'  AND ISNULL(ORSTATUS,'') ='R' AND ISNULL(CANCEL,'')<>'Y'").ToString)
                End If
                ObjExtraWt.txtExtraWt_WET.Text = OREXTRAWT
                ObjExtraWt.txtExtraWt_WET.Focus()
                ObjExtraWt.ShowDialog()
            Else
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
                    RepairLot = False
                Else
                    'ObjExtraWt.Visible = False
                    RepairLot = False
                End If
            End If

            If objGPack.GetSqlValue("SELECT 1 FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & tempitemid & " And (COVERWT='Y' OR TAGWT='Y')", "", "-1") = "1" Then
                strSql = "SELECT ISNULL(TAGWT,0)TAGWT,ISNULL(COVERWT,0)COVERWT FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME='" & cmbCounter_MAN.Text & "'"
                Dim dr As DataRow
                dr = GetSqlRow(strSql, cn)
                If Not dr Is Nothing Then
                    ObjTagWt.txtTagWt_WET.Text = Val(dr("TAGWT").ToString)
                    ObjTagWt.txtCoverWt_WET.Text = Val(dr("COVERWT").ToString)
                Else
                    ObjTagWt.txtTagWt_WET.Text = 0
                    ObjTagWt.txtCoverWt_WET.Text = 0
                End If
                'ObjTagWt.txtCoverWt_WET.Focus()
                'ObjTagWt.ShowDialog()
            End If
            strSql = " SELECT MCASVAPER FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & tempitemid & ""
            If objGPack.GetSqlValue(strSql).ToString = "Y" Then Label44.Text = "MC PERCENT" Else Label44.Text = "Max Mc Per Grm"
            If cmbSubItem_Man.Text <> "" Then
                strSql = " SELECT MCASVAPER FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = " & tempitemid & " AND SUBITEMID=" & tempSitemid & ""
                If objGPack.GetSqlValue(strSql).ToString = "Y" Then Label44.Text = "MC PERCENT" Else Label44.Text = "Max Mc Per Grm"
            End If

            With TabControl1
                If .TabPages.Contains(tabMultiMetal) Then
                    .SelectedTab = tabMultiMetal
                    Me.SelectNextControl(tabMultiMetal, True, True, True, True)
                ElseIf .TabPages.Contains(tabStone) Then
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
            If WT_BALANCE_FORMAT = 2 Then
                txtGrossWt_Wet.Text = ""
                'GetGrsWeightFromPort_Format2()
                GetGrsWeightFromPort_Format3()
            Else
                GetGrsWeightFromPort()
            End If
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

    Function CheckCounterstk(ByVal MitemId As String) As Decimal
        Dim Mcostid As String = ""
        Dim Mgrswt As Decimal = 0
        If cmbCostCentre_Man.Text <> "" Then Mcostid = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre_Man.Text & "'")
        strSql = " SELECT SUM(ISNULL(GRSWT,0)) GRSWT FROM ("
        strSql += vbCrLf + " SELECT SUM(GRSWT) AS GRSWT FROM " & cnAdminDb & "..ITEMTAG WHERE COMPANYID='" & GetStockCompId() & "' AND ISNULL(COSTID,'')='" & Mcostid & "' AND ISSDATE IS NULL "
        strSql += vbCrLf + " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE CATCODE='" & MitemId & "')"
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
            If TAGIMAGE = False Then
                SubItemPic = False
                btnSave.Focus()
                Exit Sub
            End If
            piccap()
            SubItemPic = False
            btnSave.Focus()
            Exit Sub
        End If
        Try
            Dim openDia As New OpenFileDialog
            Dim str As String
            If IO.Directory.Exists(defalutSourcePath) Then openDia.InitialDirectory = defalutSourcePath
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
        data = Clipboard.GetDataObject()
        If data.GetDataPresent(GetType(System.Drawing.Bitmap)) Then
            bmap = CType(data.GetData(GetType(System.Drawing.Bitmap)), Image)
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

        hHwnd = capCreateCaptureWindowA(iDevice, WS_VISIBLE Or WS_CHILD, 0, 0, 640,
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
            SetWindowPos(hHwnd, HWND_BOTTOM, 0, 0, picCapture.Width, picCapture.Height,
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

    Private Sub txtPieces_Num_Man_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPieces_Num_Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Val(txtPieces_Num_Man.Text.ToString) = 0 And cmbItem_MAN.Text.ToString <> "" Then
                strSql = " SELECT 1 FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItem_MAN.Text.ToString & "' AND ISNULL(ALLOWZEROPCS,'')='Y' "
                If Val(objGPack.GetSqlValue(strSql, , "")) = 0 Then
                    MsgBox("Pcs should not be zero", MsgBoxStyle.Information)
                    txtPieces_Num_Man.Focus()
                End If
            End If
        End If
    End Sub

    Private Sub txtPieces_Num_Man_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPieces_Num_Man.Leave
        If tabCheckBy = "P" And TagLotCheck = True Then
            If Val(lblPBalance.Text) + Val(Tag_Tolerance) < Val(txtPieces_Num_Man.Text) - lotPcs Then
                MsgBox("Pcs should not exceed lot balance pcs", MsgBoxStyle.Information)
                txtPieces_Num_Man.Focus()
            End If
            If STK_REORD_VALID = True Then StockChecking()
        End If
        If GetAdmindbSoftValue("SPECIFIED_STYLENO", "N") = "Y" And tagEdit = True Then
            txtStyleCode.Text = funcspecifiedstyleno()
            txtStyleCode.ReadOnly = True
        Else
            txtStyleCode.ReadOnly = False
        End If
    End Sub

    Private Sub txtPieces_Num_Man_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPieces_Num_Man.TextChanged
        CalcFinalTotal()
    End Sub

    Private Sub txtGrossWt_Wet_Man_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGrossWt_Wet.Leave
        If tabCheckBy <> "P" Then
            If Val(lblWBalance.Text) + Val(Tag_Tolerance) < Val(txtGrossWt_Wet.Text) - lotGrsWt And TAGWOLOT = False Then
                MsgBox("Weight should not exceed lot balance weight", MsgBoxStyle.Information)
                txtGrossWt_Wet.Text = 0
                txtGrossWt_Wet.Focus()
            End If
        End If
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

        If CHK_BOOK_CTR_STOCK And Val(txtGrossWt_Wet.Text.ToString) > 0 Then
            'Check Book stock and counter stock
            Dim tempcatcode As String = objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') = '" & cmbItem_MAN.Text & "'").ToString
            TChkbStk = False
            'objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') = '" & cmbItem_MAN.Text & "'").ToString) + Val(txtGrossWt_Wet.Text.ToString
            If CheckBookstk(tempcatcode) < CheckCounterstk(tempcatcode) + Val(txtGrossWt_Wet.Text.ToString) Then
                MsgBox("Book stock weight is less than counter stock.")
                txtGrossWt_Wet.Text = ""
                btnSave.Enabled = False
                'txtGrossWt_Wet.Focus()
                txtPieces_Num_Man.Focus()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub txtNetWt_Wet_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNetWt_Wet.GotFocus
        If Val(txtGrossWt_Wet.Text) = 0 Then SendKeys.Send("{TAB}") : Exit Sub
        If TAGEDITPCSWT = "A" Then
            Dim wt As Double = Nothing
            wt = Val(txtGrossWt_Wet.Text) - Val(txtLessWt_Wet.Text)
            strSql = "SELECT ISNULL(NETWTPER,0) NETWTPER FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"
            Dim mnetwtper As Double = Val(objGPack.GetSqlValue(strSql).ToString)
            If mnetwtper <> 0 Then wt = wt * (mnetwtper / 100)
            txtNetWt_Wet.Text = IIf(wt <> 0, Format(wt, "0.000"), "")
        End If
        CalcMaxMinValues()
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
        If TAGEDITPCSWT = "A" Then
            If Val(txtGrossWt_Wet.Text.ToString) < Val(txtNetWt_Wet.Text.ToString) Then
                txtNetWt_Wet.Text = Format(Val(txtGrossWt_Wet.Text.ToString), "0.000")
            End If
        End If
        If tagEdit Then txtMaxWastagePer_TextChanged(Me, New System.EventArgs)
        If tagEdit Then txtMaxMcPerGrm_TextChanged(Me, New System.EventArgs)
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
            If cmbCalType.Enabled Then
                calcType = Mid(cmbCalType.Text, 1, 1)
            End If
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

                'ObjMinValue = New TagMinValues
                AddHandler ObjMinValue.txtMinWastage_Per.TextChanged, AddressOf ObjMinValues_txtMinWastage_Per_TextChanged
                AddHandler ObjMinValue.txtMinMcPerGram_Amt.TextChanged, AddressOf ObjMinValues_txtMinMcPerGram_Amt_TextChanged
                If tagEdit And TAGEDITDISABLE.Contains("MC") Then ObjMinValue.txtMinMcPerGram_Amt.ReadOnly = True : ObjMinValue.txtMinMkCharge_Amt.ReadOnly = True
                If tagEdit And TAGEDITDISABLE.Contains("WS") Then ObjMinValue.txtMinWastage_Per.ReadOnly = True : ObjMinValue.txtMinWastage_Wet.ReadOnly = True
                ObjMinValue.txtMinWastage_Per.Select()
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
                If MetalBasedStone Then
                    txtMMNetwt_Wet.Text = .Cells("NETWT").FormattedValue
                End If
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
        If MetalBasedStone And dtStoneDetails.Rows.Count > 0 Then
            Dim tempkeyno As String = ""
            If gridMultimetal.Rows.Count > 0 Then
                For Each drm As DataRow In dtMultiMetalDetails.Rows
                    tempkeyno += drm("keyno").ToString & ","
                Next
                If tempkeyno.ToString <> "" Then tempkeyno = tempkeyno.Substring(0, tempkeyno.Length - 1)
            Else
                tempkeyno = ""
            End If
            If tempkeyno.ToString = "" Then
                dtStoneDetails.Rows.Clear()
            Else
xxx:
                For cnt As Integer = 0 To dtStoneDetails.Rows.Count - 1
                    If Not dtStoneDetails.Rows(cnt).Item("MKEYNO").ToString.Contains(tempkeyno.ToString) Then
                        dtStoneDetails.Rows.RemoveAt(cnt)
                        dtStoneDetails.AcceptChanges()
                        GoTo xxx
                    End If
                Next
            End If
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
                If ACC_STUDITEM_POPUP Then txtStItem.Focus() Else cmbStItem.Focus()
            End If
        End If

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
            strSql = " SELECT ISNULL(STUDDED,'') STUDDED FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') ='" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "' AND METALID = 'D'"
            Studded_Loose = objGPack.GetSqlValue(strSql, "STUDDED", "")
            If NEEDUS = True And Studded_Loose <> "" And Val(txtStRate_Amt.Text) = 0 Then
                'If calType = "M" Then
                If ObjRsUs.ShowDialog = Windows.Forms.DialogResult.OK Then
                    txtStRate_Amt.Text = Convert.ToDouble(Val(ObjRsUs.txtUSDollar_Amt.Text) * Val(ObjRsUs.txtIndRs_Amt.Text)).ToString("0.00")
                End If
                'End If
            End If
            Dim cent As Double = 0

            strSql = " SELECT ISNULL(CALTYPE,'') CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') ='" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "'"
            Dim mCaltype As String = objGPack.GetSqlValue(strSql, "CALTYPE", "")
            strSql = " SELECT ISNULL(CALTYPE,'') CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST "
            strSql += " WHERE ISNULL(SUBITEMNAME,'') ='" & IIf(ACC_STUDITEM_POPUP, txtStSubItem.Text, CmbStSubItem.Text) & "' "
            strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "')"
            mCaltype = objGPack.GetSqlValue(strSql, "CALTYPE", "")
            If mCaltype = "D" Then
                cent = Val(txtStWeight.Text)
            Else
                cent = (Val(txtStWeight.Text) / IIf(Val(txtStPcs_Num.Text) = 0, 1, Val(txtStPcs_Num.Text)))
                'If cmbStUnit.Text = "C" Then cent = (Val(txtStWeight.Text) / IIf(Val(txtStPcs_Num.Text) = 0, 1, Val(txtStPcs_Num.Text))) Else cent = Val(txtStWeight.Text) / IIf(Val(txtStPcs_Num.Text) = 0, 1, Val(txtStPcs_Num.Text))
            End If

            cent *= 100
            strSql = " DECLARE @CENT FLOAT"
            strSql += " SET @CENT = " & cent & ""
            strSql += " SELECT MINRATE FROM " & cnAdminDb & "..CENTRATE WHERE"
            strSql += " @CENT BETWEEN FROMCENT AND TOCENT "
            strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "')"
            strSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStSubItem.Text, CmbStSubItem.Text) & "' "
            strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "')),0)"
            If CENTRATE_DESIGNER Then strSql += " AND ISNULL(DESIGNERID,0) =ISNULL((SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME='" & cmbDesigner_MAN.Text & "'),0)"
            If cmbCostCentre_Man.Text.Trim <> "" Then strSql += " AND COSTID IN(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre_Man.Text & "')"
            If _FourCMaintain Then
                Dim ColorId As Integer = 0
                Dim CutId As Integer = 0
                Dim ClarityId As Integer = 0
                ColorId = objGPack.GetSqlValue("SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME = '" & CmbStColor.Text & "'", "COLORID", 0)
                CutId = objGPack.GetSqlValue("SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & IIf(ORDER_MULTI_MIMR, ObjDiamondDetails.CmbCut.Text, ObjDiaDetails.CmbCut.Text) & "'", "CUTID", 0)
                ClarityId = objGPack.GetSqlValue("SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & CmbStClarity.Text & "'", "CLARITYID", 0)
                If CmbStShape.Text <> "" Then
                    Dim _Shapeid As Integer = 0
                    _Shapeid = objGPack.GetSqlValue("SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & CmbStShape.Text & "'", "SHAPEID", 0)
                    strSql += vbCrLf + " AND ISNULL(SHAPEID,0)=" & _Shapeid
                End If
                strSql += vbCrLf + " AND ISNULL(COLORID,0)=" & ColorId
                strSql += vbCrLf + " AND ISNULL(CUTID,0)=" & CutId
                strSql += vbCrLf + " AND ISNULL(CLARITYID,0)=" & ClarityId
            End If
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

            If objGPack.DupCheck("SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "' AND  STUDDED IN('S','B') AND ACTIVE = 'Y'") = False Then
                MsgBox("Invalid Item", MsgBoxStyle.Information)
                txtStItem.Focus()
                Exit Sub
            End If
            If UCase(objGPack.GetSqlValue("SELECT SUBITEM FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "' AND STUDDED IN('S','B') AND ACTIVE = 'Y'")) = "Y" Then
                If IIf(ACC_STUDITEM_POPUP, txtStSubItem.Text, CmbStSubItem.Text) = "" And IIf(ACC_STUDITEM_POPUP, txtStSubItem.Enabled, CmbStSubItem.Enabled) Then
                    MsgBox("SubItem Should Not Empty", MsgBoxStyle.Information)
                    If ACC_STUDITEM_POPUP Then txtStSubItem.Focus() Else CmbStSubItem.Focus()
                    Exit Sub
                End If
                If objGPack.DupCheck("SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "') AND SUBITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStSubItem.Text, CmbStSubItem.Text) & "'") = False Then
                    MsgBox("Invalid SubItem", MsgBoxStyle.Information)
                    If ACC_STUDITEM_POPUP Then txtStSubItem.Focus() Else CmbStSubItem.Focus()
                    Exit Sub
                End If
            Else
                If ACC_STUDITEM_POPUP Then txtStSubItem.Clear()
            End If
            If Val(txtStPcs_Num.Text) = 0 And Val(txtStWeight.Text) = 0 And Val(txtStAmount_Amt.Text) = 0 Then
                MsgBox(Me.GetNextControl(txtStPcs_Num, False).Text + "," + Me.GetNextControl(txtStWeight, False).Text + E0001, MsgBoxStyle.Information)
                If ACC_STUDITEM_POPUP Then txtStItem.Focus() Else cmbStItem.Focus()
                Exit Sub
            End If
            Dim cent As Double = Nothing
            strSql = " SELECT ISNULL(CALTYPE,'') CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') ='" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "'"
            Dim mCaltype As String = objGPack.GetSqlValue(strSql, "CALTYPE", "")
            strSql = " SELECT ISNULL(CALTYPE,'') CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST "
            strSql += " WHERE ISNULL(SUBITEMNAME,'') ='" & IIf(ACC_STUDITEM_POPUP, txtStSubItem.Text, CmbStSubItem.Text) & "' "
            strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "')"
            mCaltype = objGPack.GetSqlValue(strSql, "CALTYPE", "")

            If mCaltype = "D" Then
                cent = Val(txtStWeight.Text)
            Else
                cent = (Val(txtStWeight.Text) / IIf(Val(txtStPcs_Num.Text) = 0, 1, Val(txtStPcs_Num.Text)))
                'If cmbStUnit.Text = "C" Then cent = (Val(txtStWeight.Text) / IIf(Val(txtStPcs_Num.Text) = 0, 1, Val(txtStPcs_Num.Text))) Else cent = Val(txtStWeight.Text) / IIf(Val(txtStPcs_Num.Text) = 0, 1, Val(txtStPcs_Num.Text))
            End If
            cent *= 100
            strSql = " DECLARE @CENT FLOAT"
            strSql += " SET @CENT = " & cent & ""
            strSql += " SELECT MINRATE FROM " & cnAdminDb & "..CENTRATE WHERE"
            strSql += " @CENT BETWEEN FROMCENT AND TOCENT "
            strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "')"
            strSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST "
            strSql += " WHERE SUBITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStSubItem.Text, CmbStSubItem.Text) & "' "
            strSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "')),0)"
            If CENTRATE_DESIGNER Then strSql += " AND ISNULL(DESIGNERID,0) =ISNULL((SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME='" & cmbDesigner_MAN.Text & "'),0)"
            If cmbCostCentre_Man.Text.Trim <> "" Then strSql += " AND COSTID IN(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre_Man.Text & "')"
            If _FourCMaintain Then
                Dim ColorId As Integer = 0
                Dim CutId As Integer = 0
                Dim ClarityId As Integer = 0

                ColorId = objGPack.GetSqlValue("SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME = '" & CmbStColor.Text & "'", "COLORID", 0)
                CutId = objGPack.GetSqlValue("SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & IIf(ORDER_MULTI_MIMR, ObjDiamondDetails.CmbCut.Text, ObjDiaDetails.CmbCut.Text) & "'", "CUTID", 0)
                ClarityId = objGPack.GetSqlValue("SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & CmbStClarity.Text & "'", "CLARITYID", 0)
                If CmbStShape.Text <> "" Then
                    Dim _Shapeid As Integer = 0
                    _Shapeid = objGPack.GetSqlValue("SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & CmbStShape.Text & "'", "SHAPEID", 0)
                    strSql += vbCrLf + " AND ISNULL(SHAPEID,0)=" & _Shapeid
                End If
                strSql += vbCrLf + " AND ISNULL(COLORID,0)=" & ColorId
                strSql += vbCrLf + " AND ISNULL(CUTID,0)=" & CutId
                strSql += vbCrLf + " AND ISNULL(CLARITYID,0)=" & ClarityId
            End If
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
                    If ACC_STUDITEM_POPUP Then
                        .Cells("ITEM").Value = txtStItem.Text
                        .Cells("SUBITEM").Value = txtStSubItem.Text
                    Else
                        .Cells("ITEM").Value = cmbStItem.Text
                        .Cells("SUBITEM").Value = CmbStSubItem.Text
                    End If
                    .Cells("COLOR").Value = CmbStColor.Text
                    .Cells("CLARITY").Value = CmbStClarity.Text
                    .Cells("SHAPE").Value = CmbStShape.Text
                    .Cells("STNSIZE").Value = CmbStSize.Text
                    .Cells("UNIT").Value = cmbStUnit.Text
                    .Cells("CALC").Value = cmbStCalc.Text
                    If STUD_STNWTPER Then .Cells("WPER").Value = IIf(Val(txtStWPer_AMT.Text) <> 0, Val(txtStWPer_AMT.Text), DBNull.Value)
                    .Cells("PCS").Value = IIf(Val(txtStPcs_Num.Text) <> 0, Val(txtStPcs_Num.Text), DBNull.Value)
                    .Cells("WEIGHT").Value = IIf(Val(txtStWeight.Text) <> 0, Format(Val(txtStWeight.Text), FormatNumberStyle(DiaRnd)), DBNull.Value)
                    .Cells("RATE").Value = IIf(Val(txtStRate_Amt.Text) <> 0, Format(Val(txtStRate_Amt.Text), "0.00"), DBNull.Value)
                    .Cells("AMOUNT").Value = IIf(Val(txtStAmount_Amt.Text) <> 0, Format(Val(txtStAmount_Amt.Text), "0.00"), DBNull.Value)
                    .Cells("METALID").Value = txtStMetalCode.Text
                    .Cells("USRATE").Value = IIf(NEEDUS = True And Studded_Loose <> "L", Val(ObjRsUs.txtUSDollar_Amt.Text), 0)
                    .Cells("INDRS").Value = IIf(NEEDUS = True And Studded_Loose <> "L", Val(ObjRsUs.txtIndRs_Amt.Text), 0)
                    If _FourCMaintain Then
                        If ORDER_MULTI_MIMR Then
                            .Cells("STNGRPID").Value = Val(objGPack.GetSqlValue("SELECT GROUPID FROM " & cnAdminDb & " ..STONEGROUP WHERE GROUPNAME = '" & ObjDiamondDetails.cmbStnGrp.Text & "' ", "GROUPID", , tran))
                            .Cells("CUT").Value = ObjDiamondDetails.CmbCut.Text
                            .Cells("SETTYPE").Value = ObjDiamondDetails.cmbSetType.Text
                            .Cells("HEIGHT").Value = Val(ObjDiamondDetails.txtHeight_WET.Text)
                            .Cells("WIDTH").Value = Val(ObjDiamondDetails.txtWidth_WET.Text)
                        Else
                            .Cells("CUT").Value = ObjDiaDetails.CmbCut.Text
                            .Cells("SETTYPE").Value = ObjDiaDetails.cmbSetType.Text
                            .Cells("HEIGHT").Value = Val(ObjDiaDetails.txtHeight_WET.Text)
                            .Cells("WIDTH").Value = Val(ObjDiaDetails.txtWidth_WET.Text)
                        End If
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
            If ACC_STUDITEM_POPUP Then
                ro("ITEM") = txtStItem.Text
                ro("SUBITEM") = txtStSubItem.Text
            Else
                ro("ITEM") = cmbStItem.Text
                ro("SUBITEM") = CmbStSubItem.Text
            End If
            ro("COLOR") = CmbStColor.Text
            ro("CLARITY") = CmbStClarity.Text
            ro("SHAPE") = CmbStShape.Text
            ro("STNSIZE") = CmbStSize.Text
            If STUD_STNWTPER Then ro("WPER") = IIf(Val(txtStWPer_AMT.Text) <> 0, Val(txtStWPer_AMT.Text), DBNull.Value)
            ro("PCS") = IIf(Val(txtStPcs_Num.Text) <> 0, Val(txtStPcs_Num.Text), DBNull.Value)
            ro("UNIT") = cmbStUnit.Text
            ro("CALC") = cmbStCalc.Text
            ro("WEIGHT") = IIf(Val(txtStWeight.Text) <> 0, Format(Val(txtStWeight.Text), FormatNumberStyle(DiaRnd)), DBNull.Value)
            ro("RATE") = IIf(Val(txtStRate_Amt.Text) <> 0, Format(Val(txtStRate_Amt.Text), "0.00"), DBNull.Value)
            ro("AMOUNT") = IIf(Val(txtStAmount_Amt.Text) <> 0, Format(Val(txtStAmount_Amt.Text), "0.00"), DBNull.Value)
            If PURSTNRATE_AMOUNT = True Then
                ro("PURRATE") = IIf(Val(txtStRate_Amt.Text) <> 0, Format(Val(txtStRate_Amt.Text), "0.00"), DBNull.Value)
                ro("PURVALUE") = IIf(Val(txtStAmount_Amt.Text) <> 0, Format(Val(txtStAmount_Amt.Text), "0.00"), DBNull.Value)
            End If
            ro("METALID") = txtStMetalCode.Text
            ro("USRATE") = IIf(NEEDUS = True And Studded_Loose <> "L", Val(ObjRsUs.txtUSDollar_Amt.Text), 0)
            ro("INDRS") = IIf(NEEDUS = True And Studded_Loose <> "L", Val(ObjRsUs.txtIndRs_Amt.Text), 0)
            If _FourCMaintain Then
                If ORDER_MULTI_MIMR Then

                    ro("STNGRPID") = Val(objGPack.GetSqlValue("SELECT GROUPID FROM " & cnAdminDb & " ..STONEGROUP WHERE GROUPNAME = '" & ObjDiamondDetails.cmbStnGrp.Text & "' ", "GROUPID", , tran))
                    ro("CUT") = ObjDiamondDetails.CmbCut.Text
                    ro("SETTYPE") = ObjDiamondDetails.cmbSetType.Text
                    ro("HEIGHT") = Val(ObjDiamondDetails.txtHeight_WET.Text)
                    ro("WIDTH") = Val(ObjDiamondDetails.txtWidth_WET.Text)
                Else
                    ro("CUT") = ObjDiaDetails.CmbCut.Text
                    ro("SETTYPE") = ObjDiaDetails.cmbSetType.Text
                    ro("HEIGHT") = Val(ObjDiaDetails.txtHeight_WET.Text)
                    ro("WIDTH") = Val(ObjDiaDetails.txtWidth_WET.Text)
                End If

            End If
            dtStoneDetails.Rows.Add(ro)
            dtStoneDetails.AcceptChanges()
            gridStone.CurrentCell = gridStone.Rows(gridStone.RowCount - 1).Cells(1)
AFTERINSERT:
            CalcLessWt()
            CalcFinalTotal()

            ''CLEAR
            'cmbStItem_Man.Text = ""
            'cmbStSubItem_Man.Text = ""
            txtStSubItem.Clear()
            txtStPcs_Num.Clear()
            txtStWPer_AMT.Clear()
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
                If ACC_STUDITEM_POPUP Then txtStItem.Focus() Else cmbStItem.Focus()
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
        If CmbStSize.Text = "" Then
            If Not TAG_STONECALTYPE Then cmbStCalc.Text = IIf(Val(txtStWeight.Text) > 0, "W", "P")
        End If
        txtStWeight.Text = IIf(Val(txtStWeight.Text) <> 0, Format(Val(txtStWeight.Text), FormatNumberStyle(DiaRnd)), txtStWeight.Text)
    End Sub
    Private Sub txtStWeight_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStWeight.TextChanged, txtStWeight.Leave
        Dim cent As Double = 0

        Dim tempitemid As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') = '" & cmbItem_MAN.Text & "'").ToString)
        Dim tempSitemid As Integer = 0
        If cmbSubItem_Man.Text <> "" Then
            tempSitemid = objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE ISNULL(SUBITEMNAME,'') = '" & cmbSubItem_Man.Text & "'")
        End If
        Dim dtdesgstnrow As DataRow
        Dim tempDesignerid As Integer
        If cmbDesigner_MAN.Text <> "" Then tempDesignerid = objGPack.GetSqlValue("SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE ISNULL(DESIGNERNAME,'') = '" & cmbDesigner_MAN.Text & "'")
        dtdesgstnrow = GetSqlRow("SELECT STNDEDPER,STN_RATE,STNITEMID,STNSUBITEMID,CALTYPE,STONEUNIT FROM " & cnAdminDb & "..DESIGNERSTONE WHERE ACTIVE <> 'N' AND DESIGNERID = " & tempDesignerid & " AND ITEMID = " & tempitemid & " AND SUBITEMID = " & tempSitemid, cn)
        If dtdesgstnrow Is Nothing Then
            dtdesgstnrow = GetSqlRow("SELECT STNDEDPER,STN_RATE,STNITEMID,STNSUBITEMID,CALTYPE,STONEUNIT FROM " & cnAdminDb & "..DESIGNERSTONE WHERE ACTIVE <> 'N' AND DESIGNERID = 0 AND ITEMID = " & tempitemid & " AND SUBITEMID = " & tempSitemid, cn)
        Else
            cmbStUnit.Text = dtdesgstnrow(5).ToString
            cmbStCalc.Text = dtdesgstnrow(4).ToString
        End If
        Dim tempcalmode As String = ""
        Dim tempwtunit As String = ""
        If Not dtdesgstnrow Is Nothing Then
            tempcalmode = dtdesgstnrow(4).ToString
            tempwtunit = dtdesgstnrow(5).ToString
        End If

        strSql = " SELECT ISNULL(CALTYPE,'') CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') ='" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "'"
        Dim mCaltype As String = objGPack.GetSqlValue(strSql, "CALTYPE", "")
        strSql = " SELECT ISNULL(CALTYPE,'') CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE ISNULL(SUBITEMNAME,'') ='" & IIf(ACC_STUDITEM_POPUP, txtStSubItem.Text, CmbStSubItem.Text) & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "')"
        mCaltype = objGPack.GetSqlValue(strSql, "CALTYPE", "")
        If tempcalmode.ToString <> "" Then mCaltype = tempcalmode
        If mCaltype = "D" Then
            cent = Val(txtStWeight.Text)
        Else
            cent = (Val(txtStWeight.Text) / IIf(Val(txtStPcs_Num.Text) = 0, 1, Val(txtStPcs_Num.Text)))
        End If
        'If cmbStUnit.Text = "C" Then
        '    cent = (Val(txtStWeight.Text) / IIf(Val(txtStPcs_Num.Text) = 0, 1, Val(txtStPcs_Num.Text)))
        'Else
        '    cent = Val(txtStWeight.Text) / IIf(Val(txtStPcs_Num.Text) = 0, 1, Val(txtStPcs_Num.Text))
        'End If
        cent *= 100
        strSql = " DECLARE @CENT FLOAT"
        strSql += vbCrLf + " SET @CENT = " & cent & ""
        strSql += vbCrLf + " SELECT MAXRATE FROM " & cnAdminDb & "..CENTRATE WHERE"
        strSql += vbCrLf + " @CENT BETWEEN FROMCENT AND TOCENT "
        strSql += vbCrLf + " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "')"
        strSql += vbCrLf + " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST "
        strSql += vbCrLf + " WHERE SUBITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStSubItem.Text, CmbStSubItem.Text) & "' "
        strSql += vbCrLf + " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "')),0)"
        If CENTRATE_DESIGNER Then strSql += " AND ISNULL(DESIGNERID,0) =ISNULL((SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME='" & cmbDesigner_MAN.Text & "'),0)"
        'strSql += vbCrLf + " AND ISNULL(ACCODE,'') = ISNULL((SELECT ACCODE FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesigner_MAN.Text & "'),'')"
        If cmbCostCentre_Man.Text.Trim <> "" Then strSql += " AND COSTID IN(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostCentre_Man.Text & "')"
        If _FourCMaintain Then

            Dim ColorId As Integer = 0
            Dim CutId As Integer = 0
            Dim ClarityId As Integer = 0

            ColorId = objGPack.GetSqlValue("SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME = '" & CmbStColor.Text & "'", "COLORID", 0)
            CutId = objGPack.GetSqlValue("SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & IIf(ORDER_MULTI_MIMR, ObjDiamondDetails.CmbCut.Text, ObjDiaDetails.CmbCut.Text) & "'", "CUTID", 0)
            ClarityId = objGPack.GetSqlValue("SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & CmbStClarity.Text & "'", "CLARITYID", 0)
            If CmbStShape.Text <> "" Then
                Dim _Shapeid As Integer = 0
                _Shapeid = objGPack.GetSqlValue("SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & CmbStShape.Text & "'", "SHAPEID", 0)
                strSql += vbCrLf + " AND ISNULL(SHAPEID,0)=" & _Shapeid
            End If
            strSql += vbCrLf + " AND ISNULL(COLORID,0)=" & ColorId
            strSql += vbCrLf + " AND ISNULL(CUTID,0)=" & CutId
            strSql += vbCrLf + " AND ISNULL(CLARITYID,0)=" & ClarityId
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
        If Not dtdesgstnrow Is Nothing And Val(txtStRate_Amt.Text.ToString) = 0 And tagEdit = True Then
            txtStRate_Amt.Text = dtdesgstnrow(1).ToString
        End If
        Dim PieRate As String = objGPack.GetSqlValue(Replace(strSql, "MAXRATE", "ISNULL(PIERATE,'N') PIERATE"), "PIERATE", "N", tran).ToString
        If PieRate = "Y" Then cmbStCalc.Text = "P"
        CalcStoneAmount()
    End Sub

    Private Sub cmbStCalc_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStCalc.SelectedIndexChanged
        CalcStoneAmount()
    End Sub

    Private Sub txtStPcs_Num_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStPcs_Num.GotFocus
        Dim tempitemid As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') = '" & cmbItem_MAN.Text & "'").ToString)
        Dim tempSitemid As Integer = 0
        If cmbSubItem_Man.Text <> "" Then
            tempSitemid = objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE ISNULL(SUBITEMNAME,'') = '" & cmbSubItem_Man.Text & "'")
        End If
        Dim dtdesgstnrow As DataRow
        Dim tempDesignerid As Integer
        If cmbDesigner_MAN.Text <> "" Then tempDesignerid = objGPack.GetSqlValue("SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE ISNULL(DESIGNERNAME,'') = '" & cmbDesigner_MAN.Text & "'")
        dtdesgstnrow = GetSqlRow("SELECT STNDEDPER,STN_RATE,STNITEMID,STNSUBITEMID,CALTYPE,STONEUNIT FROM " & cnAdminDb & "..DESIGNERSTONE WHERE ACTIVE <> 'N' AND DESIGNERID = " & tempDesignerid & " AND ITEMID = " & tempitemid & " AND SUBITEMID = " & tempSitemid, cn)
        If dtdesgstnrow Is Nothing Then
            dtdesgstnrow = GetSqlRow("SELECT STNDEDPER,STN_RATE,STNITEMID,STNSUBITEMID,CALTYPE,STONEUNIT FROM " & cnAdminDb & "..DESIGNERSTONE WHERE ACTIVE <> 'N' AND DESIGNERID = 0 AND ITEMID = " & tempitemid & " AND SUBITEMID = " & tempSitemid, cn)
        Else
            cmbStUnit.Text = dtdesgstnrow(5).ToString
            cmbStCalc.Text = dtdesgstnrow(4).ToString
        End If
    End Sub

    Private Sub txtStPcs_Num_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtStPcs_Num.KeyDown
        If e.KeyCode = Keys.Down Then
            gridStone.Select()
        End If
    End Sub

    Private Sub txtStPcs_Num_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStPcs_Num.Leave
        If STUDDIA_MAND Then
            strSql = "SELECT ISNULL(METALID,'')METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "' AND METALID='D'"
            If objGPack.GetSqlValue(strSql).ToString.ToUpper = "D" Then
                If Val(txtStPcs_Num.Text.ToString) = 0 Then
                    MsgBox("Diamond Pcs should not be empty.", MsgBoxStyle.Information)
                    txtStPcs_Num.Focus() : txtStPcs_Num.Select() : txtStPcs_Num.SelectAll()
                    Exit Sub
                End If
            End If
        End If
        Dim view4c As String = ""
        Dim maintain4c As Boolean
        strSql = "SELECT ISNULL(MAINTAIN4C,'N') FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "' AND METALID='D'"
        If objGPack.GetSqlValue(strSql).ToString.ToUpper = "N" Then maintain4c = False Else maintain4c = True
        strSql = "SELECT ISNULL(MAINTAIN4C,'N') FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStSubItem.Text, CmbStSubItem.Text) & "' AND ITEMID=(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "')"
        If objGPack.GetSqlValue(strSql).ToString.ToUpper = "N" Then maintain4c = False Else maintain4c = True
        Dim Show4C As Boolean = False
        If _FourCMaintain And Not tagEdit And maintain4c Then
            If ORDER_MULTI_MIMR Then
                If IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "'", , , )
                If IIf(ACC_STUDITEM_POPUP, txtStSubItem.Text, CmbStSubItem.Text) <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & IIf(ACC_STUDITEM_POPUP, txtStSubItem.Text, CmbStSubItem.Text) & "' AND ITEMID=(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "')", , , )
                If Not view4c.Contains("CU") Then ObjDiamondDetails.CmbCut.Enabled = False Else Show4C = True
                If Not view4c.Contains("SE") Then ObjDiamondDetails.cmbSetType.Enabled = False Else Show4C = True
                If Not view4c.Contains("HE") Then ObjDiamondDetails.txtHeight_WET.Enabled = False Else Show4C = True
                If Not view4c.Contains("WI") Then ObjDiamondDetails.txtWidth_WET.Enabled = False Else Show4C = True
            Else
                ObjDiaDetails = New frm4C
                If IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "'", , , )
                If IIf(ACC_STUDITEM_POPUP, txtStSubItem.Text, CmbStSubItem.Text) <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & IIf(ACC_STUDITEM_POPUP, txtStSubItem.Text, CmbStSubItem.Text) & "' AND ITEMID=(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "')", , , )
                If Not view4c.Contains("CU") Then ObjDiaDetails.CmbCut.Enabled = False Else Show4C = True
                If Not view4c.Contains("SE") Then ObjDiaDetails.cmbSetType.Enabled = False Else Show4C = True
                If Not view4c.Contains("HE") Then ObjDiaDetails.txtHeight_WET.Enabled = False Else Show4C = True
                If Not view4c.Contains("WI") Then ObjDiaDetails.txtWidth_WET.Enabled = False Else Show4C = True
                If Show4C Then
                    ObjDiaDetails.CmbCut.Focus()
                    ObjDiaDetails.ShowDialog()
                End If
            End If
        ElseIf _FourCMaintain And tagEdit And maintain4c Then
            If ORDER_MULTI_MIMR Then
                ObjDiamondDetails = New frmDiamondDetails
                ObjDiamondDetails.CmbCut.Text = Cut
                ObjDiamondDetails.cmbSetType.Text = SetType
                ObjDiamondDetails.txtWidth_WET.Text = Width
                ObjDiamondDetails.txtHeight_WET.Text = Height
                If IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "'", , , )
                If IIf(ACC_STUDITEM_POPUP, txtStSubItem.Text, CmbStSubItem.Text) <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & IIf(ACC_STUDITEM_POPUP, txtStSubItem.Text, CmbStSubItem.Text) & "' AND ITEMID=(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "')", , , )
                If Not view4c.Contains("CU") Then ObjDiamondDetails.CmbCut.Enabled = False Else Show4C = True
                If Not view4c.Contains("SE") Then ObjDiamondDetails.cmbSetType.Enabled = False Else Show4C = True
                If Not view4c.Contains("HE") Then ObjDiamondDetails.txtHeight_WET.Enabled = False Else Show4C = True
                If Not view4c.Contains("WI") Then ObjDiamondDetails.txtWidth_WET.Enabled = False Else Show4C = True
                If Show4C Then
                    ObjDiamondDetails.cmbStnGrp.Focus()
                    ObjDiamondDetails.ShowDialog()
                End If
            Else
                ObjDiaDetails = New frm4C
                ObjDiaDetails.CmbCut.Text = Cut
                ObjDiaDetails.cmbSetType.Text = SetType
                ObjDiaDetails.txtWidth_WET.Text = Width
                ObjDiaDetails.txtHeight_WET.Text = Height
                If IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "'", , , )
                If IIf(ACC_STUDITEM_POPUP, txtStSubItem.Text, CmbStSubItem.Text) <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & IIf(ACC_STUDITEM_POPUP, txtStSubItem.Text, CmbStSubItem.Text) & "' AND ITEMID=(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "')", , , )
                If Not view4c.Contains("CU") Then ObjDiaDetails.CmbCut.Enabled = False Else Show4C = True
                If Not view4c.Contains("SE") Then ObjDiaDetails.cmbSetType.Enabled = False Else Show4C = True
                If Not view4c.Contains("HE") Then ObjDiaDetails.txtHeight_WET.Enabled = False Else Show4C = True
                If Not view4c.Contains("WI") Then ObjDiaDetails.txtWidth_WET.Enabled = False Else Show4C = True
                If Show4C Then
                    ObjDiaDetails.CmbCut.Focus()
                    ObjDiaDetails.ShowDialog()
                End If
            End If
        End If
    End Sub

    Private Sub CmbStSize_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles CmbStSize.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtStPcs_Num_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStPcs_Num.TextChanged, CmbStSize.Leave
        If CmbStSize.Enabled = True And CmbStSize.Text <> "" Then
            strSql = "SELECT STNWT,STNUNIT,CALCTYPE FROM " & cnAdminDb & "..STNSIZE WHERE SIZENAME='" & CmbStSize.Text & "'"
            If ACC_STUDITEM_POPUP Then
                strSql += " AND STNITEMID=(SELECT TOP 1 ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & txtStItem.Text & "')"
                strSql += " AND STNSITEMID=(SELECT TOP 1 SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & txtStSubItem.Text & "')"
            Else
                strSql += " AND STNITEMID=(SELECT TOP 1 ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbStItem.Text & "')"
                strSql += " AND STNSITEMID=(SELECT TOP 1 SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & CmbStSubItem.Text & "')"
            End If
            Dim dr As DataRow = GetSqlRow(strSql, cn, tran)
            If Not dr Is Nothing Then
                cmbStCalc.Text = dr("CALCTYPE").ToString
                cmbStUnit.Text = dr("STNUNIT").ToString
                txtStWeight.Text = Val(txtStPcs_Num.Text) * Val(dr("STNWT").ToString)
            End If
        End If
        CalcStoneAmount()
    End Sub

    Private Sub gridStone_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridStone.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            gridStone.CurrentCell = gridStone.Rows(gridStone.CurrentRow.Index).Cells(1)
        ElseIf e.KeyCode = Keys.delete Then
            If MetalBasedStone And dtMultiMetalDetails.Rows.Count > 0 And dtStoneDetails.Rows.Count > 0 And txtMMRowIndex.Text <> "" Then
                If Val(gridStone.Rows(gridStone.CurrentRow.Index).Cells("MKEYNO").Value.ToString) <> Val(dtMultiMetalDetails.Rows(Val(txtMMRowIndex.Text)).Item("KEYNO").ToString) _
                    And Val(gridStone.Rows(gridStone.CurrentRow.Index).Cells("MKEYNO").Value.ToString) <> 0 _
                    Then
                    e.Handled = True
                    MsgBox("Cannot Edit another Item Stone detail", MsgBoxStyle.Information)
                    Exit Sub
                End If
            End If
        End If
    End Sub

    Private Sub gridStone_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridStone.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If MetalBasedStone And dtMultiMetalDetails.Rows.Count > 0 And dtStoneDetails.Rows.Count > 0 And txtMMRowIndex.Text <> "" Then
                If Val(gridStone.Rows(gridStone.CurrentRow.Index).Cells("MKEYNO").Value.ToString) <> Val(dtMultiMetalDetails.Rows(Val(txtMMRowIndex.Text)).Item("KEYNO").ToString) Then
                    MsgBox("Cannot Edit another Item Stone detail", MsgBoxStyle.Information)
                    Exit Sub
                End If
            End If
            With gridStone.Rows(gridStone.CurrentRow.Index)
                txtstPackettno.Text = .Cells("PACKETNO").FormattedValue
                If ACC_STUDITEM_POPUP Then
                    txtStItem.Text = .Cells("ITEM").FormattedValue
                    txtStSubItem.Text = .Cells("SUBITEM").FormattedValue
                Else
                    cmbStItem.Text = .Cells("ITEM").FormattedValue
                    CmbStSubItem.Text = .Cells("SUBITEM").FormattedValue
                End If
                TagStnGrpId = Val(.Cells("STNGRPID").Value.ToString)
                TagCutId = Val(objGPack.GetSqlValue("SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & .Cells("CUT").Value.ToString & "'", "CUTID",, tran))
                TagColorId = Val(objGPack.GetSqlValue("SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME = '" & .Cells("COLOR").Value.ToString & "'", "COLORID",, tran))
                TagClarityId = Val(objGPack.GetSqlValue("SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & .Cells("CLARITY").Value.ToString & "'", "CLARITYID",, tran))
                TagShapeId = Val(objGPack.GetSqlValue("SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & .Cells("SHAPE").Value.ToString & "'", "SHAPEID",, tran))
                TagSetTypeId = Val(objGPack.GetSqlValue("SELECT SETTYPEID FROM " & cnAdminDb & "..STNSETTYPE WHERE SETTYPENAME = '" & .Cells("SETTYPE").Value.ToString & "'", "SETTYPEID",, tran))
                CmbStColor.Text = .Cells("COLOR").FormattedValue
                CmbStClarity.Text = .Cells("CLARITY").FormattedValue
                CmbStShape.Text = .Cells("SHAPE").FormattedValue
                CmbStSize.Text = .Cells("STNSIZE").FormattedValue
                If STUD_STNWTPER Then txtStWPer_AMT.Text = .Cells("WPER").FormattedValue
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
                    SetType = IIf(IsDBNull(.Cells("SETTYPE").Value), "", .Cells("SETTYPE").Value)
                    Height = IIf(IsDBNull(.Cells("HEIGHT").Value), 0, .Cells("HEIGHT").Value)
                    Width = IIf(IsDBNull(.Cells("WIDTH").Value), 0, .Cells("WIDTH").Value)
                    If ORDER_MULTI_MIMR Then
                        ObjDiamondDetails.cmbStnGrp.Text = objGPack.GetSqlValue("SELECT GROUPNAME FROM " & cnAdminDb & "..STONEGROUP WHERE GROUPID = '" & Val(.Cells("STNGRPID").Value.ToString) & "'", "GROUPNAME",, tran)
                        ObjDiamondDetails.CmbCut.Text = Cut
                        ObjDiamondDetails.cmbSetType.Text = SetType
                        ObjDiamondDetails.txtWidth_WET.Text = Width
                        ObjDiamondDetails.txtHeight_WET.Text = Height
                    Else
                        ObjDiaDetails.CmbCut.Text = Cut
                        ObjDiaDetails.cmbSetType.Text = SetType
                        ObjDiaDetails.txtWidth_WET.Text = Width
                        ObjDiaDetails.txtHeight_WET.Text = Height
                    End If
                End If
                txtStMetalCode.Text = .Cells("METALID").FormattedValue
                txtStRowIndex.Text = gridStone.CurrentRow.Index
                If PacketNoEnable <> "N" Then
                    txtstPackettno.Focus()
                    txtstPackettno.SelectAll()
                Else
                    If ACC_STUDITEM_POPUP Then
                        If DesignerStone Then
                            txtStPcs_Num.Focus()
                            txtStPcs_Num.SelectAll()
                        Else
                            txtStItem.Focus()
                            txtStItem.SelectAll()
                        End If
                    Else
                        If DesignerStone Then
                            txtStPcs_Num.Focus()
                            txtStPcs_Num.SelectAll()
                        Else
                            cmbStItem.Focus()
                            cmbStItem.SelectAll()
                        End If
                    End If
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
            If ACC_STUDITEM_POPUP Then txtStItem.Focus() Else cmbStItem.Focus()
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
        Dim wastwt As Double = 0
        Dim gorn As String = ""
        If cmbCalcMode.SelectedIndex = 0 Then gorn = "G" : wt = Val(txtGrossWt_Wet.Text) Else gorn = "N" : wt = Val(txtNetWt_Wet.Text)

        If Not OrderRow Is Nothing Then
            If Mid(OrderRow(1).ToString, 6, 1) = "R" And RepWastMcExOnly And OrderDetail <> "" Then
                Dim ordetailarr() As String = OrderDetail.Split(",")
                wt = wt - IIf(gorn = "G", Val(ordetailarr(0).ToString), Val(ordetailarr(1).ToString))
            End If
        End If

        wastwt = wt * (Val(ObjMinValue.txtMinWastage_Per.Text) / 100)
        wastwt = Math.Round(wastwt, WastageRound)
        ObjMinValue.txtMinWastage_Wet.Text = IIf(wastwt <> 0, Format(wastwt, "0.000"), "")
    End Sub


    Private Sub ObjMinValues_txtMinMcPerGram_Amt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Not Val(txtMaxMcPerGrm_Amt.Text) >= Val(ObjMinValue.txtMinMcPerGram_Amt.Text + e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub ObjMinValues_txtMinMcPerGram_Amt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim mc As Double = Nothing
        Dim wast As Double = IIf(McWithWastage, Val(ObjMinValue.txtMinWastage_Wet.Text), 0)
        Dim wt As Decimal = 0
        Dim gorn As String
        If cmbCalcMode.SelectedIndex = 0 Then wt = Val(txtGrossWt_Wet.Text) Else wt = Val(txtNetWt_Wet.Text)
        If Not OrderRow Is Nothing Then
            If Mid(OrderRow(1).ToString, 6, 1) = "R" And RepWastMcExOnly And OrderDetail <> "" Then
                Dim ordetailarr() As String = OrderDetail.Split(",")
                wt = wt - IIf(gorn = "G", Val(ordetailarr(0).ToString), Val(ordetailarr(1).ToString))
            End If
        End If
        mc = (wt + wast) * Val(ObjMinValue.txtMinMcPerGram_Amt.Text)
        ObjMinValue.txtMinMkCharge_Amt.Text = IIf(mc <> 0, Format(mc, "0.00"), "")
    End Sub

    Private Sub txtLotNo_Num_Man_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLotNo_Num_Man.LostFocus
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
        'strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "') ORDER BY SUBITEMNAME"
        'Dim iId As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"))
        'strSql = GetSubItemQry(New String() {"SUBITEMNAME SUBITEM"}, iId)
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
        If Val(ObjMinValue.txtMinWastage_Wet.Text + e.KeyChar) <> 0 And (Not Val(txtMaxWastage_Wet.Text) >= Val(ObjMinValue.txtMinWastage_Wet.Text + e.KeyChar)) Then
            e.Handled = True
            ' Else
            MsgBox("Check Maximum value")
        End If
    End Sub

    Private Sub ObjMinValue_txtMinMkCharge_Amt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        If Val(ObjMinValue.txtMinMcPerGram_Amt.Text) > 0 Then ObjMinValue.txtMinMcPerGram_Amt.ReadOnly = True Else ObjMinValue.txtMinMcPerGram_Amt.ReadOnly = False
    End Sub

    Private Sub ObjMinValue_txtMinMkCharge_Amt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Val(ObjMinValue.txtMinMkCharge_Amt.Text + e.KeyChar) <> 0 And (Not Val(txtMaxMkCharge_Amt.Text) >= Val(ObjMinValue.txtMinMkCharge_Amt.Text + e.KeyChar)) Then
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
        strSql += " WHERE ISNULL(STUDDED,'') IN ('S','B') AND ACTIVE = 'Y'"
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
        If ORDER_MULTI_MIMR Then
            ObjDiamondDetails = New frmDiamondDetails
            ObjDiamondDetails.StartPosition = FormStartPosition.CenterScreen
            ObjDiamondDetails.cmbStnGrp.Text = objGPack.GetSqlValue(" SELECT GROUPNAME FROM " & cnAdminDb & "..STONEGROUP WHERE GROUPID = '" & TagStnGrpId & "'", "GROUPNAME", , tran)
            ObjDiamondDetails.CmbCut.Text = objGPack.GetSqlValue(" SELECT CUTNAME FROM " & cnAdminDb & "..STNCUT WHERE CUTID = '" & TagCutId & "'", "CUTNAME", , tran)
            ObjDiamondDetails.CmbColor.Text = objGPack.GetSqlValue(" SELECT COLORNAME FROM " & cnAdminDb & "..STNCOLOR WHERE COLORID = '" & TagColorId & "'", "COLORNAME", , tran)
            ObjDiamondDetails.CmbClarity.Text = objGPack.GetSqlValue(" SELECT CLARITYNAME FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYID = '" & TagClarityId & "'", "CLARITYNAME", , tran)
            ObjDiamondDetails.cmbShape.Text = objGPack.GetSqlValue(" SELECT SHAPENAME FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPEID = '" & TagShapeId & "'", "SHAPENAME", , tran)
            ObjDiamondDetails.cmbSetType.Text = objGPack.GetSqlValue(" SELECT SETTYPENAME FROM " & cnAdminDb & "..STNSETTYPE WHERE SETTYPEID = '" & TagSetTypeId & "'", "SETTYPENAME", , tran)
            ObjDiamondDetails.txtWidth_WET.Text = ""
            ObjDiamondDetails.txtHeight_WET.Text = "" 'Val(dtTagDetails.Rows(0).Item("HEIGHT").ToString)
            ObjDiamondDetails.ShowDialog()
            CmbStColor.Text = ObjDiamondDetails.CmbColor.Text : CmbStColor.Enabled = False
            CmbStClarity.Text = ObjDiamondDetails.CmbClarity.Text : CmbStClarity.Enabled = False
            CmbStShape.Text = ObjDiamondDetails.cmbShape.Text : CmbStShape.Enabled = False
            TagStnGrpId = Val(objGPack.GetSqlValue("SELECT GROUPID FROM " & cnAdminDb & "..STONEGROUP WHERE GROUPNAME = '" & ObjDiamondDetails.cmbStnGrp.Text & "'", "GROUPID",, tran))
            TagCutId = Val(objGPack.GetSqlValue("SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & ObjDiamondDetails.CmbCut.Text & "'", "CUTID",, tran))
            TagColorId = Val(objGPack.GetSqlValue("SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME = '" & ObjDiamondDetails.CmbColor.Text & "'", "COLORID",, tran))
            TagClarityId = Val(objGPack.GetSqlValue("SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & ObjDiamondDetails.CmbClarity.Text & "'", "CLARITYID",, tran))
            TagShapeId = Val(objGPack.GetSqlValue("SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & ObjDiamondDetails.cmbShape.Text & "'", "SHAPEID",, tran))
            TagSetTypeId = Val(objGPack.GetSqlValue("SELECT SETTYPEID FROM " & cnAdminDb & "..STNSETTYPE WHERE SETTYPENAME = '" & ObjDiamondDetails.cmbSetType.Text & "'", "SETTYPEID",, tran))
        Else
            CmbStColor.Enabled = True
            CmbStClarity.Enabled = True
            CmbStShape.Enabled = True
        End If
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
                    If MetalBasedStone Then
                        .Item("NETWT") = IIf(Val(txtMMNetwt_Wet.Text) <> 0, Val(txtMMNetwt_Wet.Text), DBNull.Value)
                    End If
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
            If MetalBasedStone Then
                ro("NETWT") = IIf(Val(txtMMNetwt_Wet.Text) <> 0, Val(txtMMNetwt_Wet.Text), DBNull.Value)
            End If
            ro("WASTAGEPER") = IIf(Val(txtMMWastagePer_PER.Text) <> 0, Val(txtMMWastagePer_PER.Text), DBNull.Value)
            ro("WASTAGE") = IIf(Val(txtMMWastage_WET.Text) <> 0, Val(txtMMWastage_WET.Text), DBNull.Value)
            ro("MCPERGRM") = IIf(Val(txtMMMcPerGRm_AMT.Text) <> 0, Val(txtMMMcPerGRm_AMT.Text), DBNull.Value)
            ro("MC") = IIf(Val(txtMMMc_AMT.Text) <> 0, Val(txtMMMc_AMT.Text), DBNull.Value)
            ro("AMOUNT") = IIf(Val(txtMMAmount_AMT.Text) <> 0, Val(txtMMAmount_AMT.Text), DBNull.Value)
            ro("RATE") = IIf(Val(txtMMRate.Text) <> 0, Val(txtMMRate.Text), DBNull.Value)
            dtMultiMetalDetails.Rows.Add(ro)
AFTERINSERT:
            CalcFinalTotal()

            If MetalBasedStone And dtStoneDetails.Rows.Count > 0 And dtMultiMetalDetails.Rows.Count > 0 Then
                For Each rss As DataRow In dtStoneDetails.Rows
                    If rss("MKEYNO").ToString <> "" Then Continue For
                    If txtMMRowIndex.Text <> "" Then
                        rss("MKEYNO") = dtMultiMetalDetails.Rows(txtMMRowIndex.Text).Item("KEYNO").ToString
                    Else
                        rss("MKEYNO") = dtMultiMetalDetails.Rows(dtMultiMetalDetails.Rows.Count - 1).Item("KEYNO").ToString
                    End If

                Next
                dtStoneDetails.AcceptChanges()
            End If

            txtMMWeight_Wet.Clear()
            If MetalBasedStone Then
                txtMMNetwt_Wet.Clear()
            End If
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
        Dim wt As Double = 0
        If MetalBasedStone Then
            If Val(txtMMWeight_Wet.Text) <> Val(txtMMNetwt_Wet.Text) Then
                wt = Val(txtMMNetwt_Wet.Text) * (Val(txtMMWastagePer_PER.Text) / 100)
            Else
                wt = Val(txtMMWeight_Wet.Text) * (Val(txtMMWastagePer_PER.Text) / 100)
            End If
        Else
            wt = Val(txtMMWeight_Wet.Text) * (Val(txtMMWastagePer_PER.Text) / 100)
        End If
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
        Dim mc As Double = 0
        If MetalBasedStone Then
            If Val(txtMMWeight_Wet.Text) <> Val(txtMMNetwt_Wet.Text) Then
                mc = Val(txtMMNetwt_Wet.Text) * Val(txtMMMcPerGRm_AMT.Text)
            Else
                mc = Val(txtMMWeight_Wet.Text) * Val(txtMMMcPerGRm_AMT.Text)
            End If
        Else
            mc = Val(txtMMWeight_Wet.Text) * Val(txtMMMcPerGRm_AMT.Text)
        End If
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
        ElseIf e.KeyCode = Keys.Enter Then
            If MetalBasedStone Then
                If TabControl1.TabPages.Contains(tabStone) And Val(txtMMWeight_Wet.Text) > 0 Then
                    TabControl1.SelectTab(tabStone)
                    If ACC_STUDITEM_POPUP Then txtStItem.Focus() : txtStItem.Select() Else cmbStItem.Select()
                End If
            End If
        End If
    End Sub

    ''Private Sub txtMMWeight_Wet_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtMMWeight_Wet.KeyPress
    ''    If e.KeyChar = Chr(Keys.Enter) Then
    ''        If MetalBasedStone Then
    ''            If TabControl1.TabPages.Contains(tabStone) Then
    ''                TabControl1.SelectTab(tabStone)
    ''                If ACC_STUDITEM_POPUP Then txtStItem.Focus() : txtStItem.Select() Else cmbStItem.Select()
    ''            End If
    ''        End If
    ''    End If
    ''End Sub

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

    Private Sub cmbCostCentre_Man_Leave(sender As Object, e As EventArgs) Handles cmbCostCentre_Man.Leave
        If costLock = False Then
            Dim dtt As DataTable = Nothing
            strSql = funcAssignTagDefaultVal()
            da = New OleDbDataAdapter(strSql, cn)
            dtt = New DataTable
            da.Fill(dtt)
            Dim Assorted As Boolean = False
            With dtt.Rows(0)
                If objGPack.GetSqlValue("SELECT ISNULL(ASSORTED,'') FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & .Item("ItemName").ToString & "'") = "Y" Then
                    Assorted = True
                End If
                If .Item("ValueAddedType") = "T" Or Assorted Then
                    cmbTableCode.Enabled = True
                    strSql = " SELECT DISTINCT TABLECODE FROM " & cnAdminDb & "..WMCTABLE WHERE TABLECODE <> '' "
                    strSql += " "
                    strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_Man.Text & "'),'')"
                    strSql += " ORDER BY TABLECODE"
                    objGPack.FillCombo(strSql, cmbTableCode)

                    strSql = " select TableCode from " & cnAdminDb & "..itemMast where ItemName = '" & cmbItem_MAN.Text & "' and TableCode <> ''"
                    Dim dtTableCode As New DataTable
                    dtTableCode.Clear()
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(dtTableCode)
                    If dtTableCode.Rows.Count > 0 Then
                        cmbTableCode.Text = dtTableCode.Rows(0).Item("TableCode").ToString
                    End If

                Else
                    cmbTableCode.Items.Clear()
                    cmbTableCode.Enabled = False
                End If
            End With

        End If
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
            strSql = funcAssignTagDefaultVal()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            funcAssignVal(dt)
            If cmbSubItem_Man.Enabled Then cmbSubItem_SelectedIndexChanged(Me, New EventArgs)
            funcAssignTabControls()
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
            Dim dt As New DataTable
            strSql = funcAssignTagDefaultVal()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            funcAssignVal(dt)
            If cmbSubItem_Man.Enabled Then cmbSubItem_SelectedIndexChanged(Me, New EventArgs)
            funcAssignTabControls()
            'txtLotNo_Num_Man.Enabled = False            
            ItemLock = False
            Me.SelectNextControl(cmbItem_MAN, True, True, True, True)
        End If
    End Sub

    Private Sub cmbItem_MAN_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItem_MAN.Leave
        Dim DT_ As DataTable
        DT_ = New DataTable
        If TAGTYPE_CATEGORY Then
            If cmbItem_MAN.Text <> "" Then
                strSql = " SELECT ISNULL(TAGTYPE,'') TAGTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "' "
                If GetSqlValue(cn, strSql).ToString = "N" Or GetSqlValue(cn, strSql).ToString = "" Then
                    cmbItemType_MAN.Enabled = False
                    Exit Sub
                End If
            End If
            strSql = "SELECT NAME FROM " + cnAdminDb + "..ITEMTYPE WHERE CATCODE = (SELECT CATCODE FROM " + cnAdminDb + "..ITEMMAST WHERE ITEMNAME = '" + cmbItem_MAN.Text + "')"
            DT_ = GetSqlTable(strSql, cn)
            If DT_.Rows.Count > 0 Then
                cmbItemType_MAN.Items.Clear()
                For i As Integer = 0 To DT_.Rows.Count - 1
                    cmbItemType_MAN.Items.Add(DT_.Rows(i).Item(0))
                Next
                If cmbItemType_MAN.Items.Count > 0 Then cmbItemType_MAN.Enabled = True Else cmbItemType_MAN.Enabled = False
            Else
                strSql = " SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE ORDER BY dispalyorder,NAME"
                objGPack.FillCombo(strSql, cmbItemType_MAN, , False)
                If cmbItemType_MAN.Items.Count > 0 Then cmbItemType_MAN.Enabled = True Else cmbItemType_MAN.Enabled = False
            End If
        End If
        'strSql = "SELECT SUBITEMNAME FROM " + cnAdminDb + "..SUBITEMMAST WHERE ITEMID = (SELECT ITEMID FROM " + cnAdminDb + "..ITEMMAST WHERE ITEMNAME = '" + cmbItem_MAN.Text + "')"
        'DT_.Clear()
        'DT_ = GetSqlTable(strSql, cn)
        'cmbSubItem_Man.Items.Clear()
        'For i As Integer = 0 To DT_.Rows.Count - 1
        '    cmbSubItem_Man.Items.Add(DT_.Rows(i).Item(0))
        'Next
        Get_Tollerence()
        If cmbItem_MAN.Text.Trim = "" Then
            Dim iId As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"))
            strSql = GetSubItemQry(New String() {"SUBITEMNAME SUBITEM"}, iId)
            objGPack.FillCombo(strSql, cmbSubItem_Man)
        End If
        If cmbSubItem_Man.Text = "" Then
            cmbSubItem_Man.Enabled = False
        Else
            cmbSubItem_Man.Enabled = True

        End If

        If TAGIMAGE_ITEMMAST Then
            TAGIMAGE = IIf(objGPack.GetSqlValue("SELECT TAGIMAGE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'") = "Y", True, False)
        Else
            TAGIMAGE = True
        End If

        If Itemmast_PctPath Then
            strSql = "SELECT ISNULL(ITEMPCTPATH,'') ITEMPCTPATH FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"
            defalutDestination = UCase(objGPack.GetSqlValue(strSql, "ITEMPCTPATH", "", tran))
            If defalutDestination.ToString = "" Then
                strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICPATH'"
                defalutDestination = UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", , tran))
            End If
            If Not defalutDestination.EndsWith("\") And defalutDestination <> "" Then defalutDestination += "\"
        Else
            strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICPATH'"
            defalutDestination = UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", , tran))
            If Not defalutDestination.EndsWith("\") And defalutDestination <> "" Then defalutDestination += "\"
        End If


        If ItemLock Then
            cmbItem_MAN.Select()
            Exit Sub
        End If
        cmbItem_MAN_SelectionChangeCommitted(Me, New EventArgs)
    End Sub
    Function Get_Tollerence() As Integer
        If cmbItem_MAN.Text <> "" Then
            strSql = " SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"
            Dim MetalId As String = objGPack.GetSqlValue(strSql)
            If MetalId <> "" Then
                Tag_Tolerance = Val(GetAdmindbSoftValue("TAGTOLERANCE_" & MetalId, "0"))
                If Tag_Tolerance = 0 Then Tag_Tolerance = Val(GetAdmindbSoftValue("TAGTOLERANCE", "0"))
            Else
                Tag_Tolerance = Val(GetAdmindbSoftValue("TAGTOLERANCE", "0"))
            End If
        End If
    End Function
    Private Sub cmbItem_Man_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItem_MAN.LostFocus
        HideSearch()
        'If cmbItem_MAN.Text = "" Or cmbItem_MAN.Items.Contains(cmbItem_MAN.Text) = False Then
        '    cmbItem_MAN.Focus()
        '    Exit Sub
        'End If
        'Me.SelectNextControl(txtLotNo_Num_Man, True, True, True, True)
    End Sub

    Private Sub cmbItem_Man_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItem_MAN.TextChanged
        cmbSearch.Text = CType(sender, ComboBox).Text
    End Sub

    Private Sub ObjMinValue_txtMinWastage_Wet_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.ObjMinValues_txtMinMcPerGram_Amt_TextChanged(Me, New EventArgs)
    End Sub

    Private Sub WeighingScalePropertyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WeighingScalePropertyToolStripMenuItem.Click
        objPropertyDia = New PropertyDia(SerialPort1)
        objPropertyDia.ShowDialog()
    End Sub

    Private Sub txtGrossWt_Wet_Man_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGrossWt_Wet.LostFocus
        ObjPurDetail.txtPurGrossWt_Wet.Text = txtGrossWt_Wet.Text
    End Sub

    Private Sub txtGrossWt_Wet_MAN_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGrossWt_Wet.TextChanged
        If _IsWholeSaleType Then
            Dim LD_Rate As Double = 0
            strSql = " SELECT TOP 1 MAXRATE FROM " & cnAdminDb & "..CENTRATE "
            strSql += vbCrLf + " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "')"
            If cmbSubItem_Man.Enabled = True And cmbSubItem_Man.Text.Trim <> "" Then
                strSql += vbCrLf + " AND SUBITEMID IN(SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "' AND ITEMID = ( SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'))"
            End If
            strSql += vbCrLf + " AND ISNULL(CONVERT(NUMERIC(15,4),(" & Val(txtGrossWt_Wet.Text.ToString) & "/CASE WHEN " & Val(txtPieces_Num_Man.Text.ToString) & " = 0 THEN 1 ELSE " & Val(txtPieces_Num_Man.Text.ToString) & " END)*100),0) BETWEEN FROMCENT AND TOCENT"
            LD_Rate = Val(objGPack.GetSqlValue(strSql, , "0").ToString)
            If LD_Rate > 0 Then
                txtRate_Amt.Text = Format(Math.Round(Val(LD_Rate), 3), "0.00")
            End If
        End If
        strSql = " SELECT RATE FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = '" & SNO & "' AND ISNULL(RATE,0) <> 0"
        Dim _TLotrate As Double = 0
        _TLotrate = Val(objGPack.GetSqlValue(strSql,, 0).ToString)
        txtRate_Amt.Text = ""
        If Val(_TLotrate) > 0 Then
            txtRate_Amt.Text = Val(_TLotrate)
        End If
        If RATE_FROM_WMCTABLE And (calType = "F") Then txtMetalRate_Amt.Text = Format(GetMetalRate(), "0.00")
        CalcMaxMinValues()
        CalcFinalTotal()
        If tagEdit Then txtMaxWastagePer_TextChanged(Me, New EventArgs)
    End Sub

    Private Sub txtTouch_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTouch_AMT.GotFocus
        If TOUCH_LOCK_TAG Then SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtTouch_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTouch_AMT.TextChanged
        CalcFinalTotal()
    End Sub

    Private Sub txtHmBillNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtHmBillNo.GotFocus
        If Not _HasHallMark Then SendKeys.Send("{TAB}") : Exit Sub
        If Not NeedHallmark And updIssSno = "" Then SendKeys.Send("{TAB}") : Exit Sub
        StyleHallmarkDet()
        txtTagWt_WET.Clear()
        txthallmarkRowIndex.Clear()
        txtHallmarkNo.Clear()
        TabControl1.SelectedTab = TabHallmark
        Me.SelectNextControl(TabHallmark, True, True, True, True)
        If Val(txtGrossWt_Wet.Text) > 0 Then
            txtTagWt_WET.Text = txtGrossWt_Wet.Text
            txtTagWt_WET.Enabled = True
            txtTagWt_WET.SelectAll()
        Else
            txtTagWt_WET.Enabled = False
            txtHallmarkNo.Focus()
        End If
    End Sub

    Private Sub txtHmCentre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtHmCentre.GotFocus
        If Not _HasHallMark Then SendKeys.Send("{TAB}") : Exit Sub
        If Not NeedHallmark Then SendKeys.Send("{TAB}") : Exit Sub
    End Sub

    Private Sub txtRfId_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRfId.GotFocus
        If Not _HasRfId Then SendKeys.Send("{TAB}") : Exit Sub
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


    Private Sub cmbStCalc_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbStCalc.KeyPress
        '        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
        '            If txtStRate_Amt.Enabled = True And NEEDUS = True Then
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

    Private Sub txtMetalRate_Amt_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMetalRate_Amt.Enter
        'If NEEDUS = True And calType <> "R" And calType <> "M" And calType <> "F" And Studded_Loose = "L" Then
        '    SendKeys.Send("{TAB}")
        'End If
    End Sub

    Private Sub txtMetalRate_Amt_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMetalRate_Amt.Leave

        'strSql = " SELECT ISNULL(STUDDED,'') STUDDED FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') ='" & cmbItem_MAN.Text & "' AND METALID = 'D'"
        'Studded_Loose = objGPack.GetSqlValue(strSql, "STUDDED", "")
        'If NEEDUS = True And Studded_Loose = "L" And Val(txtMetalRate_Amt.Text) = 0 Then
        '    If Not (calType = "R" Or calType = "M") Then
        '        If ObjRsUs.ShowDialog = Windows.Forms.DialogResult.OK Then
        '            txtMetalRate_Amt.Text = Convert.ToDouble(Val(ObjRsUs.txtUSDollar_Amt.Text) * Val(ObjRsUs.txtIndRs_Amt.Text)).ToString("0.00")
        '        End If
        '    End If
        'End If

    End Sub

    Private Sub txtHmBillNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtHmBillNo.KeyPress
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            If HALLMARKVALID = True And txtHmBillNo.Text = "" Then MsgBox("Hallmark Billno Is empty") : txtHmBillNo.Focus() : Exit Sub
        End If
    End Sub

    Private Sub txtHmCentre_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtHmCentre.KeyPress
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            'If HALLMARKVALID = True And txtHmCentre.Text = "" Then MsgBox("Hallmark Centre Is empty") : txtHmCentre.Focus() : Exit Sub
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
        LotNo1 = LSet(txtLotNo_Num_Man.Text.ToString(), 8)
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
        Dim OItemid As Integer
        strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItem_MAN.Text & "'"
        OItemid = Val(objGPack.GetSqlValue(strSql, "ITEMID", , tran))

        strSql = "SELECT * FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE ISNULL(ACTIVE,'')<>'N' ORDER BY MISCID"
        Dim DtOth As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(DtOth)
        For k As Integer = 0 To DtOth.Rows.Count - 1
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
            strSql = " SELECT ID,NAME "
            strSql += " ,MISCID "
            strSql += " ,(SELECT TOP 1 MISCNAME FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE MISCID = A.MISCID)MISCNAME "
            strSql += " ,SHORTNAME"
            strSql += " FROM " & cnAdminDb & "..OTHERMASTER A "
            strSql += " WHERE ISNULL(ACTIVE,'')<>'N' "
            strSql += " AND MISCID='" & DtOth.Rows(k).Item("MISCID").ToString & "'"
            strSql += " AND (ITEMID='" & OItemid & "' OR ITEMID=0)"
            Dim retRow As DataRow = Nothing
            retRow = BrighttechPack.SearchDialog.Show_R("Select " & DtOth.Rows(k).Item("MISCNAME").ToString & " Detail", strSql, cn, 1, , , MNAME, , False, True)
            If retRow Is Nothing Then
            Else
                If Rindex > 0 Then
                    With DtAddStockEntry.Rows(Rindex - 1)
                        .Item("OTHID") = Val(retRow.Item("ID").ToString)
                        .Item("OTHNAME") = retRow.Item("NAME").ToString
                        .Item("MISCID") = Val(retRow.Item("MISCID").ToString)
                    End With
                Else
                    Dim ro As DataRow = Nothing
                    ro = DtAddStockEntry.NewRow
                    ro("OTHID") = Val(retRow.Item("ID").ToString)
                    ro("OTHNAME") = retRow.Item("NAME").ToString
                    ro("MISCID") = Val(retRow.Item("MISCID").ToString)
                    DtAddStockEntry.Rows.Add(ro)
                End If

            End If
        Next
    End Function


    Private Sub txtTagNo__Man_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTagNo__Man.LostFocus
        LoadAdditionalStockEntry()
        If GetAdmindbSoftValue("SPECIFIED_STYLENO", "N") = "Y" Then
            txtStyleCode.Text = funcspecifiedstyleno()
            txtStyleCode.ReadOnly = True
        Else
            txtStyleCode.ReadOnly = False
        End If
    End Sub

    'Private Sub cmbCostCentre_Man_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCostCentre_Man.TextChanged
    '    cmbCostCentre_Man.Text = dtTagDetails.Rows(0).Item("COSTCENTRE").ToString
    '    If dtTagDetails.Rows(0).Item("TABLECODE") <> "" Then
    '        cmbTableCode.Enabled = True
    '        strSql = " SELECT DISTINCT TABLECODE FROM " & cnAdminDb & "..WMCTABLE WHERE TABLECODE <> '' "
    '        If cmbCostCentre_Man.Text <> "" Then strSql += " AND ISNULL(COSTID,'') = ISNULL((SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_Man.Text & "'),'')"
    '        strSql += " ORDER BY TABLECODE"
    '        objGPack.FillCombo(strSql, cmbTableCode, True)
    '    Else
    '        cmbTableCode.Items.Clear()
    '        cmbTableCode.Enabled = False
    '    End If
    'End Sub
    Function funcReceiptStoneDetailAutoLoad(Optional ByVal Recsno As String = "") As Integer
        ''STONEDETAILS
        strSql = " SELECT "
        strSql += " '' PACKETNO,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = STNITEMID)AS ITEM"
        strSql += " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = STNSUBITEMID)AS SUBITEM"
        strSql += " ,STNPCS PCS,STNWT WEIGHT"
        strSql += " ,STONEUNIT UNIT,CALCMODE CALC,STNRATE RATE,STNAMT AMOUNT"
        strSql += " ,(SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) AS METALID"
        strSql += " ,NULL PURRATE"
        strSql += " ,NULL PURVALUE,'' STNSNO,0 AS USRATE,0 AS INDRS"
        strSql += " FROM " & cnStockDb & "..RECEIPTSTONE S"
        strSql += " WHERE ISSSNO = '" & Recsno & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtStoneDetails)
        dtStoneDetails.AcceptChanges()
        StyleGridStone()
        CalcLessWt()
        CalcFinalTotal()
    End Function
    Function funcReceiptMisCharges(Optional ByVal Recsno As String = "")
        ''MISCCHARGE
        Dim MiscId As Integer
        Dim Comm As Decimal
        strSql = "SELECT MISCID FROM " & cnStockDb & "..RECEIPTMISC WHERE ISSSNO='" & Recsno & "'"
        MiscId = Val(objGPack.GetSqlValue(strSql, "MISCID", 0).ToString)
        If MiscId = 0 Then Exit Function
        strSql = "SELECT ISNULL(COMMISSION,0)COMMISSION FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCID=" & MiscId
        Comm = Val(objGPack.GetSqlValue(strSql, "COMMISSION", 0).ToString)
        If Comm <= 0 Then Exit Function
        strSql = " SELECT"
        strSql += " (SELECT MISCNAME FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCID = T.MISCID)MISC"
        strSql += " ,CONVERT(NUMERIC(15,2),AMOUNT+((AMOUNT * " & Comm & ")/100))AMOUNT"
        strSql += " ,0 PURAMOUNT "
        strSql += " FROM " & cnStockDb & "..RECEIPTMISC AS T"
        strSql += " WHERE ISSSNO = '" & Recsno & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtMiscDetails)
        StyleGridMisc()
        dtMiscDetails.AcceptChanges()
        CalcFinalTotal()
    End Function

    Private Sub cmbStItem_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbStItem.KeyDown
        If e.KeyCode = Keys.Enter Then
            If CType(sender, ComboBox).FindStringExact(CType(sender, ComboBox).SelectedText) <> -1 Then
                If CmbStSubItem.Text <> "" And CmbStSubItem.Enabled = True Then
                    strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & CmbStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbStItem.Text & "')"
                Else
                    strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbStItem.Text & "'"
                End If
                Dim calType As String = objGPack.GetSqlValue(strSql, , , tran)
                cmbStCalc.Text = IIf(calType = "R", "P", "W")

                If CmbStSubItem.Text <> "" And CmbStSubItem.Enabled = True Then
                    strSql = " SELECT STONEUNIT FROM " & cnAdminDb & "..SUBITEMMAST  WHERE SUBITEMNAME = '" & CmbStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbStItem.Text & "')"
                Else
                    strSql = " SELECT STONEUNIT FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbStItem.Text & "'"
                End If
                cmbStUnit.Text = objGPack.GetSqlValue(strSql, , , tran)

                strSql = " SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbStItem.Text & "'"
                txtStMetalCode.Text = objGPack.GetSqlValue(strSql, "DIASTONE", , tran)
                Me.SelectNextControl(cmbStItem, True, True, True, True)

                Dim tempitemid As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') = '" & cmbItem_MAN.Text & "'").ToString)
                Dim tempSitemid As Integer = 0
                If cmbSubItem_Man.Text <> "" Then
                    tempSitemid = objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE ISNULL(SUBITEMNAME,'') = '" & cmbSubItem_Man.Text & "'")
                End If
                Dim dtdesgstnrow As DataRow
                Dim tempDesignerid As Integer
                If cmbDesigner_MAN.Text <> "" Then tempDesignerid = objGPack.GetSqlValue("SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE ISNULL(DESIGNERNAME,'') = '" & cmbDesigner_MAN.Text & "'")
                dtdesgstnrow = GetSqlRow("SELECT STNDEDPER,STN_RATE,STNITEMID,STNSUBITEMID,CALTYPE,STONEUNIT FROM " & cnAdminDb & "..DESIGNERSTONE WHERE ACTIVE <> 'N' AND DESIGNERID = " & tempDesignerid & " AND ITEMID = " & tempitemid & " AND SUBITEMID = " & tempSitemid, cn)
                If dtdesgstnrow Is Nothing Then
                    dtdesgstnrow = GetSqlRow("SELECT STNDEDPER,STN_RATE,STNITEMID,STNSUBITEMID,CALTYPE,STONEUNIT FROM " & cnAdminDb & "..DESIGNERSTONE WHERE ACTIVE <> 'N' AND DESIGNERID = 0 AND ITEMID = " & tempitemid & " AND SUBITEMID = " & tempSitemid, cn)
                Else
                    cmbStUnit.Text = dtdesgstnrow(5).ToString
                    cmbStCalc.Text = dtdesgstnrow(4).ToString
                End If

            End If
        End If
    End Sub

    Private Sub cmbStItem_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStItem.Leave
        If cmbStItem.Text = "" Then Exit Sub
        If cmbStItem.Text <> "" Then LoadSubItem(CmbStSubItem)
        Dim view4c As String = ""
        Dim maintain4c As Boolean
        strSql = "SELECT ISNULL(MAINTAIN4C,'N') FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "' "
        If objGPack.GetSqlValue(strSql).ToString.ToUpper = "N" Then maintain4c = False Else maintain4c = True
        If maintain4c Then
            If IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "'", , , )
            If Not view4c.Contains("CO") Then CmbStColor.Enabled = False : CmbStColor.Text = "" Else CmbStColor.Enabled = True
            If Not view4c.Contains("CL") Then CmbStClarity.Enabled = False : CmbStClarity.Text = "" Else CmbStClarity.Enabled = True
            If Not view4c.Contains("SH") Then CmbStShape.Enabled = False : CmbStShape.Text = "" Else CmbStShape.Enabled = True
            If Not view4c.Contains("SI") Then CmbStSize.Enabled = False : CmbStSize.Text = "" Else CmbStSize.Enabled = True
        Else
            CmbStColor.Enabled = False : CmbStColor.Text = ""
            CmbStClarity.Enabled = False : CmbStClarity.Text = ""
            CmbStShape.Enabled = False : CmbStShape.Text = ""
            CmbStSize.Enabled = False : CmbStSize.Text = ""
        End If
        Me.SelectNextControl(cmbStItem, True, True, True, True)
    End Sub

    Private Sub CmbStSubItem_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles CmbStSubItem.KeyDown
        If e.KeyCode = Keys.Enter Then
            If CType(sender, ComboBox).FindStringExact(CType(sender, ComboBox).SelectedText) <> -1 Then
                If CmbStSubItem.Text <> "" And CmbStSubItem.Enabled = True Then
                    strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & CmbStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbStItem.Text & "')"
                Else
                    strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbStItem.Text & "'"
                End If
                Dim calType As String = objGPack.GetSqlValue(strSql, , , tran)
                cmbStCalc.Text = IIf(calType = "R", "P", "W")

                If CmbStSubItem.Text <> "" And CmbStSubItem.Enabled = True Then
                    strSql = " SELECT STONEUNIT FROM " & cnAdminDb & "..SUBITEMMAST  WHERE SUBITEMNAME = '" & CmbStSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbStItem.Text & "')"
                Else
                    strSql = " SELECT STONEUNIT FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbStItem.Text & "'"
                End If
                cmbStUnit.Text = objGPack.GetSqlValue(strSql, , , tran)

                strSql = " SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbStItem.Text & "'"
                txtStMetalCode.Text = objGPack.GetSqlValue(strSql, "DIASTONE", , tran)
                Me.SelectNextControl(CmbStSubItem, True, True, True, True)
            End If
        End If
    End Sub

    Private Sub txtStItem_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStItem.Leave
        If txtStItem.Text = "" Then Exit Sub
        Dim view4c As String = ""
        Dim maintain4c As Boolean
        strSql = "SELECT ISNULL(MAINTAIN4C,'N') FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "' "
        If objGPack.GetSqlValue(strSql).ToString.ToUpper = "N" Then maintain4c = False Else maintain4c = True
        If ORDER_MULTI_MIMR Then
            If IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "'", , , )
            CmbStColor.Enabled = False
            CmbStClarity.Enabled = False
            CmbStShape.Enabled = False
            CmbStSize.Enabled = False
        Else
            If maintain4c Then
                If IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "'", , , )
                If Not view4c.Contains("CO") Then CmbStColor.Enabled = False : CmbStColor.Text = "" Else CmbStColor.Enabled = True
                If Not view4c.Contains("CL") Then CmbStClarity.Enabled = False : CmbStClarity.Text = "" Else CmbStClarity.Enabled = True
                If Not view4c.Contains("SH") Then CmbStShape.Enabled = False : CmbStShape.Text = "" Else CmbStShape.Enabled = True
                If Not view4c.Contains("SI") Then CmbStSize.Enabled = False : CmbStSize.Text = "" Else CmbStSize.Enabled = True
            Else
                CmbStColor.Enabled = False : CmbStColor.Text = ""
                CmbStClarity.Enabled = False : CmbStClarity.Text = ""
                CmbStShape.Enabled = False : CmbStShape.Text = ""
                CmbStSize.Enabled = False : CmbStSize.Text = ""
            End If
        End If
    End Sub

    Private Sub txtStSubItem_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtStSubItem.KeyDown
        If e.KeyCode = Keys.Enter Then
            'Me.SelectNextControl(txtStSubItem, True, True, True, True)
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtStSubItem_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStSubItem.Leave
        If txtStSubItem.Text = "" Then Exit Sub
        Dim view4c As String = ""
        Dim maintain4c As Boolean
        strSql = "SELECT ISNULL(MAINTAIN4C,'N') FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "' "
        If objGPack.GetSqlValue(strSql).ToString.ToUpper = "N" Then maintain4c = False Else maintain4c = True
        strSql = "SELECT ISNULL(MAINTAIN4C,'N') FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStSubItem.Text, CmbStSubItem.Text) & "' AND ITEMID=(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "')"
        If objGPack.GetSqlValue(strSql).ToString.ToUpper = "N" Then maintain4c = False Else maintain4c = True
        If ORDER_MULTI_MIMR Then
            If IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "'", , , )
            CmbStColor.Enabled = False
            CmbStClarity.Enabled = False
            CmbStShape.Enabled = False
            CmbStSize.Enabled = False
        Else
            If maintain4c Then
                If IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "'", , , )
                If Not view4c.Contains("CO") Then CmbStColor.Enabled = False : CmbStColor.Text = "" Else CmbStColor.Enabled = True
                If Not view4c.Contains("CL") Then CmbStClarity.Enabled = False : CmbStClarity.Text = "" Else CmbStClarity.Enabled = True
                If Not view4c.Contains("SH") Then CmbStShape.Enabled = False : CmbStShape.Text = "" Else CmbStShape.Enabled = True
                If Not view4c.Contains("SI") Then CmbStSize.Enabled = False : CmbStSize.Text = "" Else CmbStSize.Enabled = True
            Else
                CmbStColor.Enabled = False : CmbStColor.Text = ""
                CmbStClarity.Enabled = False : CmbStClarity.Text = ""
                CmbStShape.Enabled = False : CmbStShape.Text = ""
                CmbStSize.Enabled = False : CmbStSize.Text = ""
            End If
        End If
    End Sub

    Private Sub CmbStSubItem_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmbStSubItem.Leave
        If CmbStSubItem.Text = "" Then Exit Sub
        Dim view4c As String = ""
        Dim maintain4c As Boolean
        strSql = "SELECT ISNULL(MAINTAIN4C,'N') FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "' "
        If objGPack.GetSqlValue(strSql).ToString.ToUpper = "N" Then maintain4c = False Else maintain4c = True
        strSql = "SELECT ISNULL(MAINTAIN4C,'N') FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & IIf(ACC_STUDITEM_POPUP, txtStSubItem.Text, CmbStSubItem.Text) & "' AND ITEMID=(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "')"
        If objGPack.GetSqlValue(strSql).ToString.ToUpper = "N" Then maintain4c = False Else maintain4c = True
        If maintain4c Then
            If IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "'", , , )
            If IIf(ACC_STUDITEM_POPUP, txtStSubItem.Text, CmbStSubItem.Text) <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & IIf(ACC_STUDITEM_POPUP, txtStSubItem.Text, CmbStSubItem.Text) & "' AND ITEMID=(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & IIf(ACC_STUDITEM_POPUP, txtStItem.Text, cmbStItem.Text) & "')", , , )
            If Not view4c.Contains("CO") Then CmbStColor.Enabled = False : CmbStColor.Text = "" Else CmbStColor.Enabled = True
            If Not view4c.Contains("CL") Then CmbStClarity.Enabled = False : CmbStClarity.Text = "" Else CmbStClarity.Enabled = True
            If Not view4c.Contains("SH") Then CmbStShape.Enabled = False : CmbStShape.Text = "" Else CmbStShape.Enabled = True
            If Not view4c.Contains("SI") Then CmbStSize.Enabled = False : CmbStSize.Text = "" Else CmbStSize.Enabled = True
        Else
            CmbStColor.Enabled = False : CmbStColor.Text = ""
            CmbStClarity.Enabled = False : CmbStClarity.Text = ""
            CmbStShape.Enabled = False : CmbStShape.Text = ""
            CmbStSize.Enabled = False : CmbStSize.Text = ""
        End If
    End Sub

    Private Sub CmbStShape_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles CmbStShape.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub CmbStColor_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles CmbStColor.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub CmbStClarity_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles CmbStClarity.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub objSetItem_txtSetWastagePer_Per_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        CalcMaxMinValues()
    End Sub

    Private Sub objSetItem_txtSetWastage_WET_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        CalcMaxMinValues()
    End Sub

    Private Sub objSetItem_txtSetMcPerGrm_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        CalcMaxMinValues()
    End Sub

    Private Sub objSetItem_txtSetMc_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        CalcMaxMinValues()
    End Sub

    Private Sub SetItemToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SetItemToolStripMenuItem.Click
        If SetGrpIdVis = True Then
            setItem = True
            objSetItem.StartPosition = FormStartPosition.CenterScreen
            objSetItem.StartPosition = FormStartPosition.CenterScreen
            objSetItem.grpWastageMc.BackColor = Drawing.Color.LightGray
            objSetItem.grpWastageMc.BackgroundColor = Drawing.Color.LightGray
            objSetItem.grpWastageMc.BackgroundGradientColor = Drawing.Color.LightGray
            objSetItem.BackColor = Drawing.Color.LightGray
            objSetItem.txtSetId_NUM.Visible = False
            objSetItem.Label1.Visible = False
            If objSetItem.ShowDialog = Windows.Forms.DialogResult.OK Then
                pnlMax.Enabled = False
                txtGrossWt_Wet.Focus()
            End If
            txtGrossWt_Wet.Focus()
        End If
    End Sub

    Private Sub cmbSubItem_Man_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbSubItem_Man.KeyPress
        If cmbSubItem_Man.Text.ToString = "" Then Exit Sub
        strSql = "SELECT SETITEM FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_Man.Text & "' "
        strSql += " AND ITEMID IN (SELECT TOP 1 ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbItem_MAN.Text & "')"
        If objGPack.GetSqlValue(strSql) = "Y" Then
            txtSetTagno.Enabled = True
            CmbSetItem.Text = "YES"
        End If
    End Sub

    Private Sub txtMaxWastage_Per_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtMaxWastage_Per.KeyPress
        If LOCK_MAXWASTAGEPER_ITEMTAG Then
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Authorize, False) Then
                txtMaxWastage_Per.ReadOnly = True
                'SendKeys.Send("{TAB}")
            End If
        Else
            If tagEdit Then txtMaxWastagePer_TextChanged(Me, New System.EventArgs)
        End If
    End Sub

    Private Sub txtStWPer_Num_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStWPer_AMT.TextChanged
        If Val(txtStWPer_AMT.Text.ToString) <> 0 Then
            Dim _stWt As Double = 0
            _stWt = Val(txtGrossWt_Wet.Text.ToString) * Val(txtStWPer_AMT.Text.ToString) / 100
            _stWt = Math.Round(_stWt, 4)
            txtStWeight.Text = Format(_stWt, "0.0000").ToString
        End If
    End Sub

    Private Sub txtLessWt_Wet_Leave(sender As Object, e As EventArgs) Handles txtLessWt_Wet.Leave

    End Sub

    Function FuncprintBarcode_Single(ByVal ItemId As String, ByVal Tagno As String)
        Try
            Dim systemName As String = My.Computer.Name
            strSql = " IF OBJECT_ID('TEMPTABLEDB..[TEMPBARCODE" & systemName & "SINGLE]') IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMPBARCODE" & systemName & "SINGLE]"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            strSql = " SELECT "
            strSql += vbCrLf + " SNO,(SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID)ITEMNAME,"
            strSql += vbCrLf + " (SELECT TOP 1 SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID=T.SUBITEMID)SUBITEMNAME,"
            strSql += vbCrLf + " (SELECT TOP 1 SHORTNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID=T.SUBITEMID)SHORTNAME,"
            strSql += vbCrLf + " (SELECT TOP 1 NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID=T.ITEMTYPEID)ITEMTYPE,"
            strSql += vbCrLf + " (SELECT TOP 1 METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID)METALID,"
            strSql += vbCrLf + " ITEMID,SUBITEMID,TABLECODE,DESIGNERID,PCS,GRSWT,NETWT,LESSWT,"
            strSql += vbCrLf + " RATE,MAXWASTPER,MAXWAST,MAXMCGRM,MAXMC,SALVALUE,ITEMTYPEID,STYLENO"
            strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT DISTINCT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='S')),0)STNWT"
            strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT DISTINCT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='S') GROUP BY TAGSNO,ITEMID),0)STNPCS,'' STNNAME"
            strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT DISTINCT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D' AND ISNULL(BEEDS,'')<>'Y') GROUP BY TAGSNO,ITEMID),0)DIAWT"
            strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT DISTINCT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D' AND ISNULL(BEEDS,'')<>'Y') GROUP BY TAGSNO,ITEMID),0)DIAPCS,'' DIANAME"
            strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT DISTINCT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(BEEDS,'')='Y') GROUP BY TAGSNO,ITEMID),0)BDSWT"
            strSql += vbCrLf + " INTO TEMPTABLEDB..[TEMPBARCODE" & systemName & "SINGLE] FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + " WHERE ITEMID='" & ItemId & "' AND TAGNO='" & Tagno & "'"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            strSql = "SELECT * FROM TEMPTABLEDB..[TEMPBARCODE" & systemName & "SINGLE]"
            Dim dtTag As DataTable
            cmd = New OleDbCommand(strSql, cn)
            da = New OleDbDataAdapter(cmd)
            dtTag = New DataTable
            da.Fill(dtTag)
            If dtTag.Rows.Count = 0 Then Exit Function
            Dim ItemName As String = "" : Dim SubItemName As String = ""
            Dim GrsWt As String = "" : Dim NetWt As String = ""
            Dim LessWt As String = "" : Dim MAXWASTPER As String = ""
            Dim MAXWAST As String = "" : Dim MAXMCGRM As String = ""
            Dim MAXMC As String = "" : Dim stnAmt As String = ""
            Dim stnWt As String = "" : Dim stnPcs As String = "" : Dim stnName As String = ""
            Dim DiaAmt As String = "" : Dim DiaPcs As String = "" : Dim DiaWt As String = "" : Dim BDSWt As String = ""
            Dim DiaName As String = "" : Dim Rate As String = ""
            Dim SalValue As String = "" : Dim ItemType As String = ""
            Dim MetalId As String = ""

            Dim stnWt1 As String = "" : Dim stnPcs1 As String = "" : Dim stnName1 As String = "" : Dim stoneUnit1 As String = ""
            Dim stnWt2 As String = "" : Dim stnPcs2 As String = "" : Dim stnName2 As String = "" : Dim stoneUnit2 As String = ""
            Dim stnWt3 As String = "" : Dim stnPcs3 As String = "" : Dim stnName3 As String = "" : Dim stoneUnit3 As String = ""
            Dim stnWt4 As String = "" : Dim stnPcs4 As String = "" : Dim stnName4 As String = "" : Dim stoneUnit4 As String = ""
            Dim stnWt5 As String = "" : Dim stnPcs5 As String = "" : Dim stnName5 As String = "" : Dim stoneUnit5 As String = ""
            Dim stnWt6 As String = "" : Dim stnPcs6 As String = "" : Dim stnName6 As String = "" : Dim stoneUnit6 As String = ""
            Dim stnWt7 As String = "" : Dim stnPcs7 As String = "" : Dim stnName7 As String = "" : Dim stoneUnit7 As String = ""
            Dim stnWt8 As String = "" : Dim stnPcs8 As String = "" : Dim stnName8 As String = "" : Dim stoneUnit8 As String = ""
            Dim TotStnWt As String = "" : Dim TotStnPcs As String = ""

            strSql = " SELECT SUM(ISNULL(STNWT,0)) FROM " & cnAdminDb & "..ITEMTAGSTONE AS S WHERE TAGSNO ='" & dtTag.Rows(0)("SNO").ToString & "'"
            TotStnWt = Format(Val(objGPack.GetSqlValue(strSql, , "")), "0.00")

            strSql = " SELECT SUM(ISNULL(STNPCS,0)) FROM " & cnAdminDb & "..ITEMTAGSTONE AS S WHERE TAGSNO ='" & dtTag.Rows(0)("SNO").ToString & "'"
            TotStnPcs = Format(Val(objGPack.GetSqlValue(strSql, , "")), "0.00")

            'TotStnWt
            Dim dtTagStone As New DataTable
            strSql = "SELECT (SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=S.STNITEMID)STNITEMNAME,"
            strSql += vbCrLf + " (SELECT TOP 1 SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID=S.STNSUBITEMID)STNSUBITEMNAME,"
            strSql += vbCrLf + " * FROM " & cnAdminDb & "..ITEMTAGSTONE AS S WHERE TAGSNO ='" & dtTag.Rows(0)("SNO").ToString & "'"
            cmd = New OleDbCommand(strSql, cn)
            da = New OleDbDataAdapter(cmd)
            dtTagStone = New DataTable
            da.Fill(dtTagStone)
            For K As Integer = 0 To dtTagStone.Rows.Count - 1
                If K > 3 Then Exit For
                If K = 0 Then
                    stnWt1 = IIf(Val(dtTagStone.Rows(K)("STNWT").ToString) > 0,
                    Format(Math.Round(Val(dtTagStone.Rows(K)("STNWT").ToString), 2), "0.00") _
                    , "")
                    stnPcs1 = IIf(Val(dtTagStone.Rows(K)("STNPCS").ToString) > 0, dtTagStone.Rows(K)("STNPCS").ToString, "")
                    stnName1 = IIf(dtTagStone.Rows(K)("STNSUBITEMNAME").ToString <> "", dtTagStone.Rows(K)("STNSUBITEMNAME").ToString, dtTagStone.Rows(K)("STNITEMNAME").ToString)
                    If stnName1 <> "" Then stnName1 = stnName1.ToString.Substring(0, 1)
                    stoneUnit1 = dtTagStone.Rows(K)("STONEUNIT").ToString
                End If
                If K = 1 Then
                    stnWt2 = IIf(Val(dtTagStone.Rows(K)("STNWT").ToString) > 0,
                   Format(Math.Round(Val(dtTagStone.Rows(K)("STNWT").ToString), 2), "0.00") _
                    , "")
                    stnPcs2 = IIf(Val(dtTagStone.Rows(K)("STNPCS").ToString) > 0, dtTagStone.Rows(K)("STNPCS").ToString, "")
                    stnName2 = IIf(dtTagStone.Rows(K)("STNSUBITEMNAME").ToString <> "", dtTagStone.Rows(K)("STNSUBITEMNAME").ToString, dtTagStone.Rows(K)("STNITEMNAME").ToString)
                    If stnName2 <> "" Then stnName2 = stnName2.ToString.Substring(0, 1)
                    stoneUnit2 = dtTagStone.Rows(K)("STONEUNIT").ToString
                End If
                If K = 2 Then
                    stnWt3 = IIf(Val(dtTagStone.Rows(K)("STNWT").ToString) > 0,
                    Format(Math.Round(Val(dtTagStone.Rows(K)("STNWT").ToString), 2), "0.00") _
                    , "")
                    stnPcs3 = IIf(Val(dtTagStone.Rows(K)("STNPCS").ToString) > 0, dtTagStone.Rows(K)("STNPCS").ToString, "")
                    stnName3 = IIf(dtTagStone.Rows(K)("STNSUBITEMNAME").ToString <> "", dtTagStone.Rows(K)("STNSUBITEMNAME").ToString, dtTagStone.Rows(K)("STNITEMNAME").ToString)
                    If stnName3 <> "" Then stnName3 = stnName3.ToString.Substring(0, 1)
                    stoneUnit3 = dtTagStone.Rows(K)("STONEUNIT").ToString
                End If
                If K = 3 Then
                    stnWt4 = IIf(Val(dtTagStone.Rows(K)("STNWT").ToString) > 0,
                   Format(Math.Round(Val(dtTagStone.Rows(K)("STNWT").ToString), 2), "0.00") _
                    , "")
                    stnPcs4 = IIf(Val(dtTagStone.Rows(K)("STNPCS").ToString) > 0, dtTagStone.Rows(K)("STNPCS").ToString, "")
                    stnName4 = IIf(dtTagStone.Rows(K)("STNSUBITEMNAME").ToString <> "", dtTagStone.Rows(K)("STNSUBITEMNAME").ToString, dtTagStone.Rows(K)("STNITEMNAME").ToString)
                    If stnName4 <> "" Then stnName4 = stnName4.ToString.Substring(0, 1)
                    stoneUnit4 = dtTagStone.Rows(K)("STONEUNIT").ToString
                End If
                If K = 4 Then
                    stnWt5 = IIf(Val(dtTagStone.Rows(K)("STNWT").ToString) > 0,
                   Format(Math.Round(Val(dtTagStone.Rows(K)("STNWT").ToString), 2), "0.00") _
                    , "")
                    stnPcs5 = IIf(Val(dtTagStone.Rows(K)("STNPCS").ToString) > 0, dtTagStone.Rows(K)("STNPCS").ToString, "")
                    stnName5 = IIf(dtTagStone.Rows(K)("STNSUBITEMNAME").ToString <> "", dtTagStone.Rows(K)("STNSUBITEMNAME").ToString, dtTagStone.Rows(K)("STNITEMNAME").ToString)
                    If stnName5 <> "" Then stnName5 = stnName5.ToString.Substring(0, 1)
                    stoneUnit5 = dtTagStone.Rows(K)("STONEUNIT").ToString
                End If
                If K = 5 Then
                    stnWt6 = IIf(Val(dtTagStone.Rows(K)("STNWT").ToString) > 0,
                   Format(Math.Round(Val(dtTagStone.Rows(K)("STNWT").ToString), 2), "0.00") _
                    , "")
                    stnPcs6 = IIf(Val(dtTagStone.Rows(K)("STNPCS").ToString) > 0, dtTagStone.Rows(K)("STNPCS").ToString, "")
                    stnName6 = IIf(dtTagStone.Rows(K)("STNSUBITEMNAME").ToString <> "", dtTagStone.Rows(K)("STNSUBITEMNAME").ToString, dtTagStone.Rows(K)("STNITEMNAME").ToString)
                    If stnName6 <> "" Then stnName6 = stnName6.ToString.Substring(0, 1)
                    stoneUnit6 = dtTagStone.Rows(K)("STONEUNIT").ToString
                End If
                If K = 6 Then
                    stnWt7 = IIf(Val(dtTagStone.Rows(K)("STNWT").ToString) > 0,
                   Format(Math.Round(Val(dtTagStone.Rows(K)("STNWT").ToString), 2), "0.00") _
                    , "")
                    stnPcs7 = IIf(Val(dtTagStone.Rows(K)("STNPCS").ToString) > 0, dtTagStone.Rows(K)("STNPCS").ToString, "")
                    stnName7 = IIf(dtTagStone.Rows(K)("STNSUBITEMNAME").ToString <> "", dtTagStone.Rows(K)("STNSUBITEMNAME").ToString, dtTagStone.Rows(K)("STNITEMNAME").ToString)
                    If stnName7 <> "" Then stnName7 = stnName7.ToString.Substring(0, 1)
                    stoneUnit7 = dtTagStone.Rows(K)("STONEUNIT").ToString
                End If
                If K = 7 Then
                    stnWt8 = IIf(Val(dtTagStone.Rows(K)("STNWT").ToString) > 0,
                   Format(Math.Round(Val(dtTagStone.Rows(K)("STNWT").ToString), 2), "0.00") _
                    , "")
                    stnPcs8 = IIf(Val(dtTagStone.Rows(K)("STNPCS").ToString) > 0, dtTagStone.Rows(K)("STNPCS").ToString, "")
                    stnName8 = IIf(dtTagStone.Rows(K)("STNSUBITEMNAME").ToString <> "", dtTagStone.Rows(K)("STNSUBITEMNAME").ToString, dtTagStone.Rows(K)("STNITEMNAME").ToString)
                    If stnName8 <> "" Then stnName8 = stnName8.ToString.Substring(0, 1)
                    stoneUnit8 = dtTagStone.Rows(K)("STONEUNIT").ToString
                End If
            Next


            ItemName = dtTag.Rows(0)("ITEMNAME").ToString
            If dtTag.Rows(0)("SHORTNAME").ToString <> "" Then
                SubItemName = dtTag.Rows(0)("SHORTNAME").ToString
            Else
                SubItemName = dtTag.Rows(0)("SUBITEMNAME").ToString
            End If
            GrsWt = IIf(Val(dtTag.Rows(0)("GRSWT").ToString) > 0, dtTag.Rows(0)("GRSWT").ToString, "")
            NetWt = IIf(Val(dtTag.Rows(0)("NETWT").ToString) > 0, dtTag.Rows(0)("NETWT").ToString, "") 'dtTag.Rows(0)("NETWT").ToString
            LessWt = IIf(Val(dtTag.Rows(0)("LESSWT").ToString) > 0, dtTag.Rows(0)("LESSWT").ToString, "") ' dtTag.Rows(0)("LESSWT").ToString
            MAXWASTPER = IIf(Val(dtTag.Rows(0)("MAXWASTPER").ToString) > 0, dtTag.Rows(0)("MAXWASTPER").ToString, "") 'dtTag.Rows(0)("MAXWASTPER").ToString
            MAXWAST = IIf(Val(dtTag.Rows(0)("MAXWAST").ToString) > 0, dtTag.Rows(0)("MAXWAST").ToString, "") ' dtTag.Rows(0)("MAXWAST").ToString
            MAXMCGRM = IIf(Val(dtTag.Rows(0)("MAXMCGRM").ToString) > 0, dtTag.Rows(0)("MAXMCGRM").ToString, "") 'dtTag.Rows(0)("MAXMCGRM").ToString
            MAXMC = IIf(Val(dtTag.Rows(0)("MAXMC").ToString) > 0, dtTag.Rows(0)("MAXMC").ToString, "") 'dtTag.Rows(0)("MAXMC").ToString
            stnAmt = ""
            stnWt = IIf(Val(dtTag.Rows(0)("STNWT").ToString) > 0, dtTag.Rows(0)("STNWT").ToString, "") ' dtTag.Rows(0)("STNWT").ToString
            stnPcs = IIf(Val(dtTag.Rows(0)("STNPCS").ToString) > 0, dtTag.Rows(0)("STNPCS").ToString, "") 'dtTag.Rows(0)("STNPCS").ToString
            stnName = dtTag.Rows(0)("STNNAME").ToString
            DiaAmt = ""
            DiaWt = IIf(Val(dtTag.Rows(0)("DIAWT").ToString) > 0, dtTag.Rows(0)("DIAWT").ToString, "") 'dtTag.Rows(0)("DIAWT").ToString
            BDSWt = IIf(Val(dtTag.Rows(0)("BDSWT").ToString) > 0, dtTag.Rows(0)("BDSWT").ToString, "") 'dtTag.Rows(0)("DIAWT").ToString
            DiaPcs = IIf(Val(dtTag.Rows(0)("DIAPCS").ToString) > 0, dtTag.Rows(0)("DIAPCS").ToString, "") 'dtTag.Rows(0)("DIAPCS").ToString
            DiaName = dtTag.Rows(0)("DIANAME").ToString
            Rate = IIf(Val(dtTag.Rows(0)("RATE").ToString) > 0, dtTag.Rows(0)("RATE").ToString, "") 'dtTag.Rows(0)("RATE").ToString
            SalValue = IIf(Val(dtTag.Rows(0)("SALVALUE").ToString) > 0, dtTag.Rows(0)("SALVALUE").ToString, "") 'dtTag.Rows(0)("SALVALUE").ToString
            ItemType = dtTag.Rows(0)("ITEMTYPE").ToString
            MetalId = dtTag.Rows(0)("METALID").ToString

            'systemName
            strSql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='BARCODE" & systemName & "'"
            Dim barPrinter As String = objGPack.GetSqlValue(strSql, "", "")
            If barPrinter = "" Then
                barPrinter = System.Windows.Forms.SystemInformation.ComputerName
                barPrinter = "\\" & barPrinter & "\BARCODE" & systemName
            End If
            Dim barRead As String = ""

            Dim dtTemplate As DataTable
            strSql = "SELECT * FROM " & cnAdminDb & "..BARCODESETTING WHERE METALID='" & MetalId.ToString & "' AND ISNULL(FILENAME,'')<>''"
            cmd = New OleDbCommand(strSql, cn)
            da = New OleDbDataAdapter(cmd)
            dtTemplate = New DataTable
            da.Fill(dtTemplate)


            For k As Integer = 0 To dtTemplate.Rows.Count - 1
                Dim strCond As String = ""
                strCond = dtTemplate.Rows(k)("DESCRIPTION").ToString
                strSql = "SELECT 1 FROM TEMPTABLEDB..[TEMPBARCODE" & systemName & "SINGLE] WHERE " & strCond & " AND METALID='" & MetalId.ToString & "' "
                If Val(objGPack.GetSqlValue(strSql, , "").ToString) > 0 Then
                    If dtTemplate.Rows(k)("FILENAME").ToString <> "" Then
                        barRead = Application.StartupPath & "\BARCODE\" & dtTemplate.Rows(k)("FILENAME").ToString & ".PRN"
                        'Exit For
                    End If
                End If
            Next
            If barRead = "" Then
                If MetalId.ToString <> "" Then
                    barRead = Application.StartupPath & "\BARCODE\" & "BARCODE" & MetalId & ".PRN"
                Else
                    barRead = Application.StartupPath & "\BARCODE\" & "BARCODE1.PRN"
                End If
            End If
            If IO.Directory.Exists(Application.StartupPath & "\BARCODE") = False Then
                MsgBox("Directory not Exists..." & vbCrLf & "Directory Name : " & Application.StartupPath & "\BARCODE", MsgBoxStyle.Information)
                Exit Function
            End If
            If IO.File.Exists(barRead) = False Then
                MsgBox("Barcode Template not Found..." & vbCrLf & "File Name : " & barRead.ToString, MsgBoxStyle.Information)
                Exit Function
            End If
            Dim barWrite As String = Application.StartupPath & "\BARCODE\" & "DUPLICATE" & MetalId & systemName & ".MEM"

            Dim fileReader As New System.IO.StreamReader(barRead)
            Dim fileWriter As New System.IO.StreamWriter(barWrite)

            Dim StrRead As String
            While fileReader.EndOfStream = False
                StrRead = fileReader.ReadLine
                'DESCRIPTION
                If StrRead.Contains("TAGNO") Then
                    fileWriter.WriteLine(StrRead.Replace("TAGNO", ItemId & "-" & Tagno))
                ElseIf StrRead.Contains("SUBITEMNAME") Then
                    fileWriter.WriteLine(StrRead.Replace("SUBITEMNAME", SubItemName))
                ElseIf StrRead.Contains("ITEMNAME") Then
                    fileWriter.WriteLine(StrRead.Replace("ITEMNAME", ItemName))
                    'WT
                ElseIf StrRead.Contains("GRSWT") Then
                    If GrsWt <> "" Then fileWriter.WriteLine(StrRead.Replace("GRSWT", GrsWt)) Else Continue While
                ElseIf StrRead.Contains("NETWT") Then
                    If NetWt <> "" Then fileWriter.WriteLine(StrRead.Replace("NETWT", NetWt)) Else Continue While
                ElseIf StrRead.Contains("LESSWT") Then
                    If LessWt <> "" Then fileWriter.WriteLine(StrRead.Replace("LESSWT", LessWt)) Else Continue While
                    'VA
                ElseIf StrRead.Contains("MAXWASTPER") Then
                    If MAXWASTPER <> "" Then fileWriter.WriteLine(StrRead.Replace("MAXWASTPER", MAXWASTPER)) Else Continue While
                ElseIf StrRead.Contains("MAXWAST") Then
                    If MAXWAST <> "" Then fileWriter.WriteLine(StrRead.Replace("MAXWAST", MAXWAST)) Else Continue While
                ElseIf StrRead.Contains("MAXMCGRM") Then
                    If MAXMCGRM <> "" Then fileWriter.WriteLine(StrRead.Replace("MAXMCGRM", MAXMCGRM)) Else Continue While
                ElseIf StrRead.Contains("MAXMC") Then
                    If MAXMC <> "" Then fileWriter.WriteLine(StrRead.Replace("MAXMC", MAXMC)) Else Continue While
                ElseIf StrRead.Contains("ITEMTYPE") Then
                    If ItemType <> "" Then fileWriter.WriteLine(StrRead.Replace("ITEMTYPE", ItemType)) Else Continue While
                ElseIf StrRead.Contains("SALVALUE") Then
                    If SalValue <> "" Then fileWriter.WriteLine(StrRead.Replace("SALVALUE", SalValue)) Else Continue While
                    '    'STONE
                ElseIf StrRead.Contains("STNAMT") Then
                    If stnAmt <> "" Then fileWriter.WriteLine(StrRead.Replace("STNAMT", stnAmt)) Else Continue While
                    'ElseIf StrRead.Contains("STNWT") Then
                    '    If stnWt <> "" Then fileWriter.WriteLine(StrRead.Replace("STNWT", stnWt)) Else Continue While
                    'ElseIf StrRead.Contains("STNPCS") Then
                    '    If stnPcs <> "" Then fileWriter.WriteLine(StrRead.Replace("STNPCS", stnPcs)) Else Continue While
                    'ElseIf StrRead.Contains("STNNAME") Then
                    '    If stnName <> "" Then fileWriter.WriteLine(StrRead.Replace("STNNAME", stnName)) Else Continue While

                    '    'STONE1
                ElseIf StrRead.Contains("STNNAME1") Then
                    If stnName1 <> "" Then
                        StrRead = StrRead.Replace("STNNAME1", stnName1).ToString
                        StrRead = StrRead.Replace("STNPCS1", stnPcs1).ToString
                        StrRead = StrRead.Replace("STNWT1", stnWt1).ToString
                        StrRead = StrRead.Replace("STONEUNIT1", stoneUnit1).ToString
                        fileWriter.WriteLine(StrRead)
                    Else
                        Continue While
                    End If
                ElseIf StrRead.Contains("STNNAME2") Then
                    If stnName2 <> "" Then
                        StrRead = StrRead.Replace("STNNAME2", stnName2).ToString
                        StrRead = StrRead.Replace("STNPCS2", stnPcs2).ToString
                        StrRead = StrRead.Replace("STNWT2", stnWt2).ToString
                        StrRead = StrRead.Replace("STONEUNIT2", stoneUnit2).ToString
                        fileWriter.WriteLine(StrRead)
                    Else
                        Continue While
                    End If
                ElseIf StrRead.Contains("STNNAME3") Then
                    If stnName3 <> "" Then
                        StrRead = StrRead.Replace("STNNAME3", stnName3).ToString
                        StrRead = StrRead.Replace("STNPCS3", stnPcs3).ToString
                        StrRead = StrRead.Replace("STNWT3", stnWt3).ToString
                        StrRead = StrRead.Replace("STONEUNIT3", stoneUnit3).ToString
                        fileWriter.WriteLine(StrRead)
                    Else
                        Continue While
                    End If
                ElseIf StrRead.Contains("STNNAME4") Then
                    If stnName4 <> "" Then
                        StrRead = StrRead.Replace("STNNAME4", stnName4).ToString
                        StrRead = StrRead.Replace("STNPCS4", stnPcs4).ToString
                        StrRead = StrRead.Replace("STNWT4", stnWt4).ToString
                        StrRead = StrRead.Replace("STONEUNIT4", stoneUnit4).ToString
                        fileWriter.WriteLine(StrRead)
                    Else
                        Continue While
                    End If

                ElseIf StrRead.Contains("TOTSTNPCS") Then
                    If dtTagStone.Rows.Count > 4 Then
                        If TotStnWt <> "" Or TotStnPcs <> "" Then
                            StrRead = StrRead.Replace("TOTSTNPCS", TotStnPcs).ToString
                            StrRead = StrRead.Replace("TOTSTNWT", TotStnWt).ToString
                            fileWriter.WriteLine(StrRead)
                        Else
                            Continue While
                        End If
                    End If

                    '    'DIAMOND
                    'ElseIf StrRead.Contains("DIAMT") Then
                    '    If DiaAmt <> "" Then fileWriter.WriteLine(StrRead.Replace("DIAAMT", DiaAmt)) Else Continue While
                    'ElseIf StrRead.Contains("DIAWT") Then
                    '    If DiaWt <> "" Then fileWriter.WriteLine(StrRead.Replace("DIAWT", DiaWt)) Else Continue While
                    'ElseIf StrRead.Contains("DIAPCS") Then
                    '    If DiaPcs <> "" Then fileWriter.WriteLine(StrRead.Replace("DIAPCS", DiaPcs)) Else Continue While
                    'ElseIf StrRead.Contains("DIANAME") Then
                    '    If DiaName <> "" Then fileWriter.WriteLine(StrRead.Replace("DIANAME", DiaName)) Else Continue While
                    'BEEDS                
                ElseIf StrRead.Contains("BDSWT") Then
                    If BDSWt <> "" Then fileWriter.WriteLine(StrRead.Replace("BDSWT", BDSWt)) Else Continue While

                ElseIf StrRead.Contains("RATE") Then
                    If Rate <> "" Then fileWriter.WriteLine(StrRead.Replace("RATE", Rate)) Else Continue While
                Else
                    fileWriter.WriteLine(StrRead)
                End If
            End While
            fileReader.Close()
            fileWriter.Close()
            Dim objBarcodePrint As New RawPrinterHelper
            objBarcodePrint.SendFileToPrinter(barWrite)
            fileWriter.Dispose()
            My.Computer.FileSystem.DeleteFile(barWrite)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function

    Private Sub txtHallmarkNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtHallmarkNo.KeyPress
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            If HALLMARKVALID = True And NeedHallmark = True And txtHallmarkNo.Text = "" Then MsgBox("Hallmark Billno Is empty") : txtHallmarkNo.Focus() : Exit Sub
            If Val(txtHallmarkNo.Text.Length) > 0 Then
                If HallmarkMinLen > 0 Then
                    If Val(HallmarkMinLen) <> Val(txtHallmarkNo.Text.Length) Then
                        MsgBox("HallmarkNo Length Less Than Minimum Length", MsgBoxStyle.Information)
                        txtHallmarkNo.Select()
                        Exit Sub
                    End If
                End If
                Dim Htagsno As String = ""
                ''strSql = "SELECT DISTINCT TAGSNO FROM (SELECT ISNULL(SNO,'')TAGSNO FROM " & cnAdminDb & "..ITEMTAG WHERE  HM_BILLNO = '" & chkHallmarkNo & "'"
                ''strSql += " UNION ALL SELECT ISNULL(TAGSNO,'')TAGSNO FROM " & cnAdminDb & "..ITEMTAGHALLMARK WHERE  HM_BILLNO = '" & chkHallmarkNo & "') X "

                'strSql = "SELECT DISTINCT TAGSNO FROM (SELECT ISNULL(SNO,'')TAGSNO FROM " & cnAdminDb & "..ITEMTAG WHERE  HM_BILLNO = '" & txtHallmarkNo.Text & "'"
                'strSql += " AND NOT EXISTS(SELECT 1 FROM " & cnStockDb & "..ISSUE WHERE SNO IN "
                'strSql += " (SELECT ISSSNO FROM " & cnStockDb & "..ISSHALLMARK WHERE HM_BILLNO ='" & txtHallmarkNo.Text & "') AND ISNULL(CANCEL,'')<>'Y' AND TRANTYPE = 'SA') "
                'strSql += " UNION ALL SELECT ISNULL(TAGSNO,'')TAGSNO FROM " & cnAdminDb & "..ITEMTAGHALLMARK WHERE  HM_BILLNO = '" & txtHallmarkNo.Text & "' "
                'strSql += " AND NOT EXISTS(SELECT 1 FROM " & cnStockDb & "..ISSUE WHERE SNO IN "
                'strSql += " (SELECT ISSSNO FROM " & cnStockDb & "..ISSHALLMARK WHERE HM_BILLNO ='" & txtHallmarkNo.Text & "') AND ISNULL(CANCEL,'')<>'Y' AND TRANTYPE = 'SA') "
                'strSql += " ) X"

                strSql = "SELECT DISTINCT TAGSNO FROM (SELECT ISNULL(SNO,'')TAGSNO FROM " & cnAdminDb & "..ITEMTAG WHERE  HM_BILLNO = '" & txtHallmarkNo.Text & "'"
                strSql += " AND SNO NOT IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE LTRIM(ITEMID)+'-'+TAGNO IN "
                strSql += " (SELECT LTRIM(ITEMID)+'-'+TAGNO FROM " & cnStockDb & "..ISSUE WHERE SNO IN "
                strSql += " (SELECT ISSSNO FROM " & cnStockDb & "..ISSHALLMARK WHERE HM_BILLNO ='" & txtHallmarkNo.Text & "') AND ISNULL(CANCEL,'')<>'Y' AND TRANTYPE='SA')) "
                strSql += " UNION ALL "
                strSql += " SELECT ISNULL(TAGSNO,'')TAGSNO FROM " & cnAdminDb & "..ITEMTAGHALLMARK WHERE  HM_BILLNO = '" & txtHallmarkNo.Text & "' "
                strSql += " AND TAGSNO NOT IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE LTRIM(ITEMID)+'-'+TAGNO IN "
                strSql += " (SELECT LTRIM(ITEMID)+'-'+TAGNO FROM " & cnStockDb & "..ISSUE WHERE SNO IN "
                strSql += " (SELECT ISSSNO FROM " & cnStockDb & "..ISSHALLMARK WHERE HM_BILLNO ='" & txtHallmarkNo.Text & "') AND ISNULL(CANCEL,'')<>'Y' AND TRANTYPE='SA')) "
                strSql += " ) X"

                Htagsno = GetSqlValue(cn, strSql)
                If Not Htagsno Is Nothing Then
                    Dim Htagrow As DataRow
                    Htagrow = Nothing
                    strSql = " SELECT ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID=T.COSTID),'') COSTNAME "
                    strSql += " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID)ITEMNAME "
                    strSql += " ,ITEMID,TAGNO,CONVERT(VARCHAR(12),RECDATE,103)RECDATE "
                    strSql += " FROM " & cnAdminDb & "..ITEMTAG T WHERE "
                    strSql += " SNO='" & Htagsno.ToString & "'"
                    strSql += " AND NOT EXISTS(SELECT 1 FROM " & cnStockDb & "..ISSHALLMARK WHERE HM_BILLNO IN("
                    strSql += " SELECT DISTINCT HM_BILLNO FROM " & cnAdminDb & "..ITEMTAGHALLMARK WHERE TAGSNO IN ('" & Htagsno.ToString & "')))"
                    If tagEdit Then
                        strSql += " AND T.SNO<>'" & updIssSno & "'"
                    End If
                    Htagrow = GetSqlRow(strSql, cn, Nothing)
                    If Not Htagrow Is Nothing Then
                        MsgBox("HallmarkNo Already Exist" _
                        & IIf(Htagrow!Costname.ToString <> "", vbCrLf & " Branch : " & Htagrow!Costname.ToString, "") _
                        & vbCrLf & " Itemname : " & Htagrow!ITEMNAME.ToString & vbCrLf & " Recdate : " & Htagrow!RECDATE.ToString _
                        & vbCrLf & " Itemid : " & Htagrow!ITEMID.ToString & vbCrLf & " Tagno : " & Htagrow!TAGNO.ToString _
                        , MsgBoxStyle.Information)
                        txtHallmarkNo.Select()
                        Exit Sub
                    End If
                End If
            End If

            If Val(txtHallmarkNo.Text.Length) > 0 Then
                If Not Val(txtTagWt_WET.Text) > 0 And Val(txtGrossWt_Wet.Text) > 0 Then
                    MsgBox(Me.GetNextControl(txtTagWt_WET, False).Text + E0001, MsgBoxStyle.Information)
                    txtTagWt_WET.Select()
                    Exit Sub
                End If


                Dim CHKro() As DataRow
                CHKro = Nothing
                If dtHallmarkDetails.Rows.Count > 0 Then
                    If Val(txtTagWt_WET.Text) > 0 And Val(txtGrossWt_Wet.Text) > 0 Then
                        Dim chkwt As Decimal = 0
                        If txthallmarkRowIndex.Text = "" Then
                            chkwt = Val(dtHallmarkDetails.Compute("SUM(GRSWT)", Nothing)) + Val(txtTagWt_WET.Text)
                        Else
                            chkwt = Val(dtHallmarkDetails.Compute("SUM(GRSWT)", "KEYNO <> '" & gridHallmarkDetails.Rows(Val(txthallmarkRowIndex.Text)).Cells("KEYNO").Value & "'").ToString) + Val(txtTagWt_WET.Text)
                        End If
                        ''chkwt = Val(dtHallmarkDetails.Compute("SUM(GRSWT)", "KEYNO <> '" & gridHallmarkDetails.Rows(Val(txthallmarkRowIndex.Text)).Cells("KEYNO").Value & "'").ToString) + Val(txtTagWt_WET.Text)
                        If Val(chkwt) > Val(txtGrossWt_Wet.Text) Then
                            MsgBox("TagWeight Should Not Exceed GrossWeight", MsgBoxStyle.Information)
                            txtTagWt_WET.Clear()
                            txtHallmarkNo.Clear()
                            txtTagWt_WET.Focus()
                            Exit Sub
                        End If
                    ElseIf Val(txtPieces_Num_Man.Text) > 0 Then
                        Dim chkpcs As Integer = 0
                        If txthallmarkRowIndex.Text = "" Then
                            chkpcs = Val(dtHallmarkDetails.Compute("SUM(PCS)", Nothing)) + 1
                        Else
                            chkpcs = Val(dtHallmarkDetails.Compute("SUM(PCS)", "KEYNO <> '" & gridHallmarkDetails.Rows(Val(txthallmarkRowIndex.Text)).Cells("KEYNO").Value & "'").ToString) + 1
                        End If
                        'chkpcs = Val(dtHallmarkDetails.Compute("SUM(PCS)", "KEYNO <> '" & gridHallmarkDetails.Rows(Val(txthallmarkRowIndex.Text)).Cells("KEYNO").Value & "'").ToString) + 1
                        If Val(chkpcs) > Val(txtPieces_Num_Man.Text) And HALLMARKVALID Then
                            MsgBox("Pcs Should Not Exceed TagPcs", MsgBoxStyle.Information)
                            txtHallmarkNo.Clear()
                            txtHallmarkNo.Focus()
                            Exit Sub
                        End If
                    End If

                    If txthallmarkRowIndex.Text <> "" Then
                        With gridHallmarkDetails.Rows(Val(txthallmarkRowIndex.Text))
                            .Cells("GRSWT").Value = txtTagWt_WET.Text
                            .Cells("HM_BILLNO").Value = txtHallmarkNo.Text
                            dtHallmarkDetails.AcceptChanges()
                            GoTo AFTERINSERT
                        End With
                    End If

                    Dim dtchk As DataTable
                    dtchk = New DataTable
                    dtchk = dtHallmarkDetails.Copy
                    Dim chkhmno As String
                    chkhmno = "HM_BILLNO='" & txtHallmarkNo.Text & "'"
                    CHKro = dtchk.Select(chkhmno, Nothing)

                    If CHKro.Length = 0 Then
                        Dim ro As DataRow = Nothing
                        ro = dtHallmarkDetails.NewRow
                        ro("PCS") = 1
                        ro("GRSWT") = IIf(Val(txtTagWt_WET.Text) <> 0, Val(txtTagWt_WET.Text), DBNull.Value)
                        ro("HM_BILLNO") = txtHallmarkNo.Text
                        dtHallmarkDetails.Rows.Add(ro)
                        dtHallmarkDetails.AcceptChanges()
                    ElseIf CHKro Is Nothing Then
                        Dim ro As DataRow = Nothing
                        ro = dtHallmarkDetails.NewRow
                        ro("PCS") = 1
                        ro("GRSWT") = IIf(Val(txtTagWt_WET.Text) <> 0, Val(txtTagWt_WET.Text), DBNull.Value)
                        ro("HM_BILLNO") = txtHallmarkNo.Text
                        dtHallmarkDetails.Rows.Add(ro)
                        dtHallmarkDetails.AcceptChanges()
                    Else
                        MsgBox("Hallmark No Already In Grid", MsgBoxStyle.Information)
                        txtTagWt_WET.SelectAll()
                        Exit Sub
                    End If
                Else
                    Dim ro As DataRow = Nothing
                    ro = dtHallmarkDetails.NewRow
                    ro("PCS") = 1
                    ro("GRSWT") = IIf(Val(txtTagWt_WET.Text) <> 0, Val(txtTagWt_WET.Text), DBNull.Value)
                    ro("HM_BILLNO") = txtHallmarkNo.Text
                    dtHallmarkDetails.Rows.Add(ro)
                    dtHallmarkDetails.AcceptChanges()
                End If
AFTERINSERT:
                If txthallmarkRowIndex.Text <> "" Then
                    gridHallmarkDetails.CurrentCell = gridHallmarkDetails.Rows(gridHallmarkDetails.RowCount - 1).Cells(0)
                End If

                txtHallmarkNo.Clear()
                txthallmarkRowIndex.Clear()
                txtTagWt_WET.Clear()
                lblTagWt.Select()
            End If
        End If
    End Sub
    Private Sub txtTagWt_WET_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtTagWt_WET.KeyPress
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            If Not Val(txtTagWt_WET.Text) > 0 And Val(txtGrossWt_Wet.Text) > 0 Then
                MsgBox(Me.GetNextControl(txtTagWt_WET, False).Text + E0001, MsgBoxStyle.Information)
                txtTagWt_WET.Select()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub gridHallmarkDetails_UserDeletedRow(sender As Object, e As DataGridViewRowEventArgs) Handles gridHallmarkDetails.UserDeletedRow
        dtHallmarkDetails.AcceptChanges()
        If Not gridHallmarkDetails.RowCount > 0 Then
            txtTagWt_WET.Focus()
        End If
    End Sub

    Private Sub cmbStUnit_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStUnit.GotFocus
        Dim tempitemid As Integer = Val(objGPack.GetSqlValue("SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') = '" & cmbItem_MAN.Text & "'").ToString)
        Dim tempSitemid As Integer = 0
        If cmbSubItem_Man.Text <> "" Then
            tempSitemid = objGPack.GetSqlValue("SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE ISNULL(SUBITEMNAME,'') = '" & cmbSubItem_Man.Text & "'")
        End If
        Dim dtdesgstnrow As DataRow
        Dim tempDesignerid As Integer
        If cmbDesigner_MAN.Text <> "" Then tempDesignerid = objGPack.GetSqlValue("SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE ISNULL(DESIGNERNAME,'') = '" & cmbDesigner_MAN.Text & "'")
        dtdesgstnrow = GetSqlRow("SELECT STNDEDPER,STN_RATE,STNITEMID,STNSUBITEMID,CALTYPE,STONEUNIT FROM " & cnAdminDb & "..DESIGNERSTONE WHERE ACTIVE <> 'N' AND DESIGNERID = " & tempDesignerid & " AND ITEMID = " & tempitemid & " AND SUBITEMID = " & tempSitemid, cn)
        If dtdesgstnrow Is Nothing Then
            dtdesgstnrow = GetSqlRow("SELECT STNDEDPER,STN_RATE,STNITEMID,STNSUBITEMID,CALTYPE,STONEUNIT FROM " & cnAdminDb & "..DESIGNERSTONE WHERE ACTIVE <> 'N' AND DESIGNERID = 0 AND ITEMID = " & tempitemid & " AND SUBITEMID = " & tempSitemid, cn)
        Else
            cmbStUnit.Text = dtdesgstnrow(5).ToString
            cmbStCalc.Text = dtdesgstnrow(4).ToString
        End If
    End Sub

    Private Sub txtRate_Amt_Leave(sender As Object, e As EventArgs) Handles txtRate_Amt.Leave

    End Sub

    Private Sub gridHallmarkDetails_KeyPress(sender As Object, e As KeyPressEventArgs) Handles gridHallmarkDetails.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            txthallmarkRowIndex.Text = ""
            gridHallmarkDetails.CurrentCell = gridHallmarkDetails.Rows(gridHallmarkDetails.CurrentRow.Index).Cells(0)
            With gridHallmarkDetails.Rows(gridHallmarkDetails.CurrentRow.Index)
                txtTagWt_WET.Text = .Cells("GRSWT").FormattedValue
                txtHallmarkNo.Text = .Cells("HM_BILLNO").FormattedValue
                txthallmarkRowIndex.Text = gridHallmarkDetails.CurrentRow.Index
                lblTagWt.Select()
            End With
        End If
    End Sub

    Private Sub txtMaxWastage_Wet_Leave(sender As Object, e As EventArgs) Handles txtMaxWastage_Wet.Leave, txtMaxWastage_Per.Leave
        If tagEdit Then
            strSql = " SELECT MAXWAST FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO='" & txtTagNo__Man.Text.ToString & "'"
            Dim MaxWast As Double = Val(objGPack.GetSqlValue(strSql).ToString)
            strSql = " SELECT MAXWASTPER FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO='" & txtTagNo__Man.Text.ToString & "'"
            Dim MAXWASTPER As Double = Val(objGPack.GetSqlValue(strSql).ToString)
            If MaxWast > Val(txtMaxWastage_Wet.Text.ToString) And tagwastEditotp = False Then
                Dim strmsg As String = "Pls Share the OTP <OTP> for TAGEDIT_WASTAGE.ITEM: " & cmbItem_MAN.Text.ToString & ",OldWast:" & Val(MAXWASTPER.ToString) & "%,NewWast:" & Val(txtMaxWastage_Per.Text.ToString) & "%,Tagno:" & txtTagNo__Man.Text.ToString & ",TagWt:" & txtGrossWt_Wet.Text.ToString & ". Initiated by " + cnUserName.ToString + ",BranchName:" & cnCostName & "," + cnCompanyName.ToString.Trim & "."
                If Not usrpwdok("TAGEDIT_WASTAGE", True, strmsg) Then
                    txtMaxWastage_Wet.Text = MaxWast.ToString
                    txtMaxWastage_Per.Text = MAXWASTPER.ToString
                    MsgBox("OTP Required...", MsgBoxStyle.Information)
                    Exit Sub
                Else
                    tagwastEditotp = True
                End If
            End If
        End If
        CalcFinalTotal()
    End Sub

    Private Sub cmbItem_MAN_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmbItem_MAN.SelectionChangeCommitted
        If cmbItem_MAN.Text <> "" Then
            strSql = "SELECT ISNULL(HSN,'')HSN FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME ='" & cmbItem_MAN.Text & "'"
            txtHSN.Text = objGPack.GetSqlValue(strSql).ToString
        End If
    End Sub

    Private Sub txtHmBillNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txtHmBillNo.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridHallmarkDetails.RowCount > 0 Then gridHallmarkDetails.Focus()
        End If
    End Sub



    Private Sub txtTagWt_WET_KeyDown(sender As Object, e As KeyEventArgs) Handles txtTagWt_WET.KeyDown
        If e.KeyCode = Keys.Down Then
            If gridHallmarkDetails.RowCount > 0 Then gridHallmarkDetails.Focus()
        End If
    End Sub

    Private Sub CmbSetItem_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CmbSetItem.SelectedIndexChanged
        If Not NeedTag_SetItem Then Exit Sub
        If CmbSetItem.Text = "YES" Then
            txtSetTagno.Enabled = True
        Else
            txtSetTagno.Enabled = False
        End If
    End Sub

    Private Sub cmbCalType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCalType.SelectedIndexChanged
        If Not NeedTag_CalcType Then Exit Sub
        If cmbCalType.Text <> "" And cmbCalType.Enabled = True Then
            calType = Mid(cmbCalType.Text, 1, 1)
            funcAssignTabControls()
        End If
    End Sub


    Private Sub gridHallmarkDetails_KeyDown(sender As Object, e As KeyEventArgs) Handles gridHallmarkDetails.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtStyleCode_LostFocus(sender As Object, e As EventArgs) Handles txtStyleCode.LostFocus
        If isStyleCode = True And txtStyleCode.Text.Trim <> "" And COPYIMAGEFROMCATALOGPATH = True Then
            strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'ATTACHIMAGE'"
            If UCase(objGPack.GetSqlValue(strSql, , , tran)) = "Y" Then
                Dim _stylenoPctFilename As String = ""
                strSql = " SELECT ISNULL(PCTFILE,'')PCTFILE FROM " & cnAdminDb & "..TAGCATALOG WHERE STYLENO = '" & txtStyleCode.Text & "'"
                _stylenoPctFilename = objGPack.GetSqlValue(strSql, "PCTFILE", "", tran).ToString
                If CatalogDestination.ToString <> "" And _stylenoPctFilename.ToString <> "" Then
                    Try
                        SubItemPic = False
                        Dim Finfo As FileInfo
                        Finfo = New FileInfo(CatalogDestination.ToString & _stylenoPctFilename.ToString)
                        AutoImageSizer(CatalogDestination.ToString & _stylenoPctFilename.ToString, picModel)
                        picPath = CatalogDestination.ToString & _stylenoPctFilename.ToString
                        picExtension = Finfo.Extension

                        btnAttachImage.Enabled = False
                    Catch ex As Exception
                        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'ATTACHIMAGE'"
                        If UCase(objGPack.GetSqlValue(strSql, , , tran)) = "Y" Then
                            btnAttachImage.Enabled = True
                        Else
                            btnAttachImage.Enabled = False
                        End If
                    End Try
                Else
                    strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'ATTACHIMAGE'"
                    If UCase(objGPack.GetSqlValue(strSql, , , tran)) = "Y" Then
                        btnAttachImage.Enabled = True
                    Else
                        btnAttachImage.Enabled = False
                    End If
                End If
            End If
        Else
            strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'ATTACHIMAGE'"
            If UCase(objGPack.GetSqlValue(strSql, , , tran)) = "Y" Then
                btnAttachImage.Enabled = True
            Else
                btnAttachImage.Enabled = False
            End If
        End If
    End Sub

    Private Sub chkOldTagRecdate_CheckedChanged(sender As Object, e As EventArgs) Handles chkOldTagRecdate.CheckedChanged
        If chkOldTagRecdate.Checked Then
            dtpOldTagRecDate_OWN.Enabled = True
        Else
            dtpOldTagRecDate_OWN.Enabled = False
        End If
    End Sub

    Private Sub txtHSN_KeyDown(sender As Object, e As KeyEventArgs) Handles txtHSN.KeyDown
        If e.KeyCode = Keys.Enter Then
            If txtHSN.Text <> "" And NeedTag_Hsncode Then
                strSql = " SELECT COUNT(*)CNT FROM " & cnAdminDb & "..HSNMASTER WHERE HSNCODE = '" & txtHSN.Text & "'"
                If Val(GetSqlValue(cn, strSql).ToString) <= 0 Then
                    MsgBox("Invalid Hsncode", MsgBoxStyle.Information)
                    txtHSN.SelectAll()
                End If
            End If
        End If
    End Sub

    Private Sub txtWorkOrderNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txtWorkOrderNo.KeyDown
        If e.KeyCode = Keys.Enter Then
            If _LEntryType.ToString = "WO" Then
                CheckWorkorder()
            End If
        End If
    End Sub

    Private Function CheckWorkorder() As Boolean
        If txtWorkOrderNo.Text = "" Then
            MsgBox("Work OrderNo Should Not Be Empty", MsgBoxStyle.Information)
            txtWorkOrderNo.Select()
            Return False
        End If
        strSql = "SELECT COUNT(*) CNT FROM " & cnStockDb & "..WORKORDER WHERE ORDERNO ='" & txtWorkOrderNo.Text & "' AND ISNULL(CANCEL,'')<>'Y'"
        If Val(objGPack.GetSqlValue(strSql, "", 0).ToString) = 0 Then
            MsgBox("Invalid Work OrderNo ", MsgBoxStyle.Information)
            txtWorkOrderNo.Select()
            Return False
        End If
        Return True
    End Function
End Class
