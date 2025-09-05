Imports System.Data.OleDb
Imports System.IO

Public Class frmEmpMaster
    Dim dtCostCentre As New DataTable
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim tran As OleDbTransaction
    Dim strSql As String
    Dim flagSave As Boolean = False
    Dim chitMaindb As String = GetAdmindbSoftValue("CHITDBPREFIX", , tran) + "SAVINGS"
    Dim Emp_Desig As String = GetAdmindbSoftValue("EMP_DEF_DESID", "")
    Dim EmpImgbyte As Byte() = Nothing

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        funcGridStyle(gridView)
        cmbDiscAuthorize.Items.Add("YES")
        cmbDiscAuthorize.Items.Add("NO")
        cmbIncentive.Items.Add("YES")
        cmbIncentive.Items.Add("NO")

        cmbActive.Items.Add("YES")
        cmbActive.Items.Add("NO")
        cmbActive.Text = "YES"

        If GetAdmindbSoftValue("COSTCENTRE").ToUpper = "N" Then
            cmbCostcentre_MAN.Enabled = False
        End If

        If cmbCostcentre_MAN.Enabled Then
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            objGPack.FillCombo(strSql, cmbCostcentre_MAN, , False)
        End If

        strSql = " select DesigName from " & cnAdminDb & "..Designation order by DesigName"
        'objGPack.FillCombo(strSql, cmbDesignation_Man)
        'Newly
        Dim Emp_DesigAR As String()
        Emp_DesigAR = Emp_Desig.Split(",")
        If Emp_Desig.ToString <> "" Then
            If Emp_DesigAR.Length = 2 Then
                If userId.ToString = Emp_DesigAR(0).ToString Then
                    strSql = " SELECT DESIGNAME FROM " & cnAdminDb & "..DESIGNATION WHERE DESIGID=" & Val(Emp_DesigAR(1)) & " ORDER BY DESIGNAME"
                Else
                    GoTo flState
                End If
            Else
                GoTo flState
            End If
        Else
