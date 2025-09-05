Imports System.Data.OleDb
Imports System.Text.RegularExpressions
Public Class frmAccountHead
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim dtAcGroup As New DataTable
    Dim cnt As Integer
    Dim strSql As String
    Dim flagSave As Boolean = False
    Dim acCode As String = Nothing    ''For Update Purpose
    Dim preLock As Boolean = False
    Dim preReq As Boolean = False
    Public _ExternalCalling As Boolean
    Public NewAccName As String
    Dim objOutstDet As frmOpeningTrailBalOutStDt
    Dim dtCompany As New DataTable
    Dim dtCostcentre As New DataTable
    Dim insOutstdt As Boolean = False
    Public LastAccode As String = Nothing
    Dim PANNOFORMAT As String = GetAdmindbSoftValue("PANNOFORMAT", "")
    Dim GSTNOFORMAT As String = GetAdmindbSoftValue("GSTNOFORMAT", "")
    Dim DUELIMIT_USER_AUTH As String = GetAdmindbSoftValue("MAINTAIN_USERAUTH", "N")
    Private ViewMode As Boolean = False
    Private ViewAcName As String = Nothing
    Dim dtAccNames As New DataTable
    Public DisableCtrl As Boolean = False
    Dim RELIGION As Boolean = IIf(GetAdmindbSoftValue("RELIGION", "N") = "Y", True, False)
    Dim GSTNOVALID As String = GetAdmindbSoftValue("GSTNOVALID", "W")
    Dim GSTNOVALID_ACC As String = GetAdmindbSoftValue("GSTNOVALID_ACC", "")
    Dim editMobileNo As String
    Dim frmAc As New frmAcAddInfo
    Dim AddInfo As Boolean = False
    Dim dupCheck As Boolean = True
    Dim dtExemption As New DataTable
    Dim OTP_Password As String
    Dim APP_ACHEAD As String = GetAdmindbSoftValue("APP_ACHEAD", "")
    Dim UPDATE_MOBILENO As Boolean = IIf(GetAdmindbSoftValue("UPDATE_MOBILENO_PINFO", "N") = "Y", True, False)
    Dim UPDATE_EXISTMOBILENO As Boolean = False
    Dim ADDAREAVSPINCHK As Boolean = IIf(GetAdmindbSoftValue("ADDAREAVSPINCHK ", "N") = "Y", True, False)


    Public Sub New(ByVal AcName As String) ''Acc Information
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        With dtExemption
            .Columns.Add("OPTIONID", GetType(Integer))
            .Columns.Add("EXEMPUSER", GetType(Integer))
            .Columns.Add("EXEMPOPEN", GetType(String))
            .Columns.Add("EXEMPDATE", GetType(String))
            .Columns.Add("EXEMPTIME", GetType(String))
            .Columns.Add("DESCRIPTION", GetType(String))
            Dim col As New DataColumn("KEYNO")
            col.AutoIncrement = True
            col.AutoIncrementSeed = 1
            col.AutoIncrementStep = 1
            .Columns.Add(col)
        End With
        btnSave.Visible = False
        btnNew.Visible = False
        btnOpen.Visible = False
        ViewMode = True
        ViewAcName = AcName
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        lblStatus.Visible = False
        With dtExemption
            .Columns.Add("OPTIONID", GetType(Integer))
            .Columns.Add("EXEMPUSER", GetType(Integer))
            .Columns.Add("EXEMPOPEN", GetType(String))
            .Columns.Add("EXEMPDATE", GetType(String))
            .Columns.Add("EXEMPTIME", GetType(String))
            .Columns.Add("DESCRIPTION", GetType(String))
            Dim col As New DataColumn("KEYNO")
            col.AutoIncrement = True
            col.AutoIncrementSeed = 1
            col.AutoIncrementStep = 1
            .Columns.Add(col)
        End With
    End Sub
    Function funcClear()
        acCode = 0
        objGPack.TextClear(Me)
        objGPack.TextClear(grpInfo)
        Return 0
    End Function
    Function funcFillGrid() As Integer
        strSql = " SELECT ACCODE ACCODE,TITLE,INITIAL,ACNAME ""ACCOUNT NAME"","
        strSql += " (SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP AS LG WHERE LG.ACGRPCODE = H.ACGRPCODE)AS ""GROUP"","
        strSql += " CASE WHEN ACTYPE = 'B' THEN 'BANK' "
        strSql += " WHEN ACTYPE = 'H' THEN 'CASH' "
        strSql += " WHEN ACTYPE = 'G' THEN 'SMITH' "
        strSql += " WHEN ACTYPE = 'C' THEN 'CUSTOMER' "
        strSql += " WHEN ACTYPE = 'D' THEN 'DEALER' WHEN ACTYPE = 'I' THEN 'INTERNAL' ELSE 'OTHERS' END AS ""ACCOUNT TYPE"","
        strSql += " PREVILEGEID,"
        strSql += " DOORNO,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,"
        strSql += " PINCODE,PHONENO,MOBILE,ISNULL(LTRIM(CONVERT(VARCHAR,DOBIRTH,103)),'')DOB"
        If RELIGION Then
            strSql += " ,CASE WHEN RELIGION='H' THEN 'Hindu' WHEN RELIGION='M' THEN 'Muslim' "
            strSql += " WHEN RELIGION='C' THEN 'Christain' WHEN RELIGION='S' THEN 'Sikh' ELSE 'Others' END RELIGION"
        End If
        strSql += " ,EMAILID"
        strSql += " ,WEBSITE,"
        strSql += " CASE WHEN LEDPRINT = 'Y' THEN 'YES' ELSE 'NO' END AS LEDGPRINT,"
        strSql += " CASE WHEN TDSFLAG = 'F' THEN 'FORM-G' WHEN TDSFLAG = 'Y' THEN 'YES' ELSE 'NO' END AS TDSFLAG,"
        strSql += " TDSPER,"
        strSql += " CASE WHEN DEPFLAG = 'Y' THEN 'YES' ELSE 'NO' END AS DEPFLAG,"
        strSql += " DEPPER,"
        strSql += " CASE WHEN OUTSTANDING = 'Y' THEN 'YES' ELSE 'NO' END AS OUTSTANDING,"
        strSql += " AUTOGEN,VATEX,"
        strSql += " CASE WHEN LOCALOUTST = 'L' THEN 'LOCAL' WHEN LOCALOUTST = 'I' THEN 'IMPORT/EXPORT' ELSE 'OUTSTATION' END AS ""LOCAL OUTST"","
        strSql += " LOCALTAXNO,CENTRALTAXNO,TIN,PAN,BANKNAME,BANKACNO,CONTACTPERSON,CONTACTPNO,INVENTORY,POSPAYLIMIT"
        strSql += " ,(SELECT STATENAME FROM " & cnAdminDb & "..STATEMAST WHERE STATEID=H.STATEID)STATENAME"
        strSql += " ,ISNULL(GSTNO,'') GSTIN"
        strSql += " FROM " & cnAdminDb & "..ACHEAD AS H WHERE 1=1"
        If cmbOpenGroupName.Text <> "ALL" And cmbOpenGroupName.Text <> "" Then
            strSql += " AND ACGRPCODE=(SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP AS LG WHERE ACGRPNAME = '" & cmbOpenGroupName.Text & "')"
        End If
        'If txtSearch.Text <> "" Then
        '    strSql += " AND ACNAME LIKE '" & txtSearch.Text & "%'"
        'End If
        If txtSearch.Text <> "" And cmbSearchType.Text <> "" Then
            strSql += " AND " & cmbSearchType.Text & " LIKE '" & txtSearch.Text & "%'"
        End If
        If chkOnlyPrevilege.Checked = True Then
            strSql += " AND ISNULL(PREVILEGEID,'')<>''"
        End If
        funcOpenGrid(strSql, gridView)
        With gridView
            .Columns("ACCODE").Width = 70
            .Columns("ACCOUNT NAME").Width = 350
            .Columns("GROUP").Width = 250
            .Columns("ACCOUNT TYPE").Width = 160
        End With
    End Function
    Function funcAdd()
        Dim ds As New Data.DataSet
        ds.Clear()
        Dim tempAcCode As Integer = Nothing
        Dim AcCode As String = Nothing
        Dim AcGrpCode As Integer = Nothing
        Dim AcSubGrpCode As Integer = Nothing
        Dim dob As Date = Nothing
        Dim costname As String = GetSelectedCostId(chkcmbCostcentre, False)
        Dim tran As OleDbTransaction = Nothing
        Try
            If dupCheck = True Then
                If Trim(txtMobile.Text) <> "" Then
                    If Not GetAdmindbSoftValue("ACHEAD_MULTI_MOBILE", "N") = "Y" Then
                        If objGPack.DupCheck("SELECT 1 FROM " & cnAdminDb & "..ACHEAD WHERE MOBILE = '" & txtMobile.Text & "'", tran) Then
                            MsgBox("MobileNo already Exists.", MsgBoxStyle.Information)
                            txtMobile.Focus()
                            Exit Function
                        End If
                    End If
                End If
            End If
            tran = cn.BeginTransaction

            strSql = " SELECT CONVERT(INT,MAX(CTLTEXT))AS ACCODE FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'ACCODE'"
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(ds, "AcCode")
            tempAcCode = ds.Tables("AcCode").Rows(0).Item("AcCode").ToString
