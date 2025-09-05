Imports System.Data.OleDb
Imports System.IO
Public Class frmRearrangeTranNo
    Dim StrSql As String
    Dim Da As OleDbDataAdapter
    Dim Cmd As OleDbCommand
    Dim Syncdb As String = cnAdminDb

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub frmRearrangeTranNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmRearrangeTranNo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim uprefix As String = Mid(cnAdminDb, 1, 3)
        If GetAdmindbSoftValue("SYNC-SEPDATADB", "N", tran) = "Y" Then
            Dim usuffix As String = "UTILDB"
            If DbChecker(uprefix + usuffix) <> 0 Then Syncdb = uprefix + usuffix
        End If

        StrSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE "
        StrSql += " WHERE ISNULL(ACTIVE,'') <> 'N' "
        objGPack.FillCombo(StrSql, cmbCostName_MAN, True)
        If cmbCostName_MAN.Items.Count > 0 Then
            cmbCostName_MAN.SelectedIndex = 0
            cmbCostName_MAN.Enabled = True
        Else
            cmbCostName_MAN.Enabled = False
        End If
        StrSql = " SELECT COMPANYNAME FROM " & cnAdminDb & "..COMPANY"
        'StrSql += " WHERE ISNULL(ACTIVE,'') <> 'N' "
        objGPack.FillCombo(StrSql, CmbCompany_MAN, True)

        cmbOption_MAN.Items.Clear()
        cmbOption_MAN.Items.Add("MATERIAL RECEIPT")
        cmbOption_MAN.Items.Add("MATERIAL ISSUE")
        cmbOption_MAN.Items.Add("ACCOUNTS ENTRY")
        cmbOption_MAN.Text = ""

        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
    End Sub


    Private Sub btnGenerate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        If cmbOption_MAN.Text.Trim = "" Then MsgBox("Select any one Option.", MsgBoxStyle.Information) : cmbOption_MAN.Focus() : Exit Sub
        Dim mCompId As String = objGPack.GetSqlValue(" SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & CmbCompany_MAN.Text.Trim & "'")
        Dim mCostId As String = objGPack.GetSqlValue(" SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostName_MAN.Text.Trim & "'")
        Try
            If MessageBox.Show("Kindly Backup Current Database" + vbCrLf + "Sure you want to proceed?", "Re Arrange Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> Windows.Forms.DialogResult.Yes Then
                Exit Sub
            End If
            Dim objSecret As New frmAdminPassword()
            If objSecret.ShowDialog <> Windows.Forms.DialogResult.OK Then
                Exit Sub
            End If
            btnGenerate.Enabled = False
            tran = Nothing
            tran = cn.BeginTransaction()

            If cmbOption_MAN.Text = "ACCOUNTS ENTRY" Then
                AccountsEnt(mCompId, mCostId)
            ElseIf cmbOption_MAN.Text = "MATERIAL RECEIPT" Then
                MaterialReceipt(mCompId, mCostId)
            ElseIf cmbOption_MAN.Text = "MATERIAL ISSUE" Then
                MaterialISSUE(mCompId, mCostId)
            End If

            tran.Commit()
            MsgBox("Re arrange Completed for " & cmbOption_MAN.Text, MsgBoxStyle.Information)
            If cmbCostName_MAN.Enabled = True Then cmbCostName_MAN.SelectedIndex = 0
            dtpFrom.Value = GetServerDate()
            dtpTo.Value = GetServerDate()
            btnGenerate.Enabled = True
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            btnGenerate.Enabled = True
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Private Function AccountsEnt(ByVal mCompId As String, ByVal mCostId As String)

        StrSql = "SELECT DISTINCT PAYMODE FROM " & cnAdminDb & "..ACCENTRYMASTER "
        Dim DtPaymode As New DataTable
        Cmd = New OleDbCommand(StrSql, cn, tran)
        Da = New OleDbDataAdapter(Cmd)
        Da.Fill(DtPaymode)
        For j As Integer = 0 To DtPaymode.Rows.Count - 1
            Dim BillCtrlId As String = "GEN-" & DtPaymode.Rows(j).Item("PAYMODE").ToString
            Dim BILLNO As Integer = 0
            StrSql = " SELECT ISNULL(MAX(TRANNO),0) TRANNO FROM  " & cnStockDb & "..ACCTRAN  "
            StrSql += vbCrLf + " WHERE TRANDATE<'" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND ISNULL(CANCEL,'')=''"
            StrSql += vbCrLf + " AND PAYMODE='" & DtPaymode.Rows(j).Item("PAYMODE").ToString & "'  AND ISNULL(CANCEL,'')=''"
            If mCompId <> "" Then StrSql += vbCrLf + " AND COMPANYID='" & mCompId & "' "
            If mCostId <> "" Then StrSql += vbCrLf + " AND COSTID='" & mCostId & "' "
            BILLNO = Val(objGPack.GetSqlValue(StrSql, "", "", tran))

            StrSql = " SELECT DISTINCT TRANNO,TRANDATE,BATCHNO FROM " & cnStockDb & "..ACCTRAN "
            StrSql += vbCrLf + " WHERE PAYMODE='" & DtPaymode.Rows(j).Item("PAYMODE").ToString & "'  AND ISNULL(CANCEL,'')=''"
            StrSql += vbCrLf + " AND TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            If mCompId <> "" Then StrSql += vbCrLf + " AND COMPANYID='" & mCompId & "' "
            If mCostId <> "" Then StrSql += vbCrLf + " AND COSTID='" & mCostId & "' "
            StrSql += vbCrLf + " AND ISNULL(CANCEL,'')='' ORDER BY TRANDATE,TRANNO"
            Dim DtBillDet As New DataTable
            Cmd = New OleDbCommand(StrSql, cn, tran)
            Da = New OleDbDataAdapter(Cmd)
            Da.Fill(DtBillDet)
            For z As Integer = 0 To DtBillDet.Rows.Count - 1
                BILLNO = BILLNO + 1
                StrSql = " UPDATE " & cnAdminDb & "..OUTSTANDING SET TRANNO='" & BILLNO & "' "
                StrSql += vbCrLf + " WHERE TRANNO='" & Val(DtBillDet.Rows(z).Item("TRANNO").ToString) & "' AND BATCHNO='" & DtBillDet.Rows(z).Item("BATCHNO").ToString & "'"
                StrSql += vbCrLf + " UPDATE " & cnStockDb & "..ACCTRAN SET TRANNO='" & BILLNO & "' "
                StrSql += vbCrLf + " WHERE TRANNO='" & Val(DtBillDet.Rows(z).Item("TRANNO").ToString) & "' AND BATCHNO='" & DtBillDet.Rows(z).Item("BATCHNO").ToString & "'"
                StrSql += vbCrLf + " UPDATE " & cnStockDb & "..ISSUE SET TRANNO='" & BILLNO & "' "
                StrSql += vbCrLf + " WHERE TRANNO='" & Val(DtBillDet.Rows(z).Item("TRANNO").ToString) & "' AND BATCHNO='" & DtBillDet.Rows(z).Item("BATCHNO").ToString & "'"
                StrSql += vbCrLf + " UPDATE " & cnStockDb & "..RECEIPT SET TRANNO='" & BILLNO & "' "
                StrSql += vbCrLf + " WHERE TRANNO='" & Val(DtBillDet.Rows(z).Item("TRANNO").ToString) & "' AND BATCHNO='" & DtBillDet.Rows(z).Item("BATCHNO").ToString & "'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Cmd.ExecuteNonQuery()
            Next
            If BILLNO > 0 Then
                StrSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT='" & BILLNO & "' WHERE CTLID='" & BillCtrlId & "' AND COMPANYID='" & mCompId & "'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Cmd.ExecuteNonQuery()
            End If
        Next
    End Function
    Private Function MaterialReceipt(ByVal mCompId As String, ByVal mCostId As String)

        Dim GType As String = ""
        Dim TypePur As String = ""
        Dim TypeInt As String = ""
        Dim Typepac As String = ""
        Dim Gbctrlid As String = "GEN-SM-REC"
        Dim Purctrlid As String = ""
        Dim Intctrlid As String = ""
        Dim Pacctrlid As String = ""

        If UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = 'GEN-SM-RECPUR' AND COMPANYID = '" & mCompId & "' AND ISNULL(COSTID,'') ='" & strBCostid & "'", , , tran)) = "Y" Then
            TypePur = "RPU"
            Purctrlid = "GEN-SM-RECPUR"
        Else
            GType += ",RPU"
        End If
        
        If UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = 'GEN-SM-INTREC' AND COMPANYID = '" & mCompId & "' AND ISNULL(COSTID,'') ='" & strBCostid & "'", , , tran)) = "Y" Then
            TypeInt = "RIN"
            Intctrlid = "GEN-SM-INTREC"
        Else
            GType += ",RIN"
        End If
        
        If UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = 'GEN-PMSEPERATENO' AND COMPANYID = '" & mCompId & "' AND ISNULL(COSTID,'') ='" & strBCostid & "'", , , tran)) = "Y" Then
            Typepac = "RPA"
            Pacctrlid = "GEN-PMRECEIPTNO"
        Else
            GType += ",RPA"
        End If

        GType += ",RRE,ROT,RAP"

        GType = Mid(GType, 2, Len(GType))

        'RECEIPT PURCHASE ONLY
        If TypePur <> "" Then
            Dim BILLNO As Integer = 0
            StrSql = " SELECT ISNULL(MAX(TRANNO),0) TRANNO FROM  " & cnStockDb & "..RECEIPT  "
            StrSql += vbCrLf + " WHERE TRANDATE <'" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + " AND TRANTYPE IN('" & TypePur & "') AND ISNULL(CANCEL,'')=''"
            If mCompId <> "" Then StrSql += " AND COMPANYID='" & mCompId & "'"
            If mCostId <> "" Then StrSql += " AND COSTID='" & mCostId & "'"
            BILLNO = Val(objGPack.GetSqlValue(StrSql, "", "", tran))

            StrSql = " SELECT DISTINCT TRANNO,TRANDATE,BATCHNO FROM " & cnStockDb & "..RECEIPT "
            StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + " AND TRANTYPE IN('" & TypePur & "') AND ISNULL(CANCEL,'')=''"
            If mCompId <> "" Then StrSql += " AND COMPANYID='" & mCompId & "'"
            If mCostId <> "" Then StrSql += " AND COSTID='" & mCostId & "'"
            StrSql += vbCrLf + "   ORDER BY TRANDATE,TRANNO"
            Dim DtBillDet As New DataTable
            Cmd = New OleDbCommand(StrSql, cn, tran)
            Da = New OleDbDataAdapter(Cmd)
            Da.Fill(DtBillDet)

            For z As Integer = 0 To DtBillDet.Rows.Count - 1
                BILLNO = BILLNO + 1
                StrSql = " UPDATE " & cnAdminDb & "..OUTSTANDING SET TRANNO='" & BILLNO & "' "
                StrSql += vbCrLf + " WHERE TRANNO='" & Val(DtBillDet.Rows(z).Item("TRANNO").ToString) & "' AND BATCHNO='" & DtBillDet.Rows(z).Item("BATCHNO").ToString & "'"
                StrSql += vbCrLf + " UPDATE " & cnStockDb & "..ACCTRAN SET TRANNO='" & BILLNO & "' "
                StrSql += vbCrLf + " WHERE TRANNO='" & Val(DtBillDet.Rows(z).Item("TRANNO").ToString) & "' AND BATCHNO='" & DtBillDet.Rows(z).Item("BATCHNO").ToString & "'"
                StrSql += vbCrLf + " UPDATE " & cnStockDb & "..RECEIPTSTONE SET TRANNO='" & BILLNO & "' "
                StrSql += vbCrLf + " WHERE ISSSNO IN(SELECT SNO FROM  " & cnStockDb & "..RECEIPT WHERE TRANNO='" & DtBillDet.Rows(z).Item("TRANNO").ToString & "' AND BATCHNO='" & DtBillDet.Rows(z).Item("BATCHNO").ToString & "')"
                StrSql += vbCrLf + " UPDATE " & cnStockDb & "..RECEIPT SET TRANNO='" & BILLNO & "' "
                StrSql += vbCrLf + " WHERE TRANNO='" & Val(DtBillDet.Rows(z).Item("TRANNO").ToString) & "' AND BATCHNO='" & DtBillDet.Rows(z).Item("BATCHNO").ToString & "'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Cmd.ExecuteNonQuery()
            Next
            If BILLNO > 0 Then
                StrSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT='" & BILLNO & "' WHERE CTLID='" & Purctrlid & "' AND COMPANYID='" & mCompId & "'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Cmd.ExecuteNonQuery()
            End If
        End If

        'RECEIPT INERNAL ONLY
        If TypeInt <> "" Then
            Dim BILLNO As Integer = 0
            StrSql = " SELECT ISNULL(MAX(TRANNO),0) TRANNO FROM  " & cnStockDb & "..RECEIPT  "
            StrSql += vbCrLf + " WHERE TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + " AND TRANTYPE IN('" & TypeInt & "') AND ISNULL(CANCEL,'')=''"
            If mCompId <> "" Then StrSql += " AND COMPANYID='" & mCompId & "'"
            If mCostId <> "" Then StrSql += " AND COSTID='" & mCostId & "'"
            BILLNO = Val(objGPack.GetSqlValue(StrSql, "", "", tran))

            StrSql = " SELECT DISTINCT TRANNO,TRANDATE,BATCHNO FROM " & cnStockDb & "..RECEIPT "
            StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + " AND TRANTYPE IN('" & TypeInt & "') AND ISNULL(CANCEL,'')=''"
            If mCompId <> "" Then StrSql += " AND COMPANYID='" & mCompId & "'"
            If mCostId <> "" Then StrSql += " AND COSTID='" & mCostId & "'"
            StrSql += vbCrLf + "   ORDER BY TRANDATE,TRANNO"
            Dim DtBillDet As New DataTable
            Cmd = New OleDbCommand(StrSql, cn, tran)
            Da = New OleDbDataAdapter(Cmd)
            Da.Fill(DtBillDet)

            For z As Integer = 0 To DtBillDet.Rows.Count - 1
                BILLNO = BILLNO + 1
                StrSql = " UPDATE " & cnAdminDb & "..OUTSTANDING SET TRANNO='" & BILLNO & "' "
                StrSql += vbCrLf + " WHERE TRANNO='" & Val(DtBillDet.Rows(z).Item("TRANNO").ToString) & "' AND BATCHNO='" & DtBillDet.Rows(z).Item("BATCHNO").ToString & "'"
                StrSql += vbCrLf + " UPDATE " & cnStockDb & "..ACCTRAN SET TRANNO='" & BILLNO & "' "
                StrSql += vbCrLf + " WHERE TRANNO='" & Val(DtBillDet.Rows(z).Item("TRANNO").ToString) & "' AND BATCHNO='" & DtBillDet.Rows(z).Item("BATCHNO").ToString & "'"
                StrSql += vbCrLf + " UPDATE " & cnStockDb & "..RECEIPTSTONE SET TRANNO='" & BILLNO & "' "
                StrSql += vbCrLf + " WHERE ISSSNO IN(SELECT SNO FROM  " & cnStockDb & "..RECEIPT WHERE TRANNO='" & DtBillDet.Rows(z).Item("TRANNO").ToString & "' AND BATCHNO='" & DtBillDet.Rows(z).Item("BATCHNO").ToString & "')"
                StrSql += vbCrLf + " UPDATE " & cnStockDb & "..RECEIPT SET TRANNO='" & BILLNO & "' "
                StrSql += vbCrLf + " WHERE TRANNO='" & Val(DtBillDet.Rows(z).Item("TRANNO").ToString) & "' AND BATCHNO='" & DtBillDet.Rows(z).Item("BATCHNO").ToString & "'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Cmd.ExecuteNonQuery()
            Next
            If BILLNO > 0 Then
                StrSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT='" & BILLNO & "' WHERE CTLID='" & Intctrlid & "' AND COMPANYID='" & mCompId & "'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Cmd.ExecuteNonQuery()
            End If
        End If

        'RECEIPT PACKING ONLY
        If Typepac <> "" Then
            Dim BILLNO As Integer = 0
            StrSql = " SELECT ISNULL(MAX(TRANNO),0) TRANNO FROM  " & cnStockDb & "..RECEIPT  "
            StrSql += vbCrLf + " WHERE TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + " AND TRANTYPE IN('" & Typepac & "') AND ISNULL(CANCEL,'')=''"
            If mCompId <> "" Then StrSql += " AND COMPANYID='" & mCompId & "'"
            If mCostId <> "" Then StrSql += " AND COSTID='" & mCostId & "'"
            BILLNO = Val(objGPack.GetSqlValue(StrSql, "", "", tran))

            StrSql = " SELECT DISTINCT TRANNO,TRANDATE,BATCHNO FROM " & cnStockDb & "..RECEIPT "
            StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + " AND TRANTYPE IN('" & Typepac & "') AND ISNULL(CANCEL,'')=''"
            If mCompId <> "" Then StrSql += " AND COMPANYID='" & mCompId & "'"
            If mCostId <> "" Then StrSql += " AND COSTID='" & mCostId & "'"
            StrSql += vbCrLf + "   ORDER BY TRANDATE,TRANNO"
            Dim DtBillDet As New DataTable
            Cmd = New OleDbCommand(StrSql, cn, tran)
            Da = New OleDbDataAdapter(Cmd)
            Da.Fill(DtBillDet)

            For z As Integer = 0 To DtBillDet.Rows.Count - 1
                BILLNO = BILLNO + 1
                StrSql = " UPDATE " & cnAdminDb & "..OUTSTANDING SET TRANNO='" & BILLNO & "' "
                StrSql += vbCrLf + " WHERE TRANNO='" & Val(DtBillDet.Rows(z).Item("TRANNO").ToString) & "' AND BATCHNO='" & DtBillDet.Rows(z).Item("BATCHNO").ToString & "'"
                StrSql += vbCrLf + " UPDATE " & cnStockDb & "..ACCTRAN SET TRANNO='" & BILLNO & "' "
                StrSql += vbCrLf + " WHERE TRANNO='" & Val(DtBillDet.Rows(z).Item("TRANNO").ToString) & "' AND BATCHNO='" & DtBillDet.Rows(z).Item("BATCHNO").ToString & "'"
                StrSql += vbCrLf + " UPDATE " & cnStockDb & "..RECEIPTSTONE SET TRANNO='" & BILLNO & "' "
                StrSql += vbCrLf + " WHERE ISSSNO IN(SELECT SNO FROM  " & cnStockDb & "..RECEIPT WHERE TRANNO='" & DtBillDet.Rows(z).Item("TRANNO").ToString & "' AND BATCHNO='" & DtBillDet.Rows(z).Item("BATCHNO").ToString & "')"
                StrSql += vbCrLf + " UPDATE " & cnStockDb & "..RECEIPT SET TRANNO='" & BILLNO & "' "
                StrSql += vbCrLf + " WHERE TRANNO='" & Val(DtBillDet.Rows(z).Item("TRANNO").ToString) & "' AND BATCHNO='" & DtBillDet.Rows(z).Item("BATCHNO").ToString & "'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Cmd.ExecuteNonQuery()
            Next
            If BILLNO > 0 Then
                StrSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT='" & BILLNO & "' WHERE CTLID='" & Pacctrlid & "' AND COMPANYID='" & mCompId & "'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Cmd.ExecuteNonQuery()
            End If
        End If

        'RECEIPT ALL
        If GType <> "" Then
            Dim BILLNO As Integer = 0
            StrSql = " SELECT ISNULL(MAX(TRANNO),0) TRANNO FROM  " & cnStockDb & "..RECEIPT  "
            StrSql += vbCrLf + " WHERE TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + " AND TRANTYPE IN('" & GType.Replace(",", "','") & "') AND ISNULL(CANCEL,'')=''"
            If mCompId <> "" Then StrSql += " AND COMPANYID='" & mCompId & "'"
            If mCostId <> "" Then StrSql += " AND COSTID='" & mCostId & "'"
            BILLNO = Val(objGPack.GetSqlValue(StrSql, "", "", tran))

            StrSql = " SELECT DISTINCT TRANNO,TRANDATE,BATCHNO FROM " & cnStockDb & "..RECEIPT "
            StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + " AND TRANTYPE IN('" & GType.Replace(",", "','") & "') AND ISNULL(CANCEL,'')=''"
            If mCompId <> "" Then StrSql += " AND COMPANYID='" & mCompId & "'"
            If mCostId <> "" Then StrSql += " AND COSTID='" & mCostId & "'"
            StrSql += vbCrLf + "   ORDER BY TRANDATE,TRANNO"
            Dim DtBillDet As New DataTable
            Cmd = New OleDbCommand(StrSql, cn, tran)
            Da = New OleDbDataAdapter(Cmd)
            Da.Fill(DtBillDet)

            For z As Integer = 0 To DtBillDet.Rows.Count - 1
                BILLNO = BILLNO + 1
                StrSql = " UPDATE " & cnAdminDb & "..OUTSTANDING SET TRANNO='" & BILLNO & "' "
                StrSql += vbCrLf + " WHERE TRANNO='" & Val(DtBillDet.Rows(z).Item("TRANNO").ToString) & "' AND BATCHNO='" & DtBillDet.Rows(z).Item("BATCHNO").ToString & "'"
                StrSql += vbCrLf + " UPDATE " & cnStockDb & "..ACCTRAN SET TRANNO='" & BILLNO & "' "
                StrSql += vbCrLf + " WHERE TRANNO='" & Val(DtBillDet.Rows(z).Item("TRANNO").ToString) & "' AND BATCHNO='" & DtBillDet.Rows(z).Item("BATCHNO").ToString & "'"
                StrSql += vbCrLf + " UPDATE " & cnStockDb & "..RECEIPTSTONE SET TRANNO='" & BILLNO & "' "
                StrSql += vbCrLf + " WHERE ISSSNO IN(SELECT SNO FROM  " & cnStockDb & "..RECEIPT WHERE TRANNO='" & DtBillDet.Rows(z).Item("TRANNO").ToString & "' AND BATCHNO='" & DtBillDet.Rows(z).Item("BATCHNO").ToString & "')"
                StrSql += vbCrLf + " UPDATE " & cnStockDb & "..RECEIPT SET TRANNO='" & BILLNO & "' "
                StrSql += vbCrLf + " WHERE TRANNO='" & Val(DtBillDet.Rows(z).Item("TRANNO").ToString) & "' AND BATCHNO='" & DtBillDet.Rows(z).Item("BATCHNO").ToString & "'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Cmd.ExecuteNonQuery()
            Next
            If BILLNO > 0 Then
                StrSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT='" & BILLNO & "' WHERE CTLID='" & Gbctrlid & "' AND COMPANYID='" & mCompId & "'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Cmd.ExecuteNonQuery()
            End If
        End If

    End Function
    Private Function MaterialISSUE(ByVal mCompId As String, ByVal mCostId As String)

        Dim GType As String = ""
        Dim TypeInt As String = ""
        Dim Typepac As String = ""
        Dim Gbctrlid As String = "GEN-SM-ISS"
        Dim Intctrlid As String = ""
        Dim Pacctrlid As String = ""

        If UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = 'GEN-SM-INTISS' AND COMPANYID = '" & mCompId & "' AND ISNULL(COSTID,'') ='" & strBCostid & "'", , , tran)) = "Y" Then
            TypeInt = "IIN"
            Intctrlid = "GEN-SM-INTREC"
        Else
            GType += ",IIN"
        End If

        If UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = 'GEN-PMSEPERATENO' AND COMPANYID = '" & mCompId & "' AND ISNULL(COSTID,'') ='" & strBCostid & "'", , , tran)) = "Y" Then
            Typepac = "IPA"
            Pacctrlid = "GEN-PMISSUENO"
        Else
            GType += ",IPA"
        End If

        GType += ",IIS,IPU,IAP,IOT"
        GType = Mid(GType, 2, Len(GType))


        'ISSUE INERNAL ONLY
        If TypeInt <> "" Then
            Dim BILLNO As Integer = 0
            StrSql = " SELECT ISNULL(MAX(TRANNO),0) TRANNO FROM  " & cnStockDb & "..ISSUE "
            StrSql += vbCrLf + " WHERE TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + " AND TRANTYPE IN('" & TypeInt & "') AND ISNULL(CANCEL,'')=''"
            If mCompId <> "" Then StrSql += " AND COMPANYID='" & mCompId & "'"
            If mCostId <> "" Then StrSql += " AND COSTID='" & mCostId & "'"
            BILLNO = Val(objGPack.GetSqlValue(StrSql, "", "", tran))

            StrSql = " SELECT DISTINCT TRANNO,TRANDATE,BATCHNO FROM " & cnStockDb & "..ISSUE"
            StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + " AND TRANTYPE IN('" & TypeInt & "') AND ISNULL(CANCEL,'')=''"
            If mCompId <> "" Then StrSql += " AND COMPANYID='" & mCompId & "'"
            If mCostId <> "" Then StrSql += " AND COSTID='" & mCostId & "'"
            StrSql += vbCrLf + "   ORDER BY TRANDATE,TRANNO"
            Dim DtBillDet As New DataTable
            Cmd = New OleDbCommand(StrSql, cn, tran)
            Da = New OleDbDataAdapter(Cmd)
            Da.Fill(DtBillDet)

            For z As Integer = 0 To DtBillDet.Rows.Count - 1
                BILLNO = BILLNO + 1
                StrSql = " UPDATE " & cnAdminDb & "..OUTSTANDING SET TRANNO='" & BILLNO & "' "
                StrSql += vbCrLf + " WHERE TRANNO='" & Val(DtBillDet.Rows(z).Item("TRANNO").ToString) & "' AND BATCHNO='" & DtBillDet.Rows(z).Item("BATCHNO").ToString & "'"
                StrSql += vbCrLf + " UPDATE " & cnStockDb & "..ACCTRAN SET TRANNO='" & BILLNO & "' "
                StrSql += vbCrLf + " WHERE TRANNO='" & Val(DtBillDet.Rows(z).Item("TRANNO").ToString) & "' AND BATCHNO='" & DtBillDet.Rows(z).Item("BATCHNO").ToString & "'"
                StrSql += vbCrLf + " UPDATE " & cnStockDb & "..ISSSTONE SET TRANNO='" & BILLNO & "' "
                StrSql += vbCrLf + " WHERE ISSSNO IN(SELECT SNO FROM  " & cnStockDb & "..ISSUE WHERE TRANNO='" & DtBillDet.Rows(z).Item("TRANNO").ToString & "' AND BATCHNO='" & DtBillDet.Rows(z).Item("BATCHNO").ToString & "')"
                StrSql += vbCrLf + " UPDATE " & cnStockDb & "..ISSUE SET TRANNO='" & BILLNO & "' "
                StrSql += vbCrLf + " WHERE TRANNO='" & Val(DtBillDet.Rows(z).Item("TRANNO").ToString) & "' AND BATCHNO='" & DtBillDet.Rows(z).Item("BATCHNO").ToString & "'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Cmd.ExecuteNonQuery()
            Next
            If BILLNO > 0 Then
                StrSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT='" & BILLNO & "' WHERE CTLID='" & Intctrlid & "' AND COMPANYID='" & mCompId & "'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Cmd.ExecuteNonQuery()
            End If
        End If

        'ISSUE PACKING ONLY
        If Typepac <> "" Then
            Dim BILLNO As Integer = 0
            StrSql = " SELECT ISNULL(MAX(TRANNO),0) TRANNO FROM  " & cnStockDb & "..ISSUE "
            StrSql += vbCrLf + " WHERE TRANDATE <'" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + " AND TRANTYPE IN('" & Typepac & "') AND ISNULL(CANCEL,'')=''"
            If mCompId <> "" Then StrSql += " AND COMPANYID='" & mCompId & "'"
            If mCostId <> "" Then StrSql += " AND COSTID='" & mCostId & "'"
            BILLNO = Val(objGPack.GetSqlValue(StrSql, "", "", tran))

            StrSql = " SELECT DISTINCT TRANNO,TRANDATE,BATCHNO FROM " & cnStockDb & "..ISSUE"
            StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + " AND TRANTYPE IN('" & Typepac & "') AND ISNULL(CANCEL,'')=''"
            If mCompId <> "" Then StrSql += " AND COMPANYID='" & mCompId & "'"
            If mCostId <> "" Then StrSql += " AND COSTID='" & mCostId & "'"
            StrSql += vbCrLf + "   ORDER BY TRANDATE,TRANNO"
            Dim DtBillDet As New DataTable
            Cmd = New OleDbCommand(StrSql, cn, tran)
            Da = New OleDbDataAdapter(Cmd)
            Da.Fill(DtBillDet)

            For z As Integer = 0 To DtBillDet.Rows.Count - 1
                BILLNO = BILLNO + 1
                StrSql = " UPDATE " & cnAdminDb & "..OUTSTANDING SET TRANNO='" & BILLNO & "' "
                StrSql += vbCrLf + " WHERE TRANNO='" & Val(DtBillDet.Rows(z).Item("TRANNO").ToString) & "' AND BATCHNO='" & DtBillDet.Rows(z).Item("BATCHNO").ToString & "'"
                StrSql += vbCrLf + " UPDATE " & cnStockDb & "..ACCTRAN SET TRANNO='" & BILLNO & "' "
                StrSql += vbCrLf + " WHERE TRANNO='" & Val(DtBillDet.Rows(z).Item("TRANNO").ToString) & "' AND BATCHNO='" & DtBillDet.Rows(z).Item("BATCHNO").ToString & "'"
                StrSql += vbCrLf + " UPDATE " & cnStockDb & "..ISSSTONE SET TRANNO='" & BILLNO & "' "
                StrSql += vbCrLf + " WHERE ISSSNO IN(SELECT SNO FROM  " & cnStockDb & "..ISSUE WHERE TRANNO='" & DtBillDet.Rows(z).Item("TRANNO").ToString & "' AND BATCHNO='" & DtBillDet.Rows(z).Item("BATCHNO").ToString & "')"
                StrSql += vbCrLf + " UPDATE " & cnStockDb & "..ISSUE SET TRANNO='" & BILLNO & "' "
                StrSql += vbCrLf + " WHERE TRANNO='" & Val(DtBillDet.Rows(z).Item("TRANNO").ToString) & "' AND BATCHNO='" & DtBillDet.Rows(z).Item("BATCHNO").ToString & "'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Cmd.ExecuteNonQuery()
            Next
            If BILLNO > 0 Then
                StrSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT='" & BILLNO & "' WHERE CTLID='" & Pacctrlid & "' AND COMPANYID='" & mCompId & "'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Cmd.ExecuteNonQuery()
            End If
        End If


        'ISSUE ALL
        If GType <> "" Then
            Dim BILLNO As Integer = 0
            StrSql = " SELECT ISNULL(MAX(TRANNO),0) TRANNO FROM  " & cnStockDb & "..ISSUE  "
            StrSql += vbCrLf + " WHERE TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + " AND TRANTYPE IN('" & GType.Replace(",", "','") & "') AND ISNULL(CANCEL,'')=''"
            If mCompId <> "" Then StrSql += " AND COMPANYID='" & mCompId & "'"
            If mCostId <> "" Then StrSql += " AND COSTID='" & mCostId & "'"
            BILLNO = Val(objGPack.GetSqlValue(StrSql, "", "", tran))

            StrSql = " SELECT DISTINCT TRANNO,TRANDATE,BATCHNO FROM " & cnStockDb & "..ISSUE "
            StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            StrSql += vbCrLf + " AND TRANTYPE IN('" & GType.Replace(",", "','") & "') AND ISNULL(CANCEL,'')=''"
            If mCompId <> "" Then StrSql += " AND COMPANYID='" & mCompId & "'"
            If mCostId <> "" Then StrSql += " AND COSTID='" & mCostId & "'"
            StrSql += vbCrLf + "   ORDER BY TRANDATE,TRANNO"
            Dim DtBillDet As New DataTable
            Cmd = New OleDbCommand(StrSql, cn, tran)
            Da = New OleDbDataAdapter(Cmd)
            Da.Fill(DtBillDet)

            For z As Integer = 0 To DtBillDet.Rows.Count - 1
                BILLNO = BILLNO + 1
                StrSql = " UPDATE " & cnAdminDb & "..OUTSTANDING SET TRANNO='" & BILLNO & "' "
                StrSql += vbCrLf + " WHERE TRANNO='" & Val(DtBillDet.Rows(z).Item("TRANNO").ToString) & "' AND BATCHNO='" & DtBillDet.Rows(z).Item("BATCHNO").ToString & "'"
                StrSql += vbCrLf + " UPDATE " & cnStockDb & "..ACCTRAN SET TRANNO='" & BILLNO & "' "
                StrSql += vbCrLf + " WHERE TRANNO='" & Val(DtBillDet.Rows(z).Item("TRANNO").ToString) & "' AND BATCHNO='" & DtBillDet.Rows(z).Item("BATCHNO").ToString & "'"
                StrSql += vbCrLf + " UPDATE " & cnStockDb & "..ISSSTONE SET TRANNO='" & BILLNO & "' "
                StrSql += vbCrLf + " WHERE ISSSNO IN(SELECT SNO FROM  " & cnStockDb & "..ISSUE WHERE TRANNO='" & DtBillDet.Rows(z).Item("TRANNO").ToString & "' AND BATCHNO='" & DtBillDet.Rows(z).Item("BATCHNO").ToString & "')"
                StrSql += vbCrLf + " UPDATE " & cnStockDb & "..ISSUE SET TRANNO='" & BILLNO & "' "
                StrSql += vbCrLf + " WHERE TRANNO='" & Val(DtBillDet.Rows(z).Item("TRANNO").ToString) & "' AND BATCHNO='" & DtBillDet.Rows(z).Item("BATCHNO").ToString & "'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Cmd.ExecuteNonQuery()
            Next
            If BILLNO > 0 Then
                StrSql = " UPDATE " & cnStockDb & "..BILLCONTROL SET CTLTEXT='" & BILLNO & "' WHERE CTLID='" & Gbctrlid & "' AND COMPANYID='" & mCompId & "'"
                Cmd = New OleDbCommand(StrSql, cn, tran)
                Cmd.ExecuteNonQuery()
            End If
        End If

    End Function
End Class