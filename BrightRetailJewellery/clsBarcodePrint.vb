Imports System.IO
Imports System.Data.OleDb


Public Class clsBarcodePrint
    Dim strSql As String
    Dim cmd As OleDbCommand

    Function FuncprintBarcode_Single(ByVal ItemId As String, ByVal Tagno As String)
        Try
            Dim systemName As String = My.Computer.Name
            strSql = " IF OBJECT_ID('TEMPTABLEDB..[TEMPBARCODE" & systemName & "SINGLE]') IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMPBARCODE" & systemName & "SINGLE]"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            Dim WastMask As Boolean
            strSql = "SELECT COUNT(*)CNT FROM " & cnAdminDb & "..WASTMCCODE WHERE MASKTYPE='W'"
            If Val(objGPack.GetSqlValue(strSql, "CNT", 0)) > 0 Then
                WastMask = True
            End If
            Dim StnWTLen As Integer = Val(GetAdmindbSoftValue("BARCODE_STNWTLEN", 4))
            Dim STNPRINT_NEW As Boolean = IIf(GetAdmindbSoftValue("BARCODE_STNPRINT_NEW", "N") = "Y", True, False)
            strSql = " SELECT "
            strSql += vbCrLf + " SNO,(SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID)ITEMNAME,"
            strSql += vbCrLf + " (SELECT TOP 1 SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID=T.SUBITEMID)SUBITEMNAME,"
            strSql += vbCrLf + " (SELECT TOP 1 SHORTNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID)ITEMSHORTNAME,"
            strSql += vbCrLf + " (SELECT TOP 1 SHORTNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID=T.SUBITEMID)SUBITEMSHORTNAME,"
            strSql += vbCrLf + " (SELECT TOP 1 NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID=T.ITEMTYPEID)ITEMTYPE,"
            strSql += vbCrLf + " (SELECT TOP 1 METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID)METALID,"
            strSql += vbCrLf + " (SELECT TOP 1 DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID=T.DESIGNERID)DESIGNERNAME,"
            strSql += vbCrLf + " (SELECT TOP 1 SEAL FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID=T.DESIGNERID)SEAL,"
            strSql += vbCrLf + " (SELECT TOP 1 ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID=T.ITEMCTRID)ITEMCTRNAME,T.ITEMCTRID,"
            strSql += vbCrLf + " ITEMID,SUBITEMID,TABLECODE,DESIGNERID,PCS,GRSWT,NETWT,LESSWT,ISNULL(RATE,0)RATE,ISNULL(BOARDRATE,0)BOARDRATE"
            strSql += vbCrLf + " ,CASE WHEN PCS=0 THEN GRSWT ELSE CONVERT(NUMERIC(15,2),ROUND(GRSWT/PCS,2)) END AS AVGWT"
            strSql += vbCrLf + " ,CASE WHEN T.EXTRAWT <> 0 THEN T.EXTRAWT WHEN (SELECT SUM(GRSWT)  FROM " & cnAdminDb & ".. ORMAST   WHERE SNO = T.ORSNO  AND "
            strSql += vbCrLf + " ORNO = T. ORDREPNO ) <> 0 And ISNULL(T.GRSWT,0)<>0 THEN ISNULL(T.GRSWT,0) - (SELECT GRSWT FROM " & cnAdminDb & ".. ORMAST   WHERE SNO = T.ORSNO  "
            strSql += vbCrLf + " And ORNO = T. ORDREPNO  )  ELSE NULL END EXTRAWT"
            If WastMask Then
                strSql += vbCrLf + " ," & cnAdminDb & ".DBO.FUNCGETCODE(CAST(MAXWASTPER AS INT)) AS MAXWASTPER"
            Else
                strSql += vbCrLf + " ,MAXWASTPER"
            End If
            strSql += vbCrLf + " ,MAXWAST,MAXMCGRM,MAXMC,SALVALUE,ITEMTYPEID,STYLENO"
            strSql += vbCrLf + " ,(SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZEID=T.SIZEID)ITEMSIZE"
            strSql += vbCrLf + " ,ISNULL((SELECT CONVERT(NUMERIC(15," & StnWTLen & "),SUM(STNWT)) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO And STNITEMID IN (SELECT DISTINCT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='S')),0)STNWT"
            strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT DISTINCT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='S')),0)STNAMT"
            strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT DISTINCT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='S') GROUP BY TAGSNO,ITEMID),0)STNPCS,'' STNNAME"
            strSql += vbCrLf + " ,ISNULL((SELECT CONVERT(NUMERIC(15," & StnWTLen & "),SUM(STNWT)) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT DISTINCT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D' AND ISNULL(BEEDS,'')<>'Y') GROUP BY TAGSNO,ITEMID),0)DIAWT"
            strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT DISTINCT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D' AND ISNULL(BEEDS,'')<>'Y') GROUP BY TAGSNO,ITEMID),0)DIAAMT"
            strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT DISTINCT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D' AND ISNULL(BEEDS,'')<>'Y') GROUP BY TAGSNO,ITEMID),0)DIAPS,'' DIANAME"
            strSql += vbCrLf + " ,ISNULL((SELECT CONVERT(NUMERIC(15," & StnWTLen & "),SUM(STNWT)) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT DISTINCT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(BEEDS,'')='Y') GROUP BY TAGSNO,ITEMID),0)BDSWT"
            strSql += vbCrLf + " ,(SELECT COUNT(*) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO) STNCOUNT"
            strSql += vbCrLf + " ,(SELECT COUNT(*) FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO=T.SNO) MMCOUNT"
            strSql += vbCrLf + " ,DAY(RECDATE)DD,MONTH(RECDATE)MM,SUBSTRING(CAST(YEAR(RECDATE) AS VARCHAR),3,2)YY"
            strSql += vbCrLf + " ,SUBSTRING(ORDREPNO,6,20) AS ORDNO,SALEMODE,NARRATION"
            strSql += vbCrLf + " ,ISNULL((SELECT TOP 1 LOTNO FROM " & cnAdminDb & "..ITEMLOT WHERE SNO=T.LOTSNO),'')LOTNO "
            strSql += vbCrLf + " ,CASE WHEN ISNULL((SELECT COUNT(*)CNT FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT DISTINCT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE "
            strSql += vbCrLf + " DIASTONE='D')),0) >1 THEN '*' ELSE '' END DIACHAR"
            strSql += vbCrLf + " ,ISNULL((SELECT TOP 1 (SELECT SHORTNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=S.STNITEMID) FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE TAGSNO=T.SNO AND "
            strSql += vbCrLf + " STNITEMID In (SELECT DISTINCT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D')),'') DIASHNAME"
            strSql += vbCrLf + " INTO TEMPTABLEDB..[TEMPBARCODE" & systemName & "SINGLE] "
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + " WHERE ITEMID='" & ItemId & "' AND TAGNO='" & Tagno & "'"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            strSql = "SELECT * FROM TEMPTABLEDB..[TEMPBARCODE" & systemName & "SINGLE]"
            Dim dtTag As DataTable
            cmd = New OleDbCommand(strSql, cn)
            da = New OleDbDataAdapter(cmd)
            dtTag = New DataTable
            da.Fill(dtTag)
            If dtTag.Rows.Count = 0 Then Exit Function
            Dim OrdNo As String = ""
            Dim Pcs As String = ""
            Dim AvgWt As String = ""
            Dim ItemName As String = "" : Dim SubItemName As String = ""
            Dim ItemShortName As String = "" : Dim SubItemShortName As String = "" : Dim ItemCtrName As String = "" : Dim ItemCtrId As String = ""
            Dim GrsWt As String = "" : Dim NetWt As String = "" : Dim ExtraWt As String = ""
            Dim LessWt As String = "" : Dim MAXWASTPER As String = ""
            Dim MAXWAST As String = "" : Dim MAXMCGRM As String = ""
            Dim MAXMC As String = "" : Dim stnAmt As String = ""
            Dim stnWt As String = "" : Dim stnPcs As String = "" : Dim stnName As String = ""
            Dim DiaAmt As String = "" : Dim DIAPS As String = "" : Dim DiaWt As String = "" : Dim BDSWt As String = ""
            Dim DiaName As String = "" : Dim DiaShName As String = "" : Dim DiaChar As String = "" : Dim Rate As String = "" : Dim BoardRate As String = ""
            Dim SalValue As String = "" : Dim ItemType As String = ""
            Dim MetalId As String = "" : Dim ItemSize As String = ""
            Dim DesginerId As String = "" : Dim TableCode As String = ""
            Dim DesginerName As String = ""
            Dim Seal As String = ""
            Dim stnWt1 As String = "" : Dim stnPcs1 As String = "" : Dim stnName1 As String = "" : Dim stnshName1 As String = "" : Dim stoneUnit1 As String = "" : Dim stoneRate1 As String = ""
            Dim stnWt2 As String = "" : Dim stnPcs2 As String = "" : Dim stnName2 As String = "" : Dim stnshName2 As String = "" : Dim stoneUnit2 As String = "" : Dim stoneRate2 As String = ""
            Dim stnWt3 As String = "" : Dim stnPcs3 As String = "" : Dim stnName3 As String = "" : Dim stnshName3 As String = "" : Dim stoneUnit3 As String = "" : Dim stoneRate3 As String = ""
            Dim stnWt4 As String = "" : Dim stnPcs4 As String = "" : Dim stnName4 As String = "" : Dim stnshName4 As String = "" : Dim stoneUnit4 As String = "" : Dim stoneRate4 As String = ""
            Dim stnWt5 As String = "" : Dim stnPcs5 As String = "" : Dim stnName5 As String = "" : Dim stnshName5 As String = "" : Dim stoneUnit5 As String = "" : Dim stoneRate5 As String = ""
            Dim stnWt6 As String = "" : Dim stnPcs6 As String = "" : Dim stnName6 As String = "" : Dim stnshName6 As String = "" : Dim stoneUnit6 As String = "" : Dim stoneRate6 As String = ""
            Dim stnWt7 As String = "" : Dim stnPcs7 As String = "" : Dim stnName7 As String = "" : Dim stnshName7 As String = "" : Dim stoneUnit7 As String = "" : Dim stoneRate7 As String = ""
            Dim stnWt8 As String = "" : Dim stnPcs8 As String = "" : Dim stnName8 As String = "" : Dim stnshName8 As String = "" : Dim stoneUnit8 As String = "" : Dim stoneRate8 As String = ""
            Dim stnWt9 As String = "" : Dim stnPcs9 As String = "" : Dim stnName9 As String = "" : Dim stnshName9 As String = "" : Dim stoneUnit9 As String = "" : Dim stoneRate9 As String = ""
            Dim stnWt10 As String = "" : Dim stnPcs10 As String = "" : Dim stnName10 As String = "" : Dim stnshName10 As String = "" : Dim stoneUnit10 As String = "" : Dim stoneRate10 As String = ""
            Dim stnWt11 As String = "" : Dim stnPcs11 As String = "" : Dim stnName11 As String = "" : Dim stnshName11 As String = "" : Dim stoneUnit11 As String = "" : Dim stoneRate11 As String = ""
            Dim stnWt12 As String = "" : Dim stnPcs12 As String = "" : Dim stnName12 As String = "" : Dim stnshName12 As String = "" : Dim stoneUnit12 As String = "" : Dim stoneRate12 As String = ""
            Dim TotStnWt As String = "" : Dim TotStnPcs As String = "" : Dim TotStnAmt As String = ""
            Dim DD As String : Dim MM As String : Dim YY As String : Dim NARRATION As String

            ''HALLMARK DETAILS
            Dim HUID1 As String = "" : Dim HUID2 As String = "" : Dim HUID3 As String = ""
            Dim HUID4 As String = "" : Dim HUID5 As String = "" : Dim HUID6 As String = ""
            Dim TagLotno As String = ""

            ''MultiMetal Details
            Dim MGrsWt1 As String = "" : Dim MNetwt1 As String = "" : Dim McatShname1 As String = "" : Dim Mwastper1 As String = "" : Dim Mcatname1 As String = ""
            Dim Mwast1 As String = "" : Dim MMcGrm1 As String = "" : Dim MMc1 As String = "" : Dim MMcharge1 As String = "" : Dim MRate1 As String = "" : Dim MAmount1 As String = ""
            Dim MGrsWt2 As String = "" : Dim MNetwt2 As String = "" : Dim McatShname2 As String = "" : Dim Mwastper2 As String = "" : Dim MCatname2 As String = ""
            Dim Mwast2 As String = "" : Dim MMcGrm2 As String = "" : Dim MMc2 As String = "" : Dim MMcharge2 As String = "" : Dim MRate2 As String = "" : Dim MAmount2 As String = ""
            Dim MGrsWt3 As String = "" : Dim MNetwt3 As String = "" : Dim McatShname3 As String = "" : Dim Mwastper3 As String = "" : Dim MCatname3 As String = ""
            Dim Mwast3 As String = "" : Dim MMcGrm3 As String = "" : Dim MMc3 As String = "" : Dim MMcharge3 As String = "" : Dim MRate3 As String = "" : Dim MAmount3 As String = ""
            Dim MGrsWt4 As String = "" : Dim MNetwt4 As String = "" : Dim McatShname4 As String = "" : Dim Mwastper4 As String = "" : Dim MCatname4 As String = ""
            Dim Mwast4 As String = "" : Dim MMcGrm4 As String = "" : Dim MMc4 As String = "" : Dim MMcharge4 As String = "" : Dim MRate4 As String = "" : Dim MAmount4 As String = ""
            Dim MGrsWt5 As String = "" : Dim MNetwt5 As String = "" : Dim McatShname5 As String = "" : Dim Mwastper5 As String = "" : Dim MCatname5 As String = ""
            Dim Mwast5 As String = "" : Dim MMcGrm5 As String = "" : Dim MMc5 As String = "" : Dim MMcharge5 As String = "" : Dim MRate5 As String = "" : Dim MAmount5 As String = ""


            strSql = " SELECT SUM(ISNULL(STNWT,0)) FROM " & cnAdminDb & "..ITEMTAGSTONE AS S WHERE TAGSNO ='" & dtTag.Rows(0)("SNO").ToString & "'"
            TotStnWt = Format(Val(objGPack.GetSqlValue(strSql, , "")), "0.00")

            strSql = " SELECT SUM(ISNULL(STNPCS,0)) FROM " & cnAdminDb & "..ITEMTAGSTONE AS S WHERE TAGSNO ='" & dtTag.Rows(0)("SNO").ToString & "'"
            TotStnPcs = Format(Val(objGPack.GetSqlValue(strSql, , "")), "0.00")

            strSql = " SELECT SUM(ISNULL(STNAMT,0)) FROM " & cnAdminDb & "..ITEMTAGSTONE AS S WHERE TAGSNO ='" & dtTag.Rows(0)("SNO").ToString & "'"
            TotStnAmt = Format(Val(objGPack.GetSqlValue(strSql, , "")), "0.00")

            'TotStnWt
            Dim dtTagStone As New DataTable
            strSql = "SELECT (SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=S.STNITEMID)STNITEMNAME,"
            strSql += vbCrLf + " (SELECT TOP 1 SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID=S.STNSUBITEMID)STNSUBITEMNAME,"
            strSql += vbCrLf + " (SELECT TOP 1 SHORTNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=S.STNITEMID)STNSHITEMNAME,"
            strSql += vbCrLf + " (SELECT TOP 1 SHORTNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID=S.STNSUBITEMID)STNSHSUBITEMNAME,"
            strSql += vbCrLf + " * FROM " & cnAdminDb & "..ITEMTAGSTONE AS S WHERE TAGSNO ='" & dtTag.Rows(0)("SNO").ToString & "'"
            cmd = New OleDbCommand(strSql, cn)
            da = New OleDbDataAdapter(cmd)
            dtTagStone = New DataTable
            da.Fill(dtTagStone)

            ''MultiMetal Details
            Dim dtTagMutliMetal As New DataTable
            strSql = "SELECT (SELECT TOP 1 CATNAME FROM " & cnAdminDb & "..CATEGORY WITH (NOLOCK) WHERE CATCODE=M.CATCODE)CATNAME,"
            strSql += vbCrLf + " (SELECT TOP 1 SHORTNAME FROM " & cnAdminDb & "..CATEGORY WITH (NOLOCK) WHERE CATCODE=M.CATCODE)CATSHNAME,"
            strSql += vbCrLf + " (CASE WHEN ISNULL(M.MCGRM,0)= 0 THEN M.MC ELSE M.MCGRM END) MCHARGE,"
            strSql += vbCrLf + " * FROM " & cnAdminDb & "..ITEMTAGMETAL M WITH (NOLOCK) WHERE TAGSNO ='" & dtTag.Rows(0)("SNO").ToString & "'"
            cmd = New OleDbCommand(strSql, cn)
            da = New OleDbDataAdapter(cmd)
            dtTagMutliMetal = New DataTable
            da.Fill(dtTagMutliMetal)

            ''HALLMARK DETAILS
            Dim dtTagHmDetails As New DataTable
            strSql = "SELECT "
            strSql += vbCrLf + " * FROM " & cnAdminDb & "..ITEMTAGHALLMARK AS S WHERE TAGSNO ='" & dtTag.Rows(0)("SNO").ToString & "'"
            cmd = New OleDbCommand(strSql, cn)
            da = New OleDbDataAdapter(cmd)
            dtTagHmDetails = New DataTable
            da.Fill(dtTagHmDetails)

            Dim StnNameLen As Integer = Val(GetAdmindbSoftValue("BARCODE_STNNAMELEN", 1))

            ''Stone Details
            For K As Integer = 0 To dtTagStone.Rows.Count - 1
                If K > 11 Then Exit For
                If K = 0 Then
                    stnWt1 = IIf(Val(dtTagStone.Rows(K)("STNWT").ToString) > 0,
                    Format(Math.Round(Val(dtTagStone.Rows(K)("STNWT").ToString), 2), "0.00") _
                    , "")
                    stnPcs1 = IIf(Val(dtTagStone.Rows(K)("STNPCS").ToString) > 0, dtTagStone.Rows(K)("STNPCS").ToString, "")
                    stnName1 = IIf(dtTagStone.Rows(K)("STNSUBITEMNAME").ToString <> "", dtTagStone.Rows(K)("STNSUBITEMNAME").ToString, dtTagStone.Rows(K)("STNITEMNAME").ToString)
                    Dim s As Integer = Len(stnName1.ToString)
                    If stnName1 <> "" Then stnName1 = stnName1.ToString.Substring(0, IIf(StnNameLen <= s, StnNameLen, s)) '' IIf(Len(stnName1.ToString) >= StnNameLen, stnName1.ToString, stnName1.ToString.Substring(0, IIf(StnNameLen <= s, StnNameLen, s)))
                    stnshName1 = IIf(dtTagStone.Rows(K)("STNSHSUBITEMNAME").ToString <> "", dtTagStone.Rows(K)("STNSHSUBITEMNAME").ToString, dtTagStone.Rows(K)("STNSHITEMNAME").ToString)
                    Dim s1 As Integer = Len(stnshName1.ToString)
                    If stnshName1 <> "" Then stnshName1 = stnshName1.ToString.Substring(0, IIf(StnNameLen <= s1, StnNameLen, s1))
                    stoneUnit1 = dtTagStone.Rows(K)("STONEUNIT").ToString
                    stoneRate1 = IIf(Val(dtTagStone.Rows(K)("STNRATE").ToString) = 0, dtTagStone.Rows(K)("STNAMT").ToString, dtTagStone.Rows(K)("STNRATE").ToString)
                End If
                If K = 1 Then
                    stnWt2 = IIf(Val(dtTagStone.Rows(K)("STNWT").ToString) > 0,
                   Format(Math.Round(Val(dtTagStone.Rows(K)("STNWT").ToString), 2), "0.00") _
                    , "")
                    stnPcs2 = IIf(Val(dtTagStone.Rows(K)("STNPCS").ToString) > 0, dtTagStone.Rows(K)("STNPCS").ToString, "")
                    stnName2 = IIf(dtTagStone.Rows(K)("STNSUBITEMNAME").ToString <> "", dtTagStone.Rows(K)("STNSUBITEMNAME").ToString, dtTagStone.Rows(K)("STNITEMNAME").ToString)
                    Dim s As Integer = Len(stnName2.ToString)
                    If stnName2 <> "" Then stnName2 = stnName2.ToString.Substring(0, IIf(StnNameLen <= s, StnNameLen, s))
                    stnshName2 = IIf(dtTagStone.Rows(K)("STNSHSUBITEMNAME").ToString <> "", dtTagStone.Rows(K)("STNSHSUBITEMNAME").ToString, dtTagStone.Rows(K)("STNSHITEMNAME").ToString)
                    Dim s1 As Integer = Len(stnshName2.ToString)
                    If stnshName2 <> "" Then stnshName2 = stnshName2.ToString.Substring(0, IIf(StnNameLen <= s1, StnNameLen, s1))
                    stoneUnit2 = dtTagStone.Rows(K)("STONEUNIT").ToString
                    stoneRate2 = IIf(Val(dtTagStone.Rows(K)("STNRATE").ToString) = 0, dtTagStone.Rows(K)("STNAMT").ToString, dtTagStone.Rows(K)("STNRATE").ToString)
                End If
                If K = 2 Then
                    stnWt3 = IIf(Val(dtTagStone.Rows(K)("STNWT").ToString) > 0,
                    Format(Math.Round(Val(dtTagStone.Rows(K)("STNWT").ToString), 2), "0.00") _
                    , "")
                    stnPcs3 = IIf(Val(dtTagStone.Rows(K)("STNPCS").ToString) > 0, dtTagStone.Rows(K)("STNPCS").ToString, "")
                    stnName3 = IIf(dtTagStone.Rows(K)("STNSUBITEMNAME").ToString <> "", dtTagStone.Rows(K)("STNSUBITEMNAME").ToString, dtTagStone.Rows(K)("STNITEMNAME").ToString)
                    Dim s As Integer = Len(stnName3.ToString)
                    If stnName3 <> "" Then stnName3 = stnName3.ToString.Substring(0, IIf(StnNameLen <= s, StnNameLen, s))
                    stnshName3 = IIf(dtTagStone.Rows(K)("STNSHSUBITEMNAME").ToString <> "", dtTagStone.Rows(K)("STNSHSUBITEMNAME").ToString, dtTagStone.Rows(K)("STNSHITEMNAME").ToString)
                    Dim s1 As Integer = Len(stnshName3.ToString)
                    If stnshName3 <> "" Then stnshName3 = stnshName3.ToString.Substring(0, IIf(StnNameLen <= s1, StnNameLen, s1))
                    stoneUnit3 = dtTagStone.Rows(K)("STONEUNIT").ToString
                    stoneRate3 = IIf(Val(dtTagStone.Rows(K)("STNRATE").ToString) = 0, dtTagStone.Rows(K)("STNAMT").ToString, dtTagStone.Rows(K)("STNRATE").ToString)
                End If
                If K = 3 Then
                    stnWt4 = IIf(Val(dtTagStone.Rows(K)("STNWT").ToString) > 0,
                   Format(Math.Round(Val(dtTagStone.Rows(K)("STNWT").ToString), 2), "0.00") _
                    , "")
                    stnPcs4 = IIf(Val(dtTagStone.Rows(K)("STNPCS").ToString) > 0, dtTagStone.Rows(K)("STNPCS").ToString, "")
                    stnName4 = IIf(dtTagStone.Rows(K)("STNSUBITEMNAME").ToString <> "", dtTagStone.Rows(K)("STNSUBITEMNAME").ToString, dtTagStone.Rows(K)("STNITEMNAME").ToString)
                    Dim s As Integer = Len(stnName4.ToString)
                    If stnName4 <> "" Then stnName4 = stnName4.ToString.Substring(0, IIf(StnNameLen <= s, StnNameLen, s))
                    stnshName4 = IIf(dtTagStone.Rows(K)("STNSHSUBITEMNAME").ToString <> "", dtTagStone.Rows(K)("STNSHSUBITEMNAME").ToString, dtTagStone.Rows(K)("STNSHITEMNAME").ToString)
                    Dim s1 As Integer = Len(stnshName4.ToString)
                    If stnshName4 <> "" Then stnshName4 = stnshName4.ToString.Substring(0, IIf(StnNameLen <= s1, StnNameLen, s1))
                    stoneUnit4 = dtTagStone.Rows(K)("STONEUNIT").ToString
                    stoneRate4 = IIf(Val(dtTagStone.Rows(K)("STNRATE").ToString) = 0, dtTagStone.Rows(K)("STNAMT").ToString, dtTagStone.Rows(K)("STNRATE").ToString)
                End If
                If K = 4 Then
                    stnWt5 = IIf(Val(dtTagStone.Rows(K)("STNWT").ToString) > 0,
                   Format(Math.Round(Val(dtTagStone.Rows(K)("STNWT").ToString), 2), "0.00") _
                    , "")
                    stnPcs5 = IIf(Val(dtTagStone.Rows(K)("STNPCS").ToString) > 0, dtTagStone.Rows(K)("STNPCS").ToString, "")
                    stnName5 = IIf(dtTagStone.Rows(K)("STNSUBITEMNAME").ToString <> "", dtTagStone.Rows(K)("STNSUBITEMNAME").ToString, dtTagStone.Rows(K)("STNITEMNAME").ToString)
                    Dim s As Integer = Len(stnName5.ToString)
                    If stnName5 <> "" Then stnName5 = stnName5.ToString.Substring(0, IIf(StnNameLen <= s, StnNameLen, s))
                    stnshName5 = IIf(dtTagStone.Rows(K)("STNSHSUBITEMNAME").ToString <> "", dtTagStone.Rows(K)("STNSHSUBITEMNAME").ToString, dtTagStone.Rows(K)("STNSHITEMNAME").ToString)
                    Dim s1 As Integer = Len(stnshName5.ToString)
                    If stnshName5 <> "" Then stnshName5 = stnshName5.ToString.Substring(0, IIf(StnNameLen <= s1, StnNameLen, s1))
                    stoneUnit5 = dtTagStone.Rows(K)("STONEUNIT").ToString
                    stoneRate5 = IIf(Val(dtTagStone.Rows(K)("STNRATE").ToString) = 0, dtTagStone.Rows(K)("STNAMT").ToString, dtTagStone.Rows(K)("STNRATE").ToString)
                End If
                If K = 5 Then
                    stnWt6 = IIf(Val(dtTagStone.Rows(K)("STNWT").ToString) > 0,
                   Format(Math.Round(Val(dtTagStone.Rows(K)("STNWT").ToString), 2), "0.00") _
                    , "")
                    stnPcs6 = IIf(Val(dtTagStone.Rows(K)("STNPCS").ToString) > 0, dtTagStone.Rows(K)("STNPCS").ToString, "")
                    stnName6 = IIf(dtTagStone.Rows(K)("STNSUBITEMNAME").ToString <> "", dtTagStone.Rows(K)("STNSUBITEMNAME").ToString, dtTagStone.Rows(K)("STNITEMNAME").ToString)
                    Dim s As Integer = Len(stnName6.ToString)
                    If stnName6 <> "" Then stnName6 = stnName6.ToString.Substring(0, IIf(StnNameLen <= s, StnNameLen, s))
                    stnshName6 = IIf(dtTagStone.Rows(K)("STNSHSUBITEMNAME").ToString <> "", dtTagStone.Rows(K)("STNSHSUBITEMNAME").ToString, dtTagStone.Rows(K)("STNSHITEMNAME").ToString)
                    Dim s1 As Integer = Len(stnshName6.ToString)
                    If stnshName6 <> "" Then stnshName6 = stnshName6.ToString.Substring(0, IIf(StnNameLen <= s1, StnNameLen, s1))
                    stoneUnit6 = dtTagStone.Rows(K)("STONEUNIT").ToString
                    stoneRate6 = IIf(Val(dtTagStone.Rows(K)("STNRATE").ToString) = 0, dtTagStone.Rows(K)("STNAMT").ToString, dtTagStone.Rows(K)("STNRATE").ToString)
                End If
                If K = 6 Then
                    stnWt7 = IIf(Val(dtTagStone.Rows(K)("STNWT").ToString) > 0,
                   Format(Math.Round(Val(dtTagStone.Rows(K)("STNWT").ToString), 2), "0.00") _
                    , "")
                    stnPcs7 = IIf(Val(dtTagStone.Rows(K)("STNPCS").ToString) > 0, dtTagStone.Rows(K)("STNPCS").ToString, "")
                    stnName7 = IIf(dtTagStone.Rows(K)("STNSUBITEMNAME").ToString <> "", dtTagStone.Rows(K)("STNSUBITEMNAME").ToString, dtTagStone.Rows(K)("STNITEMNAME").ToString)
                    Dim s As Integer = Len(stnName7.ToString)
                    If stnName7 <> "" Then stnName7 = stnName7.ToString.Substring(0, IIf(StnNameLen <= s, StnNameLen, s))
                    stnshName7 = IIf(dtTagStone.Rows(K)("STNSHSUBITEMNAME").ToString <> "", dtTagStone.Rows(K)("STNSHSUBITEMNAME").ToString, dtTagStone.Rows(K)("STNSHITEMNAME").ToString)
                    Dim s1 As Integer = Len(stnshName7.ToString)
                    If stnshName7 <> "" Then stnshName7 = stnshName7.ToString.Substring(0, IIf(StnNameLen <= s1, StnNameLen, s1))
                    stoneUnit7 = dtTagStone.Rows(K)("STONEUNIT").ToString
                    stoneRate7 = IIf(Val(dtTagStone.Rows(K)("STNRATE").ToString) = 0, dtTagStone.Rows(K)("STNAMT").ToString, dtTagStone.Rows(K)("STNRATE").ToString)
                End If
                If K = 7 Then
                    stnWt8 = IIf(Val(dtTagStone.Rows(K)("STNWT").ToString) > 0,
                   Format(Math.Round(Val(dtTagStone.Rows(K)("STNWT").ToString), 2), "0.00") _
                    , "")
                    stnPcs8 = IIf(Val(dtTagStone.Rows(K)("STNPCS").ToString) > 0, dtTagStone.Rows(K)("STNPCS").ToString, "")
                    stnName8 = IIf(dtTagStone.Rows(K)("STNSUBITEMNAME").ToString <> "", dtTagStone.Rows(K)("STNSUBITEMNAME").ToString, dtTagStone.Rows(K)("STNITEMNAME").ToString)
                    Dim s As Integer = Len(stnName8.ToString)
                    If stnName8 <> "" Then stnName8 = stnName8.ToString.Substring(0, IIf(StnNameLen <= s, StnNameLen, s))
                    stnshName8 = IIf(dtTagStone.Rows(K)("STNSHSUBITEMNAME").ToString <> "", dtTagStone.Rows(K)("STNSHSUBITEMNAME").ToString, dtTagStone.Rows(K)("STNSHITEMNAME").ToString)
                    Dim s1 As Integer = Len(stnshName8.ToString)
                    If stnshName8 <> "" Then stnshName8 = stnshName8.ToString.Substring(0, IIf(StnNameLen <= s1, StnNameLen, s1))
                    stoneUnit8 = dtTagStone.Rows(K)("STONEUNIT").ToString
                    stoneRate8 = IIf(Val(dtTagStone.Rows(K)("STNRATE").ToString) = 0, dtTagStone.Rows(K)("STNAMT").ToString, dtTagStone.Rows(K)("STNRATE").ToString)
                End If

                If K = 8 Then
                    stnWt9 = IIf(Val(dtTagStone.Rows(K)("STNWT").ToString) > 0,
                   Format(Math.Round(Val(dtTagStone.Rows(K)("STNWT").ToString), 2), "0.00") _
                    , "")
                    stnPcs9 = IIf(Val(dtTagStone.Rows(K)("STNPCS").ToString) > 0, dtTagStone.Rows(K)("STNPCS").ToString, "")
                    stnName9 = IIf(dtTagStone.Rows(K)("STNSUBITEMNAME").ToString <> "", dtTagStone.Rows(K)("STNSUBITEMNAME").ToString, dtTagStone.Rows(K)("STNITEMNAME").ToString)
                    Dim s As Integer = Len(stnName9.ToString)
                    If stnName9 <> "" Then stnName9 = stnName9.ToString.Substring(0, IIf(StnNameLen <= s, StnNameLen, s))
                    stnshName9 = IIf(dtTagStone.Rows(K)("STNSHSUBITEMNAME").ToString <> "", dtTagStone.Rows(K)("STNSHSUBITEMNAME").ToString, dtTagStone.Rows(K)("STNSHITEMNAME").ToString)
                    Dim s1 As Integer = Len(stnshName9.ToString)
                    If stnshName9 <> "" Then stnshName9 = stnshName9.ToString.Substring(0, IIf(StnNameLen <= s1, StnNameLen, s1))
                    stoneUnit9 = dtTagStone.Rows(K)("STONEUNIT").ToString
                    stoneRate9 = IIf(Val(dtTagStone.Rows(K)("STNRATE").ToString) = 0, dtTagStone.Rows(K)("STNAMT").ToString, dtTagStone.Rows(K)("STNRATE").ToString)
                End If

                If K = 9 Then
                    stnWt10 = IIf(Val(dtTagStone.Rows(K)("STNWT").ToString) > 0,
                   Format(Math.Round(Val(dtTagStone.Rows(K)("STNWT").ToString), 2), "0.00") _
                    , "")
                    stnPcs10 = IIf(Val(dtTagStone.Rows(K)("STNPCS").ToString) > 0, dtTagStone.Rows(K)("STNPCS").ToString, "")
                    stnName10 = IIf(dtTagStone.Rows(K)("STNSUBITEMNAME").ToString <> "", dtTagStone.Rows(K)("STNSUBITEMNAME").ToString, dtTagStone.Rows(K)("STNITEMNAME").ToString)
                    Dim s As Integer = Len(stnName10.ToString)
                    If stnName10 <> "" Then stnName10 = stnName10.ToString.Substring(0, IIf(StnNameLen <= s, StnNameLen, s))
                    stnshName10 = IIf(dtTagStone.Rows(K)("STNSHSUBITEMNAME").ToString <> "", dtTagStone.Rows(K)("STNSHSUBITEMNAME").ToString, dtTagStone.Rows(K)("STNSHITEMNAME").ToString)
                    Dim s1 As Integer = Len(stnshName10.ToString)
                    If stnshName10 <> "" Then stnshName10 = stnshName10.ToString.Substring(0, IIf(StnNameLen <= s1, StnNameLen, s1))
                    stoneUnit10 = dtTagStone.Rows(K)("STONEUNIT").ToString
                    stoneRate10 = IIf(Val(dtTagStone.Rows(K)("STNRATE").ToString) = 0, dtTagStone.Rows(K)("STNAMT").ToString, dtTagStone.Rows(K)("STNRATE").ToString)
                End If

                If K = 10 Then
                    stnWt11 = IIf(Val(dtTagStone.Rows(K)("STNWT").ToString) > 0,
                   Format(Math.Round(Val(dtTagStone.Rows(K)("STNWT").ToString), 2), "0.00") _
                    , "")
                    stnPcs11 = IIf(Val(dtTagStone.Rows(K)("STNPCS").ToString) > 0, dtTagStone.Rows(K)("STNPCS").ToString, "")
                    stnName11 = IIf(dtTagStone.Rows(K)("STNSUBITEMNAME").ToString <> "", dtTagStone.Rows(K)("STNSUBITEMNAME").ToString, dtTagStone.Rows(K)("STNITEMNAME").ToString)
                    Dim s As Integer = Len(stnName11.ToString)
                    If stnName11 <> "" Then stnName11 = stnName11.ToString.Substring(0, IIf(StnNameLen <= s, StnNameLen, s))
                    stnshName11 = IIf(dtTagStone.Rows(K)("STNSHSUBITEMNAME").ToString <> "", dtTagStone.Rows(K)("STNSHSUBITEMNAME").ToString, dtTagStone.Rows(K)("STNSHITEMNAME").ToString)
                    Dim s1 As Integer = Len(stnshName11.ToString)
                    If stnshName11 <> "" Then stnshName11 = stnshName11.ToString.Substring(0, IIf(StnNameLen <= s1, StnNameLen, s1))
                    stoneUnit11 = dtTagStone.Rows(K)("STONEUNIT").ToString
                    stoneRate11 = IIf(Val(dtTagStone.Rows(K)("STNRATE").ToString) = 0, dtTagStone.Rows(K)("STNAMT").ToString, dtTagStone.Rows(K)("STNRATE").ToString)
                End If

                If K = 11 Then
                    stnWt12 = IIf(Val(dtTagStone.Rows(K)("STNWT").ToString) > 0,
                   Format(Math.Round(Val(dtTagStone.Rows(K)("STNWT").ToString), 2), "0.00") _
                    , "")
                    stnPcs12 = IIf(Val(dtTagStone.Rows(K)("STNPCS").ToString) > 0, dtTagStone.Rows(K)("STNPCS").ToString, "")
                    stnName12 = IIf(dtTagStone.Rows(K)("STNSUBITEMNAME").ToString <> "", dtTagStone.Rows(K)("STNSUBITEMNAME").ToString, dtTagStone.Rows(K)("STNITEMNAME").ToString)
                    Dim s As Integer = Len(stnName12.ToString)
                    If stnName12 <> "" Then stnName12 = stnName12.ToString.Substring(0, IIf(StnNameLen <= s, StnNameLen, s))
                    stnshName12 = IIf(dtTagStone.Rows(K)("STNSHSUBITEMNAME").ToString <> "", dtTagStone.Rows(K)("STNSHSUBITEMNAME").ToString, dtTagStone.Rows(K)("STNSHITEMNAME").ToString)
                    Dim s1 As Integer = Len(stnshName12.ToString)
                    If stnshName12 <> "" Then stnshName12 = stnshName12.ToString.Substring(0, IIf(StnNameLen <= s1, StnNameLen, s1))
                    stoneUnit12 = dtTagStone.Rows(K)("STONEUNIT").ToString
                    stoneRate12 = IIf(Val(dtTagStone.Rows(K)("STNRATE").ToString) = 0, dtTagStone.Rows(K)("STNAMT").ToString, dtTagStone.Rows(K)("STNRATE").ToString)
                End If

            Next

            ''Multimetal Details
            If dtTagMutliMetal.Rows.Count Then
                For K As Integer = 0 To dtTagMutliMetal.Rows.Count - 1
                    If K > 4 Then Exit For
                    If K = 0 Then
                        Mcatname1 = dtTagMutliMetal.Rows(K)("CATNAME").ToString
                        McatShname1 = dtTagMutliMetal.Rows(K)("CATSHNAME").ToString
                        MGrsWt1 = IIf(Val(dtTagMutliMetal.Rows(K)("GRSWT").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("GRSWT").ToString), "0.000"), "")
                        MNetwt1 = IIf(Val(dtTagMutliMetal.Rows(K)("NETWT").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("NETWT").ToString), "0.000"),
                                      IIf(Val(dtTagMutliMetal.Rows(K)("GRSWT").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("GRSWT").ToString), "0.000"), ""))
                        Mwast1 = IIf(Val(dtTagMutliMetal.Rows(K)("WAST").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("WAST").ToString), "0.000"), "")
                        Mwastper1 = IIf(Val(dtTagMutliMetal.Rows(K)("WASTPER").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("WASTPER").ToString), "0.00"), "")
                        MMc1 = IIf(Val(dtTagMutliMetal.Rows(K)("MC").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("MC").ToString), "0.00"), "")
                        MMcGrm1 = IIf(Val(dtTagMutliMetal.Rows(K)("MCGRM").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("MCGRM").ToString), "0.00"), "")
                        MMcharge1 = IIf(Val(dtTagMutliMetal.Rows(K)("MCHARGE").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("MCHARGE").ToString), "0.00"), "")
                        MRate1 = IIf(Val(dtTagMutliMetal.Rows(K)("RATE").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("RATE").ToString), "0.00"), "")
                    End If
                    If K = 1 Then
                        MCatname2 = dtTagMutliMetal.Rows(K)("CATNAME").ToString
                        McatShname2 = dtTagMutliMetal.Rows(K)("CATSHNAME").ToString
                        MGrsWt2 = IIf(Val(dtTagMutliMetal.Rows(K)("GRSWT").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("GRSWT").ToString), "0.000"), "")
                        MNetwt2 = IIf(Val(dtTagMutliMetal.Rows(K)("NETWT").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("NETWT").ToString), "0.000"),
                                      IIf(Val(dtTagMutliMetal.Rows(K)("GRSWT").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("GRSWT").ToString), "0.000"), ""))
                        Mwast2 = IIf(Val(dtTagMutliMetal.Rows(K)("WAST").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("WAST").ToString), "0.000"), "")
                        Mwastper2 = IIf(Val(dtTagMutliMetal.Rows(K)("WASTPER").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("WASTPER").ToString), "0.00"), "")
                        MMc2 = IIf(Val(dtTagMutliMetal.Rows(K)("MC").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("MC").ToString), "0.00"), "")
                        MMcGrm2 = IIf(Val(dtTagMutliMetal.Rows(K)("MCGRM").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("MCGRM").ToString), "0.00"), "")
                        MMcharge2 = IIf(Val(dtTagMutliMetal.Rows(K)("MCHARGE").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("MCHARGE").ToString), "0.00"), "")
                        MRate2 = IIf(Val(dtTagMutliMetal.Rows(K)("RATE").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("RATE").ToString), "0.00"), "")
                    End If
                    If K = 2 Then
                        MCatname3 = dtTagMutliMetal.Rows(K)("CATNAME").ToString
                        McatShname3 = dtTagMutliMetal.Rows(K)("CATSHNAME").ToString
                        MGrsWt3 = IIf(Val(dtTagMutliMetal.Rows(K)("GRSWT").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("GRSWT").ToString), "0.000"), "")
                        MNetwt3 = IIf(Val(dtTagMutliMetal.Rows(K)("NETWT").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("NETWT").ToString), "0.000"),
                                      IIf(Val(dtTagMutliMetal.Rows(K)("GRSWT").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("GRSWT").ToString), "0.000"), ""))
                        Mwast3 = IIf(Val(dtTagMutliMetal.Rows(K)("WAST").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("WAST").ToString), "0.000"), "")
                        Mwastper3 = IIf(Val(dtTagMutliMetal.Rows(K)("WASTPER").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("WASTPER").ToString), "0.00"), "")
                        MMc3 = IIf(Val(dtTagMutliMetal.Rows(K)("MC").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("MC").ToString), "0.00"), "")
                        MMcGrm3 = IIf(Val(dtTagMutliMetal.Rows(K)("MCGRM").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("MCGRM").ToString), "0.00"), "")
                        MMcharge3 = IIf(Val(dtTagMutliMetal.Rows(K)("MCHARGE").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("MCHARGE").ToString), "0.00"), "")
                        MRate3 = IIf(Val(dtTagMutliMetal.Rows(K)("RATE").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("RATE").ToString), "0.00"), "")
                    End If
                    If K = 3 Then
                        MCatname4 = dtTagMutliMetal.Rows(K)("CATNAME").ToString
                        McatShname4 = dtTagMutliMetal.Rows(K)("CATSHNAME").ToString
                        MGrsWt4 = IIf(Val(dtTagMutliMetal.Rows(K)("GRSWT").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("GRSWT").ToString), "0.000"), "")
                        MNetwt4 = IIf(Val(dtTagMutliMetal.Rows(K)("NETWT").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("NETWT").ToString), "0.000"),
                                      IIf(Val(dtTagMutliMetal.Rows(K)("GRSWT").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("GRSWT").ToString), "0.000"), ""))
                        Mwast4 = IIf(Val(dtTagMutliMetal.Rows(K)("WAST").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("WAST").ToString), "0.000"), "")
                        Mwastper4 = IIf(Val(dtTagMutliMetal.Rows(K)("WASTPER").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("WASTPER").ToString), "0.00"), "")
                        MMc4 = IIf(Val(dtTagMutliMetal.Rows(K)("MC").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("MC").ToString), "0.00"), "")
                        MMcGrm4 = IIf(Val(dtTagMutliMetal.Rows(K)("MCGRM").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("MCGRM").ToString), "0.00"), "")
                        MMcharge4 = IIf(Val(dtTagMutliMetal.Rows(K)("MCHARGE").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("MCHARGE").ToString), "0.00"), "")
                        MRate4 = IIf(Val(dtTagMutliMetal.Rows(K)("RATE").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("RATE").ToString), "0.00"), "")
                    End If
                    If K = 4 Then
                        MCatname5 = dtTagMutliMetal.Rows(K)("CATNAME").ToString
                        McatShname5 = dtTagMutliMetal.Rows(K)("CATSHNAME").ToString
                        MGrsWt5 = IIf(Val(dtTagMutliMetal.Rows(K)("GRSWT").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("GRSWT").ToString), "0.000"), "")
                        MNetwt5 = IIf(Val(dtTagMutliMetal.Rows(K)("NETWT").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("NETWT").ToString), "0.000"),
                                      IIf(Val(dtTagMutliMetal.Rows(K)("GRSWT").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("GRSWT").ToString), "0.000"), ""))
                        Mwast5 = IIf(Val(dtTagMutliMetal.Rows(K)("WAST").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("WAST").ToString), "0.000"), "")
                        Mwastper5 = IIf(Val(dtTagMutliMetal.Rows(K)("WASTPER").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("WASTPER").ToString), "0.00"), "")
                        MMc5 = IIf(Val(dtTagMutliMetal.Rows(K)("MC").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("MC").ToString), "0.00"), "")
                        MMcGrm5 = IIf(Val(dtTagMutliMetal.Rows(K)("MCGRM").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("MCGRM").ToString), "0.00"), "")
                        MMcharge5 = IIf(Val(dtTagMutliMetal.Rows(K)("MCHARGE").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("MCHARGE").ToString), "0.00"), "")
                        MRate5 = IIf(Val(dtTagMutliMetal.Rows(K)("RATE").ToString) > 0, Format(Val(dtTagMutliMetal.Rows(K)("RATE").ToString), "0.00"), "")
                    End If
                Next
            End If


            ''HALLMARK DETAILS
            If dtTagHmDetails.Rows.Count > 0 Then
                For cnt As Integer = 0 To dtTagHmDetails.Rows.Count - 1
                    If cnt > 5 Then Exit For
                    If cnt = 0 Then
                        HUID1 = dtTagHmDetails.Rows(cnt).Item("HM_BILLNO").ToString
                    End If
                    If cnt = 1 Then
                        HUID2 = dtTagHmDetails.Rows(cnt).Item("HM_BILLNO").ToString
                    End If
                    If cnt = 2 Then
                        HUID3 = dtTagHmDetails.Rows(cnt).Item("HM_BILLNO").ToString
                    End If
                    If cnt = 3 Then
                        HUID4 = dtTagHmDetails.Rows(cnt).Item("HM_BILLNO").ToString
                    End If
                    If cnt = 4 Then
                        HUID5 = dtTagHmDetails.Rows(cnt).Item("HM_BILLNO").ToString
                    End If
                    If cnt = 5 Then
                        HUID6 = dtTagHmDetails.Rows(cnt).Item("HM_BILLNO").ToString
                    End If
                Next
            End If

            NARRATION = dtTag.Rows(0)("NARRATION").ToString
            TagLotno = dtTag.Rows(0)("LOTNO").ToString
            OrdNo = dtTag.Rows(0)("ORDNO").ToString
            ItemName = dtTag.Rows(0)("ITEMNAME").ToString
            SubItemName = dtTag.Rows(0)("SUBITEMNAME").ToString
            ItemShortName = dtTag.Rows(0)("ITEMSHORTNAME").ToString
            SubItemShortName = dtTag.Rows(0)("SUBITEMSHORTNAME").ToString
            ItemCtrName = dtTag.Rows(0)("ITEMCTRNAME").ToString
            ItemCtrId = dtTag.Rows(0)("ITEMCTRID").ToString
            Pcs = IIf(Val(dtTag.Rows(0)("PCS").ToString) > 0, dtTag.Rows(0)("PCS").ToString, "")
            GrsWt = IIf(Val(dtTag.Rows(0)("GRSWT").ToString) > 0, dtTag.Rows(0)("GRSWT").ToString, "")
            AvgWt = IIf(Val(dtTag.Rows(0)("AVGWT").ToString) > 0, dtTag.Rows(0)("AVGWT").ToString, "")
            NetWt = IIf(Val(dtTag.Rows(0)("NETWT").ToString) > 0, dtTag.Rows(0)("NETWT").ToString, "") 'dtTag.Rows(0)("NETWT").ToString
            LessWt = IIf(Val(dtTag.Rows(0)("LESSWT").ToString) > 0, dtTag.Rows(0)("LESSWT").ToString, "") ' dtTag.Rows(0)("LESSWT").ToString
            ExtraWt = IIf(Val(dtTag.Rows(0)("EXTRAWT").ToString) > 0, dtTag.Rows(0)("EXTRAWT").ToString, "") 'dtTag.Rows(0)("EXTRAWT").ToString
            If WastMask Then
                MAXWASTPER = dtTag.Rows(0)("MAXWASTPER").ToString
            Else
                MAXWASTPER = IIf(Val(dtTag.Rows(0)("MAXWASTPER").ToString) > 0, dtTag.Rows(0)("MAXWASTPER").ToString, "") 'dtTag.Rows(0)("MAXWASTPER").ToString
            End If
            MAXWAST = IIf(Val(dtTag.Rows(0)("MAXWAST").ToString) > 0, dtTag.Rows(0)("MAXWAST").ToString, "") ' dtTag.Rows(0)("MAXWAST").ToString
            MAXMCGRM = IIf(Val(dtTag.Rows(0)("MAXMCGRM").ToString) > 0, dtTag.Rows(0)("MAXMCGRM").ToString, "") 'dtTag.Rows(0)("MAXMCGRM").ToString
            MAXMC = IIf(Val(dtTag.Rows(0)("MAXMC").ToString) > 0, Math.Round(Val(dtTag.Rows(0)("MAXMC").ToString), 0), "") 'dtTag.Rows(0)("MAXMC").ToString
            stnAmt = IIf(Val(dtTag.Rows(0)("STNAMT").ToString) > 0, dtTag.Rows(0)("STNAMT").ToString, "")
            stnWt = IIf(Val(dtTag.Rows(0)("STNWT").ToString) > 0, dtTag.Rows(0)("STNWT").ToString, "") '  dtTag.Rows(0)("STNWT").ToString, "") dtTag.Rows(0)("STNWT").ToString
            stnPcs = IIf(Val(dtTag.Rows(0)("STNPCS").ToString) > 0, dtTag.Rows(0)("STNPCS").ToString, "") 'dtTag.Rows(0)("STNPCS").ToString
            stnName = dtTag.Rows(0)("STNNAME").ToString
            DiaAmt = IIf(Val(dtTag.Rows(0)("DIAAMT").ToString) > 0, dtTag.Rows(0)("DIAAMT").ToString, "") 'dtTag.Rows(0)("DIAAMT").ToString
            DiaWt = IIf(Val(dtTag.Rows(0)("DIAWT").ToString) > 0, dtTag.Rows(0)("DIAWT").ToString, "")  'dtTag.Rows(0)("DIAWT").ToString, "")  dtTag.Rows(0)("DIAWT").ToString
            BDSWt = IIf(Val(dtTag.Rows(0)("BDSWT").ToString) > 0, dtTag.Rows(0)("BDSWT").ToString, "") 'dtTag.Rows(0)("DIAWT").ToString
            DIAPS = IIf(Val(dtTag.Rows(0)("DIAPS").ToString) > 0, dtTag.Rows(0)("DIAPS").ToString, "") 'dtTag.Rows(0)("DIAPS").ToString
            DiaName = dtTag.Rows(0)("DIANAME").ToString
            DiaShName = IIf(dtTag.Rows(0)("DIASHNAME").ToString <> "", dtTag.Rows(0)("DIASHNAME").ToString, "")
            DiaChar = IIf(dtTag.Rows(0)("DIACHAR").ToString <> "", dtTag.Rows(0)("DIACHAR").ToString, "")
            Rate = IIf(Val(dtTag.Rows(0)("RATE").ToString) > 0, dtTag.Rows(0)("RATE").ToString, "") 'dtTag.Rows(0)("RATE").ToString
            BoardRate = IIf(Val(dtTag.Rows(0)("BOARDRATE").ToString) > 0, Format(Val(dtTag.Rows(0)("BOARDRATE").ToString), "0"), "") 'dtTag.Rows(0)("RATE").ToString
            SalValue = IIf(Val(dtTag.Rows(0)("SALVALUE").ToString) > 0, dtTag.Rows(0)("SALVALUE").ToString, "") 'dtTag.Rows(0)("SALVALUE").ToString
            ItemType = dtTag.Rows(0)("ITEMTYPE").ToString
            MetalId = dtTag.Rows(0)("METALID").ToString
            ItemSize = dtTag.Rows(0)("ITEMSIZE").ToString
            DesginerName = dtTag.Rows(0)("DESIGNERNAME").ToString
            DesginerId = dtTag.Rows(0)("DESIGNERID").ToString
            Seal = IIf(dtTag.Rows(0)("SEAL").ToString <> "", dtTag.Rows(0)("SEAL").ToString, "")
            TableCode = dtTag.Rows(0)("TABLECODE").ToString
            DD = dtTag.Rows(0)("DD").ToString
            MM = dtTag.Rows(0)("MM").ToString
            YY = dtTag.Rows(0)("YY").ToString

            'systemName
            'strSql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='BARCODE" & systemName & "'"
            'Dim barPrinter As String = objGPack.GetSqlValue(strSql, "", "")
            'If barPrinter = "" Then
            '    barPrinter = System.Windows.Forms.SystemInformation.ComputerName
            '    barPrinter = "\\" & barPrinter & "\BARCODE" & systemName
            'End If
            Dim barRead As String = ""

            Dim dtTemplate As DataTable
            strSql = "SELECT * FROM " & cnAdminDb & "..BARCODESETTING WHERE METALID='" & MetalId.ToString & "' AND ISNULL(FILENAME,'')<>''"
            cmd = New OleDbCommand(strSql, cn)
            da = New OleDbDataAdapter(cmd)
            dtTemplate = New DataTable
            da.Fill(dtTemplate)

            For k As Integer = 0 To dtTemplate.Rows.Count - 1
                Dim strCond As String = ""
                strCond = dtTemplate.Rows(k)("DESCRIPTION").ToString
                strSql = "SELECT 1 FROM TEMPTABLEDB..[TEMPBARCODE" & systemName & "SINGLE] WHERE " & strCond & " AND METALID='" & MetalId.ToString & "' "
                If Val(objGPack.GetSqlValue(strSql, , "").ToString) > 0 Then
                    If dtTemplate.Rows(k)("FILENAME").ToString <> "" Then
                        barRead = Application.StartupPath & "\BARCODE\" & dtTemplate.Rows(k)("FILENAME").ToString & ".PRN"
                        'Exit For
                    End If
                End If
            Next
            If barRead = "" Then
                If MetalId.ToString <> "" Then
                    barRead = Application.StartupPath & "\BARCODE\" & "BARCODE" & MetalId & ".PRN"
                Else
                    barRead = Application.StartupPath & "\BARCODE\" & "BARCODE1.PRN"
                End If
            End If
            If IO.Directory.Exists(Application.StartupPath & "\BARCODE") = False Then
                MsgBox("Directory not Exists..." & vbCrLf & "Directory Name : " & Application.StartupPath & "\BARCODE", MsgBoxStyle.Information)
                Exit Function
            End If
            If IO.File.Exists(barRead) = False Then
                MsgBox("Barcode Template not Found..." & vbCrLf & "File Name : " & barRead.ToString, MsgBoxStyle.Information)
                Exit Function
            End If
            Dim barWrite As String = Application.StartupPath & "\BARCODE\" & "DUPLICATE" & MetalId & systemName & ".MEM"

            Dim fileReader As New System.IO.StreamReader(barRead)
            Dim fileWriter As New System.IO.StreamWriter(barWrite)

            Dim StrRead As String
            While fileReader.EndOfStream = False
                StrRead = fileReader.ReadLine
                Dim _addline As Boolean = True
                'DESCRIPTION
                If StrRead.Contains("TAGNO") Then
                    StrRead = StrRead.Replace("TAGNO", ItemId & "-" & Tagno)
                    _addline = True
                End If
                If StrRead.Contains("SUBITEMSHORTNAME") Then
                    StrRead = StrRead.Replace("SUBITEMSHORTNAME", SubItemShortName)
                    If SubItemShortName.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("ITEMSHORTNAME") Then
                    StrRead = StrRead.Replace("ITEMSHORTNAME", ItemShortName)
                    If ItemShortName.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("ITEMCTRNAME") Then
                    StrRead = StrRead.Replace("ITEMCTRNAME", ItemCtrName)
                    If ItemCtrName.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("ITEMCTRID") Then
                    StrRead = StrRead.Replace("ITEMCTRID", ItemCtrId)
                    If ItemCtrId.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("SUBITEMNAME") Then
                    StrRead = StrRead.Replace("SUBITEMNAME", SubItemName)
                    If SubItemName.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("ITEMNAME") Then
                    StrRead = StrRead.Replace("ITEMNAME", ItemName)
                    If ItemName.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("ORDNO") Then
                    StrRead = StrRead.Replace("ORDNO", OrdNo)
                    If OrdNo.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("DIASHNAME") Then
                    StrRead = StrRead.Replace("DIASHNAME", DiaShName)
                    If DiaShName.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("DIACHAR") Then
                    StrRead = StrRead.Replace("DIACHAR", DiaChar)
                    If DiaChar.ToString <> "" Then _addline = True Else _addline = False
                End If

                ''MultiMetal DETAILS
                If StrRead.Contains("MCATNAME1") Then
                    If Mcatname1.ToString <> "" Then StrRead = StrRead.Replace("MCATNAME1", Mcatname1.ToString) : _addline = True Else _addline = False
                End If
                If StrRead.Contains("MCATSHNAME1") Then
                    If McatShname1.ToString <> "" Then StrRead = StrRead.Replace("MCATSHNAME1", McatShname1.ToString) : _addline = True Else _addline = False
                End If
                If StrRead.Contains("MGRSWT1") Then
                    StrRead = StrRead.Replace("MGRSWT1", MGrsWt1.ToString)
                    If MGrsWt1.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MNETWT1") Then
                    StrRead = StrRead.Replace("MNETWT1", MNetwt1.ToString)
                    If MNetwt1.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MWAST1") Then
                    StrRead = StrRead.Replace("MWAST1", Mwast1.ToString)
                    If Mwast1.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MWASTPER1") Then
                    StrRead = StrRead.Replace("MWASTPER1", Mwastper1.ToString)
                    If Mwastper1.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MMC1") Then
                    StrRead = StrRead.Replace("MMC1", MMc1.ToString)
                    If MMc1.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MMCGRM1") Then
                    StrRead = StrRead.Replace("MMCGRM1", MMcGrm1.ToString)
                    If MMcGrm1.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MMCHARGE1") Then
                    StrRead = StrRead.Replace("MMCHARGE1", MMcharge1.ToString)
                    If MMcharge1.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MRATE1") Then
                    StrRead = StrRead.Replace("MRATE1", MRate1.ToString)
                    If MRate1.ToString <> "" Then _addline = True Else _addline = False
                End If

                If StrRead.Contains("MCATNAME2") Then
                    If MCatname2.ToString <> "" Then StrRead = StrRead.Replace("MCATNAME2", MCatname2.ToString)
                    If MCatname2.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MCATSHNAME2") Then
                    If McatShname2.ToString <> "" Then StrRead = StrRead.Replace("MCATSHNAME2", McatShname2.ToString)
                    If McatShname2.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MGRSWT2") Then
                    StrRead = StrRead.Replace("MGRSWT2", MGrsWt2.ToString)
                    If MGrsWt2.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MNETWT2") Then
                    StrRead = StrRead.Replace("MNETWT2", MNetwt2.ToString)
                    If MNetwt2.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MWAST2") Then
                    StrRead = StrRead.Replace("MWAST2", Mwast2.ToString)
                    If Mwast2.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MWASTPER2") Then
                    StrRead = StrRead.Replace("MWASTPER2", Mwastper2.ToString)
                    If Mwastper2.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MMC2") Then
                    StrRead = StrRead.Replace("MMC2", MMc2.ToString)
                    If MMc2.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MMCGRM2") Then
                    StrRead = StrRead.Replace("MMCGRM2", MMcGrm2.ToString)
                    If MMcGrm2.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MMCHARGE2") Then
                    StrRead = StrRead.Replace("MMCHARGE2", MMcharge2.ToString)
                    If MMcharge2.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MRATE2") Then
                    StrRead = StrRead.Replace("MRATE2", MRate2.ToString)
                    If MRate2.ToString <> "" Then _addline = True Else _addline = False
                End If

                If StrRead.Contains("MCATNAME3") Then
                    If MCatname3.ToString <> "" Then StrRead = StrRead.Replace("MCATNAME3", MCatname3.ToString)
                    If MCatname3.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MCATSHNAME3") Then
                    If McatShname3.ToString <> "" Then StrRead = StrRead.Replace("MCATSHNAME3", McatShname3.ToString)
                    If McatShname3.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MGRSWT3") Then
                    StrRead = StrRead.Replace("MGRSWT3", MGrsWt3.ToString)
                    If MGrsWt3.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MNETWT3") Then
                    StrRead = StrRead.Replace("MNETWT3", MNetwt3.ToString)
                    If MNetwt3.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MWAST3") Then
                    StrRead = StrRead.Replace("MWAST3", Mwast3.ToString)
                    If Mwast3.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MWASTPER3") Then
                    StrRead = StrRead.Replace("MWASTPER3", Mwastper3.ToString)
                    If Mwastper3.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MMC3") Then
                    StrRead = StrRead.Replace("MMC3", MMc3.ToString)
                    If MMc3.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MMCGRM3") Then
                    StrRead = StrRead.Replace("MMCGRM3", MMcGrm3.ToString)
                    If MMcGrm3.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MMCHARGE3") Then
                    StrRead = StrRead.Replace("MMCHARGE3", MMcharge3.ToString)
                    If MMcharge3.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MRATE3") Then
                    StrRead = StrRead.Replace("MRATE3", MRate3.ToString)
                    If MRate3.ToString <> "" Then _addline = True Else _addline = False
                End If

                If StrRead.Contains("MCATNAME4") Then
                    If MCatname4.ToString <> "" Then StrRead = StrRead.Replace("MCATNAME4", MCatname4.ToString)
                    If MCatname4.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MCATSHNAME4") Then
                    If McatShname4.ToString <> "" Then StrRead = StrRead.Replace("MCATSHNAME4", McatShname4.ToString)
                    If McatShname4.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MGRSWT4") Then
                    StrRead = StrRead.Replace("MGRSWT4", MGrsWt4.ToString)
                    If MGrsWt4.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MNETWT4") Then
                    StrRead = StrRead.Replace("MNETWT4", MNetwt4.ToString)
                    If MNetwt4.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MWAST4") Then
                    StrRead = StrRead.Replace("MWAST4", Mwast4.ToString)
                    If Mwast4.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MWASTPER4") Then
                    StrRead = StrRead.Replace("MWASTPER4", Mwastper4.ToString)
                    If Mwastper4.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MMC4") Then
                    StrRead = StrRead.Replace("MMC4", MMc4.ToString)
                    If MMc4.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MMCGRM4") Then
                    StrRead = StrRead.Replace("MMCGRM4", MMcGrm4.ToString)
                    If MMcGrm4.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MMCHARGE4") Then
                    StrRead = StrRead.Replace("MMCHARGE4", MMcharge4.ToString)
                    If MMcharge4.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MRATE4") Then
                    StrRead = StrRead.Replace("MRATE4", MRate4.ToString)
                    If MRate4.ToString <> "" Then _addline = True Else _addline = False
                End If

                If StrRead.Contains("MCATNAME5") Then
                    If MCatname5.ToString <> "" Then StrRead = StrRead.Replace("MCATNAME5", MCatname5.ToString)
                    If MCatname5.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MCATSHNAME5") Then
                    If McatShname5.ToString <> "" Then StrRead = StrRead.Replace("MCATSHNAME5", McatShname5.ToString)
                    If McatShname5.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MGRSWT5") Then
                    StrRead = StrRead.Replace("MGRSWT5", MGrsWt5.ToString)
                    If MGrsWt5.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MNETWT5") Then
                    StrRead = StrRead.Replace("MNETWT5", MNetwt5.ToString)
                    If MNetwt5.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MWAST5") Then
                    StrRead = StrRead.Replace("MWAST5", Mwast5.ToString)
                    If Mwast5.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MWASTPER5") Then
                    StrRead = StrRead.Replace("MWASTPER5", Mwastper5.ToString)
                    If Mwastper5.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MMC5") Then
                    StrRead = StrRead.Replace("MMC5", MMc5.ToString)
                    If MMc5.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MMCGRM5") Then
                    StrRead = StrRead.Replace("MMCGRM5", MMcGrm5.ToString)
                    If OrdNo.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MMCHARGE5") Then
                    StrRead = StrRead.Replace("MMCHARGE5", MMcharge5.ToString)
                    If MMcharge5.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MRATE5") Then
                    StrRead = StrRead.Replace("MRATE5", MRate5.ToString)
                    If MRate5.ToString <> "" Then _addline = True Else _addline = False
                End If
                ''END MultiMetal DETAILS

                'PCS & WT
                If StrRead.Contains("PCS") Then
                    If Pcs <> "" Then StrRead = StrRead.Replace("PCS", Pcs)
                    If Pcs.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("GRSWT") Then
                    If GrsWt <> "" Then StrRead = StrRead.Replace("GRSWT", GrsWt)
                    If GrsWt.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("AVGWT") Then
                    If AvgWt <> "" Then StrRead = StrRead.Replace("AVGWT", AvgWt)
                    If AvgWt.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("NETWT") Then
                    If NetWt <> "" Then StrRead = StrRead.Replace("NETWT", NetWt)
                    If NetWt.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("LESSWT") Then
                    If LessWt <> "" Then StrRead = StrRead.Replace("LESSWT", LessWt)
                    If LessWt.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("EXTRAWT") Then
                    StrRead = StrRead.Replace("EXTRAWT", ExtraWt)
                    If ExtraWt.ToString <> "" Then _addline = True Else _addline = False
                End If
                'VA
                If StrRead.Contains("MAXWASTPER") Then
                    StrRead = StrRead.Replace("MAXWASTPER", MAXWASTPER)
                    If MAXWASTPER.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MAXWAST") Then
                    StrRead = StrRead.Replace("MAXWAST", MAXWAST)
                    If MAXWAST.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MAXMCGRM") Then
                    StrRead = StrRead.Replace("MAXMCGRM", MAXMCGRM)
                    If MAXMCGRM.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("MAXMC") Then
                    StrRead = StrRead.Replace("MAXMC", MAXMC)
                    If MAXMC.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("ITEMTYPE") Then
                    StrRead = StrRead.Replace("ITEMTYPE", ItemType)
                    If ItemType.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("SALVALUE") Then
                    StrRead = StrRead.Replace("SALVALUE", SalValue)
                    If SalValue.ToString <> "" Then _addline = True Else _addline = False
                End If
                '    'STONE
                If StrRead.Contains("STNAMT") Then
                    StrRead = StrRead.Replace("STNAMT", stnAmt)
                    If stnAmt.ToString <> "" Then _addline = True Else _addline = False
                End If
                '    'STONE1
                If StrRead.Contains("ITEMSIZE") Then
                    StrRead = StrRead.Replace("ITEMSIZE", ItemSize)
                    If ItemSize.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("SEAL") Then
                    StrRead = StrRead.Replace("SEAL", Seal)
                    If Seal.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("DESIGNERNAME") Then
                    StrRead = StrRead.Replace("DESIGNERNAME", DesginerName)
                    If DesginerName.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("DESIGNERID") Then
                    StrRead = StrRead.Replace("DESIGNERID", DesginerId)
                    If DesginerId.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("TABLECODE") Then
                    StrRead = StrRead.Replace("TABLECODE", TableCode)
                    If TableCode.ToString <> "" Then _addline = True Else _addline = False
                End If

