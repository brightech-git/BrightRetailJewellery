Imports System.Data.OleDb
Imports System.IO
Public Class frmAddressDia
    '190113 VASANTH CLIENT NAME - DAR
    Dim cmd As OleDbCommand
    Dim strSql As String
    Public _NameMan As Boolean
    Public editBatchno As String
    Public CentCustInfo As Boolean = False
    Dim editBilldate As Date
    Public xBilldate As Date
    Dim editCostId As String
    Dim dtpersonal As New DataTable
    Dim FrmSize As New Size(409, 652)
    Dim GrpSize As New Size(390, 627)
    Public PersonalinfoInsert As Boolean = False
    Public PanMand As Boolean = False
    Dim dtachead As New DataTable
    Private lAddressLock As Boolean = False
    Private LckControls As Boolean = False
    Public EstSno As String = ""
    Private RegularSno As Boolean = True
    Dim dtSoftKeyss As DataTable
    Dim Addvalid As String = ""
    Dim Denyvalid As String = ""
    Dim cradv As Boolean
    Dim Giftiss As Boolean = False
    Dim cardcode As String
    Public _MiscAcode As Boolean = False
    Public _TranTypeCol As New List(Of String)
    Dim _DRSMAINTAIN As String = GetAdmindbSoftValue("DRSMAINTAIN", "N")
    Dim _SHOWTRANS As String = GetAdmindbSoftValue("SHOWPARTYTRANS", "N")
    Dim IS_USERLEVELPWD As Boolean = IIf(GetAdmindbSoftValue("USERLEVELPWD", "N") = "Y", True, False)
    Dim GETDUEDAYS As Boolean = IIf(GetAdmindbSoftValue("DUEDAYS_POS", "N") = "Y", True, False)
    Dim ADDAREAVSPINCHK As Boolean = IIf(GetAdmindbSoftValue("ADDAREAVSPINCHK ", "N") = "Y", True, False)
    Public _DUEAMOUNT As Double
    Dim defalutDestination As String = Nothing
    Dim picExtension As String = Nothing
    Dim picPath As String = Nothing

    Dim CustpicExtension As String = Nothing
    Dim CustpicPath As String = Nothing

    Dim defalutSourcePath As String = Nothing
    Dim ADDMOB_RESTRICT As String = "N"
    Public _IS_USERLEVELPWD As Boolean
    Public GLEDGERCODE As String = ""

    Dim UpdateAchead As Boolean = False
    Dim PER_INFO_LOCK As Boolean = IIf(GetAdmindbSoftValue("PER_INFO_LOCK", "N") = "Y", True, False)
    Dim ADDRESSUPDATE As String = GetAdmindbSoftValue("ADDRESSUPDATE", "N")
    Dim ACHEAD_LOCK As Boolean = IIf(GetAdmindbSoftValue("ACHEAD_LOCK", "N") = "Y", True, False)
    Dim Sms_Send As Boolean = IIf(GetAdmindbSoftValue("SMS_SEND", "N") = "Y", True, False)
    Dim RELIGION As Boolean = IIf(GetAdmindbSoftValue("RELIGION", "N") = "Y", True, False)
    Dim PER_VALID_FOCUS As Boolean = IIf(GetAdmindbSoftValue("PER_INFO_FOCUS", "N") = "Y", True, False)
    Dim Validate_DueDate As Boolean = IIf(GetAdmindbSoftValue("VALIDATE_DUEDATE", "N") = "Y", True, False)
    Public Val_DueDate As Boolean = False
    Public _IsCredit As Boolean = False
    Public Tag_Move As Boolean = False
    Public InterBill As Boolean = False
    Public GstFlag As Boolean = False
    Public objSoftKeys As New SoftKeys
    Public PANNOFORMAT As String = GetAdmindbSoftValue("PANNOFORMAT", "N")
    Public _Sal_value, _Tcs_Value As Double
    Dim POS_CUST_IMAGE As Boolean = IIf(GetAdmindbSoftValue("POS_CUST_IMAGE", "N") = "Y", True, False)
    Public EST_ADD_REQ As Boolean = False
    Dim POS_PRIVILEGE_UPDATE As Boolean = IIf(GetAdmindbSoftValue("POS_PRIVILEGE_UPDATE", "N") = "Y", True, False)
    Public GSTNOFORMAT As String = GetAdmindbSoftValue("GSTNOFORMAT", "")
    Dim _SHOWTRANSMOB As Boolean = IIf(GetAdmindbSoftValue("SHOWPARTYDUEAPPISS_MOBILE", "N") = "Y", True, False)
    Dim _SHOWTRANSADVMOB As Boolean = IIf(GetAdmindbSoftValue("SHOWPARTYADVANCE_MOBILE", "N") = "Y", True, False)
    Dim isbillingchk As Boolean = False
    Dim BOUNZ_SALES As Boolean = IIf(GetAdmindbSoftValue("BOUNZ_SALES", "N") = "Y", True, False)
    Dim Mand_Partycode As Boolean = IIf(GetAdmindbSoftValue("MAND_PARTYCODE", "N") = "Y", True, False)
    Dim _einvtype As String = ""
    Public _SezBill As Boolean = False
    Public _NRIBill As Boolean = False
    Public _ExportBill As Boolean = False
    Dim ADDRESS_SHOWALL_MOBILE As Boolean = IIf(GetAdmindbSoftValue("ADDRESS_SHOWALL_MOBILENOSEARCH", "N") = "Y", True, False)


    Public Property AddressLock() As Boolean
        Get
            Return lAddressLock
        End Get
        Set(ByVal value As Boolean)
            lAddressLock = value
            lockControls(lAddressLock)
            lblNewCurstomer.Visible = Not value
            lblRegularCust.Visible = Not value
            lblClear.Visible = Not value
        End Set
    End Property
    Public Sub New(ByVal Start As Boolean)
        InitializeComponent()
        isbillingchk = False
    End Sub
    Public Sub New(ByVal Giftissue As String)
        FrmSize = New Size(409, 652)
        GrpSize = New Size(390, 627)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        isbillingchk = False
        PersonalinfoInsert = False
        Dim CTLIDS As String = "Where ctlid in ('HIDE-ACCBALANCE','PRE-PREFIX','HIDE-NAMESEARCH','PER_INFO_VALID','ADDRCHECK','GVPREFIX','POS-ACTYPECHK','PER_INFO_VALID_CREDIT')"
        dtSoftKeyss = GetAdmindbSoftValueAll(CTLIDS)
        editBilldate = Now.Date
        Me.editCostId = cnCostId
        'Dim timestr As String
        'strSql = " SELECT ACCODE,PREVILEGEID,TITLE,INITIAL,ACNAME as PNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3"
        'strSql += vbcrlf + " ,AREA,CITY,STATE,COUNTRY,PINCODE,PHONENO PHONERES,MOBILE,EMAILID,'0' AS SNO FROM " & cnAdminDb & "..ACHEAD "
        'strSql += vbcrlf + "UNION ALL SELECT  ACCODE,PREVILEGEID,TITLE,INITIAL,PNAME,DOORNO"
        'strSql += vbcrlf + " ,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,STATE,COUNTRY,PINCODE,PHONERES,MOBILE,EMAIL,SNO FROM " & cnAdminDb & "..PERSONALINFO WHERE ISNULL(AREA,'') <> ''"
        'timestr = Now
        ' da = New OleDbDataAdapter(strSql, cn)
        ' da.Fill(dtachead)
        'timestr = timestr & vbCrLf & Now

        If GetAdmindbSoftValuefromDt(dtSoftKeyss, "ADDRCHECK", "Y") = "N" Then
            strSql = " EXEC " & cnAdminDb & "..SP_GETADDRESS @DBNAME='" & cnAdminDb & "',@MERGE='Y',@EMPTYROW='Y'"
        Else
            strSql = " EXEC " & cnAdminDb & "..SP_GETADDRESS @DBNAME='" & cnAdminDb & "',@MERGE='N',@EMPTYROW='N'"
        End If

        'timestr = timestr & vbCrLf & Now
        cmd = New OleDbCommand(strSql, cn) ': cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)
        Dim dss As New DataSet
        da.Fill(dss)
        dtachead = dss.Tables(0)
        If GetAdmindbSoftValuefromDt(dtSoftKeyss, "ADDRCHECK", "Y") = "N" Then
            strSql = " SELECT COUNTRYNAME FROM " & cnAdminDb & "..COUNTRY ORDER BY COUNTRYNAME"
            'objGPack.FillCombo(strSql, cmbAddressCountry_OWN, , False)
            strSql = " SELECT AREANAME FROM " & cnAdminDb & "..AREAMAST ORDER BY AREANAME"
            objGPack.FillCombo(strSql, cmbAddressArea_OWN, , False)
            strSql = " SELECT CITYNAME FROM " & cnAdminDb & "..CITYMAST ORDER BY CITYNAME"
            'objGPack.FillCombo(strSql, cmbAddressCity_OWN, , False)
        Else
            Dim dtarea As DataTable = dtachead.DefaultView.ToTable(True, "AREA")
            objGPack.FillCombonew(dtarea, cmbAddressArea_OWN, , False)
            Dim dtcity As DataTable = dtachead.DefaultView.ToTable(True, "CITY")
            objGPack.FillCombonew(dtcity, cmbAddressCity_OWN, , False)
            Dim dtcountry As DataTable = dtachead.DefaultView.ToTable(True, "COUNTRY")
            objGPack.FillCombonew(dtcountry, cmbAddressCountry_OWN, , False)
        End If
        strSql = " SELECT STATENAME FROM " & cnAdminDb & "..STATEMAST ORDER BY STATENAME"
        objGPack.FillCombo(strSql, cmbAddressState, , False)
        objGPack.FillCombo(strSql, cmbPlaceOfSupply, , False)
        objGPack.TextClear(Me)
        objGPack.Validator_Object(Me)
        btnOk.Visible = True
        btnOk.Enabled = True
        btnCancel.Visible = True
        btnCancel.Enabled = True
        dtpAddressDueDate.Enabled = False
        lblAddressDueDate.Enabled = False
        txtAddressDueDays_NUM.Enabled = False
        ' Add any initialization after the InitializeComponent() call.

        '  Dim dtpersonal As DataTable = dtachead.DefaultView.ToTable(False, "SNO", "ACCODE", "PNAME", "DOORNO+ADDRESS1+ADDRESS2+AREA", "ADDRESS3", "CITY", "PINCODE", "STATE", "COUNTRY", "PHONERES", "MOBILE", "EMAIL")


        strSql = " EXEC " & cnAdminDb & "..SP_GETADDRESS @DBNAME='" & cnAdminDb & "',@MERGE='Y',@EMPTYROW='" + GetAdmindbSoftValuefromDt(dtSoftKeyss, "HIDE-NAMESEARCH", "Y") + "'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)
        Dim dps As New DataSet
        da.Fill(dps)
        dtpersonal = dps.Tables(0)

        'strSql = "SELECT SNO,ACCODE,PNAME,DOORNO+ADDRESS1+ADDRESS2+AREA,ADDRESS3,CITY,PINCODE,STATE,COUNTRY, "
        'strSql += vbcrlf + " PHONERES,MOBILE,EMAIL,FAX  FROM " & cnAdminDb & "..PERSONALINFO"
        'strSql += vbcrlf + " WHERE 1<>1"
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dtpersonal)

        DGVPersonalInfo.DataSource = dtpersonal

        DGVPersonalInfo.Columns(0).Visible = False
        DGVPersonalInfo.Columns(1).Width = 70
        DGVPersonalInfo.Columns(2).Width = 200
        DGVPersonalInfo.Columns(2).HeaderText = "NAME"
        DGVPersonalInfo.Columns(3).Width = 330
        DGVPersonalInfo.Columns(3).HeaderText = "ADDRESS"
        DGVPersonalInfo.Columns("ADDRESS3").Visible = False
        DGVPersonalInfo.Columns("PINCODE").Visible = False
        DGVPersonalInfo.Columns(7).Visible = False
        DGVPersonalInfo.Columns(8).Visible = False
        DGVPersonalInfo.Columns(9).Visible = False
        DGVPersonalInfo.Columns(10).Visible = False
        DGVPersonalInfo.Columns(11).Visible = False
        DGVPersonalInfo.Columns(12).Visible = False
        DGVPersonalInfo.Columns("PHONERES").Visible = True
        DGVPersonalInfo.Columns("MOBILE").Visible = True
        'HasBalance()
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
            txtAddressDoorNo.Size = New Size(83, 131)
        Else
            lblReligion.Visible = False
            CmbReligion.Visible = False
            txtAddressDoorNo.Size = New Size(83, 153)
        End If
        strSql = "SELECT NAME FROM " & cnAdminDb & "..IDPROOF WHERE ISNULL(ACTIVE,'Y')<>'N' ORDER BY DISPORDER"
        objGPack.FillCombo(strSql, cmbIdType, , True)
        Hasgift()
        PanPersonal.Visible = False
        UpdateAchead = False
    End Sub

    Public Sub New(Optional ByVal isbilling As Integer = 0)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        isbillingchk = IIf(Val(isbilling) = 1, True, False)
        PersonalinfoInsert = False
        Dim CTLIDS As String = "Where ctlid in ('HIDE-ACCBALANCE','PRE-PREFIX','HIDE-NAMESEARCH','PER_INFO_VALID','ADDRCHECK','POS-ACTYPECHK','DRSMAINTAIN')"
        dtSoftKeyss = GetAdmindbSoftValueAll(CTLIDS)

        'Dim timestr As String
        'strSql = " SELECT ACCODE,PREVILEGEID,TITLE,INITIAL,ACNAME as PNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3"
        'strSql += vbcrlf + " ,AREA,CITY,STATE,COUNTRY,PINCODE,PHONENO PHONERES,MOBILE,EMAILID,'0' AS SNO FROM " & cnAdminDb & "..ACHEAD "
        'strSql += vbcrlf + "UNION ALL SELECT  ACCODE,PREVILEGEID,TITLE,INITIAL,PNAME,DOORNO"
        'strSql += vbcrlf + " ,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,STATE,COUNTRY,PINCODE,PHONERES,MOBILE,EMAIL,SNO FROM " & cnAdminDb & "..PERSONALINFO WHERE ISNULL(AREA,'') <> ''"
        'timestr = Now
        ' da = New OleDbDataAdapter(strSql, cn)
        ' da.Fill(dtachead)
        'timestr = timestr & vbCrLf & Now
        Dim PersonlaInfoF1 As Boolean = False
        Dim mxdrsmaintain As String = GetAdmindbSoftValuefromDt(dtSoftKeyss, "DRSMAINTAIN", "N")
        If Mid(mxdrsmaintain, 1, 1) <> "N" Then
            strSql = " EXEC " & cnAdminDb & "..SP_GETADDRESS @DBNAME='" & cnAdminDb & "',@MERGE='N',@EMPTYROW='A'"
            PersonlaInfoF1 = False
        Else
            If GetAdmindbSoftValuefromDt(dtSoftKeyss, "ADDRCHECK", "Y") = "N" Then
                strSql = " EXEC " & cnAdminDb & "..SP_GETADDRESS @DBNAME='" & cnAdminDb & "',@MERGE='Y',@EMPTYROW='Y'"
                PersonlaInfoF1 = True
            ElseIf GetAdmindbSoftValuefromDt(dtSoftKeyss, "ADDRCHECK", "Y") = "P" Then
                strSql = " EXEC " & cnAdminDb & "..SP_GETADDRESS @DBNAME='" & cnAdminDb & "',@MERGE='Y',@EMPTYROW='N'"
                PersonlaInfoF1 = True
            ElseIf GetAdmindbSoftValuefromDt(dtSoftKeyss, "ADDRCHECK", "Y") = "A" Then
                strSql = " EXEC " & cnAdminDb & "..SP_GETADDRESS @DBNAME='" & cnAdminDb & "',@MERGE='N',@EMPTYROW='N'"
                PersonlaInfoF1 = False
            Else
                strSql = " EXEC " & cnAdminDb & "..SP_GETADDRESS @DBNAME='" & cnAdminDb & "',@MERGE='N',@EMPTYROW='Y'"
                PersonlaInfoF1 = False
            End If

        End If

        'timestr = timestr & vbCrLf & Now
        cmd = New OleDbCommand(strSql, cn) ': cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)
        Dim dss As New DataSet
        da.Fill(dss)
        dtpersonal = dss.Tables(0)
        If GetAdmindbSoftValuefromDt(dtSoftKeyss, "ADDRCHECK", "Y") <> "Y" And GetAdmindbSoftValuefromDt(dtSoftKeyss, "ADDRCHECK", "Y") <> "A" Then
            strSql = " SELECT COUNTRYNAME FROM " & cnAdminDb & "..COUNTRY ORDER BY COUNTRYNAME"
            'objGPack.FillCombo(strSql, cmbAddressCountry_OWN, , False)
            strSql = " SELECT AREANAME FROM " & cnAdminDb & "..AREAMAST ORDER BY AREANAME"
            objGPack.FillCombo(strSql, cmbAddressArea_OWN, , False)
            strSql = " SELECT CITYNAME FROM " & cnAdminDb & "..CITYMAST ORDER BY CITYNAME"
            objGPack.FillCombo(strSql, cmbAddressCity_OWN, , False)
        Else
            Dim dtarea As DataTable = dtpersonal.DefaultView.ToTable(True, "AREA")
            objGPack.FillCombonew(dtarea, cmbAddressArea_OWN, , False)
            Dim dtcity As DataTable = dtpersonal.DefaultView.ToTable(True, "CITY")
            objGPack.FillCombonew(dtcity, cmbAddressCity_OWN, , False)
            Dim dtcountry As DataTable = dtpersonal.DefaultView.ToTable(True, "COUNTRY")
            objGPack.FillCombonew(dtcountry, cmbAddressCountry_OWN, , False)
        End If
        strSql = " SELECT STATENAME FROM " & cnAdminDb & "..STATEMAST ORDER BY STATENAME"
        objGPack.FillCombo(strSql, cmbAddressState, , False)
        objGPack.FillCombo(strSql, cmbPlaceOfSupply, , False)
        objGPack.TextClear(Me)
        objGPack.Validator_Object(Me)
        btnOk.Visible = False
        btnOk.Enabled = False
        btnCancel.Visible = False
        btnCancel.Enabled = False
        dtpAddressDueDate.Enabled = False
        lblAddressDueDate.Enabled = False
        txtAddressDueDays_NUM.Enabled = False
        ' Add any initialization after the InitializeComponent() call.

        '  Dim dtpersonal As DataTable = dtachead.DefaultView.ToTable(False, "SNO", "ACCODE", "PNAME", "DOORNO+ADDRESS1+ADDRESS2+AREA", "ADDRESS3", "CITY", "PINCODE", "STATE", "COUNTRY", "PHONERES", "MOBILE", "EMAIL")

        'Dim EmptyRow As String = GetAdmindbSoftValuefromDt(dtSoftKeyss, "HIDE-NAMESEARCH", "Y")
        'If EmptyRow = "Y" Then EmptyRow = IIf(Mid(_DRSMAINTAIN, 1, 1) <> "N", "A", "Y")
        'Dim mergerow As String = IIf(Mid(mxdrsmaintain, 1, 1) <> "N", "N", "Y")
        'If Mid(mxdrsmaintain, 1, 1) <> "N" Then EmptyRow = "A"
        'strSql = " EXEC " & cnAdminDb & "..SP_GETADDRESS @DBNAME='" & cnAdminDb & "',@MERGE='Y',@EMPTYROW='" + GetAdmindbSoftValuefromDt(dtSoftKeyss, "HIDE-NAMESEARCH", "Y") + "'"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'da = New OleDbDataAdapter(cmd)
        'Dim dps As New DataSet
        'da.Fill(dps)
        'dtpersonal = dps.Tables(0)

        'strSql = "SELECT SNO,ACCODE,PNAME,DOORNO+ADDRESS1+ADDRESS2+AREA,ADDRESS3,CITY,PINCODE,STATE,COUNTRY, "
        'strSql += vbcrlf + " PHONERES,MOBILE,EMAIL,FAX  FROM " & cnAdminDb & "..PERSONALINFO"
        'strSql += vbcrlf + " WHERE 1<>1"
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dtpersonal)
        If Mid(_DRSMAINTAIN, 1, 1) <> "N" Then
            Dim xActype As String = GetAdmindbSoftValuefromDt(dtSoftKeyss, "POS-ACTYPECHK", "C,S,D")
            xActype = Replace(xActype, ",", "','")
            strSql = " SELECT ACCODE,PREVILEGEID,TITLE,INITIAL,ACNAME as PNAME,CONVERT(VARCHAR(100),'') AS MNAME,CONVERT(VARCHAR(100),'') AS SNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3"
            strSql += vbCrLf + " ,AREA,CITY,STATE,COUNTRY,PINCODE,PHONENO PHONERES,MOBILE,EMAILID,'0' AS SNO FROM " & cnAdminDb & "..ACHEAD AS H WHERE  ACTYPE IN('" & xActype & "')"
            dtpersonal = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtpersonal)
            PersonlaInfoF1 = False
        End If

        DGVPersonalInfo.DataSource = dtpersonal

        If PersonlaInfoF1 = False Then ' Mid(_DRSMAINTAIN, 1, 1) <> "N" Then
            DGVPersonalInfo.Columns(1).Visible = False
            DGVPersonalInfo.Columns(0).Width = 70
            DGVPersonalInfo.Columns(2).Visible = False
            DGVPersonalInfo.Columns(3).Visible = False
            DGVPersonalInfo.Columns(4).Width = 200
            DGVPersonalInfo.Columns(4).HeaderText = "NAME"
            'DGVPersonalInfo.Columns(3).Width = 330
            DGVPersonalInfo.Columns(3).HeaderText = "ADDRESS"
            DGVPersonalInfo.Columns("ADDRESS3").Visible = False
            DGVPersonalInfo.Columns("PINCODE").Visible = False
            DGVPersonalInfo.Columns(7).Visible = False
            DGVPersonalInfo.Columns(8).Visible = False
            DGVPersonalInfo.Columns(9).Visible = False
            DGVPersonalInfo.Columns(10).Visible = False
            DGVPersonalInfo.Columns(11).Visible = False
            DGVPersonalInfo.Columns(12).Visible = False
            DGVPersonalInfo.Columns("PHONERES").Visible = True
            DGVPersonalInfo.Columns("MOBILE").Visible = True
        Else
            DGVPersonalInfo.Columns(0).Visible = False
            DGVPersonalInfo.Columns(1).Width = 70
            DGVPersonalInfo.Columns(2).Width = 200
            DGVPersonalInfo.Columns(2).HeaderText = "NAME"
            DGVPersonalInfo.Columns(3).Width = 330
            DGVPersonalInfo.Columns(3).HeaderText = "ADDRESS"
            DGVPersonalInfo.Columns("ADDRESS3").Visible = False
            DGVPersonalInfo.Columns("PINCODE").Visible = False
            DGVPersonalInfo.Columns(7).Visible = False
            DGVPersonalInfo.Columns(8).Visible = False
            DGVPersonalInfo.Columns(9).Visible = False
            DGVPersonalInfo.Columns(10).Visible = False
            DGVPersonalInfo.Columns(11).Visible = False
            DGVPersonalInfo.Columns(12).Visible = False
            DGVPersonalInfo.Columns("PHONERES").Visible = True
            DGVPersonalInfo.Columns("MOBILE").Visible = True
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
            txtAddressDoorNo.Size = New Size(83, 131)
        Else
            lblReligion.Visible = False
            CmbReligion.Visible = False
            txtAddressDoorNo.Size = New Size(83, 153)
        End If
        HasBalance()
        PanPersonal.Visible = False
        If ADDRESSUPDATE = "Y" Then
            chkAddchange.Visible = True : chkAddchange.Checked = False : lockControls(True)
        ElseIf ADDRESSUPDATE = "M" Then
            chkAddchange.Visible = True : chkAddchange.Checked = True : lockControls(False)
        ElseIf ADDRESSUPDATE = "N" Then
            chkAddchange.Visible = False : chkAddchange.Checked = False : lockControls(True)
        End If
        UpdateAchead = False
    End Sub

    Public Sub New(ByVal EditBillDate As Date, ByVal EditCostID As String, ByVal EditBatchno As String, ByVal BtnOkReq As Boolean)
        ' This Cons used to Update AddressInfo
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        isbillingchk = False
        Dim CTLIDS As String = "Where ctlid in ('HIDE-ACCBALANCE','PRE-PREFIX','HIDE-NAMESEARCH','PER_INFO_VALID','POS-ACTYPECHK')"
        dtSoftKeyss = GetAdmindbSoftValueAll(CTLIDS)

        strSql = " SELECT ACCODE,PREVILEGEID,TITLE,INITIAL,ACNAME as PNAME,CONVERT(VARCHAR(100),'') AS MNAME,CONVERT(VARCHAR(100),'') AS SNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3"
        strSql += vbCrLf + " ,AREA,CITY,STATE,COUNTRY,PINCODE,PHONENO PHONERES,MOBILE,EMAILID,'0' AS SNO,DOBIRTH AS DOB,ANNIVERSARY FROM " & cnAdminDb & "..ACHEAD "
        strSql += vbCrLf + "UNION ALL SELECT  ACCODE,PREVILEGEID,TITLE,INITIAL,PNAME,MNAME,SNAME,DOORNO"
        strSql += vbCrLf + " ,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,STATE,COUNTRY,PINCODE,PHONERES,MOBILE,EMAIL,SNO,DOB AS DOB,ANNIVERSARY FROM " & cnAdminDb & "..PERSONALINFO WHERE ISNULL(AREA,'') <> ''"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtachead)
        Dim dtarea As DataTable = dtachead.DefaultView.ToTable(True, "AREA")
        objGPack.FillCombonew(dtarea, cmbAddressArea_OWN, , False)

        Dim dtcity As DataTable = dtachead.DefaultView.ToTable(True, "CITY")
        objGPack.FillCombonew(dtcity, cmbAddressCity_OWN, , False)


        strSql = " SELECT STATENAME FROM " & cnAdminDb & "..STATEMAST ORDER BY STATENAME"
        objGPack.FillCombo(strSql, cmbAddressState, , False)
        objGPack.FillCombo(strSql, cmbPlaceOfSupply, , False)

        Dim dtcountry As DataTable = dtachead.DefaultView.ToTable(True, "COUNTRY")
        objGPack.FillCombonew(dtcountry, cmbAddressCountry_OWN, , False)

        'strSql = " SELECT DISTINCT COUNTRY FROM " & cnAdminDb & "..PERSONALINFO WHERE ISNULL(COUNTRY,'') <> ''"
        'strSql += vbcrlf + " ORDER BY COUNTRY"
        'objGPack.FillCombo(strSql, cmbAddressCountry_OWN, , False)
        PersonalinfoInsert = False
        objGPack.TextClear(Me)
        objGPack.Validator_Object(Me)
        Me.editBatchno = EditBatchno
        Me.editBilldate = EditBillDate
        Me.editCostId = EditCostID
        txtRemark1.Visible = True
        lblRemark.Visible = True
        txtRemark1.Enabled = True
        lblRemark.Enabled = True
        dtpAddressDueDate.Visible = False
        lblAddressDueDate.Visible = False
        dtpAddressDueDate.Enabled = False
        lblAddressDueDate.Enabled = False
        txtAddressDueDays_NUM.Enabled = False
        btnOk.Visible = BtnOkReq
        btnCancel.Visible = BtnOkReq
        btnOk.Enabled = BtnOkReq
        btnCancel.Enabled = BtnOkReq
        'btnOk.Location = New Point(btnOk.Location.X, dtpAddressDueDate.Location.Y)
        'btnCancel.Location = New Point(btnCancel.Location.X, dtpAddressDueDate.Location.Y)
        HasBalance()
        UpdateAchead = False
    End Sub

    Public Sub New(ByVal newAccAllow As Boolean, ByVal Remark As Boolean, Optional ByVal btnOkCancel As Boolean = False)
        ''this is for estimation purpose
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        isbillingchk = False
        Dim CTLIDS As String = "Where ctlid in ('HIDE-ACCBALANCE','PRE-PREFIX','ADDRCHECK','HIDE-NAMESEARCH','PER_INFO_VALID','POS-ACTYPECHK')"
        dtSoftKeyss = GetAdmindbSoftValueAll(CTLIDS)

        ' Add any initialization after the InitializeComponent() call.
        'chkCreateNewAcc.Visible = newAccAllow

        objGPack.TextClear(Me)
        objGPack.Validator_Object(Me)
        dtpAddressDueDate.Enabled = False
        lblAddressDueDate.Enabled = False
        txtAddressDueDays_NUM.Enabled = False
        txtRemark1.Visible = Remark
        txtRemark1.Enabled = Remark
        lblRemark.Visible = Remark
        lblRemark.Enabled = Remark
        btnOk.Visible = btnOkCancel
        btnCancel.Visible = btnOkCancel
        btnOk.Enabled = btnOkCancel
        btnCancel.Enabled = btnOkCancel
        lblNewCurstomer.Visible = newAccAllow
        Dim PersonlaInfoF1 As Boolean = False
        Dim mxdrsmaintain As String = GetAdmindbSoftValuefromDt(dtSoftKeyss, "DRSMAINTAIN", "N")
        If mxdrsmaintain <> "N" Then
            strSql = " EXEC " & cnAdminDb & "..SP_GETADDRESS @DBNAME='" & cnAdminDb & "',@MERGE='N',@EMPTYROW='A'"
            PersonlaInfoF1 = False
        Else
            If GetAdmindbSoftValuefromDt(dtSoftKeyss, "ADDRCHECK", "Y") = "N" Then
                strSql = " EXEC " & cnAdminDb & "..SP_GETADDRESS @DBNAME='" & cnAdminDb & "',@MERGE='Y',@EMPTYROW='Y'"
                PersonlaInfoF1 = True
            Else
                strSql = " EXEC " & cnAdminDb & "..SP_GETADDRESS @DBNAME='" & cnAdminDb & "',@MERGE='N',@EMPTYROW='N'"
                PersonlaInfoF1 = False
            End If

        End If

        'timestr = timestr & vbCrLf & Now
        cmd = New OleDbCommand(strSql, cn) ': cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)
        Dim dss As New DataSet
        da.Fill(dss)
        dtpersonal = dss.Tables(0)
        If GetAdmindbSoftValuefromDt(dtSoftKeyss, "ADDRCHECK", "Y") = "N" Then
            strSql = " SELECT COUNTRYNAME FROM " & cnAdminDb & "..COUNTRY ORDER BY COUNTRYNAME"
            'objGPack.FillCombo(strSql, cmbAddressCountry_OWN, , False)
            strSql = " SELECT AREANAME FROM " & cnAdminDb & "..AREAMAST ORDER BY AREANAME"
            objGPack.FillCombo(strSql, cmbAddressArea_OWN, , False)
            strSql = " SELECT CITYNAME FROM " & cnAdminDb & "..CITYMAST ORDER BY CITYNAME"
            'objGPack.FillCombo(strSql, cmbAddressCity_OWN, , False)
        Else
            Dim dtarea As DataTable = dtpersonal.DefaultView.ToTable(True, "AREA")
            objGPack.FillCombonew(dtarea, cmbAddressArea_OWN, , False)
            Dim dtcity As DataTable = dtpersonal.DefaultView.ToTable(True, "CITY")
            objGPack.FillCombonew(dtcity, cmbAddressCity_OWN, , False)
            Dim dtcountry As DataTable = dtpersonal.DefaultView.ToTable(True, "COUNTRY")
            objGPack.FillCombonew(dtcountry, cmbAddressCountry_OWN, , False)
        End If
        strSql = " SELECT STATENAME FROM " & cnAdminDb & "..STATEMAST ORDER BY STATENAME"
        objGPack.FillCombo(strSql, cmbAddressState, , False)
        objGPack.FillCombo(strSql, cmbPlaceOfSupply, , False)

        If Mid(_DRSMAINTAIN, 1, 1) <> "N" Then
            Dim xActype As String = GetAdmindbSoftValuefromDt(dtSoftKeyss, "POS-ACTYPECHK", "C,S,D")
            xActype = Replace(xActype, ",", "','")
            strSql = " SELECT ACCODE,PREVILEGEID,TITLE,INITIAL,ACNAME as PNAME,CONVERT(VARCHAR(100),'') AS MNAME,CONVERT(VARCHAR(100),'') AS SNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3"
            strSql += vbCrLf + " ,AREA,CITY,STATE,COUNTRY,PINCODE,PHONENO PHONERES,MOBILE,EMAILID,'0' AS SNO FROM " & cnAdminDb & "..ACHEAD AS H WHERE  ACTYPE IN('" & xActype & "')"
            dtpersonal = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtpersonal)
            PersonlaInfoF1 = False
        End If

        DGVPersonalInfo.DataSource = dtpersonal

        'If Mid(_DRSMAINTAIN, 1, 1) <> "N" And PersonlaInfoF1 = False Then
        If PersonlaInfoF1 = False Then
            DGVPersonalInfo.Columns(1).Visible = False
            DGVPersonalInfo.Columns(0).Width = 70
            DGVPersonalInfo.Columns(2).Visible = False
            DGVPersonalInfo.Columns(3).Visible = False
            DGVPersonalInfo.Columns(4).Width = 200
            DGVPersonalInfo.Columns(4).HeaderText = "NAME"
            'DGVPersonalInfo.Columns(3).Width = 330
            DGVPersonalInfo.Columns(3).HeaderText = "ADDRESS"
            DGVPersonalInfo.Columns("ADDRESS3").Visible = False
            DGVPersonalInfo.Columns("PINCODE").Visible = False
            DGVPersonalInfo.Columns(7).Visible = False
            DGVPersonalInfo.Columns(8).Visible = False
            DGVPersonalInfo.Columns(9).Visible = False
            DGVPersonalInfo.Columns(10).Visible = False
            DGVPersonalInfo.Columns(11).Visible = False
            DGVPersonalInfo.Columns(12).Visible = False
            DGVPersonalInfo.Columns("PHONERES").Visible = True
            DGVPersonalInfo.Columns("MOBILE").Visible = True
        Else
            DGVPersonalInfo.Columns(0).Visible = False
            DGVPersonalInfo.Columns(1).Width = 70
            DGVPersonalInfo.Columns(2).Width = 200
            DGVPersonalInfo.Columns(2).HeaderText = "NAME"
            DGVPersonalInfo.Columns(3).Width = 330
            DGVPersonalInfo.Columns(3).HeaderText = "ADDRESS"
            DGVPersonalInfo.Columns("ADDRESS3").Visible = False
            DGVPersonalInfo.Columns("PINCODE").Visible = False
            DGVPersonalInfo.Columns(7).Visible = False
            DGVPersonalInfo.Columns(8).Visible = False
            DGVPersonalInfo.Columns(9).Visible = False
            DGVPersonalInfo.Columns(10).Visible = False
            DGVPersonalInfo.Columns(11).Visible = False
            DGVPersonalInfo.Columns(12).Visible = False
            DGVPersonalInfo.Columns("PHONERES").Visible = True
            DGVPersonalInfo.Columns("MOBILE").Visible = True
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
            txtAddressDoorNo.Size = New Size(83, 131)
        Else
            lblReligion.Visible = False
            CmbReligion.Visible = False
            txtAddressDoorNo.Size = New Size(83, 153)
        End If
        strSql = "SELECT NAME FROM " & cnAdminDb & "..IDPROOF WHERE ISNULL(ACTIVE,'Y')<>'N' ORDER BY DISPORDER"
        objGPack.FillCombo(strSql, cmbIdType, , True)
        HasBalance()
        UpdateAchead = False
        'If ADDRESSUPDATE = True Then chkAddchange.Visible = True : chkAddchange.Checked = False
        If ADDRESSUPDATE = "Y" Then
            chkAddchange.Visible = True : chkAddchange.Checked = False
        ElseIf ADDRESSUPDATE = "M" Then
            chkAddchange.Visible = True : chkAddchange.Checked = True
        End If
    End Sub

    Private Sub HasBalance()
        lblBalance.Text = ""
        If GetAdmindbSoftValuefromDt(dtSoftKeyss, "HIDE-ACCBALANCE", "Y") = "Y" Then lblBalance.Visible = False Else lblBalance.Visible = True
    End Sub
    Private Sub Hasgift()
        Giftiss = True
        Me.grpGift.Visible = True
        strSql = " SELECT NAME FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE = 'G' ORDER BY NAME"
        objGPack.FillCombo(strSql, cmbGv_MAN)
        Me.Size = Me.Size + grpGift.Size
        Me.grpAddress.Size = Me.Size

    End Sub

    Private Function GetPartyCode(Optional ByVal tran1 As OleDbTransaction = Nothing) As String
