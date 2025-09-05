Imports System.Data.OleDb

Public Class MaterialIssRecPosting
    Dim str As String = Nothing
    Dim strSql As String = Nothing
    Dim dt As DataTable
    Dim cmd As OleDb.OleDbCommand
    Dim dtCostCentre As DataTable
    Dim dtCompany As DataTable
    Dim dtAcname As DataTable
    



    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click

        str = "SELECT A.ACNAME,R.SNO,I.LOTSNO,(ISNULL(R.GRSWT,0)) RECEIPT,SUM(ISNULL(I.GRSWT,0)) ISSUE,"
        str += vbCrLf + " (ISNULL(R.GRSWT,0))-SUM(ISNULL(I.GRSWT,0))BALANCE"
        str += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS R LEFT JOIN " & cnAdminDb & "..ACHEAD AS A ON R.ACCODE=A.ACCODE"
        str += vbCrLf + " LEFT JOIN " & cnStockDb & "..LOTISSUE AS L ON R.SNO=L.RECSNO"
        str += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMTAG AS I ON L.LOTSNO=I.LOTSNO"
        str += vbCrLf + " WHERE(1 = 1)AND TRANTYPE='RRE'"
        str += vbCrLf + " AND R.TRANDATE='" & DtPicker.Value.ToString("yyyy-MM-dd") & "'"
        str += vbCrLf + " AND R.COMPANYID='SEN'"
        str += vbCrLf + " AND ISNULL(R.CANCEL,'')=''"
        str += vbCrLf + " GROUP BY ACNAME,R.GRSWT,R.SNO,I.LOTSNO"
        dt = New DataTable
        dt.Clear()

        da = New OleDbDataAdapter(str, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            MsgBox("Record Not Found", MsgBoxStyle.Exclamation)
            Exit Sub
        Else
            dt.Columns.Add("posting", Type.GetType("System.Boolean"))
            dt.Columns("POSTING").DefaultValue = False
            'For i As Integer = 0 To dt.Rows.Count - 1
            '    dt.Rows(i).Item("posting")
            'Next
            DataGridView1.DataSource = dt
          End If
    End Sub

    Private Sub MaterialIssRecPosting_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        str = " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY where ISNULL(ACTIVE,'')<>'N'"
        str += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(str, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(Cmbcompany, dtCompany, "COMPANYNAME", True)

        str = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        str += " UNION ALL"
        str += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        str += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(str, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(CheckedComboBox2, dtCostCentre, "COSTNAME", , "ALL")

        str = " SELECT 'ALL' ACNAME,'ALL' ACCODE,1 RESULT"
        str += " UNION ALL"
        str += " SELECT  ACNAME,ACCODE,2 RESULT FROM " & cnAdminDb & "..ACHEAD"
        str += " ORDER BY ACNAME,ACCODE"
        dtAcname = New DataTable
        da = New OleDbDataAdapter(str, cn)
        da.Fill(dtAcname)
        If dtAcname.Rows.Count > 0 Then
            cmbAcName.DataSource = Nothing
            cmbAcName.DataSource = dtAcname
            cmbAcName.ValueMember = "ACCODE"
            cmbAcName.DisplayMember = "ACName"
            cmbAcName.Text = "ALL"
        End If
        DtPicker.Value = GetServerDate()
        ' BrighttechPack.GlobalMethods.FillCombo(cmbAcName, dtAcname, "ACNAME", , "ALL")
End Sub
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        btnView.Visible = True
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub MaterialIssRecPosting_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnposting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnposting.Click
        Dim COSTID As String = Nothing
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            With DataGridView1.Rows(i)

                For Each row As DataGridViewRow In DataGridView1.Rows
                If DataGridView1.Rows(row.Index).Cells(3).Value = True Then
                'If Val(.Cells("POSTING").Value.ToString) = True Then


                    If Val(.Cells("balance").Value.ToString) < 0 Then
                        Dim dtrec As New DataTable
                        strSql = "SELECT * FROM " & cnStockDb & "..RECEIPT WHERE  SNO='" & .Cells("sno").Value.ToString & "'"
                        da = New OleDbDataAdapter(strSql, cn)
                        da.Fill(dtrec)
                        If dtrec.Rows.Count > 0 Then
                            Dim issSno As String = GetNewSno(TranSnoType.RECEIPTCODE, tran)
                            strSql = " INSERT INTO " & cnStockDb & "..RECEIPT"
                            strSql += " ("
                            strSql += " SNO,TRANNO,TRANDATE,TRANTYPE,PCS"
                            strSql += " ,GRSWT,NETWT,LESSWT,PUREWT"
                            strSql += " ,TAGNO,ITEMID,SUBITEMID,WASTPER"
                            strSql += " ,WASTAGE,MCGRM,MCHARGE,AMOUNT"
                            strSql += " ,RATE,BOARDRATE,SALEMODE,GRSNET"
                            strSql += " ,TRANSTATUS,REFNO,REFDATE,COSTID"
                            strSql += " ,COMPANYID,FLAG,EMPID,TAGGRSWT"
                            strSql += " ,TAGNETWT,TAGRATEID,TAGSVALUE,TAGDESIGNER"
                            strSql += " ,ITEMCTRID,ITEMTYPEID,PURITY,TABLECODE"
                            strSql += " ,INCENTIVE,WEIGHTUNIT,CATCODE,OCATCODE"
                            strSql += " ,ACCODE,ALLOY,BATCHNO,REMARK1"
                                strSql += " ,REMARK2,USERID,UPDATED,UPTIME,SYSTEMID,DISCOUNT,RUNNO,CASHID,TAX,SC"
                            strSql += " ,DUSTWT,PUREXCH,MAKE,STNAMT,MISCAMT,METALID,STONEUNIT,APPVER"
                            strSql += " ,TOUCH,OTHERAMT"
                            strSql += " )"
                            strSql += " VALUES("
                            strSql += " '" & issSno & "'" ''SNO
                            strSql += " ," & Val(dtrec.Rows(0).Item("TRANNO")) & "" 'TRANNO
                            strSql += " ,'" & dtrec.Rows(0).Item("TRANDATE").ToString & "'" 'TRANDATE
                            'strSql += " ,'" & .Cells("TRANTYPE").Value.ToString = RRE & "'" 'TRANTYPE
                            strSql += " ,'RRE'" 'TRANTYPE
                            strSql += " ," & Val(dtrec.Rows(0).Item("PCS")) & "" 'PCS
                            strSql += " ," & Val(.Cells("balance").Value.ToString) & "" 'GRSWT
                            strSql += " ," & Val(.Cells("balance").Value.ToString) & "" 'NETWT
                            strSql += " ," & Val(.Cells("balance").Value.ToString) & "" 'LESSWT
                            strSql += " ," & Val(.Cells("balance").Value.ToString) & "" 'PUREWT '0
                            strSql += " ,''" 'TAGNO
                            strSql += " ," & Val(dtrec.Rows(0).Item("ITEMID").ToString) & "" 'ITEMID
                            strSql += " ," & Val(dtrec.Rows(0).Item("SUBITEMID").ToString) & "" 'SUBITEMID
                            strSql += " ," & Val(dtrec.Rows(0).Item("WASTPER").ToString) & "" 'WASTPER
                            strSql += " ," & Val(dtrec.Rows(0).Item("WASTAGE").ToString) & "" 'WASTAGE
                            strSql += " ,0" 'MCGRM
                            strSql += " ,0" 'MCHARGE
                            'strSql += " ," & IIf(dtrec.Rows(0).Item("TRANTYPE").ToString = "PU", Val(dtrec.Rows(0).Item("GROSSAMT").ToString) + Val(dtrec.Rows(0).Item("VAT").ToString), Val(dtrec.Rows(0).Item("GROSSAMT").ToString)) & "" 'AMOUNT
                            strSql += " ," & Val(dtrec.Rows(0).Item("AMOUNT").ToString) & "" 'AMOUNT
                            strSql += " ," & Val(dtrec.Rows(0).Item("RATE").ToString) & "" 'RATE
                            strSql += " ," & Val(dtrec.Rows(0).Item("BOARDRATE").ToString) & "" 'BOARDRATE
                            strSql += " ,''" 'SALEMODE
                            strSql += " ,''" 'GRSNET
                            strSql += " ,''" 'TRANSTATUS ''
                            strSql += " ,''" 'REFNO ''
                            strSql += " ,NULL" 'REFDATE NULL
                            strSql += " ,'" & IIf(IsDBNull(dtrec.Rows(0).Item("COSTID").ToString), "", dtrec.Rows(0).Item("COSTID").ToString) & "'" 'COSTID 
                            strSql += " ,'" & dtrec.Rows(0).Item("COMPANYID").ToString & "'" 'COMPANYID
                            strSql += " ,'" & dtrec.Rows(0).Item("FLAG").ToString & "'" 'FLAG 
                            strSql += " ," & Val(dtrec.Rows(0).Item("EMPID").ToString) & "" 'EMPID
                            strSql += " ,0" 'TAGGRSWT
                            strSql += " ,0" 'TAGNETWT
                            strSql += " ,0" 'TAGRATEID
                            strSql += " ,0" 'TAGSVALUE
                            strSql += " ,''" 'TAGDESIGNER  
                            strSql += " ,0" 'ITEMCTRID
                            strSql += " ," & Val(dtrec.Rows(0).Item("ITEMTYPEID").ToString) & "" 'ITEMTYPEID
                            strSql += " ," & Val(dtrec.Rows(0).Item("PURITY").ToString) & "" 'PURITY
                            strSql += " ,''" 'TABLECODE
                            strSql += " ,''" 'INCENTIVE
                            strSql += " ,''" 'WEIGHTUNIT
                            strSql += " ,'" & dtrec.Rows(0).Item("CATCODE").ToString & "'" 'CATCODE
                            strSql += " ,''" 'OCATCODE
                            strSql += " ,''" 'ACCODE
                            strSql += " ," & Val(dtrec.Rows(0).Item("ALLOY").ToString) & "" 'ALLOY
                            strSql += " ,'" & dtrec.Rows(0).Item("BATCHNO").ToString & "'" 'BATCHNO
                            strSql += " ,'" & dtrec.Rows(0).Item("REMARK1").ToString & "'" 'REMARK1
                            strSql += " ,'" & dtrec.Rows(0).Item("REMARK2").ToString & "'" 'REMARK2
                            strSql += " ,'" & userId & "'" 'USERID
                            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                            strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                            strSql += " ,'" & systemId & "'" 'SYSTEMID
                            strSql += " ," & Val(dtrec.Rows(0).Item("DISCOUNT").ToString) & "" 'DISCOUNT
                            strSql += " ,''" 'RUNNO
                            strSql += " ,'" & dtrec.Rows(0).Item("CASHID").ToString & "'" 'CASHID
                                strSql += " ," & Val(dtrec.Rows(0).Item("TAX").ToString) & "" 'TAX
                            strSql += " ," & Val(dtrec.Rows(0).Item("SC").ToString) & "" 'SC

                            strSql += " ," & Val(dtrec.Rows(0).Item("DUSTWT").ToString) & "" 'DUSTWT
                            strSql += " ,'" & dtrec.Rows(0).Item("PUREXCH").ToString & "'" 'PUREXCH
                            strSql += " ,''" 'MAKE
                            strSql += " ," & Val(dtrec.Rows(0).Item("STNAMT").ToString) & "" 'STONEAMT
                            strSql += " ," & Val(dtrec.Rows(0).Item("MISCAMT").ToString) & "" 'MISCAMT
                            strSql += " ,'" & Val(dtrec.Rows(0).Item("METALID").ToString) & "'" 'METALID
                            strSql += " ,'" & Val(dtrec.Rows(0).Item("STONEUNIT").ToString) & "'" 'STONEUNIT
                            strSql += " ,'" & VERSION & "'" 'APPVER
                            strSql += " ," & Val(dtrec.Rows(0).Item("TOUCH").ToString) & "" 'TOUCH
                            strSql += " ," & Val(dtrec.Rows(0).Item("OTHERAMT").ToString) & "" 'OTHERAMT
                            strSql += " )"
                            'ExecQuery(SyncMode.Transaction, strSql, cn, tran, IIf(IsDBNull(dtrec.Rows(0).Item("COSTID").ToString), "", dtrec.Rows(0).Item("COSTID").ToString))
                            'ExecQuery(SyncMode.Transaction, strSql, cn, tran, COSTID)

                            cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()

                        End If

                    Else
                        Dim dtitem As New DataTable


                            strSql = "SELECT * FROM " & cnAdminDb & "..ITEMTAG WHERE LOTSNO='" & .Cells("lotsno").Value.ToString & "'"
                            da = New OleDbDataAdapter(strSql, cn)
                            da.Fill(dtitem)
                            If dtitem.Rows.Count > 0 Then
                                Dim TagSno As String = Nothing


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
                                strSql += " BATCHNO,MARK,"
                                strSql += " PCTFILE,OLDTAGNO,ITEMTYPEID,ACTUALRECDATE,WEIGHTUNIT,"
                                strSql += " TRANSFERWT,CHKDATE,CHKTRAY,CARRYFLAG,BRANDID,PRNFLAG,"
                                strSql += " MCDISCPER,WASTDISCPER,RESDATE,TRANINVNO,SUPBILLNO,WORKDAYS,"
                                strSql += " USERID,UPDATED,UPTIME,SYSTEMID,STYLENO,APPVER,TRANSFERDATE,"
                                strSql += " BOARDRATE,RFID,TOUCH,HM_BILLNO,HM_CENTER,ADD_VA_PER,REFVALUE,VALUEADDEDTYPE,TCOSTID,EXTRAWT,"
                                strSql += " USRATE,INDRS"
                                strSql += " )VALUES("

                                strSql += " '" & TagSno & "'" 'SNO
                                strSql += " ,'" & dtitem.Rows(0).Item("RECDATE").ToString & "'" 'RECDATE 'dtpRecieptDate.Value.Date.ToString("yyyy-MM-dd")
                                strSql += " ,'" & IIf(COSTCENTRE_SINGLE = False, cnCostId, COSTID) & "'" 'COSTID
                                strSql += " ," & Val(dtitem.Rows(0).Item("ITEMID").ToString) & "" 'ITEMID
                                strSql += " ,'" & dtitem.Rows(0).Item("ORDREPNO").ToString & "'" 'ORDREPNO
                                strSql += " ,'" & dtitem.Rows(0).Item("ORSNO").ToString & "'" 'ORsno
                                strSql += " ,''" 'ORDSALMANCODE
                                strSql += " ," & Val(dtitem.Rows(0).Item("SUBITEMID").ToString) & "" 'SUBITEMID
                                strSql += " ," & Val(dtitem.Rows(0).Item("SIZEID").ToString) & "" 'SIZEID
                                strSql += " ," & Val(dtitem.Rows(0).Item("ITEMCTRID").ToString) & "" 'ITEMCTRID
                                strSql += " ,'" & dtitem.Rows(0).Item("TABLECODE").ToString & "'"
                                strSql += " ," & Val(dtitem.Rows(0).Item("DESIGNERID").ToString) & "" 'DESIGNERID
                                strSql += " ,'" & dtitem.Rows(0).Item("TAGNO").ToString & "'" 'TAGNO
                                strSql += " ," & Val(dtitem.Rows(0).Item("PCS").ToString) & "" 'PCS
                                strSql += " ," & Val(dtitem.Rows(0).Item("GRSWT").ToString) & "" 'GRSWT
                                strSql += " ," & Val(dtitem.Rows(0).Item("LESSWT").ToString) & "" 'LESSWT
                                strSql += " ," & Val(dtitem.Rows(0).Item("NETWT").ToString) & "" 'NETWT
                                strSql += " ," & Val(dtitem.Rows(0).Item("RATE").ToString)
                                strSql += ",0" 'FINERATE
                                strSql += " ," & Val(dtitem.Rows(0).Item("MAXWASTPER").ToString) & "" 'MAXWASTPER

                                strSql += " ," & Val(dtitem.Rows(0).Item("MAXMCGRM").ToString) & "" 'MAXMCGRM
                                strSql += " ," & Val(dtitem.Rows(0).Item("MAXWAST").ToString) & "" 'MAXWAST
                                strSql += " ," & Val(dtitem.Rows(0).Item("MAXMC").ToString) & "" 'MAXMC
                                strSql += " ," & Val(dtitem.Rows(0).Item("MINWASTPER").ToString) & "" 'MINWASTPER
                                strSql += " ," & Val(dtitem.Rows(0).Item("MINMCGRM").ToString) & "" 'MINMCGRM
                                strSql += " ," & Val(dtitem.Rows(0).Item("MINWAST").ToString) & "" 'MINWAST
                                strSql += " ," & Val(dtitem.Rows(0).Item("MINMC").ToString) & "" 'MINMC
                                strSql += " ,'" & Val(dtitem.Rows(0).Item("TAGKEY").ToString) & "'" 'TAGKEY
                                strSql += " ," & Val(dtitem.Rows(0).Item("TAGVAL").ToString) & "" 'TAGVAL
                                strSql += " ,'" & dtitem.Rows(0).Item("LOTSNO").ToString & "'" 'LOTSNO
                                strSql += " ,'" & dtitem.Rows(0).Item("COMPANYID").ToString & "'" 'COMPANYID
                                strSql += " ," & Val(dtitem.Rows(0).Item("SALVALUE").ToString) & "" 'SALVALUE
                                strSql += " ," & Val(dtitem.Rows(0).Item("PURITY").ToString) & "" 'PURITY
                                strSql += " ,'" & dtitem.Rows(0).Item("NARRATION").ToString & "'" 'NARRATION
                                strSql += " ,'" & dtitem.Rows(0).Item("DESCRIP").ToString & "'"
                                strSql += " ,''" 'REASON
                                strSql += " ,'" & dtitem.Rows(0).Item("ENTRYMODE").ToString & "'" 'ENTRYMODE
                                strSql += " ,'" & dtitem.Rows(0).Item("GRSNET").ToString & "'" 'GRSNET
                                strSql += " ,NULL" 'ISSDATE
                                strSql += " ,0" 'ISSREFNO
                                strSql += " ,0" 'ISSPCS

                                strSql += " ,0" 'ISSWT
                                strSql += " ,''" 'FROMFLAG
                                strSql += " ,''" 'TOFLAG
                                strSql += " ,''" 'APPROVAL
                                strSql += " ,'" & dtitem.Rows(0).Item("SALEMODE").ToString & "'" 'SALEMODE
                                strSql += " ,''" 'BATCHNO
                                strSql += " ,0" 'MARK

                                strSql += " ,''" ' pctfile
                                strSql += " ,''" 'OLDTAGNO
                                strSql += " ," & Val(dtitem.Rows(0).Item("ITEMTYPEID").ToString) & "" 'ITEMTYPEID
                                strSql += " ,'" & dtitem.Rows(0).Item("ACTUALRECDATE").ToString & "'" 'ACTUALRECDATE
                                strSql += " ,''" 'WEIGHTUNIT
                                strSql += " ,0" 'TRANSFERWT
                                strSql += " ,NULL" 'CHKDATE
                                strSql += " ,''" 'CHKTRAY
                                strSql += " ,''" 'CARRYFLAG
                                strSql += " ,''" 'BRANDID
                                strSql += " ,''" 'PRNFLAG
                                strSql += " ,0" 'MCDISCPER
                                strSql += " ,0" 'WASTDISCPER
                                strSql += " ,NULL" 'RESDATE
                                strSql += " ,'" & dtitem.Rows(0).Item("TRANINVNO").ToString & "'" 'TRANINVNO
                                strSql += " ,'" & dtitem.Rows(0).Item("SUPBILLNO").ToString & "'" 'SUPBILLNO
                                strSql += " ,''" 'WORKDAYS
                                strSql += " ," & Val(dtitem.Rows(0).Item("USERID").ToString) & "" 'USERID
                                strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'UPDATED
                                strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'UPTIME
                                strSql += " ,'" & dtitem.Rows(0).Item("SYSTEMID").ToString & "'" 'SYSTEMID
                                strSql += " ,'" & dtitem.Rows(0).Item("STYLENO").ToString & "'" 'STYLENO
                                strSql += " ,'" & dtitem.Rows(0).Item("APPVER").ToString & "'" 'APPVER
                                strSql += " ,'" & dtitem.Rows(0).Item("TRANSFERDATE").ToString & "'" 'TRANSFERDATE
                                strSql += " ," & Val(dtitem.Rows(0).Item("BOARDRATE").ToString) & "" 'BOARDRATE,RFID,TOUCH
                                strSql += " ,'" & dtitem.Rows(0).Item("RFID").ToString & "'"
                                strSql += " ," & Val(dtitem.Rows(0).Item("TOUCH").ToString) & ""
                                strSql += " ,'" & dtitem.Rows(0).Item("HM_BILLNO").ToString & "'" 'HM_BILLNO
                                strSql += " ,'" & dtitem.Rows(0).Item("HM_CENTER").ToString & "'" 'HM_CENTER
                                strSql += " ," & Val(dtitem.Rows(0).Item("ADD_VA_PER").ToString) & "" 'ADD_VA_PER
                                strSql += " ," & Val(dtitem.Rows(0).Item("REFVALUE").ToString) & "" 'REFVALUE VALUEADDEDTYPE,TCOSTID,EXTRAWT,"
                                'strSql += " USRATE,INDRS"
                                strSql += " ,'" & dtitem.Rows(0).Item("VALUEADDEDTYPE").ToString & "'"
                                strSql += " ,'" & dtitem.Rows(0).Item("TCOSTID").ToString & "'" 'TCOSTID
                                strSql += " ,'" & dtitem.Rows(0).Item("EXTRAWT").ToString & "'" 'EXTRAWT
                                strSql += " ," & Val(dtitem.Rows(0).Item("USRATE").ToString) & ""
                                strSql += " ," & Val(dtitem.Rows(0).Item("INDRS").ToString) & ""
                                strSql += " )"

                                cmd = New OleDbCommand(strSql, cn, tran) : cmd.ExecuteNonQuery()
                                'ExecQuery(SyncMode.Stock, strSql, cn, tran, COSTID, , IIf(picPath <> Nothing, (defalutDestination + "L" + txtLotNo_Num_Man.Text + "I" + itemId.ToString + "T" + txtTagNo__Man.Text.Replace(":", "") + "." + picExtension), Nothing), "TITEMTAG", , False)

                            End If

                    End If
                End If
                 Next
            End With
        Next

    End Sub
End Class