#Region "stndetails"
                If STNPRINT_NEW Then
                    If dtTagStone.Rows.Count > 0 Then
                        Dim cnt As Integer = 0
                        Dim _Stncnt As Integer = dtTagStone.Rows.Count
                        Dim _Stnlinecnt As Integer = 0
                        Dim _Stnlineprt As String = StrRead
                        Dim _Stnlineind As Integer = 0
                        Dim _Stnendlineind As Integer = 0
                        If StrRead.Contains("<STNNAME") Then
                            _Stnlineind = _Stnlineprt.IndexOf("<STNNAME")
                            _Stnendlineind = _Stnlineprt.IndexOf(">", _Stnlineind)
                            _Stnlineprt = _Stnlineprt.Substring(_Stnlineind, _Stnendlineind - _Stnlineind).ToString
                            _Stnlineprt = _Stnlineprt.Replace("<STNNAME", "").ToString
                            _Stnlineprt = _Stnlineprt.Replace(">", "").ToString
                            _Stnlinecnt = Val(_Stnlineprt.ToString)
                        ElseIf StrRead.Contains("<STNSHNAME") Then
                            _Stnlineind = _Stnlineprt.IndexOf("<STNSHNAME")
                            _Stnendlineind = _Stnlineprt.IndexOf(">", _Stnlineind)
                            _Stnlineprt = _Stnlineprt.Substring(_Stnlineind, _Stnendlineind - _Stnlineind).ToString
                            _Stnlineprt = _Stnlineprt.Replace("<STNSHNAME", "").ToString
                            _Stnlineprt = _Stnlineprt.Replace(">", "").ToString
                            _Stnlinecnt = Val(_Stnlineprt.ToString)
                        ElseIf StrRead.Contains("<STNPS") Then
                            _Stnlineind = _Stnlineprt.IndexOf("<STNPS")
                            _Stnendlineind = _Stnlineprt.IndexOf(">", _Stnlineind)
                            _Stnlineprt = _Stnlineprt.Substring(_Stnlineind, _Stnendlineind - _Stnlineind).ToString
                            _Stnlineprt = _Stnlineprt.Replace("<STNPS", "").ToString
                            _Stnlineprt = _Stnlineprt.Replace(">", "").ToString
                            _Stnlinecnt = Val(_Stnlineprt.ToString)
                        ElseIf StrRead.Contains("<STNWT") Then
                            _Stnlineind = _Stnlineprt.IndexOf("<STNWT")
                            _Stnendlineind = _Stnlineprt.IndexOf(">", _Stnlineind)
                            _Stnlineprt = _Stnlineprt.Substring(_Stnlineind, _Stnendlineind - _Stnlineind).ToString
                            _Stnlineprt = _Stnlineprt.Replace("<STNWT", "").ToString
                            _Stnlineprt = _Stnlineprt.Replace(">", "").ToString
                            _Stnlinecnt = Val(_Stnlineprt.ToString)
                        ElseIf StrRead.Contains("<STONEUNIT") Then
                            _Stnlineind = _Stnlineprt.IndexOf("<STONEUNIT")
                            _Stnendlineind = _Stnlineprt.IndexOf(">", _Stnlineind)
                            _Stnlineprt = _Stnlineprt.Substring(_Stnlineind, _Stnendlineind - _Stnlineind).ToString
                            _Stnlineprt = _Stnlineprt.Replace("<STONEUNIT", "").ToString
                            _Stnlineprt = _Stnlineprt.Replace(">", "").ToString
                            _Stnlinecnt = Val(_Stnlineprt.ToString)
                        ElseIf StrRead.Contains("<STNRATE") Then
                            _Stnlineind = _Stnlineprt.IndexOf("<STNRATE")
                            _Stnendlineind = _Stnlineprt.IndexOf(">", _Stnlineind)
                            _Stnlineprt = _Stnlineprt.Substring(_Stnlineind, _Stnendlineind - _Stnlineind).ToString
                            _Stnlineprt = _Stnlineprt.Replace("<STNRATE", "").ToString
                            _Stnlineprt = _Stnlineprt.Replace(">", "").ToString
                            _Stnlinecnt = Val(_Stnlineprt.ToString)
                        End If
                        If Val(_Stnlinecnt) > _Stncnt Then Continue While
                        For cntt As Integer = 0 To dtTagStone.Rows.Count - 1
                            cnt = cntt + 1
                            stnWt1 = IIf(Val(dtTagStone.Rows(cntt)("STNWT").ToString) > 0,
                        Format(Math.Round(Val(dtTagStone.Rows(cntt)("STNWT").ToString), 2), "0.00") _
                        , "")
                            stnPcs1 = IIf(Val(dtTagStone.Rows(cntt)("STNPCS").ToString) > 0, dtTagStone.Rows(cntt)("STNPCS").ToString, "")
                            stnName1 = IIf(dtTagStone.Rows(cntt)("STNSUBITEMNAME").ToString <> "", dtTagStone.Rows(cntt)("STNSUBITEMNAME").ToString, dtTagStone.Rows(cntt)("STNITEMNAME").ToString)
                            Dim s As Integer = Len(stnName1.ToString)
                            If stnName1 <> "" Then stnName1 = stnName1.ToString.Substring(0, IIf(StnNameLen <= s, StnNameLen, s))
                            stnshName1 = IIf(dtTagStone.Rows(cntt)("STNSHSUBITEMNAME").ToString <> "", dtTagStone.Rows(cntt)("STNSHSUBITEMNAME").ToString, dtTagStone.Rows(cntt)("STNSHITEMNAME").ToString)
                            Dim s1 As Integer = Len(stnshName1.ToString)
                            If stnshName1 <> "" Then stnshName1 = stnshName1.ToString.Substring(0, IIf(StnNameLen <= s1, StnNameLen, s1))
                            stoneUnit1 = dtTagStone.Rows(cntt)("STONEUNIT").ToString
                            stoneRate1 = IIf(Val(dtTagStone.Rows(cntt)("STNRATE").ToString) = 0, dtTagStone.Rows(cntt)("STNAMT").ToString, dtTagStone.Rows(cntt)("STNRATE").ToString)
                            stoneRate1 = IIf(Val(stoneRate1.ToString) > 0, stoneRate1.ToString, "")

                            If StrRead.Contains("<STNNAME" & cnt & ">") Then
                                StrRead = StrRead.Replace("<STNNAME" & cnt & ">", stnName1).ToString
                                If stnName1.ToString <> "" Then _addline = True Else _addline = False
                            End If
                            If StrRead.Contains("<STNSHNAME" & cnt & ">") Then
                                StrRead = StrRead.Replace("<STNSHNAME" & cnt & ">", stnshName1).ToString
                                If stnshName1.ToString <> "" Then _addline = True Else _addline = False
                            End If
                            If StrRead.Contains("<STNPS" & cnt & ">") Then
                                StrRead = StrRead.Replace("<STNPS" & cnt & ">", stnPcs1).ToString
                                If stnPcs1.ToString <> "" Then _addline = True Else _addline = False
                            End If
                            If StrRead.Contains("<STNWT" & cnt & ">") Then
                                StrRead = StrRead.Replace("<STNWT" & cnt & ">", stnWt1).ToString
                                If stnWt1.ToString <> "" Then _addline = True Else _addline = False
                            End If
                            If StrRead.Contains("<STONEUNIT" & cnt & ">") Then
                                StrRead = StrRead.Replace("<STONEUNIT" & cnt & ">", stoneUnit1).ToString
                                If stoneUnit1.ToString <> "" Then _addline = True Else _addline = False
                            End If
                            If StrRead.Contains("<STNRATE" & cnt & ">") Then
                                StrRead = StrRead.Replace("<STNRATE" & cnt & ">", stoneRate1).ToString
                                If stoneRate1.ToString <> "" Then _addline = True Else _addline = False
                            End If
                        Next
                    End If
                Else
                    If dtTagStone.Rows.Count > 0 Then
                        If StrRead.Contains("STNNAME10") Then
                            StrRead = StrRead.Replace("STNNAME10", stnName10).ToString : If stnName10.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNSHNAME10") Then
                            StrRead = StrRead.Replace("STNSHNAME10", stnshName10).ToString : If stnshName10.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNPS10") Then
                            StrRead = StrRead.Replace("STNPS10", stnPcs10).ToString : If stnPcs10.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNWT10") Then
                            StrRead = StrRead.Replace("STNWT10", stnWt10).ToString : If stnWt10.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STONEUNIT10") Then
                            StrRead = StrRead.Replace("STONEUNIT10", stoneUnit10).ToString : If stoneUnit10.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNRATE10") Then
                            StrRead = StrRead.Replace("STNRATE10", stoneRate10).ToString : If stoneRate10.ToString <> "" Then _addline = True Else _addline = False
                        End If

                        If StrRead.Contains("STNNAME11") Then
                            StrRead = StrRead.Replace("STNNAME11", stnName11).ToString : If stnName11.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNSHNAME11") Then
                            StrRead = StrRead.Replace("STNSHNAME11", stnshName11).ToString : If stnshName11.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNPS11") Then
                            StrRead = StrRead.Replace("STNPS11", stnPcs11).ToString : If stnPcs11.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNWT11") Then
                            StrRead = StrRead.Replace("STNWT11", stnWt11).ToString : If stnWt11.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STONEUNIT11") Then
                            StrRead = StrRead.Replace("STONEUNIT11", stoneUnit11).ToString : If stoneUnit11.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNRATE11") Then
                            StrRead = StrRead.Replace("STNRATE11", stoneRate11).ToString : If stoneRate11.ToString <> "" Then _addline = True Else _addline = False
                        End If

                        If StrRead.Contains("STNNAME12") Then
                            StrRead = StrRead.Replace("STNNAME12", stnName12).ToString : If stnName12.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNSHNAME12") Then
                            StrRead = StrRead.Replace("STNSHNAME12", stnshName12).ToString : If stnshName12.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNPS12") Then
                            StrRead = StrRead.Replace("STNPS12", stnPcs12).ToString : If stnPcs12.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNWT12") Then
                            StrRead = StrRead.Replace("STNWT12", stnWt12).ToString : If stnWt12.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STONEUNIT12") Then
                            StrRead = StrRead.Replace("STONEUNIT12", stoneUnit12).ToString : If stoneUnit12.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNRATE12") Then
                            StrRead = StrRead.Replace("STNRATE12", stoneRate12).ToString : If stoneRate12.ToString <> "" Then _addline = True Else _addline = False
                        End If

                        If StrRead.Contains("STNNAME1") Then
                            StrRead = StrRead.Replace("STNNAME1", stnName1).ToString : If stnName1.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNSHNAME1") Then
                            StrRead = StrRead.Replace("STNSHNAME1", stnshName1).ToString : If stnshName1.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNPS1") Then
                            StrRead = StrRead.Replace("STNPS1", stnPcs1).ToString : If stnPcs1.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNWT1") Then
                            StrRead = StrRead.Replace("STNWT1", stnWt1).ToString : If stnWt1.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STONEUNIT1") Then
                            StrRead = StrRead.Replace("STONEUNIT1", stoneUnit1).ToString : If stoneUnit1.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNRATE1") Then
                            StrRead = StrRead.Replace("STNRATE1", stoneRate1).ToString : If stoneRate1.ToString <> "" Then _addline = True Else _addline = False
                        End If

                        If StrRead.Contains("STNNAME2") Then
                            StrRead = StrRead.Replace("STNNAME2", stnName2).ToString : If stnName2.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNSHNAME2") Then
                            StrRead = StrRead.Replace("STNSHNAME2", stnshName2).ToString : If stnshName2.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNPS2") Then
                            StrRead = StrRead.Replace("STNPS2", stnPcs2).ToString : If stnPcs2.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNWT2") Then
                            StrRead = StrRead.Replace("STNWT2", stnWt2).ToString : If stnWt2.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STONEUNIT2") Then
                            StrRead = StrRead.Replace("STONEUNIT2", stoneUnit2).ToString : If stoneUnit2.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNRATE2") Then
                            StrRead = StrRead.Replace("STNRATE2", stoneRate2).ToString : If stoneRate2.ToString <> "" Then _addline = True Else _addline = False
                        End If

                        If StrRead.Contains("STNNAME3") Then
                            StrRead = StrRead.Replace("STNNAME3", stnName3).ToString : If stnName3.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNSHNAME3") Then
                            StrRead = StrRead.Replace("STNSHNAME3", stnshName3).ToString : If stnshName3.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNPS3") Then
                            StrRead = StrRead.Replace("STNPS3", stnPcs3).ToString : If stnPcs3.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNWT3") Then
                            StrRead = StrRead.Replace("STNWT3", stnWt3).ToString : If stnWt3.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STONEUNIT3") Then
                            StrRead = StrRead.Replace("STONEUNIT3", stoneUnit3).ToString : If stoneUnit3.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNRATE3") Then
                            StrRead = StrRead.Replace("STNRATE3", stoneRate3).ToString : If stoneRate3.ToString <> "" Then _addline = True Else _addline = False
                        End If

                        If StrRead.Contains("STNNAME4") Then
                            StrRead = StrRead.Replace("STNNAME4", stnName4).ToString : If stnName4.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNSHNAME4") Then
                            StrRead = StrRead.Replace("STNSHNAME4", stnshName4).ToString : If stnshName4.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNPS4") Then
                            StrRead = StrRead.Replace("STNPS4", stnPcs4).ToString : If stnPcs4.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNWT4") Then
                            StrRead = StrRead.Replace("STNWT4", stnWt4).ToString : If stnWt4.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STONEUNIT4") Then
                            StrRead = StrRead.Replace("STONEUNIT4", stoneUnit4).ToString : If stoneUnit4.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNRATE4") Then
                            StrRead = StrRead.Replace("STNRATE4", stoneRate4).ToString : If stoneRate4.ToString <> "" Then _addline = True Else _addline = False
                        End If

                        If StrRead.Contains("STNNAME5") Then
                            StrRead = StrRead.Replace("STNNAME5", stnName5).ToString : If stnName5.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNSHNAME5") Then
                            StrRead = StrRead.Replace("STNSHNAME5", stnshName5).ToString : If stnshName5.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNPS5") Then
                            StrRead = StrRead.Replace("STNPS5", stnPcs5).ToString : If stnPcs5.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNWT5") Then
                            StrRead = StrRead.Replace("STNWT5", stnWt5).ToString : If stnWt5.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STONEUNIT5") Then
                            StrRead = StrRead.Replace("STONEUNIT5", stoneUnit5).ToString : If stoneUnit5.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNRATE5") Then
                            StrRead = StrRead.Replace("STNRATE5", stoneRate5).ToString : If stoneRate5.ToString <> "" Then _addline = True Else _addline = False
                        End If

                        If StrRead.Contains("STNNAME6") Then
                            StrRead = StrRead.Replace("STNNAME6", stnName6).ToString : If stnName6.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNSHNAME6") Then
                            StrRead = StrRead.Replace("STNSHNAME6", stnshName6).ToString : If stnshName6.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNPS6") Then
                            StrRead = StrRead.Replace("STNPS6", stnPcs6).ToString : If stnPcs6.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNWT6") Then
                            StrRead = StrRead.Replace("STNWT6", stnWt6).ToString : If stnWt6.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STONEUNIT6") Then
                            StrRead = StrRead.Replace("STONEUNIT6", stoneUnit6).ToString : If stoneUnit6.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNRATE6") Then
                            StrRead = StrRead.Replace("STNRATE6", stoneRate6).ToString : If stoneRate6.ToString <> "" Then _addline = True Else _addline = False
                        End If

                        If StrRead.Contains("STNNAME7") Then
                            StrRead = StrRead.Replace("STNNAME7", stnName7).ToString : If stnName7.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNSHNAME7") Then
                            StrRead = StrRead.Replace("STNSHNAME7", stnshName7).ToString : If stnshName7.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNPS7") Then
                            StrRead = StrRead.Replace("STNPS7", stnPcs7).ToString : If stnPcs7.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNWT7") Then
                            StrRead = StrRead.Replace("STNWT7", stnWt7).ToString : If stnWt7.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STONEUNIT7") Then
                            StrRead = StrRead.Replace("STONEUNIT7", stoneUnit7).ToString : If stoneUnit7.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNRATE7") Then
                            StrRead = StrRead.Replace("STNRATE7", stoneRate7).ToString : If stoneRate7.ToString <> "" Then _addline = True Else _addline = False
                        End If

                        If StrRead.Contains("STNNAME8") Then
                            StrRead = StrRead.Replace("STNNAME8", stnName8).ToString : If stnName8.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNSHNAME8") Then
                            StrRead = StrRead.Replace("STNSHNAME8", stnshName8).ToString : If stnshName8.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNPS8") Then
                            StrRead = StrRead.Replace("STNPS8", stnPcs8).ToString : If stnPcs8.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNWT8") Then
                            StrRead = StrRead.Replace("STNWT8", stnWt8).ToString : If stnWt8.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STONEUNIT8") Then
                            StrRead = StrRead.Replace("STONEUNIT8", stoneUnit8).ToString : If stoneUnit8.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNRATE8") Then
                            StrRead = StrRead.Replace("STNRATE8", stoneRate8).ToString : If stoneRate8.ToString <> "" Then _addline = True Else _addline = False
                        End If

                        If StrRead.Contains("STNNAME9") Then
                            StrRead = StrRead.Replace("STNNAME9", stnName9).ToString : If stnName9.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNSHNAME9") Then
                            StrRead = StrRead.Replace("STNSHNAME9", stnshName9).ToString : If stnshName9.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNPS9") Then
                            StrRead = StrRead.Replace("STNPS9", stnPcs9).ToString : If stnPcs9.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNWT9") Then
                            StrRead = StrRead.Replace("STNWT9", stnWt9).ToString : If stnWt9.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STONEUNIT9") Then
                            StrRead = StrRead.Replace("STONEUNIT9", stoneUnit9).ToString : If stoneUnit9.ToString <> "" Then _addline = True Else _addline = False
                        End If
                        If StrRead.Contains("STNRATE9") Then
                            StrRead = StrRead.Replace("STNRATE9", stoneRate9).ToString : If stoneRate9.ToString <> "" Then _addline = True Else _addline = False
                        End If
                    End If
                End If
