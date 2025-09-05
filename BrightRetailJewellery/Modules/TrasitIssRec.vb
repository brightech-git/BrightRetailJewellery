Imports System.Data.OleDb

Public Class TrasitIssRec
    Private MainId As String
    Private DA As OleDbDataAdapter
    Private Cmd As OleDbCommand
    Private StrSql As String
    Private FromId As String
    Private ToId As String
    Private IssRec As String
    Private TranDate As Date
    Private TagSno As String
    Private _Trefno As String
    Private _Cn As OleDbConnection
    Private _Tran As OleDbTransaction = Nothing
    Private _HasStone As Boolean
    Private _HasMisc As Boolean
    Private _HasMetal As Boolean
    Private _HasPurTag As Boolean
    Private _HasPurStone As Boolean
    Private _HasPurMisc As Boolean
    Private _HasPurMetal As Boolean
    Private _HasHallmarkDet As Boolean
    Private _HasAddInfoTagDet As Boolean
    Private IsCloud As Boolean = False
    Dim citemtagcolumns As String
    Dim syncdb As String
    Dim CENTR_DB_GLB As Boolean = IIf(GetAdmindbSoftValue("CENTR_DB_ALLCOSTID", "N") = "Y", True, False)
    Dim PURVALTRF As Boolean = IIf(GetAdmindbSoftValue("PURVALTRF", "Y") = "Y", True, False)
    Dim SPECIFICFORMAT As String = GetAdmindbSoftValue("SPECIFICFORMAT", "")

    Public Sub New(
    ByVal FromId As String,
    ByVal ToId As String,
    ByVal IssRec As String,
    ByVal TranDate As Date,
    ByVal TagSno As String,
    ByVal Cn As OleDbConnection,
    Optional ByVal Tran As OleDbTransaction = Nothing,
Optional ByVal Trefno As String = Nothing)
        Me.FromId = FromId
        Me.ToId = ToId
        Me.IssRec = IssRec
        Me.TranDate = TranDate
        Me.TagSno = TagSno
        Me._Cn = Cn
        Me._Tran = Tran
        If Trefno Is Nothing Then Me._Trefno = "" Else Me._Trefno = Trefno
        _HasStone = False
        _HasMisc = False
        _HasMetal = False
        _HasPurTag = False
        _HasPurStone = False
        _HasPurMisc = False
        _HasPurMetal = False
        _HasHallmarkDet = False
        _HasAddInfoTagDet = False

        MainId = BrighttechPack.GlobalMethods.GetSqlValue(Me._Cn, "SELECT TOP 1 COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE MAIN = 'Y'", , , Me._Tran)
        IsCloud = IIf(BrighttechPack.GlobalMethods.GetSqlValue(Me._Cn, "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'CLOUDDATA'", , , Me._Tran) = "Y", True, False)
        syncdb = cnAdminDb
        Dim uprefix As String = Mid(cnAdminDb, 1, 3)
        If GetAdmindbSoftValueNew("SYNC-SEPDATADB", _Cn, "N", _Tran) = "Y" Then
            Dim usuffix As String = "UTILDB"
            If DbCheckernew(uprefix + usuffix, _Cn, _Tran) <> 0 Then syncdb = uprefix + usuffix
        End If

        'Dim batWriter As StreamWriter
    End Sub

    Public Function InsertTagReceipt() As Boolean
        'Re Calculating Tag Sal Value based on Wmc

        StrSql = "SELECT '" + FromId + "" + ToId + ":'+CONVERT(VARCHAR(100),NEWID())ID"
        Dim _NewId As String = ""
        _NewId = GetSqlValue(StrSql, _Cn, _Tran)


        StrSql = "SELECT TOP 1 TRANKEY FROM " & cnAdminDb & "..TITEMTAG WHERE SNO='" & TagSno & "'"
        _NewId = GetSqlValue(StrSql, _Cn, _Tran)

        Dim DEFPICTPATH = GetAdmindbSoftValueNew("PICPATH", _Cn, "", _Tran)

        If GetAdmindbSoftValueNew("BRANCH_VA", _Cn, "Y", _Tran) = "Y" Then
            'Dim OrderBranchVa As Boolean = IIf(GetAdmindbSoftValueNew("BRANCH_VA4ORDER", _Cn, "Y", _Tran) = "Y", True, False)
            Dim ObjTagVal As New CLS_TAGVALUE_CALC(ToId, TagSno, "T", cnStockDb, cnAdminDb, _Cn, _Tran)
            If ObjTagVal.RowTagInfo IsNot Nothing Then
                If ObjTagVal.WmcValueUpdated Then
                    StrSql = " UPDATE " & cnAdminDb & "..TITEMTAG SET"
                    StrSql += vbCrLf + " MAXWASTPER = " & Val(ObjTagVal.RowTagInfo.Item("MAXWASTPER").ToString) & ""
                    StrSql += vbCrLf + " ,MAXWAST = " & Val(ObjTagVal.RowTagInfo.Item("MAXWAST").ToString) & ""
                    StrSql += vbCrLf + " ,MAXMCGRM = " & Val(ObjTagVal.RowTagInfo.Item("MAXMCGRM").ToString) & ""
                    StrSql += vbCrLf + " ,MAXMC = " & Val(ObjTagVal.RowTagInfo.Item("MAXMC").ToString) & ""
                    StrSql += vbCrLf + " ,TOUCH = " & Val(ObjTagVal.RowTagInfo.Item("TOUCH").ToString) & ""
                    StrSql += vbCrLf + " ,SALVALUE = " & Val(ObjTagVal.RowTagInfo.Item("SALVALUE").ToString) & ""
                    StrSql += " WHERE SNO = '" & TagSno & "' AND ISNULL(TAGFIXEDVA,'') <>'Y' "
                    'If Not OrderBranchVa Then StrSql += " AND ISNULL(ORDREPNO,'') =''"
                    Cmd = New OleDbCommand(StrSql, _Cn, _Tran) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
                End If
            End If
        End If
        If GetAdmindbSoftValueNew("BRANCH_CENT", _Cn, "N", _Tran) = "Y" Then
            StrSql = " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPTAGSTONE')>0 DROP TABLE TEMPTAGSTONE"
            StrSql += vbCrLf + "  SELECT *"
            StrSql += vbCrLf + "  ,(SELECT TOP 1 MAXRATE FROM " & cnAdminDb & "..CENTRATE C WHERE C.ITEMID = X.STNITEMID AND C.SUBITEMID = X.STNSUBITEMID AND X.CENTWT BETWEEN C.FROMCENT AND C.TOCENT AND ISNULL(C.ACCODE,'') = '')AS CENTRATE"
            StrSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),0)AS STNVALUE"
            StrSql += vbCrLf + "  INTO TEMPTAGSTONE"
            StrSql += vbCrLf + "  FROM"
            StrSql += vbCrLf + "  	("
            StrSql += vbCrLf + "  	SELECT SNO,STNITEMID,STNSUBITEMID,STNPCS,STNWT,STNRATE,STNAMT,CALCMODE,STONEUNIT"
            StrSql += vbCrLf + "  	,CONVERT(NUMERIC(15,4),CASE WHEN STONEUNIT = 'C' THEN "
            StrSql += vbCrLf + "  		(STNWT / CASE WHEN STNPCS = 0 THEN 1 ELSE STNPCS END)*100"
            StrSql += vbCrLf + "  	 ELSE"
            StrSql += vbCrLf + "  		STNWT / CASE WHEN STNPCS = 0 THEN 1 ELSE STNPCS END"
            StrSql += vbCrLf + "  	 END) AS CENTWT"
            StrSql += vbCrLf + "  	   "
            StrSql += vbCrLf + "  	FROM " & cnAdminDb & "..TITEMTAGSTONE S"
            StrSql += vbCrLf + "  	WHERE EXISTS (SELECT 1 FROM " & cnAdminDb & "..CENTRATE C WHERE C.ITEMID = S.STNITEMID AND C.SUBITEMID = S.STNSUBITEMID AND C.COSTID = S.COSTID)"
            StrSql += vbCrLf + "      AND TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..TITEMTAG WHERE SNO = '" & TagSno & "')"
            StrSql += vbCrLf + "  )X"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            StrSql = " DELETE FROM TEMPTAGSTONE WHERE ISNULL(CENTRATE,0) = 0"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            StrSql = " UPDATE TEMPTAGSTONE SET STNVALUE = CASE WHEN CALCMODE = 'P' THEN CENTRATE*STNPCS ELSE CENTRATE*STNWT END"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            StrSql = " UPDATE " & cnAdminDb & "..TITEMTAGSTONE SET STNRATE = C.CENTRATE,STNAMT = C.STNVALUE"
            StrSql += vbCrLf + "  FROM " & cnAdminDb & "..TITEMTAGSTONE AS T INNER JOIN TEMPTAGSTONE C ON T.SNO = C.SNO"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If

        Dim DefItemctrid As Integer = Val(GetAdmindbSoftValueNew("TAG_RECD_CTRID", _Cn, "", _Tran))
        If DefItemctrid <> 0 And objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE ISNULL(MAIN,'')='Y'", , , ) = cnCostId Then
            StrSql = " UPDATE " & cnAdminDb & "..TITEMTAG SET"
            StrSql += vbCrLf + " ITEMCTRID = " & DefItemctrid & ""
            StrSql += " WHERE SNO = '" & TagSno & "'"
            Cmd = New OleDbCommand(StrSql, _Cn, _Tran) : Cmd.CommandTimeout = 1000 : Cmd.ExecuteNonQuery()
        End If


        Dim Qry As String = Nothing
        Dim dtTemp As New DataTable
        StrSql = vbCrLf + "   select DISTINCT "
        StrSql += vbCrLf + "   T.SNO TAG,TS.TAGSNO STONE,TMI.TAGSNO MISC,TME.TAGSNO METAL,THM.TAGSNO HALLMARK,TAD.TAGSNO ADDINFOTAG "
        StrSql += vbCrLf + "   ,PT.TAGSNO PTAG,PTS.TAGSNO PSTONE,PTMI.TAGSNO PMISC,PTME.TAGSNO PMETAL"
        StrSql += vbCrLf + "   FROM " & cnAdminDb & "..TITEMTAG AS T"
        StrSql += vbCrLf + "   LEFT OUTER JOIN " & cnAdminDb & "..TITEMTAGSTONE AS TS ON TS.TAGSNO = T.SNO"
        StrSql += vbCrLf + "   LEFT OUTER JOIN " & cnAdminDb & "..TITEMTAGMISCCHAR AS TMI ON TMI.TAGSNO = T.SNO"
        StrSql += vbCrLf + "   LEFT OUTER JOIN " & cnAdminDb & "..TITEMTAGMETAL AS TME ON TME.TAGSNO = T.SNO"
        StrSql += vbCrLf + "   LEFT OUTER JOIN " & cnAdminDb & "..TITEMTAGHALLMARK AS THM ON THM.TAGSNO = T.SNO"
        StrSql += vbCrLf + "   LEFT OUTER JOIN " & cnAdminDb & "..TADDINFOITEMTAG AS TAD ON TAD.TAGSNO = T.SNO"
        StrSql += vbCrLf + "   LEFT OUTER JOIN " & cnAdminDb & "..TPURITEMTAG AS PT ON PT.TAGSNO = T.SNO"
        StrSql += vbCrLf + "   LEFT OUTER JOIN " & cnAdminDb & "..TPURITEMTAGSTONE AS PTS ON PTS.TAGSNO = T.SNO"
        StrSql += vbCrLf + "   LEFT OUTER JOIN " & cnAdminDb & "..TPURITEMTAGMISCCHAR AS PTMI ON PTMI.TAGSNO = T.SNO"
        StrSql += vbCrLf + "   LEFT OUTER JOIN " & cnAdminDb & "..TPURITEMTAGMETAL AS PTME ON PTME.TAGSNO = T.SNO"
        StrSql += vbCrLf + "   WHERE T.SNO = '" & TagSno & "'"
        dtTemp = New DataTable
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        DA = New OleDbDataAdapter(Cmd)
        DA.Fill(dtTemp)
        If Not dtTemp.Rows.Count > 0 Then Return False
        If dtTemp.Rows(0).Item("STONE").ToString <> "" Then _HasStone = True
        If dtTemp.Rows(0).Item("MISC").ToString <> "" Then _HasMisc = True
        If dtTemp.Rows(0).Item("METAL").ToString <> "" Then _HasMetal = True
        If dtTemp.Rows(0).Item("PTAG").ToString <> "" Then _HasPurTag = True
        If dtTemp.Rows(0).Item("PSTONE").ToString <> "" Then _HasPurStone = True
        If dtTemp.Rows(0).Item("PMISC").ToString <> "" Then _HasPurMisc = True
        If dtTemp.Rows(0).Item("PMETAL").ToString <> "" Then _HasPurMetal = True
        If dtTemp.Rows(0).Item("HALLMARK").ToString <> "" Then _HasHallmarkDet = True
        If dtTemp.Rows(0).Item("ADDINFOTAG").ToString <> "" Then _HasAddInfoTagDet = True
        If _HasStone Then
            If Val(objGPack.GetSqlValue("select count(*) from " & cnAdminDb & "..ITEMTAGSTONE t where SNO in (select sno from " & cnAdminDb & "..TITEMTAGSTONE b where b.TAGSNO <> t.tagsno and b.tagsno = '" & TagSno & "')").ToString) > 0 Then
                StrSql = "update t set t.sno = t.sno+'D' from " & cnAdminDb & "..ITEMTAGSTONE t where SNO in (select sno from " & cnAdminDb & "..TITEMTAGSTONE b where b.TAGSNO <> t.tagsno and b.tagsno = '" & TagSno & "')"
                Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
                Cmd.ExecuteNonQuery()
            End If
        End If
        Dim ColList As String
        ''if exsts copy to ctag


        'StrSql = vbCrLf + " UPDATE " & cnAdminDb & "..ITEMTAG SET TOFLAG = 'TR',ISSDATE = '" & TranDate.ToString("yyyy-MM-dd") & "' WHERE SNO = '" & TagSno & "' AND ISSDATE IS NULL"
        'Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        'Cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()

        StrSql = vbCrLf + " UPDATE " & cnAdminDb & "..ITEMTAG SET TRANKEY='" + _NewId + "' WHERE SNO = '" & TagSno & "' "
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()


        StrSql = vbCrLf + " UPDATE " & cnAdminDb & "..ITEMTAG SET TOFLAG = 'TR',ISSDATE = '" & TranDate.ToString("yyyy-MM-dd") & "' WHERE SNO = '" & TagSno & "' AND ISSDATE IS NULL"
        ColList = GetColumnNames(cnAdminDb, "CITEMTAG", )
        StrSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..CITEMTAG(" & ColList & ")"
        StrSql += vbCrLf + " SELECT  " & ColList & " FROM " & cnAdminDb & "..ITEMTAG WHERE SNO = '" & TagSno & "'"
        ColList = GetColumnNames(cnAdminDb, "CITEMTAGSTONE", )
        If _HasStone Then StrSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..CITEMTAGSTONE(" & ColList & ")"
        If _HasStone Then StrSql += vbCrLf + " SELECT  " & ColList & " FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = '" & TagSno & "'"
        ColList = GetColumnNames(cnAdminDb, "CITEMTAGMISCCHAR", )
        If _HasMisc Then StrSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..CITEMTAGMISCCHAR(" & ColList & ")"
        If _HasMisc Then StrSql += vbCrLf + " SELECT  " & ColList & " FROM " & cnAdminDb & "..ITEMTAGMISCCHAR WHERE TAGSNO = '" & TagSno & "'"
        ColList = GetColumnNames(cnAdminDb, "CITEMTAGMETAL", )
        If _HasMetal Then StrSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..CITEMTAGMETAL(" & ColList & ")"
        If _HasMetal Then StrSql += vbCrLf + " SELECT  " & ColList & " FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO = '" & TagSno & "'"
        ColList = GetColumnNames(cnAdminDb, "CPURITEMTAG", )
        If _HasPurTag Then StrSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..CPURITEMTAG(" & ColList & ")"
        If _HasPurTag Then StrSql += vbCrLf + " SELECT " & ColList & " FROM " & cnAdminDb & "..PURITEMTAG WHERE TAGSNO = '" & TagSno & "'"
        ColList = GetColumnNames(cnAdminDb, "CPURITEMTAGSTONE", )
        If _HasPurStone Then StrSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..CPURITEMTAGSTONE(" & ColList & ")"
        If _HasPurStone Then StrSql += vbCrLf + " SELECT " & ColList & " FROM " & cnAdminDb & "..PURITEMTAGSTONE WHERE TAGSNO = '" & TagSno & "'"
        ColList = GetColumnNames(cnAdminDb, "CPURITEMTAGMISCCHAR", )
        If _HasPurMisc Then StrSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..CPURITEMTAGMISCCHAR(" & ColList & ")"
        If _HasPurMisc Then StrSql += vbCrLf + " SELECT " & ColList & " FROM " & cnAdminDb & "..PURITEMTAGMISCCHAR WHERE TAGSNO = '" & TagSno & "'"
        ColList = GetColumnNames(cnAdminDb, "CPURITEMTAGMETAL", )
        If _HasPurMetal Then StrSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..CPURITEMTAGMETAL(" & ColList & ")"
        If _HasPurMetal Then StrSql += vbCrLf + " SELECT " & ColList & " FROM " & cnAdminDb & "..PURITEMTAGMETAL WHERE TAGSNO = '" & TagSno & "'"
        ColList = GetColumnNames(cnAdminDb, "CITEMTAGHALLMARK", )
        If _HasHallmarkDet Then StrSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..CITEMTAGHALLMARK(" & ColList & ")"
        If _HasHallmarkDet Then StrSql += vbCrLf + " SELECT  " & ColList & " FROM " & cnAdminDb & "..ITEMTAGHALLMARK WHERE TAGSNO = '" & TagSno & "'"
        ColList = GetColumnNames(cnAdminDb, "CADDINFOITEMTAG", )
        If _HasAddInfoTagDet Then StrSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..CADDINFOITEMTAG(" & ColList & ")"
        If _HasAddInfoTagDet Then StrSql += vbCrLf + " SELECT  " & ColList & " FROM " & cnAdminDb & "..ADDINFOITEMTAG WHERE TAGSNO = '" & TagSno & "'"

        ''if exsts update corrsponding cost and rec date

        StrSql += vbCrLf + " UPDATE " & cnAdminDb & "..TITEMTAG SET COSTID = '" & ToId & "',RECDATE = '" & TranDate.ToString("yyyy-MM-dd") & "',TCOSTID = '" & ToId & "',REFNO = '',REFDATE = NULL WHERE SNO = '" & TagSno & "'"
        StrSql += vbCrLf + " UPDATE " & cnAdminDb & "..TITEMTAG SET PCTPATH = '" & DEFPICTPATH & "' WHERE SNO = '" & TagSno & "'"
        If _HasStone Then StrSql += vbCrLf + " UPDATE " & cnAdminDb & "..TITEMTAGSTONE SET COSTID = '" & ToId & "',RECDATE = '" & TranDate.ToString("yyyy-MM-dd") & "' WHERE TAGSNO = '" & TagSno & "'"
        If _HasMisc Then StrSql += vbCrLf + " UPDATE " & cnAdminDb & "..TITEMTAGMISCCHAR SET COSTID = '" & ToId & "' WHERE TAGSNO = '" & TagSno & "'"
        If _HasMetal Then StrSql += vbCrLf + " UPDATE " & cnAdminDb & "..TITEMTAGMETAL SET COSTID = '" & ToId & "',RECDATE = '" & TranDate.ToString("yyyy-MM-dd") & "' WHERE TAGSNO = '" & TagSno & "'"
        If _HasPurTag Then StrSql += vbCrLf + " UPDATE " & cnAdminDb & "..TPURITEMTAG SET COSTID = '" & ToId & "' WHERE TAGSNO = '" & TagSno & "'"
        If _HasPurStone Then StrSql += vbCrLf + " UPDATE " & cnAdminDb & "..TPURITEMTAGSTONE SET COSTID = '" & ToId & "' WHERE TAGSNO = '" & TagSno & "'"
        If _HasPurMisc Then StrSql += vbCrLf + " UPDATE " & cnAdminDb & "..TPURITEMTAGMISCCHAR SET COSTID = '" & ToId & "' WHERE TAGSNO = '" & TagSno & "'"
        If _HasPurMetal Then StrSql += vbCrLf + " UPDATE " & cnAdminDb & "..TPURITEMTAGMETAL SET COSTID = '" & ToId & "' WHERE TAGSNO = '" & TagSno & "'"
        If _HasHallmarkDet Then StrSql += vbCrLf + " UPDATE " & cnAdminDb & "..TITEMTAGHALLMARK SET COSTID = '" & ToId & "' WHERE TAGSNO = '" & TagSno & "'"
        If _HasAddInfoTagDet Then StrSql += vbCrLf + " UPDATE " & cnAdminDb & "..TADDINFOITEMTAG SET COSTID = '" & ToId & "' WHERE TAGSNO = '" & TagSno & "'"


        ''if exists delete from tag
        StrSql += vbCrLf + " DELETE FROM " & cnAdminDb & "..ITEMTAG WHERE SNO = '" & TagSno & "'"
        If _HasStone Then StrSql += vbCrLf + " DELETE FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = '" & TagSno & "'"
        If _HasMisc Then StrSql += vbCrLf + " DELETE FROM " & cnAdminDb & "..ITEMTAGMISCCHAR WHERE TAGSNO = '" & TagSno & "'"
        If _HasMetal Then StrSql += vbCrLf + " DELETE FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO = '" & TagSno & "'"
        If _HasPurTag Then StrSql += vbCrLf + " DELETE FROM " & cnAdminDb & "..PURITEMTAG WHERE TAGSNO = '" & TagSno & "'"
        If _HasPurStone Then StrSql += vbCrLf + " DELETE FROM " & cnAdminDb & "..PURITEMTAGSTONE WHERE TAGSNO = '" & TagSno & "'"
        If _HasPurMisc Then StrSql += vbCrLf + " DELETE FROM " & cnAdminDb & "..PURITEMTAGMISCCHAR WHERE TAGSNO = '" & TagSno & "'"
        If _HasPurMetal Then StrSql += vbCrLf + " DELETE FROM " & cnAdminDb & "..PURITEMTAGMETAL WHERE TAGSNO = '" & TagSno & "'"
        If _HasHallmarkDet Then StrSql += vbCrLf + " DELETE FROM " & cnAdminDb & "..ITEMTAGHALLMARK WHERE TAGSNO = '" & TagSno & "'"
        If _HasAddInfoTagDet Then StrSql += vbCrLf + " DELETE FROM " & cnAdminDb & "..ADDINFOITEMTAG WHERE TAGSNO = '" & TagSno & "'"

        ''copy to tag
        ColList = GetColumnNames(cnAdminDb, "ITEMTAG", )
        StrSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..ITEMTAG (" & ColList & ")"
        StrSql += vbCrLf + " SELECT DISTINCT " & ColList & " FROM " & cnAdminDb & "..TITEMTAG WHERE SNO = '" & TagSno & "'"
        ColList = GetColumnNames(cnAdminDb, "ITEMTAGSTONE", )
        If _HasStone Then StrSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..ITEMTAGSTONE (" & ColList & ")"
        If _HasStone Then StrSql += vbCrLf + " SELECT DISTINCT " & ColList & " FROM " & cnAdminDb & "..TITEMTAGSTONE WHERE TAGSNO = '" & TagSno & "'"
        ColList = GetColumnNames(cnAdminDb, "ITEMTAGMISCCHAR", )
        If _HasMisc Then StrSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..ITEMTAGMISCCHAR (" & ColList & ")"
        If _HasMisc Then StrSql += vbCrLf + " SELECT DISTINCT " & ColList & " FROM " & cnAdminDb & "..TITEMTAGMISCCHAR WHERE TAGSNO = '" & TagSno & "'"
        ColList = GetColumnNames(cnAdminDb, "ITEMTAGMETAL", )
        If _HasMetal Then StrSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..ITEMTAGMETAL (" & ColList & ")"
        If _HasMetal Then StrSql += vbCrLf + " SELECT DISTINCT " & ColList & " FROM " & cnAdminDb & "..TITEMTAGMETAL WHERE TAGSNO = '" & TagSno & "'"
        ColList = GetColumnNames(cnAdminDb, "PURITEMTAG", )
        If _HasPurTag Then StrSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..PURITEMTAG (" & ColList & ")"
        If _HasPurTag Then StrSql += vbCrLf + " SELECT DISTINCT " & ColList & " FROM " & cnAdminDb & "..TPURITEMTAG WHERE TAGSNO = '" & TagSno & "'"
        ColList = GetColumnNames(cnAdminDb, "PURITEMTAGSTONE", )
        If _HasPurStone Then StrSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..PURITEMTAGSTONE (" & ColList & ")"
        If _HasPurStone Then StrSql += vbCrLf + " SELECT DISTINCT " & ColList & " FROM " & cnAdminDb & "..TPURITEMTAGSTONE WHERE TAGSNO = '" & TagSno & "'"
        ColList = GetColumnNames(cnAdminDb, "PURITEMTAGMISCCHAR", )
        If _HasPurMisc Then StrSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..PURITEMTAGMISCCHAR (" & ColList & ")"
        If _HasPurMisc Then StrSql += vbCrLf + " SELECT DISTINCT " & ColList & " FROM " & cnAdminDb & "..TPURITEMTAGMISCCHAR WHERE TAGSNO = '" & TagSno & "'"
        ColList = GetColumnNames(cnAdminDb, "PURITEMTAGMETAL", )
        If _HasPurMetal Then StrSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..PURITEMTAGMETAL (" & ColList & ")"
        If _HasPurMetal Then StrSql += vbCrLf + " SELECT DISTINCT " & ColList & " FROM " & cnAdminDb & "..TPURITEMTAGMETAL WHERE TAGSNO = '" & TagSno & "'"
        ColList = GetColumnNames(cnAdminDb, "ITEMTAGHALLMARK", )
        If _HasHallmarkDet Then StrSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..ITEMTAGHALLMARK (" & ColList & ")"
        If _HasHallmarkDet Then StrSql += vbCrLf + " SELECT DISTINCT " & ColList & " FROM " & cnAdminDb & "..TITEMTAGHALLMARK WHERE TAGSNO = '" & TagSno & "'"
        ColList = GetColumnNames(cnAdminDb, "ADDINFOITEMTAG", )
        If _HasAddInfoTagDet Then StrSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..ADDINFOITEMTAG (" & ColList & ")"
        If _HasAddInfoTagDet Then StrSql += vbCrLf + " SELECT DISTINCT " & ColList & " FROM " & cnAdminDb & "..TADDINFOITEMTAG WHERE TAGSNO = '" & TagSno & "'"
        ''delete from ttag
        StrSql += vbCrLf + " DELETE FROM " & cnAdminDb & "..TITEMTAG WHERE SNO = '" & TagSno & "'"
        If _HasStone Then StrSql += vbCrLf + " DELETE FROM " & cnAdminDb & "..TITEMTAGSTONE WHERE TAGSNO = '" & TagSno & "'"
        If _HasMisc Then StrSql += vbCrLf + " DELETE FROM " & cnAdminDb & "..TITEMTAGMISCCHAR WHERE TAGSNO = '" & TagSno & "'"
        If _HasMetal Then StrSql += vbCrLf + " DELETE FROM " & cnAdminDb & "..TITEMTAGMETAL WHERE TAGSNO = '" & TagSno & "'"
        If _HasPurTag Then StrSql += vbCrLf + " DELETE FROM " & cnAdminDb & "..TPURITEMTAG WHERE TAGSNO = '" & TagSno & "'"
        If _HasPurStone Then StrSql += vbCrLf + " DELETE FROM " & cnAdminDb & "..TPURITEMTAGSTONE WHERE TAGSNO = '" & TagSno & "'"
        If _HasPurMisc Then StrSql += vbCrLf + " DELETE FROM " & cnAdminDb & "..TPURITEMTAGMISCCHAR WHERE TAGSNO = '" & TagSno & "'"
        If _HasPurMetal Then StrSql += vbCrLf + " DELETE FROM " & cnAdminDb & "..TPURITEMTAGMETAL WHERE TAGSNO = '" & TagSno & "'"
        If _HasHallmarkDet Then StrSql += vbCrLf + " DELETE FROM " & cnAdminDb & "..TITEMTAGHALLMARK WHERE TAGSNO = '" & TagSno & "'"
        If _HasAddInfoTagDet Then StrSql += vbCrLf + " DELETE FROM " & cnAdminDb & "..TADDINFOITEMTAG WHERE TAGSNO = '" & TagSno & "'"
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = "INSERT INTO " & syncdb & "..TRANS_STATUS (FROMID,TAGSNO,STATUS,TRANNO,TRANDATE,STOCKMODE,MOVETYPE)"
        StrSql += " SELECT '" & FromId & "','" & TagSno & "','T',0,'" & TranDate.ToString("yyyy-MM-dd") & "','T','I'"
        Cmd = New OleDbCommand(StrSql, _Cn, tran)
        Cmd.ExecuteNonQuery()

        Qry = vbCrLf + " DELETE FROM " & cnAdminDb & "..PITEMTAG WHERE SNO = '" & TagSno & "'"
        If _HasStone Then Qry += vbCrLf + " DELETE FROM " & cnAdminDb & "..PITEMTAGSTONE WHERE TAGSNO = '" & TagSno & "'"
        If Not CENTR_DB_GLB And strBCostid = "" Then
            StrSql = " INSERT INTO " & syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
            StrSql += vbCrLf + " SELECT '" & cnCostId & "','" & FromId & "','" & Qry.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
            Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
            Cmd.ExecuteNonQuery()
        Else
            Cmd = New OleDbCommand(Qry, _Cn, _Tran)
            Cmd.ExecuteNonQuery()
        End If

        StrSql = "SELECT TRANINVNO FROM " & cnAdminDb & "..ITEMTAG WHERE SNO='" & TagSno & "'"
        Dim Trefno_ As String = GetSqlValue(StrSql, _Cn, _Tran)

        StrSql = vbCrLf + " DECLARE @BSNO VARCHAR(15)"
        StrSql += vbCrLf + " IF (SELECT COUNT(*) FROM " & cnAdminDb & "..CTRANSFER WHERE TAGSNO = '" & TagSno & "')>0  "
        StrSql += vbCrLf + " 	BEGIN  "
        StrSql += vbCrLf + " 	UPDATE " & cnAdminDb & "..CTRANSFER SET ISSDATE = '" & TranDate.ToString("yyyy-MM-dd") & "'  "
        StrSql += vbCrLf + " 	WHERE TAGSNO = '" & TagSno & "'  "
        StrSql += vbCrLf + " 	AND ISSDATE IS NULL  "
        StrSql += vbCrLf + " 	END  "
        StrSql += vbCrLf + " ELSE  "
        StrSql += vbCrLf + " 	BEGIN  "
        StrSql += vbCrLf + " 	EXEC " & cnStockDb & "..GET_ADMINSNO_TRAN  "
        StrSql += vbCrLf + " 	@COSTID = '" & FromId & "'"
        StrSql += vbCrLf + " 	,@CTLID = 'CTRANSFERCODE'  "
        StrSql += vbCrLf + " 	,@CHECKTABLENAME = 'CTRANSFER'  "
        StrSql += vbCrLf + " 	,@COMPANYID = '" & _MainCompId & "'"
        StrSql += vbCrLf + " 	,@RETVALUE = @BSNO OUTPUT  "
        StrSql += vbCrLf + " 	INSERT INTO " & cnAdminDb & "..CTRANSFER  "
        StrSql += vbCrLf + " 	(SNO,TAGSNO,ITEMID,TAGNO,COSTID,RECDATE,ISSDATE,TAGVAL,USERID  "
        StrSql += vbCrLf + " 	,UPDATED,UPTIME,APPVER,ENTORDER,ITEMCTRID,TRFNO,FLAG)  "
        StrSql += vbCrLf + " 	SELECT @BSNO AS SNO,SNO TAGSNO  "
        StrSql += vbCrLf + " 	,ITEMID,TAGNO,'" & FromId & "',RECDATE,'" & TranDate.ToString("yyyy-MM-dd") & "'  AS ISSDATE  "
        StrSql += vbCrLf + " 	,TAGVAL,0 USERID,'" & TranDate.ToString("yyyy-MM-dd") & "' UPDATED,NULL  UPTIME,'" & GlobalVariables.VERSION & "' APPVER,1,ITEMCTRID,'" & Trefno_ & "','I'  "
        StrSql += vbCrLf + " 	FROM " & cnAdminDb & "..ITEMTAG WHERE SNO = '" & TagSno & "'   "
        StrSql += vbCrLf + " 	END  "
        StrSql += vbCrLf + " EXEC " & cnStockDb & "..GET_ADMINSNO_TRAN  "
        StrSql += vbCrLf + " @COSTID = '" & FromId & "'"
        StrSql += vbCrLf + " ,@CTLID = 'CTRANSFERCODE'  "
        StrSql += vbCrLf + " ,@CHECKTABLENAME = 'CTRANSFER'  "
        StrSql += vbCrLf + " ,@COMPANYID = '" & _MainCompId & "'"
        StrSql += vbCrLf + " ,@RETVALUE = @BSNO OUTPUT  "
        StrSql += vbCrLf + " DECLARE @ENTORDER INT  "
        StrSql += vbCrLf + " SELECT @ENTORDER = ISNULL(MAX(ENTORDER),0)+1 FROM " & cnAdminDb & "..CTRANSFER WHERE TAGSNO = '" & TagSno & "'  "
        StrSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..CTRANSFER  "
        StrSql += vbCrLf + " (SNO,TAGSNO,ITEMID,TAGNO,COSTID,RECDATE,TAGVAL,USERID  "
        StrSql += vbCrLf + " ,UPDATED,UPTIME,APPVER,ENTORDER,ITEMCTRID,TRFNO,FLAG)  "
        StrSql += vbCrLf + " SELECT @BSNO AS SNO,SNO TAGSNO  "
        StrSql += vbCrLf + " ,ITEMID,TAGNO,'" & ToId & "','" & TranDate.ToString("yyyy-MM-dd") & "'  AS RECDATE  "
        StrSql += vbCrLf + " ,TAGVAL,0 USERID,'" & TranDate.ToString("yyyy-MM-dd") & "' UPDATED,NULL UPTIME,'" & GlobalVariables.VERSION & "' APPVER,@ENTORDER,ITEMCTRID,'" & Trefno_ & "','R'  "
        StrSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG   "
        StrSql += vbCrLf + " WHERE SNO = '" & TagSno & "'  "
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = " EXEC " & cnStockDb & "..SP_INSERTTAGSORIDDETAIL @TAGSNO = '" & TagSno & "'"
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        DA = New OleDbDataAdapter(Cmd)
        Dim dtTest As New DataTable
        DA.Fill(dtTest)

        StrSql = " EXEC " & cnStockDb & "..SP_INSERTTAGSORIDDETAIL @TAGSNO = '" & TagSno & "'"
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        Cmd.ExecuteNonQuery()

        If Not CENTR_DB_GLB And objGPack.GetSqlValue("select CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SYNC-TO'", , , ) <> "" Then
            StrSql = "SELECT TOP 1 COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE MAIN = 'Y'"
            Dim mfromCostid As String = BrighttechPack.GlobalMethods.GetSqlValue(_Cn, StrSql, , " ", _Tran)
            Qry = " EXEC " & cnStockDb & "..SP_INSERTTAGSORIDDETAIL @TAGSNO = '" & TagSno & "'"
            StrSql = " INSERT INTO " & syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
            StrSql += vbCrLf + " SELECT '" & cnCostId & "','" & mfromCostid & "','" & Qry.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
            Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
            Cmd.ExecuteNonQuery()

            Qry = vbCrLf + " DELETE FROM " & cnAdminDb & "..PITEMTAG WHERE SNO = '" & TagSno & "'"
            If _HasStone Then Qry += vbCrLf + " DELETE FROM " & cnAdminDb & "..PITEMTAGSTONE WHERE TAGSNO = '" & TagSno & "'"
            StrSql = " INSERT INTO " & syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
            StrSql += vbCrLf + " SELECT '" & cnCostId & "','" & mfromCostid & "','" & Qry.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
            Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
            Cmd.ExecuteNonQuery()

            SendResponse(cnCostId, mfromCostid, _NewId)
        End If

        Return True
    End Function





    Private Sub SendResponse(ByVal FromCostId As String, ByVal ToCostId As String, ByVal NewId As String)
        Dim Qry As String = ""
        Dim dtTemp As New DataTable
        'Qry = vbCrLf + " EXEC " & cnAdminDb & "..SP_CCTRANSFER"
        'Qry += vbCrLf + " @TAGSNO = '" & TagSno & "'"
        'Qry += vbCrLf + " ,@COSTID = '" & FromCostId & "'"
        'Qry += vbCrLf + " ,@ISSDATE = NULL"
        'Qry += vbCrLf + " ,@ISSTIME = NULL"
        'Qry += vbCrLf + " ,@VERSION = '" & GlobalVariables.VERSION & "'"
        'Qry += vbCrLf + " ,@USERID = " & GlobalVariables.userId
        'Qry += vbCrLf + " ,@TRFNO = '" & Trefno_ & "'"
        'Qry += vbCrLf + " ,@FLAG = 'R'"


        'StrSql = vbCrLf + " INSERT INTO " & syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
        'StrSql += vbCrLf + " SELECT '" & FromCostId & "','" & ToCostId & "','" & Qry.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
        'Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        ''Cmd.CommandTimeout = 500
        'Cmd.ExecuteNonQuery()
        '' UPDATE TAG ISSDATE 


        'Qry = vbCrLf + " UPDATE " & cnAdminDb & "..ITEMTAG SET TOFLAG = 'TR',ISSDATE = '" & TranDate.ToString("yyyy-MM-dd") & "',TRANKEY='" + NewId + "' WHERE SNO = '" & TagSno & "' AND ISSDATE IS NULL"
        'StrSql = vbCrLf + " INSERT INTO " & syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
        'StrSql += vbCrLf + " SELECT '" & FromCostId & "','" & ToCostId & "','" & Qry.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
        'Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        'Cmd.ExecuteNonQuery()


        Qry = vbCrLf + " UPDATE " & cnAdminDb & "..ITEMTAG SET TOFLAG = 'TR',ISSDATE = '" & TranDate.ToString("yyyy-MM-dd") & "',TRANKEY='" & NewId & "' WHERE SNO = '" & TagSno & "' AND ISSDATE IS NULL"
        StrSql = vbCrLf + " INSERT INTO " & syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
        StrSql += vbCrLf + " SELECT '" & FromCostId & "','" & ToCostId & "','" & Qry.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        'Cmd.CommandTimeout = 500
        Cmd.ExecuteNonQuery()
        '' COPY TAG TO CTAG
        Dim ColList As String = ""
        ColList = GetColumnNames(cnAdminDb, "CITEMTAG", )
        Qry = vbCrLf + " INSERT INTO " & cnAdminDb & "..CITEMTAG (" & ColList & ")"
        Qry += vbCrLf + " SELECT DISTINCT " & ColList & " FROM " & cnAdminDb & "..ITEMTAG WHERE SNO = '" & TagSno & "'"
        StrSql = vbCrLf + " INSERT INTO " & syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
        StrSql += vbCrLf + " SELECT '" & FromCostId & "','" & ToCostId & "','" & Qry.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        'Cmd.CommandTimeout = 500
        Cmd.ExecuteNonQuery()
        If _HasStone Then
            ColList = GetColumnNames(cnAdminDb, "CITEMTAGSTONE", )
            Qry = vbCrLf + " INSERT INTO " & cnAdminDb & "..CITEMTAGSTONE (" & ColList & ")"
            Qry += vbCrLf + " SELECT DISTINCT " & ColList & " FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = '" & TagSno & "'"
            StrSql = vbCrLf + " INSERT INTO " & syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
            StrSql += vbCrLf + " SELECT '" & FromCostId & "','" & ToCostId & "','" & Qry.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
            Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
            '   Cmd.CommandTimeout = 500
            Cmd.ExecuteNonQuery()
        End If
        If _HasMisc Then
            ColList = GetColumnNames(cnAdminDb, "CITEMTAGMISCCHAR", )
            Qry = vbCrLf + " INSERT INTO " & cnAdminDb & "..CITEMTAGMISCCHAR (" & ColList & ")"
            Qry += vbCrLf + " SELECT DISTINCT " & ColList & " FROM " & cnAdminDb & "..ITEMTAGMISCCHAR WHERE TAGSNO = '" & TagSno & "'"
            StrSql = vbCrLf + " INSERT INTO " & syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
            StrSql += vbCrLf + " SELECT '" & FromCostId & "','" & ToCostId & "','" & Qry.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
            Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
            '  Cmd.CommandTimeout = 500
            Cmd.ExecuteNonQuery()
        End If
        If _HasMetal Then
            ColList = GetColumnNames(cnAdminDb, "CITEMTAGMETAL", )
            Qry = vbCrLf + " INSERT INTO " & cnAdminDb & "..CITEMTAGMETAL (" & ColList & ")"
            Qry += vbCrLf + " SELECT DISTINCT " & ColList & " FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO = '" & TagSno & "'"
            StrSql = vbCrLf + " INSERT INTO " & syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
            StrSql += vbCrLf + " SELECT '" & FromCostId & "','" & ToCostId & "','" & Qry.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
            Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
            ' Cmd.CommandTimeout = 500
            Cmd.ExecuteNonQuery()
        End If
        If _HasPurTag Then
            ColList = GetColumnNames(cnAdminDb, "CPURITEMTAG", )
            Qry = vbCrLf + " INSERT INTO " & cnAdminDb & "..CPURITEMTAG (" & ColList & ")"
            Qry += vbCrLf + " SELECT " & ColList & " FROM " & cnAdminDb & "..PURITEMTAG WHERE TAGSNO = '" & TagSno & "'"
            StrSql = vbCrLf + " INSERT INTO " & syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
            StrSql += vbCrLf + " SELECT '" & FromCostId & "','" & ToCostId & "','" & Qry.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
            Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
            'Cmd.CommandTimeout = 500
            Cmd.ExecuteNonQuery()
        End If
        If _HasPurStone Then
            ColList = GetColumnNames(cnAdminDb, "CPURITEMTAGSTONE", )
            Qry = vbCrLf + " INSERT INTO " & cnAdminDb & "..CPURITEMTAGSTONE (" & ColList & ")"
            Qry += vbCrLf + " SELECT " & ColList & " FROM " & cnAdminDb & "..PURITEMTAGSTONE WHERE TAGSNO = '" & TagSno & "'"
            StrSql = vbCrLf + " INSERT INTO " & syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
            StrSql += vbCrLf + " SELECT '" & FromCostId & "','" & ToCostId & "','" & Qry.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
            Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
            ' Cmd.CommandTimeout = 500
            Cmd.ExecuteNonQuery()
        End If
        If _HasPurMisc Then
            ColList = GetColumnNames(cnAdminDb, "CPURITEMTAGMISCCHAR", )
            Qry = vbCrLf + " INSERT INTO " & cnAdminDb & "..CPURITEMTAGMISCCHAR (" & ColList & ")"
            Qry += vbCrLf + " SELECT " & ColList & " FROM " & cnAdminDb & "..PURITEMTAGMISCCHAR WHERE TAGSNO = '" & TagSno & "'"
            StrSql = vbCrLf + " INSERT INTO " & syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
            StrSql += vbCrLf + " SELECT '" & FromCostId & "','" & ToCostId & "','" & Qry.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
            Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
            'Cmd.CommandTimeout = 500
            Cmd.ExecuteNonQuery()
        End If
        If _HasPurMetal Then
            ColList = GetColumnNames(cnAdminDb, "CPURITEMTAGMETAL", )
            Qry = vbCrLf + " INSERT INTO " & cnAdminDb & "..CPURITEMTAGMETAL (" & ColList & ")"
            Qry += vbCrLf + " SELECT " & ColList & " FROM " & cnAdminDb & "..PURITEMTAGMETAL WHERE TAGSNO = '" & TagSno & "'"
            StrSql = vbCrLf + " INSERT INTO " & syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
            StrSql += vbCrLf + " SELECT '" & FromCostId & "','" & ToCostId & "','" & Qry.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
            Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
            'Cmd.CommandTimeout = 500
            Cmd.ExecuteNonQuery()
        End If

        If _HasHallmarkDet Then
            ColList = GetColumnNames(cnAdminDb, "CITEMTAGHALLMARK", )
            Qry = vbCrLf + " INSERT INTO " & cnAdminDb & "..CITEMTAGHALLMARK (" & ColList & ")"
            Qry += vbCrLf + " SELECT " & ColList & " FROM " & cnAdminDb & "..ITEMTAGHALLMARK WHERE TAGSNO = '" & TagSno & "'"
            StrSql = vbCrLf + " INSERT INTO " & syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
            StrSql += vbCrLf + " SELECT '" & FromCostId & "','" & ToCostId & "','" & Qry.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
            Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
            'Cmd.CommandTimeout = 500
            Cmd.ExecuteNonQuery()
        End If

        If _HasAddInfoTagDet Then
            ColList = GetColumnNames(cnAdminDb, "CADDINFOITEMTAG", )
            Qry = vbCrLf + " INSERT INTO " & cnAdminDb & "..CADDINFOITEMTAG (" & ColList & ")"
            Qry += vbCrLf + " SELECT " & ColList & " FROM " & cnAdminDb & "..ADDINFOITEMTAG WHERE TAGSNO = '" & TagSno & "'"
            StrSql = vbCrLf + " INSERT INTO " & syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
            StrSql += vbCrLf + " SELECT '" & FromCostId & "','" & ToCostId & "','" & Qry.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
            Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
            'Cmd.CommandTimeout = 500
            Cmd.ExecuteNonQuery()
        End If

        '' REMOVE TAG
        Qry = vbCrLf + " DELETE FROM " & cnAdminDb & "..ITEMTAG WHERE SNO = '" & TagSno & "'"
        If _HasStone Then Qry += vbCrLf + " DELETE FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = '" & TagSno & "'"
        If _HasMisc Then Qry += vbCrLf + " DELETE FROM " & cnAdminDb & "..ITEMTAGMISCCHAR WHERE TAGSNO = '" & TagSno & "'"
        If _HasMetal Then Qry += vbCrLf + " DELETE FROM " & cnAdminDb & "..ITEMTAGMETAL WHERE TAGSNO = '" & TagSno & "'"
        If _HasPurTag Then Qry += vbCrLf + " DELETE FROM " & cnAdminDb & "..PURITEMTAG WHERE TAGSNO = '" & TagSno & "'"
        If _HasPurStone Then Qry += vbCrLf + " DELETE FROM " & cnAdminDb & "..PURITEMTAGSTONE WHERE TAGSNO = '" & TagSno & "'"
        If _HasPurMisc Then Qry += vbCrLf + " DELETE FROM " & cnAdminDb & "..PURITEMTAGMISCCHAR WHERE TAGSNO = '" & TagSno & "'"
        If _HasPurMetal Then Qry += vbCrLf + " DELETE FROM " & cnAdminDb & "..PURITEMTAGMETAL WHERE TAGSNO = '" & TagSno & "'"
        If _HasHallmarkDet Then Qry += vbCrLf + " DELETE FROM " & cnAdminDb & "..ITEMTAGHALLMARK WHERE TAGSNO = '" & TagSno & "'"
        If _HasAddInfoTagDet Then Qry += vbCrLf + " DELETE FROM " & cnAdminDb & "..ADDINFOITEMTAG WHERE TAGSNO = '" & TagSno & "'"
        StrSql = vbCrLf + " INSERT INTO " & syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
        StrSql += vbCrLf + " SELECT '" & FromCostId & "','" & ToCostId & "','" & Qry.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
        'Cmd = New OleDbCommand(StrSql, _Cn, _Tran)

        'Cmd.ExecuteNonQuery()


        ' inserting tag to tag qry into sendsync 

        InsertDetails("ITEMTAG", "ITEMTAG", "SNO", TagSno, FromCostId, ToCostId, True)
        ' inserting tagstone to tagstone qry into sendsync 
        If _HasStone Then InsertDetails("ITEMTAGSTONE", "ITEMTAGSTONE", "TAGSNO", TagSno, FromCostId, ToCostId, True)
        ' inserting tagmiscchar to tagmiscchar qry into sendsync 
        If _HasMisc Then InsertDetails("ITEMTAGMISCCHAR", "ITEMTAGMISCCHAR", "TAGSNO", TagSno, FromCostId, ToCostId, True)
        ' inserting tagmetal to tagmetal qry into sendsync 
        If _HasMetal Then InsertDetails("ITEMTAGMETAL", "ITEMTAGMETAL", "TAGSNO", TagSno, FromCostId, ToCostId, True)

        ' inserting purtag to purtag qry into sendsync 
        If _HasPurTag Then InsertDetails("PURITEMTAG", "PURITEMTAG", "TAGSNO", TagSno, FromCostId, ToCostId, True)
        ' inserting purtagstone to purtagstone qry into sendsync 
        If _HasPurStone Then InsertDetails("PURITEMTAGSTONE", "PURITEMTAGSTONE", "TAGSNO", TagSno, FromCostId, ToCostId, True)
        ' inserting purtagmiscchar to purtagmiscchar qry into sendsync 
        If _HasPurMisc Then InsertDetails("PURITEMTAGMISCCHAR", "PURITEMTAGMISCCHAR", "TAGSNO", TagSno, FromCostId, ToCostId, True)
        ' inserting purtagmetal to purtagmetal qry into sendsync 
        If _HasPurMetal Then InsertDetails("PURITEMTAGMETAL", "PURITEMTAGMETAL", "TAGSNO", TagSno, FromCostId, ToCostId, True)
        ' inserting ITEMTAGHALLMARK to ITEMTAGHALLMARK qry into sendsync 
        If _HasHallmarkDet Then InsertDetails("ITEMTAGHALLMARK", "ITEMTAGHALLMARK", "TAGSNO", TagSno, FromCostId, ToCostId, True)
        ' inserting ADDINFOITEMTAG to ADDINFOITEMTAG qry into sendsync 
        If _HasAddInfoTagDet Then InsertDetails("ADDINFOITEMTAG", "ADDINFOITEMTAG", "TAGSNO", TagSno, FromCostId, ToCostId, True)

        StrSql = "SELECT TRANINVNO FROM " & cnAdminDb & "..ITEMTAG WHERE SNO='" & TagSno & "'"
        Dim Trefno_ As String = GetSqlValue(StrSql, _Cn, _Tran)

        Qry = vbCrLf + " EXEC " & cnAdminDb & "..SP_CCTRANSFER"
        Qry += vbCrLf + " @TAGSNO = '" & TagSno & "'"
        Qry += vbCrLf + " ,@COSTID = '" & FromCostId & "'"
        Qry += vbCrLf + " ,@ISSDATE = NULL"
        Qry += vbCrLf + " ,@ISSTIME = NULL"
        Qry += vbCrLf + " ,@VERSION = '" & GlobalVariables.VERSION & "'"
        Qry += vbCrLf + " ,@USERID = " & GlobalVariables.userId
        Qry += vbCrLf + " ,@TRFNO = '" & Trefno_ & "'"
        Qry += vbCrLf + " ,@FLAG = 'R'"
        StrSql = vbCrLf + " INSERT INTO " & syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
        StrSql += vbCrLf + " SELECT '" & FromCostId & "','" & ToCostId & "','" & Qry.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        Cmd.ExecuteNonQuery()

        Qry = vbCrLf + " INSERT INTO " & cnStockDb & "..TRANSIT_AUDIT(COSTID,REFNO,TAGSNO,FROMID,TOID,TRANDATE,STATUS,ISSREC,STOCKTYPE)"
        Qry += vbCrLf + " SELECT '" & cnCostId & "','" & Trefno_ & "','" & TagSno & "','" & FromId & "','" & ToId & "','" & TranDate.ToString("yyyy-MM-dd") & "','Y','R','T'"
        StrSql = vbCrLf + " INSERT INTO " & syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
        StrSql += vbCrLf + " SELECT '" & FromCostId & "','" & ToCostId & "','" & Qry.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        Cmd.ExecuteNonQuery()
    End Sub


    Private Sub SendResponse_NonTag(ByVal FromCostId As String, ByVal ToCostId As String, ByVal NewSno As String)

        InsertDetails("ITEMNONTAG", "ITEMNONTAG", "SNO", NewSno, FromCostId, ToCostId)
        ' inserting tagstone to tagstone qry into sendsync 
        If _HasStone Then InsertDetails("ITEMNONTAGSTONE", "ITEMNONTAGSTONE", "TAGSNO", NewSno, FromCostId, ToCostId)
        Dim Qry As String = Nothing
        Dim dtTemp As New DataTable
        Qry = vbCrLf + " INSERT INTO " & cnStockDb & "..TRANSIT_AUDIT(COSTID,REFNO,TAGSNO,FROMID,TOID,TRANDATE,STATUS,ISSREC,STOCKTYPE)"
        Qry += vbCrLf + " SELECT '" & cnCostId & "','" & _Trefno & "','" & NewSno & "','" & FromId & "','" & ToId & "','" & TranDate.ToString("yyyy-MM-dd") & "','Y','R','N'"
        StrSql = vbCrLf + " INSERT INTO " & syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
        StrSql += vbCrLf + " SELECT '" & FromCostId & "','" & ToCostId & "','" & Qry.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        '        Cmd.CommandTimeout = 500
        Cmd.ExecuteNonQuery()

        Qry = vbCrLf + " DELETE " & cnAdminDb & "..Pitemnontag where sno = '" & TagSno & "'"
        StrSql = vbCrLf + " INSERT INTO " & syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
        StrSql += vbCrLf + " SELECT '" & FromCostId & "','" & ToCostId & "','" & Qry.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        '        Cmd.CommandTimeout = 500
        Cmd.ExecuteNonQuery()


    End Sub

    Private Sub InsertDetailsTran(ByVal FromTableName As String, ByVal ToTableName As String, ByVal CondColName As String, ByVal CondColValue As String, ByVal FromCostId As String, ByVal ToCostId As String)
        Dim Qry As String = Nothing
        Dim dtTemp As New DataTable
        StrSql = " IF OBJECT_ID('" & cnStockDb & "..INS_" & FromTableName & "', 'U') IS NOT NULL DROP TABLE " & cnStockDb & "..INS_" & FromTableName & ""
        StrSql += vbCrLf + " SELECT * INTO " & cnStockDb & "..INS_" & FromTableName & " FROM " & cnStockDb & ".." & FromTableName & " WHERE " & CondColName & " = '" & CondColValue & "'"
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        Cmd.ExecuteNonQuery()
        StrSql = " UPDATE " & cnStockDb & "..INS_" & FromTableName & " SET COSTID = '" & FromCostId & "'"
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        Cmd.ExecuteNonQuery()
        StrSql = " EXEC " & cnStockDb & "..INSERTQRYGENERATOR_TABLE "
        StrSql += vbCrLf + " @DBNAME = '" & cnStockDb & "',@TABLENAME = 'INS_" & FromTableName & "',@MASK_TABLENAME = '" & ToTableName & "'"
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        DA = New OleDbDataAdapter(Cmd)
        dtTemp = New DataTable
        DA.Fill(dtTemp)
        Qry = ""
        For Each ro As DataRow In dtTemp.Rows
            Qry = ro.Item(0).ToString
            StrSql = " INSERT INTO " & syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
            StrSql += vbCrLf + " SELECT '" & FromCostId & "','" & ToCostId & "','" & Qry.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
            Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
            Cmd.ExecuteNonQuery()
        Next
        StrSql = " DROP TABLE " & cnStockDb & "..INS_" & FromTableName & ""
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        Cmd.ExecuteNonQuery()
    End Sub

    Private Sub InsertDetails(ByVal FromTableName As String, ByVal ToTableName As String, ByVal CondColName As String, ByVal CondColValue As String, ByVal FromCostId As String, ByVal ToCostId As String, Optional ByVal IsDelete As Boolean = False, Optional ByVal IsTrfdate As Boolean = False)
        Dim Qry As String = Nothing
        Dim dtTemp As New DataTable
        StrSql = " IF OBJECT_ID('" & cnAdminDb & "..INS_" & FromTableName & "', 'U') IS NOT NULL DROP TABLE " & cnAdminDb & "..INS_" & FromTableName & ""
        StrSql += vbCrLf + " SELECT * INTO " & cnAdminDb & "..INS_" & FromTableName & " FROM " & cnAdminDb & ".." & FromTableName & " WHERE " & CondColName & " = '" & CondColValue & "'"
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        Cmd.ExecuteNonQuery()
        StrSql = " UPDATE " & cnAdminDb & "..INS_" & FromTableName & " SET COSTID = '" & FromCostId & "'"
        If IsTrfdate Then StrSql += " ,RECDATE = '" & TranDate.ToString("yyyy-MM-dd") & "'"
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        Cmd.ExecuteNonQuery()
        StrSql = " EXEC " & cnAdminDb & "..INSERTQRYGENERATOR_TABLE "
        StrSql += vbCrLf + " @DBNAME = '" & cnAdminDb & "',@TABLENAME = 'INS_" & FromTableName & "',@MASK_TABLENAME = '" & ToTableName & "'"
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        DA = New OleDbDataAdapter(Cmd)
        dtTemp = New DataTable
        DA.Fill(dtTemp)
        Qry = ""

        If IsDelete Then
            Qry = " DELETE FROM " & cnAdminDb & ".." & FromTableName & " WHERE " & CondColName & " = '" & CondColValue & "'"
            Qry += vbCrLf
        End If
        For Each ro As DataRow In dtTemp.Rows
            Qry += ro.Item(0).ToString
            StrSql = " INSERT INTO " & syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
            StrSql += vbCrLf + " SELECT '" & FromCostId & "','" & ToCostId & "','" & Qry.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
            Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
            Cmd.ExecuteNonQuery()
            Qry = ""
        Next
        StrSql = " DROP TABLE " & cnAdminDb & "..INS_" & FromTableName & ""
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        Cmd.ExecuteNonQuery()
    End Sub


    Private Sub InsertDetails_Glb(ByVal FromTableName As String, ByVal ToTableName As String, ByVal CondColName As String, ByVal CondColValue As String, ByVal FromCostId As String, ByVal ToCostId As String,
    Optional ByVal IsDelete As Boolean = False, Optional ByVal IsTrfdate As Boolean = False,
    Optional ByVal MaxWastper As Double = 0, Optional ByVal MinWastper As Double = 0,
    Optional ByVal MaxMcGrm As Double = 0, Optional ByVal MinMcGrm As Double = 0)
        Dim Qry As String = Nothing
        Dim dtTemp As New DataTable

        StrSql = " insert INTO " & cnAdminDb & ".." & ToTableName & " select * FROM " & cnAdminDb & ".." & FromTableName & " WHERE " & CondColName & " = '" & CondColValue & "'"
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        Cmd.ExecuteNonQuery()

        StrSql = " UPDATE " & cnAdminDb & ".." & ToTableName & " SET COSTID = '" & FromCostId & "'"
        If ToTableName = "TITEMTAG" Then
            StrSql += " ,TCOSTID = '" & ToCostId & "' "
            If MaxWastper <> 0 Then StrSql += " ,MAXWASTPER = " & MaxWastper & " "
            If MinWastper <> 0 Then StrSql += " ,MINWASTPER = " & MinWastper & " "
            If MaxMcGrm <> 0 Then StrSql += " ,MAXMCGRM = " & MaxMcGrm & " "
            If MinMcGrm <> 0 Then StrSql += " ,MINMCGRM = " & MinMcGrm & " "
        End If
        StrSql += " WHERE " & CondColName & " = '" & CondColValue & "'"
        'If IsTrfdate Then StrSql += " ,RECDATE = '" & TranDate.ToString("yyyy-MM-dd") & "'"
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        Cmd.ExecuteNonQuery()

    End Sub



    Public Sub InsertNonTagIssue()
        Dim Qry As String = Nothing
        Dim dtTemp As New DataTable
        StrSql = vbCrLf + "   select DISTINCT "
        StrSql += vbCrLf + "   T.SNO TAG,TS.TAGSNO STONE"
        StrSql += vbCrLf + "   FROM " & cnAdminDb & "..ITEMNONTAG AS T"
        StrSql += vbCrLf + "   LEFT OUTER JOIN " & cnAdminDb & "..ITEMNONTAGSTONE AS TS ON TS.TAGSNO = T.SNO"
        StrSql += vbCrLf + "   WHERE T.SNO = '" & TagSno & "'"
        dtTemp = New DataTable
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        DA = New OleDbDataAdapter(Cmd)
        DA.Fill(dtTemp)
        If Not dtTemp.Rows.Count > 0 Then Exit Sub
        If dtTemp.Rows(0).Item("STONE").ToString <> "" Then _HasStone = True

        InsertDetails("ITEMNONTAG", "TITEMNONTAG", "SNO", TagSno, FromId, ToId)
        If _HasStone Then InsertDetails("ITEMNONTAGSTONE", "TITEMNONTAGSTONE", "TAGSNO", TagSno, FromId, ToId)
    End Sub

    Public Sub InsertNonTagReceipt()
        Dim Qry As String = Nothing
        Dim dtTemp As New DataTable
        StrSql = vbCrLf + "   select DISTINCT "
        StrSql += vbCrLf + "   T.SNO TAG,TS.TAGSNO STONE"
        StrSql += vbCrLf + "   FROM " & cnAdminDb & "..TITEMNONTAG AS T"
        StrSql += vbCrLf + "   LEFT OUTER JOIN " & cnAdminDb & "..TITEMNONTAGSTONE AS TS ON TS.TAGSNO = T.SNO"
        StrSql += vbCrLf + "   WHERE T.SNO = '" & TagSno & "'"
        dtTemp = New DataTable
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        DA = New OleDbDataAdapter(Cmd)
        DA.Fill(dtTemp)
        If Not dtTemp.Rows.Count > 0 Then Exit Sub
        If dtTemp.Rows(0).Item("STONE").ToString <> "" Then _HasStone = True

        Dim DefItemctrid As Integer = Val(GetAdmindbSoftValueNew("TAG_RECD_CTRID", _Cn, "", _Tran))
        If DefItemctrid <> 0 And objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE ISNULL(MAIN,'')='Y'", , , ) = cnCostId Then
            StrSql = " UPDATE " & cnAdminDb & "..TITEMNONTAG SET"
            StrSql += vbCrLf + " ITEMCTRID = " & DefItemctrid & ""
            StrSql += " WHERE SNO = '" & TagSno & "'"
            Cmd = New OleDbCommand(StrSql, _Cn, _Tran) : Cmd.ExecuteNonQuery()
        End If

        StrSql = " DECLARE @RETSNOVALUE VARCHAR(15)"
        StrSql += " EXEC " & cnStockDb & "..GET_ADMINSNO_TRAN"
        StrSql += " @COSTID = '" & cnCostId & "'"
        StrSql += " ,@CTLID = 'ITEMNONTAGCODE'"
        StrSql += " ,@CHECKTABLENAME = 'ITEMNONTAG'"
        StrSql += " ,@COMPANYID = '" & strCompanyId & "'"
        StrSql += " ,@RETVALUE = @RETSNOVALUE OUTPUT"
        StrSql += " SELECT @RETSNOVALUE"
        Dim dtNt As New DataTable
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        DA = New OleDbDataAdapter(Cmd)
        DA.Fill(dtNt)
        Dim NewSno As String = ""
        'For MEJ 
        'If dtNt.Rows.Count > 0 Then
        If dtNt.Rows.Count > 0 Then 'And CENTR_DB_GLB = False ' commented on 19-02-2021
            NewSno = dtNt.Rows(0).Item(0).ToString
            StrSql = " UPDATE " & cnAdminDb & "..TITEMNONTAG SET"
            StrSql += vbCrLf + " SNO = '" & NewSno & "'"
            StrSql += " WHERE SNO = '" & TagSno & "'"
            Cmd = New OleDbCommand(StrSql, _Cn, _Tran) : Cmd.ExecuteNonQuery()
            StrSql = " UPDATE " & cnAdminDb & "..TITEMNONTAGSTONE SET"
            StrSql += vbCrLf + " TAGSNO = '" & NewSno & "'"
            StrSql += " WHERE TAGSNO = '" & TagSno & "'"
            Cmd = New OleDbCommand(StrSql, _Cn, _Tran) : Cmd.ExecuteNonQuery()
        End If
        ''if exsts update corrsponding cost and rec date
        StrSql += vbCrLf + " UPDATE " & cnAdminDb & "..TITEMNONTAG SET TCOSTID = '" & FromId & "',REFDATE = NULL,COSTID = '" & ToId & "',RECDATE = '" & TranDate.ToString("yyyy-MM-dd") & "' WHERE SNO = '" & NewSno & "'"
        'StrSql += vbCrLf + " UPDATE " & cnAdminDb & "..TITEMNONTAG SET REFDATE = NULL,RECDATE = '" & TranDate.ToString("yyyy-MM-dd") & "' WHERE SNO = '" & TagSno & "'"
        If _HasStone Then StrSql += vbCrLf + " UPDATE " & cnAdminDb & "..TITEMNONTAGSTONE SET COSTID = '" & ToId & "',RECDATE = '" & TranDate.ToString("yyyy-MM-dd") & "' WHERE TAGSNO = '" & NewSno & "'"

        ''copy to tag
        StrSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..ITEMNONTAG (" & GetColumnNames(cnAdminDb, "ITEMNONTAG", ) & ")"
        StrSql += vbCrLf + " SELECT " & GetColumnNames(cnAdminDb, "ITEMNONTAG", ) & " FROM " & cnAdminDb & "..TITEMNONTAG WHERE SNO = '" & NewSno & "'"
        If _HasStone Then StrSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..ITEMNONTAGSTONE (" & GetColumnNames(cnAdminDb, "ITEMNONTAGSTONE", ) & ")"
        If _HasStone Then StrSql += vbCrLf + " SELECT " & GetColumnNames(cnAdminDb, "ITEMNONTAGSTONE", ) & " FROM " & cnAdminDb & "..TITEMNONTAGSTONE WHERE TAGSNO = '" & NewSno & "'"
        ''delete from ttag
        If CENTR_DB_GLB Then
            StrSql += vbCrLf + " DELETE FROM " & cnAdminDb & "..TITEMNONTAG WHERE SNO = '" & TagSno & "'"
            If _HasStone Then StrSql += vbCrLf + " DELETE FROM " & cnAdminDb & "..TITEMNONTAGSTONE WHERE TAGSNO = '" & TagSno & "'"
        Else
            StrSql += vbCrLf + " DELETE FROM " & cnAdminDb & "..TITEMNONTAG WHERE SNO = '" & NewSno & "'"
            If _HasStone Then StrSql += vbCrLf + " DELETE FROM " & cnAdminDb & "..TITEMNONTAGSTONE WHERE TAGSNO = '" & NewSno & "'"
        End If
        StrSql += vbCrLf + " DELETE FROM " & cnAdminDb & "..TITEMNONTAG WHERE SNO = '" & NewSno & "'"
        If _HasStone Then StrSql += vbCrLf + " DELETE FROM " & cnAdminDb & "..TITEMNONTAGSTONE WHERE TAGSNO = '" & NewSno & "'"
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran) : Cmd.ExecuteNonQuery()

        StrSql = "INSERT INTO " & syncdb & "..TRANS_STATUS (FROMID,TAGSNO,STATUS,TRANNO,TRANDATE,STOCKMODE,MOVETYPE)"
        StrSql += " SELECT '" & FromId & "','" & NewSno & "','T',0,'" & TranDate.ToString("yyyy-MM-dd") & "','N','I'"
        Cmd = New OleDbCommand(StrSql, _Cn, tran)
        Cmd.ExecuteNonQuery()

        If objGPack.GetSqlValue("select CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SYNC-TO'", , , ) <> "" Then
            StrSql = "SELECT TOP 1 COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE MAIN = 'Y'"
            Dim mfromCostid As String = BrighttechPack.GlobalMethods.GetSqlValue(_Cn, StrSql, , " ", _Tran)
            SendResponse_NonTag(cnCostId, mfromCostid, NewSno)
        End If

    End Sub

    Public Function InsertTagIssue(ByVal Manual As Boolean,
    Optional ByVal MaxWastper As Double = 0, Optional ByVal MinWastper As Double = 0,
    Optional ByVal MaxMcGrm As Double = 0, Optional ByVal MinMcGrm As Double = 0
    ) As Boolean

        StrSql = "SELECT '" + _Trefno + ":" + FromId + "" + ToId + ":'+CONVERT(VARCHAR(100),NEWID())ID"
        Dim _NewId As String = ""
        _NewId = GetSqlValue(StrSql, _Cn, _Tran)


        Dim Qry As String = Nothing
        Dim dtTemp As New DataTable
        Dim ReplRecdate As Boolean = IIf(GetAdmindbSoftValueNew("STKTRAN_REC_DATE", _Cn, "N", _Tran) = "Y", True, False)
        StrSql = vbCrLf + "   select DISTINCT "
        StrSql += vbCrLf + "   T.SNO TAG,TS.TAGSNO STONE,TMI.TAGSNO MISC,TME.TAGSNO METAL,THM.TAGSNO HALLMARK,TAD.TAGSNO ADDINFOTAG"
        StrSql += vbCrLf + "   ,PT.TAGSNO PTAG,PTS.TAGSNO PSTONE,PTMI.TAGSNO PMISC,PTME.TAGSNO PMETAL"
        StrSql += vbCrLf + "   FROM " & cnAdminDb & "..ITEMTAG AS T"
        StrSql += vbCrLf + "   LEFT OUTER JOIN " & cnAdminDb & "..ITEMTAGSTONE AS TS ON TS.TAGSNO = T.SNO"
        StrSql += vbCrLf + "   LEFT OUTER JOIN " & cnAdminDb & "..ITEMTAGMISCCHAR AS TMI ON TMI.TAGSNO = T.SNO"
        StrSql += vbCrLf + "   LEFT OUTER JOIN " & cnAdminDb & "..ITEMTAGMETAL AS TME ON TME.TAGSNO = T.SNO"
        StrSql += vbCrLf + "   LEFT OUTER JOIN " & cnAdminDb & "..ITEMTAGHALLMARK AS THM ON THM.TAGSNO = T.SNO"
        StrSql += vbCrLf + "   LEFT OUTER JOIN " & cnAdminDb & "..ADDINFOITEMTAG AS TAD ON TAD.TAGSNO = T.SNO"
        StrSql += vbCrLf + "   LEFT OUTER JOIN " & cnAdminDb & "..PURITEMTAG AS PT ON PT.TAGSNO = T.SNO"
        StrSql += vbCrLf + "   LEFT OUTER JOIN " & cnAdminDb & "..PURITEMTAGSTONE AS PTS ON PTS.TAGSNO = T.SNO"
        StrSql += vbCrLf + "   LEFT OUTER JOIN " & cnAdminDb & "..PURITEMTAGMISCCHAR AS PTMI ON PTMI.TAGSNO = T.SNO"
        StrSql += vbCrLf + "   LEFT OUTER JOIN " & cnAdminDb & "..PURITEMTAGMETAL AS PTME ON PTME.TAGSNO = T.SNO"
        StrSql += vbCrLf + "   WHERE T.SNO = '" & TagSno & "'"
        dtTemp = New DataTable
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        DA = New OleDbDataAdapter(Cmd)
        DA.Fill(dtTemp)
        If Not dtTemp.Rows.Count > 0 Then Return False
        If dtTemp.Rows(0).Item("STONE").ToString <> "" Then _HasStone = True
        If dtTemp.Rows(0).Item("MISC").ToString <> "" Then _HasMisc = True
        If dtTemp.Rows(0).Item("METAL").ToString <> "" Then _HasMetal = True
        If dtTemp.Rows(0).Item("PTAG").ToString <> "" Then _HasPurTag = True
        If dtTemp.Rows(0).Item("PSTONE").ToString <> "" Then _HasPurStone = True
        If dtTemp.Rows(0).Item("PMISC").ToString <> "" Then _HasPurMisc = True
        If dtTemp.Rows(0).Item("PMETAL").ToString <> "" Then _HasPurMetal = True
        If dtTemp.Rows(0).Item("HALLMARK").ToString <> "" Then _HasHallmarkDet = True
        If dtTemp.Rows(0).Item("ADDINFOTAG").ToString <> "" Then _HasAddInfoTagDet = True

        StrSql = "UPDATE A SET TRANINVNO = '" & _Trefno & "',TRANKEY='" & _NewId & "' FROM " & cnAdminDb & "..ITEMTAG AS A WHERE A.SNO = '" & TagSno & "'"
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        Cmd.ExecuteNonQuery()
        Dim ColList As String
        ColList = GetColumnNames(cnAdminDb, "PITEMTAG", )
        StrSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..PITEMTAG(" & ColList & ")"
        StrSql += vbCrLf + " SELECT DISTINCT " & ColList & " FROM " & cnAdminDb & "..ITEMTAG WHERE SNO = '" & TagSno & "'"
        StrSql += vbCrLf + " UPDATE " & cnAdminDb & "..PITEMTAG SET TCOSTID ='" & ToId & "' WHERE SNO = '" & TagSno & "'"
        ColList = GetColumnNames(cnAdminDb, "PITEMTAGSTONE", )
        If _HasStone Then
            StrSql += vbCrLf + " INSERT INTO " & cnAdminDb & "..PITEMTAGSTONE(" & ColList & ")"
            StrSql += vbCrLf + " SELECT DISTINCT " & ColList & " FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = '" & TagSno & "'"
            'StrSql += vbCrLf + " UPDATE " & cnAdminDb & "..PITEMTAGSTONE SET COSTID = '" & ToId & "' WHERE TAGSNO = '" & TagSno & "'"
        End If

        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
        If CENTR_DB_GLB Then
            InsertDetails_Glb("ITEMTAG", "TITEMTAG", "SNO", TagSno, FromId, ToId, Nothing, ReplRecdate, MaxWastper, MinWastper, MaxMcGrm, MinMcGrm)
            If _HasStone Then InsertDetails_Glb("ITEMTAGSTONE", "TITEMTAGSTONE", "TAGSNO", TagSno, FromId, ToId, Nothing, ReplRecdate)
            If _HasMisc Then InsertDetails_Glb("ITEMTAGMISCCHAR", "TITEMTAGMISCCHAR", "TAGSNO", TagSno, FromId, ToId)
            If _HasMetal Then InsertDetails_Glb("ITEMTAGMETAL", "TITEMTAGMETAL", "TAGSNO", TagSno, FromId, ToId)
            If _HasHallmarkDet Then InsertDetails_Glb("ITEMTAGHALLMARK", "TITEMTAGHALLMARK", "TAGSNO", TagSno, FromId, ToId)
            If _HasAddInfoTagDet Then InsertDetails_Glb("ADDINFOITEMTAG", "TADDINFOITEMTAG", "TAGSNO", TagSno, FromId, ToId)
            If PURVALTRF Then
                If _HasPurTag Then InsertDetails_Glb("PURITEMTAG", "TPURITEMTAG", "TAGSNO", TagSno, FromId, ToId)
                If _HasPurStone Then InsertDetails_Glb("PURITEMTAGSTONE", "TPURITEMTAGSTONE", "TAGSNO", TagSno, FromId, ToId)
                If _HasPurMisc Then InsertDetails_Glb("PURITEMTAGMISCCHAR", "TPURITEMTAGMISCCHAR", "TAGSNO", TagSno, FromId, ToId)
                If _HasPurMetal Then InsertDetails_Glb("PURITEMTAGMETAL", "TPURITEMTAGMETAL", "TAGSNO", TagSno, FromId, ToId)
            End If
            If Manual Then
                StrSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET ISSDATE = '" & TranDate.ToString("yyyy-MM-dd") & "',ISSPCS = PCS,ISSWT = GRSWT,TOFLAG = 'TR',TCOSTID = '" & ToId & "',TRANKEY='" & _NewId & "' WHERE SNO = '" & TagSno & "'"
                Cmd = New OleDbCommand(StrSql, _Cn, _Tran) : Cmd.ExecuteNonQuery()
            End If
        Else
            ' inserting tag to ttag qry into sendsync 
            InsertDetails("ITEMTAG", "TITEMTAG", "SNO", TagSno, FromId, ToId, Nothing, ReplRecdate)
            ' inserting tagstone to ttagstone qry into sendsync 
            If _HasStone Then InsertDetails("ITEMTAGSTONE", "TITEMTAGSTONE", "TAGSNO", TagSno, FromId, ToId, Nothing, ReplRecdate)
            ' inserting tagmiscchar to ttagmiscchar qry into sendsync 
            If _HasMisc Then InsertDetails("ITEMTAGMISCCHAR", "TITEMTAGMISCCHAR", "TAGSNO", TagSno, FromId, ToId)
            ' inserting tagmetal to ttagmetal qry into sendsync 
            If _HasMetal Then InsertDetails("ITEMTAGMETAL", "TITEMTAGMETAL", "TAGSNO", TagSno, FromId, ToId)
            If _HasHallmarkDet Then InsertDetails("ITEMTAGHALLMARK", "TITEMTAGHALLMARK", "TAGSNO", TagSno, FromId, ToId)
            If _HasAddInfoTagDet Then InsertDetails("ADDINFOITEMTAG", "TADDINFOITEMTAG", "TAGSNO", TagSno, FromId, ToId)
            If PURVALTRF Then
                ' inserting purtag to purttag qry into sendsync 
                If _HasPurTag Then InsertDetails("PURITEMTAG", "TPURITEMTAG", "TAGSNO", TagSno, FromId, ToId)
                ' inserting purtagstone to purttagstone qry into sendsync 
                If _HasPurStone Then InsertDetails("PURITEMTAGSTONE", "TPURITEMTAGSTONE", "TAGSNO", TagSno, FromId, ToId)
                ' inserting purtagmiscchar to purttagmiscchar qry into sendsync 
                If _HasPurMisc Then InsertDetails("PURITEMTAGMISCCHAR", "TPURITEMTAGMISCCHAR", "TAGSNO", TagSno, FromId, ToId)
                ' inserting purtagmetal to purttagmetal qry into sendsync 
                If _HasPurMetal Then InsertDetails("PURITEMTAGMETAL", "TPURITEMTAGMETAL", "TAGSNO", TagSno, FromId, ToId)
                'End If
            End If
            If Manual Then
                StrSql = " UPDATE " & cnAdminDb & "..ITEMTAG SET ISSDATE = '" & TranDate.ToString("yyyy-MM-dd") & "',ISSPCS = PCS,ISSWT = GRSWT,TOFLAG = 'TR',TCOSTID = '" & ToId & "',TRANKEY='" & _NewId & "' WHERE SNO = '" & TagSno & "' AND COSTID = '" & FromId & "'"
                ExecQuery(SyncMode.Transaction, StrSql, _Cn, _Tran, FromId)
                'Cmd = New OleDbCommand(StrSql, _Cn, _Tran) : Cmd.ExecuteNonQuery()
            End If

            'Sending Response to HO when loc-loc transfer
            StrSql = vbCrLf + " SELECT TOP 1 COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE"
            StrSql += vbCrLf + " WHERE 1 = (SELECT COUNT(*) FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE COSTID = '" & FromId & "' AND MAIN = 'Y')"
            StrSql += vbCrLf + " OR 1 = (SELECT COUNT(*) FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE COSTID = '" & ToId & "' AND MAIN = 'Y')"
            Dim tofindmaincostid As String = "'" & FromId & "','" & ToId & "'"
            StrSql = "SELECT COUNT(*) FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE COSTID in(" & tofindmaincostid & ") AND MAIN = 'Y'"
            If BrighttechPack.GlobalMethods.GetSqlValue(_Cn, StrSql, , "0", _Tran) = "0" Then
                StrSql = vbCrLf + " INSERT INTO " & cnStockDb & "..TRANSIT_AUDIT(COSTID,REFNO,TAGSNO,FROMID,TOID,TRANDATE,STATUS,ISSREC,STOCKTYPE)"
                StrSql += vbCrLf + " SELECT '" & cnCostId & "','" & _Trefno & "','" & TagSno & "','" & FromId & "','" & ToId & "','" & TranDate.ToString("yyyy-MM-dd") & "','Y','I','T'"
                Qry = vbCrLf + " INSERT INTO " & syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)"
                Qry += vbCrLf + " SELECT '" & FromId & "','" & MainId & "','" & StrSql.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
                Cmd = New OleDbCommand(Qry, _Cn, _Tran) : Cmd.ExecuteNonQuery()

                InsertDetails("ITEMTAG", "PITEMTAG", "SNO", TagSno, FromId, MainId, Nothing, ReplRecdate)
                If _HasStone Then InsertDetails("ITEMTAGSTONE", "PITEMTAGSTONE", "TAGSNO", TagSno, FromId, MainId, Nothing, ReplRecdate)
            Else
                'FOR SVV ON 28-11-2020
                If SPECIFICFORMAT.ToString = "1" Then
                    StrSql = vbCrLf + " INSERT INTO " & cnStockDb & "..TRANSIT_AUDIT(COSTID,REFNO,TAGSNO,FROMID,TOID,TRANDATE,STATUS,ISSREC,STOCKTYPE)"
                    StrSql += vbCrLf + " SELECT '" & cnCostId & "','" & _Trefno & "','" & TagSno & "','" & FromId & "','" & ToId & "','" & TranDate.ToString("yyyy-MM-dd") & "','Y','I','T'"
                    If FromId <> MainId Then
                        Qry = vbCrLf + " INSERT INTO " & syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)"
                        Qry += vbCrLf + " SELECT '" & FromId & "','" & MainId & "','" & StrSql.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
                        Cmd = New OleDbCommand(Qry, _Cn, _Tran) : Cmd.ExecuteNonQuery()
                    End If
                End If
                'END FOR SVV ON 28-11-2020

                StrSql = " SELECT COUNT(*) FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE COSTID ='" & ToId & "' AND MAIN = 'Y'"
                Dim mxtoid As String = ToId
                Dim mxfromid As String = FromId
                If BrighttechPack.GlobalMethods.GetSqlValue(_Cn, StrSql, , "0", _Tran) = "1" Then mxtoid = FromId : mxfromid = ToId
                StrSql = " UPDATE " & cnAdminDb & "..PITEMTAG SET COSTID ='" & mxtoid & "',TCOSTID='" & mxfromid & "' WHERE SNO = '" & TagSno & "'"
                StrSql += vbCrLf + " UPDATE " & cnAdminDb & "..PITEMTAGSTONE SET COSTID = '" & mxtoid & "' WHERE TAGSNO = '" & TagSno & "'"
                Cmd = New OleDbCommand(StrSql, _Cn, _Tran) : Cmd.ExecuteNonQuery()
            End If
        End If
        Return True

    End Function

    Public Function InsertTagGift(ByVal Manual As Boolean) As Boolean
        Dim Qry As String = Nothing
        Dim dtTemp As New DataTable
        StrSql = vbCrLf + "   select DISTINCT "
        StrSql += vbCrLf + "   T.SNO TAG "
        StrSql += vbCrLf + "   FROM " & cnStockDb & "..GVTRAN AS T"
        StrSql += vbCrLf + "   WHERE T.SNO = '" & TagSno & "'"
        dtTemp = New DataTable
        Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        DA = New OleDbDataAdapter(Cmd)
        DA.Fill(dtTemp)
        If Not dtTemp.Rows.Count > 0 Then Return False

        ' inserting tag to ttag qry into sendsync 
        InsertDetailsTran("GVTRAN", "GVTRAN", "SNO", TagSno, FromId, ToId)


        'Sending Response to HO when loc-loc transfer
        StrSql = vbCrLf + " SELECT TOP 1 COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE"
        StrSql += vbCrLf + " WHERE 1 = (SELECT COUNT(*) FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE COSTID = '" & FromId & "' AND MAIN = 'Y')"
        StrSql += vbCrLf + " OR 1 = (SELECT COUNT(*) FROM " & cnAdminDb & "..SYNCCOSTCENTRE WHERE COSTID = '" & ToId & "' AND MAIN = 'Y')"
        If BrighttechPack.GlobalMethods.GetSqlValue(_Cn, StrSql, , "-1", _Tran) = "-1" Then
            Qry = vbCrLf + " INSERT INTO " & syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)"
            Qry += vbCrLf + " SELECT '" & FromId & "','" & MainId & "','" & StrSql.Replace("'", "''") & "','N' AS STATUS,'' UPDFILE"
            Cmd = New OleDbCommand(Qry, _Cn, _Tran)
            ' Cmd.CommandTimeout = 500
            Cmd.ExecuteNonQuery()
        End If
        Return True
    End Function

End Class
