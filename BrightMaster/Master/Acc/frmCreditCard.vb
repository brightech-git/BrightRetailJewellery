Imports System.Data.OleDb
Public Class frmCreditCard
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim flagSave As Boolean = False
    Dim cardCode As Integer = Nothing
    Dim tempAcCode As String = Nothing ''For Update purpose
    Dim chitDbFound As Boolean = False
    Dim withother As Boolean = True
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Function funcCallGrid() As Integer
        Dim dt As New DataTable
        dt.Clear()
        strSql = " SELECT CARDCODE CARDCODE,"
        strSql += " NAME,SHORTNAME,"
        strSql += " CASE WHEN CARDTYPE = 'R' THEN 'CREDIT CARD' ELSE"
        strSql += " CASE WHEN CARDTYPE = 'G' THEN 'GIFT VOUCHER' ELSE"
        strSql += " CASE WHEN CARDTYPE = 'C' THEN 'CHIT CARD' ELSE 'CURRENCY' END END END AS CARDTYPE,"
        strSql += " COMMISSION,SURCHARGE,ACCODE,"

        strSql += vbCrLf + "  (SELECT TOP 1 NAME FROM  " & cnAdminDb & "..CREDITCARD WHERE C.DEFAULTAC  = ACCODE)AS DEFAULTAC"
        strSql += vbCrLf + "  ,ISNULL(CURENCY,'')AS CURRENCY"
        strSql += vbCrLf + "  ,(SELECT TOP 1 ACNAME FROM  " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.DEFAULTAC)AS DEFAULTACNAME"
        strSql += vbCrLf + "  ,(SELECT TOP 1 ACNAME FROM  " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.GIFTAC)AS GIFTACNAME"
        strSql += vbCrLf + "  ,(SELECT TOP 1 ACNAME FROM  " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.PRIZEAC)AS PRIZEACNAME"
        strSql += vbCrLf + "  ,(SELECT TOP 1 ACNAME FROM  " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.BONUSAC)AS BONUSACNAME"
        strSql += vbCrLf + "  ,(SELECT TOP 1 ACNAME FROM  " & cnAdminDb & "..ACHEAD WHERE ACCODE = C.DEDUCTAC)AS DEDUCTACNAME "
        strSql += ",(SELECT TOP 1 NAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE  = C.DEFAULTAC)AS DEFAULTAC,"
        strSql += " ISNULL(CURENCY,'')AS CURRENCY,GIFTAC,"
        strSql += " PRIZEAC,BONUSAC,DEDUCTAC,ISNULL(ACTIVE,'')ACTIVE,DISPORDER "
        If chitDbFound = True Then
            strSql += " ,ISNULL((SELECT CNAME FROM " & cnChitCompanyid & "SAVINGS..COMPANY WHERE COMPANYID = COMPCODE),'')AS CNAME"
            strSql += " ,ISNULL((SELECT SCHEMENAME FROM " & cnChitCompanyid & "SAVINGS..SCHEME WHERE CONVERT(VARCHAR,SCHEMEID) = SCHEMECODE),'')AS SCHEMENAME"
        End If
        strSql += " FROM " & cnAdminDb & "..CREDITCARD C"
        Try
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            Return 0
        End Try
        gridView.DataSource = dt
        With gridView
            .Columns("CARDCODE").Width = 60
            .Columns("CARDCODE").HeaderText = "CODE"
            .Columns("NAME").Width = 250
            .Columns("SHORTNAME").Width = 200
            .Columns("CARDTYPE").Width = 120
        End With
        For cnt As Integer = 4 To gridView.ColumnCount - 1
            gridView.Columns(cnt).Visible = False
        Next
        Return 0
    End Function
    Function funcNew()
        tabMain.SelectedTab = tabGeneral
        tempAcCode = Nothing
        cardCode = Nothing
        objGPack.TextClear(Me)
        objGPack.TextClear(grpInfo)
        chkAutoPost.Checked = True

        funcLoadDefaultAcCode()
        cmbCardType.Focus()
        funcCallGrid()
        cmbActive.Text = "YES"
        flagSave = False
        Return 0
    End Function
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(Me) Then Exit Function
        If objGPack.DupChecker(txtCardName__Man, "SELECT 1 FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & txtCardName__Man.Text & "' AND CARDCODE <> '" & cardCode & "'") Then
            Exit Function
        End If
        If flagSave = False Then
            funcAdd()
        ElseIf flagSave = True Then
            funcUpdate()
        End If
    End Function
    Function funcLoadScheme()
        Dim dt As New DataTable
        dt.Clear()
        cmbScheme.Items.Clear()
        strSql = " Select SchemeName from " & cnChitCompanyid & "Savings..Scheme where Companyid = "
        strSql += " (select CompanyId from " & cnChitCompanyid & "Savings..Company where CName = '" & cmbCompany.Text & "')"
        strSql += " order by SchemeName"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Dim cnt As Integer
            For cnt = 0 To dt.Rows.Count - 1
                cmbScheme.Items.Add(dt.Rows(cnt).Item("SchemeName").ToString)
            Next
            cmbScheme.Text = dt.Rows(0).Item("SchemeName").ToString
        End If
        Return 0
    End Function
    Function funcLoadCompany()
        Dim dt As New DataTable
        dt.Clear()
        cmbCompany.Items.Clear()
        strSql = " Select CName from " & cnChitCompanyid & "Savings..Company order by CName"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Dim cnt As Integer
            For cnt = 0 To dt.Rows.Count - 1
                cmbCompany.Items.Add(dt.Rows(cnt).Item("CName").ToString)
            Next
            cmbCompany.Text = dt.Rows(0).Item("CName").ToString
        Else
            MsgBox("SCHEME Company Details not inserted", MsgBoxStyle.Information)
            Me.Close()
        End If
        Return 0
    End Function
    Function funcAdd() As Integer
        If cmbCompany.Visible = True Then
            strSql = " select CompanyId from " & cnChitCompanyid & "Savings..company where CName = '" & cmbCompany.Text & "'"
            Dim tCompId As String = objGPack.GetSqlValue(strSql)
            strSql = " select SchemeId from " & cnChitCompanyid & "Savings..scheme where SchemeName = '" & cmbScheme.Text & "'"
            Dim tSchemeId As String = objGPack.GetSqlValue(strSql)
            strSql = " SELECT 'EXISTS' FROM " & cnAdminDb & "..CREDITCARD WHERE COMPCODE = '" & tCompId & "' AND SCHEMECODE = '" & tSchemeId & "'"
            If objGPack.GetSqlValue(strSql, , "-1") <> "-1" Then
                MsgBox("Already Exists this Company,Scheme Details", MsgBoxStyle.Information)
                cmbCompany.Focus()
                Exit Function
            End If
        End If

        Dim ds As New Data.DataSet
        ds.Clear()
        Dim dr As OleDbDataReader = Nothing
        Dim tran As OleDbTransaction = Nothing
        Dim CardCode As String = Nothing
        Dim AcCode As String = Nothing
        Dim DefaultAcCode As String = Nothing
        Dim GiftAc As String = Nothing
        Dim PrizeAc As String = Nothing
        Dim BonusAc As String = Nothing
        Dim DeductAc As String = Nothing
        Dim CompanyId As String = Nothing
        Dim SchemeId As String = Nothing

        Try
            tran = cn.BeginTransaction()
            If cmbCompany.Visible = True Then
                strSql = " select CompanyId from " & cnChitCompanyid & "Savings..company where CName = '" & cmbCompany.Text & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                da = New OleDbDataAdapter(cmd)
                da.Fill(ds, "CompanyId")
                CompanyId = ds.Tables("CompanyId").Rows(0).Item("CompanyId").ToString

                strSql = " select SchemeId from " & cnChitCompanyid & "Savings..scheme where SchemeName = '" & cmbScheme.Text & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                da = New OleDbDataAdapter(cmd)
                da.Fill(ds, "SchemeId")
                SchemeId = ds.Tables("SchemeId").Rows(0).Item("SchemeId").ToString
            End If

            strSql = " select isnull(max(CardCode),0)+1 as CardCode from " & cnAdminDb & "..creditcard"
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(ds, "CardCode")
            CardCode = ds.Tables("CardCode").Rows(0).Item("CardCode").ToString
            If cmbCardType.Text <> "CREDIT CARD" Then
                AcCode = "CC" + CardCode
            Else
                'Dim IsCardbank As String = GetAdmindbSoftValue("POS_INCL_CCBANK", "N")
                '' accode generate
                strSql = " SELECT CONVERT(INT,MAX(CTLTEXT))AS ACCODE FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'ACCODE'"
                cmd = New OleDbCommand(strSql, cn, tran)
                da = New OleDbDataAdapter(cmd)
                da.Fill(ds, "AcCode")
                tempAcCode = ds.Tables("AcCode").Rows(0).Item("AcCode").ToString
ACCODE_GEN:     tempAcCode += 1
                'If _ExternalCalling Then
                ' AcCode = cnCostId & funcSetNumberStyle(tempAcCode.ToString, 5)
                ' Else
                AcCode = funcSetNumberStyle(tempAcCode.ToString, 7)
                'End If

                If objGPack.DupCheck("SELECT 1 FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & AcCode & "'", tran) Then
                    GoTo ACCODE_GEN
                End If

                strSql = " Update " & cnAdminDb & "..SoftControl "
                strSql += " Set CtlText = '" & tempAcCode & "' "
                strSql += " where CtlId = 'ACCODE' "
                cmd = New OleDbCommand(strSql, cn, tran)
                cmd.ExecuteNonQuery()
                'AcCode = "CC" + CardCode
            End If
            Dim chkaccode As Boolean = False
            If withother Then
                strSql = " Select AcCode from " & cnAdminDb & "..achead where active= 'Y' and acName = '" & cmbDefaultAcCode_OWN.Text & "'order by AcName"
            Else
                strSql = " Select AcCode from " & cnAdminDb & "..CreditCard where Name = '" & cmbDefaultAcCode_OWN.Text & "'"
            End If
            'strSql = " select AcCode from " & cnAdminDb & "..CreditCard where Name = '" & cmbDefaultAcCode.Text & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(ds, "DefaultAcCode")
            If ds.Tables("DefaultAcCode").Rows.Count > 0 Then
                DefaultAcCode = ds.Tables("DefaultAccode").Rows(0).Item("AcCode").ToString
                AcCode = DefaultAcCode
                chkaccode = True
            Else
                DefaultAcCode = AcCode
            End If

            GiftAc = AcCode + "G"
            PrizeAc = AcCode + "P"
            BonusAc = AcCode + "B"
            DeductAc = AcCode + "D"

            strSql = " Insert into " & cnAdminDb & "..CreditCard"
            strSql += " ("
            strSql += " CardType,CardCode,Name,"
            strSql += " ShortName,Commission,Surcharge,"
            strSql += " Accode,DefaultAc,"
            If txtCurrency.Enabled = True Then
                strSql += " Curency,"
            End If
            strSql += " GiftAc,PrizeAc,BonusAc,"
            strSql += " DeductAc,"
            strSql += " COMPCODE,SCHEMECODE,"
            strSql += " UserId,Updated,Uptime,autopost"
            strSql += " ,Active,DispOrder"
            strSql += " ) Values ("
            strSql += " '" & funcGetCardType(cmbCardType) & "'"
            strSql += " ,'" & CardCode & "'"
            strSql += " ,'" & txtCardName__Man.Text & "'"
            strSql += " ,'" & txtShortName.Text & "'"
            strSql += " ," & Val(txtCommision.Text) & ""
            strSql += " ," & Val(txtSurcharge.Text) & ""
            strSql += " ,'" & AcCode & "'"
            strSql += " ,'" & DefaultAcCode & "'"
            If txtCurrency.Enabled = True Then
                strSql += " ,'" & txtCurrency.Text & "'"
            End If
            If cmbCardType.Text = "CHIT CARD" Then
                strSql += " ,'" & GiftAc & "'"
                strSql += " ,'" & PrizeAc & "'"
                strSql += " ,'" & BonusAc & "'"
                strSql += " ,'" & DeductAc & "'"
            Else
                strSql += " ,''"
                strSql += " ,''"
                strSql += " ,''"
                strSql += " ,''"
            End If
            strSql += " ,'" & CompanyId & "'"
            strSql += " ,'" & SchemeId & "'"
            strSql += " ," & userId & ""
            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'"
            strSql += " ,'" & Date.Now.ToLongTimeString & "'"
            If cmbCardType.Text = "CHIT CARD" Then
                strSql += ",'" & IIf(chkAutoPost.Checked, "Y", "N") & "'" 'AUTOPOST
            Else
                strSql += ",''"
            End If
            strSql += ",'" & Mid(cmbActive.Text, 1, 1) & "'"
            strSql += " ," & Val(txtDispOrder.Text) & ""
            strSql += " )"
            ExecQuery(SyncMode.Master, strSql, cn, tran)

            ''Insert into AcHead
            If chkaccode = False Then
                If cmbCardType.Text <> "CHIT CARD" Then
                    strSql = " insert into " & cnAdminDb & "..AcHead("
                    strSql += " AcCode,AcName,ACGrpCode,ACSubGrpCode,"
                    strSql += " AcType,DoorNo,Address1,Address2,"
                    strSql += " Address3,Area,City,Pincode,"
                    strSql += " PhoneNo,Mobile,"
                    strSql += " Emailid,"
                    strSql += " WebSite,Ledprint,TdsFlag,TdsPer,"
                    strSql += " Depflag,Depper,Outstanding,AutoGen,"
                    strSql += " VATEX,LocalOutst,LocalTaxNo,CentralTaxNo,"
                    strSql += " Userid,CrDate,CrTime,active)values("
                    strSql += " '" & AcCode & "','" & txtCardName__Man.Text & "','2','0',"
                    strSql += " 'O','','','',"
                    strSql += " '','','','',"
                    strSql += " '','',"
                    strSql += " '',"
                    strSql += " '','','',0,"
                    strSql += " '',0,'','',"
                    strSql += " '','','','',"
                    strSql += " " & userId & ",'" & Today.Date.ToString("yyyy-MM-dd") & "','" & Date.Now.ToLongTimeString & "','Y')"
                    ExecQuery(SyncMode.Master, strSql, cn, tran)
                Else
                    strSql = " insert into " & cnAdminDb & "..AcHead("
                    strSql += " AcCode,AcName,ACGrpCode,ACSubGrpCode,"
                    strSql += " AcType,DoorNo,Address1,Address2,"
                    strSql += " Address3,Area,City,Pincode,"
                    strSql += " PhoneNo,Mobile,"
                    strSql += " Emailid,"
                    strSql += " WebSite,Ledprint,TdsFlag,TdsPer,"
                    strSql += " Depflag,Depper,Outstanding,AutoGen,"
                    strSql += " VATEX,LocalOutst,LocalTaxNo,CentralTaxNo,"
                    strSql += " Userid,CrDate,CrTime,active)values("
                    strSql += " '" & AcCode & "','" & txtCardName__Man.Text & "','2','0',"
                    strSql += " 'O','','','',"
                    strSql += " '','','','',"
                    strSql += " '','',"
                    strSql += " '',"
                    strSql += " '','','',0,"
                    strSql += " '',0,'','',"
                    strSql += " '','','','',"
                    strSql += " " & userId & ",'" & Today.Date.ToString("yyyy-MM-dd") & "','" & Date.Now.ToLongTimeString & "','Y')"
                    ExecQuery(SyncMode.Master, strSql, cn, tran)

                    ''GiftAc
                    strSql = " insert into " & cnAdminDb & "..AcHead("
                    strSql += " AcCode,AcName,ACGrpCode,ACSubGrpCode,"
                    strSql += " AcType,DoorNo,Address1,Address2,"
                    strSql += " Address3,Area,City,Pincode,"
                    strSql += " PhoneNo,Mobile,"
                    strSql += " Emailid,"
                    strSql += " WebSite,Ledprint,TdsFlag,TdsPer,"
                    strSql += " Depflag,Depper,Outstanding,AutoGen,"
                    strSql += " VATEX,LocalOutst,LocalTaxNo,CentralTaxNo,"
                    strSql += " Userid,CrDate,CrTime)values("
                    strSql += " '" & GiftAc & "','" & txtCardName__Man.Text & " GiftAccount','2','0',"
                    strSql += " 'O','','','',"
                    strSql += " '','','','',"
                    strSql += " '','',"
                    strSql += " '',"
                    strSql += " '','','',0,"
                    strSql += " '',0,'','',"
                    strSql += " '','','','',"
                    strSql += " " & userId & ",'" & Today.Date.ToString("yyyy-MM-dd") & "','" & Date.Now.ToLongTimeString & "')"
                    ExecQuery(SyncMode.Master, strSql, cn, tran)

                    ''PrizeAc
                    strSql = " insert into " & cnAdminDb & "..AcHead("
                    strSql += " AcCode,AcName,ACGrpCode,ACSubGrpCode,"
                    strSql += " AcType,DoorNo,Address1,Address2,"
                    strSql += " Address3,Area,City,Pincode,"
                    strSql += " PhoneNo,Mobile,"
                    strSql += " Emailid,"
                    strSql += " WebSite,Ledprint,TdsFlag,TdsPer,"
                    strSql += " Depflag,Depper,Outstanding,AutoGen,"
                    strSql += " VATEX,LocalOutst,LocalTaxNo,CentralTaxNo,"
                    strSql += " Userid,CrDate,CrTime)values("
                    strSql += " '" & PrizeAc & "','" & txtCardName__Man.Text & " Prize Account','2','0',"
                    strSql += " 'O','','','',"
                    strSql += " '','','','',"
                    strSql += " '','',"
                    strSql += " '',"
                    strSql += " '','','',0,"
                    strSql += " '',0,'','',"
                    strSql += " '','','','',"
                    strSql += " " & userId & ",'" & Today.Date.ToString("yyyy-MM-dd") & "','" & Date.Now.ToLongTimeString & "')"
                    ExecQuery(SyncMode.Master, strSql, cn, tran)

                    ''BonusAc
                    strSql = " insert into " & cnAdminDb & "..AcHead("
                    strSql += " AcCode,AcName,ACGrpCode,ACSubGrpCode,"
                    strSql += " AcType,DoorNo,Address1,Address2,"
                    strSql += " Address3,Area,City,Pincode,"
                    strSql += " PhoneNo,Mobile,"
                    strSql += " Emailid,"
                    strSql += " WebSite,Ledprint,TdsFlag,TdsPer,"
                    strSql += " Depflag,Depper,Outstanding,AutoGen,"
                    strSql += " VATEX,LocalOutst,LocalTaxNo,CentralTaxNo,"
                    strSql += " Userid,CrDate,CrTime)values("
                    strSql += " '" & BonusAc & "','" & txtCardName__Man.Text & " Bonus Account','2','0',"
                    strSql += " 'O','','','',"
                    strSql += " '','','','',"
                    strSql += " '','',"
                    strSql += " '',"
                    strSql += " '','','',0,"
                    strSql += " '',0,'','',"
                    strSql += " '','','','',"
                    strSql += " " & userId & ",'" & Today.Date.ToString("yyyy-MM-dd") & "','" & Date.Now.ToLongTimeString & "')"
                    ExecQuery(SyncMode.Master, strSql, cn, tran)

                    ''DeductAc
                    strSql = " insert into " & cnAdminDb & "..AcHead("
                    strSql += " AcCode,AcName,ACGrpCode,ACSubGrpCode,"
                    strSql += " AcType,DoorNo,Address1,Address2,"
                    strSql += " Address3,Area,City,Pincode,"
                    strSql += " PhoneNo,Mobile,"
                    strSql += " Emailid,"
                    strSql += " WebSite,Ledprint,TdsFlag,TdsPer,"
                    strSql += " Depflag,Depper,Outstanding,AutoGen,"
                    strSql += " VATEX,LocalOutst,LocalTaxNo,CentralTaxNo,"
                    strSql += " Userid,CrDate,CrTime)values("
                    strSql += " '" & DeductAc & "','" & txtCardName__Man.Text & " Deduct Account','2','0',"
                    strSql += " 'O','','','',"
                    strSql += " '','','','',"
                    strSql += " '','',"
                    strSql += " '',"
                    strSql += " '','','',0,"
                    strSql += " '',0,'','',"
                    strSql += " '','','','',"
                    strSql += " " & userId & ",'" & Today.Date.ToString("yyyy-MM-dd") & "','" & Date.Now.ToLongTimeString & "')"
                    ExecQuery(SyncMode.Master, strSql, cn, tran)
                End If
            End If

            If Not tran Is Nothing Then tran.Commit()
            funcNew()
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            tran.Dispose()
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Function funcUpdate()
        Dim ds As New Data.DataSet
        ds.Clear()
        Dim CompanyId As String = Nothing
        Dim SchemeId As String = Nothing
        Dim DefaultAcCode As String = Nothing

        Dim tran As OleDbTransaction = Nothing

        Try
            tran = cn.BeginTransaction()

            If cmbCompany.Visible = True Then
                strSql = " select CompanyId from " & cnChitCompanyid & "Savings..company where CName = '" & cmbCompany.Text & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                da = New OleDbDataAdapter(cmd)
                da.Fill(ds, "CompanyId")
                CompanyId = ds.Tables("CompanyId").Rows(0).Item("CompanyId").ToString

                strSql = " select SchemeId from " & cnChitCompanyid & "Savings..scheme where SchemeName = '" & cmbScheme.Text & "'"
                cmd = New OleDbCommand(strSql, cn, tran)
                da = New OleDbDataAdapter(cmd)
                da.Fill(ds, "SchemeId")
                SchemeId = ds.Tables("SchemeId").Rows(0).Item("SchemeId").ToString
            End If

            If withother Then
                strSql = " Select AcCode from " & cnAdminDb & "..achead where active= 'Y' and acName = '" & cmbDefaultAcCode_OWN.Text & "'order by AcName"
            Else
                strSql = " Select AcCode from " & cnAdminDb & "..CreditCard where Name = '" & cmbDefaultAcCode_OWN.Text & "'"
            End If

            'strSql = " select AcCode from " & cnAdminDb & "..CreditCard where Name = '" & cmbDefaultAcCode.Text & "'"
            cmd = New OleDbCommand(strSql, cn, tran)
            da = New OleDbDataAdapter(cmd)
            da.Fill(ds, "DefaultAcCode")
            If ds.Tables("DefaultAcCode").Rows.Count > 0 Then
                DefaultAcCode = ds.Tables("DefaultAcCode").Rows(0).Item("AcCode").ToString
            Else
                DefaultAcCode = tempAcCode
            End If

            '''''''''''''''''''''''
            strSql = " Update " & cnAdminDb & "..CreditCard Set "
            strSql += " CardType = '" & funcGetCardType(cmbCardType) & "'"
            strSql += " ,Name = '" & txtCardName__Man.Text & "'"
            strSql += " ,ShortName = '" & txtShortName.Text & "'"
            strSql += " ,Commission = " & Val(txtCommision.Text) & ""
            strSql += " ,Surcharge = " & Val(txtSurcharge.Text) & ""
            strSql += " ,DefaultAc = '" & DefaultAcCode & "'"
            If txtCurrency.Enabled = True Then
                strSql += " ,Curency = '" & txtCurrency.Text & "'"
            Else
                strSql += " ,Curency = ''"
            End If
            If cmbCompany.Visible = True Then
                strSql += " ,CompCode = '" & CompanyId & "'"
                strSql += " ,SchemeCode = '" & SchemeId & "'"
            Else
                strSql += " ,GiftAc = ''"
                strSql += " ,PrizeAc = ''"
                strSql += " ,BonusAc = ''"
                strSql += " ,DeductAc = ''"
                strSql += " ,CompCode = ''"
                strSql += " ,SchemeCode = ''"
            End If
            strSql += " ,UserId = " & userId & ""
            strSql += " ,Updated = '" & Today.Date.ToString("yyyy-MM-dd") & "'"
            strSql += " ,Uptime = '" & Date.Now.ToLongTimeString & "'"
            If cmbCompany.Visible = True Then
                strSql += ",AUTOPOST = '" & IIf(chkAutoPost.Checked, "Y", "N") & "'"
            End If
            strSql += " ,Active = '" & Mid(cmbActive.Text, 1, 1) & "'"
            strSql += " ,Disporder= " & Val(txtDispOrder.Text) & ""
            strSql += " Where CardCode = " & cardCode & ""
            ExecQuery(SyncMode.Master, strSql, cn, tran)

            strSql = " Update " & cnAdminDb & "..ACHEAD SET ACNAME = '" & txtCardName__Man.Text & "'"
            strSql += " Where ACCODE = '" & objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..CREDITCARD WHERE CARDCODE = " & cardCode & "", , , tran) & "'"
            ExecQuery(SyncMode.Master, strSql, cn, tran)
            If Not tran Is Nothing Then tran.Commit()
            funcNew()
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
        Return 0
    End Function
    Function funcCheckChitDb() As Boolean
        Dim dt As New DataTable
        dt.Clear()
        Dim QRY As String
        'QRY = " Select CtlText from " & cnAdminDb & "..Softcontrol where ctlId = 'ChitDb' and ctlText = 'Y'"
        QRY = "select * from sysdatabases where name = '" & cnChitCompanyid & "Savings'"
        'strSql = "select * from sysdatabases where name = '" & cnChitCompanyId & "Savings'"
        da = New OleDbDataAdapter(QRY, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return False
        End If
        Return True
    End Function
    Function funcCheckDefaultAcCode() As Boolean
        Dim dt As New DataTable
        dt.Clear()
        'If funcSqValidation(Me) = True Then
        '    Return False
        'End If
        strSql = " Select Name from " & cnAdminDb & "..CreditCard where CardType = '" & funcGetCardType(cmbCardType) & "' and Name = '" & txtCardName__Man.Text & "'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return False
        End If
        Return True
    End Function
    Function funcLoadDefaultAcCode()
        Dim dt As New DataTable
        dt.Clear()
        cmbDefaultAcCode_OWN.Items.Clear()
        cmbDefaultAcCode_OWN.Text = ""

        If withother Then
            strSql = " SELECT ACNAME AS NAME FROM " & cnAdminDb & "..ACHEAD WHERE ISNULL(ACTIVE,'')<>'N' AND ACGRPCODE IN(SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP  WHERE GRPLEDGER<>'T') ORDER BY ACNAME"
        Else
            strSql = " SELECT NAME FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE = '" & funcGetCardType(cmbCardType) & "' ORDER BY NAME"
        End If
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        Dim cnt As Integer = Nothing
        For cnt = 0 To dt.Rows.Count - 1
            With dt.Rows(cnt)
                cmbDefaultAcCode_OWN.Items.Add(.Item("Name").ToString)
            End With
        Next
        cmbDefaultAcCode_OWN.Text = dt.Rows(0).Item("Name").ToString
        Return 0
    End Function

    Private Sub frmCreditCard_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                tabMain.SelectedTab = tabGeneral
            End If
        End If
    End Sub

    Private Sub frmCreditCard_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub GrpFieldPos()
        If funcCheckChitDb() = True Then
            'Available
            'grpField.Location = New System.Drawing.Point(350, 300)
            If pnlChit.Visible Then
                pnlChit.Visible = True
                pnlChit.BringToFront()
                pnl1.Location = New Point(8, 16)
                pnlChit.Location = New System.Drawing.Point(8, 90)
                pnl2.Location = New Point(8, 139)
                Panel1.Location = New Point(8, 216)
                pnlButtons.Location = New System.Drawing.Point(8, 240)
                grpField.Size = New System.Drawing.Size(500, 300)
            Else
                pnl1.Location = New Point(8, 16)
                pnlChit.Location = New System.Drawing.Point(8, 224)
                pnl2.Location = New Point(8, 91)
                Panel1.Location = New Point(8, 166)
                pnlButtons.Location = New System.Drawing.Point(8, 190)
                grpField.Size = New System.Drawing.Size(500, 250)
            End If
            grpField.Location = New System.Drawing.Point(350, (Me.Height / 2) - (grpField.Height / 2))
        Else
            'Not Available
            'grpField.Location = New System.Drawing.Point(350, 300)
            pnlChit.Visible = False
            pnlChit.BringToFront()
            chitDbFound = False
            pnl1.Location = New Point(8, 16)
            pnlChit.Location = New System.Drawing.Point(8, 224)
            pnl2.Location = New Point(8, 91)
            Panel1.Location = New Point(8, 166)
            pnlButtons.Location = New System.Drawing.Point(8, 190)
            grpField.Size = New System.Drawing.Size(500, 250)
            grpField.Location = New System.Drawing.Point(350, (Me.Height / 2) - (grpField.Height / 2))
        End If
    End Sub

    Private Sub GrpFieldPosOld()
        If funcCheckChitDb() = True Then
            'Available
            If pnlChit.Visible Then
                pnlChit.Visible = True
                pnlChit.BringToFront()
                pnl1.Location = New Point(6, 16)
                pnlChit.Location = New System.Drawing.Point(6, 90)
                pnl2.Location = New Point(6, 139)
                Panel1.Location = New Point(6, 216)
                pnlButtons.Location = New System.Drawing.Point(6, 240)
                grpField.Size = New System.Drawing.Size(450, 300)
            Else
                pnl1.Location = New Point(6, 16)
                pnlChit.Location = New System.Drawing.Point(8, 224)
                pnl2.Location = New Point(6, 91)
                Panel1.Location = New Point(6, 166)
                pnlButtons.Location = New System.Drawing.Point(6, 190)
                grpField.Size = New System.Drawing.Size(450, 250)
            End If
        Else
            'Not Available
            pnlChit.Visible = False
            pnlChit.BringToFront()
            chitDbFound = False
            pnl1.Location = New Point(6, 16)
            pnlChit.Location = New System.Drawing.Point(8, 224)
            pnl2.Location = New Point(6, 91)
            Panel1.Location = New Point(6, 166)
            pnlButtons.Location = New System.Drawing.Point(6, 190)
            grpField.Size = New System.Drawing.Size(450, 250)
        End If
    End Sub
    Private Sub frmCreditCard_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcGridStyle(gridView)
        tabMain.ItemSize = New Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))

        Dim dt As New DataTable
        dt.Clear()
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'CHITDBPREFIX'"

        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            cnChitCompanyid = dt.Rows(0).Item("ctlText").ToString
        End If
        If funcCheckChitDb() = True Then
            'Available

            funcLoadCompany()
            chitDbFound = True
            'pnlChit.BringToFront()
            'pnl1.Location = New Point(6, 16)
            'pnlChit.Location = New System.Drawing.Point(6, 66)
            'pnl2.Location = New Point(6, 116)
            'pnlButtons.Location = New System.Drawing.Point(4, 164)
            'grpField.Size = New System.Drawing.Size(395, 201)
        Else
            'Not Available
            'MsgBox("Chit DB Not Found...")
            chitDbFound = False
            'pnl1.Location = New Point(6, 16)
            'pnlChit.Location = New System.Drawing.Point(138, 65)
            'pnl2.Location = New Point(6, 66)
            'pnlButtons.Location = New System.Drawing.Point(4, 113)
            'grpField.Size = New System.Drawing.Size(395, 149)
        End If
        GrpFieldPos()

        cmbCardType.Items.Add("CREDIT CARD")
        cmbCardType.Items.Add("GIFT VOUCHER")
        cmbCardType.Items.Add("CHIT CARD")
        cmbCardType.Items.Add("CURRENCY")
        cmbCardType.Text = "CREDIT CARD"
        funcLoadDefaultAcCode()
        funcNew()
    End Sub

    Private Sub cmbCardType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCardType.SelectedIndexChanged
        If cmbCardType.Text = "CURRENCY" Then
            txtCurrency.Enabled = True
            txtCommision.Enabled = False
            txtSurcharge.Enabled = False
            chkOtherAc.Enabled = False
        ElseIf cmbCardType.Text = "CHIT CARD" Then
            txtCommision.Enabled = False
            txtSurcharge.Enabled = False
            txtCurrency.Enabled = False
            chkOtherAc.Enabled = False
        Else
            txtCurrency.Enabled = False
            txtCommision.Enabled = True
            txtSurcharge.Enabled = True
            chkOtherAc.Enabled = True
        End If
        If cmbCardType.Text = "CHIT CARD" Then
            If funcCheckChitDb() = True Then
                funcLoadCompany()
                pnlChit.Visible = True
            End If
        Else
            pnlChit.Visible = False
        End If
        GrpFieldPos()
        'funcLoadDefaultAcCode()
    End Sub


    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        funcSave()
    End Sub
    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        btnOpen_Click(Me, New EventArgs)
    End Sub
    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
    End Sub
    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        funcCallGrid()
        tabMain.SelectedTab = tabView
        gridView.Focus()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub

    Private Sub cmbCompany_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCompany.SelectedIndexChanged
        funcLoadScheme()
    End Sub
    Function funcGetDetails(ByVal tempCardCode As Integer)
        Dim dt As New DataTable
        dt.Clear()
        Try
            strSql = " select CardCode,"
            strSql += vbCrLf + " Name,ShortName,"
            strSql += vbCrLf + " case when CardType = 'R' then 'CREDIT CARD' else"
            strSql += vbCrLf + " case when CardType = 'G' then 'GIFT VOUCHER' else"
            strSql += vbCrLf + " case when CardType = 'C' then 'CHIT CARD' else 'CURRENCY' end end end as CardType,"
            strSql += vbCrLf + " Commission,Surcharge,AcCode,"
            strSql += vbCrLf + " (select top 1 acName from " & cnAdminDb & "..achead where  AcCode=C.DefaultAc)as DefaultAc,"
            strSql += vbCrLf + " isnull(Curency,'')as Currency,GiftAc"
            strSql += vbCrLf + " PrizeAc,BonusAc,DeductAc,autopost,ISNULL(DISPORDER,'0')DISPORDER"
            If funcCheckChitDb() Then
                strSql += vbCrLf + " ,isnull((select CName from " & cnChitCompanyid & "Savings..Company where CompanyId = CompCode),'')as CName"
                strSql += vbCrLf + " ,isnull((select SchemeName from " & cnChitCompanyid & "Savings..Scheme where CONVERT(VARCHAR,SchemeId) = SchemeCode),'')as SchemeName"
            Else
                strSql += vbCrLf + " ,''CNAME,''SCHEMENAME"
            End If
            strSql += vbCrLf + " from " & cnAdminDb & "..CreditCard as C where CardCode = '" & tempCardCode & "'"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If Not dt.Rows.Count > 0 Then
                Return 0
            End If
            With dt.Rows(0)
                txtCardName__Man.Text = .Item("Name").ToString
                txtShortName.Text = .Item("ShortName").ToString
                cmbCardType.Text = .Item("CardType").ToString
                txtCommision.Text = .Item("Commission").ToString
                txtSurcharge.Text = .Item("Surcharge").ToString
                cmbDefaultAcCode_OWN.Text = .Item("DefaultAc").ToString
                txtCurrency.Text = .Item("Currency").ToString
                cardCode = tempCardCode
                tempAcCode = .Item("AcCode").ToString
                If .Item("AUTOPOST").ToString = "Y" Then
                    chkAutoPost.Checked = True
                Else
                    chkAutoPost.Checked = False
                End If
                tabMain.SelectedTab = tabGeneral
                cmbCardType_SelectedIndexChanged(Me, New EventArgs)
                cmbCompany.Text = .Item("CName").ToString
                cmbScheme.Text = .Item("SchemeName").ToString
                txtDispOrder.Text = .Item("DISPORDER").ToString
            End With
            cmbCardType.Select()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        Return 0
    End Function

    Private Sub txtCommision_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCommision.KeyPress
        Dim keyChar As String
        keyChar = e.KeyChar
        If AscW(e.KeyChar) = 46 Then
            If txtCommision.Text.Contains(".") = True Then
                e.Handled = True
            End If
        End If
        If (Not Char.IsDigit(keyChar)) And AscW(keyChar) <> 8 And AscW(keyChar) <> 13 And AscW(keyChar) <> 46 Then
            e.Handled = True
            MsgBox("Digits only Allowed 1 to 9")
            txtCommision.Focus()
        End If
    End Sub

    Private Sub txtSurcharge_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSurcharge.KeyPress
        Dim keyChar As String
        keyChar = e.KeyChar
        If AscW(e.KeyChar) = 46 Then
            If txtSurcharge.Text.Contains(".") = True Then
                e.Handled = True
            End If
        End If
        If (Not Char.IsDigit(keyChar)) And AscW(keyChar) <> 8 And AscW(keyChar) <> 13 And AscW(keyChar) <> 46 Then
            e.Handled = True
            MsgBox("Digits only Allowed 1 to 9")
            txtSurcharge.Focus()
        End If
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblStatus.Visible = True
    End Sub
    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridView.RowCount > 0 Then
                funcGetDetails(gridView.Item(0, gridView.CurrentRow.Index).Value.ToString)
                flagSave = True
                cmbCardType.Focus()
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            cmbCardType.Focus()
        End If
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus

        lblStatus.Visible = False
    End Sub

    Private Sub gridView_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.RowEnter
        objGPack.TextClear(grpInfo)
        With gridView.Rows(e.RowIndex)
            detCommision.Text = .Cells("COMMISSION").Value.ToString
            detSurcharge.Text = .Cells("SURCHARGE").Value.ToString
            detDefaultAcc.Text = .Cells("DEFAULTACNAME").Value.ToString
            detCurrency.Text = .Cells("CURRENCY").Value.ToString
            detGiftAcc.Text = .Cells("GIFTACNAME").Value.ToString
            detPrizeAcc.Text = .Cells("PRIZEACNAME").Value.ToString
            detBonusAcc.Text = .Cells("BONUSACNAME").Value.ToString
            detDeductAcc.Text = .Cells("DEDUCTACNAME").Value.ToString
            DetActive.Text = .Cells("ACTIVE").Value.ToString
            DetDisp.Text = .Cells("DISPORDER").Value.ToString
            If chitDbFound Then
                detCompanyName.Text = .Cells("CNAME").Value.ToString
                detSchemeName.Text = .Cells("SCHEMENAME").Value.ToString
            End If
        End With
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not gridView.RowCount > 0 Then
            Exit Sub
        End If
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        Dim chkQry As String = Nothing
        Dim delKey As String = gridView.Rows(gridView.CurrentRow.Index).Cells("CARDCODE").Value.ToString

        strSql = " SELECT DBNAME FROM " & cnAdminDb & "..DBMASTER"
        Dim dtDb As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtDb)
        If dtDb.Rows.Count > 0 Then
            For cnt As Integer = 0 To dtDb.Rows.Count - 1
                With dtDb.Rows(cnt)
                    chkQry += " SELECT TOP 1 convert(varchar(10),CARDID) FROM " & .Item("DBNAME").ToString & "..ACCTRAN WHERE CARDID = '" & delKey & "'"
                    chkQry += " UNION "
                    chkQry += " SELECT TOP 1 ACCODE FROM " & .Item("DBNAME").ToString & "..ACCTRAN WHERE ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..CREDITCARD WHERE CARDCODE = '" & delKey & "'))"
                    chkQry += " UNION "
                    chkQry += " SELECT TOP 1 ACCODE FROM " & .Item("DBNAME").ToString & "..ACCTRAN WHERE ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE = (SELECT DEFAULTAC FROM " & cnAdminDb & "..CREDITCARD WHERE CARDCODE = '" & delKey & "'))"
                    If cnt <> dtDb.Rows.Count - 1 Then
                        chkQry += " UNION "
                    End If
                End With
            Next
            Dim delQry As String = Nothing
            delQry += " DELETE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..CREDITCARD WHERE CARDCODE = '" & delKey & "')"
            delQry += " DELETE FROM " & cnAdminDb & "..CREDITCARD WHERE CARDCODE = '" & delKey & "'"
            DeleteItem(SyncMode.Master, chkQry, delQry)
            funcCallGrid()
        End If
    End Sub

    Private Sub txtCardName__Man_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCardName__Man.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If objGPack.DupChecker(txtCardName__Man, "SELECT 1 FROM " & cnAdminDb & "..CREDITCARD WHERE NAME = '" & txtCardName__Man.Text & "' AND CARDCODE <> '" & cardCode & "'") Then
                Exit Sub
            End If
        End If
    End Sub

    Private Sub txtCardName__Man_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCardName__Man.LostFocus
        If txtCardName__Man.Text <> "" Then
            If funcCheckDefaultAcCode() = False Then
                cmbDefaultAcCode_OWN.Items.Clear()
                funcLoadDefaultAcCode()
                cmbDefaultAcCode_OWN.Items.Add(txtCardName__Man.Text)
                cmbDefaultAcCode_OWN.Text = txtCardName__Man.Text
            Else
                funcLoadDefaultAcCode()
                cmbDefaultAcCode_OWN.Text = txtCardName__Man.Text
            End If
        End If
    End Sub
    Function funcGetCardType(ByVal f As ComboBox) As Char
        If f.Text = "CREDIT CARD" Then
            Return "R"
        ElseIf f.Text = "GIFT VOUCHER" Then
            Return "G"
        ElseIf f.Text = "CHIT CARD" Then
            Return "C"
        Else
            Return "U"
        End If
    End Function

    Private Sub chkAutoPost_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAutoPost.CheckedChanged

    End Sub

    Private Sub chkOtherAc_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOtherAc.CheckedChanged
        If chkOtherAc.Checked Then withother = True Else withother = False
        funcLoadDefaultAcCode()
    End Sub

End Class