#End Region


                If StrRead.Contains("TOTSTNPS") Then
                    If dtTagStone.Rows.Count > 4 Then
                        If TotStnWt <> "" Or TotStnPcs <> "" Then
                            StrRead = StrRead.Replace("TOTSTNPS", TotStnPcs).ToString
                            StrRead = StrRead.Replace("TOTSTNWT", TotStnWt).ToString
                            _addline = True
                        Else
                            Continue While
                        End If
                    End If
                End If
                'BEEDS                
                If StrRead.Contains("BDSWT") Then
                    If BDSWt <> "" Then StrRead = StrRead.Replace("BDSWT", BDSWt)
                    If BDSWt.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("DIAPS") Then
                    If DIAPS <> "" Then StrRead = StrRead.Replace("DIAPS", DIAPS)
                    If DIAPS.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("DIAWT") Then
                    If DiaWt <> "" Then StrRead = StrRead.Replace("DIAWT", DiaWt)
                    If DiaWt.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("DIAAMT") Then
                    If DiaAmt <> "" Then StrRead = StrRead.Replace("DIAAMT", DiaAmt)
                    If DiaAmt.ToString <> "" Then _addline = True Else _addline = False
                End If

                If StrRead.Contains("STNPS") Then
                    If stnPcs <> "" Then StrRead = StrRead.Replace("STNPS", stnPcs)
                    If stnPcs.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("STNWT") Then
                    If stnWt <> "" Then StrRead = StrRead.Replace("STNWT", stnWt)
                    If stnWt.ToString <> "" Then _addline = True Else _addline = False
                End If
                If StrRead.Contains("STNAMT") Then
                    If stnAmt <> "" Then StrRead = StrRead.Replace("STNAMT", stnAmt)
                    If stnAmt.ToString <> "" Then _addline = True Else _addline = False
                End If

                If StrRead.Contains("NARRATION") Then
                    If NARRATION <> "" Then StrRead = StrRead.Replace("NARRATION", NARRATION)
                    If NARRATION.ToString <> "" Then _addline = True Else _addline = False
                End If

                If StrRead.Contains("BOARDRATE") Then
                    StrRead = StrRead.Replace("BOARDRATE", BoardRate)
                    If BoardRate.ToString <> "" Then _addline = True Else _addline = False
                End If

                If StrRead.Contains("RATE") Then
                    StrRead = StrRead.Replace("RATE", Rate)
                    If Rate.ToString <> "" Then _addline = True Else _addline = False
                End If


                If StrRead.Contains("DDMMYY") Then
                    If DD <> "" Then StrRead = StrRead.Replace("DDMMYY", DD & MM & YY) : _addline = True
                End If
                If StrRead.Contains("DDMM") Then
                    If DD <> "" Then StrRead = StrRead.Replace("DDMM", DD & MM) : _addline = True
                End If
                If StrRead.Contains("MMYY") Then
                    If MM <> "" Then StrRead = StrRead.Replace("MMYY", MM & YY) : _addline = True
                End If


                ''HALLMARK DETAILS
                If StrRead.Contains("HUID1") Then
                    If HUID1.ToString <> "" Then StrRead = StrRead.Replace("HUID1", HUID1.ToString) : _addline = True
                End If
                If StrRead.Contains("HUID2") Then
                    If HUID2.ToString <> "" Then StrRead = StrRead.Replace("HUID2", HUID2.ToString) : _addline = True
                End If
                If StrRead.Contains("HUID3") Then
                    If HUID3.ToString <> "" Then StrRead = StrRead.Replace("HUID3", HUID3.ToString) : _addline = True
                End If
                If StrRead.Contains("HUID4") Then
                    If HUID4.ToString <> "" Then StrRead = StrRead.Replace("HUID4", HUID4.ToString) : _addline = True
                End If
                If StrRead.Contains("HUID5") Then
                    If HUID5.ToString <> "" Then StrRead = StrRead.Replace("HUID5", HUID5.ToString) : _addline = True
                End If
                If StrRead.Contains("HUID6") Then
                    If HUID6.ToString <> "" Then StrRead = StrRead.Replace("HUID6", HUID6.ToString) : _addline = True
                End If

                If StrRead.Contains("LOTNO") Then
                    If TagLotno.ToString <> "" Then StrRead = StrRead.Replace("LOTNO", TagLotno.ToString) : _addline = True
                End If
                If _addline = True Then
                    fileWriter.WriteLine(StrRead)
                End If
            End While
            fileReader.Close()
            fileWriter.Close()
            Dim objBarcodePrint As New RawPrinterHelper
            RawPrinterHelper.SendFileToPrinter(barWrite)
            fileWriter.Dispose()
            My.Computer.FileSystem.DeleteFile(barWrite)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function
    Public Function FuncprintBarcode_Multi(ByVal ItemId As String, ByVal Tagno As String, ByVal PrintFormat As Double)
        Dim MetalId As String = ""
        Try
            Dim systemName As String = My.Computer.Name
            strSql = "IF OBJECT_ID('TEMPTABLEDB..TEMPBARCODE" & systemName & "') IS NOT NULL "
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPBARCODE" & systemName & " ("
            strSql += vbCrLf + " ITEMNAME,SUBITEMNAME,ITEMTYPE,METALID,TAGNO,ITEMID,SUBITEMID,TABLECODE,DESIGNERID,"
            strSql += vbCrLf + " PCS,GRSWT,NETWT,LESSWT,RATE,MAXWASTPER,MAXWAST,MAXMCGRM,MAXMC,SALVALUE,"
            strSql += vbCrLf + " ITEMTYPEID,STYLENO,STNWT,STNPCS,STNNAME,DIAWT,DIAPCS,DIANAME)"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " (SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID)ITEMNAME,"
            strSql += vbCrLf + " (SELECT TOP 1 SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID=T.SUBITEMID)SUBITEMNAME,"
            strSql += vbCrLf + " (SELECT TOP 1 NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID=T.ITEMTYPEID)ITEMTYPE,"
            strSql += vbCrLf + " (SELECT TOP 1 METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID)METALID,"
            strSql += vbCrLf + " TAGNO,ITEMID,SUBITEMID,TABLECODE,DESIGNERID,PCS,GRSWT,NETWT,LESSWT,"
            strSql += vbCrLf + " RATE,MAXWASTPER,MAXWAST,MAXMCGRM,MAXMC,SALVALUE,ITEMTYPEID,STYLENO"
            strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT DISTINCT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='T'))STNWT"
            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT DISTINCT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='T') GROUP BY TAGSNO,ITEMID)STNPCS,'' STNNAME"
            strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT DISTINCT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D') GROUP BY TAGSNO,ITEMID)DIAWT"
            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT DISTINCT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D') GROUP BY TAGSNO,ITEMID)DIAPCS,'' DIANAME"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + " WHERE ITEMID='" & ItemId & "' AND TAGNO='" & Tagno & "'"
            strSql += vbCrLf + " END"
            strSql += vbCrLf + " ELSE"
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " (SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID)ITEMNAME,"
            strSql += vbCrLf + " (SELECT TOP 1 SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID=T.SUBITEMID)SUBITEMNAME,"
            strSql += vbCrLf + " (SELECT TOP 1 NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID=T.ITEMTYPEID)ITEMTYPE,"
            strSql += vbCrLf + " (SELECT TOP 1 METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID)METALID,"
            strSql += vbCrLf + " TAGNO,ITEMID,SUBITEMID,TABLECODE,DESIGNERID,PCS,GRSWT,NETWT,LESSWT,"
            strSql += vbCrLf + " RATE,MAXWASTPER,MAXWAST,MAXMCGRM,MAXMC,SALVALUE,ITEMTYPEID,STYLENO"
            strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT DISTINCT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='T'))STNWT"
            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT DISTINCT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='T') GROUP BY TAGSNO,ITEMID)STNPCS,'' STNNAME"
            strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT DISTINCT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D') GROUP BY TAGSNO,ITEMID)DIAWT"
            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO=T.SNO AND STNITEMID IN (SELECT DISTINCT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D') GROUP BY TAGSNO,ITEMID)DIAPCS,'' DIANAME"
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPBARCODE" & systemName & ""
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + " WHERE ITEMID='" & ItemId & "' AND TAGNO='" & Tagno & "'"
            strSql += vbCrLf + " END"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            strSql = " SELECT COUNT(*) FROM TEMPTABLEDB..TEMPBARCODE" & systemName & " AS T"
            If Val(objGPack.GetSqlValue(strSql, , "").ToString) < PrintFormat Then Exit Function

            strSql = " SELECT * FROM TEMPTABLEDB..TEMPBARCODE" & systemName & " AS T"
            Dim dtTag As DataTable
            cmd = New OleDbCommand(strSql, cn)
            da = New OleDbDataAdapter(cmd)
            dtTag = New DataTable
            da.Fill(dtTag)
            If dtTag.Rows.Count = 0 Then Exit Function

            MetalId = dtTag.Columns.Contains("METALID").ToString
            'systemName            
            Dim barRead As String = ""
            If MetalId.ToString <> "" Then
                barRead = Application.StartupPath & "\" & "BARCODE" & MetalId & ".PRN"
            Else
                barRead = Application.StartupPath & "\" & "BARCODE1.PRN"
            End If
            If IO.File.Exists(barRead) = False Then
                MsgBox("Barcode Template not Found...", MsgBoxStyle.Information)
                Exit Function
            End If
            Dim barWrite As String = Application.StartupPath & "\DUPLICATE" & MetalId & systemName & "_ORG" & ".MEM"

            Dim fileReader As New System.IO.StreamReader(barRead)
            Dim fileWriter As New System.IO.StreamWriter(barWrite)
            Dim StrRead As String
            While fileReader.EndOfStream = False
                Dim Isprinted As Boolean = False
                StrRead = fileReader.ReadLine
                For k As Integer = 1 To dtTag.Rows.Count
                    With dtTag
                        'DESCRIPTION
                        If StrRead.Contains("TAGNO" & k.ToString) Then
                            fileWriter.WriteLine(StrRead.Replace("TAGNO" & k.ToString, .Rows(k - 1)("ITEMID").ToString & .Rows(k - 1)("TAGNO").ToString))
                            Isprinted = True : Exit For
                        ElseIf StrRead.Contains("ITEMNAME" & k.ToString) Then
                            fileWriter.WriteLine(StrRead.Replace("ITEMNAME" & k.ToString, .Rows(k - 1)("ITEMNAME").ToString))
                            Isprinted = True : Exit For
                        ElseIf StrRead.Contains("SUBITEMNAME" & k.ToString) Then
                            fileWriter.WriteLine(StrRead.Replace("SUBITEMNAME" & k.ToString, .Rows(k - 1)("SUBITEMNAME").ToString))
                            Isprinted = True : Exit For
                            'WT 
                        ElseIf StrRead.Contains("GRSWT" & k.ToString) Then
                            If .Rows(k - 1)("GRSWT").ToString <> "" Then fileWriter.WriteLine(StrRead.Replace("GRSWT" & k.ToString, .Rows(k - 1)("GRSWT").ToString))
                            Isprinted = True : Exit For
                        ElseIf StrRead.Contains("NETWT" & k.ToString) Then
                            If .Rows(k - 1)("NETWT").ToString <> "" Then fileWriter.WriteLine(StrRead.Replace("NETWT" & k.ToString, .Rows(k - 1)("NETWT").ToString))
                            Isprinted = True : Exit For
                        ElseIf StrRead.Contains("LESSWT" & k.ToString) Then
                            If .Rows(k - 1)("LESSWT").ToString <> "" Then fileWriter.WriteLine(StrRead.Replace("LESSWT" & k.ToString, .Rows(k - 1)("LESSWT").ToString))
                            Isprinted = True : Exit For
                            'VA
                        ElseIf StrRead.Contains("WASTPER" & k.ToString) Then
                            If .Rows(k - 1)("WASTPER").ToString <> "" Then fileWriter.WriteLine(StrRead.Replace("WASTPER" & k.ToString, .Rows(k - 1)("WASTPER").ToString))
                            Isprinted = True : Exit For
                        ElseIf StrRead.Contains("WASTWT" & k.ToString) Then
                            If .Rows(k - 1)("WASTWT").ToString <> "" Then fileWriter.WriteLine(StrRead.Replace("WASTWT" & k.ToString, .Rows(k - 1)("WASTWT").ToString))
                            Isprinted = True : Exit For
                        ElseIf StrRead.Contains("MCGRM" & k.ToString) Then
                            If .Rows(k - 1)("MCGRM").ToString <> "" Then fileWriter.WriteLine(StrRead.Replace("MCGRM" & k.ToString, .Rows(k - 1)("MCGRM").ToString))
                            Isprinted = True : Exit For
                        ElseIf StrRead.Contains("MCAMT" & k.ToString) Then
                            If .Rows(k - 1)("MCAMT").ToString <> "" Then fileWriter.WriteLine(StrRead.Replace("MCAMT" & k.ToString, .Rows(k - 1)("MCAMT").ToString))
                            Isprinted = True : Exit For
                        ElseIf StrRead.Contains("ITEMTYPE" & k.ToString) Then
                            If .Rows(k - 1)("ITEMTYPE").ToString <> "" Then fileWriter.WriteLine(StrRead.Replace("ITEMTYPE" & k.ToString, .Rows(k - 1)("ITEMTYPE").ToString))
                            Isprinted = True : Exit For
                        ElseIf StrRead.Contains("SALVALUE" & k.ToString) Then
                            If .Rows(k - 1)("SALVALUE").ToString <> "" Then fileWriter.WriteLine(StrRead.Replace("SALVALUE" & k.ToString, .Rows(k - 1)("SALVALUE").ToString))
                            Isprinted = True : Exit For
                        ElseIf StrRead.Contains("RATE" & k.ToString) Then
                            If .Rows(k - 1)("RATE").ToString <> "" Then fileWriter.WriteLine(StrRead.Replace("RATE" & k.ToString, .Rows(k - 1)("RATE").ToString))
                            Isprinted = True : Exit For
                            'STONE
                        ElseIf StrRead.Contains("STNAMT" & k.ToString) Then
                            If .Rows(k - 1)("STNAMT").ToString <> "" Then fileWriter.WriteLine(StrRead.Replace("STNAMT" & k.ToString, .Rows(k - 1)("STNAMT").ToString))
                            Isprinted = True : Exit For
                        ElseIf StrRead.Contains("STNWT" & k.ToString) Then
                            If .Rows(k - 1)("STNWT").ToString <> "" Then fileWriter.WriteLine(StrRead.Replace("STNWT" & k.ToString, .Rows(k - 1)("STNWT").ToString))
                            Isprinted = True : Exit For
                        ElseIf StrRead.Contains("STNPCS" & k.ToString) Then
                            If .Rows(k - 1)("STNPCS").ToString <> "" Then fileWriter.WriteLine(StrRead.Replace("STNPCS" & k.ToString, .Rows(k - 1)("STNPCS").ToString))
                            Isprinted = True : Exit For
                        ElseIf StrRead.Contains("STNNAME" & k.ToString) Then
                            If .Rows(k - 1)("STNNAME").ToString <> "" Then fileWriter.WriteLine(StrRead.Replace("STNNAME" & k.ToString, .Rows(k - 1)("STNNAME").ToString))
                            Isprinted = True : Exit For
                            'DIAMOND
                        ElseIf StrRead.Contains("DIAMT" & k.ToString) Then
                            If .Rows(k - 1)("DIAMT").ToString <> "" Then fileWriter.WriteLine(StrRead.Replace("DIAMT" & k.ToString, .Rows(k - 1)("DIAMT").ToString))
                            Isprinted = True : Exit For
                        ElseIf StrRead.Contains("DIAWT" & k.ToString) Then
                            If .Rows(k - 1)("DIAWT").ToString <> "" Then fileWriter.WriteLine(StrRead.Replace("DIAWT" & k.ToString, .Rows(k - 1)("DIAWT").ToString))
                            Isprinted = True : Exit For
                        ElseIf StrRead.Contains("DIAPCS" & k.ToString) Then
                            If .Rows(k - 1)("DIAPCS").ToString <> "" Then fileWriter.WriteLine(StrRead.Replace("DIAPCS" & k.ToString, .Rows(k - 1)("DIAPCS").ToString))
                            Isprinted = True : Exit For
                        ElseIf StrRead.Contains("DIANAME" & k.ToString) Then
                            If .Rows(k - 1)("DIANAME").ToString <> "" Then fileWriter.WriteLine(StrRead.Replace("DIANAME" & k.ToString, .Rows(k - 1)("DIANAME").ToString))
                            Isprinted = True : Exit For
                        End If
                    End With
                Next
                If Isprinted = False Then fileWriter.WriteLine(StrRead)
            End While
            fileReader.Close()
            fileWriter.Close()
            'SEND FILE TO PRINTER
            Dim objBarcodePrint As New RawPrinterHelper
            objBarcodePrint.SendFileToPrinter(barWrite)
            strSql = "IF OBJECT_ID('TEMPTABLEDB..TEMPBARCODE" & systemName & "') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPBARCODE" & systemName & " "
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            fileWriter.Dispose()
            My.Computer.FileSystem.DeleteFile(barWrite)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function


    Function FuncprintSticker()
        Try
            Dim systemName As String = My.Computer.Name

            Dim ItemId As String = ""
            Dim TagNo As String = ""
            Dim OrdNo As String = ""
            Dim Pcs As String = ""
            Dim AvgWt As String = ""
            Dim ItemName As String = "" : Dim SubItemName As String = ""
            Dim ItemShortName As String = "" : Dim SubItemShortName As String = "" : Dim ItemCtrName As String = ""
            Dim GrsWt As String = "" : Dim NetWt As String = ""
            Dim LessWt As String = "" : Dim MAXWASTPER As String = ""
            Dim MAXWAST As String = "" : Dim MAXMCGRM As String = ""
            Dim MAXMC As String = "" : Dim stnAmt As String = ""
            Dim stnWt As String = "" : Dim stnPcs As String = "" : Dim stnName As String = ""
            Dim DiaAmt As String = "" : Dim DIAPS As String = "" : Dim DiaWt As String = "" : Dim BDSWt As String = ""
            Dim DiaName As String = "" : Dim Rate As String = ""
            Dim SalValue As String = "" : Dim ItemType As String = ""
            Dim MetalId As String = "" : Dim ItemSize As String = ""
            Dim DesginerId As String = "" : Dim TableCode As String = ""
            Dim DesginerName As String = ""
            Dim Seal As String = ""
            Dim stnWt1 As String = "" : Dim stnPcs1 As String = "" : Dim stnName1 As String = "" : Dim stoneUnit1 As String = "" : Dim stoneRate1 As String = ""
            Dim stnWt2 As String = "" : Dim stnPcs2 As String = "" : Dim stnName2 As String = "" : Dim stoneUnit2 As String = "" : Dim stoneRate2 As String = ""
            Dim stnWt3 As String = "" : Dim stnPcs3 As String = "" : Dim stnName3 As String = "" : Dim stoneUnit3 As String = "" : Dim stoneRate3 As String = ""
            Dim stnWt4 As String = "" : Dim stnPcs4 As String = "" : Dim stnName4 As String = "" : Dim stoneUnit4 As String = "" : Dim stoneRate4 As String = ""
            Dim stnWt5 As String = "" : Dim stnPcs5 As String = "" : Dim stnName5 As String = "" : Dim stoneUnit5 As String = "" : Dim stoneRate5 As String = ""
            Dim stnWt6 As String = "" : Dim stnPcs6 As String = "" : Dim stnName6 As String = "" : Dim stoneUnit6 As String = "" : Dim stoneRate6 As String = ""
            Dim stnWt7 As String = "" : Dim stnPcs7 As String = "" : Dim stnName7 As String = "" : Dim stoneUnit7 As String = "" : Dim stoneRate7 As String = ""
            Dim stnWt8 As String = "" : Dim stnPcs8 As String = "" : Dim stnName8 As String = "" : Dim stoneUnit8 As String = "" : Dim stoneRate8 As String = ""
            Dim stnWt9 As String = "" : Dim stnPcs9 As String = "" : Dim stnName9 As String = "" : Dim stoneUnit9 As String = "" : Dim stoneRate9 As String = ""
            Dim stnWt10 As String = "" : Dim stnPcs10 As String = "" : Dim stnName10 As String = "" : Dim stoneUnit10 As String = "" : Dim stoneRate10 As String = ""
            Dim stnWt11 As String = "" : Dim stnPcs11 As String = "" : Dim stnName11 As String = "" : Dim stoneUnit11 As String = "" : Dim stoneRate11 As String = ""
            Dim stnWt12 As String = "" : Dim stnPcs12 As String = "" : Dim stnName12 As String = "" : Dim stoneUnit12 As String = "" : Dim stoneRate12 As String = ""
            Dim TotStnWt As String = "" : Dim TotStnPcs As String = "" : Dim TotStnAmt As String = ""
            Dim DD As String : Dim MM As String : Dim YY As String : Dim NARRATION As String


            Dim dtTag As New DataTable
            Dim dtTagStone As New DataTable
            strSql = "SELECT * FROM TEMP" & systemName & "STICKERPRINT"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtTag)

            ItemId = dtTag.Rows(0)("ITEMID").ToString
            NARRATION = dtTag.Rows(0)("NARR").ToString
            ItemName = dtTag.Rows(0)("ITEMNAME").ToString
            SubItemName = dtTag.Rows(0)("SUBITEMNAME").ToString
            ItemCtrName = dtTag.Rows(0)("ITEMCTRNAME").ToString
            Pcs = IIf(Val(dtTag.Rows(0)("PCS").ToString) > 0, dtTag.Rows(0)("PCS").ToString, "")
            GrsWt = IIf(Val(dtTag.Rows(0)("GRSWT").ToString) > 0, dtTag.Rows(0)("GRSWT").ToString, "")
            NetWt = IIf(Val(dtTag.Rows(0)("NETWT").ToString) > 0, dtTag.Rows(0)("NETWT").ToString, "") 'dtTag.Rows(0)("NETWT").ToString
            LessWt = IIf(Val(dtTag.Rows(0)("LESSWT").ToString) > 0, dtTag.Rows(0)("LESSWT").ToString, "") ' dtTag.Rows(0)("LESSWT").ToString
            MAXWASTPER = dtTag.Rows(0)("MAXWASTPER").ToString
            MAXWAST = IIf(Val(dtTag.Rows(0)("MAXWAST").ToString) > 0, dtTag.Rows(0)("MAXWAST").ToString, "") ' dtTag.Rows(0)("MAXWAST").ToString
            MAXMCGRM = IIf(Val(dtTag.Rows(0)("MAXMCGRM").ToString) > 0, dtTag.Rows(0)("MAXMCGRM").ToString, "") 'dtTag.Rows(0)("MAXMCGRM").ToString
            MAXMC = IIf(Val(dtTag.Rows(0)("MAXMC").ToString) > 0, Math.Round(Val(dtTag.Rows(0)("MAXMC").ToString), 0), "") 'dtTag.Rows(0)("MAXMC").ToString
            ItemSize = dtTag.Rows(0)("ITEMSIZE").ToString
            DesginerName = dtTag.Rows(0)("DESIGNERNAME").ToString
            DesginerId = dtTag.Rows(0)("DESIGNERID").ToString


            Dim barRead As String = Application.StartupPath & "\BARCODE\" & "STICKERPRINT.PRN"

            If IO.Directory.Exists(Application.StartupPath & "\BARCODE") = False Then
                MsgBox("Directory not Exists..." & vbCrLf & "Directory Name : " & Application.StartupPath & "\BARCODE", MsgBoxStyle.Information)
                Exit Function
            End If
            If IO.File.Exists(barRead) = False Then
                MsgBox("Barcode Template not Found..." & vbCrLf & "File Name : " & barRead.ToString, MsgBoxStyle.Information)
                Exit Function
            End If
            Dim barWrite As String = Application.StartupPath & "\BARCODE\" & "DUPLICATE" & MetalId & systemName & ".MEM"

            Dim fileReader As New System.IO.StreamReader(barRead)
            Dim fileWriter As New System.IO.StreamWriter(barWrite)

            Dim StrRead As String
            While fileReader.EndOfStream = False
                StrRead = fileReader.ReadLine
                'DESCRIPTION
                If StrRead.Contains("TAGNO") Then
                    fileWriter.WriteLine(StrRead.Replace("TAGNO", ItemId & "-" & TagNo))
                ElseIf StrRead.Contains("SUBITEMSHORTNAME") Then
                    fileWriter.WriteLine(StrRead.Replace("SUBITEMSHORTNAME", SubItemShortName))
                ElseIf StrRead.Contains("ITEMSHORTNAME") Then
                    fileWriter.WriteLine(StrRead.Replace("ITEMSHORTNAME", ItemShortName))
                ElseIf StrRead.Contains("ITEMCTRNAME") Then
                    fileWriter.WriteLine(StrRead.Replace("ITEMCTRNAME", ItemCtrName))
                ElseIf StrRead.Contains("SUBITEMNAME") Then
                    fileWriter.WriteLine(StrRead.Replace("SUBITEMNAME", SubItemName))
                ElseIf StrRead.Contains("ITEMNAME") Then
                    fileWriter.WriteLine(StrRead.Replace("ITEMNAME", ItemName))
                ElseIf StrRead.Contains("ORDNO") Then
                    fileWriter.WriteLine(StrRead.Replace("ORDNO", OrdNo))
                    'PCS & WT
                ElseIf StrRead.Contains("PCS") Then
                    If Pcs <> "" Then fileWriter.WriteLine(StrRead.Replace("PCS", Pcs)) Else Continue While
                ElseIf StrRead.Contains("GRSWT") Then
                    If GrsWt <> "" Then fileWriter.WriteLine(StrRead.Replace("GRSWT", GrsWt)) Else Continue While
                ElseIf StrRead.Contains("AVGWT") Then
                    If AvgWt <> "" Then fileWriter.WriteLine(StrRead.Replace("AVGWT", AvgWt)) Else Continue While
                ElseIf StrRead.Contains("NETWT") Then
                    If NetWt <> "" Then fileWriter.WriteLine(StrRead.Replace("NETWT", NetWt)) Else Continue While
                ElseIf StrRead.Contains("LESSWT") Then
                    If LessWt <> "" Then fileWriter.WriteLine(StrRead.Replace("LESSWT", LessWt)) Else Continue While
                    'VA
                ElseIf StrRead.Contains("MAXWASTPER") Then
                    If MAXWASTPER <> "" Then fileWriter.WriteLine(StrRead.Replace("MAXWASTPER", MAXWASTPER)) Else Continue While
                ElseIf StrRead.Contains("MAXWAST") Then
                    If MAXWAST <> "" Then fileWriter.WriteLine(StrRead.Replace("MAXWAST", MAXWAST)) Else Continue While
                ElseIf StrRead.Contains("MAXMCGRM") Then
                    If MAXMCGRM <> "" Then fileWriter.WriteLine(StrRead.Replace("MAXMCGRM", MAXMCGRM)) Else Continue While
                ElseIf StrRead.Contains("MAXMC") Then
                    If MAXMC <> "" Then fileWriter.WriteLine(StrRead.Replace("MAXMC", MAXMC)) Else Continue While
                ElseIf StrRead.Contains("ITEMTYPE") Then
                    If ItemType <> "" Then fileWriter.WriteLine(StrRead.Replace("ITEMTYPE", ItemType)) Else Continue While
                ElseIf StrRead.Contains("SALVALUE") Then
                    If SalValue <> "" Then fileWriter.WriteLine(StrRead.Replace("SALVALUE", SalValue)) Else Continue While
                    '    'STONE
                ElseIf StrRead.Contains("STNAMT") Then
                    If stnAmt <> "" Then fileWriter.WriteLine(StrRead.Replace("STNAMT", stnAmt)) Else Continue While
                    '    'STONE1
                ElseIf StrRead.Contains("ITEMSIZE") Then
                    If ItemSize <> "" Then fileWriter.WriteLine(StrRead.Replace("ITEMSIZE", ItemSize)) Else Continue While
                ElseIf StrRead.Contains("SEAL") Then
                    If Seal <> "" Then fileWriter.WriteLine(StrRead.Replace("SEAL", Seal)) Else Continue While
                ElseIf StrRead.Contains("DESIGNERNAME") Then
                    If DesginerName <> "" Then fileWriter.WriteLine(StrRead.Replace("DESIGNERNAME", DesginerName)) Else Continue While
                ElseIf StrRead.Contains("DESIGNERID") Then
                    If DesginerId <> "" Then fileWriter.WriteLine(StrRead.Replace("DESIGNERID", DesginerId)) Else Continue While
                ElseIf StrRead.Contains("TABLECODE") Then
                    If TableCode <> "" Then fileWriter.WriteLine(StrRead.Replace("TABLECODE", TableCode)) Else Continue While

                ElseIf StrRead.Contains("STNWT10") Then
                    If stnName10 <> "" Then
                        StrRead = StrRead.Replace("STNNAME10", stnName10).ToString
                        StrRead = StrRead.Replace("STNPS10", stnPcs10).ToString
                        StrRead = StrRead.Replace("STNWT10", stnWt10).ToString
                        StrRead = StrRead.Replace("STONEUNIT10", stoneUnit10).ToString
                        StrRead = StrRead.Replace("STNRATE10", stoneRate10).ToString
                        fileWriter.WriteLine(StrRead)
                    Else
                        Continue While
                    End If
                ElseIf StrRead.Contains("STNWT11") Then
                    If stnName11 <> "" Then
                        StrRead = StrRead.Replace("STNNAME11", stnName11).ToString
                        StrRead = StrRead.Replace("STNPS11", stnPcs11).ToString
                        StrRead = StrRead.Replace("STNWT11", stnWt11).ToString
                        StrRead = StrRead.Replace("STONEUNIT11", stoneUnit11).ToString
                        StrRead = StrRead.Replace("STNRATE11", stoneRate11).ToString
                        fileWriter.WriteLine(StrRead)
                    Else
                        Continue While
                    End If
                ElseIf StrRead.Contains("STNWT12") Then
                    If stnName12 <> "" Then
                        StrRead = StrRead.Replace("STNNAME12", stnName12).ToString
                        StrRead = StrRead.Replace("STNPS12", stnPcs12).ToString
                        StrRead = StrRead.Replace("STNWT12", stnWt12).ToString
                        StrRead = StrRead.Replace("STONEUNIT12", stoneUnit12).ToString
                        StrRead = StrRead.Replace("STNRATE12", stoneRate12).ToString
                        fileWriter.WriteLine(StrRead)
                    Else
                        Continue While
                    End If


                ElseIf StrRead.Contains("STNWT1") Then
                    If stnName1 <> "" Then
                        StrRead = StrRead.Replace("STNNAME1", stnName1).ToString
                        StrRead = StrRead.Replace("STNPS1", stnPcs1).ToString
                        StrRead = StrRead.Replace("STNWT1", stnWt1).ToString
                        StrRead = StrRead.Replace("STONEUNIT1", stoneUnit1).ToString
                        StrRead = StrRead.Replace("STNRATE1", stoneRate1).ToString
                        fileWriter.WriteLine(StrRead)
                    Else
                        Continue While
                    End If
                ElseIf StrRead.Contains("STNWT2") Then
                    If stnName2 <> "" Then
                        StrRead = StrRead.Replace("STNNAME2", stnName2).ToString
                        StrRead = StrRead.Replace("STNPS2", stnPcs2).ToString
                        StrRead = StrRead.Replace("STNWT2", stnWt2).ToString
                        StrRead = StrRead.Replace("STONEUNIT2", stoneUnit2).ToString
                        StrRead = StrRead.Replace("STNRATE2", stoneRate2).ToString
                        fileWriter.WriteLine(StrRead)
                    Else
                        Continue While
                    End If
                ElseIf StrRead.Contains("STNWT3") Then
                    If stnName3 <> "" Then
                        StrRead = StrRead.Replace("STNNAME3", stnName3).ToString
                        StrRead = StrRead.Replace("STNPS3", stnPcs3).ToString
                        StrRead = StrRead.Replace("STNWT3", stnWt3).ToString
                        StrRead = StrRead.Replace("STONEUNIT3", stoneUnit3).ToString
                        StrRead = StrRead.Replace("STNRATE3", stoneRate3).ToString
                        fileWriter.WriteLine(StrRead)
                    Else
                        Continue While
                    End If
                ElseIf StrRead.Contains("STNWT4") Then
                    If stnName4 <> "" Then
                        StrRead = StrRead.Replace("STNNAME4", stnName4).ToString
                        StrRead = StrRead.Replace("STNPS4", stnPcs4).ToString
                        StrRead = StrRead.Replace("STNWT4", stnWt4).ToString
                        StrRead = StrRead.Replace("STONEUNIT4", stoneUnit4).ToString
                        StrRead = StrRead.Replace("STNRATE4", stoneRate4).ToString
                        fileWriter.WriteLine(StrRead)
                    Else
                        Continue While
                    End If


                ElseIf StrRead.Contains("STNWT5") Then
                    If stnName5 <> "" Then
                        StrRead = StrRead.Replace("STNNAME5", stnName5).ToString
                        StrRead = StrRead.Replace("STNPS5", stnPcs5).ToString
                        StrRead = StrRead.Replace("STNWT5", stnWt5).ToString
                        StrRead = StrRead.Replace("STONEUNIT5", stoneUnit5).ToString
                        StrRead = StrRead.Replace("STNRATE5", stoneRate5).ToString
                        fileWriter.WriteLine(StrRead)
                    Else
                        Continue While
                    End If
                ElseIf StrRead.Contains("STNWT6") Then
                    If stnName6 <> "" Then
                        StrRead = StrRead.Replace("STNNAME6", stnName6).ToString
                        StrRead = StrRead.Replace("STNPS6", stnPcs6).ToString
                        StrRead = StrRead.Replace("STNWT6", stnWt6).ToString
                        StrRead = StrRead.Replace("STONEUNIT6", stoneUnit6).ToString
                        StrRead = StrRead.Replace("STNRATE6", stoneRate6).ToString
                        fileWriter.WriteLine(StrRead)
                    Else
                        Continue While
                    End If
                ElseIf StrRead.Contains("STNWT7") Then
                    If stnName7 <> "" Then
                        StrRead = StrRead.Replace("STNNAME7", stnName7).ToString
                        StrRead = StrRead.Replace("STNPS7", stnPcs7).ToString
                        StrRead = StrRead.Replace("STNWT7", stnWt7).ToString
                        StrRead = StrRead.Replace("STONEUNIT7", stoneUnit7).ToString
                        StrRead = StrRead.Replace("STNRATE7", stoneRate7).ToString
                        fileWriter.WriteLine(StrRead)
                    Else
                        Continue While
                    End If
                ElseIf StrRead.Contains("STNWT8") Then
                    If stnName8 <> "" Then
                        StrRead = StrRead.Replace("STNNAME8", stnName8).ToString
                        StrRead = StrRead.Replace("STNPS8", stnPcs8).ToString
                        StrRead = StrRead.Replace("STNWT8", stnWt8).ToString
                        StrRead = StrRead.Replace("STONEUNIT8", stoneUnit8).ToString
                        StrRead = StrRead.Replace("STNRATE8", stoneRate8).ToString
                        fileWriter.WriteLine(StrRead)
                    Else
                        Continue While
                    End If
                ElseIf StrRead.Contains("STNWT9") Then
                    If stnName9 <> "" Then
                        StrRead = StrRead.Replace("STNNAME9", stnName9).ToString
                        StrRead = StrRead.Replace("STNPS9", stnPcs9).ToString
                        StrRead = StrRead.Replace("STNWT9", stnWt9).ToString
                        StrRead = StrRead.Replace("STONEUNIT9", stoneUnit9).ToString
                        StrRead = StrRead.Replace("STNRATE9", stoneRate9).ToString
                        fileWriter.WriteLine(StrRead)
                    Else
                        Continue While
                    End If


                ElseIf StrRead.Contains("TOTSTNPS") Then
                    If dtTagStone.Rows.Count > 4 Then
                        If TotStnWt <> "" Or TotStnPcs <> "" Then
                            StrRead = StrRead.Replace("TOTSTNPS", TotStnPcs).ToString
                            StrRead = StrRead.Replace("TOTSTNWT", TotStnWt).ToString
                            fileWriter.WriteLine(StrRead)
                        Else
                            Continue While
                        End If
                    End If
                    'BEEDS                
                ElseIf StrRead.Contains("BDSWT") Then
                    If BDSWt <> "" Then fileWriter.WriteLine(StrRead.Replace("BDSWT", BDSWt)) Else Continue While

                ElseIf StrRead.Contains("DIAPS") Then
                    If DIAPS <> "" Then fileWriter.WriteLine(StrRead.Replace("DIAPS", DIAPS)) Else Continue While
                ElseIf StrRead.Contains("DIAWT") Then
                    If DiaWt <> "" Then fileWriter.WriteLine(StrRead.Replace("DIAWT", DiaWt)) Else Continue While
                ElseIf StrRead.Contains("DIAAMT") Then
                    If DiaAmt <> "" Then fileWriter.WriteLine(StrRead.Replace("DIAAMT", DiaAmt)) Else Continue While


                ElseIf StrRead.Contains("STNPS") Then
                    If stnPcs <> "" Then fileWriter.WriteLine(StrRead.Replace("STNPS", stnPcs)) Else Continue While
                ElseIf StrRead.Contains("STNWT") Then
                    If stnWt <> "" Then fileWriter.WriteLine(StrRead.Replace("STNWT", stnWt)) Else Continue While
                ElseIf StrRead.Contains("STNAMT") Then
                    If stnAmt <> "" Then fileWriter.WriteLine(StrRead.Replace("STNAMT", stnAmt)) Else Continue While

                ElseIf StrRead.Contains("NARRATION") Then
                    If NARRATION <> "" Then fileWriter.WriteLine(StrRead.Replace("NARRATION", NARRATION)) Else Continue While

                ElseIf StrRead.Contains("RATE") Then
                    If Rate <> "" Then fileWriter.WriteLine(StrRead.Replace("RATE", Rate)) Else Continue While
                ElseIf StrRead.Contains("DDMM") Then
                    If DD <> "" Then fileWriter.WriteLine(StrRead.Replace("DDMM", DD & MM)) Else Continue While
                ElseIf StrRead.Contains("MMYY") Then
                    If MM <> "" Then fileWriter.WriteLine(StrRead.Replace("MMYY", MM & YY)) Else Continue While
                ElseIf StrRead.Contains("DDMMYY") Then
                    If DD <> "" Then fileWriter.WriteLine(StrRead.Replace("DDMMYY", DD & MM & YY)) Else Continue While
                ElseIf StrRead.Contains("DDMM") Then
                    If DD <> "" Then fileWriter.WriteLine(StrRead.Replace("DDMM", DD & MM)) Else Continue While
                ElseIf StrRead.Contains("MMYY") Then
                    If MM <> "" Then fileWriter.WriteLine(StrRead.Replace("MMYY", MM & YY)) Else Continue While
                ElseIf StrRead.Contains("DDMMYY") Then
                    If DD <> "" Then fileWriter.WriteLine(StrRead.Replace("DDMMYY", DD & MM & YY)) Else Continue While
                Else
                    fileWriter.WriteLine(StrRead)
                End If
            End While
            fileReader.Close()
            fileWriter.Close()
            Dim objBarcodePrint As New RawPrinterHelper
            RawPrinterHelper.SendFileToPrinter(barWrite)
            fileWriter.Dispose()
            My.Computer.FileSystem.DeleteFile(barWrite)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Function






End Class