GETNSNO:
        Dim tSno As Integer = 0
        strSql = " SELECT ISNULL(MAX(CTLTEXT),0) AS ACCODE FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'ACCODE'"
        tSno = Val(objGPack.GetSqlValue(strSql, , , tran1))
        If tran1 IsNot Nothing Then
            ''UPDATING 
            ''TAGNO INTO SOFTCONTROL
            strSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT = '" & tSno + 1 & "' "
            strSql += vbCrLf + " WHERE CTLID = 'ACCODE' AND CONVERT(INT,CTLTEXT) = " & tSno & ""
            cmd = New OleDbCommand(strSql, cn, tran1)
            If cmd.ExecuteNonQuery() = 0 Then
                GoTo GETNSNO
            End If
            strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & cnCostId & funcSetNumberStyle((tSno + 1).ToString, 7 - cnCostId.Length) & "'"
            If objGPack.GetSqlValue(strSql, , "-1", tran1) <> "-1" Then
                GoTo GETNSNO
            End If
        End If
        Return cnCostId & funcSetNumberStyle((tSno + 1).ToString, 7 - cnCostId.Length)
    End Function
    Private Sub PanInfoValidation(ByVal Sal_value As Double, ByVal TcsValue As Double)
        If (Sal_value >= objSoftKeys.PANVALUE And objSoftKeys.PANVALUE <> 0) Or Val(TcsValue) <> 0 Then
            If txtAddressPan.Text = "" Then
                If objSoftKeys.PANVALUELOCK = "W" Then
                    If MessageBox.Show("Pan info should not empty" + vbCrLf + "Do you wish to Continue?", "Pan Info Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
                        txtAddressPan.Focus()
                        Exit Sub
                    End If
                ElseIf objSoftKeys.PANVALUELOCK = "R" Then
                    MsgBox("Pan info should not empty", MsgBoxStyle.Information)
                    txtAddressPan.Focus()
                    Exit Sub
                End If
            Else
                If PANNOFORMAT <> "" And objSoftKeys.PANVALUELOCK = "R" Then
                    If Not formatchk(PANNOFORMAT, txtAddressPan.Text.Trim) Then
                        MsgBox("Pan No format (" & PANNOFORMAT & ") Does not match", MsgBoxStyle.Information)
                        txtAddressPan.Focus()
                        Exit Sub
                    End If
                End If
            End If
        End If
    End Sub
    Private Function GetPrevId(Optional ByVal tran1 As OleDbTransaction = Nothing) As String
        If UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PRE-REQ'", , , tran1)) <> "Y" Then
            Return Nothing
        End If
GETNSNO:
        Dim tSno As Integer = 0
        strSql = " SELECT ISNULL(MAX(CTLTEXT),0) AS ACCODE FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PRE-NO'"
        tSno = Val(objGPack.GetSqlValue(strSql, , , tran1))
        If tran1 IsNot Nothing Then
            ''UPDATING 
            ''TAGNO INTO SOFTCONTROL
            strSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT = '" & tSno + 1 & "' "
            strSql += vbCrLf + " WHERE CTLID = 'PRE-NO' AND CONVERT(INT,CTLTEXT) = " & tSno & ""
            cmd = New OleDbCommand(strSql, cn, tran1)
            If cmd.ExecuteNonQuery() = 0 Then
                GoTo GETNSNO
            End If
        End If
        'Dim preId As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PRE-PREFIX'", , , tran1)
        Dim preId As String = GetAdmindbSoftValuefromDt(dtSoftKeyss, "PRE-PREFIX", "")
        preId = cnCostId + preId + (tSno + 1).ToString
        If tran1 IsNot Nothing Then
            strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..ACHEAD WHERE PREVILEGEID = '" & preId & "'"
            If objGPack.GetSqlValue(strSql, , "-1", tran1) <> "-1" Then
                GoTo GETNSNO
            End If
        End If
        Return preId
    End Function

    Public Shared Function GetPersonalInfoSno(ByVal tran1 As OleDbTransaction) As String
GETNSNO:
        Dim tSno As Integer = 0
        Dim strSql As String
        Dim cmd As OleDbCommand
        strSql = " SELECT CTLTEXT AS TAGSNO FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PERSONALINFOCODE'"
        tSno = Val(objGPack.GetSqlValue(strSql, , , tran1))
        ''UPDATING 
        ''TAGNO INTO SOFTCONTROL
        strSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT = '" & tSno + 1 & "' "
        strSql += vbCrLf + " WHERE CTLID = 'PERSONALINFOCODE' AND CONVERT(INT,CTLTEXT) = " & tSno & ""
        cmd = New OleDbCommand(strSql, cn, tran1)
        If cmd.ExecuteNonQuery() = 0 Then
            GoTo GETNSNO
        End If
        strSql = " SELECT SNO FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = '" & GetCostId(cnCostId) & (tSno + 1).ToString & "'"
        If objGPack.GetSqlValue(strSql, , "-1", tran1) <> "-1" Then
            GoTo GETNSNO
        End If
        Return GetCostId(cnCostId) & (tSno + 1).ToString
    End Function

    Private Sub FocusAddress()
        If Addvalid.Contains("N") And txtAddressName.Focused = False And txtAddressName.ReadOnly = False Then txtAddressName.Select() : Exit Sub
        If Addvalid.Contains("DR") And txtAddressDoorNo.Focused = False And txtAddressDoorNo.ReadOnly = False Then txtAddressDoorNo.Select() : Exit Sub
        If Addvalid.Contains("AD1") And txtAddress1.Focused = False And txtAddress1.ReadOnly = False Then txtAddress1.Select() : Exit Sub
        If Addvalid.Contains("AD2") And txtAddress2.Focused And txtAddress2.ReadOnly = False Then txtAddress2.Select() : Exit Sub
        If Addvalid.Contains("AD3") And txtAddress3.Focused = False And txtAddress3.ReadOnly = False Then txtAddress3.Select() : Exit Sub
        If Addvalid.Contains("PC") And txtAddressPincode_NUM.Focused = False And txtAddressPincode_NUM.ReadOnly = False Then txtAddressPincode_NUM.Select() : Exit Sub
        If Addvalid.Contains("AR") And cmbAddressArea_OWN.Focused = False And cmbAddressArea_OWN.Enabled = True Then cmbAddressArea_OWN.Select() : Exit Sub
        If Addvalid.Contains("CY") And cmbAddressCity_OWN.Focused = False And cmbAddressCity_OWN.Enabled = True Then cmbAddressCity_OWN.Select() : Exit Sub
        If Addvalid.Contains("ST") And cmbAddressState.Focused = False And cmbAddressState.Enabled = True Then cmbAddressState.Select() : Exit Sub
        If Addvalid.Contains("ST") And cmbPlaceOfSupply.Focused = False And cmbPlaceOfSupply.Enabled = True Then cmbPlaceOfSupply.Select() : Exit Sub
        If Addvalid.Contains("CN") And cmbAddressCountry_OWN.Focused = False And cmbAddressCountry_OWN.Enabled = True Then cmbAddressCountry_OWN.Select() : Exit Sub
        If Addvalid.Contains("PR") And txtAddressPhoneRes.Focused = False And txtAddressPhoneRes.ReadOnly = False Then txtAddressPhoneRes.Select() : Exit Sub
        If Addvalid.Contains("M") And txtAddressMobile.Focused = False And txtAddressMobile.ReadOnly = False Then txtAddressMobile.Select() : Exit Sub
        If Addvalid.Contains("EL") And txtAddressEmailId_OWN.Focused = False And txtAddressEmailId_OWN.ReadOnly = False Then txtAddressEmailId_OWN.Select() : Exit Sub
        If Addvalid.Contains("FX") And txtAddressFax.Focused = False And txtAddressFax.ReadOnly = False Then txtAddressFax.Select() : Exit Sub
        If Addvalid.Contains("PAN") And txtAddressPan.Focused = False And txtAddressPan.ReadOnly = False Then txtAddressPan.Select() : Exit Sub
    End Sub

    Private Sub DenyFocusAddress()
        If Denyvalid.Contains("IN") Then txtAddressInitial.Enabled = False
        If Denyvalid.Contains("MN") Then txtAddressMName.Enabled = False
        If Denyvalid.Contains("MN") Then cmbAliasName.Enabled = False
        If Denyvalid.Contains("SN") Then txtAddressSName.Enabled = False
        If Denyvalid.Contains("DR") Then txtAddressDoorNo.Enabled = False
    End Sub

    Private Function ValidatateAddress() As Boolean

        If txtAddressName.Text.Trim = "" Then
            MsgBox("Enter the name", MsgBoxStyle.Information)
            txtAddressName.Focus()
            Return False
            Exit Function
        End If

        If txtAddressDueDays_NUM.Enabled And dtpAddressDueDate.Enabled And Validate_DueDate Then
            If GetEntryDate(GetServerDate) >= dtpAddressDueDate.Value Then
                MsgBox("Due date less than/same of Bill date", MsgBoxStyle.Information)
                dtpAddressDueDate.Focus()
                Return False
                Exit Function
            End If
        End If

        'If txtAddressDueDays_NUM.Enabled And dtpAddressDueDate.Enabled And Validate_DueDate Then
        '    If txtAddressDueDays_NUM.Text.ToString = "" Then
        '        MsgBox("Due days should not be empty...", MsgBoxStyle.Information)
        '        txtAddressDueDays_NUM.Focus()
        '        Return False
        '    End If
        'End If
        If txtAddressRegularSno.Text <> "" Then
            If Not objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = '" & txtAddressRegularSno.Text & "'").Length > 0 Then
                MsgBox("Invalid regular customer info", MsgBoxStyle.Information)
                'chkRegularCustomer.Focus()
                Return False
            End If
        End If
        If txtAddressPartyCode.Text <> "" Then
            If Not objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & txtAddressPartyCode.Text & "'").Length > 0 Then
                MsgBox("Invalid Partycode", MsgBoxStyle.Information)
                txtAddressPartyCode.Focus()
                Return False
            End If
        End If
        If chkIdProof.Checked And cmbIdType.Text <> "" Then
            strSql = "SELECT LENGTH,FORMAT FROM " & cnAdminDb & "..IDPROOF WHERE NAME='" & cmbIdType.Text & "'"
            Dim dr As DataRow
            dr = GetSqlRow(strSql, cn, tran)
            Dim Len As Integer = 0
            Dim Format As String = ""
            If Not dr Is Nothing Then
                Format = dr("FORMAT").ToString
                Len = Val(dr("LENGTH").ToString)
            End If
            If Len > 0 Then
                If txtAddressIdNo.Text.Trim.Length <> Len Then
                    MsgBox(cmbIdType.Text & " Must be " & Len & " Characters", MsgBoxStyle.Information)
                    txtAddressIdNo.Select()
                    txtAddressIdNo.Focus()
                    Return False
                End If
            End If
            If Format <> "" Then
                If Not formatchk(Format, txtAddressIdNo.Text.Trim) Then
                    MsgBox(cmbIdType.Text & " (" & Format & ") Does not match", MsgBoxStyle.Information)
                    txtAddressIdNo.Select()
                    txtAddressIdNo.Focus()
                    Return False
                End If
            End If
        End If
        If RELIGION = True Then
            If CmbReligion.Text.Trim = "" Then
                MsgBox("Religion Should not Empty", MsgBoxStyle.Information)
                CmbReligion.SelectAll()
                CmbReligion.Focus()
                Return False
            End If
        End If

        If Addvalid.Contains("N") And txtAddressName.Text.Trim = "" And txtAddressName.ReadOnly = False Then txtAddressName.Focus() : Return False
        If Addvalid.Contains("AD1") And txtAddress1.Text.Trim = "" And txtAddress1.ReadOnly = False Then txtAddress1.Focus() : Return False
        If Addvalid.Contains("PAN") And txtAddressPan.Text.Trim = "" And txtAddressPan.ReadOnly = False Then txtAddressPan.Focus() : Return False
        If Addvalid.Contains("AD2") And txtAddress2.Text.Trim = "" And txtAddress2.ReadOnly = False Then txtAddress2.Focus() : Return False
        If Addvalid.Contains("AD3") And txtAddress3.Text.Trim = "" And txtAddress3.ReadOnly = False Then txtAddress3.Focus() : Return False
        If Addvalid.Contains("DR") And txtAddressDoorNo.Text.Trim = "" And txtAddressDoorNo.ReadOnly = False Then txtAddressDoorNo.Focus() : Return False
        If Addvalid.Contains("EL") And txtAddressEmailId_OWN.Text.Trim = "" And txtAddressEmailId_OWN.ReadOnly = False Then txtAddressEmailId_OWN.Focus() : Return False
        If Addvalid.Contains("FX") And txtAddressFax.Text.Trim = "" And txtAddressFax.ReadOnly = False Then txtAddressFax.Focus() : Return False
        If Addvalid.Contains("M") And txtAddressMobile.Text.Trim = "" And txtAddressMobile.ReadOnly = False Then txtAddressMobile.Focus() : Return False
        If Addvalid.Contains("PR") And txtAddressPhoneRes.Text.Trim = "" And txtAddressPhoneRes.ReadOnly = False Then txtAddressPhoneRes.Focus() : Return False
        If Addvalid.Contains("PC") And txtAddressPincode_NUM.Text.Trim = "" And txtAddressPincode_NUM.ReadOnly = False Then txtAddressPincode_NUM.Focus() : Return False
        If Addvalid.Contains("AR") And cmbAddressArea_OWN.Text.Trim = "" And cmbAddressArea_OWN.Enabled = True Then cmbAddressArea_OWN.Focus() : Return False
        If Addvalid.Contains("CY") And cmbAddressCity_OWN.Text.Trim = "" And cmbAddressCity_OWN.Enabled = True Then cmbAddressCity_OWN.Focus() : Return False
        If Addvalid.Contains("ST") And cmbAddressState.Text.Trim = "" And cmbAddressState.Enabled = True Then cmbAddressState.Focus() : Return False
        If Addvalid.Contains("ST") And cmbPlaceOfSupply.Text.Trim = "" And cmbPlaceOfSupply.Enabled = True Then cmbPlaceOfSupply.Focus() : Return False
        If Addvalid.Contains("CN") And cmbAddressCountry_OWN.Text.Trim = "" And cmbAddressCountry_OWN.Enabled = True Then cmbAddressCountry_OWN.Focus() : Return False

        'If chkCreateNewAcc.Checked And txtAddressName.Text = "" Then
        '    MsgBox("Name should not empty", MsgBoxStyle.Information)
        '    txtAddressName.Focus()
        '    Return False
        'End If
        Return True
    End Function


    Private Sub txtAddressFax_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles txtAddressFax.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If (dtpAddressDueDate.Visible And dtpAddressDueDate.Enabled) _
            Or (txtRemark1.Visible And txtRemark1.Enabled) _
            Or btnOk.Visible Then
                SendKeys.Send("{TAB}")
            Else
                'chkCreateNewAcc.Focus()
                If ValidatateAddress() Then
                    If _IsWholeSaleType Then
                        If txtAddressName.Text <> "" Then Me.DialogResult = Windows.Forms.DialogResult.OK
                        Me.Close()
                    Else
                        Me.DialogResult = Windows.Forms.DialogResult.OK
                        Me.Close()
                    End If
                End If
            End If

        End If
    End Sub
    Private Sub frmAddressDia_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        PanPersonal.Visible = False
    End Sub
    Private Sub AddressDia_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If editBatchno <> "" Then Me.DialogResult = Windows.Forms.DialogResult.Cancel : Me.Close() : Exit Sub
            If btnOk.Visible Then Exit Sub

            'chkCreateNewAcc.Focus()
            If ValidatateAddress() And EST_ADD_REQ = False Then
                If _IsWholeSaleType Then
                    If txtAddressName.Text <> "" Then Me.DialogResult = Windows.Forms.DialogResult.OK
                    Me.Close()
                Else
                    Me.DialogResult = Windows.Forms.DialogResult.OK
                    Me.Close()
                End If
            ElseIf EST_ADD_REQ = True Then
                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            End If
        ElseIf e.KeyCode = Keys.Tab Then

        ElseIf e.KeyCode = Keys.R And e.Control = True And lblRegularCust.Visible Then
            ''REGULAR CUSTOMER
            Dim obj As New frmRegularCustomerSearchDia
            obj.Location = Me.Location
            If obj.ShowDialog = Windows.Forms.DialogResult.OK Then
                strSql = " SELECT SNO,PREVILEGEID,ACCODE,TITLE,INITIAL,PNAME,MNAME,SNAME,DOORNO"
                strSql += vbCrLf + " ,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,PINCODE,STATE,COUNTRY,PHONERES,MOBILE,EMAIL,FAX,PAN"
                strSql += vbCrLf + " ,RELIGION,GSTNO"
                strSql += vbCrLf + " ,(SELECT TOP 1 STATENAME FROM " & cnAdminDb & "..STATEMAST WHERE STATEID=P.STATEID)STATEID"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..PERSONALINFO P"
                strSql += vbCrLf + " WHERE SNO = '" & obj.sno & "'"
                Dim dtAdd As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtAdd)
                If Not dtAdd.Rows.Count > 0 Then Exit Sub
                With dtAdd.Rows(0)
                    txtAddressPrevilegeId.Text = .Item("PREVILEGEID").ToString
                    txtAddressPartyCode.Text = .Item("ACCODE").ToString
                    cmbAddressTitle_OWN.Text = .Item("TITLE").ToString
                    txtAddressInitial.Text = .Item("INITIAL").ToString
                    txtAddressName.Text = .Item("PNAME").ToString
                    txtAddressMName.Text = .Item("MNAME").ToString
                    txtAddressSName.Text = .Item("SNAME").ToString
                    txtAddressDoorNo.Text = .Item("DOORNO").ToString
                    txtAddress1.Text = .Item("ADDRESS1").ToString
                    txtAddress2.Text = .Item("ADDRESS2").ToString
                    txtAddress3.Text = .Item("ADDRESS3").ToString
                    cmbAddressArea_OWN.Text = .Item("AREA").ToString
                    cmbAddressCity_OWN.Text = .Item("CITY").ToString
                    cmbAddressState.Text = .Item("STATE").ToString
                    cmbPlaceOfSupply.Text = .Item("STATEID").ToString
                    cmbAddressCountry_OWN.Text = .Item("COUNTRY").ToString
                    txtAddressPincode_NUM.Text = .Item("PINCODE").ToString
                    txtAddressPhoneRes.Text = .Item("PHONERES").ToString
                    txtAddressMobile.Text = .Item("MOBILE").ToString
                    txtAddressEmailId_OWN.Text = .Item("EMAIL").ToString
                    txtAddressFax.Text = .Item("FAX").ToString
                    txtAddressRegularSno.Text = .Item("SNO").ToString
                    txtAddressPan.Text = .Item("PAN").ToString
                    txtGSTNo.Text = .Item("GSTNO").ToString
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
                    If InterBill = False Then
                        If cmbPlaceOfSupply.Text.ToString.Trim = "" Then cmbPlaceOfSupply.Text = CompanyState
                        If cmbAddressState.Text = "" Then cmbAddressState.Text = CompanyState
                    End If
                    If _NRIBill Then
                        cmbAddressState.Text = ""
                        cmbPlaceOfSupply.Text = ""
                    End If
                    txtAddressPrevilegeId.Select()
                    lockControls(True)
                End With
            End If
        ElseIf e.KeyCode = Keys.N And e.Control = True And lblNewCurstomer.Visible Then
            ''New Customer
            If IS_USERLEVELPWD Then
                Dim mpwdid As Integer
                mpwdid = usrpwdid("POSACCREATE", _IS_USERLEVELPWD)
                If mpwdid = 0 Then MsgBox("Access Denied") : Exit Sub
            End If

            Dim objAcHead As New frmAccountHead
            objAcHead._ExternalCalling = True
            objAcHead.DisableCtrl = ACHEAD_LOCK
            objAcHead.ShowDialog()
            If objAcHead.LastAccode <> "" Then
                txtAddressPartyCode.Text = objAcHead.LastAccode
                strSql = " SELECT PREVILEGEID,TITLE,INITIAL,ACNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3"
                strSql += vbCrLf + " ,AREA,CITY,PINCODE,PHONENO PHONERES,MOBILE,EMAILID,PAN,STATE"
                strSql += vbCrLf + " ,(SELECT TOP 1 STATENAME FROM " & cnAdminDb & "..STATEMAST WHERE STATEID=A.STATEID)STATEID"
                strSql += vbCrLf + " ,RELIGION,GSTNO"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ACHEAD A WHERE ACCODE = '" & txtAddressPartyCode.Text & "'"
                strSql += GetAcNameQryFilteration()
                Dim dt As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    With dt.Rows(0)
                        txtAddressPrevilegeId.Text = .Item("PREVILEGEID").ToString
                        cmbAddressTitle_OWN.Text = .Item("TITLE").ToString
                        txtAddressInitial.Text = .Item("INITIAL").ToString
                        txtAddressName.Text = .Item("ACNAME").ToString
                        txtAddressDoorNo.Text = .Item("DOORNO").ToString
                        txtAddress1.Text = .Item("ADDRESS1").ToString
                        txtAddress2.Text = .Item("ADDRESS2").ToString
                        txtAddress3.Text = .Item("ADDRESS3").ToString
                        cmbAddressArea_OWN.Text = .Item("AREA").ToString
                        cmbAddressCity_OWN.Text = .Item("CITY").ToString
                        cmbAddressState.Text = .Item("STATE").ToString
                        cmbPlaceOfSupply.Text = .Item("STATEID").ToString
                        txtAddressPincode_NUM.Text = .Item("PINCODE").ToString
                        txtAddressPhoneRes.Text = .Item("PHONERES").ToString
                        txtAddressMobile.Text = .Item("MOBILE").ToString
                        txtAddressEmailId_OWN.Text = .Item("EMAILID").ToString
                        txtAddressPan.Text = .Item("PAN").ToString
                        txtGSTNo.Text = .Item("GSTNO").ToString
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
                        lockControls(True)
                    End With
                End If
            End If
        ElseIf e.KeyCode = Keys.F4 And txtMobile.Text.Trim <> "" And txtAddressName.ReadOnly = True Then
            lockControls(False)
            txtAddressRegularSno.Text = ""
        ElseIf e.KeyCode = Keys.F3 And lblClear.Visible Then
            If AddressLock = False Then
                objGPack.TextClear(Me)
                lockControls(False)
                txtAddressPrevilegeId.Select()
            End If
        ElseIf e.KeyCode = Keys.Enter Then
            If txtAddressPartyCode.Focused Then Exit Sub
            If txtAddressPrevilegeId.Focused Then Exit Sub
            If txtAddressFax.Focused Then Exit Sub
            If txtRemark1.Focused And CurrencyDecimal <> 3 Then Exit Sub
            'If txtRemark5.Focused Then Exit Sub
            If txtAddressPan.Focused Then Exit Sub
            If PER_VALID_FOCUS Then
                If ValidatateAddress() = True Then
                    Me.DialogResult = Windows.Forms.DialogResult.OK
                    Me.Close()
                End If
            End If
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmAddressDia_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        'If e.KeyChar = Chr(Keys.Enter) Then
        '    If txtAddressPartyCode.Focused Then Exit Sub
        '    If txtAddressPrevilegeId.Focused Then Exit Sub
        '    If txtAddressFax.Focused Then Exit Sub
        '    If txtRemark1.Focused Then Exit Sub
        '    If txtAddressPan.Focused Then Exit Sub
        '    SendKeys.Send("{TAB}")
        'End If
    End Sub

    Private Sub cmbAddressArea_OWN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbAddressArea_OWN.KeyDown
        If e.KeyCode = Keys.Enter Then
            If cmbAddressArea_OWN.Text = "" Then Exit Sub
            If ADDAREAVSPINCHK Then
                Getarea("A")
            End If
        End If
    End Sub

    Private Sub cmbAddressArea_OWN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbAddressArea_OWN.KeyPress
        If textCharacterCasing = CharacterCasing.Upper Then
            e.KeyChar = UCase(e.KeyChar)
        ElseIf textCharacterCasing = CharacterCasing.Lower Then
            e.KeyChar = LCase(e.KeyChar)
        End If
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbAddressArea_OWN.Text = "" Then Exit Sub
            If ADDAREAVSPINCHK Then
                Getarea("A")
            End If
        End If
    End Sub

    'Private Sub cmb_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles _
    'cmbAddressArea_OWN.KeyPress, cmbAddressCity_OWN.KeyPress, cmbAddressCountry_OWN.KeyPress, cmbAddressState_OWN.KeyPress
    '    If textCharacterCasing = CharacterCasing.Upper Then
    '        e.KeyChar = UCase(e.KeyChar)
    '    ElseIf textCharacterCasing = CharacterCasing.Lower Then
    '        e.KeyChar = LCase(e.KeyChar)
    '    End If
    'End Sub



    Private Sub cmbAddressArea_OWN_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbAddressArea_OWN.KeyUp
        'ComboScript(cmbAddressArea_OWN, e)
    End Sub

    Private Sub cmbAddressCity_OWN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbAddressCity_OWN.KeyPress
        If textCharacterCasing = CharacterCasing.Upper Then
            e.KeyChar = UCase(e.KeyChar)
        ElseIf textCharacterCasing = CharacterCasing.Lower Then
            e.KeyChar = LCase(e.KeyChar)
        End If
    End Sub

    Private Sub cmbAddressCity_OWN_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbAddressCity_OWN.KeyUp
        'ComboScript(cmbAddressCity_OWN, e)
    End Sub

    Private Sub cmbAddressState_OWN_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbAddressState.KeyDown

    End Sub

    Private Sub cmbAddressState_OWN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbAddressState.KeyPress
        If textCharacterCasing = CharacterCasing.Upper Then
            e.KeyChar = UCase(e.KeyChar)
        ElseIf textCharacterCasing = CharacterCasing.Lower Then
            e.KeyChar = LCase(e.KeyChar)
        End If
    End Sub

    Private Sub cmbAddressState_OWN_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbAddressState.KeyUp
        'ComboScript(cmbAddressState_OWN, e)
    End Sub

    Private Sub cmbAddressCountry_OWN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbAddressCountry_OWN.KeyPress
        If textCharacterCasing = CharacterCasing.Upper Then
            e.KeyChar = UCase(e.KeyChar)
        ElseIf textCharacterCasing = CharacterCasing.Lower Then
            e.KeyChar = LCase(e.KeyChar)
        End If
    End Sub

    Private Sub cmbAddressCountry_OWN_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbAddressCountry_OWN.KeyUp
        'ComboScript(cmbAddressCountry_OWN, e)
    End Sub

    Private Sub txtAddressPartyCode_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAddressPartyCode.GotFocus
        'If chkCreateNewAcc.Checked Then
        '    txtAddressPartyCode.Text = GetPartyCode()
        '    SendKeys.Send("{TAB}")
        'End If
    End Sub
    Private Sub txtAddressPartyCode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtAddressPartyCode.KeyUp
        If e.KeyCode = Keys.Insert Then
            If Tag_Move Then Exit Sub
            If AddressLock = True Then Exit Sub
            'If chkRegularCustomer.Checked Then Exit Sub
            Dim xActype As String = GetAdmindbSoftValuefromDt(dtSoftKeyss, "POS-ACTYPECHK", "C,S,D")
            xActype = Replace(xActype, ",", "','")
            strSql = " SELECT ACCODE,ACNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,PINCODE,PHONENO,MOBILE,PAN,EMAILID"
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..ACHEAD AS H WHERE  ACTYPE IN('" & xActype & "')"
            strSql += GetAcNameQryFilteration()
            strSql += vbCrLf + "  ORDER BY CRDATE DESC"
            Dim pCode As String = BrighttechPack.SearchDialog.Show("Search Party Account Code", strSql, cn, 1, , , , , , )
            If pCode <> Nothing Then
                objGPack.TextClear(grpAddress)
                txtAddressPartyCode.Text = pCode
            End If
        End If
    End Sub
    Public Sub SetCustomerBalance(ByVal tran As OleDbTransaction)
        If lblBalance.Visible = False Then Exit Sub
        strSql = vbCrLf + " SELECT "
        strSql += vbCrLf + " SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END)BALANCE"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING"
        strSql += vbCrLf + " WHERE COMPANYID = '" & strCompanyId & "' "
        strSql += vbCrLf + " AND TRANTYPE = 'D'"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        If txtAddressRegularSno.Text <> "" Then
            strSql += vbCrLf + " AND BATCHNO IN (SELECT BATCHNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE PSNO = '" & txtAddressRegularSno.Text & "')"
        Else
            If txtAddressPartyCode.Text <> "" Then strSql += vbCrLf + " AND ACCODE = '" & txtAddressPartyCode.Text & "'" Else strSql += vbCrLf + " AND 1=2"
        End If
        Dim Bal As Decimal = Val(objGPack.GetSqlValue(strSql, "", "", tran))
        Dim tBal As Decimal = Math.Abs(Bal)
        lblBalance.Text = ""
        If Bal <> 0 Then
            lblBalance.Text = "Balance : " & Format(tBal, "0.00") & IIf(Bal > 0, " Dr", " Cr")
        End If
    End Sub
    Private Sub txtAddressPartyCode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAddressPartyCode.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If AddressLock Then
                If chkAddchange.Visible = False Then
                    lockControls(False)
                End If
                SendKeys.Send("{TAB}")
                Exit Sub
            End If
            If txtAddressPartyCode.Text = "" Then

                If _IsWholeSaleType Or _MiscAcode Or Mand_Partycode Then
                    MsgBox("PartyCode should not empty", MsgBoxStyle.Information)
                    txtAddressPartyCode.Focus()
                    Exit Sub
                End If
                SendKeys.Send("{TAB}")
                Exit Sub
            Else
                If Mid(_DRSMAINTAIN, 1, 1) <> "N" Then SetCustomerBalanceGrid() Else SetCustomerBalance(Nothing)
                If Mid(_DRSMAINTAIN, 1, 1) <> "N" Then
                    If Not Validateduelimit() Then
                        If MsgBox("Customer credit limit is exceed" & vbCrLf & "Have OTP to Proceed", MsgBoxStyle.YesNo) = MsgBoxResult.No Then txtAddressPartyCode.Text = "" : txtAddressName.Text = "" : Me.DialogResult = Windows.Forms.DialogResult.Cancel : Exit Sub
                        Dim mpwdid As Integer
                        mpwdid = usrpwdid("EXCESSCREDITALLOW", _IS_USERLEVELPWD)
                        If mpwdid = 0 Then MsgBox("Access Denied") : txtAddressPartyCode.Text = "" : txtAddressName.Text = "" : Me.DialogResult = Windows.Forms.DialogResult.Cancel : Exit Sub
                    End If
                Else
                    If Not Validateduelimit() Then
                        If MsgBox("Customer credit limit is exceed" & vbCrLf & "Can you Proceed ?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then txtAddressPartyCode.Text = "" : txtAddressName.Text = "" : Me.DialogResult = Windows.Forms.DialogResult.Cancel : Exit Sub
                    End If
                    Dim OLDDUE As Double = Oldcreditvalue()
                    If OLDDUE <> 0 Then MsgBox("Customer Previous Balance is Rs." & OLDDUE.ToString, MsgBoxStyle.Information) : MsgBox("Customer Previous Balance is Rs." & OLDDUE.ToString, MsgBoxStyle.Critical)
                End If

            End If

            strSql = " SELECT PREVILEGEID,TITLE,INITIAL,ACNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3"
            strSql += vbCrLf + " ,AREA,CITY,PINCODE,PHONENO PHONERES,MOBILE,EMAILID,PAN"
            strSql += vbCrLf + " ,RELIGION,GSTNO,DOBIRTH,ANNIVERSARY"
            strSql += vbCrLf + " ,(SELECT TOP 1 STATENAME FROM " & cnAdminDb & "..STATEMAST WHERE STATEID=A.STATEID)STATEID"
            strSql += vbCrLf + " , ISNULL(ACTIVE,'') ACTIVE"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ACHEAD A WHERE ACCODE = '" & txtAddressPartyCode.Text & "'"
            strSql += GetAcNameQryFilteration()
            Dim dt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                With dt.Rows(0)
                    txtAddressPrevilegeId.Text = .Item("PREVILEGEID").ToString
                    cmbAddressTitle_OWN.Text = .Item("TITLE").ToString
                    txtAddressInitial.Text = .Item("INITIAL").ToString
                    txtAddressName.Text = .Item("ACNAME").ToString
                    txtAddressDoorNo.Text = .Item("DOORNO").ToString
                    txtAddress1.Text = .Item("ADDRESS1").ToString
                    txtAddress2.Text = .Item("ADDRESS2").ToString
                    txtAddress3.Text = .Item("ADDRESS3").ToString
                    cmbAddressArea_OWN.Text = .Item("AREA").ToString
                    cmbAddressState.Text = .Item("STATEID").ToString
                    cmbAddressCity_OWN.Text = .Item("CITY").ToString
                    txtAddressPincode_NUM.Text = .Item("PINCODE").ToString
                    txtAddressPhoneRes.Text = .Item("PHONERES").ToString
                    txtAddressMobile.Text = .Item("MOBILE").ToString
                    txtAddressEmailId_OWN.Text = .Item("EMAILID").ToString
                    txtAddressPan.Text = .Item("PAN").ToString
                    txtGSTNo.Text = .Item("GSTNO").ToString
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
                    If UpdateAchead = False Then
                        If ADDRESSUPDATE = "M" Then
                            lockControls(False)
                        Else
                            lockControls(True)
                        End If
                    Else
                        lockControls(False)
                    End If

                    If InterBill = False Then
                        If cmbPlaceOfSupply.Text = "" Then cmbPlaceOfSupply.Text = CompanyState
                        If cmbAddressState.Text = "" Then cmbAddressState.Text = CompanyState
                    End If
                    If _NRIBill Then
                        cmbPlaceOfSupply.Text = ""
                        cmbAddressState.Text = ""
                    End If

                    If .Item("DOBIRTH").ToString.Trim <> "" Then
                        chkDOB.Checked = True
                        Dim tempDOBdate As Date = .Item("DOBIRTH").ToString.Trim
                        dtpDob_OWN.Value = tempDOBdate.Date
                    Else
                        chkDOB.Checked = False
                        dtpDob_OWN.Value = GetServerDate()
                    End If
                    If .Item("ANNIVERSARY").ToString.Trim <> "" Then
                        chkAnniversary.Checked = True
                        Dim tempANVdate As Date = .Item("ANNIVERSARY").ToString.Trim
                        dtpAnniv_OWN.Value = tempANVdate.Date
                    Else
                        chkAnniversary.Checked = False
                        dtpAnniv_OWN.Value = GetServerDate()
                    End If

                    If .Item("ACTIVE").ToString = "" Then
                        MsgBox("PartyCode Not Approve", MsgBoxStyle.Information)
                        objGPack.TextClear(grpAddress)
                        txtAddressPartyCode.Select()
                        lockControls(False)
                    End If
                End With
                If txtAddressDoorNo.Enabled = False And txtAddressDoorNo.Text <> "" Then
                    If txtAddress1.Text = "" Then txtAddress1.Text = txtAddressDoorNo.Text
                    txtAddressDoorNo.Text = ""
                End If
                SendKeys.Send("{TAB}")
            Else
                MsgBox("Invalid PartyCode", MsgBoxStyle.Information)
                txtAddressPartyCode.Select()
                lockControls(False)
            End If
        End If
    End Sub

    Private Sub txtAddressPrevilegeId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtAddressPrevilegeId.KeyDown
        If e.KeyCode = Keys.Insert Then
            If AddressLock = True Then Exit Sub
            If Tag_Move Then Exit Sub
            'If chkRegularCustomer.Checked Then Exit Sub
            Dim xActype As String = GetAdmindbSoftValuefromDt(dtSoftKeyss, "POS-ACTYPECHK", "C,S,D")
            xActype = Replace(xActype, ",", "','")
            strSql = " SELECT PREVILEGEID,ACCODE,ACNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,PINCODE,PHONENO,MOBILE"
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..ACHEAD AS H WHERE ISNULL(PREVILEGEID,'') <> '' AND ACTYPE IN('" & xActype & "')"
            strSql += GetAcNameQryFilteration()
            strSql += vbCrLf + "  ORDER BY CRDATE DESC"
            Dim pCode As String = BrighttechPack.SearchDialog.Show("Search Previlege Customer id", strSql, cn, 1, , , , , , )
            If pCode <> Nothing Then
                objGPack.TextClear(grpAddress)
                txtAddressPrevilegeId.Text = pCode
            End If
        End If
    End Sub


    Private Sub txtPrevilegeId_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAddressPrevilegeId.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtAddressPrevilegeId.Text = "" Then
                If chkAddchange.Visible = False Then
                    If PER_INFO_LOCK = False Then lockControls(False)
                End If
                SendKeys.Send("{TAB}")
                Exit Sub
            End If
            strSql = " SELECT ACCODE,TITLE,INITIAL,ACNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3"
            strSql += vbCrLf + " ,AREA,CITY,PINCODE,PHONENO PHONERES,MOBILE,EMAILID,PAN"
            strSql += vbCrLf + " ,RELIGION,GSTNO,DOBIRTH,ANNIVERSARY"
            strSql += vbCrLf + " ,(SELECT TOP 1 STATENAME FROM " & cnAdminDb & "..STATEMAST WHERE STATEID=A.STATEID)STATEID"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ACHEAD A WHERE ISNULL(PREVILEGEID,'') = '" & txtAddressPrevilegeId.Text & "'"
            strSql += GetAcNameQryFilteration()
            Dim dt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                With dt.Rows(0)
                    txtAddressPartyCode.Text = .Item("ACCODE").ToString
                    cmbAddressTitle_OWN.Text = .Item("TITLE").ToString
                    txtAddressInitial.Text = .Item("INITIAL").ToString
                    txtAddressName.Text = .Item("ACNAME").ToString
                    txtAddressDoorNo.Text = .Item("DOORNO").ToString
                    txtAddress1.Text = .Item("ADDRESS1").ToString
                    txtAddress2.Text = .Item("ADDRESS2").ToString
                    txtAddress3.Text = .Item("ADDRESS3").ToString
                    cmbAddressArea_OWN.Text = .Item("AREA").ToString
                    cmbAddressState.Text = .Item("STATEID").ToString
                    cmbAddressCity_OWN.Text = .Item("CITY").ToString
                    txtAddressPincode_NUM.Text = .Item("PINCODE").ToString
                    txtAddressPhoneRes.Text = .Item("PHONERES").ToString
                    txtAddressMobile.Text = .Item("MOBILE").ToString
                    txtAddressEmailId_OWN.Text = .Item("EMAILID").ToString
                    txtAddressPan.Text = .Item("PAN").ToString
                    txtGSTNo.Text = .Item("GSTNO").ToString
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

                    If .Item("DOBIRTH").ToString.Trim <> "" Then
                        chkDOB.Checked = True
                        Dim tempDOBdate As Date = .Item("DOBIRTH").ToString.Trim
                        dtpDob_OWN.Value = tempDOBdate.Date
                    Else
                        chkDOB.Checked = False
                        dtpDob_OWN.Value = GetServerDate()
                    End If
                    If .Item("ANNIVERSARY").ToString.Trim <> "" Then
                        chkAnniversary.Checked = True
                        Dim tempANVdate As Date = .Item("ANNIVERSARY").ToString.Trim
                        dtpAnniv_OWN.Value = tempANVdate.Date
                    Else
                        chkAnniversary.Checked = False
                        dtpAnniv_OWN.Value = GetServerDate()
                    End If

                    lockControls(True)
                    If txtAddressDoorNo.Enabled = False And txtAddressDoorNo.Text <> "" Then
                        If txtAddress1.Text = "" Then txtAddress1.Text = txtAddressDoorNo.Text
                        txtAddressDoorNo.Text = ""
                    End If
                    txtAddressName.Focus()
                End With
            Else
                MsgBox("Invalid PrevilegeId", MsgBoxStyle.Information)
                txtAddressPrevilegeId.Focus()
            End If
        End If
    End Sub

    Private Sub chkCreateNewAcc_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        objGPack.TextClear(grpAddress)
    End Sub

    Private Sub txtAddressName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAddressName.GotFocus
        If txtAddressPartyCode.Text <> "" Or txtAddressPrevilegeId.Text <> "" Then
            If chkAddchange.Checked = True Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cmbAddressTitle_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbAddressTitle_OWN.KeyPress
        If cmbAddressTitle_OWN.Text.Length >= 10 Then e.Handled = True
    End Sub

    Private Sub CmbLock_Gotfocus(ByVal sender As Object, ByVal e As EventArgs) Handles cmbAddressArea_OWN.GotFocus, cmbAddressCity_OWN.GotFocus, cmbAddressState.GotFocus, cmbAddressCountry_OWN.GotFocus, cmbAddressTitle_OWN.GotFocus
        If LckControls Then SendKeys.Send("{TAB}")
    End Sub

    Public Sub lockControls(ByVal bool As Boolean)
        'If bool Then chkCreateNewAcc.Checked = False
        'chkCreateNewAcc.Enabled = Not bool
        LckControls = bool
        txtMobile.ReadOnly = bool
        txtAddressPrevilegeId.ReadOnly = bool
        ''txtAddressPartyCode.ReadOnly = bool
        If chkAddchange.Checked Then
            If GetAdmindbSoftValue("ADD_UPDATE_RESTRICT_ACCODE", "N") = "Y" And txtAddressPartyCode.Text.Trim <> "" Then txtAddressPartyCode.ReadOnly = True Else txtAddressPartyCode.ReadOnly = bool
        Else
            txtAddressPartyCode.ReadOnly = bool
        End If
        txtAddressInitial.ReadOnly = bool
        ''txtAddressName.ReadOnly = bool
        If chkAddchange.Checked Then
            If GetAdmindbSoftValue("ADD_UPDATE_RESTRICT_NAME", "N") = "Y" And txtAddressName.Text.Trim <> "" Then txtAddressName.ReadOnly = True Else txtAddressName.ReadOnly = bool
        Else
            txtAddressName.ReadOnly = bool
        End If
        txtAddressDoorNo.ReadOnly = bool
        txtAddress1.ReadOnly = bool
        txtAddress2.ReadOnly = bool
        txtAddress3.ReadOnly = bool
        txtAddressPincode_NUM.ReadOnly = bool
        txtAddressPhoneRes.ReadOnly = bool
        txtAddressMobile.ReadOnly = bool
        txtAddressEmailId_OWN.ReadOnly = bool
        txtAddressFax.ReadOnly = bool
        If GetAdmindbSoftValue("ADD_UPDATE_RESTRICT_GST", "N") = "Y" And txtGSTNo.Text.Trim <> "" Then txtGSTNo.ReadOnly = True Else txtGSTNo.ReadOnly = bool
        If txtAddressIdNo.Text.Trim() <> "" Then
            chkIdProof.Checked = True
        End If
    End Sub

    Private Sub lockControls(ByVal sender As Object, ByVal e As EventArgs) Handles cmbAddressArea_OWN.GotFocus,
    cmbAddressCity_OWN.GotFocus, cmbAddressCity_OWN.GotFocus, cmbAddressCountry_OWN.GotFocus, cmbAddressState.GotFocus,
    cmbAddressTitle_OWN.GotFocus
        If AddressLock And Not chkAddchange.Checked Then SendKeys.Send("{TAB}")
        'If chkRegularCustomer.Checked Then SendKeys.Send("{TAB}")
    End Sub



    Private Sub txtgvNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtGvNo.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
            Exit Sub
        End If
        If e.KeyCode = Keys.Insert Then


            strSql = " SELECT T.RUNNO,"
            strSql += vbCrLf + " T.AMOUNT AS DENOMINATION"
            strSql += vbCrLf + " ,T.QTY AS QTY"
            strSql += vbCrLf + " ,T.TRANDATE AS DATE,T.DUEDAYS AS DUEDAYS "
            strSql += vbCrLf + " ,V.CARDCODE FROM " & cnStockDb & "..GVTRAN T "
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CREDITCARD AS V ON V.CARDCODE= T.CARDID"
            strSql += vbCrLf + " WHERE T.COSTID = '" & editCostId & "' "
            strSql += vbCrLf + " AND RUNNO NOT IN ("
            strSql += vbCrLf + " SELECT RUNNO"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING O"
            strSql += vbCrLf + " WHERE TRANTYPE = 'GV'"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " AND ISNULL(COSTID,'') = '" & editCostId & "')"
            Dim row As DataRow = Nothing
            row = BrighttechPack.SearchDialog.Show_R("Search Gift Voucher No", strSql, cn, 0, , , , , , , False)
            If Not row Is Nothing Then
                With row
                    cardcode = .Item("CARDCODE").ToString
                    txtGvNo.Text = .Item("RUNNO").ToString
                    If dtpValid.MaxDate <= DateAdd(DateInterval.Day, Val(.Item("DUEDAYS")), editBilldate) Then dtpValid.MaxDate = DateAdd(DateInterval.Day, Val(.Item("DUEDAYS")), editBilldate)
                    dtpValid.Value = DateAdd(DateInterval.Day, Val(.Item("DUEDAYS")), editBilldate)
                    lblValue.Text = IIf(Val(.Item("DENOMINATION").ToString) <> 0, Format(Val(.Item("DENOMINATION").ToString), "0.00"), Nothing)
                    'If .Item("ACCODE").ToString = "" Then
                    'If txtAdvanceNo.Text.StartsWith("O") Then
                    ' txtAdvanceAcCode.Text = "ADVORD"
                    'End If
                    'End If
                    'txtAdvanceCompanyId.Text = .Item("COMPANYID").ToString

                End With
            End If
        End If
    End Sub

    Private Sub txtGvNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtGvNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtGvNo.Text = "" Then
                MsgBox(Me.GetNextControl(txtGvNo, False).Text + E0001, MsgBoxStyle.Information)
                txtGvNo.Focus()
                Exit Sub
            Else
                strSql = " SELECT T.RUNNO,"
                strSql += vbCrLf + " T.AMOUNT AS DENOMINATION"
                strSql += vbCrLf + " ,T.QTY AS QTY"
                strSql += vbCrLf + " ,T.TRANDATE AS DATE,T.DUEDAYS "
                strSql += vbCrLf + " ,V.CARDCODE FROM " & cnStockDb & "..GVTRAN T "
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CREDITCARD AS V ON V.CARDCODE= T.CARDID"
                strSql += vbCrLf + " WHERE T.COSTID = '" & editCostId & "' "
                strSql += vbCrLf + " AND RUNNO ='" & txtGvNo.Text & "'"
                Dim dGdt As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dGdt)
                If dGdt.Rows.Count > 0 Then
                    cardcode = dGdt.Rows(0).Item("CARDCODE").ToString
                    dtpValid.Value = DateAdd(DateInterval.Day, Val(dGdt.Rows(0).Item("DUEDAYS")), editBilldate)
                    lblValue.Text = IIf(Val(dGdt.Rows(0).Item("DENOMINATION").ToString) <> 0, Format(Val(dGdt.Rows(0).Item("DENOMINATION").ToString), "0.00"), Nothing)
                Else
                    MsgBox("Invalid Voucher", MsgBoxStyle.OkOnly)
                    txtGvNo.Focus()
                End If
            End If
        End If
    End Sub
    'Private Sub chkRegularCustomer_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    objGPack.TextClear(grpAddress)
    '    lockControls(chkRegularCustomer.Checked)
    'End Sub

    'Private Sub chkRegularCustomer_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    lockControls(chkRegularCustomer.Checked)
    'End Sub

    Private Sub txtAddressName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtAddressName.KeyDown
        If e.KeyCode = Keys.Down Then
            DGVPersonalInfo.Focus()
        ElseIf e.KeyCode = Keys.Enter Then
            PanPersonal.Visible = False

        End If
    End Sub

    Private Sub txtAddressName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAddressName.KeyPress
        PanPersonal.Location = New Point(6, 131)
        If e.KeyChar = Chr(Keys.Enter) Then
            If _NameMan And txtAddressName.Text = "" Then
                MsgBox("PartyName should not empty", MsgBoxStyle.Information)
                txtAddressName.Select()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub txtAddressPrevilegeId_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAddressPrevilegeId.LostFocus
        SetCustomerBalance(Nothing)
    End Sub

    Private Sub txtAddressMobile_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAddressMobile.GotFocus
        If txtMobile.Text <> "" And txtAddressMobile.Text = "" Then txtAddressMobile.Text = txtMobile.Text

    End Sub



    Private Sub readOnlyChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
        txtAddressPrevilegeId.ReadOnlyChanged,
        txtAddressPartyCode.ReadOnlyChanged,
        txtAddressInitial.ReadOnlyChanged,
        txtAddressName.ReadOnlyChanged,
        txtAddressDoorNo.ReadOnlyChanged,
        txtAddress1.ReadOnlyChanged,
        txtAddress2.ReadOnlyChanged,
        txtAddress3.ReadOnlyChanged,
        txtAddressPincode_NUM.ReadOnlyChanged,
        txtAddressPhoneRes.ReadOnlyChanged,
        txtAddressMobile.ReadOnlyChanged,
        txtAddressEmailId_OWN.ReadOnlyChanged,
        txtAddressFax.ReadOnlyChanged
        CType(sender, Control).BackColor = Color.White
    End Sub
    Public Function InsertIntoPersonalInfo(ByVal billdate As Date, ByVal billCostId As String, ByVal batchno As String, ByVal tran1 As OleDbTransaction _
           , Optional ByVal est As Boolean = False _
           , Optional ByVal _EINVOICETYPE As String = "") As String
        If PersonalinfoInsert = True Then Return ""
        If chkAddchange.Checked Then txtAddressRegularSno.Text = ""
        Dim psno As String = Nothing
        Dim IDIMAGEFILE As String
        Dim CUSTIMAGEFILE As String
        If UpdateAchead = True Then UpdateIntoAchead(tran1)
        If txtAddressRegularSno.Text <> "" Then
            strSql = "SELECT TOP 1 PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO='" & txtAddressRegularSno.Text & "'"
            If objGPack.GetSqlValue(strSql, "PNAME", 0, tran1).ToString <> txtAddressName.Text.ToString Then
                txtAddressRegularSno.Text = ""
            End If
        End If
        If txtAddressRegularSno.Text <> "" Then
            strSql = "SELECT ISNULL(STATEID,0) AS STATEID FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO='" & txtAddressRegularSno.Text & "'"
            Dim _StateId As Integer = Val(objGPack.GetSqlValue(strSql, "STATEID", 0, tran1).ToString)
            If _StateId <> 0 Then
                strSql = "SELECT STATENAME FROM " & cnAdminDb & "..STATEMAST WHERE STATEID=" & _StateId
                Dim _StateName As String = objGPack.GetSqlValue(strSql, "STATENAME", 0, tran1).ToString
                If cmbPlaceOfSupply.Text <> _StateName Then
                    GoTo NewGen
                End If
            End If
        End If
        If (txtAddressRegularSno.Text = "" And txtAddressName.Text <> "") Or editBatchno <> "" Or chkAddchange.Checked Then
            If EstSno <> "" Or (editBatchno <> "" And txtAddressRegularSno.Text <> "") Then
                If EstSno <> "" Then
                    psno = EstSno
                Else
                    psno = txtAddressRegularSno.Text
                End If
                If File.Exists(picPath) = True Then IDIMAGEFILE = "ID" & psno.ToString
                If File.Exists(CustpicPath) = True Then CUSTIMAGEFILE = psno.ToString
                Dim StateId As Integer
                strSql = "SELECT STATEID FROM " & cnAdminDb & "..STATEMAST WHERE STATENAME='" & cmbPlaceOfSupply.Text & "'"
                StateId = Val(objGPack.GetSqlValue(strSql, "STATEID", 0, tran1).ToString)

                strSql = " UPDATE " & cnAdminDb & "..PERSONALINFO SET"
                strSql += vbCrLf + " ACCODE = '" & txtAddressPartyCode.Text & "'" 'ACCODE
                strSql += vbCrLf + " ,TRANDATE = '" & GetEntryDate(billdate, tran1).ToString("yyyy-MM-dd") & "'" 'TRANDATE
                strSql += vbCrLf + " ,TITLE = '" & cmbAddressTitle_OWN.Text & "'" 'TITLE
                strSql += vbCrLf + " ,INITIAL = '" & txtAddressInitial.Text & "'" 'INITIAL
                strSql += vbCrLf + " ,PNAME = '" & txtAddressName.Text & "'" 'PNAME
                strSql += vbCrLf + " ,MNAME = '" & txtAddressMName.Text & "'" 'MNAME
                strSql += vbCrLf + " ,SNAME = '" & IIf(cmbAliasName.Text.Trim <> "", cmbAliasName.Text.Trim & " ", "") & txtAddressSName.Text & "' " 'SNAME
                strSql += vbCrLf + " ,DOORNO = '" & txtAddressDoorNo.Text & "'" 'DOORNO
                strSql += vbCrLf + " ,ADDRESS1 = '" & txtAddress1.Text & "'" 'ADDRESS1
                strSql += vbCrLf + " ,ADDRESS2 = '" & txtAddress2.Text & "'" 'ADDRESS2
                strSql += vbCrLf + " ,ADDRESS3 = '" & txtAddress3.Text & "'" 'ADDRESS3
                strSql += vbCrLf + " ,AREA = '" & cmbAddressArea_OWN.Text & "'" 'AREA
                strSql += vbCrLf + " ,CITY = '" & cmbAddressCity_OWN.Text & "'" 'CITY
                strSql += vbCrLf + " ,STATE = '" & cmbAddressState.Text & "'" 'STATE
                strSql += vbCrLf + " ,COUNTRY = '" & cmbAddressCountry_OWN.Text & "'" 'COUNTRY
                strSql += vbCrLf + " ,PINCODE = '" & txtAddressPincode_NUM.Text & "'" 'PINCODE
                strSql += vbCrLf + " ,PHONERES = '" & txtAddressPhoneRes.Text & "'" 'PHONERES
                strSql += vbCrLf + " ,MOBILE = '" & txtAddressMobile.Text & "'" 'MOBILE
                strSql += vbCrLf + " ,EMAIL = '" & txtAddressEmailId_OWN.Text & "'" 'EMAIL
                strSql += vbCrLf + " ,FAX = '" & txtAddressFax.Text & "'" 'Fax
                strSql += vbCrLf + " ,APPVER = '" & VERSION & "'" 'APPVER
                strSql += vbCrLf + " ,PREVILEGEID = '" & txtAddressPrevilegeId.Text & "'" 'PREVILEGEID
                strSql += vbCrLf + " ,COMPANYID = '" & strCompanyId & "'" 'COMPANYID
                strSql += vbCrLf + " ,COSTID = '" & billCostId & "'" 'COSTID
                strSql += vbCrLf + " ,PAN = '" & txtAddressPan.Text & "'" 'PAN
                If chkIdProof.Checked = True Then
                    strSql += vbCrLf + " ,IDTYPE= '" & cmbIdType.Text & "'" 'PAN
                    strSql += vbCrLf + " ,IDNO = '" & txtAddressIdNo.Text & "'" 'PAN
                    strSql += vbCrLf + " ,IDIMAGEFILE = '" & IDIMAGEFILE & "'" 'PAN
                Else
                    strSql += vbCrLf + ",IDTYPE=''"
                    strSql += vbCrLf + ",IDNO =''"
                    strSql += vbCrLf + ",IDIMAGEFILE = ''"
                End If
                strSql += vbCrLf + " ,PIMAGEFILE = '" & CUSTIMAGEFILE & "'" 'PAN
                If RELIGION Then
                    strSql += vbCrLf + " ,RELIGION='" & Mid(CmbReligion.Text, 1, 1) & "'"
                End If
                If chkDOB.Checked Then
                    strSql += vbCrLf + " ,DOB='" & dtpDob_OWN.Value.Date.ToString("yyyy-MM-dd") & "'"
                End If
                If chkAnniversary.Checked Then
                    strSql += vbCrLf + " ,ANNIVERSARY='" & dtpAnniv_OWN.Value.Date.ToString("yyyy-MM-dd") & "'"
                End If
                strSql += vbCrLf + " ,GSTNO = '" & txtGSTNo.Text & "'" 'GSTNO
                strSql += vbCrLf + " ,STATEID = '" & StateId & "'" 'GSTNO
                strSql += vbCrLf + " WHERE SNO = '" & psno & "'"
                ExecQuery(SyncMode.Master, strSql, cn, tran1, billCostId)
                If cradv Then
                    '190113
                    If txtAddressPartyCode.Text <> "" Then
                        strSql = " UPDATE " & cnAdminDb & "..OUTSTANDING SET"
                        strSql += vbCrLf + " ACCODE = '" & txtAddressPartyCode.Text & "' WHERE PAYMODE IN('DU','AR','MP','MR') AND BATCHNO ='" & editBatchno & "'"
                        ExecQuery(SyncMode.Transaction, strSql, cn, tran1, billCostId)

                        strSql = " UPDATE " & cnStockDb & "..ACCTRAN SET"
                        strSql += vbCrLf + " ACCODE = '" & txtAddressPartyCode.Text & "' WHERE PAYMODE IN('DU','AR','MP','MR') AND BATCHNO = '" & editBatchno & "'"
                        ExecQuery(SyncMode.Transaction, strSql, cn, tran1, billCostId)
                    End If
                End If
                strSql = " UPDATE " & cnStockDb & "..ACCTRAN SET"
                strSql += vbCrLf + " REMARK1 = '" & txtRemark1.Text & "' WHERE BATCHNO = '" & editBatchno & "'"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran1, billCostId)
            Else
                If txtAddressRegularSno.Text <> "" And RegularSno Then
                    psno = txtAddressRegularSno.Text
                Else