ACCODE_GEN: tempAcCode += 1
            If _ExternalCalling Then
                AcCode = GetCostId(cnCostId) & funcSetNumberStyle(tempAcCode.ToString, 5)
            Else
                AcCode = funcSetNumberStyle(tempAcCode.ToString, 7)
            End If

            If objGPack.DupCheck("SELECT 1 FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & AcCode & "'", tran) Then
                GoTo ACCODE_GEN
            End If

            strSql = " Update " & cnAdminDb & "..SoftControl "
            strSql += " Set CtlText = '" & tempAcCode & "' "
            strSql += " where CtlId = 'ACCODE' "
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()

            ''Finding GroupCode
            AcGrpCode = funcFindGroupCode(cmbGroup_Man.Text, tran)

            Dim CompId As String = ""
            strSql = " SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & ")"
            Dim dtCompId As New DataTable
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(dtCompId)
            If dtCompId.Rows.Count > 0 Then
                For cnt As Integer = 0 To dtCompId.Rows.Count - 1
                    CompId += dtCompId.Rows(cnt)("COMPANYID").ToString()
                    'If cnt <> dtCompId.Rows.Count - 1 Then
                    CompId += ","
                    'End If
                Next
                CompId = Mid(CompId, 1, Len(CompId) - 1)
            End If

            strSql = "SELECT STATEID FROM " & cnAdminDb & "..STATEMAST WHERE STATENAME='" & cmbState_OWN.Text & "'"
            Dim StateId As Integer = Val(objGPack.GetSqlValue(strSql, "STATEID", 24, tran).ToString)

            Dim GLEDGERCODE As String = ""
            If cmbLedger.Text <> "" Then GLEDGERCODE = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbLedger.Text & "'", "ACCODE", "", tran)
            Dim TdsCatid As Integer = 0
            If cmbTDSCategory.Text <> "" Then TdsCatid = Val(objGPack.GetSqlValue("SELECT TDSCATID FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATNAME = '" & cmbTDSCategory.Text & "'", "TDSCATID", "", tran))

            If ChkAddInfo.Checked And AcCode <> "" And AddInfo Then
                Dim IssId As Integer
                Dim EntId As Integer
                strSql = "SELECT EMPID FROM " & cnAdminDb & "..EMPMASTER WHERE EMPNAME='" & frmAc.CmbIssuedBy.Text & "'"
                IssId = objGPack.GetSqlValue(strSql, "EMPID", , tran)
                strSql = "SELECT EMPID FROM " & cnAdminDb & "..EMPMASTER WHERE EMPNAME='" & frmAc.CmbEnteredBy.Text & "'"
                EntId = objGPack.GetSqlValue(strSql, "EMPID", , tran)
                strSql = "INSERT INTO " & cnAdminDb & "..ACCADDINFO(ACCODE,GUARDIAN,SUPNAME,CHILD1,CHILD2,CHILD3"
                strSql += " ,S_DOB,C1_DOB,C2_DOB,C3_DOB,OCCUPATION,ISSUEDBY,ENTERED)"
                strSql += " VALUES"
                strSql += " ("
                strSql += "'" & AcCode & "'"
                strSql += ",'" & frmAc.txtGuardian.Text & "'"
                strSql += ",'" & frmAc.txtSupName.Text & "'"
                strSql += ",'" & frmAc.txtChild1.Text & "'"
                strSql += ",'" & frmAc.txtChild2.Text & "'"
                strSql += ",'" & frmAc.txtChild3.Text & "'"
                If frmAc.Chkdb1.Checked Then
                    strSql += ",'" & frmAc.dtpDOB.Value.Date.ToString("yyyy-MM-dd") & "'"
                Else
                    strSql += ",NULL"
                End If
                If frmAc.Chkdb2.Checked Then
                    strSql += ",'" & frmAc.dtpC1DOB.Value.Date.ToString("yyyy-MM-dd") & "'"
                Else
                    strSql += ",NULL"
                End If
                If frmAc.CheckBox2.Checked Then
                    strSql += ",'" & frmAc.dtpC2DOB.Value.Date.ToString("yyyy-MM-dd") & "'"
                Else
                    strSql += ",NULL"
                End If
                If frmAc.CheckBox3.Checked Then
                    strSql += ",'" & frmAc.dtpC3DOB.Value.Date.ToString("yyyy-MM-dd") & "'"
                Else
                    strSql += ",NULL"
                End If
                strSql += ",'" & frmAc.txtOccupation.Text & "'"
                strSql += ",'" & IssId & "'"
                strSql += ",'" & EntId & "'"
                strSql += " )"
                ExecQuery(SyncMode.Master, strSql, cn, tran, , , , , , , , Not _ExternalCalling)
            End If

            strSql = " INSERT INTO " & cnAdminDb & "..ACHEAD("
            strSql += " ACCODE,TITLE,INITIAL,ACNAME,ACGRPCODE,ACSUBGRPCODE,"
            strSql += " ACTYPE,DOORNO,ADDRESS1,ADDRESS2,"
            strSql += " ADDRESS3,AREA,CITY,PINCODE,"
            strSql += " PHONENO,MOBILE,"
            If dtpDOB.Enabled = True Then
                strSql += " DOBIRTH,"
            End If
            If RELIGION Then
                strSql += " RELIGION,"
            End If
            If dtpADate.Enabled = True Then
                strSql += " ANNIVERSARY,"
            End If
            strSql += " EMAILID,"
            strSql += " WEBSITE,LEDPRINT,TDSFLAG,TDSPER,TDSCATID,TCSFLAG,TCSPER,"
            strSql += " DEPFLAG,DEPPER,OUTSTANDING,AUTOGEN,"
            strSql += " VATEX,LOCALOUTST,LOCALTAXNO,CENTRALTAXNO,"
            strSql += " USERID,CRDATE,CRTIME "
            strSql += " ,TIN,PAN,BANKNAME,BANKACNO"
            strSql += " ,CONTACTPERSON,CONTACTPNO"
            strSql += " ,PREVILEGEID,INVENTORY"

            strSql += " ,GSTNO,CREDITDAYS,CREDITLIMIT,BANKOTHERDET,ACTIVE,COMPANYID,MACCODE,POSPAYLIMIT,INTPER"
            strSql += " ,COSTID,STATEID,COUNTRY,PREVILEGETYPEID"

            strSql += " )VALUES("
            strSql += " '" & AcCode & "','" & cmbAddressTitle.Text & "','" & txtAddressInitial.Text & "','" & txtName_MAN.Text & "'," & AcGrpCode & "," & AcSubGrpCode & ","
            Dim strAccType As String = ""
            strAccType = objGPack.GetSqlValue("SELECT TOP 1 TYPEID FROM " & cnAdminDb & "..ACCTYPE WHERE TYPENAME='" & cmbAccountType.Text.ToString & "'", , , tran)
            strSql += " '" & strAccType & "'"
            'strSql += " '" & cmbAccountType.Text.Substring(0, 1) & "'"
            strSql += " ,'" & txtDoorNo.Text & "','" & txtAddress1.Text & "','" & txtAddress2.Text & "',"
            strSql += " '" & txtAddress3.Text & "'"
            strSql += " ,'" & cmbArea_OWN.Text & "'"
            'strSql += " ,'" & BrighttechPack.GetSqlValue(cn, "SELECT AREAID FROM " & cnAdminDb & "..AREAMAST WHERE AREANAME = '" & cmbArea_OWN.Text & "'", "AREAID", "", tran) & "'"
            strSql += " ,'" & cmbCity_OWN.Text & "','" & txtPincode.Text & "',"
            strSql += " '" & txtPhoneNo.Text & "','" & txtMobile.Text & "',"
            If dtpDOB.Enabled = True Then
                strSql += " '" & dtpDOB.Value.Date.ToString("yyyy-MM-dd") & "',"
            End If
            If RELIGION Then
                strSql += " '" & Mid(CmbReligion.Text, 1, 1) & "',"
            End If
            If dtpADate.Enabled = True Then
                strSql += " '" & dtpADate.Value.Date.ToString("yyyy-MM-dd") & "',"
            End If
            strSql += " '" & txtEmailId.Text & "',"
            strSql += " '" & txtWebSite.Text & "','" & Mid(cmbLedgerPrint.Text, 1, 1) & "','" & Mid(cmbTdsFlag.Text, 1, 1) & "'," & Val(txtTdsPer_PER.Text) & "," & TdsCatid & ","
            strSql += " '" & Mid(cmbTcsFlag.Text, 1, 1) & "'," & Val(txtTcsPer_PER.Text) & ","
            strSql += " '" & Mid(cmbDepFlag.Text, 1, 1) & "'," & Val(txtDepPer_PER.Text) & ",'" & Mid(cmbOutStanding.Text, 1, 1) & "','M',"
            strSql += " '','" & Mid(cmbLocalOuts.Text, 1, 1) & "','" & txtLocalTaxNo.Text & "','" & txtCentralTaxNo.Text & "',"
            strSql += " " & userId & ",'" & Today.Date.ToString("yyyy-MM-dd") & "','" & Date.Now.ToLongTimeString & "'"
            strSql += " ,'" & txtTINno.Text & "','" & txtPANno.Text & "','" & txtBankName.Text & "','" & txtBankAccNo.Text & "'"
            strSql += " ,'" & txtContactPer.Text & "','" & txtContactPerDet.Text & "'"
            strSql += " ,'" & txtPrevilegeId.Text & "'"
            strSql += " ,'" & IIf(chkInventory.Checked, "Y", "N") & "'"

            strSql += " ,'" & txtGSTNo.Text & "','" & IIf(txtCrDays_NUM.Text <> "", txtCrDays_NUM.Text, "0") & "','" & IIf(txtCrLimit_AMT.Text <> "", txtCrLimit_AMT.Text, "0") & "'"
            strSql += " ,'" & txtBankOtherDet.Text & "','" & Mid(cmbActive.Text, 1, 1) & "','" & CompId & "'"
            strSql += ",'" & GLEDGERCODE & "'," & Val(txtPoslimit.Text) & "," & Val(txtInt_PER.Text)
            strSql += ",'" & costname & "'"
            strSql += "," & StateId
            strSql += ",'" & cmbCountry_OWN.Text & "'"
            Dim strPREVILEGETYPEID As String = ""
            strPREVILEGETYPEID = objGPack.GetSqlValue("SELECT TOP 1 ISNULL(TYPEID,0)TYPEID FROM " & cnAdminDb & "..PRIVILEGETYPE WHERE TYPENAME='" & cmbPrivilegeType.Text.ToString & "'", , , tran)
            strSql += " ,'" & Val(strPREVILEGETYPEID.ToString.Trim) & "'"
            strSql += " )"
            ExecQuery(SyncMode.Master, strSql, cn, tran, , , , , , , , Not _ExternalCalling)
            If chkPrevilege.Checked Then
                Dim preId As Int64 = Val(objGPack.GetSqlValue("SELECT ISNULL(CONVERT(BIGINT,CTLTEXT),0) FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PRE-NO'", , , tran))
                Dim id As String = Nothing
                For Each c As Char In txtPrevilegeId.Text
                    If IsNumeric(c) Then
                        id += c
                    End If
                Next
                If Val(id) > preId Then
                    strSql = " update " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT = '" & Val(id) & "' WHERE CTLID = 'PRE-NO'"
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()
                End If
            End If
            If Val(txtOpBalAmt_AMT.Text) <> 0 Then
                SaveIntoTrailBalance(cn, tran, AcCode)
            End If
            If _ExternalCalling Then LastAccode = AcCode
            If Not tran Is Nothing Then tran.Commit() : MsgBox("Accode : " & AcCode & " Generated..")
            Dim grp As String = cmbGroup_Man.Text
            Dim type As String = cmbAccountType.Text
            NewAccName = txtName_MAN.Text
            funcNew()
            cmbGroup_Man.Text = grp
            cmbAccountType.Text = type
            Me.SelectNextControl(cmbGroup_Man, True, True, True, True)
        Catch ex As OleDbException
            If Not tran Is Nothing Then tran.Rollback()
            If ex.ErrorCode = 2627 Then
                tran = Nothing
                MsgBox("Name Already Exist", MsgBoxStyle.Information)
                txtName_MAN.Focus()
            Else
                MsgBox(ex.Message)
                MsgBox(ex.StackTrace)
            End If

        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            tran = Nothing
            MsgBox(ex.Message & vbCrLf & ex.StackTrace)
        Finally
            If cn.State = ConnectionState.Open Then
                tran = Nothing
            End If
        End Try
        LoadPicture(AcCode)
        Return 0
    End Function

    Private Function LoadPicture(ByVal acCode As String)
        If acCode <> "" Then
            If objGPack.GetSqlValue("SELECT GSTNO FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE='" & acCode & "'", , "").ToString <> "" Then
                Dim objAttach As New frmAcheadAttachments
                objAttach.txtAccode.Text = acCode.ToString
                Me.Hide()
                objAttach.ShowDialog()
                objAttach.StartPosition = FormStartPosition.CenterScreen
                Me.Show()
            End If
        End If
    End Function

    Function funcOpen()
        If ViewMode Then
            btnExit_Click(Me, New EventArgs)
            Me.Close()
        End If
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Function
        cmbOpenGroupName.Items.Clear()
        cmbOpenGroupName.Items.Add("ALL")
        strSql = "select AcGrpName from " & cnAdminDb & "..AcGroup order by AcGrpName"
        objGPack.FillCombo(strSql, cmbOpenGroupName, False)
        cmbOpenGroupName.Text = "ALL"
        txtSearch.Clear()
        gridView.DataSource = Nothing
        tabMain.SelectedTab = tabView
        cmbOpenGroupName.Focus()
        Return 0
    End Function
    Function funcFindGroupCode(ByVal AcGrpName As String, Optional ByVal tran As OleDbTransaction = Nothing) As Integer
        Dim dt As New DataTable
        dt.Clear()
        strSql = " Select AcGrpCode from " & cnAdminDb & "..AcGroup "
        strSql += " Where AcGrpName = '" & AcGrpName & "'"
        If tran Is Nothing Then
            da = New OleDbDataAdapter(strSql, cn)
        Else
            Dim cmd As OleDbCommand
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
        End If
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Return dt.Rows(0).Item("AcGrpCode").ToString
        Else
            Return 0
        End If
    End Function
    Function funcUpdate()
        If acCode = "0" Then
            Return 0
        End If
        Dim costname As String = GetSelectedCostId(chkcmbCostcentre, False)
        If Trim(txtMobile.Text) <> "" Then
            If Trim(txtMobile.Text) <> editMobileNo Then
                If Not GetAdmindbSoftValue("ACHEAD_MULTI_MOBILE", "N") = "Y" Then
                    If objGPack.DupCheck("SELECT 1 FROM " & cnAdminDb & "..ACHEAD WHERE MOBILE = '" & txtMobile.Text & "'", tran) Then
                        MsgBox("MobileNo already Exists.", MsgBoxStyle.Information)
                        txtMobile.Focus()
                        Exit Function
                    End If
                End If
            End If
        End If

        tran = Nothing
        tran = cn.BeginTransaction

        Dim CompId As String = ""
        strSql = " SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & ")"
        Dim dtCompId As New DataTable
        cmd = New OleDbCommand(strSql, cn, tran)
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtCompId)
        If dtCompId.Rows.Count > 0 Then
            For cnt As Integer = 0 To dtCompId.Rows.Count - 1
                CompId += dtCompId.Rows(cnt)("COMPANYID").ToString()
                'If cnt <> dtCompId.Rows.Count - 1 Then
                CompId += ","
                'End If
            Next
            CompId = Mid(CompId, 1, Len(CompId) - 1)
        End If
        Dim TdsCatid As Integer = 0
        If cmbTDSCategory.Text <> "" Then TdsCatid = Val(objGPack.GetSqlValue("SELECT TDSCATID FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATNAME = '" & cmbTDSCategory.Text & "'", "TDSCATID", "", tran))

        strSql = "SELECT STATEID FROM " & cnAdminDb & "..STATEMAST WHERE STATENAME='" & cmbState_OWN.Text & "'"
        Dim StateId As Integer = Val(objGPack.GetSqlValue(strSql, "STATEID", 24, tran).ToString)

        If ChkAddInfo.Checked And acCode <> "" And AddInfo Then
            Dim IssId As Integer
            Dim EntId As Integer
            strSql = "SELECT EMPID FROM " & cnAdminDb & "..EMPMASTER WHERE EMPNAME='" & frmAc.CmbIssuedBy.Text & "'"
            IssId = objGPack.GetSqlValue(strSql, "EMPID", , tran)
            strSql = "SELECT EMPID FROM " & cnAdminDb & "..EMPMASTER WHERE EMPNAME='" & frmAc.CmbEnteredBy.Text & "'"
            EntId = objGPack.GetSqlValue(strSql, "EMPID", , tran)
            strSql = "UPDATE " & cnAdminDb & "..ACCADDINFO SET "
            strSql += " GUARDIAN='" & frmAc.txtGuardian.Text & "'"
            strSql += " ,SUPNAME='" & frmAc.txtSupName.Text & "'"
            strSql += " ,CHILD1='" & frmAc.txtChild1.Text & "'"
            strSql += " ,CHILD2='" & frmAc.txtChild2.Text & "'"
            strSql += " ,CHILD3='" & frmAc.txtChild3.Text & "'"
            If frmAc.Chkdb1.Checked Then
                strSql += " ,S_DOB='" & frmAc.dtpDOB.Value.Date.ToString("yyyy-MM-dd") & "'"
            Else
                strSql += " ,S_DOB=NULL"
            End If
            If frmAc.Chkdb2.Checked Then
                strSql += " ,C1_DOB='" & frmAc.dtpC1DOB.Value.Date.ToString("yyyy-MM-dd") & "'"
            Else
                strSql += " ,C1_DOB=NULL"
            End If
            If frmAc.CheckBox2.Checked Then
                strSql += " ,C2_DOB='" & frmAc.dtpC2DOB.Value.Date.ToString("yyyy-MM-dd") & "'"
            Else
                strSql += " ,C2_DOB=NULL"
            End If
            If frmAc.CheckBox3.Checked Then
                strSql += " ,C3_DOB='" & frmAc.dtpC3DOB.Value.Date.ToString("yyyy-MM-dd") & "'"
            Else
                strSql += " ,C3_DOB=NULL"
            End If
            strSql += " ,OCCUPATION='" & frmAc.txtOccupation.Text & "'"
            strSql += " ,ISSUEDBY='" & IssId & "'"
            strSql += " ,ENTERED='" & EntId & "'"
            strSql += " WHERE ACCODE='" & acCode & "'"
            ExecQuery(SyncMode.Master, strSql, cn, tran, , , , , , , , Not _ExternalCalling)
        End If

        strSql = " UPDATE " & cnAdminDb & "..ACHEAD SET"
        strSql += " TITLE = '" & cmbAddressTitle.Text & "',"
        strSql += " INITIAL = '" & txtAddressInitial.Text & "',"
        strSql += " ACNAME = '" & txtName_MAN.Text & "',"
        strSql += " ACGRPCODE = (SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME = '" & cmbGroup_Man.Text & "'),"
        'strSql += " ACTYPE = '" & GetAcType() & "',"
        Dim strAccType As String = ""
        strAccType = objGPack.GetSqlValue("SELECT TOP 1 TYPEID FROM " & cnAdminDb & "..ACCTYPE WHERE TYPENAME='" & cmbAccountType.Text.ToString & "'", , , tran)
        strSql += " ACTYPE = '" & strAccType & "',"
        strSql += " DOORNO = '" & txtDoorNo.Text & "',"
        strSql += " ADDRESS1 = '" & txtAddress1.Text & "',"
        strSql += " ADDRESS2 = '" & txtAddress2.Text & "',"
        strSql += " ADDRESS3 = '" & txtAddress3.Text & "',"
        strSql += " AREA = '" & cmbArea_OWN.Text & "',"
        strSql += " CITY = '" & cmbCity_OWN.Text & "',"
        strSql += " STATE = '" & cmbState_OWN.Text & "',"
        strSql += " COUNTRY = '" & cmbCountry_OWN.Text & "',"
        strSql += " PINCODE = '" & txtPincode.Text & "',"
        strSql += " PHONENO = '" & txtPhoneNo.Text & "',"
        strSql += " MOBILE = '" & txtMobile.Text & "',"
        If dtpDOB.Enabled = True Then
            strSql += " DOBIRTH = '" & dtpDOB.Value.Date.ToString("yyyy-MM-dd") & "',"
        Else
            strSql += " DOBIRTH = NULL,"
        End If
        If RELIGION Then
            strSql += " RELIGION = '" & Mid(CmbReligion.Text, 1, 1) & "',"
        End If
        If dtpADate.Enabled = True Then
            strSql += " ANNIVERSARY = '" & dtpADate.Value.Date.ToString("yyyy-MM-dd") & "',"
        Else
            strSql += " ANNIVERSARY = NULL,"
        End If
        strSql += " EMAILID = '" & txtEmailId.Text & "',"
        strSql += " WEBSITE = '" & txtWebSite.Text & "',"
        strSql += " LEDPRINT = '" & Mid(cmbLedgerPrint.Text, 1, 1) & "',"
        strSql += " TDSFLAG = '" & Mid(cmbTdsFlag.Text, 1, 1) & "',"
        strSql += " TDSPER = " & Val(txtTdsPer_PER.Text) & ","
        strSql += " TDSCATID = " & TdsCatid & ","
        strSql += " TCSFLAG = '" & Mid(cmbTcsFlag.Text, 1, 1) & "',"
        strSql += " TCSPER = " & Val(txtTcsPer_PER.Text) & ","
        strSql += " DEPFLAG = '" & Mid(cmbDepFlag.Text, 1, 1) & "',"
        strSql += " DEPPER = " & Val(txtDepPer_PER.Text) & ","
        strSql += " OUTSTANDING = '" & Mid(cmbOutStanding.Text, 1, 1) & "',"
        strSql += " AUTOGEN = 'M',"
        strSql += " VATEX = '',"
        strSql += " LOCALOUTST = '" & Mid(cmbLocalOuts.Text, 1, 1) & "',"
        strSql += " LOCALTAXNO = '" & txtLocalTaxNo.Text & "',"
        strSql += " CENTRALTAXNO = '" & txtCentralTaxNo.Text & "',"
        strSql += " USERID = " & userId & ","
        strSql += " CRDATE = '" & Today.Date.ToString("yyyy-MM-dd") & "',"
        strSql += " CRTIME = '" & Date.Now.ToLongTimeString & "',"
        strSql += " TIN = '" & txtTINno.Text & "',"
        strSql += " PAN = '" & txtPANno.Text & "',"
        strSql += " BANKNAME = '" & txtBankName.Text & "',"
        strSql += " BANKACNO = '" & txtBankAccNo.Text & "',"
        strSql += " CONTACTPERSON = '" & txtContactPer.Text & "',"
        strSql += " CONTACTPNO = '" & txtContactPerDet.Text & "',"
        strSql += " PREVILEGEID = '" & txtPrevilegeId.Text & "',"
        strSql += " INVENTORY = '" & IIf(chkInventory.Checked, "Y", "N") & "'"
        strSql += " ,INTPER = '" & Val(txtInt_PER.Text) & "'"
        strSql += " ,GSTNO = '" & txtGSTNo.Text & "'"
        strSql += " ,CREDITDAYS = '" & IIf(txtCrDays_NUM.Text <> "", txtCrDays_NUM.Text, "0") & "'"
        strSql += " ,CREDITLIMIT = '" & IIf(txtCrLimit_AMT.Text <> "", txtCrLimit_AMT.Text, "0") & "'"
        strSql += " ,BANKOTHERDET = '" & txtBankOtherDet.Text & "'"
        strSql += " ,POSPAYLIMIT= " & Val(txtPoslimit.Text)
        strSql += " ,ACTIVE ='" & Mid(cmbActive.Text, 1, 1) & "'"
        strSql += " ,COMPANYID = '" & CompId & "'"
        strSql += " ,COSTID = '" & costname & "'"
        strSql += " ,STATEID = " & StateId
        Dim strPREVILEGETYPEID As String = ""
        strPREVILEGETYPEID = objGPack.GetSqlValue("SELECT TOP 1 ISNULL(TYPEID,0)TYPEID FROM " & cnAdminDb & "..PRIVILEGETYPE WHERE TYPENAME='" & cmbPrivilegeType.Text.ToString & "'", , , tran)
        strSql += " ,PREVILEGETYPEID = '" & Val(strPREVILEGETYPEID.ToString.Trim) & "'"
        strSql += " WHERE"
        strSql += " ACCODE = '" & acCode & "'"
        Try
            ExecQuery(SyncMode.Master, strSql, cn, tran)
            If UPDATE_MOBILENO = True Then
                strSql = " UPDATE " & cnAdminDb & "..PERSONALINFO SET"
                strSql += " MOBILE ='" & txtMobile.Text & "'"
                strSql += " WHERE"
                strSql += " ACCODE = '" & acCode & "'"
                If UPDATE_EXISTMOBILENO = False Then
                    strSql += " AND ISNULL(MOBILE,'') =''  "
                End If
                ExecQuery(SyncMode.Master, strSql, cn, tran)
            End If

            If chkPrevilege.Checked Then
                Dim preId As Int64 = Val(objGPack.GetSqlValue("SELECT ISNULL(CONVERT(BIGINT,CTLTEXT),0) FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PRE-NO'", , , tran))
                Dim id As String = Nothing
                For Each c As Char In txtPrevilegeId.Text
                    If IsNumeric(c) Then
                        id += c
                    End If
                Next
                If Val(id) > preId Then
                    strSql = " update " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT = '" & Val(id) & "' WHERE CTLID = 'PRE-NO'"
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()
                End If
            End If
            If Val(txtOpBalAmt_AMT.Text) <> 0 Then
                SaveIntoTrailBalance(cn, tran, acCode)
            End If
            If Not tran Is Nothing Then tran.Commit()
            If _ExternalCalling Then LastAccode = acCode
            NewAccName = txtName_MAN.Text
            LoadPicture(acCode)
            funcNew()
        Catch ex As OleDbException
            If Not tran Is Nothing Then
                tran.Rollback()
            End If
            tran.Dispose()
            tran = Nothing
            If ex.ErrorCode = 2627 Then
                MsgBox("Name Already Exist", MsgBoxStyle.Information)
                txtName_MAN.Focus()
            Else
                MsgBox(ex.Message & vbCrLf & ex.StackTrace)
            End If
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & ex.StackTrace)
        End Try
        Return 0
    End Function

    Private Sub SaveIntoTrailBalance(ByVal cn As OleDbConnection, ByVal tran As OleDbTransaction, ByVal acCode As String)
        Dim debit As Double = 0
        Dim credit As Double = 0
        If cmbOpBal.Text.ToUpper = "DR" Then
            debit = Val(txtOpBalAmt_AMT.Text)
        Else
            credit = Val(txtOpBalAmt_AMT.Text)
        End If
        Dim amt As Double = IIf(debit > 0,
        debit,
        credit)
        If amt = 0 Then GoTo AFTEROUTSTDT
        If cmbOutStanding.Text = "YES" Then
            If insOutstdt = False Then
                GoTo AFTERSAVE
            End If
        End If

