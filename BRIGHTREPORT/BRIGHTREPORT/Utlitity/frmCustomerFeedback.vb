Imports System.Data.OleDb
Public Class frmCustomerFeedback
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim dr As OleDbDataReader
    Dim strSql As String
    Dim updFlag As Boolean

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click
        Try
            If txtQry.Text = "" Then MsgBox("Feed Back is empty...") : chkOrder.Focus() : Exit Sub
            Dim Sno As Integer = 1
            Dim EmpId As Integer = 0
            Dim PSno As String = ""
            strSql = " SELECT  ISNULL(MAX(CAST(SUBSTRING(SNO,6,20) AS INT))+1,1)SNO FROM  " & cnAdminDb & " ..CUSTOMERQUERY"
            Dim dt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then Sno = dt.Rows(0).Item("SNO")
            strSql = " SELECT EMPID FROM " & cnAdminDb & "..EMPMASTER WHERE EMPNAME='" & CmbEmployee.Text & "'"
            dt = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then EmpId = dt.Rows(0).Item("EMPID")
            strSql = " SELECT SNO FROM " & cnAdminDb & "..PERSONALINFO WHERE " & cmbExportTo.Text & " ='" & txtInkFontDetail.Text & "'"
            dt = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then PSno = dt.Rows(0).Item("SNO")

            strSql = "SELECT  TOP 1 BATCHNO FROM  " & cnAdminDb & " ..ORMAST WHERE ORNO='" & GetCostId(cnCostId) & GetCompanyId(strCompanyId) & txtOrderNo.Text.ToString & "' AND ISNULL(CANCEL,'')='' "
            Dim strBatchno As String = objGPack.GetSqlValue(strSql)
            If strBatchno = "" Then MsgBox("Record Not Found")
            If strBatchno = "" Then
                strSql = " INSERT INTO " & cnAdminDb & "..CUSTOMERQUERY"
                strSql += " (SNO,PSNO,TRANDATE,QUERY,EMPID,USERID,UPDATED,UPTIME,APPVER)VALUES"
                strSql += " ("
                strSql += " '" & GetCostId(cnCostId) + GetCompanyId(strCompanyId) + Sno.ToString & "'" 'serialno
                strSql += " ,'" & PSno.ToString & "'" 'personalinfo
                strSql += " ,'" & GetEntryDate(GetServerDate) & "'"
                strSql += " ,'" & txtQry.Text & "'"
                strSql += " ," & EmpId & ""
                strSql += " ," & userId & ""
                strSql += " ,'" & GetEntryDate(GetServerDate) & "'"
                strSql += " ,'" & GetServerTime() & "'"
                strSql += " ,'" & VERSION & "'" 'VERSION
                strSql += " )"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
            Else
                strSql = " INSERT INTO " & cnAdminDb & "..CUSTOMERQUERY"
                strSql += " (SNO,PSNO,BATCHNO,TRANDATE,QUERY,EMPID,USERID,UPDATED,UPTIME,APPVER)VALUES"
                strSql += " ("
                strSql += " '" & GetCostId(cnCostId) + GetCompanyId(strCompanyId) + Sno.ToString & "'" 'serialno
                strSql += " ,'" & PSno.ToString & "'" 'personalinfo
                strSql += " ,'" & strBatchno & "'" 'personalinfo
                strSql += " ,'" & GetEntryDate(GetServerDate) & "'"
                strSql += " ,'" & txtQry.Text & "'"
                strSql += " ," & EmpId & ""
                strSql += " ," & userId & ""
                strSql += " ,'" & GetEntryDate(GetServerDate) & "'"
                strSql += " ,'" & GetServerTime() & "'"
                strSql += " ,'" & VERSION & "'" 'VERSION
                strSql += " )"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
            End If
            MsgBox("Saved..")
            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
   Private Sub frmCustomerFeedback_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmCustomerFeedback_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        funcNew()
        GrpContainer.Location = New Point((ScreenWid - GrpContainer.Width) / 2, ((ScreenHit - 128) - GrpContainer.Height) / 2)
    End Sub
    Private Sub funcNew()
        cmbExportTo.SelectedText = ""
        txtInkFontDetail.Text = ""
        txtName.Text = ""
        txtAdr1.Text = ""
        txtAdr2.Text = ""
        txtAdr3.Text = ""
        txtMbl.Text = ""
        txtPvg.Text = ""
        txtQry.Text = ""
        txtOrderNo.Text = ""
        CmbEmployee.SelectedText = ""
        cmbExportTo.Focus()
        Dim dt As New DataTable
        strSql = "SELECT EMPNAME  FROM " & cnAdminDb & "..EMPMASTER WHERE ACTIVE='Y'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        BrighttechPack.GlobalMethods.FillCombo(CmbEmployee, dt, "EMPNAME")
        strSql = "SELECT NAME FROM  " & cnAdminDb & "..SYSCOLUMNS  WHERE ID= OBJECT_ID('" & cnAdminDb & "..PERSONALINFO') "
        da = New OleDbDataAdapter(strSql, cn)
        dt = New DataTable
        da.Fill(dt)
        chkOrder.Checked = False
        txtOrderNo.Enabled = False
        BrighttechPack.GlobalMethods.FillCombo(cmbExportTo, dt, "NAME")
        cmbExportTo.Text = "Mobile"
        chkOrder.Focus()
    End Sub

    Private Sub txtInkFontDetail_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtInkFontDetail.KeyDown
        If e.KeyCode = Keys.Enter Then
            If txtInkFontDetail.Text = "" Then MsgBox("Please Enter " & cmbExportTo.Text) : txtInkFontDetail.Focus() : Exit Sub
            FuncRead()
        End If
    End Sub
    Public Sub FuncRead()
        Try
            strSql = "SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE " & cmbExportTo.Text & "='" & txtInkFontDetail.Text & "' "
            Dim ds As New DataSet
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                strSql = "SELECT PNAME,ADDRESS1,ADDRESS2,ADDRESS3,MOBILE,PREVILEGEID,PINCODE,SNO,STATE,COUNTRY,TYPE,ACCODE,TRANDATE,TITLE,INITIAL,DOORNO,AREA,CITY,PHONERES,EMAIL,FAX,SYSTEMID,CANCEL,VATEXM,APPVER,PREVILEGEID,COMPANYID,COSTID,LASTMAILDATE,LASTSMSDATE,PAN,IDTYPE,IDNO,IDIMAGEFILE,USERID FROM " & cnAdminDb & "..PERSONALINFO WHERE " & cmbExportTo.Text & "='" & txtInkFontDetail.Text & "' "
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
                dr = cmd.ExecuteReader()
                While dr.Read()
                    txtName.Text = dr(0).ToString
                    txtAdr1.Text = dr(1).ToString
                    txtAdr2.Text = dr(2).ToString
                    txtAdr3.Text = dr(3).ToString
                    txtMbl.Text = dr(4).ToString
                    txtPvg.Text = dr(5).ToString
                End While
            Else
                MsgBox("Record Not Found")
                txtInkFontDetail.Focus()
            End If
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Public Sub FuncReadOrder()
        Try
                strSql = "SELECT  TOP 1 BATCHNO FROM  " & cnAdminDb & " ..ORMAST WHERE ORNO='" & GetCostId(cnCostId) & GetCompanyId(strCompanyId) & txtOrderNo.Text.ToString & "' AND ISNULL(CANCEL,'')='' "
            Dim strBatchno As String = objGPack.GetSqlValue(strSql)
            If strBatchno = "" Then MsgBox("Record Not Found") : Exit Try
            strSql = "SELECT  TOP 1 PSNO FROM  " & cnAdminDb & " ..CUSTOMERINFO WHERE BATCHNO='" & strBatchno & "' "
            Dim PSno As String = objGPack.GetSqlValue(strSql)
            If PSno = "" Then MsgBox("Personal Details Not Found") : Exit Try

            strSql = "SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = '" & PSno & "' "
            Dim ds As New DataSet
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                strSql = "SELECT PNAME,ADDRESS1,ADDRESS2,ADDRESS3,MOBILE,PREVILEGEID,PINCODE,SNO,STATE,COUNTRY,TYPE,ACCODE,TRANDATE,TITLE,INITIAL,DOORNO,AREA,CITY,PHONERES,EMAIL,FAX,SYSTEMID,CANCEL,VATEXM,APPVER,PREVILEGEID,COMPANYID,COSTID,LASTMAILDATE,LASTSMSDATE,PAN,IDTYPE,IDNO,IDIMAGEFILE,USERID FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO ='" & PSno & "' "
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
                dr = cmd.ExecuteReader()
                While dr.Read()
                    txtName.Text = dr(0).ToString
                    txtAdr1.Text = dr(1).ToString
                    txtAdr2.Text = dr(2).ToString   
                    txtAdr3.Text = dr(3).ToString
                    txtMbl.Text = dr(4).ToString
                    txtPvg.Text = dr(5).ToString
                End While
            Else
                MsgBox("Record Not Found")
                txtInkFontDetail.Focus()
            End If
        Catch ex As Exception
                MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
        End Sub

    Private Sub CmbEmployee_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles CmbEmployee.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If CmbEmployee.Text = "" Then
                MsgBox("Select Employee", MsgBoxStyle.Information)
                CmbEmployee.Focus()
            End If
        End If
    End Sub

    Private Sub txtQry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtQry.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtQry.Text = "" Then
                MsgBox("Enter Customer Query", MsgBoxStyle.Information)
                txtQry.Focus()
            End If
        End If
    End Sub

    Private Sub chkOrder_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOrder.CheckedChanged
        If chkOrder.Checked Then
            txtOrderNo.Enabled = True
            cmbExportTo.Enabled = False
            txtInkFontDetail.Enabled = False
            txtName.Enabled = False
            txtAdr1.Enabled = False
            txtAdr2.Enabled = False
            txtAdr3.Enabled = False
            txtMbl.Enabled = False
            txtPvg.Enabled = False
        Else
            txtOrderNo.Enabled = False
            cmbExportTo.Enabled = True
            txtInkFontDetail.Enabled = True
            txtName.Enabled = True
            txtAdr1.Enabled = True
            txtAdr2.Enabled = True
            txtAdr3.Enabled = True
            txtMbl.Enabled = True
            txtPvg.Enabled = True
        End If
    End Sub

    Private Sub txtOrderNo_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtOrderNo.KeyDown
        If e.KeyCode = Keys.Enter Then
            FuncReadOrder()
        ElseIf e.KeyCode = Keys.Insert Then
            Dim TEMPHELPORD As String = "TEMPHELPORD" + systemId
            strSql = "  IF OBJECT_ID('TEMPTABLEDB.." & TEMPHELPORD & "')IS NOT NULL DROP TABLE TEMPTABLEDB.." & TEMPHELPORD & ""
            strSql += vbCrLf + "  SELECT"
            strSql += vbCrLf + "  	 DISTINCT SUBSTRING(ORNO,6,20)ORNO,O.COMPANYID COMPANYID_HIDE,O.COSTID COSTID_HIDE"
            strSql += vbCrLf + "  	,PNAME"
            strSql += vbCrLf + "  	,CASE WHEN ISNULL(DOORNO,'') = '' THEN ADDRESS1 ELSE ISNULL(DOORNO,'') + ISNULL(ADDRESS1,'') END ADDRESS1"
            strSql += vbCrLf + "  	,ADDRESS2,MOBILE"
            strSql += vbCrLf + "  	,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = O.COSTID) COSTCENTRE"
            strSql += " INTO TEMPTABLEDB.DBO." & TEMPHELPORD
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..ORMAST AS O LEFT OUTER JOIN " & cnAdminDb & "..CUSTOMERINFO AS C"
            strSql += vbCrLf + "  ON O.BATCHNO = C.BATCHNO"
            strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..PERSONALINFO AS P"
            strSql += vbCrLf + "  ON C.PSNO = P.SNO WHERE ISNULL(O.CANCEL,'') != 'Y'"
            strSql += vbCrLf + "  AND ISNULL(O.COSTID,'') = '" & cnCostId & "'"
            strSql += vbCrLf + "AND  ORTYPE='O'"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            strSql = "SELECT * FROM  TEMPTABLEDB.DBO." & TEMPHELPORD & " ORDER BY CONVERT(INT,SUBSTRING(ORNO,2,14))"
            Dim dr As DataRow
            dr = Nothing
            dr = BrighttechPack.SearchDialog.Show_R("Find Order No", strSql, cn, 3, , , , , , , False)
            If dr IsNot Nothing Then
                txtOrderNo.Text = dr.Item("ORNO").ToString
            End If
            txtOrderNo.SelectAll()
        End If
    End Sub
End Class