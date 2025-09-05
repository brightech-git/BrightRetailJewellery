Imports System.Data.OleDb
Public Class frmOpeningWeightEntry
    Dim cmd As OleDbCommand
    Dim tran As OleDbTransaction
    Dim strSql As String
    Dim dr As OleDbDataReader
    Dim editFlag As Boolean = False
    Dim dtTemp As New DataTable
    Dim Sno As String = Nothing

    Dim dtOpenView As New DataTable
    Function funcGetDetails(ByVal RwIndex As Integer) As Integer
        editFlag = True
        With gridView.Rows(RwIndex)
            Sno = .Cells("SNO").Value
            If StrComp(.Cells("STOCKTYPE").Value.ToString, "C") = 0 Then
                rbtCompany.Checked = True
            Else
                rbtSmith.Checked = True
            End If
            cmbCostCentre_MAN.Text = objGPack.GetSqlValue("SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = '" & .Cells("COSTID").Value.ToString & "'")
            cmbSupplier_MAN.Text = .Cells("ACNAME").Value.ToString
            If StrComp(.Cells("TRANTYPE").Value.ToString, "ISSUE") = 0 Then
                rbtIssue.Checked = True
            Else
                rbtReceipt.Checked = True
            End If
            cmbMetal_MAN.Text = objGPack.GetSqlValue("SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & .Cells("CATNAME").Value.ToString & "')")
            cmbCategory_MAN.Text = .Cells("CATNAME").Value.ToString
            cmbItem_MAN.Text = .Cells("ITEMNAME").Value.ToString
            If cmbItem_MAN.Text = "" Then cmbItem_MAN.Enabled = False
            cmbSubItem_MAN.Text = .Cells("SUBITEMNAME").Value.ToString
            If cmbSubItem_MAN.Text = "" Then cmbSubItem_MAN.Enabled = False
            txtPcs_NUM.Text = .Cells("PCS").Value.ToString
            If .Cells("APPROVAL").Value.ToString = "A" Then
                txtAppGrsWeight_WET.Text = .Cells("GRSWT").Value.ToString
                txtAppNetWeight_WET.Text = .Cells("NETWT").Value.ToString
                txtAppPureWeight_WET.Text = .Cells("PUREWT").Value.ToString
            Else
                txtGrsWeight_WET.Text = .Cells("GRSWT").Value.ToString
                txtNetWeight_WET.Text = .Cells("NETWT").Value.ToString
                txtPureWeight_WET.Text = .Cells("PUREWT").Value.ToString
            End If
            txtRate_AMT.Text = .Cells("RATE").Value.ToString
            If StrComp(.Cells("CALCMODE").Value.ToString, "PIECE") = 0 Then
                rbtPiece.Checked = True
            Else
                rbtWeight.Checked = True
            End If

            If StrComp(.Cells("STONEUNIT").Value.ToString, "C") = 0 Then
                pnlStone.Visible = True
                rbtCarat.Checked = True
            ElseIf StrComp(.Cells("STONEUNIT").Value.ToString, "G") = 0 Then
                pnlStone.Visible = True
                rbtGram.Checked = True
            Else
                pnlStone.Visible = False
            End If
            txtAmount_AMT.Text = .Cells("AMOUNT").Value.ToString
            txtRemark.Text = .Cells("REMARK1").Value.ToString
        End With
        tabMain.SelectedTab = tabGeneral
        grpMain.Focus()
    End Function
    Function funcOpenViewStyle() As Integer
        With gridView
            With .Columns("ACNAME")
                .Width = 150
            End With
            With .Columns("CATNAME")
                .Width = 150
            End With
            With .Columns("ITEMNAME")
                .HeaderText = "ITEM"
                .Width = 150
            End With
            With .Columns("SUBITEMNAME")
                .HeaderText = "SUBITEM"
                .Width = 100
            End With
            With .Columns("PCS")
                .Width = 60
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("GRSWT")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("NETWT")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("AMOUNT")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("PUREWT")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("RATE")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
            With .Columns("TRANTYPE")
                .Width = 70
            End With
            With .Columns("CALCMODE")
                .Width = 70
            End With
            With .Columns("STONEUNIT")
                .HeaderText = "UNIT"
                .Width = 60
            End With
            For CNT As Integer = 13 To gridView.ColumnCount - 1
                .Columns(CNT).Visible = False
            Next
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        End With
    End Function
    Function funcOpenClear() As Integer
        cmbOpenMetal.Text = "ALL"
        rbtOpenCompany.Checked = True
        dtOpenView.Rows.Clear()
        If cmbOpenCostCentre.Enabled Then
            cmbOpenCostCentre.Select()
        Else
            cmbMetal_MAN.Select()
        End If
    End Function
    Function funcCalcAmount() As String
        Dim amt As Double = Nothing
        If rbtPiece.Checked = True Then
            amt = Val(txtPcs_NUM.Text) * Val(txtRate_AMT.Text)
        Else
            amt = Val(txtGrsWeight_WET.Text) * Val(txtRate_AMT.Text)
        End If
        If amt <> 0 Then
            Return Format(amt, "0.00")
        End If
        Return ""
    End Function
    Function funcCalcPureWeight(ByVal grswt As Double) As String
        Dim purity As Double = Nothing
        Dim wt As Double = Nothing
        strSql = " SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = "
        strSql += " (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbCategory_MAN.Text & "')"
        dtTemp.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTemp)
        If dtTemp.Rows.Count > 0 Then
            purity = Val(dtTemp.Rows(0).Item("Purity").ToString)
        End If
        wt = (purity / 100) * grswt
        If wt <> 0 Then
            Return Format(wt, "0.000")
        End If
        Return ""
    End Function
    Function funcAddNew() As Integer
        Dim purity As Double = Nothing
        Dim catCode As String = Nothing
        Dim acCode As String = Nothing
        Dim ITEMID As Integer = Nothing
        Dim SUBITEMID As Integer = Nothing

        ''FIND PURITY
        strSql = " select PURITY from " & CNADMINDB & "..puritymast WHERE PURITYID = (SELECT PURITYID FROM " & CNADMINDB & "..CATEGORY WHERE CATNAME = '" & cmbCategory_MAN.Text & "')"
        dtTemp.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTemp)
        If dtTemp.Rows.Count > 0 Then
            purity = Val(dtTemp.Rows(0).Item("PURITY").ToString)
        End If

        ''find CatCode
        strSql = " SELECT CATCODE FROM " & CNADMINDB & "..CATEGORY WHERE CATNAME = '" & cmbCategory_MAN.Text & "'"
        dtTemp.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTemp)
        If dtTemp.Rows.Count > 0 Then
            catCode = dtTemp.Rows(0).Item("CATCODE").ToString
        End If

        ''find AcCode
        strSql = " SELECT ACCODE FROM " & CNADMINDB & "..ACHEAD WHERE ACNAME = '" & cmbSupplier_MAN.Text & "'"
        dtTemp.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTemp)
        If dtTemp.Rows.Count > 0 Then
            acCode = dtTemp.Rows(0).Item("ACCODE").ToString
        End If

        ''FIND ITEMID
        If cmbItem_MAN.Enabled Then
            strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"
            dtTemp.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTemp)
            If dtTemp.Rows.Count > 0 Then
                ITEMID = dtTemp.Rows(0).Item("ITEMID").ToString
            End If
        End If

        ''FIND SUBITEMID

        If cmbSubItem_MAN.Enabled Then
            strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_MAN.Text & "'"
            strSql += " AND ITEMID = " & ITEMID & ""
            dtTemp.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTemp)
            If dtTemp.Rows.Count > 0 Then
                SUBITEMID = dtTemp.Rows(0).Item("SUBITEMID").ToString
            End If
        End If
        Dim opCostid As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'")
        strSql = "select 1 from " & cnStockDb & "..OPENWEIGHT "
        strSql += "  WHERE STOCKTYPE = '" & IIf(rbtCompany.Checked = True, "C", "S") & "'"
        strSql += "  AND TRANTYPE = '" & IIf(rbtIssue.Checked = True, "I", "R") & "'"
        strSql += "  AND COMPANYID = '" & strCompanyId & "'"
        strSql += "  AND ISNULL(COSTID,'') = '" & opCostid & "'"
        strSql += "  AND ISNULL(ACCODE,'') = '" & acCode & "'"
        strSql += "  AND CATCODE = '" & catCode & "'"
        strSql += "  AND ISNULL(ITEMID,0) = " & ITEMID & "" 'ITEMID
        strSql += "  AND ISNULL(SUBITEMID,0) = " & SUBITEMID & "" 'SUBITEMID
        If Val(txtAppGrsWeight_WET.Text) <> 0 Or Val(txtAppNetWeight_WET.Text) Then strSql += " AND APPROVAL='A' "
        dtTemp.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTemp)
        If dtTemp.Rows.Count > 0 Then
            MsgBox("This is Duplicate entries. If you change do modified. ", MsgBoxStyle.Critical)
            Exit Function
        End If
        Try
            If Val(txtGrsWeight_WET.Text) <> 0 Or Val(txtNetWeight_WET.Text) <> 0 Or Val(txtPcs_NUM.Text) <> 0 Then
                strSql = " INSERT INTO " & cnStockDb & "..OPENWEIGHT"
                strSql += " ("
                strSql += " SNO,STOCKTYPE,TRANTYPE,PCS,GRSWT,NETWT,PUREWT,ITEMID,SUBITEMID"
                strSql += " ,CALCMODE,RATE,AMOUNT,COSTID,COMPANYID,PURITY,CATCODE,ACCODE"
                strSql += " ,REMARK1,USERID,UPDATED,UPTIME,SYSTEMID,STONEUNIT,APPVER"
                strSql += " )"
                strSql += " VALUES"
                strSql += " ("
                strSql += " '" & GetNewSno(TranSnoType.OPENWEIGHTCODE, tran) & "'" ''SNO
                strSql += " ,'" & IIf(rbtCompany.Checked = True, "C", "S") & "'" 'STOCKTYPE
                strSql += " ,'" & IIf(rbtIssue.Checked = True, "I", "R") & "'" 'TRANTYPE
                strSql += " ," & Val(txtPcs_NUM.Text) & "" 'PCS
                strSql += " ," & Val(txtGrsWeight_WET.Text) & "" 'GRSWT
                strSql += " ," & Val(txtNetWeight_WET.Text) & "" 'NETWT
                strSql += " ," & Val(txtPureWeight_WET.Text) & "" 'PUREWT
                strSql += " ," & ITEMID & "" 'ITEMID
                strSql += " ," & SUBITEMID & "" 'SUBITEMID
                strSql += " ,'" & IIf(rbtPiece.Checked = True, "P", "W") & "'" 'CALCMODE
                strSql += " ," & Val(txtRate_AMT.Text) & "" 'RATE
                strSql += " ," & Val(txtAmount_AMT.Text) & "" 'AMOUNT
                strSql += " ,'" & opCostid & "'" 'COSTID
                strSql += " ,'" & strCompanyId & "'" 'COMPANYID
                strSql += " ," & purity & "" 'PURITY
                strSql += " ,'" & catCode & "'" 'CATCODE
                strSql += " ,'" & acCode & "'" 'ACCODE
                strSql += " ,'" & txtRemark.Text & "'" 'REMARK1
                strSql += " ," & userId & "" 'USERID
                strSql += " ,'" & Today.Date & "'" 'UPDATED
                strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                strSql += " ,'" & systemId & "'" 'SYSTEMID
                strSql += " ,'" & IIf(pnlStone.Visible = True, IIf(rbtCarat.Checked = True, "C", "G"), "") & "'" 'STONEUNIT
                strSql += " ,'" & VERSION & "'"
                strSql += " )"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'"))
            End If
            If Val(txtAppGrsWeight_WET.Text) <> 0 Or Val(txtAppNetWeight_WET.Text) <> 0 Then
                strSql = " INSERT INTO " & cnStockDb & "..OPENWEIGHT"
                strSql += " ("
                strSql += " SNO,STOCKTYPE,TRANTYPE,PCS,GRSWT,NETWT,PUREWT,ITEMID,SUBITEMID"
                strSql += " ,CALCMODE,RATE,AMOUNT,COSTID,COMPANYID,PURITY,CATCODE,ACCODE"
                strSql += " ,REMARK1,USERID,UPDATED,UPTIME,SYSTEMID,STONEUNIT,APPVER,APPROVAL"
                strSql += " )"
                strSql += " VALUES"
                strSql += " ("
                strSql += " '" & GetNewSno(TranSnoType.OPENWEIGHTCODE, tran) & "'" ''SNO
                strSql += " ,'" & IIf(rbtCompany.Checked = True, "C", "S") & "'" 'STOCKTYPE
                strSql += " ,'" & IIf(rbtIssue.Checked = True, "I", "R") & "'" 'TRANTYPE
                strSql += " ," & Val(txtPcs_NUM.Text) & "" 'PCS
                strSql += " ," & Val(txtAppGrsWeight_WET.Text) & "" 'APPROVAL GRSWT
                strSql += " ," & Val(txtAppNetWeight_WET.Text) & "" 'APPROVAL NETWT
                strSql += " ," & Val(txtAppPureWeight_WET.Text) & "" 'APPROVAL PUREWT
                strSql += " ," & ITEMID & "" 'ITEMID
                strSql += " ," & SUBITEMID & "" 'SUBITEMID
                strSql += " ,'" & IIf(rbtPiece.Checked = True, "P", "W") & "'" 'CALCMODE
                strSql += " ," & Val(txtRate_AMT.Text) & "" 'RATE
                strSql += " ," & Val(txtAmount_AMT.Text) & "" 'AMOUNT
                strSql += " ,'" & opCostid & "'" 'COSTID
                strSql += " ,'" & strCompanyId & "'" 'COMPANYID
                strSql += " ," & purity & "" 'PURITY
                strSql += " ,'" & catCode & "'" 'CATCODE
                strSql += " ,'" & acCode & "'" 'ACCODE
                strSql += " ,'" & txtRemark.Text & "'" 'REMARK1
                strSql += " ," & userId & "" 'USERID
                strSql += " ,'" & Today.Date & "'" 'UPDATED
                strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                strSql += " ,'" & systemId & "'" 'SYSTEMID
                strSql += " ,'" & IIf(pnlStone.Visible = True, IIf(rbtCarat.Checked = True, "C", "G"), "") & "'" 'STONEUNIT
                strSql += " ,'" & VERSION & "'" 'APPVERSION
                strSql += " ,'A'" 'APPROVAL
                strSql += " )"
                ExecQuery(SyncMode.Transaction, strSql, cn, tran, objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'"))
            End If

            funcNew()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function
    Function funcUpdate() As Integer
        Dim purity As Double = Nothing
        Dim catCode As String = Nothing
        Dim acCode As String = Nothing
        Dim ITEMID As Integer = Nothing
        Dim SUBITEMID As Integer = Nothing

        ''FIND PURITY
        strSql = " select PURITY from " & cnAdminDb & "..puritymast WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbCategory_MAN.Text & "')"
        dtTemp.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTemp)
        If dtTemp.Rows.Count > 0 Then
            purity = Val(dtTemp.Rows(0).Item("PURITY").ToString)
        End If

        ''find CatCode
        strSql = " SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbCategory_MAN.Text & "'"
        dtTemp.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTemp)
        If dtTemp.Rows.Count > 0 Then
            catCode = dtTemp.Rows(0).Item("CATCODE").ToString
        End If

        ''find AcCode
        strSql = " SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbSupplier_MAN.Text & "'"
        dtTemp.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTemp)
        If dtTemp.Rows.Count > 0 Then
            acCode = dtTemp.Rows(0).Item("ACCODE").ToString
        End If

        ''FIND ITEMID
        strSql = " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "'"
        dtTemp.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTemp)
        If dtTemp.Rows.Count > 0 Then
            ITEMID = dtTemp.Rows(0).Item("ITEMID").ToString
        End If

        ''FIND SUBITEMID
        strSql = " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItem_MAN.Text & "' AND ITEMID = " & ITEMID & ""
        dtTemp.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTemp)
        If dtTemp.Rows.Count > 0 Then
            SUBITEMID = dtTemp.Rows(0).Item("SUBITEMID").ToString
        End If
        If Val(txtGrsWeight_WET.Text) <> 0 Or Val(txtNetWeight_WET.Text) <> 0 Or Val(txtPcs_NUM.Text) <> 0 Then
            strSql = " UPDATE " & cnStockDb & "..OPENWEIGHT SET "
            strSql += " STOCKTYPE = '" & IIf(rbtCompany.Checked = True, "C", "S") & "'" 'STOCKTYPE
            strSql += " ,TRANTYPE = '" & IIf(rbtIssue.Checked = True, "I", "R") & "'" 'TRANTYPE
            strSql += " ,PCS = " & Val(txtPcs_NUM.Text) & "" 'PCS
            strSql += " ,GRSWT = " & Val(txtGrsWeight_WET.Text) & "" 'GRSWT
            strSql += " ,NETWT = " & Val(txtNetWeight_WET.Text) & "" 'NETWT
            strSql += " ,PUREWT = " & Val(txtPureWeight_WET.Text) & "" 'PUREWT
            strSql += " ,ITEMID = " & ITEMID & "" 'ITEMID
            strSql += " ,SUBITEMID = " & SUBITEMID & "" 'SUBITEMID
            strSql += " ,CALCMODE = '" & IIf(rbtPiece.Checked = True, "P", "W") & "'" 'CALCMODE
            strSql += " ,RATE = " & Val(txtRate_AMT.Text) & "" 'RATE
            strSql += " ,AMOUNT = " & Val(txtAmount_AMT.Text) & "" 'AMOUNT
            strSql += " ,COSTID = '" & objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'") & "'" 'COSTID
            strSql += " ,PURITY = " & purity & "" 'PURITY
            strSql += " ,CATCODE = '" & catCode & "'" 'CATCODE
            strSql += " ,ACCODE = '" & acCode & "'" 'ACCODE
            strSql += " ,REMARK1 = '" & txtRemark.Text & "'" 'REMARK1
            strSql += " ,USERID = " & userId & "" 'USERID
            strSql += " ,UPDATED = '" & Today.Date & "'" 'UPDATED
            strSql += " ,UPTIME = '" & Date.Now.ToLongTimeString & "'" 'UPTIME
            strSql += " ,SYSTEMID = '" & systemId & "'" 'SYSTEMID
            strSql += " ,STONEUNIT = '" & IIf(pnlStone.Visible = True, IIf(rbtCarat.Checked = True, "C", "G"), "") & "'" 'STONEUNIT
            strSql += " ,APPVER = '" & VERSION & "'" 'APPVER
            strSql += " WHERE SNO = '" & Sno & "'"
            strSql += " AND ISNULL(COMPANYID,'') = '" & strCompanyId & "'" 'COMPANYID
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'"))
        End If

        If Val(txtAppGrsWeight_WET.Text) <> 0 Or Val(txtAppNetWeight_WET.Text) <> 0 Then

            strSql = " UPDATE " & cnStockDb & "..OPENWEIGHT SET "
            strSql += " STOCKTYPE = '" & IIf(rbtCompany.Checked = True, "C", "S") & "'" 'STOCKTYPE
            strSql += " ,TRANTYPE = '" & IIf(rbtIssue.Checked = True, "I", "R") & "'" 'TRANTYPE
            strSql += " ,PCS = " & Val(txtPcs_NUM.Text) & "" 'PCS
            strSql += " ,GRSWT = " & Val(txtAppGrsWeight_WET.Text) & "" 'APPROVAL GRSWT
            strSql += " ,NETWT = " & Val(txtAppNetWeight_WET.Text) & "" 'APPROVAL NETWT
            strSql += " ,PUREWT = " & Val(txtAppPureWeight_WET.Text) & "" 'APPROVAL PUREWT
            strSql += " ,ITEMID = " & ITEMID & "" 'ITEMID
            strSql += " ,SUBITEMID = " & SUBITEMID & "" 'SUBITEMID
            strSql += " ,CALCMODE = '" & IIf(rbtPiece.Checked = True, "P", "W") & "'" 'CALCMODE
            strSql += " ,RATE = " & Val(txtRate_AMT.Text) & "" 'RATE
            strSql += " ,AMOUNT = " & Val(txtAmount_AMT.Text) & "" 'AMOUNT
            strSql += " ,COSTID = '" & objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'") & "'" 'COSTID
            strSql += " ,PURITY = " & purity & "" 'PURITY
            strSql += " ,CATCODE = '" & catCode & "'" 'CATCODE
            strSql += " ,ACCODE = '" & acCode & "'" 'ACCODE
            strSql += " ,REMARK1 = '" & txtRemark.Text & "'" 'REMARK1
            strSql += " ,USERID = " & userId & "" 'USERID
            strSql += " ,UPDATED = '" & Today.Date & "'" 'UPDATED
            strSql += " ,UPTIME = '" & Date.Now.ToLongTimeString & "'" 'UPTIME
            strSql += " ,SYSTEMID = '" & systemId & "'" 'SYSTEMID
            strSql += " ,STONEUNIT = '" & IIf(pnlStone.Visible = True, IIf(rbtCarat.Checked = True, "C", "G"), "") & "'" 'STONEUNIT
            strSql += " ,APPVER = '" & VERSION & "'" 'APPVER
            strSql += " ,APPROVAL = 'A'" 'APPROVAL
            strSql += " WHERE SNO = '" & Sno & "'"
            strSql += " AND ISNULL(COMPANYID,'') = '" & strCompanyId & "'" 'COMPANYID
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'"))

        End If

        funcNew()
    End Function
    Function funcSave() As Integer
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Function
        If objGPack.Validator_Check(grpMain) Then
            Exit Function
        End If
        If rbtCompany.Checked = True Then
            If cmbCategory_MAN.Text = "" Then
                MsgBox("Category Should Not Empty", MsgBoxStyle.Information)
                cmbCategory_MAN.Focus()
                Exit Function
            End If
            If cmbItem_MAN.Enabled = True Then
                If cmbItem_MAN.Text = "" Then
                    MsgBox("Item Should Not Empty", MsgBoxStyle.Information)
                    cmbItem_MAN.Focus()
                    Exit Function
                End If
            End If
            If rbtPiece.Checked And Val(txtPcs_NUM.Text) = 0 Then
                MsgBox("Piece Should not Empty", MsgBoxStyle.Information)
                txtPcs_NUM.Focus()
                Exit Function
            End If
            If Val(txtAppGrsWeight_WET.Text) = 0 Then
                If rbtWeight.Checked And Val(txtGrsWeight_WET.Text) = 0 Then
                    MsgBox("GrsWt Should not Empty", MsgBoxStyle.Information)
                    txtGrsWeight_WET.Focus()
                    Exit Function
                End If
                If Val(txtPcs_NUM.Text) = 0 And Val(txtGrsWeight_WET.Text) = 0 Then
                    MsgBox("Piece Or Weight Should Not Empty", MsgBoxStyle.Information)
                    txtPcs_NUM.Focus()
                    Exit Function
                End If
            End If
        Else
            If cmbSupplier_MAN.Text = "" Then
                MsgBox("Supplier Should Not Empty", MsgBoxStyle.Information)
                cmbSupplier_MAN.Focus()
                Exit Function
            End If
            If cmbCategory_MAN.Text = "" Then
                MsgBox("Category Should Not Empty", MsgBoxStyle.Information)
                cmbCategory_MAN.Focus()
                Exit Function
            End If
            If Val(txtAppGrsWeight_WET.Text) = 0 Then
                If rbtPiece.Checked And Val(txtPcs_NUM.Text) = 0 Then
                    MsgBox("Piece Should not Empty", MsgBoxStyle.Information)
                    txtPcs_NUM.Focus()
                    Exit Function
                End If
                If rbtWeight.Checked And Val(txtGrsWeight_WET.Text) = 0 Then
                    MsgBox("GrsWt Should not Empty", MsgBoxStyle.Information)
                    txtGrsWeight_WET.Focus()
                    Exit Function
                End If
                If Val(txtPcs_NUM.Text) = 0 And Val(txtGrsWeight_WET.Text) = 0 Then
                    MsgBox("Piece Or Weight Should Not Empty", MsgBoxStyle.Information)
                    txtPcs_NUM.Focus()
                    Exit Function
                End If
            End If
            End If

            If editFlag = False Then
                funcAddNew()
            End If
            If editFlag = True Then
                funcUpdate()
            End If
    End Function
    Function funcNew() As Integer
        Sno = Nothing
        editFlag = False
        objGPack.TextClear(Me)
        pnlIssueReceipt.Enabled = True
        pnlStone.Visible = False
        rbtCompany.Checked = True
        rbtReceipt.Checked = True
        rbtWeight.Checked = True
        rbtGram.Checked = True
        cmbItem_MAN.Enabled = True
        cmbSubItem_MAN.Enabled = True
        cmbSupplier_MAN.Text = ""
        cmbMetal_MAN.Text = ""
        cmbCategory_MAN.Text = ""
        cmbItem_MAN.Text = ""
        cmbSubItem_MAN.Text = ""
        If cmbCostCentre_MAN.Enabled = True Then
            cmbCostCentre_MAN.Focus()
        Else
            Me.SelectNextControl(cmbCostCentre_MAN, False, False, False, False)
        End If
    End Function
    Private Sub frmOpeningWeightEntry_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Escape) Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                tabMain.SelectedTab = tabGeneral
            End If
        ElseIf e.KeyChar = Chr(Keys.Enter) Then
            If gridView.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmOpeningWeightEntry_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        ''Create dtOpenView Table
        strSql = " SELECT"
        strSql += " (SELECT ACNAME FROM " & CNADMINDB & "..ACHEAD WHERE ACCODE = O.ACCODE)AS ACNAME"
        strSql += " ,(SELECT CATNAME FROM " & CNADMINDB & "..CATEGORY WHERE CATCODE = O.CATCODE)AS CATNAME"
        strSql += " ,(SELECT ITEMNAME FROM " & CNADMINDB & "..ITEMMAST WHERE ITEMID = O.ITEMID)AS ITEMNAME"
        strSql += " ,(SELECT SUBITEMNAME FROM " & CNADMINDB & "..SUBITEMMAST WHERE SUBITEMID = O.SUBITEMID)AS SUBITEMNAME"
        strSql += " ,PCS,GRSWT,NETWT,AMOUNT,PUREWT,RATE,TRANTYPE,CALCMODE,STONEUNIT"

        strSql += " ,SNO,STOCKTYPE,ITEMID,SUBITEMID,COSTID"
        strSql += " ,COMPANYID,PURITY,CATCODE,ACCODE,REMARK1,SYSTEMID"
        strSql += " FROM " & CNSTOCKDB & "..OPENWEIGHT AS O"
        strSql += " WHERE 1<>1"
        dtOpenView.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtOpenView)
        gridView.DataSource = dtOpenView
        dtOpenView.Rows.Clear()
        funcOpenViewStyle()

        ''LOAD SUPPLIER
        strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
        strSql += " WHERE ACTYPE <> 'O'"
        strSql += GetAcNameQryFilteration()
        strSql += " ORDER BY ACNAME"
        objGPack.FillCombo(strSql, cmbSupplier_MAN)
        cmbSupplier_MAN.Text = ""

        ''Load Metal
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE ISNULL(ACTIVE,'') <> 'N' ORDER BY METALNAME"
        objGPack.FillCombo(strSql, cmbMetal_MAN)
        cmbMetal_MAN.Text = ""

        ''Load Metal
        cmbOpenMetal.Items.Add("ALL")
        strSql = " SELECT METALNAME FROM " & CNADMINDB & "..METALMAST ORDER BY METALNAME"
        objGPack.FillCombo(strSql, cmbOpenMetal, False)
        cmbOpenMetal.Text = ""

        If objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE'") = "Y" Then
            cmbCostCentre_MAN.Items.Clear()
            strSql = "SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE ISNULL(COMPANYID,'" & strCompanyId & "')LIKE'%" & strCompanyId & "%' ORDER BY COSTNAME"
            objGPack.FillCombo(strSql, cmbCostCentre_MAN, False)
            cmbCostCentre_MAN.Enabled = True
            cmbOpenCostCentre.Items.Clear()
            cmbOpenCostCentre.Items.Add("ALL")
            objGPack.FillCombo(strSql, cmbOpenCostCentre, False, False)
            cmbOpenCostCentre.Enabled = True
            cmbOpenCostCentre.Text = "ALL"
        Else
            cmbCostCentre_MAN.Enabled = False
            cmbOpenCostCentre.Enabled = False
        End If
        funcNew()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        tabMain.SelectedTab = tabView
        funcOpenClear()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        funcSave()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        btnSave_Click(Me, New EventArgs)
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

    Private Sub rbtIssue_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtIssue.CheckedChanged
        If rbtCompany.Checked = True Then
            rbtReceipt.Checked = True
        End If
    End Sub

    Private Sub rbtCompany_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtCompany.CheckedChanged
        If rbtCompany.Checked = True Then
            cmbSupplier_MAN.Text = ""
            cmbSupplier_MAN.Enabled = False
            rbtReceipt.Checked = True
            pnlIssueReceipt.Enabled = False
            cmbItem_MAN.Enabled = True
            cmbSubItem_MAN.Enabled = True
        Else
            cmbSupplier_MAN.Text = ""
            cmbSupplier_MAN.Enabled = True
            rbtReceipt.Checked = False
            rbtIssue.Checked = True
            pnlIssueReceipt.Enabled = True
            cmbItem_MAN.Enabled = False
            cmbSubItem_MAN.Enabled = False
        End If
    End Sub




    Private Sub txtGrsWeight_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGrsWeight_WET.LostFocus
        If Val(txtGrsWeight_WET.Text) <> 0 Then
            txtNetWeight_WET.Text = Format(Val(txtGrsWeight_WET.Text), "0.000")
            txtPureWeight_WET.Text = funcCalcPureWeight(Val(txtGrsWeight_WET.Text))
            txtAmount_AMT.Text = funcCalcAmount()
            txtNetWeight_WET.SelectAll()
        End If
    End Sub

    Private Sub txtPcs_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPcs_NUM.LostFocus
        txtAmount_AMT.Text = funcCalcAmount()
    End Sub

    Private Sub txtRate_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRate_AMT.LostFocus
        txtAmount_AMT.Text = funcCalcAmount()
    End Sub

    Private Sub rbtPiece_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtPiece.LostFocus
        txtAmount_AMT.Text = funcCalcAmount()
    End Sub

    Private Sub rbtWeight_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtWeight.LostFocus
        txtAmount_AMT.Text = funcCalcAmount()
    End Sub

    Private Sub rbtCompany_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtCompany.GotFocus
        rbtCompany_CheckedChanged(Me, New EventArgs)
    End Sub

    Private Sub rbtSmith_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtSmith.GotFocus
        rbtCompany_CheckedChanged(Me, New EventArgs)
    End Sub

    Private Sub btnOpenView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        strSql = " SELECT"
        strSql += " (SELECT ACNAME FROM " & CNADMINDB & "..ACHEAD WHERE ACCODE = O.ACCODE)AS ACNAME"
        strSql += " ,(SELECT CATNAME FROM " & CNADMINDB & "..CATEGORY WHERE CATCODE = O.CATCODE)AS CATNAME"
        strSql += " ,(SELECT ITEMNAME FROM " & CNADMINDB & "..ITEMMAST WHERE ITEMID = O.ITEMID)AS ITEMNAME"
        strSql += " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = O.SUBITEMID)AS SUBITEMNAME"
        strSql += " ,PCS,GRSWT,NETWT,AMOUNT,PUREWT,RATE"
        strSql += " ,CASE WHEN TRANTYPE = 'I' THEN 'ISSUE' ELSE 'RECEIPT' END TRANTYPE"
        strSql += " ,CASE WHEN CALCMODE = 'P' THEN 'PIECE' ELSE 'WEIGHT' END CALCMODE"
        strSql += " ,CASE WHEN STONEUNIT = 'C' THEN 'CARAT' "
        strSql += "       WHEN STONEUNIT = 'G' THEN 'GRAM' ELSE '' END STONEUNIT"
        strSql += " ,SNO,STOCKTYPE,ITEMID,SUBITEMID,COSTID"
        strSql += " ,COMPANYID,PURITY,CATCODE,ACCODE,REMARK1,SYSTEMID"
        strSql += " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = O.COSTID)AS COSTNAME"
        strSql += " ,ISNULL(APPROVAL,'') AS APPROVAL FROM " & cnStockDb & "..OPENWEIGHT AS O"
        strSql += " WHERE ISNULL(COMPANYID,'') = '" & strCompanyId & "'"
        If rbtOpenCompany.Checked = True Then
            strSql += " AND STOCKTYPE = 'C'"
        Else
            strSql += " AND STOCKTYPE = 'S'"
        End If
        If cmbOpenMetal.Text <> "ALL" And cmbOpenMetal.Text <> "" Then
            strSql += " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbOpenMetal.Text & "'))"
        End If
        If cmbOpenCostCentre.Text <> "ALL" And cmbOpenCostCentre.Text <> "" Then
            strSql += " AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbOpenCostCentre.Text & "')"
        End If
        strSql += " UNION ALL"
        strSql += " SELECT 'TOTAL' AS ACNAME,''AS CATNAME,'' AS ITEMNAME,'' AS SUBITEMNAME"
        strSql += " ,SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(AMOUNT),SUM(PUREWT),NULL AS RATE"
        strSql += " ,'' AS TRANTYPE,'' AS CALCMODE,'' AS STONEUNIT"
        strSql += " ,'' SNO,'' STOCKTYPE,0 ITEMID,0 SUBITEMID,'' COSTID"
        strSql += " ,'' COMPANYID,0 PURITY,'' CATCODE,'' ACCODE,'' REMARK1,'' SYSTEMID"
        strSql += " ,'' AS COSTNAME,''APPROVAL "
        strSql += " FROM " & cnStockDb & "..OPENWEIGHT AS O"
        strSql += " WHERE ISNULL(COMPANYID,'') = '" & strCompanyId & "'"
        If rbtOpenCompany.Checked = True Then
            strSql += " AND STOCKTYPE = 'C'"
        Else
            strSql += " AND STOCKTYPE = 'S'"
        End If
        If cmbOpenMetal.Text <> "ALL" And cmbOpenMetal.Text <> "" Then
            strSql += " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbOpenMetal.Text & "'))"
        End If
        If cmbOpenCostCentre.Text <> "ALL" And cmbOpenCostCentre.Text <> "" Then
            strSql += " AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbOpenCostCentre.Text & "')"
        End If
        dtOpenView.Rows.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtOpenView)
        If dtOpenView.Rows.Count > 0 Then
            gridView.Focus()
        Else
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            cmbOpenMetal.Focus()
        End If
    End Sub

    Private Sub btnOpenClear_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        funcOpenClear()
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridView.RowCount > 0 Then
                gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells(0)
            End If
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Not gridView.RowCount > 0 Then Exit Sub
            funcGetDetails(gridView.CurrentRow.Index)
        End If
    End Sub

    Private Sub cmbMetal_MAN_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMetal_MAN.LostFocus
        strSql = " SELECT 1 FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal_MAN.Text & "' AND METALID IN ('D','T') "
        dtTemp.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTemp)
        If dtTemp.Rows.Count > 0 Then
            pnlStone.Visible = True
        Else
            pnlStone.Visible = False
        End If
    End Sub

    Private Sub cmbMetal_MAN_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMetal_MAN.SelectedIndexChanged
        cmbCategory_MAN.Items.Clear()
        strSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
        If cmbMetal_MAN.Text <> "ALL" And cmbMetal_MAN.Text <> "" Then
            strSql += " WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal_MAN.Text & "') "
        End If
        strSql += " ORDER BY CATNAME"
        objGPack.FillCombo(strSql, cmbCategory_MAN, , False)
        If editFlag = False Then
            cmbCategory_MAN.Text = ""
        End If

        strSql = " SELECT 1 FROM " & cnAdminDb & "..METALMAST WHERE METALNAME = '" & cmbMetal_MAN.Text & "' AND METALID IN ('D','T') "
        dtTemp.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTemp)
        If dtTemp.Rows.Count > 0 Then
            pnlStone.Visible = True
        Else
            pnlStone.Visible = False
        End If
    End Sub

    Private Sub cmbCategory_MAN_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCategory_MAN.SelectedIndexChanged
        cmbItem_MAN.Items.Clear()
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE 1=1"
        If cmbCategory_MAN.Text <> "ALL" And cmbCategory_MAN.Text <> "" Then
            strSql += " AND CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbCategory_MAN.Text & "')"
        End If
        strSql += GetItemQryFilteration()
        strSql += " ORDER BY ITEMNAME"
        objGPack.FillCombo(strSql, cmbItem_MAN, , False)
        If editFlag = False Then
            cmbItem_MAN.Text = ""
        End If

        strSql = " SELECT METALTYPE FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbCategory_MAN.Text & "')"
        dtTemp.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTemp)
        If dtTemp.Rows.Count > 0 Then
            If StrComp(dtTemp.Rows(0).Item("METALTYPE").ToString, "M", CompareMethod.Text) = 0 Then
                cmbItem_MAN.Text = ""
                cmbSubItem_MAN.Text = ""
                cmbItem_MAN.Enabled = False
                cmbSubItem_MAN.Enabled = False
            Else
                cmbItem_MAN.Enabled = True
                cmbSubItem_MAN.Enabled = True
            End If
        Else
            cmbItem_MAN.Enabled = True
            cmbSubItem_MAN.Enabled = True
        End If

        strSql = " SELECT 1 FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbCategory_MAN.Text & "') AND METALID IN ('D','T') "
        dtTemp.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTemp)
        If dtTemp.Rows.Count > 0 Then
            pnlStone.Visible = True
        Else
            pnlStone.Visible = False
        End If

        txtPureWeight_WET.Text = funcCalcPureWeight(Val(txtGrsWeight_WET.Text))
        txtRate_AMT.Text = GetRate(GetEntryDate(GetServerDate), objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbCategory_MAN.Text & "'"))
    End Sub

    Private Sub cmbItem_MAN_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItem_MAN.SelectedIndexChanged
        cmbSubItem_MAN.Items.Clear()
        strSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST "
        If cmbItem_MAN.Text <> "ALL" And cmbItem_MAN.Text <> "" Then
            strSql += " WHERE ITEMID = ( SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem_MAN.Text & "' AND SUBITEM = 'Y') ORDER BY SUBITEMNAME"
        End If
        objGPack.FillCombo(strSql, cmbSubItem_MAN, , False)
        If cmbSubItem_MAN.Items.Count > 0 Then
            cmbSubItem_MAN.Enabled = True
        Else
            cmbSubItem_MAN.Enabled = False
        End If
        If editFlag = False Then
            cmbSubItem_MAN.Text = ""
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        If Not gridView.RowCount > 0 Then Exit Sub
        Try
            tran = Nothing
            tran = cn.BeginTransaction
            strSql = " DELETE FROM " & cnStockDb & "..OPENWEIGHT"
            strSql += " WHERE SNO = '" & gridView.CurrentRow.Cells("SNO").Value.ToString & "'"
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, objGPack.GetSqlValue("SELECT COSTID FROM " & cnStockDb & "..OPENWEIGHT WHERE SNO = '" & gridView.CurrentRow.Cells("SNO").Value.ToString & "'", , , tran))
            tran.Commit()
            MsgBox("Deleted Successfully", MsgBoxStyle.Information)
            btnOpenView_Click(Me, New EventArgs)
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub


    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "Opening Weight", gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "Opening Weight", gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub txtAppGrsWeight_WET_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAppGrsWeight_WET.LostFocus
        If Val(txtAppGrsWeight_WET.Text) <> 0 Then
            txtAppNetWeight_WET.Text = Format(Val(txtAppGrsWeight_WET.Text), "0.000")
            txtAppPureWeight_WET.Text = funcCalcPureWeight(Val(txtAppGrsWeight_WET.Text))
            txtNetWeight_WET.SelectAll()
        End If
    End Sub
End Class