AFTEROUTSTDT:
        Try
            strSql = "DELETE FROM " & cnAdminDb & "..OUTSTANDING"
            strSql += " WHERE ACCODE = '" & acCode & "'"
            strSql += " AND ISNULL(COSTID,'') = '" & cnCostId & "'"
            strSql += " AND COMPANYID = '" & cnCompanyId & "'"
            strSql += " AND FROMFLAG = 'O'"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            If Val(txtOpBalAmt_AMT.Text) = 0 Then
                strSql = "DELETE FROM " & cnStockDb & "..OPENTRAILBALANCE"
                strSql += " WHERE ACCODE = '" & acCode & "'"
                strSql += " AND ISNULL(COSTID,'') = '" & cnCostId & "'"
                strSql += " AND COMPANYID = '" & cnCompanyId & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
                tran.Commit()
                GoTo AFTERSAVE
            End If

            Dim batchno As String = GetNewBatchno(cnCostId, GetEntryDate(GetServerDate(tran), tran), tran)
            If insOutstdt Then
                For Each ro As DataRow In objOutstDet.dtGridView.Rows
                    Dim recPay As String = Nothing
                    Dim payMode As String = Nothing
                    If credit > 0 Then
                        Select Case ro!REFTYPE.ToString
                            Case "DUE"
                                recPay = "R"
                                payMode = "DR"
                            Case "DUE REC"
                                recPay = "P"
                                payMode = "DU"
                            Case "ADVANCE"
                                recPay = "R"
                                payMode = "AR"
                        End Select
                    Else
                        Select Case ro!REFTYPE.ToString
                            Case "DUE"
                                recPay = "P"
                                payMode = "DU"
                            Case "DUE REC"
                                recPay = "R"
                                payMode = "DR"
                            Case "ADVANCE"
                                recPay = "P"
                                payMode = "AA"
                        End Select
                    End If
                    Dim MRUNNO As String = GetCostId(cnCostId) + GetCompanyId(cnCompanyId) + ro!RUNNO.ToString
                    InsertIntoOustanding(batchno, ro!TRANNO, Mid(ro!RUNNO.ToString, 1, 1), MRUNNO.ToString, Val(ro!AMOUNT.ToString), recPay _
                    , ro!TRANDATE, payMode, , , , , , , , , , , ro!REMARK.ToString, , acCode, cn, tran)
                Next
                InsertIntoPersonalinfo(tran, batchno, acCode, GetEntryDate(GetServerDate(tran), tran) _
                , txtName_MAN.Text, txtAddress1.Text, txtAddress2.Text)
            End If
            strSql = "SELECT ACCODE FROM " & cnStockDb & "..OPENTRAILBALANCE"
            strSql += " WHERE ACCODE = '" & acCode & "'"
            strSql += " AND ISNULL(COSTID,'') = '" & cnCostId & "'"
            strSql += " AND COMPANYID = '" & cnCompanyId & "'"
            If objGPack.GetSqlValue(strSql, , , tran).Length > 0 Then
                ''UPDATE
                strSql = " UPDATE " & cnStockDb & "..OPENTRAILBALANCE SET"
                strSql += " ACCODE = '" & acCode & "'"
                strSql += " ,DEBIT = " & debit & ""
                strSql += " ,CREDIT = " & credit & ""
                strSql += " ,COSTID = '" & cnCostId & "'"
                strSql += " ,USERID  = " & userId & ""
                strSql += " ,UPDATED = '" & Today.Date.ToString("yyyy-MM-dd") & "'"
                strSql += " ,UPTIME = '" & Date.Now.ToLongTimeString & "'"
                strSql += " ,SETTLEMENT = ''"
                strSql += " ,DEBITWT = 0"
                strSql += " ,CREDITWT = 0"
                strSql += " ,VATEXM = ''"
                strSql += " ,APPVER = '" & VERSION & "'"
                strSql += " ,COMPANYID = '" & cnCompanyId & "'"
                strSql += " ,APPROVAL = ''"
                strSql += " ,SETSNO = ''"
                strSql += " ,SETREFNO = ''"
                strSql += " WHERE ACCODE = '" & acCode & "'"
                strSql += " AND ISNULL(COSTID,'') = '" & cnCostId & "'"
                strSql += " AND COMPANYID = '" & cnCompanyId & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            Else
                ''INSERT
                strSql = " INSERT INTO " & cnStockDb & "..OPENTRAILBALANCE("
                strSql += " ACCODE,DEBIT,CREDIT,COSTID"
                strSql += " ,USERID,UPDATED,UPTIME,SETTLEMENT"
                strSql += " ,DEBITWT,CREDITWT,VATEXM,APPVER"
                strSql += " ,COMPANYID,APPROVAL,SETSNO,SETREFNO"
                strSql += " )"
                strSql += " VALUES"
                strSql += " ("
                strSql += " '" & acCode & "'" 'ACCODE
                strSql += " ," & debit & "" 'DEBIT
                strSql += " ," & credit & "" 'CREDIT
                strSql += " ,'" & cnCostId & "'" 'COSTID
                strSql += " ," & userId & "" 'USERID
                strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                strSql += " ,''" 'SETTLEMENT
                strSql += " ,0" 'DEBITWT
                strSql += " ,0" 'CREDITWT
                strSql += " ,''" 'VATEXM
                strSql += " ,'" & VERSION & "'" 'APPVER
                strSql += " ,'" & cnCompanyId & "'" 'COMPANYID
                strSql += " ,''" 'APPROVAL
                strSql += " ,''" 'SETSNO
                strSql += " ,''" 'SETREFNO
                strSql += ")"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
            End If



        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