NewGen:
                    psno = GetPersonalInfoSno(tran1)
                End If
                If File.Exists(picPath) = True Then IDIMAGEFILE = "ID" & psno.ToString
                If File.Exists(CustpicPath) = True Then CUSTIMAGEFILE = psno.ToString

                Dim StateId As Integer
                strSql = "SELECT STATEID FROM " & cnAdminDb & "..STATEMAST WHERE STATENAME='" & cmbPlaceOfSupply.Text & "'"
                StateId = Val(objGPack.GetSqlValue(strSql, "STATEID", 0, tran1).ToString)

                strSql = " INSERT INTO " & cnAdminDb & "..PERSONALINFO"
                strSql += vbCrLf + " ("
                strSql += vbCrLf + " SNO,ACCODE,TRANDATE,TITLE"
                strSql += vbCrLf + " ,INITIAL,PNAME,MNAME,SNAME,DOORNO,ADDRESS1"
                strSql += vbCrLf + " ,ADDRESS2,ADDRESS3,AREA,CITY"
                strSql += vbCrLf + " ,STATE,COUNTRY,PINCODE,PHONERES"
                strSql += vbCrLf + " ,MOBILE,EMAIL,FAX,APPVER"
                strSql += vbCrLf + " ,PREVILEGEID,COMPANYID,COSTID,PAN"
                strSql += vbCrLf + " ,IDTYPE,IDNO,IDIMAGEFILE,PIMAGEFILE"
                If RELIGION Then
                    strSql += vbCrLf + " ,RELIGION"
                End If
                If chkDOB.Checked Then strSql += vbCrLf + " ,DOB"
                If chkAnniversary.Checked Then strSql += vbCrLf + " ,ANNIVERSARY"
                strSql += vbCrLf + " ,GSTNO,STATEID"
                strSql += vbCrLf + " )"
                strSql += vbCrLf + " VALUES"
                strSql += vbCrLf + " ("
                strSql += vbCrLf + " '" & psno & "'" ''SNO
                strSql += vbCrLf + " ,'" & txtAddressPartyCode.Text & "'" 'ACCODE
                strSql += vbCrLf + " ,'" & GetEntryDate(billdate, tran1).ToString("yyyy-MM-dd") & "'" 'TRANDATE
                strSql += vbCrLf + " ,'" & cmbAddressTitle_OWN.Text & "'" 'TITLE
                strSql += vbCrLf + " ,'" & txtAddressInitial.Text & "'" 'INITIAL
                strSql += vbCrLf + " ,'" & txtAddressName.Text & "'" 'PNAME
                strSql += vbCrLf + " ,'" & txtAddressMName.Text & "'" 'MNAME
                strSql += vbCrLf + " ,'" & IIf(cmbAliasName.Text.Trim <> "", cmbAliasName.Text.Trim & " ", "") & txtAddressSName.Text & "'" 'SNAME
                strSql += vbCrLf + " ,'" & txtAddressDoorNo.Text & "'" 'DOORNO
                strSql += vbCrLf + " ,'" & txtAddress1.Text & "'" 'ADDRESS1
                strSql += vbCrLf + " ,'" & txtAddress2.Text & "'" 'ADDRESS2
                strSql += vbCrLf + " ,'" & txtAddress3.Text & "'" 'ADDRESS3
                strSql += vbCrLf + " ,'" & cmbAddressArea_OWN.Text & "'" 'AREA
                strSql += vbCrLf + " ,'" & cmbAddressCity_OWN.Text & "'" 'CITY
                strSql += vbCrLf + " ,'" & cmbAddressState.Text & "'" 'STATE
                strSql += vbCrLf + " ,'" & cmbAddressCountry_OWN.Text & "'" 'COUNTRY
                strSql += vbCrLf + " ,'" & txtAddressPincode_NUM.Text & "'" 'PINCODE
                strSql += vbCrLf + " ,'" & txtAddressPhoneRes.Text & "'" 'PHONERES
                strSql += vbCrLf + " ,'" & txtAddressMobile.Text & "'" 'MOBILE
                strSql += vbCrLf + " ,'" & txtAddressEmailId_OWN.Text & "'" 'EMAIL
                strSql += vbCrLf + " ,'" & txtAddressFax.Text & "'" 'Fax
                strSql += vbCrLf + " ,'" & VERSION & "'" 'APPVER
                strSql += vbCrLf + " ,'" & txtAddressPrevilegeId.Text & "'" 'PREVILEGEID
                strSql += vbCrLf + " ,'" & strCompanyId & "'" 'COMPANYID
                strSql += vbCrLf + " ,'" & billCostId & "'" 'COSTID
                strSql += vbCrLf + " ,'" & txtAddressPan.Text & "'" 'PAN
                If chkIdProof.Checked = True Then
                    strSql += vbCrLf + " ,'" & IIf(chkIdProof.Checked, cmbIdType.Text.ToString, "") & "'" 'PAN
                    strSql += vbCrLf + " ,'" & txtAddressIdNo.Text & "'" 'PAN
                    strSql += vbCrLf + " ,'" & IDIMAGEFILE & "'"
                Else
                    strSql += vbCrLf + ",''"
                    strSql += vbCrLf + ",''"
                    strSql += vbCrLf + ",''"
                End If
                strSql += vbCrLf + " ,'" & CUSTIMAGEFILE & "'"
                If RELIGION Then
                    strSql += vbCrLf + " ,'" & Mid(CmbReligion.Text, 1, 1) & "'"
                End If

                If chkDOB.Checked Then
                    strSql += vbCrLf + " ,'" & dtpDob_OWN.Value.Date.ToString("yyyy-MM-dd") & "'"
                End If
                If chkAnniversary.Checked Then
                    strSql += vbCrLf + " ,'" & dtpAnniv_OWN.Value.Date.ToString("yyyy-MM-dd") & "'"
                End If

                strSql += vbCrLf + " ,'" & txtGSTNo.Text & "'" 'GSTNO
                strSql += vbCrLf + " ,'" & StateId & "'" 'STATEID
                strSql += vbCrLf + " )"
                ExecQuery(SyncMode.Master, strSql, cn, tran1, billCostId)
                strSql = ""
                '190113
                If txtAddressPartyCode.Text <> "" Then
                    strSql = " UPDATE " & cnStockDb & "..ISSUE SET"
                    strSql += vbCrLf + " ACCODE = '" & txtAddressPartyCode.Text & "' WHERE TRANTYPE='MI' AND BATCHNO = '" & editBatchno & "'"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran1, billCostId)
                End If
                'NSC
                If txtAddressPartyCode.Text <> "" Then
                    strSql = " UPDATE " & cnStockDb & "..ACCTRAN SET"
                    strSql += vbCrLf + " CONTRA = '" & txtAddressPartyCode.Text & "' WHERE PAYMODE IN('GV') AND BATCHNO = '" & editBatchno & "'"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran1, billCostId)
                End If
                If cradv Then
                    '190113
                    If txtAddressPartyCode.Text <> "" Then
                        strSql = " UPDATE " & cnAdminDb & "..OUTSTANDING SET"
                        strSql += vbCrLf + " ACCODE = '" & txtAddressPartyCode.Text & "' WHERE PAYMODE IN('DU','AR','MP','MR') AND BATCHNO ='" & editBatchno & "'"
                        ExecQuery(SyncMode.Transaction, strSql, cn, tran1, billCostId)

                        strSql = " UPDATE " & cnStockDb & "..ACCTRAN SET"
                        strSql += vbCrLf + " ACCODE = '" & txtAddressPartyCode.Text & "' WHERE PAYMODE IN('DU','AR','MP','MR') AND BATCHNO = '" & editBatchno & "'"
                        ExecQuery(SyncMode.Transaction, strSql, cn, tran1, billCostId)
                    End If
                End If

            End If
        Else
            psno = txtAddressRegularSno.Text
            If txtAddressPan.Text.Trim <> "" Then
                strSql = " UPDATE " & cnAdminDb & "..PERSONALINFO SET"
                strSql += vbCrLf + " PAN = '" & txtAddressPan.Text & "'" 'PAN
                strSql += vbCrLf + " ,GSTNO = '" & txtGSTNo.Text & "'" 'GSTNO
                If chkIdProof.Checked = True Then
                    strSql += vbCrLf + " ,IDTYPE= '" & cmbIdType.Text & "'" 'PAN
                    strSql += vbCrLf + " ,IDNO = '" & txtAddressIdNo.Text & "'" 'PAN
                End If
                If RELIGION Then
                    strSql += vbCrLf + " ,RELIGION='" & Mid(CmbReligion.Text, 1, 1) & "'"
                End If
                strSql += vbCrLf + " WHERE SNO = '" & psno & "' AND ISNULL(PAN,'')=''"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran1, billCostId)
            End If

            If txtAddressIdNo.Text.Trim <> "" And chkIdProof.Checked = True Then
                strSql = " UPDATE " & cnAdminDb & "..PERSONALINFO SET"
                strSql += vbCrLf + " IDTYPE= '" & cmbIdType.Text & "'" 'PAN
                strSql += vbCrLf + " ,IDNO = '" & txtAddressIdNo.Text & "'" 'PAN
                If RELIGION Then
                    strSql += vbCrLf + " ,RELIGION='" & Mid(CmbReligion.Text, 1, 1) & "'"
                End If
                strSql += vbCrLf + " WHERE SNO = '" & psno & "' "
                ExecQuery(SyncMode.Transaction, strSql, cn, tran1, billCostId)
            End If
            If cmbAddressState.Text.Trim <> "" Then
                strSql = " UPDATE " & cnAdminDb & "..PERSONALINFO SET"
                strSql += vbCrLf + " STATE = '" & cmbAddressState.Text & "'" 'STATE
                strSql += vbCrLf + " WHERE SNO = '" & psno & "' AND ISNULL(STATE,'')=''"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran1, billCostId)
            End If
            If cmbPlaceOfSupply.Text.Trim <> "" Then
                Dim StateId As Integer
                strSql = "SELECT STATEID FROM " & cnAdminDb & "..STATEMAST WHERE STATENAME='" & cmbPlaceOfSupply.Text & "'"
                StateId = Val(objGPack.GetSqlValue(strSql, "STATEID", 0, tran1).ToString)

                strSql = " UPDATE " & cnAdminDb & "..PERSONALINFO SET"
                strSql += vbCrLf + " STATEID = '" & StateId & "'" 'STATEID
                strSql += vbCrLf + " WHERE SNO = '" & psno & "' AND ISNULL(STATEID,0)=0"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran1, billCostId)
            End If
        End If


        If Not est Then

            'If chkSendSms.Checked = False And Sms_Send = True Then
            '    strSql = " SELECT NAME FROM " & cnAdminDb & "..SYSOBJECTS S WHERE XTYPE = 'TR' "
            '    strSql += vbCrLf + " AND PARENT_OBJ=(SELECT ID FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE='U' AND NAME ='CUSTOMERINFO')"
            '    Dim dtTrig As New DataTable
            '    cmd = New OleDbCommand(strSql, cn, tran1)
            '    da = New OleDbDataAdapter(cmd)
            '    da.Fill(dtTrig)
            '    For v As Integer = 0 To dtTrig.Rows.Count - 1
            '        strSql = " ALTER TABLE " & cnAdminDb & "..CUSTOMERINFO DISABLE TRIGGER " & dtTrig.Rows(v).Item(0).ToString & ""
            '        cmd = New OleDbCommand(strSql, cn, tran1)
            '        cmd.ExecuteNonQuery()
            '    Next
            'End If
            strSql = " IF NOT (SELECT COUNT(*) FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = '" & batchno & "')>0"
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..CUSTOMERINFO"
            strSql += vbCrLf + " (BATCHNO,PSNO"
            strSql += vbCrLf + " ,REMARK1"
            If _EINVOICETYPE.ToString <> "" Then
                strSql += vbCrLf + " ,EINVOICETYPE"
            End If
            strSql += vbCrLf + " ,COSTID,PAN,DUEDATE,SMSALERT)VALUES"
            strSql += vbCrLf + " ('" & batchno & "','" & psno & "'"
            strSql += vbCrLf + " ,'" & txtRemark1.Text & "'"
            If _EINVOICETYPE.ToString <> "" Then
                strSql += vbCrLf + " ,'" & _EINVOICETYPE.ToString & "'"
            End If
            strSql += vbCrLf + " ,'" & billCostId & "','" & txtAddressPan.Text & "','" & IIf(dtpAddressDueDate.Value.Year <> 9998, dtpAddressDueDate.Value.ToString("yyyy-MM-dd"), Nothing) & "','" & IIf(chkSendSms.Checked, "Y", "N") & "')"
            strSql += vbCrLf + " END"
            If CentCustInfo Then
                ExecQuery(SyncMode.Master, strSql, cn, tran1, billCostId, , , , , , , False)
            Else
                ExecQuery(SyncMode.Transaction, strSql, cn, tran1, billCostId)
            End If
            strSql = ""
            'If chkSendSms.Checked = False And Sms_Send = True Then
            '    strSql = " SELECT NAME FROM " & cnAdminDb & "..SYSOBJECTS S WHERE XTYPE = 'TR' "
            '    strSql += vbCrLf + " AND PARENT_OBJ=(SELECT ID FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE='U' AND NAME ='CUSTOMERINFO')"
            '    Dim dtTrig As New DataTable
            '    cmd = New OleDbCommand(strSql, cn, tran1)
            '    da = New OleDbDataAdapter(cmd)
            '    da.Fill(dtTrig)
            '    For v As Integer = 0 To dtTrig.Rows.Count - 1
            '        strSql = " ALTER TABLE " & cnAdminDb & "..CUSTOMERINFO ENABLE TRIGGER " & dtTrig.Rows(v).Item(0).ToString & ""
            '        cmd = New OleDbCommand(strSql, cn, tran1)
            '        cmd.ExecuteNonQuery()
            '    Next
            'End If
        End If
        If File.Exists(picPath) = True And chkIdProof.Checked = True Then
            Dim serverPath As String = Nothing
            Dim fileDestPath As String = (defalutDestination + IDIMAGEFILE + IIf(picExtension.ToString.StartsWith("."), picExtension.ToString, "." + picExtension.ToString))
            Dim Finfo As FileInfo
            Finfo = New FileInfo(fileDestPath)
            If IO.Directory.Exists(Finfo.Directory.FullName) = False Then
                MsgBox(Finfo.Directory.FullName & " Does not exist. Please make a corresponding path", MsgBoxStyle.Information)
                Exit Function
            End If
            If UCase(picPath) <> fileDestPath.ToUpper Then
                Dim cFile As New FileInfo(picPath)
                cFile.CopyTo(fileDestPath, True)
            End If
        End If

        If File.Exists(CustpicPath) = True Then
            Dim serverPath As String = Nothing
            Dim fileDestPath As String = (defalutDestination + CUSTIMAGEFILE + IIf(CustpicExtension.ToString.StartsWith("."), CustpicExtension.ToString, "." + CustpicExtension.ToString))
            Dim Finfo As FileInfo
            Finfo = New FileInfo(fileDestPath)
            If IO.Directory.Exists(Finfo.Directory.FullName) = False Then
                MsgBox(Finfo.Directory.FullName & " Does not exist. Please make a corresponding path", MsgBoxStyle.Information)
                Exit Function
            End If
            If UCase(CustpicPath) <> fileDestPath.ToUpper Then
                Dim cFile As New FileInfo(CustpicPath)
                cFile.CopyTo(fileDestPath, True)
            End If
        End If

        PersonalinfoInsert = True
        Return psno
    End Function
    Public Sub SetCustomerPrevTrans(ByVal cond As String, ByVal condValue As String)
        If _SHOWTRANS = "N" Then Exit Sub
        If condValue = "" Then dtBalance.DataSource = Nothing : dtBalance.Visible = False : Exit Sub
        strSql = vbCrLf + "SELECT TRANNO AS NO,CONVERT(VARCHAR(20),I.TRANDATE,103)DATE,SUM(AMOUNT)AMOUNT "
        strSql += vbCrLf + "FROM " & cnStockDb & "..ISSUE I"
        strSql += vbCrLf + "INNER JOIN " & cnAdminDb & "..CUSTOMERINFO C ON I.BATCHNO=C.BATCHNO"
        strSql += vbCrLf + "INNER JOIN " & cnAdminDb & "..PERSONALINFO P ON P.SNO=C.PSNO"
        strSql += vbCrLf + "WHERE TRANTYPE IN('SA','OD')"
        strSql += vbCrLf + "AND P." & cond & "='" & condValue & "'"
        strSql += vbCrLf + "GROUP BY TRANNO,I.TRANDATE"
        strSql += vbCrLf + "ORDER BY I.TRANDATE,TRANNO"
        Dim Baldt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(Baldt)
        If Baldt.Rows.Count > 0 Then
            dtBalance.DataSource = Baldt
            dtBalance.Visible = True
            dtBalance.Columns("NO").Width = 30
            dtBalance.Columns("DATE").Width = 75
            dtBalance.Columns("AMOUNT").Width = 80
        End If
    End Sub

    Public Sub SetCustomerBalanceGrid()
        If Mid(_DRSMAINTAIN, 1, 1) = "N" Then Exit Sub
        If txtAddressPartyCode.Text = "" Then dtBalance.DataSource = Nothing : dtBalance.Visible = False : Exit Sub
        strSql = " SELECT X.TYPE,SUM(X.BALANCE) AS BALANCE FROM "
        strSql += vbCrLf + " (SELECT 'ADV' TYPE,"
        strSql += vbCrLf + " SUM(CASE WHEN RECPAY = 'P' THEN -1*AMOUNT ELSE AMOUNT  END)BALANCE"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING"
        strSql += vbCrLf + " WHERE COMPANYID = '" & strCompanyId & "' "
        strSql += vbCrLf + " AND TRANTYPE = 'A'"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND BATCHNO NOT IN (SELECT BATCHNO FROM  " & cnAdminDb & "..ORMAST)"
        strSql += vbCrLf + " AND ACCODE = '" & txtAddressPartyCode.Text & "'"
        strSql += vbCrLf & " UNION ALL "
        strSql += vbCrLf + " SELECT 'ORD' TYPE,"
        strSql += vbCrLf + " SUM(CASE WHEN RECPAY = 'P' THEN -1*AMOUNT ELSE AMOUNT END)BALANCE"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING"
        strSql += vbCrLf + " WHERE COMPANYID = '" & strCompanyId & "' "
        strSql += vbCrLf + " AND TRANTYPE = 'A'"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND BATCHNO IN (SELECT BATCHNO FROM  " & cnAdminDb & "..ORMAST)"
        strSql += vbCrLf + " AND ACCODE = '" & txtAddressPartyCode.Text & "'"
        strSql += vbCrLf & " UNION ALL "
        strSql += vbCrLf + " SELECT 'DUE' TYPE,"
        strSql += vbCrLf + " SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END)BALANCE"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING"
        strSql += vbCrLf + " WHERE COMPANYID = '" & strCompanyId & "' "
        strSql += vbCrLf + " AND TRANTYPE = 'D'"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND BATCHNO NOT IN (SELECT BATCHNO FROM  " & cnAdminDb & "..ITEMDETAIL )"
        strSql += vbCrLf + " AND ACCODE = '" & txtAddressPartyCode.Text & "'"
        strSql += vbCrLf & " UNION ALL "
        strSql += vbCrLf + " SELECT 'JND' TYPE,"
        strSql += vbCrLf + " SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END)BALANCE"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING"
        strSql += vbCrLf + " WHERE COMPANYID = '" & strCompanyId & "' "
        strSql += vbCrLf + " AND TRANTYPE = 'D'"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND BATCHNO IN (SELECT BATCHNO FROM  " & cnAdminDb & "..ITEMDETAIL )"
        strSql += vbCrLf + " AND ACCODE = '" & txtAddressPartyCode.Text & "'"
        strSql += vbCrLf + " ) X "
        strSql += vbCrLf + " GROUP BY X.TYPE "
        Dim Baldt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(Baldt)
        If Baldt.Rows.Count > 0 Then
            Dim tot As Decimal = Val(Baldt.Compute("SUM(BALANCE)", "TYPE IN('ORD','ADV')").ToString) - Val(Baldt.Compute("SUM(BALANCE)", "TYPE IN('DUE','JND')").ToString)

            Dim Drrow As DataRow = Nothing
            Drrow = Baldt.NewRow
            With Drrow
                .Item("TYPE") = "TOTAL"
                .Item("BALANCE") = Format(tot, "0.00")
            End With
            Baldt.Rows.Add(Drrow)
            Baldt.AcceptChanges()


            dtBalance.DataSource = Baldt
            If tot <> 0 Then dtBalance.Visible = True
            dtBalance.Rows(dtBalance.RowCount - 1).Cells("TYPE").Style.ForeColor = Color.Blue
            dtBalance.Rows(dtBalance.RowCount - 1).Cells("TYPE").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
            dtBalance.Columns("TYPE").Width = 60
            dtBalance.Columns("BALANCE").Width = 100
        End If
    End Sub

    Public Sub SetCustomerBalanceGridMob()
        If txtMobile.Text = "" Then dtBalance.DataSource = Nothing : dtBalance.Visible = False : Exit Sub
        strSql = " SELECT X.TYPE,SUM(X.BALANCE) AS BALANCE FROM "
        strSql += vbCrLf + " (SELECT 'ADV' TYPE,"
        strSql += vbCrLf + " SUM(CASE WHEN RECPAY = 'P' THEN -1*AMOUNT ELSE AMOUNT  END)BALANCE"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING O"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CUSTOMERINFO C ON O.BATCHNO=C.BATCHNO"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..PERSONALINFO P ON P.SNO=C.PSNO"
        strSql += vbCrLf + " WHERE O.COMPANYID = '" & strCompanyId & "' "
        strSql += vbCrLf + " AND O.TRANTYPE = 'A'"
        strSql += vbCrLf + " AND ISNULL(O.CANCEL,'') = ''"
        strSql += vbCrLf + " AND P.MOBILE = '" & txtMobile.Text & "'"
        strSql += vbCrLf + " ) X "
        strSql += vbCrLf + " GROUP BY X.TYPE "
        Dim Baldt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(Baldt)
        If Baldt.Rows.Count > 0 Then
            Dim tot As Decimal = Val(Baldt.Compute("SUM(BALANCE)", "").ToString)

            Dim Drrow As DataRow = Nothing
            Drrow = Baldt.NewRow
            With Drrow
                .Item("TYPE") = "TOTAL"
                .Item("BALANCE") = Format(tot, "0.00")
            End With
            Baldt.Rows.Add(Drrow)
            Baldt.AcceptChanges()


            dtBalance.DataSource = Baldt
            If tot <> 0 Then dtBalance.Visible = True
            dtBalance.Rows(dtBalance.RowCount - 1).Cells("TYPE").Style.ForeColor = Color.Blue
            dtBalance.Rows(dtBalance.RowCount - 1).Cells("TYPE").Style.Font = New Font("VERDANA", 8, FontStyle.Bold)
            dtBalance.Columns("TYPE").Width = 60
            dtBalance.Columns("BALANCE").Width = 100
        End If
    End Sub
    Private Function Validateduelimit() As Boolean
        strSql = " SELECT ISNULL(CREDITLIMIT,0) AS CRLIMIT FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE  = '" & txtAddressPartyCode.Text & "'"
        Dim Duelimit As Double = Val(objGPack.GetSqlValue(strSql).ToString)
        strSql = vbCrLf + " SELECT SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END)BALANCE"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING"
        strSql += vbCrLf + " WHERE COMPANYID = '" & strCompanyId & "' "
        strSql += vbCrLf + " AND TRANTYPE = 'D'"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND BATCHNO NOT IN (SELECT BATCHNO FROM  " & cnAdminDb & "..ITEMDETAIL )"
        strSql += vbCrLf + " AND ACCODE = '" & txtAddressPartyCode.Text & "'"
        Dim oldBalance As Double = Val(objGPack.GetSqlValue(strSql).ToString)
        If _DUEAMOUNT = 0 Then oldBalance = 0
        If Duelimit <> 0 And _DUEAMOUNT + oldBalance > Duelimit Then Return False Else Return True
    End Function

    Private Function Oldcreditvalue() As Double
        strSql = vbCrLf + " SELECT SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END)BALANCE"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING"
        strSql += vbCrLf + " WHERE COMPANYID = '" & strCompanyId & "' "
        strSql += vbCrLf + " AND TRANTYPE = 'D'"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND BATCHNO NOT IN (SELECT BATCHNO FROM  " & cnAdminDb & "..ITEMDETAIL )"
        strSql += vbCrLf + " AND ACCODE = '" & txtAddressPartyCode.Text & "'"
        Dim oldBalance As Double = Val(objGPack.GetSqlValue(strSql).ToString)
        If _DUEAMOUNT = 0 Then oldBalance = 0
        Return oldBalance
    End Function

    Private Function OldcreditvalueNew(ByVal condvalue As String) As Double
        If condvalue = "" Then Return 0 : Exit Function
        strSql = vbCrLf + " SELECT SUM(CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE -1*AMOUNT END)BALANCE"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING O"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CUSTOMERINFO C ON O.BATCHNO=C.BATCHNO"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..PERSONALINFO P ON P.SNO=C.PSNO"
        strSql += vbCrLf + " WHERE O.COMPANYID = '" & strCompanyId & "' "
        strSql += vbCrLf + " AND O.TRANTYPE = 'D'"
        strSql += vbCrLf + " AND ISNULL(O.CANCEL,'') = ''"
        strSql += vbCrLf + " AND O.BATCHNO NOT IN (SELECT BATCHNO FROM  " & cnAdminDb & "..ITEMDETAIL )"
        strSql += vbCrLf + " AND P.MOBILE='" & condvalue & "'"
        Dim oldBalance As Double = Val(objGPack.GetSqlValue(strSql).ToString)
        Return oldBalance
    End Function

    Private Function SetCustomerPrevTransNew(ByVal condvalue As String) As String
        If condvalue = "" Then Return 0 : Exit Function
        strSql = vbCrLf + " SELECT SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT FROM( "
        strSql += vbCrLf + " SELECT PCS,GRSWT,NETWT  FROM " & cnStockDb & "..ISSUE I"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CUSTOMERINFO C ON I.BATCHNO=C.BATCHNO"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..PERSONALINFO P ON P.SNO=C.PSNO"
        strSql += vbCrLf + " WHERE I.COMPANYID = '" & strCompanyId & "' "
        strSql += vbCrLf + " AND I.TRANTYPE = 'AI'"
        strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
        strSql += vbCrLf + " AND ISNULL(I.REFDATE,'') = ''"
        strSql += vbCrLf + " AND P.MOBILE='" & condvalue & "'"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT PCS,GRSWT,NETWT FROM " & cnStockDb & "..APPISSUE I"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CUSTOMERINFO C ON I.BATCHNO=C.BATCHNO"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..PERSONALINFO P ON P.SNO=C.PSNO"
        strSql += vbCrLf + " WHERE I.COMPANYID = '" & strCompanyId & "' "
        strSql += vbCrLf + " AND I.TRANTYPE = 'AI'"
        strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
        strSql += vbCrLf + " AND ISNULL(I.REFDATE,'') = ''"
        strSql += vbCrLf + " AND P.MOBILE='" & condvalue & "') X"
        Dim drr As DataRow
        drr = GetSqlRow(strSql, cn, Nothing)
        If Val(drr!pcs.ToString) <> 0 Or Val(drr!GRSWT.ToString) <> 0 Or Val(drr!NETWT.ToString) <> 0 Then
            Dim retstr As String = "Pcs : " & drr!pcs.ToString & ",Grswt : " & drr!GRSWT.ToString & ",Netwt : " & drr!NETWT.ToString & "."
            Return retstr
        Else
            Return ""
        End If
    End Function

    Public Function InsertintoAchead(ByVal tran1 As OleDbTransaction)
        If txtAddressPartyCode.Text <> "" Then Exit Function
        Dim drsdetail() As String = _DRSMAINTAIN.Split(",")

        strSql = " SELECT CONVERT(INT,MAX(CTLTEXT))AS ACCODE FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'ACCODE'"
        Dim tempAcCode As Integer = Val(objGPack.GetSqlValue(strSql, , , tran1).ToString)
