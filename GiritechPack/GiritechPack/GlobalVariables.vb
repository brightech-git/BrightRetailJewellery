Imports System.Data.OleDb
Public Module GlobalVariables
    Public G_SearchDialogColAutoFit As Boolean
    Public G_Cn As OleDbConnection
    Public G_CnAdmindb As String = Nothing
    Public G_LangId As String = Nothing
    Public G_DTable As DataTable
    Public G_Cmd As OleDbCommand
    Public G_DAdapter As OleDbDataAdapter
    Public G_Tran As OleDbTransaction = Nothing

    Public G_TextCharacterCasing As CharacterCasing = CharacterCasing.Upper
    Public G_CnTranFromDate As Date
    Public G_CnTranToDate As Date

    Public G_FocusColor As Color = Color.LightGreen
    Public G_LostFocusColor As Color = SystemColors.Window
    Public G_Radio_Check_LostFocusColor As Color = Color.Transparent
    Public G_Button_LostFocusColor As Color = Color.FromKnownColor(KnownColor.Control)
    Public G_BakImage As Image = Nothing
    Public G_GrdBackGroundColor As Color = SystemColors.AppWorkspace
    Public G_Decimal As Integer = 2

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
End Module