AFTERSAVE:
    End Sub

    Private Sub InsertIntoPersonalinfo(ByVal tran As OleDbTransaction, ByVal batchno As String, ByVal accode As String, ByVal billdate As Date _
, ByVal Pname As String, ByVal address1 As String, ByVal address2 As String)
        Dim psno As String = frmAddressDia.GetPersonalInfoSno(tran)
        strSql = " INSERT INTO " & cnAdminDb & "..PERSONALINFO"
        strSql += " ("
        strSql += " SNO,ACCODE,TRANDATE,TITLE"
        strSql += " ,INITIAL,PNAME,DOORNO,ADDRESS1"
        strSql += " ,ADDRESS2,ADDRESS3,AREA,CITY"
        strSql += " ,STATE,COUNTRY,PINCODE,PHONERES"
        strSql += " ,MOBILE,EMAIL,FAX,APPVER"
        strSql += " ,PREVILEGEID,COMPANYID,COSTID"
        strSql += " )"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & psno & "'" ''SNO
        strSql += " ,'" & accode & "'" 'ACCODE
        strSql += " ,'" & GetEntryDate(billdate, tran).ToString("yyyy-MM-dd") & "'" 'TRANDATE
        strSql += " ,''" 'TITLE
        strSql += " ,''" 'INITIAL
        strSql += " ,'" & Pname & "'" 'PNAME
        strSql += " ,''" 'DOORNO
        strSql += " ,'" & address1 & "'" 'ADDRESS1
        strSql += " ,'" & address2 & "'" 'ADDRESS2
        strSql += " ,''" 'ADDRESS3
        strSql += " ,''" 'AREA
        strSql += " ,''" 'CITY
        strSql += " ,''" 'STATE
        strSql += " ,''" 'COUNTRY
        strSql += " ,''" 'PINCODE
        strSql += " ,''" 'PHONERES
        strSql += " ,''" 'MOBILE
        strSql += " ,''" 'EMAIL
        strSql += " ,''" 'Fax
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,''" 'PREVILEGEID
        strSql += " ,'" & cnCompanyId & "'" 'COMPANYID
        strSql += " ,'" & cnCostId & "'" 'COSTID
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

        strSql = " INSERT INTO " & cnAdminDb & "..CUSTOMERINFO"
        strSql += " (BATCHNO,PSNO,COSTID)VALUES"
        strSql += " ('" & batchno & "','" & psno & "','" & cnCostId & "')"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
    End Sub

    Private Sub InsertIntoOustanding _
   (
   ByVal BATCHNO As String, ByVal tNo As Integer, ByVal tType As String, ByVal RunNo As String, ByVal Amount As Double,
   ByVal RecPay As String,
   ByVal recDate As Date,
   ByVal PAYMODE As String,
   Optional ByVal GrsWt As Double = 0,
   Optional ByVal NetWt As Double = 0,
   Optional ByVal CatCode As String = Nothing,
   Optional ByVal Rate As Double = Nothing,
   Optional ByVal Value As Double = Nothing,
   Optional ByVal refNo As String = Nothing,
   Optional ByVal refDate As String = Nothing,
   Optional ByVal purity As Double = Nothing,
   Optional ByVal proId As Integer = Nothing,
   Optional ByVal dueDate As String = Nothing,
   Optional ByVal Remark1 As String = Nothing,
   Optional ByVal Remark2 As String = Nothing,
   Optional ByVal Accode As String = Nothing,
   Optional ByVal cn As OleDbConnection = Nothing,
   Optional ByVal tran As OleDbTransaction = Nothing
       )
        If Amount = 0 And GrsWt = 0 Then Exit Sub
        'Dim accode As String = objAddressDia.txtAddressPartyCode.Text
        strSql = " INSERT INTO " & cnAdminDb & "..OUTSTANDING"
        strSql += " ("
        strSql += " SNO,TRANNO,TRANDATE,TRANTYPE,RUNNO"
        strSql += " ,AMOUNT,GRSWT,NETWT,RECPAY"
        strSql += " ,REFNO,REFDATE,EMPID"
        strSql += " ,TRANSTATUS,PURITY,CATCODE,BATCHNO"
        strSql += " ,USERID,UPDATED,UPTIME,SYSTEMID"
        strSql += " ,RATE,VALUE,CASHID,VATEXM,REMARK1,REMARK2,ACCODE,CTRANCODE,DUEDATE,APPVER,COMPANYID,COSTID,FROMFLAG,PAYMODE)"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & GetNewSno(TranSnoType.OUTSTANDINGCODE, tran, "GET_ADMINSNO_TRAN") & "'" ''SNO
        strSql += " ," & tNo & "" 'TRANNO
        strSql += " ,'" & GetEntryDate(recDate, tran).ToString("yyyy-MM-dd") & "'" 'TRANDATE
        strSql += " ,'" & tType & "'" 'TRANTYPE
        strSql += " ,'" & RunNo & "'" 'RUNNO
        strSql += " ," & Math.Abs(Amount) & "" 'AMOUNT
        strSql += " ," & Math.Abs(GrsWt) & "" 'GRSWT
        strSql += " ," & Math.Abs(NetWt) & "" 'NETWT
        strSql += " ,'" & RecPay & "'" 'RECPAY
        strSql += " ,'" & refNo & "'" 'REFNO
        If refDate <> Nothing Then
            strSql += " ,'" & refDate & "'" 'REFDATE
        Else
            strSql += " ,NULL" 'REFDATE
        End If

        strSql += " ,0" 'EMPID
        strSql += " ,''" 'TRANSTATUS
        strSql += " ," & purity & "" 'PURITY
        strSql += " ,'" & CatCode & "'" 'CATCODE
        strSql += " ,'" & BATCHNO & "'" 'BATCHNO
        strSql += " ," & userId & "" 'USERID

        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & systemId & "'" 'SYSTEMID
        strSql += " ," & Rate & "" 'RATE
        strSql += " ," & Value & "" 'VALUE
        strSql += " ,''" 'CASHID
        strSql += " ,''" 'VATEXM
        strSql += " ,'" & Remark1 & "'" 'REMARK1
        strSql += " ,'" & Remark2 & "'" 'REMARK1
        strSql += " ,'" & Accode & "'" 'ACCODE
        strSql += " ," & proId & "" 'CTRANCODE
        If dueDate <> Nothing Then
            strSql += " ,'" & dueDate & "'" 'DUEDATE
        Else
            strSql += " ,NULL" 'DUEDATE
        End If
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,'" & cnCompanyId & "'" 'COMPANYID
        strSql += " ,'" & cnCostId & "'" 'COSTID
        strSql += " ,'O'" 'FROMFLAG
        strSql += " ,'" & PAYMODE & "'" 'PAYMODE
        strSql += " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
    End Sub

    Function funcSave() As Integer
        If ViewMode Then
            btnExit_Click(Me, New EventArgs)
            Me.Close()
        End If
        If txtMobile.Text <> "" And flagSave = False Then
            If Val(objGPack.GetSqlValue("SELECT 1 FROM " & cnAdminDb & "..ACHEAD WHERE MOBILE = '" & txtMobile.Text & "' AND ISNULL(PREVILEGEID,'') <> ''")) = 1 Then
                Dim MsgStr As String
                MsgStr = " Previlege ID Already Created In This Mobile NO "
                MsgStr += vbCrLf + " Previlege ID : " + objGPack.GetSqlValue("SELECT PREVILEGEID FROM " & cnAdminDb & "..ACHEAD WHERE MOBILE = '" & txtMobile.Text & "' AND ISNULL(PREVILEGEID,'') <> ''").ToString
                '                MsgStr += vbCrLf + " Do you want to create?"
                If dupCheck = True Then
                    MsgBox(MsgStr)
                    txtMobile.Focus()
                    txtMobile.SelectAll()
                    Exit Function
                End If
            End If
        End If
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        If objGPack.DupChecker(txtName_MAN, "SELECT 1 FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & txtName_MAN.Text & "' AND ACCODE <> '" & acCode & "'") Then
            txtName_MAN.Focus()
            Exit Function
        End If
        If chkPrevilege.Checked Then
            If txtPrevilegeId.Text = "" Then
                MsgBox("PrevilegeId Should Not Empty", MsgBoxStyle.Information)
                chkPrevilege.Focus()
                Exit Function
            ElseIf objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE PREVILEGEID = '" & txtPrevilegeId.Text & "' AND ACCODE <> '" & acCode & "'") <> "" Then
                MsgBox("PrevilegeId Already Exist", MsgBoxStyle.Information)
                txtPrevilegeId.Focus()
                Exit Function
            End If
        End If
        If txtGSTNo.Text = "" Then
            Dim AcType As String = GetAcType()
            If GSTNOVALID_ACC.Contains(AcType) Then
                If GSTNOVALID = "R" Then
                    MsgBox("GSTIN should not Empty...", MsgBoxStyle.Information)
                    txtGSTNo.Focus()
                    Exit Function
                ElseIf GSTNOVALID = "W" Then
                    If MessageBox.Show("GSTIN should not empty" + vbCrLf + "Do you wish to Continue?", "GstNo Info Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
                        txtGSTNo.Focus()
                        Exit Function
                    End If
                End If
            End If
        End If
        If OTP_Check() = False Then Exit Function

        If acCode <> "" And UPDATE_MOBILENO = True And flagSave = True Then
            Dim permobilenos As String = ""
            Dim dtchkmobilenos As New DataTable
            strSql = "SELECT DISTINCT MOBILE FROM " & cnAdminDb & "..PERSONALINFO WHERE ACCODE='" & acCode & "' AND ISNULL(MOBILE,'')<>'' AND MOBILE<>'" & txtMobile.Text & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtchkmobilenos)
            If dtchkmobilenos.Rows.Count > 0 Then
                For cnt As Integer = 0 To dtchkmobilenos.Rows.Count - 1
                    permobilenos += dtchkmobilenos.Rows(cnt).Item("MOBILE").ToString
                    If cnt < dtchkmobilenos.Rows.Count - 1 Then
                        permobilenos += ","
                    End If
                Next
                If permobilenos.Length > 0 And acCode <> "" Then
                    If MessageBox.Show("Existing mobile Nos:" + vbCrLf + permobilenos.ToString + vbCrLf + "Do you wish to Overwrite?", "MobileNo Info Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
                        UPDATE_EXISTMOBILENO = False
                    Else
                        UPDATE_EXISTMOBILENO = True
                    End If
                End If
            End If
        End If

        If flagSave = False Then ''''Savings AcHead Details
            funcAdd()
        ElseIf flagSave = True Then
            funcUpdate()
        End If
        If _ExternalCalling Then Me.Close()
    End Function
    Function OTP_Check() As Boolean
        If OTPOPTIONCHK("ADDACNAME") Then 'IS_USERLEVELPWD Then
            If MsgBox("Do You Have OTP to proceed?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                OTP_Password = usrpwdid("ADDACNAME", True)
                If OTP_Password = 0 Then MsgBox("OTP Not generated", MsgBoxStyle.OkOnly) : AddExemptionrow(35, "OTP", "ACNAME CREATION ALLOWED") : Exit Function
                If OTP_Password <> 0 Then Return True : Exit Function
            Else
                Return False : Exit Function
            End If
        Else
            Return True : Exit Function
        End If
    End Function
    Private Function AddExemptionrow(ByVal mxOptionid As Integer, ByVal mxOpened As String, ByVal mxDesc As String)
        Dim Exrow As DataRow
        Exrow = dtExemption.NewRow
        Exrow!OPTIONID = mxOptionid
        Exrow!EXEMPUSER = userId
        Exrow!EXEMPOPEN = mxOpened
        Exrow!EXEMPDATE = "'" & Now.Date.ToString("dd-MMM-yyyy") & "'"
        Exrow!EXEMPTIME = "'" & Date.Now.ToLongTimeString & "'"
        Exrow!DESCRIPTION = mxDesc
        dtExemption.Rows.Add(Exrow)
    End Function
    Private Sub InsertExemptiondetails(ByVal Refno As String, ByVal tran As OleDbTransaction)
        For Each ro As DataRow In dtExemption.Rows
            strSql = " INSERT INTO " & cnAdminDb & "..EXEMPTION"
            strSql += " ("
            strSql += " Exempid,OPTIONID,COSTID,EXEMPDATE,EXEMPTIME,EXEMPUSER,EXEMPOPEN,DESCRIPTION"
            strSql += " )"
            strSql += " VALUES"
            strSql += " ("
            strSql += " " & Val(objGPack.GetMax("Exempid", "EXEMPTION", cnAdminDb, tran).ToString)
            strSql += " ," & ro.Item("OPTIONID") 'ISSSNO
            strSql += " ,'" & cnCostId & "'"
            strSql += " ," & ro.Item("EXEMPDATE")
            strSql += " ," & ro.Item("EXEMPTIME")
            strSql += " ," & ro.Item("EXEMPUSER")
            strSql += " ,'" & ro.Item("EXEMPOPEN") & "'"
            strSql += " ,'" & ro.Item("DESCRIPTION") & "-" & Refno & "'"
            strSql += " )"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()
        Next
    End Sub
    Private Function OTPOPTIONCHK(ByVal PWDCTRL As String) As Boolean
        Dim Sqlqry As String = "Select Optionid from " & cnAdminDb & "..PRJPWDOPTION where OPTIONNAME ='" & PWDCTRL & "' AND active = 'Y'"
        Dim Optionid As Integer = Val("" & objGPack.GetSqlValue(Sqlqry, , , tran))
        If Optionid = 0 Then Return False Else Return True
    End Function
    Function funcNew()
        AddInfo = False
        dupCheck = True
        frmAc = New frmAcAddInfo
        ChkAddInfo.Checked = False
        funcClear()
        editMobileNo = ""
        flagSave = False
        dtpDOB.Enabled = False
        chkDOB.Checked = False
        dtpADate.Enabled = False
        chkADate.Checked = False
        dtpDOB.Value = GetServerDate(tran)
        dtpADate.Value = GetServerDate(tran)
        chkInventory.Checked = False
        cmbAccountType.Text = "OTHERS"
        cmbLedgerPrint.Text = "YES"
        lblAcCode.Text = ""
        lblCrBy.Text = ""
        cmbTdsFlag.Text = "NO"
        cmbTcsFlag.Text = "NO"
        cmbDepFlag.Text = "NO"
        cmbOutStanding.Text = "NO"
        cmbLocalOuts.Text = "LOCAL"
        preReq = IIf(UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PRE-REQ'")) = "Y", True, False)
        If preReq Then
            pnlPrivilege.Visible = True
            pnlPrivilege.Enabled = True
            chkPrevilege.Visible = True
            chkPrevilege.Enabled = True
        Else
            pnlPrivilege.Visible = False
            chkPrevilege.Visible = False
        End If

        chkPrevilege.Checked = False
        preLock = IIf(UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PRE-LOCK'")) = "Y", True, False)

        insOutstdt = False
        cmbOutStanding.Text = "Cr"
        tabMain.SelectedTab = tabGeneral
        objOutstDet = New frmOpeningTrailBalOutStDt(0)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", True, "ALL")
        If GetAdmindbSoftValue("CENTR_DB_ALLCOSTID").ToUpper = "Y" Then
            BrighttechPack.GlobalMethods.FillCombo(chkcmbCostcentre, dtCostcentre, "COSTNAME", , cnCostName)
        Else
            BrighttechPack.GlobalMethods.FillCombo(chkcmbCostcentre, dtCostcentre, "COSTNAME", , "ALL")
        End If
        'BrighttechPack.GlobalMethods.FillCombo(chkcmbCostcentre, dtCostcentre, "COSTNAME", True, "ALL")
        funcLoadArea()
        If DisableCtrl Then
            cmbAddressTitle.Select()
        Else
            cmbGroup_Man.Select()
        End If

        If _ExternalCalling Then
            cmbGroup_Man.Text = objGPack.GetSqlValue("select ACNAME from " & cnAdminDb & "..achead  WHERE ACCODE = 'DRS'")
        End If
        If RELIGION Then
            lblReligion.Visible = True
            CmbReligion.Visible = True
            CmbReligion.Items.Clear()
            CmbReligion.Items.Add("")
            CmbReligion.Items.Add("Hindu")
            CmbReligion.Items.Add("Muslim")
            CmbReligion.Items.Add("Christian")
            CmbReligion.Items.Add("Sikh")
            CmbReligion.Items.Add("Others")
            CmbReligion.SelectedIndex = 0
            txtDoorNo.Size = New Size(147, 21)
        Else
            lblReligion.Visible = False
            CmbReligion.Visible = False
            txtDoorNo.Size = New Size(309, 21)
        End If
        funcLoadState()
        If ADDAREAVSPINCHK = False Then
            cmbArea_OWN.Text = ""
            txtPincode.Text = ""
        End If
        UPDATE_EXISTMOBILENO = False
        Return 0
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcLoadGroup()
        dtAcGroup.Clear()
        cmbGroup_Man.Items.Clear()
        strSql = "select AcGrpName,AcMAINCODE from " & cnAdminDb & "..AcGroup order by AcGrpName"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtAcGroup)
        If dtAcGroup.Rows.Count > 0 Then
            For cnt = 0 To dtAcGroup.Rows.Count - 1
                cmbGroup_Man.Items.Add(dtAcGroup.Rows(cnt).Item("AcGrpName").ToString)
            Next
            cmbGroup_Man.Text = dtAcGroup.Rows(0).Item("AcGrpName").ToString
        End If
        Return 0
    End Function

    Private Sub frmAccountHead_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Escape) Then
            If ViewMode Then
                Me.Close()
                Exit Sub
            End If
            If tabMain.SelectedTab.Name = tabView.Name Then
                tabMain.SelectedTab = tabGeneral
            End If
        ElseIf e.KeyChar = Chr(Keys.Enter) Then
            If txtOpBalAmt_AMT.Focused Then Return
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Function GetAcType() As String
        Select Case cmbAccountType.Text
            Case "SMITH"
                Return "G"
            Case "CUSTOMER"
                Return "C"
            Case "DEALER"
                Return "D"
            Case "OTHERS"
                Return "O"
            Case "INTERNAL"
                Return "I"
            Case "BANK"
                Return "B"
            Case "CASH"
                Return "H"
            Case "OTHER"
                Return "O"
            Case "EXPENSES"
                Return "E"
            Case "SGST"
                Return "SG"
            Case "CGST"
                Return "CG"
            Case "IGST"
                Return "IG"
        End Select
    End Function
    Private Sub frmAccountHead_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If DisableCtrl = True Then
            If BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Authorize, False) = False Then
                Dim ctrl As Control
                For Each ctrl In Me.tabGeneral.Controls
                    ctrl.Enabled = False
                Next
                txtName_MAN.Enabled = True
                DgvSearch.Enabled = True
                Label3.Enabled = True
                cmbAddressTitle.Enabled = True
                txtAddressInitial.Enabled = True
                btnSave.Enabled = True
                btnNew.Enabled = True
                btnOpen.Enabled = True
                btnExit.Enabled = True
                pnlPersonalDetail.Enabled = True
                Dim ctrl1 As Control
                For Each ctrl1 In Me.pnlPersonalDetail.Controls
                    ctrl1.Enabled = True
                Next
            End If
        End If
        funcGridStyle(gridView)
        gridView.RowTemplate.Height = 18
        dtpDOB.MaximumDate = New Date(9998, 12, 31)
        dtpDOB.MinimumDate = New Date(1753, 1, 1)
        dtpADate.MaximumDate = New Date(9998, 12, 31)
        dtpADate.MinimumDate = New Date(1753, 1, 1)
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))

        'tabGeneral.BackgroundImage = bakImage
        'tabGeneral.BackgroundImageLayout = ImageLayout.Stretch
        'tabView.BackgroundImage = bakImage
        'tabView.BackgroundImageLayout = ImageLayout.Stretch

        '' START

        cmbAccountType.Items.Clear()
        strSql = "SELECT TYPENAME FROM " & cnAdminDb & "..ACCTYPE  ORDER BY  DISORDER"
        objGPack.FillCombo(strSql, cmbAccountType)
        cmbAccountType.Text = "SMITH"

        cmbPrivilegeType.Items.Clear()
        strSql = "SELECT TYPENAME FROM " & cnAdminDb & "..PRIVILEGETYPE  ORDER BY  TYPENAME"
        objGPack.FillCombo(strSql, cmbPrivilegeType)
        cmbPrivilegeType.Text = ""

        funcLoadState()
        funcLoadCountry()
        ''

        'cmbAccountType.Items.Add("SMITH")
        'cmbAccountType.Items.Add("CUSTOMER")
        'cmbAccountType.Items.Add("DEALER")
        'cmbAccountType.Items.Add("OTHERS")
        'cmbAccountType.Items.Add("BANK")
        'cmbAccountType.Items.Add("CASH")
        'cmbAccountType.Items.Add("INTERNAL")
        'cmbAccountType.Text = "SMITH"

        cmbLedgerPrint.Items.Add("YES")
        cmbLedgerPrint.Items.Add("NO")
        cmbLedgerPrint.Text = "YES"

        cmbTdsFlag.Items.Add("NO")
        cmbTdsFlag.Items.Add("YES")
        cmbTdsFlag.Items.Add("FORM-G")
        cmbTdsFlag.Text = "NO"

        cmbTcsFlag.Items.Add("NO")
        cmbTcsFlag.Items.Add("YES")
        cmbTcsFlag.Text = "NO"


        cmbDepFlag.Items.Add("NO")
        cmbDepFlag.Items.Add("YES")
        cmbDepFlag.Text = "NO"

        cmbOutStanding.Items.Add("YES")
        cmbOutStanding.Items.Add("NO")
        cmbOutStanding.Text = "YES"

        cmbLocalOuts.Items.Add("LOCAL")
        cmbLocalOuts.Items.Add("OUTSTATION")
        cmbLocalOuts.Items.Add("IMPORT/EXPORT")
        cmbLocalOuts.Text = "ALL"

        cmbActive.Items.Add("YES")
        cmbActive.Items.Add("NO")
        cmbActive.Items.Add("HIDE")
        cmbActive.Text = "YES"

        cmbSearchType.Items.Add("ACNAME")
        cmbSearchType.Items.Add("MOBILE")
        cmbSearchType.Items.Add("GSTNO")
        cmbSearchType.Text = "ACNAME"

        If APP_ACHEAD = "Y" Then
            cmbActive.Items.Clear()
            cmbActive.Text = ""
        End If


        strSql = " SELECT " & cnAdminDb & ".DBO.WORDCAPS(ACNAME)ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE 1=1"
        'strSql += " AND ISNULL(MACCODE,'') = ''"
        'strSql += GetAcNameQryFilteration()
        strSql += " ORDER BY ACNAME"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtAccNames)
        DgvSearch.DataSource = dtAccNames
        DgvSearch.ColumnHeadersVisible = False
        DgvSearch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DgvSearch.Visible = False

        If GetAdmindbSoftValue("COSTCENTRE").ToUpper = "Y" Then
            strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
            strSql += " UNION ALL"
            strSql += " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
            strSql += " ORDER BY RESULT,COSTNAME"
            dtCostcentre = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCostcentre)
            If GetAdmindbSoftValue("CENTR_DB_ALLCOSTID").ToUpper = "Y" Then
                BrighttechPack.GlobalMethods.FillCombo(chkcmbCostcentre, dtCostcentre, "COSTNAME", , cnCostName)
            Else
                BrighttechPack.GlobalMethods.FillCombo(chkcmbCostcentre, dtCostcentre, "COSTNAME", , "ALL")
            End If
            chkcmbCostcentre.Enabled = True
        Else
            chkcmbCostcentre.Enabled = False
        End If

        strSql = " SELECT CITYNAME FROM " & cnAdminDb & "..CITYMAST ORDER BY CITYNAME"
        objGPack.FillCombo(strSql, cmbCity_OWN, , False)

        txtEmailId.CharacterCasing = CharacterCasing.Normal
        txtWebSite.CharacterCasing = CharacterCasing.Normal
        funcLoadGroup()
        funcLoadArea()
        funcLoadCompany()
        strSql = "SELECT TDSCATNAME FROM " & cnAdminDb & "..TDSCATEGORY  ORDER BY DISPLAYORDER "
        objGPack.FillCombo(strSql, cmbTDSCategory, True, False)

        cmbOpBal.Items.Clear()
        cmbOpBal.Items.Add("Cr")
        cmbOpBal.Items.Add("Dr")
        cmbOpBal.Text = "Cr"
        dtpCr.Enabled = False
        chkOnlyPrevilege.Checked = False
        funcNew()
        If ViewMode Then
            funcGetDetails(objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & ViewAcName & "'"))
        End If
        DgvSearch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DgvSearch.BackgroundColor = Color.Lavender
    End Sub

    Private Sub cmbAccountType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAccountType.SelectedIndexChanged
        If cmbAccountType.Text = "CUSTOMER" Then
            If preReq Then pnlPrivilege.Visible = True
            chkDOB.Enabled = True
            chkADate.Enabled = True
            ChkAddInfo.Visible = True
        Else
            pnlPrivilege.Visible = False
            chkDOB.Enabled = False
            chkADate.Enabled = False
            ChkAddInfo.Visible = False
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
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
    Function funcGetDetails(ByVal tempAcCode As String)
        Dim dt As New DataTable
        dt.Clear()
        strSql = " SELECT ACNAME,"
        strSql += vbCrLf + "  TITLE,INITIAL,"
        strSql += vbCrLf + "  (SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP AS LG WHERE LG.ACGRPCODE = H.ACGRPCODE)AS ACGRPNAME,"
        'strSql += vbCrLf + "  CASE WHEN ACTYPE = 'G' THEN 'SMITH' "
        'strSql += vbCrLf + "  WHEN ACTYPE = 'C' THEN 'CUSTOMER' "
        'strSql += vbCrLf + "  WHEN ACTYPE = 'B' THEN 'BANK' "
        'strSql += vbCrLf + "  WHEN ACTYPE = 'H' THEN 'CASH' "
        'strSql += vbCrLf + "  WHEN ACTYPE = 'D' THEN 'DEALER' WHEN ACTYPE = 'I' THEN 'INTERNAL' ELSE 'OTHERS' END AS ACTYPE,"
        strSql += vbCrLf + "  (SELECT TOP 1 TYPENAME FROM " & cnAdminDb & "..ACCTYPE WHERE TYPEID = H.ACTYPE) AS ACTYPE,"
        strSql += vbCrLf + "  PREVILEGEID,"
        strSql += vbCrLf + "  DOORNO,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,"
        strSql += vbCrLf + "  PINCODE,PHONENO,MOBILE,DOBIRTH,ANNIVERSARY,EMAILID,"
        strSql += vbCrLf + "  WEBSITE,"
        strSql += vbCrLf + "  CASE WHEN LEDPRINT = 'Y' THEN 'YES' ELSE 'NO' END AS LEDPRINT,"
        strSql += vbCrLf + "  CASE WHEN TDSFLAG = 'Y' THEN 'YES' ELSE 'NO' END AS TDSFLAG,"
        strSql += vbCrLf + "  TDSPER,"
        strSql += vbCrLf + "  CASE WHEN TCSFLAG = 'Y' THEN 'YES' ELSE 'NO' END AS TCSFLAG,"
        strSql += vbCrLf + "  TCSPER,"
        strSql += vbCrLf + "  ISNULL((SELECT TDSCATNAME FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATID=H.TDSCATID),'') TDSCATEGORY,"
        strSql += vbCrLf + "  CASE WHEN DEPFLAG = 'Y' THEN 'YES' ELSE 'NO' END AS DEPFLAG,"
        strSql += vbCrLf + "  DEPPER,"
        strSql += vbCrLf + "  CASE WHEN OUTSTANDING = 'Y' THEN 'YES' ELSE 'NO' END AS OUTSTANDING,"
        strSql += vbCrLf + "  AUTOGEN,VATEX,"
        strSql += vbCrLf + "  CASE WHEN LOCALOUTST = 'L' THEN 'LOCAL' WHEN LOCALOUTST = 'I' THEN 'IMPORT/EXPORT'  ELSE 'OUTSTATION' END AS LOCALOUTST,"
        strSql += vbCrLf + "  LOCALTAXNO,CENTRALTAXNO,TIN,PAN,BANKNAME,BANKACNO,CONTACTPERSON,CONTACTPNO,INVENTORY"

        strSql += vbCrLf + "  ,GSTNO,CREDITDAYS,CREDITLIMIT,BANKOTHERDET,ISNULL(ACTIVE,'Y')ACTIVE,COMPANYID,POSPAYLIMIT,INTPER"
        strSql += vbCrLf + "  ,ISNULL(COSTID,'') COSTID"
        strSql += vbCrLf + "  ,ACCODE,CRDATE,(SELECT USERNAME + ' [' + CONVERT(VARCHAR(15),USERID) + ']' FROM " & cnAdminDb & "..USERMASTER WHERE USERID=H.USERID) AS USERNAME "
        strSql += vbCrLf + "  ,RELIGION"
        strSql += vbCrLf + "  ,COUNTRY"
        strSql += vbCrLf + "  ,(SELECT STATENAME FROM " & cnAdminDb & "..STATEMAST WHERE STATEID=H.STATEID)STATENAME"
        strSql += vbCrLf + "  ,(SELECT GUARDIAN FROM " & cnAdminDb & "..ACCADDINFO WHERE ACCODE=H.ACCODE)GUARDIAN"
        strSql += vbCrLf + "  ,(SELECT SUPNAME FROM " & cnAdminDb & "..ACCADDINFO WHERE ACCODE=H.ACCODE)SUPNAME"
        strSql += vbCrLf + "  ,(SELECT CHILD1 FROM " & cnAdminDb & "..ACCADDINFO WHERE ACCODE=H.ACCODE)CHILD1"
        strSql += vbCrLf + "  ,(SELECT CHILD2 FROM " & cnAdminDb & "..ACCADDINFO WHERE ACCODE=H.ACCODE)CHILD2"
        strSql += vbCrLf + "  ,(SELECT CHILD3 FROM " & cnAdminDb & "..ACCADDINFO WHERE ACCODE=H.ACCODE)CHILD3"
        strSql += vbCrLf + "  ,(SELECT S_DOB FROM " & cnAdminDb & "..ACCADDINFO WHERE ACCODE=H.ACCODE)S_DOB"
        strSql += vbCrLf + "  ,(SELECT C1_DOB FROM " & cnAdminDb & "..ACCADDINFO WHERE ACCODE=H.ACCODE)C1_DOB"
        strSql += vbCrLf + "  ,(SELECT C2_DOB FROM " & cnAdminDb & "..ACCADDINFO WHERE ACCODE=H.ACCODE)C2_DOB"
        strSql += vbCrLf + "  ,(SELECT C3_DOB FROM " & cnAdminDb & "..ACCADDINFO WHERE ACCODE=H.ACCODE)C3_DOB"
        strSql += vbCrLf + "  ,(SELECT OCCUPATION FROM " & cnAdminDb & "..ACCADDINFO WHERE ACCODE=H.ACCODE)OCCUPATION"
        strSql += vbCrLf + "  ,(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID=(SELECT TOP 1 ISSUEDBY FROM " & cnAdminDb & "..ACCADDINFO WHERE ACCODE=H.ACCODE))ISSUED"
        strSql += vbCrLf + "  ,(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID=(SELECT TOP 1 ENTERED FROM " & cnAdminDb & "..ACCADDINFO WHERE ACCODE=H.ACCODE))ENTERED"
        strSql += vbCrLf + "  ,(SELECT TYPENAME FROM " & cnAdminDb & "..PRIVILEGETYPE WHERE TYPEID=H.PREVILEGETYPEID)PREVILEGETYPE"
        strSql += vbCrLf + "  FROM " & cnAdminDb & "..ACHEAD AS H WHERE ACCODE = '" & tempAcCode & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        funcNew()
        With dt.Rows(0)

            If .Item("GUARDIAN").ToString <> "" Then
                ChkAddInfo.Checked = True
                frmAc.editflag = True
                frmAc.txtGuardian.Text = .Item("GUARDIAN").ToString
                frmAc.txtSupName.Text = .Item("SUPNAME").ToString
                frmAc.txtChild1.Text = .Item("CHILD1").ToString
                frmAc.txtChild2.Text = .Item("CHILD2").ToString
                frmAc.txtChild3.Text = .Item("CHILD3").ToString
                frmAc.txtOccupation.Text = .Item("OCCUPATION").ToString
                strSql = "select Empname from " & cnAdminDb & "..EMPMASTER order by Empname"
                objGPack.FillCombo(strSql, frmAc.CmbEnteredBy, False)
                objGPack.FillCombo(strSql, frmAc.CmbIssuedBy, False)
                frmAc.CmbIssuedBy.Text = .Item("ISSUED").ToString
                frmAc.CmbEnteredBy.Text = .Item("ENTERED").ToString
                If .Item("S_DOB").ToString <> "" Then
                    frmAc.dtpDOB.Value = .Item("S_DOB").ToString
                    frmAc.Dob = .Item("S_DOB").ToString
                    frmAc.dtpDOB.Enabled = True
                    frmAc.Chkdb1.Checked = True
                End If
                If .Item("C1_DOB").ToString <> "" Then
                    frmAc.dtpC1DOB.Value = .Item("C1_DOB").ToString
                    frmAc.C1Dob = .Item("C1_DOB").ToString
                    frmAc.dtpC1DOB.Enabled = True
                    frmAc.Chkdb2.Checked = True
                End If
                If .Item("C2_DOB").ToString <> "" Then
                    frmAc.dtpC2DOB.Value = .Item("C2_DOB").ToString
                    frmAc.C2Dob = .Item("C2_DOB").ToString
                    frmAc.dtpC2DOB.Enabled = True
                    frmAc.CheckBox2.Checked = True
                End If
                If .Item("C3_DOB").ToString <> "" Then
                    frmAc.dtpC3DOB.Value = .Item("C3_DOB").ToString
                    frmAc.C3Dob = .Item("C3_DOB").ToString
                    frmAc.dtpC3DOB.Enabled = True
                    frmAc.CheckBox3.Checked = True
                End If
            End If
            cmbCountry_OWN.Text = .Item("COUNTRY").ToString
            cmbState_OWN.Text = .Item("STATENAME").ToString
            lblAcCode.Text = .Item("ACCODE").ToString
            lblCrBy.Text = .Item("USERNAME").ToString
            cmbGroup_Man.Text = .Item("AcGrpName").ToString
            cmbAddressTitle.Text = .Item("TITLE").ToString
            txtAddressInitial.Text = .Item("INITIAL").ToString
            txtName_MAN.Text = .Item("AcName").ToString
            cmbAccountType.Text = .Item("AcType").ToString
            txtDoorNo.Text = .Item("DoorNo").ToString
            txtAddress1.Text = .Item("Address1").ToString
            txtAddress2.Text = .Item("Address2").ToString
            txtAddress3.Text = .Item("Address3").ToString
            cmbArea_OWN.Text = .Item("Area").ToString
            cmbCity_OWN.Text = .Item("City").ToString
            txtPincode.Text = .Item("Pincode").ToString
            txtPhoneNo.Text = .Item("PhoneNo").ToString
            txtMobile.Text = .Item("Mobile").ToString
            editMobileNo = .Item("Mobile").ToString
            txtEmailId.Text = .Item("EmailId").ToString
            txtWebSite.Text = .Item("WebSite").ToString
            If .Item("DOBIRTH").ToString <> "" Then
                dtpDOB.Value = .Item("DOBIRTH").ToString
                chkDOB.Checked = True
            End If
            If .Item("ANNIVERSARY").ToString <> "" Then
                dtpADate.Value = .Item("ANNIVERSARY").ToString
                chkADate.Checked = True
            End If
            If .Item("CRDATE").ToString <> "" Then
                dtpCr.Value = .Item("CRDATE").ToString
                dtpCr.Enabled = False
            End If
            cmbLedgerPrint.Text = .Item("LedPrint").ToString
            cmbTdsFlag.Text = .Item("TdsFlag").ToString
            txtTdsPer_PER.Text = .Item("TdsPer").ToString
            cmbTcsFlag.Text = .Item("TcsFlag").ToString
            txtTcsPer_PER.Text = .Item("TcsPer").ToString
            cmbTDSCategory.Text = .Item("TDSCATEGORY").ToString
            cmbDepFlag.Text = .Item("DepFlag").ToString
            txtDepPer_PER.Text = .Item("DepPer").ToString
            cmbOutStanding.Text = .Item("Outstanding").ToString
            Dim LocalOut As String = Nothing
            LocalOut = .Item("LocalOutSt").ToString
            cmbLocalOuts.Text = LocalOut
            txtLocalTaxNo.Text = .Item("localTaxNo").ToString
            txtCentralTaxNo.Text = .Item("CentralTaxNo").ToString

            txtTINno.Text = .Item("tin").ToString
            txtPANno.Text = .Item("pan").ToString
            txtBankName.Text = .Item("bankname").ToString
            txtBankAccNo.Text = .Item("bankacno").ToString
            txtContactPer.Text = .Item("contactperson").ToString
            txtContactPerDet.Text = .Item("contactpno").ToString
            If .Item("PREVILEGEID").ToString <> "" Then
                chkPrevilege.Checked = True
                txtPrevilegeId.Text = .Item("PREVILEGEID").ToString
            Else
                chkPrevilege.Checked = False
                txtPrevilegeId.Clear()
            End If
            If .Item("INVENTORY").ToString = "Y" Then
                chkInventory.Checked = True
            Else
                chkInventory.Checked = False
            End If

            txtGSTNo.Text = .Item("GSTNO").ToString
            txtCrDays_NUM.Text = .Item("CREDITDAYS").ToString
            txtCrLimit_AMT.Text = .Item("CREDITLIMIT").ToString
            txtBankOtherDet.Text = .Item("BANKOTHERDET").ToString
            txtPoslimit.Text = .Item("POSPAYLIMIT").ToString
            txtInt_PER.Text = .Item("INTPER").ToString
            cmbActive.Text = IIf(.Item("ACTIVE").ToString = "Y", "YES", "NO")
            If RELIGION Then
                If .Item("RELIGION").ToString = "H" Then
                    CmbReligion.Text = "Hindu"
                ElseIf .Item("RELIGION").ToString = "M" Then
                    CmbReligion.Text = "Muslim"
                ElseIf .Item("RELIGION").ToString = "C" Then
                    CmbReligion.Text = "Christian"
                ElseIf .Item("RELIGION").ToString = "S" Then
                    CmbReligion.Text = "Sikh"
                Else
                    CmbReligion.Text = "Others"
                End If
            End If

            If .Item("PREVILEGETYPE").ToString.Trim <> "" Then
                cmbPrivilegeType.Text = .Item("PREVILEGETYPE").ToString.Trim
                chkPrevilege.Checked = True
            End If

            Dim selCompId As New List(Of String)
            For Each s As String In .Item("COMPANYID").ToString().Split(",")
                selCompId.Add(s.Replace("'", ""))
            Next

            If .Item("COMPANYID").ToString() = "" Then
                chkCmbCompany.SetItemChecked(0, True)
            Else
                For cnt As Integer = 0 To dtCompany.Rows.Count - 1
                    If selCompId.Contains(dtCompany.Rows(cnt).Item("COMPANYID").ToString) Then
                        chkCmbCompany.SetItemChecked(cnt, True)
                    Else
                        chkCmbCompany.SetItemChecked(cnt, False)
                    End If
                Next
            End If

            Dim selCostid As New List(Of String)
            For Each s As String In .Item("COSTID").ToString().Split(",")
                selCostid.Add(s.Replace("'", ""))
            Next

            If .Item("COSTID").ToString <> "" Then
                For cnt As Integer = 0 To dtCostcentre.Rows.Count - 1
                    If selCostid.Contains(dtCostcentre.Rows(cnt).Item("COSTID").ToString) Then
                        chkcmbCostcentre.SetItemChecked(cnt, True)
                    Else
                        chkcmbCostcentre.SetItemChecked(cnt, False)
                    End If
                Next
            Else
                chkCmbCompany.SetItemChecked(0, True)
            End If
            strSql = "SELECT CASE WHEN DEBIT > CREDIT THEN 'Dr' ELSE 'Cr' END AMTTYPE, CASE WHEN DEBIT > CREDIT THEN DEBIT ELSE CREDIT END AMT FROM " & cnStockDb & "..OPENTRAILBALANCE"
            strSql += vbCrLf + "  WHERE COMPANYID = '" & cnCompanyId & "' AND COSTID = '" & cnCostId & "' AND ACCODE = '" & tempAcCode & "'"
            Dim dtOutStanding As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtOutStanding)
            If dtOutStanding.Rows.Count > 0 Then
                cmbOpBal.Text = dtOutStanding.Rows(0).Item("AMTTYPE").ToString
                txtOpBalAmt_AMT.Text = dtOutStanding.Rows(0).Item("AMT").ToString
            End If

            strSql = "  SELECT "
            strSql += vbCrLf + "   	 TRANNO,TRANDATE"
            strSql += vbCrLf + "   	,CASE WHEN (RECPAY = 'P' AND PAYMODE = 'DP') OR (RECPAY = 'R' AND PAYMODE = 'DR') THEN 'DUE'"
            strSql += vbCrLf + "   	      WHEN (RECPAY = 'R' AND PAYMODE = 'DP') OR (RECPAY = 'R' AND PAYMODE = 'DR') THEN 'DUE'"
            strSql += vbCrLf + "   	      WHEN (RECPAY = 'P' AND PAYMODE = 'DP') OR (RECPAY = 'R' AND PAYMODE = 'DR') THEN 'DUE'"
            strSql += vbCrLf + "   	 END REFTYPE,REFNO,RUNNO,AMOUNT,REMARK1 REMARK"
            strSql += vbCrLf + "   FROM " & cnAdminDb & "..OUTSTANDING WHERE COMPANYID = '" & cnCompanyId & "' AND COSTID = '" & cnCostId & "' AND ACCODE = '" & tempAcCode & "' AND FROMFLAG = 'O'"
            objOutstDet.dtGridView = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(objOutstDet.dtGridView)
            objOutstDet.gridView.DataSource = objOutstDet.dtGridView
            objOutstDet.gridView.ColumnHeadersVisible = False
            If objOutstDet.gridView.RowCount > 0 Then
                With objOutstDet.gridView
                    objOutstDet.UpdateFlag = True
                    .Columns("TRANNO").Width = objOutstDet.txtTranNo_NUM_MAN.Width + 1
                    .Columns("TRANNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("TRANDATE").Width = objOutstDet.dtpTranDate.Width + 1
                    .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
                    .Columns("REFTYPE").Width = objOutstDet.cmbMode.Width + 1
                    .Columns("REFNO").Width = objOutstDet.txtRefNo_NUM_MAN.Width + 1
                    .Columns("REFNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("RUNNO").Width = objOutstDet.txtRunno.Width + 1
                    .Columns("AMOUNT").Width = objOutstDet.txtAmount_AMT_MAN.Width + 1
                    .Columns("AMOUNT").DefaultCellStyle.Format = "##,##,##,###.00"
                    .Columns("AMOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("REMARK").Width = objOutstDet.txtRemark.Width
                End With
            End If
        End With
        flagSave = True
        acCode = tempAcCode
        Return 0
    End Function

    Private Sub cmbTdsFlag_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTdsFlag.SelectedIndexChanged
        If cmbTdsFlag.Text = "NO" Or cmbTdsFlag.Text = "FORM-G" Then
            txtTdsPer_PER.Enabled = False
            cmbTDSCategory.Enabled = False
        Else
            txtTdsPer_PER.Enabled = True
            cmbTDSCategory.Enabled = True
        End If
    End Sub

    Private Sub cmbTcsFlag_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTcsFlag.SelectedIndexChanged
        If cmbTcsFlag.Text = "NO" Then
            txtTcsPer_PER.Enabled = False
        Else
            txtTcsPer_PER.Enabled = True
        End If
    End Sub
    Private Sub cmbDepFlag_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbDepFlag.SelectedIndexChanged
        If cmbDepFlag.Text = "NO" Then
            txtDepPer_PER.Enabled = False
        Else
            txtDepPer_PER.Enabled = True
        End If
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        funcOpen()
    End Sub

    Private Sub txtPincode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPincode.KeyPress
        Dim keyChar As String
        keyChar = e.KeyChar
        If (Not Char.IsDigit(keyChar)) And AscW(keyChar) <> 8 And AscW(keyChar) <> 13 Then
            e.Handled = True
            MsgBox("Digits only Allowed 1 to 9")
            txtPincode.Focus()
        End If
    End Sub

    Private Sub txtPhoneNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPhoneNo.KeyPress
        Dim keyChar As String
        keyChar = e.KeyChar
        If (Not Char.IsDigit(keyChar)) And AscW(keyChar) <> 8 And AscW(keyChar) <> 13 And AscW(keyChar) <> 45 And AscW(keyChar) <> 43 Then
            e.Handled = True
            MsgBox("Digits only Allowed 1 to 9")
            txtPhoneNo.Focus()
        End If
    End Sub

    Private Sub txtMobile_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMobile.KeyPress
        Dim keyChar As String
        keyChar = e.KeyChar
        If (Not Char.IsDigit(keyChar)) And AscW(keyChar) <> 8 And AscW(keyChar) <> 13 And AscW(keyChar) <> 45 And AscW(keyChar) <> 43 Then
            e.Handled = True
            MsgBox("Digits only Allowed 1 to 9")
            txtMobile.Focus()
        ElseIf keyChar = Chr(Keys.Enter) Then
            If txtMobile.Text <> "" Then
                If Val(objGPack.GetSqlValue("SELECT 1 FROM " & cnAdminDb & "..ACHEAD WHERE MOBILE = '" & txtMobile.Text & "' AND ISNULL(PREVILEGEID,'') <> ''")) = 1 Then
                    Dim MsgStr As String
                    MsgStr = " Previlege Id Already Created In This Mobile NO "
                    MsgStr += vbCrLf + "           Previlege ID : " + objGPack.GetSqlValue("SELECT PREVILEGEID FROM " & cnAdminDb & "..ACHEAD WHERE MOBILE = '" & txtMobile.Text & "' AND ISNULL(PREVILEGEID,'') <> ''").ToString
                    MsgStr += vbCrLf + "Do you want to create?"
                    If MsgBox(MsgStr, MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                        txtMobile.Focus()
                        txtMobile.SelectAll()
                        Exit Sub
                    Else
                        dupCheck = False
                        SendKeys.Send("{TAB}")
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub txtEmailId_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtEmailId.LostFocus
        If txtEmailId.Text <> "" Then
            'Dim pattern As String = "^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$"
            Dim pattern As String = "^((\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)\s*[;,.]{0,1}\s*)+$"
            Dim match As System.Text.RegularExpressions.Match
            match = Regex.Match(txtEmailId.Text.Trim(), pattern, RegexOptions.IgnoreCase)
            If Not match.Success Then
                MsgBox("Enter valid Email address")
                txtEmailId.Focus()
            End If
        End If
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub btnOpenSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOpenSearch.Click
        funcFillGrid()
        gridView.Focus()
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblStatus.Visible = True
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            If gridView.Rows.Count > 0 Then
                e.Handled = True
                funcGetDetails(gridView.Item(0, gridView.CurrentRow.Index).Value.ToString)
                tabMain.SelectedTab = tabGeneral
                cmbGroup_Man.Focus()
            End If
        End If
    End Sub

    Private Sub cmbSubGroup_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        e.KeyChar = UCase(e.KeyChar)
    End Sub

    Private Sub txtName_MAN_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtName_MAN.GotFocus
        DgvSearch.Location = New Point(txtName_MAN.Location.X, txtName_MAN.Location.Y + txtName_MAN.Height)
        DgvSearch.Size = New Size(txtName_MAN.Size.Width, 150)
    End Sub

    Private Sub txtName_MAN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtName_MAN.KeyDown
        If Keys.Insert Then
        ElseIf Keys.Down Then
            'If DgvSearch.Visible Then
            '    If DgvSearch.RowCount > 0 Then
            '        DgvSearch.CurrentCell = DgvSearch.Rows(0).Cells(DgvSearch.FirstDisplayedCell.ColumnIndex)
            '        DgvSearch.Select()
            '    End If
            'Else
            '    DownRow()
            'End If
        ElseIf Keys.Up Then
            If DgvSearch.Visible Then
            Else
                'UpperRow()
            End If
        ElseIf Keys.Delete Then
            e.Handled = True
            Exit Sub
            txtName_MAN.Clear()
        ElseIf Keys.Enter Then
            'KeyEnter(e)
            Exit Sub
            'e.Handled = True
        End If
    End Sub
    'Private Sub UpperRow()
    '    txtName_MAN.SelectAll()
    'End Sub

    'Private Sub DownRow()
    '    If txtName_MAN.Text.Trim = "" Then Exit Sub
    '    If DgvSearch.Visible Then
    '        DgvSearch.Focus()
    '    End If
    'End Sub

    Private Sub txtName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtName_MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If objGPack.DupChecker(txtName_MAN, "SELECT 1 FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & txtName_MAN.Text & "' AND ACCODE <> '" & acCode & "'") Then
                txtName_MAN.Focus()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub chkDOB_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDOB.CheckedChanged
        dtpDOB.Enabled = chkDOB.Checked
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        lblStatus.Visible = False

    End Sub

    Private Sub gridView_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.RowEnter
        objGPack.TextClear(grpInfo)
        With gridView.Rows(e.RowIndex)
            detPrevilegeId.Text = .Cells("PREVILEGEID").Value.ToString
            detDoorNo.Text = .Cells("DOORNO").Value.ToString
            detAddress1.Text = .Cells("ADDRESS1").Value.ToString
            detAddress2.Text = .Cells("ADDRESS2").Value.ToString
            detAddress3.Text = .Cells("ADDRESS3").Value.ToString
            detArea.Text = .Cells("AREA").Value.ToString
            detCity.Text = .Cells("CITY").Value.ToString
            detPincode.Text = .Cells("PINCODE").Value.ToString
            detPhone.Text = .Cells("PHONENO").Value.ToString
            detMobile.Text = .Cells("MOBILE").Value.ToString
            detEmailId.Text = .Cells("EMAILID").Value.ToString
            detWebSite.Text = .Cells("WEBSITE").Value.ToString
            detDob.Text = .Cells("DOB").Value.ToString
            detLedgerPrint.Text = .Cells("LEDGPRINT").Value.ToString
            detTdsFlag.Text = .Cells("TDSFLAG").Value.ToString
            detTdsPer.Text = .Cells("TDSPER").Value.ToString
            detDepFlag.Text = .Cells("DEPFLAG").Value.ToString
            detDepPer.Text = .Cells("DEPPER").Value.ToString
            detOutstanding.Text = .Cells("OUTSTANDING").Value.ToString
            detLocOut.Text = .Cells("LOCAL OUTST").Value.ToString
            detLocTaxNo.Text = .Cells("LOCALTAXNO").Value.ToString
            detCenTaxNo.Text = .Cells("CENTRALTAXNO").Value.ToString

            detTinNo.Text = .Cells("TIN").Value.ToString
            detPanNo.Text = .Cells("PAN").Value.ToString
            detBankName.Text = .Cells("BANKNAME").Value.ToString
            detBankAccNo.Text = .Cells("BANKACNO").Value.ToString
            detContPerson.Text = .Cells("CONTACTPERSON").Value.ToString
            detPersonDet.Text = .Cells("CONTACTPNO").Value.ToString
        End With
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub

        Dim delKey As String = gridView.Rows(gridView.CurrentRow.Index).Cells("ACCODE").Value.ToString
        Dim chkQry As String = "SELECT TOP 1 ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & delKey & "' AND ISNULL(AUTOGENERATOR,'') = 'A'"
        chkQry += " UNION"
        chkQry += " SELECT ACCODE FROM " & cnAdminDb & "..CREDITCARD WHERE ACCODE = '" & delKey & "'"
        chkQry += " UNION"
        chkQry += " SELECT DEFAULTAC FROM " & cnAdminDb & "..CREDITCARD WHERE DEFAULTAC = '" & delKey & "'"
        chkQry += " UNION "
        chkQry += " SELECT TOP 1 ACCODE FROM " & cnAdminDb & "..PERSONALINFO WHERE ACCODE = '" & delKey & "'"
        chkQry += " UNION "
        chkQry += " SELECT TOP 1 ACCODE FROM " & cnAdminDb & "..OUTSTANDING WHERE ACCODE = '" & delKey & "'"
        chkQry += " UNION "
        strSql = " SELECT DBNAME FROM " & cnAdminDb & "..DBMASTER WHERE DBNAME IN(SELECT NAME FROM MASTER..SYSDATABASES)"
        Dim dtDb As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtDb)
        If dtDb.Rows.Count > 0 Then
            For cnt As Integer = 0 To dtDb.Rows.Count - 1
                With dtDb.Rows(cnt)
                    chkQry += " SELECT TOP 1 ACCODE FROM " & .Item("DBNAME").ToString & "..ISSUE WHERE ACCODE = '" & delKey & "'"
                    chkQry += " UNION "
                    chkQry += " SELECT TOP 1 ACCODE FROM " & .Item("DBNAME").ToString & "..RECEIPT WHERE ACCODE = '" & delKey & "'"
                    chkQry += " UNION "
                    chkQry += " SELECT TOP 1 ACCODE FROM " & .Item("DBNAME").ToString & "..ACCTRAN WHERE ACCODE = '" & delKey & "'"
                    chkQry += " UNION "
                    chkQry += " SELECT TOP 1 ACCODE FROM " & .Item("DBNAME").ToString & "..OPENTRAILBALANCE WHERE ACCODE = '" & delKey & "'"
                    chkQry += " UNION "
                    chkQry += " SELECT TOP 1 ACCODE FROM " & .Item("DBNAME").ToString & "..OPENWEIGHT WHERE ACCODE = '" & delKey & "'"
                    chkQry += " UNION "
                    chkQry += " SELECT TOP 1 ACCODE FROM " & .Item("DBNAME").ToString & "..OPENITEM WHERE ACCODE = '" & delKey & "'"
                    chkQry += " UNION "
                    chkQry += " SELECT TOP 1 ACCODE FROM " & .Item("DBNAME").ToString & "..ACCLEDGERDEFAULTVAL WHERE ACCODE = '" & delKey & "'"
                    chkQry += " UNION "
                    chkQry += " SELECT TOP 1 ACCODE FROM " & .Item("DBNAME").ToString & "..ESTISSUE WHERE ACCODE = '" & delKey & "'"
                    chkQry += " UNION "
                    chkQry += " SELECT TOP 1 ACCODE FROM " & .Item("DBNAME").ToString & "..ESTRECEIPT WHERE ACCODE = '" & delKey & "'"
                    'chkQry += " UNION "
                    'chkQry += " SELECT TOP 1 ACCODE FROM " & .Item("DBNAME").ToString & "..ESTPERSONALINFO WHERE ACCODE = '" & delKey & "'"
                    If cnt <> dtDb.Rows.Count - 1 Then
                        chkQry += " UNION "
                    End If
                End With
            Next
            DeleteItem(SyncMode.Master, chkQry, "DELETE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & delKey & "' AND ISNULL(AUTOGENERATOR,'') = ''")
            funcFillGrid()

        End If
    End Sub

    Private Sub cmbGroup_Man_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbGroup_Man.SelectedIndexChanged
        If cmbGroup_Man.SelectedIndex <> -1 Then
            If dtAcGroup.Rows(cmbGroup_Man.SelectedIndex).Item("ACMAINCODE").ToString = "5" Then
                'cmbAccountType.Enabled = True
                cmbAccountType.Text = "SMITH"
            ElseIf dtAcGroup.Rows(cmbGroup_Man.SelectedIndex).Item("ACMAINCODE").ToString = "6" Then
                'cmbAccountType.Enabled = True
                cmbAccountType.Text = "CUSTOMER"
            Else
                'cmbAccountType.Enabled = False
                cmbAccountType.Text = "OTHERS"
            End If
        End If
    End Sub

    Private Sub chkPrevilege_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkPrevilege.CheckedChanged
        If chkPrevilege.Checked Then
            If flagSave = False Or (flagSave = True And txtPrevilegeId.Text.Trim = "") Then
                Dim preId As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PRE-PREFIX'")
                preId += Val(objGPack.GetSqlValue("SELECT ISNULL(CONVERT(BIGINT,CTLTEXT),0)+1 FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PRE-NO'")).ToString
                txtPrevilegeId.Text = preId
            End If
            txtPrevilegeId.Enabled = True
            txtPrevilegeId.Visible = True
            lblPrevilege.Enabled = True
            lblPrevilege.Visible = True
            cmbPrivilegeType.Enabled = True
            cmbPrivilegeType.Visible = True
        Else
            txtPrevilegeId.Enabled = False
            lblPrevilege.Enabled = False
            cmbPrivilegeType.Enabled = False
            If flagSave = False Then txtPrevilegeId.Clear()
            If flagSave = False Then cmbPrivilegeType.Text = ""
        End If
    End Sub

    Private Sub txtPrevilegeId_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPrevilegeId.GotFocus
        If preLock Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub cmbAddressTitle_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbAddressTitle.KeyPress
        If cmbAddressTitle.Text.Length >= 10 Then e.Handled = True
    End Sub

    Private Sub funcLoadCompany()
        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY "
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", True, "ALL")
    End Sub
    Private Sub funcLoadCountry()
        strSql = " SELECT COUNTRYNAME FROM " & cnAdminDb & "..COUNTRYMAST "
        strSql += " ORDER BY COUNTRYNAME"
        objGPack.FillCombo(strSql, cmbCountry_OWN, , False)
        cmbCountry_OWN.SelectedIndex = 0
    End Sub
    Private Sub funcLoadState()
        strSql = " SELECT STATENAME FROM " & cnAdminDb & "..STATEMAST "
        strSql += " ORDER BY STATENAME"
        objGPack.FillCombo(strSql, cmbState_OWN, , False)

        strSql = "SELECT STATENAME FROM " & cnAdminDb & "..STATEMAST WHERE STATEID=" & CompanyStateId
        cmbState_OWN.Text = objGPack.GetSqlValue(strSql, "STATENAME", "")
    End Sub
    Private Sub funcLoadArea()
        strSql = "  SELECT DISTINCT AREANAME FROM " & cnAdminDb & "..AREAMAST "
        If cmbState_OWN.Text <> "" Then
            strSql += " WHERE CITYID IN(SELECT CITYID FROM " & cnAdminDb & "..CITYMAST WHERE STATEID IN(SELECT STATEID FROM " & cnAdminDb & "..STATEMAST WHERE STATENAME='" & cmbState_OWN.Text & "'))"
        End If
        strSql += " ORDER BY AREANAME"
        Dim dtArea As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtArea)
        cmbArea_OWN.DataSource = dtArea
        cmbArea_OWN.DisplayMember = "AREANAME"
        'BrighttechPack.GlobalMethods.FillCombo(cmbArea_OWN, dtArea, "AREANAME", True)
    End Sub


    Private Sub txtOpBalAmt_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtOpBalAmt_AMT.KeyPress
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            If Val(txtOpBalAmt_AMT.Text) = 0 Then
                Me.SelectNextControl(txtOpBalAmt_AMT, True, True, True, True)
                Exit Sub
            End If
            Dim debit As Double = 0
            Dim credit As Double = 0
            If cmbOpBal.Text = "Dr" Then
                debit = txtOpBalAmt_AMT.Text
            Else
                credit = txtOpBalAmt_AMT.Text
            End If
            Dim amt As Double = IIf(debit > 0,
        debit,
        credit)
            objOutstDet.amt = amt
            objOutstDet.type = IIf(debit > 0, frmOpeningTrailBalOutStDt.EntryType.Debit, frmOpeningTrailBalOutStDt.EntryType.Credit)
            If cmbOutStanding.Text = "YES" Then
                If objOutstDet.ShowDialog = Windows.Forms.DialogResult.OK Then
                    insOutstdt = True
                End If
            End If
            Me.SelectNextControl(txtOpBalAmt_AMT, True, True, True, True)
        End If
    End Sub

    Private Sub cmbOutStanding_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbOutStanding.SelectedIndexChanged
        If cmbOutStanding.Text = "YES" Then
            txtOpBalAmt_AMT.Enabled = True
        Else
            txtOpBalAmt_AMT.Enabled = False
        End If
    End Sub

    Private Sub cmbArea_OWN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbArea_OWN.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cmbArea_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbArea_OWN.SelectedIndexChanged
        If cmbArea_OWN.Text.Trim <> "" Then
            strSql = "SELECT PINCODE,C.CITYNAME FROM " & cnAdminDb & "..AREAMAST A "
            strSql += " LEFT JOIN " & cnAdminDb & "..CITYMAST C ON A.CITYID=C.CITYID "
            strSql += " WHERE A.AREANAME='" & cmbArea_OWN.Text & "'"
            Dim Dtarea As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(Dtarea)
            If Dtarea.Rows.Count > 0 Then
                cmbCity_OWN.Text = Dtarea.Rows(0).Item("CITYNAME").ToString
                txtPincode.Text = Dtarea.Rows(0).Item("PINCODE").ToString
            End If
        Else
            cmbCity_OWN.Text = ""
            txtPincode.Text = ""
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkLedger.CheckedChanged
        cmbLedger.Enabled = chkLedger.Checked
        If chkLedger.Checked Then
            Dim AcgrId As String = objGPack.GetSqlValue("SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME = '" & cmbGroup_Man.Text & "'")
            strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD AS H WHERE ISNULL(MACCODE,'') = '' AND ACGRPCODE ='" & AcgrId & "'"
            Dim dtledger As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtledger)
            BrighttechPack.GlobalMethods.FillCombo(cmbLedger, dtledger, "ACNAME", True)
        End If
    End Sub

    Private Sub txtPANno_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPANno.Leave
        If PANNOFORMAT <> "" And txtPANno.Text <> "" Then
            If Not formatchk(PANNOFORMAT, txtPANno.Text.Trim) Then
                MsgBox("Pan No format(" & PANNOFORMAT & ")should not match", MsgBoxStyle.Information)
                txtPANno.Focus()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub txtName_MAN_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtName_MAN.LostFocus
        DgvSearch.Visible = False
    End Sub

    Private Sub txtName_MAN_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtName_MAN.TextChanged
        If txtName_MAN.Focused = False Then Exit Sub
        If txtName_MAN.Text = "" Then
            DgvSearch.Visible = False
            Exit Sub
        Else
            DgvSearch.Visible = True
        End If
        Dim sw As String = txtName_MAN.Text
        Dim RowFilterStr As String  'VouchFilteration
        'If RowFilterStr <> Nothing Then RowFilterStr += " AND "
        RowFilterStr += "ACNAME LIKE '%" & sw.Replace("%", "") & "%'"
        dtAccNames.DefaultView.RowFilter = RowFilterStr
    End Sub

    Private Sub DgvSearch_CellPainting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles DgvSearch.CellPainting

        If e.RowIndex >= 0 And e.ColumnIndex >= 0 Then

            e.Handled = True
            e.PaintBackground(e.CellBounds, True)
            Dim sw As String = txtName_MAN.Text
            If Not String.IsNullOrEmpty(sw) Then
                'highlight search word

                Dim val As String = DirectCast(e.FormattedValue, String)

                Dim sindx As Integer = val.ToLower.IndexOf(sw.ToLower)
                If sindx >= 0 Then
                    'search word found

                    Dim sf As Font = New Font(e.CellStyle.Font, FontStyle.Bold)
                    Dim sbr As SolidBrush = New SolidBrush(Color.Red)

                    Dim br As SolidBrush
                    If e.State = DataGridViewElementStates.Selected Then
                        br = New SolidBrush(e.CellStyle.SelectionForeColor)
                    Else
                        br = New SolidBrush(e.CellStyle.ForeColor)
                    End If

                    Dim sBefore As String = val.Substring(0, sindx)
                    Dim sBeforeSize As SizeF = e.Graphics.MeasureString(sBefore, e.CellStyle.Font, e.CellBounds.Size)
                    Dim sWord As String = val.Substring(sindx, sw.Length)
                    Dim sWordSize As SizeF = e.Graphics.MeasureString(sWord, sf, e.CellBounds.Size)
                    Dim sAfter As String = val.Substring(sindx + sw.Length, val.Length - (sindx + sw.Length))

                    e.Graphics.DrawString(sBefore, e.CellStyle.Font, br, e.CellBounds)
                    e.Graphics.DrawString(sWord, sf, sbr, e.CellBounds.X + sBeforeSize.Width, e.CellBounds.Location.Y)
                    e.Graphics.DrawString(sAfter, e.CellStyle.Font, br, e.CellBounds.X + sBeforeSize.Width + sWordSize.Width, e.CellBounds.Location.Y)
                Else
                    'paint as usual
                    e.PaintContent(e.CellBounds)
                End If
            Else
                'paint as usual
                e.PaintContent(e.CellBounds)
            End If
        End If
    End Sub

    Private Sub DgvSearch_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DgvSearch.KeyDown
        If e.KeyCode = Keys.Up Then
            If DgvSearch.CurrentRow Is Nothing Then Exit Sub
            If DgvSearch.CurrentRow.Index = 0 Then
                txtName_MAN.Select()
            End If
        ElseIf e.KeyCode = Keys.Enter Then
            If DgvSearch.CurrentRow Is Nothing Then Exit Sub
            e.Handled = True
            txtName_MAN.Text = DgvSearch.CurrentRow.Cells("ACNAME").Value.ToString
            DgvSearch.Visible = False
            txtName_MAN.Select()
            txtName_MAN_KeyDown(Me, e)
        End If
    End Sub

    Private Sub txtCrLimit_AMT_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCrLimit_AMT.Leave
        If DUELIMIT_USER_AUTH = "Y" Then
            Dim pwdstring As String : Dim pwdpass As Boolean = False
            Dim Sqlqry As String = "Select Optionid from " & cnAdminDb & "..PRJPWDOPTION where OPTIONNAME ='CREDITBILL' AND active = 'Y'"
            Dim Optionid As Integer = Val("" & objGPack.GetSqlValue(Sqlqry, , , tran))
            If Optionid = 0 Then Exit Sub
            If userId = 999 Then Exit Sub
            strSql = "SELECT VALUEVALUE FROM " & cnAdminDb & "..USERAUTHORIZE WHERE AUSERID = " & userId & " and valueid=" & Optionid
            Dim limitvalue As Double = Val(objGPack.GetSqlValue(strSql).ToString)
            If limitvalue > 0 Then
                If Check_Is_Authrorize(Val(txtCrLimit_AMT.Text), Val(txtCrLimit_AMT.Text), 0, 0, limitvalue) = False Then txtCrLimit_AMT.Text = "" : Exit Sub
            End If
        End If
    End Sub

    Private Sub cmbCity_OWN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbCity_OWN.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub chkADate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkADate.CheckedChanged
        dtpADate.Enabled = chkADate.Checked
    End Sub

    Private Sub txtGSTNo_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtGSTNo.Leave
        If GSTNOFORMAT <> "" And txtGSTNo.Text <> "" Then
            If Not formatchkGST(GSTNOFORMAT, txtGSTNo.Text.Trim) Then
                MsgBox("GSTIN format(" & GSTNOFORMAT & ")should not match", MsgBoxStyle.Information)
                txtGSTNo.Focus()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub cmbState_OWN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbState_OWN.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cmbState_OWN_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbState_OWN.SelectedIndexChanged
        funcLoadArea()
    End Sub

    Private Sub cmbCountry_OWN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbCountry_OWN.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 And tabMain.SelectedTab.Name = tabView.Name Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "HEAD OF ACCOUNT DETAILS", gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub
    Private Sub ChkAddInfo_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkAddInfo.Leave
        If ChkAddInfo.Checked Then
            If frmAc.Visible = True Then Exit Sub
            If frmAc.ShowDialog = Windows.Forms.DialogResult.OK Then
                AddInfo = True
            End If
        End If
    End Sub



    ''Private Sub txtMobile_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMobile.Leave
    ''    If txtMobile.Text = "" Then
    ''        Exit Sub
    ''    End If
    ''    If Val(objGPack.GetSqlValue("SELECT 1 FROM " & cnAdminDb & "..ACHEAD WHERE MOBILE = '" & txtMobile.Text & "' AND ISNULL(PREVILEGEID,'') <> ''")) = 1 Then
    ''        Dim MsgStr As String
    ''        MsgStr = " Previlege Id Already Created In This Mobile NO "
    ''        MsgStr += vbCrLf + "           Previlege ID : " + objGPack.GetSqlValue("SELECT PREVILEGEID FROM " & cnAdminDb & "..ACHEAD WHERE MOBILE = '" & txtMobile.Text & "' AND ISNULL(PREVILEGEID,'') <> ''").ToString
    ''        MsgBox(MsgStr)
    ''        txtMobile.Focus()
    ''        txtMobile.SelectAll()
    ''    End If
    ''End Sub
End Class