flState:
            strSql = " select DesigName from " & cnAdminDb & "..Designation order by DesigName"
        End If
        objGPack.FillCombo(strSql, cmbDesignation_Man, , False)
        funcLoadItemCounter()
    End Sub
    Function funcNew() As Integer
        objGPack.TextClear(Me)
        'Newly Add
        picbxUserImage.Image = My.Resources.noimagenew

        dtpDoj.MinimumDate = (New DateTimePicker).MinDate
        dtpDoj.MaximumDate = (New DateTimePicker).MaxDate
        dtpDoj.Value = GetEntryDate(GetServerDate)
        flagSave = False
        txtEmpId_Num_Man.Text = Val(objGPack.GetMax("EMPID", "EMPMASTER", cnAdminDb))
        cmbItemCounter.Text = ""
        cmbDiscAuthorize.Text = "NO"
        cmbIncentive.Text = "NO"
        cmbCostcentre_MAN.Text = ""
        chkDoj.Checked = False
        tabMain.SelectedTab = tabGeneral
        chkCentralizedLogin.Checked = False
        If cmbCostcentre_MAN.Enabled Then cmbCostcentre_MAN.Select() Else txtEmpId_Num_Man.Select()
    End Function
    Function funcOpen() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Function
        strSql = " SELECT"
        strSql += " (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = E.COSTID)aS COSTNAME"
        strSql += " ,EMPID,EMPNAME,ALIASNAME,"
        strSql += " CASE WHEN ACTIVE = 'Y' THEN 'YES' ELSE 'NO' END AS ACTIVE,"
        strSql += " CONVERT(VARCHAR(10),DATEOFJOIN,103)AS DATEOFJOIN,"
        strSql += " ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4,"
        strSql += " (SELECT DESIGNAME FROM " & cnAdminDb & "..DESIGNATION WHERE DESIGID = DESIGNATIONID)AS DESIGNAME,"
        strSql += " ISNULL((SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER AS I WHERE I.ITEMCTRID = E.ITEMCTRID),'')AS ITEMCTRNAME,"
        strSql += " CASE WHEN DISCAUTHORIZE = 'Y' THEN 'YES' ELSE 'NO' END AS DISCAUTHORIZE,"
        strSql += " PASSWORD,DISCPER,DISCAMT,"
        strSql += " STARGETPIECE,STARGETWEIGHT,STARGETAMOUNT,"
        strSql += " CASE WHEN INCENTIVE = 'Y' THEN 'YES' ELSE 'NO' END AS INCENTIVE,CENTLOGIN"

        strSql += " FROM " & cnAdminDb & "..EMPMASTER AS E"
        strSql += " ORDER BY EMPNAME"
        funcOpenGrid(strSql, gridView)
        If Not gridView.RowCount > 0 Then
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            Exit Function
        End If
        'gridView.Columns("EMPID").Visible = False
        If GetAdmindbSoftValue("COSTCENTRE").ToUpper <> "Y" Then
            gridView.Columns("COSTNAME").Visible = False
        End If
        With gridView
            .Columns("DESIGNAME").HeaderText = "DESIGNAME"
            .Columns("ITEMCTRNAME").HeaderText = "ITEM CTRNAME"
            .Columns("DISCAUTHORIZE").HeaderText = "DISC AUTHORIZE"
            .Columns("STARGETPIECE").HeaderText = "STARGET PIECE"
            .Columns("STARGETWEIGHT").HeaderText = "STARGET WEIGHT"
            .Columns("STARGETAMOUNT").HeaderText = "STARGET AMOUNT"
        End With
        gridView.Columns("PASSWORD").Visible = False
        tabMain.SelectedTab = tabView
        gridView.Select()
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcSave() As Integer
        If GetAdmindbSoftValue("SYNC-TO", , tran) <> "" Then
            MsgBox("Master entry cannot allow at location", MsgBoxStyle.Information)
            Exit Function
        End If
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        If flagSave = False Then
            If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = " & Val(txtEmpId_Num_Man.Text) & "").Length > 0 Then
                MsgBox("EmpId Already Exist", MsgBoxStyle.Information)
                txtEmpId_Num_Man.Focus()
                Exit Function
            End If
        End If
        If objGPack.DupChecker(txtEmpName__Man, "SELECT 1 FROM " & cnAdminDb & "..EMPMASTER WHERE EMPNAME = '" & txtEmpName__Man.Text & "' AND EMPID <> '" & txtEmpId_Num_Man.Text & "'") Then
            Exit Function
        End If
        'If txtPassword.Enabled And txtPassword.Text = "" Then
        '    MsgBox("Password Should Not Empty", MsgBoxStyle.Information)
        '    txtPassword.Focus()
        '    Exit Function
        'End If
        If flagSave = False Then
            funcAdd()
        Else
            funcUpdate()
        End If
        Return 0
    End Function
    Function funcAdd() As Integer
        Dim designationId As Integer = 0
        Dim itemCntr As Integer
        Dim ds As New Data.DataSet
        ds.Clear()
        strSql = " select ItemCtrId from " & cnAdminDb & "..ItemCounter where ItemCtrName = '" & cmbItemCounter.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "ItemCtrId")
        If ds.Tables("ItemCtrId").Rows.Count > 0 Then
            itemCntr = Val(ds.Tables("ItemCtrId").Rows(0).Item("ItemCtrId").ToString)
        End If
        Dim costId As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostcentre_MAN.Text & "'")
        designationId = Val(objGPack.GetSqlValue("SELECT DESIGID FROM " & cnAdminDb & "..DESIGNATION WHERE DESIGNAME = '" & cmbDesignation_Man.Text & "'"))
        Try
            tran = cn.BeginTransaction
            If chkCentralizedLogin.Checked Then
                strSql = InsertQry(costId, designationId, itemCntr)
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
                For Each Ro As DataRow In dtCostCentre.Rows
                    If Ro.Item("COSTID").ToString.ToUpper = cnCostId.ToUpper Then Continue For
                    strSql = InsertQry(Ro.Item("COSTID").ToString, designationId, itemCntr)
                    Exec(strSql.Replace("'", "''"), cn, Ro.Item("COSTID").ToString, OpenFileDialog1.FileName.ToString, tran, "" & cnAdminDb & ":EMPMASTER:EMPIMAGE:EMPID:" & Val(txtEmpId_Num_Man.Text), , EmpImgbyte)
                Next
            Else
                strSql = InsertQry(costId, designationId, itemCntr)
                ExecQuery(SyncMode.Stock, strSql, cn, tran, costId, , OpenFileDialog1.FileName.ToString, , , , "" & cnAdminDb & ":EMPMASTER:EMPIMAGE:EMPID:" & Val(txtEmpId_Num_Man.Text), , , , EmpImgbyte)
            End If

            'Newly
            If OpenFileDialog1.FileName.ToString <> "" Then
                Dim ImageTemp As Byte() = ConvertImageFiletoBytes(OpenFileDialog1.FileName.ToString)
                strSql = "UPDATE " & cnAdminDb & "..EMPMASTER SET EMPIMAGE = ? "
                strSql += " WHERE  EMPID = " & txtEmpId_Num_Man.Text & ""
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.Parameters.AddWithValue("@EMPIMAGE", ImageTemp)
                cmd.ExecuteNonQuery()
            End If

            tran.Commit()
            MsgBox("Saved Successfully..")
            tran = Nothing
            funcNew()
            EmpImgbyte = Nothing
            OpenFileDialog1.FileName = ""
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message & vbCrLf & ex.StackTrace, MsgBoxStyle.Information)
        End Try
    End Function
    Private Function UpdateQry(ByVal CostId As String, ByVal designationId As Integer, ByVal itemCntr As Integer) As String
        Dim Qry As String = ""
        Qry = " Update " & cnAdminDb & "..EmpMaster Set"
        Qry += " COSTID = '" & CostId & "'" 'COSTID
        Qry += " ,EMPNAME='" & txtEmpName__Man.Text & "'"
        If chkDoj.Checked Then
            Qry += " ,DATEOFJOIN='" & dtpDoj.Value & "'"
        Else
            Qry += " ,DATEOFJOIN=NULL"
        End If
        Qry += " ,ADDRESS1='" & txtAddress1.Text & "'"
        Qry += " ,ADDRESS2='" & txtAddress2.Text & "'"
        Qry += " ,ADDRESS3='" & txtAddress3.Text & "'"
        Qry += " ,ADDRESS4='" & txtAddress4.Text & "'"
        Qry += " ,DesignationId=" & designationId & ""
        Qry += " ,ItemCtrId=" & itemCntr & ""
        Qry += " ,DiscAuthorize='" & Mid(cmbDiscAuthorize.Text, 1, 1) & "'"
        If cmbDiscAuthorize.Text = "YES" Then
            Qry += " ,Password='" & txtPassword.Text & "'"
            Qry += " ,DiscPer=" & Val(txtDiscount_Per.Text) & ""
            Qry += " ,Discamt=" & Val(txtDiscount_Amt.Text) & ""
        Else
            Qry += " ,Password=''"
            Qry += " ,DiscPer=0"
            Qry += " ,Discamt=0"
        End If
        Qry += " ,STargetpiece=" & Val(txtSTPiece_Num.Text) & ""
        Qry += " ,STargetweight=" & Val(txtSTWeight_Wet.Text) & ""
        Qry += " ,STargetamount=" & Val(txtSTAmount_Amt.Text) & ""
        Qry += " ,Incentive='" & Mid(cmbIncentive.Text, 1, 1) & "'"
        Qry += " ,UserId=" & userId & ""
        Qry += " ,Updated='" & Today.Date.ToString("yyyy-MM-dd") & "'"
        Qry += " ,Uptime='" & Date.Now.ToLongTimeString & "'"
        Qry += " ,Active='" & Mid(cmbActive.Text, 1, 1) & "'"
        Qry += " ,Aliasname = '" & txtAliasName.Text & "'"
        Qry += " where EMPID = '" & txtEmpId_Num_Man.Text & "'"
        Dim chitMaindb As String = GetAdmindbSoftValue("CHITDBPREFIX", , tran) + "SAVINGS"
        If objGPack.GetSqlValue("SELECT NAME FROM SYSDATABASES WHERE NAME = '" & chitMaindb & "'", , , tran).Length > 0 Then
            Qry += " Update " & chitMaindb & "..EMPMASTER SET"
            Qry += " EMPNAME='" & txtEmpName__Man.Text & "'"
            If chkDoj.Checked Then
                Qry += " ,DATEOFJOIN='" & dtpDoj.Value & "'"
            Else
                Qry += " ,DATEOFJOIN=NULL"
            End If
            Qry += " ,ADDRESS1='" & txtAddress1.Text & "'"
            Qry += " ,ADDRESS2='" & txtAddress2.Text & "'"
            Qry += " ,ADDRESS3='" & txtAddress3.Text & "'"
            Qry += " ,ADDRESS4='" & txtAddress4.Text & "'"
            Qry += " ,USERID=" & userId & ""
            Qry += " ,UPDATETIME='" & Today.Date.ToString("yyyy-MM-dd") & "'"
            Qry += " ,ACTIVE='" & Mid(cmbActive.Text, 1, 1) & "'"
            Qry += " WHERE EMPID = '" & txtEmpId_Num_Man.Text & "'"
            ExecQuery(SyncMode.Master, Qry, cn, tran)
        End If
        Return Qry
    End Function
    Private Function InsertQry(ByVal CostId As String, ByVal designationId As Integer, ByVal itemCntr As Integer) As String
        Dim Qry As String = ""
        Qry = " Insert into " & cnAdminDb & "..EmpMaster"
        Qry += " ("
        Qry += " EMPID,EMPNAME,DATEOFJOIN,"
        Qry += " ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4,"
        Qry += " DesignationId,ItemCtrId,DiscAuthorize,"
        Qry += " Password,DiscPer,Discamt,STargetpiece,"
        Qry += " STargetweight,STargetamount,Incentive,"
        Qry += " UserId,Updated,Uptime,Active,COSTID,ALIASNAME,CENTLOGIN"
        Qry += " )Values ("
        Qry += " '" & txtEmpId_Num_Man.Text & "'" 'EMPID
        Qry += " ,'" & txtEmpName__Man.Text & "'" 'EMPNAME
        If chkDoj.Checked Then
            Qry += " ,'" & dtpDoj.Value.Date.ToString("yyyy-MM-dd") & "'" 'DATEOFJOIN
        Else
            Qry += " ,NULL"
        End If
        Qry += " ,'" & txtAddress1.Text & "'" 'ADDRESS1
        Qry += " ,'" & txtAddress2.Text & "'" 'ADDRESS2
        Qry += " ,'" & txtAddress3.Text & "'" 'ADDRESS3
        Qry += " ,'" & txtAddress4.Text & "'" 'ADDRESS4
        Qry += " ," & designationId & "" 'DesignationId
        Qry += " ," & itemCntr & "" 'ItemCtrId
        Qry += " ,'" & Mid(cmbDiscAuthorize.Text, 1, 1) & "'" 'DiscAuthorize
        If pnlDiscDetails.Enabled = True Then
            Qry += " ,'" & BrighttechPack.Methods.Encrypt(txtPassword.Text) & "'" 'Password
            Qry += " ," & Val(txtDiscount_Per.Text) & "" 'DiscPer
            Qry += " ," & Val(txtDiscount_Amt.Text) & "" 'Discamt
        Else
            Qry += " ,''" 'Password
            Qry += " ,0" 'DiscPer
            Qry += " ,0" 'Discamt
        End If
        Qry += " ," & Val(txtSTPiece_Num.Text) & "" 'STargetpiece
        Qry += " ," & Val(txtSTWeight_Wet.Text) & "" 'STargetweight
        Qry += " ," & Val(txtSTAmount_Amt.Text) & "" 'STargetamount
        Qry += " ,'" & Mid(cmbIncentive.Text, 1, 1) & "'" 'Incentive
        Qry += " ," & userId & "" 'UserId
        Qry += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Updated
        Qry += " ,'" & Date.Now.ToLongTimeString & "'" 'Uptime
        Qry += " ,'" & Mid(cmbActive.Text, 1, 1) & "'" 'Active
        Qry += " ,'" & CostId & "'" 'COSTID
        Qry += " ,'" & txtAliasName.Text & "'" 'ALISANAME
        Qry += " ,'" & IIf(chkCentralizedLogin.Checked, "Y", "N") & "'" ' CENTLOGIN
        Qry += " )"
        If chitMaindb <> "" Then
            Qry += " INSERT INTO " & chitMaindb & "..EMPMASTER"
            Qry += " ("
            Qry += " EMPID,EMPNAME,DATEOFJOIN,"
            Qry += " ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4,"
            Qry += " USERID,UPDATETIME,ACTIVE,COSTID"
            Qry += " ) VALUES ("
            Qry += " '" & txtEmpId_Num_Man.Text & "'" 'EMPID
            Qry += " ,'" & txtEmpName__Man.Text & "'" 'EMPNAME
            If chkDoj.Checked Then
                Qry += " ,'" & dtpDoj.Value.Date.ToString("yyyy-MM-dd") & "'" 'DATEOFJOIN
            Else
                Qry += " ,NULL"
            End If
            Qry += " ,'" & txtAddress1.Text & "'" 'ADDRESS1
            Qry += " ,'" & txtAddress2.Text & "'" 'ADDRESS2
            Qry += " ,'" & txtAddress3.Text & "'" 'ADDRESS3
            Qry += " ,'" & txtAddress4.Text & "'" 'ADDRESS4            
            Qry += " ," & userId & "" 'UserId
            Qry += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Uptime
            Qry += " ,'" & Mid(cmbActive.Text, 1, 1) & "'" 'Active
            Qry += " ,'" & CostId & "'" 'COSTID
            Qry += " )"
        End If
        Return Qry
    End Function
    Function funcUpdate() As Integer
        Dim designationId As Integer = 0
        Dim itemCntr As Integer
        Dim ds As New Data.DataSet
        ds.Clear()
        strSql = " select ItemCtrId from " & cnAdminDb & "..ItemCounter where ItemCtrName = '" & cmbItemCounter.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ds, "ItemCtrId")
        If ds.Tables("ItemCtrId").Rows.Count > 0 Then
            itemCntr = ds.Tables("ItemCtrId").Rows(0).Item("ItemCtrId")
        End If

        designationId = objGPack.GetSqlValue("SELECT DESIGID FROM " & cnAdminDb & "..DESIGNATION WHERE DESIGNAME = '" & cmbDesignation_Man.Text & "'")

        Dim costId As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostcentre_MAN.Text & "'")

        Dim DelQry As String = " DELETE FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = '" & txtEmpId_Num_Man.Text & "'"
        If chitMaindb <> "" Then
            DelQry += " DELETE FROM " & chitMaindb & "..EMPMASTER WHERE EMPID = '" & txtEmpId_Num_Man.Text & "'"
        End If
        Try
            tran = cn.BeginTransaction
            If chkCentralizedLogin.Checked Then
                strSql = DelQry + InsertQry(costId, designationId, itemCntr)
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
                For Each Ro As DataRow In dtCostCentre.Rows
                    If Ro.Item("COSTID").ToString.ToUpper = cnCostId.ToUpper Then Continue For
                    strSql = DelQry + InsertQry(Ro.Item("COSTID").ToString, designationId, itemCntr)
                    Exec(strSql.Replace("'", "''"), cn, Ro.Item("COSTID").ToString, OpenFileDialog1.FileName.ToString, tran, "" & cnAdminDb & ":EMPMASTER:EMPIMAGE:EMPID:" & Val(txtEmpId_Num_Man.Text), , EmpImgbyte)
                Next
            Else
                strSql = DelQry + InsertQry(costId, designationId, itemCntr)
                ExecQuery(SyncMode.Stock, strSql, cn, tran, costId, , OpenFileDialog1.FileName.ToString, , , , "" & cnAdminDb & ":EMPMASTER:EMPIMAGE:EMPID:" & Val(txtEmpId_Num_Man.Text), , , , EmpImgbyte)
            End If

            If OpenFileDialog1.FileName.ToString <> "" Then
                Dim ImageTemp As Byte() = ConvertImageFiletoBytes(OpenFileDialog1.FileName.ToString)
                strSql = "UPDATE " & cnAdminDb & "..EMPMASTER SET EMPIMAGE = ? "
                strSql += " WHERE  EMPID = " & txtEmpId_Num_Man.Text & ""
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.Parameters.AddWithValue("@EMPIMAGE", ImageTemp)
                cmd.ExecuteNonQuery()
                If chitMaindb <> "" Then
                    strSql = "UPDATE " & cnAdminDb & "..EMPMASTER SET EMPIMAGE = ? "
                    strSql += " WHERE  EMPID = " & txtEmpId_Num_Man.Text & ""
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.Parameters.AddWithValue("@EMPIMAGE", ImageTemp)
                    cmd.ExecuteNonQuery()
                End If
            End If
            If Not EmpImgbyte Is Nothing Then
                strSql = "UPDATE " & cnAdminDb & "..EMPMASTER SET EMPIMAGE = ? "
                strSql += " WHERE  EMPID = " & txtEmpId_Num_Man.Text & ""
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.Parameters.AddWithValue("@EMPIMAGE", EmpImgbyte)
                cmd.ExecuteNonQuery()
                If chitMaindb <> "" Then
                    strSql = "UPDATE " & cnAdminDb & "..EMPMASTER SET EMPIMAGE = ? "
                    strSql += " WHERE  EMPID = " & txtEmpId_Num_Man.Text & ""
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.Parameters.AddWithValue("@EMPIMAGE", EmpImgbyte)
                    cmd.ExecuteNonQuery()
                End If
            End If
            tran.Commit()
            tran = Nothing
            funcNew()
            MsgBox("Updated")
            OpenFileDialog1.FileName = ""
            EmpImgbyte = Nothing
        Catch ex As Exception
            If Not tran Is Nothing Then Exit Function
            MsgBox(ex.Message & vbCrLf & ex.StackTrace, MsgBoxStyle.Information)
        End Try
    End Function
    Function funcGetDetails(ByVal temp As Integer) As Integer

        Dim tempbyte As Byte() = Nothing
        Dim dt_image As New DataTable
        Dim imgMemoryStream As MemoryStream = New MemoryStream()

        strSql = " Select"
        strSql += " EMPID,EMPNAME,"
        strSql += " DATEOFJOIN,"
        strSql += " ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4,"
        strSql += " (SELECT DESIGNAME FROM " & cnAdminDb & "..DESIGNATION WHERE DESIGID = DESIGNATIONID)aS DESIGNAME,"
        strSql += " isnull((select ItemCtrName from " & cnAdminDb & "..ItemCounter as i where i.ItemCtrId = e.ItemCtrId),'')as ItemCtrName,"
        strSql += " case when DiscAuthorize = 'Y' then 'YES' else 'NO' end as DiscAuthorize,"
        strSql += " Password,DiscPer,Discamt,"
        strSql += " STargetpiece,STargetweight,STargetamount,"
        strSql += " case when Incentive = 'Y' then 'YES' else 'NO' end as Incentive,"
        strSql += " case when Active = 'Y' then 'YES' else 'NO' end as Active"
        strSql += " ,(select costname from " & cnAdminDb & "..costcentre where costid = e.costid)as costname"
        strSql += " ,ALIASNAME,CENTLOGIN"
        strSql += " from " & cnAdminDb & "..EmpMaster as E"
        strSql += " where Empid = " & temp & ""
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        With dt.Rows(0)
            cmbCostcentre_MAN.Text = .Item("COSTNAME").ToString
            txtEmpId_Num_Man.Text = .Item("EMPID").ToString
            txtEmpName__Man.Text = .Item("EMPNAME").ToString

            If .Item("DATEOFJOIN").ToString <> "" Then
                dtpDoj.Value = .Item("DATEOFJOIN")
                chkDoj.Checked = True
            Else
                chkDoj.Checked = False
            End If
            txtAddress1.Text = .Item("ADDRESS1").ToString
            txtAddress2.Text = .Item("ADDRESS2").ToString
            txtAddress3.Text = .Item("ADDRESS3").ToString
            txtAddress4.Text = .Item("ADDRESS4").ToString
            cmbDesignation_Man.Text = .Item("DESIGNAME").ToString
            cmbItemCounter.Text = .Item("ItemCtrName").ToString
            cmbDiscAuthorize.Text = .Item("DiscAuthorize").ToString


            If .Item("DiscAuthorize") = "YES" Then
                txtPassword.Text = BrighttechPack.Methods.Decrypt(.Item("Password").ToString)
                txtDiscount_Per.Text = .Item("DiscPer").ToString
                txtDiscount_Amt.Text = .Item("Discamt").ToString
            Else
                txtPassword.Text = ""
                txtDiscount_Per.Text = ""
                txtDiscount_Amt.Text = ""
            End If
            txtSTPiece_Num.Text = .Item("STargetpiece").ToString
            txtSTWeight_Wet.Text = .Item("STargetweight").ToString
            txtSTAmount_Amt.Text = .Item("STargetamount").ToString
            txtAliasName.Text = .Item("ALIASNAME").ToString
            cmbIncentive.Text = .Item("Incentive").ToString
            cmbActive.Text = .Item("ACTIVE").ToString
            If .Item("CENTLOGIN").ToString = "Y" Then chkCentralizedLogin.Checked = True Else chkCentralizedLogin.Checked = False
        End With

        'View Image Newly add txtEmpId_Num_Man
        strSql = "SELECT EMPIMAGE FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID= " & txtEmpId_Num_Man.Text & ""
        da = New OleDbDataAdapter(strSql, cn)
        dt_image.Clear()
        da.Fill(dt_image)

        If (IsDBNull(dt_image.Rows(0).Item("EMPIMAGE")) = True) Then
            picbxUserImage.Image = My.Resources.noimagenew
        Else
            tempbyte = dt_image.Rows(0).Item("EMPIMAGE")
            EmpImgbyte = dt_image.Rows(0).Item("EMPIMAGE")
            imgMemoryStream = New MemoryStream(tempbyte)
            picbxUserImage.Image = Drawing.Image.FromStream(imgMemoryStream)
            picbxUserImage.SizeMode = PictureBoxSizeMode.StretchImage
        End If
        flagSave = True
    End Function
    Function funcLoadItemCounter() As Integer
        If GetAdmindbSoftValue("ITEMCOUNTER").ToUpper = "Y" Then
            strSql = "select ItemCtrName from " & cnAdminDb & "..ItemCounter order by ItemCtrName"
            objGPack.FillCombo(strSql, cmbItemCounter)
            cmbItemCounter.Enabled = True
        Else
            cmbItemCounter.Enabled = False
        End If
    End Function

    Private Sub cmbDiscAuthorize_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbDiscAuthorize.SelectedIndexChanged
        If cmbDiscAuthorize.Text = "YES" Then
            pnlDiscDetails.Enabled = True
        Else
            pnlDiscDetails.Enabled = False
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        funcOpen()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        funcSave()
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        funcOpen()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        funcNew()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        funcExit()
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblStatus.Visible = True
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            If gridView.RowCount > 0 Then
                funcGetDetails(gridView.Item(1, gridView.CurrentRow.Index).Value)
                tabMain.SelectedTab = tabGeneral
                txtEmpId_Num_Man.Select()
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            txtEmpName__Man.Focus()
        ElseIf e.KeyCode = Keys.P Then
            btnPrint_Click(Me, New EventArgs)
        ElseIf e.KeyCode = Keys.X Then
            btnExcel_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub frmEmpMaster_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If tabMain.SelectedTab.Name = tabView.Name Then btnBack_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub frmEmpMaster_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtEmpName__Man.Focused Then
                Exit Sub
            End If
            If txtEmpId_Num_Man.Focused Then Exit Sub
            If gridView.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtEmpName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtEmpName__Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If objGPack.DupChecker(txtEmpName__Man, "SELECT 1 FROM " & cnAdminDb & "..EMPMASTER WHERE EMPNAME = '" & txtEmpName__Man.Text & "' AND EMPID <> '" & txtEmpId_Num_Man.Text & "'") Then
                Exit Sub
            Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus

        lblStatus.Visible = False
    End Sub

    Private Sub txtPassword_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPassword.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtPassword.Text = "" Then
                MsgBox("Password Should not Empty", MsgBoxStyle.Information)
                txtPassword.Focus()
            End If
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
    End Sub

    Private Sub frmEmpMaster_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        strSql = " SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID <> '" & cnCostId & "'"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        If Not objGPack.GetSqlValue("SELECT NAME FROM SYSDATABASES WHERE NAME = '" & chitMaindb & "'", , , tran).Length > 0 Then
            chitMaindb = ""
        End If

        funcNew()
    End Sub

    Private Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGeneral
        If cmbCostcentre_MAN.Enabled Then cmbCostcentre_MAN.Select() Else txtEmpId_Num_Man.Select()
    End Sub

    Private Sub txtEmpId_Num_Man_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtEmpId_Num_Man.GotFocus
        If flagSave Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtEmpId_Num_Man_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtEmpId_Num_Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtEmpId_Num_Man.Text = "" Then
                Exit Sub
            ElseIf objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = " & Val(txtEmpId_Num_Man.Text) & "").Length > 0 Then
                MsgBox("EmpId Already Exist", MsgBoxStyle.Information)
                txtEmpId_Num_Man.Focus()
                Exit Sub
            Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub chkDob_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDoj.CheckedChanged
        dtpDoj.Enabled = chkDoj.Checked
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "EMPLOYEE MASTER", gridView, BrightPosting.GExport.GExportType.Export)
        End If
        Me.Cursor = Cursors.Arrow
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "EMPLOYEE MASTER", gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub
    Private Sub ResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResizeToolStripMenuItem.Click
        If gridView.RowCount > 0 Then
            If ResizeToolStripMenuItem.Checked Then
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridView.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
            'gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End If
    End Sub

    Private Sub btnUserImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUserImage.Click
        If OpenFileDialog1.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            picbxUserImage.Image = Image.FromFile(OpenFileDialog1.FileName)
        End If
    End Sub

#Region "USER DEFINITION"
    Public Function ConvertImageFiletoBytes(ByVal ImageFilePath As String) As Byte()
        Dim _tempByte() As Byte = Nothing
        If String.IsNullOrEmpty(ImageFilePath) = True Then
            Throw New ArgumentNullException("Image File Name Cannot be Null or Empty", "ImageFilePath")
            Return Nothing
        End If
        Try
            Dim _fileInfo As New IO.FileInfo(ImageFilePath)
            Dim _NumBytes As Long = _fileInfo.Length
            Dim _FStream As New IO.FileStream(ImageFilePath, IO.FileMode.Open, IO.FileAccess.Read)
            Dim _BinaryReader As New IO.BinaryReader(_FStream)
            _tempByte = _BinaryReader.ReadBytes(Convert.ToInt32(_NumBytes))
            _fileInfo = Nothing
            _NumBytes = 0
            _FStream.Close()
            _FStream.Dispose()
            _BinaryReader.Close()
            Return _tempByte
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
#End Region
End Class