ACCODE_GEN: tempAcCode += 1
        Dim AcCode As String = funcSetNumberStyle(tempAcCode.ToString, 7)
        If objGPack.DupCheck("SELECT 1 FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & AcCode & "'", tran1) Then
            GoTo ACCODE_GEN
        End If
        strSql = " Update " & cnAdminDb & "..SoftControl "
        strSql += vbCrLf + " Set CtlText = '" & tempAcCode & "' "
        strSql += vbCrLf + " where CtlId = 'ACCODE' "
        cmd = New OleDbCommand(strSql, cn, tran1)
        cmd.ExecuteNonQuery()

        ''Finding GroupCode
        Dim AcGrpCode As String = drsdetail(1)

        Dim Acsubgrpcode As String = "0"
        Dim CompId As String = strCompanyId
        GLEDGERCODE = ""
        If drsdetail.Length > 2 Then GLEDGERCODE = drsdetail(2)

        Dim TdsCatid As Integer = 0
        strSql = " INSERT INTO " & cnAdminDb & "..ACHEAD("
        strSql += vbCrLf + " ACCODE,TITLE,INITIAL,ACNAME,ACGRPCODE,ACSUBGRPCODE,"
        strSql += vbCrLf + " ACTYPE,DOORNO,ADDRESS1,ADDRESS2,"
        strSql += vbCrLf + " ADDRESS3,AREA,CITY,PINCODE,"
        strSql += vbCrLf + " PHONENO,MOBILE,"
        If RELIGION Then
            strSql += vbCrLf + " RELIGION,"
        End If
        strSql += vbCrLf + " EMAILID,"
        strSql += vbCrLf + " WEBSITE,LEDPRINT,TDSFLAG,TDSPER,TDSCATID,"
        strSql += vbCrLf + " DEPFLAG,DEPPER,OUTSTANDING,AUTOGEN,"
        strSql += vbCrLf + " VATEX,LOCALOUTST,LOCALTAXNO,CENTRALTAXNO,"
        strSql += vbCrLf + " USERID,CRDATE,CRTIME "
        strSql += vbCrLf + " ,TIN,PAN,BANKNAME,BANKACNO"
        strSql += vbCrLf + " ,CONTACTPERSON,CONTACTPNO"
        strSql += vbCrLf + " ,PREVILEGEID,INVENTORY"
        strSql += vbCrLf + " ,GSTNO,CREDITDAYS,CREDITLIMIT,BANKOTHERDET,ACTIVE,COMPANYID,MACCODE"
        strSql += vbCrLf + " )VALUES("
        strSql += vbCrLf + " '" & AcCode & "','" & cmbAddressTitle_OWN.Text & "','" & txtAddressInitial.Text & "','" & txtAddressName.Text & "','" & AcGrpCode & "','" & Acsubgrpcode & "',"
        strSql += vbCrLf + " 'C','" & txtAddressDoorNo.Text & "','" & txtAddress1.Text & "','" & txtAddress2.Text & "',"
        strSql += vbCrLf + " '" & txtAddress3.Text & "'"
        strSql += vbCrLf + " ,'" & cmbAddressArea_OWN.Text & "'"
        'strSql += vbcrlf + " ,'" & BrighttechPack.GetSqlValue(cn, "SELECT AREAID FROM " & cnAdminDb & "..AREAMAST WHERE AREANAME = '" & cmbArea_OWN.Text & "'", "AREAID", "", tran) & "'"
        strSql += vbCrLf + " ,'" & cmbAddressCity_OWN.Text & "','" & txtAddressPincode_NUM.Text & "',"
        strSql += vbCrLf + " '" & txtAddressPhoneRes.Text & "','" & txtAddressMobile.Text & "',"
        If RELIGION Then
            strSql += vbCrLf + " '" & Mid(CmbReligion.Text, 1, 1) & "',"
        End If
        strSql += vbCrLf + " '" & txtAddressEmailId_OWN.Text & "',"
        strSql += vbCrLf + " '','Y','N',0," & TdsCatid & ","
        strSql += vbCrLf + " 'N',0,'Y','M',"
        strSql += vbCrLf + " '','','','',"
        strSql += vbCrLf + " " & userId & ",'" & Today.Date.ToString("yyyy-MM-dd") & "','" & Date.Now.ToLongTimeString & "'"
        strSql += vbCrLf + " ,'','" & txtAddressPan.Text & "','',''"
        strSql += vbCrLf + " ,'',''"
        strSql += vbCrLf + " ,'" & txtAddressPrevilegeId.Text & "'"
        strSql += vbCrLf + " ,'N'"
        strSql += vbCrLf + " ,'" & txtGSTNo.Text & "'"
        strSql += vbCrLf + " ,'0','0'"
        strSql += vbCrLf + " ,'','Y','" & strCompanyId & "'"
        strSql += vbCrLf + ",'" & GLEDGERCODE & "'"
        strSql += vbCrLf + " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran1)
        'If chkPrevilege.Checked Then
        '    Dim preId As Int64 = Val(objGPack.GetSqlValue("SELECT ISNULL(CONVERT(BIGINT,CTLTEXT),0) FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PRE-NO'", , , tran))
        '    Dim id As String = Nothing
        '    For Each c As Char In txtPrevilegeId.Text
        '        If IsNumeric(c) Then
        '            id += c
        '        End If
        '    Next
        '    If Val(id) > preId Then
        '        strSql = " update " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT = '" & Val(id) & "' WHERE CTLID = 'PRE-NO'"
        '        cmd = New OleDbCommand(strSql, cn, tran)
        '        cmd.ExecuteNonQuery()
        '    End If
        'End If
        txtAddressPartyCode.Text = AcCode

        'ExecQuery(SyncMode.Transaction, strSql, cn, tran1, billCostId)
        strSql = ""
    End Function

    Public Function UpdateIntoAchead(ByVal tran1 As OleDbTransaction)
        If txtAddressPartyCode.Text.Trim = "" Then Exit Function
        Dim drsdetail() As String = _DRSMAINTAIN.Split(",")
        If Val(objGPack.GetSqlValue("SELECT COUNT(*) FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE='" & txtAddressPartyCode.Text & "'", , , tran1)) = 0 Then Exit Function
        ''Finding GroupCode
        Dim AcGrpCode As String
        If drsdetail.Length > 1 Then AcGrpCode = drsdetail(1)
        Dim Acsubgrpcode As String = "0"
        Dim CompId As String = strCompanyId
        GLEDGERCODE = ""
        If drsdetail.Length > 2 Then GLEDGERCODE = drsdetail(2)

        Dim TdsCatid As Integer = 0
        strSql = " UPDATE " & cnAdminDb & "..ACHEAD SET"
        strSql += vbCrLf + " TITLE='" & cmbAddressTitle_OWN.Text & "'"
        strSql += vbCrLf + " ,INITIAL='" & txtAddressInitial.Text & "'"
        strSql += vbCrLf + " ,ACNAME='" & txtAddressName.Text & "'"
        ''strSql += vbCrLf + " ,ACTYPE='C'"
        strSql += vbCrLf + " ,DOORNO='" & txtAddressDoorNo.Text & "'"
        strSql += vbCrLf + " ,ADDRESS1='" & txtAddress1.Text & "'"
        strSql += vbCrLf + " ,ADDRESS2='" & txtAddress2.Text & "'"
        strSql += vbCrLf + " ,ADDRESS3='" & txtAddress3.Text & "'"
        strSql += vbCrLf + " ,AREA='" & cmbAddressArea_OWN.Text & "'"
        strSql += vbCrLf + " ,CITY='" & cmbAddressCity_OWN.Text & "'"
        strSql += vbCrLf + " ,PINCODE='" & txtAddressPincode_NUM.Text & "'"
        strSql += vbCrLf + " ,PHONENO='" & txtAddressPhoneRes.Text & "'"
        strSql += vbCrLf + " ,MOBILE='" & txtAddressMobile.Text & "'"
        strSql += vbCrLf + " ,EMAILID='" & txtAddressEmailId_OWN.Text & "'"
        If RELIGION Then
            strSql += vbCrLf + " ,RELIGION='" & Mid(CmbReligion.Text, 1, 1) & "'"
        End If
        ''strSql += vbCrLf + " ,WEBSITE=''"
        ''strSql += vbCrLf + " ,LEDPRINT='Y'"
        ''strSql += vbCrLf + " ,TDSFLAG='N'"
        ''strSql += vbCrLf + " ,TDSPER=0"
        ''strSql += vbCrLf + " ,TDSCATID=" & TdsCatid & ""
        ''strSql += vbCrLf + " ,DEPFLAG='N',DEPPER=0,OUTSTANDING='Y',AUTOGEN='M'"
        ''strSql += vbCrLf + " ,VATEX='',LOCALOUTST='',LOCALTAXNO='',CENTRALTAXNO=''"
        strSql += vbCrLf + " ,USERID=" & userId & ""
        strSql += vbCrLf + " ,CRDATE='" & Today.Date.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,CRTIME='" & Date.Now.ToLongTimeString & "' "
        ''strSql += vbCrLf + " ,TIN=''"
        strSql += vbCrLf + " ,PAN='" & txtAddressPan.Text & "'"
        ''strSql += vbCrLf + " ,BANKNAME=''"
        ''strSql += vbCrLf + " ,BANKACNO=''"
        ''strSql += vbCrLf + " ,CONTACTPERSON=''"
        ''strSql += vbCrLf + " ,CONTACTPNO=''"
        strSql += vbCrLf + " ,PREVILEGEID='" & txtAddressPrevilegeId.Text & "'"
        ''strSql += vbCrLf + " ,INVENTORY='N'"
        ''strSql += vbCrLf + " ,CREDITDAYS='0',CREDITLIMIT='0',BANKOTHERDET='',ACTIVE='Y'"
        strSql += vbCrLf + " ,COMPANYID='" & strCompanyId & "'"
        ''strSql += vbCrLf + " ,MACCODE='" & GLEDGERCODE & "'"
        strSql += vbCrLf + " ,GSTNO='" & txtGSTNo.Text & "'"
        strSql += vbCrLf + " WHERE ACCODE='" & txtAddressPartyCode.Text & "'"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran1)
        strSql = ""
    End Function

    Private Sub frmAddressDia_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'If CurrencyDecimal <> 3 Then
        '    Label11.Visible = False : txtRemark2.Visible = False : txtRemark2.Enabled = False
        '    Label12.Visible = False : txtRemark3.Visible = False : txtRemark3.Enabled = False
        '    Label13.Visible = False : txtRemark4.Visible = False : txtRemark3.Enabled = False
        '    Label14.Visible = False : txtRemark5.Visible = False : txtRemark3.Enabled = False
        '    txtRemark1.Multiline = True
        '    txtRemark1.Height = 41
        'End If
        If Validate_DueDate = False Then Val_DueDate = False
        Dim HidHeight As Integer = 0
        If Not btnOk.Enabled Then
            HidHeight += btnOk.Size.Height
        End If
        If Not txtRemark1.Enabled Then
            HidHeight += txtRemark1.Size.Height
        End If
        'If Not txtRemark2.Enabled Then
        '    HidHeight += txtRemark2.Size.Height
        'End If
        'If Not txtRemark3.Enabled Then
        '    HidHeight += txtRemark3.Size.Height
        'End If
        'If Not txtRemark4.Enabled Then
        '    HidHeight += txtRemark4.Size.Height
        'End If
        'If Not txtRemark5.Enabled Then
        '    HidHeight += txtRemark5.Size.Height
        'End If
        'If Not dtpAddressDueDate.Visible Then
        '    HidHeight += dtpAddressDueDate.Height
        'End If
        Me.Size = New Size(FrmSize.Width, FrmSize.Height - HidHeight)
        grpAddress.Size = New Size(GrpSize.Width, GrpSize.Height - HidHeight)
        'btnOk_Cancel_VisibleChanged(Me, New EventArgs)

        'chkRegularCustomer.Select()
        If RELIGION Then
            If CmbReligion.Items.Count = 0 Then
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
            End If
            txtAddressDoorNo.Size = New Size(143, 21)
        Else
            lblReligion.Visible = False
            CmbReligion.Visible = False
            txtAddressDoorNo.Size = New Size(301, 21)
        End If
        If editBatchno <> "" Then SetAddressDetails()
        If _IsWholeSaleType Then
            lblRegularCust.Visible = False
            'chkRegularCustomer.Visible = False
        End If
        If editBatchno = "" Then
            chkDOB.Checked = False : dtpDob_OWN.Enabled = False : chkAnniversary.Checked = False : dtpAnniv_OWN.Enabled = False
            dtpDob_OWN.Value = GetEntryDate(GetServerDate)
            dtpAnniv_OWN.Value = GetEntryDate(GetServerDate)
        End If
        If txtAddressPrevilegeId.Text <> "" Then txtPrevilegeId_KeyPress(Me, New KeyPressEventArgs(Chr(Keys.Enter)))
        If txtAddressPartyCode.Text <> "" Then txtAddressPartyCode_LostFocus(Me, New EventArgs)
        dtpAddressDueDate.MaximumDate = (New BrighttechPack.DatePicker).MaximumDate
        Addvalid = GetAdmindbSoftValuefromDt(dtSoftKeyss, "PER_INFO_VALID", "")
        Denyvalid = GetAdmindbSoftValue("PER_INFO_DENY", "")
        If _IsCredit = False And GetAdmindbSoftValue("PER_INFO_VALID_CREDIT", "N") = "Y" Then
            Addvalid = ""
        End If
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICPATH'"
        defalutDestination = UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", , tran))
        If Not defalutDestination.EndsWith("\") And defalutDestination <> "" Then defalutDestination += "\"
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICSOURCEPATH'"
        defalutSourcePath = UCase(objGPack.GetSqlValue(strSql, "CTLTEXT", , tran))
        'ADDMOB_RESTRICT = IIf(GetAdmindbSoftValue("ADD_MOBILENO_RES", "N", ) = "Y", True, False)
        ADDMOB_RESTRICT = GetAdmindbSoftValue("ADD_MOBILENO_RES", "N", )
        If ADDMOB_RESTRICT <> "N" Then txtAddressMobile.MaxLength = 10
        If AddressLock Then
            ADDRESSUPDATE = "N"
        Else
            ADDRESSUPDATE = GetAdmindbSoftValue("ADDRESSUPDATE", "N")
        End If
        If GetAdmindbSoftValue("ADD_UPDATE_RESTRICT_GST", "N") = "Y" And txtGSTNo.Text.Trim <> "" Then
            ADDRESSUPDATE = "N"
        End If
        If ADDRESSUPDATE = "Y" Then
            chkAddchange.Visible = True
            chkAddchange.Checked = False
            chkAddchange.Enabled = True
            If txtAddressName.Text.Trim = "" Then
                lockControls(False)
            Else
                lockControls(True)
            End If
        ElseIf ADDRESSUPDATE = "M" Then
            chkAddchange.Visible = True
            chkAddchange.Checked = True
            lockControls(False)
        ElseIf ADDRESSUPDATE = "N" Then
            chkAddchange.Visible = True
            chkAddchange.Checked = False
            chkAddchange.Enabled = False
            If txtAddressName.Text.Trim = "" Then
                lockControls(False)
            Else
                lockControls(True)
            End If
        End If
        btnAttachCustomerImage.Enabled = POS_CUST_IMAGE
        btnAttachCustomerImage.Visible = POS_CUST_IMAGE
        strSql = "SELECT NAME FROM " & cnAdminDb & "..IDPROOF WHERE ISNULL(ACTIVE,'Y')<>'N' ORDER BY DISPORDER"
        FillCombo_cmbIdType(strSql, cmbIdType, , IIf(txtAddressIdNo.Text.Trim() <> "", False, True))
        If GstFlag = False Then
            lblTimer.Enabled = False
            lblBillType.Visible = False
        End If
        If InterBill = False Then
            If editBatchno = "" Then
                cmbPlaceOfSupply.Text = CompanyState
                If cmbAddressState.Text = "" Then cmbAddressState.Text = CompanyState
            End If
        End If
        If _NRIBill Then
            cmbPlaceOfSupply.Text = ""
            cmbAddressState.Text = ""
        End If
        If BOUNZ_SALES Then
            chkBounz.Visible = True
            chkBounz.Checked = False
        Else
            chkBounz.Visible = False
            chkBounz.Checked = False
        End If
        txtAddressInitial.Enabled = True
        txtAddressMName.Enabled = True
        cmbAliasName.Enabled = False
        txtAddressSName.Enabled = True
        txtAddressDoorNo.Enabled = True
        If PER_VALID_FOCUS Then FocusAddress()
        If Denyvalid <> "" Then DenyFocusAddress()
        If txtAddressDoorNo.Enabled = False And txtAddressDoorNo.Text <> "" Then
            txtAddressDoorNo.Text = ""
        End If
        If _NRIBill Then
            txtRemark1.Text = "NRI SALES"
            txtRemark1.ReadOnly = True
        Else
            txtRemark1.ReadOnly = False
        End If
    End Sub
    Public Sub FillCombo_cmbIdType(ByVal str As String, ByVal Sender As Object _
    , Optional ByVal clear As Boolean = True, Optional ByVal SetDefault As Boolean = True)
        Dim cmb As ComboBox = CType(Sender, ComboBox)
        If clear = True Then cmb.Items.Clear()
        Dim G_DAdapter As OleDbDataAdapter
        G_DAdapter = New OleDbDataAdapter(str, cn)
        Dim G_DTable As DataTable
        G_DTable = New DataTable
        G_DAdapter.Fill(G_DTable)
        For cnt As Integer = 0 To G_DTable.Rows.Count - 1
            cmb.Items.Add(G_DTable.Rows(cnt).Item(0).ToString)
        Next
        If SetDefault Then
            If cmb.Items.Count > 0 Then cmb.SelectedIndex = 0
        End If
    End Sub
    Sub funcGetDetails(ByVal tempAcCode As String, Optional ByVal tAccode As String = "")
        Dim dtpersonal As New DataTable
        dtpersonal.Clear()
        If tAccode <> "" Then
            strSql = "SELECT ''SNO,ACCODE,PREVILEGEID,TITLE,INITIAL,ACNAME PNAME,MNAME,SNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,PINCODE,STATE,COUNTRY,"
            strSql += vbCrLf + " PHONENO PHONERES,MOBILE,EMAILID EMAIL,FAX,PAN,''IDTYPE,'' IDNO"
            strSql += vbCrLf + " ,ISNULL(RELIGION,'')RELIGION,GSTNO"
            strSql += vbCrLf + " ,(SELECT TOP 1 STATENAME FROM " & cnAdminDb & "..STATEMAST WHERE STATEID=P.STATEID)STATEID"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ACHEAD P WHERE ACCODE = '" & tAccode & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtpersonal)
        Else
            strSql = "SELECT SNO,ACCODE,PREVILEGEID,TITLE,INITIAL,PNAME,MNAME,SNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,PINCODE,STATE,COUNTRY,"
            strSql += vbCrLf + " PHONERES,MOBILE,EMAIL,FAX,PAN,IDTYPE,IDNO"
            strSql += vbCrLf + " ,'' RELIGION,GSTNO"
            strSql += vbCrLf + " ,(SELECT TOP 1 STATENAME FROM " & cnAdminDb & "..STATEMAST WHERE STATEID=P.STATEID)STATEID"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..PERSONALINFO P WHERE SNO = '" & tempAcCode & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtpersonal)
        End If

        If Not dtpersonal.Rows.Count > 0 Then
            Exit Sub
        End If
        With dtpersonal.Rows(0)
            If .Item("accode").ToString <> "" Then
                GLEDGERCODE = objGPack.GetSqlValue("SELECT MACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & .Item("ACCODE").ToString & "'", , , tran).ToString
            End If
            AddressLock = True
            'chkRegularCustomer.Checked = True
            txtAddressRegularSno.Text = .Item("SNO").ToString
            txtAddressPrevilegeId.Text = .Item("PREVILEGEID").ToString
            txtAddressPartyCode.Text = .Item("ACCODE").ToString
            cmbAddressTitle_OWN.Text = .Item("TITLE").ToString
            txtAddressInitial.Text = .Item("INITIAL").ToString
            txtAddressName.Text = .Item("PNAME").ToString
            txtAddressMName.Text = .Item("MNAME").ToString
            txtAddressSName.Text = .Item("SNAME").ToString
            txtAddressDoorNo.Text = .Item("DOORNO").ToString
            txtAddress1.Text = .Item("ADDRESS1").ToString
            txtAddress2.Text = .Item("ADDRESS2").ToString
            txtAddress3.Text = .Item("ADDRESS3").ToString
            cmbAddressArea_OWN.Text = .Item("AREA").ToString
            cmbAddressCity_OWN.Text = .Item("CITY").ToString
            txtAddressPincode_NUM.Text = .Item("PINCODE").ToString
            cmbAddressState.Text = .Item("STATE").ToString
            cmbPlaceOfSupply.Text = .Item("STATEID").ToString
            cmbAddressCountry_OWN.Text = .Item("COUNTRY").ToString
            txtAddressPhoneRes.Text = .Item("PHONERES").ToString
            txtAddressMobile.Text = .Item("MOBILE").ToString
            txtAddressEmailId_OWN.Text = .Item("EMAIL").ToString
            txtAddressFax.Text = .Item("FAX").ToString
            txtAddressPan.Text = .Item("PAN").ToString
            cmbIdType.Text = .Item("IDTYPE").ToString
            txtAddressIdNo.Text = .Item("IDNO").ToString
            txtGSTNo.Text = .Item("GSTNO").ToString
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
            If InterBill = False Then
                If cmbPlaceOfSupply.Text = "" Then cmbPlaceOfSupply.Text = CompanyState
                If cmbAddressState.Text = "" Then cmbAddressState.Text = CompanyState
            End If
            If _NRIBill Then
                cmbPlaceOfSupply.Text = ""
                cmbAddressState.Text = ""
            End If
            SetCustomerBalanceGrid()
            If txtAddressName.Text <> "" Then SetCustomerPrevTrans("PNAME", txtAddressName.Text.Trim)
        End With
    End Sub

    Private Sub SetAddressDetails()
        strSql = " SELECT SNO,PREVILEGEID,ACCODE,TITLE,INITIAL,PNAME,MNAME,SNAME,DOORNO"
        strSql += vbCrLf + " ,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,PINCODE,STATE,COUNTRY,PHONERES,MOBILE,EMAIL,FAX,P.IDNO,P.IDTYPE,P.IDIMAGEFILE"
        strSql += vbCrLf + " ,C.REMARK1 AS REMARK1,P.PAN,P.RELIGION,GSTNO"
        strSql += vbCrLf + " ,(SELECT TOP 1 STATENAME FROM " & cnAdminDb & "..STATEMAST WHERE STATEID=P.STATEID)STATEID"
        strSql += vbCrLf + " ,DOB,ANNIVERSARY"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..PERSONALINFO AS P "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CUSTOMERINFO C ON C.PSNO = P.SNO "
        strSql += vbCrLf + " WHERE C.BATCHNO = '" & editBatchno & "'"
        Dim dtAdd As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtAdd)
        If Not dtAdd.Rows.Count > 0 Then Exit Sub
        With dtAdd.Rows(0)
            If .Item("accode").ToString <> "" Then
                GLEDGERCODE = objGPack.GetSqlValue("SELECT MACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & .Item("ACCODE").ToString & "'", , , tran).ToString
            End If
            txtAddressPrevilegeId.Text = .Item("PREVILEGEID").ToString
            txtAddressPartyCode.Text = .Item("ACCODE").ToString
            cmbAddressTitle_OWN.Text = .Item("TITLE").ToString
            txtAddressInitial.Text = .Item("INITIAL").ToString
            txtAddressName.Text = .Item("PNAME").ToString
            txtAddressMName.Text = .Item("MNAME").ToString
            txtAddressSName.Text = .Item("SNAME").ToString
            txtAddressDoorNo.Text = .Item("DOORNO").ToString
            txtAddress1.Text = .Item("ADDRESS1").ToString
            txtAddress2.Text = .Item("ADDRESS2").ToString
            txtAddress3.Text = .Item("ADDRESS3").ToString
            cmbAddressArea_OWN.Text = .Item("AREA").ToString
            cmbAddressCity_OWN.Text = .Item("CITY").ToString
            cmbAddressState.Text = .Item("STATE").ToString
            cmbPlaceOfSupply.Text = .Item("STATEID").ToString
            cmbAddressCountry_OWN.Text = .Item("COUNTRY").ToString
            txtAddressPincode_NUM.Text = .Item("PINCODE").ToString
            txtAddressPhoneRes.Text = .Item("PHONERES").ToString
            txtAddressMobile.Text = .Item("MOBILE").ToString
            txtAddressEmailId_OWN.Text = .Item("EMAIL").ToString
            txtAddressFax.Text = .Item("FAX").ToString
            txtAddressRegularSno.Text = .Item("SNO").ToString
            txtRemark1.Text = .Item("REMARK1").ToString
            txtAddressPan.Text = .Item("PAN").ToString
            txtAddressIdNo.Text = .Item("IDNO").ToString
            cmbIdType.Text = .Item("IDTYPE").ToString
            txtGSTNo.Text = .Item("GSTNO").ToString
            If .Item("DOB").ToString() <> "" Then
                If Not .Item("DOB").ToString().Contains("1900-01-01") Then
                    dtpDob_OWN.Value = .Item("DOB").ToString()
                    chkDOB.Checked = True
                Else
                    chkDOB.Checked = False
                    dtpDob_OWN.Enabled = False
                End If
            Else
                chkDOB.Checked = False
                dtpDob_OWN.Enabled = False
            End If
            If .Item("ANNIVERSARY").ToString() <> "" Then
                If Not .Item("ANNIVERSARY").ToString().Contains("1900-01-01") Then
                    dtpAnniv_OWN.Value = .Item("ANNIVERSARY").ToString()
                    chkAnniversary.Checked = True
                Else
                    chkAnniversary.Checked = False
                    dtpAnniv_OWN.Enabled = False
                End If
            Else
                chkAnniversary.Checked = False
                dtpAnniv_OWN.Enabled = False
            End If
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
            If InterBill = False Then
                If editBatchno = "" Then
                    If cmbPlaceOfSupply.ToString.Trim = "" Then cmbPlaceOfSupply.Text = CompanyState
                    If cmbAddressState.Text = "" Then cmbAddressState.Text = CompanyState
                End If
            End If
            If _NRIBill Then
                cmbPlaceOfSupply.Text = ""
                cmbAddressState.Text = ""
            End If
            cmbAddressTitle_OWN.Select()
        End With
    End Sub

    Public Sub DeleteAndInsert(ByVal trn As OleDbTransaction)

        _einvtype = ""
        strSql = " SELECT ISNULL(EINVOICETYPE,'')EINVOICETYPE FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = '" & editBatchno & "'"
        _einvtype = objGPack.GetSqlValue(strSql, "EINVOICETYPE", "", trn)

        strSql = " DELETE FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = '" & editBatchno & "'"
        'cmd = New OleDbCommand(strSql, cn, trn)
        'cmd.ExecuteNonQuery()
        If CentCustInfo Then
            ExecQuery(SyncMode.Master, strSql, cn, trn, editCostId, , , , , , , False)
        Else
            ExecQuery(SyncMode.Transaction, strSql, cn, trn, editCostId)
        End If
        InsertIntoPersonalInfo(editBilldate, editCostId, editBatchno, trn,, _einvtype.ToString)

    End Sub

    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Try
            If Giftiss = True Then
                Dim GVRACCODE As String
                '= objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & roGift!CARDTYPE.ToString & "'", , , tran)

                'InsertIntoAccTran(TRANNO, IIf(Val(txtAdjReceive_AMT.Text) + Val(txtAdjSrCredit_AMT.Text) > 0, "C", "D"), _
                'GVRACCODE, amt, 0, 0, 0, "GV", "", , , , , , roGift!REMARK.ToString)

                '            Dim runNo As String = GetCostId(BillCostId) & GetCompanyId(strCompanyId) & "G" + Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString + TRANNO.ToString
                Dim gvprefix As String = GetAdmindbSoftValuefromDt(dtSoftKeyss, "GVPREFIX", "GV,8")
                Dim GVPREFIXARR() As String = Split(gvprefix, ",")
                editBatchno = GetNewBatchno(editCostId, editBilldate, tran)
                Dim runno As String = txtGvNo.Text ' GetGvNo(tran)
                Dim prefixlen As Integer = Len(GVPREFIXARR(0).ToString & GetCostId(cnCostId) & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString)
                Dim tranno As Integer = Val(Mid(txtGvNo.Text, prefixlen + 1))
                Dim amt As Decimal = Val(lblValue.Text)
                InsertIntoOustanding(tranno, "GV", runno, amt, "R", "GV", , , , 0, amt, cardcode, , , , dtpValid.Value, txtRemark1.Text, , GVRACCODE)
                Gift_Personalinfoinsert(editBilldate, editBatchno)
                'InsertIntoPersonalInfo(editBilldate, editCostId, editBatchno, tran)
            Else

                Dim PRIV_CARD As String = IIf(GetAdmindbSoftValue("PRIV_CARD", "N") = "Y", "Y", "N")
                Dim BLRetail As New BL_RetailBill
                Dim dtprevil As New DataTable
                Dim Save_Previlige As Boolean = True
                Dim TRANNO As Integer = 0
                If POS_PRIVILEGE_UPDATE = True Then
                    If PRIV_CARD = "Y" Then
                        If txtAddressPrevilegeId.Text.Trim <> "" Then
                            strSql = vbCrLf + "SELECT TRANNO,ITEMID,SUBITEMID,GRSWT,NETWT,AMOUNT AS GROSSAMT,'Y' ENTFLAG,'SA' TRANTYPE,(FIN_DISCOUNT+DISCOUNT) DISCOUNT,RESNO  FROM " & cnStockDb & "..ISSUE WHERE TRANTYPE = 'SA' AND BATCHNO = '" & editBatchno & "' AND ISNULL(cANCEL,'') = ''"
                            strSql += vbCrLf + "UNION ALL"
                            strSql += vbCrLf + "SELECT TRANNO,ITEMID,SUBITEMID,GRSWT,NETWT,AMOUNT AS GROSSAMT,'Y' ENTFLAG, 'SR' TRANTYPE,(FIN_DISCOUNT+DISCOUNT) DISCOUNT,JOBISNO FROM " & cnStockDb & "..RECEIPT WHERE TRANTYPE = 'SR' AND BATCHNO = '" & editBatchno & "' AND ISNULL(CANCEL,'') = ''"
                            da = New OleDbDataAdapter(strSql, cn)
                            da.Fill(dtprevil)
                            TRANNO = dtprevil.Rows(0).Item("TRANNO").ToString
                            If Val(dtprevil.Compute("SUM(DISCOUNT)", "TRANTYPE='SA'").ToString) > 0 Then
                                If MessageBox.Show("Discount given." + vbCrLf + " Do you want to add Privilige Points?", "Brighttech", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = MsgBoxResult.No Then
                                    Save_Previlige = False
                                End If
                            End If
                            If dtprevil.Rows(0).Item("RESNO").ToString <> "" Then
                                MsgBox("This Billno already previlege available", MsgBoxStyle.OkOnly)
                                PRIV_CARD = "N"
                            End If
                        End If
                    End If
                End If

                Dim dtcradv As New DataTable
                strSql = " select 1 from " & cnAdminDb & "..OUTSTANDING WHERE PAYMODE IN ('DU','AR','MP','MR') AND BATCHNO = '" & editBatchno & "' "
                strSql += vbCrLf + " AND ACCODE <> '" & txtAddressPartyCode.Text & "'"
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtcradv)
                If dtcradv.Rows.Count > 0 Then
                    If dtcradv.Rows(0).Item(0) > 0 Then cradv = True
                End If
                tran = Nothing
                tran = cn.BeginTransaction
                DeleteAndInsert(tran)

                If POS_PRIVILEGE_UPDATE = True Then
                    If txtAddressPrevilegeId.Text.Trim <> "" Then
                        If PRIV_CARD = "Y" Then
                            Dim _TranTypeCol As New List(Of String)
                            _TranTypeCol.Clear()
                            _TranTypeCol.Add("SA")
                            If dtprevil.Rows.Count > 0 Then
                                BLRetail.funccalcPreviledge(PRIV_CARD, tran, dtprevil, Save_Previlige, editBilldate, editCostId, editBatchno, _TranTypeCol, TRANNO, txtAddressPrevilegeId.Text.Trim, "", 0, 0, 0)
                            End If
                        End If
                    End If
                End If


                tran.Commit()
                tran = Nothing
            End If
            Dim msg As String = "Updated Successfully.." + vbCrLf
            MsgBox(msg)
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub


    Private Sub DGVPersonalInfo_CellContentDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DGVPersonalInfo.CellContentDoubleClick
        If DGVPersonalInfo.Rows.Count > 0 Then
            funcGetDetails(DGVPersonalInfo.CurrentRow.Cells("SNO").Value)
            PanPersonal.Visible = False
            txtAddressName.Focus()
        End If
    End Sub

    Private Sub DGVPersonalInfo_CellPainting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles DGVPersonalInfo.CellPainting
        If e.RowIndex >= 0 And e.ColumnIndex >= 0 Then

            e.Handled = True
            e.PaintBackground(e.CellBounds, True)
            Dim sw As String = txtAddressName.Text
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

    Private Sub DGVPersonalInfo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DGVPersonalInfo.KeyDown
        If e.KeyCode = Keys.Up Then
            If DGVPersonalInfo.CurrentRow.Index = 0 Then
                PanPersonal.Visible = False
                txtAddressName.Focus()
            End If
        End If
        If e.KeyCode = Keys.Enter Then
            If DGVPersonalInfo.Rows.Count > 0 Then
                funcGetDetails(DGVPersonalInfo.CurrentRow.Cells("SNO").Value, DGVPersonalInfo.CurrentRow.Cells("ACCODE").Value.ToString)
                PanPersonal.Visible = False
                txtAddressName.Focus()
            End If
        End If
    End Sub

    Private Sub txtAddressName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAddressName.TextChanged
        If _IsWholeSaleType Then Exit Sub
        If Not txtAddressName.Focused Then
            PanPersonal.Visible = False
            Exit Sub
        End If
        Dim HIDE_NAME_SRCH As String = GetAdmindbSoftValuefromDt(dtSoftKeyss, "HIDE-NAMESEARCH", "Y")
        If Mid(_DRSMAINTAIN, 1, 1) = "N" And HIDE_NAME_SRCH = "Y" Then Exit Sub
        If DGVPersonalInfo.Rows.Count = 0 Then
            PanPersonal.Visible = False
        Else
            PanPersonal.Visible = True
        End If
        If dtpersonal.Rows.Count > 0 Then
            Dim DView As New DataView
            DView = dtpersonal.DefaultView

            With DView
                .AllowDelete = False
                .AllowEdit = False
                .AllowNew = False
                If HIDE_NAME_SRCH = "N" Then
                    .RowFilter = "PNAME LIKE '" & txtAddressName.Text.ToString().Trim() & "%' "
                Else
                    Dim SrchStr As String = "PNAME LIKE '" & txtAddressName.Text.ToString().Trim() & "%'"
                    SrchStr += "  OR ACCODE LIKE '%" & txtAddressName.Text.ToString().Trim() & "%' "
                    SrchStr += "  OR PHONERES LIKE '%" & txtAddressName.Text.ToString().Trim() & "%' "
                    SrchStr += "  OR MOBILE LIKE '%" & txtAddressName.Text.ToString().Trim() & "%'"
                    SrchStr += "  OR DOORNO LIKE '%" & txtAddressName.Text.ToString().Trim() & "%'"
                    SrchStr += "  OR EMAILID LIKE '%" & txtAddressName.Text.ToString().Trim() & "%'"
                    SrchStr += "  OR ADDRESS1 LIKE '%" & txtAddressName.Text.ToString().Trim() & "%'"
                    SrchStr += "  OR ADDRESS2 LIKE '%" & txtAddressName.Text.ToString().Trim() & "%'"
                    SrchStr += "  OR ADDRESS3 LIKE '%" & txtAddressName.Text.ToString().Trim() & "%'"
                    SrchStr += "  OR AREA LIKE '%" & txtAddressName.Text.ToString().Trim() & "%'"
                    SrchStr += "  OR CITY LIKE '%" & txtAddressName.Text.ToString().Trim() & "%'"
                    SrchStr += "  OR STATE LIKE '%" & txtAddressName.Text.ToString().Trim() & "%'"
                    SrchStr += "  OR COUNTRY LIKE '%" & txtAddressName.Text.ToString().Trim() & "%'"
                    SrchStr += "  OR PINCODE LIKE '%" & txtAddressName.Text.ToString().Trim() & "%'"
                    .RowFilter = SrchStr
                End If
                .RowStateFilter = DataViewRowState.OriginalRows
                '.Sort = "PNAME"
            End With
            DGVPersonalInfo.DataSource = DView.Table
        End If
    End Sub

    'Private Sub txtRemark1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtRemark1.KeyDown
    '    If e.KeyCode = Keys.Enter Then
    '        If txtRemark5.Enabled = False Then e.Handled = True
    '    End If
    'End Sub

    Private Sub txtRemark1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtRemark1.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If CurrencyDecimal = 3 Then Exit Sub
            e.Handled = True
            If btnOk.Visible Then
                btnOk.Select()
                Exit Sub
            End If
            txtAddressPrevilegeId.Select()
            'chkCreateNewAcc.Focus()
            If ValidatateAddress() Then
                If _IsWholeSaleType Then
                    If txtAddressName.Text <> "" Then Me.DialogResult = Windows.Forms.DialogResult.OK
                    Me.Close()
                Else
                    Me.DialogResult = Windows.Forms.DialogResult.OK
                    Me.Close()
                End If
            End If
        End If
    End Sub

    Private Sub txtAddressRegularSno_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAddressRegularSno.TextChanged
        SetCustomerBalance(tran)
    End Sub

    Private Sub txtAddressPartyCode_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAddressPartyCode.LostFocus
        If Mid(_DRSMAINTAIN, 1, 1) <> "N" Then SetCustomerBalanceGrid() Else SetCustomerBalance(Nothing)

    End Sub

    Private Sub txtAddressPan_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAddressPan.Enter
        If txtAddressDueDays_NUM.Enabled And dtpAddressDueDate.Enabled And Val_DueDate Then
            If GetEntryDate(GetServerDate) >= dtpAddressDueDate.Value Then
                MsgBox("Due date less than/same of Bill date", MsgBoxStyle.Information)
                dtpAddressDueDate.Focus()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub txtAddressPan_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAddressPan.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtAddressPan.Text = "" And PanMand And _NRIBill = False And _ExportBill = False Then
                MsgBox("Pan no should not empty", MsgBoxStyle.Information)
                txtAddressPan.Select()
                Exit Sub
            End If
            If (txtRemark1.Visible And txtRemark1.Enabled) _
            Or btnOk.Visible Then
                SendKeys.Send("{TAB}")
            Else
                'chkCreateNewAcc.Focus()
                If ValidatateAddress() Then
                    If _IsWholeSaleType Then
                        If txtAddressName.Text <> "" Then Me.DialogResult = Windows.Forms.DialogResult.OK
                        Me.Close()
                    Else
                        Me.DialogResult = Windows.Forms.DialogResult.OK
                        Me.Close()
                        Exit Sub
                    End If
                End If
            End If

        End If
    End Sub

    Private Sub txtMobile_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMobile.GotFocus
        If Mid(_DRSMAINTAIN, 1, 1) = "G" Or Mid(_DRSMAINTAIN, 1, 1) = "S" Then txtAddressName.Focus()
    End Sub

    Private Sub txtMobile_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMobile.KeyPress

        Dim keyChar As String
        keyChar = e.KeyChar
        If (Not Char.IsDigit(keyChar)) And AscW(keyChar) <> 8 And AscW(keyChar) <> 13 And AscW(keyChar) <> 45 And AscW(keyChar) <> 43 And ((AscW(keyChar) = 44 And ADDMOB_RESTRICT <> "N") Or AscW(keyChar) <> 44) And ((AscW(keyChar) = 32 And ADDMOB_RESTRICT <> "N") Or AscW(keyChar) <> 32) Then
            e.Handled = True
            MsgBox("Digits only Allowed 1 to 9")
            txtMobile.Focus()
            Exit Sub
        End If

        Dim sno As String
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtMobile.Text = "" Then
                'SendKeys.Send("{TAB}")
                Exit Sub
            End If

            If _SHOWTRANSMOB Then
                Dim OLDDUE As Double = OldcreditvalueNew(txtMobile.Text)
                If OLDDUE <> 0 Then MsgBox("Customer Previous Balance is Rs." & OLDDUE.ToString, MsgBoxStyle.Information)
                Dim pendappiss As String = SetCustomerPrevTransNew(txtMobile.Text)
                If pendappiss <> "" Then MsgBox("Customer Previous Approval Issue" & vbCrLf & pendappiss.ToString, MsgBoxStyle.Information)
            End If

            If _SHOWTRANSADVMOB Then
                SetCustomerBalanceGridMob()
            End If

            strSql = " SELECT SNO,PREVILEGEID,ACCODE,PNAME,MNAME,SNAME,DOORNO,ADDRESS1"
            strSql += vbCrLf + " ,ADDRESS2,ADDRESS3,AREA,PINCODE,CITY,COUNTRY,PHONERES,MOBILE,IDTYPE,IDNO,PAN,GSTNO"
            strSql += vbCrLf + " ,(SELECT TOP 1 STATENAME FROM " & cnAdminDb & "..STATEMAST WHERE STATEID=P.STATEID)STATEID,DOB DOBIRTH,ANNIVERSARY"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..PERSONALINFO P"
            strSql += vbCrLf + " WHERE MOBILE = '" & txtMobile.Text & "' OR PHONERES = '" & txtMobile.Text & "' "
            strSql += vbCrLf + " ORDER BY TRANDATE DESC"
            Dim dtcnt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtcnt)
            If Not dtcnt.Rows.Count > 0 Then
                sno = "-1"
            ElseIf dtcnt.Rows.Count = 1 Then
                sno = dtcnt.Rows(0).Item("SNO")
            Else
                If dtcnt.Rows.Count > 0 Then
                    strSql = " SELECT SNO,PREVILEGEID,ACCODE,PNAME,MNAME,SNAME,DOORNO,ADDRESS1,ADDRESS2"
                    strSql += vbCrLf + " ,ADDRESS3,AREA,PINCODE,CITY,COUNTRY,PHONERES,MOBILE,IDTYPE,IDNO,PAN,GSTNO"
                    strSql += vbCrLf + " ,(SELECT TOP 1 STATENAME FROM " & cnAdminDb & "..STATEMAST WHERE STATEID=P.STATEID)STATEID,DOB DOBIRTH,ANNIVERSARY,EMAIL EMAILID "
                    strSql += vbCrLf + " FROM " & cnAdminDb & "..PERSONALINFO P"
                    If ADDRESS_SHOWALL_MOBILE Then
                        strSql += vbCrLf + " WHERE MOBILE = '" & txtMobile.Text & "' OR PHONERES = '" & txtMobile.Text & "' "
                        strSql += vbCrLf + " ORDER BY TRANDATE DESC"
                    Else
                        strSql += vbCrLf + " WHERE SNO='" & dtcnt.Rows(0).Item("SNO").ToString & "'"
                    End If

                End If
                sno = BrighttechPack.SearchDialog.Show("Select Customer", strSql, cn)
            End If
            If sno <> "-1" Then
                strSql = " SELECT SNO,PREVILEGEID,ACCODE,TITLE,INITIAL,PNAME,MNAME,SNAME,DOORNO"
                strSql += vbCrLf + " ,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,PINCODE,STATE"
                strSql += vbCrLf + " ,COUNTRY,PHONERES,MOBILE,EMAIL,FAX,PAN,IDTYPE,IDNO"
                strSql += vbCrLf + " ,RELIGION,GSTNO"
                strSql += vbCrLf + " ,(SELECT TOP 1 STATENAME FROM " & cnAdminDb & "..STATEMAST WHERE STATEID=P.STATEID)STATEID,DOB DOBIRTH,ANNIVERSARY"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..PERSONALINFO P"
                strSql += vbCrLf + " WHERE SNO = '" & sno & "'"
                Dim dtAdd As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtAdd)
                If Not dtAdd.Rows.Count > 0 Then Exit Sub
                With dtAdd.Rows(0)
                    If .Item("accode").ToString <> "" Then
                        GLEDGERCODE = objGPack.GetSqlValue("SELECT MACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & .Item("ACCODE").ToString & "'", , , tran).ToString
                    End If
                    txtAddressPrevilegeId.Text = .Item("PREVILEGEID").ToString
                    txtAddressPartyCode.Text = .Item("ACCODE").ToString
                    cmbAddressTitle_OWN.Text = .Item("TITLE").ToString
                    txtAddressInitial.Text = .Item("INITIAL").ToString
                    txtAddressName.Text = .Item("PNAME").ToString
                    txtAddressMName.Text = .Item("MNAME").ToString
                    txtAddressSName.Text = .Item("SNAME").ToString
                    txtAddressDoorNo.Text = .Item("DOORNO").ToString
                    txtAddress1.Text = .Item("ADDRESS1").ToString
                    txtAddress2.Text = .Item("ADDRESS2").ToString
                    txtAddress3.Text = .Item("ADDRESS3").ToString
                    cmbAddressArea_OWN.Text = .Item("AREA").ToString
                    cmbAddressCity_OWN.Text = .Item("CITY").ToString
                    cmbAddressState.Text = .Item("STATE").ToString
                    cmbPlaceOfSupply.Text = .Item("STATEID").ToString
                    cmbAddressCountry_OWN.Text = .Item("COUNTRY").ToString
                    txtAddressPincode_NUM.Text = .Item("PINCODE").ToString
                    txtAddressPhoneRes.Text = .Item("PHONERES").ToString
                    txtAddressMobile.Text = .Item("MOBILE").ToString
                    txtAddressEmailId_OWN.Text = .Item("EMAIL").ToString
                    txtAddressFax.Text = .Item("FAX").ToString
                    txtAddressRegularSno.Text = .Item("SNO").ToString
                    txtAddressPan.Text = .Item("PAN").ToString
                    txtAddressIdNo.Text = .Item("IDNO").ToString
                    cmbIdType.Text = .Item("IDTYPE").ToString
                    txtGSTNo.Text = .Item("GSTNO").ToString
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
                    If InterBill = False Then
                        If cmbPlaceOfSupply.Text.ToString.Trim = "" Then cmbPlaceOfSupply.Text = CompanyState
                        If cmbAddressState.Text = "" Then cmbAddressState.Text = CompanyState
                    End If
                    If _NRIBill Then
                        cmbPlaceOfSupply.Text = ""
                        cmbAddressState.Text = ""
                    End If

                    If .Item("DOBIRTH").ToString.Trim <> "" Then
                        chkDOB.Checked = True
                        Dim tempDOBdate As Date = .Item("DOBIRTH").ToString.Trim
                        dtpDob_OWN.Value = tempDOBdate.Date
                    Else
                        chkDOB.Checked = False
                        dtpDob_OWN.Value = GetServerDate()
                    End If
                    If .Item("ANNIVERSARY").ToString.Trim <> "" Then
                        chkAnniversary.Checked = True
                        Dim tempANVdate As Date = .Item("ANNIVERSARY").ToString.Trim
                        dtpAnniv_OWN.Value = tempANVdate.Date
                    Else
                        chkAnniversary.Checked = False
                        dtpAnniv_OWN.Value = GetServerDate()
                    End If

                    txtAddressPrevilegeId.Select()
                    If ADDRESSUPDATE = "M" Then
                        lockControls(False)
                    Else
                        lockControls(True)
                    End If
                End With
            Else
                Dim accode As String = ""
                strSql = " SELECT PREVILEGEID,ACCODE,ACNAME PNAME,CONVERT(VARCHAR(100),'') AS MNAME,CONVERT(VARCHAR(100),'') AS SNAME,DOORNO,ADDRESS1,ADDRESS2,ADDRESS3,AREA,PINCODE,CITY,COUNTRY,MOBILE"
                strSql += vbCrLf + " ,RELIGION,PAN,GSTNO,DOBIRTH,ANNIVERSARY,EMAILID"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ACHEAD"
                strSql += vbCrLf + " WHERE PHONENO = '" & txtMobile.Text & "' OR MOBILE = '" & txtMobile.Text & "'"
                dtcnt = New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtcnt)
                If Not dtcnt.Rows.Count > 0 Then
                    accode = "-1"
                ElseIf dtcnt.Rows.Count = 1 Then
                    accode = dtcnt.Rows(0).Item("ACCODE")
                End If
                If accode <> "-1" Then
                    strSql = " SELECT '' SNO,PREVILEGEID,ACCODE,TITLE,INITIAL,ACNAME PNAME,CONVERT(VARCHAR(100),'') AS MNAME,CONVERT(VARCHAR(100),'') AS SNAME,DOORNO"
                    strSql += vbCrLf + " ,ADDRESS1,ADDRESS2,ADDRESS3,AREA,CITY,PINCODE"
                    strSql += vbCrLf + " ,STATE,COUNTRY,PHONENO,MOBILE,EMAILID,FAX,PAN,''IDTYPE,''IDNO"
                    strSql += vbCrLf + " ,RELIGION,GSTNO,DOBIRTH,ANNIVERSARY "
                    strSql += vbCrLf + " ,(SELECT TOP 1 STATENAME FROM " & cnAdminDb & "..STATEMAST WHERE STATEID=A.STATEID)STATEID"
                    strSql += vbCrLf + " FROM " & cnAdminDb & "..ACHEAD A"
                    strSql += vbCrLf + " WHERE ACCODE = '" & accode & "'"
                    Dim dtAdd As New DataTable
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(dtAdd)
                    If Not dtAdd.Rows.Count > 0 Then Exit Sub
                    With dtAdd.Rows(0)
                        If .Item("accode").ToString <> "" Then
                            GLEDGERCODE = objGPack.GetSqlValue("SELECT MACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & .Item("ACCODE").ToString & "'", , , tran).ToString
                        End If
                        txtAddressPrevilegeId.Text = .Item("PREVILEGEID").ToString
                        txtAddressPartyCode.Text = .Item("ACCODE").ToString
                        cmbAddressTitle_OWN.Text = .Item("TITLE").ToString
                        txtAddressInitial.Text = .Item("INITIAL").ToString
                        txtAddressName.Text = .Item("PNAME").ToString
                        txtAddressMName.Text = .Item("MNAME").ToString
                        txtAddressSName.Text = .Item("SNAME").ToString
                        txtAddressDoorNo.Text = .Item("DOORNO").ToString
                        txtAddress1.Text = .Item("ADDRESS1").ToString
                        txtAddress2.Text = .Item("ADDRESS2").ToString
                        txtAddress3.Text = .Item("ADDRESS3").ToString
                        cmbAddressArea_OWN.Text = .Item("AREA").ToString
                        cmbAddressCity_OWN.Text = .Item("CITY").ToString
                        cmbAddressState.Text = .Item("STATE").ToString
                        cmbPlaceOfSupply.Text = .Item("STATEID").ToString
                        cmbAddressCountry_OWN.Text = .Item("COUNTRY").ToString
                        txtAddressPincode_NUM.Text = .Item("PINCODE").ToString
                        txtAddressPhoneRes.Text = .Item("PHONENO").ToString
                        txtAddressMobile.Text = .Item("MOBILE").ToString
                        txtAddressEmailId_OWN.Text = .Item("EMAILID").ToString
                        txtAddressFax.Text = .Item("FAX").ToString
                        txtAddressRegularSno.Text = .Item("SNO").ToString
                        txtAddressPan.Text = .Item("PAN").ToString
                        txtAddressIdNo.Text = .Item("IDNO").ToString
                        cmbIdType.Text = .Item("IDTYPE").ToString
                        txtGSTNo.Text = .Item("GSTNO").ToString
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
                        If InterBill = False Then
                            If cmbPlaceOfSupply.Text.ToString.Trim = "" Then cmbPlaceOfSupply.Text = CompanyState
                            If cmbAddressState.Text = "" Then cmbAddressState.Text = CompanyState
                        End If
                        If _NRIBill Then
                            cmbPlaceOfSupply.Text = ""
                            cmbAddressState.Text = ""
                        End If

                        If .Item("DOBIRTH").ToString.Trim <> "" Then
                            chkDOB.Checked = True
                            Dim tempDOBdate As Date = .Item("DOBIRTH").ToString.Trim
                            dtpDob_OWN.Value = tempDOBdate.Date
                        Else
                            chkDOB.Checked = False
                            dtpDob_OWN.Value = GetServerDate()
                        End If
                        If .Item("ANNIVERSARY").ToString.Trim <> "" Then
                            chkAnniversary.Checked = True
                            Dim tempANVdate As Date = .Item("ANNIVERSARY").ToString.Trim
                            dtpAnniv_OWN.Value = tempANVdate.Date
                        Else
                            chkAnniversary.Checked = False
                            dtpAnniv_OWN.Value = GetServerDate()
                        End If

                        txtAddressPrevilegeId.Select()
                        If ADDRESSUPDATE = "M" Then
                            lockControls(False)
                        Else
                            lockControls(True)
                        End If
                    End With
                End If
            End If
            If txtMobile.Text <> "" And txtAddressMobile.Text = "" Then txtAddressMobile.Text = txtMobile.Text
            If txtAddressDoorNo.Enabled = False And txtAddressDoorNo.Text <> "" Then
                If txtAddress1.Text = "" Then txtAddress1.Text = txtAddressDoorNo.Text
                txtAddressDoorNo.Text = ""
            End If
        End If
    End Sub


    Private Sub txtAddressName_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAddressName.Leave
        If Mid(_DRSMAINTAIN, 1, 1) <> "N" Then SetCustomerBalanceGrid() Else SetCustomerBalance(Nothing)
        If txtAddressName.Text <> "" Then SetCustomerPrevTrans("PNAME", txtAddressName.Text.Trim)
        Dim tempnamechk As Boolean = CheckAlphacharacters(txtAddressName.Text.ToString)
        If Addvalid.Contains("N") And txtAddressName.Focused And txtAddressName.Text.Trim = "" Then txtAddressName.Focus() : Exit Sub
        If Addvalid.Contains("N") And txtAddressName.Text.Trim <> "" And tempnamechk = False And isbillingchk = True Then txtAddressName.Focus() : Exit Sub
    End Sub
    Function CheckAlphacharacters(ByVal StringToCheck As String)
        For cntt As Integer = 0 To StringToCheck.Length - 1
            If Char.IsLetter(StringToCheck.Chars(cntt)) Then
                Return True
            End If
        Next
        Return False
    End Function
    Private Sub txtAddress1_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAddress1.Leave
        If Addvalid.Contains("AD1") And txtAddress1.Focused And txtAddress1.Text.Trim = "" Then txtAddress1.Focus() : Exit Sub
    End Sub

    Private Sub txtAddressPan_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAddressPan.Leave
        If Addvalid.Contains("PAN") And txtAddressPan.Focused And txtAddressPan.Text.Trim = "" Then
            txtAddressPan.Focus()
            Exit Sub
        ElseIf txtAddressPan.Focused And txtAddressPan.Text.Trim <> "" Then
            PanInfoValidation(_Sal_value, _Tcs_Value)
        End If
    End Sub
    Private Sub txtAddress2_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAddress2.Leave
        If Addvalid.Contains("AD2") And txtAddress2.Focused And txtAddress2.Text.Trim = "" Then txtAddress2.Focus() : Exit Sub
    End Sub

    Private Sub txtAddress3_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAddress3.Leave
        If Addvalid.Contains("AD3") And txtAddress3.Focused And txtAddress3.Text.Trim = "" Then txtAddress3.Focus() : Exit Sub
    End Sub

    Private Sub txtAddressDoorNo_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAddressDoorNo.Leave
        If Addvalid.Contains("DR") And txtAddressDoorNo.Focused And txtAddressDoorNo.Text.Trim = "" Then txtAddressDoorNo.Focus() : Exit Sub
    End Sub

    Private Sub txtAddressEmailId_OWN_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAddressEmailId_OWN.Leave
        If Addvalid.Contains("EL") And txtAddressEmailId_OWN.Focused And txtAddressEmailId_OWN.Text.Trim = "" Then txtAddressEmailId_OWN.Focus() : Exit Sub
    End Sub

    Private Sub txtAddressFax_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAddressFax.Leave
        If Addvalid.Contains("FX") And txtAddressFax.Focused And txtAddressFax.Text = "".Trim Then txtAddressFax.Focus() : Exit Sub
    End Sub

    Private Sub txtAddressMobile_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAddressMobile.Leave
        If Addvalid.Contains("M") And txtAddressMobile.Focused And txtAddressMobile.Text.Trim = "" Then txtAddressMobile.Focus() : Exit Sub
        If ADDMOB_RESTRICT = "R" And Len(txtAddressMobile.Text) <> 10 Then txtAddressMobile.ReadOnly = False : MsgBox("Mobile No. lentgh restricted with 10 Charectors", MsgBoxStyle.Information) : txtAddressMobile.Focus() : Exit Sub
        If BOUNZ_SALES And txtAddressPrevilegeId.Text <> "" And txtAddressMobile.Text <> "" Then If MsgBox("Can you update the address in Bounz?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then chkBounz.Checked = True Else chkBounz.Checked = False
    End Sub

    Private Sub txtAddressPhoneRes_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAddressPhoneRes.Leave
        If Addvalid.Contains("PR") And txtAddressPhoneRes.Focused And txtAddressPhoneRes.Text.Trim = "" Then txtAddressPhoneRes.Focus() : Exit Sub
    End Sub

    Private Sub txtAddressPincode_NUM_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAddressPincode_NUM.Leave
        ''If txtAddressPincode_NUM.Text = "" Then Exit Sub
        If ADDAREAVSPINCHK Then
            If Addvalid.Contains("PC") And txtAddressPincode_NUM.Text.Trim = "" And txtAddressPincode_NUM.Focused Then txtAddressPincode_NUM.Focus() : Exit Sub Else Getarea("P")
        Else
            If Addvalid.Contains("PC") And txtAddressPincode_NUM.Text.Trim = "" Then
                txtAddressPincode_NUM.Focus() : Exit Sub
            Else
                If Not txtAddressPincode_NUM.Focused Then Getarea("")
            End If
        End If
    End Sub

    Private Sub Getarea(ByVal Flag As String)
        If txtAddressPincode_NUM.Text <> "" Or cmbAddressArea_OWN.Text <> "" Then
            strSql = " SELECT AREANAME,B.CITYNAME,C.STATENAME,C.COUNTRYNAME,PINCODE"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..AREAMAST A "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CITYMAST B ON A.CITYID = B.CITYID LEFT JOIN " & cnAdminDb & "..STATEMAST C  ON B.STATEID = C.STATEID "
            strSql += vbCrLf + " WHERE 1=1 "
            If Flag = "P" And txtAddressPincode_NUM.Text.Trim <> "" Then strSql += vbCrLf + " AND PINCODE= '" & txtAddressPincode_NUM.Text & "'"
            If Flag.Contains("A") And cmbAddressArea_OWN.Text.Trim <> "" Then strSql += vbCrLf + " AND AREANAME= '" & cmbAddressArea_OWN.Text & "'"
            strSql += vbCrLf + " ORDER BY AREANAME "
            Dim dtAdd As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtAdd)
            If Not dtAdd.Rows.Count > 0 Then
                strSql = " SELECT AREANAME,B.CITYNAME,C.STATENAME,C.COUNTRYNAME,PINCODE"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..AREAMAST A "
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CITYMAST B ON A.CITYID = B.CITYID LEFT JOIN " & cnAdminDb & "..STATEMAST C  ON B.STATEID = C.STATEID "
                strSql += vbCrLf + " WHERE 1=1 "
                If txtAddressPincode_NUM.Text.Trim <> "" Then strSql += vbCrLf + " AND PINCODE= '" & txtAddressPincode_NUM.Text & "'"
                If Flag.Contains("A") And cmbAddressArea_OWN.Text.Trim <> "" Then strSql += vbCrLf + " AND AREANAME= '" & cmbAddressArea_OWN.Text & "'"
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtAdd)
                ' Exit Sub
            End If
            If dtAdd.Rows.Count > 0 Then
                With dtAdd.Rows(0)
                    'If cmbAddressArea_OWN.Text = "" Then
                    '    objGPack.FillCombo(strSql, cmbAddressArea_OWN, , False)
                    '    cmbAddressArea_OWN.Text = .Item("AREANAME").ToString
                    'End If
                    'If cmbAddressCity_OWN.Text = "" Then
                    '    cmbAddressCity_OWN.Text = .Item("CITYNAME").ToString
                    'End If
                    'If cmbAddressState_OWN.Text = "" Then
                    '    cmbAddressState_OWN.Text = .Item("STATENAME").ToString
                    'End If
                    'If cmbAddressCountry_OWN.Text = "" Then
                    '    cmbAddressCountry_OWN.Text = .Item("COUNTRYNAME").ToString
                    'End If
                    'If Flag <> "A" Then txtAddressPincode_NUM.Text = .Item("PINCODE").ToString
                    If cmbAddressArea_OWN.Text = "" Then
                        objGPack.FillCombo(strSql, cmbAddressArea_OWN, , False)
                    End If
                    cmbAddressArea_OWN.Text = .Item("AREANAME").ToString
                    cmbAddressCity_OWN.Text = .Item("CITYNAME").ToString
                    cmbAddressState.Text = .Item("STATENAME").ToString
                    cmbPlaceOfSupply.Text = .Item("STATENAME").ToString
                    cmbAddressCountry_OWN.Text = .Item("COUNTRYNAME").ToString
                    txtAddressPincode_NUM.Text = .Item("PINCODE").ToString
                End With
            End If
        End If
    End Sub
    Private Sub cmbAddressArea_OWN_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAddressArea_OWN.Leave
        'If Addvalid.Contains("AR") And cmbAddressArea_OWN.Focused And cmbAddressArea_OWN.Text = "" Then cmbAddressArea_OWN.Focus() : Exit Sub Else Getarea("AE")        
    End Sub

    Private Sub cmbAddressCity_OWN_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAddressCity_OWN.Leave
        If Addvalid.Contains("CY") And cmbAddressCity_OWN.Focused And cmbAddressCity_OWN.Text = "" Then cmbAddressCity_OWN.Focus() : Exit Sub
    End Sub

    Private Sub cmbAddressState_OWN_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAddressState.Leave
        If Addvalid.Contains("ST") And cmbAddressState.Focused And cmbAddressState.Text = "" Then cmbAddressState.Focus() : Exit Sub
    End Sub

    Private Sub cmbAddressCountry_OWN_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAddressCountry_OWN.Leave
        If Addvalid.Contains("CN") And cmbAddressCountry_OWN.Focused And cmbAddressCountry_OWN.Text = "" Then cmbAddressCountry_OWN.Focus() : Exit Sub
    End Sub
    Private Sub txtGvNo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtGvNo.TextChanged

    End Sub

    Private Sub InsertIntoOustanding _
