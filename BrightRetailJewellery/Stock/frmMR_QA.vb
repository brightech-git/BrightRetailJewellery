Imports System.Data.OleDb
Imports System.Runtime.InteropServices
Imports System.Web.UI.WebControls
Public Class frmMR_QA
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim dReader As OleDbDataReader
    Dim tran As OleDbTransaction

    Dim strSql As String

    Dim TranNo As String = Nothing
    Dim EntryOrder As Integer = 0
    Dim multipleTagFlag As String = Nothing
    Dim pieceRate As Decimal = 0
    Dim pcs As Integer = 0
    Dim narration As String

    Dim purFineRate As Double
    Dim purTouch As Double


    Dim TagNoGen As String = Nothing
    Dim TagNoFrom As String = Nothing
    Dim LastTagNo As String = Nothing

    Dim objTag As New TagGeneration
    Dim actPieces As Integer = 0
    Dim actGrsWt As Double = 0
    Dim actNetWt As Double = 0
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Dim dt As New DataTable
        ''CostCentre Checking..
        strSql = " SELECT 1 FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE' AND CTLTEXT = 'Y'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            cmbCostCentre.Items.Clear()
            cmbCostCentre.Enabled = False
        End If

        If cmbCostCentre.Enabled = True Then
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            objGPack.FillCombo(strSql, cmbCostCentre)
        End If

        ''LOADING ITEMTYPE
        strSql = " SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE ORDER BY NAME"
        objGPack.FillCombo(strSql, cmbItemType)

        ''LOADING COUNTER
        If UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'ITEMCOUNTER'")) = "Y" Then
            strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER ORDER BY DISPLAYORDER,ITEMCTRNAME "
            objGPack.FillCombo(strSql, cmbCounter)
            cmbCounter.Enabled = True
        Else
            cmbCounter.Enabled = False
        End If

        ''LOADING DESIGNER
        strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER ORDER BY DESIGNERNAME"
        objGPack.FillCombo(strSql, cmbDesigner)

        ''TAGNO GEN
        strSql = " SELECT CTLTEXT AS TAGNOGEN FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'TAGNOGEN'"
        TagNoGen = objGPack.GetSqlValue(strSql)
        funcNew()
        LoadTransaction()
        LoadHMDetails()
        tabMain.SelectedTab = tabEntry
        optQC.Checked = True
        optVQC.Checked = True
    End Sub
    Sub LoadTransaction()
        cmbTransaction.Items.Clear()
        cmbTransaction.Items.Add("Issue")
        cmbTransaction.Items.Add("Receipt")
        cmbTransaction.Items.Add("Return")
        cmbTransaction.SelectedIndex = 0

        cmbViewTran.Items.Clear()
        cmbViewTran.Items.Add("Issue")
        cmbViewTran.Items.Add("Receipt")
        cmbViewTran.Items.Add("Return")
        cmbViewTran.SelectedIndex = 0
    End Sub
    Sub LoadHMDetails()
        Dim dt As DataTable

        strSql = "select EMPID,EMPNAME from " & cnAdminDb & "..EMPMASTER WHERE ACTIVE = 'Y' ORDER BY EMPNAME"
        dt = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        cmbEmployee.DataSource = Nothing
        cmbEmployee.DataSource = dt
        cmbEmployee.DisplayMember = "EMPNAME"
        cmbEmployee.ValueMember = "EMPID"
        cmbEmployee.SelectedIndex = 0

        strSql = "select HALLMARKID,HALLMARKNAME from " & cnAdminDb & "..HALLMARK WHERE ACTIVE = 'Y' ORDER BY HALLMARKNAME"
        dt = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        cmbHM.DataSource = Nothing
        cmbHM.DataSource = dt
        cmbHM.DisplayMember = "HALLMARKNAME"
        cmbHM.ValueMember = "HALLMARKID"
        cmbHM.SelectedIndex = 0

        If optHM.Checked Then
            If cmbTransaction.Text = "Issue" Then
                cmbHM.Enabled = True
                cmbEmployee.Enabled = True
            Else
                cmbHM.Enabled = False
            End If
        Else
            cmbHM.Enabled = False
        End If
    End Sub
    Function funcExit() As Integer
        Me.Close()
    End Function
    Function funcGenPurItemTagStrSql(ByVal TagSno As String, ByVal ItemId As Integer, ByVal TagNo As String, ByVal PurRate As Decimal, ByVal BulkPcs As Integer, ByVal CostId As String) As String
        ''ITEM PUR DETAIL
        strSql = vbCrLf + " INSERT INTO " & cnAdminDb & "..PURITEMTAG"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " TAGSNO"
        strSql += vbCrLf + " ,ITEMID"
        strSql += vbCrLf + " ,TAGNO"
        strSql += vbCrLf + " ,PURLESSWT"
        strSql += vbCrLf + " ,PURNETWT"
        strSql += vbCrLf + " ,PURRATE"
        strSql += vbCrLf + " ,PURGRSNET"
        strSql += vbCrLf + " ,PURWASTAGE"
        strSql += vbCrLf + " ,PURTOUCH"
        strSql += vbCrLf + " ,PURMC"
        strSql += vbCrLf + " ,PURVALUE"
        strSql += vbCrLf + " ,PURTAX"
        strSql += vbCrLf + " ,RECDATE"
        strSql += vbCrLf + " ,COMPANYID,COSTID"
        strSql += vbCrLf + " )"
        strSql += vbCrLf + " VALUES"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " '" & TagSno & "'" 'TAGSNO
        strSql += vbCrLf + " ," & ItemId & "" 'ITEMID
        strSql += vbCrLf + " ,'" & TagNo & "'" 'TAGNO
        strSql += vbCrLf + " ,0" ' PURLESSWT
        strSql += vbCrLf + " ,0" ' PURNETWT"
        strSql += vbCrLf + " ," & purFineRate & "" 'PURRATE
        strSql += vbCrLf + " ,''" 'PURGRSNET"
        strSql += vbCrLf + " ,0" 'PURWASTAGE"
        strSql += vbCrLf + " ,0" 'PURTOUCH"
        strSql += vbCrLf + " ,0" 'PURMC"
        strSql += vbCrLf + " ," & BulkPcs * pieceRate & "" ' PURVALUE"
        strSql += vbCrLf + " ,0"
        strSql += vbCrLf + " ,'" & GetEntryDate(dtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,'" & GetStockCompId() & "'"
        strSql += vbCrLf + " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, CostId) & "'"
        strSql += vbCrLf + " )"
        Return strSql
    End Function
    Function funcGenItemTagStrSql(ByVal TagSno As String, ByVal COSTID As String _
    , ByVal itemId As Integer, ByVal subItemId As Integer, ByVal SizeId As String, ByVal itemCtrId As String _
    , ByVal designerId As String, ByVal tagNo As String, ByVal bulkPcs As Integer _
    , ByVal tagVAl As Integer, ByVal saleMode As String, ByVal itemTypeId As String _
    , ByVal tranInvNo As String, ByVal supBillNo As String) As String

        Dim pctFile As String = ""
        Dim StyleCode As String = ""
        If subItemId <> 0 Then
            strSql = " SELECT PCTFILE,STYLECODE FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = " & itemId & " AND SUBITEMID = " & subItemId & ""
        Else
            strSql = " SELECT PCTFILE,STYLECODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & itemId & ""
        End If
        pctFile = objGPack.GetSqlValue(strSql, "PCTFILE", , tran)
        StyleCode = objGPack.GetSqlValue(strSql, "STYLECODE", , tran)
        ''Inserting itemTag
        strSql = " INSERT INTO " & cnAdminDb & "..ITEMTAG"
        strSql += " ("
        strSql += " SNO,RECDATE,COSTID,ITEMID,ORDREPNO,ORSNO,"
        strSql += " ORDSALMANCODE,SUBITEMID,SIZEID,ITEMCTRID,TABLECODE,DESIGNERID,"
        strSql += " TAGNO,PCS,GRSWT,"
        strSql += " LESSWT,NETWT,RATE,FINERATE,MAXWASTPER,MAXMCGRM,"
        strSql += " MAXWAST,MAXMC,MINWASTPER,MINMCGRM,MINWAST,MINMC,"
        strSql += " TAGKEY,TAGVAL,LOTSNO,COMPANYID,SALVALUE,PURITY,NARRATION,DESCRIP,"
        strSql += " REASON,ENTRYMODE,GRSNET,"
        strSql += " ISSDATE,ISSREFNO,ISSPCS,ISSWT,FROMFLAG,TOFLAG,APPROVAL,SALEMODE,"
        'strSql += " PURRATE,PURGRSNET,PURWASTAGE,PURTOUCH,PURMC,PURSTNVALUE,PURVALUE,"
        strSql += " BATCHNO,MARK,"
        strSql += " VATEXM,PCTFILE,OLDTAGNO,ITEMTYPEID,ACTUALRECDATE,WEIGHTUNIT,"
        strSql += " TRANSFERWT,CHKDATE,CHKTRAY,CARRYFLAG,BRANDID,PRNFLAG,"
        strSql += " MCDISCPER,WASTDISCPER,RESDATE,TRANINVNO,SUPBILLNO,WORKDAYS,"
        strSql += " USERID,UPDATED,UPTIME,SYSTEMID,APPVER,TCOSTID,STYLENO)VALUES("
        strSql += " '" & TagSno & "'" 'SNO
        strSql += " ,'" & GetEntryDate(dtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'RECDATE
        strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
        strSql += " ," & itemId & "" 'ITEMID
        strSql += " ,''" 'ORDREPNO
        strSql += " ,''" 'ORSNO
        strSql += " ,''" 'ORDSALMANCODE
        strSql += " ," & subItemId & "" 'SUBITEMID
        strSql += " ,'" & SizeId & "'" 'SIZEID
        strSql += " ,'" & itemCtrId & "'" 'ITEMCTRID
        strSql += " ,''" 'TABLECODE
        strSql += " ,'" & Val(designerId) & "'" 'DESIGNERID
        strSql += " ,'" & tagNo & "'" 'TAGNO
        strSql += " ," & bulkPcs & "" 'PCS
        strSql += " ,0" 'GRSWT
        strSql += " ,0" 'LESSWT
        strSql += " ,0" 'NETWT
        strSql += " ," & pieceRate & "" 'RATE
        strSql += " ,0" 'FINERATE
        strSql += " ,0" 'MAXWASTPER
        strSql += " ,0" 'MAXMCGRM
        strSql += " ,0" 'MAXWAST
        strSql += " ,0" 'MAXMC
        strSql += " ,0" 'MINWASTPER
        strSql += " ,0" 'MINMCGRM
        strSql += " ,0" 'MINWAST
        strSql += " ,0" 'MINMC
        strSql += " ,'" & itemId & "" & tagNo & "'" 'TAGKEY
        strSql += " ," & tagVAl & "" 'TAGVAL
        strSql += " ,'" & TranNo & "'" 'LOSNO
        strSql += " ,'" & GetStockCompId() & "'" 'COMPANYID
        strSql += " ," & bulkPcs * pieceRate & "" 'SALVALUE
        strSql += " ,0" 'PURITY
        strSql += " ,'" & narration & "'" 'NARRATION
        strSql += " ,''" 'DESCRIP
        strSql += " ,''" 'REASON
        strSql += " ,'A'" 'ENTRYMODE
        strSql += " ,''" 'GRSNET
        strSql += " ,null" 'ISSDATE
        strSql += " ,0" 'ISSREFNO
        strSql += " ,0" 'ISSPCS
        strSql += " ,0" 'ISSWT
        strSql += " ,''" 'FROMFLAG
        strSql += " ,''" 'TOFLAG
        strSql += " ,''" 'APPROVAL
        strSql += " ,'" & saleMode & "'" 'SALEMODE
        'strSql += " ," & purFineRate & "" 'PURRATE
        'strSql += " ,''" 'PURGRSNET
        'strSql += " ,0" 'PURWASTAGE
        'strSql += " ," & purTouch & "" 'PURTOUCH
        'strSql += " ,0" 'PURMC
        'strSql += " ,0" 'PURSTNVALUE
        'strSql += " ," & bulkPcs * purFineRate & "" 'PURVALUE
        strSql += " ,''" 'BATCHNO
        strSql += " ,0" 'MARK
        strSql += " ,''" 'VATEXM
        strSql += " ,'" & pctFile & "'" 'PCTFILE
        strSql += " ,''" 'OLDTAGNO
        strSql += " ," & Val(itemTypeId) & "" 'ITEMTYPEID
        strSql += " ,'" & GetEntryDate(dtpRecieptDate.Value, tran).ToString("yyyy-MM-dd") & "'" 'ACTUALRECDATE
        strSql += " ,''" 'WEIGHTUNIT
        strSql += " ,0" 'TRANSFERWT
        strSql += " ,''" 'CHKDATE
        strSql += " ,''" 'CHKTRAY
        strSql += " ,''" 'CARRYFLAG
        strSql += " ,''" 'BRANDID
        strSql += " ,''" 'PRNFLAG
        strSql += " ,0" 'MCDISCPER
        strSql += " ,0" 'WASTDISCPER
        strSql += " ,''" 'RESDATE
        strSql += " ,'" & tranInvNo & "'" 'TRANINVNO
        strSql += " ,'" & supBillNo & "'" 'SUPBILLNO
        strSql += " ,''" 'WORKDAYS
        strSql += " ," & userId & "" 'USERID
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
        strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
        strSql += " ,'" & systemId & "'"
        strSql += " ,'" & VERSION & "'" 'APPVER
        strSql += " ,'" & COSTID & "'" 'TCOSTID
        strSql += " ,'" & StyleCode & "'" 'STYLECODE
        strSql += " )"
        Return strSql
    End Function
    Function funcNew() As Integer
        objGPack.TextClear(Me)
        TranNo = Nothing
        EntryOrder = 0
        multipleTagFlag = Nothing
        pieceRate = 0
        pcs = 0

        lblPCompled.Text = ""
        lblPLot.Text = ""
        lblPBalance.Text = ""
        lblLastTagNo.Text = ""
        dtpRecieptDate.Value = GetEntryDate(GetServerDate)
        cmbCostCentre.Text = ""
        cmbItemType.Text = ""
        cmbCounter.Text = ""
        cmbDesigner.Text = ""
        cmbItemSize.Text = ""
        objTag = New TagGeneration
        If cmbItemType.Items.Count > 0 Then cmbItemType.Enabled = True Else cmbItemType.Enabled = False
        If Not cmbItemSize.Items.Count > 0 Then
            cmbItemSize.Enabled = False
        End If
        txtTranNo.Enabled = True
        txtPieces.Text = ""
        txtGrsWt.Text = ""
        txtNetWt.Text = ""
        dtpFromDate.Value = Now.Date.ToString("dd-MMM-yyyy")
        dtpToDate.Value = Now.Date.ToString("dd-MMM-yyyy")
        dgView.DataSource = Nothing
        LoadTransaction()
        LoadHMDetails()
        cmbTransaction.Focus()
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If txtGrsWt.Text = "" Then
            MsgBox("Cannot be saved.", MsgBoxStyle.Information)
            Exit Function
        End If
        Dim _tranNo As String = ""
        Dim _msgStr As String = ""
        Dim _tranDate As Date = Nothing
        Dim _batchNo As String = ""
        Dim _type As String = ""
        If cmbTransaction.Text = "Issue" Then
            _tranNo = SaveIssue(TranNo, _tranDate, _batchNo)
            If optHM.Checked Then
                _msgStr = $"Hall Marking Issue, TranNo : {_tranNo}"
                _type = "HI"
            Else
                _msgStr = $"Quality Check Issue, TranNo : {_tranNo}"
                _type = "QI"
            End If
        ElseIf cmbTransaction.Text = "Receipt" Then
            If txtPieces.Text.Trim = "" Or txtGrsWt.Text.Trim = "" Or txtNetWt.Text.Trim = "" Then Throw New Exception("Invalid input.")
            If (Integer.Parse(txtPieces.Text) > actPieces) Or (Double.Parse(txtGrsWt.Text) > actGrsWt) Or (Double.Parse(txtNetWt.Text) > actNetWt) Then Throw New Exception("Invalid input.")
            _tranNo = SaveReceiptAndReturn(TranNo, "", _tranDate, _batchNo)
            If optHM.Checked Then
                _msgStr = $"Hall Marking Receipt, TranNo : {_tranNo}"
                _type = "HR"
            Else
                _msgStr = $"Quality Check Receipt, TranNo : {_tranNo}"
                _type = "QR"
            End If
        ElseIf cmbTransaction.Text = "Return" Then
            If txtPieces.Text.Trim = "" Or txtGrsWt.Text.Trim = "" Or txtNetWt.Text.Trim = "" Then Throw New Exception("Invalid input.")
            If (Integer.Parse(txtPieces.Text) > actPieces) Or (Double.Parse(txtGrsWt.Text) > actGrsWt) Or (Double.Parse(txtNetWt.Text) > actNetWt) Then Throw New Exception("Invalid input.")
            _tranNo = SaveReceiptAndReturn(TranNo, "R", _tranDate, _batchNo)
            If optHM.Checked Then
                _msgStr = $"Hall Marking Return, TranNo : {_tranNo}"
                _type = "HR"
            Else
                _msgStr = $"Quality Check Return, TranNo : {_tranNo}"
                _type = "QR"
            End If
        End If
        MsgBox($"Transaction saved successfully. {vbCrLf} {_msgStr}", MsgBoxStyle.Information)

        If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
            Dim write As IO.StreamWriter
            write = IO.File.CreateText(Application.StartupPath & "\BillPrint.mem")
            write.WriteLine(LSet("TYPE", 15) & ":" & _type)
            write.WriteLine(LSet("BATCHNO", 15) & ":" & _batchNo)
            write.WriteLine(LSet("TRANDATE", 15) & ":" & _tranDate)
            write.WriteLine(LSet("DUPLICATE", 15) & ":N")
            write.Flush()
            write.Close()

            If EXE_WITH_PARAM = False Then
                System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
            Else
                System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe",
                LSet("TYPE", 15) & ":" & _type & ";" &
                LSet("BATCHNO", 15) & ":" & _batchNo & ";" &
                LSet("TRANDATE", 15) & ":" & _tranDate & ";" &
                LSet("DUPLICATE", 15) & ":N")
            End If

        Else
            MsgBox("Billprint exe not found", MsgBoxStyle.Information)
        End If

        funcNew()
        dtpRecieptDate.Value = Now.Date : dtpRecieptDate.Enabled = False
        cmbTransaction.SelectedIndex = 0
        txtTranNo.Text = ""
        txtPieces.Enabled = True
        txtGrsWt.Enabled = True
        txtNetWt.Enabled = True
        'If Not CheckDate(dtpRecieptDate.Value) Then Exit Function
        'If CheckEntryDate(dtpRecieptDate.Value) Then Exit Function
        'If Not Val(lblPBalance.Text) > 0 Then
        '    txtTranNo_Num_Man.Select()
        '    Exit Function
        'End If
        'funcAdd()
    End Function
    Function funcFindTagVal(ByVal TagNo As String) As String
        Dim tagVal As String
        ''Find TagVal
        If IsNumeric(TagNo) = True Then
            tagVal = Val(TagNo)
        Else
            Dim index As Integer = 0
            For Each c As Char In TagNo
                If Char.IsLetter(c) = True Then
                    index += 1
                Else
                    Exit For
                End If
            Next
            Dim fNo As String
            Dim sNo As String
            fNo = TagNo.Substring(0, index)
            sNo = TagNo.Substring(index, TagNo.Length - index)
            tagVal = (AscW(fNo) * 1000) + Val(sNo)
        End If
        Return tagVal
    End Function
    Private Function GetTagNo() As String
        Dim tagNo As String = Nothing
        Dim str As String = Nothing
        If GetAdmindbSoftValue("TAGNOFROM", , tran) = "I" Then ''FROM ITEMMAST OR UNIQUE
            If GetAdmindbSoftValue("TAGNOGEN", , tran) = "I" Then ''FROM ITEM
                str = " SELECT CURRENTTAGNO LASTTAGNO FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = "
                str += " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItemName__Man.Text & "')"
            Else ''FROM LOT
                str = " SELECT CURTAGNO AS LASTTAGNO FROM " & cnAdminDb & "..ITEMLOT WHERE ITEMID = "
                str += " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItemName__Man.Text & "')"
                str += " AND SNO = '" & TranNo & "'"
            End If
        Else 'UNIQUE
            str = " SELECT CTLTEXT AS LASTTAGNO FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'LASTTAGNO'"
        End If
        tagNo = objGPack.GetSqlValue(str, , "1", tran)
        Return GenTagNo(tagNo)
    End Function

    Private Sub UpdateTagNo(ByVal tagNo As String, ByVal COSTID As String)
        Dim tagPrefix As String = GetAdmindbSoftValue("TAGPREFIX", , tran)
        Dim updTagNo As String = Nothing
        If tagPrefix.Length > 0 Then
            updTagNo = tagNo.Replace(tagPrefix, "")
        Else
            updTagNo = tagNo
        End If

        If GetAdmindbSoftValue("TAGNOFROM", , tran) = "I" Then ''FROM ITEMMAST OR UNIQUE
            If GetAdmindbSoftValue("TAGNOGEN", , tran) = "I" Then ''FROM ITEM
                strSql = " UPDATE " & cnAdminDb & "..ITEMMAST SET CURRENTTAGNO = '" & updTagNo & "' WHERE ITEMNAME = '" & txtItemName__Man.Text & "'"
            Else ''FROM LOT
                strSql = " UPDATE " & cnAdminDb & "..ITEMLOT SET CURTAGNO = '" & updTagNo & "'"
            End If
        Else 'UNIQUE
            strSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT  = '" & updTagNo & "' WHERE CTLID = 'LASTTAGNO'"
        End If
        ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID)
    End Sub

    Private Function GenTagNo(ByRef TagNo As String) As String
        Dim str As String = Nothing
        If IsNumeric(TagNo) Then
            TagNo = Val(TagNo) + 1
        Else
            Dim fPart As String = Nothing
            Dim sPart As String = Nothing
            For Each c As Char In TagNo
                If IsNumeric(c) Then
                    sPart += c
                Else
                    fPart += c
                End If
            Next
            TagNo = fPart + (Val(sPart) + 1).ToString
        End If
        Dim tagPrefix As String = GetAdmindbSoftValue("TAGPREFIX", , tran)
        Return tagPrefix + TagNo
    End Function

    '    Function GetTagSno() As String
    'GETNTAGSNO:
    '        Dim tSno As Integer = 0
    '        strSql = " SELECT CTLTEXT AS TAGSNO FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'TAGSNO'"
    '        tSno = Val(objGPack.GetSqlValue(strSql, "TAGSNO", , tran))
    '        ''UPDATING 
    '        ''TAGNO INTO SOFTCONTROL
    '        strSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT = '" & tSno + 1 & "' "
    '        strSql += " WHERE CTLID = 'TAGSNO' AND CONVERT(INT,CTLTEXT) = " & tSno & ""
    '        cmd = New OleDbCommand(strSql, cn, tran)
    '        If cmd.ExecuteNonQuery() = 0 Then
    '            GoTo GETNTAGSNO
    '        End If
    '        strSql = " SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE SNO = '" & cnCostId & (tSno + 1).ToString & "'"
    '        If objGPack.GetSqlValue(strSql, , "-1", tran) <> "-1" Then
    '            GoTo GETNTAGSNO
    '        End If
    '        Return cnCostId & (tSno + 1).ToString
    '    End Function


    Function funcAdd() As Integer
        Dim TagSno As String = Nothing
        Dim TagNo As String = Nothing

        Dim COSTID As String = cnCostId
        Dim sizeId As Integer = Nothing
        Dim itemCtrId As Integer = Nothing
        Dim designerId As Integer = Nothing
        Dim itemTypeId As Integer = Nothing

        Dim itemId As Integer = Nothing
        Dim subitemId As Integer = Nothing

        Dim tagVal As Integer = 0
        Dim saleMode As String = Nothing

        Dim tranInvNo As String = Nothing
        Dim supBillno As String = Nothing

        tran = Nothing
        ''Find ItemId
        strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & txtItemName__Man.Text & "'"
        itemId = Val(objGPack.GetSqlValue(strSql, , , tran))
        ''FIND SUBITEMID
        subitemId = Val(objGPack.GetSqlValue(" SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_MAN.Text & "' AND ITEMID = " & itemId & "", , , tran))
        If subitemId <> 0 Then
            strSql = " SELECT PIECERATE,PIECERATE_PUR FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = " & subitemId & " AND ITEMID = " & itemId & ""
        Else
            strSql = " SELECT PIECERATE,PIECERATE_PUR FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & itemId & ""
        End If
        Dim dtGetRate As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGetRate)
        If pieceRate = 0 Then
            If dtGetRate.Rows.Count > 0 Then
                pieceRate = Val(dtGetRate.Rows(0).Item("PIECERATE").ToString)
                purFineRate = Val(dtGetRate.Rows(0).Item("PIECERATE_PUR").ToString)
            End If
        End If
        If purFineRate = 0 Then
            If dtGetRate.Rows.Count > 0 Then
                purFineRate = Val(dtGetRate.Rows(0).Item("PIECERATE_PUR").ToString)
            End If
        End If
        If pieceRate = 0 Then
            MsgBox("PieceRate does not assign for selected Item,SubItem", MsgBoxStyle.Information)
            Exit Function
        End If

        Try
            tran = Nothing
            tran = cn.BeginTransaction()
            ''Find TagSno
            TagSno = GetNewSno(TranSnoType.ITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") ' GetTagSno()
            If cmbCostCentre.Enabled = True Then
                ''Find COSTID
                strSql = " SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME  = '" & cmbCostCentre.Text & "'"
                COSTID = objGPack.GetSqlValue(strSql, , , tran)
            End If


            If cmbItemSize.Enabled = True Then
                ''Find ItemSize
                strSql = " SELECT SIZEID FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZENAME = '" & cmbItemSize.Text & "'"
                sizeId = Val(objGPack.GetSqlValue(strSql, , , tran))
            End If

            If cmbCounter.Enabled = True Then
                ''Find ItemCounter
                strSql = " SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbCounter.Text & "'"
                itemCtrId = Val(objGPack.GetSqlValue(strSql, , , tran))
            End If

            ''Find DesignerId
            strSql = " SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesigner.Text & "'"
            designerId = Val(objGPack.GetSqlValue(strSql, , , tran))
            TagNo = objTag.GetTagNo(GetEntryDate(dtpRecieptDate.Value, tran).ToString("yyyy-MM-dd"), txtItemName__Man.Text, TranNo, tran)
            tagVal = objTag.GetTagVal(TagNo)
            'TagNo = GetTagNo()
            'tagVal = GetTagVal(TagNo)
            ''Find SaleMode
            strSql = " SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID='" & itemId & "'"
            saleMode = objGPack.GetSqlValue(strSql, , , tran)

            ''Find itemTypeId
            strSql = " SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType.Text & "'"
            itemTypeId = Val(objGPack.GetSqlValue(strSql, , , tran))

            ''Find TranInvNo and SupBillNo
            strSql = " SELECT TRANINVNO,BILLNO,SUBITEMID FROM " & cnAdminDb & "..ITEMLOT WHERE SNO = '" & TranNo & "'" '' LOTNO ='" & txtTranNo_Num_Man_Num_Man.Text & "' AND ENTRYORDER = '" & EntryOrder & "'"
            tranInvNo = objGPack.GetSqlValue(strSql, "TRANINVNO", , tran)
            supBillno = objGPack.GetSqlValue(strSql, "BILLNO", , tran)


            Dim dtTagPrint As New DataTable
            dtTagPrint.Columns.Add("ITEMID", GetType(Integer))
            dtTagPrint.Columns.Add("TAGNO", GetType(String))

            Dim rowTag As DataRow = Nothing
            Dim bulkPcs As Integer = pcs
            ''Inserting itemTag
            If multipleTagFlag = "N" Then
                rowTag = dtTagPrint.NewRow
                rowTag!ITEMID = itemId
                rowTag!TAGNO = TagNo
                dtTagPrint.Rows.Add(rowTag)
                strSql = funcGenItemTagStrSql(TagSno, COSTID, itemId, subitemId, sizeId, itemCtrId, designerId, TagNo,
                pcs, tagVal, saleMode, itemTypeId, tranInvNo, supBillno)
                cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                If purFineRate <> 0 Then
                    strSql = funcGenPurItemTagStrSql(TagSno, itemId, TagNo, purFineRate, pcs, COSTID)
                    cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                End If
                UpdateTagNo(TagNo, COSTID)
            Else
                For cnt As Integer = 1 To pcs
                    rowTag = dtTagPrint.NewRow
                    rowTag!ITEMID = itemId
                    rowTag!TAGNO = TagNo
                    dtTagPrint.Rows.Add(rowTag)
                    bulkPcs = cnt
                    strSql = funcGenItemTagStrSql(TagSno, COSTID, itemId, subitemId, sizeId, itemCtrId, designerId, TagNo,
                    1, tagVal, saleMode, itemTypeId, tranInvNo, supBillno)
                    cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                    'ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID)
                    If purFineRate <> 0 Then
                        strSql = funcGenPurItemTagStrSql(TagSno, itemId, TagNo, purFineRate, 1, COSTID)
                        cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                    End If
                    UpdateTagNo(TagNo, COSTID)
                    If cnt <> pcs Then
                        TagSno = GetNewSno(TranSnoType.ITEMTAGCODE, tran, "GET_ADMINSNO_TRAN") ' GetTagSno()
                        TagNo = objTag.GetTagNo(GetEntryDate(dtpRecieptDate.Value, tran).ToString("yyyy-MM-dd"), txtItemName__Man.Text, TranNo, tran)
                        tagVal = objTag.GetTagVal(TagNo)
                        'TagNo = GetTagNo()
                        'tagVal = GetTagVal(TagNo)
                    End If
                Next
            End If
            ' ''Updating TagNo
            'If TagNoGen = "I" Then
            '    If TagNoFrom = "I" Then
            '        strSql = " UPDATE " & cnAdminDb & "..ITEMMAST SET CURRENTTAGNO = '" & TagNo & "' WHERE ITEMNAME = '" & txtItemName__Man.Text & "'"
            '    Else 'SOFTCONTROL
            '        strSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT  = '" & TagNo & "' WHERE CTLID = 'LASTTAGNO'"
            '    End If
            'Else ''L
            '    strSql = " UPDATE " & cnAdminDb & "..ITEMLOT SET CURTAGNO = '" & TagNo & "'"
            'End If
            'cmd = New OleDbCommand(strSql, cn, tran)
            'cmd.ExecuteNonQuery()

            ' ''Updating 
            ' ''TagNo into SoftControl
            'strSql = " UPDATE " & cnAdminDb & "..SOFTCONTROL SET CTLTEXT = '" & TagSno & "' WHERE CTLID = 'TAGSNO'"
            'cmd = New OleDbCommand(strSql, cn, tran)
            'cmd.ExecuteNonQuery()

            'CPIECES AND CWT
            strSql = " UPDATE " & cnAdminDb & "..ITEMLOT SET CPCS = CPCS + " & pcs & ""
            strSql += " WHERE LOTNO = '" & txtTranNo.Text & "'"
            strSql += " AND ENTRYORDER = '" & EntryOrder & "'"
            ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID)

            tran.Commit()
            tran = Nothing
            MsgBox(E0008, MsgBoxStyle.Exclamation)
            ''Last Tag and Wt
            Dim lotPcs As Integer = lblPLot.Text
            Dim cPcs As Integer = pcs
            funcNew()
            ''Lot Pcs
            lblPLot.Text = lotPcs
            lblPCompled.Text = cPcs
            lblPBalance.Text = IIf(Val(lblPLot.Text) - Val(lblPCompled.Text) <> 0,
            Val(lblPLot.Text) - Val(lblPCompled.Text), "")
            lblLastTagNo.Text = TagNo

            Dim oldItem As Integer = Nothing
            Dim write As IO.StreamWriter
            write = IO.File.CreateText(Application.StartupPath & "\Barcodeprint.mem")
            For Each ro As DataRow In dtTagPrint.Rows
                If oldItem <> Val(ro!itemid.ToString) Then
                    write.WriteLine(LSet("PROC", 7) & ":" & ro!ITEMID.ToString)
                    oldItem = Val(ro!itemid.ToString)
                End If
                write.WriteLine(LSet("TAGNO", 7) & ":" & ro!TAGNO.ToString)
            Next
            write.Flush()
            write.Close()
            If IO.File.Exists(Application.StartupPath & "\PrjBarcodePrint.exe") Then
                System.Diagnostics.Process.Start(Application.StartupPath & "\PrjBarcodePrint.exe")
            Else
                MsgBox("Barcode print exe not found", MsgBoxStyle.Information)
            End If
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        Finally
            If Not tran Is Nothing Then tran.Dispose()
        End Try
    End Function
    Private Function GetTagVal(ByVal tagNo As String) As String
        Dim TagVal As Integer = Nothing
        ''Find TagVal
        If IsNumeric(tagNo) = True Then
            TagVal = Val(tagNo)
        Else
            Dim index As Integer = 0
            For Each c As Char In tagNo
                If Char.IsLetter(c) = True Then
                    index += 1
                Else
                    Exit For
                End If
            Next
            Dim fNo As String
            Dim sNo As String
            fNo = tagNo.Substring(0, index)
            sNo = tagNo.Substring(index, tagNo.Length - index)
            TagVal = (AscW(fNo) * 1000) + Val(sNo)
        End If
        Return TagVal.ToString
    End Function

    Function funcGetTagNo() As String
        ''Find LastTagNo
        If TagNoGen = "I" Then
            strSql = " SELECT CTLTEXT AS TAGNOFROM FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'TAGNOFROM'"
            TagNoFrom = objGPack.GetSqlValue(strSql, , , tran)
            If TagNoFrom = "I" Then
                strSql = " SELECT CURRENTTAGNO FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = "
                strSql += " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItemName__Man.Text & "')"
                LastTagNo = Val(objGPack.GetSqlValue(strSql, , , tran)).ToString
            Else
                strSql = " Select ctlText as LastTagNo from " & cnAdminDb & "..SoftControl where ctlId = 'LastTagNo'"
                LastTagNo = objGPack.GetSqlValue(strSql, , , tran).ToString
            End If
        Else ''L--Lot Item Entry
            strSql = " SELECT CURTAGNO FROM " & cnAdminDb & "..ITEMLOT WHERE ITEMID = "
            strSql += " (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItemName__Man.Text & "')"
            strSql += " AND LOTNO = '" & txtTranNo.Text & "' AND ENTRYORDER = '" & EntryOrder & "'"
            LastTagNo = objGPack.GetSqlValue(strSql, , , tran)
        End If
        Return funcGenTagNo(LastTagNo)
    End Function
    Function funcGenTagNo(ByVal lstTagNo As String) As String
        'If lstTagNo = "0" Then
        '    Return "0"
        'End If
        If IsNumeric(lstTagNo) = True Then
            lstTagNo = Val(lstTagNo) + 1
        Else
            Dim index As Integer = 0
            For Each c As Char In lstTagNo
                If Char.IsLetter(c) = True Then
                    index += 1
                Else
                    Exit For
                End If
            Next
            Dim fNo As String
            Dim sNo As String
            fNo = lstTagNo.Substring(0, index)
            sNo = lstTagNo.Substring(index, lstTagNo.Length - index)
            sNo = Val(sNo) + 1
            lstTagNo = fNo + sNo
        End If
        Return lstTagNo
    End Function
    Private Sub funcGetIssueDetails_old()
        If optHM.Checked Then
            'strSql = " SELECT I.TRANNO,(I.PCS-ISNULL(R.PCS,0))PCS,(I.GRSWT-ISNULL(R.GRSWT,0))GRSWT,(I.NETWT-ISNULL(R.NETWT,0))NETWT,I.TRANDATE, “
            'strSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)ITEMNAME, “
            'strSql += vbCrLf + " (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID)SUBITEMNAME, “
            'strSql += vbCrLf + " ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = I.COSTID),'')COSTNAME, “
            'strSql += vbCrLf + " ISNULL((SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACCODE = I.ACCODE),'')DESIGNERNAME, I.ITEMID,I.SUBITEMID FROM " & cnStockDb & "..QCRECEIPT I “
            'strSql += vbCrLf + " LEFT JOIN (SELECT SNO, ISNULL(SUM(PCS),0) PCS, ISNULL(SUM(GRSWT),0) GRSWT, “
            'strSql += vbCrLf + " ISNULL(SUM(NETWT),0) NETWT FROM " & cnStockDb & "..QCISSUE WHERE TRANTYPE = 'H' AND ISNULL(CANCEL,'')='' GROUP BY SNO) AS R ON R.SNO = I.BATCHNO WHERE 1=1 “
            'strSql += vbCrLf + " AND I.GRSWT > ISNULL(R.GRSWT,0) AND I.TRANTYPE = 'Q' AND ISNULL(R.SNO,'')='' AND ISNULL(I.RETURNSTATUS,'')=''“
            'If TranNo <> "" Then strSql += vbCrLf + " AND I.TRANNO = " & TranNo
            strSql = " ;WITH CTE1 AS("
            strSql += vbCrLf + " SELECT I.TRANNO,I.SNO,"
            strSql += vbCrLf + " (I.PCS-ISNULL(R.PCS,0))PCS,"
            strSql += vbCrLf + " (I.GRSWT-ISNULL(R.GRSWT,0))GRSWT,"
            strSql += vbCrLf + " (I.NETWT-ISNULL(R.NETWT,0))NETWT,I.TRANDATE, "
            strSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)ITEMNAME, "
            strSql += vbCrLf + " (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID)SUBITEMNAME, "
            strSql += vbCrLf + " ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = I.COSTID),'')COSTNAME, "
            strSql += vbCrLf + " ISNULL((SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACCODE = I.ACCODE),'')DESIGNERNAME, I.ITEMID,I.SUBITEMID FROM " & cnStockDb & "..QCRECEIPT I "
            strSql += vbCrLf + " LEFT JOIN (SELECT SNO, ISNULL(SUM(PCS),0) PCS, ISNULL(SUM(GRSWT),0) GRSWT, "
            strSql += vbCrLf + " ISNULL(SUM(NETWT),0) NETWT FROM " & cnStockDb & "..QCISSUE WHERE TRANTYPE = 'H' AND ISNULL(CANCEL,'')='' "
            strSql += vbCrLf + " GROUP BY SNO) AS R ON R.SNO = I.BATCHNO WHERE 1=1 "
            strSql += vbCrLf + " AND I.GRSWT > ISNULL(R.GRSWT,0) AND I.TRANTYPE = 'Q' AND ISNULL(I.RETURNSTATUS,'')='' AND ISNULL(I.CANCEL,'')=''"
            strSql += vbCrLf + " ),"
            strSql += vbCrLf + " CTE2 AS("
            strSql += vbCrLf + " SELECT I.SNO,"
            strSql += vbCrLf + " ISNULL(I.PCS,0)PCS,"
            strSql += vbCrLf + " ISNULL(I.GRSWT,0)GRSWT,"
            strSql += vbCrLf + " ISNULL(I.NETWT,0)NETWT "
            strSql += vbCrLf + " FROM " & cnStockDb & "..QCRECEIPT I "
            strSql += vbCrLf + " WHERE 1=1 "
            strSql += vbCrLf + " AND I.TRANTYPE = 'Q' AND ISNULL(I.RETURNSTATUS,'')='R' AND ISNULL(I.CANCEL,'')=''"
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " SELECT CTE1.TRANNO,CTE1.PCS-ISNULL(CTE2.PCS,0) PCS,CTE1.GRSWT-ISNULL(CTE2.GRSWT,0) GRSWT,CTE1.NETWT-ISNULL(CTE2.NETWT,0) NETWT,CTE1.TRANDATE,CTE1.ITEMNAME,CTE1.SUBITEMNAME,CTE1.COSTNAME,CTE1.DESIGNERNAME,"
            strSql += vbCrLf + " CTE1.ITEMID,CTE1.SUBITEMID FROM CTE1 "
            strSql += vbCrLf + " LEFT JOIN CTE2 ON CTE1.SNO = CTE2.SNO WHERE 1=1"
            strSql += vbCrLf + " AND (CTE1.GRSWT-ISNULL(CTE2.GRSWT,0)) > 0"
            If TranNo <> "" Then strSql += vbCrLf + " AND CTE1.TRANNO = " & TranNo
        Else
            strSql = " SELECT IT.PCS,IT.GRSWT,IT.NETWT,IT.TRANDATE,"
            strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = IT.ITEMID)ITEMNAME,"
            strSql += " (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = IT.ITEMID AND SUBITEMID = IT.SUBITEMID)SUBITEMNAME,"
            strSql += " ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = IT.COSTID),'')COSTNAME,"
            strSql += " ISNULL((SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = IT.ITEMCTRID),'')ITEMCTRNAME,"
            strSql += " ISNULL((SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACCODE = IT.ACCODE),'')DESIGNERNAME,"
            strSql += " IT.ITEMID,IT.SUBITEMID FROM " & cnStockDb & "..RECEIPT IT"
            strSql += " LEFT JOIN " & cnStockDb & "..QCISSUE AS QC ON QC.SNO = IT.BATCHNO AND QC.TRANTYPE = 'Q' AND ISNULL(QC.CANCEL,'')=''"
            strSql += " WHERE 1 = 1"
            If TranNo <> "" Then strSql += " AND IT.TRANNO = " & TranNo
            strSql += " AND IT.TRANTYPE = 'RPU'"
            strSql += " AND ISNULL(QC.SNO,'')=''"
        End If
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Exit Sub
        End If
        With dt.Rows(0)
            txtTranNo.Text = TranNo.ToString
            dtpRecieptDate.Value = GetEntryDate(.Item("TRANDATE"))
            dtpRecieptDate.Enabled = False

            txtItemName__Man.Text = .Item("ITEMNAME").ToString
            If cmbCostCentre.Enabled = True Then
                cmbCostCentre.Text = .Item("COSTNAME").ToString
            End If
            'cmbCounter.Text = .Item("ITEMCTRNAME").ToString
            If cmbCounter.Items.Count > 0 Then cmbCounter.Enabled = True Else cmbCounter.Enabled = False
            cmbDesigner.Text = .Item("DESIGNERNAME").ToString
            If cmbDesigner.Items.Count > 0 Then cmbDesigner.Enabled = True Else cmbDesigner.Enabled = False
            cmbItemSize.Items.Clear()
            If txtItemName__Man.Text <> "" Then
                strSql = " SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE"
                strSql += " WHERE ITEMID =(SELECT ITEMID FROM  " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItemName__Man.Text & "')"
                strSql += " ORDER BY SIZENAME"
                objGPack.FillCombo(strSql, cmbItemSize)
            End If
            If cmbItemSize.Items.Count > 0 Then cmbItemSize.Enabled = True Else cmbItemSize.Enabled = False
            cmbSubItem_MAN.Items.Clear()
            cmbSubItem_MAN.Text = ""
            cmbSubItem_MAN.Enabled = False
            If .Item("SUBITEMNAME").ToString <> "" Then
                cmbSubItem_MAN.Enabled = True
                cmbSubItem_MAN.Items.Add(.Item("SUBITEMNAME").ToString)
                cmbSubItem_MAN.Text = .Item("SUBITEMNAME").ToString
            Else
                pieceRate = 0
                purFineRate = 0
                strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = " & Val(.Item("ITEMID").ToString) & ""
                objGPack.FillCombo(strSql, cmbSubItem_MAN)
                If cmbSubItem_MAN.Items.Count > 0 Then cmbSubItem_MAN.Enabled = True
            End If
            txtPieces.Text = IIf(Val(.Item("PCS").ToString) <> 0, .Item("PCS").ToString, 0)
            txtGrsWt.Text = IIf(Val(.Item("GRSWT").ToString) <> 0, .Item("GRSWT").ToString, 0)
            txtNetWt.Text = IIf(Val(.Item("NETWT").ToString) <> 0, .Item("NETWT").ToString, 0)
            If optHM.Checked Then
                txtPieces.Enabled = True
                txtGrsWt.Enabled = True
                txtNetWt.Enabled = True
            Else
                txtPieces.Enabled = False
                txtGrsWt.Enabled = False
                txtNetWt.Enabled = False
            End If
        End With
    End Sub
    Private Sub funcGetIssueDetails()
        If optHM.Checked Then
            strSql = " SELECT I.TRANNO,(I.PCS-ISNULL(R.PCS,0))PCS,(I.GRSWT-ISNULL(R.GRSWT,0))GRSWT,(I.NETWT-ISNULL(R.NETWT,0))NETWT,I.TRANDATE, “
            strSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)ITEMNAME, “
            strSql += vbCrLf + " (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID)SUBITEMNAME, “
            strSql += vbCrLf + " ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = I.COSTID),'')COSTNAME, “
            strSql += vbCrLf + " ISNULL((SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACCODE = I.ACCODE),'')DESIGNERNAME, I.ITEMID,I.SUBITEMID FROM " & cnStockDb & "..QCRECEIPT I “
            strSql += vbCrLf + " LEFT JOIN (SELECT SNO, ISNULL(SUM(PCS),0) PCS, ISNULL(SUM(GRSWT),0) GRSWT, “
            strSql += vbCrLf + " ISNULL(SUM(NETWT),0) NETWT FROM " & cnStockDb & "..QCISSUE WHERE TRANTYPE = 'H' AND ISNULL(CANCEL,'')='' GROUP BY SNO) AS R ON R.SNO = I.BATCHNO WHERE 1=1 “
            strSql += vbCrLf + " AND I.GRSWT > ISNULL(R.GRSWT,0) AND I.TRANTYPE = 'Q' AND ISNULL(R.SNO,'')='' AND ISNULL(I.RETURNSTATUS,'')=''“
            If TranNo <> "" Then strSql += vbCrLf + " AND I.TRANNO = " & TranNo
            strSql += vbCrLf + " UNION “
            strSql += vbCrLf + " SELECT I.TRANNO,(I.PCS-ISNULL(R.PCS,0))PCS,(I.GRSWT-ISNULL(R.GRSWT,0))GRSWT,(I.NETWT-ISNULL(R.NETWT,0))NETWT,I.TRANDATE, “
            strSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)ITEMNAME, “
            strSql += vbCrLf + " (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID)SUBITEMNAME, “
            strSql += vbCrLf + " ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = I.COSTID),'')COSTNAME, “
            strSql += vbCrLf + " ISNULL((SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACCODE = I.ACCODE),'')DESIGNERNAME, I.ITEMID,I.SUBITEMID FROM " & cnStockDb & "..QCRECEIPT I “
            strSql += vbCrLf + " INNER JOIN (SELECT SNO, ISNULL(SUM(PCS),0) PCS, ISNULL(SUM(GRSWT),0) GRSWT, ISNULL(SUM(NETWT),0) NETWT FROM " & cnStockDb & "..QCISSUE WHERE TRANTYPE = 'H' “
            strSql += vbCrLf + " AND ISNULL(CANCEL,'')='' GROUP BY SNO) AS R ON R.SNO = I.BATCHNO WHERE 1=1 AND I.GRSWT > ISNULL(R.GRSWT,0) AND I.TRANTYPE = 'Q' AND ISNULL(I.RETURNSTATUS,'')=''“
            If TranNo <> "" Then strSql += vbCrLf + " AND I.TRANNO = " & TranNo
        Else
            strSql = " SELECT IT.PCS,IT.GRSWT,IT.NETWT,IT.TRANDATE,"
            strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = IT.ITEMID)ITEMNAME,"
            strSql += " (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = IT.ITEMID AND SUBITEMID = IT.SUBITEMID)SUBITEMNAME,"
            strSql += " ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = IT.COSTID),'')COSTNAME,"
            strSql += " ISNULL((SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = IT.ITEMCTRID),'')ITEMCTRNAME,"
            strSql += " ISNULL((SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACCODE = IT.ACCODE),'')DESIGNERNAME,"
            strSql += " IT.ITEMID,IT.SUBITEMID FROM " & cnStockDb & "..RECEIPT IT"
            strSql += " LEFT JOIN " & cnStockDb & "..QCISSUE AS QC ON QC.SNO = IT.BATCHNO AND QC.TRANTYPE = 'Q' AND ISNULL(QC.CANCEL,'')=''"
            strSql += " WHERE 1 = 1"
            If TranNo <> "" Then strSql += " AND IT.TRANNO = " & TranNo
            strSql += " AND IT.TRANTYPE = 'RPU'"
            strSql += " AND ISNULL(QC.SNO,'')=''"
        End If
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Exit Sub
        End If
        With dt.Rows(0)
            txtTranNo.Text = TranNo.ToString
            dtpRecieptDate.Value = GetEntryDate(.Item("TRANDATE"))
            dtpRecieptDate.Enabled = False

            txtItemName__Man.Text = .Item("ITEMNAME").ToString
            If cmbCostCentre.Enabled = True Then
                cmbCostCentre.Text = .Item("COSTNAME").ToString
            End If
            'cmbCounter.Text = .Item("ITEMCTRNAME").ToString
            If cmbCounter.Items.Count > 0 Then cmbCounter.Enabled = True Else cmbCounter.Enabled = False
            If .Item("DESIGNERNAME").ToString <> "" Then
                cmbDesigner.Text = .Item("DESIGNERNAME").ToString
                cmbDesigner.Enabled = False
            End If
            'If cmbDesigner.Items.Count > 0 Then cmbDesigner.Enabled = True Else cmbDesigner.Enabled = False
            cmbItemSize.Items.Clear()
            If txtItemName__Man.Text <> "" Then
                strSql = " SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE"
                strSql += " WHERE ITEMID =(SELECT ITEMID FROM  " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItemName__Man.Text & "')"
                strSql += " ORDER BY SIZENAME"
                objGPack.FillCombo(strSql, cmbItemSize)
            End If
            If cmbItemSize.Items.Count > 0 Then cmbItemSize.Enabled = True Else cmbItemSize.Enabled = False
            cmbSubItem_MAN.Items.Clear()
            cmbSubItem_MAN.Text = ""
            cmbSubItem_MAN.Enabled = False
            If .Item("SUBITEMNAME").ToString <> "" Then
                cmbSubItem_MAN.Enabled = True
                cmbSubItem_MAN.Items.Add(.Item("SUBITEMNAME").ToString)
                cmbSubItem_MAN.Text = .Item("SUBITEMNAME").ToString
            Else
                pieceRate = 0
                purFineRate = 0
                strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = " & Val(.Item("ITEMID").ToString) & ""
                objGPack.FillCombo(strSql, cmbSubItem_MAN)
                If cmbSubItem_MAN.Items.Count > 0 Then cmbSubItem_MAN.Enabled = True
            End If
            txtPieces.Text = IIf(Val(.Item("PCS").ToString) <> 0, .Item("PCS").ToString, 0)
            txtGrsWt.Text = IIf(Val(.Item("GRSWT").ToString) <> 0, .Item("GRSWT").ToString, 0)
            txtNetWt.Text = IIf(Val(.Item("NETWT").ToString) <> 0, .Item("NETWT").ToString, 0)
            If optHM.Checked Then
                txtPieces.Enabled = True
                txtGrsWt.Enabled = True
                txtNetWt.Enabled = True
            Else
                txtPieces.Enabled = False
                txtGrsWt.Enabled = False
                txtNetWt.Enabled = False
            End If
        End With
    End Sub
    Private Sub funcGetReceiptDetails_old()
        Dim tranType As String = IIf(optHM.Checked, "H", "Q")
        strSql = " SELECT (I.PCS-ISNULL(R.PCS,0))PCS,(I.GRSWT-ISNULL(R.GRSWT,0))GRSWT,(I.NETWT-ISNULL(R.NETWT,0))NETWT,I.TRANDATE,"
        strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)ITEMNAME,"
        strSql += " (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID)SUBITEMNAME,"
        strSql += " ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = I.COSTID),'')COSTNAME,"
        strSql += " ISNULL((SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACCODE = I.ACCODE),'')DESIGNERNAME,"
        strSql += " I.ITEMID,I.SUBITEMID,I.HMEMPID,I.HALLMARKID FROM " & cnStockDb & "..QCISSUE I"
        strSql += " LEFT JOIN (SELECT SNO, ISNULL(SUM(PCS),0) PCS, ISNULL(SUM(GRSWT),0) GRSWT, ISNULL(SUM(NETWT),0) NETWT FROM " & cnStockDb & "..QCRECEIPT WHERE TRANTYPE = '" & tranType & "' AND ISNULL(CANCEL,'')='' GROUP BY SNO) AS R ON R.SNO = I.BATCHNO"
        strSql += " WHERE 1 = 1"
        If TranNo <> "" Then strSql += " AND I.TRANNO = " & TranNo
        strSql += " AND I.TRANTYPE = '" & tranType & "'"
        strSql += " AND ISNULL(R.SNO,'')=''"
        'strSql += " UNION"
        'strSql += " SELECT (I.PCS-ISNULL(R.PCS,0))PCS,(I.GRSWT-ISNULL(R.GRSWT,0))GRSWT,(I.NETWT-ISNULL(R.NETWT,0))NETWT,I.TRANDATE,"
        'strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)ITEMNAME,"
        'strSql += " (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID)SUBITEMNAME,"
        'strSql += " ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = I.COSTID),'')COSTNAME,"
        'strSql += " ISNULL((SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACCODE = I.ACCODE),'')DESIGNERNAME,"
        'strSql += " I.ITEMID,I.SUBITEMID FROM " & cnStockDb & "..QCISSUE I"
        'strSql += " INNER JOIN (SELECT SNO, ISNULL(SUM(PCS),0) PCS, ISNULL(SUM(GRSWT),0) GRSWT, ISNULL(SUM(NETWT),0) NETWT FROM " & cnStockDb & "..QCRECEIPT WHERE TRANTYPE = '" & tranType & "' AND ISNULL(CANCEL,'')='' GROUP BY SNO) AS R ON R.SNO = I.BATCHNO"
        'strSql += " WHERE 1 = 1"
        'If TranNo <> "" Then strSql += " AND I.TRANNO = " & TranNo
        'strSql += " AND I.TRANTYPE = '" & tranType & "'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Exit Sub
        End If
        With dt.Rows(0)
            txtTranNo.Text = TranNo.ToString
            dtpRecieptDate.Value = GetEntryDate(.Item("TRANDATE"))
            dtpRecieptDate.Enabled = False

            txtItemName__Man.Text = .Item("ITEMNAME").ToString
            If cmbCostCentre.Enabled = True Then
                cmbCostCentre.Text = .Item("COSTNAME").ToString
            End If
            If cmbCounter.Items.Count > 0 Then cmbCounter.Enabled = True Else cmbCounter.Enabled = False
            cmbDesigner.Text = .Item("DESIGNERNAME").ToString
            If cmbDesigner.Items.Count > 0 Then cmbDesigner.Enabled = True Else cmbDesigner.Enabled = False
            cmbItemSize.Items.Clear()
            If txtItemName__Man.Text <> "" Then
                strSql = " SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE"
                strSql += " WHERE ITEMID =(SELECT ITEMID FROM  " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItemName__Man.Text & "')"
                strSql += " ORDER BY SIZENAME"
                objGPack.FillCombo(strSql, cmbItemSize)
            End If
            If cmbItemSize.Items.Count > 0 Then cmbItemSize.Enabled = True Else cmbItemSize.Enabled = False
            cmbSubItem_MAN.Items.Clear()
            cmbSubItem_MAN.Text = ""
            cmbSubItem_MAN.Enabled = False
            If .Item("SUBITEMNAME").ToString <> "" Then
                cmbSubItem_MAN.Enabled = True
                cmbSubItem_MAN.Items.Add(.Item("SUBITEMNAME").ToString)
                cmbSubItem_MAN.Text = .Item("SUBITEMNAME").ToString
            Else
                pieceRate = 0
                purFineRate = 0
                strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = " & Val(.Item("ITEMID").ToString) & ""
                objGPack.FillCombo(strSql, cmbSubItem_MAN)
                If cmbSubItem_MAN.Items.Count > 0 Then cmbSubItem_MAN.Enabled = True
            End If
            actPieces = IIf(Val(.Item("PCS").ToString) <> 0, .Item("PCS").ToString, 0)
            actGrsWt = IIf(Val(.Item("GRSWT").ToString) <> 0, .Item("GRSWT").ToString, 0)
            actNetWt = IIf(Val(.Item("NETWT").ToString) <> 0, .Item("NETWT").ToString, 0)

            cmbEmployee.SelectedValue = IIf(Val(.Item("HMEMPID").ToString) <> 0, .Item("HMEMPID").ToString, 0)
            cmbHM.SelectedValue = IIf(Val(.Item("HALLMARKID").ToString) <> 0, .Item("HALLMARKID").ToString, 0)

            txtPieces.Text = actPieces
            txtGrsWt.Text = actGrsWt
            txtNetWt.Text = actNetWt
            txtPieces.Enabled = False
            txtGrsWt.Enabled = False
            txtNetWt.Enabled = False
        End With
    End Sub
    Private Sub funcGetReceiptDetails()
        Dim tranType As String = IIf(optHM.Checked, "H", "Q")
        strSql = " SELECT (I.PCS-ISNULL(R.PCS,0))PCS,(I.GRSWT-ISNULL(R.GRSWT,0))GRSWT,(I.NETWT-ISNULL(R.NETWT,0))NETWT,I.TRANDATE,"
        strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)ITEMNAME,"
        strSql += " (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID)SUBITEMNAME,"
        strSql += " ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = I.COSTID),'')COSTNAME,"
        strSql += " ISNULL((SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACCODE = I.ACCODE),'')DESIGNERNAME,"
        strSql += " I.ITEMID,I.SUBITEMID,I.HMEMPID,I.HALLMARKID FROM " & cnStockDb & "..QCISSUE I"
        strSql += " LEFT JOIN (SELECT SNO, ISNULL(SUM(PCS),0) PCS, ISNULL(SUM(GRSWT),0) GRSWT, ISNULL(SUM(NETWT),0) NETWT FROM " & cnStockDb & "..QCRECEIPT WHERE TRANTYPE = '" & tranType & "' AND ISNULL(CANCEL,'')='' GROUP BY SNO) AS R ON R.SNO = I.BATCHNO"
        strSql += " WHERE 1 = 1"
        If TranNo <> "" Then strSql += " AND I.TRANNO = " & TranNo
        strSql += " AND I.TRANTYPE = '" & tranType & "'"
        strSql += " AND ISNULL(R.SNO,'')=''"
        strSql += " UNION"
        strSql += " SELECT (I.PCS-ISNULL(R.PCS,0))PCS,(I.GRSWT-ISNULL(R.GRSWT,0))GRSWT,(I.NETWT-ISNULL(R.NETWT,0))NETWT,I.TRANDATE,"
        strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)ITEMNAME,"
        strSql += " (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID)SUBITEMNAME,"
        strSql += " ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = I.COSTID),'')COSTNAME,"
        strSql += " ISNULL((SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACCODE = I.ACCODE),'')DESIGNERNAME,"
        strSql += " I.ITEMID,I.SUBITEMID,I.HMEMPID,I.HALLMARKID FROM " & cnStockDb & "..QCISSUE I"
        strSql += " INNER JOIN (SELECT SNO, ISNULL(SUM(PCS),0) PCS, ISNULL(SUM(GRSWT),0) GRSWT, ISNULL(SUM(NETWT),0) NETWT FROM " & cnStockDb & "..QCRECEIPT WHERE TRANTYPE = '" & tranType & "' AND ISNULL(CANCEL,'')='' GROUP BY SNO) AS R ON R.SNO = I.BATCHNO"
        strSql += " WHERE 1 = 1"
        If TranNo <> "" Then strSql += " AND I.TRANNO = " & TranNo
        strSql += " AND I.TRANTYPE = '" & tranType & "'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Exit Sub
        End If
        With dt.Rows(0)
            txtTranNo.Text = TranNo.ToString
            dtpRecieptDate.Value = GetEntryDate(.Item("TRANDATE"))
            dtpRecieptDate.Enabled = False

            txtItemName__Man.Text = .Item("ITEMNAME").ToString
            If cmbCostCentre.Enabled = True Then
                cmbCostCentre.Text = .Item("COSTNAME").ToString
            End If
            If cmbCounter.Items.Count > 0 Then cmbCounter.Enabled = True Else cmbCounter.Enabled = False
            If .Item("DESIGNERNAME").ToString <> "" Then
                cmbDesigner.Text = .Item("DESIGNERNAME").ToString
                cmbDesigner.Enabled = False
            End If
            'If cmbDesigner.Items.Count > 0 Then cmbDesigner.Enabled = True Else cmbDesigner.Enabled = False
            cmbItemSize.Items.Clear()
            If txtItemName__Man.Text <> "" Then
                strSql = " SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE"
                strSql += " WHERE ITEMID =(SELECT ITEMID FROM  " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItemName__Man.Text & "')"
                strSql += " ORDER BY SIZENAME"
                objGPack.FillCombo(strSql, cmbItemSize)
            End If
            If cmbItemSize.Items.Count > 0 Then cmbItemSize.Enabled = True Else cmbItemSize.Enabled = False
            cmbSubItem_MAN.Items.Clear()
            cmbSubItem_MAN.Text = ""
            cmbSubItem_MAN.Enabled = False
            If .Item("SUBITEMNAME").ToString <> "" Then
                cmbSubItem_MAN.Enabled = True
                cmbSubItem_MAN.Items.Add(.Item("SUBITEMNAME").ToString)
                cmbSubItem_MAN.Text = .Item("SUBITEMNAME").ToString
            Else
                pieceRate = 0
                purFineRate = 0
                strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = " & Val(.Item("ITEMID").ToString) & ""
                objGPack.FillCombo(strSql, cmbSubItem_MAN)
                If cmbSubItem_MAN.Items.Count > 0 Then cmbSubItem_MAN.Enabled = True
            End If
            actPieces = IIf(Val(.Item("PCS").ToString) <> 0, .Item("PCS").ToString, 0)
            actGrsWt = IIf(Val(.Item("GRSWT").ToString) <> 0, .Item("GRSWT").ToString, 0)
            actNetWt = IIf(Val(.Item("NETWT").ToString) <> 0, .Item("NETWT").ToString, 0)

            cmbEmployee.SelectedValue = IIf(Val(.Item("HMEMPID").ToString) <> 0, .Item("HMEMPID").ToString, 0)
            cmbHM.SelectedValue = IIf(Val(.Item("HALLMARKID").ToString) <> 0, .Item("HALLMARKID").ToString, 0)

            txtPieces.Text = actPieces
            txtGrsWt.Text = actGrsWt
            txtNetWt.Text = actNetWt
            txtPieces.Enabled = True
            txtGrsWt.Enabled = True
            txtNetWt.Enabled = True
            cmbEmployee.Enabled = False
            cmbHM.Enabled = False
        End With
    End Sub
    Private Sub funcGetReturnDetails_old()
        Dim tranType As String = IIf(optHM.Checked, "H", "Q")
        strSql = " 	;WITH CTE1 AS(	“
        strSql += vbCrLf + " 	SELECT R.SNO,R.TRANNO,(ISNULL(R.PCS,0))PCS,(ISNULL(R.GRSWT,0))GRSWT,(ISNULL(R.NETWT,0))NETWT,I.TRANDATE, (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)ITEMNAME, 	“
        strSql += vbCrLf + " 	(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID)SUBITEMNAME, 	“
        strSql += vbCrLf + " 	ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = I.COSTID),'')COSTNAME, ISNULL((SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACCODE = I.ACCODE),'')DESIGNERNAME, I.ITEMID,	“
        strSql += vbCrLf + " 	I.SUBITEMID FROM " & cnStockDb & "..QCISSUE I 	“
        strSql += vbCrLf + " 	INNER JOIN (SELECT SNO,TRANNO, ISNULL(SUM(PCS),0) PCS, ISNULL(SUM(GRSWT),0) GRSWT, ISNULL(SUM(NETWT),0) NETWT FROM " & cnStockDb & "..QCRECEIPT WHERE TRANTYPE = 'Q' 	“
        strSql += vbCrLf + " 	AND ISNULL(CANCEL,'')='' AND ISNULL(RETURNSTATUS,'') <> 'R' GROUP BY SNO,TRANNO) AS R ON R.SNO = I.BATCHNO WHERE 1=1 	“
        strSql += vbCrLf + " 	AND I.TRANTYPE = 'Q' AND ISNULL(I.CANCEL,'')='' 	“
        strSql += vbCrLf + " 	),	“
        strSql += vbCrLf + " 	CTE2 AS(	“
        strSql += vbCrLf + " 	SELECT R.SNO,R.TRANNO,(ISNULL(R.PCS,0))PCS,(ISNULL(R.GRSWT,0))GRSWT,(ISNULL(R.NETWT,0))NETWT,I.TRANDATE, (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)ITEMNAME, 	“
        strSql += vbCrLf + " 	(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID)SUBITEMNAME, 	“
        strSql += vbCrLf + " 	ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = I.COSTID),'')COSTNAME, ISNULL((SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACCODE = I.ACCODE),'')DESIGNERNAME, I.ITEMID,	“
        strSql += vbCrLf + " 	I.SUBITEMID FROM " & cnStockDb & "..QCISSUE I 	“
        strSql += vbCrLf + " 	INNER JOIN (SELECT SNO,0 TRANNO, ISNULL(SUM(PCS),0) PCS, ISNULL(SUM(GRSWT),0) GRSWT, ISNULL(SUM(NETWT),0) NETWT FROM " & cnStockDb & "..QCRECEIPT WHERE TRANTYPE = 'Q' 	“
        strSql += vbCrLf + " 	AND ISNULL(CANCEL,'')='' AND ISNULL(RETURNSTATUS,'') = 'R' GROUP BY SNO) AS R ON R.SNO = I.BATCHNO WHERE 1=1 	“
        strSql += vbCrLf + " 	AND I.TRANTYPE = 'Q' AND ISNULL(I.CANCEL,'')=''	“
        strSql += vbCrLf + " 	)	“
        strSql += vbCrLf + " 	SELECT CTE1.TRANNO, (ISNULL(CTE1.PCS,0)-ISNULL(CTE2.PCS,0))PCS, (ISNULL(CTE1.GRSWT,0)-ISNULL(CTE2.GRSWT,0))GRSWT, (ISNULL(CTE1.NETWT,0)-ISNULL(CTE2.NETWT,0))NETWT, CTE1.TRANDATE, CTE1.ITEMNAME,“
        strSql += vbCrLf + " 	CTE1.SUBITEMNAME, CTE1.COSTNAME, CTE1.DESIGNERNAME,CTE1.DESIGNERNAME, CTE1.ITEMID, CTE1.SUBITEMID FROM CTE1“
        strSql += vbCrLf + " 	LEFT JOIN CTE2 ON CTE1.SNO = CTE2.SNO	“
        strSql += vbCrLf + " 	WHERE 1=1 AND (ISNULL(CTE1.GRSWT, 0) - ISNULL(CTE2.GRSWT, 0)) > 0"
        If txtTranNo.Text <> "" Then strSql += vbCrLf + " AND CTE1.TRANNO = " & txtTranNo.Text
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Exit Sub
        End If
        With dt.Rows(0)
            txtTranNo.Text = TranNo.ToString
            dtpRecieptDate.Value = GetEntryDate(.Item("TRANDATE"))
            dtpRecieptDate.Enabled = False

            txtItemName__Man.Text = .Item("ITEMNAME").ToString
            If cmbCostCentre.Enabled = True Then
                cmbCostCentre.Text = .Item("COSTNAME").ToString
            End If
            If cmbCounter.Items.Count > 0 Then cmbCounter.Enabled = True Else cmbCounter.Enabled = False
            cmbDesigner.Text = .Item("DESIGNERNAME").ToString
            If cmbDesigner.Items.Count > 0 Then cmbDesigner.Enabled = True Else cmbDesigner.Enabled = False
            cmbItemSize.Items.Clear()
            If txtItemName__Man.Text <> "" Then
                strSql = " SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE"
                strSql += " WHERE ITEMID =(SELECT ITEMID FROM  " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItemName__Man.Text & "')"
                strSql += " ORDER BY SIZENAME"
                objGPack.FillCombo(strSql, cmbItemSize)
            End If
            If cmbItemSize.Items.Count > 0 Then cmbItemSize.Enabled = True Else cmbItemSize.Enabled = False
            cmbSubItem_MAN.Items.Clear()
            cmbSubItem_MAN.Text = ""
            cmbSubItem_MAN.Enabled = False
            If .Item("SUBITEMNAME").ToString <> "" Then
                cmbSubItem_MAN.Enabled = True
                cmbSubItem_MAN.Items.Add(.Item("SUBITEMNAME").ToString)
                cmbSubItem_MAN.Text = .Item("SUBITEMNAME").ToString
            Else
                pieceRate = 0
                purFineRate = 0
                strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = " & Val(.Item("ITEMID").ToString) & ""
                objGPack.FillCombo(strSql, cmbSubItem_MAN)
                If cmbSubItem_MAN.Items.Count > 0 Then cmbSubItem_MAN.Enabled = True
            End If
            actPieces = IIf(Val(.Item("PCS").ToString) <> 0, .Item("PCS").ToString, 0)
            actGrsWt = IIf(Val(.Item("GRSWT").ToString) <> 0, .Item("GRSWT").ToString, 0)
            actNetWt = IIf(Val(.Item("NETWT").ToString) <> 0, .Item("NETWT").ToString, 0)

            txtPieces.Text = actPieces
            txtGrsWt.Text = actGrsWt
            txtNetWt.Text = actNetWt
            txtPieces.Enabled = True
            txtGrsWt.Enabled = True
            txtNetWt.Enabled = True
        End With
    End Sub
    Private Sub funcGetReturnDetails()
        Dim tranType As String = IIf(optHM.Checked, "H", "Q")
        strSql = " 	;WITH CTE1 AS(	“
        strSql += vbCrLf + " 	SELECT R.SNO,R.TRANNO,(ISNULL(R.PCS,0))PCS,(ISNULL(R.GRSWT,0))GRSWT,(ISNULL(R.NETWT,0))NETWT,I.TRANDATE, (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)ITEMNAME, 	“
        strSql += vbCrLf + " 	(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID)SUBITEMNAME, 	“
        strSql += vbCrLf + " 	ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = I.COSTID),'')COSTNAME, ISNULL((SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACCODE = I.ACCODE),'')DESIGNERNAME, I.ITEMID,	“
        strSql += vbCrLf + " 	I.SUBITEMID FROM " & cnStockDb & "..QCISSUE I 	“
        strSql += vbCrLf + " 	INNER JOIN (SELECT SNO,TRANNO, ISNULL(SUM(PCS),0) PCS, ISNULL(SUM(GRSWT),0) GRSWT, ISNULL(SUM(NETWT),0) NETWT FROM " & cnStockDb & "..QCRECEIPT WHERE TRANTYPE = 'Q' 	“
        strSql += vbCrLf + " 	AND ISNULL(CANCEL,'')='' AND ISNULL(RETURNSTATUS,'') <> 'R' GROUP BY SNO,TRANNO) AS R ON R.SNO = I.BATCHNO WHERE 1=1 	“
        strSql += vbCrLf + " 	AND I.TRANTYPE = 'Q' AND ISNULL(I.CANCEL,'')='' 	“
        strSql += vbCrLf + " 	),	“
        strSql += vbCrLf + " 	CTE2 AS(	“
        strSql += vbCrLf + " 	SELECT R.SNO,R.TRANNO,(ISNULL(R.PCS,0))PCS,(ISNULL(R.GRSWT,0))GRSWT,(ISNULL(R.NETWT,0))NETWT,I.TRANDATE, (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)ITEMNAME, 	“
        strSql += vbCrLf + " 	(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID)SUBITEMNAME, 	“
        strSql += vbCrLf + " 	ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = I.COSTID),'')COSTNAME, ISNULL((SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACCODE = I.ACCODE),'')DESIGNERNAME, I.ITEMID,	“
        strSql += vbCrLf + " 	I.SUBITEMID FROM " & cnStockDb & "..QCISSUE I 	“
        strSql += vbCrLf + " 	INNER JOIN (SELECT SNO,0 TRANNO, ISNULL(SUM(PCS),0) PCS, ISNULL(SUM(GRSWT),0) GRSWT, ISNULL(SUM(NETWT),0) NETWT FROM " & cnStockDb & "..QCRECEIPT WHERE TRANTYPE = 'Q' 	“
        strSql += vbCrLf + " 	AND ISNULL(CANCEL,'')='' AND ISNULL(RETURNSTATUS,'') = 'R' GROUP BY SNO) AS R ON R.SNO = I.BATCHNO WHERE 1=1 	“
        strSql += vbCrLf + " 	AND I.TRANTYPE = 'Q' AND ISNULL(I.CANCEL,'')=''	“
        strSql += vbCrLf + " 	)	“
        strSql += vbCrLf + " 	SELECT CTE1.TRANNO, (ISNULL(CTE1.PCS,0)-ISNULL(CTE2.PCS,0))PCS, (ISNULL(CTE1.GRSWT,0)-ISNULL(CTE2.GRSWT,0))GRSWT, (ISNULL(CTE1.NETWT,0)-ISNULL(CTE2.NETWT,0))NETWT, CTE1.TRANDATE, CTE1.ITEMNAME,“
        strSql += vbCrLf + " 	CTE1.SUBITEMNAME, CTE1.COSTNAME, CTE1.DESIGNERNAME,CTE1.DESIGNERNAME, CTE1.ITEMID, CTE1.SUBITEMID FROM CTE1“
        strSql += vbCrLf + " 	LEFT JOIN CTE2 ON CTE1.SNO = CTE2.SNO	“
        strSql += vbCrLf + " 	WHERE 1=1 AND (ISNULL(CTE1.GRSWT, 0) - ISNULL(CTE2.GRSWT, 0)) > 0"
        If txtTranNo.Text <> "" Then strSql += vbCrLf + " AND CTE1.TRANNO = " & txtTranNo.Text
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Exit Sub
        End If
        With dt.Rows(0)
            txtTranNo.Text = TranNo.ToString
            dtpRecieptDate.Value = GetEntryDate(.Item("TRANDATE"))
            dtpRecieptDate.Enabled = False

            txtItemName__Man.Text = .Item("ITEMNAME").ToString
            If cmbCostCentre.Enabled = True Then
                cmbCostCentre.Text = .Item("COSTNAME").ToString
            End If
            If cmbCounter.Items.Count > 0 Then cmbCounter.Enabled = True Else cmbCounter.Enabled = False
            If .Item("DESIGNERNAME").ToString <> "" Then
                cmbDesigner.Text = .Item("DESIGNERNAME").ToString
                cmbDesigner.Enabled = False
            End If
            'If cmbDesigner.Items.Count > 0 Then cmbDesigner.Enabled = True Else cmbDesigner.Enabled = False
            cmbItemSize.Items.Clear()
            If txtItemName__Man.Text <> "" Then
                strSql = " SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE"
                strSql += " WHERE ITEMID =(SELECT ITEMID FROM  " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & txtItemName__Man.Text & "')"
                strSql += " ORDER BY SIZENAME"
                objGPack.FillCombo(strSql, cmbItemSize)
            End If
            If cmbItemSize.Items.Count > 0 Then cmbItemSize.Enabled = True Else cmbItemSize.Enabled = False
            cmbSubItem_MAN.Items.Clear()
            cmbSubItem_MAN.Text = ""
            cmbSubItem_MAN.Enabled = False
            If .Item("SUBITEMNAME").ToString <> "" Then
                cmbSubItem_MAN.Enabled = True
                cmbSubItem_MAN.Items.Add(.Item("SUBITEMNAME").ToString)
                cmbSubItem_MAN.Text = .Item("SUBITEMNAME").ToString
            Else
                pieceRate = 0
                purFineRate = 0
                strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = " & Val(.Item("ITEMID").ToString) & ""
                objGPack.FillCombo(strSql, cmbSubItem_MAN)
                If cmbSubItem_MAN.Items.Count > 0 Then cmbSubItem_MAN.Enabled = True
            End If
            actPieces = IIf(Val(.Item("PCS").ToString) <> 0, .Item("PCS").ToString, 0)
            actGrsWt = IIf(Val(.Item("GRSWT").ToString) <> 0, .Item("GRSWT").ToString, 0)
            actNetWt = IIf(Val(.Item("NETWT").ToString) <> 0, .Item("NETWT").ToString, 0)

            txtPieces.Text = actPieces
            txtGrsWt.Text = actGrsWt
            txtNetWt.Text = actNetWt
            txtPieces.Enabled = True
            txtGrsWt.Enabled = True
            txtNetWt.Enabled = True
        End With
    End Sub
    Private Sub frmitemBulkTag_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub txtTranNo_Num_Man_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTranNo.KeyDown
        TranNo = Nothing
        If e.KeyCode = Keys.Insert Then
            If cmbTransaction.Text = "Issue" Then
                'If optHM.Checked Then
                '    strSql = " ;WITH CTE1 AS("
                '    strSql += vbCrLf + " SELECT I.TRANNO,I.SNO,"
                '    strSql += vbCrLf + " (I.PCS-ISNULL(R.PCS,0))PCS,"
                '    strSql += vbCrLf + " (I.GRSWT-ISNULL(R.GRSWT,0))GRSWT,"
                '    strSql += vbCrLf + " (I.NETWT-ISNULL(R.NETWT,0))NETWT,I.TRANDATE, "
                '    strSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)ITEMNAME, "
                '    strSql += vbCrLf + " (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID)SUBITEMNAME, "
                '    strSql += vbCrLf + " ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = I.COSTID),'')COSTNAME, "
                '    strSql += vbCrLf + " ISNULL((SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACCODE = I.ACCODE),'')DESIGNERNAME, I.ITEMID,I.SUBITEMID FROM " & cnStockDb & "..QCRECEIPT I "
                '    strSql += vbCrLf + " LEFT JOIN (SELECT SNO, ISNULL(SUM(PCS),0) PCS, ISNULL(SUM(GRSWT),0) GRSWT, "
                '    strSql += vbCrLf + " ISNULL(SUM(NETWT),0) NETWT FROM " & cnStockDb & "..QCISSUE WHERE TRANTYPE = 'H' AND ISNULL(CANCEL,'')='' "
                '    strSql += vbCrLf + " GROUP BY SNO) AS R ON R.SNO = I.BATCHNO WHERE 1=1 "
                '    strSql += vbCrLf + " AND I.GRSWT > ISNULL(R.GRSWT,0) AND I.TRANTYPE = 'Q' AND ISNULL(I.RETURNSTATUS,'')='' AND ISNULL(I.CANCEL,'')=''"
                '    strSql += vbCrLf + " ),"
                '    strSql += vbCrLf + " CTE2 AS("
                '    strSql += vbCrLf + " SELECT I.SNO,"
                '    strSql += vbCrLf + " ISNULL(I.PCS,0)PCS,"
                '    strSql += vbCrLf + " ISNULL(I.GRSWT,0)GRSWT,"
                '    strSql += vbCrLf + " ISNULL(I.NETWT,0)NETWT "
                '    strSql += vbCrLf + " FROM " & cnStockDb & "..QCRECEIPT I "
                '    strSql += vbCrLf + " WHERE 1=1 "
                '    strSql += vbCrLf + " AND I.TRANTYPE = 'Q' AND ISNULL(I.RETURNSTATUS,'')='R' AND ISNULL(I.CANCEL,'')=''"
                '    strSql += vbCrLf + " )"
                '    strSql += vbCrLf + " SELECT CTE1.TRANNO,CTE1.PCS-ISNULL(CTE2.PCS,0) PCS,CTE1.GRSWT-ISNULL(CTE2.GRSWT,0) GRSWT,CTE1.NETWT-ISNULL(CTE2.NETWT,0) NETWT,CTE1.TRANDATE,CTE1.ITEMNAME,CTE1.SUBITEMNAME,CTE1.COSTNAME,CTE1.DESIGNERNAME,"
                '    strSql += vbCrLf + " CTE1.ITEMID,CTE1.SUBITEMID FROM CTE1 "
                '    strSql += vbCrLf + " LEFT JOIN CTE2 ON CTE1.SNO = CTE2.SNO WHERE 1=1"
                '    strSql += vbCrLf + " AND (CTE1.GRSWT-ISNULL(CTE2.GRSWT,0)) > 0"
                '    If txtTranNo_Num_Man.Text <> "" Then strSql += vbCrLf + " AND CTE1.TRANNO = " & txtTranNo_Num_Man.Text
                '    'strSql = " SELECT I.TRANNO,(I.PCS-ISNULL(R.PCS,0))PCS,(I.GRSWT-ISNULL(R.GRSWT,0))GRSWT,(I.NETWT-ISNULL(R.NETWT,0))NETWT,I.TRANDATE, “
                '    'strSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)ITEMNAME, “
                '    'strSql += vbCrLf + " (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID)SUBITEMNAME, “
                '    'strSql += vbCrLf + " ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = I.COSTID),'')COSTNAME, “
                '    'strSql += vbCrLf + " ISNULL((SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACCODE = I.ACCODE),'')DESIGNERNAME, I.ITEMID,I.SUBITEMID FROM " & cnStockDb & "..QCRECEIPT I “
                '    'strSql += vbCrLf + " LEFT JOIN (SELECT SNO, ISNULL(SUM(PCS),0) PCS, ISNULL(SUM(GRSWT),0) GRSWT, “
                '    'strSql += vbCrLf + " ISNULL(SUM(NETWT),0) NETWT FROM " & cnStockDb & "..QCISSUE WHERE TRANTYPE = 'H' AND ISNULL(CANCEL,'')='' GROUP BY SNO) AS R ON R.SNO = I.BATCHNO WHERE 1=1 “
                '    'strSql += vbCrLf + " AND I.GRSWT > ISNULL(R.GRSWT,0) AND I.TRANTYPE = 'Q' AND ISNULL(R.SNO,'')='' AND ISNULL(I.RETURNSTATUS,'')='' AND ISNULL(I.CANCEL,'')=''“
                '    'If txtTranNo_Num_Man.Text <> "" Then strSql += vbCrLf + " AND I.TRANNO = " & txtTranNo_Num_Man.Text
                '    'strSql += vbCrLf + " UNION “
                '    'strSql += vbCrLf + " SELECT I.TRANNO,(I.PCS-ISNULL(R.PCS,0))PCS,(I.GRSWT-ISNULL(R.GRSWT,0))GRSWT,(I.NETWT-ISNULL(R.NETWT,0))NETWT,I.TRANDATE, “
                '    'strSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)ITEMNAME, “
                '    'strSql += vbCrLf + " (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID)SUBITEMNAME, “
                '    'strSql += vbCrLf + " ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = I.COSTID),'')COSTNAME, “
                '    'strSql += vbCrLf + " ISNULL((SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACCODE = I.ACCODE),'')DESIGNERNAME, I.ITEMID,I.SUBITEMID FROM " & cnStockDb & "..QCRECEIPT I “
                '    'strSql += vbCrLf + " INNER JOIN (SELECT SNO, ISNULL(SUM(PCS),0) PCS, ISNULL(SUM(GRSWT),0) GRSWT, ISNULL(SUM(NETWT),0) NETWT FROM " & cnStockDb & "..QCISSUE WHERE TRANTYPE = 'H' “
                '    'strSql += vbCrLf + " AND ISNULL(CANCEL,'')='' GROUP BY SNO) AS R ON R.SNO = I.BATCHNO WHERE 1=1 AND I.GRSWT > ISNULL(R.GRSWT,0) AND I.TRANTYPE = 'Q' AND ISNULL(I.RETURNSTATUS,'')=''“
                '    'If txtTranNo_Num_Man.Text <> "" Then strSql += vbCrLf + " AND I.TRANNO = " & txtTranNo_Num_Man.Text
                'Else
                '    strSql = vbCrLf + "  SELECT I.TRANNO,I.TRANDATE"
                '    strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = I.ACCODE)ACNAME"
                '    strSql += vbCrLf + "  ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)AS ITEM"
                '    strSql += vbCrLf + "  ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID)AS SUBITEM"
                '    strSql += vbCrLf + "  ,SUM(I.PCS - ISNULL(L.PCS,0))PCS"
                '    strSql += vbCrLf + "  ,SUM(I.GRSWT - ISNULL(L.GRSWT,0))GRSWT"
                '    strSql += vbCrLf + "  ,SUM(I.NETWT - ISNULL(L.NETWT,0))NETWT"
                '    strSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT I"
                '    strSql += vbCrLf + "  LEFT OUTER JOIN " & cnStockDb & "..MR_QA AS L ON L.MRSNO  = I.SNO "
                '    strSql += vbCrLf + "  LEFT JOIN " & cnStockDb & "..QCISSUE AS QC ON QC.SNO = I.BATCHNO AND QC.TRANTYPE = 'Q' AND ISNULL(QC.CANCEL,'')=''"
                '    strSql += vbCrLf + "  WHERE ISNULL(I.CANCEL,'') = '' AND ISNULL(L.CANCEL,'') = ''"
                '    strSql += vbCrLf + "  AND I.GRSWT <> 0 AND I.TRANTYPE = 'RPU'"
                '    strSql += vbCrLf + "  AND ISNULL(QC.SNO,'')=''"
                '    If txtTranNo_Num_Man.Text <> "" Then strSql += vbCrLf + "  AND I.TRANNO = " & txtTranNo_Num_Man.Text
                '    strSql += vbCrLf + "  GROUP BY I.SNO,I.TRANNO,I.TRANDATE,I.ACCODE,I.ITEMID,I.SUBITEMID,I.TOUCH,I.PUREWT,I.SNO,I.ITEMTYPEID,I.WASTAGE,I.MCHARGE"
                '    strSql += vbCrLf + "  HAVING "
                '    strSql += vbCrLf + "  (SUM(I.PCS - ISNULL(L.PCS,0)) > 0"
                '    strSql += vbCrLf + "  OR SUM(I.GRSWT - ISNULL(L.GRSWT,0)) > 0)"
                '    strSql += vbCrLf + "  ORDER BY ITEM"
                'End If
                If optHM.Checked Then
                    strSql = " SELECT I.TRANNO,(I.PCS-ISNULL(R.PCS,0))PCS,(I.GRSWT-ISNULL(R.GRSWT,0))GRSWT,(I.NETWT-ISNULL(R.NETWT,0))NETWT,I.TRANDATE, “
                    strSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)ITEMNAME, “
                    strSql += vbCrLf + " (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID)SUBITEMNAME, “
                    strSql += vbCrLf + " ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = I.COSTID),'')COSTNAME, “
                    strSql += vbCrLf + " ISNULL((SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACCODE = I.ACCODE),'')DESIGNERNAME, I.ITEMID,I.SUBITEMID FROM " & cnStockDb & "..QCRECEIPT I “
                    strSql += vbCrLf + " LEFT JOIN (SELECT SNO, ISNULL(SUM(PCS),0) PCS, ISNULL(SUM(GRSWT),0) GRSWT, “
                    strSql += vbCrLf + " ISNULL(SUM(NETWT),0) NETWT FROM " & cnStockDb & "..QCISSUE WHERE TRANTYPE = 'H' AND ISNULL(CANCEL,'')='' GROUP BY SNO) AS R ON R.SNO = I.BATCHNO WHERE 1=1 “
                    strSql += vbCrLf + " AND I.GRSWT > ISNULL(R.GRSWT,0) AND I.TRANTYPE = 'Q' AND ISNULL(R.SNO,'')='' AND ISNULL(I.RETURNSTATUS,'')='' AND ISNULL(I.CANCEL,'')=''“
                    If txtTranNo.Text <> "" Then strSql += vbCrLf + " AND I.TRANNO = " & txtTranNo.Text
                    strSql += vbCrLf + " UNION “
                    strSql += vbCrLf + " SELECT I.TRANNO,(I.PCS-ISNULL(R.PCS,0))PCS,(I.GRSWT-ISNULL(R.GRSWT,0))GRSWT,(I.NETWT-ISNULL(R.NETWT,0))NETWT,I.TRANDATE, “
                    strSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)ITEMNAME, “
                    strSql += vbCrLf + " (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID)SUBITEMNAME, “
                    strSql += vbCrLf + " ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = I.COSTID),'')COSTNAME, “
                    strSql += vbCrLf + " ISNULL((SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACCODE = I.ACCODE),'')DESIGNERNAME, I.ITEMID,I.SUBITEMID FROM " & cnStockDb & "..QCRECEIPT I “
                    strSql += vbCrLf + " INNER JOIN (SELECT SNO, ISNULL(SUM(PCS),0) PCS, ISNULL(SUM(GRSWT),0) GRSWT, ISNULL(SUM(NETWT),0) NETWT FROM " & cnStockDb & "..QCISSUE WHERE TRANTYPE = 'H' “
                    strSql += vbCrLf + " AND ISNULL(CANCEL,'')='' GROUP BY SNO) AS R ON R.SNO = I.BATCHNO WHERE 1=1 AND I.GRSWT > ISNULL(R.GRSWT,0) AND I.TRANTYPE = 'Q' AND ISNULL(I.RETURNSTATUS,'')=''“
                    If txtTranNo.Text <> "" Then strSql += vbCrLf + " AND I.TRANNO = " & txtTranNo.Text
                Else
                    strSql = vbCrLf + "  SELECT I.TRANNO,I.TRANDATE"
                    strSql += vbCrLf + "  ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = I.ACCODE)ACNAME"
                    strSql += vbCrLf + "  ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)AS ITEM"
                    strSql += vbCrLf + "  ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID)AS SUBITEM"
                    strSql += vbCrLf + "  ,SUM(I.PCS - ISNULL(L.PCS,0))PCS"
                    strSql += vbCrLf + "  ,SUM(I.GRSWT - ISNULL(L.GRSWT,0))GRSWT"
                    strSql += vbCrLf + "  ,SUM(I.NETWT - ISNULL(L.NETWT,0))NETWT"
                    strSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT I"
                    strSql += vbCrLf + "  LEFT OUTER JOIN " & cnStockDb & "..MR_QA AS L ON L.MRSNO  = I.SNO "
                    strSql += vbCrLf + "  LEFT JOIN " & cnStockDb & "..QCISSUE AS QC ON QC.SNO = I.BATCHNO AND QC.TRANTYPE = 'Q' AND ISNULL(QC.CANCEL,'')=''"
                    strSql += vbCrLf + "  WHERE ISNULL(I.CANCEL,'') = '' AND ISNULL(L.CANCEL,'') = ''"
                    strSql += vbCrLf + "  AND I.GRSWT <> 0 AND I.TRANTYPE = 'RPU'"
                    strSql += vbCrLf + "  AND ISNULL(QC.SNO,'')=''"
                    If txtTranNo.Text <> "" Then strSql += vbCrLf + "  AND I.TRANNO = " & txtTranNo.Text
                    strSql += vbCrLf + "  GROUP BY I.SNO,I.TRANNO,I.TRANDATE,I.ACCODE,I.ITEMID,I.SUBITEMID,I.TOUCH,I.PUREWT,I.SNO,I.ITEMTYPEID,I.WASTAGE,I.MCHARGE"
                    strSql += vbCrLf + "  HAVING "
                    strSql += vbCrLf + "  (SUM(I.PCS - ISNULL(L.PCS,0)) > 0"
                    strSql += vbCrLf + "  OR SUM(I.GRSWT - ISNULL(L.GRSWT,0)) > 0)"
                    strSql += vbCrLf + "  ORDER BY ITEM"
                End If
                Dim dt As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    'If dt.Rows.Count = 1 Then
                    '    TranNo = dt.Rows(0).Item("TRANNO").ToString
                    '    funcGetIssueDetails()
                    'Else 'MORE THAN ONE 
                    '    TranNo = BrighttechPack.SearchDialog.Show("Finding TranNo", strSql, cn, 1)
                    '    funcGetIssueDetails()
                    'End If
                    TranNo = BrighttechPack.SearchDialog.Show("Finding TranNo", strSql, cn, 1)
                    If TranNo.Trim <> "" Then
                        funcGetIssueDetails()
                    End If
                    If TranNo <> "" Then
                        txtTranNo.Enabled = False
                        Me.SelectNextControl(txtTranNo, True, True, True, True)
                        'Else
                        '    MsgBox(E0004 + Me.GetNextControl(txtTranNo, False).Text, MsgBoxStyle.Information)
                    End If
                Else
                    MsgBox("No Transaction found.", MsgBoxStyle.Information)
                End If
            ElseIf (cmbTransaction.Text = "Receipt" Or cmbTransaction.Text = "Return") Then
                Dim tranType As String = IIf(optHM.Checked, "H", "Q")
                strSql = " SELECT I.TRANNO,(I.PCS-ISNULL(R.PCS,0))PCS,(I.GRSWT-ISNULL(R.GRSWT,0))GRSWT,(I.NETWT-ISNULL(R.NETWT,0))NETWT,I.TRANDATE,"
                strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)ITEMNAME,"
                strSql += " (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID)SUBITEMNAME,"
                strSql += " ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = I.COSTID),'')COSTNAME,"
                strSql += " ISNULL((SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACCODE = I.ACCODE),'')DESIGNERNAME,"
                strSql += " I.ITEMID,I.SUBITEMID FROM " & cnStockDb & "..QCISSUE I"
                strSql += " LEFT JOIN (SELECT SNO, ISNULL(SUM(PCS),0) PCS, ISNULL(SUM(GRSWT),0) GRSWT, ISNULL(SUM(NETWT),0) NETWT FROM " & cnStockDb & "..QCRECEIPT WHERE TRANTYPE = '" & tranType & "' AND ISNULL(CANCEL,'')='' GROUP BY SNO) AS R ON R.SNO = I.BATCHNO"
                strSql += " WHERE 1=1"
                If txtTranNo.Text <> "" Then strSql += vbCrLf + " AND I.TRANNO = " & txtTranNo.Text
                strSql += " AND I.GRSWT > ISNULL(R.GRSWT,0)"
                strSql += " AND I.TRANTYPE = '" & tranType & "'"
                strSql += " AND ISNULL(R.SNO,'')='' AND ISNULL(I.CANCEL,'')=''"
                strSql += " UNION"
                strSql += " SELECT I.TRANNO,(I.PCS-ISNULL(R.PCS,0))PCS,(I.GRSWT-ISNULL(R.GRSWT,0))GRSWT,(I.NETWT-ISNULL(R.NETWT,0))NETWT,I.TRANDATE,"
                strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)ITEMNAME,"
                strSql += " (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID)SUBITEMNAME,"
                strSql += " ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = I.COSTID),'')COSTNAME,"
                strSql += " ISNULL((SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACCODE = I.ACCODE),'')DESIGNERNAME,"
                strSql += " I.ITEMID,I.SUBITEMID FROM " & cnStockDb & "..QCISSUE I"
                strSql += " INNER JOIN (SELECT SNO, ISNULL(SUM(PCS),0) PCS, ISNULL(SUM(GRSWT),0) GRSWT, ISNULL(SUM(NETWT),0) NETWT FROM " & cnStockDb & "..QCRECEIPT WHERE TRANTYPE = '" & tranType & "' AND ISNULL(CANCEL,'')='' GROUP BY SNO) AS R ON R.SNO = I.BATCHNO"
                strSql += " WHERE 1=1"
                If txtTranNo.Text <> "" Then strSql += vbCrLf + " AND I.TRANNO = " & txtTranNo.Text
                strSql += " AND I.GRSWT > ISNULL(R.GRSWT,0)"
                strSql += " AND I.TRANTYPE = '" & tranType & "' AND ISNULL(I.CANCEL,'')=''"
                Dim dt As New DataTable
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    'If dt.Rows.Count = 1 Then
                    '    TranNo = dt.Rows(0).Item("TRANNO").ToString
                    '    funcGetReceiptDetails()
                    'Else 'MORE THAN ONE 
                    '    TranNo = BrighttechPack.SearchDialog.Show("Finding TranNo", strSql, cn, 1)
                    '    funcGetReceiptDetails()
                    'End If
                    TranNo = BrighttechPack.SearchDialog.Show("Finding TranNo", strSql, cn, 1)
                    If TranNo.Trim <> "" Then
                        funcGetReceiptDetails()
                    End If
                    If TranNo <> "" Then
                        txtTranNo.Enabled = False
                        Me.SelectNextControl(txtTranNo, True, True, True, True)
                        'Else
                        '    MsgBox(E0004 + Me.GetNextControl(txtTranNo, False).Text, MsgBoxStyle.Information)
                    End If
                Else
                    MsgBox("No Transaction found.", MsgBoxStyle.Information)
                End If

                'ElseIf cmbTransaction.Text = "Return" Then
                '    Dim tranType As String = IIf(optHM.Checked, "H", "Q")
                '    'strSql = " 	;WITH CTE1 AS(	“
                '    'strSql += vbCrLf + " 	SELECT R.SNO,R.TRANNO,(ISNULL(R.PCS,0))PCS,(ISNULL(R.GRSWT,0))GRSWT,(ISNULL(R.NETWT,0))NETWT,I.TRANDATE, (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)ITEMNAME, 	“
                '    'strSql += vbCrLf + " 	(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID)SUBITEMNAME, 	“
                '    'strSql += vbCrLf + " 	ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = I.COSTID),'')COSTNAME, ISNULL((SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACCODE = I.ACCODE),'')DESIGNERNAME, I.ITEMID,	“
                '    'strSql += vbCrLf + " 	I.SUBITEMID FROM " & cnStockDb & "..QCISSUE I 	“
                '    'strSql += vbCrLf + " 	INNER JOIN (SELECT SNO,TRANNO, ISNULL(SUM(PCS),0) PCS, ISNULL(SUM(GRSWT),0) GRSWT, ISNULL(SUM(NETWT),0) NETWT FROM " & cnStockDb & "..QCRECEIPT WHERE TRANTYPE = 'Q' 	“
                '    'strSql += vbCrLf + " 	AND ISNULL(CANCEL,'')='' AND ISNULL(RETURNSTATUS,'') <> 'R' GROUP BY SNO,TRANNO) AS R ON R.SNO = I.BATCHNO WHERE 1=1 	“
                '    'strSql += vbCrLf + " 	AND I.TRANTYPE = 'Q' AND ISNULL(I.CANCEL,'')='' 	“
                '    'strSql += vbCrLf + " 	),	“
                '    'strSql += vbCrLf + " 	CTE2 AS(	“
                '    'strSql += vbCrLf + " 	SELECT R.SNO,R.TRANNO,(ISNULL(R.PCS,0))PCS,(ISNULL(R.GRSWT,0))GRSWT,(ISNULL(R.NETWT,0))NETWT,I.TRANDATE, (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)ITEMNAME, 	“
                '    'strSql += vbCrLf + " 	(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID)SUBITEMNAME, 	“
                '    'strSql += vbCrLf + " 	ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = I.COSTID),'')COSTNAME, ISNULL((SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACCODE = I.ACCODE),'')DESIGNERNAME, I.ITEMID,	“
                '    'strSql += vbCrLf + " 	I.SUBITEMID FROM " & cnStockDb & "..QCISSUE I 	“
                '    'strSql += vbCrLf + " 	INNER JOIN (SELECT SNO,0 TRANNO, ISNULL(SUM(PCS),0) PCS, ISNULL(SUM(GRSWT),0) GRSWT, ISNULL(SUM(NETWT),0) NETWT FROM " & cnStockDb & "..QCRECEIPT WHERE TRANTYPE = 'Q' 	“
                '    'strSql += vbCrLf + " 	AND ISNULL(CANCEL,'')='' AND ISNULL(RETURNSTATUS,'') = 'R' GROUP BY SNO) AS R ON R.SNO = I.BATCHNO WHERE 1=1 	“
                '    'strSql += vbCrLf + " 	AND I.TRANTYPE = 'Q' AND ISNULL(I.CANCEL,'')=''	“
                '    'strSql += vbCrLf + " 	)	“
                '    'strSql += vbCrLf + " 	SELECT CTE1.TRANNO, (ISNULL(CTE1.PCS,0)-ISNULL(CTE2.PCS,0))PCS, (ISNULL(CTE1.GRSWT,0)-ISNULL(CTE2.GRSWT,0))GRSWT, (ISNULL(CTE1.NETWT,0)-ISNULL(CTE2.NETWT,0))NETWT, CTE1.TRANDATE, CTE1.ITEMNAME,“
                '    'strSql += vbCrLf + " 	CTE1.SUBITEMNAME, CTE1.COSTNAME, CTE1.DESIGNERNAME,CTE1.DESIGNERNAME, CTE1.ITEMID, CTE1.SUBITEMID FROM CTE1“
                '    'strSql += vbCrLf + " 	LEFT JOIN CTE2 ON CTE1.SNO = CTE2.SNO	“
                '    'strSql += vbCrLf + " 	WHERE 1=1 AND (ISNULL(CTE1.GRSWT, 0) - ISNULL(CTE2.GRSWT, 0)) > 0"
                '    'If txtTranNo_Num_Man.Text <> "" Then strSql += vbCrLf + " AND CTE1.TRANNO = " & txtTranNo_Num_Man.Text
                '    strSql = " SELECT I.TRANNO,(I.PCS-ISNULL(R.PCS,0))PCS,(I.GRSWT-ISNULL(R.GRSWT,0))GRSWT,(I.NETWT-ISNULL(R.NETWT,0))NETWT,I.TRANDATE,"
                '    strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)ITEMNAME,"
                '    strSql += " (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID)SUBITEMNAME,"
                '    strSql += " ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = I.COSTID),'')COSTNAME,"
                '    strSql += " ISNULL((SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACCODE = I.ACCODE),'')DESIGNERNAME,"
                '    strSql += " I.ITEMID,I.SUBITEMID FROM " & cnStockDb & "..QCISSUE I"
                '    strSql += " LEFT JOIN (SELECT SNO, ISNULL(SUM(PCS),0) PCS, ISNULL(SUM(GRSWT),0) GRSWT, ISNULL(SUM(NETWT),0) NETWT FROM " & cnStockDb & "..QCRECEIPT WHERE TRANTYPE = '" & tranType & "' AND ISNULL(CANCEL,'')='' GROUP BY SNO) AS R ON R.SNO = I.BATCHNO"
                '    strSql += " WHERE 1=1"
                '    If txtTranNo_Num_Man.Text <> "" Then strSql += vbCrLf + " AND I.TRANNO = " & txtTranNo_Num_Man.Text
                '    strSql += " AND I.GRSWT > ISNULL(R.GRSWT,0)"
                '    strSql += " AND I.TRANTYPE = '" & tranType & "'"
                '    strSql += " AND ISNULL(R.SNO,'')='' AND ISNULL(I.CANCEL,'')=''"
                '    strSql += " UNION"
                '    strSql += " SELECT I.TRANNO,(I.PCS-ISNULL(R.PCS,0))PCS,(I.GRSWT-ISNULL(R.GRSWT,0))GRSWT,(I.NETWT-ISNULL(R.NETWT,0))NETWT,I.TRANDATE,"
                '    strSql += " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)ITEMNAME,"
                '    strSql += " (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID)SUBITEMNAME,"
                '    strSql += " ISNULL((SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = I.COSTID),'')COSTNAME,"
                '    strSql += " ISNULL((SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACCODE = I.ACCODE),'')DESIGNERNAME,"
                '    strSql += " I.ITEMID,I.SUBITEMID FROM " & cnStockDb & "..QCISSUE I"
                '    strSql += " INNER JOIN (SELECT SNO, ISNULL(SUM(PCS),0) PCS, ISNULL(SUM(GRSWT),0) GRSWT, ISNULL(SUM(NETWT),0) NETWT FROM " & cnStockDb & "..QCRECEIPT WHERE TRANTYPE = '" & tranType & "' AND ISNULL(CANCEL,'')='' GROUP BY SNO) AS R ON R.SNO = I.BATCHNO"
                '    strSql += " WHERE 1=1"
                '    If txtTranNo_Num_Man.Text <> "" Then strSql += vbCrLf + " AND I.TRANNO = " & txtTranNo_Num_Man.Text
                '    strSql += " AND I.GRSWT > ISNULL(R.GRSWT,0)"
                '    strSql += " AND I.TRANTYPE = '" & tranType & "' AND ISNULL(I.CANCEL,'')=''"
                '    Dim dt As New DataTable
                '    da = New OleDbDataAdapter(strSql, cn)
                '    da.Fill(dt)
                '    If dt.Rows.Count > 0 Then
                '        If dt.Rows.Count = 1 Then
                '            TranNo = dt.Rows(0).Item("TRANNO").ToString
                '            funcGetReturnDetails()
                '        Else 'MORE THAN ONE 
                '            TranNo = BrighttechPack.SearchDialog.Show("Finding TranNo", strSql, cn, 1)
                '            funcGetReturnDetails()
                '        End If
                '        If TranNo <> "" Then
                '            txtTranNo_Num_Man.Enabled = False
                '            Me.SelectNextControl(txtTranNo_Num_Man, True, True, True, True)
                '        Else
                '            MsgBox(E0004 + Me.GetNextControl(txtTranNo_Num_Man, False).Text, MsgBoxStyle.Information)
                '        End If
                '    Else
                '        MsgBox(E0004 + Me.GetNextControl(txtTranNo_Num_Man, False).Text, MsgBoxStyle.Information)
                '    End If

            End If
        End If
    End Sub
    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        btnSave_Click(Me, New EventArgs)
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
    Private Function SaveIssue(tranNo As String, ByRef trandate As Date, ByRef batchNo As String) As String
        strSql = "SELECT ISNULL(MAX(TRANNO),0)+1 TRANNO FROM " & cnStockDb & "..QCISSUE"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        Dim maxTranNo As Long = dt.Rows(0)(0).ToString()
        trandate = Now.Date.ToString("yyyy-MM-dd")
        batchNo = "000" & cnCompanyId & maxTranNo.ToString
        strSql = "INSERT INTO " & cnStockDb & "..QCISSUE (SNO,TRANNO,TRANDATE,TRANTYPE,PCS,GRSWT,NETWT,LESSWT,PUREWT,ITEMID,SUBITEMID,AMOUNT,RATE,BOARDRATE,RETURNSTATUS,COSTID,COMPANYID,"
        strSql += vbCrLf + "FLAG,EMPID,PURITY,CATCODE,OCATCODE,ACCODE,BATCHNO,REMARK1,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,CANCEL,CASHID,METALID,HMEMPID)"
        If optHM.Checked Then
            If cmbTransaction.Text = "Issue" Then
                strSql = "INSERT INTO " & cnStockDb & "..QCISSUE (SNO,TRANNO,TRANDATE,TRANTYPE,PCS,GRSWT,NETWT,LESSWT,PUREWT,ITEMID,SUBITEMID,AMOUNT,RATE,BOARDRATE,RETURNSTATUS,COSTID,COMPANYID,"
                strSql += vbCrLf + "FLAG,EMPID,PURITY,CATCODE,OCATCODE,ACCODE,BATCHNO,REMARK1,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,CANCEL,CASHID,METALID,HALLMARKID,HMEMPID)"
                strSql += vbCrLf + "SELECT R.BATCHNO," & maxTranNo & ",'" & trandate & "','H'," & txtPieces.Text & "," & txtGrsWt.Text & "," & txtNetWt.Text & ",R.LESSWT,R.PUREWT,R.ITEMID,R.SUBITEMID,R.AMOUNT,R.RATE,R.BOARDRATE,'','" & cnCostId & "','" & cnCompanyId & "',"
                strSql += vbCrLf + "R.FLAG,R.EMPID,R.PURITY,R.CATCODE,R.OCATCODE,R.ACCODE,'" & batchNo & "',R.REMARK1,R.REMARK2,'" & userId & "','" & trandate & "','" & DateTime.Now & "','','','',R.METALID," & cmbHM.SelectedValue & "," & cmbEmployee.SelectedValue & " FROM " & cnStockDb & "..QCRECEIPT R"
                strSql += vbCrLf + "WHERE 1=1 "
                strSql += vbCrLf + "AND R.TRANNO = " & tranNo
            Else
                strSql = "INSERT INTO " & cnStockDb & "..QCISSUE (SNO,TRANNO,TRANDATE,TRANTYPE,PCS,GRSWT,NETWT,LESSWT,PUREWT,ITEMID,SUBITEMID,AMOUNT,RATE,BOARDRATE,RETURNSTATUS,COSTID,COMPANYID,"
                strSql += vbCrLf + "FLAG,EMPID,PURITY,CATCODE,OCATCODE,ACCODE,BATCHNO,REMARK1,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,CANCEL,CASHID,METALID,HALLMARKID,HMEMPID)"
                strSql += vbCrLf + "SELECT R.BATCHNO," & maxTranNo & ",'" & trandate & "','H',R.PCS,R.GRSWT,R.NETWT,R.LESSWT,R.PUREWT,R.ITEMID,R.SUBITEMID,R.AMOUNT,R.RATE,R.BOARDRATE,'','" & cnCostId & "','" & cnCompanyId & "',"
                strSql += vbCrLf + "R.FLAG,R.EMPID,R.PURITY,R.CATCODE,R.OCATCODE,R.ACCODE,'" & batchNo & "',R.REMARK1,R.REMARK2,'" & userId & "','" & trandate & "','" & DateTime.Now & "','','','',R.METALID," & cmbHM.SelectedValue & "," & cmbEmployee.SelectedValue & " FROM " & cnStockDb & "..QCRECEIPT R"
                strSql += vbCrLf + "LEFT JOIN " & cnStockDb & "..QCISSUE QC ON QC.SNO = R.BATCHNO AND QC.TRANTYPE = 'H' AND ISNULL(QC.CANCEL,'')=''"
                strSql += vbCrLf + "WHERE R.TRANNO = " & tranNo & " AND R.TRANTYPE = 'Q'"
                strSql += vbCrLf + "AND ISNULL(QC.SNO,'')=''"
            End If
        Else
            strSql += vbCrLf + "SELECT R.BATCHNO," & maxTranNo & ",'" & trandate & "','Q',R.PCS,R.GRSWT,R.NETWT,R.LESSWT,R.PUREWT,R.ITEMID,R.SUBITEMID,R.AMOUNT,R.RATE,R.BOARDRATE,'','" & cnCostId & "','" & cnCompanyId & "',"
            strSql += vbCrLf + "R.FLAG,R.EMPID,R.PURITY,R.CATCODE,R.OCATCODE,R.ACCODE,'" & batchNo & "',R.REMARK1,R.REMARK2,'" & userId & "','" & trandate & "','" & DateTime.Now & "','','','',R.METALID," & cmbEmployee.SelectedValue & " FROM " & cnStockDb & "..RECEIPT R"
            strSql += vbCrLf + "LEFT JOIN " & cnStockDb & "..QCISSUE QC ON QC.SNO = R.BATCHNO AND QC.TRANTYPE = 'Q' AND ISNULL(QC.CANCEL,'')=''"
            strSql += vbCrLf + "WHERE R.TRANNO = " & tranNo & " AND R.TRANTYPE = 'RPU'"
            strSql += vbCrLf + "AND ISNULL(QC.SNO,'')=''"
        End If
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        Return maxTranNo
    End Function
    Private Function SaveReceiptAndReturn(tranNo As String, returnStatus As String, ByRef trandate As Date, ByRef batchNo As String) As String
        Dim tranType As String = IIf(optHM.Checked, "H", "Q")
        strSql = "SELECT ISNULL(MAX(TRANNO),0)+1 TRANNO FROM " & cnStockDb & "..QCRECEIPT"
        Dim dt As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        Dim maxTranNo As Long = dt.Rows(0)("TRANNO").ToString()
        trandate = Now.Date.ToString("yyyy-MM-dd")
        batchNo = "000" & cnCompanyId & maxTranNo.ToString

        strSql = "SELECT * FROM " & cnStockDb & "..QCISSUE WHERE TRANNO = " & tranNo
        dt = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)

        If optHM.Checked Then
            strSql = "INSERT INTO " & cnStockDb & "..QCRECEIPT (SNO,TRANNO,TRANDATE,TRANTYPE,PCS,GRSWT,NETWT,LESSWT,PUREWT,ITEMID,SUBITEMID,AMOUNT,RATE,BOARDRATE,RETURNSTATUS,COSTID,COMPANYID,"
            strSql += vbCrLf + "FLAG,EMPID,PURITY,CATCODE,OCATCODE,ACCODE,BATCHNO,REMARK1,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,CANCEL,CASHID,METALID,HALLMARKID,HMEMPID)"
            strSql += vbCrLf + $"SELECT '{dt.Rows(0)("BATCHNO").ToString()}',{maxTranNo},'{trandate}','{tranType}',{txtPieces.Text},{txtGrsWt.Text},{txtNetWt.Text},{dt.Rows(0)("LESSWT")},{dt.Rows(0)("PUREWT")},{dt.Rows(0)("ITEMID")},{dt.Rows(0)("SUBITEMID")},{txtGrsWt.Text}*{dt.Rows(0)("RATE")},{dt.Rows(0)("RATE")},{dt.Rows(0)("BOARDRATE")},'{returnStatus}','{cnCostId}','{cnCompanyId}',"
            strSql += vbCrLf + $"'{dt.Rows(0)("FLAG")}',{dt.Rows(0)("EMPID")},{dt.Rows(0)("PURITY")},'{dt.Rows(0)("CATCODE")}','{dt.Rows(0)("OCATCODE")}','{dt.Rows(0)("ACCODE")}','{batchNo}','{dt.Rows(0)("REMARK1")}','{dt.Rows(0)("REMARK2")}','{userId}','{trandate}','{DateTime.Now}','','','','{dt.Rows(0)("METALID")}'," & cmbHM.SelectedValue & "," & cmbEmployee.SelectedValue
        Else
            strSql = "INSERT INTO " & cnStockDb & "..QCRECEIPT (SNO,TRANNO,TRANDATE,TRANTYPE,PCS,GRSWT,NETWT,LESSWT,PUREWT,ITEMID,SUBITEMID,AMOUNT,RATE,BOARDRATE,RETURNSTATUS,COSTID,COMPANYID,"
            strSql += vbCrLf + "FLAG,EMPID,PURITY,CATCODE,OCATCODE,ACCODE,BATCHNO,REMARK1,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,CANCEL,CASHID,METALID,HMEMPID)"
            strSql += vbCrLf + $"SELECT '{dt.Rows(0)("BATCHNO").ToString()}',{maxTranNo},'{trandate}','{tranType}',{txtPieces.Text},{txtGrsWt.Text},{txtNetWt.Text},{dt.Rows(0)("LESSWT")},{dt.Rows(0)("PUREWT")},{dt.Rows(0)("ITEMID")},{dt.Rows(0)("SUBITEMID")},{txtGrsWt.Text}*{dt.Rows(0)("RATE")},{dt.Rows(0)("RATE")},{dt.Rows(0)("BOARDRATE")},'{returnStatus}','{cnCostId}','{cnCompanyId}',"
            strSql += vbCrLf + $"'{dt.Rows(0)("FLAG")}',{dt.Rows(0)("EMPID")},{dt.Rows(0)("PURITY")},'{dt.Rows(0)("CATCODE")}','{dt.Rows(0)("OCATCODE")}','{dt.Rows(0)("ACCODE")}','{batchNo}','{dt.Rows(0)("REMARK1")}','{dt.Rows(0)("REMARK2")}','{userId}','{tranDate}','{DateTime.Now}','','','','{dt.Rows(0)("METALID")}',{cmbEmployee.SelectedValue}"
        End If
        cmd = New OleDbCommand(strSql, cn, tran)
        cmd.ExecuteNonQuery()
        Return maxTranNo
    End Function
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub
    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        funcNew()
        tabMain.SelectedTab = tabEntry
    End Sub
    Private Sub btnView_Click(sender As Object, e As EventArgs) Handles btnView.Click
        Fn_View()
    End Sub
    Private Sub Fn_View()
        Try
            Dim tranType As String = IIf(optVHM.Checked, "H", "Q")
            If cmbViewTran.Text = "Issue" Then
                strSql = "SELECT TRANNO,CONVERT(VARCHAR(MAX),TRANDATE,105)TRANDATE,C.CATNAME CATEGORY,D.DESIGNERNAME DESIGNER,A.ITEMNAME ITEM,ISNULL(B.SUBITEMNAME,'')SUBITEM,PCS,I.GRSWT,NETWT,BATCHNO FROM " & cnStockDb & "..QCISSUE I"
                strSql += vbCrLf + "LEFT JOIN " & cnAdminDb & "..ITEMMAST A ON A.ITEMID = I.ITEMID"
                strSql += vbCrLf + "LEFT JOIN " & cnAdminDb & "..SUBITEMMAST B ON B.ITEMID = I.ITEMID AND B.SUBITEMID = I.SUBITEMID"
                strSql += vbCrLf + "INNER JOIN " & cnAdminDb & "..DESIGNER D ON D.ACCODE = I.ACCODE"
                strSql += vbCrLf + "INNER JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
                strSql += vbCrLf + "WHERE 1=1"
                strSql += vbCrLf + "AND ISNULL(CANCEL,'')=''"
                strSql += vbCrLf + "AND I.TRANTYPE = '" & tranType & "'"
                strSql += vbCrLf + "AND TRANDATE BETWEEN '" & dtpFromDate.Value.ToString("dd-MMM-yyyy") & "' AND '" & dtpToDate.Value.ToString("dd-MMM-yyyy") & "'"
            ElseIf (cmbViewTran.Text = "Receipt" Or cmbViewTran.Text = "Return") Then
                strSql = "SELECT TRANNO,CONVERT(VARCHAR(MAX),TRANDATE,105)TRANDATE,C.CATNAME CATEGORY,D.DESIGNERNAME DESIGNER,A.ITEMNAME ITEM,ISNULL(B.SUBITEMNAME,'')SUBITEM,PCS,I.GRSWT,NETWT,BATCHNO FROM " & cnStockDb & "..QCRECEIPT I"
                strSql += vbCrLf + "LEFT JOIN " & cnAdminDb & "..ITEMMAST A ON A.ITEMID = I.ITEMID"
                strSql += vbCrLf + "LEFT JOIN " & cnAdminDb & "..SUBITEMMAST B ON B.ITEMID = I.ITEMID AND B.SUBITEMID = I.SUBITEMID"
                strSql += vbCrLf + "INNER JOIN " & cnAdminDb & "..DESIGNER D ON D.ACCODE = I.ACCODE"
                strSql += vbCrLf + "INNER JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
                strSql += vbCrLf + "WHERE 1=1"
                strSql += vbCrLf + "AND ISNULL(CANCEL,'')=''"
                strSql += vbCrLf + "AND I.TRANTYPE = '" & tranType & "'"
                strSql += vbCrLf + "AND TRANDATE BETWEEN '" & dtpFromDate.Value.ToString("dd-MMM-yyyy") & "' AND '" & dtpToDate.Value.ToString("dd-MMM-yyyy") & "'"
                If cmbViewTran.Text = "Return" Then
                    strSql += vbCrLf + "AND RETURNSTATUS = 'R'"
                Else
                    strSql += vbCrLf + "AND ISNULL(RETURNSTATUS,'') = ''"
                End If
            End If
            Dim dt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            dgView.DataSource = Nothing
            If dt.Rows.Count > 0 Then
                dgView.DataSource = dt
                dgView.Columns("BATCHNO").Visible = False
            Else
                MsgBox("Records not found.", MsgBoxStyle.Information)
                Exit Sub
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub
    Private Sub dgView_KeyDown(sender As Object, e As KeyEventArgs) Handles dgView.KeyDown
        Try
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Cancel) Then Exit Sub
            If IsNothing(dgView) Then Exit Sub
            If e.KeyCode = Keys.C Then
                Dim tranNo As Long = dgView.Item("TRANNO", dgView.CurrentRow.Index).Value.ToString
                Dim batchNo As String = dgView.Item("BATCHNO", dgView.CurrentRow.Index).Value.ToString
                Dim upDate As Date = Now.Date.ToString("yyyy-MM-dd")
                If cmbViewTran.Text = "Issue" Then
                    strSql = "SELECT BATCHNO FROM " & cnStockDb & "..QCRECEIPT"
                    strSql += vbCrLf + "WHERE SNO = '" & batchNo & "'"
                    Dim dt As New DataTable
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(dt)
                    If dt.Rows.Count > 0 Then
                        MsgBox("This transaction cannot be canceled.", MsgBoxStyle.Information)
                        Exit Sub
                    Else
                        strSql = "UPDATE " & cnStockDb & "..QCISSUE SET CANCEL = 'Y', UPDATED = '" & upDate & "', UPTIME = '" & DateTime.Now & "'"
                        strSql += vbCrLf + "WHERE BATCHNO = '" & batchNo & "'"
                        cmd = New OleDbCommand(strSql, cn, tran)
                        cmd.ExecuteNonQuery()
                        MsgBox("Transaction canceled successfully.", MsgBoxStyle.Information)
                        Fn_View()
                    End If
                ElseIf (cmbViewTran.Text = "Receipt" Or cmbViewTran.Text = "Return") Then
                    strSql = "UPDATE " & cnStockDb & "..QCRECEIPT SET CANCEL = 'Y', UPDATED = '" & upDate & "', UPTIME = '" & DateTime.Now & "'"
                    strSql += vbCrLf + "WHERE BATCHNO = '" & batchNo & "'"
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()
                    MsgBox("Transaction canceled successfully.", MsgBoxStyle.Information)
                    Fn_View()
                End If
            ElseIf e.KeyCode = Keys.D Then
                Dim _batchNo As String = dgView.Item("BATCHNO", dgView.CurrentRow.Index).Value.ToString
                Dim _tranDate As String = dgView.Item("TRANDATE", dgView.CurrentRow.Index).Value.ToString
                Dim _type As String = ""
                If cmbViewTran.Text = "Issue" Then
                    If optVHM.Checked Then
                        _type = "HI"
                    Else
                        _type = "QI"
                    End If
                Else
                    If optVHM.Checked Then
                        _type = "HR"
                    Else
                        _type = "QR"
                    End If
                End If
                If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
                    Dim write As IO.StreamWriter
                    write = IO.File.CreateText(Application.StartupPath & "\BillPrint.mem")
                    write.WriteLine(LSet("TYPE", 15) & ":" & _type)
                    write.WriteLine(LSet("BATCHNO", 15) & ":" & _batchNo)
                    write.WriteLine(LSet("TRANDATE", 15) & ":" & _tranDate)
                    write.WriteLine(LSet("DUPLICATE", 15) & ":Y")
                    write.Flush()
                    write.Close()

                    If EXE_WITH_PARAM = False Then
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
                    Else
                        System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe",
                    LSet("TYPE", 15) & ":" & _Type & ";" &
                    LSet("BATCHNO", 15) & ":" & _batchNo & ";" &
                    LSet("TRANDATE", 15) & ":" & _tranDate & ";" &
                    LSet("DUPLICATE", 15) & ":Y")
                    End If

                Else
                    MsgBox("Billprint exe not found", MsgBoxStyle.Information)
                End If

            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub
    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If dgView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "QC " & cmbViewTran.Text, dgView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnOpen_Click(sender As Object, e As EventArgs) Handles btnOpen.Click
        cmbViewTran.SelectedIndex = 0
        dtpFromDate.Value = Now.Date.ToString("dd-MMM-yyyy")
        dtpToDate.Value = Now.Date.ToString("dd-MMM-yyyy")
        dgView.DataSource = Nothing
        tabMain.SelectedTab = tabView
        dtpFromDate.Focus()
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If dgView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "QC " & cmbViewTran.Text, dgView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub txtGrsWt_TextChanged(sender As Object, e As EventArgs) Handles txtGrsWt.TextChanged
        txtNetWt.Text = txtGrsWt.Text
    End Sub

    Private Sub cmbTransaction_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTransaction.SelectedIndexChanged
        If optHM.Checked Then
            If cmbTransaction.Text = "Issue" Then
                cmbHM.Enabled = True
                cmbEmployee.Enabled = True
            Else
                cmbHM.Enabled = False
            End If
        Else
            cmbHM.Enabled = False
        End If
    End Sub
    Private Sub optHM_CheckedChanged(sender As Object, e As EventArgs) Handles optHM.CheckedChanged
        If sender.Checked Then
            If cmbTransaction.Text = "Issue" Then
                cmbHM.Enabled = True
                cmbEmployee.Enabled = True
            Else
                cmbHM.Enabled = False
            End If
        Else
            cmbHM.Enabled = False
        End If
    End Sub

    Private Sub optQC_CheckedChanged(sender As Object, e As EventArgs) Handles optQC.CheckedChanged
        If optHM.Checked Then
            If cmbTransaction.Text = "Issue" Then
                cmbHM.Enabled = True
                cmbEmployee.Enabled = True
            Else
                cmbHM.Enabled = False
            End If
        Else
            cmbHM.Enabled = False
        End If
    End Sub

    Private Sub txtTranNo_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtTranNo.Validating
        If btnExit.Focused Or cmbTransaction.Focused Or btnNew.Focused Or optHM.Focused Or optQC.Focused Then Exit Sub
        If txtTranNo.Text.Trim = "" Then
            MsgBox("Enter TranNo.", MsgBoxStyle.Information)
            txtTranNo.Focus()
            Exit Sub
        End If
    End Sub
End Class

