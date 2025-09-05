Imports System.Data.OleDb
Public Class frmOtherMast
    Dim strSql As String
    Dim dt As DataTable
    Dim TagMiscName As String = ""
    Dim TagMiscVal As String = ""
    Dim TagSno As String = ""
    Dim TagItemid As Integer
    Dim TagDate As Date
    Dim TagNo As String = ""
    Dim TagCostId As String = ""
    Dim Cmd As New OleDbCommand
    Dim Updateflag As Boolean
    Public Sub New()
        InitializeComponent()
        objGPack.Validator_Object(Me)
    End Sub
    Public Sub New(ByVal Sno As String, ByVal No As String, ByVal MiscName As String, ByVal Miscval As String, ByVal Uflag As Boolean)
        InitializeComponent()
        TagSno = Sno
        TagNo = No
        TagMiscName = MiscName
        TagMiscVal = Miscval
        Updateflag = Uflag
    End Sub
    Public Sub New(ByVal Sno As String, ByVal No As String, ByVal Id As Integer, ByVal Tdate As Date, ByVal Costid As String, ByVal Uflag As Boolean)
        InitializeComponent()
        TagSno = Sno
        TagNo = No
        TagItemid = Id
        TagDate = Tdate
        TagCostId = Costid
        Updateflag = Uflag
    End Sub
    Private Sub frmAccLedgerMore_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        strSql = " SELECT MISCNAME,MISCID,1 RESULT FROM " & cnAdminDb & "..OTHERMASTERENTRY  WHERE ISNULL(ACTIVE,'N')<>'N'"
        strSql += " ORDER BY MISCID"
        Dim dtMetal = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtMetal)
        BrighttechPack.GlobalMethods.FillCombo(CmbOtherName, dtMetal, "MISCNAME")
        If Updateflag Then CmbOtherName.Text = TagMiscName
    End Sub

    Private Sub frmcheckothermaster_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub
    Private Sub CmbOtherVal_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles CmbOtherVal.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If CmbOtherVal.Text <> "" And CmbOtherName.Text <> "" Then
                If Updateflag Then
                    strSql = " UPDATE " & cnAdminDb & "..ADDINFOITEMTAG"
                    strSql += " SET OTHID=" & GetSqlValue(cn, "SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE NAME='" & CmbOtherVal.Text & "'")
                    strSql += " ,USERID=" & userId
                    strSql += " WHERE SNO='" & TagSno & "'"
                    Cmd = New OleDbCommand(strSql, cn, tran) : Cmd.ExecuteNonQuery()
                Else
                    strSql = " INSERT INTO " & cnAdminDb & "..ADDINFOITEMTAG("
                    strSql += " SNO,TAGSNO,OTHID,ITEMID,TAGNO,RECDATE,COMPANYID"
                    strSql += " ,COSTID,SYSTEMID,APPVER,USERID"
                    strSql += " )VALUES("
                    strSql += " '" & GetNewSno(TranSnoType.ADDINFOITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") & "'" 'SNO  
                    strSql += " ,'" & TagSno & "'" 'TAGSNO
                    strSql += " ,'" & GetSqlValue(cn, "SELECT ID FROM " & cnAdminDb & "..OTHERMASTER WHERE NAME='" & CmbOtherVal.Text & "'") & "'" 'OTHID
                    strSql += " ,'" & TagItemid & "'" 'ITEMID
                    strSql += " ,'" & TagNo & "'" 'TAGNO
                    strSql += " ,'" & TagDate.ToString("yyyy-MM-dd") & "'" 'RECDATE
                    strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
                    strSql += " ,'" & TagCostId & "'" 'COSTID
                    strSql += " ,'" & systemId & "'" 'SYSTEMID
                    strSql += " ,'" & VERSION & "'" 'APPVER
                    strSql += " ,'" & userId & "'" 'USERID
                    strSql += " )"
                    Cmd = New OleDbCommand(strSql, cn, tran) : Cmd.ExecuteNonQuery()
                End If
            End If
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub CmbOtherName_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmbOtherName.Leave
        If CmbOtherName.Text <> "" Then
            strSql = " SELECT NAME,ID,1 RESULT FROM " & cnAdminDb & "..OTHERMASTER WHERE ISNULL(ACTIVE,'N')<>'N' "
            If CmbOtherName.Text <> "ALL" And CmbOtherName.Text <> "" Then
                strSql += "AND MISCID IN (SELECT MISCID FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE MISCNAME = '" & CmbOtherName.Text & "')"
            End If
            strSql += " ORDER BY ID"
            Dim dtMetal = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtMetal)
            BrighttechPack.GlobalMethods.FillCombo(CmbOtherVal, dtMetal, "NAME")
            If Updateflag Then CmbOtherVal.Text = TagMiscVal
        End If
    End Sub
End Class