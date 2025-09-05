Imports System.Data.OleDb
Public Module Globalvariables

    Public cnSuffix As String
    Public cnCompanyId As String
    Public cnChitCompanyid As String
    Public cnCompanyName As String
    Public userId As Integer
    Public cnUserName
    Public cnPassword
    Public cnCostName As String
    Public cnAdminDb As String
    Public tempdbname As String = "TEMPTABLEDB"
    Public cn As New OleDbConnection
    Public tran As OleDbTransaction = Nothing
    Public _CashOpening As Boolean
    Public _DtUserRights As DataTable
    Public ScreenWid As Integer = Screen.PrimaryScreen.Bounds.Width
    Public ScreenHit As Integer = Screen.PrimaryScreen.Bounds.Height
    Public ConInfo As BrighttechPack.Coninfo

    ''NEW
    Public cnTranFromDate As Date
    Public cnTranToDate As Date
    Public systemId As String
    Public cnStockDb As String
    Public FormUserRights As String
    Public findStr As String = Nothing
    Public da As OleDbDataAdapter
    Public cnCostId As String
    Public VERSION As String
    Public cnHOCostId As String
    Public cnChitTrandb As String
    Public strCompanyName As String
    Public strDesignerName As String
    Public strCompanyId As String
    Public strBCostid As String = Nothing
    Public strUserCentrailsed As String = Nothing
    Public cnDefaultCostId As Boolean = False
    Public _IsCostCentre As Boolean = False
    Public RATE_BRANCHWISE As Boolean = False

    ''vinoth
    Public reportHeadStyle As New DataGridViewCellStyle
    Public reportSubTotalStyle As New DataGridViewCellStyle
    Public reportHeadStyle1 As New DataGridViewCellStyle
    Public reportSubTotalStyle1 As New DataGridViewCellStyle
    Public reportHeadStyle2 As New DataGridViewCellStyle
    Public reportSubTotalStyle2 As New DataGridViewCellStyle
    Public reportTotalStyle As New DataGridViewCellStyle
    Public reportColumnHeadStyle As New DataGridViewCellStyle
    Public reportHigLightStyle As New DataGridViewCellStyle
    Public reportGSTStyle As New DataGridViewCellStyle
    Public reportGSTHeadStyle As New DataGridViewCellStyle
    Public EXE_WITH_PARAM As Boolean = False
    Public grdBackGroundColor As Color = SystemColors.AppWorkspace
    Public cnShortCutKey = ""
    Public _AppId As Integer
    Public frmBackColor As Color = Color.Silver
    Public bakImage As Image = Nothing
    Public LangId As String = "ENG"
    Public cnCentStock As Boolean = True
    Public _DefaultPic As String
    Public _Demo As Boolean = False
    Public PICPATH As String = ""
    Public PIC_ITEMWISE As Boolean = False
    Public WTAMTOPT As Boolean = False
    Public _SyncTo As String = Nothing
    Public _HasCostcentre As Boolean = False
    Public COSTCENTRE_SINGLE As Boolean = False
    Public DiaRnd As Integer = 3
    Public _MainCompId As String = Nothing
    Public _IsWholeSaleType As Boolean = False
    Public _HideBackOffice As Boolean
    Public _CounterWiseCashMaintanance As Boolean = False
    Public _SubItemOrderByName As Boolean = True
    Public _DemoLImitDays As Integer = 30
    Public _DemoExpiredDate As Date
    Public objGPack As New BrighttechPack.Methods
    Public cnDataSource As String
    Public _MaxDate As Date = "2030-03-31"
    Public _AsOnFromDate As Date = "1950-04-01"
    Public textCharacterCasing As CharacterCasing = CharacterCasing.Upper
    Public frmGridDispDia As String
    Public Estmationviewinown As Boolean = False
    Public _DtTransactionYear As New DataTable

    ''public AkshayaRetailJewellery as 
    ''Message Caption Code
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
    Public GST As Boolean = False
    Public GstDate As Date
    Public CompanyState As String = ""
    Public CompanyStateId As Integer = 0
End Module