(
ByVal tNo As Integer, ByVal tType As String, ByVal RunNo As String, ByVal Amount As Double,
ByVal RecPay As String,
ByVal Paymode As String,
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
Optional ByVal Flag As String = Nothing,
Optional ByVal EmpId As Integer = Nothing,
Optional ByVal PureWt As Double = Nothing,
Optional ByVal Advwtper As Double = Nothing
    )
        If Amount = 0 And GrsWt = 0 And PureWt = 0 Then Exit Sub
        'Dim accode As String = objAddressDia.txtAddressPartyCode.Text
        strSql = " INSERT INTO " & cnAdminDb & "..OUTSTANDING"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SNO,TRANNO,TRANDATE,TRANTYPE,RUNNO"
        strSql += vbCrLf + " ,AMOUNT,GRSWT,NETWT,PUREWT,RECPAY"
        strSql += vbCrLf + " ,REFNO,REFDATE,EMPID"
        strSql += vbCrLf + " ,TRANSTATUS,PURITY,CATCODE,BATCHNO"
        strSql += vbCrLf + " ,USERID,UPDATED,UPTIME,SYSTEMID"
        strSql += vbCrLf + " ,RATE,ADVFIXWTPER,VALUE,CASHID,VATEXM,REMARK1,REMARK2,ACCODE,CTRANCODE,DUEDATE,APPVER,COMPANYID,COSTID,FROMFLAG,FLAG,PAYMODE)"
        strSql += vbCrLf + " VALUES"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " '" & GetNewSno(TranSnoType.OUTSTANDINGCODE, tran, "GET_ADMINSNO_TRAN") & "'" ''SNO
        strSql += vbCrLf + " ," & tNo & "" 'TRANNO
        strSql += vbCrLf + " ,'" & editBilldate & "'" 'TRANDATE
        strSql += vbCrLf + " ,'" & tType & "'" 'TRANTYPE
        strSql += vbCrLf + " ,'" & RunNo & "'" 'RUNNO
        strSql += vbCrLf + " ," & Math.Abs(Amount) & "" 'AMOUNT
        strSql += vbCrLf + " ," & Math.Abs(GrsWt) & "" 'GRSWT
        strSql += vbCrLf + " ," & Math.Abs(NetWt) & "" 'NETWT
        strSql += vbCrLf + " ," & Math.Abs(PureWt) & "" 'PUREWT
        strSql += vbCrLf + " ,'" & RecPay & "'" 'RECPAY
        strSql += vbCrLf + " ,'" & refNo & "'" 'REFNO
        If refDate <> Nothing Then
            strSql += vbCrLf + " ,'" & refDate & "'" 'REFDATE
        Else
            strSql += vbCrLf + " ,NULL" 'REFDATE
        End If
        If EmpId <> 0 Then
            strSql += vbCrLf + " ," & EmpId & "" 'EMPID
        Else
            strSql += vbCrLf + " ,0" 'EMPID
        End If
        strSql += vbCrLf + " ,''" 'TRANSTATUS
        strSql += vbCrLf + " ," & purity & "" 'PURITY
        strSql += vbCrLf + " ,'" & CatCode & "'" 'CATCODE
        strSql += vbCrLf + " ,'" & editBatchno & "'" 'BATCHNO
        strSql += vbCrLf + " ," & userId & "" 'USERID

        strSql += vbCrLf + " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += vbCrLf + " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += vbCrLf + " ,'" & systemId & "'" 'SYSTEMID
        strSql += vbCrLf + " ," & Rate & "" 'RATE
        strSql += vbCrLf + " ," & Advwtper & "" 'RATE
        strSql += vbCrLf + " ," & Value & "" 'VALUE
        strSql += vbCrLf + " ,''"
        strSql += vbCrLf + " ,'Y'"
        strSql += vbCrLf + " ,'" & Remark1 & "'" 'REMARK1
        strSql += vbCrLf + " ,'" & Remark2 & "'" 'REMARK1
        strSql += vbCrLf + " ,'" & Accode & "'" 'ACCODE
        strSql += vbCrLf + " ," & proId & "" 'CTRANCODE
        If dueDate <> Nothing Then
            strSql += vbCrLf + " ,'" & dueDate & "'" 'DUEDATE
        Else
            strSql += vbCrLf + " ,NULL" 'DUEDATE
        End If
        strSql += vbCrLf + " ,'" & VERSION & "'" 'APPVER
        strSql += vbCrLf + " ,'" & strCompanyId & "'" 'COMPANYID
        strSql += vbCrLf + " ,'" & editCostId & "'" 'COSTID
        strSql += vbCrLf + " ,'P'" 'FROMFLAG
        strSql += vbCrLf + " ,'" & Flag & "'" 'FLAG FOR ORDER ADVANCE REPAY
        strSql += vbCrLf + " ,'" & Paymode & "'"
        strSql += vbCrLf + " )"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, editCostId)
    End Sub

    Private Sub Gift_Personalinfoinsert(ByVal estbilldate As Date, ByVal editBatchno As String)
        Dim psno As String
        If txtAddressRegularSno.Text <> "" And RegularSno Then
            psno = txtAddressRegularSno.Text
        Else
            psno = GetPersonalInfoSno(tran)
        End If
        strSql = " INSERT INTO " & cnAdminDb & "..PERSONALINFO"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SNO,ACCODE,TRANDATE,TITLE"
        strSql += vbCrLf + " ,INITIAL,PNAME,MNAME,SNAME,DOORNO,ADDRESS1"
        strSql += vbCrLf + " ,ADDRESS2,ADDRESS3,AREA,CITY"
        strSql += vbCrLf + " ,STATE,COUNTRY,PINCODE,PHONERES"
        strSql += vbCrLf + " ,MOBILE,EMAIL,FAX,APPVER"
        strSql += vbCrLf + " ,PREVILEGEID,COMPANYID,COSTID,PAN"
        strSql += vbCrLf + " ,IDTYPE,IDNO,IDIMAGEFILE,GSTNO"
        strSql += vbCrLf + " )"
        strSql += vbCrLf + " VALUES"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " '" & psno & "'" ''SNO
        strSql += vbCrLf + " ,'" & txtAddressPartyCode.Text & "'" 'ACCODE
        strSql += vbCrLf + " ,'" & GetEntryDate(editBilldate, tran).ToString("yyyy-MM-dd") & "'" 'TRANDATE
        strSql += vbCrLf + " ,'" & cmbAddressTitle_OWN.Text & "'" 'TITLE
        strSql += vbCrLf + " ,'" & txtAddressInitial.Text & "'" 'INITIAL
        strSql += vbCrLf + " ,'" & txtAddressName.Text & "'" 'PNAME
        strSql += vbCrLf + " ,'" & txtAddressMName.Text & "'" 'MNAME
        strSql += vbCrLf + " ,'" & txtAddressSName.Text & "'" 'SNAME
        strSql += vbCrLf + " ,'" & txtAddressDoorNo.Text & "'" 'DOORNO
        strSql += vbCrLf + " ,'" & txtAddress1.Text & "'" 'ADDRESS1
        strSql += vbCrLf + " ,'" & txtAddress2.Text & "'" 'ADDRESS2
        strSql += vbCrLf + " ,'" & txtAddress3.Text & "'" 'ADDRESS3
        strSql += vbCrLf + " ,'" & cmbAddressArea_OWN.Text & "'" 'AREA
        strSql += vbCrLf + " ,'" & cmbAddressCity_OWN.Text & "'" 'CITY
        strSql += vbCrLf + " ,'" & cmbAddressState.Text & "'" 'STATE
        strSql += vbCrLf + " ,'" & cmbAddressCountry_OWN.Text & "'" 'COUNTRY
        strSql += vbCrLf + " ,'" & txtAddressPincode_NUM.Text & "'" 'PINCODE
        strSql += vbCrLf + " ,'" & txtAddressPhoneRes.Text & "'" 'PHONERES
        strSql += vbCrLf + " ,'" & txtAddressMobile.Text & "'" 'MOBILE
        strSql += vbCrLf + " ,'" & txtAddressEmailId_OWN.Text & "'" 'EMAIL
        strSql += vbCrLf + " ,'" & txtAddressFax.Text & "'" 'Fax
        strSql += vbCrLf + " ,'" & VERSION & "'" 'APPVER
        strSql += vbCrLf + " ,'" & txtAddressPrevilegeId.Text & "'" 'PREVILEGEID
        strSql += vbCrLf + " ,'" & strCompanyId & "'" 'COMPANYID
        strSql += vbCrLf + " ,'" & cnCostId & "'" 'COSTID
        strSql += vbCrLf + " ,'" & txtAddressPan.Text & "'" 'PAN
        strSql += vbCrLf + " ,'" & cmbIdType.Text & "','" & txtAddressIdNo.Text & "',''" 'PAN
        strSql += vbCrLf + " ,'" & txtGSTNo.Text & "'" 'GSTNO
        strSql += vbCrLf + " )"
        ExecQuery(SyncMode.Master, strSql, cn, tran, cnCostId)
        strSql = ""
        strSql = " IF NOT (SELECT COUNT(*) FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = '" & editBatchno & "')>0"
        strSql += vbCrLf + " BEGIN"
        strSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..CUSTOMERINFO"
        strSql += vbCrLf + " (BATCHNO,PSNO,REMARK1,COSTID,PAN)VALUES"
        strSql += vbCrLf + " ('" & editBatchno & "','" & psno & "','" & txtRemark1.Text & "','" & cnCostId & "','" & txtAddressPan.Text & "')"
        strSql += vbCrLf + " END"
        ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)
        strSql = ""
        PersonalinfoInsert = True
        'Return psno
    End Sub

    Private Sub txtAddressPartyCode_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAddressPartyCode.Leave
        If txtAddressPartyCode.Text = "" And (_IsWholeSaleType Or _MiscAcode Or Mand_Partycode) Then
            MsgBox("PartyCode should not empty", MsgBoxStyle.Information)
            txtAddressPartyCode.Focus()
            Exit Sub
        End If
    End Sub

    Private Sub txtAddress_Changed(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAddress1.TextChanged, txtAddress2.TextChanged, txtAddress3.TextChanged, txtAddressName.TextChanged, txtAddressDoorNo.TextChanged, cmbAddressArea_OWN.TextChanged
        If txtAddress1.Focused Or txtAddress2.Focused Or txtAddress3.Focused Or txtAddressDoorNo.Focused Then
            If _TranTypeCol.Contains("PE") = True Or _TranTypeCol.Contains("RE") = True Then txtAddressPartyCode.Text = ""
        End If
        'If cmbAddressArea_OWN.Focused Then Getarea("A")
    End Sub
    Private Sub cmbAddressArea_OWN_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAddressArea_OWN.SelectedIndexChanged
        'If cmbAddressArea_OWN.Text.Trim <> "" Then Getarea("A")
    End Sub
    Private Sub dtpAddressDueDate_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpAddressDueDate.GotFocus
        'If GETDUEDAYS Then
        '    Dim ObjDuedays As New Duedays
        '    If ObjDuedays.ShowDialog = Windows.Forms.DialogResult.OK Then dtpAddressDueDate.Value = GetEntryDate(dtpAddressDueDate.Value, tran).AddDays(Val(ObjDuedays.txtDueday.Text))
        'End If
    End Sub

    Private Sub dtpAddressDueDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtpAddressDueDate.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim strTest As String = ""
            'If (dtpAddressDueDate.Visible And dtpAddressDueDate.Enabled) _
            'Or (txtRemark1.Visible And txtRemark1.Enabled) _
            'Or btnOk.Visible Then
            '    SendKeys.Send("{TAB}")
            'Else
            '    'chkCreateNewAcc.Focus()
            '    If ValidatateAddress() Then
            '        If _IsWholeSaleType Then
            '            If txtAddressName.Text <> "" Then Me.DialogResult = Windows.Forms.DialogResult.OK
            '            Me.Close()
            '        Else
            '            Me.DialogResult = Windows.Forms.DialogResult.OK
            '            Me.Close(t)
            '        End If
            '    End If
            'End If

            'If btnOk.Visible Then Exit Sub
            'chkCreateNewAcc.Focus()
            'If ValidatateAddress() Then
            '    Me.DialogResult = Windows.Forms.DialogResult.OK
            '    Me.Close()
            'End If
        End If
    End Sub

    Private Sub Label114_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label114.Click

    End Sub

    Private Sub btnAttachImage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAttachImage.Click
        If Not IO.Directory.Exists(defalutDestination) Then
            MsgBox(defalutDestination & vbCrLf & " Destination Path not found" & vbCrLf & "Please make the correct path", MsgBoxStyle.Information)
            Exit Sub
        End If

        'If flagDeviceMode = True Then
        ' piccap()
        ' btnSave.Focus()
        ' Exit Sub
        ' End If
        picCapture.Visible = True
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

                Dim Finfo As FileInfo
                Finfo = New FileInfo(openDia.FileName)

                AutoImageSizer(openDia.FileName, picCapture)
                picPath = openDia.FileName
                picExtension = Finfo.Extension

                Me.SelectNextControl(btnAttachImage, True, True, True, True)
            Else
                Me.SelectNextControl(btnAttachImage, True, True, True, True)
            End If
        Catch ex As Exception
            MsgBox(E0016, MsgBoxStyle.Exclamation)
        End Try
    End Sub
    'Private Sub piccap()
    '    Dim data As IDataObject
    '    Dim bmap As Image
    '    If File.Exists(Application.StartupPath & "\tst.jpg") Then File.Delete(Application.StartupPath & "\tst.jpg")
    '    ' Copy image to clipboard 
    '    SendMessage(hHwnd, CAP_EDIT_COPY, 0, 0)
    '    ' Get image from clipboard and convert it to a bitmap 
    '    Data = Clipboard.GetDataObject()
    '    If Data.GetDataPresent(GetType(System.Drawing.Bitmap)) Then
    '        bmap = CType(Data.GetData(GetType(System.Drawing.Bitmap)), Image)
    '        picModel.Image = bmap

    '        picPath = Application.StartupPath & "\tst.jpg"
    '        picExtension = "Jpg"
    '        picModel.Image.Save(picPath, System.Drawing.Imaging.ImageFormat.Jpeg)
    '        picCapture.Visible = False
    '    End If

    'End Sub
    Private Sub grpAddress_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grpAddress.Load

    End Sub

    Private Sub chkIdProof_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIdProof.CheckedChanged
        cmbIdType.Enabled = chkIdProof.Checked
        txtAddressIdNo.Enabled = chkIdProof.Checked
        btnAttachImage.Enabled = chkIdProof.Checked
        If chkIdProof.Checked = False Then cmbIdType.Text = "" : btnAttachImage.Enabled = False
    End Sub
    Private Sub txtAddressIdNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAddressIdNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbIdType.Text.ToUpper = "PAN CARD" And txtAddressPan.Text = "" And txtAddressIdNo.Text <> "" Then txtAddressPan.Text = txtAddressIdNo.Text
        End If
    End Sub

    Private Sub chkAddchange_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAddchange.CheckedChanged
        If chkAddchange.Checked And ADDRESSUPDATE = "Y" Then
            lockControls(False)
            If txtAddressPartyCode.Text <> "" Then
                If MsgBox("Can you update the address in Ac Head also?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then UpdateAchead = True Else UpdateAchead = False
            End If
        Else
            lockControls(True)
            UpdateAchead = False
        End If
    End Sub

    Private Sub txtAddressDueDays_NUM_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAddressDueDays_NUM.Leave
        'If txtAddressDueDays_NUM.Enabled And dtpAddressDueDate.Enabled And Validate_DueDate Then
        '    If txtAddressDueDays_NUM.Text.ToString = "" Then
        '        MsgBox("Due days should not be empty...", MsgBoxStyle.Information)
        '        txtAddressDueDays_NUM.Focus()
        '    End If
        'End If
        If txtAddressDueDays_NUM.Enabled Then dtpAddressDueDate.Value = GetEntryDate(xBilldate, tran).AddDays(Val(txtAddressDueDays_NUM.Text))
    End Sub

    Private Sub txtAddressDueDays_NUM_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAddressDueDays_NUM.TextChanged
        If txtAddressDueDays_NUM.Enabled Then dtpAddressDueDate.Value = GetEntryDate(xBilldate, tran).AddDays(Val(txtAddressDueDays_NUM.Text))
    End Sub

    Private Sub txtAddressMobile_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAddressMobile.KeyPress
        Dim keyChar As String
        keyChar = e.KeyChar
        If (Not Char.IsDigit(keyChar)) And AscW(keyChar) <> 8 And AscW(keyChar) <> 13 And AscW(keyChar) <> 45 And AscW(keyChar) <> 43 And ((AscW(keyChar) = 44 And ADDMOB_RESTRICT <> "N") Or AscW(keyChar) <> 44) And ((AscW(keyChar) = 32 And ADDMOB_RESTRICT <> "N") Or AscW(keyChar) <> 32) Then
            e.Handled = True
            MsgBox("Digits only Allowed 1 to 9")
            txtAddressMobile.Focus()
        End If
    End Sub

    Private Sub txtAddressIdNo_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAddressIdNo.Leave
        If chkIdProof.Checked And cmbIdType.Text <> "" Then
            strSql = "SELECT LENGTH FROM " & cnAdminDb & "..IDPROOF WHERE NAME='" & cmbIdType.Text & "'"
            Dim Len As Integer = objGPack.GetSqlValue(strSql, "LENGTH", 0, tran)
            If Len > 0 Then
                If txtAddressIdNo.Text.Trim.Length <> Len Then
                    MsgBox(cmbIdType.Text & " Must be " & Len & " Characters", MsgBoxStyle.Information)
                    txtAddressIdNo.Select()
                    txtAddressIdNo.Focus()
                End If
            End If
        End If
    End Sub


    Private Sub txtMobile_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMobile.Leave
        If txtMobile.Text <> "" And txtAddressMobile.Text = "" Then
            txtAddressMobile.Text = txtMobile.Text
        End If
        If Addvalid.Contains("M") And txtMobile.Focused And txtMobile.Text.Trim = "" Then txtMobile.Focus() : Exit Sub
        If ADDMOB_RESTRICT = "R" And txtMobile.Text <> "" And Len(txtMobile.Text) <> 10 Then txtMobile.ReadOnly = False : MsgBox("Mobile No. lentgh restricted with 10 Charectors", MsgBoxStyle.Information) : txtMobile.Focus() : Exit Sub
        If ADDMOB_RESTRICT = "R" And Len(txtMobile.Text) = 10 Then
            txtAddressMobile.Text = txtMobile.Text
        End If
    End Sub

    Private Sub txtRemark5_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtRemark5_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = Chr(Keys.Enter) Then
            e.Handled = True
            If btnOk.Visible Then
                btnOk.Select()
                Exit Sub
            End If
            txtAddressPrevilegeId.Select()
            If ValidatateAddress() Then
                If _IsWholeSaleType Then
                    If txtAddressName.Text <> "" Then Me.DialogResult = Windows.Forms.DialogResult.OK
                    Me.Close()
                Else
                    Me.DialogResult = Windows.Forms.DialogResult.OK
                    Me.Close()
                End If
            End If
        End If
    End Sub

    Private Sub txtAddressMobile_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtAddressMobile.KeyUp
        If Len(txtAddressMobile.Text) > 10 Then
            txtAddressMobile.ForeColor = Color.Red
        Else
            txtAddressMobile.ForeColor = Color.DarkGreen
        End If
    End Sub

    Private Sub lblTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblTimer.Tick
        If InterBill Then
            lblBillType.Text = "Other State Bill"
        Else
            lblBillType.Text = "Own State Bill"
        End If
        If lblBillType.Visible = False Then
            lblBillType.Visible = True
        Else
            lblBillType.Visible = False
        End If
    End Sub

    Private Sub cmbPlaceOfSupply_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbPlaceOfSupply.KeyDown
        If e.KeyCode = Keys.Enter Then
            If GstFlag And cmbPlaceOfSupply.Text <> "" And _NRIBill = False Then
                If cmbPlaceOfSupply.Text <> CompanyState And InterBill = False Then
                    MsgBox("If U Selecting Own State Bill , " & vbCrLf & "Please Choose Same State...", MsgBoxStyle.Information)
                    cmbPlaceOfSupply.Focus()
                    Exit Sub
                End If
                If cmbPlaceOfSupply.Text = CompanyState And InterBill And _SezBill = False Then
                    MsgBox("If U Selecting Other State Bill , " & vbCrLf & "Please Choose different State...", MsgBoxStyle.Information)
                    cmbPlaceOfSupply.Focus()
                    Exit Sub
                End If
            End If
        End If
    End Sub
    Private Sub cmbPlaceOfSupply_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPlaceOfSupply.Leave
        If Addvalid.Contains("ST") And cmbPlaceOfSupply.Focused And cmbPlaceOfSupply.Text = "" Then cmbPlaceOfSupply.Focus() : Exit Sub
        If cmbPlaceOfSupply.Text <> "" Then
            strSql = "SELECT STATEID FROM " & cnAdminDb & "..STATEMAST WHERE STATENAME='" & cmbPlaceOfSupply.Text & "'"
            If Val(objGPack.GetSqlValue(strSql, "STATEID", 0).ToString) = 0 Then
                MsgBox("Enter Valid State...", MsgBoxStyle.Information)
                cmbPlaceOfSupply.Focus()
                Exit Sub
            End If
        End If
        If lblBillType.Text = "Other State Bill" Then
            If cmbPlaceOfSupply.Text = "" Then
                cmbPlaceOfSupply.Focus()
            End If
        End If
    End Sub
    Private Sub chkIdProof_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkIdProof.GotFocus
        If Not IO.Directory.Exists(defalutDestination) Then Exit Sub
        If txtAddressRegularSno.Text.ToString = "" Then Exit Sub
        picCapture.Visible = True
        Try
            picPath = defalutDestination
            If picPath.Length > 0 Then
                If picPath.Substring(Len(picPath) - 1, 1) = "\" Then
                    picPath = picPath.Substring(0, Len(picPath) - 1)
                End If
            End If
            picPath = picPath & "\ID" & txtAddressRegularSno.Text.ToString & ".jpg"
            If IO.File.Exists(picPath) Then
                AutoImageSizer(picPath, picCapture)
                Exit Sub
            End If
            picPath = picPath & "\ID" & txtAddressRegularSno.Text.ToString & ".jpeg"
            If IO.File.Exists(picPath) Then
                AutoImageSizer(picPath, picCapture)
                Exit Sub
            End If
            picPath = picPath & "\ID" & txtAddressRegularSno.Text.ToString & ".png"
            If IO.File.Exists(picPath) Then
                AutoImageSizer(picPath, picCapture)
                Exit Sub
            End If
            picPath = picPath & "\ID" & txtAddressRegularSno.Text.ToString & ".bmp"
            If IO.File.Exists(picPath) Then
                AutoImageSizer(picPath, picCapture)
                Exit Sub
            End If
            Exit Sub
        Catch ex As Exception

        End Try

    End Sub

    Private Sub chkDOB_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDOB.CheckedChanged
        If chkDOB.Checked Then
            dtpDob_OWN.Enabled = True
        Else
            dtpDob_OWN.Enabled = False
        End If
    End Sub

    Private Sub chkAnniversary_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAnniversary.CheckedChanged
        If chkAnniversary.Checked Then
            dtpAnniv_OWN.Enabled = True
        Else
            dtpAnniv_OWN.Enabled = False
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnAttachCustomerImage.Click
        If Not IO.Directory.Exists(defalutDestination) Then
            MsgBox(defalutDestination & vbCrLf & " Destination Path not found" & vbCrLf & "Please make the correct path", MsgBoxStyle.Information)
            Exit Sub
        End If

        picCapture.Visible = True
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
                Dim Finfo As FileInfo
                Finfo = New FileInfo(openDia.FileName)
                AutoImageSizer(openDia.FileName, picCapture)
                CustpicPath = openDia.FileName
                CustpicExtension = Finfo.Extension
                Me.SelectNextControl(btnAttachCustomerImage, True, True, True, True)
            Else
                Me.SelectNextControl(btnAttachCustomerImage, True, True, True, True)
            End If
        Catch ex As Exception
            MsgBox(E0016, MsgBoxStyle.Exclamation)
        End Try
    End Sub

    Private Sub txtGSTNo_Leave(sender As Object, e As EventArgs) Handles txtGSTNo.Leave
        If txtGSTNo.Text.ToString <> "" Then
            If GSTNOFORMAT <> "" Then
                If Not formatchkGST(GSTNOFORMAT, txtGSTNo.Text.Trim) Then
                    MsgBox("GST No format (" & GSTNOFORMAT & ") Does not match", MsgBoxStyle.Information)
                    txtGSTNo.Focus()
                    Exit Sub
                End If
            End If
            If GetAdmindbSoftValue("ADDRESS_PANFROMGSTNO", "N") = "Y" And txtGSTNo.Text.Trim.Length = 15 Then
                txtAddressPan.Text = txtGSTNo.Text.Trim.Substring(2, 10)
            End If
        End If
    End Sub

    Private Sub txtDueDate_NUM_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDueDate_NUM.TextChanged
        dtpAddressDueDate.Value = GetEntryDate(xBilldate, tran).AddDays(Val(txtDueDate_NUM.Text))
    End Sub
End Class