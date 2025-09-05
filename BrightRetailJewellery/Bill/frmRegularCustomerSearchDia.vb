Imports System.Data.OleDb
Public Class frmRegularCustomerSearchDia
    Dim strSql As String
    Public sno As String = "-1"
    Dim cmd As OleDbCommand
    Public STRSQL1 As String = ""
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        strSql = " SELECT NAME"
        strSql += " FROM " & cnAdminDb & "..SYSCOLUMNS WHERE ID = (SELECT ID FROM " & cnAdminDb & "..SYSOBJECTS WHERE NAME = 'PERSONALINFO')"
        strSql += " AND NAME IN ('ACCODE','ADDRESS1','ADDRESS2','ADDRESS3','APPVER','AREA','CITY','COSTID','COUNTRY','DOORNO','EMAIL','FAX','MOBILE','PHONERES','PINCODE','PNAME','PREVILEGEID','STATE','RELIGION','PAN','GSTNO')"
        strSql += " ORDER BY NAME"
        objGPack.FillCombo(strSql, cmbSearchKey, , False)
    End Sub

    Private Sub frmRegularCustomerSearchDia_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If sno <> "-1" Then Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub frmRegularCustomerSearchDia_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtSearchString.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmRegularCustomerSearchDia_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        cmbSearchKey.Text = "PNAME"
        sno = "-1"
    End Sub

    Private Sub txtSearchString_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSearchString.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtSearchString.Text = "" Then
                MsgBox("Please enter search string", MsgBoxStyle.Information)
                Exit Sub
            End If
            strSql = " SELECT SNO,PREVILEGEID,ACCODE,PNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3,AREA,PINCODE,CITY,COUNTRY,PHONERES,MOBILE,PAN,GSTNO,RELIGION,EMAIL"
            strSql += " FROM " & cnAdminDb & "..PERSONALINFO WHERE COMPANYID='" & strCompanyId & "'"
            'Alter by vasanth- filter companyid
            If rbtExact.Checked Then
                strSql += " AND " & cmbSearchKey.Text & " = '" & txtSearchString.Text & "'"
            Else
                strSql += " AND " & cmbSearchKey.Text & " LIKE '%" & txtSearchString.Text & "%'"
            End If
            sno = BrighttechPack.SearchDialog.Show("Select Customer", strSql, cn)

            STRSQL1 = " IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP_SNO') > 0"
            STRSQL1 += vbCrLf + "  DROP TABLE TEMPTABLEDB..TEMP_SNO"
            STRSQL1 += vbCrLf + "  CREATE TABLE TEMPTABLEDB..TEMP_SNO (SNO VARCHAR(15))"
            STRSQL1 += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP_SNO "
            STRSQL1 += vbCrLf + "  SELECT '" & sno & "'SNO"
            cmd = New OleDbCommand(STRSQL1, cn)
            cmd.ExecuteNonQuery()

            If sno <> "-1" Then
                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            End If
        End If
    End Sub
End Class