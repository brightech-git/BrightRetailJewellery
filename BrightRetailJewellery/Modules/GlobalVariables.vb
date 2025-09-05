Imports System.Data.OleDb
Module GlobalVariables
    ''' <summary> 
    ''' A Stock
    ''' C Estimation
    ''' B Bill
    ''' D Order & Repair
    ''' E Accounts
    ''' F Store Management
    ''' G Savings Scheme
    ''' </summary>
    ''' <remarks></remarks>
    Public _MaxDate As Date = "2030-03-31"
    Public _CnAdmin As OleDbConnection
    Public _UserLimit As Integer
    Public _AppId As Integer
    Public _DtTransactionYear As New DataTable
    Public _Demo As Boolean = False
    Public _DemoExpiredDate As Date
    Public _DemoLImitDays As Integer = 30
    Public ConInfo As BrighttechPack.Coninfo
    Public _DtUserRights As New DataTable
    Public _DefaultPic As String
    Public objGPack As BrighttechPack.Methods
    Public LangId As String = "ENG"
    Public cnDataSource As String
    Public _PrjModules As New List(Of String)
    Public _IsWholeSaleType As Boolean = False
    Public _IsCostCentre As Boolean = False
    Public _HideBackOffice As Boolean = False
    Public _CounterWiseCashMaintanance As Boolean = False
    Public _AsOnFromDate As Date = "1950-04-01"
    Public _SubItemOrderByName As Boolean = True
    Public _MainCompId As String = Nothing
    Public _SyncTo As String = Nothing
    Public _CashOpening As Boolean
    Public _HasCostcentre As Boolean = False
    Public PIC_ITEMWISE As Boolean = False
    Public PICPATH As String = ""
    Public WTAMTOPT As Boolean = False
    Public cnSuffix As String
    Public cnCompanyId As String
    Public cnChitCompanyid As String
    Public cnCompanyName As String = ""
    Public userId As Integer
    Public IpAddress As String = ""
    Public HostName As String = ""
    Public cnUserName = ""
    Public cnPassword = ""
    Public cnCostName As String
    Public cnAdminDb As String
    Public tempdbname As String = "TEMPTABLEDB"
    Public cn As New OleDbConnection
    Public tran As OleDbTransaction = Nothing
    Public CurrencyDecimal As Integer = 2
    Public cnShortCutKey = ""
    Public EXE_WITH_PARAM As Boolean = False
    Public COSTCENTRE_SINGLE As Boolean = False
    Public cnCentStock As Boolean = True
    Public DiaRnd As Integer = 3
    Public Estmationviewinown As Boolean = False

    Public _Reportbydll As String

    ''NEW
    Public cnTranFromDate As Date = Nothing
    Public cnTranToDate As Date = Nothing
    Public systemId As String
    Public cnStockDb As String
    Public findStr As String = Nothing
    Public da As OleDbDataAdapter
    Public cnCostId As String = Nothing
    Public VERSION As String
    Public cnHOCostId As String
    Public cnDefaultCostId As Boolean = False
    Public RATE_BRANCHWISE As Boolean = False
    Public ISDISPLAY_92 As Boolean = False

    ''Focus and Unfocus Colors
    Public frmBackColor As Color = Color.Silver
    ''SystemColors.Control
    '' Color.WhiteSmoke

    Public bakImage As Image = Nothing
    Public grdBackGroundColor As Color = SystemColors.AppWorkspace
    Public focusColor As Color = Color.LightGreen
    Public lostFocusColor As Color = SystemColors.Window
    Public Radio_Check_LostFocusColor As Color = Color.Transparent
    Public Button_LostFocusColor As Color = Color.FromKnownColor(KnownColor.Control)
    Public imgList As New ImageList

    Public textCharacterCasing As CharacterCasing = CharacterCasing.Upper

    Public BarcodeTagNo As String
    Public BarcodeSno As String
    Public BarcodeDescrip As String

    Public ScreenWid As Integer = Screen.PrimaryScreen.Bounds.Width
    Public ScreenHit As Integer = Screen.PrimaryScreen.Bounds.Height
    Public StrChitAdminDB As String = Nothing
    Public StrChitTranDB As String = Nothing
    Public StrChitCompanyID As String = Nothing

    ''Message Caption Code's
    Public E0001 As String = " Should Not Empty "
    Public E0002 As String = " Already Exist "
    Public E0003 As String = " Digits Only Allowed "
    Public E0004 As String = " Invalid "
    Public E0005 As String = " Enter Valid Range "
    Public E0006 As String = " Range With in "
    Public E0007 As String = " Enter Valid Rate "
    Public E0008 As String = " Successfully Saved.."
    Public E0009 As String = " Successfully Updated.."
    Public E0010 As String = " Should Not Exceed "
    Public E0011 As String = " Record Not Found "
    Public E0012 As String = " Generated.."
    Public E0013 As String = " Non Tag Item "
    Public E0014 As String = " Packet Based Item "
    Public E0015 As String = " With in "
    Public E0016 As String = " Invalid Format "
    Public E0017 As String = " Purchase Value Exceed "
    Public E0018 As String = " Completed "
    Public E0019 As String = " Do you wish to Continue? "
    Public E0020 As String = " Min Value "
    Public E0021 As String = " This Tag Already Loaded in Sales Grid"
    Public E0022 As String = " Not Found "
    Public E0023 As String = " Please Check the Entered Rate"
    Public E0024 As String = " Lot unable to complete due to invalid weight(+/-) "
    Public E0025 As String = " Unhandled exception has occurred in a component in your application,possible of Data corruption"
    ''RAAJ VAR
    Public reportHeadStyle As New DataGridViewCellStyle
    Public reportSubTotalStyle As New DataGridViewCellStyle
    Public reportHeadStyle1 As New DataGridViewCellStyle
    Public reportSubTotalStyle1 As New DataGridViewCellStyle
    Public reportHeadStyle2 As New DataGridViewCellStyle
    Public reportSubTotalStyle2 As New DataGridViewCellStyle
    Public reportTotalStyle As New DataGridViewCellStyle
    Public reportHigLightStyle As New DataGridViewCellStyle
    Public reportColumnHeadStyle As New DataGridViewCellStyle
    Public reportGSTStyle As New DataGridViewCellStyle
    Public reportGSTHeadStyle As New DataGridViewCellStyle

    Public cnChitTrandb As String
    Public strCompanyId As String = Nothing
    Public strBCostid As String = Nothing
    Public strUserCentrailsed As String = Nothing
    Public strCompanyName As String = Nothing
    Public strCompanyGst As String = Nothing
    Public dtSoftKeys As DataTable = Nothing
    Public Ogstnrateedit As Boolean = False
    Public ALERTBASE_MENU As Boolean = True
    Public _IsAdmin As Boolean = False
    Public GEN_SKUFILE As Boolean = False
    Public SKUFILEPATH As String = ""
    Public SMSURL As String
    Public CENTR_DB_BR As Boolean = False
    Public DemoLogin As Boolean = False
    Public GST As Boolean = False
    Public SRCESS As Boolean = False
    Public GstDate As Date
    Public CompanyStateId As Integer = 0
    Public CompanyState As String = ""
End Module
